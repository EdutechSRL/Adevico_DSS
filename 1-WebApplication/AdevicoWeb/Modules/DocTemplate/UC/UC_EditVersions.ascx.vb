Imports lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management

Public Class UC_EditVersions
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event RecoverItem(ByVal Id As Int64)
    Public Event DeleteItem(ByVal Id As Int64)
    Public Event DeleteItems(ByVal Ids As IList(Of Int64))
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templateEdit", "Modules", "DocTemplates")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        Me.Resource.setLinkButton(LKBdeleteSel, True, True, False, False)
    End Sub
#End Region

#Region "Internal"
    Public Sub BindList(ByVal Versions As IList(Of DTO_EditPreviousVersion))
        If Not IsNothing(Versions) AndAlso Versions.Count() > 0 Then
            Me.RPTsubVersion.Visible = True
            Me.RPTsubVersion.DataSource = Versions
            Me.RPTsubVersion.DataBind()
        Else
            Me.RPTsubVersion.Visible = False
        End If
    End Sub
#End Region

#Region "Handler"
    Private Sub RPTsubVersion_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTsubVersion.ItemCommand

        Dim ID As Int64 = System.Convert.ToInt64(e.CommandArgument)

        Select Case e.CommandName
            Case "Delete"
                RaiseEvent DeleteItem(ID)
            Case "Recover"
                RaiseEvent RecoverItem(ID)
        End Select
    End Sub
    Private Sub RPTsubVersion_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsubVersion.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then

            Dim LITcode_t As Literal = e.Item.FindControl("LITcode_t")
            Dim LITrevision_t As Literal = e.Item.FindControl("LITrevision_t")
            Dim LITaction_t As Literal = e.Item.FindControl("LITaction_t")

            If Not IsNothing(LITcode_t) Then
                Resource.setLiteral(LITcode_t)
            End If

            If Not IsNothing(LITrevision_t) Then
                Resource.setLiteral(LITrevision_t)
            End If
            If Not IsNothing(LITaction_t) Then
                Resource.setLiteral(LITaction_t)
            End If

        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim dto As DTO_EditPreviousVersion = e.Item.DataItem

            If Not IsNothing(dto) Then
                Dim LBLcode As Label = e.Item.FindControl("LBLcode")
                If Not IsNothing(LBLcode) Then
                    LBLcode.Text = dto.Id.ToString()
                End If

                Dim LBLdateTime As Label = e.Item.FindControl("LBLdateTime")
                If Not IsNothing(LBLdateTime) Then
                    LBLdateTime.Text = dto.Lastmodify.ToString()
                End If

                Dim LKBrecover As LinkButton = e.Item.FindControl("LKBrecover")
                If Not IsNothing(LKBrecover) Then
                    Resource.setLinkButton(LKBrecover, True, True, False, False)
                    LKBrecover.CommandArgument = dto.Id
                    LKBrecover.CommandName = "Recover"
                End If

                Dim LKBdelete As LinkButton = e.Item.FindControl("LKBdelete")
                If Not IsNothing(LKBdelete) Then
                    Resource.setLinkButton(LKBdelete, True, True, False, False)
                    LKBdelete.CommandArgument = dto.Id
                    LKBdelete.CommandName = "Delete"
                End If

            End If
        End If
    End Sub
    Private Sub LKBdeleteSel_Click(sender As Object, e As System.EventArgs) Handles LKBdeleteSel.Click
        Dim Ids As New List(Of Int64)

        For Each item As System.Web.UI.WebControls.RepeaterItem In Me.RPTsubVersion.Items
            If item.ItemType = ListItemType.Item OrElse item.ItemType = ListItemType.AlternatingItem Then
                Dim CBXselect As CheckBox = item.FindControl("CBXselect")
                If Not IsNothing(CBXselect) AndAlso CBXselect.Checked Then
                    Dim LKBdelete As LinkButton = item.FindControl("LKBdelete")

                    If Not IsNothing(LKBdelete) AndAlso Not IsNothing(LKBdelete.CommandArgument) Then
                        Try
                            Dim id As Int64 = System.Convert.ToInt64(LKBdelete.CommandArgument)
                            Ids.Add(id)
                        Catch ex As Exception

                        End Try

                    End If

                End If
            End If
        Next

        If Not IsNothing(Ids) AndAlso Ids.Count > 0 Then
            RaiseEvent DeleteItems(Ids)
        End If
    End Sub
#End Region

End Class