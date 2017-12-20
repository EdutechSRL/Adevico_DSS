Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports System.Linq
Imports Telerik.WebControls

Public Class GestioneIscritti
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

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

#End Region
    Private _PageUtility As PresentationLayer.OLDpageUtility

    Private ReadOnly Property PageUtility(Optional ByVal oContext As HttpContext = Nothing) As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) OrElse Not IsNothing(oContext) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    Private Enum StringaAbilitato
        abilitato = 1
        bloccato = 0
        inAttesa = -1
    End Enum

#Region "Pannello Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
#End Region

#Region "Filtri"
    Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table
    Protected WithEvents LBtipoRuolo_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLTipoRuolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBtipoRicerca_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBvalore_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBiscrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLiscrizione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents CBXautoUpdate As System.Web.UI.WebControls.CheckBox
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button
    Protected WithEvents LBnumeroRecord_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList

#Region "Filtro Lettere"
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

    Protected WithEvents HDN_filtroRuolo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoRicerca As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroValore As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroIscrizione As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "Menu"
    Protected WithEvents LBbuoto As System.Web.UI.WebControls.Label
    Protected WithEvents LNBiscrivi As System.Web.UI.WebControls.LinkButton

#End Region
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

    Protected WithEvents PNLmenuPrincipale As System.Web.UI.WebControls.Panel

    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LNBstampa As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBexcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBLexcel As System.Web.UI.WebControls.Table

    Protected WithEvents LBanagrafica_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBruolo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBresponsabile_t As System.Web.UI.WebControls.Label

    Protected WithEvents HDN_TPCM_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_totale As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNnonEliminabili As System.Web.UI.HtmlControls.HtmlInputHidden

#Region "Pannello Modifica"
    Protected WithEvents LBNomeCognome As System.Web.UI.WebControls.Label

    Protected WithEvents DDLruolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents PNLmenuModifica As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBsalva As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBannulla As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLmodifica As System.Web.UI.WebControls.Panel
    Protected WithEvents HDrlpc As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNprsnID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNrlpc_Attivato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNrlpc_Abilitato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents CHBresponsabile As System.Web.UI.WebControls.CheckBox
#End Region



#Region "Pannello DeIscrivi"
    Protected WithEvents PNLmenuDeIscrivi As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaDeiscrizione As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBdeIscriviCorrente As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBdeIscriviTutte As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBdeIscriviSelezionate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLdeiscrivi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBinfoDeIscrivi As System.Web.UI.WebControls.Label

    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcmnt_Path As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNprsn_Id As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents RDTcomunita As Telerik.WebControls.RadTreeView
#End Region

#Region "Pannello Deiscrivi multiplo"
    Protected WithEvents PNLmenuDeIscriviMultiplo As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannulla_multi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBdeIscriviTutte_multi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBdeIscriviCorrente_multi As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLdeiscriviMultiplo As System.Web.UI.WebControls.Panel
    Protected WithEvents HDNelencoID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LBinfoDeIscrivi_multiplo As System.Web.UI.WebControls.Label
#End Region

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents DGiscritti As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBnoIscritti As System.Web.UI.WebControls.Label
    Protected WithEvents LNBgotoGestioneComunita As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLiscritti As System.Web.UI.WebControls.Panel
    Protected WithEvents HDazione As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LNBcancellaInAttesa As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBabilita As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBdisabilita As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBelimina As System.Web.UI.WebControls.LinkButton

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
            SetCulture(Session("LinguaCode"))
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        Dim oServizioIscritti As New Services_GestioneIscritti
        Try

            If Not Page.IsPostBack Then
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
            Try
                Me.HDN_totale.Value = 0
                Me.SetupInternazionalizzazione()
                Me.Bind_Dati(oServizioIscritti)

                Session("azione") = "load"
            Catch ex As Exception
            End Try
        Else
            Me.DGiscritti.VirtualItemCount = Me.HDN_totale.Value
        End If

        Me.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Page.Form.DefaultFocus = Me.TXBValore.UniqueID 'Me.TXBvalue.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.TXBValore.UniqueID 'Me.TXBvalue.UniqueID

    End Sub

#Region "Gestione Sessione e Redirect"
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
        Me.Response.Redirect("./EntrataComunita.aspx", True)
    End Sub
#End Region

