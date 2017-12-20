Imports System.Drawing
'Imports apocryph.BitmapManip
Imports System.IO
Imports System.Net
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D

Partial Public Class latexImage
    Inherits System.Web.UI.Page

    Public Function DownloadData(ByVal url As String) As Byte()
        'WebRequest req = WebRequest.Create("[URL here]");
        'WebResponse response = req.GetResponse();
        'Stream stream = response.GetResponseStream();

        Dim req As WebRequest = WebRequest.Create(url)
        Dim response As WebResponse = req.GetResponse()
        Dim stream As Stream = response.GetResponseStream()

        Dim memStream As MemoryStream = New MemoryStream()
        Dim buffer(1024) As Byte
        Dim bytesRead As Integer = 1
        While (bytesRead > 0)
            bytesRead = stream.Read(buffer, 0, buffer.Length)
            memStream.Write(buffer, 0, bytesRead)

        End While

        Return memStream.ToArray()
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim qs As String = Server.UrlDecode(Request.QueryString.ToString)
        Dim oUtility As New OLDpageUtility(Me.Context)
        Dim oServers As New List(Of ForeignRenderServer)
        oServers = oUtility.SystemSettings.Latex.FindAvailableServers

        Dim oBitmpap As Bitmap
        If oServers.Count > 0 Then
            For Each oServer As ForeignRenderServer In oServers
                Try

                    'byte[] imageData = DownloadData(Url); //DownloadData function from here
                    'MemoryStream stream = new MemoryStream(imageData);
                    'Image img = Image.FromStream(stream);
                    'stream.Close();

                    Dim imageData As Byte() = DownloadData(oServer.RemoteUrl + qs)
                    Dim stream As MemoryStream = New MemoryStream(imageData)
                    'Dim img As Image = Image.FromStream(stream)
                    oBitmpap = Bitmap.FromStream(stream)

                    'oBitmpap = apocryph.BitmapManip.BitmapManipulator.GetBitmapFromUri(oServer.RemoteUrl + qs)
                    Exit For
                Catch ex As Exception
                    'Try
                    '	a = apocryph.BitmapManip.BitmapManipulator.GetBitmapFromUri(_ip_latex_alternate + qs)
                    'Catch exc As Exception
                    'End Try
                End Try
            Next

        End If


        Response.Clear()
        Response.ContentType = "image/png"
        Try
            Dim MemStream As New MemoryStream()
            ' send the image to the memory stream then output
            oBitmpap.Save(MemStream, ImageFormat.Png)
            MemStream.WriteTo(Response.OutputStream)
            ' tidy up
            oBitmpap.Dispose()
        Catch ex As Exception

        End Try

    End Sub
End Class