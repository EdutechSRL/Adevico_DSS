Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.FileLayer

Imports COL_BusinessLogic_v2.CL_permessi
Imports lm.Comol.Core.File

Public Class AdminG_WizardTipoComunita
    Inherits System.Web.UI.Page

    Private oResource As ResourceManager

    Public Enum Inserimento
        ErroreGenerico = 0
        Creato = 1
        Modificato = 2
        ServiziAssociati = 3
        PermessiAssociati = 4
        ServiziDefault = 5
        OperazioneConclusa = 6
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
        NessunaIconaValida = -11
        ErroreAssociazioneIcona = -12
        ErroreAssociazioneLingue = -13
        ErroreAssociazioneSottoComunita = -14
        ErroreAssociazioneRuoli = -15
        ErroreAssociazioneRuoloDefault = -16
        ErroreAssociazioneModelli = -17
        ErroreAssociazioneModelloDefault = -18
        ErroreAssociazioneCategorie = -19
        ErroreAssociazioneRuoliObbligatori = -20
    End Enum
    Public Enum IndiceFasi
        Fase1_Dati = 0
        Fase2_Categorie = 1
        Fase3_Ruoli = 2
        Fase4_RuoliObbligatori = 3
        Fase5_Modelli = 4
        Fase6_Finale = 5
        Fase7_Servizi = 6
        Fase8_Permessi = 7
    End Enum

    Protected WithEvents HDNtpcm_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HIDcheckbox As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNazione As System.Web.UI.HtmlControls.HtmlInputHidden

    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents TBRmenu As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

#Region "Dati tipologia"
    Protected WithEvents TBLdati As System.Web.UI.WebControls.Table
    Protected WithEvents LBtipoComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoSottoComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBtipoComunita As System.Web.UI.WebControls.TextBox

    Protected WithEvents RPTnome As System.Web.UI.WebControls.Repeater
    Protected WithEvents CHLtipoSottoComunita As System.Web.UI.WebControls.CheckBoxList
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
    Protected WithEvents LBmodelloDefault_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLmodelloDefault As System.Web.UI.WebControls.DropDownList
    Protected WithEvents CBLmodelli As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents LBmodelli_t As System.Web.UI.WebControls.Label
#End Region

#Region "Dati Finali"
    Protected WithEvents TBLverificaFinale As System.Web.UI.WebControls.Table
    Protected WithEvents LBverificaFinale As System.Web.UI.WebControls.Label
    Protected WithEvents LBicona_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBFile As System.Web.UI.HtmlControls.HtmlInputFile
#End Region
#Region "Servizi"
    Protected WithEvents TBLservizio As System.Web.UI.WebControls.Table
    Protected WithEvents LBserviziAttivi_t As System.Web.UI.WebControls.Label
    Protected WithEvents TBLtipoComunita As System.Web.UI.WebControls.Table
    Protected WithEvents LBorganizzazione_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents RPTservizio As System.Web.UI.WebControls.Repeater
#End Region

#Region "Servizi"
    Protected WithEvents TBLpermessi As System.Web.UI.WebControls.Table
    Protected WithEvents LBorganizzazionePermessi_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLorganizzazionePermessi As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBserviziPermessi_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLserviziPermessi As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBLpermessiRuoli As System.Web.UI.WebControls.Table
#End Region

    Protected WithEvents PNLnavigazione As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNelenco As System.Web.UI.WebControls.Button
    Protected WithEvents BTNindietro As System.Web.UI.WebControls.Button
    Protected WithEvents BTNavanti As System.Web.UI.WebControls.Button
    Protected WithEvents BTNconferma As System.Web.UI.WebControls.Button
    Protected WithEvents LNBaddRuolo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents PNLnavigazioneFinale As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBmanagement As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBdefault As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBtipocomunitaForAll As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBsalvaServizi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBindetroServizi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBgoToPermessi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBsalvaPermessi As System.Web.UI.WebControls.LinkButton

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
            Me.SetCulture(Session("LinguaCode"))
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        If Not Page.IsPostBack Then
            Dim oPersona As COL_Persona
            Session("azione") = "load"
            oPersona = Session("objPersona")

            Me.SetupInternazionalizzazione()
            If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.SysAdmin Or oPersona.TipoPersona.ID = Main.TipoPersonaStandard.AdminSecondario Then
                Me.PNLcontenuto.Visible = True
                Me.TBRmenu.Visible = False
                Me.PNLpermessi.Visible = False
                Me.Bind_dati()
            Else
                Me.TBRmenu.Visible = True
                Me.PNLcontenuto.Visible = False
                Me.PNLpermessi.Visible = True
            End If
        Else
            If Me.HDNazione.Value = "reload" Then
                Me.AggiornaRuoli()
                Me.HDNazione.Value = "gestioneTipo"
            End If
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
            Return False
        End If
    End Function

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_UC_modificaTipoComunita"
        oResource.Folder_Level1 = "Admin_Globale"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBtipoComunita_t)
            .setLabel(Me.LBtipoSottoComunita_t)
            .setLabel(Me.LBicona_t)
            .setLabel(Me.LBcategoriaFile_t)
            .setLabel(Me.LBtipiRuolo_t)
            .setLabel(Me.LBmodelli_t)
            .setLabel(Me.LBserviziAttivi_t)
            .setLabel(Me.LBorganizzazione_t)
            .setLabel(Me.LBruoloDefault_t)
            .setLabel(Me.LBorganizzazionePermessi_t)
            .setLabel(Me.LBserviziPermessi_t)
            .setLabel(Me.LBruoloDefault_t)
            .setLabel(Me.LBverificaFinale)
            .setButton(Me.BTNavanti, True, , , True)
            .setButton(Me.BTNconferma, True, , , True)
            .setButton(Me.BTNelenco, True, , , True)
            .setButton(Me.BTNindietro, True, , , True)
            .setLinkButton(Me.LNBaddRuolo, True, True)
            .setLabel(Me.LBtipiRuoloAll_t)
            Dim i_link As String
            i_link = "./AdminG_AggiungiTipoRuolo.aspx"
            Me.LNBaddRuolo.Attributes.Add("onclick", "OpenWin('" & i_link & "','600','700','no','no');return false;")
            .setLinkButton(Me.LNBmanagement, True, True)
            .setLinkButton(Me.LNBdefault, True, True)
            .setLinkButton(Me.LNBtipocomunitaForAll, True, True)
            .setLinkButton(Me.LNBsalvaServizi, True, True)

            .setLinkButton(Me.LNBsalvaPermessi, True, True)
            .setLinkButton(Me.LNBindietro, True, True)
            .setLinkButton(Me.LNBgoToPermessi, True, True)
            .setLinkButton(Me.LNBindetroServizi, True, True)
            .setLabel(Me.LBNopermessi)
        End With

    End Sub
