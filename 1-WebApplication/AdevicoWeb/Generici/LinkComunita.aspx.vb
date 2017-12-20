Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.UCServices

Imports Telerik.WebControls

Public Class LinkComunita
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager


    Private _Utility As OLDpageUtility
    Private ReadOnly Property Utility() As OLDpageUtility
        Get
            If IsNothing(_Utility) Then
                _Utility = New OLDpageUtility(Me.Context)
            End If
            Return _Utility
        End Get
    End Property

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
#Region "Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

    Private Enum TabSelezionato
        Lista = 0
        Gestione = 1
        ImportaLink = 2
        EsportaLink = 3
    End Enum

    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label

#Region "Pannelli menu"

    Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip

    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBaggiorna As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBnuovoFolderLink As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBnuovoLink As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBespandi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcomprimi As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLmenugestione As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBinserisci As System.Web.UI.WebControls.LinkButton


    Protected WithEvents PNLmanagement As System.Web.UI.WebControls.Panel

    Protected WithEvents LBfolder_t As System.Web.UI.WebControls.Label
    Protected WithEvents LNBdettagliDir As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBmodificaDir As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBeliminaDir As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBeliminaContenuto As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcreaDirectory As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcreaLink As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLconfermaCancella As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaElimina As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBelimina As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBreplicaElimina As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLmenuLink As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBdettagli As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBmodifica As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBeliminaLink As System.Web.UI.WebControls.LinkButton
#End Region

#Region "Lista Raccolta"
    Protected WithEvents PNLlista As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLlistaLink As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLdettagliLink As Comunita_OnLine.UC_DettagliLink
#Region "Treeview"
    Protected WithEvents PNLtreeView As System.Web.UI.WebControls.Panel

    Protected WithEvents RDTraccoltaLink As Telerik.WebControls.RadTreeView
#End Region

#End Region

#Region "Gestione Link"
    Protected WithEvents TBLgestione As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLgestioneLink As Comunita_OnLine.UC_GestioneLink
#End Region

#Region "Gesione Creazione Link"
    Protected WithEvents TBLcreaLink As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLcreaLink As Comunita_OnLine.UC_CreaLink
#End Region

#Region "Esporta Link"
    Protected WithEvents TBLesporta As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLesportaLink As Comunita_OnLine.UC_EsportaLink
    Protected WithEvents LNBesporta As System.Web.UI.WebControls.LinkButton
#End Region

#Region "Importa Link"
    Protected WithEvents TBLimporta As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLimportaLink As Comunita_OnLine.UC_ImportaLink
    Protected WithEvents LNBimporta As System.Web.UI.WebControls.LinkButton
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Me.SessioneScaduta() Then
            Exit Sub
        End If
        If Not Page.IsPostBack Then
            Try
                Dim oServizioRaccoltaLink As New Services_RaccoltaLink

                Me.SetupInternazionalizzazione()
                Try
                    If Not Page.IsPostBack Then
                        Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioRaccoltaLink.Codex)
                        oServizioRaccoltaLink.PermessiAssociati = Me.ViewState("PermessiAssociati")
                    Else
                        If Me.ViewState("PermessiAssociati") = "" Then
                            Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioRaccoltaLink.Codex)
                        End If
                        oServizioRaccoltaLink.PermessiAssociati = Me.ViewState("PermessiAssociati")
                    End If
                Catch ex As Exception
                    oServizioRaccoltaLink.PermessiAssociati = "00000000000000000000000000000000"
                End Try

                If oServizioRaccoltaLink.List Or oServizioRaccoltaLink.Admin Or oServizioRaccoltaLink.Moderate Then
                    Me.PNLcontenuto.Visible = True
                    Me.PNLpermessi.Visible = False
                    Me.Bind_MenuTab()
                    Me.Bind_RaccoltaLink()

                    Me.Utility.AddAction(Services_RaccoltaLink.ActionType.ViewLinks, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
                Else

                    Me.Utility.AddAction(Services_RaccoltaLink.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.SystemToUser)

                    Me.PNLcontenuto.Visible = False
                    Me.PNLpermessi.Visible = True
                End If
                Me.LNBcomprimi.Attributes.Add("onclick", "CollapseAll();return false;")
                Me.LNBespandi.Attributes.Add("onclick", "ExpandAll();return false;")
            Catch ex As Exception

            End Try
        End If
    End Sub

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
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Me.Response.Redirect(PageUtility.GetDefaultLogoutPage, True)
            Return True
        Else
            Try
                If Session("Limbo") = True Then
                    Return False
                Else
                    Dim CMNT_ID As Integer = 0
                    Try
                        CMNT_ID = Session("idComunita")
                    Catch ex As Exception
                        CMNT_ID = 0
                    End Try

                    If CMNT_ID <= 0 Then
                        Me.ExitToLimbo()
                        Return True
                    End If
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
        Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultLogonPage)
    End Sub


