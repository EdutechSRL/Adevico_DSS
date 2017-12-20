Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Eventi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class Modifica_Evento
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager

    Private _PageUtility As PresentationLayer.OLDpageUtility
    Protected Friend ReadOnly Property PageUtility() As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property

    Private Enum Inserimento
        DataErrata = -7
        ErroreModifica = -6
        DataMancante = -5
        ErroreProgramma = -4
        ErroreCreazioneOrari = -3
        ErroreCreazionePersonale = -2
        ErroreCreazione = -1
        ErroreGenerico = 0
        EventoInserito = 1
        EventoModificato = 2
        Gi‡Modificato = 3
    End Enum

    Protected WithEvents HDNselezionato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LNBcalendario As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBricerca As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBmodifica As System.Web.UI.WebControls.LinkButton

    Protected WithEvents TBReditorNote As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBReditorProgramma As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRaula As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRluogo As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRore As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRcalendari As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRavvisoPersonale As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRannoAccademico As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRmacro As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRdate As System.Web.UI.WebControls.TableRow

    Protected WithEvents LBnote_t As System.Web.UI.WebControls.Label
    Protected WithEvents TBLnote As System.Web.UI.WebControls.Table
    Protected WithEvents LNBmodificaNote As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBsalvaNote As System.Web.UI.WebControls.LinkButton

    Protected WithEvents LBProgramma_t As System.Web.UI.WebControls.Label
    Protected WithEvents CTRLeditorProgramma As UC_Editor
    Protected WithEvents CTRLeditorNote As UC_Editor
    Protected WithEvents TBLprogramma As System.Web.UI.WebControls.Table
    Protected WithEvents LNBmodificaProgramma As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBsalvaProgramma As System.Web.UI.WebControls.LinkButton


    Protected WithEvents TBRprogramma As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBprogrammaNormale_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBprogramma As System.Web.UI.WebControls.TextBox
    Protected WithEvents TBRnote As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBnoteNormale_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBnote As System.Web.UI.WebControls.TextBox

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: la seguente dichiarazione Ë richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo Ë richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

#Region "generale"
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBinfo As System.Web.UI.WebControls.Label
    Protected WithEvents LBnopermessi As System.Web.UI.WebControls.Label
#End Region

#Region "dati"
    Protected WithEvents DDLCategoria As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBNomeEvento As System.Web.UI.WebControls.TextBox
    Protected WithEvents DDLAnnoAccademico As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLOra1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLOra2 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLMinuti1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLMinuti2 As System.Web.UI.WebControls.DropDownList

	Protected WithEvents LBdataI As System.Web.UI.WebControls.Label
	Protected WithEvents LBdataF As System.Web.UI.WebControls.Label
	Protected WithEvents HDNdataF As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDNdataI As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents TBLink As System.Web.UI.WebControls.TextBox
    Protected WithEvents TBLuogo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TBAula As System.Web.UI.WebControls.TextBox
    Protected WithEvents CBVisibile As System.Web.UI.WebControls.CheckBox
    Protected WithEvents DDLTipoAvviso As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBAvviso As System.Web.UI.WebControls.Label
    Protected WithEvents LBAnnoAccademico As System.Web.UI.WebControls.Label
    Protected WithEvents LBAula As System.Web.UI.WebControls.Label
    Protected WithEvents LBProgramma As System.Web.UI.WebControls.Label
    Protected WithEvents LBvisualizzazione As System.Web.UI.WebControls.Label
    Protected WithEvents PNLinfo As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLMain As System.Web.UI.WebControls.Panel
    Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBNomeEvento As System.Web.UI.WebControls.Label
    Protected WithEvents RFVNomeEvento As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents REVNomeEvento As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents LBCategoria As System.Web.UI.WebControls.Label
    Protected WithEvents LBdataInizio As System.Web.UI.WebControls.Label
    Protected WithEvents LBDataFine As System.Web.UI.WebControls.Label
    Protected WithEvents LBLOraInizio As System.Web.UI.WebControls.Label
    Protected WithEvents LBLOra1 As System.Web.UI.WebControls.Label
    Protected WithEvents LBLMinuti1 As System.Web.UI.WebControls.Label
    Protected WithEvents LBLOraFine As System.Web.UI.WebControls.Label
    Protected WithEvents LBLOra2 As System.Web.UI.WebControls.Label
    Protected WithEvents LBLMinuti2 As System.Web.UI.WebControls.Label
    Protected WithEvents LBLuogo As System.Web.UI.WebControls.Label
    Protected WithEvents REVLuogo As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents REVAula As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents LBNote As System.Web.UI.WebControls.Label
    Protected WithEvents LBLink As System.Web.UI.WebControls.Label
    Protected WithEvents LBMacro As System.Web.UI.WebControls.Label
    Protected WithEvents CBMacro As System.Web.UI.WebControls.CheckBox
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        If Not Page.IsPostBack Then
            Me.DDLCategoria.Attributes.Add("onchange", "return AggiornaForm();")
            Session("Azione") = "insert"
            SetupInternazionalizzazione()
            If Request.QueryString("from") = "trova" Then
                Me.LNBricerca.Visible = True
            Else
                Me.LNBricerca.Visible = False
            End If
        End If

        If Not Page.IsPostBack Then
            Dim oServizio As New UCServices.Services_Eventi
            Try
                Dim oPersona As New COL_Persona
                If IsNumeric(Session("Evento_CMNT_id")) Then
                    oPersona = Session("objPersona")
                    oServizio.PermessiAssociati = oPersona.GetPermessiForServizio(Session("Evento_CMNT_id"), oServizio.Codex)
                Else
                    oServizio.PermessiAssociati = Permessi(oServizio.Codex, Me.Page)
                End If
            Catch ex As Exception
                oServizio.PermessiAssociati = "00000000000000000000000000000000"
            End Try

            Me.SetupInternazionalizzazione()

            Dim forRemainder As Boolean = False
            Try
                If Session("ReminderID") > 0 Then
                    forRemainder = True
                End If
            Catch ex As Exception

            End Try

            If Not forRemainder Then
                If oServizio.ChangeEvents Or oServizio.DelEvents Or oServizio.AdminService Then
                    Me.Bind_Dati(forRemainder)
                    Me.PNLpermessi.Visible = False
                    Me.PNLcontenuto.Visible = True
                    Me.LNBmodifica.Visible = True
                Else
                    Me.PNLpermessi.Visible = True
                    Me.PNLcontenuto.Visible = False
                    Me.LNBmodifica.Visible = False
                End If
            Else
                Me.Bind_Dati(forRemainder)
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
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Me.Response.Redirect(PageUtility.GetDefaultLogoutPage, True)
            Return True
        Else
            Return False
        End If
    End Function

