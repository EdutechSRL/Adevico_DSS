Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_permessi

Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_ElencaComunita
Imports Telerik.WebControls
Imports System.Linq
Public Class NavigazioneTreeView
    Inherits System.Web.UI.Page

    Private oResource As ResourceManager
    Private _PageUtility As OLDpageUtility

    Private ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property

    Protected Enum AzioneTree
        Aggiorna = 1
        Dettagli = 2
        Entra = 3
        Iscrivi = 4
        Novità = 5
    End Enum

    Private Enum Iscrizioni_code
        IscrizioniAperteIl = 0
        IscrizioniChiuse = 1
        IscrizioniComplete = 2
        IscrizioniEntro = 3
    End Enum

    Private Enum StringaAbilitato
        abilitato = 1
        bloccato = 0
        inAttesa = -1
        errore = 2
        noCommunity = 3
    End Enum

    Private Enum stringaRegistrazione
        errore = 0
        inAttesa = 1
        limiteIscrizione = 2
    End Enum

#Region "FORM PERMESSI"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

#Region "Gestione Menu"
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label

    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBlista As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBalberoGerarchico As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBalbero As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLmenuAccesso As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannulla As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLmenuDettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaDettagli As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBentra As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBiscrivi As System.Web.UI.WebControls.LinkButton
#End Region

#Region "Filtro"
    Protected WithEvents CBXautoUpdate As System.Web.UI.WebControls.CheckBox
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button

    Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton

    Protected WithEvents TBRfiltro As System.Web.UI.WebControls.TableRow

    Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow

    Protected WithEvents LBtipoComunita_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLTipo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBorganizzazione_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBannoAccademico_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLannoAccademico As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBperiodo_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLperiodo As System.Web.UI.WebControls.DropDownList

    Protected WithEvents TBRorgnCorsi As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLcorsi As System.Web.UI.WebControls.Table
    Protected WithEvents TBLcorsiDiStudio As System.Web.UI.WebControls.Table
    Protected WithEvents LBcorsoDiStudi_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoCorsoDiStudi As System.Web.UI.WebControls.DropDownList

    Protected WithEvents LBstatoComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBLstatoComunita As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LBnoCorsi As System.Web.UI.WebControls.Label

    Protected WithEvents LBvalore_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoRicerca_c As System.Web.UI.WebControls.Label

#Region "Filtri automatici"
    Protected WithEvents HDNselezionato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcomunitaSelezionate As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroFacolta As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoRicerca As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroValore As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroResponsabileID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroLaureaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoCdl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroAnno As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroPeriodo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoComunitaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroStatus As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList

    Protected WithEvents DDLresponsabile As System.Web.UI.WebControls.DropDownList
#End Region

#Region "Form Vari"

#Region "FORM TreeView"
    Protected WithEvents PNLtreeView As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBaggiorna As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBespandi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcomprimi As System.Web.UI.WebControls.LinkButton
    ' Protected WithEvents LNBelenco As System.Web.UI.WebControls.LinkButton

    Protected WithEvents RDTcomunita As Telerik.WebControls.RadTreeView
    Protected WithEvents HDN_Path As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

    Protected WithEvents LBavviso As System.Web.UI.WebControls.Label

#Region "Form Dettagli"
    Protected WithEvents LBlegend As System.Web.UI.WebControls.Label
    Protected WithEvents PNLdettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLdettagli As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLDettagli As Comunita_OnLine.UC_DettagliComunita

    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDisChiusa As System.Web.UI.HtmlControls.HtmlInputHidden

#End Region

#Region "Form messaggio"
    Protected WithEvents PNLmessaggi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBmessaggi As System.Web.UI.WebControls.Label
    Protected WithEvents LBtreeView As System.Web.UI.WebControls.Label
#End Region

#End Region

    Protected WithEvents PNLiscrizioneAvvenuta As System.Web.UI.WebControls.Panel
    Protected WithEvents LBiscrizione As System.Web.UI.WebControls.Label

    Protected WithEvents PNLmenuConferma As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaConferma As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBiscriviConferma As System.Web.UI.WebControls.LinkButton

#Region "Conferma Iscrizione"
    Protected WithEvents PNLconferma As System.Web.UI.WebControls.Panel
    Protected WithEvents LBconferma As System.Web.UI.WebControls.Label
    Protected WithEvents LBconfermaMultipla As System.Web.UI.WebControls.Label
#End Region

    'Protected WithEvents PNLmessaggi As System.Web.UI.WebControls.Panel
    'Protected WithEvents LBMessaggi As System.Web.UI.WebControls.Label

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

        Try
            Dim iCMNT_ID As Integer
            Dim iCMNT_PAth, elenco() As String

            If Page.IsPostBack = False Then
                Dim oServizioElencaComunita As New Services_ElencaComunita
                oServizioElencaComunita = Me.ImpostaPermessi

                'Se entro la prima volta
                Session("AdminForChange") = False
                Session("idComunita_forAdmin") = ""
                Session("CMNT_path_forAdmin") = ""
                Session("CMNT_path_forNews") = ""
                Session("CMNT_ID_forNews") = ""

                Me.SetupInternazionalizzazione()
                If oServizioElencaComunita.List Or oServizioElencaComunita.Admin Then
                    'LO "DOVREBBE FARE IN MANIERA AUTONOMA L'HEADER...

                    'If Session("Limbo") = "true" Then
                    '    Me.CTRLintestazione.ShowNews = True
                    'Else
                    '    Me.CTRLintestazione.ShowNews = False
                    'End If
                    'Recupero impostazioni utente
                    Me.LNBalbero.Visible = False
                    Me.LNBalberoGerarchico.Visible = False

                    If Me.Request.QueryString("re_set") <> "" Then
                        Try
                            If Me.Response.Cookies("EntrataComunita")("RBLvisualizza") = 2 Then
                                Me.LNBalbero.Visible = True
                            Else
                                Me.LNBalberoGerarchico.Visible = True
                            End If
                        Catch ex As Exception
                            Me.LNBalberoGerarchico.Visible = True
                        End Try
                    Else
                        Dim oImpostazioni As New COL_ImpostazioniUtente
                        Try
                            If IsNothing(Session("oImpostazioni")) Then
                                Me.ChangePage()
                            Else
                                Try
                                    oImpostazioni = Session("oImpostazioni")
                                    'If Me.Request.QueryString("forChange") <> "true" And Me.Request.QueryString("show") = "" Then
                                    If oImpostazioni.Visualizza_Iscritto = 1 Then
                                        Me.LNBalberoGerarchico.Visible = True
                                    ElseIf oImpostazioni.Visualizza_Iscritto = 2 Then
                                        Me.LNBalbero.Visible = True
                                    Else
                                        Me.LNBalberoGerarchico.Visible = True
                                    End If
                                    ' Else
                                    '  Me.ChangePage()
                                    ' End If
                                Catch ex As Exception
                                    Me.LNBalberoGerarchico.Visible = True
                                End Try
                            End If
                        Catch ex As Exception
                            Me.LNBalberoGerarchico.Visible = True
                        End Try
                    End If

                    Me.SetupFiltri()
                    If Me.LNBalbero.Visible Or Me.LNBalberoGerarchico.Visible Then
                        Me.TBRfiltro.Visible = True
                        Me.Bind_TreeView(True)
                    Else
                        Me.PNLmenuDettagli.Visible = False
                        Me.PNLtreeView.Visible = False
                        Me.TBRfiltro.Visible = False
                    End If
                    Me.PNLpermessi.Visible = False
                    Me.PNLmenu.Visible = True
                    Me.PNLcontenuto.Visible = True
                Else
                    Me.PNLmenu.Visible = False
                    Me.PNLpermessi.Visible = True
                    Me.PNLcontenuto.Visible = False
                End If
				Me.PageUtility.AddAction(IIf(Me.PNLpermessi.Visible, ActionType.NoPermission, ActionType.List), Nothing)
            End If
        Catch ex As Exception

        End Try

        Me.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Page.Form.DefaultFocus = Me.TXBValore.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.TXBValore.UniqueID

    End Sub

    Private Function ImpostaPermessi() As Services_ElencaComunita
        Dim ComunitaID As Integer = 0
        Dim iResponse As String = "00000000000000000000000000000000"
        Dim oServizioElencaComunita As New Services_ElencaComunita
        Dim oPersona As COL_Persona

        Try
            oPersona = Session("objPersona")
            ComunitaID = Session("idComunita")
        Catch ex As Exception
            ComunitaID = 0
        End Try
        Try
            If ComunitaID = 0 Then
                Session("Limbo") = True
                If oPersona.TipoPersona.id = Main.TipoPersonaStandard.Guest Then
                    iResponse = "00000000000000000000000000000000"
                Else
                    oServizioElencaComunita.Admin = False
                    oServizioElencaComunita.List = True
                    Return oServizioElencaComunita
                End If
            Else
                iResponse = Permessi(oServizioElencaComunita.Codex, Me.Page)
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
        End If
    End Function

    Private Sub ChangePage()
        Try
            Me.LNBalbero.Visible = False
            Me.LNBalberoGerarchico.Visible = False
            Select Case Me.Request.QueryString("show")
                Case ""
                    Me.LNBalberoGerarchico.Visible = True
                Case Is = "1"
                    Me.LNBalberoGerarchico.Visible = True
                Case Is = "2"
                    Me.LNBalbero.Visible = True
                Case Else
                    Me.LNBalberoGerarchico.Visible = True
            End Select
        Catch ex As Exception
            Me.LNBalberoGerarchico.Visible = True
        End Try
    End Sub


