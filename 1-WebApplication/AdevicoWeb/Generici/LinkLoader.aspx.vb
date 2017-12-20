Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports lm.ActionDataContract

Partial Public Class LinkLoader
    Inherits PageBase

    Private _Utility As OLDpageUtility
    Private ReadOnly Property Utility() As OLDpageUtility
        Get
            If IsNothing(_Utility) Then
                _Utility = New OLDpageUtility(Me.Context)
            End If
            Return _Utility
        End Get
    End Property

#Region "Default property"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

    Private _PreLoadedPageUrl As String

    Private ReadOnly Property PreLoadedCommunityID() As Integer
        Get
            Dim CommunityID As Integer = 0
            Try
                If IsNumeric(Me.Request.QueryString("CommunityID")) Then
                    CommunityID = CInt(Me.Request.QueryString("CommunityID"))
                End If
            Catch ex As Exception

            End Try
            Return CommunityID
        End Get
    End Property
    Private ReadOnly Property PreLoadedLinkID() As Integer
        Get
            Dim LinkID As Integer = 0
            Try
                If IsNumeric(Me.Request.QueryString("LinkID")) Then
                    LinkID = CInt(Me.Request.QueryString("LinkID"))
                End If
            Catch ex As Exception

            End Try
            Return LinkID
        End Get
    End Property
    Private ReadOnly Property PreLoadedURL() As String
        Get
            If _PreLoadedPageUrl = "" Then
                _PreLoadedPageUrl = Me.Request.QueryString("DestinationUrl")
            End If
            Return _PreLoadedPageUrl
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False

        Dim oLink As New COL_RaccoltaLink() With {.ID = Me.PreLoadedLinkID}
        oLink.Estrai()
        If oLink.CMNT_ID = Me.PreLoadedCommunityID Then
            If oLink.CMNT_ID = Me.PageUtility.WorkingCommunityID Then
                Dim ModuleID As Integer = Utility.GetModule(Services_RaccoltaLink.Codex).ID
                Me.Utility.AddActionToModule(ModuleID, Services_RaccoltaLink.ActionType.ViewLink, CreateObjectForUserAction(oLink.ID, ModuleID), lm.ActionDataContract.InteractionType.UserWithLearningObject)
                'Utility.GetModule(Services_RaccoltaLink.Codex).ID, Services_RaccoltaLink.ActionType.ViewLink, CreateObjectToNotify(oLink.ID), lm.ActionDataContract.InteractionType.UserWithLearningObject)
            End If
            Dim Url As String = oLink.Url
            If Not String.IsNullOrEmpty(Me.PreLoadedURL) Then
                Url = Me.PreLoadedURL
            End If

            If Url.StartsWith("http://") OrElse Url.StartsWith("https://") OrElse Url.StartsWith("ftp://") OrElse Url.StartsWith("gopher://") OrElse Url.StartsWith("mailto://") OrElse Url.StartsWith("news:") OrElse Url.StartsWith("telnet://") Then
                Response.Redirect(Url, True)
            Else
                Response.Redirect("http://" & Url, True)
            End If
        Else
            Me.Master.ShowNoPermission = True
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return Me.PreLoadedCommunityID > 0 AndAlso Me.PreLoadedLinkID > 0
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub


    'Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
    '    Utility.CurrentModule = Utility.GetModule(Services_RaccoltaLink.Codex)
    'End Sub

    Private Function CreateObjectForUserAction(ByVal LinkID As Integer, ByVal ModuleID As Integer) As List(Of PresentationLayer.WS_Actions.ObjectAction)
        Dim oList As New List(Of PresentationLayer.WS_Actions.ObjectAction)
        If LinkID > 0 Then
            oList.Add(New PresentationLayer.WS_Actions.ObjectAction() With {.ModuleID = ModuleID, .ObjectTypeId = Services_RaccoltaLink.ObjectType.Link, .ValueID = LinkID})
        End If
        Return oList
    End Function
End Class