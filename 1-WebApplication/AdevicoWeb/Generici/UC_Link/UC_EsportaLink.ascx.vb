Imports COL_BusinessLogic_v2

Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports Telerik
Imports Telerik.WebControls

Public Class UC_EsportaLink
    Inherits System.Web.UI.UserControl

    Private oResourceEsporta As ResourceManager
    Public Enum Export
        ErroreInserimento = -1
        NessunLinkSelezionato = 0
        ErroreGenerico = -2
        Ok = 1
        Giàeffettuato = 2
    End Enum
    Public Property HasExport() As Boolean
        Get
            If Me.HDN_hasExport.Value = "" Then
                HasExport = False
            Else
                HasExport = Me.HDN_hasExport.Value
            End If
        End Get
        Set(ByVal Value As Boolean)
            Me.HDN_hasExport.Value = Value
        End Set
    End Property
    Protected WithEvents HDN_hasExport As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents PNLesporta As System.Web.UI.WebControls.Panel
    Protected WithEvents LBesporta As System.Web.UI.WebControls.Label
    Protected WithEvents RDTesporta As Telerik.WebControls.RadTreeView

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
        If IsNothing(oResourceEsporta) Then
            SetCulture(Session("LinguaCode"))
        End If
    End Sub


    Public Sub SetupIniziale(ByVal NodeId As Integer)
        If IsNothing(oResourceEsporta) Then
            SetCulture(Session("LinguaCode"))
        End If

        Me.BindTree_ForEsporta(NodeId)
        If Me.HDN_hasExport.Value = "" Then
            Me.HDN_hasExport.Value = ""
        End If
        If Me.HDN_hasExport.Value = "" Or Me.HDN_hasExport.Value = "" Then
            ' mostro label in cui dico che sono tutti già presenti nei preferiti !!!
            Me.oResourceEsporta.setLabel_To_Value(LBesporta, "LBesporta.false")
            ' = "Tutti i link presenti i nquesta comunità sono già presenti nella sua raccolta link personale'
            Me.RDTesporta.Visible = False
        Else
            Me.oResourceEsporta.setLabel_To_Value(LBesporta, "LBesporta.true")
            ' = "Scegliere i link da inserire nei propri preferiti:'
            Me.RDTesporta.Visible = True
        End If
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceEsporta = New ResourceManager

        oResourceEsporta.UserLanguages = Code
        oResourceEsporta.ResourcesName = "pg_UC_EsportaLink"
        oResourceEsporta.Folder_Level1 = "Generici"
        oResourceEsporta.Folder_Level2 = "UC_Link"
        oResourceEsporta.setCulture()
    End Sub