#End Region

#Region "Bind_Dati"
    Private Sub Bind_dati()
        Session("Azione") = "loaded"
        Me.HDNtpcm_id.Value = -1
        Me.TBLcategoriaFile.Visible = False
        Me.TBLmodelli.Visible = False
        Me.TBLtipoRuolo.Visible = False
        Me.TBLruoliAllways.Visible = False
        Me.TBLservizio.Visible = False
        Me.TBLdati.Visible = True
        Me.TBLpermessi.Visible = False
        SetupInternazionalizzazione()

        Me.Bind_TipoRuolo()
        Me.Bind_modelli()
        Me.Bind_categoriaFile()
        Me.Bind_TipoSottoComunita()
        Me.Bind_Organizzazioni()
        Me.Bind_Lingue()
        TXBtipoComunita.Text = ""
        Me.BTNindietro.Visible = False
        Me.BTNconferma.Visible = False
        Me.BTNavanti.Visible = True
        Me.BTNelenco.Visible = True
        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase1_Dati, IndiceFasi))
        Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase1_Dati, IndiceFasi))

    End Sub
    Private Sub AggiornaRuoli()
        Me.Bind_TipoRuolo()
    End Sub

    Private Sub Bind_Lingue()
        Try
            Me.RPTnome.DataSource = ManagerLingua.List
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
                Me.DDLruoloDefault.SelectedIndex = 0
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
            If Me.HDNtpcm_id.Value > 0 Then
                oDataset = oTipoComunita.ElencaTipoRuoloAssociati(Session("LinguaID"), Main.FiltroRuoli.ForTipoComunita)

                totale = oDataset.Tables(0).Rows.Count
                If totale > 0 Then
                    Me.CBLtipoRuoloAll.DataSource = oDataset
                    Me.CBLtipoRuoloAll.DataTextField = "TPRL_Nome"
                    Me.CBLtipoRuoloAll.DataValueField = "TPRL_ID"
                    Me.CBLtipoRuoloAll.DataBind()

                    For i = 0 To Me.CBLtipoRuoloAll.Items.Count - 1
                        Dim oListitem As New ListItem
                        oListitem = Me.CBLtipoRuoloAll.Items(i)

                        Try
                            oListitem.Selected = oDataset.Tables(0).Rows(i).Item("LKTT_Allways")
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
                Me.CBLmodelli.DataSource = oDataset
                Me.CBLmodelli.DataTextField = "MDCM_Nome"
                Me.CBLmodelli.DataValueField = "MDCM_ID"
                Me.CBLmodelli.DataBind()


                Me.DDLmodelloDefault.DataSource = oDataset
                Me.DDLmodelloDefault.DataTextField = "MDCM_Nome"
                Me.DDLmodelloDefault.DataValueField = "MDCM_ID"
                Me.DDLmodelloDefault.DataBind()
                Me.DDLmodelloDefault.SelectedIndex = 0
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

                Try
                    Dim oListitem As ListItem
                    oListitem = Me.CBLcategoriaFile.Items.FindByValue(0)
                    oListitem.Selected = True
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TipoSottoComunita()
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oDataset As New DataSet

        Try
            Dim i, totale As Integer
            oDataset = oTipoComunita.ElencaSottoComunita(Me.HDNtpcm_id.Value, Session("LinguaID"))
            totale = oDataset.Tables(0).Rows.Count
            If totale > 0 Then
                Me.CHLtipoSottoComunita.DataSource = oDataset
                Me.CHLtipoSottoComunita.DataTextField = "TPCM_descrizione"
                Me.CHLtipoSottoComunita.DataValueField = "TPCM_id"
                Me.CHLtipoSottoComunita.DataBind()
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

    Private Sub Bind_ServiziPermessi()
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oDataset As DataSet

        Try
            Dim i, totale As Integer
            oTipoComunita.ID = Me.HDNtpcm_id.Value
            oDataset = oTipoComunita.ServiziAssociati(Me.DDLorganizzazione.SelectedValue, Session("LinguaID"))
            Me.DDLserviziPermessi.DataSource = oDataset
            Me.DDLserviziPermessi.DataTextField = "SRVZ_Nome"
            Me.DDLserviziPermessi.DataValueField = "SRVZ_ID"
            Me.DDLserviziPermessi.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_PermessiRuoliServizi(ByVal SRVZ_ID As Integer, ByVal ORGN_ID As Integer, ByVal TPCM_ID As Integer, Optional ByVal start As Boolean = False)
        Dim oServizio As New COL_Servizio
        Dim oDataset As New DataSet
        Dim oDatasetRuoli As New DataSet

        oServizio.ID = SRVZ_ID
        Try
            Dim i, j, totalePermessi, totale, PRMS_Posizione As Integer
            Dim ARRpermServizio As Integer()
            Dim oTBrow As New TableRow
            Dim oStringaPermessi As String
            oDataset = oServizio.ElencaPermessiAssociatiByLingua(Session("LinguaID"))
            totalePermessi = oDataset.Tables(0).Rows.Count - 1

            'Caricamento riga con nome permessi !

            Dim oTableCell As New TableCell
            oTableCell.Text = "&nbsp;"
            oTableCell.BackColor = System.Drawing.Color.Lavender
            oTBrow.Cells.Add(oTableCell)
            For i = 0 To totalePermessi
                Dim oRow As DataRow
                oTableCell = New TableCell
                oTableCell.HorizontalAlign = HorizontalAlign.Center

                oRow = oDataset.Tables(0).Rows(i)
                If IsDBNull(oRow.Item("Nome")) Then
                    Try
                        oTableCell.Text = oRow.Item("NomeDefault")
                    Catch ex As Exception
                        oTableCell.Text = "--"
                    End Try
                Else
                    oTableCell.Text = oRow.Item("Nome")
                End If
                oTableCell.CssClass = "Header_Repeater10"
                If i Mod 2 = 0 Then
                    oTableCell.BackColor = System.Drawing.Color.White
                Else
                    oTableCell.BackColor = System.Drawing.Color.LightYellow
                End If
                oTBrow.Cells.Add(oTableCell)
                ReDim Preserve ARRpermServizio(i)
                ARRpermServizio(i) = oRow.Item("PRMS_Posizione")
            Next
            Me.TBLpermessiRuoli.Rows.Add(oTBrow)

            oDatasetRuoli = oServizio.ElencaRuoliPermessiByTipoComunita(ORGN_ID, TPCM_ID)
            totale = oDatasetRuoli.Tables(0).Rows.Count - 1

            If start Then
                Me.HIDcheckbox.Value = ""
            End If
            Dim uniqueID As String
            For i = 0 To totale
                oTableCell = New TableCell
                Dim oRow As DataRow
                oTBrow = New TableRow

                oRow = oDatasetRuoli.Tables(0).Rows(i)

                oStringaPermessi = oRow.Item("LPRS_valore")

                Dim oLabel As New Label
                oLabel.ID = "LB" & i & "_" & (oRow.Item("TPRL_id"))
                oLabel.Text = oRow.Item("TPRL_nome")
                oTableCell.Controls.Add(oLabel)
                oTableCell.BackColor = System.Drawing.Color.Lavender
                oTBrow.Cells.Add(oTableCell)

                For j = 0 To UBound(ARRpermServizio)
                    oTableCell = New TableCell
                    oTableCell.HorizontalAlign = HorizontalAlign.Center

                    If j Mod 2 = 0 Then
                        oTableCell.BackColor = System.Drawing.Color.White
                    Else
                        oTableCell.BackColor = System.Drawing.Color.LightYellow
                    End If
                    Dim oCheckbox As New HtmlControls.HtmlInputCheckBox
                    PRMS_Posizione = ARRpermServizio(j)
                    uniqueID = PRMS_Posizione & "_" & oRow.Item("TPRL_id")
                    oCheckbox.ID = "CB_" & uniqueID
                    oCheckbox.Value = uniqueID
                    If start Then
                        If oStringaPermessi.Substring(PRMS_Posizione, 1) = 1 Then
                            oCheckbox.Checked = True
                            If Me.HIDcheckbox.Value = "" Then
                                Me.HIDcheckbox.Value = "," & uniqueID & ","
                            Else
                                Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & uniqueID & ","
                            End If
                        Else
                            oCheckbox.Checked = False
                        End If
                    Else
                        If InStr(Me.HIDcheckbox.Value, "," & uniqueID & ",") > 0 Then
                            oCheckbox.Checked = True
                        Else
                            oCheckbox.Checked = False
                        End If
                    End If

                    oCheckbox.Attributes.Add("onclick", "SelectFromNameAndAssocia('" & oCheckbox.ClientID & "','" & uniqueID & "');return true;")

                    oTableCell.Controls.Add(oCheckbox)

                    oTBrow.Cells.Add(oTableCell)
                Next

                Me.TBLpermessiRuoli.Rows.Add(oTBrow)
            Next
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Bind_Organizzazioni()
        Dim oOrganizzazione As New COL_Organizzazione
        Dim oDataset As New DataSet

        Try
            Me.DDLorganizzazione.Items.Clear()
            oDataset = oOrganizzazione.ElencaByIstituzione(Session("ISTT_ID"))
            If oDataset.Tables(0).Rows.Count > 0 Then
                Me.DDLorganizzazione.DataTextField = "ORGN_ragioneSociale"
                Me.DDLorganizzazione.DataValueField = "ORGN_ID"
                Me.DDLorganizzazione.DataSource = oDataset
                Me.DDLorganizzazione.DataBind()
                Me.DDLorganizzazione.Items.Insert(0, New ListItem("< Default >", -1))
                Me.DDLorganizzazione.Enabled = True

                Me.DDLorganizzazionePermessi.DataTextField = "ORGN_ragioneSociale"
                Me.DDLorganizzazionePermessi.DataValueField = "ORGN_ID"
                Me.DDLorganizzazionePermessi.DataSource = oDataset
                Me.DDLorganizzazionePermessi.DataBind()
                Me.DDLorganizzazionePermessi.Items.Insert(0, New ListItem("< Default >", -1))
                Me.DDLorganizzazionePermessi.Enabled = True
            Else
                Me.DDLorganizzazione.Items.Add(New ListItem("< Default >", -1))
                Me.DDLorganizzazione.Enabled = False

                Me.DDLorganizzazionePermessi.Items.Add(New ListItem("< Default >", -1))
                Me.DDLorganizzazionePermessi.Enabled = False
            End If
        Catch ex As Exception
            Me.DDLorganizzazione.Items.Add(New ListItem("< Default >", -1))
            Me.DDLorganizzazione.Enabled = False

            Me.DDLorganizzazionePermessi.Items.Add(New ListItem("< Default >", -1))
            Me.DDLorganizzazionePermessi.Enabled = False
        End Try
        Me.oResource.setDropDownList(Me.DDLorganizzazione, -1)
        Me.oResource.setDropDownList(Me.DDLorganizzazionePermessi, -1)
    End Sub
