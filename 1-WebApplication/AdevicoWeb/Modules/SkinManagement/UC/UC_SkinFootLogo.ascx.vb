Imports lm.Comol.Modules.Standard.Skin.Domain
Imports lm.Comol.Modules.Standard.Skin.Presentation
Imports System.Linq
Public Class UC_SkinFootLogo
    Inherits BaseControl
    Implements IViewSkinFooterLogo

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As SkinFooterLogoPresenter

    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As SkinFooterLogoPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SkinFooterLogoPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property AllowEdit As Boolean Implements IViewSkinFooterLogo.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
            DVnewLogo.Visible = value
        End Set
    End Property
    Private Property IdSkin As Long Implements IViewSkinFooterLogo.IdSkin
        Get
            Return ViewStateOrDefault("IdSkin", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSkin") = value
        End Set
    End Property
    Private ReadOnly Property VirtualFullPath As String Implements IViewSkinFooterLogo.VirtualFullPath
        Get
            Return MyBase.BaseUrl & MyBase.SystemSettings.SkinSettings.SkinVirtualPath
        End Get
    End Property
    Private ReadOnly Property BasePath As String Implements IViewSkinFooterLogo.BasePath
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

    Private Property AvailableLanguages As IList(Of DTO.DtoSkinLanguage)
        Get
            Return ViewStateOrDefault("AvailableLanguages", New List(Of DTO.DtoSkinLanguage))
        End Get
        Set(value As IList(Of DTO.DtoSkinLanguage))
            ViewState("AvailableLanguages") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_SkinModuleManagement", "Skin")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBnewLogoInfo_t)
            .setLabel(LBlogoLink_t)
            .setLabel(LBlogoToolTip_t)
            .setLabel(LBlanguageToAssociate_t)
            .setLinkButton(LNBuploadFooterLogo, True, True)

            .setLabel(LBoverrideVoid_t)
            .setCheckBox(CBXovverride)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idSkin As Long, allowEdit As Boolean) Implements IViewSkinFooterLogo.InitializeControl
        Me.TXBfooterLogoToolTip.Text = ""
        Me.TXBfooterLogoLink.Text = ""
        CurrentPresenter.InitView(idSkin, allowEdit)

    End Sub
    Private Sub LoadItems(dto As DTO.DtoFooterLogosList) Implements IViewSkinFooterLogo.LoadItems
        AvailableLanguages = dto.Languages
        Me.LoadLanguages(Me.CBLlanguages, New List(Of String))

        If (Not IsNothing(dto.Logos) OrElse dto.Logos.Count > 0) Then
            Me.RPTfooterLogos.Visible = True
            Me.RPTfooterLogos.DataSource = dto.Logos
            Me.RPTfooterLogos.DataBind()
        Else
            Me.RPTfooterLogos.Visible = False
        End If
    End Sub
    Private Sub LoadNoItems() Implements IViewSkinFooterLogo.LoadNoItems
        Me.RPTfooterLogos.Visible = False
    End Sub
