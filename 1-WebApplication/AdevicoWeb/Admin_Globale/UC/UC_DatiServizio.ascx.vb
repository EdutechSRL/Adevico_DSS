Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.Comunita


Public Class UC_DatiServizio
    Inherits System.Web.UI.UserControl
    Private oResource As ResourceManager

    Public ReadOnly Property ServizioID() As Integer
        Get
            Try
                ServizioID = Me.HDN_servizioID.Value
            Catch ex As Exception
                Me.HDN_servizioID.Value = 0
                ServizioID = 0
            End Try
        End Get
    End Property

    Public ReadOnly Property GetNomeServizio() As String
        Get
            Dim name As String = "--"
            If String.IsNullOrEmpty(Me.HDN_nomeServizio.Value) AndAlso Not String.IsNullOrEmpty(Me.TXBNome.Text) Then
                name = Me.TXBNome.Text
            ElseIf Not String.IsNullOrEmpty(Me.HDN_nomeServizio.Value) Then
                name = Me.HDN_nomeServizio.Value
            End If
            Return name
        End Get
    End Property
  
    Public Event AggiornaMenuServizi(ByVal AttivaDefault As Boolean, ByVal Abilitato As Boolean)
    Protected WithEvents HDN_servizioID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_nomeServizio As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LBnomeServizio_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBNome As System.Web.UI.WebControls.TextBox
    Protected WithEvents RPTnome As System.Web.UI.WebControls.Repeater
    Protected WithEvents LBdescrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBdescrizione As System.Web.UI.WebControls.TextBox
    Protected WithEvents RPTdescrizione As System.Web.UI.WebControls.Repeater
    Protected WithEvents LBcodice_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBcodice As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBattiva_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBXattiva As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LBnonDisattivabile_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBXnoDisattiva As System.Web.UI.WebControls.CheckBox

    Protected WithEvents LBisNotificabile_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBXnotificabile As System.Web.UI.WebControls.CheckBox

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
            Me.SetCulture(Session("LinguaCode"))
        End If
    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_UC_DatiServizio"
        oResource.Folder_Level1 = "Admin_Globale"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBattiva_t)
            .setLabel(Me.LBcodice_t)
            .setLabel(Me.LBdescrizione_t)
            .setLabel(Me.LBnomeServizio_t)
            .setLabel(Me.LBnonDisattivabile_t)
            .setCheckBox(Me.CBXattiva)
            .setCheckBox(Me.CBXnoDisattiva)
            .setLabel(Me.LBisNotificabile_t)
            .setCheckBox(Me.CBXnotificabile)
        End With
    End Sub
#End Region

#Region "Bind_Dati"
    Public Function Setup_Controllo(ByVal ServizioID As Integer) As WizardServizio_Message
        Dim iResponse As WizardServizio_Message = WizardServizio_Message.ServizioNonTrovato

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Me.SetupInternazionalizzazione()
        Try
            Me.HDN_servizioID.Value = ServizioID
            Me.HDN_nomeServizio.Value = ""
            If ServizioID = 0 Then
                Me.TXBcodice.Text = ""
                Me.TXBNome.Text = ""
                Me.TXBdescrizione.Text = ""
                Me.CBXnoDisattiva.Checked = False
                Me.CBXattiva.Checked = False
                Me.CBXnotificabile.Checked = False
            Else
                Dim oServizio As New COL_Servizio
                oServizio.ID = ServizioID
                oServizio.EstraiByLingua(Session("LinguaID"))
                If oServizio.Errore = Errori_Db.None Then
                    Me.TXBcodice.Text = oServizio.Codice
                    Me.TXBdescrizione.Text = oServizio.Descrizione
                    Me.TXBNome.Text = oServizio.Nome
                    Me.CBXnoDisattiva.Checked = oServizio.isNonDisattivabile
                    Me.CBXattiva.Checked = oServizio.isAttivato
                    Me.CBXnotificabile.Checked = oServizio.isNotificabile
                Else
                    Me.HDN_servizioID.Value = 0
                    Me.TXBcodice.Text = ""
                    Me.TXBNome.Text = ""
                    Me.TXBdescrizione.Text = ""
                    Me.CBXnoDisattiva.Checked = False
                    Me.CBXattiva.Checked = True
                    Me.CBXnotificabile.Checked = False
                    iResponse = WizardServizio_Message.ServizioNonTrovato
                End If
            End If
        Catch ex As Exception

        End Try
        Me.Bind_Lingue()
        Return iResponse
    End Function

    Private Sub Bind_Lingue()
        Dim oServizio As New COL_Servizio
        Dim oDataset As DataSet
        Dim i, totale As Integer

        Try
            oServizio.ID = Me.HDN_servizioID.Value
            oDataset = oServizio.ElencaDefinizioniLingue()
            For i = 0 To oDataset.Tables(0).Rows.Count - 1
                If IsDBNull(oDataset.Tables(0).Rows(i).Item("Nome")) Then
                    oDataset.Tables(0).Rows(i).Item("Nome") = ""
                Else
                    Try
                        If Me.HDN_servizioID.Value > 0 Then
                            If oDataset.Tables(0).Rows(i).Item("LNGU_ID") = Session("LinguaID") Then
                                Me.HDN_nomeServizio.Value = oDataset.Tables(0).Rows(i).Item("Nome")
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                End If
                If IsDBNull(oDataset.Tables(0).Rows(i).Item("Descrizione")) Then
                    oDataset.Tables(0).Rows(i).Item("Descrizione") = ""
                Else
                    If oDataset.Tables(0).Rows(i).Item("Descrizione") = "" Then

                    End If
                End If
            Next

            Try
                If Me.HDN_servizioID.Value > 0 Then
                    If Me.HDN_nomeServizio.Value = "" Then
                        Me.HDN_nomeServizio.Value = Me.TXBNome.Text
                    End If
                Else
                    Me.HDN_nomeServizio.Value = ""
                End If
            Catch ex As Exception
                Me.HDN_nomeServizio.Value = ""
            End Try

            Me.RPTnome.DataSource = oDataset
            Me.RPTnome.DataBind()

            Me.RPTdescrizione.DataSource = oDataset
            Me.RPTdescrizione.DataBind()
        Catch ex As Exception

        End Try
    End Sub
