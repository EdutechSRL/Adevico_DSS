Imports Comunita_OnLine
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports lm.Comol.Core.File

'ATTENZIONE: 
'Nell'inserimento e nella modifica, manca il settaggio dei seguenti parametri:
'.HasAccessoCopisteria=???
'.HasAccessoLibero = ???
'Vengono quindi mantenuti quelli di default... (True x copisteria, False x Accesso Libero)
'
'Ricontrollare anche .Archiviata e .Bloccata (Da settare nella stessa maniera per l'inserimento)
'Nella modifica non vengono toccati...
'
'(Mirco)

Public Class ADM_Organizzazione
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager


    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum
    Private Enum Inserimento
        InserimentoOK = 1
        ErroreGenerico = 0
        ModificaOK = 2
        CancellazioneOK = 3
        ErroreModifica = -2
        ErroreInserimento = -1
        ErroreCancellazione = -3
        ErroreCancellazioneComunità = -4
        ErroreCaricamentoDati = -5
    End Enum

#Region "Gestione Permessi"
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
#End Region

    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBnuovo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLmenuInserimento As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannulla As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBinserisci As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBmodifica As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLautenticazioni As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBautenticazioneIns As System.Web.UI.WebControls.LinkButton

#Region "Griglia"
    Protected WithEvents LBfiltroIstituzione_t As System.Web.UI.WebControls.Label
    Protected WithEvents DGOrganizzazione As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PNLdgOrganizzazione As System.Web.UI.WebControls.Panel
    Protected WithEvents DDLIstituzioni As System.Web.UI.WebControls.DropDownList
#End Region

#Region "Modifica Organizzazione"
    Protected WithEvents TBR_0 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBR_1 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBR_2 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBR_3 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBR_4 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBR_5 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBR_6 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBR_7 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBR_8 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBR_9 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBR_11 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBR_12 As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBlimiti_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBLlimiti As System.Web.UI.WebControls.CheckBoxList

    Protected WithEvents TBR_uploadLogo As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBlogo_t As System.Web.UI.WebControls.Label
    Protected WithEvents FILElogo As System.Web.UI.HtmlControls.HtmlInputFile


    Protected WithEvents TBR_modificaLogo As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBlogoModifica_t As System.Web.UI.WebControls.Label
    Protected WithEvents IMGlogo As System.Web.UI.WebControls.Image
    Protected WithEvents FILElogoModifica As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents BTNeliminaLogo As System.Web.UI.WebControls.Button
    Protected WithEvents BTNaggiorna As System.Web.UI.WebControls.Button
    Protected WithEvents TBRiscrizioniAutonome As System.Web.UI.WebControls.TableRow

    Protected WithEvents LBiscrizioniAutonome As System.Web.UI.WebControls.Label
    Protected WithEvents CBLiscrizioniAutonome As System.Web.UI.WebControls.CheckBoxList


    Protected WithEvents TXBragioneSociale As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LBisFacoltà As System.Web.UI.WebControls.Label
    Protected WithEvents CBXisFacolta As System.Web.UI.WebControls.CheckBox

    Protected WithEvents TXBindirizzo As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents TXBcap As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator7 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Cap As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents TXBcitta As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents DDLprovincia As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLstato As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBtelefono1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents phoneReqVal As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Regularexpressionvalidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents TXBtelefono2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Regularexpressionvalidator3 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents TXBfax As System.Web.UI.WebControls.TextBox
    Protected WithEvents Regularexpressionvalidator4 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents TXBhomePage As System.Web.UI.WebControls.TextBox
    Protected WithEvents RBLisChiusa As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents TXBid_n As System.Web.UI.HtmlControls.HtmlInputHidden


    Protected WithEvents LBiscrivimi As System.Web.UI.WebControls.Label
    Protected WithEvents CBXiscrivimi As System.Web.UI.WebControls.CheckBox

    Protected WithEvents LBistituzione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBragioneSociale_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBindirizzo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcitta_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcap_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBprovincia As System.Web.UI.WebControls.Label
    Protected WithEvents LBstato_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtelefono1_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtelefono2_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBfax_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBhomePage_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBisChiusa As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents DDLIstituzioneForm As System.Web.UI.WebControls.DropDownList
    Protected WithEvents PNLDatiOrganizzazione As System.Web.UI.WebControls.Panel

#Region "Griglia autenticazione"
    Protected WithEvents DGautenticazione As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LNBassociaAutenticazione As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBnoAutenticaz As System.Web.UI.WebControls.Label

#End Region



