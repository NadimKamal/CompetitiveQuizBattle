<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SYS_Acc_Policy.aspx.cs" Inherits="System_SYS_Acc_Policy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Access Policy</title>
    <link type="text/css" rel="stylesheet" href="../css/style.css" />
</head>
<body style="background-color: lightgrey">
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
     </asp:ScriptManager>
      <div style="background-color: lightgrey">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
          <contenttemplate>
            <div>
                <asp:SqlDataSource ID="sdsSysGroup" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:sqlConString %>" ProviderName="<%$ ConnectionStrings:sqlConString.ProviderName %>" 
                
                    SelectCommand='SELECT SYS_USR_GRP_ID, SYS_USR_GRP_TITLE FROM CM_SYSTEM_USER_GROUP where CMP_BRANCH_ID=@CMP_BRANCH_ID'                   
                    
                    >

                     <SelectParameters>
                <asp:SessionParameter Name="CMP_BRANCH_ID" SessionField="Branch_ID" Type="String" />
            </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="sdsSysMenu" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:sqlConString %>" ProviderName="<%$ ConnectionStrings:sqlConString.ProviderName %>" 
                
                    SelectCommand='SELECT SYS_MENU_ID, SYS_MENU_TITLE FROM CM_SYSTEM_MENU ORDER BY SYS_MENU_TITLE'></asp:SqlDataSource>
                <asp:SqlDataSource ID="sdsRemainSysMenu" runat="server" ConnectionString="<%$ ConnectionStrings:sqlConString %>"
                    ProviderName="<%$ ConnectionStrings:sqlConString.ProviderName %>" 
                    
                    SelectCommand="SELECT DISTINCT SYS_MENU_ID, SYS_MENU_TITLE, SYS_MENU_SERIAL FROM CM_SYSTEM_MENU WHERE (SYS_MENU_ID NOT IN (SELECT DISTINCT SYS_MENU_ID FROM CM_SYSTEM_ACCESS_POLICY WHERE (SYS_USR_GRP_ID = @SYS_USR_GRP_ID))) ORDER BY SYS_MENU_SERIAL">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlUserGroup" Name="SYS_USR_GRP_ID" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="sdsSysAccessPolicy" runat="server" 
                    ConflictDetection="CompareAllValues" 
                    ConnectionString="<%$ ConnectionStrings:sqlConString %>" 
                    DeleteCommand='DELETE FROM CM_SYSTEM_ACCESS_POLICY WHERE (SYS_ACCP_ID = @original_SYS_ACCP_ID) AND (SYS_USR_GRP_ID = @original_SYS_USR_GRP_ID OR SYS_USR_GRP_ID IS NULL AND @original_SYS_USR_GRP_ID IS NULL) AND (SYS_MENU_ID = @original_SYS_MENU_ID OR SYS_MENU_ID IS NULL AND @original_SYS_MENU_ID IS NULL) AND (SYS_ACCP_VIEW = @original_SYS_ACCP_VIEW OR SYS_ACCP_VIEW IS NULL AND @original_SYS_ACCP_VIEW IS NULL) AND (SYS_ACCP_ADD = @original_SYS_ACCP_ADD OR SYS_ACCP_ADD IS NULL AND @original_SYS_ACCP_ADD IS NULL) AND (SYS_ACCP_DELETE = @original_SYS_ACCP_DELETE OR SYS_ACCP_DELETE IS NULL AND @original_SYS_ACCP_DELETE IS NULL) AND (SYS_ACCP_EDIT = @original_SYS_ACCP_EDIT OR SYS_ACCP_EDIT IS NULL AND @original_SYS_ACCP_EDIT IS NULL) AND (SYS_ACCP_PRINT = @original_SYS_ACCP_PRINT OR SYS_ACCP_PRINT IS NULL AND @original_SYS_ACCP_PRINT IS NULL)' 
                    InsertCommand='INSERT INTO CM_SYSTEM_ACCESS_POLICY(SYS_ACCP_ID, SYS_USR_GRP_ID, SYS_MENU_ID, SYS_ACCP_VIEW, SYS_ACCP_ADD, SYS_ACCP_DELETE, SYS_ACCP_EDIT, SYS_ACCP_PRINT,SYS_ACCP_SEARCH) VALUES (@SYS_ACCP_ID, @SYS_USR_GRP_ID, @SYS_MENU_ID, @SYS_ACCP_VIEW, @SYS_ACCP_ADD, @SYS_ACCP_DELETE, @SYS_ACCP_EDIT, @SYS_ACCP_PRINT,"")' 
                    OldValuesParameterFormatString="original_{0}" 
                    ProviderName="<%$ ConnectionStrings:sqlConString.ProviderName %>" 
                    SelectCommand='SELECT SP.SYS_ACCP_ID, SP.SYS_USR_GRP_ID, SP.SYS_MENU_ID, SP.SYS_ACCP_VIEW, SP.SYS_ACCP_ADD, SP.SYS_ACCP_DELETE, SP.SYS_ACCP_EDIT, SP.SYS_ACCP_PRINT FROM CM_SYSTEM_ACCESS_POLICY AS SP INNER JOIN CM_SYSTEM_MENU AS SM ON SP.SYS_MENU_ID = SM.SYS_MENU_ID WHERE (SP.SYS_USR_GRP_ID = @SYS_USR_GRP_ID) ORDER BY SM.SYS_MENU_SERIAL' 
                    
                    
                    
                    UpdateCommand='UPDATE "CM_SYSTEM_ACCESS_POLICY" 
