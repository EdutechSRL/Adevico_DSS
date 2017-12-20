<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AssignedUser.ascx.vb"
    Inherits="Comunita_OnLine.UC_AssignedUser" EnableTheming="true" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../UC/UC_PagerControl.ascx" %>
<div id="DIVtitleResource" runat="server" style="width: 900px; text-align: left;"
    class="RigaTitolo" align="center">
    <div style="float: left; width: 90%;">
        <asp:Label ID="LBtitolo" runat="server" Text="*Resource"></asp:Label>
    </div>
    <div style="float: right;">
        <asp:HyperLink ID="HYPaddResource" runat="server" Text="*Add" CssClass="Link_Menu"></asp:HyperLink>
    </div>
</div>
<br />
<div style="width: 70%;">
    <asp:GridView ID="GDVassignedPersons" AutoGenerateColumns="false" runat="server"
        HorizontalAlign="Left" SkinID="griglia60pc" UseAccessibleHeader="true" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="*E" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                <ItemTemplate>
                    <asp:LinkButton ID="LNBvirtualDelete" runat="server" CommandName="virtualdelete"
                        CausesValidation="false"></asp:LinkButton>
                    <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false"
                        Visible="false"></asp:LinkButton>
                    <asp:LinkButton ID="LNBdelete" runat="server" CausesValidation="false" CommandName="confirmDelete"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="20px">
                <ItemTemplate>
                    <input id="CBselectUser" runat="server" type="checkbox" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="*Assigned Person">
                <ItemTemplate>
                    <asp:Literal ID="LTassignedPerson" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="*Role">
                <ItemTemplate>
                    <asp:Literal ID="LTrole" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="*Completeness">
                <ItemTemplate>
                    <div style="width: 165px; padding-left:5px">
                        <div style="float: left; width: 55px; text-align: left; ">
                            <asp:Literal ID="LTcompleteness" runat="server" EnableViewState="false" Text=""></asp:Literal>
                        </div>
                        <div id="DIVcompleteness" runat="server" >
                            <div style="float: left; width: 100px; text-align: left; width: 100px; height: 15px;
                                border: 1px solid black">
                                <asp:Image ID="IMcompleteness" runat="server" BackColor="Green" Height="15px" />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle CssClass="ROW_header_Small_Center" />
        <RowStyle CssClass="ROW_Normal_Small" />
        <AlternatingRowStyle CssClass="ROW_Alternate_Small" />
    </asp:GridView>
    <div style="text-align: right; padding-top: 5px; clear: both; height: 22px; position: relative;">
        <CTRL:GridPager ID="PGgrid" runat="server" EnableQueryString="false"></CTRL:GridPager>
    </div>
</div>
