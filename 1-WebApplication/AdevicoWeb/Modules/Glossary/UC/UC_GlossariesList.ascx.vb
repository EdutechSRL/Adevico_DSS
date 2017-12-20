Imports System.Text
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.UC

Public Class UC_GlossariesList
    Inherits GLbaseControl
    Implements IViewUC_GlossariesList

    Public Const GlossaryDeleteCommandString As String = "GlossaryDelete"

#Region "Context"

    Private _Presenter As UC_GlossariesListPresenter

    Private ReadOnly Property CurrentPresenter() As UC_GlossariesListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UC_GlossariesListPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Public Property CommunityGlossaryList As List(Of DTO_Glossary)
        Get
            Return ViewStateOrDefault("CommunityGlossaryList", New List(Of DTO_Glossary))
        End Get
        Set(value As List(Of DTO_Glossary))
            ViewState("CommunityGlossaryList") = value
        End Set
    End Property

    Public Property PublicGlossaryList As List(Of DTO_Glossary)
        Get
            Return ViewStateOrDefault("PublicGlossaryList", New List(Of DTO_Glossary))
        End Get
        Set(value As List(Of DTO_Glossary))
            ViewState("PublicGlossaryList") = value
        End Set
    End Property

    Public Property InternalGlossary As Boolean
        Get
            Return ViewStateOrDefault("InternalGlossary", True)
        End Get
        Set(value As Boolean)
            ViewState("InternalGlossary") = value
        End Set
    End Property

    Public Property CommunitySort As SortObject
        Get
            Return ViewStateOrDefault("CommunitySort", New SortObject())
        End Get
        Set(value As SortObject)
            ViewState("CommunitySort") = value
        End Set
    End Property

    Public Property PublicSort As SortObject
        Get
            Return ViewStateOrDefault("PublicSort", New SortObject())
        End Get
        Set(value As SortObject)
            ViewState("PublicSort") = value
        End Set
    End Property

#End Region

#Region "Overrides"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'PGgrid.Pager = Pager
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPmanageGlossary, False, False, False, False)
            .setHyperLink(HYPrecycleBin, False, False, False, False)
            .setHyperLink(HYPglossaryImportIntoCommunity, String.Empty, False, False, False)

            .setHyperLink(HYPglossaryFromFile, String.Empty, True, False, False, False)
            .setHyperLink(HYPglossaryToFile, String.Empty, True, False, False, False)

            .setLiteral(LTcommunityGlossaries)
            .setLiteral(LTpublicGlossaries)
            .setLiteral(LTcreateNewGlossary)
            .setLiteral(LTlegend)
            .setLiteral(LTwaitingForApproval)
            .setLiteral(LTexternalShared)
            .setLiteral(LTexternalPublic)
            .setLiteral(LTinternalPublic)
            .setLiteral(LTinternalShared)
            .setLiteral(LTunpublished)
            .setLabel(LBorderBySelectorDescription)
            .setLabel(LBorderBySelectorDescriptionPublic)

            .setLabel(LBclickExpand_t)
            .setLabel(LBclickCollapse_t)
            .setLabel(LBlemma_t)
            .setLabel(LBlemmacontent_t)
            .setLabel(LBterms_t)
            .setLinkButton(LNBsearchApply, True, True, False, False)
            .setLinkButton(LNBreset, True, True, False, False)
            .setLinkButton(LNBsearch, True, True, False, False)

            .setLiteral(LTsearchTools_t)

            '.setLiteral(LTimportGlossariesIntoCommunity)
            '.setLiteral(LTImportGlossariesFromFile)
            '.setLiteral(LTExportGlossariesToFile)

        End With
    End Sub

#End Region

