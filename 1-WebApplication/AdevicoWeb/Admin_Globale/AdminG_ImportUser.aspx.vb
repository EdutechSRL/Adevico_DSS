Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_permessi
Imports Telerik.WebControls

Public Class AdminG_ImportUser
    Inherits System.Web.UI.Page

    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label


#Region "Panello Tab"
    Protected WithEvents TBSmenu As Telerik.Web.UI.RadTabStrip
#End Region
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents HDN_valore As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected Enum TabIndex
        SorgenteDestinazione = 0
        SceltaUtente = 1
        SceltaRuolo = 2
        Finale = 3
    End Enum

#Region "Pannello_Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

#Region "Pannello_SorgenteDestinatario"
    Protected WithEvents HDN_valoreSorgente As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_valoreDestinatario As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TXBsorgente As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBdestinatario As System.Web.UI.WebControls.TextBox
#Region "Filtri_Sorgente"
    Protected WithEvents PNLsorgente As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLsorgente As System.Web.UI.WebControls.Table
    Protected WithEvents TBRfiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table
    Protected WithEvents TBCorganizzazione_c As System.Web.UI.WebControls.TableCell
    Protected WithEvents LBorganizzazione_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBCorganizzazione As System.Web.UI.WebControls.TableCell
    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBtipoFiltro As System.Web.UI.WebControls.Label
    Protected WithEvents DDLTipo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBRcorsi As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLcorsi As System.Web.UI.WebControls.Table
    Protected WithEvents LBannoAccademico_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLannoAccademico As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBperiodo_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLperiodo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBRcorsoDiStudi As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBtipoCorsoDiStudi As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoCorsoStudi As System.Web.UI.WebControls.DropDownList
#End Region


    Protected WithEvents RDTcomunita As Telerik.WebControls.RadTreeView

#Region "Filtri_Destinatari"
    Protected WithEvents TBLdestinatario As System.Web.UI.WebControls.Table
    Protected WithEvents TBRfiltroDest As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLfiltroDest As System.Web.UI.WebControls.Table
    Protected WithEvents TBCorganizzazioneDest_c As System.Web.UI.WebControls.TableCell
    Protected WithEvents LBorganizzazioneDest_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBCorganizzazioneDest As System.Web.UI.WebControls.TableCell
    Protected WithEvents DDLorganizzazioneDest As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBtipoFiltroDest As System.Web.UI.WebControls.Label
    Protected WithEvents DDLTipoDest As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBRcorsiDest As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLcorsiDest As System.Web.UI.WebControls.Table
    Protected WithEvents LBannoAccademicoDest_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLannoAccademicoDest As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBperiodoDest_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLperiodoDest As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBRcorsoDiStudiDest As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBtipoCorsoDiStudiDest As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoCorsoStudiDest As System.Web.UI.WebControls.DropDownList
#End Region

    Protected WithEvents RDTdestinatario As Telerik.WebControls.RadTreeView

    Protected WithEvents BTNfase0ToFase1 As System.Web.UI.WebControls.Button
#End Region

#Region "Gestione Utenti"
    Protected WithEvents PNLutenti As System.Web.UI.WebControls.Panel
    Protected WithEvents LBsorgenteCmnt_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBsorgenteCmnt As System.Web.UI.WebControls.Label
    Protected WithEvents LBdestinazioneCmnt_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdestinazioneCmnt As System.Web.UI.WebControls.Label
    Protected WithEvents LBsceltaUtenti As System.Web.UI.WebControls.Label
    Protected WithEvents RBLutenti As System.Web.UI.WebControls.RadioButtonList

    Protected WithEvents HDNsorgente_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNdestinazione_ID As System.Web.UI.HtmlControls.HtmlInputHidden

#Region "Filtro"
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
    Protected WithEvents PNLfiltro As System.Web.UI.WebControls.Panel
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button
    Protected WithEvents DDLTipoRuolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoPersona As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
    Protected WithEvents HDabilitato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDattivato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDtutti As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents LBtipoRuolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoPersona As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents DGPersone As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PNLNoUsers As System.Web.UI.WebControls.Panel
    Protected WithEvents LBMessaggio As System.Web.UI.WebControls.Label

    Protected WithEvents PNLsceltaRuoli As System.Web.UI.WebControls.Panel
    Protected WithEvents LBsceltaRuoli As System.Web.UI.WebControls.Label
    Protected WithEvents CBLsceltaRuoli As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents LBsceltaRuoliDesc As System.Web.UI.WebControls.Label
    Protected WithEvents BTNfase1ToFase2 As System.Web.UI.WebControls.Button
#End Region

#Region "Pannello Ruoli"
    Protected WithEvents PNLruoliDestinazione As System.Web.UI.WebControls.Panel
    Protected WithEvents LBtitoloRuoli As System.Web.UI.WebControls.Label
    Protected WithEvents BTNimporta As System.Web.UI.WebControls.Button
    Protected WithEvents DGruoli As System.Web.UI.WebControls.DataGrid
#End Region

#Region "Pannello Finale"
    Protected WithEvents PNLfinale As System.Web.UI.WebControls.Panel
    Protected WithEvents LBfinale As System.Web.UI.WebControls.Label
    Protected WithEvents BTNreImport As System.Web.UI.WebControls.Button
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

        Me.Master.ServiceTitle = "Import utenti"

        Dim oPersona As New COL_Persona

        If Page.IsPostBack = False Then
            Session("azione") = "load"

            oPersona = Session("objPersona")
            If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.SysAdmin Or oPersona.TipoPersona.ID = Main.TipoPersonaStandard.AdminSecondario Then
                Me.PNLcontenuto.Visible = True
                Me.PNLsorgente.Visible = True
                Me.PNLpermessi.Visible = False
                Me.TBSmenu.Tabs(TabIndex.SceltaRuolo).Visible = False
                Me.TBSmenu.Tabs(TabIndex.Finale).Visible = False
                Me.TBSmenu.Tabs(TabIndex.SceltaUtente).Visible = False
                Me.TBSmenu.SelectedIndex = 0
                Me.SetupFiltri()
                Me.SetStartupScript()
                Me.Bind_TreeView()
                Me.Bind_TreeView_Destinatario()
            Else
                Me.PNLcontenuto.Visible = False
                Me.PNLpermessi.Visible = True
                Me.LBNopermessi.Text = "Non si dispone dei permessi necessari per accedere a questa pagina."
            End If


        End If

    End Sub

    Private Sub SetStartupScript()
        'aggiunge ai link button le proprietà da visualizzare nella barra di stato
        Dim i As Integer
        For i = Asc("a") To Asc("z") 'status dei link button delle lettere
            Dim oLinkButton As New LinkButton
            oLinkButton = FindControl("LKB" & Chr(i))
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
        '-- SETTAGGIO PROPRIETA' LINKBUTTON

        LKBaltro.Attributes.Add("onclick", "window.status='Altri caratteri' ;return true;")
        LKBaltro.Attributes.Add("onmouseover", "window.status='Altri caratteri' ;return true;")
        LKBaltro.Attributes.Add("onfocus", "window.status='Altri caratteri' ;return true;")
        LKBaltro.Attributes.Add("onmouseout", "window.status='';return true;")

        LKBtutti.Attributes.Add("onclick", "window.status='Tutti i caratteri' ;return true;")
        LKBtutti.Attributes.Add("onmouseover", "window.status='Tutti i caratteri' ;return true;")
        LKBtutti.Attributes.Add("onfocus", "window.status='Tutti i caratteri' ;return true;")
        LKBtutti.Attributes.Add("onmouseout", "window.status='';return true;")
    End Sub



