Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq
'Imports lm.Comol.Core.Dss.Domain.Templates
Imports lm.Comol.Modules.CallForPapers.Advanced
Imports lm.Comol.Modules.CallForPapers.Advanced.dto
Imports lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain

Public Class cpAdvCommissionEdit
    Inherits FRcallPageBase
    Implements lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView.iViewAdvCommissionEdit
    'IViewEvaluationCommitteesEditor

#Region "Context"
    Private _Presenter As Presentation.AdvComEditPresenter 'AdvComEditPresenter
    Private ReadOnly Property CurrentPresenter() As Presentation.AdvComEditPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New Presentation.AdvComEditPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

    ''ToDo: togliere
    Private Property AllowSave As Boolean Implements Presentation.iView.iViewAdvCommissionEdit.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
            'Me.BTNaddCommitteeTop.Visible = value
            Me.BTNsaveCommitteeBottom.Visible = value
            Me.BTNsaveCommitteeTop.Visible = value
        End Set
    End Property

    Private Property IdCall As Long Implements Presentation.iView.iViewAdvCommissionEdit.IdCall
        Get
            Dim IdC As Int64 = 0

            If IsNumeric(ViewStateOrDefault("IdCall", 0)) Then
                IdC = ViewStateOrDefault("IdCall", 0)
            End If

            If (IdC = 0 AndAlso IsNumeric(Me.Request.QueryString("cId"))) Then
                IdC = CLng(Me.Request.QueryString("cId"))
            End If

            ViewState("IdCall") = IdC

            Return IdC
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property


    Public Property IdComm As Long Implements iViewAdvCommissionEdit.IdComm
        Get
            Dim IdC As Int64 = 0

            If IsNumeric(ViewStateOrDefault("commissionId", 0)) Then
                IdC = ViewStateOrDefault("commissionId", 0)
            End If

            If (IdC = 0 AndAlso IsNumeric(Me.Request.QueryString("cnId"))) Then
                IdC = CLng(Me.Request.QueryString("cnId"))
            End If

            ViewState("commissionId") = IdC

            Return IdC
        End Get
        Set(value As Long)
            Me.ViewState("commissionId") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property


#End Region

    Private Sub EditEvaluationCommittees_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        If Not Page.IsPostBack() Then

            CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionView,
                Me.IdComm,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                "")

            CurrentPresenter.InitView()
        End If

    End Sub

    Public Overrides Sub BindNoPermessi()

        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionView,
                Me.IdComm,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                "NoPermission")

        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCommittees", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls"), Helpers.MessageType.error)

            '.setHyperLink(HYPbackTop, True, True)

            HYPbackTop.NavigateUrl = BaseUrl & lm.Comol.Modules.CallForPapers.Domain.RootObject.AdvStepsEdit(Me.IdCall)
            '.setHyperLink(HYPbackBandiTop, True, True)
            'HYPbackBandiTop.NavigateUrl = HYPbackTop.NavigateUrl


            .setButton(BTNsaveCommitteeBottom, True, , , True)
            .setButton(BTNsaveCommitteeTop, True, , , True)
            '.setButton(BTNaddCommitteeTop, True, , , True)
            '.setLabel(LBcollapseAllTop)
            '.setLabel(LBexpandAllTop)
            .setLabel(LBcriteriaHideTop)
            .setLabel(LBcriteriaShowTop)

            .setButton(BTNcreateCriterion, True, , , True)
            .setButton(BTNcloseCreateCriterionWindow, True, , , True)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub



    Public Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        'Throw New NotImplementedException()
        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionView,
                Me.IdComm,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                "SessionTimeOut")
    End Sub

    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        'Throw New NotImplementedException()
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()


    End Sub
#End Region