#Region "Gestione Autenticazione"
    Protected WithEvents LBtipoAutenticazione As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoAutenticazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBdescrizione As System.Web.UI.WebControls.Label
    Protected WithEvents TXBdescrizione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBparametro_1 As System.Web.UI.WebControls.Label
    Protected WithEvents TXBparametro_1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBparametro_2 As System.Web.UI.WebControls.Label
    Protected WithEvents TXBparametro_2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBparametro_3 As System.Web.UI.WebControls.Label
    Protected WithEvents TXBparametro_3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBparametro_4 As System.Web.UI.WebControls.Label
    Protected WithEvents TXBparametro_4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents HDNlkao_ID As System.Web.UI.HtmlControls.HtmlInputHidden
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
        Dim oUtility As New OLDpageUtility(HttpContext.Current)
        oUtility.RedirectToUrl("Admin_Globale/OrganizationsManagement.aspx")
        'If IsNothing(oResource) Then
        '    Me.SetCulture(Session("LinguaCode"))
        'End If
        'If Me.SessioneScaduta() Then
        '    Exit Sub
        'End If
        'If Not Page.IsPostBack Then
        '    Me.DDLstato.Attributes.Add("onchange", "return AggiornaForm();")
        '    Me.SetupInternazionalizzazione()
        '    Me.Bind_Dati()
        '    Me.Bind_Griglia()
        '    Session("azione") = "load"

        '    Me.SetupStartupScript()
        'End If
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

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_ADMorganizzazione"
        oResource.Folder_Level1 = "Admin_Globale"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")

            .setLabel(Me.LBNopermessi)
            .setLinkButton(Me.LNBnuovo, True, True)
            .setHeaderDatagrid(Me.DGOrganizzazione, 1, "ORGN_ragioneSociale", True)
            .setHeaderDatagrid(Me.DGOrganizzazione, 2, "ORGN_indirizzo", True)
            .setHeaderDatagrid(Me.DGOrganizzazione, 3, "ORGN_citta", True)
            .setHeaderDatagrid(Me.DGOrganizzazione, 4, "HomePage", True)
            .setHeaderDatagrid(Me.DGOrganizzazione, 5, "isFacolta", True)

            .setLinkButton(Me.LNBinserisci, True, True)
            .setLinkButton(Me.LNBmodifica, True, True)
            .setLinkButton(Me.LNBannulla, True, True)
            .setLinkButton(Me.LNBassociaAutenticazione, True, True)
            .setLinkButton(Me.LNBautenticazioneIns, True, True)
            .setLinkButton(Me.LNBindietro, True, True)
            .setLabel(Me.LBfiltroIstituzione_t)
            .setLabel(Me.LBistituzione_t)
            .setLabel(Me.LBragioneSociale_t)
            .setLabel(Me.LBisFacoltà)
            .setCheckBox(Me.CBXisFacolta)
            .setLabel(Me.LBindirizzo_t)
            .setLabel(Me.LBcitta_t)
            .setLabel(Me.LBcap_t)
            .setLabel(Me.LBprovincia)
            .setLabel(Me.LBstato_t)
            .setLabel(Me.LBtelefono1_t)
            .setLabel(Me.LBtelefono2_t)
            .setLabel(Me.LBfax_t)
            .setLabel(Me.LBhomePage_t)
            .setLabel(Me.LBlogo_t)
            .setLabel(Me.LBlogoModifica_t)
            .setLabel(Me.LBisChiusa)
            .setButton(Me.BTNaggiorna, True)
            .setButton(Me.BTNeliminaLogo, True)
            .setRadioButtonList(Me.RBLisChiusa, 0)
            .setRadioButtonList(Me.RBLisChiusa, 1)
            .setLabel(Me.LBiscrivimi)
            .setCheckBox(Me.CBXiscrivimi)
            .setLabel(Me.LBiscrizioniAutonome)
            .setLabel(Me.LBlimiti_t)
            .setHeaderDatagrid(Me.DGautenticazione, 1, "AUTN_nome", True)
            .setHeaderDatagrid(Me.DGautenticazione, 2, "LKAO_descrizione", True)
            .setHeaderDatagrid(Me.DGautenticazione, 3, "LKAO_parametro_1", True)
            .setHeaderDatagrid(Me.DGautenticazione, 3, "LKAO_parametro_2", True)
            .setHeaderDatagrid(Me.DGautenticazione, 3, "LKAO_parametro_3", True)
            .setHeaderDatagrid(Me.DGautenticazione, 3, "LKAO_parametro_4", True)
            .setLabel(Me.LBnoAutenticaz)
            .setLabel(Me.LBtipoAutenticazione)
            .setLabel(Me.LBdescrizione)
            .setLabel(Me.LBparametro_1)
            .setLabel(Me.LBparametro_2)
            .setLabel(Me.LBparametro_3)
            .setLabel(Me.LBparametro_4)

        End With

    End Sub
#End Region

    Private Sub SetupStartupScript()
        'aggiunta Proprietà dei linkbutton della pagina
        Me.LNBinserisci.Attributes.Add("onclick", "window.status='Aggiungi una nuova Organizzazione';return true;")
        Me.LNBinserisci.Attributes.Add("onmouseover", "window.status='Aggiungi una nuova Organizzazione';return true;")
        Me.LNBinserisci.Attributes.Add("onfocus", "window.status='Aggiungi una nuova Organizzazione';return true;")
        Me.LNBinserisci.Attributes.Add("onmouseout", "window.status='';return true;")

        Me.LNBassociaAutenticazione.Attributes.Add("onclick", "window.status='Aggiungi nuova Autenticazione';return true;")
        Me.LNBassociaAutenticazione.Attributes.Add("onmouseover", "window.status='Aggiungi nuova Autenticazione';return true;")
        Me.LNBassociaAutenticazione.Attributes.Add("onfocus", "window.status='Aggiungi nuova Autenticazione';return true;")
        Me.LNBassociaAutenticazione.Attributes.Add("onmouseout", "window.status='';return true;")

        Me.TXBdescrizione.Attributes.Add("onkeypress", "return(LimitText(this,100));")
    End Sub

