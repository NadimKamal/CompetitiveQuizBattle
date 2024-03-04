<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmLogin.aspx.cs" Inherits="frmLogin" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="title"></title>
    <link rel="stylesheet" type="text/css" href="resources/css/main.css" />
    <style type="text/css">
        #ext-gen9 {
            height: 206px !important;
        }

        #ext-gen68 {
            visibility: hidden;
        }

        #extimage {
            border: none;
            /*height: 35px;*/
            margin-top: -25px;
            margin-left: 120px;
        }
        /* #txtTuring {
            height: 18px;
            margin-left: -95px;
        }*/

        #ext-gen60 {
            visibility: hidden;
        }

        .x-panel-bl {
            margin-top: -20px !important;
        }
    </style>
    <script type="text/javascript">


        $("#btnLogin").click(function (e) {
            // This logic is firing when pressing "Enter" in the "Amount" input
        });


    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ScriptManager1" runat="server" />
        <asp:Label ID="lblTest" runat="server" Visible="false"></asp:Label>
        <asp:SqlDataSource ID="sqlLogin" runat="server" SelectCommand="SELECT TITLE_BFR_LOGIN FROM CM_SYSTEM_INFO"></asp:SqlDataSource>
        <ext:Panel ID="Panel4" IDMode="Ignore" runat="server" Header="false" Border="false" />
        <ext:Panel ID="Panel2" runat="server" Height="350" Header="false" HideBorders="True" BodyStyle="padding:50px 0;" Layout="Center">

            <Items>
                <ext:Panel runat="server" ID="pnlLogin"
                    Title="Login"
                    Width="380"
                    BodyStyle="padding:5px 20px;"
                    AutoHeight="true"
                    Frame="true"
                    ButtonAlign="Center"
                    DefaultButton="btnLogin"
                    Icon="Lock">
                    <Items>
                        <ext:FormLayout ID="FormLayout1" runat="server" AnchorHorizontal="true">
                            <Anchors>
                                <ext:Anchor Horizontal="100%">
                                    <ext:TextField runat="server"
                                        ID="txtUsername"
                                        AllowBlank="false"
                                        Width="250"
                                        FieldLabel="Username"
                                        BlankText="Your username is required." />
                                </ext:Anchor>

                                <ext:Anchor Horizontal="100%">
                                    <ext:TextField
                                        ID="txtPassword"
                                        runat="server"
                                        InputType="Password"
                                        AllowBlank="false"
                                        FieldLabel="Password"
                                        BlankText="Your password is required." />
                                </ext:Anchor>

                                <ext:Anchor Horizontal="61%">
                                    <ext:TextField
                                        ID="txtTuring"
                                        runat="server"
                                        InputType="Text"
                                        FieldLabel="Enter Code"
                                        AllowBlank="false"
                                        BlankText="Your Captcha is required." />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="100%">
                                    <ext:Image
                                        ID="extimage"
                                        runat="server"
                                        Width="100"
                                        BodyStyle="padding:5px 20px;"
                                        Height="25"
                                        ImageUrl="Turing.aspx">
                                    </ext:Image>
                                </ext:Anchor>
                            </Anchors>
                        </ext:FormLayout>
                    </Items>
                    <Buttons>
                        <ext:Button ID="btnLogin" runat="server" Text="Login" UseSubmitBehavior="false" Enabled="true" Icon="Accept">
                            <Listeners>
                                <Click Handler="
                                if(!#{txtUsername}.validate() || !#{txtPassword}.validate()) {
                                    Ext.Msg.alert('Error','The Username and Password fields are both required');
                                    // return false to prevent the btnLogin_Click Ajax Click event from firing.
                                    return false; 
                                }" />
                            </Listeners>
                            <DirectEvents>
                                <Click OnEvent="btnLogin_Click">
                                    <EventMask ShowMask="true" Msg="Login..." MinDelay="500" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="btnClear" runat="server" Text="Clear" Icon="Decline">
                            <DirectEvents>
                                <Click OnEvent="btnClear_Click" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="btnReset" runat="server" Visible="true" Text="Reset" Enabled="true" Icon="ApplicationEdit">
                            <DirectEvents>
                                <Click OnEvent="btnReset_Click">
                                    <EventMask ShowMask="true" Msg="Login..." MinDelay="500" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="btnPassRequest" runat="server" Text="Password Request" Icon="EmailGo">
                            <DirectEvents>
                                <Click OnEvent="btnPassRequest_Click" />
                            </DirectEvents>
                        </ext:Button>
                        
                    </Buttons>
                </ext:Panel>
            </Items>
        </ext:Panel>

        <div id="CopyRightMsg" runat="server" class="copyrightmessage"></div>

    </form>
</body>

</html>
