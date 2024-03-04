using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
public partial class Forms_frmWellcomeMessage : System.Web.UI.Page
{
    clsGlobalSetup objGs = new clsGlobalSetup();
    clsHRM objHrm = new clsHRM();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string bId = Session["Branch_ID"].ToString();
            if(bId!="")
            {
                string WelCmMsg = objGs.GetWelcmMsg(bId);
                
            }
            else
            {

            }
           


            try
            {
                if (!IsPostBack)
                {
                    
                    // 1st time load 
                    //---------- only admin can see the Birthday -----------------
                    if (Session["UserLoginName"].ToString() == "admin") // admin employee
                    {

                    }

                }
            }
            catch (Exception)
            {

            }

            
           
           
        }

    }
   
}


