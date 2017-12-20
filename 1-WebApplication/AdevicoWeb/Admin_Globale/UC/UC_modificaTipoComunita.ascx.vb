Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer

Imports COL_BusinessLogic_v2.CL_permessi
Imports lm.Comol.Core.File
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports System
Imports System.Linq

Public Class UC_modificaTipoComunita
    Inherits BaseControl



    'Public ReadOnly Property PageUtility() As OLDpageUtility
    '    Get
    '        If IsNothing(_PageUtility) Then
    '            _PageUtility = New OLDpageUtility(Me.Context)
    '        End If
    '        Return _PageUtility
    '    End Get
    'End Property
    Private ReadOnly Property PreloadedIdCommunityType() As Integer
        Get
            Dim idRole As Integer = -1

            If IsNumeric(Me.Request.QueryString("IdCommunityType")) Then
                idRole = Me.Request.QueryString("IdCommunityType")
            End If
            Return idRole
        End Get
    End Property
    Private Property IdCommunityType() As Integer
        Get
            Return ViewStateOrDefault("IdCommunityType", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdCommunityType") = value
        End Set
    End Property
    Private Property StartupPermission() As Boolean
        Get
            Return ViewStateOrDefault("StartupPermission", False)
        End Get
        Set(value As Boolean)
            ViewState("StartupPermission") = value
        End Set
    End Property
    Private _CurrentManager As ManagerPermission
    Public ReadOnly Property CurrentManager() As ManagerPermission
        Get
            If IsNothing(_CurrentManager) Then
                _CurrentManager = New ManagerPermission(Me.PageUtility.CurrentContext, Me.SystemSettings.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL).Name)
            End If
            Return _CurrentManager
        End Get
    End Property
    Private ReadOnly Property CurrentIdModule() As Integer
        Get
            If Me.DDLmodules.Items.Count = 0 Then
                Return -1
            Else
                Return CInt(Me.DDLmodules.SelectedValue)
            End If
        End Get
    End Property
    Private ReadOnly Property CurrentIdOrganization() As Integer
        Get
            If Me.DDLorganization.Items.Count = 0 Then
                Return -999
            Else
                Return CInt(Me.DDLorganization.SelectedValue)
            End If
        End Get
    End Property

#Region "Servizi"
    Protected WithEvents TBLpermessi As System.Web.UI.WebControls.Table
    Protected WithEvents DDLmodules As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLorganization As System.Web.UI.WebControls.DropDownList
    Protected WithEvents RPTtemplatePermission As System.Web.UI.WebControls.Repeater
    'Protected WithEvents LBorganizzazionePermessi_t As System.Web.UI.WebControls.Label
    'Protected WithEvents DDLorganizzazionePermessi As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents LBserviziPermessi_t As System.Web.UI.WebControls.Label
    'Protected WithEvents DDLserviziPermessi As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents TBLpermessiRuoli As System.Web.UI.WebControls.Table
#End Region


    Public Enum Inserimento
        ErroreGenerico = 0
        Creato = 1
        Modificato = 2
        ServiziAssociati = 3
        PermessiAssociati = 4
        ServiziDefault = 5
        DescrizioneMancante = -1
        NONModificato = -2
        NONinserito = -3
        SelezionaCategoria = -4
        SelezionaModello = -5
        SelezionaServizio = -6
        ErroreServizi = -7
        SelezionaRuolo = -8
        ModificaRuoli = 6
        NessunServizioPresente = -9
        PermessiAssociatiParziali = -10
        ErroreAssociazioneIcona = -11
        ErroreAssociazioneRuoliObbligatori = -12
    End Enum
    Public Enum IndiceSelezionato
        DatiPrincipali = 0
        CategoriaFile = 1
        TipoRuolo = 2
        RuoliProfili = 3
        Modello = 4
        Servizi = 5
        Permessi = 6

    End Enum

    Public ReadOnly Property TipoComunitaID() As Integer
        Get
            Try
                TipoComunitaID = Me.HDNtpcm_id.Value
            Catch ex As Exception
                Me.HDNtpcm_id.Value = -1
                TipoComunitaID = -1
            End Try
        End Get
    End Property

    Public Event AggiornaMenuServizi(ByVal AttivaDefault As Boolean, ByVal Abilitato As Boolean, ByVal displaySetToAllCommunities As Boolean)
    Protected WithEvents HDNtpcm_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HIDcheckbox As System.Web.UI.HtmlControls.HtmlInputHidden

#Region "Dati tipologia"
    Protected WithEvents TBLdati As System.Web.UI.WebControls.Table
    Protected WithEvents LBtipoComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoSottoComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBtipoComunita As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBicona_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents IMicona As System.Web.UI.WebControls.Image

    Protected WithEvents CHLtipoSottoComunita As System.Web.UI.WebControls.CheckBoxList

    Protected WithEvents RPTnome As System.Web.UI.WebControls.Repeater
    Protected WithEvents LBlinguaNome_t As System.Web.UI.WebControls.Label
#End Region

#Region "Dati categoriaFile"
    Protected WithEvents TBLcategoriaFile As System.Web.UI.WebControls.Table
    Protected WithEvents LBcategoriaFile_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBLcategoriaFile As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents HDNcategoriaAssociate As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "Dati tipoRuolo"
    Protected WithEvents TBLtipoRuolo As System.Web.UI.WebControls.Table
    Protected WithEvents LBtipiRuolo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBruoloDefault_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLruoloDefault As System.Web.UI.WebControls.DropDownList
    Protected WithEvents CBLtipoRuolo As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents HDNruoliAssociati As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNruoloNONdeassociabili As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "Dati Ruoli Profili"
    Protected WithEvents TBLruoliAllways As System.Web.UI.WebControls.Table
    Protected WithEvents LBtipiRuoloAll_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBLtipoRuoloAll As System.Web.UI.WebControls.CheckBoxList
#End Region

#Region "Dati modelli"
    Protected WithEvents TBLmodelli As System.Web.UI.WebControls.Table
    Protected WithEvents CBLmodelli As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents LBmodelli_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmodelloDefault_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLmodelloDefault As System.Web.UI.WebControls.DropDownList
#End Region

#Region "Servizi"
    Protected WithEvents TBLservizio As System.Web.UI.WebControls.Table
    Protected WithEvents LBserviziAttivi_t As System.Web.UI.WebControls.Label
    Protected WithEvents TBLtipoComunita As System.Web.UI.WebControls.Table
    Protected WithEvents LBorganizzazione_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents RPTservizio As System.Web.UI.WebControls.Repeater
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
        'If IsNothing(oResource) Then
        '    Me.SetCulture(Session("LinguaCode"))
        'End If

        CBLtipoRuolo.Attributes.Add("onClick", "return RestaSelezionato();")
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_modificaTipoComunita", "Admin_Globale", "UC")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(Me.LBtipoComunita_t)
            .setLabel(Me.LBtipoSottoComunita_t)
            .setLabel(Me.LBicona_t)
            .setLabel(Me.LBcategoriaFile_t)
            .setLabel(Me.LBtipiRuolo_t)
            .setLabel(Me.LBmodelli_t)
            .setLabel(Me.LBserviziAttivi_t)
            .setLabel(Me.LBorganizzazione_t)
            .setLabel(Me.LBruoloDefault_t)
            '.setLabel(Me.LBorganizzazionePermessi_t)
            '.setLabel(Me.LBserviziPermessi_t)
            .setLabel(Me.LBlinguaNome_t)
            .setLabel(Me.LBmodelloDefault_t)
            .setLabel(Me.LBtipiRuoloAll_t)
        End With
    End Sub

