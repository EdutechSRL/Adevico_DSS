Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita

Imports Telerik
Imports Telerik.WebControls


Public Class UC_CreaBookmark
    Inherits System.Web.UI.UserControl
    Protected oResource As ResourceManager

    Public Enum Inserimento
        CartellaCreata = 1
        LinkCreato = 2
        CartellaModificata = 3
        LinkModificato = 4
        LinkGiaEsistente = -1
        CartellaGiaEsistente = -2
        ErroreCreazione = -3
        ErroreModifica = -4
        ErroreGenerico = -5
    End Enum

    Private Enum VisualizzaDopo
        Inizio = -1
        Fine = -99
        DopoIl = 1
    End Enum

    Public Property CartellaDestinazioneID() As Integer
        Get
            Try
                CartellaDestinazioneID = Me.RDTdestinazioneCreaIn.SelectedNode.Value()
            Catch ex As Exception
                CartellaDestinazioneID = Me.HDN_RLNK_ID.Value
            End Try
        End Get
        Set(ByVal Value As Integer)
            Me.HDN_RLNK_ID.Value = Value
        End Set
    End Property

    Public Property BookMarkID() As Integer
        Get
            BookMarkID = Me.HDN_BOKM_ID.Value
        End Get
        Set(ByVal Value As Integer)
            Me.HDN_BOKM_ID.Value = Value
        End Set
    End Property
    Public Property BookMarkPadreID() As Integer
        Get
            BookMarkPadreID = Me.BOKM_PadreID.Value
        End Get
        Set(ByVal Value As Integer)
            Me.BOKM_PadreID.Value = Value
        End Set
    End Property
    Public Property isCartella() As Boolean
        Get
            Try
                isCartella = Me.BOKM_isCartella.Value
            Catch ex As Exception
                Me.BOKM_isCartella.Value = True
                Me.TBRurl.Visible = False
                isCartella = True
            End Try
        End Get
        Set(ByVal Value As Boolean)
            Me.BOKM_isCartella.Value = Value
            Me.TBRurl.Visible = Not Value
        End Set
    End Property
#Region "Pannello Aggiungi"

    Protected WithEvents BOKM_PadreID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents BOKM_isCartella As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_RLNK_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_BOKM_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TBLmodifica As System.Web.UI.WebControls.Table
    Protected WithEvents LBinfoModFile As System.Web.UI.WebControls.Label
    Protected WithEvents LBnomeLink_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBnomeCartellaLink_t As System.Web.UI.WebControls.Label

    Protected WithEvents TBRcreaIN As System.Web.UI.WebControls.TableRow
    Protected WithEvents RDTdestinazioneCreaIn As Telerik.WebControls.RadTreeView
    Protected WithEvents LBuploadIn_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBdestinatarioCreaIn As System.Web.UI.WebControls.TextBox

    Protected WithEvents TXBnome As System.Web.UI.WebControls.TextBox

    Protected WithEvents TBRurl As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBurl_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBurl As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBtipoLink_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoLink As System.Web.UI.WebControls.Label

    Protected WithEvents LBdescrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBdescrizione As System.Web.UI.WebControls.TextBox

    Protected WithEvents LBvisualizzaDopo_t As System.Web.UI.WebControls.Label

    Protected WithEvents RBLvisualizzaDopo As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents DDldopo As System.Web.UI.WebControls.DropDownList
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
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        If Page.IsPostBack = False Then
            Me.SetupInternazionalizzazione()
        End If
    End Sub

