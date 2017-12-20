<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_GlossaryShareState.ascx.vb" Inherits="Comunita_OnLine.UC_GlossaryShareState" %>
<%@ Register TagPrefix="CTRL" TagName="Switch" Src="~/Modules/Common/UC/UC_Switch.ascx" %>

<div class="content">
    <asp:MultiView runat="server" ID="MLVglossaryShare">

        <asp:View runat="server" ID="VIWglossarySharePending">
            <div class="grid_12 defaultservicecontainer sharing pending notenabled collapsable">
                <div class="box first last innerwrapper">
                    <div class="itemheader objectheader clearfix">
                        <div class="left">
                            <h4 class="title">
                                <span class="handle expander"></span>
                                <span class="text">
                                    <asp:Literal ID="LTsharedFrom_t" runat="server">* Shared from </asp:Literal>
                                    <span class="name">
                                        <asp:Literal ID="LTcommunityFrom" runat="server">* Community XYZ</asp:Literal>
                                    </span>
                                </span>
                                <span class="status">
                                    <span class="option <%=GetShareState()%>">
                                        <span class="statusitem">
                                            <asp:Literal ID="LTshareState" runat="server">* LTshareState3</asp:Literal>
                                        </span>
                                    </span>
                                </span>
                            </h4>
                        </div>
                        <div class="right"></div>
                        <div class="clearer"></div>
                    </div>
                    <div class="itemcontent">
                        <div class="fieldobject">
                            <div class="fieldrow status">
                                <asp:Label ID="LBsharedStatus_t" runat="server" CssClass="fieldlabel title">* Sharing Status:</asp:Label>

                                <div class="inlinewrapper">
                                    <div class="fieldrow status">
                                        <span class="optiongroup">
                                            <span class="option pending">
                                                <span class="statusitem">
                                                    <asp:Literal ID="LTshareState2" runat="server">* LTshareState2</asp:Literal>
                                                </span>
                                            </span>
                                        </span>


                                        <asp:Panel runat="server" CssClass="optiongroup" ID="PNLRefuse1" Visible="false">
                                            <span class="option">
                                                <asp:Label ID="LBallowGlossarySharing_t" runat="server" CssClass="text">* Allow glossary sharing</asp:Label>
                                                <asp:LinkButton ID="LNBshareAccept" runat="server" CssClass="big linkMenu">* Accept</asp:LinkButton>
                                            </span>
                                            <span class="option">
                                                <asp:Label ID="LBorRefuseIt_t" runat="server" CssClass="text">* or permanently refuse it</asp:Label>
                                                <asp:LinkButton ID="LNBshareRefuse" runat="server" CssClass="big linkMenu">* Refuse</asp:LinkButton>
                                            </span>
                                        </asp:Panel>
                                    </div>
                                </div>

                            </div>
                            <div class="fieldrow permissions">
                                <label class="fieldlabel title">
                                    <asp:Literal ID="LTpermission_t" runat="server">* Permissions</asp:Literal>
                                </label>
                                <div class="inlinewrapper">
                                    <div class="options">
                                        <span class="option">
                                            <asp:CheckBox runat="server" ID="CBXaddTerm" />
                                            <asp:Label AssociatedControlID="CBXaddTerm" ID="LBaddTerm_t" runat="server" CssClass="text" Enabled="false">* Add Term</asp:Label>
                                        </span><span class="option">
                                            <asp:CheckBox runat="server" ID="CBXdeleteTerm" />
                                            <asp:Label AssociatedControlID="CBXdeleteTerm" ID="LBdeleteTerm_t" runat="server" CssClass="text" Enabled="false">* Delete Term</asp:Label>
                                        </span><span class="option">
                                            <asp:CheckBox runat="server" ID="CBXeditTerm" />
                                            <asp:Label AssociatedControlID="CBXeditTerm" ID="LBeditTerm_t" runat="server" CssClass="text" Enabled="false">* Edit Term</asp:Label>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="fieldrow description">
                                <asp:Literal ID="LTglossaryDescription" runat="server">* Description...some text</asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>

        <asp:View runat="server" ID="VIWglossaryShareEnabled">
            <div class="grid_12 defaultservicecontainer sharing active enabled collapsable">
                <div class="box first last innerwrapper">
                    <div class="itemheader objectheader clearfix">
                        <div class="left">
                            <h4 class="title">
                                <span class="handle expander"></span>
                                <span class="text">
                                    <asp:Literal ID="LTsharedFrom2_t" runat="server">* Shared from </asp:Literal>
                                    <span class="name">
                                        <asp:Literal ID="LTcommunityFrom2" runat="server">* Community XYZ</asp:Literal>
                                    </span>
                                </span>
                                <span class="status">
                                    <span class="option active">
                                        <span class="statusitem">
                                            <asp:Literal ID="LTshareState3" runat="server">* LTshareState3</asp:Literal></span>
                                    </span>
                                </span>

                            </h4>
                        </div>
                        <div class="right"></div>
                        <div class="clearer"></div>
                    </div>
                    <div class="itemcontent">
                        <div class="fieldobject">
                            <div class="fieldrow status">
                                <asp:Label ID="LBsharedStatus2_t" runat="server" CssClass="fieldlabel title">* Sharing Status:</asp:Label>

                                <div class="inlinewrapper">
                                    <div class="fieldrow status">
                                        <span class="optiongroup">
                                            <span class="option active">
                                                <span class="statusitem">
                                                    <asp:Literal ID="LTshareState4" runat="server">* LTshareState4</asp:Literal>
                                                </span>
                                            </span>
                                        </span>
                                    </div>
                                </div>

                            </div>
                            <div class="fieldrow permissions">
                                <label class="fieldlabel title">
                                    <asp:Literal ID="LTpermission2_t" runat="server">* Permissions</asp:Literal>
                                </label>
                                <div class="inlinewrapper">
                                    <div class="options">
                                        <span class="option">
                                            <asp:CheckBox runat="server" ID="CBXaddTerm2" Enabled="False" />
                                            <asp:Label AssociatedControlID="CBXaddTerm2" ID="LBaddTerm2_t" runat="server" CssClass="text" Enabled="False">* Add Term</asp:Label>
                                        </span><span class="option">
                                            <asp:CheckBox runat="server" ID="CBXdeleteTerm2" Enabled="False" />
                                            <asp:Label AssociatedControlID="CBXdeleteTerm2" ID="LBdeleteTerm2_t" runat="server" CssClass="text" Enabled="False">* Delete Term</asp:Label>
                                        </span><span class="option">
                                            <asp:CheckBox runat="server" ID="CBXeditTerm2" Enabled="False" />
                                            <asp:Label AssociatedControlID="CBXeditTerm2" ID="LBeditTerm2_t" runat="server" CssClass="text" Enabled="False">* Edit Term</asp:Label>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="fieldrow description">
                                <asp:Literal ID="LTglossaryDescription2" runat="server">* Description...some text</asp:Literal>
                            </div>
                            <div class="fieldrow status clearfix">
                                <div class="left">

                                    <asp:Label ID="LBvisibiltyStatus2_t" runat="server" CssClass="fieldlabel title">* Visibility status</asp:Label>
                                    <CTRL:Switch ID="SWHshareEnabled" runat="server" Status="true"/>

                                </div>
                                <asp:Panel runat="server" CssClass="right" ID="PNLRefuse" Visible="false">
                                    <label class="text">
                                        <asp:Literal ID="LTpermanentDiscard2_t" runat="server">* Permanently discard this glossary</asp:Literal>
                                        <asp:LinkButton ID="LNBrefuse2" runat="server" CssClass="linkMenu">* Discard</asp:LinkButton>
                                    </label>
                                </asp:Panel>
                                <div class="clearer"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </asp:View>

        <asp:View runat="server" ID="VIWglossaryShareRefused">
            <%--   <h3>Refused</h3>--%>
        </asp:View>

        <asp:View runat="server" ID="VIWglossaryShareEmpty">
            <%--  <h3>Empty</h3>--%>
        </asp:View>
    </asp:MultiView>
</div>

<div class="footer">
</div>