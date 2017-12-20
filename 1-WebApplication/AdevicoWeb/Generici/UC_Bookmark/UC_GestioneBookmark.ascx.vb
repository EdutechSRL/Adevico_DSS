Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita

Imports Telerik
Imports Telerik.WebControls

Public Class UC_GestioneBookmark
    Inherits System.Web.UI.UserControl
    Protected oResourceGestione As ResourceManager

    Public Event AggiornaMenu(ByVal LinkId As Integer, ByVal isCartella As Boolean)
    Public Enum Elimina
        NessunaAzione = 0
        EliminaLink = 1
        EliminaDir = 2
        EliminaContenuto = 3
    End Enum
    Public ReadOnly Property CartellaCorrente() As Integer
        Get
            Try
                If Me.RDTgestioneRaccolta.SelectedNode.Category = True Then
                    CartellaCorrente = Me.RDTgestioneRaccolta.SelectedNode.Value()
                Else
                    CartellaCorrente = Me.RDTgestioneRaccolta.SelectedNode.Parent.Value()
                End If

            Catch ex As Exception
                CartellaCorrente = 0
            End Try
        End Get
    End Property

    Public ReadOnly Property LinkCorrenteID() As Integer
        Get
            Try
                If Me.RDTgestioneRaccolta.SelectedNode.Category = False Then
                    LinkCorrenteID = Me.RDTgestioneRaccolta.SelectedNode.Value()
                Else
                    LinkCorrenteID = -1
                End If

            Catch ex As Exception
                LinkCorrenteID = -1
            End Try
        End Get
    End Property

#Region ""
    Protected WithEvents RDTgestioneRaccolta As Telerik.WebControls.RadTreeView
#Region "Dettagli Link"
    Protected WithEvents PNLdettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents CTRLdettagliLink As Comunita_OnLine.UC_DettagliBookmark
#End Region

#Region "Conferma Cancellazione"
    Protected WithEvents PNLconfermaElimina As System.Web.UI.WebControls.Panel
    Protected WithEvents LBconfermaElimina As System.Web.UI.WebControls.Label
#End Region

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
        If IsNothing(oResourceGestione) Then
            SetCulture(Session("LinguaCode"))
        End If
        If Page.IsPostBack = False Then
            Me.SetupInternazionalizzazione()
        End If
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceGestione = New ResourceManager

        oResourceGestione.UserLanguages = Code
        oResourceGestione.ResourcesName = "pg_UC_GestioneBookmark"
        oResourceGestione.Folder_Level1 = "Generici"
        oResourceGestione.Folder_Level2 = "UC_Bookmark"
        oResourceGestione.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceGestione
            .setLabel(Me.LBconfermaElimina)
        End With
    End Sub
#End Region

    Public Sub SetupIniziale(Optional ByVal SelezionaId As Integer = 0)
        If IsNothing(oResourceGestione) Then
            SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        End If
        Me.Bind_DatiRaccolta(SelezionaId)
        Me.HidePannelli()
        Try
            If Not IsNothing(Me.RDTgestioneRaccolta.SelectedNode) Then
                If Me.RDTgestioneRaccolta.SelectedNode.Value <> SelezionaId Then
                    Me.RDTgestioneRaccolta.SelectedNode.Selected = False
                End If
            End If
            Dim oNode As Telerik.WebControls.RadTreeNode
            oNode = Me.RDTgestioneRaccolta.FindNodeByValue(SelezionaId)
            If Not IsNothing(oNode) Then
                oNode.Selected = True
                oNode.Expanded = True
            End If
            Me.Bind_Dettagli(Me.RDTgestioneRaccolta.SelectedNode.Value)

        Catch ex As Exception

        End Try


    End Sub
    Public Sub Bind_Dettagli(ByVal LinkID As Integer)
        Try
            If LinkID > 0 Then
                Me.PNLdettagli.Visible = True
                Me.CTRLdettagliLink.Bind_dettagli(LinkID)
            Else
                Me.PNLdettagli.Visible = False
            End If
        Catch ex As Exception
            Me.PNLdettagli.Visible = False
        End Try
    End Sub

    Public Sub Bind_forDelete(ByVal Show As Boolean, Optional ByVal oElimina As Elimina = Elimina.NessunaAzione)
        Dim oBook As New COL_BookMark
        Me.PNLconfermaElimina.Visible = Show
        Me.PNLdettagli.Visible = Not Show
        If IsNothing(oResourceGestione) Then
            SetCulture(Session("LinguaCode"))
        End If
        Me.LBconfermaElimina.Text = Me.oResourceGestione.getValue("Elimina." & CType(oElimina, Elimina))
    End Sub

