Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class CategoriesList
    Inherits TicketBase
    Implements TK.Presentation.View.iViewCategoriesList
    
#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.CategoriesListPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.CategoriesListPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.CategoriesListPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Internal"
    'Property interni

    Public ReadOnly Property DeleteTitle As String
        Get
            Return Resource.getValue("Modal.Category.Delete")
        End Get
    End Property

    Public ReadOnly Property InfoTitle As String
        Get
            Return Resource.getValue("Modal.Category.Description")
        End Get
    End Property

    Private _HasWarning = False

#End Region

#Region "Implements"
    'Property della VIEW
    Public Property ViewCommunityId As Integer Implements TK.Presentation.View.iViewCategoriesList.ViewCommunityId
        Get
            Dim ComId As Integer = 0
            Try
                ComId = ViewStateOrDefault("CurrentComId", -1)
            Catch ex As Exception
            End Try

            If ComId < 0 Then
                Try
                    ComId = System.Convert.ToInt32(Request.QueryString("CommunityId"))
                Catch ex As Exception
                End Try
            End If

            Return ComId
        End Get
        Set(value As Integer)
            Me.ViewState("CurrentComId") = value
        End Set
    End Property

    Public WriteOnly Property CommunityName As String Implements TK.Presentation.View.iViewCategoriesList.CommunityName
        Set(value As String)

            Resource.setLiteral(LTtitle_t)

            If (String.IsNullOrEmpty(value)) Then
                Return
            End If

            If value = TK.TicketService.ComPortalName Then
                Me.LTtitle_t.Text &= " - " & value
            End If

            Me.LTtitle_t.Text &= " - " & value

        End Set
    End Property

#End Region

#Region "Inherits"
    'Property del PageBase

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    'Public Overrides ReadOnly Property VerifyAuthentication As Boolean
    '    Get
    '        Return False
    '    End Get
    'End Property

#End Region


    Private Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CTRLheadMessages.Visible = False
    End Sub

#Region "Inherits"
    'Sub e Function PageBase

    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CategoryList", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTpageTitle_t)
            .setLiteral(LTtitle_t)
            .setLiteral(LTthName)
            .setLiteral(LTthResource)
            .setLiteral(LTthActions)

            .setLiteral(LTnoData_t)

            .setHyperLink(HYPtable, True, True)
            HYPtable.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.CategoryList(Me.ViewCommunityId)
            .setHyperLink(HYPtree, True, True)
            HYPtree.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.CategoryListTree(Me.ViewCommunityId)
            .setHyperLink(HYPuser, True, True)
            HYPuser.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.CategoryListTree(Me.ViewCommunityId)

            .setHyperLink(HYPnew, True, True)
            HYPnew.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.CategoryAdd(Me.ViewCommunityId)

            'Legend
            .setLiteral(LTleged_t)

            .setLinkButton(LNBnotifyAll, True, True, False, False)
            .setLinkButton(LNBnotifyManager, True, True, False, False)
            .setLinkButton(LNBnotifyResolver, True, True, False, False)

        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#Region "TicketBase"

    Public Overrides Sub DisplaySessionTimeout(CommunityId As Integer)
        RedirectOnSessionTimeOut(TK.Domain.RootObject.CategoryList(CommunityId), CommunityId)
    End Sub

    Public Overrides Sub ShowNoAccess()
        Me.Master.ShowNoPermission = True
        Me.Master.ServiceNopermission = Resource.getValue("Error.NoAccess")
    End Sub

#End Region

#End Region

