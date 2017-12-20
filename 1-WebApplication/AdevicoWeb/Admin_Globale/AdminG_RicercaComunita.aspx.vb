Imports Comunita_OnLine.ModuloGenerale
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Public Class AdminG_RicercaComunita
    Inherits System.Web.UI.Page

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

#Region "Filtri Persona"
    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLricercaPersonaBy As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBvaloreSearch As System.Web.UI.WebControls.TextBox

    Protected WithEvents DDLtipoPersona As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNcerca As System.Web.UI.WebControls.Button
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
    Protected WithEvents PNLfiltri As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table
#End Region

#Region "Griglia Dati persona"
    Protected WithEvents DGpersona As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PNLpersona As System.Web.UI.WebControls.Panel

    Protected WithEvents DDLpaginazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents PNLpaginazione As System.Web.UI.WebControls.Panel
    'Pannelli NO Record
    Protected WithEvents PNLnorecordPrsn As System.Web.UI.WebControls.Panel
    Protected WithEvents LBnorecordPrsn As System.Web.UI.WebControls.Label

    Protected WithEvents BTNok As System.Web.UI.WebControls.Button

    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLsceltaPersona As System.Web.UI.WebControls.Panel
#End Region

#Region "Griglia Dati comunita"
    Protected WithEvents PNLfiltriComunita As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLcomunita As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLsceltaComunita As System.Web.UI.WebControls.Panel
    Protected WithEvents DGComunita As System.Web.UI.WebControls.DataGrid
    Protected WithEvents RBLvisualizza As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents HDprsn_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents BTNindietro As System.Web.UI.WebControls.Button
    Protected WithEvents PNLnorecordCmnt As System.Web.UI.WebControls.Panel
    Protected WithEvents LBnorecordCmnt As System.Web.UI.WebControls.Label
    Protected WithEvents IMGEdit As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IMGDettagli As System.Web.UI.WebControls.ImageButton

#End Region
#Region "Filtri comunita"
    Protected WithEvents BTNcercaComunita As System.Web.UI.WebControls.Button
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
    Protected WithEvents DDLtipoRuolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLtipo As System.Web.UI.WebControls.DropDownList
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Page.IsPostBack Then
            Session("idComunita_forAdmin") = 0
            Dim oPersona As New COL_Persona
            oPersona = Session("objPersona")

            oPersona.Istituzione.RagioneSociale = Session("Istituzione")

            Me.SetupFiltri()
            If Me.Request.QueryString("re_set") <> "true" Then
                Me.ViewState("intCurPage") = 0
                Me.ViewState("intAnagrafica") = "-1"
                Me.ViewState("intCurPage2") = 0
                Me.LKBtutti.CssClass = "lettera_Selezionata"
            End If
            SetStartupScript()
        Else
            If Page.IsPostBack() Then
                If Me.PNLpersona.Visible = True Then
                    BindGriglia()
                End If
                If Me.PNLcomunita.Visible = True Then
                    Dim PRSN_ID As Integer
                    PRSN_ID = Me.HDprsn_id.Value
                    Me.Bind_GrigliaComunita(PRSN_ID)
                End If
            End If
        End If

    End Sub
    Private Sub SetStartupScript()
        'aggiunge ai link button le proprietà da visualizzare nella barra di stato
        Dim i As Integer
        For i = Asc("a") To Asc("z") 'status dei link button delle lettere
            Dim oLinkButton As New LinkButton
            oLinkButton = FindControlRecursive(Me.Master, "LKB" & Chr(i))
            Dim Carattere As String = Chr(i)

            If IsNothing(oLinkButton) = False Then

                oLinkButton.Attributes.Add("onclick", "window.status='Lettera " & Carattere.ToUpper & "' ;return true;")
                oLinkButton.Attributes.Add("onmouseover", "window.status='Lettera " & Carattere.ToUpper & "' ;return true;")
                oLinkButton.Attributes.Add("onfocus", "window.status='Lettera " & Carattere.ToUpper & "' ;return true;")
                oLinkButton.Attributes.Add("onmouseout", "window.status='';return true;")
                oLinkButton.ToolTip = "Lettera " & Carattere.ToUpper

            End If
        Next
        'gli altri link button della pagina...

        LKBaltro.Attributes.Add("onclick", "window.status='Altri caratteri' ;return true;")
        LKBaltro.Attributes.Add("onmouseover", "window.status='Altri caratteri' ;return true;")
        LKBaltro.Attributes.Add("onfocus", "window.status='Altri caratteri' ;return true;")
        LKBaltro.Attributes.Add("onmouseout", "window.status='';return true;")

        LKBtutti.Attributes.Add("onclick", "window.status='Tutti i caratteri' ;return true;")
        LKBtutti.Attributes.Add("onmouseover", "window.status='Tutti i caratteri' ;return true;")
        LKBtutti.Attributes.Add("onfocus", "window.status='Tutti i caratteri' ;return true;")
        LKBtutti.Attributes.Add("onmouseout", "window.status='';return true;")

    End Sub


