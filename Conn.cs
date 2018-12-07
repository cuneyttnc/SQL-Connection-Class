using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

public class Conn
{
    public string ConnectionString
    {
        get { return sConnectionString; }
        set { sConnectionString = value; }
    }

    
    private SqlConnection Connection = new SqlConnection();
    private SqlCommand Command = new SqlCommand();

    private SqlDataReader DataReaders;
    public SqlDataReader DataReader
    {
        get { return DataReaders; }
        set { DataReaders = value; }
    }
    

    public SqlDataAdapter DataAdapters;
    public SqlDataAdapter DataAdapter
    {
        get { return DataAdapters; }
        set { DataAdapters.Dispose(); DataAdapters = new SqlDataAdapter(); DataAdapters = value; }
    }
    

    private DataSet DataSet;

    private SqlParameter Parameter = new SqlParameter();
    private string sConnectionString = "Server = localhost\\SQLEXPRESS; Database = MyDatabase; User Id = sa; Password = as; Trusted_Connection=False; MultipleActiveResultSets=True;";

    #region Open Close Procedur
    public bool Open()
    {
        if (Connection.State != ConnectionState.Open)
        {
            Connection.ConnectionString = sConnectionString;
            Connection.Open();
        }

        if (Connection.State == ConnectionState.Open || Connection.State == ConnectionState.Connecting)
        {
            Command.Connection = Connection;
            return true;
        }
        else
            return false;
    }
    public void Close()
    {
        if (Connection.State == ConnectionState.Open)
        {
            Connection.Dispose();
            Connection.Close();
        }
    }
    #endregion

    #region ExecuteReader SELECT işlemi, Bu işlemin Execleri

    public SqlDataReader ExecuteReader(string queryString)
    {

        SqlCommand Command = new SqlCommand(queryString, Connection);
        DataReaders = Command.ExecuteReader();
        Command.Dispose();
        return DataReaders;
    }

    public void CloseExecuteReader(SqlDataReader DataReader)
    {
        DataReader.Dispose();
    }



    public void ExecuteDataReader(string queryString)
    {
        SqlCommand Command = new SqlCommand(queryString, Connection);
        DataReaders = Command.ExecuteReader();
        Command.Dispose();
    }

    public void ExecuteReaderAdapter(string queryString)
    {
        DataAdapters = new SqlDataAdapter(queryString, Connection);
    }


    #endregion

    #region ExecuteNonQuery INSERT, DELETE, UPDATE işlemi, Bu işlemlerin Execleri
    public int ExecuteNonQuery(string queryString)
    {
        try
        {
            SqlCommand Command = new SqlCommand(queryString, Connection);
            Command.Connection.Open();
            Command.ExecuteNonQuery();
            Command.Connection.Close();
            return 1;
        }
        catch{ return 0; }
    }
    #endregion

    #region ExecuteScalar Geriye Tek Satır işlemi Count,Max,Min, Select Top 1, 1 Satır 1 Sutun işlemleri, Bu işlemlerin Execleri
    public object ExecuteScalar(string queryString)
    {
        SqlCommand Command = new SqlCommand(queryString, Connection);
        object o = Command.ExecuteScalar();
        Command.Dispose();
        return o;
    }
    #endregion


    public DataSet DataSets(string queryString)
    {
        DataAdapter = new SqlDataAdapter(queryString, Connection);
        DataSet = new DataSet();
        DataAdapter.Fill(DataSet);
        return DataSet;
    }


    public string ExecSQL(string paramater)
    {
        Command.CommandType = CommandType.StoredProcedure;
        Command.Parameters.Add(new SqlParameter(paramater, SqlDbType.VarChar));
        Command.Parameters[paramater].Direction = ParameterDirection.Output;
        Command.ExecuteNonQuery();
        return Command.Parameters[paramater].Value.ToString();
    }

}

