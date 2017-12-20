Imports lm.Comol.Core.BaseModules.MailSender.Presentation
Imports lm.ActionDataContract
Imports System.Linq

Public Class UC_SelectMessage
    Inherits BaseControl
    Implements IViewSelectMessage

#Region "Context"
    Private _Presenter As SelectMessagePresenter
    Private ReadOnly Property CurrentPresenter() As SelectMessagePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SelectMessagePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IsInitialized As Boolean Implements IViewSelectMessage.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public ReadOnly Property SelectedItems As List(Of Long) Implements IViewSelectMessage.SelectedItems
        Get
            Dim items As New List(Of Long)

            Dim oLiteral As Literal = Nothing
            Dim oCheck As HtmlInputCheckBox = Nothing

            For Each row As RepeaterItem In RPTmessages.Items
                oCheck = row.FindControl("CBXmessage")
                If oCheck.Checked Then
                    oLiteral = row.FindControl("LTidMessage")
                    items.Add(CLng(oLiteral.Text))
                End If
            Next

            Return items
        End Get
    End Property
    Private Property IdCommunityContainer As Integer Implements IViewSelectMessage.IdCommunityContainer
        Get
            Return ViewStateOrDefault("IdCommunityContainer", 0)
        End Get
        Set(value As Integer)
            ViewState("IdCommunityContainer") = value
        End Set
    End Property
    Private Property IdModuleContainer As Integer Implements IViewSelectMessage.IdModuleContainer
        Get
            Return ViewStateOrDefault("IdModuleContainer", 0)
        End Get
        Set(value As Integer)
            ViewState("IdModuleContainer") = value
        End Set
    End Property
    Private Property CodeModuleContainer As String Implements IViewSelectMessage.CodeModuleContainer
        Get
            Return ViewStateOrDefault("CodeModuleContainer", "")
        End Get
        Set(value As String)
            ViewState("CodeModuleContainer") = value
        End Set
    End Property
    Private Property ObjContainer As lm.Comol.Core.DomainModel.ModuleObject Implements IViewSelectMessage.ObjContainer
        Get
            If Not IsNothing(ViewState("ObjContainer")) Then
                Try
                    Return DirectCast(ViewState("ObjContainer"), lm.Comol.Core.DomainModel.ModuleObject)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
        End Get
        Set(value As lm.Comol.Core.DomainModel.ModuleObject)
            ViewState("ObjContainer") = value
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

#Region "Internal"
    Protected Function MessageCssClass(ByVal item As lm.Comol.Core.Mail.Messages.dtoDisplayMessage) As String
        Return ""
    End Function

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Messages", "Modules", "Mail")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTmessageSentName_t)
            .setLiteral(LTmessageSentAction_t)
            .setLiteral(LTmessageSentOn_t)
        End With
    End Sub
#End Region

