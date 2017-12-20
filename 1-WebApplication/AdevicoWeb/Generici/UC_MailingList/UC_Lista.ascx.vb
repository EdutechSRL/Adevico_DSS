Partial Public Class UC_Lista
    Inherits BaseControlWithLoad
    Implements IviewLsMail_Lista

    Public Event ShowAddInt()
    Private oPresenter As PresenterLsMail_Lista

    Private Sub UC_Lista_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        oPresenter = New PresenterLsMail_Lista(Me)
    End Sub

#Region "Property iView"
    Public Sub Inizializza()
        Me.oPresenter.Initialize()
    End Sub

    Public Property IdLista() As Integer Implements IviewLsMail_Lista.IdLista
        Get
            Try
                Return CInt(ViewState("IdLista"))
            Catch ex As Exception
                Return -1
            End Try
        End Get
        Set(ByVal value As Integer)
            ViewState("IdLista") = value
        End Set
    End Property
    Public Property IdProprietario() As Integer Implements IviewLsMail_Lista.IdProprietario
        Get
            Return Me.UtenteCorrente.Id
        End Get
        Set(ByVal value As Integer)
            'nothing
        End Set
    End Property

    Public Property Ind_Id() As Integer Implements IviewLsMail_Lista.Ind_Id
        Get
            Try
                Return CInt(ViewState("IdAddress"))
            Catch ex As Exception
                Return 0
            End Try
        End Get
        Set(ByVal value As Integer)
            ViewState("IdAddress") = value
        End Set
    End Property
    Public Property Ind_PrsnId() As Integer Implements IviewLsMail_Lista.Ind_PrsnId
        Get
            Try
                Return CInt(ViewState("IdPrsnModifica"))
            Catch ex As Exception
                Return 0
            End Try
        End Get
        Set(ByVal value As Integer)
            ViewState("IdPrsnModifica") = value
        End Set
    End Property
    Public Property Ind_Titolo() As String Implements IviewLsMail_Lista.Ind_Titolo
        Get
            Return Me.TXB_ITitolo.Text
        End Get
        Set(ByVal value As String)
            Me.TXB_ITitolo.Text = value
        End Set
    End Property
    Public Property Ind_Nome() As String Implements IviewLsMail_Lista.Ind_Nome
        Get
            Return Me.TXB_INome.Text
        End Get
        Set(ByVal value As String)
            Me.TXB_INome.Text = value
        End Set
    End Property
    Public Property Ind_Cognome() As String Implements IviewLsMail_Lista.Ind_Cognome
        Get
            Return Me.TXB_ICognome.Text
        End Get
        Set(ByVal value As String)
            Me.TXB_ICognome.Text = value
        End Set
    End Property
    Public Property Ind_Mail() As String Implements IviewLsMail_Lista.Ind_Mail
        Get
            Return Me.TXB_IMail.Text
        End Get
        Set(ByVal value As String)
            Me.TXB_IMail.Text = value
        End Set
    End Property
    Public Property Ind_Struttura() As String Implements IviewLsMail_Lista.Ind_Struttura
        Get
            Return Me.TXB_IStruttura.Text
        End Get
        Set(ByVal value As String)
            Me.TXB_IStruttura.Text = value
        End Set
    End Property

    Public Property NomeLista() As String Implements IviewLsMail_Lista.NomeLista
        Get
            Return TXB_NomeLista.Text
        End Get
        Set(ByVal value As String)
            TXB_NomeLista.Text = value
        End Set
    End Property

    Public Sub ShowElenco() Implements IviewLsMail_Lista.ShowElenco
        Me.GRVIndirizzi.Visible = True
        Me.PNL_NomeLista.Visible = True
        Me.PNL_InserimentoModifica.Visible = False
        Me.HideMessage()

        Me.BTN_InserisciExt.Visible = True
        Me.BTN_InserisciInt.Visible = True
    End Sub
    Public Sub ShowModifica() Implements IviewLsMail_Lista.ShowModifica
        Me.GRVIndirizzi.Visible = False
        Me.PNL_NomeLista.Visible = False
        Me.PNL_InserimentoModifica.Visible = True
        Me.Resource.setLabel_To_Value(Me.LBL_IAddModify, "Modify")
        If Me.Ind_PrsnId > 0 Then
            Me.TXB_INome.Enabled = False
            Me.TXB_ICognome.Enabled = False
            Me.TXB_IMail.Enabled = False

            Me.BTNinsert.Visible = False
            Me.BTNupdate.Visible = True
            Me.BTNCancel.Visible = True
        Else
            Me.TXB_INome.Enabled = True
            Me.TXB_ICognome.Enabled = True
            Me.TXB_IMail.Enabled = True

            Me.BTNinsert.Visible = False
            Me.BTNupdate.Visible = True
            Me.BTNCancel.Visible = True
        End If

        Me.BTN_InserisciExt.Visible = False
        Me.BTN_InserisciInt.Visible = False
    End Sub
    Public Sub ShowNuovo() Implements IviewLsMail_Lista.ShowNuovo
        Me.PNL_NomeLista.Visible = False

        Me.GRVIndirizzi.Visible = False

        '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        Me.PNL_NomeLista.Visible = False

        Me.PNL_InserimentoModifica.Visible = True
        Me.TXB_INome.Enabled = True
        Me.TXB_ICognome.Enabled = True
        Me.TXB_IMail.Enabled = True

        Me.BTNinsert.Visible = True
        Me.BTNupdate.Visible = False
        Me.BTNCancel.Visible = True

        Me.Resource.setLabel_To_Value(Me.LBL_IAddModify, "New")

        Me.BTN_InserisciExt.Visible = False
        Me.BTN_InserisciInt.Visible = False


    End Sub
    Public Sub ShowNoItem() Implements IviewLsMail_Lista.ShowNoItem
        Me.GRVIndirizzi.Visible = False
        LBLMessage.Visible = True
        Me.Resource.setLabel_To_Value(LBLMessage, "Info.NoItem")
    End Sub