#Region "Bind_Dati + bind griglia persone"

    Private Sub SetupFiltri()
        If Me.Request.QueryString("re_set") = "true" Then
            Me.Bind_Organizzazioni()

            Try
                Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("ListaComunita")("organizzazione")
            Catch ex As Exception
                Me.Response.Cookies("ListaComunita")("organizzazione") = Me.DDLorganizzazione.SelectedValue
            End Try
            Me.Bind_TipoPersona()
            Me.Bind_TipoComunita()
            Me.Bind_TipoRuolo()
            Me.Setup_Visualizzazione(True, True)
            'Da scommentare quando si è fatta parte relativa ai cookie....
            Me.SetupSearchParameters()
            Me.Bind_GrigliaComunita(Me.HDprsn_id.Value)
            Me.PNLsceltaComunita.Visible = True
            Me.PNLsceltaPersona.Visible = False
        Else
            Me.Bind_Organizzazioni()
            Me.Bind_TipoPersona()
            Me.Bind_TipoComunita()
            Me.Bind_TipoRuolo()
            Me.Setup_Visualizzazione(True, True)
            Me.BindGriglia(True)
        End If

    End Sub



    'Gestione Visualizzazione Colonne Datagrid
    Private Sub Setup_Visualizzazione(Optional ByVal salva As Boolean = False, Optional ByVal recupera As Boolean = False)

    End Sub

    'Private Sub _Bind_Filtri()
    '    '  Me.Bind_Organizzazione()
    '    Me.Bind_TipoPersona()
    '    Me.Bind_TipoComunita()
    '    Me.Bind_TipoRuolo()
    'End Sub

    Private Sub Bind_Organizzazioni()
        Dim oDataSet As New DataSet
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona
        Dim ISTT_ID, i, Totale As Integer

        oPersona = Session("objPersona")
        ISTT_ID = oPersona.Istituzione.Id

        Try
            oDataSet = oPersona.GetOrganizzazioniByIstituzione(ISTT_ID)
            Me.DDLorganizzazione.Items.Clear()

            Totale = oDataSet.Tables(0).Rows.Count() - 1
            For i = 0 To Totale
				Dim oListItem As New ListItem
				If IsDBNull(oDataSet.Tables(0).Rows(i).Item("CMNT_nome")) Then
					oListItem.Text = "--"
				Else
					oListItem.Text = oDataSet.Tables(0).Rows(i).Item("CMNT_nome")
				End If
				oListItem.Value = oDataSet.Tables(0).Rows(i).Item("ORGN_id") 'oDataSet.Tables(0).Rows(i).Item("CMNT_id") & "," & 
				Me.DDLorganizzazione.Items.Add(oListItem)
				'metto nel value della ddl l'id della comunita e l'id dell'organizzazione facente parte
				'nella ddlorgn invece metto solo gli id delle organizzazioni come values
			Next
			'-1 significa TUTTE !!!!!!!!!!!!!!
			DDLorganizzazione.Items.Insert(0, New ListItem("-- Tutte --", -1))
			If DDLorganizzazione.Items.Count > 1 Then
				Me.DDLorganizzazione.SelectedIndex = 1
			End If
		Catch ex As Exception
			'-1 significa TUTTE !!!!!!!!!!!!!!
			DDLorganizzazione.Items.Insert(0, New ListItem("-- Tutte --", -1))
		End Try

	End Sub

	Private Sub Bind_TipoPersona()
		Dim oDataset As DataSet
		Dim oTipoPersona As New COL_TipoPersona
		Dim oListItem As New ListItem

		Try
			oDataset = oTipoPersona.Elenca(Session("LinguaID"), Main.FiltroElencoTipiPersona.WithUserAssociated_NoGuest)
			DDLtipoPersona.Items.Clear()
			If oDataset.Tables(0).Rows.Count > 0 Then
				DDLtipoPersona.DataSource = oDataset
				DDLtipoPersona.DataTextField() = "TPPR_descrizione"
				DDLtipoPersona.DataValueField() = "TPPR_id"
				DDLtipoPersona.DataBind()

				DDLtipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
				Me.DDLtipoPersona.SelectedValue = CType(Main.TipoPersonaStandard.Docente, Main.TipoPersonaStandard)

			Else
				DDLtipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
			End If
		Catch ex As Exception
			DDLtipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
		End Try

	End Sub
	Private Sub Bind_TipoComunita()
		Dim oDataSet As New DataSet

		Try
			oDataSet = COL_Tipo_Comunita.ElencaForFiltri(Session("LinguaID"), True)
			If oDataSet.Tables(0).Rows.Count > 0 Then
				DDLtipo.DataSource = oDataSet
				DDLtipo.DataTextField() = "TPCM_descrizione"
				DDLtipo.DataValueField() = "TPCM_id"
				DDLtipo.DataBind()

				'aggiungo manualmente elemento che indica tutti i tipi di comunità
				DDLtipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
			End If
		Catch ex As Exception
			DDLtipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
		End Try

	End Sub
	Public Sub Bind_TipoRuolo()

		Me.DDLtipoRuolo.Items.Clear()
		Try
			Dim oDataset As DataSet
			Dim i, Totale As Integer
			Dim oComunita As New COL_Comunita
			oDataset = oComunita.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForAll_NoGuest)

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
					Me.DDLtipoRuolo.Items.Add(oListItem)
				Next
				DDLtipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", -1))

			Else
				Me.DDLtipoRuolo.Items.Add(New ListItem("< nessun ruolo >", -1))

			End If
		Catch ex As Exception
			Me.DDLtipoRuolo.Items.Add(New ListItem("< nessun ruolo >", -1))

		End Try
	End Sub

    Public Sub BindGriglia(Optional ByVal ricalcola As Boolean = False)
        Dim dstable As New DataSet
        dstable = FiltraggioDati(ricalcola)

        Try

            dstable.Tables(0).Columns.Add(New DataColumn("oPRSN_datanascita"))

            Dim i, TotaleRecord As Integer
            TotaleRecord = dstable.Tables(0).Rows.Count


            For i = 0 To TotaleRecord - 1
                Dim oRow As DataRow
                oRow = dstable.Tables(0).Rows(i)
                Try

                    oRow.Item("oPRSN_datanascita") = FormatDateTime(oRow.Item("PRSN_datanascita"), DateFormat.ShortDate)

                Catch ex As Exception

                End Try
            Next

            If TotaleRecord > 0 Then
                '        Me.PNLnorecord.Visible = False
                Me.DGpersona.VirtualItemCount = dstable.Tables(0).Rows(0).Item("Totale")
                Mod_Visualizzazione(TotaleRecord - 1)

                Dim oDataview As DataView
                oDataview = dstable.Tables(0).DefaultView
                If viewstate("SortExspression") = "" Then
                    viewstate("SortExspression") = "PRSN_Anagrafica"
                    viewstate("SortDirection") = "asc"
                End If

                Me.PNLpersona.Visible = True
                PNLnorecordPrsn.Visible = False
                'Me.DGpersona.Visible = True
                Me.DGpersona.DataSource = dstable
                Me.DGpersona.DataBind()
            Else
                Me.PNLpersona.Visible = False
                PNLpaginazione.Visible = False
                PNLnorecordPrsn.Visible = True
                LBnorecordPrsn.Text = "Spiacente, al momento non ci utenti presenti in base ai parametri di ricerca specificati."
            End If
        Catch ex As Exception
            Me.PNLpersona.Visible = False
            PNLpaginazione.Visible = False
            PNLnorecordPrsn.Visible = True
            LBnorecordPrsn.Text = "Spiacente, al momento non ci utenti presenti in base ai parametri di ricerca specificati."
        End Try

    End Sub
#End Region

