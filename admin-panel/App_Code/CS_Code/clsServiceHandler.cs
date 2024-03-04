using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

/// <summary>
/// Summary description for clsGlobalSetup
/// </summary>
public class clsServiceHandler
{
    private SqlTransaction dbTransaction = null;
    string cn = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
    SqlConnection con;
    public clsServiceHandler()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string ExecuteScript(string strScript)
    {
        string strReturn = "";
        con= new SqlConnection(cn);
        try
        {
           
            con.Open();
            SqlCommand cmd = new SqlCommand(strScript);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            //conn.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }


    public DataTable getDataTableByQuery(string strQry)
    {
        DataTable dt = new DataTable();
        DataBaseClassSql db = new DataBaseClassSql();
        //string qry = "SELECT DPT_ID FROM PR_EMPLOYEE_LIST WHERE EMP_ID='" + empid + "'";
        dt = db.ConnectDataBaseReturnDT(strQry);
        return dt;
    }
    public int GetDeptName(string deptName)
    {
        string strQuery = "SELECT SYS_DEPT_NAME FROM CM_SYSTEM_DEPT "
                            + "WHERE UPPER(SYS_DEPT_NAME) = UPPER('" + deptName + "') ";

        int getValue = 0;

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_CMP_BRANCH");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                getValue = 1;
            }

        }
        catch (Exception ex)
        {

        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return getValue;
    }
    public int GetSubjectName(string deptName,string deptId)
    {
        string strQuery = "SELECT SYS_SUB_NAME FROM CM_SYSTEM_SUB "
                            + "WHERE UPPER(SYS_SUB_NAME) = UPPER('" + deptName + "') AND SYS_DEPT_ID='"+deptId+"' ";

        int getValue = 0;

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_CMP_BRANCH");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                getValue = 1;
            }

        }
        catch (Exception ex)
        {

        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return getValue;
    }

    public string InsertDept(string deptName, string addedBy, string type)
    {
        DataBaseClassSql db = new DataBaseClassSql();
       
        string qry = "INSERT INTO CM_SYSTEM_DEPT (SYS_DEPT_NAME, DEPT_ADDED_BY, SYS_DEPT_TYPE_ID) VALUES ('" + deptName + "','" + addedBy + "','" + type + "')";
        db.ConnectDataBaseToInsert(qry);
        return "OK";
    }
    public string InsertSubject(string deptName, string addedBy, string dept,string code)
    {
        DataBaseClassSql db = new DataBaseClassSql();

        string qry = "INSERT INTO CM_SYSTEM_SUBJECT (SYS_SUB_NAME,SYS_SUB_CODE, SUB_ADDED_BY, SYS_DEPT_ID) VALUES ('" + deptName + "','" + code + "','" + addedBy + "','" + dept + "')";
        db.ConnectDataBaseToInsert(qry);
        return "OK";
    }

