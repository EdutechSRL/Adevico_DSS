Public Class CategoriesUsers
    Inherits PageBase


#Region "Context"
    'Definizion Presenter...

#End Region

#Region "Internal"
    'Property interni


#End Region

#Region "Implements"
    'Property della VIEW

#End Region

#Region "Inherits"
    'Property del PageBase

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


    Private Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    'Sub e Function PageBase

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    'Sub e function della View

#End Region

#Region "Internal"
    'Sub e Function "della pagina"

#End Region

#Region "Handler"
    'Gestione eventi oggetti pagina (Repeater.ItemDataBound(), Button.Click, ...)

#End Region

    'Public Shared Function CssVersion() As String
    '    Return TicketBase.CssVersion()
    'End Function

    Private _CssVerison As String = ""

    Public Function CssVersion() As String

        If String.IsNullOrEmpty(_CssVerison) Then
            Dim tkVerUC As UC_TicketCssVersion = LoadControl(BaseUrl & "/Modules/Ticket/UC/UC_TicketCssVersion.ascx")
            _CssVerison = TkVerUC.GetVersionString()
        End If

        Return _CssVerison
        'Return "?v=201507080935lm"
    End Function

End Class