Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Curriculum_Europeo


Public Class UC_ConoscenzaLingua
    Inherits System.Web.UI.UserControl
    Protected oResourceLingua As ResourceManager

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

#Region "accessori"
    Protected WithEvents HDNprsn_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcreu_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcnln_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents PNLinserimento As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLlingua As System.Web.UI.WebControls.Panel
#End Region
#Region "aggiungi"
    '  Protected WithEvents BTNannulla As System.Web.UI.WebControls.Button
    '  Protected WithEvents BTNaggiungi As System.Web.UI.WebControls.Button
    '  Protected WithEvents BTNmodifica As System.Web.UI.WebControls.Button
    Protected WithEvents TXBnome As System.Web.UI.WebControls.TextBox
    Protected WithEvents RBLlettura As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents RBLscrittura As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents RBLespressioneOrale As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents CBXrendiPubblico As System.Web.UI.WebControls.CheckBox

#End Region
#Region "lista"

    Protected WithEvents RPTlingua As System.Web.UI.WebControls.Repeater

    Protected WithEvents LBnome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBnessuna As System.Web.UI.WebControls.Label
    Protected WithEvents LBlimitata As System.Web.UI.WebControls.Label
    Protected WithEvents LBdiscreta As System.Web.UI.WebControls.Label
    Protected WithEvents LBbuona As System.Web.UI.WebControls.Label
    Protected WithEvents LBottima As System.Web.UI.WebControls.Label
    Protected WithEvents LBmadrelingua As System.Web.UI.WebControls.Label
    Protected WithEvents LBlettura_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBscrittura_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBespressioneOrale_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBrendiPubblico_t As System.Web.UI.WebControls.Label
#End Region

    Public Property CurriculumID() As Integer
        Get
            If Me.HDNcreu_id.Value = "" Then
                Me.HDNcreu_id.Value = 0
            End If
            CurriculumID = Me.HDNcreu_id.Value
        End Get
        Set(ByVal Value As Integer)
            Me.HDNcreu_id.Value = Value
        End Set
    End Property
    Public Property PRSN_ID() As Integer
        Get
            If Me.HDNprsn_id.Value = "" Then
                Me.HDNprsn_id.Value = 0
            End If
            PRSN_ID = Me.HDNprsn_id.Value
        End Get
        Set(ByVal Value As Integer)
            Me.HDNprsn_id.Value = Value
        End Set
    End Property
    Public Event MostraModifica(ByVal sender As Object, ByVal e As EventArgs)
    Public Event MostraAggiungi(ByVal sender As Object, ByVal e As EventArgs)

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResourceLingua) Then
            SetCulture(Session("LinguaCode"))
        End If

        Try
            If Page.IsPostBack = False Then
                Me.SetupInternazionalizzazione()

                Dim PRSN_id As Integer
                PRSN_id = Me.HDNprsn_id.Value
                Dim oConoscenzaLingua As New COL_ConoscenzaLingua
                oConoscenzaLingua.EstraiByPersona(PRSN_id)

                If oConoscenzaLingua.ID = -1 Then
                    PNLinserimento.Visible = True
                    PNLlingua.Visible = False
                    Viewstate("startLingua") = "si"
                Else
                    PNLinserimento.Visible = False
                    PNLlingua.Visible = True
                    Bind_Dati()
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub Start()
        If IsNothing(oResourceLingua) Then
            SetCulture(Session("LinguaCode"))
        End If
        If Viewstate("startLingua") <> "si" Then
            Me.PNLinserimento.Visible = False
            Me.PNLlingua.Visible = True
        Else
            RaiseEvent MostraAggiungi(Me, EventArgs.Empty)
        End If

        Me.PulisciCampi()
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceLingua = New ResourceManager

        oResourceLingua.UserLanguages = Code
        oResourceLingua.ResourcesName = "pg_UC_ConoscenzaLingua"
        oResourceLingua.Folder_Level1 = "Curriculum"
        oResourceLingua.Folder_Level2 = "UC"
        oResourceLingua.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceLingua
            .setLabel(LBnome_t)
            .setLabel(LBnessuna)
            .setLabel(LBlimitata)
            .setLabel(LBdiscreta)
            .setLabel(LBbuona)
            .setLabel(LBottima)
            .setLabel(LBmadrelingua)
            .setLabel(LBlettura_t)
            .setLabel(LBscrittura_t)
            .setLabel(LBespressioneOrale_t)
            .setLabel(LBrendiPubblico_t)

            '.setButton(BTNaggiungi)
            '.setButton(BTNannulla)
            '.setButton(BTNmodifica)

        End With
    End Sub

#End Region

