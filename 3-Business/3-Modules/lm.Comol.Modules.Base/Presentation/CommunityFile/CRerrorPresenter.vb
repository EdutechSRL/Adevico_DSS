Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class CRerrorPresenter
        Inherits DomainPresenter

        Private _CommonManager As ManagerCommon
        Public Overloads Property CurrentManager() As ManagerCommunityFiles
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerCommunityFiles)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property

        Public Overloads ReadOnly Property View() As IViewCommunityRepositoryError
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewCommunityRepositoryError)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub


        Public Sub InitView()
            Dim PersonID As Integer = Me.View.PreloadedForUserID
            Dim LanguageCode As String = Me.View.PreloadedLanguageCode
            Dim oLanguage As Language = Me.CommonManager.GetLanguage(LanguageCode)


            If PersonID > 0 AndAlso (IsNothing(oLanguage) OrElse oLanguage.Id = 0) Then
                Dim oPerson As Person = Me.CommonManager.GetPerson(PersonID)
                oLanguage = Me.CommonManager.GetLanguage(oPerson.LanguageID)
            End If
            If IsNothing(oLanguage) Then
                oLanguage = Me.CommonManager.GetDefaultLanguage
            End If
            Me.View.CurrentFolderID = Me.View.PreloadedFolderID
            Me.View.CurrentCommunityID = Me.View.PreloadedCommunityID
            Me.View.CurrentForUserID = PersonID
            Me.View.CurrentItemID = Me.View.PreloadedItemID
            Me.View.CurrentFirstError = Me.View.PreloadedFirstError
            Me.View.CurrentItemRepositoryToCreate = View.PreloadedCreate
            Me.View.LoadLanguage(oLanguage)
        End Sub

        Public Sub LogintoSystem()
            Dim PersonID As Integer = Me.View.CurrentForUserID
            If PersonID = 0 Then
                Me.View.LoadStandardLoginPage()
            Else
                Me.LoadUserInfo(PersonID)
            End If
        End Sub

        Public Sub LoadUserInfo(ByVal PersonID As Integer)
            Dim oPerson As Person = Me.CommonManager.GetPerson(PersonID)
            If IsNothing(oPerson) Then
                Me.View.LoadStandardLoginPage()
            Else
                Dim service As New lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(AppContext)
                Dim providers As List(Of lm.Comol.Core.Authentication.AuthenticationProvider) = service.GetUserAuthenticationProviders(oPerson)

                If providers.Count = 0 OrElse oPerson.IdDefaultProvider = 0 OrElse Not providers.Where(Function(p) p.Id.Equals(oPerson.IdDefaultProvider)).Any() Then
                    View.LoadStandardLoginPage()
                Else
                    Dim provider As lm.Comol.Core.Authentication.AuthenticationProvider = providers.Where(Function(p) p.Id.Equals(oPerson.IdDefaultProvider)).FirstOrDefault
                    Select Case provider.ProviderType
                        Case Core.Authentication.AuthenticationProviderType.Internal
                            View.LoadInternalLoginPage(View.CurrentCommunityID, oPerson.Id, oPerson.Login, View.PreservedDownloadUrl)
                        Case Core.Authentication.AuthenticationProviderType.Shibboleth
                            View.LoadShibbolethLoginPage(View.CurrentCommunityID, oPerson.Id, oPerson.Login, View.PreservedDownloadUrl)
                        Case Core.Authentication.AuthenticationProviderType.Url
                            Dim urlProvider As lm.Comol.Core.Authentication.UrlAuthenticationProvider = DirectCast(provider, lm.Comol.Core.Authentication.UrlAuthenticationProvider)
                            View.LoadExternalProviderPage(urlProvider.RemoteLoginUrl, View.CurrentCommunityID, oPerson.Id, oPerson.Login, View.PreservedDownloadUrl)
                        Case Else
                            View.LoadStandardLoginPage()

                    End Select
                End If
            End If
        End Sub
    End Class
End Namespace