#End Region

#Region "Salvataggio Dati"
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
                Delete.File_FM(OLDpath)
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
        Dim i, totale, LKST_id, TPCM_ID As Integer
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

    Private Function AssociaPermessiRuoli(ByVal Replica As Boolean) As Inserimento
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim iResponse As Inserimento = Inserimento.ErroreGenerico
        oTipoComunita.ID = Me.HDNtpcm_id.Value


        If Me.DDLserviziPermessi.Items.Count = 0 Then
            Return Inserimento.NessunServizioPresente
        Else
            Try
                Dim i, j, TPRL_ID, totaleV, totaleO, Posizione, Associati, Totali As Integer
                Dim oStringaPermessi, nome() As String
                Dim Permessi() As Char
                Dim oTableCell As TableCell

                totaleV = Me.TBLpermessiRuoli.Rows.Count - 1
                totaleO = Me.TBLpermessiRuoli.Rows(0).Cells.Count - 1
                TPRL_ID = 0
                For i = 1 To totaleV
                    oStringaPermessi = "00000000000000000000000000000000"
                    Permessi = oStringaPermessi.ToCharArray
                    For j = 1 To totaleO
                        Dim oCheckbox As New HtmlControls.HtmlInputCheckBox
                        oTableCell = Me.TBLpermessiRuoli.Rows(i).Cells(j)
                        oCheckbox = oTableCell.Controls(0)
                        nome = oCheckbox.ID.Split("_")
                        TPRL_ID = nome(2)
                        Posizione = nome(1)

                        If oCheckbox.Checked Then
                            oStringaPermessi = oStringaPermessi.Remove(Posizione, 1)
                            If Posizione > 0 Then
                                oStringaPermessi = oStringaPermessi.Insert(Posizione, 1)
                            Else
                                oStringaPermessi = oStringaPermessi.Insert(0, 1)
                            End If
                        Else
                            oStringaPermessi = oStringaPermessi.Remove(Posizione, 1)
                            If Posizione > 0 Then
                                oStringaPermessi = oStringaPermessi.Insert(Posizione, 0)
                            Else
                                oStringaPermessi = oStringaPermessi.Insert(0, 0)
                            End If
                        End If
                    Next
                    If TPRL_ID <> 0 Then
                        oTipoComunita.DefinisciPermessiRuoli(Me.DDLorganizzazionePermessi.SelectedValue, Me.DDLserviziPermessi.SelectedValue, TPRL_ID, oStringaPermessi, Replica)
                        If oTipoComunita.ErroreDB = Errori_Db.None Then
                            Associati += 1
                        End If
                        Totali += 1
                    End If
                Next
                If Totali > 0 Then
                    If Totali = Associati Then
                        iResponse = Inserimento.PermessiAssociati
                    ElseIf Associati > 0 Then
                        iResponse = Inserimento.PermessiAssociatiParziali
                    Else
                        iResponse = Inserimento.NONModificato
                    End If
                End If
            Catch ex As Exception

            End Try
        End If
        Return iResponse
    End Function