#End Region

#Region "Bind_Dati"
    Public Sub AggiornaRuoli()
        Me.Bind_TipoRuolo()

    End Sub

    Public Sub Setup_Controllo(ByVal TipoComunitaID As Integer, ByVal Indice As IndiceSelezionato)
        Me.HDNtpcm_id.Value = TipoComunitaID
        Me.IdCommunityType = TipoComunitaID
        Me.TBLcategoriaFile.Visible = False
        Me.TBLmodelli.Visible = False
        Me.TBLtipoRuolo.Visible = False
        Me.TBLruoliAllways.Visible = False
        Me.TBLservizio.Visible = False
        Me.TBLdati.Visible = False
        Me.TBLpermessi.Visible = False
        Me.SetInternazionalizzazione()

        Select Case Indice
            Case IndiceSelezionato.DatiPrincipali
                Me.TBLdati.Visible = True
            Case IndiceSelezionato.CategoriaFile
                Me.TBLcategoriaFile.Visible = True
            Case IndiceSelezionato.Modello
                Me.TBLmodelli.Visible = True
            Case IndiceSelezionato.Permessi
                Me.TBLpermessi.Visible = True
            Case IndiceSelezionato.Servizi
                Me.TBLservizio.Visible = True
            Case IndiceSelezionato.TipoRuolo
                Me.TBLtipoRuolo.Visible = True
            Case IndiceSelezionato.RuoliProfili
                Me.TBLruoliAllways.Visible = True
            Case Else
                Me.TBLdati.Visible = True
        End Select

        Me.Bind_TipoRuolo()
        Me.Bind_modelli()
        Me.Bind_categoriaFile()
        Me.Bind_TipoSottoComunita()
        Me.Bind_Organizzazioni()
        Me.Bind_Lingue()
        Me.Bind_TipoRuoloObbligatori()
        Dim oTipoComunita As New COL_Tipo_Comunita
        oTipoComunita.ID = Me.HDNtpcm_id.Value
        oTipoComunita.Estrai()
        TXBtipoComunita.Text = oTipoComunita.Descrizione
        IMicona.Visible = True
        Me.IMicona.ImageUrl = "./../../" & oTipoComunita.Icona
        If Not Exists.File(Server.MapPath(Me.IMicona.ImageUrl)) Then
            IMicona.Visible = False
        End If
    End Sub
    Public Sub CambioElemento(ByVal Indice As IndiceSelezionato)
        Me.TBLcategoriaFile.Visible = False
        Me.TBLmodelli.Visible = False
        Me.TBLtipoRuolo.Visible = False
        Me.TBLruoliAllways.Visible = False
        Me.TBLservizio.Visible = False
        Me.TBLdati.Visible = False
        Me.TBLpermessi.Visible = False
        Select Case Indice
            Case IndiceSelezionato.DatiPrincipali
                Me.TBLdati.Visible = True
            Case IndiceSelezionato.CategoriaFile
                Me.TBLcategoriaFile.Visible = True
            Case IndiceSelezionato.Modello
                Me.TBLmodelli.Visible = True
            Case IndiceSelezionato.Permessi
                InitializeSettings()
            Case IndiceSelezionato.Servizi
                Me.TBLservizio.Visible = True
                Me.DDLorganizzazione.SelectedIndex = 0
                Me.Bind_Servizi()
            Case IndiceSelezionato.TipoRuolo
                Me.TBLtipoRuolo.Visible = True

            Case IndiceSelezionato.RuoliProfili
                Me.TBLruoliAllways.Visible = True
                Me.Bind_TipoRuoloObbligatori()
            Case Else
                Me.TBLdati.Visible = True
        End Select
    End Sub

    Private Sub Bind_Lingue()
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oDataset As DataSet
        Dim i As Integer

        Try
            oTipoComunita.ID = Me.HDNtpcm_id.Value
            oDataset = oTipoComunita.ElencaDefinizioniLingue()
            For i = 0 To oDataset.Tables(0).Rows.Count - 1
                If IsDBNull(oDataset.Tables(0).Rows(i).Item("Nome")) Then
                    oDataset.Tables(0).Rows(i).Item("Nome") = ""
                End If

            Next
            Me.RPTnome.DataSource = oDataset
            Me.RPTnome.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TipoRuolo()
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oDataset As New DataSet

        Try
            Dim i, totale As Integer
            oTipoComunita.ID = Me.HDNtpcm_id.Value
            oDataset = oTipoComunita.ElencaTipoRuolo(Session("LinguaID"))

            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                Me.CBLtipoRuolo.DataSource = oDataset
                Me.CBLtipoRuolo.DataTextField = "TPRL_Nome"
                Me.CBLtipoRuolo.DataValueField = "TPRL_ID"
                Me.CBLtipoRuolo.DataBind()


                Me.DDLruoloDefault.DataSource = oDataset
                Me.DDLruoloDefault.DataTextField = "TPRL_Nome"
                Me.DDLruoloDefault.DataValueField = "TPRL_ID"
                Me.DDLruoloDefault.DataBind()
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)
                    If oRow.Item("Associato") Then
                        If Me.HDNruoliAssociati.Value = "" Then
                            Me.HDNruoliAssociati.Value = "," & oRow.Item("TPRL_ID") & ","
                        Else
                            Me.HDNruoliAssociati.Value += oRow.Item("TPRL_ID") & ","
                        End If
                        Me.CBLtipoRuolo.Items(i).Selected = True
                    End If

                    If oRow.Item("NoDelete") <> 0 Then
                        If Me.HDNruoloNONdeassociabili.Value = "" Then
                            Me.HDNruoloNONdeassociabili.Value = "," & oRow.Item("TPRL_ID") & ","
                        Else
                            Me.HDNruoloNONdeassociabili.Value += oRow.Item("TPRL_ID") & ","
                        End If
                    End If

                    If oRow.Item("isDefault") Then
                        Me.DDLruoloDefault.SelectedIndex = -1
                        Me.DDLruoloDefault.Items(i).Selected = True
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TipoRuoloObbligatori()
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oDataset As New DataSet

        Try
            Dim i, totale As Integer
            oTipoComunita.ID = Me.HDNtpcm_id.Value
            If Me.HDNtpcm_id.Value >= 0 Then
                oDataset = oTipoComunita.ElencaTipoRuoloAssociati(Session("LinguaID"), Main.FiltroRuoli.ForTipoComunita)

                totale = oDataset.Tables(0).Rows.Count
                If totale > 0 Then
                    Me.CBLtipoRuoloAll.DataSource = oDataset
                    Me.CBLtipoRuoloAll.DataTextField = "TPRL_Nome"
                    Me.CBLtipoRuoloAll.DataValueField = "TPRL_ID"
                    Me.CBLtipoRuoloAll.DataBind()

                    For i = 0 To Me.CBLtipoRuoloAll.Items.Count - 1
                        Dim oListitem As New ListItem
                        Dim oRow As DataRow
                        oListitem = Me.CBLtipoRuoloAll.Items(i)

                        oRow = oDataset.Tables(0).Rows(i)
                        Try
                            oListitem.Selected = oRow.Item("LKTT_Allways")
                        Catch ex As Exception

                        End Try

                    Next
                End If
            Else
                Me.CBLtipoRuoloAll.Items.Clear()
                For i = 0 To Me.CBLtipoRuolo.Items.Count - 1
                    Dim oListitem As New ListItem


                    If Me.CBLtipoRuolo.Items(i).Selected Then
                        oListitem.Text = Me.CBLtipoRuolo.Items(i).Text
                        oListitem.Value = Me.CBLtipoRuolo.Items(i).Value
                        oListitem.Selected = (oListitem.Value = Me.DDLruoloDefault.SelectedValue)
                        Me.CBLtipoRuoloAll.Items.Add(oListitem)
                    End If
                Next
                If IsNothing(Me.CBLtipoRuoloAll.Items.FindByValue(Me.DDLruoloDefault.SelectedValue)) Then
                    Dim oListitem As New ListItem
                    oListitem.Value = Me.DDLruoloDefault.SelectedValue
                    oListitem.Text = Me.DDLruoloDefault.SelectedItem.Text
                    oListitem.Selected = True
                    Me.CBLtipoRuoloAll.Items.Insert(0, oListitem)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_modelli()
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oDataset As New DataSet

        Try
            Dim i, totale As Integer
            oTipoComunita.ID = Me.HDNtpcm_id.Value
            oDataset = oTipoComunita.ElencaModelli

            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                Me.CBLmodelli.SelectedIndex = -1
                Me.CBLmodelli.DataSource = oDataset
                Me.CBLmodelli.DataTextField = "MDCM_Nome"
                Me.CBLmodelli.DataValueField = "MDCM_ID"
                Me.CBLmodelli.DataBind()


                Me.DDLmodelloDefault.DataSource = oDataset
                Me.DDLmodelloDefault.DataTextField = "MDCM_Nome"
                Me.DDLmodelloDefault.DataValueField = "MDCM_ID"
                Me.DDLmodelloDefault.DataBind()
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)
                    If oRow.Item("LKTM_TPCM_ID") <> -1 Then ' vuol dire che è associato
                        Me.CBLmodelli.Items(i).Selected = True
                    End If
                    If oRow.Item("LKTM_Default") Then
                        Me.DDLmodelloDefault.SelectedIndex = -1
                        Me.DDLmodelloDefault.Items(i).Selected = True
                    End If
                Next
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Bind_categoriaFile()
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oDataset As New DataSet

        Try
            Dim i, totale As Integer
            oTipoComunita.ID = Me.HDNtpcm_id.Value
            oDataset = oTipoComunita.ElencaCategorieFile(Session("LinguaID"))

            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                Me.CBLcategoriaFile.DataSource = oDataset
                Me.CBLcategoriaFile.DataTextField = "CTGR_nome"
                Me.CBLcategoriaFile.DataValueField = "CTGR_ID"
                Me.CBLcategoriaFile.DataBind()
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)
                    If oRow.Item("associato") = 1 Then ' vuol dire che è associato
                        If Me.HDNcategoriaAssociate.Value = "" Then
                            Me.HDNcategoriaAssociate.Value = "," & oRow.Item("CTGR_ID") & ","
                        Else
                            Me.HDNcategoriaAssociate.Value += oRow.Item("CTGR_ID") & ","
                        End If
                        Me.CBLcategoriaFile.Items(i).Selected = True
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TipoSottoComunita()
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oDataset As New DataSet

        Try
            Dim i, totale As Integer
            oDataset = COL_Tipo_Comunita.ElencaSottoComunitaCreabili(Me.HDNtpcm_id.Value, Session("LinguaID"))
            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                Me.CHLtipoSottoComunita.DataSource = oDataset
                Me.CHLtipoSottoComunita.DataTextField = "TPCM_descrizione"
                Me.CHLtipoSottoComunita.DataValueField = "TPCM_id"
                Me.CHLtipoSottoComunita.DataBind()

                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)
                    If oRow.Item("Associato") = 1 Then ' vuol dire che è associato
                        Me.CHLtipoSottoComunita.Items(i).Selected = True
                    End If
                Next
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Bind_Servizi()
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oDataset As DataSet

        Try
            Dim i, totale As Integer
            oTipoComunita.ID = Me.HDNtpcm_id.Value
            oDataset = oTipoComunita.ServiziDefiniti(Me.DDLorganizzazione.SelectedValue, Session("LinguaID"))

            oDataset.Tables(0).Columns.Add(New DataColumn("oCheckAssociato"))
            oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDefault"))

            totale = oDataset.Tables(0).Rows.Count - 1
            For i = 0 To totale
                Dim oRow As DataRow
                oRow = oDataset.Tables(0).Rows(i)
                oRow.Item("oCheckDefault") = CBool(oRow.Item("Attivato"))
                oRow.Item("oCheckAssociato") = CBool(oRow.Item("Associato"))
                If oRow.Item("totale") > 0 Then
                    oRow.Item("SRVZ_Nome") &= " (<b>" & oRow.Item("totale") & "</b>)"
                End If

            Next

            Me.RPTservizio.DataSource = oDataset
            Me.RPTservizio.DataBind()

        Catch ex As Exception

        End Try
    End Sub


    'Private Sub Bind_PermessiRuoliServizi(ByVal SRVZ_ID As Integer, ByVal ORGN_ID As Integer, ByVal TPCM_ID As Integer, Optional ByVal start As Boolean = False)
    '    Dim oServizio As New COL_Servizio
    '    Dim oDataset As New DataSet
    '    Dim oDatasetRuoli As New DataSet

    '    oServizio.ID = SRVZ_ID
    '    Try
    '        Dim i, j, totalePermessi, totale, PRMS_Posizione As Integer
    '        Dim ARRpermServizio As Integer() = Nothing
    '        Dim oTBrow As New TableRow
    '        Dim oStringaPermessi As String
    '        oDataset = oServizio.ElencaPermessiAssociatiByLingua(Session("LinguaID"))
    '        totalePermessi = oDataset.Tables(0).Rows.Count - 1

    '        'Caricamento riga con nome permessi !

    '        Dim oTableCell As New TableCell
    '        oTableCell.Text = "&nbsp;"
    '        oTableCell.BackColor = System.Drawing.Color.Lavender
    '        oTBrow.Cells.Add(oTableCell)
    '        For i = 0 To totalePermessi
    '            Dim oRow As DataRow
    '            Dim oLinkColonna As New HtmlControls.HtmlInputButton
    '            oTableCell = New TableCell
    '            oTableCell.HorizontalAlign = HorizontalAlign.Center

    '            oRow = oDataset.Tables(0).Rows(i)
    '            If IsDBNull(oRow.Item("Nome")) Then
    '                Try
    '                    oLinkColonna.Value = oRow.Item("NomeDefault")
    '                Catch ex As Exception
    '                    oLinkColonna.Value = "--"
    '                End Try
    '            Else
    '                oLinkColonna.Value = oRow.Item("Nome")
    '            End If

    '            oTableCell = New TableCell
    '            oLinkColonna.Attributes.Add("class", "Header_Repeater10")
    '            oLinkColonna.Attributes.Add("onclick", "SelezioneColonna('CB_" & oRow.Item("PRMS_Posizione") & "');return false;")
    '            oTableCell.Controls.Add(oLinkColonna)
    '            oTableCell.CssClass = "Header_Repeater10"
    '            If i Mod 2 = 0 Then
    '                oTableCell.BackColor = System.Drawing.Color.White
    '            Else
    '                oTableCell.BackColor = System.Drawing.Color.LightYellow
    '            End If
    '            oTBrow.Cells.Add(oTableCell)
    '            ReDim Preserve ARRpermServizio(i)
    '            ARRpermServizio(i) = oRow.Item("PRMS_Posizione")
    '        Next
    '        oTableCell = New TableCell
    '        oTBrow.Cells.Add(oTableCell)
    '        Me.TBLpermessiRuoli.Rows.Add(oTBrow)


    '        oDatasetRuoli = oServizio.ElencaRuoliPermessiByTipoComunita(ORGN_ID, TPCM_ID)
    '        totale = oDatasetRuoli.Tables(0).Rows.Count - 1

    '        If start Then
    '            Me.HIDcheckbox.Value = ""
    '        End If
    '        Dim uniqueID As String
    '        For i = 0 To totale
    '            oTableCell = New TableCell
    '            Dim oRow As DataRow
    '            Dim ElencoControlli As String = ","
    '            Dim TotaleSelezionati As Integer = 0
    '            oTBrow = New TableRow

    '            oRow = oDatasetRuoli.Tables(0).Rows(i)

    '            oStringaPermessi = oRow.Item("LPRS_valore")

    '            Dim oLabel As New Label
    '            oLabel.ID = "LB" & i & "_" & (oRow.Item("TPRL_id"))
    '            oLabel.Text = oRow.Item("TPRL_nome")
    '            oTableCell.Controls.Add(oLabel)
    '            oTableCell.BackColor = System.Drawing.Color.Lavender
    '            oTBrow.Cells.Add(oTableCell)

    '            For j = 0 To UBound(ARRpermServizio)
    '                oTableCell = New TableCell
    '                oTableCell.HorizontalAlign = HorizontalAlign.Center

    '                If j Mod 2 = 0 Then
    '                    oTableCell.BackColor = System.Drawing.Color.White
    '                Else
    '                    oTableCell.BackColor = System.Drawing.Color.LightYellow
    '                End If
    '                Dim oCheckbox As New HtmlControls.HtmlInputCheckBox
    '                PRMS_Posizione = ARRpermServizio(j)
    '                uniqueID = PRMS_Posizione & "_" & oRow.Item("TPRL_id")
    '                oCheckbox.ID = "CB_" & uniqueID
    '                oCheckbox.Value = uniqueID
    '                If start Then
    '                    If oStringaPermessi.Substring(PRMS_Posizione, 1) = 1 Then
    '                        oCheckbox.Checked = True
    '                        TotaleSelezionati += 1
    '                        If Me.HIDcheckbox.Value = "" Then
    '                            Me.HIDcheckbox.Value = "," & uniqueID & ","
    '                        Else
    '                            Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & uniqueID & ","
    '                        End If
    '                    Else
    '                        oCheckbox.Checked = False
    '                    End If
    '                Else
    '                    If InStr(Me.HIDcheckbox.Value, "," & uniqueID & ",") > 0 Then
    '                        oCheckbox.Checked = True
    '                        TotaleSelezionati += 1
    '                    Else
    '                        oCheckbox.Checked = False
    '                    End If
    '                End If

    '                oCheckbox.Attributes.Add("onclick", "SelectFromNameAndAssocia('" & Me.ClientID & ":" & oCheckbox.ClientID & "','" & uniqueID & "');return true;")

    '                oTableCell.Controls.Add(oCheckbox)

    '                oTBrow.Cells.Add(oTableCell)
    '                ElencoControlli &= Me.UniqueID & Me.IdSeparator.ToString & "CB_" & uniqueID & ","
    '            Next

    '            Dim oLink As New HtmlControls.HtmlInputButton
    '            oTableCell = New TableCell
    '            oLink.Value = "Sel / Desel"
    '            oLink.Attributes.Add("onclick", "SelezioneRiga('" & ElencoControlli & "');return false;")
    '            oLink.Attributes.Add("class", "Command_Repeater10")
    '            oTableCell.Controls.Add(oLink)
    '            oTBrow.Cells.Add(oTableCell)

    '            Me.TBLpermessiRuoli.Rows.Add(oTBrow)
    '        Next
    '    Catch ex As Exception

    '    End Try
    'End Sub


    Private Sub Bind_Organizzazioni()
        Dim oOrganizzazione As New COL_Organizzazione
        Dim oDataset As New DataSet

        Try
            Me.DDLorganizzazione.Items.Clear()
            oDataset = oOrganizzazione.ElencaByIstituzione(Session("ISTT_ID"))
            If oDataset.Tables(0).Rows.Count = 0 Or Me.TipoComunitaID = -2 Then
                Me.DDLorganizzazione.Items.Add(New ListItem("< Default >", -1))
                Me.DDLorganizzazione.Enabled = False
            Else
                Me.DDLorganizzazione.DataTextField = "ORGN_ragioneSociale"
                Me.DDLorganizzazione.DataValueField = "ORGN_ID"
                Me.DDLorganizzazione.DataSource = oDataset
                Me.DDLorganizzazione.DataBind()
                Me.DDLorganizzazione.Items.Insert(0, New ListItem("< Default >", -1))
                Me.DDLorganizzazione.Enabled = True
            End If
        Catch ex As Exception
            Me.DDLorganizzazione.Items.Add(New ListItem("< Default >", -1))
            Me.DDLorganizzazione.Enabled = False

        End Try
        Me.Resource.setDropDownList(Me.DDLorganizzazione, -1)
    End Sub
