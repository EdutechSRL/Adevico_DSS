<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TinyEditor.ascx.vb" Inherits="Comunita_OnLine.UC_TinyEditor" %>

<%--    <script type="text/javascript" src="<%=Me.BaseUrl %>Jscript/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="<%=Me.BaseUrl %>Jscript/jquery.validate.min.js"></script>--%>
    
<%--    <script type="text/javascript" src="<%=Me.BaseUrl %>uc/Editor/tiny_mce/jquery.tinymce.js"></script>
    <script type="text/javascript" src="<%=Me.BaseUrl %>uc/Editor/Editor.js"></script> 
--%>
<%--    <script  type="text/javascript">
        var TinyMceUrl= <%=Me.BaseUrl & "uc/Editor/Editor.js" %>
    </script>--%>

    <script  type="text/javascript">
        //var TinyMceUrl = "../../uc/Editor/tiny_mce/tiny_mce.js"
        var TinyMceUrl = '../uc/Editor/tiny_mce/tiny_mce.js'

        var Wiki_url = '';
        var Wiki_img = '~/Uc/Editor/tiny_mce/buttons/wiki.png") %>';

        var MyPlugin_url = '';
        var MyPlugin_img = '~/Uc/Editor/tiny_mce/buttons/plugin.png") %>';
    </script>
    
    <asp:Literal id="Lit_EditorConfig"
           Text="Hello World!!"
           runat="server"/>

    <script type="text/javascript" src="<%=Me.BaseUrl %>uc/Editor/tiny_mce/jquery.tinymce.js"></script>
    <script type="text/javascript" src="<%=Me.BaseUrl %>uc/Editor/Editor.js"></script> 



<div id="DIVeditorHTML" runat="server" align="center"  style="width: 100%;">
    <div id="DIVmenu" runat="server" style="float:left; width: 100%; text-align:right;">
	   <div id="DIVmenuPreview" runat="server" style="text-align:left; float:left; width: 60%; ">
		  <asp:Button ID="BTNpreview" runat="server" Text="Preview" /> 
	   </div>
	   <div id="DIVmenuScrolling" runat="server" style="text-align: right; float:right; width: 40%;" align="right">
		  <asp:Label ID="LBvelocitaAvanzato_t" Runat="server" CssClass="FiltroCampoSmall">Velocità scorrimento:&nbsp;</asp:Label>
		  <asp:dropdownlist ID="DDLScorrimento" Runat="server" CssClass="FiltroCampoSmall">
			 <asp:ListItem Value="0">Nessuno</asp:ListItem>
			 <asp:ListItem Value="1" Selected="True">Lento</asp:ListItem>
			 <asp:ListItem Value="2">Veloce</asp:ListItem>
			 <asp:ListItem Value="3">Molto Veloce</asp:ListItem>
		  </asp:dropdownlist>
	    </div>
    </div>
    <div class="contenitoreEditor" style=" clear:left; font-size:small;">
        <textarea class="editorFull" runat="server" id="TinyText" cols="50" rows="15"></textarea>
        
        <br />


        <script type="text/javascript">
            function showSelectedTextContent() {
                //Get a reference to the client-side RadEditor object:
                var myRadEditor = GetRadEditor("</%= RDEtext.ClientID %>");
                var theSelectionObject = myRadEditor.GetSelection();
                var theSelectedText = theSelectionObject.GetText();
                return theSelectedText
                // alert("The selected text is:\n" + theSelectedText + "\n\nin contrast to the selected Html:\n" + theSelectedHtml);
            }
            function showSelectedHTMLContent() {
                //Get a reference to the client-side RadEditor object:
                var myRadEditor = GetRadEditor("</%= RDEtext.ClientID %>");
                var theSelectionObject = myRadEditor.GetSelection();
                var theSelectedHtml = theSelectionObject.GetHtmlText();
                return theSelectedHtml
                // alert("The selected text is:\n" + theSelectedText + "\n\nin contrast to the selected Html:\n" + theSelectedHtml);
            }
        </script> 
    </div>
</div>


<div id="DIVpreview" runat="server" align="center"  style="width: 100%;">
    <div id="DIVmenuClose" runat="server" style="text-align:left; width: 100%; ">
	   <asp:Button ID="BTNclosePreview" runat="server" Text="Chiudi Preview" /> 
    </div>
    <div>
	   <asp:Literal ID="LTpreview" runat="server" ></asp:Literal>
    </div>
</div>
<div style="clear:both;"></div>
    <asp:CustomValidator 
        id="CVlunghezza" 
        Runat="server" 
        ErrorMessage="Testo troppo lungo." 
        Text="Testo troppo lungo."
        ClientValidationFunction="ValidateEditor"  
        Display="None"
        ControlToValidate="TinyText"></asp:CustomValidator>				      

    <asp:Literal 
        ID="LTscriptEditor" 
        runat="server"></asp:Literal>