Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_permessi
Imports Telerik.WebControls

Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_ElencaComunita
Imports lm.ActionDataContract


Public Class EntrataComunita
    Inherits System.Web.UI.Page

    Private oResource As ResourceManager
	Private OrganizzazioneDefaultID As Integer

    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label

    Private _PageUtility As OLDpageUtility
    Private ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property

    Private Enum StringaAbilitato
        abilitato = 1
        bloccato = 0
        inAttesa = -1
        comunitaArchiviata = 2
        comunitaBloccata = 3
    End Enum
    Private Enum StringaMessaggio
        responsabile = 2
        ischiusa = 3
        isArchiviata = 4
        isBloccata = 5
    End Enum
    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

#Region "FORM PERMESSI"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

#Region "Filtri"
    Protected WithEvents TBCletters As System.Web.UI.WebControls.TableCell
    Protected WithEvents TBLfiltroNew As System.Web.UI.WebControls.Table
    Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton

    Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table
    Protected WithEvents LBtipoComunita_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBnumeroRecord_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoRicerca_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBvalore_c As System.Web.UI.WebControls.Label

    Protected WithEvents LBstatoComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBLstatoComunita As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents DDLTipo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList

    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button

    Protected WithEvents LBnoCorsi As System.Web.UI.WebControls.Label
    Protected WithEvents TBRorgnCorsi As System.Web.UI.WebControls.TableRow

    Protected WithEvents TBLcorsi As System.Web.UI.WebControls.Table
    Protected WithEvents TBCorganizzazione0 As System.Web.UI.WebControls.TableCell
    Protected WithEvents TBCorganizzazione1 As System.Web.UI.WebControls.TableCell

    Protected WithEvents LBorganizzazione_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBannoAccademico_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLannoAccademico As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLperiodo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBperiodo_c As System.Web.UI.WebControls.Label


    Protected WithEvents TBLcorsiDiStudio As System.Web.UI.WebControls.Table
    Protected WithEvents LBcorsoDiStudi_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoCorsoDiStudi As System.Web.UI.WebControls.DropDownList

    Protected WithEvents PNLData As System.Web.UI.WebControls.Panel

    Protected WithEvents CBXautoUpdate As System.Web.UI.WebControls.CheckBox
#Region "Lettere"
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

#Region "Filtri automatici"
    Protected WithEvents HDNselezionato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroFacolta As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoRicerca As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroValore As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroResponsabileID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroLaureaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoCdl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroAnno As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroPeriodo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoComunitaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroRicercaByIscrizione As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroStatus As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region
    Protected WithEvents DDLresponsabile As System.Web.UI.WebControls.DropDownList
#End Region

#Region "Griglia"
    Protected WithEvents DGComunita As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PNLdettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents CBXmostraPadre As System.Web.UI.WebControls.CheckBox


    Protected WithEvents LBcomunitaNome As System.Web.UI.WebControls.Label
    Protected WithEvents LNBlogin As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBdettagli As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBseparatorNews As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBnews As System.Web.UI.WebControls.LinkButton
    Protected WithEvents IMBnews As System.Web.UI.WebControls.Image
#End Region

    

    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBalbero As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBalberoGerarchico As System.Web.UI.WebControls.LinkButton

    Protected WithEvents LBMessaggi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLmessaggi As System.Web.UI.WebControls.Panel

    Protected WithEvents PNLmenuAccesso As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannulla As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLmenuDeiscrivi As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaDeIscrizione As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBdeIscriviCorrente As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBdeIscriviSelezionate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBdeIscriviDaTutte As System.Web.UI.WebControls.LinkButton

    Protected WithEvents RDTcomunita As Telerik.WebControls.RadTreeView

#Region "Form Dettagli"

    Protected WithEvents PNLmenuDettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaDettagli As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBentra As System.Web.UI.WebControls.LinkButton

    Protected WithEvents LBlegend As System.Web.UI.WebControls.Label
    Protected WithEvents LBmsgDG As System.Web.UI.WebControls.Label
    Protected WithEvents CTRLDettagli As Comunita_OnLine.UC_DettagliComunita
    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcmnt_Path As System.Web.UI.HtmlControls.HtmlInputHidden

#End Region

#Region "### BANNER ###"
    Protected WithEvents DVBanner As System.Web.UI.HtmlControls.HtmlContainerControl
    Protected WithEvents LBLBanner As System.Web.UI.WebControls.Label
    'Protected WithEvents LNBGoQuestion As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents LNBCloseBanner As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBLBannerLink As System.Web.UI.WebControls.Label
    Protected WithEvents HDNChecked As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HREFRedirect As System.Web.UI.HtmlControls.HtmlAnchor
#End Region

#Region "Pannello DeIscrivi"
    Protected WithEvents PNLdeiscrivi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBinfoDeIscrivi As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents HDNazione As System.Web.UI.HtmlControls.HtmlInputHidden

#Region " Codice generato da Progettazione Web Form "

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim SetupInfo As Boolean = True
        Dim ExitSub As Boolean = False
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        '--Per attivare le "bandierine" delle lingue
        'Me.Intestazione.ShowLanguage = True
        Me.Master.ShowHeaderLanguageChanger = True
        '--

        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        If Not IsPostBack Then
    Dim oServizioElencaComunita As New Services_ElencaComunita
            oServizioElencaComunita = Me.ImpostaPermessi

            Session("azione") = "load"
            Session("AdminForChange") = False
            Session("idComunita_forAdmin") = ""
            Session("CMNT_path_forAdmin") = ""
            Session("CMNT_path_forNews") = ""
            Session("CMNT_ID_forNews") = ""




    'Recupero impostazioni utente
    'Dim oImpostazioni As New COL_ImpostazioniUtente
    'Try
    '    If IsNothing(Session("oImpostazioni")) Then
    '        Me.ChangePage(SetupInfo, ExitSub)
    '    Else
    '        Try
    '            oImpostazioni = Session("oImpostazioni")

    '            If Me.Request.QueryString("forChange") <> "true" Then
    '                If oImpostazioni.Visualizza_Iscritto = 1 Then
    '                    SetupInfo = False
    '                    Me. ## Response.Redirect("./NavigazioneTreeView.aspx?show=1", True)
    '                    ExitSub = True
    '                ElseIf oImpostazioni.Visualizza_Iscritto Then
    '                    SetupInfo = False
    '                    Me. ## Response.Redirect("./NavigazioneTreeView.aspx?show=2", True)
    '                    ExitSub = True
    '                End If
    '            End If
    '        Catch ex As Exception
    '            Me.ChangePage(SetupInfo, ExitSub)
    '        End Try
    '    End If
    'Catch ex As Exception
    '    Me.ChangePage(SetupInfo, ExitSub)
    'End Try

            If ExitSub Then
                Exit Sub
            End If

            If SetupInfo Then
                Try
                    Me.SetupInternazionalizzazione()
                    If oServizioElencaComunita.List Or oServizioElencaComunita.Admin Then
                        Me.PNLmenu.Visible = True
                        If Me.Request.QueryString("re_set") <> "true" Then
                            Me.ViewState("intCurPage") = 0
                            Me.ViewState("SortExspression") = "RLPC_UltimoCollegamento"
                            Me.ViewState("SortDirection") = "desc"
                            Me.ViewState("intAnagrafica") = CType(Main.FiltroComunita.tutti, Main.FiltroComunita)
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.TBRapriFiltro.Visible = False
                            Me.TBRchiudiFiltro.Visible = True
                            Me.TBRfiltri.Visible = True
                        End If
                        Me.SetupFiltri()
                        Me.PNLpermessi.Visible = False
                        Me.PNLData.Visible = True
                    Else
                        Me.PNLmenu.Visible = False
                        Me.PNLpermessi.Visible = True
                        Me.PNLData.Visible = False
                    End If
                Catch ex As Exception

                End Try
            End If

			Me.PageUtility.AddAction(IIf(Me.PNLpermessi.Visible, ActionType.NoPermission, ActionType.List), Nothing, InteractionType.Generic)
		End If
	End Sub


    Private Function ImpostaPermessi() As Services_ElencaComunita
        Dim ComunitaID As Integer = 0
        Dim ForAdmin As Boolean = False
        Dim iResponse As String = "00000000000000000000000000000000"
        Dim oServizioElencaComunita As New Services_ElencaComunita
        Dim oPersona As New COL_Persona

        Try
            oPersona = Session("objPersona")
            ComunitaID = Session("idComunita")
        Catch ex As Exception
            ComunitaID = 0
        End Try
        Try
            If ComunitaID = 0 Then
                Session("Limbo") = True
                If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Guest Then
                    iResponse = "00000000000000000000000000000000"
                Else
                    oServizioElencaComunita.Admin = False
                    oServizioElencaComunita.List = True
                    Return oServizioElencaComunita
                End If
            Else
                iResponse = Permessi(Services_ElencaComunita.Codex, Me.Page)
            End If

            If (iResponse = "") Then
                iResponse = "00000000000000000000000000000000"
            End If
        Catch ex As Exception
            iResponse = "00000000000000000000000000000000"
        End Try
        oServizioElencaComunita.PermessiAssociati = iResponse
        Return oServizioElencaComunita
    End Function
    Private Function SessioneScaduta() As Boolean
        Dim oPersona As COL_Persona
        Dim isScaduta As Boolean = True
        Try
            oPersona = Session("objPersona")
            If oPersona.ID > 0 Then
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
          
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & Me.PageUtility.GetDefaultLogoutPage & "');</script>")
            Return True
        End If
    End Function


    Private Sub ChangePage(ByRef SetupInfo As Boolean, ByRef exitsub As Boolean)
        Try
            If Me.Request.Cookies("EntrataComunita")("RBLvisualizza") = 1 Then
                SetupInfo = False
                Me.PageUtility.RedirectToUrl("Comunita/NavigazioneTreeView.aspx?show=1")
                exitsub = True
            ElseIf Me.Request.Cookies("EntrataComunita")("RBLvisualizza") = 2 Then
                SetupInfo = False
                Me.PageUtility.RedirectToUrl("Comunita/NavigazioneTreeView.aspx?show=2")
                exitsub = True
            End If
        Catch ex As Exception

        End Try
    End Sub

