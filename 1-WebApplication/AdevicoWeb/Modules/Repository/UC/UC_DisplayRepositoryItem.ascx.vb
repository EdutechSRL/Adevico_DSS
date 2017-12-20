Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Repository
Imports lm.Comol.Core.BaseModules.Repository.Presentation
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel.Helpers

Public Class UC_DisplayRepositoryItem
    Inherits BaseControl
    Implements IViewDisplayRepositoryItem



#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
    Private _Presenter As DisplayRepositoryItemPresenter

    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As DisplayRepositoryItemPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DisplayRepositoryItemPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IconSize As IconSize Implements IViewDisplayRepositoryItem.IconSize
        Get
            Return ViewStateOrDefault("IconSize", IconSize.Medium)
        End Get
        Set(value As IconSize)
            ViewState("IconSize") = value
        End Set
    End Property
    Private Property AvailableCommands As ItemAvailableCommand Implements IViewDisplayRepositoryItem.AvailableCommands
        Get
            Return ViewStateOrDefault("AvailableCommands", ItemAvailableCommand.Download)
        End Get
        Set(value As ItemAvailableCommand)
            ViewState("AvailableCommands") = value
        End Set
    End Property

    Public Property DescriptionDisplayMode As ItemDescriptionDisplayMode Implements IViewDisplayRepositoryItem.DescriptionDisplayMode
        Get
            Return ViewStateOrDefault("DescriptionDisplayMode", ItemDescriptionDisplayMode.None)
        End Get
        Set(value As ItemDescriptionDisplayMode)
            ViewState("DescriptionDisplayMode") = value
        End Set
    End Property
    Public Property DisplayView As ItemDisplayView Implements IViewDisplayRepositoryItem.DisplayView
        Get
            Return ViewStateOrDefault("DisplayView", ItemDisplayView.multilineFull)
        End Get
        Set(value As ItemDisplayView)
            ViewState("DisplayView") = value
            'Me.DVmultiline.Visible = (value = ItemDisplayView.Multiline)
            'Me.DVsingleline.Visible = (value = ItemDisplayView.Singleline)
        End Set
    End Property

    Private Property FolderNavigationUrl As String Implements IViewDisplayRepositoryItem.FolderNavigationUrl
        Get
            Return ViewStateOrDefault("FolderNavigationUrl", "")
        End Get
        Set(value As String)
            ViewState("FolderNavigationUrl") = value
        End Set
    End Property
    Public Property DisplayMode As ItemDisplayMode Implements IViewDisplayRepositoryItem.DisplayMode
        Get
            Return ViewStateOrDefault("DisplayMode", ItemDisplayMode.inline)
        End Get
        Set(value As ItemDisplayMode)
            ViewState("DisplayMode") = value
            Me.MLVdisplayMode.ActiveViewIndex = CInt(value)
        End Set
    End Property
    Public Property DisplayCommandText As ItemAvailableCommand Implements IViewDisplayRepositoryItem.DisplayCommandText
        Get
            Return ViewStateOrDefault("DisplayCommandText", ItemAvailableCommand.None)
        End Get
        Set(value As ItemAvailableCommand)
            ViewState("DisplayCommandText") = value
        End Set
    End Property
#End Region

