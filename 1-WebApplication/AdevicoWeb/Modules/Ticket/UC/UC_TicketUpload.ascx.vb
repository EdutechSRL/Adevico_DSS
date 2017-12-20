Imports TK = lm.Comol.Core.BaseModules.Tickets
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class UC_TicketUpload
    Inherits BaseControl

#Region "Internal"
    Public Property AllowInternalAnonymousUpload As Boolean
        Get
            Return CTRLinternalUploader.AllowAnonymousUpload
        End Get
        Set(value As Boolean)
            CTRLinternalUploader.AllowAnonymousUpload = value
        End Set
    End Property
    Public Property InternalUploadAsUser As Integer
        Get
            Return CTRLinternalUploader.UploadAsUser
        End Get
        Set(value As Integer)
            CTRLinternalUploader.UploadAsUser = value
        End Set
    End Property
#End Region
    Public Event UploadFile()
    Public Event UploadFileToCommunity()
    Public Event LinkFromCommunity()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub

    Public Sub InitializeUploaderControl(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, Optional idCommunity As Integer = -1, Optional anonymousUpload As Boolean = False, Optional uploadAsUser As Integer = 0) 'Implements IViewAddAttachment.InitializeUploaderControl

        'Inizializzazione uploader
        CTRLinternalUploader.Visible = True
        CTRLrepositoryUploader.Visible = False
        CTRLinternalUploader.AllowAnonymousUpload = anonymousUpload
        CTRLinternalUploader.UploadAsUser = uploadAsUser

        CTRLinternalUploader.InitializeControl(0)
        BTNaddAttachment.OnClientClick = "ProgressStart();"

    End Sub

    Public Sub InitializeCommunityUploader(rPermissions As lm.Comol.Core.DomainModel.CoreModuleRepository, idCommunity As Integer)

        Me.CTRLinternalUploader.Visible = False
        CTRLrepositoryUploader.Visible = True

        CTRLrepositoryUploader.InitializeControl(0, idCommunity, rPermissions)
        BTNaddAttachment.OnClientClick = "ProgressStart();"
    End Sub

    Public Sub InitializeLinkRepositoryItems(rPermissions As lm.Comol.Core.DomainModel.CoreModuleRepository, idCommunity As Integer, alreadyLinkedFiles As List(Of iCoreItemFileLink(Of Long)))
        CTRLlinkItems.Visible = True
        CTRLlinkItems.InitializeControl(idCommunity, alreadyLinkedFiles, False, False, rPermissions.Administration, rPermissions.Administration)
    End Sub
    'Private Sub BaseInitializeControl(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, Optional description As String = "", Optional dialogclass As String = "")
    '    BTNaddAttachment.OnClientClick = ""
    '    If Not String.IsNullOrEmpty(dialogclass) Then
    '        UploaderCssClass = dialogclass
    '    End If
    '    If String.IsNullOrEmpty(description) Then
    '        DVdescription.Visible = False
    '    Else
    '        LTdescription.Text = description
    '    End If
    'End Sub


    Public Function UploadFiles(Message As TK.Domain.Message) As MultipleUploadResult(Of dtoModuleUploadedFile)

        Return Me.CTRLinternalUploader.AddModuleInternalFiles( _
            FileRepositoryType.InternalLong, _
            Message, _
            TK.ModuleTicket.UniqueCode, _
            TK.ModuleTicket.ActionType.MessageAttachDownload, _
            TK.ModuleTicket.ObjectType.Message)

    End Function
    
    
#Region "Internal"

    Public Property UploaderCssClass As String
        Get
            Return ViewStateOrDefault("UploaderCssClass", "")
        End Get
        Set(value As String)
            ViewState("UploaderCssClass") = value
        End Set
    End Property

#End Region

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditResolver", "Modules", "Ticket")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setButton(BTNaddAttachment, True, False, False, True)
            .setLinkButton(LNBcloseAttachmentWindow, True, True)
            .setLiteral(LTdescription)

        End With
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property


   

    Private Sub BTNaddAttachment_Click(sender As Object, e As EventArgs) Handles BTNaddAttachment.Click
        If (CTRLinternalUploader.Visible) Then
            RaiseEvent UploadFile()
        ElseIf CTRLrepositoryUploader.Visible Then
            RaiseEvent UploadFileToCommunity()
        ElseIf CTRLlinkItems.Visible Then
            RaiseEvent LinkFromCommunity()
        End If
        'Select case AttachmentType (se file item, se file item+com, se DA com o se link
        'CurrentPresenter.AddFilesToItem(IdProject, IdActivity)

    End Sub

    Public Function UploadedToCommunity() As MultipleUploadResult(Of dtoUploadedFile)
        Return Me.CTRLrepositoryUploader.AddFilesToCommunityRepository()
    End Function

    Public Function GetLinktoCommunityItems() As List(Of ModuleActionLink)
        Return CTRLlinkItems.GetNewRepositoryItemLinks()
    End Function

End Class