#Region "Bind_Dati"
    Private Sub ChangeNumeroRecord(ByVal num As Integer)
        Try
            Me.DDLNumeroRecord.SelectedValue = num

        Catch ex As Exception
            Dim i, totale As Integer
            totale = Me.DDLNumeroRecord.Items.Count - 1
            For i = 0 To totale
                If Me.DDLNumeroRecord.Items(0).Value <= num Then
                    Me.DDLNumeroRecord.SelectedIndex = -1
                    Me.DDLNumeroRecord.Items(0).Selected = True
                Else
                    Exit For
                End If
            Next
        End Try

    End Sub
    Private Sub ChangeTipoComunita()
        Dim showFiltroCorso As Boolean = True ' False

        'If Session("limbo") = True Then
        '    showFiltroCorso = True
        'Else
        '    Try
        '        Dim oComunita As New COL_Comunita
        '        oComunita.Id = Session("IDComunita")
        '        oComunita.Estrai()
        '        If oComunita.Livello = 0 Or oComunita.Livello = 1 Then
        '            showFiltroCorso = True
        '        End If
        '    Catch ex As Exception

        '    End Try
        'End If

        Me.TBLcorsi.Visible = False
        Me.TBLcorsiDiStudio.Visible = False
        Me.LBnoCorsi.Visible = False

       
            Me.LBnoCorsi.Visible = True
            Me.DDLtipoCorsoDiStudi.SelectedIndex = 0
            Me.DDLannoAccademico.SelectedIndex = 0
            Me.DDLperiodo.SelectedIndex = 0
            If Me.DDLorganizzazione.Items.Count > 1 Then
                Me.TBRorgnCorsi.Visible = True
            Else
                Me.TBRorgnCorsi.Visible = False
            End If

    End Sub
    Private Sub SetupFiltri()
        Me.Bind_StatusComunità()
        Me.Bind_TipiComunita()
        Me.Bind_Organizzazioni()


        If Me.Request.QueryString("re_set") = "true" Then
            Try
                Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("EntrataComunita")("DDLorganizzazione")
            Catch ex As Exception
                Me.Response.Cookies("EntrataComunita")("DDLorganizzazione") = Me.DDLorganizzazione.SelectedValue
            End Try

            Me.SetupSearchParameters()
        ElseIf Me.Request.QueryString("forChange") <> "true" Then
            Dim oImpostazioni As New COL_ImpostazioniUtente
            Try
                oImpostazioni = Session("oImpostazioni")
                If Not IsNothing(oImpostazioni) Then
                    Try
                        Me.DDLorganizzazione.SelectedValue = oImpostazioni.Organizzazione_Iscritto
                    Catch ex As Exception

                    End Try

                    Try
                        Me.DDLTipo.SelectedValue = oImpostazioni.TipoComunita_Iscritto
                    Catch ex As Exception

                    End Try

                    Try
                        Me.ChangeNumeroRecord(oImpostazioni.Nrecord_Iscritto)
                    Catch ex As Exception

                    End Try
                End If
                Me.SaveSearchParameters(3)
            Catch ex As Exception
            End Try

        End If
        If Me.Request.QueryString("re_set") <> "true" Then

            If Me.RBLstatoComunita.Items.Count > 1 Then
                Me.RBLstatoComunita.SelectedIndex = 1
            Else
                Me.RBLstatoComunita.SelectedIndex = 0
            End If

            If Not Session("limbo") = True Then
                Dim oComunita As New COL_Comunita
                oComunita.Id = Session("idComunita")
                If oComunita.isBloccata() Then
                    Try
                        Me.RBLstatoComunita.SelectedValue = 2
                    Catch ex As Exception

                    End Try
                ElseIf oComunita.isArchiviata() Then
                    Try
                        Me.RBLstatoComunita.SelectedValue = 1
                    Catch ex As Exception

                    End Try
                End If
            End If
        End If
        Dim oFiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.iscritto
        Me.Bind_Responsabili(, oFiltroIscrizione)

        Me.DDLannoAccademico.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLperiodo.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLtipoCorsoDiStudi.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.RBLstatoComunita.AutoPostBack = Me.CBXautoUpdate.Checked

        Me.DDLTipoRicerca.Attributes.Add("onchange", "return AggiornaForm();")
        Me.HDNselezionato.Value = Me.DDLTipoRicerca.SelectedValue
        Me.DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value

        Me.Bind_Griglia(True)
    End Sub

    Private Sub Bind_Responsabili(Optional ByVal DocenteID As Integer = -1, Optional ByVal FiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.nonIscritto)
        Dim oDataSet As New DataSet
        Dim FacoltaID As Integer = -1
        Dim ComunitaID As Integer = -1
        Try
            Dim TipoComuniaID As Integer = -1
            Dim TipoCdlID As Integer = -1
            Dim AnnoAcc As Integer = -1
            Dim PeriodoID As Integer = -1

            Me.DDLresponsabile.Items.Clear()
            Try
                FacoltaID = Me.DDLorganizzazione.SelectedValue
            Catch ex As Exception
                FacoltaID = -1
            End Try
            Try
                If Session("IdComunita") > 0 Then
                    ComunitaID = Session("IdComunita")
                End If
            Catch ex As Exception

            End Try
            Try
                '   If Me.CBXautoUpdate.Checked Then
                TipoComuniaID = Me.DDLTipo.SelectedValue
                'Else
                '    TipoComuniaID = Me.HDN_filtroTipoComunitaID.Value
                'End If
            Catch ex As Exception

            End Try

         
                oDataSet = COL_Comunita.RicercaResponsabili(FacoltaID, ComunitaID, Session("objPersona").id, TipoComuniaID, , , , , Me.RBLstatoComunita.SelectedValue, FiltroIscrizione)


                If oDataSet.Tables(0).Rows.Count > 0 Then
                    DDLresponsabile.DataSource = oDataSet
                    DDLresponsabile.DataTextField() = "Anagrafica"
                    DDLresponsabile.DataValueField() = "PRSN_ID"
                    DDLresponsabile.DataBind()

                    'aggiungo manualmente elemento che indica tutti i tipi di comunità
                    DDLresponsabile.Items.Insert(0, New ListItem("-- Tutti --", -1))
                End If
        Catch ex As Exception
            Me.DDLresponsabile.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try
        oResource.setDropDownList(Me.DDLresponsabile, -1)
        If DocenteID > 0 Then
            Try
                Me.DDLresponsabile.SelectedValue = DocenteID
            Catch ex As Exception

            End Try
        End If

        Try
            If Me.DDLTipoRicerca.SelectedValue = Main.FiltroComunita.IDresponsabile Then
                Me.DDLresponsabile.Visible = True
                Me.TXBValore.Visible = False
                Me.TXBValore.Text = ""
            Else
                Me.DDLresponsabile.Visible = False
                Me.TXBValore.Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_StatusComunità()
        Dim oPersona As New COL_Persona
        Dim totale, TotaleArchiviate, totaleBloccate As Integer
        Try
            Dim oListItem_Archiviate, oListItem_Bloccate, oListItem_Tutte As ListItem
            oPersona = Session("objPersona")
            oPersona.StatusComunitaIscritto(oPersona.ID, totale, TotaleArchiviate, totaleBloccate)

            oListItem_Archiviate = Me.RBLstatoComunita.Items.FindByValue(1)
            oListItem_Bloccate = Me.RBLstatoComunita.Items.FindByValue(2)
            oListItem_Tutte = Me.RBLstatoComunita.Items.FindByValue(-1)
            If totaleBloccate = 0 And TotaleArchiviate = 0 Then
                If Not IsNothing(oListItem_Tutte) Then
                    Me.RBLstatoComunita.Items.Remove(oListItem_Tutte)
                End If
            Else
                If IsNothing(oListItem_Tutte) Then
                    Me.RBLstatoComunita.Items.Insert(0, New ListItem("Tutte", -1))
                    oResource.setRadioButtonList(Me.RBLstatoComunita, -1)
                End If
            End If

            If totaleBloccate = 0 Then
                If Not IsNothing(oListItem_Bloccate) Then
                    Me.RBLstatoComunita.Items.Remove(oListItem_Bloccate)
                End If
            Else
                If IsNothing(oListItem_Bloccate) Then
                    If IsNothing(oListItem_Archiviate) Then
                        Me.RBLstatoComunita.Items.Insert(2, New ListItem("Bloccate", 2))
                    Else
                        Me.RBLstatoComunita.Items.Insert(3, New ListItem("Bloccate", 2))
                    End If
                    oResource.setRadioButtonList(Me.RBLstatoComunita, 2)
                End If
            End If

            If TotaleArchiviate = 0 Then
                If Not IsNothing(oListItem_Archiviate) Then
                    Me.RBLstatoComunita.Items.Remove(oListItem_Archiviate)
                End If
            Else
                If IsNothing(oListItem_Archiviate) Then
                    Me.RBLstatoComunita.Items.Insert(2, New ListItem("Archiviate", 1))
                    oResource.setRadioButtonList(Me.RBLstatoComunita, 1)
                End If
            End If
            If Me.RBLstatoComunita.Items(0).Value = -1 Then
                Me.RBLstatoComunita.SelectedIndex = 1
            Else
                Me.RBLstatoComunita.SelectedIndex = 0
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TipiComunita()
        '...nella ddl che mi farà da filtro delle tipologie di utenti associate al tipo comunità
        Dim oDataSet As New DataSet


        Try
            oDataSet = COL_Tipo_Comunita.ElencaForFiltri(Session("LinguaID"), True)
            If oDataSet.Tables(0).Rows.Count > 0 Then
                DDLTipo.DataSource = oDataSet
                DDLTipo.DataTextField() = "TPCM_descrizione"
                DDLTipo.DataValueField() = "TPCM_id"
                DDLTipo.DataBind()

                'aggiungo manualmente elemento che indica tutti i tipi di comunità
                DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
            End If
        Catch ex As Exception
            DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try
        oResource.setDropDownList(Me.DDLTipo, -1)
    End Sub

    Private Sub Bind_Organizzazioni()
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona

        Me.DDLorganizzazione.Items.Clear()
        Try
            oPersona = Session("objPersona")
            oDataset = oPersona.GetOrganizzazioniAssociate()

            If oDataset.Tables(0).Rows.Count > 0 Then
                Me.DDLorganizzazione.DataValueField = "ORGN_id"
                Me.DDLorganizzazione.DataTextField = "ORGN_ragioneSociale"
                Me.DDLorganizzazione.DataSource = oDataset
                Me.DDLorganizzazione.DataBind()

                If Me.DDLorganizzazione.Items.Count > 1 Then
                    Me.DDLorganizzazione.Enabled = True
                    Me.DDLorganizzazione.Items.Insert(0, New ListItem("< tutte >", -1))
                    'If IsNothing(Session("ORGN_id")) = False Then
                    '    Try
                    '        Me.DDLorganizzazione.SelectedValue = Session("ORGN_id")
                    '    Catch ex As Exception
                    '        Me.DDLorganizzazione.SelectedIndex = 0
                    '    End Try
                    'Else
                    '    Me.DDLorganizzazione.SelectedIndex = 0
                    'End If
                    ' LE MARMOTTE VOGLIONO VEDERE TUTTO !!!!
                    Me.DDLorganizzazione.SelectedIndex = 0
                    Me.TBRorgnCorsi.Visible = True
                Else
                    Me.DDLorganizzazione.Enabled = False
                    Me.TBRorgnCorsi.Visible = False
                End If
            Else
                Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", -1))
                Me.DDLorganizzazione.Enabled = False
                Me.TBRorgnCorsi.Visible = False
            End If
        Catch ex As Exception
            Me.DDLorganizzazione.Items.Clear()
            Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", -1))
            Me.DDLorganizzazione.Enabled = False
            Me.TBRorgnCorsi.Visible = False
        End Try
        oResource.setDropDownList(Me.DDLorganizzazione, -1)
    End Sub

    Private Function FiltraggioDati(ByVal Applicafiltri As Boolean) As DataSet
        Dim oTreeComunita As New COL_TreeComunita
        Dim oPersona As New COL_Persona
        Dim oDataset As New DataSet
        Dim ArrComunita(,) As String

        Try
            Dim oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti
            Dim oFiltroLettera As Main.FiltroComunita = Main.FiltroComunita.tutti
            Dim valore As String = ""

            oPersona = Session("objPersona")
            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & oPersona.ID & "\"
            oTreeComunita.Nome = oPersona.ID & ".xml"
            oPersona = Session("objPersona")
            Try
                If IsNumeric(Me.ViewState("intAnagrafica")) Then
                    oFiltroLettera = CType(Me.ViewState("intAnagrafica"), Main.FiltroComunita)
                Else
                    oFiltroLettera = Main.FiltroComunita.tutti
                    Me.SelezionaLink_All()
                End If
            Catch ex As Exception
                oFiltroLettera = Main.FiltroComunita.tutti
                Me.SelezionaLink_All()
            End Try

            If Me.CBXautoUpdate.Checked Or Applicafiltri = True Then
                If Me.TXBValore.Text <> "" Then
                    Me.TXBValore.Text = Trim(Me.TXBValore.Text)
                End If
                valore = Me.TXBValore.Text
            Else
                Try
                    valore = Trim(Me.HDN_filtroValore.Value)
                Catch ex As Exception

                End Try
            End If

            Dim TipoRicercaID As Integer
            If Me.CBXautoUpdate.Checked Or Applicafiltri = True Then
                TipoRicercaID = Me.DDLTipoRicerca.SelectedValue
            Else
                Try
                    TipoRicercaID = Me.HDN_filtroTipoRicerca.Value
                Catch ex As Exception
                    TipoRicercaID = -1
                End Try
            End If
            If valore <> "" Or (Me.CBXautoUpdate.Checked And Me.DDLresponsabile.Visible) Or (Not Me.CBXautoUpdate.Checked And TipoRicercaID = Main.FiltroComunita.IDresponsabile) Then
                Select Case TipoRicercaID
                    Case Main.FiltroComunita.nome
                        oFiltroTipoRicerca = Main.FiltroComunita.nome
                    Case Main.FiltroComunita.creataDopo
                        If IsDate(valore) = False Then
                            valore = ""
                        Else
                            oFiltroTipoRicerca = Main.FiltroComunita.creataDopo
                        End If
                    Case Main.FiltroComunita.creataPrima
                        If IsDate(valore) = False Then
                            valore = ""
                        Else
                            oFiltroTipoRicerca = Main.FiltroComunita.creataPrima
                        End If
                    Case Main.FiltroComunita.dataIscrizioneDopo
                        If IsDate(valore) = False Then
                            valore = ""
                        Else
                            oFiltroTipoRicerca = Main.FiltroComunita.dataIscrizioneDopo
                        End If
                    Case Main.FiltroComunita.dataFineIscrizionePrima
                        If IsDate(valore) = False Then
                            valore = ""
                        Else
                            oFiltroTipoRicerca = Main.FiltroComunita.dataFineIscrizionePrima
                        End If
                    Case Main.FiltroComunita.contiene
                        oFiltroTipoRicerca = Main.FiltroComunita.contiene
                    Case Main.FiltroComunita.cognomeDocente
                        oFiltroTipoRicerca = Main.FiltroComunita.cognomeDocente
                    Case Main.FiltroComunita.IDresponsabile
                        Try
                            If Me.CBXautoUpdate.Checked Then
                                valore = Me.DDLresponsabile.SelectedValue
                            Else
                                valore = Me.HDN_filtroResponsabileID.Value
                            End If

                            oFiltroTipoRicerca = Main.FiltroComunita.IDresponsabile
                        Catch ex As Exception
                            valore = -1
                        End Try
                    Case Else
                        valore = ""
                End Select
            End If
            If (Me.CBXautoUpdate.Checked Or Applicafiltri = True) And valore = "" Then
                Me.TXBValore.Text = valore
                Me.HDN_filtroValore.Value = ""
            End If

            Dim ComunitaPadreID As Integer
            Try
                ComunitaPadreID = Session("idComunita")
                If ComunitaPadreID < 1 Then
                    ComunitaPadreID = -1
                End If
            Catch ex As Exception
                ComunitaPadreID = -1
            End Try


            Dim FacoltaID, PeriodoID, AAid, TipocomunitaID, TipoCdlID, StatusID As Integer
            If Me.CBXautoUpdate.Checked Or Applicafiltri = True Then
                Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
                Me.HDN_filtroValore.Value = Me.TXBValore.Text
                Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue
                Me.HDN_filtroTipoCdl.Value = Me.DDLtipoCorsoDiStudi.SelectedValue
                Me.HDN_filtroAnno.Value = Me.DDLannoAccademico.SelectedValue
                Me.HDN_filtroPeriodo.Value = Me.DDLperiodo.SelectedValue
                Me.HDN_filtroTipoComunitaID.Value = Me.DDLTipo.SelectedValue
                Me.HDN_filtroFacolta.Value = Me.DDLorganizzazione.SelectedValue
                Me.HDN_filtroStatus.Value = Me.RBLstatoComunita.SelectedValue
            End If
            Try
                FacoltaID = Me.HDN_filtroFacolta.Value
            Catch ex As Exception
                FacoltaID = -1
            End Try
            Try
                AAid = Me.HDN_filtroAnno.Value
            Catch ex As Exception
                AAid = -1
            End Try
            Try
                PeriodoID = Me.HDN_filtroPeriodo.Value
            Catch ex As Exception
                PeriodoID = -1
            End Try
            Try
                TipocomunitaID = Me.HDN_filtroTipoComunitaID.Value
            Catch ex As Exception
                TipocomunitaID = -1
            End Try
            Try
                TipoCdlID = Me.HDN_filtroTipoCdl.Value
            Catch ex As Exception
                TipoCdlID = -1
            End Try
            Try
                StatusID = Me.HDN_filtroStatus.Value
            Catch ex As Exception

            End Try

            Dim ComunitaPath As String = ""
            If IsArray(Session("ArrComunita")) Then
                Try
                    ArrComunita = Session("ArrComunita")
                    ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
                Catch ex As Exception

                End Try
            End If
            If valore <> "" Then
                valore = Replace(valore, "'", "''")
            End If
            Dim ImageBaseDir As String
            ImageBaseDir = GetPercorsoApplicazione(Me.Request)
            ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"
            ImageBaseDir = Replace(ImageBaseDir, "//", "/")

            oDataset = oTreeComunita.RicercaComunita(oPersona, ComunitaPadreID, Me.oResource, ImageBaseDir, TipocomunitaID, FacoltaID, , , , ComunitaPadreID, ComunitaPath, oFiltroTipoRicerca, oFiltroLettera, valore, StatusID, True, Me.CBXmostraPadre.Checked)

        Catch ex As Exception

        End Try
        Return oDataset
    End Function
    Private Sub Bind_Griglia(Optional ByVal Applicafiltri As Boolean = False)
        Dim ORGN_ID As Integer
        Dim oPersona As New COL_Persona
        Dim oDataset As DataSet
        Dim totale As Integer
        Dim ImageBaseDir As String
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"
        ImageBaseDir = Replace(ImageBaseDir, "//", "/")

        Me.LBmsgDG.Visible = False
        Me.DGComunita.Visible = True

        Try
            Dim oTreeComunita As New COL_TreeComunita

            oPersona = Session("objPersona")
            oDataset = Me.FiltraggioDati(Applicafiltri)

            totale = oDataset.Tables(0).Rows.Count
            Me.CBXmostraPadre.Enabled = False
            If totale = 0 Then 'se datagrid vuota
                Me.DGComunita.Visible = False
                Me.LBmsgDG.Visible = True
                Me.LNBalbero.Visible = False
                Me.LNBalberoGerarchico.Visible = False
                Me.LBnumeroRecord_c.Visible = False
                Me.DDLNumeroRecord.Visible = False
            Else
                Me.LNBalbero.Visible = True
                Me.LNBalberoGerarchico.Visible = True
                totale = oDataset.Tables(0).Rows.Count

                oResource.setHeaderDatagrid(Me.DGComunita, 0, "TPCM_Descrizione", True)
                oResource.setHeaderDatagrid(Me.DGComunita, 1, "CMNT_Nome", True)
                oResource.setHeaderDatagrid(Me.DGComunita, 3, "AnnoAccademico", True)
                oResource.setHeaderDatagrid(Me.DGComunita, 4, "Periodo", True)
                oResource.setHeaderDatagrid(Me.DGComunita, 5, "TPRL_nome", True)
                oResource.setHeaderDatagrid(Me.DGComunita, 8, "RLPC_UltimoCollegamento", True)

                If totale > 0 Then
                    Me.LBnumeroRecord_c.Visible = True
                    Me.DDLNumeroRecord.Visible = True

                    Me.CBXmostraPadre.Enabled = True
                    Me.DGComunita.Columns(3).Visible = False
                    Me.DGComunita.Columns(4).Visible = False

                    If Me.TBLcorsi.Visible = True Then
                        If Me.DDLannoAccademico.SelectedValue < 1 Then
                            Me.DGComunita.Columns(3).Visible = True
                        End If
                        If Me.DDLperiodo.SelectedValue < 1 Then
                            Me.DGComunita.Columns(4).Visible = True
                        End If
                        Me.DGComunita.Columns(5).Visible = False
                    Else
                        Me.DGComunita.Columns(5).Visible = True
                    End If

                    Me.OrganizzazioneDefaultID = oPersona.GetOrganizzazioneDefault()
                    'cicla gli elementi del dataset e prepara i dati per la successiva visualizzazione nella datagrid


                    'For i = 0 To totale - 1
                    '    Dim oRow As DataRow

                    '    oRow = oDataset.Tables(0).Rows(i)
                    'If oRow.Item("CMNT_ORGN_ID") = ORGN_ID Then
                    '    oRow.Item("ORGN_Diretta") = True
                    'Else
                    '    oRow.Item("ORGN_Diretta") = False
                    'End If



                    'If Session("limbo") = False Then
                    '    If Page.IsPostBack = False Then
                    '        Dim oComunita As New COL_Comunita
                    '        Dim oRuolo As New COL_RuoloPersonaComunita
                    '        Dim CMNT_Responsabile, CMNT_Creatore As String

                    '        If Not IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                    '            CMNT_Responsabile = oRow.Item("CMNT_Responsabile")
                    '        Else
                    '            CMNT_Responsabile = "n.d."
                    '        End If

                    '        If Not IsDBNull(oRow.Item("AnagraficaCreatore")) Then
                    '            CMNT_Creatore = oRow.Item("AnagraficaCreatore")
                    '        Else
                    '            CMNT_Creatore = "n.d."
                    '        End If
                    '        ' aggiorno l'XML solo la prima volta !!!!!!!!

                    '        Try
                    '            oComunita.Id = oRow.Item("CMNT_ID")
                    '            oComunita.IdPadre = oRow.Item("CMNT_idPadre")
                    '            oComunita.Nome = oRow.Item("CMNT_nome")
                    '            oComunita.IsChiusa = CBool(oRow.Item("CMNT_IsChiusa"))
                    '            oComunita.TipoComunita.ID = oRow.Item("CMNT_TPCM_ID")
                    '            oComunita.TipoComunita.Icona = oRow.Item("TPCM_icona")
                    '            oComunita.TipoComunita.Descrizione = oRow.Item("TPCM_Descrizione")
                    '            oComunita.Organizzazione.Id = oRow.Item("CMNT_ORGN_id")
                    '            oRuolo.TipoRuolo.Id = oRow.Item("RLPC_TPRL_id")
                    '            oRuolo.TipoRuolo.Nome = oRow.Item("TPRL_nome")
                    '            oRuolo.Abilitato = CBool(oRow.Item("RLPC_abilitato"))
                    '            oRuolo.Attivato = CBool(oRow.Item("RLPC_attivato"))
                    '            If IsDate(oRow.Item("RLPC_UltimoCollegamento")) Then
                    '                If Equals(New Date, oRow.Item("RLPC_UltimoCollegamento")) Then
                    '                    oRuolo.UltimoCollegamento = Nothing
                    '                Else
                    '                    oRuolo.UltimoCollegamento = oRow.Item("RLPC_UltimoCollegamento")
                    '                End If
                    '            Else
                    '                oRuolo.UltimoCollegamento = Nothing
                    '            End If

                    '            oTreeComunita.Update(oComunita, oRow.Item("CMNT_Path"), CMNT_Responsabile, CMNT_Creatore, oRuolo)
                    '        Catch ex As Exception

                    '        End Try

                    '    End If
                    'End If

                    'Trovo il nome del padre del figlio..
                    ' Ha senso solo se decido di mostrare il padre....
                    'If Me.CBXmostraPadre.Checked Then
                    '    Dim oDataviewPadre As DataView
                    '    oDataviewPadre = oDataset.Tables(0).DefaultView

                    '    Dim CMNT_Path, CMNT_Padri() As String
                    '    Dim CMNT_IDPadre, CMNT_ID As Integer

                    '    CMNT_Path = oRow.Item("CMNT_Path")
                    '    CMNT_ID = oRow.Item("CMNT_ID")
                    '    If CMNT_Path <> "" Then
                    '        CMNT_Path = CMNT_Path.Remove(CMNT_Path.Length - 1, 1)
                    '        CMNT_Padri = CMNT_Path.Split(".")
                    '        If UBound(CMNT_Padri) >= 2 Then
                    '            CMNT_IDPadre = CMNT_Padri(UBound(CMNT_Padri) - 1)
                    '            If CMNT_IDPadre <> Session("IdComunita") Then
                    '                oDataviewPadre.RowFilter = "CMNT_ID=" & CMNT_IDPadre
                    '                If oDataviewPadre.Count > 0 Then
                    '                    oRow.Item("CMNT_NomePadre") = oDataviewPadre.Item(0).Item("CMNT_nome") & "> "
                    '                Else
                    '                    Try
                    '                        Dim oComunita As COL_Comunita
                    '                        oRow.Item("CMNT_NomePadre") = oComunita.EstraiNome(CMNT_IDPadre) & "> "
                    '                    Catch ex As Exception
                    '                        oRow.Item("CMNT_NomePadre") = "> "
                    '                    End Try
                    '                End If

                    '                'AGGIUNTO
                    '                oDataviewPadre.RowFilter = ""

                    '                Dim PathToRemove As String
                    '                PathToRemove = "." & CMNT_IDPadre & "." & CMNT_ID & "."
                    '                oDataviewPadre.RowFilter = "CMNT_PATH like '%" & PathToRemove & "' and  CMNT_PATH <> '" & CMNT_Path & ".'"
                    '                If oDataviewPadre.Count > 0 Then
                    '                    Dim j, totRemove As Integer
                    '                    While oDataviewPadre.Count > 0
                    '                        oDataviewPadre.Item(0).Row.Delete()
                    '                    End While
                    '                End If


                    '                oDataviewPadre.RowFilter = ""
                    '                totale = oDataset.Tables(0).Rows.Count

                    '            Else
                    '                oRow.Item("CMNT_NomePadre") = ""
                    '            End If

                    '            oDataviewPadre.RowFilter = ""
                    '        Else
                    '            oRow.Item("CMNT_NomePadre") = ""
                    '        End If
                    '    Else
                    '        oRow.Item("CMNT_NomePadre") = ""
                    '    End If
                    '    oRow.Item("CMNT_Esteso") = "<span class=small_Padre>" & oRow.Item("CMNT_NomePadre") & "</span><span class=DG_nomecomunita>" & oRow.Item("CMNT_Nome") & "</span>"
                    '    oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_NomePadre") & oRow.Item("CMNT_Nome")
                    'Else
                    '    oRow.Item("CMNT_Esteso") = "<span class=DG_nomecomunita>" & oRow.Item("CMNT_Nome") & "</span>"
                    '    oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")
                    'End If

                    '    If i >= totale - 1 Then
                    '        Exit For
                    '    End If
                    'Next

                    Dim oDataview As DataView
                    oDataview = oDataset.Tables(0).DefaultView

                    oDataview.RowFilter = ""
                    If ViewState("SortExspression") = "" Then
                        ViewState("SortExspression") = "RLPC_UltimoCollegamento"
                        ViewState("SortDirection") = "desc"
                    End If
                    oDataview.Sort = ViewState("SortExspression") & " " & ViewState("SortDirection")
                    totale = oDataview.Count

                    If totale < Me.DDLNumeroRecord.Items(0).Value Then
                        Me.DGComunita.PagerStyle.Position = PagerPosition.Top
                    Else
                        Me.DGComunita.PagerStyle.Position = PagerPosition.TopAndBottom
                    End If
                    DGComunita.DataSource = oDataview
                    DGComunita.DataBind()
                Else
                    Me.DGComunita.Visible = False
                    Me.LBmsgDG.Visible = True
                End If
            End If
        Catch ex As Exception
            Me.DGComunita.Visible = False
            Me.LBmsgDG.Visible = True
        End Try
    End Sub

