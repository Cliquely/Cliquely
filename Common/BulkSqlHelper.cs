using System;
using System.Data;
using System.Data.SQLite;

namespace Cliquely
{
	class BulkSqlHelper : IDisposable
    {
        SqlHandler m_Connection;
	    private string path = @"Proteins.data";

		public string Path
	    {
		    get => path;
		    set
		    {
			    path = value;
			    m_Connection = new SqlHandler(path);

		    }
	    }

	    public BulkSqlHelper(string path = @"Proteins.data")
        {
            m_Connection = new SqlHandler(path);
			m_Connection.OpenConnection();
        }

        //Insert/Update/Delete statement
        public void Edit(string i_Query)
        {
            var cmd = new SQLiteCommand(i_Query, m_Connection.Connection);
            cmd.ExecuteNonQuery();
        }

        public DataTable Select(string i_Query)
        {
            var table = new DataTable();
            var cmd = new SQLiteCommand(i_Query, m_Connection.Connection);

            var dataReader = cmd.ExecuteReader();

            table.Load(dataReader);
            dataReader.Close();

            return table;
        }

	    public void Dispose()
	    {
		    m_Connection.CloseConnection();
		}
	}
}
