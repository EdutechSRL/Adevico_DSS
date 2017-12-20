<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleItemFilesSelector.ascx.vb"
    Inherits="Comunita_OnLine.UC_ModuleItemFilesSelector" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:MultiView ID="MLVselector" runat="server" ActiveViewIndex="1">
    <asp:View ID="VIWselector" runat="server">
        <style type="text/css">
            .rtChk
            {
                font-size: 18px;
                /*font-family: Verdana, Arial;*/
            }
        </style>
        <script language="javascript" type="text/javascript">
            function UpdateAllChildren(nodes, checked) {
                var i;
                for (i = 0; i < nodes.get_count(); i++) {
                    if (checked) {
                        nodes.getNode(i).check();
                    }
                    else {
                        nodes.getNode(i).set_checked(false);
                    }

                    if (nodes.getNode(i).get_nodes().get_count() > 0) {
                        UpdateAllChildren(nodes.getNode(i).get_nodes(), checked);
                    }
                }
            }

            function CheckChildNodesImport(sender, eventArgs) {
                var node = eventArgs.get_node();
                var childNodes = eventArgs.get_node().get_nodes();
                var isChecked = eventArgs.get_node().get_checked();
                UpdateAllChildren(childNodes, isChecked);


                if (!node.get_checked()) {
                    while (node.get_parent().set_checked != null) {
                        node.get_parent().set_checked(false);
                        node = node.get_parent();
                    }
                }
                //UpdateAllChildrenImport(node.Nodes, node.Checked);
            }
			
        </script>
        <telerik:radtreeview id="RDTcommunityRepository" runat="server" width="600px" autopostback="false"
             cssfile="~/RadControls/TreeView/Skins/Materiale/StyleImport.css"
            checkchildnodes="true" tristatecheckboxes="true" multipleselect="true" checkboxes="true" />    
    </asp:View>
    <asp:View id="VIWempty" runat="server">
        <asp:Literal ID="LTnofile" runat="server">No community file to link found !</asp:Literal>
    </asp:View>
</asp:MultiView>