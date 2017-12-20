Imports lm.Comol.Core.BaseModules.Tickets.Presentation.View
Imports lm.Comol.Core.BaseModules.Tickets.Presentation
Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class Category
    Inherits TicketBase 'PageBase
    Implements iViewCategory
    
#Region "Context"
    'Definizion Presenter...
    Private _presenter As CategoryPresenter
    Private ReadOnly Property CurrentPresenter() As CategoryPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New CategoryPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Internal"
    'Property interne

    Private UsersNumber As Integer = 0


    Private Property CurrentTranslations As List(Of TK.Domain.CategoryTranslation)
        Get
            Return ViewStateOrDefault("CurrentTranslations", New List(Of TK.Domain.CategoryTranslation))
        End Get
        Set(value As List(Of TK.Domain.CategoryTranslation))
            ViewState("CurrentTranslations") = value
        End Set
    End Property

    Public ReadOnly Property InternalUserAddTitle As String
        Get
            Return Resource.getValue("Modal.InternalUsers.Title")
        End Get
    End Property

#End Region

#Region "Implements"
    'Property della VIEW

    Public Property Category As lm.Comol.Core.BaseModules.Tickets.Domain.Category Implements iViewCategory.Category
        Get
            Dim Cat As New lm.Comol.Core.BaseModules.Tickets.Domain.Category()

            Cat.Id = Me.CurrentCategoryId
            Cat.Name = Me.TXBname.Text
            Cat.Description = Me.TXBdescription.Text

            Dim CurrentType As Int32 = System.Convert.ToInt32(TK.Domain.Enums.CategoryType.Current)

            Try
                CurrentType = System.Convert.ToInt32(Me.DDLtype.SelectedValue)
            Catch ex As Exception

            End Try

            Cat.Type = CType(CurrentType, TK.Domain.Enums.CategoryType)

            Return Cat
        End Get
        Set(value As lm.Comol.Core.BaseModules.Tickets.Domain.Category)
            If Not IsNothing(value) AndAlso value.Id > 0 Then

                'Sostituire con localizzazioni varie...
                TXBname.Text = value.Name
                TXBdescription.Text = value.Description

                Me.DDLtype.SelectedValue = value.Type
            End If
        End Set
    End Property

    Public ReadOnly Property FathersId As Long Implements iViewCategory.FathersId
        Get

        End Get
    End Property

    Public ReadOnly Property HasFather As Boolean Implements iViewCategory.HasFather
        Get

        End Get
    End Property

    Public Property CurrentCategoryId As Long Implements TK.Presentation.View.iViewCategory.CurrentCategoryId
        Get
            Dim Id As Int64 = 0
            If IsNothing(Me.ViewState("PreLoadCategoryId")) Then
                Try
                    Id = Convert.ToInt64(Request.QueryString("Id"))

                    If (Id > 0) Then
                        Me.ViewState("PreLoadCategoryId") = Id
                    End If
                Catch ex As Exception

                End Try
            Else
                Id = Convert.ToInt64(Me.ViewState("PreLoadCategoryId"))
            End If

            Return Id
        End Get
        Set(value As Long)
            Me.ViewState("PreLoadCategoryId") = value
        End Set
    End Property

    Public WriteOnly Property AssociatedUsers As System.Collections.Generic.IList(Of TK.Domain.DTO.DTO_UserRole) Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewCategory.AssociatedUsers
        Set(value As System.Collections.Generic.IList(Of TK.Domain.DTO.DTO_UserRole))

            DVselectUsers.Visible = False

            Dim selectedIds As List(Of Integer) = (From usr As TK.Domain.DTO.DTO_UserRole In value Where Not IsNothing(usr) AndAlso Not IsNothing(usr.User.Person) AndAlso usr.User.Person.Id > 0 Select usr.User.Person.Id).ToList()

            Dim ComId As Integer = CurrentPresenter.CurrentCommunityId

            If (ComId > 0) Then
                Me.CTRLselectUsers.InitializeControl( _
                lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, _
                True, ComId, selectedIds, Nothing, _
                Resource.getValue("Modal.InternalUsers.Description"))
            Else
                Me.CTRLselectUsers.InitializeControl( _
                    lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.SystemUsers, _
                    True, False, selectedIds, Nothing, Resource.getValue("Modal.InternalUsers.Description"))
            End If

            UsersNumber = value.Count

            Me.RPTUsers.DataSource = value
            Me.RPTUsers.DataBind()
        End Set
    End Property

    Public ReadOnly Property CurrentLanguage As lm.Comol.Core.DomainModel.Languages.LanguageItem Implements TK.Presentation.View.iViewCategory.CurrentLanguage
        Get
            Return CTRLlanguageSelector.SelectedItem
        End Get
    End Property

    Public Property PreviousLanguage As lm.Comol.Core.DomainModel.Languages.LanguageItem Implements TK.Presentation.View.iViewCategory.PreviousLanguage
        Get
            Return DirectCast(Me.ViewState("CurrentLanguage"), lm.Comol.Core.DomainModel.Languages.LanguageItem)
        End Get
        Set(value As lm.Comol.Core.DomainModel.Languages.LanguageItem)
            Me.ViewState("CurrentLanguage") = value
        End Set
    End Property

    Public Property ShowDeleteManagers As Boolean Implements TK.Presentation.View.iViewCategory.ShowDeleteManagers
        Get
            Dim out As Boolean = True

            Try
                out = System.Convert.ToBoolean(Me.ViewState("ShowDelManager"))
            Catch
            End Try

            Return out
        End Get
        Set(value As Boolean)
            Me.ViewState("ShowDelManager") = value
        End Set
    End Property

    Public ReadOnly Property UsersSettings As System.Collections.Generic.IDictionary(Of Long, Boolean) Implements TK.Presentation.View.iViewCategory.UsersSettings
        Get
            Dim Roles As New Dictionary(Of Long, Boolean)()

            For Each item As RepeaterItem In Me.RPTUsers.Items
                If item.ItemType = ListItemType.Item OrElse item.ItemType = ListItemType.AlternatingItem Then
                    Dim IsManager As Boolean = False
                    Dim UsrId As Int64 = 0
                    Dim HDFid As HiddenField = item.FindControl("HDFid")
                    If Not IsNothing(HDFid) Then
                        UsrId = System.Convert.ToInt64(HDFid.Value)
                    End If

                    Dim RBLrole As RadioButtonList = item.FindControl("RBLrole")
                    If Not IsNothing(RBLrole) Then
                        If (RBLrole.SelectedValue = "1") Then
                            IsManager = True
                        End If
                    End If

                    Try
                        Roles.Add(UsrId, IsManager)
                    Catch ex As Exception

                    End Try
                End If
            Next

            Return Roles

        End Get
    End Property

    Public Property ViewCommunityId As Integer Implements TK.Presentation.View.iViewCategory.ViewCommunityId
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
        CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub


    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Category", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTpageTitle_t)
            .setLiteral(LTtitle_t)

            .setLabel(LBname_t)
            .setLabel(LBdescription_t)
            .setLabel(LBusers_t)
            .setLabel(LBtype_t)

            .setLinkButton(LNBaddUser, True, True)
            .setLinkButton(LNBadd, True, True)
            .setLinkButton(LNBmodify, True, True)
            .setLinkButton(LNBtoList, True, True)
            .setLinkButton(LNBdelLang, True, True, False, True)

            .setDropDownList(DDLtype, "1")
            .setDropDownList(DDLtype, "2")
            .setDropDownList(DDLtype, "3")

            '.setLiteral(LTintUsrRoles_t)
            .setLabel(LBintUserRoles_t)

            .setRadioButtonList(RBLroles, "1")
            .setRadioButtonList(RBLroles, "0")



            .setLinkButton(LNBnotifyAll, True, True, False, False)
            .setLinkButton(LNBnotifyManager, True, True, False, False)
            .setLinkButton(LNBnotifyResolver, True, True, False, False)


        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#Region "TicketBase"

    Public Overrides Sub DisplaySessionTimeout(CommunityId As Integer)
        RedirectOnSessionTimeOut(TK.Domain.RootObject.CategoryModify(CommunityId, Me.CurrentCategoryId), CommunityId)
    End Sub

    Public Overrides Sub ShowNoAccess()
        Me.Master.ShowNoPermission = True
        Me.Master.ServiceNopermission = Resource.getValue("Error.NoAccess")
    End Sub

