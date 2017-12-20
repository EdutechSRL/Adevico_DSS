Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comol.Manager
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.UI.Presentation

Partial Public Class UserActivation
    Inherits PageBase

#Region "View Property"
    Private _PageUtility As OLDpageUtility
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

    Public ReadOnly Property PreLoadedCommunityID() As Integer
        Get
            Dim CommunityID As Integer = -1
            If IsNumeric(Me.Request.QueryString("CommunityID")) Then
                CommunityID = Me.Request.QueryString("CommunityID")
            End If
            Return CommunityID
        End Get
    End Property
    Public ReadOnly Property PreLoadedPersonID() As Integer
        Get
            Dim PersonID As Integer = 0
            If IsNumeric(Me.Request.QueryString("PersonID")) Then
                PersonID = Me.Request.QueryString("PersonID")
            End If
            Return PersonID
        End Get
    End Property
#End Region

#Region "Inherited Property"


    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

#Region "Permission"
    Private _CommunitiesPermission As List(Of ModuleCommunityPermission(Of Services_GestioneIscritti))
    Private ReadOnly Property CommunitiesPermission() As List(Of ModuleCommunityPermission(Of Services_GestioneIscritti))
        Get
            If IsNothing(_CommunitiesPermission) Then
                Dim oList As New List(Of ModuleCommunityPermission(Of Services_GestioneIscritti))
                Dim PermissionsList As IList(Of ServiceBase) = ManagerPersona.GetPermessiServizio(Me.PageUtility.CurrentUser.ID, Services_GestioneIscritti.Codex)

                For Each oPermission As ServiceBase In PermissionsList
                    oList.Add(New ModuleCommunityPermission(Of Services_GestioneIscritti)() With {.ID = oPermission.CommunityID, .Permissions = New Services_GestioneIscritti(oPermission.PermissionString)})
                Next
                _CommunitiesPermission = oList
            End If
            Return _CommunitiesPermission
        End Get
    End Property
    Public ReadOnly Property CommunityPermission() As Services_GestioneIscritti
        Get
            Dim oService As Services_GestioneIscritti = Nothing
            If Not IsNothing(CommunitiesPermission) AndAlso CommunitiesPermission.Count > 0 Then
                oService = (From s In CommunitiesPermission Where s.ID = Me.PreLoadedCommunityID Select s.Permissions).FirstOrDefault()
            End If
            If IsNothing(oService) Then
                oService = Services_GestioneIscritti.Create
            End If
            Return oService
        End Get
    End Property

#End Region
    Private Enum ErroreAccesso
        URLmissingParameters = -100
        SubscriptionDoesntExist = -102
        SubscriptionActivated = -103
        UnableToactivatesubscription = -104
        SubscriptionAlreadyAccepted = -109
        NoPermission = -110
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherited Methods"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.InitView()
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_activateFromMail", "root")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(Me.HYPbackHistory, True, True)
            Me.Master.ServiceTitle = .getValue("title")
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Private Sub InitView()
        Me.HYPbackHistory.Visible = Not Me.CurrentContext.UserContext.isAnonymous
        If Me.HYPbackHistory.Visible Then
            Me.HYPbackHistory.NavigateUrl = Me.Request.UrlReferrer.ToString
            Dim PersonID As Integer = Me.PreLoadedPersonID
            Dim CommunityID As Integer = Me.PreLoadedCommunityID
            If PersonID <= 0 OrElse CommunityID <= 0 Then
                Me.LBinfoAccesso.Text = String.Format(Resource.getValue("ErroreAccesso." & Me.ErroreAccesso.URLmissingParameters), PageUtility.LocalizedMail.SystemSender.Address, PageUtility.LocalizedMail.SubjectPrefix)
            Else
                Dim oPermessi As Services_GestioneIscritti = Me.CommunityPermission()
                If oPermessi.Management OrElse oPermessi.Admin OrElse oPermessi.Change Then
                    Dim oSubscription As New COL_RuoloPersonaComunita
                    Dim oCommunity As New COL_Comunita With {.Id = CommunityID}

                    Dim oPerson As New COL_Persona With {.ID = PersonID}
                    oPerson.Estrai(Me.LinguaID)
                    oCommunity.Estrai()
                    If oCommunity.Errore = Errori_Db.None AndAlso oPerson.Errore = Errori_Db.None Then
                        oSubscription.EstraiByLinguaDefault(CommunityID, PersonID)
                        If oSubscription.Errore = Errori_Db.None Then
                            If oSubscription.Abilitato And oSubscription.Attivato Then
                                COL_Comunita.RemoveWaitingSubscription(PersonID, CommunityID)
                                Me.LBinfoAccesso.Text = String.Format(Resource.getValue("ErroreAccesso." & Me.ErroreAccesso.SubscriptionAlreadyAccepted), oPerson.Anagrafica, oCommunity.Nome)
                            Else
                                oCommunity.AttivaIscritto(PersonID)
                                If oCommunity.Errore = Errori_Db.None Then
                                    COL_Comunita.RemoveWaitingSubscription(PersonID, CommunityID)
                                    Dim oTreeComunita As New COL_TreeComunita
                                    oTreeComunita.Directory = PageUtility.ProfilePath & PersonID & "\"
                                    oTreeComunita.Nome = PersonID & ".xml"
                                    oTreeComunita.CambiaAttivazione(CommunityID, True, Resource)

                                    oCommunity.MailAccettazione(oPerson, Me.PageUtility.CurrentUser, PageUtility.LocalizedMail)
                                    Me.LBinfoAccesso.Text = String.Format(Resource.getValue("ErroreAccesso." & Me.ErroreAccesso.SubscriptionActivated), oPerson.Anagrafica, oCommunity.Nome)
                                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(PersonID))
                                Else
                                    Me.LBinfoAccesso.Text = String.Format(Resource.getValue("ErroreAccesso." & Me.ErroreAccesso.UnableToactivatesubscription), oPerson.Anagrafica, oCommunity.Nome)
                                End If
                            End If
                        Else
                            Me.LBinfoAccesso.Text = Resource.getValue("ErroreAccesso." & Me.ErroreAccesso.SubscriptionDoesntExist)
                        End If
                    Else
                        Me.LBinfoAccesso.Text = Resource.getValue("ErroreAccesso." & Me.ErroreAccesso.SubscriptionDoesntExist)
                    End If
                Else
                    Me.LBinfoAccesso.Text = Resource.getValue("ErroreAccesso." & Me.ErroreAccesso.NoPermission)
                End If
            End If
        Else
            Me.BindNoPermessi()
        End If
    End Sub
End Class