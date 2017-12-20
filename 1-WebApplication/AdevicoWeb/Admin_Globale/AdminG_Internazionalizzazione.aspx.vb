Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2


Public Class AdminG_Internazionalizzazione
    Inherits System.Web.UI.Page

    Private oInternaz As New COL_Internazionalizzazione
    Private oResource As ResourceManager

    Private Enum stringaInserimento
        Inserito = 0
        InserireValore = 1
        NonInserito = 2
        Eliminato = 3
        NonEliminato = 4
    End Enum
#Region "Pannello_Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

#Region "contenuto"
    Protected WithEvents TCTabelle As System.Web.UI.WebControls.TableCell
    Protected WithEvents TCCampi As System.Web.UI.WebControls.TableCell
    Protected WithEvents TCDati As System.Web.UI.WebControls.TableCell
    Protected WithEvents LBXtabelle As System.Web.UI.WebControls.ListBox
    Protected WithEvents DDLCampi As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DGValoriCampo As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DGRecord As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBvaloreOld_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBvaloreOld As System.Web.UI.WebControls.Label
    Protected WithEvents LBvallingua_t As System.Web.UI.WebControls.Label
    Protected WithEvents TBValLingua As System.Web.UI.WebControls.TextBox
    Protected WithEvents BTNElimina As System.Web.UI.WebControls.Button
    Protected WithEvents BTNsuLingua As System.Web.UI.WebControls.Button
    Protected WithEvents BTNsxRecord As System.Web.UI.WebControls.Button
    Protected WithEvents BTNgiuLingua As System.Web.UI.WebControls.Button
    Protected WithEvents BTNdxRecord As System.Web.UI.WebControls.Button
    Protected WithEvents BTNConferma As System.Web.UI.WebControls.Button
    Protected WithEvents BTNAnnulla As System.Web.UI.WebControls.Button
    Protected WithEvents BTNimport As System.Web.UI.WebControls.Button
    Protected WithEvents BTNImportOk As System.Web.UI.WebControls.Button
    Protected WithEvents BTNchiudi As System.Web.UI.WebControls.Button
    Protected WithEvents LBInfo As System.Web.UI.WebControls.Label
    Protected WithEvents PNLCambioValore As System.Web.UI.WebControls.Panel
    Protected WithEvents LBlingua_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBTraduzione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBimport As System.Web.UI.WebControls.Label
    Protected WithEvents PNLimport As System.Web.UI.WebControls.Panel
    Protected WithEvents DDLlingua As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBmessaggio As System.Web.UI.WebControls.Label

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
        Dim oPersona As New COL_Persona

        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Me.SessioneScaduta() Then
            Exit Sub
        End If


        If Page.IsPostBack = False Then
            Me.SetupInternazionalizzazione()

            oPersona = Session("objPersona")

			If oPersona.TipoPersona.id = Main.TipoPersonaStandard.SysAdmin Or oPersona.TipoPersona.id = Main.TipoPersonaStandard.AdminSecondario Then
				Me.PNLcontenuto.Visible = True
				Me.PNLpermessi.Visible = False
				Me.Bind_TabelleSistema()
			Else
				Me.PNLcontenuto.Visible = False
				Me.PNLpermessi.Visible = True
			End If
        End If
    End Sub

    Private Function SessioneScaduta() As Boolean
        Dim oPersona As COL_Persona
        Dim isScaduta As Boolean = True
        Try
            oPersona = Session("objPersona")
            If oPersona.Id > 0 Then
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