#End Region

    Private Sub RPTservizio_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTservizio.ItemCreated
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                oResource.setLabel(e.Item.FindControl("LBservizio_t"))
            Catch ex As Exception

            End Try
            Try
                oResource.setLabel(e.Item.FindControl("LBassociato_t"))
            Catch ex As Exception

            End Try
            Try
                oResource.setLabel(e.Item.FindControl("LBattiva_t"))
            Catch ex As Exception

            End Try
        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Try
                oResource.setCheckBox(e.Item.FindControl("CBXservizioAssociato"))
            Catch ex As Exception

            End Try
            Try
                oResource.setCheckBox(e.Item.FindControl("CBXservizioAttivato"))
            Catch ex As Exception

            End Try
        End If

    End Sub

    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
        Me.Bind_Servizi()
        Me.LNBtipocomunitaForAll.Enabled = (Me.RPTservizio.Items.Count > 0)
        Me.LNBtipocomunitaForAll.Visible = (Me.DDLorganizzazione.SelectedValue = -1)
        Me.LNBdefault.Enabled = Me.LNBtipocomunitaForAll.Enabled
        Me.LNBdefault.Visible = Not Me.LNBtipocomunitaForAll.Visible
    End Sub

    Private Sub DDLorganizzazionePermessi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazionePermessi.SelectedIndexChanged
        Dim ServizioID As Integer

        Try
            ServizioID = Me.DDLserviziPermessi.SelectedValue
        Catch ex As Exception

        End Try
        Me.Bind_ServiziPermessi()
        If Me.DDLserviziPermessi.Items.Count > 0 Then
            Try
                Me.DDLserviziPermessi.SelectedValue = ServizioID
            Catch ex As Exception
                Me.DDLserviziPermessi.SelectedIndex = 0
            End Try
            Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.DDLorganizzazionePermessi.SelectedValue, Me.HDNtpcm_id.Value, True)
            Me.LNBtipocomunitaForAll.Enabled = True
        Else
            Me.LNBtipocomunitaForAll.Enabled = False
        End If
        Me.LNBtipocomunitaForAll.Visible = (Me.DDLorganizzazionePermessi.SelectedValue = -1)
        Me.LNBdefault.Enabled = Me.LNBtipocomunitaForAll.Enabled
        Me.LNBdefault.Visible = Not Me.LNBtipocomunitaForAll.Visible
    End Sub

    Private Sub DDLserviziPermessi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLserviziPermessi.SelectedIndexChanged
        Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.DDLorganizzazionePermessi.SelectedValue, Me.HDNtpcm_id.Value, True)
    End Sub

