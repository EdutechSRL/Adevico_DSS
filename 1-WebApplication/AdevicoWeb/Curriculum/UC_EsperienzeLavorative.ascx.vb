Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Curriculum_Europeo


Imports iTextSharp

Public Class UC_EsperienzeLavorative
    Inherits System.Web.UI.UserControl
    Protected oResourceEsperienze As ResourceManager


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


#Region "accessori"
    Protected WithEvents HDNprsn_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcreu_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNeslv_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents PNLinserimento As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLesperienze As System.Web.UI.WebControls.Panel
#End Region
#Region "lista"

    Protected WithEvents RPTesperienze As System.Web.UI.WebControls.Repeater
    'Protected WithEvents BTNinserisci As System.Web.UI.WebControls.Button
    Protected WithEvents TBRsettore As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRtipoImpiego As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRmansione As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBinizio_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBfine_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBMeseInizio As System.Web.UI.WebControls.Label
    Protected WithEvents LBAnnoInizio As System.Web.UI.WebControls.Label
    Protected WithEvents LBMeseFine As System.Web.UI.WebControls.Label
    Protected WithEvents LBAnnoFine As System.Web.UI.WebControls.Label
    Protected WithEvents LBnome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBsettore_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoImpiego_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmansione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBrendiPubblico_t As System.Web.UI.WebControls.Label
#End Region

#Region "aggiungi"
	Protected WithEvents LBdataI As System.Web.UI.WebControls.Label
	Protected WithEvents LBdataF As System.Web.UI.WebControls.Label
	Protected WithEvents HDNdataF As System.Web.UI.HtmlControls.HtmlInputHidden
	Protected WithEvents HDNdataI As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TXBnome As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBsettore As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBtipoImpiego As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBmansione As System.Web.UI.WebControls.TextBox
    Protected WithEvents CBXrendiPubblico As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBXinCorso As System.Web.UI.WebControls.CheckBox
#End Region
    Protected WithEvents IMBapriFine As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LBLErrData As System.Web.UI.WebControls.Label


    Public Property CurriculumID() As Integer
        Get
            If Me.HDNcreu_id.Value = "" Then
                Me.HDNcreu_id.Value = 0
            End If
            CurriculumID = Me.HDNcreu_id.Value
        End Get
        Set(ByVal Value As Integer)
            Me.HDNcreu_id.Value = Value
        End Set
    End Property
    Public Property PRSN_ID() As Integer
        Get
            If Me.HDNprsn_id.Value = "" Then
                Me.HDNprsn_id.Value = 0
            End If
            PRSN_ID = Me.HDNprsn_id.Value
        End Get
        Set(ByVal Value As Integer)
            Me.HDNprsn_id.Value = Value
        End Set
    End Property
    Public Event MostraModifica(ByVal sender As Object, ByVal e As EventArgs)
    Public Event MostraAggiungi(ByVal sender As Object, ByVal e As EventArgs)

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResourceEsperienze) Then
            SetCulture(Session("LinguaCode"))
        End If
        Try
            If Page.IsPostBack = False Then
                Me.SetupInternazionalizzazione()
				Me.HDNdataI.Value = Format(CDate(Date.Now), "d/M/yyyy")
				Me.HDNdataF.Value = Format(CDate(Date.Now), "d/M/yyyy")

                Dim PRSN_id As Integer
                PRSN_id = Me.HDNprsn_id.Value
                Dim oEsperienzeLavorative As New COL_EsperienzeLavorative
                oEsperienzeLavorative.EstraiByPersona(PRSN_id)

                If oEsperienzeLavorative.ID = -1 Then
                    PNLinserimento.Visible = True
                    PNLesperienze.Visible = False
                    Viewstate("startEsperienze") = "si"
                Else
                    PNLinserimento.Visible = False
                    PNLesperienze.Visible = True
                    Bind_Dati()
				End If
			End If
        Catch ex As Exception

		End Try
		Me.LBdataI.Text = Me.HDNdataI.Value
		Me.LBdataF.Text = Me.HDNdataF.Value
    End Sub
    Public Sub Setup_Controllo()
        If IsNothing(oResourceEsperienze) Then
            SetCulture(Session("LinguaCode"))
        End If
        If Viewstate("startEsperienze") <> "si" Then
            Me.PNLinserimento.Visible = False
            Me.PNLesperienze.Visible = True
        Else
            RaiseEvent MostraAggiungi(Me, EventArgs.Empty)
        End If

        Me.PulisciCampi()
    End Sub
