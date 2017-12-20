Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.BaseModules.MailSender.Presentation

Imports System.Linq
Imports lm.Comol.Core.Mail

Public Class UC_MailEditor
    Inherits BaseControl
    Implements IViewMailEditor


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
    Private _Presenter As MailEditorPresenter
    Private ReadOnly Property CurrentPresenter() As MailEditorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MailEditorPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property AllowCopyToSender As Boolean Implements IViewMailEditor.AllowCopyToSender
        Get
            Return ViewStateOrDefault("AllowCopyToSender", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowCopyToSender") = value
            Me.CBXcopyToSender.Enabled = value
            If Not value Then
                Me.CBXcopyToSender.Checked = False
            End If
        End Set
    End Property
    Public Property AllowFormatSelection As Boolean Implements IViewMailEditor.AllowFormatSelection
        Get
            Return ViewStateOrDefault("AllowFormatSelection", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowFormatSelection") = value
            Me.RBLmailType.Enabled = value
            DVmailType.Visible = value
        End Set
    End Property

    Public Property AllowNotifyToSender As Boolean Implements IViewMailEditor.AllowNotifyToSender
        Get
            Return ViewStateOrDefault("AllowNotifyToSender", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowNotifyToSender") = value
            Me.CBXnotifyToSender.Enabled = value
            If Not value Then
                Me.CBXnotifyToSender.Checked = False
            End If
        End Set
    End Property
    Private Property AllowSenderEdit As Boolean Implements IViewMailEditor.AllowSenderEdit
        Get
            Return ViewStateOrDefault("AllowSenderEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSenderEdit") = value
            Me.DVsender.Visible = value
        End Set
    End Property
    Private Property AllowSubjectEdit As Boolean Implements IViewMailEditor.AllowSubjectEdit
        Get
            Return ViewStateOrDefault("AllowSubjectEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSubjectEdit") = value
            Me.DVsubject.Visible = value
        End Set
    End Property
    Public Property AllowValidation As Boolean Implements IViewMailEditor.AllowValidation
        Get
            Return ViewStateOrDefault("AllowValidation", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowValidation") = value
        End Set
    End Property
    Private Property CopyToSender As Boolean
        Get
            Return Me.CBXcopyToSender.Checked
        End Get
        Set(value As Boolean)
            Me.CBXcopyToSender.Checked = value
        End Set
    End Property
    Private Property NotifyToSender As Boolean
        Get
            Return Me.CBXnotifyToSender.Checked
        End Get
        Set(value As Boolean)
            Me.CBXnotifyToSender.Checked = value
        End Set
    End Property
    Public Property Settings As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings Implements IViewMailEditor.Settings
        Get
            Dim oDto As New lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings

            With oDto
                .IsBodyHtml = isHTMLformat
                .CopyToSender = CopyToSender
                .NotifyToSender = NotifyToSender
                .SenderType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.MailCommons.Domain.SenderUserType).GetByString(Me.DDLsender.SelectedValue, lm.Comol.Core.MailCommons.Domain.SenderUserType.LoggedUser)
                .PrefixType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.MailCommons.Domain.SubjectPrefixType).GetByString(Me.DDLsubject.SelectedValue, lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration)
            End With
            Return oDto
        End Get
        Set(value As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings)
            With value
                isHTMLformat = .IsBodyHtml
                CopyToSender = .CopyToSender
                NotifyToSender = .NotifyToSender
                Me.DDLsubject.SelectedValue = .PrefixType.ToString
                Me.DDLsender.SelectedValue = .SenderType.ToString
                Me.LBpreSubject.Visible = (.PrefixType = lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration)
                Me.TXBstandardMail.Visible = Not .IsBodyHtml
                Me.CTRLhtmlMail.Visible = .IsBodyHtml
            End With
        End Set
    End Property
    Private Property isHTMLformat As Boolean
        Get
            Return (Me.RBLmailType.SelectedIndex = 0)
        End Get
        Set(value As Boolean)
            Me.RBLmailType.SelectedIndex = IIf(value, 0, 1)
        End Set
    End Property
    Public ReadOnly Property Mail As dtoMailContent Implements IViewMailEditor.Mail
        Get
            Dim dto As New dtoMailContent
            With dto
                .Subject = Me.TXBsubject.Text
                .Body = IIf(isHTMLformat, Me.CTRLhtmlMail.HTML, Me.TXBstandardMail.Text)
                .Settings = Settings

            End With
            Return dto
        End Get
    End Property

    Public Property MandatoryAttributes As List(Of TranslatedItem(Of String)) Implements IViewMailEditor.MandatoryAttributes
        Get
            Return ViewStateOrDefault("MandatoryAttributes", New List(Of TranslatedItem(Of String)))
        End Get
        Set(value As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of String)))
            ViewState("MandatoryAttributes") = value
        End Set
    End Property
    Public Property TranslatedAttributes As List(Of TranslatedItem(Of String))
        Get
            Return ViewStateOrDefault("TranslatedAttributes", New List(Of TranslatedItem(Of String)))
        End Get
        Set(value As List(Of TranslatedItem(Of String)))
            ViewState("TranslatedAttributes") = value
        End Set
    End Property
    Public Property ContainerLeft As String Implements IViewMailEditor.ContainerLeft
        Get
            Return ViewStateOrDefault("ContainerLeft", "")
        End Get
        Set(value As String)
            ViewState("ContainerLeft") = value
        End Set
    End Property

    Public Property ContainerRight As String Implements IViewMailEditor.ContainerRight
        Get
            Return ViewStateOrDefault("ContainerRight", "")
        End Get
        Set(value As String)
            ViewState("ContainerRight") = value
        End Set
    End Property
    Public Property DisplayCopyToSender As Boolean Implements IViewMailEditor.DisplayCopyToSender
        Get
            Return ViewStateOrDefault("DisplayCopyToSender", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayCopyToSender") = value
            Me.CBXcopyToSender.Visible = value
        End Set
    End Property
    Public Property DisplayNotifyToSender As Boolean Implements IViewMailEditor.DisplayNotifyToSender
        Get
            Return ViewStateOrDefault("DisplayNotifyToSender", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayNotifyToSender") = value
            Me.CBXnotifyToSender.Visible = value
        End Set
    End Property

    Public Property MustValidate As Boolean Implements IViewMailEditor.MustValidate
        Get
            Return ViewStateOrDefault("MustValidate", False)
        End Get
        Set(value As Boolean)
            ViewState("MustValidate") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewMailEditor.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public ReadOnly Property MailBodyPreview As String
        Get
            If isHTMLformat Then
                Return Me.CTRLhtmlMail.HTML
            Else
                Return Me.TXBstandardMail.Text.Replace(vbCrLf, "</br>")
            End If
        End Get
    End Property
    Public Property RaiseUpdateEvent As Boolean
        Get
            Return ViewStateOrDefault("RaiseUpdateEvent", False)
        End Get
        Set(value As Boolean)
            ViewState("RaiseUpdateEvent") = value
        End Set
    End Property
    Public Event UpdateView()

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region
    Public ReadOnly Property TagOnKeyUpScript As String
        Get
            Return IIf(isHTMLformat, HTMLMailOnKeyUpScript, StandardMailOnKeyUpScript)
        End Get
    End Property

    Public Property StandardMailOnKeyUpScript As String
        Get
            Return ViewStateOrDefault("StandardMailOnKeyUpScript", "")
        End Get
        Set(value As String)
            Me.TXBstandardMail.Attributes.Add("onkeyup", value)
            Me.ViewState("StandardMailOnKeyUpScript") = value
        End Set
    End Property
    Public Property HTMLMailOnKeyUpScript As String
        Get
            Return ViewStateOrDefault("HTMLMailOnKeyUpScript", "")
        End Get
        Set(value As String)
            Me.ViewState("HTMLMailOnKeyUpScript") = value
        End Set
    End Property
    Public WriteOnly Property EditorOnClientCommandExecuted As String
        Set(value As String)
            Me.CTRLhtmlMail.OnClientCommandExecuted = value
        End Set
    End Property
    Public WriteOnly Property EditorClientLoadScript As String
        Set(value As String)
            Me.CTRLhtmlMail.OnClientLoadScript = value
        End Set
    End Property
    Public ReadOnly Property EditorClientId As String
        Get
            Return IIf(isHTMLformat, CTRLhtmlMail.EditorClientId, TXBstandardMail.ClientID)
        End Get
    End Property
    Public ReadOnly Property EditorHTMLClientId As String
        Get
            Return CTRLhtmlMail.EditorClientId
        End Get
    End Property
    Public ReadOnly Property EditorStandardClientId As String
        Get
            Return TXBstandardMail.ClientID
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_MailEditor", "Modules", "Common")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBmailType_t)
            .setLabel(LBsubject_t)
            .setLabel(LBmailMessage_t)
            .setRadioButtonList(RBLmailType, 0)
            .setRadioButtonList(RBLmailType, 1)

            .setLabel(LBsender_t)
            .setDropDownList(DDLsender, "System")
            .setDropDownList(DDLsender, "LoggedUser")

            .setLabel(LBsubjectType_t)

            .setDropDownList(DDLsubject, "SystemConfiguration")
            .setDropDownList(DDLsubject, "None")
            .setCheckBox(CBXcopyToSender)
            .setCheckBox(CBXnotifyToSender)
            Dim oMailLocalized As MailLocalized = PageUtility.LocalizedMail
            'DDLsubject.Items(0).Text & =  " - " & oMailLocalized.SubjectPrefix
            Me.LBpreSubject.Text = oMailLocalized.SubjectPrefix
            Try
                DDLsender.Items(1).Text = String.Format(DDLsender.Items(1).Text, SystemSettings.Mail.SystemSender.Address)
            Catch ex As Exception

            End Try
            .setLabel(LBtags_t)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(dtoContent As dtoMailContent, senderEdit As Boolean, subjectEdit As Boolean) Implements IViewMailEditor.InitializeControl
        Me.SetInternazionalizzazione()
        Me.MustValidate = False
        Me.AllowValidation = False
        Me.CurrentPresenter.InitView(dtoContent, senderEdit, subjectEdit)
        DisplayMail(dtoContent, Nothing)
    End Sub

    Public Sub InitializeControl(dtoContent As dtoMailContent, senderEdit As Boolean, subjectEdit As Boolean, attributes As List(Of TranslatedItem(Of String)), validateAttributes As Boolean) Implements IViewMailEditor.InitializeControl
        Me.SetInternazionalizzazione()
        Me.MustValidate = validateAttributes
        Me.CurrentPresenter.InitView(dtoContent, senderEdit, subjectEdit)
        DisplayMail(dtoContent, attributes)

    End Sub

    Public Sub InitializeControl(dtoContent As dtoMailContent, senderEdit As Boolean, subjectEdit As Boolean, attributes As List(Of TranslatedItem(Of String)), mandatory As List(Of TranslatedItem(Of String)), validateAttributes As Boolean) Implements IViewMailEditor.InitializeControl
        Me.SetInternazionalizzazione()
        Me.MustValidate = validateAttributes
        Me.MandatoryAttributes = mandatory
        Me.CurrentPresenter.InitView(dtoContent, senderEdit, subjectEdit)
        DisplayMail(dtoContent, attributes)
    End Sub

    Public Sub DisplayMail(dtoContent As dtoMailContent, attributes As List(Of TranslatedItem(Of String)))

        Me.Settings = dtoContent.Settings
        If Not Me.CTRLhtmlMail.isInitialized Then
            Me.CTRLhtmlMail.InitializeControl()
        End If
        If String.IsNullOrEmpty(dtoContent.Body) Then
            Me.TXBstandardMail.Text = ""
            Me.CTRLhtmlMail.HTML = ""
        Else
            Me.TXBstandardMail.Text = dtoContent.Body.Replace("<br>", vbCrLf)
            Me.CTRLhtmlMail.HTML = dtoContent.Body.Replace(vbCrLf, "<br>")
        End If
        Me.TXBsubject.Text = dtoContent.Subject

        If IsNothing(attributes) OrElse attributes.Count = 0 Then
            Me.DVattributes.Visible = False
            Me.DVdialog.Visible = False
        Else
            Me.DVattributes.Visible = True
            Me.DVdialog.Visible = True
            DVdialog.Attributes.Add("Title", Resource.getValue("DVdialog.Title"))
            TranslatedAttributes = attributes

            RPTplaceHolder.DataSource = attributes
            RPTplaceHolder.DataBind()

            RPTattributes.DataSource = attributes
            RPTattributes.DataBind()
        End If


        Me.DVnotificationCopy.Visible = AllowCopyToSender OrElse AllowNotifyToSender
    End Sub

    Public Sub SetUserMail(userMail As String) Implements IViewMailEditor.SetUserMail
        Me.DDLsender.Items(0).Text = userMail
    End Sub
#End Region

#Region ""
    Private Sub RPTplaceHolder_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTplaceHolder.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim item As TranslatedItem(Of String) = DirectCast(e.Item.DataItem, TranslatedItem(Of String))
            Dim oButton As Button = e.Item.FindControl("BTNattribute")

            oButton.Text = item.Translation
            oButton.CommandArgument = item.Id.ToString
            oButton.CssClass = IIf(isHTMLformat, "addTextTelerik", "addTextTextarea")
            If MandatoryAttributes.Select(Function(a) a.Id).Contains(item.Id) Then
                oButton.CssClass &= " required"
            End If

            oButton.Attributes.Add("rel", ContainerLeft & item.Id.ToString & ContainerRight)

        ElseIf e.Item.ItemType = ListItemType.Header Then
            'Dim oLabel As Label = e.Item.FindControl("LBattributesInfo")
            'Resource.setLabel(oLabel)
            ''Aggiunto per evitare di mandare a capo i pulsanti se la label è vuota.
            'If Not oLabel.Text = "" Then
            '    oLabel.Text &= "<br/>"
            'End If
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oLabel As Label = e.Item.FindControl("LBlegend")
            oLabel.Visible = (Me.RPTplaceHolder.Items.Count > 0)
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
    Private Sub RBLmailType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLmailType.SelectedIndexChanged
        If isHTMLformat AndAlso Me.TXBstandardMail.Visible Then
            Me.CTRLhtmlMail.HTML = Me.TXBstandardMail.Text.Replace(vbCrLf, "</br>")
        ElseIf Not isHTMLformat AndAlso Me.CTRLhtmlMail.Visible Then
            Me.TXBstandardMail.Text = Me.CTRLhtmlMail.Text
        End If

        Me.CTRLhtmlMail.Visible = isHTMLformat
        Me.TXBstandardMail.Visible = Not isHTMLformat

        RPTplaceHolder.DataSource = TranslatedAttributes
        RPTplaceHolder.DataBind()
        If RaiseUpdateEvent Then
            RaiseEvent UpdateView()
        End If
    End Sub

    Private Sub DDLsubject_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLsubject.SelectedIndexChanged
        Me.LBpreSubject.Visible = (Me.DDLsubject.SelectedIndex = 0)
        If RaiseUpdateEvent Then
            RaiseEvent UpdateView()
        End If
    End Sub
#End Region

    Public Function Validate() As Boolean Implements IViewMailEditor.Validate
        Return Validate(MandatoryAttributes)
    End Function

    Public Function Validate(attributes As List(Of TranslatedItem(Of String))) As Boolean Implements IViewMailEditor.Validate
        Dim body As String = IIf(isHTMLformat, Me.CTRLhtmlMail.HTML, Me.TXBstandardMail.Text)
        Dim skipItems As New List(Of TranslatedItem(Of String))
        Dim result As Boolean = True
        For Each attribute As TranslatedItem(Of String) In attributes
            If body.Contains(ContainerLeft & attribute.Id.ToString & ContainerRight) Then
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
            For Each attribute As String In skipItems.Select(Function(i) i.Translation).ToList
                LBmandatorySkipped.Text &= attribute & " "
            Next
        End If
        Return result
    End Function
End Class