#Region "Navigazione Standard"
    Private Sub BTNelenco_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNelenco.Click, LNBmanagement.Click
        Me.Response.Redirect("./AdminG_TipiComunita.aspx")
    End Sub
    Private Sub BTNindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNindietro.Click
        If Session("Azione") <> "inserted" Then
            Me.BTNconferma.Visible = False
            Me.BTNavanti.Visible = True
            Me.LNBaddRuolo.Visible = False
            If Me.TBLcategoriaFile.Visible = True Then
                Me.BTNindietro.Visible = False
                Me.TBLcategoriaFile.Visible = False
                Me.TBLdati.Visible = True
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase1_Dati, IndiceFasi))
                Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase1_Dati, IndiceFasi))

            ElseIf Me.TBLtipoRuolo.Visible = True Then
                Me.TBLtipoRuolo.Visible = False
                Me.TBLcategoriaFile.Visible = True
                Me.LNBaddRuolo.Visible = False
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase2_Categorie, IndiceFasi))
                Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase2_Categorie, IndiceFasi))

            ElseIf Me.TBLruoliAllways.Visible = True Then
                Me.TBLruoliAllways.Visible = False
                Me.TBLtipoRuolo.Visible = True
                Me.LNBaddRuolo.Visible = True
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase3_Ruoli, IndiceFasi))
                Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase3_Ruoli, IndiceFasi))

            ElseIf Me.TBLmodelli.Visible = True Then
                Me.TBLmodelli.Visible = False
                Me.TBLruoliAllways.Visible = True
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase4_RuoliObbligatori, IndiceFasi))
                Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase4_RuoliObbligatori, IndiceFasi))

            ElseIf Me.TBLverificaFinale.Visible = True Then
                Me.TBLverificaFinale.Visible = False
                Me.TBLmodelli.Visible = True
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase5_Modelli, IndiceFasi))
                Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase5_Modelli, IndiceFasi))

            End If
        Else
            Me.Setup_NavigazioneServizi()
            Me.TBLdati.Visible = False
            Me.TBLcategoriaFile.Visible = False
            Me.TBLmodelli.Visible = False
            Me.TBLruoliAllways.Visible = False
            Me.TBLverificaFinale.Visible = False
            Me.TBLtipoRuolo.Visible = False

            Me.BTNconferma.Visible = False
            Me.BTNindietro.Visible = False

            Me.TBLservizio.Visible = True
            Me.TBLpermessi.Visible = False
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase7_Servizi, IndiceFasi))
            Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase7_Servizi, IndiceFasi))

            Me.BTNavanti.Visible = True

            Me.DDLorganizzazione.SelectedIndex = 0
            Me.Bind_Servizi()
        End If
    End Sub
    Private Sub BTNavanti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNavanti.Click
        Dim alertMSG As String = ""

        If Session("Azione") <> "inserted" Then
            Me.LNBaddRuolo.Visible = False
            Me.BTNindietro.Visible = True
            Me.BTNconferma.Visible = False
            If Me.TBLdati.Visible = True And Me.TXBtipoComunita.Text <> "" Then
                Me.TBLdati.Visible = False
                Me.TBLcategoriaFile.Visible = True
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase2_Categorie, IndiceFasi))
                Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase2_Categorie, IndiceFasi))

            ElseIf Me.TBLdati.Visible = True And Me.TXBtipoComunita.Text = "" Then
                Me.BTNindietro.Visible = False
            ElseIf Me.TBLcategoriaFile.Visible = True Then
                If Me.CBLcategoriaFile.SelectedIndex = -1 Then
                    alertMSG = Me.oResource.getValue("Inserimento." & CType(Me.Inserimento.SelezionaCategoria, Inserimento))
                    If alertMSG <> "" Then
                        alertMSG = alertMSG.Replace("'", "\'")
                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                    End If
                    Exit Sub
                End If
                Me.TBLcategoriaFile.Visible = False
                Me.TBLtipoRuolo.Visible = True
                Me.LNBaddRuolo.Visible = True
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase3_Ruoli, IndiceFasi))
                Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase3_Ruoli, IndiceFasi))

            ElseIf Me.TBLtipoRuolo.Visible = True Then
                If Me.CBLtipoRuolo.SelectedIndex = -1 Then
                    alertMSG = Me.oResource.getValue("Inserimento." & CType(Me.Inserimento.SelezionaRuolo, Inserimento))
                    If alertMSG <> "" Then
                        alertMSG = alertMSG.Replace("'", "\'")
                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                    End If
                    Exit Sub
                End If
                Me.LNBaddRuolo.Visible = False
                Me.TBLtipoRuolo.Visible = False
                Me.TBLruoliAllways.Visible = True
                If Me.CBLtipoRuoloAll.SelectedIndex = -1 Then
                    Me.Bind_TipoRuoloObbligatori()
                Else
                    If Me.CBLtipoRuolo.SelectedIndex > -1 Then
                        Dim i As Integer
                        For i = 0 To Me.CBLtipoRuolo.Items.Count - 1
                            Dim oListitem As New ListItem
                            If Me.CBLtipoRuolo.Items(i).Selected Then
                                If IsNothing(Me.CBLtipoRuoloAll.Items.FindByValue(Me.CBLtipoRuolo.Items(i).Value)) Then
                                    oListitem.Text = Me.CBLtipoRuolo.Items(i).Text
                                    oListitem.Value = Me.CBLtipoRuolo.Items(i).Value
                                    Me.CBLtipoRuoloAll.Items.Add(oListitem)
                                End If
                            Else
                                oListitem = Me.CBLtipoRuoloAll.Items.FindByValue(Me.CBLtipoRuolo.Items(i).Value)
                                If Not IsNothing(oListitem) Then
                                    Me.CBLtipoRuoloAll.Items.Remove(oListitem)
                                End If
                            End If
                        Next
                    End If
                End If
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase4_RuoliObbligatori, IndiceFasi))
                Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase4_RuoliObbligatori, IndiceFasi))

            ElseIf Me.TBLruoliAllways.Visible = True Then
                If Me.CBLtipoRuoloAll.SelectedIndex = -1 Then
                    alertMSG = Me.oResource.getValue("Inserimento." & CType(Me.Inserimento.SelezionaRuolo, Inserimento))
                    If alertMSG <> "" Then
                        alertMSG = alertMSG.Replace("'", "\'")
                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                    End If
                    Exit Sub
                End If
                Me.TBLruoliAllways.Visible = False
                Me.TBLmodelli.Visible = True
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase5_Modelli, IndiceFasi))
                Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase5_Modelli, IndiceFasi))

            ElseIf Me.TBLmodelli.Visible = True Then
                If Me.CBLmodelli.SelectedIndex = -1 Then
                    alertMSG = Me.oResource.getValue("Inserimento." & CType(Me.Inserimento.SelezionaModello, Inserimento))
                    If alertMSG <> "" Then
                        alertMSG = alertMSG.Replace("'", "\'")
                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                    End If
                    Exit Sub
                End If
                Me.TBLmodelli.Visible = False
                Me.TBLverificaFinale.Visible = True
                Me.BTNavanti.Visible = False
                Me.BTNconferma.Visible = True
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase6_Finale, IndiceFasi))
                Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase6_Finale, IndiceFasi))

            End If
        Else
            Me.Setup_NavigazioneServizi()


            Me.TBLdati.Visible = False
            Me.TBLcategoriaFile.Visible = False
            Me.TBLmodelli.Visible = False
            Me.TBLverificaFinale.Visible = False
            Me.TBLtipoRuolo.Visible = False
            Me.TBLruoliAllways.Visible = False

            Me.BTNavanti.Visible = False
            Me.BTNconferma.Visible = False
            Me.BTNindietro.Visible = False
            If Me.TBLservizio.Visible = False And Me.TBLpermessi.Visible = False Then
                Me.TBLservizio.Visible = True
                Me.TBLpermessi.Visible = False
                ' Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase7_Servizi, IndiceFasi))
                Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase7_Servizi, IndiceFasi))

                Me.BTNavanti.Visible = True
                Me.DDLorganizzazione.SelectedIndex = 0
                Me.Bind_Servizi()

            ElseIf Me.TBLservizio.Visible = True Then
                Me.TBLservizio.Visible = False
                Me.TBLpermessi.Visible = True
                'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase8_Permessi, IndiceFasi))
                Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase8_Permessi, IndiceFasi))

                Me.BTNindietro.Visible = True

                Me.DDLorganizzazionePermessi.SelectedIndex = 0
                Me.Bind_ServiziPermessi()
                Me.DDLserviziPermessi.SelectedIndex = 0
                Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.DDLorganizzazionePermessi.SelectedValue, Me.HDNtpcm_id.Value, True)
            End If
        End If
    End Sub
    Private Sub BTNconferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNconferma.Click
        Dim alertMSG As String = ""
        Dim iResponse As Inserimento
        Dim iRisultato As Inserimento

        If Session("azione") <> "inserted" Then
            If Me.TXBFile.Value = "" Then
                iResponse = Inserimento.NessunaIconaValida
            Else
                Dim Percorso As String = ""
                If VerificaDownload(Percorso) = True Then
                    Dim oTipoComunita As New COL_Tipo_Comunita
                    oTipoComunita.Descrizione = Me.TXBtipoComunita.Text
                    oTipoComunita.Aggiungi()
                    If oTipoComunita.ErroreDB = Errori_Db.None Then
                        Me.HDNtpcm_id.Value = oTipoComunita.ID
                        Me.DefinizioneElementiTipoComunita(Percorso)
                        Session("azione") = "inserted"
                        Me.Setup_NavigazioneServizi()
                        Exit Sub
                    Else
                        iResponse = Inserimento.NONinserito
                    End If
                Else
                    iResponse = Inserimento.NessunaIconaValida
                End If
            End If

            alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
        Else
            Me.Setup_NavigazioneServizi()
        End If

    End Sub
