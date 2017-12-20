Imports lm.Comol.Core.TemplateMessages
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.Comol.Core.BaseModules.TemplateMessages.Presentation

Public Class UC_TemplateSelector
    Inherits BaseControl
    Implements IViewTemplateSelector

#Region "Context"
    Private _Presenter As TemplateSelectorPresenter
    Private ReadOnly Property CurrentPresenter() As TemplateSelectorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TemplateSelectorPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implemnts"
    Public Property AllowSelect As Boolean Implements IViewTemplateSelector.AllowSelect
        Get
            Return ViewStateOrDefault("AllowSelect", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowSelect") = value
            Me.DDLtemplates.Enabled = value
        End Set
    End Property
    Private Property SelectorContext As dtoSelectorContext Implements IViewTemplateSelector.SelectorContext
        Get
            Return ViewStateOrDefault("SelectorContext", New dtoSelectorContext)
        End Get
        Set(value As dtoSelectorContext)
            ViewState("SelectorContext") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewTemplateSelector.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property isInAjaxPanel As Boolean Implements IViewTemplateSelector.isInAjaxPanel
        Get
            Return ViewStateOrDefault("isInAjaxPanel", False)
        End Get
        Set(value As Boolean)
            ViewState("isInAjaxPanel") = value
        End Set
    End Property
    Public Property Channel As lm.Comol.Core.Notification.Domain.NotificationChannel Implements IViewTemplateSelector.Channel
        Get
            Return ViewStateOrDefault("Channel", lm.Comol.Core.Notification.Domain.NotificationChannel.Mail)
        End Get
        Set(value As lm.Comol.Core.Notification.Domain.NotificationChannel)
            ViewState("Channel") = value
        End Set
    End Property

    Public WriteOnly Property AllowPreview As Boolean Implements IViewTemplateSelector.AllowPreview
        Set(value As Boolean)
            Me.HYPpreview.Visible = value
        End Set
    End Property

    'Private Property Items As List(Of dtoVersionItem) Implements IViewTemplateSelector.Items
    '    Get
    '        Return ViewStateOrDefault("Items", New List(Of dtoVersionItem))
    '    End Get
    '    Set(value As List(Of dtoVersionItem))
    '        ViewState("Items") = value
    '    End Set
    'End Property
    Public ReadOnly Property SelectedItem As dtoVersionItem Implements IViewTemplateSelector.SelectedItem
        Get
            Dim item As dtoVersionItem = Nothing

            If Me.DDLtemplates.Items.Count > 0 Then
                Dim value As String = Me.DDLtemplates.SelectedValue
                item = New dtoVersionItem(CInt(value.Split("_")(2))) With {.Id = System.Convert.ToInt64(value.Split("_")(1)), .IdTemplate = System.Convert.ToInt64(value.Split("_")(0))}
            End If
            If item Is Nothing Then
                item = New dtoVersionItem(TemplateLevel.Removed) With {.Id = 0, .IdTemplate = 0}
            End If
            Return item
        End Get
    End Property
    Private Property Items As List(Of dtoTemplateItem) Implements IViewTemplateSelector.Items
        Get
            Return ViewStateOrDefault("Items", New List(Of dtoTemplateItem))
        End Get
        Set(value As List(Of dtoTemplateItem))
            ViewState("Items") = value
        End Set
    End Property
    Public Property RaiseSelectionEvent As Boolean Implements IViewTemplateSelector.RaiseSelectionEvent
        Get
            Return ViewStateOrDefault("RaiseSelectionEvent", False)
        End Get
        Set(value As Boolean)
            ViewState("RaiseSelectionEvent") = value
            Me.DDLtemplates.AutoPostBack = value
        End Set
    End Property
#End Region

#Region "Internal"
    Public Event TemplateSelected(idTemplate As Long, idVersion As Long)
    Private Property ItemIdentifier As String
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
        MyBase.SetCulture("pg_Templates", "Modules", "Templates")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(HYPpreview, True, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(permissions As lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages, channel As lm.Comol.Core.Notification.Domain.NotificationChannel, sContext As lm.Comol.Core.TemplateMessages.Domain.dtoSelectorContext, idTemplate As Long, idVersion As Long, Optional tItems As List(Of dtoTemplateItem) = Nothing) Implements lm.Comol.Core.BaseModules.TemplateMessages.Presentation.IViewTemplateSelector.InitializeControl
        ItemIdentifier = "template_" & sContext.IdAction & "_" & sContext.ModuleCode
        Me.CurrentPresenter.InitView(permissions, channel, sContext, idTemplate, idVersion, False, tItems)
    End Sub

    Public Sub InitializeControl(permissions As lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages, channel As lm.Comol.Core.Notification.Domain.NotificationChannel, idAction As Long, idModule As Integer, moduleCode As String, idCommunty As Integer, Optional idOrganization As Integer = 0, Optional forPortal As Boolean = False, Optional idTemplate As Long = 0, Optional idVersion As Long = 0, Optional source As lm.Comol.Core.DomainModel.ModuleObject = Nothing, Optional tItems As List(Of dtoTemplateItem) = Nothing) Implements lm.Comol.Core.BaseModules.TemplateMessages.Presentation.IViewTemplateSelector.InitializeControl
        ItemIdentifier = "template_" & idAction & "_" & moduleCode
        Me.CurrentPresenter.InitView(permissions, channel, idAction, idModule, moduleCode, idCommunty, idOrganization, forPortal, idTemplate, idVersion, source, tItems)
    End Sub

    Private Sub LoadTemplates(templates As List(Of dtoTemplateItem)) Implements IViewTemplateSelector.LoadTemplates
        Me.DDLtemplates.Items.Clear()
        Me.Items = templates

        If Not IsNothing(templates) AndAlso templates.Count() > 0 Then
            If templates.Select(Function(t) t.Level).Distinct().Count = 1 OrElse templates.Select(Function(t) t.Level).Where(Function(l) l <> TemplateLevel.Removed).Distinct().Count = 1 Then
                LoadStandardTemplates(templates)
            Else
                If (templates.Where(Function(t) t.Level = TemplateLevel.Portal).Any()) Then
                    Me.DDLtemplates.AddItemGroup(Resource.getValue("Group." & TemplateLevel.Portal.ToString))
                    LoadStandardTemplates(templates.Where(Function(t) t.Level = TemplateLevel.Portal).ToList())
                End If
                If (templates.Where(Function(t) t.Level = TemplateLevel.Organization).Any()) Then
                    Me.DDLtemplates.AddItemGroup(Resource.getValue("Group." & TemplateLevel.Organization.ToString))
                    LoadStandardTemplates(templates.Where(Function(t) t.Level = TemplateLevel.Organization).ToList())
                End If
                If (templates.Where(Function(t) t.Level = TemplateLevel.Community).Any()) Then
                    Me.DDLtemplates.AddItemGroup(Resource.getValue("Group." & TemplateLevel.Community.ToString))
                    LoadStandardTemplates(templates.Where(Function(t) t.Level = TemplateLevel.Community).ToList())
                End If
                If (templates.Where(Function(t) t.Level = TemplateLevel.Object).Any()) Then
                    Me.DDLtemplates.AddItemGroup(Resource.getValue("Group." & TemplateLevel.Object.ToString))
                    LoadStandardTemplates(templates.Where(Function(t) t.Level = TemplateLevel.Object).ToList())
                End If
                If (templates.Where(Function(t) t.Level = TemplateLevel.Removed).Any()) Then
                    Me.DDLtemplates.AddItemGroup(Resource.getValue("Group." & TemplateLevel.Removed.ToString))
                    LoadStandardTemplates(templates.Where(Function(t) t.Level = TemplateLevel.Removed).ToList())
                End If
            End If
        End If
        Me.DDLtemplates.Items.Insert(0, New ListItem(Resource.getValue("Element.Select"), "0_0_" & CInt(TemplateLevel.None)))
        Dim item As dtoVersionItem = SelectedItem
        If Not IsNothing(item) AndAlso item.IdTemplate > 0 Then
            Me.SetPreviewLink(SelectedItem)
            HYPpreview.Visible = True
        Else
            HYPpreview.Visible = False
        End If
    End Sub
    Private Sub LoadStandardTemplates(templates As List(Of dtoTemplateItem))
        For Each dto As dtoTemplateItem In templates
            Me.DDLtemplates.AddItemGroup(dto.Name)
            If (dto.Versions.Count > 0) Then
                For Each dtoV As dtoVersionItem In dto.Versions
                    Dim oListitem As New ListItem("", dto.Id.ToString() & "_" & dtoV.Id & "_" & CInt(dto.Level))
                    Dim ItemText As String = ""
                    If (dtoV.Id <= 0) Then
                        oListitem.Attributes.Add("class", "ItemLast")
                        ItemText = Resource.getValue("Element.Last")
                    ElseIf (dtoV.Status = TemplateStatus.Draft) Then
                        oListitem.Attributes.Add("class", "ItemDraft")
                        ItemText = Resource.getValue("Element.Draft")
                    ElseIf (dtoV.Status = TemplateStatus.Replaced) Then
                        oListitem.Attributes.Add("class", "ItemDisapproved")
                        ItemText = Resource.getValue("Element.Disapproved")
                    Else
                        ItemText = Resource.getValue("Element.Generic")
                    End If

                    If (IsNothing(ItemText) OrElse ItemText = "") Then
                        ItemText = dtoV.Number.ToString
                    End If

                    ' Eventualmente aggiungere altri TAG o usare altre funzioni per il replace

                    ItemText = ItemText.Replace("#item.Version#", dtoV.Number) _
                        .Replace("#item.TemplateName#", dto.Name)

                    If Not IsNothing(dtoV.Lastmodify) Then
                        ItemText = ItemText.Replace("#item.VersionDate#", dtoV.Lastmodify)
                    Else
                        ItemText = ItemText.Replace("#item.VersionDate#", "")
                    End If
                    oListitem.Text = ItemText

                    If (dtoV.IsSelected) Then
                        oListitem.Selected = True
                        ' Me.SetPreviewLink(dtoV)
                    End If

                    Me.DDLtemplates.Items.Add(oListitem)
                Next
            End If
        Next
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewTemplateSelector.DisplaySessionTimeout
        Me.MLVcontent.SetActiveView(VIWsessionTimeout)
    End Sub

    Public Sub LoadEmptyTemplate() Implements IViewTemplateSelector.LoadEmptyTemplate
        Me.DDLtemplates.Items.Clear()
        Me.DDLtemplates.Items.Insert(0, New ListItem(Resource.getValue("Element.Select"), "0_0_" & CInt(TemplateLevel.None)))
    End Sub

#End Region

#Region "Internal"

    ' ''' <summary>
    ' ''' Se il link è visibile, ne imposta l'URL
    ' ''' </summary>
    ' ''' <param name="TemplateId">Id del template</param>
    ' ''' <param name="VersionId">Id della versione</param>
    ' ''' <remarks>
    ' ''' Eventualmente correggere con variabili globali (link)
    ' ''' </remarks>
    Private Sub SetPreviewLink(ByVal item As dtoVersionItem)
        '    Me.HYPpreview.Text = Resource.getValue("HYPpreview.Text")
        Me.HYPpreview.NavigateUrl = BaseUrl & lm.Comol.Core.TemplateMessages.RootObject.PreviewTemplate(item.IdTemplate, item.Id, SelectorContext.IdModule, SelectorContext.ModuleCode, SelectorContext.IsForPortal, SelectorContext.IdCommunity, SelectorContext.IdOrganization, SelectorContext.ObjectOwner)
    End Sub

    Private Sub DDLtemplates_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLtemplates.SelectedIndexChanged
        Dim item As dtoVersionItem = SelectedItem
        ''Modifico l'url del link
        ''Internamente, se l'url non è visibile, non ne viene fatto il bind.
        If Not IsNothing(item) AndAlso item.IdTemplate > 0 Then
            Me.SetPreviewLink(SelectedItem)
            HYPpreview.Visible = True
        Else
            HYPpreview.Visible = False
        End If
        If RaiseSelectionEvent Then
            RaiseEvent TemplateSelected(item.IdTemplate, item.Id)
        End If
        'If isInAjaxPanel Then
        '    RaiseEvent SelectedIndexChange()
        'End If
    End Sub

#End Region

   
End Class