#End Region


    Private Sub RPTnome_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTnome.ItemCreated
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                oResource.setLabel(e.Item.FindControl("LBlinguaNome_t"))
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub RPTdescrizione_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTdescrizione.ItemCreated
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                oResource.setLabel(e.Item.FindControl("LBlinguaDescrizione_t"))
            Catch ex As Exception

            End Try
        End If
    End Sub


    Public Function Salva_Dati() As WizardServizio_Message
        Dim iResponse As WizardServizio_Message = WizardServizio_Message.ErroreGenerico
        Dim oServizio As New COL_Servizio

        Try
            With oServizio
                If Me.HDN_servizioID.Value > 0 Then
                    .ID = Me.HDN_servizioID.Value
                    .EstraiByLingua(Session("LinguaID"))
                    If .Errore <> Errori_Db.None Then
                        Return WizardServizio_Message.ServizioNonTrovato
                    End If
                End If
                .Codice = Me.TXBcodice.Text
                .Descrizione = Me.TXBdescrizione.Text
                .Nome = Me.TXBNome.Text
                .isAttivato = Me.CBXattiva.Checked
                .isNonDisattivabile = Me.CBXnoDisattiva.Checked
                .isNotificabile = Me.CBXnotificabile.Checked
                '   .Padre = 0
                If Me.HDN_servizioID.Value > 0 Then
                    .Modifica()
                    If .Errore = Errori_Db.None Then
                        iResponse = Me.Salva_DefinizioniLingue()
                        If iResponse = WizardServizio_Message.OperazioneConclusa Then
                            iResponse = WizardServizio_Message.Modificato
                        Else
                            iResponse = WizardServizio_Message.ErroreAssociazioneLingue
                        End If
                    Else
                        iResponse = WizardServizio_Message.NONModificato
                    End If
                Else
                    .Aggiungi()
                    If .Errore = Errori_Db.None Then
                        Me.HDN_servizioID.Value = .ID
                        iResponse = Me.Salva_DefinizioniLingue()
                        If iResponse = WizardServizio_Message.OperazioneConclusa Then
                            iResponse = WizardServizio_Message.Creato
                        Else
                            iResponse = WizardServizio_Message.ErroreAssociazioneLingue
                        End If
                    Else
                        iResponse = WizardServizio_Message.NONinserito
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
        Return iResponse
    End Function

    Private Function Salva_DefinizioniLingue() As WizardServizio_Message
        Dim LinguaID, i, totale As Integer
        Dim Nome, Descrizione As String
        Dim oServizio As New COL_Servizio

        oServizio.ID = Me.HDN_servizioID.Value
        totale = Me.RPTnome.Items.Count

        Try
            If totale > 0 Then
                For i = 0 To totale - 1
                    Dim oLabel As Label
                    Dim oText As TextBox

                    Try
                        oLabel = Me.RPTnome.Items(i).FindControl("LBlinguaID")
                        LinguaID = oLabel.Text
                    Catch ex As Exception
                        LinguaID = 0
                    End Try
                    If LinguaID > 0 Then
                        Try
                            oText = Me.RPTnome.Items(i).FindControl("TXBtermine")
                            Nome = oText.Text
                        Catch ex As Exception
                            Nome = ""
                        End Try

                        If Nome = "" Then
                            Nome = Me.TXBNome.Text
                        End If

                        Try
                            oText = Me.RPTnome.Items(i).FindControl("TXBtermine2")
                            Descrizione = oText.Text
                        Catch ex As Exception
                            Descrizione = ""
                        End Try

                        If Descrizione = "" Then
                            Descrizione = Me.TXBdescrizione.Text
                        End If

                        Try
                            If LinguaID = Session("LinguaID") Then
                                Me.HDN_nomeServizio.Value = Nome
                            End If
                        Catch ex As Exception

                        End Try
                        oServizio.Translate(Nome, Descrizione, LinguaID)
                    End If
                Next
                If Me.HDN_nomeServizio.Value = "" Then
                    Me.HDN_nomeServizio.Value = Me.TXBNome.Text
                End If
                Return WizardServizio_Message.OperazioneConclusa
            Else
                Return WizardServizio_Message.ErroreAssociazioneLingue
            End If
        Catch ex As Exception
            Return WizardServizio_Message.ErroreAssociazioneLingue
        End Try
    End Function

    Public Sub validate()
        Me.Page.Validate()
    End Sub

End Class