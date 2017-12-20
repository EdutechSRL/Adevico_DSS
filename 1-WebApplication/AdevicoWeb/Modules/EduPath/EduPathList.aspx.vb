Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.Modules.EduPath.Domain
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Linq

Public Class EPList
    Inherits PageBaseEduPath

    Dim DragDrop As Boolean = True

    Protected Overrides ReadOnly Property PathType As EPType
        Get
            Return EPType.None
        End Get
    End Property
#Region "InitStandard"

    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _serviceEP As lm.Comol.Modules.EduPath.BusinessLogic.Service
    Private _ServiceCertification As lm.Comol.Core.Certifications.Business.CertificationsService
    Private ReadOnly Property ServiceEP As Service
        Get
            If IsNothing(_serviceEP) Then
                _serviceEP = New Service(Me.CurrentContext)
            End If
            Return _serviceEP
        End Get
    End Property
    Private ReadOnly Property ServiceCF As lm.Comol.Core.Certifications.Business.CertificationsService
        Get
            If IsNothing(_ServiceCertification) Then
                _ServiceCertification = New lm.Comol.Core.Certifications.Business.CertificationsService(Me.CurrentContext)
            End If
            Return _ServiceCertification
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region

#Region " Base"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()

        If Me.CurrentCommunityID = 0 Then
            Me.ShowError(EpError.Generic)
        Else
            Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.List, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, "all"), InteractionType.UserWithLearningObject)
            InitPage()
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.ShowError(EpError.NotPermission)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return ServiceEP.CheckCommunityId(Me.CurrentCommunityID)
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EpList", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(Me.HYPnewEp, False, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property

#End Region


#Region "Property"


    Private _BottomIsVisible As String
    Private _CssClassDescription As String
    Private _CanSwichEduPaths As Boolean
    Private _EpCount As Int16
    Private _SmartTagsAvailable As SmartTags
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase(True))
            End If
            Return _SmartTagsAvailable
        End Get
    End Property
    Public Property EpCount As Int16
        Get
            Return _EpCount
        End Get
        Set(ByVal value As Int16)
            _EpCount = value
        End Set
    End Property

    Public Property CanSwichEduPaths As Boolean
        Get
            Return _CanSwichEduPaths
        End Get
        Set(ByVal value As Boolean)
            _CanSwichEduPaths = value
        End Set
    End Property

    Public ReadOnly Property CssClassDescription As String
        Get
            If Me.ViewModeType = EpViewModeType.Manage Then
                Return "description"
            Else
                Return "description bottomLine"
            End If
        End Get

    End Property

    Public ReadOnly Property BottomIsVisible As String
        Get
            If Me.ViewModeType = EpViewModeType.Manage Then
                Return ""
            Else
                Return " hidden"
            End If
        End Get
    End Property


    Private ReadOnly Property ViewModeType As EpViewModeType
        Get
            Dim qs_viewModeType As String = Request.QueryString("ViewMode")
            If IsNumeric(qs_viewModeType) Then

                Select Case qs_viewModeType
                    Case EpViewModeType.Manage
                        Return EpViewModeType.Manage
                    Case EpViewModeType.View
                        Return EpViewModeType.View
                    Case Else
                        Return EpViewModeType.None
                End Select
            Else
                Return EpViewModeType.None
            End If
        End Get
    End Property
    Private ReadOnly Property CurrentCommRoleID As Integer
        Get
            Return UtenteCorrente.GetIDRuoloForComunita(CurrentCommunityID)
        End Get
    End Property

    Private ReadOnly Property CurrentUserId() As Integer
        Get
            Return Me.CurrentContext.UserContext.CurrentUserID
        End Get
    End Property

    Public ReadOnly Property sortVisibility() As String
        Get
            If Me.CanSwichEduPaths Then
                Return "true"
            End If
            Return "false"
        End Get
    End Property