#Region "Bind_Dati"
    Public Function Bind_DatiBookMark(Optional ByVal PadreId As Integer = -1) As Boolean
        Dim oBookMark As New COL_BookMark


        Me.TXBdescrizione.Attributes.Add("onkeypress", "return(LimitText(this,300));")
        Try

            If IsNothing(oResource) Then
                SetCulture(Session("LinguaCode"))
            End If

            Me.Bind_TreeCartelle(True)
            If Me.BookMarkID = 0 Then
                Me.HDN_BOKM_ID.Value = 0
                Me.TXBdescrizione.Text = ""
                Me.TXBnome.Text = ""
                Me.TXBurl.Text = ""
                If Me.isCartella Then
                    Me.TBRurl.Visible = False
                    Me.LBtipoLink.Text = Me.oResource.getValue("LBtipoLink.cartella")
                Else
                    Me.TBRurl.Visible = True
                    Me.LBtipoLink.Text = Me.oResource.getValue("LBtipoLink.link")
                End If
                If PadreId > -1 Then
                    Try
                        If Not (IsNothing(Me.RDTdestinazioneCreaIn.SelectedNode)) Then
                            Me.RDTdestinazioneCreaIn.SelectedNode.Selected = False
                        End If
                        Dim oNode As RadTreeNode = Me.RDTdestinazioneCreaIn.FindNodeByValue(PadreId)
                        If Not (IsNothing(oNode)) Then
                            oNode.Selected = True
                            Me.TXBdestinatarioCreaIn.Text = oNode.Text
                        End If
                    Catch ex As Exception

                    End Try
                End If
                Me.Bind_datiVisualizzazione()
                Me.RBLvisualizzaDopo.SelectedIndex = 0
                Me.DDldopo.Visible = False
            Else
                With oBookMark
                    .ID = Me.BookMarkID
                    .Estrai()

                    If .Errore = Errori_Db.None Then
                        Me.BOKM_PadreID.Value = .Padre_Id
                        Me.BOKM_isCartella.Value = .isCartella
                        Me.TBRurl.Visible = Not .isCartella
                        Me.TXBnome.Text = .Nome
                        Me.TXBdescrizione.Text = .Descrizione
                        Me.TXBurl.Text = .Url

                        Dim oNode, SelectedNode As RadTreeNode
                        oNode = Me.RDTdestinazioneCreaIn.FindNodeByValue(.Padre_Id)
                        If Not Equals(oNode, Me.RDTdestinazioneCreaIn.SelectedNode) Then
                            SelectedNode = Me.RDTdestinazioneCreaIn.SelectedNode
                            If IsNothing(SelectedNode) = False Then
                                SelectedNode.Selected = False
                            End If
                            oNode.Selected = True
                            Me.TXBdestinatarioCreaIn.Text = oNode.Text
                        End If
                        Me.Bind_datiVisualizzazione(.OrdineVisualizzazione)

                        If .OrdineVisualizzazione = 0 Then
                            Me.RBLvisualizzaDopo.SelectedIndex = 0
                            Me.DDldopo.Visible = False
                        ElseIf .OrdineVisualizzazione > Me.DDldopo.Items.Count - 1 Then
                            Me.RBLvisualizzaDopo.SelectedIndex = 1
                            Me.DDldopo.Visible = False
                        Else
                            Me.RBLvisualizzaDopo.SelectedIndex = 2
                            Me.DDldopo.Visible = True
                        End If
                        If .isFromComunita Then
                            If Me.isCartella Then
                                Me.LBtipoLink.Text = Me.oResource.getValue("LBtipoLink.cartella.importata")
                            Else
                                Me.LBtipoLink.Text = Me.oResource.getValue("LBtipoLink.link.importata")
                            End If
                        Else
                            If Me.isCartella Then
                                Me.LBtipoLink.Text = Me.oResource.getValue("LBtipoLink.cartella")
                            Else
                                Me.LBtipoLink.Text = Me.oResource.getValue("LBtipoLink.link")
                            End If
                        End If

                        If Me.isCartella Then
                            Me.TBRurl.Visible = False
                        Else
                            Me.TBRurl.Visible = True
                        End If
                    Else
                        Me.HDN_RLNK_ID.Value = 0
                        Me.Bind_datiVisualizzazione()
                        Me.HDN_RLNK_ID.Value = 0
                        Me.TXBdescrizione.Text = ""
                        Me.TXBnome.Text = ""
                        Me.TXBurl.Text = ""
                        If Me.isCartella Then
                            Me.TBRurl.Visible = False
                            Me.LBtipoLink.Text = Me.oResource.getValue("LBtipoLink.cartella")
                        Else
                            Me.TBRurl.Visible = True
                            Me.LBtipoLink.Text = Me.oResource.getValue("LBtipoLink.link")
                        End If
                        Me.Bind_datiVisualizzazione()
                        Me.RBLvisualizzaDopo.SelectedIndex = 0
                        Me.DDldopo.Visible = False
                    End If
                End With
            End If

            Me.LBnomeLink_t.Visible = Not Me.isCartella
            Me.LBnomeCartellaLink_t.Visible = Me.isCartella

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub Bind_datiVisualizzazione(Optional ByVal SelezionaID As Integer = -1)
        Dim i, totale As Integer
        Dim oBookMark As COL_BookMark
        Dim oDataset As DataSet

        Try
            oDataset = oBookMark.ElencaForOrdineVisualizzazione(Session("objPersona").id, Me.RDTdestinazioneCreaIn.SelectedNode.Value, Me.HDN_BOKM_ID.Value)

            Me.DDldopo.DataSource = oDataset
            Me.DDldopo.DataTextField = "BOKM_Nome"
            Me.DDldopo.DataValueField = "BOKM_OrdineV"
            Me.DDldopo.DataBind()

            Try
                If SelezionaID > 0 Then
                    Me.DDldopo.SelectedIndex = SelezionaID - 1
                End If
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
        If Me.DDldopo.Items.Count = 0 Then
            Me.DDldopo.Items.Add(New ListItem("-inizio-", -1))
            Me.oResource.setDropDownList(Me.DDldopo, -1)
        End If
    End Sub


