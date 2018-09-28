Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Core.DomainModel
Public Class UC_DialogProjectAttachments
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Private _CssClass As String
    Public Property CssClass As String
        Get
            Return _CssClass
        End Get
        Set(value As String)
            If Not String.IsNullOrWhiteSpace(value) Then
                _CssClass = " " & value
            Else
                _CssClass = ""
            End If
        End Set
    End Property

    Public ReadOnly Property DefaultCssClass As String
        Get
            Return LTdefaultCssClass.Text
        End Get
    End Property
    Private _DialogIdentifier As String
    Public Property DialogIdentifier As String
        Get
            Return _DialogIdentifier
        End Get
        Set(value As String)
            _DialogIdentifier = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(items As List(Of lm.Comol.Modules.Standard.ProjectManagement.Domain.dtoAttachmentItem))
        RPTattachments.Visible = Not IsNothing(items) AndAlso items.Any()
        If Not IsNothing(items) Then
            RPTattachments.DataSource = items
            RPTattachments.DataBind()
        End If
    End Sub
    Public Function GetDialogTitle() As String
        Return Resource.getValue("DialogTitle.ProjectAttachments")
    End Function

    Private Sub RPTattachments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTattachments.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.AlternatingItem, ListItemType.Item
                Dim dto As lm.Comol.Modules.Standard.ProjectManagement.Domain.dtoAttachmentItem = e.Item.DataItem

                Select Case dto.Attachment.Type
                    Case AttachmentType.url
                        Dim renderUrl As UC_DisplayUrlItem = e.Item.FindControl("CTRLdisplayUrl")
                        renderUrl.IconSize = Helpers.IconSize.Small
                        renderUrl.InitializeControl(lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction, dto.Attachment.Url, dto.Attachment.DisplayName)
                        renderUrl.Visible = True
                    Case AttachmentType.file
                        Dim renderItem As UC_RepositoryRenderAction = e.Item.FindControl("CTRLdisplayItem")
                        Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
                        initializer.RefreshContainerPage = False
                        initializer.SaveObjectStatistics = True
                        initializer.Link = dto.Attachment.Link
                        initializer.SetOnModalPageByItem = True
                        initializer.SetPreviousPage = False

                        renderItem.InitializeRemoteControl(initializer, StandardActionType.Play, lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction)
                        renderItem.Visible = True
                End Select
        End Select
    End Sub
#End Region
End Class