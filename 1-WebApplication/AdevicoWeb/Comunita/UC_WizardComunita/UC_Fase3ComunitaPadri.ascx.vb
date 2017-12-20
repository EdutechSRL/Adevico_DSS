Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita

Imports Telerik
Imports Telerik.WebControls

Public Class UC_Fase3ComunitaPadri
    Inherits System.Web.UI.UserControl
    Private oResourceUCcomunita As ResourceManager

    Protected WithEvents HDN_ComunitaAttualePercorso As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ComunitaAttualeID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_Livello As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_hasComunitaForServizio As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ORGN_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNhasSetup As System.Web.UI.HtmlControls.HtmlInputHidden

    Public Property ShowFiltro() As Boolean
        Get
            ShowFiltro = Me.TBRchiudiFiltro.Visible
        End Get
        Set(ByVal Value As Boolean)
            Me.TBRapriFiltro.Visible = Not Value
            Me.TBRchiudiFiltro.Visible = Value
            Me.TBRfiltri.Visible = Value
        End Set
    End Property
    Public ReadOnly Property HasComunita() As Boolean
        Get
            Try
                If HDN_hasComunitaForServizio.Value = True Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Get
    End Property
    Public Property Width() As String
        Get
            'Width = Me.TBLdati.Width.ToString
            Width = Me.TBRfiltri.Width.ToString
        End Get
        Set(ByVal Value As String)
            Dim isPercent As Boolean = False
            Dim isPixel As Boolean = False
            Value = LCase(Value)
            If InStr(Value, "%") > 0 Then
                isPercent = True
                Value = Left(Value, InStr(Value, "%") - 1)
            ElseIf InStr(Value, "px") > 0 Then
                isPixel = True
                Value = Left(Value, InStr(Value, "px") - 1)
            End If

            Try
                If isPercent And CInt(Value) > 0 Then
                    'Me.TBLdati.Width = System.Web.UI.WebControls.Unit.Percentage(Value)
                    Me.TBLFiltroCom.Width = System.Web.UI.WebControls.Unit.Percentage(Value)
                    TBLDdlCom.Width = System.Web.UI.WebControls.Unit.Percentage(Value)

                ElseIf isPixel And CInt(Value) > 0 Then
                    'Me.TBLdati.Width = System.Web.UI.WebControls.Unit.Pixel(Value)
                    Me.TBLFiltroCom.Width = System.Web.UI.WebControls.Unit.Pixel(Value)
                    TBLDdlCom.Width = System.Web.UI.WebControls.Unit.Percentage(Value)
                End If
            Catch ex As Exception

            End Try

        End Set
    End Property
    Public Property LarghezzaFinestraAlbero() As String
        Get
            'Width = Me.TBLdati.Width.ToString
            LarghezzaFinestraAlbero = Me.RDTcomunita.Width.ToString
        End Get
        Set(ByVal Value As String)
            Dim isPercent As Boolean = False
            Dim isPixel As Boolean = False
            Value = LCase(Value)
            If InStr(Value, "%") > 0 Then
                isPercent = True
                Value = Left(Value, InStr(Value, "%") - 1)
            ElseIf InStr(Value, "px") > 0 Then
                isPixel = True
                Value = Left(Value, InStr(Value, "px") - 1)
            End If

            Try
                If isPercent And CInt(Value) > 0 Then
                    Me.RDTcomunita.Width = System.Web.UI.WebControls.Unit.Percentage(Value)
                ElseIf isPixel And CInt(Value) > 0 Then
                    Me.RDTcomunita.Width = System.Web.UI.WebControls.Unit.Pixel(Value)
                End If
            Catch ex As Exception

            End Try

        End Set
    End Property
    Public Property ColonneNome() As Integer
        Get
            'Width = Me.TBLdati.Width.ToString
            ColonneNome = Me.TXBdestinatario.Columns
        End Get
        Set(ByVal Value As Integer)
            Dim isPercent As Boolean = False
            Dim isPixel As Boolean = False
            Value = LCase(Value)
            If Value = 0 Then
                Value = 60
            End If

            Try
                Me.TXBdestinatario.Columns = Value
            Catch ex As Exception

            End Try

        End Set
    End Property

    Public ReadOnly Property isInizializzato() As Boolean
        Get
            Try
                isInizializzato = HDNhasSetup.Value
            Catch ex As Exception
                isInizializzato = False
            End Try
        End Get
    End Property
    Public ReadOnly Property GetPadri() As String
        Get
            Try
                If Me.RDTcomunita.CheckedNodes.Count = 0 Then
                    GetPadri = ""
                Else
                    Dim oNode As RadTreeNode
                    Dim ElencoPadri As String = ","
                    Dim NomePadri As String
                    For Each oNode In Me.RDTcomunita.CheckedNodes
                        If oNode.Enabled Then
                            If InStr(ElencoPadri, "," & oNode.Value & ",") < 1 Then
                                ElencoPadri &= oNode.Value & ","
                                If NomePadri = "" Then
                                    NomePadri = oNode.Text
                                Else
                                    NomePadri &= ", " & oNode.Text
                                End If
                            End If
                        End If
                    Next
                    GetPadri = NomePadri
                End If
            Catch ex As Exception
                GetPadri = ""
            End Try
        End Get
    End Property

