<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ItemDetails.ascx.vb" Inherits="Comunita_OnLine.UC_ItemDetails" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsectionOpener" Src="~/Modules/Common/UC/UC_ExpandAndCollapse.ascx" %>
<div class="section grid_12" id="DVmain" runat="server">
    <div class="sectioninner">
        <div class="cisectionheader clearfix">
            <div class="left">
                <h3><asp:Literal ID="LTinformationSection" runat="server">*Information</asp:Literal></h3>
                <a name="#action_edit" href="#"></a><a name="action_details" href="#"></a>
            </div>
        </div>
        <div class="cisectioncontent">
            <div class="tablewrapper">
                <table class="table minimal fullwidth filedetails">
                    <tbody>
                        <tr id="TRitemName" runat="server">
                            <td class="key"><asp:Literal ID="LTitemName_t" runat="server">*Item name</asp:Literal></td>
                            <td class="value">
                                <asp:Literal ID="LTitemName" runat="server"></asp:Literal>
                                <asp:TextBox ID="TXBitemName" runat="server" Visible="false" MaxLength="4000"></asp:TextBox>
                                <asp:Label ID="LBitemExtension" runat="server" Visible="false"></asp:Label>
                                <asp:RequiredFieldValidator ID="RFVitemName" Visible="false" runat="server" ControlToValidate="TXBitemName" Display="Dynamic" ErrorMessage="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr id="TRurl" runat="server" visible="false">
                            <td class="key"><asp:Literal ID="LTitemUrl_t" runat="server">*Url</asp:Literal></td>
                            <td class="value">
                                <asp:HyperLink ID="HYPurl" runat="server" Target="_blank"></asp:HyperLink>
                                <asp:TextBox ID="TXBitemUrl" runat="server" Visible="false" MaxLength="2000"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVitemUrl" Visible="false" runat="server" ControlToValidate="TXBitemUrl" Display="Dynamic" ErrorMessage="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr id="TRurlName" runat="server" visible="false">
                            <td class="key"><asp:Literal ID="LTitemUrlName_t" runat="server">*Url</asp:Literal></td>
                            <td class="value">
                                <asp:Literal ID="LTitemUrlName" runat="server"></asp:Literal>
                                <asp:TextBox ID="TXBitemUrlName" runat="server" Visible="false" MaxLength="2000"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="TRpath" runat="server" visible="false">
                            <td class="key"><asp:Literal ID="LTitemPathTitle" runat="server" Text="*Original path"></asp:Literal></td>
                            <td class="value"><asp:Literal ID="LTitemPath" runat="server" ></asp:Literal></td>
                        </tr>
                        <tr>
                            <td class="key"><asp:Literal ID="LTitemTypeTitle" runat="server" Text="*File Type"></asp:Literal></td>
                            <td class="value"><asp:Literal ID="LTitemTypeValue" runat="server"></asp:Literal></td>
                        </tr>
                        <tr id="TRdisplayMode" runat="server">
                            <td class="key"><asp:Literal ID="LTdisplayModeTitle" runat="server" Text="*Display mode:"></asp:Literal></td>
                            <td class="value">
                                <asp:Radiobuttonlist ID="RBLdisplayMode" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="inputgroup"></asp:Radiobuttonlist>
                            </td>
                        </tr>
                        <tr id="TRstatus" runat="server" visible="false">
                            <td class="key"><asp:Literal ID="LTitemStatusTitle" runat="server" Text="*Status"></asp:Literal></td>
                            <td class="value"><asp:Literal ID="LTitemStatusValue" runat="server"></asp:Literal></td>
                        </tr>
                        <tr id="TRvisibility" runat="server" visible="false">
                            <td class="key"><asp:Literal ID="LTitemVisibilityTitle" runat="server">*Visibility:</asp:Literal></td>
                            <td class="value"><asp:CheckBox id="CBXvisible" runat="server" Visible="false" CssClass="inputgroup"  /><asp:Literal ID="LTitemVisibilityValue" runat="server"></asp:Literal></td>
                        </tr>
                        <tr id="TRallowUpload" runat="server" visible="false">
                            <td class="key"><asp:Literal ID="LTitemAllowUploadTitle" runat="server" Text="*Allow upload"></asp:Literal></td>
                            <td class="value"><asp:Literal ID="LTitemAllowUploadValue" runat="server"></asp:Literal><asp:CheckBox id="CBXallowUpload" runat="server" Visible="false" CssClass="inputgroup"  /></td>
                        </tr>
                        <tr>
                            <td class="key"><asp:Literal ID="LTitemAuthorTitle" runat="server" Text="*Author"></asp:Literal></td>
                            <td class="value"><asp:Literal ID="LTitemAuthorValue" runat="server"></asp:Literal></td>
                        </tr>
                        <tr id="TRitemSize" runat="server" visible="false">
                            <td class="key"><asp:Literal ID="LTitemSizeTitle" runat="server" Text="*Size"></asp:Literal></td>
                            <td class="value"><asp:Literal ID="LTitemSizeValue" runat="server"></asp:Literal></td>
                        </tr>
                        <tr id="TRitemCreatedOn" runat="server">
                            <td class="key"><asp:Literal ID="LTitemCreatedOnTitle" runat="server" Text="*Created On"></asp:Literal></td>
                            <td class="value"><asp:Literal ID="LTitemCreatedOnValue" runat="server"></asp:Literal></td>
                        </tr>
                        <tr id="TRitemUpdatedOn" runat="server" visible="false">
                            <td class="key"><asp:Literal ID="LTitemUpdatedOnTitle" runat="server" Text="*Updated On"></asp:Literal></td>
                            <td class="value"><asp:Literal ID="LTitemUpdatedOnValue" runat="server"></asp:Literal></td>
                        </tr>
                        <tr id="TRmyDownloads" runat="server" visible="false">
                            <td class="key"><asp:Literal ID="LTitemMyDownloadsTitle" runat="server" Text="*My Downloads"></asp:Literal></td>
                            <td class="value"><asp:Literal ID="LTitemMyDownloadsValue" runat="server" Text="*My Downloads"/></td>
                        </tr>
                        <tr id="TRdownloads" runat="server" visible="false">
                            <td class="key"><asp:Literal ID="LTitemDownloadsTitle" runat="server" Text="*Downloads"></asp:Literal></td>
                            <td class="value"><asp:Literal ID="LTitemDownloadsValue" runat="server" Text="*Downloads"></asp:Literal></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="cisectionfooter">
            <div class="tags fieldobject" id="DVeditTags" runat="server" visible="false">
                <div class="fieldrow token">
                    <asp:Label ID="LBitemEditTagsTitle" CssClass="fieldlabel" runat="server">*Tags:</asp:Label>
                    <asp:TextBox ID="TXBtags" runat="server" CssClass="tokeninputtag"></asp:TextBox>
                </div>
            </div>
            <div class="extrainfo" id="DVtag" runat="server">
                <asp:Label ID="LBitemTagsTitle" CssClass="label" runat="server">*Tags:</asp:Label>
                <span class="tags">
                    <asp:Label ID="LBtagItemType" runat="server" CssClass="tag type" Visible="false"></asp:Label>
                <asp:Repeater ID="RPTtags" runat="server">
                    <ItemTemplate>
                        <span class="tag"><%#Container.DataItem %></span>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        <span class="sep">|</span>
                    </SeparatorTemplate>
                </asp:Repeater>
                </span>
            </div>
            <span class="commands light">
                <asp:HyperLink ID="HYPdownloadItem" runat="server" CssClass="commandlight" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPplayItem" runat="server" CssClass="commandlight" Visible="false"></asp:HyperLink>
            </span>
        </div>
    </div>
