Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.UCServices



Public Class WizardNuovoIscritto
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

    Protected WithEvents CTRLmessages As Global.Comunita_OnLine.UC_ActionMessages

#Region "TEMP"
    Private _ProfileService As lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService
    Private ReadOnly Property ProfileService() As lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService
        Get
            If IsNothing(_ProfileService) Then
                _ProfileService = New lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(PageUtility.CurrentContext)
            End If
            Return _ProfileService
        End Get
    End Property
    Private Property HideSourceSelector As Boolean
        Get
            Return ViewStateOrDefault("HideSourceSelector", False)
        End Get
        Set(value As Boolean)
            ViewState("HideSourceSelector") = value
        End Set
    End Property
    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
#End Region
    Private _PageUtility As OLDpageUtility

    Private ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property

    Private Enum FasiNuovaIscrizione
        Fase1sceltaSorgente = 1
        Fase2sceltaUtente = 2
        Fase3definizioneRuolo = 3
    End Enum
    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum
    Private _OldPageUtility As OLDpageUtility

    Private ReadOnly Property Utility() As OLDpageUtility
        Get
            If IsNothing(_OldPageUtility) Then
                _OldPageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _OldPageUtility
        End Get
    End Property

#Region "Generali"
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents TBRmenu As System.Web.UI.WebControls.TableRow
    Protected WithEvents BTNgoToManagement As System.Web.UI.WebControls.Button
    Protected WithEvents BTNtornaPaginaElenco As System.Web.UI.WebControls.Button
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

    Protected WithEvents HDN_ComunitaPadreID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDabilitato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDattivato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDtutti As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents HDN_filtroRuolo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoRicerca As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroValore As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "Navigazione"
    Protected WithEvents PNLnavigazione As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNindietro As System.Web.UI.WebControls.Button
    Protected WithEvents BTNavanti As System.Web.UI.WebControls.Button
    Protected WithEvents BTNconferma As System.Web.UI.WebControls.Button

    Protected WithEvents PNLnavigazione2 As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNindietro2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNavanti2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNconferma2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNgoToManagementAlto As System.Web.UI.WebControls.Button
    Protected WithEvents BTNgoToManagementBasso As System.Web.UI.WebControls.Button
    Protected WithEvents BTNtornaPaginaElencoAlto As System.Web.UI.WebControls.Button
    Protected WithEvents BTNtornaPaginaElencoBasso As System.Web.UI.WebControls.Button
#End Region

#Region "Fase1"
    Protected WithEvents TBLsorgente As System.Web.UI.WebControls.Table
    Protected WithEvents LBscelta_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBLimporta As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents TBRcomunita As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBcomunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBinfoSorgente As System.Web.UI.WebControls.Label
    Protected WithEvents CTRLsorgenteComunita As Comunita_OnLine.UC_FiltroComunitaByServizio_NEW
#End Region

#Region "Fase2"
    Protected WithEvents TBLutenti As System.Web.UI.WebControls.Table

#Region "Filtro_Lettere"
    Protected WithEvents LKBtutti As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBaltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBa As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBb As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBc As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBe As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBf As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBg As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBh As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBj As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBk As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBn As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBp As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBq As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBr As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBs As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBt As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBu As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBv As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBw As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBx As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBy As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBz As System.Web.UI.WebControls.LinkButton
#End Region

#Region "Filtro Generale"
    Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button
    Protected WithEvents DDLTipoRuolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoPersona As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBtipoRuolo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoPersona_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBnumeroRecord_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoRicerca_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBvalore_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBXaggiorna As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
#End Region

    Protected WithEvents DGPersone As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PNLnessunUtente As System.Web.UI.WebControls.Panel
    Protected WithEvents LBnessunUtente As System.Web.UI.WebControls.Label


    'New
    Protected WithEvents PNLuserEnabled As System.Web.UI.WebControls.Panel
    Protected WithEvents LBLuserEnabled_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLuserEnabled As System.Web.UI.WebControls.DropDownList


#End Region

