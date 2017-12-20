Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.UCServices



Public Class WizardImportaDati
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager


    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents TBRmenu As System.Web.UI.WebControls.TableRow
    Protected WithEvents BTNgoToManagement As System.Web.UI.WebControls.Button
    Protected WithEvents BTNtornaPaginaElenco As System.Web.UI.WebControls.Button
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

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

    Protected WithEvents TBLprincipale As System.Web.UI.WebControls.Table
    Protected WithEvents TBLsceltaDati As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLsceltaDati As Comunita_OnLine.UC_WZDid_Fase1SceltaDati
    Protected WithEvents TBLimpostazioni As System.Web.UI.WebControls.Table
    Protected WithEvents TBLsceltaUtenti As System.Web.UI.WebControls.Table
    Protected WithEvents TBLsceltaMateriale As System.Web.UI.WebControls.Table
    Protected WithEvents TBLsceltaDiario As System.Web.UI.WebControls.Table
    Protected WithEvents TBLfinale As System.Web.UI.WebControls.Table



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


        Dim oServizioAmministra As New Services_AmministraComunita
        Dim PermessiAssociati As String

        Try
            oPersona = Session("objPersona")
            If Not Page.IsPostBack Then
                'Session("azione") = "load"
                Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioAmministra.Codex)
                oServizioAmministra.PermessiAssociati = Me.ViewState("PermessiAssociati")
            Else
                If Me.ViewState("PermessiAssociati") = "" Then
                    Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioAmministra.Codex)
                End If
                oServizioAmministra.PermessiAssociati = Me.ViewState("PermessiAssociati")
            End If
        Catch ex As Exception
            oServizioAmministra.PermessiAssociati = "00000000000000000000000000000000"
        End Try
        If Not Page.IsPostBack Then
            Session("azione") = "loaded"
            Try
                If oServizioAmministra.Admin Or oServizioAmministra.Moderate Or oServizioAmministra.Change Then
                    Me.Bind_Dati()
                Else
                    Me.Reset_ToNoPermessi()
                End If
            Catch ex As Exception
                Me.Reset_ToNoComunita()
            End Try
        End If

        Me.Page.Form.DefaultButton = Me.BTNavanti.UniqueID
        Me.Page.Form.DefaultFocus = Me.BTNavanti.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNavanti.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.BTNavanti.UniqueID
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
        oResource.ResourcesName = "pg_WizardImportaDati"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo")
            .setLabel(Me.LBNopermessi)
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

            Me.BTNavanti.Attributes.Add("onclick", "return UserSelezionati('" & Replace(Me.oResource.getValue("messaggioSelezione"), "'", "\'") & "');")
            Me.BTNavanti2.Attributes.Add("onclick", "return UserSelezionati('" & Replace(Me.oResource.getValue("messaggioSelezione"), "'", "\'") & "');")
        End With
    End Sub
#End Region

#Region "Reset impostazioni"
    Private Sub Reset_HideAll()
        Me.PNLnavigazione2.Visible = False
        Me.PNLnavigazione.Visible = False
        Me.PNLpermessi.Visible = False
        Me.PNLcontenuto.Visible = False
        Me.TBLfinale.Visible = False
        Me.TBLimpostazioni.Visible = False
        Me.TBLsceltaDati.Visible = False
        Me.TBLsceltaDiario.Visible = False
        Me.TBLsceltaMateriale.Visible = False
        Me.TBLsceltaUtenti.Visible = False

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
#End Region

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
        Me.Setup_ComunitàOrigine()
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
            'If Me.HDN_ComunitaPadreID.Value = 0 Then
            '    Me.RBLimporta.SelectedIndex = 0
            '    Me.TBRcomunita.Visible = False
            '    Me.BTNavanti.Enabled = True
            '    Me.BTNavanti2.Enabled = True
            'ElseIf Me.HDN_ComunitaPadreID.Value = -1 Then
            '    ComunitaPathPadre = Replace(ComunitaPath, "." & ComunitaId & ".", ".")
            '    If ComunitaPathPadre = "" Or ComunitaPathPadre = "." Or ComunitaPathPadre = ".." Then
            '        ComunitaPadreId = 0
            '    Else
            '        Dim oArrayPadri() As String
            '        oArrayPadri = ComunitaPathPadre.Split(".")
            '        ComunitaPadreId = oArrayPadri(oArrayPadri.Length - 2)
            '    End If
            '    Me.CTRLsorgenteComunita.ShowFiltro = True
            '    Me.CTRLsorgenteComunita.ServizioCode = UCServices.Services_GestioneIscritti.Codex
            '    Me.CTRLsorgenteComunita.SetupControl(ComunitaPadreId, oComunitaAttuale.Livello, oComunitaAttuale.Id, ComunitaPath)
            '    Me.CTRLsorgenteComunita.Visible = True


            '    If Me.CTRLsorgenteComunita.ComunitaID > 0 Then
            '        Me.BTNavanti.Enabled = True
            '    Else
            '        Me.BTNavanti.Enabled = False
            '    End If
            '    Me.BTNavanti2.Enabled = Me.BTNavanti.Enabled
            'End If

            'Me.Reset_ToSorgenteComunita()
        Else
            Me.Reset_ToNoComunita()
        End If
    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class
