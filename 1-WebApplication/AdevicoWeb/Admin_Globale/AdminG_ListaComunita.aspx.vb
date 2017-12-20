
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_permessi


Public Class AdminG_ListaComunita
    Inherits System.Web.UI.Page


#Region "FORM PERMESSI"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

#Region "Filtri"

    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table

    Protected WithEvents TBRorganizzazione As System.Web.UI.WebControls.TableRow
    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLannoAccademico As System.Web.UI.WebControls.DropDownList

    Protected WithEvents LBtipoComunita_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBnumeroRecord_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBCtipoRicerca_c As System.Web.UI.WebControls.TableCell
    Protected WithEvents LBtipoRicerca_c As System.Web.UI.WebControls.Label

    Protected WithEvents LBvalore_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBCvalore_c As System.Web.UI.WebControls.TableCell
    Protected WithEvents LBvuota_c As System.Web.UI.WebControls.Label


    Protected WithEvents DDLTipo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList

    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button

    Protected WithEvents TBRcorsi As System.Web.UI.WebControls.TableRow

    Protected WithEvents TBLcorsi As System.Web.UI.WebControls.Table

    Protected WithEvents LBorganizzazione_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBannoAccademico_c As System.Web.UI.WebControls.Label
 
    Protected WithEvents DDLperiodo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBperiodo_c As System.Web.UI.WebControls.Label




    '  Protected WithEvents LNBtreeView As System.Web.UI.WebControls.LinkButton
    Protected WithEvents RBLvisualizza As System.Web.UI.WebControls.RadioButtonList


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

#Region "Pannello Contenuto"
    Protected WithEvents PNLContenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents DGcomunita As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBmsgDG As System.Web.UI.WebControls.Label
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LNBvisualizzazione As System.Web.UI.WebControls.LinkButton
#End Region

#Region "Form Dettagli"
    Protected WithEvents PNLdettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents CTRLDettagli As Comunita_OnLine.UC_DettagliComunita
    Protected WithEvents BTNnascondi As System.Web.UI.WebControls.Button
    Protected WithEvents BTNentra As System.Web.UI.WebControls.Button
    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcmnt_Path As System.Web.UI.HtmlControls.HtmlInputHidden

#End Region

#Region "Form Visualizza"

    Protected WithEvents PNLvisualizzazione As System.Web.UI.WebControls.Panel
    Protected WithEvents LBvisualizzazione As System.Web.UI.WebControls.Label
    Protected WithEvents CBXvisualizza As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents LNBchiudi As System.Web.UI.WebControls.LinkButton
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

        Me.Master.ServiceTitle = "Elenco comunità"
        If Not IsPostBack Then
            Try
                Dim oPersona As New COL_Persona
                oPersona = Session("objPersona")

                Session("idComunita_forAdmin") = 0
                If oPersona.TipoPersona.id = Main.TipoPersonaStandard.SysAdmin Then
                    Me.PNLContenuto.Visible = True
                    Me.PNLpermessi.Visible = False

                    Me.SetupFiltri()
                    If Me.Request.QueryString("re_set") <> "true" Then
                        Me.ViewState("intCurPage") = 0
                        Me.ViewState("intAnagrafica") = "-1"
                        Me.LKBtutti.CssClass = "lettera_Selezionata"
                    End If
                Else
                    Me.PNLContenuto.Visible = False
                    Me.PNLpermessi.Visible = True
                    Me.LBNopermessi.Text = "Spiacente, non dispone dei permessi necessari per accedere a tale sezione."
                End If
            Catch ex As Exception
                Response.Redirect("./../index.aspx")
            End Try
        End If
    End Sub