#End Region

#Region "Salvataggio Dati"
    Public Function SalvaDati(ByVal Indice As IndiceSelezionato, Optional ByVal Replica As Boolean = False, Optional ByVal DefaultValue As Boolean = False, Optional ByVal updateCommunities As Boolean = False) As Inserimento
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim i, totale As Integer
        Dim iResponse As Inserimento

        Try
            If Me.HDNtpcm_id.Value <> -1 Then
                oTipoComunita.ID = Me.HDNtpcm_id.Value
                oTipoComunita.Estrai()
            Else
                Return Inserimento.ErroreGenerico
            End If
        Catch ex As Exception
            Return Inserimento.ErroreGenerico
        End Try


        Select Case Indice
            Case IndiceSelezionato.DatiPrincipali
                oTipoComunita.Descrizione = Me.TXBtipoComunita.Text
                oTipoComunita.Modifica()
                If oTipoComunita.ErroreDB = Errori_Db.None Then
                    Dim LinguaID As Integer
                    Dim Termine As String = ""

                    totale = Me.RPTnome.Items.Count
                    If totale > 0 Then
                        For i = 0 To totale - 1
                            Dim oLabel As Label
                            Dim oText As TextBox

                            Try
                                oLabel = Me.RPTnome.Items(i).FindControl("LBlinguaID")
                                LinguaID = oLabel.Text
                            Catch ex As Exception
                                LinguaID = 0
                            End Try
                            If LinguaID > 0 Then
                                Try
                                    oText = Me.RPTnome.Items(i).FindControl("TXBtermine")
                                    Termine = oText.Text
                                Catch ex As Exception
                                    Termine = ""
                                End Try

                                If Termine = "" Then
                                    Termine = Me.TXBtipoComunita.Text
                                End If

                                oTipoComunita.Translate(Termine, LinguaID)
                            End If
                        Next
                    End If
                Else
                    Return Inserimento.NONModificato
                End If
                Try
                    Dim ListaSottoComunita As String = ","
                    totale = Me.CHLtipoSottoComunita.Items.Count() - 1
                    For i = 0 To totale
                        If CHLtipoSottoComunita.Items.Item(i).Selected = True Then
                            ListaSottoComunita &= CHLtipoSottoComunita.Items.Item(i).Value & ","
                        End If
                    Next
                    If ListaSottoComunita = "," Then
                        ListaSottoComunita = ""
                    End If
                    oTipoComunita.DefinisciSottoComunita(ListaSottoComunita)

                    If oTipoComunita.ErroreDB = Errori_Db.None Then
                        iResponse = Inserimento.Modificato
                    Else
                        iResponse = Inserimento.NONModificato
                    End If
                Catch ex As Exception

                End Try
                If Me.TXBFile.Value <> "" Then
                    Dim Logo As String
                    Logo = Me.Upload_logo(Me.HDNtpcm_id.Value)
                    If Logo <> "" Then
                        Me.IMicona.ImageUrl = Logo
                    End If
                End If
                Return iResponse
            Case IndiceSelezionato.CategoriaFile
                Try
                    Dim ListaCategorie As String = ","
                    totale = Me.CBLcategoriaFile.Items.Count() - 1
                    For i = 0 To totale
                        If Me.CBLcategoriaFile.Items.Item(i).Selected = True Then
                            ListaCategorie &= Me.CBLcategoriaFile.Items.Item(i).Value & ","
                        End If
                    Next
                    If ListaCategorie = "," Then
                        ListaCategorie = ""
                        Return Inserimento.SelezionaCategoria
                    End If
                    oTipoComunita.DefinisciCategorieFile(ListaCategorie)
                    If oTipoComunita.ErroreDB = Errori_Db.None Then
                        Return Inserimento.Modificato
                    Else
                        Return Inserimento.NONModificato
                    End If
                Catch ex As Exception
                    Return Inserimento.NONModificato
                End Try
            Case IndiceSelezionato.Modello
                Try
                    Dim ListaModelli As String = ","
                    totale = Me.CBLmodelli.Items.Count() - 1
                    For i = 0 To totale
                        If CBLmodelli.Items.Item(i).Selected = True Then
                            ListaModelli &= Me.CBLmodelli.Items.Item(i).Value & ","
                        End If
                    Next
                    If ListaModelli = "," Then
                        ListaModelli = ""
                        Return Inserimento.SelezionaModello
                    End If
                    oTipoComunita.DefinisciModelli(ListaModelli)

                    If Me.DDLmodelloDefault.SelectedIndex > -1 Then
                        oTipoComunita.DefinisciModelloDefault(Me.DDLmodelloDefault.SelectedValue)
                    End If
                    If oTipoComunita.ErroreDB = Errori_Db.None Then
                        Return Inserimento.Modificato
                    Else
                        Return Inserimento.NONModificato
                    End If
                Catch ex As Exception
                    Return Inserimento.NONModificato
                End Try
            Case IndiceSelezionato.Permessi
                LoadTemplatePermissions(CurrentIdModule, CurrentIdOrganization, False)
                If updateCommunities Then
                    Me.CurrentManager.ApplyToCommunities(IdCommunityType, CurrentIdOrganization, CurrentIdModule, Me.GetTemplatePermission())

                    CacheHelper.PurgeCacheItems(lm.Comol.Modules.Standard.Menu.Domain.CacheKeys.RenderAllCommunity)
                    CacheHelper.PurgeCacheItems(Comol.Entity.CachePolicy.Permessi)
                    CacheHelper.PurgeCacheItems(Comol.Entity.CachePolicy.PermessiServizio(Me.DDLmodules.SelectedValue))
                    Dim Code As String = Me.CurrentManager.GetModuleCode(CurrentIdModule)
                    If Code <> "" Then
                        CacheHelper.PurgeCacheItems(Comol.Entity.CachePolicy.PermessiServizioUtente(Code))
                    End If
                    Return Inserimento.PermessiAssociati
                ElseIf DefaultValue Then
                    LoadTemplatePermissions(CurrentIdModule, -1, True)
                    Me.CurrentManager.SaveCommunityTemplate(IdCommunityType, CurrentIdOrganization, CurrentIdModule, Me.GetTemplatePermission)
                    Return Inserimento.PermessiAssociati
                Else
                    If Replica Then
                        Me.CurrentManager.ApplyToCommunityTemplateToAll(IdCommunityType, CurrentIdOrganization, CurrentIdModule, Me.GetTemplatePermission)
                    Else
                        Me.CurrentManager.SaveCommunityTemplate(IdCommunityType, CurrentIdOrganization, CurrentIdModule, Me.GetTemplatePermission)
                    End If
                    Return Inserimento.PermessiAssociati
                End If
                'Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.DDLorganizzazionePermessi.SelectedValue, Me.HDNtpcm_id.Value, False)
                'If DefaultValue Then
                '    If Me.DDLserviziPermessi.Items.Count = 0 Then
                '        ' Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.DDLorganizzazionePermessi.SelectedValue, Me.HDNtpcm_id.Value)
                '        Return Inserimento.NessunServizioPresente
                '    Else
                '        oTipoComunita.DefinisciPermessiRuoliDefault(Me.DDLorganizzazionePermessi.SelectedValue, Me.DDLserviziPermessi.SelectedValue)
                '        Me.TBLpermessiRuoli.Rows.Clear()
                '        Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.DDLorganizzazionePermessi.SelectedValue, Me.HDNtpcm_id.Value, True)
                '        Return Inserimento.PermessiAssociati
                '    End If
                'Else
                '    Return Me.AssociaPermessiRuoli(Replica)
                'End If
            Case IndiceSelezionato.Servizi
                If DefaultValue Then
                    oTipoComunita.DefinisciServiziDefault(Me.DDLorganizzazione.SelectedValue)
                    Me.Bind_Servizi()
                    Return Inserimento.ServiziDefault
                Else
                    If Me.HasServiziAssociati Then
                        Return AssociaServizi(Replica)
                    Else
                        Return Inserimento.SelezionaServizio
                    End If
                End If
            Case IndiceSelezionato.TipoRuolo
                Dim ListaRuoli As String = ","

                Try
                    totale = Me.CBLtipoRuolo.Items.Count() - 1
                    For i = 0 To totale
                        If CBLtipoRuolo.Items.Item(i).Selected = True Then
                            ListaRuoli &= Me.CBLtipoRuolo.Items.Item(i).Value & ","
                        End If
                    Next
                    If ListaRuoli = "," Then
                        ListaRuoli = ""
                        Return Inserimento.SelezionaRuolo
                    Else
                        ListaRuoli &= Me.DDLruoloDefault.SelectedValue & ","
                        oTipoComunita.DefinisciRuoli(ListaRuoli)
                        oTipoComunita.DefinisciRuoloDefault(Me.DDLruoloDefault.SelectedValue)
                        If oTipoComunita.ErroreDB = Errori_Db.None Then
                            Return Inserimento.ModificaRuoli
                        Else
                            Return Inserimento.NONModificato
                        End If
                    End If
                Catch ex As Exception
                    Return Inserimento.NONModificato
                End Try
            Case IndiceSelezionato.RuoliProfili
                Dim ListaRuoli As String = ","
                totale = Me.CBLtipoRuoloAll.Items.Count() - 1
                For i = 0 To totale
                    If CBLtipoRuoloAll.Items.Item(i).Selected = True Then
                        ListaRuoli &= Me.CBLtipoRuoloAll.Items.Item(i).Value & ","
                    End If
                Next
                If ListaRuoli = "," Then
                    ListaRuoli = ""
                    Return Inserimento.ErroreAssociazioneRuoliObbligatori
                Else
                    oTipoComunita.DefinisciRuoliProfiloObbligatori(ListaRuoli)
                    If oTipoComunita.ErroreDB = Errori_Db.None Then
                        Return Inserimento.Modificato
                    Else
                        Return Inserimento.NONModificato
                    End If
                End If
                '  oTipoComunita.
        End Select
    End Function
    Private Function Upload_logo(ByVal TPCM_ID As Integer) As String
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oFile As New COL_File
        Dim OLDpath, TempPath, NewPath As String
        oTipoComunita.ID = TPCM_ID

        If Not TXBFile.Value Is "" Then 'controllo che la texbox non sia vuota
            'cancello l'immagine vecchia se presente
            If Session("azione") = "modifica" Then
                oTipoComunita.Estrai()
                OLDpath = Server.MapPath("./../" & oTipoComunita.Icona)
                Try
                    Delete.File_FM(OLDpath)
                Catch ex As Exception

                End Try
            End If

            Try 'mando il file al server in una cartella temporanea

                TempPath = Server.MapPath("./../RadControls/TreeView/Skins/Comunita/logo/temp/")
                oFile.Upload(TXBFile, TempPath)

                Try 'ridimensiono l'immagine
                    Dim strFileNameOnly As String = oFile.FileNameOnly(TXBFile.PostedFile.FileName)
                    NewPath = Server.MapPath("./../RadControls/TreeView/Skins/Comunita/logo/" & strFileNameOnly)
                    If oFile.Resize(TempPath & strFileNameOnly, NewPath, 15, 15) < 1 Then
                        'IMFoto.Visible = False 'file non trovato su disco
                        'Me.PNLmessaggio.Visible = True
                        ' Me.LBerrore.Visible = True
                        'Me.LBerrore.Text = "Il file non è un'immagine,<br>Si prega di selezionare un file di tipo immagine"
                        'oFile.CancFile(TempPath & strFileNameOnly)
                        'oPersona.UpDateNomeFoto("")
                        ' Exit Try
                    Else
                        Delete.File_FM(TempPath & strFileNameOnly)
                    End If

                    Try
                        'aggiorno il nome del file sul db
                        oTipoComunita.UpDateNomeFoto("./RadControls/TreeView/Skins/Comunita/logo/" & strFileNameOnly)
                        If oTipoComunita.ErroreDB = Errori_Db.None Then
                            Return "./RadControls/TreeView/Skins/Comunita/logo/" & strFileNameOnly
                        End If
                    Catch ex As Exception
                        'errore nell'update del nome dell'immagine
                    End Try
                Catch ex As Exception
                    'errore nel ridimensionamento 
                End Try
            Catch ex As Exception
                'errore nell'upload
            End Try
        End If
        Return ""
    End Function

    Private Function AssociaServizi(ByVal Replica As Boolean) As Inserimento
        Dim i, totale As Integer
        Dim oTipocomunita As New COL_Tipo_Comunita

        Try
            Dim ListaAssociati As String = ","
            Dim ListaAttivati As String = ","

            totale = Me.RPTservizio.Items.Count()
            oTipocomunita.ID = Me.HDNtpcm_id.Value

            For i = 0 To totale - 1
                Dim oLabel As System.Web.UI.WebControls.Label

                Dim ocheck, ocheckDefault As System.Web.UI.WebControls.CheckBox
                ocheck = Me.RPTservizio.Items(i).FindControl("CBXservizioAssociato")
                ocheckDefault = Me.RPTservizio.Items(i).FindControl("CBXservizioAttivato")
                oLabel = Me.RPTservizio.Items(i).FindControl("LBsrvz_ID")
                If ocheckDefault.Checked Then
                    ListaAttivati &= oLabel.Text & ","
                End If
                If ocheck.Checked Then
                    ListaAssociati &= oLabel.Text & ","
                ElseIf ocheckDefault.Checked Then
                    ListaAssociati &= oLabel.Text & ","
                End If
            Next
            If ListaAssociati = "," Then
                ListaAssociati = ""
            End If
            If ListaAttivati = "," Then
                ListaAttivati = ""
            End If
            oTipocomunita.DefinisciServiziDisponibili(Me.DDLorganizzazione.SelectedValue, ListaAssociati, Replica)
            If oTipocomunita.ErroreDB = Errori_Db.None Then
                oTipocomunita.DefinisciServiziAttivati(Me.DDLorganizzazione.SelectedValue, ListaAttivati, Replica)
                Dim oManagerCommunity As New ManagerCommunity(PageUtility.CurrentUser, PageUtility.ComunitaCorrente, True)
                Dim oList As List(Of Community) = oManagerCommunity.ListAllForAdmin()
                For Each oCommunity As Community In (From o In oList Where o.Type.ID = Me.HDNtpcm_id.Value AndAlso (Me.DDLorganizzazione.SelectedValue = -1 OrElse Me.DDLorganizzazione.SelectedValue = o.Organization.ID))
                    COL_Comunita.DefinisciServiziStandardONLYforAdministration(oCommunity.ID, ListaAttivati)
                Next

                Return Inserimento.ServiziAssociati
            Else
                Return Inserimento.ErroreServizi
            End If
        Catch ex As Exception
            Return Inserimento.ErroreServizi
        End Try
    End Function
    Private Function HasServiziAssociati() As Boolean
        Dim i, totale As Integer

        totale = Me.RPTservizio.Items.Count() - 1

        For i = 0 To totale
            Dim ocheck As System.Web.UI.WebControls.CheckBox
            ocheck = Me.RPTservizio.Items(i).FindControl("CBXservizioAssociato")

            If ocheck.Checked Then
                Return True
            End If
        Next
    End Function



    'Private Function AssociaPermessiRuoli(ByVal Replica As Boolean) As Inserimento
    '    Dim oTipoComunita As New COL_Tipo_Comunita
    '    Dim iResponse As Inserimento = Inserimento.ErroreGenerico
    '    oTipoComunita.ID = Me.HDNtpcm_id.Value


    '    If Me.DDLserviziPermessi.Items.Count = 0 Then
    '        Return Inserimento.NessunServizioPresente
    '    Else
    '        Try
    '            Dim i, j, TPRL_ID, totaleV, totaleO, Posizione, Associati, Totali As Integer
    '            Dim oStringaPermessi, nome() As String
    '            Dim Permessi() As Char
    '            Dim oTableCell As TableCell

    '            totaleV = Me.TBLpermessiRuoli.Rows.Count - 1
    '            totaleO = Me.TBLpermessiRuoli.Rows(0).Cells.Count - 2
    '            TPRL_ID = 0
    '            For i = 1 To totaleV
    '                oStringaPermessi = "00000000000000000000000000000000"
    '                Permessi = oStringaPermessi.ToCharArray
    '                For j = 1 To totaleO
    '                    Dim oCheckbox As New HtmlControls.HtmlInputCheckBox
    '                    oTableCell = Me.TBLpermessiRuoli.Rows(i).Cells(j)
    '                    oCheckbox = oTableCell.Controls(0)
    '                    nome = oCheckbox.ID.Split("_")
    '                    TPRL_ID = nome(2)
    '                    Posizione = nome(1)

    '                    If oCheckbox.Checked Then
    '                        oStringaPermessi = oStringaPermessi.Remove(Posizione, 1)
    '                        If Posizione > 0 Then
    '                            oStringaPermessi = oStringaPermessi.Insert(Posizione, 1)
    '                        Else
    '                            oStringaPermessi = oStringaPermessi.Insert(0, 1)
    '                        End If
    '                    Else
    '                        oStringaPermessi = oStringaPermessi.Remove(Posizione, 1)
    '                        If Posizione > 0 Then
    '                            oStringaPermessi = oStringaPermessi.Insert(Posizione, 0)
    '                        Else
    '                            oStringaPermessi = oStringaPermessi.Insert(0, 0)
    '                        End If
    '                    End If
    '                Next
    '                If TPRL_ID <> 0 Then
    '                    oTipoComunita.DefinisciPermessiRuoli(Me.DDLorganizzazionePermessi.SelectedValue, Me.DDLserviziPermessi.SelectedValue, TPRL_ID, oStringaPermessi, Replica)
    '                    If oTipoComunita.ErroreDB = Errori_Db.None Then
    '                        Associati += 1
    '                    End If
    '                    Totali += 1
    '                End If
    '            Next
    '            If Totali > 0 Then
    '                If Totali = Associati Then
    '                    iResponse = Inserimento.PermessiAssociati
    '                ElseIf Associati > 0 Then
    '                    iResponse = Inserimento.PermessiAssociatiParziali
    '                Else
    '                    iResponse = Inserimento.NONModificato
    '                End If
    '            End If
    '        Catch ex As Exception

    '        End Try
    '    End If
    '    Return iResponse
    'End Function