#Region "Bind_Dati"
    Private Sub Bind_Dati()
        Dim PRSN_id As Integer
        Dim oDataset As DataSet
        Dim i, totale As Integer
        Dim oEsperienzeLavorative As New COL_EsperienzeLavorative
        Try
            PRSN_id = Me.HDNprsn_id.Value
            oDataset = oEsperienzeLavorative.ElencaByPersona(PRSN_id)
            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                Me.PNLinserimento.Visible = True
                Me.PNLesperienze.Visible = False
                RaiseEvent MostraAggiungi(Me, EventArgs.Empty)
            Else
                Viewstate("startEsperienze") = ""
                oDataset.Tables(0).Columns.Add(New DataColumn("oInizio"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oFine"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oRendiPubblico"))
                For i = 0 To totale - 1
                    Try
                        Dim oRow As DataRow
                        oRow = oDataset.Tables(0).Rows(i)
                        If Not IsDBNull(oRow.Item("ESLV_inizio")) Then
                            oRow.Item("oInizio") = Format(CDate(oRow.Item("ESLV_inizio")), "d/M/yyyy")
                        Else
                            oRow.Item("oInizio") = oResourceEsperienze.getValue("null") '"Null" 'oResource.
                        End If

                        'If Not IsDBNull(oRow.Item("ESLV_fine")) Then
                        '    oRow.Item("oFine") = Format(CDate(oRow.Item("ESLV_fine")), "d/M/yyyy")
                        'Else
                        '    oRow.Item("oFine") = "Null" 'oResource.
                        'End If
                        If oRow.Item("ESLV_esperienzaInCorso") = "1" Then
                            oRow.Item("oFine") = "Esperienza in Corso" 'oResource.
                        Else
                            If Not IsDBNull(oRow.Item("ESLV_fine")) Then
                                oRow.Item("oFine") = Format(CDate(oRow.Item("ESLV_fine")), "d/M/yyyy")
                            Else
                                oRow.Item("oFine") = oResourceEsperienze.getValue("null") '"Null" 'oResource.
                            End If
                        End If

                        If oRow.Item("ESLV_rendiPubblico") = "1" Then
                            oRow.Item("oRendiPubblico") = oResourceEsperienze.getValue("si") '"Si" 'oResource.
                        Else
                            oRow.Item("oRendiPubblico") = oResourceEsperienze.getValue("no") '"No" 'oResource.
                        End If


                        If Not IsDBNull(oRow.Item("ESLV_nomeDatore")) Then
                            oRow.Item("ESLV_nomeDatore") = Replace(oRow.Item("ESLV_nomeDatore"), vbCrLf, "<br>")
                        End If
                        If Not IsDBNull(oRow.Item("ESLV_settore")) Then
                            oRow.Item("ESLV_settore") = Replace(oRow.Item("ESLV_settore"), vbCrLf, "<br>")
                        End If
                        If Not IsDBNull(oRow.Item("ESLV_tipoImpiego")) Then
                            oRow.Item("ESLV_tipoImpiego") = Replace(oRow.Item("ESLV_tipoImpiego"), vbCrLf, "<br>")
                        End If
                        If Not IsDBNull(oRow.Item("ESLV_mansione")) Then
                            oRow.Item("ESLV_mansione") = Replace(oRow.Item("ESLV_mansione"), vbCrLf, "<br>")
                        End If


                    Catch ax As Exception

                    End Try


                Next
                RPTesperienze.DataSource = oDataset
                RPTesperienze.DataBind()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub BindModifica()
        Dim oEsperienzeLavorative As New COL_EsperienzeLavorative
        Try
            oEsperienzeLavorative.ID = HDNeslv_id.Value

            oEsperienzeLavorative.Estrai()
			Me.HDNdataI.Value = Format(CDate(oEsperienzeLavorative.inizio), "d/M/yyyy")
            If oEsperienzeLavorative.inizio > oEsperienzeLavorative.fine Then
				Me.HDNdataF.Value = Format(CDate(oEsperienzeLavorative.inizio), "d/M/yyyy")
            Else
				Me.HDNdataF.Value = Format(CDate(oEsperienzeLavorative.fine), "d/M/yyyy")
            End If
			Me.LBdataF.Text = Me.HDNdataF.Value
			Me.LBdataI.Text = Me.HDNdataI.Value

            Me.TXBnome.Text = oEsperienzeLavorative.nomeDatore
            Me.TXBsettore.Text = oEsperienzeLavorative.settore
            Me.TXBtipoImpiego.Text = oEsperienzeLavorative.tipoImpiego
            Me.TXBmansione.Text = oEsperienzeLavorative.mansione
            Me.CBXrendiPubblico.Checked = oEsperienzeLavorative.RendiPubblico
            Me.CBXinCorso.Checked = oEsperienzeLavorative.EsperienzaInCorso
            'oFormazione.Curriculum_id = Me.HDNcreu_id.Value
            If Me.CBXinCorso.Checked = True Then
				Me.IMBapriFine.Enabled = False
				Me.LBdataF.Enabled = False
			Else
				Me.IMBapriFine.Enabled = True
				Me.LBdataF.Enabled = True
			End If
        Catch ex As Exception

        End Try

    End Sub
