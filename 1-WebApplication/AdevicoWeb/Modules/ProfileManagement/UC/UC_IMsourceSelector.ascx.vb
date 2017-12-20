Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation

Public Class UC_IMsourceSelector
    Inherits BaseControl
    Implements IViewIMsourceSelector

#Region "Implements"
    Public Property CurrentSource As SourceType Implements IViewIMsourceSelector.CurrentSource
        Get
            If (Me.RBLsourceType.SelectedIndex > -1) Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SourceType).GetByString(Me.RBLsourceType.SelectedValue, SourceType.None)
            Else
                Return SourceType.None
            End If
        End Get
        Set(value As SourceType)
            If Not IsNothing(Me.RBLsourceType.Items.FindByValue(value.ToString)) Then
                Me.RBLsourceType.SelectedValue = value.ToString
            End If
            If Me.RBLsourceType.Items.Count > 0 AndAlso Me.RBLsourceType.SelectedIndex = -1 Then
                Me.RBLsourceType.SelectedIndex = 0
            End If
        End Set
    End Property
    Public Property CurrentRange As lm.Comol.Core.DomainModel.Helpers.SearchRange Implements IViewIMsourceSelector.CurrentRange
        Get
            Return ViewStateOrDefault("CurrentRange", lm.Comol.Core.DomainModel.Helpers.SearchRange.Portal)
        End Get
        Set(value As lm.Comol.Core.DomainModel.Helpers.SearchRange)
            ViewState("CurrentRange") = value
        End Set
    End Property
    Public Property IdCommunityRange As Integer Implements IViewIMsourceSelector.IdCommunityRange
        Get
            Return ViewStateOrDefault("IdCommunityRange", -1)
        End Get
        Set(value As Integer)
            ViewState("IdCommunityRange") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewIMsourceSelector.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal Control"
    Private _CallService As lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers
    Private ReadOnly Property CallService() As lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers
        Get
            If IsNothing(_CallService) Then
                _CallService = New lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers(Me.PageUtility.CurrentContext)
            End If
            Return _CallService
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfilesImport", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBemptyMessage)
            .setLabel(LBsourceSelector_t)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(defaultSource As SourceType) Implements IViewIMsourceSelector.InitializeControl
        InitializeControl(defaultSource, CurrentRange, IdCommunityRange)
    End Sub
    Public Sub InitializeControl(defaultSource As SourceType, range As lm.Comol.Core.DomainModel.Helpers.SearchRange, Optional idCommunity As Integer = -1) Implements IViewIMsourceSelector.InitializeControl
        Dim items As New List(Of SourceType)
        items.Add(SourceType.FileCSV)
        If CallService.ExistCallForEnroll(range, lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.RequestForMembership, idCommunity) Then
            items.Add(SourceType.RequestForMembership)
        End If
        If CallService.ExistCallForEnroll(range, lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids, idCommunity) Then
            items.Add(SourceType.CallForPapers)
        End If

        Me.LoadAvailableSources(items)

        Me.isInitialized = True
        Me.CurrentSource = defaultSource
        Me.MLVcontrolData.SetActiveView(VIWsource)
    End Sub
    Public Sub LoadAvailableSources(items As List(Of SourceType)) Implements IViewIMsourceSelector.LoadAvailableSources
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In items Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("SourceType." & s.ToString)}).ToList

        Me.RBLsourceType.DataSource = translations
        Me.RBLsourceType.DataValueField = "Id"
        Me.RBLsourceType.DataTextField = "Translation"
        Me.RBLsourceType.DataBind()
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewIMsourceSelector.DisplaySessionTimeout
        Me.MLVcontrolData.SetActiveView(VIWempty)

    End Sub
#End Region

End Class