#Region "Gestione Fase1"
#Region "Bind_Dati"
#Region "Bind_Filtri"
    Private Sub SetupFiltri()

        Me.Bind_Organizzazioni()
        If Me.Request.QueryString("from") = "" Then
            Me.Bind_TipiComunita()

        Else
            Me.Bind_TipiComunita()

            Try
                Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("ListaComunita")("organizzazione")
            Catch ex As Exception
                Me.Response.Cookies("ListaComunita")("organizzazione") = Me.DDLorganizzazione.SelectedValue
            End Try

        End If

    End Sub
    Private Sub SetupSearchParameters()
        Dim stringaCookie As String

        Try

            If LCase(Me.Request.QueryString("from")) = "ricercacomunita" Then
                stringaCookie = "ListaComunita"

                Try
                    If IsNumeric(Me.Request.Cookies(stringaCookie)("annoAccademico")) Then
                        Try
                            Me.DDLannoAccademico.SelectedValue = Me.Request.Cookies(stringaCookie)("annoAccademico")
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception

                End Try

                ' Setto il periodo
                Try
                    If IsNumeric(Me.Request.Cookies(stringaCookie)("periodo")) Then
                        Try
                            Me.DDLperiodo.SelectedValue = Me.Request.Cookies(stringaCookie)("periodo")
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception

                End Try
            Else
                stringaCookie = "RicercaComunita"
            End If
            ' Setto l'anno accademico


            ' Setto il numero di record
            Try
                If IsNumeric(Me.Request.Cookies(stringaCookie)("tipo")) Then
                    Me.DDLTipo.SelectedValue = Me.Request.Cookies(stringaCookie)("tipo")
                    If Me.DDLTipo.SelectedValue = 1 Then
                        Me.TBRcorsi.Visible = True
                    Else
                        Me.TBRcorsi.Visible = False
                    End If
                Else
                    Me.TBRcorsi.Visible = False
                End If
            Catch ex As Exception
                Me.TBRcorsi.Visible = False
            End Try

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Bind_Organizzazioni()
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona

        Me.DDLorganizzazione.Items.Clear()
        Me.DDLorganizzazioneDest.Items.Clear()
        Try
            oPersona = Session("objPersona")
            oDataset = oPersona.GetOrganizzazioniAssociate()

            If oDataset.Tables(0).Rows.Count > 0 Then
                Me.DDLorganizzazione.DataValueField = "ORGN_id"
                Me.DDLorganizzazione.DataTextField = "ORGN_ragioneSociale"
                Me.DDLorganizzazione.DataSource = oDataset
                Me.DDLorganizzazione.DataBind()

                Me.DDLorganizzazioneDest.DataValueField = "ORGN_id"
                Me.DDLorganizzazioneDest.DataTextField = "ORGN_ragioneSociale"
                Me.DDLorganizzazioneDest.DataSource = oDataset
                Me.DDLorganizzazioneDest.DataBind()

                If Me.DDLorganizzazione.Items.Count > 1 Then
                    Me.DDLorganizzazione.Enabled = True
                    Me.DDLorganizzazioneDest.Enabled = True
                    If IsNothing(Session("ORGN_id")) = False Then
                        Try
                            Me.DDLorganizzazione.SelectedValue = Session("ORGN_id")
                            Me.DDLorganizzazioneDest.SelectedValue = Session("ORGN_id")
                        Catch ex As Exception
                            Me.DDLorganizzazione.SelectedIndex = 0
                            Me.DDLorganizzazioneDest.SelectedIndex = 0
                        End Try
                    Else
                        Me.DDLorganizzazione.SelectedIndex = 0
                        Me.DDLorganizzazioneDest.SelectedIndex = 0
                    End If
                    Me.TBCorganizzazione.Visible = True
                    Me.TBCorganizzazione_c.Visible = True
                    Me.TBCorganizzazioneDest.Visible = True
                    Me.TBCorganizzazioneDest_c.Visible = True
                Else
                    Me.DDLorganizzazione.Enabled = False
                    Me.TBCorganizzazione.Visible = False
                    Me.TBCorganizzazione_c.Visible = False

                    Me.DDLorganizzazioneDest.Enabled = False
                    Me.TBCorganizzazioneDest.Visible = False
                    Me.TBCorganizzazioneDest_c.Visible = False
                End If
            Else
                Me.TBCorganizzazione.Visible = False
                Me.TBCorganizzazione_c.Visible = False
                Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
                Me.DDLorganizzazione.Enabled = False

                Me.TBCorganizzazioneDest.Visible = False
                Me.TBCorganizzazioneDest_c.Visible = False
                Me.DDLorganizzazioneDest.Items.Add(New ListItem("< nessuna >", 0))
                Me.DDLorganizzazioneDest.Enabled = False
            End If
        Catch ex As Exception
            Me.TBCorganizzazione.Visible = False
            Me.TBCorganizzazione_c.Visible = False
            Me.DDLorganizzazione.Items.Clear()
            Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
            Me.DDLorganizzazione.Enabled = False

            Me.TBCorganizzazioneDest.Visible = False
            Me.TBCorganizzazioneDest_c.Visible = False
            Me.DDLorganizzazioneDest.Items.Clear()
            Me.DDLorganizzazioneDest.Items.Add(New ListItem("< nessuna >", 0))
            Me.DDLorganizzazioneDest.Enabled = False
        End Try
    End Sub

    Private Sub Bind_TipiComunita()
        '...nella ddl che mi farà da filtro delle tipologie di utenti associate al tipo comunità
        Dim oDataSet As New DataSet

        Try
            oDataSet = COL_Tipo_Comunita.ElencaForFiltri(Session("LinguaID"), False)
            If oDataSet.Tables(0).Rows.Count > 0 Then
                DDLTipo.DataSource = oDataSet
                DDLTipo.DataTextField() = "TPCM_descrizione"
                DDLTipo.DataValueField() = "TPCM_id"
                DDLTipo.DataBind()

                'aggiungo manualmente elemento che indica tutti i tipi di comunità
                DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))

                Me.DDLTipoDest.DataSource = oDataSet
                DDLTipoDest.DataTextField() = "TPCM_descrizione"
                DDLTipoDest.DataValueField() = "TPCM_id"
                DDLTipoDest.DataBind()
                DDLTipoDest.Items.Insert(0, New ListItem("-- Tutti --", -1))
            End If
        Catch ex As Exception
            DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
            DDLTipoDest.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try

    End Sub

    Private Sub DLLtipoFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipo.SelectedIndexChanged
       
            Me.TBRcorsi.Visible = False
            Me.TBRcorsoDiStudi.Visible = False
            Me.DDLperiodo.SelectedIndex = 0
            Me.DDLannoAccademico.SelectedIndex = 0
            Me.DDLtipoCorsoStudi.SelectedIndex = 0

        Me.Bind_TreeView()
    End Sub
    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
     
        Me.Bind_TreeView()
    End Sub
  
    Private Sub DDLTipoDest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoDest.SelectedIndexChanged
    
            Me.TBRcorsiDest.Visible = False
            Me.TBRcorsoDiStudiDest.Visible = False
            Me.DDLperiodoDest.SelectedIndex = 0
            Me.DDLannoAccademicoDest.SelectedIndex = 0
            Me.DDLtipoCorsoStudiDest.SelectedIndex = 0

        Me.Bind_TreeView_Destinatario()
    End Sub
    Private Sub DDLorganizzazioneDest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazioneDest.SelectedIndexChanged
        Me.Bind_TreeView_Destinatario()
    End Sub
    Private Sub DDLperiodoDest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLperiodoDest.SelectedIndexChanged
        Me.Bind_TreeView_Destinatario()
    End Sub
    Private Sub DDLannoAccademicoDest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLannoAccademicoDest.SelectedIndexChanged
        Me.Bind_TreeView_Destinatario()
    End Sub
    Private Sub DDLtipoCorsoStudiDest_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLtipoCorsoStudiDest.SelectedIndexChanged
        Me.Bind_TreeView_Destinatario()
    End Sub

#End Region

#End Region

