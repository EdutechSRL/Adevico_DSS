<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ConfirmProjectSettings.ascx.vb" Inherits="Comunita_OnLine.UC_ConfirmProjectSettings" %>
<div class="tableview" id="DVselectors" runat="server">
    <div class="fieldobject fielddescription" id="DVdescription" runat="server" visible="false">
        <div class="fieldrow">
            <asp:Label ID="LBdescription" runat="server" CssClass="description"></asp:Label>
        </div>
    </div>
    <div class="fieldobject options" id="DVdateactions" runat="server" visible="false">
        <div class="fieldrow fieldlongtext" >
            <asp:Label ID="LBdateactionsInfo_t" class="" runat="server">*:</asp:Label>
        </div>
        <asp:Repeater ID="RPTdateactions" runat="server" OnItemDataBound="RPTactions_ItemDataBound">
            <ItemTemplate>
                <div class="fieldrow">
                    <input type="radio" id="RDdateactions" name="RDdateactions" value="<%#CInt(Container.DataItem.Id) %>" <%# SetChecked(Container.DataItem)%>/>
                    <label for="RDdateactions"><%#Resource.getValue(Container.DataItem.Translation & Container.DataItem.Id.ToString & ".name")%></label>
                    <asp:Label ID="LBactionDescription" runat="server" cssclass="description"> </asp:Label>
                    <asp:Literal ID="LTaction" runat="server" Visible="false"></asp:Literal>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="fieldobject options" id="DVcpmactions" runat="server" visible="false">
        <div class="fieldrow fieldlongtext" >
            <asp:Label ID="LBcpmactionsInfo_t" class="" runat="server">*Name:</asp:Label>
        </div>
        <asp:Repeater ID="RPTcpmactions" runat="server" OnItemDataBound="RPTactions_ItemDataBound">
            <ItemTemplate>
                <div class="fieldrow">
                    <input type="radio" id="RDcpmactions" name="RDcpmactions" value="<%#CInt(Container.DataItem.Id) %>" <%# SetChecked(Container.DataItem)%> />
                    <label for="RDcpmactions"><%#Resource.getValue(Container.DataItem.Translation & Container.DataItem.Id.ToString & ".name")%></label>
                    <asp:Label ID="LBactionDescription" runat="server" cssclass="description"> </asp:Label>
                    <asp:Literal ID="LTaction" runat="server" Visible="false"></asp:Literal>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="fieldobject options" id="DVmanualactions" runat="server" visible="false">
        <div class="fieldrow fieldlongtext" >
            <asp:Label ID="LBmanualactionsInfo_t" class="" runat="server">*Name:</asp:Label>
        </div>
        <asp:Repeater ID="RPTmanualactions" runat="server" OnItemDataBound="RPTactions_ItemDataBound">
            <ItemTemplate>
                <div class="fieldrow">
                    <input type="radio" id="RDmanualactions" name="RDmanualactions" value="<%#CInt(Container.DataItem.Id) %>" <%# SetChecked(Container.DataItem)%> />
                    <label for="RDmanualactions"><%#Resource.getValue(Container.DataItem.Translation & Container.DataItem.Id.ToString & ".name")%></label>
                    <asp:Label ID="LBactionDescription" runat="server" cssclass="description"> </asp:Label>
                    <asp:Literal ID="LTaction" runat="server" Visible="false"></asp:Literal>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="fieldobject options" id="DVmilestonesactions" runat="server" visible="false">
        <div class="fieldrow fieldlongtext" >
            <asp:Label ID="LBmilestonesactionsInfo_t" class="" runat="server">*Name:</asp:Label>
        </div>
        <asp:Repeater ID="RPTmilestonesactions" runat="server" OnItemDataBound="RPTactions_ItemDataBound">
            <ItemTemplate>
                <div class="fieldrow">
                    <input type="radio" id="RDmilestonesactions" name="RDmilestonesactions" value="<%#CInt(Container.DataItem.Id) %>" <%# SetChecked(Container.DataItem)%> />
                    <label for="RDmilestonesactions"><%#Resource.getValue(Container.DataItem.Translation & Container.DataItem.Id.ToString & ".name")%></label>
                    <asp:Label ID="LBactionDescription" runat="server" cssclass="description"> </asp:Label>
                    <asp:Literal ID="LTaction" runat="server" Visible="false"></asp:Literal>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="fieldobject options" id="DVsummariesactions" runat="server" visible="false">
        <div class="fieldrow fieldlongtext" >
            <asp:Label ID="LBsummariesactionsInfo_t" class="" runat="server">*Name:</asp:Label>
        </div>
        <asp:Repeater ID="RPTsummariesactions" runat="server" OnItemDataBound="RPTactions_ItemDataBound">
            <ItemTemplate>
                <div class="fieldrow">
                    <input type="radio" id="RDsummariesactions" name="RDsummariesactions" value="<%#CInt(Container.DataItem.Id) %>" <%# SetChecked(Container.DataItem)%> />
                    <label for="RDsummariesactions"><%#Resource.getValue(Container.DataItem.Translation & Container.DataItem.Id.ToString & ".name")%></label>
                    <asp:Label ID="LBactionDescription" runat="server" cssclass="description"> </asp:Label>
                    <asp:Literal ID="LTaction" runat="server" Visible="false"></asp:Literal>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="fieldobject options" id="DVestimatedactions" runat="server" visible="false">
        <div class="fieldrow fieldlongtext" >
            <asp:Label ID="LBestimatedactionsInfo_t" class="" runat="server">*Name:</asp:Label>
        </div>
        <asp:Repeater ID="RPTestimatedactions" runat="server" OnItemDataBound="RPTactions_ItemDataBound">
            <ItemTemplate>
                <div class="fieldrow">
                    <input type="radio" id="RDestimatedactions" name="RDestimatedactions" value="<%#CInt(Container.DataItem.Id) %>" <%# SetChecked(Container.DataItem)%> />
                    <label for="RDestimatedactions"><%#Resource.getValue(Container.DataItem.Translation & Container.DataItem.Id.ToString & ".name")%></label>
                    <asp:Label ID="LBactionDescription" runat="server" cssclass="description"> </asp:Label>
                    <asp:Literal ID="LTaction" runat="server" Visible="false"></asp:Literal>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="fieldobject clearfix" id="DVcommands" runat="server">
        <div class="fieldrow buttons right">
            <asp:Button ID="BTNapplySettingsActions" runat="server" CssClass="linkMenu" Visible="false" />
            <asp:Button ID="BTNcancelSettingsActions" runat="server" CssClass="linkMenu" CausesValidation="false" />
        </div>
    </div>
</div>