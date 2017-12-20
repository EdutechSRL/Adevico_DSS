<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ExternalService.Master" CodeBehind="AccessExternal.aspx.vb" Inherits="Comunita_OnLine.AccessExternal" %>
 <%--Nomi Standard: OK--%>
<%@ MasterType VirtualPath="~/ExternalService.Master" %>

<%@ Register TagPrefix="CTRL" TagName="Language" Src="~/Modules/Ticket/UC/UC_LanguageChange.ascx" %>

<%--<%@ Register TagPrefix="qsf" Namespace="Telerik.QuickStart" %>--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
	<asp:Literal ID="LTpageTitle_t" runat="server">*Accesso Ticket</asp:Literal>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
	<asp:Literal ID="LTViewTitle" runat="server">Titolo view</asp:Literal>
	<%--<asp:Literal ID="LTcontentTitle_t" runat="server">*Lista Ticket</asp:Literal>--%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PreHeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

	<link rel="Stylesheet" href="../../Graphics/Generics/css/4_UI_Elements.css<%=CssVersion()%>" />
	<link rel="Stylesheet" href="../../Graphics/Modules/Ticket/Css/tickets.css<%=CssVersion()%>" />

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">

	<%-- <telerik:RadFormDecorator runat="server" DecoratedControls="All" ID="rfdacc1" DecorationZoneID="audioCaptcha"
		  Skin="Telerik" />--%>

