<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditSettings.ascx.vb" Inherits="Comunita_OnLine.UC_EditSettings" %>

<%@ Register TagPrefix="CTRL" TagName="Measures" Src="~/Modules/DocTemplate/Uc/UC_Measure.ascx"%>
<%@ Register TagPrefix="CTRL" TagName="Image" Src="~/Modules/DocTemplate/Uc/UC_EditImage.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="HFPlacing" Src="~/Modules/DocTemplate/Uc/UC_EditPagePlacing.ascx" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Src="~/Modules/DocTemplate/Uc/UC_EditVersions.ascx" TagName="CTRLprevVersion" TagPrefix="CTRL" %>

<div class="fieldobject">
	<div class="fieldrow fieldtitle">
		<asp:Label ID="LBLname_t" runat="server" CssClass="fieldlabel">*Template name</asp:Label>
		<asp:TextBox ID="TXBname" runat="server" CssClass="inputtext" Enabled="false"></asp:TextBox>
		<span class="right code">
			<asp:Label ID="LBLcode_t" cssclass="fieldlabel" runat="server">Cod.</asp:Label>&nbsp;
			<asp:Label ID="LBLcode" cssclass="fieldlabel" runat="server">##</asp:Label>&nbsp;
		</span>
	</div>
</div>
					
<fieldset class="light">
	<legend>
		<asp:Literal ID="LITlayout_t" runat="server">*Page Layout</asp:Literal>
	</legend>

	<div class="fieldobject">			            
		<div class="fieldrow pagesize">
			<asp:Label ID="LBLpagesize" runat="server" CssClass="fieldlabel">*Page size</asp:Label>
			<span class="fieldblockwrapper">
				<asp:Label ID="LBLpageFormat_t" runat="server">*format</asp:Label>
				<asp:DropDownList ID="DDLpageFormat" runat="server" AutoPostBack="true">
				</asp:DropDownList>
				<br />
				<CTRL:Measures ID="CTRLmeasureWidth" runat="server" Label="*width" />
				<CTRL:Measures ID="CTRLmeasureHeight" runat="server" Label="*height" />
			</span>
		</div>
						
		<div class="fieldrow pagemargins">
			<asp:Label ID="LBLmargin_t" runat="server" CssClass="fieldlabel">*Margin</asp:Label>
			<span class="fieldblockwrapper">
				<CTRL:Measures ID="CTRLmarginTop" runat="server" Label="*top" />
				<CTRL:Measures ID="CTRLmarginRight" runat="server" Label="*right" />
				<br />
				<CTRL:Measures ID="CTRLmarginBottom" runat="server" Label="*bottom" />
				<CTRL:Measures ID="CTRLmarginLeft" runat="server" Label="*left" />
			</span>    
		</div>
					  
	</div>
						
</fieldset>

<fieldset class="light">
	<legend>
		<asp:Literal ID="LITbackground_t" runat="server">*Background</asp:Literal>
	</legend>
	<div class="fieldobject">
						
		<div class="fieldrow">
			<asp:Label ID="LBLtype_t" runat="server" CssClass="fieldlabel">*Type</asp:Label>
			<asp:RadioButtonList ID="RBLbgType" runat="server" AutoPostBack="true" RepeatDirection="Vertical" RepeatLayout="Flow" CssClass="fieldblockwrapper">
				<asp:ListItem Value="0" Text="*None"></asp:ListItem>
				<asp:ListItem Value="1" Text="*Color"></asp:ListItem>
				<asp:ListItem Value="2" Text="*Image"></asp:ListItem>
			</asp:RadioButtonList>
		</div>
											

		<div class="fieldrow">
				<asp:MultiView id="MLVbgType" runat="server">
					<asp:View ID="Vnone" runat="server"></asp:View>
					<asp:View ID="Vcolor" runat="server">
						<div class="fieldblockwrapper contentcontextual color">
							<div class="fieldrow">
								 <asp:Label ID="LBLbgColor_t" runat="server" CssClass="fieldlabel">#Color</asp:Label>
								 <div class="fieldblockwrapper">
									<telerik:RadColorPicker ID="RDPbackgorundColor" runat="server" AutoPostBack="true" 
									ShowIcon="true" EnableCustomColor="True" Preset="Paper" ></telerik:RadColorPicker>
								</div>
							</div>
						</div>
					</asp:View>
					<asp:View ID="Vimage" runat="server">
						<asp:Label Id="LBLbgimage_t" runat="server" CssClass="fieldlabel">#image</asp:Label>
						<span class="fieldblockwrapper contentcontextual image">
							<CTRL:Image Id="UCimage" runat="server" ShowMeasure="False"></CTRL:Image>
						</span>
						<span class="fieldblockwrapper contentcontextual arrange">
							<asp:Label Id="LBLbgImgaArrange_t" runat="server" CssClass="fieldlabel">#arrange</asp:Label>
							<asp:DropDownList ID="DDLimageArrange" runat="server">
								<asp:ListItem Text="*Center" Value="-1"></asp:ListItem>
								<asp:ListItem Text="*Tiled" Value="0"></asp:ListItem>
								<asp:ListItem Text="*Stretch" Value="1"></asp:ListItem>
							</asp:DropDownList>
						</span>
					</asp:View>
				</asp:MultiView>
		</div>
	</div>