#Region "Setup Parametri Ricerca"
    Private Sub SaveSearchParameters()
        Try
            'Salvare dati relativi alle comunità
            'Me.Response.Cookies("RicercaComunita")("annoAccademico") = Me.DDLannoAccademico.SelectedValue
            'Me.Response.Cookies("RicercaComunita")("periodo") = Me.DDLperiodo.SelectedValue

            Me.Response.Cookies("RicercaComunita")("numeroRecord") = Me.DDLNumeroRecord.SelectedValue

            Me.Response.Cookies("RicercaComunita")("tipo") = Me.DDLtipo.SelectedValue
            Me.Response.Cookies("RicercaComunita")("tiporicerca") = Me.DDLTipoRicerca.SelectedValue
            Me.Response.Cookies("RicercaComunita")("valore") = Me.TXBValore.Text
            Me.Response.Cookies("RicercaComunita")("intCurPage2") = Me.ViewState("intCurPage2")

            Me.Response.Cookies("RicercaComunita")("SortDirection2") = Me.ViewState("SortDirection2")
            Me.Response.Cookies("RicercaComunita")("SortExspression2") = Me.ViewState("SortExspression2")

            Me.Response.Cookies("RicercaComunita")("TPRL_ID") = Me.DDLtipoRuolo.SelectedValue
            Me.Response.Cookies("RicercaComunita")("PRSN_ID") = Me.HDprsn_id.Value
            Me.Response.Cookies("RicercaComunita")("RBLvisualizza") = Me.RBLvisualizza.SelectedValue


            'Salvare dati relativi all'elenco persone
            Me.Response.Cookies("RicercaComunita")("intCurPage") = Me.ViewState("intCurPage")
            Me.Response.Cookies("RicercaComunita")("intAnagrafica") = Me.ViewState("intAnagrafica")
            Me.Response.Cookies("RicercaComunita")("SortDirection") = Me.ViewState("SortDirection")
            Me.Response.Cookies("RicercaComunita")("SortExspression") = Me.ViewState("SortExspression")

            Me.Response.Cookies("RicercaComunita")("DDLricercaPersonaBy") = Me.DDLricercaPersonaBy.SelectedValue
            Me.Response.Cookies("RicercaComunita")("organizzazione") = Me.DDLorganizzazione.SelectedValue
            Me.Response.Cookies("RicercaComunita")("TPPR_ID") = Me.DDLtipoPersona.SelectedValue
            Me.Response.Cookies("RicercaComunita")("TXBvaloreSearch") = Me.TXBvaloreSearch.Text

        Catch ex As Exception

        End Try
    End Sub
    Private Sub SetupSearchParameters()
        Try
            'Setup filtri Comunità
            Try
                Me.HDprsn_id.Value = Me.Request.Cookies("RicercaComunita")("PRSN_ID")
                Dim oPersona As New COL_Persona
                oPersona.Id = Me.HDprsn_id.Value
                oPersona.Estrai(Session("LinguaID"))
                'Me.LBTitolo.Text = "Lista comunità di '" & oPersona.Nome & " " & oPersona.Cognome & "'"
                Me.Master.ServiceTitle = "Lista comunità di '" & oPersona.Nome & " " & oPersona.Cognome & "'"
            Catch ex As Exception

            End Try
            Try
                Me.DDLtipoRuolo.SelectedValue = Me.Request.Cookies("RicercaComunita")("TPRL_ID")
            Catch ex As Exception
                Me.DDLtipoRuolo.SelectedIndex = 0
            End Try
            Try
                Me.RBLvisualizza.SelectedValue = Me.Request.Cookies("RicercaComunita")("RBLvisualizza")
            Catch ex As Exception
                Me.RBLvisualizza.SelectedIndex = 0
            End Try

            'Recupero fattori di ricerca relativi all'ordinamento
            Try
                'Ordinamento griglie
                Me.ViewState("SortDirection") = Me.Request.Cookies("RicercaComunita")("SortDirection")
                Me.ViewState("SortExspression") = Me.Request.Cookies("RicercaComunita")("SortExspression")

                Me.ViewState("SortDirection2") = Me.Request.Cookies("RicercaComunita")("SortDirection2")
                Me.ViewState("SortExspression2") = Me.Request.Cookies("RicercaComunita")("SortExspression2")
            Catch ex As Exception

            End Try

            'Griglia comunità
            Try
                If IsNumeric(Me.Request.Cookies("RicercaComunita")("intCurPage2")) Then
                    Me.ViewState("intCurPage2") = CInt(Me.Request.Cookies("RicercaComunita")("intCurPage2"))
                    Me.DGComunita.CurrentPageIndex = CInt(Me.ViewState("intCurPage2"))
                Else
                    Me.ViewState("intCurPage2") = 0
                    Me.DGComunita.CurrentPageIndex = 0
                End If
            Catch ex As Exception
                Me.ViewState("intCurPage2") = 0
                Me.DGComunita.CurrentPageIndex = 0
            End Try

            'Griglia persone
            Try
                If IsNumeric(Me.Request.Cookies("RicercaComunita")("intCurPage")) Then
                    Me.ViewState("intCurPage") = CInt(Me.Request.Cookies("RicercaComunita")("intCurPage"))
                    Me.DGpersona.CurrentPageIndex = CInt(Me.ViewState("intCurPage"))
                Else
                    Me.ViewState("intCurPage") = 0
                    Me.DGpersona.CurrentPageIndex = 0
                End If
            Catch ex As Exception
                Me.ViewState("intCurPage") = 0
                Me.DGpersona.CurrentPageIndex = 0
            End Try

            Try
                Me.TXBValore.Text = Me.Request.Cookies("RicercaComunita")("valore")
            Catch ex As Exception
                Me.TXBValore.Text = ""
            End Try

            'Vedo se selezionare qualche linkbutton !!
            Try
                If IsNumeric(Me.Request.Cookies("RicercaComunita")("intAnagrafica")) Then
                    Dim Lettera As String
                    Lettera = CType(CInt(Me.Request.Cookies("RicercaComunita")("intAnagrafica")), Main.FiltroComunita).ToString

                    Dim oLink As System.Web.UI.WebControls.LinkButton
                    oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
                    If IsNothing(oLink) = False Then
                        oLink.CssClass = "lettera_Selezionata"
                        Me.ViewState("intAnagrafica") = CInt(Me.Request.Cookies("RicercaComunita")("intAnagrafica"))
                    Else
                        Me.ViewState("intAnagrafica") = -1
                        Me.LKBtutti.CssClass = "lettera_Selezionata"
                    End If
                Else
                    Me.ViewState("intAnagrafica") = -1
                    Me.LKBtutti.CssClass = "lettera_Selezionata"
                End If

            Catch ex As Exception
                Me.ViewState("intAnagrafica") = -1
                Me.LKBtutti.CssClass = "lettera_Selezionata"
            End Try

            ' Setto l'anno accademico
            'Try
            '    If IsNumeric(Me.Request.Cookies("ListaComunita")("annoAccademico")) Then
            '        Try
            '            Me.DDLannoAccademico.SelectedValue = Me.Request.Cookies("ListaComunita")("annoAccademico")
            '        Catch ex As Exception

            '        End Try
            '    End If
            'Catch ex As Exception

            'End Try
            '' Setto il periodo
            'Try
            '    If IsNumeric(Me.Request.Cookies("ListaComunita")("periodo")) Then
            '        Try
            '            Me.DDLperiodo.SelectedValue = Me.Request.Cookies("ListaComunita")("periodo")
            '        Catch ex As Exception

            '        End Try
            '    End If
            'Catch ex As Exception

            'End Try
            ' Setto l'organizzazione
            Try
                If IsNumeric(Me.Request.Cookies("RicercaComunita")("organizzazione")) Then
                    Try
                        Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("RicercaComunita")("organizzazione")
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try

            ' Setto il numero di record
            Try
                If IsNumeric(Me.Request.Cookies("ListaComunita")("numeroRecord")) Then
                    Me.DDLNumeroRecord.SelectedValue = Me.Request.Cookies("RicercaComunita")("numeroRecord")
                End If
            Catch ex As Exception

            End Try
            ' Setto il numero di record
            Try
                If IsNumeric(Me.Request.Cookies("RicercaComunita")("tipo")) Then
                    Me.DDLtipo.SelectedValue = Me.Request.Cookies("RicercaComunita")("tipo")
                    'If Me.DDLtipo.SelectedValue = 1 Then
                    '    Me.TBRcorsi.Visible = True
                    'Else
                    '    Me.TBRcorsi.Visible = False
                    'End If
                Else
                    'Me.TBRcorsi.Visible = False
                End If
            Catch ex As Exception
                'Me.TBRcorsi.Visible = False
            End Try

            ' Setto il tipo di ricerca
            Try
                If IsNumeric(Me.Request.Cookies("RicercaComunita")("tiporicerca")) Then
                    Me.DDLTipoRicerca.SelectedValue = Me.Request.Cookies("RicercaComunita")("tiporicerca")
                End If
            Catch ex As Exception
            End Try

            'Dati Filtro Persona
            Try
                If IsNumeric(Me.Request.Cookies("RicercaComunita")("DDLricercaPersonaBy")) Then
                    Me.DDLricercaPersonaBy.SelectedValue = Me.Request.Cookies("RicercaComunita")("DDLricercaPersonaBy")
                End If
            Catch ex As Exception

            End Try
            Try
                Me.TXBvaloreSearch.Text = Me.Request.Cookies("RicercaComunita")("TXBvaloreSearch")
            Catch ex As Exception

            End Try
            Try
                Me.DDLtipoPersona.SelectedValue = Me.Request.Cookies("RicercaComunita")("TPPR_ID")
            Catch ex As Exception
                Me.DDLtipoPersona.SelectedIndex = 0
            End Try
        Catch ex As Exception

        End Try

    End Sub
