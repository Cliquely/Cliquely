using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Web;

namespace Cliquely
{
    class SqlHelper
    {
        SqlHandler m_Connection;
        const string path = @"Proteins.data";
        public SqlHelper()
        {
            m_Connection = new SqlHandler(path);
        }

        //Insert/Update/Delete statement
        public void Edit(string i_Query)
        {
            m_Connection.OpenConnection();

            SQLiteCommand cmd = new SQLiteCommand(i_Query, m_Connection.Connection);

            cmd.ExecuteNonQuery();

            m_Connection.CloseConnection();
        }

        public DataTable Select(string i_Query)
        {
            DataTable table = new DataTable();

            m_Connection.OpenConnection();

            SQLiteCommand cmd = new SQLiteCommand(i_Query, m_Connection.Connection);

            SQLiteDataReader dataReader = cmd.ExecuteReader();

            table.Load(dataReader);

            dataReader.Close();
            m_Connection.CloseConnection();

            return table;
        }
    }
}