#Region "Fase3"
    Protected WithEvents TBLruolo As System.Web.UI.WebControls.Table
    Protected WithEvents TBLdatiRuolo As System.Web.UI.WebControls.Table
    Protected WithEvents LBdescrizione As System.Web.UI.WebControls.Label
    Protected WithEvents LBtiporuoloAggiungi_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoRuoloAggiungi As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBRresponsabile As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBresponsabile_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBXresponsabile As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TBRprofilo As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBsceltaRuoli_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBLsceltaRuoli As System.Web.UI.WebControls.RadioButtonList
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
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona


        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            Me.SetupInternazionalizzazione()
        End If


        Dim oServizioIscritti As New Services_GestioneIscritti
        Dim PermessiAssociati As String

        Try
            oPersona = Session("objPersona")
            If Not Page.IsPostBack Then
                'Session("azione") = "load"
                Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioIscritti.Codex)
                oServizioIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
            Else
                If Me.ViewState("PermessiAssociati") = "" Then
                    Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioIscritti.Codex)
                End If
                oServizioIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
            End If
        Catch ex As Exception
            oServizioIscritti.PermessiAssociati = "00000000000000000000000000000000"
        End Try
        If Not Page.IsPostBack Then
            Session("azione") = "loaded"
            Try
                If oServizioIscritti.Admin Or oServizioIscritti.Management Or oServizioIscritti.AddUser Then
                    Me.Bind_Dati()
                Else
                    Me.Reset_ToNoPermessi()
                End If
            Catch ex As Exception
                Me.Reset_ToNoComunita()
            End Try
        End If

        Me.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Page.Form.DefaultFocus = Me.TXBValore.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.TXBValore.UniqueID

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
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
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
        Me.Response.Redirect(GetPercorsoApplicazione(Me.Request) & "/EntrataComunita.aspx", True)
    End Sub

    Private Function GetPermessiForPage(ByVal Codex As String) As String
        Dim oPersona As New COL_Persona
        Dim oRuoloComunita As New COL_RuoloPersonaComunita
        Dim CMNT_id As Integer

        Dim PermessiAssociati As String

        Try
            oPersona = Session("objPersona")
            PermessiAssociati = Permessi(Codex, Me.Page)

            If (PermessiAssociati = "") Then
                PermessiAssociati = "00000000000000000000000000000000"
            End If
        Catch ex As Exception
            PermessiAssociati = "00000000000000000000000000000000"
        End Try

        If Session("AdminForChange") = False Then
            Try
                CMNT_id = Session("IdComunita")
                PermessiAssociati = Permessi(Codex, Me.Page)
                If (PermessiAssociati = "") Then
                    PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                PermessiAssociati = "00000000000000000000000000000000"
            End Try
            Try
                oRuoloComunita.EstraiByLinguaDefault(CMNT_id, oPersona.Id)
                Me.ViewState("PRSN_TPRL_Gerarchia") = oRuoloComunita.TipoRuolo.Gerarchia

            Catch ex As Exception
                Me.ViewState("PRSN_TPRL_Gerarchia") = "99999"
            End Try
        Else
            Dim oComunita As New COL_Comunita
            CMNT_id = Session("idComunita_forAdmin")
            oComunita.Id = CMNT_id

            'Vengo dalla pagina di amministrazione generale
            Try
                PermessiAssociati = oComunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, CMNT_id, Codex)
                If (PermessiAssociati = "") Then
                    PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                PermessiAssociati = "00000000000000000000000000000000"
            End Try
        End If

        Return PermessiAssociati
    End Function

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_WizardNuovoIscritto"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(Me.LBNopermessi)
            .setLinkButton(LKBaltro, True, True)
            .setLinkButton(LKBtutti, True, True)
            .setButton(Me.BTNCerca)
            .setCheckBox(Me.CBXaggiorna)
            .setButton(Me.BTNgoToManagement, True)
            .setButton(Me.BTNtornaPaginaElenco, True)
            .setButton(Me.BTNindietro, True)
            .setButton(Me.BTNconferma, True)
            .setButton(Me.BTNavanti, True)
            .setButton(Me.BTNindietro2, True)
            .setButton(Me.BTNconferma2, True)
            .setButton(Me.BTNavanti2, True)

            .setButton(Me.BTNgoToManagementAlto, True)
            .setButton(Me.BTNgoToManagementBasso, True)
            .setButton(Me.BTNtornaPaginaElencoAlto, True)
            .setButton(Me.BTNtornaPaginaElencoBasso, True)

            .setLabel(LBnessunUtente)


            .setLabel(Me.LBinfoSorgente)
            .setLabel(LBtipoRuolo_t)
            .setLabel(LBtipoPersona_t)
            .setLabel(LBnumeroRecord_t)
            .setLabel(LBtipoRicerca_t)
            .setLabel(LBvalore_t)
            .setDropDownList(DDLTipoRicerca, -2)
            .setDropDownList(DDLTipoRicerca, -3)
            .setDropDownList(DDLTipoRicerca, -4)

            .setDropDownList(DDLTipoRicerca, -7)
            .setButton(BTNCerca, False)
            .setCheckBox(CBXaggiorna)
            .setHeaderDatagrid(Me.DGPersone, 2, "PRSN_matricola", True)
            .setHeaderDatagrid(Me.DGPersone, 3, "cognome", True)
            .setHeaderDatagrid(Me.DGPersone, 4, "nome", True)
            .setHeaderDatagrid(Me.DGPersone, 6, "PRSN_mail", True)
            .setHeaderDatagrid(Me.DGPersone, 7, "TPRL_nome", True)
            .setHeaderDatagrid(Me.DGPersone, 9, "TPPR_descrizione", True)

            Me.BTNavanti.Attributes.Add("onclick", "return UserSelezionati('" & Replace(Me.oResource.getValue("messaggioSelezione"), "'", "\'") & "');")
            Me.BTNavanti2.Attributes.Add("onclick", "return UserSelezionati('" & Replace(Me.oResource.getValue("messaggioSelezione"), "'", "\'") & "');")
            .setLabel(LBtiporuoloAggiungi_t)
            .setLabel(LBresponsabile_t)
            .setCheckBox(CBXresponsabile)
            .setLinkButton(Me.LNBchiudiFiltro, True, True)
            .setLinkButton(Me.LNBapriFiltro, True, True)
            .setRadioButtonList(Me.RBLimporta, 0)
            .setRadioButtonList(Me.RBLimporta, 1)

            .setLabel(LBsceltaRuoli_t)
            .setRadioButtonList(Me.RBLsceltaRuoli, 1)
            .setRadioButtonList(Me.RBLsceltaRuoli, 5)

            .setLabel(LBLuserEnabled_t)
            .setDropDownList(DDLuserEnabled, -1)
            .setDropDownList(DDLuserEnabled, 1)
            .setDropDownList(DDLuserEnabled, 2)
            .setDropDownList(DDLuserEnabled, 3)
            .setDropDownList(DDLuserEnabled, 4)

        End With
    End Sub
#End Region

