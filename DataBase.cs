using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;


namespace OblikCleaner
{
    public static class OblikDB
    {
        delegate void MyAction();
        static private FbConnection con;    //соединение с БД
   
        static public bool isError;         //Состояние ошибки
        static public string ErrorMsg;      //Текст ошибки
        static public DataTable dbOblik;    //список счетчиков    

        public static void Initialize() => CreateTableHeaders();

        
        static private void CreateConnection(ref FbConnection connection)   //Создать соединение к БД
        {
            isError = false;
            ErrorMsg = "";

            FbConnectionStringBuilder cs = new FbConnectionStringBuilder
            {
                DataSource = Settings.DBSrvName,
                Database = Settings.DBPath,
                UserID = Settings.DBUser,
                Password = Settings.DBPasswd,
                Charset = "NONE",
                Pooling = false,
                Dialect = 1
            };
            try
            {
                connection.ConnectionString = cs.ToString();
                connection.Open();
            }
            catch (Exception e)
            {
                isError = true;
                ErrorMsg = e.Message;
            }
        }
        static private void CloseConnection (ref FbConnection connection)   //Закрыть соединение к БД
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
                connection.Dispose();
            }
        }
        static private void CreateTableHeaders()                //Создание заголовков таблицы счетчиков
        {
            DataColumn column;
            dbOblik = new DataTable();

            column = new DataColumn
            {
                ColumnName = "name",        //имя счетчика
                DataType = typeof(string)
            };
            dbOblik.Columns.Add(column);

            column = new DataColumn
            {
                ColumnName = "port",        //COM порт
                DataType = typeof(int)
            };
            dbOblik.Columns.Add(column);

            column = new DataColumn
            {
                ColumnName = "addr",        //адрес
                DataType = typeof(int)
            };
            dbOblik.Columns.Add(column);

            column = new DataColumn
            {
                ColumnName = "net_key",     //ИД сети (COM порта) в БД
                DataType = typeof(int)
            };
            dbOblik.Columns.Add(column);

            column = new DataColumn
            {
                ColumnName = "cntr_id",     //ИД счетчика в БД
                DataType = typeof(int)
            };
            dbOblik.Columns.Add(column);

            column = new DataColumn
            {
                ColumnName = "main_feeder",     //фидер основного канала в счетчике
                DataType = typeof(int)
            };
            dbOblik.Columns.Add(column);

            column = new DataColumn
            {
                ColumnName = "last_rec",        //Дата и время последнего опроса
                DataType = typeof(DateTime)
            };
            dbOblik.Columns.Add(column);
        }       
        static public void GetCountersList()                   //Получение списка счетчиков из БД
        {
            FbDataReader reader;
            FbCommand cmd;
            FbConnection con = new FbConnection();
            CreateConnection(ref con);
            if (isError)    //Выход при наличии ошибки
            {
                return;
            }
            string sql;
            //Получение списка счетчиков
            sql = "SELECT * FROM OBLIK";
            cmd = new FbCommand(sql, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DataRow row = dbOblik.NewRow();
                    row["addr"] = reader["ADDRESS"];
                    row["net_key"] = reader["KEY_NETWORK"];
                    row["cntr_id"] = reader["KEY_COUNTER"];
                    dbOblik.Rows.Add(row);
                }
            }
            reader.Close();
            //Получение портов
            foreach (DataRow row in dbOblik.Rows)
            {
                sql = "SELECT COM_PORT FROM NETWORKS WHERE KEY_NETWORK=@net";
                cmd = new FbCommand(sql, con);
                FbParameter pNet = new FbParameter("@net", (int)row["net_key"]);
                cmd.Parameters.Add(pNet);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        row["port"] = reader.GetValue(0);
                    }
                }
                reader.Close();
                //Получение названий счетчиков
                sql = "SELECT " +
                        "FIDERS.NAME_FIDER " +
                        "FROM FIDERS,CHANNELS,CHANNELS_FIDERS " +
                        "WHERE " +
                        "(" +
                            "(FIDERS.KEY_FIDER = CHANNELS_FIDERS.KEY_FIDER) " +
                            "AND (CHANNELS_FIDERS.KEY_CHANNEL = CHANNELS.KEY_CHANNEL) " +
                            "AND (CHANNELS.CHANNEL_TYPE = 0) " +
                            "AND (CHANNELS.CHANNEL_NUM = 1) " +
                            "AND (CHANNELS.KEY_COUNTER = @cntr)" +
                        ")";
                cmd = new FbCommand(sql, con);
                FbParameter cntr = new FbParameter("@cntr", (int)row["cntr_id"]);
                cmd.Parameters.Add(cntr);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        row["name"] = reader.GetValue(0);
                    }
                }
                reader.Close();
                //Получение номера основного фидера
                sql = "SELECT " +
                    "CHANNELS.KEY_CHANNEL " +
                    "FROM CHANNELS " +
                    "WHERE " +
                    "(" +
                    "(CHANNELS.KEY_COUNTER = @cntr) " +
                    "AND (CHANNELS.CHANNEL_NUM = 1) " +
                    "AND (CHANNELS.CHANNEL_TYPE = 0)" +
                    ")";
                cmd = new FbCommand(sql, con);
                cntr = new FbParameter("@cntr", (int)row["cntr_id"]);
                cmd.Parameters.Add(cntr);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        row["main_feeder"] = reader.GetValue(0);
                    }
                }
                reader.Close();
                cmd.Dispose();
            }
            CloseConnection(ref con);
        }
        static public void GetLastRequest()                    //Получить время последнего опроса счетчиков
        {
            FbDataReader reader;
            FbCommand cmd;
            FbConnection con = new FbConnection();
            CreateConnection(ref con);
            if(isError) { return; } // Выход при ошибке
            string sql;
            sql = "select " +
                "day_graph.date_info " +
                "from day_graph " +
                "where day_graph.key_val = " +
                "(select max(day_graph.key_val) " +
                "from day_graph " +
                "where day_graph.key_channel = @channel)";
            foreach (DataRow row in dbOblik.Rows)
            {
                cmd = new FbCommand(sql, con);
                FbParameter cntr = new FbParameter("@channel", (int)row["main_feeder"]);
                cmd.Parameters.Add(cntr);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        row["last_rec"] = reader.GetValue(0);
                    }
                }
                reader.Close();
                cmd.Dispose();
                CloseConnection(ref con);
            }
        }
    } 
}