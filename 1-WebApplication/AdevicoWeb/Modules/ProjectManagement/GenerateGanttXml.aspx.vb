Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class ProjectGanttToXml
    Inherits PMpageBaseEdit
    Implements IViewGenerateGantt

#Region "Context"
    Private _Presenter As GenerateGanttPresenter
    Private ReadOnly Property CurrentPresenter() As GenerateGanttPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GenerateGanttPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Private ReadOnly Property PreloadFormatDateString As String Implements IViewGenerateGantt.PreloadFormatDateString
        Get
            Return LTformatDatePattern.Text
            'Dim pattern As String = Request.QueryString("pattern")
            'If String.IsNullOrEmpty(pattern) Then
            '    Return CurrentShortDatePattern
            'Else
            '    Try
            '        Dim odate As DateTime = DateTime.Now
            '        odate.ToString(pattern)
            '        Return pattern
            '    Catch ex As Exception
            '        Return CurrentShortDatePattern
            '    End Try
            'End If
        End Get
    End Property
#End Region

#End Region

#Region "Internal"
    Protected ReadOnly Property CurrentShortDatePattern As String
        Get
            Return Resource.CultureInfo.DateTimeFormat.ShortDatePattern
        End Get
    End Property
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView(PreloadIdProject, PreloadFormatDateString, GetActivityCssClass(), ApplicationUrlBase)
    End Sub
    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource

        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
   
#Region "Display"

#End Region
    Private Sub GenerateXML(project As ProjectForGantt) Implements IViewGenerateGantt.GenerateXML
        'Response.ClearContent()
        'Response.ContentType = "text/xml"
        'Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(ProjectForGantt))
        'serializer.Serialize(Response.OutputStream, project)
        'Response.OutputStream.Flush()
        'Response.Flush()
        Response.Clear()
        Response.ContentType = "text/xml"
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetAllowResponseInBrowserHistory(True)
        Response.AddHeader("Access-Control-Allow-Origin", "*")
        Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(ProjectForGantt))
        serializer.Serialize(Response.Output, project)
        Response.Output.Flush()
        Response.Flush()
        Response.End()
    End Sub

    Private Function GetActivityCssClass() As Dictionary(Of GanttCssClass, String) Implements IViewGenerateGantt.GetActivityCssClass
        Dim dict As New Dictionary(Of GanttCssClass, String)

        dict.Add(GanttCssClass.Critical, LTcritical.Text)
        dict.Add(GanttCssClass.Milestone, LTmilestone.Text)
        dict.Add(GanttCssClass.Summary, LTsummary.Text)
        dict.Add(GanttCssClass.None, LTnone.Text)
        Return dict
    End Function
#End Region

End Class