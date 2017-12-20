
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Repository
Imports lm.Comol.Core.BaseModules.Repository.Presentation
Imports lm.Comol.Core.DomainModel.Helpers

Public Class UC_ModuleToRepositoryDisplay
    Inherits BaseControl
    Implements IViewModuleModuleToRepositoryDisplay

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As ModuleToRepositoryDisplayPresenter

    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As ModuleToRepositoryDisplayPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleToRepositoryDisplayPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Iview"

    Public Property ServiceCode As String Implements IViewModuleModuleToRepositoryDisplay.ModuleCode
        Get
            Return Me.ViewState("ServiceCode")
        End Get
        Set(ByVal value As String)
            Me.ViewState("ServiceCode") = value
        End Set
    End Property

    Public Property ServiceID As Integer Implements IViewModuleModuleToRepositoryDisplay.IdModule
        Get
            Return Me.ViewState("ServiceID")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("ServiceID") = value
        End Set
    End Property
    Public Property FileDisplayName As String Implements IViewModuleModuleToRepositoryDisplay.FileDisplayName
        Get
            Return Me.ViewState("FileDisplayName")
        End Get
        Set(ByVal value As String)
            Me.ViewState("FileDisplayName") = value
        End Set
    End Property
    Public Property DisplayAsTable As Boolean Implements IViewModuleModuleToRepositoryDisplay.DisplayAsTable
        Get
            Return (MLVdisplayMode.GetActiveView Is VIWtable)
        End Get
        Set(ByVal value As Boolean)
            MLVdisplayMode.SetActiveView(IIf(value, VIWtable, VIWinline))
        End Set
    End Property
    Public Property DisplayOnly As Boolean Implements IViewModuleModuleToRepositoryDisplay.DisplayOnly
        Get
            Return ViewStateOrDefault("DisplayOnly", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("DisplayOnly") = value
        End Set
    End Property
    Public Property IconSize As lm.Comol.Core.DomainModel.Helpers.IconSize Implements IViewModuleModuleToRepositoryDisplay.IconSize
        Get
            Return ViewStateOrDefault("IconSize", lm.Comol.Core.DomainModel.Helpers.IconSize.Small)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.Helpers.IconSize)
            Me.ViewState("IconSize") = value
        End Set
    End Property
#End Region

#Region "Inherited"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherited"

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ModuleToRepository", "Modules", "Repository")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "InitializeControl"
    'Public Sub InitializeControlByLink(ByVal linkID As Long)
    '    Me.CurrentPresenter.InitView(linkID)
    'End Sub
    Public Sub InitializeControlByLink(ByVal idLink As Long, ByVal permission As iCoreFilePermission)
        Me.CurrentPresenter.InitView(idLink, permission)
    End Sub
    Public Sub InitializeControlByLink(ByVal pLink As ModuleLink, ByVal permission As iCoreFilePermission)
        Me.CurrentPresenter.InitView(pLink, permission)
    End Sub
    'Public Sub InitializeControlByLink(ByVal pLink As ModuleLink)
    '    Me.CurrentPresenter.InitView(pLink)
    'End Sub
    'Public Sub InitializeControlByFile(ByVal oItem As dtoCommunityItemRepository)
    '    Me.CurrentPresenter.InitView(oItem)
    'End Sub
#End Region

