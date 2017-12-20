Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class UC_Fase4DefinizioneServizi
    Inherits System.Web.UI.UserControl

    Private oResourceServizi As ResourceManager
    Public Event AggiornamentoVisualizzazione(ByVal Selezionato As Boolean)
    Protected WithEvents HDN_TipoComunitaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNhasServizi As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ORGN_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNhasSetup As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_PersonaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ResponsabileID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNserviziSelezionati As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents TBLdefinizioni As System.Web.UI.WebControls.Table
    Protected WithEvents LBsceltaServizio As System.Web.UI.WebControls.Label
    Protected WithEvents LBservizio_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBLsceltaServizio As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents DDLprofilo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents CBLservizi As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents RPTservizio As System.Web.UI.WebControls.Repeater

   

    Public ReadOnly Property isInizializzato() As Boolean
        Get
            Try
                isInizializzato = HDNhasSetup.Value
            Catch ex As Exception
                isInizializzato = False
            End Try
        End Get
    End Property
    Public ReadOnly Property GetServizi() As String
        Get
            Try
                GetServizi = GetNomiServiziSelezionati()
            Catch ex As Exception
                GetServizi = ""
            End Try
        End Get
    End Property



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
        End If
    End Sub

    Public Sub SetupControl(ByVal TipoComunitaID As Integer, ByVal OrganizzazioneID As Integer, ByVal PersonaID As Integer, ByVal ResponsabileID As Integer, Optional ByVal ComunitaID As Integer = 0) 'Optional ByVal ComunitaID As Integer = 0)
        Dim SceltaDefault As Integer = 0
        Dim ProfiloID As Integer = -1

        If IsNothing(oResourceServizi) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        Me.HDN_TipoComunitaID.Value = TipoComunitaID
        Me.HDN_ORGN_ID.Value = OrganizzazioneID
        Me.HDNhasSetup.Value = True
        Me.HDN_PersonaID.Value = PersonaID
        Me.HDN_ResponsabileID.Value = ResponsabileID
        Me.HDNcmnt_ID.Value = ComunitaID

        If ComunitaID = 0 Then
            SceltaDefault = 0
        Else
            Dim oComunita As COL_Comunita
            oComunita.Id = ComunitaID
            ProfiloID = oComunita.GetProfiloServizioID()
            If ProfiloID > 0 Then
                SceltaDefault = 1
            End If
        End If
        Me.RBLsceltaServizio.SelectedValue = SceltaDefault

        Me.Bind_DatiProfili(TipoComunitaID, ProfiloID)
        If SceltaDefault <> 0 Then
            Me.DDLprofilo.Visible = True
        Else
            Me.Bind_DatiServiziDefault(True, TipoComunitaID, OrganizzazioneID, ComunitaID)
            Me.DDLprofilo.Visible = False
        End If

        If Me.DDLprofilo.Items.Count = 0 Then
            Me.RBLsceltaServizio.Enabled = False
        Else
            Me.RBLsceltaServizio.Enabled = True
        End If
        Me.SetupInternazionalizzazione()
        RaiseEvent AggiornamentoVisualizzazione(AbilitaPulsanti)
    End Sub
    Public Sub AggiornaDati(ByVal TipoComunitaID As Integer, ByVal ComunitaID As Integer, ByVal ResponsabileID As Integer)
        If IsNothing(oResourceServizi) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        Try
            Dim ProfiloID As Integer = -1
            Dim ricalcola As Boolean = False
            If TipoComunitaID <> Me.HDN_TipoComunitaID.Value Then


                If ComunitaID > 0 Then
                    Dim oComunita As COL_Comunita
                    oComunita.Id = ComunitaID
                    ProfiloID = oComunita.GetProfiloServizioID()
                End If
                Me.HDN_ResponsabileID.Value = ResponsabileID
                Me.Bind_DatiProfili(TipoComunitaID, ProfiloID)
                ricalcola = True
            Else
                Try
                    ProfiloID = Me.DDLprofilo.SelectedValue
                Catch ex As Exception

                End Try
                If Me.HDN_ResponsabileID.Value <> ResponsabileID Then
                    Me.Bind_DatiProfili(TipoComunitaID, ProfiloID)
                    ricalcola = True
                End If
                Me.HDN_ResponsabileID.Value = ResponsabileID
            End If
            If Me.RBLsceltaServizio.SelectedValue = 0 Then
                Me.Bind_DatiServiziDefault(ricalcola, TipoComunitaID, Me.HDN_ORGN_ID.Value, ComunitaID)
            Else
                Me.Bind_DatiServiziPersonali(ricalcola, TipoComunitaID, ComunitaID, Me.HDN_ORGN_ID.Value)
            End If
        Catch ex As Exception
            Me.HDNhasServizi.Value = False
        End Try
        RaiseEvent AggiornamentoVisualizzazione(AbilitaPulsanti)
    End Sub
    Public Sub ResetControllo()
        Me.HDNserviziSelezionati.Value = ""
        Me.HDNhasServizi.Value = False
        Me.HDNhasSetup.Value = False
        Me.HDN_ResponsabileID.Value = 0
        Me.HDN_TipoComunitaID.Value = 0
        Me.HDN_ORGN_ID.Value = 0
        Me.HDN_PersonaID.Value = 0
        Me.HDNcmnt_ID.Value = 0
    End Sub

    Private Sub Bind_DatiServiziDefault(ByVal Ricalcola As Boolean, ByVal TipoComunitaID As Integer, ByVal OrganizzazioneID As Integer, ByVal ComunitaID As Integer)
        Dim oServizio As New COL_Servizio
        Dim oDataset As New DataSet

        Me.HDNhasServizi.Value = False
        Me.HDN_TipoComunitaID.Value = TipoComunitaID
        Me.HDNcmnt_ID.Value = ComunitaID
        Me.HDN_ORGN_ID.Value = OrganizzazioneID
        Try
            Dim i, totale As Integer
            If ComunitaID > 0 Then
                oDataset = oServizio.ElencaByComunita(ComunitaID, Session("LinguaID"))
            Else
                oDataset = oServizio.ElencaByTipoComunita(TipoComunitaID, OrganizzazioneID, Session("LinguaID"))
            End If

            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                Me.HDNhasServizi.Value = True
                Me.CBLservizi.Items.Clear()


                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDisabled"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDefault"))
                If Ricalcola Then
                    Me.HDNserviziSelezionati.Value = ""
                End If
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    oRow.Item("oCheckDisabled") = ""
                    If CBool(oRow.Item("isNonDisattivabile")) Then
                        oRow.Item("oCheckDisabled") = "disabled"
                        oRow.Item("oCheckDefault") = "checked"

                        If Not (InStr(Me.HDNserviziSelezionati.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0) Then
                            If Me.HDNserviziSelezionati.Value = "" Then
                                Me.HDNserviziSelezionati.Value = "," & oRow.Item("SRVZ_id") & ","
                            Else
                                Me.HDNserviziSelezionati.Value = Me.HDNserviziSelezionati.Value & oRow.Item("SRVZ_id") & ","
                            End If
                        End If
                    ElseIf Not CBool(oRow.Item("isAbilitato")) Then
                        oRow.Item("oCheckDisabled") = "disabled"
                        oRow.Item("oCheckDefault") = ""

                        If InStr(Me.HDNserviziSelezionati.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                            Me.HDNserviziSelezionati.Value = Me.HDNserviziSelezionati.Value.Replace("," & oRow.Item("SRVZ_ID") & ",", ",")
                        End If
                    Else
                        If Ricalcola Then
                            If CBool(oRow.Item("isDefault")) Then
                                oRow.Item("oCheckDefault") = "checked"

                                If Me.HDNserviziSelezionati.Value = "" Then
                                    Me.HDNserviziSelezionati.Value = "," & oRow.Item("SRVZ_id") & ","
                                Else
                                    Me.HDNserviziSelezionati.Value = Me.HDNserviziSelezionati.Value & oRow.Item("SRVZ_id") & ","
                                End If
                            Else
                                oRow.Item("oCheckDefault") = ""
                            End If
                        Else
                            If InStr(Me.HDNserviziSelezionati.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                                oRow.Item("oCheckDefault") = "checked"
                            Else
                                oRow.Item("oCheckDefault") = ""
                            End If
                        End If
                    End If
                Next
                Me.RPTservizio.DataSource = oDataset
                Me.RPTservizio.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_DatiServiziPersonali(ByVal Ricalcola As Boolean, ByVal TipoComunitaID As Integer, ByVal ComunitaID As Integer, ByVal OrganizzazioneID As Integer)
        Dim oProfilo As New COL_ProfiloServizio
        Dim oDataset As New DataSet


        Me.HDNhasServizi.Value = False
        Me.HDN_TipoComunitaID.Value = TipoComunitaID
        Me.HDNcmnt_ID.Value = ComunitaID

        Try
            Dim i, totale As Integer

            oProfilo.Id = Me.DDLprofilo.SelectedValue
            oDataset = oProfilo.ElencaServiziByComunita(Session("LinguaID"), TipoComunitaID, ComunitaID, OrganizzazioneID)

            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                Me.HDNhasServizi.Value = True
                Me.CBLservizi.Items.Clear()


                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDisabled"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDefault"))
                If Ricalcola Then
                    Me.HDNserviziSelezionati.Value = ""
                End If
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    oRow.Item("oCheckDisabled") = ""
                    If CBool(oRow.Item("isNonDisattivabile")) Then
                        oRow.Item("oCheckDisabled") = "disabled"
                        oRow.Item("oCheckDefault") = "checked"

                        If Not (InStr(Me.HDNserviziSelezionati.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0) Then
                            If Me.HDNserviziSelezionati.Value = "" Then
                                Me.HDNserviziSelezionati.Value = "," & oRow.Item("SRVZ_id") & ","
                            Else
                                Me.HDNserviziSelezionati.Value = Me.HDNserviziSelezionati.Value & oRow.Item("SRVZ_id") & ","
                            End If
                        End If
                    ElseIf Not CBool(oRow.Item("isAbilitato")) Then
                        oRow.Item("oCheckDisabled") = "disabled"
                        oRow.Item("oCheckDefault") = ""

                        If InStr(Me.HDNserviziSelezionati.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                            Me.HDNserviziSelezionati.Value = Me.HDNserviziSelezionati.Value.Replace("," & oRow.Item("SRVZ_ID") & ",", ",")
                        End If
                    Else
                        If Ricalcola Then
                            If CBool(oRow.Item("isDefault")) Then
                                oRow.Item("oCheckDefault") = "checked"

                                If Me.HDNserviziSelezionati.Value = "" Then
                                    Me.HDNserviziSelezionati.Value = "," & oRow.Item("SRVZ_id") & ","
                                Else
                                    Me.HDNserviziSelezionati.Value = Me.HDNserviziSelezionati.Value & oRow.Item("SRVZ_id") & ","
                                End If
                            Else
                                oRow.Item("oCheckDefault") = ""
                            End If
                        Else
                            If InStr(Me.HDNserviziSelezionati.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                                oRow.Item("oCheckDefault") = "checked"
                            Else
                                oRow.Item("oCheckDefault") = ""
                            End If
                        End If
                    End If
                Next
                Me.RPTservizio.DataSource = oDataset
                Me.RPTservizio.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_DatiProfili(ByVal TipoComunitaID As Integer, Optional ByVal SelezionatoID As Integer = -1)
        Dim oProfilo As New COL_ProfiloServizio
        Dim oDataset As New DataSet
        Dim i, totale As Integer

        Try
            Dim IsResponsabile, isAttuale As Boolean

            oDataset = oProfilo.ElencaByComunita(Me.HDNcmnt_ID.Value, Me.HDN_PersonaID.Value, Me.HDN_ResponsabileID.Value, Session("LinguaID"), TipoComunitaID)
            totale = oDataset.Tables(0).Rows.Count - 1
            For i = 0 To totale
                Dim oRow As DataRow
                Dim oListItem As New ListItem

                oRow = oDataset.Tables(0).Rows(i)
                IsResponsabile = CBool(oRow.Item("IsResponsabile"))
                isAttuale = CBool(oRow.Item("isAttuale"))

                oListItem.Value = oRow.Item("PRFS_ID")
                If Me.HDN_ResponsabileID.Value = Me.HDN_PersonaID.Value Then
                    If Me.HDNcmnt_ID.Value = 0 Then
                        oListItem.Text = oRow.Item("PRFS_Nome")
                    ElseIf isAttuale Then
                        oListItem.Text = Me.oResourceServizi.getValue("Profilo.IsAttuale") & oRow.Item("PRFS_Nome")
                    Else
                        oListItem.Text = oRow.Item("PRFS_Nome")
                    End If
                Else
                    If Me.HDNcmnt_ID.Value = 0 Then
                        If IsResponsabile Then
                            oListItem.Text = Me.oResourceServizi.getValue("Profilo.IsResponsabile") & oRow.Item("PRFS_Nome")
                        Else
                            oListItem.Text = oRow.Item("PRFS_Nome")
                        End If
                    Else
                        If IsResponsabile Then
                            oListItem.Text = Me.oResourceServizi.getValue("Profilo.IsResponsabile") & oRow.Item("PRFS_Nome")
                        ElseIf isAttuale Then
                            oListItem.Text = Me.oResourceServizi.getValue("Profilo.IsAttuale") & oRow.Item("PRFS_Nome")
                        Else
                            oListItem.Text = oRow.Item("PRFS_Nome")
                        End If
                    End If
                End If
                Me.DDLprofilo.Items.Add(oListItem)
            Next
        Catch ex As Exception

        End Try

        Try
            Me.DDLprofilo.SelectedValue = SelezionatoID
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RPTservizio_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTservizio.ItemCreated
        If IsNothing(oResourceServizi) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                oResourceServizi.setLabel(e.Item.FindControl("LBservizio_t"))
            Catch ex As Exception

            End Try
            Try
                oResourceServizi.setLabel(e.Item.FindControl("LBattiva_t"))
            Catch ex As Exception

            End Try
        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

            Try
                oResourceServizi.setCheckBox(e.Item.FindControl("CBXservizioAttiva"))
            Catch ex As Exception

            End Try
            Try
                Dim oCheckbox As CheckBox
                Dim UniqueID As String
                oCheckbox = e.Item.FindControl("CBXservizioAttiva")

                UniqueID = e.Item.DataItem("SRVZ_ID")
                oCheckbox.ID = "CB_" & UniqueID
                oCheckbox.Attributes.Add("onclick", "SelectFromNameAndAssocia(this,'" & e.Item.DataItem("SRVZ_ID") & "');return true;")

                If e.Item.DataItem("oCheckDefault") = "checked" Then
                    oCheckbox.Checked = True
                Else
                    oCheckbox.Checked = False
                End If
                If e.Item.DataItem("oCheckDisabled") = "disabled" Then
                    oCheckbox.Enabled = False
                Else
                    oCheckbox.Enabled = True
                End If
            Catch ex As Exception

            End Try
        End If

    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        oResourceServizi = New ResourceManager

        oResourceServizi.UserLanguages = code
        oResourceServizi.ResourcesName = "pg_UC_Fase4DefinizioneServizi"
        oResourceServizi.Folder_Level1 = "Comunita"
        oResourceServizi.Folder_Level2 = "UC_WizardComunita"
        oResourceServizi.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        If IsNothing(oResourceServizi) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        With oResourceServizi
            .setLabel(Me.LBsceltaServizio)
            .setLabel(Me.LBservizio_t)
            .setRadioButtonList(Me.RBLsceltaServizio, 0)
            .setRadioButtonList(Me.RBLsceltaServizio, 1)
        End With
    End Sub
#End Region

    Private Sub RBLsceltaServizio_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLsceltaServizio.SelectedIndexChanged
        If Me.RBLsceltaServizio.SelectedValue = 0 Then
            Me.DDLprofilo.Visible = False
            Me.Bind_DatiServiziDefault(True, Me.HDN_TipoComunitaID.Value, Me.HDN_ORGN_ID.Value, Me.HDNcmnt_ID.Value)
        Else
            Me.DDLprofilo.Visible = True
            Me.Bind_DatiServiziPersonali(True, Me.HDN_TipoComunitaID.Value, Me.HDNcmnt_ID.Value, Me.HDN_ORGN_ID.Value)
        End If
        RaiseEvent AggiornamentoVisualizzazione(AbilitaPulsanti)
    End Sub

    Private Sub DDLprofilo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLprofilo.SelectedIndexChanged


        Me.Bind_DatiServiziPersonali(True, Me.HDN_TipoComunitaID.Value, Me.HDNcmnt_ID.Value, Me.HDN_ORGN_ID.Value)

        RaiseEvent AggiornamentoVisualizzazione(AbilitaPulsanti)
    End Sub
    Private Function AbilitaPulsanti() As Boolean
        Dim HasSelezionati As Boolean = False
        Try
            If Me.HDNhasServizi.Value = True Then
                If Not (Me.HDNserviziSelezionati.Value = "" Or Me.HDNserviziSelezionati.Value = "," Or Me.HDNserviziSelezionati.Value = ",,") Then
                    HasSelezionati = True
                End If
            End If

        Catch ex As Exception
            HasSelezionati = False
        End Try
        Return HasSelezionati
    End Function

    Public Function RegistraImpostazioni(ByVal ComunitaID As Integer) As WizardComunita_Message
        Dim oComunita As New COL_Comunita
        Dim iResponse As WizardComunita_Message = WizardComunita_Message.ErroreServizi

        Try
            oComunita.Id = ComunitaID
            oComunita.Estrai()
            If oComunita.Errore = Errori_Db.None Then
                Dim ListaServizi As String
                Dim ElencoServiziID As String()
                ListaServizi = Me.HDNserviziSelezionati.Value

                If ListaServizi = "" Or ListaServizi = "," Or ListaServizi = ",," Then
                    iResponse = WizardComunita_Message.ServiziDefault
                Else
                    If Me.RBLsceltaServizio.SelectedValue = 0 Then
                        oComunita.DefinisciServiziDefault(ListaServizi)
                    Else
                        oComunita.AssociaProfiloServizi(Session("objPersona").id, Me.DDLprofilo.SelectedValue, ListaServizi)
                    End If
                    If oComunita.Errore = Errori_Db.None Then
                        iResponse = WizardComunita_Message.ServiziAttivati
                    Else
                        iResponse = WizardComunita_Message.ServiziDefault
                    End If
                End If
            Else
                iResponse = WizardComunita_Message.NessunaComunita
            End If
        Catch ex As Exception
            iResponse = WizardComunita_Message.ErroreServizi
        End Try
        Return iResponse
    End Function

    Private Function GetNomiServiziSelezionati() As String
        Try
            Dim ComunitaID, TipoComunitaID, OrganizzazioneID As Integer
            Dim oDataset As New DataSet

            Try
                ComunitaID = Me.HDNcmnt_ID.Value
            Catch ex As Exception
                ComunitaID = 0
            End Try
            Try
                TipoComunitaID = Me.HDN_TipoComunitaID.Value
            Catch ex As Exception

            End Try
            Try
                OrganizzazioneID = Me.HDN_ORGN_ID.Value
            Catch ex As Exception

            End Try

            If Me.RBLsceltaServizio.SelectedValue = 0 Then
                Dim oServizio As New COL_Servizio
                If ComunitaID > 0 Then
                    oDataset = COL_Servizio.ElencaByComunita(ComunitaID, Session("LinguaID"))
                Else
                    oDataset = oServizio.ElencaByTipoComunita(TipoComunitaID, OrganizzazioneID, Session("LinguaID"))
                End If
            Else
                Dim oProfilo As New COL_ProfiloServizio
                oDataset = oProfilo.ElencaServiziByComunita(Session("LinguaID"), TipoComunitaID, ComunitaID, OrganizzazioneID)
            End If

            Dim ListaServizi As String = ""
            Dim totale, i As Integer
            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    If CBool(oRow.Item("isNonDisattivabile")) Then
                        If ListaServizi = "" Then
                            ListaServizi = oRow.Item("SRVZ_Nome")
                        Else
                            ListaServizi &= ", " & oRow.Item("SRVZ_Nome")
                        End If
                    Else

                        If InStr(Me.HDNserviziSelezionati.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                            If ListaServizi = "" Then
                                ListaServizi = oRow.Item("SRVZ_Nome")
                            Else
                                ListaServizi &= ", " & oRow.Item("SRVZ_Nome")
                            End If
                        End If
                    End If
                Next
            End If
            Return ListaServizi
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Class