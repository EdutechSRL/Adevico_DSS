Imports lm.Comol.Modules.Standard.Skin

Public Class UcOrganizationSelector
    Inherits BaseControl

#Region "Inherits"
    Public Event SelectedOrgnChanged(ByVal OrgId As Integer, ByVal OrgnName As String)

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
        End Get
    End Property
#End Region

#Region "Internal"
    Dim _CurOrgnId As Integer = -1
    Public Property CurrentOrganizationID As Integer
        Get
            Dim CurId As Integer = 0
            Try
                CurId = System.Convert.ToInt32(Me.ViewState("CurOrgnId"))
            Catch ex As Exception

            End Try
            Return CurId
        End Get
        Private Set(value As Integer)
            _CurOrgnId = value
            Me.ViewState("CurOrgnId") = value

        End Set
    End Property
    Public Property CurrentOrganizationName As String
        Get
            Return Me.ViewState("CurOrgnName")
        End Get
        Private Set(ByVal value As String)
            Me.ViewState("CurOrgnName") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
    End Sub
#End Region

#Region "Internal"
    Public Sub BindOrganizations(ByVal SelOrgn As Integer)

        If (SelOrgn > 0) Then
            Me.CurrentOrganizationID = SelOrgn
        End If

        Dim ServiceSkin As New Business.ServiceSkin(Me.PageUtility.CurrentContext)

        Me.Rpt_OrgnMod.DataSource = ServiceSkin.GetOrganizationList(0) 'Organization
        Me.Rpt_OrgnMod.DataBind()
    End Sub
    Private Sub Rpt_OrgnMod_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles Rpt_OrgnMod.ItemCommand
        Dim OrgnID As Integer

        Try
            OrgnID = e.CommandArgument
        Catch ex As Exception
            OrgnID = 0
        End Try

        Me.CurrentOrganizationID = OrgnID

        Dim LKBselOrgn As LinkButton = e.Item.FindControl("LKBselOrgn")
        Dim OrgnName As String = ""

        If Not IsNothing(LKBselOrgn) Then
            OrgnName = LKBselOrgn.Text
        End If

        Me.CurrentOrganizationName = OrgnName
        RaiseEvent SelectedOrgnChanged(OrgnID, OrgnName)
    End Sub
#End Region

#Region "Handler"
    Private Sub Rpt_OrgnMod_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles Rpt_OrgnMod.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim DtoOrgn As Domain.DTO.DtoSkinOrganization = e.Item.DataItem

            Dim LKBselOrgn As LinkButton = e.Item.FindControl("LKBselOrgn")

            If (Not IsNothing(LKBselOrgn)) Then
                If DtoOrgn.Id = _CurOrgnId Then
                    LKBselOrgn.Enabled = False
                End If

                LKBselOrgn.CommandArgument = DtoOrgn.Id
                LKBselOrgn.Text = DtoOrgn.Name

            End If
        End If
    End Sub
#End Region
End Class