#Region "implements"
    Public Sub InitializeControl(isPortal As Boolean, idCommunity As Integer, idModule As Integer, Optional modulecode As String = "") Implements IViewSelectMessage.InitializeControl
        Dim owner As New lm.Comol.Core.Mail.Messages.dtoOwnership
        owner.IsPortal = isPortal
        owner.IdCommunity = idCommunity
        owner.ModuleCode = modulecode
        owner.IdModule = idModule
        owner.Type = IIf(isPortal, lm.Comol.Core.Mail.Messages.OwnershipType.Portal, lm.Comol.Core.Mail.Messages.OwnershipType.Community)
        Me.CurrentPresenter.InitView(owner)
    End Sub

    Public Sub InitializeControl(isPortal As Boolean, idCommunity As Integer, modulecode As String, Optional idModule As Integer = 0) Implements IViewSelectMessage.InitializeControl
        Dim owner As New lm.Comol.Core.Mail.Messages.dtoOwnership
        owner.IsPortal = isPortal
        owner.IdCommunity = idCommunity
        owner.ModuleCode = modulecode
        owner.IdModule = idModule
        owner.Type = IIf(isPortal, lm.Comol.Core.Mail.Messages.OwnershipType.Portal, lm.Comol.Core.Mail.Messages.OwnershipType.Community)
        Me.CurrentPresenter.InitView(owner)
    End Sub

    Public Sub InitializeControl(obj As lm.Comol.Core.DomainModel.ModuleObject) Implements IViewSelectMessage.InitializeControl
        Dim owner As New lm.Comol.Core.Mail.Messages.dtoOwnership
        'owner.IsPortal = isPortal
        'owner.IdCommunity = idCommunity
        'owner.ModuleCode = modulecode
        'owner.IdModule = idModule
        owner.ModuleObject = obj
        owner.Type = lm.Comol.Core.Mail.Messages.OwnershipType.Object
        Me.CurrentPresenter.InitView(owner)
    End Sub
    Public Sub InitializeControl(isPortal As Boolean, idCommunity As Integer, modulecode As String, idModule As Integer, obj As lm.Comol.Core.DomainModel.ModuleObject) Implements IViewSelectMessage.InitializeControl
        Dim owner As New lm.Comol.Core.Mail.Messages.dtoOwnership
        owner.IsPortal = isPortal
        owner.IdCommunity = idCommunity
        owner.ModuleCode = modulecode
        owner.IdModule = idModule
        owner.ModuleObject = obj
        owner.Type = lm.Comol.Core.Mail.Messages.OwnershipType.Object
        Me.CurrentPresenter.InitView(owner)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewSelectMessage.DisplaySessionTimeout
        Me.RPTmessages.DataSource = New List(Of lm.Comol.Core.Mail.Messages.dtoDisplayMessage)
        Me.RPTmessages.DataBind()
    End Sub
    Private Sub LoadMessages(items As List(Of lm.Comol.Core.Mail.Messages.dtoDisplayMessage)) Implements IViewSelectMessage.LoadMessages
        Me.RPTmessages.DataSource = items
        Me.RPTmessages.DataBind()
    End Sub
#End Region

    Private Sub RPTmessages_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmessages.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As lm.Comol.Core.Mail.Messages.dtoDisplayMessage = DirectCast(e.Item.DataItem, lm.Comol.Core.Mail.Messages.dtoDisplayMessage)
                Dim oLabel As Label = e.Item.FindControl("LBmessageSentName")
                oLabel.Text = dto.DisplayName
                If Not String.IsNullOrEmpty(dto.TemplateName) Then
                    oLabel = e.Item.FindControl("LBmessageSeparator")
                    oLabel.Visible = True
                    oLabel = e.Item.FindControl("LBmessageTemplateName")
                    oLabel.Visible = True
                    oLabel.Text = dto.TemplateName
                    If Not dto.ExternalTemplateCompliant Then
                        oLabel.ToolTip = String.Format(Resource.getValue("Not.ExternalTemplateCompliant"), dto.TemplateName)
                    End If
                End If
                oLabel = e.Item.FindControl("LBmessageSentOn")
                oLabel.Text = FormatDateTime(dto.CreatedOn, DateFormat.ShortDate) & " " & FormatDateTime(dto.CreatedOn, DateFormat.ShortTime)

                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPviewMessage")
                Resource.setHyperLink(oHyperlink, False, True)
                oHyperlink.NavigateUrl = PageUtility.BaseUrl & lm.Comol.Core.Mail.Messages.RootObject.ViewMessageTemplate(dto.Id, CodeModuleContainer, IdCommunityContainer, IdModuleContainer, ObjContainer)
            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                Dim oLabel As Label = e.Item.FindControl("LBmessageSentEmptyItems")
                oTableItem.Visible = (Me.RPTmessages.Items.Count = 0)

                oLabel.Text = Resource.getValue("NoMessageFound")
        End Select
    End Sub

  
End Class