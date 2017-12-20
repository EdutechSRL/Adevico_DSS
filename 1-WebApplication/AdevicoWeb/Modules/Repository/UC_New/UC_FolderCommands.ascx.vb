Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_FolderCommands
    Inherits FRbaseControl

#Region "Internal"
    Private _AutoPostBack As Boolean
    Public Property AutoPostBack As Boolean
        Get
            Return _AutoPostBack
        End Get
        Set(value As Boolean)
            _AutoPostBack = value
        End Set
    End Property
    Private Property CurrentOrderBy As OrderBy
    Private Property CurrentAscending As Boolean
    Private Property TemplateUrl As String
        Get
            Return ViewStateOrDefault("TemplateUrl", "")
        End Get
        Set(value As String)
            ViewState("TemplateUrl") = value
        End Set
    End Property
    Public Event ItemCommand(ByVal action As ItemAction)
    Public Event ItemLoadFolder(idFolder As Long, path As String, type As FolderType)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBcommandFolder_t)
            .setLabel(LBcommandResources_t)
            .setLabel(LBcommandSelectedResources_t)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(actions As List(Of ItemAction), quota As dtoContainerQuota, idFather As Long, fatherPath As String, fatherType As FolderType, folderName As String, fatherUrl As String, fatherName As String, order As OrderBy, ascending As Boolean)
        CurrentOrderBy = order
        CurrentAscending = ascending
        Dim hyperlinks As List(Of HyperLink) = Me.Controls.OfType(Of HyperLink)().ToList
        Dim linkbuttons As List(Of LinkButton) = Me.Controls.OfType(Of LinkButton)().ToList

        If actions.Any Then
            TemplateUrl = fatherUrl
            MLVcommands.SetActiveView(VIWcommands)
            Dim placeHolders As New Dictionary(Of Integer, Boolean)
            placeHolders.Add(0, actions.Any(Function(a) a = ItemAction.addfolder OrElse a = ItemAction.gotofolderfather))
            placeHolders.Add(1, actions.Any(Function(a) a = ItemAction.addlink OrElse a = ItemAction.upload))
            placeHolders.Add(2, actions.Any(Function(a) a = ItemAction.allowSelection OrElse Not (a = ItemAction.move OrElse a = ItemAction.addfolder OrElse a = ItemAction.gotofolderfather OrElse a = ItemAction.addlink OrElse a = ItemAction.upload)))
            '  placeHodlers.Add(2, actions.Any(Function(a) Not (a = ItemAction.addfolder OrElse a = ItemAction.gotofolderfather OrElse a = ItemAction.addlink OrElse a = ItemAction.upload)))
            SPNfolder.Visible = placeHolders(0)
            If placeHolders(0) Then
                If String.IsNullOrEmpty(fatherUrl) Then
                    HYPgotofolderfather.Visible = False
                    LNBgotofolderfather.Visible = False
                Else
                    LNBgotofolderfather.Visible = AutoPostBack
                    LNBgotofolderfather.CommandArgument = fatherPath & "|" & fatherType.ToString
                    LNBgotofolderfather.CommandName = idFather
                    HYPgotofolderfather.Visible = Not AutoPostBack
                    HYPgotofolderfather.NavigateUrl = PageUtility.ApplicationUrlBase() + Replace(Replace(fatherUrl, "#OrderBy#", order.ToString), "#Boolean#", ascending.ToString().ToLower)
                    If String.IsNullOrEmpty(fatherName) OrElse String.IsNullOrEmpty(Resource.getValue("HYPgotofolderfather.ToolTip.name")) OrElse Not Resource.getValue("HYPgotofolderfather.ToolTip.name").Contains("{0}") Then
                        Resource.setHyperLink(HYPgotofolderfather, False, True)
                    Else
                        HYPgotofolderfather.ToolTip = String.Format(Resource.getValue("HYPgotofolderfather.ToolTip.name"), fatherName)
                    End If
                    LNBgotofolderfather.ToolTip = HYPgotofolderfather.ToolTip
                End If
                HYPaddFolder.Visible = actions.Contains(ItemAction.addfolder)
                Resource.setHyperLink(HYPaddFolder, False, True)
            End If
            SPNresources.Visible = placeHolders(1)
            If placeHolders(1) Then
                Resource.setHyperLink(HYPupload, False, True)
                HYPupload.Visible = actions.Contains(ItemAction.upload)
                Resource.setHyperLink(HYPaddlink, False, True)
                HYPaddlink.Visible = actions.Contains(ItemAction.addlink)
            End If
            SPNselected.Visible = placeHolders(2)
            If placeHolders(2) Then
                LNBhideItems.Visible = actions.Contains(ItemAction.hide)
                LNBhideItems.Attributes("data-command") = CInt(ItemAction.hide)
                Resource.setLinkButton(LNBhideItems, False, True)
                LNBshowItems.Visible = actions.Contains(ItemAction.show)
                LNBshowItems.Attributes("data-command") = CInt(ItemAction.show)
                Resource.setLinkButton(LNBshowItems, False, True)
                LNBvirtualDeleteItems.Visible = actions.Contains(ItemAction.virtualdelete)
                LNBvirtualDeleteItems.Attributes("data-command") = CInt(ItemAction.virtualdelete)
                Resource.setLinkButton(LNBvirtualDeleteItems, False, True)
                LNBvirtualUndeleteItems.Visible = actions.Contains(ItemAction.undelete)
                LNBvirtualUndeleteItems.Attributes("data-command") = CInt(ItemAction.undelete)
                Resource.setLinkButton(LNBvirtualUndeleteItems, False, True)
                LNBphisicalDeleteItems.Visible = actions.Contains(ItemAction.delete)
                LNBphisicalDeleteItems.Attributes("data-command") = CInt(ItemAction.delete)
                Resource.setLinkButton(LNBphisicalDeleteItems, False, True)
            End If
        Else
            TemplateUrl = ""
            MLVcommands.SetActiveView(VIWempty)
        End If
        ' none = 0,
        'preview = 1,
        'play = 2,
        'download = 3,
        'details= 4,
        'link = 5,
        'zip = 6,
        'unzip = 7,
        'hide =8,
        'show = 9,
        'move = 10,
        'virtualdelete = 11,
        'undelete = 12,
        'delete = 13,
        'edit = 14,
        'editPermission = 15,
        'viewPermission = 16,        
        'editSettings = 17,
        'manageVersions = 18,
        'addVersion = 19,
        'removeVersion = 20,
        'setCurrentVersion = 21,
        'viewMyStatistics = 22,
        'viewOtherStatistics = 23,
        'allowSelection = 24,
        'upload = 25,
        'addfolder = 26,
        ' addlink = 27,
        ' gotofolderfather = 28
        'If controls.Any(Function(c) c.ID.Contains("LTclientScript" & dto.Item.Action.ToString)) Then
        '    oLinkButton.OnClientClick = controls.Where(Function(c) c.ID.Contains("LTclientScript" & dto.Item.Action.ToString)).FirstOrDefault().Text
        'End If
        'HYProotFolder.NavigateUrl = BaseUrl & rootUrl
        'MLVcontent.SetActiveView(VIWbreadCrumb)
        'If IsNothing(folders) OrElse Not folders.Any() Then
        '    isSingleItem = True
        '    RPTfolders.DataBind()
        'Else
        '    MLVcontent.SetActiveView(VIWbreadCrumb)
        '    Dim items As New List(Of lm.Comol.Core.DomainModel.GenericOrderItem(Of dtoFolderItem))
        '    items = folders.Select(Function(o) New lm.Comol.Core.DomainModel.GenericOrderItem(Of dtoFolderItem) With {.DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.item, .Item = o, .DisplayName = o.Name}).ToList()
        '    If items.Count > 1 Then
        '        items.FirstOrDefault().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.first
        '    Else
        '        isSingleItem = True
        '    End If
        '    items.LastOrDefault().DisplayAs = lm.Comol.Core.DomainModel.ItemDisplayOrder.last
        '    RPTfolders.DataSource = items
        '    RPTfolders.DataBind()
        'End If
    End Sub

 
    Public Sub UpdateItemsUrl(orderBy As OrderBy, ascending As Boolean)
        CurrentOrderBy = orderBy
        CurrentAscending = ascending
        If SPNfolder.Visible AndAlso HYPgotofolderfather.Visible AndAlso Not String.IsNullOrWhiteSpace(TemplateUrl) Then
            HYPgotofolderfather.NavigateUrl = PageUtility.ApplicationUrlBase & Replace(Replace(TemplateUrl, "#OrderBy#", orderBy.ToString), "#Boolean#", ascending.ToString().ToLower)
        End If
        'For Each row As RepeaterItem In RPTfolders.Items
        '    Dim oRepeater As Repeater = row.FindControl("RPTchildren")
        '    For Each rowChild As RepeaterItem In oRepeater.Items
        '        Dim oLiteral As Literal = rowChild.FindControl("LTfolder")
        '        Dim oHyperLink As HyperLink = rowChild.FindControl("HYPfolder")
        '        oHyperLink.NavigateUrl = BaseUrl & Replace(Replace(oLiteral.Text, "#OrderBy#", orderBy.ToString), "#Boolean#", ascending.ToString().ToLower)
        '    Next
        'Next
    End Sub
    Private Sub LNBhideItems_Click(sender As Object, e As EventArgs) Handles LNBhideItems.Click
        RaiseEvent ItemCommand(ItemAction.hide)
    End Sub
    Private Sub LNBphisicalDeleteItems_Click(sender As Object, e As EventArgs) Handles LNBphisicalDeleteItems.Click
        RaiseEvent ItemCommand(ItemAction.delete)
    End Sub
    Private Sub LNBshowItems_Click(sender As Object, e As EventArgs) Handles LNBshowItems.Click
        RaiseEvent ItemCommand(ItemAction.show)
    End Sub
    Private Sub LNBvirtualDeleteItems_Click(sender As Object, e As EventArgs) Handles LNBvirtualDeleteItems.Click
        RaiseEvent ItemCommand(ItemAction.virtualdelete)
    End Sub
    Private Sub LNBvirtualUndeleteItems_Click(sender As Object, e As EventArgs) Handles LNBvirtualUndeleteItems.Click
        RaiseEvent ItemCommand(ItemAction.undelete)
    End Sub
    Private Sub LNBgotofolderfather_Click(sender As Object, e As EventArgs) Handles LNBgotofolderfather.Click
        Dim idFolder As Long = -1
        Long.TryParse(LNBgotofolderfather.CommandName, idFolder)
        Dim items As List(Of String) = LNBgotofolderfather.CommandArgument.Split("|").ToList()
        RaiseEvent ItemLoadFolder(idFolder, items.FirstOrDefault(), lm.Comol.Core.DomainModel.Helpers.EnumParser(Of FolderType).GetByString(items.Last(), FolderType.standard))
    End Sub
#End Region

    
End Class