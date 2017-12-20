Imports lm.Comol.Core.BaseModules.Errors.Presentation
Public MustInherit Class ERpageBase
    Inherits PageBase
    Implements IViewErrorMessage

#Region "Implements"
#Region "Preload"
    Protected Friend ReadOnly Property PreloadedDisplayError As Boolean Implements IViewErrorMessage.PreloadedDisplayError
        Get
            Return Not String.IsNullOrEmpty(Request.QueryString("displayError")) AndAlso Request.QueryString("displayError").ToLower = "true"
        End Get
    End Property
#End Region
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

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Not Page.IsPostBack Then
            Dim oControl As UC_ActionMessages = GetMessageContainer()
            If Not IsNothing(oControl) Then
                Try
                    oControl.InitializeControl(Resource.getValue("StandardError." & PreloadedDisplayError.ToString), IIf(PreloadedDisplayError, lm.Comol.Core.DomainModel.Helpers.MessageType.error, lm.Comol.Core.DomainModel.Helpers.MessageType.alert))
                Catch ex As Exception
                    oControl.InitializeControl(IIf(PreloadedDisplayError, "Errore grave/Fatal Error/fataler Fehler/erreur fatale/error grave", "url o documento non trovato/URL or document not found/URL oder Datei nicht gefunden/URL ou un document introuvable/URL o documento que no se encuentra"), IIf(PreloadedDisplayError, lm.Comol.Core.DomainModel.Helpers.MessageType.error, lm.Comol.Core.DomainModel.Helpers.MessageType.alert))
                End Try
            End If
        End If
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_errors", "Modules", "Errors")
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
    Public Overrides Sub BindNoPermessi()

    End Sub
    Protected Friend MustOverride Function GetMessageContainer() As UC_ActionMessages

#End Region
End Class