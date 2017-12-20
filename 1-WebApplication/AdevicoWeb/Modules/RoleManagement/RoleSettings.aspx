<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" CodeBehind="RoleSettings.aspx.vb" Inherits="Comunita_OnLine.RolePermissionManagement" %>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" language="javascript">

        function SelezioneRiga(elementi) {
            var HIDcheckbox;
            var totale, selezionati;
            HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>');
            totale = 0
            selezionati = 0
            deselezionati = 0
            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];
                /*  if (e.type == 'checkbox')
                alert(e.name);*/
                if (e.type == 'checkbox' && elementi.indexOf(',' + e.value + ',') != -1) {
                    totale = totale + 1
                    if (e.checked == true)
                        selezionati = selezionati + 1
                    else
                        deselezionati = deselezionati + 1
                }
            }
            if (totale > 0 && totale == selezionati) {
                for (i = 0; i < document.forms[0].length; i++) {
                    e = document.forms[0].elements[i];
                    if (e.type == 'checkbox' && elementi.indexOf(',' + e.value + ',') != -1) {
                        e.checked = false;
                        HIDcheckbox.value = HIDcheckbox.value.replace(',' + e.value + ',', ',')
                    }
                }
            }
            else {
                for (i = 0; i < document.forms[0].length; i++) {
                    e = document.forms[0].elements[i];
                     if (e.type == 'checkbox' && elementi.indexOf(',' + e.value + ',') != -1) {
                        e.checked = true;
                        if (HIDcheckbox.value == "")
                            HIDcheckbox.value = ',' + e.value + ','
                        else {
                            pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
                            if (pos1 == -1)
                                HIDcheckbox.value = HIDcheckbox.value + e.value + ','
                        }
                    }
                }
            }

            if (HIDcheckbox.value == ",")
                HIDcheckbox.value = "";
           return false
        }

        function SelezioneColonna(NomeColonna) {
            var HIDcheckbox;
            var totale, selezionati;
            //eval('HIDcheckbox= this.document.forms[0].<%=Me.HIDcheckbox.clientID%>');
            HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>');
            totale = 0
            selezionati = 0
            deselezionati = 0

            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];
                if (e.type == 'checkbox' && e.value.indexOf(NomeColonna + '_') != -1){
                    totale = totale + 1
                    if (e.checked == true)
                        selezionati = selezionati + 1
                    else
                        deselezionati = deselezionati + 1
                }
            }
            if (totale > 0 && totale == selezionati) {
                for (i = 0; i < document.forms[0].length; i++) {
                    e = document.forms[0].elements[i];
                    if (e.type == 'checkbox' && e.value.indexOf(NomeColonna + '_') != -1) {
                        e.checked = false;
                        HIDcheckbox.value = HIDcheckbox.value.replace(',' + e.value + ',', ',')
                    }
                }
            }
            else {
                for (i = 0; i < document.forms[0].length; i++) {
                    e = document.forms[0].elements[i];
                    if (e.type == 'checkbox' && e.value.indexOf(NomeColonna + '_') != -1) {
                        e.checked = true;
                        if (HIDcheckbox.value == "")
                            HIDcheckbox.value = ',' + e.value + ','
                        else {
                            pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
                            if (pos1 == -1)
                                HIDcheckbox.value = HIDcheckbox.value + e.value + ','
                        }
                    }
                }
            }

            if (HIDcheckbox.value == ",")
                HIDcheckbox.value = "";
            return false
        }



        function SelectFromNameAndAssocia(value) {
            var HIDcheckbox;
            //eval('HIDcheckbox= this.document.forms[0].<%=Me.HIDcheckbox.clientID%>');
            HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>');
            for (i = 0; i < document.forms[0].length; i++) {
                e = document.forms[0].elements[i];

                if (e.type == 'checkbox' && e.value== value) {//"CBXassocia"
                    if (e.checked == true) {
                        if (HIDcheckbox.value == "")
                            HIDcheckbox.value = ',' + value + ','
                        else {
                            pos1 = HIDcheckbox.value.indexOf(',' + value + ',')
                            if (pos1 == -1)
                                HIDcheckbox.value = HIDcheckbox.value + value + ','
                        }
                    }
                    else {
                        valore = HIDcheckbox.value;
                        pos1 = HIDcheckbox.value.indexOf(',' + value + ',')
                        if (pos1 != -1) {
                            stringa = ',' + value + ','
                            HIDcheckbox.value = HIDcheckbox.value.substring(0, pos1)
                            HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1 + value.length + 1, valore.length)
                        }
                    }
                }
            }
            if (HIDcheckbox.value == ",")
                HIDcheckbox.value = "";
        }
	
	
	 </script>
     <style type="text/css">
         .FirstCellRow
         {
             text-align:left;
             padding:5px;
             }
         .FirstRow
         {
             text-align:center;
             font-size:x-small;
             }
         .FirstColumn
         {
             background-color:Aqua;
             text-align:left;
             padding:5px;
             font-size:small;
             }
         .ColumnItem
         {
             background-color:#FFFFFF;
             text-align:center;
             font-size:small;
             }
         .ColumnAlternateItem
         {
            background-color:#FCFFE3;
            text-align:center;
            font-size:small;
             }
     </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">

    <div style="text-align:right; padding-top:5px;" >
        <asp:HyperLink ID="HYPbackToManagementBottom" runat="server" CssClass="Link_Menu" Text="Back"
        Height="18px" CausesValidation="false" Visible="false"></asp:HyperLink>
        &nbsp;
        <asp:HyperLink ID="HYPbackToEdit" runat="server" CssClass="Link_Menu" Text="Back to edit"
        Height="18px" CausesValidation="false" Visible="false"></asp:HyperLink>
        &nbsp;
        <asp:Button ID="BTNsave" runat="server" Text="Save" Visible="false"/>
        &nbsp;
        <asp:Button ID="BTNsaveForAll" runat="server" Text="Save for all" />
        &nbsp;
        <asp:Button ID="BTBdefaultValue" runat="server" Text="Default value" />
            &nbsp;
        <asp:Button ID="BTNreplaceCommunityValues" runat="server" Text="Set to all community" ToolTip="Set permission to all community of selected community type" />
    </div>
    <br />
    <div>
        <div style="float:left;">
            <asp:label ID="LBorganizations_t" CssClass="Titolo_campo" runat="server" AssociatedControlID="DDLorganization">Organization:</asp:label>
            <asp:DropDownList ID="DDLorganization" AutoPostBack="true" Runat="server"></asp:DropDownList>
        </div>
        <div style="float:left; padding-left:5px;">
        <asp:label ID="LBmodules_t" CssClass="Titolo_campo" runat="server" AssociatedControlID="DDLmodules">Module:</asp:label>
        <asp:DropDownList ID="DDLmodules" AutoPostBack="true" Runat="server"></asp:DropDownList>
        </div>
    </div>
    <br />
    <div style="clear:both;">
        <asp:Repeater ID="RPTroleOrganizationPermission" runat="server">
            <HeaderTemplate>
            </HeaderTemplate>
            <itemtemplate>
                 <table class="table" border="1" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr class="ROW_header_Small FirstRow">
                            <th class="FirstCellRow">
                                <asp:Label ID="LBcommunityType_t" runat="server">Community</asp:Label>
                            </th>
                            <asp:Repeater ID="RPTpermissionName" runat="server" DataSource="<%#Container.DataItem.Columns%>" OnItemDataBound="RPTpermissionName_ItemDataBound">
                                <ItemTemplate>
                                    <th class="<%#GetBackground(Container.ItemType)%>">
                                       <asp:button ID="BTNpermission" runat="server" CausesValidation="false" Text="<%#Container.Dataitem.Nome %>" />
                                    </th>
                                </ItemTemplate>
                            </asp:Repeater>
                            <th>
                                <asp:Label ID="LBactions" runat="server">A</asp:Label>
                            </th>
                        </tr>        
                    </thead>
                    <tbody>
                    <asp:Repeater ID="RPTtypes" runat="server" DataSource="<%#Container.DataItem.Rows%>" OnItemDataBound="RPTtypes_ItemDataBound">
                        <ItemTemplate>
                        <tr>
                            <td class="FirstColumn">
                                <asp:Literal ID="LTidCommunityType" runat="server" Visible="false" Text="<%#Container.DataItem.IdCommunityType%>" />
                                 <asp:Label ID="LBcommunityTypeName" runat="server" Text="<%#Container.DataItem.CommunityTypeName%>"></asp:Label>
                            </td>
                            <asp:Repeater ID="RPTpermissionValue" runat="server" datasource="<%#Container.DataItem.Positions%>" OnItemDataBound="RPTpermissionValue_ItemDataBound">
                                <ItemTemplate>
                                    <td class="<%#GetBackground(Container.ItemType)%>">
                                        <asp:Literal ID="LTposition" runat="server" Visible="false" Text="<%#Container.DataItem.IdPosition%>"/>
                                        <asp:PlaceHolder ID="PLHpermission" runat="server"></asp:PlaceHolder>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <td>
                                <asp:Literal ID="LTempty" runat="server">&nbsp;</asp:Literal>
                                <asp:button ID="BTNsetAll" runat="server" Text="Sel/Desel"></asp:button>
                            </td>
                    </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                     </tbody>
                </table>
            </itemtemplate>

            <FooterTemplate>
                   
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <input type="hidden" runat="server" id="HIDcheckbox" />
</asp:Content>