SET "SYS_USR_GRP_ID" = @SYS_USR_GRP_ID, "SYS_MENU_ID" =@SYS_MENU_ID, "SYS_ACCP_VIEW" = @SYS_ACCP_VIEW, "SYS_ACCP_ADD" = @SYS_ACCP_ADD, "SYS_ACCP_DELETE" = @SYS_ACCP_DELETE, "SYS_ACCP_EDIT" = @SYS_ACCP_EDIT, "SYS_ACCP_PRINT" = @SYS_ACCP_PRINT WHERE "SYS_ACCP_ID" = @original_SYS_ACCP_ID AND (("SYS_USR_GRP_ID" = @original_SYS_USR_GRP_ID) OR ("SYS_USR_GRP_ID" IS NULL AND @original_SYS_USR_GRP_ID IS NULL)) AND (("SYS_MENU_ID" = @original_SYS_MENU_ID) OR ("SYS_MENU_ID" IS NULL AND @original_SYS_MENU_ID IS NULL)) AND (("SYS_ACCP_VIEW" = @original_SYS_ACCP_VIEW) OR ("SYS_ACCP_VIEW" IS NULL AND @original_SYS_ACCP_VIEW IS NULL)) AND (("SYS_ACCP_ADD" = @original_SYS_ACCP_ADD) OR ("SYS_ACCP_ADD" IS NULL AND @original_SYS_ACCP_ADD IS NULL)) AND (("SYS_ACCP_DELETE" = @original_SYS_ACCP_DELETE) OR ("SYS_ACCP_DELETE" IS NULL AND @original_SYS_ACCP_DELETE IS NULL)) AND (("SYS_ACCP_EDIT" = @original_SYS_ACCP_EDIT) OR ("SYS_ACCP_EDIT" IS NULL AND @original_SYS_ACCP_EDIT IS NULL)) AND (("SYS_ACCP_PRINT" = @original_SYS_ACCP_PRINT) OR ("SYS_ACCP_PRINT" IS NULL AND @original_SYS_ACCP_PRINT IS NULL))'>
                    <DeleteParameters>
                        <asp:Parameter Name="original_SYS_ACCP_ID" Type="String" />
                        <asp:Parameter Name="original_SYS_USR_GRP_ID" Type="String" />
                        <asp:Parameter Name="original_SYS_MENU_ID" Type="String" />
                        <asp:Parameter Name="original_SYS_ACCP_VIEW" Type="String" />
                        <asp:Parameter Name="original_SYS_ACCP_ADD" Type="String" />
                        <asp:Parameter Name="original_SYS_ACCP_DELETE" Type="String" />
                        <asp:Parameter Name="original_SYS_ACCP_EDIT" Type="String" />
                        <asp:Parameter Name="original_SYS_ACCP_PRINT" Type="String" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="SYS_USR_GRP_ID" Type="String" />
                        <asp:Parameter Name="SYS_MENU_ID" Type="String" />
                        <asp:Parameter Name="SYS_ACCP_VIEW" Type="String" />
                        <asp:Parameter Name="SYS_ACCP_ADD" Type="String" />
                        <asp:Parameter Name="SYS_ACCP_DELETE" Type="String" />
                        <asp:Parameter Name="SYS_ACCP_EDIT" Type="String" />
                        <asp:Parameter Name="SYS_ACCP_PRINT" Type="String" />
                        <asp:Parameter Name="original_SYS_ACCP_ID" Type="String" />
                        <asp:Parameter Name="original_SYS_USR_GRP_ID" Type="String" />
                        <asp:Parameter Name="original_SYS_MENU_ID" Type="String" />
                        <asp:Parameter Name="original_SYS_ACCP_VIEW" Type="String" />
                        <asp:Parameter Name="original_SYS_ACCP_ADD" Type="String" />
                        <asp:Parameter Name="original_SYS_ACCP_DELETE" Type="String" />
                        <asp:Parameter Name="original_SYS_ACCP_EDIT" Type="String" />
                        <asp:Parameter Name="original_SYS_ACCP_PRINT" Type="String" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="SYS_ACCP_ID" Type="String" />
                        <asp:Parameter Name="SYS_USR_GRP_ID" Type="String" />
                        <asp:Parameter Name="SYS_MENU_ID" Type="String" />
                        <asp:Parameter Name="SYS_ACCP_VIEW" Type="String" />
                        <asp:Parameter Name="SYS_ACCP_ADD" Type="String" />
                        <asp:Parameter Name="SYS_ACCP_DELETE" Type="String" />
                        <asp:Parameter Name="SYS_ACCP_EDIT" Type="String" />
                        <asp:Parameter Name="SYS_ACCP_PRINT" Type="String" />
                    </InsertParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlUserGroup" Name="SYS_USR_GRP_ID" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <table>
                    <tr>
                        <td valign="top">
                <DIV style="BACKGROUND-COLOR: royalblue"><STRONG><SPAN style="COLOR: white">System Access Policy For
                    <asp:DropDownList ID="ddlUserGroup" runat="server" AutoPostBack="True" DataSourceID="sdsSysGroup"
                        DataTextField="SYS_USR_GRP_TITLE" DataValueField="SYS_USR_GRP_ID" Style="position: relative">
                    </asp:DropDownList>
                    &nbsp;
                    <asp:Button ID="btnAddAll" runat="server" OnClick="btnAddAll_Click" Style="position: relative"
                        Text="Add All" />
                    <asp:Label ID="lblShowMsg" runat="server" Font-Bold="True" ForeColor="Red" Style="
                        position: relative;"></asp:Label></SPAN></STRONG></DIV>
                    <div>
                        <asp:GridView ID="gdvSysAccessPolicy" runat="server" AllowSorting="True"
                            AutoGenerateColumns="False" DataKeyNames="SYS_ACCP_ID" DataSourceID="sdsSysAccessPolicy"
                            Style="position: relative" BorderColor="#E0E0E0" AllowPaging="True" PageSize="14"
                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="SYS_ACCP_ID" HeaderText="Access ID" ReadOnly="True" SortExpression="SYS_ACCP_ID" Visible="False" />
                                <asp:TemplateField HeaderText="User Group" SortExpression="SYS_USR_GRP_ID">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGdvUsrGrpEI" runat="server" DataSourceID="sdsSysGroup" DataTextField="SYS_USR_GRP_TITLE"
                                            DataValueField="SYS_USR_GRP_ID" SelectedValue='<%# Bind("SYS_USR_GRP_ID") %>'
                                            Style="position: relative">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlGdvUsrGrpI" runat="server" DataSourceID="sdsSysGroup" DataTextField="SYS_USR_GRP_TITLE"
                                            DataValueField="SYS_USR_GRP_ID" Enabled="False" SelectedValue='<%# Bind("SYS_USR_GRP_ID") %>'
                                            Style="position: relative">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Menu" SortExpression="SYS_MENU_ID">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGdvSysMenuEI" runat="server" DataSourceID="sdsSysMenu" DataTextField="SYS_MENU_TITLE"
                                            DataValueField="SYS_MENU_ID" SelectedValue='<%# Bind("SYS_MENU_ID") %>' Style="position: relative">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlGdvSysMenuI" runat="server" DataSourceID="sdsSysMenu" DataTextField="SYS_MENU_TITLE"
                                            DataValueField="SYS_MENU_ID" Enabled="False" SelectedValue='<%# Bind("SYS_MENU_ID") %>'
                                            Style="position: relative">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View" SortExpression="SYS_ACCP_VIEW">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGdvViewEI" runat="server" SelectedValue='<%# Bind("SYS_ACCP_VIEW") %>'
                                            Style="position: relative">
                                            <asp:ListItem Selected="True" Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlGdvViewI" runat="server" Enabled="False" SelectedValue='<%# Bind("SYS_ACCP_VIEW") %>'
                                            Style="position: relative">
                                            <asp:ListItem Selected="True" Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Addition" SortExpression="SYS_ACCP_ADD">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGdvAddEI" runat="server" SelectedValue='<%# Bind("SYS_ACCP_ADD") %>'
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlGdvAddI" runat="server" Enabled="False" SelectedValue='<%# Bind("SYS_ACCP_ADD") %>'
                                            Style="position: relative">
                                            <asp:ListItem Selected="True" Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" SortExpression="SYS_ACCP_DELETE">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGdvDeleteEI" runat="server" SelectedValue='<%# Bind("SYS_ACCP_DELETE") %>'
                                            Style="position: relative">
                                            <asp:ListItem Selected="True" Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlGdvDeleteI" runat="server" Enabled="False" SelectedValue='<%# Bind("SYS_ACCP_DELETE") %>'
                                            Style="position: relative">
                                            <asp:ListItem Selected="True" Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" SortExpression="SYS_ACCP_EDIT">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGdvEditEI" runat="server" SelectedValue='<%# Bind("SYS_ACCP_EDIT") %>'
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlGdvEditI" runat="server" Enabled="False" SelectedValue='<%# Bind("SYS_ACCP_EDIT") %>'
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Print" SortExpression="SYS_ACCP_PRINT">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGdvPrintEI" runat="server" SelectedValue='<%# Bind("SYS_ACCP_PRINT") %>'
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlGdvPrintI" runat="server" Enabled="False" SelectedValue='<%# Bind("SYS_ACCP_PRINT") %>'
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                            </Columns>
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                        </asp:GridView>
                        <br />
                        &nbsp;</div>
                        </td>
                        <td valign="top">
                 <DIV style="BACKGROUND-COLOR: royalblue"><STRONG><SPAN style="COLOR: white">Add System Access Policy</SPAN></STRONG></DIV>
                    <div>
                          <table>
                            <tr>
                                <td>
                                    <asp:Label ID="label1" runat="server" Text="Menu"></asp:Label>
                                </td>
                                <td>
                                      <asp:DropDownList ID="ddlDdvSysMenuEI" runat="server" DataSourceID="sdsRemainSysMenu" DataTextField="SYS_MENU_TITLE"
                                            DataValueField="SYS_MENU_ID"  Style="position: relative">
                                        </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="label2" runat="server" Text="View"></asp:Label>
                                </td>
                                <td>
                                     <asp:DropDownList ID="ddlDdvViewEI" runat="server" 
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y" Selected="True">YES</asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="label8" runat="server" Text="Addition"></asp:Label>
                                </td>
                                <td>
                                      <asp:DropDownList ID="ddlDdvAddI" runat="server" 
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                         <asp:Label ID="label9" runat="server" Text="Delete"></asp:Label>
                                </td>
                                <td>
                                     <asp:DropDownList ID="ddlDdvDeleteEI" runat="server" 
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                         <asp:Label ID="label10" runat="server" Text="Edit"></asp:Label>
                                </td>
                                <td>
                                     <asp:DropDownList ID="ddlDdvEditEI" runat="server"
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                         <asp:Label ID="label11" runat="server" Text="print"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDdvPrintEI" runat="server" 
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Label ID="label3" runat="server" Text="Search"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDdvSearchEI" runat="server" 
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>

                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                </td>
                            </tr>
                        </table>
                      <%--  <asp:DetailsView ID="dtvSysAccessPolicy" runat="server" AutoGenerateRows="False"
                            DataKeyNames="SYS_ACCP_ID" DataSourceID="sdsSysAccessPolicy" DefaultMode="Insert"
                            Height="50px" Style="position: relative" Width="125px" BorderColor="#E0E0E0" OnItemInserting="dtvSysAccessPolicy_ItemInserting">
                            <Fields>
                                <asp:BoundField DataField="SYS_ACCP_ID" HeaderText="SYS_ACCP_ID" ReadOnly="True"
                                    SortExpression="SYS_ACCP_ID" Visible="False" />
                                
                                <asp:TemplateField HeaderText="Menu" SortExpression="SYS_MENU_ID">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("SYS_MENU_ID") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlDdvSysMenuEI" runat="server" DataSourceID="sdsRemainSysMenu" DataTextField="SYS_MENU_TITLE"
                                            DataValueField="SYS_MENU_ID" SelectedValue='<%# Bind("SYS_MENU_ID") %>' Style="position: relative">
                                        </asp:DropDownList>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("SYS_MENU_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:TemplateField>
                                
                                
                                <asp:TemplateField HeaderText="View" SortExpression="SYS_ACCP_VIEW">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("SYS_ACCP_VIEW") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlDdvViewEI" runat="server" SelectedValue='<%# Bind("SYS_ACCP_VIEW") %>'
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y" Selected="True">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("SYS_ACCP_VIEW") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:TemplateField>
                                
                                
                                <asp:TemplateField HeaderText="Addition" SortExpression="SYS_ACCP_ADD">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("SYS_ACCP_ADD") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlDdvAddI" runat="server" SelectedValue='<%# Bind("SYS_ACCP_ADD") %>'
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("SYS_ACCP_ADD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:TemplateField>
                                
                                
                                <asp:TemplateField HeaderText="Delete" SortExpression="SYS_ACCP_DELETE">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("SYS_ACCP_DELETE") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlDdvDeleteEI" runat="server" SelectedValue='<%# Bind("SYS_ACCP_DELETE") %>'
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("SYS_ACCP_DELETE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:TemplateField>
                                
                                
                                <asp:TemplateField HeaderText="Edit" SortExpression="SYS_ACCP_EDIT">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("SYS_ACCP_EDIT") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlDdvEditEI" runat="server" SelectedValue='<%# Bind("SYS_ACCP_EDIT") %>'
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("SYS_ACCP_EDIT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:TemplateField>
                                
                                
                                <asp:TemplateField HeaderText="Print" SortExpression="SYS_ACCP_PRINT">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("SYS_ACCP_PRINT") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlDdvPrintEI" runat="server" SelectedValue='<%# Bind("SYS_ACCP_PRINT") %>'
                                            Style="position: relative">
                                            <asp:ListItem Value="N">NO</asp:ListItem>
                                            <asp:ListItem Value="Y">YES</asp:ListItem>
                                        </asp:DropDownList>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("SYS_ACCP_PRINT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:TemplateField>
                                
                                
                                <asp:CommandField ButtonType="Button" ShowInsertButton="True" >
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:CommandField>
                            </Fields>
                        </asp:DetailsView>--%>
                        
                        </div>
                        </td>
                    </tr> 
                </table>
            </div>
          </contenttemplate>
        </asp:UpdatePanel>    
       </div>
    </form>
</body>
</html>
