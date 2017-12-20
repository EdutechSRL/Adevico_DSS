Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.UCServices
Imports Telerik.WebControls


Public Class Link_Personali
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager

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

        EsportaLink = 4
    End Enum

    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label

#Region "Pannelli menu"

  Protected WithEvents TBSmenu As Global.Telerik.Web.UI.RadTabStrip
    Protected WithEvents PNLmenu As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLmenugestione As System.Web.UI.WebControls.Panel

    Protected WithEvents LNBaggiorna As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBespandi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcomprimi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBnuovoFolderLink As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBnuovoLink As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBinserisci As System.Web.UI.WebControls.LinkButton
#End Region

#Region "Lista Raccolta"
    Protected WithEvents PNLlista As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLlistaLink As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLdettagliBookmark As Comunita_OnLine.UC_DettagliBookmark
#Region "Treeview"
    Protected WithEvents PNLtreeView As System.Web.UI.WebControls.Panel

    Protected WithEvents RDTraccoltaLink As Telerik.WebControls.RadTreeView
#End Region

#End Region

#Region "Gestione Link"
    Protected WithEvents PNLmanagement As System.Web.UI.WebControls.Panel

    Protected WithEvents LBfolder_t As System.Web.UI.WebControls.Label
    Protected WithEvents LNBdettagliDir As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBmodificaDir As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBeliminaDir As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBeliminaContenuto As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcreaDirectory As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcreaLink As System.Web.UI.WebControls.LinkButton

    Protected WithEvents TBLgestione As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLgestioneBookmark As Comunita_OnLine.UC_GestioneBookmark


    Protected WithEvents PNLmenuLink As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBdettagli As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBmodifica As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBeliminaLink As System.Web.UI.WebControls.LinkButton
#End Region

#Region "Gestione Creazione Link"
    Protected WithEvents TBLcreaLink As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLcreaBookmark As Comunita_OnLine.UC_CreaBookmark
#End Region

    Protected WithEvents PNLconfermaCancella As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBannullaElimina As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBelimina As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBreplicaElimina As System.Web.UI.WebControls.LinkButton

#Region "Esporta Link"
    Protected WithEvents TBLesporta As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLesportaLink As Comunita_OnLine.UC_EsportaLink
    Protected WithEvents LNBesporta As System.Web.UI.WebControls.LinkButton
#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oPersona As COL_Persona
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        Try
            oPersona = Session("objPersona")
            If oPersona.Id <> 0 Then

            End If
        Catch ex As Exception
            Dim alertMSG As String
            alertMSG = oResource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
            End If
            oPersona = Nothing
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            Response.End()
            Exit Sub
        End Try
        If Not Page.IsPostBack Then
            Try
                Me.SetupInternazionalizzazione()

                Me.PNLcontenuto.Visible = True
                Me.PNLpermessi.Visible = False
                Me.Bind_MenuTab()
                Me.Bind_RaccoltaBookMark()
                Me.LNBcomprimi.Attributes.Add("onclick", "CollapseAll();return false;")
                Me.LNBespandi.Attributes.Add("onclick", "ExpandAll();return false;")
            Catch ex As Exception
                Me.PNLcontenuto.Visible = False
                Me.PNLpermessi.Visible = True
            End Try
        End If

    End Sub

#Region "Gestione Permessi"
    Private Sub Bind_MenuTab()
        Dim i As Integer = 1
        If Me.TBSmenu.Tabs(2).Visible Then
            i += 1
        End If
        If Me.TBSmenu.Tabs(1).Visible Then
            i += 1
        End If
        If i = 5 Then
            Me.TBSmenu.Width = System.Web.UI.WebControls.Unit.Point(650)
        ElseIf i = 4 Then
            Me.TBSmenu.Width = System.Web.UI.WebControls.Unit.Point(600)
        ElseIf i = 3 Then
            Me.TBSmenu.Width = System.Web.UI.WebControls.Unit.Point(450)
        ElseIf i = 2 Then
            Me.TBSmenu.Width = System.Web.UI.WebControls.Unit.Point(350)
        ElseIf i = 1 Then
            Me.TBSmenu.Width = System.Web.UI.WebControls.Unit.Point(160)
        End If
    End Sub