#Region "Gestione TreeView"
    Private Function FiltraggioDati(ByVal oTree As Telerik.WebControls.RadTreeView) As DataSet
        Dim oDataset As New DataSet

        Try
            If Equals(oTree, Me.RDTcomunita) Then
                oDataset = COL_Comunita.ElencaComunita_ForAdmin(Session("LinguaID"), 0, 100, Me.DDLorganizzazione.SelectedValue, , Session("objPersona").id, Me.DDLTipo.SelectedValue, Main.FiltroOrdinamento.Crescente, Main.FiltroCampoOrdineComunita.Nessuno, Main.FiltroComunita.tutti, , Me.DDLtipoCorsoStudi.SelectedValue, Me.DDLannoAccademico.SelectedValue, Me.DDLperiodo.SelectedValue, Main.ElencoRecord.AdAlbero)
                '(0, 100, Me.DDLorganizzazione.SelectedValue, Main.FiltroRicercaComunita.tutte, , Session("objPersona").id, Me.DDLTipo.SelectedValue, Main.FiltroOrdinamento.Crescente, Main.FiltroCampoOrdineComunita.Nessuno, Main.FiltroComunita.tutti, , Me.DDLtipoCorsoStudi.SelectedValue, Me.DDLannoAccademico.SelectedValue, Me.DDLperiodo.SelectedValue, Main.ElencoRecord.AdAlbero)
            Else
                oDataset = COL_Comunita.ElencaComunita_ForAdmin(Session("LinguaID"), 0, 100, Me.DDLorganizzazioneDest.SelectedValue, , Session("objPersona").id, Me.DDLTipoDest.SelectedValue, Main.FiltroOrdinamento.Crescente, Main.FiltroCampoOrdineComunita.Nessuno, Main.FiltroComunita.tutti, , Me.DDLtipoCorsoStudiDest.SelectedValue, Me.DDLannoAccademicoDest.SelectedValue, Me.DDLperiodoDest.SelectedValue, Main.ElencoRecord.AdAlbero)
                '               oDataset = oComunita.ElencaComunita_ForAdmin(0, 100, Me.DDLorganizzazioneDest.SelectedValue, Main.FiltroRicercaComunita.tutte, , Session("objPersona").id, Me.DDLTipoDest.SelectedValue, Main.FiltroOrdinamento.Crescente, Main.FiltroCampoOrdineComunita.Nessuno, Main.FiltroComunita.tutti, , Me.DDLtipoCorsoStudi.SelectedValue, Me.DDLannoAccademicoDest.SelectedValue, Me.DDLperiodoDest.SelectedValue, Main.ElencoRecord.AdAlbero)
            End If
        Catch ex As Exception

        End Try
        Return oDataset
    End Function
    Private Sub Bind_TreeView()
        Dim oPersona As New COL_Persona
        Dim oComunita As New COL_Comunita
        Dim oDataset As New DataSet

        Dim ImageBaseDir As String

        'ImageBaseDir = "http://" & Me.Request.Url.Host & GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")


        Me.RDTcomunita.Nodes.Clear()
        Try
            oPersona = Session("objPersona")

            oDataset = Me.FiltraggioDati(Me.RDTcomunita)

            Me.RDTcomunita.Nodes.Clear()


            Dim nodeRoot As New RadTreeNode
            nodeRoot.Text = "Elenco comunità: "
            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "folder.gif"
            nodeRoot.Value = ""
            nodeRoot.ToolTip = "Elenco comunità: "

            Me.RDTcomunita.Nodes.Add(nodeRoot)
            If oDataset.Tables(0).Rows.Count = 0 Then
                ' nessuna comunità a cui si è iscritti
                Me.GeneraNoNode(Me.RDTcomunita)
            Else
                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("CMNT_ID"), oDataset.Tables(0).Columns("CMNT_idPadre_Link"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("CMNT_idPadre") = 0 Then
                        Dim node As RadTreeNode = CreateNode(dbRow, True)
                        nodeRoot.Nodes.Add(node)
                        RecursivelyPopulate(dbRow, node)
                    End If
                Next dbRow
                'Me.PNLtreeView.Visible = True
            End If
        Catch ex As Exception
            Me.GeneraNoNode(Me.RDTcomunita)
        End Try
    End Sub
    Private Sub Bind_TreeView_Destinatario()
        Dim oPersona As New COL_Persona
        Dim oComunita As New COL_Comunita
        Dim oDataset As New DataSet
        Dim ImageBaseDir As String

        'ImageBaseDir = "http://" & Me.Request.Url.Host & GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")


        Me.RDTdestinatario.Nodes.Clear()
        Try
            oPersona = Session("objPersona")
            oDataset = Me.FiltraggioDati(Me.RDTdestinatario)

            Me.RDTdestinatario.Nodes.Clear()


            Dim nodeRoot As New RadTreeNode
            nodeRoot.Text = "Elenco comunità: "
            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "folder.gif"
            nodeRoot.Value = ""
            nodeRoot.ToolTip = "Elenco comunità: "
            Me.RDTdestinatario.Nodes.Add(nodeRoot)
            If oDataset.Tables(0).Rows.Count = 0 Then
                ' nessuna comunità a cui si è iscritti
                Me.GeneraNoNode(Me.RDTdestinatario)
            Else
                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("CMNT_ID"), oDataset.Tables(0).Columns("CMNT_idPadre_Link"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("CMNT_idPadre") = 0 Then
                        Dim node As RadTreeNode = CreateNode(dbRow, True)
                        nodeRoot.Nodes.Add(node)
                        RecursivelyPopulate(dbRow, node)
                    End If
                Next dbRow
                'Me.PNLtreeView.Visible = True
            End If
        Catch ex As Exception
            Me.GeneraNoNode(Me.RDTdestinatario)
        End Try
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
    Private Sub GeneraNoNode(ByRef oTree As Telerik.WebControls.RadTreeView)
        Dim oRootNode As New RadTreeNode
        Dim oNode As New RadTreeNode

        oRootNode = New RadTreeNode
        oRootNode.Text = "Comunità: "
        oRootNode.Value = ""
        oRootNode.Expanded = True
        oRootNode.ImageUrl = "folder.gif"
        oRootNode.ToolTip = "Elenco comunità a cui si è iscritti"
        oRootNode.Category = 0

        oNode = New RadTreeNode
        oNode.Expanded = True
        oNode.Text = "Non si è iscritti ad alcuna comunità"
        oNode.Value = ""
        oNode.ToolTip = "Non si è iscritti ad alcuna comunità"
        oNode.Category = 0
        oNode.Checkable = False
        oRootNode.Nodes.Add(oNode)

        oTree.Nodes.Clear()
        oTree.Nodes.Add(oRootNode)
    End Sub
    Private Sub RecursivelyPopulate(ByVal dbRow As DataRow, ByVal node As RadTreeNode)
        Dim childRow As DataRow

        For Each childRow In dbRow.GetChildRows("NodeRelation")
            Dim childNode As RadTreeNode = CreateNode(childRow, False)
            node.Nodes.Add(childNode)
            RecursivelyPopulate(childRow, childNode)
            ' End If
        Next childRow
    End Sub 'RecursivelyPopulate
    Private Function CreateNode(ByVal dbRow As DataRow, ByVal expanded As Boolean) As RadTreeNode
        Dim node As New RadTreeNode

        Dim start As Integer
        '  Dim [continue] As Boolean = False
        start = 0

        Dim CMNT_id, RLPC_TPRL_id As Integer
        Dim CMNT_Responsabile, img As String
        Dim CMNT_isIscritto As Boolean
        CMNT_id = dbRow.Item("CMNT_id")

        If IsDBNull(dbRow.Item("RLPC_TPRL_id")) Then
            RLPC_TPRL_id = -1
        Else
            RLPC_TPRL_id = dbRow.Item("RLPC_TPRL_id")
        End If

        Dim ImageBaseDir As String
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")

        'If IsDBNull(dbRow.Item("CMNT_Iscritti")) = False Then
        '    'Dim oComunita As New COL_Comunita

        '    'oComunita.Id = CMNT_id
        '    'oComunita.Estrai()
        '    maxIscritti = dbRow.Item("CMNT_MaxIscritti")

        '    numIscritti = dbRow.Item("CMNT_Iscritti")

        '    If maxIscritti <= 0 Then
        '        dbRow.Item("CMNT_Iscritti") = 0
        '        iscritti = 0
        '    Else
        '        If numIscritti > maxIscritti Then
        '            dbRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
        '            iscritti = maxIscritti - numIscritti
        '        ElseIf numIscritti = maxIscritti Then
        '            dbRow.Item("CMNT_Iscritti") = -1
        '            iscritti = -1
        '        Else
        '            dbRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
        '            iscritti = maxIscritti - numIscritti
        '        End If
        '    End If
        'Else
        '    dbRow.Item("CMNT_Iscritti") = 0
        'End If
        'TROVO IL RESPONSABILE
        If IsDBNull(dbRow.Item("CMNT_Responsabile")) Then
            CMNT_Responsabile = ""
            If Not IsDBNull(dbRow.Item("AnagraficaCreatore")) Then
                CMNT_Responsabile = " (creata da: " & dbRow.Item("AnagraficaCreatore") & ") "
            End If
        Else
            CMNT_Responsabile = " (" & dbRow.Item("CMNT_Responsabile") & ") "
        End If
        If IsDBNull(dbRow.Item("TPCM_icona")) Then
            img = ""
        Else
            img = dbRow.Item("TPCM_icona")
            img = "./logo/" & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
            ' img = ImageBaseDir & img
        End If
        If IsDBNull(dbRow.Item("CMNT_isIscritto")) Then
            CMNT_isIscritto = True
        Else
            CMNT_isIscritto = dbRow.Item("CMNT_isIscritto")
        End If

        Dim CMNT_Nome, CMNT_NomeVisibile, CMNT_REALpath, CMNT_path As String
        Dim CMNT_IsChiusa As Boolean

        CMNT_Nome = dbRow.Item("CMNT_Nome")
        CMNT_NomeVisibile = CMNT_Nome
        CMNT_IsChiusa = dbRow.Item("CMNT_IsChiusa")
        If dbRow.Item("CMNT_isChiusaForPadre") = True Then
            CMNT_IsChiusa = True
        End If

        If CMNT_id > 0 Then
            CMNT_Nome = CMNT_Nome & CMNT_Responsabile
            CMNT_NomeVisibile = CMNT_Nome
            If CMNT_IsChiusa Then
                CMNT_Nome = CMNT_Nome & Me.GenerateImage(ImageBaseDir & "lucchetto_closed.gif", "Comunità chiusa")
            Else
                CMNT_Nome = CMNT_Nome & Me.GenerateImage(ImageBaseDir & "lucchetto_open.gif", "Comunità aperta")
            End If

            If dbRow.IsNull("CMNT_AnnoAccademico") = False Then
                If dbRow.Item("CMNT_AnnoAccademico") <> "" Then
                    CMNT_Nome = CMNT_Nome & "&nbsp;(" & dbRow.Item("CMNT_AnnoAccademico") & ")&nbsp;"
                End If
            End If
        Else
            CMNT_NomeVisibile = CMNT_Nome
        End If
        CMNT_path = dbRow.Item("CMNT_path")
        CMNT_REALpath = dbRow.Item("CMNT_REALpath")
        'End If
        If CMNT_id > 0 Then
            If CMNT_isIscritto And RLPC_TPRL_id <> -2 And RLPC_TPRL_id <> -3 Then
                'node.ContextMenuName = "AdminEntra"
            Else
                If dbRow.Item("CMNT_Iscritti") = 0 Or dbRow.Item("CMNT_Iscritti") > 0 Then
                    'If IsDate(dbRow.Item("CMNT_dataInizioIscrizione")) Then
                    '    CMNT_dataInizioIscrizione = dbRow.Item("CMNT_dataInizioIscrizione")
                    '    If CMNT_dataInizioIscrizione > Now Then
                    '        ' devo iscrivermi, ma iscrizioni non aperte !
                    '        CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & " (iscrizioni aperte il " & CMNT_dataInizioIscrizione & ")"
                    '        node.ContextMenuName = "AdminIscrivi"
                    '    Else
                    '        If IsDate(dbRow.Item("CMNT_dataFineIscrizione")) Then
                    '            CMNT_dataFineIscrizione = dbRow.Item("CMNT_dataFineIscrizione")
                    '            If CMNT_dataFineIscrizione < Now Then
                    '                CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & "(Iscrizioni chiuse)"
                    '                node.ContextMenuName = "AdminIscrivi"
                    '            Else
                    '                node.ContextMenuName = "AdminIscrivi"
                    '            End If
                    '        Else
                    '            node.ContextMenuName = "AdminIscrivi"
                    '        End If
                    '    End If
                    'Else
                    '    node.ContextMenuName = "AdminIscrivi"
                    'End If
                ElseIf RLPC_TPRL_id = -2 Then
                    'node.ContextMenuName = "AdminIscrivi"
                Else
                    'Non c'è spazio per nuovi iscritti !!!
                    'node.ContextMenuName = "AdminIscrivi"
                    CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & " (n° max iscritti raggiunto)"
                End If
            End If
        End If

        node.Text = CMNT_Nome
        node.Value = CMNT_id & "," & CMNT_REALpath & "," & CMNT_IsChiusa
        node.Expanded = expanded
        node.ImageUrl = img
        node.ToolTip = CMNT_NomeVisibile
        node.Category = CMNT_id


        node.Checkable = True
        If RLPC_TPRL_id = -2 Then
            node.CssClass = "TreeNodeDisabled"
        ElseIf RLPC_TPRL_id = -3 Then
            node.CssClass = "TreeNodeDisabled"
        ElseIf CMNT_isIscritto = False Then
            node.CssClass = "TreeNodeDisabled"
        End If
        node.Checkable = True


        Return node
    End Function
#End Region

    Private Sub BTNfase0ToFase1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNfase0ToFase1.Click
        '   If Session("azione") = "load" Or Session("azione") = "loaded" Then
        Me.PNLsorgente.Visible = False
        Me.PNLutenti.Visible = True
        Me.TBSmenu.Tabs(TabIndex.SceltaUtente).Visible = True
        Me.TBSmenu.SelectedIndex = 1
        Me.RBLutenti.SelectedValue = 0
        Session("azione") = "loadUtenti"
        Me.Bind_DatiPersone(True)
        '   End If
    End Sub
#End Region

#Region "Gestione Fase2"
    Private Sub RBLutenti_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLutenti.SelectedIndexChanged
        Select Case Me.RBLutenti.SelectedValue
            Case 0
                Me.Bind_DatiPersone(True)
                Me.CBLsceltaRuoli.SelectedIndex = -1
            Case 1
                Me.PNLsceltaRuoli.Visible = True
                Me.Bind_DatiSceltaRuoli()
                Me.PNLfiltro.Visible = False
                Me.DGPersone.Visible = False
            Case 2
                Me.PNLfiltro.Visible = False
                Me.DGPersone.Visible = False
                Me.PNLsceltaRuoli.Visible = False
                Me.PNLNoUsers.Visible = False
            Case Else

        End Select
    End Sub



#Region "Filtri_Persone"
    Private Sub Bind_TipoRuolo()
        Dim oDataSet As New DataSet

        Try
            Dim oComunita As New COL_Comunita
            oComunita.Id = Me.HDNsorgente_ID.Value
            oDataSet = oComunita.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForUtenti_NoGuest)

            If oDataSet.Tables(0).Rows.Count > 0 Then
                DDLTipoRuolo.DataSource = oDataSet
                DDLTipoRuolo.DataTextField() = "TPRL_nome"
                DDLTipoRuolo.DataValueField() = "TPRL_id"
                DDLTipoRuolo.DataBind()

                DDLTipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", -1))
            End If
        Catch ex As Exception
            DDLTipoRuolo.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try
    End Sub
    Private Sub Bind_TipoPersona()
        Dim oDataset As DataSet
        Dim oTipoPersona As New COL_TipoPersona

        Try
            oDataset = oTipoPersona.Elenca(Session("LinguaID"), Main.FiltroElencoTipiPersona.WithUserAssociated_NoGuest)
            DDLTipoPersona.Items.Clear()
            If oDataset.Tables(0).Rows.Count > 0 Then
                DDLTipoPersona.DataSource = oDataset
                DDLTipoPersona.DataTextField() = "TPPR_descrizione"
                DDLTipoPersona.DataValueField() = "TPPR_id"
                DDLTipoPersona.DataBind()

                DDLTipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
                Me.DDLTipoPersona.SelectedValue = 1
            Else
                DDLTipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
            End If
        Catch ex As Exception
            DDLTipoPersona.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try

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
        Me.DGPersone.CurrentPageIndex = 0
        Me.Bind_Griglia(True)
    End Sub
    Private Sub DeselezionaLink(ByVal Lettera As String)
        Lettera = CType(CInt(Lettera), Main.FiltroAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControl("LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub
    Private Sub BTNCerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        'eseguire il filtraggio!!!
        If Me.TXBValore.Text <> "" Then
            Bind_Griglia(True)
        End If
    End Sub


    Private Sub DDLTipoRuolo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLTipoRuolo.SelectedIndexChanged
        DGPersone.CurrentPageIndex = 0
        ViewState("RipristinaCheck") = "si"
        If Me.TXBValore.Text <> "" Then
            Bind_Griglia(True)
        Else
            Bind_Griglia()
        End If

    End Sub
    Private Sub DDLTipoPersona_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoPersona.SelectedIndexChanged
        DGPersone.CurrentPageIndex = 0
        ViewState("RipristinaCheck") = "si"
        If Me.TXBValore.Text <> "" Then
            Bind_Griglia(True)
        Else
            Bind_Griglia()
        End If
    End Sub
#End Region

    Private Sub Bind_DatiPersone(ByVal setUp As Boolean)
        Dim oStringaSorgente, oStringaDestinatario As String

        oStringaSorgente = Me.HDN_valoreSorgente.Value
        oStringaDestinatario = Me.HDN_valoreDestinatario.Value
        If oStringaSorgente = "" Or oStringaDestinatario = "" Then
            Me.RBLutenti.Enabled = False
        Else
            Dim oComunita As New COL_Comunita
            Dim oComunitaDestinatari As New COL_Comunita
            Dim stringaIDsorgente(), stringaIDdestinatario() As String

            Me.RBLutenti.Enabled = True

            If setUp Then
                stringaIDsorgente = Me.HDN_valoreSorgente.Value.Split(",")
                stringaIDdestinatario = Me.HDN_valoreDestinatario.Value.Split(",")
                Me.HDNsorgente_ID.Value = stringaIDsorgente(0)
                Me.HDNdestinazione_ID.Value = stringaIDdestinatario(0)

                oComunita.Id = Me.HDNsorgente_ID.Value
                oComunita.Estrai()
                Me.LBsorgenteCmnt.Text = oComunita.Nome

                oComunitaDestinatari.Id = Me.HDNdestinazione_ID.Value
                oComunitaDestinatari.Estrai()
                Me.LBdestinazioneCmnt.Text = oComunitaDestinatari.Nome

                'If oComunita.IdPadre = 0 And oComunita.TipoComunita.ID = 0 Then
                '    Bind_TipoPersona()
                '    Me.DDLTipoPersona.Visible = True
                '    Me.DDLTipoRuolo.Visible = False
                '    Me.LBtipoRuolo.Visible = False
                '    Me.LBtipoPersona.Visible = True
                'Else
                Me.DDLTipoPersona.Visible = False
                Me.DDLTipoRuolo.Visible = True
                Me.LBtipoRuolo.Visible = True
                Me.LBtipoPersona.Visible = False
                Me.Bind_TipoRuolo()
                'End If
                Me.ViewState("intCurPage") = 0
                Me.ViewState("intAnagrafica") = "-1"
                Me.LKBtutti.CssClass = "lettera_Selezionata"

                If Me.RBLutenti.SelectedValue = 0 Then
                    Me.PNLfiltro.Visible = True
                    Me.DGPersone.Visible = True
                    Me.PNLsceltaRuoli.Visible = False
                    Me.Bind_Griglia(True)
                ElseIf Me.RBLutenti.SelectedValue = 1 Then
                    Me.PNLsceltaRuoli.Visible = True
                    Me.PNLfiltro.Visible = False
                    Me.DGPersone.Visible = False
                End If
            Else
                If Me.RBLutenti.SelectedValue = 0 Then
                    Me.PNLfiltro.Visible = True
                    Me.DGPersone.Visible = True
                    Me.PNLsceltaRuoli.Visible = False
                    Me.Bind_Griglia(False)
                ElseIf Me.RBLutenti.SelectedValue = 1 Then
                    Me.PNLsceltaRuoli.Visible = True
                    Me.PNLfiltro.Visible = False
                    Me.DGPersone.Visible = False
                End If
            End If
        End If

    End Sub
    Private Sub Bind_Griglia(Optional ByVal Filtraggio As Boolean = False)
        Dim CMNT_id_passato As Integer
        Dim dsTable As New DataSet
        Dim oComunita As New COL_Comunita
        Dim Valore As String

        CMNT_id_passato = Me.HDNsorgente_ID.Value
        oComunita.Id = Me.HDNdestinazione_ID.Value

        Valore = Me.TXBValore.Text

        Dim oFiltroAnagrafica As Main.FiltroAnagrafica
        Dim oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti
        Try
            If Valore = "" Then
                If Me.ViewState("intAnagrafica") = "" Then
                    oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Else
                    oFiltroAnagrafica = CType(Me.ViewState("intAnagrafica"), Main.FiltroAnagrafica)
                End If
            Else
                If Filtraggio = True Then
                    Select Case Me.DDLTipoRicerca.SelectedItem.Value
                        Case Main.FiltroRicercaAnagrafica.nome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.nome
                        Case Main.FiltroRicercaAnagrafica.dataNascita
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.dataNascita
                        Case Main.FiltroRicercaAnagrafica.cognome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.cognome

                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                        Case Main.FiltroRicercaAnagrafica.nomeCognome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.nomeCognome
                        Case Main.FiltroRicercaAnagrafica.matricola
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.matricola

                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
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

                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                        Case Else
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.tutti
                    End Select
                Else
                    oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                End If
            End If
        Catch ex As Exception
            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
        End Try



        Try
            oComunita.Estrai()
            'If oComunita.IdPadre = 0 And oComunita.TipoComunita.ID = 0 Then 'vuol dire che è un'istituzione
            '    Dim oOrganizzazione As New COL_Organizzazione
            '    Dim ISTT_ID As Integer
            '    '  oOrganizzazione.Id = Session("ORGN_id")
            '    ISTT_ID = Session("ISTT_ID")
            '    oOrganizzazione.Id = oComunita.GetOrganizzazioneID()
            '    dsTable = oOrganizzazione.ElencaNonIscrittiByIstituzione(ISTT_ID, Me.DDLTipoPersona.SelectedItem.Value, Valore, oFiltroAnagrafica)
            '    DGPersone.Columns(6).Visible = False
            '    DGPersone.Columns(7).Visible = False
            '    DGPersone.Columns(9).Visible = True

            'Else
            DGPersone.Columns(10).Visible = False
            dsTable = oComunita.ElencaNonIscritti_MaIscrittiACmntPassata(Session("LinguaID"), Me.DDLTipoRuolo.SelectedItem.Value, CMNT_id_passato, Valore, oFiltroAnagrafica, Main.FiltroUtenti.NoPassantiNoCreatori, oFiltroRicerca)
            If Me.DDLTipoRuolo.SelectedValue <> -1 Then
                DGPersone.Columns(8).Visible = False
            Else
                DGPersone.Columns(8).Visible = True
            End If
            'End If
            dsTable.Tables(0).Columns.Add(New DataColumn("oCheckAbilitato"))
            dsTable.Tables(0).Columns.Add(New DataColumn("oPRSN_datanascita"))
            Dim i As Integer
            If dsTable.Tables(0).Rows.Count = 0 Then
                'nascondo la DG e visualizzo messaggio
                Me.DGPersone.Visible = False
                Me.PNLNoUsers.Visible = True
                Me.LBMessaggio.Text = "Nessun utente disponibile"
            Else
                For i = 0 To dsTable.Tables(0).Rows.Count - 1
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
                Next

                'ordinamento delle colonne e databind della griglia

                Me.DGPersone.Visible = True
                ' Me.DGiscritti.CurrentPageIndex = 0
                Dim oDataview As DataView
                oDataview = dsTable.Tables(0).DefaultView
                If ViewState("SortExspression") = "" Then
                    ViewState("SortExspression") = "PRSN_Anagrafica"
                    ViewState("SortDirection") = "asc"
                End If
                oDataview.Sort = ViewState("SortExspression") & " " & ViewState("SortDirection")
                Me.DGPersone.DataSource = oDataview
                Me.DGPersone.PageSize = Me.DDLNumeroRecord.SelectedValue
                Me.DGPersone.DataBind()
                Me.PNLNoUsers.Visible = False
            End If
        Catch ex As Exception
            'nascondo la DG e visualizzo messaggio
            Me.DGPersone.Visible = False
            Me.PNLNoUsers.Visible = True
            Me.LBMessaggio.Text = "Spiacenti, si è verificato un Errore"
        End Try
    End Sub

#Region "Griglia Persone"
    Sub DGPersone_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGPersone.PageIndexChanged
        DGPersone.CurrentPageIndex = e.NewPageIndex
        ViewState("RipristinaCheck") = "si"
        Me.Bind_Griglia()
    End Sub
    Private Sub DGPersone_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGPersone.ItemCreated
        Dim i As Integer

        If e.Item.ItemType = ListItemType.Header Then
            Dim oSortExspression, oSortDirection, oText As String
            oSortExspression = ViewState("SortExspression")
            oSortDirection = ViewState("SortDirection")
            If oSortDirection = "asc" Then
                oText = " 5"
            Else
                oText = " 6"
            End If
            For i = 0 To sender.columns.count - 1
                If sender.columns(i).visible = True And sender.columns(i).SortExpression <> "" Then
                    Dim oWebControl As WebControl
                    Dim oCell As New TableCell
                    Dim oLabel As New System.Web.UI.WebControls.Label

                    oCell = e.Item.Cells(i)
                    If oSortExspression = sender.columns(i).SortExpression Then
                        oLabel.Text = oText
                        Try
                            oWebControl = oCell.Controls(0)
                            Dim oLinkbutton As LinkButton
                            oLinkbutton = oWebControl

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

                            oLinkbutton.Font.Name = "webdings"
                            oLinkbutton.Font.Size = FontUnit.XSmall
                            oLabel.Text = oLinkbutton.Text & " "
                            oLinkbutton.Text = oText
                            oCell.Controls.AddAt(0, oLabel)
                        Catch ex As Exception
                            oLabel.Font.Name = "webdings"
                            oLabel.Font.Size = FontUnit.XSmall
                            oLabel.Text = "&nbsp;&nbsp;"
                            oCell.Controls.Add(oLabel)
                        End Try
                    Else
                        Try
                            oWebControl = oCell.Controls(0)
                            Dim oLinkbutton As LinkButton
                            oLinkbutton = oWebControl

                            oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                            oLinkbutton.Attributes.Add("onfocus", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                            oLinkbutton.Attributes.Add("onmouseover", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                            oLinkbutton.Attributes.Add("onclick", "window.status='Ordina per <" & oLinkbutton.Text & ">';return true;")
                        Catch ex As Exception

                        End Try
                        oLabel.Font.Name = "webdings"
                        oLabel.Font.Size = FontUnit.XSmall
                        oLabel.Text = "&nbsp;&nbsp;"
                        oCell.Controls.Add(oLabel)
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

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Try
                If CBool(e.Item.DataItem("RLPC_Attivato")) = False Then
                    e.Item.CssClass = "Righe_Disattivate"
                ElseIf CBool(e.Item.DataItem("RLPC_Abilitato")) = False Then
                    e.Item.CssClass = "Righe_Disabilitate"
                ElseIf e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "Righe_Alternate"
                Else
                    e.Item.CssClass = "Righe_Normali"
                End If
            Catch ex As Exception
                'If e.Item.ItemType = ListItemType.AlternatingItem Then
                '    e.Item.CssClass = "Righe_Alternate"
                'Else
                'End If
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
                'in base al tipo di utente decido la dimensione della finestra di popup
                Select Case TPPR_id
                    Case Main.TipoPersonaStandard.Studente
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Docente
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','480','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Tutor
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','480','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Esterno
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Amministrativo
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.SysAdmin
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Main.TipoPersonaStandard.Dottorando
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                    Case Else
                        oImagebutton.Attributes.Add("onClick", "OpenWin('" & i_link2 & "','480','450','no','yes');return false;")
                End Select
                oImagebutton.ToolTip = "Info Persona"
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGPersone.SortCommand
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
        End If
        Bind_Griglia()
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        DGPersone.CurrentPageIndex = 0 'azzero indice paginazione
        ViewState("RipristinaCheck") = "si"
        Bind_Griglia()
    End Sub
    Private Sub DGPersone_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGPersone.ItemCommand
        If e.CommandName = "Aggiungi" Then
            Session("Azione") = "SceltaRuoli"
            Me.HDabilitato.Value = "," & CInt(DGPersone.DataKeys.Item(e.Item.ItemIndex)) & ","

            Me.TBSmenu.Tabs(TabIndex.SceltaRuolo).Visible = True
            Me.TBSmenu.SelectedIndex = 2
            Me.PNLutenti.Visible = False
            Me.PNLruoliDestinazione.Visible = True
            Me.Bind_Ruoli_SorgenteDestinazione()
        End If
    End Sub

#End Region

    Private Sub Bind_DatiSceltaRuoli()
        Dim oDataset As New DataSet

        Me.CBLsceltaRuoli.Items.Clear()
        Try
            Dim oComunita As New COL_Comunita
            Dim i, totale, TotaleUtenti, TotaleAttivati, TotaleAbilitati As Integer
            oComunita.Id = Me.HDNsorgente_ID.Value

            oDataset = COL_Comunita.Ruoli_EsportaNoIscritti(Me.HDNsorgente_ID.Value, Me.HDNdestinazione_ID.Value, Main.FiltroAbilitazione.Attivato, Main.FiltroUtenti.NoPassantiNoCreatori)

            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    Dim oListItem As New ListItem
                    oRow = oDataset.Tables(0).Rows(i)

                    oListItem.Value = oRow.Item("TPRL_id") & "," & oRow.Item("TPRL_Gerarchia")

                    TotaleUtenti = oRow.Item("TotaleUtenti")
                    TotaleAttivati = oRow.Item("TotaleAttivati")
                    TotaleAbilitati = oRow.Item("TotaleAbilitati")
                    If TotaleUtenti = TotaleAttivati And TotaleAttivati = TotaleAbilitati Then
                        oListItem.Text = oRow.Item("TPRL_nome") & " (n° utenti=" & TotaleUtenti & ")"
                    ElseIf TotaleUtenti = TotaleAttivati And TotaleAttivati > TotaleAbilitati Then
                        oListItem.Text = oRow.Item("TPRL_nome") & " (n° utenti=" & TotaleUtenti & " di cui "
                        If TotaleAttivati - TotaleAbilitati = 1 Then
                            oListItem.Text = oListItem.Text & "uno non abilitato)"
                        Else
                            oListItem.Text = oListItem.Text & TotaleAttivati - TotaleAbilitati & " non abilitati)"
                        End If
                    Else
                        oListItem.Text = oRow.Item("TPRL_nome") & " (n° utenti=" & TotaleUtenti & " di cui "

                        If TotaleUtenti - TotaleAbilitati = 1 Then
                            oListItem.Text = oListItem.Text & "uno non abilitato "
                        ElseIf TotaleUtenti - TotaleAbilitati > 1 Then
                            oListItem.Text = oListItem.Text & TotaleUtenti - TotaleAbilitati & " non abilitati "
                        End If

                        If TotaleUtenti - TotaleAttivati = 1 Then
                            oListItem.Text = oListItem.Text & " uno non attivato)"
                        ElseIf TotaleUtenti - TotaleAttivati > 1 Then
                            oListItem.Text = oListItem.Text & " " & TotaleUtenti - TotaleAttivati & " non attivati)"
                        Else
                            oListItem.Text = oListItem.Text & ")"
                        End If
                    End If
                    Me.CBLsceltaRuoli.Items.Add(oListItem)
                Next
                Me.LBMessaggio.Visible = False
                Me.PNLNoUsers.Visible = False
            Else
                Me.PNLsceltaRuoli.Visible = False
                Me.BTNfase1ToFase2.Enabled = False
                Me.PNLNoUsers.Visible = True
                Me.LBMessaggio.Visible = True
                Me.LBMessaggio.Text = "Spiacente, tutti gli utenti della comunità Sorgente risultano iscritti alla comunità Destinazione !"
            End If
        Catch ex As Exception
            Me.PNLsceltaRuoli.Visible = False
            Me.BTNfase1ToFase2.Enabled = False
            Me.PNLNoUsers.Visible = True
            Me.LBMessaggio.Visible = True
            Me.LBMessaggio.Text = "Spiacente, tutti gli utenti della comunità Sorgente risultano iscritti alla comunità Destinazione !"
        End Try
    End Sub

    Private Sub BTNfase1ToFase2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNfase1ToFase2.Click
        If Me.RBLutenti.SelectedValue = 1 And Me.CBLsceltaRuoli.SelectedIndex = -1 Then
            Exit Sub
        Else
            Me.TBSmenu.Tabs(TabIndex.SceltaRuolo).Visible = True
            Me.TBSmenu.SelectedIndex = 2
            Me.PNLutenti.Visible = False
            Me.PNLruoliDestinazione.Visible = True
            Me.Bind_Ruoli_SorgenteDestinazione()
            Session("Azione") = "SceltaRuoli"
        End If
    End Sub
#End Region


#Region "Fase Scelta Ruoli Destinazione"

#Region "Bind_Dati"
    Private Sub Bind_Ruoli_SorgenteDestinazione()
        Dim oComunita As New COL_Comunita
        Dim oComunitaDest As New COL_Comunita
        Dim isUnico As Boolean = False
        Try
            oComunita.Id = Me.HDNsorgente_ID.Value
            oComunita.Estrai()
            oComunitaDest.Id = Me.HDNdestinazione_ID.Value
            oComunitaDest.Estrai()

            Dim oDatasetSorgente As New DataSet
            Dim oDatasetDestinatario As New DataSet

            If Me.RBLutenti.SelectedValue = 0 Then
                Dim ElencoIDutenti() As String


                ElencoIDutenti = Me.HDabilitato.Value.Split(",")
                If ElencoIDutenti.Length - 2 = 1 Then
                    Me.LBtitoloRuoli.Text = "Definizione ruoli di destinazione per l'utente selezionato"
                    isUnico = True
                Else
                    Me.LBtitoloRuoli.Text = "Definizione ruoli di destinazione per gli utenti selezionati"
                End If
            Else
                Me.LBtitoloRuoli.Text = "Definizione ruoli di destinazione"
            End If
            If isUnico Then
                ' CVodice se seleziono un'uncio utente 
                Me.Bind_Ruoli_ForUser()
            ElseIf Me.RBLutenti.SelectedValue = 1 Then
                Me.Bind_Ruoli_Selezionati()
            Else
                Me.Bind_Ruoli_Completi()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_Ruoli_ForUser()
        Dim totale, TPRLsorgente_ID, TPRL_Gerarchia As Integer
        Dim oComunita As New COL_Comunita

        Try
            oComunita.Id = Me.HDNsorgente_ID.Value
            oComunita.Estrai()

            Dim oRuoloComunita As New COL_RuoloPersonaComunita
            Dim ElencoIDutenti() As String

            Dim oDatasetSorgente As New DataSet
            oDatasetSorgente.Tables.Add(New DataTable("Tipo_Ruolo"))
            oDatasetSorgente.Tables(0).Columns.Add(New DataColumn("TPRL_ID"))
            oDatasetSorgente.Tables(0).Columns.Add(New DataColumn("TPRL_nome"))
            oDatasetSorgente.Tables(0).Columns.Add(New DataColumn("TPRL_Gerarchia"))

            ElencoIDutenti = Me.HDabilitato.Value.Split(",")
            oRuoloComunita.EstraiByLinguaDefault(Me.HDNsorgente_ID.Value, ElencoIDutenti(1))
            If oRuoloComunita.Errore = Errori_Db.None Then
                Dim oDataRow As DataRow

                TPRL_Gerarchia = oRuoloComunita.TipoRuolo.Gerarchia
                TPRLsorgente_ID = oRuoloComunita.TipoRuolo.Id

                oDataRow = oDatasetSorgente.Tables(0).NewRow
                oDataRow.Item("TPRL_Gerarchia") = TPRL_Gerarchia
                oDataRow.Item("TPRL_ID") = TPRLsorgente_ID
                oDataRow.Item("TPRL_nome") = oRuoloComunita.TipoRuolo.Nome
                oDatasetSorgente.Tables(0).Rows.Add(oDataRow)
            End If

            totale = oDatasetSorgente.Tables(0).Rows.Count
            If totale > 0 Then
                Me.DGruoli.DataSource = oDatasetSorgente
                Me.DGruoli.DataBind()
                Me.BTNimporta.Enabled = True
            Else
                Me.BTNimporta.Enabled = False
            End If
        Catch ex As Exception
            Me.BTNimporta.Enabled = False
        End Try
    End Sub
    Private Sub Bind_Ruoli_Selezionati()
        Dim i, totale As Integer
        Dim oComunita As New COL_Comunita

        Try
            Dim oDatasetSorgente As New DataSet
            oComunita.Id = Me.HDNsorgente_ID.Value
            oComunita.Estrai()

            totale = Me.CBLsceltaRuoli.Items.Count
            Dim oStringa() As String

            oDatasetSorgente.Tables.Add(New DataTable("Tipo_Ruolo"))
            oDatasetSorgente.Tables(0).Columns.Add(New DataColumn("TPRL_ID"))
            oDatasetSorgente.Tables(0).Columns.Add(New DataColumn("TPRL_nome"))
            oDatasetSorgente.Tables(0).Columns.Add(New DataColumn("TPRL_Gerarchia"))

            For i = 0 To totale - 1
                oStringa = Me.CBLsceltaRuoli.Items(i).Value.Split(",")
                If Me.CBLsceltaRuoli.Items(i).Selected Then
                    Dim oDataRow As DataRow

                    oDataRow = oDatasetSorgente.Tables(0).NewRow
                    oDataRow.Item("TPRL_Gerarchia") = oStringa(1)
                    oDataRow.Item("TPRL_ID") = oStringa(0)
                    oDataRow.Item("TPRL_nome") = Me.CBLsceltaRuoli.Items(i).Text
                    oDatasetSorgente.Tables(0).Rows.Add(oDataRow)
                End If
            Next
            totale = oDatasetSorgente.Tables(0).Rows.Count
            If totale > 0 Then
                Me.DGruoli.DataSource = oDatasetSorgente
                Me.DGruoli.DataBind()
                Me.BTNimporta.Enabled = True
            Else
                Me.BTNimporta.Enabled = False
            End If
        Catch ex As Exception
            Me.BTNimporta.Enabled = False
        End Try
    End Sub
    Private Sub Bind_Ruoli_Completi()
        Dim totale As Integer
        Dim oComunita As New COL_Comunita
        Dim oDatasetSorgente As New DataSet

        Try
            oComunita.Id = Me.HDNsorgente_ID.Value
            oComunita.Estrai()

            oDatasetSorgente = COL_Comunita.Ruoli_EsportaNoIscritti(Me.HDNsorgente_ID.Value, Me.HDNdestinazione_ID.Value, Main.FiltroAbilitazione.Attivato, Main.FiltroUtenti.NoPassantiNoCreatori)

            totale = oDatasetSorgente.Tables(0).Rows.Count
            If totale > 0 Then
                Me.DGruoli.DataSource = oDatasetSorgente
                Me.DGruoli.DataBind()
                Me.BTNimporta.Enabled = True
            Else
                Me.BTNimporta.Enabled = False
            End If
        Catch ex As Exception
            Me.BTNimporta.Enabled = False
        End Try
    End Sub

    Private Sub DGruoli_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGruoli.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDDLruoloDest As DropDownList
            Dim Indice As Integer
            oDDLruoloDest = e.Item.FindControl("DDLruoloDest")
            If Not IsNothing(oDDLruoloDest) Then
                Try
                    Indice = Me.Bind_TipoRuoloImport(oDDLruoloDest, e.Item.DataItem("TPRL_ID"), e.Item.DataItem("TPRL_Gerarchia"))
                Catch ex As Exception

                End Try
                oDDLruoloDest.SelectedIndex = Indice
            End If
        End If
    End Sub
    Private Function Bind_TipoRuoloImport(ByRef oDDLruoloDest As DropDownList, ByVal TPRLsorgente_ID As Integer, ByVal TPRL_Gerarchia As Integer) As Integer
        Dim TPRLdestinatario_ID As Integer
        Dim oComunitaDest As New COL_Comunita

        Try
            Dim oDatasetDestinatario As New DataSet
            oComunitaDest.Id = Me.HDNdestinazione_ID.Value
            oComunitaDest.Estrai()
            oDatasetDestinatario = oComunitaDest.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForTipoComunita_NoGuest, Main.FiltroUtenti.NoPassantiNoCreatori)

            oDDLruoloDest.DataSource = oDatasetDestinatario
            oDDLruoloDest.DataTextField = "TPRL_Nome"
            oDDLruoloDest.DataValueField = "TPRL_ID"
            oDDLruoloDest.DataBind()

            Try
                oDDLruoloDest.SelectedValue = TPRLsorgente_ID
            Catch ex As Exception
                ' Devo trovare un ruolo con gerarchia inferiore ?
                Dim oDataview As New DataView
                oDataview = oDatasetDestinatario.Tables(0).DefaultView
                oDataview.RowFilter = "TPRL_Gerarchia < " & TPRL_Gerarchia
                If oDataview.Count > 0 Then
                    oDataview.Sort = "TPRL_Gerarchia DESC"
                    TPRLdestinatario_ID = oDataview.Item(0).Item("TPRL_Id")
                    oDDLruoloDest.SelectedValue = TPRLdestinatario_ID
                Else
                    oDataview.RowFilter = "TPRL_Gerarchia = " & TPRL_Gerarchia
                    If oDataview.Count > 0 Then
                        oDataview.Sort = "TPRL_Gerarchia DESC"
                        TPRLdestinatario_ID = oDataview.Item(0).Item("TPRL_Id")
                        oDDLruoloDest.SelectedValue = TPRLdestinatario_ID
                    Else
                        oDataview.RowFilter = "TPRL_Gerarchia > " & TPRL_Gerarchia
                        oDataview.Sort = "TPRL_Gerarchia DESC"
                        If oDataview.Count > 0 Then
                            TPRLdestinatario_ID = oDataview.Item(0).Item("TPRL_Id")
                            oDDLruoloDest.SelectedValue = TPRLdestinatario_ID
                        End If
                    End If
                End If
            End Try
        Catch ex As Exception
            Return 0
        End Try
        Return oDDLruoloDest.SelectedIndex
    End Function

#End Region


    Private Sub BTNimporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNimporta.Click
        Dim Imported As Integer
        Imported = 0
        If Session("azione") <> "importUser" Then
            Session("azione") = "importUser"
            If Me.RBLutenti.SelectedValue = 0 Then
                Imported = Me.Import_UtenteSingolo_Selezionati(Me.HDabilitato.Value)
            ElseIf Me.RBLutenti.SelectedValue > 0 Then
                Imported = Me.Import_Ruoli()
            End If

            Me.TBSmenu.Tabs(TabIndex.Finale).Visible = True
            Me.TBSmenu.Tabs(TabIndex.SceltaRuolo).Visible = False
            Me.TBSmenu.Tabs(TabIndex.SorgenteDestinazione).Visible = False
            Me.TBSmenu.Tabs(TabIndex.SceltaUtente).Visible = False
            Me.TBSmenu.SelectedIndex = 3
            Me.PNLruoliDestinazione.Visible = False
            Me.PNLsceltaRuoli.Visible = False
            Me.PNLutenti.Visible = False
            Me.PNLsorgente.Visible = False
            Me.PNLfinale.Visible = True

            If Imported > 0 Then
                Me.LBfinale.Text = "Sono stati importati tutti gli utenti richiesti.<br> Per importare altri utenti in altre comunità premere il pulsante sottostante."
            Else
                Me.LBfinale.Text = "Non stati importati gli utenti richiesti, per riprovare premere il pulsante sottostante."
            End If
        End If
    End Sub

    'Importo gli utenti:
    '1) individuo il ruolo che hanno, prendo la trasformazione che ho deciso
    Private Function Import_UtenteSingolo_Selezionati(ByVal ListaIscritti As String) As Integer
        Dim CMNTsorgente_ID, CMNTdestinatario_ID, iResponse As Integer
        Dim CMNTpath_Sorgente, CMNTpath_Destinazione, CMNT_PathPassanti, RuoliImport As String
        Dim stringaSorgente(), stringaDestinazione() As String
        Dim ElencoIDutenti(), ElencoIDcomunita() As String
        Dim isIscritto, isAttivo, isAbilitato As Boolean


        iResponse = 0
        If Session("azione") = "importUser" Then
            Try
                Dim i, totaleC, totaleI, totalePadri, PRSN_ID, j As Integer
                Dim oComunitaDest As New COL_Comunita
                Dim oPersona As New COL_Persona

                stringaSorgente = Me.HDN_valoreSorgente.Value.Split(",")
                stringaDestinazione = Me.HDN_valoreDestinatario.Value.Split(",")
                CMNTsorgente_ID = Me.HDNsorgente_ID.Value
                CMNTdestinatario_ID = Me.HDNdestinazione_ID.Value
                CMNTpath_Sorgente = stringaSorgente(1)
                CMNTpath_Destinazione = stringaDestinazione(1)
                CMNT_PathPassanti = CMNTpath_Destinazione

                ElencoIDcomunita = CMNTpath_Destinazione.Split(".")
                ElencoIDutenti = ListaIscritti.Split(",")
                totaleI = ElencoIDutenti.Length - 2

                oComunitaDest.Id = CMNTdestinatario_ID
                For i = 1 To totaleI
                    Dim oDataset As New DataSet
                    isIscritto = False
                    PRSN_ID = ElencoIDutenti(i)
                    totaleC = UBound(ElencoIDcomunita) - 1


                    ' Recupero i ruoli e le loro definizioni nella comunità destinazione
                    'recupero le info dalla comunità sorgente
                    RuoliImport = Me.DefineExportRoles
                    Dim oRuoloComunita As New COL_RuoloPersonaComunita
                    Dim TPRL_ID As Integer
                    oRuoloComunita.EstraiByLinguaDefault(CMNTsorgente_ID, PRSN_ID)
                    If oRuoloComunita.Errore = Errori_Db.None Then
                        TPRL_ID = Me.ReturnExportRoles(RuoliImport, "," & oRuoloComunita.TipoRuolo.Id & "-")
                        isAttivo = oRuoloComunita.Attivato
                        isAbilitato = oRuoloComunita.Abilitato
                        TPRL_ID = VerificaRuolo(CMNTdestinatario_ID, PRSN_ID, TPRL_ID)

                        If totaleC = 1 Then
                            'oComunitaDest.IscriviUtente(PRSN_ID, TPRL_ID, , , False)
                            'Me.AggiornaXML(CMNTdestinatario_ID, PRSN_ID, CMNTpath_Destinazione)
                            isIscritto = True
                            iResponse += 1
                        Else
                            For j = totaleC To 0 Step -1
                                If IsNumeric(ElencoIDcomunita(j)) Then
                                    oDataset = oPersona.VerificaIscrizioneAPadri(ElencoIDcomunita(j), PRSN_ID)
                                    totalePadri = oDataset.Tables(0).Rows.Count
                                    If totalePadri > 0 Then
                                        isIscritto = True
                                        iResponse += 1
                                        'oComunitaDest.IscriviUtente(PRSN_ID, TPRL_ID, isAttivo, isAbilitato, False)
                                        'Me.AggiornaXML(CMNTdestinatario_ID, PRSN_ID, CMNTpath_Destinazione)
                                        Exit For
                                    Else
                                        Dim oComunita2 As New COL_Comunita
                                        Dim CMNT_Id2 As Integer

                                        CMNT_Id2 = ElencoIDcomunita(j - 1)
                                        oComunita2.Id = CMNT_Id2
                                        If j - 1 > 0 Then
                                            'oComunita2.IscriviUtenteComePassante(PRSN_ID)
                                            CMNT_PathPassanti = Left(CMNT_PathPassanti, InStr(CMNT_PathPassanti, "." & ElencoIDcomunita(j - 1) & "."))
                                            CMNT_PathPassanti = CMNT_PathPassanti & ElencoIDcomunita(j - 1) & "."
                                            'Me.AggiornaXML(CMNT_Id2, PRSN_ID, CMNT_PathPassanti)
                                        End If
                                    End If
                                End If
                            Next
                        End If

                        If isIscritto = False Then
                            iResponse += 1
                            'oComunitaDest.IscriviUtente(PRSN_ID, TPRL_ID, isAttivo, isAbilitato, False)
                            'Me.AggiornaXML(CMNTdestinatario_ID, PRSN_ID, CMNTpath_Destinazione)
                        End If
                    End If
                Next
            Catch ex As Exception

            End Try
        End If
        Return iResponse
    End Function

    Private Function Import_Ruoli() As Integer
        Dim i, totale, iResponse As Integer

        totale = Me.DGruoli.Items.Count
        iResponse = 0
        For i = 0 To totale - 1
            Dim oDGitem As DataGridItem
            Dim oDDLruoloDest As New System.Web.UI.WebControls.DropDownList
            Dim TPRL_ID, TPRLdest_ID As Integer
            oDGitem = Me.DGruoli.Items(i)

            Try
                TPRL_ID = CInt(oDGitem.Cells(0).Text)
                oDDLruoloDest = oDGitem.Cells(3).FindControl("DDLruoloDest")
                If Not IsNothing(oDDLruoloDest) Then
                    TPRLdest_ID = oDDLruoloDest.SelectedValue
                End If
                If TPRL_ID <> 0 And TPRLdest_ID <> 0 Then
                    ' Faccio l'import per il ruolo selezionato

                    Dim CMNTsorgente_ID, CMNTdestinatario_ID, IdPassante As Integer
                    Dim CMNTpath_Sorgente, CMNTpath_Destinazione, CMNT_PathPassanti As String
                    Dim stringaSorgente(), stringaDestinazione(), stringaPassante() As String
                    Dim ElencoIDcomunita() As String
                    Dim isIscritti As Boolean


                    If Session("azione") = "importUser" Then
                        Try
                            Dim totaleComunita, j As Integer
                            Dim oComunitaDest As New COL_Comunita
                            Dim oPersona As New COL_Persona

                            stringaSorgente = Me.HDN_valoreSorgente.Value.Split(",")
                            stringaDestinazione = Me.HDN_valoreDestinatario.Value.Split(",")
                            CMNTsorgente_ID = Me.HDNsorgente_ID.Value
                            CMNTdestinatario_ID = Me.HDNdestinazione_ID.Value
                            CMNTpath_Sorgente = stringaSorgente(1)
                            CMNTpath_Destinazione = stringaDestinazione(1)
                            CMNT_PathPassanti = CMNTpath_Destinazione

                            ElencoIDcomunita = CMNTpath_Destinazione.Split(".")
                            oComunitaDest.Id = CMNTdestinatario_ID


                            Dim oDataset As New DataSet
                            totaleComunita = UBound(ElencoIDcomunita) - 2
                            isIscritti = False

                            If totaleComunita = 1 Then
                                iResponse = COL_Comunita.ImportaNoIscrittiByRuolo(CMNTsorgente_ID, CMNTdestinatario_ID, TPRL_ID, TPRLdest_ID, Main.FiltroAbilitazione.Attivato)
                                isIscritti = True
                            Else
                                Dim oNode As Telerik.WebControls.RadTreeNode
                                oNode = Me.RDTdestinatario.FindNodeByValue(Me.HDN_valoreDestinatario.Value)
                                For j = totaleComunita To 0 Step -1
                                    If IsNumeric(ElencoIDcomunita(j)) Then
                                        Dim isChiusaForPadre As Boolean = False
                                        oNode = oNode.Parent
                                        stringaPassante = oNode.Value.Split(",")
                                        isChiusaForPadre = CBool(stringaPassante(2))
                                        IdPassante = ElencoIDcomunita(j)
                                        COL_Comunita.ImportaComePassanteNoIscrittiByRuolo(CMNTsorgente_ID, CMNTdestinatario_ID, TPRL_ID, IdPassante, isChiusaForPadre, Main.FiltroAbilitazione.Attivato)
                                    End If
                                Next
                            End If

                            If isIscritti = False Then
                                iResponse = COL_Comunita.ImportaNoIscrittiByRuolo(CMNTsorgente_ID, CMNTdestinatario_ID, TPRL_ID, TPRLdest_ID, Main.FiltroAbilitazione.Attivato)
                            End If
                        Catch ex As Exception

                        End Try
                    End If

                End If
            Catch ex As Exception

            End Try
        Next
        Return iResponse
    End Function

    Private Function DefineExportRoles() As String
        Dim i, totale As Integer
        Dim stringaRuoli As String = ""

        totale = Me.DGruoli.Items.Count
        For i = 0 To totale - 1
            Dim oDGitem As DataGridItem
            Dim oDDLruoloDest As New System.Web.UI.WebControls.DropDownList
            Dim TPRL_ID, TPRLdest_ID As Integer
            Dim valore As String = ""

            oDGitem = Me.DGruoli.Items(i)

            Try
                TPRL_ID = CInt(oDGitem.Cells(0).Text)
                oDDLruoloDest = oDGitem.Cells(3).FindControl("DDLruoloDest")
                If Not IsNothing(oDDLruoloDest) Then
                    TPRLdest_ID = oDDLruoloDest.SelectedValue
                End If
                If TPRL_ID <> 0 And TPRLdest_ID <> 0 Then
                    If stringaRuoli = "" Then
                        stringaRuoli = "," & TPRL_ID & "-" & TPRLdest_ID & ","
                    Else
                        stringaRuoli = stringaRuoli & TPRL_ID & "-" & TPRLdest_ID & ","
                    End If
                End If
            Catch ex As Exception
                stringaRuoli = ""
            End Try
        Next
        Return stringaRuoli
    End Function
    Private Function ReturnExportRoles(ByVal stringaRuoli As String, ByVal ruolo As String) As Integer
        Dim pos, Init, posFinale As Integer
        Dim stringa As String

        Try
            pos = InStr(stringaRuoli, ruolo)
            Init = pos + Len(ruolo)
            posFinale = InStr(Init, stringaRuoli, ",")
            stringa = Mid(stringaRuoli, Init, posFinale - Init)
            Return CInt(stringa)
        Catch ex As Exception
            Return 0
        End Try
    End Function
#End Region

#Region "Gestione Tab"
    Private Sub TBSmenu_TabClick(ByVal sender As Object, ByVal args As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        Select Case Me.TBSmenu.SelectedIndex
            Case TabIndex.SorgenteDestinazione
                Me.PNLsorgente.Visible = True
                Me.PNLutenti.Visible = False
                Me.PNLruoliDestinazione.Visible = False
                Session("azione") = "loaded"
                If Me.TBSmenu.Tabs(TabIndex.SceltaUtente).Visible = True Then
                    Me.BTNfase0ToFase1.Visible = False
                Else
                    Me.BTNfase0ToFase1.Visible = True
                End If
            Case TabIndex.SceltaUtente
                If Me.TXBsorgente.Text = "" Or Me.TXBdestinatario.Text = "" Then
                    Me.PNLsorgente.Visible = True
                    Me.PNLutenti.Visible = False
                    Me.PNLruoliDestinazione.Visible = False
                    Me.TBSmenu.SelectedIndex = 0
                    Exit Sub
                End If
                Me.PNLsorgente.Visible = False
                Me.PNLutenti.Visible = True
                Me.PNLruoliDestinazione.Visible = False

                Session("azione") = "loadUtenti"
                If Me.RBLutenti.SelectedValue = 0 Then
                    Me.PNLfiltro.Visible = True
                    Me.DGPersone.Visible = True
                    Me.PNLsceltaRuoli.Visible = False

                    Dim stringaIDsorgente(), stringaIDdestinatario() As String
                    stringaIDsorgente = Me.HDN_valoreSorgente.Value.Split(",")
                    stringaIDdestinatario = Me.HDN_valoreDestinatario.Value.Split(",")

                    If Me.HDNsorgente_ID.Value <> stringaIDsorgente(0) Or Me.HDNdestinazione_ID.Value <> stringaIDdestinatario(0) Then
                        Me.Bind_DatiPersone(True)
                    Else
                        Me.Bind_DatiPersone(False)
                    End If
                ElseIf Me.RBLutenti.SelectedValue = 1 Then
                    Me.PNLsceltaRuoli.Visible = True
                    Me.PNLfiltro.Visible = False
                    Me.DGPersone.Visible = False
                    Me.PNLNoUsers.Visible = False
                    '  Me.Bind_DatiSceltaRuoli()
                ElseIf Me.RBLutenti.SelectedValue = 2 Then
                    Me.PNLfiltro.Visible = False
                    Me.DGPersone.Visible = False
                    Me.PNLNoUsers.Visible = False
                    Me.PNLsceltaRuoli.Visible = False
                End If
            Case TabIndex.SceltaRuolo
                If Me.TXBsorgente.Text = "" Or Me.TXBdestinatario.Text = "" Then
                    Me.PNLsorgente.Visible = True
                    Me.PNLutenti.Visible = False
                    Me.PNLruoliDestinazione.Visible = False
                    Me.TBSmenu.Tabs(TabIndex.Finale).Visible = False
                    Me.TBSmenu.Tabs(TabIndex.SceltaRuolo).Visible = False
                    Me.TBSmenu.SelectedIndex = 0
                    Exit Sub
                ElseIf Me.RBLutenti.SelectedIndex = -1 Then
                    Me.PNLsorgente.Visible = False
                    Me.PNLutenti.Visible = True
                    Me.TBSmenu.Tabs(TabIndex.SceltaUtente).Visible = True
                    Me.TBSmenu.SelectedIndex = 1
                    Me.RBLutenti.SelectedValue = 0
                    Session("azione") = "loadUtenti"
                    Me.Bind_DatiPersone(False)
                    Exit Sub
                Else
                    Me.PNLutenti.Visible = False
                    Me.PNLruoliDestinazione.Visible = True
                    Me.Bind_Ruoli_SorgenteDestinazione()
                    Session("Azione") = "SceltaRuoli"
                End If
                'Me.PNLsorgente.Visible = False
                'Me.PNLutenti.Visible = False
                'Me.PNLruoliDestinazione.Visible = False
            Case TabIndex.Finale

        End Select
    End Sub
#End Region

#Region "Import Utenti"
    Private Function VerificaRuolo(ByVal CMNT_ID As Integer, ByVal PRSN_Id As Integer, ByVal TPRL_ID As Integer) As Integer
        Dim oRuoloComunita As New COL_RuoloPersonaComunita

        Try
            oRuoloComunita.EstraiByLinguaDefault(CMNT_ID, PRSN_Id)
            If oRuoloComunita.Errore = Errori_Db.None Then
                If oRuoloComunita.TipoRuolo.Id = -2 Then
                    'trattasi di amministratore..... come lo iscrivo ???
                    TPRL_ID = CType(Main.TipoRuoloStandard.AdminComunità, Main.TipoRuoloStandard)
                End If
            End If
        Catch ex As Exception
            Return TPRL_ID
        End Try
        Return TPRL_ID
    End Function
    Private Sub AggiornaXML(ByVal CMNT_ID As Integer, ByVal PRSN_Id As Integer, ByVal CMNT_PathPassanti As String)
        'aggiorna i file xml degli utenti selezionati nella datagrid
        Dim oTreeComunita As New COL_TreeComunita
        Dim oComunita As New COL_Comunita


        'salvo le modifiche su file xml

        Try

            oComunita.Id = CMNT_ID
            oComunita.Estrai()

            'ArrComunita = Session("ArrComunita")
            'CMNT_Path = ArrComunita(2, UBound(ArrComunita, 2))

            Dim oRuoloPersonaComunita As New COL_RuoloPersonaComunita
            oRuoloPersonaComunita.EstraiByLinguaDefault(CMNT_ID, PRSN_Id)

            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_Id & "\"
            oTreeComunita.Nome = PRSN_Id & ".xml"
            oTreeComunita.Update(oComunita, CMNT_PathPassanti, oComunita.GetNomeResponsabile_NomeCreatore, oRuoloPersonaComunita)
        Catch ex As Exception

        End Try
    End Sub
#End Region


    Private Sub BTNreImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNreImport.Click
        Session("Azione") = "loaded"
        Me.TBSmenu.Tabs(TabIndex.Finale).Visible = False
        Me.TBSmenu.Tabs(TabIndex.SorgenteDestinazione).Visible = True
        Me.TBSmenu.SelectedIndex = 0
        Me.TXBdestinatario.Text = ""
        Me.TXBsorgente.Text = ""
        Me.HDN_valoreDestinatario.Value = ""
        Me.HDN_valoreSorgente.Value = ""
        Me.HDNdestinazione_ID.Value = ""
        Me.HDNsorgente_ID.Value = ""
        Me.RBLutenti.SelectedIndex = 0
        Me.HDabilitato.Value = ""
        Me.HDtutti.Value = ""

        Me.DDLannoAccademico.SelectedIndex = 0
        Me.DDLannoAccademicoDest.SelectedIndex = 0
        Me.DDLperiodo.SelectedIndex = 0
        Me.DDLperiodoDest.SelectedIndex = 0

        Me.DDLTipo.SelectedIndex = 0
        Me.DDLTipoDest.SelectedIndex = 0
        Me.TBRcorsi.Visible = False
        Me.TBRcorsiDest.Visible = False
        Me.TBRcorsoDiStudi.Visible = False
        Me.TBRcorsoDiStudiDest.Visible = False

        Me.DDLTipoPersona.SelectedIndex = 0
        Me.DDLTipoRuolo.SelectedIndex = 0

        Me.Bind_TreeView()
        Me.Bind_TreeView_Destinatario()

        Me.BTNfase0ToFase1.Visible = True
    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property
End Class
