using System.Data;
using System.Data.SQLite;

namespace Cliquely
{
	class SqlHelper
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

	    public SqlHelper(string path = @"Proteins.data")
        {
            m_Connection = new SqlHandler(path);
        }

        //Insert/Update/Delete statement
        public void Edit(string i_Query)
        {
            m_Connection.OpenConnection();

            var cmd = new SQLiteCommand(i_Query, m_Connection.Connection);

            cmd.ExecuteNonQuery();

            m_Connection.CloseConnection();
        }

        public DataTable Select(string i_Query)
        {
            var table = new DataTable();

            m_Connection.OpenConnection();

            var cmd = new SQLiteCommand(i_Query, m_Connection.Connection);

            var dataReader = cmd.ExecuteReader();

            table.Load(dataReader);

            dataReader.Close();
            m_Connection.CloseConnection();

            return table;
        }
    }
}