#Region "Implements"


    Private _Permission As CommissionPermission
    Private _IsManager As Boolean
    Private Property _CanEdit As Boolean
        Get
            Return ViewStateOrDefault("CanEditCommission", False)
        End Get
        Set(value As Boolean)
            ViewState("CanEditCommission") = value
        End Set
    End Property
    Private _CanUpdateMember As Boolean

    Private Sub Init(CommissionData As dtoCommissionEdit, IsManager As Boolean) Implements iViewAdvCommissionEdit.Init

        MemberToChangeId = 0

        If IsNothing(CommissionData) Then
            Return
        End If

        PresId = CommissionData.President.Id
        SecrId = CommissionData.Secretary.Id

        _Permission = CommissionData.Permission
        _IsManager = IsManager
        _CanEdit = CommissionData.canEdit
        _CanUpdateMember = CommissionData.Status = CommissionStatus.Locked ' AndAlso (_IsManager OrElse 

        If Not CommissionData.IsMaster Then
            DVevaluationStep.Attributes.Item("css") = "stepeval hide"
        Else
            DVevaluationStep.Attributes.Item("css") = "stepeval"
        End If


        HDNidCommittee.Value = CommissionData.Id

        InitMenu(CommissionData)

        Me.TXBcommName.Text = CommissionData.Name
        Me.TXBcommDescription.Text = CommissionData.Description

        Me.TXBtags.Text = CommissionData.CommissionTags
        tagsinputSuggest.Text = CommissionData.CallTags


        CBXisMaster.Checked = CommissionData.IsMaster
        CBXisMaster.Enabled = CommissionData.EnableMaster

        Me.TXBevalMinValue.Text = CommissionData.EvalMinValue

        Me.TXBmaxTotal.Text = CommissionData.MaxValue

        Select Case CommissionData.EvalType
            Case EvalType.Average
                Me.DDLevalType.SelectedValue = "1"
            Case EvalType.Sum
                Me.DDLevalType.SelectedValue = "2"
            Case Else
                Me.DDLevalType.SelectedValue = "1"
        End Select

        Select Case CommissionData.StepEvalType
            Case EvalType.Average
                Me.DDLevalStep.SelectedValue = "1"
            Case EvalType.Sum
                Me.DDLevalStep.SelectedValue = "2"
            Case Else
                Me.DDLevalStep.SelectedValue = "1"
        End Select

        CBXevalBoolType.Checked = CommissionData.EvalBoolBlock





        If Not String.IsNullOrEmpty(CommissionData.President.SurnameAndName) Then
            LBLpresident.Text = CommissionData.President.SurnameAndName
        Else
            LBLpresident.Text = "--"
        End If

        If Not String.IsNullOrEmpty(CommissionData.Secretary.SurnameAndName) Then
            LBLsecretary.Text = CommissionData.Secretary.SurnameAndName
        Else
            LBLsecretary.Text = "--"
        End If


        If Not IsNothing(CommissionData.Members) AndAlso CommissionData.Members.Any Then
            Me.RPTmembers.Visible = True
            AllUsers = (From P As litePerson In CommissionData.Members Select P.Id).Distinct().ToList()

            Me.RPTmembers.DataSource = CommissionData.Members
            Me.RPTmembers.DataBind()
        Else
            Me.RPTmembers.Visible = False
        End If







        'ByPermission
        LKBStatCheckEvaluation.ToolTip = ""
        LKBStatStart.ToolTip = ""

        If Not _CanEdit Then
            Me.TXBcommName.Enabled = False

            Me.TXBcommDescription.Enabled = False

            Me.CBXisMaster.Enabled = False

            Me.LKBeditPresident.Visible = False
            Me.LKBeditPresident.Enabled = False

            Me.LKBeditSecretary.Visible = False
            Me.LKBeditSecretary.Enabled = False

            Me.LKBaddMember.Visible = False
            Me.LKBaddMember.Enabled = False

            Me.BTNsaveCommitteeTop.Enabled = False
            Me.BTNsaveCommitteeTop.Visible = False

            Me.BTNsaveCommitteeBottom.Enabled = False
            Me.BTNsaveCommitteeBottom.Visible = False

            Me.TXBevalMinValue.Enabled = False
            Me.CBXevalBoolType.Enabled = False
            Me.DDLevalType.Enabled = False
            DDLevalStep.Enabled = False
            TXBmaxTotal.Enabled = False

            BTNaddCriteria.Visible = False
        Else
            If Not ((CommissionData.Permission And CommissionPermission.OpenEvaluation) = CommissionPermission.OpenEvaluation) Then
                'LKBStatStart.Visible = False
                LKBStatStart.Enabled = False
                LKBStatStart.ToolTip = "Per attivare la commissione è necessario che lo step precedente sia chiuso."
                'Else
                '    LKBStatStart.Visible = True
                '    LKBStatStart.Enabled = True
            End If

            If Not ((CommissionData.Permission And CommissionPermission.CloseEvaluation) = CommissionPermission.CloseEvaluation) Then
                LKBStatCheckEvaluation.Enabled = False
                LKBStatCheckEvaluation.ToolTip = "Per chiudere la commissione è necessario che tutte le valutazioni siano state confermate."
                'LKBStatCheckEvaluation.Visible = False
                'Else
                '    LKBStatCheckEvaluation.Enabled = True
                '    LKBStatCheckEvaluation.Visible = True
            End If
        End If


        Me.RPTcriteria.DataSource = CommissionData.Criterion
        Me.RPTcriteria.DataBind()

        BindPresident(CommissionData.Permission, CommissionData.Status, CommissionData.IsPresident)

        Dim upLoaded = Not IsNothing(CommissionData.VerbaleLink) _
            AndAlso CommissionData.VerbaleLink.Id > 0 _
            AndAlso CommissionData.VerbaleLink.Deleted = BaseStatusDeleted.None

        Dim toUpload As Boolean = (CommissionData.Status = CommissionStatus.Closed) _
            AndAlso CommissionData.HasCommissionPermission(CommissionPermission.UploadVerbale) _
            AndAlso Not upLoaded

        If toUpload Then
            Me.CTRLfileUploader.InitializeControl(
                MyBase.ComunitaCorrenteID, False)

            LKBupload.Visible = True
            CTRLfileUploader.Visible = True
        Else
            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer
            ' DIMENSIONI IMMAGINI
            initializer.IconSize = Helpers.IconSize.Small
            CTRLdisplayFile.EnableAnchor = True

            initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction _
                Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions

            initializer.Link = CommissionData.VerbaleLink
            CTRLdisplayFile.InsideOtherModule = True
            Dim actions As List(Of dtoModuleActionControl)
            actions = CTRLdisplayFile.InitializeRemoteControl(initializer, initializer.Display)

            CTRLfileUploader.Visible = False
            LKBupload.Visible = False
        End If



        If (CommissionData.isEconomic) Then
            HYPevalSummary.NavigateUrl = BaseUrl & lm.Comol.Modules.CallForPapers.Domain.RootObject.EcoSummaries(Me.IdComm)
        Else
            HYPevalSummary.NavigateUrl = BaseUrl & lm.Comol.Modules.CallForPapers.Domain.RootObject.AdvEvalSummary(Me.IdComm, Me.IdCall, Me.IdComm, lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.None)
        End If

        HYPsubmissionSummary.NavigateUrl = BaseUrl &
            lm.Comol.Modules.CallForPapers.Domain.RootObject.ViewSubmissions(
                lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids,
                Me.IdCall,
                lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.None) &
                "&cmmId=" & Me.IdComm

        PHevalutation.Visible = Not CommissionData.isEconomic
        PHeconomic.Visible = CommissionData.isEconomic
    End Sub


    Private Sub RPTmembers_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTmembers.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim member As litePerson = e.Item.DataItem

            Dim isPresident As Boolean = (member.Id = Me.PresId)



            Dim LBmemName As Label = e.Item.FindControl("LBmemName")
            If Not IsNothing(LBmemName) Then
                If (isPresident) Then
                    LBmemName.Text = "[P] " & member.SurnameAndName
                Else
                    LBmemName.Text = member.SurnameAndName
                End If

            End If

            Dim uId As Int32 = _Presenter.CurrentUserId()

            Dim IsCurrentPresident As Boolean = (uId = Me.PresId)

            '(isPresident OrElse _IsManager)

            _CanEdit = _CanEdit AndAlso (_IsManager OrElse IsCurrentPresident)
            _CanUpdateMember = _CanUpdateMember AndAlso (_IsManager OrElse IsCurrentPresident)

            Dim Lkb As LinkButton = e.Item.FindControl("LKBdelMember")
            If Not IsNothing(Lkb) Then
                Lkb.CommandName = "Delete"
                Lkb.CommandArgument = member.Id
                Lkb.Enabled = _CanEdit AndAlso Not isPresident
                Lkb.Visible = _CanEdit AndAlso Not isPresident


            End If

            Lkb = e.Item.FindControl("LKBupdateMember")
            If Not IsNothing(Lkb) Then
                Lkb.CommandName = "Update"
                Lkb.CommandArgument = member.Id
                Lkb.Enabled = _CanUpdateMember AndAlso Not isPresident
                Lkb.Visible = _CanUpdateMember AndAlso Not isPresident
            End If



        End If

    End Sub

    Private Sub RPTmembers_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTmembers.ItemCommand

        Dim IdPerson As Integer = System.Convert.ToInt32(e.CommandArgument)

        If (e.CommandName = "Delete") Then
            Me.CurrentPresenter.MemberDel(IdPerson)
        ElseIf (e.CommandName = "Update") Then
            Me.MemberToChangeId = IdPerson

            updateCommitteeMember()
        End If


    End Sub

    Private Property MemberToChangeId As Int32
        Get
            Dim Id As Int32 = 0

            Try
                Id = System.Convert.ToInt64(ViewStateOrDefault("MemberToChangeId", 0))
            Catch ex As Exception

            End Try

            Return Id
        End Get
        Set(value As Int32)
            ViewState("MemberToChangeId") = value
        End Set
    End Property