#Region "Display"
    Public Sub DisplayNoAction() Implements IViewModuleModuleToRepositoryDisplay.DisplayNoAction
        Me.LTnoFile.Visible = False
    End Sub
    Public Sub DisplayRemovedObject() Implements IViewModuleModuleToRepositoryDisplay.DisplayRemovedObject
        Me.LTnoFile.Text = Resource.getValue("action.RemovedObject")
    End Sub
    Public Sub DisplayLinkDownload(ByVal idLink As Long, ByVal idCommunity As Integer, ByVal item As BaseCommunityFile, ByVal permission As iCoreFilePermission) Implements IViewModuleModuleToRepositoryDisplay.DisplayLinkDownload
        If Me.DisplayAsTable Then
            Me.DisplayLinkDownloadAsTable(idLink, idCommunity, item, permission)
        Else
            LTitem.Text = RenderItem(idLink, idCommunity, item, CoreModuleRepository.ActionType.DownloadFile, permission)
        End If
    End Sub
    Public Sub DisplayLinkForPlay(ByVal idLink As Long, ByVal idCommunity As Integer, ByVal item As BaseCommunityFile, ByVal permission As iCoreFilePermission) Implements IViewModuleModuleToRepositoryDisplay.DisplayLinkForPlay
        If Me.DisplayAsTable Then
            Me.DisplayLinkForPlayAsTable(idLink, idCommunity, item, permission)
        Else
            LTitem.Text = RenderItem(idLink, idCommunity, item, CoreModuleRepository.ActionType.PlayFile, permission)
        End If
    End Sub
    Public Sub DisplayLinkForPlayInternal(ByVal idLink As Long, ByVal idCommunity As Integer, ByVal item As BaseCommunityFile, ByVal permission As iCoreFilePermission, ByVal idAction As Integer) Implements IViewModuleModuleToRepositoryDisplay.DisplayLinkForPlayInternal
        If Me.DisplayAsTable Then
            Me.DisplayLinkForPlayInternalAsTable(idLink, idCommunity, item, idAction, permission)
        Else
            LTitem.Text = RenderItem(idLink, idCommunity, item, idAction, permission)
        End If
    End Sub

    'Private Sub UpdateDisplay(ByVal LinkID As Long, ByVal Item As BaseCommunityFile, ByVal CommunityID As Integer, ByVal permission As iCoreFilePermission, ByVal filePlayUrl As String)
    '    If Item.isFile Then
    '        LTfileImage.Text = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(Item.Extension) & "'>"
    '    Else
    '        LTfileImage.Text = "<img src='" & BaseUrl & "RadControls/TreeView/Skins/Materiale/folder.gif" & "'>"
    '    End If
    '    If Not IsNothing(permission) Then
    '        Dim DestinationUrl As String = Request.Url.LocalPath
    '        If Me.BaseUrl <> "/" Then
    '            DestinationUrl = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
    '        End If
    '        DestinationUrl = PageUtility.GetUrlEncoded(DestinationUrl & Request.Url.Query)

    '        If permission.Download Then
    '            Me.LTnomeFile.Visible = False
    '            Me.HYPfile.Visible = True
    '            Me.HYPfile.Text = Item.DisplayName
    '            Me.HYPfile.NavigateUrl = BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.DownloadFileFromModule(Item.Id, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, Me.ServiceID, CommunityID, LinkID)
    '        Else
    '            Me.HYPfile.Visible = False
    '            Me.LTnomeFile.Visible = True
    '            Me.LTnomeFile.Text = Item.DisplayName
    '        End If
    '        Dim iconStatistic As String = BaseUrl & "images/grid/statistic.gif"
    '        Dim iconMetadata As String = BaseUrl & "images/scorm/metadati_icon.gif"

    '        'If Item.isSCORM Then
    '        If permission.Play Then
    '            Dim iconPlay As String = BaseUrl & "images/Scorm/visualizza.png"
    '            Me.Resource.setHyperLink(HYPplayItem, True, True)
    '            HYPplayItem.Visible = True
    '            HYPplayItem.NavigateUrl = filePlayUrl
    '            HYPplayItem.Text = String.Format(HYPplayItem.Text, iconPlay, Resource.getValue("action.RepositoryItemType." & Item.RepositoryItemType.ToString))
    '        End If
    '        If permission.ViewStatistics Then
    '            Me.Resource.setHyperLink(HYPstatistics, True, True)
    '            HYPstatistics.Visible = True
    '            HYPstatistics.NavigateUrl = Me.BaseUrl & "Modules/Scorm/ScormStatisticheMain.aspx?FileId=" & Item.Id.ToString & "&LinkId=" & LinkID.ToString & "&BackUrl=" & DestinationUrl
    '            HYPstatistics.Text = String.Format(HYPstatistics.Text, iconStatistic, HYPstatistics.ToolTip)
    '        ElseIf permission.ViewPersonalStatistics Then
    '            Me.Resource.setHyperLink(HYPstatistics, True, True)
    '            HYPstatistics.Visible = True
    '            HYPstatistics.NavigateUrl = Me.BaseUrl & "Modules/Scorm/ScormStatisticheUtente.aspx?FileId=" & Item.Id.ToString & "&LinkId=" & LinkID.ToString & "&BackUrl=" & DestinationUrl
    '            HYPstatistics.Text = String.Format(HYPstatistics.Text, iconStatistic, HYPstatistics.ToolTip)
    '        End If
    '        If permission.EditMetadata Then
    '            Me.Resource.setHyperLink(HYPeditMetadata, True, True)
    '            HYPeditMetadata.Visible = True
    '            HYPeditMetadata.NavigateUrl = Me.BaseUrl & "Modules/Scorm/ScormPackageSettings.aspx?FileId=" & Item.Id.ToString & "&LinkId=" & LinkID.ToString & "&BackUrl=" & DestinationUrl
    '            HYPeditMetadata.Text = String.Format(HYPeditMetadata.Text, iconMetadata, HYPeditMetadata.ToolTip)
    '        End If
    '        'End If
    '        Dim iconCommunityPermission As String = BaseUrl & "images/grid/permessicomunita.gif"
    '        Dim iconUserPermission As String = BaseUrl & "images/grid/permessiutenti.gif"
    '        If permission.EditRepositoryPermission Then
    '            Me.Resource.setHyperLink(HYPeditPermission, True, True)
    '            HYPeditPermission.Visible = True
    '            '    HYPeditPermission.NavigateUrl = Me.BaseUrl & "Modules/Scorm/ScormPackageSettings.aspx?FileId=" & Item.ToString & "&BackUrl=" & DestinationUrl
    '            HYPeditPermission.Text = String.Format(HYPeditPermission.Text, iconCommunityPermission, HYPeditPermission.ToolTip)
    '            '    Item.
    '            '    oHyperlink.Text = String.Format(oHyperlink.Text, Me.BaseUrl & "images/grid/" & IIf(oDto.AvailableForAll, "permessicomunita.gif", "permessiutenti.gif"), IIf(oDto.AvailableForAll, Me.Resource.getValue("communityPermission"), Me.Resource.getValue("roleUserPermission")))
    '            HYPeditPermission.NavigateUrl = Me.BaseUrl & "Modules/Repository/CommunityRepositoryItemPermission.aspx?ItemID=" & Item.Id.ToString & "&FolderID=" & Item.FolderId.ToString & "&CommunityID=" & CommunityID.ToString & "&LinkId=" & LinkID.ToString & "&BackUrl=" & DestinationUrl

    '        End If

    '    End If
    'End Sub
