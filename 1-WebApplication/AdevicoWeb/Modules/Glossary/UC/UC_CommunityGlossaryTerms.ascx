<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunityGlossaryTerms.ascx.vb" Inherits="Comunita_OnLine.UC_CommunityGlossaryTerms" %>
<%@ Register TagPrefix="CTRL" TagName="GlossaryImportTerms" Src="~/Modules/Glossary/UC/UC_GlossaryImportTerms.ascx" %>


<asp:Repeater runat="server" ID="RPTCommunites">
    <ItemTemplate>
        <CTRL:GlossaryImportTerms ID="UCglossaryTerms" runat="server" ItemDataBound="RPTCommunites_ItemDataBound"/>
    </ItemTemplate>
</asp:Repeater>