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
using System.Security.Cryptography;
using System.IO;
using System.Text;

/// <summary>
/// Summary description for clsGlobalSetup
/// </summary>
public class clsGlobalSetup
{
    // private string strConString = ConfigurationSettings.AppSettings["dbConnectionString"];
    SqlTransaction dbTransaction = null;
    private string cn = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
    SqlConnection con;
    public clsGlobalSetup()
    {
        //var cn = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
        //con = new SqlConnection(cn);
        //con.Open();
    }


    public string GetCompanyName()
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = "SELECT * FROM CM_COMPANY ORDER BY COMPANY_ID";
        DataSet oDs = new DataSet();
       
        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "CM_COMPANY");

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
        DataRow dRow = oDs.Tables["CM_COMPANY"].Rows[0];
        return dRow["COMPANY_NAME"].ToString();
    }
    public string GetLogoUrl( string branchId)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = "SELECT * FROM CM_COMPANY As CM ,CM_CMP_BRANCH as BR where CM.COMPANY_ID=BR.CMP_COMPANY_ID and BR.CMP_BRANCH_ID='" + branchId + "' ORDER BY COMPANY_ID";
        DataSet oDs = new DataSet();

        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "CM_COMPANY");

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
        DataRow dRow = oDs.Tables["CM_COMPANY"].Rows[0];
        return dRow["IMAGES_PATH"].ToString();
    }
    public string GetCompanyName( string branch)
    {
        con = new SqlConnection(cn);
        con.Open();
        string brnachId = "";
        if(branch=="")
        {
            brnachId = "";
        }
        else
        {
            brnachId = " where  BR. CMP_BRANCH_ID ='" + branch + "'";
        }

        string strSql = @"SELECT * FROM  CM_CMP_BRANCH BR 
         left join  CM_COMPANY as CM on BR.CMP_COMPANY_ID= CM.COMPANY_ID
      " + brnachId + "  ORDER BY COMPANY_ID ";
        DataSet oDs = new DataSet();

        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "CM_COMPANY");

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
        DataRow dRow = oDs.Tables["CM_COMPANY"].Rows[0];
        return dRow["COMPANY_NAME"].ToString();
    }    

    #region Dynamic Login & Welcome Page Info
    public DataSet GetSolnNameAndCpyRtInfo() /*** added by : Rezoana, 11-02-2013 ***/
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql;
        DataSet oDS = new DataSet();
        strSql = "Select SOLN_NAME_BFR_LOGIN,COPYRIGHT_INFO,TITLE_BFR_LOGIN from CM_SYSTEM_INFO";
        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            oOrdersDataAdapter.Fill(oDS, "CM_SYSTEM_INFO");
           
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
    public DataSet GetSlonName_Aft_Login( string branchId) /*** added by : Rezoana, 11-02-2013 ***/
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = "SELECT SOLN_NAME_AFTR_LOGIN,TITLE_AFTR_LOGIN from CM_SYSTEM_INFO where CMP_BRANCH_ID='" + branchId + "'";
        DataSet oDs = new DataSet();
        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "CM_SYSTEM_INFO");
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
        return oDs;
        //DataRow dRow = oDs.Tables["CM_SYSTEM_INFO"].Rows[0];
        //return dRow["SOLN_NAME_AFTR_LOGIN"].ToString();
    }    
    public string GetWelcmMsg( string branchId) /*** added by : Rezoana, 11-02-2013 ***/
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = "SELECT WELCOME_MSG from CM_SYSTEM_INFO where CMP_BRANCH_ID='"+branchId+"'";
       
        DataSet oDs = new DataSet();
        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "CM_SYSTEM_INFO");
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
        DataRow dRow = oDs.Tables["CM_SYSTEM_INFO"].Rows[0];
        return dRow["WELCOME_MSG"].ToString();
    }


    public string GetCompanyBanglaName(string branchId) /*** polash ***/
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = "SELECT COMPANY_NAME_BANGLA from CM_COMPANY as CM,CM_CMP_BRANCH as BR where CM.COMPANY_ID=BR.CMP_COMPANY_ID and  BR.CMP_BRANCH_ID='" + branchId + "'";

        DataSet oDs = new DataSet();
        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "CM_SYSTEM_INFO");
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
        DataRow dRow = oDs.Tables["CM_SYSTEM_INFO"].Rows[0];
        return dRow["COMPANY_NAME_BANGLA"].ToString();
    }
    #endregion

    #region Birthday Reminder
    public DataSet GetActualBday(string sysDate,string branchId) 
    {
        con = new SqlConnection(cn);
        con.Open();
         string strSql;
        DataSet oDS = new DataSet();
        strSql = @" select DS.DSG_TITLE, EL.DSG_ID,EL.DSG_ID_MAIN,EL.DPT_ID, EMP_ID,EL.EMP_CODE, EMP_TITLE, EMP_NAME,ACTUAL_BIRTHDAY, EMP_NAME, MAIL_SEND_STATUS FROM PR_EMPLOYEE_LIST EL,PR_DESIGNATION DS 
        WHERE DS.DSG_ID=EL.DSG_ID_MAIN and EL.CMP_BRANCH_ID ='" +branchId+"' and  DATEPART(d, ACTUAL_BIRTHDAY) = DATEPART(d, GETDATE()) AND DATEPART(m, ACTUAL_BIRTHDAY) = DATEPART(m, GETDATE()) ";
       
        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            oOrdersDataAdapter.Fill(oDS, "GET_BIRTHDAY");
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
    public DataSet GetActualBdaySelf(string sysDate, string EmpId) 
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql;

        DataSet oDS = new DataSet();
        strSql = " SELECT EL.DSG_ID,EL.DSG_ID_MAIN,EL.DPT_ID, EMP_ID, EMP_TITLE, EMP_NAME,ACTUAL_BIRTHDAY, EMP_NAME, MAIL_SEND_STATUS FROM PR_EMPLOYEE_LIST EL "
                +" WHERE  DATEPART(d, ACTUAL_BIRTHDAY) = DATEPART(d, GETDATE()) AND DATEPART(m, ACTUAL_BIRTHDAY) = DATEPART(m, GETDATE()) AND EMP_ID ='" + EmpId + "' ";
        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            oOrdersDataAdapter.Fill(oDS, "GET_BIRTHDAY");
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

    public DataSet GetEmailID(string bId) /*** added by : Niaz Morshed, 12-02-2013 ***/
    {
        con = new SqlConnection(cn);
        con.Open();
        DataSet oDS = new DataSet();
        string strSql;
        strSql = "SELECT EMP_EMAIL FROM PR_EMPLOYEE_LIST WHERE CMP_BRANCH_ID='" + bId + "' and EMP_EMAIL is not null   AND EMP_STATUS NOT IN (9,11)  ";
        try
        {
           
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            oOrdersDataAdapter.Fill(oDS, "EMP_EMAILID");
           
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

    public string UpdateEmailStatus(string strAsDate) /*** added by : Polash,01-31-2019 ***/
    {
        con = new SqlConnection(cn);
        con.Open();
        string updateString;

        updateString = @"UPDATE PR_EMPLOYEE_LIST SET MAIL_SEND_STATUS='Y' WHERE day(ACTUAL_BIRTHDAY) =( day('" + strAsDate + "')) and month(ACTUAL_BIRTHDAY) =( month('" + strAsDate + "')) ";

       

        string strReturn = "";
        try
        {
           
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();
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

    public string UpdateEStatusN() /*** added by : Niaz Morshed, 13-02-2013 ***/
    {
        con = new SqlConnection(cn);
        con.Open();
        string updateString;

        updateString = "UPDATE PR_EMPLOYEE_LIST SET MAIL_SEND_STATUS='N' ";
        string strReturn = "";
        try
        {
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();
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

    //public DataSet GetChaque(string sysDate) /*** added by : Niaz Morshed, 13-02-2013 ***/
    //{

    //    string strSql;
    //    strSql = "SELECT ACTUAL_BIRTHDAY FROM PR_EMPLOYEE_LIST WHERE TO_CHAR(ACTUAL_BIRTHDAY, 'DD/MM') =TO_CHAR( TO_DATE('" + sysDate + "', 'DD/MM/YYYY'),'DD/MM') ";
    //    try
    //    {
    //       
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
    //        oOrdersDataAdapter.Fill(oDS, "CHEQUE_BIRTHDAY");
    //        return oDS;
    //    }
    //    catch (Exception e)
    //    {
    //        return null;
    //    }
    //}
    #endregion

    public DataSet GetBaseCurrency()    /*** added by : Niaz Morshed, 04-Augest-2013, Decription : Select Base Currency ***/                              
    {
        con.Open();
        string strSql = "SELECT TRANS_RATE, CURRENCY_NAME FROM CM_CURRENCY WHERE BASE_CURRENCY='Y' ";
       
        DataSet oDS = new DataSet();

        try
        {
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "INV_CM_CURRENCY");
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
        return oDS;
    }

    /// <summary>
    /// added by saydur 3-31-2014
    /// to encrypt string
    /// </summary>

    public string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    /// <summary>
    /// added by saydur 3-31-2014
    /// to Decrypt string
    /// </summary>
    public string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }


    public object GetCompanyId(string branchId)
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = @"select * from CM_CMP_BRANCH as BR
inner join CM_COMPANY as CO on CO.COMPANY_ID=BR.CMP_COMPANY_ID 
where BR.CMP_BRANCH_ID='"+branchId+"' ORDER BY COMPANY_ID";
        DataSet oDs = new DataSet();

        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "CM_COMPANY");

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
        DataRow dRow = oDs.Tables["CM_COMPANY"].Rows[0];
        return dRow["COMPANY_ID"].ToString();
    }

    public string GetWelComeMsg()
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql = "SELECT * FROM CM_SYSTEM_INFO";
        DataSet oDs = new DataSet();

        try
        {
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "CM_SYSTEM_INFO");

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
        DataRow dRow = oDs.Tables["CM_SYSTEM_INFO"].Rows[0];
        return dRow["WELCOME_MSG"].ToString();
    }
}