#End Region

    Private Sub InitVisibiltyFilters(ByVal isVisible As Boolean)
        If isVisible Then
            Me.RBLvisibility.Text = Me.Resource.getValue("Filter")
            Dim oListItem As New ListItem
            oListItem.Text = Me.Resource.getValue("AllEp")
            oListItem.Value = "AllEp"
            oListItem.Selected = True
            Me.RBLvisibility.Items.Add(oListItem)
            oListItem = New ListItem
            oListItem.Text = Me.Resource.getValue("VisibleEp")
            oListItem.Value = "VisibleEp"
            oListItem.Selected = False
            Me.RBLvisibility.Items.Add(oListItem)
        Else
            Me.RBLvisibility.Visible = False
        End If
    End Sub

    Private Sub InitPage()
        Dim EPList As New List(Of dtoEPitemList)
        Me.Resource.setHyperLink(Me.HYPnewEp, False, True)
        Me.Master.ServiceTitle = Me.Resource.getValue("LBtitolo.text") & ServiceEP.GetCommunityName(Me.CurrentCommunityID)
      
        EPList = ServiceEP.GetMyEduPaths(Me.CurrentContext.UserContext.CurrentUserID, CurrentCommunityID, CurrentCommRoleID, Me.ViewModeType)

        LNBSwitchView.Visible = MyBase.PermissionOtherCommunity(CurrentCommunityID).Admin

        Select Case Me.ViewModeType

            Case EpViewModeType.View


                ULPathList.Attributes("class") = ULPathList.Attributes("class") + " view"

                Me.HYPsummary.NavigateUrl = MyBase.BaseUrl & RootObject.UserPathSummary(Me.CurrentContext.UserContext.CurrentUserID, CurrentCommunityID, "list")

                Me.HYPsummary.Text = Me.Resource.getValue("VIEW-HYPsummary.text")
                Me.HYPsummary.ToolTip = Me.Resource.getValue("VIEW-HYPsummary.ToolTip")

                If EPList.Count = 1 AndAlso Not Request.QueryString("Red") = -1 AndAlso ServiceEP.CheckStatus(EPList(0).Status, Status.NotLocked) Then

                    'Dim oPermission As PermissionEP = ServiceEP.GetUserPermission_ByPath(EPList(0).Id, CurrentUserId, CurrentCommRoleID)
                    'Dim onlystats As Boolean = oPermission.Read = False And oPermission.ViewOwnStat = True

                    Dim onlystats As Boolean = EPList(0).PermissionEP.Read = False And EPList(0).PermissionEP.ViewOwnStat = True
                    If onlystats Then
                        Me.RPeduPathList.DataSource = EPList
                        Me.RPeduPathList.DataBind()
                    Else
                        RedirectToUrl(RootObject.PathView(EPList(0).Id, Me.CurrentCommunityID, Me.ViewModeType, ServiceEP.CheckEpType(EPList(0).type, EPType.PlayMode)))
                    End If

                    'RedirectToUrl(RootObject.PathView(EPList(0).Id, Me.CurrentCommunityID, Me.ViewModeType, ServiceEP.CheckEpType(EPList(0).type, EPType.PlayMode)))
                Else
                    Me.RPeduPathList.DataSource = EPList
                    Me.RPeduPathList.DataBind()
                End If

                Me.CanSwichEduPaths = False

                LNBSwitchView.Text = Me.Resource.getValue("Manage")
                LNBSwitchView.ToolTip = Me.Resource.getValue("Manage")
            Case EpViewModeType.Manage

                Me.HYPsummary.NavigateUrl = MyBase.BaseUrl & RootObject.PathSummary(CurrentCommunityID)
                Me.HYPsummary.Text = Me.Resource.getValue("MANAGE-HYPsummary.text")
                Me.HYPsummary.ToolTip = Me.Resource.getValue("MANAGE-HYPsummary.ToolTip")

                LNBSwitchView.Text = Me.Resource.getValue("View")
                LNBSwitchView.ToolTip = Me.Resource.getValue("View")
                ULPathList.Attributes("class") = ULPathList.Attributes("class") + " manage"

                Me.HYPnewEp.NavigateUrl = Me.BaseUrl & RootObject.AddPath(CurrentCommunityID, EPType.TimeAuto)  'RootObject.NewPath(Me.CurrentCommunityID)
                Me.HYPnewEp.Visible = MyBase.Permission.Admin
                Dim NotVisibleEp As Integer = ServiceEP.GetEvaluateManagerLockedPathCount(EPList)
                InitVisibiltyFilters(NotVisibleEp > 0)
                Me.EpCount = CType(EPList.Count, Integer)
                Me.CanSwichEduPaths = EpCount > 1 AndAlso (MyBase.Permission.Admin OrElse Me.ServiceEP.GetIfCanChangePathsDisplayOrder(EPList, Me.CurrentUserId, Me.CurrentCommRoleID))
                Me.InitDialog()
                Me.RPeduPathList.DataSource = EPList
                Me.RPeduPathList.DataBind()
            Case Else
                ShowError(EpError.Url)
        End Select
    End Sub

    Private Sub ShowError(ByVal ErrorType As EpError)
        Me.Resource.setHyperLink(Me.HYPerror, False, True)
        Me.HYPerror.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, Me.ViewModeType)
        Select Case ErrorType
            Case EpError.Generic
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Generic.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.NotPermission
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.NotPermission.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.Url
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Url.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.Data
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Data.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
        End Select
        Me.MLVeduPathList.ActiveViewIndex = 1
    End Sub

    Private Sub RPeduPathList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPeduPathList.ItemCommand
        Try
            Select Case e.CommandName
                Case "default"
                    If ServiceEP.SetDefaultPath(e.CommandArgument, Me.CurrentCommunityID, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage))
                    Else
                        ShowError(EpError.Generic)
                    End If

                Case "virtualdelete"
                    If ServiceEP.VirtualDeleteAllEp(e.CommandArgument, Me.CurrentCommunityID, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage))
                    Else
                        ShowError(EpError.Generic)
                    End If

                Case "moveUp"
                    If ServiceEP.MovePathDisplayOrderBefore(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage))
                    Else
                        ShowError(EpError.Generic)
                    End If

                Case "moveDown"
                    If ServiceEP.MovePathDisplayOrderAfter(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
                        RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage))
                    Else
                        ShowError(EpError.Generic)
                    End If

                Case "visibility"
                    Select Case ServiceEP.UpdatePathVisibilityStatus(e.CommandArgument, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)
                        Case 1
                            RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage))
                        Case 0
                            ShowError(EpError.Data)
                        Case 2
                            LTalert.Text = Resource.getValue("MSGalert")
                        Case 3
                            LTalert.Text = Resource.getValue("EpError.EmptyActivities")
                    End Select

                    'da togliere
                Case "delStat"
                    Dim idPath = CLng(e.CommandArgument)
                    Dim setting As lm.Comol.Modules.EduPath.Domain.dtoConfigurationSetting
                    setting = ServiceEP.GetConfigurationSetting(idPath, CurrentCommunityID, ConfigurationType.Module)


                    If setting.AllowDeleteStatistics Then
                        If setting.AllowDeleteFullStatistics Then
                            ServiceEP.ServiceStat.DeleteAllEpStat(idPath)
                            Dim items As List(Of Long) = Me.ServiceEP.GetSubactiviesIdLinkedObjects(idPath, SubActivityType.Quiz)
                            If items.Count > 0 Then
                                Dim s As New COL_Questionario.Business.ServiceQuestionnaire(PageUtility.CurrentContext)
                                For Each idQuestionnaire As Long In items
                                    s.DeleteStatistics(idQuestionnaire)
                                Next
                            End If
                            ServiceCF.DeleteUsersCertifications(idPath, ServiceEP.ServiceModuleID(), COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex, True)
                            RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage))
                        Else
                            Me.LTidPath.Text = idPath
                            CBLusers.DataSource = ServiceEP.GetUsersByPermission(CurrentCommunityID, Services_EduPath.Base2Permission.AdminService)
                            CBLusers.DataTextField = "Displayname"
                            CBLusers.DataValueField = "Id"
                            CBLusers.DataBind()

                            For Each item As ListItem In CBLusers.Items
                                item.Attributes.Add("class", "item")
                            Next
                            Me.UDPperson.Update()
                            Me.LTscriptOpen.Visible = True
                        End If
                    Else
                        RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage))
                    End If

                    'If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random OrElse Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                    '    DALRisposte.cancellaRisposteBYIDQuestionarioRandom(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id)
                    'Else
                    '    DALRisposte.cancellaRisposteBYIDQuestionario(Me.QuestionarioCorrente.id, Me.UtenteCorrente.ID)
                    'End If



                    'fine da togliere
            End Select
        Catch ex As Exception
            Dim a = ex
            ShowError(EpError.Generic)
        End Try
    End Sub

    Protected Function CssIsBlocked(dtoItem As dtoEPitemList) As String
        Dim isBlocked As Boolean = ServiceEP.CheckStatus(dtoItem.Status, Status.Locked)

        'Dim oPermission As PermissionEP = ServiceEP.GetUserPermission_ByPath(dtoItem.Id, CurrentUserId, CurrentCommRoleID)
        'Dim onlystats As Boolean = oPermission.Read = False And oPermission.ViewOwnStat = True
        Dim onlystats As Boolean = dtoItem.PermissionEP.Read = False And dtoItem.PermissionEP.ViewOwnStat = True

        If isBlocked Then
            Return " blocked"
        Else
            If onlystats Then
                Return " onlystats"
            Else
                Return ""
            End If
        End If
    End Function

    Protected Function IsDefaultPath(dtoitem As dtoEPitemList) As String
        If dtoitem.isDefault Then
            Return " default"
        Else
            Return ""
        End If
    End Function

    Protected Function DescriptionIsEmpty(description As String) As String
        If String.IsNullOrEmpty(description) Then
            Return " empty"
        Else
            Return ""
        End If
    End Function

    Private Sub RPeduPathList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPeduPathList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oLKBdefault, oLKBdelete, oLKBblocked As LinkButton
            Dim oHyp, oHypStatistic As HyperLink
            Dim oLb As Label
            Dim dtoItem As dtoEPitemList = e.Item.DataItem

            Dim oBtn As Button = e.Item.FindControl("BTNdeleteStat")
            Dim setting As lm.Comol.Modules.EduPath.Domain.dtoConfigurationSetting
            setting = ServiceEP.GetConfigurationSetting(dtoItem.Id, CurrentCommunityID, ConfigurationType.Module)

            If ViewModeType = EpViewModeType.Manage OrElse Not setting.AllowDeleteStatistics Then


                'da togliere
                Me.Resource.setButton(oBtn, True, False, True)
                If ServiceEP.CheckRoleEp(ServiceEP.GetUserRole_ByPath(dtoItem.Id, CurrentUserId, CurrentCommRoleID), RoleEP.Manager) Then

                    oBtn.CommandArgument = dtoItem.Id
                Else
                    hideControl(oBtn)
                End If
            Else
                hideControl(oBtn)
                'fine da togliere
            End If
            Dim isPlayMode As Boolean = ServiceEP.CheckEpType(dtoItem.type, EPType.PlayMode)

            If Me.CanSwichEduPaths Then
                Dim SPANupdown As HtmlControl = e.Item.FindControl("SPANupdown")
                SPANupdown.Visible = DragDrop 'And Not dtoItem.isDefault


                Dim oLKBmove As LinkButton
                oLKBmove = e.Item.FindControl("LKBup")
                Me.Resource.setLinkButton(oLKBmove, False, True)
                oLKBmove.CommandArgument = dtoItem.Id
                oLKBmove.Visible = Not DragDrop And Not dtoItem.isDefault

                oLKBmove = e.Item.FindControl("LKBdown")
                Me.Resource.setLinkButton(oLKBmove, False, True)
                oLKBmove.CommandArgument = dtoItem.Id
                oLKBmove.Visible = Not DragDrop And Not dtoItem.isDefault
            End If

            oLb = e.Item.FindControl("LBdetail")
            Me.Resource.setLabel(oLb)

            oLb = e.Item.FindControl("LBdescription")
            oLb.Text = SmartTagsAvailable.TagAll(dtoItem.Description)

            Select Case Me.ViewModeType
                Case EpViewModeType.Manage

                    oHyp = e.Item.FindControl("HYPeduPath")

                    oHyp.Visible = True
                    oHyp.Text = dtoItem.Name
                    oHyp.NavigateUrl = Me.BaseUrl & RootObject.PathView(dtoItem.Id, Me.CurrentCommunityID, Me.ViewModeType, isPlayMode)


                    oHypStatistic = e.Item.FindControl("HYPstatistic")
                    Me.Resource.setHyperLink(oHypStatistic, False, True)
                    'oHypStatistic.ImageUrl = RootObject.ImgStat(Me.BaseUrl)
                    oHypStatistic.Text = ""
                    oHypStatistic.Visible = dtoItem.PermissionEP.ViewUserStat
                    oHypStatistic.NavigateUrl = Me.BaseUrl & RootObject.PathStatistics(dtoItem.Id, Me.CurrentCommunityID, ServiceEP.GetEpEndDate(dtoItem.Id), False)

                    If MyBase.Permission.Admin Then

                        If dtoItem.isDefault Then
                            Dim oImgDefault As HtmlControl = e.Item.FindControl("IMGdefault")
                            'oImgDefault.ImageUrl = RootObject.ImgDefault(Me.BaseUrl)
                            oImgDefault.Visible = True
                            oImgDefault.Attributes("title") = Me.Resource.getValue("IsDefault")
                        Else
                            oLKBdefault = e.Item.FindControl("LKBdefault")
                            Me.Resource.setLinkButton(oLKBdefault, False, True)
                            oLKBdefault.CommandArgument = dtoItem.Id
                            'oLKBdefault.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                            oLKBdefault.Visible = True
                            oLKBdefault.Text = ""
                            'oLKBdefault.CssClass = "icon makedefault"

                            'oLKBdefault.Text = String.Format(oLKBdefault.Text, RootObject.ImgSetDefault(Me.BaseUrl), oLKBdefault.ToolTip)
                        End If
                    Else
                        If dtoItem.isDefault Then
                            Dim oImg As HtmlControl
                            oImg = e.Item.FindControl("IMGdefault")
                            'oImg.ImageUrl = RootObject.ImgDefault(Me.BaseUrl)
                            oImg.Visible = True
                            oImg.Attributes("title") = Me.Resource.getValue("IsDefault")
                        End If
                    End If

                    If dtoItem.PermissionEP.Update Then 'dtoItem.isModificable AndAlso dtoItem.PermissionEP.Update Then 'il path puo' sempre essere modificato!
                        oHyp = e.Item.FindControl("HYPupdate")
                        Me.Resource.setHyperLink(oHyp, False, True)
                        oHyp.Text = ""
                        oHyp.NavigateUrl = Me.BaseUrl & RootObject.PathManagement(Me.CurrentCommunityID, dtoItem.Id, StepPathManagement.Update, dtoItem.type)
                        'oHyp.ImageUrl = RootObject.ImgModifyMedium(BaseUrl)
                        oHyp.Visible = True

                        oLKBblocked = e.Item.FindControl("LKBvisibility")
                        oLKBblocked.Visible = True
                        oLKBblocked.Text = ""
                        'oLKBblocked.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
                        If Not ServiceEP.CheckStatus(dtoItem.Status, Status.Locked) Then
                            '    oLKBblocked.Text = String.Format(oLKBblocked.Text, RootObject.ImgBtnBlocked_On(Me.BaseUrl), oLKBblocked.ToolTip)
                            oLKBblocked.CssClass = "icon unlocked"
                        Else
                            oLKBblocked.CssClass = "icon locked"
                            '    oLKBblocked.Text = String.Format(oLKBblocked.Text, RootObject.ImgBtnBlocked_Off(Me.BaseUrl), oLKBblocked.ToolTip)
                        End If
                        oLKBblocked.CommandArgument = dtoItem.Id

                    End If

                    If dtoItem.PermissionEP.Delete Then
                        oLKBdelete = e.Item.FindControl("LKBdelete")
                        oLKBdelete.CommandArgument = dtoItem.Id
                        Me.Resource.setLinkButton(oLKBdelete, False, True, , True)
                        oLKBdelete.Text = ""
                        oLKBdelete.Visible = True
                    End If

                    If dtoItem.PermissionEP.Evaluate AndAlso ServiceEP.ExistItemToEvaluate(dtoItem.Id, dtoItem.type) Then
                        oHyp = e.Item.FindControl("HYPevaluate")
                        oHyp.ToolTip = MyBase.Resource.getValue("Evaluate")
                        oHyp.Visible = True
                        oHyp.Text = ""
                        oHyp.NavigateUrl = Me.BaseUrl & RootObject.EvaluateGlobal(dtoItem.Id, Me.CurrentCommunityID, DateTime.Now)
                        oHyp.Enabled = ServiceAssignment.ExistActsEvaluableAnalogic(dtoItem.Id, CurrentUserId, Me.CurrentCommRoleID)
                        'oHyp.ImageUrl = RootObject.ImgEvaluate(Me.BaseUrl)
                    End If

                    Dim SPANUnitSep As HtmlControl = e.Item.FindControl("SPANUnitSep")
                    Dim SPANActivitySep As HtmlControl = e.Item.FindControl("SPANActivitySep")

                    If (dtoItem.UnitToManage > 0) Then
                        oHyp = e.Item.FindControl("HYPunitToMan")
                        oHyp.Text = Me.Resource.getValue("ToManage") & dtoItem.UnitToManage
                        oHyp.NavigateUrl = Me.BaseUrl & RootObject.PathView(dtoItem.Id, Me.CurrentCommunityID, Me.ViewModeType, isPlayMode)
                        oHyp.Visible = True




                    Else
                        oLb = e.Item.FindControl("LBunitToMan")
                        oLb.Text = Me.Resource.getValue("ToManage") & "0"
                        oLb.Visible = True

                    End If

                    If Not ServiceEP.CheckEpType(dtoItem.type, EPType.TimeAuto) Then
                        If (dtoItem.UnitToEvaluate > 0) Then
                            oHyp = e.Item.FindControl("HYPunitToEval")
                            oHyp.Text = Me.Resource.getValue("ToEvaluate") & dtoItem.UnitToEvaluate
                            oHyp.NavigateUrl = Me.BaseUrl & RootObject.PathView(dtoItem.Id, Me.CurrentCommunityID, Me.ViewModeType, isPlayMode)
                            oHyp.Visible = True
                        Else
                            oLb = e.Item.FindControl("LBunitToEval")
                            oLb.Text = Me.Resource.getValue("ToEvaluate") & "0"
                            oLb.Visible = True
                        End If

                        If (dtoItem.ActivityToEvaluate > 0) Then
                            oHyp = e.Item.FindControl("HYPactivityTOEval")
                            oHyp.Text = Me.Resource.getValue("ToEvaluate") & dtoItem.ActivityToEvaluate
                            oHyp.NavigateUrl = Me.BaseUrl & RootObject.PathView(dtoItem.Id, Me.CurrentCommunityID, Me.ViewModeType, isPlayMode)
                            oHyp.Visible = True
                        Else
                            oLb = e.Item.FindControl("LBactivityTOEva")
                            oLb.Text = Me.Resource.getValue("ToEvaluate") & "0"
                            oLb.Visible = True
                        End If
                    Else
                        SPANUnitSep.Visible = False
                        SPANActivitySep.Visible = False
                    End If

                    If (dtoItem.ActivityToManage > 0) Then
                        oHyp = e.Item.FindControl("HYPactivityToMan")
                        oHyp.Text = Me.Resource.getValue("ToManage") & dtoItem.ActivityToManage
                        oHyp.NavigateUrl = Me.BaseUrl & RootObject.PathView(dtoItem.Id, Me.CurrentCommunityID, Me.ViewModeType, isPlayMode)
                        oHyp.Visible = True
                    Else
                        oLb = e.Item.FindControl("LBactivityToMan")
                        oLb.Text = Me.Resource.getValue("ToManage") & "0"
                        oLb.Visible = True
                    End If

                    oLb = e.Item.FindControl("LBactivity")
                    Me.Resource.setLabel(oLb)
                    oLb = e.Item.FindControl("LBunit")
                    Me.Resource.setLabel(oLb)
                    'move.Visible = True

                Case EpViewModeType.View

                    Dim oPermission As PermissionEP = ServiceEP.GetUserPermission_ByPath(dtoItem.Id, CurrentUserId, CurrentCommRoleID)
                    Dim onlystats As Boolean = oPermission.Read = False And oPermission.ViewOwnStat = True

                    Dim div As HtmlControl
                    div = e.Item.FindControl("DIVmanageUnits")
                    div.Visible = False
                    div = e.Item.FindControl("DIVmanageActivities")
                    div.Visible = False

                    Dim oImg As HtmlControl

                    If dtoItem.isDefault Then
                        oImg = e.Item.FindControl("IMGdefault")
                        'oImg.ImageUrl = RootObject.ImgDefault(Me.BaseUrl)
                        oImg.Visible = True
                        oImg.Attributes("title") = Me.Resource.getValue("IsDefault")
                    End If



                    Dim isBlocked As Boolean = ServiceEP.CheckStatus(dtoItem.Status, Status.Locked)
                    Dim ospan As HtmlControl
                    ospan = e.Item.FindControl("IMGblocked")
                    ospan.Visible = True
                    ospan.Attributes("title") = Me.Resource.getValue("Blocked." & isBlocked)
                    'ospan.ImageUrl = RootObject.ImgItemBlocked(Me.BaseUrl, Not isBlocked)
                    If isBlocked Then
                        ospan.Attributes("class") = "icon locked"
                    Else
                        ospan.Attributes("class") = "icon unlocked"
                    End If


                    oLb = e.Item.FindControl("LBeduPath") 'se non e' visibile sempre sparisce lo span con il pathtile e quindi saltano gli stili.
                    oLb.Visible = True
                    If isBlocked Then
                        oLb.Text = dtoItem.Name
                        hideControl(e.Item.FindControl("HYPeduPath"))

                        hideControl(e.Item.FindControl("HYPstart"))

                    Else

                        If onlystats Then
                            oLb.Text = dtoItem.Name
                            hideControl(e.Item.FindControl("HYPeduPath"))

                            hideControl(e.Item.FindControl("HYPstart"))
                        Else

                            oHyp = e.Item.FindControl("HYPeduPath")
                            oHyp.Visible = True
                            oHyp.Text = dtoItem.Name
                            oHyp.NavigateUrl = Me.BaseUrl & RootObject.PathView(dtoItem.Id, Me.CurrentCommunityID, Me.ViewModeType, isPlayMode)

                            oLb.Text = " "
                            oHyp = e.Item.FindControl("HYPstart")
                            Me.Resource.setHyperLink(oHyp, False, True)
                            'oHyp.ImageUrl = RootObject.ImgPlay(Me.BaseUrl)
                            oHyp.NavigateUrl = Me.BaseUrl & RootObject.PathView(dtoItem.Id, Me.CurrentCommunityID, Me.ViewModeType, isPlayMode)
                            oHyp.Visible = True

                        End If



                        oHypStatistic = e.Item.FindControl("HYPstatistic")
                        Me.Resource.setHyperLink(oHypStatistic, False, True)
                        oHypStatistic.Text = ""
                        'oHypStatistic.ImageUrl = RootObject.ImgStat(Me.BaseUrl)
                        oHypStatistic.Visible = True
                        oHypStatistic.NavigateUrl = Me.BaseUrl & RootObject.UserStatisticsView(dtoItem.Id, CurrentCommunityID, DateTime.Now, False)
                    End If

            End Select

        End If
    End Sub


    Public Sub RBLvisibility_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim EPList As New List(Of dtoEPitemList)
        EPList = ServiceEP.GetMyEduPaths(Me.CurrentContext.UserContext.CurrentUserID, CurrentCommunityID, CurrentCommRoleID, Me.ViewModeType)

        If RBLvisibility.SelectedValue = "AllEp" Then
            Me.EpCount = CType(EPList.Count, Integer)
            Me.CanSwichEduPaths = EpCount > 1 AndAlso (MyBase.Permission.Admin OrElse Me.ServiceEP.GetIfCanChangePathsDisplayOrder(EPList, Me.CurrentUserId, Me.CurrentCommRoleID))
            Me.RPeduPathList.DataSource = EPList

        ElseIf RBLvisibility.SelectedValue = "VisibleEp" Then
            Me.CanSwichEduPaths = False
            EPList = ServiceEP.GetVisibleEp(EPList, Me.CurrentUserId, Me.CurrentCommRoleID)
            Me.EpCount = CType(EPList.Count, Integer)

            Me.RPeduPathList.DataSource = EPList
        End If

        Me.RPeduPathList.DataBind()
    End Sub
