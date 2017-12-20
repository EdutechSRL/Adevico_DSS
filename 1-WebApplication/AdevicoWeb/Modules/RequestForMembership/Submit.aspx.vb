
'Imports lm.Comol.Core.DomainModel
'Imports lm.ActionDataContract
'Imports lm.Comol.Modules.CallForPapers.Domain
'Imports lm.Comol.Modules.CallForPapers.Presentation
'Imports lm.Comol.UI.Presentation

Public Class RequestForMembership_Submit
    Inherits PageBase


    '    Inherits PageBase
    '    Implements lm.Comol.Modules.CallForPapers.Presentation.IViewRFMsubmission


    '#Region "Context"
    '    Private _Presenter As RfM_submissionPresenter
    '    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    '    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
    '        Get
    '            If IsNothing(_CurrentContext) Then
    '                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
    '            End If
    '            Return _CurrentContext
    '        End Get
    '    End Property
    '    Public ReadOnly Property CurrentPresenter() As RfM_submissionPresenter
    '        Get
    '            If IsNothing(_Presenter) Then
    '                _Presenter = New RfM_submissionPresenter(Me.CurrentContext, Me)
    '            End If
    '            Return _Presenter
    '        End Get
    '    End Property
    '#End Region

    '#Region ""
    '    Public ReadOnly Property Portalname As String Implements IViewRFMsubmission.Portalname
    '        Get
    '            Return Resource.getValue("Portalname")
    '        End Get
    '    End Property
    Public ReadOnly Property PreloadRequestForMembershipID As Long
        Get
            If IsNumeric(Me.Request.QueryString("CfpId")) Then
                Return CLng(Me.Request.QueryString("CfpId"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property FromPublicList As Boolean
        Get
            If String.IsNullOrEmpty(Me.Request.QueryString("FromPublicList")) Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property
    '    Public ReadOnly Property PreloadSubmissionID As Long Implements IViewRFMsubmission.PreloadSubmissionID
    '        Get
    '            If IsNumeric(Me.Request.QueryString("SId")) Then
    '                Return CLng(Me.Request.QueryString("SId"))
    '            Else
    '                Return 0
    '            End If
    '        End Get
    '    End Property

    '    Public WriteOnly Property AllowCompleteSubmission As Boolean Implements IViewRFMsubmission.AllowCompleteSubmission
    '        Set(ByVal value As Boolean)
    '            'Me.LNBsaveAndSubmit.Visible = value
    '            'Me.LNBsaveAndSubmitBottom.Visible = value
    '        End Set
    '    End Property
    '    Public WriteOnly Property AllowSave As Boolean Implements IViewRFMsubmission.AllowSave
    '        Set(ByVal value As Boolean)
    '            'Me.LNBsave.Visible = value
    '            'Me.LNBsaveBottom.Visible = value
    '        End Set
    '    End Property
    '    Public WriteOnly Property AllowSubmitterSelection As Boolean Implements IViewRFMsubmission.AllowSubmitterSelection
    '        Set(ByVal value As Boolean)
    '            Me.DVsubmittersType.Visible = value
    '        End Set
    '    End Property
    '    Public WriteOnly Property AllowDeleteSubmission As Boolean Implements IViewRFMsubmission.AllowDeleteSubmission
    '        Set(ByVal value As Boolean)
    '            'Me.LNBdelete.Visible = value
    '        End Set
    '    End Property

    '    Public WriteOnly Property AllowStartSubmission As Boolean Implements IViewRFMsubmission.AllowStartSubmission
    '        Set(ByVal value As Boolean)
    '            ' Me.LNBdelete.Visible = value
    '        End Set
    '    End Property

    '    Public Function CommunityPermission(ByVal CommunityID As Integer) As ModuleRequestForMembership 'Implements IViewRequestForMembership.CommunityPermission

    '    End Function

    '    Public Property SubmissionUniqueId As Guid Implements IViewRFMsubmission.SubmissionUniqueID
    '        Get
    '            Try
    '                Return CType(Me.ViewState("SubmissionUniqueId"), Guid)
    '            Catch ex As Exception
    '                Return New System.Guid()
    '            End Try
    '        End Get
    '        Set(value As Guid)
    '            Me.ViewState("SubmissionUniqueId") = value
    '        End Set
    '    End Property

    '    Public Property CallCommunityID As Integer Implements IViewRFMsubmission.CallCommunityID
    '        Get
    '            If IsNumeric(Me.ViewState("CallCommunityID")) Then
    '                Return CInt(Me.ViewState("CallCommunityID"))
    '            Else
    '                Return 0
    '            End If
    '        End Get
    '        Set(ByVal value As Integer)
    '            Me.ViewState("CallCommunityID") = value
    '        End Set
    '    End Property
    '    Public Property RequestForMembershipID As Long Implements IViewRFMsubmission.RequestForMembershipID
    '        Get
    '            If IsNumeric(Me.ViewState("RequestForMembershipID")) Then
    '                Return CLng(Me.ViewState("RequestForMembershipID"))
    '            Else
    '                Return 0
    '            End If
    '        End Get
    '        Set(ByVal value As Long)
    '            Me.ViewState("RequestForMembershipID") = value
    '        End Set
    '    End Property
    '    Public Property SelectedSubmitterTypeID As Long Implements IViewRFMsubmission.SelectedSubmitterTypeID
    '        Get
    '            If IsNumeric(Me.ViewState("SelectedSubmitterTypeID")) Then
    '                Return CLng(Me.ViewState("SelectedSubmitterTypeID"))
    '            End If
    '            Return 0
    '        End Get
    '        Set(ByVal value As Long)
    '            Me.ViewState("SelectedSubmitterTypeID") = value
    '        End Set
    '    End Property
    '    Public Property SubmissionID As Long Implements IViewRFMsubmission.SubmissionID
    '        Get
    '            If IsNumeric(Me.ViewState("SubmissionID")) Then
    '                Return CLng(Me.ViewState("SubmissionID"))
    '            End If
    '            Return 0
    '        End Get
    '        Set(ByVal value As Long)
    '            Me.ViewState("SubmissionID") = value
    '        End Set
    '    End Property

    '    'Public Function CommunityPermission(ByVal CommunityID As Integer) As ModuleRequestForMembership Implements IViewRequestForMembership.CommunityPermission
    '    '    Dim oModule As ModuleRequestForMembership = Nothing

    '    '    If CommunityID = 0 Then
    '    '        oModule = ModuleRequestForMembership.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)
    '    '    Else
    '    '        oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, ModuleRequestForMembership.UniqueCode) _
    '    '                      Where sb.CommunityID = CommunityID Select New ModuleRequestForMembership(New UCServices.Service_CallForPapers(sb.PermissionString).ConvertToLong)).FirstOrDefault

    '    '    End If

    '    '    If IsNothing(oModule) Then
    '    '        oModule = New ModuleRequestForMembership
    '    '    End If
    '    '    Return oModule
    '    'End Function

    '    Public Property InitSubmissionTime As DateTime Implements IViewRFMsubmission.InitSubmissionTime
    '        Get
    '            If Not TypeOf (Me.ViewState("InitSubmissionTime")) Is DateTime Then
    '                Me.ViewState("InitSubmissionTime") = Now
    '            End If
    '            Return CDate(Me.ViewState("InitSubmissionTime"))
    '        End Get
    '        Set(ByVal value As DateTime)
    '            Me.ViewState("InitSubmissionTime") = value
    '        End Set
    '    End Property
    '#End Region

    '#Region "Steps"

    '    'Public ReadOnly Property CurrentStep As SubmissionToRFMwizardStep Implements IViewRFMsubmission.CurrentStep
    '    '    Get
    '    '        Return ViewStateOrDefault("CurrentStep", SubmissionToRFMwizardStep.None)
    '    '    End Get
    '    'End Property




    '#End Region


    '#Region "Inherited"
    '    Public Overrides ReadOnly Property AlwaysBind As Boolean
    '        Get
    '            Return False
    '        End Get
    '    End Property
    '    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
    '        Get
    '            Return False
    '        End Get
    '    End Property
    '#End Region

    '    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.LNBsaveAndSubmitBottom)
    '        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.LNBsaveBottom)
    '    End Sub

    '#Region "Inherited Methods"
    '    Public Overrides Sub BindDati()
    '        Me.Master.ShowNoPermission = False
    '        If Page.IsPostBack = False Then
    '            Me.SetInternazionalizzazione()
    '            ' Me.TMsession.Interval = Me.SystemSettings.Presenter.AjaxTimer
    '            Me.CurrentPresenter.InitView()
    '        End If
    '    End Sub
    '    Public Overrides Sub BindNoPermessi()
    '        Me.Master.ShowNoPermission = True
    '    End Sub
    '    Public Overrides Function HasPermessi() As Boolean
    '        Return True
    '    End Function
    '    Public Overrides Sub RegistraAccessoPagina()

    '    End Sub
    '    Public Overrides Sub SetCultureSettings()
    '        MyBase.SetCulture("pg_Submission", "Modules", "RequestForMembership")
    '    End Sub

    '    Public Overrides Sub SetInternazionalizzazione()
    '        With MyBase.Resource
    '            Master.ServiceTitle = .getValue("ServiceTitle")
    '            Master.ServiceNopermission = .getValue("ServiceNopermission")
    '            '.setHyperLink(HYPlist, True, True)
    '            '.setLinkButton(LNBdelete, True, True)
    '            '.setLinkButton(LNBsave, True, True)
    '            '.setLinkButton(LNBsaveAndSubmit, True, True)
    '            '.setLinkButton(LNBsaveBottom, True, True)
    '            '.setLiteral(LTsaveBottomExplanation)
    '            ' .setLinkButton(LNBsaveAndSubmitBottom, True, True)
    '            '.setLiteral(LTsaveAndSubmitBottomExplanation)
    '            '.setLiteral(LTdisplayTitle)
    '            '.setLiteral(LTendDateTitle)
    '            '.setLiteral(LTstartDateTitle)
    '            '.setLiteral(LTsubmitterTypeSelection)

    '            '.setHyperLink(HYPlistFromMessage, True, True)
    '            '.setLinkButton(LNBretry, True, True)
    '            '.setLiteral(LTuserMessage)
    '            .setLiteral(LTemptyFields)
    '            .setLiteral(LTemptyFiles)
    '            '.setHyperLink(HYPlistFromMessageBottom, True, True)
    '            .setLinkButton(LNBretryBottom, True, True)
    '            .setLinkButton(Me.LNBback, True, True)
    '            .setLinkButton(Me.LNBnext, True, True)
    '            '.setButton(BTNback, True)
    '            '.setButton(BTNnext, True)
    '            .setLiteral(LTmandatoryLegend)
    '        End With
    '    End Sub
    '#End Region

    '#Region "User Action / Notification"
    '    Public Sub SendUserAction(ByVal IdCommunity As Integer, ByVal IdModule As Integer, ByVal IdSubmission As Long, ByVal action As ModuleRequestForMembership.ActionType) Implements IViewRFMsubmission.SendUserAction
    '        Me.PageUtility.AddActionToModule(IdCommunity, IdModule, action, PageUtility.CreateObjectsList(IdModule, ModuleCallForPaper.ObjectType.UserSubmission, IdSubmission.ToString), InteractionType.UserWithLearningObject)
    '    End Sub
    '    Public Sub SendStartSubmission(ByVal IdCommunity As Integer, ByVal IdModule As Integer, ByVal IdCallForPaper As Long, ByVal action As ModuleRequestForMembership.ActionType) Implements IViewRFMsubmission.SendStartSubmission
    '        Me.PageUtility.AddActionToModule(IdCommunity, IdModule, action, PageUtility.CreateObjectsList(IdModule, ModuleCallForPaper.ObjectType.CallForPaper, IdCallForPaper.ToString), InteractionType.UserWithLearningObject)
    '    End Sub

    '#End Region

    '#Region "Load Call For Paper Info"
    '    Public Sub LoadRequestForMembership(ByVal requestForMembership As dtoRequestForMembership) Implements IViewRFMsubmission.LoadRequestForMembership
    '        'Me.LTawardDate.Text = requestForMembership.AwardDate
    '        Me.LTdescription.Text = requestForMembership.StartMessage 'Description
    '        'Me.LTprojectEdition.Text = requestForMembership.Edition
    '        Me.Master.ServiceTitle = requestForMembership.Name
    '        'If Not IsNothing(requestForMembership.StartDate) Then
    '        '    Me.LTstartDate.Text = FormatDateTime(requestForMembership.StartDate.ToString(), DateFormat.ShortDate) & IIf(requestForMembership.EndDate.Value.Hour = 0, "", " - " & FormatDateTime(requestForMembership.StartDate.ToString(), DateFormat.ShortTime))
    '        'Else
    '        '    Me.LTendDate.Text = "//"
    '        'End If
    '        'If requestForMembership.EndDate.HasValue Then
    '        '    Me.LTendDate.Text = FormatDateTime(requestForMembership.EndDate.Value, DateFormat.ShortDate) & IIf(requestForMembership.EndDate.Value.Hour = 0, "", " - " & FormatDateTime(requestForMembership.EndDate.Value, DateFormat.ShortTime))
    '        'Else
    '        '    Me.LTendDate.Text = "//"
    '        'End If

    '    End Sub

    '#Region "Submitters"
    '    Public Sub LoadSubmittersType(ByVal list As IList(Of dtoSubmitterType)) Implements IViewRFMsubmission.LoadSubmittersType
    '        Dim isVisible As Boolean = Not IsNothing(list) AndAlso (list.Count > 0)
    '        Me.DVsubmittersType.Visible = isVisible
    '        If isVisible Then
    '            Me.RPTsubmittersType.DataSource = list
    '            Me.RPTsubmittersType.DataBind()
    '        End If
    '    End Sub
    '    Private Sub RPTsubmittersType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsubmittersType.ItemDataBound
    '        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '            Dim oDto As dtoSubmitterType = DirectCast(e.Item.DataItem, dtoSubmitterType)
    '            Dim oLinkButton As LinkButton = e.Item.FindControl("LNBsubmittersSelector")
    '            oLinkButton.Text = oDto.Name
    '            If String.IsNullOrEmpty(oDto.Description) Then
    '                oLinkButton.ToolTip = oDto.Name
    '            Else
    '                oLinkButton.ToolTip = oDto.Description
    '            End If
    '            oLinkButton.CommandArgument = oDto.Id
    '        ElseIf e.Item.ItemType = ListItemType.Header Then
    '            Dim oLiteral As Literal = e.Item.FindControl("LTfileAttachments")
    '            Me.Resource.setLiteral(oLiteral)
    '        End If
    '    End Sub
    '    Private Sub RPTsubmittersType_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTsubmittersType.ItemCommand
    '        If e.CommandName = "selectType" Then
    '            Me.CurrentPresenter.LoadFields(CLng(e.CommandArgument)) '(SetupSubmission(CLng(e.CommandArgument)))

    '            'Check if it's right
    '            'Me.ViewState("CurrentStep") = SubmissionToRFMwizardStep.SubmissionFields
    '        End If
    '    End Sub
    '#End Region

    '#Region "Attachments"
    '    Public Sub LoadAttachments(ByVal list As IList(Of dtoAttachmentFile)) Implements IViewRFMsubmission.LoadAttachments
    '        Me.RPTfileAttachments.Visible = (list.Count <> 0)
    '        If list.Count > 0 Then
    '            Me.RPTfileAttachments.DataSource = list
    '            Me.RPTfileAttachments.DataBind()
    '        End If
    '    End Sub
    '    Private Sub RPTfileAttachments_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfileAttachments.ItemDataBound
    '        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
    '            Dim oDto As dtoAttachmentFile = DirectCast(e.Item.DataItem, dtoAttachmentFile)
    '            Dim oFileDisplay As UC_CallForPaperFile = e.Item.FindControl("CTRLcallForPaperFile")
    '            oFileDisplay.InitControl(oDto, Me.CallCommunityID, False)
    '        End If
    '    End Sub
    '#End Region

    '#End Region

    '#Region "Manage submission"
    '#Region "Sections"
    '    Public Sub LoadSubmissionSections(ByVal list As IList(Of dtoFieldsSection(Of dtoSubmissionField)), ByVal files As IList(Of dtoSubmissionFile)) Implements IViewRFMsubmission.LoadSubmissionSections
    '        Me.MLVsubmission.SetActiveView(Me.VIWfields)
    '        Me.RPTsections.Visible = (list.Count <> 0)
    '        If list.Count > 0 Then
    '            Me.RPTsections.DataSource = list
    '            Me.RPTsections.DataBind()
    '        End If

    '        Me.RPTrequestedFiles.Visible = (files.Count <> 0)
    '        If files.Count > 0 Then
    '            Me.RPTrequestedFiles.DataSource = files
    '            Me.RPTrequestedFiles.DataBind()
    '        End If
    '    End Sub
    '    Private Sub RPTsections_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsections.ItemDataBound
    '        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '            Dim oDto As dtoFieldsSection(Of dtoSubmissionField) = DirectCast(e.Item.DataItem, dtoFieldsSection(Of dtoSubmissionField))
    '            Dim oRepeater As Repeater = e.Item.FindControl("RPTfields")

    '            AddHandler oRepeater.ItemDataBound, AddressOf RPTfields_ItemDataBound
    '            oRepeater.DataSource = oDto.Fields
    '            oRepeater.DataBind()
    '        End If
    '    End Sub
    '    Private Sub RPTfields_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
    '        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '            Dim oDto As dtoSubmissionField = DirectCast(e.Item.DataItem, dtoSubmissionField)
    '            Dim oControl As Control
    '            Select Case oDto.Type
    '                Case FieldType.SingleLine
    '                    oControl = CType(e.Item.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.MultiLine
    '                    oControl = CType(e.Item.FindControl("CTRLmultiLine"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.Disclaimer
    '                    oControl = CType(e.Item.FindControl("CTRLdisclaimer"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.Mail
    '                    oControl = CType(e.Item.FindControl("CTRLmail"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.Name
    '                    oControl = CType(e.Item.FindControl("CTRLname"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.Surname
    '                    oControl = CType(e.Item.FindControl("CTRLsurname"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.DropDownList
    '                    oControl = CType(e.Item.FindControl("CTRLdropDownList"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.CheckboxList
    '                    oControl = CType(e.Item.FindControl("CTRLcheckboxList"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                    'Sviluppare gli UC appositi UC e poi sostituire
    '                Case FieldType.CompanyCode
    '                    oControl = CType(e.Item.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.CompanyTaxCode
    '                    oControl = CType(e.Item.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.TaxCode
    '                    oControl = CType(e.Item.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.TelephoneNumber
    '                    oControl = CType(e.Item.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.Time
    '                    oControl = CType(e.Item.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.VatCode
    '                    oControl = CType(e.Item.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    oControl.Visible = True
    '                Case FieldType.ZipCode
    '                    oControl = CType(e.Item.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    oControl.Visible = True
    '            End Select
    '            If Not oControl Is Nothing Then
    '                Dim oFieldControl As IViewsubmissionField
    '                oFieldControl = CType(oControl, IViewsubmissionField)
    '                oFieldControl.InitializeView(oDto)
    '            End If
    '            Dim oLiteral As Literal = e.Item.FindControl("LTtypeID")
    '            oLiteral.Text = oDto.Type
    '        End If
    '    End Sub
    '    Private Sub RPTrequestedFiles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTrequestedFiles.ItemDataBound
    '        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '            Dim oDto As dtoSubmissionFile = DirectCast(e.Item.DataItem, dtoSubmissionFile)
    '            Dim oSubmissionFile As UC_SubmissionFile = e.Item.FindControl("CTRLsubmissionFile")

    '            'oSubmissionFile.InitControl(oDto, Me.CallCommunityID)
    '        ElseIf e.Item.ItemType = ListItemType.Header Then
    '            Dim oLiteral As Literal = e.Item.FindControl("LTrequestedFiles")
    '            Me.Resource.setLiteral(oLiteral)
    '        End If
    '    End Sub
    '    Protected Sub RemoveFile(ByVal SubmittedFileID As Long)
    '        Me.CurrentPresenter.RemoveSubmittedFile(SubmittedFileID, GetFieldsValue)
    '    End Sub

    '#End Region

    '#Region "Items value"
    '    Private Function GetFieldsValue() As IList(Of dtoSubmissionField)
    '        Dim oList As IList(Of dtoSubmissionField) = New List(Of dtoSubmissionField)
    '        For Each RowSection As RepeaterItem In RPTsections.Items
    '            Dim oFieldRepeater As Repeater = RowSection.FindControl("RPTfields")
    '            For Each RowField As RepeaterItem In oFieldRepeater.Items
    '                Dim oControl As IViewsubmissionField
    '                Dim oLiteral As Literal = RowField.FindControl("LTtypeID")
    '                Dim fieldType As FieldType = DirectCast(CInt(oLiteral.Text), FieldType)
    '                Select Case fieldType
    '                    Case fieldType.SingleLine
    '                        oControl = CType(RowField.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    Case fieldType.MultiLine
    '                        oControl = CType(RowField.FindControl("CTRLmultiLine"), IViewsubmissionField)
    '                    Case fieldType.Disclaimer
    '                        oControl = CType(RowField.FindControl("CTRLdisclaimer"), IViewsubmissionField)
    '                        'Added
    '                    Case fieldType.Mail
    '                        oControl = CType(RowField.FindControl("CTRLmail"), IViewsubmissionField)
    '                    Case fieldType.Name
    '                        oControl = CType(RowField.FindControl("CTRLname"), IViewsubmissionField)
    '                    Case fieldType.Surname
    '                        oControl = CType(RowField.FindControl("CTRLsurname"), IViewsubmissionField)
    '                        'Case FieldType.CompanyCode
    '                        '    oControl = CType(RowField.FindControl("CTRLdisclaimer"), IViewsubmissionField)
    '                    Case fieldType.DropDownList
    '                        oControl = CType(RowField.FindControl("CTRLdropDownList"), IViewsubmissionField)
    '                    Case fieldType.CheckboxList
    '                        oControl = CType(RowField.FindControl("CTRLcheckboxList"), IViewsubmissionField)
    '                        'Sviluppare gli UC appositi UC e poi sostituire
    '                    Case fieldType.CompanyCode
    '                        oControl = CType(RowField.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    Case fieldType.CompanyTaxCode
    '                        oControl = CType(RowField.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    Case fieldType.TaxCode
    '                        oControl = CType(RowField.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    Case fieldType.TelephoneNumber
    '                        oControl = CType(RowField.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    Case fieldType.VatCode
    '                        oControl = CType(RowField.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    Case fieldType.Time
    '                        oControl = CType(RowField.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                    Case fieldType.ZipCode
    '                        oControl = CType(RowField.FindControl("CTRLsingleLine"), IViewsubmissionField)
    '                End Select
    '                If Not IsNothing(oControl) Then
    '                    Dim oField As dtoSubmissionField = New dtoSubmissionField()
    '                    oField.FieldDefinitionId = oControl.FieldDefinitionID
    '                    oField.FieldValueId = oControl.FieldValueID
    '                    oField.Value = oControl.FieldValue
    '                    oField.Type = fieldType
    '                    '
    '                    'oField.DisplayOrder =  
    '                    '
    '                    oList.Add(oField)
    '                End If
    '            Next
    '        Next
    '        Return oList
    '    End Function

    '    Public Function GetSubmittedFiles(ByVal submission As UserSubmission, ByVal serviceCode As String, ByVal serviceID As Integer, ByVal ServiceOwnerActionID As Integer, ByVal objectTypeID As Integer) As IList(Of dtoSubmissionUploadedFile) Implements IViewRFMsubmission.GetSubmittedFiles
    '        Dim oList As IList(Of dtoSubmissionUploadedFile) = New List(Of dtoSubmissionUploadedFile)
    '        For Each RowSection As RepeaterItem In RPTrequestedFiles.Items
    '            Dim oControl As UC_SubmissionFile = RowSection.FindControl("CTRLsubmissionFile")
    '            Dim oDto As dtoSubmissionUploadedFile = oControl.AddInternalFile(submission, serviceCode, serviceID, ServiceOwnerActionID, objectTypeID)
    '            If Not IsNothing(oDto) Then
    '                oList.Add(oDto)
    '            End If
    '        Next
    '        Return oList
    '    End Function
    '#End Region

    '    'Private Sub LNBsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsave.Click, LNBsaveBottom.Click
    '    '    Me.CurrentPresenter.SaveSubmission(GetFieldsValue)
    '    'End Sub

    '    'Private Sub LNBsaveAndSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsaveAndSubmit.Click, LNBsaveAndSubmitBottom.Click
    '    '    Dim oLocalizedMail As MailLocalized = PageUtility.LocalizedMail
    '    '    Dim CommunityPath As String = ""
    '    '    If Me.SystemSettings.File.Materiale.DrivePath = "" Then
    '    '        CommunityPath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath)
    '    '    Else
    '    '        CommunityPath = Me.SystemSettings.File.Materiale.DrivePath
    '    '    End If

    '    '    Me.CurrentPresenter.SaveCompleteSubmission(GetFieldsValue, oLocalizedMail.ServerSMTP, oLocalizedMail.SubjectPrefix, oLocalizedMail.RealMailSenderAccount, oLocalizedMail.SendMailByReply, Me.Resource.getValue("submissionConfirm.Subject"), Me.Resource.getValue("submissionConfirm.Body"), CommunityPath)
    '    'End Sub

    '    Public Sub SaveCompleteSubmission() Implements IViewRFMsubmission.SaveCompleteSubmission
    '        Dim oLocalizedMail As MailLocalized = PageUtility.LocalizedMail
    '        Dim CommunityPath As String = ""
    '        If Me.SystemSettings.File.Materiale.DrivePath = "" Then
    '            CommunityPath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath)
    '        Else
    '            CommunityPath = Me.SystemSettings.File.Materiale.DrivePath
    '        End If
    '        'Me.Resource.getValue("submissionConfirm.Subject"), Me.Resource.getValue("submissionConfirm.Body")
    '        Dim translations As New Dictionary(Of Boolean, String)
    '        translations.Add(True, Me.Resource.getValue("AcceptedDisclaimer"))
    '        translations.Add(False, Me.Resource.getValue("DeniedDisclaimer"))
    '        Me.CurrentPresenter.SaveCompleteSubmission("Replaced On", GetFieldsValue, PageUtility.CurrentSmtpConfig, translations, CommunityPath)
    '    End Sub

    '#End Region


    '    Public Sub NoPermission(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewRFMsubmission.NoPermission
    '        Me.BindNoPermessi()
    '    End Sub

    '    Public Sub LoadFieldsRequired(ByVal fields As IList(Of dtoFieldsSection(Of dtoFieldDefinition)), ByVal files As IList(Of dtoRequestedFile)) Implements IViewRFMsubmission.LoadFieldsRequired
    '        Me.MLVsubmission.SetActiveView(Me.VIWmessages)
    '        Me.DVemptyfields.Visible = (fields.Count <> 0)
    '        If fields.Count > 0 Then
    '            Me.RPTemptyFields.DataSource = fields
    '            Me.RPTemptyFields.DataBind()
    '        End If

    '        Me.DVemptyfiles.Visible = (files.Count <> 0)
    '        If files.Count > 0 Then
    '            Me.RPTemptyFiles.DataSource = files
    '            Me.RPTemptyFiles.DataBind()
    '        End If

    '        If fields.Count > 0 OrElse files.Count > 0 Then
    '            Me.LTinfoSubmission.Text = Me.Resource.getValue("SubmissionErrorView.RequiredItems")
    '        End If

    '        Me.LNBnext.Visible = False
    '        Me.LNBback.Visible = False

    '        'Me.BTNback.Visible = False
    '        'Me.BTNnext.Visible = False
    '    End Sub

    '    Public Sub LoadError(ByVal errorView As SubmissionErrorView, ByVal communityID As Integer, ByVal callForPaperId As Long, ByVal view As CallStatusForSubmitters) Implements IViewRFMsubmission.LoadError
    '        Me.MLVsubmission.SetActiveView(Me.VIWmessages)
    '        Me.DVemptyfiles.Visible = False
    '        Me.DVemptyfields.Visible = False
    '        Me.LTinfoSubmission.Text = Me.Resource.getValue("SubmissionErrorView." & errorView.ToString())
    '        Me.LNBnext.Visible = False
    '        Me.LNBback.Visible = False

    '        'Me.BTNback.Visible = False
    '        'Me.BTNnext.Visible = False

    '        ' Me.HYPlist.NavigateUrl = Me.PageUtility.BaseUrl & RootObject.ListCall(communityID, callForPaperId, view)
    '        'Me.HYPlistFromMessage.NavigateUrl = Me.PageUtility.BaseUrl & RootObject.ListCall(communityID, callForPaperId, view)
    '        'Me.HYPlistFromMessageBottom.NavigateUrl = Me.PageUtility.BaseUrl & RootObject.ListCall(communityID, callForPaperId, view)
    '        'Me.LNBretry.Visible = (errorView <> SubmissionErrorView.SubmissionDeleted AndAlso errorView <> SubmissionErrorView.SubmissionTimeExpired)
    '        'Me.LNBretryBottom.Visible = Me.LNBretry.Visible
    '    End Sub
    '    Public Sub LoadDefaultList(ByVal communityID As Integer, ByVal callForPaperId As Long, ByVal view As CallStatusForSubmitters) Implements IViewRFMsubmission.LoadDefaultList
    '        PageUtility.RedirectToUrl(RootObject.ListCall(communityID, callForPaperId, view))
    '    End Sub

    '    Public Sub LoadFinalResume(ByVal RequestForMembershipID As Long) Implements IViewRFMsubmission.LoadFinalResume
    '        PageUtility.RedirectToUrl(RfM_RootObject.FinalResume(RequestForMembershipID))
    '    End Sub

    '    Public Sub LoadCompleteSubmissionMessage(ByVal communityID As Integer, ByVal callForPaperId As Long, ByVal submissionId As Long, ByVal view As CallStatusForSubmitters) Implements IViewRFMsubmission.LoadCompleteSubmissionMessage
    '        Me.MLVsubmission.SetActiveView(Me.VIWmessages)
    '        Me.DVemptyfiles.Visible = False
    '        Me.DVemptyfields.Visible = False
    '        Me.LTinfoSubmission.Text = Me.Resource.getValue("CompleteSubmissionMessage")
    '        'Me.HYPlist.NavigateUrl = Me.PageUtility.BaseUrl & RootObject.ListCall(communityID, callForPaperId, view)
    '        'Me.HYPlistFromMessage.NavigateUrl = Me.PageUtility.BaseUrl & RootObject.ListCall(communityID, callForPaperId, view)
    '        'Me.HYPlistFromMessageBottom.NavigateUrl = Me.PageUtility.BaseUrl & RootObject.ListCall(communityID, callForPaperId, view)
    '    End Sub
    '    Private Sub LNBretry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBretryBottom.Click
    '        If Me.DVemptyfiles.Visible OrElse Me.DVemptyfields.Visible Then
    '            Me.MLVsubmission.SetActiveView(VIWsubmission)
    '            Me.CurrentPresenter.LoadSubmission()
    '            Me.LNBback.Visible = True
    '            Me.LNBnext.Visible = True
    '            'Me.BTNnext.Visible = True
    '            'BTNback.Visible = True
    '        Else
    '            PageUtility.RedirectToUrl(RootObject.ViewSubmission(Me.RequestForMembershipID, Me.SubmissionID, CallStatusForSubmitters.Submitted))
    '        End If
    '    End Sub

    '    Public ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewRFMsubmission.PreloadView
    '        Get
    '            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
    '        End Get
    '    End Property

    '    Public Sub SetBackUrl(ByVal communityID As Integer, ByVal callForPaperId As Long, ByVal view As CallStatusForSubmitters) Implements IViewRFMsubmission.SetBackUrl
    '        'Me.HYPlist.NavigateUrl = Me.PageUtility.BaseUrl & RootObject.ListCall(communityID, callForPaperId, view)
    '        'Me.HYPlistFromMessage.NavigateUrl = Me.PageUtility.BaseUrl & RootObject.ListCall(communityID, callForPaperId, view)
    '    End Sub

    '    'Private Sub LNBdelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdelete.Click
    '    '    Me.CurrentPresenter.VirtualDeleteSubmission(PageUtility.ClientIPadress, PageUtility.ProxyIPadress)
    '    'End Sub

    '    Public Sub LoadSubmissionView(ByVal callForPaperId As Long, ByVal submissionId As Long, ByVal view As CallStatusForSubmitters) Implements IViewRFMsubmission.LoadSubmissionView
    '        PageUtility.RedirectToUrl(RootObject.ViewSubmission(callForPaperId, submissionId, CallStatusForSubmitters.Submitted))
    '    End Sub

    '    Public ReadOnly Property CurrentStep As SubmissionToRFMwizardStep Implements IViewRFMsubmission.CurrentStep
    '        Get
    '            Return ViewStateOrDefault("CurrentStep", SubmissionToRFMwizardStep.None)
    '        End Get
    '    End Property

    '    'Private Sub BTNnext_Click(sender As Object, e As System.EventArgs) Handles BTNnext.Click

    '    'End Sub

    '    'Private Sub BTNback_Click(sender As Object, e As System.EventArgs) Handles BTNback.Click

    '    'End Sub

    '    Private Sub LNBback_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBback.Click
    '        If Page.IsValid Then
    '            Me.CurrentPresenter.MoveToPreviousStep(CurrentStep)
    '        End If
    '    End Sub

    '    Private Sub LNBnext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBnext.Click
    '        If Page.IsValid Then
    '            Me.CurrentPresenter.MoveToNextStep(CurrentStep)
    '        End If
    '    End Sub

    '    Private Function GetDisclaimerValue(ByVal str As String) As String Implements IViewRFMsubmission.GetDisclaimerValue
    '        Dim result As String = ""
    '        If str = "True" Then
    '            result = Me.Resource.getValue("AcceptedDisclaimer")
    '        ElseIf str = "False" Then
    '            result = Me.Resource.getValue("DeniedDisclaimer")
    '        Else
    '            result = Me.Resource.getValue("UncheckedDisclaimer")
    '        End If
    '        Return result
    '    End Function

    '    Private Function CheckBoxListEmptyMessage() As String Implements IViewRFMsubmission.CheckBoxListEmptyMessage
    '        Dim msg As String = Me.Resource.getValue("CBLnotSelected")
    '        Return msg
    '    End Function



    '    Public Sub GotoStep(pStep As SubmissionToRFMwizardStep) Implements IViewRFMsubmission.GotoStep

    '        Select Case pStep
    '            Case SubmissionToRFMwizardStep.Disclaimer
    '                MLVsubmission.SetActiveView(VIWsubmission)
    '                Me.ViewState("CurrentStep") = SubmissionToRFMwizardStep.Disclaimer
    '            Case SubmissionToRFMwizardStep.SubmissionFields 'redirect vs nuova pagina complete info
    '                MLVsubmission.SetActiveView(VIWfields)
    '                Me.ViewState("CurrentStep") = SubmissionToRFMwizardStep.SubmissionFields

    '            Case SubmissionToRFMwizardStep.CompleteInfo
    '                Me.ViewState("CurrentStep") = SubmissionToRFMwizardStep.CompleteInfo
    '                PageUtility.RedirectToUrl(RfM_RootObject.FinalResume(Me.RequestForMembershipID))
    '        End Select
    '        Me.LNBback.Visible = (pStep <> SubmissionToRFMwizardStep.Disclaimer)
    '        Me.LNBnext.Visible = (pStep <> SubmissionToRFMwizardStep.CompleteInfo)
    '        'Me.BTNback.Visible = (pStep <> SubmissionToRFMwizardStep.Disclaimer)
    '        'Me.BTNnext.Visible = (pStep <> SubmissionToRFMwizardStep.CompleteInfo)

    '    End Sub



    '    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    '    End Sub


    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        PageUtility.RedirectToUrl(lm.Comol.Modules.CallForPapers.Domain.RootObject.StartNewSubmission(lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.RequestForMembership, PreloadRequestForMembershipID, True, FromPublicList, lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.None, 0))
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
End Class