#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_Link_Personali"
        oResource.Folder_Level1 = "Generici"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
            .setLabel(Me.LBNopermessi)
            TBSmenu.Tabs(0).Text = .getValue("TABlista.Text")
            TBSmenu.Tabs(0).ToolTip = .getValue("TABlista.ToolTip")
            TBSmenu.Tabs(1).Text = .getValue("TABgestione.Text")
            TBSmenu.Tabs(1).ToolTip = .getValue("TABgestione.ToolTip")
            TBSmenu.Tabs(2).Text = .getValue("TABesporta.Text")
            TBSmenu.Tabs(2).ToolTip = .getValue("TABesporta.ToolTip")

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
    Private Sub Bind_RaccoltaBookMark()
        Dim oBookMark As New COL_BookMark
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona
        oPersona = Session("objPersona")
        Try
            oDataset = oBookMark.ElencaByUser(oPersona.Id)

            Me.RDTraccoltaLink.Nodes.Clear()

            Dim nodeRoot As New RadTreeNode
            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "folder.gif"
            nodeRoot.Value = -1
            nodeRoot.Category = 0
            nodeRoot.Selected = True
            nodeRoot.Checked = True
            Try
                nodeRoot.Text = oResource.getValue("oRootNode.Text")
                If nodeRoot.Text = "" Then
                    nodeRoot.Text = "Link Personali"
                End If
            Catch ex As Exception
                nodeRoot.Text = "Link Personali"
            End Try
            Try
                nodeRoot.ToolTip = oResource.getValue("oRootNode.ToolTip")
                If nodeRoot.ToolTip = "" Then
                    nodeRoot.ToolTip = "Link Personali"
                End If
            Catch ex As Exception
                nodeRoot.Text = "Link Personali"
            End Try


            Me.RDTraccoltaLink.Nodes.Add(nodeRoot)
            If oDataset.Tables(0).Rows.Count = 0 Then
                Me.GeneraNoNode(Me.RDTraccoltaLink)
            Else
                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("BOKM_ID"), oDataset.Tables(0).Columns("BOKM_Padre_Id"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("BOKM_Padre_Id") = 0 Then
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
        Dim BOKM_CMNT_ID, BOKM_ID As Integer
        Dim BOKM_Nome, BOKM_Url, BOKM_Descrizione As String
        Dim BOKM_Modified, BOKM_Created As DateTime
        Dim BOKM_isCartella As Boolean

        If IsDBNull(dbRow.Item("BOKM_CMNT_ID")) Then
            BOKM_CMNT_ID = -1
        Else
            BOKM_CMNT_ID = dbRow.Item("BOKM_CMNT_ID")
        End If
        BOKM_ID = dbRow.Item("BOKM_ID")
        BOKM_Nome = dbRow.Item("BOKM_Nome")
        If IsDBNull(dbRow.Item("BOKM_Url")) Then
            BOKM_Url = ""
        Else
            BOKM_Url = dbRow.Item("BOKM_Url")
        End If
        If IsDBNull(dbRow.Item("BOKM_Descrizione")) Then
            BOKM_Descrizione = ""
        Else
            BOKM_Descrizione = dbRow.Item("BOKM_Descrizione")
        End If

        BOKM_isCartella = dbRow.Item("BOKM_isCartella")
        If IsDBNull(dbRow.Item("BOKM_Created")) = False Then
            BOKM_Created = dbRow.Item("BOKM_Created")
        End If
        If IsDBNull(dbRow.Item("BOKM_Modified")) = False Then
            BOKM_Modified = dbRow.Item("BOKM_Modified")
        End If

        node.ToolTip = BOKM_Nome
        node.Value = BOKM_ID
        node.Expanded = expanded
        node.Category = BOKM_ID
        node.Checkable = False

        If BOKM_isCartella = False Then
            node.ImageUrl = "html.gif"

            'BOKM_Nome = BOKM_Nome &  Me.GenerateURLFromPath(CMNT_REALpath, "Dettagli", "dettagli" & "," & CMNT_IsChiusa & ",1", "Visualizza i dettagli  relativi alla comunità.")
            node.Target = "_blank"
            If InStr(BOKM_Url, "http://") > 0 Then
                node.NavigateUrl = BOKM_Url
            Else
                node.NavigateUrl = "http://" & BOKM_Url
            End If
            '  BOKM_Nome = BOKM_Nome & Me.GenerateURLFromPath(CMNT_REALpath, "Aggiungi", "dettagli" & "," & CMNT_IsChiusa & ",1", "Aggingi un nuovo link.")
        Else
            node.ImageUrl = "folder.gif"
            node.ImageExpandedUrl = "FolderOpen.gif"
        End If
        node.Text = BOKM_Nome
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
                oRootNode.Text = "Link Personali"
            End If
        Catch ex As Exception
            oRootNode.Text = "Link Personali"
        End Try
        Try
            oRootNode.ToolTip = oResource.getValue("oRootNode.ToolTip")
            If oRootNode.ToolTip = "" Then
                oRootNode.ToolTip = "Link Personali"
            End If
        Catch ex As Exception
            oRootNode.Text = "Link Personali"
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

    Private Sub HideAll()
        Me.TBLcreaLink.Visible = False
        Me.TBLlistaLink.Visible = False
        Me.TBLesporta.Visible = False
        Me.TBLgestione.Visible = False

        Me.PNLmenu.Visible = False
        Me.PNLmenugestione.Visible = False
        Me.PNLmanagement.Visible = False
        Me.PNLmenuLink.Visible = False
        Me.PNLconfermaCancella.Visible = False
        Me.LNBesporta.Visible = False
        Me.LNBinserisci.Visible = False
    End Sub

    Private Sub TBSmenu_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmenu.TabClick
        Me.HideAll()

        Select Case Me.TBSmenu.SelectedIndex
            Case Me.TabSelezionato.Lista
                Me.PNLmenu.Visible = True
                Me.TBLlistaLink.Visible = True
                'Me.oResource.setLabel(Me.LBTitolo)
                Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.text")
                Me.Bind_RaccoltaBookMark()

            Case Me.TabSelezionato.Gestione
                Me.ResetToGestione()
                Me.LNBeliminaDir.Visible = False
                Me.TBLgestione.Visible = True
                Me.CTRLgestioneBookmark.SetupIniziale()

            Case Me.TabSelezionato.EsportaLink
                Me.TBLesporta.Visible = True
                Me.CTRLesportaLink.SetupIniziale(-1)
                If Me.CTRLesportaLink.HasExport Then
                    Me.LNBesporta.Enabled = True
                Else
                    Me.LNBesporta.Enabled = False
                End If
                Me.PNLmenugestione.Visible = True
                Me.LNBesporta.Visible = True
                Session("Azione") = "esporta"

        End Select
    End Sub

    Private Sub ResetToInsert(ByVal isLink As Boolean)
        Me.HideAll()
        Me.PNLcontenuto.Visible = False
        Me.PNLmenugestione.Visible = True
        Me.TBLcreaLink.Visible = True
        Me.LNBinserisci.Visible = True
        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.InserimentoLink." & isLink)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.InserimentoLink." & isLink)
    End Sub

    Private Sub ResetToGestione()
        Me.HideAll()
        Me.TBLgestione.Visible = True
        Me.PNLmanagement.Visible = True
        Me.LNBeliminaDir.Visible = False

        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione")
    End Sub

