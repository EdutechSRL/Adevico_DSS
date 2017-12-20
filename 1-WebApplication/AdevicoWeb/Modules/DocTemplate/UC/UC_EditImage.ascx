<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditImage.ascx.vb" Inherits="Comunita_OnLine.UC_EditImage" %>
<%@ Register TagPrefix="CTRL" TagName="Measures" Src="~/Modules/DocTemplate/Uc/UC_Measure.ascx"%>

<div class="fieldrow">
     <asp:Label ID="LBaddNewImage_t" runat="server" CssClass="fieldlabel">Select file:</asp:Label>
     <div class="fieldblockwrapper">
        <asp:FileUpload ID="FUPimageFile" runat="server" />
        <asp:LinkButton ID="LKBaddImageFile" runat="server" CssClass="linkMenu " Text="#Upload" />
        <asp:HiddenField ID="HIDimgPath" runat="server" />
    </div>
</div>
<div class="fieldrow">
    <asp:Image ID="IMGpreview" runat="server" ImageUrl="" CssClass="fieldlabel image"/>
    <asp:panel id="PNLimgSize" runat="server">
        <div class="fieldblockwrapper">
            <CTRL:Measures ID="UCimgWidth" runat="server" ShowDot="true" Label="*Width: " />
            <CTRL:Measures ID="UCimgHeight" runat="server" ShowDot="true" Label="*Height: "/>
        </div>
    </asp:panel>
</div>