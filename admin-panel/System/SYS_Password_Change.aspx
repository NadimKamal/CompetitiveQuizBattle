<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SYS_Password_Change.aspx.cs" Inherits="System_SYS_Password_Change" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Password Change</title>
 
</head>
<body style="background-color: lightgrey;">
<link type="text/css" rel="stylesheet" href="../css/style.css" />
    
  <form id="form1" runat="server">
    <div>
            <ajaxToolkit:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ToolkitScriptManager1" />
          <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div>
                        <div>
                            <asp:SqlDataSource ID="sdsBranch" runat="server" ConnectionString="<%$ ConnectionStrings:sqlConString %>"
                            ProviderName="<%$ ConnectionStrings:sqlConString.ProviderName %>" 
                            
                            SelectCommand='SELECT "CMP_BRANCH_ID", "CMP_BRANCH_NAME" FROM "CM_CMP_BRANCH"'>
                            </asp:SqlDataSource>
                        </div>
                        <div>
                        
                        <div style="BACKGROUND-COLOR: royalblue"><STRONG><SPAN style="COLOR: white">Reset Password</SPAN></STRONG>
                        &nbsp;
                            <asp:Label ID="lblSpMessage" runat="server" Font-Bold="True" Font-Italic="True" 
                                Font-Size="12px" ForeColor="#CC0000"></asp:Label>
                           </div>
                           <asp:Panel runat="server" BackColor="white">
                             <span  style="color:Royalblue;" > <b >Instructions:&nbsp; </b> Total Minimum of 4 characters (where atleast of One letter and one Digit)
        </span>
                           </asp:Panel>
                        
        <table>
           
            <tr>
                <td align="right">
                    <asp:Label ID="Label3" runat="server" Text="Login Name" Font-Size="12px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLogInName" runat="server" Font-Size="12px" Enabled="False" 
                        Width="170px"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label4" runat="server" Text="Old Password"  Font-Size="12px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtOldPass" runat="server" TextMode="Password" 
                        Font-Size="12px" Width="170px" placeholder="previous password"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label1" runat="server" Text="Enter New Password" Font-Size="12px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNPassword" runat="server" TextMode="Password" 
                        Font-Size="12px" Width="170px" placeholder="new password" ></asp:TextBox>
                     <asp:RegularExpressionValidator ID="reExp" runat="server" 
                        ControlToValidate="txtNPassword" ErrorMessage="Look Instructions" 
                       ValidationExpression="^(?=.*?[a-z])(?=.*?[0-9]).{4,}$" 
                        
                        ValidationGroup="check">Look Instructions</asp:RegularExpressionValidator>
                </td>
                  <%--ValidationExpression="^(?=.{4,})[a-zA-Z0-9]+$" --%>
                 <%--ValidationExpression="^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#*!?$%^&amp;+=]).*$"--%> 
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label2" runat="server" Text="Rewrite New Password" Font-Size="12px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRepass" runat="server" TextMode="Password" Font-Size="12px" 
                        Width="170px" placeholder="retype new password"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ControlToValidate="txtRepass" ErrorMessage="Look Instructions" 
                        ValidationExpression="^(?=.*?[a-z])(?=.*?[0-9]).{4,}$" 
                        ValidationGroup="check">Look Instructions</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                     
                </td>
                <td align="left">
                    
                    <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" 
                        ValidationGroup="check" /> 
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                        onclick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </div>
                    </div>
                    
                
                   
        </ContentTemplate>
        </asp:UpdatePanel>
     </div>
    </div>
   </form>
</body>
</html>
