Partial Public Class UC_ListaUtenti
    Inherits BaseControlWithLoad
    Implements IviewLsMail_interni

    Public ListaComunita As List(Of Integer)

#Region "Generici"
    Public Event Cancel()
    Public Event Added(ByVal IdList As System.Collections.Generic.List(Of Integer))

    Private oPresenter As PresenterLsMail_Interni

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        oPresenter = New PresenterLsMail_Interni(Me)
    End Sub
#End Region

#Region "Binding"
    Private Sub BindFiltroComunita()
        If Me.isPortalCommunity Then
            Dim oServizio As New UCServices.Services_Mail
            Me.CTRLsorgenteComunita.ServizioCode = UCServices.Services_Mail.Codex
            Me.CTRLsorgenteComunita.SetupControl()
            Me.CTRLsorgenteComunita.Visible = True
        Else
            Dim ActualCommunityId As Integer
            ActualCommunityId = Me.ComunitaCorrenteID

            Dim oServizio As New UCServices.Services_Mail
            Me.CTRLsorgenteComunita.ServizioCode = UCServices.Services_Mail.Codex
            Me.CTRLsorgenteComunita.SetupControl(ActualCommunityId)
            Me.CTRLsorgenteComunita.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Imposta la lista
    ''' </summary>
    ''' <param name="Items">Elenco dei nominativi da visualizzare</param>
    ''' <remarks></remarks>
    Public Sub SetLista(ByVal Items As System.Collections.IList) Implements IviewLsMail_interni.SetLista
        Me.GRVInterni.PageSize = Me.DDLNumRec.SelectedValue

        Me.GRVInterni.DataSource = Items
        Me.GRVInterni.DataBind()

        Me.ShowMessageToPage("")

    End Sub
#End Region

#Region "System implemented - Internazionalizzazione, bind generico, ..."
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_Interni", "Generici", "UC_MailingList")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource

            .setLabel(Me.LBL_RecPerPage_t)
            .setLabel(Me.LBnorecord)
            Me.LBnorecord.Text = "<br/>" & Me.LBnorecord.Text & "<br/>"

            .setHeaderGridView(Me.GRVInterni, 0, "Cognome", True)
            .setHeaderGridView(Me.GRVInterni, 1, "Nome", True)
            .setHeaderGridView(Me.GRVInterni, 2, "Mail", True)
            .setHeaderGridView(Me.GRVInterni, 3, "Seleziona", True)

            .setLabel(Me.LBL_R_Cerca_t)
            .setLabel(Me.LBL_R_Cognome_t)
            .setLabel(Me.LBL_R_Login_t)
            .setLabel(Me.LBL_R_Mail_t)
            .setLabel(Me.LBL_R_Matricola_t)
            .setLabel(Me.LBL_R_Nome_t)
            .setLabel(Me.LBL_R_Tipo_t)

            .setCheckBox(Me.CBX_R_Comunita)

            .setButton(Me.BTN_R_Submit)
            .setButton(BTN_AnnullaInt)
            .setButton(BTN_ConfermaSel)

        End With
    End Sub
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides Sub BindDati()
        Me.OrderDir = "asc"
        Me.OldOrderExpr = "Cognome"

        Me.Bind_TipoRuoloFiltro()
    End Sub

    Public Sub ShowMessageToPage(ByVal errorMessage As String) Implements IviewLsMail_interni.ShowMessageToPage
        Me.LBL_Message.Text = errorMessage
        If errorMessage = "" Then
            LBL_Message.Visible = False
        Else
            LBL_Message.Visible = True
        End If
    End Sub
    Public Sub Bind()
        Me.BindFiltroComunita()
    End Sub
#End Region

#Region "Property - FILTRI - ORDINAMENTO - VIEW"
    Public ReadOnly Property SelectedComunitaId() As Integer Implements IviewLsMail_interni.SelectedComunitaId
        Get
            Dim IdComunita As Integer
            'If Me.CTRLsorgenteComunita.HasComunita Then
            IdComunita = Me.CTRLsorgenteComunita.ComunitaID
            If Not (IdComunita >= 0) Then
                IdComunita = Me.ComunitaCorrenteID
            End If
            Return IdComunita
        End Get
    End Property
    Public Property AddressId() As Integer Implements IviewLsMail_interni.AddressId
        Get
            Try
                Return CInt(ViewState("ListaId"))
            Catch ex As Exception
                Return -1
            End Try
        End Get
        Set(ByVal value As Integer)
            ViewState("ListaId") = value
        End Set
    End Property
    Private Sub Bind_TipoRuoloFiltro()
        Me.DDLtipoPersona.Items.Clear()
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oComunita As New COL_BusinessLogic_v2.Comunita.COL_Comunita
            oComunita.Id = Me.CTRLsorgenteComunita.ComunitaID

            oDataset = oComunita.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForUtenti_NoGuest)
            Totale = oDataset.Tables(0).Rows.Count()
            If Totale > 0 Then
                Totale = Totale - 1
                For i = 0 To Totale
					Dim oListItem As New ListItem
                    If IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRL_nome")) Then
                        oListItem.Text = "--"
                    Else
                        oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRL_nome")
                    End If
                    oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRL_ID")
                    Me.DDLtipoPersona.Items.Add(oListItem)
                Next
            End If
        Catch ex As Exception
        End Try

        Me.DDLtipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
        Resource.setDropDownList(Me.DDLtipoPersona, -1)
    End Sub

    Public Property OrderDir() As String Implements IviewLsMail_interni.OrderDir
        Get
            Dim str As String
            Try
                str = Me.ViewState("OrderDir")
            Catch ex As Exception
                str = "asc"
            End Try
            If IsNothing(str) Then
                str = "asc"
            End If
            If str = "" Then
                str = "asc"
            End If
            Return str
        End Get
        Set(ByVal value As String)
            Me.ViewState("OrderDir") = value
        End Set
    End Property
    Public Property OldOrderExpr() As String Implements IviewLsMail_interni.OldOrderExpr
        Get
            Try
                Return Me.ViewState("ShortExpr")
            Catch ex As Exception
                Return "Cognome"
            End Try
        End Get
        Set(ByVal value As String)
            Me.ViewState("ShortExpr") = value
        End Set
    End Property

    Private Property SelectedId() As System.Collections.Generic.List(Of Integer)
        Get
            If ViewState("SelecteUserID") Is Nothing Then
                Return New List(Of Integer)
            Else
                Return ViewState("SelecteUserID")
            End If
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of Integer))
            ViewState("SelecteUserID") = value
        End Set
    End Property

    ''' <summary>
    ''' Passa il valore del controllo comunità
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property HasComunita() As Boolean
        Get
            Return Me.CTRLsorgenteComunita.HasComunita
        End Get
    End Property
#End Region

#Region "Navigazione griglia e visualizzazioni"

#Region "Gestione eventi"
    Private Sub DDLNumRec_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLNumRec.SelectedIndexChanged
        Me.SaveCurrent() 'IMPORTANTE PER NON PERDERE LE SELEZIONI AL CAMBIO DI PAGINA!!!
        Me.DoSearch()
    End Sub
    Private Sub GRVInterni_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GRVInterni.Sorting
        If Me.OldOrderExpr = e.SortExpression Then
            If Me.OrderDir = "asc" Then
                Me.OrderDir = "desc"
            Else
                Me.OrderDir = "asc"
            End If
        Else
            Me.OrderDir = "asc"
        End If
        Me.OldOrderExpr = e.SortExpression

        DoSearch(e.SortExpression, Me.OrderDir)
    End Sub
    Private Sub BTN_R_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_R_Submit.Click
        DoSearch(Me.GRVInterni.SortExpression, Me.GRVInterni.SortDirection)
    End Sub
#End Region

#Region "Visualizzazioni"
    ''' <summary>
    ''' Visualizza la griglia
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Showgriglia() Implements IviewLsMail_interni.Showgriglia
        Me.GRVInterni.Visible = True
        Me.LBnorecord.Visible = False
        Me.LBL_Message.Visible = False
    End Sub

    Public Sub ShowNoItem() Implements IviewLsMail_interni.ShowNoItem
        Me.GRVInterni.Visible = False
        Me.LBnorecord.Visible = True
        Me.LBL_Message.Visible = False
    End Sub

    Public Sub ShowNoParameter() Implements IviewLsMail_interni.ShowNoParameter
        Me.GRVInterni.Visible = False
        Me.LBL_Message.Text = Resource.getValue("Message.PochiParametri")
        Me.LBL_Message.Visible = True
    End Sub
#End Region

    ''' <summary>
    '''  Salva lo stato di selezione della schermata corrente.
    ''' </summary>
    ''' <remarks>
    ''' Da eseguire ogni volta che si aggiorna la lista o selezioni precedenti andranno perse.
    ''' Da eseguire anche prima di inviare la lista degli ID selezionati
    ''' </remarks>
    Private Sub SaveCurrent() Implements IviewLsMail_interni.SaveCurrent
        'Dim IdUser As New System.Collections.Generic.List(Of Integer)
        Dim id As Integer
        Dim Flag As Boolean
        For Each row As GridViewRow In Me.GRVInterni.Rows
            Flag = False
            Dim cbx As CheckBox
            Try
                cbx = row.Cells(3).FindControl("CBX_Selected")
                Flag = cbx.Checked
            Catch ex As Exception
            End Try

            Dim Indice As Integer

            If Me.GRVInterni.PageIndex = 0 Then
                Indice = row.RowIndex
            Else
                Indice = (Me.GRVInterni.PageSize * Me.GRVInterni.PageIndex) + row.RowIndex
            End If

            id = Me.GRVInterni.DataKeys(row.RowIndex).Value()

            If Flag Then 'E' selezionato
                If Not Me.SelectedId.Contains(id) Then 'se non c'e' ed è selezionato viene aggiunto
                    Me.SelectedId.Add(id)
                End If
            Else
                If Me.SelectedId.Contains(id) Then 'se c'e' ma non è selezionato, viene tolto
                    Me.SelectedId.Remove(id)
                End If
            End If
        Next
    End Sub
    ''' <summary>
    ''' Inizializza il controllo, impostando eventuali ID già selezionati.
    ''' </summary>
    ''' <param name="IdList"></param>
    ''' <remarks>
    ''' Verranno aggiornati solo gli ID mostrati nella lista. Gli ID selezionati in più non verrano toccati.
    ''' In questo modo verranno restituiti solo gli ID corretti, cioè quelli selezionati nella lista (senza quelli deselezionati) più quelli che vengono qui passati e non sono nella lista.
    ''' </remarks>
    Public Sub SetSelected(ByVal IdList As Generic.List(Of Integer))
        Me.SelectedId = IdList
        Me.oPresenter.Initialize()
    End Sub
    ''' <summary>
    ''' Controlla i parametri di ricerca e li passa al presenter
    ''' </summary>
    ''' <param name="sortExpr"></param>
    ''' <param name="sortDir"></param>
    ''' <remarks></remarks>
    Private Sub DoSearch(Optional ByVal sortExpr As String = "Cognome", Optional ByVal sortDir As String = "asc")
        Me.SaveCurrent()

        Dim CmntID As Integer = -1
        If Me.CBX_R_Comunita.Checked = True Then
            Try
                CmntID = Me.CTRLsorgenteComunita.ComunitaID
            Catch ex As Exception
            End Try
        End If
        Dim TipoPersonaID As Integer = -1
        Try
            TipoPersonaID = Me.DDLtipoPersona.SelectedValue
        Catch ex As Exception

        End Try

        Me.oPresenter.Cerca(CmntID, Me.TXB_R_Nome.Text, Me.TXB_R_Cognome.Text, Me.TXB_R_Matricola.Text, Me.TXB_R_Mail.Text, Me.TXB_R_Login.Text, TipoPersonaID, sortExpr, sortDir)
    End Sub