#Region "Show Message"

    Public Sub HideMessage() Implements IviewLsMail_Lista.HideMessage
        LBLMessage.Text = ""
        LBLMessage.Visible = False
    End Sub
    Public Sub ShowAddUserError() Implements IviewLsMail_Lista.ShowAddUserError
        LBLMessage.Text = Me.Resource.getValue("Message.AddUserError")
        LBLMessage.Visible = True
    End Sub
    Public Sub ShowAddUserOk() Implements IviewLsMail_Lista.ShowAddUserOk
        LBLMessage.Text = Me.Resource.getValue("Message.AddUserOK")
        LBLMessage.Visible = True
    End Sub
    Public Sub ShowDelError() Implements IviewLsMail_Lista.ShowDelError
        LBLMessage.Text = Me.Resource.getValue("Message.DeleteError")
        LBLMessage.Visible = True
    End Sub

    Public Sub ShowDelOk() Implements IviewLsMail_Lista.ShowDelOk
        LBLMessage.Text = Me.Resource.getValue("Message.DeleteOK")
        LBLMessage.Visible = True
    End Sub
    Public Sub ShowUpdateError() Implements IviewLsMail_Lista.ShowUpdateError
        LBLMessage.Text = Me.Resource.getValue("Message.UpdateError")
        LBLMessage.Visible = True
    End Sub
    Public Sub ShowUpdateOk() Implements IviewLsMail_Lista.ShowUpdateOk
        LBLMessage.Text = Me.Resource.getValue("Message.UpdateOk")
        LBLMessage.Visible = True
    End Sub

#End Region

    Public Property SortDir() As String Implements IviewLsMail_Lista.SortDir
        Get
            Return ViewState("SortDir")
        End Get
        Set(ByVal value As String)
            ViewState("SortDir") = value
        End Set
    End Property
    Public Property SortExp() As String
        Get
            Return ViewState("SortExpr")
        End Get
        Set(ByVal value As String)
            ViewState("SortExpr") = value
        End Set
    End Property
#End Region

