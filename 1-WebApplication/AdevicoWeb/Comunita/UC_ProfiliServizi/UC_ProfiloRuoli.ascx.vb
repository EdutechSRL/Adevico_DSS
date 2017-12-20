Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi



Public Class UC_ProfiloRuoli
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

    Public ReadOnly Property HasRuoliSelezionati() As Boolean
        Get
            Try
                If Me.CBLtipoRuolo.SelectedIndex > -1 Then
                    HasRuoliSelezionati = True
                Else
                    HasRuoliSelezionati = (Me.HDN_TPRL_ID.Value <> "" And Me.HDN_TPRL_ID.Value <> "0")
                End If
            Catch ex As Exception
                HasRuoliSelezionati = False
            End Try
        End Get
    End Property

    Public ReadOnly Property isDefinito() As Boolean
        Get
            Try
                isDefinito = (HDN_isDefinito.Value = True)
            Catch ex As Exception
                isDefinito = False
            End Try
        End Get
    End Property

    Protected WithEvents HDN_isDefinito As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_profiloID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_TPCM_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_TPRL_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TBLtipoRuolo As System.Web.UI.WebControls.Table
    Protected WithEvents LBinfoRuoli As System.Web.UI.WebControls.Label
    Protected WithEvents LBruoloDefault_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBXdefault As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LBtipiRuolo_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBLtipoRuolo As System.Web.UI.WebControls.CheckBoxList

    Protected WithEvents TBRruoloObbligatori As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBruoliNonDisattivabili_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBLtipoRuoloNonDisattivabili As System.Web.UI.WebControls.CheckBoxList


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
            .setLabel(Me.LBinfoRuoli)
            .setLabel(Me.LBruoloDefault_t)
            .setLabel(Me.LBtipiRuolo_t)
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
            Dim oProfilo As New COL_ProfiloServizio
            Dim i, totale As Integer
            Dim oDataset As New DataSet

            Me.HDN_profiloID.Value = ProfiloID
            Me.HDN_TPRL_ID.Value = ""
            Me.HDN_isDefinito.Value = False
            oProfilo.Id = ProfiloID
            If ProfiloID > 0 Then
                oProfilo.Estrai()
                If oProfilo.Errore <> Errori_Db.None Then
                    iResponse = WizardProfilo_Message.ProfiloNonTrovato
                Else
                    Me.HDN_TPCM_ID.Value = oProfilo.TipoComunitaID
                End If
            Else
                iResponse = WizardProfilo_Message.ProfiloNonTrovato
            End If
            oDataset = oProfilo.ElencaTipoRuolo(Session("LinguaID"))

            Try
                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                oDataview.RowFilter = "LKTT_default=1"
                If oDataview.Count > 0 Then
                    Me.CBXdefault.Text = oDataview.Item(0).Item("TPRL_Nome")
                    Me.HDN_TPRL_ID.Value = oDataview.Item(0).Item("TPRL_ID")

                    Try
                        Me.HDN_isDefinito.Value = (oDataview.Item(0).Item("isDefinito") = 1)
                    Catch ex As Exception
                        Me.HDN_isDefinito.Value = False
                    End Try
                Else
                    Return WizardProfilo_Message.TipoRuoloNonTrovato
                End If

                oDataview.RowFilter = "LKTT_default=0 and LKTT_Allways =0"
                totale = oDataview.Count - 1

                Me.CBLtipoRuolo.Items.Clear()
                For i = 0 To totale
                    Dim oListItem As New ListItem
                    oListItem.Text = oDataview.Item(i).Item("TPRL_Nome")
                    oListItem.Value = oDataview.Item(i).Item("TPRL_ID")
                    oListItem.Selected = (oDataview.Item(i).Item("Associato") = 1)
                    If oDataview.Item(i).Item("TPRL_ID") = Main.TipoRuoloStandard.AdminComunità Then
                        oListItem.Attributes.Add("onclick", "this.checked=true;return false;")
                    ElseIf oDataview.Item(i).Item("TPRL_ID") = Main.TipoRuoloStandard.AdminComunità Then
                        oListItem.Attributes.Add("onclick", "this.checked=true;return false;")
                    ElseIf oDataview.Item(i).Item("TPRL_ID") = Main.TipoRuoloStandard.AdminOrganizzazione Then
                        oListItem.Attributes.Add("onclick", "this.checked=true;return false;")
                    ElseIf oDataview.Item(i).Item("TPRL_ID") = Main.TipoRuoloStandard.AccessoNonAutenticato Then
                        oListItem.Attributes.Add("onclick", "this.checked=true;return false;")
                    End If
                    Me.CBLtipoRuolo.Items.Add(oListItem)
                Next

                oDataview.RowFilter = "LKTT_default=0 and LKTT_Allways =1"
                totale = oDataview.Count - 1

                Me.CBLtipoRuoloNonDisattivabili.Items.Clear()
                For i = 0 To totale
                    Dim oListItem As New ListItem
                    oListItem.Text = oDataview.Item(i).Item("TPRL_Nome")
                    oListItem.Value = oDataview.Item(i).Item("TPRL_ID")
                    oListItem.Selected = True
                    If oDataview.Item(i).Item("TPRL_ID") = Main.TipoRuoloStandard.AdminComunità Then
                        oListItem.Attributes.Add("onclick", "this.checked=true;return false;")
                    ElseIf oDataview.Item(i).Item("TPRL_ID") = Main.TipoRuoloStandard.AdminComunità Then
                        oListItem.Attributes.Add("onclick", "this.checked=true;return false;")
                    ElseIf oDataview.Item(i).Item("TPRL_ID") = Main.TipoRuoloStandard.AdminOrganizzazione Then
                        oListItem.Attributes.Add("onclick", "this.checked=true;return false;")
                    End If
                    Me.CBLtipoRuoloNonDisattivabili.Items.Add(oListItem)
                Next

                iResponse = WizardProfilo_Message.OperazioneConclusa
            Catch ex As Exception
                iResponse = WizardProfilo_Message.TipoRuoloNonTrovato
            End Try
        Catch ex As Exception

        End Try
        Return iResponse
    End Function
