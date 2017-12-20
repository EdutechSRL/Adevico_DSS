Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Tags.Presentation
Imports lm.Comol.Core.Tag.Domain

Public Class UC_TagsList
    Inherits TGbaseControl
    Implements IViewTagsList

#Region "Context"
    Private _presenter As TagsListPresenter
    Protected Friend ReadOnly Property CurrentPresenter As TagsListPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TagsListPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property IdTagsCommunity As Integer Implements IViewTagsList.IdTagsCommunity
        Get
            Return ViewStateOrDefault("IdTagsCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdTagsCommunity") = value
        End Set
    End Property
    Private Property CurrentPageSize As Integer Implements IViewTagsList.CurrentPageSize
        Get
            Return ViewStateOrDefault("CurrentPageSize", 25)
        End Get
        Set(value As Integer)
            ViewState("CurrentPageSize") = value
        End Set
    End Property
    Public Property IdSelectedTagLanguage As Integer Implements IViewTagsList.IdSelectedTagLanguage
        Get
            Return ViewStateOrDefault("IdSelectedTagLanguage", -1)
        End Get
        Set(value As Integer)
            ViewState("IdSelectedTagLanguage") = value
        End Set
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewTagsList.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridBottom.Pager = value
            Me.DVpager.Visible = (Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize))
        End Set
    End Property
    Private Property FirstLoad As Boolean Implements IViewTagsList.FirstLoad
        Get
            Return ViewStateOrDefault("FirstLoad", True)
        End Get
        Set(value As Boolean)
            ViewState("FirstLoad") = value
        End Set
    End Property
    Private Property FirstLoadForLanguages As Dictionary(Of Integer, Boolean) Implements IViewTagsList.FirstLoadForLanguages
        Get
            Return ViewStateOrDefault("FirstLoadForLanguages", New Dictionary(Of Integer, Boolean))
        End Get
        Set(value As Dictionary(Of Integer, Boolean))
            ViewState("FirstLoadForLanguages") = value
        End Set
    End Property
    Private Property CurrentFilters As dtoFilters Implements IViewTagsList.CurrentFilters
        Get
            Return ViewStateOrDefault("CurrentFilters", New dtoFilters(False))
        End Get
        Set(value As dtoFilters)
            ViewState("CurrentFilters") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Public Event LanguageChanged(ByVal idLanguage As Integer)
    Public Event SessionTimeout()
