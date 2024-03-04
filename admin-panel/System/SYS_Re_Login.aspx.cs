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
using Ext.Net;

public partial class System_SYS_Re_Login : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, DirectEventArgs e)
    {
        Response.Redirect("Desktop.aspx");
    }
    protected void LoadUser()
    {
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
            string userId = "";

            clsSystemAdmin objSysAdmin = new clsSystemAdmin();
            clsGlobalSetup objGS = new clsGlobalSetup();
            DataSet dtsLoginUser = new DataSet();
            dtsLoginUser = objSysAdmin.LoginWithUserName(username, password);
            string UserActive = "";
            if (dtsLoginUser.Tables["CM_SYSTEM_USERS"].Rows.Count == 1)
            {
                foreach (DataRow prow in dtsLoginUser.Tables["CM_SYSTEM_USERS"].Rows)
                {
                    
                        Session["CompanyBranch"] = "N/A";
                        Session["Branch_Type"] = prow["CMP_BRANCH_TYPE_ID"].ToString();
                        Session["Branch_ID"] = prow["CMP_BRANCH_ID"].ToString();
                        Session["UserDname"] = prow["SYS_USR_DNAME"].ToString();
                        Session["UserLoginName"] = prow["SYS_USR_LOGIN_NAME"].ToString();
                        Session["Password"] = prow["SYS_USR_PASS"].ToString();
                        Session["UserID"] = prow["SYS_USR_ID"].ToString();
                        userId = prow["SYS_USR_ID"].ToString();
                        Session["GroupID"] = prow["SYS_USR_GRP_ID"].ToString();
                        Session["UserEmail"] = prow["SYS_USR_EMAIL"].ToString();
                        //Session["CompanyName"] = objGS.GetCompanyName();
                        //Session["CmpanyAddress"] = "Jahangir Tower, Kawran Bazar";
                    
                }

                if (userId != "" )
                {
                    DataSet dtsLoginUserGrp = new DataSet();
                    dtsLoginUserGrp = objSysAdmin.LoginGroupUser(userId);

                    if (dtsLoginUserGrp.Tables["CM_SYSTEM_USERS_GROUP"].Rows.Count == 1)
                    {
                        foreach (DataRow prow in dtsLoginUserGrp.Tables["CM_SYSTEM_USERS_GROUP"].Rows)
                        {
                            Session["ClientID"] = prow["POSCL_ID"].ToString();
                        }
                    }
                    pnlLogin.Title = "Login [ Login Successfull ]";
                    this.txtPassword.Text = "";
                }
                else
                {
                    pnlLogin.Title = "Login [Sorry, User name expired]";
                    this.txtPassword.Text = "";
                }

            }
            else
            {
                pnlLogin.Title = "Login [ Please insert correct username & password ]";
            }
        }
        catch (Exception ex)
        {

            pnlLogin.Title = ex.Message.ToString();
        }
    }
    
    protected void btnClear_Click(object sender, DirectEventArgs e)
    {
        pnlLogin.Title = "Login";
        this.txtUsername.Text = "";
        this.txtPassword.Text = "";
    }

}
