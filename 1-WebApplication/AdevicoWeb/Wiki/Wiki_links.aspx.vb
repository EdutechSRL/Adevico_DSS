Partial Public Class Wiki_links
    Inherits PageBase
    Implements COL_Wiki.WikiNew.IViewWikiUC


    Private _Servizio As New UCServices.Services_Wiki 'Diverrà poi quello pagebase, eprmessi, etc...
    Private oPresenter As COL_Wiki.WikiNew.PresenterWikiUC

    Private Property PageIndex() As Integer Implements COL_Wiki.WikiNew.IViewWikiUC.pageIndex
        Get
            If String.IsNullOrEmpty(ViewState("Currentpagewikisearch")) Then
                ViewState("Currentpagewikisearch") = 1
            End If
            Return ViewState("Currentpagewikisearch")
        End Get
        Set(ByVal value As Integer)
            ViewState("Currentpagewikisearch") = value
        End Set
    End Property
    Public Property PageSize() As Integer Implements COL_Wiki.WikiNew.IViewWikiUC.pageSize
        Get
            If String.IsNullOrEmpty(ViewState("Sizepagewikisearch")) Then
                ViewState("Sizepagewikisearch") = 5
            End If
            Return ViewState("Sizepagewikisearch")
        End Get
        Set(ByVal value As Integer)
            ViewState("Sizepagewikisearch") = value
        End Set
    End Property
    Public Property IsLastPage() As Boolean Implements COL_Wiki.WikiNew.IViewWikiUC.IsLastPage
        Get
            If String.IsNullOrEmpty(ViewState("IsLastPageWiki")) Then
                ViewState("IsLastPageWiki") = False
            End If
            Return ViewState("IsLastPageWiki")
        End Get
        Set(ByVal value As Boolean)
            ViewState("IsLastPageWiki") = value
        End Set
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Property TitoloPagina() As String Implements COL_Wiki.IViewGenerico.TitoloPagina
        Get
            Return "Testo a mano"
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Public ReadOnly Property MyIscrizioneCorrente() As COL_BusinessLogic_v2.IscrizioneComunita Implements COL_Wiki.IViewGenerico.MyIscrizioneCorrente
        Get

        End Get
    End Property
    Public ReadOnly Property MyComunitaCorrente() As COL_BusinessLogic_v2.Comunita.COL_Comunita Implements COL_Wiki.WikiNew.IViewWikiUC.MyComunitaCorrente
        Get
            'Return MyBase.ComunitaCorrente
            Dim oComunita As New COL_BusinessLogic_v2.Comunita.COL_Comunita
            Try
                oComunita.Id = Session("IdComunita") 'Session("ComunitaCorrente").id()
            Catch ex As Exception
                oComunita.Id = -1
            End Try
            Return oComunita
        End Get
    End Property
    Private Property ActualWikiId() As System.Guid Implements COL_Wiki.WikiNew.IViewWikiUC.ActualWikiId
        Get
            Dim GuidStr As String = ""
            Dim GuidOut As Guid
            Try
                GuidStr = Session("Wiki_Id")
                GuidOut = New System.Guid(GuidStr)
                'Return New System.Guid(GuidStr)
                Return GuidOut
            Catch ex As Exception
                GuidStr = Me.oPresenter.GetWikiID
                If Not GuidStr = "" Then
                    Session("Wiki_Id") = GuidStr
                    'Return New System.Guid(GuidStr)
                    'Return System.Guid.NewGuid(GuidStr)
                    GuidOut = New System.Guid(GuidStr)
                    Return GuidOut
                Else
                    Return System.Guid.Empty
                End If
            End Try
        End Get
        Set(ByVal value As System.Guid)
            Session("Wiki_Id") = value.ToString
        End Set
    End Property
    Public Property NomeWiki() As String Implements COL_Wiki.WikiNew.IViewWikiUC.NomeWiki
        Set(ByVal value As String)
            'Me.TXB_WikiAdd_Nome.Text = value
        End Set
        Get
            Return "Testo manuale1" 'Me.TXB_WikiAdd_Nome.Text
        End Get
    End Property

    Private Property SearchString() As String Implements COL_Wiki.WikiNew.IViewWikiUC.SearchString
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
                    If (String.IsNullOrEmpty(temp)) Then
                        temp = "%"
                    End If
                    temp = temp
            End Select
            Return temp
        End Get
        Set(ByVal value As String)
            Me.TXB_search.Text = value
        End Set
    End Property
    Private Property ActualTopicId() As System.Guid Implements COL_Wiki.WikiNew.IViewWikiUC.ActualTopicId
        Get
            If Not IsNothing(Session("Wiki_TopicId")) Then
                Try
                    Dim GuidStr As String = Session("Wiki_TopicId")
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
            Session("Wiki_TopicId") = value.ToString
        End Set
    End Property
    Private Property ActualSezioneId() As System.Guid Implements COL_Wiki.WikiNew.IViewWikiUC.ActualSezioneId
        Get
            If Not IsNothing(Session("Wiki_SezioneId")) Then
                Try
                    Dim GuidStr As String = Session("Wiki_SezioneId")
                    Return New System.Guid(GuidStr)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As System.Guid)
            Session("Wiki_SezioneId") = value.ToString
        End Set
    End Property

    Private Property Servizio() As UCServices.Services_Wiki Implements COL_Wiki.WikiNew.IViewWikiUC.Servizio
        Get
            Return _Servizio
        End Get
        Set(ByVal value As UCServices.Services_Wiki)
            _Servizio = value
        End Set
    End Property

    Private Sub Wiki_links_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.oPresenter = New COL_Wiki.WikiNew.PresenterWikiUC(Me, PageUtility.CurrentContext, COL_Wiki.FactoryWiki.ConnectionType.SQL, True)
    End Sub
    Public Sub Show(ByVal Content As COL_Wiki.WikiNew.VisualizzazioniUC) Implements COL_Wiki.WikiNew.IViewWikiUC.Show
        'Verifica se attivare i pulsanti avanti indetro della paginazione
        If PageIndex = 1 Then
            Me.IMG_indietro.Visible = False
        Else
            Me.IMG_indietro.Visible = True
        End If
        If IsLastPage = True Then
            Me.IMG_avanti.Visible = False
        Else
            Me.IMG_avanti.Visible = True
        End If

        Select Case Content
            Case COL_Wiki.WikiNew.VisualizzazioniUC.NoPermessi
                Me.MultiView.ActiveViewIndex = 1

            Case COL_Wiki.WikiNew.VisualizzazioniUC.NoTopic
                Me.MultiView.ActiveViewIndex = 0

            Case COL_Wiki.WikiNew.VisualizzazioniUC.ListaTopicSearched
                Me.MultiView.ActiveViewIndex = 2
        End Select
    End Sub

