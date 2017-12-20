Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.TaskList.Domain


Partial Public Class UC_ReallocateResourcesOnNodes
    Inherits BaseControlSession
    Implements IViewUC_ReallocateResourcesOnNodes

    Private _Presenter As ReallocateResourcesOnNodesUCPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

#Region "PAGEBASE"
    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_ReallocateResourcesOnNodes", "TaskList")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            '.setButton(Me.BTNaddUser)
        End With
    End Sub
#End Region

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As ReallocateResourcesOnNodesUCPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ReallocateResourcesOnNodesUCPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#Region "IView"

    Public Property CurrentEditMode() As lm.Comol.Modules.Base.Presentation.TaskList.IViewUC_ReallocateResourcesOnNodes.EditType Implements IViewUC_ReallocateResourcesOnNodes.CurrentEditMode
        Get
            Return Me.ViewState("CurrentEditMode")
        End Get
        Set(ByVal value As IViewUC_ReallocateResourcesOnNodes.EditType)
            Me.ViewState("CurrentEditMode") = value
        End Set
    End Property

    Public Sub LoadResources(ByVal oList As List(Of dtoReallocateTAWithHeader)) Implements IViewUC_ReallocateResourcesOnNodes.LoadResources
        Me.RPTtaskResources.DataSource = oList
        Me.RPTtaskResources.DataBind()
    End Sub