#Region "Gestione Permessi"
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
    Private Sub Bind_MenuTab()
        Dim oServizioRaccoltaLink As New Services_RaccoltaLink

        Try
            If Me.ViewState("PermessiAssociati") = "" Then
                Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioRaccoltaLink.Codex)
            End If
            oServizioRaccoltaLink.PermessiAssociati = Me.ViewState("PermessiAssociati")

        Catch ex As Exception
            oServizioRaccoltaLink.PermessiAssociati = "00000000000000000000000000000000"
        End Try

        Me.TBSmenu.Tabs(TabSelezionato.ImportaLink).Visible = False
        Me.TBSmenu.Tabs(TabSelezionato.EsportaLink).Visible = False
        Me.TBSmenu.Tabs(TabSelezionato.Gestione).Visible = False
        Me.LNBnuovoFolderLink.Visible = False
        Me.LNBnuovoLink.Visible = False

        With oServizioRaccoltaLink
            Me.LNBnuovoFolderLink.Visible = (.Admin Or .Moderate Or .AddLink)
            Me.LNBnuovoLink.Visible = (.Admin Or .Moderate Or .AddLink)
            Me.TBSmenu.Tabs(TabSelezionato.ImportaLink).Visible = (.ImportLink Or .Admin Or .Moderate)
            Me.TBSmenu.Tabs(TabSelezionato.EsportaLink).Visible = (.ExportLink Or .Admin Or .Moderate)
            Me.TBSmenu.Tabs(TabSelezionato.Gestione).Visible = (.Moderate Or .Admin)


            Me.LNBcreaLink.Visible = (.Admin Or .Moderate Or .AddLink)
            Me.LNBcreaDirectory.Visible = (.Admin Or .Moderate Or .AddLink)
            ' Me.LNBmodificaDir.Visible = (.Admin Or .Moderate Or .ChangeLink)
        End With

        Dim i As Integer = 1
        If Me.TBSmenu.Tabs(TabSelezionato.ImportaLink).Visible Then
            i += 1
        End If
        If Me.TBSmenu.Tabs(TabSelezionato.EsportaLink).Visible Then
            i += 1
        End If
        If Me.TBSmenu.Tabs(TabSelezionato.Gestione).Visible Then
            i += 1
        End If

        If i = 4 Then
            Me.TBSmenu.Width = System.Web.UI.WebControls.Unit.Point(600)
        ElseIf i = 3 Then
            Me.TBSmenu.Width = System.Web.UI.WebControls.Unit.Point(450)
        ElseIf i = 2 Then
            Me.TBSmenu.Width = System.Web.UI.WebControls.Unit.Point(300)
        ElseIf i = 1 Then
            Me.TBSmenu.Width = System.Web.UI.WebControls.Unit.Point(160)
        End If
    End Sub
#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_Raccolta_Link"
        oResource.Folder_Level1 = "Generici"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
            .setLabel(Me.LBNopermessi)
            TBSmenu.Tabs(TabSelezionato.Lista).Text = .getValue("TABlista.Text")
            TBSmenu.Tabs(TabSelezionato.Lista).ToolTip = .getValue("TABlista.ToolTip")
            TBSmenu.Tabs(TabSelezionato.Gestione).Text = .getValue("TABgestione.Text")
            TBSmenu.Tabs(TabSelezionato.Gestione).ToolTip = .getValue("TABgestione.ToolTip")
            TBSmenu.Tabs(TabSelezionato.ImportaLink).Text = .getValue("TABimporta.Text")
            TBSmenu.Tabs(TabSelezionato.ImportaLink).ToolTip = .getValue("TABimporta.ToolTip")
            TBSmenu.Tabs(TabSelezionato.EsportaLink).Text = .getValue("TABesporta.Text")
            TBSmenu.Tabs(TabSelezionato.EsportaLink).ToolTip = .getValue("TABesporta.ToolTip")

            .setLinkButton(Me.LNBesporta, True, True)
            .setLinkButton(Me.LNBimporta, True, True)

            .setLinkButton(Me.LNBespandi, True, True)
            .setLinkButton(Me.LNBcomprimi, True, True)
            .setLinkButton(Me.LNBinserisci, True, True)
            .setLinkButton(Me.LNBesporta, True, True)
            .setLinkButton(Me.LNBaggiorna, True, True)
            .setLinkButton(Me.LNBnuovoFolderLink, True, True)
            .setLinkButton(Me.LNBnuovoLink, True, True)
            .setLinkButton(Me.LNBindietro, True, True)

            .setLinkButton(Me.LNBdettagli, True, True)
            .setLinkButton(Me.LNBmodifica, True, True)
            .setLinkButton(Me.LNBeliminaLink, True, True, , True)

            .setLabel(Me.LBfolder_t)
            .setLinkButton(Me.LNBdettagliDir, True, True)
            .setLinkButton(Me.LNBmodificaDir, True, True)
            .setLinkButton(Me.LNBeliminaDir, True, True, , True)
            .setLinkButton(Me.LNBeliminaContenuto, True, True, , True)
            .setLinkButton(Me.LNBcreaDirectory, True, True)
            .setLinkButton(Me.LNBcreaLink, True, True)


            .setLinkButton(Me.LNBannullaElimina, True, True)
            .setLinkButton(Me.LNBreplicaElimina, True, True)
            .setLinkButton(Me.LNBelimina, True, True)
        End With
    End Sub
#End Region

