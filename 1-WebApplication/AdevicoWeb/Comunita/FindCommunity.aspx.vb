Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita

Imports COL_BusinessLogic_v2.UCServices
'Imports COL_BusinessLogic_v2.UCServices.Services_IscrizioneComunita
Imports Comunita_OnLine.ModuloGenerale
Imports lm.ActionDataContract
Imports COL_BusinessLogic_v2.UCServices.Services_ElencaComunita

Partial Public Class FindCommunity
    Inherits PageBase


    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property

#Region "OLD"
    Private Enum Iscrizioni_code
        IscrizioniAperteIl = 0
        IscrizioniChiuse = 1
        IscrizioniComplete = 2
        IscrizioniEntro = 3
    End Enum
    Private Enum stringaRegistrazione
        errore = 0
        inAttesa = 1
        limiteIscrizione = 2
        iscritto = 3
    End Enum
    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum
    Private Enum stringaTitolo
        forSubscribe = 0
        Subscribed = 1
        standard = 2
    End Enum
    Private Enum StringaElenco
        noCommunityForFilter = 0
        noCommunity = 1
    End Enum
    Private Enum stringaMessaggio
        seleziona = 0
        indietro = 1
    End Enum
    Private Enum StringaAbilitato
        abilitato = 1
        bloccato = 0
        inAttesa = -1
        comunitaArchiviata = 2
        comunitaBloccata = 3
    End Enum

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Page.Form.DefaultFocus = Me.TXBValore.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.TXBValore.UniqueID
    End Sub


    Private Function isLoadedForSubscription() As Boolean
        If Me.Request.QueryString("cerca") = "noniscritto" Then
            Me.RBLricercaByIscrizione.SelectedValue = 0
            Me.Master.ServiceTitle = Me.Resource.getValue("LBtitolo." & stringaTitolo.forSubscribe)
            'Me.Resource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.forSubscribe)
            Return True
        ElseIf Me.Request.QueryString("cerca") = "iscritto" Then
            Me.RBLricercaByIscrizione.SelectedValue = 1
            Me.Master.ServiceTitle = Me.Resource.getValue("LBtitolo." & stringaTitolo.Subscribed)
            'Me.Resource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.Subscribed)
            Return False
        Else
            Me.RBLricercaByIscrizione.SelectedValue = 0
            Me.Master.ServiceTitle = Me.Resource.getValue("LBtitolo." & stringaTitolo.forSubscribe)
            'Me.Resource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.forSubscribe)
            Return True
        End If
        Return True
    End Function