#End Region

#Region "Render InLine"
    Private Function RenderItemName(ByVal item As BaseCommunityFile) As String

        Dim renderItem As String = ""
        Dim itemName As String = item.DisplayName
        Dim title As String = ""

        Dim itemIco As String = Me.BaseUrl & IIf(item.isFile, Me.PageUtility.SystemSettings.Extension.FindIconImage(item.Extension), "RadControls/TreeView/Skins/Materiale/folder.gif")

        If itemName.Length > 30 Then
            itemName = itemName.Substring(0, 13) & "...." & itemName.Substring(itemName.Length - 14)
        End If
        If item.isFile Then
            title = Resource.getValue("fileTitle") & " " & item.DisplayName & " (" & GetFileSize(item.Size) & ")."
        Else
            title = itemName
        End If

        renderItem = "<span class=""File_Name"" title=""{0}""><img src=""{1}"" class=""File_Ext""/>{2}</span>" & vbCrLf

        Return String.Format(renderItem, title, itemIco, itemName)
    End Function

    Private Function RenderItemFileLink(ByVal IdLink As Long, idCommunity As Integer, ByVal item As BaseCommunityFile, ByVal permission As iCoreFilePermission, idAction As Integer) As String
        Dim renderItem As String = ""
        Dim title As String = ""
        Dim itemUrl As String = ""


        itemUrl = IIf(item.isFile, lm.Comol.Core.BaseModules.Repository.Domain.RootObject.DownloadFileFromModule(item.Id, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, PageUtility.UniqueGuidSession, Me.ServiceID, idCommunity, IdLink, False), "")


        Dim iconRender As String = "<img src=""{0}"" class=""File_Ext""> "
        Dim fileName As String = item.DisplayName
        If item.isFile AndAlso item.RepositoryItemType = RepositoryItemType.FileStandard Then
            iconRender = String.Format(iconRender, BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(item.Extension))
        ElseIf Not item.isFile Then
            iconRender = String.Format(iconRender, BaseUrl & "RadControls/TreeView/Skins/Materiale/folder.gif")
        Else
            fileName = item.Name
            iconRender = IconCommandsHelper.RenderSpan(Me.Resource.getValue("action.RepositoryItemType." & item.RepositoryItemType.ToString) & " " & item.Name, item.RepositoryItemType, IconSize, "File_Ext") & " "
        End If
        If fileName.Length > 50 Then
            fileName = fileName.Substring(0, 23) & "...." & fileName.Substring(fileName.Length - 24)
        End If

        If item.isFile Then
            Dim fileLink As String = ""
            title = Resource.getValue("fileTitle") & " " & item.DisplayName & " (" & GetFileSize(item.Size) & ")."

            If item.RepositoryItemType = RepositoryItemType.FileStandard Then
                If permission.Download Then
                    fileLink = "<div class=""File_NameExt"">" & vbCrLf & "<a href=""{0}"" class=""File_Name ROW_ItemLink_Small fileRepositoryCookie"" target=""_blank"" title=""{1}"">{2} <span>{3}</span></a>" & vbCrLf & "</div>" & vbCrLf

                    Return String.Format(fileLink, itemUrl, title, iconRender, fileName)
                Else
                    fileLink = "<div class=""File_NameExt"">" & vbCrLf & "<span class=""File_Name"" title=""{0}"">{1} <span>{2}</span></span>" & vbCrLf & "</div>" & vbCrLf
                    Return String.Format(fileLink, title, iconRender, fileName)
                End If
            ElseIf permission.Play Then
                Dim url As String = ""
                Dim BasePlayUrl As String = ""

                BasePlayUrl = Me.BaseUrl

                Select Case item.RepositoryItemType
                    Case RepositoryItemType.ScormPackage
                        url = ""
                    Case RepositoryItemType.Multimedia
                        If Me.ServiceCode <> CoreModuleRepository.UniqueID Then
                            url = BasePlayUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.PlayMultimediaFileFromModule(item.Id, item.UniqueID, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, False, Me.ServiceID, idCommunity, IdLink, idAction, PreLoadedContentView)
                        Else
                            url = BasePlayUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.PlayMultimediaFile(item.Id, item.UniqueID, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, False, PreLoadedContentView)
                        End If
                End Select
                fileLink = "<div class=""File_NameExt"">" & vbCrLf & "<a href=""{0}"" class=""File_Name ROW_ItemLink_Small fileRepositoryCookieNoBlockUI"" title=""{1}""  target=""_blank"">{2} <span>{3}</span></a>" & vbCrLf & "</div>" & vbCrLf

                Return String.Format(fileLink, url, title, iconRender, item.Name)
            Else
                title = Me.Resource.getValue("action.RepositoryItemType." & item.RepositoryItemType.ToString) & " " & item.Name

                fileLink = "<div class=""File_NameExt"">" & vbCrLf & "<span class=""File_Name"" title=""{0}"">{1} <span>{2}</span></span>" & vbCrLf & "</div>"
                Return String.Format(fileLink, title, iconRender, fileName)
            End If
        Else
            renderItem = "<div class=""File_NameExt"">" & vbCrLf & "<span class=""File_Name"" title=""{0}"">{1} {2}</span>" & vbCrLf & "</div>" & vbCrLf
            title = fileName
            Return String.Format(renderItem, title, iconRender, fileName)
        End If
    End Function

    Private Function RenderActions(ByVal idLink As Long, ByVal idCommunity As Integer, ByVal file As BaseCommunityFile, ByVal idAction As Integer, ByVal permission As iCoreFilePermission) As String
        Dim result As String = ""
        Dim DestinationUrl As String = Request.Url.LocalPath
        If Me.BaseUrl <> "/" Then
            DestinationUrl = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
        End If
        DestinationUrl = Server.UrlEncode(DestinationUrl & Request.Url.Query)



        If permission.Download Then
            Dim title As String = Resource.getValue("fileTitle") & " " & IIf(file.RepositoryItemType = RepositoryItemType.FileStandard, file.DisplayName, file.Name) & " (" & GetFileSize(file.Size) & ")."

            result &= "<li>" & vbCrLf
            result &= IconCommandsHelper.RenderActionlLink(lm.Comol.Core.BaseModules.Repository.Domain.RootObject.DownloadFile(file.Id, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, PageUtility.UniqueGuidSession), _
                                                    title, IconCommandsHelper.LinkTarget._blank,
                                                  IconCommandsHelper.CommandIcon.DownloadItem, IconSize, "File_ImgLnk File_Img_Action fileRepositoryCookie") & vbCrLf

            result &= "</li>" & vbCrLf
        Else
            result &= "<li>&nbsp;</li>" & vbCrLf
        End If
        If (permission.ViewPersonalStatistics OrElse permission.ViewStatistics) Then
            Dim url As String = ""
            Select Case file.RepositoryItemType
                Case RepositoryItemType.ScormPackage
                    If permission.ViewStatistics Then
                        url = Me.BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.ManagementScormStatistics(file.Id, DestinationUrl, PreLoadedContentView)
                    ElseIf permission.ViewPersonalStatistics Then
                        url = Me.BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.UserScormStatistics(file.Id, DestinationUrl, PreLoadedContentView)
                    End If
            End Select

            If Not String.IsNullOrEmpty(url) Then
                result &= "<li>" & vbCrLf
                result &= IconCommandsHelper.RenderActionlLink(url, Me.Resource.getValue("statistic.RepositoryItemType." & file.RepositoryItemType.ToString),
                                                      IconCommandsHelper.CommandIcon.Statistics, IconSize, "File_Img_Action") & vbCrLf

                result &= "</li>" & vbCrLf
            End If
        Else
            result &= "<li>&nbsp;</li>" & vbCrLf
        End If
        'If permission.Download AndAlso file.RepositoryItemType = RepositoryItemType.FileStandard Then
        '    Dim title As String = Resource.getValue("fileTitle") & " " & IIf(file.RepositoryItemType = RepositoryItemType.FileStandard, file.DisplayName, file.Name) & " (" & GetFileSize(file.Size) & ")."

        '    result &= "<li>" & vbCrLf
        '    result &= "<a href=""" & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.DownloadFileFromModule(file.Id, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, Me.ServiceID, idCommunity, idLink) & """  target=""_blank"" class=""File_ImgLnk"">" & vbCrLf
        '    result &= "<img src=""" & BaseUrl & "images/DefaultCommand/download16.png"" alt=""" & Me.Resource.getValue("action.RepositoryItemType." & file.RepositoryItemType.ToString) & """ class=""File_Img_Action"" title=""" & Me.Resource.getValue("action.RepositoryItemType." & file.RepositoryItemType.ToString) & """/>"
        '    result &= "</a>" & vbCrLf
        '    result &= "</li>" & vbCrLf
        'ElseIf permission.Play Then
        'Dim url As String = ""
        'Dim imageUrl As String = ""
        'result &= "<li>" & vbCrLf
        'Dim BasePlayUrl As String = ""
        'If SystemSettings.Icodeon.OverrideSSLsettings Then
        '    BasePlayUrl = Me.BaseUrlNoSSL
        'Else
        '    BasePlayUrl = Me.BaseUrl
        'End If
        'Select Case file.RepositoryItemType
        '    Case RepositoryItemType.ScormPackage
        '        imageUrl = "playScorm16.png"
        '        If Me.ServiceCode <> CoreModuleRepository.UniqueID Then
        '            url = BasePlayUrl & Me.PageUtility.SystemSettings.Icodeon.GetScormOtherModuleLinkUpdateAction(file.Id, file.UniqueID, Me.CurrentContext.UserContext.CurrentUserID, Me.ServiceID, idAction, idCommunity, idLink, Me.PageUtility.LinguaCode, PageUtility.UniqueGuidSession)
        '        Else
        '            url = BasePlayUrl & Me.PageUtility.SystemSettings.Icodeon.GetScormRepositoryLink(file.Id, file.UniqueID, Me.CurrentContext.UserContext.CurrentUserID, Me.ServiceID, idCommunity, CoreModuleRepository.ActionType.AjaxUpdate, Me.PageUtility.LinguaCode)
        '        End If
        '    Case RepositoryItemType.Multimedia
        '        imageUrl = "playMMD16.png"
        '        If Me.ServiceCode <> CoreModuleRepository.UniqueID Then
        '            url = BasePlayUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.PlayMultimediaFileFromModule(file.Id, file.UniqueID, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, Me.ServiceID, idCommunity, idLink, idAction, PreLoadedContentView)
        '        Else
        '            url = BasePlayUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.PlayMultimediaFile(file.Id, file.UniqueID, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, PreLoadedContentView)
        '        End If
        'End Select

        'If Not String.IsNullOrEmpty(url) AndAlso Not String.IsNullOrEmpty(imageUrl) Then
        '    result &= "<a href=""" & url & """ class=""File_ImgLnk fileRepositoryCookieNoBlockUI"" target=""_blank"">" & vbCrLf
        '    result &= "<img src=""" & BaseUrl & "images/DefaultCommand/" & imageUrl & """ alt=""" & Me.Resource.getValue("action.RepositoryItemType." & file.RepositoryItemType.ToString) & """ class=""File_Img_Action"" title=""" & Me.Resource.getValue("action.RepositoryItemType." & file.RepositoryItemType.ToString) & """/>"
        '    result &= "</a>" & vbCrLf
        '    result &= "</li>" & vbCrLf
        'End If

        'End If
        'If (permission.ViewPersonalStatistics OrElse permission.ViewStatistics) Then
        '    Dim url As String = ""

        '    result &= "<li>" & vbCrLf
        '    Select Case file.RepositoryItemType
        '        Case RepositoryItemType.ScormPackage

        '            If permission.ViewStatistics Then
        '                url = Me.BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.ManagementScormStatistics(file.Id, DestinationUrl, PreLoadedContentView)
        '                '"Modules/Scorm/ScormStatisticheMain.aspx?FileID=" & oCommunityFile.Id.ToString & "&BackUrl=" & DestinationUrl
        '            ElseIf permission.ViewPersonalStatistics Then
        '                url = Me.BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.UserScormStatistics(file.Id, DestinationUrl, PreLoadedContentView)
        '                '"Modules/Scorm/ScormStatisticheUtente.aspx?FileID=" & oCommunityFile.Id.ToString & "&BackUrl=" & DestinationUrl
        '            End If
        '    End Select

        '    If Not String.IsNullOrEmpty(url) Then
        '        result &= "<a href=""" & url & """ class=""File_ImgLnk"">" & vbCrLf
        '        result &= "<img src=""" & BaseUrl & "images/DefaultCommand/stat16.png"" alt=""" & Me.Resource.getValue("statistic.RepositoryItemType." & file.RepositoryItemType.ToString) & """ class=""File_Img_Action"" title=""" & Me.Resource.getValue("statistic.RepositoryItemType." & file.RepositoryItemType.ToString) & """/>"
        '        result &= "</a>" & vbCrLf
        '        result &= "</li>" & vbCrLf
        '    End If

        'End If
        If permission.EditMetadata Then
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
                                                        IconCommandsHelper.CommandIcon.Settings, IconSize, "File_Img_Action") & vbCrLf

                result &= "</li>" & vbCrLf
            End If
        Else
            result &= "<li>&nbsp;</li>" & vbCrLf
        End If
        'If permission.EditMetadata Then
        '    Dim url As String = ""
        '    Dim imageUrl As String = ""
        '    result &= "<li>" & vbCrLf
        '    Select Case file.RepositoryItemType
        '        Case RepositoryItemType.ScormPackage
        '            imageUrl = "setting16.png"
        '            url = BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.EditScormPackageSettings(file.Id, idLink, DestinationUrl, PreLoadedContentView)
        '        Case RepositoryItemType.Multimedia
        '            imageUrl = "setting16.png"
        '            url = BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.EditMultimediaFileSettings(file.Id, idLink, DestinationUrl, PreLoadedContentView)
        '        Case RepositoryItemType.VideoStreaming
        '            imageUrl = "setting16.png"
        '    End Select
        '    If Not String.IsNullOrEmpty(url) AndAlso Not String.IsNullOrEmpty(imageUrl) Then
        '        result &= "<a href=""" & url & """ class=""File_ImgLnk"">" & vbCrLf
        '        result &= "<img src=""" & BaseUrl & "images/DefaultCommand/" & imageUrl & """ alt=""" & Me.Resource.getValue("settings.RepositoryItemType." & file.RepositoryItemType.ToString) & """ class=""File_Img_Action"" title=""" & Me.Resource.getValue("settings.RepositoryItemType." & file.RepositoryItemType.ToString) & """/>"
        '        result &= "</a>" & vbCrLf
        '        result &= "</li>" & vbCrLf
        '    End If
        'End If
        Return result
    End Function
    Private Function RenderItem(ByVal idLink As Long, ByVal idCommunity As Integer, ByVal item As BaseCommunityFile, ByVal idAction As Integer, ByVal permission As iCoreFilePermission) As String
        Dim result As String = "<div class=""File_Main"">" & vbCrLf & "<div class=""File_MainInfo_TinyCut"">" & vbCrLf

        result &= RenderItemFileLink(idLink, idCommunity, item, permission, idAction)
        result &= "</div>" & vbCrLf & "</div>" & vbCrLf
        Dim render As String = ""
        If item.isFile Then

            result &= "<div class=""File_SMInfo_Actions"" > " & vbCrLf
            result &= "<ul class=""File_Actions"">" & vbCrLf
            result &= RenderActions(idLink, idCommunity, item, idAction, permission)

            result &= "</ul>" & vbCrLf
            result &= "</div>" & vbCrLf
        End If



        Return result
    End Function

