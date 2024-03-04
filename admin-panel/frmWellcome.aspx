<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmWellcome.aspx.cs" Inherits="frmWellcome" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="Ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="resources/css/main.css" />

    <script type="text/javascript" src="resources/ExampleTab.js"></script>

    <script type="text/javascript">
        var loadExample = function (href, id, ptitle) {
            var tab = ExampleTabs.getComponent(id);

            if (tab) {
                ExampleTabs.setActiveTab(tab);
            } else {
                createExampleTab(id, href, ptitle);
            }
        }

        var selectionChaged = function (dv, nodes) {
            if (nodes.length > 0) {
                var url = nodes[0].getAttribute("ext:url"),

                    id = nodes[0].getAttribute("ext:id"),
                    ptitle = nodes[0].getAttribute("ext:text");

                loadExample(url, id, ptitle);
            }
        }

        var viewClick = function (dv, e) {
            var group = e.getTarget("h2", 3, true);

            if (group) {
                group.up("div").toggleClass("collapsed");
            }
        }
    </script>
</head>
<body>
    <Ext:ResourceManager ID="ScriptManager1" runat="server" DirectMethodNamespace="MIT" />
    <Ext:Viewport runat="server" Layout="Border">
        <Items>
            <Ext:BorderLayout runat="server">
                <North Margins-Bottom="0" Collapsible="true">
                    <Ext:Panel runat="server" Title="North" ID="Panel1" IDMode="Ignore" Header="false"></Ext:Panel>
                </North>
                <West MinWidth="225" MaxWidth="400" Split="true" Collapsible="true">
                    <Ext:Panel ID="WestPanel" runat="server" Title="CAAT Menu" Width="225" Icon="BookOpen" >
                        <Items>
                            <Ext:FitLayout ID="FitLayout3" runat="server">
                                <Items>
                                    <Ext:TreePanel ID="exampleTree" runat="server" Title="Caat Menu" AutoScroll="true" Lines="false"
                                        Header="false" CollapseFirst="false" ContainerScroll="true" RootVisible="false" Icon="BookOpen">
                                        <TopBar>
                                            <Ext:Toolbar ID="Toolbar1" runat="server">
                                                <Items>
                                                    <Ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="Theme: " />
                                                    <Ext:ComboBox ID="cbTheme" runat="server" EmptyText="Choose Theme" Width="75" Editable="false"
                                                        TypeAhead="true" AutoScroll="true">

                                                        <Template Visible="False" ID="ctl1948" EnableViewState="False" runat="server"></Template>
                                                        <Items>
                                                            <Ext:ListItem Text="Default" Value="Default" />
                                                            <Ext:ListItem Text="Gray" Value="Gray" />
                                                            <Ext:ListItem Text="Slate" Value="Slate" />
                                                        </Items>
                                                        <Listeners>
                                                            <Select Handler="MIT.GetThemeUrl(cbTheme.getValue(),{
                                                                success: function (result) {
                                                                    Ext.Net.setTheme(result);
                                                                    ExampleTabs.items.each(function (el) {
                                                                        if (!Ext.isEmpty(el.iframe)) {
                                                                            el.iframe.dom.contentWindow.Ext.Net.setTheme(result);
                                                                        }
                                                                    });
                                                                }
                                                            });" />
                                                        </Listeners>
                                                    </Ext:ComboBox>
                                                    <Ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                                    <Ext:Toolbar runat="server" ID="ctl1950" AutoScroll="true">
                                                        <Items>
                                                            <Ext:Button ID="tbbLogin" runat="server" Icon="LockEdit">
                                                                <DirectEvents>
                                                                    <Click OnEvent="tbbLogin_Click" />
                                                                </DirectEvents>
                                                                <ToolTips>
                                                                    <Ext:ToolTip ID="ToolTip3" IDMode="Ignore" runat="server" Html="Log Out">
                                                                    </Ext:ToolTip>
                                                                </ToolTips>
                                                            </Ext:Button>
                                                            <Ext:Button ID="ToolbarButton1" runat="server" IconCls="icon-expand-all">
                                                                <Listeners>
                                                                    <Click Handler="#{exampleTree}.root.expand(true);" />
                                                                </Listeners>
                                                                <ToolTips>
                                                                    <Ext:ToolTip ID="ToolTip1" IDMode="Ignore" runat="server" Html="Expand All" />
                                                                </ToolTips>
                                                            </Ext:Button>
                                                            <Ext:Button ID="ToolbarButton2" runat="server" IconCls="icon-collapse-all">
                                                                <Listeners>
                                                                    <Click Handler="#{exampleTree}.root.collapse(true);" />
                                                                </Listeners>
                                                                <ToolTips>
                                                                    <Ext:ToolTip ID="ToolTip2" IDMode="Ignore" runat="server" Html="Collapse All" />
                                                                </ToolTips>
                                                            </Ext:Button>
                                                            <Ext:Button ID="tbbRefreshMenu" runat="server" Icon="ArrowRefresh">
                                                                <Listeners>
                                                                    <Click Handler="#{exampleTree}.root.reload(true);" />
                                                                </Listeners>
                                                                <ToolTips>
                                                                    <Ext:ToolTip ID="ToolTip4" IDMode="Ignore" runat="server" Html="Refresh Menu" />
                                                                </ToolTips>
                                                            </Ext:Button>
                                                        </Items>
                                                    </Ext:Toolbar>
                                                </Items>
                                            </Ext:Toolbar>
                                        </TopBar>
                                        <Root>
                                            <Ext:AsyncTreeNode Text="Examples" NodeID="root" Expanded="true" />
                                        </Root>
                                        <BottomBar>
                                            <Ext:StatusBar ID="StatusBar1" runat="server" AutoClear="1500" />
                                        </BottomBar>
                                        <Loader>
                                            <Ext:PageTreeLoader RequestMethod="GET" OnNodeLoad="GetTreeMenuNodes" PreloadChildren="true">
                                                <EventMask ShowMask="true" Target="Parent" Msg="Loading..." />
                                            </Ext:PageTreeLoader>
                                        </Loader>
                                        <Listeners>
                                            <Click Handler="if(node.isLeaf()){e.stopEvent();loadExample(node.attributes.href, node.id,node.attributes.text);}" />
                                        </Listeners>
                                    </Ext:TreePanel>
                                </Items>
                            </Ext:FitLayout>
                        </Items>
                    </Ext:Panel>
                </West>
                <Center>
                    <Ext:TabPanel ID="ExampleTabs" runat="server" ActiveTabIndex="0" EnableTabScroll="true">
                        <Items>
                            <Ext:Panel ID="tabHome" runat="server" Icon="Application" Title="Dash Board" 
                                AutoScroll="true">
                                <Items>
                                    <Ext:FitLayout ID="FitLayout1" runat="server">
                                        <Items>
                                            <Ext:Panel ID="ImagePanel" runat="server" Cls="images-view" AutoHeight="true" Border="false">
                                                <Items>
                                                    <Ext:FitLayout ID="FitLayout2" runat="server">
                                                        <Items>
                                                           
                                                            <Ext:Panel ID="Panel4" runat="server" Height="550" Width="350" >
                                                                <AutoLoad Url="frmWellcomeMessage.aspx" Mode="IFrame" ShowMask="true" />
                                                            </Ext:Panel>
                                                        </Items>
                                                    </Ext:FitLayout>
                                                </Items>
                                            </Ext:Panel>
                                        </Items>
                                    </Ext:FitLayout>
                                </Items>
                            </Ext:Panel>
                        </Items>
                        <Plugins>
                            <Ext:TabCloseMenu ID="TabCloseMenu1" runat="server" />
                        </Plugins>
                    </Ext:TabPanel>
                </Center>
            </Ext:BorderLayout>
        </Items>
    </Ext:Viewport>
</body>
</html>