#Region "Menu Principali"
    Private Sub LNBaggiorna_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBaggiorna.Click
        Me.Bind_RaccoltaBookMark()
    End Sub
    Private Sub LNBnuovoFolderLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBnuovoFolderLink.Click
        Me.ResetToInsert(False)
        Try
            Me.CTRLcreaBookmark.isCartella = True
            Me.CTRLcreaBookmark.BookMarkPadreID = 0
            Me.CTRLcreaBookmark.BookMarkID = 0
            Me.CTRLcreaBookmark.Bind_DatiBookMark()
            Session("Azione") = "inserisci"
        Catch ex As Exception

        End Try

    End Sub
    Private Sub LNBnuovoLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBnuovoLink.Click
        Me.ResetToInsert(True)
        Try
            Me.CTRLcreaBookmark.isCartella = False
            Me.CTRLcreaBookmark.BookMarkPadreID = 0
            Me.CTRLcreaBookmark.BookMarkID = 0
            Me.CTRLcreaBookmark.Bind_DatiBookMark()
            Session("Azione") = "inserisci"
        Catch ex As Exception

        End Try

    End Sub
    Private Sub LNBesporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBesporta.Click
        If Session("Azione") = "esporta" Then
            Dim esporta As Boolean = False
            esporta = Me.CTRLesportaLink.Esporta
            Session("Azione") = "loaded"

        End If
        Me.PNLmenu.Visible = False
        Me.PNLmenugestione.Visible = False
        Me.TBLesporta.Visible = False

        If Me.TBSmenu.Tabs(1).Visible Then
            Me.TBSmenu.SelectedIndex = Me.TabSelezionato.Gestione
            Me.TBLgestione.Visible = True
            Me.CTRLgestioneBookmark.SetupIniziale()
        Else
            Me.PNLmenu.Visible = True
            Me.TBSmenu.SelectedIndex = Me.TabSelezionato.Lista
            Me.TBLlistaLink.Visible = True
            Me.Bind_RaccoltaBookMark()
        End If
    End Sub
