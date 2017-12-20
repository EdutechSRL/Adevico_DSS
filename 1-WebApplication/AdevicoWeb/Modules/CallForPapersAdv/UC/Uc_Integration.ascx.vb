Imports lm.Comol.Modules.CallForPapers.Advanced.dto
Imports lm.Comol.Modules.CallForPapers.Advanced.Presentation.iView
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports Adv = lm.Comol.Modules.CallForPapers.Advanced

Public Class Uc_Integration
    Inherits BaseControl
    Implements Adv.Presentation.iView.iViewUcIntegration

    Public Sub InitializeUc(CommId As Long, SubmissId As Long, SubmFieldId As Long, SubmittId As Integer, CommunityId As Integer)

        Me.SubmissionId = SubmissId
        Me.FieldId = SubmFieldId
        Me.CommissionId = CommId
        _communityId = CommunityId
        Me.SubmitterId = SubmittId

        Me.CurrentPresenter.InitView(CommId, SubmissionId, SubmFieldId, SubmittId)
    End Sub


    Private Property _communityId As Integer
        Get
            Return ViewStateOrDefault("CurrentCommId", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentCommId") = value
        End Set
    End Property

#Region "Context"
    Private _Presenter As Adv.Presentation.AdvUcIntegrationPresenter
    Private ReadOnly Property CurrentPresenter() As Adv.Presentation.AdvUcIntegrationPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New Adv.Presentation.AdvUcIntegrationPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub



#Region "BaseView"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

    Public Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout

    End Sub

    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission

    End Sub

#End Region

#Region "View"
    Public Sub BindView(integration As dtoIntegration) Implements iViewUcIntegration.BindView

        If IsNothing(integration) Then
            Me.RPTintegrations.Visible = False
            Return
        End If


        SecretaryId = integration.SecretaryId
        _isSecretary = integration.IsSecretary
        _isSubmitter = integration.IsSubmitter

        PNLAdd.Visible = False

        Dim hasItem As Boolean = Not IsNothing(integration.items) AndAlso integration.items.Any()

        If _isSecretary Then
            PNLAdd.Visible = Not hasItem OrElse integration.items.All(Function(i) i.IsSended)
        End If

        If hasItem Then
            Me.RPTintegrations.Visible = True
            Me.RPTintegrations.DataSource = integration.items
            Me.RPTintegrations.DataBind()
        Else
            Me.RPTintegrations.Visible = False
        End If

    End Sub


#End Region

#Region "Internal Property"
    Private Property SubmissionId As Long
        Get
            Dim value As Long = 0
            Try
                value = System.Convert.ToInt64(HDNSubmissionId.Value)
            Catch ex As Exception

            End Try

            Return value
        End Get
        Set(value As Long)
            HDNSubmissionId.Value = value
        End Set
    End Property

    Private Property FieldId As Long
        Get
            Dim value As Long = 0
            Try
                value = System.Convert.ToInt64(HDNSubmissionFieldId.Value)
            Catch ex As Exception

            End Try

            Return value
        End Get
        Set(value As Long)
            HDNSubmissionFieldId.Value = value
        End Set
    End Property

    Private Property CommissionId As Long
        Get
            Dim value As Long = 0
            Try
                value = System.Convert.ToInt64(HDNCommissionId.Value)
            Catch ex As Exception

            End Try

            Return value
        End Get
        Set(value As Long)
            HDNCommissionId.Value = value
        End Set
    End Property


    Private Property SecretaryId As Long
        Get
            Dim value As Long = 0
            Try
                value = System.Convert.ToInt64(HDNSecretaryId.Value)
            Catch ex As Exception

            End Try

            Return value
        End Get
        Set(value As Long)
            HDNSecretaryId.Value = value
        End Set
    End Property

    Private Property SubmitterId As Long Implements iViewUcIntegration.SubmitterId
        Get
            Dim value As Long = 0
            Try
                value = System.Convert.ToInt64(HDNSubmitterId.Value)
            Catch ex As Exception

            End Try

            Return value
        End Get
        Set(value As Long)
            HDNSubmitterId.Value = value
        End Set
    End Property

    Private _isSecretary As Boolean = False
    Private _isSubmitter As Boolean = False
#End Region

    Private Sub RPTintegrations_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTintegrations.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim integr As dtoIntegrationItem = e.Item.DataItem
            If Not IsNothing(integr) Then

                Dim RBLtype As RadioButtonList = e.Item.FindControl("RBLtype")
                If Not IsNothing(RBLtype) Then
                    If integr.Type = Adv.IntegrationType.TextnFile Then
                        RBLtype.SelectedValue = "textfile"
                    Else
                        RBLtype.SelectedValue = "text"
                    End If

                    RBLtype.Enabled = Not integr.IsSended
                End If

                Dim HDN As HiddenField = e.Item.FindControl("HDNintegrationId")
                If Not IsNothing(HDN) Then
                    HDN.Value = integr.Id
                End If

                Dim TXB As TextBox = e.Item.FindControl("TXBrequest")
                If Not IsNothing(TXB) Then
                    TXB.Text = integr.RequestText
                    TXB.Visible = _isSecretary AndAlso Not integr.IsSended
                End If

                Dim LB As Label = e.Item.FindControl("LBLrequest")
                If Not IsNothing(LB) Then
                    LB.Text = TXB.Text
                    LB.Visible = Not TXB.Visible
                End If

                LB = e.Item.FindControl("LBLreqsendedOn")
                If Not IsNothing(LB) Then

                    If integr.IsSended Then
                        LB.Text = integr.SendedOn.ToString()
                    Else
                        LB.Text = "In bozza"
                    End If
                End If

                Dim LKB As LinkButton = e.Item.FindControl("LKBsave")
                If Not IsNothing(LKB) Then

                    If Not integr.IsSended AndAlso _isSecretary Then
                        LKB.Visible = True
                        LKB.CommandName = "Save"
                        LKB.CommandArgument = integr.Id
                    Else
                        LKB.Visible = False
                    End If
                End If

                LKB = e.Item.FindControl("LKBsend")
                If Not IsNothing(LKB) Then

                    If Not integr.IsSended AndAlso _isSecretary Then
                        LKB.Visible = True
                        LKB.CommandName = "Send"
                        LKB.CommandArgument = integr.Id
                    Else
                        LKB.Visible = False
                    End If
                End If

                Dim PNL As Panel = e.Item.FindControl("PNLsubmitter")
                If Not IsNothing(PNL) Then

                    PNL.Visible = False

                    If Not integr.IsSended Then
                        PNL.Visible = False
                    Else
                        If integr.IsAnswered OrElse _isSubmitter Then
                            PNL.Visible = True
                        End If
                    End If

                    If PNL.Visible Then

                        Dim PNLfile As Panel = e.Item.FindControl("PNLfile")

                        If Not IsNothing(PNLfile) Then
                            If integr.Type = Adv.IntegrationType.Text Then
                                PNLfile.Visible = False
                            Else

                                Dim CTRLdisplayFile As UC_ModuleRepositoryAction = e.Item.FindControl("CTRLdisplayFile")
                                Dim CTRLfileUploader As UC_CompactInternalFileUploader2 = e.Item.FindControl("CTRLfileUploader")
                                'Dim LKBupload As LinkButton = e.Item.FindControl("LKBupload")

                                If Not IsNothing(integr.Link) Then

                                    'LKBupload.Visible = False
                                    CTRLfileUploader.Visible = False

                                    CTRLdisplayFile.Visible = True

                                    Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

                                    initializer.IconSize = lm.Comol.Core.DomainModel.Helpers.IconSize.Small
                                    CTRLdisplayFile.EnableAnchor = True

                                    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction _
                                        Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions

                                    initializer.Link = integr.Link
                                    CTRLdisplayFile.InsideOtherModule = True
                                    Dim actions As List(Of lm.Comol.Core.DomainModel.dtoModuleActionControl)
                                    actions = CTRLdisplayFile.InitializeRemoteControl(initializer, initializer.Display)
                                Else
                                    'LKBupload.Visible = True

                                    'LKBupload.CommandName = "Upload"
                                    'LKBupload.CommandArgument = integr.Id

                                    CTRLfileUploader.Visible = True
                                    CTRLfileUploader.InitializeControl(_communityId, False)

                                    CTRLdisplayFile.Visible = False

                                End If
                            End If
                        End If

                        TXB = e.Item.FindControl("TXBanswer")
                        If Not IsNothing(TXB) Then
                            TXB.Text = integr.AswerText
                            TXB.Visible = _isSubmitter AndAlso Not integr.IsAnswered
                        End If

                        LB = e.Item.FindControl("LBLanswer")
                        If Not IsNothing(LB) Then
                            LB.Text = integr.AswerText
                            LB.Visible = Not TXB.Visible
                        End If

                        LB = e.Item.FindControl("LBLansSended")
                        If Not IsNothing(LB) Then

                            If integr.IsAnswered Then
                                LB.Text = integr.AnsweredOn.ToString()
                            Else
                                LB.Text = "In bozza"
                            End If
                        End If

                        LKB = e.Item.FindControl("LKBansSave")
                        If Not IsNothing(LKB) Then

                            If Not integr.IsAnswered AndAlso _isSubmitter Then
                                LKB.Visible = True
                                LKB.CommandName = "AnsSave"
                                LKB.CommandArgument = integr.Id
                            Else
                                LKB.Visible = False
                            End If
                        End If

                        LKB = e.Item.FindControl("LKBansSend")
                        If Not IsNothing(LKB) Then

                            If Not integr.IsAnswered AndAlso _isSubmitter Then
                                LKB.Visible = True
                                LKB.CommandName = "AnsSend"
                                LKB.CommandArgument = integr.Id
                            Else
                                LKB.Visible = False
                            End If
                        End If



                    End If
                End If

            End If
        End If
    End Sub

    Private Sub LKBaddsave_Click(sender As Object, e As EventArgs) Handles LKBaddsave.Click
        Add(False)
    End Sub

    Private Sub LKBaddsend_Click(sender As Object, e As EventArgs) Handles LKBaddsend.Click
        Add(True)
    End Sub

    Private Sub Add(ByVal send As Boolean)

        Dim type As Adv.IntegrationType = Adv.IntegrationType.Text

        Select Case Me.RBLtype.SelectedValue
            Case "text"
                type = Adv.IntegrationType.Text
            Case "textfile"
                type = Adv.IntegrationType.TextnFile
        End Select

        Me.CurrentPresenter.SaveRequest(
            Me.CommissionId,
            Me.TXBaddtext.Text,
            type,
            SubmissionId,
            Me.FieldId,
            Me.SubmitterId,
            0,
            send
        )

        Me.TXBaddtext.Text = ""
    End Sub

    Public Sub ForceBind() Implements iViewUcIntegration.ForceBind
        Me.CurrentPresenter.InitView(Me.CommissionId, Me.SubmissionId, Me.FieldId, Me.SubmitterId)
    End Sub

    Private Sub RPTintegrations_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTintegrations.ItemCommand


        Dim idIntegration As Long = 0
        If IsNumeric(e.CommandArgument) Then
            idIntegration = CLng(e.CommandArgument)
        End If

        'If e.CommandName = "Upload" Then

        '    Dim moduleCode As String = ""
        '    Dim idModule As Integer = 0
        '    Dim moduleAction As Integer = 0
        '    Dim objectType As Integer = 0


        '    Dim integration As lm.Comol.Modules.CallForPapers.Advanced.Domain.AdvEvalIntegration =
        '        Me.CurrentPresenter.GetIntegrationForUpload(idIntegration, moduleCode, idModule, moduleAction, objectType)

        '    If IsNothing(integration) Then
        '        Return
        '    End If

        '    Dim CTRLfileUploader As UC_CompactInternalFileUploader = e.Item.FindControl("CTRLfileUploader")

        '    If IsNothing(CTRLfileUploader) Then
        '        Return
        '    End If

        '    Dim maLink As lm.Comol.Core.DomainModel.ModuleActionLink =
        '        CTRLfileUploader.UploadAndLinkInternalFile(
        '        lm.Comol.Core.DomainModel.FileRepositoryType.InternalLong,
        '        integration,
        '        moduleCode,
        '        moduleAction,
        '        objectType)

        '    Dim text As String = ""
        '    Dim TXBanswer As TextBox = e.Item.FindControl("TXBanswer")
        '    If Not IsNothing(text) Then
        '        text = TXBanswer.Text
        '    End If

        '    Me.CurrentPresenter.IntegrationFileUpload(idIntegration, maLink, text, Me.CommissionId, Me.SubmissionId, Me.FieldId, Me.SubmitterId)

        '    Return
        'End If

        If idIntegration = 0 Then
            Return
        End If

        Dim send As Boolean = False
        Dim request As Boolean = False

        Select Case e.CommandName
            Case "Save"
                send = False
                request = True
            Case "Send"
                send = True
                request = True
            Case "AnsSave"
                send = False
                request = False
            Case "AnsSend"
                send = True
                request = False
        End Select


        If Not request Then

            Dim CTRLfileUploader As UC_CompactInternalFileUploader2 = e.Item.FindControl("CTRLfileUploader")
            If IsNothing(CTRLfileUploader) Then
                Return
            End If

            If CTRLfileUploader.Visible Then

                Dim moduleCode As String = ""
                Dim idModule As Integer = 0
                Dim moduleAction As Integer = 0
                Dim objectType As Integer = 0


                Dim integration As lm.Comol.Modules.CallForPapers.Advanced.Domain.AdvEvalIntegration =
                Me.CurrentPresenter.GetIntegrationForUpload(idIntegration, moduleCode, idModule, moduleAction, objectType)

                If IsNothing(integration) Then
                    Return
                End If

                Dim maLink As lm.Comol.Core.DomainModel.ModuleActionLink =
                CTRLfileUploader.UploadAndLinkInternalFile(
                lm.Comol.Core.DomainModel.FileRepositoryType.InternalLong,
                integration,
                moduleCode,
                moduleAction,
                objectType)

                Dim text As String = ""
                Dim TXBanswer As TextBox = e.Item.FindControl("TXBanswer")
                If Not IsNothing(text) Then
                    text = TXBanswer.Text
                End If

                Me.CurrentPresenter.IntegrationFileUpload(idIntegration, maLink, text, Me.CommissionId, Me.SubmissionId, Me.FieldId, Me.SubmitterId, send)

                Return

            End If
        End If

        Dim type As Adv.IntegrationType = Adv.IntegrationType.Text

        Select Case Me.RBLtype.SelectedValue
            Case "text"
                type = Adv.IntegrationType.Text
            Case "textfile"
                type = Adv.IntegrationType.TextnFile
        End Select

        If (request) Then

            Dim txbRequest As TextBox = e.Item.FindControl("TXBrequest")
            Dim RBLtypeReq As RadioButtonList = e.Item.FindControl("RBLtype")

            Select Case RBLtypeReq.SelectedValue
                Case "text"
                    type = Adv.IntegrationType.Text
                Case "textfile"
                    type = Adv.IntegrationType.TextnFile
            End Select

            Me.CurrentPresenter.SaveRequest(
                Me.CommissionId,
                txbRequest.Text,
                type,
                SubmissionId,
                Me.FieldId,
                Me.SubmitterId,
                idIntegration,
                send
        )
        Else

            Dim TXBanswer As TextBox = e.Item.FindControl("TXBanswer")

            Me.CurrentPresenter.SaveAnswer(
                idIntegration,
                TXBanswer.Text,
                send
            )

        End If

    End Sub



End Class