#Region "Bind Dati"

    Private Sub Bind_TabelleSistema()
        Dim oDataset As DataSet

        Try
            LBXtabelle.Items.Clear()

            oDataset = oInternaz.ElencaTabelle
            LBXtabelle.DataSource = oDataset.Tables(0).DefaultView
            LBXtabelle.DataValueField = "TBSS_ID"
            LBXtabelle.DataTextField = "TBSS_Name"
            LBXtabelle.SelectedIndex = -1
            LBXtabelle.DataBind()
        Catch ex As Exception
            LBXtabelle.Items.Clear()
            LBXtabelle.Items.Add(New ListItem(oResource.getValue("LBXtabelle.-1"), "-1"))
        End Try
    End Sub
    Private Sub Bind_CampiTabella()
        Dim oDataset As New DataSet
        Try
            DDLCampi.Items.Clear()
            oInternaz.TabellaId = CInt(LBXtabelle.SelectedValue)
            oDataset = oInternaz.ElencaColonne()
            DDLCampi.DataSource = oDataset.Tables(0).DefaultView
            DDLCampi.DataValueField = "COLM_ID"
            DDLCampi.DataTextField = "COLM_Name"

            DDLCampi.DataBind()
            DDLCampi.SelectedIndex = DDLCampi.Items.Count() - 1
            TCCampi.Visible = True
            DDLCampi.Visible = True
            TCDati.Visible = False
            Bind_RecordTabella()
        Catch ex As Exception
            LBXtabelle.Items.Add(New ListItem(oResource.getValue("DDLCampi.-1"), "-1"))
            DGRecord.DataSource = Nothing
            DGRecord.DataBind()
        End Try

    End Sub

    Private Sub Bind_RecordTabella()
        Dim oDataset As New DataSet
        Try
            oDataset = oInternaz.ElencaRecord(LBXtabelle.SelectedItem.Text, DDLCampi.SelectedItem.Text)
            DGRecord.DataSource = oDataset.Tables(0).DefaultView
            DGRecord.Columns(1).HeaderText = DDLCampi.SelectedItem.Text
            DGRecord.DataBind()
            If DDLCampi.Width.Value > DGRecord.Width.Value Then
                DGRecord.Width = DDLCampi.Width
            Else
                DDLCampi.Width = DGRecord.Width
            End If
            If oDataset.Tables(0).Rows.Count > 0 Then
                BTNimport.Visible = True
                DGRecord.Visible = True
            Else
                BTNimport.Visible = False
                DGRecord.Visible = False
            End If
            TCDati.Visible = False
        Catch ex As Exception

        End Try
    End Sub

    Private Sub bindDGValoriCampo(ByVal RecordID As Integer)
        Try
            Dim oDatasetValoriCampo As New DataSet
            oInternaz.TabellaId = CInt(LBXtabelle.SelectedValue)
            oInternaz.ColonnaId = DDLCampi.SelectedValue
            oInternaz.RecordId = RecordID
            BTNConferma.CommandArgument = RecordID
            oDatasetValoriCampo = oInternaz.ElencaValori
            DGValoriCampo.DataSource = oDatasetValoriCampo.Tables(0).DefaultView
            oResource.setHeaderDatagrid(DGValoriCampo, 0, "LNGU_nome", True)
            oResource.setHeaderDatagrid(DGValoriCampo, 3, "INTR_Valore", True)
            DGValoriCampo.DataBind()
            PNLCambioValore.Visible = False
            TCDati.Visible = True
            Me.DGValoriCampo.Visible = True
            Me.PNLimport.Visible = False
            Me.LBmessaggio.Text = ""
        Catch ex As Exception

        End Try
    End Sub

	Private Sub Bind_Lingue()
		Me.DDLlingua.Items.Clear()

		
		Try
			Me.DDLlingua.DataSource = ManagerLingua.List
			Me.DDLlingua.DataTextField() = "Nome"
			Me.DDLlingua.DataValueField = "Id"
			Me.DDLlingua.DataBind()

			If Me.DDLlingua.Items.Count = 0 Then
				Me.DDLlingua.Items.Add(New ListItem("<nessuna>", -1))
				Me.DDLlingua.Enabled = False
			Else
				Me.DDLlingua.Enabled = True
			End If
		Catch ex As Exception
			Me.DDLlingua.Items.Add(New ListItem("<nessuna>", -1))
			Me.DDLlingua.Enabled = False
		End Try

	End Sub

#End Region

