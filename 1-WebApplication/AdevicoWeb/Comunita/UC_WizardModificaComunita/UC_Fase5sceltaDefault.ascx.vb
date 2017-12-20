Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class UC_Fase5sceltaDefault
    Inherits System.Web.UI.UserControl
    Private oResourceServizi As ResourceManager


    Public Event AggiornamentoVisualizzazione(ByVal Selezionato As Boolean)
    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNhasSetup As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNhasServizi As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TBRutenteSelezionato As System.Web.UI.WebControls.TableRow 'Era Table, ma è una row...
    Protected WithEvents TBLdatiPrincipali As System.Web.UI.WebControls.Table
    Protected WithEvents LBavviso As System.Web.UI.WebControls.Label
    Protected WithEvents TBRdefault As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBpaginaDefault_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLpagineDefault As System.Web.UI.WebControls.DropDownList

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
        If IsNothing(oResourceServizi) Then
            Me.SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        End If
    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        Me.oResourceServizi = New ResourceManager

        oResourceServizi.UserLanguages = code
        oResourceServizi.ResourcesName = "pg_ManagementServizi"
        oResourceServizi.Folder_Level1 = "Comunita"
        oResourceServizi.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceServizi
            .setLabel(Me.LBavviso)
            .setLabel(Me.LBpaginaDefault_t)
        End With
    End Sub
#End Region


    Public Sub SetupControl(ByVal ComunitaID As Integer)
        Dim SceltaDefault As Integer = 0
        Dim ProfiloID As Integer = -1

        If IsNothing(oResourceServizi) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        Me.HDNhasSetup.Value = True
        Me.HDNcmnt_ID.Value = ComunitaID
        Me.HDNhasServizi.Value = False
        Me.Bind_SezioneDefault()
        Me.SetupInternazionalizzazione()

        RaiseEvent AggiornamentoVisualizzazione(AbilitaPulsanti)
    End Sub


#Region "Bind_Dati"
    Private Sub Bind_SezioneDefault()
        Me.DDLpagineDefault.Enabled = True
        Me.Bind_PagineDefault()
        Me.SelezionaPaginaDefault()
    End Sub
    Private Sub Bind_PagineDefault(Optional ByVal SelezionaID As Integer = -1)
        Dim oComunita As New COL_Comunita

        If IsNothing(oResourceServizi) Then
            SetCulture(Session("LinguaCode"))
        End If

        Me.DDLpagineDefault.Items.Clear()
        Try
            Dim i, totale As Integer
            Dim oDataset As New DataSet
            oDataset = oComunita.ElencoPagineDefault(Me.HDNcmnt_ID.Value, Session("LinguaID"))
            totale = oDataset.Tables(0).Rows.Count
            For i = 0 To totale - 1
                Dim oRow As DataRow
                oRow = oDataset.Tables(0).Rows(i)

                If oRow.Item("SRVZ_Attivato") = False Or oRow.Item("SRVC_isAbilitato") = False Then
                    oRow("DFLP_Nome") = oRow("DFLP_Nome") & " (" & Me.oResourceServizi.getValue("disattivato") & ")"
                End If
            Next
            If totale > 0 Then
                Me.DDLpagineDefault.DataSource = oDataset
                Me.DDLpagineDefault.DataTextField = "DFLP_Nome"
                Me.DDLpagineDefault.DataValueField = "DFLP_ID"
                Me.DDLpagineDefault.DataBind()

                If SelezionaID > 0 Then
                    Try
                        Me.DDLpagineDefault.SelectedValue = SelezionaID
                    Catch ex As Exception

                    End Try
                End If
                Me.HDNhasServizi.Value = True
            Else
                Me.HDNhasServizi.Value = False
            End If
        Catch ex As Exception
            ' aggiungere riga "nessuno"
        End Try
    End Sub
    Private Sub SelezionaPaginaDefault()
        Try
            Dim oComunita As New COL_Comunita
            Dim percorso, codice As String
            Dim PaginaID As Integer
            oComunita.GetDefaultPage(Me.HDNcmnt_ID.Value, percorso, codice, PaginaID)
            If PaginaID > 0 Then
                Me.DDLpagineDefault.SelectedValue = PaginaID
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Public Function RegistraServizioDefault() As WizardComunita_Message
        Dim iResponse As WizardComunita_Message = ModuloEnum.WizardComunita_Message.NesunaOperazione
        Try
            If Me.DDLpagineDefault.Items.Count > 0 Then
                Dim oComunita As COL_Comunita
                If oComunita.SetDefaultPage(Me.HDNcmnt_ID.Value, DDLpagineDefault.SelectedValue) Then
                    iResponse = ModuloEnum.WizardComunita_Message.ServiziDefault
                Else
                    iResponse = ModuloEnum.WizardComunita_Message.ServizioDefaultNonDefinito
                End If
            Else
                iResponse = ModuloEnum.WizardComunita_Message.NessunServizio
            End If
        Catch ex As Exception

        End Try
        Return iResponse
    End Function

    Private Function AbilitaPulsanti() As Boolean
        Dim HasSelezionati As Boolean = False
        Try
            If Me.HDNhasServizi.Value = True Then
                HasSelezionati = True
            End If
        Catch ex As Exception
            HasSelezionati = False
        End Try
        Return HasSelezionati
    End Function
End Class
