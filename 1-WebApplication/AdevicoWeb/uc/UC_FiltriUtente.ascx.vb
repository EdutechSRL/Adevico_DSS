Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class UC_FiltriUtente
    Inherits System.Web.UI.UserControl
    Private oResourceUCutenti As ResourceManager
    Public Event AggiornaDati(ByVal sender As Object, ByVal e As EventArgs)

    Protected WithEvents HDN_CMNT_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_CMNT_Path As System.Web.UI.HtmlControls.HtmlInputHidden

    Public Property ComunitaID() As Integer
        Get
            Try
                ComunitaID = Me.HDN_CMNT_ID.Value
            Catch ex As Exception
                ComunitaID = -1
            End Try
        End Get
        Set(ByVal Value As Integer)
            Me.HDN_CMNT_ID.Value = Value
        End Set
    End Property
    Public Property ComunitaPath() As String
        Get
            Try
                Return Me.HDN_CMNT_Path.Value
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(ByVal Value As String)
            Me.HDN_CMNT_Path.Value = Value
        End Set
    End Property
    Public ReadOnly Property isPaginated() As Boolean
        Get
            Try
                Return (Me.RBLpaginazione.SelectedIndex = 1)
            Catch ex As Exception
                Return False
            End Try
        End Get
    End Property
    Public ReadOnly Property PageSize() As Integer
        Get
            Try
                Return Me.DDLpaginazione.SelectedValue
            Catch ex As Exception
                Return 15
            End Try
        End Get
    End Property
    Public ReadOnly Property RoleSelected() As String
        Get
            Try

                Return Me.RuoliSelezionati
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    Public ReadOnly Property AllRoles() As Boolean
        Get
            Try
                Return (Me.CBXall.Checked Or Me.CBLruoli.SelectedIndex = -1)
            Catch ex As Exception
                Return True
            End Try
        End Get
    End Property

    Public ReadOnly Property AllActivation() As Boolean
        Get
            Try
                Return (Me.CBXattivazione.Checked Or Me.CBLattivazione.SelectedIndex = -1)
            Catch ex As Exception
                Return True
            End Try
        End Get
    End Property
    Public ReadOnly Property ActivationSelected() As String
        Get
            Try

                Return Me.AttivazioniSelezionate
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    Protected WithEvents TBLfiltriUtente As System.Web.UI.WebControls.Table
    Protected WithEvents TBRfiltroPaginazione As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBpaginazioneLegend As System.Web.UI.WebControls.Label
    Protected WithEvents RBLpaginazione As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents TBRpaginazioneDDL As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBpaginazione_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLpaginazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBRinfra0 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRfiltroRuolo As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBruoloLegend As System.Web.UI.WebControls.Label
    Protected WithEvents TBLruoli As System.Web.UI.WebControls.Table
    Protected WithEvents CBLruoli As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents CBXall As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TBRinfra1 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRfiltroAttivazione As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBattivazioneLegend As System.Web.UI.WebControls.Label
    Protected WithEvents TBLattivazione As System.Web.UI.WebControls.Table
    Protected WithEvents CBXattivazione As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBLattivazione As System.Web.UI.WebControls.CheckBoxList

    Protected WithEvents BTNaggiorna As System.Web.UI.WebControls.Button

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
        If IsNothing(Me.oResourceUCutenti) Then
            Me.SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        End If
    End Sub

#Region "Bind Dati"
    Public Sub Bind_Dati(ByVal IdComunita As Integer, ByVal PathComunita As String)
        Me.HDN_CMNT_ID.Value = IdComunita
        Me.HDN_CMNT_Path.Value = PathComunita

        Me.SetCulture(Session("LinguaCode"))
        Me.SetupInternazionalizzazione()
        Me.Bind_TipoRuoloFiltro()
        Me.Bind_Attivazione()
        Me.RBLpaginazione.SelectedIndex = 0
        Me.TBRpaginazioneDDL.Visible = False
        Me.CBLruoli.Enabled = False
        Me.CBXall.Checked = True
        Me.CBLattivazione.Enabled = False
        Me.CBXattivazione.Checked = True
    End Sub

    Public Sub Bind_TipoRuoloFiltro()
        Me.CBLruoli.Items.Clear()
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oComunita As New COL_Comunita

            oComunita.Id = Me.HDN_CMNT_ID.Value
            oDataset = oComunita.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForUtenti_NoGuest)

            Me.CBLruoli.DataSource = oDataset
            Me.CBLruoli.DataTextField = "TPRL_nome"
            Me.CBLruoli.DataValueField = "TPRL_ID"
            Me.CBLruoli.DataBind()

            Me.CBXall.Enabled = True
            Me.CBLruoli.Visible = True
        Catch ex As Exception
            Me.CBXall.Checked = True
            Me.CBXall.Enabled = False
            Me.CBLruoli.Visible = False
        End Try
    End Sub

    Private Sub Bind_Attivazione()
        Dim oComunita As New COL_Comunita

        Try
            oComunita.Id = Me.HDN_CMNT_ID.Value
            Me.CBLattivazione.Items.Clear()

            Me.CBLattivazione.Items.Add(New ListItem("Enabled User", Main.TipoAttivazione.Attivati))

            If oComunita.HasBlockedUsers() Then
                Me.CBLattivazione.Items.Add(New ListItem("Blocked Users", Main.TipoAttivazione.Bloccati))
                Me.oResourceUCutenti.setCheckBoxList(Me.CBLattivazione, Main.TipoAttivazione.Bloccati)
            End If
            If oComunita.HasWaitingUsers() Then
                Me.CBLattivazione.Items.Add(New ListItem("Waiting Users", Main.TipoAttivazione.InAttesa))
                Me.oResourceUCutenti.setCheckBoxList(Me.CBLattivazione, Main.TipoAttivazione.InAttesa)
            End If
            If oComunita.HasNoActivatedUsers() Then
                Me.CBLattivazione.Items.Add(New ListItem("No activated users", Main.TipoAttivazione.AccountNonConfermato))
                Me.oResourceUCutenti.setCheckBoxList(Me.CBLattivazione, Main.TipoAttivazione.AccountNonConfermato)
            End If
            If oComunita.HasNewUsers(Session("objPersona").id, True) Then
                Me.CBLattivazione.Items.Add(New ListItem("New Users", Main.TipoAttivazione.NuoviIscritti))
                Me.oResourceUCutenti.setCheckBoxList(Me.CBLattivazione, Main.TipoAttivazione.NuoviIscritti)
            End If
            Me.oResourceUCutenti.setCheckBoxList(Me.CBLattivazione, Main.TipoAttivazione.Attivati)
            Me.CBXattivazione.Enabled = True
            Me.CBLattivazione.Visible = True
        Catch ex As Exception
            Me.CBXattivazione.Checked = True
            Me.CBXattivazione.Enabled = False
            Me.CBLattivazione.Visible = False

        End Try
    End Sub
