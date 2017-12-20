Imports lm.Comol.Modules.Standard.Skin

Public Class UC_SkinList
    Inherits BaseControl
    Implements Presentation.iViewSkinList

    Public Event EditSkin(ByVal SkinId As Int64)

    Private TempSkinId As Int64

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(MyBase.Resource) Then
            Me.SetCultureSettings()
        End If

    End Sub

    Public Sub RefreshList()
        Me.presenter.RefreshList(GetView())
    End Sub

    Private Function GetView() As Domain.SkinShareType
        Select Case Me.TBSSkinEdit.SelectedIndex
            Case 0
                Return Domain.SkinShareType.NotAssociate
            Case 1
                Return Domain.SkinShareType.Community
            Case 2
                Return Domain.SkinShareType.Organization
            Case 3
                Return Domain.SkinShareType.Portal
            Case Else
                Return Domain.SkinShareType.All
        End Select
    End Function

    Private _presenter As Presentation.SkinListPresenter

    Private ReadOnly Property presenter As Presentation.SkinListPresenter
        Get
            If IsNothing(_presenter) Then
                Me._presenter = New Presentation.SkinListPresenter(MyBase.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property


    'Private Sub Testbind()
    '    Dim MyTestList As New List(Of Integer)
    '    For i As Integer = 0 To 5
    '        MyTestList.Add(i)
    '    Next

    '    Me.Rpt_List.DataSource = MyTestList
    '    Me.Rpt_List.DataBind()
    'End Sub


#Region "Repeater"
#Region "Databind"

    Private Sub Rpt_List_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles Rpt_List.ItemDataBound


        If (e.Item.ItemType = ListItemType.Header) Then
            'LOCALIZZAZIONE
            With MyBase.Resource
                .setLabel(e.Item.FindControl("Lbl_Name_t"))
                .setLabel(e.Item.FindControl("Lbl_Association_t"))
                .setLabel(e.Item.FindControl("LBskinId_t"))
            End With
        End If

        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim Lkb As LinkButton
            Dim Skin As Domain.DTO.DtoSkinList = e.Item.DataItem
            If IsNothing(Skin) Then
                Exit Sub
            End If

            TempSkinId = Skin.Id

            Dim LBskinId As Label = e.Item.FindControl("LBskinId")
            If Not IsNothing(LBskinId) Then
                LBskinId.Text = Skin.Id.ToString()
            End If

            With MyBase.Resource
                Lkb = e.Item.FindControl("Lkb_Modify")
                .setLinkButton(Lkb, True, True)
                Lkb.CommandArgument = Skin.Id
                Lkb.CommandName = "ModifySkin"

                Lkb = e.Item.FindControl("Lkb_Delete")
                .setLinkButton(Lkb, True, True, False, True)
                Lkb.CommandArgument = Skin.Id
                Lkb.CommandName = "DeleteSkin"


                Lkb = e.Item.FindControl("Lkb_Copy")
                .setLinkButton(Lkb, True, True, False, False)
                Lkb.CommandArgument = Skin.Id
                Lkb.CommandName = "CopySkin"


                Dim lbl As Label = e.Item.FindControl("Lbl_SkinName")
                lbl.Text = Skin.Name

                lbl = e.Item.FindControl("Lbl_Portal")
                Lkb = e.Item.FindControl("Lkb_RemPortal")
                If Skin.IsPortal Then
                    .setLinkButton(Lkb, True, True, False, True)
                    Lkb.CommandArgument = Skin.Id
                    Lkb.CommandName = "RemovePortal"
                    .setLabel(lbl)
                Else
                    lbl.Visible = False
                    Lkb.Visible = False
                End If

            End With

            If Not IsNothing(Skin.Organizations) Then
                Dim Rpt_AssOrgn As System.Web.UI.WebControls.Repeater = e.Item.FindControl("Rpt_AssOrgn")
                Rpt_AssOrgn.DataSource = Skin.Organizations
                'registro il gestore dell'evento 
                AddHandler Rpt_AssOrgn.ItemDataBound, AddressOf Rpt_AssOrgn_ItemDataBound
                AddHandler Rpt_AssOrgn.ItemCommand, AddressOf Rpt_AssOrgn_ItemCommand
                Rpt_AssOrgn.DataBind()
            End If

            If Not IsNothing(Skin.Communities) Then
                Dim Rpt_AssCom As System.Web.UI.WebControls.Repeater = e.Item.FindControl("Rpt_AssCom")
                Rpt_AssCom.DataSource = Skin.Communities
                'registro il gestore dell'evento 
                AddHandler Rpt_AssCom.ItemDataBound, AddressOf Rpt_AssCom_ItemDataBound
                AddHandler Rpt_AssCom.ItemCommand, AddressOf Rpt_AssCom_ItemCommand
                Rpt_AssCom.DataBind()
            End If

        End If

    End Sub

    Private Sub Rpt_AssOrgn_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            With MyBase.Resource
                Dim Organization As Domain.DTO.DtoSkinShareItem = e.Item.DataItem

                Dim Lkb_RemOrgn As LinkButton = e.Item.FindControl("Lkb_RemOrgn")
                .setLinkButton(Lkb_RemOrgn, True, True, False, True)
                Lkb_RemOrgn.CommandName = TempSkinId.ToString()
                Lkb_RemOrgn.CommandArgument = Organization.Id.ToString() 'TempSkinId.ToString() & "," & Organization.Id.ToString()

                Dim lbl As Label = e.Item.FindControl("Lbl_Orgn")
                lbl.Text = Organization.Name

            End With
        End If
    End Sub

    Private Sub Rpt_AssCom_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            With MyBase.Resource
                Dim Community As Domain.DTO.DtoSkinShareItem = e.Item.DataItem

                Dim Lkb_RemCom As LinkButton = e.Item.FindControl("Lkb_RemCom")
                .setLinkButton(Lkb_RemCom, True, True, False, True)
                Lkb_RemCom.CommandName = TempSkinId.ToString()
                Lkb_RemCom.CommandArgument = Community.Id.ToString() 'TempSkinId.ToString() & "," & Community.Id.ToString()

                Dim lbl As Label = e.Item.FindControl("Lbl_Com")
                lbl.Text = Community.Name

            End With
        End If
    End Sub
#End Region
#Region "ItemCommand"

    Private Sub Rpt_List_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles Rpt_List.ItemCommand

        Dim SkinId As Int64 = System.Convert.ToInt64(e.CommandArgument)

        Select Case e.CommandName
            Case "ModifySkin"
                RaiseEvent EditSkin(SkinId)
            Case "DeleteSkin"
                Me.presenter.EraseSkin(SkinId)
                Me.presenter.RefreshList(Me.GetView)
            Case "CopySkin"
                Me.presenter.Copyskin(SkinId)
                Me.TBSSkinEdit.SelectedIndex = 0
                Me.presenter.RefreshList(Me.GetView)
            Case "RemovePortal"
                Me.presenter.RemPortal(SkinId)
                Me.presenter.RefreshList(Me.GetView)

        End Select


    End Sub

    Public Sub Rpt_AssCom_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)

        Dim ObjId As Integer = System.Convert.ToInt32(e.CommandArgument)
        Dim SkinId As Int64 = System.Convert.ToInt64(e.CommandName)

        Me.presenter.RemComAss(SkinId, ObjId)
        Me.presenter.RefreshList(Me.GetView)

    End Sub

    Public Sub Rpt_AssOrgn_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)

        Dim ObjId As Integer = System.Convert.ToInt32(e.CommandArgument)
        Dim SkinId As Int64 = System.Convert.ToInt64(e.CommandName)

        Me.presenter.RemOrgnAss(SkinId, ObjId)

        Me.presenter.RefreshList(Me.GetView)

    End Sub