#Region "Bind_Dati"
    Private Sub ChangeTipoComunita()
        Dim showFiltroCorso As Boolean = True ' False

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
                        If oImpostazioni.AA_Iscritto = -2 Then
                            Me.DDLannoAccademico.SelectedIndex = 1
                        ElseIf oImpostazioni.AA_Iscritto = -2 Then
                            Me.DDLannoAccademico.SelectedIndex = 0
                        ElseIf Me.TBLcorsi.Visible Then
                            Me.DDLannoAccademico.SelectedValue = oImpostazioni.AA_Iscritto
                        End If

                    Catch ex As Exception

                    End Try
                   
                Else

                End If
                Me.SaveSearchParameters(3)
            Catch ex As Exception
            End Try
        Else

        End If
        If Me.Request.QueryString("re_set") <> "true" Then
            If Session("limbo") = True Then
                Me.RBLstatoComunita.SelectedIndex = 0
            Else
                Dim oComunita As New COL_Comunita
                oComunita.Id = Session("idComunita")
                If oComunita.isBloccata() Then
                    Try
                        Me.RBLstatoComunita.SelectedValue = 2
                    Catch ex As Exception
                        Me.RBLstatoComunita.SelectedIndex = 0
                    End Try

                ElseIf oComunita.isArchiviata() Then
                    Try
                        Me.RBLstatoComunita.SelectedValue = 1
                    Catch ex As Exception
                        Me.RBLstatoComunita.SelectedIndex = 0
                    End Try
                Else
                    Me.RBLstatoComunita.SelectedIndex = 0
                End If

            End If
        End If
        Me.Bind_Responsabili(-1, Main.FiltroRicercaComunitaByIscrizione.iscritto)

        Me.DDLannoAccademico.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLperiodo.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLtipoCorsoDiStudi.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.RBLstatoComunita.AutoPostBack = Me.CBXautoUpdate.Checked

        Me.DDLTipoRicerca.Attributes.Add("onchange", "return AggiornaForm();")
        Me.HDNselezionato.Value = Me.DDLTipoRicerca.SelectedValue
    End Sub

    Private Sub Bind_Responsabili(Optional ByVal DocenteID As Integer = -1, Optional ByVal FiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.nonIscritto)
        Dim oDataSet As New DataSet
        Dim oComunita As COL_Comunita
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

            oDataSet = oComunita.RicercaResponsabili(FacoltaID, ComunitaID, Session("objPersona").id, TipoComuniaID, , , , , Me.RBLstatoComunita.SelectedValue, FiltroIscrizione)



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
            Dim oListItem_Archiviate, oListItem_Bloccate As ListItem
            oPersona = Session("objPersona")
            oPersona.StatusComunitaIscritto(oPersona.Id, totale, TotaleArchiviate, totaleBloccate)

            oListItem_Archiviate = Me.RBLstatoComunita.Items.FindByValue(1)
            oListItem_Bloccate = Me.RBLstatoComunita.Items.FindByValue(2)
            If totaleBloccate = 0 Then
                If Not IsNothing(oListItem_Bloccate) Then
                    Me.RBLstatoComunita.Items.Remove(oListItem_Bloccate)
                End If
            Else
                If IsNothing(oListItem_Bloccate) Then
                    If IsNothing(oListItem_Archiviate) Then
                        Me.RBLstatoComunita.Items.Insert(1, New ListItem("Bloccate", 2))
                    Else
                        Me.RBLstatoComunita.Items.Insert(2, New ListItem("Bloccate", 2))
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
                    Me.RBLstatoComunita.Items.Insert(1, New ListItem("Archiviate", 1))
                    oResource.setRadioButtonList(Me.RBLstatoComunita, 1)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TipiComunita()
        '...nella ddl che mi farà da filtro delle tipologie di utenti associate al tipo comunità
        Dim oDataSet As New DataSet
        Dim oTipoComunita As New COL_Tipo_Comunita


        Try
            oDataSet = oTipoComunita.ElencaForFiltri(Session("LinguaID"), True)
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

        Dim i, totale, totaleHistory, TPCM_ID As Integer
        Dim Path As String
        Dim oDataset As New DataSet
        Dim ArrComunita(,) As String

        Try
            Dim oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti
            Dim oFiltroLettera As Main.FiltroComunita = Main.FiltroComunita.tutti
            Dim valore As String

            oPersona = Session("objPersona")
            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & oPersona.Id & "\"
            oTreeComunita.Nome = oPersona.Id & ".xml"

            oPersona = Session("objPersona")
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


            Dim FacoltaID, LaureaID, PeriodoID, AAid, TipocomunitaID, TipoCdlID, StatusID As Integer
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

            Dim ImageBaseDir, img As String
            ImageBaseDir = GetPercorsoApplicazione(Me.Request)
            ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"
            ImageBaseDir = Replace(ImageBaseDir, "//", "/")

            Dim oFiltroAlbero As Main.ElencoRecord = Main.ElencoRecord.AdAlbero
            If Me.LNBalbero.Visible = True Then
                oFiltroAlbero = Main.ElencoRecord.AdAlberoOrganizzativo
            End If

            oDataset = oTreeComunita.RicercaComunitaAlbero(oPersona, ComunitaPadreID, Me.oResource, ImageBaseDir, TipocomunitaID, FacoltaID, , , , ComunitaPadreID, ComunitaPath, oFiltroTipoRicerca, valore, StatusID, , oFiltroAlbero)

        Catch ex As Exception

        End Try
        Return oDataset
    End Function
#End Region