#Region "Bind Dati"
    Private Sub Bind_Dati(ByVal isReminder As Boolean)
        Me.Setup_Script()
        Me.BindDDLOre()
        Me.Bind_CategorieEventi()
        Me.Bind_Campi(isReminder)
    End Sub
    Private Sub Setup_Script()
        Me.TXBnote.Attributes.Add("onkeypress", "return(LimitText(this,4000));")
        Me.TXBprogramma.Attributes.Add("onkeypress", "return(LimitText(this,8000));")
    End Sub
    Private Sub Bind_Campi(ByVal isReminder As Boolean)
        Dim OrarioID, EventoID, ReminderID As Integer

        Try
            OrarioID = Session("OrarioID")
        Catch ex As Exception
            OrarioID = 0
        End Try
        Try
            EventoID = Session("EventoID")
        Catch ex As Exception
            EventoID = 0
        End Try
        Try
            ReminderID = Session("ReminderID")
        Catch ex As Exception
            ReminderID = 0
        End Try


        If isReminder = False And OrarioID > 0 And EventoID > 0 Then
            Dim oEvento As New COL_Evento
            Dim oOrario As New COL_Orario
            Dim oProgrammaEvento As New COL_Programma_Evento
            '  Dim oTipoEvento As New COL_Tipo_Evento


            oEvento.Id = EventoID
            oOrario.Id = OrarioID
            oProgrammaEvento.Id = OrarioID
            oEvento.Estrai()
            oOrario.Estrai()
            oProgrammaEvento.Estrai()

            If oEvento.Errore = Errori_Db.None And oOrario.Errore = Errori_Db.None Then
                Try
                    Me.DDLCategoria.SelectedValue = oEvento.TipoEvento.Id
                Catch ex As Exception

                End Try
                Me.TXBNomeEvento.Text = oEvento.Nome
                If Me.DDLCategoria.SelectedValue = Main.TipiEvento.Lezione Then
                    Me.Setup_Editor()
                    Me.TBReditorProgramma.Visible = True
                    Me.TBReditorNote.Visible = True
                    Me.TBRnote.Visible = False
                    Me.TBRprogramma.Visible = False
                Else
                    Me.TBReditorProgramma.Visible = False
                    Me.TBReditorNote.Visible = False
                    Me.TBRnote.Visible = True
                    Me.TBRprogramma.Visible = True
                End If

				With oOrario
					Me.LBdataI.Text = Format(CDate(.Inizio), "d/M/yyyy")
					Me.HDNdataI.Value = Format(CDate(.Inizio), "d/M/yyyy")
					Me.DDLOra1.SelectedValue = Format(CDate(.Inizio), "HH")
					Me.DDLMinuti1.SelectedValue = Format(CDate(.Inizio), "mm")

					Me.LBdataF.Text = Format(CDate(.Fine), "d/M/yyyy")
					Me.HDNdataF.Value = Format(CDate(.Fine), "d/M/yyyy")
					Me.DDLOra2.SelectedValue = Format(CDate(.Fine), "HH")
					Me.DDLMinuti2.SelectedValue = Format(CDate(.Fine), "mm")

					If Session("Comando") = "modificaTUTTE" Then
						Me.TBRdate.Visible = False
					Else
						Me.TBRdate.Visible = True
					End If

					Me.TBLink.Text = .Link
					Me.TBAula.Text = .Luogo
					If Me.TBReditorNote.Visible Then
                        CTRLeditorProgramma.HTML = oProgrammaEvento.ProgrammaSvolto
                        CTRLeditorNote.HTML = .Note
						Me.LBProgramma.Text = oProgrammaEvento.ProgrammaSvolto
						Me.LBNote.Text = .Note
					Else
						Me.TXBnote.Text = .Note
						Me.TXBprogramma.Text = oProgrammaEvento.ProgrammaSvolto
					End If
					Me.CBVisibile.Checked = .Visibile
				End With
                Me.TBLuogo.Text = oEvento.Luogo
                Me.Bind_AnniAccademici(oEvento.AnnoAccademico)
                Me.CBMacro.Checked = oEvento.Macro
            Else
                If Request.QueryString("from") = "trova" Then
                    Session("Comando") = Nothing
                    Session("EventoID") = Nothing
                    Session("OrarioID") = Nothing
                    Session("ORRI_id") = Nothing
                    Session("LBVisibileInizio") = Nothing
                    Session("LBVisibileFine") = Nothing
                    Session("Evento_CMNT_id") = Nothing
                    Response.Redirect("./RicercaEvento.aspx?reset=true")
                Else
                    Session("Comando") = Nothing
                    Session("EventoID") = Nothing
                    Session("OrarioID") = Nothing
                    Session("ORRI_id") = Nothing
                    Session("LBVisibileInizio") = Nothing
                    Session("LBVisibileFine") = Nothing
                    Session("Evento_CMNT_id") = Nothing
                    Response.Redirect("./CalendarioSettimanale.aspx")
                End If
            End If
            Me.oResource.setLabel(Me.LBtitolo)
            Try
                Me.HDNselezionato.Value = Me.DDLCategoria.SelectedValue
            Catch ex As Exception

            End Try
        ElseIf isReminder And ReminderID > 0 Then
            Me.LBtitolo.Text = Me.oResource.getValue("LBtitolo.Personale")

            Dim oReminder As New COL_Reminder
            With oReminder
                .Id = ReminderID
                .Estrai_da_id()

                TXBNomeEvento.Text = .Oggetto
				Me.LBdataI.Text = Format(CDate(.Inizio), "d/M/yyyy")
				Me.HDNdataI.Value = Format(CDate(.Inizio), "d/M/yyyy")
                DDLOra1.SelectedValue = Format(CDate(.Inizio), "HH")
                DDLMinuti1.SelectedValue = Format(CDate(.Inizio), "mm")
				Me.LBdataF.Text = Format(CDate(.Fine), "d/M/yyyy")
				Me.HDNdataF.Value = Format(CDate(.Fine), "d/M/yyyy")
                DDLOra2.SelectedValue = Format(CDate(.Fine), "HH")
                DDLMinuti2.SelectedValue = Format(CDate(.Fine), "mm")
                TXBnote.Text = .Testo
                TBLuogo.Text = .Luogo
                TBLink.Text = .Link

                Me.TBRannoAccademico.Visible = False
                Me.TBRaula.Visible = False
                Me.LBvisualizzazione.Visible = False
                Me.CBVisibile.Visible = False
                Me.TBReditorNote.Visible = False
                Me.TBReditorProgramma.Visible = False
                Me.TBRnote.Visible = True
                Me.TBRprogramma.Visible = False
                Me.TBRmacro.Visible = False

                Me.DDLCategoria.Items.Add(New ListItem("evento personale", 0))
                Me.HDNselezionato.Value = 0
                oResource.setDropDownList(DDLCategoria, 0)
                DDLCategoria.SelectedValue = 0
                DDLCategoria.Enabled = False
                Me.Bind_TipoAvviso()
                Try
                    DDLTipoAvviso.SelectedValue = .idTipoAvviso
                Catch ex As Exception

                End Try

                If .idPersona <> Session("objPersona").id Then
                    Me.LNBmodifica.Visible = False
                Else
                    Me.LNBmodifica.Visible = True
                End If
            End With
        Else
            Me.PNLcontenuto.Visible = False
            Me.PNLpermessi.Visible = True
            Me.LNBmodifica.Enabled = False
        End If
    End Sub
    Private Sub Bind_AnniAccademici(Optional ByVal AnnoIniziale As Integer = -1)
        Dim minAnno, maxAnno, i As Integer
        Dim DataAttuale As Date
        Try
            DDLAnnoAccademico.Items.Clear()

            Try
				DataAttuale = CDate(HDNdataI.Value)
            Catch ex As Exception
                DataAttuale = Now
            End Try
            If Not (DataAttuale > Now) Then
                DataAttuale = Now
            End If
            If DataAttuale.Month >= 9 Then
                maxAnno = Year(DataAttuale) + 1
            Else
                maxAnno = Year(DataAttuale)
            End If
            If AnnoIniziale > maxAnno Then
                maxAnno = AnnoIniziale + 1
                minAnno = maxAnno - 10
            ElseIf AnnoIniziale < maxAnno - 10 Then
                minAnno = AnnoIniziale - 1
            Else
                minAnno = maxAnno - 10
            End If


            For i = minAnno To maxAnno
                DDLAnnoAccademico.Items.Insert(0, New ListItem(i - 1 & "-" & i, i - 1))
            Next
            If AnnoIniziale > 0 Then
                Try
                    Me.DDLAnnoAccademico.SelectedValue = AnnoIniziale
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Bind_TipoAvviso()
        Dim oReminder As New COL_Reminder
        DDLTipoAvviso.Items.Clear()
        Try
            Dim oDataset As DataSet
            oDataset = oReminder.Get_Tipo_Avviso(Session("LinguaID"))
            DDLTipoAvviso.DataTextField = "TPAV_descrizione"
            DDLTipoAvviso.DataValueField = "TPAV_id"
            DDLTipoAvviso.DataSource = oDataset
            DDLTipoAvviso.DataBind()
            If oDataset.Tables(0).Rows.Count > 0 Then
                DDLTipoAvviso.Visible = True
                LBAvviso.Visible = True
                DDLTipoAvviso.Enabled = True
                DDLTipoAvviso.SelectedValue = "4"
            Else
                DDLTipoAvviso.Visible = False
                LBAvviso.Visible = False
            End If
        Catch ex As Exception
            DDLTipoAvviso.Items.Clear()
            DDLTipoAvviso.Items.Add(New ListItem(oResource.getValue("Modifica.nessuno"), 4))
            DDLTipoAvviso.Enabled = False
            DDLTipoAvviso.SelectedValue = "4"
        End Try

        DDLTipoAvviso.Visible = True
        LBAvviso.Visible = True
    End Sub

    Private Sub Bind_CategorieEventi()
        Dim oDataset As New DataSet
        Dim oTipoEvento As New COL_Tipo_Evento
        Try
            DDLCategoria.Items.Clear()
            oDataset = oTipoEvento.Elenca(CInt(Session("LinguaID")))
            If oDataset.Tables(0).Rows.Count > 0 Then
                Dim oListItem As New ListItem
                DDLCategoria.DataTextField = "TPEV_nome"
                DDLCategoria.DataValueField = "TPEV_id"
                DDLCategoria.DataSource = oDataset
                DDLCategoria.DataBind()
                DDLCategoria.Enabled = True
            Else
                DDLCategoria.Items.Add(New ListItem(oResource.getValue("Modifica.nessuna"), -1))
                DDLCategoria.Enabled = False
            End If
        Catch ex As Exception
            DDLCategoria.Items.Add(New ListItem(oResource.getValue("Modifica.nessuna"), -1))
            DDLCategoria.Enabled = False
        End Try

    End Sub
    Private Sub BindDDLOre()
        Dim i As Integer
        DDLOra1.Items.Clear()
        DDLOra2.Items.Clear()
        For i = 0 To 23
            DDLOra1.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
            DDLOra2.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
        Next
        i = 0
        DDLMinuti1.Items.Clear()
        DDLMinuti2.Items.Clear()
        For i = 0 To 55 Step 5
            DDLMinuti1.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
            DDLMinuti2.Items.Add(New ListItem(i.ToString("00"), i.ToString("00")))
        Next
    End Sub

    Private Sub Setup_Editor()
        Dim oImpostazioni As New COL_ImpostazioniUtente
        Dim showEditor As Boolean = True 'False

        If Request.Browser.Browser = "IE" And Request.Browser.MajorVersion >= 5 Then
            showEditor = True
        End If
        Try
            If Not IsNothing(Session("oImpostazioni")) Then
                oImpostazioni = Session("oImpostazioni")
                showEditor = oImpostazioni.ApriEditorDiario
            End If
        Catch ex As Exception

        End Try
        If showEditor Then
            Me.LNBmodificaNote.Visible = False
            Me.LNBmodificaProgramma.Visible = False
            Me.LNBsalvaNote.Visible = False
            Me.LNBsalvaProgramma.Visible = False
            CTRLeditorProgramma.Visible = True
            CTRLeditorNote.Visible = True
            Me.TBLnote.Visible = False
            Me.TBLprogramma.Visible = False
        Else
            Me.LNBmodificaNote.Visible = True
            Me.LNBmodificaProgramma.Visible = True
            Me.LNBsalvaNote.Visible = False
            Me.LNBsalvaProgramma.Visible = False
            CTRLeditorProgramma.Visible = False
            CTRLeditorNote.Visible = False
            Me.TBLnote.Visible = True
            Me.TBLprogramma.Visible = True
        End If
    End Sub
    Private Sub DDLCategoria_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLCategoria.SelectedIndexChanged
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If DDLCategoria.SelectedValue = Main.TipiEvento.Reminder Then
            'reminder
            Me.TBRavvisoPersonale.Visible = True
            Me.TBRannoAccademico.Visible = False
            Me.TBRaula.Visible = False
            Me.TBRmacro.Visible = False

            Me.Bind_TipoAvviso()
            LBProgramma.Visible = False
            CBVisibile.Visible = False
            LBvisualizzazione.Visible = False
            Me.TBRnote.Visible = False
            Me.TBRprogramma.Visible = False
            Me.TBReditorProgramma.Visible = False
            Me.TBReditorNote.Visible = False
            Me.LBtitolo.Text = Me.oResource.getValue("LBtitoloPersonale.text")
        Else
            'evento
            Me.oResource.setLabel(Me.LBtitolo)
            Try
                If Me.DDLCategoria.SelectedValue = Main.TipiEvento.Lezione And Me.TBReditorProgramma.Visible = False Then
                    Me.LBProgramma.Text = Me.TXBprogramma.Text
                    Me.LBNote.Text = Me.TXBnote.Text
                    CTRLeditorNote.HTML = Me.LBNote.Text
                    CTRLeditorProgramma.HTML = Me.LBProgramma.Text
                    Me.Setup_Editor()
                ElseIf Me.TBReditorProgramma.Visible And Me.DDLCategoria.SelectedValue = Main.TipiEvento.Lezione Then
                    Me.TXBprogramma.Text = Me.LBProgramma.Text
                    Me.TXBnote.Text = Me.LBNote.Text
                End If
                If Me.DDLCategoria.SelectedValue = Main.TipiEvento.Lezione Then
                    Me.TBRnote.Visible = False
                    Me.TBRprogramma.Visible = False
                    Me.TBReditorProgramma.Visible = True
                    Me.TBReditorNote.Visible = True
                Else
                    Me.TBReditorProgramma.Visible = False
                    Me.TBReditorNote.Visible = False
                    Me.TBRnote.Visible = True
                    Me.TBRprogramma.Visible = True
                End If
            Catch ex As Exception
                Me.TBReditorProgramma.Visible = False
                Me.TBReditorNote.Visible = False
                Me.TBRnote.Visible = True
                Me.TBRprogramma.Visible = True
            End Try

            Me.TBRannoAccademico.Visible = True
            Me.TBRavvisoPersonale.Visible = False
            Me.TBRaula.Visible = True
            Me.TBRmacro.Visible = True

            LBProgramma.Visible = True
            CTRLeditorProgramma.Visible = True
            CBVisibile.Visible = True
            LBvisualizzazione.Visible = True
        End If
    End Sub