#Region "Bind_Dati"
    Private Sub Bind_Dati(ByVal oServizioIscritti As UCServices.Services_GestioneIscritti)

        Me.ViewState("intCurPage") = 0
        Me.ViewState("intAnagrafica") = CType(Main.FiltroComunita.tutti, Main.FiltroComunita)
        Me.LKBtutti.CssClass = "lettera_Selezionata"
        'Me.oResource.setLabel(LBtitolo)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.text")
        Me.Bind_TipoRuoloFiltro()

        Dim NuovoIndice As Integer = -15
        Dim isSimpleUser As Boolean = False
        isSimpleUser = Not (oServizioIscritti.Admin Or oServizioIscritti.Management Or oServizioIscritti.Change Or oServizioIscritti.Delete Or oServizioIscritti.AddUser)

        Try
            If oServizioIscritti.Admin Or oServizioIscritti.Management Or oServizioIscritti.Change Or oServizioIscritti.Delete Or oServizioIscritti.List Then
                If Session("azione") = "tutti" And Not isSimpleUser Then
                    NuovoIndice = Main.TipoAttivazione.Tutti
                ElseIf Session("azione") = "abilitati" Then
                    NuovoIndice = Main.TipoAttivazione.Attivati
                ElseIf Session("azione") = "bloccati" And Not isSimpleUser Then
                    NuovoIndice = Main.TipoAttivazione.Bloccati
                ElseIf Session("azione") = "inattesa" And Not isSimpleUser Then
                    NuovoIndice = Main.TipoAttivazione.InAttesa
                ElseIf Session("azione") = "ultimiIscritti" And Not isSimpleUser Then
                    NuovoIndice = Main.TipoAttivazione.NuoviIscritti
                Else 'in ogni altro caso
                    NuovoIndice = Main.TipoAttivazione.Attivati
                End If
            End If
        Catch ex As Exception
        End Try

        Me.Bind_Visualizzazione(oServizioIscritti, NuovoIndice)
        If Session("AdminForChange") = True Then
            Try

                Dim TitleStr As String = Me.oResource.getValue("LBtitolo.text")
                'ATTENZIONE!!!
                'Nel vecchio codice al posto di Titstr c'era LBtitle.text, solo che Me.Master.ServiceTitle è READONLY!!!
                Me.Master.ServiceTitle = TitleStr & ":" & COL_Comunita.EstraiNomeBylingua(Session("idComunita_forAdmin"), Session("LinguaID"))
                Me.Master.ServiceTitle = Replace(TitleStr, " -", " ") & " -"

            Catch ex As Exception

            End Try
        End If

        If oServizioIscritti.Admin Or oServizioIscritti.Management Or oServizioIscritti.Change Or oServizioIscritti.Delete Or oServizioIscritti.List Then
			Reset_ToListaUtenti()
			Me.Bind_TipoRuolo()
            Me.Bind_Griglia(True, True)
        Else
            Reset_ToNoPermessi()
        End If
    End Sub

    Private Sub Bind_Visualizzazione(Optional ByVal NuovoIndice As Integer = -15)
        Dim oServizioIscritti As New Services_GestioneIscritti
        Try

            If Not Page.IsPostBack Then
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
        Me.Bind_Visualizzazione(oServizioIscritti, NuovoIndice)
    End Sub
    Private Sub Bind_Visualizzazione(ByVal oServizioIscritti As UCServices.Services_GestioneIscritti, Optional ByVal NuovoIndice As Integer = -15)
        Dim HasNewUser, HasWaitingUsers, HasBlockedUsers, TabTutti As Boolean

        Try
            Dim IndiceCorrente As Integer = Main.TipoAttivazione.Attivati
            Dim oComunita As New COL_Comunita
            Dim ComunitaID As Integer

            Me.LNBstampa.Visible = False
            If Session("AdminForChange") = False Then
                ComunitaID = Session("IdComunita")
            Else
                ComunitaID = Session("idComunita_forAdmin")
            End If

            oComunita.Id = ComunitaID
            If Me.HDN_TPCM_ID.Value = "" Then
                Try
                    oComunita.Estrai()
                    Me.HDN_TPCM_ID.Value = oComunita.TipoComunita.ID
                Catch ex As Exception
                    Me.HDN_TPCM_ID.Value = -1
                End Try
            End If
            HasNewUser = oComunita.HasNewUsers(Session("objPersona").id, True)
            HasWaitingUsers = oComunita.HasWaitingUsers()
            HasBlockedUsers = oComunita.HasBlockedUsers()
            TabTutti = HasNewUser Or HasWaitingUsers Or HasBlockedUsers


            Try
                IndiceCorrente = Me.DDLiscrizione.SelectedIndex
            Catch ex As Exception

            End Try
            Me.DDLiscrizione.Items.Clear()

            Me.LBbuoto.Visible = False
            Me.LNBiscrivi.Visible = False
            If oServizioIscritti.Admin Or oServizioIscritti.Management Then
                Me.LBbuoto.Visible = oServizioIscritti.AddUser
                Me.LNBiscrivi.Visible = oServizioIscritti.AddUser
            ElseIf oServizioIscritti.Change Or oServizioIscritti.Delete Then
                Me.LNBstampa.Visible = oServizioIscritti.Print
            ElseIf oServizioIscritti.AddUser Then
                HasBlockedUsers = False
                Me.LBbuoto.Visible = True
                Me.LNBiscrivi.Visible = True
            ElseIf oServizioIscritti.List Then
                TabTutti = False
                HasNewUser = False
                HasWaitingUsers = False
                HasBlockedUsers = False
            End If

            Me.LNBstampa.Visible = oServizioIscritti.Print
            If (TabTutti) Then
                Me.DDLiscrizione.Items.Add(New ListItem("Tutti", CType(Main.TipoAttivazione.Tutti, Main.TipoAttivazione)))
            End If
            If (HasNewUser) Then
                Me.DDLiscrizione.Items.Add(New ListItem("Nuovi iscritti", CType(Main.TipoAttivazione.NuoviIscritti, Main.TipoAttivazione)))
            End If
            Me.DDLiscrizione.Items.Add(New ListItem("Abilitati", CType(Main.TipoAttivazione.Attivati, Main.TipoAttivazione)))
            If (HasWaitingUsers) Then
                Me.DDLiscrizione.Items.Add(New ListItem("In attesa di conferma", CType(Main.TipoAttivazione.InAttesa, Main.TipoAttivazione)))
            End If
            If (HasBlockedUsers) Then
                Me.DDLiscrizione.Items.Add(New ListItem("Bloccati", CType(Main.TipoAttivazione.Bloccati, Main.TipoAttivazione)))
            End If
            Try
                If NuovoIndice <> -15 Then
                    Me.DDLiscrizione.SelectedValue = NuovoIndice
                Else
                    Me.DDLiscrizione.SelectedValue = IndiceCorrente
                End If
            Catch ex As Exception
                Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.Attivati
            End Try

            Me.oResource.setDropDownList(Me.DDLiscrizione, CInt(Main.TipoAttivazione.Bloccati))
            Me.oResource.setDropDownList(Me.DDLiscrizione, CInt(Main.TipoAttivazione.Attivati))
            Me.oResource.setDropDownList(Me.DDLiscrizione, CInt(Main.TipoAttivazione.InAttesa))
            Me.oResource.setDropDownList(Me.DDLiscrizione, CInt(Main.TipoAttivazione.NuoviIscritti))
            Me.oResource.setDropDownList(Me.DDLiscrizione, CInt(Main.TipoAttivazione.Tutti))

            If Session("AdminForChange") = False Then
                Me.Master.ServiceTitle = Me.oResource.getValue("LBtitolo." & Me.DDLiscrizione.SelectedValue)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Bind_TipoRuolo()
        Me.DDLruolo.Items.Clear()
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oComunita As New COL_Comunita
            If Session("AdminForChange") = False Then
                Dim IdComunita As Integer = Session("IdComunita")
                oComunita.Id = IdComunita
            Else
                oComunita.Id = Session("idComunita_forAdmin")

            End If
            oDataset = oComunita.RuoliAssociabiliByPersona(Session("objPersona").id, Main.FiltroRuoli.ForTipoComunita_NoGuest)

            Totale = oDataset.Tables(0).Rows.Count()
            If Totale > 0 Then
                Totale = Totale - 1
                For i = 0 To Totale
					Dim oListItem As New ListItem
					If IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRL_nome")) Then
						oListItem.Text = "--"
					Else
						oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRL_nome")
					End If
					oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRL_ID")
					Me.DDLruolo.Items.Add(oListItem)
				Next
				Me.LNBsalva.Enabled = True
			Else
				Me.DDLruolo.Items.Add(New ListItem("< nessun ruolo >", -1))
				Me.LNBsalva.Enabled = False
			End If
		Catch ex As Exception
			Me.DDLruolo.Items.Add(New ListItem("< nessun ruolo >", -1))
			Me.LNBsalva.Enabled = False
		End Try
		oResource.setDropDownList(Me.DDLruolo, -1)
	End Sub
	Private Sub Bind_TipoRuoloFiltro()

		Me.DDLTipoRuolo.Items.Clear()
		Try
			Dim oDataset As DataSet
			Dim i, Totale As Integer
			Dim oComunita As New COL_Comunita
			If Session("AdminForChange") = False Then
				Dim IdComunita As Integer = Session("IdComunita")
				oComunita.Id = IdComunita
			Else
				oComunita.Id = Session("idComunita_forAdmin")
			End If
			oDataset = oComunita.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForUtenti_NoGuest)
			Totale = oDataset.Tables(0).Rows.Count()
			If Totale > 0 Then
				Totale = Totale - 1
				For i = 0 To Totale
					Dim oListItem As New ListItem
					If IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRL_nome")) Then
						oListItem.Text = "--"
					Else
						oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRL_nome")
					End If
					oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRL_ID")
					Me.DDLTipoRuolo.Items.Add(oListItem)
				Next
				If Totale >= 1 Then
					DDLTipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", -1))
				End If
				Me.LNBsalva.Enabled = True
			Else
				Me.DDLTipoRuolo.Items.Add(New ListItem("< nessun ruolo >", -1))
				Me.LNBsalva.Enabled = False
			End If
		Catch ex As Exception
			Me.DDLTipoRuolo.Items.Add(New ListItem("< nessun ruolo >", -1))
			Me.LNBsalva.Enabled = False
		End Try
		oResource.setDropDownList(Me.DDLTipoRuolo, -1)
	End Sub


    Private Function FiltraggioDati(Optional ByVal Rigenera As Boolean = False, Optional ByVal ApplicaFiltri As Boolean = False) As DataSet
        Dim oDataset As New DataSet
        Dim oFiltro As Main.FiltroAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
        Try
            Dim oPersona As New COL_Persona
            Dim Valore As String
            oPersona = Session("objPersona")

            Dim oComunita As New COL_Comunita
            If Session("AdminForChange") = False Then
                oComunita.Id = Session("IdComunita")
            Else
                oComunita.Id = Session("idComunita_forAdmin")
            End If

            If ApplicaFiltri Or Me.CBXautoUpdate.Checked Then
                Try
                    Me.HDN_filtroRuolo.Value = Me.DDLTipoRuolo.SelectedValue
                Catch ex As Exception

                End Try
                Me.HDN_filtroValore.Value = Me.TXBValore.Text
                Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
                Try
                    Me.HDN_filtroIscrizione.Value = Me.DDLiscrizione.SelectedValue
                Catch ex As Exception

                End Try
            End If
            Dim RuoloID, TipoRicercaID, IscrizioneID As Integer
            Try
                RuoloID = Me.HDN_filtroRuolo.Value
            Catch ex As Exception
                RuoloID = -1
            End Try
            Try
                TipoRicercaID = Me.HDN_filtroTipoRicerca.Value
            Catch ex As Exception
                TipoRicercaID = -1
            End Try
            Try
                IscrizioneID = Me.HDN_filtroIscrizione.Value
            Catch ex As Exception
                IscrizioneID = -1
            End Try
            Try
                Valore = Me.HDN_filtroValore.Value
            Catch ex As Exception
                Valore = ""
            End Try

            If Rigenera Then
                Me.DGiscritti.CurrentPageIndex = 0
                Session("azione") = "loaded"
                Me.HDazione.Value = ","
                ViewState("Paginazione") = ""
                ViewState("SortDirection") = "asc"
                ViewState("SortExspression") = "PRSN_Cognome"
                Me.ViewState("intCurPage") = 0
                Me.DGiscritti.CurrentPageIndex = 0
            End If

            Select Case IscrizioneID
                Case Main.TipoAttivazione.Tutti
                    Me.DGiscritti.Columns(17).Visible = True
                    Me.DGiscritti.Columns(11).Visible = True
                    oFiltro = Main.FiltroAbilitazione.Tutti

                Case Main.TipoAttivazione.NuoviIscritti
                    Session("azione") = "loaded"

                    If Rigenera Then
                        ViewState("SortExspression") = "rlpc_iscrittoil"
                        ViewState("SortDirection") = "desc"
                    End If
                    Me.DGiscritti.Columns(17).Visible = False
                    Me.DGiscritti.Columns(11).Visible = True
                    oFiltro = Main.FiltroAbilitazione.TuttiUltimiIscritti

                Case Main.TipoAttivazione.InAttesa
                    'Me.LNBcancellaInAttesa.Visible = True
                    Me.DGiscritti.Columns(17).Visible = False
                    Me.DGiscritti.Columns(11).Visible = True
                    oFiltro = Main.FiltroAbilitazione.NonAttivatoNonAbilitato


                Case Main.TipoAttivazione.Bloccati
                    Me.DGiscritti.Columns(17).Visible = True
                    Me.DGiscritti.Columns(11).Visible = True
                    oFiltro = Main.FiltroAbilitazione.NonAbilitatoAttivato

                Case Else
                    Me.DGiscritti.Columns(17).Visible = True
                    Me.DGiscritti.Columns(11).Visible = True
                    oFiltro = Main.FiltroAbilitazione.AttivatoAbilitato

            End Select

            Dim oFiltroLettera As Main.FiltroAnagrafica
            Dim oFiltroCampoOrdine As COL_Comunita.FiltroCampoOrdine
            Dim oFiltroOrdinamento As Main.FiltroOrdinamento
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
            End Try

            If Valore <> "" Then
                Valore = Trim(Valore)
            End If

            If Valore <> "" Then
                Select Case TipoRicercaID
                    Case Main.FiltroRicercaAnagrafica.nome
                        oFiltroRicerca = Main.FiltroRicercaAnagrafica.nome
                    Case Main.FiltroRicercaAnagrafica.cognome
                        oFiltroRicerca = Main.FiltroRicercaAnagrafica.cognome
                        oFiltroLettera = Main.FiltroAnagrafica.tutti
                        Me.LKBtutti.CssClass = "lettera_Selezionata"
                        Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                        Me.ViewState("intAnagrafica") = -1
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
                    Case Main.FiltroRicercaAnagrafica.nomeCognome
                        oFiltroRicerca = Main.FiltroRicercaAnagrafica.nomeCognome
                        oFiltroLettera = Main.FiltroAnagrafica.tutti
                        Me.LKBtutti.CssClass = "lettera_Selezionata"
                        Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                        Me.ViewState("intAnagrafica") = -1
                    Case Else
                        oFiltroRicerca = Main.FiltroRicercaAnagrafica.tutti
                End Select
            End If

            If ViewState("SortExspression") = "" Or LCase(ViewState("SortExspression")) = "prsn_anagrafica" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.anagrafica
            ElseIf LCase(ViewState("SortExspression")) = "prsn_datanascita" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.dataNascita
            ElseIf LCase(ViewState("SortExspression")) = "tprl_nome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.tipoRuolo
            ElseIf LCase(ViewState("SortExspression")) = "tppr_descrizione" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.tipoPersona
            ElseIf LCase(ViewState("SortExspression")) = "prsn_login" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.login
            ElseIf LCase(ViewState("SortExspression")) = "rlpc_iscrittoil" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.dataIscrizione
            ElseIf LCase(ViewState("SortExspression")) = "prsn_nome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.nome
            ElseIf LCase(ViewState("SortExspression")) = "prsn_cognome" Then
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.cognome
            Else
                oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.cognome
            End If

            If ViewState("SortDirection") = "" Or ViewState("SortDirection") = "asc" Then
                oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente
            Else
                oFiltroOrdinamento = Main.FiltroOrdinamento.Decrescente
            End If


            If IscrizioneID = Main.TipoAttivazione.NuoviIscritti Then
                oDataset = oComunita.ElencaIscrittiNoMittente(Session("LinguaID"), oPersona.Id, oFiltro, Main.FiltroUtenti.NoPassantiNoCreatori, RuoloID, Me.DGiscritti.PageSize, Me.ViewState("intCurPage"), Valore, oFiltroLettera, oFiltroOrdinamento, oFiltroCampoOrdine, , oFiltroRicerca)
            Else
                oDataset = oComunita.ElencaIscrittiNoMittente(Session("LinguaID"), 0, oFiltro, Main.FiltroUtenti.NoPassantiNoCreatori, RuoloID, Me.DGiscritti.PageSize, Me.ViewState("intCurPage"), Valore, oFiltroLettera, oFiltroOrdinamento, oFiltroCampoOrdine, , oFiltroRicerca)
            End If

            Dim i, totale As Integer
            Dim valori As String = ","
            Try
                If Me.HDNnonEliminabili.Value <> "" Then
                    valori = Me.HDNnonEliminabili.Value
                Else
                    valori = ","
                End If
            Catch ex As Exception
                valori = ","
            End Try
            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                oDataset.Tables(0).Columns.Add(New DataColumn("oCheck"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oIscrittoIl"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oUltimoCollegamento"))
                Dim PRSN_TPRL_Gerarchia, TPRL_Gerarchia As Integer
                Try
                    PRSN_TPRL_Gerarchia = Me.ViewState("PRSN_TPRL_Gerarchia")
                Catch ex As Exception
                    PRSN_TPRL_Gerarchia = "9999999"
                End Try

                oDataset.Tables(0).Columns.Add("oCheckDisabled")
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    If IsDBNull(oRow.Item("RLPC_IscrittoIl")) = False Then
                        If Equals(oRow.Item("RLPC_IscrittoIl"), New Date) Then
                            oRow.Item("oIscrittoIl") = "&nbsp;" & "--"
                        Else
                            oRow.Item("oIscrittoIl") = "&nbsp;" & CDate(oRow.Item("RLPC_IscrittoIl")).ToString("dd/MM/yy HH:mm:ss")
                        End If
                    Else
                        oRow.Item("oIscrittoIl") = "&nbsp;" & "--"
                    End If
                    If IsDBNull(oRow.Item("RLPC_ultimoCollegamento")) = False Then
                        If Equals(oRow.Item("RLPC_ultimoCollegamento"), New Date) Then
                            oRow.Item("oUltimoCollegamento") = "&nbsp;" & "--"
                        Else
                            oRow.Item("oUltimoCollegamento") = "&nbsp;" & CDate(oRow.Item("RLPC_ultimoCollegamento")).ToString("dd/MM/yy HH:mm:ss")
                        End If
                    Else
                        oRow.Item("oUltimoCollegamento") = "&nbsp;" & "--"
                    End If

                    If ViewState("Paginazione") <> "si" Then

                    Else

                        Dim ElencoID As Integer
                        Dim selezionato() As String
                        selezionato = Me.HDazione.Value.Split(",")
                        For ElencoID = 1 To selezionato.Length - 2
                            If oRow.Item("PRSN_ID") = selezionato(ElencoID) Then
                                oRow.Item("oCheck") = "checked"
                                Exit For
                            End If
                        Next
                    End If
                    TPRL_Gerarchia = oRow.Item("TPRL_Gerarchia")
                    If TPRL_Gerarchia < PRSN_TPRL_Gerarchia Then
                        oRow.Item("oCheckDisabled") = "disabled"
                    Else
                        oRow.Item("oCheckDisabled") = ""
                    End If
                    If oPersona.Id = oRow.Item("PRSN_ID") Then
                        oRow.Item("oCheckDisabled") = "disabled"
                    End If
                    If Not IsDBNull(oRow.Item("LKPO_Default")) And Me.HDN_TPCM_ID.Value = Main.TipoComunitaStandard.Organizzazione Then
                        If CBool(oRow.Item("LKPO_Default")) Then
                            If InStr(valori, "," & oRow.Item("PRSN_ID") & ",") < 1 Then
                                valori = valori & oRow.Item("PRSN_ID") & ","
                            End If
                        Else
                            If InStr(valori, "," & oRow.Item("PRSN_ID") & ",") > 0 Then
                                valori = valori & oRow.Item("PRSN_ID") & ","
                            End If
                        End If
                    End If
                    'responsabile [R]
                    Try
                        If CBool(oRow.Item("RLPC_Responsabile")) = True Then
                            oRow.Item("PRSN_Anagrafica") = oRow.Item("PRSN_Anagrafica") & "[R]"
                            oRow.Item("PRSN_Cognome") = oRow.Item("PRSN_Cognome") & "[R]"
                        End If
                    Catch ex As Exception

                    End Try
                Next
            End If
            If valori <> "," Then
                Me.HDNnonEliminabili.Value = valori
            Else
                Me.HDNnonEliminabili.Value = ""
            End If

            Return oDataset
        Catch ex As Exception
            Return oDataset
        End Try
    End Function
    Private Sub Bind_Griglia(Optional ByVal Rigenera As Boolean = False, Optional ByVal ApplicaFiltri As Boolean = False)
        Dim oPersona As New COL_Persona

        Try
            Dim oDataset As New DataSet
            oPersona = Session("objPersona")

            oDataset = FiltraggioDati(Rigenera, ApplicaFiltri)

            Dim i, totale As Integer
            totale = oDataset.Tables(0).Rows.Count

            If totale = 0 Then
                Me.DGiscritti.Visible = False
                LBnoIscritti.Visible = True
                Me.MostraLinkbutton(False)
                Me.DGiscritti.VirtualItemCount = 0
                Me.HDN_totale.Value = 0
                LBnoIscritti.Text = oResource.getValue("LBnoIscritti.1.text") '"Nessun utente in questa categoria"
            Else
                Me.HDN_totale.Value = oDataset.Tables(0).Rows(0).Item("Totale")
                Me.DGiscritti.VirtualItemCount = oDataset.Tables(0).Rows(0).Item("Totale")
                Me.MostraLinkbutton()


                If totale > 0 Then
                    Mod_Visualizzazione(totale - 1)
                    Me.DGiscritti.Visible = True

                    Dim oDataview As DataView
                    oDataview = oDataset.Tables(0).DefaultView
                    If viewstate("SortExspression") = "" Then
                        viewstate("SortExspression") = "PRSN_Cognome"
                        viewstate("SortDirection") = "asc"
                    End If
                    oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                    LBnoIscritti.Visible = False

                    'Ora nascondo le colonne in base ai permessi.......
                    Dim PermessiAssociati As String
                    Dim oServizioIscritti As New UCServices.Services_GestioneIscritti
                    Try
                        oServizioIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
                    Catch ex As Exception
                        oServizioIscritti.PermessiAssociati = Me.GetPermessiForPage(oServizioIscritti.Codex)
                    End Try

                    'colonna Cancella
                    Me.DGiscritti.Columns(1).Visible = (oServizioIscritti.Admin Or oServizioIscritti.Delete Or oServizioIscritti.Management Or oServizioIscritti.AddUser)
                    'colonna(Modifica)
                    Me.DGiscritti.Columns(2).Visible = (oServizioIscritti.Admin Or oServizioIscritti.Change Or oServizioIscritti.Management Or oServizioIscritti.AddUser)
                    'colonna(Informazioni)
                    Me.DGiscritti.Columns(3).Visible = (oServizioIscritti.Admin Or oServizioIscritti.Change Or oServizioIscritti.Management Or oServizioIscritti.InfoEstese)
                    'Colonna Checkbox
                    Me.DGiscritti.Columns(19).Visible = (Me.DGiscritti.Columns(1).Visible Or Me.DGiscritti.Columns(2).Visible)
                    Me.DGiscritti.DataSource = oDataview
                    Me.DGiscritti.DataBind()
                Else
                    Me.DGiscritti.Visible = False
                    LBnoIscritti.Visible = True
                    Me.MostraLinkbutton(False)
                    LBnoIscritti.Text = oResource.getValue("LBnoIscritti.0.text") '"Spiacente, al momento non ci utenti presenti in base ai parametri di ricerca specificati."
                End If
            End If
        Catch ex As Exception
            Me.DGiscritti.Visible = False
            LBnoIscritti.Visible = True
            Me.MostraLinkbutton(False)
            LBnoIscritti.Text = oResource.getValue("LBnoIscritti.1.text") ' "Nessun utente in questa categoria"
        End Try
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
            oComunita.Id = Session("idComunita_forAdmin")

            'Vengo dalla pagina di amministrazione generale
            Try
                PermessiAssociati = oComunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Session("idComunita_forAdmin"), Codex)
                If (PermessiAssociati = "") Then
                    PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                PermessiAssociati = "00000000000000000000000000000000"

            End Try
        End If
        Return PermessiAssociati
    End Function

    Private Sub MostraLinkbutton(Optional ByVal hasRecord As Boolean = True)
        Dim Abilitazione As Boolean = False
        Dim Cancellazione As Boolean = False

        Dim PermessiAssociati As String
        Dim oServizioIscritti As New UCServices.Services_GestioneIscritti
        Try
            oServizioIscritti.PermessiAssociati = Me.ViewState("PermessiAssociati")
        Catch ex As Exception
            oServizioIscritti.PermessiAssociati = Me.GetPermessiForPage(oServizioIscritti.Codex)
        End Try

        Abilitazione = (oServizioIscritti.Admin Or oServizioIscritti.Change Or oServizioIscritti.Management Or oServizioIscritti.AddUser)
        Cancellazione = (oServizioIscritti.Admin Or oServizioIscritti.Delete Or oServizioIscritti.Management Or oServizioIscritti.AddUser)

        If hasRecord = True And (Abilitazione Or Cancellazione) Then
            If Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.Tutti Then
                Me.LNBabilita.Visible = Abilitazione
                Me.LNBdisabilita.Visible = Abilitazione
                Me.LNBelimina.Visible = Cancellazione 'per adesso
                Me.LNBcancellaInAttesa.Visible = False
            ElseIf Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.NuoviIscritti Then
                Me.LNBabilita.Visible = Abilitazione
                Me.LNBdisabilita.Visible = Abilitazione
                Me.LNBelimina.Visible = Cancellazione 'per adesso
                Me.LNBcancellaInAttesa.Visible = False
            ElseIf Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.Bloccati Then
                Me.LNBabilita.Visible = Abilitazione
                Me.LNBdisabilita.Visible = False
                Me.LNBelimina.Visible = Cancellazione 'per adesso
                Me.LNBcancellaInAttesa.Visible = False
            ElseIf Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.InAttesa Then
                Me.LNBabilita.Visible = Abilitazione
                Me.LNBdisabilita.Visible = False
                Me.LNBelimina.Visible = Cancellazione 'per adesso
                Me.LNBcancellaInAttesa.Visible = True
            ElseIf Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.Attivati Then
                Me.LNBabilita.Visible = False
                Me.LNBdisabilita.Visible = Abilitazione
                Me.LNBelimina.Visible = Cancellazione 'per adesso
                Me.LNBcancellaInAttesa.Visible = False
            End If
        Else
            Me.LNBabilita.Visible = False
            Me.LNBdisabilita.Visible = False
            Me.LNBelimina.Visible = False 'per adesso
            Me.LNBcancellaInAttesa.Visible = False
        End If
        Me.LNBstampa.Visible = (oServizioIscritti.Admin Or oServizioIscritti.Print Or oServizioIscritti.Management)
        Me.LNBexcel.Visible = (oServizioIscritti.Admin Or oServizioIscritti.Print Or oServizioIscritti.Management)
    End Sub
