using System.Data;
using System.IO;
using System.Xml.Linq;
using System;



namespace OblikCleaner
{
    public static class Counters
    {
        public static DataTable tblCounters { get; set; }                  //Таблица для хранения списка счетчиков
        const string counters_file = "counters.xml";                       //Имя XML с данными
        private static XDocument xCountersFile;                            //Привязка к XML

        //Создание шаблона таблицы
        public static void Initialize()
        {
            DataColumn column;      //Тип "столбец таблицы"
            tblCounters = new DataTable("counters");

            //Создание структуры таблицы

            //Название счетчика
            column = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "name",
                ReadOnly = false,
                Unique = false,
                AllowDBNull = true
            };
            tblCounters.Columns.Add(column);

            //Порт подключения счетчика
            column = new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "port",
                ReadOnly = false,
                Unique = false,
                DefaultValue = 1,
                AllowDBNull = false
            };
            tblCounters.Columns.Add(column);

            //Адрес счетчика
            column = new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "addr",
                ReadOnly = false,
                Unique = false,
                AllowDBNull = false,
                DefaultValue = 0
            };
            tblCounters.Columns.Add(column);

            //Количество записей суточного графика
            column = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "dg_recs",
                ReadOnly = false,
                Unique = false,
                AllowDBNull = false,
                DefaultValue = "Нет данных"
            };
            tblCounters.Columns.Add(column);

            //Статус выбора счетчиков
            column = new DataColumn
            {
                DataType = typeof(bool),
                ColumnName = "sel",
                ReadOnly = false,
                Unique = false,
                AllowDBNull = false,
                DefaultValue = false
            };
            tblCounters.Columns.Add(column);

            //Дата последнего опроса
            column = new DataColumn
            {
                DataType = typeof(DateTime),
                ColumnName = "last_rec",
                ReadOnly = true,
                Unique = false,
                AllowDBNull = true,
            };
            tblCounters.Columns.Add(column);


            LoadData(); // Загрузка данных из файла, если он существует
        }

        //Загрузка структуры данных XML       
        private static void LoadData()
        {
            if (File.Exists(counters_file))
            {
                //Загрузка сохраненных данных в таблицу
                xCountersFile = XDocument.Load(counters_file);
                foreach (XElement e in xCountersFile.Element("Counters").Elements("Counter"))
                {
                    DataRow row = tblCounters.NewRow();
                    row["name"] = e.Element("name").Value;
                    row["port"] = e.Element("port").Value;
                    row["addr"] = e.Element("addr").Value;
                    row["sel"] = e.Element("sel").Value;
                    tblCounters.Rows.Add(row);
                }
            }
        }

        //Сохранение данных в XML
        public static void SaveData()
        {
            xCountersFile = new XDocument();
            XElement counters = new XElement("Counters");
            foreach (DataRow row in tblCounters.Rows)
            {
                XElement counter = new XElement("Counter");
                counter.Add(new XElement("name", row["name"].ToString()));
                counter.Add(new XElement("port", row["port"].ToString()));
                counter.Add(new XElement("addr", row["addr"].ToString()));
                counter.Add(new XElement("sel", row["sel"].ToString()));
                counters.Add(counter);
            }
            xCountersFile.Add(counters);
            xCountersFile.Save(counters_file);
        }

    }
}
