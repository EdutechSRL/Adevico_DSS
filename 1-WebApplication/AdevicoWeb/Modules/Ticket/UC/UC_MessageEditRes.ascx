<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MessageEditRes.ascx.vb" Inherits="Comunita_OnLine.UC_MessageEditRes" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="CTRL" TagName="Editor" Src="~/Modules/Common/Editor/UC_Editor.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsCommands" Src="~/Modules/Repository/Common/UC_ModuleAttachmentInlineCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsView" Src="~/Modules/Ticket/UC/UC_AttachmentsView.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
	<%--<asp:Literal ID="LTmessageHeaderCSS" runat="server" Visible="false">messageheader clearfix empty</asp:Literal>--%>
	<div class="left column">
		<span class="username"><asp:Literal ID="LTuserName" runat="server">#User Name</asp:Literal></span>
		<span class="userrole"><asp:Literal ID="LTuserRole" runat="server">#Role</asp:Literal></span>
	</div>
	
	<div class="right column">
		<div class="messagewrapper">

			<%--<div class="messageheader clearfix empty" runat="server" id="DVmessageHeader">
				
				<%--<div class="left"></div>
				<div class="right"></div>
			</div>--%>
							
			<div class="messagecontent clearfix">
				<asp:Label ID="LBucMessage_t" runat="server" CssClass="fieldlabel title">*Your messagge:</asp:Label><asp:Literal ID="LTanchor" runat="server"><a id="editor"></a></asp:Literal>
				<%--<asp:TextBox ID="TXBucMessage" runat="server" CssClass="fieldeditor" TextMode="MultiLine"></asp:TextBox>--%>
				<CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false" />
				<CTRL:Editor ID="CTRLEditorText" runat="server" 
					ContainerCssClass="containerclass"
					LoaderCssClass="loadercssclass fieldinput inputtext" EditorCssClass="editorcssclass"             
					AllAvailableFontnames="false" AutoInitialize="true"  MaxHtmlLength="800000"
					EditorWidth="700px" />				 
				
				<!-- + classe empty se vuoto -->
				<asp:Literal ID="LTattachmentDiv" runat="server" Visible="true">
				<div class="fieldrow attachments empty">
				</asp:Literal>
					<CTRL:AttachmentsView ID="CTRLattView" runat="server" />
					<%--<span class="renderedfile">
												
						<span class="objectRender file">
							<span class="leftDetail">
								<span class="itemTitle">
									<a title="(Premi per scaricare) Lorem_Ipsum.pdf (31 kb)." class="fileRepositoryCookie" href="" target="_blank"><span class="fileIco extpdf">&nbsp;</span>Dolor_sit_amet.pdf</a>
								</span>
								<span class="itemDetails">31 kb</span>
							</span>
						</span>
												
						<span class="icons visibility">
							<span class="icon hide"></span>
							<span class="icon show"></span>
						</span>
											
					</span>
																		
					<span class="renderedfile hidden">
												
						<span class="objectRender file">
							<span class="leftDetail">
								<span class="itemTitle">
									<a title="(Premi per scaricare) Lorem_Ipsum.pdf (31 kb)." class="fileRepositoryCookie" href="" target="_blank"><span class="fileIco extpdf">&nbsp;</span>Dolor_sit_amet.pdf</a>
								</span>
								<span class="itemDetails">31 kb</span>
							</span>
						</span>
												
						<span class="icons visibility">
							<span class="icon hide"></span>
							<span class="icon show"></span>
						</span>
											
					</span>
											
					<span class="renderedfile">
												
						<span class="objectRender file">
							<span class="leftDetail">
								<span class="itemTitle">
									<a title="(Premi per scaricare) Lorem_Ipsum.pdf (31 kb)." class="fileRepositoryCookie" href="" target="_blank"><span class="fileIco extpdf">&nbsp;</span>Dolor_sit_amet.pdf</a>
								</span>
								<span class="itemDetails">31 kb</span>
							</span>
						</span>
												
						<span class="icons visibility">
							<span class="icon hide"></span>
							<span class="icon show"></span>
						</span>
											
					</span>
																		
					<span class="renderedfile">
												
						<span class="objectRender file">
							<span class="leftDetail">
								<span class="itemTitle">
									<a title="(Premi per scaricare) Lorem_Ipsum.pdf (31 kb)." class="fileRepositoryCookie" href="" target="_blank"><span class="fileIco extpdf">&nbsp;</span>Dolor_sit_amet.pdf</a>
								</span>
								<span class="itemDetails">31 kb</span>
							</span>
						</span>
												
						<span class="icons visibility">
							<span class="icon hide"></span>
							<span class="icon show"></span>
						</span>
											
					</span>--%>
				</div>
																				 
				<%--<div class="fieldrow uploader">
					<div class="uploader">
						<input  type="file"/>
						<input class="Link_Menu" type="button" value="Upload"/>
					</div>
				</div>--%>
			
			<div class="fieldobject toolbar clearfix">
				<div class="fieldrow right">
					<CTRL:AttachmentsCommands ID="CTRLcommands" runat="server" Visible="true" />
				</div>
			</div>
									
			<div class="messagefooter clearfix">
									
				<div class="fieldrow status clearfix">
					<asp:Label ID="LBucStatus_t" runat="server" CssClass="fieldlabel">*Status</asp:Label>
					<span class="value inputwrapper">
						<asp:DropDownList ID="DDLucStatus" runat="server"></asp:DropDownList>
					</span>					
				</div>
									
				<div class="fieldrow markhidden">
					<asp:CheckBox ID="CBXucShowToUser" runat="server" />
				</div>
								
				<div class="fieldrow submit">
					<asp:LinkButton ID="LNBucSubmit" runat="server" CssClass="linkMenu big">*Submit</asp:LinkButton>

				<%--	<div id="DIVexport" class="ddbuttonlist enabled" visible="true"><!-- 
						--><asp:LinkButton ID="LKBucSubmit" runat="server" CssClass="linkMenu big">*Submit</asp:LinkButton><!--
						--><asp:LinkButton ID="LKBucSubmitSign" runat="server" CssClass="linkMenu big">*Submit & Sign</asp:LinkButton><!--
						--><asp:LinkButton ID="LKBucSubmitLock" runat="server" CssClass="linkMenu big">*Submit & Lock</asp:LinkButton><!--
					--></div>--%>
				</div>
				
			</div>

			<div class="fieldobject collapsable moderation">
				<div class="fieldrow title">
					<asp:Literal ID="LTmoderationTitle_t" runat="server">Moderazione</asp:Literal>
				</div>
				<div class="fieldrow">
					<asp:LinkButton ID="LNBreport" runat="server" CssClass="Link_Menu">Segnala</asp:LinkButton>
                    <asp:LinkButton ID="LNBunreport" runat="server" Visible="false" CssClass="Link_Menu">Riattiva</asp:LinkButton>
					<asp:LinkButton ID="LNBblock" runat="server" CssClass="Link_Menu">Blocca</asp:LinkButton>
					<asp:LinkButton ID="LNBunblock" runat="server" Visible="false" CssClass="Link_Menu">Riattiva</asp:LinkButton>
					<asp:LinkButton ID="LNBdelete" runat="server" Visible="false" CssClass="Link_Menu">Annulla(Elimina)</asp:LinkButton>
				</div>
			</div>
		</div>

<%--            <div class="fieldrow action">
				*Azione:
				<input type="radio" checked="checked" value="0" title="Nessuna" name="Action" checked="checked"><span>none</span>
				<input type="radio" checked="checked" value="1" title="Nessuna" name="Action"><span>Segnala</span>
				<input type="radio" checked="checked" value="2" title="Nessuna" name="Action"><span>Blocca</span>
			</div>--%>

		</div>
<!--	</div>-->