<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SYS_System_Users.aspx.cs"
    Inherits="System_SYS_System_Users" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

  
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Manage User</title>
    <link type="text/css" rel="stylesheet" href="../css/style.css" />
   
    <script type="text/javascript">



</script>
</head>
  <body style="background-color: lightgrey;">
    <form id="form2" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlTop" runat="server" CssClass="Top_Panel" style="background-color: black; color:white">
                <table width="100%">
                    <tr>
                        <td class="style1">
                            <asp:Label ID="Label1" runat="server" Text="Add Subject or Course" ></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td align="right">
                            <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                                <ProgressTemplate>
                                    <img alt="Loading" src="../resources/images/loading.gif" />&nbsp;&nbsp;
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlInsert" runat="server">
                <table>

                     <tr>
                        <td>
                           Name
                        </td>&nbsp;&nbsp;
                        <td>
                            <asp:TextBox ID="txtName" runat="server" Width="225px" Height="22px"></asp:TextBox>
                        </td>
                    </tr>

                   <tr>
                        <td>
                           Login Name
                        </td>&nbsp;&nbsp;
                        <td>
                            <asp:TextBox ID="txtLogin" runat="server" Width="225px" Height="22px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           Password
                        </td>&nbsp;&nbsp;
                        <td>
                            <asp:TextBox ID="txtPass" runat="server" Width="225px" Height="22px"></asp:TextBox>
                        </td>
                    </tr>
                    
                     <tr>
                        <td style="width:100px;">
                            Group
                        </td>&nbsp;&nbsp;
                        <td>
                        <asp:DropDownList ID="ddlGroup" runat="server" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="OnSelectedIndexChanged_type"> 
                            
                        </asp:DropDownList> 
                        </td>
                    </tr>
                   

                    
                    
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" 
                                OnClick="btnSave_Click" Height="26px" />
                        </td>    
                    </tr>
                </table>
            </asp:Panel>
            <br />
            
            <asp:Panel ID="Panel4" runat="server" CssClass="Top_Panel" style="background-color: black; color:white">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="User List"></asp:Label>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Panel ID="pnlSearch" runat="server">
                <table>
                    <tr>
                        <td>Search By Name : </td>
                        <td><asp:TextBox ID="txtSearch" runat="server"></asp:TextBox></td>
                        <td><asp:Button ID="btnSearch" runat="server" Text="Search" 
                                onclick="btnSearch_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Panel ID="pnlGridView" runat="server">
                <asp:GridView ID="gdvBranchList" runat="server" AutoGenerateColumns="False" DataKeyNames="SYS_USR_ID"
                    CssClass="mGrid" PageSize="10" AllowPaging="true" Width="100%" 
                    EmptyDataText="No data found....." 
                    onpageindexchanging="gdvBranchList_PageIndexChanging" 
                    onrowcancelingedit="gdvBranchList_RowCancelingEdit" 
                    onrowdeleting="gdvBranchList_RowDeleting" 
                    onrowediting="gdvBranchList_RowEditing" 
                    onselectedindexchanging="gdvBranchList_SelectedIndexChanging" 
                    onrowupdating="gdvBranchList_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="User Id" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchId" Visible="false" runat="server" Text='<%#Bind("SYS_USR_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Name" ControlStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchName" runat="server" Text='<%#Bind("SYS_USR_DNAME") %>'></asp:Label>
                            </ItemTemplate>
                              <EditItemTemplate>
                                <asp:TextBox ID="gvtxtBranchName" runat="server" Text='<%#Bind("SYS_USR_DNAME") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Login Name" ControlStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblsubCode" runat="server" Text='<%#Bind("SYS_USR_LOGIN_NAME") %>'></asp:Label>
                            </ItemTemplate>
                              <EditItemTemplate>
                                <asp:TextBox ID="gvtxtsubCode" runat="server" Text='<%#Bind("SYS_USR_LOGIN_NAME") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                      
                        <asp:TemplateField HeaderText="Password" ControlStyle-Width="200">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchAddress" runat="server" Text='<%#Bind("SYS_USR_PASS") %>'></asp:Label>
                            </ItemTemplate>
                           
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="User Group" ControlStyle-Width="200">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchCode" runat="server" Text='<%#Bind("SYS_USR_GRP_TITLE") %>'></asp:Label>
                            </ItemTemplate>
                            
                        </asp:TemplateField>
                        
                        
                         <asp:TemplateField HeaderText="" ControlStyle-Width="100">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="edit" Text="Edit"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnUpdate" runat="server" CommandName="update" Text="Update"></asp:LinkButton> <br /> 
                                     <asp:LinkButton ID="btnDelete" runat="server" CommandName="delete" Text="Delete"></asp:LinkButton> <br /> 
                                    <asp:LinkButton ID="btnCancel" runat="server" CommandName="cancel" Text="Cancel"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>    
                    </Columns>
                    <HeaderStyle BackColor="Gray" ForeColor="White" />
                    <EditRowStyle BorderStyle="Outset" />
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>