#End Region

#Region "Filtri"
    Private Sub CBXautoUpdate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXautoUpdate.CheckedChanged
        Me.DDLTipoRuolo.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.Bind_Griglia(False, True)
    End Sub
    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Me.ViewState("intCurPage") = 0
        Me.DGiscritti.CurrentPageIndex = 0
        Me.Bind_Griglia(True, True)
    End Sub
    'Public Sub FiltroLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
    '    Me.Bind_Griglia(True, Me.CBXautoUpdate.Checked)
    'End Sub
    Private Sub DeselezionaLink(ByVal Lettera As String)
        Dim oFiltro As Main.FiltroAnagrafica
        Lettera = CType(CInt(Lettera), Main.FiltroAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub
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
        Me.DGiscritti.CurrentPageIndex = 0
        Me.Bind_Griglia(True, Me.CBXautoUpdate.Checked)
    End Sub
    Private Sub DDLTipoRuolo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoRuolo.SelectedIndexChanged
        Me.ViewState("intCurPage") = 0
        Me.DGiscritti.CurrentPageIndex = 0
        Me.Bind_Griglia(True, Me.CBXautoUpdate.Checked)
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.DGiscritti.PageSize = Me.DDLNumeroRecord.Items(DDLNumeroRecord.SelectedIndex).Value
        Me.ViewState("intCurPage") = 0
        Me.DGiscritti.CurrentPageIndex = 0
        viewstate("Paginazione") = "si"
        Me.Bind_Griglia(True, Me.CBXautoUpdate.Checked)
    End Sub

    Private Sub DDLiscrizione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLiscrizione.SelectedIndexChanged
        Me.DGiscritti.PageSize = Me.DDLNumeroRecord.Items(DDLNumeroRecord.SelectedIndex).Value
        Me.ViewState("intCurPage") = 0
        Me.DGiscritti.CurrentPageIndex = 0
        viewstate("Paginazione") = ""
        Me.Bind_Visualizzazione(Me.DDLiscrizione.SelectedValue)
        Me.Bind_Griglia(True, Me.CBXautoUpdate.Checked)
    End Sub
#End Region


#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "NEW_pg_GestioneIscritti"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(Me.LBNopermessi)


            .setLabel(LBtipoRuolo_t)
            .setLabel(LBnumeroRecord_t)
            .setLabel(LBtipoRicerca_t)
            .setLabel(LBvalore_t)
            .setDropDownList(DDLTipoRicerca, -2)
            .setDropDownList(DDLTipoRicerca, -3)
            .setDropDownList(DDLTipoRicerca, -4)
            .setDropDownList(DDLTipoRicerca, -7)
            .setLabel(Me.LBiscrizione_t)
            .setButton(Me.BTNCerca)

            oResource.setHeaderDatagrid(Me.DGiscritti, 4, "cognome", True)
            oResource.setHeaderDatagrid(Me.DGiscritti, 5, "nome", True)
            oResource.setHeaderDatagrid(Me.DGiscritti, 6, "anagrafica", True)
            oResource.setHeaderDatagrid(Me.DGiscritti, 7, "mail", True)
            oResource.setHeaderDatagrid(Me.DGiscritti, 9, "ruolo", True)
            oResource.setHeaderDatagrid(Me.DGiscritti, 11, "iscrittoIl", True)
            oResource.setHeaderDatagrid(Me.DGiscritti, 17, "ultimoCollegamento", True)

            .setLabel(LBinfoDeIscrivi_multiplo)
            .setLinkButton(Me.LNBabilita, True, False)
            .setLinkButton(Me.LNBdisabilita, True, False)
            .setLinkButton(Me.LNBelimina, True, False)
            .setLinkButton(Me.LNBcancellaInAttesa, True, False)
            .setLinkButton(Me.LNBstampa, True, True)
            .setLinkButton(Me.LNBexcel, True, True)

            .setLabel(LBanagrafica_t)
            .setLabel(LBruolo_t)
            .setLabel(LBresponsabile_t)
            .setCheckBox(CHBresponsabile)
            .setCheckBox(Me.CBXautoUpdate)

            .setLabel(LBinfoDeIscrivi)
            .setLinkButton(LKBaltro, True, False)
            .setLinkButton(LKBtutti, True, False)
            .setLinkButton(Me.LNBannulla, True, True)
            .setLinkButton(Me.LNBsalva, True, True)
            .setLinkButton(Me.LNBannulla_multi, True, True)
            .setLinkButton(Me.LNBdeIscriviCorrente_multi, True, True)
            .setLinkButton(Me.LNBdeIscriviTutte_multi, True, True)
            .setLinkButton(Me.LNBannullaDeiscrizione, True, True)
            .setLinkButton(Me.LNBdeIscriviCorrente, True, True)
            .setLinkButton(Me.LNBdeIscriviTutte, True, True)
            .setLinkButton(Me.LKBaltro, True, True)
            .setLinkButton(Me.LKBtutti, True, True)
            .setLinkButton(Me.LNBdeIscriviSelezionate, True, True)
            '.setLinkButton(Me.LKBChangeOwner, True, False)
            .setLinkButton(Me.LNBiscrivi, True, True)
            .setLinkButton(Me.LNBgotoGestioneComunita, True, True)
            If Session("AdminForChange") = True Then
                Me.LNBgotoGestioneComunita.Visible = True
            Else
                Me.LNBgotoGestioneComunita.Visible = False
            End If

            .setLinkButton(Me.LNBapriFiltro, True, True)
            .setLinkButton(Me.LNBchiudiFiltro, True, True)
            Dim i As Integer
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControlRecursive(Me.Master, "LKB" & Chr(i))
                Dim Carattere As String = Chr(i)

                If IsNothing(oLinkButton) = False Then
                    oResource.setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
                End If
            Next

            Me.LNBcancellaInAttesa.Attributes.Add("onclick", "window.status='';return UserForCancella('" & Replace(Me.oResource.getValue("messaggioConferma"), "'", "\'") & "','" & Replace(Me.oResource.getValue("messaggioSelezione"), "'", "\'") & "');")
            Me.LNBabilita.Attributes.Add("onclick", "window.status='';return UserSelezionati('" & Replace(Me.oResource.getValue("messaggioSelezione"), "'", "\'") & "');")
            Me.LNBdisabilita.Attributes.Add("onclick", "window.status='';return UserSelezionati('" & Replace(Me.oResource.getValue("messaggioSelezione"), "'", "\'") & "');")
            Me.LNBelimina.Attributes.Add("onclick", "window.status='';return UserForCancella('" & Replace(Me.oResource.getValue("messaggioConferma"), "'", "\'") & "','" & Replace(Me.oResource.getValue("messaggioSelezione"), "'", "\'") & "');")
            Me.LNBstampa.Attributes.Add("onClick", "Stampa();window.status='';return false;")
            'Me.LNBstampa.OnClientClick = "Stampa();window.status='';return false;"
        End With
    End Sub
#End Region

#Region "Reset Form"
    Private Sub Reset_HideAllForm()
        Me.PNLiscritti.Visible = False
        Me.PNLdeiscrivi.Visible = False
        Me.PNLdeiscriviMultiplo.Visible = False
        Me.PNLmenu.Visible = False
        Me.PNLmenuDeIscrivi.Visible = False
        Me.PNLmenuDeIscriviMultiplo.Visible = False
        Me.PNLmenuModifica.Visible = False
        Me.PNLmenuPrincipale.Visible = False
        Me.PNLmodifica.Visible = False
        Me.PNLpermessi.Visible = False
    End Sub
    Private Sub Reset_ToNoPermessi()
        Me.Reset_HideAllForm()
        Me.PNLcontenuto.Visible = False
        Me.PNLpermessi.Visible = True
    End Sub
    Private Sub Reset_ToListaUtenti()
        Me.Reset_HideAllForm()
        Me.PNLcontenuto.Visible = True
        Me.PNLpermessi.Visible = False
        Me.PNLmenuPrincipale.Visible = True
        Me.PNLmenu.Visible = True
        Me.PNLiscritti.Visible = True
    End Sub
    Private Sub Reset_ToModifica()
        Me.Reset_HideAllForm()
        Me.PNLmenuModifica.Visible = True
        Me.PNLmodifica.Visible = True
        'Me.oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo.modifica")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBtitolo.modifica")
    End Sub
    Private Sub Reset_ToDeIscriviSingolo()
        Me.Reset_HideAllForm()
        Me.PNLdeiscrivi.Visible = True
        Me.PNLmenuDeIscrivi.Visible = True
        'Me.oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo.elimina")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBtitolo.elimina")
    End Sub
    Private Sub Reset_ToDeIscriviMultiplo()
        Me.Reset_HideAllForm()
        Me.PNLdeiscriviMultiplo.Visible = True
        Me.PNLmenuDeIscriviMultiplo.Visible = True
        'Me.oResource.setLabel_To_Value(Me.LBtitolo, "LBtitolo.elimina")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBtitolo.elimina")
    End Sub

#End Region

#Region "Gestione Griglia"
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGiscritti.SortCommand
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
        Else
            viewstate("SortDirection") = "asc"
        End If
        Me.Bind_Griglia()
    End Sub
    Protected Sub DGiscritti_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGiscritti.PageIndexChanged
        Me.ViewState("intCurPage") = e.NewPageIndex
        Me.DGiscritti.CurrentPageIndex = e.NewPageIndex
        viewstate("Paginazione") = "si"
        Me.Bind_Griglia()
    End Sub
    Private Sub DGgriglia_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGiscritti.ItemCreated
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
                    If Me.DGiscritti.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGiscritti, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGiscritti, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGiscritti.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGiscritti, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGiscritti.HeaderStyle.CssClass
                                oLabelAfter.text = oLinkbutton.Text & " "
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


            ' If Me.TBSmenu.SelectedIndex <= 1 Then
            Try
                Dim oRow As TableRow
                Dim oTableCell As New TableCell
                Dim num As Integer = 0
                oRow = oCell.Parent()

                oTableCell.Controls.Add(Me.CreaLegenda)
                If Me.DGiscritti.Columns(19).Visible Then
                    num += 1
                End If
                If Me.DGiscritti.Columns(1).Visible Then
                    num += 1
                End If
                If Me.DGiscritti.Columns(2).Visible Then
                    num += 1
                End If
                If Me.DGiscritti.Columns(3).Visible Then
                    num += 1
                End If
                If Me.DGiscritti.Columns(11).Visible Then
                    num += 1
                End If
                If Me.DGiscritti.Columns(17).Visible Then
                    num += 1
                End If
                num += 2
                oTableCell.ColumnSpan = num
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oCell.ColumnSpan = 2
                oRow.Cells.AddAt(0, oTableCell)
                e.Item.Cells(0).Attributes.Item("colspan") = num.ToString
            Catch ex As Exception

            End Try
            ' End If


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
                    oResource.setPageDatagrid(Me.DGiscritti, oLinkbutton)
                End Try
            Next
        End If


    End Sub
    Private Sub DGiscritti_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGiscritti.ItemDataBound
        Dim i As Integer
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
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

            Dim PRSN_TPRL_Gerarchia As Integer
            Dim oPermessoSuRuolo As Boolean = False
            Dim oPermessoSuRuoloElimina As Boolean = False
            Dim oPersona As New COL_Persona
            oPersona = Session("objPersona")

            Try
                PRSN_TPRL_Gerarchia = Me.ViewState("PRSN_TPRL_Gerarchia")
                If PRSN_TPRL_Gerarchia <= e.Item.DataItem("TPRL_Gerarchia") Then
                    oPermessoSuRuolo = True
                    oPermessoSuRuoloElimina = True
                End If
                If oPermessoSuRuolo And Me.HDN_TPCM_ID.Value = Main.TipoComunitaStandard.Organizzazione Then
                    If e.Item.DataItem("LKPO_Default") = True Then
                        oPermessoSuRuoloElimina = False
                    End If
                End If
            Catch ex As Exception

            End Try

            ' Cancellazione iscrizione utente.........
            Dim oImagebutton As ImageButton
            Try
                oImagebutton = e.Item.Cells(0).FindControl("IMBCancella")
                If Not IsNothing(oImagebutton) Then
                    'oImagebutton.ToolTip = "Cancella iscrizione"
                    If oPersona.Id <> e.Item.DataItem("PRSN_ID") Then
                        oImagebutton.Enabled = oPermessoSuRuoloElimina
                        oImagebutton.Visible = oPermessoSuRuoloElimina
                    Else
                        oImagebutton.Enabled = False
                        oImagebutton.Visible = False
                    End If
                    oResource.setImageButton_Datagrid(Me.DGiscritti, oImagebutton, "IMBCancella", oImagebutton.Enabled, True, True, True)
                End If
            Catch ex As Exception

            End Try

            'Modifica ruolo
            Try
                oImagebutton = e.Item.Cells(0).FindControl("IMBmodifica")
                If Not IsNothing(oImagebutton) Then
                    oResource.setImageButton_Datagrid(Me.DGiscritti, oImagebutton, "IMBmodifica", True, True, True)

                    'oImagebutton.ToolTip = "Modifica"
                    oImagebutton.Enabled = oPermessoSuRuolo
                    oImagebutton.Visible = oPermessoSuRuolo
                End If
            Catch ex As Exception

            End Try

            Try
                'Gestione link MAIL !!!
                Dim oHYPMail As HyperLink
                oHYPMail = e.Item.Cells(8).FindControl("HYPMail")
                If Not IsNothing(oHYPMail) Then
                    oHYPMail.CssClass = cssLink
                End If

            Catch ex As Exception

            End Try


            'bottone informazioni
            oImagebutton = Nothing
            Dim Cell As New TableCell
            Dim TPPR_id As Integer
            Dim PRSN_ID As Integer

            Try
                PRSN_ID = e.Item.DataItem("PRSN_id")
                TPPR_id = e.Item.DataItem("PRSN_TPPR_id")
                Dim i_link2 As String


                Cell = CType(e.Item.Cells(0), TableCell)

                oImagebutton = Cell.FindControl("IMBinfo")
                oResource.setImageButton_Datagrid(Me.DGiscritti, oImagebutton, "IMBinfo", True, True)

              
                    i_link2 = "./InfoIscritto.aspx?TPPR_ID=" & TPPR_id & "&PRSN_ID=" & PRSN_ID

                    'in base al tipo di utente decido la dimensione della finestra di popup
                    Select Case TPPR_id
                    Case Main.TipoPersonaStandard.Esterno
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                        Case Main.TipoPersonaStandard.Amministrativo
                            oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                        Case Main.TipoPersonaStandard.SysAdmin
                            oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Else
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                End Select

                'oImagebutton.ToolTip = "Info Persona"
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub DGiscritti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGiscritti.ItemCommand
        Dim RuoloID, RuoloPersonaComunitaID, IDGestore, UtenteID, ComunitaId As Integer
        Dim isAbilitato, isAttivato, isResponsebile As Boolean
        Dim ComunitaPath As String = ""
        Dim oPersona As New COL_Persona

        Try
            RuoloPersonaComunitaID = source.Items(e.Item.ItemIndex).Cells(0).Text()
        Catch ex As Exception
            RuoloPersonaComunitaID = -1
        End Try
        Try
            oPersona = Session("objPersona")
            IDGestore = oPersona.Id
        Catch ex As Exception

        End Try

        Try
            UtenteID = source.Items(e.Item.ItemIndex).Cells(13).Text()
        Catch ex As Exception

        End Try
        Try
            If Session("AdminForChange") = False Then
                ComunitaId = Session("IdComunita")
                Try
                    Dim ArrComunita(,) As String = Session("ArrComunita")
                    ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
                Catch exc As Exception
                    ComunitaPath = "."
                End Try
            Else
                ComunitaId = Session("idComunita_forAdmin")
                ComunitaPath = Session("CMNT_path_forAdmin")
            End If
        Catch ex As Exception
            ComunitaId = 0
        End Try


        Try
            RuoloID = source.Items(e.Item.ItemIndex).Cells(8).text()
        Catch ex As Exception

        End Try

        Try
            isAttivato = source.Items(e.Item.ItemIndex).Cells(14).Text()
            isAbilitato = source.Items(e.Item.ItemIndex).Cells(15).Text()
            isResponsebile = CBool(source.Items(e.Item.ItemIndex).Cells(16).Text())
        Catch ex As Exception

        End Try



        If e.CommandName = "modifica" Then
            Session("azione") = "modifica"

            Me.PNLmodifica.Visible = True
            Me.HDrlpc.Value = RuoloPersonaComunitaID
            Me.HDNprsnID.Value = UtenteID
            Me.HDNrlpc_Attivato.Value = isAttivato
            Me.HDNrlpc_Abilitato.Value = isAbilitato
            Me.HDNcmnt_Path.Value = ComunitaPath
            Me.HDNcmnt_ID.Value = ComunitaId
            Me.CHBresponsabile.Checked = isResponsebile
            Me.CHBresponsabile.Enabled = Not isResponsebile

            Me.LBNomeCognome.Text = source.Items(e.Item.ItemIndex).Cells(6).Text()
            Try
                If RuoloID > -1 Then
                    Me.DDLruolo.SelectedValue = RuoloID
                End If

            Catch ex As Exception

            End Try
            Me.DDLruolo.Enabled = Not (UtenteID = IDGestore)
            Reset_ToModifica()
        ElseIf e.CommandName = "deiscrivi" Then
            Session("azione") = "deiscrivi"
            Try
                If Me.LNBcancellaInAttesa.Visible = False Then
                    Me.HDNprsn_Id.Value = UtenteID
                    Me.HDNcmnt_Path.Value = ComunitaPath
                    Me.HDNcmnt_ID.Value = ComunitaId
                    Me.DeIscrivi(ComunitaId, ComunitaPath, UtenteID)
                Else
                    Dim oComunita As New COL_Comunita
                    oComunita.Id = ComunitaId
                    oComunita.EliminaUtentiInAttesa("," & UtenteID & ",")
                    If oComunita.Errore = Errori_Db.None Then
                        Try
                            Me.AggiornaProfiloXML(ComunitaId, UtenteID, ComunitaPath)
                        Catch ex As Exception

                        End Try
                    End If
                    Me.Bind_Visualizzazione(Me.DDLiscrizione.SelectedValue)
                    Me.Bind_Griglia(True)
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub Mod_Visualizzazione(ByVal oRecord As Integer)
        If oRecord > Me.DGiscritti.PageSize Or oRecord > Me.DDLNumeroRecord.Items(0).Value Or Me.DGiscritti.VirtualItemCount > Me.DGiscritti.PageSize Then
            Me.DGiscritti.AllowPaging = True
            Me.DGiscritti.PageSize = Me.DDLNumeroRecord.SelectedItem.Value
        Else
            Me.DGiscritti.AllowPaging = False
        End If
        If oRecord < 0 Then
            Me.DGiscritti.Visible = False
            LBnoIscritti.Visible = True
            MostraLinkbutton(False)
            LBnoIscritti.Text = oResource.getValue("LBnoIscritti.0.text") '"Nessun utente in questa categoria"
        End If
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


#Region "Gestione iscritti"
    Private Sub DisabilitaIscritti(ByVal ElencoIscritti As String, ByVal CMNT_ID As Integer)
        Dim oComunita As New COL_Comunita
        Dim PRSN_ID, TPRL_ID, i, totale As Integer
        Dim ElencoID() As String

        ElencoID = ElencoIscritti.Split(",")

        'salvo le modifiche su file xml
        For i = 0 To UBound(ElencoID)
            Try
                If IsNumeric(ElencoID(i)) Then
                    PRSN_ID = ElencoID(i)
                    Me.DisattivaIscritto(PRSN_ID, CMNT_ID)
                End If
            Catch ex As Exception

            End Try
        Next
        'salvo le modifiche su db
        ' ElencoIscritti = ElencoIscritti.Replace(",", " ")
        oComunita.Id = CMNT_ID
        oComunita.DisabilitaIscritti(ElencoIscritti)
    End Sub
    Private Sub AbilitaIscritti(ByVal ElencoIscritti As String, ByVal CMNT_ID As Integer)
        Dim oComunita As New COL_Comunita
        Dim PRSN_ID, TPRL_ID, i, totale As Integer
        Dim ElencoID() As String

        ElencoID = ElencoIscritti.Split(",")

        'salvo le modifiche su file xml
        For i = 0 To UBound(ElencoID)
            Try
                If IsNumeric(ElencoID(i)) Then
                    PRSN_ID = ElencoID(i)
                    Me.AbilitaIscritto(PRSN_ID, CMNT_ID)
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(PRSN_ID))
                End If
            Catch ex As Exception

            End Try
        Next
        'salvo le modifiche su db
        '  ElencoIscritti = ElencoIscritti.Replace(",", " ")
        oComunita.Id = CMNT_ID
        oComunita.AbilitaIscritti(ElencoIscritti)
    End Sub
    Private Sub AttivaIscritti(ByVal ElencoIscritti As String, ByVal CMNT_ID As Integer)
        Dim oComunita As New COL_Comunita
        Dim PRSN_ID, TPRL_ID, i, totale As Integer
        Dim ElencoID() As String

        ElencoID = ElencoIscritti.Split(",")

        'salvo le modifiche su file xml
        Try
            oComunita.Id = CMNT_ID
            oComunita.Estrai()

            For i = 0 To UBound(ElencoID)
                Try
                    If IsNumeric(ElencoID(i)) Then
                        Dim oPersona As New COL_Persona
                        PRSN_ID = ElencoID(i)
                        oPersona.Id = PRSN_ID
                        oPersona.Estrai(Session("LinguaID"))
                        If oPersona.Errore = Errori_Db.None Then
                            Me.AttivaIscritto(PRSN_ID, CMNT_ID, oPersona.Lingua.Codice)
                        Else
                            Me.AttivaIscritto(PRSN_ID, CMNT_ID, Session("LinguaCode"))
                        End If
                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(PRSN_ID))
						Dim oUtility As New OLDpageUtility(Me.Context)
						oComunita.MailAccettazione(oPersona, oUtility.LocalizedMail)
                    End If
                Catch ex As Exception

                End Try
            Next

            'salvo le modifiche su db
            ' ElencoIscritti = ElencoIscritti.Replace(",", " ")

            oComunita.AttivaIscritti(ElencoIscritti)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub DisattivaIscritto(ByVal PRSN_ID As Integer, ByVal CMNT_ID As Integer)
        Dim oTreeComunita As New COL_TreeComunita
        oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
        oTreeComunita.Nome = PRSN_ID & ".xml"
        If Session("AdminForChange") = False Then
            oTreeComunita.CambiaAbilitazione(Session("idComunita"), False)
        Else
            oTreeComunita.CambiaAbilitazione(Session("idComunita_forAdmin"), False)
        End If
        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(PRSN_ID))
    End Sub
    Private Sub AbilitaIscritto(ByVal PRSN_ID As Integer, ByVal CMNT_ID As Integer)
        Dim oTreeComunita As New COL_TreeComunita

        oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
        oTreeComunita.Nome = PRSN_ID & ".xml"
        If Session("AdminForChange") = False Then
            oTreeComunita.CambiaAbilitazione(Session("idComunita"), True)
        Else
            oTreeComunita.CambiaAbilitazione(Session("idComunita_forAdmin"), True)
        End If
        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(PRSN_ID))
    End Sub
    Private Sub AttivaIscritto(ByVal PersonaId As Integer, ByVal ComunitaID As Integer, ByVal LinguaCode As String)
        Dim oTreeComunita As New COL_TreeComunita
        Dim oResourceUtente As New ResourceManager
        oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PersonaId & "\"
        oTreeComunita.Nome = PersonaId & ".xml"

        oResourceUtente = Me.oResource
        Try
            oResourceUtente.UserLanguages = LinguaCode
            oResourceUtente.ResourcesName = "pg_GestioneIscritti"
            oResourceUtente.Folder_Level1 = "Comunita"
            oResourceUtente.setCulture()
        Catch ex As Exception

        End Try

        If Session("AdminForChange") = False Then
            oTreeComunita.CambiaAttivazione(Session("idComunita"), True, oResourceUtente)
        Else
            oTreeComunita.CambiaAttivazione(Session("idComunita_forAdmin"), True, oResourceUtente)
        End If
        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(PersonaId))
    End Sub
