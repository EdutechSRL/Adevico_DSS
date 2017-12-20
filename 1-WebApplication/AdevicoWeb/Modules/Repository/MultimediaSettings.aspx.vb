
Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Public Class MultimediaSettings
    Inherits FReditViewDetailsPageBase
    Implements IViewMultimediaSettings

#Region "Context"
    Private _Presenter As MultimediaSettingsPresenter
    Private ReadOnly Property CurrentPresenter() As MultimediaSettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MultimediaSettingsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private WriteOnly Property AllowSave As Boolean Implements IViewMultimediaSettings.AllowSave
        Set(value As Boolean)
            BTNsaveDefaultDocument.Visible = value
        End Set
    End Property
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        CTRLheader.InitializeHeader(True, lm.Comol.Core.FileRepository.Domain.PresetType.None, New Dictionary(Of PresetType, List(Of ViewOption)), New Dictionary(Of PresetType, List(Of ViewOption)), -2, -2, "IViewMultimediaSettings")
        CurrentPresenter.InitView(PreloadIdLink, PreloadIdItem, PreloadIdVersion, PreloadIdFolder, PreloadIdentifierPath, PreloadSetBackUrl, PreloadBackUrl)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim url As String = DefaultLogoutUrl
        If String.IsNullOrEmpty(url) Then
            url = GetCurrentUrl()
        End If
        RedirectOnSessionTimeOut(url, RepositoryIdCommunity)
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPbackToPreviousUrl, False, True)
            .setButton(BTNsaveDefaultDocument, True)
            Master.ServiceTitle = .getValue("MultimediaSettings.ServiceTitle")
            Master.ServiceTitleToolTip = .getValue("MultimediaSettings.ServiceTitle")
            Master.ServiceNopermission = .getValue("MultimediaSettings.ServiceTitle.NoPermission")
            .setLabel(LBdefaultDocument_t)
            .setLabel(LBdescriptionMultimediaSettings)
        End With
    End Sub
    Protected Friend Overrides Function GetBackUrlItem(Optional top As Boolean = True) As HyperLink
        Return HYPbackToPreviousUrl
    End Function
#End Region

