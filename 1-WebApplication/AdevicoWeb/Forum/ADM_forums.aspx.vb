Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Forum
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class ADM_forums
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

#Region "private"
    Private _PageUtility As OLDpageUtility
    Public ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
#End Region

    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    Private Enum StringaAbilita
        Abilita = 1
        Disabilita = 0
    End Enum


#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLforums As System.Web.UI.WebControls.Panel
    '   Protected WithEvents LBtitoletto As System.Web.UI.WebControls.Label
    Protected WithEvents TXBnomeForum As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents Requiredfieldvalidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents TXBDescrizione As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents DDLruoloDefault As System.Web.UI.WebControls.DropDownList
    Protected WithEvents PNLcreaModifica As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents DGforums As System.Web.UI.WebControls.DataGrid
    Protected WithEvents HDfrum_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents IMBmodifica As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IMBcancella As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LBnoforum As System.Web.UI.WebControls.Label
    Protected WithEvents HDfrumID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table
    Protected WithEvents PNLfiltri As System.Web.UI.WebControls.Panel
    Protected WithEvents LBavviso As System.Web.UI.WebControls.Label
    Protected WithEvents BTNok As System.Web.UI.WebControls.Button
    Protected WithEvents PNLavviso As System.Web.UI.WebControls.Panel

    Protected WithEvents PNLgestioneIscritti As System.Web.UI.WebControls.Panel
    Protected WithEvents CHBmoderato As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LBnomeforum_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdescrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBruoloDefault_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBforumModerato_t As System.Web.UI.WebControls.Label
#Region "controllie tabs"
    Protected WithEvents CTRLgestione As Comunita_OnLine.UC_GestioneiscrittiForum
    Protected WithEvents CTRLaggiungi As Comunita_OnLine.UC_AggiungiUtenteForum
   Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip
#End Region

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Protected WithEvents LBselezioneForum_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBLselezioneForum As System.Web.UI.WebControls.RadioButtonList
#Region "Pannello Menu"

    Protected WithEvents PNLmenuIniziale As System.Web.UI.WebControls.Panel
    Protected WithEvents IMpipe As System.Web.UI.WebControls.Image
    Protected WithEvents LKBindietro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBcrea As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLmenuInserimento As System.Web.UI.WebControls.Panel
    Protected WithEvents IMpipe1 As System.Web.UI.WebControls.Image
    Protected WithEvents LNBindietroFromInserimento As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBsalva As System.Web.UI.WebControls.LinkButton
#End Region

    Protected WithEvents PNLmenuUtenti As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaRuolo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBmodificaRuolo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLmenuAggiungiUtenti As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBaggiungiUtente As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBindietroToLista As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLmenuInserimentoUtenti As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBindietroToAggiungi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBassociaUtenti As System.Web.UI.WebControls.LinkButton

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        'inizio pagina
        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        Dim oServizio As New UCServices.Services_Forum
        If Not Page.IsPostBack Then
            Me.SetupInternazionalizzazione()
            Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
            oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
        Else
            If Me.ViewState("PermessiAssociati") = "" Then
                Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
            End If
            oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
        End If

        If oServizio.AccessoForum Or oServizio.GestioneForum Then
            Me.PNLpermessi.Visible = False
            Try

                If Not Page.IsPostBack Then
                    Bind_Ruoli_Forum()
                    Bind_Griglia_Forum()
                    'Bind_TipoRuoloComunita()
                    Me.PNLforums.Visible = True
                    Me.ViewState("intCurPage") = 0
                    Me.ViewState("intAnagrafica") = -1
                    'Me.LKBtutti.CssClass = "lettera_Selezionata"
                    ' Me.SetStartupScripts()
                Else
                    If Page.IsPostBack() Then
                        If Me.PNLgestioneIscritti.Visible = True And Me.CTRLgestione.Visible = True Then
                            Me.CTRLgestione.SetupControllo(False)
                        End If
                    End If
                End If
                If oServizio.GestioneForum And Me.PNLforums.Visible = True Then
                    Me.LKBcrea.Visible = True
                End If

            Catch ex As Exception
                Me.PNLcontenuto.Visible = False
                Me.PNLpermessi.Visible = True
            End Try
        Else
            Me.PNLcontenuto.Visible = False
            Me.PNLpermessi.Visible = True
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
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            Response.End()

            Return True
        Else
            Try
                If Session("idComunita") <= 0 Then
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
        Me.Response.Redirect("./../Comunita/EntrataComunita.aspx", True)

    End Sub