#End Region

#Region "Salvataggio Dati"
    Private Function VerificaDownload(ByRef Percorso As String) As Boolean
        Dim oFile As New COL_File
        Dim TempPath, NewPath As String

        Try
            TempPath = Server.MapPath("./../RadControls/TreeView/Skins/Comunita/logo/temp/")
            oFile.Upload(TXBFile, TempPath)

            Try 'ridimensiono l'immagine
                Dim strFileNameOnly As String = oFile.FileNameOnly(TXBFile.PostedFile.FileName)
                NewPath = Server.MapPath("./../RadControls/TreeView/Skins/Comunita/logo/" & strFileNameOnly)
                'If oFile.Resize(TempPath & strFileNameOnly, NewPath, 15, 15) < 1 Then
                'Else
                '    oFile.CancFile(TempPath & strFileNameOnly)
                'End If
                Percorso = "./RadControls/TreeView/Skins/Comunita/logo/" & strFileNameOnly
                Return True
            Catch ex As Exception

            End Try
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub DefinizioneElementiTipoComunita(ByVal Percorso As String)
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim messaggio, valore As String
        Dim iResponse As Inserimento

        oTipoComunita.ID = Me.HDNtpcm_id.Value
        messaggio = Me.oResource.getValue("Inserimento." & CType(Inserimento.Creato, Inserimento))
        oTipoComunita.UpDateNomeFoto(Percorso)

        If oTipoComunita.ErroreDB <> Errori_Db.None Then
            valore = Me.oResource.getValue("Inserimento." & CType(Inserimento.ErroreAssociazioneIcona, Inserimento))
            If valore <> "" Then
                If messaggio <> "" Then
                    messaggio &= "\r"
                End If
                messaggio &= valore
            End If
        End If

        'Definizione lingue
        iResponse = Me.Salva_DefinizioniLingue()
        If iResponse <> Inserimento.OperazioneConclusa Then
            valore = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
            If valore <> "" Then
                If messaggio <> "" Then
                    messaggio &= "\r"
                End If
                messaggio &= valore
            End If
        End If

        'Definizione sotto Comunità
        iResponse = Me.Salva_SottoComunita()
        If iResponse <> Inserimento.OperazioneConclusa Then
            valore = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
            If valore <> "" Then
                If messaggio <> "" Then
                    messaggio &= "\r"
                End If
                messaggio &= valore
            End If
        End If

        'Definizione Ruoli
        iResponse = Me.Salva_Ruoli()
        If iResponse <> Inserimento.OperazioneConclusa Then
            valore = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
            If valore <> "" Then
                If messaggio <> "" Then
                    messaggio &= "\r"
                End If
                messaggio &= valore
            End If
        End If

        'Definizione Ruoli Obbligatori
        iResponse = Me.Salva_RuoliObbligatori
        If iResponse <> Inserimento.OperazioneConclusa Then
            valore = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
            If valore <> "" Then
                If messaggio <> "" Then
                    messaggio &= "\r"
                End If
                messaggio &= valore
            End If
        End If

        'Definizione Categorie
        iResponse = Me.Salva_CategorieFile()
        If iResponse <> Inserimento.OperazioneConclusa Then
            valore = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
            If valore <> "" Then
                If messaggio <> "" Then
                    messaggio &= "\r"
                End If
                messaggio &= valore
            End If
        End If

        'Definizione Modelli
        iResponse = Me.Salva_Modelli()
        If iResponse <> Inserimento.OperazioneConclusa Then
            valore = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
            If valore <> "" Then
                If messaggio <> "" Then
                    messaggio &= "\r"
                End If
                messaggio &= valore
            End If
        End If
        Session("Azione") = "inserted"

        If messaggio <> "" Then
            messaggio = messaggio.Replace("'", "\'")
            Response.Write("<script language='javascript'>alert('" & messaggio & "');</script>")
        End If
    End Sub
    Private Function Salva_DefinizioniLingue() As Inserimento
        Dim LinguaID, i, totale As Integer
        Dim Termine As String = ""
        Dim oTipoComunita As New COL_Tipo_Comunita

        oTipoComunita.ID = Me.HDNtpcm_id.Value
        totale = Me.RPTnome.Items.Count

        Try
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
                Return Inserimento.OperazioneConclusa
            Else
                Return Inserimento.ErroreAssociazioneLingue
            End If
        Catch ex As Exception
            Return Inserimento.ErroreAssociazioneLingue
        End Try

    End Function
    Private Function Salva_SottoComunita() As Inserimento
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim i, totale As Integer
        oTipoComunita.ID = Me.HDNtpcm_id.Value
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
                Return Inserimento.OperazioneConclusa
            Else
                Return Inserimento.ErroreAssociazioneSottoComunita
            End If
        Catch ex As Exception
            Return Inserimento.ErroreAssociazioneSottoComunita
        End Try
    End Function
    Private Function Salva_Ruoli() As Inserimento
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim i, totale As Integer
        oTipoComunita.ID = Me.HDNtpcm_id.Value

        Dim ListaRuoli As String = ","
        totale = Me.CBLtipoRuolo.Items.Count() - 1
        For i = 0 To totale
            If CBLtipoRuolo.Items.Item(i).Selected = True Then
                ListaRuoli &= Me.CBLtipoRuolo.Items.Item(i).Value & ","
            End If
        Next
        If ListaRuoli = "," Then
            ListaRuoli = ""
            Return Inserimento.ErroreAssociazioneRuoli
        Else
            oTipoComunita.DefinisciRuoli(ListaRuoli)
            If oTipoComunita.ErroreDB = Errori_Db.None Then
                oTipoComunita.DefinisciRuoloDefault(Me.DDLruoloDefault.SelectedValue)
                If oTipoComunita.ErroreDB = Errori_Db.None Then
                    Return Inserimento.OperazioneConclusa
                Else
                    Return Inserimento.ErroreAssociazioneRuoloDefault
                End If
            Else
                Return Inserimento.ErroreAssociazioneRuoli
            End If
        End If
    End Function
    Private Function Salva_RuoliObbligatori() As Inserimento
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim i, totale As Integer
        oTipoComunita.ID = Me.HDNtpcm_id.Value

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
                Return Inserimento.OperazioneConclusa
            Else
                Return Inserimento.ErroreAssociazioneRuoliObbligatori
            End If
        End If
    End Function
    Private Function Salva_CategorieFile() As Inserimento
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim i, totale As Integer
        oTipoComunita.ID = Me.HDNtpcm_id.Value

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
                Return Inserimento.ErroreAssociazioneCategorie
            End If
            oTipoComunita.DefinisciCategorieFile(ListaCategorie)
            If oTipoComunita.ErroreDB = Errori_Db.None Then
                Return Inserimento.OperazioneConclusa
            Else
                Return Inserimento.ErroreAssociazioneCategorie
            End If
        Catch ex As Exception
            Return Inserimento.ErroreAssociazioneCategorie
        End Try
    End Function
    Private Function Salva_Modelli() As Inserimento
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim i, totale As Integer
        oTipoComunita.ID = Me.HDNtpcm_id.Value

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
                Return Inserimento.ErroreAssociazioneModelli
            End If
            oTipoComunita.DefinisciModelli(ListaModelli)

            If DDLmodelloDefault.SelectedIndex > -1 Then
                oTipoComunita.DefinisciModelloDefault(Me.CBLmodelli.Items(DDLmodelloDefault.SelectedIndex).Value)
            End If
            If oTipoComunita.ErroreDB = Errori_Db.None Then
                Return Inserimento.OperazioneConclusa
            Else
                Return Inserimento.ErroreAssociazioneModelli
            End If
        Catch ex As Exception
            Return Inserimento.ErroreAssociazioneModelli
        End Try
    End Function
