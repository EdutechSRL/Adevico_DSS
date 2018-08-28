Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_ElencaComunita
Imports lm.ActionDataContract

Partial Public Class NEWentrataComunita
	Inherits PageBase
	Implements IviewEntrataComunita


	Private _presenter As AccessoComunitaPresenter
	Private _Servizio As Services_ElencaComunita

	Public Overrides ReadOnly Property AlwaysBind() As Boolean
		Get
			Return False
		End Get
	End Property
	Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
		Get
			Return True
		End Get
	End Property
#Region "Proprieta popolamento Filtri"
    Public Property Selezione_Organizzazioneata() As COL_BusinessLogic_v2.FilterElement Implements IviewEntrataComunita.Selezione_Organizzazioneata
        Get
            Try
                Selezione_Organizzazioneata = New FilterElement(Me.DDLorganizzazione.SelectedValue, Me.DDLorganizzazione.SelectedItem.Text)
            Catch ex As Exception
                Selezione_Organizzazioneata = Nothing
            End Try
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.FilterElement)
            Try
                Me.DDLorganizzazione.SelectedValue = value.Value
            Catch ex As Exception

            End Try
        End Set
    End Property
    Public Property Selezione_Referente() As COL_BusinessLogic_v2.FilterElement Implements IviewEntrataComunita.Selezione_Referente
        Get
            Try
                Selezione_Referente = New FilterElement(Me.DDLresponsabile.SelectedValue, Me.DDLresponsabile.SelectedItem.Text)
            Catch ex As Exception
                Selezione_Referente = Nothing
            End Try
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.FilterElement)
            Try
                Me.DDLresponsabile.SelectedValue = value.Value
            Catch ex As Exception

            End Try
        End Set
    End Property
    Public Property Selezione_Status() As COL_BusinessLogic_v2.FilterElement Implements IviewEntrataComunita.Selezione_Status
        Get
            Try
                Selezione_Status = New FilterElement(Me.RBLstatoComunita.SelectedValue, Me.RBLstatoComunita.SelectedItem.Text)
            Catch ex As Exception
                Selezione_Status = Nothing
            End Try
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.FilterElement)
            Try
                Me.RBLstatoComunita.SelectedValue = value.Value
            Catch ex As Exception

            End Try
        End Set
    End Property
    Public Property Selezione_TipoComunita() As COL_BusinessLogic_v2.FilterElement Implements IviewEntrataComunita.Selezione_TipoComunita
        Get
            Try
                Selezione_TipoComunita = New FilterElement(Me.DDLTipo.SelectedValue, Me.DDLTipo.SelectedItem.Text)
            Catch ex As Exception
                Selezione_TipoComunita = Nothing
            End Try
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.FilterElement)
            Try
                Me.DDLTipo.SelectedValue = value.Value
            Catch ex As Exception

            End Try
        End Set
    End Property


    Public WriteOnly Property ElencoOrganizzazioni() As System.Collections.Generic.List(Of COL_BusinessLogic_v2.IscrizioneComunita) Implements IviewEntrataComunita.ElencoOrganizzazioni
        Set(ByVal value As System.Collections.Generic.List(Of COL_BusinessLogic_v2.IscrizioneComunita))
            Me.DDLorganizzazione.DataSource = value
            Me.DDLorganizzazione.DataTextField = "OrganizzazioneNome"
            Me.DDLorganizzazione.DataValueField = "OrganizzazioneId"
            Me.DDLorganizzazione.DataBind()
        End Set
    End Property

    Public WriteOnly Property ElencoResponsabili() As System.Collections.Generic.List(Of COL_BusinessLogic_v2.CL_persona.COL_Persona) Implements IviewEntrataComunita.ElencoResponsabili
        Set(ByVal value As System.Collections.Generic.List(Of COL_BusinessLogic_v2.CL_persona.COL_Persona))
            Me.DDLresponsabile.DataSource = value
            Me.DDLresponsabile.DataTextField = "Anagrafica"
            Me.DDLresponsabile.DataValueField = "Id"
            Me.DDLresponsabile.DataBind()
        End Set
    End Property
    Public WriteOnly Property ElencoStatus() As System.Collections.Generic.List(Of COL_BusinessLogic_v2.FilterElement) Implements IviewEntrataComunita.ElencoStatus
        Set(ByVal value As System.Collections.Generic.List(Of COL_BusinessLogic_v2.FilterElement))
            Me.RBLstatoComunita.DataSource = value
            Me.RBLstatoComunita.DataTextField = "Text"
            Me.RBLstatoComunita.DataValueField = "Value"
            Me.RBLstatoComunita.DataBind()
        End Set
    End Property

    Public WriteOnly Property ElencoTipoComunita() As System.Collections.Generic.List(Of COL_BusinessLogic_v2.Comunita.COL_Tipo_Comunita) Implements IviewEntrataComunita.ElencoTipoComunita
        Set(ByVal value As System.Collections.Generic.List(Of COL_BusinessLogic_v2.Comunita.COL_Tipo_Comunita))
            Me.DDLTipo.DataSource = value
            Me.DDLTipo.DataTextField = "Descrizione"
            Me.DDLTipo.DataValueField = "Id"
            Me.DDLTipo.DataBind()
        End Set
    End Property