#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_ADM_forum"
        oResource.Folder_Level1 = "Forum"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")

            .setLabel(LBNopermessi)
            .setLabel(LBnoforum)

            .setLinkButton(LKBcrea, True, True)
            .setLabel(LBnomeforum_t)
            .setLabel(LBdescrizione_t)
            .setLabel(LBruoloDefault_t)
            .setLabel(LBforumModerato_t)
            .setLabel(LBavviso)

            .setLinkButton(Me.LNBindietroFromInserimento, True, True)
            .setButton(BTNok)
            .setLinkButton(LKBindietro, True, True)
            .setCheckBox(CHBmoderato)

            oResource.setHeaderDatagrid(Me.DGforums, 2, "FRUM_name", True)
            oResource.setHeaderDatagrid(Me.DGforums, 3, "FRUM_DateCreated", True)
            oResource.setHeaderDatagrid(Me.DGforums, 4, "FRUM_Ntopics", True)
            oResource.setHeaderDatagrid(Me.DGforums, 5, "FRUM_NPost", True)
            oResource.setHeaderDatagrid(Me.DGforums, 6, "TotIscritti", True)

            .setLabel(LBselezioneForum_t)
            .setRadioButtonList(Me.RBLselezioneForum, -1)
            .setRadioButtonList(Me.RBLselezioneForum, 0)
            .setRadioButtonList(Me.RBLselezioneForum, 1)

            .setLinkButton(LNBannullaRuolo, True, True)
            .setLinkButton(LNBmodificaRuolo, True, True)
            .setLinkButton(LNBaggiungiUtente, True, True)
            .setLinkButton(LNBindietroToLista, True, True)

            .setLinkButton(LNBassociaUtenti, True, True)
            .setLinkButton(LNBindietroToAggiungi, True, True)
        End With
    End Sub
#End Region

    Private Function GetPermessiForPage(ByVal Codex As String) As String
        Dim PermessiAssociati As String

        Try
            PermessiAssociati = Permessi(Codex, Me.Page)

            If (PermessiAssociati = "") Then
                PermessiAssociati = "00000000000000000000000000000000"
            End If
        Catch ex As Exception
            PermessiAssociati = "00000000000000000000000000000000"
        End Try

        Return PermessiAssociati
    End Function

