Imports lm.Comol.Core.BaseModules.Tickets.Domain.DTO
Imports lm.Comol.Core.BaseModules.Tickets.Domain.Enums
Imports lm.Comol.Core.DomainModel.Helpers
Imports NHibernate.Hql.Ast.ANTLR
Imports TK = lm.Comol.Core.BaseModules.Tickets
Imports MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType

Public Class GlobalAdmin
    Inherits TicketBase 'PageBase
    Implements TK.Presentation.View.iViewGlobalAdmin
    

#Region "Context"
    Private _Presenter As TK.Presentation.GlobalAdminPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.GlobalAdminPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TK.Presentation.GlobalAdminPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

    Public Property Settings As TK.Domain.SettingsPortal Implements TK.Presentation.View.iViewGlobalAdmin.Settings
        Get
            Dim sets As New TK.Domain.SettingsPortal()

            sets.HasExternalLimitation = Me.CBXexternalLimit.Checked
            sets.HasInternalLimitation = Me.CBXinternalLimit.Checked
            sets.HasDraftLimitation = Me.CBXdraftLimit.Checked

            'sets.hasDraftLimitationError = False
            'sets.hasExternalLimitationError = False
            'sets.hasInternalLimitationError = False

            Dim extLimit As Integer = -1
            Dim intLimit As Integer = -1
            Dim drfLimit As Integer = -1

            TXBexternalLimit.CssClass = "inputchar"
            TXBinternalLimit.CssClass = "inputchar"
            TXBdraftLimit.CssClass = "inputchar"

            Try
                extLimit = System.Convert.ToInt16(Me.TXBexternalLimit.Text)
            Catch ex As Exception
                extLimit = -1
                TXBexternalLimit.CssClass = "inputchar input-validation-error"
                sets.HasExternalLimitation = False
                'sets.hasExternalLimitationError = True
            End Try

            sets.ExternalLimitation = extLimit

            Try
                intLimit = System.Convert.ToInt16(Me.TXBinternalLimit.Text)
            Catch ex As Exception
                intLimit = -1
                TXBinternalLimit.CssClass = "inputchar input-validation-error"
                sets.HasInternalLimitation = False
                'sets.hasInternalLimitationError = False
            End Try

            sets.InternalLimitation = intLimit

            Try
                drfLimit = System.Convert.ToInt16(Me.TXBdraftLimit.Text)
            Catch ex As Exception
                drfLimit = -1
                TXBdraftLimit.CssClass = "inputchar input-validation-error"
                sets.HasDraftLimitation = False
                'sets.hasDraftLimitationError = True
            End Try

            sets.DraftLimitation = drfLimit

            sets.CommunityTypeSettings = New List(Of TK.Domain.SettingsComType)

            For Each Item As RepeaterItem In Me.RPTcommunityTypes.Items

                If Item.ItemType = ListItemType.Item OrElse Item.ItemType = ListItemType.AlternatingItem Then

                    Dim SCT As New TK.Domain.SettingsComType()


                    Dim hif_Id As HiddenField = Item.FindControl("hif_Id")
                    If Not IsNothing(hif_Id) Then
                        SCT.Id = hif_Id.Value
                    End If

                    Dim cbxSettingsView, cbxSettingsPublic, cbxSettingsTicket, cbxSettingsPrivate As CheckBox
                    cbxSettingsView = Item.FindControl("CBXsettingsView")
                    If Not IsNothing(cbxSettingsView) Then
                        SCT.ViewTicket = cbxSettingsView.Checked
                    End If

                    cbxSettingsPublic = Item.FindControl("CBXsettingsPublic")
                    If Not IsNothing(cbxSettingsPublic) Then
                        SCT.CreatePublic = cbxSettingsPublic.Checked
                    End If

                    cbxSettingsTicket = Item.FindControl("CBXsettingsTicket")
                    If Not IsNothing(cbxSettingsTicket) Then
                        SCT.CreateTicket = cbxSettingsTicket.Checked
                    End If

                    cbxSettingsPrivate = Item.FindControl("CBXsettingsPrivate")
                    If Not IsNothing(cbxSettingsPrivate) Then
                        SCT.CreatePrivate = cbxSettingsPrivate.Checked
                    End If

                    sets.CommunityTypeSettings.Add(SCT)


                End If
            Next

            'sets.MailSettings = Me.CTRLmailSets.MailSettings


            'Impostazioni accesso servizio
            sets.CanCreateCategory = Me.CTRLswCat.Status ' Me.CBXcanCreateCategory.Checked
            sets.CanShowTicket = Me.CTRLswView.Status  'Me.CBXcanShowTicket.Checked
            sets.CanEditTicket = Me.CTRLswEdit.Status   'Me.CBXcanEditTicket.Checked

            Return sets
        End Get
        Set(value As TK.Domain.SettingsPortal)

            'Me.CTRLmailSets.ShowDefault = False

            Me.CBXexternalLimit.Checked = value.HasExternalLimitation
            Me.TXBexternalLimit.Text = value.ExternalLimitation

            If Not value.HasExternalLimitation Then
                Me.TXBexternalLimit.CssClass &= " disabled"
                Me.TXBexternalLimit.Enabled = False
            End If


            Me.CBXinternalLimit.Checked = value.HasInternalLimitation
            Me.TXBinternalLimit.Text = value.InternalLimitation
            If Not value.HasInternalLimitation Then
                Me.TXBinternalLimit.CssClass &= " disabled"
                Me.TXBinternalLimit.Enabled = False
            End If

            Me.CBXdraftLimit.Checked = value.HasDraftLimitation
            Me.TXBdraftLimit.Text = value.DraftLimitation
            If Not value.HasDraftLimitation Then
                Me.TXBdraftLimit.CssClass &= " disabled"
                Me.TXBdraftLimit.Enabled = False
            End If

            SetCategories(value)

            If Not IsNothing(value.CommunityTypeSettings) AndAlso value.CommunityTypeSettings.Count >= 0 Then
                RPTcommunityTypes.Visible = True
                RPTcommunityTypes.DataSource = value.CommunityTypeSettings
                RPTcommunityTypes.DataBind()
            Else
                RPTcommunityTypes.Visible = False
            End If

            'Me.CTRLmailSets.MailSettings = value.MailSettings
            SetSwitch(New DTO_PortalSettingsSwitch(value))

            'NOTIFICHE
            Me.CTRLmailSetUser.BindSettings(False, False, value.MailSettingsUser, TK.Domain.Enums.ViewSettingsUser.Owner, True, True, True)
            Me.CTRLmailSetMan.BindSettings(False, False, value.MailSettingsManager, TK.Domain.Enums.ViewSettingsUser.Manager, True, True, False)




        End Set
    End Property

    Public ReadOnly Property CurrentCategorySelection As Long Implements TK.Presentation.View.iViewGlobalAdmin.CurrentCategorySelection
        Get
            Return Me.CTRLddlCat.SelectedId
        End Get
    End Property
 

    Public WriteOnly Property CategoryInfo As TK.Domain.DTO.DTO_SysCategoryInfo Implements TK.Presentation.View.iViewGlobalAdmin.CategoryInfo
        Set(value As TK.Domain.DTO.DTO_SysCategoryInfo)
            LBpublicNum.Text = value.Public.ToString()
            LBcommunityNum.Text = value.Community.ToString()
            LBticketNum.Text = value.Ticket.ToString()
        End Set
    End Property

    Public Property ViewCommunityId As Integer Implements TK.Presentation.View.iViewGlobalAdmin.ViewCommunityId
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

