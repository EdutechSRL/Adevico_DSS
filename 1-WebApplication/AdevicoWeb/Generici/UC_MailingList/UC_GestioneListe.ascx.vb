Public Partial Class UC_GestioneListe
    Inherits BaseControlWithLoad
    Implements IviewLsMail_Gestione

    Event Modify(ByVal IdLista As Integer)
    Public Event Selected(ByVal IdLista As System.Collections.Generic.List(Of Integer))

    Private oPresenter As PresenterLsMail_Gestione

    Private Sub UC_GestioneListe_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        oPresenter = New PresenterLsMail_Gestione(Me)
    End Sub


#Region "Property IviewLsMail_Gestione"
    Public Property NewName() As String Implements IviewLsMail_Gestione.NewName
        Get
            Return Me.TXB_NewListaName.Text
        End Get
        Set(ByVal value As String)
            Me.TXB_NewListaName.Text = value
        End Set
    End Property
    Public Sub SetLista(ByVal items As System.Collections.IList) Implements IviewLsMail_Gestione.SetLista
        Me.GRVListe.DataSource = items
        Me.GRVListe.DataBind()
    End Sub
    Public Property OrderDir() As String Implements IviewLsMail_Gestione.OrderDir
        Get
            Return ViewState("SortingDir")
        End Get
        Set(ByVal value As String)
            ViewState("SortingDir") = value
        End Set
    End Property

#Region "Gestione messaggi <- Rivedere con ShowAlertMessage generico"
    Public Sub HideMessage() Implements IviewLsMail_Gestione.HideMessage
        Me.LBLMessage.Text = ""
        Me.LBLMessage.Visible = False
    End Sub

    Public Sub ShowLista() Implements IviewLsMail_Gestione.ShowLista
        Me.GRVListe.Visible = True
    End Sub
    Public Sub ShowNoData() Implements IviewLsMail_Gestione.ShowNoData
        Me.GRVListe.Visible = False
        Me.LBLMessage.Visible = True
        Me.LBLMessage.Text = "Nessuna lista presente."
    End Sub
    Public Sub ShowEliminaOk() Implements IviewLsMail_Gestione.ShowEliminaOk
        Me.LBLMessage.Text = "Eliminato con successo."
        Me.LBLMessage.Visible = True
    End Sub
    Public Sub ShowErrorElimina() Implements IviewLsMail_Gestione.ShowErrorElimina
        Me.LBLMessage.Text = "Errore nell'eliminazione."
        Me.LBLMessage.Visible = True
    End Sub
    Public Sub ShowErrorInsert() Implements IviewLsMail_Gestione.ShowErrorInsert
        Me.LBLMessage.Text = "Errore nell'inserimento"
        Me.LBLMessage.Visible = True
    End Sub
    Public Sub ShowInsertOk() Implements IviewLsMail_Gestione.ShowInsertOk
        Me.LBLMessage.Text = "Inserimento avvenuto con successo"
        Me.LBLMessage.Visible = True
    End Sub

    Public Sub ShowAlertMessage(ByVal message As String) Implements IviewLsMail_Gestione.ShowAlertMessage
        Me.LBLMessage.Text = System.Web.HttpUtility.HtmlEncode(message)
        Me.LBLMessage.Visible = True
    End Sub
#End Region

