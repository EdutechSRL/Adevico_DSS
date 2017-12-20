<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_GlossariesList.ascx.vb" Inherits="Comunita_OnLine.UC_GlossariesList" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<asp:Literal ID="LTmessageheaderCss" runat="server" Visible="false">fieldobject _fieldgroup hide</asp:Literal>

<div id="DVmessages" class="fieldobject fieldgroup hide" runat="server">
    <div class="fieldrow">
        <CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false" />
    </div>
</div>

<div class="glossarywrapper list">
    <div class="glossariescontent">
        <div class="DivEpButton">
            <div class="ddbuttonlist top enabled"><!--
                --><asp:HyperLink runat="server" ID="HYPglossaryImportIntoCommunity" CssClass="linkMenu">*Import glossaries into community</asp:HyperLink><!--
                --><asp:HyperLink runat="server" ID="HYPglossaryFromFile" CssClass="linkMenu">*Import glossaries from file</asp:HyperLink><!--
                --><asp:HyperLink runat="server" ID="HYPglossaryToFile" CssClass="linkMenu">*Export glossaries to file</asp:HyperLink><!--
            --></div>
            <asp:HyperLink runat="server" ID="HYPmanageGlossary" CssClass="linkMenu">*Manage Glossaries</asp:HyperLink><asp:HyperLink runat="server" ID="HYPrecycleBin" CssClass="linkMenu">*Recycle Bin</asp:HyperLink>
        </div>
        <div class="tiles container_12 clearfix">
            <div class="asidecontent grid_12">
                <div class="filters clearfix collapsable collapsed">
                    <div class="sectionheader clearfix">
                        <div class="left">
                            <h3 class="sectiontitle clearfix">
                                <asp:Literal ID="LTsearchTools_t" runat="server">*Search tools</asp:Literal>
                                <span class="extrainfo expander">
                                    <asp:Label runat="server" CssClass="on" ID="LBclickExpand_t">*click to expand</asp:Label>
                                    <asp:Label runat="server" CssClass="off" ID="LBclickCollapse_t">*click to collapse</asp:Label>
                                </span>
                            </h3>
                        </div>
                        <div class="right">
                            <div class="input-group">
                                <asp:TextBox ID="TXBsearch" runat="server" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="LNBsearch" runat="server" CssClass="btn btn-default">*Search</asp:LinkButton>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="filtercontainer hideme container_12 clearfix">
                        <div class="filter grid_6 textselect">
                            <div class="filterinner">
                                <asp:Label runat="server" CssClass="title" ID="LBlemma_t">*Lemma</asp:Label>
                                <span class="content">
                                    <asp:DropDownList runat="server" CssClass="input" ID="DDLsearchType" />
                                    <asp:TextBox ID="TXBsearchLemma" runat="server" CssClass="input"></asp:TextBox>
                                </span>
                            </div>
                        </div>
                        <div class="filter grid_6 text">
                            <div class="filterinner">

                                <asp:Label runat="server" CssClass="title" ID="LBlemmacontent_t">*Contenuto Lemma</asp:Label>
                                <span class="content">
                                    <asp:TextBox ID="TXBsearchLemmaContent" runat="server" CssClass="input"></asp:TextBox>
                                </span>
                            </div>
                        </div>
                        <div class="filter grid_6 select">
                            <div class="filterinner">
                                <asp:Label runat="server" CssClass="title" ID="LBterms_t">*Termini</asp:Label>
                                <span class="content">
                                    <asp:DropDownList runat="server" CssClass="input" ID="DDLsearchVisibility" />
                                </span>
                            </div>
                        </div>
                        <div class="filter grid_6 buttons">
                            <div class="viewbuttons">
                                <asp:LinkButton ID="LNBreset" runat="server" CssClass="linkMenu">*Reset</asp:LinkButton>
                                <asp:LinkButton ID="LNBsearchApply" runat="server" CssClass="linkMenu">*Apply</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="sectionfooter hideme"></div>

            </div>

            <div class="maincontent grid_12 glossaries">
                <div class="header clearfix first">
                    <h3 class="gridtitle left">
                        <span class="text">
                            <asp:Literal ID="LTcommunityGlossaries" runat="server">*Community Glossaries</asp:Literal>
                        </span>
                    </h3>
                    <div class="tool right">
                        <div class="groupedselector" id="DVorderBySelector" runat="server" visible="true">
                            <asp:Label ID="LBorderBySelectorDescription" runat="server" CssClass="description">*Sort by: </asp:Label>
                            <div class="selectorgroup">
                                <asp:Label ID="LBorderBySelected" runat="server" CssClass="selectorlabel"></asp:Label>
                                <span class="selectoricon">&nbsp;</span>
                            </div>
                            <div class="selectormenu">
                                <div class="selectorinner">
                                    <div class="selectoritems">
                                        <asp:Repeater ID="RPTorderBy" runat="server">
                                            <ItemTemplate>
                                                <%--  <div class="<%# GetCommunityItemActive(Container.DataItem)%>">
                                                    <span class="icon activeicon">&nbsp;</span>
                                                    <asp:LinkButton ID="LNBorderItemsBy" runat="server" CssClass="selectorlabel">
                                                    </asp:LinkButton>
                                                </div>  --%>

                                                <div class="<%# GetCommunityItemActive(Container.DataItem)%>">
                                                    <asp:LinkButton ID="LNBorderItemsBy" runat="server" CssClass="item">
                                                        <span class="icon activeicon">&nbsp;</span>
                                                        <asp:Label ID="LBsortName" runat="server" CssClass="selectorlabel"></asp:Label>
                                                    </asp:LinkButton>
                                                </div>

                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="section internals first clearfix">

                    <asp:Repeater runat="server" ID="RPTlist">
                        <ItemTemplate>
                            <div id="tile-<%# Container.DataItem.Id%>" class="tile <%# GetTileClass(Container.DataItem.Id)%> glossary grid_4">
                                <div class="innerwrapper">
                                    <div class="tileheader clearfix">
                                        <div class="left">
                                            <asp:HyperLink runat="server" ID="HYPname">
                                                <h3>
                                                    <asp:Literal ID="LTname" runat="server"></asp:Literal>
                                                </h3>
                                            </asp:HyperLink>
                                        </div>
                                        <div class="right"></div>
                                    </div>
                                    <div class="tilecontent descriptioncontent clearfix">
                                        <div class="description">
                                            <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
                                        </div>
                                    </div>


                                    <div class="tilefooter">
                                        <div class="footerinner container_12">
                                            <div class="grid_2 alpha">
                                                <div class="footerblockinner">
                                                    <span class="text">
                                                        <span class="templatelanguage" title="<%# GetGlossaryLanguage(Container.DataItem.IdLanguage)%>">
                                                            <asp:Literal ID="LTlanguage" runat="server"></asp:Literal>
                                                        </span>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="grid_6">
                                                <div class="footerblockinner">
                                                    <span class="counter">
                                                        <asp:Literal ID="LTtermsCount" runat="server"></asp:Literal>
                                                    </span>
                                                    <span class="text">lemmas</span>
                                                </div>
                                            </div>
                                            <div class="grid_4 omega">
                                                <div class="footerblockinner">
                                                    <span class="icons">
                                                        <asp:LinkButton runat="server" ID="LNBglossaryDelete" CssClass="icon delete"></asp:LinkButton>
                                                        <asp:HyperLink runat="server" ID="HYPglossaryStats" CssClass="icon stats"></asp:HyperLink>
                                                        <asp:HyperLink runat="server" ID="HYPglossarySettings" CssClass="icon settings"></asp:HyperLink>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <span class="indicator">
                                    <span class="text" title="<%# GetTileClassLocalized(Container.DataItem.Id)%>">
                                        <asp:Literal ID="LTindicator" runat="server"></asp:Literal>
                                    </span>
                                </span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel runat="server" CssClass="tile add glossary custom grid_4" ID="PNLAddTile" Visible="false">

                        <%-- <div id="tile-last" class="tile add glossary custom grid_4">--%>
                        <div class="innerwrapper">
                            <div class="tileheader clearfix">
                                <div class="left">
                                    <asp:HyperLink runat="server" ID="HYPglossaryNew" CssClass="icon stats">
                                        <h3>
                                            <asp:Literal ID="LTcreateNewGlossary" runat="server">*Create New Glossary</asp:Literal>
                                        </h3>
                                    </asp:HyperLink>

                                </div>
                                <div class="right"></div>
                            </div>
                            <div class="tilecontent clearfix">
                                <asp:HyperLink runat="server" ID="HYPglossaryNew2" CssClass="icon comtype_64 add">
                                </asp:HyperLink>
                            </div>
                            <div class="tilefooter">
                                <div class="footerinner container_12">
                                </div>
                            </div>
                        </div>
                        <span class="indicator">
                            <span class="text">add</span>
                        </span>
                        <%--  </div>--%>
                    </asp:Panel>
                </div>

                <%--<div class="header clearfix">--%>
                <asp:Panel runat="server" CssClass="section publics clearfix" ID="PNLHeaderPublic">
                    <h3 class="gridtitle left">
                        <span class="text">
                            <asp:Literal ID="LTpublicGlossaries" runat="server">*Public Glossaries</asp:Literal>
                        </span>
                    </h3>
                    <div class="tool right">

                        <div class="groupedselector" runat="server" visible="true">
                            <asp:Label ID="LBorderBySelectorDescriptionPublic" runat="server" CssClass="description">*Sort by: </asp:Label>
                            <div class="selectorgroup">
                                <asp:Label ID="LBorderBySelectedPublic" runat="server" CssClass="selectorlabel"></asp:Label>
                                <span class="selectoricon">&nbsp;</span>
                            </div>
                            <div class="selectormenu">
                                <div class="selectorinner">
                                    <div class="selectoritems">
                                        <asp:Repeater ID="RPTOrderByPublic" runat="server">
                                            <ItemTemplate>
                                                <%-- <div class="<%# GetPublicItemActive(Container.DataItem)%>">
                                                    <span class="icon activeicon">&nbsp;</span>
                                                    <asp:LinkButton ID="LNBorderItemsBy" runat="server" CssClass="selectorlabel">
                                                    </asp:LinkButton>
                                                </div>--%>
                                                <div class="<%# GetPublicItemActive(Container.DataItem)%>">
                                                    <asp:LinkButton ID="LNBorderItemsBy" runat="server" CssClass="item">
                                                        <span class="icon activeicon">&nbsp;</span>
                                                        <asp:Label ID="LBsortName" runat="server" CssClass="selectorlabel"></asp:Label>
                                                    </asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                    <%--</div>--%>
                </asp:Panel>
                <asp:Panel runat="server" CssClass="section publics clearfix" ID="PNLPublic">



                    <%-- <div class="section publics clearfix">--%>
                    <asp:Repeater runat="server" ID="RPTpublic">
                        <ItemTemplate>
                            <div id="tile-public-<%# Container.DataItem.Id%>" class="tile <%# GetTileClass(Container.DataItem.Id)%> glossary grid_4">
                                <div class="innerwrapper">
                                    <div class="tileheader clearfix">
                                        <div class="left">
                                            <asp:HyperLink runat="server" ID="HYPname">
                                                <h3>
                                                    <asp:Literal ID="LTname" runat="server"></asp:Literal>
                                                </h3>
                                            </asp:HyperLink>
                                        </div>
                                        <div class="right"></div>
                                    </div>
                                    <div class="tilecontent descriptioncontent clearfix">
                                        <div class="description">
                                            <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
                                        </div>
                                    </div>

                                    <div class="tilefooter">
                                        <div class="footerinner container_12">
                                            <div class="grid_2 alpha">
                                                <div class="footerblockinner">
                                                    <span class="text">
                                                        <span class="templatelanguage" title="italian">
                                                            <asp:Literal ID="LTlanguage" runat="server"></asp:Literal>
                                                        </span>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="grid_6">
                                                <div class="footerblockinner">
                                                    <span class="counter">
                                                        <asp:Literal ID="LTtermsCount" runat="server"></asp:Literal>
                                                    </span>
                                                    <span class="text">lemmas</span>
                                                </div>
                                            </div>
                                            <div class="grid_4 omega">
                                                <div class="footerblockinner">
                                                    <span class="icons">
                                                        <asp:HyperLink runat="server" ID="HYPglossaryStats" CssClass="icon stats"></asp:HyperLink>
                                                        <asp:HyperLink runat="server" ID="HYPglossarySettings" CssClass="icon settings"></asp:HyperLink>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <span class="indicator">
                                    <span class="text" title="public">
                                        <asp:Literal ID="LTindicatorPublic" runat="server"></asp:Literal>
                                    </span>
                                </span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <%-- </div>--%>
                </asp:Panel>
                <div class="clearfix"></div>
            </div>

            <div class="asidecontent grid_12">
                <div class="grid_12 legend">
                    <span class="fieldlabel">
                        <asp:Literal ID="LTlegend" runat="server">*Legend</asp:Literal>
                    </span>
                    <span class="inlinewrapper">
                        <span class="legenditem waiting">
                            <span class="indicator"></span>
                            <span class="text" title="">
                                <asp:Literal ID="LTwaitingForApproval" runat="server">*Waiting for approval</asp:Literal>
                            </span>
                        </span>
                        <span class="legenditem external shared">
                            <span class="indicator"></span>
                            <span class="text" title="">
                                <asp:Literal ID="LTexternalShared" runat="server">*External shared</asp:Literal>
                            </span>
                        </span>
                        <span class="legenditem external public">
                            <span class="indicator"></span>
                            <span class="text" title="">
                                <asp:Literal ID="LTexternalPublic" runat="server">*External public</asp:Literal>
                            </span>
                        </span>
                        <span class="legenditem internal shared">
                            <span class="indicator"></span>
                            <span class="text" title="">
                                <asp:Literal ID="LTinternalShared" runat="server">*Internal shared</asp:Literal>
                            </span>
                        </span>
                        <span class="legenditem internal public">
                            <span class="indicator"></span>
                            <span class="text" title="">
                                <asp:Literal ID="LTinternalPublic" runat="server">*Internal public</asp:Literal>
                            </span>
                        </span>
                        <span class="legenditem unpublished">
                            <span class="indicator"></span>
                            <span class="text" title="">
                                <asp:Literal ID="LTunpublished" runat="server">*Unpublished</asp:Literal>
                            </span>
                        </span>
                    </span>
                </div>
            </div>

        </div>
    </div>
</div>