<asp:Panel ID="PNLcontent" runat="server" cssclass="tickets extaccess personal" Visible="true">

	<!-- Template -->
	<asp:Literal ID="LThintTemplate" runat="server" Visible="false">
		<span class="rightwrapper">
			<span class="hint">{HintText}</span>
			<span class="errormessage">{ErrorText}</span>
		</span>
	</asp:Literal>

	<asp:Literal ID="LTmandatoryTemplate" runat="server" Visible="false"><span class="mandatory">*</span></asp:Literal>
	<!-- /Template -->


	<%--<div class="sectionheader">
		<h3></h3>
	</div>--%>

	<div class="innerwrapper clearfix">

		<div class="fielobject top clearfix">
			<div class="fieldrow description left">
				<span class="text"><asp:Literal ID="LTviewDescription" runat="server">Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Duis mollis, est non commodo luctus, nisi erat porttitor ligula, eget lacinia odio sem nec elit. Maecenas faucibus mollis interdum. Nullam quis risus eget urna mollis ornare vel eu leo. Duis mollis, est non commodo luctus, nisi erat porttitor ligula, eget lacinia odio sem nec elit. Curabitur blandit tempus porttitor. Nullam quis risus eget urna mollis ornare vel eu leo.</asp:Literal></span>
			</div>

			<div class="fieldrow right">
				<div class="languageselector">
					<CTRL:Language ID="CTRLlang" runat="server" DDLAutoPostBack="false" />
				</div>
			</div>

		</div>

		<div class="fieldobject maincontent clearfix">
			<div class="mandatorylegend fieldrow">
				<asp:Literal ID="LTmandatory_t" runat="server">
					Marked fields <span class="mandatory">*</span> are mandatory
				</asp:Literal>
			</div>
			<asp:MultiView ID="MLVviews" runat="server">
		
				<asp:View ID="VIWlogin" runat="server">

					<asp:PlaceHolder ID="PLHlogin" runat="server">
						<div class="fieldrow email" id="DIVloginmail" runat="server">
							<span class="leftwrapper">
								<asp:Label ID="LBloginmail_t" runat="server" AssociatedControlID="TXBloginmail" CssClass="fieldlabel">E-Mail</asp:Label>
								<asp:TextBox ID="TXBloginmail" runat="server" CssClass="inputtext"></asp:TextBox>
							</span>
							<asp:Literal ID="LTloginmailHint" runat="server"></asp:Literal>
						</div>

						<div class="fieldrow login" id="DIVlogincode" runat="server"> <%--error">--%>
							<span class="leftwrapper">
								<asp:Label ID="LBlogincode_t" runat="server" AssociatedControlID="TXBlogincode" CssClass="fieldlabel">Code</asp:Label>
								<asp:TextBox ID="TXBlogincode" runat="server" CssClass="inputtext" TextMode="Password"></asp:TextBox>
							</span>
							<asp:Literal ID="LTlogincodeHint" runat="server"></asp:Literal>
						</div>
					</asp:PlaceHolder>

					<asp:PlaceHolder ID="PLHtoken" runat="server">
						<div class="fieldrow token" id="DIVlogintoken" runat="server"> <%--error">--%>
							<span class="leftwrapper">
								<asp:Label ID="LBloginToken_t" runat="server" AssociatedControlID="TXBlogincode" CssClass="fieldlabel">Code</asp:Label>
								<asp:TextBox ID="TXBloginToken" runat="server" CssClass="inputtext"></asp:TextBox>
							</span>
							<asp:Literal ID="LTtokenHint" runat="server"></asp:Literal>
						</div>
					</asp:PlaceHolder>

				</asp:View>

				<asp:View ID="VIWRegister" runat="server">
				
					<div class="fieldrow email" id="DIVregmail" runat="server">
						<span class="leftwrapper">
							<asp:Label ID="LBregmail_t" runat="server" AssociatedControlID="TXBregMail" CssClass="fieldlabel">E-Mail</asp:Label>
							<asp:TextBox ID="TXBregMail" runat="server" CssClass="inputtext"></asp:TextBox>
						</span>
						<asp:Literal ID="LTregmailHint" runat="server"></asp:Literal>
					</div>

					<div class="fieldrow name" id="DIVregname" runat="server">
						<span class="leftwrapper">
							<asp:Label ID="LBregname_t" runat="server" AssociatedControlID="TXBregname" CssClass="fieldlabel">Name</asp:Label>
							<asp:TextBox ID="TXBregname" runat="server" CssClass="inputtext"></asp:TextBox>
						</span>
						<asp:Literal ID="LTregnameHint" runat="server"></asp:Literal>
					</div>

					<div class="fieldrow sname" id="DIVregSname" runat="server">
						<span class="leftwrapper">
							<asp:Label ID="LBregSname_t" runat="server" AssociatedControlID="TXBregname" CssClass="fieldlabel">Surename</asp:Label>
							<asp:TextBox ID="TXBregSname" runat="server" CssClass="inputtext"></asp:TextBox>
						</span>
						<asp:Literal ID="LTregSnameHint" runat="server"></asp:Literal>
					</div>

					<div class="fieldrow password" id="DIVregPassword" runat="server">
						<span class="leftwrapper">
							<asp:Label ID="LBregPwd" runat="server" AssociatedControlID="TXBregPwd" CssClass="fieldlabel">*Password</asp:Label>
							<asp:TextBox ID="TXBregPwd" runat="server" CssClass="inputtext" TextMode="Password"></asp:TextBox>
						</span>
						<asp:Literal ID="LTregPwd" runat="server"></asp:Literal>
					</div>

					<div class="fieldrow sname" id="DIVregPassword2" runat="server">
						<span class="leftwrapper">
							<asp:Label ID="LBregPwd2" runat="server" AssociatedControlID="TXBregPwd2" CssClass="fieldlabel">*Password</asp:Label>
							<asp:TextBox ID="TXBregPwd2" runat="server" CssClass="inputtext" TextMode="Password"></asp:TextBox>
						</span>
						<asp:Literal ID="LTregPwd2" runat="server"></asp:Literal>	
					</div>

				</asp:View>

				<asp:View ID="VIWregistered" runat="server">
					<div class="fieldrow info">
						<asp:Literal ID="LTreged" runat="server"></asp:Literal>
					</div>
				</asp:View>
				
				<asp:View ID="VIWRecover" runat="server">
					<div class="fieldrow email" id="DIVrecMail" runat="server">
						<span class="leftwrapper">
							<asp:Label ID="LBrecMail_t" runat="server" AssociatedControlID="TXBreCMail" CssClass="fieldlabel">E-Mail</asp:Label>
							<asp:TextBox ID="TXBrecMail" runat="server" CssClass="inputtext"></asp:TextBox>
						</span>
						<asp:Literal ID="LTrecMailHint" runat="server"></asp:Literal>
					</div>
				</asp:View>

				<asp:View ID="VIWRecoverRequestSended" runat="server">
					<div class="fieldrow info">
						<asp:Literal ID="LTrecovered" runat="server"></asp:Literal>
					</div>
				</asp:View>
				<%--
				<asp:View ID="V_ChangePassword" runat="server">

					<div class="fieldrow email" id="DIVchangeMail" runat="server">
						<span class="leftwrapper">
							<asp:Label ID="LBLchangeMail_t" runat="server" AssociatedControlID="TXBchangeMail" CssClass="fieldlabel">E-Mail</asp:Label>
							<asp:TextBox ID="TXBchangeMail" runat="server" CssClass="inputtext"></asp:TextBox>
						</span>
						<asp:Literal ID="LITchangeMailHint" runat="server"></asp:Literal>
					</div>

					<div class="fieldrow password" id="DIVchangeOLD" runat="server">
						<span class="leftwrapper">
							<asp:Label ID="LBLchangePwdOLD_t" runat="server" AssociatedControlID="TXBchangePwdOLD" CssClass="fieldlabel">*Password corrente</asp:Label>
							<asp:TextBox ID="TXBchangePwdOLD" runat="server" CssClass="inputtext"></asp:TextBox>
						</span>
						<asp:Literal ID="LITchangeOLD" runat="server"></asp:Literal>
					</div>
					
					<div class="fieldrow password" id="DIVchangeNEW1" runat="server">
						<span class="leftwrapper">
							<asp:Label ID="LBLchangePwdNEW1_t" runat="server" AssociatedControlID="TXBchangePwdNEW1" CssClass="fieldlabel">*Password corrente</asp:Label>
							<asp:TextBox ID="TXBchangePwdNEW1" runat="server" CssClass="inputtext"></asp:TextBox>
						</span>
						<asp:Literal ID="LITchangeNEW1" runat="server"></asp:Literal>
					</div>

					<div class="fieldrow password" id="DIVchangeNEW2" runat="server">
						<span class="leftwrapper">
							<asp:Label ID="LBLchangePwdNEW2_t" runat="server" AssociatedControlID="TXBchangePwdNEW2" CssClass="fieldlabel">*Password corrente</asp:Label>
							<asp:TextBox ID="TXBchangePwdNEW2" runat="server" CssClass="inputtext"></asp:TextBox>
						</span>
						<asp:Literal ID="LITchangeNEW2" runat="server"></asp:Literal>
					</div>
					
					
				</asp:View>
