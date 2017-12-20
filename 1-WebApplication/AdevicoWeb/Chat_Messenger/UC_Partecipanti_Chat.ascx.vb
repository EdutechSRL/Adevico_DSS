Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.Chat

Public Class UC_Partecipanti_Chat
    Inherits System.Web.UI.UserControl
    Protected oResource As ResourceManager

#Region "Oggetti"
    'Pan admin
    Protected WithEvents PanAdmin As System.Web.UI.WebControls.Panel

    'Pan writer
    'Protected WithEvents PanWrite As System.Web.UI.WebControls.Panel
    Protected WithEvents LBUtenti As System.Web.UI.WebControls.ListBox
    Protected WithEvents IMBaggiorna As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IMBblocca As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IMBsblocca As System.Web.UI.WebControls.ImageButton

    'Altro
    Dim oMessaggi As New WS_Chat.WS_ChatSoapClient
    'Protected WithEvents Pan_UTWrite As System.Web.UI.WebControls.Panel
    Protected WithEvents Pan_UTAdmin As System.Web.UI.WebControls.Panel
    Protected WithEvents BAMSetLvl As System.Web.UI.WebControls.Button
    Protected WithEvents DDLLivello As System.Web.UI.WebControls.DropDownList
    Dim NumUtenti As Integer = 0

    Public Property IDCom() As Integer
        Get
            Return CInt(ViewState("IDCom"))
        End Get
        Set(ByVal Value As Integer)
            ViewState("IDCom") = Value
        End Set
    End Property

#End Region

#Region " Codice generato da Progettazione Web Form "



    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"), "page_UC_Partecipanti_Chat")
        End If

        If Not Page.IsPostBack Then
            Me.SetupInternazionalizzazione()
        End If
      
    End Sub

#Region "Metodi e proprieta' Pubbliche"
    'Optional ByVal ElencoRuoli_ID As String = ""
    Public Sub Refresh(ByVal CMNT_ID As Integer)
        Me.IDCom = CMNT_ID
        Dim oDataset As New DataSet
        Dim PRSN_ID As Integer

        Try
            Dim oDatasetBloccati As New DataSet

            PRSN_ID = Session("objPersona").Id
            'CMNT_ID = Session("IdComunita")

            oDataset = oMessaggi.RecuperaListaUtenti(PRSN_ID, CMNT_ID).Copy
            oDatasetBloccati = oMessaggi.GetBlockFrom(PRSN_ID)

            Dim row As DataRow
            For Each row In oDataset.Tables(0).Rows
                For Each RowBlock As DataRow In oDatasetBloccati.Tables(0).Rows
                    If RowBlock(1) = row.Item("ID") Then
                        row.Item("Nome") = "B - " & row.Item("Nome")
                    End If
                Next
                Select Case row.Item("UtLvl")
                    Case 4
                        row.Item("Nome") = "A - " & row.Item("Nome")
                    Case 2
                        row.Item("Nome") = "W - " & row.Item("Nome")
                    Case 1
                        row.Item("Nome") = "R - " & row.Item("Nome")
                    Case 0
                        row.Item("Nome") = "O - " & row.Item("Nome")
                End Select

            Next
            NumUtenti = oDataset.Tables(0).Rows.Count
            oDataset.AcceptChanges()

            With LBUtenti
                .DataSource = oDataset.Tables("DTUtenti")
                .DataTextField = "Nome"
                .DataValueField = "ID"
                .SelectionMode = ListSelectionMode.Multiple
                .DataBind()
            End With
        Catch ex As Exception

        End Try


    End Sub

    'Recupera lista utenti selezionati
    Public ReadOnly Property GetUtenti() As DataTable
        Get
            Dim DtUtTemp As New DataTable
            Dim DcUtTemp As DataColumn
            Dim DrUtTemp As DataRow

            ' I colonna: nome utente
            DcUtTemp = New DataColumn
            DcUtTemp.DataType = System.Type.GetType("System.String")
            DcUtTemp.ColumnName = "Nome"
            DtUtTemp.Columns.Add(DcUtTemp)

            ' II colonna: ID utente
            DcUtTemp = New DataColumn
            DcUtTemp.DataType = System.Type.GetType("System.Int32")
            DcUtTemp.ColumnName = "ID"
            DtUtTemp.Columns.Add(DcUtTemp)
            DtUtTemp.AcceptChanges()

            For i As Integer = 0 To (Me.LBUtenti.Items.Count - 1)
                If Me.LBUtenti.Items(i).Selected Then
                    DrUtTemp = DtUtTemp.NewRow
                    DrUtTemp.Item("ID") = LBUtenti.Items(i).Value()
                    DrUtTemp.Item("Nome") = LBUtenti.Items(i).Text.Remove(0, 3)
                    DtUtTemp.Rows.Add(DrUtTemp)
                End If
            Next
            Return DtUtTemp
        End Get
    End Property

    'Tipo di visualizzazione
    Public WriteOnly Property IsAdmin() As Boolean
        Set(ByVal Value As Boolean)
            Me.Pan_UTAdmin.Visible = Value
            'Me.PanAdmin.Visible = Value
        End Set
    End Property
    Public WriteOnly Property IsWriter() As Boolean
        Set(ByVal Value As Boolean)
            'Me.PanWrite.Visible = Value
        End Set
    End Property

    'Utenti connessi
    Public ReadOnly Property NUtenti() As Integer
        Get
            Return Me.NumUtenti
        End Get
    End Property