#Region "Bind Dati"

    Private Sub Bind_Dati()
        Me.Bind_FiltroIstituzioni()
        'Me.Bind_Provincie()
        Me.Bind_Stati()
        Bind_Provincie(DDLstato.SelectedItem.Text)
        Me.Bind_Istituzioni()
    End Sub
    Private Sub Bind_FiltroIstituzioni()
        Dim oIstituzione As New COL_Istituzione

        Try
            Me.DDLIstituzioni.Items.Clear()

            Me.DDLIstituzioni.DataSource = oIstituzione.Elenca.Tables(0).DefaultView
            Me.DDLIstituzioni.DataTextField() = "ISTT_ragioneSociale"
            Me.DDLIstituzioni.DataValueField = "ISTT_Id"
            Me.DDLIstituzioni.DataBind()
            If Me.DDLIstituzioni.Items.Count > 1 Then
                Me.DDLIstituzioni.Items.Insert(0, New ListItem("-- Tutti --", 0))
            End If

        Catch ex As Exception
            Me.DDLIstituzioni.Items.Add(New ListItem("< dati non presenti >", -1))
        End Try

    End Sub
    Private Sub Bind_Provincie(ByVal Stato As String)
        'riempio la DDLprovincia solo se lo stato è l'Italia
        If Stato = "Italia" Then
            DDLprovincia.Enabled = True
			DDLprovincia.DataSource = ManagerProvincia.List
			DDLprovincia.DataTextField() = "Nome"
			DDLprovincia.DataValueField = "ID"
            DDLprovincia.DataBind()
        Else
            DDLprovincia.Enabled = False
        End If
    End Sub
    Private Sub Bind_Stati()
		DDLstato.DataSource = COL_Stato.List
		DDLstato.DataTextField() = "Descrizione"
		DDLstato.DataValueField = "ID"
		DDLstato.DataBind()
		Try
			Dim oStatoItaliano As COL_Stato = COL_Stato.GetByName("Italia")
			If Not IsNothing(oStatoItaliano) Then
				DDLstato.SelectedValue = oStatoItaliano.ID
			End If
		Catch ex As Exception

		End Try
    End Sub
    Private Sub Bind_Istituzioni()
        Dim oIstituzione As New COL_Istituzione

        Try
            DDLIstituzioneForm.DataSource = oIstituzione.Elenca.Tables(0).DefaultView
            DDLIstituzioneForm.DataTextField() = "ISTT_ragioneSociale"
            DDLIstituzioneForm.DataValueField = "ISTT_id"
            DDLIstituzioneForm.DataBind()
        Catch ex As Exception
            Me.DDLIstituzioneForm.Items.Add(New ListItem("< dati non presenti >", -1))
        End Try

    End Sub

    Private Sub DDLstato_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLstato.SelectedIndexChanged
        If Me.DDLstato.SelectedItem.Value <> "193" Then 'italia
            Bind_Provincie(DDLstato.SelectedItem.Text)
            DDLprovincia.SelectedValue = "0" 'estera provincia

            DDLprovincia.Enabled = False
        Else
            Bind_Provincie(DDLstato.SelectedItem.Text)
            DDLprovincia.Enabled = True
        End If
        Me.Bind_Autenticazioni(Me.TXBid_n.Value)
    End Sub
    Private Sub DDLIstituzioni_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLIstituzioni.SelectedIndexChanged
        Bind_Griglia()
    End Sub

    Private Sub Bind_Autenticazioni(ByVal ORGN_Id As Integer)
        Try
            Dim oDataset As DataSet
            Dim oOrganizzazione As New COL_Organizzazione
            oOrganizzazione.Id = ORGN_Id
            oDataset = oOrganizzazione.ElencaAutenticazioni

            If oDataset.Tables(0).Rows.Count > 0 Then
                Me.DGautenticazione.Visible = True
                Me.LBnoAutenticaz.Text = ""
                Me.DGautenticazione.DataSource = oDataset
                Me.DGautenticazione.DataBind()

                If oDataset.Tables(0).Rows.Count = 1 Then
                    Dim oImageButton As ImageButton
                    oImageButton = Me.DGautenticazione.Items(0).Cells(0).FindControl("IMBCancella2")
                    If IsNothing(oImageButton) = False Then
                        oImageButton.Enabled = False
                        oImageButton.ImageUrl = "../images/x_d.gif"
                    End If
                End If
            Else
                Me.DGautenticazione.Visible = False
                Me.LBnoAutenticaz.Text = "Nessuna autenticazione specificata"
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TipoAutenticazioni(ByVal ORGN_Id As Integer)
        Try
            Dim oDataset As DataSet
            Dim oOrganizzazione As New COL_Organizzazione
            oOrganizzazione.Id = ORGN_Id


            Me.DDLtipoAutenticazione.Items.Clear()
            oDataset = oOrganizzazione.ElencaTipiAutenticazioneDisponibili()

            If oDataset.Tables(0).Rows.Count > 0 Then
                Me.DDLtipoAutenticazione.DataSource = oDataset
                Me.DDLtipoAutenticazione.DataTextField = "AUTN_Nome"
                Me.DDLtipoAutenticazione.DataValueField = "AUTN_ID"
                Me.DDLtipoAutenticazione.DataBind()
                Me.DDLtipoAutenticazione.Enabled = True
                Me.LNBautenticazioneIns.Enabled = True
            Else
                Me.DDLtipoAutenticazione.Enabled = False
                Me.LNBautenticazioneIns.Enabled = False
            End If

        Catch ex As Exception
            Me.DDLtipoAutenticazione.Enabled = False
            Me.LNBautenticazioneIns.Enabled = False
        End Try
    End Sub
    Private Sub Bind_IscrizioniAutonome(ByVal ORGN_ID As Integer)
        Try
            Dim i, totale As Integer
            Dim oDataset As DataSet
            Dim oOrganizzazione As COL_Organizzazione

            Me.CBLiscrizioniAutonome.Items.Clear()
            oDataset = oOrganizzazione.getIscrizioniAutonome(ORGN_ID)

            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                For i = 0 To totale - 1
                    Dim oListitem As New ListItem
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    If IsDBNull(oRow.Item("TPPR_descrizione")) Then
                        oListitem.Text = oRow.Item("TPPR_ID")
                    Else
                        oListitem.Text = oRow.Item("TPPR_descrizione")
                    End If
                    If IsDBNull(oRow.Item("ISOR_ID")) Then
                        oListitem.Value = oRow.Item("TPPR_ID") & ",0"
                        oListitem.Selected = False
                    Else
                        oListitem.Value = oRow.Item("TPPR_ID") & "," & oRow.Item("ISOR_ID")
                        oListitem.Selected = True
                    End If
                    Me.CBLiscrizioniAutonome.Items.Add(oListitem)
                Next
                Me.LNBmodifica.Enabled = True
                Me.LNBinserisci.Enabled = True
            Else
                Me.LNBmodifica.Enabled = False
                Me.LNBinserisci.Enabled = False
            End If
        Catch ex As Exception
            Me.LNBmodifica.Enabled = False
            Me.LNBinserisci.Enabled = False
        End Try
    End Sub
#End Region

