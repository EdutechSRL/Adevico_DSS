Imports lm.Comol.Modules.Standard.Skin.Domain
Imports lm.Comol.Modules.Standard.Skin.Presentation

Public Class UC_SkinFootText
    Inherits BaseControl
    Implements IViewSkinFooterText

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As SkinFooterTextPresenter

    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As SkinFooterTextPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SkinFooterTextPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property AllowEdit As Boolean Implements IViewSkinFooterText.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
        End Set
    End Property
    Private Property IdSkin As Long Implements IViewSkinFooterText.IdSkin
        Get
            Return ViewStateOrDefault("IdSkin", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSkin") = value
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
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_SkinModuleManagement", "Skin")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource

        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idSkin As Long, allowEdit As Boolean) Implements IViewSkinFooterText.InitializeControl
        Me.CurrentPresenter.InitView(idSkin, allowEdit)
    End Sub
    Private Sub LoadItems(items As IList(Of DTO.DtoSkinFooterText)) Implements IViewSkinFooterText.LoadItems
        Me.RPTlanguages.Visible = True
        Me.RPTlanguages.DataSource = items
        Me.RPTlanguages.DataBind()
    End Sub
    Private Sub LoadNoItems() Implements lm.Comol.Modules.Standard.Skin.Presentation.IViewSkinFooterText.LoadNoItems
        Me.RPTlanguages.Visible = False
    End Sub
#End Region

    Private Sub RPTlanguages_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTlanguages.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item As DTO.DtoSkinFooterText = DirectCast(e.Item.DataItem, DTO.DtoSkinFooterText)

            With MyBase.Resource

                Dim oLinkButton As LinkButton = e.Item.FindControl("LNBdeleteFootText")
                If item.Id > 0 Then
                    oLinkButton.Visible = True
                    .setLinkButton(oLinkButton, True, True)
                    oLinkButton.CommandArgument = item.LangCode
                    oLinkButton.CommandName = "Delete"
                    oLinkButton.Visible = AllowEdit
                Else
                    oLinkButton.Visible = False
                End If

                oLinkButton = e.Item.FindControl("LNBsaveFootText")
                .setLinkButton(oLinkButton, True, True)
                oLinkButton.CommandName = "Save"
                oLinkButton.CommandArgument = item.LangCode
                oLinkButton.Visible = AllowEdit

                'BIND
                Dim oLabel As Label = e.Item.FindControl("LBlanguageName_t")
                oLabel.Text = item.LangName
                If item.IsDefault Then
                    oLabel.Text &= Resource.getValue("Language.Default")
                End If

                Dim editor As UC_Editor = e.Item.FindControl("CTRLeditor")
                If Not editor.isInitialized Then
                    editor.InitializeControl(lm.Comol.Modules.Standard.Skin.Business.ServiceSkin.UniqueCode)
                End If
                editor.HTML = item.Text
                editor.IsEnabled = AllowEdit
            End With

        End If
    End Sub
    Private Sub RPTlanguages_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTlanguages.ItemCommand
        Dim languageCode As String = e.CommandArgument
        Select Case e.CommandName
            Case "Delete"
                Me.CurrentPresenter.SaveFooterText(IdSkin, languageCode, "")
            Case "Save"
                Dim editor As UC_Editor = e.Item.FindControl("CTRLeditor")
                Me.CurrentPresenter.SaveFooterText(IdSkin, languageCode, editor.HTML)
        End Select
    End Sub

End Class