Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita

Imports Telerik
Imports Telerik.WebControls

Public Class UC_FiltroComunitaByServizio_NEW
    Inherits System.Web.UI.UserControl
    Private oResourceUCcomunita As ResourceManager

    Public Event AggiornaDati(ByVal sender As Object, ByVal e As EventArgs)

    Protected WithEvents HDN_ComunitaAttualePercorso As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ComunitaAttualeID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_Livello As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ServizioCode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_hasComunitaForServizio As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_valoreDestinatario As System.Web.UI.HtmlControls.HtmlInputHidden
    Private _PageUtility As PresentationLayer.OLDpageUtility
    Private ReadOnly Property PageUtility(Optional ByVal oContext As HttpContext = Nothing) As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) OrElse Not IsNothing(oContext) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property
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
    Public ReadOnly Property ComunitaID() As Integer
        Get
            Try
                Dim oSelectedNode As RadTreeNode
                oSelectedNode = Me.RDTcomunita.FindNodeByValue(Me.HDN_valoreDestinatario.Value)
                If IsNothing(oSelectedNode) Then
                    Return -1
                Else
                    If oSelectedNode.Category > 0 Then
                        Return oSelectedNode.Category
                    Else
                        Return -1
                    End If
                End If
            Catch ex As Exception
                Return -1
            End Try
        End Get
    End Property
    Public ReadOnly Property ComunitaPath() As String
        Get
            Try
                Return Me.HDN_valoreDestinatario.Value
            Catch ex As Exception
                Return ""
            End Try
        End Get
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
    Public Property ServizioCode() As String
        Get
            ServizioCode = Me.HDN_ServizioCode.Value
        End Get
        Set(ByVal Value As String)
            Me.HDN_ServizioCode.Value = Value
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

#Region "Filtro"
    'TBCorganizzazione1
    'TBCtipoRicerca
    'TBCvalore

    'IMBfiltro
    'LNBfiltro
    'LBlegendaFiltro
    'TBRsceltaComunita
    'Protected WithEvents LBlegendaFiltro As System.Web.UI.WebControls.Label
    'Protected WithEvents IMBfiltro As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents LNBfiltro As System.Web.UI.WebControls.LinkButton

    'Protected WithEvents TBLdati As System.Web.UI.WebControls.Table
    'Protected WithEvents TBRfiltro As System.Web.UI.WebControls.TableRow

    Protected WithEvents TBLFiltroComAll As System.Web.UI.WebControls.Table
    Protected WithEvents TBLFiltroCom As System.Web.UI.WebControls.Table
    Protected WithEvents TBLDdlCom As System.Web.UI.WebControls.Table

    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList


    Protected WithEvents LBtipoComunita_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBCtipoRicerca_c As System.Web.UI.WebControls.TableCell
    Protected WithEvents LBtipoRicerca_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBstatoComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBvalore_c As System.Web.UI.WebControls.Label
    'Protected WithEvents LBvuota_c As System.Web.UI.WebControls.Label

    Protected WithEvents DDLTipo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList

    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button

    Protected WithEvents LBorganizzazione_c As System.Web.UI.WebControls.Label
    Protected WithEvents TBRorgnCorsi As System.Web.UI.WebControls.Tablerow
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
    Private Enum Iscrizioni_code
        IscrizioniAperteIl = 0
        IscrizioniChiuse = 1
        IscrizioniComplete = 2
        IscrizioniEntro = 3
    End Enum

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

