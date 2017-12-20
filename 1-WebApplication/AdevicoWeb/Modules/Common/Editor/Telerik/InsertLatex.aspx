<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InsertLatex.aspx.vb" Inherits="Comunita_OnLine.InsertLatex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><asp:Literal ID="LTlatexTitle" runat="server"></asp:Literal></title>

    <link rel="stylesheet" href="../../../../Graphics/Generics/css/4_UI_Elements.css" />
    <link rel="stylesheet" href="../../../../Graphics/Generics/Command/css/command.css" />
    <link rel="Stylesheet" href="../../../../Graphics/Modules/Editor/css/popup.css" />
      
      <script language="javascript" type="text/javascript">
          String.prototype.trim = function () {
              return this.replace(/^\s+|\s+$/g, "");
          }
          String.prototype.ltrim = function () {
              return this.replace(/^\s+/, "");
          }
          String.prototype.rtrim = function () {
              return this.replace(/\s+$/, "");
          }
          // This function replaces all instances of findStr in oldStr with repStr.

          function replaceAll(oldStr, findStr, repStr) {
              var srchNdx = 0;  // srchNdx will keep track of where in the whole line
              // of oldStr are we searching.
              var newStr = "";  // newStr will hold the altered version of oldStr.
              while (oldStr.indexOf(findStr, srchNdx) != -1)
              // As long as there are strings to replace, this loop
              // will run. 
              {
                  newStr += oldStr.substring(srchNdx, oldStr.indexOf(findStr, srchNdx));
                  // Put it all the unaltered text from one findStr to
                  // the next findStr into newStr.
                  newStr += repStr;
                  // Instead of putting the old string, put in the
                  // new string instead. 
                  srchNdx = (oldStr.indexOf(findStr, srchNdx) + findStr.length);
                  // Now jump to the next chunk of text till the next findStr.           
              }
              newStr += oldStr.substring(srchNdx, oldStr.length);
              // Put whatever's left into newStr.             
              return newStr;
          }
          function RenderLatex(el) {
              lit = document.getElementById('<%=me.IMGlatex.ClientID %>');

              address = '<%=me.RendeUrl%>';
              txt = document.getElementById('<%=me.TXBvalue.ClientID %>');

              st = "";
              st = txt.value;
              st = st.trim();
              url = st;
              while (url.indexOf("+") > 0)
                  url = url.replace("+", "%2B");

              if (st != "") {
                  lit.style.display = "block";
                  lit.setAttribute("src", address + url);
                  lit.setAttribute("alt", st);
                  lit.setAttribute("title", st);
                  lit.style.display = "block";
                  //lit.setAttribute("onClick","javascript:alert('"+st+"'); return false;");            
              } else {
                  lit.style.display = "none";
              }
          }
	   
	   </script>
    <%--<script type="text/javascript">
        function getRadWindow() //mandatory for the RadWindow dialogs functionality
        {
            if (window.radWindow) {
                return window.radWindow;
            }
            if (window.frameElement && window.frameElement.radWindow) {
                return window.frameElement.radWindow;
            }
            return null;
        }

        var tagValue = document.getElementById("<%= TXBvalue.ClientId%>");
        function initDialog() //called when the dialog is initialized
        {
            var clientParameters = getRadWindow().ClientParameters;

            RenderLatex(args.text)
        }
        if (window.attachEvent) {
            window.attachEvent("onload", initDialog);
        }
        else if (window.addEventListener) {
            window.addEventListener("load", initDialog, false);
        }

        function insertLatex() //fires when the Insert Link button is clicked
        {
            var closeArgument = {};
            closeArgument.openTag = "[latex]";
            closeArgument.text = tagValue.value;
            closeArgument.closeTag = "[latex]";
            getRadWindow().close(closeArgument);

            return false;
        }
    </script>--%>

     <!-- Preview Latex -->
	 
</head>
<body>
    <form id="form1" runat="server">
        <%--<fieldset>
            <legend></legend>
            <div>
			    
			    
	   	    </div>
            <div>
	   		    <br />
	   		    
	   	    </div>
        </fieldset>--%>

        <div class="popupcontainer latex">
    <div class="fieldobject">
        <div class="fieldrow logo">
            <span class="logoimg"></span>
            <span class="fieldlabel">
                <asp:Literal ID="LTlatexTitleInfo" runat="server"></asp:Literal>
            </span>
        </div>
        <div class="fieldrow input">
            <asp:TextBox ID="TXBvalue" runat="server" TextMode="MultiLine" TabIndex="1"></asp:TextBox>
            <span class="icons">
                <!--<span class="icon delete"></span>-->
                <!--<span class="icon view"></span>-->
            </span>
        </div>
        <div class="fieldrow previewer">
            <div class="preview">
                <asp:Image id="IMGlatex" runat="server"/>
            </div>
        </div>
        <div class="fieldrow commands">
            <!--<a class="linkMenu">Annulla</a>-->
            <%--<input class="linkMenu" type="submit" value="Inserisci">--%>
            <asp:Button id="BTNinsertLatex" runat="server" Text="*Add" OnClientClick="return insertLatex();"/>
        </div>
    </div>
</div>

        <script type="text/javascript">
            function getRadWindow() //mandatory for the RadWindow dialogs functionality
            {
                if (window.radWindow) {
                    return window.radWindow;
                }
                if (window.frameElement && window.frameElement.radWindow) {
                    return window.frameElement.radWindow;
                }
                return null;
            }

            var tagValue = document.getElementById("<%= TXBvalue.ClientId%>");
            function initDialog() //called when the dialog is initialized
            {
                var clientParameters = getRadWindow().ClientParameters;

                document.getElementById("<%= TXBvalue.ClientId%>").focus();

                document.getElementById("<%= TXBvalue.ClientId%>").value = clientParameters;

                RenderLatex(clientParameters)
            }
            if (window.attachEvent) {
                window.attachEvent("onload", initDialog);
            }
            else if (window.addEventListener) {
                window.addEventListener("load", initDialog, false);
            }

            function insertLatex() //fires when the Insert Link button is clicked
            {
                var closeArgument = {};

                var notEditable = false;

                if (notEditable) {
                    closeArgument.openTag = "<span class='noteditable' contenteditable='false'>[latex]<span class='editable' contenteditable='true'>";
                    closeArgument.text = tagValue.value;
                    closeArgument.closeTag = "</span>[/latex]</span>";
                }
                else {
                    closeArgument.openTag = "[latex]";
                    closeArgument.text = tagValue.value;
                    closeArgument.closeTag = "[/latex]";
                }

                
                getRadWindow().close(closeArgument);

                return false;
            }
    </script>
    </form>
</body>
</html>