#Region "Gestione Treeview"
    Private Sub Bind_TreeView(Optional ByVal ApplicaFiltri As Boolean = False)
        Dim oPersona As New COL_Persona
        Dim oDataset As New DataSet

        Try
            oPersona = Session("objPersona")

            oDataset = Me.FiltraggioDati(ApplicaFiltri)
            Me.RDTcomunita.Nodes.Clear()

            Dim nodeRoot As New RadTreeNode
            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "folder.gif"
            nodeRoot.Value = ""
            Try
                nodeRoot.Text = oResource.getValue("oRootNode.Text")
                If nodeRoot.Text = "" Then
                    nodeRoot.Text = "Comunità: "
                End If
            Catch ex As Exception
                nodeRoot.Text = "Comunità: "
            End Try
            Try
                nodeRoot.ToolTip = oResource.getValue("oRootNode.ToolTip")
                If nodeRoot.ToolTip = "" Then
                    nodeRoot.ToolTip = "Elenco comunità"
                End If
            Catch ex As Exception
                nodeRoot.ToolTip = "Elenco comunità"
            End Try

            Me.CreateContextMenu(nodeRoot, False, True)
            Me.RDTcomunita.Nodes.Add(nodeRoot)
            If oDataset.Tables(0).Rows.Count = 0 Then
                ' nessuna comunità a cui si è iscritti
                Me.GeneraNoNode()
            Else
                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("ALCM_PAth"), oDataset.Tables(0).Columns("ALCM_RealPath"), False)

                Dim ComunitaPath As String = ""
                If IsArray(Session("ArrComunita")) Then
                    Try
                        Dim ArrComunita(,) As String
                        ArrComunita = Session("ArrComunita")
                        ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
                    Catch ex As Exception

                    End Try
                End If

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    Dim s As String = ""
                    s = dbRow.Item("ALCM_PAth")
                    s = dbRow.Item("ALCM_RealPath")
                    If dbRow("ALCM_PadreVirtuale_ID") = 0 Or ComunitaPath = dbRow("ALCM_RealPath") Then
                        Dim node As RadTreeNode = CreateNode(dbRow, True)
                        nodeRoot.Nodes.Add(node)
                        RecursivelyPopulate(dbRow, node)
                    End If
                Next dbRow
                Me.PNLtreeView.Visible = True
            End If
        Catch ex As Exception
            Me.PNLtreeView.Visible = False
        End Try
    End Sub

    Private Sub RecursivelyPopulate(ByVal dbRow As DataRow, ByVal node As RadTreeNode)
        Dim childRow As DataRow

        Try
            For Each childRow In dbRow.GetChildRows("NodeRelation")
                Dim childNode As RadTreeNode = CreateNode(childRow, False)
                node.Nodes.Add(childNode)
                RecursivelyPopulate(childRow, childNode)
            Next childRow
        Catch ex As Exception

        End Try

    End Sub
    Private Function CreateNode(ByVal dbRow As DataRow, ByVal expanded As Boolean) As RadTreeNode
        Dim node As New RadTreeNode

        Dim start As Integer
        Dim [continue] As Boolean = False
        Dim numIscritti, maxIscritti, iscritti As Integer
        start = 0

        Dim CMNT_id, RLPC_TPRL_id As Integer
        Dim CMNT_Responsabile, img As String
        Dim isIscritto As Boolean = False

        Try
            CMNT_id = dbRow.Item("CMNT_id")
            If IsDBNull(dbRow.Item("RLPC_TPRL_id")) Then
                RLPC_TPRL_id = -1
            Else
                RLPC_TPRL_id = dbRow.Item("RLPC_TPRL_id")

                If RLPC_TPRL_id > -1 Then
                    isIscritto = True
                End If
            End If

            Dim ImageBaseDir As String
            ImageBaseDir = GetPercorsoApplicazione(Me.Request)
            ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")

            Dim CMNT_Nome, CMNT_NomeVisibile, CMNT_REALpath, CMNT_path As String
            Dim CMNT_IsChiusa As Boolean

            CMNT_Nome = dbRow.Item("CMNT_Nome")
            CMNT_NomeVisibile = CMNT_Nome
            CMNT_IsChiusa = dbRow.Item("CMNT_IsChiusa")
            If dbRow.Item("ALCM_isChiusaForPadre") = True Then
                CMNT_IsChiusa = True
            End If

            If CMNT_id > 0 Then
                If IsDBNull(dbRow.Item("CMNT_Iscritti")) = False Then
                    maxIscritti = dbRow.Item("CMNT_MaxIscritti")
                    numIscritti = dbRow.Item("CMNT_Iscritti")

                    If maxIscritti <= 0 Then
                        dbRow.Item("CMNT_Iscritti") = 0
                        iscritti = 0
                    Else
                        If numIscritti > maxIscritti Then
                            dbRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                            iscritti = maxIscritti - numIscritti
                        ElseIf numIscritti = maxIscritti Then
                            dbRow.Item("CMNT_Iscritti") = -1
                            iscritti = -1
                        Else
                            dbRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                            iscritti = maxIscritti - numIscritti
                        End If
                    End If
                Else
                    dbRow.Item("CMNT_Iscritti") = 0
                End If

                CMNT_Responsabile = dbRow.Item("AnagraficaResponsabile")

                If IsDBNull(dbRow.Item("TPCM_icona")) Then
                    img = ""
                Else
                    img = "./logo/" & dbRow.Item("TPCM_icona")
                End If

                CMNT_Nome = CMNT_Nome & CMNT_Responsabile
                CMNT_NomeVisibile = CMNT_Nome

                CMNT_Nome = CMNT_Nome & Me.GenerateImage(ImageBaseDir & oResource.getValue("stato.image." & CMNT_IsChiusa), oResource.getValue("stato." & CMNT_IsChiusa))

                'If dbRow.IsNull("CMNT_AnnoAccademico") = False Then
                '    If dbRow.Item("CMNT_AnnoAccademico") <> "" Then
                '        CMNT_Nome = CMNT_Nome & "&nbsp;(" & dbRow.Item("CMNT_AnnoAccademico") & ")&nbsp;"
                '    End If
                'End If
            Else
                CMNT_NomeVisibile = CMNT_Nome
            End If
            'If IsDBNull(dbRow.Item("CMNT_path")) Then
            '    CMNT_path = "." & CMNT_idPadre & "."
            'Else
            '    CMNT_path = oRow.Item("CMNT_path")
            'End If

            'If IsDBNull(oRow.Item("CMNT_REALpath")) Then
            '    CMNT_REALpath = "." & CMNT_idPadre & "."
            'Else
            CMNT_path = dbRow.Item("ALCM_path")
            CMNT_REALpath = dbRow.Item("ALCM_REALpath")
            'End If


            Dim ForSubscribe As Boolean = False
            Dim ForEnter As Boolean = False
            Dim ForDetails As Boolean = True
            Dim dataStringa As String = ""

            If CMNT_id > 0 Then
                If isIscritto And RLPC_TPRL_id <> -2 And RLPC_TPRL_id <> -3 Then
                    ForEnter = True
                Else
                    Dim CMNT_dataInizioIscrizione, CMNT_dataFineIscrizione As DateTime

                    If dbRow.Item("CMNT_Iscritti") = 0 Or dbRow.Item("CMNT_Iscritti") > 0 Then
                        If IsDate(dbRow.Item("CMNT_dataInizioIscrizione")) Then
                            CMNT_dataInizioIscrizione = dbRow.Item("CMNT_dataInizioIscrizione")
                            If CMNT_dataInizioIscrizione > Now Then
                                '' devo iscrivermi, ma iscrizioni non aperte !
                                'dataStringa = oResource.getValue("iscrizioni." & Me.Iscrizioni_code.IscrizioniAperteIl)
                                'dataStringa = dataStringa.Replace("#%%#", CMNT_dataInizioIscrizione)
                                'CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & dataStringa
                            Else
                                If IsDate(dbRow.Item("CMNT_dataFineIscrizione")) Then
                                    Dim DataTemp As DateTime
                                    CMNT_dataFineIscrizione = dbRow.Item("CMNT_dataFineIscrizione")

                                    DataTemp = CMNT_dataFineIscrizione.Date()
                                    DataTemp = DataTemp.AddHours(23)
                                    DataTemp = DataTemp.AddMinutes(59)
                                    CMNT_dataFineIscrizione = DataTemp

                                    If CMNT_dataFineIscrizione < Now Then
                                        'CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & oResource.getValue("iscrizioni." & Me.Iscrizioni_code.IscrizioniChiuse)
                                    Else
                                        'dataStringa = oResource.getValue("iscrizioni." & Me.Iscrizioni_code.IscrizioniEntro)
                                        'dataStringa = dataStringa.Replace("#%%#", CMNT_dataFineIscrizione)
                                        'CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & dataStringa
                                        ForSubscribe = True
                                    End If
                                Else
                                    ForSubscribe = True
                                End If
                            End If
                        Else
                            ForSubscribe = True
                        End If
                    ElseIf RLPC_TPRL_id = -2 Then
                        ForSubscribe = True
                    Else
                        'Non c'è spazio per nuovi iscritti !!!
                        'CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & oResource.getValue("iscrizioni." & Me.Iscrizioni_code.IscrizioniComplete)
                    End If
                End If
            End If




            If CMNT_id > 0 Then
                Dim HasNews As Boolean = True
                If dbRow.Item("CMNT_CanSubscribe") = False Then
                    ForSubscribe = False
                End If
                If ForSubscribe Then
                    If dbRow.Item("CMNT_Archiviata") Or dbRow.Item("CMNT_Bloccata") Then
                        ForSubscribe = False
                    End If
                End If
                If dbRow.Item("CMNT_Bloccata") Then
                    ForEnter = False
                    HasNews = False
                End If

                If dbRow.Item("CMNT_AccessoCopisteria") = 0 And Session("objPersona").tipoPersona.id = Main.TipoPersonaStandard.Copisteria Then
                    ForSubscribe = False
                    HasNews = False
                End If

                Dim StatusID As Integer = -1
                Try
                    If Me.CBXautoUpdate.Checked Then
                        StatusID = Me.RBLstatoComunita.SelectedValue
                    Else
                        StatusID = Me.HDN_filtroStatus.Value
                    End If
                Catch ex As Exception

                End Try

               

                Select Case StatusID
                    Case 0 'attivate
                        If dbRow.Item("CMNT_Bloccata") Then
                            CMNT_Nome = CMNT_Nome & " " & Me.oResource.getValue("status.Bloccata")
                        ElseIf dbRow.Item("CMNT_Archiviata") Then
                            CMNT_Nome = CMNT_Nome & " " & Me.oResource.getValue("status.Archiviata")
                        End If
                    Case 1 ' archiviate
                        If dbRow.Item("CMNT_Bloccata") Then
                            CMNT_Nome = CMNT_Nome & " " & Me.oResource.getValue("status.Bloccata")
                        End If
                    Case 2 ' bloccate
                        If dbRow.Item("CMNT_Archiviata") Then
                            CMNT_Nome = CMNT_Nome & " " & Me.oResource.getValue("status.Archiviata")
                        End If
                End Select


               
                    Try
                        If Session("idComunita") = CMNT_id Then
                            Me.CreateContextMenu(node, False, False, False, False, False)
                        Else
                            Me.CreateContextMenu(node, ForDetails, False, ForEnter, ForSubscribe, HasNews)
                        End If
                    Catch ex As Exception

                    End Try


                If HasNews AndAlso ForEnter AndAlso Not String.IsNullOrEmpty(Me.oResource.getValue("LThasnews.text")) Then
                    Dim oCurrent As lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount = Me.PageUtility.CommunityNewsCount(Me.PageUtility.CurrentUser.ID, CMNT_id)

                    If oCurrent.Count > 0 Then
                        Try
                            If dbRow.Item("HasNews") = True And ForEnter Then
                                CMNT_Nome = CMNT_Nome & " " & Me.GenerateImage("./../images/HasNews.gif", "")
                                HasNews = True
                            End If
                        Catch ex As Exception

                        End Try

                        Dim Url As String = PageUtility.GetCommunityNewsUrl(PageUtility.CurrentUser.ID, CMNT_id, lm.Modules.NotificationSystem.Domain.ViewModeType.FromCommunityTree)
                        If Url <> "" Then
                            CMNT_Nome = CMNT_Nome & String.Format(Me.oResource.getValue("LThasnews.text"), Url, oCurrent.Count, PageUtility.BaseUrl & "images/HasNews.gif", "ROW_ItemLink_Small")
                        End If
                    End If
                End If


            End If

            node.Text = CMNT_Nome
            node.Value = CMNT_id & "," & CMNT_path

            node.Expanded = expanded
            node.ImageUrl = img
            node.ToolTip = CMNT_NomeVisibile
            node.Category = CMNT_IsChiusa


            node.Checkable = True
            If RLPC_TPRL_id = -2 Then
                node.CssClass = "TreeNodeDisabled"
            ElseIf RLPC_TPRL_id = -3 Then
                node.CssClass = "TreeNodeDisabled"
            ElseIf isIscritto = False Then
                node.CssClass = "TreeNodeDisabled"
            End If
            node.Checkable = True

        Catch ex As Exception
            Return Nothing
        End Try

        Return node
    End Function 'CreateNode


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