#Region "Bind_Filtri"
    Public Sub SetupControl(Optional ByVal SelezionataID As Integer = -1, Optional ByVal Livello As Integer = -1, Optional ByVal ComunitaAttualeID As Integer = -1, Optional ByVal ComunitaAttualePercorso As String = "")
        Me.HDN_Livello.Value = Livello
        Me.HDN_ComunitaAttualePercorso.Value = ComunitaAttualePercorso
        Me.HDN_ComunitaAttualeID.Value = ComunitaAttualeID
        Me.SetupInternazionalizzazione()
        Me.SetupFiltri(SelezionataID)


        Try
            Dim PercorsoPadre As String = ""
            Dim oSelectedNode As RadTreeNode
			If SelezionataID = -1 Then
				PercorsoPadre = Me.HDN_ComunitaAttualePercorso.Value
				PercorsoPadre = Replace(PercorsoPadre, "." & Me.HDN_ComunitaAttualeID.Value & ".", ".")
			Else
				PercorsoPadre = Replace(Me.HDN_ComunitaAttualePercorso.Value, "." & SelezionataID & ".", "." & SelezionataID & ".#")
				PercorsoPadre = Left(PercorsoPadre, InStr(PercorsoPadre, "#") - 1)
			End If
            
            oSelectedNode = Me.RDTcomunita.FindNodeByValue(PercorsoPadre)
            If Not IsNothing(oSelectedNode) Then
                Me.RipristinaNodoSelezionato(oSelectedNode)
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub SetupFiltri(Optional ByVal SelezionataID As Integer = -1)
        Me.Bind_TipiComunita()
        Me.Bind_Organizzazioni()
        Me.Bind_StatusComunità()
        Me.Bind_TreeComunita(SelezionataID)

        TBRorgnCorsi.Visible = False
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
            oDataSet = oTipoComunita.ElencaForFiltri(Session("LinguaID"), True, 0)
            If oDataSet.Tables(0).Rows.Count > 0 Then
                DDLTipo.DataSource = oDataSet
                DDLTipo.DataTextField() = "TPCM_descrizione"
                DDLTipo.DataValueField() = "TPCM_id"
                DDLTipo.DataBind()

                'aggiungo manualmente elemento che indica tutti i tipi di comunità
                DDLTipo.Items.Insert(0, New ListItem(oResourceUCcomunita.getValue("DDLTipo.-1"), -1))
            End If
        Catch ex As Exception
            DDLTipo.Items.Insert(0, New ListItem(oResourceUCcomunita.getValue("DDLTipo.-1"), -1))
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
                        Me.DDLstatoComunita.Items.Insert(1, New ListItem(oResourceUCcomunita.getValue("DDLstatoComunita.2"), 2))
                    Else
                        Me.DDLstatoComunita.Items.Insert(2, New ListItem(oResourceUCcomunita.getValue("DDLstatoComunita.2"), 2))
                    End If
                End If
            End If

            If TotaleArchiviate = 0 Then
                If Not IsNothing(oListItem_Archiviate) Then
                    Me.DDLstatoComunita.Items.Remove(oListItem_Archiviate)
                End If
            Else
                If IsNothing(oListItem_Archiviate) Then
                    Me.DDLstatoComunita.Items.Insert(1, New ListItem(oResourceUCcomunita.getValue("DDLstatoComunita.1"), 1))
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

                    If ORGN_ID >= 0 Then
                        Try
                            Me.DDLorganizzazione.SelectedValue = ORGN_ID
                        Catch ex As Exception
                            Me.DDLorganizzazione.Items.Clear()
                            Me.DDLorganizzazione.Items.Add(New ListItem(oComunita.Nome, ORGN_ID))
                            Me.DDLorganizzazione.SelectedIndex = 0
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
        Dim oSelectedNode As RadTreeNode
        oSelectedNode = Me.RDTcomunita.SelectedNode

   
        Me.Bind_TreeComunita()
        Me.RipristinaNodoSelezionato(oSelectedNode)

        RaiseEvent AggiornaDati(Me, EventArgs.Empty)
    End Sub

    Private Sub DDLTipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipo.SelectedIndexChanged
        Dim oSelectedNode As RadTreeNode
        oSelectedNode = Me.RDTcomunita.SelectedNode

        Me.TBRorgnCorsi.Visible = False


        Me.Bind_TreeComunita()
        Me.RipristinaNodoSelezionato(oSelectedNode)
        RaiseEvent AggiornaDati(Me, EventArgs.Empty)
    End Sub
    
    Private Sub BTNCerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Dim oSelectedNode As RadTreeNode
        oSelectedNode = Me.RDTcomunita.SelectedNode

        Me.Bind_TreeComunita()
        Me.RipristinaNodoSelezionato(oSelectedNode)
        RaiseEvent AggiornaDati(Me, EventArgs.Empty)
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
            oTreeComunita.Directory = PageUtility.BaseUrlDrivePath & "profili\" & oPersona.ID & "\"
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
            TipocomunitaID = -1
            TipoCdlID = -1

            Try
                StatusID = Me.DDLstatoComunita.SelectedValue
            Catch ex As Exception

            End Try

            Dim ComunitaPath As String = ""
            If IsArray(Session("ArrComunita")) Then
                Try
                    ArrComunita = Session("ArrComunita")
                    ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
                Catch ex As Exception

                End Try
            End If

            Dim ImageBaseDir, img As String
            ImageBaseDir = GetPercorsoApplicazione(Me.Request)
            ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"
            ImageBaseDir = Replace(ImageBaseDir, "//", "/")

            oDataset = oTreeComunita.RicercaComunitaAlberoByServizio(Me.ServizioCode, oPersona, ComunitaPadreID, Me.oResourceUCcomunita, ImageBaseDir, TipocomunitaID, FacoltaID, , , , ComunitaPadreID, ComunitaPath, oFiltroTipoRicerca, valore, StatusID, , , True, Me.HDN_Livello.Value)

        Catch ex As Exception

        End Try
        Return oDataset
    End Function

