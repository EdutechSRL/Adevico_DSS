<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditFieldAssociation.aspx.vb" Inherits="Comunita_OnLine.EditFieldAssociation" %>
<%@ Register Assembly="lm.Comol.Core.BaseModules" Namespace="lm.Comol.Core.BaseModules.Web.Controls" TagPrefix="asp" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers//UC/UC_WizardSteps.ascx" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailEditor.ascx" TagName="CTRLtemplateMail" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
    <link href="../../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
     <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/CallForPapers/callforpapers.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView id="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBnocalls" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWsettings" runat="server">
            <div class="contentwrapper edit clearfix persist-area">
                <div class="column left persist-header copyThis">
                    <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
                </div>
                <div class="column right resizeThis">
                    <div class="rightcontent">
                        <div class="header">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackTop" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:HyperLink ID="HYPpreviewCallTop" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNsaveFieldAssociationsTop" runat="server" Text="Save"/>
                            </div>
                            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                <!-- @Start FIELD ASSICIATIONS TO SUBMIT -->
                                <div class="tree">
                                    <div class="treetop clearfix">
                                        <div class="visibilitynav left">
                                            <asp:Label ID="LBcollapseAllTop" cssclass="collapseAll" runat="server">Collapse</asp:Label>
                                            <asp:Label ID="LBexpandAllTop" cssclass="expandAll" runat="server">Expand</asp:Label>
                                        </div>
                                    </div>
                                    <a name="#section_0"></a>
                                    <asp:Repeater ID="RPTsections" runat="server">
                                        <HeaderTemplate>
                                            <ul class="sections playmode">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li class="section clearfix autoOpen" id="section_<%#Container.DataItem.Id %>">
                                                <div class="externalleft"></div>
                                                <div class="sectioncontent">
                                                    <span class="switchsection handle">+</span>
                                                    <div class="innerwrapper">
                                                        <div class="internal clearfix">
                                                            <span class="left">
                                                                <a name="#section_<%#Container.DataItem.Id %>"></a>
                                                                <asp:Literal ID="LTidSection" runat="server" Visible="false"></asp:Literal>
                                                                <asp:Label ID="LBsectionName" cssclass="title" runat="server">Section</asp:Label>
                                                            </span>
                                                            <span class="right">
                                                                <span class="icons"></span>
                                                            </span>
                                                        </div>
                                                        <div class="sectiondetails"></div>
                                                    </div>
                                                    <div class="clearer"></div>
                                                    <ul class="fields">
                                                         <li class="sectiondesc clearfix autoOpen" id="sectiondesc_<%#Container.DataItem.Id %>"></li>
                                                         <li class="cfield clearfix autoOpen" id="field_1">
                                                            <div class="externalleft"></div>
                                                            <div class="fieldcontent">
                                                                <asp:Repeater ID="RPTfields" runat="server" DataSource="<%#Container.DataItem.Fields%>" OnItemDataBound="RPTfields_ItemDataBound">
                                                                    <HeaderTemplate>
                                                                        <table class="matchingtable minimal">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th class="field"><asp:literal runat="server" ID="LTfieldName_t">*Nome campo</asp:literal></th>
                                                                                    <th class="match"><asp:literal runat="server" ID="LTmatchProfileItem_t">*Corrispondenza campo</asp:literal></th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                                <asp:Literal ID="LTidAssociation" runat="server" Visible="false"></asp:Literal>
                                                                                <asp:Literal ID="LTidField" runat="server" Visible="false"></asp:Literal>
                                                                                <tr id="TRfieldRow" runat="server">
                                                                                    <td class="field"><asp:Literal ID="LTfieldName" runat="server"></asp:Literal></td>

                                                                                    <td class="match">
                                                                                        <asp:ExtendedDropDown ID="DDLprofileFields" runat="server"></asp:ExtendedDropDown>
                                                                                    </td>
                                                                                </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                            </tbody>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </div>
                                                        </li>
                                                    </ul>
                                                </div>
                                                <div class="clearer"></div>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                                <!-- @End FIELD ASSICIATIONS TO SUBMIT -->
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:HyperLink ID="HYPpreviewCallBottom" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNsaveFieldAssociationsBottom" runat="server" Text="Save"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>