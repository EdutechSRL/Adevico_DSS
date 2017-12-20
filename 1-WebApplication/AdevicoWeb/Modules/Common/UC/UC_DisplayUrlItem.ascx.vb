Imports lm.Comol.Core.ModuleLinks
Imports lm.Comol.Core.BaseModules.Repository.Presentation
Imports lm.Comol.Core.DomainModel

Public Class UC_DisplayUrlItem
    Inherits BaseControl
    Implements IViewDisplayUrlItem


#Region "Context"
    Private _Presenter As DisplayUrlItemPresenter
    Private ReadOnly Property CurrentPresenter() As DisplayUrlItemPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DisplayUrlItemPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Base"
    Public Property ContainerCSS As String Implements IViewDisplayUrlItem.ContainerCSS
        Get
            Return ViewStateOrDefault("ContainerCSS", "")
        End Get
        Set(value As String)
            ViewState("ContainerCSS") = value
        End Set
    End Property
    Public Property Display As DisplayActionMode Implements IViewDisplayUrlItem.Display
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
                Me.LBurl.Visible = ((value And DisplayActionMode.text) > 0)
                If (value And DisplayActionMode.text) > 0 OrElse (value And DisplayActionMode.textDefault) Then
                    Me.HYPurl.Visible = False
                ElseIf (value And DisplayActionMode.defaultAction) > 0 OrElse ((value And DisplayActionMode.adminMode) > 0) Then
                    Me.HYPurl.Visible = True
                    LBurl.Visible = False
                End If
            End If
        End Set
    End Property
    Public Property IconSize As lm.Comol.Core.DomainModel.Helpers.IconSize Implements IViewDisplayUrlItem.IconSize
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
    Public ReadOnly Property ItemType As lm.Comol.Core.DomainModel.Repository.RepositoryItemType Implements IViewDisplayUrlItem.ItemType
        Get
            Return lm.Comol.Core.DomainModel.Repository.RepositoryItemType.Url
        End Get
    End Property
    Private ReadOnly Property GetRemovedUser As String Implements IViewDisplayUrlItem.GetRemovedUser
        Get
            Return Resource.getValue("GetRemovedUser")
        End Get
    End Property

    Private ReadOnly Property GetUnknownUser As String Implements IViewDisplayUrlItem.GetUnknownUser
        Get
            Return Resource.getValue("GetUnknownUser")
        End Get
    End Property
