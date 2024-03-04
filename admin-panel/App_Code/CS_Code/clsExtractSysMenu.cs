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

/// <summary>
/// Summary description for clsSystemAdmin
/// </summary>
public class clsExtractSysMenu
{
    private SqlTransaction dbTransaction = null;
    string cn = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
    SqlConnection con;

    public clsExtractSysMenu()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public DataSet GetRootMenu()
    {
        string strSql = "";
        strSql = "SELECT * FROM CM_SYSTEM_MENU WHERE SYS_MENU_TYPE='RT' ORDER BY SYS_MENU_SERIAL";
        con = new SqlConnection(cn);
        DataSet oDS = new DataSet();
        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            oOrdersDataAdapter.Fill(oDS, "CM_SYSTEM_MENU");
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
        return oDS;
    }

    public DataSet GetChildMenu(string strParentMenu,string strGroupID)
    {
        string strSql;
        strSql = "SELECT DISTINCT "
               + "M.SYS_MENU_ID, SYS_MENU_TITLE, SYS_MENU_FILE," 
               + "SYS_MENU_PARENT, CMP_BRANCH_ID, SYS_MENU_TYPE," 
               + "SYS_MENU_SERIAL "
               + "FROM CM_SYSTEM_MENU M,CM_SYSTEM_ACCESS_POLICY P "
               + "WHERE P.SYS_MENU_ID=M.SYS_MENU_ID AND M.SYS_MENU_PARENT='" + strParentMenu + "' AND P.SYS_ACCP_VIEW = 'Y' "
                + "AND M.SYS_MENU_PARENT<> M.SYS_MENU_ID AND P.SYS_USR_GRP_ID = '" + strGroupID + "' ORDER BY SYS_MENU_SERIAL";
        con = new SqlConnection(cn);
        DataSet oDS = new DataSet();
        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            oOrdersDataAdapter.Fill(oDS, "CM_SYSTEM_MENU");
          
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
        return oDS;
    }


    
    // ############## START:  Code Added by MUNIR on 26-04-2010 in order to check data
    //when a user write his/her Password on the Login Form  :START ##################
    public DataSet FrmLogin(string loginid, string password)
    {

        string strSql = "SELECT SU.*,CB.CMP_BRANCH_TYPE_ID FROM CM_SYSTEM_USERS SU, CM_CMP_BRANCH CB WHERE SU.CMP_BRANCH_ID=CB.CMP_BRANCH_ID AND SU.SYS_USR_ID = '" + loginid + "' AND SU.SYS_USR_PASS = '" + password + "'";
        //string retint;
         con = new SqlConnection(cn);
        DataSet oDS = new DataSet();
        try
        {
           
            SqlDataAdapter odaFrmLogin = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaFrmLogin.Fill(oDS, "CM_SYSTEM_USERS");
            
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
        return oDS;

    }

    // ############## END:  Code Added by MUNIR on 26-04-2010 in order to check data
    //when a user write his/her Password on the Login Form  :END ##################

    public DataSet GetSysMenu()
    {
        string sqlCommand = "SELECT Distinct SYS_MENU_ID "
                           + "FROM CM_SYSTEM_MENU ";
         con = new SqlConnection(cn);
        DataSet oDS = new DataSet();
        try
        {
           
            SqlDataAdapter odaSysMenu = new SqlDataAdapter(new SqlCommand(sqlCommand, con));
            odaSysMenu.Fill(oDS, "CM_SYSTEM_MENU");
                     
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
        return oDS;
    } 

    public void AddAllMenu(string strUsr_Grp_ID)//string strMenu_Id
    {
        string strSql = "";
        string query;
        DataSet oDSfull = new DataSet();
        DataSet oDSremain = new DataSet();

        oDSfull = GetSysMenu();

        con = new SqlConnection(cn);
        con.Open();

        try
        {
            dbTransaction = con.BeginTransaction();

            //SqlConnection con = new SqlConnection(cn);
            //con.Open(); // 1. Instantiate a new command with command text only

            foreach (DataRow prow in oDSfull.Tables["CM_SYSTEM_MENU"].Rows)
            {
                query = @"INSERT INTO CM_SYSTEM_ACCESS_POLICY (SYS_USR_GRP_ID, SYS_MENU_ID) "
                       + "VALUES ('" + strUsr_Grp_ID + "', '" + prow["SYS_MENU_ID"].ToString() + "')";

                SqlCommand olcmd = new SqlCommand(query, con, dbTransaction); // 2. Set the Connection property
                //olcmd.Connection = con;
                olcmd.ExecuteNonQuery();// 3. Call ExecuteNonQuery to send command

            }
            dbTransaction.Commit();
        }
        catch (Exception)
        {
            
            
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
         con = new SqlConnection(cn);
        DataSet oDS = new DataSet();
        try
        {
            
            SqlDataAdapter odaSysMenu = new SqlDataAdapter(new SqlCommand(sqlCommand, con));
            odaSysMenu.Fill(oDS, "CM_SYSTEM_ACCESS_POLICY");
            
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
        return oDS;
    }

    // ############## END:  Function Created by MUNIR on 28-04-2010 in order Count Dataset :END ##################
}