#Region "Filtro"
    Private Sub LNBapriFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapriFiltro.Click
        Me.TBRfiltri.Visible = True
        Me.TBRchiudiFiltro.Visible = True
        Me.TBRapriFiltro.Visible = False
    End Sub
    Private Sub LNBchiudiFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudiFiltro.Click
        Me.TBRfiltri.Visible = False
        Me.TBRchiudiFiltro.Visible = False
        Me.TBRapriFiltro.Visible = True
    End Sub

    Private Sub CBXautoUpdate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXautoUpdate.CheckedChanged
        Me.DDLannoAccademico.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLtipoCorsoDiStudi.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLperiodo.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.RBLstatoComunita.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.Bind_TreeView(True)
    End Sub
    Private Sub DDLresponsabile_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLresponsabile.SelectedIndexChanged
        If Me.CBXautoUpdate.Checked Then
            Me.Bind_TreeView(True)
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
            Me.Bind_TreeView(True)
        End If
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
        Dim oFiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.iscritto
        Try
            If Me.DDLresponsabile.SelectedValue > 0 Then
                Me.Bind_Responsabili(Me.DDLresponsabile.SelectedValue, oFiltroIscrizione)
            Else
                Me.Bind_Responsabili(-1, oFiltroIscrizione)
            End If
        Catch ex As Exception
            Me.Bind_Responsabili(-1, oFiltroIscrizione)
        End Try

        If Me.CBXautoUpdate.Checked Then
            Me.Bind_TreeView(True)
        End If
    End Sub
  
    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
        Me.Bind_Responsabili(, Main.FiltroRicercaComunitaByIscrizione.iscritto)
        If Me.CBXautoUpdate.Checked Then
            Me.Bind_TreeView(True)
        End If
    End Sub
    Private Sub RBLstatoComunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLstatoComunita.SelectedIndexChanged
        If Me.CBXautoUpdate.Checked Then
            Me.Bind_TreeView(True)
        End If
    End Sub
    Private Sub BTNCerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Me.Bind_TreeView(True)
    End Sub

#End Region

#Region "Setup Parametri Ricerca"
    Private Sub SaveSearchParameters(ByVal Visualizza As Integer)
        Try
            Me.Response.Cookies("EntrataComunita")("DDLannoAccademico") = Me.DDLannoAccademico.SelectedValue
            Try
                Me.Response.Cookies("EntrataComunita")("DDLNumeroRecord") = Me.Request.Cookies("EntrataComunita")("DDLNumeroRecord")
            Catch ex As Exception
                Me.Response.Cookies("EntrataComunita")("DDLNumeroRecord") = 15
            End Try

            Me.Response.Cookies("EntrataComunita")("DDLorganizzazione") = Me.DDLorganizzazione.SelectedValue
            Me.Response.Cookies("EntrataComunita")("DDLperiodo") = Me.DDLperiodo.SelectedValue
            Me.Response.Cookies("EntrataComunita")("DDLTipo") = Me.DDLTipo.SelectedValue
            Me.Response.Cookies("EntrataComunita")("DDLTipoRicerca") = Me.DDLTipoRicerca.SelectedValue
            Me.Response.Cookies("EntrataComunita")("TXBValore") = Me.TXBValore.Text
            Me.Response.Cookies("EntrataComunita")("intCurPage") = Me.ViewState("intCurPage")
            Me.Response.Cookies("EntrataComunita")("SortDirection") = Me.ViewState("SortDirection")
            Me.Response.Cookies("EntrataComunita")("SortExspression") = Me.ViewState("SortExspression")
            Try
                Me.Response.Cookies("EntrataComunita")("CBXmostraPadre") = Me.Request.Cookies("CBXmostraPadre")("DDLNumeroRecord")
            Catch ex As Exception
                Me.Response.Cookies("EntrataComunita")("CBXmostraPadre") = False
            End Try
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
                Me.TXBValore.Text = Me.Request.Cookies("EntrataComunita")("TXBValore")
            Catch ex As Exception
                Me.TXBValore.Text = ""
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
                ' If IsNumeric(Me.Request.Cookies("ListaComunita")("tipo")) Then
                Me.DDLTipo.SelectedValue = Me.Request.Cookies("EntrataComunita")("DDLTipo")
            Catch ex As Exception

            End Try
            Me.TBLcorsi.Visible = False
            Me.TBLcorsiDiStudio.Visible = False
            Me.LBnoCorsi.Visible = True
            ' Setto il tipo di ricerca
            Try
                If IsNumeric(Me.Request.Cookies("EntrataComunita")("DDLTipoRicerca")) Then
                    Me.DDLTipoRicerca.SelectedValue = Me.Request.Cookies("EntrataComunita")("DDLTipoRicerca")
                End If
            Catch ex As Exception
            End Try
        Catch ex As Exception

        End Try
    End Sub
#End Region