#Region "Bind Dati"
    Private Sub Bind_Dati()

        Me.BTNgoToManagementAlto.Visible = False
        Me.BTNgoToManagementBasso.Visible = False
        Try
            If Session("AdminForChange") = True Then
                Me.BTNgoToManagementAlto.Visible = True
                Me.BTNgoToManagementBasso.Visible = True
            End If
        Catch ex As Exception
            
        End Try

        Me.BTNtornaPaginaElenco.Visible = False
        Me.BTNgoToManagement.Visible = False
        Me.HDN_ComunitaPadreID.Value = -1
        Me.Setup_ComunitàOrigine()
        Me.Bind_TipoPersona()
    End Sub

	Private Sub Setup_ComunitàOrigine()
		Dim oComunitaAttuale As New COL_Comunita
		Dim ComunitaId, ComunitaPadreId As Integer
		Dim ComunitaPath, ComunitaPathPadre As String

		Try
			If Session("AdminForChange") = False Then
				ComunitaId = Session("IdComunita")
				Try
					Dim ArrComunita(,) As String = Session("ArrComunita")
					ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
				Catch ex As Exception

				End Try
			Else
				ComunitaId = Session("idComunita_forAdmin")
				ComunitaPath = Session("CMNT_path_forAdmin")
			End If
		Catch ex As Exception

		End Try

		oComunitaAttuale.Id = ComunitaId
		oComunitaAttuale.Estrai()

		If oComunitaAttuale.Errore = Errori_Db.None Then
			If Me.HDN_ComunitaPadreID.Value = 0 Or oComunitaAttuale.IdPadre = 0 Then
				Me.RBLimporta.SelectedIndex = 0
				Me.HDN_ComunitaPadreID.Value = 0
				Me.TBRcomunita.Visible = False
				Me.BTNavanti.Enabled = True
				Me.BTNavanti2.Enabled = True
			ElseIf Me.HDN_ComunitaPadreID.Value = -1 Then
				ComunitaPathPadre = Replace(ComunitaPath, "." & ComunitaId & ".", ".")
				If ComunitaPathPadre = "" Or ComunitaPathPadre = "." Or ComunitaPathPadre = ".." Then
					ComunitaPadreId = 0
				Else
					Dim oArrayPadri() As String
					oArrayPadri = ComunitaPathPadre.Split(".")
					ComunitaPadreId = oArrayPadri(1) 'oArrayPadri(oArrayPadri.Length - 2)
					ComunitaPathPadre = "." & ComunitaPadreId.ToString & "."
				End If
				Me.CTRLsorgenteComunita.ShowFiltro = True
				Me.CTRLsorgenteComunita.ServizioCode = UCServices.Services_GestioneIscritti.Codex
				Me.CTRLsorgenteComunita.SetupControl(ComunitaPadreId, oComunitaAttuale.Livello, oComunitaAttuale.Id, ComunitaPath)
				Me.CTRLsorgenteComunita.Visible = True

				Me.HDN_ComunitaPadreID.Value = Me.CTRLsorgenteComunita.ComunitaID
				Me.RBLimporta.SelectedIndex = 1
				Me.TBRcomunita.Visible = True
				If Me.CTRLsorgenteComunita.ComunitaID > 0 Then
					Me.BTNavanti.Enabled = True
				Else
					Me.BTNavanti.Enabled = False
				End If
				Me.BTNavanti2.Enabled = Me.BTNavanti.Enabled
			End If
			If oComunitaAttuale.GetProfiloServizioID > 0 Then
				Me.Bind_RuoloNuovoUtente(Main.FiltroRuoli.ForProfiloComunita_NoGuest)
				Me.RBLsceltaRuoli.SelectedValue = Main.FiltroRuoli.ForProfiloComunita_NoGuest
				Me.TBRprofilo.Visible = True
			Else
				Me.Bind_RuoloNuovoUtente(Main.FiltroRuoli.ForTipoComunita_NoGuest)
				Me.RBLsceltaRuoli.SelectedValue = Main.FiltroRuoli.ForTipoComunita_NoGuest
				Me.TBRprofilo.Visible = False
			End If

            If oComunitaAttuale.IdPadre = 0 Then
                HideSourceSelector = True
                Me.Reset_ToelencoUtenti()
                Me.Setup_ElencoUtenti()
            Else
                Me.Reset_ToSorgenteComunita()
            End If

		Else
			Me.Reset_ToNoComunita()
		End If
	End Sub
    Private Sub Setup_ElencoUtenti()
        Dim RuoloAttualeID As Integer
        Try
            RuoloAttualeID = Me.DDLTipoRuolo.SelectedValue
        Catch ex As Exception
            RuoloAttualeID = -1
        End Try

        Me.LBtipoRuolo_t.Visible = False
        Me.DDLTipoRuolo.Visible = False
        Me.LBtipoPersona_t.Visible = False
        Me.DDLTipoPersona.Visible = False

        If Me.HDN_ComunitaPadreID.Value <> 0 Then
            LBtipoRuolo_t.Visible = True
            Me.DDLTipoRuolo.Visible = True
            Me.Bind_TipoRuolo(RuoloAttualeID)
        Else
            Me.LBtipoPersona_t.Visible = True
            Me.DDLTipoPersona.Visible = True
        End If

        Me.ViewState("intCurPage") = 0
        Me.ViewState("intAnagrafica") = -1
        Me.LKBtutti.CssClass = "lettera_Selezionata"
        Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
        Me.DDLTipoPersona.AutoPostBack = Me.CBXaggiorna.Checked
        Me.DDLTipoRuolo.AutoPostBack = Me.CBXaggiorna.Checked
        Me.DDLuserEnabled.AutoPostBack = Me.CBXaggiorna.Checked

        Me.TXBValore.Text = ""
        Try
            If Me.DDLTipoPersona.Visible Then
                Me.HDN_filtroRuolo.Value = Me.DDLTipoPersona.SelectedValue
            Else
                Me.HDN_filtroRuolo.Value = Me.DDLTipoRuolo.SelectedValue
            End If
        Catch ex As Exception
            Me.HDN_filtroRuolo.Value = -1
        End Try
        Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
        Me.HDN_filtroValore.Value = Me.TXBValore.Text
        Me.HDabilitato.Value = ""

        Try
            Dim oComunita As New COL_Comunita
            If Session("AdminForChange") = False Then
                oComunita.Id = Session("IdComunita")
            Else
                oComunita.Id = Session("idComunita_forAdmin")
            End If
            Me.DDLtipoRuoloAggiungi.SelectedValue = oComunita.RuoloDefault
        Catch exct As Exception

        End Try
        Me.Bind_Griglia(True)
    End Sub

    Private Sub Bind_RuoloNuovoUtente(ByVal oFiltroRuoli As Main.FiltroRuoli, Optional ByVal SelectedRole As Integer = -1)
        Me.DDLtipoRuoloAggiungi.Items.Clear()
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oComunita As New COL_Comunita

            If Session("AdminForChange") = False Then
                oComunita.Id = Session("IdComunita")
            Else
                oComunita.Id = Session("idComunita_forAdmin")
            End If
            oDataset = oComunita.RuoliAssociabiliByPersona(Session("objPersona").id, oFiltroRuoli)

            Totale = oDataset.Tables(0).Rows.Count()
            If Totale > 0 Then
                Totale = Totale - 1
                For i = 0 To Totale
                    If oDataset.Tables(0).Rows(i).Item("TPRL_ID") > 0 Then
						Dim oListItem As New ListItem
                        If IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRL_nome")) Then
                            oListItem.Text = "--"
                        Else
                            oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRL_nome")
                        End If
                        oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRL_ID")
                        Me.DDLtipoRuoloAggiungi.Items.Add(oListItem)
                    End If
                Next

                Try
                    If SelectedRole > -1 Then
                        Me.DDLtipoRuoloAggiungi.SelectedValue = SelectedRole
                    End If
                Catch ex As Exception
                    Try
                        Me.DDLtipoRuoloAggiungi.SelectedValue = oComunita.RuoloDefault
                    Catch exct As Exception

                    End Try
                End Try
            Else
                Me.DDLtipoRuoloAggiungi.Items.Add(New ListItem("< nessun ruolo >", -1))
            End If
        Catch ex As Exception
            Me.DDLtipoRuoloAggiungi.Items.Add(New ListItem("< nessun ruolo >", -1))
        End Try
        oResource.setDropDownList(Me.DDLtipoRuoloAggiungi, -1)
    End Sub
    Private Sub Bind_TipoRuolo(Optional ByVal SelectedRole As Integer = -1)
        Dim oDataSet As New DataSet

        Try
            Dim oComunita As New COL_Comunita
            oComunita.Id = Me.HDN_ComunitaPadreID.Value
            oDataSet = oComunita.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForUtenti_NoGuest)

            If oDataSet.Tables(0).Rows.Count > 0 Then
                DDLTipoRuolo.DataSource = oDataSet
                DDLTipoRuolo.DataTextField() = "TPRL_nome"
                DDLTipoRuolo.DataValueField() = "TPRL_id"
                DDLTipoRuolo.DataBind()

                If Me.DDLTipoRuolo.Items.Count > 1 Then
                    DDLTipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", Main.TipoRuoloStandard.Tutti_NoGuest))
                End If
                Try
                    If SelectedRole > 0 Then
                        Me.DDLTipoRuolo.SelectedValue = SelectedRole
                    End If
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception
            DDLTipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", Main.TipoRuoloStandard.Tutti_NoGuest))
        End Try
        oResource.setDropDownList(Me.DDLTipoRuolo, Main.TipoRuoloStandard.Tutti_NoGuest)
    End Sub
    Private Sub Bind_TipoPersona()
        Dim oDataset As DataSet
        Dim oTipoPersona As New COL_TipoPersona
        Dim oListItem As New ListItem

        Try
            oDataset = oTipoPersona.Elenca(Session("LinguaID"), Main.FiltroElencoTipiPersona.WithUserAssociated_NoGuest)
            DDLTipoPersona.Items.Clear()
            If oDataset.Tables(0).Rows.Count > 0 Then
                DDLTipoPersona.DataSource = oDataset
                DDLTipoPersona.DataTextField() = "TPPR_descrizione"
                DDLTipoPersona.DataValueField() = "TPPR_id"
                DDLTipoPersona.DataBind()

                DDLTipoPersona.Items.Insert(0, New ListItem("-- Tutti --", Main.TipoPersonaStandard.Tutti_NoGuest))
                Me.DDLTipoPersona.SelectedValue = 1
            Else
                DDLTipoPersona.Items.Insert(0, New ListItem("-- Tutti --", Main.TipoPersonaStandard.Tutti_NoGuest))
            End If
        Catch ex As Exception
            DDLTipoPersona.Items.Insert(0, New ListItem("-- Tutti --", Main.TipoPersonaStandard.Tutti_NoGuest))
        End Try
        oResource.setDropDownList(Me.DDLTipoPersona, Main.TipoPersonaStandard.Tutti_NoGuest)
    End Sub

    Private Sub Bind_Griglia(Optional ByVal Filtraggio As Boolean = False)
        Dim ComunitaPadreID As Integer
        Dim dsTable As New DataSet
        Dim oComunita As New COL_Comunita

        ComunitaPadreID = Me.HDN_ComunitaPadreID.Value
        If Session("AdminForChange") = False Then
            oComunita.Id = Session("IdComunita")
        Else
            oComunita.Id = Session("idComunita_forAdmin")
        End If


        'definisco il filtraggio per lettera !
        Dim oFiltroLettera As Main.FiltroAnagrafica
        Dim oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti
        Try
            If IsNothing(Me.ViewState("intAnagrafica")) Then
                oFiltroLettera = Main.FiltroAnagrafica.tutti
            Else
                oFiltroLettera = CType(Me.ViewState("intAnagrafica"), Main.FiltroAnagrafica)
            End If
        Catch ex As Exception
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            oFiltroLettera = Main.FiltroAnagrafica.tutti
            Me.LKBtutti.CssClass = "lettera_Selezionata"
            Me.ViewState("intAnagrafica") = -1
        End Try

        Dim TipoRuoloPersonaID, TipoRicercaID As Integer
        Dim Valore As String = ""
        If Filtraggio Then
            Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
            Me.HDN_filtroValore.Value = Me.TXBValore.Text
            If Me.DDLTipoPersona.Visible Then
                Me.HDN_filtroRuolo.Value = Me.DDLTipoPersona.SelectedValue
            Else
                Me.HDN_filtroRuolo.Value = Me.DDLTipoRuolo.SelectedValue
            End If
        End If

        Try
            TipoRuoloPersonaID = Me.HDN_filtroRuolo.Value
        Catch ex As Exception
            TipoRuoloPersonaID = -1
        End Try
        Try
            TipoRicercaID = Me.HDN_filtroTipoRicerca.Value
        Catch ex As Exception
            TipoRicercaID = -1
        End Try

        Try
            Valore = Me.HDN_filtroValore.Value
            If Valore <> "" Then
                Valore = Trim(Valore)
            End If
        Catch ex As Exception
            Valore = ""
        End Try

        Try
            If Valore <> "" Then
                Select Case TipoRicercaID
                    Case Main.FiltroRicercaAnagrafica.nome
                        oFiltroRicerca = Main.FiltroRicercaAnagrafica.nome
                    Case Main.FiltroRicercaAnagrafica.dataNascita
                        oFiltroRicerca = Main.FiltroRicercaAnagrafica.dataNascita
                    Case Main.FiltroRicercaAnagrafica.cognome
                        oFiltroRicerca = Main.FiltroRicercaAnagrafica.cognome

                        oFiltroLettera = Main.FiltroAnagrafica.tutti
                        Me.LKBtutti.CssClass = "lettera_Selezionata"
                        Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                        Me.ViewState("intAnagrafica") = -1
                    Case Main.FiltroRicercaAnagrafica.nomeCognome
                        oFiltroRicerca = Main.FiltroRicercaAnagrafica.nomeCognome
                    Case Main.FiltroRicercaAnagrafica.matricola
                        oFiltroRicerca = Main.FiltroRicercaAnagrafica.matricola

                        oFiltroLettera = Main.FiltroAnagrafica.tutti
                        Me.LKBtutti.CssClass = "lettera_Selezionata"
                        Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                        Me.ViewState("intAnagrafica") = -1
                    Case Main.FiltroRicercaAnagrafica.dataNascita
                        Try
                            If IsDate(Valore) Then
                                Valore = Main.DateToString(Valore, False)
                                oFiltroRicerca = Main.FiltroRicercaAnagrafica.dataNascita
                            End If
                        Catch ex As Exception

                        End Try
                    Case Main.FiltroRicercaAnagrafica.login
                        oFiltroRicerca = Main.FiltroRicercaAnagrafica.login

                        oFiltroLettera = Main.FiltroAnagrafica.tutti
                        Me.LKBtutti.CssClass = "lettera_Selezionata"
                        Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                        Me.ViewState("intAnagrafica") = -1
                    Case Else
                        oFiltroRicerca = Main.FiltroRicercaAnagrafica.tutti
                End Select

            End If
        Catch ex As Exception
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            oFiltroLettera = Main.FiltroAnagrafica.tutti
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End Try

        Dim profileFilter As ProfileStatusFilter = ProfileStatusFilter.all

        If (PNLuserEnabled.Visible) Then

            Select Case DDLuserEnabled.SelectedValue
                Case "1"
                    profileFilter = ProfileStatusFilter.onlyactive
                Case "2"
                    profileFilter = ProfileStatusFilter.allnotavailable
                Case "3"
                    profileFilter = ProfileStatusFilter.onlywaiting
                Case "4"
                    profileFilter = ProfileStatusFilter.onlydisabled
                Case Else
                    profileFilter = ProfileStatusFilter.all
            End Select

        End If


        Try
            oComunita.Estrai()
            If ComunitaPadreID = 0 Then 'vuol dire che è un'istituzione
                dsTable = oComunita.ElencaNonIscritti_BySistema(Session("LinguaID"), TipoRuoloPersonaID, Valore, oFiltroLettera, oFiltroRicerca, profileFilter)
                DGPersone.Columns(7).Visible = False
                DGPersone.Columns(9).Visible = True
            Else
                DGPersone.Columns(11).Visible = False
                dsTable = oComunita.ElencaNonIscritti_MaIscrittiACmntPassata(Session("LinguaID"), TipoRuoloPersonaID, ComunitaPadreID, Valore, oFiltroLettera, Main.FiltroUtenti.NoPassantiNoCreatori, oFiltroRicerca, profileFilter)
                'If Me.DDLTipoRuolo.SelectedValue <> -1 Then
                '    DGPersone.Columns(7).Visible = False
                'Else
                DGPersone.Columns(7).Visible = True
                'End If
                DGPersone.Columns(9).Visible = False
            End If
            dsTable.Tables(0).Columns.Add(New DataColumn("oCheckAbilitato"))
            dsTable.Tables(0).Columns.Add(New DataColumn("oPRSN_datanascita"))
            dsTable.Tables(0).Columns.Add(New DataColumn("oSTDN_Matricola"))

            Dim i, totale As Integer
            totale = dsTable.Tables(0).Rows.Count
            If totale = 0 Then
                Me.DGPersone.Visible = False
                Me.PNLnessunUtente.Visible = True
            Else
                Me.DGPersone.Columns(2).Visible = False
                Me.PNLnessunUtente.Visible = False
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = dsTable.Tables(0).Rows(i)
                    oRow.Item("oPRSN_datanascita") = FormatDateTime(oRow.Item("PRSN_datanascita"), DateFormat.ShortDate)

                    If ViewState("RipristinaCheck") <> "si" Then
                        Me.HDtutti.Value += oRow.Item("PRSN_id") & ","
                    Else
                        If InStr(Me.HDabilitato.Value, "," & oRow.Item("PRSN_id") & ",") > 0 Then
                            oRow.Item("oCheckAbilitato") = "checked"
                        End If
                    End If
                    If IsDBNull(oRow.Item("STDN_Matricola")) Then
                        oRow.Item("oSTDN_Matricola") = ""
                    Else
                        If oRow.Item("STDN_Matricola") = "-1" Then
                            oRow.Item("oSTDN_Matricola") = Me.oResource.getValue("NOmatricola")
                        Else
                            oRow.Item("oSTDN_Matricola") = oRow.Item("STDN_Matricola")
                            Me.DGPersone.Columns(2).Visible = True
                        End If
                    End If
                Next

                'ordinamento delle colonne e databind della griglia
                If Me.DDLNumeroRecord.Items(0).Value < totale Then
                    Me.DGPersone.AllowPaging = True
                    Me.LBnumeroRecord_t.Visible = True
                    Me.DDLNumeroRecord.Visible = True
                    Me.DGPersone.PagerStyle.Position = PagerPosition.TopAndBottom
                Else
                    Me.LBnumeroRecord_t.Visible = False
                    Me.DDLNumeroRecord.Visible = False
                    Me.DGPersone.PagerStyle.Position = PagerPosition.Top
                End If
                Me.DGPersone.Visible = True
                ' Me.DGiscritti.CurrentPageIndex = 0
                Dim oDataview As DataView
                oDataview = dsTable.Tables(0).DefaultView
                If ViewState("SortExspression") = "" Then
                    ViewState("SortExspression") = "PRSN_Cognome"
                    ViewState("SortDirection") = "asc"
                End If
                oDataview.Sort = ViewState("SortExspression") & " " & ViewState("SortDirection")
                Me.DGPersone.DataSource = oDataview
                Me.DGPersone.PageSize = Me.DDLNumeroRecord.SelectedValue
                Me.DGPersone.DataBind()
            End If
        Catch ex As Exception
            Me.DGPersone.Visible = False
            Me.PNLnessunUtente.Visible = True

        End Try
    End Sub

