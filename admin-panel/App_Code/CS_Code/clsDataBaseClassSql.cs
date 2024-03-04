using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// does any kind of database opperation on Sql
/// </summary>

public class DataBaseClassSql
{
    SqlDataAdapter daSql;
    string cn = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString);

    
    SqlCommand command = new SqlCommand();
    DataSet dsSql = new DataSet();
    DataTable dtSql = new DataTable();

    public void ConnectDataBaseToInsert(string Query)
    {
        connect = new SqlConnection(cn);
        command.CommandText = Query;
        command.Connection = connect;
        daSql = new SqlDataAdapter(command);
        connect.Open();
        command.ExecuteNonQuery();
        connect.Close();
        //#########################
        connect.Dispose();
        SqlConnection.ClearPool(connect);
        connect = null;

    }
    public void ConnectDataBaseReturnDR(string Query)
    {
        connect = new SqlConnection(cn);
        command.CommandText = Query;
        command.Connection = connect;
        daSql = new SqlDataAdapter(command);
        connect.Open();
        command.ExecuteNonQuery();
        connect.Close();
        //#########################
        connect.Dispose();
        SqlConnection.ClearPool(connect);
        connect = null;

    }
    public DataSet ConnectDataBaseReturnDS(string Query)
    {
        connect = new SqlConnection(cn);
        dsSql = new DataSet();
      
        command.CommandText = Query;
        command.Connection = connect;
        daSql = new SqlDataAdapter(command);
        daSql.Fill(dsSql);
        connect.Open();
        command.ExecuteNonQuery();
        connect.Close();
        //#########################
        connect.Dispose();
        SqlConnection.ClearPool(connect);
        connect = null;
        return dsSql;
    }
    public DataTable ConnectDataBaseReturnDT(string Query)
    {
        connect = new SqlConnection(cn);
        dtSql = new DataTable();
        
        command.CommandText = Query;
        command.Connection = connect;
        daSql = new SqlDataAdapter(command);
        daSql.Fill(dtSql);
        connect.Open();
        command.ExecuteNonQuery();
        connect.Close();
        //#########################
        connect.Dispose();
        SqlConnection.ClearPool(connect);
        connect = null;
        return dtSql;
    }
}