#Region "Filtro"
    Protected WithEvents TBLFiltroComAll As System.Web.UI.WebControls.Table
    Protected WithEvents TBLFiltroCom As System.Web.UI.WebControls.Table
    Protected WithEvents TBLDdlCom As System.Web.UI.WebControls.Table

    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList


    Protected WithEvents LBtipoComunita_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBCtipoRicerca_c As System.Web.UI.WebControls.TableCell
    Protected WithEvents LBtipoRicerca_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBstatoComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBvalore_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLTipo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList

    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button

    Protected WithEvents LBorganizzazione_c As System.Web.UI.WebControls.Label

    Protected WithEvents DDLstatoComunita As System.Web.UI.WebControls.DropDownList
#End Region
#Region "Apri Chiudi NEW"
    Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow
    Protected WithEvents Filtro_CellFiltri As System.Web.UI.WebControls.TableCell
#End Region

    Protected WithEvents TXBdestinatario As System.Web.UI.WebControls.TextBox
    Protected WithEvents RDTcomunita As Telerik.WebControls.RadTreeView

    Private Enum StringaVisualizza
        nascondi = 0
        mostra = 1
    End Enum
    'Private Enum Iscrizioni_code
    '    IscrizioniAperteIl = 0
    '    IscrizioniChiuse = 1
    '    IscrizioniComplete = 2
    '    IscrizioniEntro = 3
    'End Enum

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
        If IsNothing(oResourceUCcomunita) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
    End Sub


    Public Sub SetupControl(ByVal Livello As Integer, ByVal ComunitaAttualeID As Integer, ByVal ComunitaAttualePercorso As String)
        If IsNothing(oResourceUCcomunita) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        Me.HDN_Livello.Value = Livello
        Me.HDN_ComunitaAttualePercorso.Value = ComunitaAttualePercorso
        Me.HDN_ComunitaAttualeID.Value = ComunitaAttualeID
        Me.HDNhasSetup.Value = True
        Me.SetupInternazionalizzazione()
        Me.SetupFiltri()
    End Sub
    Public Sub ResetControllo()
        Me.HDN_Livello.Value = ""
        Me.HDN_ComunitaAttualePercorso.Value = ""
        Me.HDN_ComunitaAttualeID.Value = ""
        Me.HDNhasSetup.Value = False
    End Sub

