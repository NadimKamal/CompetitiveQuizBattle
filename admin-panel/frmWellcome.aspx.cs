using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Ext.Net;

public partial class frmWellcome : System.Web.UI.Page
{
    clsGlobalSetup objGs = new clsGlobalSetup();
    public static ArrayList AllEmpInfo = new ArrayList();
    #region For Mail
    public static string strSMTP_Host = "";
    public static string strSMTP_Port = "";
    public static string strMailSender = "";
    //public static string strSenderAddress = "info@bdmitech.com";
    public static string strSenderAddress = "";
    public static string strSenderPass = "";
    public static string strSenderID = "";
    public static string strReceiverAddress = "";
    public static string strServiceString = "";
    public static string strReceipentID = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            try
            {
                StatusBar1.SetStatus("Logged As: " + Session["UserLoginName"].ToString() + '(' + Session["UserDname"].ToString() + ')');
                clsGlobalSetup objGs = new clsGlobalSetup();
                string bId = Session["Branch_ID"].ToString();
                DataSet oSS = objGs.GetSlonName_Aft_Login(bId);
                if (oSS.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow sRow in oSS.Tables[0].Rows)
                    {
                        string SolnName = sRow["SOLN_NAME_AFTR_LOGIN"].ToString();
                        string TitleAfterLogin = sRow["TITLE_AFTR_LOGIN"].ToString();
                        Title = TitleAfterLogin; //Md.Asaduzzaman
                        Panel1.Html = "<div id='header'><h1>" + SolnName + "</h1></div>";
                    }
                }
                if (bId == "100310005")
               {
                    FinalSmail();
               }
                   
               

            }
            catch (Exception ex)
            {
                Session.Clear();
                Response.Redirect("frmLogin.aspx");
            }
        }
    }


    protected void tbbRefreshMenu_Click(object sender, DirectEventArgs e)
    {
        clsTreeView objTreeView = new clsTreeView();
        objTreeView.BuildTreeNodes(false, Session["GroupID"].ToString());
        exampleTree.Reload();
        //WestPanel.
        //ViewPort1.rel
        //Response.Redirect("~/");
    }

    protected void GetTreeMenuNodes(object sender, NodeLoadEventArgs e)
    {
        clsTreeView objTreeView = new clsTreeView();
        if (e.NodeID == "root")
        {
            //var nodes = this.Page.Cache["ExamplesTreeNodes"] as Coolite.Ext.Web.TreeNodeCollection;
            //Coolite.Ext.Web.TreeNodeCollection nodes = new Coolite.Ext.Web.TreeNodeCollection();
            Ext.Net.TreeNodeCollection nodes = new Ext.Net.TreeNodeCollection();

            //if (nodes == null)
            //{
            nodes = objTreeView.BuildTreeNodes(false, Session["GroupID"].ToString());
            //    this.Page.Cache.Add("ExamplesTreeNodes", nodes, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            //}

            e.Nodes = nodes;
        }
    }

    private bool CheckQueryString(string url)
    {
        url = url.ToLower();
        if (!url.EndsWith("/"))
        {
            url = string.Concat(url, "/");
        }
        string examplesFolder = new Uri(HttpContext.Current.Request.Url, "/Examples/").ToString().ToLower();
        if (!url.StartsWith(examplesFolder))
        {
            url = string.Concat(examplesFolder.TrimEnd(new[] { '/' }), url);
        }
        Uri uri = new Uri(url, UriKind.Absolute);
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(this.Server.MapPath(uri.AbsolutePath));
        return System.IO.File.Exists(string.Concat(dir.FullName, "config.xml"));
    }

    [DirectMethod]
    protected void tbbLogin_Click(object sender, DirectEventArgs e)
    {
        X.Msg.Confirm("FiS Message", "Are you sure to log out?", new MessageBoxButtonsConfig
        {
            Yes = new MessageBoxButtonConfig
            {
                Handler = "MIT.LogOut()",
                Text = "Yes Please"
            },
            No = new MessageBoxButtonConfig
            {
                Handler = "MIT.DoNo()",
                Text = "No Thanks"
            }
        }).Show();
    }
    [DirectMethod]
    public void LogOut()
    {
        Session.Clear();
        Response.Redirect("frmLogin.aspx");
    }

    [DirectMethod]
    public void DoYes()
    {
        Session.Clear();
        Response.Redirect("frmLogin.aspx");
    }

    [DirectMethod]
    public void DoNo()
    {
        X.Msg.Prompt("FiS", "NO");
    }
    #region Email Send

    /**
* Prepared By    : Niaz
* Page Update By : 
* Date           : 13-Feb-2013
* Decription     : send Mail for Birthday
* Addition       : None
**/

    protected void FinalSmail()
    {
        LoadSMTPInfo();
        string bId = Session["Branch_ID"].ToString();
        clsGlobalSetup ObjGS = new clsGlobalSetup();
        DataSet oBs = new DataSet();
        DateTime asdate = DateTime.Now;
        string ASDate = asdate.ToString().Trim() == "" ? "" : String.Format("{0:dd-MMM-yyyy}", DateTime.Parse(asdate.ToString()));
        oBs = objGs.GetActualBday(ASDate, bId);
        int sendMailCount = 0;
        string ename = "";
        //DataSet oCs = new DataSet();
        //oCs = objGs.GetChaque(ASDate);
        //if (oCs.Tables["CHEQUE_BIRTHDAY"].Rows.Count > 0)
        if (oBs.Tables["GET_BIRTHDAY"].Rows.Count > 0)
        {
            foreach (DataRow pRow in oBs.Tables["GET_BIRTHDAY"].Rows)
            {
                string Estatus = pRow["MAIL_SEND_STATUS"].ToString();
                if (Estatus == "N")
                {
                    ++sendMailCount;
                    //mail();
                    string EmpName = pRow["EMP_NAME"].ToString();
                    string DsgTitel = pRow["DSG_TITLE"].ToString();
                    string EMPCODE = pRow["EMP_CODE"].ToString();

                    //ename = ename + "" + "Today is Birthday of " + EmpName + " <br />";
                    ename = ename + EmpName + '(' + EMPCODE + ')' +'-' + DsgTitel + " <br />";
                }
            }

            if (sendMailCount > 0) // not send mail 
            {
                string strSubject = "Birthday Reminder";
                string strBody = "<table style='font-family:Verdana, Arial, Helvetica, sans-serif;font-size:12px;' align='left' border='0' cellpadding='2' cellspacing='2'>"
                                                              + "<tbody>"
                                                                  + "<tr>"
                                                                      + "<td><u><b>Today's Birthday:</b></u><br><br>" + ename + "</td>"
                                                                  + "</tr>"
                                                              + "</tbody>"
                                                          + "</table>";

                SendMail(strSubject, strBody);
                string strResult = ObjGS.UpdateEmailStatus(ASDate);
            }
        }
        else
        {
            string strUpdate = ObjGS.UpdateEStatusN();
        }
    }

    //protected void mail()
    //{
    //    string bId = Session["Branch_ID"].ToString();
    //    DataSet oBs = new DataSet();
    //    DateTime sysdate = DateTime.Now;
    //    string ename = "";
    //    string SysDate = sysdate.ToString().Trim() == "" ? "" : String.Format("{0:dd-MMM-yyyy}", DateTime.Parse(sysdate.ToString()));
    //    oBs = objGs.GetActualBday(SysDate,bId);
    //    foreach (DataRow pRow in oBs.Tables["PR_EMPLOYEE_LIST"].Rows)
    //    {
    //        string EmpName = pRow["EMP_NAME"].ToString();
    //        ename = ename + "" + "Today is Birthday of " + EmpName + " <br />";
    //    }

    //    if (ename == "")
    //    {
    //    }
    //    else
    //    {
    //        //string strSubject = "Birthday Reminder";
    //        //string strBody = "<table style='font-family:Verdana, Arial, Helvetica, sans-serif;font-size:12px;' align='left' border='0' cellpadding='2' cellspacing='2'>"
    //        //                                              + "<tbody>"
    //        //                                                  + "<tr>"
    //        //                                                      + "<td><b>Name:</b> </td><td>" + ename + "</td>"
    //        //                                                  + "</tr>"
    //        //                                              + "</tbody>"
    //        //                                          + "</table>";

    //        //SendMail(strSubject, strBody);
    //    }
    //}
    public void LoadSMTPInfo()
    {
        string bId = Session["Branch_ID"].ToString();
        clsHRM objHRM = new clsHRM();
        DataSet oDSSMTPInfo = new DataSet();
        oDSSMTPInfo = objHRM.GetSMTPInfoNew(bId);

        if (oDSSMTPInfo.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow prow in oDSSMTPInfo.Tables["SMTP_INFO"].Rows)
            {
                strSMTP_Host = prow["SYS_SMTP_HOST"].ToString();
                strSMTP_Port = prow["SYS_SMTP_PORT"].ToString();
                strMailSender = prow["SYS_SMTP_SENDER"].ToString();
                strSenderAddress = prow["SYS_SENDER_ADDRESS"].ToString();
                strSenderPass = prow["SYS_SMTP_SPASS"].ToString();
                //strMailReceiver = prow["ACCNT_SEC_QUES_DESC"].ToString();
            }
        }
    }
    public void SendMail(string strSubject, string strBody)
    {
        DataSet oBs = new DataSet();
        DateTime sysdate = DateTime.Now;
        string bId = Session["Branch_ID"].ToString();
        oBs = objGs.GetEmailID(bId);
        foreach (DataRow pRow in oBs.Tables["EMP_EMAILID"].Rows)
        {

            strReceiverAddress = pRow["EMP_EMAIL"].ToString();

            try
            {
                //lblMessage.Text = "Mail Sending";
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                //System.Net.NetworkCredential cred = new System.Net.NetworkCredential("priscila.gomes@bdmitech.com", "Priscila123");
                //System.Net.NetworkCredential cred = new System.Net.NetworkCredential("info@bdmitech.com", "info123456");
                System.Net.NetworkCredential cred = new System.Net.NetworkCredential(strSenderAddress, strSenderPass);

                mail.To.Add(strReceiverAddress);
                mail.Subject = strSubject;

                mail.From = new System.Net.Mail.MailAddress(strSenderAddress, strMailSender);
                mail.IsBodyHtml = true;
                mail.Body = strBody;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(strSMTP_Host);
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = false;
                smtp.Credentials = cred;
                smtp.Port = int.Parse(strSMTP_Port);
                smtp.Send(mail);
                // lblMessage.Text = "Application Sent Successfully."; // +DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
            }
            catch (Exception ex)
            {
                // lblMessage.Text = ex.Message.ToString();
            }
        }
    }
    #endregion
}