#End Region

#Region "Pulsanti"
    Private Sub IMBaggiorna_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBaggiorna.Click
        Me.Refresh(IDCom)
    End Sub

    Private Sub IMBblocca_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBblocca.Click
        If Me.LBUtenti.Items.Count <= 0 Then
            Me.Refresh(IDCom)
        Else
            For i As Integer = 0 To Me.LBUtenti.Items.Count - 1
                If Me.LBUtenti.Items(i).Selected Then
                    If Not LBUtenti.Items(i).Text.Substring(2, 3) = "- B" Then
                        If Not LBUtenti.Items(i).Text.Chars(0) = "A" Then 'Se l'utente è amministratore non effettua il blocco...
                            Me.oMessaggi.SetBlock(Session("objPersona").Id, Me.LBUtenti.Items(i).Value)
                        End If
                    End If
                End If
                'me.oMessaggi.SetBlock(Me.LBUtenti.Items(i)
            Next
        End If
        Me.Refresh(Me.IDCom)
    End Sub
    Private Sub IMBsblocca_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBsblocca.Click
        If Me.LBUtenti.Items.Count <= 0 Then
            Me.Refresh(IDCom)
        Else
            For i As Integer = 0 To Me.LBUtenti.Items.Count - 1
                If LBUtenti.Items(i).Selected Then
                    If LBUtenti.Items(i).Text.Substring(2, 3) = "- B" Then
                        Me.oMessaggi.RemBlock(Session("objPersona").Id, Me.LBUtenti.Items(i).Value())
                    End If
                End If
            Next
        End If
        Me.Refresh(Me.IDCom)
    End Sub

    Private Sub BAMSetLvl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BAMSetLvl.Click
        If Me.LBUtenti.Items.Count <= 0 Then
            Me.Refresh(IDCom)
        Else
            For i As Integer = 0 To Me.LBUtenti.Items.Count - 1
                If LBUtenti.Items(i).Selected Then
                    If LBUtenti.Items(i).Text.Substring(2, 3) = "- B" Then
                        oMessaggi.SetLvl(Session("objPersona").Id, LBUtenti.Items(i).Value(), DDLLivello.SelectedValue, Session("IdComunita"))
                    End If
                End If
            Next
        End If
    End Sub

#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String, ByVal ResourcesName As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_UC_Partecipanti_Chat"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setImageButton(Me.IMBaggiorna, True, True, True, False)
            .setImageButton(Me.IMBblocca, True, True, True, False)
            .setImageButton(Me.IMBsblocca, True, True, True, False)
            .setButton(Me.BAMSetLvl, False, False, False, False)
        End With
    End Sub
#End Region

End Class