#Region "AlberoComunità"
    Private Sub RipristinaNodoSelezionato(ByVal oSelectedNode As RadTreeNode)
        Try

            If Not IsNothing(oSelectedNode) Then
                Dim oNewNode As RadTreeNode
                oNewNode = Me.RDTcomunita.FindNodeByValue(oSelectedNode.Value)
                If IsNothing(oNewNode) Then
                    Me.HDN_valoreDestinatario.Value = ""
                    Me.TXBdestinatario.Text = ""
                Else
                    If IsNothing(Me.RDTcomunita.SelectedNode) Then
                        oNewNode.Selected = True
                        Me.HDN_valoreDestinatario.Value = oNewNode.Value
                        Me.TXBdestinatario.Text = Me.SetupNomeComunita(oNewNode.Text)
                    Else
                        Me.RDTcomunita.SelectedNode.Selected = False
                        oNewNode.Selected = True
                        Me.HDN_valoreDestinatario.Value = oNewNode.Value
                        Me.TXBdestinatario.Text = Me.SetupNomeComunita(oNewNode.Text)
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TreeComunita(Optional ByVal SelezionataID As Integer = -1)
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
            Me.RDTcomunita.Nodes.Add(nodeRoot)

            If oDataset.Tables(0).Rows.Count = 0 Then
                Me.GeneraNoNode()
            Else
                Dim oRow As DataRow

                oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("ALCM_path"), oDataset.Tables(0).Columns("ALCM_RealPath"), False)

                Dim dbRow As DataRow
                For Each dbRow In oDataset.Tables(0).Rows
                    If dbRow("ALCM_PadreVirtuale_ID") = 0 Or ComunitaPath = dbRow("ALCM_RealPath") Then
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
                    End If
                Catch ex As Exception

                End Try
            End If
        Next childRow
    End Sub
    Private Function CreateNode(ByVal dbRow As DataRow, ByVal expanded As Boolean, ByVal nodeFather As RadTreeNode) As RadTreeNode
        Dim node As New RadTreeNode
        Dim start As Integer
        Dim [continue] As Boolean = False
        start = 0

        Dim CMNT_id, RLPC_TPRL_id As Integer
        Dim CMNT_Responsabile, img As String
        CMNT_id = dbRow.Item("CMNT_id")

        Dim ImageBaseDir As String
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")

        Dim CMNT_Nome, CMNT_NomeVisibile, CMNT_REALpath, CMNT_path As String
        Dim CMNT_IsChiusa As Boolean

        CMNT_Nome = dbRow.Item("CMNT_Nome")
        CMNT_NomeVisibile = CMNT_Nome
        CMNT_IsChiusa = dbRow.Item("CMNT_IsChiusa")
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


        If Me.ServizioCode = Services_File.Codex Then
            Dim oServizio As New Services_File
            oServizio.PermessiAssociati = dbRow.Item("LKSC_Permessi")
            node.Checkable = (oServizio.Admin Or oServizio.Moderate)
        ElseIf Me.ServizioCode = Services_Mail.Codex Then
            Dim oServizio As New Services_Mail
            oServizio.PermessiAssociati = dbRow.Item("LKSC_Permessi")
            node.Checkable = (oServizio.Admin Or oServizio.SendMail)
        ElseIf Me.ServizioCode = Services_PostIt.Codex Then
            Dim oServizio As New Services_PostIt
            oServizio.PermessiAssociati = dbRow.Item("LKSC_Permessi")
            node.Checkable = (oServizio.GestioneServizio Or oServizio.ViewPostIt Or oServizio.CreatePostIt)
        ElseIf Me.ServizioCode = Services_RaccoltaLink.Codex Then
            Dim oServizio As New Services_RaccoltaLink
            oServizio.PermessiAssociati = dbRow.Item("LKSC_Permessi")
            node.Checkable = (oServizio.Admin Or oServizio.ExportLink Or oServizio.Moderate)
        End If


        Try
            If (Me.HDN_ComunitaAttualeID.Value = CMNT_id) Then
                node.CssClass = "TreeNodeDisabled"
                node.Category = -CMNT_id
            ElseIf Not node.Checkable Or (Me.HDN_ComunitaAttualeID.Value = CMNT_id And Me.ServizioCode = Services_File.Codex) Then
                node.CssClass = "TreeNodeDisabled"
                node.Category = -CMNT_id
            Else
                node.Category = CMNT_id
                HDN_hasComunitaForServizio.Value = True
            End If
        Catch ex As Exception

        End Try
        node.Text = CMNT_Nome
        node.Value = CMNT_path
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
    'Private Sub IMBfiltro_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBfiltro.Click
    '    'Me.MostraFiltri()
    'End Sub
    'Private Sub LNBfiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBfiltro.Click
    '    'Me.MostraFiltri()
    'End Sub
    Private Sub MostraFiltri()
        'If Me.TBRfiltro.Visible Then
        '    'Me.IMBfiltro.ImageUrl = "./../images/filtro_open.gif"
        '    'oResourceUCcomunita.setLinkButtonToValue(Me.LNBfiltro, Me.StringaVisualizza.mostra, True, True)
        'Else
        '    'oResourceUCcomunita.setLinkButtonToValue(Me.LNBfiltro, Me.StringaVisualizza.nascondi, True, True)
        'End If
        'If Me.TBRfiltri.Visible Then
        If Me.TBRchiudiFiltro.Visible Then
            Me.TBRapriFiltro.Visible = True
            Me.TBRchiudiFiltro.Visible = False
            Me.TBRfiltri.Visible = False
            'Me.TBRfiltri.Visible = False
        Else
            Me.TBRapriFiltro.Visible = False
            Me.TBRchiudiFiltro.Visible = True
            Me.TBRfiltri.Visible = True
            'Me.TBRfiltri.Visible = True
        End If
        'Me.TBRfiltro.Visible = Not (Me.TBRfiltro.Visible)
    End Sub
#End Region

#Region "internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        oResourceUCcomunita = New ResourceManager

        oResourceUCcomunita.UserLanguages = code
        oResourceUCcomunita.ResourcesName = "pg_UC_FiltroComunita_NEW"
        oResourceUCcomunita.Folder_Level1 = "UC"
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

            .setDropDownList(Me.DDLstatoComunita, 0)
            .setDropDownList(Me.DDLstatoComunita, 1)
            .setDropDownList(Me.DDLstatoComunita, 2)

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

    Private Sub RDTcomunita_NodeClick(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles RDTcomunita.NodeClick
           Me.TXBdestinatario.Text = Me.SetupNomeComunita(e.NodeClicked.Text)
        RaiseEvent AggiornaDati(Me, EventArgs.Empty)
    End Sub

    Private Sub DDLstatoComunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLstatoComunita.SelectedIndexChanged
        Me.Bind_TreeComunita()
    End Sub
End Class