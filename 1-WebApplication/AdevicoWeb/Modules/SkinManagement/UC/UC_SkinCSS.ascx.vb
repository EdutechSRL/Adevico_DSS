Imports lm.Comol.Modules.Standard.Skin.Domain
Imports lm.Comol.Modules.Standard.Skin.Presentation
Imports System.Linq

Public Class UC_SkinCSS
    Inherits BaseControl
    Implements IViewSkinCSS

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As SkinCssPresenter

    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As SkinCssPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SkinCssPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region
 
   
#Region "Implements"
    Public Property AllowEdit As Boolean Implements IViewSkinCSS.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
            LNBuploadAdminCss.Visible = value
            LNBuploadIeCss.Visible = value
            LNBuploadLoginCss.Visible = value
            LNBuploadMainCss.Visible = value
        End Set
    End Property
    Private Property IdSkin As Long Implements IViewSkinCSS.IdSkin
        Get
            Return ViewStateOrDefault("IdSkin", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSkin") = value
        End Set
    End Property
    Private ReadOnly Property VirtualFullPath As String Implements IViewSkinCSS.VirtualFullPath
        Get
            Return MyBase.BaseUrl & MyBase.SystemSettings.SkinSettings.SkinVirtualPath
        End Get
    End Property
    Private ReadOnly Property BasePath As String Implements IViewSkinCSS.BasePath
        Get
            Return MyBase.SystemSettings.SkinSettings.SkinPhisicalPath
        End Get
    End Property
    Public ReadOnly Property SkinPath As String Implements IViewSkinCSS.SkinPath
        Get
            Return BasePath & Me.IdSkin.ToString & "\"
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
        If IsNothing(MyBase.Resource) Then
            Me.SetCultureSettings()
        End If
        With MyBase.Resource
            .setLabel(LBmainCss_t)
            .setLinkButton(LNBdeleteMainCss, True, True, False, True)
            .setLinkButton(LNBuploadMainCss, True, True, False, True)

            .setLabel(LBadminCss_t)
            .setLinkButton(LNBdeleteAdminCss, True, True, False, True)
            .setLinkButton(LNBuploadAdminCss, True, True, False, True)

            .setLabel(LBieCss_t)
            .setLinkButton(LNBdeleteIeCss, True, True, False, True)
            .setLinkButton(LNBuploadIeCss, True, True, False, True)

            .setLabel(LBloginCss_t)
            .setLabel(LBloginCssInfo_t)
            .setLinkButton(LNBdeleteLoginCss, True, True, False, True)
            .setLinkButton(LNBuploadLoginCss, True, True, False, True)

            .setLinkButton(LNBsaveAll, True, True, False, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idSkin As Long, allowEdit As Boolean) Implements IViewSkinEditor.InitializeControl
        Me.CurrentPresenter.InitView(idSkin, allowEdit)
    End Sub
    Private Sub LoadNoItems() Implements IViewSkinEditor.LoadNoItems
        Me.HYPadminCss.Visible = False
        HYPieCss.Visible = False
        HYPloginCss.Visible = False
        HYPmainCss.Visible = False

    End Sub
    Private Sub LoadItems(dto As DTO.DtoSkinCss) Implements IViewSkinCSS.LoadItems
        With dto
            HYPmainCss.Enabled = Not (.MainCss = "")
            LNBdeleteMainCss.Visible = Not (.MainCss = "") AndAlso AllowEdit
            If .MainCss = "" Then
                Me.HYPmainCss.Text = Resource.getValue("NoCss")
                Me.HYPmainCss.NavigateUrl = "#"
            Else
                Me.HYPmainCss.Text = .MainCss
                Me.HYPmainCss.NavigateUrl = .MainCssUrl
            End If

            HYPadminCss.Enabled = Not (.AdminCss = "")
            LNBdeleteAdminCss.Visible = Not (.AdminCss = "") AndAlso AllowEdit
            If .AdminCss = "" Then
                Me.HYPadminCss.Text = Resource.getValue("NoCss")
                Me.HYPadminCss.NavigateUrl = "#"
            Else
                Me.HYPadminCss.Text = .AdminCss
                Me.HYPadminCss.NavigateUrl = .AdminCssUrl
            End If

            HYPieCss.Enabled = Not (.IeCss = "")
            LNBdeleteIeCss.Visible = Not (.IeCss = "") AndAlso AllowEdit
            If .IeCss = "" Then
                Me.HYPieCss.Text = Resource.getValue("NoCss")
                Me.HYPieCss.NavigateUrl = "#"
            Else
                Me.HYPieCss.Text = .IeCss
                Me.HYPieCss.NavigateUrl = .IeCssUrl
            End If

            HYPloginCss.Enabled = Not (.LoginCss = "")
            LNBdeleteLoginCss.Visible = Not (.LoginCss = "") AndAlso AllowEdit
            If .LoginCss = "" Then
                Me.HYPloginCss.Text = Resource.getValue("NoCss")
                Me.HYPloginCss.NavigateUrl = "#"
            Else
                Me.HYPloginCss.Text = .LoginCss
                Me.HYPloginCss.NavigateUrl = .LoginCssUrl
            End If
        End With
    End Sub
#End Region

    Protected Sub LNBdeleteAdminCss_Click(sender As Object, e As System.EventArgs) Handles LNBdeleteAdminCss.Click, LNBdeleteIeCss.Click, LNBdeleteLoginCss.Click, LNBdeleteMainCss.Click, LNBuploadAdminCss.Click, LNBuploadIeCss.Click, LNBuploadLoginCss.Click, LNBuploadMainCss.Click
        Dim type As lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType).GetByString(sender.CommandArgument, lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.None)
        Select Case sender.CommandName
            Case "delete"
                Me.CurrentPresenter.DeleteCss(IdSkin, type)
            Case "upload"
                If Upload(type) = SkinUpload.SkUp_ErrorCode.none Then

                End If
        End Select
    End Sub

    Private Function GetUploader(type As lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType) As System.Web.UI.WebControls.FileUpload
        Select Case type
            Case lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Main
                Return FUPmainCss
            Case lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Admin
                Return FUPadminCss
            Case lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.IE
                Return FUPieCss
            Case lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Login
                Return FUPloginCss
            Case Else
                Return Nothing
        End Select
    End Function

    Private Function Upload(ByVal type As lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType, Optional ByVal Reload As Boolean = True) As SkinUpload.SkUp_ErrorCode
        Dim cssName As String = ""
        Dim uploader As System.Web.UI.WebControls.FileUpload = GetUploader(type)
        Dim fileName As String = ""
        If Not IsNothing(uploader) Then
            cssName = uploader.FileName
            fileName = CurrentPresenter.GetCssName(type)
        End If

        Dim errorCode As SkinUpload.SkUp_ErrorCode = SkinUpload.SkUp_ErrorCode.none

        If Not String.IsNullOrEmpty(cssName) Then
            errorCode = SkinUpload.UploadFile(uploader, fileName)
            Me.CurrentPresenter.UpdateCss(IdSkin, cssName, type, Reload)
        End If

        Return errorCode
    End Function

    Private Sub LNBsaveAll_Click(sender As Object, e As System.EventArgs) Handles LNBsaveAll.Click

        Me.Upload(lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Main, False)
        Me.Upload(lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Admin, False)
        Me.Upload(lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.IE, False)
        Me.Upload(lm.Comol.Modules.Standard.Skin.Business.SkinFileManagement.CssType.Login, True)

        
    End Sub
End Class