#Region "GESTIONE DIALOG ATTUALMENTE ELIMINATE... IN CASO DI MODIFICA GESTIVANO SE AGGIORNARE LE STATISTICHE O PERSONALIZZARE GLI ASSEGNAMENTI DI CHI HA GIA' INIZIATO IL PERCORSO"

    Private Sub InitDialog()
        'Me.DLGvisibility.DialogTitle = Me.Resource.getValue("DLGvisibilityTitle")
        'Me.DLGvisibility.DialogText = Me.Resource.getValue("DLGvisibilityText")
        'Dim visibilityOptions As New List(Of String)
        'visibilityOptions.Add(Me.Resource.getValue("DLGvisibilityDoNotPersonalizeAss"))
        'visibilityOptions.Add(Me.Resource.getValue("DLGvisibilitypersonalizeAss"))
        'Me.DLGvisibility.DataSource = visibilityOptions
        'Me.DLGvisibility.DataBind()
    End Sub

    'Private Sub DLGvisibility_ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGvisibility.ButtonPressed
    '    If dialogResult >= 0 Then
    '        If ServiceEP.UpdatePathVisibilityStatus(CommandArgument, dialogResult = 1, Me.CurrentUserId, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress) Then
    '            RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage))
    '        Else
    '            ShowError(EpError.Generic)
    '        End If
    '    End If
    'End Sub