#End Region


    Public Sub RPTtaskResources_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTtaskResources.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDto As lm.Comol.Modules.TaskList.Domain.dtoReallocateTAWithHeader = e.Item.DataItem
            '.....
            Dim oLBtaskName As Label
            Dim oBTNaddUser As System.Web.UI.WebControls.Button
            Dim oIMBmodify As System.Web.UI.WebControls.ImageButton
            oLBtaskName = e.Item.FindControl("LBtaskName")
            oLBtaskName.Text = System.Web.HttpUtility.HtmlEncode(oDto.TaskName)
            oBTNaddUser = e.Item.FindControl("BTNaddUser")
            oIMBmodify = e.Item.FindControl("IMBmodify")
            oBTNaddUser.CommandArgument = oDto.TaskID
            oIMBmodify.CommandArgument = oDto.TaskID
            oIMBmodify.ImageUrl = Me.BaseUrl & "images/Grid/modifica.gif"
            oIMBmodify.ToolTip = Me.Resource.getValue("IMBmodify")
            oBTNaddUser.Text = Me.Resource.getValue("BTNaddUser.text")
            oBTNaddUser.ToolTip = Me.Resource.getValue("BTNaddUser.tool")

            oBTNaddUser.Visible = Me.CurrentEditMode = IViewUC_ReallocateResourcesOnNodes.EditType.Edit
            oIMBmodify.Visible = Me.CurrentEditMode = IViewUC_ReallocateResourcesOnNodes.EditType.Edit
            Dim RPTcomponentResources As Repeater
            RPTcomponentResources = e.Item.FindControl("RPTcomponentResources")
            If Not IsNothing("RPTcomponentResources") Then
                RPTcomponentResources.DataSource = oDto.TaskAssignments
                AddHandler RPTcomponentResources.ItemDataBound, AddressOf RPTcomponentResources_ItemDataBound
                RPTcomponentResources.DataBind()
            End If
        End If
    End Sub
    Public Sub RPTcomponentResources_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType.AlternatingItem Then
            Dim oDtoComp As dtoReallocateTA = e.Item.DataItem
            If Not IsNothing(e.Item.DataItem) Then
                Dim oLTuser, oLTrole, oLTcompleteness As System.Web.UI.WebControls.Literal
                Dim oCBXuser As System.Web.UI.WebControls.CheckBox
                Dim oTBXcompleteness As System.Web.UI.WebControls.TextBox

                oCBXuser = e.Item.FindControl("CBXuser")
                oLTuser = e.Item.FindControl("LTuser")
                oLTcompleteness = e.Item.FindControl("LTcompleteness")
                oTBXcompleteness = e.Item.FindControl("TBXcompleteness")
                oLTrole = e.Item.FindControl("LTrole")


                If Not IsNothing(oLTcompleteness) Then
                    If (oDtoComp.Role = TaskRole.Resource) Or (oDtoComp.Role = TaskRole.Customized_Resource) Then
                        'oLTcompleteness.Visible = True
                        oLTcompleteness.Text = System.Web.HttpUtility.HtmlEncode(oDtoComp.Completeness.ToString())
                    Else
                        oLTcompleteness.Text = "-"
                    End If
                End If

                If Not IsNothing(oTBXcompleteness) Then
                    If ((oDtoComp.Role = TaskRole.Resource) Or (oDtoComp.Role = TaskRole.Customized_Resource)) Then
                        oTBXcompleteness.Text = System.Web.HttpUtility.HtmlEncode(oDtoComp.Completeness.ToString())
                    Else
                        oTBXcompleteness.Text = "0"
                    End If
                End If
                Dim oLabel As Label = e.Item.FindControl("LBpercent")
                If ((oDtoComp.Role = TaskRole.Resource) Or (oDtoComp.Role = TaskRole.Customized_Resource)) _
                  AndAlso (Me.CurrentEditMode = IViewUC_ReallocateResourcesOnNodes.EditType.Edit Or Me.CurrentEditMode = IViewUC_ReallocateResourcesOnNodes.EditType.EditNoButton) Then
                    oTBXcompleteness.Visible = True
                    oLabel.Visible = True
                    oLTcompleteness.Visible = False
                Else
                    oTBXcompleteness.Visible = False
                    oLabel.Visible = False
                    oLTcompleteness.Visible = True
                End If

                If Not IsNothing(oLTuser) Then
                    oLTuser.Text = System.Web.HttpUtility.HtmlEncode(oDtoComp.PersonSurnameName)
                End If

                If Not IsNothing(oLTrole) Then
                    oLTrole.Text = oDtoComp.Role.ToString()
                End If
               
                If Not IsNothing(oCBXuser) Then
                    oCBXuser.Checked = (oDtoComp.isDeleted = False)

                    oCBXuser.Attributes.Add("TaskAssignmentID", oDtoComp.ID.ToString)
                    oCBXuser.Attributes.Add("TaskID", oDtoComp.TaskID.ToString)
                    oCBXuser.Attributes.Add("dtoID", oDtoComp.ID.ToString)
                    oCBXuser.Attributes.Add("PersonID", oDtoComp.PersonID.ToString)
                    'oCBXuser.Text = oCBXuser.Attributes.Item("checkID")
                End If
                Dim oTDitemCheckBox As HtmlTableCell
                oTDitemCheckBox = e.Item.FindControl("TDitemCheckBox")
                If Not IsNothing(oTDitemCheckBox) Then
                    oTDitemCheckBox.Visible = (Me.CurrentEditMode = IViewUC_ReallocateResourcesOnNodes.EditType.Edit Or Me.CurrentEditMode = IViewUC_ReallocateResourcesOnNodes.EditType.EditNoButton)
                End If

            ElseIf e.Item.ItemType = ListItemType.Header Then
                Dim LTheaderCheckBox, LTheaderUser, LTheaderRole, LTheaderCompleteness As System.Web.UI.WebControls.Literal
                LTheaderCheckBox = e.Item.FindControl("LTheaderCheckBox")
                LTheaderUser = e.Item.FindControl("LTheaderUser")
                LTheaderRole = e.Item.FindControl("LTheaderRole")
                LTheaderCompleteness = e.Item.FindControl("LTheaderCompleteness")

                Try
                    Me.Resource.setLiteral(LTheaderCheckBox)
                    Me.Resource.setLiteral(LTheaderUser)
                    Me.Resource.setLiteral(LTheaderRole)
                    Me.Resource.setLiteral(LTheaderCompleteness)
                Catch ex As Exception

                End Try
                Dim oTDheaderCheckBox As HtmlTableCell
                oTDheaderCheckBox = e.Item.FindControl("TDheaderCheckBox")
                If Not IsNothing(oTDheaderCheckBox) Then
                    oTDheaderCheckBox.Visible = (Me.CurrentEditMode = IViewUC_ReallocateResourcesOnNodes.EditType.Edit Or Me.CurrentEditMode = IViewUC_ReallocateResourcesOnNodes.EditType.EditNoButton)
                End If
            End If

        End If
    End Sub

    Public Sub RPTtaskResources_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTtaskResources.ItemCommand

        If e.CommandName = "AddUser" Then
            Try
                Dim oList As List(Of dtoReallocateTA) = Me.CurrentPresenter.GetUserList
                RaiseEvent FatherSelected(e.CommandArgument, oList)
            Catch ex As Exception

            End Try

        ElseIf e.CommandName = "Modify" Then
            Try
                Dim oList As List(Of dtoReallocateTA) = Me.CurrentPresenter.GetUserList
                RaiseEvent ModifyProps(e.CommandArgument, oList)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Public Event FatherSelected(ByVal oID As Long, ByVal O As List(Of dtoReallocateTA))
    Public Event ModifyProps(ByVal oID As Long, ByVal O As List(Of dtoReallocateTA))

    Public Function GetValidateUsersList()
        Me.Page.Validate()
        If Page.IsValid Then
            Return GetUsersList()
        End If
        Return Nothing
    End Function

    Public Function GetValidateUsersListWithHeader()
        Me.Page.Validate()
        If Page.IsValid Then
            Return GetUsersListWithHeader()
        End If
        Return Nothing
    End Function


    Public Function GetUsersList() As List(Of dtoReallocateTA) Implements IViewUC_ReallocateResourcesOnNodes.GetUserList

        Dim RPTtaskResources As System.Web.UI.WebControls.Repeater = Me.RPTtaskResources
        Dim InnerList As List(Of dtoReallocateTA) = New List(Of dtoReallocateTA)
        Dim InnerTA As dtoReallocateTA

        For i As Integer = 0 To RPTtaskResources.Items.Count - 1

            Dim oRepComp As RepeaterItem = RPTtaskResources.Items.Item(i)
            Dim RPTcomponentResources As System.Web.UI.WebControls.Repeater = oRepComp.FindControl("RPTcomponentResources")

            For Each item As RepeaterItem In RPTcomponentResources.Items
                Dim oLTuser As Literal
                oLTuser = item.FindControl("LTuser")
                Dim oUsername As String = System.Web.HttpUtility.HtmlEncode(oLTuser.Text)

                Dim oLTrole As Literal
                oLTrole = item.FindControl("LTrole")
                Dim oRole As TaskRole = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TaskRole).GetByString(oLTrole.Text, TaskRole.None)

                Dim oCBX As CheckBox
                oCBX = item.FindControl("CBXuser")
                Dim TAid As Guid = New System.Guid(oCBX.Attributes.Item("TaskAssignmentID"))
                Dim Tid As Long = CType(oCBX.Attributes.Item("TaskID"), Long)
                Dim TpersonID As Integer = CType(oCBX.Attributes.Item("PersonID"), Integer)

                Dim oLTcomplet As Literal
                Dim oTBXcomplet As TextBox
                oLTcomplet = item.FindControl("LTcompleteness")
                oTBXcomplet = item.FindControl("TBXcompleteness")

                Dim oIsDeleted As Boolean = Not oCBX.Checked

                Dim ToLTcompleteness As Integer
                If oRole = TaskRole.Resource Or oRole = TaskRole.Customized_Resource Then
                    ToLTcompleteness = CType(oTBXcomplet.Text, Integer)
                Else
                    ToLTcompleteness = -1
                End If
                InnerTA = New dtoReallocateTA()
                InnerTA.ID = TAid
                InnerTA.TaskID = Tid
                InnerTA.Completeness = ToLTcompleteness
                InnerTA.PersonSurnameName = oUsername
                InnerTA.Role = oRole
                InnerTA.PersonID = TpersonID
                InnerTA.isDeleted = oIsDeleted
                InnerList.Add(InnerTA)
            Next
        Next

        Return InnerList
    End Function

    Private Function GetUsersListWithHeader() As List(Of dtoReallocateTAWithHeader)

        Dim RPTtaskResources As System.Web.UI.WebControls.Repeater = Me.RPTtaskResources
        Dim InnerList As List(Of dtoReallocateTA)
        Dim InnerTA As dtoReallocateTA
        Dim ListDtoWithHeader As New List(Of dtoReallocateTAWithHeader)
        For i As Integer = 0 To RPTtaskResources.Items.Count - 1

            Dim oRepComp As RepeaterItem = RPTtaskResources.Items.Item(i)
            Dim RPTcomponentResources As System.Web.UI.WebControls.Repeater = oRepComp.FindControl("RPTcomponentResources")
            InnerList = New List(Of dtoReallocateTA)
            For Each item As RepeaterItem In RPTcomponentResources.Items
                Dim oLTuser As Literal
                oLTuser = item.FindControl("LTuser")
                Dim oUsername As String = System.Web.HttpUtility.HtmlEncode(oLTuser.Text)

                Dim oLTrole As Literal
                oLTrole = item.FindControl("LTrole")
                Dim oRole As TaskRole = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TaskRole).GetByString(oLTrole.Text, TaskRole.None)

                Dim oCBX As CheckBox
                oCBX = item.FindControl("CBXuser")
                Dim TAid As Guid = New System.Guid(oCBX.Attributes.Item("TaskAssignmentID"))
                Dim Tid As Long = CType(oCBX.Attributes.Item("TaskID"), Long)
                Dim TpersonID As Integer = CType(oCBX.Attributes.Item("PersonID"), Integer)

                Dim oLTcomplet As Literal
                Dim oTBXcomplet As TextBox
                oLTcomplet = item.FindControl("LTcompleteness")
                oTBXcomplet = item.FindControl("TBXcompleteness")

                Dim oIsDeleted As Boolean = Not oCBX.Checked

                Dim ToLTcompleteness As Integer
                If oRole = TaskRole.Resource Or oRole = TaskRole.Customized_Resource Then
                    ToLTcompleteness = CType(oTBXcomplet.Text, Integer)
                Else
                    ToLTcompleteness = -1
                End If
                InnerTA = New dtoReallocateTA()
                InnerTA.ID = TAid
                InnerTA.TaskID = Tid
                InnerTA.Completeness = ToLTcompleteness
                InnerTA.PersonSurnameName = oUsername
                InnerTA.Role = oRole
                InnerTA.PersonID = TpersonID
                InnerTA.isDeleted = oIsDeleted
                InnerList.Add(InnerTA)
            Next
            Dim LBtaskName As Label = oRepComp.FindControl("LBtaskName")
            Dim dtoWithHeader As New dtoReallocateTAWithHeader
            dtoWithHeader.TaskAssignments = InnerList
            dtoWithHeader.TaskName = System.Web.HttpUtility.HtmlEncode(LBtaskName.Text)
            dtoWithHeader.TaskID = InnerList.First.TaskID
            ListDtoWithHeader.Add(dtoWithHeader)
        Next

        Return ListDtoWithHeader
    End Function
    'Public Sub BTNaddUser_OnClick(ByVal sender As Object, ByVal e As CommandEventArgs)
    '     e.CommandArgument = 
    'End Sub
End Class