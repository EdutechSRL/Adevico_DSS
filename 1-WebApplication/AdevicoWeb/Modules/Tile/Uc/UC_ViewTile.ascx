<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ViewTile.ascx.vb" Inherits="Comunita_OnLine.UC_ViewTile" %>

<div class="edit">
    <div class="fieldobject box first settings">
        <div class="fieldrow objectheader">
            <h4 class="title"><asp:Literal ID="LTeditTileTitleSettings" runat="server">*Settings</asp:Literal></h4>
        </div>
        <div class="fieldrow status">
            <asp:Label ID="LBtileStatus_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBtileStatus">*Status:</asp:Label>
            <asp:Label ID="LBtileStatus" runat="server" CssClass="inputtext"></asp:Label>
        </div>
        <div class="fieldrow type">
            <asp:Label ID="LBtileType_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBtileType">*Type:</asp:Label>
            <span class="inputtext">
                <asp:Label ID="LBtileType" runat="server" CssClass="inputtext"></asp:Label>
            </span>
        </div>
        <div class="fieldrow url" id="DVurl" runat="server" visible="false">
            <asp:Label ID="LBtileUrl_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBtileUrl">*Url:</asp:Label>
            <asp:Label ID="LBtileUrl" runat="server" CssClass="inputtext">*Url:</asp:Label>
        </div>
        <div class="fieldrow selector" id="DVtagsSelector" runat="server" visible="false">
            <asp:Label ID="LBtileSelectedTags_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBtileSelectedTags">*Selected tags:</asp:Label>
            <asp:Label ID="LBtileSelectedTags" runat="server"></asp:Label>
        </div>
        <div class="fieldrow" id="DVassociatedEntry" runat="server" visible="false">
            <asp:Label ID="LBtileAssociatedEntry_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBtileAssociatedEntry">*Entry:</asp:Label>
            <asp:Label ID="LBtileAssociatedEntry" runat="server" CssClass="inputtext"></asp:Label>
        </div>
    </div>
    <div class="tabs">
        <ul>
            <asp:Repeater ID="RPTlanguages" runat="server">
                <ItemTemplate><li><a href="#tabs-<%#Container.DataItem.ShortCode%>"><%#Container.DataItem.ShortCode.ToUpper()%></a></li></ItemTemplate>
            </asp:Repeater>
        </ul>
        <asp:Repeater ID="RPTtranslations" runat="server">
            <ItemTemplate>
                 <div id="tabs-<%#Container.DataItem.LanguageInfo.ShortCode%>">
                    <div class="fieldobject">
                        <div class="fieldrow haslang">
                            <label class="fieldlabel wrap" for="">
                                <asp:Label ID="LBtileTranslationName_t" runat="server" CssClass="left">*Name</asp:Label>
                                <span class="templatelanguage right" title="international"><%#Container.DataItem.LanguageInfo.ShortCode.ToUpper()%></span></label>
                            <asp:TextBox ID="TXBname" runat="server" CssClass="inputtext" Text="<%#Container.DataItem.Title()%>" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="fieldrow haslang">
                            <label class="fieldlabel wrap" for="">
                                <asp:Label ID="LBtileTranslationDescription_t" runat="server" CssClass="left">*Description</asp:Label>
                                <span class="templatelanguage right" title="international"><%#Container.DataItem.LanguageInfo.ShortCode.ToUpper()%></span></label>
                            <asp:TextBox ID="TXBdescription" runat="server" TextMode="MultiLine" CssClass="textarea big" ReadOnly="true" Text="<%#Container.DataItem.Description()%>"></asp:TextBox>
                        </div>
                    </div>
                    <div class="fieldobject actions" id="DVactions" runat="server" visible="false">
                        <div class="fieldrow title">
                            <asp:Label ID="LBtileTranslationActions" runat="server" CssClass="title">*Actions</asp:Label>
                        </div>
                        <div class="fieldrow haslang new hasicons">
                            <label class="fieldlabel wrap" for="">
                                <span class="left">
                                    <span class="icons"><span class="icon new"></span></span>
                                     <asp:Label ID="LBtileActionNew" runat="server" CssClass="text">*New</asp:Label>
                                </span>
                                <span class="templatelanguage right" title="international"><%#Container.DataItem.LanguageInfo.ShortCode.ToUpper()%></span></label>
                            <asp:TextBox ID="TXBnew" runat="server" CssClass="inputtext" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="fieldrow haslang stats hasicons">
                            <label class="fieldlabel wrap" for="">
                                <span class="left">
                                    <span class="icons"><span class="icon stats"></span></span>
                                    <asp:Label ID="LBtileActionStat" runat="server" CssClass="text">*Stat</asp:Label>
                                </span>
                                <span class="templatelanguage right" title="international"><%#Container.DataItem.LanguageInfo.ShortCode.ToUpper()%></span></label>
                             <asp:TextBox ID="TXBstat" runat="server" CssClass="inputtext" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="fieldrow haslang settings hasicons">
                            <label class="fieldlabel wrap" for="">
                                <span class="left">
                                    <span class="icons"><span class="icon settings"></span></span>
                                    <asp:Label ID="LBtileActionSettings" runat="server" CssClass="text">*Settings</asp:Label>
                                </span>
                                <span class="templatelanguage right" title="international"><%#Container.DataItem.LanguageInfo.ShortCode.ToUpper()%></span></label>
                            <asp:TextBox ID="TXBsettings" runat="server" CssClass="inputtext" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="fieldobject gallerywrapper box cssclass" runat="server" id="DVcssClass">
        <div class="fieldrow objectheader">
            <h4 class="title"><asp:Label ID="LBtileCssImages" runat="server" CssClass="title">*CSS Images</asp:Label></h4>
        </div>
        <div class="fieldrow">
            <label class="fieldlabel wrap" for="">
                <asp:Label ID="LBtileCssClass_t" runat="server" CssClass="left">*Icon Class:</asp:Label>
            </label>
            <div class="inlinewrapper">
                <div class="gallery clearfix small onlyone container_12">
                    <div class="iconcontainer grid_2">
                        <div class="icon comtype_64 <%=TileCssClass%>" ></div>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <asp:TextBox ID="TXBtileCssClass" runat="server" CssClass="inputtext galleryvalue" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="fieldobject gallerywrapper box img"  runat="server" id="DVimage">
        <div class="fieldrow objectheader">
            <h4 class="title"><asp:Label ID="LBtileUploadedImages" runat="server">*Images</asp:Label></h4>
        </div>
        <div class="fieldrow">
            <div class="inlinewrapper">
                <div class="gallery clearfix small onlyone container_12">
                    <div class="iconcontainer grid_2">
                        <img class="img" src="<%=BaseUrl & TilesVirtualPath & TileImage%>" alt=""/>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>
    </div>
    <div class="fieldobject box settings actions" id="DVactions" runat="server" visible="false">
        <div class="fieldrow objectheader">
            <h4 class="title"><asp:Literal ID="LTeditTileTitleActions" runat="server">*Actions</asp:Literal></h4>
        </div>
        <div class="fieldobject action new">
            <div class="fieldrow">
                <asp:Label ID="LBactionNew_t" runat="server" CssClass="title">*Action - New</asp:Label>
            </div>
            <div class="fieldrow">
                <asp:Label ID="LBactionNewPageSelector_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBactionNewPageSelector">*Url:</asp:Label>
                <asp:Label ID="LBactionNewPageSelector" runat="server" CssClass="inputtext"></asp:Label>
            </div>
            <div class="fieldrow permissions">
                <asp:Label ID="LBactionNewPermissions_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBLactionNewPermissions">*Permissions</asp:Label>
                <div class="inlinewrapper">
                    <asp:CheckBoxList ID="CBLactionNewPermissions" runat="server">

                    </asp:CheckBoxList>
                   <%-- <span id="" class="inputtext checkboxlist">
                        <input id="CBLpermission_0" type="checkbox" name="CBLpermission0" checked="checked" value="1"><label for="CBLpermission_0">Permission</label>
                        <input id="CBLpermission_2" type="checkbox" name="CBLpermission2" checked="checked" value="8"><label for="CBLpermission_2">Permission</label>
                        <input id="CBLpermission_3" type="checkbox" name="CBLpermission3" checked="checked" value="3"><label for="CBLpermission_3">Permission</label>
                        <input id="CBLpermission_4" type="checkbox" name="CBLpermission4" checked="checked" value="31"><label for="CBLpermission_4">Permission</label>
                        <input id="CBLpermission_1" type="checkbox" name="CBLpermission1" checked="checked" value="4"><label for="CBLpermission_1">Permission</label>
                    </span>--%>
                </div>
            </div>
        </div>
        <div class="fieldobject action stats">
            <div class="fieldrow">
                <asp:Label ID="LBactionStat_t" runat="server" CssClass="title">*Action - Stats</asp:Label>
            </div>
            <div class="fieldrow">
                <asp:Label ID="LBactionStatPageSelector_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBactionStatPageSelector">*Url:</asp:Label>
                <asp:Label ID="LBactionStatPageSelector" runat="server" CssClass="inputtext"></asp:Label>
            </div>
            <div class="fieldrow permissions">
                <asp:Label ID="LBactionStatPagePermissions_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBLactionStatPermissions">*Permissions</asp:Label>
                <div class="inlinewrapper">
                    <asp:CheckBoxList ID="CBLactionStatPermissions" runat="server">

                    </asp:CheckBoxList>
                   <%-- <span id="" class="inputtext checkboxlist">
                        <input id="CBLpermission_0" type="checkbox" name="CBLpermission0" checked="checked" value="1"><label for="CBLpermission_0">Permission</label>
                        <input id="CBLpermission_2" type="checkbox" name="CBLpermission2" checked="checked" value="8"><label for="CBLpermission_2">Permission</label>
                        <input id="CBLpermission_3" type="checkbox" name="CBLpermission3" checked="checked" value="3"><label for="CBLpermission_3">Permission</label>
                        <input id="CBLpermission_4" type="checkbox" name="CBLpermission4" checked="checked" value="31"><label for="CBLpermission_4">Permission</label>
                        <input id="CBLpermission_1" type="checkbox" name="CBLpermission1" checked="checked" value="4"><label for="CBLpermission_1">Permission</label>
                    </span>--%>
                </div>
            </div>
        </div>
        <div class="fieldobject action settings">
            <div class="fieldrow">
                <asp:Label ID="LBactionSettings_t" runat="server" CssClass="title">*Action - Settings</asp:Label>
            </div>
            <div class="fieldrow">
                <asp:Label ID="LBactionSettingsPageSelector_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBactionSettingsPageSelector">*Url:</asp:Label>
                <asp:Label ID="LBactionSettingsPageSelector" runat="server" CssClass="inputtext"></asp:Label>
            </div>
            <div class="fieldrow permissions">
                <asp:Label ID="LBactionSettingsPagePermissions_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBLactionSettingsPermissions">*Permissions</asp:Label>
                <div class="inlinewrapper">
                    <asp:CheckBoxList ID="CBLactionSettingsPermissions" runat="server">

                    </asp:CheckBoxList>
                   <%-- <span id="" class="inputtext checkboxlist">
                        <input id="CBLpermission_0" type="checkbox" name="CBLpermission0" checked="checked" value="1"><label for="CBLpermission_0">Permission</label>
                        <input id="CBLpermission_2" type="checkbox" name="CBLpermission2" checked="checked" value="8"><label for="CBLpermission_2">Permission</label>
                        <input id="CBLpermission_3" type="checkbox" name="CBLpermission3" checked="checked" value="3"><label for="CBLpermission_3">Permission</label>
                        <input id="CBLpermission_4" type="checkbox" name="CBLpermission4" checked="checked" value="31"><label for="CBLpermission_4">Permission</label>
                        <input id="CBLpermission_1" type="checkbox" name="CBLpermission1" checked="checked" value="4"><label for="CBLpermission_1">Permission</label>
                    </span>--%>
                </div>
            </div>
        </div>
    </div>
</div>
<asp:Literal ID="LTcssClassGeneric" runat="server" Visible="false">generic</asp:Literal>