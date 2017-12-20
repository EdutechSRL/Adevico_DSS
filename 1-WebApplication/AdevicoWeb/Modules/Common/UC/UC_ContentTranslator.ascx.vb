Imports lm.Comol.Core.DomainModel.Languages
Imports lm.Comol.Core.BaseModules.CommonControls.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.Comol.Core.DomainModel.Helpers

Public Class UC_ContentTranslator
    Inherits BaseControl
    Implements IViewContentTranslator

#Region "Context"
    Private _Presenter As ContentTranslatorPresenter
    Private ReadOnly Property CurrentPresenter() As ContentTranslatorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ContentTranslatorPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IsInitialized As Boolean Implements IViewContentTranslator.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public Property IsHtml As Boolean Implements IViewContentTranslator.IsHtml
        Get
            Return ViewStateOrDefault("IsHtml", False)
        End Get
        Set(value As Boolean)
            ViewState("IsHtml") = value
        End Set
    End Property
    Public Property ShowBody As Boolean Implements IViewContentTranslator.ShowBody
        Get
            Return ViewStateOrDefault("ShowBody", False)
        End Get
        Set(value As Boolean)
            ViewState("ShowBody") = value
            Me.DVbody.Visible = value
        End Set
    End Property
    Public Property ShowName As Boolean Implements IViewContentTranslator.ShowName
        Get
            Return ViewStateOrDefault("ShowName", False)
        End Get
        Set(value As Boolean)
            ViewState("ShowName") = value
            Me.DVname.Visible = value
        End Set
    End Property
    Public Property ShowShortText As Boolean Implements IViewContentTranslator.ShowShortText
        Get
            Return ViewStateOrDefault("ShowShortText", False)
        End Get
        Set(value As Boolean)
            ViewState("ShowShortText") = value
            Me.DVshortText.Visible = value
        End Set
    End Property
    Public Property ShowSubject As Boolean Implements IViewContentTranslator.ShowSubject
        Get
            Return ViewStateOrDefault("ShowSubject", False)
        End Get
        Set(value As Boolean)
            ViewState("ShowSubject") = value
            Me.DVsubject.Visible = value
        End Set
    End Property
    Public Property AllowValidation As Boolean Implements IViewContentTranslator.AllowValidation
        Get
            Return ViewStateOrDefault("AllowValidation", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowValidation") = value
        End Set
    End Property
    Public Property MandatoryAttributes As List(Of TemplatePlaceHolder) Implements IViewContentTranslator.MandatoryAttributes
        Get
            Return ViewStateOrDefault("MandatoryAttributes", New List(Of TemplatePlaceHolder))
        End Get
        Set(value As List(Of TemplatePlaceHolder))
            ViewState("MandatoryAttributes") = value
        End Set
    End Property
    Private Property TranslatedAttributes As List(Of TemplatePlaceHolder)
        Get
            Return ViewStateOrDefault("TranslatedAttributes", New List(Of TemplatePlaceHolder))
        End Get
        Set(value As List(Of TemplatePlaceHolder))
            ViewState("TranslatedAttributes") = value
        End Set
    End Property
    Public Property MustValidate As Boolean Implements IViewContentTranslator.MustValidate
        Get
            Return ViewStateOrDefault("MustValidate", False)
        End Get
        Set(value As Boolean)
            ViewState("MustValidate") = value
        End Set
    End Property

    Public Property Content As lm.Comol.Core.DomainModel.Languages.dtoObjectTranslation Implements IViewContentTranslator.Content
        Get
            Dim t As lm.Comol.Core.DomainModel.Languages.dtoObjectTranslation = ViewStateOrDefault("TranslatedAttributes", New lm.Comol.Core.DomainModel.Languages.dtoObjectTranslation)
            t.Translation.Name = Me.TXBname.Text
            t.Translation.ShortText = Me.TXBshortText.Text
            t.Translation.Subject = Me.TXBsubject.Text
            t.Translation.IsHtml = IsHtml
            If t.Translation.IsHtml Then
                t.Translation.Body = CTRLhtml.HTML
            Else
                t.Translation.Body = Me.TXBstandard.Text
            End If
            Return t
        End Get
        Set(value As lm.Comol.Core.DomainModel.Languages.dtoObjectTranslation)
            ViewState("TranslatedAttributes") = value
            Me.TXBname.Text = value.Translation.Name
            Me.TXBshortText.Text = value.Translation.ShortText
            Me.TXBsubject.Text = value.Translation.Subject
            If IsHtml Then
                Me.CTRLhtml.HTML = value.Translation.Body
            Else
                Me.TXBstandard.Text = value.Translation.Body
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

#Region "Property"
    Public Property InEditing As Boolean
        Get
            Return ViewStateOrDefault("InEditing", True)
        End Get
        Set(value As Boolean)
            ViewState("InEditing") = value
            Me.TXBname.ReadOnly = Not value
            Me.TXBshortText.ReadOnly = Not value
            Me.TXBstandard.ReadOnly = Not value
            Me.TXBsubject.ReadOnly = Not value
            Me.CTRLhtml.IsEnabled = value

            If IsHtml Then
                If value Then
                    DVbody.Attributes("class") = DVbody.Attributes("class").Replace("readonly", "")
                Else
                    DVbody.Attributes("class") &= " readonly"
                End If
            End If
        End Set

    End Property
    Public Property BodyCssClass As String
        Get
            Return ViewStateOrDefault("BodyCssClass", "")
        End Get
        Set(value As String)
            ViewState("BodyCssClass") = value
            If Not String.IsNullOrEmpty(value) Then
                DVbody.Attributes("class") &= " " & value
            End If
        End Set
    End Property
    'Public Property ContainerRight As String
    '    Get
    '        Return ViewStateOrDefault("ContainerRight", "]")
    '    End Get
    '    Set(value As String)
    '        ViewState("ContainerRight") = value
    '    End Set
    'End Property
    Public Property NewLineMode As lm.Comol.Core.BaseModules.Editor.EditorNewLineModes
        Get
            Return CTRLhtml.NewLineMode
        End Get
        Set(value As lm.Comol.Core.BaseModules.Editor.EditorNewLineModes)
            CTRLhtml.NewLineMode = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_ContentTranslation", "Modules", "Common")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBname_t)
            .setLabel(LBshortText_t)
            .setLabel(LBsubject_t)
            .setLabel(LBplaceholders_t)
        End With
    End Sub
#End Region

#Region "Implements"
    Private Sub DisplaySessionTimeout() Implements IViewContentTranslator.DisplaySessionTimeout
        MLVselector.SetActiveView(VIWempty)
    End Sub
    Public Sub InitializeControl(content As dtoObjectTranslation, item As LanguageItem) Implements IViewContentTranslator.InitializeControl
        InitializeControl(item)
        IsHtml = content.Translation.IsHtml
        Me.CurrentPresenter.InitView(content)
    End Sub
    Public Sub InitializeControl(content As dtoObjectTranslation, item As LanguageItem, attributes As List(Of TemplatePlaceHolder), validateAttributes As Boolean) Implements IViewContentTranslator.InitializeControl
        Me.MustValidate = validateAttributes
        IsHtml = content.Translation.IsHtml
        InitializeControl(item, attributes)
        Me.CurrentPresenter.InitView(content)
    End Sub
    Public Sub InitializeControl(content As dtoObjectTranslation, item As LanguageItem, attributes As List(Of TemplatePlaceHolder), mandatory As List(Of TemplatePlaceHolder), validateAttributes As Boolean) Implements IViewContentTranslator.InitializeControl
        Me.MustValidate = validateAttributes
        Me.MandatoryAttributes = mandatory
        IsHtml = content.Translation.IsHtml
        InitializeControl(item, attributes)
        Me.CurrentPresenter.InitView(content)
    End Sub
    Private Sub InitializeControl(item As LanguageItem)
        LBnameLanguage_t.Text = item.ShortCode
        LBnameLanguage_t.ToolTip = item.Name
        LBshortTextLanguage_t.Text = item.ShortCode
        LBshortTextLanguage_t.ToolTip = item.Name
        LBsubjectLanguage_t.Text = item.ShortCode
        LBsubjectLanguage_t.ToolTip = item.Name
        Me.MLVselector.SetActiveView(VIWactive)
    End Sub
    Private Sub InitializeControl(item As LanguageItem, attributes As List(Of TemplatePlaceHolder))
        InitializeControl(item)
        If IsNothing(attributes) OrElse attributes.Count = 0 Then
            Me.DVplaceholders.Visible = False
            Me.DVdialog.Visible = False
        Else
            Me.DVplaceholders.Visible = True
            Me.DVdialog.Visible = True
            DVdialog.Attributes.Add("Title", Resource.getValue("DVdialog.Title"))
            TranslatedAttributes = attributes

            LBlegend.Visible = (attributes.Count > 0)
            Me.DVplaceholders.Visible = InEditing AndAlso attributes.Count > 0
            If InEditing Then
                RPTplaceHolder.DataSource = attributes
                RPTplaceHolder.DataBind()
            End If

            RPTattributes.DataSource = attributes
            RPTattributes.DataBind()
        End If
    End Sub
    Public Function Validate() As Boolean Implements IViewContentTranslator.Validate
        Return Validate(MandatoryAttributes)
    End Function

    Public Function Validate(attributes As List(Of TemplatePlaceHolder)) As Boolean Implements IViewContentTranslator.Validate
        Dim body As String = IIf(IsHtml, Me.CTRLhtml.HTML, Me.TXBstandard.Text)
        Dim skipItems As New List(Of TemplatePlaceHolder)
        Dim result As Boolean = True
        For Each attribute As TemplatePlaceHolder In attributes
            '            If body.Contains(ContainerLeft & attribute.Id.ToString & ContainerRight) Then
            If body.Contains(attribute.Tag) Then
                result = result AndAlso True
            Else
                skipItems.Add(attribute)
                result = result AndAlso False
            End If
        Next
        LBmandatorySkipped.Visible = Not result
        If Not result Then
            Me.Resource.setLabel(LBmandatorySkipped)
            LBmandatorySkipped.Text = "</br>" & LBmandatorySkipped.Text
            For Each attribute As TemplatePlaceHolder In skipItems
                LBmandatorySkipped.Text &= String.Format("<span title='{0}'>{1}</span>", attribute.ToolTip, attribute.Name) & "&nbsp;"
            Next
        End If
        Return result
    End Function
#End Region

    Private Sub RPTplaceHolder_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTplaceHolder.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim item As TemplatePlaceHolder = DirectCast(e.Item.DataItem, TemplatePlaceHolder)
            Dim oButton As Button = e.Item.FindControl("BTNattribute")

            oButton.Text = item.Name
            oButton.ToolTip = item.ToolTip
            oButton.CommandArgument = item.Tag
            oButton.CssClass = IIf(IsHtml, "addTextTelerik", "addTextTextarea")
            If MandatoryAttributes.Select(Function(a) a.Tag).Contains(item.Tag) Then
                oButton.CssClass &= " required"
            End If

            'oButton.Attributes.Add("rel", ContainerLeft & item.Id.ToString & ContainerRight)
            oButton.Attributes.Add("data-tag", item.Tag)
            oButton.Attributes.Add("data-editor", CTRLhtml.EditorClientId)
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oLabel As Label = e.Item.FindControl("LBdisplayAllPlaceHolders")
            Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBdisplaySomePlaceHolders")
            Resource.setLabel(oLabel)
        End If
    End Sub
    Private Sub RPTattributes_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTattributes.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTtagHeaderTranslatedName")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTtagHeaderValue")
            Resource.setLiteral(oLiteral)

        End If
    End Sub

End Class