#End Region

#Region "Proprieta impostazioni filtro"
    Public Property AutomaticFilterUpdate() As Boolean Implements IviewEntrataComunita.AutomaticFilterUpdate
        Get
            AutomaticFilterUpdate = Me.CBXautoUpdate.Checked
        End Get
        Set(ByVal value As Boolean)
            Me.CBXautoUpdate.Checked = value
        End Set
    End Property

    Public Property FiltroTipoComunita() As COL_BusinessLogic_v2.FilterElement Implements IviewEntrataComunita.FiltroTipoComunita
        Get
            Try
                FiltroTipoComunita = DirectCast(Me.ViewState("FiltroTipoComunita"), FilterElement)
            Catch ex As Exception
                FiltroTipoComunita = Nothing
            End Try
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.FilterElement)
            Me.ViewState("FiltroTipoComunita") = value
        End Set
    End Property
    Public Property FiltroLettera() As COL_BusinessLogic_v2.Main.FiltroComunita Implements IviewEntrataComunita.FiltroLettera
        Get
            Try
                FiltroLettera = DirectCast(Me.ViewState("FiltroLettera"), FiltroComunita)
            Catch ex As Exception
                FiltroLettera = Nothing
            End Try
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.Main.FiltroComunita)
            Me.ViewState("FiltroLettera") = value
        End Set
    End Property
    Public Property FiltroOrganizzazione() As COL_BusinessLogic_v2.FilterElement Implements IviewEntrataComunita.FiltroOrganizzazione
        Get
            Try
                FiltroOrganizzazione = DirectCast(Me.ViewState("FiltroOrganizzazione"), FilterElement)
            Catch ex As Exception
                FiltroOrganizzazione = Nothing
            End Try
        End Get
        Set(ByVal value As FilterElement)
            Me.ViewState("FiltroOrganizzazione") = value
        End Set
    End Property

    Public ReadOnly Property FiltroQuickSearch() As COL_BusinessLogic_v2.QuickSearchElement Implements IviewEntrataComunita.FiltroQuickSearch
        Get
            FiltroQuickSearch = New QuickSearchElement(Me.DDLTipoRicerca.SelectedItem.Text, Me.DDLTipoRicerca.SelectedValue, Me.TXBValore.Text)
        End Get
    End Property
    Public Property FiltroReferente() As COL_BusinessLogic_v2.FilterElement Implements IviewEntrataComunita.FiltroReferente
        Get
            Try
                FiltroReferente = DirectCast(Me.ViewState("FiltroReferente"), FilterElement)
            Catch ex As Exception
                FiltroReferente = Nothing
            End Try
        End Get
        Set(ByVal value As FilterElement)
            Me.ViewState("FiltroReferente") = value
        End Set
    End Property
    Public Property FiltroStatus() As COL_BusinessLogic_v2.FilterElement Implements IviewEntrataComunita.FiltroStatus
        Get
            Try
                FiltroStatus = DirectCast(Me.ViewState("FiltroStatus"), FilterElement)
            Catch ex As Exception
                FiltroStatus = Nothing
            End Try
        End Get
        Set(ByVal value As FilterElement)
            Me.ViewState("FiltroStatus") = value
        End Set
    End Property