#End Region

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLinkButton(LNBapplyTagsFilters, False, True)
            .setLabel(LBdisplayLanguageSelectorDescription)

            .setLiteral(LTthName)
            .setLiteral(LTthUsedInCommunities)
            .setLiteral(LTthTranslations)
            .setLiteral(LTthModifiedBy)
            .setLiteral(LTthModifiedOn)
            LBactions.ToolTip = .getValue("LBactions.ToolTip")

            .setLinkButton(LNBorderByNameUp, False, True)
            .setLinkButton(LNBorderByNameDown, False, True)
            .setLinkButton(LNBorderByUsedByUp, False, True)
            .setLinkButton(LNBorderByUsedByDown, False, True)
            .setLinkButton(LNBorderByModifiedByUp, False, True)
            .setLinkButton(LNBorderByModifiedByDown, False, True)
            .setLinkButton(LNBorderByModifiedOnDown, False, True)
            .setLinkButton(LNBorderByModifiedOnUp, False, True)


            .setLabel(LBtagsStatistics)
            .setLabel(LBtagsNotTranslated_t)
            .setLabel(LBcommunitiesWithNoTags_t)
            .setHyperLink(HYPcommunitiesWithNoTags, False, True)

            .setLabel(LBtableLegend)
            .setLabel(LBspanExpandList)
            .setLabel(LBspanCollapseList)
            .setLiteral(LTsearchTagFiltersTitle)
            .setButton(BTNreloadItems)

            LTdraftItem.Text = String.Format(LTtemplateLegendItem.Text, .getValue("Tag.Draft"), LTcssClassDraft.Text)
            LTdefaultItem.Text = String.Format(LTtemplateLegendItem.Text, .getValue("Tag.isDefault"), LTcssClassDefault.Text)

        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(permissions As ModuleTags, idCommunity As Integer, Optional fromRecycleBin As Boolean = False, Optional fromOrganization As Boolean = False) Implements IViewTagsList.InitializeControl
        CurrentPresenter.InitView(permissions, idCommunity, fromRecycleBin, fromOrganization)
    End Sub
    Private Sub LoadLanguages(items As List(Of lm.Comol.Core.Dashboard.Domain.dtoItemFilter(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem))) Implements IViewTagsList.LoadLanguages
        If items.Count > 1 Then
            DVlanguageSelector.Visible = True
            RPTdisplayLanguage.DataSource = items
            RPTdisplayLanguage.DataBind()
            If items.Where(Function(i) i.Selected).Any Then
                LBdisplayLanguageSelected.Text = items.Where(Function(i) i.Selected).FirstOrDefault.Value.LanguageName
            Else
                LBdisplayLanguageSelected.Text = ""
            End If
        Else
            DVlanguageSelector.Visible = False
        End If
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission

    End Sub
    Private Sub DisplayMessage(action As ModuleTags.ActionType) Implements IViewTagsList.DisplayMessage
        Dim mType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        Select Case action
            Case ModuleTags.ActionType.DisableTag, ModuleTags.ActionType.EnableTag, ModuleTags.ActionType.VirtualDelete, ModuleTags.ActionType.VirtualUndelete, ModuleTags.ActionType.AddedBulkTagsToOrganization, ModuleTags.ActionType.AddedBulkTagsToPortal
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        End Select
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayMessage.ModuleTags.ActionType." & action.ToString), mType)
    End Sub
    Private Sub DisplayMessage(action As ModuleTags.ActionType, tags As List(Of String)) Implements IViewTagsList.DisplayMessage
        Dim mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        Dim message As String = Resource.getValue("DisplayMessage.ModuleTags.ActionType." & action.ToString)
        If (message.Contains("{0}")) Then
            Dim invalidTags As String = LTtemplateMessageDetails.Text
            invalidTags = String.Format(invalidTags, String.Join("", tags.Select(Function(s) String.Format(LTtemplateMessageDetail.Text, s)).ToList()))
            message = String.Format(message, invalidTags)
        End If
        CTRLmessages.InitializeControl(message, mType)
    End Sub
    Private Sub DisplayErrorLoadingFromDB() Implements IViewTagsList.DisplayErrorLoadingFromDB
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayErrorLoadingFromDB"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        RaiseEvent SessionTimeout()
    End Sub
    Private Sub AllowApplyFilters(allow As Boolean) Implements IViewTagsList.AllowApplyFilters
        DVfilters.Visible = allow
    End Sub
    Private Function GetDefaultLanguageName() As String Implements IViewTagsList.GetDefaultLanguageName
        Return Resource.getValue("GetDefaultLanguageName")
    End Function
    Private Function GetDefaultLanguageCode() As String Implements IViewTagsList.GetDefaultLanguageCode
        Return Resource.getValue("GetDefaultLanguageCode")
    End Function
    Private Function GetUnknownUserName() As String Implements IViewTagsList.GetUnknownUserName
        Return Resource.getValue("UnknownUserName")
    End Function
    Private Sub LoadTagsInfo(communititesWithNoTags As Integer, unstranslatedTags As Integer, Optional unusedTags As Integer = 0, Optional ByVal url As String = "") Implements IViewTagsList.LoadTagsInfo
        DVstatistics.Visible = True
        LBcommunitiesWithNoTags.Text = communititesWithNoTags.ToString
        LBtagsNotTranslated.Text = unstranslatedTags.ToString
        If communititesWithNoTags > 0 Then
            LBcommunitiesWithNoTags.CssClass = LTcssClassInfoToDo.Text
        Else
            LBcommunitiesWithNoTags.CssClass = LTcssClassInfo.Text
        End If
        If unstranslatedTags > 0 Then
            LBtagsNotTranslated.CssClass = LTcssClassInfoWarning.Text
        Else
            LBtagsNotTranslated.CssClass = LTcssClassInfo.Text
        End If
        If communititesWithNoTags > 0 AndAlso Not String.IsNullOrEmpty(url) Then
            LBcommunitiesWithNoTags_t.Visible = False
            HYPcommunitiesWithNoTags.Visible = True
            HYPcommunitiesWithNoTags.NavigateUrl = BaseUrl & url
        Else
            LBcommunitiesWithNoTags_t.Visible = True
            HYPcommunitiesWithNoTags.Visible = False
        End If
    End Sub
    Private Function GetSubmittedFilters() As dtoFilters Implements IViewTagsList.GetSubmittedFilters
        Dim filter As dtoFilters = CurrentFilters

        With filter
            Dim keys As List(Of String) = Request.Form.AllKeys.ToList()
            For Each item As lm.Comol.Core.Tag.Domain.searchFilterType In [Enum].GetValues(GetType(lm.Comol.Core.Tag.Domain.searchFilterType))
                Select Case item
                    Case lm.Comol.Core.Tag.Domain.searchFilterType.createdby
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdCreatedBy = CInt(Request.Form(item.ToString))
                        Else
                            .IdCreatedBy = -1
                        End If
                    Case lm.Comol.Core.Tag.Domain.searchFilterType.organization
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdOrganization = CInt(Request.Form(item.ToString))
                        Else
                            .IdOrganization = IIf(filter.ForOrganization, -1, -3)
                        End If
                    Case lm.Comol.Core.Tag.Domain.searchFilterType.status
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .Status = CInt(Request.Form(item.ToString))
                        Else
                            .Status = lm.Comol.Core.Dashboard.Domain.AvailableStatus.Any
                        End If
                    Case lm.Comol.Core.Tag.Domain.searchFilterType.name
                        .Name = Request.Form(item.ToString)
                    Case lm.Comol.Core.Tag.Domain.searchFilterType.letters
                        If Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            Dim charInt As Integer = CInt(Request.Form(item.ToString))
                            Select Case charInt
                                Case -1
                                    .StartWith = ""
                                Case -9
                                    .StartWith = "#"
                                Case Else
                                    .StartWith = Char.ConvertFromUtf32(charInt).ToLower()
                            End Select
                        End If
                End Select
            Next
        End With

        Return filter
    End Function
    Private Sub LoadTags(items As List(Of dtoTagItem), idLanguage As Integer) Implements IViewTagsList.LoadTags
        RPTtags.DataSource = items
        RPTtags.DataBind()
        Dim display As Boolean = items.Any AndAlso items.Count > 1
        LNBorderByNameUp.Visible = display
        LNBorderByNameDown.Visible = display
        LNBorderByUsedByUp.Visible = display
        LNBorderByUsedByDown.Visible = display
        LNBorderByModifiedByUp.Visible = display
        LNBorderByModifiedByDown.Visible = display
        LNBorderByModifiedOnDown.Visible = display
        LNBorderByModifiedOnUp.Visible = display
        FirstLoad = False
        FirstLoadForLanguages(idLanguage) = False
        DVlegend.Visible = items.Any()

    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleTags.ActionType) Implements IViewTagsList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idTag As Long, action As ModuleTags.ActionType) Implements IViewTagsList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleTags.ObjectType.Tag, idTag.ToString), InteractionType.UserWithLearningObject)
    End Sub
