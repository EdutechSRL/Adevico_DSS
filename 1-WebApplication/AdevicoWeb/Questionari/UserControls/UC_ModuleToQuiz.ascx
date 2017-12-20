<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleToQuiz.ascx.vb"
    Inherits="Comunita_OnLine.UC_ModuleToQuiz" %>
<%@ Register TagPrefix="CTRL" TagName="CloneCommQuiz" Src="~/Questionari/UserControls/UC_SelectQuiz.ascx" %>
<style type="text/css">
    .right
    {
        text-align: right;
    }
    UL LI
    {
        list-style-type: none;
    }
    .UploaderContainer
    {
        float: left;
        width: 80%;
    }
    .UploaderButtonContainer
    {
        float: left;
        width: 19%;
        padding-top: 210px;
        padding-left: 10px;
    }
</style>
<div class="Row" id="DVcommandsTop" runat="server" visible="false">
    <b>
        <asp:Literal ID="LTcurrentAction" runat="server"></asp:Literal>
    </b>
    <hr />
    <div class="ContainerLeft">
        
    </div>
    <div style="text-align:right">
        <asp:Button ID="BTNcloseAddActionWindowTop" Text="Close" runat="server" />
        <asp:Button ID="BTNselectActionTop" Text="" runat="server" Visible="false" />
        <asp:Button ID="BTNLinkToModuleTop" runat="server" Text="Link" Visible="false"  />
    </div>
</div>
<asp:MultiView ID="MLVcontrol" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server">
    
    </asp:View>
    <asp:View ID="VIWactionSelector" runat="server">
        <asp:Repeater ID="RPTactions" runat="server">
            <ItemTemplate>
                <div class="Row">
                    <div class="ContainerLeft">
                        <asp:Button ID="BTNaddAction" Text="Add action" runat="server" />
                    </div>
                    <div class="ContainerRight">
                        <asp:Literal ID="LTaddAction" runat="server"></asp:Literal>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </asp:View>
    <asp:View ID="VIWaddCommunityQuizAction" runat="server">
        <div class="Row">
            <CTRL:CloneCommQuiz id="CTRLcloneCommQuiz" runat="server" />
        </div>
    </asp:View>
</asp:MultiView>
 <div class="Row" id="DVcommandsBottom" runat="server" visible="false">
    <div class="ContainerLeft">
        
    </div>
    <div style="text-align:right">
        <asp:Button ID="BTNcloseAddActionWindowBottom" Text="Close" runat="server" />
        <asp:Button ID="BTNselectActionBottom" Text="" runat="server" Visible="false" />
        <asp:Button ID="BTNLinkToModuleBottom" runat="server" Text="Link" Visible="false"  />
    </div>
</div>