#End Region

#Region "From View"
    'Protected Sub RPTcriteria_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)

    '    Dim AllowSaveBaseInfo As Boolean = True

    '    If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim criterion As dtoCriterion = DirectCast(e.Item.DataItem, dtoCriterion)

    '        Dim oLiteral As Literal = e.Item.FindControl("LTidCriterion")
    '        oLiteral.Text = criterion.Id
    '        Dim oLabel As Label = e.Item.FindControl("LBcriterionName_t")
    '        Me.Resource.setLabel(oLabel)
    '        Dim oTextBox As TextBox = e.Item.FindControl("TXBcriterionName")
    '        oTextBox.Text = criterion.Name
    '        oTextBox.Enabled = AllowSave OrElse AllowSaveBaseInfo

    '        oLabel = e.Item.FindControl("LBcriterionType")
    '        oLabel.Text = Resource.getValue("Criterion.CriterionType." & criterion.Type.ToString)


    '        oLabel = e.Item.FindControl("LBmoveCriterion")
    '        oLabel.ToolTip = Resource.getValue("LBmoveCriterion.Text")
    '        oLabel.Visible = AllowSave OrElse AllowSaveBaseInfo

    '        Dim oButton As Button = e.Item.FindControl("BTNdeleteCriterion")
    '        oButton.CommandArgument = criterion.Id
    '        oButton.Visible = AllowSave
    '        Resource.setButton(oButton, True)

    '        Dim oControl As UC_EditCriterion = e.Item.FindControl("CTRLeditCriterion")
    '        oControl.InitializeControl(criterion, AllowSave, AllowSaveBaseInfo)

    '        Dim hidden As HtmlInputHidden = e.Item.FindControl("HDNcommitteeOwner")
    '        hidden.Value = "committee_" & criterion.IdCommittee
    '        hidden = e.Item.FindControl("HDNdisplayOrder")
    '        hidden.Value = criterion.DisplayOrder

    '    End If
    'End Sub
    Private Sub RPTcriteria_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTcriteria.ItemCommand
        Dim idCriterion As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idCriterion = CLng(e.CommandArgument)
        End If
        If e.CommandName = "virtualDelete" Then
            If isValidOperation() Then
                CurrentPresenter.RemoveCriteria(Me.IdComm, idCriterion)
            End If
        End If
    End Sub
#End Region