    public ArrayList GetDeptTypeList()
    {
        ArrayList list = new ArrayList();
        string strQuery = "Select SYS_DEPT_TYPE_ID,SYS_DEPT_TYPE from CM_SYSTEM_DEPT_TYPE";

       

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_SYSTEM_DEPT_TYPE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow prow in oDS.Tables[0].Rows)
                {
                    string temp = prow["SYS_DEPT_TYPE_ID"] + "*" + prow["SYS_DEPT_TYPE"];
                    list.Add(temp);
                }
            }

        }
        catch (Exception ex)
        {

        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return list;
    }
    public ArrayList GetDeptList()
    {
        ArrayList list = new ArrayList();
        string strQuery = "Select SYS_DEPT_ID,SYS_DEPT_NAME from CM_SYSTEM_DEPT";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_SYSTEM_DEPT_TYPE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow prow in oDS.Tables[0].Rows)
                {
                    string temp = prow["SYS_DEPT_ID"] + "*" + prow["SYS_DEPT_NAME"];
                    list.Add(temp);
                }
            }

        }
        catch (Exception ex)
        {

        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return list;
    }
    public ArrayList GetDeptListByType(string typeId)
    {
        ArrayList list = new ArrayList();
        string strQuery = "Select SYS_DEPT_ID,SYS_DEPT_NAME from CM_SYSTEM_DEPT WHERE SYS_DEPT_TYPE_ID='"+typeId+"'";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_SYSTEM_DEPT_TYPE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow prow in oDS.Tables[0].Rows)
                {
                    string temp = prow["SYS_DEPT_ID"] + "*" + prow["SYS_DEPT_NAME"];
                    list.Add(temp);
                }
            }

        }
        catch (Exception ex)
        {

        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return list;
    }
    public ArrayList GetSubListByDept(string typeId)
    {
        ArrayList list = new ArrayList();
        string strQuery = "Select SYS_SUB_ID,SYS_SUB_NAME from CM_SYSTEM_SUBJECT WHERE SYS_DEPT_ID='" + typeId + "'";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_SYSTEM_DEPT_TYPE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow prow in oDS.Tables[0].Rows)
                {
                    string temp = prow["SYS_SUB_ID"] + "*" + prow["SYS_SUB_NAME"];
                    list.Add(temp);
                }
            }

        }
        catch (Exception ex)
        {

        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return list;
    }
    public string InsertQuestion(string title,string a,string b,string c,string d,string cr,string sub, string addedBy)
    {
        DataBaseClassSql db = new DataBaseClassSql();

        string qry = "INSERT INTO CM_SYSTEM_QUESTION (SYS_QUES_NAME,SYS_OPTION_A,SYS_OPTION_B,SYS_OPTION_C,SYS_OPTION_D,CORRECT_OPTION,SYS_SUB_ID, QUES_ADDED_BY) VALUES ('" + title + "','" + a + "','" + b + "','" + c + "','" + d + "','" + cr + "','" + sub + "','" + addedBy + "')";
        db.ConnectDataBaseToInsert(qry);
        return "OK";
    }

    public string InsertQuestions(string sub, string addedBy)
    {

        string strQuery = "Select * from CM_SYSTEM_QUESTION_EXCEL";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_SYSTEM_QUESTION");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow prow in oDS.Tables[0].Rows)
                {
                    string t = prow["SYS_QUES_NAME"].ToString();
                    string a = prow["SYS_OPTION_A"].ToString();
                    string b = prow["SYS_OPTION_B"].ToString();
                    string c = prow["SYS_OPTION_C"].ToString();
                    string d = prow["SYS_OPTION_D"].ToString();
                    string cr = prow["CORRECT_OPTION"].ToString();
                    InsertQuestion(t, a, b, c, d, cr, sub, addedBy);

                }
            }

        }
        catch (Exception ex)
        {

        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return "Saved successfully";
    }


    public string deleteDataFromTable(string tableName, string priMaryId,string primaryValue)
    {
        string strSql;
        strSql = "DELETE from "+tableName+" WHERE "+priMaryId+" = '" + primaryValue + "'";

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();

            return "Deleted successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public ArrayList GetGroupList()
    {
        ArrayList list = new ArrayList();
        string strQuery = "Select SYS_USR_GRP_ID,SYS_USR_GRP_TITLE from CM_SYSTEM_USER_GROUP";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_SYSTEM_DEPT_TYPE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow prow in oDS.Tables[0].Rows)
                {
                    string temp = prow["SYS_USR_GRP_ID"] + "*" + prow["SYS_USR_GRP_TITLE"];
                    list.Add(temp);
                }
            }

        }
        catch (Exception ex)
        {

        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return list;
    }

    public string InsertUser(string name, string login, string group, string pass)
    {
        DataBaseClassSql db = new DataBaseClassSql();

        string qry = "INSERT INTO CM_SYSTEM_USERS (SYS_USR_DNAME,SYS_USR_LOGIN_NAME, SYS_USR_GRP_ID, SYS_USR_PASS,CMP_BRANCH_ID) VALUES ('" + name + "','" + login + "','" + group + "','" + pass + "','100310003')";
        db.ConnectDataBaseToInsert(qry);
        return "OK";
    }

    public string deleteDataFromExcelTable()
    {
        string strSql;
        strSql = "DELETE from CM_SYSTEM_QUESTION_EXCEL";

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();

            return "Deleted successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertExcelTableToMainTable(string subId,string userName)
    {
        string strSql;
        strSql = "INSERT INTO [QB_DB].[dbo].[CM_SYSTEM_QUESTION]  ( [SYS_QUES_NAME] ,[SYS_OPTION_A] ,[SYS_OPTION_B] ,[SYS_OPTION_C] ,[SYS_OPTION_D] ,[CORRECT_OPTION] ,[SYS_SUB_ID] ,[QUES_ADDED_BY] ) SELECT [SYS_QUES_NAME] ,[SYS_OPTION_A] ,[SYS_OPTION_B] ,[SYS_OPTION_C] ,[SYS_OPTION_D] ,[CORRECT_OPTION] ,'"+subId+"','"+userName+"' FROM [QB_DB].[dbo].[CM_SYSTEM_QUESTION_EXCEL]";

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();

            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
 

}


