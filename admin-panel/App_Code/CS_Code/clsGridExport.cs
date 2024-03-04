using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Net;
using System.Collections;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

using Excel = Microsoft.Office.Interop.Excel;


/// <summary>
/// 
/// </summary>
public class clsGridExport
{
    SqlTransaction dbTransaction;
    private static string strConString = System.Configuration.ConfigurationManager.AppSettings["dbConnectionString"];
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="gv"></param>
    public static void ExportGridPayroll(string fileName, GridView gv, string company, string branch, string prMonthDesc)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a form to contain the grid
                Table table = new Table();                

                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    clsGridExport.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    clsGridExport.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    clsGridExport.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);

                string strHTML = "";
                string payroll = "";
                strHTML =  "<table >"
                                       + "<tr ><td colspan='19' align='center'><h2>" + company + "</h2></td></tr>"
                                       + "<tr><td colspan='19' align='center'><h3>" + branch + "</h3></td></tr>"
                                       + " <tr><td colspan='19' align='center'><h3>" + prMonthDesc + "</h3></td></tr>"
                                + "</table>";

                payroll = "<table align='center' border='0'>"
                                   + "<tr><td align='center'>" + strHTML + "</td></tr>"
                                   + "<tr><table><tr><td>" + sw.ToString() + "</td></tr></table></tr>"
                                   + "<tr><td align='center'><br /></td></tr>"
                                   +"<tr><td align='center'><br /></td></tr>"
                                   + "<tr><td align='center'><br /></td></tr>"
                                   + "<tr><td align='center'><br /></td></tr>"
                                   + "<tr><table><tr> <td></td><td> <td><u><b>Prepared By</b></u></td> <td></td><td></td><td></td> <td><u><b>Checked by</b></u></td> <td></td><td></td><td></td> <td><u><b>Authorized by</b></u></td> <td></td><td></td><td></td> <td><u><b>Approved by</b></u></td> </tr></table></tr>"
                                                       
                         + "</table>";

                string finalStr = payroll;

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(finalStr);
                HttpContext.Current.Response.End();
            }
        }
    }
    //public static void ExportGridPayroll(string fileName, GridView gv, string company, string branch, string prMonthDesc)
    //{
    //    HttpContext.Current.Response.Clear();
    //    HttpContext.Current.Response.AddHeader(
    //        "content-disposition", string.Format("attachment; filename={0}", fileName));
    //    HttpContext.Current.Response.ContentType = "application/ms-excel";

    //    using (StringWriter sw = new StringWriter())
    //    {
    //        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
    //        {
    //            //  Create a form to contain the grid
    //            Table table = new Table();                

    //            //  add the header row to the table
    //            if (gv.HeaderRow != null)
    //            {
    //                clsGridExport.PrepareControlForExport(gv.HeaderRow);
    //                table.Rows.Add(gv.HeaderRow);
    //            }

    //            //  add each of the data rows to the table
    //            foreach (GridViewRow row in gv.Rows)
    //            {
    //                clsGridExport.PrepareControlForExport(row);
    //                table.Rows.Add(row);
    //            }

    //            //  add the footer row to the table
    //            if (gv.FooterRow != null)
    //            {
    //                clsGridExport.PrepareControlForExport(gv.FooterRow);
    //                table.Rows.Add(gv.FooterRow);
    //            }

    //            //  render the table into the htmlwriter
    //            table.RenderControl(htw);

    //            string strHTML = "";
    //            strHTML = "<table align='center' border='0' cellpadding='0' cellspacing='0'>"
    //                            + "<tbody>"
    //                                + "<tr><td>" + company + "</td></tr>"
    //                                + "<tr><td>" + branch + "</td></tr>"
    //                                + "<tr><td>" + prMonthDesc + "</td></tr>"
    //                            + "<tbody>"
    //                      + "</table>";

    //            string finalStr = strHTML + sw.ToString();

    //            //  render the htmlwriter into the response
    //            HttpContext.Current.Response.Write(finalStr);
    //            HttpContext.Current.Response.End();
    //        }
    //    }
    //}
   
    public static void ExportGrid(string fileName, GridView gv)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a form to contain the grid
                Table table = new Table();

                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    clsGridExport.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    clsGridExport.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    clsGridExport.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
        }
    }

    /// <summary>
    /// Replace any of the contained controls with literals
    /// </summary>
    /// <param name="control"></param>
    private static void PrepareControlForExport(Control control)
    {
        for (int i = 0; i < control.Controls.Count; i++)
        {
            Control current = control.Controls[i];
            if (current is LinkButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
            }
            else if (current is ImageButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
            }
            else if (current is HyperLink)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
            }
            else if (current is DropDownList)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
            }
            else if (current is CheckBox)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
            }

            if (current.HasControls())
            {
                clsGridExport.PrepareControlForExport(current);
            }
        }
    }
    public static void ExportToWord(string fileName, string strType)
    {
        HttpContext.Current.Response.Clear(); //this clears the Response of any headers or previous output
        HttpContext.Current.Response.Buffer = true; //make sure that the entire output is rendered simultaneously
        string strFileType = "";
        ///
        ///Set content type to MS Excel sheet
        ///Use "application/msword" for MS Word doc files
        ///"application/pdf" for PDF files
        ///
        if (strType.Equals("word"))
        {
            HttpContext.Current.Response.ContentType = "application/msword";
        }
        else
        {
            HttpContext.Current.Response.ContentType = "application/pdf";
        }
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");

        StringWriter stringWriter = new StringWriter(); //System.IO namespace should be used
        HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

        ///
        ///Render the entire Page control in the HtmlTextWriter object
        ///We can render individual controls also, like a DataGrid to be
        ///exported in custom format (excel, word etc)
        ///
        //this.RenderControl(htmlTextWriter);
        HttpContext.Current.Response.Write(stringWriter.ToString());
        HttpContext.Current.Response.End();
    }
    //public static void ExportClientList()
    //{
    //    string strSQL;
    //    SqlConnection con;
    //    SqlDataAdapter oOrdersDataAdapter;
    //    con = new SqlConnection(cn);
    //    con.Open();

    //    strSQL = "SELECT CLIENT_ID,CLINT_TITLE,CLINT_M_NAME,CLINT_L_NAME,CLINT_NAME,CLINT_ADDRESS1,CLINT_ADDRESS2,CLINT_TOWN,CLINT_POSTCODE,CLINT_CITY,"
    //          + "COUNTRY_ID,CLINT_CONTACT_F_NAME,CLINT_CONTACT_M_NAME,CLINT_CONTACT_L_NAME,CLINT_JOB_TITLE, CLINT_CONTACT_EMAIL,CLINT_LAND_LINE,"
    //          + "CLINT_MOBILE,CLINT_FAX,CLINT_CONTACT_TITLE,CLINT_GENDER,CLINT_N_ID,CLINT_PASSPORT_NO FROM CLIENT_LIST";

    //    strSQL = "SELECT CLINT_ID,CLINT_TITLE||' '||CLINT_NAME||' '||CLINT_M_NAME||' '||CLINT_L_NAME NAME,"
    //           + "DECODE(CLINT_GENDER,'M','Male','F','Female') Gender,CLINT_N_ID ZIP_NID,CLINT_PASSPORT_NO Passport_No FROM CLIENT_LIST";
    //    DataSet dstClientList = new DataSet();
    //    SqlDataAdapter adpClientList = new SqlDataAdapter(new SqlCommand(strSQL, con));
    //    SqlCommandBuilder cmbClientList = new SqlCommandBuilder(adpClientList);
    //    adpClientList.Fill(dstClientList, "CLIENT_LIST");

    //    ExportDataSetToExcel(dstClientList, "CLIENT_LIST_" + DateTime.Now.ToShortDateString() + ".xls");
    //}
    public static void ExportDataSetToExcel(DataSet ds, string filename)
    {
        HttpResponse response = HttpContext.Current.Response;

        // first let's clean up the response.object
        response.Clear();
        response.Charset = "";

        // set the response mime type for excel
        response.ContentType = "application/vnd.ms-excel";
        response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");

        // create a string writer
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                // instantiate a datagrid
                DataGrid dg = new DataGrid();
                dg.DataSource = ds.Tables[0];
                dg.DataBind();
                dg.RenderControl(htw);
                response.Write(sw.ToString());
                //response.End();              
               // response.Redirect("HRM_Leave_Approval_HR.aspx", false);
              
            }
        }
    }

    public static void ExportToMSWord(string fileName, string strType,string strContent,string strPageOrientation)
    {
        fileName = fileName + "_" + string.Format("{0:ddmmyy_hhmmss}", DateTime.Now);
        if (strType.ToLower().Equals("msword"))
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "application/msword";
            fileName = fileName + ".doc";
            HttpContext.Current.Response.ContentEncoding = System.Text.UnicodeEncoding.UTF8;
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.Write("<html>");
            HttpContext.Current.Response.Write("<head>");
            HttpContext.Current.Response.Write("<META HTTP-EQUIV=\" Content-Type\" CONTENT=\" text/html; charset=UTF-8\">");
            HttpContext.Current.Response.Write("<meta name=ProgId content=Word.Document>");
            HttpContext.Current.Response.Write("<meta name=Generator content=\"Microsoft Word 9\">");
            HttpContext.Current.Response.Write("<meta name=Originator content=\"Microsoft Word 9\">");
            HttpContext.Current.Response.Write("<style>");

            // Following was Comment -- asad
            HttpContext.Current.Response.Write("@page Section1 {size:595.45pt 841.7pt; margin:1.0in 1.25in 1.0in 1.25in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            HttpContext.Current.Response.Write("div.Section1 {page:Section1;}");
            HttpContext.Current.Response.Write("@page Section2 {size:841.7pt 595.45pt;mso-page-orientation:" + strPageOrientation + ";margin:1.25in 1.0in 1.25in 1.0in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            HttpContext.Current.Response.Write("div.Section2 {page:Section2;}");

            HttpContext.Current.Response.Write("@page Section1 {size:595.45pt 841.7pt; margin:1.0in 1.25in 1.0in 1.25in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            HttpContext.Current.Response.Write("div.Section1 {page:Section1;}");
            HttpContext.Current.Response.Write("@page Section2 {size:841.7pt 595.45pt;mso-page-orientation:" + strPageOrientation + ";margin:1.25in 1.0in 1.25in 1.0in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            HttpContext.Current.Response.Write("@page Section2 {size:595.45pt 841.7pt;mso-page-orientation:" + strPageOrientation + ";margin:1.0in 1.0in 1.0in 1.0in;mso-paper-source:0;}");
            HttpContext.Current.Response.Write("div.Section2 {page:Section2;}");
            // End of above was Comment -- asad
            HttpContext.Current.Response.Write("</style>");
            HttpContext.Current.Response.Write("</head>");
        }
        else
        {

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "application/pdf";
            fileName = fileName + ".pdf";
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.Write("<html>");
            HttpContext.Current.Response.Write("<head>");
            HttpContext.Current.Response.Write("</head>");

        }
        HttpContext.Current.Response.Write("<body>");
        HttpContext.Current.Response.Write("<div class=Section2>");

        HttpContext.Current.Response.Write(strContent);
        HttpContext.Current.Response.Write("</div>");
        HttpContext.Current.Response.Write("</body>");
        HttpContext.Current.Response.Write("</html>");
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }
    public static void ExportToMSWordAgreement(string fileName, string strType, string strContent, string strPageOrientation)
    {
        fileName = fileName + "_" + string.Format("{0:ddmmyy_hhmmss}", DateTime.Now);
        if (strType.ToLower().Equals("msword"))
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "application/msword";
            fileName = fileName + ".doc";
            HttpContext.Current.Response.ContentEncoding = System.Text.UnicodeEncoding.UTF8;
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.Write("<html>");
            HttpContext.Current.Response.Write("<head>");
            HttpContext.Current.Response.Write("<META HTTP-EQUIV=\" Content-Type\" CONTENT=\" text/html; charset=UTF-8\">");
            HttpContext.Current.Response.Write("<meta name=ProgId content=Word.Document>");
            HttpContext.Current.Response.Write("<meta name=Generator content=\"Microsoft Word 9\">");
            HttpContext.Current.Response.Write("<meta name=Originator content=\"Microsoft Word 9\">");
            HttpContext.Current.Response.Write("<style>");

            HttpContext.Current.Response.Write("@page Section1 {margin-top: 500px;}");
            HttpContext.Current.Response.Write("div.Section1 {page:Section1;}");

            //HttpContext.Current.Response.Write("@page Section1 {size:595.45pt 841.7pt; margin:1.0in 1.25in 1.0in 1.25in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            //HttpContext.Current.Response.Write("div.Section1 {page:Section1;}");
            //HttpContext.Current.Response.Write("@page Section2 {size:841.7pt 595.45pt;mso-page-orientation:" + strPageOrientation + ";margin:1.25in 1.0in 1.25in 1.0in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            //HttpContext.Current.Response.Write("div.Section2 {page:Section2;}");

            //HttpContext.Current.Response.Write("@page Section1 {size:595.45pt 841.7pt; margin:1.0in 1.25in 1.0in 1.25in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            //HttpContext.Current.Response.Write("div.Section1 {page:Section1;}");
            //HttpContext.Current.Response.Write("@page Section2 {size:841.7pt 595.45pt;mso-page-orientation:" + strPageOrientation + ";margin:1.25in 1.0in 1.25in 1.0in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            //HttpContext.Current.Response.Write("@page Section2 {size:595.45pt 841.7pt;mso-page-orientation:" + strPageOrientation + ";margin:1.0in 1.0in 1.0in 1.0in;mso-paper-source:0;}");
            //HttpContext.Current.Response.Write("div.Section2 {page:Section2;}");
            HttpContext.Current.Response.Write("</style>");
            HttpContext.Current.Response.Write("</head>");
        }
        else
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "application/pdf";
            fileName = fileName + ".pdf";
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.Write("<html>");
            HttpContext.Current.Response.Write("<head>");
            HttpContext.Current.Response.Write("</head>");
        }


        HttpContext.Current.Response.Write("<body>");
        HttpContext.Current.Response.Write("<div class=Section1>");
        HttpContext.Current.Response.Write("</div>");
        HttpContext.Current.Response.Write("<div class=Section2>");
        HttpContext.Current.Response.Write(strContent);
        HttpContext.Current.Response.Write("</div>");
        HttpContext.Current.Response.Write("</body>");
        HttpContext.Current.Response.Write("</html>");
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public static void ExportToMSExcel(string fileName, string strType, string strContent, string strPageOrientation)
    {
        fileName = fileName + "_" + string.Format("{0:ddmmyy_hhmmss}", DateTime.Now);
        if (strType.ToLower().Equals("msexcel"))
        {
            
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "application/msexcel";
            fileName = fileName + ".xls";

            HttpContext.Current.Response.ContentEncoding = System.Text.UnicodeEncoding.UTF8;
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.Write("<html>");
            HttpContext.Current.Response.Write("<head>");
            HttpContext.Current.Response.Write("<META HTTP-EQUIV=\" Content-Type\" CONTENT=\" text/html; charset=UTF-8\">");
            HttpContext.Current.Response.Write("<meta name=ProgId content=Word.Document>");
            HttpContext.Current.Response.Write("<meta name=Generator content=\"Microsoft Word 9\">");
            HttpContext.Current.Response.Write("<meta name=Originator content=\"Microsoft Word 9\">");
            //HttpContext.Current.Response.Write("<style>");

            //HttpContext.Current.Response.Write("@page Section1 {size:595.45pt 841.7pt; margin:1.0in 1.25in 1.0in 1.25in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            //HttpContext.Current.Response.Write("div.Section1 {page:Section1;}");
            //HttpContext.Current.Response.Write("@page Section2 {size:841.7pt 595.45pt;mso-page-orientation:" + strPageOrientation + ";margin:1.25in 1.0in 1.25in 1.0in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            //HttpContext.Current.Response.Write("div.Section2 {page:Section2;}");

            //HttpContext.Current.Response.Write("@page Section1 {size:595.45pt 841.7pt; margin:1.0in 1.25in 1.0in 1.25in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            //HttpContext.Current.Response.Write("div.Section1 {page:Section1;}");
            //HttpContext.Current.Response.Write("@page Section2 {size:841.7pt 595.45pt;mso-page-orientation:" + strPageOrientation + ";margin:1.25in 1.0in 1.25in 1.0in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
            //HttpContext.Current.Response.Write("@page Section2 {size:595.45pt 841.7pt;mso-page-orientation:" + strPageOrientation + ";margin:1.0in 1.0in 1.0in 1.0in;mso-paper-source:0;}");
            //HttpContext.Current.Response.Write("div.Section2 {page:Section2;}");
            //HttpContext.Current.Response.Write("</style>");
            HttpContext.Current.Response.Write("</head>");
        }
        else
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "application/pdf";
            fileName = fileName + ".pdf";
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.Write("<html>");
            HttpContext.Current.Response.Write("<head>");
            HttpContext.Current.Response.Write("</head>");
        }


        HttpContext.Current.Response.Write("<body>");
        HttpContext.Current.Response.Write("<div class=Section2>");
        //HttpContext.Current.Response.Write("<style>.textmode{mso-number-format:\\@;}");
                HttpContext.Current.Response.Write(strContent);
        //HttpContext.Current.Response.Write("</style>");
        HttpContext.Current.Response.Write("</div>");
        HttpContext.Current.Response.Write("</body>");
        HttpContext.Current.Response.Write("</html>");
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

   
}