#End Region

#Region "Reset Form"
    Private Sub Reset_HideAll()
        Me.PNLnavigazione2.Visible = False
        Me.PNLnavigazione.Visible = False
        Me.PNLpermessi.Visible = False
        Me.PNLnessunUtente.Visible = False
        Me.PNLcontenuto.Visible = False
        Me.TBLutenti.Visible = False
        Me.TBLruolo.Visible = False
        Me.TBLsorgente.Visible = False
        Me.BTNavanti.Visible = False
        Me.BTNconferma.Visible = False
        Me.BTNindietro.Visible = False
        Me.BTNavanti2.Visible = False
        Me.BTNconferma2.Visible = False
        Me.BTNindietro2.Visible = False
    End Sub
    Private Sub Reset_ToNoPermessi()
        Me.Reset_HideAll()
        Me.PNLpermessi.Visible = True
        Me.BTNtornaPaginaElenco.Visible = True
        If Session("AdminForChange") = True Then
            Me.BTNgoToManagement.Visible = True
        Else
            Me.BTNgoToManagement.Visible = False
        End If
    End Sub
    Private Sub Reset_ToNoComunita()
        Me.Reset_HideAll()
    End Sub
    Private Sub Reset_ToSorgenteComunita()
        Me.Reset_HideAll()
        Me.PNLnavigazione.Visible = True
        Me.PNLnavigazione2.Visible = True
        Me.PNLcontenuto.Visible = True
        'Me.oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & Me.FasiNuovaIscrizione.Fase1sceltaSorgente)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBtitolo." & Me.FasiNuovaIscrizione.Fase1sceltaSorgente)
        Me.BTNavanti.Visible = True
        Me.BTNavanti2.Visible = True
        Me.TBLsorgente.Visible = True
    End Sub
    Private Sub Reset_ToelencoUtenti()
        Me.Reset_HideAll()
        Me.PNLnavigazione.Visible = True
        Me.PNLnavigazione2.Visible = True
        Me.PNLcontenuto.Visible = True
        Me.TBLutenti.Visible = True
        Me.BTNavanti.Visible = True
        Me.BTNavanti2.Visible = True
        Me.BTNindietro.Visible = Not HideSourceSelector
        Me.BTNindietro2.Visible = Not HideSourceSelector
        'Me.oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & Me.FasiNuovaIscrizione.Fase2sceltaUtente)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBtitolo." & Me.FasiNuovaIscrizione.Fase2sceltaUtente)
    End Sub
    Private Sub Reset_ToRuoloUtenti()
        Me.Reset_HideAll()
        Me.PNLnavigazione.Visible = True
        Me.PNLnavigazione2.Visible = True
        Me.PNLcontenuto.Visible = True

        Me.TBLruolo.Visible = True
        Me.BTNindietro.Visible = True
        Me.BTNindietro2.Visible = True
        Me.BTNconferma.Visible = True
        Me.BTNconferma2.Visible = True
        'Me.oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & Me.FasiNuovaIscrizione.Fase3definizioneRuolo)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBtitolo." & Me.FasiNuovaIscrizione.Fase3definizioneRuolo)
    End Sub