#Region "Gestione griglia"

    Sub DGOrganizzazione_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGOrganizzazione.PageIndexChanged
        DGOrganizzazione.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia()
        'DGOrganizzazione.DataBind()
    End Sub

    Private Sub Bind_Griglia()
        Dim oOrganizzazione As New COL_Organizzazione
        Dim oDataset As New DataSet

        Try
            Dim i, totale As Integer
            oDataset = oOrganizzazione.ElencaByIstituzione(Me.DDLIstituzioni.SelectedValue)

            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then
                Me.DGOrganizzazione.Visible = False
                Exit Sub
            Else
                Me.DGOrganizzazione.Visible = True

            End If
            oDataset.Tables(0).Columns.Add(New DataColumn("isFacolta"))
            For i = 0 To totale - 1
                Try
                    oDataset.Tables(0).Rows(i).Item("isFacolta") = Me.oResource.getValue("isFacolta." & oDataset.Tables(0).Rows(i).Item("ORGN_isFacolta"))
                Catch ex As Exception

                End Try
            Next
            Me.ViewState("numOrganizzazioni") = oDataset.Tables(0).Rows.Count

            Me.DGOrganizzazione.DataSource = oDataset
            Me.DGOrganizzazione.DataBind()
        Catch ex As Exception
            Me.ViewState("numOrganizzazioni") = -1
        End Try
    End Sub

    Private Sub DGOrganizzazione_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGOrganizzazione.ItemCreated
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
                    If Me.DGOrganizzazione.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGOrganizzazione, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGOrganizzazione, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGOrganizzazione.HeaderStyle.CssClass
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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGOrganizzazione, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGOrganizzazione.HeaderStyle.CssClass
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
        If (e.Item.ItemType = ListItemType.Footer) Then
            'rimuovo le altre colonne 
            For i = 1 To e.Item.Cells.Count - 1
                e.Item.Cells.RemoveAt(1)
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
                    oResource.setPageDatagrid(Me.DGOrganizzazione, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then


            Dim oImageButton As ImageButton

            Dim oCell As New TableCell
            Try
                oCell = CType(e.Item.Cells(0), TableCell)
                Try
                    oImageButton = oCell.FindControl("IMBCancella")
                    Me.oResource.setImageButton(oImageButton, True, True, True, True)
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try

            Dim oImageButton2 As ImageButton

            Dim oCell2 As New TableCell
            Try
                oCell2 = CType(e.Item.Cells(0), TableCell)
                Try
                    oImageButton2 = oCell2.FindControl("IMBModifica")
                    Me.oResource.setImageButton(oImageButton2, True, True, True)
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try


        End If
    End Sub

    Private Sub DGOrganizzazione_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGOrganizzazione.ItemCommand
        Try
            Dim alertMSG As String
            Dim oOrganizzazione As New COL_Organizzazione
            oOrganizzazione.Id = CInt(DGOrganizzazione.DataKeys.Item(e.Item.ItemIndex))
            Dim Codice As Integer
            Select Case e.CommandName
                Case "modifica"

                    oOrganizzazione.Estrai()
                    If oOrganizzazione.Errore <> Errori_Db.DBError Then
                        Me.PNLDatiOrganizzazione.Visible = True
                        Me.PNLdgOrganizzazione.Visible = False
                        Me.PNLmenuInserimento.Visible = True
                        Me.PNLautenticazioni.Visible = False
                        Me.PNLmenu.Visible = False
                        Me.LNBinserisci.Visible = False
                        Me.LNBmodifica.Visible = True


                        Me.DDLIstituzioneForm.SelectedValue = oOrganizzazione.GetIstituzione
                        Me.TXBid_n.Value = oOrganizzazione.Id
                        Me.TXBcap.Text = oOrganizzazione.Cap
                        Me.TXBcitta.Text = oOrganizzazione.Citta
                        Me.TXBfax.Text = oOrganizzazione.Fax
                        Me.TXBhomePage.Text = oOrganizzazione.HomePage
                        Me.TXBindirizzo.Text = oOrganizzazione.Indirizzo
                        Me.TXBragioneSociale.Text = oOrganizzazione.RagioneSociale
                        Me.TXBtelefono1.Text = oOrganizzazione.Telefono1
                        Me.TXBtelefono2.Text = oOrganizzazione.Telefono2
                        Me.CBXisFacolta.Checked = oOrganizzazione.IsFacolta
                        If oOrganizzazione.IsChiusa = True Then
                            Me.RBLisChiusa.SelectedValue = 1
                        Else
                            Me.RBLisChiusa.SelectedValue = 0
                        End If
                        'Me.RBisChiusa.SelectedValue = oOrganizzazione.IsChiusa
                        Me.DDLstato.SelectedValue = oOrganizzazione.Stato.Id
                        Me.DDLprovincia.SelectedValue = oOrganizzazione.Provincia.Id
                        If DDLstato.SelectedValue <> "193" Then
                            Me.DDLprovincia.Enabled = False
                        End If

                        If oOrganizzazione.Logo = "" Then
                            Me.IMGlogo.ImageUrl = "./../images/nologo.gif"
                            Me.BTNeliminaLogo.Visible = False
                        Else
                            If Exists.File(Server.MapPath("./../images/Organizzazione/" & oOrganizzazione.Logo)) Then
                                Me.IMGlogo.ImageUrl = "./../images/Organizzazione/" & oOrganizzazione.Logo
                                Me.BTNeliminaLogo.Visible = True
                            Else
                                Me.IMGlogo.ImageUrl = "./../images/nologo.gif"
                                Me.BTNeliminaLogo.Visible = False
                            End If
                        End If
                        Me.TBR_uploadLogo.Visible = False
                        Me.TBR_modificaLogo.Visible = True
                        Me.LNBassociaAutenticazione.Visible = True
                        'FACCIO IL BIND DELLA DGAUTENTICAZIONI!!!
                        Me.Bind_Autenticazioni(CInt(DGOrganizzazione.DataKeys.Item(e.Item.ItemIndex)))
                        Me.Bind_IscrizioniAutonome(CInt(DGOrganizzazione.DataKeys.Item(e.Item.ItemIndex)))
                        Me.LBiscrivimi.Visible = False
                        Me.CBXiscrivimi.Visible = False
                        Session("azione") = "modifica"
                    Else
                        alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ErroreCaricamentoDati, Inserimento))
                        If alertMSG <> "" Then
                            alertMSG = alertMSG.Replace("'", "\'")
                        End If
                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                        Me.Bind_Griglia()
                    End If

                Case "cancella"
                    'cancelliamo l'organizzazione solamente se ad essa non sono associate delle persone e delle sottocomunità
                    Codice = oOrganizzazione.Elimina()


                    If oOrganizzazione.Errore <> Errori_Db.DBError Then
                        Me.AggiornaApplication()
                    Else
                        alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ErroreModifica, Inserimento))
                    End If

                    If Codice = 1 Then
                        alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.CancellazioneOK, Inserimento))
                    ElseIf Codice = -1 Or -2 Then
                        ' LBavviso.Text = "Non è possiblile cancellare in quanto all'organizzazione sono associate altre relazioni"
                        alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ErroreCancellazioneComunità, Inserimento))
                    ElseIf Codice = -3 Then
                        ' LBavviso.Text = "Spiacente, si sono verificati Problemi nella cancellazione"
                        alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ErroreCancellazione, Inserimento))
                    End If

                    If alertMSG <> "" Then
                        alertMSG = alertMSG.Replace("'", "\'")
                    End If
                    Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                    Me.Bind_Griglia()
            End Select
        Catch ex As Exception

        End Try
    End Sub
    Private Sub AzzeraTextBox()
        'azzera texbox, dropdownlist e radiobutton relative a modifica e inserimento

        Me.TXBid_n.Value = ""
        Me.TXBcap.Text = ""
        Me.TXBcitta.Text = ""
        Me.TXBfax.Text = ""
        Me.TXBhomePage.Text = ""
        Me.TXBindirizzo.Text = ""
        Me.TXBragioneSociale.Text = ""
        Me.TXBtelefono1.Text = ""
        Me.TXBtelefono2.Text = ""
        Me.RBLisChiusa.SelectedValue = 0
        Me.DDLstato.SelectedItem.Text = "Italia"
        '  Me.DDLprovincia.SelectedIndex = 0
        Me.DDLIstituzioneForm.SelectedIndex = 0
        Me.CBXiscrivimi.Checked = True
        Me.CBXisFacolta.Checked = False
        Me.Bind_IscrizioniAutonome(0)
    End Sub