#Region "Gestione Comunità"
    'Visualizzo i dettagli della comunità
    Private Sub VisualizzaDettagli(ByVal CMNT_Path As String)
        Dim CMNT_ID As Integer
        Dim Elenco() As String

        Elenco = CMNT_Path.Split(".")
        CMNT_ID = Elenco(UBound(Elenco) - 1)
        Try
            Dim oComunita As New COL_Comunita
            Me.ResetFormAll()
            Me.PNLdettagli.Visible = True
            Me.PNLmenuDettagli.Visible = True

            Me.CTRLDettagli.SetupDettagliComunita(CMNT_ID)

            oComunita.Id = CMNT_ID
            oComunita.Estrai()
            GenericCacheManager.PurgeCacheItems(CachePolicy.PermessiServizioUtente())
            If oComunita.Errore = Errori_Db.None Then
                Me.HDN_Path.Value = CMNT_Path
                If CMNT_ID = Session("idComunita") Then
                    Me.LNBentra.Visible = False
                    Me.LNBiscrivi.Visible = False
                Else
                    Dim oRuolo As New COL_RuoloPersonaComunita
                    Dim canSubscribe As Boolean = True
                    Dim canEnter As Boolean = False


                    oRuolo.EstraiByLinguaDefault(CMNT_ID, Session("objPersona").id)
                    If oRuolo.Errore = Errori_Db.None Then
                        If oRuolo.TipoRuolo.Id > 0 Then
                            canSubscribe = False
                            canEnter = (oRuolo.Abilitato And oRuolo.Attivato)
                        Else
                            canEnter = False
                            canSubscribe = oComunita.CanSubscribe
                        End If
                    Else
                        canEnter = False
                        canSubscribe = oComunita.CanSubscribe
                    End If
                    If oComunita.DataInizioIscrizione > Now Then
                        canSubscribe = False
                    Else
                        If Not Equals(New Date, oComunita.DataFineIscrizione) Then
                            Dim DataTemp As DateTime
                            DataTemp = oComunita.DataFineIscrizione.Date
                            DataTemp = DataTemp.AddHours(23)
                            DataTemp = DataTemp.AddMinutes(59)
                            If DataTemp < Now Then
                                canSubscribe = False
                            End If
                        End If
                    End If

                    If canSubscribe Then
                        Me.LNBiscrivi.Visible = True
                        Me.LNBiscrivi.Enabled = Not (oComunita.Bloccata Or oComunita.Archiviata)
                    Else
                        Me.LNBiscrivi.Visible = False
                    End If
                    If canEnter Then
                        Me.LNBentra.Visible = True
                        Me.LNBentra.Enabled = Not (oComunita.Bloccata)
                    Else
                        Me.LNBentra.Visible = False
                    End If
                End If
            Else
                Me.LNBiscrivi.Visible = False
                Me.LNBentra.Visible = False
            End If
        Catch ex As Exception
            Me.ResetForm(False)
        End Try

    End Sub
    Private Sub Entra_Comunita(ByVal CMNT_PATH As String)
        'Dim oResourceConfig As New ResourceManager
        'oResourceConfig = GetResourceConfig(Session("LinguaCode"))
        'Dim status As lm.Comol.Core.DomainModel.SubscriptionStatus
        'Dim idPerson As Integer = PageUtility.CurrentUser.ID
        'Dim array() As String = {"."}
        'Dim idCommunity As Integer = 0
        'idCommunity = (From s In (path.Split(array, StringSplitOptions.RemoveEmptyEntries)) Select CInt(s)).ToList().FirstOrDefault()


        'Dim oComunita As New COL_Comunita With {.Id = idCommunity}
        'oComunita.Estrai()
        'oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona



        'Dim oTreeComunita As New COL_TreeComunita
        'Try
        '    oTreeComunita.Directory = Server.MapPath(PageUtility.BaseUrl & "profili/") & idPerson & "\"
        '    oTreeComunita.Nome = idPerson & ".xml"
        'Catch ex As Exception

        'End Try

        'If oComunita.Errore = Errori_Db.None Then
        '    Dim oRuolo As New COL_RuoloPersonaComunita

        '    oRuolo.EstraiByLinguaDefault(idCommunity, idPerson)
        '    status = PageUtility.AccessToCommunity(idPerson, idCommunity, path, oResourceConfig, True)
        '    Select Case status
        '        Case lm.Comol.Core.DomainModel.SubscriptionStatus.activemember
        '            Exit Sub
        '        Case lm.Comol.Core.DomainModel.SubscriptionStatus.blocked
        '            Me.Reset_ToMessaggi()
        '            Me.HDN_Path.Value = ""
        '            oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.bloccato)
        '            oTreeComunita.CambiaAttivazione(idCommunity, False, oResource)

        '        Case lm.Comol.Core.DomainModel.SubscriptionStatus.waiting
        '            Me.Reset_ToMessaggi()
        '            Me.HDN_Path.Value = ""
        '            oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.inAttesa)
        '            oTreeComunita.CambiaAbilitazione(idCommunity, False)

        '        Case lm.Comol.Core.DomainModel.SubscriptionStatus.communityblocked
        '            'Spiacente, non si ha accesso alla comunità
        '            Me.Reset_ToMessaggi()
        '            Me.HDN_Path.Value = ""
        '            oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.errore)
        '            oTreeComunita.CambiaIsBloccata(idCommunity, True)
        '        Case Else
        '            Me.Reset_ToMessaggi()
        '            Me.HDN_Path.Value = ""
        '    End Select
        'Else
        '    ' la comunità non esiste più !!
        '    oTreeComunita.Delete(idCommunity, path)
        '    Me.Reset_ToMessaggi()

        '    oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.noCommunity)
        '    '   Me.LBmessaggio.Text = "ATTENZIONE: errore di accesso al sistema, la comunità prescelta sembra non essere presente nel sistema."
        '    Me.HDN_Path.Value = ""
        'End If



        Dim CMNT_ID, PRSN_ID As Integer
        Dim Elenco_CMNT_ID() As String
        Dim oTreeComunita As New COL_TreeComunita
        Dim oPersona As New COL_Persona
        Dim oComunita As New COL_Comunita

        Elenco_CMNT_ID = CMNT_PATH.Split(".")
        CMNT_ID = Elenco_CMNT_ID(UBound(Elenco_CMNT_ID) - 1)

        Try

            oPersona = Session("objPersona")
            PRSN_ID = oPersona.ID

            oTreeComunita.Directory = Server.MapPath("./../profili/") & PRSN_ID & "\"
            oTreeComunita.Nome = PRSN_ID & ".xml"
        Catch ex As Exception

        End Try


        Try
            oComunita.Id = CMNT_ID
            oComunita.Estrai()
            oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona
            If oComunita.Errore = Errori_Db.None Then
                Dim i, j, totale, ORGN_ID, RuoloID As Integer
                Dim oRuolo As New COL_RuoloPersonaComunita

                oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)

                If oRuolo.Errore = Errori_Db.None Then
                    RuoloID = oRuolo.Id

                    If oRuolo.Attivato And oRuolo.Abilitato Then
                        Me.PageUtility.AddAction(ActionType.Access, Me.PageUtility.CreateObjectsList(ObjectType.Community, CMNT_ID), Nothing)
                        With oComunita
                            Session("IdComunita") = CMNT_ID
                            Session("ORGN_id") = .Organizzazione.Id
                            Session("RLPC_id") = oRuolo.Id
                            Session("IdRuolo") = oRuolo.TipoRuolo.Id

                            'carico il ruolo che la persona adempie nella comunità selezionata
                            Try

                                Dim oServizio As New COL_Servizio
                                Dim oDataSet As New DataSet
                                oDataSet = oServizio.ElencaByTipoRuoloByComunita(Session("IdRuolo"), CMNT_ID)
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
                            Catch ex As Exception

                            End Try

                            Try
                                If Session("LogonAs") = False Then
                                    oRuolo.UpdateUltimocollegamento()
                                End If

                            Catch ex As Exception

                            End Try

                            'Aggiorno gli array relativi al menu history !!!

                            Dim ArrComunita(,) As String
                            Dim tempArray(,), Path As String

                            ' RIMOSSO PER PROBLEMA history.back()
                            j = 0
                            For i = 0 To UBound(Elenco_CMNT_ID) - 1

                                If IsNumeric(Elenco_CMNT_ID(i)) Then
                                    If Elenco_CMNT_ID(i) > 0 Then
                                        ReDim Preserve ArrComunita(3, j)
                                        ArrComunita(0, j) = Elenco_CMNT_ID(i)
                                        ArrComunita(1, j) = COL_Comunita.EstraiNomeBylingua(Elenco_CMNT_ID(i), Session("LinguaID"))

                                        If Path = "" Then
                                            Path = "." & Elenco_CMNT_ID(i) & "."
                                        Else
                                            Path = Path & Elenco_CMNT_ID(i) & "."
                                        End If
                                        ArrComunita(2, j) = Path

                                        ' Ruolo svolto..........
                                        ArrComunita(3, j) = oPersona.GetIDRuoloForComunita(Elenco_CMNT_ID(i))
                                        j = j + 1
                                    End If
                                End If
                            Next
                            Session("ArrComunita") = ArrComunita
                            Session("limbo") = False

                        End With

                        Session("RLPC_ID") = oRuolo.Id
                        oComunita.RegistraAccesso(CMNT_ID, PRSN_ID, Me.PageUtility.SystemSettings.CodiceDB)
                        Me.PageUtility.SendNotificationUpdateCommunityAccess(PRSN_ID, CMNT_ID, oRuolo.UltimoCollegamento)
                        oTreeComunita.Update(oComunita, CMNT_PATH, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)

                        Session("AdminForChange") = False
                        Session("CMNT_path_forAdmin") = ""
                        Session("idComunita_forAdmin") = ""

                        ' REGISTRAZIONE EVENTO
                        Session("TPCM_ID") = oComunita.TipoComunita.ID


                        Dim defaultUrl As String = PageUtility.GetCommunityDefaultPage(CMNT_ID, PRSN_ID)
                        If oComunita.ShowCover(CMNT_ID, PRSN_ID) Then
                            If oRuolo.SaltaCopertina Then
                                Me.PageUtility.RedirectToUrl(defaultUrl)
                            Else
                                Me.PageUtility.RedirectToUrl("Generici/Cover.aspx")
                            End If
                        Else
                            Me.PageUtility.RedirectToUrl(defaultUrl)
                        End If


                    Else
                        ' non si ha accesso alla comunità
                        Me.Reset_ToMessaggi()
                        Me.HDN_Path.Value = ""
                        If Not (oRuolo.Attivato) Then
                            oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.inAttesa)
                            'Me.LBmessaggio.Text = "ATTENZIONE: non è possibile accedere alla comunità selezionata, si è in attesa di attivazione da parte del relativo amministratore/responsabile."
                        ElseIf Not oRuolo.Abilitato Then
                            oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.bloccato)
                            'Me.LBmessaggio.Text = "ATTENZIONE: non è possibile accedere alla comunità selezionata, l'accesso è stato bloccato dal relativo amministratore/responsabile."
                        Else
                            oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.errore)
                            'Me.LBmessaggio.Text = "ATTENZIONE: l'accesso alla comunità non è al momento consentito."
                        End If
                        oTreeComunita.Update(oComunita, CMNT_PATH, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)
                    End If
                Else
                    'Spiacente, non si ha accesso alla comunità
                    Me.Reset_ToMessaggi()
                    Me.HDN_Path.Value = ""
                    oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.errore)

                    ' la comunità non esiste più o non si è più iscritti !
                    oTreeComunita.Delete(CMNT_ID, CMNT_PATH)
                End If
            Else
                ' la comunità non esiste più !!
                oTreeComunita.Delete(CMNT_ID, CMNT_PATH)
                Me.Reset_ToMessaggi()

                oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.noCommunity)
                '   Me.LBmessaggio.Text = "ATTENZIONE: errore di accesso al sistema, la comunità prescelta sembra non essere presente nel sistema."
                Me.HDN_Path.Value = ""
            End If
        Catch ex As Exception
            Me.Reset_ToMessaggi()
            Me.HDN_Path.Value = ""
            oResource.setLabel_To_Value(Me.LBmessaggi, "abilitato." & Me.StringaAbilitato.errore)
        End Try
    End Sub
