<%@Page Language="C#" AutoEventWireup="true" CodeFile="SYS_User_Group.aspx.cs" Inherits="System_SYS_User_Group" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Group</title>
    <link type="text/css" rel="stylesheet" href="../css/style.css" />
</head>
<body style="background-color: lightgrey;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="background-color: royalblue">
                <strong><span style="color: white">System User Group<strong> <span style="color: white">
                    <strong><span style="color: white">
                        <asp:DropDownList ID="ddlCmpBranch" runat="server" 
                    DataSourceID="sdsCmpBranch" DataTextField="CMP_BRANCH_NAME"
                            DataValueField="CMP_BRANCH_ID" Font-Size="11px" 
                    AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="#CC3300"></asp:Label>
                    </span></strong></span></strong></span></strong>
            </div>
            <div>
                <asp:SqlDataSource ID="sdsSysUsrGroup" runat="server" ConnectionString="<%$ ConnectionStrings:sqlConString %>"
                    DeleteCommand='DELETE FROM "CM_SYSTEM_USER_GROUP" WHERE "SYS_USR_GRP_ID" = @original_SYS_USR_GRP_ID'
                    InsertCommand='INSERT INTO "CM_SYSTEM_USER_GROUP" ("SYS_USR_GRP_ID", "SYS_USR_GRP_TITLE", "SYS_USR_GRP_PARENT", "SYS_USR_GRP_TYPE", "CMP_BRANCH_ID", "POSCL_ID") 