#End Region

#Region "Modifica Ruolo"

    Private Sub BTNmodifica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBsalva.Click

        'cambia il ruolo alla persona
        If Session("azione") = "modifica" Then
            Try
                Dim PRSN_ID, TPRL_ID As Integer
                Dim isAbilitato, isAttivato, isResponsabile As Boolean
                Dim oRuoloPersonaComunita As New COL_RuoloPersonaComunita

                TPRL_ID = Me.DDLruolo.SelectedItem.Value
                oRuoloPersonaComunita.Id = Me.HDrlpc.Value
                oRuoloPersonaComunita.TipoRuolo.Id = TPRL_ID
                oRuoloPersonaComunita.isResponsabile = CHBresponsabile.Checked
                oRuoloPersonaComunita.CambiaRuolo(Me.HDNprsnID.Value)

                If CBool(Me.HDNrlpc_Attivato.Value) Then
                    isAttivato = CBool(Me.HDNrlpc_Attivato.Value)
                Else
                    isAttivato = False
                End If
                If CBool(Me.HDNrlpc_Abilitato.Value) Then
                    isAbilitato = CBool(Me.HDNrlpc_Abilitato.Value)
                Else
                    isAbilitato = False
                End If
                If Me.CHBresponsabile.Checked = True Then
                    isResponsabile = True
                Else
                    isResponsabile = False
                End If
                PRSN_ID = Me.HDNprsnID.Value
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(PRSN_ID))
            Catch ex As Exception

            End Try
        End If
        Session("azione") = "loaded"
        Me.MostraLinkbutton(True)
        Me.Bind_Griglia(True)
        Me.Reset_ToListaUtenti()
    End Sub

    Private Sub BTNannulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBannulla.Click
        Me.MostraLinkbutton(True)
        Me.Bind_Griglia(False)
        Me.Reset_ToListaUtenti()
        Session("azione") = "loaded"
    End Sub

