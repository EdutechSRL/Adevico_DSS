Imports lm.Comol.Modules.Standard.Skin.Domain
Imports lm.Comol.Modules.Standard.Skin.Presentation
Imports System.Linq

Public Class UC_SkinHeadLogo
    Inherits BaseControl
    Implements IViewSkinHeaderLogo

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As SkinHeaderLogoPresenter

    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As SkinHeaderLogoPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SkinHeaderLogoPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property AllowEdit As Boolean Implements IViewSkinHeaderLogo.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
        End Set
    End Property
    Private Property IdSkin As Long Implements IViewSkinHeaderLogo.IdSkin
        Get
            Return ViewStateOrDefault("IdSkin", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSkin") = value
        End Set
    End Property
    Private ReadOnly Property VirtualFullPath As String Implements IViewSkinHeaderLogo.VirtualFullPath
        Get
            Return MyBase.BaseUrl & MyBase.SystemSettings.SkinSettings.SkinVirtualPath
        End Get
    End Property
    Private ReadOnly Property BasePath As String Implements IViewSkinHeaderLogo.BasePath
        Get
            Return MyBase.SystemSettings.SkinSettings.SkinPhisicalPath
        End Get
    End Property
    Public Property IdLogoDefaultLanguage As Long Implements IViewSkinHeaderLogo.IdLogoDefaultLanguage
        Get
            Return ViewStateOrDefault("IdLogoDefaultLanguage", -1)
        End Get
        Set(value As Long)
            ViewState("IdLogoDefaultLanguage") = value
        End Set
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
            .setLinkButton(Me.LNBsaveAll, True, True, False, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idSkin As Long, allowEdit As Boolean) Implements IViewSkinEditor.InitializeControl
        Me.CurrentPresenter.InitView(idSkin, allowEdit)
    End Sub
    Public Sub LoadNoItems() Implements IViewSkinEditor.LoadNoItems
        Me.IdLogoDefaultLanguage = 0
    End Sub
    Public Sub LoadItems(items As IList(Of DTO.DtoHeadLogoLang)) Implements IViewSkinHeaderLogo.LoadItems
        Me.IdLogoDefaultLanguage = items.Where(Function(c) c.Language.IsDefault AndAlso Not IsNothing(c.Logo)).Select(Function(c) c.Logo.Id).FirstOrDefault()
        Me.RPTheaderLogos.DataSource = items
        Me.RPTheaderLogos.DataBind()
    End Sub
#End Region

    Private Sub RPTheaderLogos_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTheaderLogos.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dto As DTO.DtoHeadLogoLang = e.Item.DataItem

            With MyBase.Resource
                .setLabel(e.Item.FindControl("LBlogoLink_t"))
                .setLabel(e.Item.FindControl("LBlogoToolTip_t"))

                Dim oLabel As Label = e.Item.FindControl("LBlanguageName")
                oLabel.Text = dto.Language.LangName

                If dto.Language.IsDefault Then
                    oLabel.Text &= .getValue("Default.Language")
                End If

                Dim headerLogo As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGheaderLogo")
                Dim idLogo As Long = 0
                If Not IsNothing(dto.Logo) Then
                    idLogo = dto.Logo.Id
                End If
                If idLogo = 0 OrElse dto.Logo.ImageUrl = "" Then
                    headerLogo.Visible = False
                Else
                    headerLogo.ImageUrl = CurrentPresenter.LogoVirtualFullPath(dto.Logo.Id, dto.Logo.ImageUrl)
                    headerLogo.AlternateText = dto.Logo.ImageUrl
                End If

                Dim oLiteral As Literal = e.Item.FindControl("LTlanguageCode")
                oLiteral.Text = dto.Language.LangCode

                Dim oLinkButton As LinkButton = e.Item.FindControl("LNBdeleteHeaderLogo")
                oLinkButton.CommandArgument = idLogo
                If IsNothing(dto.Logo) OrElse Not AllowEdit Then
                    oLinkButton.Visible = False
                Else
                    .setLinkButton(oLinkButton, True, True, False, True)
                    oLinkButton.Visible = True
                End If

                'Lkb_Save
                oLinkButton = e.Item.FindControl("LNBsaveHeaderLogo")

                .setLinkButton(oLinkButton, True, True, False, Not (IsNothing(dto.Logo)))
                oLinkButton.CommandArgument = e.Item.ItemIndex
                oLinkButton.Visible = AllowEdit
                oLinkButton.CommandArgument = idLogo

                'Lkb_TakeDef
                oLinkButton = e.Item.FindControl("LNBcopyFromDefault")
                oLinkButton.CommandArgument = idLogo
                If dto.Language.IsDefault OrElse Me.IdLogoDefaultLanguage <= 0 OrElse Not AllowEdit Then
                    oLinkButton.Visible = False
                Else
                    .setLinkButton(oLinkButton, True, True, False, Not (IsNothing(dto.Logo)))
                    oLinkButton.Visible = True
                End If

                Dim HDFlogoId As HiddenField = e.Item.FindControl("HDFlogoId")
                HDFlogoId.Value = idLogo

            End With

            Dim oText As TextBox = e.Item.FindControl("TXBheaderLogoLink")
            If IsNothing(dto.Logo) Then
                oText.Text = ""
            Else
                oText.Text = dto.Logo.Link
            End If

            oText = e.Item.FindControl("TXBheaderLogoAlternate")
            If IsNothing(dto.Logo) Then
                oText.Text = ""
            Else
                oText.Text = dto.Logo.Alt
            End If
        End If
    End Sub

    Private Sub RPTheaderLogos_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTheaderLogos.ItemCommand
        Dim idLogo As Long = System.Convert.ToInt64(e.CommandArgument)
        Dim oLiteral As Literal = e.Item.FindControl("LTlanguageCode")
        Dim languageCode As String = oLiteral.Text
        Select Case e.CommandName
            Case "delete"
                Me.CurrentPresenter.DeleteLogo(IdSkin, idLogo)
            Case "save"
                Dim logoLink, logoToolTip As String
                Dim oTexBox As TextBox = e.Item.FindControl("TXBheaderLogoLink")
                logoLink = oTexBox.Text

                oTexBox = e.Item.FindControl("TXBheaderLogoAlternate")
                logoToolTip = oTexBox.Text

                Dim fileUploader As FileUpload = e.Item.FindControl("FUPheaderLogo")
                Dim fileName As String = SkinUpload.CheckFileName(fileUploader.FileName)

                'Dim IsNew As Boolean = Not (idLogo >= 0)

                idLogo = Me.CurrentPresenter.SaveLogo(idLogo, fileName, logoToolTip, logoLink, languageCode)

                If (fileName <> "") Then
                    SkinUpload.UploadFile(fileUploader, CurrentPresenter.LogoFullPath(idLogo, fileName))
                End If

                Me.CurrentPresenter.ClearCache()
                Me.CurrentPresenter.LoadLogos(IdSkin)
            Case "clone"
                If Me.IdLogoDefaultLanguage > 0 Then
                    Me.CurrentPresenter.CloneDefault(IdSkin, idLogo, languageCode)
                End If
            Case Else
                Me.CurrentPresenter.LoadLogos(IdSkin)
        End Select
    End Sub

    Private Sub LNBsaveAll_Click(sender As Object, e As System.EventArgs) Handles LNBsaveAll.Click

        For Each itm As System.Web.UI.WebControls.RepeaterItem In Me.RPTheaderLogos.Items
            If (itm.ItemType = ListItemType.Item OrElse itm.ItemType = ListItemType.AlternatingItem) Then

                Dim HDFlogoId As HiddenField = itm.FindControl("HDFlogoId")

                Dim idLogo As Long = 0

                If Not IsNothing(HDFlogoId) Then
                    Try
                        idLogo = System.Convert.ToInt64(HDFlogoId.Value)
                    Catch ex As Exception

                    End Try

                End If

                'If (idLogo > 0) Then

                Dim oLiteral As Literal = itm.FindControl("LTlanguageCode")
                Dim languageCode As String = oLiteral.Text

                Dim logoLink, logoToolTip As String
                Dim oTexBox As TextBox = itm.FindControl("TXBheaderLogoLink")
                logoLink = oTexBox.Text

                oTexBox = itm.FindControl("TXBheaderLogoAlternate")
                logoToolTip = oTexBox.Text

                Dim fileUploader As FileUpload = itm.FindControl("FUPheaderLogo")
                Dim fileName As String = fileUploader.FileName

                'Dim IsNew As Boolean = Not (idLogo >= 0)

                idLogo = Me.CurrentPresenter.SaveLogo(idLogo, fileName, logoToolTip, logoLink, languageCode)

                If (fileName <> "") Then
                    SkinUpload.UploadFile(fileUploader, CurrentPresenter.LogoFullPath(idLogo, fileName))
                End If
                'End If



            End If

        Next

        Me.CurrentPresenter.ClearCache()
        
        Me.CurrentPresenter.LoadLogos(IdSkin)



    End Sub
End Class