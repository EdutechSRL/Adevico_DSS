Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Standard.Skin

Public Class SkinManagement
    Inherits PageBase

#Region "Context"
    Private _service As Business.ServiceSkin

    Private ReadOnly Property service As Business.ServiceSkin
        Get
            If IsNothing(_service) Then
                Me._service = New Business.ServiceSkin(MyBase.PageUtility.CurrentContext)
            End If
            Return _service
        End Get
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.Show(SkinManagement.View.List)
            'Me.RadCP_BgColor.Preset = Telerik.Web.UI.ColorPreset.Web216
        End If

        Me.SetInternazionalizzazione()

        If Not Page.IsPostBack() Then
            Me.CTRLskinList.RefreshList()
        End If

        'Me.Master.AddToolTip()

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.MLVskins.Visible = False
        Me.LNBaddSkin.Visible = False
        Me.LNBbackToList.Visible = False
        Me.Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Dim SkinSettings As SkinSettings = MyBase.SystemSettings.SkinSettings

        Dim HasPerm As Boolean = False
        If Not IsNothing(SessionHelpers.CurrentUserContext) Then
            Try
                If (SkinSettings.PersonTypeIds.Contains(SessionHelpers.CurrentUserContext.UserTypeID) OrElse _
                SkinSettings.PersonsIds.Contains(SessionHelpers.CurrentUserContext.CurrentUserID)) Then
                    HasPerm = True
                End If
            Catch ex As Exception
            End Try
        End If
        Return HasPerm
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ManagementSkin", "Skin")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        If IsNothing(MyBase.Resource) Then
            Me.SetCultureSettings()
        End If

        With MyBase.Resource
            .setLinkButton(LNBaddSkin, True, True)
            .setLinkButton(LNBbackToList, True, True)

            .setLabel(LBnewSkinName_t)

            .setLinkButton(LNBcreateNew, True, True)

            Me.Master.ServiceTitle = .getValue("ServiceTitle.text")

            .setLiteral(Me.LITnoteTitle_t)
            .setLiteral(Me.LITnoteText_t)
        End With

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Navigazione"
    Private Enum View As Integer
        List = 0
        NewSkin = 1
        Edit = 2
    End Enum

    Private Sub Show(ByVal view As View)
        LNBaddSkin.Visible = (view = SkinManagement.View.List)
        LNBbackToList.Visible = (view <> SkinManagement.View.List)
        Select Case View
            Case SkinManagement.View.Edit
                Me.MLVskins.SetActiveView(Me.VIWedit)
            Case SkinManagement.View.List
                Me.MLVskins.SetActiveView(Me.VIWlist)
                Me.CTRLskinList.RefreshList()
            Case SkinManagement.View.NewSkin
                Me.MLVskins.SetActiveView(Me.VIWadd)
        End Select
    End Sub
#End Region

#Region "Command"
    Private Sub CTRLskinList_EditSkin(ByVal SkinId As Long) Handles CTRLskinList.EditSkin
        Me.CTRLedit.BindData(SkinId)
        Me.Show(SkinManagement.View.Edit)
    End Sub

    Private Sub LNBaddSkin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBaddSkin.Click
        Me.Show(SkinManagement.View.NewSkin)
    End Sub

    Private Sub LNBbackToList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBbackToList.Click
        Me.Show(SkinManagement.View.List)
    End Sub

    Private Sub LNBcreateNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBcreateNew.Click
        If (Me.TXBnewSkinName.Text <> "") Then
            Dim NewId As Int64
            NewId = Me.service.AddNew(Me.TXBnewSkinName.Text, MyBase.SystemSettings.SkinSettings.SkinPhisicalPath)

            Me.CTRLedit.BindData(NewId)
            Me.Show(View.Edit)
        Else
            'Manca il nome!
        End If
    End Sub

#End Region

#Region "Background color"
    Private Sub RadCP_BgColor_ColorChanged(sender As Object, e As System.EventArgs) Handles RDPbackgorundColor.ColorChanged
        Me.PNLbgColor.BackColor = System.Drawing.Color.FromArgb(RDPbackgorundColor.SelectedColor.ToArgb())
    End Sub
#End Region
End Class