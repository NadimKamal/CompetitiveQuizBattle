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
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Threading;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;

public partial class System_SYS_System_Users : System.Web.UI.Page
{
    
    clsSystemAdmin objSysAdmin = new clsSystemAdmin();
    clsServiceHandler objsrvsHndlr = new clsServiceHandler();
    DataSet dsData;
    private static string strUserName = string.Empty;
    private static string strPassword = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            try
            {
                strUserName = Session["UserLoginName"].ToString();
                strPassword = Session["Password"].ToString();
            }
            catch
            {
                Session.Clear();
                Response.Redirect("../frmSeesionExpMesage.aspx");
            }
            
            getGroup();
            try
            {
                GetData();

            }
            catch (Exception exxx)
            {

            }
        }
        lblMessage.Text = "";

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetData();

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        string strName = txtName.Text.ToString();
        string login = txtLogin.Text.ToString();
        string group = ddlGroup.SelectedValue.ToString();
        string pass = txtPass.Text.ToString();
        

        string insertSrvsRes = objsrvsHndlr.InsertUser(strName, login, group, pass);
        if (insertSrvsRes == "OK")
        {
            lblMessage.Text = "Information Successfully Saved.";
        }
        else
        {
            lblMessage.Text = "Unsuccessfull.";
        }


        GetData();


    }

    protected void gdvBranchList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvBranchList.PageIndex = e.NewPageIndex;
        GetData();
    }
    protected void gdvBranchList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gdvBranchList.EditIndex = e.NewEditIndex;
    }
    protected void gdvBranchList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //TextBox txtNamee = gdvBranchList.Rows[e.RowIndex].FindControl("gvtxtBranchName") as TextBox;
        //TextBox txtcodee = gdvBranchList.Rows[e.RowIndex].FindControl("gvtxtBranchCode") as TextBox;
        //TextBox txtaddresss = gdvBranchList.Rows[e.RowIndex].FindControl("gvtxtBranchAddress") as TextBox;

        //TextBox txtLead = gdvBranchList.Rows[e.RowIndex].FindControl("gvtxtlblLeadsMobile") as TextBox;
        //TextBox txtEmpNamee = gdvBranchList.Rows[e.RowIndex].FindControl("gvtxtEmployeeName") as TextBox;
        //TextBox txtEmpMobilee = gdvBranchList.Rows[e.RowIndex].FindControl("gvtxtEmployeeMobile") as TextBox;
        //TextBox txtCon = gdvBranchList.Rows[e.RowIndex].FindControl("gvtxtlblConMobile") as TextBox;
        //TextBox txtWallett = gdvBranchList.Rows[e.RowIndex].FindControl("gvtxtWallet") as TextBox;
        //TextBox tstatus = gdvBranchList.Rows[e.RowIndex].FindControl("txtStatus") as TextBox;

        //Label lblId = gdvBranchList.Rows[e.RowIndex].FindControl("lblBranchId") as Label;
        //string name = txtNamee.Text.Trim();

        //string code = txtcodee.Text.Trim();
        //string address = txtaddresss.Text.Trim();
        //string branchId = lblId.Text.Trim();

        //string Lead = txtLead.Text.Trim();
        // string empName = txtEmpNamee.Text.Trim();
        //string empMobile = txtEmpMobilee.Text.Trim();
        //string Con = txtCon.Text.Trim();
        // string wallet = txtWallett.Text.Trim();
        //string status = tstatus.Text.Trim();
        //txtSearch.Text = branchId;
        //string updateRes = objsrvsHndlr.UpdatePartexBranchInfo(branchId, name, address, empName, empMobile, status);
        //lblMessage.Text = updateRes;
        //gdvBranchList.EditIndex = -1;

        GetData();

    }
    protected void gdvBranchList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gdvBranchList.EditIndex = -1;

    }
    protected void gdvCustomerEmailAddressList_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gdvBranchList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lblId = gdvBranchList.Rows[e.RowIndex].FindControl("lblBranchId") as Label;
        string branchId = lblId.Text.Trim();
        string deleteMsg = objsrvsHndlr.deleteDataFromTable("CM_SYSTEM_USERS", "SYS_USR_ID", branchId);
        lblMessage.Text = deleteMsg;
        GetData();
    }

    protected void gdvBranchList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
    }
    protected void OnSelectedIndexChanged_type(object sender, EventArgs e)
    {
        
    }

    protected void GetData()
    {
        string all = txtSearch.Text.Trim();
        string strQry = "select SU.*,SG.SYS_USR_GRP_TITLE FROM CM_SYSTEM_USERS SU,CM_SYSTEM_USER_GROUP SG WHERE SU.SYS_USR_GRP_ID=SG.SYS_USR_GRP_ID";
        if (all != "")
        {
            strQry = "select SU.*,SG.SYS_USR_GRP_TITLE FROM CM_SYSTEM_USERS SU,CM_SYSTEM_USER_GROUP SG WHERE SU.SYS_USR_GRP_ID=SG.SYS_USR_GRP_ID AND SYS_USR_LOGIN_NAME LIKE'%" + all + "%'";
        }
        
        DataTable dt = objsrvsHndlr.getDataTableByQuery(strQry);
        gdvBranchList.DataSource = dt;
        gdvBranchList.DataBind();
    }
    protected void getGroup()

    {
        ddlGroup.Items.Clear();
        
        ArrayList list = new ArrayList();
        list = objsrvsHndlr.GetGroupList();
        for (int i = 0; i < list.Count; i++)
        {
            string[] name = list[i].ToString().Split('*');
            ddlGroup.Items.Add(new ListItem(name[1], name[0]));

        }

    }

}


