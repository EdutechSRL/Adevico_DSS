Imports lm.Comol.Modules.Standard.Skin.Domain
Imports lm.Comol.Modules.Standard.Skin.Presentation

Public Class UC_SkinImages
    Inherits BaseControl
    Implements IViewSkinImages

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As SkinImagesPresenter

    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As SkinImagesPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SkinImagesPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property AllowEdit As Boolean Implements IViewSkinImages.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
            DVaddImage.Visible = value
        End Set
    End Property
    Private Property IdSkin As Long Implements IViewSkinImages.IdSkin
        Get
            Return ViewStateOrDefault("IdSkin", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSkin") = value
        End Set
    End Property
    Private ReadOnly Property VirtualFullPath As String Implements IViewSkinImages.VirtualFullPath
        Get
            Return MyBase.BaseUrl & MyBase.SystemSettings.SkinSettings.SkinVirtualPath
        End Get
    End Property
    Private ReadOnly Property BasePath As String Implements IViewSkinImages.BasePath
        Get
            Return MyBase.SystemSettings.SkinSettings.SkinPhisicalPath
        End Get
    End Property

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(MyBase.Resource) Then
            Me.SetCultureSettings()
        End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_SkinModuleManagement", "Skin")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBaddNewImage_t)
            .setButton(BTNaddImageFile, True, , , True)
            .setLabel(LBaddZip_t)
            .setButton(BTNaddZipFile, True, , True, True)
            .setLabel(LBnoImages)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idSkin As Long, allowEdit As Boolean) Implements IViewSkinEditor.InitializeControl
        Me.CurrentPresenter.InitView(idSkin, allowEdit)
    End Sub
    Private Sub LoadNoItems() Implements IViewSkinEditor.LoadNoItems
        Me.RPTimages.Visible = False
        LBnoImages.Visible = True
    End Sub
    Private Sub LoadItems(items As IList(Of DTO.DtoSkinImage)) Implements IViewSkinImages.LoadItems
        If IsNothing(items) OrElse items.Count = 0 Then
            LoadNoItems()
        Else
            LBnoImages.Visible = False
            Me.RPTimages.Visible = True
            Me.RPTimages.DataSource = items
            Me.RPTimages.DataBind()
        End If
    End Sub
#End Region

    Private Sub RPTimages_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTimages.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dto As DTO.DtoSkinImage = e.Item.DataItem

            Dim oLinkButton As LinkButton = e.Item.FindControl("LNBremoveImage")
            Resource.setLinkButton(oLinkButton, True, True, False, True)
            oLinkButton.CommandArgument = dto.Name
            oLinkButton.Visible = AllowEdit
            Dim oLabel As Label = e.Item.FindControl("LBimageName")
            oLabel.Text = dto.Name

            oLabel = e.Item.FindControl("LBimageSize")
            oLabel.Text = GetFileSize(dto.SizeByte)
            Dim oImage As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGpreviewImage")
            oImage.ImageUrl = CurrentPresenter.ImageVirtualPath(IdSkin, dto.Name)
            oImage.AlternateText = dto.Name

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Resource.setLabel(e.Item.FindControl("LBimageName_t"))
            Resource.setLabel(e.Item.FindControl("LBimageSize_t"))
            Resource.setLabel(e.Item.FindControl("LBimageDisplay_t"))
        End If
    End Sub

    Private Function GetFileSize(ByVal size As Long) As String
        Dim sizeTranslated As String = ""
        If size = 0 Then
            sizeTranslated = "&nbsp;"
        Else
            If size < 1024 Then
                sizeTranslated = size.ToString & " Kb"
            Else
                size = size / 1024
                If size < 1024 Then
                    sizeTranslated = Math.Round(size) & " kb"
                ElseIf size >= 1024 Then
                    sizeTranslated = Math.Round(size / 1024, 2) & " mb"
                End If
            End If
        End If
        Return sizeTranslated
    End Function
    Private Sub RPTimages_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTimages.ItemCommand
        If e.CommandName = "delete" Then
            Me.CurrentPresenter.DeleteImage(IdSkin, e.CommandArgument)
        End If
    End Sub

    Private Sub BTNaddImageFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddImageFile.Click
        If (Not String.IsNullOrEmpty(Me.FUPimageFile.FileName)) AndAlso IdSkin > 0 AndAlso AllowEdit Then
            SkinUpload.UploadFile(Me.FUPimageFile, CurrentPresenter.ImageFullPath(IdSkin, Me.FUPimageFile.FileName))
        End If
        Me.CurrentPresenter.LoadImages(IdSkin)
    End Sub

    'Private Sub BTNaddZipFile_Click(sender As Object, e As System.EventArgs) Handles BTNaddZipFile.Click
    '    lm.Comol.Core.File.Create.UploadFile(Me.FUPimageFile.PostedFile, CurrentPresenter.ImageFullPath(IdSkin, Me.FUPimageFile.FileName))
    'End Sub
End Class