#End Region

#Region "Pulsanti Menu"
    Private Sub LNBmodifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmodifica.Click
        Dim iResponse As Inserimento
        Dim OrarioID, EventoID, ReminderID As Integer

        Try
            OrarioID = Session("OrarioID")
        Catch ex As Exception
            OrarioID = 0
        End Try
        Try
            EventoID = Session("EventoID")
        Catch ex As Exception
            EventoID = 0
        End Try
        Try
            ReminderID = Session("ReminderID")
        Catch ex As Exception
            ReminderID = 0
        End Try
		If CDate(Me.HDNdataI.Value) = CDate(Me.HDNdataF.Value) Then
			If (DDLOra2.SelectedIndex < DDLOra1.SelectedIndex) Or (DDLOra2.SelectedIndex = DDLOra1.SelectedIndex And DDLMinuti2.SelectedIndex < DDLMinuti1.SelectedIndex) Then
				Response.Write("<script language=Javascript>alert('" & Replace(Me.oResource.getValue("erroreData"), "'", "\'") & "');</script>")
				Exit Sub
			End If
		End If

        If DDLCategoria.SelectedValue <> Main.TipiEvento.Reminder Then
            iResponse = Me.ModificaEvento(EventoID, OrarioID)
        ElseIf DDLCategoria.SelectedValue = 0 Then
            iResponse = ModificaReminder(ReminderID)
        End If
        Dim alertMSG As String = ""
        alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
        If alertMSG <> "" Then
            alertMSG = Replace(alertMSG, "'", "\'")
        End If
        If iResponse = Inserimento.EventoModificato Or iResponse = Inserimento.ErroreProgramma Then
            If Request.QueryString("from") = "trova" Then
                Response.Write("<script language='javascript'>function AlertRedirect(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertRedirect('" & alertMSG & "','" & "http://" & Me.Request.Url.Host & GetPercorsoApplicazione(Me.Request) & "/Eventi/RicercaEvento.aspx?reset=true" & "');</script>")
            Else
                Response.Write("<script language='javascript'>function AlertRedirect(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertRedirect('" & alertMSG & "','" & "http://" & Me.Request.Url.Host & GetPercorsoApplicazione(Me.Request) & "/Eventi/CalendarioSettimanale.aspx" & "');</script>")
            End If
            Response.End()
        Else
            Response.Write("<script language=Javascript>alert('" & alertMSG & "');</script>")
        End If
    End Sub
    Private Function ModificaEvento(ByVal EventoID As Integer, ByVal OrarioID As Integer) As Inserimento
        Dim oEvento As New COL_Evento
        Dim oOrario As New COL_Orario
        Dim oProgrammaEvento As New COL_Programma_Evento

        If Session("Comando") = "modifica" Or Session("Comando") = "modificaTUTTE" Then

            oEvento.Id = EventoID
            oEvento.Estrai()
            Dim oServiceUtility As New LessonDiaryNotificationUtility(Me.PageUtility)
			If Session("Comando") = "modifica" And Not ((CDate(Me.HDNdataI.Value & " " & DDLOra1.SelectedValue & ":" & DDLMinuti1.SelectedValue)) < (CDate(Me.HDNdataF.Value & " " & DDLOra2.SelectedValue & ":" & DDLMinuti2.SelectedValue))) Then
				Return Inserimento.DataErrata
			End If

            With oEvento
                .AnnoAccademico = DDLAnnoAccademico.SelectedValue
                .Link = ""
                .Luogo = TBLuogo.Text
                .Macro = CBMacro.Checked
                .Nome = TXBNomeEvento.Text
                .Note = ""
                .TipoEvento.Id = Me.DDLCategoria.SelectedValue
                .Visualizza = CBVisibile.Checked
                .idPersona = Session("objPersona").Id
                .Modifica()
                If .Errore = Errori_Db.None Then
                    Dim OLDinizio, OLDfine As DateTime
                    Dim CommunityID As Integer = oEvento.Comunita.Id
                    With oOrario
                        .Id = OrarioID
                        .Estrai()
                        OLDinizio = .Inizio
                        OLDfine = .Fine

                        If Session("Comando") = "modifica" Then
                            .Inizio = Me.HDNdataI.Value & " " & DDLOra1.SelectedValue & ":" & DDLMinuti1.SelectedValue
                            .Fine = Me.HDNdataF.Value & " " & DDLOra2.SelectedValue & ":" & DDLMinuti2.SelectedValue
                        Else
                            .Inizio = .Inizio.ToString("dd/MM/yyyy") & " " & DDLOra1.SelectedValue & ":" & DDLMinuti1.SelectedValue
                            .Fine = .Fine.ToString("dd/MM/yyyy") & " " & DDLOra2.SelectedValue & ":" & DDLMinuti2.SelectedValue
                        End If
                        .Luogo = TBAula.Text
                        .Link = TBLink.Text
                        If Me.TBReditorNote.Visible And Me.TBReditorProgramma.Visible Then
                            If CTRLeditorNote.HTML = "" Then
                                .Note = ""
                            Else
                                .Note = CTRLeditorNote.HTML
                                .Note = Replace(.Note, "<script>", "&lt;script&gt;")
                                .Note = Replace(.Note, "</script>", "&lt;/script&gt;")
                                .Note = Replace(.Note, "<script", "&lt; script")
                                .Note = Replace(.Note, "</script", "&lt;script")

                            End If
                        Else
                            If Me.TXBnote.Text = "" Then
                                .Note = ""
                            Else
                                .Note = Me.TXBnote.Text
                                .Note = Replace(.Note, "<script>", "&lt;script&gt;")
                                .Note = Replace(.Note, "</script>", "&lt;/script&gt;")
                                .Note = Replace(.Note, "<script", "&lt; script")
                                .Note = Replace(.Note, "</script", "&lt;script")
                            End If
                        End If
                        If .Note <> "" Then
                            .Note = Replace(.Note, "</pre>", "")

                            If InStr(.Note, "<pre") > 0 Then
                                Dim startPos, endPos As Integer

                                While InStr(.Note, "<pre") > 0
                                    startPos = InStr(.Note, "<pre")
                                    endPos = InStr(startPos, .Note, ">")
                                    If startPos > 0 And endPos > 0 Then
                                        .Note = .Note.Remove(startPos - 1, endPos - startPos + 1)
                                    End If
                                End While
                            End If
                        End If
                        .Visibile = CBVisibile.Checked
                        .idPersona = Session("objPersona").Id
                    End With
                    If Session("Comando") = "modifica" Then
                        oOrario.Modifica()
                        If oOrario.Errore = Errori_Db.None Then
                            If OLDinizio.Equals(oOrario.Inizio) AndAlso OLDfine.Equals(oOrario.Fine) Then
                                oServiceUtility.NotifyEditItem(CommunityID, oOrario.Id, OLDinizio, OLDfine, oOrario.Visibile)
                            Else
                                oServiceUtility.NotifyMoveItem(CommunityID, oOrario.Id, OLDinizio, OLDfine, oOrario.Inizio, oOrario.Fine, oOrario.Visibile)
                            End If
                        End If
                    Else
                        oOrario.ModificaTutti()
                    End If
                    If oOrario.Errore = Errori_Db.None Then
                        With oProgrammaEvento
                            If Me.TBReditorNote.Visible And Me.TBReditorProgramma.Visible Then
                                If CTRLeditorProgramma.HTML = "" Then
                                    .ProgrammaSvolto = ""
                                Else
                                    .ProgrammaSvolto = CTRLeditorProgramma.HTML
                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "<script>", "&lt;script&gt;")
                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "</script>", "&lt;/script&gt;")
                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "<script", "&lt; script")
                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "</script", "&lt;script")
                                End If
                            Else
                                If Me.TXBprogramma.Text = "" Then
                                    .ProgrammaSvolto = ""
                                Else
                                    .ProgrammaSvolto = Me.TXBprogramma.Text
                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "<script>", "&lt;script&gt;")
                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "</script>", "&lt;/script&gt;")
                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "<script", "&lt; script")
                                    .ProgrammaSvolto = Replace(.ProgrammaSvolto, "</script", "&lt;script")
                                End If
                            End If

                            If .ProgrammaSvolto <> "" Then
                                .ProgrammaSvolto = Replace(.ProgrammaSvolto, "</pre>", "")

                                If InStr(.ProgrammaSvolto, "<pre") > 0 Then
                                    Dim startPos, endPos As Integer

                                    While InStr(.ProgrammaSvolto, "<pre") > 0
                                        startPos = InStr(.ProgrammaSvolto, "<pre")
                                        endPos = InStr(startPos, .ProgrammaSvolto, ">")
                                        If startPos > 0 And endPos > 0 Then
                                            .ProgrammaSvolto = .ProgrammaSvolto.Remove(startPos - 1, endPos - startPos + 1)
                                        End If
                                    End While
                                End If
                            End If
                            .Id = OrarioID
                            If Session("Comando") = "modifica" Then
                                .Modifica()
                            Else
                                .ModificaTutti()
                            End If
                            If .Errore = Errori_Db.None Then
                                Return Inserimento.EventoModificato
                            Else
                                Return Inserimento.ErroreProgramma
                            End If
                        End With
                    Else
                        Return Inserimento.ErroreModifica
                    End If
                Else
                    Return Inserimento.ErroreGenerico
                End If
            End With
        Else
            Return Inserimento.Gi‡Modificato
        End If
    End Function
    Private Function ModificaReminder(ByVal ReminderID As Integer) As Inserimento
        If Session("Comando") = "modifica" Then
			If (CDate(HDNdataI.Value & " " & DDLOra1.SelectedValue & ":" & DDLMinuti1.SelectedValue)) < (CDate(Me.HDNdataF.Value & " " & DDLOra2.SelectedValue & ":" & DDLMinuti2.SelectedValue)) Then
				Try
					Dim oReminder As New COL_Reminder
					With oReminder
						.Id = ReminderID
						.Inizio = Me.HDNdataI.Value & " " & DDLOra1.SelectedValue & ":" & DDLMinuti1.SelectedValue
						.Fine = Me.HDNdataF.Value & " " & DDLOra2.SelectedValue & ":" & DDLMinuti2.SelectedValue
						.idPersona = Session("objPersona").Id
						.idTipoAvviso = Me.DDLTipoAvviso.SelectedValue
						.Link = TBLink.Text
						.Luogo = TBLuogo.Text
						.Oggetto = TXBNomeEvento.Text
						.Sospeso = False
						If Me.TBReditorNote.Visible Then
                            If CTRLeditorNote.HTML = "" Then
                                .Testo = ""
                            Else
                                .Testo = CTRLeditorNote.HTML
                                .Testo = Replace(.Testo, "<script>", "&lt;script&gt;")
                                .Testo = Replace(.Testo, "</script>", "&lt;/script&gt;")
                                .Testo = Replace(.Testo, "<script", "&lt; script")
                                .Testo = Replace(.Testo, "</script", "&lt;script")
                            End If
						Else
							If Me.TXBprogramma.Text = "" Then
								.Testo = ""
							Else
								.Testo = Me.TXBprogramma.Text
								.Testo = Replace(.Testo, "<script>", "&lt;script&gt;")
								.Testo = Replace(.Testo, "</script>", "&lt;/script&gt;")
								.Testo = Replace(.Testo, "<script", "&lt; script")
								.Testo = Replace(.Testo, "</script", "&lt;script")
							End If
						End If
						If .Testo <> "" Then
							.Testo = Replace(.Testo, "</pre>", "")
							If InStr(.Testo, "<pre") > 0 Then
								Dim startPos, endPos As Integer
								While InStr(.Testo, "<pre") > 0
									startPos = InStr(.Testo, "<pre")
									endPos = InStr(startPos, .Testo, ">")
									If startPos > 0 And endPos > 0 Then
										.Testo = .Testo.Remove(startPos - 1, endPos - startPos + 1)
									End If
								End While
							End If
						End If
					End With
					oReminder.Modifica()
					If oReminder.Errore = Errori_Db.None Then
						Session("Comando") = Nothing
						Session("ReminderID") = Nothing
						Return Inserimento.EventoModificato
					Else
						Return Inserimento.ErroreModifica
					End If
				Catch ex As Exception
					Return Inserimento.ErroreGenerico
				End Try
			End If
        Else
            Return Inserimento.Gi‡Modificato
        End If
    End Function
    Private Sub LNBcalendario_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcalendario.Click
        Session("Comando") = Nothing
        Session("EventoID") = Nothing
        Session("OrarioID") = Nothing
        Session("ORRI_id") = Nothing
        Session("LBVisibileInizio") = Nothing
        Session("LBVisibileFine") = Nothing
        Session("Evento_CMNT_id") = Nothing
        Response.Redirect("./CalendarioSettimanale.aspx")
    End Sub
    Private Sub LNBricerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBricerca.Click
        Session("Comando") = Nothing
        Session("EventoID") = Nothing
        Session("OrarioID") = Nothing
        Session("ORRI_id") = Nothing
        Session("LBVisibileInizio") = Nothing
        Session("LBVisibileFine") = Nothing
        Session("Evento_CMNT_id") = Nothing
        Response.Redirect("./RicercaEvento.aspx?reset=true")
    End Sub