#End Region

    Private Sub BindTree_ForEsporta(ByVal selectNode_Id As Integer)
        Dim oRaccoltaLink As New COL_RaccoltaLink
        Dim oDataset As New DataSet

        Try
            oDataset = oRaccoltaLink.ElencaForEsporta(Session("idComunita"), Session("objPersona").id)

            Me.RDTesporta.Nodes.Clear()

            Dim nodeRoot As New RadTreeNode
            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "folder.gif"
            nodeRoot.Value = 0
            nodeRoot.Category = 0
            nodeRoot.Checkable = False
            nodeRoot.Checked = False
            Try
                nodeRoot.Text = oResourceEsporta.getValue("oRootNodeCartelle.Text")
                If nodeRoot.Text = "" Then
                    nodeRoot.Text = "* Raccolta Link"
                End If
            Catch ex As Exception
                nodeRoot.Text = "* Raccolta Link"
            End Try
            Try
                nodeRoot.ToolTip = oResourceEsporta.getValue("oRootNodeCartelle.ToolTip")
                If nodeRoot.ToolTip = "" Then
                    nodeRoot.ToolTip = nodeRoot.Text
                End If
            Catch ex As Exception
                nodeRoot.Text = "* Raccolta Link"
            End Try

            Me.RDTesporta.Nodes.Add(nodeRoot)
            If oDataset.Tables(0).Rows.Count = 0 Then
                Me.GeneraNoNode()
            Else
                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("RLNK_ID"), oDataset.Tables(0).Columns("RLNK_Padre_Id"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("RLNK_Padre_Id") = 0 Then
                        Dim node As RadTreeNode = CreateNodeForEsporta(dbRow, True, selectNode_Id)
                        nodeRoot.Nodes.Add(node)
                        RecursivelyPopulateForEsporta(dbRow, node, selectNode_Id)
                    End If
                Next dbRow
                If selectNode_Id > 0 Then
                    Dim oNode As Telerik.WebControls.RadTreeNode
                    oNode = Me.RDTesporta.FindNodeByValue(selectNode_Id)
                    If Not IsNothing(oNode) Then
                        oNode.ExpandParentNodes()
                    End If
                End If
            End If
        Catch ex As Exception
            Me.RDTesporta.Nodes.Clear()
            Me.GeneraNoNode()
        End Try
    End Sub
    Private Sub RecursivelyPopulateForEsporta(ByVal dbRow As DataRow, ByVal node As RadTreeNode, ByVal selectNode_Id As Integer)
        Dim oData, oDataAttuale As DateTime
        Dim childRow As DataRow

        For Each childRow In dbRow.GetChildRows("NodeRelation")
            Dim childNode As RadTreeNode = CreateNodeForEsporta(childRow, False, selectNode_Id)
            If node.Checked And childNode.Checkable Then
                childNode.Checked = True
            End If
            node.Nodes.Add(childNode)
            Me.RecursivelyPopulateForEsporta(childRow, childNode, selectNode_Id)
        Next childRow
    End Sub
    Private Function CreateNodeForEsporta(ByVal dbRow As DataRow, ByVal expanded As Boolean, ByVal selectNode_Id As Integer) As RadTreeNode
        Dim node As New RadTreeNode

        Dim RLNK_CMNT_ID, RLNK_ID, RLNK_CreatoreID, RLNK_BOKM_ID As Integer
        Dim RLNK_Nome, RLNK_Url, RLNK_Descrizione As String
        Dim RLNK_Modified, RLNK_Created As DateTime
        Dim RLNK_isCartella, RLNK_FromBookmark, RLNK_Esportato As Boolean

        If IsDBNull(dbRow.Item("RLNK_CMNT_ID")) Then
            RLNK_CMNT_ID = -1
        Else
            RLNK_CMNT_ID = dbRow.Item("RLNK_CMNT_ID")
        End If
        RLNK_CreatoreID = dbRow.Item("RLNK_CreatoreID")
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

        RLNK_FromBookmark = dbRow.Item("RLNK_FromBookmark")
        RLNK_BOKM_ID = dbRow.Item("RLNK_BOKM_ID")
        If IsDBNull(dbRow.Item("RLNK_Created")) = False Then
            RLNK_Created = dbRow.Item("RLNK_Created")
        End If
        If IsDBNull(dbRow.Item("RLNK_Modified")) = False Then
            RLNK_Modified = dbRow.Item("RLNK_Modified")
        End If
        RLNK_Esportato = dbRow.Item("RLNK_Esportato")
        node.ToolTip = RLNK_Nome
        node.Value = RLNK_ID
        node.Expanded = expanded
        node.Category = RLNK_ID

        'RLNK_FromBookmark --> è sotto un bookmark inserito: solo dettagli !
        If RLNK_isCartella = False Then
            node.ImageUrl = "html.gif"
        Else
            node.ImageUrl = "folder.gif"
            node.ImageExpandedUrl = "FolderOpen.gif"
        End If

        If Me.HDN_hasExport.Value = "" Then
            Me.HDN_hasExport.Value = False
        End If
        If RLNK_Esportato Then
            node.Checkable = False
            node.Checked = False
        Else
            If RLNK_FromBookmark Then
                'è sotto un bookmark inserito: solo dettagli !
                If RLNK_CreatoreID <> Session("objPersona").id Then
                    Me.HDN_hasExport.Value = True
                Else
                    node.Checkable = False
                End If
            Else
                If RLNK_BOKM_ID > 0 Then
                    If RLNK_CreatoreID <> Session("objPersona").id Then
                        node.Checkable = False
                    Else
                        node.Checkable = False
                    End If
                Else
                    node.Checkable = True
                    Me.HDN_hasExport.Value = True
                End If
            End If
            If selectNode_Id = RLNK_ID And node.Checkable Then
                node.Checked = True
            End If
        End If


        node.Text = RLNK_Nome
        Return node
    End Function 'CreateNode
    Private Function GeneraNoNode()
        Dim oRootNode As New RadTreeNode
        Dim oNode As New RadTreeNode

        oRootNode = New RadTreeNode
        oRootNode.Value = ""
        oRootNode.Expanded = True
        oRootNode.ImageUrl = "root_folder.gif"
        Try
            oRootNode.Text = oResourceEsporta.getValue("oRootNodeCartelle.Text")
            If oRootNode.Text = "" Then
                oRootNode.Text = "* Raccolta Link"
            End If
        Catch ex As Exception
            oRootNode.Text = "* Raccolta Link"
        End Try
        Try
            oRootNode.ToolTip = oResourceEsporta.getValue("oRootNodeCartelle.ToolTip")
            If oRootNode.ToolTip = "" Then
                oRootNode.ToolTip = oRootNode.Text
            End If
        Catch ex As Exception
            oRootNode.Text = "* Raccolta Link"
        End Try

        oRootNode.Category = 0
        oRootNode.Value = 0
        oRootNode.Checkable = False

        Me.RDTesporta.Nodes.Clear()
        Me.RDTesporta.Nodes.Add(oRootNode)

        oRootNode.Selected = True
    End Function

    Public Function Esporta() As Export
        Try
            'importa i Link
            Dim i, totale As Integer
            Dim ListaAssociati As String = ","

            If Me.RDTesporta.CheckedNodes.Count > 0 Then
                Dim oArrayLink As ArrayList
                oArrayLink = Me.RDTesporta.CheckedNodes()

                totale = Me.RDTesporta.CheckedNodes.Count - 1
                For i = 0 To totale
                    Dim oNode As Telerik.WebControls.RadTreeNode
                    oNode = Me.RDTesporta.CheckedNodes(i)
                    If InStr(ListaAssociati, "," & oNode.Category & ",") <= 0 Then
                        Dim oBookMark As New COL_BookMark

                        oBookMark.Importa(oNode.Category, Session("idComunita"), Session("objPersona").id)
                        If oBookMark.Errore = Errori_Db.None Then
                            If oBookMark.ID > 0 Then
                                If oNode.Nodes.Count > 0 Then
                                    Me.EsportaSottoLink(oBookMark.ID, oNode.Nodes)
                                End If
                            End If
                        End If
                        ListaAssociati = ListaAssociati & oNode.Category & ","
                        oNode.Remove()
                        i = i - 1
                    End If
                    totale = Me.RDTesporta.CheckedNodes.Count - 1
                    If totale < 0 Or i > totale Then
                        Exit For
                    End If
                Next
                Return Export.Ok
            Else
                Return Export.NessunLinkSelezionato
            End If
        Catch ex As Exception
            Return Export.ErroreGenerico
        End Try
        Return Export.ErroreGenerico
    End Function

    Private Sub EsportaSottoLink(ByVal BookPadreID As Integer, ByVal oNodi As Telerik.WebControls.RadTreeNodeCollection)
        Dim i, totale As Integer

        Try
            totale = oNodi.Count - 1
            If totale < 0 Then

            Else
                For i = 0 To totale
                    Dim oNode As Telerik.WebControls.RadTreeNode
                    Dim oBookMark As New COL_BookMark

                    oNode = oNodi.Item(i)
                    oBookMark.CopiaForImporta(oNode.Category, BookPadreID, Session("objPersona").id, i)
                    If oBookMark.Errore = Errori_Db.None Then
                        If oBookMark.ID > 0 Then
                            If oNode.Nodes.Count > 0 Then
                                Me.EsportaSottoLink(oBookMark.ID, oNode.Nodes)
                            End If
                        End If
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    '#Region "Gestione TreeView"
    '    Private Sub Bind_CartellePersonali(Optional ByVal reset As Boolean = False)
    '        Dim oBookMark As New COL_BookMark
    '        Dim oPersona As New COL_Persona
    '        Dim oDataset As New DataSet

    '        Try
    '            Dim i, tot As Integer

    '            oPersona = Session("objPersona")
    '            oDataset = oBookMark.AlberoCartelle(Session("IdComunita"))

    '            Me.RDTdestinazioneCreaIn.Nodes.Clear()

    '            Dim nodeRoot As New RadTreeNode

    '            Try
    '                nodeRoot.Text = oResourceEsporta.getValue("oRootNodePersonali.Text")
    '                If nodeRoot.Text = "" Then
    '                    nodeRoot.Text = "Link Personali"
    '                End If
    '            Catch ex As Exception
    '                nodeRoot.Text = "Cartella base"
    '            End Try
    '            Try
    '                nodeRoot.ToolTip = Me.oResourceEsporta.getValue("oRootNodePersonali.ToolTip")
    '                If nodeRoot.ToolTip = "" Then
    '                    nodeRoot.ToolTip = nodeRoot.Text
    '                End If
    '            Catch ex As Exception
    '                nodeRoot.Text = "Link Personali"
    '            End Try

    '            nodeRoot.Expanded = True
    '            nodeRoot.ImageUrl = "root_folder.gif"
    '            nodeRoot.Value = 0
    '            nodeRoot.Category = 0

    '            RDTdestinazioneCreaIn.Nodes.Add(nodeRoot)

    '            'Me.CreateContextMenu(nodeRoot, False, True)

    '            If oDataset.Tables(0).Rows.Count > 0 Then
    '                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("BOKM_ID"), oDataset.Tables(0).Columns("BOKM_Padre_Id"), False)

    '                Dim dbRow As DataRow
    '                For Each dbRow In oDataset.Tables(0).Rows
    '                    If dbRow("RLNK_Padre_Id") = 0 Then
    '                        Dim node As RadTreeNode = CreateNode(dbRow, True, nodeRoot)
    '                        If Not IsNothing(node) Then
    '                            nodeRoot.Nodes.Add(node)
    '                            RecursivelyPopulate(dbRow, node, nodeRoot)
    '                        End If
    '                    End If
    '                Next dbRow
    '            End If

    '            Try
    '                Dim oNodeSelected As RadTreeNode
    '                oNodeSelected = Me.RDTdestinazioneCreaIn.FindNodeByValue(0)
    '                If Not IsNothing(oNodeSelected) Then
    '                    Try
    '                        Me.RDTdestinazioneCreaIn.SelectedNode.Selected = False
    '                    Catch ex As Exception

    '                    End Try
    '                    oNodeSelected.Selected = True
    '                End If
    '            Catch ex As Exception
    '                nodeRoot.Selected = True
    '            End Try

    '        Catch ex As Exception
    '            Try
    '                Me.GeneraNoNodePersonali()
    '                If IsNothing(Me.RDTdestinazioneCreaIn.SelectedNode) Then
    '                    Me.RDTdestinazioneCreaIn.Nodes(0).Selected = True
    '                End If
    '            Catch exFind As Exception

    '            End Try

    '        End Try

    '        Try
    '            Me.TXBdestinatarioEsportaIn.Text = Me.RDTdestinazioneCreaIn.SelectedNode.Text
    '        Catch ex As Exception
    '            Me.TXBdestinatarioEsportaIn.Text = ""
    '        End Try
    '    End Sub

    '    Private Sub RecursivelyPopulate(ByVal dbRow As DataRow, ByVal node As RadTreeNode, ByVal nodeFather As RadTreeNode)
    '        Dim childRow As DataRow

    '        For Each childRow In dbRow.GetChildRows("NodeRelation")
    '            Dim childNode As RadTreeNode = CreateNode(childRow, False, node)

    '            If Not (IsNothing(childNode)) Then
    '                If childNode.Category < 0 Then
    '                    If childRow.GetChildRows("NodeRelation").GetLength(0) > 0 Then
    '                        node.Nodes.Add(childNode)
    '                        RecursivelyPopulate(childRow, childNode, node)
    '                    End If
    '                Else
    '                    node.Nodes.Add(childNode)
    '                    RecursivelyPopulate(childRow, childNode, node)
    '                End If
    '            End If
    '        Next childRow
    '    End Sub
    '    Private Function CreateNode(ByVal dbRow As DataRow, ByVal expanded As Boolean, ByVal nodeFather As RadTreeNode) As RadTreeNode
    '        Dim node As New RadTreeNode

    '        Dim RLNK_padreID, BOKM_ID As Integer
    '        Dim BOKM_nome As String

    '        BOKM_ID = dbRow.Item("BOKM_ID")
    '        BOKM_nome = dbRow.Item("RLNK_nome")
    '        Dim ImageBaseDir As String
    '        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
    '        ImageBaseDir = ImageBaseDir & Me.RDTdestinazioneCreaIn.ImagesBaseDir().Replace("~", "")



    '        node.Text = BOKM_nome
    '        node.Value = BOKM_ID
    '        node.Expanded = expanded
    '        node.ImageUrl = "folder.gif"
    '        node.ImageExpanded = "folderOpen.gif"
    '        node.ToolTip = BOKM_nome
    '        node.Category = BOKM_ID
    '        node.Checkable = True

    '        node.Checkable = True

    '        Return node
    '    End Function 'CreateNode

    '    Private Function GeneraNoNodePersonali()
    '        Dim oRootNode As New RadTreeNode
    '        Dim oNode As New RadTreeNode

    '        oRootNode = New RadTreeNode
    '        oRootNode.Value = ""
    '        oRootNode.Expanded = True
    '        oRootnode.ImageUrl = "root_folder.gif"
    '        Try
    '            oRootNode.Text = oResourceEsporta.getValue("oRootNodePersonali.Text")
    '            If oRootNode.Text = "" Then
    '                oRootNode.Text = "Link Personali"
    '            End If
    '        Catch ex As Exception
    '            oRootNode.Text = "Link Personali"
    '        End Try
    '        Try
    '            oRootNode.ToolTip = oResourceEsporta.getValue("oRootNodePersonali.ToolTip")
    '            If oRootNode.ToolTip = "" Then
    '                oRootNode.ToolTip = oRootNode.Text
    '            End If
    '        Catch ex As Exception
    '            oRootNode.Text = "Link Personali"
    '        End Try

    '        oRootNode.Category = 0
    '        oRootNode.Value = 0
    '        oRootNode.Checkable = False

    '        Me.RDTdestinazioneCreaIn.Nodes.Clear()
    '        Me.RDTdestinazioneCreaIn.Nodes.Add(oRootNode)

    '        oRootNode.Selected = True
    '    End Function

    '#End Region

End Class