#End Region

    Private Sub LNBSwitchView_Click(sender As Object, e As System.EventArgs) Handles LNBSwitchView.Click
        Dim url As String = Request.Url.ToString()
        If Request.QueryString("ViewMode") = 1 Then

            url = url.Replace("ViewMode=1", "ViewMode=2")

        Else
            url = url.Replace("ViewMode=2", "ViewMode=1")
        End If

        Response.Redirect(url)

    End Sub

    Private Sub BTNclearStatistics_Click(sender As Object, e As System.EventArgs) Handles BTNclearStatistics.Click
        Me.LTscriptOpen.Visible = False
        If IsNumeric(Me.LTidPath.Text) Then
            Dim idPath As Long = CLng(Me.LTidPath.Text)
            Dim setting As lm.Comol.Modules.EduPath.Domain.dtoConfigurationSetting
            setting = ServiceEP.GetConfigurationSetting(idPath, CurrentCommunityID, ConfigurationType.Module)
            If setting.AllowDeleteStatistics Then
                Dim items As List(Of Long) = Me.ServiceEP.GetSubactiviesIdLinkedObjects(idPath, SubActivityType.Quiz)
                Dim idModule As Integer = ServiceEP.ServiceModuleID()
                For Each item As ListItem In (From i As ListItem In CBLusers.Items Where i.Selected Select i).ToList
                    ServiceEP.ServiceStat.DeleteAllEpStat(idPath, CInt(item.Value))
                    ServiceCF.DeleteUsersCertifications(idPath, idModule, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex, CInt(item.Value), True)
                    If items.Count > 0 Then
                        Dim s As New COL_Questionario.Business.ServiceQuestionnaire(PageUtility.CurrentContext)
                        For Each idQuestionnaire As Long In items
                            s.DeleteStatistics(idQuestionnaire, CInt(item.Value))
                        Next
                    End If
                Next
            End If
        End If
        RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage))
    End Sub

    Private Sub BTNundoClearStatistics_Click(sender As Object, e As System.EventArgs) Handles BTNundoClearStatistics.Click
        RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage))
    End Sub

    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
        CTRLmoduleStatusMessage.Visible = True
        CTRLmoduleStatusMessage.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
        Me.Master.ServiceTitle = Me.Resource.getValue("LBtitolo.text") & ServiceEP.GetCommunityName(Me.CurrentCommunityID)
        MLVeduPathList.SetActiveView(VIWmessages)
        CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property
End Class