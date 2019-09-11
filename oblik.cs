using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.IO;



namespace Obliks
{
    public class Oblik
    {

        //Локальные переменные
        private readonly int _port;             //порт счетчика
        private readonly int _addr;             //адрес счетчика
        private readonly int _baudrate;         //скорость работы порта, 9600 бод - по умолчанию
        private int _timeout, _repeats;         //таймаут и повторы
        private byte[] _passwd;                 //пароль
        private bool _isError;                  //Наличие ошибки
        private string _error_txt = "";         //текст ошибки
        private byte _user;                     //Пользователь от 0 до 3 (3 - максимальные привелегии, 0 - минимальные)
        private readonly object SerialIncoming;          //Монитор таймаута чтения порта

        //Интерфейс класса
        //Структура строки суточного графика
        public struct DayGraphRow
        {
            uint time;          //время записи
            float act_en_p;     //активная энергия "+" за период сохранения 
            float act_en_n;     //активная энергия "-" за период сохранения
            float rea_en_p;     //реактивная энергия "+" за период сохранения
            float rea_en_n;     //реактивная энергия "-" за период сохранения
            ushort[] channel;   //Количество импульсов по каналам
        }
        //Структура ответа счетчика
        public int L1Result { get; set; }                //Результат фрейма L1
        public string L1ResultMsg { get; set; }          //Результат фрейма L1, расшифровка
        public int L1Lenght { get; set; }                //Количетво байт в полях "Длина", "L2Data", "Результат" 
        public int L1Sum { get; set; }                   //Контрольная сумма
        public int L2Result { get; set; }                //Результат запроса L2
        public string L2ResultMsg { get; set; }          //Результат запроса L2, расшифровка
        public int L2Lenght { get; set; }                //Количество данных, успешно обработанных операцией
        public byte[] L2Data { get; set; }               //Данные L2
        public int Repeats
        {
            set => _repeats = value;
            get => _repeats;
        }           //Количество повторов передачи
        public int Timeout
        {
            set => _timeout = value;
            get => _timeout;
        }           //Таймаут соединения
        public bool IsError
        {
            set => _isError = value;
            get => _isError;
        }          //Индикатор наличия ошибки
        public string ErrorMsg
        {
            get => _error_txt;
        }       //Последнее сообщение об ошибке
        public string Password
        {
            set => _passwd = Encoding.Default.GetBytes(value);
            get => Encoding.Default.GetString(_passwd);
        }       //Пароль счетчика
        public int User
        {
            set => _user = (byte)value;
            get => _user;
        }              //Пользователь
        public Oblik(int port, int baudrate, int addr, int timeout, int repeats, string password)
        {
            _port = port;
            _addr = addr;
            _timeout = timeout;
            _repeats = repeats;
            _baudrate = baudrate;
            _passwd = new byte[8];
            _isError = false;
            _error_txt = "";
            if (password == "")
            {
                for (int i = 0; i < 8; i++) { _passwd[i] = 0; }
            }
            else
            {
                _passwd = Encoding.Default.GetBytes(password);
            }
            _user = 2;
            SerialIncoming = new object();
        }
        public Oblik(int port, int addr, int timeout, int repeats) : this(port, 9600, addr, timeout, repeats, "") { }
        public Oblik(int port, int addr) : this(port, 9600, addr, 500, 2, "") { }

        //Реализация

