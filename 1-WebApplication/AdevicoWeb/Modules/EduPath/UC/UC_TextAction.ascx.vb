Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath.Presentation
Imports lm.Comol.Core.ModuleLinks

Public Class UC_TextAction
    Inherits BaseControl
    Implements IViewModuleTextAction

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As ModuleTextActionPresenter

    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
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
    Private ReadOnly Property CurrentPresenter() As ModuleTextActionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleTextActionPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Base"
    Public Property ContainerCSS As String Implements IViewModuleTextAction.ContainerCSS
        Get
            Return ViewStateOrDefault("ContainerCSS", "")
        End Get
        Set(value As String)
            ViewState("ContainerCSS") = value
        End Set
    End Property
    Public Property Display As DisplayActionMode Implements IViewModuleTextAction.Display
        Get
            Return ViewStateOrDefault("Display", DisplayActionMode.defaultAction)
        End Get
        Set(value As DisplayActionMode)
            ViewState("Display") = value
            If value = DisplayActionMode.none Then
                Me.MLVcontrol.SetActiveView(VIWempty)
                Me.LBempty.Text = " "
            Else
                Me.MLVcontrol.SetActiveView(VIWdata)
                Me.RPTactions.Visible = ((value And DisplayActionMode.actions) > 0)
            End If
        End Set
    End Property
    Public Property IconSize As lm.Comol.Core.DomainModel.Helpers.IconSize Implements IGenericModuleDisplayAction.IconSize
        Get
            Return ViewStateOrDefault("IconSize", lm.Comol.Core.DomainModel.Helpers.IconSize.Medium)
        End Get
        Set(value As lm.Comol.Core.DomainModel.Helpers.IconSize)
            ViewState("IconSize") = value
        End Set
    End Property
    Private ReadOnly Property IconSizeToString As String
        Get
            Dim result As String = ViewStateOrDefault("IconSizeToString", "")
            If String.IsNullOrEmpty(result) Then
                Select Case IconSize
                    Case Helpers.IconSize.Large
                        result = "_l"
                    Case Helpers.IconSize.Medium
                        result = "_m"
                    Case Helpers.IconSize.Small
                        result = "_s"
                    Case Helpers.IconSize.Smaller
                        result = "_xs"
                End Select
                ViewState("IconSizeToString") = result
            End If
            Return result
        End Get
    End Property
    Public Property ShortDescription As Boolean Implements IGenericModuleDisplayAction.ShortDescription
        Get
            Return ViewStateOrDefault("ShortDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("ShortDescription") = value
        End Set
    End Property
    Public Property EnableAnchor As Boolean Implements IGenericModuleDisplayAction.EnableAnchor
        Get
            Return ViewStateOrDefault("EnableAnchor", False)
        End Get
        Set(value As Boolean)
            ViewState("EnableAnchor") = value
        End Set
    End Property
#End Region
    Public Property RefreshContainer As Boolean Implements IViewModuleTextAction.RefreshContainer
        Get
            Return ViewStateOrDefault("RefreshContainer", False)
        End Get
        Set(value As Boolean)
            ViewState("RefreshContainer") = value
        End Set
    End Property
    Public Property ForUserId As Integer Implements IViewModuleTextAction.ForUserId
        Get
            Return ViewStateOrDefault("ForUserId", CurrentContext.UserContext.CurrentUserID)
        End Get
        Set(value As Integer)
            ViewState("ForUserId") = value
        End Set
    End Property
    Public Property InsideOtherModule As Boolean Implements IViewModuleTextAction.InsideOtherModule
        Get
            Return ViewStateOrDefault("InsideOtherModule", False)
        End Get
        Set(value As Boolean)
            ViewState("InsideOtherModule") = value
        End Set
    End Property
    Private Property ItemIdentifier As String Implements IViewModuleTextAction.ItemIdentifier
        Get
            Return ViewStateOrDefault("ItemIdentifier", "")
        End Get
        Set(value As String)
            ViewState("ItemIdentifier") = value
            If String.IsNullOrEmpty(value) Then
                LTidentifier.Visible = False
            Else
                LTidentifier.Text = "<a name=""" & value & """> </a>"
                LTidentifier.Visible = True
            End If
        End Set
    End Property

    Private ReadOnly Property PreLoadedContentView As ContentView Implements IViewModuleTextAction.PreLoadedContentView
        Get
            Return PageUtility.PreLoadedContentView
        End Get
    End Property
    Private ReadOnly Property DestinationUrl As String Implements IViewModuleTextAction.DestinationUrl
        Get
            Dim url As String = Request.Url.LocalPath
            If Me.BaseUrl <> "/" Then
                url = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
            End If
            url = Server.UrlEncode(url & Request.Url.Query & IIf(EnableAnchor, "#" & ItemIdentifier, ""))
            Return url
        End Get
    End Property

    'Public Event RefreshContainerEvent(sender As Object, e As lm.Comol.Modules.EduPath.Presentation.RefreshContainerArgs) Implements lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction.RefreshContainerEvent
    'Public Event RefreshContainerEvent(sender As Object, e As RefreshContainerArgs) Implements IViewModuleTextAction.RefreshContainerEvent
    ' Public Event RefreshRequired(ByVal executed As Boolean) RefreshContainerEvent
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Private _SmartTagsAvailable As Comol.Entity.SmartTags
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.PageUtility.ApplicationUrlBase(True))
            End If
            Return _SmartTagsAvailable
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_TextAction", "EduPath")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LNBexecute, True, True)
        End With
    End Sub
#End Region

#Region "Implements"

#Region "InitializeControl"
    Public Sub InitializeControl(initializer As dtoModuleDisplayActionInitializer) Implements IGenericModuleDisplayAction.InitializeControl
        CurrentPresenter.InitView(initializer)
    End Sub
    Public Function InitializeRemoteControl(initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl) Implements IGenericModuleDisplayAction.InitializeRemoteControl
        Return CurrentPresenter.InitRemoteControlView(initializer, actionsToDisplay)
    End Function
    Public Sub InitializeControl(idUser As Integer, initializer As dtoModuleDisplayActionInitializer) Implements IGenericModuleDisplayAction.InitializeControl
        ForUserId = idUser
        CurrentPresenter.InitView(initializer)
    End Sub
    Public Sub InitializeControl(idUser As Integer, initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) Implements IGenericModuleDisplayAction.InitializeControl
        ForUserId = idUser
        CurrentPresenter.InitView(initializer, actionsToDisplay)
    End Sub
    Public Sub InitializeControl(initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) Implements IGenericModuleDisplayAction.InitializeControl
        CurrentPresenter.InitView(initializer, actionsToDisplay)
    End Sub
    Public Function InitializeRemoteControl(idUser As Integer, initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl) Implements IGenericModuleDisplayAction.InitializeRemoteControl
        ForUserId = idUser
        Return CurrentPresenter.InitRemoteControlView(initializer, actionsToDisplay)
    End Function
#End Region

    Public Sub DisplayRemovedObject() Implements IGenericModuleDisplayAction.DisplayRemovedObject
        Me.LBempty.Text = Resource.getValue("action.RemovedObject")
        Me.MLVcontrol.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayEmptyAction() Implements IViewModuleTextAction.DisplayEmptyAction
        Me.LBempty.Text = "&nbsp;"
        Me.MLVcontrol.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayUnknownAction() Implements IViewModuleTextAction.DisplayUnknownAction
        Me.LBempty.Text = Resource.getValue("action.unhandled")
        Me.MLVcontrol.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayItem(actionText As String) Implements IViewModuleTextAction.DisplayItem
        DisplayItem(0, actionText)
    End Sub
    Private Sub DisplayItem(idItem As Long, actionText As String) Implements IViewModuleTextAction.DisplayItem
        Me.MLVcontrol.SetActiveView(VIWdata)
        Me.LBtextAction.Text = Me.SmartTagsAvailable.TagAll(actionText)
        Me.LNBexecute.Visible = (idItem > 0)
        Me.LNBexecute.CommandArgument = idItem
        Me.Resource.setLinkButton(LNBexecute, True, True)
    End Sub

#Region "Display Actions"
    Private Sub DisplayActions(actions As List(Of dtoModuleActionControl)) Implements IViewModuleTextAction.DisplayActions
        If actions.Count = 0 Then
            Me.RPTactions.Visible = False
        End If
        Me.RPTactions.DataSource = actions
        Me.RPTactions.DataBind()
        Me.MLVcontrol.SetActiveView(VIWdata)
    End Sub
    Private Sub DisplayEmptyActions() Implements IViewModuleTextAction.DisplayEmptyActions
        Me.RPTactions.Visible = False
    End Sub
    Private Sub RPTactions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTactions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim action As dtoModuleActionControl = DirectCast(e.Item.DataItem, dtoModuleActionControl)
            Dim link As HyperLink = e.Item.FindControl("HYPaction")
            link.Target = IIf(action.isPopupUrl, "_blank", "")
            link.CssClass = "action "
            link.ToolTip = Me.Resource.getValue("ToolTip.StandardActionType." & action.ControlType.ToString)
            Select Case action.ControlType
                Case StandardActionType.Play
                    'link.CssClass &= "questionario" & IconSizeToString
                    'link.ToolTip = Me.Resource.getValue("ToolTip.StandardActionType." & action.ControlType.ToString & ".QuizStatus." & ItemStatus.ToString)

                    'If String.IsNullOrEmpty(link.ToolTip) Then
                    '    link.ToolTip = String.Format(Me.Resource.getValue("ToolTip.StandardActionType." & action.ControlType.ToString), QuizName)
                    'ElseIf ItemStatus = QuizStatus.ViewCompiled OrElse ItemStatus = QuizStatus.Compiled Then
                    '    link.ToolTip = String.Format(link.ToolTip, QuizName, Score)
                    'Else
                    '    link.ToolTip = String.Format(link.ToolTip, QuizName)
                    'End If
                Case StandardActionType.ViewAdvancedStatistics
                    link.CssClass &= "stats" & IconSizeToString
                Case StandardActionType.ViewPersonalStatistics
                    link.CssClass &= "stats" & IconSizeToString
                Case StandardActionType.ViewUserStatistics
                    link.CssClass &= "stats" & IconSizeToString
                Case StandardActionType.Create
                    link.CssClass &= "add" & IconSizeToString
                Case StandardActionType.Edit
                    link.CssClass &= "edit" & IconSizeToString
            End Select
            link.Text = action.ControlType.ToString()
            link.NavigateUrl = action.LinkUrl
        End If
    End Sub
#End Region

    Public Sub DisplayPlaceHolders(items As List(Of lm.Comol.Core.ModuleLinks.dtoPlaceHolder)) Implements IViewModuleTextAction.DisplayPlaceHolders
        Dim places As New Dictionary(Of PlaceHolderType, Integer)
        places.Add(PlaceHolderType.zero, 0)
        places.Add(PlaceHolderType.one, 1)
        places.Add(PlaceHolderType.two, 2)
        places.Add(PlaceHolderType.three, 3)
        places.Add(PlaceHolderType.four, 4)

        For Each item As lm.Comol.Core.ModuleLinks.dtoPlaceHolder In items.Where(Function(i) i.Type <> PlaceHolderType.fullContainer AndAlso i.Type <> PlaceHolderType.none).ToList()
            Dim oLabel As Label = FindControl("LBplace" & places(item.Type))
            If Not IsNothing(oLabel) Then
                oLabel.Text = item.Text
                oLabel.Visible = True
                If Not String.IsNullOrEmpty(item.CssClass) Then
                    oLabel.CssClass = "plh plh" & places(item.Type).ToString() & " " & item.CssClass
                End If
            End If
        Next
    End Sub

    Public Function GetBaseUrl() As String Implements IViewModuleTextAction.GetBaseUrl
        Return PageUtility.BaseUrl()
    End Function

    Public Function GetBaseUrl(useSSL As Boolean) As String Implements IViewModuleTextAction.GetBaseUrl
        Return PageUtility.BaseUrl(useSSL)
    End Function

#End Region

    Private Sub LNBexecute_Click(sender As Object, e As System.EventArgs) Handles LNBexecute.Click
        Dim executed As Boolean = False
        If Not String.IsNullOrEmpty(sender.CommandArgument) Then
            executed = Me.CurrentPresenter.ExecuteAction(Convert.ToInt64(sender.CommandArgument))
        End If
        If Me.RefreshContainer Then
            RaiseEvent RefreshContainerEvent(sender, New RefreshContainerArgs() With {.Executed = executed})
        End If
    End Sub


    Public Event RefreshContainerEvent(sender As Object, e As lm.Comol.Modules.EduPath.Presentation.RefreshContainerArgs)

    Public Function getDescriptionByLink(link As lm.Comol.Core.DomainModel.ModuleLink, ByVal isGeneric As Boolean) As String Implements lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction.getDescriptionByLink
        Return ""
    End Function

    Public Function GetInLineDescriptionByLink(link As lm.Comol.Core.DomainModel.ModuleLink, ByVal isGeneric As Boolean) As String Implements lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction.GetInLineDescriptionByLink
        Return ""
    End Function
End Class