#End Region

#Region "Gestione griglia Autenticazione"

    Private Sub DGautenticazione_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGautenticazione.ItemCreated
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
                    If Me.DGOrganizzazione.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGOrganizzazione, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGOrganizzazione, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGOrganizzazione.HeaderStyle.CssClass
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
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGOrganizzazione, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGOrganizzazione.HeaderStyle.CssClass
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
        If (e.Item.ItemType = ListItemType.Footer) Then
            'rimuovo le altre colonne 
            For i = 1 To e.Item.Cells.Count - 1
                e.Item.Cells.RemoveAt(1)
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
                    oResource.setPageDatagrid(Me.DGOrganizzazione, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Try
                If IsDBNull(e.Item.DataItem("LKAO_parametro_1")) = False Then
                    Me.DGautenticazione.Columns(3).Visible = True
                End If
                If IsDBNull(e.Item.DataItem("LKAO_parametro_2")) = False Then
                    Me.DGautenticazione.Columns(4).Visible = True
                End If
                If IsDBNull(e.Item.DataItem("LKAO_parametro_3")) = False Then
                    Me.DGautenticazione.Columns(5).Visible = True
                End If
                If IsDBNull(e.Item.DataItem("LKAO_parametro_4")) = False Then
                    Me.DGautenticazione.Columns(6).Visible = True
                End If
            Catch ex As Exception

            End Try

            Dim oImageButton As ImageButton

            Dim oCell As New TableCell
            Try
                oCell = CType(e.Item.Cells(0), TableCell)
                Try
                    oImageButton = oCell.FindControl("IMBCancella2")

                    Me.oResource.setImageButton(oImageButton, True, True, True, True)
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try

            Dim oImageButton2 As ImageButton

            Dim oCell2 As New TableCell
            Try
                oCell2 = CType(e.Item.Cells(0), TableCell)
                Try
                    Me.oResource.setImageButton(oImageButton, True, True, True)
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try

        End If
    End Sub

    Private Sub DGautenticazione_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGautenticazione.ItemCommand
        Dim oOrganizzazione As New COL_Organizzazione
        Dim LKAO_id As Integer


        Try
            oOrganizzazione.Id = Me.TXBid_n.Value
            LKAO_id = CInt(Me.DGautenticazione.DataKeys.Item(e.Item.ItemIndex))
            Select Case e.CommandName
                Case "modifica"
                    Dim oParametro1, oParametro2, oParametro3, oParametro4, oDescrizione As String
                    Dim AUTN_ID As Integer

                    Session("azione") = "modificaAutenticazione"
                    oOrganizzazione.EstraiDatiAutenticazione(LKAO_id, AUTN_ID, oDescrizione, oParametro1, oParametro2, oParametro3, oParametro4)
                    If oOrganizzazione.Errore = Errori_Db.None Then
                        Me.NascondiRigheStandard(True)
                        Me.Bind_TipoAutenticazioni(-1)
                        Me.TXBdescrizione.Text = oDescrizione
                        Me.TXBparametro_1.Text = oParametro1
                        Me.TXBparametro_2.Text = oParametro2
                        Me.TXBparametro_3.Text = oParametro3
                        Me.TXBparametro_4.Text = oParametro4
                        Try
                            Me.DDLtipoAutenticazione.SelectedValue = AUTN_ID
                            If AUTN_ID = Main.TipoAutenticazione.ComunitaOnLine Then
                                Me.DDLtipoAutenticazione.Enabled = False
                            Else
                                Me.DDLtipoAutenticazione.SelectedValue = True
                            End If
                        Catch ex As Exception

                        End Try
                        Me.HDNlkao_ID.Value = LKAO_id
                        Me.oResource.setLinkButtonToValue(LNBautenticazioneIns, "modifica", True, True)
                    End If
                Case "cancella"
                    oOrganizzazione.DisAssociaAutenticazione(-1, LKAO_id)
                    Me.Bind_Autenticazioni(Me.TXBid_n.Value)
            End Select
        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Gestione Organizzazione"
    Private Sub LNBnuovo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBnuovo.Click
        Me.LNBassociaAutenticazione.Visible = False
        Me.PNLdgOrganizzazione.Visible = False
        Me.PNLDatiOrganizzazione.Visible = True
        Me.PNLmenuInserimento.Visible = True
        Me.PNLmenu.Visible = False

        Me.LNBinserisci.Visible = True
        Me.LNBmodifica.Visible = False

        Me.AzzeraTextBox()
        Me.LBiscrivimi.Visible = True
        Me.CBXiscrivimi.Visible = True
        Me.TBR_uploadLogo.Visible = True
        Me.TBR_modificaLogo.Visible = False
        Me.TXBid_n.Value = 0
        Me.DGautenticazione.Visible = False
        Me.LBnoAutenticaz.Visible = False
        Session("azione") = "aggiungi"
    End Sub
    Private Sub LNBinserisci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBinserisci.Click
        If Session("azione") = "aggiungi" Then
            Dim oOrganizzazione As New COL_Organizzazione
            Dim oTipoComunita As New COL_Tipo_Comunita
            Dim oPersona As New COL_Persona
            oTipoComunita.ID = Main.TipoComunitaStandard.Organizzazione

            oPersona = Session("objPersona")
            'Me.DDLIstituzioneForm.SelectedValue = oOrganizzazione.GetIstituzione

            Dim CMNT_ID As Integer
            Dim PRSN_ID As Integer
            PRSN_ID = oPersona.Id

            With oOrganizzazione
                .Cap = Me.TXBcap.Text
                .Citta = Me.TXBcitta.Text
                .Fax = Me.TXBfax.Text
                .HomePage = Me.TXBhomePage.Text
                .Indirizzo = Me.TXBindirizzo.Text
                .RagioneSociale = Me.TXBragioneSociale.Text
                .Telefono1 = Me.TXBtelefono1.Text
                .Telefono2 = Me.TXBtelefono2.Text
                .IsChiusa = CBool(Me.RBLisChiusa.SelectedValue = 1)
                .Stato.Id = Me.DDLstato.SelectedValue
                .Provincia.Id = Me.DDLprovincia.SelectedValue
                .IsFacolta = Me.CBXisFacolta.Checked
                '.HasAccessoCopisteria=???
                '.HasAccessoLibero = ???
                CMNT_ID = .Aggiungi(Me.DDLIstituzioneForm.SelectedValue, oTipoComunita.ModelloDefault(), PRSN_ID, Me.CBXiscrivimi.Checked, 0)

                If .Errore = Errori_Db.None And CMNT_ID > 0 Then
                    Dim oTreeComunita As New COL_TreeComunita
                    Dim oComunita As New COL_Comunita
                    Dim oRuolo As New COL_RuoloPersonaComunita

                    If CMNT_ID > 0 Then

                        oComunita.Id = CMNT_ID
                        Try
                            Dim creatore As String
                            oComunita.Estrai()
                            oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona
                            oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)
                            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
                            oTreeComunita.Nome = PRSN_ID & ".xml"
                            creatore = oPersona.Cognome & " " & oPersona.Nome
                            oTreeComunita.AggiornaInfo(PRSN_ID, -1)
                            '  oTreeComunita.Insert(oComunita, "." & CMNT_ID & ".", oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)
                        Catch ex As Exception

                        End Try

                    End If

                    Dim i, totale, TipoPersonaId As Integer
                    For i = 0 To Me.CBLiscrizioniAutonome.Items.Count - 1
                        Dim valori() As String
                        Dim oListitem As New ListItem
                        oListitem = Me.CBLiscrizioniAutonome.Items(i)
                        valori = oListitem.Value.Split(",")
                        TipoPersonaId = CInt(valori(0))

                        If valori(1) = 0 And oListitem.Selected Then
                            .AssociaTipologiaPersona(.Id, TipoPersonaId)
                        ElseIf valori(1) > 0 And Not oListitem.Selected Then
                            .DeAssociaTipologiaPersona(.Id, TipoPersonaId)
                        End If
                    Next
                    'Faccio ora l'upload della foto !

                    Dim oFile As New COL_File
                    Dim OLDpath, TempPath, NewPath As String

                    If Not Me.FILElogo.Value Is "" Then
                        Try
                            OLDpath = Server.MapPath("./../images/Organizzazione/temp/" & oFile.FileNameOnly(Me.FILElogo.PostedFile.FileName))
                            Delete.File_FM(OLDpath)
                            Try
                                TempPath = Server.MapPath("./../images/Organizzazione/temp/")
                                oFile.Upload(Me.FILElogo, TempPath)

                                Try
                                    Dim Estensione, NomeFile As String
                                    Dim strFileNameOnly As String = oFile.FileNameOnly(Me.FILElogo.PostedFile.FileName)
                                    Dim data As String = Now.ToString

                                    NomeFile = Left(strFileNameOnly, InStrRev(strFileNameOnly, ".") - 1)
                                    Estensione = Right(strFileNameOnly, Len(strFileNameOnly) - InStrRev(strFileNameOnly, ".") + 1)
                                    data = data.Replace(" ", "-")
                                    data = data.Replace("/", "-")
                                    data = data.Replace(".", "_")
                                    data = data.Replace(":", "_")
                                    NewPath = Server.MapPath("./../images/Organizzazione/" & NomeFile & "_" & data & Estensione)


                                    If oFile.ResizeLogo(TempPath & strFileNameOnly, NewPath, 43, 356) < 1 Then
                                        Delete.File_FM(TempPath & strFileNameOnly)
                                        Exit Try
                                    Else
                                        Delete.File_FM(TempPath & strFileNameOnly)
                                    End If

                                    Try
                                        oOrganizzazione.AssociaLogo(NomeFile & "_" & data & Estensione)
                                    Catch ex As Exception
                                        'errore nell'update del nome dell'immagine
                                    End Try
                                Catch ex As Exception
                                    'errore nel ridimensionamento 
                                End Try
                            Catch ex As Exception
                                'errore nell'upload
                            End Try
                        Catch ex As Exception
                            'errore nella cancellazione
                        End Try
                    End If

                End If
            End With

            Dim alertMSG As String
            If oOrganizzazione.Errore <> Errori_Db.DBError Then
                alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ModificaOK, Inserimento))
                Me.AggiornaApplication()
            Else
                alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ErroreModifica, Inserimento))
            End If

            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If

            Me.Reset_ToIniziale()
        Else
            Me.Reset_ToIniziale()
        End If
    End Sub

    Private Sub LNBmodifica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBmodifica.Click
        If Session("azione") = "modifica" Then

            Dim oOrganizzazione As New COL_Organizzazione
            Dim oTipoComunita As New COL_Tipo_Comunita
            oTipoComunita.ID = 0

            With oOrganizzazione
                .Id = Me.TXBid_n.Value
                oOrganizzazione.Estrai()
                Me.DDLIstituzioneForm.SelectedValue = .GetIstituzione

                .Cap = Me.TXBcap.Text
                .Citta = Me.TXBcitta.Text
                .Fax = Me.TXBfax.Text
                .HomePage = Me.TXBhomePage.Text
                .Indirizzo = Me.TXBindirizzo.Text
                .RagioneSociale = Me.TXBragioneSociale.Text
                .Telefono1 = Me.TXBtelefono1.Text
                .Telefono2 = Me.TXBtelefono2.Text
                .IsChiusa = CBool(Me.RBLisChiusa.SelectedValue = 1)
                .Stato.Id = Me.DDLstato.SelectedItem.Value
                .Provincia.Id = Me.DDLprovincia.SelectedItem.Value
                .IsFacolta = Me.CBXisFacolta.Checked
                .Modifica(Me.DDLIstituzioneForm.SelectedValue, oTipoComunita.ModelloDefault())


                Dim i, totale, TipoPersonaId As Integer
                For i = 0 To Me.CBLiscrizioniAutonome.Items.Count - 1
                    Dim valori() As String
                    Dim oListitem As New ListItem
                    oListitem = Me.CBLiscrizioniAutonome.Items(i)
                    valori = oListitem.Value.Split(",")
                    TipoPersonaId = CInt(valori(0))

                    If valori(1) = 0 And oListitem.Selected Then
                        .AssociaTipologiaPersona(.Id, TipoPersonaId)
                    ElseIf valori(1) > 0 And Not oListitem.Selected Then
                        .DeAssociaTipologiaPersona(.Id, TipoPersonaId)
                    End If
                Next
            End With
            Dim alertMSG As String
            If oOrganizzazione.Errore <> Errori_Db.DBError Then
                alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ModificaOK, Inserimento))
                Me.AggiornaApplication()
            Else
                alertMSG = oResource.getValue("Inserimento." & CType(Inserimento.ErroreModifica, Inserimento))
            End If

            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If

            Me.Reset_ToIniziale()
        Else
            Me.Reset_ToIniziale()
        End If
    End Sub
    Private Sub LNBannulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBannulla.Click
        Me.Reset_ToIniziale()
    End Sub
