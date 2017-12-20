<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectCommunityFiles.ascx.vb"
    Inherits="Comunita_OnLine.UC_SelectCommunityFiles" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style type="text/css">
    .rtChk
    {
        font-size: 18px;
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

<asp:Literal ID="LTnofile" runat="server" Visible="false">No community file to link found !</asp:Literal>
<telerik:RadTreeView ID="RDTcommunityRepository" runat="server" Width="600px" AutoPostBack="false"
    CssFile="~/RadControls/TreeView/Skins/Materiale/StyleImport.css"
    CheckChildNodes="true" TriStateCheckBoxes="true" MultipleSelect="true" CheckBoxes="true" />