#End Region

#Region "Internal"
    Public ReadOnly Property CatDialogTitle As String
        Get
            Return Resource.getValue("CategoriesDialog.Title")
        End Get
    End Property
    Public ReadOnly Property InternalUserAddTitle As String
        Get
            Return Resource.getValue("Modal.InternalUsers.Title")
        End Get
    End Property
#End Region

    Private Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"

    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False

        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ServiceNopermission = Resource.getValue("Error.NoPermission")
        Master.ShowNoPermission = True
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_GlobalAdmin", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LNBupdateComType, True, True, False, False)
            .setLinkButton(LNBsaveSettings, True, True, False, False)
            .setLinkButton(LNBsaveSettings_Bot, True, True, False, False)


            .setCheckBox(CBXexternalLimit)
            .setCheckBox(CBXinternalLimit)
            .setCheckBox(CBXdraftLimit)
            .setLabel(LBcategoryTree_t)
            .setLiteral(LTpageTitle_t)
            .setLiteral(LTtitle_t)

            '.setLiteral(LTnotification_t)
            .setLabel(LBcateDef_t)

            .setLabel(LBpublicNum_t)
            .setLabel(LBcommunityNum_t)
            .setLabel(LBticketNum_t)

            .setLinkButton(LNBsetDefault, True, True, False, True)
            .setLinkButton(LNBremDefault, True, True, False, True)
            .setButton(LNBaddUsers, True, False, False, True)

            .setLiteral(LTService_t)
            .setLiteral(LTService_d)
            .setLiteral(LTlimits_t)
            .setLiteral(LTcategories_t)
            .setLiteral(LTcategories_d)
            .setLiteral(LTpermission_t)
            .setLiteral(LTmandatory_l)
            .setLiteral(LTedit_t)
            .setLiteral(LTedit_d)

            .setLiteral(LTbehalf_t)
            .setLiteral(LTbehalf_d)

            .setLabel(LBbehalfType)

            .setLiteral(LTnotificationUsr_t)
            .setLiteral(LTnotificationMan_t)

            '.setLinkButton(LNBsaveMailSets, True, True, False, False)

        End With

        'Madatory
        LBcateDef_t.Text &= Me.LTmandatoryTemplate.Text
        LTmandatory_l.Text = LTmandatory_l.Text.Replace("{0}", Me.LTmandatoryTemplate.Text)

        Me.CTRLswService.SetText(Resource, True, True)
        Me.CTRLswCat.SetText(Resource, True, True)
        Me.CTRLswEdit.SetText(Resource, True, True)
        Me.CTRLswView.SetText(Resource, True, True)
        Me.CTRLswBehalf.SetText(Resource, True, True)


        CTRLswUserNotification.SetText(Resource, True, True)
        CTRLswManNotification.SetText(Resource, True, True)

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#Region "TicketBase"

    Public Overrides Sub DisplaySessionTimeout(CommunityId As Integer)
        RedirectOnSessionTimeOut(TK.Domain.RootObject.SettingsGlobal(), CommunityId)
    End Sub

    Public Overrides Sub ShowNoAccess()

    End Sub

