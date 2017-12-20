Public Class UC_AsyncUploadHeader
    Inherits CMbaseControl

#Region "Internal"
    Public ReadOnly Property TypeError As String
        Get
            Return Resource.getValue("UploadValidationFailed.type")
        End Get
    End Property
    Public ReadOnly Property SizeError As String
        Get
            Dim message As String = Resource.getValue("UploadValidationFailed.size")
            If message.Contains("{0}") Then
                message = String.Format(message, MaxSize)
            End If
            Return message
        End Get
    End Property
    Public ReadOnly Property ExtensionError As String
        Get
            Return Resource.getValue("UploadValidationFailed.extension")
        End Get
    End Property

    Public Property MaxSize As String
        Get
            Return ViewStateOrDefault("MaxSize", "")
        End Get
        Set(value As String)
            ViewState("MaxSize") = value
        End Set
    End Property

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Sub InitializeControl(items As List(Of String), maxfilesize As String, Optional ByVal buttonUpload As String = "")
        If items.Any() Then
            LTonClientFileSelected.Text = String.Concat(items.Select(Function(n) Replace(LTtemplateDisable.Text, "#clientid#", n) & vbCrLf))
            LTonClientFilesUploaded.Text = String.Concat(items.Select(Function(n) Replace(LTtemplateEnable.Text, "#clientid#", n) & vbCrLf))

        Else
            LTonClientFilesUploaded.Text = ""
            LTonClientFileSelected.Text = ""
        End If
        MaxSize = maxfilesize
        'If Not String.IsNullOrEmpty(buttonUpload) Then
        '    LTonClientFileSelected.Text &= Replace(LTtemplateDisable.Text, "#clientid#", buttonUpload)
        '    LTonClientFilesUploaded.Text &= Replace(LTtemplateEnable.Text, "#clientid#", buttonUpload)
        'End If
    End Sub
End Class