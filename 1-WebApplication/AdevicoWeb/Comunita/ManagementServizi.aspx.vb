Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.UCServices


Public Class ManagementServizi
    Inherits System.Web.UI.Page

    Private oResource As ResourceManager
	Private _OldPageUtility As OLDpageUtility

	Private ReadOnly Property Utility() As OLDpageUtility
		Get
			If IsNothing(_OldPageUtility) Then
				_OldPageUtility = New OLDpageUtility(Me.Context)
			End If
			Return _OldPageUtility
		End Get
	End Property

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    Private Enum StatusPermessi
        Definiti = 0
        ListaCompleta = 1
        ListaDefault = 2
    End Enum

#Region "Sezione Servizi Default"
    Protected WithEvents TBLdatiPrincipali As System.Web.UI.WebControls.Table
    Protected WithEvents LBavviso As System.Web.UI.WebControls.Label
    Protected WithEvents TBRdefault As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBpaginaDefault_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLpagineDefault As System.Web.UI.WebControls.DropDownList

    Protected WithEvents BTNmodifica As System.Web.UI.WebControls.Button
    Protected WithEvents BTNsalvaDefault As System.Web.UI.WebControls.Button
#End Region


#Region "Sezione Profili"
    Protected WithEvents TBRprofilo As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBsceltaServizio As System.Web.UI.WebControls.Label
    Protected WithEvents RBLsceltaServizio As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents DDLprofilo As System.Web.UI.WebControls.DropDownList

    Protected WithEvents BTNcambiaProfilo As System.Web.UI.WebControls.Button
    Protected WithEvents BTNannullaModificheProfilo As System.Web.UI.WebControls.Button
    Protected WithEvents BTNsalvaModificheProfilo As System.Web.UI.WebControls.Button
#End Region

#Region "FORM Servizi"
    Protected WithEvents PNLservizi As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LBserviceName_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdescription_t As System.Web.UI.WebControls.Label
    Protected WithEvents DGServizi As System.Web.UI.WebControls.DataGrid
    Protected WithEvents HIDcheckbox As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LNBsalvaServizi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBimpostazioni As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBtoGestione As System.Web.UI.WebControls.LinkButton
#End Region

#Region "FORM PERMESSI"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

#Region "FORM Impostazioni"
    Protected WithEvents PNLmenuSecondario As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLimpostazioni As System.Web.UI.WebControls.Panel
    '  Protected WithEvents TBLpermessiRuoli As System.Web.UI.WebControls.Table
    Protected WithEvents LBdefinizioneServizio As System.Web.UI.WebControls.Label
    Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBsalvaImpostazioniIndietro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBLpermessiRuoli As System.Web.UI.WebControls.Table
    Protected WithEvents HDNpermessi As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LNBsalvaImpostazioni As System.Web.UI.WebControls.LinkButton
    Protected WithEvents HDNsrvz_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents RBLruoli As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LBlegendaRuoli As System.Web.UI.WebControls.Label
    Protected WithEvents LBlegendaPermessi As System.Web.UI.WebControls.Label
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
            Me.SetCulture(Session("LinguaCode"))
        End If

        If Me.SessioneScaduta() Then
            Exit Sub
        End If


        If Page.IsPostBack = False Then
            Dim oServizio As New UCServices.Services_AmministraComunita
            Dim PermessiAssociati As String
            Dim ComunitaID As Integer
            Dim forAdmin As Boolean = False

            Dim oPersona As New COL_Persona
            oPersona = Session("objPersona")


            Try
                If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                    ComunitaID = Session("idComunita_forAdmin")
                    forAdmin = True
                Else
                    ComunitaID = Session("idComunita")
                End If
            Catch ex As Exception
                ComunitaID = Session("idComunita")
            End Try

            Try
                If forAdmin Then
                    PermessiAssociati = oPersona.GetPermessiForServizioForAdmin(ComunitaID, oServizio.Codex, False, oServizio.GetPermission_Admin, oServizio.GetPermission_Moderate, oServizio.GetPermission_GrantPermission)
                Else
                    PermessiAssociati = Permessi(oServizio.Codex, Me.Page)
                End If
                If Not (PermessiAssociati = "") Then
                    oServizio.PermessiAssociati = PermessiAssociati
                End If
            Catch ex As Exception
                oServizio.PermessiAssociati = "00000000000000000000000000000000"
            End Try

            Me.SetupInternazionalizzazione()
            If oServizio.Admin Or oServizio.Moderate Or oServizio.GrantPermission Then
                Me.PNLservizi.Visible = False
                Me.Bind_Dati(forAdmin, ComunitaID)
            Else
                Me.PNLpermessi.Visible = True
                Me.PNLservizi.Visible = False
                Me.LNBsalvaServizi.Enabled = False
                If Request.QueryString("topage") <> "" Or Request.QueryString("toTree") <> "" Then
                    Me.LNBtoGestione.Visible = True
                Else
                    Me.LNBtoGestione.Visible = False
                End If
            End If
        Else
            '    If Me.PNLservizi.Visible = True Then
            '        Me.Bind_Servizi(False)
            '    Else
            If Me.PNLimpostazioni.Visible Then
                Try
                    Me.Bind_TabellaPermessiRuoli(Me.ViewState("SRVZ_ID"))
                Catch ex As Exception
                    '            Me.Bind_Servizi(False)
                    '            Me.PNLservizi.Visible = True
                    '            Me.PNLpermessi.Visible = False
                End Try
            End If
        End If
    End Sub

    Private Function SessioneScaduta() As Boolean
        Dim oPersona As COL_Persona
        Dim isScaduta As Boolean = True
        Try
            oPersona = Session("objPersona")
            If oPersona.Id > 0 Then
                isScaduta = False
                Return False
            End If
        Catch ex As Exception

		End Try


        If isScaduta Then
            Dim alertMSG As String
            alertMSG = oResource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
            End If
			'Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
			'Me.Response.Redirect("./../index.aspx", True)



            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & Me.Utility.GetDefaultLogoutPage & "');</script>")

            Return True
        Else
            Try
                Dim CMNT_ID As Integer = 0
                Try
                    If Session("AdminForChange") = True Then
                        CMNT_ID = Session("idComunita_forAdmin")
                    Else
                        CMNT_ID = Session("idComunita")
                    End If
                Catch ex As Exception
                    Try
                        CMNT_ID = Session("idComunita")
                    Catch ex2 As Exception
                        CMNT_ID = 0
                    End Try

                End Try

                If CMNT_ID <= 0 Then
                    Me.ExitToLimbo()
                    Return True
                End If
            Catch ex As Exception
                Me.ExitToLimbo()
                Return True
            End Try
        End If
    End Function
    Private Sub ExitToLimbo()
        Session("Limbo") = True
        Session("ORGN_id") = 0
        Session("IdRuolo") = ""
        Session("ArrPermessi") = ""
        Session("RLPC_ID") = ""

        Session("AdminForChange") = False
        Session("CMNT_path_forAdmin") = ""
        Session("idComunita_forAdmin") = ""
        Session("TPCM_ID") = ""
        Me.Response.Expires = 0
        Me.Utility.RedirectToUrl("Comunita/Entratacomunita.aspx")
    End Sub
    Private Sub SetStartupScript()
        Dim oScript As String
        oScript = "<script language=Javascript>" & vbCrLf
        oScript = oScript & "elencoCampi[9]='" & Replace(Me.HIDcheckbox.UniqueID, ":", "_") & "';" & vbCrLf
        oScript = oScript & "</script>" & vbCrLf
        If (Not Me.Page.ClientScript.IsClientScriptBlockRegistered("clientScriptRicevuti")) Then
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScriptRicevuti", oScript)
        End If
    End Sub

    Private Sub LNBtoGestione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBtoGestione.Click
        If Request.QueryString("topage") <> "" Then
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true&topage=true")
        ElseIf Request.QueryString("toTree") <> "" Then
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true&toTree=true")
        Else
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true")
        End If
    End Sub


