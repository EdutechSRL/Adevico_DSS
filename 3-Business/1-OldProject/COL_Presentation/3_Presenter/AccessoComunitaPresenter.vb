Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports System.Collections.Generic

Public Class AccessoComunitaPresenter
	Inherits GenericPresenter
	Implements IviewAccessoComunitaPresenter


	Private Shadows ReadOnly Property View() As IviewEntrataComunita Implements IviewAccessoComunitaPresenter.View
		Get
			View = MyBase.view
		End Get
	End Property

	Public Sub New(ByVal view As IviewBase)
		MyBase.view = view
	End Sub

	Public Sub Initialize() Implements IviewAccessoComunitaPresenter.Initialize
		Me.CaricaStatusComunita()
		Me.CaricaTipoComunita()
		Me.CaricaOrganizzazione()
    End Sub

	Public Sub AccediComunita(ByVal ComunitaID As Integer, ByVal Percorso As String) Implements IviewAccessoComunitaPresenter.AccediComunita

	End Sub

	Public Sub CancellaIscrizione(ByVal ComunitaID As Integer, ByVal Percorso As String) Implements IviewAccessoComunitaPresenter.CancellaIscrizione

	End Sub

	Public Sub MostraDettagliComunita(ByVal ComunitaID As Integer) Implements IviewAccessoComunitaPresenter.MostraDettagliComunita

	End Sub

	


