using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;
using Oblik;

namespace OblikCleaner
{

    public partial class frmMain : Form

    {
        //Инициализация базовых структур
        delegate void MyAction(int index);

        public frmMain()
        {
            InitializeComponent();
            Counters.Initialize();  //Инициализация списка счетчиков
            SetSettings();          //Применение настроек к форме
            CreareDataTable();      //Отображение таблицы счетчиков
            OblikDB.Initialize();   //Инициализация БД Облик
        }

        //Методы пользователя
        private void MassAction(MyAction action)                                                //Массовая операция над выделенными строками
        {
            bool MassSel = false;
            for (int i = 0; i < Counters.CountersTable.Rows.Count; i++)
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
            for (int i = Counters.CountersTable.Rows.Count - 1; i >= 0; i--)
            {
                if (dgCounters.Rows[i].Cells["sel"].Value != null && (bool)dgCounters.Rows[i].Cells["sel"].Value == true)
                {
                    MassSel = true;
                    Counters.CountersTable.Rows.RemoveAt(i);
                }
            }
            if (!MassSel)
            {
                int i = dgCounters.CurrentCell.RowIndex;
                Counters.CountersTable.Rows.RemoveAt(i);
            }
        }
        private void SetSettings()                                                              // Начальные настройки формы
        {
            Settings.GetSettings();
            chkSaveLogs.Checked = Settings.SaveLogs;
        }
        private void CreareDataTable()                                                          //Настройка отображения таблицы счетчиков
        {
            //Создание столбцов отображаемой таблицы
            dgCounters.DataSource = Counters.CountersTable;
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

            dgCounters.Columns["last_rec"].HeaderText = "Последний опрос";
            dgCounters.Columns["last_rec"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
            using (ServiceController odcs = new ServiceController("OasysDataCollectionService"))
            {
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
            }
            log.Items.Add("");
            LogLine("Остановка службы мониторинга");
            using (ServiceController omms = new ServiceController("OasysMetersMonitoringService"))
            {
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
            }

            LogLine("-----------------------------");
        }
        private void StartOblikServices()                                                       //Запуск служб ОБЛИК
        {
            LogLine("Попытка запуска служб Облик");
            LogLine("-----------------------------");
            LogLine("Запуск службы автоматического опроса");
            using (ServiceController odcs = new ServiceController("OasysDataCollectionService"))
            {
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
            }
            LogLine("");
            log.Items.Add(System.DateTime.Now.ToString() + " : " + "Запуск службы мониторинга");
            using (ServiceController omms = new ServiceController("OasysMetersMonitoringService"))
            {
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
            }
            LogLine("-----------------------------");
        }
        private void GetDGRecs(int index)                                                       //Получение заполнения суточного графика
        {
            int result;
            string lstr;
            int port = (int)dgCounters.Rows[index].Cells["port"].Value;
            int addr = (int)dgCounters.Rows[index].Cells["addr"].Value;
            lstr = String.Format("Получение данных со счетчика COM{0}, адрес:{1:X2}", port, addr);
            LogLine(lstr);
            Oblik.Oblik oblik = new Oblik.Oblik(port, addr);
            oblik.OnError += LogError;
            oblik.OnStatusChange += LogStatus;
            result = oblik.GetDayGraphRecs();
            if (!oblik.IsError)
            {
                dgCounters.Rows[index].Cells["dg_recs"].Value = result;
            }
            else
            {
                dgCounters.Rows[index].Cells["dg_recs"].Value = "Ошибка";
            }
            dgCounters.Refresh();
        }
        private void CleanDGRecs(int index)                                                     //Очистка суточного графика + установка текущего времени
        {
            int port = (int)dgCounters.Rows[index].Cells["port"].Value;
            int addr = (int)dgCounters.Rows[index].Cells["addr"].Value;
            LogLine(String.Format("Очистка суточного графика счетчика COM{0}, адрес:{1:X2}", port, addr));
            Oblik.Oblik oblik = new Oblik.Oblik(port, addr);
            oblik.OnError += LogError;
            oblik.OnStatusChange += LogStatus;
            oblik.CleanDayGraph();      //Очистка суточного графика
            if (!oblik.IsError)
            {
                LogLine("Суточный график очищен");
                GetDGRecs(index);
            }
            else
            {
                dgCounters.Rows[index].Cells["dg_recs"].Value = "Ошибка";
            }
            LogLine(String.Format("Установка текущего времени счетчика COM{0}, адрес:{1:X2}", port, addr));
            oblik.SetCurrentTime();     //Установка текущего времени
            if (!oblik.IsError)
            {
                LogLine("Текущее время установлено");
            }
            else
            {
                LogLine("Ошибка установки текущего времени");
            }
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
                if (Settings.SaveLogs)
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
        private void GetOblikCounters()                                                         //Получить список счетчиков из БД Облик 
        {
            OblikDB.GetCountersList();
            if (!OblikDB.isError)
            {
                foreach (DataRow dbr in OblikDB.dbOblik.Rows)
                {
                    DataRow cr = Counters.CountersTable.NewRow();
                    cr["port"] = dbr["port"];
                    cr["addr"] = dbr["addr"];
                    cr["name"] = dbr["name"];
                    Counters.CountersTable.Rows.Add(cr);
                }
                dgCounters.Refresh();
            }
            else
            {
                LogLine(OblikDB.ErrorMsg);
            }
        }
        private void LogStatus (object sender, StatusChangeArgs e)                              //Запись в лог состояния
        {
            LogLine("Статус: " + e.Message);
        }
        private void LogError(object sender, ErrEventArgs e)                                    //Запись в лог ошибок
        {
            LogLine("Ошибка: " + e.Message);
        }
        //Обработчики событий элементов формы
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e) => Counters.SaveData();
        private void CmAdd_Click(object sender, EventArgs e) => Counters.CountersTable.Rows.Add();
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
        private void BtnSave_Click(object sender, EventArgs e) => Counters.SaveData();
        private void ChkSaveLogs_CheckedChanged(object sender, EventArgs e) => Settings.SaveLogs = chkSaveLogs.Checked;
        private void BtnAdd_Click(object sender, EventArgs e) => Counters.CountersTable.Rows.Add();
        private void DgCounters_CellValueChanged(object sender, DataGridViewCellEventArgs e) => Counters.SaveData();
        private void BtnCleanLog_Click(object sender, EventArgs e) => log.Items.Clear();
        private void BtnSvcStop_Click(object sender, EventArgs e) => StopOblikServices();
        private void BtnGetdata_Click(object sender, EventArgs e)
        {
            MyAction action = new MyAction(GetDGRecs);
            MassAction(action);
        }
        private void BtnDelDG_Click(object sender, EventArgs e)
        {
            if (Settings.StopService)
            {
                StopOblikServices();
            }
            MyAction action = new MyAction(CleanDGRecs);
            MassAction(action);
            if (Settings.StopService)
            {
                StartOblikServices();
            }
        }
        private void CmGetData_Click(object sender, EventArgs e)
        {
            MyAction action = new MyAction(GetDGRecs);
            MassAction(action);
        }
        private void BtnSetBD_Click(object sender, EventArgs e)
        {
            frmSettings frmSettings = new frmSettings();
            frmSettings.Show();
        }
        private void BtnGetDB_Click(object sender, EventArgs e)
        {
            GetOblikCounters();
        }

        private void BtnLastAsk_Click(object sender, EventArgs e)
        {
            OblikDB.GetLastRequest();
            foreach (DataRow row in Counters.CountersTable.Rows)
            {
                int port = (int)row["port"];
                int addr = (int)row["addr"];
                IEnumerable<DataRow> query = from myrow in OblikDB.dbOblik.AsEnumerable()
                                             where ((myrow.Field<int>("port") == port) && (myrow.Field<int>("addr") == addr))
                                             select myrow;
                if (query.Any())
                {
                    row["last_rec"] = query.First().Field<DateTime>("last_rec");
                }
            }
            dgCounters.Refresh();
        }
    }
}