#Region "Property"
    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
    Private _BaseUrlNoSSL As String
    Private Overloads ReadOnly Property BaseUrlNoSSL() As String
        Get
            If _BaseUrlNoSSL = "" Then
                _BaseUrlNoSSL = Me.PageUtility.ApplicationUrlBase()
                If Not _BaseUrlNoSSL.EndsWith("/") Then
                    _BaseUrlNoSSL &= "/"
                End If
            End If
            Return _BaseUrlNoSSL
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ModuleToRepository", "Modules", "Repository")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Initialize"
    Public Sub InitializeControl(item As dtoDisplayItemRepository, currentUrl As String, IdModule As Integer, IdCommunity As Integer, IdAction As Integer) Implements IViewDisplayRepositoryItem.InitializeControl
        CurrentPresenter.InitView(item, currentUrl, IdModule, IdCommunity, IdAction)
    End Sub
    Public Sub InitializeControl(item As dtoDisplayItemRepository, currentUrl As String, IdModule As Integer, IdCommunity As Integer, IdAction As Integer, view As ItemDisplayView, mode As ItemDisplayMode, descriptionDisplay As ItemDescriptionDisplayMode, commands As ItemAvailableCommand) Implements IViewDisplayRepositoryItem.InitializeControl
        CurrentPresenter.InitView(item, currentUrl, IdModule, IdCommunity, IdAction, view, mode, descriptionDisplay, commands)
    End Sub
    Public Sub InitializeControlForFolder(item As dtoDisplayItemRepository, url As String, IdModule As Integer, IdCommunity As Integer) Implements IViewDisplayRepositoryItem.InitializeControlForFolder
        FolderNavigationUrl = url
        CurrentPresenter.InitView(item, "", IdModule, IdCommunity, CoreModuleRepository.ActionType.ViewFolder)
    End Sub
    Public Sub InitializeControlForFolder(item As dtoDisplayItemRepository, url As String, IdModule As Integer, IdCommunity As Integer, view As ItemDisplayView, mode As ItemDisplayMode, descriptionDisplay As ItemDescriptionDisplayMode, commands As ItemAvailableCommand) Implements IViewDisplayRepositoryItem.InitializeControlForFolder
        FolderNavigationUrl = url
        CurrentPresenter.InitView(item, "", IdModule, IdCommunity, CoreModuleRepository.ActionType.ViewFolder, view, mode, descriptionDisplay, commands)
    End Sub
#End Region

#Region "display"
    Public Sub DisplayDeletedFile(displayName As String, type As RepositoryItemType, extension As String) Implements IViewDisplayRepositoryItem.DisplayDeletedFile
        Dim itemName As String = displayName
        If DisplayView <> ItemDisplayView.multilineFull AndAlso itemName.Length > 30 Then
            itemName = itemName.Substring(0, 13) & "...." & itemName.Substring(itemName.Length - 14)
        End If

        Dim fileLink As String = ""
        If type = RepositoryItemType.Multimedia OrElse type = RepositoryItemType.ScormPackage OrElse type = RepositoryItemType.VideoStreaming Then
            fileLink = "<div class=""File_NameExt""><span class=""File_Name"" title=""{0}"">{1} <span>{2}</span></span></div>"
            LTitem.Text = String.Format(fileLink, displayName, IconCommandsHelper.RenderSpan(displayName, IconCommandsHelper.GetDefaultIcon(type), IconSize, "File_Ext"), itemName)

        Else
            Dim itemIco As String = Me.BaseUrl & IIf(type <> RepositoryItemType.Folder, Me.PageUtility.SystemSettings.Extension.FindIconImage(extension), "RadControls/TreeView/Skins/Materiale/folder.gif")
            fileLink = "<div class=""File_NameExt""><span class=""File_Name"" title=""{0}""><img src=""{1}"" class=""File_Ext""/>  <span>{2}</span></span></div>"
            LTitem.Text = String.Format(fileLink, displayName, itemIco, itemName)
        End If
    End Sub
    Public Sub DisplayUnknownItem() Implements IViewDisplayRepositoryItem.DisplayUnknownItem

    End Sub
    Public Sub DisplayItemName(item As dtoDisplayItemRepository) Implements IViewDisplayRepositoryItem.DisplayItemName
        LTitem.Text = RenderItemName(DisplayView, item)
    End Sub
    Public Sub DisplayFolder(item As dtoDisplayItemRepository, url As String, IdModule As Integer, IdCommunity As Integer, IdAction As Integer) Implements IViewDisplayRepositoryItem.DisplayFolder
        LTitem.Text = RenderItemFileLink(DisplayView, item, url, IdModule, IdCommunity, IdAction)
    End Sub
    Public Sub DisplayItem(item As dtoDisplayItemRepository, currentUrl As String, IdModule As Integer, IdCommunity As Integer, IdAction As Integer) Implements IViewDisplayRepositoryItem.DisplayItem
        LTitem.Text = RenderItem(DisplayView, item, currentUrl, "", IdModule, IdCommunity, IdAction)
    End Sub
    Public Sub DisplayMultimediaItem(item As dtoDisplayItemRepository, currentUrl As String, defaultUrl As String, IdModule As Integer, IdCommunity As Integer, IdAction As Integer) Implements IViewDisplayRepositoryItem.DisplayMultimediaItem
        LTitem.Text = RenderItem(DisplayView, item, currentUrl, defaultUrl, IdModule, IdCommunity, IdAction)
    End Sub