#End Region
#End Region

#Region "Implements"
    'Sub e function della View
    Public Sub InitCategories(EnabledCategories As TK.Domain.DTO.DTO_CategoryTypeComPermission, _
                              CurrentType As TK.Domain.Enums.CategoryType,
                              IsDefault As Boolean) Implements TK.Presentation.View.iViewCategory.InitCategories

        Me.InitDDLType(EnabledCategories, CurrentType, IsDefault)

    End Sub

    Public Sub BindLanguages(availableLanguages As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.Languages.BaseLanguageItem), Languages As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.Languages.LanguageItem), current As lm.Comol.Core.DomainModel.Languages.LanguageItem) Implements TK.Presentation.View.iViewCategory.BindLanguages

        Me.CTRLlanguageSelector.InitializeControl(availableLanguages, Languages, current)

        If Not IsNothing(current) Then
            Me.LBnameLanguage.Text = current.ShortCode
            Me.LBdescriptionLang.Text = current.ShortCode

            If (current.Code.ToLower() = "multi") Then
                LNBdelLang.Visible = False
            Else
                LNBdelLang.Visible = True
            End If
        Else
            Me.LBnameLanguage.Text = Resource.getValue("language.multi")
            Me.LBdescriptionLang.Text = Resource.getValue("language.multi")

            LNBdelLang.Visible = False
        End If

    End Sub

    Public Function Validate() As Boolean Implements TK.Presentation.View.iViewCategory.Validate
        If String.IsNullOrEmpty(Me.TXBname.Text) Then
            Me.LTnameError.Visible = True
            Return False
        End If

        Me.LTnameError.Visible = False
        Return True
    End Function

    Public Sub RedirectToList() Implements TK.Presentation.View.iViewCategory.RedirectToList
        Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.CategoryList(Me.ViewCommunityId))
    End Sub

    Public Sub ShowNoPermission() Implements TK.Presentation.View.iViewCategory.ShowNoPermission

        Me.PNLcontent.Visible = False

        Me.Master.ShowNoPermission = True
        Me.Master.ServiceNopermission = Resource.getValue("Error.NoAccess")

    End Sub

    Public Sub ShowWrongCategory() Implements TK.Presentation.View.iViewCategory.ShowWrongCategory

        Me.Master.ShowNoPermission = True
        Me.Master.ServiceNopermission = Resource.getValue("Error.WrongCategory")

        Me.PNLcontent.Visible = False
    End Sub

    Public Sub ShowForcedReassigned(response As TK.Domain.DTO.DTO_CategoryCheckResponse) Implements iViewCategory.ShowForcedReassigned

        If IsNothing(response) Then
            Me.DVmessages.Visible = False
            Me.DVmessages.Attributes.Add("class", "fieldswrapper fullwidth hide")
            Me.CTRLmessagesInfo.Visible = False

        Else
            Me.DVmessages.Visible = True
            Me.DVmessages.Attributes.Add("class", "fieldswrapper fullwidth")
            Dim message As String

            If response.IsCurrentUser Then
                message = Resource.getValue("ForceReassign.ToCurrent")
                If String.IsNullOrEmpty(message) Then
                    message = "Current user assign as Manager."
                End If
            Else
                message = Resource.getValue("ForceReassign.ToResolver")
                If String.IsNullOrEmpty(message) Then
                    message = "{user} assigned as Manager."
                End If
                message = message.Replace("{user}", response.UserDisplayName)
            End If

            CTRLmessagesInfo.InitializeControl(message, lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
            Me.CTRLmessagesInfo.Visible = True
        End If

    End Sub

    Public Sub ShowReassignError(AssignError As TK.Domain.Enums.CategoryAssignersError) Implements iViewCategory.ShowReassignError
        If AssignError = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryAssignersError.none Then

            Me.DVmessages.Visible = False
            Me.DVmessages.Attributes.Add("class", "fieldswrapper fullwidth hide")
            Me.CTRLmessagesInfo.Visible = False

        Else

            Me.DVmessages.Visible = True
            Me.DVmessages.Attributes.Add("class", "fieldswrapper fullwidth")

            Dim message As String = Resource.getValue("Reassign.NoManager")

            CTRLmessagesInfo.InitializeControl(message, lm.Comol.Core.DomainModel.Helpers.MessageType.error)
            Me.CTRLmessagesInfo.Visible = True

        End If

    End Sub

#End Region

#Region "Internal"
    'Sub e Function "della pagina"

    Private Sub InitDDLType( _
                           ByVal Permission As TK.Domain.DTO.DTO_CategoryTypeComPermission, _
                           ByVal CurrentType As TK.Domain.Enums.CategoryType, _
                           IsDefault As Boolean)

        If Not Permission.CanCreate Then
            Me.PNLcontent.Visible = False
            Me.ShowNoPermission()
            Exit Sub
        End If

        Me.DDLtype.Items.Clear()

        Dim HasCurrent As Boolean = False

        If Permission.CanPublic OrElse IsDefault Then
            Me.DDLtype.Items.Add(GetLiType(TK.Domain.Enums.CategoryType.Public, CurrentType, True))
            HasCurrent = HasCurrent OrElse TK.Domain.Enums.CategoryType.Public = CurrentType
        End If

        If Permission.CanPrivate Then
            Me.DDLtype.Items.Add(GetLiType(TK.Domain.Enums.CategoryType.Current, CurrentType, True))
            HasCurrent = HasCurrent OrElse TK.Domain.Enums.CategoryType.Current = CurrentType
        End If

        If Permission.CanTicket Then
            Me.DDLtype.Items.Add(GetLiType(TK.Domain.Enums.CategoryType.Ticket, CurrentType, True))
            HasCurrent = HasCurrent OrElse TK.Domain.Enums.CategoryType.Ticket = CurrentType
        End If

        If Not HasCurrent Then
            Me.DDLtype.Items.Add(GetLiType(CurrentType, CurrentType, False))
        End If

        If (DDLtype.Items.Count() <= 1) Then
            DDLtype.Enabled = False
        End If

        If IsDefault Then
            Me.DDLtype.SelectedValue = TK.Domain.Enums.CategoryType.Public
            Me.DDLtype.Enabled = False
        End If

    End Sub

    Private Function GetLiType( _
                              ByVal Type As TK.Domain.Enums.CategoryType, _
                              ByVal Current As TK.Domain.Enums.CategoryType, _
                              ByVal Selectable As Boolean) As ListItem
        Dim item As New ListItem(Resource.getValue("DDLtype." & Type.ToString()), _
                                     System.Convert.ToInt32(Type))
        If (Current = Type) Then
            item.Selected = True
        Else
            item.Selected = False
        End If

        If Selectable Then
            item.Enabled = Selectable
        Else
            item.Enabled = False
            item.Attributes.Add("disabled", "disabled")

        End If

        Return item
    End Function
#End Region

#Region "Handler"
    'Gestione eventi oggetti pagina (Repeater.ItemDataBound(), Button.Click, ...)
    Private Sub RPTUsers_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTUsers.ItemCommand
        Dim Id As Int64 = System.Convert.ToInt64(e.CommandArgument)

        Select Case e.CommandName
            Case "delete"
                Me.CurrentPresenter.UserRemove(Id)
            Case "sendNotification"
                Me.CurrentPresenter.SendNotificationUser(Id)
        End Select
    End Sub

    Private Sub RPTUsers_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTUsers.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim LBuserName_t, LBrole_t, LBaction_t As Label
            LBuserName_t = e.Item.FindControl("LBuserName_t")
            LBrole_t = e.Item.FindControl("LBrole_t")
            LBaction_t = e.Item.FindControl("LBaction_t")

            If Not IsNothing(LBuserName_t) Then
                Resource.setLabel(LBuserName_t)
            End If

            If Not IsNothing(LBrole_t) Then
                Resource.setLabel(LBrole_t)
            End If

            If Not IsNothing(LBaction_t) Then
                Resource.setLabel(LBaction_t)
            End If

        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim User As TK.Domain.DTO.DTO_UserRole = e.Item.DataItem

            If Not IsNothing(User) AndAlso Not IsNothing(User.User) Then


                Dim HDFid As HiddenField = e.Item.FindControl("HDFid")
                If Not IsNothing(HDFid) Then
                    HDFid.Value = User.User.Id
                End If

                Dim LBuserName As Label = e.Item.FindControl("LBuserName")
                If Not IsNothing(LBuserName) Then
                    If Not IsNothing(User.User.Person) Then
                        LBuserName.Text = User.User.Person.SurnameAndName
                    Else
                        LBuserName.Text = User.User.DisplayName
                    End If
                End If

                Dim RBLrole As RadioButtonList = e.Item.FindControl("RBLrole")
                If Not IsNothing(RBLrole) Then
                    Resource.setRadioButtonList(RBLrole, "1")
                    Resource.setRadioButtonList(RBLrole, "0")

                    If User.IsManager Then
                        RBLrole.SelectedValue = "1"
                    Else
                        RBLrole.SelectedValue = "0"
                    End If

                    If (Not Me.ShowDeleteManagers And User.IsManager) Then
                        RBLrole.Enabled = False
                    Else
                        RBLrole.Enabled = True
                    End If

                End If

                Dim LNBdelete As LinkButton = e.Item.FindControl("LNBdelete")
                If Not IsNothing(LNBdelete) Then
                    If (Not Me.ShowDeleteManagers And User.IsManager) Then
                        LNBdelete.Visible = False
                    Else
                        LNBdelete.Visible = True
                        Resource.setLinkButton(LNBdelete, True, True)
                        LNBdelete.CommandName = "delete"
                        LNBdelete.CommandArgument = User.User.Id
                    End If
                End If

                Dim LNBsend As LinkButton = e.Item.FindControl("LNBsend")
                If Not IsNothing(LNBsend) Then
                    Resource.setLinkButton(LNBsend, True, True)
                    LNBsend.CommandName = "sendNotification"
                    LNBsend.CommandArgument = User.User.Id
                End If
            End If
        ElseIf e.Item.ItemType = ListItemType.Footer Then

            Dim PNLfooter As Panel = e.Item.FindControl("PNLfooter")

            If Not IsNothing(PNLfooter) Then


                If Me.UsersNumber > 5 Then
                    Dim LTfootShow, LTfootHide As Literal
                    LTfootShow = e.Item.FindControl("LTfootShow")
                    LTfootHide = e.Item.FindControl("LTfootHide")

                    If Not IsNothing(LTfootShow) Then
                        LTfootShow.Text = Resource.getValue("TBLfooter.ShowAll").Replace("{allItem}", Me.UsersNumber.ToString())
                    End If

                    If Not IsNothing(LTfootHide) Then
                        LTfootHide.Text = Resource.getValue("TBLfooter.HideAll") '.Replace("{remainItem}", (Me.UsersNumber - 5).ToString())
                    End If
                Else
                    PNLfooter.Visible = False
                End If
            End If


        End If
    End Sub

    Private Sub LNBaddUser_Click(sender As Object, e As System.EventArgs) Handles LNBaddUser.Click

        DVselectUsers.Visible = True

    End Sub

    Private Sub LNBtoList_Click(sender As Object, e As System.EventArgs) Handles LNBtoList.Click
        Me.CurrentPresenter.Save(False)
        RedirectToList()
    End Sub

    Private Sub LNBmodify_Click(sender As Object, e As System.EventArgs) Handles LNBmodify.Click
        Me.CurrentPresenter.Save(True)
    End Sub

    Private Sub CTRLlanguageSelector_LanguageAdded(l As lm.Comol.Core.DomainModel.Languages.BaseLanguageItem) Handles CTRLlanguageSelector.LanguageAdded
        Me.CurrentPresenter.LanguageAdd(l)
    End Sub

    Private Sub LNBadd_Click(sender As Object, e As System.EventArgs) Handles LNBadd.Click
        Me.CurrentPresenter.Save(False)
        Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.CategoryAdd(Me.ViewCommunityId))
    End Sub

    Private Sub CTRLlanguageSelector_SelectedLanguage(l As lm.Comol.Core.DomainModel.Languages.LanguageItem) Handles CTRLlanguageSelector.SelectedLanguage
        Me.CurrentPresenter.Save(True)
    End Sub

    Private Sub LNBdelLang_Click(sender As Object, e As System.EventArgs) Handles LNBdelLang.Click
        Me.CurrentPresenter.LanguageDelete()
    End Sub


    Private Sub CTRLselectUsers_CloseWindow() Handles CTRLselectUsers.CloseWindow
        DVselectUsers.Visible = False
    End Sub

    Private Sub CTRLselectUsers_UsersSelected(idUsers As System.Collections.Generic.List(Of Integer)) Handles CTRLselectUsers.UsersSelected

        Dim IsManagers As Boolean = If(Me.RBLroles.SelectedValue = "1", True, False)
        Me.CurrentPresenter.UsersAdd(idUsers, IsManagers)

    End Sub
#End Region

    Public Sub ShowSendResponse(sended As Boolean) Implements iViewCategory.ShowSendResponse

        Me.DVmessages.Visible = True
        Me.DVmessages.Attributes.Add("class", "fieldswrapper fullwidth")

        If (sended) Then
            CTRLmessagesInfo.InitializeControl(Resource.getValue("Message.Sended"), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
        Else
            CTRLmessagesInfo.InitializeControl(Resource.getValue("Message.UnSended"), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
        End If

        Me.CTRLmessagesInfo.Visible = True

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