#End Region

#Region "Menu Azione"
    Private Sub LNBabilita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBabilita.Click
        Dim ComunitaID As Integer
        If Session("AdminForChange") = True Then
            Try
                ComunitaID = Session("idComunita_forAdmin")
            Catch ex As Exception
                ComunitaID = Session("IdComunita")
            End Try
        Else
            ComunitaID = Session("IdComunita")
        End If
        Dim Selezionato As String
        If Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.Bloccati Or Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.Tutti Or Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.NuoviIscritti Then
            If Me.HDazione.Value <> "," Then
                Me.AbilitaIscritti(Me.HDazione.Value, ComunitaID)
            End If
        ElseIf Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.InAttesa Then
            If Me.HDazione.Value <> "," Then
                Me.AttivaIscritti(Me.HDazione.Value, ComunitaID)
            End If
        End If

        Me.HDazione.Value = ","
        viewstate("Paginazione") = ""
        Me.Bind_Visualizzazione(Me.DDLiscrizione.SelectedValue)
        Me.Bind_Griglia(False)
    End Sub
    Private Sub LNBdisabilita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdisabilita.Click
        Dim ComunitaID As Integer
        If Session("AdminForChange") = True Then
            Try
                ComunitaID = Session("idComunita_forAdmin")
            Catch ex As Exception
                ComunitaID = Session("IdComunita")
            End Try
        Else
            ComunitaID = Session("IdComunita")
        End If

        Dim Selezionato As String
        If Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.Attivati Or Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.Tutti Or Me.DDLiscrizione.SelectedValue = Main.TipoAttivazione.NuoviIscritti Then
            If Me.HDazione.Value <> "," Then
                Me.DisabilitaIscritti(Me.HDazione.Value, ComunitaID)
            End If
        End If

        Me.HDazione.Value = ","
        viewstate("Paginazione") = ""
        Me.Bind_Visualizzazione(Me.DDLiscrizione.SelectedValue)
        Me.Bind_Griglia(False)
    End Sub
    Private Sub LNBelimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBelimina.Click
        Try
            Dim ComunitaID As Integer
            Dim ComunitaPath As String
            Me.HDNelencoID.Value = Me.HDazione.Value
            'Me.LBinfoDeIscrivi_multiplo.Text = "Sicuro di voler cancellare l'iscrizione degli utenti selezionati ? <br>" _
            '    & " utilizzare i pulsanti sottostanti per annullare l'operazione, per cancellare l'iscrizione alla sola comunità corrente o " _
            '    & " per cancellare l'iscrizione anche alle relative sottocomunità."

            Session("azione") = "deiscrivi"

            Try
                If Session("AdminForChange") = False Then
                    ComunitaID = Session("IdComunita")
                    Try
                        Dim ArrComunita(,) As String = Session("ArrComunita")
                        ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
                    Catch exc As Exception
                        ComunitaPath = "."
                    End Try
                Else
                    ComunitaID = Session("idComunita_forAdmin")
                    ComunitaPath = Session("CMNT_path_forAdmin")
                End If
            Catch ex As Exception
                ComunitaID = 0
            End Try
            Me.HDNcmnt_ID.Value = ComunitaID
            Me.HDNcmnt_Path.Value = ComunitaPath
            Me.Reset_ToDeIscriviMultiplo()
            '    Me.DeIscrivi(ComunitaID, ComunitaPath, Me.HDNprsn_Id.Value)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LNBcancellaInAttesa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcancellaInAttesa.Click
        Dim ComunitaID, i, totale As Integer
        Dim ElencoID(), ComunitaPath As String

        If Session("AdminForChange") = True Then
            Try
                ComunitaID = Session("idComunita_forAdmin")
                ComunitaPath = Session("CMNT_path_forAdmin")
            Catch ex As Exception
                ComunitaID = Session("IdComunita")
            End Try
        Else
            ComunitaID = Session("IdComunita")
        End If
        If ComunitaPath <> "" Then
            Try
                Dim ArrComunita(,) As String = Session("ArrComunita")
                ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
            Catch exc As Exception
                ComunitaPath = "."
            End Try
        End If

        Try
            Dim oComunita As New COL_Comunita
            oComunita.Id = ComunitaID
            oComunita.EliminaUtentiInAttesa(Me.HDazione.Value)
            If oComunita.Errore = Errori_Db.None Then
                ElencoID = Me.HDazione.Value.Split(",")
                For i = 0 To UBound(ElencoID)
                    If IsNumeric(ElencoID(i)) Then
                        Try
                            Me.AggiornaProfiloXML(ComunitaID, ElencoID(i), ComunitaPath)
                        Catch ex As Exception

                        End Try
                    End If
                Next
            End If
            Me.HDazione.Value = ","
        Catch ex As Exception

        End Try
        Me.Bind_Visualizzazione()
        Me.Bind_Griglia(True)
    End Sub
#End Region

