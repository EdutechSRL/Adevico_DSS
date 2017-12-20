Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.EduPath.Presentation
Imports lm.ActionDataContract

Public Class EduPathIndex
    Inherits PageBase
    Implements IViewEduPathIndex

#Region "Inherits"
    Protected Sub RedirectOnSessionTimeOut(ByVal destinationUrl As String, ByVal idCommunity As Integer, Optional ByVal mustBeInCommunity As Boolean = True)
        If String.IsNullOrEmpty(destinationUrl) OrElse (mustBeInCommunity AndAlso idCommunity = 0) Then
            Response.Redirect(PageUtility.GetDefaultLogoutPage)
        Else
            Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
            Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
            dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

            If destinationUrl.StartsWith("http") Then
                destinationUrl = destinationUrl.Replace("http" & Request.Url.Host & BaseUrl, "")
                destinationUrl = destinationUrl.Replace("https" & Request.Url.Host & BaseUrl, "")
            End If

            dto.DestinationUrl = destinationUrl

            If idCommunity > 0 Then
                dto.IdCommunity = idCommunity
            End If

            webPost.Redirect(dto)
        End If
    End Sub
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Private Property CurrentCommunityID() As Integer
        Get
            Return ViewStateOrDefault("CurrentCommunityID", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentCommunityID") = value
        End Set
    End Property
#End Region

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
    Private _Presenter As EduPathIndexPresenter
    Private ReadOnly Property CurrentPresenter() As EduPathIndexPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EduPathIndexPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region
    Private _CurrentModule As PlainService
    Protected ReadOnly Property CurrentModule As PlainService
        Get
            If IsNothing(_CurrentModule) Then
                _CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
            End If
            Return _CurrentModule

        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"

#End Region

    Public Overrides Sub BindDati()
        Dim idCommunity = ComunitaCorrenteID
        If Page.IsPostBack = False Then
            Me.TMsession.Interval = Me.SystemSettings.Presenter.AjaxTimer
        End If
        If idCommunity > 0 Then
            CurrentCommunityID = idCommunity
            Me.PageUtility.AddActionToModule(idCommunity, CurrentModule.ID, Services_EduPath.ActionType.Access, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, -100), InteractionType.UserWithLearningObject)
            Dim BasePath As String = Server.MapPath("~/File/EduPath/") 'TODO: eliminare la lettura da file una volta finita la sperimentazione di Gianluca
            If lm.Comol.Core.File.Exists.File(BasePath & "title_" & idCommunity & ".txt") Then
                Master.ServiceTitle = lm.Comol.Core.File.ContentOf.TextFile(BasePath & "title_" & idCommunity & ".txt")
                'Aggiunta Default
            ElseIf lm.Comol.Core.File.Exists.File(BasePath & "title_default.txt") Then
                Master.ServiceTitle = lm.Comol.Core.File.ContentOf.TextFile(BasePath & "title_default.txt")
            Else
                Master.ServiceTitle = ComunitaCorrente.Nome
            End If

            LTrender.Text = ""
            If lm.Comol.Core.File.Exists.File(BasePath & "counter_" & idCommunity & ".txt") Then
                Dim fileContent As String = lm.Comol.Core.File.ContentOf.TextFile(BasePath & "counter_" & idCommunity & ".txt")
                If Not String.IsNullOrEmpty(fileContent) Then
                    Dim items As List(Of lm.Comol.Modules.EduPath.Domain.dtoSubscriptionInfo) = CurrentPresenter.GetSubscriptionsInfo(fileContent)
                    If items.Count > 0 Then
                        Dim roleTranslations As List(Of Comol.Entity.Role) = COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).ToList

                        'Dim roleTotal As String = "<li class=""role total"">" & Resource.getValue("TotalSubscribers") & " <span class=""qty"">{0}</span></li>" & vbCrLf
                        Dim roleTotal As String = "<li class=""role total"">Totale iscritti: <span class=""qty"">{0}</span></li>" & vbCrLf
                        Dim role As String = "<li class=""role"">{0}: <span class=""qty"">{1}</span></li>" & vbCrLf
                        Dim roles As String = ""
                        roles &= String.Format(roleTotal, items.Select(Function(i) i.Count).Sum)

                        For Each item As lm.Comol.Modules.EduPath.Domain.dtoSubscriptionInfo In items.Where(Function(i) roleTranslations.Where(Function(r) r.ID = i.IdRole).Any()).ToList()
                            roles &= String.Format(role, roleTranslations.Where(Function(r) r.ID = item.IdRole).Select(Function(r) r.Name).FirstOrDefault(), item.Count.ToString)
                        Next
                        If Not String.IsNullOrEmpty(roles) Then
                            LTrender.Text = String.Format("<!— File Counter  --><div id=""usercounter""><fieldset><legend>{0}</legend><ul class=""roles"">{1}</ul></fieldset></div>", Resource.getValue("SubscribersInfo"), roles)
                        End If
                    End If
                End If
            End If
            If lm.Comol.Core.File.Exists.File(BasePath & "text_" & idCommunity & ".txt") Then
                LTrender.Text &= lm.Comol.Core.File.ContentOf.TextFile(BasePath & "text_" & idCommunity & ".txt")
                'Aggiunta Default
            ElseIf lm.Comol.Core.File.Exists.File(BasePath & "text_default.txt") Then
                LTrender.Text &= lm.Comol.Core.File.ContentOf.TextFile(BasePath & "text_default.txt")
            Else
                LTrender.Text = ""
            End If
        End If

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return Not isUtenteAnonimo
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Sub DisplaySessionTimeout() Implements lm.Comol.Modules.EduPath.Presentation.IViewEduPathIndex.DisplaySessionTimeout

    End Sub

    Private Sub UpdateAction()
        Dim idCommunity = ComunitaCorrenteID
        If idCommunity > 0 Then
            Me.PageUtility.AddActionToModule(idCommunity, CurrentModule.ID, Services_EduPath.ActionType.Access, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, -100), InteractionType.SystemToSystem)
        End If
    End Sub

    Private Sub TMsession_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TMsession.Tick
        UpdateAction()
        If Me.CurrentContext.UserContext.isAnonymous Then
            Me.TMsession.Enabled = False
        End If
    End Sub

    Private Sub EduPathIndex_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
        Me.Master.ShowDocType = True
    End Sub
End Class