#End Region

#Region "Render"
    Private Function RenderItemName(display As ItemDisplayView, dto As dtoDisplayItemRepository) As String
        Dim renderItem As String = ""
        Dim item As BaseCommunityFile = dto.File
        Dim itemName As String = IIf(item.RepositoryItemType = RepositoryItemType.FileStandard, dto.DisplayName, item.Name)
        Dim title As String = ""

        Dim itemIco As String = ""
        If display <> ItemDisplayView.multilineFull AndAlso itemName.Length > 40 Then
            itemName = itemName.Substring(0, 18) & "...." & itemName.Substring(itemName.Length - 19)
        End If

        renderItem = "<span class=""File_Name"" title=""{0}"">{1} <span>{2}</span></span>"
        If item.RepositoryItemType = RepositoryItemType.Multimedia OrElse item.RepositoryItemType = RepositoryItemType.ScormPackage OrElse item.RepositoryItemType = RepositoryItemType.VideoStreaming Then
            itemIco = IconCommandsHelper.RenderSpan(title, item.RepositoryItemType, IconSize, "File_Ext")
        Else
            Dim img As String = "<img src=""{0}"" class=""File_Ext""/>"
            itemIco = String.Format(img, Me.BaseUrl & IIf(item.isFile, Me.PageUtility.SystemSettings.Extension.FindIconImage(item.Extension), "RadControls/TreeView/Skins/Materiale/folder.gif"))
        End If
        Return String.Format(renderItem, title, itemIco, itemName)
    End Function

    Private Function RenderItemFileLink(display As ItemDisplayView, dto As dtoDisplayItemRepository, defaultUrl As String, IdModule As Integer, IdCommunity As Integer, IdAction As Integer) As String
        Dim renderItem As String = ""
        Dim item As BaseCommunityFile = dto.File
        Dim itemName As String = dto.DisplayName
        Dim title As String = ""
        Dim itemUrl As String = ""

        Dim itemIco As String = Me.BaseUrl & IIf(item.isFile, Me.PageUtility.SystemSettings.Extension.FindIconImage(item.Extension), "RadControls/TreeView/Skins/Materiale/folder.gif")
        itemUrl = IIf(item.isFile, "File.repository?FileID=" & item.Id.ToString & "&ForUserID=" & Me.CurrentContext.UserContext.CurrentUserID.ToString & "&Language=" & PageUtility.LinguaCode, BaseUrl & defaultUrl)

        If display <> ItemDisplayView.multilineFull AndAlso itemName.Length > 40 Then
            itemName = itemName.Substring(0, 18) & "...." & itemName.Substring(itemName.Length - 19)
        End If

        If item.isFile Then
            Dim fileLink As String = ""
            title = IIf(display = ItemDisplayView.multilineFull, Resource.getValue("fileTitle"), Resource.getValue("fileTitle") & " " & dto.DisplayName)
            If item.RepositoryItemType = RepositoryItemType.FileStandard Then
                If dto.Permission.Download Then
                    fileLink = "<div class=""File_NameExt"">" & vbCrLf & "<a href=""{0}"" class=""File_Name ROW_ItemLink_Small "" title=""{1}""><img src=""{2}"" class=""File_Ext""/> <span>{3}</span></a>" & vbCrLf & "</div>" & vbCrLf
                    'fileRepositoryCookie
                    Return String.Format(fileLink, itemUrl, title, itemIco, itemName)
                Else
                    fileLink = "<div class=""File_NameExt"">" & vbCrLf & " <span class=""File_Name"" title=""{0}""><img src=""{1}"" class=""File_Ext""/> <span>{2}</span></span>" & vbCrLf & "</div>" & vbCrLf
                    Return String.Format(fileLink, title, itemIco, itemName)
                End If
            ElseIf dto.Permission.Play Then
                Dim url As String = ""
                Dim BasePlayUrl As String = ""
                'If SystemSettings.Icodeon.OverrideSSLsettings Then
                BasePlayUrl = Me.BaseUrlNoSSL
                'Else
                '    BasePlayUrl = Me.BaseUrl
                'End If
                Select Case item.RepositoryItemType
                    Case RepositoryItemType.ScormPackage
                        url = ""
                    Case RepositoryItemType.Multimedia
                        url = BasePlayUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.PlayMultimediaFile(item.Id, item.UniqueID, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, False, PreLoadedContentView)
                End Select
                fileLink = "<a href=""{0}"" class=""File_Name ROW_ItemLink_Small fileRepositoryCookieNoBlockUI"" title=""{1}""  target=""_blank"">{2} <span>{3}</span></a>" & vbCrLf & "</div>" & vbCrLf

                title = Me.Resource.getValue("action.RepositoryItemType." & item.RepositoryItemType.ToString) & " " & item.Name
                Return String.Format(fileLink, url, title, IconCommandsHelper.RenderSpan(title, item.RepositoryItemType, IconSize, "File_Ext"), item.Name)
            Else
                title = Me.Resource.getValue("action.RepositoryItemType." & item.RepositoryItemType.ToString) & " " & item.Name

                fileLink = "<div class=""File_NameExt"">" & vbCrLf & "<span class=""File_Name"" title=""{0}"">{1} <span>{2}</span></span>" & vbCrLf & "</div>" & vbCrLf
                Return String.Format(fileLink, title, IconCommandsHelper.RenderSpan(title, item.RepositoryItemType, IconSize, "File_Ext"), itemName)
            End If
        Else
            renderItem = "<a href=""{0}"" class=""File_Name ROW_ItemLink_Small"" title=""{1}""><img src=""{2}"" class=""File_Ext""/> <span>{3}</span></a>" & vbCrLf
            title = itemName
            renderItem = String.Format(renderItem, itemUrl, title, itemIco, itemName)

            If PermissionHelper.CheckPermissionSoft(ItemDescriptionDisplayMode.Show, DescriptionDisplayMode) AndAlso Not String.IsNullOrEmpty(dto.File.Description) Then
                renderItem &= "<div class=""File_Detail ToolTip"">"
                renderItem &= vbCrLf & dto.File.Description & vbCrLf & "</div>" & vbCrLf
            End If
            Return renderItem
        End If
    End Function

    Private Function RenderActions(display As ItemDisplayView, dto As dtoDisplayItemRepository, currentUrl As String, IdModule As Integer, IdCommunity As Integer) As String
        Dim permission As RepositoryItemPermission = dto.Permission
        Dim file As BaseCommunityFile = dto.File

        Dim result As String = ""

        Dim DestinationUrl As String = currentUrl
        If String.IsNullOrEmpty(DestinationUrl) Then
            DestinationUrl = Request.Url.LocalPath
            If Me.BaseUrl <> "/" Then
                DestinationUrl = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
            End If
            DestinationUrl = Server.UrlEncode(DestinationUrl & Request.Url.Query)
        Else
            DestinationUrl = Server.UrlEncode(currentUrl)
        End If


        If permission.Download AndAlso PermissionHelper.CheckPermissionSoft(ItemAvailableCommand.Download, AvailableCommands) Then
            Dim title As String = Resource.getValue("fileTitle") & " " & IIf(file.RepositoryItemType = RepositoryItemType.FileStandard, file.DisplayName, file.Name) & " (" & GetFileSize(file.Size) & ")."

            result &= "<li>" & vbCrLf
            result &= IconCommandsHelper.RenderActionlLink(lm.Comol.Core.BaseModules.Repository.Domain.RootObject.DownloadFile(file.Id, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, PageUtility.UniqueGuidSession), _
                                                    title, IconCommandsHelper.LinkTarget._blank,
                                                  IconCommandsHelper.CommandIcon.DownloadItem, IconSize, "File_ImgLnk File_Img_Action ") 'fileRepositoryCookie

            result &= "</li>" & vbCrLf
        Else
            result &= "<li>&nbsp;</li>" & vbCrLf
        End If
        If (permission.ViewBaseStatistics OrElse permission.ViewAdvancedStatistics) AndAlso PermissionHelper.CheckPermissionSoft(ItemAvailableCommand.Statistics, AvailableCommands) Then
            Dim url As String = ""
            Select Case file.RepositoryItemType
                Case RepositoryItemType.ScormPackage
                    If permission.ViewAdvancedStatistics Then
                        url = Me.BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.ManagementScormStatistics(file.Id, DestinationUrl, PreLoadedContentView)
                    ElseIf permission.ViewBaseStatistics Then
                        url = Me.BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.UserScormStatistics(file.Id, DestinationUrl, PreLoadedContentView)
                    End If
            End Select

            If Not String.IsNullOrEmpty(url) Then
                result &= "<li>" & vbCrLf
                result &= IconCommandsHelper.RenderActionlLink(url, Me.Resource.getValue("statistic.RepositoryItemType." & file.RepositoryItemType.ToString),
                                                      IconCommandsHelper.CommandIcon.Statistics, IconSize, "File_Img_Action")

                result &= "</li>" & vbCrLf
            End If
        Else
            result &= "<li>&nbsp;</li>" & vbCrLf
        End If
        '    If (permission.ViewBaseStatistics OrElse permission.ViewAdvancedStatistics) AndAlso PermissionHelper.CheckPermissionSoft(ItemAvailableCommand.Statistics, AvailableCommands) Then
        '        Dim url As String = ""

        '        result &= "<li>" & vbCrLf
        '        Select Case file.RepositoryItemType
        '            Case RepositoryItemType.ScormPackage

        '                If permission.ViewAdvancedStatistics Then
        '                    url = Me.BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.ManagementScormStatistics(file.Id, DestinationUrl, PreLoadedContentView)
        '                    '"Modules/Scorm/ScormStatisticheMain.aspx?FileID=" & oCommunityFile.Id.ToString & "&BackUrl=" & DestinationUrl
        '                ElseIf permission.ViewBaseStatistics Then
        '                    url = Me.BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.UserScormStatistics(file.Id, DestinationUrl, PreLoadedContentView)
        '                    '"Modules/Scorm/ScormStatisticheUtente.aspx?FileID=" & oCommunityFile.Id.ToString & "&BackUrl=" & DestinationUrl
        '                End If
        '        End Select

        '        If Not String.IsNullOrEmpty(url) Then
        '            result &= "<a href=""" & url & """ class=""File_ImgLnk"">" & vbCrLf
        '            result &= "<img src=""" & BaseUrl & "images/DefaultCommand/stat24.png"" alt=""" & Me.Resource.getValue("statistic.RepositoryItemType." & file.RepositoryItemType.ToString) & """ class=""File_Img_Action"" title=""" & Me.Resource.getValue("statistic.RepositoryItemType." & file.RepositoryItemType.ToString) & """/>"
        '            result &= "</a>" & vbCrLf
        '            result &= "</li>" & vbCrLf
        '        End If

        '    End If
        If permission.EditSettings AndAlso PermissionHelper.CheckPermissionSoft(ItemAvailableCommand.Settings, AvailableCommands) Then
            Dim url As String = ""

            Select Case file.RepositoryItemType
                Case RepositoryItemType.ScormPackage
                    url = BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.EditScormPackageSettings(file.Id, DestinationUrl, PreLoadedContentView)
                Case RepositoryItemType.VideoStreaming
                Case RepositoryItemType.Multimedia
                    url = BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.EditMultimediaFileSettings(file.Id, DestinationUrl, PreLoadedContentView)
            End Select
            If Not String.IsNullOrEmpty(url) Then  'AndAlso Not String.IsNullOrEmpty(imageUrl)
                result &= "<li>" & vbCrLf

                result &= IconCommandsHelper.RenderActionlLink(url, Me.Resource.getValue("settings.RepositoryItemType." & file.RepositoryItemType.ToString),
                                                        IconCommandsHelper.CommandIcon.Settings, IconSize, "File_Img_Action")

                result &= "</li>" & vbCrLf
            End If
        Else
            result &= "<li>&nbsp;</li>" & vbCrLf
        End If
        '    If permission.EditSettings AndAlso PermissionHelper.CheckPermissionSoft(ItemAvailableCommand.Settings, AvailableCommands) Then
        '        Dim url As String = ""
        '        Dim imageUrl As String = ""
        '        result &= "<li>" & vbCrLf
        '        Select Case file.RepositoryItemType
        '            Case RepositoryItemType.ScormPackage
        '                imageUrl = "setting24.png"
        '                url = BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.EditScormPackageSettings(file.Id, DestinationUrl, PreLoadedContentView)
        '            Case RepositoryItemType.Multimedia
        '                imageUrl = "setting24.png"
        '                url = BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.EditMultimediaFileSettings(file.Id, DestinationUrl, PreLoadedContentView)
        '            Case RepositoryItemType.VideoStreaming
        '                imageUrl = "setting24.png"
        '        End Select
        '        If Not String.IsNullOrEmpty(url) AndAlso Not String.IsNullOrEmpty(imageUrl) Then
        '            result &= "<a href=""" & url & """ class=""File_ImgLnk"">" & vbCrLf
        '            result &= "<img src=""" & BaseUrl & "images/DefaultCommand/" & imageUrl & """ alt=""" & Me.Resource.getValue("settings.RepositoryItemType." & file.RepositoryItemType.ToString) & """ class=""File_Img_Action"" title=""" & Me.Resource.getValue("settings.RepositoryItemType." & file.RepositoryItemType.ToString) & """/>"
        '            result &= "</a>" & vbCrLf
        '            result &= "</li>" & vbCrLf
        '        End If
        '    End If
        Return result
    End Function


    Private Function RenderItem(display As ItemDisplayView, dto As dtoDisplayItemRepository, currentUrl As String, defaultUrl As String, IdModule As Integer, IdCommunity As Integer, IdAction As Integer) As String
        Dim result As String = "<div class=""File_Main"">" & vbCrLf & "<div class="""
        Select Case display
            Case ItemDisplayView.singleline
                result &= "File_MainInfo_TinyCut"">"
            Case ItemDisplayView.multilineFull
                result &= "File_MainInfo"">"
            Case ItemDisplayView.multiline
                result &= "File_MainInfo_Normal"">"
        End Select

        result &= vbCrLf & RenderItemFileLink(display, dto, defaultUrl, IdModule, IdCommunity, IdAction)
        Dim render As String = ""
        If dto.File.isFile Then
            'If display = ItemDisplayView.multiline Then
            '    result &= "<br/><div class=""File_SMInfo_Actions File_Download"">"
            '    result &= "<span class=""Titolo_campoSmall"">" & Me.GetFileSize(dto.File.Size) & "</span> - "
            '    If dto.Permission.Edit OrElse dto.Permission.Delete OrElse dto.Permission.UnDelete OrElse dto.Permission.VirtualDelete OrElse dto.Permission.ViewAdvancedStatistics Then
            '        result &= "<span class=""Titolo_campoSmall"">" & dto.File.Downloads & " " & Resource.getValue("downloadInfo") & "</span>"
            '    End If

            '    If PermissionHelper.CheckPermissionSoft(ItemDescriptionDisplayMode.AsTooltip, DescriptionDisplayMode) AndAlso Not String.IsNullOrEmpty(dto.File.Description) Then
            '        result &= "<span class=""File_Detail ToolTip"" title=""" & dto.File.Description & """>" & Resource.getValue("descriptionInfo") & "</span>"
            '    End If
            '    result &= "</div>" & vbCrLf
            'End If
            result &= "<span>" & vbCrLf
            result &= "<span class=""Titolo_campoSmall"">" & Me.GetFileSize(dto.File.Size) & "</span>" & vbCrLf
            If dto.Permission.Edit OrElse dto.Permission.Delete OrElse dto.Permission.UnDelete OrElse dto.Permission.VirtualDelete OrElse dto.Permission.ViewAdvancedStatistics Then
                result &= "<span class=""Titolo_campoSmall""> " & dto.File.Downloads & " " & Resource.getValue("downloadInfo") & "</span>" & vbCrLf
            End If
            result &= "</span>" & vbCrLf

            'result &= "</div>" & vbCrLf
            result &= "</div>" & vbCrLf & "</div>" & vbCrLf
            Select Case display
                Case ItemDisplayView.singleline
                    If Me.AvailableCommands <> ItemAvailableCommand.None Then
                        result &= "<div class=""File_SMInfo_Actions" > "" & vbCrLf
                        result &= "<ul class=""File_Actions"">" & vbCrLf
                        result &= RenderActions(display, dto, currentUrl, IdModule, IdCommunity)

                        'result &= "<li>"
                        'result &= "<a href=""#"" title=""(" & Me.GetFileSize(dto.File.Size) & ""
                        'If dto.Permission.Edit OrElse dto.Permission.Delete OrElse dto.Permission.UnDelete OrElse dto.Permission.VirtualDelete OrElse dto.Permission.ViewAdvancedStatistics Then
                        '    result &= "- " & dto.File.Downloads & " " & Resource.getValue("downloadInfo")
                        'End If
                        'result &= ")"
                        'If PermissionHelper.CheckPermissionSoft(ItemDescriptionDisplayMode.AsTooltip, DescriptionDisplayMode) AndAlso Not String.IsNullOrEmpty(dto.File.Description) Then
                        '    result &= vbCrLf & dto.File.Description
                        'End If
                        'result &= """>?</a></li>
                        result &= "</ul>" & vbCrLf
                        result &= "</div>" & vbCrLf
                    End If

                Case ItemDisplayView.multilineFull
                    result &= "<div class=""File_SMInfo_Actions"" > "

                    'result &= "<span class=""Titolo_campoSmall"">" & Me.GetFileSize(dto.File.Size) & "</span> - "
                    'If dto.Permission.Edit OrElse dto.Permission.Delete OrElse dto.Permission.UnDelete OrElse dto.Permission.VirtualDelete OrElse dto.Permission.ViewAdvancedStatistics Then
                    '    result &= "<span class=""Titolo_campoSmall"">" & dto.File.Downloads & " " & Resource.getValue("downloadInfo") & "</span>"
                    'End If
                    Dim actions As String = RenderActions(display, dto, currentUrl, IdModule, IdCommunity)

                    If Not String.IsNullOrEmpty(actions) Then
                        result &= "<ul class=""File_Actions"">" & vbCrLf
                        result &= actions
                        result &= "</ul>" & vbCrLf
                    End If
                    result &= "</div>" & vbCrLf
                    If PermissionHelper.CheckPermissionSoft(ItemDescriptionDisplayMode.Show, DescriptionDisplayMode) AndAlso Not String.IsNullOrEmpty(dto.File.Description) Then
                        result &= "<div class=""File_Detail ToolTip"">"
                        result &= vbCrLf & dto.File.Description & vbCrLf & "</div>" & vbCrLf
                    End If
                Case ItemDisplayView.multiline
                    Dim actions As String = RenderActions(display, dto, currentUrl, IdModule, IdCommunity)

                    If Not String.IsNullOrEmpty(actions) Then
                        result &= "<div class=""File_SMInfo_Actions"" > " & vbCrLf
                        result &= "<ul class=""File_Actions"">" & vbCrLf
                        result &= actions
                        result &= """</ul>" & vbCrLf
                        result &= "</div>" & vbCrLf
                    End If
            End Select
        Else
            result &= "</div>" & vbCrLf & "</div>"
        End If



        Return result
    End Function

    Private Function GetFileSize(ByVal size As Long) As String
        Dim sizeTranslated As String = ""
        If size = 0 Then
            sizeTranslated = "&nbsp;"
        Else
            If size < 1024 Then
                sizeTranslated = "1 kb"
            Else
                size = size / 1024
                If size < 1024 Then
                    sizeTranslated = Math.Round(size) & " kb"
                ElseIf size >= 1024 Then
                    sizeTranslated = Math.Round(size / 1024, 2) & " mb"
                End If
            End If
        End If
        Return sizeTranslated
    End Function

#End Region



End Class