#End Region

#Region "Gestione TreeView"

    Private Sub RDTcomunita_NodeContextClick(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles RDTcomunita.NodeContextClick
        Dim ComunitaPath As String
        Dim ComunitaID As Integer
        Dim ArrayDati As String()
        Dim oNode As Telerik.WebControls.RadTreeNode
        oNode = e.NodeClicked

        Try
            Dim isChiusa As Boolean
            Dim oComunita As New COL_Comunita

            ' Il path reale (nel caso in cui sia organizzativo non c'è quindi il -1, ecc ecc !)
            ArrayDati = oNode.Value.Split(",")
            ComunitaID = ArrayDati(0)
            ComunitaPath = ArrayDati(1)
            isChiusa = oNode.Category

            oComunita.Id = ComunitaID
            oComunita.Estrai()
            If InStr(ComunitaPath, "-") > 0 Then
                Dim start, endRiga As Integer
                start = InStr(ComunitaPath, "-") - 1
                endRiga = InStr(start + 1, ComunitaPath, ".")
                ComunitaPath = ComunitaPath.Remove(start, endRiga - start)
            End If
            Select Case e.ContextMenuItemID
                Case AzioneTree.Aggiorna
                    Me.Bind_TreeView()
                Case AzioneTree.Dettagli
                    Me.HDNcmnt_ID.Value = ComunitaID
                    Me.HDN_Path.Value = ComunitaPath
                    Me.HDisChiusa.Value = isChiusa
                    Me.VisualizzaDettagli(ComunitaPath)
                Case AzioneTree.Entra
                    Dim canEnter As Boolean = False
                    Dim alertMSG As String
                    Try
                        If oComunita.Bloccata Then
                            alertMSG = oResource.getValue("entra.isBloccata")
                            If alertMSG <> "" Then
                                alertMSG = alertMSG.Replace("'", "\'")
                            Else
                                alertMSG = "Non è possibile entrare in una comunità il cui status è impostato su Bloccata."
                            End If

                            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                        Else
                            canEnter = True
                        End If

                    Catch ex As Exception

                    End Try
                    If canEnter Then
                        Me.HDNcmnt_ID.Value = ComunitaID
                        Me.HDN_Path.Value = ComunitaPath
                        Me.HDisChiusa.Value = isChiusa
                        Session("Azione") = "entra"
                        Me.PNLmessaggi.Visible = False
                        Me.Entra_Comunita(ComunitaPath)
                    End If

                Case AzioneTree.Iscrivi
                    Dim canEnter As Boolean = False
                    Dim alertMSG As String
                    Try
                        If oComunita.isBloccata() Then
                            alertMSG = oResource.getValue("iscrivi.isBloccata")
                            If alertMSG <> "" Then
                                alertMSG = alertMSG.Replace("'", "\'")
                            Else
                                alertMSG = "Non è possibile iscriversi ad una comunità il cui status è impostato su Bloccata."
                            End If

                            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")

                        ElseIf oComunita.isArchiviata() Then
                            alertMSG = oResource.getValue("iscrivi.isArchiviata")
                            If alertMSG <> "" Then
                                alertMSG = alertMSG.Replace("'", "\'")
                            Else
                                alertMSG = "Non è possibile iscriversi ad una comunità il cui status è impostato su Archiviata."
                            End If

                            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                        Else
                            canEnter = True
                        End If

                    Catch ex As Exception

                    End Try
                    If canEnter Then
                        Me.HDNcmnt_ID.Value = ComunitaID
                        Me.HDN_Path.Value = ComunitaPath
                        Me.HDisChiusa.Value = isChiusa
                        Session("Azione") = "iscrivi"

                        Dim oImpostazioni As New COL_ImpostazioniUtente
                        Dim exitSub As Boolean = False
                        Try
                            oImpostazioni = Session("oImpostazioni")
                            exitSub = Not oImpostazioni.ShowConferma
                        Catch ex As Exception
                            exitSub = False
                        End Try

                        If Not exitSub Then
                            Session("azione") = "iscrivi"
                            Me.ResetFormToConferma(oNode.ToolTip, oComunita.GetNomeResponsabile_NomeCreatore())
                        Else
                            If Session("azione") <> "iscrivi" Then
                                Dim iResponse As Main.ErroriIscrizioneComunita
                                Dim oUtility As New OLDpageUtility(Me.Context)
                                Dim oPersona As COL_Persona

                                Session("azione") = "iscrivi"
                                oPersona = Session("objPersona")
                                iResponse = oPersona.IscrizioneComunitaNew(ComunitaID, ComunitaPath, isChiusa, Server.MapPath("./../profili/") & oPersona.ID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                                If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
                                    Me.Reset_ToMessaggi()
                                    Me.LBmessaggi.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                                Else
                                    Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                                    If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                                        oServiceUtility.NotifyAddSelfSubscription(ComunitaID, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                                    Else
                                        oServiceUtility.NotifyAddWaitingSubscription(ComunitaID, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                                    End If
                                    Me.LBiscrizione.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                                    Me.Reset_ToIscrizioneAvvenuta()
                                End If
                            Else
                                Session("azione") = "loaded"
                                Me.ResetForm(False)
                            End If
                        End If
                    End If

                Case AzioneTree.Novità
                    If Me.LNBalbero.Visible Then
                        Me.SaveSearchParameters(2)
                    Else
                        Me.SaveSearchParameters(1)
                    End If

                    Dim Elenco() As String
                    Dim CMNT_ID As Integer
                    Elenco = ComunitaPath.Split(".")
                    CMNT_ID = Elenco(UBound(Elenco) - 1)

                    Session("CMNT_path_forNews") = ComunitaPath
                    Session("CMNT_ID_forNews") = CMNT_ID
                    Me.Response.Redirect("./../generici/News_Comunita.aspx?from=NavigazioneTreeView", True)
            End Select
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Reset Form"
    Private Sub ResetFormAll()
        Me.PNLdettagli.Visible = False
        Me.PNLmessaggi.Visible = False
        Me.PNLtreeView.Visible = False
        Me.PNLconferma.Visible = False
        Me.PNLiscrizioneAvvenuta.Visible = False

        Me.PNLmenuAccesso.Visible = False
        Me.PNLmenu.Visible = False
        Me.PNLmenuAccesso.Visible = False
        Me.PNLmenuDettagli.Visible = False
        Me.PNLmenuConferma.Visible = False
    End Sub
    Private Sub ResetForm(ByVal updateTree As Boolean)
        Me.ResetFormAll()
        Me.HDN_Path.Value = ""
        Me.PNLtreeView.Visible = True
        Me.PNLmenu.Visible = True
        Session("azione") = "loaded"
        If updateTree Then
            Me.Bind_TreeView()
        End If
    End Sub
    Private Sub Reset_ToIscrizioneAvvenuta()
        Me.ResetFormAll()
        Me.PNLiscrizioneAvvenuta.Visible = True
        Me.PNLmenuAccesso.Visible = True
    End Sub
    Private Sub Reset_ToMessaggi()
        Me.ResetFormAll()
        Me.PNLmessaggi.Visible = True
        Me.PNLmenuAccesso.Visible = True
    End Sub

    Private Sub ResetFormToConferma(ByVal Comunita As String, ByVal Responsabile As String)
        Me.ResetFormAll()
        Me.PNLconferma.Visible = True
        Me.PNLmenuConferma.Visible = True
        Me.oResource.setLabel(Me.LBconferma)
        Me.LBconferma.Text = Replace(Me.LBconferma.Text, "#nomeComunita#", Comunita)
        Me.LBconferma.Text = Replace(Me.LBconferma.Text, "#nomeResponsabile#", Responsabile)
    End Sub


#End Region
#Region "Pannello Dettagli"

    Private Sub LNBentra_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBentra.Click
        Me.Entra_Comunita(Me.HDN_Path.Value)
    End Sub
    Private Sub LNBiscrivi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscrivi.Click
        Dim oImpostazioni As New COL_ImpostazioniUtente
        Dim exitSub As Boolean = False
        Try
            oImpostazioni = Session("oImpostazioni")
            exitSub = Not oImpostazioni.ShowConferma
        Catch ex As Exception
            exitSub = False
        End Try
        Me.PNLdettagli.Visible = False

        If Not exitSub Then
            Session("azione") = "iscrivi"
            Dim oComunita As New COL_Comunita
            oComunita.Id = Me.HDNcmnt_ID.Value
            Me.ResetFormToConferma(oComunita.EstraiNomeBylingua(Session("LinguaID")), oComunita.GetNomeResponsabile_NomeCreatore())
        Else
            If Session("azione") <> "iscrivi" Then
                Session("azione") = "iscrivi"
                Try
                    Dim iResponse As Main.ErroriIscrizioneComunita
                    Dim oPersona As New COL_Persona
                    Dim oUtility As New OLDpageUtility(Me.Context)

                    oPersona = Session("objPersona")

                    iResponse = oPersona.IscrizioneComunitaNew(Me.HDNcmnt_ID.Value, Me.HDN_Path.Value, Me.HDisChiusa.Value, Server.MapPath("./../profili/") & oPersona.ID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                    If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
                        Me.Reset_ToMessaggi()
                        Me.LBmessaggi.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                    Else
                        Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                        If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                            oServiceUtility.NotifyAddSelfSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                        Else
                            oServiceUtility.NotifyAddWaitingSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                        End If
                        Me.LBiscrizione.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                        Me.Reset_ToIscrizioneAvvenuta()
                    End If
                    Me.HDNcmnt_ID.Value = ""
                    Me.HDisChiusa.Value = ""
                    Me.HDN_Path.Value = ""
                Catch ex As Exception
                    Session("azione") = "loaded"
                    Me.ResetForm(True)
                End Try
            Else
                Session("azione") = "loaded"
                Me.ResetForm(True)
            End If
        End If
    End Sub

    Private Sub LNBannullaDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaDettagli.Click
        Me.ResetForm(False)
    End Sub
#End Region



#Region "Gestione Albero"
    Private Function CreateContextMenu(ByVal childNode As RadTreeNode, ByVal ForDetails As Boolean, ByVal ForUpdate As Boolean, Optional ByVal ForEntra As Boolean = False, Optional ByVal ForIscrivi As Boolean = False, Optional ByVal HasNews As Boolean = False)
        Dim contextMenus As New ArrayList
        Dim nodeMenu As New Telerik.WebControls.RadTreeViewContextMenu.ContextMenu

        Dim NomeContextMenu As String = "_"

        Dim iMenuDettagli As RadTreeViewContextMenu.ContextMenuItem
        Dim iMenuNews As RadTreeViewContextMenu.ContextMenuItem
        Dim iMenuEntra As RadTreeViewContextMenu.ContextMenuItem
        Dim iMenuAggiorna As RadTreeViewContextMenu.ContextMenuItem
        Dim iMenuIscrivi As RadTreeViewContextMenu.ContextMenuItem
        If ForDetails Then
            iMenuDettagli = New RadTreeViewContextMenu.ContextMenuItem
            iMenuDettagli.Image = "./images/12.gif"
            iMenuDettagli.PostBack = True
            iMenuDettagli.ID = 2
            iMenuDettagli.Text = oResource.getValue("menu." & Me.AzioneTree.Dettagli)
            NomeContextMenu = NomeContextMenu & "1_"
        End If
        If HasNews Then
            iMenuNews = New RadTreeViewContextMenu.ContextMenuItem
            iMenuNews.Image = "./images/0.gif"
            iMenuNews.PostBack = True
            iMenuNews.ID = 5
            iMenuNews.Text = oResource.getValue("menu." & Me.AzioneTree.Novità)
            NomeContextMenu = NomeContextMenu & "5_"
        End If
        If ForUpdate Then
            iMenuAggiorna = New RadTreeViewContextMenu.ContextMenuItem
            iMenuAggiorna.Image = "./images/14.gif"
            iMenuAggiorna.PostBack = True
            iMenuAggiorna.ID = 1
            iMenuAggiorna.Text = oResource.getValue("menu." & Me.AzioneTree.Aggiorna)
            NomeContextMenu = NomeContextMenu & "2_"
        End If
        If ForEntra Then
            iMenuEntra = New RadTreeViewContextMenu.ContextMenuItem
            iMenuEntra.Image = "./images/0.gif"
            iMenuEntra.PostBack = True
            iMenuEntra.ID = 3
            iMenuEntra.Text = oResource.getValue("menu." & Me.AzioneTree.Entra)
            NomeContextMenu = NomeContextMenu & "3_"
        End If
        If ForIscrivi Then
            iMenuIscrivi = New RadTreeViewContextMenu.ContextMenuItem
            iMenuIscrivi.Image = "./images/4.gif"
            iMenuIscrivi.PostBack = True
            iMenuIscrivi.ID = 4
            iMenuIscrivi.Text = oResource.getValue("menu." & Me.AzioneTree.Iscrivi)
            NomeContextMenu = NomeContextMenu & "4_"
        End If

        If Not IsNothing(iMenuEntra) Then
            nodeMenu.Items.Add(iMenuEntra)
        End If
        If Not IsNothing(iMenuIscrivi) Then
            nodeMenu.Items.Add(iMenuIscrivi)
        End If
        If Not IsNothing(iMenuAggiorna) Then
            nodeMenu.Items.Add(iMenuAggiorna)
        End If
        If Not IsNothing(iMenuIscrivi) Or Not IsNothing(iMenuEntra) Then
            If Not IsNothing(iMenuDettagli) Then
                Dim iMenuBlanck As New RadTreeViewContextMenu.ContextMenuItem
                iMenuBlanck.Text = ""
                iMenuBlanck.Image = ""
                iMenuBlanck.PostBack = False
                nodeMenu.Items.Add(iMenuBlanck)

                nodeMenu.Items.Add(iMenuDettagli)
            End If
        ElseIf Not IsNothing(iMenuDettagli) Then
            nodeMenu.Items.Add(iMenuDettagli)
        End If
        If Not IsNothing(iMenuNews) Then
            nodeMenu.Items.Add(iMenuNews)
        End If

        contextMenus = Me.RDTcomunita.ContextMenus
        If contextMenus.Count = 0 Then
            nodeMenu.Name = NomeContextMenu
            contextMenus.Add(nodeMenu)
        Else
            Dim i, totale As Integer
            Dim found As Boolean = False
            totale = contextMenus.Count - 1
            For i = 0 To contextMenus.Count - 1
                If contextMenus.Item(i).Name = NomeContextMenu Then
                    found = True
                    Exit For
                End If
            Next
            If Not found Then
                nodeMenu.Name = NomeContextMenu
                contextMenus.Add(nodeMenu)
            End If
        End If
        childNode.ContextMenuName = NomeContextMenu
        Me.RDTcomunita.ContextMenus = contextMenus
    End Function
#End Region

    Private Sub LNBalberoGerarchico_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBalberoGerarchico.Click
        Me.SaveSearchParameters(2)
        Me.LNBalberoGerarchico.Visible = False
        Me.LNBalbero.Visible = True
        Me.Bind_TreeView(True)
    End Sub
    Private Sub LNBalbero_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBalbero.Click
        Me.SaveSearchParameters(1)
        Me.LNBalberoGerarchico.Visible = True
        Me.LNBalbero.Visible = False
        Me.Bind_TreeView(True)
    End Sub
    Private Sub LNBlista_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBlista.Click
        Me.SaveSearchParameters(3)
        PageUtility.RedirectToUrl("Comunita/EntrataComunita.aspx?forchange=true")

    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_NavigazioneComunita"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(Me.LBNopermessi)
            .setLabel(Me.LBlegend)

            .setLabel(Me.LBorganizzazione_c)
            .setLabel(Me.LBannoAccademico_c)
            .setLabel(Me.LBperiodo_c)

            .setLinkButton(LNBaggiorna, True, True)
            .setLinkButton(Me.LNBespandi, True, True)
            .setLinkButton(Me.LNBcomprimi, True, True)
            Me.LNBcomprimi.Attributes.Add("onclick", "window.status='';CollapseAll();return false;")
            Me.LNBespandi.Attributes.Add("onclick", "window.status='';ExpandAll();return false;")
            .setLabel(Me.LBavviso)

            .setLinkButton(Me.LNBlista, True, True)
            .setLinkButton(Me.LNBalbero, True, True)
            .setLinkButton(Me.LNBalberoGerarchico, True, True)
            .setLinkButton(Me.LNBentra, True, True)
            .setLinkButton(Me.LNBiscrivi, True, True)
            .setLinkButton(Me.LNBannullaDettagli, True, True)
            .setLinkButton(Me.LNBannulla, True, True)
            .setLabel(Me.LBstatoComunita_t)

            .setCheckBox(Me.CBXautoUpdate)
            .setRadioButtonList(Me.RBLstatoComunita, -1)
            .setRadioButtonList(Me.RBLstatoComunita, 0)
            .setRadioButtonList(Me.RBLstatoComunita, 1)
            .setRadioButtonList(Me.RBLstatoComunita, 2)

            .setDropDownList(Me.DDLTipoRicerca, -2)
            .setDropDownList(Me.DDLTipoRicerca, -7)
            .setDropDownList(Me.DDLTipoRicerca, -5)
            .setDropDownList(Me.DDLTipoRicerca, -6)
            .setDropDownList(Me.DDLTipoRicerca, -3)
            .setDropDownList(Me.DDLTipoRicerca, -4)
            .setDropDownList(Me.DDLTipoRicerca, -8)
            .setDropDownList(Me.DDLTipoRicerca, -9)

            .setLabel(Me.LBcorsoDiStudi_t)
            .setLinkButton(Me.LNBapriFiltro, True, True)
            .setLinkButton(Me.LNBchiudiFiltro, True, True)
            .setLabel(Me.LBtipoComunita_c)
            .setButton(Me.BTNCerca, True)



            .setLabel(Me.LBannoAccademico_c)
            .setLabel(Me.LBperiodo_c)
            .setLabel(Me.LBtipoComunita_c)
            .setLabel(Me.LBtipoRicerca_c)
            .setLabel(Me.LBvalore_c)
            .setLabel(Me.LBcorsoDiStudi_t)


            .setLinkButton(Me.LNBlista, True, True)
            .setLinkButton(Me.LNBalbero, True, True)
            .setLinkButton(Me.LNBalberoGerarchico, True, True)
            .setLinkButton(Me.LNBentra, True, True)
            .setLinkButton(Me.LNBiscrivi, True, True)
            .setLinkButton(Me.LNBannullaDettagli, True, True)
            .setLabel(Me.LBstatoComunita_t)

            .setCheckBox(Me.CBXautoUpdate)
            .setRadioButtonList(Me.RBLstatoComunita, -1)
            .setRadioButtonList(Me.RBLstatoComunita, 0)
            .setRadioButtonList(Me.RBLstatoComunita, 1)
            .setRadioButtonList(Me.RBLstatoComunita, 2)

            .setLinkButton(Me.LNBapriFiltro, True, True)
            .setLinkButton(Me.LNBchiudiFiltro, True, True)
            .setLinkButton(Me.LNBiscriviConferma, True, True)
            .setLinkButton(Me.LNBannullaConferma, True, True)
        End With

    End Sub
#End Region

    Private Sub LNBiscriviConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviConferma.Click
        Dim iResponse As Main.ErroriIscrizioneComunita
        Dim oPersona As New COL_Persona

        If Session("azione") = "iscrivi" Then
            Me.PNLconferma.Visible = False
            Try
                Dim oUtility As New OLDpageUtility(Me.Context)
                oPersona = Session("objPersona")

                iResponse = oPersona.IscrizioneComunitaNew(Me.HDNcmnt_ID.Value, Me.HDN_Path.Value, Me.HDisChiusa.Value, Server.MapPath("./../profili/") & oPersona.ID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
				If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
					Me.Reset_ToMessaggi()
					Me.LBmessaggi.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                Else
                    Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                    If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                        oServiceUtility.NotifyAddSelfSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                    Else
                        oServiceUtility.NotifyAddWaitingSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                    End If
                    Me.LBiscrizione.Text = Me.oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                    Me.Reset_ToIscrizioneAvvenuta()
				End If
				Me.HDNcmnt_ID.Value = ""
				Me.HDisChiusa.Value = ""
				Me.HDN_Path.Value = ""
			Catch ex As Exception
				Me.ResetForm(False)
			End Try
		Else
			Session("azione") = "loaded"
			Me.ResetForm(False)
		End If
	End Sub
	Private Sub LNBannullaConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaConferma.Click
		Me.ResetForm(False)
	End Sub

	Private Sub LNBannulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannulla.Click
		Me.ResetForm(True)
	End Sub

Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_ElencaComunita.Codex)
    End Sub


    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class