#End Region

#Region "Fase 1"
    Private Sub RBLimporta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLimporta.SelectedIndexChanged
        If Me.RBLimporta.SelectedIndex = 0 Then
            Me.TBRcomunita.Visible = False
            Me.HDN_ComunitaPadreID.Value = 0
            Me.BTNavanti.Enabled = True
            Me.BTNavanti2.Enabled = True

            ' if me.DDLTipoPersona.
            'Me.DDLTipoRuolo.SelectedIndex = 0
        Else
            Me.BTNavanti.Enabled = False

            Me.TBRcomunita.Visible = True
            Me.HDN_ComunitaPadreID.Value = Me.CTRLsorgenteComunita.ComunitaID
            Try
                If Me.HDN_ComunitaPadreID.Value > 0 Then
                    Me.BTNavanti.Enabled = True
                End If
            Catch ex As Exception

            End Try
            Me.BTNavanti2.Enabled = Me.BTNavanti.Enabled
        End If
    End Sub
    Private Sub CTRLsorgenteComunita_AggiornaDati(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLsorgenteComunita.AggiornaDati
        Me.HDN_ComunitaPadreID.Value = Me.CTRLsorgenteComunita.ComunitaID
        Me.BTNavanti.Enabled = (Me.CTRLsorgenteComunita.ComunitaID > 0)
        Me.BTNavanti2.Enabled = (Me.CTRLsorgenteComunita.ComunitaID > 0)
    End Sub
#End Region

#Region "Fase 2"
    Public Sub FiltroLinkLettere_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
        If sender.commandArgument <> "" Then
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            Me.ViewState("intAnagrafica") = sender.commandArgument
            sender.CssClass = "lettera_Selezionata"
        Else
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End If
        Me.ViewState("intCurPage") = 0
        DGPersone.CurrentPageIndex = 0
        Me.Bind_Griglia(True)
    End Sub
    Private Sub DeselezionaLink(ByVal Lettera As String)
        Dim oFiltro As Main.FiltroAnagrafica

        Try
            Lettera = CType(CInt(Lettera), Main.FiltroAnagrafica).ToString

            Dim oLink As System.Web.UI.WebControls.LinkButton
            'oLink = Me.FindControl("LKB" & Lettera)
            oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)

            If IsNothing(oLink) = False Then
                oLink.CssClass = "lettera"
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub BTNCerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Bind_Griglia(True)
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        DGPersone.CurrentPageIndex = 0 'azzero indice paginazione
        ViewState("RipristinaCheck") = "si"
        Bind_Griglia()
    End Sub
    Private Sub DDLTipoRuolo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLTipoRuolo.SelectedIndexChanged
        DGPersone.CurrentPageIndex = 0
        ViewState("RipristinaCheck") = "si"
        Bind_Griglia(True)
    End Sub
    Private Sub DDLTipoPersona_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoPersona.SelectedIndexChanged
        DGPersone.CurrentPageIndex = 0
        ViewState("RipristinaCheck") = "si"
        Bind_Griglia(True)
    End Sub

    Private Sub DDLuserEnabled_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLuserEnabled.SelectedIndexChanged
        DGPersone.CurrentPageIndex = 0
        ViewState("RipristinaCheck") = "si"
        Bind_Griglia(True)
    End Sub

    Private Sub CBXaggiorna_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXaggiorna.CheckedChanged
        Me.DDLTipoRuolo.AutoPostBack = Me.CBXaggiorna.Checked
        Me.DDLTipoPersona.AutoPostBack = Me.CBXaggiorna.Checked
        Me.DDLuserEnabled.AutoPostBack = Me.CBXaggiorna.Checked
        Me.Bind_Griglia(True)
    End Sub

