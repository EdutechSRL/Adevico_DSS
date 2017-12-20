Public Partial Class PermissionDetails
	Inherits PageBasePopUp
	Implements IviewPermissionDetails

	Private _presenter As PresenterPermissionDetails

	Public ReadOnly Property Presenter() As PresenterPermissionDetails
		Get
			If IsNothing(_presenter) Then
				_presenter = New PresenterPermissionDetails(Me)
			End If
			Presenter = _presenter
		End Get
	End Property
	Public ReadOnly Property ComunitaID() As Integer Implements IviewPermissionDetails.ComunitaID
		Get
            If Me.isModalitaAmministrazione Then
                Return Me.AmministrazioneComunitaID
            Else
                Return Me.ComunitaCorrenteID
            End If
		End Get
	End Property
	Public ReadOnly Property ServizioID() As Integer Implements IviewPermissionDetails.ServizioID
		Get
			Try
				ServizioID = CInt(EncryptedQueryString("ServizioID", SecretKeyUtil.EncType.Altro))
			Catch ex As Exception
				ServizioID = 0
			End Try
		End Get
	End Property
	Public ReadOnly Property RuoloID() As Integer Implements IviewPermissionDetails.RuoloID
		Get
			Try
				RuoloID = CInt(EncryptedQueryString("RuoloID", SecretKeyUtil.EncType.Altro))
			Catch ex As Exception
				RuoloID = -1
			End Try
		End Get
	End Property


	Public ReadOnly Property LinguaCorrenteID() As Integer Implements IviewPermissionDetails.LinguaCorrenteID
		Get
			Return Me.LinguaID
		End Get
	End Property
	Public Overrides Sub BindDati()
		If Page.IsPostBack = False Then
			Me.Presenter.Init()
		End If
	End Sub

	Public Overrides Sub BindNoPermessi()
		Me.MLVdati.SetActiveView(Me.VIWpermessi)
	End Sub
	Public Overrides Function HasPermessi() As Boolean
		Return True
	End Function
	Public Overrides Sub SetCultureSettings()
		MyBase.SetCulture("pg_ManagementServizi", "Comunita")
	End Sub
	Public Overrides Sub SetInternazionalizzazione()
		With MyBase.Resource
			.setLabel(Me.LBnoServizio)
			.setLabel(Me.LBNopermessi)
			.setButton(Me.BTNchiudi, True, , , True)
			Me.LBtitoloDettagli.Text = .getValue("TitoloDettagli")
			Me.BTNchiudi.Attributes.Add("onclick", "Javascript:window.close();return false;")
		End With
	End Sub

	Public Overrides ReadOnly Property AutoCloseWindow() As Boolean
		Get
			Return True
		End Get
	End Property

	Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
		Get
			Return False
		End Get
	End Property

	Public Sub PopulateDetails(ByVal Lista As System.Collections.Generic.List(Of COL_BusinessLogic_v2.CL_permessi.RuoloServizio)) Implements IviewPermissionDetails.PopulateDetails
		Me.MLVdati.SetActiveView(Me.VIWimpostazioni)
		Me.TBLpermessiRuoli.Rows.Clear()
		Dim oTableCell As New TableCell
		Dim oTBrow As New TableRow

		oTableCell.Text = Me.Resource.getValue("RPT_ruolo")
		oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(120)
		oTableCell.HorizontalAlign = HorizontalAlign.Center
		oTableCell.CssClass = "TBLpermessi_Header"
		oTBrow.Cells.Add(oTableCell)

		oTableCell = New TableCell
		oTableCell.Text = Me.Resource.getValue("RPT_permessi")
		oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(460)
		oTableCell.HorizontalAlign = HorizontalAlign.Center
		oTableCell.CssClass = "TBLpermessi_Header"
		oTBrow.Cells.Add(oTableCell)
		Me.TBLpermessiRuoli.Rows.Add(oTBrow)