#End Region

    Public Property ItemIdentifier As String Implements IViewDisplayUrlItem.ItemIdentifier
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
    Public Property EnableAnchor As Boolean Implements IViewDisplayUrlItem.EnableAnchor
        Get
            Return ViewStateOrDefault("EnableAnchor", False)
        End Get
        Set(value As Boolean)
            ViewState("EnableAnchor") = value
        End Set
    End Property
    Public Property DisplayCreateInfo As Boolean Implements IViewDisplayUrlItem.DisplayCreateInfo
        Get
            Return ViewStateOrDefault("DisplayCreateInfo", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayCreateInfo") = value
        End Set
    End Property
#End Region

#Region "Inherits"
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

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ModuleToRepository", "Modules", "Repository")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Implements"

#Region "Initializers"
    Public Sub InitializeControl(mode As lm.Comol.Core.ModuleLinks.DisplayActionMode, url As String, Optional name As String = "", Optional createdOn As Date? = Nothing, Optional createdBy As lm.Comol.Core.DomainModel.Person = Nothing, Optional placeHolders As List(Of lm.Comol.Core.ModuleLinks.dtoPlaceHolder) = Nothing) Implements IViewDisplayUrlItem.InitializeControl
        Me.CurrentPresenter.InitView(mode, url, name, createdOn, createdBy, placeHolders)
    End Sub
    Public Sub InitializeControlLite(mode As lm.Comol.Core.ModuleLinks.DisplayActionMode, url As String, Optional name As String = "", Optional createdOn As Date? = Nothing, Optional createdBy As litePerson = Nothing, Optional placeHolders As List(Of lm.Comol.Core.ModuleLinks.dtoPlaceHolder) = Nothing) Implements IViewDisplayUrlItem.InitializeControlLite
        Me.CurrentPresenter.InitViewLite(mode, url, name, createdOn, createdBy, placeHolders)
    End Sub
#End Region

    Private Sub DisplayRemovedObject() Implements IViewDisplayUrlItem.DisplayRemovedObject
        Me.LBempty.Text = Resource.getValue("action.RemovedObject")
        Me.MLVcontrol.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayEmptyAction() Implements IViewDisplayUrlItem.DisplayEmptyAction
        Me.LBempty.Text = "&nbsp;"
        Me.MLVcontrol.SetActiveView(VIWempty)
    End Sub

    Private Sub DisplayItem(name As String, Optional createdOn As Date? = Nothing, Optional username As String = "", Optional usersurname As String = "") Implements IViewDisplayUrlItem.DisplayItem
        LBurl.Text = name
        BaseDisplayItem(createdOn, username, usersurname)
    End Sub

    Private Sub DisplayItem(name As String, url As String, Optional createdOn As Date? = Nothing, Optional username As String = "", Optional usersurname As String = "") Implements IViewDisplayUrlItem.DisplayItem
        HYPurl.NavigateUrl = url
        HYPurl.Text = name
        '    Me.HYPfileName.CssClass = IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, "fileRepositoryCookie", "")
        '    Me.HYPfileName.CssClass = IIf(InsideOtherModule AndAlso Display <> DisplayActionMode.adminMode, "fileRepositoryCookie", "")
        Me.HYPurl.ToolTip = Resource.getValue("urlTitle") & url
        BaseDisplayItem(createdOn, username, usersurname)
    End Sub

    Private Sub DisplayPlaceHolders(items As List(Of lm.Comol.Core.ModuleLinks.dtoPlaceHolder)) Implements IViewDisplayUrlItem.DisplayPlaceHolders
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
    Private Function GetBaseUrl() As String Implements IViewDisplayUrlItem.GetBaseUrl
        Return PageUtility.ApplicationUrlBase
    End Function
#End Region

#Region "Display item"

    Private Sub BaseDisplayItem(Optional createdOn As Date? = Nothing, Optional username As String = "", Optional usersurname As String = "")
        Me.MLVcontrol.SetActiveView(VIWdata)
        LBuploadedOn.Visible = createdOn.HasValue
        If createdOn.HasValue Then
            LBuploadedOn.Text = String.Format(LTtemplateUploadOn.Text, Resource.getValue("CreatedOn"), createdOn.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern) & " " & createdOn.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortTimePattern))
        End If
        If Not String.IsNullOrEmpty(username) OrElse Not String.IsNullOrEmpty(usersurname) Then
            LBuploadedBy.Text = String.Format(LTtemplateAuthor.Text, Resource.getValue("CreatedBy"), usersurname & IIf(String.IsNullOrEmpty(usersurname), "", " ") & username)
            LBuploadedBy.Visible = True
        Else
            LBuploadedBy.Visible = False
        End If
        Dim ico As String = "<span class=""fileIco {0}"">&nbsp;</span>"

        Me.LBurl.Text = String.Format(ico, GetIconClass(Repository.RepositoryItemType.Url)) & LBurl.Text
        Me.HYPurl.Text = String.Format(ico, GetIconClass(Repository.RepositoryItemType.Url)) & HYPurl.Text

    End Sub

    Private Function GetIconClass(type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType, Optional extension As String = "") As String
        Select Case type
            Case Repository.RepositoryItemType.Folder
                Return "folder"
            Case Repository.RepositoryItemType.Multimedia
                Return "multimedia"
            Case Repository.RepositoryItemType.ScormPackage
                Return "scorm"
            Case Repository.RepositoryItemType.VideoStreaming
                Return "streaming"
            Case Repository.RepositoryItemType.Url
                Return "extlink"
            Case Else
                Return "ext" + extension.Replace(".", "")
        End Select
    End Function
#End Region


  
End Class