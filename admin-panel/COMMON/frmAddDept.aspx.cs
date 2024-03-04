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

public partial class COMMON_frmAddDept : System.Web.UI.Page
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
                GetData();
            }
            catch (Exception exxx)
            {

            }

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
            lblMessage.Text = "";
            ddlBrType.Items.Clear();
            try
            {

                ArrayList list = new ArrayList();
                list = objsrvsHndlr.GetDeptTypeList();
                for (int i = 0; i < list.Count; i++)
                {
                    string[] name = list[i].ToString().Split('*');
                    ddlBrType.Items.Add(new ListItem(name[1], name[0]));
                    //ddlBrType.DataValueField=name[0];
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                Response.Redirect("../frmSeesionExpMesage.aspx");
            }
        }
        
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetData();

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        string strName = txtBranchCode.Text.Trim();
        string brType = ddlBrType.SelectedValue.Trim();


      
            
               
    var isExist = objsrvsHndlr.GetDeptName(strName);
    if (isExist==1)
    {
        lblMessage.Text = "Department Already Exist";
        return;
    }
   

    string insertSrvsRes = objsrvsHndlr.InsertDept(strName,strUserName, brType);
        
    lblMessage.Text = "Information Successfully Saved.";
    txtBranchCode.Text = "";
           
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
        string deleteMsg = objsrvsHndlr.deleteDataFromTable("CM_SYSTEM_DEPT", "SYS_DEPT_ID", branchId);
        lblMessage.Text = deleteMsg;
        GetData();
    }

    protected void gdvBranchList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }

    protected void GetData()
    {
        if (Session["GroupId"].ToString()=="13052901001002" || Session["GroupId"].ToString() == "13052901001003") 
        {
            string all = txtSearch.Text.Trim();
            string strQry = "SELECT * FROM CM_SYSTEM_DEPT CD,CM_SYSTEM_DEPT_TYPE CT WHERE CT.SYS_DEPT_TYPE_ID=CD.SYS_DEPT_TYPE_ID";
            if (all != "")
            {
                strQry = "SELECT * FROM CM_SYSTEM_DEPT CD,CM_SYSTEM_DEPT_TYPE CT WHERE CT.SYS_DEPT_TYPE_ID=CD.SYS_DEPT_TYPE_ID AND SYS_DEPT_NAME='" + all + "' ORDER BY SYS_DEPT_ID DESC";
            }
            else
            {
                strQry = "SELECT * FROM CM_SYSTEM_DEPT CD,CM_SYSTEM_DEPT_TYPE CT WHERE CT.SYS_DEPT_TYPE_ID=CD.SYS_DEPT_TYPE_ID ORDER BY SYS_DEPT_ID DESC";
            }
            DataTable dt = objsrvsHndlr.getDataTableByQuery(strQry);
            gdvBranchList.DataSource = dt;
            gdvBranchList.DataBind();
        }
        else
        {
            string all = txtSearch.Text.Trim();
            string strQry = "SELECT * FROM CM_SYSTEM_DEPT CD,CM_SYSTEM_DEPT_TYPE CT WHERE CT.SYS_DEPT_TYPE_ID=CD.SYS_DEPT_TYPE_ID AND CD.DEPT_ADDED_BY='" + Session["UserLoginName"].ToString() + "'";
            if (all != "")
            {
                strQry = "SELECT * FROM CM_SYSTEM_DEPT CD,CM_SYSTEM_DEPT_TYPE CT WHERE CT.SYS_DEPT_TYPE_ID=CD.SYS_DEPT_TYPE_ID AND SYS_DEPT_NAME='" + all + "' AND CD.DEPT_ADDED_BY='"+ Session["UserLoginName"].ToString() + "' ORDER BY SYS_DEPT_ID DESC";
            }
            else
            {
                strQry = "SELECT * FROM CM_SYSTEM_DEPT CD,CM_SYSTEM_DEPT_TYPE CT WHERE CT.SYS_DEPT_TYPE_ID=CD.SYS_DEPT_TYPE_ID AND CD.DEPT_ADDED_BY='" + Session["UserLoginName"].ToString() + "' ORDER BY SYS_DEPT_ID DESC";
            }
            DataTable dt = objsrvsHndlr.getDataTableByQuery(strQry);
            gdvBranchList.DataSource = dt;
            gdvBranchList.DataBind();
        }
    }

}