#End Region

#Region "As tAble"
    Private Sub DisplayLinkDownloadAsTable(ByVal LinkID As Long, ByVal CommunityID As Integer, ByVal Item As BaseCommunityFile, ByVal permission As iCoreFilePermission)
        'Dim sizeTranslated As String = GetFileSize(Item.Size)
        'Me.HYPfile.Visible = True
        'Me.HYPfile.Text = Item.DisplayName
        'Me.HYPfile.NavigateUrl = BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.DownloadFileFromModule(Item.Id, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, Me.ServiceID, CommunityID, LinkID)

        UpdateDisplayAsTable(LinkID, Item, CommunityID, permission, "")
    End Sub

    Private Sub DisplayLinkForPlayAsTable(ByVal LinkID As Long, ByVal CommunityID As Integer, ByVal Item As BaseCommunityFile, ByVal permission As iCoreFilePermission)
        Dim fileUrl As String = ""
        Dim BasePlayUrl As String = ""
        '   If SystemSettings.Icodeon.OverrideSSLsettings Then
        BasePlayUrl = Me.BaseUrlNoSSL
        'Else
        '    BasePlayUrl = Me.BaseUrl
        'End If
        If Item.RepositoryItemType = RepositoryItemType.ScormPackage Then
            fileUrl = ""
        ElseIf Item.RepositoryItemType = RepositoryItemType.Multimedia OrElse Item.RepositoryItemType = RepositoryItemType.VideoStreaming Then
            fileUrl = BasePlayUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.PlayMultimediaFile(Item.Id, Item.UniqueID, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, False, PreLoadedContentView)
        End If


        'Me.LTnomeFile.Visible = True
        'Me.LTnomeFile.Text = Item.DisplayName
        UpdateDisplayAsTable(LinkID, Item, CommunityID, permission, fileUrl)
    End Sub

    Private Sub DisplayLinkForPlayInternalAsTable(ByVal LinkID As Long, ByVal CommunityID As Integer, ByVal Item As BaseCommunityFile, ByVal ServiceActionID As Integer, ByVal permission As iCoreFilePermission)
        Dim WorkingSessionID As System.Guid = Me.PageUtility.UniqueGuidSession
        Dim fileUrl As String = ""
        Dim BasePlayUrl As String = ""
        ' If SystemSettings.Icodeon.OverrideSSLsettings Then
        BasePlayUrl = Me.BaseUrlNoSSL
        'Else
        '    BasePlayUrl = Me.BaseUrl
        'End If
        If Item.RepositoryItemType = RepositoryItemType.ScormPackage Then
            fileUrl = ""
        ElseIf Item.RepositoryItemType = RepositoryItemType.Multimedia OrElse Item.RepositoryItemType = RepositoryItemType.VideoStreaming Then
            fileUrl = BasePlayUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.PlayMultimediaFileFromModule(Item.Id, Item.UniqueID, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, False, Me.ServiceID, CommunityID, LinkID, ServiceActionID, PreLoadedContentView)
        End If


        'Me.LTnomeFile.Visible = True
        'Me.LTnomeFile.Text = Item.DisplayName
        UpdateDisplayAsTable(LinkID, Item, CommunityID, permission, fileUrl)
    End Sub

    Private Sub UpdateDisplayAsTable(ByVal LinkID As Long, ByVal Item As BaseCommunityFile, ByVal CommunityID As Integer, ByVal permission As iCoreFilePermission, ByVal filePlayUrl As String)
        Dim iconRender As String = "<img src=""{0}""> "
        Dim fileName As String = Item.DisplayName
        If Item.isFile AndAlso Item.RepositoryItemType = RepositoryItemType.FileStandard Then
            iconRender = String.Format(iconRender, BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(Item.Extension))
        ElseIf Not Item.isFile Then
            iconRender = String.Format(iconRender, BaseUrl & "RadControls/TreeView/Skins/Materiale/folder.gif")
        Else
            iconRender = IconCommandsHelper.RenderSpan("", Item.RepositoryItemType, IconSize) & " "
            fileName = Item.Name
        End If

        If Not IsNothing(permission) Then
            Dim downloadUrl As String = BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.DownloadFileFromModule(Item.Id, Me.CurrentContext.UserContext.CurrentUserID, Me.PageUtility.LinguaCode, PageUtility.UniqueGuidSession, Me.ServiceID, CommunityID, LinkID, False)
            Dim DestinationUrl As String = Request.Url.LocalPath
            If Me.BaseUrl <> "/" Then
                DestinationUrl = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
            End If
            DestinationUrl = PageUtility.GetUrlEncoded(DestinationUrl & Request.Url.Query)

            If permission.Download Then
                If Item.RepositoryItemType = RepositoryItemType.FileStandard Then
                    HYPitemAction.Text = iconRender & fileName
                    HYPitemAction.NavigateUrl = downloadUrl
                    HYPitemAction.Visible = True
                    HYPitemAction.CssClass &= " fileRepositoryCookie"
                    HYPitemAction.ToolTip = Resource.getValue("action.RepositoryItemType.FileStandard") & " " & fileName
                    HYPdownloadItem.NavigateUrl = downloadUrl
                    HYPdownloadItem.Visible = True
                    HYPdownloadItem.CssClass &= " fileRepositoryCookie " & IconCommandsHelper.CommandIcon.DownloadItem.GetStringValue() & IconSize.GetStringValue
                    HYPdownloadItem.ToolTip = Resource.getValue("action.RepositoryItemType.FileStandard") & " " & fileName
                ElseIf Item.RepositoryItemType <> RepositoryItemType.Folder Then
                    HYPdownloadItem.NavigateUrl = downloadUrl
                    HYPdownloadItem.Visible = True
                    HYPdownloadItem.CssClass &= " fileRepositoryCookieNoBlockUI " & IconCommandsHelper.CommandIcon.DownloadItem.GetStringValue() & IconSize.GetStringValue
                    HYPdownloadItem.ToolTip = Resource.getValue("action.RepositoryItemType.FileStandard") & " " & fileName
                    HYPitemAction.Visible = False
                Else
                    Me.LTitemAction.Text = iconRender & fileName
                End If
                Me.LTitemAction.Visible = False
            Else
                Me.LTitemAction.Text = iconRender & fileName
            End If
         
            If permission.Play Then
                HYPitemAction.Text = iconRender & fileName
                HYPitemAction.NavigateUrl = filePlayUrl
                HYPitemAction.Visible = True
                HYPitemAction.CssClass &= " fileRepositoryCookieNoBlockUI"
                HYPitemAction.ToolTip = Resource.getValue("action.RepositoryItemType") & Item.RepositoryItemType.ToString & " " & fileName
            End If
            If permission.ViewStatistics OrElse permission.ViewPersonalStatistics Then
                Dim url As String = ""
                Select Case Item.RepositoryItemType
                    Case RepositoryItemType.ScormPackage
                        If permission.ViewStatistics Then
                            url = Me.BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.ManagementScormStatistics(Item.Id, DestinationUrl, PreLoadedContentView)
                        ElseIf permission.ViewPersonalStatistics Then
                            url = Me.BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.UserScormStatistics(Item.Id, DestinationUrl, PreLoadedContentView)
                        End If
                End Select

                If Not String.IsNullOrEmpty(url) Then
                    HYPsettings.ToolTip = Resource.getValue("statistic.RepositoryItemType." & Item.RepositoryItemType.ToString)
                    HYPstatistics.Visible = True
                    HYPstatistics.NavigateUrl = url
                    HYPstatistics.Text = IconCommandsHelper.RenderSpan(Me.Resource.getValue("statistic.RepositoryItemType." & Item.RepositoryItemType.ToString),
                                                        IconCommandsHelper.CommandIcon.Statistics, IconSize)
                End If
            End If
            If permission.EditMetadata Then
                Dim url As String = ""
                Select Case Item.RepositoryItemType
                    Case RepositoryItemType.ScormPackage
                        url = BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.EditScormPackageSettings(Item.Id, LinkID, DestinationUrl, PreLoadedContentView)
                    Case RepositoryItemType.Multimedia
                        url = BaseUrl & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.EditMultimediaFileSettings(Item.Id, LinkID, DestinationUrl, PreLoadedContentView)
                End Select
                If String.IsNullOrEmpty(url) = False Then
                    HYPsettings.ToolTip = Resource.getValue("settings.RepositoryItemType." & Item.RepositoryItemType.ToString)
                    HYPsettings.Visible = True
                    HYPsettings.NavigateUrl = url
                    HYPsettings.Text = IconCommandsHelper.RenderSpan(Me.Resource.getValue("settings.RepositoryItemType." & Item.RepositoryItemType.ToString),
                                                        IconCommandsHelper.CommandIcon.Settings, IconSize)

                End If

            End If

            If permission.EditRepositoryPermission Then
                Me.Resource.setHyperLink(HYPeditPermission, True, True)
                HYPeditPermission.Visible = True
                HYPeditPermission.ToolTip = Resource.getValue("permission.RepositoryItemType." & Item.RepositoryItemType.ToString)
                HYPeditPermission.NavigateUrl = Me.BaseUrl & "Modules/Repository/CommunityRepositoryItemPermission.aspx?ItemID=" & Item.Id.ToString & "&FolderID=" & Item.FolderId.ToString & "&CommunityID=" & CommunityID.ToString & "&LinkId=" & LinkID.ToString & "&BackUrl=" & DestinationUrl
                HYPeditPermission.Text = IconCommandsHelper.RenderSpan(Me.Resource.getValue("permission.RepositoryItemType." & Item.RepositoryItemType.ToString),
                                                       IconCommandsHelper.CommandIcon.CommunityPermission, IconSize)

            End If
        Else
            Me.LTitemAction.Text = iconRender & fileName
        End If
    End Sub
#End Region

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

End Class