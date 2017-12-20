
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class UC_EnrollDialogMessage
    Inherits DBbaseControl

#Region "Internal"
    Public Property CssClass As String
        Get
            Return ViewStateOrDefault("CssClass", "")
        End Get
        Set(value As String)
            ViewState("CssClass") = value
        End Set
    End Property
    Public Property AllowSelection As Boolean
        Get
            Return ViewStateOrDefault("AllowSelection", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSelection") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBshowEnrollMessageDetails)
            .setLabel(LBhideEnrollMessageDetails)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(ByVal message As String, Optional ByVal items As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll) = Nothing)
        LBnumber.Visible = False
        InternalInitializeControl(message, 1, items)
    End Sub
    Public Sub InitializeControl(ByVal message As String, ByVal number As Integer, Optional ByVal items As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll) = Nothing)
        LBnumber.Visible = True
        DVseeMore.Visible = True
        InternalInitializeControl(message, number, items)
    End Sub
    Public Sub InitializeControlForConstraints(ByVal message As String, Optional ByVal items As List(Of lm.Comol.Core.DomainModel.dtoCommunityConstraint) = Nothing)
        LBnumber.Visible = False
        InternalInitializeControl(message, 1, Nothing, items)
    End Sub
    Public Sub InitializeControl(ByVal message As String, ByVal number As Integer, Optional ByVal items As List(Of lm.Comol.Core.DomainModel.dtoCommunityConstraint) = Nothing)
        LBnumber.Visible = True
        InternalInitializeControl(message, number, Nothing, items)
    End Sub
    Public Sub InitializeControl(ByVal message As String, ByVal number As Integer, ByVal items As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoEnrollment))
        LBnumber.Visible = True
        InternalInitializeControl(message, number, Nothing, Nothing, items)
    End Sub
    Private Sub InternalInitializeControl(ByVal message As String, Optional ByVal number As Integer = 1, Optional ByVal items As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll) = Nothing, Optional ByVal constraints As List(Of lm.Comol.Core.DomainModel.dtoCommunityConstraint) = Nothing, Optional ByVal enrollments As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoEnrollment) = Nothing)
        LBmessage.Text = message
        LBnumber.Text = number
        If Not IsNothing(items) AndAlso items.Any() Then
            RPTdetails.DataSource = items
            RPTdetails.DataBind()
            RPTdetails.Visible = True
            DVseeMore.Visible = True
        ElseIf Not IsNothing(constraints) AndAlso constraints.Any() Then
            RPTdetails.DataSource = constraints
            RPTdetails.DataBind()
            RPTdetails.Visible = True
            DVseeMore.Visible = True
        ElseIf Not IsNothing(enrollments) AndAlso enrollments.Any() Then
            RPTdetails.DataSource = enrollments
            RPTdetails.DataBind()
            RPTdetails.Visible = True
            DVseeMore.Visible = True
        Else
            RPTdetails.Visible = False
            DVseeMore.Visible = False
        End If
    End Sub
    Private Sub RPTdetails_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTdetails.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim idCommunity As Integer = 0
                Dim name As String = ""

                Select Case e.Item.DataItem.GetType
                    Case GetType(lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll)
                        Dim c As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll = DirectCast(e.Item.DataItem, lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunityInfoForEnroll)
                        name = c.Name
                        idCommunity = c.Id
                    Case GetType(lm.Comol.Core.DomainModel.dtoCommunityConstraint)
                        Dim c As lm.Comol.Core.DomainModel.dtoCommunityConstraint = DirectCast(e.Item.DataItem, lm.Comol.Core.DomainModel.dtoCommunityConstraint)
                        name = c.Name
                        idCommunity = c.Id

                    Case GetType(lm.Comol.Core.BaseModules.CommunityManagement.dtoEnrollment)
                        Dim t As lm.Comol.Core.BaseModules.CommunityManagement.dtoEnrollment = DirectCast(e.Item.DataItem, lm.Comol.Core.BaseModules.CommunityManagement.dtoEnrollment)
                        name = t.CommunityName
                        idCommunity = t.IdCommunity
                End Select
                Dim oSpan As HtmlGenericControl = e.Item.FindControl("SPNselect")
                oSpan.Visible = AllowSelection

                Dim oLabel As Label = e.Item.FindControl("LBname")
                oLabel.Visible = Not AllowSelection
                oLabel.Text = name
                oLabel = e.Item.FindControl("LBnameForCheckbox")
                oLabel.Text = name

                Dim oLiteral As Literal = e.Item.FindControl("LTidCommunity")
                oLiteral.Text = idCommunity
               
        End Select
    End Sub

    Public Function GetSelectedItems() As List(Of Integer)
        Dim results As New List(Of Integer)

        For Each row As RepeaterItem In RPTdetails.Items
            Dim oLiteral As Literal = row.FindControl("LTidCommunity")
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBselect")
            If oCheck.Checked Then
                results.Add(CInt(oLiteral.Text))
            End If
        Next
        Return results
    End Function
#End Region


End Class