#End Region

    Private Sub RDTraccoltaLink_NodeClick(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles RDTraccoltaLink.NodeClick
        If e.NodeClicked.Value < 0 Then
            Me.CTRLdettagliBookmark.Visible = False
        Else
            Me.CTRLdettagliBookmark.Visible = True
            Me.CTRLdettagliBookmark.Bind_dettagli(e.NodeClicked.Value)
        End If
    End Sub

#Region "Inserimento Link"
    Private Sub LNBinserisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBinserisci.Click
        If Me.CTRLcreaBookmark.Visible And (Session("Azione") = "inserisci" Or Session("Azione") = "modifica") Then
            Dim insert As UC_CreaBookmark.Inserimento = UC_CreaBookmark.Inserimento.ErroreGenerico
            If Me.CTRLcreaBookmark.BookMarkID > 0 Then
                insert = Me.CTRLcreaBookmark.ModificaLink()
            Else
                insert = Me.CTRLcreaBookmark.AggiungiLink()
            End If


            Dim Messaggio As String = ""
            Messaggio = oResource.getValue("Inserimento." & insert)
            If Messaggio <> "" Then
                Messaggio = Messaggio.Replace("'", "\'")
            End If
            If Messaggio <> "" Then
                Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
            End If
            If Not (insert = UC_CreaBookmark.Inserimento.LinkCreato Or insert = UC_CreaBookmark.Inserimento.CartellaCreata Or insert = UC_CreaBookmark.Inserimento.CartellaModificata Or insert = UC_CreaBookmark.Inserimento.LinkModificato) Then
                Exit Sub
            Else
                Session("Azione") = "loaded"
            End If
        End If
        Me.HideAll()
        Me.PNLcontenuto.Visible = True

        If Me.TBSmenu.SelectedIndex = Me.TabSelezionato.Lista Then
            Me.PNLmenu.Visible = True
            Me.TBLlistaLink.Visible = True
            Me.Bind_RaccoltaBookMark()
            'Me.oResource.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = oResource.getValue("LBTitolo.text")
        ElseIf Me.TBSmenu.SelectedIndex = Me.TabSelezionato.Gestione Then
            Me.ResetToGestione()
            Me.LNBeliminaDir.Visible = (Me.CTRLgestioneBookmark.CartellaCorrente <> 0)
            Me.TBLgestione.Visible = True
            If Me.LNBeliminaDir.Visible Then
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.dettagli.False")
                Me.Master.ServiceTitle = oResource.getValue("LBTitolo.gestione.dettagli.False")
            Else
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione")
                Me.Master.ServiceTitle = oResource.getValue("LBTitolo.gestione")
            End If
            Me.CTRLgestioneBookmark.SetupIniziale(Me.CTRLgestioneBookmark.CartellaCorrente)
        End If
    End Sub
    Private Sub LNBindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietro.Click
        Me.HideAll()
        Session("Azione") = "loaded"
        Me.PNLcontenuto.Visible = True
        Select Case Me.TBSmenu.SelectedIndex
            Case Me.TabSelezionato.Lista
                Me.PNLmenu.Visible = True
                Me.TBLlistaLink.Visible = True
                Me.Bind_RaccoltaBookMark()
                'Me.oResource.setLabel(Me.LBTitolo)
                Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.text")
            Case Me.TabSelezionato.Gestione
                Me.ResetToGestione()
                Me.LNBeliminaDir.Visible = (Me.CTRLgestioneBookmark.CartellaCorrente <> 0)
                Me.TBLgestione.Visible = True
                Me.CTRLgestioneBookmark.SetupIniziale(Me.CTRLgestioneBookmark.CartellaCorrente)

            Case Me.TabSelezionato.EsportaLink
                Me.TBLesporta.Visible = True
                Me.CTRLesportaLink.SetupIniziale(-1)
                If Me.CTRLesportaLink.HasExport Then
                    Me.LNBesporta.Enabled = True
                Else
                    Me.LNBesporta.Enabled = False
                End If
                Me.PNLmenugestione.Visible = True
                Me.LNBesporta.Visible = True
                Session("Azione") = "esporta"
        End Select
    End Sub
#End Region


    Private Sub CTRLgestioneBookmark_AggiornaMenu(ByVal LinkId As Integer, ByVal isCartella As Boolean) Handles CTRLgestioneBookmark.AggiornaMenu
        Session("Azione") = "loaded"
        If LinkId = 0 Then
            Me.LNBeliminaDir.Visible = False
            Me.PNLmanagement.Visible = True
            Me.PNLmenuLink.Visible = False
            Me.PNLconfermaCancella.Visible = False
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione")
            Me.Master.ServiceTitle = oResource.getValue("LBTitolo.gestione")
        ElseIf isCartella Then
            Me.LNBeliminaDir.Visible = True
            Me.PNLmanagement.Visible = True
            Me.PNLmenuLink.Visible = False
            Me.PNLconfermaCancella.Visible = False
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.dettagli.False")
            Me.Master.ServiceTitle = oResource.getValue("LBTitolo.gestione.dettagli.False")
        Else
            Me.LNBeliminaDir.Visible = False
            Me.PNLmanagement.Visible = False
            Me.PNLmenuLink.Visible = True
            Me.PNLconfermaCancella.Visible = False
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.dettagli.True")
            Me.Master.ServiceTitle = oResource.getValue("LBTitolo.gestione.dettagli.True")
        End If
    End Sub

#Region "Azioni Link"
    Private Sub LNBdettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdettagli.Click
        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.dettagli.True")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.dettagli.True")
        Me.CTRLgestioneBookmark.Bind_Dettagli(Me.CTRLgestioneBookmark.LinkCorrenteID)
    End Sub

    Private Sub LNBmodifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmodifica.Click
        Me.ResetToInsert(False)

        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.modifica.True")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.modifica.True")
        Me.CTRLcreaBookmark.isCartella = True
        Me.CTRLcreaBookmark.BookMarkPadreID = Me.CTRLgestioneBookmark.CartellaCorrente
        Me.CTRLcreaBookmark.BookMarkID = Me.CTRLgestioneBookmark.LinkCorrenteID
        Me.CTRLcreaBookmark.Bind_DatiBookMark(Me.CTRLgestioneBookmark.CartellaCorrente)
        Session("Azione") = "modifica"
    End Sub

    Private Sub LNBeliminaLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBeliminaLink.Click
        Dim oBookMark As New COL_BookMark

        Try
            With oBookMark
                .ID = Me.CTRLgestioneBookmark.LinkCorrenteID
                .Estrai()
                If .Errore = Errori_Db.None Then
                    If .HasComunitaAssociate Then
                        Session("azione") = "eliminaLink"
                        Me.PNLmanagement.Visible = False
                        Me.PNLmenuLink.Visible = False
                        Me.PNLconfermaCancella.Visible = True
                        Me.CTRLgestioneBookmark.Bind_forDelete(True, UC_GestioneBookmark.Elimina.EliminaLink)
                        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.cancella.True")
                        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.cancella.True")
                    Else
                        Me.CancellaLink(False)
                    End If
                End If
              
            End With
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Azioni Cartella"
    Private Sub LNBdettagliDir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdettagliDir.Click
        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.dettagli.False")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.dettagli.False")
        Me.CTRLgestioneBookmark.Bind_Dettagli(Me.CTRLgestioneBookmark.CartellaCorrente)
    End Sub
    Private Sub LNBcreaDirectory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcreaDirectory.Click
        Me.ResetToInsert(False)
        Try
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.InserimentoLink.False")
            Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.InserimentoLink.False")
            Me.CTRLcreaBookmark.isCartella = True
            Me.CTRLcreaBookmark.BookMarkPadreID = Me.CTRLgestioneBookmark.CartellaCorrente
            Me.CTRLcreaBookmark.BookMarkID = 0
            Me.CTRLcreaBookmark.Bind_DatiBookMark(Me.CTRLgestioneBookmark.CartellaCorrente)
            Session("Azione") = "inserisci"
        Catch ex As Exception

        End Try

    End Sub
    Private Sub LNBcreaLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcreaLink.Click
        Me.ResetToInsert(True)
        Try
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.InserimentoLink.True")
            Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.InserimentoLink.True")
            Me.CTRLcreaBookmark.isCartella = False
            Me.CTRLcreaBookmark.BookMarkPadreID = Me.CTRLgestioneBookmark.CartellaCorrente
            Me.CTRLcreaBookmark.BookMarkID = 0
            Me.CTRLcreaBookmark.Bind_DatiBookMark(Me.CTRLgestioneBookmark.CartellaCorrente)
            Session("Azione") = "inserisci"
        Catch ex As Exception

        End Try

    End Sub
    Private Sub LNBmodificaDir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBmodificaDir.Click
        Me.ResetToInsert(False)

        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.modifica.False")
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.modifica.False")
        Me.CTRLcreaBookmark.isCartella = True
        Me.CTRLcreaBookmark.BookMarkID = Me.CTRLgestioneBookmark.CartellaCorrente
        Me.CTRLcreaBookmark.Bind_DatiBookMark()
        Session("Azione") = "modifica"
    End Sub

    Private Sub LNBeliminaDir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBeliminaDir.Click
        Dim oBookMark As New COL_BookMark
        Dim CartellaPadre_ID As Integer
        Try
            With oBookMark
                .ID = Me.CTRLgestioneBookmark.CartellaCorrente
                If .ID > 0 Then
                    .Estrai()
                    If .Errore = Errori_Db.None Then
                        CartellaPadre_ID = .Padre_Id
                        If .HasComunitaAssociate Then
                            Session("azione") = "elimina"
                            Me.PNLmanagement.Visible = False
                            Me.PNLmenuLink.Visible = False
                            Me.PNLconfermaCancella.Visible = True
                            Me.CTRLgestioneBookmark.Bind_forDelete(True, UC_GestioneBookmark.Elimina.EliminaDir)
                            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.cancella.False")
                            Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo.gestione.cancella.False")
                        Else
                            Me.CancellaDir(False)
                        End If
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LNBeliminaContenuto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBeliminaContenuto.Click
        Dim oBookMark As New COL_BookMark
        Dim CartellaPadre_ID As Integer

        With oBookMark
            .ID = Me.CTRLgestioneBookmark.CartellaCorrente
            If .ID = 0 Then
                'If .HasComunitaAssociate(Me.CTRLgestioneBookmark.CartellaCorrente, Session("objPersona").id) Then
                '    Session("azione") = "eliminaContenuto"
                '    Me.PNLmanagement.Visible = False
                '    Me.PNLmenuLink.Visible = False
                '    Me.PNLconfermaCancella.Visible = True
                '    Me.CTRLgestioneBookmark.Bind_forDelete(True, UC_GestioneBookmark.Elimina.EliminaContenuto)
                '    Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.cancella.False")
                'Else
                    Me.CancellaContenuto(False)
                'End If
            Else
                .Estrai()
                If .Errore = Errori_Db.None Then
                    CartellaPadre_ID = .Padre_Id
                    'If .HasComunitaAssociate Then
                    '    Session("azione") = "eliminaContenuto"
                    '    Me.PNLmanagement.Visible = False
                    '    Me.PNLmenuLink.Visible = False
                    '    Me.PNLconfermaCancella.Visible = True
                    '    Me.CTRLgestioneBookmark.Bind_forDelete(True, UC_GestioneBookmark.Elimina.EliminaContenuto)
                    '    Me.oResource.setLabel_To_Value(Me.LBTitolo, "LBTitolo.gestione.cancella.False")
                    'Else
                        Me.CancellaContenuto(False)
                    'End If
                End If
            End If
        End With
    End Sub