--%>
			</asp:MultiView>            

			<asp:Panel ID="PNLconfirm" runat="server">

				<asp:Panel ID="PNLcatcha" runat="server">
					<%--Modificate classe _captchaType con TIPO captcha--%>
					<div class="fieldrow left captcha imgaudio" id="DIVcaptcha" runat="server">
						<span class="leftwrapper">
							<asp:Label ID="LBcaptcha_t" runat="server" AssociatedControlID="TXBcaptcha" cssclass="fieldlabel">*Inserisci il codice nell'immagine</asp:Label>
							
							
								<!-- INIZIO blocco Captcha -->
								<telerik:RadCaptcha ID="RCPcaptcha" runat="server" 
									ErrorMessage="Page not valid. The code you entered is not valid."
									ValidationGroup="vgAudio" ValidatedTextBoxID="TXBcaptcha"
									Display="None">
									<CaptchaImage EnableCaptchaAudio="true" RenderImageOnly="true"
										ImageCssClass="rcCaptchaImage"
										BackgroundColor="#609f0a" TextColor="White"
										BackgroundNoise="None"
										AudioFilesPath="~/Graphics/RadCaptcha/"
										UseAudioFiles="true"
										></CaptchaImage>
								</telerik:RadCaptcha>
								<!-- FINE blocco Captcha -->
								<asp:TextBox ID="TXBcaptcha" runat="server" MaxLength="5" CssClass="inputtext"></asp:TextBox>
						</span>
						<asp:Literal ID="LTcaptcha" runat="server">Captcha error</asp:Literal>
					</div>


					
				</asp:Panel>

				<div class="fieldrow right submit">
				
					<asp:Button ID="BTNenter" runat="server" CssClass="Link_Menu big" Text="*Enter" />
					<asp:Button ID="BTNcreate"  runat="server" CssClass="Link_Menu big" Text="*Create" />
					<asp:Button ID="BTNrecover"  runat="server" CssClass="Link_Menu big" Text="*Invia richiesta" />
					<asp:Button ID="BTNvalidate"  runat="server" CssClass="Link_Menu big" Text="*Conferma utente" />
					<%--<asp:Button ID="BTNchange"  runat="server" CssClass="Link_Menu big" Text="*Cambia Password" />--%>
					<%--<asp:LinkButton ID="LKBenter" runat="server" CssClass="Link_Menu big">*Enter</asp:LinkButton>
					<asp:LinkButton ID="LKBcreate" runat="server" CssClass="Link_Menu big">*Create</asp:LinkButton>
					<asp:LinkButton ID="LKBsendRequest" runat="server" CssClass="Link_Menu big">*SendRequest</asp:LinkButton>--%>
				</div>

			</asp:Panel>
		</div>
		<!-- END MainContent -->

		<asp:Panel ID="PNLnavigation" runat="server">
			<div id="" class="fieldobject asidecontent clearfix">
				<div class="fieldrow links">
					<asp:LinkButton ID="LNBtoLogin" runat="server" CssClass="accesscommands first">*ToLogin</asp:LinkButton>
					<asp:LinkButton ID="LNBtoCreate" runat="server" CssClass="accesscommands">*ToCreate</asp:LinkButton>
					<asp:LinkButton ID="LNBtoRecover" runat="server" CssClass="accesscommands last">*ToRecover</asp:LinkButton>
				</div>
			</div>
		</asp:Panel>
		
		<telerik:RadScriptBlock ID="RSBscript" runat="server">
		  <script type="text/javascript">
			   //<![CDATA[
			  function pageLoad() {
				  var captchaTextBox = document.getElementById("<%=TXBcaptcha.ClientID %>");
				  captchaTextBox.setAttribute("autocomplete", "off");
			  }
			   //]]>
		  </script>
	 </telerik:RadScriptBlock>
	</div>
</asp:Panel>

<asp:Panel ID="PNLserviceDisabled" runat="server" cssclass="tickets extaccess personal" Visible="false">
	<div class="error">
		<span class="errormessage">
			<asp:Literal ID="LTnoAccess" runat="server">*L'accesso ai Ticket esterni è stato momentaneamente disattivato...</asp:Literal>
		</span>
	</div>
</asp:Panel>

</asp:Content>