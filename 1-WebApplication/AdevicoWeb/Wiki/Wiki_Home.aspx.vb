Imports lm.ActionDataContract
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Wiki

Partial Public Class Wiki_Home
	Inherits PageBase
    Implements COL_Wiki.WikiNew.IViewWikiHome

    Private _Servizio As New UCServices.Services_Wiki 'Diverrà poi quello pagebase, eprmessi, etc...
    Private oPresenter As COL_Wiki.WikiNew.PresenterWikiHome
    Private _UltimaLettera As String = "^"

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
    Private Property SearchString() As String Implements COL_Wiki.WikiNew.IViewWikiHome.SearchString
        Get
            Dim temp As String
            temp = Me.TXB_search.Text
            Select Case DDL_ricerca.SelectedIndex
                Case 1
                    temp &= "%"
                Case 2
                    temp = "%" & temp
                Case 3
                    temp &= "%"
                    temp = "%" & temp
                Case Else
                    temp = temp
            End Select
            Return temp
        End Get
        Set(ByVal value As String)
            Me.TXB_search.Text = value
        End Set
    End Property
    Private Property ActualTopicId() As System.Guid Implements COL_Wiki.WikiNew.IViewWikiHome.ActualTopicId
        Get
            If Not IsNothing(ViewState("Wiki_TopicId")) Then
                Try
                    Dim GuidStr As String = ViewState("Wiki_TopicId")
                    If GuidStr = "" Then
                        Return Guid.Empty
                    End If
                    Return New System.Guid(GuidStr)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Guid.Empty
            End If
        End Get
        Set(ByVal value As System.Guid)
            ViewState("Wiki_TopicId") = value.ToString
        End Set
    End Property
    Private Property ActualSezioneId() As System.Guid Implements COL_Wiki.WikiNew.IViewWikiHome.ActualSezioneId
        Get
            If Not IsNothing(ViewState("Wiki_SezioneId")) Then
                Try
                    Dim GuidStr As String = ViewState("Wiki_SezioneId")
                    Return New System.Guid(GuidStr)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As System.Guid)
            ViewState("Wiki_SezioneId") = value.ToString
        End Set
    End Property

    Private Property Servizio() As UCServices.Services_Wiki Implements COL_Wiki.WikiNew.IViewWikiHome.Servizio
        Get
            Return _Servizio
        End Get
        Set(ByVal value As UCServices.Services_Wiki)
            _Servizio = value
        End Set
    End Property

    Private Sub Wiki_Home2_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'Per il reindirizzamento
        Response.Redirect("wiki_comunita.aspx?" + Request.QueryString.ToString)


        Me.oPresenter = New COL_Wiki.WikiNew.PresenterWikiHome(Me, COL_Wiki.FactoryWiki.ConnectionType.SQL, True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Property ActualView() As COL_Wiki.WikiNew.VisualizzazioniHome Implements COL_Wiki.WikiNew.IViewWikiHome.ActualView
        Get
            Dim Out As COL_Wiki.WikiNew.VisualizzazioniHome
            Try
                If Not IsNothing(ViewState("WikiView")) Then
                    Out = CInt(ViewState("WikiView"))
                Else
                    Out = COL_Wiki.WikiNew.VisualizzazioniHome.PageNotRender
                End If
            Catch ex As Exception
                Out = COL_Wiki.WikiNew.VisualizzazioniHome.PageNotRender
            End Try
            Return Out
        End Get
        Set(ByVal value As COL_Wiki.WikiNew.VisualizzazioniHome)
            ViewState("WikiView") = value
        End Set
    End Property

    Public Sub Show(ByVal Content As COL_Wiki.WikiNew.VisualizzazioniHome) Implements COL_Wiki.WikiNew.IViewWikiHome.Show

        Me.PNL_NoPermessi.Visible = False
        Me.PNL_Wiki.Visible = False
        Me.PNL_NoTopic.Visible = False
        Me.PNL_Navigatore.Visible = False
        Me.PNL_NavigatoreNoTopic.Visible = False
        Me.PNL_TopicElenco.Visible = False
        Me.PNL_NoSezioni.Visible = False
        Me.PNL_ResultSearch.Visible = False
        Me.BTN_Lista.Visible = False
        Me.PNL_TopicView.Visible = False
        'Me.LBL_TitoloWiki.Visible = False

        Select Case Content
            Case COL_Wiki.WikiNew.VisualizzazioniHome.NoPermessi
                Me.PNL_NoPermessi.Visible = True
            Case COL_Wiki.WikiNew.VisualizzazioniHome.NoSezione
                Me.PNL_Wiki.Visible = True
                Me.PNL_NavigatoreNoTopic.Visible = True
                Me.PNL_NoSezioni.Visible = True
                Me.PNL_search.Visible = True
            Case COL_Wiki.WikiNew.VisualizzazioniHome.NoTopic
                Me.PNL_Wiki.Visible = True
                Me.PNL_Navigatore.Visible = True
                Me.PNL_search.Visible = True
                Me.PNL_NoTopic.Visible = True
            Case COL_Wiki.WikiNew.VisualizzazioniHome.ListaTopic
                Me.PNL_Wiki.Visible = True
                Me.PNL_Navigatore.Visible = True
                Me.PNL_search.Visible = True
                Me.PNL_TopicElenco.Visible = True
            Case COL_Wiki.WikiNew.VisualizzazioniHome.ListaTopicSearched
                Me.PNL_Wiki.Visible = True
                Me.PNL_search.Visible = True
                Me.PNL_ResultSearch.Visible = True
                Me.PNL_Navigatore.Visible = False
                'Me.BTN_Lista.Visible = True
                Me.BTN_Homewiki.Visible = True
                'Me.LBL_TitoloWiki.Visible = False
            Case COL_Wiki.WikiNew.Visualizzazioni.ShowTopic
                Me.PNL_Wiki.Visible = True
                Me.PNL_TopicView.Visible = True
                'Me.BTN_Lista.Visible = True
        End Select
    End Sub

#Region "Binding"
    Public Sub BindNavigatore(ByVal Sezioni As System.Collections.IList) Implements COL_Wiki.WikiNew.IViewWikiHome.BindNavigatore
        Me.RPT_LinkNavigatore.DataSource = Sezioni
        Me.RPT_LinkNavigatore.DataBind()
    End Sub
    Public Sub BindTopicsFunded(ByVal Topics As System.Collections.IList) Implements COL_Wiki.WikiNew.IViewWikiHome.BindTopicsFunded

        Me.PNL_search.Visible = True
        Me.DLS_result.DataSource = Topics
        Me.DLS_result.DataBind()

    End Sub
    Public Sub BindTopicsSezione(ByVal Topics As System.Collections.IList, ByVal Sezione As COL_Wiki.WikiNew.SezioneWiki) Implements COL_Wiki.WikiNew.IViewWikiHome.BindTopicsSezione

        'Try
        '    Me.Master.ServiceTitle = Resource.getValue("LBTitolo.text") & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & Sezione.Wiki.Nome
        '    'Me.LBL_TitoloWiki.Text = Sezione.Wiki.Nome

        '    'Me.LBL_TitoloWiki.Visible = True
        'Catch ex As Exception
        '    Me.Master.ServiceTitle = Resource.getValue("LBTitolo.text")
        '    'Me.LBL_TitoloWiki.Visible = False
        'End Try
        Try
            Me.LBL_ElencoSezioneComunita.Text = Sezione.Wiki.Comunita.Nome
            Me.LBL_ElencoSezioneComunita.Visible = True
            Me.LBL_ElencoSezioneComunita_t.Visible = True
        Catch ex As Exception
            Me.LBL_ElencoSezioneComunita.Visible = False
            Me.LBL_ElencoSezioneComunita_t.Visible = False
        End Try

        Me.LBL_ElencoSezioneDescrizioneData.Text = Sezione.DataInserimento.ToString

        Try
            Me.LBL_ElencoSezionePersona.Text = Sezione.Persona.Anagrafica
            Me.LBL_SezioneAttuale.Text = Sezione.NomeSezione
        Catch ex As Exception
        End Try

        Me.DLS_topics.DataSource = Topics
        Me.DLS_topics.DataBind()

    End Sub

#End Region

#Region "PageBase"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides Sub BindDati()

        If Not (String.IsNullOrEmpty(Request.QueryString("id"))) Then

            Dim a As String = Request.QueryString("id")
            Me.ActualTopicId = New Guid(a)
            'Me.AddAction(ActionType.ViewCrossTopic, Me.PageUtility.CreateObjectsList(ObjectType.Topic, Me.ActualTopicId.ToString))
            a = Me.ActualTopicId.ToString
            Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.TopicView)
        Else
            Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.Reset)
        End If

    End Sub
    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Dim oServizio As New CL_permessi.COL_Servizio
        oServizio.EstraiByCode(UCServices.Services_Wiki.Codex, MyBase.LinguaID)
        Return oServizio.isAttivato
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Wiki_Home", "Wiki")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            '.setLabel(LBTitolo) '               Wiki
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")

            .setLabel(LBL_NoPermessi) '         Non si dispone dei permessi necessari per visualizzare la pagina.
            .setLabel(LBL_Nav_NoSezione) '      Nessuna sezione presente
            .setLabel(LBL_Navigatore_t) '       Elenco(sezioni)

            .setLabel(LBL_Con_SezioneNO) '      Nessuna sezione presente
            .setLabel(LBL_NoTopic_t) '          Nessun Topic presente

            .setLabel(Me.LBL_ElencoSezioneDescrizioneData_t) '     Modificata(il)
            .setLabel(LBL_ElencoSezionePersona_t) '     da()
            .setLabel(LBL_ElencoSezioneComunita_t) '    Comunità:
            '.setLabel(LBL_LinkTopic_t) '                Indice dei topic

        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Eventi page"
    Private Sub RPT_LinkNavigatore_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPT_LinkNavigatore.ItemCommand
        Select Case e.CommandName
            Case "GotoSection"
                Me.ActualSezioneId = New Guid(e.CommandArgument.ToString)
                Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.SezioneGoto)
        End Select

    End Sub
    Private Sub RPT_LinkNavigatore_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPT_LinkNavigatore.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim LnkSezione As LinkButton
            Try
                LnkSezione = e.Item.FindControl("LNK_VoceNavi")
                Dim tmpSezione As COL_Wiki.WikiNew.SezioneWiki
                tmpSezione = e.Item.DataItem
                LnkSezione.Text = tmpSezione.NomeSezione 'e.Item.DataItem("NomeSezione")
                LnkSezione.CommandName = "GotoSection"
                LnkSezione.CommandArgument = tmpSezione.ID.ToString 'e.Item.DataItem("ID")
                '--Parte che sottolinea o fa in grassetto la sezione ocrrente
                'If Me.ActualSezioneId = tmpSezione.ID Then
                '    LnkSezione.Font.Bold = True
                'End If
            Catch ex As Exception

            End Try
        End If
    End Sub
