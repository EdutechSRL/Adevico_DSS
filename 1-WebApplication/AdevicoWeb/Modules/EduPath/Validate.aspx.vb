Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Domain.DTO
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel


Public Class Validate
    Inherits PageBaseEduPath

#Region "InitStandard"

    Private _Service As Services_EduPath
    Private _TempUnitId As Long
    Private _PlayerCssVisibility As String
    Private _PlayerUnitId As Long
    Private _VisibleActivity As IList(Of Int64)
    Private _VisibleUnit As IList(Of Int64)

    Private Property VisibleActivity As IList(Of Int64)
        Get
            Return _VisibleActivity
        End Get
        Set(ByVal value As IList(Of Int64))
            _VisibleActivity = value
        End Set
    End Property

    Private Property VisibleUnit As IList(Of Int64)
        Get
            Return _VisibleUnit
        End Get
        Set(ByVal value As IList(Of Int64))
            _VisibleUnit = value
        End Set
    End Property

    Protected Overrides ReadOnly Property PathType As EPType
        Get
            If _PathType = lm.Comol.Modules.EduPath.Domain.EPType.None Then
                _PathType = ServiceEP.GetEpType(CurrentPathId, ItemType.Path)
            End If
            Return _PathType
        End Get
    End Property

    Private Property PlayerUnitId As Long
        Get
            Return _PlayerUnitId
        End Get
        Set(ByVal value As Long)
            _PlayerUnitId = value
        End Set
    End Property

#End Region

#Region " Base"


    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Access, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, Me.CurrentPathId), InteractionType.UserWithLearningObject)

        EvaluatedItems = ServiceEP.GetEpStructure_toValidate(CurrentPathId, CurrentUserId, CurrentCommRoleID)
        initPageValidate()

    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.ShowError(EpError.NotPermission)
    End Sub

    Public Overrides Function HasPermessi() As Boolean

        Return ServiceEP.CheckCommunityId(Of Path)(Me.CurrentPathId, Me.CurrentCommunityID) AndAlso Not (lm.Comol.Modules.EduPath.Domain.ItemType.Unit = ItemType AndAlso isAutoEp) AndAlso ServiceEP.GetUserPermission_ByPath(CurrentPathId, Me.CurrentUserId, Me.CurrentCommRoleID).Evaluate

    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EpView", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(Me.LKBreset, False, True)
            .setLinkButton(Me.LKBeduPathView, False, True)
            .setLinkButton(Me.LKBsave, False, True)
            .setLabel(Me.LBunitList)
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

    Public Function UserIsManager() As Boolean
        Dim roleEp As RoleEP = ServiceEP.GetUserRole_ByPath(CurrentPathId, CurrentUserId)
        Return roleEp = lm.Comol.Modules.EduPath.Domain.RoleEP.Manager
    End Function


#Region "Property"


    Private _unitId As Int16
    Private _isEditablePath As Boolean
    Private _SmartTagsAvailable As SmartTags
    Private _unitDtoIndex As Int16


    Public Property TempUnitId() As Long
        Set(ByVal value As Long)
            _TempUnitId = value
        End Set
        Get
            Return _TempUnitId
        End Get
    End Property

    Public ReadOnly Property CurrentPathId() As Long
        Get
            Dim qs_pathId As String = Me.Request.QueryString("PId")
            If IsNumeric(qs_pathId) Then
                Return qs_pathId
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property CurrentUserId() As Integer
        Get
            Return Me.CurrentContext.UserContext.CurrentUserID
        End Get
    End Property
    Private ReadOnly Property CurrentCommRoleID As Integer
        Get
            Return UtenteCorrente.GetIDRuoloForComunita(CurrentCommunityID)
        End Get
    End Property

    Private Property EvaluatedItems As dtoEpStructureValidate
        Get
            Return Session("EvaluatedItems" & SessionUniqueKey)
        End Get
        Set(ByVal value As dtoEpStructureValidate)
            Session("EvaluatedItems" & SessionUniqueKey) = value
        End Set
    End Property


    Private ReadOnly Property ItemType As ItemType
        Get
            Dim qs_ItemType As String = Me.Request.QueryString("It")
            If IsNumeric(qs_ItemType) AndAlso (qs_ItemType = ItemType.Activity OrElse qs_ItemType = ItemType.Unit) Then
                Return qs_ItemType
            Else
                Return ItemType.Activity
            End If
        End Get
    End Property

    Private ReadOnly Property SessionUniqueKey As String
        Get
            Return "_" & Me.CurrentUserId & "_" & Me.CurrentPathId
        End Get
    End Property

