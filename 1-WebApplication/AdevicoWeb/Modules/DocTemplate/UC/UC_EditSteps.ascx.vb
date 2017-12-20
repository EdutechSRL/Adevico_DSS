Imports lm.Comol.Core.BaseModules.DocTemplate
Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers

Public Class UC_EditSteps
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event SetCurrentView(ByVal ViewID As Integer)
    Public Property CurrentStepId As Integer
        Get
            Dim Id As Integer = 0
            Try
                Id = System.Convert.ToInt16(ViewState("CurrentElement"))
            Catch ex As Exception

            End Try
            Return Id
        End Get
        Set(value As Integer)
            ViewState("CurrentElement") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templateEdit", "Modules", "DocTemplates")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
    End Sub
#End Region

#Region "Internal"
    Public Sub BindData(ByVal ViewID As Integer, ByVal Views() As Integer)

        CurrentStepId = ViewID

        Dim Values As New List(Of DTO_ViewElement)()
        Dim currentIndex As Integer = 1
        For Each val As Integer In Views
            If (val <> 0) Then
                Dim elm As New DTO_ViewElement()
                elm.id = val
                elm.value = val

                If val = ViewID Then
                    elm.selected = True
                    elm.css = "navigationitem active"
                Else
                    elm.selected = True
                    elm.css = "navigationitem"
                End If

                If currentIndex = 0 Then
                    elm.css &= " first"
                ElseIf currentIndex = Views.Count() Then
                    elm.css &= " last"
                End If

                Values.Add(elm)
                currentIndex += 1

            End If
        Next

        Me.RPTsteps.DataSource = Values
        Me.RPTsteps.DataBind()
    End Sub
#End Region

#Region "Handler"
    Private Sub RPTsteps_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTsteps.ItemCommand
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Try
                Dim ID As Integer = System.Convert.ToInt16(e.CommandArgument)
                Me.CurrentStepId = ID
                RaiseEvent SetCurrentView(ID)
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub RPTsteps_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsteps.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim element As DTO_ViewElement = DirectCast(e.Item.DataItem, DTO_ViewElement)
            Dim LBLitem As Label = e.Item.FindControl("LBLitem")
            Dim LNBlink As LinkButton = e.Item.FindControl("LNBlink")

            If Not IsNothing(LBLitem) AndAlso Not IsNothing(LNBlink) Then
                If element.id = CurrentStepId Then
                    LNBlink.Visible = False
                    LBLitem.Visible = True
                    LBLitem.Text = Me.Resource.getValue(element.value & ".text")
                Else
                    LNBlink.Visible = True
                    LBLitem.Visible = False
                    LNBlink.Text = Me.Resource.getValue(element.value & ".text")
                    LNBlink.CommandName = "GoTo"
                    LNBlink.CommandArgument = element.id
                End If
            End If

            ' SOLO PER TEST!!!
            Dim SPNstatus As System.Web.UI.HtmlControls.HtmlControl = e.Item.FindControl("SPNstatus")
            If Not IsNothing(SPNstatus) Then

                If (element.id < 2) Then
                    SPNstatus.Attributes.Add("class", "valid")
                ElseIf (element.id > 3) Then
                    SPNstatus.Attributes.Add("class", "disabled")
                Else
                    SPNstatus.Attributes.Add("class", "warning")
                End If


                Dim LBLmessage As Label = e.Item.FindControl("LBLmessage")
                If Not IsNothing(LBLmessage) Then
                    LBLmessage.Text = Me.Resource.getValue(element.value & ".description")
                End If
            End If
        End If
    End Sub
#End Region

    Private Class DTO_ViewElement
        Public id As Integer
        Public value As String
        Public css As String
        Public selected As Boolean
    End Class
End Class

