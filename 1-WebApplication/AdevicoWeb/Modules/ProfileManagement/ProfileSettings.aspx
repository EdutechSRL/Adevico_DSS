<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ProfileSettings.aspx.vb" Inherits="Comunita_OnLine.ProfileSettings" %>
<%@ Register TagPrefix="CTRL" TagName="ProfileData" Src="./UC/UC_ProfileData.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AvatarEditor" Src="./UC/UC_ProfileAvatarEditor.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="MailEditor" Src="./UC/UC_ProfileMailEditor.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="MailPolicy" Src="./UC/UC_ProfileMailPolicy.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProfileManagement/css/ProfileManagement.css?v=201602221000lm" rel="Stylesheet" type="text/css" />
      <script language="Javascript" type="text/javascript">
        function showDialog(id) {
//            if ($.browser.msie && id =='criterionManagement') {
//                $('#' + id).dialog("option", "height", 870);
//            }
//            else if ($.browser.msie && id =='addRequestedFile') {
//                $('#' + id).dialog("option", "height", 350);
//            }
            $('#' + id).dialog("open");
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }
        
        $(document).ready(function () {
            $(".view-modal").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                title: "<%#Me.TranslateModalView("MailEditor") %>",
                width: 700,
                height: 350,
                minHeight: 250,
                minWidth: 650,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });

            $(".view-modal-avatar").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                title: "<%#Me.TranslateModalView("AvatarEditor") %>",
                width: 700,
                height: 250,
                minHeight: 200,
                minWidth: 650,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
        });    
        function onUpdating() {
            $.blockUI({ message: '<h1><%#Me.OnLoadingTranslation %></h1>' });
            return true;
        }   
    </script>

    <script language="javascript" type="text/javascript">
        function AfterCheckHandler(sender, eventArgs) {
            var tree = $find("<%=TreeViewClientID %>");
            var nodes = tree.get_allNodes();
            var check = false;
            var node = eventArgs.get_node();
            if (node.get_checked()) {
                check = true;
            }
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].get_checked() != check && nodes[i] != node && nodes[i].get_value() == node.get_value()) {
                    (check) ? nodes[i].check() : nodes[i].uncheck();
                }
            }
            //tree.UpdateState();
        }
    </script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $(".AgencyText").autocomplete({
                autoFocus: true,
                appendTo: "form",
                source: function (request, response) {
                    var dataparameter = $(this.element).siblings(".AgencyValue").attr("data-parameters");
                    $.ajax({
                        url: "../Common/AutoComplete.asmx/AgencyNamesByUser",
                        data: "{ 'filter': '" + request.term + "'," + dataparameter + "}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.Name,
                                    id: item.Id
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    $(this).siblings(".AgencyValue").val(ui.item.id);
                },
                change: function (event, ui) {
                    if (!$(this).hasClass("permitNew") && ui.item == null) {
                        $(this).siblings(".AgencyValue").val("");
                        $(this).val("");
                    }
                },
                close: function (event, ui) {

                }
            }).keydown(function () {
                $(this).siblings(".AgencyValue").val("");
            });
        });

        $(function(){
            $(".toggler").each(function(){
                var $toggler = $(this);
                var isOn = $toggler.is(".on");
                var $item = $($toggler.data("item"));
                $item.addClass("toggled");
                if(isOn)
                {
                    $item.addClass("on").removeClass("off");
                }else
                {
                    $item.addClass("off").removeClass("on");
                }
            });
        
            $(".toggler .toggle").click(function(){
                var $toggle = $(this);
                var $toggler = $(this).parents(".toggler").first();
                var isOn = $toggle.is(".on");
                var $item = $($toggler.data("item"));
                if(isOn)
                {
                    $toggler.addClass("on").removeClass("off");
                    $item.addClass("on").removeClass("off");
                }else
                {
                    $toggler.addClass("off").removeClass("on");
                    $item.addClass("off").removeClass("on");
                }
            });
        });


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
     <asp:MultiView ID="MLVprofile" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWdefault" runat="server">
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="LBmessages" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <br />
            <br />
        </asp:View>
        <asp:View ID="VIWedit" runat="server">
            <div id="DVmenu" class="DVmenu" runat="server">
                <asp:HyperLink ID="HYPeditPassword" runat="server" CssClass="Link_Menu" Text="Edit password"
                    Height="18px" CausesValidation="false" Visible="false"></asp:HyperLink>
            </div>
            <CTRL:messages ID="CTRLmessages" runat="server" Visible="true"/>
                    <div class="TopInfo">
                        <span class="ImageInfo">
                            <asp:Image ID="IMGavatar" runat="server" Visible="False" ToolTip="Immagine Personale"
                                Height="125px" Width="100px"></asp:Image>
                            <asp:Button ID="BTNedit" runat="server" Text="Edit" CausesValidation="false" />
                        </span>
                        <span class="TextInfo">
                            <span class="Field_Row">
                                <asp:Label ID="LBdisplayName_t" runat="server" CssClass="Titolo_Campo">User</asp:Label>
                                <asp:Label ID="LBdisplayName" runat="server" CssClass="Testo_Campo"></asp:Label>
                            </span>
                            <span class="Field_Row">
                                <asp:Label ID="LBprofileType_t" runat="server" CssClass="Titolo_Campo"></asp:Label>
                                <asp:Label ID="LBprofileType" runat="server" CssClass="Testo_Campo"></asp:Label>
                            </span>
                        </span>
                        <span class="SaveTop">
                            <asp:Button ID="BTNsaveTop" runat="server" Text="Save" />
                        </span>
                    </div>
                    <div>
                        <telerik:RadTabStrip ID="TBSprofile" runat="server"  Align="Justify"
                            CausesValidation="false" AutoPostBack="false" Skin="Outlook" EnableEmbeddedSkins="true" CssClass="InfoTab">
                            <Tabs>
                                <telerik:RadTab Text="Profile Info" Value="profileData" Visible="false">
                                </telerik:RadTab>
                                <telerik:RadTab Text="Mail privacy Info" Value="mailPolicy" Visible="false">
                                </telerik:RadTab>
                                <telerik:RadTab Text="Advanced Settings" Value="advancedSettings" Visible="false">
                                </telerik:RadTab>
