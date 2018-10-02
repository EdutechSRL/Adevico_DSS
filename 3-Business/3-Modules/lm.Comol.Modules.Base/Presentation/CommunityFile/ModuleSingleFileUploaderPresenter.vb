Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core

Namespace lm.Comol.Modules.Base.Presentation
    Public Class ModuleSingleFileUploaderPresenter
        Inherits DomainPresenter

        Private _ModuleID As Integer


        Private ReadOnly Property ModuleRepositoryID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.CommonManager.GetModuleID(UCServices.Services_File.Codex)
                End If
                Return _ModuleID
            End Get
        End Property

        Private _CommonManager As ManagerCommon
        Public Overloads Property CurrentManager() As ManagerCommunityFiles
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerCommunityFiles)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property

        Public Overloads ReadOnly Property View() As IviewModuleSingleFileUploader
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewModuleSingleFileUploader)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub


        Public Sub InitView(ByVal CommunityID As Integer)
            Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityID)
            If CommunityID > 0 AndAlso oCommunity Is Nothing Then
                CommunityID = Me.UserContext.CurrentCommunityID
            End If

            Me.View.RepositoryCommunityID = CommunityID
            Me.View.ItemType = Repository.RepositoryItemType.FileStandard
            Me.View.AllowDownload = True
        End Sub

        Public Function AddModuleInternalFile(ByVal FileType As FileRepositoryType, ByVal oFile As ModuleInternalFile, ByVal SavedFile As String, ByVal CommunityPath As String, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As dtoModuleUploadedFile
            Dim iResponse As New dtoModuleUploadedFile(oFile, ItemRepositoryStatus.CreationError)
            If Not IsNothing(oFile) AndAlso File.Exists.File(SavedFile) Then
                Dim CommunityID As Integer = Me.View.RepositoryCommunityID
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityID)
                Dim fileStatus As ItemRepositoryStatus = ItemRepositoryStatus.None
                If (CommunityID > 0 AndAlso Not IsNothing(CommunityID)) OrElse (IsNothing(oCommunity) AndAlso CommunityID = 0) Then
                    If Me.View.AllowAnonymousUpload AndAlso Me.UserContext.CurrentUserID = 0 Then
                        oFile.Owner = Me.CommonManager.GetAnonymousUser()
                    Else
                        oFile.Owner = Me.CommonManager.GetPerson(Me.UserContext.CurrentUserID)
                    End If

                    oFile.CommunityOwner = oCommunity
                    oFile.CreatedOn = Now
                    oFile.ModifiedOn = Now
                    oFile.ModifiedBy = oFile.Owner
                    Dim Permission As Long = UCServices.Services_File.Base2Permission.DownloadFile
                    If FileType = FileRepositoryType.InternalLong Then
                        Dim oLongFile As New ModuleLongInternalFile(oFile)
                        oLongFile.ServiceOwner = ServiceOwner
                        oLongFile.ServiceActionAjax = ServiceOwnerActionID
                        oLongFile.ObjectTypeID = ObjectTypeID
                        oLongFile.ObjectOwner = ObjectOwner
                        iResponse.File = Me.CurrentManager.AddModuleLongInternalFile(oLongFile, oFile.Owner, CommunityPath, Permission, fileStatus)
                        If Not IsNothing(iResponse.File) Then
                            iResponse.Link = (Me.CurrentManager.CreateModuleActionLink(iResponse.File, UCServices.Services_File.Base2Permission.DownloadFile, CurrentManager.ModuleId))
                        End If
                    ElseIf FileType = FileRepositoryType.InternalGuid Then
                        Dim oGuidFile As New ModuleGuidInternalFile(oFile)
                        oGuidFile.ServiceActionAjax = ServiceOwnerActionID
                        oGuidFile.ServiceOwner = ServiceOwner
                        oGuidFile.ObjectTypeID = ObjectTypeID
                        oGuidFile.ObjectOwner = ObjectOwner
                        iResponse.File = Me.CurrentManager.AddModuleGuidInternalFile(oGuidFile, oFile.Owner, CommunityPath, Permission, fileStatus)

                        If Not IsNothing(iResponse.File) Then
                            iResponse.Link = (Me.CurrentManager.CreateModuleActionLink(iResponse.File, UCServices.Services_File.Base2Permission.DownloadFile, CurrentManager.ModuleId))
                        End If
                    End If
                End If

                iResponse.Status = fileStatus
            End If
            Return iResponse
        End Function

        Public Function GetModuleInternalFileLink(ByVal oFile As ModuleInternalFile) As ModuleActionLink
            Dim iResponse As ModuleActionLink = Nothing
            If Not IsNothing(oFile) Then
                iResponse = Me.CurrentManager.CreateModuleActionLink(oFile, UCServices.Services_File.Base2Permission.DownloadFile, Me.ModuleRepositoryID)
            End If
            Return iResponse
        End Function

        'Private Function ItemDefaultAction(ByVal Item As BaseCommunityFile) As Integer
        '    If Item.isSCORM Then
        '        Return UCServices.Services_File.ActionType.PlayFile
        '    Else
        '        Return UCServices.Services_File.ActionType.DownloadFile
        '    End If
        'End Function
    End Class
End Namespace