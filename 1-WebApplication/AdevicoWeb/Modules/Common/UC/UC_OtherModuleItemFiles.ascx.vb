Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.Repository.Presentation
Imports lm.Comol.UI.Presentation

Public Class UC_OtherModuleItemFiles
    Inherits BaseControl
    Implements IViewOtherModuleItemFilesLong

    Public Event VirtualDelete(ByVal ItemID As Long, ByVal ItemLinkId As Long)
    Public Event UnDelete(ByVal ItemID As Long, ByVal ItemLinkId As Long)
    Public Event PhysicalDelete(ByVal ItemID As Long, ByVal ItemLinkId As Long)
    Public Event UnlinkRepositoryItem(ByVal ItemID As Long, ByVal ItemLinkId As Long)
    '  Public Event EditFileItemVisibility(ByVal ItemID As Long, ByVal ItemLinkId As Long, ByVal visibilityStatus As ModuleItemFileVisibilityStatus)
    Public Event EditFileItemVisibility(ByVal ItemID As Long, ByVal ItemLinkId As Long, ByVal visibleForModule As Boolean, ByVal visibleForRepository As Boolean)
    Public Event UnChangeEvent()
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As OtherModuleManagmentFilesLongPresenter
    Public ReadOnly Property CurrentPresenter() As OtherModuleManagmentFilesLongPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New OtherModuleManagmentFilesLongPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

