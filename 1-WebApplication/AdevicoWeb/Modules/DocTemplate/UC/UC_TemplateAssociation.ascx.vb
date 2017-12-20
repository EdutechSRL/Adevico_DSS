Imports DocTemplate = lm.Comol.Core.BaseModules.DocTemplate
Imports lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport

Public Class UC_TemplateAssociation
    Inherits BaseControl
    Implements DocTemplate.Presentation.IViewDocTemplateAssociation

#Region "Context"
    Private _Presenter As DocTemplate.Presentation.DocTemplateAssociationPresenter

    Private ReadOnly Property CurrentPresenter() As DocTemplate.Presentation.DocTemplateAssociationPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New DocTemplate.Presentation.DocTemplateAssociationPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region


#Region "Implemnts"
    Public Property AllowSelect As Boolean Implements DocTemplate.Presentation.IViewDocTemplateAssociation.AllowSelect
        Get
            Return ViewStateOrDefault("AllowSelect", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowSelect") = value
            Me.DDLtemplates.Enabled = value
        End Set
    End Property
    Private Property Source As lm.Comol.Core.DomainModel.ModuleObject Implements DocTemplate.Presentation.IViewDocTemplateAssociation.Source
        Get
            Return ViewStateOrDefault("Source", New lm.Comol.Core.DomainModel.ModuleObject())
        End Get
        Set(value As lm.Comol.Core.DomainModel.ModuleObject)
            ViewState("Source") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements DocTemplate.Presentation.IViewDocTemplateAssociation.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property isInAjaxPanel As Boolean Implements DocTemplate.Presentation.IViewDocTemplateAssociation.isInAjaxPanel
        Get
            Return ViewStateOrDefault("isInAjaxPanel", False)
        End Get
        Set(value As Boolean)
            ViewState("isInAjaxPanel") = value
        End Set
    End Property
    Public WriteOnly Property AllowPreview As Boolean Implements DocTemplate.Presentation.IViewDocTemplateAssociation.AllowPreview
        Set(value As Boolean)
            Me.HYPpreview.Visible = value
            If (value) Then
                Me.DDLtemplates.AutoPostBack = True
            End If
        End Set
    End Property
    Public Property CurrentIdModule As Long Implements DocTemplate.Presentation.IViewDocTemplateAssociation.CurrentIdModule
        Get
            Return ViewStateOrDefault("CurrentIdModule", CLng(0))
        End Get
        Set(value As Long)
            ViewState("CurrentIdModule") = value
        End Set
    End Property
    Private ReadOnly Property DestinationUrl As String Implements DocTemplate.Presentation.IViewDocTemplateAssociation.DestinationUrl
        Get
            Dim url As String = Request.Url.LocalPath
            If Me.BaseUrl <> "/" Then
                url = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
            End If
            url = Server.UrlEncode(url & Request.Url.Query & "#" & ItemIdentifier)
            Return url
        End Get
    End Property
    Private Property Items As List(Of DTO_sTemplate) Implements DocTemplate.Presentation.IViewDocTemplateAssociation.Items
        Get
            Return ViewStateOrDefault("Items", New List(Of DTO_sTemplate))
        End Get
        Set(value As List(Of DTO_sTemplate))
            ViewState("Items") = value
        End Set
    End Property

    Public ReadOnly Property SelectedItem As DTO_sTemplateVersion Implements DocTemplate.Presentation.IViewDocTemplateAssociation.SelectedItem
        Get
            Dim item As DTO_sTemplateVersion

            If Me.DDLtemplates.Items.Count > 0 Then
                Dim value As String = Me.DDLtemplates.SelectedValue
                item = New DTO_sTemplateVersion() With {.Id = System.Convert.ToInt64(value.Split("_")(1)), .IdTemplate = System.Convert.ToInt64(value.Split("_")(0))}
            End If
            If item Is Nothing Then
                item = New DTO_sTemplateVersion() With {.Id = 0, .IdTemplate = 0}
            End If
            Return item
        End Get
    End Property
#End Region


#Region "Internal"
    Public Event SelectedIndexChange()

    Public Property EnabledSelectedIndexChanged As Boolean
        Get
            Return ViewStateOrDefault("EnabledSelectedIndexChanged", False)
        End Get
        Set(value As Boolean)

            If (Me.HYPpreview.Visible) Then
                DDLtemplates.AutoPostBack = True
            Else
                DDLtemplates.AutoPostBack = value
            End If

            ViewState("EnabledSelectedIndexChanged") = value
        End Set
    End Property
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
    Public Sub InitializeControl(idTemplate As Long, idVersion As Long, idModule As Long) Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateAssociation.InitializeControl
        InitializeControl(idTemplate, idVersion, idModule, Nothing)
    End Sub
    Public Sub InitializeControl(idTemplate As Long, idVersion As Long, idModule As Long, owner As lm.Comol.Core.DomainModel.ModuleObject) Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateAssociation.InitializeControl
        If Not IsNothing(owner) Then
            ItemIdentifier = "template_" & Source.ObjectLongID.ToString
            Source = owner
        End If
        CurrentIdModule = idModule
        Me.CurrentPresenter.InitView(idTemplate, idVersion, idModule)
    End Sub

    Public Sub LoadTemplates(templates As List(Of DTO_sTemplate)) Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateAssociation.LoadTemplates
        Me.DDLtemplates.Items.Clear()
        Me.Items = templates

        If Not IsNothing(templates) AndAlso templates.Count() > 0 Then
            For Each dto As DTO_sTemplate In templates
                Me.DDLtemplates.AddItemGroup(dto.Name)
                If (dto.Versions.Count > 0) Then
                    For Each dtoV As DTO_sTemplateVersion In dto.Versions
                        Dim oListitem As New ListItem("", dto.Id.ToString() & "_" & dtoV.Id)
                        Dim ItemText As String = ""
                        If (dtoV.Id <= 0) Then
                            oListitem.Attributes.Add("class", "ItemLast")
                            ItemText = Resource.getValue("Element.Last")
                        ElseIf (dtoV.IsDraft) Then
                            oListitem.Attributes.Add("class", "ItemDraft")
                            ItemText = Resource.getValue("Element.Draft")
                        ElseIf (Not dtoV.IsActive) Then
                            oListitem.Attributes.Add("class", "ItemDisapproved")
                            ItemText = Resource.getValue("Element.Disapproved")
                        Else
                            ItemText = Resource.getValue("Element.Generic")
                        End If

                        If (IsNothing(ItemText) OrElse ItemText = "") Then
                            ItemText = dtoV.Version.ToString
                        End If

                        ' Eventualmente aggiungere altri TAG o usare altre funzioni per il replace

                        ItemText = ItemText.Replace("#item.Version#", dtoV.Version) _
                            .Replace("#item.TemplateName#", dto.Name)

                        If Not IsNothing(dtoV.Lastmodify) Then
                            ItemText = ItemText.Replace("#item.VersionDate#", dtoV.Lastmodify)
                        Else
                            ItemText = ItemText.Replace("#item.VersionDate#", "")
                        End If
                        oListitem.Text = ItemText

                        If (dtoV.IsSelected) Then
                            oListitem.Selected = True
                            Me.SetPreviewLink(dtoV)
                        End If

                        Me.DDLtemplates.Items.Add(oListitem)
                    Next
                End If
            Next
        End If
        Me.DDLtemplates.Items.Insert(0, New ListItem(Resource.getValue("Element.Select"), "0_0"))
    End Sub
    Public Sub DisplaySessionTimeout() Implements DocTemplate.Presentation.IViewDocTemplateAssociation.DisplaySessionTimeout
        HYPpreview.Visible = False
        Me.DDLtemplates.Enabled = False
    End Sub

    Public Sub LoadEmptyTemplate() Implements DocTemplate.Presentation.IViewDocTemplateAssociation.LoadEmptyTemplate
        Me.DDLtemplates.Items.Clear()
        Me.DDLtemplates.Items.Add(New ListItem(Resource.getValue("Element.Select"), "0_0"))
    End Sub

#End Region

#Region "Internal"

    ''' <summary>
    ''' Se il link è visibile, ne imposta l'URL
    ''' </summary>
    ''' <param name="TemplateId">Id del template</param>
    ''' <param name="VersionId">Id della versione</param>
    ''' <remarks>
    ''' Eventualmente correggere con variabili globali (link)
    ''' </remarks>
    Private Sub SetPreviewLink(ByVal item As DTO_sTemplateVersion)
        Me.HYPpreview.Text = Resource.getValue("HYPpreview.Text")
        Me.HYPpreview.NavigateUrl = BaseUrl & lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.PreviewTemplate(item, Source)
    End Sub

    Private Sub DDLtemplates_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLtemplates.SelectedIndexChanged
        'Modifico l'url del link
        'Internamente, se l'url non è visibile, non ne viene fatto il bind.
        Me.SetPreviewLink(SelectedItem)

        If EnabledSelectedIndexChanged Then
            RaiseEvent SelectedIndexChange()
        End If
        'If isInAjaxPanel Then
        '    RaiseEvent SelectedIndexChange()
        'End If
    End Sub

#End Region

End Class