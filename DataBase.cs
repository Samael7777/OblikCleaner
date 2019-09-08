using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;

namespace OblikCleaner
{
    public static class OblikDB
    {
        static private FbConnection con;


        static public bool isError;     //Состояние ошибки
        static public string ErrorMsg;  //Текст ошибки
        static public DataTable dbOblik;  //список счетчиков    
     
        static public void GetList()
        {
            isError = false;
            ErrorMsg = "";

            FbConnectionStringBuilder cs = new FbConnectionStringBuilder();
            cs.DataSource = Settings.DBSrvName;
            cs.Database = Settings.DBPath;
            cs.UserID = Settings.DBUser;
            cs.Password = Settings.DBPasswd;
            cs.Charset = "NONE";
            cs.Pooling = false;

            try
            {
                using (con = new FbConnection(cs.ToString()))
                {
                    con.Open();
                    CreateTableHeaders();
                    GetData();
                    con.Close();
                }
                
               
            }
            catch (Exception e)
            {
                isError = true;
                ErrorMsg = e.Message;
            }

        }

        static private void CreateTableHeaders()
        {
            DataColumn column;
            dbOblik = new DataTable();

            column = new DataColumn();
            column.ColumnName = "name";
            column.DataType = typeof(string);
            dbOblik.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "port";
            column.DataType = typeof(int);
            dbOblik.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "addr";
            column.DataType = typeof(int);
            dbOblik.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "net_key";
            column.DataType = typeof(int);
            dbOblik.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "cntr_id";
            column.DataType = typeof(int);
            dbOblik.Columns.Add(column);
        }
        static private void GetData()
        {
            FbDataReader reader;
            FbCommand cmd;
            string sql;
            isError = false;
            try
            {
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
                }
            }
            catch (Exception e)
            {
                isError = true;
                ErrorMsg = e.Message;
            }
            
        }
        
    } 
}