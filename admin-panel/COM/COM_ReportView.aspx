<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="COM_ReportView.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <%--<link href="/aspnet_client/System_Web/4_0_30319/CrystalreportViewer13/css/default.css"
        rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript" src="../crystalreportviewers13/js/crviewer/crv.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="background-color: white;">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <CR:CrystalReportViewer ID="crystalReportViewer" runat="server" AutoDataBind="true" 
            EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" ToolPanelView="None" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="crystalReportViewer" />
            </Triggers>
        </asp:UpdatePanel>
        &nbsp;
    
    </div>
    </form>
</body>
</html>