#Region "Lista"
    Public Sub SetLista(ByVal Items As System.Collections.IList) Implements IviewLsMail_Lista.SetLista
        Me.GRVIndirizzi.DataSource = Items
        Me.GRVIndirizzi.DataBind()
    End Sub

    Private Sub GRVIndirizzi_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVIndirizzi.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim LkbMod, LkbDel As LinkButton
            Try
                LkbMod = CType(e.Row.Cells(0).Controls(0), LinkButton)
            Catch ex As Exception
            End Try
            If Not IsNothing(LkbMod) Then
                Me.Resource.setImageButton_GridView("GRVIndirizzi", Me.BaseUrl, LkbMod, "Modifica", True, True, True, False)
            End If

            Try
                LkbDel = CType(e.Row.Cells(1).Controls(0), LinkButton)
            Catch ex As Exception
            End Try
            If Not IsNothing(LkbDel) Then
                Me.Resource.setImageButton_GridView("GRVIndirizzi", Me.BaseUrl, LkbDel, "Delete", True, True, True, True)
            End If
        End If

    End Sub
    Private Sub GRVIndirizzi_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GRVIndirizzi.RowDeleting

        Dim Indice As Integer
        Try
            Indice = e.RowIndex
        Catch ex As Exception
        End Try
        Me.oPresenter.DeleteAddress(Me.GRVIndirizzi.DataKeys(Indice).Value)
    End Sub
    Private Sub GRVIndirizzi_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GRVIndirizzi.RowEditing
        Dim oAddress As New MailingAddress

        Dim Indice As Integer
        If Me.GRVIndirizzi.PageIndex = 0 Then
            Indice = e.NewEditIndex
        Else
            Indice = (Me.GRVIndirizzi.PageSize * Me.GRVIndirizzi.PageIndex) + e.NewEditIndex
        End If

        Me.oPresenter.BindModifica(Me.GRVIndirizzi.DataKeys(Indice).Value)

    End Sub
    Private Sub GRVIndirizzi_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GRVIndirizzi.Sorting
        If Me.SortExp = e.SortExpression Then
            If Me.SortDir = "asc" Then
                Me.SortDir = "desc"
            Else
                Me.SortDir = "asc"
            End If
        Else
            Me.SortExp = e.SortExpression
            Me.SortDir = "asc"
        End If

        Me.oPresenter.ReorderList(e.SortExpression, Me.SortDir)
    End Sub
#End Region

#Region "Eventi Pagina"
    Private Sub BTNupdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNupdate.Click
        If Me.Page.IsValid Then
            Me.oPresenter.ModifyAddress()
        End If
    End Sub
    Private Sub BTNinsert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNinsert.Click
        If Me.Page.IsValid Then
            Me.oPresenter.AddNewAddress()
        End If
    End Sub

    Private Sub BTNCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCancel.Click
        Me.oPresenter.Initialize()
    End Sub
    Private Sub LKB_InserisciExt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_InserisciExt.Click
        RaiseEvent ShowAddInt()
    End Sub
    Private Sub LKB_InserisciInt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_InserisciInt.Click
        Me.oPresenter.BindNew()
    End Sub
    Private Sub BTN_ModificaNome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_ModificaNome.Click
        Me.oPresenter.SalvaNomeLista()
    End Sub
#End Region

#Region "Implemented System"
    Public Sub ShowMessageToPage(ByVal errorMessage As String) Implements IviewLsMail_Lista.ShowMessageToPage
        LBLMessage.Text = errorMessage
        If errorMessage = "" Then
            LBLMessage.Visible = False
        Else
            LBLMessage.Visible = True
        End If
    End Sub
    Public Property IsVisible() As Boolean
        Get
            Return Me.PNLContenitore.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.PNLContenitore.Visible = value
        End Set
    End Property
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_Lista", "Generici", "UC_MailingList")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(Me.LBL_NomeLista_t)

            .setLabel(Me.LBL_ITitolo_t)
            .setLabel(Me.LBL_INome_t)
            .setLabel(Me.LBL_ICognome_t)
            .setLabel(Me.LBL_IMail_t)
            .setLabel(Me.LBL_IStruttura_t)

            .setButton(Me.BTN_InserisciInt, True)
            .setButton(Me.BTN_InserisciExt, True)

            .setButton(Me.BTNCancel, True, False, False, True)
            .setButton(Me.BTNinsert, True, False, False, True)
            .setButton(Me.BTNupdate, True, False, False, True)

            .setRegularExpressionValidator(Me.REV_Mail)

            .setHeaderGridView(Me.GRVIndirizzi, 0, "M", True)
            .setHeaderGridView(Me.GRVIndirizzi, 1, "C", True)
            .setHeaderGridView(Me.GRVIndirizzi, 2, "Titolo", True)
            .setHeaderGridView(Me.GRVIndirizzi, 3, "Nome", True)
            .setHeaderGridView(Me.GRVIndirizzi, 4, "Cognome", True)
            .setHeaderGridView(Me.GRVIndirizzi, 5, "Mail", True)
            .setHeaderGridView(Me.GRVIndirizzi, 6, "Struttura", True)

            .setButton(Me.BTN_ModificaNome, True, True)
            'me.Resource.setLabel_To_Value(LBLMessage,"NoItem")

            'me.Resource.setLabel_To_Value(me.LBL_IAddModify,"New")
            'me.Resource.setLabel_To_Value(me.LBL_IAddModify,"Modify")
        End With
    End Sub
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

End Class