Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public MustInherit Class PMpageProjectDashboard
    Inherits PMpageBaseDashboard

#Region "Context"
    Private _Presenter As ProjectDashboardPresenter
    Protected Friend ReadOnly Property CurrentPresenter() As ProjectDashboardPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProjectDashboardPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "internal"
    Protected Function GetMessageType(savedTasks As Integer, unsavedTasks As Integer) As lm.Comol.Core.DomainModel.Helpers.MessageType
        If savedTasks = 0 AndAlso unsavedTasks > 0 Then
            Return Helpers.MessageType.error
        ElseIf savedTasks > 0 AndAlso unsavedTasks = 0 Then
            Return Helpers.MessageType.success
        ElseIf savedTasks > 0 AndAlso unsavedTasks > 0 Then
            Return Helpers.MessageType.alert
        Else
            Return Helpers.MessageType.info
        End If
    End Function
#End Region
End Class