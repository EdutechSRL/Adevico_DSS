<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditFileToSubmit.aspx.vb" Inherits="Comunita_OnLine.EditFileToSubmit" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/UC/UC_WizardSteps.ascx" %>
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
                                <asp:HyperLink ID="HYPpreviewCallTop" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false"  Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNsaveFileToSubmitTop" runat="server" Text="Save"/>
                            </div>
                            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                <!-- @Start FILE TO SUBMIT -->
						        <div class="treetop clearfix">
							        <div class="visibilitynav left">
								        <asp:Label ID="LBrequiredFilesHideTop" cssclass="fieldsHide" runat="server">Hide details</asp:Label>
                                        <asp:Label ID="LBrequiredFileShowTop" cssclass="fieldsShow" runat="server">Show details</asp:Label>
							        </div>                                    
						        </div>
					            <ul class="sections playmode">
						            <li class="section clearfix autoOpen keepOpen" id="section_1">
					                    <div class="externalleft">
					                    </div>
					                    <div class="sectioncontent fileupload">
					                        <div class="innerwrapper">
						                        <div class="internal clearfix">
								                    <span class="left">
                                                        <asp:label ID="LBrequiredFilesDescription" runat="server" CssClass="title">Required files</asp:label>
								                    </span>
								                    <span class="right">
									                    <span class="icons">
                                                            <asp:Button ID="BTNaddRequiredFile" runat="server" Text="A" CssClass="icon addfield" />
									                    </span>
							                        </span>
						                        </div>
						                        <div class="sectiondetails">
						                        </div>	
					                        </div>
					                        <div class="clearer"></div>
					                        <ul class="fields requiredfiles">
						                        <li class="sectiondesc clearfix autoOpen" id="sectiondesc_1">
                                                    <div class="externalleft"></div>
                                                    <div class="clearer"></div>
						                        </li>
                                                <asp:Repeater ID="RPTrequiredFiles" runat="server">
                                                    <ItemTemplate>
                                                        <li class="cfield clearfix" id="requiredFile_<%#Container.DataItem.Id %>">
                                                            <asp:Literal ID="LTidRequiredFile" Visible="false" runat="server" Text="<%#Container.DataItem.Id %>"></asp:Literal>
                                                            <input type="hidden" id="HDNdisplayOrder" runat="server" class="hiddendisplayorder"/>
                                                            <div class="externalleft">
                                                                <asp:Label ID="LBmoveRequiredFile" runat="server" CssClass="movecfield"></asp:Label>
                                                            </div>
                                                            <div class="fieldcontent">
                                                                <span class="switchcfield handle">+</span>
	                                                            <div class="internal clearfix">
                                                                    <span class="left">
                                                                        <asp:Label ID="LBrequiredFileName_t" runat="server" AssociatedControlID="TXBrequiredFileName" CssClass="fieldlabel"></asp:Label>
                                                                        <asp:TextBox ID="TXBrequiredFileName" runat="server" CssClass="itemname" Text="<%#Container.DataItem.File.Name %>"></asp:TextBox>
                                                                    </span>
								                                    <span class="right">
									                                    <asp:Label ID="LBrequiredFileMandatory_t" runat="server" AssociatedControlID="CBXrequiredFileMandatory">Mandatory</asp:Label>
                                                                        <input type="checkbox" id="CBXrequiredFileMandatory" class="mandatory" runat="server" />
									                                    <span class="icons">
										                                    <asp:Button id="BTNrequiredFileVirtualDelete" runat="server" Text="D" CommandName="virtualDelete" cssclass="icon delete"/>
									                                    </span>
								                                    </span>
								                                </div>
								                                <div style="display: none;" class="fielddetails">
									                                <div class="fieldobject fileupload">
                                                                        <div class="fieldrow fielddescription">
                                                                            <asp:Label id="LBfieldDescription_t" CssClass="Titolo_campo fieldlabel" runat="server" AssociatedControlID="TXBdescription"></asp:Label>
                                                                            <asp:TextBox ID ="TXBdescription" runat="server" CssClass="textarea small" text="<%#Container.DataItem.File.Description %>"></asp:TextBox>
                                                                        </div>
                                                                        <div class="fieldrow fieldhelp" runat="server" id="DVhelp">
			                                                                <asp:Label AssociatedControlID="TXBhelp" runat="server" ID="LBfieldHelp_t" CssClass="fieldlabel">Help</asp:Label>
                                                                            <asp:TextBox runat="server" ID="TXBhelp" CssClass="inputtext" text="<%#Container.DataItem.File.Tooltip %>"></asp:TextBox>
		                                                                </div>
									                                </div>
									                                <div class="fieldfooter">
                                                                        <div class="choseselect clearfix">
                                                                            <div class="left">
                                                                                <asp:Label ID="LBrequiredFileSubmitters_t" runat="server" AssociatedControlID="SLBsubmitters" CssClass="Titolo_campo fieldlabel"></asp:Label>
                                                                                <select runat="server" id="SLBsubmitters" class="partecipants chzn-select" multiple tabindex="2">
                                                                        
                                                                                </select>
                                                                            </div>
                                                                            <div class="right">
											                                    <span class="icons">
												                                    <span class="icon selectall" title="All" runat="server" id="SPNrequiredFileSelectAll">&nbsp;</span><span class="icon selectnone" title="None" runat="server" id="SPNrequiredFileSelectNone">&nbsp;</span>
											                                    </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
								                                </div>
							                                </div>
                                                            <div class="clearer"></div>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater> 
					                        </ul>
                                        </div>
                                    </li>
                                </ul>
                                <!-- @End FILE TO SUBMIT -->
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:HyperLink ID="HYPpreviewCallBottom" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNsaveFileToSubmitBottom" runat="server" Text="Save"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>