#Region "Implements"
#Region "Messages"
    Private Sub DisplayMessage(name As String, extension As String, type As ItemType, messageType As Domain.UserMessageType, Optional status As ItemAvailability = lm.Comol.Core.FileRepository.Domain.ItemAvailability.available, Optional ByVal defaultDocument As String = "") Implements IViewMultimediaSettings.DisplayMessage
        Dim mType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        Dim message As String = Resource.getValue("UserMessageType." & messageType.ToString)

        Select Case messageType
            Case Domain.UserMessageType.multimediaSettingsInvalidStatus
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
                message = Resource.getValue("UserMessageType." & messageType.ToString & ".ItemAvailability." & status.ToString)
            Case Domain.UserMessageType.multimediaSettingsInvalidType, Domain.UserMessageType.multimediaSettingsNoDefaultDocument
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case Domain.UserMessageType.multimediaSettingsSaved
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        End Select
        message = Replace(message, "#filename#", GetFilenameRender(name, extension, type))
        message = Replace(message, "#defaultdocument#", defaultDocument)
        DisplayMessage(message, mType)
    End Sub
    Private Sub DisplayMessage(name As String, extension As String, type As ItemType, messageType As Domain.UserMessageType, obj As dtoMultimediaFileObject) Implements IViewMultimediaSettings.DisplayMessage
        Dim mType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        Dim message As String = Resource.getValue("UserMessageType." & messageType.ToString)

        Select Case messageType
            Case Domain.UserMessageType.multimediaSettingsInvalidType, Domain.UserMessageType.multimediaSettingsNoDefaultDocument
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case Domain.UserMessageType.multimediaSettingsSaved
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
                LBdefaultDocument.Text = GetFilenameRender(obj.DisplayName, obj.Extension, lm.Comol.Core.FileRepository.Domain.ItemType.File)
                If Not String.IsNullOrWhiteSpace(obj.Path) Then
                    LBdefaultDocumentPath.Visible = True
                    LBdefaultDocumentPath.Text = "( " & obj.Path & IIf(obj.Path.EndsWith("\"), "", "\") & " )"
                Else
                    LBdefaultDocumentPath.Visible = False
                End If
        End Select
        message = Replace(message, "#filename#", GetFilenameRender(name, extension, type))
        message = Replace(message, "#defaultdocument#", obj.Fullname)
        DisplayMessage(message, mType)
    End Sub
    Private Sub DisplayUnknownItem() Implements IViewMultimediaSettings.DisplayUnknownItem
        DisplayMessage(Resource.getValue("IViewMultimediaSettings.DisplayUnknownItem"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
        MLVcontent.SetActiveView(VIWempty)
    End Sub
#End Region

    Private Sub LoadItems(uniqueId As String, filename As String, items As List(Of dtoMultimediaFileObject), selectedItem As dtoMultimediaFileObject) Implements IViewMultimediaSettings.LoadItems
        If Not IsNothing(selectedItem) Then
            LBdefaultDocument.Text = GetFilenameRender(selectedItem.DisplayName, selectedItem.Extension, lm.Comol.Core.FileRepository.Domain.ItemType.File)
            If Not String.IsNullOrWhiteSpace(selectedItem.Path) Then
                LBdefaultDocumentPath.Visible = True
                LBdefaultDocumentPath.Text = "( " & selectedItem.Path & IIf(selectedItem.Path.EndsWith("\"), "", "\") & " )"
            Else
                LBdefaultDocumentPath.Visible = False
            End If
            HDNidSelectedFile.Value = "file-" & selectedItem.Id.ToString
        Else
            HDNidSelectedFile.Value = ""
            Resource.setLabel(LBdefaultDocument)
            LBdefaultDocumentPath.Visible = False
        End If
        RenderTree(uniqueId, filename, items)
    End Sub
    Private Function GetLinkPermissions(link As lm.Comol.Core.DomainModel.liteModuleLink, idUser As Integer) As ItemPermission Implements IViewMultimediaSettings.GetLinkPermissions
        Dim permissions As New ItemPermission()

        Dim oSender As PermissionService.IServicePermission = Nothing
        Try
            oSender = New PermissionService.ServicePermissionClient
            If oSender.AllowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.EditMetadata, link.SourceItem, link.DestinationItem, idUser) Then
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

    Private Function HasPermissionForLink(idUser As Integer, idLink As Long, item As liteRepositoryItem, version As liteRepositoryItemVersion, idModule As Integer, moduleCode As String) As Boolean Implements IViewMultimediaSettings.HasPermissionForLink
        Return False
    End Function
#End Region

#Region "Internal"

#Region "Render Tree"
    Private Sub RenderTree(versionIdentifier As String, filename As String, items As List(Of dtoMultimediaFileObject))
        Dim render As String = LTtreeRoot.Text
        render = Replace(render, "#uniqueIdVersion#", versionIdentifier)
        LTcookieTemplate.Text = Replace(LTcookieTemplate.Text, "#uniqueIdVersion#", versionIdentifier)
        Dim root As dtoNode = CreateTree(versionIdentifier, filename, items)
        render = Replace(render, "#nodes#", GenerateFolderNode(0, filename, root.Nodes.Any))
        If root.Nodes.Any() Then
            render = Replace(render, "#childrennodes#", RenderTree(root.Nodes))
        End If
        LTrenderTree.Text = render
        SetFoldersCookies(root.GetAllNodes().Where(Function(n) n.HasSelected).ToList())
    End Sub
    Private Function RenderTree(nodes As List(Of dtoNode)) As String
        Dim render As String = ""
        Dim nodeRender As String = ""
        For Each node As dtoNode In nodes
            nodeRender = ""
            If node.IsFolder Then
                nodeRender = LTtreeFolderNode.Text
                nodeRender = Replace(nodeRender, "#idItem#", node.IdNode)
                nodeRender = Replace(nodeRender, "#foldername#", node.Name)
                If Not node.HasSelected AndAlso node.Level > 3 Then
                    nodeRender = Replace(nodeRender, " autoOpen", "")
                ElseIf node.HasSelected Then
                    nodeRender = Replace(nodeRender, " autoOpen", " autoOpen")
                End If
                If node.Nodes.Any() Then
                    nodeRender = Replace(nodeRender, "#childrennodes#", RenderTree(node.Nodes))
                Else
                    nodeRender = Replace(nodeRender, "#childrennodes#", "")
                End If
                render &= nodeRender
            Else
                nodeRender = LTtreeFileNode.Text
                nodeRender = Replace(nodeRender, "#idItem#", node.IdNode)
                nodeRender = Replace(nodeRender, "#name#", node.Name)
                nodeRender = Replace(nodeRender, "#extension#", node.Extension)
                nodeRender = Replace(nodeRender, "#ico#", node.Extension)
                If node.IsCurrent Then
                    nodeRender = Replace(nodeRender, "#active#", " active")
                Else
                    nodeRender = Replace(nodeRender, "#active#", "")
                End If
                render &= nodeRender
            End If
        Next
        Return Replace(LTtreeChildrenNodes.Text, "#childrennodes#", render)
    End Function
    Private Function GenerateFolderNode(idItem As Long, name As String, hasChildren As Boolean) As String
        Dim node As String = LTtreeFolderNode.Text
        node = Replace(node, "#foldername#", name)
        node = Replace(node, "#idItem#", idItem.ToString)
        If Not hasChildren Then
            node = Replace(node, "#childrennodes#", "")
        End If
        Return node
    End Function
    Private Sub SetFoldersCookies(nodes As List(Of dtoNode))
        For Each node As dtoNode In nodes
            Dim oCookie As HttpCookie = Request.Cookies(String.Format(LTcookieTemplate.Text, "folder-" & node.IdNode))
            If IsNothing(oCookie) Then
                oCookie = New HttpCookie(String.Format(LTcookieTemplate.Text, "folder-" & node.IdNode))
                oCookie.Value = Boolean.FalseString.ToLower()
                Response.Cookies.Add(oCookie)
            Else
                oCookie.Value = Boolean.FalseString.ToLower()
            End If
        Next
    End Sub
#End Region

#Region "Create Tree"
    Private Function CreateTree(versionIdentifier As String, filename As String, items As List(Of dtoMultimediaFileObject)) As dtoNode
        Dim root As dtoNode = New dtoNode()
        root.IdNode = versionIdentifier
        root.Name = filename
        root.FullName = "root"
        root.IsRoot = True
        root.IsFolder = True
        root.FullName = ""
        Dim node As dtoNode = root
        Dim index As Long = -1
        Dim relativePath As String = ""

        If Not IsNothing(items) AndAlso items.Any() Then
            Dim folders As List(Of String) = items.Select(Function(i) i.Path).Distinct().Where(Function(i) Not String.IsNullOrWhiteSpace(i)).OrderBy(Function(i) i).ToList
            For Each folder As String In folders
                node = root
                relativePath = "\"
                For Each value As String In folder.Split("\")
                    relativePath &= value
                    node = AddFolder(versionIdentifier, node, value, relativePath, index)
                    relativePath &= "\"
                Next
            Next
            Dim allNodes As List(Of dtoNode) = root.GetAllNodes()
            For Each obj As dtoMultimediaFileObject In items.OrderBy(Function(i) i.Fullname)
                If String.IsNullOrWhiteSpace(obj.Path) Then
                    node = root
                Else
                    node = allNodes.Where(Function(n) n.FullName = "\" & obj.Path()).FirstOrDefault()
                End If
                If Not IsNothing(node) Then
                    AddNode(node, obj)
                Else
                    node = node
                End If
            Next
        End If
        Return root
    End Function
    Private Function AddFolder(versionIdentifier As String, node As dtoNode, value As String, fullpath As String, ByRef index As Long) As dtoNode
        Dim item As dtoNode = node.FindNodeByFullName(fullpath)

        If (Not IsNothing(item)) Then
            Return item
        Else
            item = New dtoNode
            item.IdNode = versionIdentifier & "_" & index
            item.IsFolder = True
            item.Name = value
            item.FullName = node.FullName & "\" & value
            item.Level = node.Level + 1
            index -= 1

            node.Nodes.Add(item)
            Return item
        End If
    End Function
    Private Sub AddNode(node As dtoNode, obj As dtoMultimediaFileObject)
        Dim item As dtoNode = New dtoNode
        item.IdNode = obj.Id
        item.Extension = obj.Extension
        item.IdNode = obj.Id
        item.Name = obj.DisplayName
        item.IsCurrent = obj.IsDefaultDocument
        item.Level = node.Level + 1
        node.Nodes.Add(item)
    End Sub
    Protected Friend Class dtoNode
        Public IdNode As String
        Public IsRoot As Boolean
        Public IsFolder As Boolean
        Public IsCurrent As Boolean
        Public Name As String
        Public Extension As String
        Public FullName As String
        Public Nodes As List(Of dtoNode)
        Public Level As Integer
        Public Sub New()
            Nodes = New List(Of dtoNode)
        End Sub

        'Public Function FindNodeByText(ByVal key As String) As dtoNode
        '    Dim result As dtoNode = Nothing
        '    If FullName = key Then
        '        result = Me
        '    ElseIf Nodes.Any Then
        '        result = Nodes.Where(Function(n) n.FullName = key).FirstOrDefault
        '        If IsNothing(result) Then
        '            result = Nodes.Where(Function(n) Not IsNothing(n.FindNodeByText(key))).FirstOrDefault()
        '        End If
        '    End If
        '    Return result
        'End Function
        Public Function FindNodeByFullName(ByVal key As String) As dtoNode
            Dim result As dtoNode = Nothing
            If FullName = key Then
                result = Me
            ElseIf Nodes.Any Then
                result = Nodes.Where(Function(n) n.FullName = key).FirstOrDefault
                If IsNothing(result) Then
                    result = Nodes.Select(Function(n) n.FindNodeByFullName(key)).Where(Function(n) Not IsNothing(n)).FirstOrDefault()
                End If
            End If
            Return result
        End Function
        Public Function GetAllNodes() As List(Of dtoNode)
            Dim items As New List(Of dtoNode)
            items.Add(Me)
            If Nodes.Any() Then
                items.AddRange(Nodes.SelectMany(Function(n) n.GetAllNodes()).ToList())
            End If
            Return items
        End Function
        Public Function HasSelected() As Boolean
            Return IsCurrent OrElse Nodes.Any(Function(n) n.HasSelected)
        End Function
        Public Function ToString() As String
            Return IdNode & " IsFolder:" & IsFolder.ToString & " Name:" & Name & " Extension:" & Extension & " FullName:" & FullName
        End Function
    End Class
#End Region
    Private Function GetFilenameRender(fullname As String, fileExtension As String, type As ItemType) As String
        Dim template As String = LTtemplateFile.Text
        Select Case type
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Folder
                template = Replace(template, "#ico#", LTitemFolderCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Link
                template = Replace(template, "#ico#", LTitemUrlCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.Multimedia
                template = Replace(template, "#ico#", LTitemMultimediaCssClass.Text)
            Case lm.Comol.Core.FileRepository.Domain.ItemType.ScormPackage
                template = Replace(template, "#ico#", LTitemScormPackageCssClass.Text)
            Case Else
                If Not String.IsNullOrWhiteSpace(fileExtension) Then
                    fileExtension = fileExtension.ToLower
                End If
                If fileExtension.StartsWith(".") Then
                    template = Replace(template, "#ico#", LTitemExtensionCssClass.Text & Replace(fileExtension, ".", ""))
                Else
                    template = Replace(template, "#ico#", LTitemExtensionCssClass.Text)
                End If
        End Select
        template = Replace(template, "#name#", fullname)
        Return template
    End Function
    Private Sub DisplayMessage(ByVal message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType)
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(message, type)
    End Sub
    Private Sub BTNsaveDefaultDocument_Click(sender As Object, e As EventArgs) Handles BTNsaveDefaultDocument.Click
        If Not String.IsNullOrWhiteSpace(HDNidSelectedFile.Value) Then
            Dim idDocument As Long = 0
            Long.TryParse(Replace(HDNidSelectedFile.Value, "file-", ""), idDocument)
            If isValidOperation() Then
                CurrentPresenter.SetDefaultDocument(IdItem, IdVersion, idDocument)
            End If
        End If
    End Sub
#End Region

    Private Sub RepositoryItemEdit_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub


   
End Class