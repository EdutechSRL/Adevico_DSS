Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.Base.Presentation

Partial Public Class WorkBookError
    Inherits PageBase
    Implements IviewWorkBookItemErrors


#Region "View "
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
    Public ReadOnly Property CommunitiesPermission() As System.Collections.Generic.IList(Of lm.Comol.Modules.Base.Presentation.WorkBookCommunityPermission) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookItemErrors.CommunitiesPermission
        Get

        End Get
    End Property

    Public ReadOnly Property ItemCommunityFiles() As List(Of String) Implements IviewWorkBookItemErrors.ItemCommunityFiles
        Get
            Dim Names As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_Name_") Select Me.Request.Form(v)).ToList
            If IsNothing(Names) Then
                Names = New List(Of String)
            End If

            Return Names
        End Get
    End Property

    Public ReadOnly Property ItemInternalFiles() As List(Of String) Implements IviewWorkBookItemErrors.ItemInternalFiles
        Get
            Dim Names As List(Of String) = (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_I_") Select Me.Request.Form(v)).ToList
            If IsNothing(Names) Then
                Names = New List(Of String)
            End If

            Return Names
        End Get
    End Property

    Public ReadOnly Property PreloadedItemID() As System.Guid Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookItemErrors.PreloadedItemID
        Get
            Dim UrlID As String = Request.QueryString("ItemID")
            If Not UrlID = "" Then
                Try
                    Return New System.Guid(UrlID)
                Catch ex As Exception

                End Try
            End If
            Return System.Guid.Empty
        End Get
    End Property
    Public ReadOnly Property PreviousWorkBookView() As lm.Comol.Modules.Base.DomainModel.WorkBookTypeFilter
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return lm.Comol.Modules.Base.DomainModel.WorkBookTypeFilter.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.Base.DomainModel.WorkBookTypeFilter).GetByString(Request.QueryString("View"), lm.Comol.Modules.Base.DomainModel.WorkBookTypeFilter.None)
            End If
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'oRemotePost.Add("FILE_I_" & Index.ToString, oFile.DisplayName)
        'oRemotePost.Add("FILE_C_" & Index.ToString, oFile.Nome)
        ' oRemotePost.Add("FromView", "upload")
        ' oRemotePost.Add("ItemID", "Me.PreloadedItemID.ToString")

        'For Each oKey As String In Me.Request.Form.AllKeys
        '    Me.Response.Write("<br><br>" & oKey & " = " & Me.Request.Form(oKey))
        'Next
    End Sub

#Region "Inherits "
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            If ItemCommunityFiles.Count > 0 Then
                Me.LBfileError.Text &= Me.Resource.getValue("communityfile.error")
                Me.LBfileError.Text &= "<ul>"
                For Each oName As String In ItemCommunityFiles
                    Dim FileName As String = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(GetFileExtension(oName)) & "'>&nbsp;" & oName
                    Me.LBfileError.Text &= "<li>" & FileName & "</li>"
                Next
                Me.LBfileError.Text &= "</ul>"
            End If

            If ItemInternalFiles.Count > 0 Then
                Me.LBfileError.Text &= Me.Resource.getValue("internalfile.error")
                Me.LBfileError.Text &= "<ul>"
                For Each oName As String In ItemCommunityFiles
                    Dim FileName As String = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(GetFileExtension(oName)) & "'>&nbsp;" & oName
                    Me.LBfileError.Text &= "<li>" & FileName & "</li>"
                Next
                Me.LBfileError.Text &= "</ul>"
            End If
            Me.SetBackToFileManagement(Me.PreloadedItemID)
            'Me.SetBackToWorkbookItem(Me.PreloadedItemID)
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddAction(Services_WorkBook.ActionType.NoPermission, Nothing, InteractionType.Generic)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("WorkBookItemManagementFile", "Generici")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitleItemFileError")
            Me.Master.ServiceNopermission = .getValue("nopermissionItemFileError")
            .setHyperLink(Me.HYPbackToFileManagement, True, True)
            .setHyperLink(Me.HYPbackToItem, True, True)
            .setHyperLink(Me.HYPbackToItems, True, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Private Function GetFileExtension(ByVal FileName As String) As String
        Return Right(FileName, Len(FileName) - InStrRev(FileName, ".") + 1)
    End Function

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_WorkBook.Codex)
    End Sub

    Public Sub SetBackToFileManagement(ByVal ItemID As System.Guid) Implements IviewWorkBookItemErrors.SetBackToFileManagement
        Me.HYPbackToFileManagement.Visible = Not (ItemID = System.Guid.Empty)
        Me.HYPbackToFileManagement.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemManagementFile.aspx?ItemID=" & ItemID.ToString & "&View=" & PreviousWorkBookView.ToString
    End Sub

    Public Sub SetBackToWorkbook(ByVal WorkBookID As System.Guid, ByVal ItemID As System.Guid) Implements IviewWorkBookItemErrors.SetBackToWorkbook
        Me.HYPbackToItems.Visible = Not (WorkBookID = System.Guid.Empty)
        Me.HYPbackToItems.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemsList.aspx?WorkBookID=" & WorkBookID.ToString & "&View=" & PreviousWorkBookView.ToString
    End Sub

    Public Sub SetBackToWorkbookItem(ByVal ItemID As System.Guid) Implements IviewWorkBookItemErrors.SetBackToWorkbookItem
        Me.HYPbackToItem.Visible = Not (ItemID = System.Guid.Empty)
        Me.HYPbackToItem.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItem.aspx?ItemID=" & ItemID.ToString & "&View=" & PreviousWorkBookView.ToString
    End Sub
End Class