#End Region

    Private Sub BTN_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_search.Click
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.TopicSearch)
    End Sub
    '-----------BIND PER IL SINGOLO TOPIC--------------------------
    Public Sub BindTopicTest(ByVal oTopic As COL_Wiki.WikiNew.TopicWiki) Implements COL_Wiki.WikiNew.IViewWikiHome.BindTopicTest
        Me.ActualTopicId = oTopic.ID
        'Me.TXB_TitoloTopic.Text = oTopic.Nome
        Me.LBL_TitoloTopicView.Text = oTopic.Nome
        Me.LBL_autore.Text = "Inserito da " & oTopic.Persona.Anagrafica.ToString & "  ultima modifica " & oTopic.DataModifica.ToString
        Me.LBL_TestView.Text = oTopic.Contenuto
    End Sub

    Private Sub DLS_topics_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLS_topics.ItemDataBound
        Dim LBL_temp As Label
        Dim LBL_temp2 As Label = _
         CType(e.Item.FindControl("LBL_Iniziale"), Label)
        Try

            LBL_temp = e.Item.FindControl("LBL_topic")
            If (Me._UltimaLettera = Char.ToUpper(LBL_temp.Text.Chars(0))) Then
                LBL_temp2.Visible = False
            Else
                LBL_temp2.Text = Char.ToUpper(LBL_temp.Text.Chars(0)) + "<br/>"
                LBL_temp2.Visible = True
                LBL_temp2 = e.Item.FindControl("LBL_separatore")
                LBL_temp2.Visible = True

            End If
            Me._UltimaLettera = Char.ToUpper(LBL_temp.Text.Chars(0))


        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BTN_Homewiki_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_Homewiki.Click
        Response.Redirect("wiki_home.aspx")
        'Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.Reset)
    End Sub

    Protected Sub BTN_Lista_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_Lista.Click
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.Binding.SezioneGoto)
    End Sub

    'Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
    '	PageUtility.CurrentModule = PageUtility.GetModule(UCServices.Services_Wiki.Codex)
    'End Sub
	Public Sub AddAction(ByVal oType As Integer, ByVal oObjectActions As List(Of PresentationLayer.WS_Actions.ObjectAction), Optional ByVal TypeIteration As InteractionType = InteractionType.Generic)
		Me.PageUtility.AddActionToModule(Me.PageUtility.GetModuleID(UCServices.Services_Wiki.Codex), oType, oObjectActions, TypeIteration)
	End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub
End Class

'<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
'<html xmlns="http://www.w3.org/1999/xhtml">
'<head id="Head1" runat="server">
'    <title>Wiki</title>
'    <link href="./../Styles.css" type="text/css" rel="stylesheet" />
'    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

'    <%--<script type="text/jscript">

'	    if (document.getElementById && document.createElement) {
'	        document.write('<style type="text/css">*.toggle{display:none}</style>');
'	        window.onload = function() {
'	            /*le modifiche allo script vanno solo fatte qui*/
'	            /* Aggingiamo qui le modifiche indicando "ID DIV","Testo per visualizzare","Testo per nascondere"*/
'	            Attiva("ORicerca", "Opzioni", "Semplice");

'	        }
'	    }

'	    function Attiva(id, s1, s2) {
'	        var el = document.getElementById(id);
'	        el.style.display = "none";
'	        var c = document.createElement("div");
'	        var link = document.createElement("a");
'	        link.href = "#";
'	        link.appendChild(document.createTextNode(s1));
'	        link.onclick = function() {
'	            link.firstChild.nodeValue = (link.firstChild.nodeValue == s1) ? s2 : s1;
'	            el.style.display = (el.style.display == "none") ? "block" : "none";
'	            return (false);
'	        }
'	        c.appendChild(link);
'	        el.parentNode.insertBefore(c, el);
'	    }