#End Region
#End Region

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("UC_SkinList", "Skin", "UC")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        If IsNothing(Resource) Then
            Me.SetCultureSettings()
        End If

        With Resource
            For Each Tab As Telerik.Web.UI.RadTab In Me.TBSSkinEdit.Tabs
                .setRadTab(Tab, True)
            Next
            .setLabel(Me.Lbl_NoData)

            .setLabel(Lbl_TestCom_t)
            .setLabel(Lbl_TestOrgn_t)
            .setLabel(Lbl_TestLang_t)

            .setLinkButton(Lkb_TEST, True, True)
            .setLinkButton(Lkb_ClearTest, True, True)

            .setLabel(Lbl_Time_t)
            .setLabel(Lbl_MainLogo_t)
            .setLabel(Lbl_FooterLogos_t)
            .setLabel(Lbl_FooterText_t)
            .setLabel(Lbl_Styles_t)


        End With
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property

    Public Sub BindSkins(ByVal Skins As System.Collections.Generic.IList(Of Domain.DTO.DtoSkinList)) Implements Presentation.iViewSkinList.BindSkins
        If Not IsNothing(Skins) AndAlso Skins.Count > 0 Then
            Me.Rpt_List.Visible = True
            Me.Rpt_List.DataSource = Skins
            Me.Rpt_List.DataBind()
            Me.Lbl_NoData.Visible = False
        Else
            Me.Rpt_List.Visible = False
            Me.Lbl_NoData.Visible = True
        End If
        
    End Sub


    Private Sub TBSSkinEdit_TabClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSSkinEdit.TabClick
        'Momentaneo - eventualmente spostare in UC apposito.

        If Me.TBSSkinEdit.SelectedIndex = 5 Then

            Me.Rpt_List.Visible = False
            Me.Lbl_NoData.Visible = False
            Me.Pnl_Test.Visible = True

        Else

            Me.Rpt_List.Visible = True
            Me.Pnl_Test.Visible = False

            Me.presenter.RefreshList(GetView())

        End If


    End Sub

    Public ReadOnly Property BasePath As String Implements Presentation.iViewSkinList.BasePath
        Get
            Return MyBase.SystemSettings.SkinSettings.SkinPhisicalPath
        End Get
    End Property

    Private Sub Lkb_TEST_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lkb_TEST.Click
        Dim ComId, OrgnId As Integer

        Try
            ComId = System.Convert.ToInt32(Me.Txb_TestComId.Text)
        Catch ex As Exception
            ComId = -1
        End Try

        If ComId <= 0 Then
            Try
                OrgnId = System.Convert.ToInt32(Me.Txb_TestOrgnId.Text)
            Catch ex As Exception
                OrgnId = -1
            End Try
        Else
            Me.Txb_TestOrgnId.Text = ""
            OrgnId = -1
        End If
        

        Dim VirPath As String = MyBase.BaseUrl & MyBase.SystemSettings.SkinSettings.SkinVirtualPath

        Dim LangCode As String = Me.DDL_TestLang.SelectedValue.ToString()


        Dim Css As String = ""
        Dim Inizio As DateTime = Now()
        Dim html As Domain.HTML.HTMLSkin = presenter.Test(ComId, OrgnId, VirPath, LangCode, Me.SystemSettings.DefaultLanguage.Codice, Me.SystemSettings.SkinSettings, Me.BaseUrl)

        'Config SE caricare o meno CSS Organizzazione nel portale


        '"it-IT"

        Css += "<b>Main</b> <br />" + vbCrLf
        Css += System.Web.HttpUtility.HtmlEncode( _
            presenter.TestCss(ComId, _
                OrgnId, _
                VirPath, _
                "it-IT", _
                Business.SkinFileManagement.CssType.Main, _
                Me.BaseAppUrl, SystemSettings.SkinSettings) _
            ).Replace(vbCrLf, " <br />")
        Css += "<b>IE</b> <br />" + vbCrLf
        Css += System.Web.HttpUtility.HtmlEncode( _
            presenter.TestCss( _
                ComId, _
                OrgnId, _
                VirPath, _
                "it-IT", _
                Business.SkinFileManagement.CssType.IE, _
                Me.BaseAppUrl, SystemSettings.SkinSettings) _
            ).Replace(vbCrLf, " <br />")

        Css += "<b>Admin</b> <br />" + vbCrLf
        Css += System.Web.HttpUtility.HtmlEncode( _
            presenter.TestCss( _
                ComId, _
                OrgnId, _
                VirPath, _
                "it-IT", _
                Business.SkinFileManagement.CssType.Admin, _
                Me.BaseAppUrl, SystemSettings.SkinSettings) _
            ).Replace(vbCrLf, " <br />")

        Css += "<b>Login</b> <br />" + vbCrLf
        Css += System.Web.HttpUtility.HtmlEncode( _
            presenter.TestCss( _
                ComId, _
                OrgnId, _
                VirPath, _
                "it-IT", _
                Business.SkinFileManagement.CssType.Login, _
                Me.BaseAppUrl, SystemSettings.SkinSettings) _
            ).Replace(vbCrLf, " <br />")

        Dim Fine As TimeSpan = Now() - Inizio

        Me.Lbl_Time.Text = Fine.ToString()

        Lbl_MainLogo.Text = html.HtmlHeadLogo
        Lbl_FooterText.Text = html.FooterText
        Lit_FooterLogos.Text = ""
        For Each Ftlogo As String In html.HtmlFooterLogos
            Lit_FooterLogos.Text += "<li>" + Ftlogo + "</li>"
        Next

        Lbl_Styles.Text = Css


        ' Preview
        Hyp_Preview.Visible = True

        'SkinDisplayType
        If ComId > 0 Then
            Hyp_Preview.NavigateUrl = Me.BaseUrl & "Modules/SkinManagement/Preview.aspx?itemType=Community&idItem=" & ComId.ToString()
        ElseIf OrgnId > 0 Then
            Hyp_Preview.NavigateUrl = Me.BaseUrl & "Modules/SkinManagement/Preview.aspx?itemType=Organization&idItem=" & OrgnId.ToString()
        Else
            Hyp_Preview.NavigateUrl = Me.BaseUrl & "Modules/SkinManagement/Preview.aspx?itemType=Portal"
        End If
        Hyp_Preview.Text = Resource.getValue("Hyp_Preview.Text")

    End Sub

    Private Sub Lkb_ClearTest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lkb_ClearTest.Click
        presenter.ClearCache()
        Lbl_MainLogo.Text = ""
        Lit_FooterLogos.Text = ""
        Lbl_FooterText.Text = ""
        Lbl_Styles.Text = ""
        Me.Hyp_Preview.Visible = False
    End Sub

    Public ReadOnly Property BaseAppUrl() As String
        Get
            Dim url As String = Me.Request.ApplicationPath
            If url.EndsWith("/") Then
                Return url
            Else
                Return url + "/"
            End If
        End Get
    End Property

    Public Function GetActionTitle() As String
        Return Resource.getValue("Action.Title")
    End Function
End Class