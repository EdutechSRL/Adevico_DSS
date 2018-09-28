<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="Validate.aspx.vb" Inherits="Comunita_OnLine.Validate" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagPrefix="CTRL" TagName="Messages" %>
<%@ Register TagPrefix="CTRL" TagName="ProgressBar" Src="UC/UC_ProgressBar.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/Edupath/css/<%=GetCssFileByType()%>pfstyle.css?v=201605041410lm" rel="Stylesheet" />
    <script type="text/javascript">
        var hash = new Array();
        var code = "ep-pathview";

        function cookieName(id) {
            return code + "-item-" + id;
        }

        function createCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";
            document.cookie = name + "=" + value + expires + "; path=/";
        }

        function readCookie(name) {
            var nameEQ = name + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') c = c.substring(1, c.length);
                if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
            }
            return null;
        }

        function eraseCookie(name) {
            createCookie(name, "", -1);
        }

        function WriteOpenCloseStatus(ItemId, current) {
            createCookie(cookieName(ItemId), current);
        }

        function OpenFromCookie(External, Internal, Class) {
            $(External).each(function () {
                var itemId = $(this).attr("id");
                var value = readCookie(cookieName(itemId));

                if (value != "true") {
                    //$(this).removeClass(Class).children(Internal).show();
                    $(this).find("span.switch").removeClass("collapsed");
                    $(this).find("ul.activities").toggleClass("hidden");
                    //$(this).parents("li.unit").children("ul.rules").toggleClass("hidden");

                }
            });
        }

        function OpenClose(el, External, Internal, Class) {
            var itemParent = el.parents(External);
            var itemParentId = itemParent.attr("id");
            var itemChildren = itemParent.children(Internal);

            var currentClosed = itemParent.find("span.switch").hasClass(Class);


            WriteOpenCloseStatus(itemParentId, currentClosed);

        }

        
        $(document).ready(function () {

            function Change(el) {
                el.prevAll("input[type=radio]").attr("checked", true);
                el.prevAll("input[type=radio]").change();

            }

        
        function showDialog(id) {
            InitializeAll();
            $('#' + id).dialog("open");
        }

        function closeDialog(id) {
            InitializeAll();
            $('#' + id).dialog("close");
        }

        function showDialogSender(id, sender) {
            InitializeAll();

            SetupSliders();

            var values = sender.split(";");

            var unit = values[0];
            var activity = values[1];

            //alert(unit + " ; " + activity);

            if (unit == null && activity != null) {
                unit = activity;
                activity = null;

                //unit rule

            } else {
                //activity rule

                var value = $("#ACT-" + activity).html();

                $(".Act1").val(value);

                $(".Act2-lbl").html($(".Act2").children("option:selected").text());
                $(".Act1-lbl").html($(".Act1").val());
                $("input[id$='HIDact2']").val($(".Act2").first().val().replace("ACT-", ""));
                $("input[id$='HIDact1']").val(activity);

            }

            //            alert($("input[id$='HIDact1']").val());
            //            alert($("input[id$='HIDact2']").val());


            //$("li.unit[id$='-" + unit + "']").children("ul.activities").removeClass("hidden");
            //$("li.unit[id$='-" + unit + "']").find("span.switch").removeClass("collapsed");


            if ($.browser.msie) {
                $('#' + id).dialog("option", "height", 820);
            }
            $('#' + id).dialog("open");


        }

        function closeDialogSender(id, sender) {
            $('#' + id).dialog("close");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:UpdatePanel ID="UDPpath" UpdateMode="Conditional" ChildrenAsTriggers="true"
        runat="server">
        <ContentTemplate>
            <asp:MultiView ID="MLVeduPathView" runat="server" ActiveViewIndex="0">
                <asp:View ID="VIWeduPathView" runat="server">
                    <div class="DivEpButton">
                        <asp:LinkButton runat="server" ID="LKBeduPathView" CssClass="Link_Menu"></asp:LinkButton>
                        <asp:HyperLink runat="server" ID="HYPswitchMode" CssClass="Link_Menu" Visible="false"/>
                        <asp:LinkButton runat="server" ID="LKBsave"  CssClass="Link_Menu"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="LKBreset"  CssClass="Link_Menu"></asp:LinkButton>
                    </div>
                   
                  
                    <div id="DIVprogressBar" runat="server" class="summaryBlock">
                        <CTRL:ProgressBar ID="CTRLprogressBar" runat="server"></CTRL:ProgressBar>
                    </div>
                    <CTRL:Messages runat="server" ID="CTRLmoduleStatusMessage" Visible="false" />
                    <div align="center">
                        <asp:Label ID="LBErrorMSG" runat="server" CssClass="errore"></asp:Label>
                    </div>
                    <span class="titleFuture">
                        <asp:Label ID="LBunitList" runat="server" Text="**UnitList" Visible="true"></asp:Label></span>
                    <asp:Repeater ID="RPunit" runat="server">
                        <HeaderTemplate>
                            <ul class="units unmovable">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li id="UNIT-<%# DataBinder.Eval(Container.DataItem,"Id")%>" class="unit"><span class="buttons">
                                <span class="button switch">S</span><asp:Literal runat="server" ID="LITmoveUnit"></asp:Literal>
                                <asp:Image ID="IMGmandatory" runat="server" Visible="true" />
                                <asp:LinkButton ID="LKBmandatory" runat="server" Visible="true" CommandName="mandatory"></asp:LinkButton>
                            </span>
                                <div class="title" runat="server" id="DIVunitName">
                                    <span class="leftSide">
                                        <asp:Label ID="LBunitNumber" runat="server" Visible="true" />
                                        <asp:Label ID="LBunit" runat="server" Visible="true" CssClass="Titolo_campoSmall"></asp:Label>
                                        <asp:Label ID="LBunitWeight" runat="server" Visible="false" />
                                        <asp:Label ID="LBdescription" runat="server" Visible="true" />
                                    </span><span class="buttons rightSide leftBordered">
                                        <asp:Image ID="IMGvisibility" runat="server" Visible="true" />
                                        <asp:Image ID="IMGstatus" runat="server" Visible="false" />
                                    </span><span class="expireDate rightSide leftBordered">&nbsp; </span><span class="rightSide leftBordered">
                                        <asp:Label runat="server" ID="LBmark" Visible="false"></asp:Label>
                                        <asp:TextBox runat="server" ID="TXBmark" Visible="false" Width="30px" MaxLength="3"
                                            TextMode="SingleLine"></asp:TextBox>
                                        <asp:RangeValidator ID="RNVmark" runat="server" ErrorMessage="*" Text="" ControlToValidate="TXBmark"
                                            MinimumValue="0" MaximumValue="100" Type="Integer"></asp:RangeValidator>
                                        <asp:CompareValidator ID="COVmark" runat="server" ErrorMessage="*" Text="" ControlToValidate="TXBmark"
                                            Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                        <asp:Label runat="server" ID="LBcompletion" Visible="false">
                                        </asp:Label>
                                        <asp:TextBox runat="server" ID="TXBCompletion" Width="30px" MaxLength="3" TextMode="SingleLine"
                                            Visible="false"></asp:TextBox>
                                        <asp:RangeValidator ID="RNVcompletion" runat="server" ErrorMessage="*" Text="" ControlToValidate="TXBCompletion"
                                            MinimumValue="0" MaximumValue="100" Type="Integer"></asp:RangeValidator>
                                        <asp:CompareValidator ID="COVcompletion" runat="server" ErrorMessage="*" Text=""
                                            ControlToValidate="TXBCompletion" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                    </span>
                                    <div id="DIVunitDescription" class="renderedtext" runat="server" visible="true">
                                        <asp:Label ID="LBunitDesc" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <asp:Repeater ID="RPactivity" runat="server">
                                    <HeaderTemplate>
                                        <ul class="activities unmovable">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li id="ACT-<%# DataBinder.Eval(Container.DataItem,"Id")%>" class="<%#Me.GetActivityCssClass(DataBinder.Eval(Container.DataItem,"Status")) %>">
                                            <span class="buttons">
                                                <asp:Image ID="IMGmandatory" runat="server" Visible="true" />
                                            </span>
                                            <div class="title" runat="server" id="DIVactName">
                                                <span class="leftSide">
                                                    <asp:Label ID="LBactNumber" runat="server" />
                                                    <asp:Label ID="LBactName" runat="server" CssClass="Titolo_campoSmall" Text="name"
                                                        Visible="true" />
                                                    <asp:Label ID="LBactWeight" runat="server" Visible="true" />
                                                </span><span class="buttons rightSide leftBordered">
                                                    <asp:Image ID="IMGvisibility" runat="server" Visible="true" />
                                                    <asp:Image ID="IMGstatus" runat="server" />
                                                </span><span class="expireDate rightSide leftBordered">
                                                    <asp:Label ID="LBdate" runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
                                                </span><span class="rightSide leftBordered">
                                                    <asp:Label runat="server" ID="LBmark" Text="*Mark: " Visible="false"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TXBmark" Width="30px" MaxLength="3" TextMode="SingleLine"
                                                        Visible="false"></asp:TextBox>
                                                    <asp:RangeValidator ID="RNVmark" runat="server" ErrorMessage="*" Text="" ControlToValidate="TXBmark"
                                                        MinimumValue="0" MaximumValue="100" Type="Integer"></asp:RangeValidator>
                                                    <asp:CompareValidator ID="COVmark" runat="server" ErrorMessage="*" Text="" ControlToValidate="TXBmark"
                                                        Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                                    
                                                    <asp:Label runat="server" ID="LBcompletion" Text="*Completion: "></asp:Label>
                                                    <asp:TextBox runat="server" ID="TXBCompletion" Width="30px" MaxLength="3" TextMode="SingleLine"></asp:TextBox>
                                                    <asp:RangeValidator ID="RNVcompletion" runat="server" ErrorMessage="*" Text="" ControlToValidate="TXBCompletion"
                                                        MinimumValue="0" MaximumValue="100" Type="Integer"></asp:RangeValidator>
                                                    <asp:CompareValidator ID="COVcompletion" runat="server" ErrorMessage="*" Text=""
                                                        ControlToValidate="TXBCompletion" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                                </span>
                                            </div>
                                            <ul id="ULactRules" runat="server" class=" rules ">
                                                <asp:Repeater ID="RPrule" runat="server">
                                                    <ItemTemplate>
                                                        <li class="rule">
                                                            <asp:Label ID="LBrule" runat="server" CssClass="Titolo_campoSmall" Text="rule***"></asp:Label>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <ul id="ULunitRules" runat="server" class=" rules ">
                                    <asp:Repeater ID="RPunitrule" runat="server">
                                        <ItemTemplate>
                                            <li class="rule">
                                                <asp:Label ID="LBunitrule" runat="server" CssClass="Titolo_campoSmall" Text="rule***"></asp:Label>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </li>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:View>
                <asp:View ID="VIWerror" runat="server">
                    <div id="DVerror" align="center">
                        <div class="DivEpButton">
                            <asp:HyperLink ID="HYPerror" runat="server" CssClass="Link_Menu" />
                        </div>
                        <div align="center">
                            <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="VIWmessages" runat="server">
                    <CTRL:Messages runat="server" ID="CTRLmessages"/>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="dialog_target">
    </div>
</asp:Content>