#Region "Criteria"
    'Protected Sub AddOption(idCriterion As Long, name As String, value As Double)
    '    If isValidOperation() Then
    '        'CurrentPresenter.AddOption(GetCommittees(), UseDssMethods, idCriterion, name, value)
    '    End If
    'End Sub
    'Protected Sub RemoveOption(idOption As Long)
    '    If isValidOperation() Then
    '        CurrentPresenter.RemoveCriteria(Me.IdComm, idOption)
    '    End If
    'End Sub
    Protected Sub ChangeToIntegerType(idCriterion As Long, minValue As Integer, maxValue As Integer)
        If isValidOperation() Then
            ChangeCriterionType(idCriterion, CriterionType.IntegerRange)
        End If
    End Sub
    Protected Sub ChangeToDecimalType(idCriterion As Long, minValue As Decimal, maxValue As Decimal)
        If isValidOperation() Then
            ChangeCriterionType(idCriterion, CriterionType.DecimalRange)
        End If
    End Sub
    Private Sub ChangeCriterionType(idCriterion As Long, criterionType As lm.Comol.Modules.CallForPapers.Domain.Evaluation.CriterionType)

        'For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTcommittees.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
        '    Dim oRepeater As Repeater = row.FindControl("RPTcriteria")
        '    For Each cRow As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
        '        Dim oLiteral As Literal = cRow.FindControl("LTidCriterion")
        '        If oLiteral.Text = idCriterion.ToString() Then
        '            Dim oLabel As Label = cRow.FindControl("LBcriterionType")
        '            oLabel.Text = Resource.getValue("Criterion.CriterionType." & criterionType.ToString)
        '            Exit Sub
        '        End If
        '    Next
        'Next


        'Dim oRepeater As Repeater = row.FindControl("RPTcriteria")
        For Each cRow As RepeaterItem In (From r As RepeaterItem In Me.RPTcriteria.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oLiteral As Literal = cRow.FindControl("LTidCriterion")
            If oLiteral.Text = idCriterion.ToString() Then
                Dim oLabel As Label = cRow.FindControl("LBcriterionType")
                oLabel.Text = Resource.getValue("Criterion.CriterionType." & criterionType.ToString)
                Exit Sub
            End If
        Next

    End Sub
#End Region

    Private Property ViewPage As CommiteeEditPage
        Get

            Dim cp As CommiteeEditPage = CommiteeEditPage.Members

            Dim val As String = ViewState("CurrentViewPage")

            If String.IsNullOrEmpty(val) Then
                val = Me.Request.QueryString("pg")
            End If


            If Not String.IsNullOrEmpty(val) Then
                Try
                    cp = DirectCast([Enum].Parse(GetType(CommiteeEditPage), val), CommiteeEditPage)
                Catch ex As Exception

                End Try
            End If

            ViewState("CurrentViewPage") = cp
            Return cp

        End Get
        Set(value As CommiteeEditPage)
            ViewState("CurrentViewPage") = value
        End Set
    End Property

    Private Sub InitMenu(ByVal commData As dtoCommissionEdit)

        '.Members.Count(), CommissionData.Criterion.Count(), 0

        Dim criterionCount As Integer = 0
        If Not IsNothing(commData.Criterion) Then
            criterionCount = commData.Criterion.Count()
        End If

        Dim membersCount As Integer = 0
        If Not IsNothing(commData.Members) Then
            membersCount = commData.Members.Count()
        End If

        Dim submissionCount As Integer = 0
        'If Not IsNothing(commData.Members) Then
        '    membersCount = commData.Members.Count()
        'End If


        Dim Criterion As New dtoWizStep
        Dim Members As New dtoWizStep
        'Dim Submission As New dtoWizStep

        With Members
            .IsCurrent = False
            .IsDisabled = False
            .IsSeparator = False

            .Name = "Membri"
            .Status = dtoWizStep.StatusColor.Valid
            .Description = String.Format("{0} membro/i", membersCount)
            .Link = BaseUrl & lm.Comol.Modules.CallForPapers.Domain.RootObject.AdvCommissionEdit(Me.IdCall, Me.IdComm, CommiteeEditPage.Members)
            .CommandArgument = CommiteeEditPage.Members
            .CommandName = "Members"
        End With


        With Criterion
            .IsCurrent = False
            .IsDisabled = False
            .IsSeparator = False

            .Name = "Criteri"
            If (criterionCount > 0) Then
                .Status = dtoWizStep.StatusColor.Valid
            Else
                .Status = dtoWizStep.StatusColor.Warning
            End If

            .Description = String.Format("{0} criterio/i", criterionCount)
            .Link = BaseUrl & lm.Comol.Modules.CallForPapers.Domain.RootObject.AdvCommissionEdit(Me.IdCall, Me.IdComm, CommiteeEditPage.Criterion)
            .CommandArgument = CommiteeEditPage.Criterion
            .CommandName = "Criterion"
        End With


        'Dim isSubmissionEnabled As Boolean = False

        'With Submission
        '    .IsCurrent = False
        '    .IsDisabled = Not isSubmissionEnabled
        '    .IsSeparator = False

        '    .Name = "Sottomissioni"
        '    .Status = dtoWizStep.StatusColor.Error
        '    .Description = String.Format("{0} sottomissione/i", submissionCount)
        '    .Link = BaseUrl & lm.Comol.Modules.CallForPapers.Domain.RootObject.AdvCommissionEdit(Me.IdCall, Me.IdComm, CommiteeEditPage.Submission)
        '    .CommandArgument = CommiteeEditPage.Submission
        '    .CommandName = "Submission"
        'End With

        Select Case ViewPage
            Case CommiteeEditPage.Members
                Members.IsCurrent = True
                Me.MLVmain.SetActiveView(VMembres)

            Case CommiteeEditPage.Criterion
                Criterion.IsCurrent = True
                Me.MLVmain.SetActiveView(VCriterion)
                'Case CommiteeEditPage.Submission
                '    Submission.IsCurrent = True
                '    MLVmain.SetActiveView(VSubmission)

            Case Else   'Members
                Members.IsCurrent = True
                Me.MLVmain.SetActiveView(VMembres)
        End Select

        Dim items As New List(Of dtoWizStep)


        If Not commData.isEconomic Then
            items.Add(Criterion)
        Else
            Members.IsCurrent = True
            Me.MLVmain.SetActiveView(VMembres)
        End If

        items.Add(Members)

        'items.Add(Submission)

        CTRLmenu.BindSteps(items)

    End Sub

    Private Sub BTNsaveCommitteeBottom_Click(sender As Object, e As EventArgs) Handles BTNsaveCommitteeBottom.Click
        SaveCommitee(True)
    End Sub

    Private Sub BTNsaveCommitteeTop_Click(sender As Object, e As EventArgs) Handles BTNsaveCommitteeTop.Click
        SaveCommitee(True)
    End Sub

    Private Sub SaveCommitee(UpdateView As Boolean)


        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionSave,
                Me.IdComm,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                "")

        Dim criterions As New List(Of dtoCriterion)

        For Each item As RepeaterItem In Me.RPTcriteria.Items
            Dim uc As UC_EditCriterion = item.FindControl("CTRLeditCriterion")
            If Not IsNothing(uc) Then

                Dim crit As dtoCriterion = uc.GetCriterion()
                crit.Id = uc.IdCriterion


                Dim txb As TextBox = item.FindControl("TXBcriterionName")
                If Not IsNothing(txb) Then
                    crit.Name = txb.Text
                Else
                    crit.Name = ""
                End If

                criterions.Add(crit)
            End If
        Next

        SaveCommitee(UpdateView, criterions)

    End Sub

    Private Sub SaveCommitee(UpdateView As Boolean, criterions As List(Of dtoCriterion))

        Dim Type As EvalType = EvalType.Average

        Select Case Me.DDLevalType.SelectedValue
            Case "1"
                Type = EvalType.Average
            Case "2"
                Type = EvalType.Sum
        End Select

        Dim StepType As EvalType = EvalType.Average

        Select Case Me.DDLevalStep.SelectedValue
            Case "1"
                StepType = EvalType.Average
            Case "2"
                StepType = EvalType.Sum
        End Select

        Dim MinVal As Integer = 0

        Try
            MinVal = System.Convert.ToInt32(Me.TXBevalMinValue.Text)
        Catch ex As Exception

        End Try

        Dim MaxValue As Double = 0
        Try
            MaxValue = System.Convert.ToInt32(Me.TXBmaxTotal.Text)
        Catch ex As Exception

        End Try

        Me.CurrentPresenter.SaveCommitee(
            TXBcommName.Text,
            TXBcommDescription.Text,
            TXBtags.Text,
            CBXisMaster.Checked,
            Type,
            MinVal,
            Me.CBXevalBoolType.Checked,
            UpdateView,
            StepType,
            MaxValue,
            criterions)

    End Sub