#Region "Bind Dati"
    Private Sub Bind_RaccoltaLink()
        Dim oRaccoltaLink As New COL_RaccoltaLink
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona
        oPersona = Session("objPersona")
        Try
            oDataset = oRaccoltaLink.ElencaByComunita(Session("idComunita"), oPersona.Id)

            Me.RDTraccoltaLink.Nodes.Clear()

            Dim nodeRoot As New RadTreeNode
            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "folder.gif"
            nodeRoot.Value = -1
            nodeRoot.Category = True
            nodeRoot.Selected = True
            nodeRoot.Checked = True
            Try
                nodeRoot.Text = oResource.getValue("oRootNode.Text")
                If nodeRoot.Text = "" Then
                    nodeRoot.Text = "Raccolta Link"
                End If
            Catch ex As Exception
                nodeRoot.Text = "Raccolta Link"
            End Try
            Try
                nodeRoot.ToolTip = oResource.getValue("oRootNode.ToolTip")
                If nodeRoot.ToolTip = "" Then
                    nodeRoot.ToolTip = "Raccolta Link"
                End If
            Catch ex As Exception
                nodeRoot.Text = "Raccolta Link"
            End Try

            Me.RDTraccoltaLink.Nodes.Add(nodeRoot)
            If oDataset.Tables(0).Rows.Count = 0 Then
                Me.GeneraNoNode(Me.RDTraccoltaLink)
            Else
                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("RLNK_ID"), oDataset.Tables(0).Columns("RLNK_Padre_Id"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("RLNK_Padre_Id") = 0 Then
                        Dim node As RadTreeNode = CreateNode(dbRow, True)
                        nodeRoot.Nodes.Add(node)
                        RecursivelyPopulate(dbRow, node)
                    End If
                Next dbRow
            End If
        Catch ex As Exception
            Me.RDTraccoltaLink.Nodes.Clear()
            Me.GeneraNoNode(Me.RDTraccoltaLink)
        End Try
    End Sub
#End Region

#Region "Gestione Treeview"
    Private Sub RecursivelyPopulate(ByVal dbRow As DataRow, ByVal node As RadTreeNode)
        Dim oData, oDataAttuale As DateTime
        Dim childRow As DataRow

        For Each childRow In dbRow.GetChildRows("NodeRelation")
            Dim childNode As RadTreeNode = CreateNode(childRow, False)
            node.Nodes.Add(childNode)
            RecursivelyPopulate(childRow, childNode)
        Next childRow
    End Sub

    Private Function CreateNode(ByVal dbRow As DataRow, ByVal expanded As Boolean) As RadTreeNode
        Dim node As New RadTreeNode
        Dim RLNK_CMNT_ID, RLNK_ID As Integer
        Dim RLNK_Nome, RLNK_Url, RLNK_Descrizione As String
        Dim RLNK_Modified, RLNK_Created As DateTime
        Dim RLNK_isCartella As Boolean

        If IsDBNull(dbRow.Item("RLNK_CMNT_ID")) Then
            RLNK_CMNT_ID = -1
        Else
            RLNK_CMNT_ID = dbRow.Item("RLNK_CMNT_ID")
        End If
        RLNK_ID = dbRow.Item("RLNK_ID")
        RLNK_Nome = dbRow.Item("RLNK_Nome")
        If IsDBNull(dbRow.Item("RLNK_Url")) Then
            RLNK_Url = ""
        Else
            RLNK_Url = dbRow.Item("RLNK_Url")
        End If
        If IsDBNull(dbRow.Item("RLNK_Descrizione")) Then
            RLNK_Descrizione = ""
        Else
            RLNK_Descrizione = dbRow.Item("RLNK_Descrizione")
        End If

        RLNK_isCartella = dbRow.Item("RLNK_isCartella")
        If IsDBNull(dbRow.Item("RLNK_Created")) = False Then
            RLNK_Created = dbRow.Item("RLNK_Created")
        End If
        If IsDBNull(dbRow.Item("RLNK_Modified")) = False Then
            RLNK_Modified = dbRow.Item("RLNK_Modified")
        End If

        node.ToolTip = RLNK_Nome
        node.Value = RLNK_ID
        node.Expanded = expanded
        node.Category = RLNK_ID
        node.Checkable = False

        If RLNK_isCartella = False Then
            node.ImageUrl = "html.gif"

            'RLNK_Nome = RLNK_Nome &  Me.GenerateURLFromPath(CMNT_REALpath, "Dettagli", "dettagli" & "," & CMNT_IsChiusa & ",1", "Visualizza i dettagli  relativi alla comunità.")
            node.Target = "_blank"
            If InStr(RLNK_Url, "http://") > 0 Then
                node.NavigateUrl = RLNK_Url
            Else
                node.NavigateUrl = "http://" & RLNK_Url
            End If
            '  RLNK_Nome = RLNK_Nome & Me.GenerateURLFromPath(CMNT_REALpath, "Aggiungi", "dettagli" & "," & CMNT_IsChiusa & ",1", "Aggingi un nuovo link.")
        Else
            node.ImageUrl = "folder.gif"
            node.ImageExpandedUrl = "FolderOpen.gif"
        End If
        node.Text = RLNK_Nome
        Return node
    End Function 'CreateNode

    Private Function GeneraNoNode(ByVal oTree As Telerik.WebControls.RadTreeView)
        Dim oRootNode As New RadTreeNode
        Dim oNode As New RadTreeNode

        oRootNode = New RadTreeNode
        oRootNode.Value = -1
        oRootNode.Expanded = True
        oRootNode.ImageUrl = "folder.gif"
        oRootNode.Category = 0
        oRootNode.Checkable = False


        Try
            oRootNode.Text = oResource.getValue("oRootNode.Text")
            If oRootNode.Text = "" Then
                oRootNode.Text = "Raccolta Link"
            End If
        Catch ex As Exception
            oRootNode.Text = "Raccolta Link"
        End Try
        Try
            oRootNode.ToolTip = oResource.getValue("oRootNode.ToolTip")
            If oRootNode.ToolTip = "" Then
                oRootNode.ToolTip = "Raccolta Link"
            End If
        Catch ex As Exception
            oRootNode.Text = "Raccolta Link"
        End Try




        oNode = New RadTreeNode
        oNode.Expanded = True
        oNode.Value = -1
        Try
            oNode.Text = oResource.getValue("oNoNode.Text")
            If oNode.Text = "" Then
                oNode.Text = "Nessun link presente"
            End If
        Catch ex As Exception
            oNode.Text = "Nessun link presente"
        End Try
        Try
            oNode.ToolTip = oResource.getValue("oNoNode.ToolTip")
            If oNode.ToolTip = "" Then
                oNode.ToolTip = "Nessun link presente"
            End If
        Catch ex As Exception
            oNode.Text = "Nessun link presente"
        End Try
        oNode.Category = 0
        oNode.Checkable = False
        oRootNode.Nodes.Add(oNode)

        oTree.Nodes.Clear()
        oTree.Nodes.Add(oRootNode)

    End Function
#End Region

#Region "Menu utente"
    Private Sub TBSmenu_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        Me.Form_HideAll()

        Select Case Me.TBSmenu.SelectedIndex
            Case Me.TabSelezionato.Lista
                Me.PNLcontenuto.Visible = True
                Me.PNLmenu.Visible = True
                Me.TBLlistaLink.Visible = True
                'Me.oResource.setLabel(Me.LBTitolo)
                Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.text")
                Me.Bind_RaccoltaLink()
                Me.Utility.AddAction(Services_RaccoltaLink.ActionType.ViewLinks, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
            Case Me.TabSelezionato.Gestione
                Me.Form_ResetToGestione()
                Me.LNBeliminaDir.Visible = False
                Me.TBLgestione.Visible = True
                Me.CTRLgestioneLink.SetupIniziale()
                Me.Utility.AddAction(Services_RaccoltaLink.ActionType.ManageLink, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
            Case Me.TabSelezionato.EsportaLink
                Me.CTRLesportaLink.SetupIniziale(-1)
                Me.Form_ResetToExport()
                Session("Azione") = "esporta"
                Me.Utility.AddAction(Services_RaccoltaLink.ActionType.ExportLink, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
            Case Me.TabSelezionato.ImportaLink
                Session("Azione") = "importa"
                Me.CTRLimportaLink.SetupIniziale()
                Me.Utility.AddAction(Services_RaccoltaLink.ActionType.ImportLink, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
                Me.Form_ResetToImport()
        End Select
    End Sub
  

    Private Sub LNBesporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBesporta.Click
        If Session("Azione") = "esporta" Then
            Dim iResponse As UC_EsportaLink.Export
            Dim Messaggio As String = ""
            iResponse = Me.CTRLesportaLink.Esporta

            If iResponse <> UC_EsportaLink.Export.Giàeffettuato Then
                Messaggio = oResource.getValue("Export." & iResponse)
                If Messaggio <> "" Then
                    Messaggio = Messaggio.Replace("'", "\'")
                End If
                If Messaggio <> "" Then
                    Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
                End If
            End If

            If Not (iResponse = UC_EsportaLink.Export.Ok Or iResponse = UC_EsportaLink.Export.Giàeffettuato) Then
                Exit Sub
            Else
                Session("Azione") = "loaded"
            End If
        End If
        Me.PNLmenu.Visible = False
        Me.PNLmenugestione.Visible = False
        Me.TBLesporta.Visible = False
        Me.TBLimporta.Visible = False

        If Me.TBSmenu.Tabs(TabSelezionato.Gestione).Visible Then
            Me.PNLmanagement.Visible = True
            Me.TBSmenu.SelectedIndex = Me.TabSelezionato.Gestione
            Me.TBLgestione.Visible = True
            Me.CTRLgestioneLink.SetupIniziale()
            Me.Utility.AddAction(Services_RaccoltaLink.ActionType.ManageLink, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
        Else
            Me.PNLmenu.Visible = True
            Me.TBSmenu.SelectedIndex = Me.TabSelezionato.Lista
            Me.TBLlistaLink.Visible = True
            Me.Bind_RaccoltaLink()
            Me.Utility.AddAction(Services_RaccoltaLink.ActionType.ViewLinks, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
        End If
    End Sub
    Private Sub LNBimporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBimporta.Click
        If Session("Azione") = "importa" Then
            Dim iResponse As UC_ImportaLink.Import
            Dim Messaggio As String = ""
            iResponse = Me.CTRLimportaLink.ImportaLink

            If iResponse <> UC_ImportaLink.Import.Giàeffettuato Then
                Messaggio = oResource.getValue("Importa." & iResponse)
                If Messaggio <> "" Then
                    Messaggio = Messaggio.Replace("'", "\'")
                End If
                If Messaggio <> "" Then
                    Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
                End If
            End If
           
            If Not (iResponse = UC_ImportaLink.Import.Ok Or iResponse = UC_ImportaLink.Import.Giàeffettuato) Then
                Exit Sub
            Else
                Session("Azione") = "loaded"
            End If
        End If

        If Me.TBSmenu.Tabs(TabSelezionato.Gestione).Visible Then
            Me.TBSmenu.SelectedIndex = Me.TabSelezionato.Gestione
            Me.Form_ResetToGestione()
            Me.CTRLgestioneLink.SetupIniziale(Me.CTRLimportaLink.CartellaDestinazioneID)
            Me.PNLmenuLink.Visible = False
            If Me.CTRLimportaLink.CartellaDestinazioneID = 0 Then
                Me.LNBeliminaDir.Visible = False
                Me.LNBmodificaDir.Visible = False
                Me.LNBelimina.Visible = False
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione")
                Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione")
            Else
                Dim oServizioRaccoltaLink As New Services_RaccoltaLink
                Try
                    If Me.ViewState("PermessiAssociati") = "" Then
                        Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioRaccoltaLink.Codex)
                    End If
                    oServizioRaccoltaLink.PermessiAssociati = Me.ViewState("PermessiAssociati")
                Catch ex As Exception
                    oServizioRaccoltaLink.PermessiAssociati = "00000000000000000000000000000000"
                End Try

                ' Me.LNBdettagliDir.Visible = True
                Me.LNBmodificaDir.Visible = (oServizioRaccoltaLink.Admin Or oServizioRaccoltaLink.Moderate Or oServizioRaccoltaLink.ChangeLink)
                Me.LNBeliminaDir.Visible = (oServizioRaccoltaLink.Admin Or oServizioRaccoltaLink.Moderate Or oServizioRaccoltaLink.RemoveLink)
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.dettagli.False")
                Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.dettagli.False")
            End If
            Me.Utility.AddAction(Services_RaccoltaLink.ActionType.ManageLink, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
        Else
            Me.Form_HideAll()
            Me.PNLcontenuto.Visible = True
            Me.PNLmenu.Visible = True
            Me.TBSmenu.SelectedIndex = Me.TabSelezionato.Lista
            Me.TBLlistaLink.Visible = True
            Me.Bind_RaccoltaLink()
            Me.Utility.AddAction(Services_RaccoltaLink.ActionType.ViewLinks, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
        End If
    End Sub
#End Region

    Private Sub CTRLgestioneLink_AggiornaMenu(ByVal LinkId As Integer, ByVal isCartella As Boolean) Handles CTRLgestioneLink.AggiornaMenu
        Session("Azione") = "loaded"
        If LinkId = 0 Then
            Me.LNBdettagliDir.Visible = False
            Me.LNBeliminaDir.Visible = False
            Me.LNBmodificaDir.Visible = False
            Me.PNLmanagement.Visible = True
            Me.PNLmenuLink.Visible = False
            Me.PNLconfermaCancella.Visible = False
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione")
            Me.Master.ServiceTitle = oResource.getValue("LBTitolo.gestione")
        ElseIf isCartella Then
            Dim oServizioRaccoltaLink As New Services_RaccoltaLink

            Try
                If Me.ViewState("PermessiAssociati") = "" Then
                    Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioRaccoltaLink.Codex)
                End If
                oServizioRaccoltaLink.PermessiAssociati = Me.ViewState("PermessiAssociati")
            Catch ex As Exception
                oServizioRaccoltaLink.PermessiAssociati = "00000000000000000000000000000000"
            End Try

            Me.LNBmodificaDir.Visible = (oServizioRaccoltaLink.Admin Or oServizioRaccoltaLink.Moderate Or oServizioRaccoltaLink.ChangeLink)
            Me.LNBeliminaDir.Visible = (oServizioRaccoltaLink.Admin Or oServizioRaccoltaLink.Moderate Or oServizioRaccoltaLink.RemoveLink)
            Me.PNLmanagement.Visible = True
            Me.PNLmenuLink.Visible = False
            Me.PNLconfermaCancella.Visible = False
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.dettagli.False")
            Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.dettagli.False")
        Else
            Me.LNBeliminaDir.Visible = False
            Me.PNLmanagement.Visible = False
            Me.PNLmenuLink.Visible = True
            Me.PNLconfermaCancella.Visible = False
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.dettagli.True")
            Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.dettagli.True")
        End If
    End Sub

#Region "Inserimento Link"
    Private Sub LNBinserisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBinserisci.Click
        If Me.CTRLcreaLink.Visible And (Session("Azione") = "inserisci" Or Session("Azione") = "modifica") Then
            Dim insert As UC_CreaLink.Inserimento = UC_CreaLink.Inserimento.ErroreGenerico
            If Me.CTRLcreaLink.LinkID > 0 Then
                insert = Me.CTRLcreaLink.ModificaLink()
            Else
                insert = Me.CTRLcreaLink.AggiungiLink()
            End If


            Dim Messaggio As String = ""
            Messaggio = oResource.getValue("Inserimento." & insert)
            If Messaggio <> "" Then
                Messaggio = Messaggio.Replace("'", "\'")
            End If
            If Messaggio <> "" Then
                Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
            End If
            If Not (insert = UC_CreaLink.Inserimento.LinkCreato Or insert = UC_CreaLink.Inserimento.CartellaCreata Or insert = UC_CreaLink.Inserimento.CartellaModificata Or insert = UC_CreaLink.Inserimento.LinkModificato) Then
                Exit Sub
            Else
                Session("Azione") = "loaded"
            End If
        End If
        Me.Form_HideAll()
        Me.PNLcontenuto.Visible = True

        If Me.TBSmenu.SelectedIndex = Me.TabSelezionato.Lista Then
            Me.PNLmenu.Visible = True
            Me.TBLlistaLink.Visible = True
            Me.Bind_RaccoltaLink()
            'Me.oResource.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.text")
        ElseIf Me.TBSmenu.SelectedIndex = Me.TabSelezionato.Gestione Then
            Me.Form_ResetToGestione()
            Me.LNBeliminaDir.Visible = (Me.CTRLgestioneLink.CartellaCorrente <> 0)
            Me.TBLgestione.Visible = True
            If Me.LNBeliminaDir.Visible Then
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.dettagli.False")
                Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.dettagli.False")
            Else
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione")
                Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione")
            End If
            Me.CTRLgestioneLink.SetupIniziale(Me.CTRLgestioneLink.CartellaCorrente)
        End If
    End Sub

    Private Sub LNBindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietro.Click
        Me.Form_HideAll()
        Session("Azione") = "loaded"
        Me.PNLcontenuto.Visible = True
        Select Case Me.TBSmenu.SelectedIndex
            Case Me.TabSelezionato.Lista
                Me.PNLmenu.Visible = True
                Me.TBLlistaLink.Visible = True
                Me.Bind_RaccoltaLink()
                'Me.oResource.setLabel(Me.LBTitolo)
                Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.text")
                Me.Utility.AddAction(Services_RaccoltaLink.ActionType.ViewLinks, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
            Case Me.TabSelezionato.Gestione
                Me.Form_ResetToGestione()
                Me.LNBeliminaDir.Visible = (Me.CTRLgestioneLink.CartellaCorrente <> 0)
                Me.TBLgestione.Visible = True
                Me.CTRLgestioneLink.SetupIniziale(Me.CTRLgestioneLink.CartellaCorrente)

                Me.Utility.AddAction(Services_RaccoltaLink.ActionType.ManageLink, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
                'Case Me.TabSelezionato.EsportaLink
                '    Me.TBLesporta.Visible = True
                '    Me.CTRLesportaLink.SetupIniziale(-1)
                '    If Me.CTRLesportaLink.HasExport Then
                '        Me.LNBesporta.Enabled = True
                '    Else
                '        Me.LNBesporta.Enabled = False
                '    End If
                '    Me.PNLmenugestione.Visible = True
                '    Me.LNBesporta.Visible = True
                '    Session("Azione") = "esporta"
        End Select
    End Sub
#End Region

#Region "Azioni Link"
    Private Sub LNBdettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdettagli.Click
        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.dettagli.True")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.dettagli.True")
        Me.CTRLgestioneLink.Bind_Dettagli(Me.CTRLgestioneLink.LinkCorrenteID)
    End Sub

    Private Sub LNBmodifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmodifica.Click
        Me.Form_ResetToInsert(False)

        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.modifica.True")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.modifica.True")

        Me.CTRLcreaLink.isCartella = True
        Me.CTRLcreaLink.LinkPadreID = Me.CTRLgestioneLink.CartellaCorrente
        Me.CTRLcreaLink.LinkID = Me.CTRLgestioneLink.LinkCorrenteID
        Me.CTRLcreaLink.Bind_DatiLink(Me.CTRLgestioneLink.CartellaCorrente)

        Me.Utility.AddAction(Services_RaccoltaLink.ActionType.EditLinkTitleAndUrl, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)

        Session("Azione") = "modifica"
    End Sub

    Private Sub LNBeliminaLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBeliminaLink.Click
        Dim oRaccolta As New COL_RaccoltaLink

        Try
            With oRaccolta
                .ID = Me.CTRLgestioneLink.LinkCorrenteID
                .Estrai()
                If .Errore = Errori_Db.None Then
                    Me.CancellaLink(False)
                    Me.Utility.AddAction(Services_RaccoltaLink.ActionType.DeleteLink, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CancellaLink(ByVal Replica As Boolean)
        Dim oRaccolta As New COL_RaccoltaLink

        Try
            With oRaccolta
                .ID = Me.CTRLgestioneLink.LinkCorrenteID
                .Estrai()
                If .Errore = Errori_Db.None Then
                    .Cancella(Replica)
                    Dim Messaggio As String = ""
                    Messaggio = oResource.getValue("CancellaLink." & (.Errore = Errori_Db.None))
                    If Messaggio <> "" Then
                        Messaggio = Messaggio.Replace("'", "\'")
                    End If
                    If Messaggio <> "" Then
                        Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
                    End If
                    If .Errore = Errori_Db.None Then
                        Session("Azione") = "loaded"
                        Me.PNLconfermaCancella.Visible = False
                        Me.PNLmenuLink.Visible = True
                        Me.PNLmanagement.Visible = False
                        Me.CTRLgestioneLink.SetupIniziale(Me.CTRLgestioneLink.CartellaCorrente)
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub CancellaDir(ByVal Replica As Boolean)
        Dim oRaccolta As New COL_RaccoltaLink
        Dim CartellaPadre_ID As Integer
        Try
            With oRaccolta
                .ID = Me.CTRLgestioneLink.CartellaCorrente
                If .ID > 0 Then
                    .Estrai()
                    If .Errore = Errori_Db.None Then
                        CartellaPadre_ID = .Padre_Id
                        .Cancella(Replica)
                        Dim Messaggio As String = ""
                        Messaggio = oResource.getValue("CancellaDir." & (.Errore = Errori_Db.None))
                        If Messaggio <> "" Then
                            Messaggio = Messaggio.Replace("'", "\'")
                        End If
                        If Messaggio <> "" Then
                            Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
                        End If
                        If .Errore = Errori_Db.None Then
                            Session("Azione") = "loaded"
                            Me.PNLconfermaCancella.Visible = False
                            Me.PNLmenuLink.Visible = False
                            Me.PNLmanagement.Visible = True
                            Me.CTRLgestioneLink.SetupIniziale(CartellaPadre_ID)
                        End If
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub CancellaContenuto(ByVal Replica As Boolean)
        Dim oRaccolta As New COL_RaccoltaLink
        Dim CartellaPadre_ID As Integer

        With oRaccolta
            .ID = Me.CTRLgestioneLink.CartellaCorrente
            If .ID = 0 Then
                .CancellaContenuto()
                Dim Messaggio As String = ""
                Messaggio = oResource.getValue("CancellaContenuto." & (.Errore = Errori_Db.None))
                If Messaggio <> "" Then
                    Messaggio = Messaggio.Replace("'", "\'")
                End If
                If Messaggio <> "" Then
                    Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
                End If
                If .Errore = Errori_Db.None Then
                    Session("Azione") = "loaded"
                    Me.PNLconfermaCancella.Visible = False
                    Me.PNLmenuLink.Visible = False
                    Me.PNLmanagement.Visible = True
                    Me.CTRLgestioneLink.SetupIniziale(CartellaPadre_ID)
                End If
            Else
                .Estrai()
                If .Errore = Errori_Db.None Then
                    CartellaPadre_ID = .Padre_Id

                    .CancellaContenuto()

                    Dim Messaggio As String = ""
                    Messaggio = oResource.getValue("CancellaContenuto." & (.Errore = Errori_Db.None))
                    If Messaggio <> "" Then
                        Messaggio = Messaggio.Replace("'", "\'")
                    End If
                    If Messaggio <> "" Then
                        Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
                    End If
                    If .Errore = Errori_Db.None Then
                        Session("Azione") = "loaded"
                        Me.PNLconfermaCancella.Visible = False
                        Me.PNLmenuLink.Visible = False
                        Me.PNLmanagement.Visible = True
                        Me.CTRLgestioneLink.SetupIniziale(CartellaPadre_ID)
                    End If
                End If
            End If
        End With
    End Sub
#End Region

#Region "Azioni Cartella"
    Private Sub LNBdettagliDir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdettagliDir.Click
        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.dettagli.False")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.dettagli.False")
        Me.CTRLgestioneLink.Bind_Dettagli(Me.CTRLgestioneLink.CartellaCorrente)
    End Sub
    Private Sub LNBcreaDirectory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcreaDirectory.Click
        Me.Form_ResetToInsert(False)
        Try
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.InserimentoLink.False")
            Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.InserimentoLink.False")
            Me.CTRLcreaLink.isCartella = True
            Me.CTRLcreaLink.LinkPadreID = Me.CTRLgestioneLink.CartellaCorrente
            Me.CTRLcreaLink.LinkID = 0
            Me.CTRLcreaLink.Bind_DatiLink(Me.CTRLgestioneLink.CartellaCorrente)
            Session("Azione") = "inserisci"

            Me.Utility.AddAction(Services_RaccoltaLink.ActionType.AddLink, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
        Catch ex As Exception

        End Try

    End Sub
    Private Sub LNBcreaLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcreaLink.Click
        Me.Form_ResetToInsert(True)
        Try
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.InserimentoLink.True")
            Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.InserimentoLink.True")

            Me.CTRLcreaLink.isCartella = False
            Me.CTRLcreaLink.LinkPadreID = Me.CTRLgestioneLink.CartellaCorrente
            Me.CTRLcreaLink.LinkID = 0
            Me.CTRLcreaLink.Bind_DatiLink(Me.CTRLgestioneLink.CartellaCorrente)
            Me.Utility.AddAction(Services_RaccoltaLink.ActionType.AddLink, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
            Session("Azione") = "inserisci"
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LNBmodificaDir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmodificaDir.Click
        Me.Form_ResetToInsert(False)

        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.modifica.False")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.modifica.False")

        Me.CTRLcreaLink.isCartella = True
        Me.CTRLcreaLink.LinkID = Me.CTRLgestioneLink.CartellaCorrente
        Me.CTRLcreaLink.Bind_DatiLink()
        Session("Azione") = "modifica"
    End Sub

    Private Sub LNBeliminaDir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBeliminaDir.Click
        Dim oRaccolta As New COL_RaccoltaLink
        Dim CartellaPadre_ID As Integer
        Try
            With oRaccolta
                .ID = Me.CTRLgestioneLink.CartellaCorrente
                If .ID > 0 Then
                    .Estrai()
                    If .Errore = Errori_Db.None Then
                        CartellaPadre_ID = .Padre_Id
                        Me.CancellaDir(False)
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LNBeliminaContenuto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBeliminaContenuto.Click
        Dim oRaccolta As New COL_RaccoltaLink
        Dim CartellaPadre_ID As Integer

        With oRaccolta
            .ID = Me.CTRLgestioneLink.CartellaCorrente
            If .ID = 0 Then
                Me.CancellaContenuto(False)
            Else
                .Estrai()
                If .Errore = Errori_Db.None Then
                    CartellaPadre_ID = .Padre_Id
                    Me.CancellaContenuto(False)
                End If
            End If
        End With
    End Sub
#End Region


    Private Sub RDTraccoltaLink_NodeClick(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles RDTraccoltaLink.NodeClick
        If e.NodeClicked.Value < 0 Then
            Me.CTRLdettagliLink.Visible = False
        Else
            Me.CTRLdettagliLink.Visible = True
            Me.CTRLdettagliLink.Bind_dettagli(e.NodeClicked.Value)
        End If
    End Sub

#Region "Menu Principali"
    Private Sub LNBaggiorna_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBaggiorna.Click
        Me.Bind_RaccoltaLink()

        Me.Utility.AddAction(Services_RaccoltaLink.ActionType.ViewLinks, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub

    Private Sub LNBnuovoFolderLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBnuovoFolderLink.Click
        Me.Form_ResetToInsert(False)
        Try
            Me.CTRLcreaLink.isCartella = True
            Me.CTRLcreaLink.LinkPadreID = 0
            Me.CTRLcreaLink.LinkID = 0
            Me.CTRLcreaLink.Bind_DatiLink()
            Session("Azione") = "inserisci"
        Catch ex As Exception

        End Try

    End Sub
    Private Sub LNBnuovoLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBnuovoLink.Click
        Me.Form_ResetToInsert(True)
        Try
            Me.CTRLcreaLink.isCartella = False
            Me.CTRLcreaLink.LinkPadreID = 0
            Me.CTRLcreaLink.LinkID = 0
            Me.CTRLcreaLink.Bind_DatiLink()
            Session("Azione") = "inserisci"
        Catch ex As Exception

        End Try

    End Sub
#End Region

#Region "Gestione Elementi pagina"
    Private Sub Form_HideAll()
        Me.PNLmenu.Visible = False
        Me.PNLmenugestione.Visible = False
        Me.PNLmanagement.Visible = False
        Me.PNLmenuLink.Visible = False
        Me.PNLconfermaCancella.Visible = False

        Me.TBLcreaLink.Visible = False
        Me.TBLesporta.Visible = False
        Me.TBLgestione.Visible = False
        Me.TBLimporta.Visible = False
        Me.TBLlistaLink.Visible = False

        Me.LNBindietro.Visible = False
        Me.LNBinserisci.Visible = False
        Me.LNBesporta.Visible = False
        Me.LNBimporta.Visible = False
    End Sub
    Private Sub Form_ResetToInsert(ByVal isLink As Boolean)
        Me.Form_HideAll()
        Me.PNLcontenuto.Visible = False
        Me.PNLmenugestione.Visible = True
        Me.TBLcreaLink.Visible = True
        Me.LNBinserisci.Visible = True
        Me.LNBindietro.Visible = True
        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.InserimentoLink." & isLink)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.InserimentoLink." & isLink)
    End Sub
    Private Sub Form_ResetToGestione()
        Me.Form_HideAll()
        Me.TBLgestione.Visible = True
        Me.PNLmanagement.Visible = True
        Me.LNBeliminaDir.Visible = False

        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione")
    End Sub
    Private Sub Form_ResetToExport()
        Me.Form_HideAll()
        Me.PNLcontenuto.Visible = True
        Me.PNLmenugestione.Visible = True
        Me.TBLesporta.Visible = True

        Me.LNBesporta.Visible = True
        If Me.CTRLesportaLink.HasExport Then
            Me.LNBesporta.Enabled = True
        Else
            Me.LNBesporta.Enabled = False
        End If
    End Sub
    Private Sub Form_ResetToImport()
        Me.Form_HideAll()
        Me.PNLcontenuto.Visible = True
        Me.PNLmenugestione.Visible = True
        Me.TBLimporta.Visible = True
        Me.LNBimporta.Visible = True

        If Me.CTRLimportaLink.HasImport Then
            Me.LNBimporta.Enabled = True
        Else
            Me.LNBimporta.Enabled = False
        End If
    End Sub
#End Region

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        Utility.CurrentModule = Utility.GetModule(Services_RaccoltaLink.Codex)
    End Sub

    Private Function CreateObjectToNotify(ByVal LinkID As Integer) As dtoObjectToNotify
        Dim obj As New dtoObjectToNotify
        obj.ObjectID = LinkID.ToString
        obj.ObjectTypeID = Services_RaccoltaLink.ObjectType.Link
        obj.FullyQualiFiedNameField = GetType(COL_BusinessLogic_v2.COL_RaccoltaLink).FullName
        Return obj
    End Function

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class