#End Region

    Private Function RuoliSelezionati() As String
        Dim ruoli As String = ""
        Dim i, totale As Integer

        Try
            If Me.CBLruoli.SelectedIndex >= 0 Then
                For i = Me.CBLruoli.SelectedIndex To Me.CBLruoli.Items.Count - 1
                    If Me.CBLruoli.Items(i).Selected Then
                        If ruoli = "" Then
                            ruoli = "," & Me.CBLruoli.Items(i).Value & ","
                        Else
                            ruoli = ruoli & Me.CBLruoli.Items(i).Value & ","
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            ruoli = ""
        End Try
        Return ruoli
    End Function

    Private Function AttivazioniSelezionate() As String
        Dim ruoli As String = ""
        Dim i, totale As Integer

        Try
            If Me.CBLattivazione.SelectedIndex >= 0 Then
                For i = Me.CBLattivazione.SelectedIndex To Me.CBLattivazione.Items.Count - 1
                    If Me.CBLattivazione.Items(i).Selected Then
                        If ruoli = "" Then
                            ruoli = "," & Me.CBLattivazione.Items(i).Value & ","
                        Else
                            ruoli = ruoli & Me.CBLattivazione.Items(i).Value & ","
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            ruoli = ""
        End Try
        Return ruoli
    End Function

    Private Sub CBXall_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXall.CheckedChanged
        Me.CBLruoli.Enabled = Not Me.CBXall.Checked
        If Me.CBXall.Checked Then
            Me.CBLruoli.SelectedIndex = -1
            RaiseEvent AggiornaDati(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub RBLpaginazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLpaginazione.SelectedIndexChanged
        Me.TBRpaginazioneDDL.Visible = (Me.RBLpaginazione.SelectedIndex = 1)
        RaiseEvent AggiornaDati(Me, EventArgs.Empty)
    End Sub
    Private Sub CBXattivazione_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXattivazione.CheckedChanged
        Me.CBLattivazione.Enabled = Not Me.CBXattivazione.Checked
        If Me.CBXattivazione.Checked Then
            Me.CBLattivazione.SelectedIndex = -1
            RaiseEvent AggiornaDati(Me, EventArgs.Empty)
        End If
    End Sub

#Region "internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        oResourceUCutenti = New ResourceManager

        oResourceUCutenti.UserLanguages = code
        oResourceUCutenti.ResourcesName = "pg_FiltroUtenti"
        oResourceUCutenti.Folder_Level1 = "UC"
        oResourceUCutenti.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceUCutenti
            .setCheckBox(Me.CBXattivazione)
            .setLabel(Me.LBattivazioneLegend)

            .setCheckBox(Me.CBXall)
            .setLabel(Me.LBruoloLegend)

            .setLabel(Me.LBpaginazione_t)
            .setRadioButtonList(Me.RBLpaginazione, 0)
            .setRadioButtonList(Me.RBLpaginazione, 1)
            .setLabel(Me.LBpaginazioneLegend)
            .setButton(Me.BTNaggiorna, True)
        End With
    End Sub
#End Region


    Private Sub DDLpaginazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLpaginazione.SelectedIndexChanged
        RaiseEvent AggiornaDati(Me, EventArgs.Empty)
    End Sub

    Private Sub BTNaggiorna_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaggiorna.Click
        RaiseEvent AggiornaDati(Me, EventArgs.Empty)
    End Sub

    Public ReadOnly Property SearchButtonClientId As String
        Get
            Return Me.BTNaggiorna.UniqueID
        End Get
    End Property
    Public ReadOnly Property SearchTextField As String
        Get

        End Get
    End Property
End Class