#Region "Bind_Dati"
    Private Sub SetupFiltri()
        If Me.Request.QueryString("re_set") = "true" Then
            Me.Bind_TipiComunita()
            Me.Bind_Organizzazioni()
            Try
                Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("ListaComunita")("organizzazione")
            Catch ex As Exception
                Me.Response.Cookies("ListaComunita")("organizzazione") = Me.DDLorganizzazione.SelectedValue
            End Try
      
            Me.Setup_Visualizzazione(True, True)
            Me.SetupSearchParameters()
            Me.Bind_Griglia()
        Else
            Me.Bind_TipiComunita()
            Me.Bind_Organizzazioni()
          
            Me.Setup_Visualizzazione(True, True)
            Me.Bind_Griglia()
        End If
        
    End Sub

    'Gestione Visualizzazione Colonne Datagrid
    Private Sub Setup_Visualizzazione(Optional ByVal salva As Boolean = False, Optional ByVal recupera As Boolean = False)
        Dim elencoSelezionati As String
        Dim i, totale As Integer

        If recupera Then
            Try
                elencoSelezionati = Me.Request.Cookies("ListaComunita")("Colonne")
                If elencoSelezionati = "" Then
                    Me.SetColonneDefault()
                Else
                    For i = 0 To Me.CBXvisualizza.Items.Count - 1
                        If InStr(elencoSelezionati, "," & Me.CBXvisualizza.Items(i).Value & ",") > 0 Then
                            CBXvisualizza.Items(i).Selected = True
                            Me.DGcomunita.Columns(CBXvisualizza.Items(i).Value).Visible = True
                        Else
                            CBXvisualizza.Items(i).Selected = False
                            Me.DGcomunita.Columns(CBXvisualizza.Items(i).Value).Visible = False
                        End If
                    Next
                End If
            Catch ex As Exception
                Me.SetColonneDefault()
                Me.Response.Cookies("ListaComunita")("Colonne") = ",1,3,7,10,11,"
            End Try

        ElseIf salva Then
            elencoSelezionati = ""
            If Me.CBXvisualizza.SelectedIndex < 0 Then
                'Tipo Comunità
                Me.CBXvisualizza.Items(0).Selected = True
                ' Nome
                Me.CBXvisualizza.Items(2).Selected = True
                'Creatore
                Me.CBXvisualizza.Items(6).Selected = True
                'Servizi
                Me.CBXvisualizza.Items(8).Selected = True
                'Iscritti
                Me.CBXvisualizza.Items(9).Selected = True

                Me.DGcomunita.Columns(1).Visible = True
                Me.DGcomunita.Columns(3).Visible = True
                Me.DGcomunita.Columns(7).Visible = True
                Me.DGcomunita.Columns(10).Visible = True
                Me.DGcomunita.Columns(11).Visible = True
            Else
                For i = 0 To Me.CBXvisualizza.Items.Count - 1
                    If CBXvisualizza.Items(i).Selected = True Then
                        If elencoSelezionati = "" Then
                            elencoSelezionati = "," & CBXvisualizza.Items(i).Value & ","
                            Me.DGcomunita.Columns(CBXvisualizza.Items(i).Value).Visible = True
                        Else
                            Me.DGcomunita.Columns(CBXvisualizza.Items(i).Value).Visible = True
                        End If
                    Else
                        Me.DGcomunita.Columns(CBXvisualizza.Items(i).Value).Visible = False
                    End If
                Next
            End If

            Me.Response.Cookies("ListaComunita")("Colonne") = elencoSelezionati
            Me.Bind_Griglia()
        Else
            Try
                elencoSelezionati = Me.Request.Cookies("ListaComunita")("Colonne")

                If elencoSelezionati = "" Then
                    Me.SetColonneDefault()
                Else
                    For i = 0 To Me.CBXvisualizza.Items.Count - 1
                        If InStr(elencoSelezionati, "," & Me.CBXvisualizza.Items(i).Value & ",") > 0 Then
                            CBXvisualizza.Items(i).Selected = True
                            Me.DGcomunita.Columns(CBXvisualizza.Items(i).Value).Visible = True
                        Else
                            CBXvisualizza.Items(i).Selected = False
                            Me.DGcomunita.Columns(CBXvisualizza.Items(i).Value).Visible = False
                        End If
                    Next
                End If
            Catch ex As Exception
                Me.SetColonneDefault()
            End Try
        End If
    End Sub
    Private Sub SetColonneDefault()
        Me.CBXvisualizza.Items(0).Selected = True
        Me.CBXvisualizza.Items(1).Selected = False
        Me.CBXvisualizza.Items(2).Selected = True
        Me.CBXvisualizza.Items(3).Selected = False
        Me.CBXvisualizza.Items(4).Selected = False
        Me.CBXvisualizza.Items(5).Selected = False
        Me.CBXvisualizza.Items(6).Selected = True
        Me.CBXvisualizza.Items(7).Selected = False
        Me.CBXvisualizza.Items(8).Selected = True
        Me.CBXvisualizza.Items(9).Selected = True

        Me.DGcomunita.Columns(1).Visible = True
        Me.DGcomunita.Columns(2).Visible = False
        Me.DGcomunita.Columns(3).Visible = True
        Me.DGcomunita.Columns(4).Visible = False
        Me.DGcomunita.Columns(5).Visible = False
        Me.DGcomunita.Columns(6).Visible = False
        Me.DGcomunita.Columns(7).Visible = True
        Me.DGcomunita.Columns(9).Visible = False
        Me.DGcomunita.Columns(10).Visible = True
        Me.DGcomunita.Columns(11).Visible = True

    End Sub

    'Bind dati relativi ai filtri.
    Private Function Bind_TipiComunita()
        '...nella ddl che mi farà da filtro delle tipologie di utenti associate al tipo comunità
        Dim oDataSet As New DataSet
        Dim oTipoComunita As New COL_Tipo_Comunita


        Try
            oDataSet = oTipoComunita.ElencaForFiltri(Session("LinguaID"), True, 0)
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

    End Function
    Private Function Bind_Organizzazioni()
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

                    If IsNothing(Session("ORGN_id")) = False Then
                        Try
                            Me.DDLorganizzazione.SelectedValue = Session("ORGN_id")
                        Catch ex As Exception
                            Me.DDLorganizzazione.SelectedIndex = 0
                        End Try
                    Else
                        Me.DDLorganizzazione.SelectedIndex = 0
                    End If
                    Me.TBRorganizzazione.Visible = True
                Else
                    Me.DDLorganizzazione.Enabled = False
                    Me.TBRorganizzazione.Visible = False
                End If
            Else
                Me.TBRorganizzazione.Visible = False
                Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
                Me.DDLorganizzazione.Enabled = False
            End If
        Catch ex As Exception
            Me.DDLorganizzazione.Items.Clear()
            Me.TBRorganizzazione.Visible = False
            Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
            Me.DDLorganizzazione.Enabled = False
        End Try
    End Function

    Private Function FiltraggioDati() As DataSet
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona

        Dim i, totale, totaleHistory, TPCM_ID As Integer
        Dim oDataset As New DataSet
        Dim ArrComunita(,) As String

        Try

            oPersona = Session("objPersona")

            If DDLTipo.SelectedIndex = -1 Then
                TPCM_ID = -1
            Else
                TPCM_ID = DDLTipo.SelectedItem.Value
            End If

            Dim valore As String = ""
            Dim oFiltro As Main.FiltroComunita
            Dim oFiltroCampoOrdine As Main.FiltroCampoOrdineComunita
            Dim oFiltroOrdinamento As Main.FiltroOrdinamento

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

            If valore = "" Then
                Try
                    If IsNumeric(Me.ViewState("intAnagrafica")) Then
                        oFiltro = CType(Me.ViewState("intAnagrafica"), Main.FiltroComunita)
                    Else
                        oFiltro = Main.FiltroComunita.tutti
                    End If
                Catch ex As Exception
                    oFiltro = Main.FiltroComunita.tutti
                End Try
            End If

            If viewstate("SortDirection") = "" Or viewstate("SortDirection") = "asc" Then
                oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente
            Else
                oFiltroOrdinamento = Main.FiltroOrdinamento.Decrescente
            End If

            Select Case viewstate("SortExspression")
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
                    oFiltroCampoOrdine = Main.FiltroCampoOrdineComunita.Livello
            End Select
            'If Session("limbo") = True Then
            '    'se vengo dal limbo allora mostro tutte le comunità a cui appartengo, ordinate per data ultimo accesso
            '    If Me.TBRcorsi.Visible = False Then
            '        oDataset = oComunita.ElencaComunita_New(Session("LinguaID"), Me.ViewState("intCurPage"), Me.DGcomunita.PageSize, Me.DDLorganizzazione.SelectedValue, Main.FiltroRicercaComunitaByIscrizione.tutte, 0, Session("objPersona").id, TPCM_ID, oFiltroOrdinamento, oFiltroCampoOrdine, oFiltro, valore)
            '    Else
            '        oDataset = oComunita.ElencaComunita_New(Session("LinguaID"), Me.ViewState("intCurPage"), Me.DGcomunita.PageSize, Me.DDLorganizzazione.SelectedValue, Main.FiltroRicercaComunitaByIscrizione.tutte, 0, Session("objPersona").id, TPCM_ID, oFiltroOrdinamento, oFiltroCampoOrdine, oFiltro, valore, , Me.DDLannoAccademico.SelectedValue, Me.DDLperiodo.SelectedValue)
            '    End If
            'Else
            '    If Me.TBRcorsi.Visible = False Then
            '        oDataset = oComunita.ElencaComunita_New(Session("LinguaID"), Me.ViewState("intCurPage"), Me.DGcomunita.PageSize, Me.DDLorganizzazione.SelectedValue, Main.FiltroRicercaComunitaByIscrizione.tutte, Session("IdComunita"), Session("objPersona").id, TPCM_ID, oFiltroOrdinamento, oFiltroCampoOrdine, oFiltro, valore)
            '    Else
            '        oDataset = oComunita.ElencaComunita_New(Session("LinguaID"), Me.ViewState("intCurPage"), Me.DGcomunita.PageSize, Me.DDLorganizzazione.SelectedValue, Main.FiltroRicercaComunitaByIscrizione.tutte, Session("IdComunita"), Session("objPersona").id, TPCM_ID, oFiltroOrdinamento, oFiltroCampoOrdine, oFiltro, valore, , Me.DDLannoAccademico.SelectedValue, Me.DDLperiodo.SelectedValue)
            '    End If
            'End If
           
                oDataset = oComunita.RicercaComunitaForManagement(Session("LinguaID"), Me.DDLorganizzazione.SelectedValue, , , Session("objPersona").id, oFiltro, , valore, Me.DDLTipo.SelectedValue, , , , , Main.FiltroStatoComunita.Tutte, Main.FiltroRicercaComunitaByIscrizione.tutte)


            oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_NomePadre"))
            oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Esteso"))
            ''     oDataset.Tables(0).Columns.Add(New DataColumn("RLPC_UltimoCollegamentoStringa"))
            oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_EstesoNoSpan"))
            oDataset.Tables(0).Columns.Add(New DataColumn("AnnoAccademico"))
            oDataset.Tables(0).Columns.Add(New DataColumn("Periodo"))

            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                Me.DGcomunita.CurrentPageIndex = Me.ViewState("intCurPage")
                Me.DGcomunita.VirtualItemCount = oDataset.Tables(0).Rows(0).Item("CMNT_Totale")
                Me.LNBvisualizzazione.Visible = True
            Else
                If Me.ViewState("intCurPage") > 0 Then
                    Me.ViewState("intCurPage") = Me.ViewState("intCurPage") - 1
                    Me.DGcomunita.CurrentPageIndex = Me.ViewState("intCurPage")
                    Return Me.FiltraggioDati
                Else
                    Me.DGcomunita.VirtualItemCount = 0
                    Me.LNBvisualizzazione.Visible = False
                End If

            End If
        Catch ex As Exception

        End Try
        Return oDataset
    End Function

    Private Function Bind_Griglia()
        Dim CMNT_Padre_ID, ORGN_ID As Integer
        'carica le comunità nella datagrid DGComunita
        'se gli passo 0 seleziona tutte
        Me.LBmsgDG.Visible = False
        Me.DGcomunita.Visible = True 'se la datagrid era vuota allora era stata nascosta


        Dim oPersona As New COL_Persona
        Dim oDataset As DataSet
        Dim i, totale, totaleHistory As Integer
        Dim Path As String
        oPersona = Session("objPersona")

        Try
            Dim oTreeComunita As New COL_TreeComunita

            oDataset = Me.FiltraggioDati()

            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                ' al posto della datagrid mostro un messaggio!
                Me.DGcomunita.Visible = False
                Me.LBmsgDG.Visible = True
                Me.LBmsgDG.Text = "Nessun elemento per questa selezione"
                Me.RBLvisualizza.Visible = False
            Else
                Me.RBLvisualizza.Visible = True

                totale = oDataset.Tables(0).Rows.Count
                If totale > 0 Then
                    Dim ImageBaseDir, img As String
                    ImageBaseDir = GetPercorsoApplicazione(Me.Request)
                    ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"

                    'cicla gli elementi del dataset e prepara i dati per la successiva visualizzazione nella datagrid
                    For i = 0 To totale - 1
                        Dim oRow As DataRow

                        oRow = oDataset.Tables(0).Rows(i)
                        ' mostro una diversa icona in base all'attivazione o all'abilitazione
                        '  If CBool(oRow.Item("CMNT_isIscritto")) Then
                        If IsDBNull(oRow.Item("CMNT_AnnoAccademico")) Then
                            oRow.Item("AnnoAccademico") = "&nbsp;"
                        Else
                            oRow.Item("AnnoAccademico") = oRow.Item("CMNT_AnnoAccademico")
                        End If
                        If IsDBNull(oRow.Item("CMNT_PRDO_descrizione")) Then
                            oRow.Item("Periodo") = "&nbsp;"
                        Else
                            oRow.Item("Periodo") = oRow.Item("CMNT_PRDO_descrizione")
                        End If
                        If IsDBNull(oRow.Item("TPCM_icona")) Then
                            img = ""
                        Else
                            img = oRow.Item("TPCM_icona")
                            img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                            ' img = ImageBaseDir & img
                        End If
                        oRow.Item("TPCM_icona") = img

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

                        oRow.Item("CMNT_Esteso") = "<span class=small_Padre>" & oRow.Item("CMNT_NomePadre") & "</span>" & oRow.Item("CMNT_Nome")
                        oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_NomePadre") & oRow.Item("CMNT_Nome")
                        '  End If
                        If i >= totale - 1 Then
                            Exit For
                        End If
                    Next

                    Dim oDataview As DataView
                    oDataview = oDataset.Tables(0).DefaultView
                    If viewstate("SortExspression") = "" Then
                        viewstate("SortExspression") = "CMNT_Livello"
                        viewstate("SortDirection") = "asc"
                    End If
                    oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                    '     Me.PNLgriglia.Visible = True
                    '     Me.PNLnorecord.Visible = False

                    totale = oDataset.Tables(0).Rows.Count
                    totale = oDataview.Count

                    DGcomunita.DataSource = oDataview
                    DGcomunita.DataBind()
                Else
                    Me.DGcomunita.Visible = False
                    Me.LBmsgDG.Visible = True
                    Me.LBmsgDG.Text = "Nessun elemento per questa selezione"
                End If
            End If
        Catch ex As Exception
            Me.DGcomunita.Visible = False
            Me.LBmsgDG.Visible = True
            Me.LBmsgDG.Text = "Nessun elemento per questa selezione"
        End Try
    End Function

