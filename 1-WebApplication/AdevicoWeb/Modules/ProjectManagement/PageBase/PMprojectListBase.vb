Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public MustInherit Class PMprojectListBase
    Inherits BaseControl
    Implements IViewProjectListBase


#Region "Implements"
    Protected Property CurrentPageType As PageListType Implements IViewProjectListBase.CurrentPageType
        Get
            Return ViewStateOrDefault("CurrentPageType", PageListType.ListResource)
        End Get
        Set(value As PageListType)
            ViewState("CurrentPageType") = value
            DisplayToggleProjectRoles(value <> PageListType.ListAdministrator)
        End Set
    End Property
    Public Property IsInitialized As Boolean Implements IViewProjectListBase.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Private translations As New Dictionary(Of ActivityRole, String)
    Protected ReadOnly Property RoleTranslations As Dictionary(Of ActivityRole, String) Implements IViewProjectListBase.RoleTranslations
        Get
            If Not translations.Values.Any() Then
                translations = (From i In [Enum].GetValues(GetType(ActivityRole)).Cast(Of ActivityRole).ToList() Select i).ToDictionary(Function(i) i, Function(i) Resource.getValue("RoleTranslations." & i.ToString))
            End If
            Return translations
        End Get
    End Property
    Protected Property IdCurrentCommunityForList As Integer Implements IViewProjectListBase.IdCurrentCommunityForList
        Get
            Return ViewStateOrDefault("IdCurrentCommunityForList", -1)
        End Get
        Set(value As Integer)
            ViewState("IdCurrentCommunityForList") = value
        End Set
    End Property
    Public Property CurrentAscending As Boolean Implements IViewProjectListBase.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", False)
        End Get
        Set(value As Boolean)
            ViewState("CurrentAscending") = value
        End Set
    End Property
    Public Property CurrentOrderBy As ProjectOrderBy Implements IViewProjectListBase.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", ProjectOrderBy.Deadline)
        End Get
        Set(value As ProjectOrderBy)
            ViewState("CurrentOrderBy") = value
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


#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
#End Region

#Region "Implements"
    Protected Friend ReadOnly Property UnknownUserTranslation As String Implements IViewProjectListBase.UnknownUserTranslation
        Get
            Return Me.Resource.getValue("UnknownUser")
        End Get
    End Property
#End Region

    Protected MustOverride Sub DisplayToggleProjectRoles(ByVal display As Boolean)

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleProjectManagement.ActionType) Implements IViewProjectListBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idProject As Long, action As ModuleProjectManagement.ActionType) Implements IViewProjectListBase.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleProjectManagement.ObjectType.Project, idProject.ToString), InteractionType.UserWithLearningObject)
    End Sub
End Class