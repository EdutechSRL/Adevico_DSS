Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class UC_AssignerSelector
    Inherits BaseControl

#Region "Internal"
    Private HasElements As Boolean = False

#End Region
#Region "Override"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region


    Public Event UserSelected(ByVal UserId As Int64)
    Public Event CloseWindows()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub BindUser(ByVal Users As IList(Of TK.Domain.DTO.DTO_User))
        If Not IsNothing(Users) AndAlso Users.Any Then
            HasElements = True
        End If
        Me.RPTtkUsers.DataSource = Users
        Me.RPTtkUsers.DataBind()
    End Sub

#Region "Override"

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("uc_AssignerSelector", "Modules", "Ticket")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        'LKBselect
        With Me.Resource
            .setLinkButton(LNBcloseAssignersWindow, True, True)
        End With

    End Sub

#End Region

    Private Sub RPTtkUsers_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTtkUsers.ItemCommand
        Dim id As Int64 = -1
        Try
            id = System.Convert.ToInt64(e.CommandArgument)
        Catch ex As Exception

        End Try

        RaiseEvent UserSelected(id)
    End Sub

    Private Sub RPTtkUsers_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTtkUsers.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            SetLabel("LBsname_t", e)
            SetLabel("LBname_t", e)
            SetLabel("LBmail_t", e)

        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim User As TK.Domain.DTO.DTO_User = e.Item.DataItem

            If Not IsNothing(User) Then

                Dim LKBselect As LinkButton = e.Item.FindControl("LNBselect")
                If Not IsNothing(LKBselect) Then
                    Resource.setLinkButton(LKBselect, True, True, False, True)
                    LKBselect.CommandName = "Assign"
                    LKBselect.CommandArgument = User.UserId.ToString()
                End If

                Dim LITsname As Literal = e.Item.FindControl("LTsname")
                If Not IsNothing(LITsname) Then
                    LITsname.Text = User.SName
                End If

                Dim LITname As Literal = e.Item.FindControl("LTname")
                If Not IsNothing(LITname) Then
                    LITname.Text = User.Name
                End If

                Dim LITmail As Literal = e.Item.FindControl("LTmail")
                If Not IsNothing(LITmail) Then
                    LITmail.Text = User.Mail
                End If
            End If
        ElseIf e.Item.ItemType = ListItemType.Footer Then

            Dim PLHfooterVoid As PlaceHolder = e.Item.FindControl("PLHfooterVoid")
            If Not IsNothing(PLHfooterVoid) Then
                If (HasElements) Then
                    PLHfooterVoid.Visible = False
                Else
                    PLHfooterVoid.Visible = True
                    Dim LTempty As Literal = e.Item.FindControl("LTempty")
                    If Not IsNothing(LTempty) Then
                        Resource.setLiteral(LTempty)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub SetLabel(Label As String, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim lbl As Label = e.Item.FindControl(Label)
        If Not IsNothing(lbl) Then
            Me.Resource.setLabel(lbl)
        End If
    End Sub

    Private Sub LNBcloseAssignersWindow_Click(sender As Object, e As EventArgs) Handles LNBcloseAssignersWindow.Click
        RaiseEvent CloseWindows()
    End Sub
End Class