<%--                                <telerik:RadTab Text="Istant Messaging Settings" Value="istantMessaging" Visible="false">
                                </telerik:RadTab>--%>
                                
                            </Tabs>
                        </telerik:RadTabStrip>
                    </div>
            <div>
                <asp:MultiView ID="MLVuserInfo" runat="server" ActiveViewIndex="0">
                    <asp:View ID="VIWempty" runat="server">
                    </asp:View>
                    <asp:View ID="VIWprofileInfo" runat="server">
                        <br />
                        <CTRL:ProfileData id="CTRLprofileData" runat="server" ContainerMailEdit="True"></CTRL:ProfileData>
                    </asp:View>
                    <asp:View ID="VIWmailPolicy" runat="server">
                        <br />
                        <CTRL:MailPolicy ID="CTRLmailPolicy" runat="server" ClientScript="return onUpdating();"/>
                    </asp:View>
                    <asp:View ID="VIWadvancedSettings" runat="server">
      
                    </asp:View>
                   
                </asp:MultiView>
            </div>
            <div class="SaveBottom">
                <asp:Button ID="BTNsaveBottom" runat="server" Text="Save" />
            </div>
        </asp:View>
    </asp:MultiView>
    <div id="DVavatarEditor" runat="server" Visible="false" class="view-modal-avatar">
        <CTRL:AvatarEditor ID="CTRLavatar" runat="server" />  
    </div>
    <div id="DVmailEditor" runat="server" Visible="false" class="view-modal">
        <CTRL:MailEditor id="CTRLmailEditor" runat="server"  />
    </div>
</asp:Content>