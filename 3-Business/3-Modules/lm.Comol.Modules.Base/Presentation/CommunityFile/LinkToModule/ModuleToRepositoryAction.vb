Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class ModuleToRepositoryAction
        Inherits DomainPresenter

        Private _ModuleID As Integer
        Private _ExternalModuleID As Integer
        Private _CommonManager As ManagerCommon
        Private ReadOnly Property RepositoryModuleID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.CommonManager.GetModuleID(UCServices.Services_File.Codex)
                End If
                Return _ModuleID
            End Get
        End Property
        Private Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property
        Public Overloads Property CurrentManager() As ManagerCommunityFiles
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerCommunityFiles)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As ImoduleToRepositoryAction
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As ImoduleToRepositoryAction)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView(ByVal DescriptionOnly As Boolean, ByVal pLink As ModuleLink)
            If pLink Is Nothing Then
                Me.View.DisplayNoAction()
            ElseIf pLink.DestinationItem.ServiceCode = UCServices.Services_File.Codex Then
                AnalyzeAction(DescriptionOnly, pLink)
            Else
                Me.View.DisplayNoAction()
            End If
        End Sub
        Public Function InitRemoteControlView(ByVal DescriptionOnly As Boolean, ByVal pLink As ModuleLink) As List(Of dtoModuleActionControl)
            Dim oRemoteControls As New List(Of dtoModuleActionControl)
            If pLink Is Nothing Then
                Me.View.DisplayNoAction()
            ElseIf pLink.DestinationItem.ServiceCode = UCServices.Services_File.Codex Then
                oRemoteControls = AnalyzeAction(DescriptionOnly, pLink, True)
            Else
                Me.View.DisplayNoAction()
            End If
            Return oRemoteControls
        End Function


        Private Function AnalyzeAction(ByVal DescriptionOnly As Boolean, ByVal pLink As ModuleLink, Optional ByVal RemoteControl As Boolean = False) As List(Of dtoModuleActionControl)
            Dim ActionID As Integer = pLink.Action
            Dim oItem As BaseCommunityFile = Nothing
            Dim oRemoteControls As New List(Of dtoModuleActionControl)

            If Not IsNothing(pLink.DestinationItem) Then
                oItem = Me.CurrentManager.GetItem(pLink.DestinationItem.ObjectLongID)
                If IsNothing(oItem) OrElse (pLink.DestinationItem.ObjectLongID = 0 AndAlso (ActionID = UCServices.Services_File.ActionType.CreateFolder OrElse ActionID = UCServices.Services_File.ActionType.UploadFile)) Then
                    Me.View.DisplayRemovedObject()
                Else
                    Select Case ActionID
                        'Case UCServices.Services_File.ActionType.CreateFolder
                        '    Dim CommunityID As Integer = 0
                        '    If Not IsNothing(oItem.CommunityOwner) Then
                        '        CommunityID = oItem.CommunityOwner.Id
                        '    End If
                        '    Me.View.ActionForCreateFolder(DescriptionOnly, pLink.Id, oItem.Id, oItem.DisplayName, CommunityID)
                        Case UCServices.Services_File.ActionType.DownloadFile
                            If Not IsNothing(oItem) Then 'senza questo quando si cancella un file nel repository l'app va in crash

                                Dim CommunityID As Integer = 0
                                If TypeOf oItem Is CommunityFile Then
                                    Me.View.ServiceCode = pLink.DestinationItem.ServiceCode
                                    Me.View.ServiceID = pLink.DestinationItem.ServiceID
                                Else
                                    Me.View.ServiceCode = pLink.SourceItem.ServiceCode
                                    Me.View.ServiceID = pLink.SourceItem.ServiceID
                                End If
                                If Not IsNothing(oItem.CommunityOwner) Then
                                    CommunityID = oItem.CommunityOwner.Id
                                End If
                                'If oItem.isSCORM Then
                                '    Me.View.ActionForPlay(DescriptionOnly, pLink.Id, oItem.Id, oItem.DisplayName, oItem.Extension, oItem.Size, oItem.UniqueID, CommunityID)
                                '    If RemoteControl Then
                                '        oRemoteControls.Add(New dtoModuleActionControl(Me.View.GetUrlForPlay(pLink.Id, oItem.Id, oItem.UniqueID, CommunityID), True))
                                '    End If
                                'Else
                                If DescriptionOnly Then
                                    Me.View.DisplayItemAction(oItem.DisplayName, oItem.Extension, oItem.Size, oItem.RepositoryItemType)
                                Else
                                    Me.View.ActionForDownload(pLink.Id, CommunityID, oItem)
                                End If

                                If RemoteControl Then
                                    oRemoteControls.Add(New dtoModuleActionControl(Me.View.GetUrlForDownload(pLink.Id, oItem.Id, CommunityID), StandardActionType.Play, True))
                                End If
                                '   End If
                            Else
                                View.DisplayRemovedObject()
                            End If
                        Case UCServices.Services_File.ActionType.PlayFile
                            Me.View.ServiceCode = pLink.SourceItem.ServiceCode
                            Me.View.ServiceID = pLink.SourceItem.ServiceID

                            If Not IsNothing(oItem) Then
                                Dim CommunityID As Integer = 0
                                ' COMMENTATO IL 11/10/2011
                                '  If TypeOf oItem Is CommunityFile Then
                                ' Me.View.ServiceCode = pLink.DestinationItem.ServiceCode
                                ' Me.View.ServiceID = pLink.DestinationItem.ServiceID
                                '  Else

                                ' End If
                                If Not IsNothing(oItem.CommunityOwner) Then
                                    CommunityID = oItem.CommunityOwner.Id
                                End If

                                If TypeOf oItem Is ModuleInternalFile Then
                                    Dim oInternal As ModuleInternalFile = DirectCast(oItem, ModuleInternalFile)
                                    If DescriptionOnly Then
                                        Me.View.DisplayItemAction(oItem.DisplayName, oItem.Extension, oItem.Size, oItem.RepositoryItemType)
                                    Else
                                        Me.View.ActionForPlayInternal(pLink.Id, CommunityID, oItem, oInternal.ServiceActionAjax)
                                    End If

                                    If RemoteControl Then
                                        oRemoteControls.Add(New dtoModuleActionControl(Me.View.GetUrlForPlayInternal(pLink.Id, oItem.Id, oItem.UniqueID, CommunityID, oInternal.ServiceActionAjax, oItem.RepositoryItemType), StandardActionType.Play, True))
                                    End If
                                Else
                                    If DescriptionOnly Then
                                        Me.View.DisplayItemAction(oItem.DisplayName, oItem.Extension, oItem.Size, oItem.RepositoryItemType)
                                    Else
                                        Me.View.ActionForPlay(pLink.Id, CommunityID, oItem)
                                    End If

                                    If RemoteControl Then
                                        oRemoteControls.Add(New dtoModuleActionControl(Me.View.GetUrlForPlay(pLink.Id, oItem.Id, oItem.UniqueID, CommunityID, oItem.RepositoryItemType), StandardActionType.Play, True))

                                    End If
                                End If
                                If RemoteControl Then
                                    oRemoteControls.Add(New dtoModuleActionControl(Me.View.GetUrlForSettings(oItem.Id, oItem.RepositoryItemType), StandardActionType.EditMetadata, True))
                                    oRemoteControls.Add(New dtoModuleActionControl(Me.View.GetUrlForAdvancedStatistics(oItem.Id, oItem.RepositoryItemType), StandardActionType.ViewAdvancedStatistics, True))
                                    oRemoteControls.Add(New dtoModuleActionControl(Me.View.GetUrlForPersonalStatistics(oItem.Id, oItem.RepositoryItemType), StandardActionType.ViewPersonalStatistics, True))
                                End If
                            End If
                    

                            'Case UCServices.Services_File.ActionType.UploadFile
                            '    Dim CommunityID As Integer = 0
                            '    If Not IsNothing(oItem.CommunityOwner) Then
                            '        CommunityID = oItem.CommunityOwner.Id
                            '    End If
                            '    Me.View.ActionForUpload(DescriptionOnly, pLink.Id, oItem.Id, oItem.DisplayName, CommunityID)
                            '    If RemoteControl Then
                            '        oRemoteControls.Add(New dtoModuleActionControl(Me.View.GetUrlForUpload(pLink.Id, oItem.Id, CommunityID), True))
                            '    End If
                    End Select
                End If
            Else
                Me.View.DisplayNoAction()
            End If
            Return oRemoteControls
        End Function
    End Class
End Namespace