Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class CategoriesTree
    Inherits TicketBase
    Implements TK.Presentation.View.iViewCategoryTree

#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.CategoryTreePresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.CategoryTreePresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.CategoryTreePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Internal"
    'Property interni


#End Region

#Region "Implements"
    'Property della VIEW
    Public Property ViewCommunityId As Integer Implements TK.Presentation.View.iViewCategoryTree.ViewCommunityId
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

#End Region

#Region "Inherits"
    'Property del PageBase

    'Public Overrides ReadOnly Property AlwaysBind As Boolean
    '    Get
    '        Return True
    '    End Get
    'End Property

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
        MyBase.SetCulture("pg_CategoryTree", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTpageTitle_t)
            .setLiteral(LTtitle_t)

            .setLabel(Me.LBmain_t)

            .setHyperLink(HYPtable, True, True)
            HYPtable.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.CategoryList(Me.ViewCommunityId)
            .setHyperLink(HYPtree, True, True)
            HYPtree.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.CategoryListTree(Me.ViewCommunityId)
            .setHyperLink(HYPuser, True, True)
            HYPuser.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.CategoryListTree(Me.ViewCommunityId)

            .setLinkButton(Me.LNBsave, True, True, False, True)

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
        RedirectOnSessionTimeOut(TK.Domain.RootObject.CategoryListTree(CommunityId), CommunityId)
    End Sub

    Public Overrides Sub ShowNoAccess()
        Me.ShowMessage(TK.Domain.Enums.CategoryTreeMessageType.NoAccess)
    End Sub

#End Region

#End Region

#Region "Implements"
    'Sub e function della View
    Public Sub BindTree(TreeItems As System.Collections.Generic.IList(Of TK.Domain.DTO.DTO_CategoryTree)) Implements TK.Presentation.View.iViewCategoryTree.BindTree

        If IsNothing(TreeItems) OrElse TreeItems.Count() <= 0 Then
            ShowMessage(TK.Domain.Enums.CategoryTreeMessageType.NoCategory)

            Exit Sub
        End If

        Me.RPTCategoriesTree.DataSource = TreeItems.OrderBy(Function(x) x.Order).ToList()
        Me.RPTCategoriesTree.DataBind()
    End Sub

    Public Function GetReorderStr() As String Implements TK.Presentation.View.iViewCategoryTree.GetReorderStr
    End Function

    Public Sub ShowMessage(MsgType As TK.Domain.Enums.CategoryTreeMessageType) Implements TK.Presentation.View.iViewCategoryTree.ShowMessage

        Dim Message As String = ""

        Dim ErrType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.info


        Select Case MsgType
            Case TK.Domain.Enums.CategoryTreeMessageType.DefaultReorderWarning
                Message = Resource.getValue("Message.DefaultOrder")
                ErrType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert

            Case TK.Domain.Enums.CategoryTreeMessageType.NoAccess
                Me.LNBsave.Visible = False
                Me.PNLtree.Visible = False

                Me.Master.ShowNoPermission = True
                Me.Master.ServiceNopermission = Resource.getValue("Error.NoAccess")
                Exit Sub

            Case TK.Domain.Enums.CategoryTreeMessageType.NoCategory
                Message = Resource.getValue("Message.NoCategories")
                ErrType = lm.Comol.Core.DomainModel.Helpers.MessageType.info

                Me.LNBsave.Visible = False
                Me.PNLtree.Visible = False

            Case TK.Domain.Enums.CategoryTreeMessageType.NoPermission
                Me.LNBsave.Visible = False
                Me.PNLtree.Visible = False

                Me.Master.ShowNoPermission = True
                Me.Master.ServiceNopermission = Resource.getValue("Error.NoPermission")
                Exit Sub

            Case TK.Domain.Enums.CategoryTreeMessageType.NoReorder
                Message = Resource.getValue("Message.NoModified")
                ErrType = lm.Comol.Core.DomainModel.Helpers.MessageType.info

            Case TK.Domain.Enums.CategoryTreeMessageType.Saved
                Message = Resource.getValue("Message.Saved")
                ErrType = lm.Comol.Core.DomainModel.Helpers.MessageType.success

            Case TK.Domain.Enums.CategoryTreeMessageType.UnSaved
                Message = Resource.getValue("Message.Unsaved")
                ErrType = lm.Comol.Core.DomainModel.Helpers.MessageType.error

            Case lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryTreeMessageType.none
                Message = ""

            Case lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryTreeMessageType.MessageSend
                Message = Resource.getValue("Message.Sended")
                ErrType = lm.Comol.Core.DomainModel.Helpers.MessageType.success

            Case lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryTreeMessageType.MessageUnsend
                Message = Resource.getValue("Message.UnSended")
                ErrType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert

        End Select


        If Not String.IsNullOrEmpty(Message) Then
            CTRLmessages.Visible = True
            CTRLmessages.InitializeControl(Message, ErrType)
        Else
            CTRLmessages.Visible = False
        End If

    End Sub

#End Region

#Region "Internal"
    'Sub e Function "della pagina"

    Private Sub SaveItems(ByVal Refresh As Boolean)
        Dim Items As New List(Of TK.Domain.liteCategoryReorderItem)()

        If Not String.IsNullOrEmpty(Me.HDNreorderedData.Value) Then

            Dim OrderValue As Integer = 1
            Dim RowItems As String() = Me.HDNreorderedData.Value.Split(";")

            For Each itm As String In RowItems
                Dim lcr As New TK.Domain.liteCategoryReorderItem()
                Dim values As String() = itm.Split(":")
                Try
                    lcr.Id = System.Convert.ToInt64(values(0).Replace("srt-", ""))
                    lcr.Order = OrderValue
                    If values(1) = "0" Then
                        lcr.FatherId = Nothing
                    Else
                        lcr.FatherId = System.Convert.ToInt64(values(1).Replace("srt-", ""))
                    End If

                    OrderValue = OrderValue + 1
                    Items.Add(lcr)
                Catch ex As Exception

                End Try

            Next
        ElseIf Refresh Then
            Me.CurrentPresenter.InitView()
        End If

        If Not IsNothing(Items) AndAlso Items.Count() > 0 Then
            Me.CurrentPresenter.Save(Items, Refresh)
        Else
            Dim Message As String = Resource.getValue("Message.NoModified")
            Dim ErrType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.info

            If Not String.IsNullOrEmpty(Message) Then
                CTRLmessages.Visible = True
                CTRLmessages.InitializeControl(Message, ErrType)
            End If
        End If
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
    Private Sub RPTCategoriesTree_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTCategoriesTree.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim UCTreeItem As UC_CategoryTreeItem = e.Item.FindControl("UCTreeItem")
            Dim cat As TK.Domain.DTO.DTO_CategoryTree = DirectCast(e.Item.DataItem, TK.Domain.DTO.DTO_CategoryTree)

            If Not IsNothing(UCTreeItem) AndAlso Not IsNothing(cat) Then
                UCTreeItem.BindCategoryItem(cat)
            End If

        End If
    End Sub

    Private Sub LNBsave_Click(sender As Object, e As System.EventArgs) Handles LNBsave.Click
        SaveItems(True)
    End Sub
#End Region

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