#Region "bind"
    Private Sub Bind_Dati()
        Dim PRSN_id As Integer
        Dim oDataset As DataSet
        Dim i, totale As Integer
        Dim oConoscenzaLingua As New COL_ConoscenzaLingua
        Try
            PRSN_id = Me.HDNprsn_id.Value
            oDataset = oConoscenzaLingua.ElencaByPersona(PRSN_id)
            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota

                Me.PNLinserimento.Visible = True
                Me.PNLlingua.Visible = False
            Else
                Viewstate("startLingua") = ""
                oDataset.Tables(0).Columns.Add(New DataColumn("oLettura"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oScrittura"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oEspressioneOrale"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oCheck"))
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)


                    If oRow.Item("CNLN_lettura") = 1 Then
                        oRow.Item("oLettura") = oResourceLingua.getValue("Nessuna") '"Nessuna" 'oResource.
                    ElseIf oRow.Item("CNLN_lettura") = 2 Then
                        oRow.Item("oLettura") = oResourceLingua.getValue("Limitata") '"Limitata"
                    ElseIf oRow.Item("CNLN_lettura") = 3 Then
                        oRow.Item("oLettura") = oResourceLingua.getValue("Discreta") '"Discreta"
                    ElseIf oRow.Item("CNLN_lettura") = 4 Then
                        oRow.Item("oLettura") = oResourceLingua.getValue("Buona") '"Buona"
                    ElseIf oRow.Item("CNLN_lettura") = 5 Then
                        oRow.Item("oLettura") = oResourceLingua.getValue("Ottima") '"Ottima"
                    ElseIf oRow.Item("CNLN_lettura") = 6 Then
                        oRow.Item("oLettura") = oResourceLingua.getValue("Madrelingua") '"Madrelingua"
                    End If

                    If oRow.Item("CNLN_scrittura") = 1 Then
                        oRow.Item("oScrittura") = oResourceLingua.getValue("Nessuna") '"Nessuna" 'oResource.
                    ElseIf oRow.Item("CNLN_scrittura") = 2 Then
                        oRow.Item("oScrittura") = oResourceLingua.getValue("Limitata") '"Limitata"
                    ElseIf oRow.Item("CNLN_scrittura") = 3 Then
                        oRow.Item("oScrittura") = oResourceLingua.getValue("Discreta") '"Discreta"
                    ElseIf oRow.Item("CNLN_scrittura") = 4 Then
                        oRow.Item("oScrittura") = oResourceLingua.getValue("Buona")  '"Buona"
                    ElseIf oRow.Item("CNLN_scrittura") = 5 Then
                        oRow.Item("oScrittura") = oResourceLingua.getValue("Ottima")  '"Ottima"
                    ElseIf oRow.Item("CNLN_scrittura") = 6 Then
                        oRow.Item("oScrittura") = oResourceLingua.getValue("Madrelingua") '"Madrelingua"
                    End If

                    If oRow.Item("CNLN_espressioneOrale") = 1 Then
                        oRow.Item("oEspressioneOrale") = oResourceLingua.getValue("Nessuna") '"Nessuna" 'oResource.
                    ElseIf oRow.Item("CNLN_espressioneOrale") = 2 Then
                        oRow.Item("oEspressioneOrale") = oResourceLingua.getValue("Limitata") '"Limitata"
                    ElseIf oRow.Item("CNLN_espressioneOrale") = 3 Then
                        oRow.Item("oEspressioneOrale") = oResourceLingua.getValue("Discreta") '"Discreta"
                    ElseIf oRow.Item("CNLN_espressioneOrale") = 4 Then
                        oRow.Item("oEspressioneOrale") = oResourceLingua.getValue("Buona")  '"Buona"
                    ElseIf oRow.Item("CNLN_espressioneOrale") = 5 Then
                        oRow.Item("oEspressioneOrale") = oResourceLingua.getValue("Ottima")  '"Ottima"
                    ElseIf oRow.Item("CNLN_espressioneOrale") = 6 Then
                        oRow.Item("oEspressioneOrale") = oResourceLingua.getValue("Madrelingua") '"Madrelingua"
                    End If


                    If oRow.Item("CNLN_rendiPubblico") = True Then
                        oRow.Item("oCheck") = oResourceLingua.getValue("Si") '"Si"
                    Else
                        oRow.Item("oCheck") = oResourceLingua.getValue("No") '"No"
                    End If

                Next


                Me.RPTlingua.DataSource = oDataset
                Me.RPTlingua.DataBind()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub BindModifica()
        Dim oConoscenzaLingua As New COL_ConoscenzaLingua
        Try
            oConoscenzaLingua.ID = Me.HDNcnln_id.Value

            oConoscenzaLingua.Estrai()
            TXBnome.Text = oConoscenzaLingua.Nome
            Me.RBLlettura.SelectedValue = oConoscenzaLingua.Lettura
            Me.RBLscrittura.SelectedValue = oConoscenzaLingua.Scrittura
            Me.RBLespressioneOrale.SelectedValue = oConoscenzaLingua.EspressioneOrale
            Me.CBXrendiPubblico.Checked = oConoscenzaLingua.RendiPubblico
            Me.HDNcreu_id.Value = oConoscenzaLingua.Curriculum_id
            'oFormazione.Curriculum_id = Me.HDNcreu_id.Value
        Catch ex As Exception

        End Try

    End Sub
#End Region

#Region "gestione repeater"
    Private Sub RPTlingua_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTlingua.ItemCreated
        If IsNothing(oResourceLingua) Then
            SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Try
                Try
                    Dim oLinkbuttonM As LinkButton
                    oLinkbuttonM = e.Item.FindControl("LKBmodifica")
                    If IsNothing(oLinkbuttonM) = False Then

                        oResourceLingua.setLinkButton(oLinkbuttonM, True, True, False, False)
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim oLinkbuttonE As LinkButton
                    oLinkbuttonE = e.Item.FindControl("LKBelimina")
                    If IsNothing(oLinkbuttonE) = False Then

                        oResourceLingua.setLinkButton(oLinkbuttonE, True, True, False, True)

                    End If
                Catch ax As Exception

                End Try


                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBnome_s")
                    If IsNothing(olabel) = False Then
                        oResourceLingua.setLabel_To_Value(olabel, "LBnome_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBlettura_s")
                    If IsNothing(olabel) = False Then
                        oResourceLingua.setLabel_To_Value(olabel, "LBlettura_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBscrittura_s")
                    If IsNothing(olabel) = False Then
                        oResourceLingua.setLabel_To_Value(olabel, "LBscrittura_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBespressioneOrale_s")
                    If IsNothing(olabel) = False Then
                        oResourceLingua.setLabel_To_Value(olabel, "LBespressioneOrale_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBrendiPubblico_s")
                    If IsNothing(olabel) = False Then
                        oResourceLingua.setLabel_To_Value(olabel, "LBrendiPubblico_s.text")
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub RPTlingua_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTlingua.ItemCommand
        Dim CNLN_id As Integer
        Select Case e.CommandName
            Case "modifica"
                CNLN_id = e.CommandArgument
                Me.PNLinserimento.Visible = True
                Me.PNLlingua.Visible = False
                RaiseEvent MostraModifica(Me, EventArgs.Empty)
                ' Me.BTNmodifica.Visible = True
                ' Me.BTNaggiungi.Visible = False
                Me.HDNcnln_id.Value = CNLN_id
                Me.BindModifica()

            Case "elimina"
                Try
                    CNLN_id = e.CommandArgument
                    Dim oConoscenzaLingua As New COL_ConoscenzaLingua
                    oConoscenzaLingua.Elimina(CNLN_id)
                    Me.Bind_Dati()
                Catch ex As Exception

                End Try

        End Select




    End Sub
#End Region

#Region "aggiungi/modifica"
    Public Sub AggiungiLingua()
        Dim oConoscenzaLingua As New COL_ConoscenzaLingua
        Try

            With oConoscenzaLingua
                .Nome = TXBnome.Text
                .Lettura = Me.RBLlettura.SelectedValue
                .Scrittura = Me.RBLscrittura.SelectedValue
                .EspressioneOrale = Me.RBLespressioneOrale.SelectedValue
                .RendiPubblico = Me.CBXrendiPubblico.Checked
                .Curriculum_id = Me.HDNcreu_id.Value
            End With
            If Me.HDNcreu_id.Value <> "" Then
                oConoscenzaLingua.Aggiungi()

                Me.PNLinserimento.Visible = False
                Me.PNLlingua.Visible = True
                Me.Bind_Dati()
                PulisciCampi()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub PulisciCampi()
        Me.TXBnome.Text = ""
        Me.RBLespressioneOrale.SelectedValue = "1"
        Me.RBLlettura.SelectedValue = "1"
        Me.RBLscrittura.SelectedValue = "1"
        Me.CBXrendiPubblico.Checked = True
        ' Me.BTNaggiungi.Visible = True
        '  Me.BTNmodifica.Visible = False

    End Sub
    Public Sub InserisciLingua()
        Me.PNLlingua.Visible = False
        Me.PNLinserimento.Visible = True
        Me.PulisciCampi()
    End Sub

    Public Sub AnnullaLingua()
        Me.PNLlingua.Visible = True
        Me.PNLinserimento.Visible = False
        Me.Bind_Dati()
    End Sub
    Public Sub ModificaLingua()

        Dim oConoscenzaLingua As New COL_ConoscenzaLingua
        Try

            With oConoscenzaLingua
                .Nome = TXBnome.Text
                .Lettura = Me.RBLlettura.SelectedValue
                .Scrittura = Me.RBLscrittura.SelectedValue
                .EspressioneOrale = Me.RBLespressioneOrale.SelectedValue
                .RendiPubblico = Me.CBXrendiPubblico.Checked
                .Curriculum_id = Me.HDNcreu_id.Value
                .ID = Me.HDNcnln_id.Value
            End With
            If Me.HDNcnln_id.Value <> "" Then
                oConoscenzaLingua.Modifica()

                Me.PNLinserimento.Visible = False
                Me.PNLlingua.Visible = True
                Me.Bind_Dati()
                PulisciCampi()
            End If

        Catch ex As Exception

        End Try
    End Sub
#End Region


 

End Class