#Region "Bind_Dati"
    Private Sub Bind_DatiRaccolta(ByVal selectNode_Id As Integer)
        Dim oBookMark As New COL_BookMark
        Dim oDataset As New DataSet

        Try
            oDataset = oBookMark.ElencaByUser(Session("objPersona").id)

            Me.RDTgestioneRaccolta.Nodes.Clear()

            Dim nodeRoot As New RadTreeNode
            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "folder.gif"
            nodeRoot.Value = 0
            nodeRoot.Category = True
            nodeRoot.Selected = False
            nodeRoot.Checked = False
            Try
                nodeRoot.Text = oResourceGestione.getValue("oRootNodeCartelle.Text")
                If nodeRoot.Text = "" Then
                    nodeRoot.Text = "* Link Personali"
                End If
            Catch ex As Exception
                nodeRoot.Text = "* Link Personali"
            End Try
            Try
                nodeRoot.ToolTip = Me.oResourceGestione.getValue("oRootNodeCartelle.ToolTip")
                If nodeRoot.ToolTip = "" Then
                    nodeRoot.ToolTip = nodeRoot.Text
                End If
            Catch ex As Exception
                nodeRoot.Text = "* Link Personali"
            End Try

            Me.RDTgestioneRaccolta.Nodes.Add(nodeRoot)
            If oDataset.Tables(0).Rows.Count = 0 Then
                Me.GeneraNoNodeForManagement()
            Else
                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("BOKM_ID"), oDataset.Tables(0).Columns("BOKM_Padre_Id"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("BOKM_Padre_Id") = 0 Then
                        Dim node As RadTreeNode = CreateNodeForManagement(dbRow, True, selectNode_Id)
                        nodeRoot.Nodes.Add(node)
                        RecursivelyPopulateForManagement(dbRow, node, selectNode_Id)
                    End If
                Next dbRow
                If selectNode_Id >= 0 Then
                    Dim oNode As Telerik.WebControls.RadTreeNode
                    oNode = Me.RDTgestioneRaccolta.FindNodeByValue(selectNode_Id)
                    If Not IsNothing(oNode) Then
                        oNode.ExpandParentNodes()
                    End If
                End If
            End If
        Catch ex As Exception
            Me.RDTgestioneRaccolta.Nodes.Clear()
            Me.GeneraNoNodeForManagement()
        End Try
    End Sub

#Region "Gestione TreeView Raccolta Link"
    Private Sub RecursivelyPopulateForManagement(ByVal dbRow As DataRow, ByVal node As RadTreeNode, Optional ByVal selectNode_Id As Integer = -1)
        Dim childRow As DataRow

        For Each childRow In dbRow.GetChildRows("NodeRelation")
            Dim childNode As RadTreeNode = CreateNodeForManagement(childRow, False, selectNode_Id)
            node.Nodes.Add(childNode)
            Me.RecursivelyPopulateForManagement(childRow, childNode, selectNode_Id)
        Next childRow
    End Sub
    Private Function CreateNodeForManagement(ByVal dbRow As DataRow, ByVal expanded As Boolean, Optional ByVal selectNode_Id As Integer = -1) As RadTreeNode
        Dim node As New RadTreeNode

        Dim BOKM_CMNT_ID, BOKM_ID, BOKM_RLNK_ID As Integer
        Dim BOKM_Nome, BOKM_Url As String
        Dim BOKM_isCartella, BOKM_fromComunita As Boolean

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

        BOKM_isCartella = dbRow.Item("BOKM_isCartella")
        BOKM_fromComunita = dbRow.Item("BOKM_fromComunita")

        node.ToolTip = BOKM_Nome
        node.Value = BOKM_ID
        node.Expanded = expanded
        node.Category = BOKM_isCartella
        node.Checkable = True
        If selectNode_Id = BOKM_ID Then
            node.Selected = True
        End If


        'RLNK_FromBookmark --> è sotto un bookmark inserito: solo dettagli !
        If BOKM_isCartella = False Then
            node.ImageUrl = "html.gif"
            node.DropEnabled = False
        Else
            node.DropEnabled = False
            node.ImageUrl = "folder.gif"
            node.ImageExpandedUrl = "FolderOpen.gif"
        End If
        node.Text = BOKM_Nome

        Return node
    End Function 'CreateNode
    Private Sub RDTgestioneRaccolta_NodeClick(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles RDTgestioneRaccolta.NodeClick
        Me.PNLconfermaElimina.Visible = False
        Me.Bind_DatiLink(e.NodeClicked.Value)

    End Sub
    Private Function GeneraNoNodeForManagement()
        Dim oRootNode As New RadTreeNode
        Dim oNode As New RadTreeNode

        oRootNode = New RadTreeNode
        oRootNode.Value = 0
        oRootNode.Expanded = True
        oRootNode.ImageUrl = "folder.gif"
        oRootNode.Category = True
        oRootNode.Selected = False
        oRootNode.Checkable = False

        Try
            oRootNode.Text = oResourceGestione.getValue("oRootNodeCartelle.Text")
            If oRootNode.Text = "" Then
                oRootNode.Text = "* Link Personali"
            End If
        Catch ex As Exception
            oRootNode.Text = "* Link Personali"
        End Try
        Try
            oRootNode.ToolTip = Me.oResourceGestione.getValue("oRootNodeCartelle.ToolTip")
            If oRootNode.ToolTip = "" Then
                oRootNode.ToolTip = oRootNode.Text
            End If
        Catch ex As Exception
            oRootNode.Text = "* Link Personali"
        End Try

        Me.RDTgestioneRaccolta.Nodes.Clear()
        Me.RDTgestioneRaccolta.Nodes.Add(oRootNode)

    End Function
#End Region

    Private Sub Bind_DatiLink(ByVal BOKM_Id As Integer)
        Me.HidePannelli()

        If BOKM_Id = 0 Then
            RaiseEvent AggiornaMenu(0, True)
        Else
            Dim oBookMark As New COL_BookMark
            With oBookMark
                .ID = BOKM_Id
                .Estrai()
                If .Errore = Errori_Db.None Then
                    Me.Bind_Dettagli(BOKM_Id)
                    RaiseEvent AggiornaMenu(BOKM_Id, .isCartella)
                End If
            End With
        End If
    End Sub

#End Region

    Private Sub HidePannelli()
        Me.PNLconfermaElimina.Visible = False
        Me.PNLdettagli.Visible = False
    End Sub
End Class