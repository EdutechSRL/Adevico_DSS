Imports lm.Comol.Core.TemplateMessages
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.Comol.Core.BaseModules.TemplateMessages.Presentation

Public Class UC_TemplateMessageAssociation
    Inherits BaseControl
    Implements IViewTemplateAssociation

#Region "Context"
    Private _Presenter As TemplateAssociationPresenter
    Private ReadOnly Property CurrentPresenter() As TemplateAssociationPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TemplateAssociationPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implemnts"
    Public Property AllowSelect As Boolean Implements IViewTemplateAssociation.AllowSelect
        Get
            Return ViewStateOrDefault("AllowSelect", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowSelect") = value
            Me.DDLtemplates.Enabled = value
        End Set
    End Property
    Private Property Source As lm.Comol.Core.DomainModel.ModuleObject Implements IViewTemplateAssociation.Source
        Get
            Return ViewStateOrDefault("Source", New lm.Comol.Core.DomainModel.ModuleObject())
        End Get
        Set(value As lm.Comol.Core.DomainModel.ModuleObject)
            ViewState("Source") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewTemplateAssociation.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property isInAjaxPanel As Boolean Implements IViewTemplateAssociation.isInAjaxPanel
        Get
            Return ViewStateOrDefault("isInAjaxPanel", False)
        End Get
        Set(value As Boolean)
            ViewState("isInAjaxPanel") = value
        End Set
    End Property
    Public WriteOnly Property AllowPreview As Boolean Implements IViewTemplateAssociation.AllowPreview
        Set(value As Boolean)
            Me.HYPpreview.Visible = value
        End Set
    End Property

    'Private Property Items As List(Of dtoVersionItem) Implements IViewTemplateAssociation.Items
    '    Get
    '        Return ViewStateOrDefault("Items", New List(Of dtoVersionItem))
    '    End Get
    '    Set(value As List(Of dtoVersionItem))
    '        ViewState("Items") = value
    '    End Set
    'End Property
    Public ReadOnly Property SelectedItem As dtoVersionItem Implements IViewTemplateAssociation.SelectedItem
        Get
            Dim item As dtoVersionItem = Nothing

            If Me.DDLtemplates.Items.Count > 0 Then
                Dim value As String = Me.DDLtemplates.SelectedValue
                item = New dtoVersionItem(value.Split("_")(2)) With {.Id = System.Convert.ToInt64(value.Split("_")(1)), .IdTemplate = System.Convert.ToInt64(value.Split("_")(0))}
            End If
            If item Is Nothing Then
                item = New dtoVersionItem(TemplateLevel.Removed) With {.Id = 0, .IdTemplate = 0}
            End If
            Return item
        End Get
    End Property
    Private Property Items As List(Of dtoTemplateItem) Implements IViewTemplateAssociation.Items
        Get
            Return ViewStateOrDefault("Items", New List(Of dtoTemplateItem))
        End Get
        Set(value As List(Of dtoTemplateItem))
            ViewState("Items") = value
        End Set
    End Property
    Public Property RaiseSelectionEvent As Boolean Implements IViewTemplateAssociation.RaiseSelectionEvent
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
        MyBase.SetCulture("uc_TemplateAssociation", "Modules", "DocTemplates")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(HYPpreview, True, True)
            '.setHyperLink(HYPedit, True, True)
            '.setHyperLink(HYPadd, True, True)
            '.setHyperLink(HYPdelete, True, True)
            .setLabel(LBtemplateTitle_t)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idAction As Long, idCommunty As Integer, idTemplate As Long, idVersion As Long, Optional idModule As Integer = 0, Optional moduleCode As String = "", Optional source As lm.Comol.Core.DomainModel.ModuleObject = Nothing) Implements lm.Comol.Core.BaseModules.TemplateMessages.Presentation.IViewTemplateAssociation.InitializeControl
        If Not IsNothing(source) Then
            ItemIdentifier = "template_" & source.ObjectLongID.ToString
        End If
        Me.CurrentPresenter.InitView(idAction, idVersion, idTemplate, idVersion, idModule, moduleCode, source)
    End Sub

    Private Sub LoadTemplates(templates As List(Of dtoTemplateItem)) Implements IViewTemplateAssociation.LoadTemplates
        Me.DDLtemplates.Items.Clear()
        Me.Items = templates

        If Not IsNothing(templates) AndAlso templates.Count() > 0 Then
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
        End If
        Me.DDLtemplates.Items.Insert(0, New ListItem(Resource.getValue("Element.Select"), "0_0_" & CInt(TemplateLevel.None)))
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewTemplateAssociation.DisplaySessionTimeout
        HYPpreview.Visible = False
        Me.DDLtemplates.Enabled = False
    End Sub

    Public Sub LoadEmptyTemplate() Implements IViewTemplateAssociation.LoadEmptyTemplate
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
    'Private Sub SetPreviewLink(ByVal item As DTO_sTemplateVersion)
    '    Me.HYPpreview.Text = Resource.getValue("HYPpreview.Text")
    '    Me.HYPpreview.NavigateUrl = BaseUrl & lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.PreviewTemplate(item, Source)
    'End Sub

    Private Sub DDLtemplates_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLtemplates.SelectedIndexChanged
        ''Modifico l'url del link
        ''Internamente, se l'url non è visibile, non ne viene fatto il bind.
        'Me.SetPreviewLink(SelectedItem)

        If RaiseSelectionEvent Then
            Dim item As dtoVersionItem = SelectedItem
            RaiseEvent TemplateSelected(item.IdTemplate, item.Id)
        End If
        'If isInAjaxPanel Then
        '    RaiseEvent SelectedIndexChange()
        'End If
    End Sub

#End Region

End Class