<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AddAction.ascx.vb" Inherits="Comunita_OnLine.UC_AddAction" %>
<%@ Register TagPrefix="CTRL" TagName="ModuleToRepository" Src="~/Modules/Repository/UC/UC_ModuleToRepository.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModuleToQuiz" Src="~/Questionari/UserControls/UC_ModuleToQuiz.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="TextAction" Src="~/Modules/EduPath/UC/UC_EditTextAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="EditCertificationAction" Src="~/Modules/EduPath/UC/UC_EditCertificationAction.ascx" %>

<div class="Row" id="DVcommandsTop" runat="server">
    <b>
        <asp:Literal ID="LTcurrentAction" runat="server"></asp:Literal>
    </b>
    <hr />
    <div class="ContainerLeft">
        
    </div>
    <div style="text-align:right">
        <asp:Button ID="BTNcreateActionTop" Text="" runat="server" Visible="false" />
        <asp:Button ID="BTNselectActionTop" Text="" runat="server" Visible="false" />
        <asp:Button ID="BTNcloseAddActionWindowTop" Text="Close" runat="server" />
    </div>
</div>
<asp:MultiView ID="MLVaddSubActivity" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server">

    </asp:View>   
    <asp:View ID="VIWselector" runat="server">
        <div class="Row">
            <div class="ContainerLeft">
                <asp:Button ID="BTNaddRepositoryActivity" Text="*Add File" runat="server" CssClass="LinkMenu" />
            </div>
            <div class="ContainerRight">
                <asp:Label ID="LBsubActRepository" runat="server" CssClass="Details_Campo" />
            </div>
        </div>
        <div class="Row">
            <div class="ContainerLeft">
                <asp:Button ID="BTNaddTextAction" Text="*Add test" runat="server" CssClass="LinkMenu"  />
            </div>
            <div class="ContainerRight">
                <asp:Label ID="LBsubActText" runat="server" CssClass="Details_Campo" />
            </div>
        </div>
        <div class="Row" id="DVquestionnaire" runat="server">
            <div class="ContainerLeft">
                <asp:Button ID="BTNaddQuestionario" Text="*Add test" runat="server" CssClass="LinkMenu" />
            </div>
            <div class="ContainerRight">
                <asp:Label ID="LBsubActQuiz" runat="server" CssClass="Details_Campo" />
            </div>
        </div>
        <div class="Row" id="DVcertifications" runat="server">
            <div class="ContainerLeft">
                <asp:Button ID="BTNaddCertificationAction" Text="*Add certification" runat="server" CssClass="LinkMenu" />
            </div>
            <div class="ContainerRight">
                <asp:Label ID="LBsubActCertification" runat="server" CssClass="Details_Campo" />
            </div>
        </div>
        <div class="Row" id="DVwebinar" runat="server">
            <div class="ContainerLeft">
                <asp:Button ID="BTNaddWebinarAction" Text="*Add Webinar" runat="server" CssClass="LinkMenu" />
            </div>
            <div class="ContainerRight">
                <asp:Label ID="LBsubActWebinar" runat="server" CssClass="Details_Campo" />
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWrepository" runat="server">
        <CTRL:ModuleToRepository ID="CTRLmoduleToRepository" runat="server" AjaxViewUpdate="true" MaxFileInput="5" />
    </asp:View>
    <asp:View ID="VIWquiz" runat="server">
        <CTRL:ModuleToQuiz ID="CTRLmoduleToQuiz" runat="server" AjaxViewUpdate="true"/>
    </asp:View>
    <asp:View ID="VIWcertifications" runat="server">
        <div class="Row">
            <CTRL:EditCertificationAction runat="server" ID="CTRLcertificationAction" IsInAjaxPanel="True" />
        </div>
    </asp:View>
    <asp:View ID="VIWtextAction" runat="server">
        <div class="Row">
            <CTRL:TextAction id="CTRLtextAction" runat="server"></CTRL:TextAction>
        </div>
    </asp:View>
</asp:MultiView>
 <div class="Row" id="DVcommandsBottom" runat="server">
    <div class="ContainerLeft">
        
    </div>
    <div style="text-align:right">
        <asp:Button ID="BTNcreateActionBottom" Text="" runat="server" Visible="false" />
        <asp:Button ID="BTNselectActionBottom" Text="" runat="server" Visible="false" />
        <asp:Button ID="BTNcloseAddActionWindowBottom" Text="Close" runat="server" />        
    </div>
</div>