#End Region

#Region "Eventi pagina - GridView"
    Protected Sub BTN_ConfermaSel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_ConfermaSel.Click
        Me.SaveCurrent() 'NECESSARIO: aggiorna la lista selectedID 
        RaiseEvent Added(Me.SelectedId)
    End Sub
    Private Sub BTN_AnnullaInt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_AnnullaInt.Click
        RaiseEvent Cancel()
    End Sub

    Private Sub GRVInterni_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVInterni.PageIndexChanging
        Me.GRVInterni.PageIndex = e.NewPageIndex
        DoSearch()
    End Sub
    Private Sub GRVInterni_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVInterni.RowDataBound
        'CBX_Selected
        'Dim _SelectedId As System.Collections.Generic.List(Of Integer)
        '_SelectedId = Me.SelectedId  'Me.ViewState("SelectedId")

        Dim id As Integer
        Dim Cbx As CheckBox
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Not IsNothing(SelectedId) Then
                If SelectedId.Count > 0 Then
                    Try
                        Cbx = e.Row.Cells(3).FindControl("CBX_Selected")
                    Catch ex As Exception
                    End Try
                    If Not IsNothing(Cbx) Then
                        id = Me.GRVInterni.DataKeys(e.Row.RowIndex).Value()
                        Cbx.Checked = SelectedId.Contains(id)
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub CTRLsorgenteComunita_AggiornaDati(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLsorgenteComunita.AggiornaDati
        Me.Bind_TipoRuoloFiltro()
    End Sub

    Public Sub reset()
        Me.GRVInterni.DataSource = Nothing
        Me.GRVInterni.DataBind()

        Me.TXB_R_Cognome.Text = ""
        Me.TXB_R_Login.Text = ""
        Me.TXB_R_Mail.Text = ""
        Me.TXB_R_Matricola.Text = ""
        Me.TXB_R_Nome.Text = ""

        Me.ShowNoItem()
    End Sub
#End Region

#Region "anteprima utenti selezionati"

    Private Enum Nominativi
        HideAll = 0
        ShowNoRecord = 1
        Show = 2
    End Enum



    Private Sub BindNominativi()

        Dim oAddress As System.Net.Mail.MailAddress
        Dim AddressList As New System.Net.Mail.MailAddressCollection

        'Dim oMailTmp As COL_BusinessLogic_v2.COL_E_Mail = Me.oMail

        'For Each oAddress In oMailTmp.IndirizziTO
        '    AddressList.Add(oAddress)
        'Next
        'For Each oAddress In oMailTmp.IndirizziCC
        '    AddressList.Add(oAddress)
        'Next
        'For Each oAddress In oMailTmp.IndirizziCCN
        '    AddressList.Add(oAddress)
        'Next

        If AddressList.Count > 0 Then
            Me.NominativiShow(Nominativi.Show)
            Me.RPT_Address.DataSource = AddressList
            Me.RPT_Address.DataBind()
        Else
            Me.NominativiShow(Nominativi.ShowNoRecord)
        End If
    End Sub

    Private Sub NominativiShow(ByVal Element As Nominativi)
        Select Case Element
            Case Nominativi.HideAll
                LBL_Nominativi.Visible = False
                Me.PNL_Destinatari.Visible = False
                Btn_AggiornaDestinatari.Visible = False
            Case Nominativi.Show
                LBL_Nominativi.Visible = True
                LBL_Nominativi.Text = "<br />" & Resource.getValue("Nominativi.Selezionati") & "<br />" & "<br />"
                Me.PNL_Destinatari.Visible = True
                Btn_AggiornaDestinatari.Visible = True
            Case Nominativi.ShowNoRecord
                LBL_Nominativi.Visible = True
                LBL_Nominativi.Text = "<br />" & Resource.getValue("Nominativi.NoSelezione") & "<br />" & "<br />"
                Me.PNL_Destinatari.Visible = False
                Btn_AggiornaDestinatari.Visible = True
        End Select
    End Sub

    Private Sub CBvisualizzaSelezionati_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBvisualizzaSelezionati.CheckedChanged
        'SalvaIdListe()
        'Me.oPresenter.BindListe()

        If Me.CBvisualizzaSelezionati.Checked Then
            Me.BindNominativi()
        Else
            Me.NominativiShow(Nominativi.HideAll)
        End If
    End Sub

#End Region

    Private Sub Btn_AggiornaDestinatari_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_AggiornaDestinatari.Click
        Me.SaveCurrent()
        Me.BindNominativi()

    End Sub
End Class