#End Region


    Private Sub RPTservizio_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTservizio.ItemCreated
        If IsNothing(Resource) Then
            Me.SetCultureSettings()
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                Resource.setLabel(e.Item.FindControl("LBservizio_t"))
                Resource.setLabel(e.Item.FindControl("LBassociato_t"))
                Resource.setLabel(e.Item.FindControl("LBattiva_t"))
            Catch ex As Exception

            End Try
        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Try
                Resource.setCheckBox(e.Item.FindControl("CBXservizioAssociato"))
                Resource.setCheckBox(e.Item.FindControl("CBXservizioAttivato"))
            Catch ex As Exception

            End Try
        End If

    End Sub

    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
        Me.Bind_Servizi()
        RaiseEvent AggiornaMenuServizi(Me.DDLorganizzazione.SelectedValue = -1, Me.RPTservizio.Items.Count > 0, False)
    End Sub

#Region "Permission"
    Private Sub InitializeSettings()
        Me.TBLpermessi.Visible = True
        LoadOrganizations()
        If Me.DDLorganization.Items.Count > 0 Then
            Me.Loadmodules()
            If Me.DDLmodules.Items.Count > 0 Then
                'If Not IsNothing(DDLmodules.Items.FindByValue(PreloadedIdModule)) Then
                '    Me.DDLmodules.SelectedValue = PreloadedIdModule
                'End If
                LoadTemplatePermissions(Me.CurrentIdModule, Me.CurrentIdOrganization, True)

                RaiseEvent AggiornaMenuServizi((Me.CurrentIdOrganization = -1), True, True)
            End If
        Else
            Me.RPTtemplatePermission.Visible = False
            RaiseEvent AggiornaMenuServizi(False, False, False)
        End If
    End Sub

    Private Sub LoadOrganizations()
        Dim oLista As List(Of COL_Organizzazione) = COL_BusinessLogic_v2.Comunita.COL_Organizzazione.LazyElenca("", "", Session("ISTT_ID"))

        If oLista.Count > 0 Then
            Me.DDLorganization.DataTextField = "RagioneSociale"
            Me.DDLorganization.DataValueField = "Id"
            Me.DDLorganization.DataSource = oLista
            Me.DDLorganization.DataBind()
            Me.DDLorganization.Items.Insert(0, New ListItem("< Default >", -1))
            Me.DDLorganization.Enabled = True
        Else

            Me.DDLorganization.Items.Add(New ListItem("< Default >", -1))
            Me.DDLorganization.Enabled = False
        End If
    End Sub
    Private Sub Loadmodules()
        Try
            Dim modules As List(Of PlainService) = ManagerService.ListSystemTranslated(PageUtility.LinguaID, True)
            Dim items As List(Of Integer) = CurrentManager.GetAvailableModulesByCommunityType(IdCommunityType, Me.DDLorganization.SelectedValue)

            Me.DDLmodules.DataSource = modules.Where(Function(m) items.Contains(m.ID)).ToList()
            Me.DDLmodules.DataTextField = "Name"
            Me.DDLmodules.DataValueField = "Id"
            Me.DDLmodules.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DDLorganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganization.SelectedIndexChanged
        Dim IdModule As Integer = Me.CurrentIdModule
        If IdModule > 0 Then
            If Not IsNothing(Me.DDLmodules.Items.FindByValue(IdModule)) Then
                Me.DDLmodules.SelectedValue = IdModule
            End If
            LoadTemplatePermissions(IdModule, Me.CurrentIdOrganization, True)
        End If
        RaiseEvent AggiornaMenuServizi((Me.CurrentIdOrganization = -1), IdModule > 0, True)
    End Sub
    Private Sub DDLmodules_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLmodules.SelectedIndexChanged
        Dim IdModule As Integer = Me.CurrentIdModule
        If IdModule > 0 Then
            If Not IsNothing(Me.DDLmodules.Items.FindByValue(IdModule)) Then
                Me.DDLmodules.SelectedValue = IdModule
            End If
            Me.LoadTemplatePermissions(IdModule, Me.CurrentIdOrganization, True)
        End If
        'RaiseEvent AggiornaMenuServizi((Me.CurrentIdOrganization = -1), IdModule > 0, True)
    End Sub
    Protected Sub RPTtypes_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim dto As dtoTemplateModulePermission = e.Item.DataItem

            Dim controls As String = ","
            For Each permission As dtoPermission In dto.Positions
                controls &= "CB_" & permission.IdPosition & "_" & permission.IdOwner & ","
            Next

            Dim oButton As Button = e.Item.FindControl("BTNsetAll")
            oButton.OnClientClick = "SelezioneRiga('" & controls & "');return false;"
        End If
    End Sub
    Protected Sub RPTpermissionName_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim dto As Permessi = e.Item.DataItem
            Dim oButton As Button = e.Item.FindControl("BTNpermission")
            oButton.OnClientClick = "SelezioneColonna('CB_" & dto.Posizione & "');return false;"

        End If
    End Sub
    Protected Sub RPTpermissionValue_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim dto As dtoPermission = e.Item.DataItem
            Dim place As PlaceHolder = e.Item.FindControl("PLHpermission")
            Dim oCheckbox As New HtmlInputCheckBox
            Dim idCheck As String = "CB_" & dto.IdPosition & "_" & dto.IdOwner
            oCheckbox.ID = idCheck
            oCheckbox.Checked = dto.Selected
            oCheckbox.Value = idCheck
            If StartupPermission AndAlso dto.Selected Then
                If String.IsNullOrEmpty(Me.HIDcheckbox.Value) Then
                    Me.HIDcheckbox.Value = "," & idCheck & ","
                Else
                    Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & idCheck & ","
                End If
            ElseIf Not StartupPermission Then
                If Not String.IsNullOrEmpty(Me.HIDcheckbox.Value) AndAlso Me.HIDcheckbox.Value.Contains("," & idCheck & ",") Then
                    oCheckbox.Checked = True
                Else
                    oCheckbox.Checked = False
                End If
            End If
            oCheckbox.Attributes.Add("onclick", "SelectFromNameAndAssocia('" & idCheck & "');return true;")
            place.Controls.Add(oCheckbox)
        End If
    End Sub
    Private Sub LoadTemplatePermissions(ByVal idModule As Integer, ByVal idOrganization As Integer, Optional ByVal start As Boolean = False)
        StartupPermission = start
        Dim roles As List(Of Role) = COL_TipoRuolo.List(PageUtility.LinguaID)
        Dim permissions As List(Of Permessi) = COL_Servizio.ListPermessiServizio(idModule, PageUtility.LinguaID)
        Dim items As New List(Of dtoCommunityTypeTemplatePermission)
        Dim dto As New dtoCommunityTypeTemplatePermission()

        Dim rows As List(Of dtoTemplateModulePermission) = Me.CurrentManager.GetPermissionByCommunityType(IdCommunityType, idModule, idOrganization)
        dto.Columns = permissions

        Dim positions As List(Of Integer) = permissions.Select(Function(p) p.Posizione).ToList()
        Dim idRole As Integer = 0
        For Each row As dtoTemplateModulePermission In rows
            idRole = row.RoleId
            row.Update(roles.Where(Function(t) t.ID = idRole).Select(Function(t) t.Name).FirstOrDefault(), positions)
        Next
        dto.Rows = rows.OrderBy(Function(r) r.RoleName).ToList()
        items.Add(dto)
        Me.RPTtemplatePermission.DataSource = items
        Me.RPTtemplatePermission.DataBind()

        StartupPermission = False
    End Sub
    Private Function GetTemplatePermission() As List(Of dtoTemplateModulePermission)
        Dim items As New List(Of dtoTemplateModulePermission)


        If Not Me.DDLmodules.Items.Count = 0 Then
            Dim container As Repeater = RPTtemplatePermission.Items(0).FindControl("RPTtypes")

            For Each row As RepeaterItem In container.Items
                Dim idRole As Integer = CInt(DirectCast(row.FindControl("LTidRole"), Literal).Text)
                Dim item As New dtoTemplateModulePermission
                item.RoleId = idRole

                Dim repeater As Repeater = row.FindControl("RPTpermissionValue")
                Dim permission As Long = 0
                For Each subRow As RepeaterItem In repeater.Items
                    Dim place As PlaceHolder = subRow.FindControl("PLHpermission")

                    Dim oCheckbox As HtmlControls.HtmlInputCheckBox = place.Controls(0)
                    If oCheckbox.Checked Then
                        permission = permission Or (2 ^ CInt(DirectCast(subRow.FindControl("LTposition"), Literal).Text))
                    End If
                Next
                item.Permission = permission
                items.Add(item)
            Next
        End If
        Return items
    End Function
    Protected Function GetBackground(ByVal type As ListItemType) As String
        If type = ListItemType.Item Then
            Return "ColumnItem"
        Else
            Return "ColumnAlternateItem"

        End If
    End Function
#End Region

    <Serializable()> Public Class dtoCommunityTypeTemplatePermission
        Public Columns As List(Of Permessi)
        Public Rows As List(Of dtoTemplateModulePermission)
        Public Sub New()
            Columns = New List(Of Permessi)
            Rows = New List(Of dtoTemplateModulePermission)
        End Sub


    End Class
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
End Class