Dim ElencoSenzaPermessi As String = ""

		For Each oRuoloServizio As COL_BusinessLogic_v2.CL_permessi.RuoloServizio In Lista
			Dim ClasseCSS As String
			oTBrow = New TableRow

			ClasseCSS = "TBLpermessi_RowItem"

			' inserisco il nome del ruolo
			oTableCell = New TableCell
			oTableCell.Text = oRuoloServizio.Ruolo.Nome
			oTableCell.Wrap = False
			oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(120)
			oTableCell.HorizontalAlign = HorizontalAlign.Left
			oTableCell.CssClass = ClasseCSS
			oTBrow.Cells.Add(oTableCell)


			' inserisco Colonna Permessi
			oTableCell = New TableCell
			oTableCell.HorizontalAlign = HorizontalAlign.Left
			oTableCell.CssClass = ClasseCSS
			oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(460)

			Dim ElencoPermessi As String = ""
			For Each oPermesso As Permessi In oRuoloServizio.PermessiAssociati(RuoloServizio.Show.ValueDefault)
				If ElencoPermessi = "" Then
					ElencoPermessi = oPermesso.Nome
				Else
					ElencoPermessi &= ", " & oPermesso.Nome
				End If
			Next

			If ElencoPermessi = "" Then
				If ElencoSenzaPermessi = "" Then
					ElencoSenzaPermessi = oRuoloServizio.Ruolo.Nome
				Else
					ElencoSenzaPermessi &= ", " & oRuoloServizio.Ruolo.Nome
				End If
			Else
				oTableCell.Text = ElencoPermessi
				oTBrow.Cells.Add(oTableCell)

				Me.TBLpermessiRuoli.Rows.Add(oTBrow)
			End If
		Next
		If ElencoSenzaPermessi <> "" Then
			oTBrow = New TableRow
			oTableCell = New TableCell
			oTableCell.Text = Me.Resource.getValue("senzaPermessi")
			oTableCell.CssClass = "TBLpermessi_RowItem"
			oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(120)
			oTBrow.Cells.Add(oTableCell)
			oTableCell = New TableCell
			oTableCell.Text = ElencoSenzaPermessi
			oTableCell.CssClass = "TBLpermessi_RowItem"
			oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(460)
			oTBrow.Cells.Add(oTableCell)
			Me.TBLpermessiRuoli.Rows.Add(oTBrow)
		End If
	End Sub

	Public Sub ShowNoServizio() Implements IviewPermissionDetails.ShowNoServizio
		Me.LBtitoloDettagli.Text = Me.Resource.getValue("TitoloDettagli")
		Me.MLVdati.SetActiveView(Me.VIWnoservizio)
	End Sub

	Public Sub ChangeTitoloDettaglio(ByVal Testo As String) Implements IviewPermissionDetails.ChangeTitoloDettaglio
		If Testo <> "" Then
			Me.Resource.setLabel(Me.LBtitoloDettagli)
			Me.LBtitoloDettagli.Text = Replace(Me.LBtitoloDettagli.Text, "#servizio#", Testo)
		Else
			Me.LBtitoloDettagli.Text = Me.Resource.getValue("TitoloDettagli")
		End If
	End Sub

End Class


'					Try

'						
'						For i = 0 To totale
'							
'						Next
'						
'					Catch ex As Exception

'					End Try
'					Me.LBnoServizio.Visible = False
'					Me.PNLimpostazioni.Visible = True
'					Me.PNLpermessi.Visible = False
'				Else
'					Me.LBtitoloDettagli.Text = Me.oResource.getValue("TitoloDettagli")
'					Me.LBnoServizio.Visible = True
'					Me.PNLimpostazioni.Visible = False
'					Me.PNLpermessi.Visible = False
'				End If
'			Catch ex As Exception

'				Me.LBnoServizio.Visible = True
'				Me.PNLimpostazioni.Visible = False
'				Me.PNLpermessi.Visible = False
'			End Try
'		Else
'			Me.LBtitoloDettagli.Text = Me.oResource.getValue("TitoloDettagli")
'			Me.LBnoServizio.Visible = True
'			Me.PNLimpostazioni.Visible = False
'			Me.PNLpermessi.Visible = False
'		End If
'	End Sub
'End Class