#Region "Commitee Member"

    Protected ReadOnly Property DialogTitleTranslation() As String
        Get
            Return Resource.getValue("DialogTitleTranslation.SelectEvaluators")
        End Get
    End Property


    Private Sub addCommitteeMember() 'Top_Click(sender As Object, e As System.EventArgs) Handles BTNaddCommitteeMemberTop.Click

        CurrentUserSelection = "Member"

        Dim IdCommunity As Integer = MyBase.ComunitaCorrenteID

        If IdCommunity > 0 Then
            Me.CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, True, IdCommunity, AllUsers, Nothing, Resource.getValue("addMemberDescription"))
        Else
            Me.CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.SystemUsers, True, False, AllUsers, Nothing, Resource.getValue("addMemberDescription"))
        End If
        '  Me.SetFocus(Me.CTRLselectUsers.GetDefaultTextField)
        Me.DVselectUsers.Visible = True
    End Sub

    Private Sub updateCommitteeMember() 'Top_Click(sender As Object, e As System.EventArgs) Handles BTNaddCommitteeMemberTop.Click

        CurrentUserSelection = "UpdateMember"
        Dim UpdateTitle As String = "Aggiorna membro"
        Dim IdCommunity As Integer = MyBase.ComunitaCorrenteID

        Dim currents As New List(Of Integer)()
        currents.Add(Me.MemberToChangeId)

        If IdCommunity > 0 Then
            Me.CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, False, IdCommunity, currents, Nothing, Title)
        Else

        End If
        '  Me.SetFocus(Me.CTRLselectUsers.GetDefaultTextField)
        Me.DVselectUsers.Visible = True
    End Sub

    Private Sub LKBaddMember_Click(sender As Object, e As EventArgs) Handles LKBaddMember.Click
        addCommitteeMember()
    End Sub

    Private Property AllUsers As List(Of Integer)
        Get

            If Me.ViewState("MemberUsersIds") Is Nothing Then
                Me.ViewState("MemberUsersIds") = New List(Of Integer)()
            End If
            Return DirectCast(Me.ViewState("MemberUsersIds"), List(Of Integer))


        End Get
        Set(value As List(Of Integer))
            Me.ViewState("MemberUsersIds") = value
        End Set
    End Property

    Protected Sub CloseWindow() Handles CTRLselectUsers.CloseWindow
        Me.DVselectUsers.Visible = False
        '  Me.SetFocus(Me.btn)
    End Sub

    Private Sub CTRLselectUsers_UsersSelected(idUsers As System.Collections.Generic.List(Of Integer)) Handles CTRLselectUsers.UsersSelected
        Me.DVselectUsers.Visible = False

        If IdCall > 0 Then
            'Me.DVselectUsers.Visible = False


            Select Case Me.CurrentUserSelection
                Case "UpdateMember"
                    Me.CurrentPresenter.ChangeMember(Me.MemberToChangeId, idUsers.FirstOrDefault(), Me.IdComm)
                Case "Member"
                    Me.CurrentPresenter.MembersAdd(idUsers)
                Case "President"
                    Me.CurrentPresenter.PresidentUpdate(idUsers.FirstOrDefault())
                Case "Secretary"
                    Me.CurrentPresenter.SecretaryUpdate(idUsers.FirstOrDefault())
            End Select
        End If
    End Sub






    'Private Function GetMembers() As List(Of dtoCommitteeMember) Implements IViewManageEvaluators.GetMembers
    '    Dim items As New List(Of dtoCommitteeMember)
    '    For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTassignments.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
    '        Dim member As New dtoCommitteeMember
    '        Dim oLiteral As Literal = row.FindControl("LTidCallEvaluator")
    '        member.IdCallEvaluator = CLng(oLiteral.Text)
    '        oLiteral = row.FindControl("LTidPerson")
    '        member.IdPerson = CInt(oLiteral.Text)

    '        If IdOnlyOneCommittee > 0 Then
    '            member.Committees.Add(IdOnlyOneCommittee)
    '        Else
    '            Dim mView As MultiView = row.FindControl("MLVselectCommittee")
    '            Dim v As View = row.FindControl("VIWsingle")
    '            If mView.GetActiveView Is v Then
    '                Dim oDropdownlist As DropDownList = row.FindControl("DDLcommittees")
    '                If oDropdownlist.SelectedIndex > -1 AndAlso oDropdownlist.SelectedValue > 0 Then
    '                    member.Committees.Add(CLng(oDropdownlist.SelectedValue))
    '                End If
    '            ElseIf mView.ActiveViewIndex > 0 Then
    '                Dim oSelect As HtmlSelect = row.FindControl("SLBcommittees")
    '                member.Committees = (From i As ListItem In oSelect.Items Where i.Selected Select CLng(i.Value)).ToList()
    '            End If
    '        End If
    '        items.Add(member)
    '    Next
    '    Return items
    'End Function
