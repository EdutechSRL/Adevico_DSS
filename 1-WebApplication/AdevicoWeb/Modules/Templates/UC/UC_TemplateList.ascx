<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TemplateList.ascx.vb" Inherits="Comunita_OnLine.UC_TemplatesList" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<asp:MultiView ID="MLVlist" runat="server">
    <asp:View ID="VIWempty" runat="server">
    
    </asp:View>
    <asp:View ID="VIWlist" runat="server">
        <div class="contenttop">
            <div class="fieldobject title clearfix" id="DVtitle" runat="server" visible="false">
                <div class="fieldrow inline left">
                    <label for=""><asp:Literal ID="LTtitle" runat="server"></asp:Literal></label>
                </div>
                <div class="fieldrow inline right">
                    <div class="ddbuttonlist enabled" id="DVaddButtons" runat="server"><!--
                        --><asp:HyperLink ID="HYPaddTemplate" runat="server" Text="*Add template" CssClass="linkMenu" Visible="false"></asp:HyperLink><!--
                        --><asp:HyperLink ID="HYPaddObjectTemplate" runat="server" Text="*Add template" CssClass="linkMenu" Visible="false"></asp:HyperLink><!--
                        --><asp:HyperLink ID="HYPaddPersonalTemplate" runat="server" Text="*Add personal template" CssClass="linkMenu" Visible="false"></asp:HyperLink><!--
                    --></div>
                </div>
            </div>
            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
            <div class="fieldobject filters">
                <div class="fieldrow filter">
                    <asp:Label ID="LBtemplateNameFilter_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBtemplateName">*Name:</asp:Label>
                    <asp:TextBox ID="TXBtemplateName" runat="server" CssClass="fieldinput"></asp:TextBox>
                    <asp:Button ID="BTNfilterTemplates" CssClass="" runat="server" />
                </div>
                <div class="fieldrow filter" id="DVdisplayType" runat="server">
                    <asp:Label ID="LBtemplateDisplayFilter_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RBLdisplayType">*Show:</asp:Label>
                    <asp:RadioButtonList ID="RBLdisplayType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true"></asp:RadioButtonList>    
                </div>
            </div>
        </div>
        <asp:Repeater ID="RPTtemplates" runat="server">
            <HeaderTemplate>
                <table class="table light fullwidth treetable templatelist">
                    <thead>
                        <tr>
                            <th class="templatename">
                                <asp:Literal ID="LTtemplateName_t" runat="server">*Name</asp:Literal>
                                <asp:LinkButton ID="LNBorderByNameUp" runat="server" cssclass="icon orderUp" CommandArgument="ByName.True" CommandName="orderby"></asp:LinkButton>
                                <asp:LinkButton ID="LNBorderByNameDown" runat="server" cssclass="icon orderDown" CommandArgument="ByName.False" CommandName="orderby"></asp:LinkButton>
                            </th>
                            <th class="templatetype" runat="server" id="THtemplatetype">
                                <asp:Literal ID="LTtemplateType_t" runat="server">*Type</asp:Literal>
                                <asp:LinkButton ID="LNBorderByTypeUp" runat="server" cssclass="icon orderUp" CommandArgument="ByType.True" CommandName="orderby"></asp:LinkButton>
                                <asp:LinkButton ID="LNBorderByTypeDown" runat="server" cssclass="icon orderDown" CommandArgument="ByType.False" CommandName="orderby"></asp:LinkButton>
                            </th>
                            <th class="templateservice" runat="server" id="THmoduleName" visible="false"><asp:Literal ID="LTmoduleName_t" runat="server">*Module Name</asp:Literal></th>
                            <th class="createdby">
                                <asp:Literal ID="LTcreatedBy_t" runat="server">*Created by</asp:Literal>
                                <asp:LinkButton ID="LNBorderByUserUp" runat="server" cssclass="icon orderUp" CommandArgument="ByUser.True" CommandName="orderby"></asp:LinkButton>
                                <asp:LinkButton ID="LNBorderByUserDown" runat="server" cssclass="icon orderDown" CommandArgument="ByUser.False" CommandName="orderby"></asp:LinkButton>
                            </th>
                            <th class="createdon">
                                <asp:Literal ID="LTlastEditOn_t" runat="server">*Created On</asp:Literal>
                                <asp:LinkButton ID="LNBorderByDateUp" runat="server" cssclass="icon orderUp" CommandArgument="ByDate.True" CommandName="orderby"></asp:LinkButton>
                                <asp:LinkButton ID="LNBorderByDateDown" runat="server" cssclass="icon orderDown" CommandArgument="ByDate.False" CommandName="orderby"></asp:LinkButton>
                            </th>
                            <th class="actions"><asp:Literal ID="LTactions_t" runat="server">*Actions</asp:Literal></th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                        <tr id="tmpl-<%#Container.DataItem.Id %>" class="template <%#GetOpenCssClass(Container.DataItem.Id) %>">
                            <td class="templatename">
                                <a name="tmp_<%#Container.DataItem.Id %>"></a>
                                <asp:literal ID="LTtemplateName" runat="server" Text='<%#Container.DataItem.Name%>'></asp:literal>
                            </td>
                            <td class="templatetype" runat="server" id="TDtemplatetype">
                                <asp:literal ID="LTtemplateType" runat="server"></asp:literal>
                            </td>
                            <td class="templateservice" runat="server" id="TDmoduleName" visible="false">
                                <asp:literal ID="LTmoduleName" runat="server"></asp:literal>
                            </td>
                            <td class="createdby">
                                <asp:literal ID="LTlastEditBy" runat="server"></asp:literal>
                            </td>
                            <td class="createdon">
                                 <asp:Label ID="LBlastEditOn" runat="server"></asp:Label>
                            </td>
                            <td class="actions">
                                <asp:literal ID="LTemptyActions" runat="server" Text=" "/>
                                <span class="icons">                                            
                                    <asp:LinkButton ID="LNBtemplateVirtualDelete" runat="server" Visible="false" CssClass="icon virtualdelete needconfirm" CommandName="virtualdelete" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                    <asp:LinkButton ID="LNBtemplateDelete" runat="server" Visible="false" CssClass="icon delete needconfirm" CommandName="delete" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                    <asp:LinkButton ID="LNBtemplateRecover" runat="server" Visible="false" CssClass="icon recover" CommandName="recover" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                    <asp:LinkButton ID="LNBtemplateNewVersion" runat="server" Visible="false" CssClass="icon addversion" CommandName="new" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                    <asp:LinkButton ID="LNBtemplateclone" runat="server" Visible="false" CssClass="icon copy" CommandName="clone" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                    <asp:HyperLink ID="HYPtemplateEditPermissions" runat="server" CssClass="icon editpermissions" Visible="false"></asp:HyperLink>
                                    <asp:HyperLink ID="HYPtemplateEdit" runat="server" CssClass="icon edit" Visible="false"></asp:HyperLink>
                                    <asp:HyperLink ID="HYPtemplatePreview" runat="server" CssClass="icon view" Visible="false"></asp:HyperLink>
                                    <asp:LinkButton ID="LNBsendMail" runat="server" Visible="false" CssClass="icon mail" CommandName="sendmail" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                </span>
                            </td>
                        </tr>
                        <asp:Repeater ID="RPTrevisions" runat="server" DataSource="<%#Container.DataItem.Versions%>" OnItemDataBound="RPTrevisions_ItemDataBound" OnItemCommand="RPTrevisions_ItemCommand">
                            <ItemTemplate>
                                <tr id="tmpl-<%#Container.DataItem.IdTemplate %>-vrs-<%#Container.DataItem.Id %>" class="template version child-of-tmpl-<%#Container.DataItem.IdTemplate %> <%#GetRevisionCssClass(Container.DataItem.DisplayAs) %>">
                                    <td class="templatename">
                                        <a name="ver_<%#Container.DataItem.Id %>"></a>
                                         <asp:literal ID="LTrevisionName" runat="server"></asp:literal>
                                         <asp:literal ID="LTtemplateName" runat="server" Text='<%#Container.DataItem.Template.Name%>' Visible="false"></asp:literal>
                                    </td>
                                    <td class="templatetype" runat="server" id="TDtemplatetype">
                                        &nbsp;
                                    </td>
                                    <td class="templateservice" runat="server" id="TDmoduleName" visible="false">
                                        <asp:literal ID="LTmoduleName" runat="server"></asp:literal>
                                    </td>
                                    <td class="createdby">
                                        <asp:literal ID="LTdisplayName" runat="server" Text='<%#Container.DataItem.UserDisplayName%>' ></asp:literal>
                                    </td>
                                    <td class="createdon">
                                        <asp:Label ID="LBlastEditOn" runat="server"></asp:Label>
                                    </td>
                                    <td class="actions">
                                        <asp:literal ID="LTemptyActions" runat="server" Text=" "/>
                                        <span class="icons">                                            
                                            <asp:LinkButton ID="LNBversionDelete" runat="server" Visible="false" CssClass="icon delete needconfirm" CommandName="delete" CommandArgument='<%#Container.DataItem.Id & "," & Container.DataItem.IdTemplate %>'>&nbsp;</asp:LinkButton>
                                            <asp:LinkButton ID="LNBversionVirtualDelete" runat="server" Visible="false" CssClass="icon virtualdelete needconfirm" CommandName="virtualdelete" CommandArgument='<%#Container.DataItem.Id & "," & Container.DataItem.IdTemplate %>'>&nbsp;</asp:LinkButton>
                                            <asp:LinkButton ID="LNBversionRecover" runat="server" Visible="false" CssClass="icon recover" CommandName="recover" CommandArgument='<%#Container.DataItem.Id & "," & Container.DataItem.IdTemplate %>'>&nbsp;</asp:LinkButton>
                                            <asp:HyperLink ID="HYPversionEdit" runat="server" CssClass="icon edit" Visible="false"></asp:HyperLink>
                                            <asp:LinkButton ID="LNBversionClone" runat="server" Visible="false" CssClass="icon copy" CommandName="clone" CommandArgument='<%#Container.DataItem.Id & "," & Container.DataItem.IdTemplate %>'>&nbsp;</asp:LinkButton>
                                            <asp:LinkButton ID="LNBversionEditPermissions" runat="server" Visible="false" CssClass="icon editpermissions" CommandName="editpermissions" CommandArgument='<%#Container.DataItem.Id & "," & Container.DataItem.IdTemplate %>'>&nbsp;</asp:LinkButton>
                                            <asp:HyperLink ID="HYPversionEditPermissions" runat="server" CssClass="icon editpermissions" Visible="false"></asp:HyperLink>
                                            
                                            <asp:HyperLink ID="HYPversionPreview" runat="server" CssClass="icon view" Visible="false"></asp:HyperLink>
                                            <asp:LinkButton ID="LNBsendMail" runat="server" Visible="false" CssClass="icon mail" CommandName="sendmail" CommandArgument='<%#Container.DataItem.Id & "," & Container.DataItem.IdTemplate %>'>&nbsp;</asp:LinkButton>
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
            </ItemTemplate>
            <FooterTemplate>
                        <tr id="TRempty" runat="server" visible="false">
                            <td id="TDemptyItems" runat="server" colspan="5">
                                <asp:Label ID="LBnoTemplates" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <div class="pager" runat="server" id="DVpagerBottom" visible="false">
            <asp:literal ID="LTpageBottom" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
        </div>   
    </asp:View>
</asp:MultiView>