#Region "Implements"

    Private _ShowButton As Boolean = False
    Private _DefCateId As Int64 = 0

    'Sub e function della View
    Public Sub BindList(Categories As System.Collections.Generic.IList(Of TK.Domain.DTO.DTO_CategoryList), ByVal IsManager As Boolean, ByVal DefCateId As Int64) Implements TK.Presentation.View.iViewCategoriesList.BindList

        _ShowButton = IsManager
        _DefCateId = DefCateId

        Me.HYPtree.Enabled = Me.HYPtree.Enabled AndAlso IsManager
        Me.HYPuser.Enabled = Me.HYPuser.Enabled AndAlso IsManager

        If (IsNothing(Categories) OrElse Categories.Count() <= 0) Then
            Me.PH_noData.Visible = True
        Else
            Me.PH_noData.Visible = False
        End If

        Me.RPTcategoryItem.DataSource = Categories.ToList() '.OrderBy(Function(x) x.Order)
        Me.RPTcategoryItem.DataBind()

        If (Me._HasWarning) Then
            ShowWarning()
        End If

    End Sub

    Public Sub ShowNoPermission() Implements TK.Presentation.View.iViewCategoriesList.ShowNoPermission
        Me.Master.ShowNoPermission = True
        Me.Master.ServiceNopermission = Resource.getValue("Error.NoPermission")
        Me.PNLContent.Visible = False
    End Sub

    'Public Sub DisplaySessionTimeout() Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewCategoriesList.DisplaySessionTimeout
    '    Dim idCommunity As Integer = Me.CurrentPresenter.CurrentCommunityId
    '    Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
    '    Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
    '    dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

    '    dto.DestinationUrl = TK.Domain.RootObject.CategoryList(idCommunity)
    '    ' "" 'RootObject.Gantt(IdProject, PreloadIdCommunity, PreloadFromPage, PreloadIdContainerCommunity)

    '    If idCommunity > 0 Then
    '        dto.IdCommunity = idCommunity
    '    End If

    '    webPost.Redirect(dto)
    'End Sub


    'Public Sub SendAction( _
    '                     Action As TK.ModuleTicket.ActionType, _
    '                     idCommunity As Integer, _
    '                     Type As TK.ModuleTicket.InteractionType, _
    '                    Optional Objects As System.Collections.Generic.IList(Of System.Collections.Generic.KeyValuePair(Of Integer, String)) = Nothing) _
    '                 Implements TK.Presentation.View.iViewCategoriesList.SendAction

    '    Dim oList As List(Of WS_Actions.ObjectAction) = Nothing

    '    If Not IsNothing(Objects) Then
    '        oList = (From kvp As KeyValuePair(Of Integer, String) In Objects
    '                Select Me.PageUtility.CreateObjectAction(kvp.Key, kvp.Value)).ToList()
    '    End If

    '    Me.PageUtility.AddActionToModule(idCommunity, Me.CurrentModuleID, Action, oList, Type)
    'End Sub
#End Region

#Region "Internal"
    'Sub e Function "della pagina"
    Private Sub ShowWarning()
        Me.LTwarningItem.Visible = True
        Me.LTwarningItem.Text = Me.LTwarningItem.Text.Replace("{title}", Resource.getValue("Legend.Warning.Title")).Replace("{text}", Resource.getValue("Legend.Warning.Text"))
    End Sub

    Public Function GetLegendTitle(ByVal value As String) As String
        Dim out As String = Resource.getValue("Legend." & value & ".Title")
        If String.IsNullOrEmpty(out) Then
            Return value
        End If
        Return out
    End Function

    Public Function GetLegendText(ByVal value As String) As String
        Dim out As String = Resource.getValue("Legend." & value & ".Text")
        If String.IsNullOrEmpty(out) Then
            Return value
        End If
        Return out
    End Function

#End Region