        /*
        Доступ к данным в сегменте счетчика, возвращает количество прочитанных байт ответа счетчика или -1 в случае ошибки
        data - данные для записи в сегмент, если запрос на чтение, то null
        len - количество байт для записи / чтения
        answ - полученные данные со счетчика (не обработанные)
        AccType: 0 - чтение, 1 - запись 
        */
        public void SegmentAccsess(byte segment, UInt16 offset, byte len, byte[] data, byte AccType)
        {
            byte[] _l1;                                                     //Посылка 1 уровня
            byte[] _l2;                                                     //Посылка 2 уровня
            if (AccType != 0) { AccType = 1; }                              //Все, что больше 0 - команда на запись
            
            //Формируем запрос L2
            _l2 = new byte[5 + (len + 8) * AccType];                        //5 байт заголовка + 8 байт пароля + данные 
            _l2[0] = (byte)((segment & 127) + AccType * 128);               //(биты 0 - 6 - номер сегмента, бит 7 = 1 - операция записи)
            _l2[1] = _user;                                                 //Указываем пользователя
            _l2[2] = (byte)(offset >> 8);                                   //Старший байт смещения
            _l2[3] = (byte)(offset & 0xff);                                 //Младший байт смещения
            _l2[4] = len;                                                   //Размер считываемых данных

            //Если команда - на запись в сегмент
            if (AccType == 1)
            {
                Array.Copy(data, 0, _l2, 5, len);                               //Копируем данные в L2
                Array.Copy(_passwd, 0, _l2, len + 5, 8);                        //Копируем пароль в L2
                Encode(ref _l2);                                                //Шифруем данные и пароль L2
            }

            //Формируем фрейм L1
            _l1 = new byte[5 + _l2.Length];
            _l1[0] = 0xA5;                              //Заголовок пакета
            _l1[1] = 0x5A;                              //Заголовок пакета
            _l1[2] = (byte)(_addr & 0xff);              //Адрес счетчика
            _l1[3] = (byte)(3 + _l2.Length);            //Длина пакета L1 без ключей
            Array.Copy(_l2, 0, _l1, 4, _l2.Length);     //Вставляем запрос L2 в пакет L1

            //Вычисление контрольной суммы, побайтовое XOR, от поля "Адрес" до поля "L2"
            _l1[_l1.Length - 1] = 0;
            for (int i = 2; i < (_l1.Length - 1); i++)
            {
                _l1[_l1.Length - 1] ^= (byte)_l1[i];
            }

            //Обмен данными со счетчиком
            byte[] answer = new byte[0];
            OblikQuery(_l1, ref answer);

            //Заполняем структуру ответа счетчика
            if (!_isError)
            {
                AnswerParser(answer);
            }

        }

        //Получить количество записей суточного графика
        public int GetDayGraphRecs()
        {
            SegmentAccsess(44, 0, 2, null, 0);
            //Порядок байт в счетчике - обратный по отношению к пк, переворачиваем
            if (!_isError)
            {
                return (int)(L2Data[0] + (int)(L2Data[1] << 8));
            }
            else return -1;
        }

        //Стирание суточного графика
        public void CleanDayGraph()
        {
            byte segment = 88;
            ushort offset = 0;
            byte[] cmd = new byte[2];
            cmd[0] = (byte)~(_addr);
            cmd[1] = (byte)_addr;
            SegmentAccsess(segment, offset, (byte)cmd.Length, cmd, 1);
        }

        //Установка текущего времени в счетчике
        public void SetCurrentTime()
        {
            UInt32 _ctime;  //Время по стандарту t_time
            DateTime _curtime, _btime;
            byte[] _tbuf = new byte[4]; //Буфер для передачи счетчику
            _btime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);       //Базовая точка времени 01.01.1970 00:00 GMT
            _curtime = System.DateTime.Now.ToUniversalTime();                   //Текущее время
            _ctime = (UInt32)(_curtime - _btime).TotalSeconds + 2;              //2 секунды на вычисление, отправку и т.д.
            _tbuf[0] = (byte)((_ctime >> 24) & 0xff);
            _tbuf[1] = (byte)((_ctime >> 16) & 0xff);
            _tbuf[2] = (byte)((_ctime >> 8) & 0xff);
            _tbuf[3] = (byte)(_ctime & 0xff);
            SegmentAccsess(65, 0, (byte)_tbuf.Length, _tbuf, 1);
        }