#End Region

    Private Sub Reset_ToIniziale()
        Me.PNLdgOrganizzazione.Visible = True
        Me.PNLDatiOrganizzazione.Visible = False
        Me.PNLmenu.Visible = True
        Me.PNLmenuInserimento.Visible = False
        Me.PNLautenticazioni.Visible = False
        Session("azione") = "loaded"
        Me.AzzeraTextBox()
        Me.Bind_Griglia()
    End Sub
    Private Sub Reset_ToModifica()
        Session("azione") = "modifica"
        Me.NascondiRigheStandard(False)
        Me.PNLautenticazioni.Visible = False
        Me.PNLmenuInserimento.Visible = True
        Me.oResource.setLinkButtonToValue(LNBautenticazioneIns, "inserisci", True, True)
        Try
            If Me.TXBid_n.Value > 0 Then
                Me.Bind_TipoAutenticazioni(Me.TXBid_n.Value)
            End If
        Catch ex As Exception

        End Try

        Me.LBiscrivimi.Visible = False
        Me.CBXiscrivimi.Visible = False
    End Sub

    Private Sub AggiornaApplication()
        Dim i, totale As Integer
        Dim oArray As String(,)
        Dim oDataset As New DataSet
        Dim oOrganizzazione As New COL_Organizzazione

        Me.Application.Lock()
        Try
            oDataset = oOrganizzazione.Elenca

            totale = oDataset.Tables(0).Rows.Count - 1
            For i = 0 To totale
                Dim oRow As DataRow

                oRow = oDataset.Tables(0).Rows(i)
                ReDim Preserve oArray(2, i)
                oArray(0, i) = oRow.Item("ORGN_id")

                If Exists.File(Server.MapPath("./../images/Organizzazione/" & oRow.Item("ORGN_Logo"))) Then
                    oArray(1, i) = "./images/Organizzazione/" & oRow.Item("ORGN_Logo")
                Else
                    oArray(1, i) = "./images/noLogo.gif"
                End If
                oArray(2, i) = oRow.Item("ORGN_homePage")
            Next
            Me.Application.Item("ArrayLogo") = oArray
        Catch ex As Exception
            Me.Application.Item("ArrayLogo") = Nothing
        End Try
        Me.Application.UnLock()
    End Sub
    Private Function NascondiRigheStandard(ByVal nascondi As Boolean)
        Me.TBR_0.Visible = Not (nascondi)
        Me.TBR_1.Visible = Not (nascondi)
        Me.TBR_2.Visible = Not (nascondi)
        Me.TBR_3.Visible = Not (nascondi)
        Me.TBR_4.Visible = Not (nascondi)
        Me.TBR_5.Visible = Not (nascondi)
        Me.TBR_6.Visible = Not (nascondi)
        Me.TBR_7.Visible = Not (nascondi)
        Me.TBR_8.Visible = Not (nascondi)
        Me.TBR_9.Visible = Not (nascondi)
        Me.TBR_12.Visible = Not (nascondi)
        Me.PNLmenuInserimento.Visible = Not (nascondi)
        Me.LBiscrivimi.Visible = Not (nascondi)
        Me.CBXiscrivimi.Visible = Not (nascondi)
        Me.TBR_modificaLogo.Visible = False
        Me.TBR_uploadLogo.Visible = False

        If Not nascondi Then
            If Session("azione") = "modifica" And Me.TBR_8.Visible Then
                Me.TBR_uploadLogo.Visible = False
                Me.TBR_modificaLogo.Visible = True
            ElseIf Session("azione") = "aggiungi" And Me.TBR_8.Visible Then
                Me.TBR_uploadLogo.Visible = True
                Me.TBR_modificaLogo.Visible = False
            End If
        End If

        Me.TBRiscrizioniAutonome.Visible = Not (nascondi)
        Me.TBR_11.Visible = (nascondi)
    End Function

