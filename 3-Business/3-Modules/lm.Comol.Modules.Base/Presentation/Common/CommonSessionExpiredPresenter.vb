Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class CommonSessionExpiredPresenter
        Inherits DomainPresenter

        Private _CommonManager As ManagerCommon
        Private Overloads Property CurrentManager() As ManagerCommunityFiles
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerCommunityFiles)
                _CurrentManager = value
            End Set
        End Property
        Private Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property
        Private Overloads ReadOnly Property View() As IViewCommonSessionExpired
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewCommonSessionExpired)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            Dim PersonID As Integer = Me.View.PreloadedForUserID
            Dim LanguageCode As String = Me.View.PreloadedLanguageCode
            Dim oLanguage As Language = Me.CommonManager.GetLanguage(LanguageCode)

            View.CurrentInPopupWindow = View.PreloadedInPopupWindow
            View.CurrentForDownload = View.PreloadedForDownload
            If PersonID > 0 AndAlso (IsNothing(oLanguage) OrElse oLanguage.Id = 0) Then
                Dim oPerson As Person = Me.CommonManager.GetPerson(PersonID)
                oLanguage = Me.CommonManager.GetLanguage(oPerson.LanguageID)
            End If
            If IsNothing(oLanguage) Then
                oLanguage = Me.CommonManager.GetDefaultLanguage
            End If
            Me.View.CurrentCommunityID = Me.View.PreloadedCommunityID
            Me.View.CurrentForUserID = PersonID
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
                If oPerson.AuthenticationTypeID = Main.TipoAutenticazione.IOP Then
                    Me.View.LoadExternalLoginPage(oPerson.AuthenticationTypeID, Me.View.CurrentCommunityID, oPerson.Id, oPerson.Login, Me.View.PreservedDownloadUrl)
                Else
                    Me.View.LoadInternalLoginPage(Me.View.CurrentCommunityID, oPerson.Id, oPerson.Login, Me.View.PreservedDownloadUrl)
                End If
            End If
        End Sub

    End Class
End Namespace