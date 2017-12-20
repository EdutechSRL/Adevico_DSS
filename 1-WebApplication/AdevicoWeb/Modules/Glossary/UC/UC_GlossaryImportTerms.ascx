<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_GlossaryImportTerms.ascx.vb" Inherits="Comunita_OnLine.UC_GlossaryImportTerms" %>
<%@ Register TagPrefix="CTRL" TagName="Switch" Src="~/Modules/Common/UC/UC_Switch.ascx" %>


<ul class="communities defaultservicecontainers clearfix">
    <li class="community defaultservicecontainer default toolbar collapsable">
        <div class="innerwrapper">
            <div class="itemheader clearfix">
                <div class="left">
                    <h4 class="title">
                        <span class="handle expander"></span>
                        <asp:Label ID="LBcommunityName" runat="server" CssClass="text"></asp:Label>
                    </h4>
                    <div class="subtitle extra">
                        <%--<span class="mapto mapped">--%>
                        <asp:Label ID="LBmapTo" runat="server" CssClass="mapto mapped">
                            <%--<span class="fieldlabel">Mapped to</span>--%>
                            <%--<a class="name">Lorem ipsum</a>--%>
                            <asp:Label ID="LBMappedTo" runat="server" CssClass="fieldlabel">*Mapped to:</asp:Label>
                            <asp:LinkButton runat="server" CssClass="name" ID="LBChooseToCommunity" Text="*Seleziona Comunità" OnClick="ClickCommunity"/>
                            <asp:Literal runat="server" ID="LTSelectedCommunity" Visible="false"></asp:Literal>
                            <asp:Literal runat="server" ID="LTControlId" Visible="false"></asp:Literal>
                        </asp:Label>
                        <%--</span>--%>
                    </div>
                </div>
                <div class="right">
                    <CTRL:Switch ID="SWHcommunity" runat="server" Status="true"/>
                </div>

                <div class="clearer"></div>
            </div>
            <div class="itemcontent">
                <ul class="communities defaultservicecontainers">
                    <asp:Repeater runat="server" ID="RPTGlossaries" OnItemDataBound="RPTGlossaries_ItemDataBound">

                        <ItemTemplate>
                            <li class="glossary defaultservicecontainer default toolbar collapsable">
                                <div class="innerwrapper">
                                    <div class="itemheader clearfix">
                                        <div class="left">
                                            <h4 class="title">
                                                <span class="handle expander"></span>
                                                <asp:Label ID="LBglossaryName" runat="server" CssClass="text"></asp:Label>
                                            </h4>
                                        </div>
                                        <div class="right">
                                            <CTRL:Switch ID="SWHGlossary" runat="server" Status="true"/>
                                        </div>

                                        <div class="clearer"></div>
                                    </div>
                                    <div class="itemcontent">
                                        <span class="radiobuttonlist">
                                            <asp:Repeater runat="server" ID="RPTerms" OnItemDataBound="RPTerms_ItemDataBound">
                                                <ItemTemplate>
                                                    <span class="item">
                                                        <asp:CheckBox runat="server" ID="CBXterm" />
                                                        <asp:Label ID="LBterm" runat="server" AssociatedControlID="CBXterm"></asp:Label>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </span>
                                        <span class="commands">
                                          <%--  <asp:LinkButton ID="LNBselectAll" runat="server" CssClass="command selectall" OnClick="LNBselectAll_Click">*Select All</asp:LinkButton>
                                            <asp:LinkButton ID="LNBselectNone" runat="server" CssClass="command selectnone" OnClick="LNBselectNone_Click">*Select None</asp:LinkButton>--%>
                                             <asp:Label ID="LBselectAll" runat="server" CssClass="command selectall" >*Select All</asp:Label>
                                            <asp:Label ID="LBselectNone" runat="server" CssClass="command selectnone" >*Select None</asp:Label>
                                        </span>
                                    </div>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
    </li>
</ul>