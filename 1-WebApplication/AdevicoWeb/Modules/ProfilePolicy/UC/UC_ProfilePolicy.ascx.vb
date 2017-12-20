Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.PolicyManagement
Imports lm.Comol.Core.BaseModules.PolicyManagement.Presentation

Public Class UC_ProfilePolicy
    Inherits BaseControl
    Implements IViewProfilePolicySubmission


#Region "Context"
    Private _Presenter As ProfilePolicySubmission
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As ProfilePolicySubmission
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProfilePolicySubmission(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region
#Region ""
    Public Event AllMandatoryAccepted()
    Public Event SomeMandatoryNotAccepted()
    Public Property CurrentIdUser As Integer Implements IViewProfilePolicySubmission.CurrentIdUser
        Get
            Return ViewStateOrDefault("CurrentIdUser", CInt(0))
        End Get
        Set(value As Integer)
            Me.ViewState("CurrentIdUser") = value
        End Set
    End Property
    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
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
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AcceptLogonPolicy", "Modules", "ProfilePolicy")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

    Public Sub InitializeControl(idUser As Integer) Implements IViewProfilePolicySubmission.InitializeControl
        Me.SetInternazionalizzazione()
        Me.CurrentPresenter.InitView(idUser)
    End Sub
    Public Sub InitializeControl() Implements IViewProfilePolicySubmission.InitializeControl
        Me.SetInternazionalizzazione()
        Me.CurrentPresenter.InitAnonymousView()
    End Sub

    Public Sub LoadItems(items As List(Of dtoUserDataPolicy)) Implements IViewProfilePolicySubmission.LoadItems
        Me.RPTpolicyInfo.Visible = True
        Me.RPTpolicyInfo.DataSource = items
        Me.RPTpolicyInfo.DataBind()
    End Sub

    Private Sub RPTpolicyInfo_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTpolicyInfo.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim item As dtoUserDataPolicy = DirectCast(e.Item.DataItem, dtoUserDataPolicy)

            Dim oLiteral As Literal = e.Item.FindControl("LTitemId")
            oLiteral.Text = item.Id


            oLiteral = e.Item.FindControl("LTitemUserId")
            If Not IsNothing(item.UserInfo) Then
                oLiteral.Text = item.UserInfo.Id.ToString
            Else
                oLiteral.Text = "0"
            End If

            Dim oLabel As Label = e.Item.FindControl("LBname")
            oLabel.Text = item.Name

            Dim div As HtmlControl = e.Item.FindControl("DVdescription")
            oLiteral = e.Item.FindControl("LTdescription")
            If String.IsNullOrEmpty(item.Text) Then
                div.Visible = False
            Else
                div.Visible = True
                oLiteral.Text = item.Text
            End If

            Dim oChecbox As CheckBox = e.Item.FindControl("CBXsingle")
            Dim oRadioButtonList As RadioButtonList = e.Item.FindControl("RBLmultiple")
            Dim showChecbox As Boolean = (item.Type = lm.Comol.Core.PersonalInfo.PolicyType.acceptOnly OrElse item.Type = lm.Comol.Core.PersonalInfo.PolicyType.agreeOnly)
            Dim validator As RequiredFieldValidator = e.Item.FindControl("RFVmandatory")
            oChecbox.Visible = showChecbox
            oRadioButtonList.Visible = Not (showChecbox)

            If showChecbox Then
                oChecbox.Text = Me.Resource.getValue("PolicyType." & item.Type.ToString)
                If Not IsNothing(item.UserInfo) Then
                    oChecbox.Checked = item.UserInfo.Accepted
                Else
                    oChecbox.Checked = False
                End If
                'If item.Mandatory Then
                '    validator.Visible = True
                '    validator.ControlToValidate = oChecbox.ClientID
                'End If
            Else
                oRadioButtonList.Items.Clear()
                If item.Type = lm.Comol.Core.PersonalInfo.PolicyType.acceptRefuse Then
                    oRadioButtonList.Items.Add(New ListItem(Me.Resource.getValue("PolicyType.accept"), True))
                    oRadioButtonList.Items.Add(New ListItem(Me.Resource.getValue("PolicyType.refuse"), False))
                Else
                    oRadioButtonList.Items.Add(New ListItem(Me.Resource.getValue("PolicyType.agree"), True))
                    oRadioButtonList.Items.Add(New ListItem(Me.Resource.getValue("PolicyType.disagree"), False))
                End If
                If Not IsNothing(item.UserInfo) Then
                    oRadioButtonList.SelectedValue = item.UserInfo.Accepted
                Else
                    oRadioButtonList.SelectedValue = False
                End If
            End If
            oLiteral = e.Item.FindControl("LTmandatory")
            oLiteral.Text = item.Mandatory

            oLiteral = e.Item.FindControl("LTtype")
            oLiteral.Text = item.Type.ToString
        End If
    End Sub


    Public Function GetItemsValue() As List(Of dtoUserPolicyInfo) Implements IViewProfilePolicySubmission.GetItemsValue
        Dim result As New List(Of dtoUserPolicyInfo)

        For Each item As RepeaterItem In (From r As RepeaterItem In RPTpolicyInfo.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList
            Dim oLiteral As Literal = item.FindControl("LTitemId")
            Dim info As New dtoUserPolicyInfo

            info.PolicyId = CLng(oLiteral.Text)
            oLiteral = item.FindControl("LTitemUserId")
            info.Id = CLng(oLiteral.Text)

            oLiteral = item.FindControl("LTtype")
            Dim type As lm.Comol.Core.PersonalInfo.PolicyType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.PersonalInfo.PolicyType).GetByString(oLiteral.Text, lm.Comol.Core.PersonalInfo.PolicyType.none)

            If (type = lm.Comol.Core.PersonalInfo.PolicyType.acceptOnly OrElse type = lm.Comol.Core.PersonalInfo.PolicyType.agreeOnly) Then
                Dim oChecbox As CheckBox = item.FindControl("CBXsingle")
                info.Accepted = oChecbox.Checked
            ElseIf type = lm.Comol.Core.PersonalInfo.PolicyType.none Then
            Else
                Dim oRadioButtonList As RadioButtonList = item.FindControl("RBLmultiple")
                info.Accepted = CBool(oRadioButtonList.SelectedValue)
            End If
            result.Add(info)
        Next
        Return result
    End Function

    Public Sub DisplayItemsToAccept(items As List(Of dtoUserDataPolicy)) Implements IViewProfilePolicySubmission.DisplayItemsToAccept

    End Sub

    Public Sub DisplayUnknownUser() Implements IViewProfilePolicySubmission.DisplayUnknownUser

    End Sub


    Public Sub LoadItemsError() Implements IViewProfilePolicySubmission.LoadItemsError

    End Sub

    Public Sub SaveItems() Implements IViewProfilePolicySubmission.SaveItems
        If CurrentPresenter.SavePolicy() Then
            RaiseEvent AllMandatoryAccepted()
        End If
    End Sub

    Public Sub DisplayNoPolicyToAccept() Implements lm.Comol.Core.BaseModules.PolicyManagement.Presentation.IViewProfilePolicySubmission.DisplayNoPolicyToAccept

    End Sub

    Public ReadOnly Property AcceptedMandatoryPolicy As Boolean Implements IViewProfilePolicySubmission.AcceptedMandatoryPolicy
        Get
            Dim result As Boolean = True

            For Each item As RepeaterItem In (From r As RepeaterItem In RPTpolicyInfo.Items _
                                              Where (r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item) _
                                            AndAlso Not IsNothing(r.FindControl("LTmandatory"))
                                              Select r).ToList.Where(Function(r) DirectCast(r.FindControl("LTmandatory"), Literal).Text = True).ToList()
                Dim oLiteral As Literal = item.FindControl("LTtype")
                Dim type As lm.Comol.Core.PersonalInfo.PolicyType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.PersonalInfo.PolicyType).GetByString(oLiteral.Text, lm.Comol.Core.PersonalInfo.PolicyType.none)

                If (type = lm.Comol.Core.PersonalInfo.PolicyType.acceptOnly OrElse type = lm.Comol.Core.PersonalInfo.PolicyType.agreeOnly) Then
                    Dim oChecbox As CheckBox = item.FindControl("CBXsingle")
                    If Not oChecbox.Checked Then
                        result = False
                        Exit For
                    End If
                ElseIf type = lm.Comol.Core.PersonalInfo.PolicyType.none Then
                Else
                    Dim oRadioButtonList As RadioButtonList = item.FindControl("RBLmultiple")
                    If Not CBool(oRadioButtonList.SelectedValue) Then

                        result = False
                        Exit For
                    End If
                End If
            Next
            Return result
        End Get
    End Property

    Public Sub ReloadLanguageSettings(language As Lingua)
        Me.OverloadLanguage(language)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
    End Sub
End Class