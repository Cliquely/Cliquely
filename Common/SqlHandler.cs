using System.Data.SQLite;
using System.IO;
using System;

namespace Cliquely
{
    public class SqlHandler
    {
        private SQLiteConnection m_Connection;

        public SQLiteConnection Connection
        {
            get { return m_Connection; }
        }

        public SqlHandler(string i_DataBaseName)
        {
            if (!File.Exists(i_DataBaseName))
                throw new ArgumentException("File doesn't exist");
            
            m_Connection = new SQLiteConnection("Data Source=" + i_DataBaseName + ";Version=3; Compress=True;");
        }

        //Open Connection
        public void OpenConnection()
        {
            m_Connection.Open();
        }

        //Close connection
        public void CloseConnection()
        {
            m_Connection.Close();
        }

    }
}