#Region "Gestione Autenticazioni"
    Private Sub LNBassociaAutenticazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBassociaAutenticazione.Click
        Session("azione") = "aggiungiAutenticazione"
        Try
            If Me.TXBid_n.Value > 0 Then
                Me.Bind_TipoAutenticazioni(Me.TXBid_n.Value)
                Me.Bind_Autenticazioni(Me.TXBid_n.Value)
            Else
                Me.DGautenticazione.DataSource = Nothing
                Me.DGOrganizzazione.Visible = False
                Me.LBnoAutenticaz.Visible = True
            End If
        Catch ex As Exception

        End Try
        Me.PNLmenuInserimento.Visible = False
        Me.PNLautenticazioni.Visible = True

        Me.TXBparametro_1.Text = ""
        Me.TXBparametro_2.Text = ""
        Me.TXBparametro_3.Text = ""
        Me.TXBparametro_4.Text = ""
        Me.TXBdescrizione.Text = ""
        Me.NascondiRigheStandard(True)
    End Sub

    Private Sub LNBautenticazioneIns_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBautenticazioneIns.Click
        Dim oOrganizzazione As New COL_Organizzazione
        oOrganizzazione.Id = Me.TXBid_n.Value

        If Session("azione") = "aggiungiAutenticazione" Then
            Try
                oOrganizzazione.AssociaAutenticazione(Me.DDLtipoAutenticazione.SelectedValue, Me.TXBdescrizione.Text, Me.TXBparametro_1.Text, Me.TXBparametro_2.Text, Me.TXBparametro_3.Text, Me.TXBparametro_4.Text)
                Me.NascondiRigheStandard(False)
            Catch ex As Exception
                Me.NascondiRigheStandard(False)
            End Try
            Session("azione") = "modifica"
        ElseIf Session("azione") = "modificaAutenticazione" Then
            Try
                oOrganizzazione.ModificaDatiAutenticazione(Me.HDNlkao_ID.Value, Me.DDLtipoAutenticazione.SelectedValue, Me.TXBdescrizione.Text, Me.TXBparametro_1.Text, Me.TXBparametro_2.Text, Me.TXBparametro_3.Text, Me.TXBparametro_4.Text)
                Me.NascondiRigheStandard(False)
            Catch ex As Exception
                Me.NascondiRigheStandard(False)
            End Try
            Session("azione") = "modifica"
            Me.oResource.setLinkButtonToValue(LNBautenticazioneIns, "inserisci", True, True)
        Else
            Me.NascondiRigheStandard(False)
        End If
        Me.Reset_ToModifica()
    End Sub
    Private Sub LNBindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietro.Click
        Me.Reset_ToModifica()
    End Sub