#End Region


#Region "Presidente-segretario"
    Private Property PresId As Int32
        Get
            Return ViewStateOrDefault(Of Int32)("PresId", 0)
        End Get
        Set(value As Int32)
            ViewState("PresId") = value
        End Set
    End Property

    Private Property SecrId As Int32
        Get
            Return ViewStateOrDefault(Of Int32)("SecrId", 0)
        End Get
        Set(value As Int32)
            ViewState("SecrId") = value
        End Set
    End Property


    Private Sub LKBeditPresident_Click(sender As Object, e As EventArgs) Handles LKBeditPresident.Click

        CurrentUserSelection = "President"


        Dim IdCommunity As Integer = MyBase.ComunitaCorrenteID
        Dim Pres As List(Of Int32) = New List(Of Int32)()
        Pres.Add(PresId)

        Dim title As String = Resource.getValue("PresidentAddDescription")

        If (String.IsNullOrEmpty(title)) Then
            title = "Seleziona Presidente commissione"
        End If

        If IdCommunity > 0 Then
            Me.CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, False, IdCommunity, Pres, Nothing, title)

        Else

        End If
        '  Me.SetFocus(Me.CTRLselectUsers.GetDefaultTextField)
        Me.DVselectUsers.Visible = True
    End Sub

    Private Sub LKBeditSecretary_Click(sender As Object, e As EventArgs) Handles LKBeditSecretary.Click

        CurrentUserSelection = "Secretary"

        Dim IdCommunity As Integer = MyBase.ComunitaCorrenteID
        Dim Secr As List(Of Int32) = New List(Of Int32)()
        Secr.Add(SecrId)

        Dim title As String = Resource.getValue("PresidentAddDescription")

        If (String.IsNullOrEmpty(title)) Then
            title = "Seleziona Presidente commissione"
        End If

        If IdCommunity > 0 Then
            Me.CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, False, IdCommunity, Secr, Nothing, title)
        Else

        End If

        Me.DVselectUsers.Visible = True
    End Sub

    Private Sub CTRLselectUsers_UserSelected(idUser As Integer) Handles CTRLselectUsers.UserSelected
        Me.DVselectUsers.Visible = False

        If IdCall > 0 Then
            'Me.DVselectUsers.Visible = False


            Select Case Me.CurrentUserSelection
                Case "UpdateMember"
                    Me.CurrentPresenter.ChangeMember(Me.MemberToChangeId, idUser, Me.IdComm)
                'Case "Member"
                '    Me.CurrentPresenter.MembersAdd(idUsers)
                Case "President"
                    Me.CurrentPresenter.PresidentUpdate(idUser)
                Case "Secretary"
                    Me.CurrentPresenter.SecretaryUpdate(idUser)
            End Select
        End If
    End Sub

    Private Sub RPTcriteria_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTcriteria.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim criterion As dtoCriterion = DirectCast(e.Item.DataItem, dtoCriterion)

            Dim oLiteral As Literal = e.Item.FindControl("LTidCriterion")
            oLiteral.Text = criterion.Id
            Dim oLabel As Label = e.Item.FindControl("LBcriterionName_t")
            Me.Resource.setLabel(oLabel)
            Dim oTextBox As TextBox = e.Item.FindControl("TXBcriterionName")
            oTextBox.Text = criterion.Name
            oTextBox.Enabled = _CanEdit 'OrElse AllowSaveBaseInfo

            oLabel = e.Item.FindControl("LBcriterionType")
            oLabel.Text = Resource.getValue("Criterion.CriterionType." & criterion.Type.ToString)


            oLabel = e.Item.FindControl("LBmoveCriterion")
            oLabel.ToolTip = Resource.getValue("LBmoveCriterion.Text")
            oLabel.Visible = _CanEdit 'OrElse AllowSaveBaseInfo

            Dim oButton As Button = e.Item.FindControl("BTNdeleteCriterion")
            oButton.CommandArgument = criterion.Id
            oButton.Visible = _CanEdit
            Resource.setButton(oButton, True)

            Dim oControl As UC_EditCriterion = e.Item.FindControl("CTRLeditCriterion")
            oControl.InitializeControl(criterion, _CanEdit, _CanEdit) 'AllowSave, AllowSaveBaseInfo)

            Dim hidden As HtmlInputHidden = e.Item.FindControl("HDNcommitteeOwner")
            hidden.Value = "committee_" & criterion.IdCommittee
            hidden = e.Item.FindControl("HDNdisplayOrder")
            hidden.Value = criterion.DisplayOrder

        End If
    End Sub



    Private Property CurrentUserSelection As String
        Get
            Return ViewStateOrDefault("CurrentUserSelection", "None")
        End Get
        Set(value As String)
            ViewState("CurrentUserSelection") = value
        End Set
    End Property

#End Region

