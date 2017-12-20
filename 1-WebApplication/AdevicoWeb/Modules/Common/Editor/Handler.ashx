<%@ WebHandler Language="VB" Class="Handler" %>
Imports System.Web
Imports lm.Comol.Core.DomainModel 
Imports lm.Comol.Core.BaseModules.Editor
Imports lm.Comol.Core.BaseModules.Editor.Business
Imports PresentationLayer

Public Class Handler
    Implements IHttpHandler
    Implements System.Web.SessionState.IRequiresSessionState
    Implements IDisposable
    
    
#Region "Context"
    Private _Service As ServiceRepositoryContent
    Private _ServiceEditor As ServiceEditor
    Private _Person As Person
    Private _Community As Community
    Private _Utility As OLDpageUtility

    Private ReadOnly Property Utility As OLDpageUtility
        Get
            If IsNothing(_Utility) Then
                _Utility = New OLDpageUtility(Me.Context)
            End If
            Return _Utility
        End Get
    End Property

  
    Private ReadOnly Property CurrentService() As ServiceRepositoryContent
        Get
            If _Service Is Nothing Then
                _Service = New ServiceRepositoryContent(Utility.CurrentContext, Utility.CurrentContext.UserContext.CurrentUserID, Utility.CurrentContext.UserContext.CurrentCommunityID, "", "")
            End If
            Return _Service
        End Get
    End Property
#End Region
#Region "IHttpHandler Members"
    
    
    Private Property Context() As HttpContext
        Get
            Return m_Context
        End Get
        Set(ByVal value As HttpContext)
            m_Context = value
        End Set
    End Property
    Private m_Context As HttpContext

    Public Sub ProcessRequest(ByVal hContext As HttpContext) Implements IHttpHandler.ProcessRequest
        Context = hContext

        If hContext.Request.QueryString("path") Is Nothing Then
            Return
        End If
        Dim path As String = Context.Server.UrlDecode(Context.Request.QueryString("path"))

        Dim item As EditorRepositoryItem = Nothing
        Try
            item = CurrentService.GetItem(New System.Guid(path))
        Catch ex As Exception

        End Try

        If item Is Nothing Then
            Return
        End If

        WriteFile(item, hContext.Response)
    End Sub

    ''' <summary>
    ''' Sends a byte array to the client
    ''' </summary>
    ''' <param name="content">binary file content</param>
    ''' <param name="fileName">the filename to be sent to the client</param>
    ''' <param name="contentType">the file content type</param>
    Private Sub WriteFile(ByVal item As EditorRepositoryItem, ByVal response As HttpResponse)
        response.Buffer = True
        response.Clear()
        response.ContentType = item.MimeType
        Dim extension As String = item.Extension 'System.IO.Path.GetExtension(item.Name & item.Extension).ToLower()
        If extension <> ".htm" AndAlso extension <> ".html" AndAlso extension <> ".xml" AndAlso extension <> ".jpg" AndAlso extension <> ".gif" AndAlso extension <> ".png" Then
            response.AddHeader("content-disposition", "attachment; filename=" & item.Name & item.Extension)
        End If
 
        Try
            response.BinaryWrite(Me.CurrentService.GetItemContent(item, Utility.SystemSettings.BaseFileRepositoryPath.DrivePath))
        Catch ex As Exception

        End Try
     
        response.Flush()
        response.End()
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

'Public ReadOnly Property StorePath() As String
'    Get
'        If String.IsNullOrEmpty(_StorePath) Then
'            _StorePath = ObjectFilePath.CreateByConfigPath(ManagerConfiguration.GetInstance.File.Repository, "", "").Drive
'        End If
'        Return _StorePath
'    End Get
'End Property