#Region "Custom"

    Private Property ForManagement() As Boolean Implements IViewUC_GlossariesList.ForManagement
        Get
            Return ViewStateOrDefault("ForManagement", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ForManagement") = value
        End Set
    End Property

    Private Property BindingCommunity() As Boolean
        Get
            Return ViewStateOrDefault("BindingCommunity", True)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("BindingCommunity") = value
        End Set
    End Property

    Private Property CommandSortDictionary() As List(Of SortObject)
        Get
            Return ViewStateOrDefault("CommandSortDictionary", New List(Of SortObject))
        End Get
        Set(ByVal value As List(Of SortObject))
            Me.ViewState("CommandSortDictionary") = value
        End Set
    End Property

    Public Property IsMultiFilter As Boolean
        Get
            Return ViewStateOrDefault("isMultiFilter", False)
        End Get
        Set(value As Boolean)
            ViewState("isMultiFilter") = value
        End Set
    End Property

    Public Sub InitializeControl(ByVal idCommunity As Integer, ByVal manageEnabled As Boolean) Implements IViewUC_GlossariesList.InitializeControl
        ForManagement = manageEnabled
        Me.IdCommunity = idCommunity
        HYPglossaryNew.NavigateUrl = String.Format("{0}{1}", PageUtility.BaseUrl, RootObject.GlossaryAdd(Me.IdCommunity))
        HYPglossaryNew2.NavigateUrl = String.Format("{0}{1}", PageUtility.BaseUrl, RootObject.GlossaryAdd(Me.IdCommunity))

        'HYPglossaryImportExport.NavigateUrl = String.Format("{0}{1}", PageUtility.BaseUrl, RootObject.GlossaryImportExportXLM(Me.IdCommunity))

        PNLAddTile.Visible = manageEnabled
        If manageEnabled Then
            'HYPmanageShareGlossary.NavigateUrl = String.Format("{0}{1}", PageUtility.BaseUrl, RootObject.GlossaryList(Me.IdCommunity, -1, False, Not manage))
            HYPrecycleBin.NavigateUrl = String.Format("{0}{1}", PageUtility.BaseUrl, RootObject.RecycleBin(Me.IdCommunity))
            HYPmanageGlossary.NavigateUrl = String.Format("{0}{1}", PageUtility.BaseUrl, RootObject.GlossaryManage(Me.IdCommunity))
            HYPglossaryImportIntoCommunity.NavigateUrl = String.Format("{0}{1}", PageUtility.BaseUrl, RootObject.GlossaryImport(Me.IdCommunity))
            HYPglossaryFromFile.NavigateUrl = String.Format("{0}{1}", PageUtility.BaseUrl, RootObject.GlossaryImportFromFileCommunity(Me.IdCommunity))
            HYPglossaryToFile.NavigateUrl = String.Format("{0}{1}", PageUtility.BaseUrl, RootObject.GlossaryExportToFileCommunity(Me.IdCommunity))
        Else
            'HYPmanageShareGlossary.Visible = False
            HYPrecycleBin.Visible = False
            HYPmanageGlossary.Visible = False
            HYPglossaryImportIntoCommunity.Visible = False
            HYPglossaryFromFile.Visible = False
        End If

        CurrentPresenter.InitView(ForManagement)

        CommandSortDictionary = New List(Of SortObject)
        CommandSortDictionary.Add(New SortObject("default", Function(dtoGlossary) dtoGlossary.DisplayOrder))
        CommandSortDictionary.Add(New SortObject("alphabetical", Function(dtoGlossary) dtoGlossary.Name))
        CommandSortDictionary.Add(New SortObject("lastUpdate", Function(dtoGlossary) dtoGlossary.LastUpdate))

        PublicSort = CommandSortDictionary(0)
        CommunitySort = CommandSortDictionary(0)

        UpdateSelectedSortLabel(PublicSort, True)
        UpdateSelectedSortLabel(CommunitySort, False)
    End Sub

    Private Sub LoadViewData(glossaries As List(Of DTO_Glossary), publicGlossaries As List(Of DTO_Glossary)) Implements IViewUC_GlossariesList.LoadViewData
        BindingCommunity = True
        CommunityGlossaryList = glossaries
        InternalGlossary = True
        RPTlist.DataSource = CommunityGlossaryList
        RPTlist.DataBind()
        BindingCommunity = False
        PublicGlossaryList = publicGlossaries
        InternalGlossary = False

        RPTpublic.DataSource = PublicGlossaryList
        RPTpublic.DataBind()
        DDLsearchType.Items.Clear()
        DDLsearchType.DataSource = GetDDLElements(GetType(FilterTypeEnum))
        DDLsearchType.DataTextField = "Description"
        DDLsearchType.DataValueField = "Code"
        DDLsearchType.DataBind()

        DDLsearchType.SelectedIndex = 0

        DDLsearchVisibility.Items.Clear()
        DDLsearchVisibility.DataSource = GetDDLElements(GetType(FilterVisibilityTypeEnum))
        DDLsearchVisibility.DataTextField = "Description"
        DDLsearchVisibility.DataValueField = "Code"
        DDLsearchVisibility.DataBind()

        DDLsearchVisibility.SelectedIndex = 0

        PNLPublic.Visible = publicGlossaries.Count > 0
        PNLHeaderPublic.Visible = PNLPublic.Visible
    End Sub


    Public Function GetDDLElements(ByVal enumType As Type) As List(Of DDLElement)
        Dim list As New List(Of DDLElement)
        For Each enumItem As String In [Enum].GetNames(enumType)
            Dim item As New DDLElement
            item.Code = enumItem
            item.Description = Resource.getValue(enumType.Name & "." & enumItem)
            list.Add(item)
        Next
        Return list
    End Function


#End Region

#Region "Events"

    Private Sub RPTorderBy_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTorderBy.ItemDataBound, RPTOrderByPublic.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim repeater As Repeater = sender
            Dim itemKey As SortObject = e.Item.DataItem
            Dim LNBorderItemsBy As LinkButton = e.Item.FindControl("LNBorderItemsBy")
            If Not IsNothing(LNBorderItemsBy) Then
                LNBorderItemsBy.CommandArgument = itemKey.Key
                LNBorderItemsBy.CommandName = IIf(repeater.UniqueID.Contains("RPTOrderByPublic"), "Public", "Community")
            End If
            Dim LBsortName As Label = e.Item.FindControl("LBsortName")
            If Not IsNothing(LBsortName) Then
                LBsortName.Text = Resource.getValue(itemKey.Key)
            End If
        End If
    End Sub

    Private Sub RPTorderBy_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTorderBy.ItemCommand, RPTOrderByPublic.ItemCommand
        CurrentPresenter.Sort(CommunityGlossaryList, PublicGlossaryList, CommandSortDictionary.First(Function(item) item.Key = e.CommandArgument), e.CommandName)
        Dim repeater As Repeater = source
        UpdateSelectedSortLabel(CommandSortDictionary.First(Function(item) item.Key = e.CommandArgument), repeater.UniqueID.Contains("RPTOrderByPublic"))
    End Sub

    Private Sub UpdateSelectedSortLabel(ByVal sortObj As SortObject, ByVal isPublic As Boolean)
        If isPublic Then
            PublicSort = sortObj
            Dim LBorderBySelectedPublic As Label = FindControl("LBorderBySelectedPublic")
            If Not IsNothing(LBorderBySelectedPublic) Then
                LBorderBySelectedPublic.Text = Resource.getValue(sortObj.Key)
            End If
        Else
            CommunitySort = sortObj
            Dim LBorderBySelected As Label = FindControl("LBorderBySelected")
            If Not IsNothing(LBorderBySelected) Then
                LBorderBySelected.Text = Resource.getValue(sortObj.Key)
            End If
        End If

        RPTorderBy.DataSource = CommandSortDictionary
        RPTorderBy.DataBind()
        RPTOrderByPublic.DataSource = CommandSortDictionary
        RPTOrderByPublic.DataBind()
    End Sub

    Private Sub RTPlist_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTlist.ItemDataBound, RPTpublic.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dtoGlossary As DTO_Glossary = e.Item.DataItem

            Dim HYPname As HyperLink = e.Item.FindControl("HYPname")
            If Not IsNothing(HYPname) Then
                HYPname.NavigateUrl = PageUtility.BaseUrl & RootObject.GlossaryView(dtoGlossary.Id, IdCommunity, Not dtoGlossary.TermsArePaged)
            End If

            Dim LTname As Literal = e.Item.FindControl("LTname")
            If Not IsNothing(LTname) Then
                LTname.Text = dtoGlossary.Name
            End If

            Dim LTdescription As Literal = e.Item.FindControl("LTdescription")
            If Not IsNothing(LTdescription) Then
                LTdescription.Text = dtoGlossary.Description
            End If

            Dim LTlanguage As Literal = e.Item.FindControl("LTlanguage")
            If Not IsNothing(LTlanguage) Then
                LTlanguage.Text = CurrentPresenter.GetLanguageCode(dtoGlossary.IdLanguage, Resource.getValue("multilanguage.code"))
            End If

            Dim LTtermsCount As Literal = e.Item.FindControl("LTtermsCount")
            If Not IsNothing(LTtermsCount) Then
                LTtermsCount.Text = dtoGlossary.TermsCount
            End If

            Dim LNBglossaryDelete As LinkButton = e.Item.FindControl("LNBglossaryDelete")
            If Not IsNothing(LNBglossaryDelete) Then
                LNBglossaryDelete.Visible = dtoGlossary.Permission.DeleteGlossary
                LNBglossaryDelete.CommandName = GlossaryDeleteCommandString
                LNBglossaryDelete.CommandArgument = dtoGlossary.IdShare
                Resource.setLinkButton(LNBglossaryDelete, True, True, False, True)
            End If

            Dim HYPglossaryStats As HyperLink = e.Item.FindControl("HYPglossaryStats")
            If Not IsNothing(HYPglossaryStats) Then
                HYPglossaryStats.Visible = dtoGlossary.Permission.ViewStat
                Resource.setHyperLink(HYPglossaryStats, True, True)
            End If

            Dim HYPglossarySettings As HyperLink = e.Item.FindControl("HYPglossarySettings")
            If Not IsNothing(HYPglossarySettings) Then
                HYPglossarySettings.Visible = dtoGlossary.Permission.EditGlossary
                HYPglossarySettings.NavigateUrl = PageUtility.BaseUrl & RootObject.GlossaryEdit(dtoGlossary.Id, IdCommunity, True, False)
                Resource.setHyperLink(HYPglossarySettings, True, True)
            End If

            Dim LTindicator As Literal = e.Item.FindControl("LTindicator")
            If Not IsNothing(LTindicator) Then
                LTindicator.Text = GetTileClassLocalized(dtoGlossary.Id)
            End If

            Dim LTindicatorPublic As Literal = e.Item.FindControl("LTindicatorPublic")
            If Not IsNothing(LTindicatorPublic) Then
                Resource.setLiteral(LTindicatorPublic)
            End If

        End If
    End Sub

    Private Sub RTPlist_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTlist.ItemCommand
        Dim idShare As Int64 = Convert.ToInt64(e.CommandArgument)
        If idShare > 0 Then
            Select Case e.CommandName
                Case GlossaryDeleteCommandString
                    CurrentPresenter.VirtualDeleteGlossary(idShare)
                Case Else
            End Select
        End If
    End Sub

    Public Function GetGlossaryLanguage(ByVal idLanguage As Int32) As String
        Return CurrentPresenter.GetLanguageDescription(idLanguage, Resource.getValue("multilanguage.description"))
    End Function

    Public Function GetTileClassLocalized(ByVal id As Long) As String
        Dim dtoGlossary As DTO_Glossary
        If (BindingCommunity) Then
            dtoGlossary = CommunityGlossaryList.FirstOrDefault(Function(f) f.Id = id)
        Else
            dtoGlossary = PublicGlossaryList.FirstOrDefault(Function(f) f.Id = id)
        End If

        If dtoGlossary IsNot Nothing Then
            Dim sb As New StringBuilder()


            If (Not dtoGlossary.IsPublished) Then
                sb.Append(" ").Append(Resource.getValue("unpublished")).Append(" ")
            End If

            If (dtoGlossary.IsDefault) Then
                sb.Append(" ").Append(Resource.getValue("default")).Append(" ")
            ElseIf (dtoGlossary.IsPublic) Then
                sb.Append(" ").Append(Resource.getValue("public")).Append(" ")
                'Else
                '    sb.Append(" ").Append(Resource.getValue("normal")).Append(" ")
            End If

            If dtoGlossary.Shared = ShareStatusEnum.ForceActive Or dtoGlossary.Shared = ShareStatusEnum.Active Then
                sb.Append(" ").Append(Resource.getValue("shared")).Append(" ")
            ElseIf dtoGlossary.Shared = ShareStatusEnum.Pending Then
                sb.Append(" ").Append(Resource.getValue("shared_waiting")).Append(" ")
            Else
                If (dtoGlossary.IdCommunity = IdCommunity) Then
                    sb.Append(Resource.getValue(" internal")).Append(" ")
                Else
                    sb.Append(Resource.getValue(" external")).Append(" ")
                End If
            End If

            Dim result As String = sb.ToString().Trim().Replace("  ", " ")
            Return result
        Else
            Return String.Empty
        End If
    End Function

    Public Function GetTileClass(ByVal id As Long) As String
        Dim dtoGlossary As DTO_Glossary
        If (BindingCommunity) Then
            dtoGlossary = CommunityGlossaryList.FirstOrDefault(Function(f) f.Id = id)
        Else
            dtoGlossary = PublicGlossaryList.FirstOrDefault(Function(f) f.Id = id)
        End If
        If dtoGlossary IsNot Nothing Then
            Dim sb As New StringBuilder()


            If (Not dtoGlossary.IsPublished) Then
                sb.Append(" unpublished  ")
            End If

            If (dtoGlossary.IsDefault) Then
                sb.Append(" default ")
            ElseIf (dtoGlossary.IsPublic) Then
                sb.Append(" public ")
            End If

            If dtoGlossary.Shared = ShareStatusEnum.ForceActive Or dtoGlossary.Shared = ShareStatusEnum.Active Then
                sb.Append(" shared ")
            ElseIf dtoGlossary.Shared = ShareStatusEnum.Pending Then
                sb.Append(" shared waiting ")
            End If

            If (dtoGlossary.IdCommunity = IdCommunity) Then
                sb.Append(" internal ")
            Else
                sb.Append(" external ")
            End If


            Dim result As String = sb.ToString().Trim().Replace("  ", " ")
            Return result
        Else
            Return String.Empty
        End If
    End Function

    Public Function GetPublicItemActive(ByVal sortObject As SortObject) As String
        Dim htmlClass As StringBuilder = New StringBuilder("selectoritem ")

        If sortObject.Key = PublicSort.Key Then
            htmlClass.Append("active ")
        End If

        Dim index As Integer = CommandSortDictionary.IndexOf(sortObject)

        If index = 0 Then
            htmlClass.Append("first ")
        ElseIf (index = CommandSortDictionary.Count - 1) Then
            htmlClass.Append("last ")
        End If

        Return htmlClass.ToString().Replace("  ", " ").Trim()
    End Function

    Public Function GetCommunityItemActive(ByVal sortObject As SortObject) As String
        Dim htmlClass As StringBuilder = New StringBuilder("selectoritem ")

        If sortObject.Key = CommunitySort.Key Then
            htmlClass.Append("active ")
        End If

        Dim index As Integer = CommandSortDictionary.IndexOf(sortObject)

        If index = 0 Then
            htmlClass.Append("first ")
        ElseIf (index = CommandSortDictionary.Count - 1) Then
            htmlClass.Append("last ")
        End If

        Return htmlClass.ToString().Replace("  ", " ").Trim()
    End Function

    Private Sub LNBsearch_Click(sender As Object, e As EventArgs) Handles LNBsearch.Click
        IsMultiFilter = False
        If Not String.IsNullOrWhiteSpace(TXBsearch.Text) Then
            Dim filter As New GlossaryFilter
            filter.LemmaString = TXBsearch.Text
            CurrentPresenter.ShowFilterResult(filter)
        Else
            CurrentPresenter.ShowFilterResult(Nothing)
        End If
    End Sub

    Private Sub LNBreset_Click(sender As Object, e As EventArgs) Handles LNBreset.Click
        TXBsearchLemma.Text = String.Empty
        TXBsearch.Text = String.Empty
        TXBsearchLemmaContent.Text = String.Empty
        DDLsearchType.SelectedIndex = 0
        DDLsearchVisibility.SelectedIndex = 0
        CurrentPresenter.ShowFilterResult(Nothing)
    End Sub

    Private Sub LNBsearchApply_Click(sender As Object, e As EventArgs) Handles LNBsearchApply.Click
        IsMultiFilter = True
        CurrentPresenter.ShowFilterResult(CreateFilter())
    End Sub

    Public Function CreateFilter() As GlossaryFilter
        Dim filter As New GlossaryFilter
        filter.LemmaString = TXBsearchLemma.Text
        filter.SearchString = TXBsearch.Text
        filter.LemmaContentString = TXBsearchLemmaContent.Text
        filter.LemmaSearchType = DDLsearchType.SelectedIndex
        filter.LemmaVisibilityType = DDLsearchVisibility.SelectedIndex
        Return filter
    End Function

    Public Sub GoToGlossarySearch() Implements IViewUC_GlossariesList.GoToGlossarySearch
        Response.Redirect(String.Format("{0}{1}", ApplicationUrlBase, RootObject.GlossarySearch(IdCommunity, CreateFilter())))
    End Sub

#End Region

#Region "Messages"

    'Public Sub SetErrorMessage(ByVal message As String, ByVal type As MessageType) Implements IViewUC_GlossariesList.SetErrorMessage
    '    If (type = MessageType.none OrElse String.IsNullOrEmpty(message)) Then
    '        Me.DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text)
    '        CTRLmessagesInfo.Visible = False
    '    Else
    '        Me.DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text.Replace(" hide", ""))
    '        CTRLmessagesInfo.Visible = True
    '        CTRLmessagesInfo.InitializeControl(message, type)
    '    End If
    'End Sub

    Public Sub SetErrorMessage(ByVal type As MessageType, ByVal messageKey As String, ByVal ParamArray parameters As String()) Implements IViewUC_GlossariesList.SetErrorMessage
        If (type = MessageType.none OrElse String.IsNullOrEmpty(messageKey)) Then
            Me.DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text)
            CTRLmessagesInfo.Visible = False
        Else
            Me.DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text.Replace(" hide", ""))
            CTRLmessagesInfo.Visible = True
            CTRLmessagesInfo.InitializeControl(String.Format(Resource.getValue(messageKey), parameters), type)
        End If
    End Sub

#End Region
End Class
