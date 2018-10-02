Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Core

Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class ModuleMultipleFileUploaderPresenter
        Inherits DomainPresenter

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

        Public Overloads ReadOnly Property View() As IviewModuleMultipleFileUploader
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewModuleMultipleFileUploader)
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
        End Sub

        Public Function AddModuleInternalFile(ByVal FileType As FileRepositoryType, ByVal oFile As ModuleInternalFile, ByVal SavedFile As String, ByVal CommunityPath As String, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As dtoModuleUploadedFile
            Dim iResponse As New dtoModuleUploadedFile(oFile, ItemRepositoryStatus.CreationError)
            If Not IsNothing(oFile) AndAlso File.Exists.File(SavedFile) Then
                Dim CommunityID As Integer = Me.View.RepositoryCommunityID
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityID)
                Dim fileStatus As ItemRepositoryStatus = ItemRepositoryStatus.None
                If (CommunityID > 0 AndAlso Not IsNothing(CommunityID)) OrElse (IsNothing(oCommunity) AndAlso CommunityID = 0) Then
                    Dim owner As Person = Nothing
                    Dim idUploadAs As Integer = View.UploadAsUser
                    If Me.UserContext.CurrentUserID > 0 AndAlso idUploadAs < 1 Then
                        owner = Me.CommonManager.GetPerson(Me.UserContext.CurrentUserID)
                    ElseIf idUploadAs > 0 Then
                        owner = Me.CommonManager.GetPerson(idUploadAs)
                    End If
                    If IsNothing(owner) AndAlso View.AllowAnonymousUpload Then
                        owner = CommonManager.GetAnonymousUser()
                    End If
                    If Not IsNothing(owner) Then
                        oFile.Owner = owner
                        oFile.CommunityOwner = oCommunity
                        oFile.CreatedOn = Now
                        oFile.ModifiedOn = Now
                        oFile.ModifiedBy = oFile.Owner
                        Dim Permission As Long = UCServices.Services_File.Base2Permission.DownloadFile
                        If FileType = FileRepositoryType.InternalLong Then
                            Dim oLongFile As New ModuleLongInternalFile(oFile)
                            oLongFile.ServiceActionAjax = ServiceOwnerActionID
                            oLongFile.ServiceOwner = ServiceOwner
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
                    Else
                        fileStatus = ItemRepositoryStatus.NotLoggedIn
                    End If
                End If
                iResponse.Status = fileStatus
            End If
            Return iResponse
        End Function
    End Class
End Namespace