#End Region

#Region "Setup Parametri Ricerca"
    Private Sub SaveSearchParameters()
        Try
            Me.Response.Cookies("ListaComunita")("annoAccademico") = Me.DDLannoAccademico.SelectedValue
            Me.Response.Cookies("ListaComunita")("numeroRecord") = Me.DDLNumeroRecord.SelectedValue
            Me.Response.Cookies("ListaComunita")("organizzazione") = Me.DDLorganizzazione.SelectedValue
            Me.Response.Cookies("ListaComunita")("periodo") = Me.DDLperiodo.SelectedValue
            Me.Response.Cookies("ListaComunita")("tipo") = Me.DDLTipo.SelectedValue
            Me.Response.Cookies("ListaComunita")("tiporicerca") = Me.DDLTipoRicerca.SelectedValue
            Me.Response.Cookies("ListaComunita")("valore") = Me.TXBValore.Text
            Me.Response.Cookies("ListaComunita")("intCurPage") = Me.ViewState("intCurPage")
            Me.Response.Cookies("ListaComunita")("intAnagrafica") = Me.ViewState("intAnagrafica")
            Me.Response.Cookies("ListaComunita")("SortDirection") = Me.ViewState("SortDirection")
            Me.Response.Cookies("ListaComunita")("SortExspression") = Me.ViewState("SortExspression")
        Catch ex As Exception

        End Try
    End Sub
    Private Sub SetupSearchParameters()
        Try
            'Recupero fattori di ricerca relativi all'ordinamento
            Try
                Me.ViewState("SortDirection") = Me.Request.Cookies("ListaComunita")("SortDirection")
                Me.ViewState("SortExspression") = Me.Request.Cookies("ListaComunita")("SortExspression")
            Catch ex As Exception

            End Try

            Try
                'Recupero dati relativi alla paginazione corrente
                If IsNumeric(Me.Request.Cookies("ListaComunita")("intCurPage")) Then
                    Me.ViewState("intCurPage") = CInt(Me.Request.Cookies("ListaComunita")("intCurPage"))
                    Me.DGcomunita.CurrentPageIndex = CInt(Me.ViewState("intCurPage"))
                Else
                    Me.ViewState("intCurPage") = 0
                    Me.DGcomunita.CurrentPageIndex = 0
                End If
            Catch ex As Exception
                Me.ViewState("intCurPage") = 0
                Me.DGcomunita.CurrentPageIndex = 0
            End Try
            Try
                Me.TXBValore.Text = Me.Request.Cookies("ListaComunita")("valore")
            Catch ex As Exception
                Me.TXBValore.Text = ""
            End Try

            'Vedo se selezionare qualche linkbutton !!
            Try
                If IsNumeric(Me.Request.Cookies("ListaComunita")("intAnagrafica")) Then
                    Dim oFiltro As Main.FiltroComunita
                    Dim Lettera As String
                    Lettera = CType(CInt(Me.Request.Cookies("ListaComunita")("intAnagrafica")), Main.FiltroComunita).ToString

                    Dim oLink As System.Web.UI.WebControls.LinkButton
                    oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
                    If IsNothing(oLink) = False Then
                        oLink.CssClass = "lettera_Selezionata"
                        Me.ViewState("intAnagrafica") = CInt(Me.Request.Cookies("ListaComunita")("intAnagrafica"))
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
            Try
                If IsNumeric(Me.Request.Cookies("ListaComunita")("annoAccademico")) Then
                    Try
                        Me.DDLannoAccademico.SelectedValue = Me.Request.Cookies("ListaComunita")("annoAccademico")
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try
            ' Setto il periodo
            Try
                If IsNumeric(Me.Request.Cookies("ListaComunita")("periodo")) Then
                    Try
                        Me.DDLperiodo.SelectedValue = Me.Request.Cookies("ListaComunita")("periodo")
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try
            ' Setto l'organizzazione
            Try
                If IsNumeric(Me.Request.Cookies("ListaComunita")("organizzazione")) Then
                    Try
                        Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("ListaComunita")("organizzazione")
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try

            ' Setto il numero di record
            Try
                If IsNumeric(Me.Request.Cookies("ListaComunita")("numeroRecord")) Then
                    Me.DDLNumeroRecord.SelectedValue = Me.Request.Cookies("ListaComunita")("numeroRecord")
                End If
            Catch ex As Exception

            End Try
            ' Setto il numero di record
            Try
                ' If IsNumeric(Me.Request.Cookies("ListaComunita")("tipo")) Then
                Me.DDLTipo.SelectedValue = Me.Request.Cookies("ListaComunita")("tipo")
                If Me.DDLTipo.SelectedValue = 1 Then
                    Me.TBRcorsi.Visible = True
                Else
                    Me.TBRcorsi.Visible = False
                End If
                'Else
                '      Me.TBRcorsi.Visible = False
                '  End If
            Catch ex As Exception
                Me.TBRcorsi.Visible = False
            End Try

            ' Setto il tipo di ricerca
            Try
                If IsNumeric(Me.Request.Cookies("ListaComunita")("tiporicerca")) Then
                    Me.DDLTipoRicerca.SelectedValue = Me.Request.Cookies("ListaComunita")("tiporicerca")
                End If
            Catch ex As Exception
            End Try

        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Filtro"

    Private Sub DDLTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLTipo.SelectedIndexChanged
        Dim showFiltroCorso As Boolean = False

        If Session("limbo") = True Then
            showFiltroCorso = True
        Else
            Try
                Dim oComunita As New COL_Comunita
                oComunita.Id = Session("IDComunita")
                oComunita.Estrai()
                If oComunita.Livello = 0 Or oComunita.Livello = 1 Then
                    showFiltroCorso = True
                End If
            Catch ex As Exception

            End Try
        End If


        If Me.DDLTipo.SelectedValue = 1 And showFiltroCorso Then
            Me.TBRcorsi.Visible = True
        Else
            Me.TBRcorsi.Visible = False
        End If
        DGcomunita.CurrentPageIndex = 0
        Me.ViewState("intCurPage") = 0
        Me.Bind_Griglia()
        '  DGComunita.DataBind()
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        DGcomunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        DGcomunita.CurrentPageIndex = 0
        Me.ViewState("intCurPage") = 0
        Me.Bind_Griglia()
    End Sub
    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
     
        DGcomunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        DGcomunita.CurrentPageIndex = 0
        Me.ViewState("intCurPage") = 0
        Me.Bind_Griglia()
    End Sub
   
    Private Sub BTNCerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        DGcomunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        DGcomunita.CurrentPageIndex = 0
        Me.ViewState("intCurPage") = 0
        Me.Bind_Griglia()
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
        Me.DGcomunita.CurrentPageIndex = 0
        Me.Bind_Griglia()
    End Sub
    Private Sub DeselezionaLink(ByVal Lettera As String)
        Dim oFiltro As Main.FiltroComunita
        Lettera = CType(CInt(Lettera), Main.FiltroComunita).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub
#End Region

#Region "Gestione Griglia"

    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGcomunita.SortCommand
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
        Me.Bind_Griglia()
    End Sub

    Private Sub DGcomunita_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGcomunita.ItemCreated
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
                                oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordinamento decrescente per <" & oLinkbutton.Text & ">';return true;")
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
            Dim oCell As New TableCell
            Dim PRSN_ID As Integer
            Dim oPersona As New COL_Persona
            Dim HasPermessi As Boolean = False

            Try
                oPersona = Session("objPersona")
                If oPersona.TipoPersona.id = Main.TipoPersonaStandard.SysAdmin Then
                    HasPermessi = True
                End If
            Catch ex As Exception
                HasPermessi = False
            End Try

            oCell = CType(e.Item.Cells(0), TableCell)

            Try
                Dim oImage_Dettagli As ImageButton
                oImage_Dettagli = oCell.FindControl("IMGDettagli")
                oImage_Dettagli.ToolTip = "Dettagli"

            Catch ex As Exception

            End Try

            Try
                Dim oImage_Modifica As ImageButton
                oImage_Modifica = oCell.FindControl("IMGEdit")
                oImage_Modifica.ToolTip = "Modifica"

                oImage_Modifica.Visible = HasPermessi

            Catch ex As Exception

            End Try

            Try
                Dim oImage_AssociaPadre As ImageButton
                oImage_AssociaPadre = oCell.FindControl("IMGAssocia")
                oImage_AssociaPadre.ToolTip = "Rendi figlia di altre comunità"

                oImage_AssociaPadre.Visible = HasPermessi

            Catch ex As Exception

            End Try

            Try
                Dim oImage_Elimina As ImageButton
                oImage_Elimina = oCell.FindControl("IMGDelete")

                ' If e.Item.DataItem("CMNT_isFiglioDiretto") Then
                oImage_Elimina.Attributes.Add("onclick", "window.status='Elimina la comunità dal database.';alert(' Funzione in fase di sviluppo');return false;")
                'oImage_Elimina.Attributes.Add("onclick", "window.status='Elimina la comunità dal database.';return confirm(' Sei sicuro di eliminare la comunità selezionata ?');")
                oImage_Elimina.ToolTip = "Elimina"
                'Else
                '    oImage_Elimina.Attributes.Add("onclick", "window.status='Elimina il link alla comunità selezionata.';return confirm(' Sei sicuro di eliminare il link alla comunità selezionata ?');")
                '    oImage_Elimina.ToolTip = "Elimina"
                'End If

                oImage_Elimina.Visible = HasPermessi

            Catch ex As Exception

            End Try

            Try
                Dim oLinkButton As LinkButton
                oLinkbutton = e.Item.Cells(9).Controls(0)
                If IsNothing(oLinkbutton) = False Then
                    oLinkbutton.Attributes.Add("onclick", "window.status='Accedi alla comunità selezionata.';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='Accedi alla comunità selezionata.';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='Accedi alla comunità selezionata.';return true;")
                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Visible = True
                End If
            Catch ex As Exception

            End Try

            Try
                Dim oLinkButton As LinkButton
                oLinkbutton = e.Item.Cells(10).Controls(0)
                If IsNothing(oLinkbutton) = False Then
                    oLinkbutton.Attributes.Add("onclick", "window.status='Gestione servizi relativi alla comunità selezionata.';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='Gestione servizi relativi  alla comunità selezionata.';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='Gestione servizi relativi  alla comunità selezionata.';return true;")
                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Visible = True
                End If
            Catch ex As Exception

            End Try
            Try
                Dim oLinkButton As LinkButton
                oLinkbutton = e.Item.Cells(11).Controls(0)
                If IsNothing(oLinkbutton) = False Then
                    oLinkbutton.Attributes.Add("onclick", "window.status='Gestione iscritti relativi  alla comunità selezionata.';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='Gestione iscritti relativi  alla comunità selezionata.';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='Gestione iscritti relativi  alla comunità selezionata.';return true;")
                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Visible = True
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub DGcomunita_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGcomunita.ItemCommand
        Dim CMNT_ID As Integer
        Dim CMNT_path As String
        'Dim otableCell As New System.Web.UI.WebControls.TableCell
        'Dim oDatagridItem As System.Web.UI.WebControls.DataGridItem
        'otableCell = sender.parent
        'oDatagridItem = otableCell.Parent
        Try
            CMNT_ID = CInt(DGcomunita.DataKeys.Item(e.Item.ItemIndex))
            CMNT_path = DGcomunita.Items(e.Item.ItemIndex).Cells(13).Text
            '  CMNT_path = DGcomunita.Items(oDatagridItem.ItemIndex).Cells(15).Text()
            Select Case e.CommandName
                Case "Login"

                    '       Me.EntraComunita(CMNT_ID, CMNT_Path)
                Case "servizi"
                    Session("idComunita_forAdmin") = CMNT_ID
                    Me.SaveSearchParameters()
                    Me.Response.Redirect("./AdminG_GestioneServizi.aspx?from=RicercaComunita")
                Case "dettagli"
                    Me.HDNcmnt_ID.Value = CMNT_ID
                    Me.PNLContenuto.Visible = False
                    Me.PNLdettagli.Visible = True
                    Me.CTRLDettagli.SetupDettagliComunita(CMNT_ID)
                Case "modifica"
                    Session("idComunita_forAdmin") = CMNT_ID
                    Me.SaveSearchParameters()
                    Me.Response.Redirect("./AdminG_ModificaComunita.aspx?from=RicercaComunita")
                Case "iscritti"
                    Session("idComunita_forAdmin") = CMNT_ID
                    Session("CMNT_path_forAdmin") = CMNT_path
                    Me.SaveSearchParameters()
                    Me.Response.Redirect("./AdminG_GestioneIscritti.aspx?from=RicercaComunita")
            End Select
        Catch ex As Exception

        End Try
        'If e.CommandName = "login" Then
        '    Dim oPersona As New COL_Persona
        '    oPersona.LogonAsUser(DGpersona.DataKeys.Item(e.Item.ItemIndex))
        '    If oPersona.Errore = Errori_Db.None Then
        '        Dim oTreeComunita As New COL_TreeComunita
        '        Try
        '            Dim PRSN_ID As Integer

        '            PRSN_ID = oPersona.Id
        '            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
        '            oTreeComunita.Nome = PRSN_ID & ".xml"
        '            oTreeComunita.AggiornaInfo(PRSN_ID)
        '        Catch ex As Exception

        '        End Try

        '        'session.abandon non funziona!!!
        '        Session("limbo") = Nothing
        '        Session("objPersona") = Nothing
        '        Session("ORGN_id") = Nothing
        '        Session("Istituzione") = Nothing
        '        Session("IdRuolo") = Nothing
        '        Session("IdComunita") = Nothing


        '        'bisognerebbe andare a cercare le altre sessioni per settarle a nothing

        '        Session("limbo") = True
        '        Session("objPersona") = oPersona
        '        Session("ORGN_id") = oPersona.GetOrganizzazioneDefault
        '        Session("Istituzione") = oPersona.GetIstituzione


        '        'Session("IdRuolo")

        '        Response.Redirect("../Comunita/EntrataComunita.aspx")
        '    Else
        '        'errore in fase di login
        '    End If
        'End If
    End Sub
    Private Sub DGcomunita_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGcomunita.PageIndexChanged
        Dim oSortExpression, oSortDirection As String

        source.CurrentPageIndex = e.NewPageIndex
        Me.ViewState("intCurPage") = e.NewPageIndex
        Bind_Griglia()
    End Sub