#Region "eventi webcontrols"

    Private Sub LBXtabelle_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LBXtabelle.SelectedIndexChanged
        DGRecord.Visible = False
        BTNimport.Visible = False
        Me.Bind_CampiTabella()
    End Sub

    Private Sub DDLCampi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLCampi.SelectedIndexChanged
        DGRecord.Visible = False
        Me.Bind_RecordTabella()
    End Sub

    Private Sub DGRecord_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGRecord.ItemCommand
        Try

            If e.CommandName = "elencaValori" Then

                If e.Item.ItemIndex = 0 Then
                    BTNsxRecord.CommandArgument = DGRecord.Items.Count - 1
                Else
                    BTNsxRecord.CommandArgument = e.Item.ItemIndex - 1
                End If

                Dim i As Integer
                For i = 0 To DGRecord.Items.Count - 1
                    DGRecord.Items(i).BackColor = DGRecord.BackColor
                Next

                e.Item.BackColor = System.Drawing.Color.LemonChiffon

                BTNsuLingua.CommandName = e.Item.ItemIndex
                BTNgiuLingua.CommandName = e.Item.ItemIndex

                If e.Item.ItemIndex + 1 > DGRecord.Items.Count - 1 Then
                    BTNdxRecord.CommandArgument = 0
                Else
                    BTNdxRecord.CommandArgument = e.Item.ItemIndex + 1
                End If

                LBvaloreOld.Text = e.Item.Cells(2).Text
                bindDGValoriCampo(e.Item.Cells(0).Text)

            End If

        Catch ex As Exception

        End Try


    End Sub

    Private Sub DGValoriCampo_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGValoriCampo.ItemCommand
        Try


            PNLCambioValore.Visible = True
            LBInfo.Text = ""
            If e.Item.ItemIndex = 0 Then
                BTNsuLingua.CommandArgument = DGValoriCampo.Items.Count - 1
            Else
                BTNsuLingua.CommandArgument = e.Item.ItemIndex - 1
            End If

            Dim i As Integer
            For i = 0 To DGValoriCampo.Items.Count - 1
                DGValoriCampo.Items(i).BackColor = DGValoriCampo.BackColor
            Next

            e.Item.BackColor = System.Drawing.Color.LemonChiffon

            BTNdxRecord.CommandName = e.Item.ItemIndex
            BTNsxRecord.CommandName = e.Item.ItemIndex

            If e.Item.ItemIndex + 1 > DGValoriCampo.Items.Count - 1 Then
                BTNgiuLingua.CommandArgument = 0
            Else
                BTNgiuLingua.CommandArgument = e.Item.ItemIndex + 1
            End If

            BTNConferma.CommandName = e.Item.Cells(2).Text
            BTNElimina.CommandArgument = e.Item.Cells(6).Text
            LBvallingua_t.Text = e.Item.Cells(5).Text
            If Not e.Item.Cells(3).Text = "&nbsp;" Then
                TBValLingua.Text = e.Item.Cells(3).Text
            Else
                TBValLingua.Text = ""
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub BTNElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNElimina.Click
        LBInfo.Text = ""
        Try
            oInternaz.Id = CInt(BTNElimina.CommandArgument)
            oInternaz.Elimina()

            If oInternaz.Errore = Errori_Db.None Then
                LBInfo.Text = oResource.getValue("Eliminazione." & Me.stringaInserimento.Eliminato)
            Else
                LBInfo.Text = oResource.getValue("Eliminazione." & Me.stringaInserimento.NonEliminato)
            End If
            bindDGValoriCampo(CInt(BTNConferma.CommandArgument))
            PNLCambioValore.Visible = True
            LBvallingua_t.Text = DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(5).Text
            DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).BackColor = System.Drawing.Color.LemonChiffon
            If Not DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(3).Text = "&nbsp;" Then
                TBValLingua.Text = DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(3).Text
            Else
                TBValLingua.Text = ""
            End If
        Catch ex As Exception
            LBInfo.Text = oResource.getValue("Eliminazione." & Me.stringaInserimento.NonEliminato)
        End Try


    End Sub


    Private Sub BTNAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNAnnulla.Click
        Try
            LBInfo.Text = ""
            bindDGValoriCampo(CInt(BTNConferma.CommandArgument))
            PNLCambioValore.Visible = True
            LBvallingua_t.Text = DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(5).Text
            DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).BackColor = System.Drawing.Color.LemonChiffon
            If Not DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(3).Text = "&nbsp;" Then
                TBValLingua.Text = DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(3).Text
            Else
                TBValLingua.Text = ""
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BTNConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNConferma.Click

        Try
            LBInfo.Text = ""
            If TBValLingua.Text.Trim <> "" Then
                oInternaz.TabellaId = CInt(LBXtabelle.SelectedValue)
                oInternaz.ColonnaId = CInt(DDLCampi.SelectedValue)
                oInternaz.RecordId = CInt(BTNConferma.CommandArgument)
                oInternaz.LinguaId = CInt(BTNConferma.CommandName)
                oInternaz.Valore = TBValLingua.Text
                oInternaz.Aggiungi()
                If oInternaz.Errore = Errori_Db.None Then
                    LBInfo.Text = oResource.getValue("Inserimento." & Me.stringaInserimento.Inserito)
                Else
                    LBInfo.Text = oResource.getValue("Inserimento." & Me.stringaInserimento.NonInserito)
                End If
                bindDGValoriCampo(oInternaz.RecordId)
                PNLCambioValore.Visible = True
                LBvallingua_t.Text = DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(5).Text
                DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).BackColor = System.Drawing.Color.LemonChiffon
                If Not DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(3).Text = "&nbsp;" Then
                    TBValLingua.Text = DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(3).Text
                Else
                    TBValLingua.Text = ""
                End If
            Else
                LBInfo.Text = oResource.getValue("Inserimento." & Me.stringaInserimento.InserireValore)
            End If

        Catch ex As Exception
            LBInfo.Text = oResource.getValue("Inserimento." & Me.stringaInserimento.NonInserito)
        End Try
    End Sub

