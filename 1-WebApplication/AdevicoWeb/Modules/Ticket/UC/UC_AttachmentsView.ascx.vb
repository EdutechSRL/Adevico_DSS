Imports lm.Comol.Core.DomainModel
Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class UC_AttachmentsView
    Inherits System.Web.UI.UserControl

    'Public Event ChangeFileVisibility(ByVal sender As Object, ByVal e As FileHideEventArgs)
    Public Event FileAction(ByVal sender As Object, ByVal e As RepeaterCommandEventArgs)
    'Public Event DeleteFile(ByVal sender As Object, ByVal e As RepeaterCommandEventArgs)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub InitUc(ByVal Files As IList(Of TK.Domain.DTO.DTO_AttachmentItem), ByVal ShowHiding As Boolean, ByVal MessageId As Int64, ByVal ShowDelete As Boolean)

        _ShowHiding = ShowHiding
        _ShowDelete = ShowDelete

        Me.MessageID = MessageId

        If Not IsNothing(Files) AndAlso Files.Any() Then
            Me.RPTattachments.DataSource = Files
            Me.RPTattachments.DataBind()
            Me.PNLfilesContainer.Visible = True
        Else
            Me.PNLfilesContainer.Visible = False
        End If


    End Sub

    Private _ShowHiding As Boolean = False
    Private _ShowDelete As Boolean = False
    Private Sub RPTattachments_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTattachments.ItemCommand

        RaiseEvent FileAction(Me, e)
        RaiseBubbleEvent(Me, e)

        'Dim FileId As Int64 = 0
        'Try
        '    FileId = System.Convert.ToInt64(e.CommandArgument)
        'Catch ex As Exception

        'End Try

        'Dim _e As New RepeaterCommandHideFileEventArgs(e.Item, source, e)
        '_e.FileId = FileId
        '_e.MessageId = MessageID
        '--------------------------------------------------
        'Dim e As New RepeaterItemEventArgs
        'Dim di As New FileHideDataItem()
        'di.FileId = FileId
        'di.MessageId = MessageID


        'Dim _e As New RepeaterCommandEventArgs

        '_e.Item.DataItem = 

        'e.CommandName = "HideFile"
        'e.CommandArgument = 

        '_e.FileId = FileId
        '_e.MessageId = MessageID

        'If (FileId > 0) Then


        'Select Case e.CommandName
        '    Case "File_Hide"

        '        '_e.ForHiding = True
        '        RaiseEvent FileAction(Me, e)
        '        RaiseBubbleEvent(Me, e)
        '        'RaiseEvent ChangeFileVisibility(source, _e)
        '    Case "File_Show"
        '        'e.ForHiding = False
        '        'RaiseBubbleEvent(Me, _e)
        '        RaiseEvent FileAction(Me, e)
        '        RaiseBubbleEvent(Me, e)
        '        'RaiseEvent ChangeFileVisibility(source, _e)
        '    Case "File_Delete"
        '        RaiseEvent FileAction(Me, e)
        '        RaiseBubbleEvent(Me, e)
        'End Select
        'End If
    End Sub
    'Dim dto As dtoAttachmentItem = e.Item.DataItem
    'Private Sub InitUC(ByRef renderFile As UC_ModuleRepositoryAction, dto As TK.Domain.DTO.DTO_AttachmentItem)
    '    'Dim renderFile As UC_ModuleRepositoryAction = e.Item.FindControl("CTRLdisplayFile")

    '    Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

    '    ' DIMENSIONI IMMAGINI
    '    initializer.IconSize = Helpers.IconSize.Small
    '    renderFile.EnableAnchor = True
    '    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions

    '    'initializer.Link = dto.Attachment.Link
    '    renderFile.InsideOtherModule = True
    '    Dim actions As List(Of dtoModuleActionControl)
    '    actions = renderFile.InitializeRemoteControl(initializer, StandardActionType.Play Or StandardActionType.EditMetadata Or StandardActionType.ViewUserStatistics)
    '    actions = renderFile.InitializeRemoteControl(initializer, StandardActionType.Play)

    '    'x prova
    '    renderFile.Visible = True


    '    'Dim oHyperlink As HyperLink

    '    'Case AttachmentType.file
    '    '    Dim renderFile As UC_ModuleRepositoryAction = e.Item.FindControl("CTRLdisplayFile")

    '    '    Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

    '    '    ' DIMENSIONI IMMAGINI
    '    '    initializer.IconSize = Helpers.IconSize.Small
    '    '    renderFile.EnableAnchor = True
    '    '    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
    '    '    initializer.Link = dto.Attachment.Link
    '    '    renderFile.InsideOtherModule = True
    '    '    Dim actions As List(Of dtoModuleActionControl)
    '    '    actions = renderFile.InitializeRemoteControl(initializer, StandardActionType.Play)
    '    '    Dim isReadyToPlay As Boolean = renderFile.IsReadyToPlay
    '    '    Dim oHyperlink As HyperLink
    '    '    If isReadyToPlay AndAlso dto.Permissions.ViewOtherStatistics AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.ViewAdvancedStatistics).Any Then
    '    '        oHyperlink = e.Item.FindControl("HYPstats")
    '    '        oHyperlink.Visible = True
    '    '        oHyperlink.ToolTip = Resource.getValue("statistic.RepositoryItemType." & renderFile.ItemType.ToString)
    '    '        oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.ViewAdvancedStatistics).Select(Function(a) a.LinkUrl).FirstOrDefault
    '    '    ElseIf isReadyToPlay AndAlso dto.Permissions.ViewMyStatistics AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.ViewUserStatistics).Any Then
    '    '        oHyperlink = e.Item.FindControl("HYPstats")
    '    '        oHyperlink.ToolTip = Resource.getValue("statistic.RepositoryItemType." & renderFile.ItemType.ToString)
    '    '        oHyperlink.Visible = True
    '    '        oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.ViewUserStatistics).Select(Function(a) a.LinkUrl).FirstOrDefault
    '    '    End If
    '    '    If isReadyToPlay AndAlso dto.Permissions.SetMetadata AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.EditMetadata).Any Then
    '    '        oHyperlink = e.Item.FindControl("HYPeditMetadata")
    '    '        oHyperlink.ToolTip = Resource.getValue("settings.RepositoryItemType." & renderFile.ItemType.ToString)
    '    '        oHyperlink.Visible = True
    '    '        oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.EditMetadata).Select(Function(a) a.LinkUrl).FirstOrDefault
    '    '    End If
    'End Sub

    Private Sub RPTattachments_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTattachments.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim attachment As TK.Domain.DTO.DTO_AttachmentItem = e.Item.DataItem
            Dim renderItem As UC_RepositoryRenderAction = e.Item.FindControl("CTRLdisplayItem")
            If Not IsNothing(renderItem) AndAlso Not IsNothing(attachment) Then
                Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
                initializer.RefreshContainerPage = False
                initializer.SaveObjectStatistics = True
                initializer.Link = attachment.Link
                initializer.SetOnModalPageByItem = True
                initializer.SetPreviousPage = False
                Dim actions As List(Of dtoModuleActionControl)
                'initializer.OnModalPage
                '  initializer.OpenLinkCssClass

                Dim requiredActions As lm.Comol.Core.ModuleLinks.DisplayActionMode = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
                renderItem.InitializeControl(initializer, StandardActionType.Play, requiredActions)



                ''Me.InitUC(renderFile, FileItem)
                'Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

                '' DIMENSIONI IMMAGINI
                'initializer.IconSize = Helpers.IconSize.Small
                'renderFile.EnableAnchor = True
                'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions

                'initializer.Link = FileItem.Link.get '.link 'dto.Attachment.Link

                'renderFile.InsideOtherModule = True
                ''Dim actions As List(Of dtoModuleActionControl)
                ''actions = renderFile.InitializeRemoteControl(initializer, StandardActionType.Play Or StandardActionType.EditMetadata Or StandardActionType.ViewUserStatistics)
                ''actions = renderFile.InitializeRemoteControl(initializer, StandardActionType.Play)

                ''x prova
                'renderFile.Visible = True

                'renderFile.InitializeControl(initializer)

                Dim LNBdelete As LinkButton = e.Item.FindControl("LNBdelete")
                If Not _ShowDelete Then
                    LNBdelete.Enabled = False
                    LNBdelete.Visible = False
                Else
                    LNBdelete.CommandName = "File_Delete"
                    LNBdelete.CommandArgument = attachment.IdAttachment.ToString()
                    LNBdelete.Visible = True
                End If

                Dim LNBshowHide As LinkButton = e.Item.FindControl("LNBshowHide")
                If Not IsNothing(LNBshowHide) Then
                    If (_ShowHiding) Then
                        'LNBshowHide.CommandName = "showhide"
                        LNBshowHide.CommandArgument = attachment.IdAttachment.ToString()
                        LNBshowHide.Visible = True

                        If attachment.Visibility = TK.Domain.Enums.FileVisibility.visible Then
                            LNBshowHide.CssClass &= " visible"
                            LNBshowHide.CommandName = "File_Hide"
                        Else
                            LNBshowHide.CssClass &= " hidden"
                            LNBshowHide.CommandName = "File_Show"
                            LNBshowHide.Enabled = (attachment.Visibility = TK.Domain.Enums.FileVisibility.hidden)
                        End If
                    Else
                        LNBshowHide.Visible = False
                        LNBshowHide.Enabled = False
                    End If
                End If
            End If
        End If
    End Sub


    Private Property MessageID As Int64
        Get

            Dim MsgId As Int64 = 0
            Try
                MsgId = System.Convert.ToInt64(Me.ViewState("_MessageId"))
            Catch ex As Exception

            End Try

            Return MsgId
        End Get
        Set(value As Int64)
            Me.ViewState("_MessageId") = value
        End Set
    End Property


End Class

'Public Class RepeaterCommandHideFileEventArgs
'    Inherits RepeaterCommandEventArgs

'    Public Sub New( _
'                  ByVal item As System.Web.UI.WebControls.RepeaterItem, _
'                  ByVal CommanSource As Object, _
'                  ByVal originalArgs As System.Web.UI.WebControls.CommandEventArgs)
'        MyBase.New(item, CommanSource, originalArgs)
'    End Sub

'    Public FileId As Int64
'    Public MessageId As Int64
'    Public ForHiding As Boolean

'End Class

'Public Class FileHideDataItem
'    Inherits System.EventArgs

'    Public FileId As Int64
'    Public MessageId As Int64
'    Public ForHiding As Boolean

'End Class