#End Region



    Public Property Ordinamento() As Main.FiltroOrdinamento Implements IviewEntrataComunita.Ordinamento
		Get
			Try
				Ordinamento = DirectCast(Me.ViewState("Ordinamento"), Main.FiltroOrdinamento)
			Catch ex As Exception
				Me.ViewState("Ordinamento") = FiltroOrdinamento.Crescente
				Ordinamento = FiltroOrdinamento.Crescente
			End Try
		End Get
		Set(ByVal value As Main.FiltroOrdinamento)
			Me.ViewState("Ordinamento") = value
		End Set
	End Property

	Public ReadOnly Property Servizio() As UCServices.Services_ElencaComunita Implements IviewEntrataComunita.Servizio
		Get
			If IsNothing(_Servizio) Then
				_Servizio = MyBase.ElencoServizi.Find(Services_ElencaComunita.Codex)
			End If
			Servizio = _Servizio
		End Get
	End Property

	Private Sub NEWentrataComunita_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
		_presenter = New AccessoComunitaPresenter(Me)
	End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Page.Form.DefaultFocus = Me.TXBValore.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.TXBValore.UniqueID
	End Sub

	Public Overrides Sub BindDati()
		Me._presenter.Initialize()
	End Sub

	Public Overrides Sub BindNoPermessi()
		Me.MVIcomunita.SetActiveView(Me.VIWpermessi)
	End Sub

	Public Overrides Function HasPermessi() As Boolean
		If Me.isPortalCommunity Then
			Return Not Me.isUtenteAnonimo
		ElseIf IsNothing(Servizio) Then
			Return False
		Else
			Return (Servizio.Admin Or Servizio.List)
		End If
	End Function

	Public Overrides Sub RegistraAccessoPagina()
		Me.PageUtility.AddAction(ActionType.List, Nothing, InteractionType.Generic)
	End Sub

	Public Overrides Sub SetCultureSettings()
		Me.SetCulture("pg_EntrataComunita", "Comunita")
	End Sub

	Public Overrides Sub SetInternazionalizzazione()
		With MyBase.Resource

		End With
	End Sub

	Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)
		Me.MVIcomunita.SetActiveView(Me.VIWmessaggi)
		Me.LBMessaggi.Text = errorMessage
	End Sub

	Public Sub CaricaLista(ByVal items As System.Collections.Generic.List(Of COL_BusinessLogic_v2.Comunita.PlainComunita)) Implements IviewEntrataComunita.CaricaLista

	End Sub

	Public Sub DettagliComunita(ByVal ComunitaID As Integer) Implements IviewEntrataComunita.DettagliComunita

	End Sub

	Public Sub EntrataComunita(ByVal ComunitaID As Integer, ByVal Percorso As String) Implements IviewEntrataComunita.EntrataComunita

	End Sub

	Public Sub FiltroLinkLettere_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
		'If sender.commandArgument <> "" Then
		'	Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
		'	Me.ViewState("intAnagrafica") = sender.commandArgument
		'	sender.CssClass = "lettera_Selezionata"
		'Else
		'	Me.ViewState("intAnagrafica") = -1
		'	Me.LKBtutti.CssClass = "lettera_Selezionata"
		'End If
		'Me.ViewState("intCurPage") = 0
		'Me.DGComunita.CurrentPageIndex = 0
		'Me.Bind_Griglia()
	End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_IscrizioneComunita.Codex)
    End Sub
End Class