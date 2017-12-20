<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditSubmittersType.aspx.vb" Inherits="Comunita_OnLine.EditSubmittersType" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/UC/UC_WizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ Register Src="~/Modules/CallForPapers/UC/UC_PrintDraft.ascx" TagPrefix="CTRL" TagName="PrintDraft" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    <script type="text/javascript" src="<%#ResolveUrl("~/Jscript/Modules/CallForPapers/Localization/localization.cfp." &  Me.PageUtility.LinguaCode  & ".js")%>"></script>
</asp:Content>
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
                                <asp:button ID="BTNsaveSubmittersTop" runat="server" Text="Save"/>
                            </div>
                             <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                 <!-- @Start Submitters -->
                                 <ul class="sections submitters">
                                    <li class="section clearfix autoOpen" id="section_1">
                                        <div class="externalleft"></div>
                                        <div class="sectioncontent">
                                            <div class="innerwrapper">
                                                <div class="internal clearfix">
            					                    <span class="left">
                                                        <asp:Label ID="LBsubmittersType" runat="server" CssClass="title">Submitters</asp:Label>
            					                    </span>
            					                    <span class="right">
            						                    <span class="icons">
                                                           <asp:Button ID="BTNaddSubmitter" runat="server" Text="Add" CssClass="icon addfield" /> 
                                                        </span>
            					                    </span>
                                                </div>
                                                <div class="sectiondetails"></div>
                                             </div>
                                            <div class="clearer">
                                            </div>
                                        <div class="DivEpButton">
                                            
                                        </div>
                                        <asp:Repeater ID="RPTsubmittersType" runat="server">
                                            <HeaderTemplate>
                                                <ul class="fields submitters">    
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <li class="cfield submitter autoOpen clearfix" id="submitter_<%#Container.DataItem.Id %>">
                                                    <asp:Literal ID="LTidSubmitter" Visible="false" runat="server" Text="<%#Container.DataItem.Id %>"></asp:Literal>
                                                    <div class="externalleft">
                                                        <asp:Label ID="LBmoveSubmitter" cssclass="movecfield" runat="server">M</asp:Label>
                                                    </div>
                                                    <div class="fieldcontent">
                                                        <div class="internal clearfix">
                                                            <span class="left">
                                                                <span class="title">
                                                                    <asp:Label ID="LBsubmitterName_t" runat="server" AssociatedControlID="TXBsubmitterName"  CssClass="fieldlabel"></asp:Label>
                                                                    <asp:TextBox ID="TXBsubmitterName" runat="server" Columns="30"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RFVsubmitterName" runat="server" SetFocusOnError="true" ControlToValidate="TXBsubmitterName" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                </span>
                                                                <span class="titledetail">
                                                                    <input type="checkbox" id="CBXmultipleSubmissions" runat="server" class="inputtext activator" />
                                                                    <asp:Label ID="LBmultipleSubmissions" runat="server" AssociatedControlID="CBXmultipleSubmissions"></asp:Label>
                                                                    <asp:Label ID="LBmaxSubmissions" runat="server" AssociatedControlID="TXBmaxSubmissions"></asp:Label>
                                                                    <asp:TextBox ID="TXBmaxSubmissions" runat="server" CssClass="inputchar" MaxLength="3"></asp:TextBox>
                                                                    <asp:RangeValidator ID="RNVmaxSubscriptions" runat="server" ControlToValidate="TXBmaxSubmissions" MinimumValue="1" MaximumValue="99999" Type="Integer" SetFocusOnError="true"></asp:RangeValidator>
                                                                </span>
                                                            </span>
                                                            <span class="right">
                                                                <span class="icons">
                                                                    <asp:Button ID="BTNremoveSubmitter" CssClass="icon needconfirm delete" CommandName="delete" runat="server" Visible="false" />
                                                                </span>
                                                            </span>
                                                        </div>
                                                        <div class="fielddetails">
                                                            <input type="hidden" id="HDNdisplayOrder" runat="server" class="hiddendisplayorder"/>
                                                            <div class="fieldobject">
                                                                <div class="fieldrow">
                                                                    <asp:Label ID="LBsubmitterDescription_t"  CssClass="fieldlabel" runat="server" AssociatedControlID="TXBsubmitterDescription"></asp:Label>
                                                                    <asp:TextBox ID="TXBsubmitterDescription" runat="server" Columns="60" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="fieldobject">
                                                                <div class="fieldrow">
                                                                    <CTRL:PrintDraft runat="server" ID="CTRL_PrintDraft" Visible="true" />
                                                                </div>
                                                            </div>                    
                                                        </div>                                    
                                                    </div>
                                                    <div class="clearer">
                                                    </div>
                                                </li>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                                </ul>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        </div>
                                    </li>
                                </ul>
                                <!-- @End Submitters -->
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:HyperLink ID="HYPpreviewCallBottom" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNsaveSubmittersBottom" runat="server" Text="Save"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>