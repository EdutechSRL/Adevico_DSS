<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EPWeightFix.aspx.vb" Inherits="Comunita_OnLine.EPWeightFix" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagPrefix="CTRL" TagName="Messages" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

<link href="../../Graphics/Modules/Edupath/css/edupath.css" rel="Stylesheet" />
<script>
    $(function () {
        $("span.selectAll input[type='checkbox']").change(function () {
            var $checkbox = $(this);
            var $parentTable = $(this).parents("table").first();
            var checked = $checkbox.is(":checked");
            $parentTable.find("span.chb input[type='checkbox']").attr("checked", checked);
        });

        $("span.chb input[type='checkbox']").change(function () {
            var $checkbox = $(this);
            
            var $parentTable = $(this).parents("table").first();
            var checked = $parentTable.find("span.chb input[type='checkbox']:checked").size();
            var total = $parentTable.find("span.chb input[type='checkbox']").size();            
            $parentTable.find("span.selectAll input[type='checkbox']").attr("checked", total == checked);
        });
    });
</script>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server"><asp:Label runat="server" ID="LBservicePathSummaryFix">**Paths Fix</asp:Label></asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView runat="server" ID="MLVsummary">
        <asp:View runat="server" ID="VIWsummary">
            <CTRL:Messages runat="server" ID="CTRLmoduleStatusMessage" Visible="false" />
            <div class="epsummary epsummary-fix">               
                <table class="table light epsummary">
                    <thead>
                        <tr>
                            <th class="community"><asp:Label runat="server" ID="LBcommunityNameHeader">**Community</asp:Label></th>
                            <th class="path"><asp:Label runat="server" ID="LBpathNameHeader">**Path</asp:Label></th>
                            <th class="expected"><asp:Label runat="server" ID="LBexpectedHeader">**Expected</asp:Label></th>
                            <th class="actual"><asp:Label runat="server" ID="LBactualHeader">**Actual</asp:Label></th>
                            <th class="assigned"><asp:Label runat="server" ID="LBassignedHeader">**Assigned</asp:Label></th>
                            <th class="selection">
                                <asp:CheckBox runat="server" ID="CHBselectAll" CssClass="selectAll" Text="**Select All" />
                            </th>
                        </tr>
                    </thead>                
                    <tbody>
                    <asp:Repeater runat="server" ID="RPTfix">
                        <ItemTemplate>
                            <tr class="fixitem">
                              <td class="community">
                                <asp:Label runat="server" ID="LBcommunityname">**community</asp:Label>
                              </td>
                              <td class="path">
                                <asp:Label runat="server" ID="LBpathname">**path</asp:Label>
                                <asp:HiddenField runat="server" ID="HIDpathId" />
                              </td>
                              <td class="expected">
                                <asp:Label runat="server" ID="LBexpected">**expected</asp:Label>
                              </td>
                              <td class="actual">
                                <asp:Label runat="server" ID="LBactual">**actual</asp:Label>
                              </td>
                              <td class="assigned">
                                <asp:Label runat="server" ID="LBassigned">**assigned</asp:Label>
                              </td>
                              <td class="selection">
                                <asp:CheckBox runat="server" ID="CHBfix" CssClass="chb" Text="" />
                              </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="TRempty" visible="false">
                        <td colspan="6"><asp:Label runat="server" ID="LBempty"></asp:Label></td>
                    </tr>
                    </tbody>                
                    <tfoot>
                    </tfoot>
                </table>
                <asp:Button runat="server" ID="BTNfixall" Text="**Fix" />
                <asp:TextBox runat="server" ID="TXBpathid" Text="58"></asp:TextBox> <asp:Button runat="server" ID="Button1" Text="Fix URL" />
            </div>
        </asp:View>
        <asp:View runat="server" ID="VIWerror">
            <div id="DVerror" align="center">                
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio">**error</asp:Label>
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWmessages" runat="server">
            <CTRL:Messages runat="server" ID="CTRLmessages" />
        </asp:View>
    </asp:MultiView>
</asp:Content>