#Region "Bind_Filtri"

    Private Sub SetupFiltri()

        Me.Bind_TipiComunita()
        Me.Bind_Organizzazioni()
        Me.Bind_StatusComunità()
        Me.Bind_TreeComunita()

    End Sub

    Private Function SetupNomeComunita(ByVal nomeComunita As String) As String
        If InStr(nomeComunita, "<img") > 0 Then
            nomeComunita = Left(nomeComunita, InStr(nomeComunita, "<img") - 1)
        End If

        Return nomeComunita
    End Function
    Private Sub Bind_TipiComunita()
        '...nella ddl che mi farà da filtro delle tipologie di utenti associate al tipo comunità
        Dim oDataSet As New DataSet
        Dim oTipoComunita As New COL_Tipo_Comunita


        Try
            oDataSet = oTipoComunita.ElencaForFiltri(Session("LinguaID"), True)
            If oDataSet.Tables(0).Rows.Count > 0 Then
                DDLTipo.DataSource = oDataSet
                DDLTipo.DataTextField() = "TPCM_descrizione"
                DDLTipo.DataValueField() = "TPCM_id"
                DDLTipo.DataBind()

                'aggiungo manualmente elemento che indica tutti i tipi di comunità
                DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
            End If
        Catch ex As Exception
            DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try
        oResourceUCcomunita.setDropDownList(Me.DDLTipo, -1)
    End Sub
    Private Sub Bind_StatusComunità()
        Dim oPersona As New COL_Persona
        Dim totale, TotaleArchiviate, totaleBloccate As Integer
        Try
            Dim oListItem_Archiviate, oListItem_Bloccate As ListItem
            oPersona = Session("objPersona")
            oPersona.StatusComunitaIscritto(oPersona.Id, totale, TotaleArchiviate, totaleBloccate)

            oListItem_Archiviate = Me.DDLstatoComunita.Items.FindByValue(1)
            oListItem_Bloccate = Me.DDLstatoComunita.Items.FindByValue(2)
            If totaleBloccate = 0 Then
                If Not IsNothing(oListItem_Bloccate) Then
                    Me.DDLstatoComunita.Items.Remove(oListItem_Bloccate)
                End If
            Else
                If IsNothing(oListItem_Bloccate) Then
                    If IsNothing(oListItem_Archiviate) Then
                        Me.DDLstatoComunita.Items.Insert(1, New ListItem("Bloccate", 2))
                    Else
                        Me.DDLstatoComunita.Items.Insert(2, New ListItem("Bloccate", 2))
                    End If
                    Me.oResourceUCcomunita.setDropDownList(Me.DDLstatoComunita, 2)
                End If
            End If

            If TotaleArchiviate = 0 Then
                If Not IsNothing(oListItem_Archiviate) Then
                    Me.DDLstatoComunita.Items.Remove(oListItem_Archiviate)
                End If
            Else
                If IsNothing(oListItem_Archiviate) Then
                    Me.DDLstatoComunita.Items.Insert(1, New ListItem("Archiviate", 1))
                    oResourceUCcomunita.setDropDownList(Me.DDLstatoComunita, 1)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_Organizzazioni()
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona

        Me.DDLorganizzazione.Items.Clear()
        Try
            oPersona = Session("objPersona")
            oDataset = oPersona.GetOrganizzazioniAssociate()

            If oDataset.Tables(0).Rows.Count > 0 Then
                Dim oComunita As New COL_Comunita

                Dim ArrComunita() As String
                Dim ORGN_ID As Integer
                Dim show As Boolean = False
                Try
                    ArrComunita = Me.HDN_ComunitaAttualePercorso.Value.Split(".")
                    oComunita.Id = ArrComunita(1)
                    oComunita.Estrai()
                    ORGN_ID = oComunita.Organizzazione.Id
                    show = False
                Catch ex As Exception
                    show = True
                    ORGN_ID = -1
                End Try

                Me.DDLorganizzazione.DataValueField = "ORGN_id"
                Me.DDLorganizzazione.DataTextField = "ORGN_ragioneSociale"
                Me.DDLorganizzazione.DataSource = oDataset
                Me.DDLorganizzazione.DataBind()

                If Me.DDLorganizzazione.Items.Count > 1 Then
                    Me.DDLorganizzazione.Enabled = True
                    Me.DDLorganizzazione.Items.Insert(0, New ListItem("Qualsiasi", -1))
                    If ORGN_ID >= 0 Then
                        Try
                            Me.DDLorganizzazione.SelectedValue = ORGN_ID
                        Catch ex As Exception
                            Me.DDLorganizzazione.Items.Clear()
                            Me.DDLorganizzazione.Items.Add(New ListItem(oComunita.Nome, ORGN_ID))
                            Me.DDLorganizzazione.SelectedValue = ORGN_ID
                        End Try
                    Else
                        Me.DDLorganizzazione.SelectedIndex = 0
                    End If
                Else
                    Me.DDLorganizzazione.Enabled = False
                End If
            Else
                Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
                Me.DDLorganizzazione.Enabled = False
            End If
        Catch ex As Exception
            Me.DDLorganizzazione.Items.Clear()
            Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
            Me.DDLorganizzazione.Enabled = False
        End Try
        oResourceUCcomunita.setDropDownList(Me.DDLorganizzazione, -1)
        oResourceUCcomunita.setDropDownList(Me.DDLorganizzazione, 0)
        Me.DDLorganizzazione.Enabled = (Me.DDLorganizzazione.Items.Count > 1)
    End Sub

    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
        Me.Bind_TreeComunita()
    End Sub
 
    Private Sub DDLTipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipo.SelectedIndexChanged
        Dim oSelectedNode As RadTreeNode
        oSelectedNode = Me.RDTcomunita.SelectedNode
        Me.Bind_TreeComunita()
    End Sub

    Private Sub DDLstatoComunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLstatoComunita.SelectedIndexChanged
        Me.Bind_TreeComunita()
    End Sub
    Private Sub BTNCerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Me.Bind_TreeComunita()
    End Sub

    Private Function FiltraggioDati() As DataSet
        Dim oTreeComunita As New COL_TreeComunita
        Dim oPersona As New COL_Persona

        Dim i, totale, totaleHistory, TPCM_ID As Integer
        Dim Path As String
        Dim oDataset As New DataSet
        Dim ArrComunita(,) As String

        Try
            Dim oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti
            Dim oFiltroLettera As Main.FiltroComunita = Main.FiltroComunita.tutti
            Dim valore As String

            oPersona = Session("objPersona")
            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & oPersona.Id & "\"
            oTreeComunita.Nome = oPersona.Id & ".xml"

            oPersona = Session("objPersona")

            If Me.TXBValore.Text <> "" Then
                Me.TXBValore.Text = Trim(Me.TXBValore.Text)
            End If
            valore = Me.TXBValore.Text

            Dim TipoRicercaID As Integer
            TipoRicercaID = Me.DDLTipoRicerca.SelectedValue
            If valore <> "" Then
                Select Case TipoRicercaID
                    Case Main.FiltroComunita.nome
                        oFiltroTipoRicerca = Main.FiltroComunita.nome
                    Case Main.FiltroComunita.creataDopo
                        If IsDate(valore) = False Then
                            valore = ""
                        Else
                            oFiltroTipoRicerca = Main.FiltroComunita.creataDopo
                        End If
                    Case Main.FiltroComunita.creataPrima
                        If IsDate(valore) = False Then
                            valore = ""
                        Else
                            oFiltroTipoRicerca = Main.FiltroComunita.creataPrima
                        End If
                    Case Main.FiltroComunita.dataIscrizioneDopo
                        If IsDate(valore) = False Then
                            valore = ""
                        Else
                            oFiltroTipoRicerca = Main.FiltroComunita.dataIscrizioneDopo
                        End If
                    Case Main.FiltroComunita.dataFineIscrizionePrima
                        If IsDate(valore) = False Then
                            valore = ""
                        Else
                            oFiltroTipoRicerca = Main.FiltroComunita.dataFineIscrizionePrima
                        End If
                    Case Main.FiltroComunita.contiene
                        oFiltroTipoRicerca = Main.FiltroComunita.contiene
                    Case Main.FiltroComunita.cognomeDocente
                        oFiltroTipoRicerca = Main.FiltroComunita.cognomeDocente
                    Case Else
                        valore = ""
                End Select
            End If

            Dim ComunitaPadreID As Integer
            Try
                ComunitaPadreID = Me.HDN_ComunitaAttualeID.Value
                If ComunitaPadreID < 1 Then
                    ComunitaPadreID = -1
                End If
            Catch ex As Exception
                ComunitaPadreID = -1
            End Try


            Dim FacoltaID, LaureaID, PeriodoID, AAid, TipocomunitaID, TipoCdlID, StatusID As Integer
            Try
                FacoltaID = Me.DDLorganizzazione.SelectedValue
            Catch ex As Exception
                FacoltaID = -1
            End Try

            AAid = -1
            PeriodoID = -1
            Try
                TipocomunitaID = Me.DDLTipo.SelectedValue
            Catch ex As Exception
                TipocomunitaID = -1
            End Try
            TipoCdlID = -1

            Try
                StatusID = Me.DDLstatoComunita.SelectedValue
            Catch ex As Exception

            End Try

                Dim ImageBaseDir, img As String
                ImageBaseDir = GetPercorsoApplicazione(Me.Request)
                ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"
                ImageBaseDir = Replace(ImageBaseDir, "//", "/")

            
            oDataset = oTreeComunita.RicercaComunitaAlberoRidottoByServizio(Services_AmministraComunita.Codex, oPersona, Me.oResourceUCcomunita, ImageBaseDir, TipocomunitaID, FacoltaID, , , , oFiltroTipoRicerca, valore, StatusID, Me.HDN_Livello.Value)

            Catch ex As Exception

            End Try
        Return oDataset
    End Function