#End Region

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_Internazionalizzazione"
        oResource.Folder_Level1 = "Admin_globale"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(Me.LBNopermessi)
            .setLabel(Me.LBvaloreOld_t)
            .setButton(Me.BTNConferma, True, False, , True)
            .setButton(Me.BTNAnnulla, True, False, , True)
            .setButton(Me.BTNElimina, True, False, True, True)
            .setButton(Me.BTNdxRecord, True, False, , True)
            .setButton(Me.BTNsxRecord, True, False, , True)
            .setButton(Me.BTNsuLingua, True, False, , True)
            .setButton(Me.BTNgiuLingua, True, False, , True)
            .setLabel(LBlingua_t)
            .setLabel(LBTraduzione_t)
            .setButton(BTNimport)
            .setButton(BTNImportOk)
            .setLabel(LBimport)
            .setLabel(LBmessaggio)
            .setButton(BTNchiudi)

        End With
    End Sub

#End Region

#Region "Pulsanti spostamento"

    Private Sub BTNdxRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNdxRecord.Click
        Try

            LBInfo.Text = ""
            Dim i As Integer
            For i = 0 To DGRecord.Items.Count - 1
                DGRecord.Items(i).BackColor = DGRecord.BackColor
            Next

            DGRecord.Items(CInt(BTNdxRecord.CommandArgument)).BackColor = System.Drawing.Color.LemonChiffon

            BTNsuLingua.CommandName = CInt(BTNdxRecord.CommandArgument)
            BTNgiuLingua.CommandName = CInt(BTNdxRecord.CommandArgument)

            bindDGValoriCampo(DGRecord.Items(CInt(BTNdxRecord.CommandArgument)).Cells(0).Text)
            LBvaloreOld.Text = DGRecord.Items(CInt(BTNdxRecord.CommandArgument)).Cells(2).Text
            DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).BackColor = System.Drawing.Color.LemonChiffon
            PNLCambioValore.Visible = True

            BTNConferma.CommandName = DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(2).Text
            BTNElimina.CommandArgument = DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(6).Text
            LBvallingua_t.Text = DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(5).Text
            If Not DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(3).Text = "&nbsp;" Then
                TBValLingua.Text = DGValoriCampo.Items(CInt(BTNdxRecord.CommandName)).Cells(3).Text
            Else
                TBValLingua.Text = ""
            End If

            If BTNsxRecord.CommandArgument + 1 > DGRecord.Items.Count - 1 Then
                BTNsxRecord.CommandArgument = 0
            Else
                BTNsxRecord.CommandArgument = CInt(BTNsxRecord.CommandArgument) + 1
            End If

            If BTNdxRecord.CommandArgument + 1 > DGRecord.Items.Count - 1 Then
                BTNdxRecord.CommandArgument = 0
            Else
                BTNdxRecord.CommandArgument = CInt(BTNdxRecord.CommandArgument) + 1
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub BTNsxRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsxRecord.Click
        Try

            LBInfo.Text = ""
            Dim i As Integer
            For i = 0 To DGRecord.Items.Count - 1
                DGRecord.Items(i).BackColor = DGRecord.BackColor
            Next

            DGRecord.Items(CInt(BTNsxRecord.CommandArgument)).BackColor = System.Drawing.Color.LemonChiffon

            BTNsuLingua.CommandName = CInt(BTNsxRecord.CommandArgument)
            BTNgiuLingua.CommandName = CInt(BTNsxRecord.CommandArgument)

            bindDGValoriCampo(DGRecord.Items(CInt(BTNsxRecord.CommandArgument)).Cells(0).Text)

            LBvaloreOld.Text = DGRecord.Items(CInt(BTNsxRecord.CommandArgument)).Cells(2).Text
            PNLCambioValore.Visible = True
            DGValoriCampo.Items(CInt(BTNsxRecord.CommandName)).BackColor = System.Drawing.Color.LemonChiffon
            BTNConferma.CommandName = DGValoriCampo.Items(CInt(BTNsxRecord.CommandName)).Cells(2).Text
            BTNElimina.CommandArgument = DGValoriCampo.Items(CInt(BTNsxRecord.CommandName)).Cells(6).Text
            LBvallingua_t.Text = DGValoriCampo.Items(CInt(BTNsxRecord.CommandName)).Cells(5).Text
            If Not DGValoriCampo.Items(CInt(BTNsxRecord.CommandName)).Cells(3).Text = "&nbsp;" Then
                TBValLingua.Text = DGValoriCampo.Items(CInt(BTNsxRecord.CommandName)).Cells(3).Text
            Else
                TBValLingua.Text = ""
            End If

            If BTNdxRecord.CommandArgument - 1 < 0 Then
                BTNdxRecord.CommandArgument = DGRecord.Items.Count - 1
            Else
                BTNdxRecord.CommandArgument = CInt(BTNdxRecord.CommandArgument) - 1
            End If

            If BTNsxRecord.CommandArgument - 1 < 0 Then
                BTNsxRecord.CommandArgument = DGRecord.Items.Count - 1
            Else
                BTNsxRecord.CommandArgument = CInt(BTNsxRecord.CommandArgument) - 1
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BTNgiuLingua_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNgiuLingua.Click
        Try

            LBInfo.Text = ""
            Dim i As Integer
            For i = 0 To DGValoriCampo.Items.Count - 1
                DGValoriCampo.Items(i).BackColor = DGValoriCampo.BackColor
            Next

            DGRecord.Items(CInt(BTNgiuLingua.CommandName)).BackColor = System.Drawing.Color.LemonChiffon

            BTNdxRecord.CommandName = CInt(BTNgiuLingua.CommandArgument)
            BTNsxRecord.CommandName = CInt(BTNgiuLingua.CommandArgument)

            bindDGValoriCampo(DGRecord.Items(CInt(BTNgiuLingua.CommandName)).Cells(0).Text)
            LBvaloreOld.Text = DGRecord.Items(CInt(BTNgiuLingua.CommandName)).Cells(2).Text

            DGValoriCampo.Items(CInt(BTNgiuLingua.CommandArgument)).BackColor = System.Drawing.Color.LemonChiffon

            PNLCambioValore.Visible = True

            BTNConferma.CommandName = DGValoriCampo.Items(CInt(BTNgiuLingua.CommandArgument)).Cells(2).Text
            BTNElimina.CommandArgument = DGValoriCampo.Items(CInt(BTNgiuLingua.CommandArgument)).Cells(6).Text
            LBvallingua_t.Text = DGValoriCampo.Items(CInt(BTNgiuLingua.CommandArgument)).Cells(5).Text
            If Not DGValoriCampo.Items(CInt(BTNgiuLingua.CommandArgument)).Cells(3).Text = "&nbsp;" Then
                TBValLingua.Text = DGValoriCampo.Items(CInt(BTNgiuLingua.CommandArgument)).Cells(3).Text
            Else
                TBValLingua.Text = ""
            End If

            If BTNsuLingua.CommandArgument + 1 > DGValoriCampo.Items.Count - 1 Then
                BTNsuLingua.CommandArgument = 0
            Else
                BTNsuLingua.CommandArgument = CInt(BTNsuLingua.CommandArgument) + 1
            End If

            If BTNgiuLingua.CommandArgument + 1 > DGValoriCampo.Items.Count - 1 Then
                BTNgiuLingua.CommandArgument = 0
            Else
                BTNgiuLingua.CommandArgument = CInt(BTNgiuLingua.CommandArgument) + 1
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BTNsuLingua_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsuLingua.Click
        Try

            LBInfo.Text = ""
            Dim i As Integer
            For i = 0 To DGValoriCampo.Items.Count - 1
                DGValoriCampo.Items(i).BackColor = DGValoriCampo.BackColor
            Next

            DGRecord.Items(CInt(BTNsuLingua.CommandName)).BackColor = System.Drawing.Color.LemonChiffon

            BTNdxRecord.CommandName = CInt(BTNsuLingua.CommandName)
            BTNsxRecord.CommandName = CInt(BTNsuLingua.CommandName)


            bindDGValoriCampo(DGRecord.Items(CInt(BTNsuLingua.CommandName)).Cells(0).Text)

            LBvaloreOld.Text = DGRecord.Items(CInt(BTNsuLingua.CommandName)).Cells(2).Text
            PNLCambioValore.Visible = True

            DGValoriCampo.Items(CInt(BTNsuLingua.CommandArgument)).BackColor = System.Drawing.Color.LemonChiffon

            BTNConferma.CommandName = DGValoriCampo.Items(CInt(BTNsuLingua.CommandArgument)).Cells(2).Text
            BTNElimina.CommandArgument = DGValoriCampo.Items(CInt(BTNsuLingua.CommandArgument)).Cells(6).Text
            LBvallingua_t.Text = DGValoriCampo.Items(CInt(BTNsuLingua.CommandArgument)).Cells(5).Text
            If Not DGValoriCampo.Items(CInt(BTNsuLingua.CommandArgument)).Cells(3).Text = "&nbsp;" Then
                TBValLingua.Text = DGValoriCampo.Items(CInt(BTNsuLingua.CommandArgument)).Cells(3).Text
            Else
                TBValLingua.Text = ""
            End If

            If BTNgiuLingua.CommandArgument - 1 < 0 Then
                BTNgiuLingua.CommandArgument = DGValoriCampo.Items.Count - 1
            Else
                BTNgiuLingua.CommandArgument = CInt(BTNgiuLingua.CommandArgument) - 1
            End If

            If BTNsuLingua.CommandArgument - 1 < 0 Then
                BTNsuLingua.CommandArgument = DGValoriCampo.Items.Count - 1
            Else
                BTNsuLingua.CommandArgument = CInt(BTNsuLingua.CommandArgument) - 1
            End If
        Catch ex As Exception

        End Try
    End Sub


