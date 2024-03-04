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
using System.Data.OleDb;

public partial class System_SYS_Acc_Policy : System.Web.UI.Page
{
    private clsSystemAdmin objClsSystemAdmin = new clsSystemAdmin();
    //private clsSystemAdmin objClsSystemAdmin1 = new clsSystemAdmin();
    private string strConString = ConfigurationSettings.AppSettings["dbConnectionString"];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblShowMsg.Text = "";
        }
    }
    protected void btnAddAll_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        string strUsr_Grp_ID = ddlUserGroup.SelectedValue.ToString();
        ds = objClsSystemAdmin.CountSysMenu(strUsr_Grp_ID);
        lblShowMsg.Text = "";
        if (ds.Tables["CM_SYSTEM_ACCESS_POLICY"].Rows.Count < 1)
        {
            objClsSystemAdmin.AddAllMenu(strUsr_Grp_ID);

        }
        else
        {
            lblShowMsg.Text = "This Group Already Exists in the List";
        }
        sdsSysAccessPolicy.DataBind();
        gdvSysAccessPolicy.DataBind();
        

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            string reslut = objClsSystemAdmin.InsertSysAccPolicy(ddlUserGroup.SelectedValue.ToString(), ddlDdvSysMenuEI.SelectedValue.ToString(),
                ddlDdvViewEI.SelectedValue.ToString(), ddlDdvAddI.SelectedValue.ToString(), ddlDdvDeleteEI.SelectedValue.ToString(), ddlDdvEditEI.SelectedValue.ToString(), ddlDdvPrintEI.SelectedValue.ToString(), ddlDdvSearchEI.SelectedValue.ToString());
            lblShowMsg.Text = reslut;
            // LoadHoliday();
            sdsSysAccessPolicy.DataBind();
            gdvSysAccessPolicy.DataBind();
        }
        catch (Exception ex)
        {
            lblShowMsg.Text = ex.Message.ToString();
        }
    }

    protected void dtvSysAccessPolicy_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        sdsSysAccessPolicy.InsertParameters["SYS_USR_GRP_ID"].DefaultValue = ddlUserGroup.SelectedValue;
    }
    protected void dtvSysAccessPolicy_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
      
    }
    protected void gdvSysAccessPolicy_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
       
    }
    protected void gdvSysAccessPolicy_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
       
    }
}
