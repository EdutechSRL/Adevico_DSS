Public Class InsertEmoticonsTo
    Inherits PageBase

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

#Region "Control"
    Public ReadOnly Property AppUrl As String
        Get
            Return PageUtility.ApplicationUrlBase(True)
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Editor", "Modules", "Common")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        LTemoticonsTitle.Text = Resource.getValue("InsertEmoticons.Title")
        LTemoticonsInfo.Text = Resource.getValue("InsertEmoticons.Info")
    End Sub

    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Function HasPermessi() As Boolean

    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub



    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region



End Class