#End Region

    Private Sub CancellaLink(ByVal Replica As Boolean)
        Dim oBookMark As New COL_BookMark

        Try
            With oBookMark
                .ID = Me.CTRLgestioneBookmark.LinkCorrenteID
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
                        Me.CTRLgestioneBookmark.SetupIniziale(Me.CTRLgestioneBookmark.CartellaCorrente)
                    End If
                End If

            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub CancellaDir(ByVal Replica As Boolean)
        Dim oBookMark As New COL_BookMark
        Dim CartellaPadre_ID As Integer
        Try
            With oBookMark
                .ID = Me.CTRLgestioneBookmark.CartellaCorrente
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
                            Me.CTRLgestioneBookmark.SetupIniziale(CartellaPadre_ID)
                        End If
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub CancellaContenuto(ByVal Replica As Boolean)
        Dim oBookMark As New COL_BookMark
        Dim CartellaPadre_ID As Integer

        With oBookMark
            .ID = Me.CTRLgestioneBookmark.CartellaCorrente
            If .ID = 0 Then
                .CancellaAllFor(Session("objPersona").id)
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
                    Me.CTRLgestioneBookmark.SetupIniziale(CartellaPadre_ID)
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
                        Me.CTRLgestioneBookmark.SetupIniziale(CartellaPadre_ID)
                    End If
                End If
            End If
        End With
    End Sub


    Private Sub LNBreplicaElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBreplicaElimina.Click
        If Session("Azione") = "elimina" Then
            Me.CancellaDir(True)
        ElseIf Session("azione") = "eliminaContenuto" Then
            Me.CancellaContenuto(True)
        ElseIf Session("azione") = "eliminaLink" Then
            Me.CancellaLink(True)
        End If
    End Sub

    Private Sub LNBannullaElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBannullaElimina.Click
        Session("azione") = "loaded"
        Me.PNLconfermaCancella.Visible = False
        If Me.CTRLgestioneBookmark.LinkCorrenteID = -1 Then
            Me.PNLmanagement.Visible = True
            Me.PNLmenuLink.Visible = False
        Else
            Me.PNLmanagement.Visible = False
            Me.PNLmenuLink.Visible = True
        End If


        Me.CTRLgestioneBookmark.Bind_forDelete(False)
    End Sub

    Private Sub LNBelimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBelimina.Click
        If Session("Azione") = "elimina" Then
            Me.CancellaDir(False)
        ElseIf Session("azione") = "eliminaContenuto" Then
            Me.CancellaContenuto(False)
        ElseIf Session("azione") = "eliminaLink" Then
            Me.CancellaLink(False)
        End If
    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

End Class