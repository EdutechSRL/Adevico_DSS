Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.UCServices

Imports COL_BusinessLogic_v2.Comunita
Imports Telerik
Imports Telerik.WebControls

Public Class UC_GestioneLink
    Inherits System.Web.UI.UserControl
    Private oResourceGestione As ResourceManager


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
    Protected WithEvents CTRLdettagliLink As Comunita_OnLine.UC_DettagliLink
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
        oResourceGestione.ResourcesName = "pg_UC_gestioneRaccolta"
        oResourceGestione.Folder_Level1 = "Generici"
        oResourceGestione.Folder_Level2 = "UC_Link"
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
        Me.Bind_DatiRaccolta(0)
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
        Me.HidePannelli()
        Me.Bind_MenuContestuale()
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
#End Region

#Region "Bind_Dati"
    Private Sub Bind_DatiRaccolta(ByVal selectNode_Id As Integer)
        Dim oRaccoltaLink As New COL_RaccoltaLink
        Dim oDataset As New DataSet

        Try
            oDataset = oRaccoltaLink.ElencaByComunita(Session("idComunita"), Session("objPersona").id)

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
                    nodeRoot.Text = "* Raccolta Link"
                End If
            Catch ex As Exception
                nodeRoot.Text = "* Raccolta Link"
            End Try
            Try
                nodeRoot.ToolTip = Me.oResourceGestione.getValue("oRootNodeCartelle.ToolTip")
                If nodeRoot.ToolTip = "" Then
                    nodeRoot.ToolTip = nodeRoot.Text
                End If
            Catch ex As Exception
                nodeRoot.Text = "* Raccolta Link"
            End Try

            Me.RDTgestioneRaccolta.Nodes.Add(nodeRoot)
            If oDataset.Tables(0).Rows.Count = 0 Then
                Me.GeneraNoNodeForManagement()
            Else
                Dim oServizio As New Services_RaccoltaLink
                Dim ForDelete, ForChange, ForAdd, ForDettagli, ForImport, ForExport, forDeassocia As Boolean

                oServizio.PermessiAssociati = Me.GetPermessiForPage(oServizio.Codex)

                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("RLNK_ID"), oDataset.Tables(0).Columns("RLNK_Padre_Id"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("RLNK_Padre_Id") = 0 Then
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
        Dim oData, oDataAttuale As DateTime
        Dim childRow As DataRow

        For Each childRow In dbRow.GetChildRows("NodeRelation")
            Dim childNode As RadTreeNode = CreateNodeForManagement(childRow, False, selectNode_Id)
            node.Nodes.Add(childNode)
            Me.RecursivelyPopulateForManagement(childRow, childNode, selectNode_Id)
        Next childRow
    End Sub
    Private Function CreateNodeForManagement(ByVal dbRow As DataRow, ByVal expanded As Boolean, Optional ByVal selectNode_Id As Integer = -1) As RadTreeNode
        Dim oServizio As New Services_RaccoltaLink
        Dim node As New RadTreeNode

        oServizio.PermessiAssociati = Me.GetPermessiForPage(oServizio.Codex)

        Dim RLNK_CMNT_ID, RLNK_ID, RLNK_CreatoreID, RLNK_BOKM_ID As Integer
        Dim RLNK_Nome, RLNK_Url As String
        Dim RLNK_isCartella, RLNK_FromBookmark As Boolean

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

        RLNK_isCartella = dbRow.Item("RLNK_isCartella")
        RLNK_FromBookmark = dbRow.Item("RLNK_FromBookmark")
        RLNK_BOKM_ID = dbRow.Item("RLNK_BOKM_ID")

        node.ToolTip = RLNK_Nome
        node.Value = RLNK_ID
        node.Expanded = expanded
        node.Category = RLNK_isCartella
        node.Checkable = True
        If selectNode_Id = RLNK_ID Then
            node.Selected = True
        End If


        'RLNK_FromBookmark --> è sotto un bookmark inserito: solo dettagli !
        If RLNK_isCartella = False Then
            node.ImageUrl = "html.gif"
            node.DropEnabled = False
        Else
            node.DropEnabled = False
            node.ImageUrl = "folder.gif"
            node.ImageExpandedUrl = "FolderOpen.gif"
        End If

        node.Text = RLNK_Nome
        If Not dbRow.Item("RLNK_visibile") Then
            node.Text = node.Text & Me.oResourceGestione.getValue("visibile.false")
        End If
        Return node
    End Function 'CreateNode
    Private Sub RDTgestioneRaccolta_NodeClick(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles RDTgestioneRaccolta.NodeClick
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
                oRootNode.Text = "* Raccolta Link"
            End If
        Catch ex As Exception
            oRootNode.Text = "* Raccolta Link"
        End Try
        Try
            oRootNode.ToolTip = Me.oResourceGestione.getValue("oRootNodeCartelle.ToolTip")
            If oRootNode.ToolTip = "" Then
                oRootNode.ToolTip = oRootNode.Text
            End If
        Catch ex As Exception
            oRootNode.Text = "* Raccolta Link"
        End Try

        'oNode = New RadTreeNode
        'oNode.Expanded = True
        'oNode.Value = -99
        'oNode.Category = False
        'oNode.Selected = False
        'oNode.Checkable = False
        'Try
        '    oNode.Text = oResourceGestione.getValue("oNoNodeLink.Text")
        '    If oNode.Text = "" Then
        '        oNode.Text = "Nessun link presente"
        '    End If
        'Catch ex As Exception
        '    oNode.Text = "Nessun link presente"
        'End Try
        'Try
        '    oNode.ToolTip = oResourceGestione.getValue("oNoNodeLink.ToolTip")
        '    If oNode.ToolTip = "" Then
        '        oNode.ToolTip = oNode.Text
        '    End If
        'Catch ex As Exception
        '    oNode.Text = "Nessun link presente"
        'End Try
        'oRootNode.Nodes.Add(oNode)

        Me.RDTgestioneRaccolta.Nodes.Clear()
        Me.RDTgestioneRaccolta.Nodes.Add(oRootNode)

    End Function
#End Region

#End Region

    Private Sub Bind_MenuContestuale()
        Dim oServizioRaccoltaLink As New Services_RaccoltaLink

        Try
            If Me.ViewState("PermessiAssociati") = "" Then
                Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioRaccoltaLink.Codex)
            End If
            oServizioRaccoltaLink.PermessiAssociati = Me.ViewState("PermessiAssociati")
        Catch ex As Exception
            oServizioRaccoltaLink.PermessiAssociati = "00000000000000000000000000000000"
        End Try
    End Sub
    Private Sub HidePannelli()
        Me.PNLconfermaElimina.Visible = False
        Me.PNLdettagli.Visible = False
    End Sub

    Private Sub Bind_Dettagli(ByVal isCartella As Boolean, ByVal RLNK_Id As Integer)
        If RLNK_Id > 0 Then
            If isCartella = True Then
                Me.PNLdettagli.Visible = True
                Me.CTRLdettagliLink.Bind_dettagli(RLNK_Id)
            Else
                Me.PNLdettagli.Visible = True
                Me.CTRLdettagliLink.Bind_dettagli(RLNK_Id)
            End If
        End If
        Me.Bind_MenuContestuale()
    End Sub

    Private Sub Bind_DatiLink(ByVal Link_ID As Integer)
        Me.HidePannelli()

        If Link_ID = 0 Then
            RaiseEvent AggiornaMenu(0, True)
        Else
            Dim oRaccolta As New COL_RaccoltaLink
            With oRaccolta
                .ID = Link_ID
                .Estrai()
                If .Errore = Errori_Db.None Then
                    Me.Bind_Dettagli(Link_ID)
                    RaiseEvent AggiornaMenu(Link_ID, .isCartella)
                End If
            End With
        End If
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
        Dim oRaccolta As New COL_RaccoltaLink
        Me.PNLconfermaElimina.Visible = Show
        Me.PNLdettagli.Visible = Not Show
        If IsNothing(oResourceGestione) Then
            SetCulture(Session("LinguaCode"))
        End If
        Me.LBconfermaElimina.Text = Me.oResourceGestione.getValue("Elimina." & CType(oElimina, Elimina))
    End Sub

End Class