</fieldset>
<fieldset class="light">
	<legend>
		<asp:Literal ID="LITgenerics_t" runat="server">*Generics</asp:Literal>
	</legend>
	<div class="fieldobject">
		<div class="fieldrow">
			<asp:Label ID="LBLhfPosition_t" CssClass="fieldlabel" runat="server">#Mostra Header e Footer in:</asp:Label>
			<div class="fieldblockwrapper">
				<CTRL:HFPlacing id="UCplacing" runat="server"></CTRL:HFPlacing>
			</div>
		</div>	       
	</div>
</fieldset>
<fieldset class="light">
	<legend>
		<asp:Literal ID="LITpdfData" runat="server">*Pdf Data</asp:Literal>
	</legend>
		
	<div class="fieldobject">
		<span class="field">
			<asp:Label ID="LBLtitle_t" runat="server" CssClass="fieldlabel">*Template name</asp:Label>
			<asp:TextBox ID="TXBtitle" runat="server" CssClass="inputtext" Enabled="false"></asp:TextBox>
		</span>
		<span class="field">
			<asp:Label ID="LBLsubject_t" runat="server" CssClass="fieldlabel">*Subject</asp:Label>
			<asp:TextBox ID="TXBsubject" runat="server" CssClass="inputtext" Enabled="false"></asp:TextBox>
		</span>
		<span class="field">
			<asp:Label ID="LBLauthor_t" runat="server" CssClass="fieldlabel">*Author</asp:Label>
			<asp:TextBox ID="TXBauthor" runat="server" CssClass="inputtext" Enabled="false"></asp:TextBox>
		</span>
		<span class="field">
			<asp:Label ID="LBLcreator_t" runat="server" CssClass="fieldlabel">*Creator</asp:Label>
			<asp:TextBox ID="TXBcreator" runat="server" CssClass="inputtext" Enabled="false"></asp:TextBox>
		</span>
		<span class="field">
			<asp:Label ID="LBLproducer_t" runat="server" CssClass="fieldlabel">*Producer</asp:Label>
			<asp:TextBox ID="TXBproducer" runat="server" CssClass="inputtext" Enabled="false"></asp:TextBox>
		</span>
		<span class="field">
			<asp:Label ID="LBLkeywords_t" runat="server" CssClass="fieldlabel">*Keywords</asp:Label>
			<asp:TextBox ID="TXBkeywords" runat="server" CssClass="inputtext" Enabled="false"></asp:TextBox>
		</span>
	</div>
</fieldset>

<asp:Panel ID="PNLsubVersion" runat="server" Visible="false">
	<fieldset class="light">
		<legend>
			<asp:literal ID="LITrevision_t" runat="server">*Revision</asp:literal>
		</legend>
		<div class="fieldrow">
			<CTRL:CTRLprevVersion ID="UCprevVersion" runat="server" />
		</div>
	</fieldset>
</asp:Panel>