#End Region

#Region "Filtro"
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

    Private Sub CBXautoUpdate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXautoUpdate.CheckedChanged
        Me.DDLannoAccademico.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLtipoCorsoDiStudi.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLperiodo.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.RBLstatoComunita.AutoPostBack = Me.CBXautoUpdate.Checked

        Me.Bind_Griglia(True)
    End Sub
    Private Sub DDLresponsabile_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLresponsabile.SelectedIndexChanged
        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0

            Me.Bind_Griglia(True)
        End If
    End Sub
    Private Sub DDLTipoRicerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoRicerca.SelectedIndexChanged
        If Me.DDLTipoRicerca.SelectedValue = Main.FiltroComunita.IDresponsabile Then
            Me.DDLresponsabile.Visible = True
            Me.TXBValore.Text = ""
            Me.TXBValore.Visible = False
        Else
            Me.DDLresponsabile.Visible = False
            Me.TXBValore.Visible = True
        End If
        If Me.CBXautoUpdate.Checked Then

        End If
        Me.Bind_Griglia()
    End Sub
    Private Sub DDLTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLTipo.SelectedIndexChanged
       
            Me.TBLcorsi.Visible = False
            Me.TBLcorsiDiStudio.Visible = False
            Me.LBnoCorsi.Visible = True
            Me.DDLtipoCorsoDiStudi.SelectedIndex = 0
            Me.DDLannoAccademico.SelectedIndex = 0
            Me.DDLperiodo.SelectedIndex = 0

            If Me.DDLorganizzazione.Items.Count > 1 Then
                Me.TBRorgnCorsi.Visible = True
            End If
      
        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0

        End If
        Dim oFiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.iscritto
        Me.Bind_Responsabili(, oFiltroIscrizione)
        Me.Bind_Griglia()
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.ViewState("intCurPage") = 0
        Me.DDLNumeroRecord.SelectedIndex = Me.DDLNumeroRecord.SelectedIndex
        DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        DGComunita.CurrentPageIndex = 0
        Bind_Griglia(True)
    End Sub
    Private Sub DDLannoAccademico_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLannoAccademico.SelectedIndexChanged
        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0

            Me.Bind_Griglia(True)
        End If
    End Sub
    Private Sub DDLperiodo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLperiodo.SelectedIndexChanged
        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0

            Me.Bind_Griglia(True)
        End If
    End Sub
    Private Sub DDLtipoCorsoDiStudi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLtipoCorsoDiStudi.SelectedIndexChanged
        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0

            Me.Bind_Griglia(True)
        End If
    End Sub
    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
        Me.Bind_Responsabili(, Main.FiltroRicercaComunitaByIscrizione.iscritto)
        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0
            Me.Bind_Griglia(True)
        Else
            Me.Bind_Griglia()
        End If
    End Sub
    Private Sub CBXmostraPadre_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXmostraPadre.CheckedChanged
        If Me.DDLNumeroRecord.SelectedValue <> Me.DGComunita.PageSize Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0
        End If
        Me.Bind_Griglia()
    End Sub

    Private Sub RBLstatoComunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLstatoComunita.SelectedIndexChanged

        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.Bind_Griglia(True)
        End If
    End Sub
    Private Sub BTNCerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Me.DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        Me.DGComunita.CurrentPageIndex = 0
        Me.Bind_Griglia(True)
    End Sub
    Private Sub DeselezionaLink(ByVal selected As String)
        Dim lettera As String = CType(CInt(selected), Main.FiltroAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = TBCletters.FindControl("LKB" & lettera)
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
        Me.DGComunita.CurrentPageIndex = 0
        Me.Bind_Griglia()
    End Sub
    Private Sub SelezionaLink_All()
        Dim i As Integer
        Try
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControl("LKB" & Chr(i))
                Dim Carattere As String = Chr(i)
                If IsNothing(oLinkButton) = False Then
                    oLinkButton.CssClass = "lettera"
                End If
            Next
            Me.LKBaltro.CssClass = "lettera"
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Setup Parametri Ricerca"
    Private Sub SaveSearchParameters(ByVal Visualizza As Integer)
        Try
            Me.Response.Cookies("EntrataComunita")("DDLannoAccademico") = Me.DDLannoAccademico.SelectedValue
            Me.Response.Cookies("EntrataComunita")("DDLNumeroRecord") = Me.DDLNumeroRecord.SelectedValue
            Me.Response.Cookies("EntrataComunita")("DDLorganizzazione") = Me.DDLorganizzazione.SelectedValue
            Me.Response.Cookies("EntrataComunita")("DDLperiodo") = Me.DDLperiodo.SelectedValue
            Me.Response.Cookies("EntrataComunita")("DDLTipo") = Me.DDLTipo.SelectedValue
            Me.Response.Cookies("EntrataComunita")("DDLTipoRicerca") = Me.DDLTipoRicerca.SelectedValue
            Me.Response.Cookies("EntrataComunita")("TXBValore") = Me.TXBValore.Text
            Me.Response.Cookies("EntrataComunita")("intCurPage") = Me.ViewState("intCurPage")
            Me.Response.Cookies("EntrataComunita")("SortDirection") = Me.ViewState("SortDirection")
            Me.Response.Cookies("EntrataComunita")("SortExspression") = Me.ViewState("SortExspression")
            Me.Response.Cookies("EntrataComunita")("CBXmostraPadre") = Me.CBXmostraPadre.Checked
            Me.Response.Cookies("EntrataComunita")("DDLtipoCorsoDiStudi") = Me.DDLtipoCorsoDiStudi.SelectedValue
            Me.Response.Cookies("EntrataComunita")("RBLvisualizza") = Visualizza
            Me.Response.Cookies("EntrataComunita")("intAnagrafica") = Me.ViewState("intAnagrafica")
            Me.Response.Cookies("EntrataComunita")("TBRapriFiltro") = Me.TBRapriFiltro.Visible
            Me.Response.Cookies("EntrataComunita")("RBLstatoComunita") = Me.RBLstatoComunita.SelectedValue
        Catch ex As Exception

        End Try
    End Sub
    Private Sub SetupSearchParameters()
        Try
            'Recupero fattori di ricerca relativi all'ordinamento
            Try
                Me.ViewState("SortDirection") = Me.Request.Cookies("EntrataComunita")("SortDirection")
                Me.ViewState("SortExspression") = Me.Request.Cookies("EntrataComunita")("SortExspression")
            Catch ex As Exception

            End Try

            Try
                Me.TBRapriFiltro.Visible = Me.Request.Cookies("EntrataComunita")("TBRapriFiltro")
                Me.TBRfiltri.Visible = Not Me.TBRapriFiltro.Visible
                Me.TBRchiudiFiltro.Visible = Not Me.TBRapriFiltro.Visible
            Catch ex As Exception
                Me.TBRapriFiltro.Visible = False
                Me.TBRchiudiFiltro.Visible = True
                Me.TBRfiltri.Visible = True
            End Try

            Try
                Me.RBLstatoComunita.SelectedValue = Me.Request.Cookies("EntrataComunita")("RBLstatoComunita")
            Catch ex As Exception
                Me.RBLstatoComunita.SelectedIndex = 0
            End Try

            Try
                Me.ViewState("intAnagrafica") = Me.Request.Cookies("EntrataComunita")("intAnagrafica")
                If Me.ViewState("intAnagrafica") <> "" Then
                    Dim Lettera As String
                    Lettera = CType(CInt(Me.ViewState("intAnagrafica")), Main.FiltroAnagrafica).ToString

                    Dim oLink As System.Web.UI.WebControls.LinkButton
                    oLink = Me.FindControl("LKB" & Lettera)
                    If IsNothing(oLink) = False Then
                        oLink.CssClass = "lettera_Selezionata"
                    End If
                Else
                    Me.ViewState("intAnagrafica") = -1
                    Me.LKBtutti.CssClass = "lettera_Selezionata"
                End If
            Catch ex As Exception
                Me.ViewState("intAnagrafica") = -1
                Me.LKBtutti.CssClass = "lettera_Selezionata"
            End Try
            Try
                'Recupero dati relativi alla paginazione corrente
                If IsNumeric(Me.Request.Cookies("EntrataComunita")("intCurPage")) Then
                    Me.ViewState("intCurPage") = CInt(Me.Request.Cookies("EntrataComunita")("intCurPage"))
                    Me.DGComunita.CurrentPageIndex = CInt(Me.ViewState("intCurPage"))
                Else
                    Me.ViewState("intCurPage") = 0
                    Me.DGComunita.CurrentPageIndex = 0
                End If
            Catch ex As Exception
                Me.ViewState("intCurPage") = 0
                Me.DGComunita.CurrentPageIndex = 0
            End Try
            Try
                Me.TXBValore.Text = Me.Request.Cookies("EntrataComunita")("TXBValore")
            Catch ex As Exception
                Me.TXBValore.Text = ""
            End Try


            ' Setto l'anno accademico
            Try
                If IsNumeric(Me.Request.Cookies("EntrataComunita")("DDLannoAccademico")) Then
                    Try
                        Me.DDLannoAccademico.SelectedValue = Me.Request.Cookies("EntrataComunita")("DDLannoAccademico")
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try
            ' Setto il periodo
            Try
                If IsNumeric(Me.Request.Cookies("EntrataComunita")("DDLperiodo")) Then
                    Try
                        Me.DDLperiodo.SelectedValue = Me.Request.Cookies("EntrataComunita")("DDLperiodo")
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try
            ' Setto l'organizzazione
            Try
                If IsNumeric(Me.Request.Cookies("EntrataComunita")("DDLorganizzazione")) Then
                    Try
                        Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("EntrataComunita")("DDLorganizzazione")
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try

            ' Setto il numero di record
            Try
                If IsNumeric(Me.Request.Cookies("EntrataComunita")("DDLNumeroRecord")) Then
                    Me.DDLNumeroRecord.SelectedValue = Me.Request.Cookies("EntrataComunita")("DDLNumeroRecord")
                End If
            Catch ex As Exception

            End Try

            ' Setto il tipodi corso di studi
            Try
                If IsNumeric(Me.Request.Cookies("EntrataComunita")("DDLtipoCorsoDiStudi")) Then
                    Me.DDLtipoCorsoDiStudi.SelectedValue = Me.Request.Cookies("EntrataComunita")("DDLtipoCorsoDiStudi")
                End If
            Catch ex As Exception
                Me.DDLtipoCorsoDiStudi.SelectedIndex = 0
            End Try
            ' Setto il numero di record
            Me.TBLcorsi.Visible = False
            Me.TBLcorsiDiStudio.Visible = False
            Me.LBnoCorsi.Visible = True
            Try
                ' If IsNumeric(Me.Request.Cookies("ListaComunita")("tipo")) Then
                Me.DDLTipo.SelectedValue = Me.Request.Cookies("EntrataComunita")("DDLTipo")
            Catch ex As Exception

            End Try

            ' Setto il tipo di ricerca
            Try
                If IsNumeric(Me.Request.Cookies("EntrataComunita")("DDLTipoRicerca")) Then
                    Me.DDLTipoRicerca.SelectedValue = Me.Request.Cookies("EntrataComunita")("DDLTipoRicerca")
                End If
            Catch ex As Exception
            End Try

            Try
                Me.CBXmostraPadre.Checked = CBool(Me.Request.Cookies("EntrataComunita")("CBXmostraPadre"))
            Catch ex As Exception
                Me.CBXmostraPadre.Checked = False
            End Try
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Gestione Griglia"

    Private Sub DGComunita_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGComunita.ItemCreated
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
                    If Me.DGComunita.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGComunita, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGComunita, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGComunita.HeaderStyle.CssClass
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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGComunita, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGComunita.HeaderStyle.CssClass
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
                Dim num As Integer = 0
                oRow = oCell.Parent()

                oTableCell.Controls.Add(Me.CreaLegenda)
                If Me.DGComunita.Columns(3).Visible Then
                    num += 1
                End If
                If Me.DGComunita.Columns(4).Visible Then
                    num += 1
                End If
                If Me.DGComunita.Columns(5).Visible Then
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
                    oResource.setPageDatagrid(Me.DGComunita, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"
            Dim cssRiga As String = "ROW_TD_Small"
            Dim hasAccesso As Boolean = False
            Try
                If CBool(e.Item.DataItem("RLPC_Attivato")) = False Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                    cssRiga = "ROW_ItemDisattivate_Small"
                ElseIf CBool(e.Item.DataItem("RLPC_Abilitato")) = False Then
                    e.Item.CssClass = "ROW_Disabilitate_Small"
                ElseIf e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                    hasAccesso = True
                Else
                    e.Item.CssClass = "ROW_Normal_Small"
                    hasAccesso = True
                End If
            Catch ex As Exception
                If e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                Else
                End If
            End Try

            Try
                Dim oTBRnome As TableRow
                Dim oTBCchiusa, oTBCnome As TableCell

                oTBRnome = e.Item.Cells(1).FindControl("TBRnome")
                oTBCchiusa = e.Item.Cells(1).FindControl("TBCchiusa")
                oTBCnome = e.Item.Cells(1).FindControl("TBCnome")

                If IsNothing(oTBRnome) = False Then
                    oTBRnome.CssClass = cssRiga
                End If
                If IsNothing(oTBCchiusa) = False Then
                    oTBCchiusa.CssClass = cssRiga
                End If
                If IsNothing(oTBCnome) = False Then
                    oTBCnome.CssClass = cssRiga
                End If
            Catch ex As Exception

            End Try
            Try
                Dim oLBcomunitaNome As Label

                oLBcomunitaNome = e.Item.Cells(1).FindControl("LBcomunitaNome")
                If IsNothing(oLBcomunitaNome) = False Then
                    oLBcomunitaNome.CssClass = cssRiga
                End If
            Catch ex As Exception

            End Try


            Dim oLinkbutton As LinkButton
            Try
                Dim oCell As New TableCell
                Dim oWebControl As WebControl

                oCell = CType(e.Item.Cells(1), TableCell)

                'Sistemo la login
                Try
                    Dim CMNT_EstesoNoSpan As String
                    oLinkbutton = e.Item.Cells(1).FindControl("LNBlogin")

                    If IsNothing(oLinkbutton) = False Then
                        CMNT_EstesoNoSpan = e.Item.DataItem.Item("CMNT_EstesoNoSpan")


                        oResource.setLinkButton_DatagridToValue(Me.DGComunita, oLinkbutton, "LNBlogin", "#%%#", CMNT_EstesoNoSpan, True, True, , True)
                        Try
                            If e.Item.DataItem("CMNT_Bloccata") = True Then
                                oLinkbutton.Enabled = False
                                oResource.setLinkButton_DatagridToValue(Me.DGComunita, oLinkbutton, "LNBlogin", "#%%#", CMNT_EstesoNoSpan, True, True, False, True, True)
                            Else
                                oLinkbutton.Enabled = True
                                oResource.setLinkButton_DatagridToValue(Me.DGComunita, oLinkbutton, "LNBlogin", "#%%#", CMNT_EstesoNoSpan, True, True, , True)
                            End If
                        Catch ex As Exception

                        End Try
                        If Not hasAccesso Then
                            oLinkbutton.Enabled = False
                        End If
                        oLinkbutton.CssClass = cssLink
                    End If


                Catch ex As Exception

                End Try
                'Sistemo i dettagli
                Try
                    oLinkbutton = e.Item.Cells(1).FindControl("LNBdettagli")
                    If IsNothing(oLinkbutton) = False Then
                        oResource.setLinkButton(oLinkbutton, True, False)
                        oLinkbutton.CssClass = cssLink
                    End If
                Catch ex As Exception

                End Try
                'sistemo le news !
                Try
                    Dim oLabelNews As Label
                    Dim oImageButtonNews As ImageButton
                    Dim oLinkNews As LinkButton


                    oLabelNews = e.Item.Cells(1).FindControl("LBseparatorNews")
                    oLinkNews = e.Item.Cells(1).FindControl("LNBnews")
                    oImageButtonNews = e.Item.Cells(1).FindControl("IMBnews")

                    Dim oLiteralNews As Literal
                    oLiteralNews = e.Item.Cells(1).FindControl("LThasnews")
                    If IsNothing(oLabelNews) = False AndAlso IsNothing(oLiteralNews) = False Then
                        Dim oCurrent As lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount = Me.PageUtility.CommunityNewsCount(Me.PageUtility.CurrentUser.ID, CInt(e.Item.DataItem("CMNT_ID")))
                        oLabelNews.Visible = (oCurrent.Count > 0) AndAlso Not CBool(e.Item.DataItem("CMNT_Bloccata")) AndAlso hasAccesso
                        oLiteralNews.Visible = oLabelNews.Visible

                        If oCurrent.Count > 0 Then
                            Dim Url As String = PageUtility.GetCommunityNewsUrl(PageUtility.CurrentUser.ID, CInt(e.Item.DataItem("CMNT_ID")), lm.Modules.NotificationSystem.Domain.ViewModeType.FromCommunitiesAccess)
                            If Url <> "" Then
                                oResource.setLiteral(oLiteralNews)
                                oLiteralNews.Text = String.Format(oLiteralNews.Text, Url, oCurrent.Count, PageUtility.BaseUrl & "images/HasNews.gif", cssLink)
                            Else
                                oLabelNews.Visible = False
                                oLiteralNews.Visible = False
                            End If
                        End If
                    End If

                    'If IsNothing(oLabelNews) = False And IsNothing(oLinkNews) = False And IsNothing(oImageButtonNews) = False Then
                    '	If IsDBNull(e.Item.DataItem.Item("HasNews")) Then
                    '		oLabelNews.Visible = False
                    '		oLinkNews.Visible = False
                    '		oImageButtonNews.Visible = False
                    '	Else
                    '		oLabelNews.Visible = CBool(e.Item.DataItem.Item("HasNews"))
                    '		oLinkNews.Visible = CBool(e.Item.DataItem.Item("HasNews"))
                    '		oImageButtonNews.Visible = CBool(e.Item.DataItem.Item("HasNews"))
                    '	End If
                    '	oLinkNews.CssClass = cssLink
                    '	oResource.setLinkButton(oLinkNews, True, False)
                    '	oResource.setImageButton_Datagrid(Me.DGComunita, oImageButtonNews, "IMBnews", True, True)
                    '	Try
                    '		If e.Item.DataItem("CMNT_Bloccata") = True Then
                    '			oLinkNews.Enabled = False
                    '			oImageButtonNews.Enabled = False
                    '		Else
                    '			oLinkNews.Enabled = True
                    '			oImageButtonNews.Enabled = True
                    '		End If

                    '		If Not hasAccesso Then
                    '			oLinkNews.Enabled = False
                    '			oImageButtonNews.Enabled = False
                    '		End If
                    '	Catch ex As Exception

                    '	End Try
                    'End If
                Catch ex As Exception

                End Try


                oCell = New TableCell
                oCell = CType(e.Item.Cells(2), TableCell)
                oWebControl = oCell.Controls(0)
                Try

                    oLinkbutton = oWebControl

                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='" & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='" & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='" & oLinkbutton.Text & "';return true;")
                    oLinkbutton.CssClass = cssLink
                Catch ex As Exception

                End Try

                Try
                    Dim oIMGisChiusa As System.Web.UI.WebControls.Image
                    oIMGisChiusa = e.Item.Cells(1).FindControl("IMGisChiusa")

                    If IsNothing(oIMGisChiusa) = False Then
                        Dim ImageBaseDir As String
                        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
                        ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/"
                        ImageBaseDir = Replace(ImageBaseDir, "//", "/")

                        oIMGisChiusa.Visible = True
                        oIMGisChiusa.ImageUrl = ImageBaseDir & oResource.getValue("stato.image." & e.Item.DataItem("CMNT_isChiusa"))
                        oIMGisChiusa.AlternateText = oResource.getValue("stato." & e.Item.DataItem("CMNT_isChiusa"))
                    End If

                Catch ex As Exception

                End Try

                Try
                    Dim oImageButton As System.Web.UI.WebControls.ImageButton
                    oImageButton = e.Item.Cells(13).FindControl("IMBdeiscrivi")

                    If IsNothing(oImageButton) = False Then
                        Dim standard As Boolean = True
                        If e.Item.DataItem("CMNT_TPCM_id") = Main.TipoComunitaStandard.Organizzazione Then
                            If e.Item.DataItem("CMNT_ORGN_ID") = Me.OrganizzazioneDefaultID Then
                                standard = False
                                oImageButton.Visible = False
                            ElseIf e.Item.DataItem("CMNT_CanUnsubscribe") = False Then
                                standard = False
                                oImageButton.Visible = False
                            Else
                                oImageButton.Visible = True
                            End If
                    
                        ElseIf e.Item.DataItem("CMNT_CanUnsubscribe") = False Then
                            standard = False
                        Else
                            oImageButton.Visible = True
                        End If

                        If standard Then
                            oResource.setImageButton_Datagrid(Me.DGComunita, oImageButton, "IMBdeiscrivi", True, True, True, True)

                            oImageButton.Enabled = True
                        Else
                            oImageButton.Enabled = False
                            oImageButton.Attributes.Add("onclick", "window.status='';return true;")
                            oImageButton.Attributes.Add("onfocus", "window.status='';return true;")
                            oImageButton.Attributes.Add("onmouseover", "window.status='';return true;")
                            oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                            oImageButton.ImageUrl = "./../images/x_d.gif"

                            oResource.setImageButton_Datagrid(Me.DGComunita, oImageButton, "IMBdeiscrivi", False, True, True)
                        End If

                    End If

                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try
        End If
    End Sub

    Sub DGComunita_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGComunita.PageIndexChanged
        DGComunita.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia()
    End Sub

    Private Sub DGComunita_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGComunita.ItemCommand
        Dim CMNT_ID As Integer
        Dim CMNT_Path As String

        Try

            CMNT_Path = DGComunita.Items(e.Item.ItemIndex).Cells(9).Text()
            CMNT_ID = CInt(DGComunita.DataKeys.Item(e.Item.ItemIndex))
            Select Case e.CommandName
                Case "Login"

                    Me.EntraComunita(CMNT_ID, CMNT_Path)

                Case "dettagli"
                    Dim isArchiviata, isBloccata As Boolean
                    'richiamo il controllo utente
                    Me.HDNcmnt_ID.Value = CMNT_ID
                    Me.HDNcmnt_Path.Value = CMNT_Path
                    Me.PNLData.Visible = False
                    Me.PNLdettagli.Visible = True
                    Me.PNLmenuDettagli.Visible = True
                    Me.PNLmenu.Visible = False
                    Try
                        isArchiviata = DGComunita.Items(e.Item.ItemIndex).Cells(20).Text()
                    Catch ex As Exception

                    End Try
                    Try
                        isBloccata = DGComunita.Items(e.Item.ItemIndex).Cells(21).Text()
                    Catch ex As Exception

                    End Try
                    Me.LNBentra.Enabled = Not isBloccata

                    If Me.LNBentra.Enabled Then
                        Dim oRuoloComunita As New COL_RuoloPersonaComunita

                        Dim oPersona As COL_Persona
                        oPersona = Session("objPersona")
                        oRuoloComunita.Estrai(CMNT_ID, oPersona.ID)
                        Me.LNBentra.Enabled = (oRuoloComunita.Attivato And oRuoloComunita.Abilitato)
                    End If
                    Me.CTRLDettagli.SetupDettagliComunita(CMNT_ID)
                    Me.PageUtility.AddAction(ActionType.CommunityDetails, Nothing, InteractionType.Generic)
                Case "deIscrivi"
                    Try
                        Dim oComunita As New COL_Comunita
                        Dim oPersona As COL_Persona
                        Dim CanUnsubscribe As Boolean = False
                        Dim alertMSG As String

                        oPersona = Session("objPersona")
                        oComunita.Id = CMNT_ID
                        oComunita.Estrai()
                        If oComunita.Errore = Errori_Db.None Then
                            Select Case oComunita.TipoComunita.ID
                                Case Main.TipoComunitaStandard.Organizzazione
                                    If oPersona.ORGNDefault_id = oComunita.Organizzazione.Id Then
                                        alertMSG = oResource.getValue("messaggio.Organizzazione")
                                        If alertMSG <> "" Then
                                            alertMSG = alertMSG.Replace("'", "\'")
                                        Else
                                            alertMSG = "Non è possibile de-iscriversi dalla propria facoltà/organizzazione !"
                                        End If
                                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                                    Else
                                        CanUnsubscribe = oComunita.CanUnsubscribe
                                    End If

                                
                                Case Else
                                    If oComunita.CanUnsubscribe Then
                                        CanUnsubscribe = True
                                    Else
                                        alertMSG = oResource.getValue("messaggio.NodeIscrizione")
                                        If alertMSG <> "" Then
                                            alertMSG = alertMSG.Replace("'", "\'")
                                        Else
                                            alertMSG = "Non è possibile de-iscriversi dalla comunità selezionata !"
                                        End If
                                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                                    End If
                            End Select
                            ''If e.Item.DataItem("ORGN_Diretta") = True Then
                            If oComunita.Bloccata And CanUnsubscribe Then
                                alertMSG = oResource.getValue("messaggio.NodeIscrizione")
                                If alertMSG <> "" Then
                                    alertMSG = alertMSG.Replace("'", "\'")
                                Else
                                    alertMSG = "Non è possibile de-iscriversi dalla comunità selezionata !"
                                End If
                                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                                CanUnsubscribe = False
                            End If
                        End If

                        If CanUnsubscribe Then
                            Me.PageUtility.AddAction(ActionType.RemoveSubscription, Nothing, InteractionType.Generic)
                            Session("azione") = "deiscrivi"
                            Me.DeIscrivi(CMNT_ID, CMNT_Path)
                        Else
                            Me.Bind_Griglia()
                        End If

                    Catch ex As Exception
                        Me.Bind_Griglia()
                    End Try

                Case "legginews"
                    Me.SaveSearchParameters(3)
                    Session("CMNT_path_forNews") = CMNT_Path
                    Session("CMNT_ID_forNews") = CMNT_ID
                    Me.PageUtility.RedirectToUrl("generici/News_Comunita.aspx?from=EntrataComunita")
            End Select
        Catch ex As Exception

        End Try

    End Sub

    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGComunita.SortCommand
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

#Region "Funzioni Generali"
    Private Sub LNBalberoGerarchico_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBalberoGerarchico.Click
        Me.SaveSearchParameters(2)
        Me.PageUtility.RedirectToUrl("Comunita/NavigazioneTreeView.aspx?forChange=true&show=2")
    End Sub

    Private Sub LNBalbero_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBalbero.Click
        Me.SaveSearchParameters(1)
        Me.PageUtility.RedirectToUrl("Comunita/NavigazioneTreeView.aspx?forChange=true&show=1")
    End Sub

    Private Sub LNBannulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBannulla.Click
        Me.ResetForm()
    End Sub

    Private Sub EntraComunita(ByVal idCommunity As Integer, ByVal path As String)
        Dim oResourceConfig As New ResourceManager
        oResourceConfig = GetResourceConfig(Session("LinguaCode"))
        Dim status As lm.Comol.Core.DomainModel.SubscriptionStatus
        Dim idPerson As Integer = PageUtility.CurrentUser.ID
        status = PageUtility.AccessToCommunity(idPerson, idCommunity, path, oResourceConfig, True)

        Dim oTreeComunita As New COL_TreeComunita
        Try
            oTreeComunita.Directory = Server.MapPath(PageUtility.BaseUrl & "profili/") & idPerson & "\"
            oTreeComunita.Nome = idPerson & ".xml"
        Catch ex As Exception

        End Try

        Select Case status
            Case lm.Comol.Core.DomainModel.SubscriptionStatus.activemember
                Exit Sub
            Case lm.Comol.Core.DomainModel.SubscriptionStatus.blocked
                Me.PNLmenu.Visible = False
                Me.PNLmenuDettagli.Visible = False
                Me.PNLmenuDeiscrivi.Visible = False
                Me.PNLmenuAccesso.Visible = True
                Me.PNLData.Visible = False
                Me.PNLdettagli.Visible = False
                Me.PNLdeiscrivi.Visible = False
                PNLmessaggi.Visible = True
                oResource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & StringaAbilitato.inAttesa)
                oTreeComunita.CambiaAttivazione(idCommunity, False, oResource)

            Case lm.Comol.Core.DomainModel.SubscriptionStatus.waiting
                Me.PNLmenu.Visible = False
                Me.PNLmenuDettagli.Visible = False
                Me.PNLmenuDeiscrivi.Visible = False
                Me.PNLmenuAccesso.Visible = True

                Me.PNLdeiscrivi.Visible = False
                Me.PNLData.Visible = False
                Me.PNLdettagli.Visible = False
                PNLmessaggi.Visible = True
                oResource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & StringaAbilitato.bloccato)
                oTreeComunita.CambiaAbilitazione(idCommunity, False)

            Case lm.Comol.Core.DomainModel.SubscriptionStatus.communityblocked
                Me.PNLmenu.Visible = False
                Me.PNLmenuAccesso.Visible = True
                Me.PNLmenuDettagli.Visible = False
                Me.PNLmenuDeiscrivi.Visible = False

                Me.PNLdeiscrivi.Visible = False
                Me.PNLData.Visible = False
                Me.PNLdettagli.Visible = False
                PNLmessaggi.Visible = True
                oResource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & StringaAbilitato.comunitaBloccata)
                oTreeComunita.CambiaIsBloccata(idCommunity, True)
                '    Case 
                'Case Else
                '    oTreeComunita.Delete(idCommunity, path)



        End Select


        '    Dim oTreeComunita As New COL_TreeComunita
        '    Dim oPersona As New COL_Persona
        '    Dim PRSN_ID, RuoloID As Integer

        '    Try
        '        oPersona = Session("objPersona")
        '        PRSN_ID = oPersona.ID

        '        oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
        '        oTreeComunita.Nome = PRSN_ID & ".xml"
        '    Catch ex As Exception

        '    End Try

        '    Try
        '        Dim oRuolo As New COL_RuoloPersonaComunita
        '        oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)
        '        If oRuolo.Errore = Errori_Db.None Then
        '            RuoloID = oRuolo.TipoRuolo.Id
        '        End If

        '        'verifico se l'utente ha l'abilitazione per fare l'accesso alla comunità

        '        Dim oComunita As New COL_Comunita

        '        oComunita.Id = CMNT_ID
        '        oComunita.Estrai()
        '        GenericCacheManager.PurgeCacheItems(CachePolicy.PermessiServizioUtente())
        '        If oComunita.Errore = Errori_Db.None Then
        '            oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona


        '            If oRuolo.Abilitato And oRuolo.Attivato And oComunita.Bloccata = False Then 'se l'utente è attivato E abilitato allora
        '                Me.PageUtility.AddAction(ActionType.Access, Nothing, InteractionType.Generic)
        '                ' metto in sessione i permessi che l'utente ha per quella comunità
        '                Dim i, j As Integer
        '                Session("IdRuolo") = RuoloID
        '                Session("IdComunita") = CMNT_ID


        '                Dim Elenco_CMNT_ID() As String
        '                Elenco_CMNT_ID = CMNT_Path.Split(".")

        '                Dim totale As Integer
        '                Dim ArrComunita(,) As String = Nothing

        '                With oComunita
        '                    Session("ORGN_id") = .Organizzazione.Id
        '                    Try
        '                        Dim oDataSet As New DataSet
        '                        oDataSet = COL_Servizio.ElencaByTipoRuoloByComunita(Session("IdRuolo"), CMNT_ID)
        '                        totale = oDataSet.Tables(0).Rows.Count - 1

        '                        Dim ArrPermessi(totale, 2) As String
        '                        For i = 0 To totale
        '                            Dim oRow As DataRow
        '                            oRow = oDataSet.Tables(0).Rows(i)
        '                            ArrPermessi(i, 0) = oRow.Item("SRVZ_Codice") 'CODICE servizio
        '                            ArrPermessi(i, 1) = oRow.Item("SRVZ_ID") 'id servizio
        '                            ArrPermessi(i, 2) = oRow.Item("LKSC_Permessi") 'valore servizio
        '                        Next
        '                        Session("ArrPermessi") = ArrPermessi
        '                    Catch ex As Exception

        '                    End Try

        '                    Try
        '                        If Session("LogonAs") = False Then
        '                            oRuolo.UpdateUltimocollegamento()
        '                        End If
        '                    Catch ex As Exception

        '                    End Try

        '                    'Aggiorno gli array relativi al menu history !!!


        '                    Dim Path As String = ""

        '                    If Session("limbo") = True Then
        '                        j = 0
        '                        For i = 0 To UBound(Elenco_CMNT_ID) - 1

        '                            If IsNumeric(Elenco_CMNT_ID(i)) Then
        '                                If Elenco_CMNT_ID(i) > 0 Then
        '                                    ReDim Preserve ArrComunita(3, j)
        '                                    ArrComunita(0, j) = Elenco_CMNT_ID(i)
        '                                    ArrComunita(1, j) = COL_Comunita.EstraiNomeBylingua(Elenco_CMNT_ID(i), Session("LinguaID"))
        '                                    If Path = "" Then
        '                                        Path = "." & Elenco_CMNT_ID(i) & "."
        '                                    Else
        '                                        Path = Path & Elenco_CMNT_ID(i) & "."
        '                                    End If
        '                                    ArrComunita(2, j) = Path
        '                                    ' Ruolo svolto..........
        '                                    ArrComunita(3, j) = oPersona.GetIDRuoloForComunita(Elenco_CMNT_ID(i))
        '                                    j = j + 1
        '                                End If
        '                            End If
        '                        Next

        '                        Session("ArrComunita") = ArrComunita
        '                        Session("limbo") = False

        '                    Else 'altrimento lo faccio per passi successivi

        '                        'caricamento navigazione albero comunità
        '                        Try
        '                            ArrComunita = Session("ArrComunita")
        '                            totale = UBound(ArrComunita, 2) 'recupero il numero di comunità dell'array


        '                            ' Cerco di recuperare solo gli id delle nuove comunità da agiungere
        '                            ' nell'array
        '                            Dim Last_Path As String = ""

        '                            'recupero l'ultimo path presente nell'history
        '                            Path = ArrComunita(2, totale)
        '                            Last_Path = Right(CMNT_Path, CMNT_Path.Length - Path.Length)


        '                            j = 0
        '                            For i = 0 To UBound(Elenco_CMNT_ID) - 1

        '                                If IsNumeric(Elenco_CMNT_ID(i)) Then
        '                                    If Elenco_CMNT_ID(i) > 0 Then
        '                                        ReDim Preserve ArrComunita(3, j)
        '                                        ArrComunita(0, j) = Elenco_CMNT_ID(i)
        '                                        ArrComunita(1, j) = COL_Comunita.EstraiNomeBylingua(Elenco_CMNT_ID(i), Session("LinguaID"))

        '                                        If Path = "" Then
        '                                            Path = "." & Elenco_CMNT_ID(i) & "."
        '                                        Else
        '                                            Path = Path & Elenco_CMNT_ID(i) & "."
        '                                        End If
        '                                        ArrComunita(2, j) = Path
        '                                        ' Ruolo svolto..........
        '                                        ArrComunita(3, j) = oPersona.GetIDRuoloForComunita(Elenco_CMNT_ID(i))
        '                                        j = j + 1
        '                                    End If
        '                                End If
        '                            Next

        '                            Session("ArrComunita") = ArrComunita
        '                            Session("limbo") = False
        '                        Catch ex As Exception

        '                        End Try
        '                    End If
        '                End With


        '                Session("RLPC_ID") = oRuolo.Id
        '                Dim oResourceConfig As New ResourceManager
        '                oResourceConfig = GetResourceConfig(Session("LinguaCode"))
        '                oComunita.RegistraAccesso(CMNT_ID, PRSN_ID, oResourceConfig.getValue("systemDBcodice"))
        '                Me.PageUtility.SendNotificationUpdateCommunityAccess(PRSN_ID, CMNT_ID, oRuolo.UltimoCollegamento)
        '                Me.SaveSearchParameters(3)
        '                oTreeComunita.Update(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)

        '                Session("AdminForChange") = False
        '                Session("CMNT_path_forAdmin") = ""
        '                Session("idComunita_forAdmin") = ""

        '                ' REGISTRAZIONE EVENTO
        '                Session("TPCM_ID") = oComunita.TipoComunita.ID

        '                If oComunita.ShowCover(PRSN_ID) Then
        '                    If oRuolo.SaltaCopertina Then
        '                        Me.PageUtility.RedirectToUrl(RedirectToDefaultPage(CMNT_ID, PRSN_ID))
        '                    Else
        '                        Me.PageUtility.RedirectToUrl("Generici/Cover.aspx")
        '                    End If
        '                Else
        '                    Me.PageUtility.RedirectToUrl(RedirectToDefaultPage(CMNT_ID, PRSN_ID)) ' se non faccio il redirect mi esegue prima il page_load dell'header e quindi vedo l'id della comunità a cui ero loggato e non quella corrente
        '                End If

        '            ElseIf oRuolo.Attivato = False Then
        '                Me.PNLmenu.Visible = False
        '                Me.PNLmenuDettagli.Visible = False
        '                Me.PNLmenuDeiscrivi.Visible = False
        '                Me.PNLmenuAccesso.Visible = True
        '                Me.PNLData.Visible = False
        '                Me.PNLdettagli.Visible = False
        '                Me.PNLdeiscrivi.Visible = False
        '                PNLmessaggi.Visible = True
        '                oResource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & StringaAbilitato.inAttesa)
        '                oTreeComunita.CambiaAttivazione(CMNT_ID, False, oResource)
        '            ElseIf oRuolo.Abilitato = False Then
        '                Me.PNLmenu.Visible = False
        '                Me.PNLmenuDettagli.Visible = False
        '                Me.PNLmenuDeiscrivi.Visible = False
        '                Me.PNLmenuAccesso.Visible = True

        '                Me.PNLdeiscrivi.Visible = False
        '                Me.PNLData.Visible = False
        '                Me.PNLdettagli.Visible = False
        '                PNLmessaggi.Visible = True
        '                oResource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & StringaAbilitato.bloccato)
        '                oTreeComunita.CambiaAbilitazione(CMNT_ID, False)
        '            ElseIf oComunita.Bloccata Then
        '                Me.PNLmenu.Visible = False
        '                Me.PNLmenuAccesso.Visible = True
        '                Me.PNLmenuDettagli.Visible = False
        '                Me.PNLmenuDeiscrivi.Visible = False

        '                Me.PNLdeiscrivi.Visible = False
        '                Me.PNLData.Visible = False
        '                Me.PNLdettagli.Visible = False
        '                PNLmessaggi.Visible = True
        '                oResource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & StringaAbilitato.comunitaBloccata)
        '                oTreeComunita.CambiaIsBloccata(CMNT_ID, True)
        '            End If
        '        Else
        '            oTreeComunita.Delete(CMNT_ID, CMNT_Path)
        '        End If
        '    Catch ex As Exception

        '    End Try
    End Sub

    Private Sub DeIscrivi(ByVal CMNT_ID As Integer, ByVal CMNT_Path As String)
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona
        Dim totale As Integer
        Dim oDataset As New DataSet


        If Session("azione") = "deiscrivi" Then
            Try
                Dim multipli As Boolean = False
                oPersona = Session("objPersona")

                oComunita.Id = CMNT_ID

                Try
                    oComunita.Estrai()
                    If oComunita.Errore = Errori_Db.None Then
                        Dim oResponsabile As New COL_Persona
                        Dim showMessage As Boolean = False
                        oResponsabile = oComunita.GetResponsabile()

                        If oPersona.ID = oResponsabile.ID Then
                            oResource.setLabel_To_Value(Me.LBMessaggi, "messaggioComunita." & StringaMessaggio.responsabile)
                            showMessage = True
                        End If
                        'If oComunita.Archiviata Then
                        '    oResource.setLabel_To_Value(Me.LBMessaggi, "messaggioComunita." & StringaMessaggio.isArchiviata)
                        '    showMessage = True
                        'End If
                        If oComunita.Bloccata Then
                            oResource.setLabel_To_Value(Me.LBMessaggi, "messaggioComunita." & StringaMessaggio.isBloccata)
                            showMessage = True
                        End If
                        If showMessage Then
                            Me.PNLData.Visible = False
                            Me.PNLdettagli.Visible = False
                            Me.PNLmessaggi.Visible = True
                            Me.PNLmenuDeiscrivi.Visible = False
                            Me.PNLmenuAccesso.Visible = True
                            Me.PNLmenuDettagli.Visible = False
                            Exit Sub
                        End If
                    End If
                Catch ex As Exception

                End Try


                Try
                    Dim oDataview As DataView
                    Dim hasCreatori, HasPassanti, HasOther As Boolean

                    oDataset = oPersona.NEW_ElencaComunitaDaDeiscrivere(CMNT_ID, CMNT_Path)
                    totale = oDataset.Tables(0).Rows.Count
                    If totale > 1 Then
                        oDataview = oDataset.Tables(0).DefaultView
                        oDataview.RowFilter = "CMNT_ID <> " & CMNT_ID

                        ' Esistono dei creatori
                        oDataview.RowFilter = "CMNT_ID <> " & CMNT_ID & " and RLPC_TPRL_ID = -2"
                        hasCreatori = (oDataview.Count > 0)

                        'esistono dei passanti.....
                        oDataview.RowFilter = "CMNT_ID <> " & CMNT_ID & " and RLPC_TPRL_ID = -3"
                        HasPassanti = (oDataview.Count > 0)

                        'altri....
                        oDataview.RowFilter = "CMNT_ID <> " & CMNT_ID & " and RLPC_TPRL_ID <> -3 and RLPC_TPRL_ID <> -2"
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
                        Me.PNLdeiscrivi.Visible = True
                        Me.PNLmenuAccesso.Visible = False
                        Me.PNLmenuDettagli.Visible = False
                        Me.PNLmenu.Visible = False
                        Me.PNLmenuDeiscrivi.Visible = True

                        Me.PNLData.Visible = False
                        Me.PNLdettagli.Visible = False
                        Me.PNLmessaggi.Visible = False

                        Me.HDNcmnt_ID.Value = CMNT_ID
                        Me.HDNcmnt_Path.Value = CMNT_Path

                        Me.Bind_TreeView(oDataset)
                        oResource.setLabel(LBinfoDeIscrivi)
                        Me.LBinfoDeIscrivi.Text = Replace(Me.LBinfoDeIscrivi.Text, "#%%#", oComunita.Nome)
                    Else
                        oPersona.NEW_DeIscriviFromComunita(CMNT_ID, CMNT_Path, False)
                        If oPersona.Errore = Errori_Db.None Then
                            Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                            oServiceUtility.NotifySelfUnSubscription(CMNT_ID, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)

                            Me.AggiornaProfiloXML(CMNT_ID, oPersona.ID, CMNT_Path)
                        End If
                        Me.ResetForm()
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try
        Else
            Me.ResetForm()
        End If
    End Sub

    Private Sub AggiornaProfiloXML(ByVal ComunitaID As Integer, ByVal PRSN_Id As Integer, ByVal ComunitaPath As String)
        Dim oRuolo As New COL_RuoloPersonaComunita
        Dim oTreeComunita As New COL_TreeComunita

        Try
            oRuolo.EstraiByLingua(ComunitaID, PRSN_Id, Session("LinguaID"))

            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_Id & "\"
            oTreeComunita.Nome = PRSN_Id & ".xml"

            If oRuolo.Errore = Errori_Db.None Then

            Else
                oTreeComunita.Delete(ComunitaID, ComunitaPath)
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Gestione DeIscrizione"

    Private Sub LNBannullaDeIscrizione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaDeIscrizione.Click
        Me.ResetForm()
    End Sub
    Private Sub LNBdeIscriviCorrente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdeIscriviCorrente.Click
        Dim oPersona As New COL_Persona
        Dim oDataset As New DataSet

        If Session("azione") = "deiscrivi" Then
            Try
                oPersona = Session("objPersona")
                oPersona.NEW_DeIscriviFromComunita(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value, False)

                If oPersona.Errore = Errori_Db.None Then
                    Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                    oServiceUtility.NotifySelfUnSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                    Me.AggiornaProfiloXML(Me.HDNcmnt_ID.Value, oPersona.ID, Me.HDNcmnt_Path.Value)
                End If
            Catch ex As Exception

            End Try
        End If
        Me.ResetForm()
    End Sub

    Private Sub LNBdeIscriviSelezionate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdeIscriviSelezionate.Click
        Dim oPersona As New COL_Persona
        Dim i, totale As Integer
        Dim oDataset As New DataSet

        If Session("azione") = "deiscrivi" Then
            Try
                oPersona = Session("objPersona")

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

                            ComunitaID = oNode.Value
                            If ComunitaID > 0 And InStr(ListaEliminate, "," & ComunitaID & ",") <= 0 Then
                                ComunitaPath = oNode.Category
                                oPersona.NEW_DeIscriviFromComunita(ComunitaID, ComunitaPath, False)
                                If oPersona.Errore = Errori_Db.None Then
                                    Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                                    oServiceUtility.NotifySelfUnSubscription(ComunitaID, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                                    Me.AggiornaProfiloXML(ComunitaID, oPersona.ID, ComunitaPath)
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
        Me.ResetForm()
    End Sub

    Private Sub LNBdeIscriviDaTutte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdeIscriviDaTutte.Click
        Dim oPersona As New COL_Persona
        Dim oDataset As New DataSet

        If Session("azione") = "deiscrivi" Then
            Try
                oPersona = Session("objPersona")
                oPersona.NEW_DeIscriviFromComunita(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value, True)
                If oPersona.Errore = Errori_Db.None Then
                    Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                    oServiceUtility.NotifySelfUnSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                    Me.AggiornaProfiloXML(Me.HDNcmnt_ID.Value, oPersona.ID, Me.HDNcmnt_Path.Value)
                End If
            Catch ex As Exception

            End Try
        End If
        Me.ResetForm()
    End Sub
#End Region

#Region "Dettagli"
    Private Sub LNBentra_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBentra.Click
        Me.EntraComunita(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value)
    End Sub
    Private Sub LNBannullaDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaDettagli.Click
        Me.ResetForm()
    End Sub
#End Region

    Private Sub Reset_HideAll()
        Me.PNLmessaggi.Visible = False
        Me.PNLData.Visible = False
        Me.PNLdettagli.Visible = False
        Me.PNLdeiscrivi.Visible = False

        Me.PNLmenu.Visible = False
        Me.PNLmenuAccesso.Visible = False
        Me.PNLmenuDettagli.Visible = False
        Me.PNLmenuDeiscrivi.Visible = False
    End Sub
    Private Sub ResetForm()
        Me.Reset_HideAll()
        Me.PNLData.Visible = True
        Me.PNLmenu.Visible = True
        Me.Bind_Griglia()
        Session("azione") = "loaded"
    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_EntrataComunita"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(Me.LBNopermessi)

            .setLinkButton(Me.LNBapriFiltro, True, True)
            .setLinkButton(Me.LNBchiudiFiltro, True, True)

            .setLinkButton(Me.LNBalbero, True, True)
            .setLinkButton(Me.LNBalberoGerarchico, True, True)
            .setLinkButton(Me.LNBentra, True, True)
            .setLinkButton(Me.LNBannullaDettagli, True, True)
            .setLinkButton(Me.LNBannulla, True, True)
            .setLabel(Me.LBstatoComunita_t)


            .setLinkButton(Me.LNBannullaDeIscrizione, True, True)
            .setLinkButton(Me.LNBdeIscriviCorrente, True, True, , True)
            .setLinkButton(Me.LNBdeIscriviSelezionate, True, True, , True)
            .setLinkButton(Me.LNBdeIscriviDaTutte, True, True, , True)

            .setRadioButtonList(Me.RBLstatoComunita, -1)
            .setRadioButtonList(Me.RBLstatoComunita, 0)
            .setRadioButtonList(Me.RBLstatoComunita, 1)
            .setRadioButtonList(Me.RBLstatoComunita, 2)

            .setLabel(Me.LBtipoComunita_c)
            .setLabel(Me.LBnumeroRecord_c)

            Me.LBnumeroRecord_c.Text = "&nbsp;&nbsp;&nbsp;" & Me.LBnumeroRecord_c.Text
            .setLabel(Me.LBtipoRicerca_c)
            .setLabel(Me.LBvalore_c)
            .setDropDownList(Me.DDLTipoRicerca, -2)
            .setDropDownList(Me.DDLTipoRicerca, -7)
            .setDropDownList(Me.DDLTipoRicerca, -5)
            .setDropDownList(Me.DDLTipoRicerca, -6)
            .setDropDownList(Me.DDLTipoRicerca, -9)


            .setButton(Me.BTNCerca, False)

            .setLabel(Me.LBorganizzazione_c)
            .setLabel(Me.LBannoAccademico_c)
            .setLabel(Me.LBperiodo_c)
            .setLabel(Me.LBcorsoDiStudi_t)
            .setLabel(Me.LBnoCorsi)

            .setCheckBox(Me.CBXmostraPadre)
            .setCheckBox(Me.CBXautoUpdate)


            .setLabel(Me.LBmsgDG)
            .setLabel(Me.LBlegend)


            Dim i As Integer
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControl("LKB" & Chr(i))
                Dim Carattere As String = Chr(i)

                If IsNothing(oLinkButton) = False Then
                    .setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
                End If
            Next
            .setLinkButton(Me.LKBtutti, True, True)
            .setLinkButton(Me.LKBaltro, True, True)
        End With

    End Sub
#End Region

#Region "Gestione TreeView"
    Private Sub Bind_TreeView(ByVal oDataset As DataSet)
        Dim oTreeComunita As New COL_TreeComunita
        Dim oPersona As New COL_Persona

        Me.RDTcomunita.Nodes.Clear()
        Try
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

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow.Item("ALCM_PAth") = dbRow.Item("ALCM_RealPath") Then
                        dbRow.Item("ALCM_RealPath") = ""
                    End If
                    If dbRow("ALCM_PadreVirtuale_ID") = 0 Or dbRow("CMNT_id") = IDcorrente Then
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
        '    Dim [continue] As Boolean = False

        start = 0

        Dim CMNT_id, RLPC_TPRL_id As Integer
        Dim CMNT_Responsabile As String = "", img As String
        Dim CMNT_isIscritto, CanUnsubscribe As Boolean
        CMNT_id = dbRow.Item("CMNT_id")

        Dim ImageBaseDir As String
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")
        ImageBaseDir = Replace(ImageBaseDir, "//", "/")

        Dim CMNT_Nome, CMNT_NomeVisibile, CMNT_path, CMNT_REALpath As String
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
        CMNT_REALpath = dbRow.Item("ALCM_REALpath")

        Dim isBloccata As Boolean = False
        isBloccata = dbRow.Item("CMNT_Bloccata")

        If CMNT_id = Me.HDNcmnt_ID.Value Then
            CMNT_Nome = "<b>" & CMNT_Nome & "</b>"
        End If
        node.Text = CMNT_Nome
        node.Value = CMNT_path ' MODIFICATO il 24 settembre CMNT_REALpath
        node.Expanded = expanded
        node.ImageUrl = img
        node.ToolTip = CMNT_NomeVisibile
        node.Category = CMNT_id
        node.Checkable = True
        If CMNT_id = Me.HDNcmnt_ID.Value Then
            node.Checked = (Not isBloccata And CanUnsubscribe)
            node.Enabled = False
        ElseIf CMNT_isIscritto Then
            node.Checkable = (Not isBloccata And CanUnsubscribe)
            node.Enabled = Not isBloccata
            If isBloccata Or CanUnsubscribe = False Then
                node.CssClass = "TreeNodeBloccata"
            End If
        Else
            node.Category = -CMNT_id
            node.Checkable = False
            node.CssClass = "TreeNodeBloccata"
        End If


        ' If IsNothing(Me.RDTcomunita.FindNodeByValue(CMNT_REALpath)) And ((CMNT_REALpath = nodeFather.Value & CMNT_id & "." And nodeFather.Category <> 0) Or nodeFather.Category = 0) Then
        Return node
        'ElseIf IsNothing(Me.RDTcomunita.FindNodeByValue(CMNT_REALpath)) And nodeFather.Category < 0 Then
        '    ' sono nell'albero organizzativo ed ho un padre con id negativo !!!
        '    If CMNT_REALpath = nodeFather.Parent.Value & CMNT_id & "." Then
        '        Return node
        '    Else
        '        Return Nothing
        '    End If

        '    ' ((CMNT_REALpath = nodeFather.Value & CMNT_id & "." And <> 0) Or nodeFather.Category = 0) Then

        'Else
        '    Return Nothing
        'End If
    End Function 'CreateNode

    'Private Function AggiornaTreeView()
    '    Dim oPersona As New COL_Persona
    '    Dim oTreeComunita As New COL_TreeComunita
    '    Dim PRSN_ID As Integer

    '    oPersona = Session("objPersona")

    '    Try
    '        'TEMPORANEO
    '        'Dim odataset As New DataSet
    '        'odataset = oPersona.Elenca
    '        'Dim i, totale As Integer
    '        'totale = odataset.Tables(0).Rows.Count - 1

    '        'For i = 0 To totale
    '        ' PRSN_ID =  odataset.Tables(0).Rows(i).Item("PRSN_ID")
    '        PRSN_ID = oPersona.Id
    '        oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
    '        oTreeComunita.Nome = PRSN_ID & ".xml"

    '        oTreeComunita.AggiornaInfo(PRSN_ID, Session("LinguaID"))
    '        'Next
    '    Catch ex As Exception

    '    End Try


    'End Function
    Private Sub GeneraNoNode()
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
    End Sub

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

    'Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    '    If Not Me.oResource.UserLanguages = Session("LinguaCode") Then
    '        Me.SetCulture(Session("LinguaCode"))
    '        Me.SetupInternazionalizzazione()
    '    End If
    'End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_ElencaComunita.Codex)
    End Sub
    'Public Sub AddAction(ByVal oType As Integer, ByVal oObjectActions As System.Collections.Generic.List(Of lm.ActionDataContract.ObjectAction), Optional ByVal TypeIteration As lm.ActionDataContract.InteractionType = lm.ActionDataContract.InteractionType.Generic)
    '    Me.PageUtility.AddAction(oType, oObjectActions, TypeIteration)
    'End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

End Class