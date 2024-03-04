using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//using  
using System.Net;
using System.Threading;
using System.Collections.Generic;
using Ext.Net;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
public partial class frmLogin : System.Web.UI.Page
{
    clsGlobalSetup objGs = new clsGlobalSetup();
    clsSystemAdmin objSysAdmin = new clsSystemAdmin();
    string SessionId = string.Empty;
    string Flag;
    static int count = 0;

    string domainPath = "LDAP://DC=palonet,DC=org";
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.txtPassword.Text = "1234@abc";
        //this.txtUsername.Text = "0000";
        this.Form.DefaultButton = "btnLogin";
        {
            if (!objSysAdmin.SetConnectionString().Equals(""))
            {
                Response.Write("Lisence is not installed");
            }
            else
            {
                DataSet oSS = new DataSet();
                oSS = objGs.GetSolnNameAndCpyRtInfo();

                if (oSS.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow sRow in oSS.Tables[0].Rows)
                    {
                        //string SolnName = objGs.GetSolnNameAndCpyRtInfo();
                        string SolnName = sRow["SOLN_NAME_BFR_LOGIN"].ToString();
                        string CpyRtInfo = sRow["COPYRIGHT_INFO"].ToString();
                        string TitleBeforeLogin = sRow["TITLE_BFR_LOGIN"].ToString();

                        //header.InnerHtml = "<h1>" + SolnName + "</h1>";
                        Panel4.Html = "<div class='message'>&nbsp;&nbsp;</div><div id='header'><h1>" + SolnName + "</h1></div>";
                        CopyRightMsg.InnerHtml = "<h1>" + CpyRtInfo + "</h1>";
                        Title = TitleBeforeLogin; //Md.Asaduzzaman
                    }
                }
            }
        }
    }
    public string GetComputerName(string clientIP)
    {
        try
        {
            var hostEntry = Dns.GetHostEntry(clientIP);
            return hostEntry.HostName;
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }
    protected void Button1_Click(object sender, DirectEventArgs e)
    {
        // Do some Authentication...

        // Then user send to application
        Response.Redirect("Desktop.aspx");
    }
    protected void LoadUser()
    {
        //this.cmbUser.Items.Insert(0, new Coolite.Ext.Web.ListItem("None", "0"));
    }

    public bool AuthenticateUser(string user, string pass)
    {
        DirectoryEntry de = new DirectoryEntry(domainPath, user, pass, AuthenticationTypes.Secure);
        try
        {

            DirectorySearcher ds = new DirectorySearcher(de);
            ds.FindOne();
            return true;
        }
        catch (Exception ee)
        {

            return false;
        }
    }


    protected void btnLogin_Click(object sender, DirectEventArgs e)
    {
        try
        {
            clsSecurity secure = new clsSecurity();
            pnlLogin.Title = "Login";
            //----------------------
            string username = this.txtUsername.Text;
            if (secure.validateUserNAme(username))
            {
            }
            else
            {
                throw new Exception("Invalid User name");
            }
            string password = this.txtPassword.Text;

            //active directory login polash 03-01-2019


           
                string userId = "";
                clsGlobalSetup objGS = new clsGlobalSetup();
                DataSet dtsLoginUser = new DataSet();
                dtsLoginUser = objSysAdmin.LoginWithUserName(username, password);
                string UserActive = "";
                string AccountAssisgned = "no";
                if (dtsLoginUser.Tables["CM_SYSTEM_USERS"].Rows.Count == 1)
                {
                    foreach (DataRow prow in dtsLoginUser.Tables["CM_SYSTEM_USERS"].Rows)
                    {
                       
                            userId = prow["SYS_USR_ID"].ToString();
                            DataSet dsGrp = objSysAdmin.LoginGroupUser(userId);
                            if (dsGrp.Tables[0].Rows.Count > 0)
                            {
                                string groupTitle = dsGrp.Tables[0].Rows[0]["SYS_USR_GRP_TITLE"].ToString();
                                string grp = groupTitle.Substring(0, 5).ToLower().Trim();
                                
                            }

                            string branchId = prow["CMP_BRANCH_ID"].ToString();
                            Session["UserID"] = prow["SYS_USR_ID"].ToString();
                            Session["CompanyBranch"] = "N/A";
                            Session["Branch_Type"] = prow["CMP_BRANCH_TYPE_ID"].ToString();
                            Session["Branch_ID"] = prow["CMP_BRANCH_ID"].ToString();
                            Session["UserDname"] = prow["SYS_USR_DNAME"].ToString();
                            Session["UserLoginName"] = prow["SYS_USR_LOGIN_NAME"].ToString();
                            Session["Password"] = prow["SYS_USR_PASS"].ToString();
                            Session["GroupID"] = prow["SYS_USR_GRP_ID"].ToString();
                            Session["UserEmail"] = prow["SYS_USR_EMAIL"].ToString();
                            
                        
                    }


                    if (userId != "" )
                    {
                        DataSet dtsLoginUserGrp = new DataSet();
                        dtsLoginUserGrp = objSysAdmin.LoginGroupUser(userId);

                        if (dtsLoginUserGrp.Tables["CM_SYSTEM_USERS_GROUP"].Rows.Count == 1)
                        {
                            
                        }
                        Response.Redirect("frmWellcome.aspx");
                    }
                    else
                    {
                        if (UserActive == "")
                        {
                            pnlLogin.Title = "Login [Sorry, InActive account]";
                        }
                        else if (AccountAssisgned == "no")
                        {
                            pnlLogin.Title = "Login [Sorry, account not assigned to host]";
                        }
                        else
                        {
                            pnlLogin.Title = "Login [Sorry,Invalid attempt]";
                        }
                    }

                    //Response.Redirect("frmTest.aspx");
                }
                else
                {
                    pnlLogin.Title = "Login [ Sorry,Invalid attempt ]";
                }

            


        }
        catch (Exception ex)
        {

            pnlLogin.Title = "Login [ Sorry,operational error]";
        }
    }

    protected void btnClear_Click(object sender, DirectEventArgs e)
    {
        pnlLogin.Title = "Login";
        this.txtUsername.Text = "";
        this.txtPassword.Text = "";
    }

    protected void btnReset_Click(object sender, DirectEventArgs e)
    {
        Response.Redirect("~/RESET_PASSWORD.aspx");
    }

    protected void btnNewReg_Click(object sender, DirectEventArgs e)
    {
        Response.Redirect("~/Sys_Company_Reg.aspx");
    }
    protected void btnPassRequest_Click(object sender, DirectEventArgs e)
    {

        //----- variable declaration -----------
        int frequency = 0;
        string empId = "";
        string dptId = "";
        string dsgId = "";
        string email = "";
        //----- End of variable declaration -----------

        string logInName = this.txtUsername.Text.Trim();
        string UsrId = objSysAdmin.getUserId(logInName);
        if (UsrId == "")
        {
            pnlLogin.Title = "Invalid logIn Name";
            return;
        }
        #region old
        //-------------- checking already sent req today and previous 6 days -------------------
        //int sentRequest = objSysAdmin.ChkSentReqNew(UsrId);
        //int sentRequestApproved = objSysAdmin.ChkSentReqApproved(UsrId);
        //if (sentRequest > 0)
        //{
        //    pnlLogin.Title = "Alredy sent request and try after 7 days";
        //    btnPassRequest.Enabled = false;
        //    return;
        //}
        //if (sentRequestApproved > 0)
        //{
        //    pnlLogin.Title = "Check your e-mail for requested password";
        //    btnPassRequest.Enabled = false;
        //    return;
        //}
        //-------------- End of checking already sent req today and previous 6 days -------------------

        #endregion
        //---------------- get basic Info as userId ----------------------
        DataSet dsEmpInfo = objSysAdmin.getUsrInfo(UsrId);
        if (dsEmpInfo.Tables[0].Rows.Count > 0)
        {
            empId = dsEmpInfo.Tables[0].Rows[0]["EMP_ID"].ToString();
            dptId = dsEmpInfo.Tables[0].Rows[0]["DPT_ID"].ToString();
            dsgId = dsEmpInfo.Tables[0].Rows[0]["DSG_ID_MAIN"].ToString();
            email = dsEmpInfo.Tables[0].Rows[0]["EMP_EMAIL"].ToString();
        }
        //-------------- End of get basic Info as userId ---------------------

        if (email != "")
        {
            // --------------------- Insert/Update password request------------------------
            //if (sentRequest == 0 && sentRequestApproved == 0)
            //{
            string msg = "";
            int existPrev = objSysAdmin.ChkSentReq(UsrId);
            if (existPrev > 0)//-----update-----
            {
                DataSet dsUpdate = objSysAdmin.SentReqInfo(UsrId);
                if (dsUpdate.Tables[0].Rows.Count > 0)
                {
                    frequency = int.Parse(dsUpdate.Tables[0].Rows[0]["PASS_REQ_FREQUENCY"].ToString());
                    frequency = frequency + 1;
                    msg = objSysAdmin.UpdatePassReq(UsrId, frequency, email);
                }
            }
            else  //------insert---------
            {
                msg = objSysAdmin.InsertPassReq(UsrId, empId, dptId, dsgId, email);

            }
            if (msg == "success")
            {
                btnPassRequest.Enabled = false;
                pnlLogin.Title = "Password request sent";

            }
            //}
        }
        else
        {
            pnlLogin.Title = "Set Email at Employee Regis. System prior to Request";
        }
        // --------------------- End of Insert/Update password request------------------------
    }

}