#End Region

#Region "gestione repeater"
    Private Sub RPTesperienze_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTesperienze.ItemCreated
        If IsNothing(oResourceEsperienze) Then
            SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Try
                Try
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = e.Item.FindControl("LKBmodifica")
                    If IsNothing(oLinkbutton) = False Then
                        oResourceEsperienze.setLinkButton(oLinkbutton, True, True, False, False)
                    End If
                Catch ax As Exception

                End Try
                Try
                    Dim oLinkbuttonE As LinkButton
                    oLinkbuttonE = e.Item.FindControl("LKBelimina")
                    If IsNothing(oLinkbuttonE) = False Then
                        oResourceEsperienze.setLinkButton(oLinkbuttonE, True, True, False, True)
                    End If
                Catch ax As Exception

                End Try

                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBinizio_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBinizio_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBfine_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBfine_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBnome_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBnome_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBsettore_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBsettore_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBtipoImpiego_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBtipoImpiego_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBmansione_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBmansione_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBrendiPubblico_s")
                    If IsNothing(olabel) = False Then
                        oResourceEsperienze.setLabel_To_Value(olabel, "LBrendiPubblico_s.text")
                    End If
                Catch ex As Exception

                End Try

            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub RPTesperienze_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTesperienze.ItemCommand
        Dim ESLV_id As Integer
        Select Case e.CommandName
            Case "modifica"
                ESLV_id = e.CommandArgument
                Me.PNLinserimento.Visible = True
                Me.PNLesperienze.Visible = False
                ' Me.BTNmodifica.Visible = True
                ' Me.BTNaggiungi.Visible = False
                RaiseEvent MostraModifica(Me, EventArgs.Empty)
                HDNeslv_id.Value = ESLV_id
                Me.BindModifica()
            Case "elimina"
                Try
                    ESLV_id = e.CommandArgument
                    Dim oEsperienzeLavorative As New COL_EsperienzeLavorative
                    oEsperienzeLavorative.Elimina(ESLV_id)
                    Me.Bind_Dati()
                Catch ex As Exception

                End Try



        End Select




    End Sub
#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceEsperienze = New ResourceManager

        oResourceEsperienze.UserLanguages = Code
        oResourceEsperienze.ResourcesName = "pg_UC_EsperienzeLavorative"
        oResourceEsperienze.Folder_Level1 = "Curriculum"
        oResourceEsperienze.Folder_Level2 = "UC"
        oResourceEsperienze.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceEsperienze
            .setLabel(LBinizio_t)
            .setLabel(LBfine_t)
            .setCheckBox(CBXinCorso)
            .setLabel(LBMeseInizio)
            .setLabel(LBAnnoInizio)
            .setLabel(LBMeseFine)
            .setLabel(LBAnnoFine)
            .setLabel(LBnome_t)
            .setLabel(LBsettore_t)
            .setLabel(LBtipoImpiego_t)
            .setLabel(LBmansione_t)
            .setLabel(LBrendiPubblico_t)
        End With
    End Sub

#End Region


