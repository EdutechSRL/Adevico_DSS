<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Test.aspx.vb" Inherits="Comunita_OnLine.Test1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript">
    $(function () {
        $(".requestGlossary").click(function () {

            var title = $(this).attr("title");
            //var url = $(this).data("url");
            var html = "bla bla bla bla bla bla";

            var baseurl = "";
            var ItemId = $(this).data("id");

            $.ajax({
                type: "POST",
                url: "Glossary.asmx/GetGlossaryString",
                data: "{'ItemId':'" + ItemId + "', 'Htmltype':'2'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    html = msg.d;

                    $(".dialog.dialogGlossary").parents(".ui-dialog").find("span.ui-dialog-title").html(title);
                    $(".dialog.dialogGlossary").find("div.itemcontent").html(html);

                    $(".dialog.dialogGlossary").dialog("open");
                },
                error: function (result) {
                    html = result.status + "<br>" + result.statusText;

                    $(".dialog.dialogGlossary").parents(".ui-dialog").find("span.ui-dialog-title").html(title);
                    $(".dialog.dialogGlossary").find("div.itemcontent").html(html);

                    $(".dialog.dialogGlossary").dialog("open");
                }
            });



            return false;
        });

        $(".requestFaq dt").live("click", function () {
            $(this).toggleClass("expanded");
            $(this).next("dd").toggleClass("expanded");
        });

        $(".requestFaq").each(function () {
            //var url = $(this).data("url");
            var html = "";

            var baseurl = "";
            var ItemId = $(this).data("id");

            $this = $(this);

            if (ItemId.indexOf(";") > 0) {

                var Ids = ItemId.split(";");

                html = "";

                for (var _ids in Ids) {

                    var value = Ids[_ids];
                    if (value != "") {

                        $.ajax({
                            type: "POST",
                            url: baseurl + "Faq.asmx/GetFaqString",
                            data: "{'ItemId':'" + value + "', 'Htmltype':'4'}",
                            processData: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",

                            success: function (msg) {
                                html += msg.d;
                                $this.html("<dl>" + html + "</dl>");
                            },
                            error: function (result) {
//                                html += result.status + "<br>" + result.statusText;
//                                $this.html(html);
                            }
                        });
                    }

                }
            } else {

                $.ajax({
                    type: "POST",
                    url: baseurl + "Faq.asmx/GetFaqString",
                    data: "{'ItemId':'" + ItemId + "', 'Htmltype':'2'}",
                    processData: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: function (msg) {
                        html = msg.d;
                        $this.html(html);
                    },
                    error: function (result) {
                        html = result.status + "<br>" + result.statusText;
                        $this.html(html);
                    }
                });
            }


        });

        $(".dialog").dialog({
            autoOpen: false
        });
    });
    </script>
    
    <style type="text/css">
    div.requestFaq dl,
    div.itemcontent dl
    {
        font-size:10px;        
        text-align:left;  
        color:Black;
    }
    
    div.requestFaq dl dt,
    div.itemcontent dl dt
    {
        text-align:left;      
        color:Black;
        font-weight:bold;
    }
    
    div.requestFaq dl dd,
    div.itemcontent dl dd
    {
        text-align:left;
        color:Black;
    }
    
    div.requestFaq dl dd
    {
        display:none;
    }
    div.requestFaq dl dd.expanded
    {
        display:block;
    }
    </style>


     <%--<script type="text/javascript">
         //NOTA:
         //Gli script a seguire VANNO RIFATTI EX-NOVO,
         //servivano puramente a scopo di test per verificare:
         // 1. I parametri necessari al recupero dei dati
         // 2. la correttezza dei dati recuperati

         // QUINDI:
         // rifare ex-novo, magari mettendoli in un apposito file .js,
         // ottimizzare le varie funzioni,
         // standardizzare i vari parametri necessari
         // migliorare l'output (RIFARE). Al momento è una [mezza] porcheria.
         // M.B.

         var _mouseX;
         var _mouseY;
         jQuery(document).ready(function () {
             $(document).mousemove(function (e) {
                 _mouseX = e.pageX;
                 _mouseY = e.pageY;
             });
         })

         function showDesc(Service, ItemId) {

             var intDiv = document.getElementById('infoTxt');

             var FaqText = "";

            switch(Service)
            {
                case "SRVFAQ":
                    $.ajax({
                        type: "POST",
                        url: "Faq.asmx/GetFaqString",
                        data: "{'ItemId':'" + ItemId + "', 'Htmltype':'2'}",
                        processData: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",

                        success: function (msg) {
                            intDiv.innerHTML = msg.d;
                            intDiv.className = "info Text_show";
                            intDiv.style.top = _mouseY;
                            intDiv.style.left = _mouseX;
                        },
                        error: function (result) {
                            intDiv.innerHTML = result.status + "<br>" + result.statusText;
                            intDiv.style.top = _mouseY;
                            intDiv.style.left = _mouseX;
                        }
                    });
                    break;
                case "SRVGLS":
                    $.ajax({
                        type: "POST",
                        url: "Glossary.asmx/GetGlossaryString",
                        data: "{'ItemId':'" + ItemId + "', 'Htmltype':'2'}",
                        processData: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",

                        success: function (msg) {
                            intDiv.innerHTML = msg.d;
                            intDiv.className = "info Text_show";
                            intDiv.style.top = _mouseY;
                            intDiv.style.left = _mouseX;
                        },
                        error: function (result) {
                            intDiv.innerHTML = result.status + "<br>" + result.statusText;
                            intDiv.style.top = _mouseY;
                            intDiv.style.left = _mouseX;
                        }
                    });
                    break;
             }
         }


         function hideDesc() {
             var intDiv = document.getElementById('infoTxt');
             intDiv.className = "info Text_hide";
             intDiv.innerHTML = "";
         
         }
     </script>
     <style type="text/css">
         #infoTxt { display: block; position:fixed; }
        .test { color: Red; }
        .test2 { color: Green; }
        .test3 { color: Blue; }
        .Text_show { color: Black; border: 1px solid black; background-color: White; padding:1em; }
        .Text_hide { color: Gray;  border: 0px; padding:0; background-color:transparent; }
     </style>--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Test FAQ e GLOSSARIO.
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    
    <div class="test">

        <a href="" data-id="5" class="requestGlossary" title="Glossario">test</a>        
        <div class="requestFaq" data-id="15;14;13">&nbsp;</div>        
        
        <div class="dialog dialogGlossary">
            <div class="itemcontent"></div>
        </div>
    </div>
    
    
    <hr />
        <h2>Note</h2>
        <ol>
            <li>Usare la comunità <b>58</b> (laboratorio maieutiche).</li>
            <li>Il link alla FAQ punta alla pubblicazione locale in MVC! Modificare con l'url corretto.</li>
        </ol>
    <hr />
    <asp:Repeater ID="RPTfaq" runat="server">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                Show FAQ -<a href="http://localhost/ComolMvc/faq/Show/<%#Container.DataItem.ID%>" target="_blank" onmouseover="showDesc('SRVFAQ', <%#Container.DataItem.ID%>);" onmouseout="hideDesc()"><%#GetString(Container.DataItem.Question)%></a>- other text...
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
        
            <%--<li>
                Show <a href="#" onmouseover="showDesc('SRVFAQ', 13);" onmouseout="hideDesc()">FAQ about this text.</a>
            </li>
            <li>
                Show <a href="#" onmouseover="showDesc('SRVFAQ', 14);" onmouseout="hideDesc()">FAQ about this text.</a>
            </li>
            <li>
                Show <a href="#" onmouseover="showDesc('SRVFAQ', 15);" onmouseout="hideDesc()">FAQ about this text.</a>
            </li>
            <li>
                Show <a href="#" onmouseover="showDesc('SRVFAQ', 18);" onmouseout="hideDesc()">FAQ about this text.</a>
            </li>
            <li>
                Show <a href="#" onmouseover="showDesc('SRVFAQ', 19);" onmouseout="hideDesc()">FAQ about this text.</a>
            </li>
            <li>
                Show <a href="#" onmouseover="showDesc('SRVFAQ', 21);" onmouseout="hideDesc()">FAQ about this text.</a>
            </li>
            <li>
                Show <a href="#" onmouseover="showDesc('SRVFAQ', 22);" onmouseout="hideDesc()">FAQ about this text.</a>
            </li>
        </ul>--%>
        
        <asp:Repeater ID="Rpt_Group" runat="server">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <span><b>
                    Group: <asp:Literal ID="lit_group" runat="server"></asp:Literal>
                </b></span>

                <asp:Repeater ID="Rpt_Item" runat="server">
                    <HeaderTemplate><ul></HeaderTemplate>
                    <ItemTemplate>
                        <li>
                        Show Glossary -<a href="http://localhost/ComolMvc/Glossary/ShowTerm/<%#Container.DataItem.ID%>" target="_blank" onmouseover="showDesc('SRVGLS', <%#Container.DataItem.ID%>);" onmouseout="hideDesc()"><%#GetString(Container.DataItem.Term)%></a>- other text...                         </li>
                    </ItemTemplate>
                    <FooterTemplate></ul></FooterTemplate>
                </asp:Repeater>

            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>

    <div id="infoTxt" class="info Text_hide">
           <h1>Per test usare comunità 58 (Lab Maieutiche)</h1>
    </div>

</asp:Content>