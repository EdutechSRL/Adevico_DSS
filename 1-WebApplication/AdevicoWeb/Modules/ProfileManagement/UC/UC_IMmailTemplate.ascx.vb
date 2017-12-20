Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Mail

Public Class UC_IMmailTemplate
    Inherits BaseControl
    Implements IViewMailTemplate

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Presenter As MailTemplatePresenter
    Private ReadOnly Property CurrentPresenter() As MailTemplatePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MailTemplatePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isInitialized As Boolean Implements IViewMailTemplate.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property MandatoryAttributes As List(Of lm.Comol.Core.Authentication.ProfileAttributeType) Implements IViewMailTemplate.MandatoryAttributes
        Get
            Return ViewStateOrDefault("MandatoryAttributes", New List(Of lm.Comol.Core.Authentication.ProfileAttributeType))
        End Get
        Set(value As List(Of lm.Comol.Core.Authentication.ProfileAttributeType))
            ViewState("MandatoryAttributes") = value
        End Set
    End Property
    Private Property TranslatedAttributes As List(Of TranslatedItem(Of String))
        Get
            Return ViewStateOrDefault("TranslatedAttributes", New List(Of TranslatedItem(Of String)))
        End Get
        Set(value As List(Of TranslatedItem(Of String)))
            ViewState("TranslatedAttributes") = value
        End Set
    End Property
    Public ReadOnly Property isValid As Boolean Implements IViewMailTemplate.isValid
        Get
            Return Me.CTRLmailEditor.Validate(TranslatedAttributes)
        End Get
    End Property
    Public Property SendMailToUsers As Boolean Implements IViewMailTemplate.SendMailToUsers
        Get
            Return (RBLmailSelector.SelectedIndex = 0)
            'Return Boolean.TryParse(Me.RBLmailSelector.SelectedValue, Me.CTRLmailEditor.Visible)
        End Get
        Set(value As Boolean)
            Me.RBLmailSelector.SelectedValue = value.ToString
            Me.CTRLmailEditor.Visible = value
        End Set
    End Property
    Public ReadOnly Property MailContent As dtoMailContent Implements IViewMailTemplate.MailContent
        Get
            Return Me.CTRLmailEditor.Mail
        End Get
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
        MyBase.SetCulture("pg_ProfilesImport", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBemptyMessage)
            .setLabel(LBmailSelector_t)
            .setRadioButtonList(RBLmailSelector, "True")
            .setRadioButtonList(RBLmailSelector, "False")
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(settings As dtoImportSettings) Implements IViewMailTemplate.InitializeControl
        Me.CurrentPresenter.InitView(settings)
    End Sub

    Public Sub DisplayMail(attributes As List(Of ProfileAttributeType)) Implements IViewMailTemplate.DisplayMail

        'If Not isInitialized Then
        '    Dim loginText As String = Resource.getValue("Mail.Login").Replace("<br>", vbCrLf)

        '    Me.TXBstandardMail.Text = Resource.getValue("Mail.Standard").Replace("{0}", IIf(attributes.Contains(ProfileAttributeType.password), loginText, "")).Replace("{1}", Me.PageUtility.ApplicationUrlBase).Replace("<br>", vbCrLf)
        '    Me.CTRLhtmlMail.HTML = Resource.getValue("Mail.Login")
        'End If


        'Me.MandatoryAttributes = attributes
        'Me.CTRLhtmlMail.Visible = isHTMLformat
        'Me.TXBstandardMail.Visible = Not isHTMLformat
        Me.MLVcontrolData.SetActiveView(VIWmailTemplate)

        Dim result As New List(Of TranslatedItem(Of String))
        If IsNothing(attributes) OrElse attributes.Count = 0 Then
            result.Add(New TranslatedItem(Of String) With {.Id = ProfileAttributeType.skip, .Translation = Me.Resource.getValue("ProfileAttributeType.skip")})
        Else
            result = (From s In attributes Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("ProfileAttributeType." & s.ToString)}).ToList
        End If
        result = result.OrderBy(Function(t) t.Translation).ToList()
        TranslatedAttributes = result
        Dim dto As dtoMailContent = CTRLmailEditor.Mail
        If Not isInitialized Then
            Dim loginText As String = Resource.getValue("Mail.Login").Replace("<br>", vbCrLf)

            dto.Subject = Resource.getValue("Mail.Subject")
            dto.Body = Resource.getValue("Mail.Standard").Replace("{0}", IIf(attributes.Contains(ProfileAttributeType.password), loginText, "")).Replace("{1}", Me.PageUtility.ApplicationUrlBase).Replace("<br>", vbCrLf)
            dto.Settings.IsBodyHtml = True
            dto.Settings.SenderType = lm.Comol.Core.MailCommons.Domain.SenderUserType.System
            dto.Settings.PrefixType = lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration
        End If

        Me.CTRLmailEditor.InitializeControl(dto, True, True, result, False)
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewMailTemplate.DisplaySessionTimeout
        Me.MLVcontrolData.SetActiveView(VIWempty)
    End Sub
#End Region

    Private Sub RBLmailSelector_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLmailSelector.SelectedIndexChanged
        Me.CTRLmailEditor.Visible = CBool(RBLmailSelector.SelectedValue)
    End Sub

  
End Class