'    </script>--%>


'</head>
'<body>
'     <form id="aspnetForm" runat="server" defaultbutton="BTN_search" defaultfocus="TXB_search">
'     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
'    <table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">
'		<tr>
'			<td>
'			    <div>
'			        <HEADER:CtrLHEADER id="Intestazione" runat="server" ShowNews="false" HeaderNewsMemoHeight="85px"></HEADER:CtrLHEADER>	
'			    </div>
'			    <br style="clear:both;" />
'			</td>
'		</tr>
'        <tr>
'            <td>
'                <table cellspacing="0" cellpadding="0" width="900px" border="0">
'                    <tr>
'                        <td class="RigaTitolo" align="left">
'                            <asp:Label ID="LBTitolo" runat="server">Wiki</asp:Label>

'                            <asp:Label ID="LBL_TitoloWiki" runat="server">

'                            </asp:Label>
'                        </td>
'                    </tr>
'                    <tr>
'                        <td align="right">

'                        </td>
'                    </tr>
'                    <tr>
'                        <td>
'                            &nbsp;&nbsp;
'                        </td>
'                    </tr>
'                    <tr>
'                        <td align="center">

'                            </div>
'                        </td>
'                    </tr>
'                </table>
'            </td>
'        </tr>
'    </table>
'    <FOOTER:CtrLFooter ID="CtrLFooter" runat="server"></FOOTER:CtrLFooter>
'    </form>
'</body>
'</html>