#End Region

#End Region

#Region "Implements"

    Public Sub ShowNoPermission() Implements TK.Presentation.View.iViewGlobalAdmin.ShowNoPermission
        Me.PNLcontent.Visible = False
        Master.ServiceNopermission = Resource.getValue("Error.NoPermission")
        Me.Master.ShowNoPermission = True
    End Sub

    Public Sub ShowMessage(Status As TK.Domain.Enums.GlobalAdminStatus, Optional draftLimitErr As Boolean = False, Optional intLimitErr As Boolean = False, Optional extLimitErr As Boolean = False) Implements TK.Presentation.View.iViewGlobalAdmin.ShowMessage

        If Not Status = TK.Domain.Enums.GlobalAdminStatus.none Then


            Dim messages As New List(Of dtoMessage)()


            Dim ErrType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.success

            If (Status = GlobalAdminStatus.InternalError) Then

                Dim defMessage As New dtoMessage()
                defMessage.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.error
                defMessage.Text = Resource.getValue("Status.InternalError")
                messages.Add(defMessage)

            ElseIf intLimitErr OrElse draftLimitErr OrElse extLimitErr Then


                Dim defMessage As New dtoMessage()
                defMessage.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.success
                defMessage.Text = Resource.getValue("Status.Success.InvalidField")
                messages.Add(defMessage)

                If (draftLimitErr) Then
                    Dim draftMessage As New dtoMessage()
                    draftMessage.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
                    draftMessage.Text = Resource.getValue("Status.Success.InvalidField.Draft")
                    messages.Add(draftMessage)

                End If
                If (intLimitErr) Then
                    Dim intMessage As New dtoMessage()
                    intMessage.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
                    intMessage.Text = Resource.getValue("Status.Success.InvalidField.Internal")
                    messages.Add(intMessage)

                End If
                If (extLimitErr) Then
                    Dim extMessage As New dtoMessage()
                    extMessage.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
                    extMessage.Text = Resource.getValue("Status.Success.InvalidField.External")
                    messages.Add(extMessage)

                End If

            Else

                Dim defMessage As New dtoMessage()
                defMessage.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.success
                defMessage.Text = Resource.getValue("Status." & Status.ToString())
                messages.Add(defMessage)
            End If


            If Not IsNothing(messages) AndAlso messages.Any() Then
                UCactionMessages.Visible = True
                UCactionMessages.InitializeControl(messages)
            Else
                UCactionMessages.Visible = False
            End If
        Else

            UCactionMessages.Visible = False

        End If

    End Sub
    'Private Sub InitializeUsersSelector(removeUsers As List(Of Integer)) Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewGlobalAdmin.InitializeUsersSelector

    'End Sub
