Public Partial Class UC_MailingList
    Inherits BaseControlWithLoad

    Public Event Selected(ByVal IdLista As System.Collections.Generic.List(Of Integer))
    Public Event Undo()

    Private Enum Show
        ElencoListe = 0
        Lista = 1
        AddInterni = 2
    End Enum
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        End If
    End Sub
    Private Property OldSelectedId() As Generic.List(Of Integer)
        Get
            Return Me.ViewState("OldSelection")
        End Get
        Set(ByVal value As Generic.List(Of Integer))
            Me.ViewState("OldSelection") = value
        End Set
    End Property

#Region "visualizzazioni"
    ''' <summary>
    ''' Resetta le visualizzazioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ResetVista()
        ShowUC(UC_MailingList.Show.ElencoListe)
    End Sub
    Private Sub ShowUC(ByVal Elemento As Show)
        Me.Btn_BackLista.Visible = True

        Me.UC_GestioneListe.Visible = False
        Me.UC_Lista.IsVisible = False
        Me.UC_Interni1.Visible = False

        Select Case Elemento
            Case Show.ElencoListe
                Me.Btn_BackLista.Visible = False
                Me.UC_GestioneListe.Visible = True
                Me.UC_GestioneListe.Aggiorna()
            Case Show.Lista
                Me.UC_Lista.IsVisible = True
            Case Show.AddInterni

                Me.UC_Interni1.Visible = True
                'Me.UC_Interni1.Visible = True
        End Select
    End Sub
#End Region
#Region "Eventi pagina"
    Private Sub LKB_BackLista_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_BackLista.Click
        Me.ShowUC(Show.ElencoListe)
    End Sub
    Private Sub btn_Annulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Annulla.Click
        RaiseEvent Undo()
    End Sub
#End Region
#Region "Eventi UserControl"
#Region "UC Gestione Liste"
    Private Sub UC_GestioneListe_Modify(ByVal IdLista As Integer) Handles UC_GestioneListe.Modify
        Me.UC_Lista.IdLista = IdLista
        Me.UC_Lista.Inizializza()

        'Inizializzo le nuove selezioni
        Me.UC_Interni1.AddressId = Me.UC_Lista.IdLista
        Me.UC_Interni1.SetSelected(GetAddressId) '<- Inizializza
        Me.UC_Interni1.reset()

        Me.ShowUC(Show.Lista)
    End Sub
    Private Sub UC_GestioneListe_Selected(ByVal SelectedId As System.Collections.Generic.List(Of Integer)) Handles UC_GestioneListe.Selected
        RaiseEvent Selected(SelectedId)
    End Sub
#End Region
#Region "UC Lista"
    Private Sub UC_Lista_ShowAddInt() Handles UC_Lista.ShowAddInt
        Me.UC_Interni1.AddressId = Me.UC_Lista.IdLista
        Me.UC_Interni1.SetSelected(GetAddressId) '<- Inizializza
        Me.ShowUC(Show.AddInterni)
    End Sub
#End Region
#Region "Uc Interni"
    Private Sub UC_Interni1_Added1(ByVal IdList As System.Collections.Generic.List(Of Integer)) Handles UC_Interni1.Added
        Me.SaveInterni(IdList)
        'Me.UC_Lista.AggiornaInterni(IdList)
        Me.ShowUC(Show.Lista)
        Me.UC_Lista.Inizializza()
    End Sub
    Private Sub UC_Interni1_Cancel() Handles UC_Interni1.Cancel
        Me.ShowUC(Show.Lista)
    End Sub
#End Region
#End Region
#Region "gestione Indirizzi interni"
    Private Function GetAddressId() As Generic.List(Of Integer)
        Dim IdList As New Generic.List(Of Integer)
        Dim oList As Generic.List(Of MailingAddress)
        oList = COL_BusinessLogic_v2.MailingManager.GetIndirizziOrdinati(Me.UC_Lista.IdLista, "PersonaCognome", "asc")
        For Each oAddress As MailingAddress In oList
            If oAddress.PersonaID > 0 Then
                IdList.Add(oAddress.PersonaID)
            End If
        Next
        OldSelectedId = IdList
        Return IdList
    End Function

    ''' <summary>
    '''  Allinea i dati nel DB con la lista di ID passata.
    ''' </summary>
    ''' <param name="IdList">Lista corretta degli ID</param>
    ''' <remarks>
    ''' Viene salvata nella property OldSelectedID la precedente lista di ID, in modo da confrontarla con la nuova selezione ed aggiornare correttamente i dati.
    ''' </remarks>
    Private Sub SaveInterni(ByVal IdList As Generic.List(Of Integer))
        Dim OldSelectedList As Generic.List(Of Integer)
        Try
            OldSelectedList = OldSelectedId
        Catch ex As Exception
        End Try

        If Not IsNothing(OldSelectedList) Then
            If OldSelectedList.Count > 0 Then

                'Controllo nei vecchi e:
                '- Era gia inserito: lo tolgo da quelli da inserire
                '- Non era ancora inserito: lo aggiungo alla lista di quelli da aggiungere
                For Each Id As Integer In OldSelectedList
                    If Id > 0 Then
                        If IdList.Contains(Id) Then
                            IdList.Remove(Id)
                        Else
                            MailingAddress.DeleteInt(Id, Me.UC_Lista.IdLista)
                        End If
                    End If
                Next
            End If
        End If

        'Aggiungo quelli che non sono ancora stati aggiunti
        For Each Id As Integer In IdList
            If Id > 0 Then
                Dim oAddress As New MailingAddress(Id, "", "", "", "", "")
                oAddress.IdLista = Me.UC_Lista.IdLista
                oAddress.SaveNew()
            End If
        Next
    End Sub

    ''' <summary>
    ''' Aggiorna l'elenco delle liste
    ''' </summary>
    ''' <param name="IdListe">Id delle liste selezionate</param>
    ''' <remarks></remarks>
    Public Sub BindSelected(ByVal IdListe As System.Collections.Generic.List(Of Integer))
        Me.UC_GestioneListe.Aggiorna(IdListe)
    End Sub
#End Region

    Public Sub BindInterni()
        Me.UC_Interni1.Bind()
    End Sub

#Region "Proprietà ovverides"
    Public ReadOnly Property HasComunita() As Boolean
        Get
            Return Me.UC_Interni1.HasComunita
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides Sub BindDati()
        Me.ShowUC(Show.ElencoListe)
    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_MailingList.xml", "Generici", "UC_MailingList")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        Resource.setButton(Me.btn_Annulla)
        Resource.setButton(Me.Btn_BackLista)

    End Sub
#End Region
End Class