</div>
<div class="section box grid_3" id="DVpreview" runat="server">
    <div class="sectioninner" >
        <div class="cisectionheader clearfix">
            <div class="left">
                <h3><asp:Literal ID="LTthumbnailSection" runat="server">*Anteprima</asp:Literal></h3>
            </div>
            <div class="right">
                &nbsp;
            </div>
        </div>
        <div class="cisectioncontent">
            <div class="thumbnail">
                <div class="img">
                    <asp:HyperLink ID="HYPthumbnail" runat="server" CssClass="image">
                        <img src="#url#" />
                    </asp:HyperLink>
                    <asp:Label ID="LBemptyPreview" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <div class="cisectionfooter">

        </div>
    </div>
</div>
<div class="section box collapsable expanded grid_12" id="DVdescriptionSection" runat="server">
    <div class="sectioninner">
        <div class="cisectionheader clearfix">
            <div class="left">
                <h3>
                    <asp:Literal ID="LTdescriptionSection" runat="server">*Description</asp:Literal>
                    <CTRL:CTRLsectionOpener ID="CTRLopenDescription" runat="server" />
                </h3>
            </div>
            <div class="right hideme"></div>
        </div>
        <div class="cisectioncontent">
            <div id="DVdescription" runat="server">
                <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
            </div>
            <textarea id="TXAdescription" runat="server" class="description" Visible="false"></textarea>
        </div>
        <div class="cisectionfooter"></div>
    </div>
</div>
<asp:Literal ID="LTtemplateFile" runat="server"  Visible="false"><span class="iteminfo"><span class="name"><span class="actionbuttons"><span class="#ico#"></span></span><span class="text">#name#</span></span></span></asp:Literal><asp:Literal ID="LTitemExtensionCssClass" runat="server" Visible="false">fileIco ext</asp:Literal><asp:Literal ID="LTitemFolderCssClass" runat="server" Visible="false">fileIco folder</asp:Literal><asp:Literal ID="LTitemUrlCssClass" runat="server" Visible="false">fileIco extlink</asp:Literal><asp:Literal ID="LTitemScormPackageCssClass" runat="server" Visible="false">fileIco scorm</asp:Literal><asp:Literal ID="LTitemMultimediaCssClass" runat="server" Visible="false">fileIco multimedia</asp:Literal><asp:Literal ID="LTmodalCssClass" runat="server" Visible="false">openmodal</asp:Literal>