#End Region

#Region "Localizzazione"

    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_ModificaEvento"
        oResource.Folder_Level1 = "Eventi"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(LBtitolo)
            .setLabel(LBNomeEvento)
            .setRequiredFieldValidator(RFVNomeEvento, True, False)
            .setRegularExpressionValidator(REVNomeEvento)
            .setLabel(LBCategoria)
            .setLabel(LBdataInizio)
            .setLabel(LBDataFine)
            .setLabel(LBLOraInizio)
            .setLabel(LBLOra1)
            .setLabel(LBLMinuti1)
            .setLabel(LBLOraFine)
            .setLabel(LBLOra2)
            .setLabel(LBLMinuti2)
            .setLabel(LBLuogo)
            .setRegularExpressionValidator(REVLuogo)
            .setLabel(LBAula)
            .setRegularExpressionValidator(REVAula)
            .setLabel(LBnote_t)
            .setLabel(LBProgramma_t)
            .setLabel(LBLink)
            .setLabel(LBAnnoAccademico)
            .setLabel(LBAvviso)
            .setLabel(LBvisualizzazione)
            .setLabel(LBMacro)
            .setCheckBox(CBVisibile)
            .setCheckBox(CBMacro)
            .setLinkButton(Me.LNBcalendario, True, True)
            .setLinkButton(Me.LNBmodifica, True, True)
            .setLinkButton(Me.LNBricerca, True, True)
            .setLinkButton(Me.LNBmodificaNote, True, True)
            .setLinkButton(Me.LNBmodificaProgramma, True, True)
            .setLinkButton(Me.LNBsalvaNote, True, True)
            .setLinkButton(Me.LNBsalvaProgramma, True, True)
        End With
    End Sub