#Region "Gestione TreeView"
    Private Sub Bind_TreeCartelle(Optional ByVal reset As Boolean = False)
        Dim oBookMark As New COL_BookMark
        Dim oPersona As New COL_Persona
        Dim oDataset As New DataSet

        Try
            Dim i, tot As Integer


            oPersona = Session("objPersona")
            oDataset = oBookMark.AlberoCartelle(Session("objPersona").id)

            Me.RDTdestinazioneCreaIn.Nodes.Clear()

            Dim nodeRoot As New RadTreeNode

            Try
                nodeRoot.Text = oResource.getValue("oRootNode.Text")
                If nodeRoot.Text = "" Then
                    nodeRoot.Text = "Cartella base"
                End If
            Catch ex As Exception
                nodeRoot.Text = "Cartella base"
            End Try
            Try
                nodeRoot.ToolTip = oResource.getValue("oRootNode.ToolTip")
                If nodeRoot.ToolTip = "" Then
                    nodeRoot.ToolTip = nodeRoot.Text
                End If
            Catch ex As Exception
                nodeRoot.Text = "Cartella base"
            End Try

            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "root_folder.gif"
            nodeRoot.Value = 0
            nodeRoot.Category = 0

            RDTdestinazioneCreaIn.Nodes.Add(nodeRoot)

            'Me.CreateContextMenu(nodeRoot, False, True)

            If oDataset.Tables(0).Rows.Count > 0 Then
                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("BOKM_id"), oDataset.Tables(0).Columns("BOKM_Padre_Id"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("BOKM_Padre_Id") = 0 Then
                        Dim node As RadTreeNode = CreateNode(dbRow, True, nodeRoot)
                        If Not IsNothing(node) Then
                            nodeRoot.Nodes.Add(node)
                            RecursivelyPopulate(dbRow, node, nodeRoot)
                        End If
                    End If
                Next dbRow
            End If

            Try
                Dim oNodeSelected As RadTreeNode
                oNodeSelected = Me.RDTdestinazioneCreaIn.FindNodeByValue(Me.BOKM_PadreID.Value)
                If Not IsNothing(oNodeSelected) Then
                    Try
                        Me.RDTdestinazioneCreaIn.SelectedNode.Selected = False
                    Catch ex As Exception

                    End Try
                    oNodeSelected.Selected = True
                End If
            Catch ex As Exception
                nodeRoot.Selected = True
            End Try

        Catch ex As Exception
            Try
                Me.GeneraNoNode()
                If IsNothing(Me.RDTdestinazioneCreaIn.SelectedNode) Then
                    Me.RDTdestinazioneCreaIn.Nodes(0).Selected = True
                End If
            Catch exFind As Exception

            End Try

        End Try

        Try
            Me.TXBdestinatarioCreaIn.Text = Me.RDTdestinazioneCreaIn.SelectedNode.Text
        Catch ex As Exception
            Me.TXBdestinatarioCreaIn.Text = ""
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

        Dim BOKM_id, BOKM_Padre_Id, BOKM_Livello As Integer
        Dim BOKM_nome As String
        Dim BOKM_isCartella As Boolean


        BOKM_id = dbRow.Item("BOKM_id")
        BOKM_Padre_Id = dbRow.Item("BOKM_Padre_Id")
        BOKM_Livello = dbRow.Item("BOKM_Livello")
        BOKM_isCartella = dbRow.Item("BOKM_isCartella")
        BOKM_nome = dbRow.Item("BOKM_nome")

        Dim ImageBaseDir As String
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & Me.RDTdestinazioneCreaIn.ImagesBaseDir().Replace("~", "")



        node.Text = BOKM_nome
        node.Value = BOKM_id
        node.Expanded = expanded
        node.ImageUrl = "folder.gif"
        node.ImageExpandedUrl = "folderOpen.gif"
        node.ToolTip = BOKM_nome
        node.Category = BOKM_id
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
            oRootNode.Text = oResource.getValue("oRootNode.Text")
            If oRootNode.Text = "" Then
                oRootNode.Text = "Cartella base"
            End If
        Catch ex As Exception
            oRootNode.Text = "Cartella base"
        End Try
        Try
            oRootNode.ToolTip = oResource.getValue("oRootNode.ToolTip")
            If oRootNode.ToolTip = "" Then
                oRootNode.ToolTip = oRootNode.Text
            End If
        Catch ex As Exception
            oRootNode.Text = "Cartella base"
        End Try

        oRootNode.Category = 0
        oRootNode.Value = 0
        oRootNode.Checkable = False

        Me.RDTdestinazioneCreaIn.Nodes.Clear()
        Me.RDTdestinazioneCreaIn.Nodes.Add(oRootNode)

        oRootNode.Selected = True
    End Function

#End Region

#End Region

    'passo lo stream alla funzione, tutto il resto è nell'oggetto "file_disponibile"
    Public Function AggiungiLink() As Inserimento
        Dim oBookMark As New COL_BookMark
        Dim oPersona As New COL_Persona
        Dim BOKM_PadreId, Livello As Integer

        Try
            oPersona = Session("ObjPersona")

            Try
                BOKM_PadreId = Me.RDTdestinazioneCreaIn.SelectedNode.Value
            Catch ex As Exception
                BOKM_PadreId = Me.BOKM_PadreID.Value
            End Try

            With oBookMark
                .CMNT_ID = -1
                .Padre_Id = BOKM_PadreId
                .PRSN_ID = Session("objPersona").id
                .Descrizione = Me.TXBdescrizione.Text
                .isCartella = Me.isCartella
                .Livello = 0
                .Nome = Me.TXBnome.Text
                .Url = Me.TXBurl.Text
                If Me.RBLvisualizzaDopo.SelectedValue = Me.VisualizzaDopo.DopoIl Then
                    .OrdineVisualizzazione = Me.DDldopo.SelectedIndex + 1
                Else
                    .OrdineVisualizzazione = Me.RBLvisualizzaDopo.SelectedValue
                End If
                .Aggiungi()

                If .Errore = Errori_Db.None Then
                    If .isCartella Then
                        Return Inserimento.CartellaCreata
                    Else
                        Return Inserimento.LinkCreato
                    End If
                Else
                    Return Inserimento.ErroreCreazione
                End If
            End With
        Catch ex As Exception
            Return Inserimento.ErroreGenerico
        End Try
        Return Inserimento.ErroreGenerico
    End Function

    Public Function ModificaLink() As Inserimento
        Dim iResponse As Inserimento = Inserimento.ErroreGenerico
        Dim oBookMark As New COL_BookMark
        Dim oPersona As New COL_Persona
        Dim CMNT_ID, BOKM_PadreId, Livello As Integer

        Try
            Dim virtualPath As String
            oPersona = Session("ObjPersona")

            Try

                BOKM_PadreId = Me.RDTdestinazioneCreaIn.SelectedNode.Value
            Catch ex As Exception
                BOKM_PadreId = Me.BOKM_PadreID.Value
            End Try


            With oBookMark
                .ID = Me.HDN_BOKM_ID.Value
                .Estrai()

                .Padre_Id = BOKM_PadreId
                .PRSN_ID = Session("objPersona").id
                .Descrizione = Me.TXBdescrizione.Text
                .isCartella = Me.isCartella
                .Nome = Me.TXBnome.Text
                .Url = Me.TXBurl.Text

                If Me.RBLvisualizzaDopo.SelectedValue = Me.VisualizzaDopo.DopoIl Then
                    .OrdineVisualizzazione = Me.DDldopo.SelectedIndex + 1
                Else
                    .OrdineVisualizzazione = Me.RBLvisualizzaDopo.SelectedValue
                End If

                .Modifica()
                If .Errore = Errori_Db.None Then
                    If .isCartella Then
                        Return Inserimento.CartellaModificata
                    Else
                        Return Inserimento.LinkModificato
                    End If
                Else
                    Return Inserimento.ErroreModifica
                End If
            End With
        Catch ex As Exception
            Return Inserimento.ErroreGenerico
        End Try
        Return Inserimento.ErroreGenerico
    End Function

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_UC_creaBookmark"
        oResource.Folder_Level1 = "Generici"
        oResource.Folder_Level2 = "UC_Bookmark"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBinfoModFile)
            .setLabel(Me.LBuploadIn_t)
            .setLabel(Me.LBnomeLink_t)
            .setLabel(Me.LBnomeCartellaLink_t)
            .setLabel(LBdescrizione_t)
            .setLabel(Me.LBurl_t)
            .setLabel(Me.LBtipoLink_t)

            .setLabel(LBvisualizzaDopo_t)
            .setRadioButtonList(Me.RBLvisualizzaDopo, -1)
            .setRadioButtonList(Me.RBLvisualizzaDopo, 1)
            .setRadioButtonList(Me.RBLvisualizzaDopo, -99)
        End With
    End Sub
#End Region


    Private Sub RBLvisualizzaDopo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLvisualizzaDopo.SelectedIndexChanged
        If Me.RBLvisualizzaDopo.SelectedValue = Me.VisualizzaDopo.DopoIl Then
            Me.DDldopo.Visible = True
        Else
            Me.DDldopo.Visible = False
        End If
    End Sub

    Private Sub RDTdestinazioneCreaIn_NodeClick(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles RDTdestinazioneCreaIn.NodeClick
        Me.Bind_datiVisualizzazione(Me.DDldopo.SelectedIndex)
    End Sub
End Class
