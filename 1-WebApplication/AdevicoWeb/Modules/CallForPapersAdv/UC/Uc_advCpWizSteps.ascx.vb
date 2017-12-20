Imports lm.Comol.Modules.CallForPapers.Advanced.dto

Public Class Uc_advCpWizSteps
    Inherits BaseControl
    
    Private count As Integer = 0
    Private currentStep As Integer = 0
    Dim isFirst As Boolean = True

    Public Event ChangePage(Command As String, Argument As String)

    Private Property DisableAll As Boolean
        Get
            Return ViewStateOrDefault("DisableAll", False)
        End Get
        Set(value As Boolean)
            ViewState("DisableAll") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub BindSteps(ByVal steps As IList(Of dtoWizStep))

        currentStep = 0
        count = steps.Count()

        Me.RPTsteps.DataSource = steps
        Me.RPTsteps.DataBind()
    End Sub

    Private Sub RPTsteps_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTsteps.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item As dtoWizStep = e.Item.DataItem

            If IsNothing(item) Then
                Return
            End If

            currentStep += 1

            Dim isLast As Boolean = currentStep >= count

            Dim liContainer As HtmlControl = e.Item.FindControl("liItem")
            Dim oDiv As HtmlControl = e.Item.FindControl("DVnavigationStep")


            If (item.IsSeparator) Then
                Dim oLiteral As Literal = e.Item.FindControl("LTseparator")
                oDiv.Visible = False
                oLiteral.Visible = True
            Else
                oDiv.Visible = True
                liContainer.Attributes.Clear()
                liContainer.Attributes.Add("class", GetCssClass(item, isFirst, isLast))

                Dim oLabelName As Label = e.Item.FindControl("LBitem")
                Dim oLabelDescription As Label = e.Item.FindControl("LBmessage")
                Dim oHyperlink As HyperLink = e.Item.FindControl("HTPlink")
                Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBlink")

                oLabelName.Text = item.Name

                oLabelDescription.Visible = Not String.IsNullOrEmpty(item.Description)
                oLabelDescription.Text = item.Description


                If item.Status = lm.Comol.Core.Wizard.WizardItemStatus.disabled OrElse DisableAll OrElse item.IsCurrent Then
                    oLabelName.Visible = True
                Else
                    oLabelName.Visible = False

                    Dim IsLink As Boolean = Not String.IsNullOrWhiteSpace(item.Link)

                    If (IsLink) Then
                        oHyperlink.Visible = False
                        oLinkbutton.Visible = True

                        'If Not IsLink Then
                        oLinkbutton.Text = item.Name
                        oLinkbutton.ToolTip = item.ToolTip
                        oLinkbutton.CommandName = item.CommandArgument
                        oLinkbutton.CommandArgument = item.Link
                        'Else
                        '    oHyperlink.Text = item.Name
                        '    oHyperlink.ToolTip = item.ToolTip
                        '    oHyperlink.NavigateUrl = item.Link
                        'End If

                    Else

                        oHyperlink.Visible = IsLink
                        oLinkbutton.Visible = Not IsLink

                        If Not IsLink Then
                            oLinkbutton.Text = item.Name
                            oLinkbutton.ToolTip = item.ToolTip
                            oLinkbutton.CommandName = item.CommandArgument
                        Else
                            oHyperlink.Text = item.Name
                            oHyperlink.ToolTip = item.ToolTip
                            oHyperlink.NavigateUrl = item.Link
                        End If

                    End If
                End If

                isFirst = False
            End If

        End If
    End Sub


    Private Function GetCssClass(ByVal item As dtoWizStep, ByVal isFirst As Boolean, ByVal isLast As Boolean) As String
        Dim cssClass As String = "navigationitem"

        If Not dtoWizStep.StatusColor.None = item.Status Then
            cssClass = String.Format("{0} {1}", cssClass, item.Status.ToString().ToLower())
        End If

        If (item.IsCurrent) Then
            cssClass = String.Format("{0} {1}", cssClass, "active")
        End If

        If (item.IsDisabled) Then
            cssClass = String.Format("{0} {1}", cssClass, "disabled")
        End If

        If (isFirst) Then
            cssClass = String.Format("{0} {1}", cssClass, "first")
        End If

        If (isLast) Then
            cssClass = String.Format("{0} {1}", cssClass, "last")
        End If

        Return cssClass
    End Function

    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

    Private Sub RPTsteps_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTsteps.ItemCommand
        Dim argument As String = e.CommandArgument
        Dim command As String = e.CommandName


        RaiseEvent ChangePage(command, argument)

        'Response.Redirect(e.CommandArgument)


    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property
End Class

' ul.navigation li.navigationitem.valid .icons .icon.status
' valid = green
' warning = yellow
'
' first 
' last
' 
' active = current element
' disabled = gray
' 