#End Region
    Private Sub BTNeliminaLogo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNeliminaLogo.Click
        Dim oOrganizzazione As New COL_Organizzazione

        Try
            oOrganizzazione.Id = Me.TXBid_n.Value
            oOrganizzazione.AssociaLogo()
            Me.BTNeliminaLogo.Visible = False
            Me.IMGlogo.ImageUrl = "./../images/nologo.gif"
            Me.AggiornaApplication()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BTNaggiorna_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaggiorna.Click

        Try
            Dim Changed As Boolean = False
            Dim oFile As New COL_File
            Dim OLDpath, TempPath, NewPath As String
            Dim oOrganizzazione As New COL_Organizzazione

            oOrganizzazione.Id = Me.TXBid_n.Value
            oOrganizzazione.Estrai()

            If Not Me.FILElogoModifica.Value Is "" Then
                Try
                    OLDpath = Server.MapPath("./../images/Organizzazione/" & oOrganizzazione.Logo)
                    If OLDpath <> "" Then
                        Delete.File_FM(OLDpath)
                    End If
                    Try
                        TempPath = Server.MapPath("./../images/Organizzazione/temp/")
                        oFile.Upload(Me.FILElogoModifica, TempPath)

                        Try
                            Dim Estensione, NomeFile As String
                            Dim strFileNameOnly As String = oFile.FileNameOnly(Me.FILElogoModifica.PostedFile.FileName)
                            Dim data As String = Now.ToString

                            NomeFile = Left(strFileNameOnly, InStrRev(strFileNameOnly, ".") - 1)
                            Estensione = Right(strFileNameOnly, Len(strFileNameOnly) - InStrRev(strFileNameOnly, ".") + 1)
                            data = data.Replace(" ", "-")
                            data = data.Replace("/", "-")
                            data = data.Replace(".", "_")
                            data = data.Replace(":", "_")
                            NewPath = Server.MapPath("./../images/Organizzazione/" & NomeFile & "_" & data & Estensione)


                            If oFile.ResizeLogo(TempPath & strFileNameOnly, NewPath, 43, 356) < 1 Then
                                Delete.File_FM(TempPath & strFileNameOnly)
                                Exit Try
                            Else
                                Delete.File_FM(TempPath & strFileNameOnly)
                            End If

                            Try
                                oOrganizzazione.AssociaLogo(NomeFile & "_" & data & Estensione)
                                If oOrganizzazione.Errore = Errori_Db.None Then
                                    Changed = True
                                    Me.IMGlogo.ImageUrl = "./../images/Organizzazione/" & oOrganizzazione.Logo

                                    Me.AggiornaApplication()
                                End If
                            Catch ex As Exception
                                'errore nell'update del nome dell'immagine
                            End Try
                        Catch ex As Exception
                            'errore nel ridimensionamento 
                        End Try
                    Catch ex As Exception
                        'errore nell'upload
                    End Try
                Catch ex As Exception
                    'errore nella cancellazione
                End Try
            End If
            If Changed Then
                Me.BTNeliminaLogo.Visible = True
            End If
        Catch ex As Exception

        End Try


    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property
End Class