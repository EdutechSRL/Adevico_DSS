Imports COL_BusinessLogic_v2
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2.UCServices

Public Class HTTPhandler_Utility
    Inherits HTTPhandlerModuleUtility

    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _CommonManager As lm.Comol.Modules.Base.BusinessLogic.ManagerCommon
    Private _ManagerBaseFile As lm.Comol.Modules.Base.BusinessLogic.ManagerFiles
    Private _CurrentManager As lm.Comol.Modules.Base.BusinessLogic.ManagerCommunityFiles
    
    Public Overloads Property CurrentManager() As lm.Comol.Modules.Base.BusinessLogic.ManagerCommunityFiles
        Get
            Return _CurrentManager
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.BusinessLogic.ManagerCommunityFiles)
            _CurrentManager = value
        End Set
    End Property
    Public Overloads Property ManagerInternalFiles() As lm.Comol.Modules.Base.BusinessLogic.ManagerFiles
        Get
            Return _ManagerBaseFile
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.BusinessLogic.ManagerFiles)
            _ManagerBaseFile = value
        End Set
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public Overloads Property CommonManager() As lm.Comol.Modules.Base.BusinessLogic.ManagerCommon
        Get
            Return _CommonManager
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.BusinessLogic.ManagerCommon)
            _CommonManager = value
        End Set
    End Property
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        Dim oModule As ModuleCommunityRepository = Nothing

        If CommunityID = 0 Then
            oModule = ModuleCommunityRepository.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)
        Else
            oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                  Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
            If IsNothing(oModule) Then
                ManagerPersona.PurgeServiceCache(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex)
                oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault

                If IsNothing(oModule) Then
                    oModule = New ModuleCommunityRepository
                End If
            End If
        End If

        Return oModule
    End Function
    Public Sub New(ByVal context As System.Web.HttpContext)
        MyBase.New(context)
        CurrentManager = New lm.Comol.Modules.Base.BusinessLogic.ManagerCommunityFiles(Me.CurrentContext)
        Me.CommonManager = New lm.Comol.Modules.Base.BusinessLogic.ManagerCommon(Me.CurrentContext)
        Me.ManagerInternalFiles = New lm.Comol.Modules.Base.BusinessLogic.ManagerFiles(Me.CurrentContext)
    End Sub
    Public Sub New(ByVal context As System.Web.HttpContext, ByVal oConfig As FileSettings.ConfigType)
        MyBase.New(context, oConfig)
        CurrentManager = New lm.Comol.Modules.Base.BusinessLogic.ManagerCommunityFiles(Me.CurrentContext)
        Me.CommonManager = New lm.Comol.Modules.Base.BusinessLogic.ManagerCommon(Me.CurrentContext)
        Me.ManagerInternalFiles = New lm.Comol.Modules.Base.BusinessLogic.ManagerFiles(Me.CurrentContext)
    End Sub
End Class