#Region "Gestione DeIscrizione"
    Private Sub DeIscrivi(ByVal ComunitaID As Integer, ByVal ComunitaPath As String, ByVal PRSN_ID As Integer)
        Dim oComunita As New COL_Comunita
        Dim i, totale As Integer
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona

        Try
            oPersona.Id = PRSN_ID
            oPersona.Estrai(Session("LinguaID"))

            If Session("azione") = "deiscrivi" Then
                Try
                    Dim multipli As Boolean = False


                    oComunita.Id = ComunitaID
                    Try
                        Dim oDataview As DataView
                        Dim hasCreatori, HasPassanti, HasOther As Boolean

                        oDataset = oPersona.NEW_ElencaComunitaDaDeiscrivere(ComunitaID, ComunitaPath)
                        totale = oDataset.Tables(0).Rows.Count
                        If totale > 1 Then
                            oDataview = oDataset.Tables(0).DefaultView
                            oDataview.RowFilter = "CMNT_ID <> " & ComunitaID

                            ' Esistono dei creatori
                            oDataview.RowFilter = "CMNT_ID <> " & ComunitaID & " and RLPC_TPRL_ID = -2"
                            hasCreatori = (oDataview.Count > 0)

                            'esistono dei passanti.....
                            oDataview.RowFilter = "CMNT_ID <> " & ComunitaID & " and RLPC_TPRL_ID = -3"
                            HasPassanti = (oDataview.Count > 0)

                            'altri....
                            oDataview.RowFilter = "CMNT_ID <> " & ComunitaID & " and RLPC_TPRL_ID <> -3 and RLPC_TPRL_ID <> -2"
                            HasOther = (oDataview.Count > 0)

                            If Not HasOther And Not hasCreatori And HasPassanti Then
                                'Ho sotto comunità solo come passante......
                                multipli = False
                            ElseIf Not HasOther And hasCreatori And Not HasPassanti Then
                                'Ho sotto comunità solo come creatore......
                                multipli = False
                            Else
                                multipli = True
                            End If
                        Else
                            multipli = False
                        End If

                        If multipli Then
                            Me.HDNcmnt_ID.Value = ComunitaID
                            Me.HDNcmnt_Path.Value = ComunitaPath

                            Me.Reset_ToDeIscriviSingolo()
                            oResource.setLabel(LBinfoDeIscrivi)
                            LBinfoDeIscrivi.Text = LBinfoDeIscrivi.Text.Replace("#%#", "<b>" & oPersona.Cognome & " " & oPersona.Nome & "</b>") '& " di cui vuole annullare l'iscrizione, è iscritta ad altre sottocomunità, desidera cancellare l'iscrizione anche alle comunità elencate qui sotto ?"
                            Me.Bind_AlberoDeIscrizione(oDataset)
                            
                        Else
                            oPersona.NEW_DeIscriviFromComunita(ComunitaID, ComunitaPath, False)
                            If oPersona.Errore = Errori_Db.None Then
                                Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                                oServiceUtility.NotifyDeleteSubscription(ComunitaID, oPersona.ID, oPersona.Anagrafica, Me.PageUtility.CurrentUser.Anagrafica)
                                Me.AggiornaProfiloXML(ComunitaID, oPersona.ID, ComunitaPath)
                            End If
                            Session("azione") = "loaded"
                            Me.MostraLinkbutton(True)
                            Me.Bind_Griglia(True)
                            Me.Reset_ToListaUtenti()
                        End If
                    Catch ex As Exception

                    End Try

                Catch ex As Exception

                End Try
            Else
                Me.MostraLinkbutton(True)
                Me.Bind_Griglia()
                Me.Reset_ToListaUtenti()
                Session("azione") = "loaded"
            End If
        Catch ex As Exception
            Me.MostraLinkbutton(True)
            Me.Bind_Griglia()
            Me.Reset_ToListaUtenti()
            Session("azione") = "loaded"
        End Try

    End Sub

    Private Sub AggiornaProfiloXML(ByVal CMNT_ID As Integer, ByVal PRSN_Id As Integer, ByVal CMNT_Path As String)
        Dim oRuolo As New COL_RuoloPersonaComunita
        Dim oTreeComunita As New COL_TreeComunita

        Try
            oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_Id)
            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_Id & "\"
            oTreeComunita.Nome = PRSN_Id & ".xml"

            If oRuolo.Errore = Errori_Db.None Then

            Else
                oTreeComunita.Delete(CMNT_ID, CMNT_Path)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub LNBdeIscriviCorrente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdeIscriviCorrente.Click
        Dim oPersona As New COL_Persona
        Dim i, totale As Integer
        Dim oDataset As New DataSet

        If Session("azione") = "deiscrivi" Then
            Try
                oPersona.ID = Me.HDNprsn_Id.Value
                oPersona.Estrai(Me.PageUtility.LinguaID)
                If oPersona.Errore = Errori_Db.None Then
                    oPersona.NEW_DeIscriviFromComunita(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value, False)

                    If oPersona.Errore = Errori_Db.None Then

                        Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                        oServiceUtility.NotifyDeleteSubscription(Me.HDNcmnt_ID.Value, oPersona.ID, oPersona.Anagrafica, Me.PageUtility.CurrentUser.Anagrafica)
                        Me.AggiornaProfiloXML(Me.HDNcmnt_ID.Value, oPersona.ID, Me.HDNcmnt_Path.Value)
                    End If
                End If
               
            Catch ex As Exception

            End Try
        End If
        Session("azione") = "loaded"
        Me.HDNprsn_Id.Value = 0
        Me.HDNcmnt_ID.Value = 0
        Me.HDNcmnt_Path.Value = "."
        Me.Reset_ToListaUtenti()
        Me.Bind_Griglia(True)
    End Sub
    Private Sub LNBdeIscriviTutte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdeIscriviTutte.Click
        Dim oPersona As New COL_Persona
        Dim i, totale As Integer
        Dim oDataset As New DataSet

        If Session("azione") = "deiscrivi" Then
            Try
                oPersona.Id = Me.HDNprsn_Id.Value
                oPersona.NEW_DeIscriviFromComunita(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value, True)
                If oPersona.Errore = Errori_Db.None Then
                    Me.AggiornaProfiloXML(Me.HDNcmnt_ID.Value, oPersona.Id, Me.HDNcmnt_Path.Value)
                End If
            Catch ex As Exception

            End Try
        End If
        Me.HDNprsn_Id.Value = 0
        Me.HDNcmnt_ID.Value = 0
        Me.HDNcmnt_Path.Value = ""
        Session("azione") = "loaded"
        Me.Reset_ToListaUtenti()
        Me.Bind_Griglia(True)
    End Sub

    Private Sub LNBdeIscriviSelezionate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdeIscriviSelezionate.Click
        Dim oPersona As New COL_Persona
        Dim i, totale As Integer
        Dim oDataset As New DataSet

        Dim elencoCMNT_Path() As String
        Dim CMNT_ID As Integer
        Dim CMNT_PATH As String
        If Session("azione") = "deiscrivi" Then
            Try
                oPersona.Id = Me.HDNprsn_Id.Value

                totale = Me.RDTcomunita.CheckedNodes.Count
                If totale > 0 Then
                    Dim ListaEliminate As String = ","
                    totale -= 1

                    For i = totale To 0 Step -1
                        Dim oNode As RadTreeNode
                        oNode = Me.RDTcomunita.CheckedNodes(i)
                        Try
                            Dim ComunitaID As Integer
                            Dim ComunitaPath As String = ""

							ComunitaID = oNode.Category
                            If ComunitaID > 0 And InStr(ListaEliminate, "," & ComunitaID & ",") <= 0 Then
								ComunitaPath = oNode.Value
								oPersona.NEW_DeIscriviFromComunita(ComunitaID, ComunitaPath, False)
                                If oPersona.Errore = Errori_Db.None Then
									Me.AggiornaProfiloXML(ComunitaID, oPersona.Id, ComunitaPath)
                                    ListaEliminate &= ComunitaID & ","
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                    Next
                End If
            Catch ex As Exception

            End Try
        End If
        Session("azione") = "loaded"
        Me.Reset_ToListaUtenti()
        Me.Bind_Griglia(True)
    End Sub
    Private Sub LNBannullaDeiscrizione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaDeiscrizione.Click
        Me.HDNcmnt_Path.Value = ""
        Session("azione") = "loaded"
        Me.Reset_ToListaUtenti()
        Me.Bind_Griglia()
    End Sub