#End Region

    Private Sub BTNimport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNimport.Click
        DGValoriCampo.Visible = False
        Me.BTNchiudi.Visible = True
        Me.BTNImportOk.Visible = True
        Me.PNLCambioValore.Visible = False
        Me.TCDati.Visible = True
        Me.PNLimport.Visible = True
        Me.Bind_Lingue()
        LBmessaggio.Text = ""

    End Sub

    Private Sub BTNImportOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNImportOk.Click
        Dim i As Integer
        Try
            LBInfo.Text = ""

            oInternaz.TabellaId = CInt(LBXtabelle.SelectedValue)
            oInternaz.ColonnaId = CInt(DDLCampi.SelectedValue)

            For i = 0 To Me.DGRecord.Items.Count - 1
                oInternaz.RecordId = DGRecord.Items(i).Cells(0).Text
                oInternaz.LinguaId = Me.DDLlingua.SelectedValue
                oInternaz.Valore = DGRecord.Items(i).Cells(2).Text
                oInternaz.Aggiungi()

                If oInternaz.Errore = Errori_Db.None Then
                    LBmessaggio.Text = LBmessaggio.Text & "<br> Record " & DGRecord.Items(i).Cells(0).Text & " " & oResource.getValue("Inserimento." & Me.stringaInserimento.Inserito)
                Else
                    LBmessaggio.Text = LBmessaggio.Text & "<br> Record " & DGRecord.Items(i).Cells(0).Text & " " & oResource.getValue("Inserimento." & Me.stringaInserimento.NonInserito)
                End If
            Next
            Me.BTNchiudi.Visible = True
            Me.BTNImportOk.Visible = False
        Catch ex As Exception
            LBmessaggio.Text = LBmessaggio.Text & "<br> Il Record " & DGRecord.Items(i).Cells(0).Text & oResource.getValue(" ha bloccato Inserimento." & Me.stringaInserimento.NonInserito)
            Me.BTNchiudi.Visible = True
            Me.BTNImportOk.Visible = False
        End Try
    End Sub

    Private Sub BTNchiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNchiudi.Click
        Me.PNLimport.Visible = False
        Me.BTNchiudi.Visible = True
        Me.BTNImportOk.Visible = True
    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property
End Class