#Region "Binding"

    Public Sub BindTopicsFunded(ByVal Topics As System.Collections.IList) Implements COL_Wiki.WikiNew.IViewWikiUC.BindTopicsFunded
        Me.DLS_result.DataSource = Topics
        Me.DLS_result.DataBind()
        Me.PNL_search.Visible = True
    End Sub

#End Region

#Region "PageBase"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides Sub BindDati()
        Me.oPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.Reset)
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Show(COL_Wiki.WikiNew.VisualizzazioniUC.NoPermessi)
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
            .setLabel(LBL_NoPermessi) '         Non si dispone dei permessi necessari per visualizzare la pagina.
            '.setLabel(LBL_Nav_NoSezione) '      Nessuna sezione presente
            '.setLabel(LBL_Navigatore_t) '       Elenco(sezioni)
            .setLabel(LBL_Con_SezioneNO) '      Nessuna sezione presente
            '.setLabel(LBL_NoTopic_t) '          Nessun Topic presente
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region


    Private Sub BTN_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_search.Click
        IsLastPage = False
        PageIndex = 1
        If (CBX_comunita.Checked = False) And (CBX_pubblici.Checked = True) Then
            Me.oPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.TopicSearch)
        End If
        If (CBX_comunita.Checked = True) And (CBX_pubblici.Checked = True) Then
            Me.oPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.TopicSearchComPub)
        End If
        If (CBX_comunita.Checked = True) And (CBX_pubblici.Checked = False) Then
            Me.oPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.TopicSearchComunita)
        End If
    End Sub
    Public ReadOnly Property MyPersonaCorrente() As COL_BusinessLogic_v2.CL_persona.COL_Persona Implements COL_Wiki.IViewGenerico.MyPersonaCorrente
        Get
            Return MyBase.UtenteCorrente
        End Get
    End Property
    Public ReadOnly Property SezioneDescrizione() As String Implements COL_Wiki.WikiNew.IViewWikiUC.SezioneDescrizione
        Get

        End Get
    End Property

    Public ReadOnly Property SezioneIsDefault() As Boolean Implements COL_Wiki.WikiNew.IViewWikiUC.SezioneIsDefault
        Get

        End Get
    End Property

    Public ReadOnly Property SezioneIsPubblica() As Boolean Implements COL_Wiki.WikiNew.IViewWikiUC.SezioneIsPubblica
        Get
        End Get
    End Property

    Public ReadOnly Property SezioneNome() As String Implements COL_Wiki.WikiNew.IViewWikiUC.SezioneNome
        Get

        End Get
    End Property
    Public ReadOnly Property TopicIsCancellato() As Boolean Implements COL_Wiki.WikiNew.IViewWikiUC.TopicIsCancellato
        Get

        End Get
    End Property

    Public ReadOnly Property TopicIsPubblico() As Boolean Implements COL_Wiki.WikiNew.IViewWikiUC.TopicIsPubblico
        Get

        End Get
    End Property

    Public ReadOnly Property TopicNome() As String Implements COL_Wiki.WikiNew.IViewWikiUC.TopicNome
        Get


        End Get
    End Property

    Public ReadOnly Property TopicText() As String Implements COL_Wiki.WikiNew.IViewWikiUC.TopicText
        Get

        End Get
    End Property

    Private Sub Wiki_links_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Me.TXB_search.Text = Me.HDNdefaultSearch.Value
        End If
    End Sub

    Protected Sub DLS_result_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DLS_result.SelectedIndexChanged

    End Sub

    Protected Sub IMG_indietro_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMG_indietro.Click
        PageIndex = PageIndex - 1
        If PageIndex < 1 Then
            PageIndex = 1
        End If
        If (CBX_comunita.Checked = False) And (CBX_pubblici.Checked = True) Then
            Me.oPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.TopicSearch)
        End If
        If (CBX_comunita.Checked = True) And (CBX_pubblici.Checked = True) Then
            Me.oPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.TopicSearchComPub)
        End If
        If (CBX_comunita.Checked = True) And (CBX_pubblici.Checked = False) Then
            Me.oPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.TopicSearchComunita)
        End If
    End Sub

    Protected Sub IMG_avanti_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMG_avanti.Click
        If IsLastPage = False Then
            PageIndex = PageIndex + 1
        End If
        If (CBX_comunita.Checked = False) And (CBX_pubblici.Checked = True) Then
            Me.oPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.TopicSearch)
        End If
        If (CBX_comunita.Checked = True) And (CBX_pubblici.Checked = True) Then
            Me.oPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.TopicSearchComPub)
        End If
        If (CBX_comunita.Checked = True) And (CBX_pubblici.Checked = False) Then
            Me.oPresenter.BindDati(COL_Wiki.WikiNew.BindingUC.TopicSearchComunita)
        End If
    End Sub

    Public Property ActualView() As COL_Wiki.WikiNew.VisualizzazioniUC Implements COL_Wiki.WikiNew.IViewWikiUC.ActualView
        Get
            Dim Out As COL_Wiki.WikiNew.VisualizzazioniUC
            Try
                If Not IsNothing(ViewState("WikiView")) Then
                    Out = CInt(ViewState("WikiView"))
                Else
                    Out = COL_Wiki.WikiNew.VisualizzazioniUC.PageNotRender
                End If
            Catch ex As Exception
                Out = COL_Wiki.WikiNew.VisualizzazioniUC.PageNotRender
            End Try
            Return Out
        End Get
        Set(ByVal value As COL_Wiki.WikiNew.VisualizzazioniUC)
            ViewState("WikiView") = value
        End Set
    End Property
End Class