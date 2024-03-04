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

public partial class System_SYS_Sytem_Menu : System.Web.UI.Page
{
    clsGlobalSetup objGS = new clsGlobalSetup();
    clsSystemAdmin objSys = new clsSystemAdmin();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                // ddlCmpBranch.SelectedValue = Session["Branch_ID"].ToString();
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
    protected void ddlCmpBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSystem();
    }

    public void LoadSystem()
    {
        string strSql = "";
        string strFilter = "";

        try
        {
            if (ddlCmpBranch.SelectedIndex<1)
            {
                strFilter = strFilter + "";
            }
            else
            {
                //strFilter = strFilter + "AND (C.CMP_BRANCH_ID ='" + ddlCmpBranch.SelectedValue.ToString() + "' OR C.CMP_BRANCH_ID = '" + ddlCmpBranch.Items[0].Value + "') ";
                strFilter = strFilter + "AND (C.CMP_BRANCH_ID ='" + ddlCmpBranch.SelectedValue.ToString() + "' ) ";
            }

            strSql = "SELECT S.SYS_MENU_ID,S.SYS_MENU_TITLE,S.SYS_MENU_FILE,S.SYS_MENU_PARENT,S.CMP_BRANCH_ID,S.SYS_MENU_TYPE,S.SYS_MENU_SERIAL "
                        + "FROM CM_SYSTEM_MENU S,CM_CMP_BRANCH C "
                        + "WHERE  S.CMP_BRANCH_ID = C.CMP_BRANCH_ID  "
                        + " " + strFilter + " "
                        + "ORDER BY S.SYS_MENU_SERIAL ";
            sdsSysMenu.SelectCommand = strSql;
            sdsSysMenu.DataBind();
            grvSystemMenu.DataBind();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
        }
    }
    protected void dtvSystemMenu_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
    {

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        //string Branch = ddlCmpBranch.SelectedValue.ToString();
        //string compName = objGS.GetCompanyName();
        //string compBranch = ddlCmpBranch.SelectedItem.Text;

        //string lveTypeBr = "";

        //if (Branch != ddlCmpBranch.Items[0].Value) // IF other branch selected not 'All Branch'
        //{
        //    lveTypeBr = Branch + "','" + ddlCmpBranch.Items[0].Value;
        //}
        //else
        //{
        //    lveTypeBr = Branch;
        //}
        //DataSet oDS = new DataSet();
        //string strHTML = "";
        //string filename = "SYS_System_Menu";
        //oDS = objSys.Menu(lveTypeBr);

        //strHTML = strHTML + "<table  width=\"100%\">";
        //strHTML = strHTML + "<tr><td COLSPAN=5 align=center><h2 align=center>" + compName + "</h2></td></tr>";
        //strHTML = strHTML + "<tr><td COLSPAN=5 align=center><h2 align=center>" + compBranch + "</h2></td></tr>";
        //strHTML = strHTML + "<tr><td COLSPAN=5 align=center><h3 align=center>System Menu</h3></td></tr>";
        //strHTML = strHTML + "</table>";

        //strHTML = strHTML + "<table border=\"1\" width=\"100%\">";
        //strHTML = strHTML + "<TR>";
        //strHTML = strHTML + "<td align=center><b>SN</b></td>";
        ////strHTML = strHTML + "<td align=center><b>Order No</b></td>";
        //strHTML = strHTML + "<td align=center><b>Menu Title</b></td>";
        ////strHTML = strHTML + "<td align=center><b>Parent Menu</b></td>";
        //strHTML = strHTML + "<td align=center><b>Menu Type</b></td>";

        //strHTML = strHTML + "</TR>";


        //if (oDS.Tables[0].Rows.Count > 0)
        //{
        //    int SerialNo = 0;

        //    foreach (DataRow prow in oDS.Tables["CM_SYSTEM_MENU"].Rows)
        //    {
        //        string Type = "";
        //        string space = "";
        //        if (prow["SYS_MENU_TYPE"].ToString() == "RT")
        //        {
        //            Type = "ROOT";
        //            space = space + "";
        //        }
        //        else if (prow["SYS_MENU_TYPE"].ToString() == "GR")
        //        {
        //            Type = "GROUP";
        //            space = space + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        //        }
        //        else
        //        {
        //            Type = "MENU";
        //            space = space + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        //        }
        //        //string gender = prow["EMP_GENDER"].ToString() == "F" ? "Female" : "Male";
        //        SerialNo = SerialNo + 1;
        //        strHTML = strHTML + "<tr><td >" + SerialNo.ToString() + "</td>";
        //        //strHTML = strHTML + " <td > " + prow["SYS_MENU_SERIAL"].ToString() + " </td>";
        //        if (prow["SYS_MENU_TYPE"].ToString() == "MN")
        //        {
        //            strHTML = strHTML + " <td >" + space + " => " + prow["SYS_MENU_TITLE"].ToString() + " </td>";
        //        }
        //        else
        //        {
        //            strHTML = strHTML + " <td ><b>" + space + " " + prow["SYS_MENU_TITLE"].ToString() + " </b></td>";
        //        }
        //        //strHTML = strHTML + " <td > " + prow["ParentMenu"].ToString() + " </td>";              
        //        strHTML = strHTML + " <td align=center> " + Type + " </td>";

        //        strHTML = strHTML + " </tr>";
        //    }
        //}

        //strHTML = strHTML + " </table>";
        //clsGridExport.ExportToMSWord(filename, "msword", strHTML, "landscape");
    }

    protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadBranch(ddlCompanyName.SelectedValue.ToString());
    }
    public void LoadBranch(string companyId)
    {
        string query = "select * from CM_CMP_BRANCH where   CMP_Company_ID='" + companyId + "'";

        sdsCmpBranch.SelectCommand = query;
        sdsCmpBranch.DataBind();
        ddlCmpBranch.DataBind();
    }
}
