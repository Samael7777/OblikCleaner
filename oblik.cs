using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Obliks
{
    public class Oblik : IDisposable
    {

        //Локальные переменные
        private bool disposed = false;          //Флаг деструктора
        private SerialPort com;                 //обьявление COM порта
        private int _port;                      //порт счетчика
        private int _addr;                      //адрес счетчика
        private int _baudrate;                  //скорость работы порта, 9600 бод - по умолчанию
        private int _timeout, _repeats;         //таймаут и повторы
        private byte[] _passwd;                 //пароль
        private byte[] _rbuf;                   //Буфер для чтения
        private bool _isError;                  //Наличие ошибки
        private string _error_txt = "";         //текст ошибки
        private byte _user;                     //Пользователь от 0 до 3 (3 - максимальные привелегии, 0 - минимальные)
        private object SerialIncoming;          //Монитор таймаута чтения порта

        //Интерфейс класса

        //Структура ответа счетчика
        public int L1Result { get; set; }                //Результат фрейма L1
        public string L1ResultMsg { get; set; }          //Результат фрейма L1, расшифровка
        public int L1Lenght { get; set; }                //Количетво байт в полях "Длина", "L2Data", "Результат" 
        public int L1Sum { get; set; }                   //Контрольная сумма
        public int L2Result { get; set; }                //Результат запроса L2
        public string L2ResultMsg { get; set; }          //Результат запроса L2, расшифровка
        public int L2Lenght { get; set; }                //Количество данных, успешно обработанных операцией
        public byte[] L2Data { get; set; }               //Данные L2
        public byte[] RawResponse
        {
            get => _rbuf;
        }                    //Сырые данные со счетчика
        public int RawResponseLenght
        {
            get => _rbuf.Length;
        }                  //Количество байт в ответе счетчика
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
        public bool isError
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

        //Удаление объекта класса и очистка мусора
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                disposed = true;
                if (disposing)
                {
                    // Dispose managed resources.
                    com.Dispose();
                }
                // Dispose unmanaged resources.
                _rbuf = null;
                _passwd = null;
            }
        }
        ~Oblik()
        {
            Dispose(false);
        }

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
            if (AccType > 0) { AccType = 1; }                              //Все, что больше 0 - команда на запись
                                                                           //Формируем запрос L2

            _l2 = new byte[5 + (len + 8) * AccType];                        //5 байт заголовка + 8 байт пароля + данные 
            _l2[0] = (byte)((segment & 127) + AccType * 128);               //(биты 0 - 6 - номер сегмента, бит 7 = 1 - операция записи)
            _l2[1] = _user;                                                 //Указываем пользователя
            _l2[2] = (byte)(offset >> 8);                                   //Старший байт смещения
            _l2[3] = (byte)(offset & 0xff);                                 //Младший байт смещения
            _l2[4] = len;                                                   //Размер считываемых данных

            //Если команда - на запись в сегмент
            if (AccType > 0)
            {
                Array.Copy(data, 0, _l2, 5, len);                               //Копируем данные в L2
                Array.Copy(_passwd, 0, _l2, len + 5, 8);                        //Копируем пароль в L2

                //Шифрование полей "Данные" и "Пароль". Сперто из оригинальной процедуры шифрования
                byte _x1 = 0x3A;
                for (int i = 0; i <= 7; i++) { _x1 ^= _passwd[i]; }
                byte _dpcsize = (byte)(len + 8);                                //Размер "Данные + "Пароль" 
                int k = 4;
                for (int i = _dpcsize - 1; i >= 0; i--)
                {
                    byte _x2 = _l2[k++];
                    _l2[k] ^= _x1;
                    _l2[k] ^= _x2;
                    _l2[k] ^= _passwd[i % 8];
                    _x1 += (byte)i;
                }
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
                _l1[_l1.Length - 1] = (byte)(_l1[_l1.Length - 1] ^ _l1[i]);
            }

            //Обмен данными со счетчиком
            RSExchange(_l1);

            //Заполняем структуру ответа счетчика
            if (!_isError)
            {
                L1Result = _rbuf[0];
                L1ResultMsg = ParseL1error(L1Result);
                if (L1Result == 1)
                {
                    L1Lenght = _rbuf[1];
                    L1Sum = _rbuf[_rbuf.Length - 1];
                    L2Result = _rbuf[2];
                    L2ResultMsg = ParseL2error(L2Result);
                    L2Lenght = _rbuf[3];
                    if (L2Result == 0)
                    {
                        L2Data = new byte[L1Lenght - 2];
                        Array.Copy(_rbuf, 4, L2Data, 0, _rbuf.Length - 5);
                    }
                    else
                    {
                        _isError = true;
                        _error_txt = L2ResultMsg;
                    }
                    //Проверка контрольной суммы
                    byte cs = 0;
                    for (int i = 0; i < _rbuf.Length; i++)
                    {
                        cs ^= _rbuf[i];
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

        }

        //Получить количество записей суточного графика
        public int GetDayGraphRecs()
        {
            SegmentAccsess(44, 0, 2, null, 0);
            //Порядок байт в счетчике - обратный по отношению к пк, переворачиваем
            if (!_isError)
            {
                byte[] _tmp = new byte[L2Data.Length];
                Array.Copy(L2Data, 0, _tmp, 0, L2Data.Length);
                Array.Reverse(_tmp);
                int res = BitConverter.ToUInt16(_tmp, 0);
                return res;
            }
            else return -1;
        }

        //Стирание суточного графика
        public void CleanDayGraph()
        {
            byte[] cmd = new byte[2];
            cmd[0] = (byte)~(_addr);
            cmd[1] = (byte)_addr;
            SegmentAccsess(88, 0, (byte)cmd.Length, cmd, 1);
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

        // Событие чтения данных из порта
        void DataReciever(object s, SerialDataReceivedEventArgs ea)
        {
            _rbuf = new byte[com.BytesToRead];
            com.Read(_rbuf, 0, _rbuf.Length);
            lock (SerialIncoming)
            {
                Monitor.Pulse(SerialIncoming);
            }
        }

        //Отправка запроса и получение данных WData - запрос, _rbuf - ответ
        void RSExchange(byte[] WData)
        {
            _isError = false;
            _error_txt = "";
            try
            {
                //Параметризация и открытие порта
                com = new SerialPort
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
                };
                if (com.IsOpen) { com.Close(); }    //закрыть ранее открытый порт
                com.Open();

                //Отправка данных
                com.DiscardOutBuffer();                                                                 //очистка буфера передачи
                com.DataReceived += new SerialDataReceivedEventHandler(DataReciever);                   //событие чтения из порта
                com.Write(WData, 0, WData.Length);                                                      //отправка буфера записи
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
            }
            catch (Exception e)
            {
                _isError = true;
                _error_txt = e.Message;
            }
            finally
            {
                WData = null;
            }
        }

    }
}