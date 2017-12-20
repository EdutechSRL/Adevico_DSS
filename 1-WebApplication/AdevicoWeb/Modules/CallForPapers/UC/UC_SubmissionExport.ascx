<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SubmissionExport.ascx.vb" Inherits="Comunita_OnLine.UC_SubmissionExport" %>

<asp:MultiView ID="MLVcontent" runat="server">
    <asp:View ID="VIWempty" runat="server">
        
    </asp:View>
    <asp:View ID="VIWcontrols" runat="server">
        <asp:Literal ID="LTcontainerRenderO" runat="server"  Visible="false">
            <span class="icons">
        </asp:Literal>
                <asp:HyperLink id="HTPdownloadpdf" cssclass="icon export pdf fileRepositoryCookie" runat="server" Visible="false" Target="_blank" ToolTip="Export to Pdf">&nbsp</asp:HyperLink>
                <asp:Button ID="BTNdownloadpdf" cssclass="icon export pdf" runat="server" OnClientClick="blockUIForDownload(2);return true;" CommandName="pdf" Visible="false" ToolTip="Export to Pdf"/>
                <asp:HyperLink id="HYPdownloadrtf" cssclass="icon export rtf fileRepositoryCookie" runat="server"  Visible="false" Target="_blank" ToolTip="Export to Rtf">&nbsp</asp:HyperLink>
                <span class="display:none;">
                <asp:Button ID="BTNdownloadrtf" cssclass="icon export rtf" runat="server" OnClientClick="blockUIForDownload(3);return true;" CommandName="rtf" Visible="false" ToolTip="Export to Rtf"/>
                </span>
                <asp:HyperLink id="HYPexportEvaluations" cssclass="icon export  fileRepositoryCookie" runat="server"  Visible="false" Target="_blank" ToolTip="Export evaluations">&nbsp</asp:HyperLink>
                <asp:Button ID="BTNexportEvaluations" cssclass="icon export evaluations" runat="server" OnClientClick="blockUIForDownload(3);return true;" CommandName="rtf" Visible="false" ToolTip="Export evaluations"/>
                <span class="icon separator" runat="server" id="SPNseparator" visible="true">&nbsp;</span>
                <asp:HyperLink id="HYPdownloadzip" cssclass="icon download zip fileRepositoryCookie" runat="server" Visible="false" Target="_blank" ToolTip="Download">&nbsp</asp:HyperLink>
                <asp:Button ID="BTNdownloadzip" cssclass="icon download zip" runat="server" OnClientClick="blockUIForDownload(1);return true;" CommandName="download" Visible="false" ToolTip="Download"/>
        <asp:Literal ID="LTcontainerRenderC" runat="server" Visible="false">
            </span>
        </asp:Literal>    
    </asp:View>
</asp:MultiView>