#Region "Bind Dati"
    Public Sub Bind_RuoliForumLimitato(Optional ByVal RuoloForum As Integer = -1)
        'carica solo i ruoli inferiori al proprio!!!!!!!!!!!!!!!!!!!!!!!!!!
        'serve per la modifica del rolo della persona e per la modifica del ruolo di default per l'iscrizione alla comunita
        '  Me.DDLruoloForum.Items.Clear() 'ddlper la modifica ruolo
        Me.DDLruoloDefault.Items.Clear() 'modifica
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oTipoRuoloForum As New COL_TipoRuoloForum
            oDataset = oTipoRuoloForum.Elenca(Session("LinguaID"))

            Totale = oDataset.Tables(0).Rows.Count()
            If Totale > 0 Then
                Totale = Totale - 1
                For i = 0 To Totale
					Dim oListItem As New ListItem
                    If IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRF_nome")) Then
                        oListItem.Text = "--"
                    ElseIf RuoloForum > -1 Then
                        If oDataset.Tables(0).Rows(i).Item("TPRF_ID") > RuoloForum Then
                            oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRF_ID")
                            oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRF_nome")
                            Me.DDLruoloDefault.Items.Add(oListItem)
                        End If

                    ElseIf oDataset.Tables(0).Rows(i).Item("TPRF_ID") > Session("RuoloForum") Then
                        oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRF_ID")
                        oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRF_nome")
                        Me.DDLruoloDefault.Items.Add(oListItem)
                    End If
                Next
                Me.LNBsalva.Enabled = True
            Else
                Me.DDLruoloDefault.Items.Add(New ListItem("< nessun ruolo >", -1))
                Me.LNBsalva.Enabled = False
            End If
        Catch ex As Exception
            Me.DDLruoloDefault.Items.Add(New ListItem("< nessun ruolo >", -1))
            Me.LNBsalva.Enabled = False
        End Try

        oResource.setDropDownList(Me.DDLruoloDefault, -1)
    End Sub
    Public Sub Bind_Ruoli_Forum()
        'carica le ddl per la creazione del forum e per la ricerca in gestione iscritti 
        Me.DDLruoloDefault.Items.Clear() 'ddl per la creazione
        ' Me.DDLruoloForumRic.Items.Clear() 'ddl per la ricerca

        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oTipoRuoloForum As New COL_TipoRuoloForum
            oDataset = oTipoRuoloForum.Elenca(Session("LinguaID"))

            Totale = oDataset.Tables(0).Rows.Count()


            If Totale > 0 Then
                Totale = Totale - 1
                For i = 0 To Totale
					Dim oListItem As New ListItem
                    If IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRF_nome")) Then
                        oListItem.Text = "--"
                    Else
                        oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRF_nome")
                    End If
                    oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRF_ID")
                    Me.DDLruoloDefault.Items.Add(oListItem)
                Next
            Else
                Me.DDLruoloDefault.Items.Add(New ListItem("< nessun ruolo >", -1))
                Me.LNBsalva.Enabled = False
            End If
        Catch ex As Exception
            Me.DDLruoloDefault.Items.Add(New ListItem("< nessun ruolo >", -1))
        End Try
        oResource.setDropDownList(Me.DDLruoloDefault, -1)
    End Sub


    Private Sub Bind_Griglia_Forum()
        Dim CMNT_ID, RLPC_id As Integer
        Dim oDataset As DataSet
        Dim i, totale, sottratte As Integer
        Try
            Dim oForum As New COL_Forums


            Dim oServizio As New UCServices.Services_Forum
            Dim PermessiAssociati As String
            Try
                PermessiAssociati = Permessi(UCServices.Services_Forum.Codex, Me.Page)
                If Not (PermessiAssociati = "") Then
                    oServizio.PermessiAssociati = PermessiAssociati
                End If
            Catch ex As Exception
                oServizio.PermessiAssociati = "00000000000000000000000000000000"
            End Try

            CMNT_ID = Session("idComunita")
            RLPC_id = Session("RLPC_id")


            oDataset = oForum.ElencaByComunitaIscritto(Session("objPersona").id, CMNT_ID, RLPC_id, CType(Me.RBLselezioneForum.SelectedValue, Main.FiltroArchiviazione), True)


            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                Me.DGforums.Visible = False
                Me.LBnoforum.Visible = True
                oResource.setLabel_To_Value(LBnoforum, "LBnoforum.text")
            Else
                Me.LBnoforum.Visible = False
                Me.DGforums.Visible = True
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    If oRow.Item("RuoloForum") <> Main.RuoloForumStandard.Amministratore And oRow.Item("RuoloForum") <> Main.RuoloForumStandard.Moderatore Then
                        ' se non sono admin o moderatore, ma ho il permesso di amministrare il servizio non tolgo nulla...
                        If Not oServizio.GestioneForum Then
                            oDataset.Tables(0).Rows(i).Delete()
                            sottratte += 1
                        End If
                    End If
                Next

                If totale = sottratte Then 'se ho tolto un numero pari di righe al totale nn ce ne sono +
                    ' al posto della datagrid mostro un messaggio!
                    Me.LBnoforum.Visible = True
                    'Me.LBnoforum.Text = "Nessun Forum da amministrare"
                    Me.DGforums.Visible = False
                    oResource.setLabel_To_Value(LBnoforum, "LBnoforum.text")
                Else
                    Dim oDataview As DataView
                    oDataview = oDataset.Tables(0).DefaultView
                    If viewstate("SortExspression") = "" Then
                        viewstate("SortExspression") = "FRUM_name"
                        viewstate("SortDirection") = "desc"
                    End If
                    oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                    DGforums.DataSource = oDataview
                    DGforums.DataBind()
                End If

                'If (totale - sottratte) > Me.DGforums.PageSize Then
                '    Me.DGforums.AllowPaging = True
                'Else
                '    Me.DGforums.AllowPaging = False
                'End If
            End If
        Catch ex As Exception 'se c'è qualche errore nascondo la DG e mostro messaggio di errore
            Me.LBnoforum.Visible = True
            oResource.setLabel_To_Value(LBnoforum, "LBnoforum.text")
        End Try
    End Sub

#End Region

    Private Sub HideAllForm()
        Me.PNLmenuIniziale.Visible = False
        Me.PNLmenuInserimento.Visible = False
        Me.PNLmenuUtenti.Visible = False
        Me.PNLmenuAggiungiUtenti.Visible = False
        Me.PNLmenuInserimentoUtenti.Visible = False
        Me.PNLcreaModifica.Visible = False
        Me.PNLgestioneIscritti.Visible = False
        Me.PNLforums.Visible = False
    End Sub

    Private Sub ResetToManagement()
        Me.HideAllForm()
        Me.PNLforums.Visible = True
        Me.PNLmenuIniziale.Visible = True
        Me.LKBindietro.Visible = False


        Dim oServizio As New UCServices.Services_Forum
        If Me.ViewState("PermessiAssociati") = "" Then
            Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
        End If
        oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
        Me.LKBcrea.Visible = oServizio.GestioneForum
        'oResource.setLabel(LBTitolo)
        Me.Master.ServiceTitle = oResource.getValue("LBTitolo.text")
        Session("Azione") = "loaded"
    End Sub

    Private Sub ResetToInitForm()
        Me.HideAllForm()
        Me.PNLforums.Visible = True
        Me.PNLmenuInserimento.Visible = True
    End Sub
    Private Sub ShowModificaInserimento_Forum()
        Me.HideAllForm()
        Me.PNLcreaModifica.Visible = True
        Me.PNLmenuInserimento.Visible = True
    End Sub
    Private Sub ResetToAddUser()
        Me.HideAllForm()
        Me.PNLmenuAggiungiUtenti.Visible = False
        Me.PNLgestioneIscritti.Visible = True
        Me.PNLmenuInserimentoUtenti.Visible = True
    End Sub
    Private Sub ResetToTabAddUser()
        Me.HideAllForm()
        Me.PNLmenuInserimentoUtenti.Visible = False
        Me.PNLgestioneIscritti.Visible = True
        Me.PNLmenuAggiungiUtenti.Visible = True
    End Sub