#End Region

#Region "Internal"
    Private Sub LNBapplyTagsFilters_Click(sender As Object, e As EventArgs) Handles LNBapplyTagsFilters.Click
        Dim filters As dtoFilters = GetSubmittedFilters()
        CTRLmessages.Visible = False
        CurrentPresenter.ApplyFilters(filters, IdTagsCommunity, CurrentPageSize)
    End Sub
    Private Sub RPTdisplayLanguage_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTdisplayLanguage.ItemDataBound
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBdisplayLanguage")
        Dim oItem As lm.Comol.Core.Dashboard.Domain.dtoItemFilter(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem) = e.Item.DataItem

        oLinkButton.CommandArgument = CInt(oItem.Value.IdLanguage)
        If oLinkButton.Text.Contains("{0}") Then
            oLinkButton.Text = String.Format(oLinkButton.Text, oItem.Value.LanguageName)
        End If

        Dim oControl As HtmlControl = e.Item.FindControl("DVitemDisplayLanguage")
        oControl.Attributes("class") = LTcssClassSelectBy.Text & " " & GetOrderByItemCssClass(oItem)
    End Sub
    Private Sub RPTdisplayLanguage_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTdisplayLanguage.ItemCommand
        Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBdisplayLanguage")
        LBdisplayLanguageSelected.Text = oLinkbutton.Text

        Dim oControl As HtmlControl = e.Item.FindControl("DVitemDisplayLanguage")
        If Not oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
            oControl.Attributes("class") = oControl.Attributes("class") & " " & LTcssClassActive.Text

            For Each row As RepeaterItem In (From r As RepeaterItem In RPTdisplayLanguage.Items Where r.ItemIndex <> e.Item.ItemIndex)
                oControl = row.FindControl("DVitemDisplayLanguage")
                If oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
                    oControl.Attributes("class") = Replace(oControl.Attributes("class"), LTcssClassActive.Text, "")
                    Exit For
                End If
            Next
        End If
        RaiseEvent LanguageChanged(CInt(e.CommandArgument))
        Dim filter As dtoFilters = CurrentFilters
        filter.IdSelectedLanguage = CInt(e.CommandArgument)
        filter.Ascending = True
        filter.OrderBy = OrderTagsBy.Name
        filter.Name = ""
        filter.StartWith = ""
        CurrentFilters = filter
        CTRLmessages.Visible = False
        CurrentPresenter.LoadTags(filter, IdTagsCommunity, 0, CurrentPageSize)
    End Sub
    Private Sub RPTtags_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTtags.ItemCommand
        'Dim oResourceConfig As New ResourceManager

        If PageUtility.CurrentContext.UserContext.isAnonymous Then
            RaiseEvent SessionTimeout()
        Else
            Select Case e.CommandName
                Case "hide"
                    CurrentPresenter.SetStatus(CLng(e.CommandArgument), lm.Comol.Core.Dashboard.Domain.AvailableStatus.Unavailable, CurrentFilters, IdTagsCommunity, Pager.PageIndex, CurrentPageSize)
                Case "show"
                    CurrentPresenter.SetStatus(CLng(e.CommandArgument), lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available, CurrentFilters, IdTagsCommunity, Pager.PageIndex, CurrentPageSize)
                Case "virtualdelete"
                    CurrentPresenter.VirtualDelete(CLng(e.CommandArgument), True, CurrentFilters, IdTagsCommunity, Pager.PageIndex, CurrentPageSize)
                Case "recover"
                    CurrentPresenter.VirtualDelete(CLng(e.CommandArgument), False, CurrentFilters, IdTagsCommunity, Pager.PageIndex, CurrentPageSize)
            End Select
        End If
    End Sub

    Private Sub RPTtags_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTtags.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As dtoTagItem = e.Item.DataItem

                Dim oLabel As Label = e.Item.FindControl("LBtagLanguageInUse")
                Dim translation As lm.Comol.Core.DomainModel.Languages.dtoLanguageItem = dto.Translations.Where(Function(t) t.IdLanguage.Equals(dto.IdDisplayLanguage)).FirstOrDefault()
                If IsNothing(translation) Then
                    oLabel.Visible = False
                ElseIf translation.IdLanguage = -1 Then
                    oLabel.Visible = True
                    oLabel.Text = translation.ShortCode
                    oLabel.ToolTip = Resource.getValue("LBtagLanguageInUse-1")
                Else
                    oLabel.Text = translation.ShortCode
                    oLabel.Visible = True
                    oLabel.ToolTip = String.Format(Resource.getValue("LBtagLanguageInUse"), translation.LanguageName)
                End If
                oLabel = e.Item.FindControl("LBmodifiedOn")
                oLabel.Text = GetDateToString(dto.ModifiedOn, "-")
                oLabel.ToolTip = GetDateTimeString(dto.ModifiedOn, "-")

                Dim oLiteral As Literal = e.Item.FindControl("LTviewTag")
                oLiteral.Visible = dto.Permissions.AllowView
                If dto.Permissions.AllowView Then
                    oLiteral.Text = String.Format(oLiteral.Text, dto.Id, Resource.getValue("LTviewTag.ToolTip"))
                End If

                oLiteral = e.Item.FindControl("LTassignTag")
                oLiteral.Visible = dto.Permissions.AllowAssignTo
                If dto.Permissions.AllowAssignTo Then
                    oLiteral.Text = String.Format(oLiteral.Text, dto.Id, dto.IdDisplayLanguage, Resource.getValue("LTassignTag.ToolTip"))
                End If
                oLiteral = e.Item.FindControl("LTeditTag")
                oLiteral.Visible = dto.Permissions.AllowEdit
                If dto.Permissions.AllowEdit Then
                    oLiteral.Text = String.Format(oLiteral.Text, dto.Id, Resource.getValue("LTeditTag.ToolTip"))
                End If

                Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBhideTag")
                oLinkbutton.Visible = dto.Permissions.AllowSetUnavailable
                Resource.setLinkButton(oLinkbutton, False, True)

                oLinkbutton = e.Item.FindControl("LNBshowTag")
                oLinkbutton.Visible = dto.Permissions.AllowSetAvailable
                Resource.setLinkButton(oLinkbutton, False, True)

                oLinkbutton = e.Item.FindControl("LNBvirtualDeleteTag")
                oLinkbutton.Visible = dto.Permissions.AllowVirtualDelete
                Resource.setLinkButton(oLinkbutton, False, True)

                oLinkbutton = e.Item.FindControl("LNBvirtualUnDeleteTag")
                oLinkbutton.Visible = dto.Permissions.AllowUnDelete
                Resource.setLinkButton(oLinkbutton, False, True)

            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                oTableItem.Visible = (RPTtags.Items.Count = 0)
                If (RPTtags.Items.Count = 0) Then
                    Dim oLabel As Label = e.Item.FindControl("LBemptyItems")
                    If FirstLoad Then
                        oLabel.Text = Resource.getValue("NoTagsFound.FirstLoad." & CurrentFilters.FromRecycleBin)
                    ElseIf FirstLoadForLanguages(CurrentFilters.IdSelectedLanguage) Then
                        oLabel.Text = Resource.getValue("NoTagsFound.ForLanguage")
                    Else
                        oLabel.Text = Resource.getValue("NoTagsFound.Filters")
                    End If
                End If
        End Select
    End Sub
    Protected Sub RPTlanguages_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        Dim dto As lm.Comol.Core.DomainModel.Languages.dtoLanguageItem = e.Item.DataItem

        Dim oLabel As Label = e.Item.FindControl("LBtemplateLanguage")
        oLabel.Text = dto.ShortCode
        If dto.IdLanguage > 0 Then
            oLabel.ToolTip = dto.LanguageName
        Else
            oLabel.ToolTip = Resource.getValue("LBtemplateLanguage.Default")
        End If

    End Sub
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        CTRLmessages.Visible = False
        CurrentPresenter.LoadTags(CurrentFilters, IdTagsCommunity, Pager.PageIndex, CurrentPageSize)
    End Sub
    Private Sub BTNreloadItems_Click(sender As Object, e As EventArgs) Handles BTNreloadItems.Click
        CTRLmessages.Visible = False
        CurrentPresenter.LoadTags(CurrentFilters, IdTagsCommunity, Pager.PageIndex, CurrentPageSize)
    End Sub
    Private Sub CTRLedit_BulkInsert(results As List(Of Dictionary(Of Integer, String))) Handles CTRLedit.BulkInsert
        CurrentPresenter.BulkInsert(CurrentFilters, IdTagsCommunity, Pager.PageIndex, CurrentPageSize, results)
    End Sub
    Protected Sub LNBorderBy_Click(sender As Object, e As System.EventArgs)
        Dim ascending As Boolean = CBool(DirectCast(sender, LinkButton).CommandArgument)
        Dim orderBy As OrderTagsBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of OrderTagsBy).GetByString(DirectCast(sender, LinkButton).CommandName, OrderTagsBy.Name)

        Dim filter As dtoFilters = CurrentFilters
        filter.Ascending = ascending
        filter.OrderBy = orderBy
        CurrentFilters = filter
        CTRLmessages.Visible = False
        CurrentPresenter.LoadTags(filter, IdTagsCommunity, 0, CurrentPageSize)
    End Sub
    Public Function GetOrderByItemCssClass(ByVal item As lm.Comol.Core.Dashboard.Domain.dtoItemFilter(Of lm.Comol.Core.DomainModel.Languages.dtoLanguageItem)) As String
        Dim cssClass As String = GetItemCssClass(item.DisplayAs)
        If item.Selected Then
            cssClass &= " " & LTcssClassActive.Text
        End If
        Return cssClass
    End Function
    Private Function GetItemCssClass(ByVal d As lm.Comol.Core.DomainModel.ItemDisplayOrder) As String
        Dim cssClass As String = ""
        Select Case d
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.first, lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                cssClass = " " & d.ToString
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.item
                cssClass = ""
            Case Else
                cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString() & " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString()
        End Select
        Return cssClass
    End Function
    Public Function GetItemCssClass(item As dtoTagItem) As String
        Dim cssClass As String = ""
        If item.IsDefault Then
            cssClass = LTcssClassDefault.Text
        End If
        If item.Status = lm.Comol.Core.Dashboard.Domain.AvailableStatus.Draft Then
            cssClass &= " " & LTcssClassDraft.Text
        End If
        Return cssClass
    End Function
#End Region

End Class