#Region "aggiungi/modifica"
    Public Sub AggiungiEsperienza()
        Dim oEsperienzeLavorative As New COL_EsperienzeLavorative
        Try
			If CDate(Me.HDNdataI.Value) > CDate(Me.HDNdataF.Value) And Me.CBXinCorso.Checked = False Then
				' Me.LBLErrData.Text = "La data di fine è precedente la data di inizio"
				oResourceEsperienze.setLabel_To_Value(LBLErrData, "errore")
			Else
				Me.LBLErrData.Text = ""
				With oEsperienzeLavorative
					.inizio = Me.HDNdataI.Value
					.fine = Me.HDNdataF.Value
					.nomeDatore = Me.TXBnome.Text
					.settore = Me.TXBsettore.Text
					.tipoImpiego = Me.TXBtipoImpiego.Text
					.mansione = Me.TXBmansione.Text
					.RendiPubblico = Me.CBXrendiPubblico.Checked
					.Curriculum_id = Me.HDNcreu_id.Value
					.EsperienzaInCorso = Me.CBXinCorso.Checked
				End With
				If Me.HDNcreu_id.Value <> "" Then
					oEsperienzeLavorative.Aggiungi()

					Me.PNLinserimento.Visible = False
					Me.PNLesperienze.Visible = True
					Me.Bind_Dati()
					PulisciCampi()
				End If
			End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub InserisciEsperienza()
        Me.PNLesperienze.Visible = False
        Me.PNLinserimento.Visible = True
        Me.PulisciCampi()
    End Sub

    Public Sub AnnullaEsperienza()
        Me.PNLesperienze.Visible = True
        Me.PNLinserimento.Visible = False
        Me.Bind_Dati()
    End Sub
    Private Sub PulisciCampi()
		Me.HDNdataI.Value = Format(CDate(Date.Now), "d/M/yyyy")
		Me.HDNdataF.Value = Format(CDate(Date.Now), "d/M/yyyy")
		Me.LBdataF.Text = Me.HDNdataF.Value
		Me.LBdataI.Text = Me.LBdataF.Text
        Me.TXBnome.Text = ""
        Me.TXBsettore.Text = ""
        Me.TXBtipoImpiego.Text = ""
        Me.TXBmansione.Text = ""
        Me.CBXrendiPubblico.Checked = True
        '   Me.BTNaggiungi.Visible = True
        ' Me.BTNmodifica.Visible = False
        Me.LBLErrData.Text = ""

        Me.CBXinCorso.Checked = True

        Me.IMBapriFine.Enabled = False
		Me.LBdataF.Enabled = False
    End Sub

    Public Sub ModificaEsperienza()

        Dim oEsperienzeLavorative As New COL_EsperienzeLavorative
        Try
			If CDate(Me.HDNdataI.Value) > CDate(Me.HDNdataF.Value) And Me.CBXinCorso.Checked = False Then
				Me.HDNdataF.Value = Me.HDNdataI.Value
				Me.LBdataF.Text = Me.HDNdataI.Value
			End If

            ' Me.LBLErrData.Text = "La data di fine è precedente la data di inizio"
            'oResourceEsperienze.setLabel_To_Value(LBLErrData, "errore")
            'Else
            With oEsperienzeLavorative
				.inizio = Me.HDNdataI.Value
				.fine = Me.HDNdataF.Value
                .nomeDatore = Me.TXBnome.Text
                .settore = Me.TXBsettore.Text
                .tipoImpiego = Me.TXBtipoImpiego.Text
                .mansione = Me.TXBmansione.Text
                .RendiPubblico = Me.CBXrendiPubblico.Checked
                .Curriculum_id = Me.HDNcreu_id.Value
                .ID = HDNeslv_id.Value
                .EsperienzaInCorso = Me.CBXinCorso.Checked
            End With
            If Me.HDNeslv_id.Value <> "" Then
                oEsperienzeLavorative.Modifica()

                Me.PNLinserimento.Visible = False
                Me.PNLesperienze.Visible = True
                Me.Bind_Dati()
                PulisciCampi()
            End If
            'End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Private Sub RPTesperienze_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTesperienze.ItemDataBound
        ' Dim oCell As New TableCell
        ' Dim oWebControl As WebControl
        '  oCell = CType(e.Item.Cell(0), TableCell)

        Dim oTableRow As New TableRow
        Try

            oTableRow = e.Item.FindControl("TBRsettore")
            If IsDBNull(e.Item.DataItem("ESLV_settore")) Then
                oTableRow.Visible = False
            ElseIf e.Item.DataItem("ESLV_settore") = "" Then
                oTableRow.Visible = False
            Else
                oTableRow.Visible = True
            End If
        Catch ex As Exception

        End Try


        Dim oTableRow2 As New TableRow
        Try

            oTableRow2 = e.Item.FindControl("TBRtipoImpiego")
            If IsDBNull(e.Item.DataItem("ESLV_tipoImpiego")) Then
                oTableRow2.Visible = False
            ElseIf e.Item.DataItem("ESLV_tipoImpiego") = "" Then
                oTableRow2.Visible = False
            Else
                oTableRow2.Visible = True

            End If
        Catch ex As Exception

        End Try

        Dim oTableRow3 As New TableRow
        Try

            oTableRow3 = e.Item.FindControl("TBRmansione")
            If IsDBNull(e.Item.DataItem("ESLV_mansione")) Then
                oTableRow3.Visible = False
            ElseIf e.Item.DataItem("ESLV_mansione") = "" Then
                oTableRow3.Visible = False
            Else
                oTableRow3.Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CBXinCorso_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXinCorso.CheckedChanged
		Me.IMBapriFine.Enabled = Not Me.CBXinCorso.Checked
		Me.LBdataF.Enabled = Not Me.CBXinCorso.Checked
    End Sub

 
End Class