#Region "Criterion"
    Private Sub BTNaddCriteria_Click(sender As Object, e As EventArgs) Handles BTNaddCriteria.Click


        CTRLaddCriterion.InitializeControl(IdCall, False)
        LTscriptOpen.Visible = True

    End Sub

    Private Sub BTNcloseCreateCriterionWindow_Click(sender As Object, e As EventArgs) Handles BTNcloseCreateCriterionWindow.Click
        Me.LTscriptOpen.Visible = False
    End Sub

    Private Sub BTNcreateCriterion_Click(sender As Object, e As EventArgs) Handles BTNcreateCriterion.Click
        'Dim idCommittee As Long = 0
        'If Not String.IsNullOrEmpty(Me.HDNidCommittee.Value) Then
        '    idCommittee = CLng(Me.HDNidCommittee.Value.Replace("section-", ""))
        '    Dim settings As dtoItemMethodSettings = Nothing
        '    If UseDssMethods Then
        '        settings = GetCallDssSettings()
        '    End If
        '    Dim criteria As List(Of BaseCriterion) = Me.CTRLaddCriterion.CreateCriteria(GetCommittees(), settings, idCommittee)
        '    If (criteria.Count > 0) Then
        '        PageUtility.RedirectToUrl(lm.Comol.Modules.CallForPapers.Domain.RootObject.CriterionAddedToCommittee(criteria(0).Id, IdCall, IdCommunity, PreloadView))
        '    Else
        '        Me.LTscriptOpen.Visible = False
        '    End If
        'End If


        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionSave,
                Me.IdComm,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                "")

        Dim dtoCriterions As List(Of dtoCriterion) = Me.CTRLaddCriterion.GetCriteriaToCreate()


        'Me.CurrentPresenter.SaveCommitee(TXBcommName.Text, TXBcommDescription.Text, CBXisMaster.Checked, False, dtoCriterions)
        SaveCommitee(False, dtoCriterions)

        'SaveCommitee(False)

        'Dim criteria As List(Of BaseCriterion) = Me.CTRLaddCriterion.CreateCriteriaAdv(Me.IdComm) ', settings, idCommittee)
        Me.LTscriptOpen.Visible = False

        Me.CurrentPresenter.InitView()

        'If (criteria.Count > 0) Then
        '    PageUtility.RedirectToUrl(lm.Comol.Modules.CallForPapers.Domain.RootObject.CriterionAddedToCommittee(criteria(0).Id, IdCall, IdCommunity, PreloadView))
        'Else
        '    Me.LTscriptOpen.Visible = False
        'End If
    End Sub


    Protected Sub AddOption(idCriterion As Long, name As String, value As Double)
        If isValidOperation() Then
            SaveCommitee(False)

            CurrentPresenter.AddOption(Me.IdComm, idCriterion, name, value)
        End If
    End Sub
    Protected Sub RemoveOption(idOption As Long)
        If isValidOperation() Then

            SaveCommitee(False)

            CurrentPresenter.RemoveOption(Me.IdComm, idOption)
        End If
    End Sub







    Private Sub BindPresident(ByVal permission As CommissionPermission,
                              ByVal comStatus As CommissionStatus,
                              ByVal IsPresident As Boolean)

        Select Case comStatus
            Case CommissionStatus.Draft
                SPNstatus.Attributes("class") = String.Format("status {0}", comStatus)
                LBLcommStatus.Text = "In bozza"

                SPNsubmissionVisibility.Attributes("class") = "status hidesubs"
                LBLsubmissionVisibility.Text = "Sottomissioni nascoste"

            Case CommissionStatus.ViewSubmission
                SPNstatus.Attributes("class") = String.Format("status {0}", comStatus)
                LBLcommStatus.Text = "In bozza"

                SPNsubmissionVisibility.Attributes("class") = "status showsubs"
                LBLsubmissionVisibility.Text = "Sottomissioni visibili"

            Case CommissionStatus.Started
                SPNstatus.Attributes("class") = String.Format("status {0}", comStatus)
                LBLcommStatus.Text = "Attiva"

                SPNsubmissionVisibility.Attributes("class") = "status showsubs"
                LBLsubmissionVisibility.Text = "Sottomissioni visibili"

            Case CommissionStatus.Locked
                SPNstatus.Attributes("class") = String.Format("status {0}", comStatus)
                LBLcommStatus.Text = "Valutazioni bloccate"

                SPNsubmissionVisibility.Attributes("class") = "status showsubs"
                LBLsubmissionVisibility.Text = "Sottomissioni visibili"

            Case CommissionStatus.ValutationEnded
                SPNstatus.Attributes("class") = String.Format("status {0}", comStatus)
                LBLcommStatus.Text = "Valutazioni concluse"

                SPNsubmissionVisibility.Visible = False
                LBLsubmissionVisibility.Visible = False
            Case CommissionStatus.ValutationConfirmed
                SPNstatus.Attributes("class") = String.Format("status {0}", comStatus)
                LBLcommStatus.Text = "Valutazioni confermate"

                SPNsubmissionVisibility.Visible = False
                LBLsubmissionVisibility.Visible = False
            Case CommissionStatus.Closed
                SPNstatus.Attributes("class") = String.Format("status {0}", comStatus)
                LBLcommStatus.Text = "Commissione chiusa"

                SPNsubmissionVisibility.Visible = False
                LBLsubmissionVisibility.Visible = False



        End Select

        Dim canChangeStatus As Boolean = False

        If (permission And CommissionPermission.OpenEvaluation = CommissionPermission.OpenEvaluation) _
            OrElse (permission And CommissionPermission.CloseEvaluation = CommissionPermission.CloseEvaluation) _
            OrElse (permission And CommissionPermission.CloseCommission = CommissionPermission.CloseCommission) Then

            canChangeStatus = True

        End If


        Dim canUploadVerbale As Boolean = (permission And CommissionPermission.UploadVerbale = CommissionPermission.UploadVerbale) AndAlso
            (comStatus = StepStatus.Locked OrElse comStatus = StepStatus.Closed)

        Me.PNLpresident.Visible = canChangeStatus OrElse canUploadVerbale

        LKBStatShowSubmission.Visible = False
        LKBStatHideSubmission.Visible = False
        LKBStatStart.Visible = False
        LKBStatLockEvaluation.Visible = False
        LKBStatUnLockEvaluation.Visible = False
        LKBStatCheckEvaluation.Visible = False


        'LKBStatDownload.Visible = False
        UC_HTMLExport.Visible = False
        'LKBStatUpload.Visible = False
        'PNLStatVerbale.Visible = False

        Select Case comStatus
            Case CommissionStatus.Draft
                LKBStatShowSubmission.Visible = True
                LKBStatStart.Visible = IsPresident

            Case CommissionStatus.ViewSubmission
                LKBStatHideSubmission.Visible = True
                LKBStatStart.Visible = IsPresident

            Case CommissionStatus.Started
                LKBStatLockEvaluation.Visible = IsPresident

            Case CommissionStatus.Locked
                LKBStatUnLockEvaluation.Visible = IsPresident

            Case CommissionStatus.ValutationEnded
                LKBStatCheckEvaluation.Visible = IsPresident

            Case CommissionStatus.Closed
                If (canUploadVerbale) Then
                    UC_HTMLExport.Visible = True
                Else
                    'PNLStatVerbale.Visible = True
                End If

        End Select


        If (UC_HTMLExport.Visible) Then
            'UC_HTMLExport.ShowHideBtn = False
            UC_HTMLExport.ShowEditor = False

            UC_HTMLExport.HTML = Me.CurrentPresenter.HTMLEsportSummary(Me.IdComm)

        End If


    End Sub