VALUES (@SYS_USR_GRP_ID, @SYS_USR_GRP_TITLE, @SYS_USR_GRP_PARENT,@SYS_USR_GRP_TYPE, @CMP_BRANCH_ID,@POSCL_ID)'
                    ProviderName="<%$ ConnectionStrings:sqlConString.ProviderName %>" 
                    SelectCommand='SELECT * FROM "CM_SYSTEM_USER_GROUP" WHERE ("CMP_BRANCH_ID" = @CMP_BRANCH_ID)'
                    UpdateCommand='UPDATE "CM_SYSTEM_USER_GROUP" SET "SYS_USR_GRP_TITLE" = @SYS_USR_GRP_TITLE, "SYS_USR_GRP_PARENT" = @SYS_USR_GRP_PARENT, "SYS_USR_GRP_TYPE" =@SYS_USR_GRP_TYPE, "CMP_BRANCH_ID" = @CMP_BRANCH_ID, "POSCL_ID" =@POSCL_ID WHERE "SYS_USR_GRP_ID" = @original_SYS_USR_GRP_ID'
                    OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlCmpBranch" Name="CMP_BRANCH_ID" PropertyName="SelectedValue"
                            Type="String" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="original_SYS_USR_GRP_ID" Type="String" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="SYS_USR_GRP_TITLE" Type="String" />
                        <asp:Parameter Name="SYS_USR_GRP_PARENT" Type="String" />
                        <asp:Parameter Name="SYS_USR_GRP_TYPE" Type="String" />
                        <asp:Parameter Name="CMP_BRANCH_ID" Type="String" />
                        <asp:Parameter Name="POSCL_ID" Type="String" />
                        <asp:Parameter Name="original_SYS_USR_GRP_ID" Type="String" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="SYS_USR_GRP_ID" Type="String" />
                        <asp:Parameter Name="SYS_USR_GRP_TITLE" Type="String" />
                        <asp:Parameter Name="SYS_USR_GRP_PARENT" Type="String" />
                        <asp:Parameter Name="SYS_USR_GRP_TYPE" Type="String" />
                        <asp:Parameter Name="CMP_BRANCH_ID" Type="String" />
                        <asp:Parameter Name="POSCL_ID" Type="String" />
                    </InsertParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="sdsBranch" runat="server" ConnectionString="<%$ ConnectionStrings:sqlConString %>"
                    ProviderName="<%$ ConnectionStrings:sqlConString.ProviderName %>" 
                    SelectCommand='SELECT "CMP_BRANCH_ID", "CMP_BRANCH_NAME" FROM "CM_CMP_BRANCH"'>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="sdsGroupParent" runat="server" ConnectionString="<%$ ConnectionStrings:sqlConString %>"
                    ProviderName="<%$ ConnectionStrings:sqlConString.ProviderName %>" SelectCommand='SELECT "SYS_USR_GRP_ID", "SYS_USR_GRP_TITLE" FROM "CM_SYSTEM_USER_GROUP"'
                    OldValuesParameterFormatString="original_{0}"></asp:SqlDataSource>
                <asp:SqlDataSource ID="sdsGroupType" runat="server" ConflictDetection="CompareAllValues"
                    ConnectionString="<%$ ConnectionStrings:sqlConString %>" DeleteCommand='DELETE FROM "CM_SYSTEM_USER_GROUP" WHERE "SYS_USR_GRP_ID" = @original_SYS_USR_GRP_ID AND (("SYS_USR_GRP_TYPE" = @original_SYS_USR_GRP_TYPE) OR ("SYS_USR_GRP_TYPE" IS NULL AND @original_SYS_USR_GRP_TYPE IS NULL))'
                    InsertCommand='INSERT INTO "CM_SYSTEM_USER_GROUP" ("SYS_USR_GRP_ID", "SYS_USR_GRP_TYPE") VALUES (@SYS_USR_GRP_ID, @SYS_USR_GRP_TYPE)'
                    OldValuesParameterFormatString="original_{0}" ProviderName="<%$ ConnectionStrings:sqlConString.ProviderName %>"
                    SelectCommand='SELECT "SYS_USR_GRP_ID", "SYS_USR_GRP_TYPE" FROM "CM_SYSTEM_USER_GROUP"'
                    
                    
                    
                    UpdateCommand='UPDATE "CM_SYSTEM_USER_GROUP" SET "SYS_USR_GRP_TYPE" = @SYS_USR_GRP_TYPE WHERE "SYS_USR_GRP_ID" = @original_SYS_USR_GRP_ID AND (("SYS_USR_GRP_TYPE" = @original_SYS_USR_GRP_TYPE) OR ("SYS_USR_GRP_TYPE" IS NULL AND@original_SYS_USR_GRP_TYPE IS NULL))'>
                    <DeleteParameters>
                        <asp:Parameter Name="original_SYS_USR_GRP_ID" Type="String" />
                        <asp:Parameter Name="original_SYS_USR_GRP_TYPE" Type="String" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="SYS_USR_GRP_TYPE" Type="String" />
                        <asp:Parameter Name="original_SYS_USR_GRP_ID" Type="String" />
                        <asp:Parameter Name="original_SYS_USR_GRP_TYPE" Type="String" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="SYS_USR_GRP_ID" Type="String" />
                        <asp:Parameter Name="SYS_USR_GRP_TYPE" Type="String" />
                    </InsertParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="sdsClientName" runat="server" ConnectionString="<%$ ConnectionStrings:sqlConString %>"
                    ProviderName="<%$ ConnectionStrings:sqlConString.ProviderName %>" 
                    SelectCommand="SELECT &quot;POSCL_ID&quot;, &quot;POSCL_NAME&quot; FROM &quot;POS_CLIENT_LIST&quot;">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="sdsCmpBranch" runat="server" ConnectionString="<%$ ConnectionStrings:sqlConString %>"
                    ProviderName="<%$ ConnectionStrings:sqlConString.ProviderName %>" SelectCommand="SELECT &quot;CMP_BRANCH_ID&quot;, &quot;CMP_BRANCH_NAME&quot; FROM &quot;CM_CMP_BRANCH&quot;">
                </asp:SqlDataSource>
                <asp:GridView ID="gdvSysUsrGroup" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    DataKeyNames="SYS_USR_GRP_ID" DataSourceID="sdsSysUsrGroup" BorderColor="#E0E0E0"
                    AllowPaging="True" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                    <Columns>
                        <asp:BoundField DataField="SYS_USR_GRP_ID" HeaderText=" Group ID" ReadOnly="True"
                            SortExpression="SYS_USR_GRP_ID" Visible="False" />
                        <asp:BoundField DataField="SYS_USR_GRP_TITLE" HeaderText="Group Title" SortExpression="SYS_USR_GRP_TITLE" />
                        <asp:TemplateField HeaderText="Parent Group" SortExpression="SYS_USR_GRP_PARENT">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownList3" runat="server" DataSourceID="sdsGroupParent"
                                    DataTextField="SYS_USR_GRP_TITLE" DataValueField="SYS_USR_GRP_ID" SelectedValue='<%# Bind("SYS_USR_GRP_PARENT") %>'
                                    Style="position: relative">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="sdsGroupParent"
                                    DataTextField="SYS_USR_GRP_TITLE" DataValueField="SYS_USR_GRP_ID" SelectedValue='<%# Bind("SYS_USR_GRP_PARENT") %>'
                                    Style="position: relative" Enabled="False" AppendDataBoundItems="true">
                                    <asp:ListItem Text="N/A" Value="" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Group Type" SortExpression="SYS_USR_GRP_TYPE">
                            <EditItemTemplate>
                                &nbsp;<asp:DropDownList ID="DropDownList5" runat="server" SelectedValue='<%# Bind("SYS_USR_GRP_TYPE") %>'
                                    Style="position: relative; left: -2px;" Width="107px">
                                    <asp:ListItem Selected="True" Value="GH">Group Header</asp:ListItem>
                                    <asp:ListItem Value="AG">Active Group</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="DropDownList4" runat="server" 
                                    Style="position: relative" Enabled="False">
                                    <asp:ListItem Selected="True" Value="GH">Group Header</asp:ListItem>
                                    <asp:ListItem Value="AG">Active Group</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Client Name" SortExpression="POSCL_ID">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownList12" runat="server" DataSourceID="sdsClientName"
                                    DataTextField="POSCL_NAME" DataValueField="POSCL_ID" SelectedValue='<%# Bind("POSCL_ID") %>'>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="DropDownList11" runat="server" DataSourceID="sdsClientName"
                                    DataTextField="POSCL_NAME" DataValueField="POSCL_ID" Enabled="False" SelectedValue='<%# Bind("POSCL_ID") %>'>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Branch" SortExpression="CMP_BRANCH_ID">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="sdsBranch" DataTextField="CMP_BRANCH_NAME"
                                    DataValueField="CMP_BRANCH_ID" SelectedValue='<%# Bind("CMP_BRANCH_ID") %>'>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="dtdGdvBranch" runat="server" DataSourceID="sdsBranch" DataTextField="CMP_BRANCH_NAME"
                                    DataValueField="CMP_BRANCH_ID" SelectedValue='<%# Bind("CMP_BRANCH_ID") %>' Enabled="False">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                    </Columns>
                    <PagerStyle CssClass="pgr" />
                    <AlternatingRowStyle CssClass="alt" />
                </asp:GridView>
            </div>
            <div style="background-color: royalblue">
                <strong><span style="color: white">New System User Group</span></strong></div>
            <asp:DetailsView ID="dtvSysUsrGroup" runat="server" Height="50px" Width="354px" AutoGenerateRows="False"
                BorderColor="#E0E0E0" DataKeyNames="SYS_USER_GRP_ID" DataSourceID="sdsSysUsrGroup"
                DefaultMode="Insert" GridLines="None" 
                oniteminserting="dtvSysUsrGroup_ItemInserting">
                <Fields>
                    <asp:BoundField DataField="SYS_USR_GRP_ID" HeaderText="SYS_USR_GRP_ID" ReadOnly="True"
                        SortExpression="SYS_USR_GRP_ID" Visible="False" />
                    <asp:BoundField DataField="SYS_USR_GRP_TITLE" HeaderStyle-HorizontalAlign="Right" HeaderText="Group Title" SortExpression="SYS_USR_GRP_TITLE">
                        <HeaderStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Group Parent" HeaderStyle-HorizontalAlign="Right" SortExpression="SYS_USR_GRP_PARENT">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("SYS_USR_GRP_PARENT") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DropDownList ID="DropDownList7" runat="server" DataSourceID="sdsGroupParent"
                                DataTextField="SYS_USR_GRP_TITLE" DataValueField="SYS_USR_GRP_ID" SelectedValue='<%# Bind("SYS_USR_GRP_PARENT") %>'
                                Style="position: relative">
                            </asp:DropDownList>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("SYS_USR_GRP_PARENT") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Group Type" HeaderStyle-HorizontalAlign="Right" SortExpression="SYS_USR_GRP_TYPE">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("SYS_USR_GRP_TYPE") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DropDownList ID="DropDownList10" runat="server" SelectedValue='<%# Bind("SYS_USR_GRP_TYPE") %>'>
                                <asp:ListItem Value="GH">Group Header</asp:ListItem>
                                <asp:ListItem Value="AG">Active Group</asp:ListItem>
                            </asp:DropDownList>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("SYS_USR_GRP_TYPE") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Branch" HeaderStyle-HorizontalAlign="Right" SortExpression="CMP_BRANCH_ID">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CMP_BRANCH_ID") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DropDownList ID="ddlDtvBranch" runat="server" DataSourceID="sdsBranch" DataTextField="CMP_BRANCH_NAME"
                                DataValueField="CMP_BRANCH_ID" SelectedValue='<%# Bind("CMP_BRANCH_ID") %>' Style="position: relative">
                            </asp:DropDownList>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("CMP_BRANCH_ID") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Client Name" HeaderStyle-HorizontalAlign="Right" SortExpression="POSCL_ID">
                        <InsertItemTemplate>
                            <asp:DropDownList ID="ddlCliName" runat="server" DataSourceID="sdsClientName" DataTextField="POSCL_NAME"
                                DataValueField="POSCL_ID" SelectedValue='<%# Bind("POSCL_ID") %>'>
                            </asp:DropDownList>
                        </InsertItemTemplate>
                        <HeaderStyle HorizontalAlign="Right" />
                    </asp:TemplateField>--%>
                    <asp:CommandField ButtonType="Button" ShowInsertButton="True" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Right" />
                    </asp:CommandField>
                </Fields>
            </asp:DetailsView>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