#Region "Handler"
    'Gestione eventi oggetti pagina (Repeater.ItemDataBound(), Button.Click, ...)
    Private Sub RptCategoryItem_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcategoryItem.ItemCommand
        Dim Id As Int64 = System.Convert.ToInt64(e.CommandArgument)
        If (Id > 0) Then
            Select Case e.CommandName
                Case "Delete"
                    Me.CTRLdelCate.IsVisible = True
                    Me.CTRLdelCate.InitControl(Id, Me.ViewCommunityId, Me.CurrentModuleID)
                Case "UnDelete"
                    CurrentPresenter.RecoverCategory(Id)
            End Select
        End If
    End Sub

    Private Sub RptCategoryItem_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcategoryItem.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim LTtdBegin As Literal = e.Item.FindControl("LTtdBegin")

            Dim dtoCat As TK.Domain.DTO.DTO_CategoryList = DirectCast(e.Item.DataItem, TK.Domain.DTO.DTO_CategoryList)

            Dim ItemCss As String = ""

            If (dtoCat.IsDeleted) Then
                ItemCss = " deleted"
            End If

            If (dtoCat.Id = _DefCateId) Then
                ItemCss &= " default"
            End If

            If Not IsNothing(dtoCat) Then

                If (dtoCat.ManagerNum <= 0) Then
                    ItemCss &= " nomanager"
                    Me._HasWarning = True
                End If


                If dtoCat.FatherId <= 0 Then    'OrElse dtoCat.IsDeleted = True
                    LTtdBegin.Text = LTtdBegin.Text.Replace("{1}", ItemCss)
                Else
                    LTtdBegin.Text = LTtdBegin.Text.Replace("{1}", "child-of-ctg-" & dtoCat.FatherId.ToString() & ItemCss)
                End If

                Dim LTcatName As Literal = e.Item.FindControl("LTcatName")
                If Not IsNothing(LTcatName) Then
                    LTcatName.Text = dtoCat.Name
                End If

                Dim LTcatDesc As Literal = e.Item.FindControl("LTcatDesc")
                If Not IsNothing(LTcatDesc) Then
                    LTcatDesc.Text = dtoCat.Description
                End If

                Dim PHshowInfo As PlaceHolder = e.Item.FindControl("PHshowInfo")
                If Not IsNothing(PHshowInfo) Then
                    If String.IsNullOrEmpty(dtoCat.Description) Then
                        PHshowInfo.Visible = False
                    Else
                        PHshowInfo.Visible = True
                    End If
                End If

                LTtdBegin.Text = LTtdBegin.Text.Replace("{0}", dtoCat.Id.ToString())

                'LTManNumber
                Dim LTManNumber As Literal = e.Item.FindControl("LTManNumber")
                If Not IsNothing(LTManNumber) Then
                    LTManNumber.Text = dtoCat.ManagerNum.ToString()
                End If

                'LTResNumber
                Dim LTResNumber As Literal = e.Item.FindControl("LTResNumber")
                If Not IsNothing(LTResNumber) Then
                    LTResNumber.Text = dtoCat.ResolverNum.ToString()
                End If

                'Internazionalizzazioni
                Dim LTMan_t As Literal = e.Item.FindControl("LTMan_t")
                If Not IsNothing(LTMan_t) Then
                    Resource.setLiteral(LTMan_t)
                End If

                'Internazionalizzazioni
                Dim LTRes_t As Literal = e.Item.FindControl("LTRes_t")
                If Not IsNothing(LTRes_t) Then
                    Resource.setLiteral(LTRes_t)
                End If

                Dim HYPedit As HyperLink = e.Item.FindControl("HYPedit")
                If Not IsNothing(HYPedit) Then
                    Resource.setHyperLink(HYPedit, True, True)
                    HYPedit.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.CategoryModify(Me.ViewCommunityId, dtoCat.Id)
                End If
                Dim HYPpreview As HyperLink = e.Item.FindControl("HYPpreview")
                If Not IsNothing(HYPpreview) Then
                    Resource.setHyperLink(HYPpreview, True, True)
                End If
                Dim LNBdelete As LinkButton = e.Item.FindControl("LNBdelete")
                If Not IsNothing(LNBdelete) Then
                    If (_ShowButton AndAlso Not dtoCat.IsDeleted AndAlso Not dtoCat.Id = _DefCateId) Then
                        Resource.setLinkButton(LNBdelete, True, True, False, False)
                        LNBdelete.CommandName = "Delete"
                        LNBdelete.CommandArgument = dtoCat.Id.ToString()
                        LNBdelete.Enabled = True
                        LNBdelete.Visible = True
                    Else
                        LNBdelete.Enabled = False
                        LNBdelete.Visible = False
                    End If
                End If

                Dim LNBundelete As LinkButton = e.Item.FindControl("LNBundelete")
                If Not IsNothing(LNBundelete) Then
                    If (_ShowButton AndAlso dtoCat.IsDeleted) Then
                        Resource.setLinkButton(LNBundelete, True, True, False, True)
                        LNBundelete.CommandName = "UnDelete"
                        LNBundelete.CommandArgument = dtoCat.Id.ToString()
                        LNBundelete.Enabled = True
                        LNBundelete.Visible = True
                    Else
                        LNBundelete.Enabled = False
                        LNBundelete.Visible = False

                    End If

                End If

            End If

        End If


    End Sub

    Private Sub CTRLdelCate_EndWizard() Handles CTRLdelCate.EndWizard

        Me.CurrentPresenter.InitView()
    End Sub

    Private Sub CTRLdelCate_GoToTree() Handles CTRLdelCate.GoToTree
        Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.CategoryListTree(Me.ViewCommunityId))
    End Sub

#End Region


    
    Public Sub ShowSendInfo(sended As Boolean) Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewCategoriesList.ShowSendInfo
        Me.CTRLheadMessages.Visible = True

        If sended Then
            Me.CTRLheadMessages.InitializeControl( _
            Resource.getValue("Message.Send"), _
            lm.Comol.Core.DomainModel.Helpers.MessageType.success)
        Else
            Me.CTRLheadMessages.InitializeControl( _
           Resource.getValue("Message.NotSend"), _
           lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
        End If
        
    End Sub

    Private Sub LNBnotifyAll_Click(sender As Object, e As EventArgs) Handles LNBnotifyAll.Click
        Me.CurrentPresenter.SendNotificationALL()
    End Sub

    Private Sub LNBnotifyManager_Click(sender As Object, e As EventArgs) Handles LNBnotifyManager.Click
        Me.CurrentPresenter.SendNotificationManagers()
    End Sub

    Private Sub LNBnotifyResolver_Click(sender As Object, e As EventArgs) Handles LNBnotifyResolver.Click
        Me.CurrentPresenter.SendNotificationResolvers()
    End Sub
End Class