#Region "Gestione TreeView"
	Private Sub Bind_AlberoDeIscrizione(ByVal oDataset As DataSet)
		Dim oTreeComunita As New COL_TreeComunita
		Dim oPersona As New COL_Persona

		Me.RDTcomunita.Nodes.Clear()
		Try
			Dim i, tot, CMNT_idPadre, CMNT_id As Integer
			oPersona = Session("objPersona")


			Me.RDTcomunita.Nodes.Clear()
			Me.RDTcomunita.CheckBoxes = True
			Dim nodeRoot As New RadTreeNode

			Try
				nodeRoot.Text = oResource.getValue("oRootNode.Text")
				nodeRoot.ToolTip = oResource.getValue("oRootNode.ToolTip")
				If nodeRoot.Text = "" Then
					nodeRoot.Text = "Comunità: "
					nodeRoot.ToolTip = "Comunità: "
				End If
			Catch ex As Exception
				nodeRoot.Text = "Comunità: "
				nodeRoot.ToolTip = "Comunità: "
			End Try

			nodeRoot.Expanded = True
			nodeRoot.ImageUrl = "folder.gif"
			nodeRoot.Value = ""
			nodeRoot.Category = 0
			nodeRoot.CssClass = "confirmDelete_NomeComunità"
			nodeRoot.Checkable = False
			Me.RDTcomunita.Nodes.Add(nodeRoot)

			If oDataset.Tables(0).Rows.Count = 0 Then
				Me.GeneraNoNode()
			Else
				oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("ALCM_PAth"), oDataset.Tables(0).Columns("ALCM_RealPath"), False)
				Dim IDcorrente As Integer
				IDcorrente = Me.HDNcmnt_ID.Value
				'Response.Write("<br><br><br><br><br><br><br><br><br><br><br><br><br>")
				Dim dbRow As DataRow
				For Each dbRow In oDataset.Tables(0).Rows

					If dbRow.Item("ALCM_PAth") = dbRow.Item("ALCM_RealPath") Then
						dbRow.Item("ALCM_RealPath") = ""
					End If
					If dbRow("ALCM_PadreVirtuale_ID") = 0 Or dbRow("CMNT_id") = IDcorrente Then
						'		Response.Write(dbRow.Item("CMNT_Nome") & " ALCM_PAth=" & dbRow.Item("ALCM_PAth") & " ALCM_RealPath=" & dbRow.Item("ALCM_RealPath") & "<br>")
						Dim node As RadTreeNode = CreateNode(dbRow, True, nodeRoot)
						If Not IsNothing(node) Then
							nodeRoot.Nodes.Add(node)
							RecursivelyPopulate(dbRow, node, nodeRoot)
						End If
					End If
				Next dbRow
			End If
		Catch ex As Exception
			Me.GeneraNoNode()
		End Try
	End Sub
    Private Sub RecursivelyPopulate(ByVal dbRow As DataRow, ByVal node As RadTreeNode, ByVal nodeFather As RadTreeNode)
        Dim childRow As DataRow

        For Each childRow In dbRow.GetChildRows("NodeRelation")
            Dim childNode As RadTreeNode = CreateNode(childRow, False, node)

			'Response.Write(childRow.Item("CMNT_Nome") & " ALCM_PAth=" & childRow.Item("ALCM_PAth") & " ALCM_RealPath=" & childRow.Item("ALCM_RealPath") & "<br>")
			If Not (IsNothing(childNode)) Then
				If childNode.Category < 0 Then
					If childRow.GetChildRows("NodeRelation").GetLength(0) > 0 Then
						node.Nodes.Add(childNode)
						RecursivelyPopulate(childRow, childNode, node)
						If childNode.Nodes.Count = 0 Then
							childNode.Remove()
						End If
					End If
				Else
					node.Nodes.Add(childNode)
					RecursivelyPopulate(childRow, childNode, node)
				End If
			End If
        Next childRow
    End Sub
    Private Function CreateNode(ByVal dbRow As DataRow, ByVal expanded As Boolean, ByVal nodeFather As RadTreeNode) As RadTreeNode
        Dim node As New RadTreeNode
        Dim start As Integer
        Dim [continue] As Boolean = False

        start = 0

        Dim CMNT_id, RLPC_TPRL_id As Integer
        Dim CMNT_Responsabile, img As String
        Dim CMNT_isIscritto, CanUnsubscribe As Boolean
        CMNT_id = dbRow.Item("CMNT_id")

        Dim ImageBaseDir As String
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")
        ImageBaseDir = Replace(ImageBaseDir, "//", "/")

		Dim CMNT_Nome, CMNT_NomeVisibile, CMNT_path As String
        Dim CMNT_IsChiusa As Boolean

        CMNT_Nome = dbRow.Item("CMNT_Nome")
        CMNT_NomeVisibile = CMNT_Nome
        CMNT_IsChiusa = dbRow.Item("ALCM_IsChiusa")
        If CMNT_id > 0 Then
            If IsDBNull(dbRow.Item("RLPC_TPRL_id")) Then
                RLPC_TPRL_id = -1
                CMNT_isIscritto = False
            Else
                RLPC_TPRL_id = dbRow.Item("RLPC_TPRL_id")

                If RLPC_TPRL_id > 0 Then
                    CMNT_isIscritto = True
                Else
                    CMNT_isIscritto = False
                End If
            End If

            'TROVO IL RESPONSABILE
            If IsDBNull(dbRow.Item("ALCM_Responsabile")) Then
                CMNT_Responsabile = ""
                If Not IsDBNull(dbRow.Item("ALCM_Creatore")) Then
                    CMNT_Responsabile = oResource.getValue("creata")
                    CMNT_Responsabile = CMNT_Responsabile.Replace("#%%#", dbRow.Item("ALCM_Creatore"))
                End If
            Else
                CMNT_Responsabile = " (" & dbRow.Item("ALCM_Responsabile") & ") "
            End If
            If IsDBNull(dbRow.Item("TPCM_icona")) Then
                img = ""
            Else
                img = dbRow.Item("TPCM_icona")
                img = "./logo/" & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
            End If
            dbRow.Item("TPCM_icona") = img
        Else
            CMNT_isIscritto = True
            img = ""
        End If

        Try
            If IsDBNull(dbRow.Item("CMNT_CanUnsubscribe")) Then
                CanUnsubscribe = True
            Else
                CanUnsubscribe = dbRow.Item("CMNT_CanUnsubscribe")
            End If
        Catch ex As Exception
            CanUnsubscribe = True
        End Try


        If CMNT_id > 0 Then
            CMNT_Nome = CMNT_Nome & CMNT_Responsabile
            CMNT_NomeVisibile = CMNT_Nome
            CMNT_Nome = CMNT_Nome & Me.GenerateImage(ImageBaseDir & oResource.getValue("stato.image." & CMNT_IsChiusa), oResource.getValue("stato." & CMNT_IsChiusa))
        Else
            CMNT_NomeVisibile = CMNT_Nome
        End If
        CMNT_path = dbRow.Item("ALCM_path")
		'   CMNT_REALpath = dbRow.Item("ALCM_REALpath")

        Dim isBloccata As Boolean = False
        isBloccata = dbRow.Item("CMNT_Bloccata")

        If CMNT_id = Me.HDNcmnt_ID.Value Then
            CMNT_Nome = "<b>" & CMNT_Nome & "</b>"
        End If
        node.Text = CMNT_Nome
		'node.Value = CMNT_REALpath
		node.Value = CMNT_path
        node.Expanded = expanded
        node.ImageUrl = img
        node.ToolTip = CMNT_NomeVisibile
        node.Category = CMNT_id
        node.Checkable = True
        If CMNT_id = Me.HDNcmnt_ID.Value Then
            node.Checked = True
            node.Enabled = False
        ElseIf CMNT_isIscritto Then
            node.Checkable = True '(Not isBloccata And CanUnsubscribe)
            node.Enabled = True '0Not isBloccata
            If isBloccata Or CanUnsubscribe = False Then
                node.CssClass = "TreeNodeBloccata"
            End If
        Else
            node.Category = -CMNT_id
            node.Checkable = False
            node.CssClass = "TreeNodeBloccata"
        End If
        Return node
    End Function 'CreateNode

    Private Function GeneraNoNode()
        Dim oRootNode As New RadTreeNode
        Dim oNode As New RadTreeNode

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        oRootNode = New RadTreeNode

        oRootNode.Value = ""
        oRootNode.Expanded = True
        oRootNode.ImageUrl = "folder.gif"

        oRootNode.Category = 0
        oRootNode.Checkable = False
        Try
            oRootNode.Text = oResource.getValue("oRootNode.Text")
            If oRootNode.Text = "" Then
                oRootNode.Text = "Comunità: "
            End If
        Catch ex As Exception
            oRootNode.Text = "Comunità: "
        End Try
        Try
            oRootNode.ToolTip = oResource.getValue("oRootNode.ToolTip")
            If oRootNode.ToolTip = "" Then
                oRootNode.ToolTip = "Elenco comunità a cui si è iscritti"
            End If
        Catch ex As Exception
            oRootNode.ToolTip = "Elenco comunità a cui si è iscritti"
        End Try
        oRootNode.Checkable = False

        oNode = New RadTreeNode
        oNode.Expanded = True
        oNode.Value = ""
        Try
            oNode.ToolTip = oResource.getValue("NoNode.ToolTip")
            If oNode.ToolTip = "" Then
                oNode.ToolTip = "Non si è iscritti ad alcuna comunità"
            End If
        Catch ex As Exception
            oNode.ToolTip = "Non si è iscritti ad alcuna comunità"
        End Try
        Try
            oNode.Text = oResource.getValue("NoNode.Text")
            If oNode.Text = "" Then
                oNode.Text = "Non si è iscritti ad alcuna comunità"
            End If
        Catch ex As Exception
            oNode.Text = "Non si è iscritti ad alcuna comunità"
        End Try
        oNode.Category = 0
        oNode.Checkable = False

        oRootNode.Nodes.Add(oNode)

        Me.RDTcomunita.Nodes.Clear()
        Me.RDTcomunita.Nodes.Add(oRootNode)
    End Function

    Private Function GenerateImage(ByVal ImageName As String, Optional ByVal Status As String = "") As String
        Dim imageUrl As String
        Dim quote As String
        quote = """"

        imageUrl = "<img  align=absmiddle src=" & quote & ImageName & quote & " alt=" & quote & Status & quote

        imageUrl = imageUrl & " " & " onmouseover=" & quote & "window.status='" & Replace(Status, "'", "\'") & "';return true;" & quote & " "
        imageUrl = imageUrl & " " & " onfocus=" & quote & "window.status='" & Replace(Status, "'", "\'") & "';return true;" & quote & " "
        imageUrl = imageUrl & " " & " onmouseout=" & quote & "window.status='';return true;" & """" & " "
        imageUrl = imageUrl & " >"

        Return imageUrl
    End Function

#End Region

#End Region

#Region "Deiscrizione multipla"
    Private Sub LNBdeIscriviTutte_multi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdeIscriviTutte_multi.Click
        Dim ComunitaID, i, totale, PRSN_ID As Integer
        Dim ElencoID(), ComunitaPath As String
        Dim isForAdmin As Boolean = False

        Try
            isForAdmin = Session("AdminForChange")
        Catch ex As Exception

        End Try

        ComunitaPath = ""

        If isForAdmin Then
            Try
                ComunitaID = Session("idComunita_forAdmin")
                ComunitaPath = Session("CMNT_path_forAdmin")
            Catch ex As Exception
                ComunitaID = Session("IdComunita")
            End Try
        Else
            ComunitaID = Session("IdComunita")
        End If
        If ComunitaPath = "" And Not isForAdmin Then
            Try
                Dim ArrComunita(,) As String = Session("ArrComunita")
                ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
            Catch exc As Exception
                ComunitaPath = "."
            End Try
        End If
        If ComunitaPath = "" Then
            Exit Sub
        End If

        Try
            ElencoID = Me.HDazione.Value.Split(",")

            Dim oPersona As New COL_Persona
            For i = 0 To UBound(ElencoID)
                If IsNumeric(ElencoID(i)) Then
                    Try
                        oPersona.Id = ElencoID(i)
                        oPersona.NEW_DeIscriviFromComunita(ComunitaID, ComunitaPath, True)
                        If oPersona.Errore = Errori_Db.None Then
                            Me.AggiornaProfiloXML(ComunitaID, oPersona.ID, ComunitaPath)
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                        End If
                    Catch ex As Exception

                    End Try
                End If
            Next
        Catch ex As Exception

        End Try

        Session("azione") = "loaded"
        Me.MostraLinkbutton(True)
        Me.Reset_ToListaUtenti()
        Me.Bind_Griglia(True)
    End Sub
    Private Sub LNBdeIscriviCorrente_multi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdeIscriviCorrente_multi.Click
        Dim ComunitaID, i, totale, PRSN_ID As Integer
        Dim ElencoID(), ComunitaPath As String
        Dim isForAdmin As Boolean = False

        Try
            isForAdmin = Session("AdminForChange")
        Catch ex As Exception

        End Try

        ComunitaPath = ""
        If isForAdmin Then
            Try
                ComunitaID = Session("idComunita_forAdmin")
                ComunitaPath = Session("CMNT_path_forAdmin")
            Catch ex As Exception
                ComunitaID = Session("IdComunita")
            End Try
        Else
            ComunitaID = Session("IdComunita")
        End If
        If ComunitaPath = "" And Not isForAdmin Then
            Try
                Dim ArrComunita(,) As String = Session("ArrComunita")
                ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
            Catch exc As Exception
                ComunitaPath = "."
            End Try
        End If
        If ComunitaPath = "" Then
            Exit Sub
        End If

        Try
            ElencoID = Me.HDazione.Value.Split(",")

            Dim oPersona As New COL_Persona
            For i = 0 To UBound(ElencoID)
                If IsNumeric(ElencoID(i)) Then
                    PRSN_ID = ElencoID(i)
                    Try
                        oPersona.Id = PRSN_ID
						oPersona.NEW_DeIscriviFromComunita(ComunitaID, ComunitaPath, False)
                        If oPersona.Errore = Errori_Db.None Then
                            Me.AggiornaProfiloXML(ComunitaID, oPersona.ID, ComunitaPath)
                            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                        End If
                    Catch ex As Exception

                    End Try
                End If
            Next

        Catch ex As Exception

        End Try

        Session("azione") = "loaded"
        Me.MostraLinkbutton(True)
        Me.Reset_ToListaUtenti()
        Me.Bind_Griglia(True)
    End Sub
    Private Sub LNBannulla_multi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannulla_multi.Click
        Session("azione") = "loaded"
        Me.Reset_ToListaUtenti()
        Me.Bind_Griglia()
    End Sub
#End Region

    Private Sub LNBgotoGestioneComunita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBgotoGestioneComunita.Click
        Session("AdminForChange") = False
        Session("idComunita_forAdmin") = ""
        Session("CMNT_path_forAdmin") = ""
        If Request.QueryString("topage") <> "" Then
            Me.PageUtility.RedirectToUrl(Me.PageUtility.SystemSettings.Presenter.DefaultManagement & "?re_set=true")
        ElseIf Request.QueryString("toTree") <> "" Then
            Me.PageUtility.RedirectToUrl(Me.PageUtility.SystemSettings.Presenter.DefaultManagement & "?re_set=true&toTree=true")
        End If
    End Sub