#Region "Griglia Persone"
    Sub DGPersone_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGPersone.PageIndexChanged
        DGPersone.CurrentPageIndex = e.NewPageIndex
        ViewState("RipristinaCheck") = "si"
        Me.Bind_Griglia()
    End Sub
    Private Sub DGPersone_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGPersone.ItemCreated
        Dim i As Integer
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If e.Item.ItemType = ListItemType.Header Then
            Dim oSortExspression, oSortDirection, oText, StringaMouse As String
            oSortExspression = ViewState("SortExspression")
            oSortDirection = ViewState("SortDirection")


            For i = 0 To sender.columns.count - 1
                If sender.columns(i).SortExpression <> "" Then
                    Dim oWebControl As WebControl
                    Dim oCell As New TableCell
                    Dim oLabelAfter As New System.Web.UI.WebControls.Label
                    Dim oLabelBefore As New System.Web.UI.WebControls.Label

                    oLabelBefore.Font.Name = "webdings"
                    oLabelBefore.Font.Size = FontUnit.XSmall
                    oLabelBefore.Text = "&nbsp;"

                    oCell = e.Item.Cells(i)
                    If Me.DGPersone.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGPersone, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGPersone, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGPersone.HeaderStyle.CssClass
                                oLabelAfter.Text = oLinkbutton.Text & " "
                                'oLinkbutton.Font.Name = "webdings"
                                'oLinkbutton.Font.Size = FontUnit.XSmall

                                If oSortDirection = "asc" Then
                                    '  oText = "5"
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
                                    '  oText = "6"
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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGPersone, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGPersone.HeaderStyle.CssClass
                                oLabelAfter.Text = oLinkbutton.Text & " "
                                'oLinkbutton.Font.Name = "webdings"
                                'oLinkbutton.Font.Size = FontUnit.XSmall
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

            n = oCell.ColumnSpan
            ' Aggiungo riga con descrizione:

            Try
                Dim oRow As TableRow
                Dim oTableCell As New TableCell
                oRow = oCell.Parent()

                oTableCell.Controls.Add(Me.CreaLegenda)
                oTableCell.ColumnSpan = 5
                If Me.DGPersone.Columns(2).Visible = False Then
                    oTableCell.ColumnSpan -= 1
                End If
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oCell.ColumnSpan = 2
                oRow.Cells.AddAt(0, oTableCell)
                e.Item.Cells(0).Attributes.Item("colspan") = oTableCell.ColumnSpan.ToString
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
                    oResource.setPageDatagrid(Me.DGPersone, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"

            Try
                If CBool(e.Item.DataItem("RLPC_Attivato")) = False Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                ElseIf CBool(e.Item.DataItem("RLPC_Abilitato")) = False Then
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
                End If
            End Try



            'bottone informazioni
            Dim oImagebutton As ImageButton
            Dim Cell As New TableCell
            Dim TPPR_id As Integer
            Dim PRSN_ID As Integer

            Try
                PRSN_ID = e.Item.DataItem("PRSN_id")
                TPPR_id = e.Item.DataItem("PRSN_TPPR_id")
                Dim i_link2 As String
                i_link2 = "./InfoIscritto.aspx?TPPR_ID=" & TPPR_id & "&PRSN_ID=" & PRSN_ID
                Cell = CType(e.Item.Cells(0), TableCell)

                oImagebutton = Cell.FindControl("IMBinfo")
                oResource.setImageButton_Datagrid(Me.DGPersone, oImagebutton, "IMBinfo", True, True)
                'in base al tipo di utente decido la dimensione della finestra di popup
                Select Case TPPR_id
                    Case Main.TipoPersonaStandard.Studente
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Docente
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','520','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Tutor
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Esterno
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Amministrativo
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.SysAdmin
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Copisteria
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Dottorando
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.DocenteSuperiori
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Main.TipoPersonaStandard.StudenteSuperiori
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','500','no','yes');return false;")
                    Case Else
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                End Select

                'oImagebutton.ToolTip = "Info Persona"
            Catch ex As Exception

            End Try


            Try
                'Gestione link MAIL !!!
                Dim oHYPMail As HyperLink
                oHYPMail = e.Item.Cells(6).FindControl("HYPMail")
                If Not IsNothing(oHYPMail) Then
                    oHYPMail.CssClass = cssLink

                    If Not IsDBNull(e.Item.DataItem("PRSN_mail")) Then
                        oHYPMail.Text = Replace(e.Item.DataItem("PRSN_mail"), "-", "&ndash;")
                    End If
                End If

            Catch ex As Exception

            End Try
            Try
                Dim oCheckbox As System.Web.UI.HtmlControls.HtmlInputCheckBox
                Dim ocell As TableCell
                ocell = e.Item.Cells(10)
                oCheckbox = e.Item.Cells(10).FindControl("CBabilitato")
                If Not IsNothing(oCheckbox) Then
                    Try
                        If CBool(e.Item.DataItem("RLPC_Attivato")) = False Then
                            oCheckbox.Visible = False
                        ElseIf CBool(e.Item.DataItem("RLPC_Abilitato")) = False Then
                            oCheckbox.Visible = False
                        Else
                            oCheckbox.Visible = True
                        End If

                        If InStr(Me.HDabilitato.Value, "," & e.Item.DataItem("PRSN_id") & ",") > 0 Then
                            oCheckbox.Checked = True
                        End If
                        oCheckbox.Value = e.Item.DataItem("PRSN_id")
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try


        End If
    End Sub
    Private Sub DGPersone_SortCommand(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGPersone.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = ViewState("SortExspression")
        oSortDirection = ViewState("SortDirection")
        ViewState("SortExspression") = e.SortExpression

        If e.SortExpression = oSortExpression Then

            If ViewState("SortDirection") = "asc" Then
                ViewState("SortDirection") = "desc"
            Else
                ViewState("SortDirection") = "asc"
            End If
        Else
            ViewState("SortDirection") = "asc"
        End If
        Me.Bind_Griglia()
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
        oTableCell.Text = oResource.getValue("NONattivati")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)


        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disabilitate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResource.getValue("NONabilitati")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function


#End Region
    Private Sub LNBapriFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapriFiltro.Click
        Me.TBRfiltri.Visible = True
        Me.TBRchiudiFiltro.Visible = True
        Me.TBRapriFiltro.Visible = False
        Me.Bind_Griglia()
    End Sub
    Private Sub LNBchiudiFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudiFiltro.Click
        Me.TBRfiltri.Visible = False
        Me.TBRchiudiFiltro.Visible = False
        Me.TBRapriFiltro.Visible = True
        Me.Bind_Griglia()
    End Sub
#End Region

#Region "Fase 3"
    Private Sub RBLsceltaRuoli_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLsceltaRuoli.SelectedIndexChanged
        Dim RuoloID As Integer = -1
        Try
            RuoloID = Me.DDLtipoRuoloAggiungi.SelectedValue
        Catch ex As Exception

        End Try
        Me.Bind_RuoloNuovoUtente(Me.RBLsceltaRuoli.SelectedValue, RuoloID)
    End Sub
#End Region

#Region "Navigazione"
    Private Sub BTNavanti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNavanti.Click, BTNavanti2.Click
        Dim alertMSG As String
        If Me.TBLsorgente.Visible Then
            Me.CTRLmessages.Visible = False
            Me.Reset_ToelencoUtenti()
            Me.Setup_ElencoUtenti()
        ElseIf Me.TBLutenti.Visible Then
            If Me.HDabilitato.Value = "" Or Me.HDabilitato.Value = "," Or Me.HDabilitato.Value = ",," Then
                Me.CTRLmessages.Visible = True
                Me.CTRLmessages.InitializeControl(oResource.getValue("alertNessunaSelezione"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                Me.Bind_Griglia()
            Else
                Session("azione") = "insert"

                Dim ElencoIDutenti() As String
                Dim totaleI As Integer

                ElencoIDutenti = Me.HDabilitato.Value.Split(",")
                totaleI = ElencoIDutenti.Length - 2
                If totaleI = 1 Then
                    Me.LBdescrizione.Text = Me.oResource.getValue("descrizione1")
                    If Me.LBdescrizione.Text <> "" Then
                        Dim oPersona As New COL_Persona
                        oPersona.ID = ElencoIDutenti(1)
                        oPersona.Estrai(Session("LinguaID"))
                        Me.LBdescrizione.Text = Replace(Me.LBdescrizione.Text, "#anagrafica#", oPersona.Cognome & " " & oPersona.Nome)
                    End If
                    Me.CBXresponsabile.Enabled = True
                Else
                    Me.LBdescrizione.Text = Me.oResource.getValue("descrizione2")
                    Me.CBXresponsabile.Enabled = False
                    Me.CBXresponsabile.Checked = False
                End If
                Me.Reset_ToRuoloUtenti()
            End If
        End If
    End Sub

    Private Sub BTNindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNindietro.Click, BTNindietro2.Click
        Session("azione") = "loaded"
        If Me.TBLutenti.Visible Then
            Me.CTRLmessages.Visible = False
            Reset_ToSorgenteComunita()
        ElseIf Me.TBLruolo.Visible Then
            Me.Reset_ToelencoUtenti()
            Me.Bind_Griglia()
            CTRLmessages.Visible = False
        End If
    End Sub

    Private Sub BTNtornaPaginaElenco_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNtornaPaginaElenco.Click, BTNtornaPaginaElencoAlto.Click, BTNtornaPaginaElencoBasso.Click
        If Request.QueryString("topage") <> "" Then
            Me.Utility.RedirectToUrl("Comunita/GestioneIscritti.aspx?topage=true")
        ElseIf Request.QueryString("toTree") <> "" Then
            Me.Utility.RedirectToUrl("Comunita/GestioneIscritti.aspx?toTree=true")
        Else
            Me.Utility.RedirectToUrl("Comunita/GestioneIscritti.aspx")
        End If
    End Sub

    Private Sub BTNgoToManagement_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNgoToManagement.Click, BTNgoToManagementAlto.Click, BTNgoToManagementBasso.Click
        Session("AdminForChange") = False
        Session("idComunita_forAdmin") = ""
        Session("CMNT_path_forAdmin") = ""
        If Request.QueryString("topage") <> "" Then
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true")
        ElseIf Request.QueryString("toTree") <> "" Then
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true&toTree=true")
        End If
    End Sub


#End Region

    Private Sub BTNconferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNconferma.Click, BTNconferma2.Click
        Dim oPersona As New COL_Persona

        If Session("azione") = "insert" Then
            Dim iResponse As Main.ErroriIscrizioneComunita = Main.ErroriIscrizioneComunita.ErroreGenerico
            Dim oResourceConfig As New ResourceManager
            Dim ElencoIDutenti(), ComunitaPath, LinguaCode As String
            Dim i, totaleI, ComunitaID, LinguaID, Corretti As Integer

            ElencoIDutenti = Me.HDabilitato.Value.Split(",")
            totaleI = ElencoIDutenti.Length - 2

            Try
                If Session("AdminForChange") = False Then
                    ComunitaID = Session("IdComunita")
                    Try
                        Dim ArrComunita(,) As String = Session("ArrComunita")
                        ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
                    Catch ex As Exception

                    End Try
                Else
                    ComunitaID = Session("idComunita_forAdmin")
                    ComunitaPath = Session("CMNT_path_forAdmin")
                End If
            Catch ex As Exception

            End Try

            Corretti = 0
            Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
            For i = 1 To totaleI
                oPersona.ID = ElencoIDutenti(i)
                LinguaID = oPersona.GetLinguaID(ElencoIDutenti(i))
                oResourceConfig = GetResourceConfig(LinguaCode)
                iResponse = oPersona.IscrizioneComunitaWithRole(ComunitaID, ComunitaPath, Me.DDLtipoRuoloAggiungi.SelectedValue, Me.CBXresponsabile.Checked, PageUtility.ProfilePath & oPersona.ID & "\", "./../", LinguaID)
                If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                    'oServiceUtility.NotifyAddSubscription(ComunitaID, oPersona.ID, Me.DDLtipoRuoloAggiungi.SelectedValue, oPersona.es, Me.DDLtipoRuoloAggiungi.SelectedItem.Text, Me.PageUtility.CurrentUser.Nome)
                    Corretti += 1
                    Me.HDabilitato.Value = Replace(Me.HDabilitato.Value, "," & ElencoIDutenti(i) & ",", ",")
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(CInt(ElencoIDutenti(i))))
                End If
            Next
            Dim errati As Integer = totaleI - Corretti
            Dim displayMessages As Boolean = False
            If Corretti = 0 Then
                displayMessages = True
                If totaleI = 1 Then
                    Me.CTRLmessages.InitializeControl(oResource.getValue("IscrittiConErrori.0.1"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                Else
                    Me.CTRLmessages.InitializeControl(String.Format(oResource.getValue("IscrittiConErrori.0.n"), totaleI), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                End If
            ElseIf errati = 0 AndAlso Corretti > 0 Then
                displayMessages = True
                If totaleI = 1 Then
                    Me.CTRLmessages.InitializeControl(oResource.getValue("Iscritti.1"), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
                Else
                    Me.CTRLmessages.InitializeControl(String.Format(oResource.getValue("Iscritti.n"), totaleI), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
                End If
            Else
                displayMessages = True
                Dim displayValue As String = "IscrittiConErrori."
                Select Case Corretti
                    Case 0
                        displayValue &= "0."
                    Case 1
                        displayValue &= "1."
                    Case Else
                        displayValue &= "n."
                End Select
                Select Case errati
                    Case 0
                        displayValue &= "0"
                    Case 1
                        displayValue &= "1"
                    Case Else
                        displayValue &= "n"
                End Select
                If errati > 1 AndAlso Corretti > 1 Then
                    Me.CTRLmessages.InitializeControl(String.Format(oResource.getValue(displayValue), Corretti, errati), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                ElseIf errati > 1 Then
                    Me.CTRLmessages.InitializeControl(String.Format(oResource.getValue(displayValue), errati), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                Else
                    Me.CTRLmessages.InitializeControl(oResource.getValue(displayValue), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                End If
            End If



            Me.CTRLmessages.Visible = displayMessages
            Me.Reset_ToelencoUtenti()
            Me.Bind_Griglia()
            Session("azione") = "loaded"
        End If
    End Sub


    'ADD for Master Page
    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property


    Private Function FindControlRecursive(ByVal Root As Control, ByVal Id As String) As Control
        If Root.ID = Id Then
            Return Root
        End If

        For Each Ctl As Control In Root.Controls
            Dim FoundCtl As Control = FindControlRecursive(Ctl, Id)
            If FoundCtl IsNot Nothing Then
                Return FoundCtl
            End If
        Next
        Return Nothing
    End Function

    Public Function GetDisabled(ByVal DataItem As Object)


        If Not Me.PNLuserEnabled.Visible Then
            Return ""
        End If


        Dim Out As String = ""

        Try
            Out = DataBinder.Eval(DataItem, "PRSN_invisible").ToString()
        Catch ex As Exception

        End Try

        If Out.ToLower() = "true" Then
            Out = Me.oResource.getValue("UserDisabled.mark")
            If (String.IsNullOrEmpty(Out)) Then
                Out = "[D] "
            End If
        Else
            Out = ""
        End If


        Return Out

    End Function
 
End Class