#End Region

#Region "Internal/Handler"

    Private Sub SetCategories(value As TK.Domain.SettingsPortal) Implements TK.Presentation.View.iViewGlobalAdmin.SetCategories
        If Not IsNothing(value.PublicCategories) AndAlso value.PublicCategories.Count > 0 Then

            If Not IsNothing(value.CategoryDefault) Then
                CTRLddlCat.InitDDL(value.PublicCategories, value.CategoryDefault.Id, Resource.getValue("Category.select"))
                Me.LNBremDefault.Visible = True
            Else
                CTRLddlCat.InitDDL(value.PublicCategories, -1, Resource.getValue("Category.select"))
                Me.LNBremDefault.Visible = False
            End If
        End If
    End Sub

    Private Sub SetSwitch(ByVal settings As TK.Domain.DTO.DTO_PortalSettingsSwitch)
        'Impostazioni accesso servizio
        Me.CTRLswService.Status = settings.IsActive
        Me.CTRLswService.Enabled = False

        Me.CTRLswCat.Status = settings.CanCreateCategory
        Me.CTRLswCat.Enabled = settings.IsActive

        Me.CTRLswView.Status = settings.CanShowTicket
        Me.CTRLswView.Enabled = settings.IsActive AndAlso settings.CanCreateCategory

        Me.CTRLswEdit.Status = settings.CanEditTicket
        Me.CTRLswEdit.Enabled = settings.IsActive AndAlso settings.CanShowTicket


        Me.CTRLswBehalf.Enabled = settings.CanEditTicket
        Me.CTRLswBehalf.Status = settings.CanBehalf AndAlso settings.CanEditTicket

        Me.CTRLswManNotification.Status = settings.IsNotificationManActive
        Me.CTRLswUserNotification.Status = settings.IsNotificationUserActive
    End Sub


    Private Sub RPTcommunityTypes_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommunityTypes.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim LBsettings_Name_t, LBsettings_ViewTicket_t, LBsettings_Public_t, LBsettings_Ticket_t, LBsettings_Private_t As Label
            LBsettings_Name_t = e.Item.FindControl("LBsettings_Name_t")
            LBsettings_ViewTicket_t = e.Item.FindControl("LBsettings_ViewTicket_t")
            LBsettings_Public_t = e.Item.FindControl("LBsettings_Public_t")
            LBsettings_Ticket_t = e.Item.FindControl("LBsettings_Ticket_t")
            LBsettings_Private_t = e.Item.FindControl("LBsettings_Private_t")


            If Not IsNothing(LBsettings_Name_t) Then
                Me.Resource.setLabel(LBsettings_Name_t)
            End If
            If Not IsNothing(LBsettings_ViewTicket_t) Then
                Me.Resource.setLabel(LBsettings_ViewTicket_t)
            End If
            If Not IsNothing(LBsettings_Public_t) Then
                Me.Resource.setLabel(LBsettings_Public_t)
            End If
            If Not IsNothing(LBsettings_Ticket_t) Then
                Me.Resource.setLabel(LBsettings_Ticket_t)
            End If
            If Not IsNothing(LBsettings_Private_t) Then
                Me.Resource.setLabel(LBsettings_Private_t)
            End If

        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim SCT As TK.Domain.SettingsComType = e.Item.DataItem()

            Dim hif_Id As HiddenField = e.Item.FindControl("hif_Id")
            If Not IsNothing(hif_Id) Then
                hif_Id.Value = SCT.Id
            End If

            Dim LBsettings_Name As Label = e.Item.FindControl("LBsettings_Name")

            If Not IsNothing(LBsettings_Name) Then
                If Not IsNothing(SCT.CommunityType) Then
                    LBsettings_Name.Text = SCT.CommunityType.Name
                End If
            End If

            Dim cbxSettingsView, cbxSettingsPublic, cbxSettingsTicket, cbxSettingsPrivate As CheckBox
            cbxSettingsView = e.Item.FindControl("CBXsettingsView")
            If Not IsNothing(cbxSettingsView) Then
                cbxSettingsView.Checked = SCT.ViewTicket
            End If

            cbxSettingsPublic = e.Item.FindControl("CBXsettingsPublic")
            If Not IsNothing(cbxSettingsPublic) Then
                cbxSettingsPublic.Checked = SCT.CreatePublic
            End If

            cbxSettingsTicket = e.Item.FindControl("CBXsettingsTicket")
            If Not IsNothing(cbxSettingsTicket) Then
                cbxSettingsTicket.Checked = SCT.CreateTicket
            End If

            cbxSettingsPrivate = e.Item.FindControl("CBXsettingsPrivate")
            If Not IsNothing(cbxSettingsPrivate) Then
                cbxSettingsPrivate.Checked = SCT.CreatePrivate
            End If

        End If
    End Sub

    Private Sub LNBupdateComType_Click(sender As Object, e As System.EventArgs) Handles LNBupdateComType.Click
        Me.CurrentPresenter.UpdateCommunityTypes()
    End Sub

    Private Sub LNBsaveSettings_Click(sender As Object, e As System.EventArgs) Handles LNBsaveSettings.Click
        Save()
    End Sub

    Private Sub GlobalAdmin_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True

        SetCssClass()

    End Sub

    ''' <summary>
    ''' Imposta le classi css nei singoli elementi dei controlli .net nella pagina
    ''' </summary>
    ''' <remarks>
    '''   lit_CBXInputAttributes_Class            contiene la classe da impostare al tag INPUT di una CHECKBOX
    '''   lit_CBXLabelAttributes_Class            contiene la classe da impostare al tag LABEL di una CHECKBOX
    ''' </remarks>
    Private Sub SetCssClass()
        Me.CBXexternalLimit.InputAttributes.Add("class", Me.LTcbxInputAttributes_Class.Text)
        Me.CBXinternalLimit.InputAttributes.Add("class", Me.LTcbxInputAttributes_Class.Text)
        Me.CBXexternalLimit.LabelAttributes.Add("class", Me.LTcbxLabelAttributes_Class.Text)
        Me.CBXinternalLimit.LabelAttributes.Add("class", Me.LTcbxLabelAttributes_Class.Text)

        Me.CBXdraftLimit.InputAttributes.Add("class", Me.LTcbxInputAttributes_Class.Text)
        Me.CBXdraftLimit.LabelAttributes.Add("class", Me.LTcbxLabelAttributes_Class.Text)

    End Sub

    Private Sub LNBsetDefault_Click(sender As Object, e As System.EventArgs) Handles LNBsetDefault.Click
        Dim DefCatId As Int64 = Me.CTRLddlCat.SelectedId

        If (DefCatId > 0) Then
            Me.CurrentPresenter.CategorySetDefault(DefCatId)
            'Me.CurrentPresenter.InitView(TK.Domain.Enums.GlobalAdminStatus.CategoryDefSetted)

        Else
            'no selection
        End If
    End Sub

    Private Sub LNBremDefault_Click(sender As Object, e As System.EventArgs) Handles LNBremDefault.Click
        Me.CurrentPresenter.CategoryRemoveDefault()
        Me.CurrentPresenter.InitView(TK.Domain.Enums.GlobalAdminStatus.CategoryDefRemoved)
    End Sub

#End Region

    Public Property Permissions As TK.Domain.DTO.DTO_SettingsPermissionList Implements TK.Presentation.View.iViewGlobalAdmin.Permissions
        Get
            Dim setPerm As New TK.Domain.DTO.DTO_SettingsPermissionList()

            setPerm.PersonTypePermission = (From li As ListItem In SLBbehalfType.Items Where li.Selected = True _
                                Select New TK.Domain.DTO.DTO_SettingsPermissionPersonType With { _
                                    .DisplayName = li.Text, _
                                    .PersonTypeId = System.Convert.ToInt32(li.Value), _
                                    .IsSelected = True _
                                }).ToList()
            Return setPerm
        End Get
        Set(value As TK.Domain.DTO.DTO_SettingsPermissionList)

            SLBbehalfType.Items.Clear()

            If Not IsNothing(value.PersonTypePermission) Then

                For Each itm As TK.Domain.DTO.DTO_SettingsPermissionPersonType In value.PersonTypePermission

                    Dim li As New ListItem(itm.DisplayName, itm.PersonTypeId.ToString())
                    li.Selected = itm.IsSelected

                    SLBbehalfType.Items.Add(li)

                Next
            End If

            If IsNothing(value.UserPermission) OrElse Not value.UserPermission.Any() Then
                Me.RPTusersBehalf.Visible = False
                Resource.setLabel_To_Value(LBdescritption_t, "LBdescritption_t.noUsers")
            Else
                Resource.setLabel_To_Value(LBdescritption_t, "LBdescritption_t.hasUsers")

                Me.RPTusersBehalf.Visible = True
                Me.RPTusersBehalf.DataSource = value.UserPermission
                Me.RPTusersBehalf.DataBind()

            End If


            'DIVusers.Visible = True

            Dim removeUsers As List(Of Int32) = (From per As TK.Domain.DTO.DTO_SettingsPermissionUsers _
                                In value.UserPermission _
                                Where per.PersonId > 0
                                Select per.PersonId).Distinct().ToList()


            CTRLselectUsers.InitializeControl( _
       lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.SystemUsers, _
            True, 0, _
            removeUsers, Nothing, _
            Resource.getValue("Modal.InternalUsers.Description"))

            'DIVusers.Visible = True

            ' Me.CTRLselectUsers.InitializeControl( _
            'lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.SystemUsers,
            'True, 0,
            'UsersIds, UsersIds, _
            'Resource.getValue("Modal.InternalUsers.Description"))

        End Set
    End Property

    Public Function GetTranslation(ByVal Value As String, Optional ByVal KeepEmpty As Boolean = False) As String
        Dim outVal As String = Resource.getValue(Value)

        If Not KeepEmpty AndAlso String.IsNullOrEmpty(outVal) Then
            outVal = Value
        End If

        Return outVal

    End Function


    Private Sub CTRLswBehalf_StatusChange(Status As Boolean) Handles CTRLswBehalf.StatusChange
        Me.CurrentPresenter.SetSwitch(TK.Domain.Enums.GlobalAdminSwitch.TicketBehalf, Status)
    End Sub

    Private Sub CTRLswCat_StatusChange(Status As Boolean) Handles CTRLswCat.StatusChange
        Me.CurrentPresenter.SetSwitch(TK.Domain.Enums.GlobalAdminSwitch.CategoryManagement, Status)
    End Sub

    Private Sub CTRLswEdit_StatusChange(Status As Boolean) Handles CTRLswEdit.StatusChange
        Me.CurrentPresenter.SetSwitch(TK.Domain.Enums.GlobalAdminSwitch.TicketWrite, Status)
    End Sub

    Private Sub CTRLswView_StatusChange(Status As Boolean) Handles CTRLswView.StatusChange
        Me.CurrentPresenter.SetSwitch(TK.Domain.Enums.GlobalAdminSwitch.TicketRead, Status)
        'If CTRLswView.Status = False Then
        '    Me.CTRLswEdit.Status = False
        'End If
    End Sub

    Private Sub RPTusersBehalf_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTusersBehalf.ItemCommand
        Dim Id As Int64 = System.Convert.ToInt64(e.CommandArgument)

        If (Id > 0) Then
            Me.CurrentPresenter.PermissionUserDelete(Id)
        End If
    End Sub

    Private Sub RPTusersBehalf_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTusersBehalf.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim LTname_t, LTuserType_t As Literal
            LTname_t = e.Item.FindControl("LTname_t")
            If Not IsNothing(LTname_t) Then
                Resource.setLiteral(LTname_t)
            End If

            LTuserType_t = e.Item.FindControl("LTuserType_t")
            If Not IsNothing(LTuserType_t) Then
                Resource.setLiteral(LTuserType_t)
            End If

        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim usrPerm As TK.Domain.DTO.DTO_SettingsPermissionUsers

            Try
                usrPerm = e.Item.DataItem
            Catch ex As Exception
            End Try

            If Not IsNothing(usrPerm) Then
                Dim LTuserName, LTuserType As Literal
                Dim LKBdelete As LinkButton

                LTuserName = e.Item.FindControl("LTuserName")
                If Not IsNothing(LTuserName) Then
                    LTuserName.Text = usrPerm.DisplayName
                End If

                LTuserType = e.Item.FindControl("LTuserType")
                If Not IsNothing(LTuserType) Then
                    LTuserType.Text = usrPerm.PersonType
                End If

                LKBdelete = e.Item.FindControl("LKBdelete")
                If Not IsNothing(LKBdelete) Then
                    LKBdelete.ToolTip = Me.Resource.getValue("Table.Delete.UserPermission")
                    LKBdelete.CommandName = "Delete"
                    LKBdelete.CommandArgument = usrPerm.Id
                End If

            End If


        End If
    End Sub

#Region "Internal"
#Region "Add Users"
    Private Sub CLTRselectUsers_CloseWindow() Handles CTRLselectUsers.CloseWindow
        DIVusers.Visible = False
    End Sub
    Private Sub CLTRselectUsers_UsersSelected(idUsers As List(Of Integer)) Handles CTRLselectUsers.UsersSelected
        Me.CurrentPresenter.PermissionUserAdd(idUsers, TK.Domain.Enums.PermissionType.Behalf)
        DIVusers.Visible = False
    End Sub
    Private Sub LNBaddUsers_Click(sender As Object, e As EventArgs) Handles LNBaddUsers.Click
        'CurrentPresenter.SelectUsersToAdd()

        'Il bind lo faccio all'init, dovendo già recuperare i dati relativi ai tipi,
        'recupero in un unica query ANCHE gli ID person in un unica query,
        'dividendo le due cose DOPO il caricamento da dB.

        DIVusers.Visible = True
    End Sub
#End Region
#End Region





    Private Sub LNBsaveSettings_Bot_Click(sender As Object, e As EventArgs) Handles LNBsaveSettings_Bot.Click
        Save()
    End Sub

    Private Sub Save()
        'Me.CurrentPresenter.MailSendSettingsSet()


        'Me.CurrentPresenter.MailSendStatus(Me.CTRLswManNotification.Status, True)
        'Me.CurrentPresenter.MailSendStatus(Me.CTRLswUserNotification.Status, False)

        Me.CurrentPresenter.SaveSettings( _
            Me.CTRLswUserNotification.Status, _
            Me.CTRLswManNotification.Status, _
            Me.CTRLmailSetUser.GetSettings(), _
            Me.CTRLmailSetMan.GetSettings()
            )
    End Sub

    'Private Sub LBsaveManSets_Click(sender As Object, e As EventArgs) Handles LBsaveManSets.Click
    '    Me.CurrentPresenter.MailSendStatus(Me.CTRLswManNotification.Status, True)
    'End Sub


    'Private Sub LBsaveUsrSets_Click(sender As Object, e As EventArgs) Handles LBsaveUsrSets.Click
    '    Me.CurrentPresenter.MailSendStatus(Me.CTRLswUserNotification.Status, False)
    'End Sub

    'Private Sub LNBsaveMailSets_Click(sender As Object, e As EventArgs) Handles LNBsaveMailSets.Click
    '    
    'End Sub

    Private Sub CTRLswManNotification_StatusChange(Status As Boolean) Handles CTRLswManNotification.StatusChange
        Me.CurrentPresenter.MailSetStatus(Status, True)
        'Me.CurrentPresenter.MailSendStatus(Me.CTRLswUserNotification.Status, False)
    End Sub


    Private Sub CTRLswUserNotification_StatusChange(Status As Boolean) Handles CTRLswUserNotification.StatusChange
        Me.CurrentPresenter.MailSetStatus(Status, False)
    End Sub

    Public Sub ShowSwitchChanged(SwitchStatus As TK.Domain.DTO.DTO_PortalSettingsSwitch, _
                                 setSwitch As GlobalAdminSwitch, _
                                 status As Boolean, _
                                 success As Boolean) Implements TK.Presentation.View.iViewGlobalAdmin.ShowSwitchChanged

        If (Not success) Then
            ShowMessage(GlobalAdminStatus.InternalError)
            Return
        End If

        Me.SetSwitch(SwitchStatus)

        If (setSwitch = GlobalAdminSwitch.none) Then
            UCactionMessages.Visible = False
        Else

            UCactionMessages.Visible = True
            Dim message As String = Resource.getValue(String.Format("Status.Switch.{0}.{1}", setSwitch.ToString(), status.ToString()))

            If Not String.IsNullOrEmpty(message) Then
                UCactionMessages.Visible = True
                UCactionMessages.InitializeControl(message, MessageType.success)
            Else
                UCactionMessages.Visible = False
            End If



        End If

    End Sub


End Class