<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmManageSubjectOrCourse.aspx.cs" Inherits="COMMON_frmManageSubjectOrCourse" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

  
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Manage Subject or Course</title>
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
                           Subject Code
                        </td>&nbsp;&nbsp;
                        <td>
                            <asp:TextBox ID="txtBranchCode" runat="server" Width="225px" Height="22px"></asp:TextBox>
                        </td>
                    </tr>

                   <tr>
                        <td>
                           Subject Name
                        </td>&nbsp;&nbsp;
                        <td>
                            <asp:TextBox ID="txtSubName" runat="server" Width="225px" Height="22px"></asp:TextBox>
                        </td>
                    </tr>

                     <tr>
                        <td style="width:100px;">
                            Select Type
                        </td>&nbsp;&nbsp;
                        <td>
                        <asp:DropDownList ID="ddlTypeList" runat="server" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="OnSelectedIndexChanged_type"> 
                            
                        </asp:DropDownList> 
                        </td>
                    </tr>
                   

                    <tr>
                        <td style="width:100px;">
                            Department or Class
                        </td>&nbsp;&nbsp;
                        <td>
                        <asp:DropDownList ID="ddlBrType" runat="server" AutoPostBack="true" EnableViewState="true"> 
                            
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
                            <asp:Label ID="Label2" runat="server" Text="Subject or Course List"></asp:Label>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Panel ID="pnlSearch" runat="server">
                <table>
                    <tr>
                        <td>Search By Subject Name : </td>
                        <td><asp:TextBox ID="txtSearch" runat="server"></asp:TextBox></td>
                        <td><asp:Button ID="btnSearch" runat="server" Text="Search" 
                                onclick="btnSearch_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Panel ID="pnlGridView" runat="server">
                <asp:GridView ID="gdvBranchList" runat="server" AutoGenerateColumns="False" DataKeyNames="SYS_SUB_ID"
                    CssClass="mGrid" PageSize="10" AllowPaging="true" Width="100%" 
                    EmptyDataText="No data found....." 
                    onpageindexchanging="gdvBranchList_PageIndexChanging" 
                    onrowcancelingedit="gdvBranchList_RowCancelingEdit" 
                    onrowdeleting="gdvBranchList_RowDeleting" 
                    onrowediting="gdvBranchList_RowEditing" 
                    onselectedindexchanging="gdvBranchList_SelectedIndexChanging" 
                    onrowupdating="gdvBranchList_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="Subject Id" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchId" Visible="false" runat="server" Text='<%#Bind("SYS_SUB_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Subject Code" ControlStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchName" runat="server" Text='<%#Bind("SYS_SUB_NAME") %>'></asp:Label>
                            </ItemTemplate>
                              <EditItemTemplate>
                                <asp:TextBox ID="gvtxtBranchName" runat="server" Text='<%#Bind("SYS_SUB_NAME") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Subject Name" ControlStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblsubCode" runat="server" Text='<%#Bind("SYS_SUB_CODE") %>'></asp:Label>
                            </ItemTemplate>
                              <EditItemTemplate>
                                <asp:TextBox ID="gvtxtsubCode" runat="server" Text='<%#Bind("SYS_SUB_CODE") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                      
                        <asp:TemplateField HeaderText="Added By" ControlStyle-Width="200">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchAddress" runat="server" Text='<%#Bind("SUB_ADDED_BY") %>'></asp:Label>
                            </ItemTemplate>
                           
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Addition Date" ControlStyle-Width="200">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchCode" runat="server" Text='<%#Bind("SUB_ADD_DATE") %>'></asp:Label>
                            </ItemTemplate>
                            
                        </asp:TemplateField>
                        
                         

                       

                         <asp:TemplateField HeaderText="Department" ControlStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchType" runat="server" Text='<%#Bind("SYS_DEPT_NAME") %>'></asp:Label>
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