using System;
using System.Collections;
using System.Collections.Generic;
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
using Ext.Net.Utilities;
using System.Data.OleDb;

public partial class COMMON_frmUploadQuestionExcel :  System.Web.UI.Page
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
            lblMessage.Text = "";
            ddlBrType.Items.Clear();
            ddlSub.Items.Clear();
            ddlDept.Items.Clear();
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
            try
            {
                GetData();
                getDept();
                getSubject();

            }
            catch (Exception exxx)
            {

            }
        }

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
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
        string deleteMsg = objsrvsHndlr.deleteDataFromTable("CM_SYSTEM_QUESTION_EXCEL", "SYS_QUES_ID", branchId);
        lblMessage.Text = deleteMsg;
        GetData();

    }

    protected void gdvBranchList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
    }
    protected void OnSelectedIndexChanged_type(object sender, EventArgs e)
    {
        getDept();
    }
    protected void OnSelectedIndexChanged_dept(object sender, EventArgs e)
    {
        getSubject();
    }
    protected void OnSelectedIndexChanged_sub(object sender, EventArgs e)
    {

    }


    protected void saveExcel_Click(object sender, EventArgs e)
    {
        string saveToQuestion = objsrvsHndlr.InsertQuestions(ddlSub.SelectedValue.ToString(), strUserName);
        lblMessage.Text = saveToQuestion;
        string deleteExcelTable = objsrvsHndlr.deleteDataFromExcelTable();
        GetData();
        txtExcelFied.Text = "";
        lblMessage9.Text = "";
    }
    protected void btnExcelUpload_Click(object sender, EventArgs e)
    {
        ImportFromExcel();
        GetData(); 
     }
    protected void getDept()
    {
        ddlDept.Items.Clear();
        string typeId = ddlBrType.SelectedValue.ToString();
        ArrayList list = new ArrayList();
        list = objsrvsHndlr.GetDeptListByType(typeId);
        for (int i = 0; i < list.Count; i++)
        {
            string[] name = list[i].ToString().Split('*');
            ddlDept.Items.Add(new ListItem(name[1], name[0]));

        }

    }
    protected void getSubject()
    {
        ddlSub.Items.Clear();
        string typeId = ddlDept.SelectedValue.ToString();
        ArrayList list = new ArrayList();
        list = objsrvsHndlr.GetSubListByDept(typeId);
        for (int i = 0; i < list.Count; i++)
        {
            string[] name = list[i].ToString().Split('*');
            ddlSub.Items.Add(new ListItem(name[1], name[0]));

        }

    }
    protected void GetData()
    {
        if (Session["GroupId"].ToString() == "13052901001002" || Session["GroupId"].ToString() == "13052901001003")
        {
            string all = txtSearch.Text.Trim();
            string strQry = "SELECT CQ.* FROM CM_SYSTEM_QUESTION_EXCEL CQ";
            if (all != "")
            {
                strQry = "SELECT CQ.* FROM CM_SYSTEM_QUESTION_EXCEL CQ WHERE SYS_QUES_NAME='" + all + "' ORDER BY SYS_QUES_ID DESC";
            }
            
            DataTable dt = objsrvsHndlr.getDataTableByQuery(strQry);
            gdvBranchList.DataSource = dt;
            gdvBranchList.DataBind();
        }
        else
        {
            string all = txtSearch.Text.Trim();
            string strQry = "SELECT CQ.*,CS.SYS_SUB_NAME FROM CM_SYSTEM_QUESTION_EXCEL CQ,CM_SYSTEM_SUBJECT CS WHERE CQ.SYS_SUB_ID=CS.SYS_SUB_ID AND CQ.QUES_ADDED_BY='" + Session["UserLoginName"].ToString() + "'";
            if (all != "")
            {
                strQry = "SELECT CQ.*,CS.SYS_SUB_NAME FROM CM_SYSTEM_QUESTION_EXCEL CQ,CM_SYSTEM_SUBJECT CS WHERE CQ.SYS_SUB_ID=CS.SYS_SUB_ID AND SYS_QUES_NAME='" + all + "' AND CQ.QUES_ADDED_BY='" + Session["UserLoginName"].ToString() + "' ORDER BY SYS_QUES_ID DESC";
            }
            else
            {
                strQry = "SELECT CQ.*,CS.SYS_SUB_NAME FROM CM_SYSTEM_QUESTION_EXCEL CQ,CM_SYSTEM_SUBJECT CS WHERE CQ.SYS_SUB_ID=CS.SYS_SUB_ID AND CQ.QUES_ADDED_BY='" + Session["UserLoginName"].ToString() + "' ORDER BY SYS_QUES_ID DESC";
            }
            DataTable dt = objsrvsHndlr.getDataTableByQuery(strQry);
            gdvBranchList.DataSource = dt;
            gdvBranchList.DataBind();
        }


    }

    protected void ImportFromExcel()
    {
        // CHECK IF A FILE HAS BEEN SELECTED.
        if ((uploadExclForPending.HasFile))
        {

            if (!Convert.IsDBNull(uploadExclForPending.PostedFile) &
                uploadExclForPending.PostedFile.ContentLength > 0)
            {

                //FIRST, SAVE THE SELECTED FILE IN THE ROOT DIRECTORY.
                uploadExclForPending.SaveAs(Server.MapPath("~/INV_LC_DOC/") + "\\" + uploadExclForPending.FileName);

                SqlBulkCopy oSqlBulk = null;

                // SET A CONNECTION WITH THE EXCEL FILE.
                OleDbConnection myExcelConn = new OleDbConnection
                    ("Provider=Microsoft.ACE.OLEDB.12.0; " +
                        "Data Source=" + Server.MapPath("~/INV_LC_DOC/") + "\\" + uploadExclForPending.FileName +
                        ";Extended Properties=Excel 12.0;");
                try
                {
                    myExcelConn.Open();

                    // GET DATA FROM EXCEL SHEET.
                    OleDbCommand objOleDB =
                        new OleDbCommand("SELECT *FROM [Sheet1$]", myExcelConn);

                    // READ THE DATA EXTRACTED FROM THE EXCEL FILE.
                    OleDbDataReader objBulkReader = null;
                    objBulkReader = objOleDB.ExecuteReader();

                    // SET THE CONNECTION STRING.
                    string sCon = "Data Source=DESKTOP-A7ACU5B\\SQLEXPRESS; DataBase=QB_DB; Persist Security Info=True;User ID=juba;Password=1234;" +
                        "Connect Timeout=30;";

                    using (SqlConnection con = new SqlConnection(sCon))
                    {
                        con.Open();
                       
                        // FINALLY, LOAD DATA INTO THE DATABASE TABLE.
                        oSqlBulk = new SqlBulkCopy(con);
                        //oSqlBulk.DestinationTableName = "EmployeeDetails"; // TABLE NAME.
                        oSqlBulk.DestinationTableName = "dbo.CM_SYSTEM_QUESTION_EXCEL";

                        //[OPTIONAL]: Map the Excel columns with that of the database table
                        oSqlBulk.ColumnMappings.Add("SYS_QUES_NAME", "SYS_QUES_NAME");
                        oSqlBulk.ColumnMappings.Add("SYS_OPTION_A", "SYS_OPTION_A");
                        oSqlBulk.ColumnMappings.Add("SYS_OPTION_B", "SYS_OPTION_B");
                        oSqlBulk.ColumnMappings.Add("SYS_OPTION_C", "SYS_OPTION_C");
                        oSqlBulk.ColumnMappings.Add("SYS_OPTION_D", "SYS_OPTION_D");
                        oSqlBulk.ColumnMappings.Add("CORRECT_OPTION", "CORRECT_OPTION");
                        oSqlBulk.WriteToServer(objBulkReader);
                    }

                    txtExcelFied.Text = Server.MapPath("~/INV_LC_DOC/") + "\\" + uploadExclForPending.FileName;
                    lblMessage9.Visible = true;

                }
                catch (Exception ex)
                {

                   lblMessage.Text = ex.Message;
                   

                }
                finally
                {
                    // CLEAR.
                    oSqlBulk.Close();
                    oSqlBulk = null;
                    myExcelConn.Close();
                    myExcelConn = null;
                }
            }
        }
    }


}