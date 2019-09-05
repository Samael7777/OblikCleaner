using System.Data;
using System.IO;
using System.Xml.Linq;



namespace OblikCleaner
{
    public class Counters
    {
        public DataTable tblCounters { get; set; }                  //Таблица для хранения списка счетчиков
        const string counters_file = "counters.xml";                //Имя XML с данными
        private XDocument xCountersFile;                            //Привязка к XML

        //Создание шаблона таблицы
        public Counters()
        {
            DataColumn column;      //Тип "столбец таблицы"
            tblCounters = new DataTable("counters");

            //Создание структуры таблицы

            //Название счетчика
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "name";
            column.ReadOnly = false;
            column.Unique = false;
            column.AllowDBNull = true;
            tblCounters.Columns.Add(column);

            //Порт подключения счетчика
            column = new DataColumn();
            column.DataType = typeof(int);
            column.ColumnName = "port";
            column.ReadOnly = false;
            column.Unique = false;
            column.DefaultValue = 1;
            column.AllowDBNull = false;
            tblCounters.Columns.Add(column);

            //Адрес счетчика
            column = new DataColumn();
            column.DataType = typeof(int);
            column.ColumnName = "addr";
            column.ReadOnly = false;
            column.Unique = false;
            column.AllowDBNull = false;
            column.DefaultValue = 0;
            tblCounters.Columns.Add(column);

            //Количество записей суточного графика
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "dg_recs";
            column.ReadOnly = false;
            column.Unique = false;
            column.AllowDBNull = false;
            column.DefaultValue = "Нет данных";
            tblCounters.Columns.Add(column);

            //Статус выбора счетчиков
            column = new DataColumn();
            column.DataType = typeof(bool);
            column.ColumnName = "sel";
            column.ReadOnly = false;
            column.Unique = false;
            column.AllowDBNull = false;
            column.DefaultValue = false;
            tblCounters.Columns.Add(column);

            LoadData(); // Загрузка данных из файла, если он существует
        }

        //Загрузка структуры данных XML       
        private void LoadData()
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
        public void SaveData()
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
