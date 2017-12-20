<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="UsageDetails.aspx.vb" Inherits="Comunita_OnLine.UsageDetails" %>

<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../UC/UC_PagerControl.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="lm.Comol.Modules.Base.BusinessLogic" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .DIV_MP_Content .DVmenu
        {
            text-align: right;
            width: 100%;
        }
        .Row
        {
            height:20px;
        }
        .Header
        {
            height:18px;
        }
        .FieldCell
        {
            width: 18%;
            vertical-align: middle;
            text-align: center;
            }
        .DataCell
        {
            width: 40%;
            vertical-align: middle;
            text-align: center;
            } 
            
         html body .RadInput_Default .riTextBox
         {
             
             }
              
        div.Table_conteiner th.th_day span { display: block; width: 200px; }
        div.Table_conteiner th.th_access span { display: block; width: 347px; }
        div.Table_conteiner th.th_time span { display: block; width: 347px; }
        
        div.TimePicker { width: 900px; text-align: right; padding-top: 5px; margin: 0px auto; clear: both; 
                         margin-top: 0.5em; margin-bottom: 0.5em;
                         }
                         
        div.TimePicker_month { width: 880px; text-align: right; padding-top: 5px; margin: 0px auto; clear: both; 
                         margin-top: 0.5em; margin-bottom: 0.5em;
                         }
                         
                         
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="DVmenu" runat="server">
        <asp:HyperLink ID="HYPbackHistory" runat="server" CssClass="Link_Menu" Visible="false"
            Text="Back" Height="18px"></asp:HyperLink>
    </div>
    <asp:MultiView ID="MLVusageDetails" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWempty" runat="server">
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="LBmessage" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <br />
        </asp:View>
        <asp:View ID="VIWdata" runat="server">
            <div style="width: 900px; text-align: center; margin: 0,auto; padding-top: 5px; clear: both;"
                align="center">
                <telerik:RadTabStrip ID="TBSusageTime" runat="server" Align="Justify" Width="100%"
                    Height="20px" CausesValidation="false" AutoPostBack="true" Skin="Outlook" EnableEmbeddedSkins="true">
                    <Tabs>
                        <telerik:RadTab Text="Giornaliero" Value="viewType.Day" Selected="true">
                        </telerik:RadTab>
                        <telerik:RadTab Text="Settimanale" Value="viewType.Week">
                        </telerik:RadTab>
                        <telerik:RadTab Text="Mensile" Value="viewType.Month">
                        </telerik:RadTab>
                        <telerik:RadTab Text="Anno" Value="viewType.Year" Visible="false">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </div>
            <asp:MultiView runat="server" ID="MLVstatistics">
                <asp:View ID="VIWDay" runat="server">
                    <div class="TimePicker">
                        <telerik:RadDatePicker ID="RDTday" runat="server" AutoPostBack="True">
                            <DateInput runat="server" AutoPostBack="True">
                            </DateInput>
                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                        </telerik:RadDatePicker>
                        <asp:PlaceHolder runat="server" ID="PHDayStats"></asp:PlaceHolder>
                    </div>
                </asp:View>
                <asp:View ID="VIWWeek" runat="server">
                    <div class="TimePicker">
                        <script type="text/javascript">
                            // necessary to disable the weekends on client-side navigation
                            function OnDayRender(calendarInstance, args) {
                                // convert the date-triplet to a javascript date
                                // we need Date.getDay() method to determine 
                                // which days should be disabled (e.g. every Saturday (day = 6) and Sunday (day = 0))                
                                var jsDate = new Date(args.get_date()[0], args.get_date()[1] - 1, args.get_date()[2]);
                                if (jsDate.getDay() != 1) {
                                    var otherMonthCssClass = "otherMonth_" + calendarInstance.get_skin();
                                    args.get_cell().className = otherMonthCssClass;
                                    // replace the default cell content (anchor tag) with a span element 
                                    // that contains the processed calendar day number -- necessary for the calendar skinning mechanism 
                                    args.get_cell().innerHTML = "<span>" + args.get_date()[2] + "</span>";
                                    // disable selection and hover effect for the cell
                                    args.get_cell().DayId = "";
                                }
                            }
                        </script>
                        <telerik:RadDatePicker ID="RDTweek" runat="server" AutoPostBack="True">
                            <DateInput AutoPostBack="True">
                            </DateInput>
                            <Calendar OnDayRender="Calendar_OnDayRender">
                                <ClientEvents OnDayRender="OnDayRender" />
                            </Calendar>
                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                        </telerik:RadDatePicker>
                        <asp:PlaceHolder runat="server" ID="PHWeekStats"></asp:PlaceHolder>
                    </div>
                </asp:View>
                <asp:View ID="VIWMonth" runat="server">
                    <div class="TimePicker_month">
                        <asp:DropDownList ID="DDLMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLMonth_SelectedIndexChanged">
                            <asp:ListItem Value="1">Gennaio</asp:ListItem>
                            <asp:ListItem Value="2">Febbraio</asp:ListItem>
                            <asp:ListItem Value="3">Marzo</asp:ListItem>
                            <asp:ListItem Value="4">Aprile</asp:ListItem>
                            <asp:ListItem Value="5">Maggio</asp:ListItem>
                            <asp:ListItem Value="6">Giugno</asp:ListItem>
                            <asp:ListItem Value="7">Luglio</asp:ListItem>
                            <asp:ListItem Value="8">Agosto</asp:ListItem>
                            <asp:ListItem Value="9">Settembre</asp:ListItem>
                            <asp:ListItem Value="10">Ottobre</asp:ListItem>
                            <asp:ListItem Value="11">Novembre</asp:ListItem>
                            <asp:ListItem Value="12">Dicembre</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="DDLYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLYear_SelectedIndexChanged">
                            <asp:ListItem Value="2009">2009</asp:ListItem>
                            <asp:ListItem Value="2010">2010</asp:ListItem>
                            <asp:ListItem Value="2011">2011</asp:ListItem>
                        </asp:DropDownList>
                        <asp:PlaceHolder runat="server" ID="PHMonthStats"></asp:PlaceHolder>
                    </div>
                </asp:View>
            </asp:MultiView>
            <asp:Repeater ID="RPTstatistics" runat="server">
                <HeaderTemplate>
                    <div class="Table_conteiner">
                    <table width="900px" cellpadding="0" cellspacing="0" class="DataGrid_Generica" rules="all">
                        <tr class="ROW_header_Small_Center">
                            <th class="Header th_day">
                                <asp:Label ID="LBinfo_t" runat="server">I</asp:Label>
                            </th>
                            <th class="Header th_access">
                                <asp:Label ID="LBaccessNumber_t" runat="server">Numero accessi</asp:Label>
                            </th>
                            <th class="Header th_time">
                                <asp:Label ID="LBusageTime_t" runat="server">Tempo d'uso</asp:Label>
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class='<%#me.BackGroundItem(Container.ItemType) %> '>
                        <td class="FieldCell Row">
                            <%#Container.DataItem.DisplayName%>
                        </td>
                        <td class="DataCell Row">
                            <%#Container.DataItem.nAccesses%>
                        </td>
                        <td class="DataCell Row">
                           <asp:Literal ID="LTusageTime" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </asp:View>
    </asp:MultiView>
</asp:Content>