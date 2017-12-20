Public Class UC_AsyncUpload
    Inherits CMbaseControl

#Region "Internal"
    Public ReadOnly Property UploadButtonClientId As String
        Get
            Return BTNasyncUpload.ClientID
        End Get
    End Property
    Public Property MaxFileSize As Integer
        Get
            Return RAUcontrol.MaxFileSize
        End Get
        Set(value As Integer)
            If value < 0 Then
                value = 0
            End If
            RAUcontrol.MaxFileSize = value
            If value = 0 Then
                LIallowedSize.Visible = False
            Else
                LIallowedSize.Visible = False
                LTallowedSize.Text = GetMaxFileSize()
            End If
        End Set
    End Property
    Public Property AllowedFileExtensions As String
        Get
            If RAUcontrol.AllowedFileExtensions.Count = 0 Then
                Return ""
            Else
                Return String.Join(",", RAUcontrol.AllowedFileExtensions)
            End If
        End Get
        Set(value As String)
            If String.IsNullOrEmpty(value) Then
                LIallowedTypes.Visible = False
                RAUcontrol.AllowedFileExtensions = New String() {}
            Else
                LIallowedTypes.Visible = True
                RAUcontrol.AllowedFileExtensions = value.Split(",")
                LTallowedTypes.Text = value
            End If
        End Set
    End Property
    Public Property MaxFileInputsCount As Integer
        Get
            Return RAUcontrol.MaxFileInputsCount
        End Get
        Set(value As Integer)
            RAUcontrol.MaxFileInputsCount = value
        End Set
    End Property
    Public Property TemporaryFolder As String
        Get
            Return RAUcontrol.TemporaryFolder
        End Get
        Set(value As String)
            RAUcontrol.TemporaryFolder = value
        End Set
    End Property
    Public Property TargetFolder As String
        Get
            Return RAUcontrol.TargetFolder
        End Get
        Set(value As String)
            RAUcontrol.TargetFolder = value
        End Set
    End Property
    Public Event UploadedFiles(files As Telerik.Web.UI.UploadedFileCollection)
    Public Property SaveWithExtension As String
        Get
            Return ViewStateOrDefault("SaveWithExtension", "")

        End Get
        Set(value As String)
            ViewState("SaveWithExtension") = value
        End Set
    End Property
    Public Property AutoSave As Boolean
        Get
            Return ViewStateOrDefault("AutoSave", False)

        End Get
        Set(value As Boolean)
            ViewState("AutoSave") = value
        End Set
    End Property
    Public Property GuidName As Boolean
        Get
            Return ViewStateOrDefault("GuidName", False)

        End Get
        Set(value As Boolean)
            ViewState("GuidName") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setButton(BTNasyncUpload, True)
            .setLiteral(LTallowedTypes_t)
            .setLiteral(LTallowedSize_t)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Function GetMaxFileSize() As String
        If RAUcontrol.MaxFileSize = 0 Then
            Return ""
        ElseIf RAUcontrol.MaxFileSize >= 1073741824 Then
            Return Format(RAUcontrol.MaxFileSize / 1024 / 1024 / 1024, "#0.00") _
                 & " GB"
        ElseIf RAUcontrol.MaxFileSize >= 1048576 Then
            Return Format(RAUcontrol.MaxFileSize / 1024 / 1024, "#0.00") & " MB"
        ElseIf RAUcontrol.MaxFileSize >= 1024 Then
            Return Format(RAUcontrol.MaxFileSize / 1024, "#0.00") & " KB"
        ElseIf RAUcontrol.MaxFileSize < 1024 Then
            Return Fix(RAUcontrol.MaxFileSize) & " Bytes"
        End If
        Return ""
    End Function
    Private Sub BTNasyncUpload_Click(sender As Object, e As EventArgs) Handles BTNasyncUpload.Click
        If RAUcontrol.UploadedFiles.Count > 0 Then
            RaiseEvent UploadedFiles(RAUcontrol.UploadedFiles)
        End If
    End Sub
#End Region
End Class