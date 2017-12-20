Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita

Imports Telerik
Imports Telerik.WebControls


Public Class UC_ImportaLink
    Inherits System.Web.UI.UserControl
    Protected oResourceImporta As ResourceManager

    Public Enum Import
        ErroreInserimento = -1
        NessunLinkSelezionato = 0
        ErroreGenerico = -2
        Ok = 1
        Giàeffettuato = 2
    End Enum
    Public Property HasImport() As Boolean
        Get
            If Me.HDN_hasImport.Value = "" Then
                HasImport = False
            Else
                HasImport = Me.HDN_hasImport.Value
            End If
        End Get
        Set(ByVal Value As Boolean)
            Me.HDN_hasImport.Value = Value
        End Set
    End Property
    Public ReadOnly Property CartellaDestinazioneID() As Integer
        Get
            Try
                CartellaDestinazioneID = Me.RDTdestinazioneImportaIn.SelectedNode.Value
            Catch ex As Exception
                CartellaDestinazioneID = 0
            End Try
        End Get
    End Property

    Protected WithEvents HDN_hasImport As System.Web.UI.HtmlControls.HtmlInputHidden

#Region "Contenuto"
    Protected WithEvents PNLimporta As System.Web.UI.WebControls.Panel
    Protected WithEvents RDTimporta As Telerik.WebControls.RadTreeView
    Protected WithEvents LBimporta_t As System.Web.UI.WebControls.Label

    Protected WithEvents LBvisibile_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBXvisibile As System.Web.UI.WebControls.CheckBox

    Protected WithEvents TBRcreaIN As System.Web.UI.WebControls.TableRow
    Protected WithEvents RDTdestinazioneImportaIn As Telerik.WebControls.RadTreeView
    Protected WithEvents LBuploadIn_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBdestinatarioImportaIn As System.Web.UI.WebControls.TextBox
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
        If IsNothing(oResourceImporta) Then
            SetCulture(Session("LinguaCode"))
        End If
        If Page.IsPostBack = False Then
            Me.SetupInternazionalizzazione()
        End If
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceImporta = New ResourceManager

        oResourceImporta.UserLanguages = Code
        oResourceImporta.ResourcesName = "pg_UC_ImportaLink"
        oResourceImporta.Folder_Level1 = "Generici"
        oResourceImporta.Folder_Level2 = "UC_Link"
        oResourceImporta.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceImporta
            .setLabel(Me.LBuploadIn_t)
            .setLabel(Me.LBimporta_t)
            .setLabel(Me.LBvisibile_t)
            .setCheckBox(Me.CBXvisibile)
        End With
    End Sub
#End Region

