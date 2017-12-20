Imports HtmlRenderer
Imports lm.Comol.Core.BaseModules.NoticeBoard.Presentation
Imports lm.Comol.Core.BaseModules.NoticeBoard.Domain
Imports TheArtOfDev.HtmlRenderer.WinForms

Public Class CreateThumbnail
    Inherits NoticeboardPageBase

    Implements IViewCreateThumbnail

#Region "Context"
    Private _Presenter As CreateThumbnailPresenter
    Public ReadOnly Property CurrentPresenter() As CreateThumbnailPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CreateThumbnailPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "implements"
    Private ReadOnly Property PreloadWorkingSessionId As Guid Implements IViewCreateThumbnail.PreloadWorkingSessionId
        Get
            Dim wsid As String = Request.QueryString("wsid")
            If String.IsNullOrEmpty(wsid) Then
                Return Guid.Empty
            Else
                Try
                    Return New Guid(wsid)
                Catch ex As Exception
                    Return Guid.Empty
                End Try
            End If
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        CurrentPresenter.InitView(PreloadedIdMessage, PreloadedIdCommunity)
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub


    Public Overrides Sub SetInternazionalizzazione()

    End Sub


#End Region
#Region "implements"
    Private Sub GenerateThumbnail(idMessage As Long, idCommunity As Integer) Implements IViewCreateThumbnail.GenerateThumbnail
        Dim req As Net.WebRequest = Net.WebRequest.Create(ApplicationUrlBase & RootObject.DisplayMessage(idMessage, idCommunity))
        Dim rsp As Net.WebResponse = req.GetResponse()

        Using sw As System.IO.StreamReader = New System.IO.StreamReader(rsp.GetResponseStream())
            Dim html As String = sw.ReadToEnd()
            Dim image As System.Drawing.Image = HtmlRender.RenderToImage(html)
            image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png)

        End Using

    End Sub
    'Protected Friend Overrides Sub DisplaySessionTimeout()

    'End Sub
#End Region







End Class