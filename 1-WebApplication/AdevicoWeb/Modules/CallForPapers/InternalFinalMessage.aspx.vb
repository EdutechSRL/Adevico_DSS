Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Public Class InternalFinalMessage
    Inherits PageBase
    Implements IViewFinalMessage


#Region "Context"
    Private _Presenter As FinalMessagePresenter
    Private ReadOnly Property CurrentPresenter() As FinalMessagePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New FinalMessagePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewFinalMessage.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewFinalMessage.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewFinalMessage.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedUniqueID As Guid Implements IViewFinalMessage.PreloadedUniqueID
        Get
            If String.IsNullOrEmpty(Me.Request.QueryString("UniqueID")) Then
                Return Guid.Empty
            Else
                Try
                    Return New Guid(Me.Request.QueryString("UniqueID"))
                Catch ex As Exception
                    Return Guid.Empty
                End Try
            End If
        End Get
    End Property
    Private ReadOnly Property FromPublicList As Boolean Implements IViewFinalMessage.FromPublicList
        Get
            If String.IsNullOrEmpty(Me.Request.QueryString("FromPublicList")) Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdOtherCommunity As Integer Implements IViewFinalMessage.PreloadIdOtherCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idOtherCommunity")) Then
                Return CInt(Me.Request.QueryString("idOtherCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private Property CallType As CallForPaperType Implements IViewFinalMessage.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewFinalMessage.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewFinalMessage.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewFinalMessage.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewFinalMessage.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private Property IdSubmission As Long Implements IViewFinalMessage.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSubmission") = value
        End Set
    End Property
    Private ReadOnly Property PreloadedIdSubmission As Long Implements IViewFinalMessage.PreloadedIdSubmission
        Get
            If IsNumeric(Me.Request.QueryString("idSubmission")) Then
                Return CLng(Me.Request.QueryString("idSubmission"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private Property IdRevision As Long Implements IViewFinalMessage.IdRevision
        Get
            Return ViewStateOrDefault("IdRevision", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdRevision") = value
        End Set
    End Property
    Private ReadOnly Property PreloadedIdRevision As Long Implements IViewFinalMessage.PreloadedIdRevision
        Get
            If IsNumeric(Me.Request.QueryString("idRevision")) Then
                Return CLng(Me.Request.QueryString("idRevision"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private Property isAnonymousSubmission As Boolean Implements IViewFinalMessage.isAnonymousSubmission
        Get
            Return ViewStateOrDefault("isAnonymousSubmission", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("isAnonymousSubmission") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Private Property ContainerName As String
        Get
            Return ViewStateOrDefault("ContainerName", "")
        End Get
        Set(value As String)
            Me.ViewState("ContainerName") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView(False)
        SetInternazionalizzazione()
    End Sub
    Public Overrides Sub BindNoPermessi()
        MLVpreview.SetActiveView(VIWempty)
        Me.LBmessage.Text = Resource.getValue("ServiceFinalMessageNopermission")
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CallSubmission", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHyperLink(HYPlistBottom, CallType.ToString, True, True)
            .setHyperLink(HYPlistTop, CallType.ToString, True, True)
            .setHyperLink(HYPviewSubmissionBottom, CallType.ToString, True, True)
            .setHyperLink(HYPviewSubmissionTop, CallType.ToString, True, True)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewFinalMessage.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, IIf(CallType.CallForBids, ModuleCallForPaper.ActionType.NoPermission, ModuleRequestForMembership.ActionType.NoPermission), , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewFinalMessage.DisplaySessionTimeout
        DisplaySessionTimeout(RootObject.FinalMessage(CallForPaperType.CallForBids, PreloadIdCall, PreloadedIdSubmission, PreloadedIdRevision, PreloadedUniqueID, True, FromPublicList, PreloadView, PreloadIdOtherCommunity))
    End Sub
    Private Sub DisplaySessionTimeout(url As String)
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = url

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Private Sub InitializeView(skin As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext) Implements IViewFinalMessage.InitializeView

    End Sub

    Private Sub SetContainerName(name As String, itemName As String) Implements IViewFinalMessage.SetContainerName
        Dim translation As String = Me.Resource.getValue("serviceTitle.Submit." & CallType.ToString())
        Dim title As String = ""
        If String.IsNullOrEmpty(itemName) Then
            title = String.Format(translation, "", "")
        Else
            title = String.Format(translation, IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        End If
        If Not String.IsNullOrEmpty(itemName) Then
            Me.Master.ServiceTitleToolTip = String.Format(translation, itemName)

        ElseIf Not String.IsNullOrEmpty(name) Then
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community.Submit." & CallType.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Me.Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
        Me.Master.ServiceTitle = title

        ContainerName = title
    End Sub

    Private Sub LoadDefaultMessage() Implements IViewFinalMessage.LoadDefaultMessage
        Me.LTendMessage.Text = Resource.getValue("DefaultMessage." & CallType.ToString)
        Me.MLVpreview.SetActiveView(VIWcall)
    End Sub

    Private Sub LoadMessage(message As String) Implements IViewFinalMessage.LoadMessage
        Me.LTendMessage.Text = message
        Me.MLVpreview.SetActiveView(VIWcall)
    End Sub

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewFinalMessage.LoadUnknowCall
        MLVpreview.SetActiveView(VIWempty)
        Me.LBmessage.Text = Me.Resource.getValue("DisplayCallUnavailable." & CallForPaperType.CallForBids.ToString())
    End Sub

    Private Sub LoadUnknowSubmission(idCommunity As Integer, idModule As Integer, idSubmission As Long, type As CallForPaperType) Implements IViewFinalMessage.LoadUnknowSubmission
        MLVpreview.SetActiveView(VIWempty)
        Me.LBmessage.Text = Me.Resource.getValue("DisplayUnknowSubmission." & CallForPaperType.CallForBids.ToString())
    End Sub
    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewFinalMessage.SetActionUrl
        Select Case action
            Case CallStandardAction.List
                Me.HYPlistTop.NavigateUrl = BaseUrl & url
                Me.HYPlistTop.Visible = True
                Me.HYPlistBottom.NavigateUrl = BaseUrl & url
                Me.HYPlistBottom.Visible = True
            Case CallStandardAction.ViewOwnSubmission
                Me.HYPviewSubmissionBottom.NavigateUrl = BaseUrl & url
                Me.HYPviewSubmissionBottom.Visible = True
                Me.HYPviewSubmissionTop.NavigateUrl = BaseUrl & url
                Me.HYPviewSubmissionTop.Visible = True
        End Select
    End Sub
    Private Sub GoToUrl(url As String) Implements IViewFinalMessage.GoToUrl
        PageUtility.RedirectToUrl(url)
    End Sub

#End Region

   
    Private Sub InternalFinalMessage_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub
End Class