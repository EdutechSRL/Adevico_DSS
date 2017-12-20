Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.Comunita


Public Class UC_DatiProfilo
    Inherits System.Web.UI.UserControl
    Private oResource As ResourceManager

    Public ReadOnly Property ProfiloID() As Integer
        Get
            Try
                ProfiloID = Me.HDN_profiloID.Value
            Catch ex As Exception
                Me.HDN_profiloID.Value = 0
                ProfiloID = 0
            End Try
        End Get
    End Property

    Public ReadOnly Property GetNomeProfilo() As String
        Get
            Try
                GetNomeProfilo = Me.HDN_nomeProfilo.Value
            Catch ex As Exception
                GetNomeProfilo = ""
            End Try
        End Get
    End Property

    ' Public Event AggiornaMenuServizi(ByVal AttivaDefault As Boolean, ByVal Abilitato As Boolean)
    Protected WithEvents HDN_profiloID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_nomeProfilo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LBnome_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBNome As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBdescrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBdescrizione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBtipoComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoComunita As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBcreatore_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcreatore As System.Web.UI.WebControls.Label
    Protected WithEvents LBultimaModifica_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBultimaModifica As System.Web.UI.WebControls.Label
    Protected WithEvents LBinfo As System.Web.UI.WebControls.Label
    Protected WithEvents TBRcreatore As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRmodifica As System.Web.UI.WebControls.TableRow

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
        oResource.ResourcesName = "pg_UC_DatiProfilo"
        oResource.Folder_Level1 = "Comunita"
        oResource.Folder_Level2 = "UC_ProfiliServizi"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBcreatore_t)
            .setLabel(Me.LBdescrizione_t)
            .setLabel(Me.LBnome_t)
            .setLabel(Me.LBtipoComunita_t)
            .setLabel(Me.LBultimaModifica_t)
        End With
    End Sub
#End Region