#End Region

    Public Function GetActivityCssClass(ByRef status As Status) As String
        If ServiceEP.CheckStatus(status, status.Text) Then
            Return " note "
        Else
            Return " activity "
        End If
    End Function


    Private tempActivityRule As Long = 0

    Private _TempSelected As String

    Public Property TempSelected As String
        Get
            Return _TempSelected
        End Get
        Set(ByVal value As String)
            _TempSelected = value
        End Set
    End Property

    Private Sub InitHyperlink()
        Me.HYPerror.NavigateUrl = Me.BaseUrl & RootObject.PathView(CurrentPathId, Me.CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(CurrentPathId))

        If Not isAutoEp Then
            If ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.Unit Then
                Me.HYPswitchMode.NavigateUrl = Me.BaseUrl & RootObject.Validate(Me.CurrentPathId, Me.CurrentCommunityID, lm.Comol.Modules.EduPath.Domain.ItemType.Activity)

                Me.HYPswitchMode.Text = Resource.getValue("SwitchMode.Activity")
                Me.HYPswitchMode.ToolTip = Resource.getValue("SwitchMode.Activity")

            Else
                Me.HYPswitchMode.NavigateUrl = Me.BaseUrl & RootObject.Validate(Me.CurrentPathId, Me.CurrentCommunityID, lm.Comol.Modules.EduPath.Domain.ItemType.Unit)
                Me.HYPswitchMode.Text = Resource.getValue("SwitchMode.Unit")
                Me.HYPswitchMode.ToolTip = Resource.getValue("SwitchMode.Unit")
            End If
            Me.HYPswitchMode.Visible = True
        End If
    End Sub

    Private Sub initPageValidate()

        If IsNothing(EvaluatedItems) Then
            Me.ShowError(EpError.NotPermission)
        Else

            Dim userId As Int32 = Me.CurrentUserId
            Dim pathId As Int64 = Me.CurrentPathId

            'unit rules
            Dim unit As IList(Of dtoUnitUser) = ServiceEP.GetFreeUnitsByPathId_Validate(EvaluatedItems)

            Dim rulesUnit As IList(Of RuleUnitCompletion) = ServiceEP.MergeUnitRulesWithUserCompletion_Validate(EvaluatedItems)

            Dim engineUnit As New RuleEngine(Of dtoUnitUser)

            For Each rule As RuleUnitCompletion In rulesUnit
                engineUnit.AddRule(rule)
            Next

            Dim resultUnit As IList(Of KeyValuePair(Of dtoUnitUser, RuleEngineResult(Of dtoUnitUser))) = engineUnit.ExecuteFromTopByRoots(unit)
            If IsNothing(resultUnit) Then
                resultUnit = New List(Of KeyValuePair(Of dtoUnitUser, RuleEngineResult(Of dtoUnitUser)))
            End If
            VisibleUnit = (From item In resultUnit Select item.Key.Id).ToList()

            'end unit rules

            ' act regole
            Dim act As IList(Of dtoActivityUser) = ServiceEP.GetFreeActivitiesByPathId_Validate(EvaluatedItems)

            Dim rules As IList(Of RuleActivityCompletion) = ServiceEP.MergeActivityRulesWithUserCompletion_Validate(EvaluatedItems)

            Dim engine As New RuleEngine(Of dtoActivityUser)

            For Each rule As RuleActivityCompletion In rules
                engine.AddRule(rule)
            Next
            Dim result As IList(Of KeyValuePair(Of dtoActivityUser, RuleEngineResult(Of dtoActivityUser))) = engine.ExecuteFromTopByRoots(act)
            If result IsNot Nothing Then
                VisibleActivity = (From item In result Select item.Key.Id).ToList()
            End If
            'fine regole


            InitHyperlink()



            InitProgressBar(EvaluatedItems)

            Me.Master.ServiceTitle = EvaluatedItems.Name

            If VisibleUnit Is Nothing Then
                RPunit.DataSource = EvaluatedItems.Units
            Else
                RPunit.DataSource = (From item In EvaluatedItems.Units Where VisibleUnit.Contains(item.Id)).ToList()
            End If
            RPunit.DataBind()

        End If
    End Sub

    Private Sub InitProgressBar(ByRef dto As dtoEpStructureValidate)

        'If isAutoEp Then
        '    Dim statForBar As dtoStatWithWeight = ServiceEP.ServiceStat.GetPassedCompletedWeight_byActivity(Me.CurrentPathId, Me.CurrentUserId)
        ' Me.CTRLprogressBar.InitBar(statForBar.Completion, statForBar.MinCompletion, "")
        Me.CTRLprogressBar.InitBar(dto.Completion, dto.MinCompletion, Me.Resource.getValue("ExecutedWeight") & (dto.Weight * 100 / dto.Completion))
        'Else
        '    Dim statForBar As dtoStatWithWeight = ServiceEP.ServiceStat.GetPassedCompletedWeight_byUnit(Me.CurrentPathId, Me.CurrentUserId)
        '    ' Me.CTRLprogressBar.InitBar(statForBar.Completion, statForBar.MinCompletion, Me.Resource.getValue("ExecutedWeight") & statForBar.UserTotWeight)
        '    Me.CTRLprogressBar.InitBar(dto.Completion, dto.MinCompletion, "")
        'End If

    End Sub

    Private Sub ShowError(ByVal ErrorType As EpError)
        Me.Resource.setHyperLink(Me.HYPerror, False, True)
        'Me.HYPerror.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, Me.ViewModeType)
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
            Case EpError.PathNotFind
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Url.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)

        End Select
        Me.MLVeduPathView.ActiveViewIndex = 1
    End Sub

    Private Sub SetImgStatusMedium(ByRef oImg As System.Web.UI.WebControls.Image, ByRef status As StatusStatistic)
        oImg.Visible = True
        Dim tooltip As String
        Select Case status
            Case StatusStatistic.CompletedPassed
                oImg.ImageUrl = RootObject.ImgStatusGreenMedium(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.CompletedPassed.ToString)

            Case StatusStatistic.Browsed
                oImg.ImageUrl = RootObject.ImgStatusGreyMedium(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
            Case StatusStatistic.None
                oImg.ImageUrl = RootObject.ImgStatusGreyMedium(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
            Case Else
                oImg.ImageUrl = RootObject.ImgStatusYellowMedium(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Started.ToString)
        End Select
        oImg.AlternateText = tooltip
        oImg.ToolTip = tooltip
    End Sub

    Private Sub SetImgStatusSmall(ByRef oImg As System.Web.UI.WebControls.Image, ByRef status As StatusStatistic)
        oImg.Visible = True
        Dim tooltip As String
        Select Case status
            Case StatusStatistic.CompletedPassed
                oImg.ImageUrl = RootObject.ImgStatusGreenSmall(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.CompletedPassed.ToString)
            Case StatusStatistic.Browsed
                oImg.ImageUrl = RootObject.ImgStatusGreySmall(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
            Case StatusStatistic.None
                oImg.ImageUrl = RootObject.ImgStatusGreySmall(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Browsed.ToString)
            Case Else
                oImg.ImageUrl = RootObject.ImgStatusYellowSmall(Me.BaseUrl)
                tooltip = Resource.getValue("StatusStatistic." & StatusStatistic.Started.ToString)
        End Select
        oImg.AlternateText = tooltip
        oImg.ToolTip = tooltip
    End Sub

    Private Sub GetEvaluatedItems_TimeBased()
        Dim dtoUnit As dtoUnitStructureValidate
        Dim dtoAct As dtoActStructureValidate
        Dim oTxb As TextBox
        Dim oLb As Label


        For Each item As RepeaterItem In RPunit.Items

            oLb = item.FindControl("LBunit")
            dtoUnit = (From u In EvaluatedItems.Units
                      Where u.Id = oLb.Attributes("Id")
                      Select u).FirstOrDefault()
            oTxb = item.FindControl("TXBCompletion")

            If ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.Unit Then

                dtoUnit.Completion = oTxb.Text

                oTxb = item.FindControl("TXBmark")

                dtoUnit.Mark = oTxb.Text

                ServiceEP.ServiceStat.GetValidateUnitStat(dtoUnit, isAutoEp)

            Else

                Dim oRpAct As Repeater = item.FindControl("RPactivity")
                For Each itemAct As RepeaterItem In oRpAct.Items

                    oLb = itemAct.FindControl("LBactName")


                    dtoAct = (From a In dtoUnit.Activities
                             Where a.Id = oLb.Attributes("Id")
                             Select a).FirstOrDefault

                    oTxb = itemAct.FindControl("TXBCompletion")
                    If Not Int64.TryParse(oTxb.Text, dtoAct.Completion) Then
                        dtoAct.Completion = 0
                    End If


                    If Not isAutoEp Then
                        oTxb = itemAct.FindControl("TXBmark")
                        dtoAct.Mark = oTxb.Text
                    End If

                    ServiceEP.ServiceStat.GetValidateActStat(dtoAct, isAutoEp)

                Next

                ServiceEP.ServiceStat.GetUnitStatValidate(dtoUnit, isAutoEp)

            End If
        Next
        ServiceEP.ServiceStat.GetValidateEpStat(EvaluatedItems, isAutoEp)

    End Sub

    Private Sub GetEvaluatedItems_MarkBased()
        Dim dtoUnit As dtoUnitStructureValidate
        Dim dtoAct As dtoActStructureValidate
        Dim oTxb As TextBox
        Dim oLb As Label

        If EvaluatedItems.Weight = 0 Then
            EvaluatedItems.Weight = 1
        End If

        For Each item As RepeaterItem In RPunit.Items


            oLb = item.FindControl("LBunit")
            dtoUnit = (From u In EvaluatedItems.Units Where u.Id = oLb.Attributes("Id") Select u).FirstOrDefault

            If ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.Unit Then
                oTxb = item.FindControl("TXBCompletion")

                dtoUnit.Completion = oTxb.Text

                oTxb = item.FindControl("TXBmark")

                dtoUnit.Mark = oTxb.Text

                ServiceEP.ServiceStat.GetValidateUnitStat(dtoUnit, isAutoEp)

            Else

                Dim oRpAct As Repeater = item.FindControl("RPactivity")
                For Each itemAct As RepeaterItem In oRpAct.Items

                    oLb = itemAct.FindControl("LBactName")

                    dtoAct = (From a In dtoUnit.Activities Where a.Id = oLb.Attributes("Id") Select a).FirstOrDefault

                    oTxb = itemAct.FindControl("TXBCompletion")
                    dtoAct.Completion = oTxb.Text

                    If Not isAutoEp Then
                        oTxb = itemAct.FindControl("TXBmark")
                        dtoAct.Mark = oTxb.Text
                    End If

                    ServiceEP.ServiceStat.GetValidateActStat(dtoAct, isAutoEp)

                Next

                ServiceEP.ServiceStat.GetUnitStatValidate(dtoUnit, isAutoEp)

            End If
        Next

        ServiceEP.ServiceStat.GetValidateEpStat(EvaluatedItems, isAutoEp)


    End Sub


    Private Sub GetEvaluatedItems()

        If isTimeEp Then
            GetEvaluatedItems_TimeBased()
        Else
            GetEvaluatedItems_MarkBased()
        End If

    End Sub


#Region "Repeater Item Data Bound"

    Private Function SetDateLabel(ByVal StartDate As Date?, ByVal EndDate As Date?) As String
        If Not StartDate Is Nothing Then

            If Not EndDate Is Nothing Then
                Return String.Format(Me.Resource.getValue("Between"), StartDate, EndDate)
            Else
                Return Me.Resource.getValue("After") & StartDate
            End If
        ElseIf Not EndDate Is Nothing Then
            Return Me.Resource.getValue("Before") & EndDate
        Else
            Return " - "
        End If
    End Function


    Private Sub SetImgBlocked(ByRef oImg As System.Web.UI.WebControls.Image, ByVal isBlocked As Boolean)
        oImg.Visible = True
        oImg.ImageUrl = RootObject.ImgItemBlocked(Me.BaseUrl, Not isBlocked)
        oImg.AlternateText = Me.Resource.getValue("Blocked." & isBlocked)
    End Sub

    Private Sub SetBlockedUnitBtn(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoUnit)
        Dim oDlkb As LinkButton
        oDlkb = e.Item.FindControl("LKBvisibUnit")
        Me.Resource.setLinkButton(oDlkb, False, True)
        oDlkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
        If Not ServiceEP.CheckStatus(dtoItem.Status, Status.Locked) Then
            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgBtnBlocked_On(Me.BaseUrl), oDlkb.ToolTip)
        Else
            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgBtnBlocked_Off(Me.BaseUrl), oDlkb.ToolTip)
        End If
        oDlkb.CommandArgument = dtoItem.Id
        'oDlkb.DialogClass = "visibilityUnitDial"
        oDlkb.Visible = True
    End Sub

    Private Sub RPunit_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPunit.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dtoItem As dtoUnitStructureValidate = e.Item.DataItem
            _unitId = dtoItem.Id

            Dim oLb As Label
            Dim RpActivities As Repeater

            Dim oImg As System.Web.UI.WebControls.Image
            oImg = e.Item.FindControl("IMGvisibility")
            SetImgBlocked(oImg, ServiceEP.CheckStatus(dtoItem.Status, Status.Locked))
            oImg = e.Item.FindControl("IMGmandatory")
            If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then
                oImg.ImageUrl = RootObject.ImgMandatoryMedium(Me.BaseUrl)
                oImg.ToolTip = Me.Resource.getValue("UnitMandatory")
                oImg.Visible = True
            Else
                oImg.ImageUrl = RootObject.ImgBlindMedium(Me.BaseUrl)
                oImg.Visible = True
            End If

            oLb = e.Item.FindControl("LBunitNumber")
            oLb.Visible = True
            oLb.Text = Me.Resource.getValue("Unit") & " " & e.Item.ItemIndex + 1 & ": "

            oLb = e.Item.FindControl("LBunit")
            oLb.Visible = True
            oLb.Text = dtoItem.Name
            oLb.Attributes("Id") = dtoItem.Id





            Dim isEditable As Boolean = ItemType.Unit = ItemType


            If Not isAutoEp Then

                SetImgStatusMedium(e.Item.FindControl("IMGstatus"), dtoItem.StatusStat)

                oLb = e.Item.FindControl("LBunitWeight")
                oLb.Visible = True
                If isMarkEp Then
                    oLb.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
                Else
                    oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
                End If


                Dim id As Long = dtoItem.Id

                oLb = e.Item.FindControl("LBcompletion")
                oLb.Text = String.Format(Me.Resource.getValue("LBcompletion.text"), dtoItem.MinCompletion)

                oLb.Visible = True

                Dim oTxb As TextBox = e.Item.FindControl("TXBCompletion")

                oTxb.Text = dtoItem.Completion
                oTxb.Enabled = isEditable
                oTxb.Visible = True

                oLb = e.Item.FindControl("LBmark")
                oLb.Text = String.Format(Me.Resource.getValue("LBmark.text"), dtoItem.MinMark)
                oLb.Visible = True

                oTxb = e.Item.FindControl("TXBmark")
                oTxb.Text = dtoItem.Mark

                oTxb.Enabled = isEditable
                oTxb.Visible = True
            End If

            If Not String.IsNullOrWhiteSpace(dtoItem.Description) Then
                Dim div As HtmlControl = e.Item.FindControl("DIVunitDescription")
                div.Visible = True
                oLb = e.Item.FindControl("LBunitDesc")
                oLb.Text = dtoItem.Description
            End If

            Me.TempUnitId = dtoItem.Id
            RpActivities = e.Item.FindControl("RPactivity")

            'from rule
            If (ItemType = ItemType.Activity) Then
                If VisibleActivity Is Nothing Then
                    RpActivities.DataSource = dtoItem.Activities
                Else
                    RpActivities.DataSource = (From item In dtoItem.Activities Where VisibleActivity.Contains(item.Id)).ToList()
                End If

                AddHandler RpActivities.ItemDataBound, AddressOf RPactivity_ItemDataBound
                RpActivities.DataBind()
            End If

            Dim Rulelist As List(Of dtoRule) = ServiceEP.GetUnitDtoRuleByUnitId(dtoItem.Id)

            If Rulelist.Count > 0 Then
                Dim RpRules As Repeater = e.Item.FindControl("RPunitrule")
                RpRules.DataSource = Rulelist
                AddHandler RpRules.ItemDataBound, AddressOf RPunitRule_ItemDataBound
                RpRules.DataBind()
            Else

                Dim ulRule As HtmlControl = e.Item.FindControl("ULunitRules")
                ulRule.Attributes("class") = "empty"
                ulRule.Visible = False
                hideControl(ulRule)
            End If

        End If
    End Sub

    Public Sub RPunitRule_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbl As Label
            Dim dtoItem As dtoRule = e.Item.DataItem
            lbl = e.Item.FindControl("LBunitrule")
            lbl.Text = dtoItem.Name

        End If
    End Sub

    Private Sub SetBlockedActivityBtn(ByRef e As System.Web.UI.WebControls.RepeaterItemEventArgs, ByRef dtoItem As dtoActivity)
        Dim oDlkb As LinkButton
        oDlkb = e.Item.FindControl("LKBvisibActivity")
        Me.Resource.setLinkButton(oDlkb, False, True)
        oDlkb.Text = "<img src=" & """" & "{0}" & """" & " alt=" & """" & "{1}" & """" & "/>"
        If Not ServiceEP.CheckStatus(dtoItem.Status, Status.Locked) Then
            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgBtnBlocked_On(Me.BaseUrl), oDlkb.ToolTip)
        Else
            oDlkb.Text = String.Format(oDlkb.Text, RootObject.ImgBtnBlocked_Off(Me.BaseUrl), oDlkb.ToolTip)
        End If
        oDlkb.CommandArgument = dtoItem.Id
        'oDlkb.DialogClass = "visibilityActDial"
        oDlkb.Visible = True
    End Sub


    Public Sub RPactivity_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dtoItem As dtoActStructureValidate = e.Item.DataItem

            Dim oLb As Label
            Dim RpRoles As Repeater

            Dim isBlocked As Boolean = ServiceEP.CheckStatus(dtoItem.Status, Status.Locked)

            Dim oImg As System.Web.UI.WebControls.Image
            oImg = e.Item.FindControl("IMGvisibility")
            SetImgBlocked(oImg, isBlocked)

            oLb = e.Item.FindControl("LBactName")
            SetLbActName(oLb, dtoItem.Name)
            oLb.Attributes("Id") = dtoItem.Id

            oImg = e.Item.FindControl("IMGmandatory")
            If ServiceEP.CheckStatus(dtoItem.Status, Status.Mandatory) Then

                oImg.ImageUrl = RootObject.ImgMandatorySmall(Me.BaseUrl)
                oImg.ToolTip = Me.Resource.getValue("ActivityMandatory")

            Else
                oImg.ImageUrl = RootObject.ImgBlindSmall(Me.BaseUrl)

            End If
            oImg.Visible = True

            oLb = e.Item.FindControl("LBactNumber")
            oLb.Text = Me.Resource.getValue("Activity") & " " & e.Item.ItemIndex + 1 & ": "


            oLb = e.Item.FindControl("LBactWeight")
            oLb.Visible = True
            If isMarkEp Then
                oLb.Text = " (" & dtoItem.Weight & Resource.getValue("Points")
            Else
                oLb.Text = ServiceEP.GetTime(dtoItem.Weight)
            End If

            SetImgStatusSmall(e.Item.FindControl("IMGstatus"), dtoItem.StatusStat)


            oLb = e.Item.FindControl("LBmark")
            oLb.Text = String.Format(Me.Resource.getValue("LBmark.text"), dtoItem.MinMark)

            oLb = e.Item.FindControl("LBcompletion")
            oLb.Text = String.Format(Me.Resource.getValue("LBcompletion.text"), dtoItem.MinCompletion)

            Dim id As Long = dtoItem.Id

            Dim oTxb As TextBox = e.Item.FindControl("TXBCompletion")

            oLb = e.Item.FindControl("LBcompletion")

            If Not IsNothing(dtoItem) Then
                oTxb.Text = dtoItem.Completion
            Else
                oTxb.Text = 0
            End If

            If Not isAutoEp Then
                oTxb = e.Item.FindControl("TXBmark")
                oLb = e.Item.FindControl("LBmark")
                oLb.Visible = True

                oTxb.Text = dtoItem.Mark
                oTxb.Visible = True
                oTxb.Attributes("Min") = dtoItem.MinMark

            End If

            oLb = e.Item.FindControl("LBdate")
            oLb.Text = SetDateLabel(dtoItem.StartDate, dtoItem.EndDate)

            Dim Rules As List(Of dtoRule) = ServiceEP.GetActivityDtoRuleByActivityId(dtoItem.Id)
            If Rules.Count > 0 Then
                RpRoles = e.Item.FindControl("RPrule")
                RpRoles.DataSource = Rules
                AddHandler RpRoles.ItemDataBound, AddressOf RPrule_ItemDataBound
                RpRoles.DataBind()

            Else
                Dim ul As HtmlControl = e.Item.FindControl("ULactRules")
                ul.Attributes("class") = "empty"
                hideControl(ul)
                ul.Visible = False

            End If

        End If
    End Sub

    Private Sub SetLbActName(ByRef oLB As Label, ByRef name As String)
        oLB.Visible = True
        oLB.Text = name
    End Sub

    Public Sub RPrule_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbl As Label
            Dim dtoItem As dtoRule = e.Item.DataItem
            lbl = e.Item.FindControl("LBrule")
            lbl.Text = dtoItem.Name
        End If
    End Sub

#End Region

    Private Sub ClearSession()

        Me.Session("EvaluatedItems" & SessionUniqueKey.ToString) = Nothing

    End Sub

    Private Sub LKBsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBsave.Click
        GetEvaluatedItems()
        initPageValidate()

    End Sub

    Private Sub LKBeduPathView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBeduPathView.Click

        ClearSession()

        PageUtility.RedirectToUrl(RootObject.PathView(CurrentPathId, Me.CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(CurrentPathId)))

    End Sub

    Private Sub LKBreset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBreset.Click
        BindDati()
    End Sub

    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
        CTRLmoduleStatusMessage.Visible = True
        CTRLmoduleStatusMessage.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
         EvaluatedItems = ServiceEP.GetEpStructure_toValidate(CurrentPathId, CurrentUserId, CurrentCommRoleID)
        Master.ServiceTitle = EvaluatedItems.Name

        MLVeduPathView.SetActiveView(VIWmessages)
        CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property
End Class






