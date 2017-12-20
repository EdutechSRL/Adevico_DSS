

Imports lm.Comol.Modules.EduPath.Domain

Public Class UC_HelpEpRole
    Inherits BaseControlSession


    Private _viewEvaluator As Boolean

#Region "Base"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub



    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_HelpEpRole", "EduPath")
        SetInternazionalizzazione()
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBevaluatorTitle)
            .setLabel(LBmanagerTitle)
            .setLabel(LBpartecipantTitle)
            .setLabel(LBstatviewerTitle)
            .setLabel(LbPermissionTitle)
        End With
    End Sub
#End Region

    Public Sub InitDialog(ByVal viewEvaluator As Boolean)
        _viewEvaluator = viewEvaluator
        Dim permission As IList(Of String) = [Enum].GetNames(GetType(PermissionEP_Enum))
        permission = (From a In permission
                      Where Not String.Equals(a, PermissionEP_Enum.None.ToString) AndAlso Not String.Equals(a, PermissionEP_Enum.Participant.ToString) AndAlso Not String.Equals(a, PermissionEP_Enum.Evaluator.ToString) AndAlso Not String.Equals(a, PermissionEP_Enum.Manager.ToString) AndAlso Not String.Equals(a, PermissionEP_Enum.StatViewer.ToString)
                      Select a).ToList
        Me.THeval.Visible = viewEvaluator

        RPpermission.DataSource = permission
        RPpermission.DataBind()
    End Sub


    Private Sub RPpermission_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPpermission.ItemDataBound
        Dim permissionName As String = e.Item.DataItem
        Dim permission As PermissionEP_Enum = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.EduPath.Domain.PermissionEP_Enum).GetByString(permissionName, PermissionEP_Enum.None)
        Dim oLb As Label
        Dim oImg As System.Web.UI.WebControls.Image

        oLb = e.Item.FindControl("LBpermission")
        oLb.Text = Me.Resource.getValue("PermissionEP_Enum." & permissionName)

        If CheckPermission(PermissionEP_Enum.Participant, permission) Then
            oImg = e.Item.FindControl("IMGpartecipant")
            oImg.ImageUrl = lm.Comol.Modules.EduPath.BusinessLogic.RootObject.ImgGreen(Me.BaseUrl)
            oImg.Visible = True
        End If

        If CheckPermission(PermissionEP_Enum.StatViewer, permission) Then
            oImg = e.Item.FindControl("IMGstatviewer")
            oImg.ImageUrl = lm.Comol.Modules.EduPath.BusinessLogic.RootObject.ImgGreen(Me.BaseUrl)
            oImg.Visible = True
        End If

        If _viewEvaluator AndAlso CheckPermission(PermissionEP_Enum.Evaluator, permission) Then
            oImg = e.Item.FindControl("IMGevaluator")
            oImg.ImageUrl = lm.Comol.Modules.EduPath.BusinessLogic.RootObject.ImgGreen(Me.BaseUrl)
            oImg.Visible = True
        ElseIf Not _viewEvaluator Then
            Dim td As HtmlControl = e.Item.FindControl("TDeval")
            td.Visible = False
        End If
        If CheckPermission(PermissionEP_Enum.Manager, permission) Then
            oImg = e.Item.FindControl("IMGmanager")
            oImg.ImageUrl = lm.Comol.Modules.EduPath.BusinessLogic.RootObject.ImgGreen(Me.BaseUrl)
            oImg.Visible = True
        End If

    End Sub

  


    Private Function CheckPermission(ByVal actual As PermissionEP_Enum, ByVal expected As PermissionEP_Enum)
        Return (actual And expected) = expected
    End Function


End Class