#Region "Funzioni Excel"
    Private Sub LNBexcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBexcel.Click
        Try
            Dim oComunita As New COL_Comunita
            Dim nomeComunita As String
            Dim FileName As String

            If Session("AdminForChange") = False Then
				nomeComunita = COL_Comunita.EstraiNomeBylingua(Session("IdComunita"), Session("LinguaID"))
            Else
				nomeComunita = COL_Comunita.EstraiNomeBylingua(Session("idComunita_forAdmin"), Session("LinguaID"))
            End If
            'If nomeComunita <> "" Then
            '    nomeComunita = Replace(nomeComunita, "'", "_")
            '    If Len(nomeComunita) > 20 Then
            '        nomeComunita = Left(nomeComunita, 20)
            '    End If

            '    FileName = "IscrittiComunita_" & nomeComunita & ".xls"
            'Else
            Dim crDate As DateTime = DateTime.Now
            FileName = crDate.Year.ToString & "-" & crDate.Month.ToString("D2") & "-" & crDate.Year.ToString("D2") & "-IscrittiComunita.xls"
            'End If

            Dim oStringWriter As System.IO.StringWriter = New System.IO.StringWriter
            Dim oHTMLWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)


            Response.Buffer = True
            Page.Response.Clear()

            Me.Bind_GrigliaExcel()


            Me.TBLexcel.RenderControl(oHTMLWriter)

            Dim OpenType As String = "attachment"
            Page.Response.ContentType = "application/ms-excel"
            Page.Response.Charset = "utf-8"
            Page.Response.BinaryWrite(Response.ContentEncoding.GetPreamble())
            Page.Response.AddHeader("Content-Disposition", OpenType + ";filename=" + FileName)

            Page.Response.Write("<html>" & vbCrLf)
            Page.Response.Write("<head>" & vbCrLf)
            Page.Response.Write("<META content=Excel.Sheet charset=utf-8/>" & vbCrLf)
            Page.Response.Write("</head>" & vbCrLf)
            Context.Response.Output.Write(ConvertEncoding(oStringWriter) & vbCrLf)


            Page.Response.Write("</body>")

            Response.Flush()
            Context.Response.End()
        Catch ex As Exception

        End Try
    End Sub

    Private Function ConvertEncoding(ByVal oStringWriter As System.IO.StringWriter) As String
        Dim unicodeString As String = oStringWriter.ToString

        ' Create two different encodings.
        Dim ascii As System.Text.Encoding = System.Text.Encoding.UTF8
        Dim [unicode] As System.Text.Encoding = System.Text.Encoding.Unicode

        ' Convert the string into a byte[].
        Dim unicodeBytes As Byte() = [unicode].GetBytes(unicodeString)

        ' Perform the conversion from one encoding to the other.
        Dim asciiBytes As Byte() = System.Text.Encoding.Convert([unicode], ascii, unicodeBytes)

        ' Convert the new byte[] into a char[] and then into a string.
        ' This is a slightly different approach to converting to illustrate
        ' the use of GetCharCount/GetChars.
        Dim asciiChars(ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)) As Char
        ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0)
        Dim asciiString As New String(asciiChars)

        Return asciiString
    End Function

    Private Sub ExcelHeader(hasMatricula As Boolean)
        Dim oTableRow As New TableRow
        Dim oTableCell As New TableCell
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona

        Try
            oPersona = Session("objPersona")
            If Session("AdminForChange") = False Then
                oComunita.Id = Session("IdComunita")
            Else
                oComunita.Id = Session("idComunita_forAdmin")
            End If
            oComunita.Estrai()

            oTableCell.ColumnSpan = 6 + IIf(hasMatricula, 1, 0)
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.Font.Bold = True
            oTableCell.Font.Size = FontUnit.Point(20)
            oTableCell.Font.Name = "Verdana"
            oTableCell.ForeColor = System.Drawing.Color.Black
            oTableCell.VerticalAlign = VerticalAlign.Middle
            oTableCell.Text = oResource.getValue("Excel.organizzazione")
            oTableCell.Text = oTableCell.Text.Replace("#c#", oComunita.Organizzazione.RagioneSociale)

            oTableRow.Cells.Add(oTableCell)
            Me.TBLexcel.Rows.Add(oTableRow)

            oTableRow = New TableRow
            oTableCell = New TableCell

            oTableCell.Text = oResource.getValue("Excel.comunita")
            oTableCell.Text = oTableCell.Text.Replace("#c#", oComunita.Nome)
            oTableCell.ColumnSpan = 6 + IIf(hasMatricula, 1, 0)
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.Font.Bold = True
            oTableCell.Font.Size = FontUnit.Point(16)
            oTableCell.Font.Name = "Verdana"
            oTableCell.ForeColor = System.Drawing.Color.DarkBlue
            oTableCell.VerticalAlign = VerticalAlign.Middle
            oTableRow.Cells.Add(oTableCell)
            Me.TBLexcel.Rows.Add(oTableRow)

            oTableRow = New TableRow
            oTableCell = New TableCell
            oTableCell.ColumnSpan = 6 + IIf(hasMatricula, 1, 0)
            oTableCell.Text = oResource.getValue("Excel.stampato")
            oTableCell.Font.Italic = True
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.HorizontalAlign = HorizontalAlign.Left
            oTableCell.Text = oTableCell.Text.Replace("#users#", oPersona.Cognome & " " & oPersona.Nome)
            oTableCell.Text = oTableCell.Text.Replace("#data#", DateTime.Now.ToString("D", oResource.CultureInfo))
            oTableRow.Cells.Add(oTableCell)
            Me.TBLexcel.Rows.Add(oTableRow)

            oTableRow = New TableRow
            oTableCell = New TableCell
            oTableCell.ColumnSpan = 6 + IIf(hasMatricula, 1, 0)
            oTableCell.Text = "&nbsp"
            oTableCell.Height = Unit.Pixel(20)
            Me.TBLexcel.Rows.Add(oTableRow)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ExcelHeader_UserList(hasMatricula As Boolean)
        Dim oTableRow As New TableRow
        Dim oTableCell As New TableCell


        Try
            oTableRow = New TableRow

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.Font.Bold = True
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = "&nbsp;"
            oTableCell.Width = Unit.Pixel(70)
            oTableRow.Cells.Add(oTableCell)

            If hasMatricula Then
                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.Font.Bold = True
                oTableCell.HorizontalAlign = HorizontalAlign.Center
                oTableCell.ForeColor = System.Drawing.Color.White
                oTableCell.BackColor = System.Drawing.Color.DarkBlue
                oTableCell.Text = oResource.getValue("Excel.Matricola")
                oTableRow.Cells.Add(oTableCell)
            End If
            

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.Font.Bold = True
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue

            oTableCell.Text = oResource.getValue("Excel.Nome")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.Font.Bold = True
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue

            oTableCell.Text = oResource.getValue("Excel.Cognome")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.Font.Bold = True
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = oResource.getValue("Excel.Mail")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.Font.Bold = True
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = oResource.getValue("Excel.Ruolo")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.Font.Bold = True
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = oResource.getValue("Excel.IscrittoIl")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.Font.Bold = True
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = oResource.getValue("Excel.UltimoCollegamento")
            oTableRow.Cells.Add(oTableCell)

            oTableCell = New TableCell
            oTableCell.Font.Size = FontUnit.Point(10)
            oTableCell.Font.Name = "Verdana"
            oTableCell.Font.Bold = True
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.ForeColor = System.Drawing.Color.White
            oTableCell.BackColor = System.Drawing.Color.DarkBlue
            oTableCell.Text = oResource.getValue("Excel.Status")
            oTableRow.Cells.Add(oTableCell)

            Me.TBLexcel.Rows.Add(oTableRow)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_GrigliaExcel(Optional ByVal ricalcola As Boolean = False)
        Dim oPersona As New COL_Persona
        Dim oComunita As New COL_Comunita
        Dim oDataSet As New DataSet
        Dim i, totale As Integer
        Try

            oPersona = Session("objPersona")

            If Session("AdminForChange") = False Then
                oComunita.Id = Session("IdComunita")
            Else
                oComunita.Id = Session("idComunita_forAdmin")
            End If

            oDataSet = oComunita.ElencaIscrittiNoMittente(Session("LinguaID"), oPersona.Id, Main.FiltroAbilitazione.Tutti, Main.FiltroUtenti.NoPassantiNoCreatori, , , , , , Main.FiltroOrdinamento.Crescente, COL_Comunita.FiltroCampoOrdine.anagrafica)
            Dim hasMatricula As Boolean = oDataSet.Tables(0).Rows.OfType(Of DataRow).ToList().Where(Function(rw As DataRow) Not rw.IsNull("STDN_matricola")).Any()
            Me.ExcelHeader(hasMatricula)
            totale = oDataSet.Tables(0).Rows.Count - 1

            Dim oTableRow As New TableRow
            Dim oTableCell As New TableCell



            Me.ExcelHeader_UserList(hasMatricula)

            For i = 0 To totale
                Dim oRow As DataRow
                oRow = oDataSet.Tables(0).Rows(i)

                oTableRow = New TableRow

                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.Black
                oTableCell.HorizontalAlign = HorizontalAlign.Right
                oTableCell.Text = i + 1
                oTableCell.Width = Unit.Pixel(70)
                oTableRow.Cells.Add(oTableCell)

                If (hasMatricula) Then
                    oTableCell = New TableCell
                    oTableCell.Font.Size = FontUnit.Point(10)
                    oTableCell.Font.Name = "Verdana"
                    oTableCell.BorderColor = Color.DarkBlue
                    oTableCell.HorizontalAlign = HorizontalAlign.Left
                    If IsDBNull(oRow.Item("STDN_matricola")) Then
                        oTableCell.Text = "&nbsp;"
                    Else
                        If oRow.Item("STDN_matricola") = "-1" Then
                            oTableCell.Text = Me.oResource.getValue("Excel.noMatricola")
                        Else
                            oTableCell.Text = oRow.Item("STDN_matricola")
                        End If

                    End If
                    oTableRow.Cells.Add(oTableCell)
                End If
               

                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oTableCell.Text = oRow.Item("PRSN_Nome") ' & " (" & oRow.Item("TPPR_descrizione") & ")"
                oTableRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oTableCell.Text = oRow.Item("PRSN_Cognome") ' & " (" & oRow.Item("TPPR_descrizione") & ")"
                oTableRow.Cells.Add(oTableCell)

                'oTableCell = New TableCell
                'oTableCell.Font.Size = FontUnit.Point(10)
                'oTableCell.Font.Name = "Verdana"
                'oTableCell.BorderColor = Color.DarkBlue
                'oTableCell.HorizontalAlign = HorizontalAlign.Left
                'oTableCell.Text = oRow.Item("PRSN_login")
                'oTableRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oTableCell.Text = oRow.Item("PRSN_mail")
                oTableRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oTableCell.Text = oRow.Item("TPRL_nome")
                oTableRow.Cells.Add(oTableCell)

                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                If IsDBNull(oRow.Item("RLPC_IscrittoIl")) = False Then
                    If Equals(oRow.Item("RLPC_IscrittoIl"), New Date) Then
                        oTableCell.Text = "--"
                    Else
                        oTableCell.Text = CDate(oRow.Item("RLPC_IscrittoIl")).ToString("dd/MM/yy HH:mm:ss")
                    End If
                Else
                    oTableCell.Text = "--"
                End If
                oTableRow.Cells.Add(oTableCell)


                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                If IsDBNull(oRow.Item("RLPC_ultimoCollegamento")) = False Then
                    oTableCell.Text = CDate(oRow.Item("RLPC_ultimoCollegamento")).ToString("dd/MM/yy HH:mm:ss")
                Else
                    oTableCell.Text = "--"
                End If
                oTableRow.Cells.Add(oTableCell)


                oTableCell = New TableCell
                oTableCell.Font.Size = FontUnit.Point(10)
                oTableCell.Font.Name = "Verdana"
                oTableCell.BorderColor = Color.DarkBlue
                oTableCell.HorizontalAlign = HorizontalAlign.Left

                If oRow.Item("RLPC_Attivato") = True And oRow.Item("RLPC_Abilitato") = True Then
                    oTableCell.Text = oResource.getValue("abilitato." & StringaAbilitato.abilitato)
                ElseIf oRow.Item("RLPC_Attivato") = True And oRow.Item("RLPC_Abilitato") = False Then
                    oTableCell.Text = oResource.getValue("abilitato." & StringaAbilitato.bloccato)
                ElseIf oRow.Item("RLPC_Attivato") = False Then
                    oTableCell.Text = oResource.getValue("abilitato." & StringaAbilitato.inAttesa)
                Else
                    oTableCell.Text = "&nbsp;"
                End If
                oTableRow.Cells.Add(oTableCell)
                Me.TBLexcel.Rows.Add(oTableRow)
            Next
        Catch ex As Exception
            Dim d As Integer
            d = 5
        End Try
    End Sub
#End Region

    Private Sub LNBiscrivi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscrivi.Click
        If Request.QueryString("topage") <> "" Then
            Response.Redirect("./WizardNuovoIscritto.aspx?topage=true", True)
        ElseIf Request.QueryString("toTree") <> "" Then
            Response.Redirect("./WizardNuovoIscritto.aspx?toTree=true", True)
        Else
            Response.Redirect("./WizardNuovoIscritto.aspx", True)
        End If
    End Sub

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

    'Public ReadOnly Property BodyId As String
    '    Get
    '        Return Me.Master.BodyIdCode
    '    End Get
    'End Property
    'Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
    '    Get
    '        Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
    '    End Get
    'End Property

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

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class


'<%-- 
'             < %= Me.BodyId() % >.onkeydown = alert(event); //return SubmitRicerca(event)

'    	    function SubmitRicerca(event) {
'    	        if (document.all) {
'    	            if (event.keyCode == 13) {
'    	                event.returnValue = false;
'    	                event.cancel = true;
'    	                try {
'    	                    document.forms[0].BTNCerca.click();
'    	                }
'    	                catch (ex) {
'    	                    return false;
'    	                }
'    	            }
'    	        }
'    	        else if (document.getElementById) {
'    	            if (event.which == 13) {
'    	                event.returnValue = false;
'    	                event.cancel = true;
'    	                try {
'    	                    document.forms[0].BTNCerca.click();
'    	                }
'    	                catch (ex) {
'    	                    return false;
'    	                }
'    	            }
'    	        }
'    	        else if (document.layers) {
'    	            if (event.which == 13) {
'    	                event.returnValue = false;
'    	                event.cancel = true;
'    	                try {
'    	                    document.forms[0].BTNCerca.click();
'    	                }
'    	                catch (ex) {
'    	                    return false;
'    	                }
'    	            }
'    	        }
'    	        else
'    	            return true;
'    	    }	
'            --%>