#Region "Gestione Griglia Forum"

    Private Sub DGforums_DataBinding(sender As Object, e As EventArgs) Handles DGforums.DataBinding

    End Sub
    Private Sub DGforums_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGforums.ItemDataBound
        Dim i As Integer
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
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
                    If Me.DGforums.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                'oLinkbutton.CssClass = "ForumNW_RowHeader"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGforums, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGforums, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGforums.HeaderStyle.CssClass
                                oLabelAfter.Text = oLinkbutton.Text & " "
                                'oLinkbutton.Font.Name = "webdings"
                                'oLinkbutton.Font.Size = FontUnit.XSmall

                                If oSortDirection = "asc" Then
                                    '  oText = "5"
                                    oText = "<img src='./../images/dg/downForum.gif' id='Image_" & i & "' >"
                                    If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/downForum.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/downForum.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                    End If
                                    If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/downForum_over.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/downForum_over.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
                                    End If
                                Else
                                    '  oText = "6"
                                    oText = "<img src='./../images/dg/upForum.gif' id='Image_" & i & "' >"
                                    If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                    End If
                                    If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum_over.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum_over.gif';return true;")
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
                                'oLinkbutton.CssClass = "ForumNW_RowHeader"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGforums, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGforums.HeaderStyle.CssClass
                                oLabelAfter.Text = oLinkbutton.Text & " "
                                oLinkbutton.Text = "<img src='./../images/dg/upForum.gif' id='Image_" & i & "' >"
                                If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                    oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum.gif';return true;")
                                Else
                                    StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                    StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum.gif';return true;")
                                    oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                End If
                                If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                    oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum_over.gif';return true;")
                                Else
                                    StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                    StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/upForum_over.gif';return true;")
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
                    oResource.setPageDatagrid(Me.designerPlaceholderDeclaration, oLinkbutton)
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
                If e.Item.DataItem("FRUM_Hide") = 0 Then
                    If e.Item.ItemType = ListItemType.AlternatingItem Then
                        e.Item.CssClass = "ForumNW_RowAlternato"
                    Else
                        e.Item.CssClass = "ForumNW_RowNormal"
                    End If
                ElseIf e.Item.DataItem("FRUM_Hide") = 1 Then
                    e.Item.CssClass = "ForumNW_RowDisabilitate"
                End If

            Catch ex As Exception
                If e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ForumNW_RowAlternato"
                Else
                    e.Item.CssClass = "ForumNW_RowNormal"
                End If
            End Try


            Dim oCell As New TableCell

            ' Recupero Info relative al servizio.
            Dim oServizio As New UCServices.Services_Forum
            If Me.ViewState("PermessiAssociati") = "" Then
                Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
            End If
            oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")


            Try
                oCell = CType(e.Item.Cells(0), TableCell)
                Try
                    Dim oIMBmodifica As ImageButton
                    oIMBmodifica = oCell.FindControl("IMBmodifica")


                    If oServizio.GestioneForum = False And e.Item.DataItem("RuoloForum") <> Main.RuoloForumStandard.Amministratore Then 'solo l'amministratore del forum può modificare il forum
                        oIMBmodifica.Enabled = False
                        oResource.setImageButton_Datagrid(Me.DGforums, oIMBmodifica, "IMBmodifica", False, True, True, False)
                    Else
                        oIMBmodifica.Enabled = True
                        oResource.setImageButton_Datagrid(Me.DGforums, oIMBmodifica, "IMBmodifica", True, True, True, False)
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try

            Dim oIMBcancella As ImageButton
            Try
                Try
                    oIMBcancella = e.Item.Cells(1).FindControl("IMBcancella")
                    If oServizio.GestioneForum = False And e.Item.DataItem("RuoloForum") <> Main.RuoloForumStandard.Amministratore Then 'solo l'amministratore del forum può cancellare i forum
                        oIMBcancella.Enabled = False
                        oResource.setImageButton_Datagrid(Me.DGforums, oIMBcancella, "IMBcancella", False, True, True, True)
                    Else
                        oIMBcancella.Enabled = True
                        oResource.setImageButton_Datagrid(Me.DGforums, oIMBcancella, "IMBcancella", True, True, True, True)
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try

            'gestione iscritti
            Try
                Dim oLNBiscritti As LinkButton
                oLNBiscritti = e.Item.Cells(2).FindControl("LNBiscritti")

                If IsNothing(oLNBiscritti) = False Then
                    oResource.setLinkButton(oLNBiscritti, True, True)
                    If oServizio.GestioneForum = False And e.Item.DataItem("RuoloForum") <> Main.RuoloForumStandard.Amministratore And e.Item.DataItem("RuoloForum") <> Main.RuoloForumStandard.Moderatore Then 'solo l'amministratore del forum può cancellare i forum
                        oLNBiscritti.Enabled = False
                    Else
                        oLNBiscritti.Enabled = True
                    End If
                End If
            Catch ex As Exception

            End Try

            'abilita disabilita
            Try
                Dim oForum As New COL_Forums
                oForum.Id = e.Item.DataItem("FRUM_id")

                Dim oLNBnascondi As LinkButton
                oLNBnascondi = e.Item.Cells(2).FindControl("LNBnascondi")

                If Not IsNothing(oLNBnascondi) Then
                    If e.Item.DataItem("FRUM_Hide") = 0 Then
                        oResource.setLinkButtonToValue(oLNBnascondi, "abilita." & StringaAbilita.Disabilita, True, True)
                    Else
                        oResource.setLinkButtonToValue(oLNBnascondi, "abilita." & StringaAbilita.Abilita, True, True)
                    End If
                End If
            Catch ex As Exception

            End Try

            'entra
            Try
                Dim oLNBentra As LinkButton
                oLNBentra = e.Item.Cells(2).FindControl("LNBentra")
                If IsNothing(oLNBentra) = False Then
                    oLNBentra.Enabled = True
                    oResource.setLinkButton(oLNBentra, True, True)
                End If
            Catch ex As Exception

            End Try
            'Archivia
            Try
                Dim oLNBarchivia As LinkButton
                oLNBarchivia = e.Item.Cells(2).FindControl("LNBarchivia")
                If IsNothing(oLNBarchivia) = False Then
                    If e.Item.DataItem("FRUM_Archiviato") = 0 Then
                        Me.oResource.setLinkButtonToValue(oLNBarchivia, "NonArchiviato", True, True)
                    Else
                        Me.oResource.setLinkButtonToValue(oLNBarchivia, "Archiviato", True, True)
                    End If
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub DGforums_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGforums.ItemCommand
        Dim IdForum, IdRuolo As Integer

        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        Select Case e.CommandName
            Case "modifica"
                Dim oServizio As New UCServices.Services_Forum
                If Me.ViewState("PermessiAssociati") = "" Then
                    Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_Forum.Codex)
                End If
                oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
                IdForum = CInt(DGforums.DataKeys.Item(e.Item.ItemIndex))
                IdRuolo = CInt(DGforums.Items(e.Item.ItemIndex).Cells(7).Text)

                ' Verifico se ho i permessi per agire !
                If oServizio.GestioneForum Or IdRuolo = Main.RuoloForumStandard.Amministratore Then
                    Me.ShowModificaInserimento_Forum()
                    Dim oForum As New COL_Forums
                    'oResource.setLabel_To_Value(LBTitolo, "LBTitolo2.text")
                    Me.Master.ServiceTitle = oResource.getValue("LBTitolo2.text")
                    Try
                        Session("RuoloForum") = IdRuolo
                        Bind_RuoliForumLimitato(0)

                        oForum.Id = IdForum
                        Me.HDfrum_id.Value = oForum.Id
                        Session("Azione") = "modifica"

                        oResource.setLinkButtonToValue(Me.LNBsalva, "modifica", True, True)
                        oForum.Estrai()
                        Me.TXBnomeForum.Text = oForum.Name
                        Me.TXBDescrizione.Text = oForum.Description
                        Me.DDLruoloDefault.SelectedValue = oForum.TipoRuoloDefault
                        If oForum.Moderated = True Then
                            Me.CHBmoderato.Checked = True
                        Else
                            Me.CHBmoderato.Checked = False
                        End If
                    Catch ex As Exception
                        Me.ResetToInitForm()
                        Me.Bind_Griglia_Forum()
                    End Try
                Else
                    Me.Bind_Griglia_Forum()
                End If

            Case "GestioneIscritti"
                IdForum = CInt(DGforums.DataKeys.Item(e.Item.ItemIndex))
                IdRuolo = CInt(DGforums.Items(e.Item.ItemIndex).Cells(7).Text)
                Me.HideAllForm()
                Me.PNLgestioneIscritti.Visible = True
                Me.LKBcrea.Visible = False
                Me.LKBindietro.Visible = True
                Me.PNLmenuIniziale.Visible = True
                Dim oForum As New COL_Forums
                oForum.Id = IdForum
                oForum.Estrai()

                Dim Title As String
                Title = oResource.getValue("LBTitolo3.text")
                'oResource.setLabel_To_Value(LBTitolo, "LBTitolo3.text")
                'LBTitolo.Text = LBTitolo.Text.Replace("##", oForum.Name)
                Title = Title.Replace("##", oForum.Name)

                Me.Master.ServiceTitle = Title

                Try
                    Session("RuoloForum") = IdRuolo
                    Session("IdForum") = IdForum
                    Me.TBSmenu.SelectedIndex = 0
                    Me.CTRLgestione.SetupControllo(True, True)
                Catch ex As Exception

                End Try

            Case "Entra"
                Dim oForum As New COL_Forums
                Dim RuoloForum As String
                IdForum = CInt(DGforums.DataKeys.Item(e.Item.ItemIndex))
                oForum.Id = IdForum

                Try
                    oForum.Estrai()
                    If oForum.Errore = Errori_Db.None Then

                        RuoloForum = oForum.IscriviUtente(Session("RLPC_ID"), Main.RuoloForumStandard.Amministratore)

                        Session("IdForum") = IdForum
                        Session("ForumIsArchiviato") = oForum.isArchiviato
                        Session("NomeForum") = oForum.Name
                        Session("RuoloForum") = RuoloForum
                        Response.Redirect("./ForumThreads.aspx", True)
                    End If
                Catch ex As Exception

                End Try
            Case "Hide"
                Try
                    Dim oForum As New COL_Forums
                    oForum.Id = CInt(DGforums.DataKeys.Item(e.Item.ItemIndex))
                    oForum.AbilitaDisabilita()
                Catch ex As Exception

                End Try
                Me.Bind_Griglia_Forum()
            Case "elimina"
                Dim oForum As New COL_Forums
                Try
                    IdForum = CInt(DGforums.DataKeys.Item(e.Item.ItemIndex))
                    oForum.Id = IdForum
                    oForum.Elimina()
                Catch ex As Exception

                End Try

                Me.Bind_Griglia_Forum()
            Case Else
                Me.Bind_Griglia_Forum()
        End Select
    End Sub
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGforums.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = ViewState("SortExspression")
        oSortDirection = ViewState("SortDirection")
        ViewState("SortExspression") = e.SortExpression

        If LCase(e.SortExpression) = LCase(oSortExpression) Then
            If ViewState("SortDirection") = "asc" Then
                ViewState("SortDirection") = "desc"
            Else
                ViewState("SortDirection") = "asc"
            End If
        End If
        Bind_Griglia_Forum()
    End Sub
    Private Sub CambioPagina(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGforums.PageIndexChanged
        source.CurrentPageIndex = e.NewPageIndex
        Me.ViewState("intCurPage") = e.NewPageIndex
        Bind_Griglia_Forum()

    End Sub
    Protected Sub LNBArchivia_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim oForum As COL_Forums
            oForum.ModificaArchiviazione(sender.CommandArgument)

            Bind_Griglia_Forum()
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Operazioni su forum"
    Private Sub LNBindietroFromInserimento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietroFromInserimento.Click
        Me.ResetToManagement()
        Me.Bind_Griglia_Forum()

    End Sub

    Private Sub LNBsalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalva.Click
        Dim oForum As New COL_Forums
        Dim CMNT_ID As Integer
        Dim alertMSG As String = ""

        CMNT_ID = Session("idComunita")
        If Session("Azione") = "inserimento" Then
            If Me.DDLruoloDefault.SelectedItem.Value <> 0 Then
                oForum.Comunita.Id() = CMNT_ID
                oForum.Description = Me.TXBDescrizione.Text
                oForum.Name = Me.TXBnomeForum.Text
                oForum.TipoRuoloDefault = Me.DDLruoloDefault.SelectedItem.Value
                oForum.isArchiviato = False
                If Me.CHBmoderato.Checked = True Then
                    oForum.Moderated = 1
                Else
                    oForum.Moderated = 0
                End If


                Dim RLPC_id As Integer
                RLPC_id = Session("RLPC_ID")
                Try
                    oForum.Aggiungi(RLPC_id)
                    If oForum.Errore = Errori_Db.None Then
                        Dim oServiceNotification As New ForumNotificationUtility(Me.PageUtility)
                        oServiceNotification.NotifyAddForum(Me.PageUtility.WorkingCommunityID, oForum.Id, oForum.Name)

                        alertMSG = oResource.getValue("Forum.1")
                    Else
                        alertMSG = oResource.getValue("Forum.0")
                    End If
                Catch ex As Exception
                End Try
            Else
                alertMSG = oResource.getValue("Forum.-2")
            End If

            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            End If

            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
        ElseIf Session("Azione") = "modifica" Then
            With oForum
                .Id = Me.HDfrum_id.Value
                .Estrai()
                .Name = Me.TXBnomeForum.Text
                .Description = Me.TXBDescrizione.Text
                .TipoRuoloDefault = Me.DDLruoloDefault.SelectedItem.Value
                If Me.CHBmoderato.Checked = True Then
                    .Moderated = 1
                Else
                    .Moderated = 0
                End If

                Try
                    .Modifica()
                    If oForum.Errore = Errori_Db.None Then
                        alertMSG = oResource.getValue("Forum.2")
                    Else
                        alertMSG = oResource.getValue("Forum.0")
                    End If
                Catch ex As Exception

                End Try
            End With
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
        End If

        Me.PNLcreaModifica.Visible = False
        Me.PNLforums.Visible = True
        Me.PNLmenuIniziale.Visible = True
        Me.PNLmenuInserimento.Visible = False
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        'oResource.setLabel_To_Value(LBTitolo, "LBTitolo.text")
        Me.Master.ServiceTitle = oResource.getValue("LBTitolo.text")
        Bind_Griglia_Forum()
        Session("Azione") = "loaded"
    End Sub

    Private Sub LKBcrea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBcrea.Click
        Me.PNLforums.Visible = False
        Me.PNLcreaModifica.Visible = True

        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        Me.PNLmenuIniziale.Visible = False
        Me.PNLmenuInserimento.Visible = True
        'oResource.setLabel_To_Value(LBTitolo, "LBTitolo1.text")
        Me.Master.ServiceTitle = oResource.getValue("LBTitolo1.text")
        oResource.setLinkButtonToValue(Me.LNBsalva, "inserimento", True, True)
        Session("Azione") = "inserimento"
        puliscicampi()
    End Sub

    Public Sub puliscicampi()
        Me.TXBnomeForum.Text = ""
        Me.TXBDescrizione.Text = ""
        Me.DDLruoloDefault.SelectedValue = 3
    End Sub
#End Region

#Region "Tab"
    Private Sub TBSmenu_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        Select Case Me.TBSmenu.SelectedIndex
            Case 0
                Me.CTRLgestione.Visible = True
                Me.CTRLaggiungi.Visible = False
                Me.PNLmenuInserimento.Visible = False
                Me.PNLmenuIniziale.Visible = True
                Me.PNLmenuUtenti.Visible = False
                Me.PNLmenuAggiungiUtenti.Visible = False
                Me.PNLmenuInserimentoUtenti.Visible = False
                Me.CTRLgestione.SetupControllo(False, True)
            Case 1
                Me.CTRLgestione.Visible = False
                Me.CTRLaggiungi.Visible = True
                Me.PNLmenuInserimento.Visible = False
                Me.PNLmenuIniziale.Visible = False
                Me.PNLmenuUtenti.Visible = False
                Me.PNLmenuAggiungiUtenti.Visible = True
                Me.PNLmenuInserimentoUtenti.Visible = False
                Me.CTRLaggiungi.SetupControllo(True, True)
        End Select
    End Sub
#End Region

    Private Sub LKBindietro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LKBindietro.Click
        Me.ResetToManagement()
        Me.Bind_Griglia_Forum()
    End Sub


    Private Sub RBLselezioneForum_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLselezioneForum.SelectedIndexChanged
        Me.Bind_Griglia_Forum()
    End Sub

    Private Sub CTRLgestione_AggiornaMenu(ByVal showModifica As Boolean, ByVal abilitato As Boolean) Handles CTRLgestione.AggiornaMenu
        If showModifica Then
            Me.PNLmenuUtenti.Visible = True
            Me.PNLmenuIniziale.Visible = False
            Me.LNBmodificaRuolo.Enabled = abilitato
        Else
            Me.PNLmenuUtenti.Visible = False
            Me.PNLmenuIniziale.Visible = True
        End If
    End Sub

    Private Sub LNBannullaRuolo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaRuolo.Click
        Me.CTRLgestione.AnnullaModificaRuolo()
        Me.PNLmenuUtenti.Visible = False
        Me.PNLmenuIniziale.Visible = True
    End Sub

    Private Sub LNBmodificaRuolo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmodificaRuolo.Click
        Dim iResponse As UC_GestioneiscrittiForum.Inserimento
        Dim alertMSG As String

        iResponse = Me.CTRLgestione.ModificaRuolo
        alertMSG = oResource.getValue("Inserimento." & CType(iResponse, UC_GestioneiscrittiForum.Inserimento))
        If alertMSG <> "" Then
            alertMSG = alertMSG.Replace("'", "\'")
        End If
        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
        If iResponse = UC_GestioneiscrittiForum.Inserimento.ModificaAvvenuta Then
            Me.PNLmenuUtenti.Visible = False
            Me.PNLmenuIniziale.Visible = True
        End If
    End Sub


    Private Sub LNBaggiungiUtente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBaggiungiUtente.Click
        If Session("Azione") = "insert" Then
            Me.ResetToTabAddUser()
            Session("Azione") = "loaded"
            Me.CTRLaggiungi.SetupControllo(False, True)
        Else
            Dim iResponse As UC_AggiungiUtenteForum.Inserimento
            Dim alertMSG As String
            iResponse = Me.CTRLaggiungi.ShowIscrizione
            Me.PNLmenuInserimentoUtenti.Visible = True
            Me.PNLmenuAggiungiUtenti.Visible = False

            If iResponse = UC_AggiungiUtenteForum.Inserimento.SetupOk Then
                Session("Azione") = "insert"
            Else
                alertMSG = oResource.getValue("Inserimento." & CType(iResponse, UC_AggiungiUtenteForum.Inserimento))
                If alertMSG <> "" Then
                    alertMSG = alertMSG.Replace("'", "\'")
                End If
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
        End If
    End Sub

    Private Sub LNBindietroToLista_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietroToLista.Click
        Session("Azione") = "loaded"
        Me.ResetToManagement()
        Me.Bind_Griglia_Forum()
    End Sub

    Private Sub LNBindietroToAggiungi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietroToAggiungi.Click
        Me.ResetToTabAddUser()
        Session("Azione") = "loaded"
        Me.CTRLaggiungi.SetupControllo(False, False)
    End Sub

    Private Sub LNBassociaUtenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBassociaUtenti.Click
        If Session("Azione") = "insert" Then
            Dim iResponse As UC_AggiungiUtenteForum.Inserimento
            Dim alertMSG As String

            iResponse = Me.CTRLaggiungi.AssociaSelezionati
            alertMSG = oResource.getValue("Inserimento." & CType(iResponse, UC_AggiungiUtenteForum.Inserimento))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            End If
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")

            If iResponse = UC_AggiungiUtenteForum.Inserimento.InserimentoOk OrElse iResponse = UC_AggiungiUtenteForum.Inserimento.ModificaAvvenuta Then
                Me.ResetToTabAddUser()
                Session("Azione") = "loaded"
                Me.CTRLaggiungi.SetupControllo(False, True)
            End If
        Else
            Me.ResetToTabAddUser()
            Session("Azione") = "loaded"
            Me.CTRLaggiungi.SetupControllo(False, True)
        End If
    End Sub
End Class


'<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
'<HTML>
'	<head runat="server">
'		<title>Comunità On Line - Admin Forum</title>
'		<META http-equiv="Content-Type" content="text/html; charset=windows-1252"/>

'		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
'		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
'		<meta content="JavaScript" name="vs_defaultClientScript"/>

'		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
'	</HEAD>
'	<body>
'		 <form id="aspnetForm" runat="server">
'		 <%--<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>--%>
'			<table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">
'			    <tr>
'				    <td colspan="3" >
'				    <div>
'				        <%--<HEADER:CtrLHEADER id="Intestazione" runat="server" ShowNews="false"></HEADER:CtrLHEADER>	--%>
'				    </div>
'				    <br style="clear:both;" />
'				    </td>
'			    </tr>
'				<tr class="contenitore">
'					<td colSpan="3">
'						<table cellSpacing="0" cellPadding="0" width="900px" border="0">
'							<tr>
'								<td class="RigaTitolo" align="left">
'									<%--<asp:Label ID="LBTitolo" Runat="server">Gestione Forums</asp:Label>--%>
'								</td>
'							</tr>
'							<tr>
'								<td align="right">
'									<table align="right">
'										<tr>
'											<td>&nbsp;</td>
'											<td>

'											</td>
'										</tr>
'									</table>

'								</td>
'							</tr>
'							<tr>
'								<td align="center">

'								</td>
'							</tr>
'						</table>
'					</td>
'				</tr>
'				<tr class="contenitore">
'					<td colSpan="3"></td>
'				</tr>
'			</table>
'			<%--<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>--%>
'		</form>
'	</body>
'</HTML>