#Region "Bind_Dati"
    Public Function SetupIniziale() As Boolean
        Dim oRaccoltaLink As New COL_RaccoltaLink

        If IsNothing(oResourceImporta) Then
            SetCulture(Session("LinguaCode"))
        End If

        Try
            HDN_hasImport.Value = False
            Me.Bind_TreeCartelle(True)
            Me.BindTree_Forimporta()

            If Me.HDN_hasImport.Value = "" Then
                Me.HDN_hasImport.Value = ""
            End If
            If Me.HDN_hasImport.Value = "" Or Me.HDN_hasImport.Value = False Then
                Me.oResourceImporta.setLabel_To_Value(Me.LBimporta_t, "LBimporta_t.false")
            Else
                Me.oResourceImporta.setLabel_To_Value(LBimporta_t, "LBimporta_t.true")
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#Region "Gestione TreeView"
    Private Sub Bind_TreeCartelle(Optional ByVal reset As Boolean = False)
        Dim oRaccoltaLink As New COL_RaccoltaLink
        Dim oPersona As New COL_Persona
        Dim oDataset As New DataSet

        Try
            Dim i, tot, RLNK_id, RLNK_padreID, RLNK_Livello As Integer
            Dim RLNK_nome As String
            Dim RLNK_isFile As Integer


            oPersona = Session("objPersona")
            oDataset = oRaccoltaLink.AlberoCartelle(Session("IdComunita"))

            Me.RDTdestinazioneImportaIn.Nodes.Clear()

            Dim nodeRoot As New RadTreeNode

            Try
                nodeRoot.Text = oResourceImporta.getValue("oRootNodeCartelle.Text")
                If nodeRoot.Text = "" Then
                    nodeRoot.Text = "* Raccolta Link"
                End If
            Catch ex As Exception
                nodeRoot.Text = "* Raccolta Link"
            End Try
            Try
                nodeRoot.ToolTip = oResourceImporta.getValue("oRootNodeCartelle.ToolTip")
                If nodeRoot.ToolTip = "" Then
                    nodeRoot.ToolTip = nodeRoot.Text
                End If
            Catch ex As Exception
                nodeRoot.Text = "* Raccolta Link"
            End Try

            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "root_folder.gif"
            nodeRoot.Value = 0
            nodeRoot.Category = 0

            RDTdestinazioneImportaIn.Nodes.Add(nodeRoot)

            'Me.CreateContextMenu(nodeRoot, False, True)

            If oDataset.Tables(0).Rows.Count > 0 Then
                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("RLNK_id"), oDataset.Tables(0).Columns("RLNK_Padre_Id"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("RLNK_Padre_Id") = 0 Then
                        Dim node As RadTreeNode = CreateNode(dbRow, True, nodeRoot)
                        If Not IsNothing(node) Then
                            nodeRoot.Nodes.Add(node)
                            RecursivelyPopulate(dbRow, node, nodeRoot)
                        End If
                    End If
                Next dbRow
            End If

            Dim oNodeSelected As RadTreeNode
            If IsNothing(Me.RDTdestinazioneImportaIn.SelectedNode) Then
                oNodeSelected = Me.RDTdestinazioneImportaIn.FindNodeByValue(0)
                If Not IsNothing(oNodeSelected) Then
                    oNodeSelected.Selected = True
                End If
            End If

        Catch ex As Exception
            Try
                Me.GeneraNoNode()
                If IsNothing(Me.RDTdestinazioneImportaIn.SelectedNode) Then
                    Me.RDTdestinazioneImportaIn.Nodes(0).Selected = True
                End If
            Catch exFind As Exception

            End Try

        End Try

        Try
            Me.TXBdestinatarioImportaIn.Text = Me.RDTdestinazioneImportaIn.SelectedNode.Text
        Catch ex As Exception
            Me.TXBdestinatarioImportaIn.Text = ""
        End Try
    End Sub

    Private Sub RecursivelyPopulate(ByVal dbRow As DataRow, ByVal node As RadTreeNode, ByVal nodeFather As RadTreeNode)
        Dim childRow As DataRow

        For Each childRow In dbRow.GetChildRows("NodeRelation")
            Dim childNode As RadTreeNode = CreateNode(childRow, False, node)

            If Not (IsNothing(childNode)) Then
                If childNode.Category < 0 Then
                    If childRow.GetChildRows("NodeRelation").GetLength(0) > 0 Then
                        node.Nodes.Add(childNode)
                        RecursivelyPopulate(childRow, childNode, node)
                    End If
                Else
                    node.Nodes.Add(childNode)
                    RecursivelyPopulate(childRow, childNode, node)
                End If
            End If
        Next childRow
    End Sub
    Private Function CreateNode(ByVal dbRow As DataRow, ByVal expanded As Boolean, ByVal nodeFather As RadTreeNode) As RadTreeNode
        Dim node As New RadTreeNode

        Dim RLNK_id, RLNK_padreID, RLNK_Livello As Integer
        Dim RLNK_nome As String
        Dim RLNK_isCartella As Boolean



        RLNK_id = dbRow.Item("RLNK_id")
        RLNK_padreID = dbRow.Item("RLNK_Padre_Id")
        RLNK_Livello = dbRow.Item("RLNK_Livello")
        RLNK_isCartella = dbRow.Item("RLNK_isCartella")
        RLNK_nome = dbRow.Item("RLNK_nome")

        Dim ImageBaseDir As String
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & Me.RDTdestinazioneImportaIn.ImagesBaseDir().Replace("~", "")



        node.Text = RLNK_nome
        node.Value = RLNK_id
        node.Expanded = expanded
        node.ImageUrl = "folder.gif"
        node.ImageExpandedUrl = "folderOpen.gif"
        node.ToolTip = RLNK_nome
        node.Category = RLNK_id
        node.Checkable = True

        node.Checkable = True

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
            oRootNode.Text = Me.oResourceImporta.getValue("oRootNodeCartelle.Text")
            If oRootNode.Text = "" Then
                oRootNode.Text = "* Raccolta Link"
            End If
        Catch ex As Exception
            oRootNode.Text = "* Raccolta Link"
        End Try
        Try
            oRootNode.ToolTip = oResourceImporta.getValue("oRootNodeCartelle.ToolTip")
            If oRootNode.ToolTip = "" Then
                oRootNode.ToolTip = oRootNode.Text
            End If
        Catch ex As Exception
            oRootNode.Text = "* Raccolta Link"
        End Try

        oRootNode.Category = 0
        oRootNode.Value = 0
        oRootNode.Checkable = False

        Me.RDTdestinazioneImportaIn.Nodes.Clear()
        Me.RDTdestinazioneImportaIn.Nodes.Add(oRootNode)

        oRootNode.Selected = True
    End Function

#End Region

#End Region

#Region "Importa Dati"

#Region "Gestione Tree"

    Private Sub BindTree_Forimporta()
        Dim oBookMark As New COL_BookMark
        Dim oDataset As New DataSet

        Try
            oDataset = oBookMark.ListaForExport(Session("objPersona").id, Session("idComunita"))

            Me.RDTimporta.Nodes.Clear()

            Dim nodeRoot As New RadTreeNode
            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "folder.gif"
            nodeRoot.Value = 0
            nodeRoot.Category = 0
            nodeRoot.Checkable = False
            nodeRoot.Checked = False

            Try
                nodeRoot.Text = oResourceImporta.getValue("oRootNodeLink.Text")
                If nodeRoot.Text = "" Then
                    nodeRoot.Text = "* Raccolta Link"
                End If
            Catch ex As Exception
                nodeRoot.Text = "* Raccolta Link"
            End Try
            Try
                nodeRoot.ToolTip = oResourceImporta.getValue("oRootNodeLink.ToolTip")
                If nodeRoot.ToolTip = "" Then
                    nodeRoot.ToolTip = nodeRoot.Text
                End If
            Catch ex As Exception
                nodeRoot.Text = "* Raccolta Link"
            End Try

            Me.RDTimporta.Nodes.Add(nodeRoot)
            If oDataset.Tables(0).Rows.Count = 0 Then
                Me.GeneraNoNodePreferiti(Me.RDTimporta)
            Else
                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("BOKM_ID"), oDataset.Tables(0).Columns("BOKM_Padre_Id"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("BOKM_Padre_Id") = 0 Then
                        Dim node As RadTreeNode = CreateNodeForimporta(dbRow, True)
                        nodeRoot.Nodes.Add(node)
                        RecursivelyPopulateForimporta(dbRow, node)
                    End If
                Next dbRow
            End If
        Catch ex As Exception
            Me.RDTimporta.Nodes.Clear()
            Me.GeneraNoNodePreferiti(Me.RDTimporta)
        End Try
    End Sub
    Private Sub RecursivelyPopulateForimporta(ByVal dbRow As DataRow, ByVal node As RadTreeNode)
        Dim oData, oDataAttuale As DateTime
        Dim childRow As DataRow

        For Each childRow In dbRow.GetChildRows("NodeRelation")
            Dim childNode As RadTreeNode = CreateNodeForimporta(childRow, False)
            node.Nodes.Add(childNode)
            Me.RecursivelyPopulateForimporta(childRow, childNode)
        Next childRow
    End Sub

    Private Function CreateNodeForimporta(ByVal dbRow As DataRow, ByVal expanded As Boolean) As RadTreeNode
        Dim node As New RadTreeNode

        Dim BOKM_ID As Integer
        Dim BOKM_Nome As String
        Dim BOKM_isCartella, BOKM_Imported, isExported As Boolean

        BOKM_ID = dbRow.Item("BOKM_ID")
        BOKM_Nome = dbRow.Item("BOKM_Nome")

        BOKM_isCartella = dbRow.Item("BOKM_isCartella")
        BOKM_Imported = dbRow.Item("BOKM_Imported")
        isExported = dbRow.Item("isExported")
        node.ToolTip = BOKM_Nome
        node.Value = BOKM_ID
        node.Category = BOKM_ID

        'BOKM_FromBookmark --> è sotto un bookmark inserito: solo dettagli !
        If BOKM_isCartella = False Then
            node.ImageUrl = "html.gif"
        Else
            node.ImageUrl = "folder.gif"
            node.ImageExpandedUrl = "FolderOpen.gif"
        End If


        If isExported Then
            node.Checkable = False
        ElseIf BOKM_Imported Then
            node.Checkable = False
        Else
            node.Checkable = True
            HDN_hasImport.Value = True
        End If
        node.Text = BOKM_Nome
        Return node
    End Function 'CreateNode
    Private Function GeneraNoNodePreferiti(ByVal oTree As Telerik.WebControls.RadTreeView)
        Dim oRootNode As New RadTreeNode
        Dim oNode As New RadTreeNode

        oRootNode = New RadTreeNode
        oRootNode.Value = ""
        oRootNode.Expanded = True
        oRootNode.ImageUrl = "folder.gif"
        oRootNode.Category = 0
        oRootNode.Checkable = False
        Try
            oRootNode.Text = oResourceImporta.getValue("oRootNodeLink.Text")
            If oRootNode.Text = "" Then
                oRootNode.Text = "Link Personali"
            End If
        Catch ex As Exception
            oRootNode.Text = "Link Personali"
        End Try
        Try
            oRootNode.ToolTip = oResourceImporta.getValue("oRootNodeLink.ToolTip")
            If oRootNode.ToolTip = "" Then
                oRootNode.ToolTip = oRootNode.Text
            End If
        Catch ex As Exception
            oRootNode.Text = "Link Personali"
        End Try

        oNode = New RadTreeNode
        oNode.Expanded = True
        oNode.Value = ""
        oNode.Category = 0
        oNode.Checkable = False
        Try
            oNode.Text = oResourceImporta.getValue("oNoNodeLink.Text")
            If oNode.Text = "" Then
                oNode.Text = "Nessun link personale presente"
            End If
        Catch ex As Exception
            oNode.Text = "Nessun link personale presente"
        End Try
        Try
            oNode.ToolTip = oResourceImporta.getValue("oNoNodeLink.ToolTip")
            If oNode.ToolTip = "" Then
                oNode.ToolTip = oNode.Text
            End If
        Catch ex As Exception
            oNode.Text = "Nessun link personale presente"
        End Try
        oRootNode.Nodes.Add(oNode)

        oTree.Nodes.Clear()
        oTree.Nodes.Add(oRootNode)

    End Function
#End Region

    Public Function ImportaLink() As Import
        Try
            'importa i Link
            Dim i, totale As Integer
            Dim ListaAssociati As String = ","

            If Session("Azione") = "importa" Then
                If Me.RDTimporta.CheckedNodes.Count > 0 Then
                    Dim oArrayLink As ArrayList
                    oArrayLink = Me.RDTimporta.CheckedNodes()

                    totale = Me.RDTimporta.CheckedNodes.Count - 1
                    For i = 0 To totale
                        Dim oNode As Telerik.WebControls.RadTreeNode
                        oNode = Me.RDTimporta.CheckedNodes(i)
                        If InStr(ListaAssociati, "," & oNode.Category & ",") <= 0 Then
                            Dim oRaccoltaLink As New COL_RaccoltaLink

                            oRaccoltaLink.ImportaPreferito(Me.RDTdestinazioneImportaIn.SelectedNode.Value, oNode.Category, Session("idComunita"), Session("objPersona").id, CBXvisibile.Checked)
                            If oRaccoltaLink.Errore = Errori_Db.None Then
                                If oRaccoltaLink.ID > 0 Then
                                    If oNode.Nodes.Count > 0 Then
                                        Me.ImportaSottoLink(oRaccoltaLink.ID, oNode.Nodes)
                                    End If
                                End If
                            End If
                            oNode.Remove()
                            ListaAssociati = ListaAssociati & oNode.Category & ","
                        End If
                        totale = Me.RDTimporta.CheckedNodes.Count - 1
                        If totale < 0 Or i > totale Then
                            Exit For
                        End If
                    Next
                    Return Import.Ok
                Else
                    Return Import.NessunLinkSelezionato
                End If
            Else
                Return Import.Giàeffettuato
            End If
        Catch ex As Exception
            Return Import.ErroreGenerico
        End Try
    End Function

    Private Sub ImportaSottoLink(ByVal LinkPadreID As Integer, ByVal oNodi As Telerik.WebControls.RadTreeNodeCollection)
        Dim i, totale As Integer
        Try
            totale = oNodi.Count - 1
            If totale < 0 Then

            Else
                For i = 0 To totale
                    Dim oNode As Telerik.WebControls.RadTreeNode
                    Dim oRaccoltaLink As New COL_RaccoltaLink

                    oNode = oNodi.Item(i)
                    oRaccoltaLink.CopiaForImporta(oNode.Category, LinkPadreID, Session("objPersona").id, Session("idComunita"), i, Me.CBXvisibile.Checked)
                    If oRaccoltaLink.Errore = Errori_Db.None Then
                        If oRaccoltaLink.ID > 0 Then
                            If oNode.Nodes.Count > 0 Then
                                Me.ImportaSottoLink(oRaccoltaLink.ID, oNode.Nodes)
                            End If
                        End If
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

End Class
