Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public MustInherit Class PMpageBaseEdit
    Inherits PMpageBase
    Implements IViewPageBaseEdit

#Region "Implements"

#Region "Preload"
    Protected Friend ReadOnly Property PreloadIdProject As Long Implements IViewPageBaseEdit.PreloadIdProject
        Get
            If IsNumeric(Me.Request.QueryString("pId")) Then
                Return CLng(Me.Request.QueryString("pId"))
            Else
                Return 0
            End If
        End Get
    End Property
#End Region

#Region "Settings"
    Protected Property IdProject As Long Implements IViewPageBaseEdit.IdProject
        Get
            Return ViewStateOrDefault("IdProject", 0)
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdProject") = value
        End Set
    End Property

#End Region

#End Region


End Class