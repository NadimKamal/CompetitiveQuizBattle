using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

/// <summary>
/// Summary description for clsSystemAdmin
/// </summary>
public class clsSystemAdmin
{
    //private string strConString = ConfigurationSettings.AppSettings["dbConnectionString"];
    private SqlTransaction dbTransaction = null;
    SqlConnection con;
    string cn = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
    public clsSystemAdmin()
    {
       
    }

    public string GetbranchCom(string cID)
    {
        string strSql = "SELECT CMP_BRANCH_ID, CMP_BRANCH_NAME FROM CM_CMP_BRANCH WHERE CMP_COMPANY_ID='" + cID + "' ORDER BY CMP_BRANCH_ID ";
        SqlConnection conn = new SqlConnection(cn);
        DataSet oDs = new DataSet();

        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, conn));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "CMP_BRANCH_NAME");

        }
        catch (Exception)
        {


        }
        finally
        {
            conn.Close();
            conn = null;
        }
        DataRow dRow = oDs.Tables["CMP_BRANCH_NAME"].Rows[0];
        return strSql;
    }
    public string InsertSysAccPolicy(string grpID, string strMenuId, string view, string add, string del, string edit, string print, string search)
    {
        try  /*  asad  */
        {
            string strSql = "SELECT SYS_USR_GRP_ID,SYS_MENU_ID,SYS_ACCP_VIEW,SYS_ACCP_ADD,SYS_ACCP_DELETE,SYS_ACCP_EDIT,SYS_ACCP_PRINT,SYS_ACCP_SEARCH FROM CM_SYSTEM_ACCESS_POLICY  ";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "CM_SYSTEM_ACCESS_POLICY");
            // Insert Data
            oOrderRow = oDs.Tables["CM_SYSTEM_ACCESS_POLICY"].NewRow();

            oOrderRow["SYS_USR_GRP_ID"] = grpID;
            oOrderRow["SYS_MENU_ID"] = strMenuId;
            oOrderRow["SYS_ACCP_VIEW"] = view;
            oOrderRow["SYS_ACCP_ADD"] = add;
            oOrderRow["SYS_ACCP_DELETE"] = del;
            oOrderRow["SYS_ACCP_EDIT"] = edit;
            oOrderRow["SYS_ACCP_PRINT"] = print;
            oOrderRow["SYS_ACCP_SEARCH"] = print;

            oDs.Tables["CM_SYSTEM_ACCESS_POLICY"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "CM_SYSTEM_ACCESS_POLICY");
            dbTransaction.Commit();
        }
        catch (Exception ex)
        {
            return "";
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "success";
    }

    public DataSet LoginWithUserName(string login_name, string password)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = "SELECT SU.*,CB.CMP_BRANCH_TYPE_ID FROM CM_SYSTEM_USERS SU, CM_CMP_BRANCH CB WHERE  SU.CMP_BRANCH_ID=CB.CMP_BRANCH_ID  AND  LTRIM(RTRIM(UPPER(SU.SYS_USR_LOGIN_NAME))) = LTRIM(RTRIM(UPPER('" + login_name + "')))  AND LTRIM(RTRIM(UPPER(SU.SYS_USR_PASS))) = LTRIM(RTRIM(UPPER('" + password + "')))    ";
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaFrmLogin = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaFrmLogin.Fill(oDS, "CM_SYSTEM_USERS");
            return oDS;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet LoginActiveDirectoryId(string ActiveDirectory_login_name)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = "SELECT SU.*,CB.CMP_BRANCH_TYPE_ID FROM CM_SYSTEM_USERS SU, CM_CMP_BRANCH CB WHERE  SU.CMP_BRANCH_ID=CB.CMP_BRANCH_ID  AND  LTRIM(RTRIM(UPPER(SU.ACTIVE_DIRECTORY_LOGIN_NAME))) = LTRIM(RTRIM(UPPER('" + ActiveDirectory_login_name + "')))  ";
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaFrmLogin = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaFrmLogin.Fill(oDS, "CM_SYSTEM_USERS");
            return oDS;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string GetBranchId(string LoginId)
    {
        string branchId = "";
        string strSql = " SELECT CMP_BRANCH_ID FROM CM_SYSTEM_USERS WHERE SYS_USR_LOGIN_NAME = '" + LoginId + "' ";
        SqlConnection conn = new SqlConnection(cn);
        conn.Open();

        try
        {
            SqlCommand cmd = new SqlCommand(strSql, conn);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    branchId = dr["CMP_BRANCH_ID"].ToString();
                }
                return branchId;
            }
            else
            {
                return branchId;

            }
            conn.Close();
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
            conn.Close();
        }
    }


    public string GetNotActiveDirectoryEmpList(string LoginId,string BranchId)
    {
        string branchId = "";
        string strSql = " SELECT * FROM HR_NOT_ACTIVE_DIRECTORY_LOGIN WHERE LOGIN_NAME = '" + LoginId + "' and CMP_BRANCH_ID='" + BranchId + "' ";
        SqlConnection conn = new SqlConnection(cn);
        conn.Open();

        try
        {
            SqlCommand cmd = new SqlCommand(strSql, conn);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    branchId = dr["LOGIN_NAME"].ToString();
                }
                return branchId;
            }
            else
            {
                return branchId;

            }
            conn.Close();
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
            conn.Close();
        }
    }

    public DataSet LoginGroupUser(string userID)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = " SELECT SG.SYS_USR_GRP_TITLE,SG.SYS_USR_GRP_PARENT,SG.SYS_USR_GRP_TYPE,SG.CMP_BRANCH_ID,SG.POSCL_ID,SG.SYS_USR_GRP_ID, "
                         + " SU.SYS_USR_ID,SU.SYS_USR_DNAME,SU.SYS_USR_LOGIN_NAME,SU.SYS_USR_PASS,SU.SYS_USR_EMAIL,CB.CMP_BRANCH_TYPE_ID "
                         + " FROM CM_SYSTEM_USER_GROUP SG,CM_SYSTEM_USERS SU,CM_CMP_BRANCH CB "
                         + " WHERE SG.SYS_USR_GRP_ID=SU.SYS_USR_GRP_ID AND CB.CMP_BRANCH_ID=SG.CMP_BRANCH_ID "
                         + " AND CB.CMP_BRANCH_ID=SU.CMP_BRANCH_ID AND SU.SYS_USR_ID='" + userID + "' ";
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaFrmLogin = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaFrmLogin.Fill(oDS, "CM_SYSTEM_USERS_GROUP");
            return oDS;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #region Passwored request
    public string getUserId(string login_name)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = "SELECT SYS_USR_ID FROM CM_SYSTEM_USERS  SU WHERE SU.SYS_USR_LOGIN_NAME = '" + login_name + "'  ";
        string UsrId = "";
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaFrmLogin = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaFrmLogin.Fill(oDS, "CM_SYSTEM_USERS");
            UsrId = oDS.Tables[0].Rows[0]["SYS_USR_ID"].ToString();
        }
        catch (Exception e)
        {
            return "";   
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return UsrId;
    }
    public DataSet getUsrInfo(string userID)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = " select * from PR_EMPLOYEE_LIST where SYS_USR_ID='" + userID + "' ";
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaFrmLogin = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaFrmLogin.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public int ChkSentReqNew(string userID)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = " SELECT COUNT(*)TOTAL FROM CM_SYSTEM_PASSWORD_REQUEST "   // check today and previous 6 days 
                           +" WHERE ( "
                           +"  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23),   PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23),   GETDATE()))) "
			               +" OR convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()-1))) "
			               +" OR convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()-2))) "
			               +" OR convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()-3))) "
			               +" OR convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()-4))) "
			               +" OR convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()-5))) "
                           +" OR convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()-6))) "
                           + " ) AND SYS_USR_ID='" + userID + "' AND PASS_REQ_SENT='N' ";
            
        int exist = 0;
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaFrmLogin = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaFrmLogin.Fill(oDS, "CM_SYSTEM_PASSWORD_REQUEST");
            exist = int.Parse(oDS.Tables[0].Rows[0]["TOTAL"].ToString());
        }
        catch (Exception e)
        {
            return 0;  
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return exist;
    }
    public int ChkSentReqApproved(string userID)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = " SELECT COUNT(*)TOTAL FROM CM_SYSTEM_PASSWORD_REQUEST "   // check today and previous 6 days 
                           + " WHERE ( "
                           + "  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()))) "
                           + " OR convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()-1))) "
                           + " OR convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()-2))) "
                           + " OR convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()-3))) "
                           + " OR convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()-4))) "
                           + " OR convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()-5))) "
                           + " OR convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PASS_REQ_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), GETDATE()-6))) "
                           + " ) AND SYS_USR_ID='" + userID + "' AND PASS_REQ_SENT='Y' ";

        int exist = 0;
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaFrmLogin = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaFrmLogin.Fill(oDS, "CM_SYSTEM_PASSWORD_REQUEST");
            exist = int.Parse(oDS.Tables[0].Rows[0]["TOTAL"].ToString());
        }
        catch (Exception e)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return exist;
    }
    public int ChkSentReq(string userID)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = " SELECT COUNT(*)TOTAL FROM CM_SYSTEM_PASSWORD_REQUEST "   
                       +" WHERE  SYS_USR_ID='" + userID + "'  ";

        int exist = 0;
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaFrmLogin = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaFrmLogin.Fill(oDS, "CM_SYSTEM_PASSWORD_REQUEST");
            exist = int.Parse(oDS.Tables[0].Rows[0]["TOTAL"].ToString());
        }
        catch (Exception e)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return exist;
    }
    public DataSet SentReqInfo(string userID)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = " SELECT * FROM CM_SYSTEM_PASSWORD_REQUEST "
                       +" WHERE  SYS_USR_ID='" + userID + "'  ";
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaFrmLogin = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaFrmLogin.Fill(oDS, "CM_SYSTEM_PASSWORD_REQUEST");
            return oDS;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string UpdatePassReq(string usrId,int frequency,string email)
    {
        string updateString;
        updateString = "UPDATE CM_SYSTEM_PASSWORD_REQUEST SET PASS_REQ_DATE=GETDATE(), PASS_REQ_FREQUENCY='" + frequency + "', PASS_REQ_SENT='N', EMP_EMAIL='"+email+"'  WHERE SYS_USR_ID='" + usrId + "'  ";
        string strReturn = "success";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            con.Close();
        }
        catch (Exception ex)
        {
            strReturn = "";
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
    public string UpdatePassReqStatus(string RequestId)
    {
        string updateString;
        updateString = "UPDATE CM_SYSTEM_PASSWORD_REQUEST SET PASS_REQ_SENT='Y'  WHERE PASS_REQ_ID='" + RequestId + "'  ";
        string strReturn = "success";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            con.Close();
        }
        catch (Exception ex)
        {
            strReturn = "";
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
    public string InsertPassReq(string UsrId,string empId,string dptId,string dsgId,string email)
    {
        try  /*  asad  */
        {
            string strSql = "SELECT SYS_USR_ID,EMP_ID,DPT_ID,DSG_ID_MAIN,EMP_EMAIL FROM CM_SYSTEM_PASSWORD_REQUEST  ";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "CM_SYSTEM_PASSWORD_REQUEST");
            // Insert Data
            oOrderRow = oDs.Tables["CM_SYSTEM_PASSWORD_REQUEST"].NewRow();

            oOrderRow["SYS_USR_ID"] = UsrId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["DPT_ID"] = dptId;
            oOrderRow["DSG_ID_MAIN"] = dsgId;
            oOrderRow["EMP_EMAIL"] = email;
           

            oDs.Tables["CM_SYSTEM_PASSWORD_REQUEST"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "CM_SYSTEM_PASSWORD_REQUEST");
            dbTransaction.Commit();
        }
        catch (Exception ex)
        {
            return "";
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "success";
    }

#endregion
    public DataSet GetRootMenu()
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = "";
        strSql = "SELECT * FROM CM_SYSTEM_MENU WHERE SYS_MENU_TYPE='RT' ORDER BY SYS_MENU_SERIAL";
        try
        {
           con = new SqlConnection(cn);
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            oOrdersDataAdapter.Fill(oDS, "CM_SYSTEM_MENU");
            return oDS;
        }
        catch (Exception e)
        {
            strSql = e.Message.ToString();
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetSystemUsers(string strBranch)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql;
        strSql = "SELECT * FROM CM_SYSTEM_USERS WHERE CMP_BRANCH_ID='" + strBranch + "' ORDER BY SYS_USR_DNAME";
        try
        {
           con = new SqlConnection(cn);
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            oOrdersDataAdapter.Fill(oDS, "CM_SYSTEM_USERS");
            return oDS;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetChildMenu(string strParentMenu, string strGroupID)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql;
        strSql = "SELECT DISTINCT "
               + "M.SYS_MENU_ID, SYS_MENU_TITLE, SYS_MENU_FILE,"
               + "SYS_MENU_PARENT, CMP_BRANCH_ID, SYS_MENU_TYPE,"
               + "SYS_MENU_SERIAL "
               + "FROM CM_SYSTEM_MENU M,CM_SYSTEM_ACCESS_POLICY P "
               + "WHERE P.SYS_MENU_ID=M.SYS_MENU_ID AND M.SYS_MENU_PARENT='" + strParentMenu + "' AND P.SYS_ACCP_VIEW = 'Y' "
                + "AND M.SYS_MENU_PARENT<> M.SYS_MENU_ID AND P.SYS_USR_GRP_ID = '" + strGroupID + "' ORDER BY SYS_MENU_SERIAL";
        try
        {
           con = new SqlConnection(cn);
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            oOrdersDataAdapter.Fill(oDS, "CM_SYSTEM_MENU");
            return oDS;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public void SetSeessionData(string strBranchId)
    {
        //string strSQL = "exec pkg_erp_admin_services.SET_BRANCH_ID('" + strBranchId + "')";
        string strSQL = "exec SET_BRANCH_ID('" + strBranchId + "')";
        con = new SqlConnection(cn);
        con.Open();
        try
        {
            SqlCommand objComman = new SqlCommand(strSQL);
            objComman.CommandText = "SET_BRANCH_ID";
            objComman.CommandType = CommandType.StoredProcedure;
            objComman.Parameters.Add("inBRANCH_ID", OleDbType.VarChar).Value = strBranchId;
            //SqlCommand objComman = new SqlCommand(strSQL);
            objComman.Connection = con;
            objComman.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            string strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    public void AddAuditLog(string strUsr_ID, string strOperationType, string strHost, string strModule, string strRemarks)//string strMenu_Id
    {
        string query;
        //##########################################################
        con = new SqlConnection(cn);
        con.Open(); // 1. Instantiate a new command with command text only
        try
        {
            query = @"INSERT INTO CM_SYSTEM_AUDIT (SYS_USR_ID, OPERATION_TYPE,HOST,MODULE,REMARKS) "
                  + "VALUES ('" + strUsr_ID + "', '" + strOperationType + "','" + strHost + "','" + strModule + "','" + strRemarks + "')";
            SqlCommand olcmd = new SqlCommand(query); // 2. Set the Connection property
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();// 3. Call ExecuteNonQuery to send command
        }
        catch (Exception)
        {
            
            throw;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
      
        //########################################################
    }

    public string GetCurrentPageName()
    {
        string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
        System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
        string FileName = oInfo.Name;
        return FileName;
    }


    // ############## START:  Code Added by MUNIR on 26-04-2010 in order to check data
    //when a user write his/her Password on the Login Form  :START ##################
    public DataSet FrmLogin(string loginid, string password)
    {

        string strSql = "SELECT SU.*,CB.CMP_BRANCH_TYPE_ID FROM CM_SYSTEM_USERS SU, CM_CMP_BRANCH CB WHERE SU.CMP_BRANCH_ID=CB.CMP_BRANCH_ID AND SU.SYS_USR_ID = '" + loginid + "' AND SU.SYS_USR_PASS = '" + password + "'";
        //string retint;
        try
        {
            con = new SqlConnection(cn);
            DataSet oDS = new DataSet();
            SqlDataAdapter odaFrmLogin = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaFrmLogin.Fill(oDS, "CM_SYSTEM_USERS");
            return oDS;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    // ############## END:  Code Added by MUNIR on 26-04-2010 in order to check data
    //when a user write his/her Password on the Login Form  :END ##################

    public DataSet GetSysMenu()
    {
        string sqlCommand = "SELECT Distinct SYS_MENU_ID "
                           + "FROM CM_SYSTEM_MENU ";

        try
        {
            con = new SqlConnection(cn);
            DataSet oDS = new DataSet();
            SqlDataAdapter odaSysMenu = new SqlDataAdapter(new SqlCommand(sqlCommand, con));
            odaSysMenu.Fill(oDS, "CM_SYSTEM_MENU");
            return oDS;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public void AddAllMenu(string strUsr_Grp_ID)//string strMenu_Id
    {
        string strSql = "";
        string query;
        DataSet oDSfull = new DataSet();
        DataSet oDSremain = new DataSet();

        oDSfull = GetSysMenu();

       con = new SqlConnection(cn);
       try
       {
           con.Open(); // 1. Instantiate a new command with command text only

           foreach (DataRow prow in oDSfull.Tables["CM_SYSTEM_MENU"].Rows)
           {
               query = @"INSERT INTO CM_SYSTEM_ACCESS_POLICY (SYS_USR_GRP_ID, SYS_MENU_ID) "
                      + "VALUES ('" + strUsr_Grp_ID + "', '" + prow["SYS_MENU_ID"].ToString() + "')";

               SqlCommand olcmd = new SqlCommand(query); // 2. Set the Connection property
               olcmd.Connection = con;
               olcmd.ExecuteNonQuery();// 3. Call ExecuteNonQuery to send command

           }
       }
       catch (Exception e)
       {

           e.Message.ToString();
       }
       finally
       {
           con.Close();
           con.Dispose();
           SqlConnection.ClearPool(con);
           con = null;
       }

    }




    // ############## Start:  Function Created by MUNIR on 28-04-2010 in order Count Dataset :Start ##################

    public DataSet CountSysMenu(string strUsr_Grp_ID)
    {
        string sqlCommand = "SELECT * FROM CM_SYSTEM_ACCESS_POLICY WHERE SYS_USR_GRP_ID = '" + strUsr_Grp_ID + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaSysMenu = new SqlDataAdapter(new SqlCommand(sqlCommand, con));
            odaSysMenu.Fill(oDS, "CM_SYSTEM_ACCESS_POLICY");
            return oDS;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    // ############## END:  Function Created by MUNIR on 28-04-2010 in order Count Dataset :END ##################



    public string sChangePassword(string strUserID, string strNewPass, string strName)
    {
        string strSql = "";

        
        
            strSql = "SELECT SYS_USR_PASS, SYS_USR_ID, SYS_USR_LOGIN_NAME FROM CM_SYSTEM_USERS  "
                      + "WHERE SYS_USR_ID = '" + strUserID + "'  ";
        

        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oDbAdapter.Fill(oDs, "CM_SYSTEM_USERS");


            
            oOrderRow = oDs.Tables["CM_SYSTEM_USERS"].Rows[0];
            

            oOrderRow["SYS_USR_PASS"] = strNewPass.Replace(" ", "");
            oOrderRow["SYS_USR_LOGIN_NAME"] = strName;



            
            oDbAdapter.Update(oDs, "CM_SYSTEM_USERS");
            return "Password Changed";
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
    public string InsertWorkflowSetup(string branch, string mainType, string Item, string SettelType, string Deprtment, string SorPer,
                              string SorDegistion, string DesEmpId, string DesDesEmpId, string Description)
    {
        string strSql;
        strSql = "SELECT CMP_BRANCH_ID, DESTINATION_EMP_ID, MAIN_TYPE_ID, WS_SETTLED_TYPE, SOURCE_EMP_ID, DPT_ID, WS_DESCRIPTION, ITEM_SET_CODE, SOURCE_DSG_ID, DESTINATION_DSG_ID FROM CM_WORKFLOW_SETUP ";

        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            //######################################################
            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "CM_WORKFLOW_SETUP";
            //######################################################
            oOrderRow = oDS.Tables["CM_WORKFLOW_SETUP"].NewRow();
            oOrderRow["CMP_BRANCH_ID"] = branch;
            oOrderRow["MAIN_TYPE_ID"] = mainType;
            oOrderRow["ITEM_SET_CODE"] = Item;
            oOrderRow["WS_SETTLED_TYPE"] = SettelType;
            oOrderRow["DPT_ID"] = Deprtment;
            oOrderRow["SOURCE_EMP_ID"] = SorPer;
            oOrderRow["SOURCE_DSG_ID"] = SorDegistion;
            oOrderRow["DESTINATION_EMP_ID"] = DesEmpId;
            oOrderRow["DESTINATION_DSG_ID"] = DesDesEmpId;
            oOrderRow["WS_DESCRIPTION"] = Description;
            //#########################################################
            oDS.Tables["CM_WORKFLOW_SETUP"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "CM_WORKFLOW_SETUP");
            dbTransaction.Commit();
            return "Saved Successfully.";
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


    public bool CheckDuplicate( string mainType, string Item, string Deprtment,string selectedEmployees)  /* ASAD  */
    {
        string strSql;
        strSql = "SELECT DISTINCT  DESTINATION_EMP_ID FROM CM_WORKFLOW_SETUP WHERE MAIN_TYPE_ID='" + mainType + "' AND ITEM_SET_CODE='" + Item + "' AND DPT_ID= '" + Deprtment + "'  and DESTINATION_EMP_ID = '" + selectedEmployees + "' ";
        DataSet oDS = new DataSet();
        bool result = false;
        try
        {
            con = new SqlConnection(cn);
            SqlDataAdapter odaFrmLogin = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaFrmLogin.Fill(oDS, "CM_WORKFLOW_SETUP");
            int total = oDS.Tables[0].Rows.Count;
            if ( total > 0)
            {
                result = true;
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
        return result;
    }



    public string SetConnectionString()
    {
        /*
        string strError = objLicense.CheckLicense();
        if (strError.Equals(""))
        {
            string strConString = "";
            string strProvider = "DataProtectionConfigurationProvider";
            strConString = MiT_License.clsDBConnectionReadWrite.GetTelConnectionString();
           // ############## Change conenction string in connectionStrings################
            var configuration = WebConfigurationManager.OpenWebConfiguration("~");
            var section = (ConnectionStringsSection)configuration.GetSection("connectionStrings");
            section.ConnectionStrings["oracleConString"].ConnectionString = strConString;
            section.SectionInformation.ProtectSection(strProvider);
           // #############  Change conenction string in AppSetting #######################
            configuration.AppSettings.Settings["dbConnectionString"].Value = strConString;
            configuration.AppSettings.SectionInformation.ProtectSection(strProvider);
            configuration.Save();
        }
        return strError;
        */
        return "";
    }
}