#End Region


    Private Sub LNBvisualizzazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBvisualizzazione.Click
        Me.PNLvisualizzazione.Visible = True
        Me.LNBvisualizzazione.Visible = False
    End Sub


    Private Sub LNBchiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudi.Click
        Me.PNLvisualizzazione.Visible = False
        Me.LNBvisualizzazione.Visible = True
        Me.Setup_Visualizzazione(True)
    End Sub

#Region "Dettagli"
  

    'Private Sub BTNentra_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNentra.Click
    '    Me.EntraComunita(Me.HDNcmnt_ID.Value, Me.HDNcmnt_Path.Value)
    'End Sub

    Private Sub BTNnascondi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNnascondi.Click
        Me.PNLdettagli.Visible = False
        Me.PNLContenuto.Visible = True
        Me.Bind_Griglia()
    End Sub
#End Region

   
    Private Sub RBLvisualizza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLvisualizza.SelectedIndexChanged
        Try
            Select Case Me.RBLvisualizza.SelectedValue
                Case 1
                    Me.SaveSearchParameters()
                    Me.Response.Redirect("./AdminG_AlberoComunita.aspx?show=1&from=ricercacomunita")
                Case 2
                    Me.SaveSearchParameters()
                    Me.Response.Redirect("./AdminG_AlberoComunita.aspx?show=2&from=ricercacomunita")
                Case 3
                    Me.Bind_Griglia()
                Case Else
                    Me.Bind_Griglia()
            End Select
        Catch ex As Exception

        End Try
    End Sub




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