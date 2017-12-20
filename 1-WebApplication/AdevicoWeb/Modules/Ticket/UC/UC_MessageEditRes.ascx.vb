Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class UC_MessageEditRes
    Inherits BaseControl

    ''' <summary>
    ''' Invio messaggio
    ''' </summary>
    ''' <param name="Message"></param>
    ''' <param name="Status">Nuovo status del messaggio. NOTA: se DRAF errore di conversione: non modificare status ticket!</param>
    ''' <param name="ShowToUser"></param>
    ''' <remarks></remarks>
    Public Event SendMessage( _
                           ByVal Status As TK.Domain.Enums.TicketStatus, _
                           ByVal HideToUser As Boolean)

    Public Event FileDelete(ByVal FileId As Int64)

    Public Event TicketChangeCondition(ByVal Condition As TK.Domain.Enums.TicketCondition, ByVal Active As Boolean)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Only for test





    End Sub


    Public Sub InitUC( _
                   ByVal UserName As String, _
                   ByVal Role As String, _
                   ByVal Status As TK.Domain.Enums.TicketStatus, _
                    ByVal Resource As ResourceManager, _
                    ByVal actions As List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions), _
                   Optional ByVal CanShowToUser As Boolean = True)

        LTuserName.Text = UserName
        LTuserRole.Text = Role

        SetLocalization(Resource)

        Dim LstTkStatus As Array = [Enum].GetValues(GetType(TK.Domain.Enums.TicketStatus))

        For i As Integer = 0 To LstTkStatus.Length - 1
            If LstTkStatus(i) <> TK.Domain.Enums.TicketStatus.draft Then
                Dim LI As New ListItem( _
                      Resource.getValue("Status." & LstTkStatus(i).ToString()), _
                        LstTkStatus(i).ToString())
                If LstTkStatus(i) = Status Then
                    LI.Selected = True
                End If
                Me.DDLucStatus.Items.Add(LI)
            End If
        Next

        If (CanShowToUser) Then
            CBXucShowToUser.Checked = False
            CBXucShowToUser.Enabled = True
        Else
            CBXucShowToUser.Checked = True
            CBXucShowToUser.Enabled = False
        End If

        'Passato da Business all'inizializzazione
        CTRLcommands.InitializeControlForJQuery(actions, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem)
    End Sub




    ''' <summary>
    ''' Internazionalizza i controlli.
    ''' </summary>
    ''' <param name="Resource"></param>
    ''' <remarks>
    ''' LA PAGINA che lo richiama dovrà provvedere ad avere nel proprio XML i vari elementi qui indicati.
    ''' E' stato fatto così per non esporre i controlli interni e per evitare di avere ulteriori xml
    ''' e l'implementazione del BaseControl, visto che tale controllo serve solo per il riposizionamento degli elementi ivi contenuti.
    ''' </remarks>
    Private Sub SetLocalization(ByVal Resource As ResourceManager)
        If Not IsNothing(Resource) Then
            With Resource
                .setLabel(LBucMessage_t)
                .setLabel(LBucStatus_t)
                .setLinkButton(LNBucSubmit, True, True)
                .setCheckBox(CBXucShowToUser)
                'DDL STATUS

                Me.DDLucStatus.Items.Clear()

                'Dim AllItm As New ListItem()
                'AllItm.Value = "all"
                'AllItm.Text = Resource.getValue("Status.All")
                'AllItm.Selected = True
                'Me.DDLstatus.Items.Add(AllItm)

                .setLiteral(LTmoderationTitle_t)
                .setLinkButton(LNBreport, True, True)
                .setLinkButton(LNBunreport, True, True)
                .setLinkButton(LNBblock, True, True, False, True)
                .setLinkButton(LNBunblock, True, True, False)
                .setLinkButton(LNBdelete, True, True, False, True)



            End With
        End If
    End Sub


    Private Sub LNBsubmit_Click(sender As Object, e As System.EventArgs) Handles LNBucSubmit.Click
        Dim status As TK.Domain.Enums.TicketStatus = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketStatus.draft

        Try
            status = DirectCast([Enum].Parse(GetType(TK.Domain.Enums.TicketStatus), Me.DDLucStatus.SelectedValue), TK.Domain.Enums.TicketStatus)
        Catch ex As Exception

        End Try

        ''CTRLmessagesInfo.Visible = False
        RaiseEvent SendMessage(status, Me.CBXucShowToUser.Checked)

    End Sub

    Public Sub ClearMessage()
        Me.CTRLEditorText.HTML = ""
        CTRLmessagesInfo.Visible = False
    End Sub

    Public Sub SetDraft( _
                       ByVal HTML As String, _
                       ByVal DraftId As Int64, _
                       ByVal Attachments As IList(Of TK.Domain.DTO.DTO_AttachmentItem),
                       ByVal Condition As TK.Domain.Enums.TicketCondition)

        Me.CTRLEditorText.HTML = HTML
        DraftMessageId = DraftId

        If Not IsNothing(Attachments) OrElse Attachments.Any Then
            LTattachmentDiv.Text = LTattachmentDiv.Text.Replace(" empty", "")

            CTRLattView.InitUc(Attachments, False, DraftId, True)
        End If
        
        Me.LNBreport.Visible = True
        Me.LNBblock.Visible = True

        Me.LNBunreport.Visible = False
        Me.LNBunblock.Visible = False

        Me.LNBdelete.Visible = False

        Select Case Condition
            Case TK.Domain.Enums.TicketCondition.active
                'Me.LNBreport.Visible = True
                'Me.LNBblock.Visible = True

            Case TK.Domain.Enums.TicketCondition.flagged
                Me.LNBreport.Visible = false
                Me.LNBunreport.Visible = True

            Case TK.Domain.Enums.TicketCondition.blocked
                Me.LNBblock.Visible = False
                Me.LNBunblock.Visible = True


            Case TK.Domain.Enums.TicketCondition.flaggedNblocked
                Me.LNBreport.Visible = False
                Me.LNBblock.Visible = False

                Me.LNBunreport.Visible = True
                Me.LNBunblock.Visible = True

            Case TK.Domain.Enums.TicketCondition.cancelled
                'Not defined

        End Select

    End Sub

    Private Property DraftMessageId As Int64
        Get
            Try
                Return System.Convert.ToInt64(ViewState("MsgDraftId"))
            Catch ex As Exception
                Return -1
            End Try
        End Get
        Set(ByVal value As Int64)
            ViewState("MsgDraftId") = value
        End Set
    End Property

    Public ReadOnly Property HtmlMessage As String
        Get
            Return Me.CTRLEditorText.HTML
        End Get
    End Property

    Public ReadOnly Property PreviewMessage As String
        Get
            Return Me.CTRLEditorText.Text
        End Get
    End Property


    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property

    Public Sub ShowMessage(ByVal Message As String, ByVal Type As lm.Comol.Core.DomainModel.Helpers.MessageType)

        If String.IsNullOrEmpty(Message) OrElse Type = lm.Comol.Core.DomainModel.Helpers.MessageType.none Then

            'DVmessageHeader.Attributes.Add("class", Me.LTmessageHeaderCSS.Text)
            CTRLmessagesInfo.Visible = False

        Else

            'DVmessageHeader.Attributes.Add("class", Me.LTmessageHeaderCSS.Text.Replace(" empty", ""))
            CTRLmessagesInfo.Visible = True
            CTRLmessagesInfo.InitializeControl(Message, Type)

        End If


    End Sub

    Public Property AnchorVisibility As Boolean
        Get
            Return LTanchor.Visible
        End Get
        Set(value As Boolean)
            LTanchor.Visible = value
        End Set
    End Property


    Private Sub CTRLattView_FileAction(sender As Object, e As RepeaterCommandEventArgs) Handles CTRLattView.FileAction
        
        If e.CommandName = "File_Delete" Then
            Dim Id As Int64 = 0

            Try
                Id = System.Convert.ToInt64(e.CommandArgument)
            Catch ex As Exception

            End Try

            If Id > 0 Then
                RaiseEvent FileDelete(Id)
            End If
        End If

    End Sub

    Private Sub LNBblock_Click(sender As Object, e As EventArgs) Handles LNBblock.Click
        RaiseEvent TicketChangeCondition(TK.Domain.Enums.TicketCondition.blocked, True)
    End Sub
    Private Sub LNBunblock_Click(sender As Object, e As EventArgs) Handles LNBunblock.Click
        RaiseEvent TicketChangeCondition(TK.Domain.Enums.TicketCondition.blocked, False)
    End Sub

    Private Sub LNBreport_Click(sender As Object, e As EventArgs) Handles LNBreport.Click
        RaiseEvent TicketChangeCondition(TK.Domain.Enums.TicketCondition.flagged, True)
    End Sub

    Private Sub LNBunreport_Click(sender As Object, e As EventArgs) Handles LNBunreport.Click
        RaiseEvent TicketChangeCondition(TK.Domain.Enums.TicketCondition.flagged, False)
    End Sub


    Private Sub LNBdelete_Click(sender As Object, e As EventArgs) Handles LNBdelete.Click

        'Non previsto.
        'RaiseEvent TicketChangeCondition(TK.Domain.Enums.TicketCondition.cancelled)

    End Sub
End Class