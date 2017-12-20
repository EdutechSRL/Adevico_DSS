<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DebugAccess.aspx.vb" Inherits="Comunita_OnLine.DebugAccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        
        .submitLinks
        {
            border:1px solid #ddd;
            border-radius:5px;
            background-color:#eee;
            width:40%;
            margin-left:auto;
            margin-right:auto;
            margin-top:0.5em;
            
        }
        
        label.defaultValue,
        .submitLink 
        {
            margin:0.3em;
            display:inline-block;
            *display:inline;
            color:#444;
            text-decoration:none;
            cursor:pointer;
            
            padding:5px;
        }
        
        .submitLink:hover
        {
            text-decoration:underline;
        }
    </style>
    <script src="Scripts/jquery-1.11.0.min.js"></script>
    <script src="Scripts/jquery-migrate-1.2.1.js"></script>
    <script src="Jscript/jquery.cookie.js"></script>
    <script>
        $(document).ready(function () {
            $("input[type='submit']").click(function () {
                $("input[type='text']").each(function () {
                    var value = $(this).val();

                    $.cookie("FormSave[" + $(this).attr("id") + "]", value);
                });
            });

            $("input[type='text']").each(function () {
                $(this).data("default", $(this).val());
                var value = $.cookie("FormSave[" + $(this).attr("id") + "]");
                if (value != null) {
                    $(this).val(value);
                }
            });

            $(".eraseCookie").live("click", function () {
                $("input[type='text']").each(function () {
                    $.cookie("FormSave[" + $(this).attr("id") + "]", null);
                });
                return false;
            });

            $(".defaultValue").live("click", function () {
                var id = $(this).attr("for");
                $("#" + id).val($("#" + id).data("default"));
                return false;
            });

            $(".defaultAllValue").live("click", function () {
                $("input[type='text']").each(function () {
                    $(this).val($(this).data("default"));
                });
                return false;
            });


            $("input[type='submit']").first().after($("<div class='submitLinks'><a class='submitLink eraseCookie' href='' title='Erase cookies'>Erase Cookies</a> | <a class='submitLink defaultAllValue' href='' title='Default Values'>Default Values</a></div>"));

            $("input[type='text']").each(function () {
                $(this).after($("<label class='defaultValue' for='" + $(this).attr("id") + "' title='Default value'>x</label>"));
            });
        });
    </script>
    <style type="text/css">
        body
        {
            font-family: Arial;
            /*background-color: #FF1010;
            color: #ffffff;*/
            text-align: center;
            }
            
        div.main
        {
            display: inline-block;
            *display: inline;
            padding-top: 75px;
            width: 650px;
            text-align: left;
            }
            
        span.titolo_main
        { font-size: large; font-weight: bold; display: block; width: 100%; text-align: center;}
        span.titolo
        {  display: inline-block; width: 100px; font-weight: bold; padding: 3px 0px 0px 1px;}
        
        span.button
        {   display: block; width: 100%; text-align: center; padding: 20px;}
        
        input#BTNlogin
        {   font-weight:bold; width: 100px; height: 35px;}
        
        img
        { display: none; }
        
        /* Xmas */
        /*body
        {
            background-color: #FF1010;
            color: #ffffff;
         }*/
            
       /* div.main
        {
            
            }
            
        span.titolo_main
        { }
        span.titolo
        {  }
        
        span.button
        {   }
        
        input#BTNlogin
        {   }
        
        img
        { display: inline; }*/
        
    </style>
</head>
<body>
    <form id="aspnetForm" runat="server">
    <div class="main">
        <span class="titolo_main">Debug Access</span>
        <br />
        <span class="titolo">Comunità</span>
        <asp:TextBox ID="comunitaID" runat="server" text="160"></asp:TextBox>
        <br />
        <span class="titolo">User ID</span>
        <asp:TextBox ID="userID" runat="server" Text="1"></asp:TextBox>
        <br />
        <span class="titolo">Destinazione</span>
        <asp:TextBox ID="TXBdestinazione" runat="server" Columns="80" Text="Modules/CallForPapers/Calls.aspx"></asp:TextBox>
        <br />
        <span class="button">
            <asp:Button ID="BTNlogin" runat="server" text="Entra" />
        </span>
          <span class="titolo">Test</span>
        <asp:TextBox ID="TXBtest" runat="server" Columns="80"></asp:TextBox>
        <br />
        <span class="titolo">Value</span>
            <asp:Label ID="LBtest" runat="server"></asp:Label>
        <br />
        <span class="button">
             <asp:Button ID="BTNtest" runat="server" text="Temp" />
        </span>

         <span class="titolo">Test</span>
        <asp:TextBox ID="TXBreTest" runat="server" Columns="80"></asp:TextBox>
        <br />
        <span class="titolo">Value</span>
            <asp:Label ID="LBreTest" runat="server"></asp:Label>
        <br />
        <span class="button">
             <asp:Button ID="BTNreTest" runat="server" text="Temp" />
        </span>
         <br />
        <span class="titolo">Esporta</span>
            Esporta se ti serve !
        <br />
        <span class="button">
             <asp:Button ID="BTNexport" runat="server" text="Export" />
        </span>
         <span class="titolo">Importa</span>
            Prova a importare se ti serve !
        <br />
        <span class="button">
             <asp:Button ID="BTNimport" runat="server" text="Import" />
        </span>
    <br />
     <span class="titolo">Verifica ID ADA</span>
        <asp:TextBox ID="TXBada" runat="server" Columns="80"></asp:TextBox>
        <br />
        <span class="titolo">Value</span>
            <asp:Label ID="LBada" runat="server"></asp:Label>
        <br />
          <span class="button">
             <asp:Button ID="BTNada" runat="server" text="Verifica ADA" />
        </span>
    </div>
    </form>
</body>
</html>