#End Region

    Private Sub RPTfooterLogos_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfooterLogos.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dtoLogo As FooterLogo = e.Item.DataItem

            With MyBase.Resource
                Dim oLinkButton As LinkButton = e.Item.FindControl("LNBdeleteFooterLogo")
                .setLinkButton(oLinkButton, True, True, False, True)
                oLinkButton.CommandArgument = dtoLogo.Id
                oLinkButton.Visible = AllowEdit

                oLinkButton = e.Item.FindControl("LNBeditFooterLogo")
                .setLinkButton(oLinkButton, True, True, False, True)
                oLinkButton.CommandArgument = dtoLogo.Id
                oLinkButton.Visible = AllowEdit

                oLinkButton = e.Item.FindControl("LNBsaveFooterLogo")
                .setLinkButton(oLinkButton, True, True, False, True)
                oLinkButton.CommandArgument = dtoLogo.Id
                oLinkButton.Visible = AllowEdit
                .setLabel(e.Item.FindControl("LBlanguageToAssociate_t"))
                .setLabel(e.Item.FindControl("LBlogoLink_t"))
                .setLabel(e.Item.FindControl("LBlogoToolTip_t"))

                Dim oImage As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGlogo")
                oImage.ImageUrl = CurrentPresenter.LogoVirtualFullPath(dtoLogo.Id, dtoLogo.ImageUrl)
                oImage.AlternateText = dtoLogo.ImageUrl

                Dim oText As TextBox = e.Item.FindControl("TXBfooterLogoLink")
                oText.Text = dtoLogo.Link

                oText = e.Item.FindControl("TXBfooterLogoToolTip")
                oText.Text = dtoLogo.Alt

                Me.LoadLanguages(e.Item.FindControl("CBLlanguages"), dtoLogo.Languages.Select(Function(l) l.LangCode).ToList())

            End With
        End If
    End Sub

    Private Sub RPTfooterLogos_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTfooterLogos.ItemCommand
        Dim idLogo As Long = System.Convert.ToInt64(e.CommandArgument)
        Dim fileUploader As FileUpload = e.Item.FindControl("FUPuploadFooterLogo")
        Dim ImageName As String = SkinUpload.CheckFileName(fileUploader.FileName)

        Select Case e.CommandName
            Case "delete"
                Me.CurrentPresenter.DeleteLogo(idLogo)
            Case "edit"
                Me.CurrentPresenter.UpdateImage(idLogo, ImageName)
                SkinUpload.UploadFile(fileUploader, CurrentPresenter.LogoFullPath(idLogo, ImageName))

            Case "save"
                Dim logoLink, logoToolTip As String
                Dim oTexBox As TextBox = e.Item.FindControl("TXBfooterLogoLink")
                logoLink = oTexBox.Text

                oTexBox = e.Item.FindControl("TXBfooterLogoToolTip")
                logoToolTip = oTexBox.Text


                Me.CurrentPresenter.UpdateLogo(idLogo, ImageName, logoLink, logoToolTip, GetSelectedLanguages(e.Item.FindControl("CBLlanguages")), 0, Me.CBXovverride.Checked)

                If Not ImageName = "" Then
                    SkinUpload.UploadFile(fileUploader, CurrentPresenter.LogoFullPath(idLogo, ImageName))
                End If
        End Select
    End Sub

    Private Sub LNBuploadFooterLogo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBuploadFooterLogo.Click
        Dim idLogo As Long = CurrentPresenter.CreateNewLogo(FUPlogo.FileName, TXBfooterLogoLink.Text, TXBfooterLogoToolTip.Text, GetSelectedLanguages(CBLlanguages), 0)

        If idLogo > 0 AndAlso FUPlogo.FileName <> "" Then
            SkinUpload.UploadFile(FUPlogo, CurrentPresenter.LogoFullPath(idLogo, FUPlogo.FileName))
            Me.TXBfooterLogoToolTip.Text = ""
            Me.TXBfooterLogoLink.Text = ""
        End If
        CurrentPresenter.LoadLogos(IdSkin)
    End Sub

    Private Sub LoadLanguages(ByVal oCheckBoxList As CheckBoxList, ByVal selectedItems As List(Of String))
        oCheckBoxList.Items.Clear()
        oCheckBoxList.DataSource = AvailableLanguages
        oCheckBoxList.DataTextField = "LangName"
        oCheckBoxList.DataValueField = "LangCode"
        oCheckBoxList.DataBind()

        Dim defaultItem As ListItem = oCheckBoxList.Items.FindByValue(AvailableLanguages.Where(Function(l) l.IsDefault).Select(Function(l) l.LangCode).FirstOrDefault())
        If Not IsNothing(defaultItem) Then
            defaultItem.Text &= Resource.getValue("Default.Language")
        End If
        For Each value As String In selectedItems
            Dim oItem As ListItem = oCheckBoxList.Items.FindByValue(value)
            If Not IsNothing(oItem) Then
                oItem.Selected = True
            End If
        Next
    End Sub

    Private Function GetSelectedLanguages(ByVal oCheckBoxList As CheckBoxList) As List(Of String)
        Dim languages As New List(Of String)()

        For Each itm As System.Web.UI.WebControls.ListItem In oCheckBoxList.Items
            If itm.Selected Then
                languages.Add(itm.Value)
            End If
        Next
        Return languages
    End Function


    Public Property OverrideVoid As Boolean
        Get
            Return Me.CBXovverride.Checked
        End Get
        Set(value As Boolean)
            Me.CBXovverride.Checked = value
        End Set
    End Property


End Class