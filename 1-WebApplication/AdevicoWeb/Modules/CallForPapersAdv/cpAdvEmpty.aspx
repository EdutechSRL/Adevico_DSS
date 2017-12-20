<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="cpAdvEmpty.aspx.vb" Inherits="Comunita_OnLine.cpAdvEmpty" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server" EnableScripts="true"  EnableTreeTableScript="true" EnableDropDownButtonsScript="true"  />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="viewbuttons clearfix">
        -Bottoni top pagina
    </div>
    <div class="contentwrapper edit clearfix">
        -contenuto
        <br/><hr/><br/>
        
        
        <div> <!-- nascosta bene -->
            Textbox tags store:
            <!-- input con id statico, store dei tag condivisi -->
            <asp:textbox ID="tagsinputSuggest" CssClass="hide"  ClientIDMode="Static" runat="server">Amsterdam,Washington,Sydney,Beijing,Cairo</asp:textbox>
        </div>        
        
        <br/><hr/><br/>        
        <!-- Area input -->
        Textbox di test 1 (Con 1 tag):
        <asp:textbox ID="Textbox1" runat="server" CssClass="tagsinput hide">Prova 1</asp:textbox>
        
        <br/><hr/><br/>
        
        Textbox di test 2 (Con 2 tag):
        <asp:textbox ID="Textbox2" runat="server" CssClass="tagsinput hide">Altre prove, cose carine</asp:textbox>
                
        <br/><hr/><br/>

        Textbox Only Stored di test 3 (vuota):
        <asp:textbox ID="Textbox3" runat="server" CssClass="tagsinputOnlyStored hide"></asp:textbox>
        <!-- Fine Area input -->
        
        <br/><hr/><br/>
    </div>
    
    <!-- inizio import per tag-it -->
    <script src="../../Jscript/tag-it-new/tag-it-new.js"></script>
    <link href="../../Graphics/Plugins/tagit/jquery.tagit.css" rel="Stylesheet" />
    <script type="text/javascript">
        $(function () { //aspetto il ready di jquery

            function getSuggestArr() { //funzione che ritorna l'array di autocomplate
                return (jQuery("#tagsinputSuggest").val() + "").split(',');  // prendo  i tag presenti in #tagsinputSuggest
            };

            $('input.tagsinput').tagit({ //inizializzazione di tagit su tutte le input con classe tagsinput
                autocomplete : { sourceFN: getSuggestArr },
                allowSpaces: true,
                afterTagAdded: function(evt, ui) {
                    var _tagLabel = ui.tagLabel;
                    var _SuggestArr = getSuggestArr();
                    if(_SuggestArr.indexOf(_tagLabel) < 0){
                        _SuggestArr.push(_tagLabel);
                        jQuery("#tagsinputSuggest").val(_SuggestArr.join());                    
                    }
                }
            });

            $('input.tagsinputOnlyStored').tagit({ //inizializzazione di tagit su tutte le input con classe tagsinputOnlyStored
                autocomplete : { sourceFN: getSuggestArr },
                allowSpaces: true,
                beforeTagAdded: function(event, ui) {
                    if(getSuggestArr().indexOf(ui.tagLabel) < 0)
                    {
                        return false;
                    }
                    return true;
                }
            });

        });
    </script>
    <style type="text/css">
        .tagit-label{
            margin:0 !important;
        }
    </style>
</asp:Content>
