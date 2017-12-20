<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.Master"
    CodeBehind="InternalLogin.aspx.vb" Inherits="Comunita_OnLine.InternalLogin" %>

<%@ MasterType VirtualPath="~/Authentication.Master" %>
<asp:Content ID="CNTtitle" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="CNThead" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- INTERNAL LOGIN HEADER START-->
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=TXBlogin.ClientID %>").watermark("Login", { useNative: false });
            $("#<%=TXBpassword.ClientID %>").watermark("Password", { useNative: false  });
        });
    </script>
    <!-- INTERNAL LOGIN HEADER END-->
</asp:Content>
<asp:Content ID="CNTmenu" ContentPlaceHolderID="CPHmenu" runat="server">
    <asp:Literal ID="LTretrievePassword" runat="server" Visible="false"/>
    <asp:Literal ID="LTexternalWebLogon" runat="server" Visible="false"/>
    <asp:Literal ID="LTsubscription" runat="server" Visible="false"/>
</asp:Content>
<asp:Content ID="CNTmodule" ContentPlaceHolderID="CPHcontent" runat="server">
    <div id="form" class="section">
        <asp:MultiView ID="MLVlogon" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWdefault" runat="server">
                <h2>
                    <asp:Literal ID="LTtitleInternalLogin" runat="server"></asp:Literal>
                </h2>
                <asp:Literal ID="LTinfo" runat="server"></asp:Literal>
                <asp:TextBox ID="TXBlogin" CssClass="textbox jq_watermark" runat="server"></asp:TextBox>
                <span id="password_field">
                    <asp:TextBox ID="TXBpassword" CssClass="textbox jq_watermark" runat="server" TextMode="Password" data-watermarkMaxLength="2000" maxLength="2000"></asp:TextBox>
                </span>
                <asp:Button ID="BTNlogin" runat="server" CssClass="submit" Text="Entra" />
                <div id="submit-feedback" >
                    <span class="invisible" runat="server" id="SPNmessages">
                        <asp:Literal ID="LTloginError" runat="server"></asp:Literal>
                        <br />
                        <asp:Literal ID="LTloginErrorRetrieve" runat="server"></asp:Literal>
                    </span>
                </div>
            </asp:View>
            <asp:View ID="VIWpolicy" runat="server">
            
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="CPHbottomScripts" runat="server">
   <%-- <script type="text/javascript">
        function change_pass_field_type_topass() {
            if (document.getElementById("<%=TXBpassword.ClientID %>").value == "<%=TranslatedPassword %>") {
                var pass_new_field = document.createElement("input");

                pass_new_field.name = "password";
                pass_new_field.id = "<%=TXBpassword.ClientID %>";
                pass_new_field.className = "textbox hack";

                pass_new_field.type = "password";
                pass_new_field.value = "";

                pass_new_field.onblur = function () {
                    change_pass_field_type_totext();
                }

                document.getElementById("password_field").removeChild(document.getElementById("<%=TXBpassword.ClientID %>"));

                document.getElementById("password_field").appendChild(pass_new_field);

                document.getElementById("<%=TXBpassword.ClientID %>").focus();
            }
        }

        function change_pass_field_type_totext() {
            if (document.getElementById("<%=TXBpassword.ClientID %>").value == "") {
                var pass_new_field = document.createElement("input");

                pass_new_field.name = "password";
                pass_new_field.id = "<%=TXBpassword.ClientID %>";
                pass_new_field.className = "textbox hack";

                pass_new_field.type = "text";
                pass_new_field.value = "<%=TranslatedPassword %>";

                pass_new_field.onclick = function () {
                    change_pass_field_type_topass();
                }

                document.getElementById("password_field").removeChild(document.getElementById("<%=TXBpassword.ClientID %>"));

                document.getElementById("password_field").appendChild(pass_new_field);
            }
        }
    </script>--%>
</asp:Content> 