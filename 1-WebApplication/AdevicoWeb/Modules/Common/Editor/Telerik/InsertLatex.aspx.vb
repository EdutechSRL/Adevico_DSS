Imports lm.Comol.Core.BaseModules.Editor
Imports lm.Comol.Core.BaseModules.Editor.Business

Public Class InsertLatex
    Inherits PageBase
#Region "Context"
    Private _Service As Business.ServiceEditor
    Private ReadOnly Property CurrentService() As Business.ServiceEditor
        Get
            If _Service Is Nothing Then
                _Service = New Business.ServiceEditor(PageUtility.CurrentContext)
            End If
            Return _Service
        End Get
    End Property
    'Private _Presenter As EditorPresenter
    'Private ReadOnly Property CurrentPresenter() As EditorPresenter
    '    Get
    '        If IsNothing(_Presenter) Then
    '            _Presenter = New EditorPresenter(Me.PageUtility.CurrentContext, Me)
    '        End If
    '        Return _Presenter
    '    End Get
    'End Property
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

#Region "Control"
    Public ReadOnly Property AppUrl As String
        Get
            Return PageUtility.ApplicationUrlBase(True)
        End Get
    End Property
    Public ReadOnly Property RendeUrl As String
        Get
            Dim config As EditorConfiguration = ServiceEditor.GetConfiguration(PageUtility.BaseUrlDrivePath & RootObject.ConfigurationFile(PageUtility.SystemSettings.EditorConfigurationPath))
            If Not IsNothing(config) Then
                Dim s As lm.Comol.Core.DomainModel.Helpers.Tags.LatexSmartTag = config.AvailableSmartags.Where(Function(f) f.Tag = "latex").FirstOrDefault()
                Return AppUrl & s.RenderUrl
            Else
                Return ""
            End If

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
        LTlatexTitle.Text = Resource.getValue("InsertLatex.Title")
        LTlatexTitleInfo.Text = Resource.getValue("InsertLatex.Info")

        Me.TXBvalue.Attributes.Add("onkeypress", "RenderLatex(this);return true;")
        Me.TXBvalue.Attributes.Add("onKeyDown", "RenderLatex(this);return true;")
        Me.TXBvalue.Attributes.Add("onKeyUp", "RenderLatex(this);return true;")
        Me.TXBvalue.Attributes.Add("onBlur", "RenderLatex(this);return true;")
        Me.TXBvalue.Attributes.Add("onFocus", "RenderLatex(this);return true;")
        Resource.setButton(BTNinsertLatex, True)
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