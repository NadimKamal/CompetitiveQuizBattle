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

public partial class System_SYS_User_Group : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                ddlCmpBranch.SelectedValue = Session["Branch_ID"].ToString();
                if (Session["Branch_Type"].Equals("A"))
                {
                    ddlCmpBranch.Enabled = true;
                }
                else
                {
                    ddlCmpBranch.Enabled = false;
                }
            }
            catch
            {
                Response.Redirect("../frmSeesionExpMesage.aspx");
            }
        }
    }
   

    //public void LoadSystem()
    //{
    //    string strSql = "";
    //    string strFilter = "";

    //    try
    //    {
    //        if (ddlCmpBranch.SelectedIndex<1)
    //        {
    //            strFilter = strFilter + "";
    //        }
    //        else
    //        {
    //            strFilter = strFilter + "AND (C.CMP_BRANCH_ID ='" + ddlCmpBranch.SelectedValue.ToString() + "' OR C.CMP_BRANCH_ID = '100310001') ";
    //        }

    //        strSql = " SELECT SG.SYS_USR_GRP_ID,SG.SYS_USR_GRP_TITLE,SG.SYS_USR_GRP_PARENT,SG.SYS_USR_GRP_TYPE,SG.CMP_BRANCH_ID "
    //                    + " FROM CM_SYSTEM_USER_GROUP SG,CM_CMP_BRANCH C  "
    //                    + " WHERE  SG.CMP_BRANCH_ID=C.CMP_BRANCH_ID  "
    //                    + " " + strFilter + " " ;
    //        sdsSysUsrGroup.SelectCommand = strSql;
    //        sdsSysUsrGroup.DataBind();
    //        gdvSysUsrGroup.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMessage.Text = ex.ToString();
    //    }
    //}
    protected void dtvSysUsrGroup_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        e.Values["CMP_BRANCH_ID"] = ddlCmpBranch.SelectedValue;
    }
}