#Region "Implemented Property"
    Public Property ItemCommunityID As Integer Implements IViewOtherModuleItemFilesLong.ItemCommunityID
        Get
            If TypeOf Me.ViewState("ItemCommunityID") Is Integer Then
                Return CInt(Me.ViewState("ItemCommunityID"))
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("ItemCommunityID") = value
        End Set
    End Property

    Public Property ModuleCode As String Implements IViewOtherModuleItemFilesLong.ModuleCode
        Get
            If TypeOf Me.ViewState("ItemModuleCode") Is String Then
                Return Me.ViewState("ItemModuleCode")
            Else
                Return ""
            End If
        End Get
        Set(ByVal value As String)
            Me.ViewState("ItemModuleCode") = value
        End Set
    End Property
    Public Property ModuleID As Integer Implements IViewOtherModuleItemFilesLong.ModuleID
        Get
            If TypeOf Me.ViewState("ItemModuleID") Is Integer Then
                Return CInt(Me.ViewState("ItemModuleID"))
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("ItemModuleID") = value
        End Set
    End Property
    Public Property ItemID As Long Implements IViewOtherModuleItemFilesLong.ItemID
        Get
            If TypeOf Me.ViewState("ItemID") Is Long Then
                Return DirectCast(Me.ViewState("ItemID"), Long)
            End If
        End Get
        Set(ByVal value As Long)
            Me.ViewState("ItemID") = value
        End Set
    End Property
    Public Property ShowManagementButtons As Boolean Implements IViewOtherModuleItemFilesLong.ShowManagementButtons
        Get
            If TypeOf Me.ViewState("ShowManagementButtons") Is Boolean Then
                Return CBool(Me.ViewState("ShowManagementButtons"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ShowManagementButtons") = value
        End Set
    End Property
    Public Property ItemPermissions As iCoreItemPermission Implements IViewOtherModuleItemFilesLong.ItemPermissions
        Get
            If TypeOf Me.ViewState("ItemPermissions") Is iCoreItemPermission Then
                Return DirectCast(Me.ViewState("ItemPermissions"), iCoreItemPermission)
            Else
                Return New CoreItemPermission
            End If
        End Get
        Set(ByVal value As iCoreItemPermission)
            Me.ViewState("ItemPermissions") = value
        End Set
    End Property
    Public Property AutoUpdate As Boolean Implements IViewOtherModuleItemFilesLong.AutoUpdate
        Get
            If TypeOf Me.ViewState("AutoUpdate") Is Boolean Then
                Return CBool(Me.ViewState("AutoUpdate"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AutoUpdate") = value
        End Set
    End Property
    Public Property PublishUrl As String Implements IViewOtherModuleItemFiles.PublishUrl
        Get
            Return Me.ViewState("PublishUrl")
        End Get
        Set(ByVal value As String)
            Me.ViewState("PublishUrl") = value
        End Set
    End Property
    Public Property ShowStatus As Boolean Implements IViewOtherModuleItemFiles.ShowStatus
        Get
            Return Me.ViewState("ShowStatus")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ShowStatus") = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_OtherModuleItemFiles", "Modules", "Repository")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        'With MyBase.Resource
        '    .setHeaderGridView(Me.GDVdiaryItemFiles, 1, "E", True)
        '    .setHeaderGridView(Me.GDVdiaryItemFiles, 2, "EXPORT", True)
        '    .setHeaderGridView(Me.GDVdiaryItemFiles, 4, "CreatedOn", True)
        '    .setHeaderGridView(Me.GDVdiaryItemFiles, 5, "CreatedBy", True)
        '    .setHeaderGridView(Me.GDVdiaryItemFiles, 6, "C", True)
        'End With
    End Sub
#End Region

#Region "Initialize"
    Public Sub InitalizeControl(ByVal pItemID As Long, ByVal ItemPermissions As iCoreItemPermission, ByVal links As IList(Of iCoreItemFileLink(Of Long)), ByVal urlForPublish As String) Implements IViewOtherModuleItemFilesLong.InitalizeControl
        PublishUrl = urlForPublish
        ItemID = pItemID
        ShowStatus = False
        InitDialog()
        Me.CurrentPresenter.InitView(pItemID, ItemPermissions, links, New List(Of TranslatedItem(Of Long)), PublishUrl)
    End Sub
    Public Sub InitalizeControl(ByVal pItemID As Long, ByVal ItemPermissions As iCoreItemPermission, ByVal links As IList(Of iCoreItemFileLink(Of Long)), ByVal statusList As IList(Of TranslatedItem(Of Long)), ByVal urlForPublish As String) Implements IViewOtherModuleItemFilesLong.InitalizeControl
        PublishUrl = urlForPublish
        ItemID = pItemID
        ShowStatus = True
        InitDialog()
        Me.CurrentPresenter.InitView(pItemID, ItemPermissions, links, statusList, PublishUrl)
    End Sub
#End Region

    Public Sub LoadFiles(ByVal files As IList(Of iCoreItemFileLinkPermission(Of Long))) Implements IViewOtherModuleItemFilesLong.LoadFiles
        Me.RPTitemFiles.Visible = (files.Count > 0)
        If files.Count > 0 Then
            Me.RPTitemFiles.DataSource = files
            Me.RPTitemFiles.DataBind()
        End If
    End Sub
    Public ReadOnly Property BackGroundCss(ByVal ItemType As ListItemType, ByVal oItem As iCoreItemFileLinkPermission(Of Long))
        Get
            Dim cssLink As String = "ROW_ItemLink_Small"
            Dim cssRiga As String = "ROW_TD_Small"
            Try
                If Not oItem.ItemFileLink.isVisible Then
                    cssRiga = "ROW_Disabilitate_Small"

                ElseIf ItemType = ListItemType.AlternatingItem Then
                    cssRiga = "ROW_Alternate_Small"
                Else
                    cssRiga = "ROW_Normal_Small"
                End If


                If oItem.ItemFileLink.Deleted <> BaseStatusDeleted.None Then
                    cssRiga &= " ROW_Deleted"
                End If
            Catch ex As Exception
                If ItemType = ListItemType.AlternatingItem Then
                    cssRiga = "ROW_Alternate_Small"
                End If
            End Try
            Return cssRiga
        End Get
    End Property
    Private Sub RPTitemFiles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTitemFiles.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oItem As dtoCoreItemFilePermission(Of Long) = DirectCast(e.Item.DataItem, iCoreItemFileLinkPermission(Of Long))
            Dim oItemLink As dtoCoreItemFileLink(Of Long) = oItem.ItemFileLink
            Dim oLabel As Label
            Dim oControl As HtmlControl

            oControl = e.Item.FindControl("TDaction")
            oControl.Visible = ShowManagementButtons
            oControl = e.Item.FindControl("TDpublish")
            oControl.Visible = ShowManagementButtons
            oControl = e.Item.FindControl("TDvisible")
            oControl.Visible = ShowManagementButtons
            If ShowManagementButtons Then
                Dim oLNBunlink, oLNBvirtualDelete, oLNBundelete As LinkButton
                Dim oLNBdelete As MyUC.DialogLinkButton = e.Item.FindControl("LNBdelete")
                oLNBunlink = e.Item.FindControl("LNBunlink")
                oLNBvirtualDelete = e.Item.FindControl("LNBvirtualDelete")
                oLNBundelete = e.Item.FindControl("LNBundelete")

                Me.Resource.setLinkButton(oLNBvirtualDelete, True, True)
                oLNBvirtualDelete.Visible = oItem.Permission.VirtualDelete
                oLNBundelete.Visible = oItem.Permission.UnDelete
                oLNBunlink.Visible = oItem.Permission.Unlink
                oLNBdelete.Visible = oItem.Permission.Delete

                Me.Resource.setLinkButton(oLNBunlink, True, True)
                oLNBunlink.Text = String.Format(oLNBunlink.Text, Me.BaseUrl & "images/grid/Unlink.gif", oLNBunlink.ToolTip)

                Me.Resource.setLinkButton(oLNBvirtualDelete, True, True)
                oLNBvirtualDelete.Text = String.Format(oLNBvirtualDelete.Text, Me.BaseUrl & "images/grid/cancella.gif", oLNBvirtualDelete.ToolTip)

                Me.Resource.setLinkButton(oLNBundelete, True, True)
                oLNBundelete.Text = String.Format(oLNBundelete.Text, Me.BaseUrl & "images/grid/ripristina.gif", oLNBundelete.ToolTip)

                Me.Resource.setLinkButton(oLNBdelete, True, True)
                oLNBdelete.Text = String.Format(oLNBdelete.Text, Me.BaseUrl & "images/grid/eliminato1.gif", oLNBdelete.ToolTip)

                oLNBvirtualDelete.CommandArgument = oItem.ItemFileLinkId.ToString
                oLNBunlink.CommandArgument = oItem.ItemFileLinkId.ToString
                oLNBundelete.CommandArgument = oItem.ItemFileLinkId.ToString
                oLNBdelete.CommandArgument = oItem.ItemFileLinkId.ToString
                oLNBdelete.DialogClass = "mandatoryDial"
                '' CELL 3
                Dim oPublish As HyperLink
                oPublish = e.Item.FindControl("HYPpublishItem")
                Me.Resource.setHyperLink(oPublish, True, True)
                oPublish.Text = String.Format(oPublish.Text, Me.BaseUrl & "images/grid/SendToCommunityRepository.jpg", oPublish.ToolTip)

                oPublish.Visible = oItem.Permission.Publish AndAlso PublishUrl <> "" AndAlso Not IsNothing(oItem.ItemFileLink.File)
                If PublishUrl <> "" AndAlso Not IsNothing(oItem.ItemFileLink.File) Then

                    Dim DestinationUrl As String = Request.Url.LocalPath
                    If Me.BaseUrl <> "/" Then
                        DestinationUrl = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
                    End If
                    DestinationUrl = PageUtility.GetUrlEncoded(DestinationUrl & Request.Url.Query)

                    oPublish.NavigateUrl = PageUtility.EncryptedUrl("Modules/Common/PublishIntoRepositoryFromModuleItem.aspx", String.Format(PublishUrl, ItemID.ToString, oItemLink.Link.Id, DestinationUrl))
                End If

                Dim oDialogHide As MyUC.DialogLinkButton = e.Item.FindControl("LNBhide")

                'Me.Resource.setLinkButtonToValue(oLinkButton, oItem.ItemFileLink.isVisible, True, True)
                'Me.Resource.setLinkButtonToValue(oDialogLinkButton, oItem.ItemFileLink.isVisible, True, True)
                ' oDialogHide.Enabled = (oItem.ItemFileLink.Deleted = BaseStatusDeleted.None)
                oDialogHide.CommandArgument = oItem.ItemFileLinkId.ToString
               
                If (oItem.ItemFileLink.Deleted <> BaseStatusDeleted.None) OrElse IsNothing(oItem.ItemFileLink.File) Then
                    oDialogHide.Visible = False
                Else
                    Me.Resource.setLinkButtonToValue(oDialogHide, oItem.ItemFileLink.isVisible, True, True)
                    Dim fileStatus As ModuleItemFileVisibilityStatus = GetVisibilityStatus(oItem.ItemFileLink.File, oItem.ItemFileLink.isVisible)
                    oDialogHide.ToolTip = Me.GetVisibilityStatusTranslated(fileStatus)
                    oDialogHide.CommandArgument = oItem.ItemFileLinkId.ToString
                    oDialogHide.MultiSelection = True
                    Dim selectedVisibility As New List(Of Integer)
                    Dim unselectedVisibility As New List(Of Integer)
                    If oItem.ItemFileLink.File.IsInternal OrElse Not oItem.Permission.EditRepositoryVisibility Then

                        ' options.Add(.getValue("ModuleItemFileVisibilityStatus." & ModuleItemFileVisibilityStatus.HiddenForModule.ToString))
                        ' options.Add(.getValue("ModuleItemFileVisibilityStatus." & ModuleItemFileVisibilityStatus.VisibleForModule.ToString))

                        If oItem.ItemFileLink.isVisible Then
                            selectedVisibility.Add(0)
                        Else
                            unselectedVisibility.Add(0)
                        End If
                        oDialogHide.Visible = oItem.Permission.EditVisibility
                        oDialogHide.CommandName = "editModuleVisibility"
                        oDialogHide.InitializeMultiSelectControlByClass("moduleItemVisibility", selectedVisibility) ', unselectedVisibility)
                    Else
                        'options.Add(.getValue("ModuleItemFileVisibilityStatus." & ModuleItemFileVisibilityStatus.Hidden.ToString))
                        'options.Add(.getValue("ModuleItemFileVisibilityStatus." & ModuleItemFileVisibilityStatus.HiddenForModule.ToString))
                        'options.Add(.getValue("ModuleItemFileVisibilityStatus." & ModuleItemFileVisibilityStatus.VisibleForModule.ToString))
                        'options.Add(.getValue("ModuleItemFileVisibilityStatus." & ModuleItemFileVisibilityStatus.Visible.ToString))
                        'Select Case fileStatus
                        '    Case ModuleItemFileVisibilityStatus.Hidden
                        '        oDialogHide.DefaultValue = 3
                        '    Case ModuleItemFileVisibilityStatus.HiddenForModule
                        '        oDialogHide.DefaultValue = 2
                        '    Case ModuleItemFileVisibilityStatus.VisibleForModule
                        '        oDialogHide.DefaultValue = 1
                        '    Case ModuleItemFileVisibilityStatus.Visible
                        '        oDialogHide.DefaultValue = 0
                        'End Select
                        If oItem.ItemFileLink.isVisible Then
                            selectedVisibility.Add(0)
                        Else
                            unselectedVisibility.Add(0)
                        End If
                        If Not IsNothing(oItem.ItemFileLink.File) AndAlso Not oItem.ItemFileLink.File.IsInternal AndAlso oItem.ItemFileLink.File.isVisible Then
                            selectedVisibility.Add(1)
                        Else
                            unselectedVisibility.Add(1)
                        End If
                        oDialogHide.Visible = oItem.Permission.EditRepositoryVisibility
                        oDialogHide.CommandName = "editCommunityVisibility"
                        oDialogHide.InitializeMultiSelectControlByClass("repositoryItemVisibility", selectedVisibility) ', unselectedVisibility)

                    End If
                End If


                '  oLBhiddenFile.Text = GetVisibilityStatusTranslated(oDto.ItemFileLink.File, fileStatus)
                '  oLBhiddenFile.Visible = True

               
            End If

            Dim oLiteral As Literal = e.Item.FindControl("LTitemLinkID")
            oLiteral.Text = oItem.ItemFileLinkId
            Dim oFileToDisplay As UC_ModuleToRepositoryDisplay = e.Item.FindControl("FileDisplay")
            If oItemLink.Link Is Nothing Then
                oFileToDisplay.Visible = False
            Else
                oFileToDisplay.InitializeControlByLink(oItemLink.Link, oItem.Permission)
            End If

            oLabel = e.Item.FindControl("LBdata")
            oLabel.Text = IIf(oItemLink.CreatedOn.HasValue, oItemLink.CreatedOn.Value.ToString("dd/MM/yy HH:mm"), "//")

            oLabel = e.Item.FindControl("LBauthor")
            If Not IsNothing(oItemLink.Owner) Then
                oLabel.Text = oItemLink.Owner.SurnameAndName
            Else
                oLabel.Text = ""
            End If

            oControl = e.Item.FindControl("TDstatus")

            Dim oList As IList(Of TranslatedItem(Of Long)) = oItem.AvailableStatus
            oControl.Visible = ShowManagementButtons AndAlso oList.Count > 0
            If ShowManagementButtons AndAlso oList.Count > 0 Then
                Dim oDropDownList As DropDownList = e.Item.FindControl("DDLstatus")
                oDropDownList.DataSource = oList
                oDropDownList.DataTextField = "Translation"
                oDropDownList.DataValueField = "Id"
                oDropDownList.DataBind()
                oDropDownList.SelectedValue = oItemLink.StatusId
                oDropDownList.Enabled = oItem.Permission.EditStatus
            End If

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label
            Dim oControl As HtmlControl

            oLabel = e.Item.FindControl("LBaction")
            oControl = e.Item.FindControl("THaction")
            Me.Resource.setLabel(oLabel)
            oLabel.ToolTip = Resource.getValue("LBaction.ToolTip")
            oControl.Visible = Me.ShowManagementButtons

            oLabel = e.Item.FindControl("LBpublish")
            Me.Resource.setLabel(oLabel)
            oLabel.ToolTip = Resource.getValue("LBpublish.ToolTip")
            oControl = e.Item.FindControl("THpublish")
            oControl.Visible = Me.ShowManagementButtons

            oLabel = e.Item.FindControl("LBaddedAt")
            Me.Resource.setLabel(oLabel)
            oLabel.ToolTip = Resource.getValue("LBaddedAt.ToolTip")

            oLabel = e.Item.FindControl("LBaddedBy")
            Me.Resource.setLabel(oLabel)
            oLabel.ToolTip = Resource.getValue("LBaddedBy.ToolTip")

            oLabel = e.Item.FindControl("LBproperty")
            Me.Resource.setLabel(oLabel)
            oLabel.ToolTip = Resource.getValue("LBproperty.ToolTip")

            oLabel = e.Item.FindControl("LBvisible")
            Me.Resource.setLabel(oLabel)
            oLabel.ToolTip = Resource.getValue("LBvisible.ToolTip")
            oControl = e.Item.FindControl("THvisible")
            oControl.Visible = Me.ShowManagementButtons

            oLabel = e.Item.FindControl("LBstatus")
            Me.Resource.setLabel(oLabel)
            oLabel.ToolTip = Resource.getValue("LBstatus.ToolTip")
            oControl = e.Item.FindControl("THstatus")
            oControl.Visible = Me.ShowManagementButtons AndAlso Me.ShowStatus
        End If
    End Sub
    Private Function GetVisibilityStatusTranslated(ByVal status As ModuleItemFileVisibilityStatus) As String
        Dim iResponse As String
        'If oFile.IsInternal Then
        '    iResponse = Me.Resource.getValue("visibilityFileInternal." & status.ToString)
        'Else
        iResponse = Me.Resource.getValue("visibilityFileInternal." & CInt(status).ToString)
        'End If
        Return iResponse
    End Function
    Private Function GetVisibilityStatus(ByVal oFile As BaseCommunityFile, ByVal isVisible As Boolean) As ModuleItemFileVisibilityStatus
        Dim iResponse As ModuleItemFileVisibilityStatus
        If IsNothing(oFile) Then
            iResponse = ModuleItemFileVisibilityStatus.HiddenForModule
        Else
            If oFile.IsInternal Then
                iResponse = IIf(isVisible, ModuleItemFileVisibilityStatus.VisibleForModule, ModuleItemFileVisibilityStatus.HiddenForModule)
            Else
                If isVisible AndAlso oFile.isVisible Then
                    iResponse = (ModuleItemFileVisibilityStatus.VisibleForModule Or ModuleItemFileVisibilityStatus.VisibleForCommunity)
                ElseIf Not (isVisible AndAlso oFile.isVisible) Then
                    iResponse = (ModuleItemFileVisibilityStatus.HiddenForModule Or ModuleItemFileVisibilityStatus.HiddenForCommunity)
                ElseIf isVisible AndAlso Not oFile.isVisible Then
                    iResponse = ModuleItemFileVisibilityStatus.VisibleForModule Or ModuleItemFileVisibilityStatus.HiddenForCommunity
                ElseIf Not isVisible Then
                    iResponse = ModuleItemFileVisibilityStatus.HiddenForModule Or ModuleItemFileVisibilityStatus.VisibleForCommunity
                End If
            End If
        End If
      
        Return iResponse
    End Function
    Private Sub RPTitemFiles_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTitemFiles.ItemCommand
        Dim ItemLinkId As Long = CLng(IIf(IsNumeric(e.CommandArgument), e.CommandArgument, 0))
        Select Case e.CommandName
            Case "virtualdelete"
                RaiseEvent VirtualDelete(Me.ItemID, ItemLinkId)
            Case "undelete"
                RaiseEvent UnDelete(Me.ItemID, ItemLinkId)
            Case "confirmDelete"
                RaiseEvent PhysicalDelete(Me.ItemID, ItemLinkId)
            Case "unlink"
                RaiseEvent UnlinkRepositoryItem(Me.ItemID, ItemLinkId)
                'Case "editvisibility"
                '    RaiseEvent EditItemVisibility(Me.ItemID, ItemLinkId)
                'Case "editCommunityVisibility"
                '    RaiseEvent EditCommunityItemVisibility(Me.ItemID, ItemLinkId)
        End Select
    End Sub
    Public Function GetItemFilesStatus() As IList(Of GenericItemStatus(Of Long, Long)) Implements IViewOtherModuleItemFilesLong.GetItemFilesStatus
        Dim statusList As IList(Of GenericItemStatus(Of Long, Long)) = New List(Of GenericItemStatus(Of Long, Long))

        For Each oRow As RepeaterItem In (From r As RepeaterItem In RPTitemFiles.Items Where r.ItemType = ListItemType.Item OrElse r.ItemType = ListItemType.AlternatingItem Select r).ToList
            Dim oDropDownList As DropDownList = DirectCast(oRow.FindControl("DDLstatus"), DropDownList)
            Dim oLiteral As Literal = DirectCast(oRow.FindControl("LTitemLinkID"), Literal)

            If IsNumeric(oLiteral.Text) AndAlso Not IsNothing(oDropDownList) AndAlso oDropDownList.Items.Count > 0 Then
                Dim oLinkItemStatus As New GenericItemStatus(Of Long, Long)
                oLinkItemStatus.Id = CLng(oLiteral.Text)
                oLinkItemStatus.Status = CLng(oDropDownList.SelectedValue)
                statusList.Add(oLinkItemStatus)
            End If
        Next
        '  TEST.DisplayForPlayInternal()
        Return statusList
    End Function

    Public Sub RequireUpdate() Implements IViewOtherModuleItemFilesLong.RequireUpdate

    End Sub

    Private Sub InitDialog()
        Dim options As New List(Of String)
        options.Add(Resource.getValue("ModuleItemFileVisibilityStatus.VisibleForModule"))
        Me.DLGmoduleFileItemVisibility.DialogTitle = Me.Resource.getValue("DLGmoduleFileItemVisibilityTitle")
        Me.DLGmoduleFileItemVisibility.DialogText = Me.Resource.getValue("DLGmoduleFileItemVisibilityText")
        Me.DLGmoduleFileItemVisibility.InitializeControl(options, 0, True)
     
        options = New List(Of String)
        options.Add(Resource.getValue("ModuleItemFileVisibilityStatus.VisibleForModule"))
        options.Add(Resource.getValue("ModuleItemFileVisibilityStatus.Visible"))
        Me.DLGrepositoryFileItemVisibility.DialogTitle = Me.Resource.getValue("DLGrepositoryFileItemVisibilityTitle")
        Me.DLGrepositoryFileItemVisibility.DialogText = Me.Resource.getValue("DLGrepositoryFileItemVisibilityText")
        Me.DLGrepositoryFileItemVisibility.InitializeControl(options, 0, True)

        Options = New List(Of String)
        Me.DLGremoveFile.DialogTitle = Me.Resource.getValue("DLGremoveFileTitle")
        Me.DLGremoveFile.DialogText = Me.Resource.getValue("DLGremoveFileText")
        Me.DLGremoveFile.InitializeControl(Options, -1)
    End Sub

    Private Sub DLGremoveFile_ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGremoveFile.ButtonPressed
        Dim ItemLinkId As Long = CLng(IIf(IsNumeric(CommandArgument), CommandArgument, 0))
        RaiseEvent PhysicalDelete(Me.ItemID, ItemLinkId)
    End Sub

    Private Sub DLGmoduleFileItemVisibility_ButtonPressedMulti(ByVal dialogResult As Integer, ByVal dialogResults As IList(Of Integer), ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGmoduleFileItemVisibility.ButtonPressedMulti
        Dim ItemLinkId As Long = CLng(IIf(IsNumeric(CommandArgument), CommandArgument, 0))
        Dim iResponse As ModuleItemFileVisibilityStatus
        If dialogResult < 0 AndAlso dialogResult <> -3 AndAlso dialogResult <> -1 Then
            RaiseEvent UnChangeEvent()
        Else
            'Case 0
            '    iResponse = ModuleItemFileVisibilityStatus.HiddenForModule
            'Case 1
            '    iResponse = ModuleItemFileVisibilityStatus.VisibleForModule
            'Case Else
            RaiseEvent EditFileItemVisibility(Me.ItemID, ItemLinkId, dialogResults.Contains(0), False)
        End If
    End Sub

    Private Sub DLGrepositoryFileItemVisibility_ButtonPressedMulti(ByVal dialogResult As Integer, ByVal dialogResults As System.Collections.Generic.IList(Of Integer), ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGrepositoryFileItemVisibility.ButtonPressedMulti
        Dim ItemLinkId As Long = CLng(IIf(IsNumeric(CommandArgument), CommandArgument, 0))
        Dim iResponse As ModuleItemFileVisibilityStatus
        If dialogResult < 0 AndAlso dialogResult <> -3 AndAlso dialogResult <> -1 Then
            RaiseEvent UnChangeEvent()
        Else
            RaiseEvent EditFileItemVisibility(Me.ItemID, ItemLinkId, dialogResults.Contains(0), dialogResults.Contains(1))
            'Select Case dialogResult
            '    Case 0
            '        iResponse = ModuleItemFileVisibilityStatus.Hidden
            '    Case 1
            '        iResponse = ModuleItemFileVisibilityStatus.HiddenForModule
            '    Case 2
            '        iResponse = ModuleItemFileVisibilityStatus.VisibleForModule
            '    Case 3
            '        iResponse = ModuleItemFileVisibilityStatus.Visible
            '    Case Else
            '        iResponse = ModuleItemFileVisibilityStatus.NoChange
            'End Select
        End If
    End Sub
End Class