#End Region

    Public Function Salva_Dati() As WizardProfilo_Message
        Dim iResponse As WizardProfilo_Message = WizardProfilo_Message.ErroreGenerico
        Dim oProfilo As New COL_ProfiloServizio

        Try
            With oProfilo
                If Me.HDN_TPRL_ID.Value = "" Or Me.HDN_TPRL_ID.Value = "0" Then
                    Return ModuloEnum.WizardProfilo_Message.ProfiloNonTrovato
                ElseIf Me.HDN_profiloID.Value > 0 Then
                    .Id = Me.HDN_profiloID.Value
                    .Estrai()
                    If .Errore <> Errori_Db.None Then
                        Return WizardProfilo_Message.ProfiloNonTrovato
                    Else
                        Dim ListaRuoli As String = ","
                        Dim i, totale As Integer

                        If Me.CBLtipoRuolo.SelectedIndex > -1 Then
                            totale = Me.CBLtipoRuolo.Items.Count - 1
                            For i = Me.CBLtipoRuolo.SelectedIndex To totale
                                Dim oListItem As New ListItem
                                oListItem = Me.CBLtipoRuolo.Items(i)
                                If oListItem.Selected Then
                                    ListaRuoli &= oListItem.Value & ","
                                End If
                            Next
                        End If


                        totale = Me.CBLtipoRuoloNonDisattivabili.Items.Count - 1
                        For i = 0 To totale
                            Dim oListItem As New ListItem
                            oListItem = Me.CBLtipoRuoloNonDisattivabili.Items(i)
                            ListaRuoli &= oListItem.Value & ","
                            Me.CBLtipoRuoloNonDisattivabili.Items(i).Selected = True
                        Next

                        If ListaRuoli = "," Then
                            ListaRuoli = ""
                        End If
                        .DefinisciRuoli(ListaRuoli)
                        If .Errore = Errori_Db.None Then
                            Me.HDN_isDefinito.Value = True
                            Return ModuloEnum.WizardProfilo_Message.RuoliAssociati
                        Else
                            Return ModuloEnum.WizardProfilo_Message.ErroreGenerico
                        End If
                    End If
                Else
                    Return WizardProfilo_Message.ProfiloNonTrovato
                End If
            End With
        Catch ex As Exception

        End Try
        Return iResponse
    End Function
End Class