#Region "Inherited"
    Public Overrides Sub BindDati()
        Try
            Dim isForSubscription As Boolean = isLoadedForSubscription()
            Session("CMNT_path_forNews") = ""
            Session("CMNT_ID_forNews") = ""
            Session("azione") = "load"
            Session("AdminForChange") = False
            Session("idComunita_forAdmin") = ""
            Session("CMNT_path_forAdmin") = ""

            Me.Reset_Contenuto(False, isForSubscription)
            If Me.Request.QueryString("re_set") <> "true" Then
                Me.ViewState("intCurPage") = 0
                Me.ViewState("intAnagrafica") = "-1"
                Me.ViewState("SortExspression") = "CMNT_Nome"
                Me.ViewState("SortDirection") = "asc"
                Me.TBRchiudiFiltro.Visible = True
                Me.TBRfiltri.Visible = True
            End If

            Me.SetupFiltri()

            If isForSubscription Then
                Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_IscrizioneComunita.Codex).ID, Services_IscrizioneComunita.ActionType.CommunityList)
            Else
                Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_ElencaComunita.Codex).ID, Services_ElencaComunita.ActionType.List)
            End If
        Catch ex As Exception

        End Try

    End Sub
    Public Overrides Sub BindNoPermessi()
        Dim isForSubscription As Boolean = isLoadedForSubscription()
        Dim oSrvList As Services_ElencaComunita = Me.ImpostaPermessiElenco
        Dim oSrvSubscribe As Services_IscrizioneComunita = Me.ImpostaPermessiIscrizione

        Reset_NoPermessi(Not (oSrvList.Admin OrElse oSrvList.List OrElse oSrvSubscribe.Admin OrElse oSrvSubscribe.List))
        If isForSubscription Then
            Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_IscrizioneComunita.Codex).ID, Services_IscrizioneComunita.ActionType.NoPermission)
        Else
            Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_ElencaComunita.Codex).ID, Services_ElencaComunita.ActionType.NoPermission)
        End If

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Dim isForSubscription As Boolean = isLoadedForSubscription()
        Dim oSrvList As Services_ElencaComunita = Me.ImpostaPermessiElenco
        Dim oSrvSubscribe As Services_IscrizioneComunita = Me.ImpostaPermessiIscrizione

        If isForSubscription Then
            Return (oSrvSubscribe.Admin OrElse oSrvSubscribe.List)
        Else
            Return (oSrvList.Admin OrElse oSrvList.List)
        End If
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_RicercaComunita", "Comunita")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource


            .setLabel(Me.LBNopermessi)


            .setLabel(Me.LBannoAccademico_c)
            .setLabel(Me.LBcorsoDiStudi_t)
            .setLabel(Me.LBtipoComunita_c)
            .setLabel(Me.LBnomeComunita_t)

            .setLabel(Me.LBnumeroRecord_c)


            .setButton(Me.BTNCerca)
            .setLabel(Me.LBricercaByIscrizione_c)

            .setLabel(Me.LBlegenda)
            .setRadioButtonList(Me.RBLricercaByIscrizione, 1)
            .setRadioButtonList(Me.RBLricercaByIscrizione, 0)
            .setRadioButtonList(Me.RBLstatoComunita, 0)
            .setRadioButtonList(Me.RBLstatoComunita, 1)
            .setLabel(Me.LBorganizzazione_c)
            '.setLinkButton(Me.LNBalbero, True, True)
            '.setLinkButton(Me.LNBalberoGerarchico, True, True)

            .setLinkButton(Me.LNBannullaConferma, True, True)
            .setLinkButton(Me.LNBannullaDettagli, True, True)
            .setLinkButton(Me.LNBelencoIscritte, True, True)
            .setLinkButton(Me.LNBentraDettagli, True, True)
            .setLinkButton(Me.LNBiscriviAltre, True, True)
            .setLinkButton(Me.LNBiscriviConferma, True, True)
            .setLinkButton(Me.LNBiscriviDettagli, True, True)
            .setLinkButton(Me.LNBannulla, True, True)
            .setLinkButton(Me.LNBiscriviMultipli, True, True)
            Me.LNBiscriviMultipli.Attributes.Add("onclick", "return HasComunitaSelezionate(false,'" & Replace(Me.Resource.getValue("MessaggioSelezione"), "'", "\'") & "','');")
            .setLabel(Me.LBstatoComunita_t)

            .setLabel(Me.LBresponsabile_t)
            .setLabel(Me.LBnomeComunita_t)

            .setHeaderDatagrid(Me.DGComunita, 0, "TPCM_Descrizione", True)
            .setHeaderDatagrid(Me.DGComunita, 1, "CMNT_Nome", True)
            .setHeaderDatagrid(Me.DGComunita, 2, "AnnoAccademico", True)
            .setHeaderDatagrid(Me.DGComunita, 3, "Periodo", True)

            .setHeaderDatagrid(Me.DGComunita, 5, "AnagraficaResponsabile", True)
            .setHeaderDatagrid(Me.DGComunita, 6, "Iscritti", True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region



    Private Function ImpostaPermessiIscrizione() As Services_IscrizioneComunita
        Dim ComunitaID As Integer = Me.ComunitaCorrenteID

        Dim iResponse As String = "00000000000000000000000000000000"
        Dim oServizioIscrizione As New Services_IscrizioneComunita
        Dim oPersona As COL_Persona = Me.UtenteCorrente

        Try
            If ComunitaID = 0 Then
                Session("Limbo") = True
                If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Guest Then
                    iResponse = "00000000000000000000000000000000"
                Else
                    oServizioIscrizione.Admin = False
                    oServizioIscrizione.List = True
                    Return oServizioIscrizione
                End If
            Else
                iResponse = Permessi(Services_IscrizioneComunita.Codex, Me.Page)
            End If
            If (iResponse = "") Then
                iResponse = "00000000000000000000000000000000"
            End If
        Catch ex As Exception
            iResponse = "00000000000000000000000000000000"
        End Try
        oServizioIscrizione.PermessiAssociati = iResponse
        Return oServizioIscrizione
    End Function
    Private Function ImpostaPermessiElenco() As Services_ElencaComunita
        Dim ComunitaID As Integer = Me.ComunitaCorrenteID
        Dim ForAdmin As Boolean = False
        Dim iResponse As String = "00000000000000000000000000000000"
        Dim oServizioElencaComunita As New Services_ElencaComunita
        Dim oPersona As COL_Persona = Me.UtenteCorrente


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


#Region "Reset Pannelli"
    Private Sub ResetFormAll()
        Me.PNLcontenuto.Visible = False
        Me.PNLconferma.Visible = False
        Me.PNLdettagli.Visible = False
        Me.PNLmessaggi.Visible = False
        Me.PNLiscrizioneAvvenuta.Visible = False
        Me.PNLmenu.Visible = False
        Me.PNLmenuAccesso.Visible = False
        Me.PNLmenuConferma.Visible = False
        Me.PNLmenuIscritto.Visible = False
        Me.PNLmenuDettagli.Visible = False
        Me.PNLpermessi.Visible = False
    End Sub
    Private Sub Reset_NoPermessi(ByVal NessunPermesso As Boolean)
        Me.ResetFormAll()
        If Not NessunPermesso Then
            Me.PNLcontenuto.Visible = True
            Me.DGComunita.Visible = False
            Me.LBmessageFind.Visible = False
            Me.BTNCerca.Enabled = False
            Me.RBLstatoComunita.Enabled = False
            Me.DDLannoAccademico.Enabled = False
            Me.DDLorganizzazione.Enabled = False
            Me.DDLresponsabile.Enabled = False
            Me.DDLTipo.Enabled = False
            Me.DDLtipoCorsoDiStudi.Enabled = False
            Me.TBRfiltriGenerici.Visible = False
        End If
        Me.PNLpermessi.Visible = True
    End Sub
    Private Sub Reset_Contenuto(Optional ByVal update As Boolean = False, Optional ByVal ShowMultipli As Boolean = False)
        Session("Azione") = "loaded"
        Me.ResetFormAll()
        Me.PNLcontenuto.Visible = True
        Me.PNLmenu.Visible = True
        Me.LNBiscriviMultipli.Visible = ShowMultipli
        Me.PNLpermessi.Visible = False
        If update Then
            Me.Bind_Griglia()
        End If
        Me.HDNcmnt_ID.Value = ""
        Me.HDNcmnt_Path.Value = ""
        Me.HDNisChiusaForPadre.Value = ""
        Me.HDisChiusa.Value = ""
    End Sub
    Private Sub Reset_ToDettagli()
        Me.ResetFormAll()
        Me.PNLdettagli.Visible = True
        Me.PNLmenuDettagli.Visible = True
    End Sub
    Private Sub Reset_ToMessaggi()
        Me.ResetFormAll()
        Me.PNLmessaggi.Visible = True
        Me.PNLmenuAccesso.Visible = True
    End Sub
    Private Sub Reset_ToConferma()
        Me.ResetFormAll()
        Me.PNLconferma.Visible = True
        Me.PNLmenuConferma.Visible = True
    End Sub
    Private Sub Reset_ToIscrizioneAvvenuta()
        Me.ResetFormAll()
        Me.PNLiscrizioneAvvenuta.Visible = True
        Me.PNLmenuAccesso.Visible = True
    End Sub
    Private Sub ResetFormToConferma(ByVal Multiplo As Boolean, ByVal Comunita As String, ByVal Responsabile As String)
        Me.ResetFormAll()
        Me.PNLconferma.Visible = True
        Me.PNLmenuConferma.Visible = True
        If Multiplo Then
            Me.LBconfermaMultipla.Visible = True
            Me.LBconferma.Visible = False
            Me.Resource.setLabel(Me.LBconfermaMultipla)
            Me.LBconfermaMultipla.Text = Me.LBconfermaMultipla.Text & "<br>" & Comunita
        Else
            Me.LBconfermaMultipla.Visible = False
            Me.LBconferma.Visible = True
            Me.Resource.setLabel(Me.LBconferma)
            Me.LBconferma.Text = Replace(Me.LBconferma.Text, "#nomeComunita#", Comunita)
            Me.LBconferma.Text = Replace(Me.LBconferma.Text, "#nomeResponsabile#", Responsabile)
        End If
    End Sub
#End Region

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

    Private Sub SetupFiltri()
        Dim oImpostazioni As New COL_ImpostazioniUtente
        Dim oFiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.nonIscritto
        Me.Bind_Organizzazioni()
        Me.Bind_TipiComunita()
        If Me.Request.QueryString("re_set") = "true" Then

            Try
                Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLorganizzazione")
            Catch ex As Exception
                Me.Response.Cookies("RicercaComunitaUtente")("DDLorganizzazione") = Me.DDLorganizzazione.SelectedValue
            End Try
         
            Me.SetupSearchParameters()
            If Me.RBLricercaByIscrizione.SelectedIndex = 1 Then
                oFiltroIscrizione = Main.FiltroRicercaComunitaByIscrizione.iscritto
            End If

            Try
                Me.Bind_StatusComunità()
                Me.RBLstatoComunita.SelectedValue = Me.Request.Cookies("DDLresponsabile")("RBLstatoComunita")
            Catch ex As Exception

            End Try
            Try
                Me.Bind_Responsabili(Me.Request.Cookies("DDLresponsabile")("DDLorganizzazione"), oFiltroIscrizione)
            Catch ex As Exception
                Me.Bind_Responsabili(, oFiltroIscrizione)
            End Try

        ElseIf Not IsNothing(Session("oImpostazioni")) Then
            Try
                oImpostazioni = Session("oImpostazioni")
                Me.Bind_Organizzazioni()
                With oImpostazioni
                    Try
                        If Me.RBLricercaByIscrizione.SelectedIndex = 0 Then
                            ' a  cui iscriversi
                            Me.DDLorganizzazione.SelectedValue = .Organizzazione_Ricerca
                        Else
                            Me.DDLorganizzazione.SelectedValue = .Organizzazione_Iscritto
                        End If
                    Catch ex As Exception

                    End Try


                    Try
                        If Me.RBLricercaByIscrizione.SelectedIndex = 0 Then
                            ' a  cui iscriversi
                            If .TipoComunita_Ricerca = -1 And Session("Limbo") = True Then
                              
                            Else
                                Me.DDLTipo.SelectedValue = .TipoComunita_Ricerca
                            End If
                        Else
                            Me.DDLTipo.SelectedValue = .TipoComunita_Iscritto
                        End If
                    Catch ex As Exception

                    End Try

                    Me.TBLcorsiDiStudio.Visible = False
                    Me.TBLcorsi.Visible = False

                        Me.LBnoCorsi.Visible = True


                    Try
                        If Me.RBLricercaByIscrizione.SelectedIndex = 0 Then
                            Me.ChangeNumeroRecord(oImpostazioni.Nrecord_Ricerca)
                        Else
                            Me.ChangeNumeroRecord(oImpostazioni.Nrecord_Iscritto)
                        End If
                    Catch ex As Exception

                    End Try
                End With
            Catch ex As Exception

            End Try
            If Me.RBLricercaByIscrizione.SelectedIndex = 1 Then
                oFiltroIscrizione = Main.FiltroRicercaComunitaByIscrizione.iscritto
            End If
            Me.Bind_Responsabili(, oFiltroIscrizione)
            Me.Bind_StatusComunità()
            Me.SaveSearchParameters()
        Else
           
            If Me.RBLricercaByIscrizione.SelectedIndex = 1 Then
                oFiltroIscrizione = Main.FiltroRicercaComunitaByIscrizione.iscritto
            End If
            Me.Bind_StatusComunità()
            Me.Bind_Responsabili(, oFiltroIscrizione)
        End If

        If Me.Request.QueryString("re_set") <> "true" Then
            If Session("limbo") = True Then
                If Me.RBLricercaByIscrizione.SelectedIndex = -1 Then
                    Me.RBLricercaByIscrizione.SelectedIndex = 0
                End If
            Else
                Dim oComunita As New COL_Comunita
                oComunita.Id = Session("idComunita")
                If oComunita.isBloccata() Then
                    Try
                        Me.RBLricercaByIscrizione.SelectedValue = 2
                    Catch ex As Exception
                        Me.RBLricercaByIscrizione.SelectedIndex = 0
                    End Try

                ElseIf oComunita.isArchiviata() Then
                    Try
                        Me.RBLricercaByIscrizione.SelectedValue = 1
                    Catch ex As Exception
                        Me.RBLricercaByIscrizione.SelectedIndex = 0
                    End Try
                Else
                    Me.RBLricercaByIscrizione.SelectedIndex = 0
                End If
            End If
        End If

        Try
            Me.HDN_filtroAnno.Value = Me.DDLannoAccademico.SelectedValue
            Me.HDN_filtroFacolta.Value = Me.DDLorganizzazione.SelectedValue

            Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue
            Me.HDN_filtroRicercaByIscrizione.Value = Me.RBLricercaByIscrizione.SelectedValue
            Me.HDN_filtroTipoCdl.Value = Me.DDLtipoCorsoDiStudi.SelectedValue
            Me.HDN_filtroTipoComunitaID.Value = Me.DDLTipo.SelectedValue
            Me.HDN_filtroValore.Value = Me.TXBValore.Text
            Me.HDN_filtroStatus.Value = Me.RBLstatoComunita.SelectedValue
        Catch ex As Exception

        End Try

        Me.Bind_Griglia(True)
    End Sub

    'Bind dati relativi ai filtri.

    Private Sub Bind_StatusComunità()
        Dim oPersona As New COL_Persona
        Dim totale, TotaleArchiviate, totaleBloccate As Integer
        Try
            Dim oListItem_Archiviate As ListItem
            oPersona = Session("objPersona")

            If Me.RBLricercaByIscrizione.SelectedValue = 0 Then
                oPersona.StatusComunitaNonIscritto(oPersona.ID, totale, TotaleArchiviate, totaleBloccate)
                Me.RBLstatoComunita.SelectedIndex = 0
                Me.RBLstatoComunita.Enabled = False
            Else
                oPersona.StatusComunitaIscritto(oPersona.ID, totale, TotaleArchiviate, totaleBloccate)
                Me.RBLstatoComunita.Enabled = True
            End If


            oListItem_Archiviate = Me.RBLstatoComunita.Items.FindByValue(1)
            'oListItem_Bloccate = Me.RBLstatoComunita.Items.FindByValue(2)
            'If totaleBloccate = 0 Then
            '    If Not IsNothing(oListItem_Bloccate) Then
            '        If Me.RBLstatoComunita.SelectedValue = 2 Then
            '            Me.RBLstatoComunita.SelectedIndex = 0
            '        End If
            '        Me.RBLstatoComunita.Items.Remove(oListItem_Bloccate)
            '    End If
            'Else
            '    If IsNothing(oListItem_Bloccate) Then
            '        If IsNothing(oListItem_Archiviate) Then
            '            Me.RBLstatoComunita.Items.Insert(1, New ListItem("Bloccate", 2))
            '        Else
            '            Me.RBLstatoComunita.Items.Insert(2, New ListItem("Bloccate", 2))
            '        End If
            '        Me.Resource.setRadioButtonList(Me.RBLstatoComunita, 2)
            '    End If
            'End If

            If TotaleArchiviate = 0 Then
                If Not IsNothing(oListItem_Archiviate) Then
                    If Me.RBLstatoComunita.SelectedValue = 1 Then
                        Me.RBLstatoComunita.SelectedIndex = 0
                    End If
                    Me.RBLstatoComunita.Items.Remove(oListItem_Archiviate)
                End If
            Else
                If IsNothing(oListItem_Archiviate) Then
                    Me.RBLstatoComunita.Items.Insert(1, New ListItem("Archiviate", 1))
                    Me.Resource.setRadioButtonList(Me.RBLstatoComunita, 1)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TipiComunita()
        Dim oDataSet As New DataSet

        Try
            Dim OrganizationID As Integer = -1
            If Me.DDLorganizzazione.SelectedIndex > -1 Then
                OrganizationID = Me.DDLorganizzazione.SelectedValue
            End If
            oDataSet = COL_Tipo_Comunita.FindByOrganization(Session("LinguaID"), OrganizationID)
            If oDataSet.Tables(0).Rows.Count > 0 Then
                DDLTipo.DataSource = oDataSet
                DDLTipo.DataTextField() = "TPCM_descrizione"
                DDLTipo.DataValueField() = "TPCM_id"
                DDLTipo.DataBind()

                'aggiungo manualmente elemento che indica tutti i tipi di comunità
                If DDLTipo.Items.Count > 1 Then
                    DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
                End If
            End If
        Catch ex As Exception
            DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try
        Me.Resource.setDropDownList(Me.DDLTipo, -1)
    End Sub
    Private Sub Bind_Organizzazioni()
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona

        Me.DDLorganizzazione.Items.Clear()
        Try
            oPersona = Session("objPersona")
            oDataset = oPersona.GetOrganizzazioniAssociate(True)

            If oDataset.Tables(0).Rows.Count > 0 Then
                Dim oComunita As New COL_Comunita

                Dim ArrComunita(,) As String
                Dim FacoltaID As Integer
                Dim show As Boolean = False
                Try
                    If IsArray(Session("ArrComunita")) And Session("limbo") = False Then
                        ArrComunita = Session("ArrComunita")
                        oComunita.Id = ArrComunita(0, 0)
                        oComunita.Estrai()
                        FacoltaID = oComunita.Organizzazione.Id
                        show = False
                    ElseIf Session("limbo") = True Then
                        show = True
                        FacoltaID = -1
                    End If
                Catch ex As Exception
                    Try
                        FacoltaID = Session("ORGN_id")
                        show = False
                    Catch exc As Exception
                        FacoltaID = -1
                    End Try

                End Try

                Me.DDLorganizzazione.DataValueField = "ORGN_id"
                Me.DDLorganizzazione.DataTextField = "ORGN_ragioneSociale"
                Me.DDLorganizzazione.DataSource = oDataset
                Me.DDLorganizzazione.DataBind()
                'If oDataset.Tables(0).Rows.Count > 1 Then
                '    Me.DDLorganizzazione.Items.Insert(0, New ListItem("<< tutte >>", -1))
                'End If

                If Me.DDLorganizzazione.Items.Count > 1 Then
                    Me.DDLorganizzazione.Enabled = True

                    If FacoltaID >= 0 Then
                        Try
                            Me.DDLorganizzazione.SelectedValue = FacoltaID
                            Me.DDLorganizzazione.Enabled = False
                        Catch ex As Exception
                            Me.DDLorganizzazione.Items.Clear()
                            Me.DDLorganizzazione.Items.Add(New ListItem(oComunita.Nome, FacoltaID))
                            Me.DDLorganizzazione.SelectedIndex = 0
                        End Try
                    Else
                        Me.DDLorganizzazione.SelectedIndex = 0
                    End If
                Else
                    Me.DDLorganizzazione.Enabled = False
                End If
            Else
                Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
                Me.DDLorganizzazione.Enabled = False
            End If
        Catch ex As Exception
            Me.DDLorganizzazione.Items.Clear()
            Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
            Me.DDLorganizzazione.Enabled = False
        End Try
        Me.Resource.setDropDownList(Me.DDLorganizzazione, -1)
        Me.Resource.setDropDownList(Me.DDLorganizzazione, 0)
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
                TipoComuniaID = Me.DDLTipo.SelectedValue
            Catch ex As Exception

            End Try

            Try
                AnnoAcc = Me.DDLannoAccademico.SelectedValue
            Catch ex As Exception

            End Try

            Try
                TipoCdlID = Me.DDLtipoCorsoDiStudi.SelectedValue
            Catch ex As Exception

            End Try

            oDataSet = COL_Comunita.RicercaResponsabili(FacoltaID, ComunitaID, Session("objPersona").id, TipoComuniaID, , , , , Me.RBLstatoComunita.SelectedValue, FiltroIscrizione)
       

            If oDataSet.Tables(0).Rows.Count > 0 Then
                DDLresponsabile.DataSource = oDataSet
                DDLresponsabile.DataTextField() = "Anagrafica"
                DDLresponsabile.DataValueField() = "PRSN_ID"
                DDLresponsabile.DataBind()

                'aggiungo manualmente elemento che indica tutti i tipi di comunità
                DDLresponsabile.Items.Insert(0, New ListItem("-- Select --", -2))
            End If
        Catch ex As Exception
            Me.DDLresponsabile.Items.Insert(0, New ListItem("-- Select --", -2))
        End Try
        Me.Resource.setDropDownList(Me.DDLresponsabile, -2)
        If DocenteID > 0 Then
            Try
                Me.DDLresponsabile.SelectedValue = DocenteID
            Catch ex As Exception

            End Try
        End If

    End Sub
    Private Function FiltraggioDati(Optional ByVal ApplicaFiltri As Boolean = False) As DataSet
        Dim oPersona As New COL_Persona

        Dim i, totale, totaleHistory As Integer
        Dim oDataset As New DataSet
        Try
            Dim valore As String = ""
            Dim oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti
            Dim oFiltroLettera As Main.FiltroComunita = Main.FiltroComunita.tutti

            oPersona = Session("objPersona")
           

            Dim ResponsabileID As Integer = -1
            If ApplicaFiltri = True Then
                If Me.TXBValore.Text <> "" Then
                    Me.TXBValore.Text = Trim(Me.TXBValore.Text)
                End If
                valore = Me.TXBValore.Text
                ResponsabileID = Me.DDLresponsabile.SelectedValue
            Else
                Try
                    valore = Trim(Me.HDN_filtroValore.Value)
                Catch ex As Exception

                End Try
                ResponsabileID = Me.HDN_filtroResponsabileID.Value
            End If
            If (ApplicaFiltri = True) AndAlso valore = "" Then
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

            Dim FacoltaID, AAid, TipocomunitaID, TipoCdlID, StatusID As Integer
            Dim oFiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.nonIscritto
            If ApplicaFiltri = True Then
                Me.HDN_filtroValore.Value = Me.TXBValore.Text
                Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue
                Me.HDN_filtroTipoCdl.Value = Me.DDLtipoCorsoDiStudi.SelectedValue
                Me.HDN_filtroAnno.Value = Me.DDLannoAccademico.SelectedValue
                Me.HDN_filtroTipoComunitaID.Value = Me.DDLTipo.SelectedValue
                Me.HDN_filtroRicercaByIscrizione.Value = Me.RBLricercaByIscrizione.SelectedValue
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
                oFiltroIscrizione = Me.HDN_filtroRicercaByIscrizione.Value
            Catch ex As Exception

            End Try

            Try
                StatusID = Me.HDN_filtroStatus.Value
            Catch ex As Exception

            End Try
            Dim oComunita As New COL_Comunita
            If valore <> "" Then
                While valore.Contains("''")
                    valore = Replace(valore, "''", "'")
                End While
                valore = Replace(valore, "'", "''")
            End If
            If Not (ResponsabileID < 0 AndAlso valore = "") Then
                Select Case Me.HDN_filtroTipoComunitaID.Value
                    Case Main.TipoComunitaStandard.Organizzazione
                        oDataset = COL_Comunita.FindOrganizations(oFiltroIscrizione, Session("LinguaID"), oPersona.ID, ResponsabileID, valore, StatusID)
                    Case Else
                        oDataset = COL_Comunita.FindCommunity(oFiltroIscrizione, Session("LinguaID"), FacoltaID, ComunitaPadreID, oPersona.ID, valore, ResponsabileID, TipocomunitaID, StatusID)
                End Select

                oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Esteso"))
                oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_EstesoNoSpan"))
                oDataset.Tables(0).Columns.Add(New DataColumn("Proprieta"))
                oDataset.Tables(0).Columns.Add(New DataColumn("Alternative"))
                oDataset.Tables(0).Columns.Add(New DataColumn("Iscritti"))

                If Not oDataset.Tables(0).Columns.Contains("AnnoAccademico") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("AnnoAccademico"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("Periodo") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("Periodo"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("CMNT_Iscritti") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Iscritti"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("CMNT_Totale") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Totale"))
                End If
                If Not oDataset.Tables(0).Columns.Contains("AnagraficaResponsabile") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("AnagraficaResponsabile"))
                End If

                Dim oDataview As New DataView
                Dim ElencoComunitaID As String = ","

                oDataview = oDataset.Tables(0).DefaultView
                oDataview.AllowDelete = True
                totale = oDataset.Tables(0).Rows.Count


                If IsArray(Session("ArrComunita")) Then
                    Try
                        Dim ArrComunita(,) As String
                        ArrComunita = Session("ArrComunita")
                        totaleHistory = UBound(ArrComunita, 2)
                        oDataview.RowFilter = "ALCM_PATH not like '" & ArrComunita(2, totaleHistory) & "%'"
                        While oDataview.Count > 0
                            oDataview.Delete(0)
                        End While
                        oDataview.RowFilter = ""
                        oDataset.AcceptChanges()
                    Catch ex As Exception
                        oDataview.RowFilter = ""
                    End Try
                End If



                While oDataview.Count > 0
                    Dim ComunitaID As Integer


                    If ElencoComunitaID = "," Then
                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID
                    Else
                        oDataview.RowFilter = "'" & ElencoComunitaID & "' not like '%,' + CMNT_ID + ',%'"

                        If oDataview.Count > 0 Then
                            ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        End If
                    End If

                    If oDataview.Count = 1 Then
                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    ElseIf oDataview.Count > 1 Then
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        If oDataview.Count = 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '' '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        ElseIf oDataview.Count = 0 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '''%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                        ElseIf oDataview.Count > 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        End If

                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    End If

                End While
                oDataview.RowFilter = ""
                oDataset.AcceptChanges()


                Dim ImageBaseDir, img As String
                ImageBaseDir = GetPercorsoApplicazione(Me.Request)
                ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"
                ImageBaseDir = Replace(ImageBaseDir, "//", "/")

                totale = oDataset.Tables(0).Rows.Count
                Me.DGComunita.Columns(6).Visible = False

                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)


                    If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                        oRow.Item("AnagraficaResponsabile") = Me.Resource.getValue("creata")
                        oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                    Else
                        If oRow.Item("CMNT_Responsabile") = "" Then
                            oRow.Item("AnagraficaResponsabile") = Me.Resource.getValue("creata")
                            oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                        Else
                            oRow.Item("AnagraficaResponsabile") = oRow.Item("CMNT_Responsabile")
                        End If
                    End If
                    If oRow.Item("CMNT_IsChiusa") = True Then
                        oRow.Item("Proprieta") = Me.Resource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                        oRow.Item("Alternative") = Me.Resource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                    ElseIf oRow.Item("ALCM_isChiusaForPadre") = True Then
                        oRow.Item("Proprieta") = Me.Resource.getValue("stato.image." & oRow.Item("ALCM_isChiusaForPadre"))
                        oRow.Item("Alternative") = Me.Resource.getValue("stato." & oRow.Item("ALCM_isChiusaForPadre"))
                    Else
                        oRow.Item("Proprieta") = Me.Resource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                        oRow.Item("Alternative") = Me.Resource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                    End If
                    If IsDBNull(oRow.Item("TPCM_icona")) = False Then
                        img = oRow.Item("TPCM_icona")
                        img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                        oRow.Item("TPCM_icona") = img
                    End If
                    If IsDate(oRow.Item("CMNT_dataInizioIscrizione")) Then
                        If Not Equals(New Date, oRow.Item("CMNT_dataInizioIscrizione")) Then
                            oRow.Item("CMNT_dataInizioIscrizione") = FormatDateTime(oRow.Item("CMNT_dataInizioIscrizione"), DateFormat.GeneralDate)
                        End If
                    End If
                    If IsDate(oRow.Item("CMNT_dataFineIscrizione")) Then
                        If Not Equals(New Date, oRow.Item("CMNT_dataFineIscrizione")) Then
                            oRow.Item("CMNT_dataFineIscrizione") = FormatDateTime(oRow.Item("CMNT_dataFineIscrizione"), DateFormat.GeneralDate)
                        End If
                    End If
                    If IsDBNull(oRow.Item("CMNT_AnnoAccademico")) Then
                        oRow.Item("AnnoAccademico") = "&nbsp;"
                    Else
                        oRow.Item("AnnoAccademico") = oRow.Item("CMNT_AnnoAccademico")
                    End If
                    If IsDBNull(oRow.Item("PRDO_descrizione")) Then
                        oRow.Item("Periodo") = "&nbsp;"
                    Else
                        oRow.Item("Periodo") = oRow.Item("PRDO_descrizione")
                    End If

                    oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                    oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")

                    Try
                        Dim numIscritti, maxIscritti As Integer
                        maxIscritti = oRow.Item("CMNT_MaxIscritti")
                        numIscritti = oRow.Item("CMNT_Iscritti")
                        Try
                            oRow.Item("Iscritti") = numIscritti
                        Catch ex As Exception
                            oRow.Item("Iscritti") = 0
                            numIscritti = 0
                        End Try

                        If (maxIscritti <= 0) Then
                            oRow.Item("CMNT_Iscritti") = 0
                        Else
                            If numIscritti > maxIscritti Then
                                oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                                oRow.Item("Iscritti") = Me.Resource.getValue("limiti.superato")
                                oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#num1#", "<b>" & numIscritti & "</b>")
                                oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#limite#", maxIscritti)
                                oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#numOver#", numIscritti - maxIscritti)
                            ElseIf numIscritti = maxIscritti Then
                                oRow.Item("CMNT_Iscritti") = -1
                                oRow.Item("Iscritti") = numIscritti & " " & Me.Resource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                            Else
                                oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                                oRow.Item("Iscritti") = numIscritti & " " & Me.Resource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                            End If

                            Me.DGComunita.Columns(6).Visible = True
                        End If
                    Catch ex As Exception

                    End Try
                Next
                If totale = 0 Then
                    MyBase.Resource.setLabel_To_Value(Me.LBmessageFind, "elenco." & StringaElenco.noCommunity)
                End If
            Else
                oDataset.Tables.Add(New DataTable)
                MyBase.Resource.setLabel_To_Value(Me.LBmessageFind, "elenco." & StringaElenco.noCommunityForFilter)
            End If
        Catch ex As Exception

        End Try
        Return oDataset
    End Function
    Private Function FiltraggioDatiRistretto() As DataSet
        Dim oPersona As New COL_Persona

        Dim totale, totaleHistory As Integer
        Dim oDataset As New DataSet

        Try

            Dim valore As String = ""
            Dim oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti
            Dim oFiltroLettera As Main.FiltroComunita = Main.FiltroComunita.tutti

            oPersona = Session("objPersona")
           

            Dim ResponsabileID As Integer = -1
            valore = Trim(Me.HDN_filtroValore.Value)
            ResponsabileID = Me.HDN_filtroResponsabileID.Value

            Dim ComunitaPadreID As Integer
            Try
                ComunitaPadreID = Session("idComunita")
                If ComunitaPadreID < 1 Then
                    ComunitaPadreID = -1
                End If
            Catch ex As Exception
                ComunitaPadreID = -1
            End Try

            Dim FacoltaID, AAid, TipocomunitaID, TipoCdlID, StatusID As Integer
            Dim oFiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.nonIscritto
           
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
                oFiltroIscrizione = Me.HDN_filtroRicercaByIscrizione.Value
            Catch ex As Exception

            End Try

            Try
                StatusID = Me.HDN_filtroStatus.Value
            Catch ex As Exception
                StatusID = 0
            End Try
            If valore <> "" Then
                While valore.Contains("''")
                    valore = Replace(valore, "''", "'")
                End While
                valore = Replace(valore, "'", "''")
            End If
            Dim oComunita As New COL_Comunita
            Select Case Me.HDN_filtroTipoComunitaID.Value
                Case Main.TipoComunitaStandard.Organizzazione
                    oDataset = COL_Comunita.FindOrganizations(oFiltroIscrizione, Session("LinguaID"), oPersona.ID, ResponsabileID, valore, StatusID)
                Case Else
                    oDataset = COL_Comunita.FindCommunity(oFiltroIscrizione, Session("LinguaID"), FacoltaID, ComunitaPadreID, oPersona.ID, valore, ResponsabileID, TipocomunitaID, StatusID)
            End Select

            Dim oDataview As New DataView
            Dim ElencoComunitaID As String = ","

            oDataview = oDataset.Tables(0).DefaultView
            oDataview.AllowDelete = True
            totale = oDataset.Tables(0).Rows.Count


            If IsArray(Session("ArrComunita")) Then
                Try
                    Dim ArrComunita(,) As String
                    ArrComunita = Session("ArrComunita")
                    totaleHistory = UBound(ArrComunita, 2)
                    oDataview.RowFilter = "ALCM_PATH not like '" & ArrComunita(2, totaleHistory) & "%'"
                    While oDataview.Count > 0
                        oDataview.Delete(0)
                    End While
                    oDataview.RowFilter = ""
                    oDataset.AcceptChanges()
                Catch ex As Exception
                    oDataview.RowFilter = ""
                End Try
            End If



            While oDataview.Count > 0
                Dim ComunitaID As Integer


                If ElencoComunitaID = "," Then
                    ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                    oDataview.RowFilter = "CMNT_ID=" & ComunitaID
                Else
                    oDataview.RowFilter = "'" & ElencoComunitaID & "' not like '%,' + CMNT_ID + ',%'"

                    If oDataview.Count > 0 Then
                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                    End If
                End If

                If oDataview.Count = 1 Then
                    oDataview.RowFilter = ""
                    ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                ElseIf oDataview.Count > 1 Then
                    oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                    If oDataview.Count = 1 Then
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '' '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                        While oDataview.Count > 0
                            oDataview.Delete(0)
                        End While
                    ElseIf oDataview.Count = 0 Then
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '''%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                        While oDataview.Count > 1
                            oDataview.Delete(1)
                        End While
                    ElseIf oDataview.Count > 1 Then
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        While oDataview.Count > 1
                            oDataview.Delete(1)
                        End While
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        While oDataview.Count > 0
                            oDataview.Delete(0)
                        End While
                    End If

                    oDataview.RowFilter = ""
                    ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                End If

            End While
            oDataview.RowFilter = ""
            oDataset.AcceptChanges()
        Catch ex As Exception

        End Try
        Return oDataset
    End Function
    Private Sub Bind_Griglia(Optional ByVal ApplicaFiltri As Boolean = False)
        'carica le comunità nella datagrid DGComunita
        'se gli passo 0 seleziona tutte
        Me.LBmessageFind.Visible = False
        Me.DGComunita.Visible = True 'se la datagrid era vuota allora era stata nascosta


        Dim oPersona As New COL_Persona
        Dim oDataset As DataSet
        Dim totale As Integer
        oPersona = Session("objPersona")


        Try
            Dim oTreeComunita As New COL_TreeComunita

            oDataset = Me.FiltraggioDati(ApplicaFiltri)
            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                ' al posto della datagrid mostro un messaggio!
                Me.DGComunita.Visible = False
                Me.LBmessageFind.Visible = True
                Me.LBnumeroRecord_c.Visible = False
                Me.DDLNumeroRecord.Visible = False
                Me.DGComunita.PagerStyle.Position = PagerPosition.Top
                Me.PNLmenu.Visible = False
                Me.LNBiscriviMultipli.Enabled = False
            Else
                Me.PNLmenu.Visible = True
                totale = oDataset.Tables(0).Rows.Count

                If totale <= Me.DDLNumeroRecord.Items(0).Value Then
                    Me.LBnumeroRecord_c.Visible = False
                    Me.DDLNumeroRecord.Visible = False
                    Me.DGComunita.PagerStyle.Position = PagerPosition.Top
                Else
                    Me.LBnumeroRecord_c.Visible = True
                    Me.DDLNumeroRecord.Visible = True
                    Me.DGComunita.PagerStyle.Position = PagerPosition.TopAndBottom
                End If
                If totale > 0 Then
                    Me.DGComunita.Columns(6).Visible = False
                    Me.DGComunita.Columns(2).Visible = False
                    Me.DGComunita.Columns(3).Visible = False
                  
                    If CInt(Me.HDN_filtroRicercaByIscrizione.Value) = 0 Then
                        Me.DGComunita.Columns(28).Visible = True
                    Else
                        Me.DGComunita.Columns(28).Visible = False
                    End If


                    Dim oDataview As DataView
                    oDataview = oDataset.Tables(0).DefaultView
                    If ViewState("SortExspression") = "" Then
                        ViewState("SortExspression") = "ALCM_Livello,CMNT_Nome"
                        ViewState("SortDirection") = "asc"
                    End If
                    Me.LNBiscriviMultipli.Enabled = True
                    oDataview.Sort = ViewState("SortExspression") & " " & ViewState("SortDirection")
                    DGComunita.DataSource = oDataview
                    DGComunita.DataBind()
                Else
                    Me.LNBiscriviMultipli.Enabled = False
                    Me.DGComunita.Visible = False
                    Me.LBmessageFind.Visible = True
                End If
            End If
        Catch ex As Exception
            Me.DGComunita.Visible = False
            Me.LBmessageFind.Visible = True
            Me.LBnumeroRecord_c.Visible = False
            Me.DDLNumeroRecord.Visible = False
            Me.LNBiscriviMultipli.Enabled = False
            Me.Resource.setLabel_To_Value(Me.LBmessageFind, "elenco." & StringaElenco.noCommunityForFilter)
        End Try
    End Sub

#End Region

#Region "Setup Parametri Ricerca"
    Private Sub SaveSearchParameters() 'ByVal visualizzazione As Integer)
        Try
            Me.Response.Cookies("RicercaComunitaUtente")("DDLannoAccademico") = Me.DDLannoAccademico.SelectedValue
            Me.Response.Cookies("RicercaComunitaUtente")("DDLNumeroRecord") = Me.DDLNumeroRecord.SelectedValue
            Me.Response.Cookies("RicercaComunitaUtente")("DDLorganizzazione") = Me.DDLorganizzazione.SelectedValue
            Me.Response.Cookies("RicercaComunitaUtente")("DDLTipo") = Me.DDLTipo.SelectedValue
            Me.Response.Cookies("RicercaComunitaUtente")("TXBValore") = Me.TXBValore.Text
            Me.Response.Cookies("RicercaComunitaUtente")("intCurPage") = Me.ViewState("intCurPage")
            Me.Response.Cookies("RicercaComunitaUtente")("intAnagrafica") = Me.ViewState("intAnagrafica")
            Me.Response.Cookies("RicercaComunitaUtente")("SortDirection") = Me.ViewState("SortDirection")
            Me.Response.Cookies("RicercaComunitaUtente")("SortExspression") = Me.ViewState("SortExspression")
            Me.Response.Cookies("RicercaComunitaUtente")("RBLricercaByIscrizione") = Me.RBLricercaByIscrizione.SelectedValue
            Me.Response.Cookies("RicercaComunitaUtente")("DDLtipoCorsoDiStudi") = Me.DDLtipoCorsoDiStudi.SelectedValue
            Me.Response.Cookies("RicercaComunitaUtente")("RBLstatoComunita") = Me.RBLstatoComunita.SelectedValue
            Me.Response.Cookies("RicercaComunitaUtente")("DDLresponsabile") = Me.DDLresponsabile.SelectedValue
        Catch ex As Exception

        End Try
    End Sub
    Private Sub SetupSearchParameters()
        Try
            'Recupero fattori di ricerca relativi all'ordinamento
            Try
                Me.ViewState("SortDirection") = Me.Request.Cookies("RicercaComunitaUtente")("SortDirection")
                Me.ViewState("SortExspression") = Me.Request.Cookies("RicercaComunitaUtente")("SortExspression")
            Catch ex As Exception

            End Try

            Try
                Me.RBLstatoComunita.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("RBLstatoComunita")
            Catch ex As Exception
                Me.RBLstatoComunita.SelectedIndex = 0
            End Try

            Try
                'Recupero dati relativi alla paginazione corrente
                If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("intCurPage")) Then
                    Me.ViewState("intCurPage") = CInt(Me.Request.Cookies("RicercaComunitaUtente")("intCurPage"))
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
                Me.TXBValore.Text = Me.Request.Cookies("RicercaComunitaUtente")("TXBValore")
            Catch ex As Exception
                Me.TXBValore.Text = ""
            End Try


            ' Setto l'anno accademico
            Try
                If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("DDLannoAccademico")) Then
                    Try
                        Me.DDLannoAccademico.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLannoAccademico")
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try
            Try
                If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("RBLricercaByIscrizione")) Then
                    Try
                        Me.RBLricercaByIscrizione.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("RBLricercaByIscrizione")
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try
            ' Setto l'organizzazione
            Try
                If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("DDLorganizzazione")) Then
                    Try
                        Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLorganizzazione")
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try

            ' Setto il numero di record
            Try
                If IsNumeric(Me.Request.Cookies("RicercaComunitaUtente")("DDLNumeroRecord")) Then
                    Me.DDLNumeroRecord.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLNumeroRecord")
                End If
            Catch ex As Exception

            End Try

            Try
                ' If IsNumeric(Me.Request.Cookies("ListaComunita")("tipo")) Then
                Me.DDLTipo.SelectedValue = Me.Request.Cookies("RicercaComunitaUtente")("DDLTipo")

                Me.TBLcorsi.Visible = False
                Me.TBLcorsiDiStudio.Visible = False
                Me.LBnoCorsi.Visible = True
            Catch ex As Exception
                Me.TBLcorsi.Visible = False
                Me.TBLcorsiDiStudio.Visible = False
                Me.LBnoCorsi.Visible = True
            End Try
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Filtro"
    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
        Me.Bind_TipiComunita()

        Dim ResponsabileID As Integer = -1
        If Me.DDLresponsabile.SelectedIndex > -1 Then
            ResponsabileID = Me.DDLresponsabile.SelectedValue
        End If
        Me.Bind_Responsabili(ResponsabileID, Me.RBLricercaByIscrizione.SelectedValue)

        Me.Bind_Griglia()
    End Sub
    Private Sub RBLstatoComunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLstatoComunita.SelectedIndexChanged
    
    End Sub
    Private Sub DDLTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLTipo.SelectedIndexChanged
       
            Me.TBLcorsi.Visible = False
            Me.TBLcorsiDiStudio.Visible = False
            Me.LBnoCorsi.Visible = True
            Me.DDLtipoCorsoDiStudi.SelectedIndex = 0
            Me.DDLannoAccademico.SelectedIndex = 0

        Dim oFiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.nonIscritto
        If Me.RBLricercaByIscrizione.SelectedIndex = 1 Then
            oFiltroIscrizione = Main.FiltroRicercaComunitaByIscrizione.iscritto
        End If
        Dim ResponsabileID As Integer = -1
        If Me.DDLresponsabile.SelectedIndex > -1 Then
            ResponsabileID = Me.DDLresponsabile.SelectedValue
        End If
        Me.Bind_Responsabili(ResponsabileID, oFiltroIscrizione)
        Me.Bind_Griglia()
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.ViewState("intCurPage") = 0
        Me.DDLNumeroRecord.SelectedIndex = Me.DDLNumeroRecord.SelectedIndex
        DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        DGComunita.CurrentPageIndex = 0
        Bind_Griglia(True)
    End Sub
    Private Sub RBLricercaByIscrizione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLricercaByIscrizione.SelectedIndexChanged
        Dim oServizioElencaComunita As New Services_ElencaComunita
        Dim oServizioIscrizione As New Services_IscrizioneComunita
        Dim HasPermessi As Boolean = True
        Dim HasNoPermessi As Boolean = False
        Dim ForIscrizione As Boolean = True

        oServizioIscrizione = Me.ImpostaPermessiIscrizione
        oServizioElencaComunita = Me.ImpostaPermessiElenco
        If Me.RBLricercaByIscrizione.SelectedValue = 0 Then
            'Me.Resource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.forSubscribe)
            Me.Master.ServiceTitle = Me.Resource.getValue("LBtitolo." & stringaTitolo.forSubscribe)
        ElseIf Me.RBLricercaByIscrizione.SelectedValue = 1 Then
            ForIscrizione = False
            'Me.Resource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.Subscribed)
            Me.Master.ServiceTitle = Me.Resource.getValue("LBtitolo." & stringaTitolo.Subscribed)
        Else
            'Me.Resource.setLabel_To_Value(Me.LBtitolo, "LBtitolo." & stringaTitolo.Subscribed)
            Me.Master.ServiceTitle = Me.Resource.getValue("LBtitolo." & stringaTitolo.Subscribed)
        End If

        If ForIscrizione Then
            HasPermessi = (oServizioIscrizione.Admin Or oServizioIscrizione.List)
        Else
            HasPermessi = (oServizioElencaComunita.Admin Or oServizioElencaComunita.List)
        End If
        HasNoPermessi = Not (oServizioElencaComunita.Admin Or oServizioElencaComunita.List Or oServizioIscrizione.Admin Or oServizioIscrizione.List)

        If HasPermessi Then
            Me.Bind_StatusComunità()
            Me.BTNCerca.Enabled = True
            If Me.DDLannoAccademico.Items.Count > 0 Then
                Me.DDLannoAccademico.Enabled = True
            End If
            If Me.DDLorganizzazione.Items.Count > 1 Then
                Me.DDLorganizzazione.Enabled = True
            End If
            If Me.DDLresponsabile.Items.Count > 1 Then
                Me.DDLresponsabile.Enabled = True
            End If
            If Me.DDLTipo.Items.Count > 1 Then
                Me.DDLTipo.Enabled = True
            End If
            If Me.DDLtipoCorsoDiStudi.Items.Count > 1 Then
                Me.DDLtipoCorsoDiStudi.Enabled = True
            End If
            Me.TBRfiltriGenerici.Visible = True
            Me.PNLmenu.Visible = True
            Try
                If Me.DDLresponsabile.SelectedValue > 0 Then
                    Me.Bind_Responsabili(Me.DDLresponsabile.SelectedValue, Me.RBLricercaByIscrizione.SelectedValue)
                Else
                    Me.Bind_Responsabili(, Me.RBLricercaByIscrizione.SelectedValue)
                End If

            Catch ex As Exception
                Me.Bind_Responsabili(, Me.RBLricercaByIscrizione.SelectedValue)
            End Try
            Me.Bind_Griglia()
        Else
            Reset_NoPermessi(HasNoPermessi)
        End If
    End Sub
    Private Sub BTNCerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        DGComunita.CurrentPageIndex = 0
        Me.ViewState("intCurPage") = 0
        Me.HDNcomunitaSelezionate.Value = ""
        Me.Bind_Griglia(True)
        Me.SaveSearchParameters()
    End Sub
#End Region

#Region "Gestione Griglia"
    Private Sub DGcomunita_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DGComunita.SortCommand
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
    Private Sub DGComunita_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGComunita.ItemCreated
        Dim i As Integer

        If IsNothing(Resource) Then
            Me.SetCultureSettings()
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
                                    Me.Resource.setHeaderOrderbyLink_Datagrid(Me.DGComunita, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    Me.Resource.setHeaderOrderbyLink_Datagrid(Me.DGComunita, oLinkbutton, FiltroOrdinamento.Crescente)
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
                                Me.Resource.setHeaderOrderbyLink_Datagrid(Me.DGComunita, oLinkbutton, FiltroOrdinamento.Crescente)
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
                If Me.DGComunita.Columns(2).Visible Then
                    num += 1
                End If
                If Me.DGComunita.Columns(3).Visible Then
                    num += 1
                End If
                If Me.DGComunita.Columns(6).Visible Then
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
                    Me.Resource.setPageDatagrid(Me.DGComunita, oLinkbutton)
                End Try
            Next
        End If
        If (e.Item.ItemType = ListItemType.Footer) Then
            e.Item.Cells(0).ColumnSpan = e.Item.Cells.Count
            For i = 1 To e.Item.Cells.Count - 1
                e.Item.Cells.RemoveAt(1)
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Try
                Dim cssLink As String = "ROW_ItemLink_Small"
                Dim cssRiga As String = "ROW_TD_Small"
                Dim hasAccesso As Boolean = True
                Dim hasIscrizione As Boolean = False

                Dim forSubscription As Boolean = False
                Dim visibileEntra, visibileIscrivi As Boolean

                Dim oLNBiscrivi, oLNBentra, oLNBdettagli As LinkButton
                Dim oLabelNews As Label
                Dim oLiteralNews As Literal

                oLNBdettagli = e.Item.Cells(1).FindControl("LNBdettagli")
                oLNBentra = e.Item.Cells(1).FindControl("LNBlogin")
                oLNBiscrivi = e.Item.Cells(1).FindControl("LNBiscrivi")
                oLabelNews = e.Item.Cells(1).FindControl("LBseparatorNews")
                oLiteralNews = e.Item.Cells(1).FindControl("LThasnews")

                forSubscription = (CInt(Me.HDN_filtroRicercaByIscrizione.Value) = 0)


                If forSubscription Then
                    visibileEntra = False
                    visibileIscrivi = True
                Else
                    Dim oAbilitato As Boolean = e.Item.DataItem("RLPC_abilitato")
                    Dim oAttivato As Boolean = e.Item.DataItem("RLPC_attivato")

                    visibileEntra = True
                    visibileIscrivi = False
                    Try
                        If e.Item.DataItem("CMNT_Bloccata") Then
                            hasAccesso = False
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        ' verifico se l'iscrizione è avvenuta e l'accesso è possibile
                        If IsDBNull(e.Item.DataItem("RLPC_TPRL_ID")) Then
                            visibileEntra = False
                        Else
                            If e.Item.DataItem("RLPC_TPRL_ID") > 0 Then
                                hasAccesso = (oAttivato And oAbilitato And hasAccesso)
                            Else
                                visibileEntra = False
                            End If
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If Not (oAttivato) Then
                            e.Item.CssClass = "ROW_Disattivate_Small"
                            cssLink = "ROW_ItemLinkDisattivate_Small"
                            cssRiga = "ROW_ItemDisattivate_Small"
                        ElseIf Not oAbilitato Then
                            e.Item.CssClass = "ROW_Disabilitate_Small"
                        ElseIf Not hasAccesso Then
                            e.Item.CssClass = "ROW_Disabilitate_Small"
                        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Then
                            e.Item.CssClass = "ROW_Alternate_Small"
                        Else
                            e.Item.CssClass = "ROW_Normal_Small"
                        End If
                    Catch ex As Exception
                        If e.Item.ItemType = ListItemType.AlternatingItem Then
                            e.Item.CssClass = "ROW_Alternate_Small"
                        End If
                    End Try
                End If


                Dim oCell As New TableCell
                Dim CMNT_Nome As String

                Try
                    CMNT_Nome = e.Item.DataItem("CMNT_Nome")
                    If CMNT_Nome <> "" Then
                        CMNT_Nome = ": " & Replace(CMNT_Nome, "'", "\'")
                    End If
                Catch ex As Exception
                    CMNT_Nome = ""
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
                    Dim oIMGisChiusa As System.Web.UI.WebControls.Image
                    oIMGisChiusa = e.Item.Cells(1).FindControl("IMGisChiusa")

                    If IsNothing(oIMGisChiusa) = False Then
                        Dim ImageBaseDir As String
                        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
                        ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/"

                        oIMGisChiusa.Visible = True
                        oIMGisChiusa.ImageUrl = ImageBaseDir & Me.Resource.getValue("stato.image." & e.Item.DataItem("CMNT_isChiusa"))
                        oIMGisChiusa.AlternateText = Me.Resource.getValue("stato." & e.Item.DataItem("CMNT_isChiusa"))
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
                Try
                    If IsNothing(oLNBdettagli) = False Then
                        Me.Resource.setLinkButton(oLNBdettagli, True, True)
                        oLNBdettagli.CssClass = cssLink
                    End If
                Catch ex As Exception

                End Try

                '' SISTEMARE
                Dim CanSubscribe As Boolean = True
                Try
                    Dim oPersona As New COL_Persona
                    ' Link iscrizione comunità
                    If IsDBNull(e.Item.DataItem("RLPC_TPRL_ID")) Then
                        oLNBiscrivi.Visible = True
                    Else
                        If e.Item.DataItem("RLPC_TPRL_ID") > 0 Then
                            oLNBiscrivi.Visible = False
                            CanSubscribe = False
                        Else
                            oLNBiscrivi.Visible = True
                        End If
                    End If

                    oPersona = Session("objPersona")
                    If e.Item.DataItem("CMNT_CanSubscribe") Then
                        If oLNBiscrivi.Visible = True Then
                            If IsNothing(oLNBiscrivi) = False Then
                                Me.Resource.setLinkButton(oLNBiscrivi, True, False)
                            End If


                            ' iscrizione alla comunità
                            Dim CMNT_dataInizioIscrizione, CMNT_dataFineIscrizione As DateTime
                            If IsDate(e.Item.DataItem("CMNT_dataInizioIscrizione")) Then
                                CMNT_dataInizioIscrizione = e.Item.DataItem("CMNT_dataInizioIscrizione")
                            End If
                            If IsDate(e.Item.DataItem("CMNT_dataFineIscrizione")) Then
                                Dim DataTemp As DateTime
                                CMNT_dataFineIscrizione = e.Item.DataItem("CMNT_dataFineIscrizione")
                                DataTemp = CMNT_dataFineIscrizione.Date()
                                DataTemp = DataTemp.AddHours(23)
                                DataTemp = DataTemp.AddMinutes(59)
                                CMNT_dataFineIscrizione = DataTemp
                            End If
                            If CMNT_dataInizioIscrizione > Now Then
                                ' devo iscrivermi, ma iscrizioni non aperte !
                                CMNT_Nome = CMNT_Nome = Me.Resource.getValue("iscrizioni." & Iscrizioni_code.IscrizioniAperteIl)
                                CMNT_Nome = CMNT_Nome.Replace("#%%#", CMNT_dataInizioIscrizione)
                                oLNBiscrivi.Enabled = False
                                CanSubscribe = False
                            Else
                                If IsDate(CMNT_dataFineIscrizione) Then
                                    If CMNT_dataFineIscrizione < Now And Not Equals(New Date, CMNT_dataFineIscrizione) Then
                                        oLNBiscrivi.Text = Me.Resource.getValue("iscrizioni." & Iscrizioni_code.IscrizioniChiuse)
                                        If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Copisteria Then
                                            oLNBiscrivi.Enabled = True
                                        Else
                                            oLNBiscrivi.Enabled = False
                                            CanSubscribe = False
                                        End If
                                    Else
                                        oLNBiscrivi.Enabled = True
                                    End If
                                Else
                                    oLNBiscrivi.Enabled = True
                                End If
                            End If


                            ' se il numero iscritti è stato superato chiudo la possibilità di iscrivere !!!

                            If e.Item.DataItem("CMNT_Iscritti") >= 0 And oLNBiscrivi.Enabled Then
                                oLNBiscrivi.Enabled = True
                            Else
                                If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Copisteria And oLNBiscrivi.Enabled = True Then
                                    oLNBiscrivi.Enabled = True
                                Else
                                    oLNBiscrivi.Enabled = False
                                    CanSubscribe = False
                                End If
                            End If
                        End If
                    Else
                        If IsNothing(oLNBiscrivi) = False Then
                            Me.Resource.setLinkButton(oLNBiscrivi, True, False)
                            oLNBiscrivi.Enabled = False
                            CanSubscribe = False
                        End If
                    End If
                    If oLNBiscrivi.Enabled Then
                        If e.Item.DataItem("CMNT_Bloccata") Or e.Item.DataItem("CMNT_Archiviata") Then
                            oLNBiscrivi.Enabled = False
                            CanSubscribe = False
                        End If
                    End If


                Catch ex As Exception

                End Try

                If CanSubscribe Then
                    Me.LNBiscriviMultipli.Enabled = True
                End If
                Try
                    Dim oCheckbox As System.Web.UI.HtmlControls.HtmlInputCheckBox
                    oCheckbox = e.Item.Cells(25).FindControl("CBcorso")
                    If Not IsNothing(oCheckbox) Then
                        Try
                            oCheckbox.Visible = CanSubscribe
                            If InStr(Me.HDNcomunitaSelezionate.Value, "," & e.Item.DataItem("CMNT_ID") & ",") > 0 Then
                                If oCheckbox.Visible Then
                                    oCheckbox.Checked = True
                                Else
                                    oCheckbox.Checked = False
                                    Me.HDNcomunitaSelezionate.Value = Replace(Me.HDNcomunitaSelezionate.Value, "," & e.Item.DataItem("CMNT_ID") & ",", ",")
                                End If
                            End If
                            oCheckbox.Value = e.Item.DataItem("CMNT_ID")
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception

                End Try

                ''sistemare
                Try

                    If Not IsNothing(oLNBentra) Then
                        oLNBentra.Visible = visibileEntra
                        oLNBentra.Enabled = hasAccesso
                    End If

                    If visibileEntra Then
                        If IsNothing(oLNBentra) = False Then
                            Dim oStringa As String
                            If e.Item.DataItem("CMNT_Bloccata") Then
                                Me.Resource.setLinkButton(oLNBentra, True, False, True)
                            Else
                                Me.Resource.setLinkButton(oLNBentra, True, False)
                            End If

                            oLNBentra.Text = Replace(oLNBentra.Text, "#%%#", e.Item.DataItem("CMNT_EstesoNoSpan"))
                            oStringa = Me.Resource.getValue(oLNBentra.ID & ".status")

                            If oStringa <> "" Then
                                Try
                                    oStringa = Replace(oStringa, "#%%#", e.Item.DataItem("CMNT_EstesoNoSpan"))
                                    oStringa = oStringa.Replace("'", "\'")
                                    oLNBentra.Attributes.Item("onmouseover") = "window.status='" & oStringa & "';return true"
                                Catch ex As Exception

                                End Try
                            End If
                        End If

                        If IsNothing(oLabelNews) = False AndAlso IsNothing(oLiteralNews) = False Then
                            Dim oCurrent As lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount = Me.PageUtility.CommunityNewsCount(Me.PageUtility.CurrentUser.ID, CInt(e.Item.DataItem("CMNT_ID")))
                            oLabelNews.Visible = (oCurrent.Count > 0) AndAlso oLNBentra.Enabled AndAlso hasAccesso
                            oLiteralNews.Visible = (oCurrent.Count > 0) AndAlso oLNBentra.Enabled AndAlso hasAccesso

                            If oCurrent.Count > 0 Then
                                Dim Url As String = PageUtility.GetCommunityNewsUrl(PageUtility.CurrentUser.ID, CInt(e.Item.DataItem("CMNT_ID")), lm.Modules.NotificationSystem.Domain.ViewModeType.FromFindCommunity)
                                If Url <> "" Then
                                    Me.Resource.setLiteral(oLiteralNews)
                                    oLiteralNews.Text = String.Format(oLiteralNews.Text, Url, oCurrent.Count, Me.BaseUrl & "images/HasNews.gif")
                                Else
                                    oLabelNews.Visible = False
                                    oLiteralNews.Visible = False
                                End If
                            End If
                        End If
                    Else
                        If IsNothing(oLabelNews) = False Then
                            oLabelNews.Visible = False
                        End If
                        If IsNothing(oLiteralNews) = False Then
                            oLiteralNews.Visible = False
                        End If
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try
        End If
    End Sub
    Sub DGComunita_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGComunita.PageIndexChanged
        Me.ViewState("intCurPage") = e.NewPageIndex
        DGComunita.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia()
    End Sub
    Private Sub DGComunita_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGComunita.ItemCommand
        If e.CommandName = "Iscrivi" Or e.CommandName = "dettagli" Or e.CommandName = "Login" Or e.CommandName = "legginews" Then

            Dim oComunita As New COL_Comunita
            Dim oPersona As New COL_Persona
            Dim isAttivoForIscrizione As Boolean = True
            Dim ComunitaID, PersonaID As Integer
            Dim ComunitaPath As String
            Dim isChiusaForPadre As Boolean

            ComunitaID = CInt(DGComunita.DataKeys.Item(e.Item.ItemIndex))
            ComunitaPath = DGComunita.Items(e.Item.ItemIndex).Cells(11).Text()
            isChiusaForPadre = CBool(e.Item.Cells(20).Text)
            oComunita.Id = ComunitaID
            oComunita.Estrai()
            oPersona = Session("objPersona")
            PersonaID = oPersona.ID

            If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.Copisteria Then
                isAttivoForIscrizione = oComunita.HasAccessoCopisteria
            End If
            Select Case e.CommandName
                Case "Login"
                    Me.EntraComunita(ComunitaID, ComunitaPath)
                Case "Iscrivi"
                    'richiamo la sub che effettua l'iscrizione della persona

                    Dim oImpostazioni As New COL_ImpostazioniUtente
                    Dim exitSub As Boolean = False
                    Try
                        oImpostazioni = Session("oImpostazioni")
                        exitSub = Not oImpostazioni.ShowConferma
                    Catch ex As Exception
                        exitSub = False
                    End Try

                    Me.ResetFormAll()
                    If isAttivoForIscrizione And (oComunita.CanSubscribe And oComunita.Archiviata = False And oComunita.Bloccata = False) Then
                        If Not exitSub Then
                            Session("azione") = "iscrivi"
                            Me.HDNcmnt_ID.Value = ComunitaID
                            Me.HDisChiusa.Value = isChiusaForPadre
                            Me.HDNcmnt_Path.Value = ComunitaPath
                            Me.ResetFormToConferma(False, e.Item.Cells(25).Text, e.Item.Cells(5).Text)
                        Else
                            If Session("azione") <> "iscrivi" Then
                                Dim iResponse As Main.ErroriIscrizioneComunita
                                Dim oUtility As New OLDpageUtility(Me.Context)

                                Session("azione") = "iscrivi"
                                iResponse = oPersona.IscrizioneComunitaNew(ComunitaID, ComunitaPath, isChiusaForPadre, Server.MapPath("./../profili/") & PersonaID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                                If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
                                    Me.Reset_ToMessaggi()
                                    Me.LBMessaggi.Text = Me.Resource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                                Else
                                    Me.LBiscrizione.Text = Me.Resource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))


                                    Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                                    If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                                        oServiceUtility.NotifyAddSelfSubscription(ComunitaID, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                                    Else
                                        oServiceUtility.NotifyAddWaitingSubscription(ComunitaID, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                                    End If


                                    Me.Reset_ToIscrizioneAvvenuta()
                                End If
                            Else
                                Session("azione") = "loaded"
                                Me.Reset_Contenuto(True, True)
                            End If
                        End If
                    Else
                        Dim alertMSG As String = ""
                        If Not isAttivoForIscrizione Then
                            alertMSG = Me.Resource.getValue("messaggio.BloccataForCopisteria")
                            If alertMSG <> "" Then
                                alertMSG = alertMSG.Replace("'", "\'")
                            Else
                                alertMSG = "Non è possibile iscriversi ad una comunità bloccata !"
                            End If
                        ElseIf Not oComunita.Bloccata Then
                            alertMSG = Me.Resource.getValue("messaggio.Bloccata")
                            If alertMSG <> "" Then
                                alertMSG = alertMSG.Replace("'", "\'")
                            Else
                                alertMSG = "Non è possibile iscriversi ad una comunità bloccata !"
                            End If
                        ElseIf Not oComunita.Archiviata Then
                            alertMSG = Me.Resource.getValue("messaggio.Archiviata")
                            If alertMSG <> "" Then
                                alertMSG = alertMSG.Replace("'", "\'")
                            Else
                                alertMSG = "Non è possibile iscriversi ad una comunità archiviata !"
                            End If
                        Else
                            alertMSG = Me.Resource.getValue("messaggio.NoIscrizione")
                            If alertMSG <> "" Then
                                alertMSG = alertMSG.Replace("'", "\'")
                            Else
                                alertMSG = "Non è possibile iscriversi alla comunità selezionata !"
                            End If
                        End If

                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")

                        Session("azione") = "loaded"
                        Me.Reset_Contenuto(True, True)
                    End If

                Case "dettagli"
                    Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_ElencaComunita.Codex).ID, ActionType.CommunityDetails, Me.PageUtility.CreateObjectsList(ObjectType.Community, ComunitaID), InteractionType.Generic)

                    Dim oRuoloComunita As New COL_RuoloPersonaComunita
                    Try
                        oRuoloComunita.Estrai(ComunitaID, PersonaID)
                        If oRuoloComunita.Errore = Errori_Db.None Then
                            If oRuoloComunita.TipoRuolo.Id > -1 Then
                                Me.LNBentraDettagli.Visible = True
                                If oRuoloComunita.Abilitato And oRuoloComunita.Attivato Then
                                    Me.LNBentraDettagli.Enabled = True
                                Else
                                    Me.LNBentraDettagli.Enabled = False
                                End If
                                Me.LNBiscriviDettagli.Visible = False
                            Else
                                Me.LNBiscriviDettagli.Visible = True
                                Me.LNBiscriviDettagli.Enabled = True
                            End If
                        Else
                            Me.LNBentraDettagli.Visible = False
                            Me.LNBiscriviDettagli.Visible = True
                            Me.LNBiscriviDettagli.Enabled = True
                        End If
                        If Me.LNBiscriviDettagli.Enabled = True Then
                            If oComunita.DataInizioIscrizione > Now Then
                                Me.LNBiscriviDettagli.Visible = False
                            Else
                                If Not Equals(New Date, oComunita.DataFineIscrizione) Then
                                    Dim DataTemp As DateTime
                                    DataTemp = oComunita.DataFineIscrizione.Date
                                    DataTemp = DataTemp.AddHours(23)
                                    DataTemp = DataTemp.AddMinutes(59)
                                    If DataTemp < Now Then
                                        Me.LNBiscriviDettagli.Visible = False
                                    End If
                                End If
                            End If
                        End If

                        If oComunita.Bloccata Or oComunita.Archiviata Or Not isAttivoForIscrizione Or Not oComunita.CanSubscribe Then
                            Me.LNBiscriviDettagli.Enabled = False
                        End If
                        If oComunita.Bloccata Then
                            Me.LNBentraDettagli.Enabled = False
                        End If
                    Catch ex As Exception
                        Me.LNBentraDettagli.Visible = False
                        Me.LNBiscriviDettagli.Visible = False
                    End Try

                    Me.HDNisChiusaForPadre.Value = isChiusaForPadre
                    Me.HDNcmnt_Path.Value = ComunitaPath
                    Me.HDNcmnt_ID.Value = ComunitaID
                    Me.VisualizzaDettagli(ComunitaID)
                Case "legginews"
                    Me.SaveSearchParameters()
                    Session("CMNT_path_forNews") = ComunitaPath
                    Session("CMNT_ID_forNews") = ComunitaID
                    Me.PageUtility.RedirectToUrl("generici/News_Comunita.aspx?from=RicercaComunita")
            End Select
        End If
    End Sub

    Private Function CreaLegenda() As Table
        Dim oTable As New Table
        Dim oRow As New TableRow
        Dim oTableCell As New TableCell

        If IsNothing(Resource) Then
            Me.SetCultureSettings()
        End If


        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disattivate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        If Me.RBLricercaByIscrizione.SelectedValue = 0 Then
            oTableCell.Text = Me.Resource.getValue("ISCRIZIONIdisattivate")
        Else
            oTableCell.Text = Me.Resource.getValue("NONattivati")
        End If

        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)


        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disabilitate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        If Me.RBLricercaByIscrizione.SelectedValue = 0 Then
            oTableCell.Text = Me.Resource.getValue("ISCRIZIONInonPossibili")
        Else
            oTableCell.Text = Me.Resource.getValue("NONabilitati")
        End If

        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function
#End Region

#Region "Dettagli"
    Private Sub LNBiscriviDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviDettagli.Click
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

            Me.HDisChiusa.Value = CBool(Me.HDNisChiusaForPadre.Value)
            Me.ResetFormToConferma(False, oComunita.EstraiNomeBylingua(Session("linguaID")), oComunita.GetNomeResponsabile_NomeCreatore())
        Else
            If Session("azione") <> "iscrivi" Then
                Session("azione") = "iscrivi"
                Try
                    Dim iResponse As Main.ErroriIscrizioneComunita
                    Dim oUtility As New OLDpageUtility(Me.Context)
                    Dim oPersona As New COL_Persona


                    oPersona = Session("objPersona")

                    iResponse = oPersona.IscrizioneComunitaNew(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value, Me.HDisChiusa.Value, Server.MapPath("./../profili/") & oPersona.ID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                    If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
                        Me.Reset_ToMessaggi()
                        Me.LBMessaggi.Text = Me.Resource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                    Else
                        Me.LBiscrizione.Text = Me.Resource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                        Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                        If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                            oServiceUtility.NotifyAddSelfSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                        Else
                            oServiceUtility.NotifyAddWaitingSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                        End If
                        Me.Reset_ToIscrizioneAvvenuta()
                    End If
                    Me.HDNcmnt_ID.Value = ""
                    Me.HDisChiusa.Value = ""
                    Me.HDNcmnt_Path.Value = ""
                Catch ex As Exception
                    Session("azione") = "loaded"
                    Me.Reset_Contenuto(True, True)
                End Try
            Else
                Session("azione") = "loaded"
                Me.Reset_Contenuto(True, True)
            End If
        End If
    End Sub
    Private Sub LNBannullaDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaDettagli.Click
        Session("azione") = "loaded"
        Me.Reset_Contenuto(True)
    End Sub
    Private Sub LNBentraDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBentraDettagli.Click
        Me.EntraComunita(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value)
    End Sub
    Private Sub VisualizzaDettagli(ByVal CMNT_Id As Integer)
        Try
            Reset_ToDettagli()
            Me.CTRLDettagli.SetupDettagliComunita(CMNT_Id)
        Catch ex As Exception
            Me.Reset_Contenuto(True)
        End Try
    End Sub
#End Region

#Region "Entrata Comunita"
    Private Sub EntraComunita(ByVal CMNT_ID As Integer, ByVal CMNT_path As String)
        'Dim oResourceConfig As New ResourceManager
        'oResourceConfig = GetResourceConfig(Session("LinguaCode"))
        'Dim status As lm.Comol.Core.DomainModel.SubscriptionStatus
        'Dim idPerson As Integer = PageUtility.CurrentUser.ID
        'status = PageUtility.AccessToCommunity(idPerson, idCommunity, path, oResourceConfig, True)

        'Dim oTreeComunita As New COL_TreeComunita
        'Try
        '    oTreeComunita.Directory = Server.MapPath(PageUtility.BaseUrl & "profili/") & idPerson & "\"
        '    oTreeComunita.Nome = idPerson & ".xml"
        'Catch ex As Exception

        'End Try
        'Select Case status
        '    Case lm.Comol.Core.DomainModel.SubscriptionStatus.activemember
        '        Exit Sub
        '    Case lm.Comol.Core.DomainModel.SubscriptionStatus.blocked
        '        Me.Reset_ToMessaggi()
        '        Resource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & Me.StringaAbilitato.bloccato)
        '        oTreeComunita.CambiaAttivazione(idCommunity, False, oResourceConfig)
        '    Case lm.Comol.Core.DomainModel.SubscriptionStatus.waiting
        '        Me.Reset_ToMessaggi()
        '        Resource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & Me.StringaAbilitato.inAttesa)
        '        oTreeComunita.CambiaAbilitazione(idCommunity, False)

        '    Case lm.Comol.Core.DomainModel.SubscriptionStatus.communityblocked
        '        Me.Reset_ToMessaggi()
        '        Me.Resource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & StringaAbilitato.comunitaBloccata)
        '        oTreeComunita.CambiaAbilitazione(idCommunity, False)
        '        oTreeComunita.CambiaIsBloccata(idCommunity, True)
        '    Case Else
        '        oTreeComunita.Delete(idCommunity, path)
        'End Select

        Dim oTreeComunita As New COL_TreeComunita
        Dim oPersona As New COL_Persona
        Dim PRSN_ID, RuoloID As Integer

        Try
            oPersona = Session("objPersona")
            PRSN_ID = oPersona.ID

            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
            oTreeComunita.Nome = PRSN_ID & ".xml"
        Catch ex As Exception

        End Try

        Try
            Dim oRuolo As New COL_RuoloPersonaComunita
            oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)
            If oRuolo.Errore = Errori_Db.None Then
                RuoloID = oRuolo.TipoRuolo.Id
            End If

            'verifico se l'utente ha l'abilitazione per fare l'accesso alla comunità

            Dim oComunita As New COL_Comunita

            oComunita.Id = CMNT_ID
            oComunita.Estrai()
            GenericCacheManager.PurgeCacheItems(CachePolicy.PermessiServizioUtente())
            If oComunita.Errore = Errori_Db.None Then
                oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona


                If oRuolo.Abilitato And oRuolo.Attivato Then 'se l'utente è attivato E abilitato allora
                    Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_ElencaComunita.Codex).ID, ActionType.CommunityDetails, Me.PageUtility.CreateObjectsList(ObjectType.Community, CMNT_ID), InteractionType.Generic)

                    ' metto in sessione i permessi che l'utente ha per quella comunità
                    Dim i, j As Integer
                    Session("IdRuolo") = RuoloID
                    Session("IdComunita") = CMNT_ID


                    Dim Elenco_CMNT_ID() As String
                    Elenco_CMNT_ID = CMNT_Path.Split(".")

                    Dim totale As Integer
                    Dim ArrComunita(,) As String

                    With oComunita
                        Session("ORGN_id") = .Organizzazione.Id
                        Try
                            Dim oDataSet As New DataSet
                            oDataSet = COL_Servizio.ElencaByTipoRuoloByComunita(Session("IdRuolo"), CMNT_ID)
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



                        'Aggiorno gli array relativi al menu history !!!


                        Dim tempArray(,) As String, Path As String = ""
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
                    ' Dim oUtility As New OLDpageUtility(Me.Context)


                    Try
                        If Session("LogonAs") = False Then
                            oRuolo.UpdateUltimocollegamento()
                        End If
                    Catch ex As Exception

                    End Try
                    oComunita.RegistraAccesso(CMNT_ID, PRSN_ID, Me.PageUtility.SystemSettings.CodiceDB)
                    Me.PageUtility.SendNotificationUpdateCommunityAccess(PRSN_ID, CMNT_ID, oRuolo.UltimoCollegamento)

                    oTreeComunita.Update(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)

                    Session("AdminForChange") = False
                    Session("CMNT_path_forAdmin") = ""
                    Session("idComunita_forAdmin") = ""
                    ' REGISTRAZIONE EVENTO
                    Session("TPCM_ID") = oComunita.TipoComunita.ID

                    Dim defaultUrl As String = PageUtility.GetCommunityDefaultPage(CMNT_ID, PRSN_ID)
                    If COL_Comunita.ShowCover(CMNT_ID, PRSN_ID) Then
                        If oRuolo.SaltaCopertina Then
                            Me.PageUtility.RedirectToUrl(defaultUrl)
                        Else
                            Me.PageUtility.RedirectToUrl("Generici/Cover.aspx")
                        End If
                    Else
                        Me.PageUtility.RedirectToUrl(defaultUrl)
                    End If
                ElseIf oRuolo.Attivato = False Then
                    Me.Reset_ToMessaggi()
                    'Me.LBMessaggi.Text = "Non è possibile loggarsi! Non si è stati attivati"
                    Me.Resource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & StringaAbilitato.bloccato)
                    oTreeComunita.CambiaAttivazione(CMNT_ID, False, Resource)
                ElseIf oRuolo.Abilitato = False Then
                    Me.Reset_ToMessaggi()
                    Me.Resource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & StringaAbilitato.inAttesa)
                    oTreeComunita.CambiaAbilitazione(CMNT_ID, False)

                ElseIf oComunita.Bloccata = True Then
                    Me.Reset_ToMessaggi()
                    Me.Resource.setLabel_To_Value(Me.LBMessaggi, "LBMessaggi." & StringaAbilitato.comunitaBloccata)
                    oTreeComunita.CambiaAbilitazione(CMNT_ID, False)
                    oTreeComunita.CambiaIsBloccata(CMNT_ID, True)
                End If
            Else
                oTreeComunita.Delete(CMNT_ID, CMNT_Path)
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region


    Private Sub LNBiscriviConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviConferma.Click
        Dim iResponse As Main.ErroriIscrizioneComunita
        Dim oPersona As New COL_Persona

        If Session("azione") = "iscrivi" Then
            Me.PageUtility.AddActionToModule(Me.PageUtility.GetModule(Services_IscrizioneComunita.Codex).ID, Services_IscrizioneComunita.ActionType.EnterCommunity, Me.PageUtility.CreateObjectsList(Services_IscrizioneComunita.ObjectType.Community, Me.HDNcmnt_ID.Value))

            Me.PNLconferma.Visible = False
            Try
                oPersona = Session("objPersona")

                iResponse = oPersona.IscrizioneComunitaNew(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value, Me.HDisChiusa.Value, Server.MapPath("./../profili/") & oPersona.ID & "\", "./../", Me.PageUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), Me.PageUtility.LocalizedMail, Me.PageUtility.SystemSettings.Login.DaysToUpdateProfile)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                If iResponse <> Main.ErroriIscrizioneComunita.IscrizioneAvvenuta And iResponse <> Main.ErroriIscrizioneComunita.IscrizioneInAttesa Then
                    Me.Reset_ToMessaggi()
                    Me.LBMessaggi.Text = Me.Resource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                Else
                    Me.LBiscrizione.Text = Me.Resource.getValue("ErroriIscrizioneComunita." & CType(iResponse, Main.ErroriIscrizioneComunita))
                    Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
                    If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                        oServiceUtility.NotifyAddSelfSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                    Else
                        oServiceUtility.NotifyAddWaitingSubscription(Me.HDNcmnt_ID.Value, Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                    End If
                    Me.Reset_ToIscrizioneAvvenuta()
                End If
                Me.HDNcmnt_ID.Value = ""
                Me.HDisChiusa.Value = ""
                Me.HDNcmnt_Path.Value = ""
            Catch ex As Exception
                Me.Reset_Contenuto(True, True)
            End Try
        ElseIf Session("azione") = "iscriviMultipli" Then
            Try
                Me.IscrizioneMultipla(False)
            Catch ex As Exception
                Me.Reset_Contenuto(True, True)
            End Try
        Else
            Session("azione") = "loaded"
            Me.Reset_Contenuto(True, True)
        End If
    End Sub
    Private Sub LNBannullaConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaConferma.Click
        Me.Reset_Contenuto(True)
    End Sub

    'Private Sub LNBalberoGerarchico_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBalberoGerarchico.Click
    '    Me.SaveSearchParameters() '2)
    '    Me.PageUtility.RedirectToUrl("comunita/RicercaComunitaAlbero.aspx?show=2&from=ricerca")
    'End Sub
    'Private Sub LNBalbero_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBalbero.Click
    '    Me.SaveSearchParameters() '1)
    '    Me.PageUtility.RedirectToUrl("comunita/RicercaComunitaAlbero.aspx?show=1&from=ricerca")
    'End Sub

    Private Sub LNBelencoIscritte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBelencoIscritte.Click
        Me.PageUtility.RedirectToUrl("comunita/EntrataComunita.aspx")
    End Sub
    Private Sub LNBiscriviAltre_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviAltre.Click
        Me.Reset_Contenuto(True)
    End Sub

    Private Sub LNBannulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannulla.Click
        Me.Reset_Contenuto(True)
    End Sub

    Private Sub IscrizioneMultipla(ByVal isForconferma As Boolean)
        Dim i, totale As Integer
        Dim oDataset As DataSet
        Dim oDataview As DataView

        oDataset = Me.FiltraggioDatiRistretto
        oDataview = oDataset.Tables(0).DefaultView
        oDataview.RowFilter = "'" & Me.HDNcomunitaSelezionate.Value & "' like '%,' + CMNT_ID +',%'"
        totale = oDataview.Count

        If isForconferma Then
            Dim ListaComunita As String = ""
            For i = 0 To totale - 1
                Dim oRow As DataRow
                oRow = oDataview.Item(i).Row

                If ListaComunita = "" Then
                    ListaComunita = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome") & " - "
                Else
                    ListaComunita &= "<li>" & oRow.Item("CMNT_Nome") & " - "
                End If
                If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                    ListaComunita &= oRow.Item("AnagraficaCreatore") & vbCrLf
                Else
                    If oRow.Item("CMNT_Responsabile") = "" Then
                        ListaComunita &= oRow.Item("AnagraficaCreatore") & vbCrLf
                    Else
                        ListaComunita &= oRow.Item("CMNT_Responsabile") & vbCrLf
                    End If
                End If
            Next
            If ListaComunita <> "" Then
                ListaComunita &= "</ul>"
            End If
            Me.ResetFormToConferma(True, ListaComunita, "")
        Else
            Dim ListaLimiteSuperato As String = "", ListaIscrizioneInAttesa As String = "", ListaIscrizioneAvvenuta As String = "", ListaErroreGenerico As String = ""
            Dim iResponse As Main.ErroriIscrizioneComunita
            Dim oUtility As New OLDpageUtility(Me.Context)
            Dim oPersona As New COL_Persona

            oPersona = Session("objPersona")

            Dim oServiceUtility As New SubscriptionNotificationUtility(oUtility)
            For i = 0 To totale - 1
                Dim oRow As DataRow
                oRow = oDataview.Item(i).Row

                iResponse = oPersona.IscrizioneComunitaNew(oRow.Item("CMNT_ID"), oRow.Item("ALCM_Path"), oRow.Item("ALCM_isChiusaForPadre"), Server.MapPath("./../profili/") & oPersona.ID & "\", "./../", oUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), oUtility.LocalizedMail, oUtility.SystemSettings.Login.DaysToUpdateProfile)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))
                Select Case iResponse
                    Case Main.ErroriIscrizioneComunita.LimiteSuperato
                        If ListaLimiteSuperato = "" Then
                            ListaLimiteSuperato = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        Else
                            ListaLimiteSuperato &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        End If
                    Case Main.ErroriIscrizioneComunita.IscrizioneInAttesa
                        oServiceUtility.NotifyAddWaitingSubscription(oRow.Item("CMNT_ID"), Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                        If ListaIscrizioneInAttesa = "" Then
                            ListaIscrizioneInAttesa = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        Else
                            ListaIscrizioneInAttesa &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        End If
                    Case Main.ErroriIscrizioneComunita.IscrizioneAvvenuta
                        oServiceUtility.NotifyAddSelfSubscription(oRow.Item("CMNT_ID"), Me.PageUtility.CurrentUser.ID, Me.PageUtility.CurrentUser.Anagrafica)
                        If ListaIscrizioneAvvenuta = "" Then
                            ListaIscrizioneAvvenuta = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        Else
                            ListaIscrizioneAvvenuta &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        End If
                    Case Main.ErroriIscrizioneComunita.ErroreGenerico
                        If ListaErroreGenerico = "" Then
                            ListaErroreGenerico = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        Else
                            ListaErroreGenerico &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                        End If
                End Select
            Next

            Me.HDNcomunitaSelezionate.Value = ""
            Me.Reset_ToIscrizioneAvvenuta()
            If ListaIscrizioneAvvenuta <> "" Then
                ListaIscrizioneAvvenuta &= "</ul>"
                LBiscrizione.Text = Me.Resource.getValue("ErroriIscrizioneComunita.Multipla." & CType(Main.ErroriIscrizioneComunita.IscrizioneAvvenuta, Main.ErroriIscrizioneComunita))
                LBiscrizione.Text = Replace(LBiscrizione.Text, "#nomeComunita#", ListaIscrizioneAvvenuta) & "<br>"
            End If
            If ListaIscrizioneInAttesa <> "" Then
                ListaIscrizioneInAttesa &= "</ul>"
                LBiscrizione.Text &= Me.Resource.getValue("ErroriIscrizioneComunita.Multipla." & CType(Main.ErroriIscrizioneComunita.IscrizioneInAttesa, Main.ErroriIscrizioneComunita))
                LBiscrizione.Text = Replace(LBiscrizione.Text, "#nomeComunita#", ListaIscrizioneInAttesa) & "<br>"
            End If
            If ListaLimiteSuperato <> "" Then
                ListaLimiteSuperato &= "</ul>"
                LBiscrizione.Text &= Me.Resource.getValue("ErroriIscrizioneComunita.Multipla." & CType(Main.ErroriIscrizioneComunita.LimiteSuperato, Main.ErroriIscrizioneComunita))
                LBiscrizione.Text = Replace(LBiscrizione.Text, "#nomeComunita#", ListaLimiteSuperato) & "<br>"
            End If
            If ListaErroreGenerico <> "" Then
                ListaErroreGenerico &= "</ul>"
                LBiscrizione.Text &= Me.Resource.getValue("ErroriIscrizioneComunita.Multipla." & CType(Main.ErroriIscrizioneComunita.ErroreGenerico, Main.ErroriIscrizioneComunita))
                LBiscrizione.Text = Replace(LBiscrizione.Text, "#nomeComunita#", ListaErroreGenerico) & "<br>"
            End If
        End If
    End Sub
    Private Sub LNBiscriviMultipli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviMultipli.Click
        'Dim i, totale, CMNT_ID As Integer
        'Dim oDataset As DataSet
        'Dim oDataview As DataView

        If Session("azione") <> "iscriviMultipli" Then
            Try
                Dim oImpostazioni As New COL_ImpostazioniUtente
                Dim isRequiredConfirm As Boolean = False
                Dim ListaComunita As String = ""
                Try
                    oImpostazioni = Session("oImpostazioni")
                    isRequiredConfirm = oImpostazioni.ShowConferma
                Catch ex As Exception

                End Try
                'Dim iResponse As Main.ErroriIscrizioneComunita
                'Dim oResourceConfig As New ResourceManager
                'Dim oPersona As New COL_Persona

                'oResourceConfig = GetResourceConfig(Session("LinguaCode"))
                'oPersona = Session("objPersona")

                'oDataset = Me.FiltraggioDatiRistretto
                'oDataview = oDataset.Tables(0).DefaultView
                'oDataview.RowFilter = "'" & Me.HDNcomunitaSelezionate.Value & "' like '%,' + CMNT_ID +',%'"
                'totale = oDataview.Count

                'Dim ListaLimiteSuperato, ListaIscrizioneInAttesa, ListaIscrizioneAvvenuta, ListaErroreGenerico As String

                'For i = 0 To totale - 1
                '    Dim oRow As DataRow
                '    oRow = oDataview.Item(i).Row

                '    If isRequiredConfirm Then
                '        If ListaComunita = "" Then
                '            ListaComunita = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome") & " - "
                '        Else
                '            ListaComunita &= "<li>" & oRow.Item("CMNT_Nome") & " - "
                '        End If
                '        If IsDBNull(oRow.Item("CMNT_Responsabile")) Then
                '            ListaComunita &= oRow.Item("AnagraficaCreatore") & vbCrLf
                '        Else
                '            If oRow.Item("CMNT_Responsabile") = "" Then
                '                ListaComunita &= oRow.Item("AnagraficaCreatore") & vbCrLf
                '            Else
                '                ListaComunita &= oRow.Item("CMNT_Responsabile") & vbCrLf
                '            End If
                '        End If
                '    Else
                '        iResponse = oPersona.IscrizioneComunitaNew(oRow.Item("CMNT_ID"), oRow.Item("ALCM_Path"), oRow.Item("ALCM_isChiusaForPadre"), Server.MapPath("./../profili/") & oPersona.Id & "\", "./../",oUtility.ApplicationUrlBase, Session.SessionID, Session("LinguaCode"), oResourceConfig)
                '        Select Case iResponse
                '            Case Main.ErroriIscrizioneComunita.LimiteSuperato
                '                If ListaLimiteSuperato = "" Then
                '                    ListaLimiteSuperato = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                '                Else
                '                    ListaLimiteSuperato &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                '                End If
                '            Case Main.ErroriIscrizioneComunita.IscrizioneInAttesa
                '                If ListaIscrizioneInAttesa = "" Then
                '                    ListaIscrizioneInAttesa = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                '                Else
                '                    ListaIscrizioneInAttesa &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                '                End If
                '            Case Main.ErroriIscrizioneComunita.IscrizioneAvvenuta
                '                If ListaIscrizioneAvvenuta = "" Then
                '                    ListaIscrizioneAvvenuta = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                '                Else
                '                    ListaIscrizioneAvvenuta &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                '                End If
                '            Case Main.ErroriIscrizioneComunita.ErroreGenerico
                '                If ListaErroreGenerico = "" Then
                '                    ListaErroreGenerico = "<ul>" & vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                '                Else
                '                    ListaErroreGenerico &= vbCrLf & "<li>" & oRow.Item("CMNT_Nome")
                '                End If

                '        End Select
                '    End If
                'Next

                If isRequiredConfirm Then
                    Me.IscrizioneMultipla(True)
                    Session("azione") = "iscriviMultipli"
                Else
                    Me.IscrizioneMultipla(False)
                End If
            Catch ex As Exception

            End Try
        Else
            Session("azione") = "loaded"
            Me.Reset_Contenuto(True, True)
        End If
    End Sub

    Public ReadOnly Property BodyId As String
        Get
            Return Me.Master.BodyIdCode
        End Get
    End Property
    'Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
    '    Get
    '        Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
    '    End Get
    'End Property
End Class