#Region "Bind_Dati"
    Private Sub Bind_Dati(ByVal forAdmin As Boolean, ByVal ComunitaID As Integer)
        Me.PNLpermessi.Visible = False
        Me.PNLservizi.Visible = True

        If forAdmin Then
            Dim nomeComunita As String = ""
            nomeComunita = COL_Comunita.EstraiNomeBylingua(ComunitaID, Session("LinguaID"))
            If Len(nomeComunita) > 50 Then
                nomeComunita = nomeComunita.Substring(0, 48) & " .. .. "
            End If
            'oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo.comunita.text")
            'Me.LBtitolo.Text = Replace(Me.LBtitolo.Text, "#%#", nomeComunita)

            Me.Master.ServiceTitle = Replace(oResource.getValue("LBtitolo.comunita.text"), "#%#", nomeComunita)
            Me.LNBtoGestione.Visible = True
        Else
            'oResource.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = Me.oResource.getValue("LBtitolo.text")
            Me.LNBtoGestione.Visible = False
        End If
        Me.LNBsalvaServizi.Enabled = True

        Me.Bind_SezioneDefault()
        Me.Bind_SezioneProfilo(ComunitaID)
        Me.Bind_Servizi(True)
        Me.SetStartupScript()
    End Sub

    Private Sub SelezionaPaginaDefault()
        Try
            Dim oComunita As New COL_Comunita
            Dim percorso, codice As String
            Dim PaginaID As Integer
            oComunita.GetDefaultPage(Session("idComunita"), percorso, codice, PaginaID)
            If PaginaID > 0 Then
                Me.DDLpagineDefault.SelectedValue = PaginaID
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub Bind_PagineDefault(Optional ByVal SelezionaID As Integer = -1)
        Dim oComunita As New COL_Comunita

        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        Me.DDLpagineDefault.Items.Clear()
        Try
            Dim i, totale As Integer
            Dim oDataset As New DataSet
            oDataset = oComunita.ElencoPagineDefault(Session("idComunita"), Session("LinguaID"))
            totale = oDataset.Tables(0).Rows.Count
            For i = 0 To totale - 1
                Dim oRow As DataRow
                oRow = oDataset.Tables(0).Rows(i)

                If oRow.Item("SRVZ_Attivato") = False Or oRow.Item("SRVC_isAbilitato") = False Then
                    oRow("DFLP_Nome") = oRow("DFLP_Nome") & " (" & Me.oResource.getValue("disattivato") & ")"
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
            End If
        Catch ex As Exception
            ' aggiungere riga "nessuno"
        End Try
    End Sub
    Private Sub Bind_Servizi(ByVal Ricalcola As Boolean) '
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona
        Dim oDataset As New DataSet


        Try
            Dim i, totale
            Try
                If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                    oComunita.Id = Session("idComunita_forAdmin")
                Else
                    oComunita.Id = Session("idComunita")
                End If
            Catch ex As Exception
                oComunita.Id = Session("idComunita")
            End Try

            oPersona = Session("objPersona")
            oDataset = oComunita.ElencaServizi(oPersona.Lingua.Id)

            totale = oDataset.Tables(0).Rows.Count
            If oDataset.Tables(0).Rows.Count > 0 Then

                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDisabled"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDefault"))
                If Ricalcola Then
                    Me.HIDcheckbox.Value = ""
                End If
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    oRow.Item("oCheckDisabled") = ""
                    If CBool(oRow.Item("isNonDisattivabile")) Then
                        oRow.Item("oCheckDisabled") = "disabled"
                        oRow.Item("oCheckDefault") = "checked"

                        If Not (InStr(Me.HIDcheckbox.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0) Then
                            If Me.HIDcheckbox.Value = "" Then
                                Me.HIDcheckbox.Value = "," & oRow.Item("SRVZ_id") & ","
                            Else
                                Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & oRow.Item("SRVZ_id") & ","
                            End If
                        End If
                    ElseIf Not CBool(oRow.Item("isAbilitato")) Then
                        oRow.Item("oCheckDisabled") = "disabled"
                        oRow.Item("oCheckDefault") = ""

                        If InStr(Me.HIDcheckbox.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                            Me.HIDcheckbox.Value = Me.HIDcheckbox.Value.Replace("," & oRow.Item("SRVZ_ID") & ",", ",")
                        End If
                    Else
                        If Ricalcola Then
                            If CBool(oRow.Item("isDefault")) Then
                                oRow.Item("oCheckDefault") = "checked"

                                If Me.HIDcheckbox.Value = "" Then
                                    Me.HIDcheckbox.Value = "," & oRow.Item("SRVZ_id") & ","
                                Else
                                    Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & oRow.Item("SRVZ_id") & ","
                                End If
                            Else
                                oRow.Item("oCheckDefault") = ""
                            End If
                        Else
                            If InStr(Me.HIDcheckbox.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                                oRow.Item("oCheckDefault") = "checked"
                            Else
                                oRow.Item("oCheckDefault") = ""
                            End If
                        End If
                    End If
                Next
                Dim oDataView As New DataView
                oDataView = oDataset.Tables(0).DefaultView
                oDataView.Sort = "isAbilitato DESC,isNonDisattivabile ASC, SRVZ_Nome "
                Me.DGServizi.DataSource = oDataView
                Me.DGServizi.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_ServiziProfili(ByVal Ricalcola As Boolean) '
        Dim oProfilo As New COL_ProfiloServizio
        Dim oPersona As New COL_Persona
        Dim oDataset As New DataSet
        Dim ListaServizi As String = ""

        Try
            Dim i, totale As Integer
            Dim oComunita As New COL_Comunita
            Try
                If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                    oComunita.Id = Session("idComunita_forAdmin")
                Else
                    oComunita.Id = Session("idComunita")
                End If
            Catch ex As Exception
                oComunita.Id = Session("idComunita")
            End Try

            oProfilo.Id = Me.DDLprofilo.SelectedValue
            oComunita.Estrai()
            oDataset = oProfilo.ElencaServiziByComunita(Session("LinguaID"), oComunita.TipoComunita.ID, oComunita.Id, oComunita.Organizzazione.Id)

            totale = oDataset.Tables(0).Rows.Count
            If oDataset.Tables(0).Rows.Count > 0 Then

                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDisabled"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDefault"))

                ListaServizi = Me.HIDcheckbox.Value
                If Ricalcola Then
                    Me.HIDcheckbox.Value = ""
                End If
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    oRow.Item("oCheckDisabled") = ""
                    If CBool(oRow.Item("isNonDisattivabile")) Then
                        oRow.Item("oCheckDisabled") = "disabled"
                        oRow.Item("oCheckDefault") = "checked"

                        If Not (InStr(Me.HIDcheckbox.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0) Then
                            If Me.HIDcheckbox.Value = "" Then
                                Me.HIDcheckbox.Value = "," & oRow.Item("SRVZ_id") & ","
                            Else
                                Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & oRow.Item("SRVZ_id") & ","
                            End If
                        End If
                    ElseIf Not CBool(oRow.Item("isAbilitato")) Then
                        oRow.Item("oCheckDisabled") = "disabled"
                        oRow.Item("oCheckDefault") = ""

                        If InStr(Me.HIDcheckbox.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                            Me.HIDcheckbox.Value = Me.HIDcheckbox.Value.Replace("," & oRow.Item("SRVZ_ID") & ",", ",")
                        End If
                    Else
                        If Ricalcola Then
                            If CBool(oRow.Item("isDefault")) Then
                                oRow.Item("oCheckDefault") = "checked"

                                If Me.HIDcheckbox.Value = "" Then
                                    Me.HIDcheckbox.Value = "," & oRow.Item("SRVZ_id") & ","
                                Else
                                    Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & oRow.Item("SRVZ_id") & ","
                                End If
                            Else
                                oRow.Item("oCheckDefault") = ""
                            End If
                        Else
                            If InStr(Me.HIDcheckbox.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                                oRow.Item("oCheckDefault") = "checked"
                            Else
                                oRow.Item("oCheckDefault") = ""
                            End If
                        End If
                    End If
                Next
                Dim oDataView As New DataView
                oDataView = oDataset.Tables(0).DefaultView
                oDataView.Sort = "isAbilitato DESC,isNonDisattivabile ASC, SRVZ_Nome "
                Me.DGServizi.DataSource = oDataView
                Me.DGServizi.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_ServiziDefault(ByVal Ricalcola As Boolean) '
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona
        Dim oDataset As New DataSet


        Try
            Dim i, totale
            Try
                If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                    oComunita.Id = Session("idComunita_forAdmin")
                Else
                    oComunita.Id = Session("idComunita")
                End If
            Catch ex As Exception
                oComunita.Id = Session("idComunita")
            End Try

            oPersona = Session("objPersona")
            oComunita.Estrai()
            oDataset = oComunita.TipoComunita.ServiziAssociati(oComunita.Organizzazione.Id, Session("LinguaID"))


            totale = oDataset.Tables(0).Rows.Count
            If oDataset.Tables(0).Rows.Count > 0 Then

                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDisabled"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDefault"))
                If Ricalcola Then
                    Me.HIDcheckbox.Value = ""
                End If
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    oRow.Item("oCheckDisabled") = ""
                    If CBool(oRow.Item("isNonDisattivabile")) Then
                        oRow.Item("oCheckDisabled") = "disabled"
                        oRow.Item("oCheckDefault") = "checked"

                        If Not (InStr(Me.HIDcheckbox.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0) Then
                            If Me.HIDcheckbox.Value = "" Then
                                Me.HIDcheckbox.Value = "," & oRow.Item("SRVZ_id") & ","
                            Else
                                Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & oRow.Item("SRVZ_id") & ","
                            End If
                        End If
                    ElseIf Not CBool(oRow.Item("isAbilitato")) Then
                        oRow.Item("oCheckDisabled") = "disabled"
                        oRow.Item("oCheckDefault") = ""

                        If InStr(Me.HIDcheckbox.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                            Me.HIDcheckbox.Value = Me.HIDcheckbox.Value.Replace("," & oRow.Item("SRVZ_ID") & ",", ",")
                        End If
                    Else
                        If Ricalcola Then
                            If CBool(oRow.Item("isDefault")) Then
                                oRow.Item("oCheckDefault") = "checked"

                                If Me.HIDcheckbox.Value = "" Then
                                    Me.HIDcheckbox.Value = "," & oRow.Item("SRVZ_id") & ","
                                Else
                                    Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & oRow.Item("SRVZ_id") & ","
                                End If
                            Else
                                oRow.Item("oCheckDefault") = ""
                            End If
                        Else
                            If InStr(Me.HIDcheckbox.Value, "," & oRow.Item("SRVZ_ID") & ",") > 0 Then
                                oRow.Item("oCheckDefault") = "checked"
                            Else
                                oRow.Item("oCheckDefault") = ""
                            End If
                        End If
                    End If
                Next
                Dim oDataView As New DataView
                oDataView = oDataset.Tables(0).DefaultView
                oDataView.Sort = "isAbilitato DESC,isNonDisattivabile ASC, SRVZ_Nome "
                Me.DGServizi.DataSource = oDataView
                Me.DGServizi.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Bind_SezioneDefault()
        Me.DDLpagineDefault.Enabled = False

        Me.Bind_PagineDefault()
        Me.SelezionaPaginaDefault()
    End Sub
    Private Sub Bind_SezioneProfilo(ByVal ComunitaID As Integer)
        Dim SceltaDefault As Integer = 0
        Dim ProfiloID As Integer = -1
        Dim oComunita As New COL_Comunita

        oComunita.Id = ComunitaID
        ProfiloID = oComunita.GetProfiloServizioID()
        If ProfiloID > 0 Then
            SceltaDefault = 1
        End If
        Me.RBLsceltaServizio.SelectedValue = SceltaDefault
        Me.Bind_DatiProfili(ComunitaID, ProfiloID)
        If SceltaDefault <> 0 Then
            Me.DDLprofilo.Visible = True
        Else
            Me.DDLprofilo.Visible = False
        End If

        Me.RBLsceltaServizio.Enabled = False
        Me.DDLprofilo.Enabled = False
        If Me.DDLprofilo.Items.Count = 0 Then
            Me.BTNcambiaProfilo.Enabled = False
        Else
            Me.BTNcambiaProfilo.Enabled = True
        End If
    End Sub
    Private Sub Bind_DatiProfili(ByVal ComunitaID As Integer, Optional ByVal SelezionatoID As Integer = -1)
        Dim oComunita As New COL_Comunita
        Dim oDataset As New DataSet
        Try
            oComunita.Id = ComunitaID
            oDataset = oComunita.ElencoPossibiliProfili(Session("objPersona").id, Session("LinguaID"))

            Me.DDLprofilo.DataSource = oDataset
            Me.DDLprofilo.DataTextField = "PRFS_Nome"
            Me.DDLprofilo.DataValueField = "PRFS_ID"
            Me.DDLprofilo.DataBind()
        Catch ex As Exception

        End Try

        Try
            Me.DDLprofilo.SelectedValue = SelezionatoID
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Sezione Servizio Default"
    Private Sub BTNmodifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNmodifica.Click
        Me.DDLpagineDefault.Enabled = True
        Me.BTNsalvaDefault.Visible = True
        Me.BTNmodifica.Visible = False
        Me.Bind_Servizi(False)
    End Sub
    Private Sub BTNsalvaDefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsalvaDefault.Click
        Dim ComunitaID As Integer
        Try
            If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                ComunitaID = Session("idComunita_forAdmin")
            Else
                ComunitaID = Session("IdComunita")
            End If
        Catch ex As Exception
            ComunitaID = Session("IdComunita")
        End Try
        Dim oComunita As New COL_Comunita
        oComunita.SetDefaultPage(ComunitaID, DDLpagineDefault.SelectedValue)
        Me.BTNmodifica.Visible = True
        Me.BTNsalvaDefault.Visible = False
        Me.DDLpagineDefault.Enabled = False
        Me.Bind_Servizi(False)
    End Sub
#End Region

#Region "Sezione Profilo Servizio"
    Private Sub BTNcambiaProfilo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcambiaProfilo.Click
        If Me.DDLprofilo.Items.Count = 0 Then
            Me.RBLsceltaServizio.Enabled = False
        Else
            Me.RBLsceltaServizio.Enabled = True
        End If
        Me.DDLprofilo.Enabled = True
        Me.BTNcambiaProfilo.Visible = False
        Me.BTNannullaModificheProfilo.Visible = True
        Me.BTNsalvaModificheProfilo.Visible = True
        Me.Bind_Servizi(False)
    End Sub
    Private Sub BTNsalvaModificheProfilo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsalvaModificheProfilo.Click
        Dim ProfiloID As Integer = -1
        Dim ComunitaID As Integer
        Dim oComunita As New COL_Comunita
        Dim Aggiorna As Boolean = False

        Try
            If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                ComunitaID = Session("idComunita_forAdmin")
            Else
                ComunitaID = Session("idComunita")
            End If
        Catch ex As Exception
            ComunitaID = Session("idComunita")
        End Try

        oComunita.Id = ComunitaID
        ProfiloID = oComunita.GetProfiloServizioID()
        If ProfiloID > 0 And Me.RBLsceltaServizio.SelectedValue = 0 Then
            Aggiorna = True
            oComunita.DefinisciServiziPermessiDiSistema()
        Else
            Try
                If ProfiloID <> Me.DDLprofilo.SelectedValue And Me.RBLsceltaServizio.SelectedValue = 1 Then
                    Aggiorna = True
                    oComunita.DefinisciServiziPermessiProfiloPersonale(Session("objPersona").id, Me.DDLprofilo.SelectedValue)
                End If
            Catch ex As Exception

            End Try
        End If

        Me.DDLprofilo.Enabled = False
        Me.RBLsceltaServizio.Enabled = False
        Me.BTNcambiaProfilo.Visible = True
        Me.BTNannullaModificheProfilo.Visible = False
        Me.BTNsalvaModificheProfilo.Visible = False
        Me.Bind_Servizi(Aggiorna)

        Try
            If Aggiorna Then
                Me.Bind_PagineDefault(Me.DDLpagineDefault.SelectedValue)
            End If
        Catch ex As Exception
            If Aggiorna Then
                Me.Bind_PagineDefault()
            End If
        End Try

    End Sub
    Private Sub BTNannullaModificheProfilo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNannullaModificheProfilo.Click
        Me.DDLprofilo.Enabled = False
        Me.RBLsceltaServizio.Enabled = False
        Me.BTNcambiaProfilo.Visible = True
        Me.BTNannullaModificheProfilo.Visible = False
        Me.BTNsalvaModificheProfilo.Visible = False
        Me.Bind_Servizi(False)
    End Sub
    Private Sub RBLsceltaServizio_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLsceltaServizio.SelectedIndexChanged
        If Me.RBLsceltaServizio.SelectedValue = 0 And Me.DDLprofilo.Visible Then
            Me.DDLprofilo.Visible = False
            Bind_ServiziDefault(True)
        ElseIf Me.RBLsceltaServizio.SelectedValue = 0 Then
            Me.Bind_Servizi(True)
        ElseIf Me.DDLprofilo.Visible Then
            Bind_ServiziProfili(True)
        Else
            Bind_ServiziProfili(True)
            Me.DDLprofilo.Visible = True
        End If
    End Sub
#End Region

#Region "Sezione Elenco Servizi"
    Private Sub LNBsalvaServizi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalvaServizi.Click
        Dim oComunita As New COL_Comunita
        Dim iResponse As WizardComunita_Message = WizardComunita_Message.ErroreServizi

        Try
            Try
                If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                    oComunita.Id = Session("idComunita_forAdmin")
                Else
                    oComunita.Id = Session("idComunita")
                End If
            Catch ex As Exception
                oComunita.Id = Session("idComunita")
            End Try
            oComunita.Estrai()
            If oComunita.Errore = Errori_Db.None Then
                Dim ListaServizi As String
                Dim ElencoServiziID As String()
                ListaServizi = Me.HIDcheckbox.Value

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
					Me.UpdateCurrentPermission()
                End If
            Else
                iResponse = WizardComunita_Message.NessunaComunita
            End If
        Catch ex As Exception
            iResponse = WizardComunita_Message.ErroreServizi
        End Try
        Me.Bind_Servizi(True)
    End Sub

#Region "Gestione Griglia"
    Private Sub DGServizi_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGServizi.ItemCreated
        Dim i As Integer

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Dim oSortExspression, oSortDirection, oText, StringaMouse As String
            oSortExspression = viewstate("SortExspression")
            oSortDirection = viewstate("SortDirection")


            For i = 0 To sender.columns.count - 1
                If sender.columns(i).SortExpression <> "" Then
                    Dim oWebControl As WebControl
                    Dim oCell As New TableCell
                    Dim oLabelAfter As New System.Web.UI.WebControls.Label
                    Dim oLabelBefore As New System.Web.UI.WebControls.Label

                    oLabelBefore.font.name = "webdings"
                    oLabelBefore.font.size = FontUnit.XSmall
                    oLabelBefore.text = "&nbsp;"

                    oCell = e.Item.Cells(i)
                    If Me.DGServizi.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGServizi, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGServizi, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGServizi.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
                                If oSortDirection = "asc" Then
                                    oText = "<img src='./../images/dg/down.gif' id='Image_" & i & "' >"
                                    If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/down.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/down.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                    End If
                                    If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/down_over.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/down_over.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
                                    End If
                                Else
                                    oText = "<img src='./../images/dg/up.gif' id='Image_" & i & "' >"
                                    If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                    End If
                                    If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
                                    End If
                                End If
                                oLinkbutton.Text = oText
                                oCell.Controls.AddAt(0, oLabelAfter)
                            Catch ex As Exception
                                oCell.Controls.AddAt(0, oLabelAfter)
                            End Try
                        Else
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGServizi, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGServizi.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
                                oLinkbutton.Text = "<img src='./../images/dg/up.gif' id='Image_" & i & "' >"
                                If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                    oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
                                Else
                                    StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                    StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
                                    oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                End If
                                If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                    oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
                                Else
                                    StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                    StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
                                    oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
                                End If

                                oCell.Controls.AddAt(0, oLabelAfter)
                            Catch ex As Exception
                                oCell.Controls.AddAt(0, oLabelAfter)
                            End Try
                        End If
                    End If
                End If
            Next
        End If
        If e.Item.ItemType = ListItemType.Pager Then
            Dim oCell As TableCell
            Dim n As Integer
            oCell = CType(e.Item.Controls(0), TableCell)
            Try
                Dim oRow As TableRow
                Dim oTableCell As New TableCell
                Dim num As Integer = 1
                oRow = oCell.Parent()

                oTableCell.Controls.Add(Me.CreaLegenda)
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oCell.ColumnSpan = num
                oRow.Cells.AddAt(0, oTableCell)
                e.Item.Cells(0).Attributes.Item("colspan") = num.ToString
            Catch ex As Exception

            End Try

            For n = 0 To oCell.Controls.Count - 1 Step 2
                Dim szLnk As String
                szLnk = "System.Web.UI.WebControls.DataGridLinkButton"
                Dim oWebControl As WebControl

                oWebControl = oCell.Controls(n)

                If (oWebControl.GetType().ToString() = szLnk) Then
                    oWebControl.CssClass = "ROW_PagerLink_Small"
                End If
                Try
                    Dim oLabel As Label
                    oLabel = oWebControl
                    oLabel.Text = oLabel.Text
                    oLabel.CssClass = "ROW_PagerSpan_Small"
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "ROW_PagerLink_Small"
                    oResource.setPageDatagrid(Me.DGServizi, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"
            Dim cssRiga As String = "ROW_TD_Small"

            Try
                If CBool(e.Item.DataItem("isAbilitato")) = False Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                    cssRiga = "ROW_ItemDisattivate_Small"
                ElseIf CBool(e.Item.DataItem("isNonDisattivabile")) = True Then
                    e.Item.CssClass = "ROW_Disabilitate_Small"
                ElseIf e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                Else
                    e.Item.CssClass = "ROW_Normal_Small"
                End If
            Catch ex As Exception
                If e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                Else
                    e.Item.CssClass = "ROW_Normal_Small"
                End If
            End Try

            Try
                Dim oTBRnome As TableRow
                oTBRnome = e.Item.Cells(1).FindControl("TBRnome")

                If IsNothing(oTBRnome) = False Then
                    oTBRnome.CssClass = cssRiga
                End If
            Catch ex As Exception

            End Try

            Try
                Dim oTBRdescrizione As TableRow
                oTBRdescrizione = e.Item.Cells(1).FindControl("TBRdescrizione")

                If IsNothing(oTBRdescrizione) = False Then
                    oTBRdescrizione.CssClass = cssRiga

                    If IsDBNull(e.Item.DataItem("SRVZ_Descrizione")) Then
                        oTBRdescrizione.Visible = False
                    Else
                        Try
                            If Trim(e.Item.DataItem("SRVZ_Descrizione")) = "" Or e.Item.DataItem("SRVZ_Descrizione") = "&nbsp;" Then
                                oTBRdescrizione.Visible = False
                            End If
                        Catch ex As Exception
                            oTBRdescrizione.Visible = False
                        End Try
                    End If
                End If
            Catch ex As Exception

            End Try

            Dim oLNBimpostazioni As LinkButton

            Dim oLBseparatore As Label
            Try
                oLBseparatore = e.Item.Cells(0).FindControl("LBseparatore")
                oLNBimpostazioni = e.Item.Cells(0).FindControl("LNBimpostazioni")

                Me.oResource.setLinkButton(oLNBimpostazioni, True, True)

                oLBseparatore.CssClass = cssRiga
                oLNBimpostazioni.CssClass = cssLink

                oLBseparatore.Visible = False
                oLNBimpostazioni.Visible = False
                oLNBimpostazioni.CommandArgument = e.Item.DataItem("SRVZ_Nome")
            Catch ex As Exception

            End Try

            Try
                Dim oCheck As HtmlControls.HtmlInputCheckBox
                oCheck = e.Item.Cells(1).FindControl("CBXservizioAttivato")
                If Not IsNothing(oCheck) Then
                    Try
                        If InStr(Me.HIDcheckbox.Value, "," & e.Item.DataItem("SRVZ_ID") & ",") > 0 Then
                            oCheck.Checked = True
                        Else
                            oCheck.Checked = False
                        End If
                    Catch ex As Exception
                        oCheck.Checked = False
                    End Try
                    oCheck.Value = e.Item.DataItem("SRVZ_ID")
                    Try
                        If e.Item.DataItem("isNonDisattivabile") = True Or e.Item.DataItem("isAbilitato") = False Then
                            oCheck.Disabled = True
                        Else
                            oCheck.Disabled = False
                        End If
                    Catch ex As Exception

                    End Try

                    If Not (oCheck.Checked = False And oCheck.Disabled = True) Then
                        oLBseparatore.Visible = True
                        oLNBimpostazioni.Visible = True
                    End If
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub DGServizi_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGServizi.PageIndexChanged
        Me.DGServizi.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Servizi(False)
    End Sub
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGServizi.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = viewstate("SortExspression")
        oSortDirection = viewstate("SortDirection")
        viewstate("SortExspression") = e.SortExpression

        If e.SortExpression = oSortExpression Then
            If viewstate("SortDirection") = "asc" Then
                viewstate("SortDirection") = "desc"
            Else
                viewstate("SortDirection") = "asc"
            End If
        End If
        Me.Bind_Servizi(False)
    End Sub

    Private Function CreaLegenda() As Table
        Dim oTable As New Table
        Dim oRow As New TableRow
        Dim oTableCell As New TableCell

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If


        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disattivate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResource.getValue("NONattivato")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)


        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disabilitate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResource.getValue("NONdisattivabile")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function

    Private Sub DGServizi_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGServizi.ItemCommand
        Dim SRVZ_Codice As String
        Dim oStringa() As String

        If e.CommandName = "impostazioni" Then
            Dim ServizioID As Integer
            Dim oServizio As New COL_Servizio
            ServizioID = CInt(DGServizi.DataKeys.Item(e.Item.ItemIndex))
            oServizio.ID = ServizioID
            oServizio.EstraiByLingua(Session("LinguaID"))

            Me.PNLservizi.Visible = False
            Me.PNLimpostazioni.Visible = True
            Me.PNLmenuSecondario.Visible = True
            Me.PNLmenu.Visible = False

            'Me.LBtitolo.Text = oResource.getValue("definizioneServizio")
            'Me.LBtitolo.Text = Replace(Me.LBtitolo.Text, "#%#", oServizio.Nome)
            Me.Master.ServiceTitle = Replace(oResource.getValue("definizioneServizio"), "#%#", oServizio.Nome)

            Me.HDNsrvz_ID.Value = ServizioID
            Me.RBLruoli.SelectedIndex = 0
            Me.Bind_Impostazioni(ServizioID, oServizio.Codice)
        Else
            Me.Bind_Servizi(False)
        End If
    End Sub
#End Region

#End Region

#Region "Bind_Dati"
    Private Function AggiornaPermessi()
        Dim oServizio As New COL_Servizio
        Dim oDataSet As New DataSet
        Dim i, totale, CMNT_ID, TPRL_ID As Integer
        Dim isAdmin As Boolean = False

        Try
            If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                isAdmin = True
                CMNT_ID = Session("idComunita_forAdmin")
            Else
                CMNT_ID = Session("IdComunita")
                TPRL_ID = Session("IdRuolo")
            End If
        Catch ex As Exception
            CMNT_ID = Session("IdComunita")
            TPRL_ID = Session("IdRuolo")
        End Try

        If isAdmin = False Then
            oDataSet = oServizio.ElencaByTipoRuoloByComunita(TPRL_ID, CMNT_ID)
            totale = oDataSet.Tables(0).Rows.Count - 1

            Dim ArrPermessi(totale, 2) As String
            For i = 0 To totale
                Dim oRow As DataRow
                oRow = oDataSet.Tables(0).Rows(i)
                ArrPermessi(i, 0) = oRow.Item("SRVZ_Codice") 'CODICE servizio
                ArrPermessi(i, 1) = oRow.Item("SRVZ_ID") 'id servizio
                ArrPermessi(i, 2) = oRow.Item("LKSC_Permessi") 'valore servizio
            Next
            Session("ArrPermessi") = ArrPermessi
        End If
    End Function

    Private Sub Bind_Impostazioni(ByVal SRVZ_ID As Integer, ByVal SRVZ_Codice As String)
        Dim oAdminComunita As Services_AmministraComunita
        Dim oAdminGlobale As Services_AmministrazioneGlobale
        Dim oBacheca As Services_Bacheca
        Dim oChat As Services_CHAT
        Dim oDiarioLezioni As Services_DiarioLezioni
        Dim oServices_Cover As Services_Cover

        Dim oEventi As Services_Eventi
        Dim oFile As Services_File
        Dim oForum As Services_Forum
        Dim oGallery As Services_Gallery

        Dim oGestioneIscritti As Services_GestioneIscritti
        Dim oRaccoltaLink As Services_RaccoltaLink
        Dim oMail As Services_Mail
        Dim oStatistiche As Services_Statistiche
        Dim oPostIt As Services_PostIt
        Dim Permessi As String

        Dim oComunita As COL_Comunita
        Dim CMNT_ID, TPRL_ID As Integer

        Try
            If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                CMNT_ID = Session("idComunita_forAdmin")
                TPRL_ID = Main.TipoRuoloStandard.AdminComunità
            Else
                CMNT_ID = Session("IdComunita")
                TPRL_ID = Session("IdRuolo")
            End If
        Catch ex As Exception
            CMNT_ID = Session("IdComunita")
            TPRL_ID = Session("IdRuolo")
        End Try


        Try
            Permessi = oComunita.GetPermessiForServizio(TPRL_ID, CMNT_ID, SRVZ_ID)
        Catch ex As Exception
            Permessi = "00000000000000000000000000000000"
        End Try


        Dim HasPermessi As Boolean

        HasPermessi = False
        Select Case SRVZ_Codice
            Case Services_Cover.Codex
                oServices_Cover = New Services_Cover
                oServices_Cover.PermessiAssociati = Permessi
                HasPermessi = oServices_Cover.GrantPermission
            Case oChat.Codex
                oChat = New Services_CHAT
                oChat.PermessiAssociati = Permessi
                HasPermessi = oChat.GestionePermessi
            Case oAdminComunita.Codex
                oAdminComunita = New Services_AmministraComunita
                oAdminComunita.PermessiAssociati = Permessi
                HasPermessi = oAdminComunita.GrantPermission
            Case oAdminGlobale.Codex
                oAdminGlobale = New Services_AmministrazioneGlobale
                oAdminGlobale.PermessiAssociati = Permessi
                HasPermessi = True 'oAdminGlobale.GestionePermess
            Case oBacheca.Codex
                oBacheca = New Services_Bacheca(Permessi)
                HasPermessi = oBacheca.GrantPermission
            Case oDiarioLezioni.Codex
                oDiarioLezioni = New Services_DiarioLezioni
                oDiarioLezioni.PermessiAssociati = Permessi
                HasPermessi = oDiarioLezioni.GrantPermission

            Case oEventi.Codex
                oEventi = New Services_Eventi
                oEventi.PermessiAssociati = Permessi
                HasPermessi = oEventi.GrantPermission
            Case oFile.Codex
                oFile = New Services_File
                oFile.PermessiAssociati = Permessi
                HasPermessi = oFile.GrantPermission
            Case oForum.Codex
                oForum = New Services_Forum
                oForum.PermessiAssociati = Permessi
                HasPermessi = True 'oForum.GestionePermess
            Case oGallery.Codex
                oGallery = New Services_Gallery
                oGallery.PermessiAssociati = Permessi
                HasPermessi = oGallery.GrantPermission
            Case oGestioneIscritti.Codex
                oGestioneIscritti = New Services_GestioneIscritti
                oGestioneIscritti.PermessiAssociati = Permessi
                HasPermessi = oGestioneIscritti.GrantPermission

            Case oMail.Codex
                oMail = New Services_Mail
                oMail.PermessiAssociati = Permessi
                HasPermessi = True 'oMail.GestionePermess
            Case oPostIt.Codex
                oPostIt = New Services_PostIt
                oPostIt.PermessiAssociati = Permessi
                HasPermessi = oPostIt.GestionePermessi
            Case oRaccoltaLink.Codex
                oRaccoltaLink = New Services_RaccoltaLink
                oRaccoltaLink.PermessiAssociati = Permessi
                HasPermessi = oRaccoltaLink.GrantPermission
            Case oStatistiche.Codex
                oStatistiche = New Services_Statistiche
                oStatistiche.PermessiAssociati = Permessi
                HasPermessi = oStatistiche.GrantPermission
        End Select

        Me.PNLpermessi.Visible = False
        Me.ViewState("SRVZ_ID") = SRVZ_ID
        Me.TBLpermessiRuoli.Rows.Clear()
        Me.TBLpermessiRuoli.Rows.Clear()
        Me.Bind_TabellaPermessiRuoli(SRVZ_ID, True)
        '    Me.Bind_TabellaPermessiRuoli(SRVZ_ID, True)
    End Sub
#End Region

#Region "Impostazione Servizi"
    Private Sub Bind_TabellaPermessiRuoli(ByVal SRVZ_ID As Integer, Optional ByVal start As Boolean = False, Optional ByVal RuoloDefault As Integer = -1)
        Dim oServizio As New COL_Servizio
        Dim oDataset As New DataSet
        Dim oDatasetRuoli As New DataSet

        oServizio.ID = SRVZ_ID
        Try
            Dim i, j, totalePermessi, totale, PRMS_Posizione As Integer
            Dim ARRpermServizio As String(,)
            Dim oTBrow As New TableRow
            Dim oStringaPermessi As String
            oDataset = oServizio.ElencaPermessiAssociatiByLingua(Session("LinguaID"))
            totalePermessi = oDataset.Tables(0).Rows.Count - 1


            ' Caricamento prima riga
            Me.TBLpermessiRuoli.Rows.Clear()
            Dim oTableCell As New TableCell
            oTableCell.Text = oResource.getValue("RPT_ruolo")
            oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(120)
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.CssClass = "TBLpermessi_Header"
            oTBrow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Text = oResource.getValue("RPT_permessi")
            oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(550)
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.CssClass = "TBLpermessi_Header"
            oTBrow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Text = oResource.getValue("RPT_default")
            oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(80)
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.CssClass = "TBLpermessi_Header"
            oTBrow.Cells.Add(oTableCell)


            Me.TBLpermessiRuoli.Rows.Add(oTBrow)

            Dim ListaPermessi As String = ""
            For i = 0 To totalePermessi
                Dim oRow As DataRow

                oRow = oDataset.Tables(0).Rows(i)
                ReDim Preserve ARRpermServizio(2, i)

                ARRpermServizio(0, i) = oRow.Item("PRMS_Posizione")
                If IsDBNull(oRow.Item("Nome")) Then
                    Try
                        ARRpermServizio(1, i) = oRow.Item("NomeDefault")
                    Catch ex As Exception
                        ARRpermServizio(1, i) = "--"
                    End Try
                Else
                    ARRpermServizio(1, i) = oRow.Item("Nome")
                End If
                ListaPermessi &= "&nbsp;&nbsp;&nbsp;<b>" & ARRpermServizio(1, i) & "</b>"
                If IsDBNull(oRow.Item("Descrizione")) Then
                    Try
                        ARRpermServizio(2, i) = oRow.Item("DescrizioneDefault")
                    Catch ex As Exception
                        ARRpermServizio(2, i) = ""
                    End Try
                Else
                    ARRpermServizio(2, i) = oRow.Item("Descrizione")
                End If
                ListaPermessi &= "=&nbsp;" & ARRpermServizio(2, i) & "<br>"
            Next

            Try
                If ListaPermessi <> "" Then
                    Me.LBlegendaPermessi.Attributes.Add("onmouseover", "ChangeState(event,'LayerLegenda','visible','" & Replace(ListaPermessi, "'", "\'") & "')")
                    Me.LBlegendaPermessi.Attributes.Add("onmouseout", "ChangeState(event,'LayerLegenda','hidden','')")
                End If
            Catch ex As Exception

            End Try

            'UNA volta recuperato i permessi, devo preparare la griglia con l'associazione con i ruoli

            Dim oComunita As New COL_Comunita
            Dim oRuolo As New COL_TipoRuolo
            Dim ComunitaID, RuoloID, TPRL_IDcorrente As Integer

            Try
                If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                    ComunitaID = Session("idComunita_forAdmin")
                    RuoloID = Main.TipoRuoloStandard.AdminComunità
                Else
                    ComunitaID = Session("IdComunita")
                    RuoloID = Session("IdRuolo")
                End If
            Catch ex As Exception
                ComunitaID = Session("IdComunita")
                RuoloID = Session("IdRuolo")
            End Try

            oComunita.Id = ComunitaID
            oComunita.Estrai()
            oRuolo.Id = RuoloID
            oRuolo.Estrai()


            If Me.RBLruoli.SelectedValue = 0 Then
                oDatasetRuoli = oComunita.ElencaRuoliPermessi_Definiti(SRVZ_ID, Session("LinguaID"))
            ElseIf Me.RBLruoli.SelectedValue = 1 Then
                oDatasetRuoli = oComunita.ElencaRuoliPermessi_ListaCompleta(SRVZ_ID, Session("LinguaID"))
            ElseIf Me.RBLruoli.SelectedValue = 2 Then
                oDatasetRuoli = oComunita.ElencaRuoliPermessi_ListaDefault(SRVZ_ID, Session("LinguaID"))
            End If
            totale = oDatasetRuoli.Tables(0).Rows.Count - 1
            Me.AggiornaElencoRuoliPermessi(SRVZ_ID, oDatasetRuoli, ComunitaID)

            totale = oDatasetRuoli.Tables(0).Rows.Count - 1
            If start Then
                Me.HIDcheckbox.Value = ""
            End If
            Dim uniqueID As String
            For i = 0 To totale
                Dim ClasseCSS As String
                Dim TPRL_gerarchia As Integer
                Dim oRow As DataRow
                oTBrow = New TableRow

                oRow = oDatasetRuoli.Tables(0).Rows(i)

                TPRL_IDcorrente = orow.Item("TPRL_ID")
                TPRL_gerarchia = orow.Item("TPRL_gerarchia")
                oStringaPermessi = oRow.Item("Permessi_definiti")

                Dim NomeRuolo As String
                NomeRuolo = "<img src='" & GetPercorsoApplicazione(Me.Request) & "/"
                If oRow.Item("isFromProfilo") = 1 Then
                    ClasseCSS = "TBLpermessi_RowItem"
                    NomeRuolo &= Me.oResource.getValue("PermessiProfilo")
                ElseIf oRow.Item("isDefault") = 1 And oRow.Item("Associato") = 1 Then
                    ClasseCSS = "TBLpermessi_RowItemDefinitiDefault"
                    NomeRuolo &= Me.oResource.getValue("PermessiDefault")
                ElseIf oRow.Item("Associato") = 1 Then
                    ClasseCSS = "TBLpermessi_RowItem"
                    NomeRuolo &= Me.oResource.getValue("PermessiRidefiniti")
                ElseIf oRow.Item("Associato") = 0 Then
                    ClasseCSS = "TBLpermessi_RowItemNonDefiniti"
                    NomeRuolo &= Me.oResource.getValue("PermessiNonDefiniti")
                ElseIf oRow.Item("Associato") = 2 Then
                    ClasseCSS = "TBLpermessi_RowItemNonDefinitiConRuoli"
                    NomeRuolo &= Me.oResource.getValue("PermessiNonDefinitiConRuoli")
                End If

                NomeRuolo &= oRow.Item("TPRL_nome")

                ' inserisco il nome del ruolo
                oTableCell = New TableCell
                oTableCell.Text = NomeRuolo
                oTableCell.Wrap = False
                oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(120)
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oTableCell.CssClass = ClasseCSS
                oTBrow.Cells.Add(oTableCell)


                ' inserisco Colonna Permessi
                oTableCell = New TableCell
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oTableCell.CssClass = ClasseCSS

                Dim oTablePermessi As New Table
                Dim oTablePermessiRow As New TableRow
                Dim oTablePermessiCell As New TableCell
                oTablePermessi.GridLines = GridLines.None
                For j = 0 To UBound(ARRpermServizio, 2)
                    Dim oCheckbox As New HtmlControls.HtmlInputCheckBox
                    'Dim oCheckbox As New WebControls.CheckBox
                    Dim oLabel As New Label

                    PRMS_Posizione = ARRpermServizio(0, j)
                    uniqueID = PRMS_Posizione & "_" & oRow.Item("TPRL_id") & "_" & SRVZ_ID
                    oCheckbox.ID = "CB_" & uniqueID
                    oCheckbox.Value = uniqueID

                    oLabel.ID = "LB_" & uniqueID
                    oLabel.Text = ARRpermServizio(1, j)

                    'Try
                    '    If ARRpermServizio(2, j) <> "" Then
                    '        oLabel.Attributes.Add("onmouseover", "ChangeState(event,'layer1','visible','" & Replace(ARRpermServizio(2, j), "'", "\'") & "')")
                    '        oLabel.Attributes.Add("onmouseout", "ChangeState(event,'layer1','hidden','')")
                    '    End If
                    'Catch ex As Exception

                    'End Try
                    If start Then
                        If oStringaPermessi.Substring(PRMS_Posizione, 1) = 1 Then
                            oCheckbox.Checked = True
                            If Me.HIDcheckbox.Value = "" Then
                                Me.HIDcheckbox.Value = "," & uniqueID & ","
                            Else
                                Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & uniqueID & ","
                            End If
                            oLabel.CssClass = "TBLpermessi_RowItemPermessiSelezionati"
                        Else
                            oCheckbox.Checked = False
                            oLabel.CssClass = "TBLpermessi_RowItemPermessi"
                        End If
                    Else
                        If TPRL_IDcorrente = RuoloDefault Then
                            If oStringaPermessi.Substring(PRMS_Posizione, 1) = 1 Then
                                oCheckbox.Checked = True

                                If InStr(Me.HIDcheckbox.Value, "," & uniqueID & ",") < 1 Then
                                    ' se non è già presente lo inserisco !
                                    If Me.HIDcheckbox.Value = "" Then
                                        Me.HIDcheckbox.Value = "," & uniqueID & ","
                                    Else
                                        Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & uniqueID & ","
                                    End If
                                End If
                                oLabel.CssClass = "TBLpermessi_RowItemPermessiSelezionati"
                            Else
                                oCheckbox.Checked = False
                                If InStr(Me.HIDcheckbox.Value, "," & uniqueID & ",") > 0 Then
                                    Me.HIDcheckbox.Value = Replace(Me.HIDcheckbox.Value, "," & uniqueID & ",", ",")
                                    If Me.HIDcheckbox.Value = "," Then
                                        Me.HIDcheckbox.Value = ""
                                    End If
                                End If
                                oLabel.CssClass = "TBLpermessi_RowItemPermessi"
                            End If
                        Else
                            If InStr(Me.HIDcheckbox.Value, "," & uniqueID & ",") > 0 Then
                                oCheckbox.Checked = True
                                oLabel.CssClass = "TBLpermessi_RowItemPermessiSelezionati"
                            Else
                                oCheckbox.Checked = False
                                oLabel.CssClass = "TBLpermessi_RowItemPermessi"
                            End If
                        End If
                    End If
                    If oLabel.CssClass = "TBLpermessi_RowItemPermessi" Then
                        oLabel.Text = oLabel.Text & "&nbsp;"
                    End If

                    If oRuolo.Gerarchia < TPRL_gerarchia Then
                        oCheckbox.Disabled = False
                        oCheckbox.Attributes.Add("onclick", "SelectFromNameAndAssociaIntoControl('" & oCheckbox.ID & "','" & uniqueID & "','" & Me.HIDcheckbox.ClientID & "');return true;")
                    Else
                        oCheckbox.Disabled = True
                    End If
                    If j > 0 And j Mod 4 = 0 Then
                        oTablePermessi.Rows.Add(oTablePermessiRow)
                        oTablePermessiRow = New TableRow
                    End If

                    oTablePermessiCell = New TableCell
                    oTablePermessiCell.Width = System.Web.UI.WebControls.Unit.Pixel(15)
                    oTablePermessiCell.Controls.Add(oCheckbox)
                    oTablePermessiRow.Cells.Add(oTablePermessiCell)

                    oTablePermessiCell = New TableCell
                    oTablePermessiCell.Width = System.Web.UI.WebControls.Unit.Pixel(120)
                    oTablePermessiCell.Controls.Add(oLabel)
                    oTablePermessiRow.Cells.Add(oTablePermessiCell)

                Next
                If j Mod 4 <> 0 Then
                    oTablePermessiCell = New TableCell
                    oTablePermessiCell.Text = "&nbsp;"
                    oTablePermessiCell.ColumnSpan = (4 - (j Mod 4)) * 2
                    oTablePermessiRow.Cells.Add(oTablePermessiCell)
                    oTablePermessi.Rows.Add(oTablePermessiRow)
                Else
                    oTablePermessi.Rows.Add(oTablePermessiRow)
                End If
                oTableCell.Controls.Add(oTablePermessi)
                oTBrow.Cells.Add(oTableCell)


                ' Carico colonna visualizza
                oTableCell = New TableCell
                oTableCell.HorizontalAlign = HorizontalAlign.Center

                If Me.RBLruoli.SelectedValue = Me.StatusPermessi.ListaDefault Then
                    oTableCell.Text = Me.oResource.getValue("valoriDefault")
                Else
                    Dim oLink As New HtmlAnchor
                    Dim i_link As String

                    oLink.ID = "ANC_" & "_" & oRow("TPRL_ID")
                    oLink.InnerText = "Visualizza"
					oLink.HRef = "#"
					Dim oUtility As New OLDpageUtility(Me.Context)

					i_link = oUtility.EncryptedUrl("Comunita/PermissionDetails.aspx", "ServizioID=" & SRVZ_ID & "&RuoloID=" & oRow("TPRL_ID"), SecretKeyUtil.EncType.Altro)
                    oResource.setHtmlAnchorToValue(oLink, "ANCvisualizza", True, True)
                    oLink.Attributes.Add("onClick", "window.status='';OpenWin('" & i_link & "','620','450','yes','no');return false;")
                    oLink.Attributes.Add("class", "TBLpermessi_RowLink")
                    oTableCell.Controls.Add(oLink)


                    Dim oLinkButtonDefault As New LinkButton
                    oLinkButtonDefault.ID = "LNB_" & oRow("TPRL_ID")
                    oResource.setLinkButtonForName(oLinkButtonDefault, "LNBdefault", True, True)

                    oLinkButtonDefault.CommandArgument = "default"
                    oLinkButtonDefault.CommandName = oRow("TPRL_ID") & "_" & SRVZ_ID & "_" & oRow.Item("Permessi_Default")
                    oLinkButtonDefault.CssClass = "TBLpermessi_RowLink"
                    AddHandler oLinkButtonDefault.Click, AddressOf LNBdefault_Click
                    If oRuolo.Gerarchia < TPRL_gerarchia Then
                        oLinkButtonDefault.Enabled = True
                    Else
                        oLinkButtonDefault.Enabled = False
                    End If

                    Dim oLabelSeparatore As New Label
                    oLabelSeparatore.Text = "<br><br>"
                    oTableCell.Controls.Add(oLabelSeparatore)
                    oTableCell.Controls.Add(oLinkButtonDefault)
                End If
                oTableCell.CssClass = ClasseCSS
                oTBrow.Cells.Add(oTableCell)

                Me.TBLpermessiRuoli.Rows.Add(oTBrow)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub AggiornaElencoRuoliPermessi(ByVal ServizioID As Integer, ByRef oDatasetRuoli As DataSet, ByVal ComunitaID As Integer)
        Dim oComunita As New COL_Comunita
        Dim ElencoRuoliAssociati As String = ","
        Dim oDatasetAssociati As DataSet
        Dim i, totale As Integer


        oComunita.Id = ComunitaID

        Try
            oDatasetAssociati = oComunita.ElencaRuoliUtentiPermessi(ServizioID, Session("LinguaID"))
            totale = oDatasetAssociati.Tables(0).Rows.Count - 1

            Dim oDataview As DataView
            Try
                oDataview = oDatasetRuoli.Tables(0).DefaultView
            Catch ex As Exception

            End Try

            For i = 0 To totale
                Dim oRow As DataRow
                Dim oNewRow As DataRow
                Dim oRowCorrente As DataRow


                oRowCorrente = oDatasetAssociati.Tables(0).Rows(i)
                Try

                    oDataview.RowFilter = "TPRL_ID=" & oRowCorrente.Item("TPRL_ID")
                    oRow = oDataview.Item(0).Row
                Catch ex As Exception
                    oRow = Nothing
                End Try

                If IsNothing(oRow) Then
                    oNewRow = oDatasetRuoli.Tables(0).NewRow
                    oNewRow.Item("TPRL_gerarchia") = oRowCorrente.Item("TPRL_gerarchia")
                    oNewRow.Item("TPRL_descrizione") = oRowCorrente.Item("TPRL_descrizione")
                    oNewRow.Item("TPRL_nome") = oRowCorrente.Item("TPRL_nome")
                    oNewRow.Item("TPRL_id") = oRowCorrente.Item("TPRL_id")
                    oNewRow.Item("Permessi_Definiti") = oRowCorrente.Item("Permessi_Definiti")
                    oNewRow.Item("Associato") = 2
                    oNewRow.Item("Permessi_Default") = "00000000000000000000000000000000"
                    oNewRow.Item("Permessi_Profilo") = "00000000000000000000000000000000"
                    oNewRow.Item("isFromProfilo") = 0
                    oNewRow.Item("isDefault") = 0
                    oDatasetRuoli.Tables(0).Rows.Add(oNewRow)
                    oDatasetRuoli.AcceptChanges()
                Else
                    If oRow.Item("Associato") = 0 Then
                        oRow.Item("Associato") = 2
                    End If
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub LNBsalvaImpostazioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalvaImpostazioni.Click
        Dim SRVZ_ID, CMNT_ID As Integer

        If Me.HDNsrvz_ID.Value = "" Then
            SRVZ_ID = -1
        Else
            If CInt(Me.HDNsrvz_ID.Value) Then
                SRVZ_ID = Me.HDNsrvz_ID.Value
                Dim oComunita As New COL_Comunita
                Try
                    If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                        CMNT_ID = Session("idComunita_forAdmin")
                    Else
                        CMNT_ID = Session("IdComunita")
                    End If
                Catch ex As Exception
                    CMNT_ID = Session("IdComunita")
                End Try
                oComunita.Id = CMNT_ID

                Try
                    Dim i, j, riga, TPRL_ID, totaleV, totaleO, Posizione As Integer
                    Dim oStringaPermessi, nome() As String
                    Dim Permessi() As Char
                    Dim oTableCell As TableCell

                    totaleV = Me.TBLpermessiRuoli.Rows.Count - 1

                    TPRL_ID = 0
                    For i = 1 To totaleV
                        oStringaPermessi = "00000000000000000000000000000000"
                        Permessi = oStringaPermessi.ToCharArray

                        oTableCell = Me.TBLpermessiRuoli.Rows(i).Cells(1)

                        Dim oTablePermessi As New Table
                        oTablePermessi = oTableCell.Controls(0)

                        totaleO = oTableCell.Controls.Count - 1

                        For riga = 0 To oTablePermessi.Rows.Count - 1
                            totaleO = oTablePermessi.Rows(riga).Cells.Count - 1

                            For j = 0 To totaleO Step 2
                                If oTablePermessi.Rows(riga).Cells(j).ColumnSpan > 1 Then
                                    Exit For
                                End If
                                If j = 0 Or j Mod 2 = 0 Then
                                    Dim oCheckbox As New HtmlControls.HtmlInputCheckBox

                                    Try
                                        oCheckbox = oTablePermessi.Rows(riga).Cells(j).Controls(0)
                                        nome = oCheckbox.ID.Split("_")
                                        TPRL_ID = nome(2)
                                        Posizione = nome(1)

                                        If oCheckbox.Checked Then
                                            oStringaPermessi = oStringaPermessi.Remove(Posizione, 1)
                                            If Posizione > 0 Then
                                                oStringaPermessi = oStringaPermessi.Insert(Posizione, 1)
                                            Else
                                                oStringaPermessi = oStringaPermessi.Insert(0, 1)
                                            End If
                                        Else
                                            oStringaPermessi = oStringaPermessi.Remove(Posizione, 1)
                                            If Posizione > 0 Then
                                                oStringaPermessi = oStringaPermessi.Insert(Posizione, 0)
                                            Else
                                                oStringaPermessi = oStringaPermessi.Insert(0, 0)
                                            End If
                                        End If
                                    Catch ex As Exception

                                    End Try
                                End If
                            Next
                        Next

                        If TPRL_ID <> 0 Then
                            oComunita.ChangePermessiForRuolo(TPRL_ID, CMNT_ID, SRVZ_ID, oStringaPermessi)

                        End If
                    Next
                    COL_Comunita.PurgeServiceCache(SRVZ_ID)
                Catch ex As Exception

                End Try
            End If
        End If

    End Sub
    Public Sub LNBdefault_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oDati(), Permessi As String
        Dim oComunita As New COL_Comunita
        Dim CMNT_ID, TPRL_ID, SRVZ_ID As Integer

        Try

            If sender.CommandName <> "" Then
                oDati = sender.CommandName.split("_")
            End If

            'CMNT_ID = Session("IdComunita")
            Try
                If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                    CMNT_ID = Session("idComunita_forAdmin")
                Else
                    CMNT_ID = Session("IdComunita")
                End If
            Catch ex As Exception
                CMNT_ID = Session("IdComunita")
            End Try
            TPRL_ID = oDati(0)
            SRVZ_ID = oDati(1)
            Permessi = oDati(2)
            If sender.CommandArgument = "default" Then
                oComunita.ChangePermessiForRuolo(TPRL_ID, CMNT_ID, SRVZ_ID, Permessi)
            End If
            COL_Comunita.PurgeServiceCache(SRVZ_ID)
        Catch ex As Exception

        End Try
        Try
            SRVZ_ID = Me.ViewState("SRVZ_ID")
            If SRVZ_ID = 0 Then
                SRVZ_ID = Me.HDNsrvz_ID.Value
            End If
        Catch ex As Exception

        End Try

        Me.Bind_TabellaPermessiRuoli(SRVZ_ID, False, TPRL_ID)
    End Sub

    Private Sub LNBindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietro.Click
        Me.PNLpermessi.Visible = False
        Me.PNLimpostazioni.Visible = False
        Me.PNLservizi.Visible = True
        Me.ViewState("SRVZ_ID") = ""
        Me.Bind_Servizi(True)
        Me.PNLmenuSecondario.Visible = False
        Me.PNLmenu.Visible = True
        Try
            'oResource.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = Me.oResource.getValue("LBtitolo.text")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub LNBsalvaImpostazioniIndietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalvaImpostazioniIndietro.Click
        Dim SRVZ_ID, CMNT_ID As Integer

        If Me.HDNsrvz_ID.Value = "" Then
            SRVZ_ID = -1
        Else
            If CInt(Me.HDNsrvz_ID.Value) Then
                SRVZ_ID = Me.HDNsrvz_ID.Value
                Dim oComunita As New COL_Comunita
                Try
                    If Session("AdminForChange") = True And Session("idComunita_forAdmin") > 0 Then
                        CMNT_ID = Session("idComunita_forAdmin")
                    Else
                        CMNT_ID = Session("IdComunita")
                    End If
                Catch ex As Exception
                    CMNT_ID = Session("IdComunita")
                End Try
                oComunita.Id = CMNT_ID

                Try
                    Dim i, j, riga, TPRL_ID, totaleV, totaleO, Posizione As Integer
                    Dim oStringaPermessi, nome() As String
                    Dim Permessi() As Char
                    Dim oTableCell As TableCell

                    totaleV = Me.TBLpermessiRuoli.Rows.Count - 1

                    TPRL_ID = 0
                    For i = 1 To totaleV
                        oStringaPermessi = "00000000000000000000000000000000"
                        Permessi = oStringaPermessi.ToCharArray

                        oTableCell = Me.TBLpermessiRuoli.Rows(i).Cells(1)

                        Dim oTablePermessi As New Table
                        oTablePermessi = oTableCell.Controls(0)

                        totaleO = oTableCell.Controls.Count - 1

                        For riga = 0 To oTablePermessi.Rows.Count - 1
                            If riga = 2 Then
                                riga = 2
                            End If
                            totaleO = oTablePermessi.Rows(riga).Cells.Count - 1

                            For j = 0 To totaleO Step 2
                                If oTablePermessi.Rows(riga).Cells(j).ColumnSpan > 1 Then
                                    Exit For
                                End If
                                If j = 0 Or j Mod 2 = 0 Then
                                    Dim oCheckbox As New HtmlControls.HtmlInputCheckBox

                                    Try
                                        oCheckbox = oTablePermessi.Rows(riga).Cells(j).Controls(0)
                                        nome = oCheckbox.ID.Split("_")
                                        TPRL_ID = nome(2)
                                        Posizione = nome(1)

                                        If oCheckbox.Checked Then
                                            oStringaPermessi = oStringaPermessi.Remove(Posizione, 1)
                                            If Posizione > 0 Then
                                                oStringaPermessi = oStringaPermessi.Insert(Posizione, 1)
                                            Else
                                                oStringaPermessi = oStringaPermessi.Insert(0, 1)
                                            End If
                                        Else
                                            oStringaPermessi = oStringaPermessi.Remove(Posizione, 1)
                                            If Posizione > 0 Then
                                                oStringaPermessi = oStringaPermessi.Insert(Posizione, 0)
                                            Else
                                                oStringaPermessi = oStringaPermessi.Insert(0, 0)
                                            End If
                                        End If
                                    Catch ex As Exception

                                    End Try
                                End If
                            Next
                        Next

                        If TPRL_ID <> 0 Then
                            oComunita.ChangePermessiForRuolo(TPRL_ID, CMNT_ID, SRVZ_ID, oStringaPermessi)
                        End If
                    Next
                    COL_Comunita.PurgeServiceCache(SRVZ_ID)
                Catch ex As Exception

                End Try

            End If
        End If
        Me.PNLpermessi.Visible = False
        Me.PNLimpostazioni.Visible = False
        Me.PNLservizi.Visible = True
        Me.ViewState("SRVZ_ID") = ""
        Me.Bind_Servizi(True)
        Me.PNLmenuSecondario.Visible = False
        Me.PNLmenu.Visible = True
        Try
            'oResource.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = Me.oResource.getValue("LBtitolo.text")
        Catch ex As Exception

        End Try
    End Sub
#End Region


#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = code
        oResource.ResourcesName = "pg_ManagementServizi"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = oResource.getValue("LBtitolo")

            .setLabel(Me.LBNopermessi)
            .setLinkButton(Me.LNBindietro, True, True)
            .setLabel(Me.LBserviceName_t)
            .setLabel(Me.LBdescription_t)
            .setLabel(Me.LBlegendaRuoli)
            .setLinkButton(Me.LNBtoGestione, True, True)
            .setLinkButton(Me.LNBindietro, True, True)
            .setLinkButton(Me.LNBsalvaImpostazioni, True, True)
            .setLinkButton(Me.LNBsalvaImpostazioniIndietro, True, True)
            .setLinkButton(Me.LNBsalvaServizi, True, True)

            .setRadioButtonList(Me.RBLruoli, 0)
            .setRadioButtonList(Me.RBLruoli, 1)
            .setRadioButtonList(Me.RBLruoli, 2)
            .setLabel(Me.LBavviso)
            .setLabel(Me.LBpaginaDefault_t)
            .setButton(Me.BTNmodifica, True)
            .setButton(Me.BTNsalvaDefault, True)
            .setLabel(Me.LBsceltaServizio)
            .setRadioButtonList(Me.RBLsceltaServizio, 0)
            .setRadioButtonList(Me.RBLsceltaServizio, 1)
            .setButton(Me.BTNcambiaProfilo, True)
            .setButton(Me.BTNannullaModificheProfilo, True)
            .setButton(Me.BTNsalvaModificheProfilo, True, , True, True)
            .setLabel(Me.LBlegendaPermessi)
            Dim LegendaEstesa, Messaggio As String

            LegendaEstesa = .getValue("LegendaEstesa")

            If LegendaEstesa <> "" Then
                LegendaEstesa = Replace(LegendaEstesa, "'", "\'")
                LegendaEstesa = Replace(LegendaEstesa, "#percorso#", GetPercorsoApplicazione(Me.Request) & "/images/dg/")
                Messaggio = "ChangeState(event,'LayerLegenda','visible','" & LegendaEstesa & "')"
                Me.LBlegendaRuoli.Attributes.Add("onmouseover", Messaggio)
                Me.LBlegendaRuoli.Attributes.Add("onmouseout", "ChangeState(event,'LayerLegenda','hidden','')")
            End If

        End With
    End Sub
#End Region

    Private Sub RBLruoli_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLruoli.SelectedIndexChanged
        Try
            Me.Bind_TabellaPermessiRuoli(Me.HDNsrvz_ID.Value, True)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DDLprofilo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLprofilo.SelectedIndexChanged
        Me.Bind_ServiziProfili(True)
	End Sub


	Private Sub UpdateCurrentPermission()
		Dim oListaServizi As GenericCollection(Of PlainServizioComunita)
		oListaServizi = PlainServizioComunita.ElencaByComunita(Utility.CurrentRoleID, Utility.WorkingCommunityID)

		If oListaServizi.Count > 0 Then
			Dim ArrPermessi(oListaServizi.Count - 1, 2) As String
			Dim indice As Integer = 0
			For Each oServizio As PlainServizioComunita In oListaServizi
				ArrPermessi(indice, 0) = oServizio.Codice
				ArrPermessi(indice, 1) = oServizio.ID
				ArrPermessi(indice, 2) = oServizio.Permessi
				indice += 1
			Next
			Session("ArrPermessi") = ArrPermessi
		Else
			Session("ArrPermessi") = Nothing
		End If
	End Sub


    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class