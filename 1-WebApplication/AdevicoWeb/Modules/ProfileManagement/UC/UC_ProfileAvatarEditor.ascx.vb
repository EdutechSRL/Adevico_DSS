Imports lm.Comol.Core.File
Public Class UC_ProfileAvatarEditor
    Inherits BaseControl


#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Public"
    Public Function UploadButtonClientID() As String
        Return Me.BTNupload.ClientID
    End Function
    Public Function UploadButton() As Button
        Return Me.BTNupload
    End Function
    Public Event UploadedAvatar(ByVal avatar As String)
    Public Property IsInitialized As Boolean
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Private Property IdProfile As Integer
        Get
            Return ViewStateOrDefault("IdProfile", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Event CloseWindow()
#End Region


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim form As HtmlForm = Page.Form
        If Not IsNothing(form) AndAlso form.Enctype.Length = 0 Then
            form.Enctype = "multipart/form-data"
        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfileInfo", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBavatarInsert_t)
            .setLabel(LBavatarInfo)
            .setButton(BTNupload, True)
            .setButton(BTNcloseMailWindowFromEditor, True)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(idUser As Integer)
        Me.IsInitialized = True
        IdProfile = idUser
        SPNerrors.Visible = False

    End Sub


    Private Function UpdateAvatar(ByVal idUser As Integer, ByRef avatar As String) As Boolean
        Dim intThumbHeight As Integer = 125
        Dim intThumbWidth As Integer = 100

        If idUser > 0 AndAlso Not IsNothing(INFavatar.PostedFile) Then
            Dim filePath As String = PageUtility.ProfilePath & idUser.ToString & "/"
            Dim filePathTemp As String = PageUtility.ProfilePath & idUser.ToString & "/temp/"
            Create.Directory(filePathTemp)

            Dim avatarFile As HttpPostedFile = INFavatar.PostedFile
            If (avatarFile.ContentLength > 0) Then
                Dim extension As String = System.IO.Path.GetExtension(avatarFile.FileName).ToLower

                If extension = ".jpg" OrElse extension = ".gif" OrElse extension = ".png" Then
                    Dim tempName As String = filePathTemp & Guid.NewGuid.ToString & ".avatar"
                    Dim avatarName As String = Guid.NewGuid.ToString & extension
                    Dim result As FileMessage = Create.UploadFile(avatarFile, tempName)
                    If result = FileMessage.FileCreated Then
                        Dim myBitmap As Bitmap
                        Dim oImpersonate As New Impersonate
                        Try
                            oImpersonate.ImpersonateValidUser()

                            myBitmap = New Bitmap(tempName)
                            Dim myCallBack As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)
                            Dim myThumbnail As System.Drawing.Image = myBitmap.GetThumbnailImage(intThumbWidth, intThumbHeight, myCallBack, IntPtr.Zero)
                            myThumbnail.Save(filePath & avatarName)

                            oImpersonate.UndoImpersonation()
                            myThumbnail.Dispose()
                            myBitmap.Dispose()
                            avatar = avatarName
                            Return True
                        Catch ex As Exception
                            oImpersonate.UndoImpersonation()
                            LBavatarErrors.Text = Resource.getValue("LBavatarErrors.ThumbnailImage")
                        Finally
                            oImpersonate.UndoImpersonation()
                        End Try
                    Else
                        LBavatarErrors.Text = Resource.getValue("LBavatarErrors.FileCreated")
                    End If
                Else
                    LBavatarErrors.Text = Resource.getValue("LBavatarErrors.extension")
                End If
            Else
                LBavatarErrors.Text = Resource.getValue("LBavatarErrors.ContentLength")
            End If
        End If
        Return False

    End Function

    Public Function ThumbnailCallback() As Boolean
        Return False
    End Function

    Private Sub BTNupload_Click(sender As Object, e As System.EventArgs) Handles BTNupload.Click
        Dim avatar As String = ""
        If Me.UpdateAvatar(IdProfile, avatar) Then
            SPNerrors.Visible = False
            RaiseEvent UploadedAvatar(avatar)
        Else
            SPNerrors.Visible = True
        End If
    End Sub

    Private Sub BTNcloseMailWindowFromEditor_Click(sender As Object, e As System.EventArgs) Handles BTNcloseMailWindowFromEditor.Click
        RaiseEvent CloseWindow()
    End Sub
End Class