#End Region

    Private Sub LNBsalvaNote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalvaNote.Click
        Dim note As String = ""
        note = CTRLeditorNote.HTML

        note = Replace(note, "<script>", "&lt;script&gt;")
        note = Replace(note, "</script>", "&lt;/script&gt;")
        note = Replace(note, "<script", "&lt; script")
        note = Replace(note, "</script", "&lt;script")
        note = Replace(note, "</pre>", "")

        If InStr(note, "<pre") > 0 Then
            Dim startPos, endPos As Integer

            While InStr(note, "<pre") > 0
                startPos = InStr(note, "<pre")
                endPos = InStr(startPos, note, ">")
                If startPos > 0 And endPos > 0 Then
                    note = note.Remove(startPos - 1, endPos - startPos + 1)
                End If
            End While
        End If
        If IsNothing(note) Then
            note = ""
        Else
            If note.Length > 4000 Then
                note = Left(note, 4000)
            End If
        End If
        Me.LBNote.Text = note
        Me.LNBsalvaNote.Visible = False
        Me.LNBmodificaNote.Visible = True
        CTRLeditorNote.Visible = False
        CTRLeditorNote.IsEnabled = False
        Me.TBLnote.Visible = True
    End Sub
    Private Sub LNBmodificaNote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmodificaNote.Click
        CTRLeditorNote.IsEnabled = True
        CTRLeditorNote.HTML = Me.LBNote.Text
        Me.LNBsalvaNote.Visible = True
        Me.LNBmodificaNote.Visible = False
        Me.TBLnote.Visible = False
        CTRLeditorNote.Visible = True
    End Sub
    Private Sub LNBsalvaProgramma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalvaProgramma.Click
        Dim programmaSvolto As String = ""
        programmaSvolto = CTRLeditorNote.HTML

        If programmaSvolto <> "" Then
            programmaSvolto = Replace(programmaSvolto, "<script>", "&lt;script&gt;")
            programmaSvolto = Replace(programmaSvolto, "</script>", "&lt;/script&gt;")
            programmaSvolto = Replace(programmaSvolto, "<script", "&lt; script")
            programmaSvolto = Replace(programmaSvolto, "</script", "&lt;script")
        End If


        programmaSvolto = Replace(programmaSvolto, "</pre>", "")
        If InStr(programmaSvolto, "<pre") > 0 Then
            Dim startPos, endPos As Integer

            While InStr(programmaSvolto, "<pre") > 0
                startPos = InStr(programmaSvolto, "<pre")
                endPos = InStr(startPos, programmaSvolto, ">")
                If startPos > 0 And endPos > 0 Then
                    programmaSvolto = programmaSvolto.Remove(startPos - 1, endPos - startPos + 1)
                End If
            End While
        End If
        If IsNothing(programmaSvolto) Then
            programmaSvolto = ""
        End If
        Me.LBProgramma.Text = programmaSvolto
        Me.LNBsalvaProgramma.Visible = False
        Me.LNBmodificaProgramma.Visible = True
        CTRLeditorProgramma.Visible = False
        Me.TBLprogramma.Visible = True
    End Sub
    Private Sub LNBmodificaProgramma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmodificaProgramma.Click
        CTRLeditorProgramma.IsEnabled = True
        CTRLeditorProgramma.HTML = Me.LBProgramma.Text
        Me.LNBsalvaProgramma.Visible = True
        Me.LNBmodificaProgramma.Visible = False
        Me.TBLprogramma.Visible = False
        CTRLeditorProgramma.Visible = True
    End Sub

    Public ReadOnly Property CalendarScript As String
        Get
            Dim Var As String

            Try
                Select Case Session("LinguaCode")
                    Case "it-IT"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-it.js" & """" & "></script>"
                    Case "en-US"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-en.js" & """" & "></script>"
                    Case "de-DE"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-de.js" & """" & "></script>"
                    Case "fr-FR"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-fr.js" & """" & "></script>"
                    Case "es-ES"
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-es.js" & """" & "></script>"
                    Case Else
                        Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-en.js" & """" & "></script>"
                End Select
            Catch ex As Exception
                Var = "<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-en.js" & """" & "></script>"
            End Try
            Return Var
        End Get
    End Property
End Class