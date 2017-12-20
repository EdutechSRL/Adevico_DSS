Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.FileRepository.Domain.ScormSettings

Public MustInherit Class FRscormSettingsPageBase
    Inherits FReditViewDetailsPageBase
    Implements IViewScormSettingsBase

#Region "Implements"
    Protected Friend Property IdSettings As Long Implements IViewScormSettingsBase.IdSettings
        Get
            Return ViewStateOrDefault("IdSettings", 0)
        End Get
        Set(value As Long)
            ViewState("IdSettings") = value
        End Set
    End Property
#End Region


#Region "Implements"

#Region "Messages"
    Private Sub DisplayMessage(name As String, extension As String, type As ItemType, messageType As lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain.UserMessageType, Optional status As ItemAvailability = lm.Comol.Core.FileRepository.Domain.ItemAvailability.available) Implements IViewScormSettingsBase.DisplayMessage
        Dim mType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        Dim message As String = Resource.getValue("UserMessageType." & messageType.ToString)
        Dim _onEmptyView As Boolean = False
        Select Case messageType
            Case lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain.UserMessageType.scormSettingsInvalidStatus
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
                message = Resource.getValue("UserMessageType." & messageType.ToString & ".ItemAvailability." & status.ToString)
            Case lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain.UserMessageType.scormSettingsInvalidType, lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain.UserMessageType.scormSettingsNotFound
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain.UserMessageType.scormSettingsSaved
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
            Case lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain.UserMessageType.multimediaSettingsNoPermission
                _onEmptyView = True
        End Select
        message = Replace(message, "#filename#", GetSettingsControl.GetFilenameRender(name, extension, type))
        DisplayMessage(message, mType, _onEmptyView)
    End Sub

    Private Sub DisplayUnknownItem() Implements IViewScormSettingsBase.DisplayUnknownItem
        DisplayMessage(Resource.getValue("IViewScormSettings.DisplayUnknownItem"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert, True)
    End Sub
#End Region

    Private Sub LoadSettings(uniqueId As String, filename As String, settings As dtoScormPackageSettings, isReadOnly As Boolean, isViewMode As Boolean) Implements IViewScormSettingsBase.LoadSettings
        GetSettingsControl.Visible = True
        GetSettingsControl.InitializeControl(uniqueId, filename, settings, isReadOnly, isViewMode)
    End Sub

    Private Function GetSettings() As List(Of dtoScormItemEvaluationSettings) Implements IViewScormSettingsBase.GetSettings
        Return GetSettingsControl.GetSettings
    End Function
    Private Function GetLinkPermissions(link As lm.Comol.Core.DomainModel.liteModuleLink, idUser As Integer) As ItemPermission Implements IViewScormSettingsBase.GetLinkPermissions
        Dim permissions As New ItemPermission()

        Dim oSender As PermissionService.IServicePermission = Nothing
        Try
            oSender = New PermissionService.ServicePermissionClient
            If oSender.AllowStandardAction(StandardActionType.EditMetadata, link.SourceItem, link.DestinationItem, idUser) Then
                permissions.EditSettings = True
                'Else
                '    Dim Permission As Integer = oSender.ModuleLinkActionPermission(IdLink, CoreModuleRepository.ActionType.DownloadFile, linkedObject, idUser, GetExternalUsersLong(), Nothing)
                '    metadataPermission = IIf((UCServices.Services_File.Base2Permission.DownloadFile And Permission), ScormMetadataPermission.view, ScormMetadataPermission.none)
            End If

            If Not IsNothing(oSender) Then
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                If service.State <> ServiceModel.CommunicationState.Faulted AndAlso service.State <> ServiceModel.CommunicationState.Closed Then
                    service.Close()
                End If
                service = Nothing
            End If
        Catch ex As Exception
            If Not IsNothing(oSender) Then
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                If service.State <> ServiceModel.CommunicationState.Faulted AndAlso service.State <> ServiceModel.CommunicationState.Closed Then
                    service.Close()
                End If
                service = Nothing
            End If
        End Try
        Return permissions

    End Function
    Private Function HasPermissionForLink(idUser As Integer, idLink As Long, item As liteRepositoryItem, version As liteRepositoryItemVersion, idModule As Integer, moduleCode As String) As Boolean Implements IViewScormSettingsBase.HasPermissionForLink
        Return False
    End Function
#End Region
    Protected Friend MustOverride Function GetSettingsControl() As UC_ScormSettings
    Protected Friend MustOverride Sub DisplayMessage(message As String, mType As lm.Comol.Core.DomainModel.Helpers.MessageType, onEmptyView As Boolean)
    Protected Friend Sub DisplayInternalMessage(ByVal items As Dictionary(Of ScormSettingsError, Integer), messageType As lm.Comol.Core.DomainModel.Helpers.MessageType)
        Dim message As String = Resource.getValue("UserMessageType." & messageType.ToString)
        Dim subMessages As New List(Of String)
        For Each item As KeyValuePair(Of ScormSettingsError, Integer) In items
            Select Case item.Value
                Case 1
                    subMessages.Add(Resource.getValue("ScormSettingsError." & item.Key.ToString()))
                Case 0
                Case Else
                    subMessages.Add(String.Format(Resource.getValue("ScormSettingsError." & item.Key.ToString() & ".n"), item.Value))
            End Select
        Next
        Select Case subMessages.Count
            Case 0
            Case 1
                message &= subMessages.FirstOrDefault()
            Case Else
                message &= String.Join(", ", subMessages)

        End Select
        DisplayMessage(message, lm.Comol.Core.DomainModel.Helpers.MessageType.alert, False)
    End Sub
End Class