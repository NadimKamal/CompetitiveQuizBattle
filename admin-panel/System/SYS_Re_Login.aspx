<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SYS_Re_Login.aspx.cs" Inherits="System_SYS_Re_Login" %>

<%--<%@ Register assembly="Coolite.asp.Web" namespace="Coolite.asp.Web" tagprefix="asp" %>--%>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>FiS Login</title>
    <link rel="stylesheet" type="tasp/css" href="resources/css/main.css" />
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ScriptManager1" runat="server" />
        <ext:Panel ID="Panel2" runat="server" Height="250" Header="false" HideBorders="True" BodyStyle="padding:50px 0;">
        <Items>            
            <ext:CenterLayout ID="CenterLayout1" runat="server">  
                <Items>
            <ext:Panel 
                ID="pnlLogin" 
                runat="server" 
                Title="Login"
                BodyStyle="padding:5px 20px;"
                Width="350"
                AutoHeight="true"
                Frame="true"
                ButtonAlign="Center"
                Icon="Lock" >
                <Items>
                    <ext:FormLayout ID="FormLayout1" runat="server"> 
                                
                      <%--  <Items>--%>
                            
                        <Anchors>                            
                            <ext:Anchor Horizontal="100%">
                                <ext:TextField 
                                    ID="txtUsername" 
                                    runat="server" 
                                    FieldLabel="Username" 
                                    AllowBlank="false"
                                    BlankTasp="Your username is required."                                    
                                    />
                            </ext:Anchor>
                            <ext:Anchor Horizontal="100%">
                                <ext:TextField 
                                    ID="txtPassword" 
                                    runat="server" 
                                    InputType="Password" 
                                    FieldLabel="Password" 
                                    AllowBlank="false" 
                                    BlankTasp="Your password is required."                                    
                                    />
                            </ext:Anchor>
                        </Anchors>
                            <%--</Items>  --%>             
                    </ext:FormLayout>
                </Items>
                <Buttons>
                    <ext:Button ID="btnLogin" Width="70px" Text="Login" runat="server" Tasp="Login" Icon="Accept">
                        <Listeners>
                            <Click Handler="
                                if(!#{txtUsername}.validate() || !#{txtPassword}.validate()) {
                                    asp.Msg.alert('Error','The Username and Password fields are both required');
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
                    <ext:Button ID="btnClear" Width="70px" Text="Clear" runat="server" Tasp="Clear" Icon="Decline">
                        <DirectEvents>
                            <Click OnEvent="btnClear_Click" />
                        </DirectEvents>
                    </ext:Button>
                </Buttons>
            </ext:Panel>
                    </Items>          
            </ext:CenterLayout>
        </Items>
    </ext:Panel>   
    </form>
</body>
</html>