#Region "Bind_Dati"
	Public Sub CaricaTipoComunita() Implements IviewAccessoComunitaPresenter.CaricaTipoComunita
		Dim Lista As List(Of COL_Tipo_Comunita)
		Lista = COL_Tipo_Comunita.PlainLista(Me.View.LinguaID, True)
		If Lista.Count > 1 Then
			Lista.Insert(0, New COL_Tipo_Comunita(-1, Me.View.Resource.getValue("All_maschile")))
		End If
		Me.View.ElencoTipoComunita = Lista
	End Sub
    Public Sub CaricaStatusComunita() Implements IviewAccessoComunitaPresenter.CaricaStatusComunita
        Dim iList As New List(Of FilterElement)

        Dim totale, TotaleArchiviate, totaleBloccate As Integer
        Try
            Me.View.UtenteCorrente.StatusComunitaIscritto(Me.View.UtenteCorrente.ID, totale, TotaleArchiviate, totaleBloccate)

            If Not (totaleBloccate = 0 And TotaleArchiviate = 0) Then
                iList.Add(New FilterElement(-1, Me.View.Resource.getValue("status_tutte")))
            End If
            iList.Add(New FilterElement(0, Me.View.Resource.getValue("status_attivate")))
            If TotaleArchiviate > 0 Then
                iList.Add(New FilterElement(1, Me.View.Resource.getValue("status_archiviate")))
            End If
            If totaleBloccate > 0 Then
                iList.Add(New FilterElement(2, Me.View.Resource.getValue("status_bloccate")))
            End If
        Catch ex As Exception

        End Try
        Me.View.ElencoStatus = iList
    End Sub
	Public Sub CaricaOrganizzazione() Implements IviewAccessoComunitaPresenter.CaricaOrganizzazione
		Dim Lista As List(Of COL_BusinessLogic_v2.IscrizioneComunita)
		Lista = Me.View.UtenteCorrente.PlainOrganizzazioniAssociate()
		Me.View.ElencoOrganizzazioni = Lista
	End Sub

	'Private Sub ChangeNumeroRecord(ByVal num As Integer)
	'	Try
	'		Me.DDLNumeroRecord.SelectedValue = num

	'	Catch ex As Exception
	'		Dim i, totale As Integer
	'		totale = Me.DDLNumeroRecord.Items.Count - 1
	'		For i = 0 To totale
	'			If Me.DDLNumeroRecord.Items(0).Value <= num Then
	'				Me.DDLNumeroRecord.SelectedIndex = -1
	'				Me.DDLNumeroRecord.Items(0).Selected = True
	'			Else
	'				Exit For
	'			End If
	'		Next
	'	End Try

	'End Sub
	'Private Sub ChangeTipoComunita()
	'	Dim showFiltroCorso As Boolean = True ' False

	'	'If Session("limbo") = True Then
	'	'    showFiltroCorso = True
	'	'Else
	'	'    Try
	'	'        Dim oComunita As New COL_Comunita
	'	'        oComunita.Id = Session("IDComunita")
	'	'        oComunita.Estrai()
	'	'        If oComunita.Livello = 0 Or oComunita.Livello = 1 Then
	'	'            showFiltroCorso = True
	'	'        End If
	'	'    Catch ex As Exception

	'	'    End Try
	'	'End If

	'	Me.TBLcorsi.Visible = False
	'	Me.TBLcorsiDiStudio.Visible = False
	'	Me.LBnoCorsi.Visible = False

	'	If Me.DDLTipo.SelectedValue = Main.TipoComunitaStandard.Corso And showFiltroCorso Then
	'		Me.TBLcorsi.Visible = True
	'		Me.TBRorgnCorsi.Visible = True
	'		Me.DDLtipoCorsoDiStudi.SelectedIndex = 0
	'		If Me.DDLannoAccademico.Items.Count > 1 Then
	'			Me.DDLannoAccademico.SelectedIndex = 1
	'		Else
	'			Me.DDLannoAccademico.SelectedIndex = 0
	'		End If
	'	ElseIf Me.DDLTipo.SelectedValue = Main.TipoComunitaStandard.CorsoDiLaurea Then
	'		Me.TBRorgnCorsi.Visible = True
	'		Me.TBLcorsiDiStudio.Visible = True
	'		Me.DDLtipoCorsoDiStudi.SelectedIndex = 0
	'		Me.DDLperiodo.SelectedIndex = 0
	'		Me.DDLannoAccademico.SelectedIndex = 0
	'	Else
	'		Me.LBnoCorsi.Visible = True
	'		Me.DDLtipoCorsoDiStudi.SelectedIndex = 0
	'		Me.DDLannoAccademico.SelectedIndex = 0
	'		Me.DDLperiodo.SelectedIndex = 0
	'		If Me.DDLorganizzazione.Items.Count > 1 Then
	'			Me.TBRorgnCorsi.Visible = True
	'		Else
	'			Me.TBRorgnCorsi.Visible = False
	'		End If
	'	End If
	'End Sub
	'Private Sub SetupFiltri()
	'	Me.Bind_Organizzazioni()
	'	Me.Bind_TipiCorsoDiStudi()

	'	If Me.Request.QueryString("re_set") = "true" Then
	'		Try
	'			Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("EntrataComunita")("DDLorganizzazione")
	'		Catch ex As Exception
	'			Me.Response.Cookies("EntrataComunita")("DDLorganizzazione") = Me.DDLorganizzazione.SelectedValue
	'		End Try
	'		Me.Bind_AnnoAccademico(Me.DDLorganizzazione.SelectedValue)
	'		Me.Bind_Periodo(Me.DDLorganizzazione.SelectedValue)
	'		Me.SetupSearchParameters()
	'	ElseIf Me.Request.QueryString("forChange") <> "true" Then
	'		Dim oImpostazioni As New COL_ImpostazioniUtente
	'		Try
	'			oImpostazioni = Session("oImpostazioni")
	'			If Not IsNothing(oImpostazioni) Then
	'				Try
	'					Me.DDLorganizzazione.SelectedValue = oImpostazioni.Organizzazione_Iscritto
	'				Catch ex As Exception

	'				End Try

	'				Me.Bind_AnnoAccademico(Me.DDLorganizzazione.SelectedValue)
	'				Try
	'					Me.DDLTipo.SelectedValue = oImpostazioni.TipoComunita_Iscritto
	'				Catch ex As Exception

	'				End Try
	'				If Me.DDLTipo.SelectedValue = Main.TipoComunitaStandard.Corso Then
	'					Me.ChangeTipoComunita()
	'				ElseIf Me.DDLTipo.SelectedValue = Main.TipoComunitaStandard.CorsoDiLaurea Then
	'					Me.ChangeTipoComunita()
	'				End If

	'				Try
	'					If oImpostazioni.AA_Iscritto = -2 Then
	'						Me.DDLannoAccademico.SelectedIndex = 1
	'					ElseIf oImpostazioni.AA_Iscritto = -2 Then
	'						Me.DDLannoAccademico.SelectedIndex = 0
	'					ElseIf Me.TBLcorsi.Visible Then
	'						Me.DDLannoAccademico.SelectedValue = oImpostazioni.AA_Iscritto
	'					End If

	'				Catch ex As Exception

	'				End Try
	'				Me.Bind_Periodo(Me.DDLorganizzazione.SelectedValue)
	'				Try
	'					If Me.TBLcorsi.Visible Then
	'						Me.DDLperiodo.SelectedValue = oImpostazioni.Periodo_Iscritto
	'					End If
	'				Catch ex As Exception

	'				End Try
	'				Try
	'					Me.ChangeNumeroRecord(oImpostazioni.Nrecord_Iscritto)
	'				Catch ex As Exception

	'				End Try
	'				If Me.DDLTipo.SelectedValue = Main.TipoComunitaStandard.CorsoDiLaurea Then
	'					Try
	'						Me.DDLtipoCorsoDiStudi.SelectedValue = oImpostazioni.TipoCDS_Iscritto
	'					Catch ex As Exception

	'					End Try
	'				End If
	'			Else
	'				Me.Bind_AnnoAccademico(Me.DDLorganizzazione.SelectedValue)
	'				Me.Bind_Periodo(Me.DDLorganizzazione.SelectedValue)
	'				If Me.DDLTipo.SelectedValue = Main.TipoComunitaStandard.Corso Then
	'					Me.ChangeTipoComunita()
	'				ElseIf Me.DDLTipo.SelectedValue = Main.TipoComunitaStandard.CorsoDiLaurea Then
	'					Me.ChangeTipoComunita()
	'				End If
	'			End If
	'			Me.SaveSearchParameters(3)
	'		Catch ex As Exception
	'			Me.Bind_AnnoAccademico(Me.DDLorganizzazione.SelectedValue)
	'			Me.Bind_Periodo(Me.DDLorganizzazione.SelectedValue)
	'		End Try
	'	Else
	'		Me.Bind_AnnoAccademico(Me.DDLorganizzazione.SelectedValue)
	'		Me.Bind_Periodo(Me.DDLorganizzazione.SelectedValue)
	'	End If
	'	If Me.Request.QueryString("re_set") <> "true" Then

	'		If Me.RBLstatoComunita.Items.Count > 1 Then
	'			Me.RBLstatoComunita.SelectedIndex = 1
	'		Else
	'			Me.RBLstatoComunita.SelectedIndex = 0
	'		End If

	'		If Not Session("limbo") = True Then
	'			Dim oComunita As New COL_Comunita
	'			oComunita.Id = Session("idComunita")
	'			If oComunita.isBloccata() Then
	'				Try
	'					Me.RBLstatoComunita.SelectedValue = 2
	'				Catch ex As Exception

	'				End Try
	'			ElseIf oComunita.isArchiviata() Then
	'				Try
	'					Me.RBLstatoComunita.SelectedValue = 1
	'				Catch ex As Exception

	'				End Try
	'			End If
	'		End If
	'	End If
	'	Me.DDLannoAccademico.AutoPostBack = Me.CBXautoUpdate.Checked
	'	Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked
	'	Me.DDLperiodo.AutoPostBack = Me.CBXautoUpdate.Checked
	'	Me.DDLtipoCorsoDiStudi.AutoPostBack = Me.CBXautoUpdate.Checked
	'	Me.RBLstatoComunita.AutoPostBack = Me.CBXautoUpdate.Checked

	'	Me.DDLTipoRicerca.Attributes.Add("onchange", "return AggiornaForm();")
	'	Me.HDNselezionato.Value = Me.DDLTipoRicerca.SelectedValue
	'	Me.DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value

	'	Me.Bind_Griglia(True)
	'End Sub







