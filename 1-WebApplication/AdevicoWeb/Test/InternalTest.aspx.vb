'Imports System.Web.Script.Serialization
Imports Newtonsoft

Public Class InternalTest
    Inherits PageBase

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

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)
        LTsysMessage.Text = errorMessage
    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Me.PageUtility.CurrentUser) OrElse Not Me.PageUtility.CurrentUser.ID = 4 Then

            Response.Write("<h1> Test page. Access not allowed. </h1>")
            Response.End()
        End If

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function


    Private Sub BTNloginTrap_Click(sender As Object, e As EventArgs) Handles BTNloginTrap.Click
        Dim response As String = PageUtility.SendTrapLoginTest()
        LTsysMessage.Text = response
    End Sub

    'Private Sub BtnNewSubscriptionTest_Click(sender As Object, e As EventArgs) Handles BtnNewSubscriptionTest.Click

    '    'Dim testService As New Adevico.Modules.Test.ServiceTest(Me.PageUtility.CurrentContext)

    '    'Me.LTsubscriptionResult.Text = testService.test()


    'End Sub


End Class