        //Получение суточного графика: lines - количество строк, offset - смещение (в строках)
        DayGraphRow[] GetDayGraph(uint lines, uint offset)
        {
            const byte segment = 45;                            //Сегмент суточного графика
            const uint LineLen = 28;                            //28 байт на 1 строку данных по протоколу счетчика
            const uint MaxReqLines = 8;                         //Максимальное количество строк в запросе
            const byte MaxBytes = (byte)(LineLen * MaxReqLines);        //Максимальный размер запроса
            byte bytestoread;                                    //Байт в запросе
            DayGraphRow[] answer = null;
            byte[] _buf;                                        //Буфер
            uint TotalLines = (uint)GetDayGraphRecs();          //Количество строк суточного графика фактически в счетчике
            if (IsError) { return answer; }                     //Возврат null в случае ошибки
            if (TotalLines == 0) { return answer; }             //Если нет записей, нечего запрашивать
            if ((lines + offset) > TotalLines)                  //Если запрос выходит за диапазон, запросить только последнюю строку
            {
                lines = 1;
                offset = TotalLines - 1;
            }
            uint OffsetBytes = offset * LineLen;
            uint BytesReq = (lines - offset) * LineLen;                 //Запрошено байт
            _buf = new byte[BytesReq];
            ushort currofs = 0;                                //Текущий сдвиг
            ushort maxoffs = (ushort)(OffsetBytes + (lines - 1) * LineLen); //Максимальный сдвиг для чтения последней строки
            while (currofs <= maxoffs)
            {
                if (((BytesReq - currofs) / MaxBytes) >0)
                {
                    bytestoread = MaxBytes;
                }
                else
                {
                    bytestoread = (byte)((BytesReq - currofs) % MaxBytes);
                }
                SegmentAccsess(segment, currofs, bytestoread, null, 0);
                if (IsError) { break; }
                Array.Resize(ref _buf, (int)(currofs + LineLen));
                Array.Copy(L2Data, 0, _buf, currofs, L2Data.Length);
                currofs += MaxBytes;
            }

        }

        //Парсер ошибок L1
        private string ParseL1error(int error)
        {
            string res;
            switch (error)
            {
                case 1:
                    res = "Успешное выполнение запроса";
                    break;
                case 0xff:
                    res = "Ошибка контрольной суммы";
                    break;
                case 0xfe:
                    res = "Переполнение входного буфера счетчика";
                    break;
                default:
                    res = "Неизвестная ошибка";
                    break;
            }
            return res;
        }

        //Парсер ошибок L2
        private string ParseL2error(int error)
        {
            string res;
            switch (error)
            {
                case 0:
                    res = "Успешное выполнение операции";
                    break;
                case 0xff:
                    res = "Некорректный запрос (содержит менее 5 байт)";
                    break;
                case 0xfe:
                    res = "Неправильный идентификатор сегмента";
                    break;
                case 0xfd:
                    res = "Некорректная операция (Попытка записи в сегмент чтения и наоборот)";
                    break;
                case 0xfc:
                    res = "Неправильно задан уровень пользователя";
                    break;
                case 0xfb:
                    res = "Нет права доступа к данным";
                    break;
                case 0xfa:
                    res = "Неправильно задано смещение";
                    break;
                case 0xf9:
                    res = "Неправильный запрос на запись (несоответствие запрашиваемой и действительной длины данных)";
                    break;
                case 0xf8:
                    res = "Длина данных задана равной 0";
                    break;
                case 0xf7:
                    res = "Неправильный пароль";
                    break;
                case 0xf6:
                    res = "Неправильно задана команда стирания графиков";
                    break;
                case 0xf5:
                    res = "Запрещена смена пароля";
                    break;
                default:
                    res = "Неизвестная ошибка";
                    break;
            }
            return res;
        }

