Imports lm.Comol.Modules.Standard.Skin.Domain
Imports lm.Comol.Modules.Standard.Skin.Presentation

Public Class UC_SkinTemplate
    Inherits BaseControl
    Implements IViewSkinTemplate

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As SkinTemplatePresenter

    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As SkinTemplatePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SkinTemplatePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property AllowEdit As Boolean Implements IViewSkinTemplate.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
            Me.BTNsaveTemplate.Visible = value
            Me.RBLfooterTemplate.Enabled = value
            Me.RBLheaderTemplate.Enabled = value
        End Set
    End Property
    Private Property IdSkin As Long Implements IViewSkinTemplate.IdSkin
        Get
            Return ViewStateOrDefault("IdSkin", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSkin") = value
        End Set
    End Property
    Private ReadOnly Property SelectedIdHeader As Long Implements IViewSkinTemplate.SelectedIdHeader
        Get
            Return SelectedId(RBLheaderTemplate)
        End Get
    End Property
    Private ReadOnly Property SelectedIdFooter As Long Implements IViewSkinTemplate.SelectedIdFooter
        Get
            Return SelectedId(RBLfooterTemplate)
        End Get
    End Property

    Private Function SelectedId(ByVal rbl As RadioButtonList) As Long
        If rbl.Items.Count = 0 Then
            Return 0
        Else
            Return Convert.ToInt64(rbl.SelectedValue)
        End If
    End Function
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
            .setLabel(LBfooterTemplate_t)
            .setLabel(LBheaderTemplate_t)
            .setButton(BTNsaveTemplate, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idSkin As Long, allowEdit As Boolean) Implements IViewSkinTemplate.InitializeControl
        Me.CurrentPresenter.InitView(idSkin, allowEdit)
    End Sub

    Public Sub LoadTemplates(items As DTO.DtoSkinTemplates) Implements IViewSkinTemplate.LoadTemplates
        Me.RBLfooterTemplate.Items.Clear()
        Me.RBLheaderTemplate.Items.Clear()

        Me.RBLfooterTemplate.Items.Add(Me.SetItem(0, Resource.getValue("template.null"), "flow", True))
        Me.RBLheaderTemplate.Items.Add(Me.SetItem(0, Resource.getValue("template.null"), "flow", True))

        For Each ht As DTO.DtoSkinTemplate In items.HeaderTemplates
            Me.RBLheaderTemplate.Items.Add(Me.SetItem(ht.Id, ht.Name, ht.Css, If(ht.Id = items.CurrentHeaderTemplareID, True, False)))
        Next

        For Each ft As DTO.DtoSkinTemplate In items.FooterTemplates
            Me.RBLfooterTemplate.Items.Add(Me.SetItem(ft.Id, ft.Name, ft.Css, If(ft.Id = items.CurrentFooterTemplareID, True, False)))
        Next
    End Sub
#End Region

    Private Sub BTNsaveTemplate_Click(sender As Object, e As System.EventArgs) Handles BTNsaveTemplate.Click
        Me.CurrentPresenter.SaveTemplate()
    End Sub


    Private Function SetItem(ByVal Id As Int64, ByVal Name As String, ByVal Css As String, Optional ByVal isSelected As Boolean = False) As System.Web.UI.WebControls.ListItem
        Dim oListItem As New System.Web.UI.WebControls.ListItem()
        oListItem.Value = Id

        Dim strText As String = ""
        strText &= Name & "<br />" & _
            "<span class=""item " & Css & """ > " & _
           "<ul>" & _
            "<li><img src=""" & Me.BaseUrl & "images/list.jpg"" alt=""sample"" /></li>" & _
            "<li><img src=""" & Me.BaseUrl & "images/list.jpg"" alt=""sample"" /></li>" & _
            "<li><img src=""" & Me.BaseUrl & "images/list.jpg"" alt=""sample"" /></li>" & _
            "</ul>" & _
            "<span>Sample text <br /> Sample text</span>" & _
            "</span>"

        oListItem.Text = strText
        oListItem.Selected = isSelected

        Return oListItem
    End Function

    'Public Sub BindTemplate(Optional ByVal SkinId As Int64 = 0)
    '    Me.ViewState("SkinId") = SkinId
    '    Me.presenter.BindTemplates(SkinId)
    'End Sub




    Public Sub LoadNoItems() Implements lm.Comol.Modules.Standard.Skin.Presentation.IViewSkinEditor.LoadNoItems

    End Sub
End Class


'<style type="text/css">

'    /* div#cFooter */

'    div.template input, div.template label { display: inline-block;}

'    .item
'    {
'        display: inline-block; width: 270px; height: 100px;
'        border: 1px solid black;
'        vertical-align: top;
'        /*margin-left: 1.5em;*/
'        margin-bottom: 1em;
'        }

'     .item * { vertical-align:top; }

'    .item ul { list-style: none; }



'    /*        HEADER         */
'    /* LEFT TOP*/
'    .head_left ul, .head_left a, .head_left img, .head_left h1, .head_left span { margin: 0; padding: 0; border: 0; display: inline-block;}
'    /* RIGHT TOP*/
'    .head_right ul, .head_right a, .head_right img, .head_right h1, .head_right span { margin: 0; padding: 0; border: 0; display: inline-block;}
'    .head_right ul, .head_right a, .head_right img { float: right; text-align: right; }
'    .head_right span, .head_right h1 { float: left; }


'    /*        FOOTER         */
'    /* FLOW */
'    .flow * { display: inline; }

'    /* 50-50 left*/
'    .half_left ul, .half_left span { margin: 0; padding: 0; border: 0; width: 50%; display: inline-block;}

'    /* LEFT */
'    .left ul, .left span { margin: 0; padding: 0; border: 0; display: inline-block;}

'    /* 50-50 right */
'    .half_right ul, .half_left span { margin: 0; padding: 0; border: 0; width: 50%; display: inline-block;}
'    .half_right ul { float: right; text-align: right; }
'    .half_right span { float: left; }

'    /* Right */
'    .right ul, .right span, .right a, .righ img, .right h1 { margin: 0; padding: 0; border: 0; display: inline-block;}
'    .right ul, .right img, .right img { float: right; text-align: right; }
'    .right { text-align: right; }
'    .right span {  }




'    /* Top */
'    .top ul, .top span { margin: 0; padding: 0; border: 0; width: 100%; display: inline-block; }
'    .top li { display: inline-block; }

'    /* Bottom */
'    .bottom ul, .bottom span { margin: 0; padding: 0; border: 0; width: 100%; display: inline-block; clear:both;}
'    .bottom li { display: inline-block; }
'    .bottom ul { position:absolute; bottom:0px; }
'    /*.half_bottom span { float: left; clear:both; }*/
'    .bottom { position: relative; }

'</style>