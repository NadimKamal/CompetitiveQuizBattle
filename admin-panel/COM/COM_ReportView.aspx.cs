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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class _Default : System.Web.UI.Page 
{
    private ReportDocument objCReport;

    private void ConfigureCrystalReports()
    {
        string strReportFile="";
        //if (strReportFile == "")
        //{
        //    strReportFile = "crptClientList.rpt";
        //}
        try
        {
            objCReport = new ReportDocument();
            DataSet dtsReport = new DataSet();
            if (!Session["ReportSQL"].ToString().Equals("NOTSQL"))
            {
                dtsReport = clsReports.GetReportData(Session["ReportSQL"].ToString());
                strReportFile = Session["ReportFile"].ToString();
                strReportFile = Server.MapPath(strReportFile);
                objCReport.Load(strReportFile);
                objCReport.SetDataSource(dtsReport.Tables[0]);
            }
            else
            {
                strReportFile = Session["ReportFile"].ToString();
                strReportFile = Server.MapPath(strReportFile);
                objCReport.Load(strReportFile);
                objCReport.SetDataSource(Session["Dual"]);
                
            }
            //ReportObject rptObject= objCReport.ReportDefinition.ReportObjects["txtCompany"];
            //objCReport.ParameterFields["Company"].DefaultValues = "Test";
            //objCReport.SetParameterValue("Company", "FLFIL");
            crystalReportViewer.ReportSource = objCReport;
            //------------------------------------------------
            //CrystalDecisions.CrystalReports.Engine.TextObject myTextObjectOnReport;
            //myTextObjectOnReport = (CrystalDecisions.CrystalReports.Engine.TextObject)objCReport.ReportDefinition.ReportObjects["txtCompany"];
            //myTextObjectOnReport.Text = Session["CompanyName"].ToString();

            //myTextObjectOnReport = (CrystalDecisions.CrystalReports.Engine.TextObject)objCReport.ReportDefinition.ReportObjects["txtBranch"];
            //myTextObjectOnReport.Text = "[ "+Session["CompanyBranch"].ToString()+" ]";

            //myTextObjectOnReport = (CrystalDecisions.CrystalReports.Engine.TextObject)objCReport.ReportDefinition.ReportObjects["txtUsername"];
            //myTextObjectOnReport.Text = Session["UserLoginName"].ToString();

            //myTextObjectOnReport = (CrystalDecisions.CrystalReports.Engine.TextObject)objCReport.ReportDefinition.ReportObjects["txtAddress"];
            //myTextObjectOnReport.Text = Session["CmpanyAddress"].ToString();

            //myTextObjectOnReport = (CrystalDecisions.CrystalReports.Engine.TextObject)objCReport.ReportDefinition.ReportObjects["txtComSlogans"];
            //myTextObjectOnReport.Text = Session["COMSlogans"].ToString();

            //myTextObjectOnReport = (CrystalDecisions.CrystalReports.Engine.TextObject)objCReport.ReportDefinition.ReportObjects["txtDate1"];
            //myTextObjectOnReport.Text = "[ " + Session["Date1"].ToString()+ " ]";
            //------------------------------------------------
            //objCReport.SetDatabaseLogon("BDMIT_ERP_101", "BDMIT_ERP_101");
            
            crystalReportViewer.DataBind();
            crystalReportViewer.RefreshReport();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
        }
    }
    private void SetCurrentValuesForParameterField(ParameterFields parameterFields, ArrayList arrayList)
    {
        ParameterValues currentParameterValues = new ParameterValues();
        foreach (object submittedValue in arrayList)
        {
            ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue();
            parameterDiscreteValue.Value = submittedValue.ToString();
            currentParameterValues.Add(parameterDiscreteValue);
        }

        ParameterField parameterField = parameterFields["Company"];
        parameterField.CurrentValues = currentParameterValues;

    }

    private ArrayList GetDefaultValuesFromParameterField(ParameterFields parameterFields)
    {
        ParameterField parameterField = parameterFields["Company"];
        ParameterValues defaultParameterValues = parameterField.DefaultValues;
        ArrayList arrayList = new ArrayList();
        foreach (ParameterValue parameterValue in defaultParameterValues)
        {
            if (!parameterValue.IsRange)
            {
                ParameterDiscreteValue parameterDiscreteValue = (ParameterDiscreteValue)parameterValue;
                arrayList.Add(parameterDiscreteValue.Value.ToString());
            }
        }

        return arrayList;
    }
    private void Page_Init(object sender, EventArgs e)
    {
        ConfigureCrystalReports();
    }
    protected void Page_Unload(Object sender , System.EventArgs e)
    {
       objCReport.Close();
       objCReport.Dispose();
       GC.Collect();
    }
}