#End Region

#Region "Definizione Servizi e Permessi"
    Private Sub Setup_NavigazioneServizi()
        Me.TBLdati.Visible = False
        Me.TBLcategoriaFile.Visible = False
        Me.TBLmodelli.Visible = False
        Me.TBLverificaFinale.Visible = False
        Me.TBLtipoRuolo.Visible = False
        Me.TBLruoliAllways.Visible = False

        Me.PNLnavigazione.Visible = False
        Me.PNLnavigazioneFinale.Visible = True


        Me.Bind_Organizzazioni()
        Me.DDLorganizzazione.SelectedIndex = 0
        Me.Bind_Servizi()
        Me.TBLservizio.Visible = True
        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase7_Servizi, IndiceFasi))
        Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase7_Servizi, IndiceFasi))

        If Me.HasServiziAssociati Then
            Me.DDLorganizzazione.Enabled = True
        Else
            Me.DDLorganizzazione.Enabled = False
            Me.LNBtipocomunitaForAll.Visible = False
            Me.LNBdefault.Visible = False
            If Me.RPTservizio.Items.Count > 0 Then
                Me.LNBsalvaServizi.Enabled = True
            Else
                Me.LNBsalvaServizi.Enabled = False
            End If
        End If
        Me.LNBsalvaPermessi.Visible = False
        Me.LNBgoToPermessi.Visible = True
        Me.LNBgoToPermessi.Enabled = False
        Me.LNBindetroServizi.Visible = False
    End Sub
    Private Sub LNBsalvaServizi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalvaServizi.Click
        Dim iResponse As Inserimento
        Dim alertMSG As String = ""

        Me.LNBgoToPermessi.Enabled = False
        Try
            If Me.HasServiziAssociati Then
                iResponse = AssociaServizi(False)

                If iResponse = Inserimento.ServiziAssociati Then
                    Me.DDLorganizzazione.Enabled = True
                    Me.LNBtipocomunitaForAll.Visible = True
                    Me.LNBgoToPermessi.Enabled = True
                End If
            Else
                iResponse = Inserimento.SelezionaServizio
            End If

        Catch ex As Exception
            iResponse = Inserimento.ErroreGenerico
        End Try

        alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
        If alertMSG <> "" Then
            alertMSG = alertMSG.Replace("'", "\'")
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
        End If
        Me.LNBtipocomunitaForAll.Enabled = (Me.RPTservizio.Items.Count > 0)
        Me.LNBsalvaServizi.Enabled = Me.LNBtipocomunitaForAll.Enabled
        Me.LNBdefault.Enabled = Me.LNBtipocomunitaForAll.Enabled
        Me.LNBindetroServizi.Visible = False
        Me.LNBgoToPermessi.Visible = True
    End Sub

    Private Sub LNBdefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdefault.Click
        Dim iResponse As Inserimento
        Dim alertMSG As String = ""

        If Me.TBLservizio.Visible Then
            Try
                Dim oTipoComunita As New COL_Tipo_Comunita
                oTipoComunita.ID = Me.HDNtpcm_id.Value
                oTipoComunita.DefinisciServiziDefault(Me.DDLorganizzazione.SelectedValue)
                If oTipoComunita.ErroreDB = Errori_Db.None Then
                    iResponse = Inserimento.ServiziDefault
                Else
                    iResponse = Inserimento.ErroreServizi
                End If
                Me.Bind_Servizi()

            Catch ex As Exception
                iResponse = Inserimento.ErroreServizi
            End Try
        ElseIf Me.TBLpermessi.Visible Then
            Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.DDLorganizzazionePermessi.SelectedValue, Me.HDNtpcm_id.Value, False)

            If Me.DDLserviziPermessi.Items.Count = 0 Then
                iResponse = Inserimento.NessunServizioPresente
            Else
                Dim oTipoComunita As New COL_Tipo_Comunita
                oTipoComunita.ID = Me.HDNtpcm_id.Value

                oTipoComunita.DefinisciPermessiRuoliDefault(Me.DDLorganizzazionePermessi.SelectedValue, Me.DDLserviziPermessi.SelectedValue)
                Me.TBLpermessiRuoli.Rows.Clear()
                Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.DDLorganizzazionePermessi.SelectedValue, Me.HDNtpcm_id.Value, True)
                iResponse = Inserimento.PermessiAssociati
            End If
        End If
        alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
        If alertMSG <> "" Then
            alertMSG = alertMSG.Replace("'", "\'")
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
        End If
    End Sub

    Private Sub LNBtipocomunitaForAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBtipocomunitaForAll.Click
        Dim iResponse As Inserimento
        Dim alertMSG As String = ""

        If Me.TBLservizio.Visible Then
            If Me.HasServiziAssociati Then
                iResponse = AssociaServizi(True)
            Else
                iResponse = Inserimento.SelezionaServizio
            End If
        ElseIf TBLpermessi.Visible Then
            Try
                Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.DDLorganizzazionePermessi.SelectedValue, Me.HDNtpcm_id.Value, False)
                iResponse = Me.AssociaPermessiRuoli(True)
            Catch ex As Exception
                iResponse = Inserimento.NessunServizioPresente
            End Try
        End If
        alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
        If alertMSG <> "" Then
            alertMSG = alertMSG.Replace("'", "\'")
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
        End If
    End Sub

    Private Sub LNBgoToPermessi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBgoToPermessi.Click
        Me.TBLpermessi.Visible = True
        Me.TBLservizio.Visible = False

        Me.DDLorganizzazionePermessi.SelectedIndex = 0
        Me.Bind_ServiziPermessi()
        Me.LNBgoToPermessi.Visible = False
        Me.LNBindetroServizi.Visible = True
        Me.LNBsalvaServizi.Visible = False
        Me.LNBsalvaPermessi.Visible = True

        Try
            If Me.DDLserviziPermessi.Items.Count > 0 Then
                Me.DDLserviziPermessi.SelectedIndex = 0
                Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.DDLorganizzazionePermessi.SelectedValue, Me.HDNtpcm_id.Value, True)

                Me.LNBtipocomunitaForAll.Visible = (Me.DDLorganizzazionePermessi.SelectedValue = -1)
                Me.LNBtipocomunitaForAll.Enabled = True
                Me.LNBdefault.Enabled = True
                Me.LNBdefault.Visible = Not Me.LNBtipocomunitaForAll.Visible
                Me.LNBsalvaPermessi.Enabled = True
            Else
                Me.TBLpermessiRuoli.Rows.Clear()

                Me.LNBtipocomunitaForAll.Visible = (Me.DDLorganizzazionePermessi.SelectedValue = -1)
                Me.LNBtipocomunitaForAll.Enabled = False
                Me.LNBsalvaPermessi.Enabled = False
                Me.LNBdefault.Enabled = False
                Me.LNBdefault.Visible = Not Me.LNBtipocomunitaForAll.Visible
                Me.LNBsalvaPermessi.Enabled = False
            End If
        Catch ex As Exception
            Me.LNBtipocomunitaForAll.Visible = False
            Me.LNBsalvaPermessi.Enabled = False
            Me.LNBdefault.Visible = False
        End Try
        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase8_Permessi, IndiceFasi))
        Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase7_Servizi, IndiceFasi))

    End Sub

    Private Sub LNBindetroServizi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindetroServizi.Click
        Me.TBLpermessi.Visible = False
        Me.TBLservizio.Visible = True
        Me.DDLorganizzazione.SelectedIndex = 0
        Me.Bind_Servizi()
        Me.LNBindetroServizi.Visible = False
        Me.LNBgoToPermessi.Visible = True
        Me.LNBsalvaPermessi.Visible = False
        Me.LNBsalvaServizi.Visible = True

        If Me.RPTservizio.Items.Count > 0 Then
            Me.LNBsalvaServizi.Enabled = True
        Else
            Me.LNBsalvaServizi.Enabled = False
        End If
        Me.LNBtipocomunitaForAll.Enabled = Me.LNBsalvaServizi.Enabled
        Me.LNBdefault.Enabled = Me.LNBsalvaServizi.Enabled
        Try
            If Me.DDLorganizzazione.SelectedValue = -1 Then
                Me.LNBdefault.Visible = False
                Me.LNBtipocomunitaForAll.Visible = True
            Else
                Me.LNBdefault.Visible = True
                Me.LNBtipocomunitaForAll.Visible = False
            End If
        Catch ex As Exception

        End Try
        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase7_Servizi, IndiceFasi))
        Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase7_Servizi, IndiceFasi))

    End Sub

    Private Sub LNBsalvaPermessi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalvaPermessi.Click
        Dim iResponse As Inserimento
        Dim alertMSG As String = ""

        Try
            Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.DDLorganizzazionePermessi.SelectedValue, Me.HDNtpcm_id.Value, False)
            iResponse = Me.AssociaPermessiRuoli(False)
        Catch ex As Exception

        End Try
        alertMSG = Me.oResource.getValue("Inserimento." & CType(iResponse, Inserimento))
        If alertMSG <> "" Then
            alertMSG = alertMSG.Replace("'", "\'")
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
        End If
    End Sub

#End Region

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property

End Class