#Region "AlberoComunità"
    Private Sub Bind_TreeComunita()
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona
        Dim oDataset As New DataSet

        Dim CMNT_path, CMNT_pathPadre, CMNT_NomeVisibile As String


        Me.RDTcomunita.Nodes.Clear()

        Try
            Dim i, tot As Integer
            Dim nodeRoot As New RadTreeNode


            oDataset = Me.FiltraggioDati()
            Me.RDTcomunita.Nodes.Clear()

            Try
                nodeRoot.Text = oResourceUCcomunita.getValue("oRootNode.Text")
                nodeRoot.ToolTip = oResourceUCcomunita.getValue("oRootNode.ToolTip")
                If nodeRoot.Text = "" Then
                    nodeRoot.Text = "Comunità: "
                    nodeRoot.ToolTip = "Comunità: "
                End If
            Catch ex As Exception
                nodeRoot.Text = "Comunità: "
                nodeRoot.ToolTip = "Comunità: "
            End Try

            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "folder.gif"
            nodeRoot.Value = ""
            nodeRoot.Category = 0
            nodeRoot.Checkable = False
            Me.RDTcomunita.Nodes.Add(nodeRoot)

            If oDataset.Tables(0).Rows.Count = 0 Then
                Me.GeneraNoNode()
            Else
                Dim oRow As DataRow

                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("ALCM_path"), oDataset.Tables(0).Columns("ALCM_RealPath"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("ALCM_PadreVirtuale_ID") = 0 Then 'Or ComunitaPath = dbRow("ALCM_RealPath") Then
                        Dim node As RadTreeNode = CreateNode(dbRow, True, nodeRoot)
                        If Not IsNothing(node) Then
                            nodeRoot.Nodes.Add(node)
                            RecursivelyPopulate(dbRow, node, nodeRoot)
                        End If
                    End If
                Next dbRow
            End If
        Catch ex As Exception
            Me.GeneraNoNode()
        End Try
    End Sub

    Private Sub RecursivelyPopulate(ByVal dbRow As DataRow, ByVal node As RadTreeNode, ByVal nodeFather As RadTreeNode)
        Dim childRow As DataRow

        For Each childRow In dbRow.GetChildRows("NodeRelation")
            Dim childNode As RadTreeNode = CreateNode(childRow, False, node)

            If Not (IsNothing(childNode)) Then
                node.Nodes.Add(childNode)
                RecursivelyPopulate(childRow, childNode, node)
                Try
                    If childNode.Nodes.Count = 0 And childNode.Category = -Me.HDN_ComunitaAttualeID.Value Then
                        childNode.Remove()
                    ElseIf childNode.Nodes.Count = 0 And childRow.Item("ALCM_Livello") = Me.HDN_Livello.Value - 1 Then
                        childNode.Remove()
                    End If
                Catch ex As Exception

                End Try
                If childNode.Checked = True And childNode.Enabled = False Then
                    childNode.Parent.Expanded = True
                End If
            End If
        Next childRow
    End Sub
    Private Function CreateNode(ByVal dbRow As DataRow, ByVal expanded As Boolean, ByVal nodeFather As RadTreeNode) As RadTreeNode
        Dim node As New RadTreeNode
        Dim start As Integer
        Dim [continue] As Boolean = False
        start = 0

        Dim CMNT_id, RLPC_TPRL_id, TipoComunitaID As Integer
        Dim CMNT_Responsabile, img As String
        CMNT_id = dbRow.Item("CMNT_id")
        TipoComunitaID = dbRow.Item("CMNT_TPCM_id")

        Dim ImageBaseDir As String
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")

        Dim CMNT_Nome, CMNT_NomeVisibile, CMNT_REALpath, CMNT_path As String
        Dim CMNT_IsChiusa As Boolean
        Dim Livello As Integer

        CMNT_Nome = dbRow.Item("CMNT_Nome")
        CMNT_NomeVisibile = CMNT_Nome
        CMNT_IsChiusa = dbRow.Item("CMNT_IsChiusa")
        Livello = dbRow.Item("ALCM_Livello")

        If CMNT_id > 0 Then
            If IsDBNull(dbRow.Item("RLPC_TPRL_id")) Then
                RLPC_TPRL_id = -1
            Else
                RLPC_TPRL_id = dbRow.Item("RLPC_TPRL_id")
            End If

            'TROVO IL RESPONSABILE

            Try
                If dbRow.Item("CMNT_Responsabile") = "" Then
                    If Not IsDBNull(dbRow.Item("AnagraficaCreatore")) Then
                        CMNT_Responsabile = oResourceUCcomunita.getValue("creata")
                        CMNT_Responsabile = CMNT_Responsabile.Replace("#%%#", dbRow.Item("AnagraficaCreatore"))
                    End If
                Else
                    CMNT_Responsabile = " (" & dbRow.Item("CMNT_Responsabile") & ") "
                End If
            Catch ex As Exception
                If Not IsDBNull(dbRow.Item("AnagraficaCreatore")) Then
                    CMNT_Responsabile = oResourceUCcomunita.getValue("creata")
                    CMNT_Responsabile = CMNT_Responsabile.Replace("#%%#", dbRow.Item("AnagraficaCreatore"))
                End If
            End Try
            'If IsDBNull(dbRow.Item("CMNT_Responsabile")) Then
            '    CMNT_Responsabile = ""

            'Else

            'End If
            If IsDBNull(dbRow.Item("TPCM_icona")) Then
                img = ""
            Else
                img = dbRow.Item("TPCM_icona")
                img = "./logo/" & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
            End If
            dbRow.Item("TPCM_icona") = img
        Else
            img = ""
        End If

        If CMNT_id > 0 Then
            CMNT_Nome = CMNT_Nome & CMNT_Responsabile
            CMNT_NomeVisibile = CMNT_Nome
            CMNT_Nome = CMNT_Nome & Me.GenerateImage(ImageBaseDir & oResourceUCcomunita.getValue("stato.image." & CMNT_IsChiusa), oResourceUCcomunita.getValue("stato." & CMNT_IsChiusa))

            If dbRow.IsNull("CMNT_AnnoAccademico") = False Then
                If dbRow.Item("CMNT_AnnoAccademico") <> "" Then
                    CMNT_Nome = CMNT_Nome & "&nbsp;(" & dbRow.Item("CMNT_AnnoAccademico") & ")&nbsp;"
                End If
            End If
        Else
            CMNT_NomeVisibile = CMNT_Nome
        End If
        CMNT_path = dbRow.Item("ALCM_path")


        Dim oServizio As New Services_AmministraComunita
        oServizio.PermessiAssociati = dbRow.Item("LKSC_Permessi")
        node.Checkable = (oServizio.Admin Or oServizio.CreateComunity Or oServizio.Moderate) And (Livello = Me.HDN_Livello.Value)

        Try
            If (Me.HDN_ComunitaAttualeID.Value = CMNT_id) Then
                node.CssClass = "TreeNodeDisabled"
                node.Category = CMNT_id
                node.Checkable = True
                node.Checked = True
                node.Enabled = False
                HDN_hasComunitaForServizio.Value = True
            ElseIf Not node.Checkable Then
                node.CssClass = "TreeNodeDisabled"
                node.Category = -CMNT_id
                node.Checkable = False
                node.Checked = False
                node.Enabled = True
                If Livello = Me.HDN_Livello.Value Then
                    Return Nothing
                End If
            Else
                node.Category = CMNT_id
                HDN_hasComunitaForServizio.Value = True
            End If
        Catch ex As Exception

        End Try
        node.Text = CMNT_Nome
        node.Value = CMNT_id 'CMNT_path
        node.Expanded = expanded
        node.ImageUrl = img
        node.ToolTip = CMNT_NomeVisibile

        Return node
    End Function 'CreateNode
    Private Function GeneraNoNode()
        Dim oRootNode As New RadTreeNode
        Dim oNode As New RadTreeNode

        If IsNothing(oResourceUCcomunita) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        oRootNode = New RadTreeNode

        oRootNode.Value = ""
        oRootNode.Expanded = True
        oRootnode.ImageUrl = "folder.gif"
        oRootNode.Category = 0
        oRootNode.Checkable = False
        Try
            oRootNode.Text = oResourceUCcomunita.getValue("oRootNode.Text")
            If oRootNode.Text = "" Then
                oRootNode.Text = "Comunità: "
            End If
        Catch ex As Exception
            oRootNode.Text = "Comunità: "
        End Try
        Try
            oRootNode.ToolTip = oResourceUCcomunita.getValue("oRootNode.ToolTip")
            If oRootNode.ToolTip = "" Then
                oRootNode.ToolTip = oRootNode.Text
            End If
        Catch ex As Exception
            oRootNode.ToolTip = oRootNode.Text
        End Try

        oNode = New RadTreeNode
        oNode.Expanded = True
        oNode.Value = ""
        Try
            oNode.ToolTip = oResourceUCcomunita.getValue("NoNode.ToolTip")
            If oNode.ToolTip = "" Then
                oNode.ToolTip = "Nessuna comunità trovata."
            End If
        Catch ex As Exception
            oNode.ToolTip = oNode.ToolTip = "Nessuna comunità trovata."
        End Try
        Try
            oNode.Text = oResourceUCcomunita.getValue("NoNode.Text")
            If oNode.Text = "" Then
                oNode.Text = oNode.ToolTip = "Nessuna comunità trovata."
            End If
        Catch ex As Exception
            oNode.Text = oNode.ToolTip = "Nessuna comunità trovata."
        End Try
        oNode.Category = 0
        oNode.Checkable = False

        oRootNode.Nodes.Add(oNode)

        Me.RDTcomunita.Nodes.Clear()
        Me.RDTcomunita.Nodes.Add(oRootNode)
    End Function

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
#End Region

#End Region

#Region "Apri-chiudi filtri"
    Private Sub LNBapriFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapriFiltro.Click
        Me.MostraFiltri()
    End Sub

    Private Sub LNBchiudiFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudiFiltro.Click
        Me.MostraFiltri()
    End Sub

    Private Sub MostraFiltri()
        If Me.TBRchiudiFiltro.Visible Then
            Me.TBRapriFiltro.Visible = True
            Me.TBRchiudiFiltro.Visible = False
            Me.TBRfiltri.Visible = False
        Else
            Me.TBRapriFiltro.Visible = False
            Me.TBRchiudiFiltro.Visible = True
            Me.TBRfiltri.Visible = True
        End If
    End Sub
#End Region

#Region "internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        oResourceUCcomunita = New ResourceManager

        oResourceUCcomunita.UserLanguages = code
        oResourceUCcomunita.ResourcesName = "pg_UC_Fase3ComunitaPadri"
        oResourceUCcomunita.Folder_Level1 = "Comunita"
        oResourceUCcomunita.Folder_Level2 = "UC_WizardComunita"
        oResourceUCcomunita.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        If IsNothing(oResourceUCcomunita) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        With oResourceUCcomunita
            .setDropDownList(Me.DDLTipoRicerca, -2)
            .setDropDownList(Me.DDLTipoRicerca, -7)
            .setDropDownList(Me.DDLTipoRicerca, -5)
            .setDropDownList(Me.DDLTipoRicerca, -6)
            .setDropDownList(Me.DDLTipoRicerca, -3)
            .setDropDownList(Me.DDLTipoRicerca, -4)

            '.setLabel(Me.LBlegendaFiltro)
            .setLabel(LBorganizzazione_c)
            .setLabel(Me.LBtipoComunita_c)
            .setLabel(Me.LBtipoRicerca_c)
            .setLabel(Me.LBvalore_c)
            .setButton(Me.BTNCerca)

            .setLinkButton(Me.LNBapriFiltro, True, True, False, False)
            .setLinkButton(Me.LNBchiudiFiltro, True, True, False, False)
        End With
    End Sub
#End Region

    Public Function RegistraPadri(ByVal ComunitaID As Integer, ByVal CreatoreID As Integer, ByVal ResponsabileID As Integer) As WizardComunita_Message
        Try
            If Me.RDTcomunita.CheckedNodes.Count = 0 Then
                Return WizardComunita_Message.NesunaOperazione
            Else
                Dim oNode As RadTreeNode
                Dim ElencoPadri As String = ","
                Dim errore As Boolean = False
                Dim oComunita As New COL_Comunita
                oComunita.Id = ComunitaID

                For Each oNode In Me.RDTcomunita.CheckedNodes
                    If oNode.Enabled Then
                        If InStr(ElencoPadri, "," & oNode.Value & ",") < 1 Then
                            ElencoPadri &= oNode.Value & ","
                            oComunita.AssociaPadre(oNode.Value)
                            If oComunita.Errore = Errori_Db.None Then
                                Dim Percorso As String
                                Dim ComunitaPadreID As Integer
                                ComunitaPadreID = oNode.Value

                                Percorso = "." & ComunitaPadreID & "."
                                Try
                                    oNode = oNode.Parent
                                    While Not (oNode.Value = "0" Or oNode.Value = "")
                                        Percorso = "." & oNode.Value & Percorso
                                        oNode = oNode.Parent
                                    End While
                                Catch ex As Exception

                                End Try
                                Percorso = Replace(Percorso, "-", "")
                                If CreatoreID = ResponsabileID Then
                                    Me.RegistraXML(CreatoreID, ComunitaID, ComunitaPadreID, Percorso)
                                Else
                                    Me.RegistraXML(CreatoreID, ComunitaID, ComunitaPadreID, Percorso)
                                    Me.RegistraXML(ResponsabileID, ComunitaID, ComunitaPadreID, Percorso)
                                End If
                            Else
                                errore = True
                            End If
                        End If
                    End If
                Next
                If ElencoPadri <> "," Then


                    If Not errore Then
                        Return WizardComunita_Message.PadriAssociati
                    Else
                        Return WizardComunita_Message.PadriNonAssociati
                    End If
                Else
                    Return WizardComunita_Message.NesunaOperazione
                End If
            End If
        Catch ex As Exception
            Return WizardComunita_Message.PadriNonAssociati
        End Try
        Return WizardComunita_Message.NesunaOperazione
    End Function

    Private Sub RegistraXML(ByVal PersonaID As Integer, ByVal ComunitaID As Integer, ByVal PadreID As Integer, ByVal Percorso As String)
        Try
            Dim pageUtility As New OLDpageUtility(Context)
            Dim oTreeComunita As New COL_TreeComunita
            oTreeComunita.Directory = PageUtility.ProfilePath & PersonaID & "\"
            oTreeComunita.Nome = PersonaID & ".xml"
            oTreeComunita.ClonaIscrizione(ComunitaID, PadreID, Percorso)
        Catch ex As Exception

        End Try
    End Sub
End Class