#End Region

#Region "Gestione Filtro persone"

    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcerca.Click
        BindGriglia(True)
    End Sub
    Private Sub DDLtipoPersona_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLtipoPersona.SelectedIndexChanged
        BindGriglia(True)
    End Sub
    Public Sub FiltroLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
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
        Me.BindGriglia(True)
    End Sub

    Private Function FiltraggioDati(Optional ByVal ricalcola As Boolean = False) As DataSet
        Dim oDataset As New DataSet

        Try
            Dim oPersona As New COL_Persona
            Dim ISTT_ID As Integer
            Dim PRSN_TPPR_id As Integer
            Dim oFiltroOrdinamento As New Main.FiltroOrdinamento
            Dim oFiltroAnagrafica As New Main.FiltroAnagrafica
            Dim oCampoOrdinePersona As New Main.FiltroCampoOrdinePersona

            oPersona = Session("objPersona")
            ISTT_ID = oPersona.Istituzione.Id
            PRSN_TPPR_id = Me.DDLtipoPersona.SelectedValue

            If PRSN_TPPR_id > 0 Then
                Me.DGpersona.Columns(2).Visible = False
            Else
                Me.DGpersona.Columns(2).Visible = True
            End If



            If viewstate("SortExspression") = "" Or LCase(viewstate("SortExspression")) = "prsn_anagrafica" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.anagrafica
            ElseIf LCase(viewstate("SortExspression")) = "prsn_datanascita" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.dataNascita

                'ElseIf LCase(viewstate("SortExspression")) = "prsn_mail" Then
                '    oPersona.FiltroCampoOrdine = oPersona.FltrOrdine.mail

            ElseIf LCase(viewstate("SortExspression")) = "prsn_login" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.login

            ElseIf LCase(viewstate("SortExspression")) = "tppr_descrizione" Then
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.tipoPersona
            Else
                oCampoOrdinePersona = Main.FiltroCampoOrdinePersona.anagrafica
            End If

            If viewstate("SortDirection") = "" Or viewstate("SortDirection") = "asc" Then
                oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente
            Else
                oFiltroOrdinamento = Main.FiltroOrdinamento.Decrescente
            End If

            'definisco il filtraggio per lettera !
            Try
                If IsNothing(Me.ViewState("intAnagrafica")) Then
                    oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Else
                    oFiltroAnagrafica = CType(Me.ViewState("intAnagrafica"), Main.FiltroAnagrafica)
                End If
            Catch ex As Exception
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Me.LKBtutti.CssClass = "lettera_Selezionata"
            End Try
            Dim valore As String
            Dim oFiltroPersona As Main.FiltroPersona

            oFiltroPersona = Main.FiltroPersona.tutte

            Select Case Me.DDLricercaPersonaBy.SelectedValue
                Case Main.FiltroPersona.tutte
                    valore = ""
                Case Main.FiltroPersona.nome
                    If Me.TXBvaloreSearch.Text <> "" Then
                        valore = Me.TXBvaloreSearch.Text
                        oFiltroPersona = Main.FiltroPersona.nome
                    End If
                Case Main.FiltroPersona.cognome
                    If Me.TXBvaloreSearch.Text <> "" Then
                        valore = Me.TXBvaloreSearch.Text
                        oFiltroPersona = Main.FiltroPersona.cognome
                    End If
                Case Main.FiltroPersona.codiceFiscale
                    If Me.TXBvaloreSearch.Text <> "" Then
                        valore = Me.TXBvaloreSearch.Text
                        If Len(valore) > 16 Then
                            valore = Left(valore, 16)
                            Me.TXBvaloreSearch.Text = valore
                        End If
                        oFiltroPersona = Main.FiltroPersona.codiceFiscale
                    End If
                Case Main.FiltroPersona.mail
                    If Me.TXBvaloreSearch.Text <> "" Then
                        valore = Me.TXBvaloreSearch.Text
                        If Len(valore) > 255 Then
                            valore = Left(valore, 255)
                            Me.TXBvaloreSearch.Text = valore
                        End If
                        oFiltroPersona = Main.FiltroPersona.mail
                    End If
                Case Main.FiltroPersona.matricola
                    If Me.TXBvaloreSearch.Text <> "" Then
                        valore = Me.TXBvaloreSearch.Text
                        oFiltroPersona = Main.FiltroPersona.matricola
                    End If
                Case Main.FiltroPersona.dataNascita
                    If Me.TXBvaloreSearch.Text <> "" Then
                        Try
                            If IsDate(Me.TXBvaloreSearch.Text) Then
                                valore = Me.TXBvaloreSearch.Text
                                valore = Main.DateToString(valore, False)
                                oFiltroPersona = Main.FiltroPersona.dataNascita
                            End If
                        Catch ex As Exception

                        End Try
                    End If
                Case Main.FiltroPersona.login
                    If Me.TXBValore.Text <> "" Then
                        valore = Me.TXBValore.Text
                        oFiltroPersona = Main.FiltroPersona.login
                    End If
                Case Else
                    valore = ""
                    oFiltroPersona = Main.FiltroPersona.tutte
            End Select

            If oFiltroPersona <> Main.FiltroPersona.tutte Then
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                Me.LKBtutti.CssClass = "lettera_Selezionata"
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Me.ViewState("intAnagrafica") = CType(Main.FiltroAnagrafica.tutti, Main.FiltroAnagrafica)
            End If
            If ISTT_ID > 0 Then
                If ricalcola Then

                    Me.ViewState("intCurPage") = 0
                    Me.DGpersona.CurrentPageIndex = 0
                End If
                Return oPersona.GetPersonePaginateByIstituzione(ISTT_ID, Me.DGpersona.PageSize, Me.ViewState("intCurPage"), PRSN_TPPR_id, Me.DDLorganizzazione.SelectedValue, oFiltroAnagrafica, oFiltroOrdinamento, oCampoOrdinePersona, oFiltroPersona, valore, , Main.TipoAttivazione.Attivati)
            Else
                Me.ViewState("intCurPage") = 0
                Me.DGpersona.VirtualItemCount = 0
                Me.DGpersona.CurrentPageIndex = 0
                Return oDataset
            End If
        Catch ex As Exception
            Return oDataset
        End Try
    End Function

    Private Sub DeselezionaLink(ByVal Lettera As String)
        Lettera = CType(CInt(Lettera), Main.FiltroAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub
#End Region


#Region "Gestione Griglia Persone"

    Private Sub Mod_Visualizzazione(ByVal oRecord As Integer)
        Me.PNLpaginazione.Visible = False
        If oRecord > Me.DGpersona.PageSize Or oRecord > Me.DDLpaginazione.Items(0).Value Or Me.DGpersona.VirtualItemCount > Me.DGpersona.PageSize Then
            Me.DGpersona.AllowPaging = True
            Me.DGpersona.PageSize = Me.DDLpaginazione.SelectedItem.Value
            PNLpaginazione.Visible = True
        Else
            Me.DGpersona.AllowPaging = False
            PNLpaginazione.Visible = False
        End If
        If oRecord < 0 Then
            Me.PNLpersona.Visible = False
            PNLpaginazione.Visible = False
            PNLnorecordPrsn.Visible = True
            LBnorecordPrsn.Text = "Spiacente, al momento non ci utenti presenti in base ai parametri di ricerca specificati."
        End If
    End Sub

    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGpersona.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = viewstate("SortExspression")
        oSortDirection = viewstate("SortDirection")
        viewstate("SortExspression") = e.SortExpression

        If LCase(e.SortExpression) = LCase(oSortExpression) Then
            If viewstate("SortDirection") = "asc" Then
                viewstate("SortDirection") = "desc"
            Else
                viewstate("SortDirection") = "asc"
            End If
        End If
        BindGriglia()
    End Sub

    Private Sub CambioPagina(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGpersona.PageIndexChanged
        source.CurrentPageIndex = e.NewPageIndex
        Me.ViewState("intCurPage") = e.NewPageIndex
        BindGriglia()
    End Sub

    Private Sub Cambio_NumPagine(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLpaginazione.SelectedIndexChanged
        DGpersona.PageSize = DDLpaginazione.Items(DDLpaginazione.SelectedIndex).Value
        DGpersona.CurrentPageIndex = 0
        Me.ViewState("intCurPage") = 0
        BindGriglia()
    End Sub

    Private Sub DGgriglia_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGpersona.ItemCreated
        Dim i As Integer

        If e.Item.ItemType = ListItemType.Header Then
            Dim oSortExspression, oSortDirection, oText As String
            oSortExspression = viewstate("SortExspression")
            oSortDirection = viewstate("SortDirection")
            If oSortDirection = "asc" Then
                oText = "5"
            Else
                oText = "6"
            End If

            For i = 0 To sender.columns.count - 1
                If sender.columns(i).visible = True And sender.columns(i).SortExpression <> "" Then
                    Dim oWebControl As WebControl
                    Dim oCell As New TableCell
                    Dim oLabelAfter As New System.Web.UI.WebControls.Label
                    Dim oLabelBefore As New System.Web.UI.WebControls.Label

                    oLabelAfter.font.name = "webdings"
                    oLabelAfter.font.size = FontUnit.XSmall
                    oLabelAfter.text = "&nbsp;"

                    oLabelBefore.font.name = "webdings"
                    oLabelBefore.font.size = FontUnit.XSmall
                    oLabelBefore.text = "&nbsp;"

                    oCell = e.Item.Cells(i)
                    If oSortExspression = sender.columns(i).SortExpression Then
                        Try
                            oWebControl = oCell.Controls(0)
                            Dim oLinkbutton As LinkButton
                            oLinkbutton = oWebControl
                            oLinkbutton.CssClass = "HeaderLink"

                            oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                            If oSortDirection = "asc" Then
                                oLinkbutton.Attributes.Add("onfocus", "window.status='Ordinamento decrescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordinamento decrescentep per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onclick", "window.status='Ordinamento decrescente per <" & oLinkbutton.Text & ">';return true;")
                            Else
                                oLinkbutton.Attributes.Add("onfocus", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onclick", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                            End If

                            oLabelAfter.font.name = oLinkbutton.Font.Name
                            oLabelAfter.font.size = oLinkbutton.Font.Size
                            oLabelAfter.text = oLinkbutton.Text & " "
                            oLinkbutton.Font.Name = "webdings"
                            oLinkbutton.Font.Size = FontUnit.XSmall
                            oLinkbutton.Text = oText
                            oCell.Controls.AddAt(0, oLabelBefore)
                            oCell.Controls.AddAt(1, oLabelAfter)
                        Catch ex As Exception
                            oCell.Controls.AddAt(0, oLabelBefore)
                            oCell.Controls.AddAt(1, oLabelAfter)
                        End Try
                    Else
                        Try
                            oWebControl = oCell.Controls(0)
                            Dim oLinkbutton As LinkButton
                            oLinkbutton = oWebControl
                            oLinkbutton.CssClass = "HeaderLink"

                            oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")

                            oLinkbutton.Attributes.Add("onfocus", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                            oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                            oLinkbutton.Attributes.Add("onclick", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                        Catch ex As Exception

                        End Try
                        oCell.Controls.AddAt(0, oLabelBefore)
                        oCell.Controls.Add(oLabelAfter)
                    End If
                End If
            Next
        End If
        If e.Item.ItemType = ListItemType.Pager Then
            Dim oCell As TableCell
            Dim n As Integer
            oCell = CType(e.Item.Controls(0), TableCell)


            For n = 0 To oCell.Controls.Count - 1 Step 2
                Dim szLnk As String
                szLnk = "System.Web.UI.WebControls.DataGridLinkButton"
                Dim oWebControl As WebControl

                oWebControl = oCell.Controls(n)

                If (oWebControl.GetType().ToString() = szLnk) Then
                    oWebControl.CssClass = "PagerLink"
                End If
                Try
                    Dim oLabel As Label
                    oLabel = oWebControl
                    oLabel.Text = oLabel.Text
                    oLabel.CssClass = "PagerSpan"
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "PagerLink"
                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                End Try
            Next
        End If
        If (e.Item.ItemType = ListItemType.Footer) Then
            'riferimento alla cella corrente - aggiungo il testo 
            ' occupo tutto le colonne 
            e.Item.Cells(0).ColumnSpan = e.Item.Cells.Count

            'rimuovo le altre colonne 
            For i = 1 To e.Item.Cells.Count - 1
                e.Item.Cells.RemoveAt(1)
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oLinkbutton3 As LinkButton
            Dim Cell As New TableCell

            Try

                Cell = CType(e.Item.Cells(0), TableCell)

                oLinkbutton3 = Cell.FindControl("PRSN_Anagrafica")

                oLinkbutton3.Attributes.Add("onfocus", "window.status='Clicca per ottenere le sue comunità';return true;")
                oLinkbutton3.Attributes.Add("onmouseover", "window.status='Clicca per ottenere le sue comunità';return true;")
                oLinkbutton3.Attributes.Add("onclick", "window.status='Clicca per ottenere le sue comunità';return true;")
                oLinkbutton3.Attributes.Add("onmouseout", "window.status='';return true;")

                oLinkbutton3.ToolTip = "Clicca per ottenere le sue comunità"

            Catch ex As Exception

            End Try

        End If
    End Sub

    Private Sub DGgriglia_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGpersona.ItemCommand
        If e.CommandName = "VisualizzaComunita" Then
            Me.PNLsceltaPersona.Visible = False
            Me.PNLsceltaComunita.Visible = True
            Dim PRSN_id As Integer
            PRSN_id = DGpersona.DataKeys.Item(e.Item.ItemIndex)
            Bind_GrigliaComunita(PRSN_id)
            HDprsn_id.Value = ""
            HDprsn_id.Value = PRSN_id
            'titoletto
            Dim oPersona As New COL_Persona
            oPersona.Id = PRSN_id
            oPersona.Estrai(Session("LinguaID"))
            'Me.LBTitolo.Text = "Lista comunità di '" & oPersona.Nome & " " & oPersona.Cognome & "'"
            Me.Master.ServiceTitle = "Lista comunità di '" & oPersona.Nome & " " & oPersona.Cognome & "'"
        End If
    End Sub

#End Region


#Region "button"

    'Private Sub BTNok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNok.Click
    '    Me.PNLnorecord.Visible = False ''''''''''''''''da verificare
    '    Me.PNLpersona.Visible = True
    '    Me.PNLfiltri.Visible = True
    'End Sub

#End Region



#Region "Griglia Comunità"
    Private Sub Bind_GrigliaComunita(ByVal PRSN_id As Integer)
        Dim oPersona As New COL_Persona
        oPersona.Id = PRSN_id
        Dim dsTable As DataSet
        Dim totale As Integer

        Try
            Dim oTreeComunita As New COL_TreeComunita

            dsTable = Me.FiltraggioDatiComunita(PRSN_id)

            totale = dsTable.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                ' al posto della datagrid mostro un messaggio!
                Me.DGComunita.Visible = False
                Me.PNLnorecordCmnt.Visible = True
                Me.LBnorecordCmnt.Visible = True
                Me.LBnorecordCmnt.Text = "Nessun elemento per questa selezione"

            Else
                ' totale = dsTable.Tables(0).Rows.Count
                If totale > 0 Then


                    Dim oDataview2 As New DataView
                    oDataview2 = dsTable.Tables(0).DefaultView
                    If ViewState("SortExspression2") = "" Then
                        ViewState("SortExspression2") = "CMNT_Nome"
                        ViewState("SortDirection2") = "desc"
                    End If
                    oDataview2.Sort = ViewState("SortExspression2") & " " & ViewState("SortDirection2")

                    ' totale = dsTable.Tables(0).Rows.Count
                    totale = oDataview2.Count
                    Me.PNLnorecordCmnt.Visible = False
                    Me.DGComunita.Visible = True
                    DGComunita.DataSource = oDataview2
                    DGComunita.DataBind()
                Else
                    Me.PNLnorecordCmnt.Visible = True
                    Me.DGComunita.Visible = False
                    Me.LBnorecordCmnt.Visible = True
                    Me.LBnorecordCmnt.Text = "Nessun elemento per questa selezione"
                End If
            End If
        Catch ex As Exception
            Me.PNLnorecordCmnt.Visible = True
            Me.DGComunita.Visible = False
            Me.LBnorecordCmnt.Visible = True
            Me.LBnorecordCmnt.Text = "Nessun elemento per questa selezione"
        End Try
    End Sub

    Private Function FiltraggioDatiComunita(ByVal PRSN_ID As Integer) As DataSet
        Dim oDataset As New DataSet

        Try
            Dim oComunita As New COL_Comunita
            Dim oPersona As New COL_Persona
            Dim ISTT_ID As Integer
            Dim PRSN_TPPR_id As Integer
            oPersona.Id = PRSN_ID
            ISTT_ID = oPersona.Istituzione.Id
            PRSN_TPPR_id = Me.DDLtipoPersona.SelectedValue

            'If PRSN_TPPR_id > 0 Then
            '    Me.DGpersona.Columns(2).Visible = False
            'Else
            '    Me.DGpersona.Columns(2).Visible = True
            'End If
            '  Dim oComunita As New COL_Comunita
            Dim TPCM_ID As Integer
            If DDLtipo.SelectedItem.Value = -1 Then
                TPCM_ID = 0
            Else
                TPCM_ID = DDLtipo.SelectedItem.Value
            End If

            Dim valore As String = ""
            Dim TPRL_ID, Totale As Integer
            Dim oFiltro As Main.FiltroComunita
            Dim oFiltroUtenti As Main.FiltroUtenti
            Dim oFiltroCampoOrdine As New Main.FiltroCampoOrdineComunita
            Dim oFiltroOrdinamento As New Main.FiltroOrdinamento

            Select Case Me.DDLTipoRicerca.SelectedValue
                Case -2
                    oFiltro = Main.FiltroComunita.nome
                    valore = Me.TXBValore.Text
                Case -3
                    If Me.TXBValore.Text = "" Or IsDate(Me.TXBValore.Text) = False Then
                        oFiltro = Main.FiltroComunita.tutti
                        valore = ""
                    Else
                        oFiltro = Main.FiltroComunita.creataDopo
                    End If
                    valore = Me.TXBValore.Text
                Case -4
                    If Me.TXBValore.Text = "" Or IsDate(Me.TXBValore.Text) = False Then
                        oFiltro = Main.FiltroComunita.tutti
                        valore = ""
                    Else
                        oFiltro = Main.FiltroComunita.creataPrima
                    End If
                    valore = Me.TXBValore.Text
                Case -5
                    If Me.TXBValore.Text = "" Or IsDate(Me.TXBValore.Text) = False Then
                        oFiltro = Main.FiltroComunita.tutti
                        valore = ""
                    Else
                        oFiltro = Main.FiltroComunita.dataIscrizioneDopo
                    End If
                    valore = Me.TXBValore.Text
                Case -6
                    If Me.TXBValore.Text = "" Or IsDate(Me.TXBValore.Text) = False Then
                        oFiltro = Main.FiltroComunita.tutti
                        valore = ""
                    Else
                        oFiltro = Main.FiltroComunita.dataFineIscrizionePrima

                        valore = Me.TXBValore.Text
                    End If
                Case Else
                    oFiltro = Main.FiltroComunita.tutti
            End Select

            Select Case Me.RBLvisualizza.SelectedValue
                Case 1
                    oFiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori
                Case 2
                    oFiltroUtenti = Main.FiltroUtenti.Responsabili
                Case 3
                    oFiltroUtenti = Main.FiltroUtenti.Passanti
                Case Else
                    oFiltroUtenti = Main.FiltroUtenti.Tutti
            End Select
            If viewstate("SortDirection2") = "" Or viewstate("SortDirection2") = "asc" Then
                oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente
            Else
                oFiltroOrdinamento = Main.FiltroOrdinamento.Decrescente
            End If

            Dim SortExspression2 As String = ""
            Try
                SortExspression2 = ViewState("SortExspression2")
            Catch ex As Exception

            End Try
            Select Case SortExspression2
                Case "TPCM_Descrizione"
                    oFiltroCampoOrdine = Main.FiltroCampoOrdineComunita.TipoComunita
                Case "CMNT_Nome"
                    oFiltroCampoOrdine = Main.FiltroCampoOrdineComunita.Nome
                Case "CMNT_Anno"
                    oFiltroCampoOrdine = Main.FiltroCampoOrdineComunita.AnnoAccademico
                Case "CMNT_PRDO_descrizione"
                    oFiltroCampoOrdine = Main.FiltroCampoOrdineComunita.Periodo
                Case "CMNT_Responsabile"
                    oFiltroCampoOrdine = Main.FiltroCampoOrdineComunita.Responsabile
                Case "CMNT_Livello"
                    oFiltroCampoOrdine = Main.FiltroCampoOrdineComunita.Livello
                Case "AnagraficaCreatore"
                    oFiltroCampoOrdine = Main.FiltroCampoOrdineComunita.Creatore
                Case Else
                    oFiltroCampoOrdine = Main.FiltroCampoOrdineComunita.Nome
            End Select

            TPRL_ID = Me.DDLtipoRuolo.SelectedItem.Value
            oDataset = COL_Comunita.ElencaGlobale_PaginateByPersona(Session("LinguaID"), Me.ViewState("intCurPage2"), Me.DGComunita.PageSize, Me.DDLorganizzazione.SelectedValue, PRSN_ID, oFiltroUtenti, TPCM_ID, oFiltro, valore, TPRL_ID, , oFiltroOrdinamento, oFiltroCampoOrdine)

            Me.ViewState("intCurPage2") = 0
            Totale = oDataset.Tables(0).Rows.Count
            If Totale > 0 Then
                Dim i As Integer
                Me.DGComunita.CurrentPageIndex = Me.ViewState("intCurPage2")
                Me.DGComunita.VirtualItemCount = oDataset.Tables(0).Rows(0).Item("CMNT_Totale")

                Dim ImageBaseDir, img As String
                ImageBaseDir = GetPercorsoApplicazione(Me.Request)
                ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"
                For i = 0 To Totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)
                    If IsDBNull(oRow.Item("TPCM_icona")) Then
                        oRow.Item("TPCM_icona") = ""
                    Else
                        img = oRow.Item("TPCM_icona")
                        img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                        oRow.Item("TPCM_icona") = img
                    End If

                    If IsDate(oRow.Item("CMNT_dataCreazione")) Then
                        If Not Equals(New Date, oRow.Item("CMNT_dataCreazione")) Then
                            oRow.Item("CMNT_dataCreazione") = FormatDateTime(oRow.Item("CMNT_dataCreazione"), DateFormat.GeneralDate)
                        End If
                    End If
                Next
            Else
                If Me.ViewState("intCurPage2") > 0 Then
                    Me.ViewState("intCurPage2") = Me.ViewState("intCurPage2") - 1
                    Me.DGComunita.CurrentPageIndex = Me.ViewState("intCurPage2")
                    Return Me.FiltraggioDati
                Else
                    Me.DGComunita.VirtualItemCount = 0
                End If
            End If
            Return oDataset

        Catch ex As Exception
            Return oDataset
        End Try
    End Function

    Private Sub DGComunita_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGComunita.ItemCreated
        Dim i As Integer

        If e.Item.ItemType = ListItemType.Header Then
            Dim oSortExspression, oSortDirection, oText As String
            oSortExspression = viewstate("SortExspression2")
            oSortDirection = viewstate("SortDirection2")
            If oSortDirection = "asc" Then
                oText = "5"
            Else
                oText = "6"
            End If

            For i = 0 To sender.columns.count - 1
                If sender.columns(i).visible = True And sender.columns(i).SortExpression <> "" Then
                    Dim oWebControl As WebControl
                    Dim oCell As New TableCell
                    Dim oLabelAfter As New System.Web.UI.WebControls.Label
                    Dim oLabelBefore As New System.Web.UI.WebControls.Label

                    oLabelAfter.font.name = "webdings"
                    oLabelAfter.font.size = FontUnit.XSmall
                    oLabelAfter.text = "&nbsp;"

                    oLabelBefore.font.name = "webdings"
                    oLabelBefore.font.size = FontUnit.XSmall
                    oLabelBefore.text = "&nbsp;"

                    oCell = e.Item.Cells(i)
                    If oSortExspression = sender.columns(i).SortExpression Then
                        Try
                            oWebControl = oCell.Controls(0)
                            Dim oLinkbutton As LinkButton
                            oLinkbutton = oWebControl
                            oLinkbutton.CssClass = "HeaderLink"

                            oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                            If oSortDirection = "asc" Then
                                oLinkbutton.Attributes.Add("onfocus", "window.status='Ordinamento decrescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordinamento decrescentep per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onclick", "window.status='Ordinamento decrescente per <" & oLinkbutton.Text & ">';return true;")
                            Else
                                oLinkbutton.Attributes.Add("onfocus", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                                oLinkbutton.Attributes.Add("onclick", "window.status='Ordinamento crescente per <" & oLinkbutton.Text & ">';return true;")
                            End If

                            oLabelAfter.font.name = oLinkbutton.Font.Name
                            oLabelAfter.font.size = oLinkbutton.Font.Size
                            oLabelAfter.text = oLinkbutton.Text & " "
                            oLinkbutton.Font.Name = "webdings"
                            oLinkbutton.Font.Size = FontUnit.XSmall
                            oLinkbutton.Text = oText
                            oCell.Controls.AddAt(0, oLabelBefore)
                            oCell.Controls.AddAt(1, oLabelAfter)
                        Catch ex As Exception
                            oCell.Controls.AddAt(0, oLabelBefore)
                            oCell.Controls.AddAt(1, oLabelAfter)
                        End Try
                    Else
                        Try
                            oWebControl = oCell.Controls(0)
                            Dim oLinkbutton As LinkButton
                            oLinkbutton = oWebControl
                            oLinkbutton.CssClass = "HeaderLink"

                            oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")

                            oLinkbutton.Attributes.Add("onfocus", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                            oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                            oLinkbutton.Attributes.Add("onclick", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                        Catch ex As Exception

                        End Try
                        oCell.Controls.AddAt(0, oLabelBefore)
                        oCell.Controls.Add(oLabelAfter)
                    End If
                End If
            Next
        End If
        If e.Item.ItemType = ListItemType.Pager Then
            Dim oCell As TableCell
            Dim n As Integer
            oCell = CType(e.Item.Controls(0), TableCell)


            For n = 0 To oCell.Controls.Count - 1 Step 2
                Dim szLnk As String
                szLnk = "System.Web.UI.WebControls.DataGridLinkButton"
                Dim oWebControl As WebControl

                oWebControl = oCell.Controls(n)

                If (oWebControl.GetType().ToString() = szLnk) Then
                    oWebControl.CssClass = "PagerLink"
                End If
                Try
                    Dim oLabel As Label
                    oLabel = oWebControl
                    oLabel.Text = oLabel.Text
                    oLabel.CssClass = "PagerSpan"
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "PagerLink"
                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='Pagina " & oLinkbutton.Text & "';return true;")
                End Try
            Next
        End If
        If (e.Item.ItemType = ListItemType.Footer) Then
            'riferimento alla cella corrente - aggiungo il testo 
            ' occupo tutto le colonne 
            e.Item.Cells(0).ColumnSpan = e.Item.Cells.Count

            'rimuovo le altre colonne 
            For i = 1 To e.Item.Cells.Count - 1
                e.Item.Cells.RemoveAt(1)
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            'bottone informazioni
            Dim oCell As New TableCell
            Dim oImagebutton As ImageButton
            Try
                oCell = CType(e.Item.Cells(0), TableCell)
                oImagebutton = oCell.FindControl("IMGDettagli")
                oImagebutton.Attributes.Add("onfocus", "window.status='Info Comunità';return true;")
                oImagebutton.Attributes.Add("onmouseover", "window.status='Info Comunità';return true;")
                oImagebutton.Attributes.Add("onclick", "window.status='Info Comunità';return true;")
                oImagebutton.Attributes.Add("onmouseout", "window.status='';return true;")
                oImagebutton.ToolTip = "Info Comunità"
            Catch ex As Exception

            End Try

            Dim oImagebutton2 As ImageButton
            Try
                oCell = CType(e.Item.Cells(0), TableCell)
                Try
                    oImagebutton2 = oCell.FindControl("IMGDelete")
                    oImagebutton2.Attributes.Add("onclick", "window.status='Elimina';return confirm('Funzione in fase di sviluppo');")
                    oImagebutton2.ToolTip = "Cancella la Comunità definitivamente"
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try

            'modifica
            Dim oImagebutton3 As ImageButton
            Try
                oCell = CType(e.Item.Cells(0), TableCell)
                oImagebutton3 = oCell.FindControl("IMGEdit")
                oImagebutton3.Attributes.Add("onfocus", "window.status='Modifica Comunità';return true;")
                oImagebutton3.Attributes.Add("onmouseover", "window.status='Modifica Comunità';return true;")
                oImagebutton3.Attributes.Add("onclick", "window.status='Modifica Comunità';return true;")
                oImagebutton3.Attributes.Add("onmouseout", "window.status='';return true;")
                oImagebutton3.ToolTip = "Modifica Comunità"
            Catch ex As Exception

            End Try
            Try
                Dim oLinkGestione As LinkButton
                oLinkGestione = e.Item.Cells(8).FindControl("LKBservizi")
                If Not IsNothing(oLinkGestione) Then
                    oLinkGestione.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkGestione.Attributes.Add("onfocus", "window.status='Gestione servizi.';return true;")
                    oLinkGestione.Attributes.Add("onmouseover", "window.status='Gestione servizi.';return true;")
                    oLinkGestione.Attributes.Add("onclick", "window.status='Gestione servizi.';return true;")
                End If
            Catch ex As Exception

            End Try

            Try
                Dim oLinkIscritti As LinkButton
                oLinkIscritti = e.Item.Cells(8).FindControl("LKBiscritti")
                If Not IsNothing(oLinkIscritti) Then
                    oLinkIscritti.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkIscritti.Attributes.Add("onfocus", "window.status='Gestione Iscritti.';return true;")
                    oLinkIscritti.Attributes.Add("onmouseover", "window.status='Gestione Iscritti.';return true;")
                    oLinkIscritti.Attributes.Add("onclick", "window.status='Gestione Iscritti.';return true;")
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    Sub DGComunita_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGComunita.PageIndexChanged
        DGComunita.CurrentPageIndex = e.NewPageIndex
        Dim PRSN_ID As Integer
        PRSN_ID = Me.HDprsn_id.Value
        Me.Bind_GrigliaComunita(PRSN_ID)
    End Sub

    Private Sub SortElenco2(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGComunita.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = viewstate("SortExspression2")
        oSortDirection = viewstate("SortDirection2")
        viewstate("SortExspression2") = e.SortExpression

        If e.SortExpression = oSortExpression Then
            If viewstate("SortDirection2") = "asc" Then
                viewstate("SortDirection2") = "desc"
            Else
                viewstate("SortDirection2") = "asc"
            End If
        End If
        Dim PRSN_ID As Integer
        PRSN_ID = Me.HDprsn_id.Value
        Me.Bind_GrigliaComunita(PRSN_ID)
    End Sub

    Private Sub Cambio_NumPagine2(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.DGComunita.PageSize = DDLNumeroRecord.Items(DDLNumeroRecord.SelectedIndex).Value
        Dim PRSN_ID As Integer
        PRSN_ID = Me.HDprsn_id.Value
        Me.Bind_GrigliaComunita(PRSN_ID)
    End Sub

    Public Sub DGComunita_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGComunita.ItemCommand
        Dim CMNT_ID As Integer
        Try
            'CMNT_Path = DGComunita.Items(e.Item.ItemIndex).Cells(9).Text()
            CMNT_ID = CInt(DGComunita.DataKeys.Item(e.Item.ItemIndex))
            Select Case e.CommandName
                Case "cancella"
                    'Me.HDNcmnt_ID.Value = CMNT_ID
                    'If CBool(DGComunita.Items(e.Item.ItemIndex).Cells(10).Text) Then

                    '    Me.StartToCancel(CMNT_ID, CMNT_Path)

                    '    'tutta le gestione della cancellazione
                    'Else
                    '    CMNT_PadreId_Link = DGComunita.Items(e.Item.ItemIndex).Cells(11).Text
                    '    'Trattasi di link, posso cancellarlo senza problemi......
                    '    Me.CancellaComunitaLink_Singola(CMNT_ID, CMNT_PadreId_Link)
                    'End If

                Case "modifica"
                    'Dim i_link As String
                    'Session("idComunita_forAdmin") = CMNT_ID
                    'i_link = "./Adming_modificaComunita.aspx?FROM=ricercabypersona"
                    'Me.SaveSearchParameters()
                    'Response.Redirect(i_link)

                Case "dettagli"
                    'Dim i_link As String
                    'Session("idComunita_forAdmin") = CMNT_ID
                    'i_link = "./../AdminG_Schedainformativa.aspx?FROM=ricercabypersona"
                    'Me.SaveSearchParameters()

                    'Response.Redirect(i_link)

                Case "associa"
                    'Session("azione") = "associa"
                    'Response.Redirect("./SottoComunita.aspx?CMNT_id=" & CMNT_ID)
                Case "servizi"
                    Session("idComunita_forAdmin") = CMNT_ID
                    Me.SaveSearchParameters()
                    Me.Response.Redirect("./AdminG_GestioneServizi.aspx?from=ricercabypersona")
                Case "iscritti"
                    Dim i_link As String
                    Session("idComunita_forAdmin") = CMNT_ID
                    i_link = "./Adming_gestioneiscritti.aspx?FROM=ricercabypersona"
                    Me.SaveSearchParameters()
                    Response.Redirect(i_link)
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Public Sub IMGEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMGEdit.Click
        Dim CMNT_ID As Integer

        Dim otableCell As New System.Web.UI.WebControls.TableCell
        Dim oDatagridItem As System.Web.UI.WebControls.DataGridItem
        Dim i_link As String
        Try

            otableCell = sender.parent
            oDatagridItem = otableCell.Parent
            CMNT_ID = CInt(DGComunita.DataKeys.Item(oDatagridItem.ItemIndex))
            'CInt(DGpersona.DataKeys.Item(oDatagridItem.ItemIndex))
            Session("idComunita_forAdmin") = CMNT_ID
            i_link = "./Adming_modificaComunita.aspx?FROM=ricercabypersona"
            Me.SaveSearchParameters()
            Response.Redirect(i_link)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub IMGDettagli_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMGDettagli.Click
        Dim CMNT_ID As Integer

        Dim otableCell As New System.Web.UI.WebControls.TableCell
        Dim oDatagridItem As System.Web.UI.WebControls.DataGridItem
        Dim i_link As String
        Try

            otableCell = sender.parent
            oDatagridItem = otableCell.Parent
            CMNT_ID = CInt(DGComunita.DataKeys.Item(oDatagridItem.ItemIndex))
            'CInt(DGpersona.DataKeys.Item(oDatagridItem.ItemIndex))
            Session("idComunita_forAdmin") = CMNT_ID
            i_link = "./Adming_schedainformativa.aspx?FROM=ricercabypersona"
            Me.SaveSearchParameters()
            Response.Redirect(i_link)
        Catch ex As Exception

        End Try
    End Sub
#End Region


    Private Sub BTNindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNindietro.Click
        Me.HDprsn_id.Value = ""
        Me.PNLsceltaComunita.Visible = False
        Me.PNLsceltaPersona.Visible = True
        Me.PNLnorecordCmnt.Visible = False
        BindGriglia()
        'Me.LBTitolo.Text = "- Scegli la persona per avere le sue comunità -"
        Me.Master.ServiceTitle = "- Scegli la persona per avere le sue comunità -"
    End Sub


#Region "filtri comunita"

    Private Sub RBLvisualizza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLvisualizza.SelectedIndexChanged
        Dim PRSN_ID As Integer
        PRSN_ID = Me.HDprsn_id.Value
        Me.Bind_GrigliaComunita(PRSN_ID)
    End Sub
    Private Sub BTNcercaComunita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcercaComunita.Click
        Dim PRSN_ID As Integer
        PRSN_ID = Me.HDprsn_id.Value
        Me.Bind_GrigliaComunita(PRSN_ID)
    End Sub
#End Region



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

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property

End Class