        //Отправка запроса и получение данных Query - запрос, Answer - ответ
        private void OblikQuery(byte[] Query, ref byte[] Answer)
        {
            _isError = false;
            _error_txt = "";
            byte[] _rbuf = new byte[0];                   //Буфер для чтения
            //Параметризация и открытие порта
            using (SerialPort com = new SerialPort
            {
                PortName = "COM" + _port,
                BaudRate = _baudrate,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                ReadTimeout = _timeout,
                WriteTimeout = _timeout,
                DtrEnable = false,
                RtsEnable = false,
                Handshake = Handshake.None
            })
            {
                // Событие чтения данных из порта
                void DataReciever(object s, SerialDataReceivedEventArgs ea)
                {
                    Array.Resize(ref _rbuf, com.BytesToRead);
                    com.Read(_rbuf, 0, _rbuf.Length);
                    lock (SerialIncoming)
                    {
                        Monitor.Pulse(SerialIncoming);
                    }
                }
                try
                {
                    if (com.IsOpen) { com.Close(); }    //закрыть ранее открытый порт
                    com.Open();
                    //Отправка данных
                    com.DiscardOutBuffer();                                                                 //очистка буфера передачи
                    com.DataReceived += new SerialDataReceivedEventHandler(DataReciever);                   //событие чтения из порта
                    com.Write(Query, 0, Query.Length);                                                      //отправка буфера записи
                    com.DiscardInBuffer();                                                                  //очистка буфера приема
                    //Получение ответа
                    int r = _repeats;
                    while (r > 0)   //Повтор при ошибке
                    {
                        lock (SerialIncoming)
                        {
                            if (!Monitor.Wait(SerialIncoming, _timeout))
                            {
                                //Если таймаут
                                _isError = true;
                                _error_txt = "Timeout";
                                r--;
                            }
                            else
                            {
                                r = 0;
                                _isError = false;
                                _error_txt = "";
                            }
                        }
                    }
                    com.Close();        //Закрыть порт
                    if (!IsError)
                    {
                        Array.Resize(ref Answer, _rbuf.Length);
                        Array.Copy(_rbuf, 0, Answer, 0, _rbuf.Length);
                    }
                }
                catch (Exception e)
                {
                    _isError = true;
                    _error_txt = e.Message;
                }
                finally
                {
                    Query = null;
                }
            }
        }

        //Процедура шифрования данных L2
        private void Encode(ref byte[] l2)
        {
            //Шифрование полей "Данные" и "Пароль". Сперто из оригинальной процедуры шифрования
            byte _x1 = 0x3A;
            for (int i = 0; i <= 7; i++) { _x1 ^= _passwd[i]; }
            byte _dpcsize = (byte)(l2[4] + 8);                                //Размер "Данные + "Пароль" 
            int k = 4;
            for (int i = _dpcsize - 1; i >= 0; i--)
            {
                byte _x2 = l2[k++];
                l2[k] ^= _x1;
                l2[k] ^= _x2;
                l2[k] ^= _passwd[i % 8];
                _x1 += (byte)i;
            }
        }

        //Парсер ответа счетчика
        private void AnswerParser(byte[] answer)
        {
            L1Result = answer[0];
            L1ResultMsg = ParseL1error(L1Result);
            if (L1Result == 1)
            {
                L1Lenght = answer[1];
                L1Sum = answer[answer.Length - 1];
                L2Result = answer[2];
                L2ResultMsg = ParseL2error(L2Result);
                L2Lenght = answer[3];
                if (L2Result == 0)
                {
                    L2Data = new byte[L1Lenght - 2];
                    Array.Copy(answer, 4, L2Data, 0, answer.Length - 5);
                }
                else
                {
                    _isError = true;
                    _error_txt = L2ResultMsg;
                }
                //Проверка контрольной суммы
                byte cs = 0;
                for (int i = 0; i < answer.Length; i++)
                {
                    cs ^= answer[i];
                }
                if (cs != 0)
                {
                    _isError = true;
                    _error_txt = "Ошибка контрольной суммы";
                }
                else
                {
                    _isError = false;
                    _error_txt = L1ResultMsg;
                }
            }
        }
    
        //Группа преобразователей массива байт в различные типы данных. Принимается, что старший байт имеет младший адрес
        private UInt64 ArrayToUint64(byte[] array)                      //Преобразование массива байт в UInt64 
        {
            UInt64 res = 0;
            for (int i = 0; i < array.Length; i++)
            {
                res += (UInt64)array[array.Length - i] << (8 * i);
            }
            return res;
        }
        private float ArrayToFloat(byte[] array)                        //Преобразование массива байт в float
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(array));
            return reader.ReadSingle();
        }

    }
}