#End Region
#Region "Eventi pagina"
    Private Sub BTN_AddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_AddNew.Click
        Dim NewId As Integer
        NewId = Me.oPresenter.AddNewLista()
        RaiseEvent Modify(NewId)
    End Sub

    'Private Sub GRVListe_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVListe.RowCommand
    '    Dim id As Integer
    '    Try
    '        If e.CommandName = "Insert" Then
    '            id = CInt(Me.GRVListe.DataKeys(CInt(e.CommandArgument)).Value)
    '            RaiseEvent Selected(id)
    '            'Dim oMailingManager As MailingManager
    '            'Dim str As String = MailingManager.GetCCNString(id)

    '            'Me.ShowAlertMessage(str)
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub GRVListe_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVListe.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim LkbMod, LkbDel As LinkButton
            Try
                LkbMod = CType(e.Row.Cells(0).Controls(0), LinkButton)
            Catch ex As Exception
            End Try
            If Not IsNothing(LkbMod) Then
                Me.Resource.setImageButton_GridView("GRVListe", Me.BaseUrl, LkbMod, "Modifica", True, True, True, False)
            End If

            Try
                LkbDel = CType(e.Row.Cells(1).Controls(0), LinkButton)
            Catch ex As Exception
            End Try
            If Not IsNothing(LkbDel) Then
                Me.Resource.setImageButton_GridView("GRVListe", Me.BaseUrl, LkbDel, "Delete", True, True, True, True)
            End If
        End If

        Dim SelectedId As System.Collections.Generic.List(Of Integer)
        SelectedId = Me.ViewState("SelectedId")
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
                        id = Me.GRVListe.DataKeys(e.Row.RowIndex).Value()
                        Cbx.Checked = SelectedId.Contains(id)
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub GRVListe_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GRVListe.RowDeleting
        Dim Indice As Integer
        Try
            'If Me.GRVListe.PageIndex = 0 Then
            Indice = e.RowIndex
            'Else
            '   Indice = (Me.GRVListe.PageSize * Me.GRVListe.PageIndex) + e.RowIndex
            'End If
        Catch ex As Exception
        End Try

        Me.oPresenter.DeleteLista(Me.GRVListe.DataKeys(Indice).Value)

    End Sub
    Private Sub GRVListe_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GRVListe.RowEditing
        RaiseEvent Modify(Me.GRVListe.DataKeys(e.NewEditIndex).Value)
    End Sub
    Private Sub GRVListe_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GRVListe.Sorting
        Me.oPresenter.ReorderList(e.SortExpression)
    End Sub
#End Region
#Region "Generici implementati"
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_GestioneListe", "Generici", "UC_MailingList")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBL_NewLista_t)

            .setButton(Me.BTN_AddNew, True, False, False, True)
            .setButton(Me.BTN_Inserisci, True, False, False, True)

            .setHeaderGridView(Me.GRVListe, 0, "M", True)
            .setHeaderGridView(Me.GRVListe, 1, "C", True)
            .setHeaderGridView(Me.GRVListe, 2, "LISTA_Nome", True)

        End With
    End Sub
#End Region
#Region "Da rivedere..."
    Public Overrides Sub BindDati()
        oPresenter.Initialize()
        Me.LBLMessage.Visible = False

        Me.SetInternazionalizzazione()
    End Sub
#End Region

    ''' <summary>
    '''  Aggiorna la lista e tiene traccia degli ID selezionati
    ''' </summary>
    ''' <param name="SelectedId">Lista degli ID selezionati</param>
    ''' <remarks></remarks>
    Public Sub Aggiorna(Optional ByVal SelectedId As System.Collections.Generic.List(Of Integer) = Nothing)
        If Not IsNothing(SelectedId) Then
            Me.ViewState("SelectedId") = SelectedId
        End If
        oPresenter.RefreshLista()
    End Sub
    Public Property Visible() As Boolean
        Get
            Return Me.PNLContenitore.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.PNLContenitore.Visible = value
        End Set
    End Property

    Protected Sub BTN_Inserisci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_Inserisci.Click
        Dim IdList As New System.Collections.Generic.List(Of Integer)
        Dim id As Integer
        Dim Flag As Boolean
        For Each row As GridViewRow In Me.GRVListe.Rows
            Flag = False
            Dim cbx As CheckBox
            Try
                cbx = row.Cells(3).FindControl("CBX_Selected")
                Flag = cbx.Checked
            Catch ex As Exception
            End Try

            If Flag Then
                id = Me.GRVListe.DataKeys(row.RowIndex).Value()
                IdList.Add(id)
            End If
        Next

        Me.ViewState("SelectedId") = IdList

        RaiseEvent Selected(IdList)
    End Sub

    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
End Class