#End Region

	Public Sub CaricaResponsabile() Implements IviewAccessoComunitaPresenter.CaricaResponsabile
		'Private Sub Bind_Responsabili(Optional ByVal DocenteID As Integer = -1, Optional ByVal FiltroIscrizione As Main.FiltroRicercaComunitaByIscrizione = Main.FiltroRicercaComunitaByIscrizione.nonIscritto)
		'	Dim oDataSet As New DataSet
		'	Dim FacoltaID As Integer = -1
		'	Dim ComunitaID As Integer = -1
		'	Try
		'		Dim TipoComuniaID As Integer = -1
		'		Dim TipoCdlID As Integer = -1
		'		Dim AnnoAcc As Integer = -1
		'		Dim PeriodoID As Integer = -1

		'		Me.DDLresponsabile.Items.Clear()
		'		Try
		'			FacoltaID = Me.DDLorganizzazione.SelectedValue
		'		Catch ex As Exception
		'			FacoltaID = -1
		'		End Try
		'		Try
		'			If Session("IdComunita") > 0 Then
		'				ComunitaID = Session("IdComunita")
		'			End If
		'		Catch ex As Exception

		'		End Try
		'		Try
		'			'   If Me.CBXautoUpdate.Checked Then
		'			TipoComuniaID = Me.DDLTipo.SelectedValue
		'			'Else
		'			'    TipoComuniaID = Me.HDN_filtroTipoComunitaID.Value
		'			'End If
		'		Catch ex As Exception

		'		End Try

		'		If TipoComuniaID = Main.TipoComunitaStandard.Corso Then
		'			oDataSet = COL_Comunita.RicercaResponsabili(FacoltaID, ComunitaID, Session("objPersona").id, TipoComuniaID, , , AnnoAcc, PeriodoID, Me.RBLstatoComunita.SelectedValue, FiltroIscrizione)
		'		ElseIf TipoComuniaID = Main.TipoComunitaStandard.CorsoDiLaurea Then
		'			oDataSet = COL_Comunita.RicercaResponsabili(FacoltaID, ComunitaID, Session("objPersona").id, TipoComuniaID, TipoCdlID, , , , Me.RBLstatoComunita.SelectedValue, FiltroIscrizione)
		'		Else
		'			oDataSet = COL_Comunita.RicercaResponsabili(FacoltaID, ComunitaID, Session("objPersona").id, TipoComuniaID, , , , , Me.RBLstatoComunita.SelectedValue, FiltroIscrizione)
		'		End If


		'		If oDataSet.Tables(0).Rows.Count > 0 Then
		'			DDLresponsabile.DataSource = oDataSet
		'			DDLresponsabile.DataTextField() = "Anagrafica"
		'			DDLresponsabile.DataValueField() = "PRSN_ID"
		'			DDLresponsabile.DataBind()

		'			'aggiungo manualmente elemento che indica tutti i tipi di comunità
		'			DDLresponsabile.Items.Insert(0, New ListItem("-- Tutti --", -1))
		'		End If
		'	Catch ex As Exception
		'		Me.DDLresponsabile.Items.Insert(0, New ListItem("-- Tutti --", -1))
		'	End Try
		'	oResource.setDropDownList(Me.DDLresponsabile, -1)
		'	If DocenteID > 0 Then
		'		Try
		'			Me.DDLresponsabile.SelectedValue = DocenteID
		'		Catch ex As Exception

		'		End Try
		'	End If

		'	Try
		'		If Me.DDLTipoRicerca.SelectedValue = Main.FiltroComunita.IDresponsabile Then
		'			Me.DDLresponsabile.Visible = True
		'			Me.TXBValore.Visible = False
		'			Me.TXBValore.Text = ""
		'		Else
		'			Me.DDLresponsabile.Visible = False
		'			Me.TXBValore.Visible = True
		'		End If
		'	Catch ex As Exception

		'	End Try
		'End Sub
	End Sub


End Class
