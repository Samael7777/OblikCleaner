using Obliks;
using System;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;


namespace OblikCleaner
{

    public partial class frmMain : Form

    {
        //Инициализация базовых структур
        Settings settings = new Settings();                                                 //Основные настройки
        Counters counters = new Counters();                                                 //Инициализация списка счетчиков
        ServiceController odcs = new ServiceController("OasysDataCollectionService");       //Управление службой опроса
        ServiceController omms = new ServiceController("OasysMetersMonitoringService");     //Управление службой мониторинга
        delegate void MyAction(int index);

        public frmMain()
        {
            InitializeComponent();
            SetSettings();          //Применение настроек к форме
            CreareDataTable();      //Отображение таблицы счетчиков
        }

        //Методы пользователя
        private void MassAction(MyAction action)                                              //Массовая операция над выделенными строками
        {
            bool MassSel = false;
            for (int i = 0; i < counters.tblCounters.Rows.Count; i++)
            {
                if (dgCounters.Rows[i].Cells["sel"].Value != null && (bool)dgCounters.Rows[i].Cells["sel"].Value == true)
                {
                    MassSel = true;
                    action.Invoke(i);
                }
            }
            if (!MassSel)
            {
                int i = dgCounters.CurrentCell.RowIndex;
                action.Invoke(i);
            }
        }
        private void MassDelete()                                                               //Удаление выбранных записей
        {
            bool MassSel = false;
            for (int i = counters.tblCounters.Rows.Count - 1; i >= 0; i--)
            {
                if (dgCounters.Rows[i].Cells["sel"].Value != null && (bool)dgCounters.Rows[i].Cells["sel"].Value == true)
                {
                    MassSel = true;
                    counters.tblCounters.Rows.RemoveAt(i);
                }
            }
            if (!MassSel)
            {
                int i = dgCounters.CurrentCell.RowIndex;
                counters.tblCounters.Rows.RemoveAt(i);
            }
        }
        private void SetSettings()                                                              // Начальные настройки формы
        {
            numRepeats.Value = settings.repeats;
            numTimeout.Value = settings.timeout;
            chkSaveLogs.Checked = settings.SaveLogs;
            chkService.Checked = settings.StopService;
        }
        private void CreareDataTable()                                                          //Настройка отображения таблицы счетчиков
        {
            //Создание столбцов отображаемой таблицы
            dgCounters.DataSource = counters.tblCounters;
            dgCounters.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //Выбирать сразу всю строку
            dgCounters.MultiSelect = false;                                     //Запретить выбор нескольких строк
            dgCounters.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;  //Выравнивание заголовков по центру

            dgCounters.Columns["name"].HeaderText = "Название";
            dgCounters.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; //Ширина подстраивается для заполнения всего элемента
            dgCounters.Columns["name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgCounters.Columns["port"].HeaderText = "COM порт";
            dgCounters.Columns["port"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgCounters.Columns["addr"].HeaderText = "Адрес";
            dgCounters.Columns["addr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgCounters.Columns["dg_recs"].HeaderText = "Записей СГ";
            dgCounters.Columns["dg_recs"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgCounters.Columns["sel"].HeaderText = "Выбор";
            dgCounters.Columns["sel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //Отключение ручной сортировки столбцов
            foreach (DataGridViewColumn c in dgCounters.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void StopOblikServices()                                                        //Остановка служб ОБЛИК
        {
            LogLine("Попытка остановки служб Облик");
            LogLine("-----------------------------");
            LogLine("Остановка службы автоматического опроса");
            try
            {
                if (odcs.Status == ServiceControllerStatus.Stopped)
                {
                    LogLine("Служба не требует остановки");
                }
                else
                {
                    odcs.Stop();
                    LogLine("Служба остановлена успешно");
                }
            }
            catch (Exception e)
            {
                LogLine("Ошибка остановки службы: " + e.Message);
            }
            log.Items.Add("");

            LogLine("Остановка службы мониторинга");
            try
            {
                if (omms.Status == ServiceControllerStatus.Stopped)
                {
                    LogLine("Служба не требует остановки");
                }
                else
                {
                    omms.Stop();
                    LogLine("Служба остановлена успешно");
                }
            }
            catch (Exception e)
            {
                LogLine("Ошибка остановки службы: " + e.Message);
            }
            LogLine("-----------------------------");
        }
        private void StartOblikServices()                                                       //Запуск служб ОБЛИК
        {
            LogLine("Попытка запуска служб Облик");
            LogLine("-----------------------------");
            LogLine("Запуск службы автоматического опроса");
            try
            {
                if (odcs.Status == ServiceControllerStatus.Running)
                {
                    LogLine("Служба уже запущена");
                }
                else
                {
                    odcs.Start();
                    LogLine("Служба запущена успешно");
                }
            }
            catch (Exception e)
            {
                LogLine("Ошибка запуска службы: " + e.Message);
            }
            LogLine("");

            log.Items.Add(System.DateTime.Now.ToString() + " : " + "Запуск службы мониторинга");
            try
            {
                if (omms.Status == ServiceControllerStatus.Running)
                {
                    LogLine("Служба уже запущена");
                }
                else
                {
                    omms.Start();
                    LogLine("Служба запущена успешно");
                }
            }
            catch (Exception e)
            {
                LogLine("Ошибка запуска службы: " + e.Message);
            }
            LogLine("-----------------------------"); ;
        }
        private void GetDGRecs(int index)                                                       //Получение заполнения суточного графика
        {
            int result;
            string lstr;
            int port = (int)dgCounters.Rows[index].Cells["port"].Value;
            int addr = (int)dgCounters.Rows[index].Cells["addr"].Value;
            lstr = String.Format("Получение данных со счетчика COM{0}, адрес:{1:X2}", port, addr);
            LogLine(lstr);
            Oblik oblik = new Oblik(port, addr);
            result = oblik.GetDayGraphRecs();
            if (!oblik.isError)
            {
                LogLine("Данные получены успешно");
                dgCounters.Rows[index].Cells["dg_recs"].Value = result;
                dgCounters.Refresh();
            }
            else
            {
                LogLine("Ошибка: " + oblik.ErrorMsg);
                dgCounters.Rows[index].Cells["dg_recs"].Value = "Ошибка";
                dgCounters.Refresh();
            }
            oblik.Dispose();
        }
        private void CleanDGRecs(int index)                                                     //Очистка суточного графика + установка текущего времени
        {
            int port = (int)dgCounters.Rows[index].Cells["port"].Value;
            int addr = (int)dgCounters.Rows[index].Cells["addr"].Value;
            LogLine(String.Format("Очистка суточного графика счетчика COM{0}, адрес:{1:X2}", port, addr));
            Oblik oblik = new Oblik(port, addr);
            oblik.CleanDayGraph();      //Очистка суточного графика
            if (!oblik.isError)
            {
                LogLine("Суточный график очищен");
                GetDGRecs(index);
            }
            else
            {
                LogLine("Ошибка: " + oblik.ErrorMsg);
                dgCounters.Rows[index].Cells["dg_recs"].Value = "Ошибка";
            }
            LogLine(String.Format("Установка текущего времени счетчика COM{0}, адрес:{1:X2}", port, addr));
            oblik.SetCurrentTime();     //Установка текущего времени
            if (!oblik.isError)
            {
                LogLine("Текущее время установлено");
                GetDGRecs(index);
            }
            else
            {
                LogLine("Ошибка: " + oblik.ErrorMsg);
            }
            oblik.Dispose();
            dgCounters.Refresh();
        }
        private void LogLine(string rec)                                                        //Лог событий
        {
            try
            {
                string _ctime = System.DateTime.Now.ToString();         //Текущее время
                DateTime _cdate = System.DateTime.Now.Date;              //Теущая дата
                string line = _ctime + " : " + rec;
                string filename = _cdate.ToString("yyyy-MM-dd") + ".log";
                log.Items.Add(line);
                if (settings.SaveLogs)
                {
                    using (StreamWriter _log = new StreamWriter(filename, true, System.Text.Encoding.Default))
                    {
                        _log.WriteLine(line);
                    }
                }

            }
            catch (Exception e)
            {
                log.Items.Add(e.Message);
            }

        }

        //Обработчики событий элементов формы
        private void NumTimeout_ValueChanged(object sender, EventArgs e) => settings.timeout = (int)numTimeout.Value;
        private void NumRepeats_ValueChanged(object sender, EventArgs e) => settings.repeats = (int)numRepeats.Value;
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e) => counters.SaveData();
        private void CmAdd_Click(object sender, EventArgs e) => counters.tblCounters.Rows.Add();
        private void CmDel_Click(object sender, EventArgs e)                            //Удаление выбранных счетчиков
        {
            DialogResult dialogResult = MessageBox.Show("Удалить выбранные записи?", "Подтверждение", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                MassDelete();
            }
        }
        private void BtnDel_Click(object sender, EventArgs e)                           //Удаление выбранных счетчиков
        {
            DialogResult dialogResult = MessageBox.Show("Удалить выбранные записи?", "Подтверждение", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                MassDelete();
            }
        }
        private void CmSelAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgCounters.Rows)
            {
                row.Cells["sel"].Value = 1;
            }
        }
        private void CmSelNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgCounters.Rows)
            {
                row.Cells["sel"].Value = 0;
            }
        }
        private void BtnSave_Click(object sender, EventArgs e) => counters.SaveData();
        private void ChkSaveLogs_CheckedChanged(object sender, EventArgs e) => settings.SaveLogs = chkSaveLogs.Checked;
        private void ChkService_CheckedChanged(object sender, EventArgs e) => settings.StopService = chkService.Checked;
        private void BtnAdd_Click(object sender, EventArgs e) => counters.tblCounters.Rows.Add();
        private void DgCounters_CellValueChanged(object sender, DataGridViewCellEventArgs e) => counters.SaveData();
        private void BtnCleanLog_Click(object sender, EventArgs e) => log.Items.Clear();
        private void BtnSvcStop_Click(object sender, EventArgs e) => StopOblikServices();
        private void BtnGetdata_Click(object sender, EventArgs e)
        {
            MyAction action = new MyAction(GetDGRecs);
            MassAction(action);
        }
        private void BtnDelDG_Click(object sender, EventArgs e)
        {
            if (settings.StopService)
            {
                StopOblikServices();
            }
            MyAction action = new MyAction(CleanDGRecs);
            MassAction(action);
            if (settings.StopService)
            {
                StartOblikServices();
            }
        }
        private void CmGetData_Click(object sender, EventArgs e)
        {
            MyAction action = new MyAction(GetDGRecs);
            MassAction(action);
        }
    }
}
