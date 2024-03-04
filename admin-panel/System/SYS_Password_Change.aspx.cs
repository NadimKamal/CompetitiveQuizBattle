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

public partial class System_SYS_Password_Change : System.Web.UI.Page
{
    clsSystemAdmin objclsSystemAdmin = new clsSystemAdmin();
    string strOldPass;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            txtLogInName.Text = Session["UserLoginName"].ToString();
            
            try
            {
            //    lblSpMessage.Text = "";
            //ddlBranch.SelectedValue = Session["Branch_ID"].ToString();
            //    if (Session["Branch_Type"].Equals("A"))
            //    {
            //       ddlBranch.Enabled = true;
            //    }
            //    else
            //    {
            //     ddlBranch.Enabled = false;
            //    }
            }

            catch
            {
                Response.Redirect("../frmSeesionExpMesage.aspx");
            }

        }
    }

    public void vChangePass()
    {
        strOldPass = Session["Password"].ToString();
        string strUserID = Session["UserID"].ToString();
        string strMessage = "";
        try
        {
            if ((txtNPassword.Text.Trim().Length > 0) && (txtLogInName.Text.Trim().Length > 0) && (txtOldPass.Text.Length > 0))
            {
                if (txtOldPass.Text.Trim() == strOldPass)
                {
                    if (txtNPassword.Text.Trim() == txtRepass.Text.Trim())
                    {
                        strMessage = objclsSystemAdmin.sChangePassword(strUserID,
                                                                       txtNPassword.Text.ToString(),
                                                                       txtLogInName.Text.ToString());
                        lblSpMessage.Text = "Password changed successfully";
                    }
                    else
                    {
                        lblSpMessage.Text = "Password Mismatched";
                        txtOldPass.Text = "";
                    }
                }
                else
                {
                    lblSpMessage.Text = "Password Mismatched";
                    txtOldPass.Text = "";
                }
            }
            else
            {
                lblSpMessage.Text = "Password/Login Name/Old Password not Found";
                return;
            }
        }
        catch (Exception ex)
        {
            lblSpMessage.Text = ex.ToString();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        vChangePass();
        txtNPassword.Text = "";
        txtRepass.Text = "";
        Session["UserLoginName"] = txtLogInName.Text.ToString();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtNPassword.Text = "";
        txtRepass.Text = "";
        txtOldPass.Text = "";
    }
}