#Region "Bind_Dati"
    Public Function Setup_Controllo(ByVal ProfiloID As Integer) As WizardProfilo_Message
        Dim iResponse As WizardProfilo_Message = WizardProfilo_Message.ProfiloNonTrovato

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Me.SetupInternazionalizzazione()
        Try
            Me.HDN_profiloID.Value = ProfiloID
            Me.HDN_nomeProfilo.Value = ""

            iResponse = Me.Bind_TipoComunita
            If ProfiloID = 0 Then
                Me.TXBdescrizione.Text = ""
                Me.TXBNome.Text = ""
                Me.TBRcreatore.Visible = False
                Me.TBRmodifica.Visible = False
            Else
                Dim oProfilo As New COL_ProfiloServizio
                oProfilo.Id = ProfiloID
                oProfilo.Estrai()
                If oProfilo.Errore = Errori_Db.None Then
                    Me.TXBdescrizione.Text = oProfilo.Descrizione
                    Me.TXBNome.Text = oProfilo.Nome
                    Try
                        Me.DDLtipoComunita.SelectedValue = oProfilo.TipoComunitaID
                    Catch ex As Exception

                    End Try
                    Me.DDLtipoComunita.Enabled = False
                    Me.TBRcreatore.Visible = True
                    Me.TBRmodifica.Visible = False
                    Try
                        If oProfilo.CreatoIl < oProfilo.ModificatoIl Then
                            Me.TBRmodifica.Visible = True
                        End If
                    Catch ex As Exception
                    End Try

                    Dim oPersona As New COL_Persona
                    If Me.TBRcreatore.Visible = True Then
                        Me.oResource.setLabel(Me.LBcreatore)
                        Try
                            If Session("objPersona").id = oProfilo.CreatoreID Then
                                Me.LBcreatore.Text = Replace(Me.LBcreatore.Text, "#nome#", Session("objPersona").cognome & " " & Session("objPersona").nome)
                            Else
                                oPersona.Id = oProfilo.CreatoreID
                                oPersona.Estrai(Session("LinguaID"))
                                Me.LBcreatore.Text = Replace(Me.LBcreatore.Text, "#nome#", oPersona.Cognome & " " & oPersona.Nome)
                            End If
                            Me.LBcreatore.Text = Replace(Me.LBcreatore.Text, "#data#", oProfilo.CreatoIl)
                        Catch ex As Exception
                            Me.TBRcreatore.Visible = False
                        End Try
                    End If

                    If Me.TBRmodifica.Visible = True Then
                        Me.oResource.setLabel(Me.LBultimaModifica)
                        Try
                            If Session("objPersona").id = oProfilo.ModificatoDa Then
                                Me.LBultimaModifica.Text = Replace(Me.LBultimaModifica.Text, "#nome#", Session("objPersona").cognome & " " & Session("objPersona").nome)
                            Else
                                oPersona.Id = oProfilo.ModificatoDa
                                oPersona.Estrai(Session("LinguaID"))
                                Me.LBultimaModifica.Text = Replace(Me.LBultimaModifica.Text, "#nome#", oPersona.Cognome & " " & oPersona.Nome)
                            End If
                            Me.LBultimaModifica.Text = Replace(Me.LBultimaModifica.Text, "#data#", oProfilo.ModificatoIl)
                        Catch ex As Exception
                            Me.TBRmodifica.Visible = False
                        End Try
                    End If
                Else
                    Me.HDN_profiloID.Value = 0
                    Me.TXBNome.Text = ""
                    Me.TXBdescrizione.Text = ""
                    Me.TBRcreatore.Visible = False
                    Me.TBRmodifica.Visible = False
                    iResponse = WizardProfilo_Message.ProfiloNonTrovato
                End If
            End If
        Catch ex As Exception

        End Try
        Return iResponse
    End Function

    Private Function Bind_TipoComunita() As WizardProfilo_Message
        Dim iResponse As WizardProfilo_Message = WizardProfilo_Message.TipoComunitanonTrovato
        Dim oTipoComunita As COL_Tipo_Comunita
        Dim oDataset As DataSet

        Try
            oDataset = oTipoComunita.Elenca(Session("LinguaID"))
            Me.DDLtipoComunita.DataSource = oDataset
            Me.DDLtipoComunita.DataTextField = "TPCM_Descrizione"
            Me.DDLtipoComunita.DataValueField = "TPCM_ID"
            Me.DDLtipoComunita.DataBind()
        Catch ex As Exception

        End Try
        If Me.DDLtipoComunita.Items.Count > 0 Then
            Me.DDLtipoComunita.Enabled = True
            iResponse = ModuloEnum.WizardServizio_Message.OperazioneConclusa
        Else
            Me.DDLtipoComunita.Enabled = False
        End If
        Return iResponse
    End Function
#End Region

    Public Function Salva_Dati() As WizardProfilo_Message
        Dim iResponse As WizardProfilo_Message = WizardProfilo_Message.ErroreGenerico
        Dim oProfilo As New COL_ProfiloServizio

        Try
            With oProfilo
                If Me.HDN_profiloID.Value > 0 Then
                    .Id = Me.HDN_profiloID.Value
                    .Estrai()
                    If .Errore <> Errori_Db.None Then
                        Return WizardProfilo_Message.ProfiloNonTrovato
                    End If
                End If
                .Descrizione = Me.TXBdescrizione.Text
                .Nome = Me.TXBNome.Text
                .TipoComunitaID = Me.DDLtipoComunita.SelectedValue

                If Me.HDN_profiloID.Value > 0 Then
                    .ModificatoDa = Session("objPersona").id
                    .Modifica()
                    If .Errore = Errori_Db.None Then
                        iResponse = WizardProfilo_Message.Modificato
                    Else
                        iResponse = WizardProfilo_Message.NONModificato
                    End If
                Else
                    .CreatoreID = Session("objPersona").id
                    .ModificatoDa = Session("objPersona").id
                    .Aggiungi()
                    If .Errore = Errori_Db.None Then
                        Me.HDN_profiloID.Value = .Id
                        iResponse = WizardProfilo_Message.Creato
                    Else
                        iResponse = WizardProfilo_Message.NONinserito
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
        Return iResponse
    End Function
End Class