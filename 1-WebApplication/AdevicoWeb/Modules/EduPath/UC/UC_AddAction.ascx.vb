Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Presentation
Imports lm.Comol.Modules.EduPath.BusinessLogic
Public Class UC_AddAction
    Inherits BaseControl

#Region "Contex"
    Protected _serviceEP As lm.Comol.Modules.EduPath.BusinessLogic.Service
    Protected ReadOnly Property ServiceEP As Service
        Get
            If IsNothing(_serviceEP) Then
                _serviceEP = New Service(Me.PageUtility.CurrentContext)
            End If
            Return _serviceEP
        End Get
    End Property
    Protected _serviceTemplate As lm.Comol.Core.DomainModel.DocTemplateVers.Business.DocTemplateVersService
    Protected ReadOnly Property ServiceTemplate As lm.Comol.Core.DomainModel.DocTemplateVers.Business.DocTemplateVersService
        Get
            If IsNothing(_serviceTemplate) Then
                _serviceTemplate = New lm.Comol.Core.DomainModel.DocTemplateVers.Business.DocTemplateVersService(Me.PageUtility.CurrentContext)
            End If
            Return _serviceTemplate
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Property"
    Public Event UpdateContainer()
    Public Event UpdateAndCloseContainer(ByVal idSubActivity As Long)
    Public Event GetScriptManager(ByRef manager As ScriptManager)
    Public Property IsInAjaxPanel As Boolean
        Get
            Return ViewStateOrDefault("IsInAjaxPanel", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInAjaxPanel") = value
        End Set
    End Property
    Private Property IdActionCommunity As Integer
        Get
            Return ViewStateOrDefault("IdActionCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdActionCommunity") = value
        End Set
    End Property
    Private Property IdActionPath As Long
        Get
            Return ViewStateOrDefault("IdActionPath", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdActionPath") = value
        End Set
    End Property
    Private Property IdActionUnit As Long
        Get
            Return ViewStateOrDefault("IdActionUnit", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdActionUnit") = value
        End Set
    End Property
    Private Property IdActionActivity As Long
        Get
            Return ViewStateOrDefault("IdActionActivity", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdActionActivity") = value
        End Set
    End Property
    Public Enum ActionView
        SelectAction = 1
        RepositoryAction = 2
        QuestionAction = 3
        CertificationAction = 4
        TextAction = 5
    End Enum
    Public ReadOnly Property EditorClientId As String
        Get
            Return CTRLtextAction.EditorClientId
        End Get
    End Property
    Public Property isInitialized As Boolean
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ViewActivity", "EduPath")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(Me.LBsubActQuiz)
            .setLabel(Me.LBsubActRepository)
            .setLabel(Me.LBsubActText)
            .setButton(Me.BTNaddQuestionario, True)
            .setButton(Me.BTNaddRepositoryActivity, True)
            .setButton(Me.BTNaddCertificationAction, True)
            .setButton(Me.BTNaddTextAction, True)

            .setButton(Me.BTNaddWebinarAction, True)

            .setLabel(Me.LBsubActCertification)
            .setLabel(LBsubActWebinar)

            .setButton(BTNcloseAddActionWindowTop, True)
            .setButton(BTNselectActionTop, True)
            .setButton(BTNcreateActionTop, True)

            .setButton(BTNcloseAddActionWindowBottom, True)
            .setButton(BTNselectActionBottom, True)
            .setButton(BTNcreateActionBottom, True)
            .setLiteral(LTcurrentAction)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(manager As ScriptManager, ByVal idCommunity As Integer, ByVal idPath As Long, ByVal idUnit As Long, ByVal idActivity As Long)
        IdActionPath = idPath
        IdActionCommunity = idCommunity
        IdActionActivity = idActivity
        IdActionUnit = idUnit
        'Me.CTRLmoduleToRepository.PageScriptManager = manager
        'Me.HYPaddSubText.NavigateUrl = Me.BaseUrl & RootObject.AddSubActText(idActivity, idCommunity)
        Me.CTRLmoduleToRepository.SetInternazionalizzazione()
        If Not isInitialized Then
            DVcertifications.Visible = ServiceTemplate.HasAvailableTemplates(ServiceEP.ServiceModuleID())
            DVquestionnaire.Visible = ServiceEP.IsServiceActive(COL_Questionario.ModuleQuestionnaire.UniqueID, idCommunity)
        End If
        isInitialized = True
    End Sub
    Public Sub SetActionvView(view As ActionView)
        Me.MLVaddSubActivity.ActiveViewIndex = CInt(view)
    End Sub

#Region "Sub Activity"

#Region "Repository"
    Private Sub BTNaddRepositoryActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddRepositoryActivity.Click
        Me.MLVaddSubActivity.SetActiveView(Me.VIWrepository)
        ' QUI SI POTRREBBE AGGIUNGERE IL CODICE PER AVERE I FILE GIA ASSOCIATI ALL?ACTIVITY ED EVITARNE LA RISELEZIONE
        Dim manager As ScriptManager
        RaiseEvent GetScriptManager(manager)

        Me.CTRLmoduleToRepository.AjaxInitializeControl(Me.IdActionCommunity, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex, COL_BusinessLogic_v2.UCServices.Services_EduPath.ActionType.DoSubActivity, lm.Comol.Core.DomainModel.FileRepositoryType.InternalLong, manager)
        If IsInAjaxPanel Then
            RaiseEvent UpdateContainer()
        End If
    End Sub

    Private Sub CTRLmoduleToRepository_CloseClientWindow() Handles CTRLmoduleToRepository.CloseClientWindow
        RaiseEvent UpdateAndCloseContainer(0)
    End Sub

    Private Sub CTRLmoduleToRepository_ContainerCommands(show As Boolean) Handles CTRLmoduleToRepository.ContainerCommands
        Me.DVcommandsTop.Visible = show
        Me.DVcommandsBottom.Visible = show
        BTNselectActionTop.Visible = show
        BTNselectActionBottom.Visible = show

        Me.BTNcreateActionBottom.Visible = False
        Me.BTNcreateActionTop.Visible = False
    End Sub

    'summary
    '   ADD ACTION TO ITEM/S OF COMMUNITY REPOSITORY
    '
    Private Sub CTRLmoduleToRepository_LinkedModuleObjects(ByVal links As List(Of lm.Comol.Core.DomainModel.ModuleLink)) Handles CTRLmoduleToRepository.LinkedModuleObjects
        Try
            For Each item As ModuleLink In links
                Dim oSubActivity As New SubActivity

                oSubActivity.CreatedOn = Now
                oSubActivity.IdObjectLong = item.DestinationItem.ObjectLongID
                oSubActivity.IdModuleAction = item.Action
                oSubActivity.Description = item.Description
                oSubActivity.Link = item.Link
                oSubActivity.ContentPermission = item.Permission
                oSubActivity.Name = ""
                oSubActivity.IdModule = item.DestinationItem.ServiceID
                oSubActivity.CodeModule = item.DestinationItem.ServiceCode
                oSubActivity.ContentType = SubActivityType.File
                Dim ModuleID As Long = PageUtility.GetModuleID(COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex)
                Dim Inserted As SubActivity = ServiceEP.SaveOrUpdateSubActivity(oSubActivity, item, ModuleID, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex, IdActionActivity, IdActionCommunity, PageUtility.CurrentContext.UserContext.CurrentUserID, OLDpageUtility.ProxyIPadress, OLDpageUtility.ClientIPadress)
            Next
        Catch ex As Exception

        End Try
        Me.PageUtility.RedirectToUrl(RootObject.ViewActivity(IdActionActivity, IdActionUnit, IdActionPath, IdActionCommunity, EpViewModeType.Manage))
    End Sub


    Private Sub CTRLmoduleToRepository_AddedModuleObjects(ByVal items As List(Of ModuleActionLink)) Handles CTRLmoduleToRepository.AddedModuleObjects
        Dim Links As New List(Of ModuleLink)
        Dim InternalObjectsToRemove As New List(Of iModuleObject)
        Try
            Dim ModuleID As Long = PageUtility.GetModuleID(COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex)
            For Each item As ModuleActionLink In items
                Dim oSubActivity As New SubActivity

                oSubActivity.CreatedOn = Now
                oSubActivity.IdObjectLong = item.ModuleObject.ObjectLongID
                oSubActivity.IdModuleAction = item.Action
                oSubActivity.Description = item.Description
                oSubActivity.Link = item.Link
                oSubActivity.ContentPermission = item.Permission
                oSubActivity.Name = ""
                oSubActivity.IdModule = item.ModuleObject.ServiceID
                oSubActivity.CodeModule = item.ModuleObject.ServiceCode
                oSubActivity.ContentType = SubActivityType.File
                Dim oModuleLink As New ModuleLink(item.Link, item.Permission, item.Action)
                oModuleLink.Description = item.Description
                oModuleLink.DestinationItem = item.ModuleObject
                oModuleLink.EditEnabled = item.EditEnabled
                Dim oInserted As SubActivity = ServiceEP.SaveOrUpdateSubActivity(oSubActivity, oModuleLink, ModuleID, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex, IdActionActivity, IdActionCommunity, PageUtility.CurrentContext.UserContext.CurrentUserID, OLDpageUtility.ProxyIPadress, OLDpageUtility.ClientIPadress)

                If IsNothing(oInserted) Then
                    InternalObjectsToRemove.Add(item.ModuleObject)
                Else

                    'oModuleLink.SourceItem = ModuleObject.CreateLongObject(oSubActivity.Id, oSubActivity, COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex, ModuleID)
                    Links.Add(oModuleLink)
                End If
            Next
        Catch ex As Exception

        End Try

        If Links.Count > 0 Then
            Me.CTRLmoduleToRepository.UpdateModuleInternalFile(Links)
        End If
        If InternalObjectsToRemove.Count > 0 Then
            Me.CTRLmoduleToRepository.RemoveUploadedInternalFiles(InternalObjectsToRemove)
        End If

        'Me.CloseDialog("addSubActivity")
        'Me.MLVaddSubActivity.ActiveViewIndex = 0
        'Me.UDPaddActivity.Update()
        Me.PageUtility.RedirectToUrl(RootObject.ViewActivity(IdActionActivity, IdActionUnit, IdActionPath, IdActionCommunity, EpViewModeType.Manage))

    End Sub

    Private Sub CTRLmoduleToRepository_EmptyUpload() Handles CTRLmoduleToRepository.EmptyUpload
        Me.MLVaddSubActivity.SetActiveView(VIWselector)
        If IsInAjaxPanel Then
            RaiseEvent UpdateAndCloseContainer(0)
        End If
    End Sub
    Private Sub CTRLmoduleToRepository_UpdateAjaxPanel() Handles CTRLmoduleToRepository.UpdateAjaxPanel
        '# Me.UDPaddActivity.Update()
    End Sub
#End Region

#Region "Question"
    Private Sub BTNaddQuestionario_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddQuestionario.Click
        Me.MLVaddSubActivity.SetActiveView(Me.VIWquiz)
        Me.CTRLmoduleToQuiz.InitializeControl(Me.IdActionCommunity, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex, COL_BusinessLogic_v2.UCServices.Services_EduPath.ActionType.DoSubActivity, Request.QueryString("AId"), OwnerType_enum.EduPathSubActivity)
        BTNselectActionTop.Visible = True
        BTNselectActionBottom.Visible = True

        Me.BTNcreateActionBottom.Visible = False
        Me.BTNcreateActionTop.Visible = False

        'RedirectToUrl(RootObject.CreateQuiz(IdActionActivity, OwnerType_enum.EduPathSubActivity))
    End Sub
    Private Sub CTRLmoduleToQuiz_CloseClientWindow() Handles CTRLmoduleToQuiz.CloseClientWindow
        RaiseEvent UpdateAndCloseContainer(0)
    End Sub
    Private Sub CTRLmoduleToQuiz_ContainerCommands(show As Boolean) Handles CTRLmoduleToQuiz.ContainerCommands
        Me.DVcommandsTop.Visible = show
        Me.DVcommandsBottom.Visible = show
        BTNselectActionTop.Visible = show
        BTNselectActionBottom.Visible = show

        Me.BTNcreateActionBottom.Visible = False
        Me.BTNcreateActionTop.Visible = False
    End Sub
#End Region

#Region "Text Action"
    Private Sub BTNaddTextAction_Click(sender As Object, e As System.EventArgs) Handles BTNaddTextAction.Click
        Me.MLVaddSubActivity.SetActiveView(Me.VIWtextAction)
        Me.LTcurrentAction.Text = Resource.getValue("Selected.TextAction")
        BTNselectActionTop.Visible = True
        BTNselectActionBottom.Visible = True
        Me.BTNcreateActionBottom.Visible = True
        Me.BTNcreateActionTop.Visible = True

        Dim visible As Boolean = Me.CTRLtextAction.InitializeControl(IdActionCommunity, IdActionPath, IdActionUnit, IdActionActivity)
        Dim result As Long = Me.CTRLtextAction.SaveAction(IdActionCommunity, PageUtility.CurrentContext.UserContext.CurrentUserID)
        If result > 0 Then
            PageUtility.RedirectToUrl(RootObject.ViewActivity(IdActionActivity, IdActionUnit, IdActionPath, IdActionCommunity, EpViewModeType.Manage))
        Else
            RaiseEvent UpdateAndCloseContainer(result)
        End If
    End Sub
#End Region

#Region "Certification Action"
    Private Sub BTNaddCertificationAction_Click(sender As Object, e As System.EventArgs) Handles BTNaddCertificationAction.Click
        Me.MLVaddSubActivity.SetActiveView(Me.VIWcertifications)
        Me.LTcurrentAction.Text = Resource.getValue("Selected.Certification")
        BTNselectActionTop.Visible = True
        BTNselectActionBottom.Visible = True
        Me.BTNcreateActionBottom.Visible = True
        Me.BTNcreateActionTop.Visible = True


        CTRLcertificationAction.InitializeControl(IdActionCommunity, IdActionPath, IdActionUnit, IdActionActivity)

        If IsInAjaxPanel Then
            RaiseEvent UpdateContainer()
        End If
    End Sub
#End Region

    Private Sub BTNundoTextAction_Click(sender As Object, e As System.EventArgs) Handles BTNselectActionBottom.Click, BTNselectActionTop.Click
        'Dim view As View = MLVaddSubActivity.GetActiveView
        'If view Is VIWcertifications OrElse view Is VIWtextAction Then
        Me.MLVaddSubActivity.SetActiveView(VIWselector)
        Me.Resource.setLiteral(LTcurrentAction)

        'ElseIf view Is VIWrepository Then
        '    If CTRLmoduleToRepository.CurrentAction = lm.Comol.Core.BaseModules.Repository.Domain.UserRepositoryAction.None Then
        '        Me.MLVaddSubActivity.SetActiveView(VIWselector)
        '        Me.Resource.setLiteral(LTcurrentAction)
        '    Else
        '        Me.CTRLmoduleToRepository.ChangeDisplayAction(lm.Comol.Core.BaseModules.Repository.Domain.UserRepositoryAction.SelectAction)
        '        LTcurrentAction.Text = CTRLmoduleToRepository.GetCurrentActionTitle()
        '    End If
        'ElseIf view Is VIWquiz Then
        '    Me.MLVaddSubActivity.SetActiveView(VIWselector)
        '    Me.Resource.setLiteral(LTcurrentAction)
        'End If
        Me.DVcommandsBottom.Visible = True
        Me.DVcommandsTop.Visible = True
        BTNselectActionTop.Visible = False
        BTNselectActionBottom.Visible = False
        Me.BTNcreateActionBottom.Visible = False
        Me.BTNcreateActionTop.Visible = False
        If IsInAjaxPanel Then
            RaiseEvent UpdateContainer()
        End If
    End Sub
#End Region
  
    Private Sub BTNcloseAddActionWindow_Click(sender As Object, e As System.EventArgs) Handles BTNcloseAddActionWindowBottom.Click, BTNcloseAddActionWindowTop.Click
        PageUtility.RedirectToUrl(RootObject.ViewActivity(IdActionActivity, IdActionUnit, IdActionPath, IdActionCommunity, EpViewModeType.Manage))
    End Sub

    Private Sub BTNcreateActionTop_Click(sender As Object, e As System.EventArgs) Handles BTNcreateActionTop.Click, BTNcreateActionBottom.Click
        Dim view As View = MLVaddSubActivity.GetActiveView
        If view Is VIWtextAction Then
            Dim result As Long = Me.CTRLtextAction.SaveAction(IdActionCommunity, PageUtility.CurrentContext.UserContext.CurrentUserID)
            If result > 0 Then
                PageUtility.RedirectToUrl(RootObject.ViewActivity(IdActionActivity, IdActionUnit, IdActionPath, IdActionCommunity, EpViewModeType.Manage))
            Else
                If IsInAjaxPanel Then
                    If result > 0 Then
                        RaiseEvent UpdateAndCloseContainer(result)
                    Else
                        RaiseEvent UpdateContainer()
                    End If
                End If
            End If
        ElseIf view Is VIWcertifications Then
            Dim idSubactivity As Long = Me.CTRLcertificationAction.SaveSettings()
            If IsInAjaxPanel Then
                RaiseEvent UpdateAndCloseContainer(idSubactivity)
            End If
        End If
    End Sub


End Class