#End Region

#Region "ChangeStatus"

    Private Sub ChangeStatus(ByVal newStatus As StepStatus)

        CurrentPresenter.changeCommissionStatus(Me.IdComm, newStatus)

    End Sub

    'Private Sub LKBassignAll_Click(sender As Object, e As EventArgs) Handles LKBassignAll.Click
    '    Me.CurrentPresenter.SubmissionAssignAll(Me.IdCall, Me.IdComm)
    'End Sub



    Private Sub LKBStatHideSubmission_Click(sender As Object, e As EventArgs) Handles LKBStatHideSubmission.Click
        'Me.CurrentPresenter.changeCommissionStatus(Me.IdComm, CommissionStatus.Draft)
        ChangeCommissionStatus(CommissionStatus.Draft)
    End Sub

    Private Sub LKBStatShowSubmission_Click(sender As Object, e As EventArgs) Handles LKBStatShowSubmission.Click
        'Me.CurrentPresenter.changeCommissionStatus(Me.IdComm, CommissionStatus.ViewSubmission)
        ChangeCommissionStatus(CommissionStatus.ViewSubmission)
    End Sub

    Private Sub LKBStatStart_Click(sender As Object, e As EventArgs) Handles LKBStatStart.Click
        'Me.CurrentPresenter.changeCommissionStatus(Me.IdComm, CommissionStatus.Started)
        ChangeCommissionStatus(CommissionStatus.Started)
    End Sub

    Private Sub LKBStatUnLockEvaluation_Click(sender As Object, e As EventArgs) Handles LKBStatUnLockEvaluation.Click
        'Me.CurrentPresenter.changeCommissionStatus(Me.IdComm, CommissionStatus.Started)
        ChangeCommissionStatus(CommissionStatus.Started)
    End Sub

    Private Sub LKBStatLockEvaluation_Click(sender As Object, e As EventArgs) Handles LKBStatLockEvaluation.Click
        ChangeCommissionStatus(CommissionStatus.Locked)
    End Sub

    Private Sub ChangeCommissionStatus(ByVal Status As CommissionStatus)

        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvCommissionChangeStatus,
                Me.IdComm,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                [Enum].GetName(GetType(CommissionStatus), Status))

        Me.CurrentPresenter.changeCommissionStatus(Me.IdComm, Status)
    End Sub

    Private Sub LKBStatCheckEvaluation_Click(sender As Object, e As EventArgs) Handles LKBStatCheckEvaluation.Click
        'ToDo: GoTo check evaluation
        ''Me.CurrentPresenter.changeCommissionStatus(Me.IdComm, )
    End Sub

    'Private Sub LKBStatDownload_Click(sender As Object, e As EventArgs) Handles LKBStatDownload.Click
    '    ''ToDo: download DRAFT!
    'End Sub

    'Private Sub LKBStatUpload_Click(sender As Object, e As EventArgs) Handles LKBStatUpload.Click
    '    'Todo: carica verbale
    'End Sub

    'Private Sub LKBStatVerbaleDownload_Click(sender As Object, e As EventArgs) Handles LKBStatVerbaleDownload.Click
    '    'ToDo: scarica VERBALE
    'End Sub

    Private Sub LKBupload_Click(sender As Object, e As EventArgs) Handles LKBupload.Click

        Me.CurrentPresenter.UploadVerbale()
    End Sub

    'LKBBackDraft
    'LKBShowSubmission
    'LKBStartEvaluation
    'LKBlockEvaluation
    'LKBcloseEvaluation
    'LKBshowReport
    'LKBuploadReport






#End Region


    Public Function AddInternalFile(
                                   commission As lm.Comol.Modules.CallForPapers.Advanced.Domain.AdvCommission,
                                   moduleCode As String,
                                   idModule As Integer,
                                   moduleAction As Integer,
                                   objectType As Integer) As ModuleActionLink _
                                   Implements iViewAdvCommissionEdit.AddInternalFile

        Return CTRLfileUploader.UploadAndLinkInternalFile(FileRepositoryType.InternalLong, commission, moduleCode, moduleAction, objectType)
    End Function

    Private Sub CTRLmenu_ChangePage(Command As String, Argument As String) Handles CTRLmenu.ChangePage
        SaveCommitee(False)
        Response.Redirect(Argument)
    End Sub


#Region "Action"

    Private Sub SendUserAction(
                              idCommunity As Integer,
                              idModule As Integer,
                              action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType,
                              objectType As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType,
                              IdObject As String) _
                Implements iViewAdvCommissionEdit.SendUserAction

        PageUtility.AddActionToModule(idCommunity,
                                      idModule,
                                      action,
                                      PageUtility.CreateObjectsList(
                                        idModule,
                                        objectType,
                                        IdObject),
                                        InteractionType.UserWithLearningObject)
    End Sub


#End Region

End Class