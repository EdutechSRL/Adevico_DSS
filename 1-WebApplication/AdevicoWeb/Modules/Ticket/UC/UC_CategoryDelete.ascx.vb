Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class UC_CategoryDelete
    Inherits BaseControl
    Implements TK.Presentation.View.iViewCategoryDelete

#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.CategoryDeletePresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.CategoryDeletePresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.CategoryDeletePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"

    Public ReadOnly Property CommunityId As Integer Implements TK.Presentation.View.iViewCategoryDelete.CommunityId
        Get
            Dim ComId As Integer = 0
            Try
                ComId = System.Convert.ToInt32(Me.ViewState("CommunityId"))
            Catch ex As Exception

            End Try
            Return ComId
        End Get
    End Property

    Public Property CurrentStep As TK.Domain.Enums.CategoryDeleteSteps Implements TK.Presentation.View.iViewCategoryDelete.CurrentStep
        Get
            Return Me.MLVwizDelete.ActiveViewIndex + 1
        End Get
        Set(value As TK.Domain.Enums.CategoryDeleteSteps)
            Me.MLVwizDelete.ActiveViewIndex = System.Convert.ToInt32(value)
        End Set
    End Property

    Public ReadOnly Property HasChildren As Boolean Implements TK.Presentation.View.iViewCategoryDelete.HasChildren
        Get
            Try
                Return System.Convert.ToBoolean(ViewState("HasChildre"))
            Catch ex As Exception

            End Try
            Return False
        End Get
    End Property

    Public ReadOnly Property StartStep As TK.Domain.Enums.CategoryDeleteSteps Implements TK.Presentation.View.iViewCategoryDelete.StartStep
        Get
            Try
                Return DirectCast([Enum].Parse(GetType(TK.Domain.Enums.CategoryDeleteSteps), ViewState("StartStep")), TK.Domain.Enums.CategoryDeleteSteps)
            Catch ex As Exception

            End Try
            Return TK.Domain.Enums.CategoryDeleteSteps.Step1_Children
        End Get
    End Property

    Public Property TicketNum As Integer Implements TK.Presentation.View.iViewCategoryDelete.TicketNum
        Get
            Try
                Return System.Convert.ToInt32(ViewState("TicketNum"))
            Catch ex As Exception

            End Try
            Return 0
        End Get
        Set(value As Integer)
            Resource.setLiteral(Me.LTst2Text1)
            Me.LTst2Text1.Text = Me.LTst2Text1.Text.Replace("{num}", value.ToString())
            ViewState("TicketNum") = value.ToString()
        End Set
    End Property


    Public Property CategoryId As Long Implements TK.Presentation.View.iViewCategoryDelete.CategoryId
        Get
            Return System.Convert.ToInt64(Me.ViewState("CategoryId"))
        End Get
        Set(value As Long)
            Me.ViewState("CategoryId") = value
        End Set
    End Property
#End Region

    Public Event EndWizard()
    Public Event GoToTree()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Me.MLVwizDelete.SetActiveView(Me.V_1subCategory)
        If Not Page.IsPostBack() Then
            InitRBL()
        End If

    End Sub

    Private Sub InitRBL()
        ' RBLcssClass="fieldrow deleteoptions" RBLcssItemClass="inlinewrapper" 

        CTRLrblDelCAtegory.SetCss("radiobuttonlist", "inlinewrapper")
        CTRLrblDelCAtegory.AddItemAdvance("MoveUp", Resource, "", True)
        CTRLrblDelCAtegory.AddItemAdvance("DeleteAll", Resource)
        CTRLrblDelCAtegory.AddItemAdvance("Reorder", Resource)


        CTRLrblTicketAss.SetCss("radiobuttonlist", "inlinewrapper")
        CTRLrblTicketAss.AddItemAdvance("ReassignALL", Resource, "", True)
        CTRLrblTicketAss.AddItemAdvance("ReassignSingle", Resource)

    End Sub

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("uc_CategoryDelete", "Modules", "Ticket")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTst1Text1)
            .setLiteral(LTst1Text2)
            .setLiteral(LTst2Text2)
            .setLiteral(LTst3ReassignALL_t)
            .setLiteral(LTst3ReassignALLcate_t)
            .setLiteral(LTst3ReassignSinglecate_t)
            .setLiteral(LTconfirm)
            .setLiteral(LTclose)

            .setLiteral(Me.LTerrorSingle)
            Me.LTerrorSingle.Text = "<span class=""error"">" & LTerrorSingle.Text & "</span>"
            .setLiteral(Me.LTerrorMulti)
            Me.LTerrorMulti.Text = "<span class=""error"">" & LTerrorMulti.Text & "</span>"

            .setLinkButton(LNBundo, True, True)
            .setLinkButton(LNBback, True, True)
            .setLinkButton(LNBnext, True, True)
            .setLinkButton(LNBconfirm, True, True)
            .setLinkButton(LNBexit, True, True)
        End With
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property

    Public Sub InitControl(ByVal CategoryId As Int64, ByVal CommunityId As Integer, ByVal ModuleID As Integer)

        Me.ViewState("CommunityId") = CommunityId.ToString()
        Me.CurrentModuleID = ModuleID

        Me.CategoryId = CategoryId
        Me.CurrentPresenter.InitControl()

    End Sub

    Public Sub InitView(HasChildren As Boolean, StartStep As TK.Domain.Enums.CategoryDeleteSteps) Implements TK.Presentation.View.iViewCategoryDelete.InitView
        ViewState("HasChildre") = HasChildren.ToString()
        ViewState("StartStep") = StartStep.ToString()

        ShowByStep(StartStep)
        'Me.lbl()
    End Sub

    Private DDLCategoryItems As System.Collections.Generic.IList(Of TK.Domain.DTO.DTO_CategoryTree)

    Public Sub SetReassignCategories(SourceCategories As System.Collections.Generic.IList(Of TK.Domain.DTO.DTO_CategoryList), DDLCategories As System.Collections.Generic.IList(Of TK.Domain.DTO.DTO_CategoryTree)) Implements TK.Presentation.View.iViewCategoryDelete.SetReassignCategories
        DDLCategoryItems = DDLCategories
        Me.RPTreassignSingle.DataSource = SourceCategories
        Me.RPTreassignSingle.DataBind()

    End Sub

    Public Sub SetReassignCategory(SourceCategories As TK.Domain.DTO.DTO_CategoryTree, DDLCategories As System.Collections.Generic.IList(Of TK.Domain.DTO.DTO_CategoryTree)) Implements TK.Presentation.View.iViewCategoryDelete.SetReassignCategory
        Me.CTRLddlCategory.InitDDL(DDLCategories, -1, Resource.getValue("DDLCategory.UnAssigned"))
    End Sub



    ''{lblFor}">{text}
    '' <div class="description">{description}
    'Private Function GetRblText(ByVal Text As String, ByVal Description As String) As String

    '    Return Me.LITrblLayout.Text.Replace("{text}", Text).Replace("{description}", Description)

    'End Function

    'Private Sub SetRblItemText(ByVal Container As RadioButtonList, ByVal ItemIndex As Integer)

    '    Dim text As String = Resource.getValue(Container.ID & "." & Container.Items(ItemIndex).Value & ".Text")
    '    Dim description As String = Resource.getValue(Container.ID & "." & Container.Items(ItemIndex).Value & ".Description")

    '    Container.Items(ItemIndex).Text = Me.LITrblLayout.Text.Replace("{text}", text).Replace("{description}", description)

    'End Sub

    Private Sub LNBnext_Click(sender As Object, e As System.EventArgs) Handles LNBnext.Click

        Dim NextStep As TK.Domain.Enums.CategoryDeleteSteps = TK.Domain.Enums.CategoryDeleteSteps.Step1_Children

        Select Case Me.CurrentStep
            Case TK.Domain.Enums.CategoryDeleteSteps.Step1_Children
                Select Case Me.CTRLrblDelCAtegory.SelectedValue
                    Case "MoveUp"
                        Me.CurrentPresenter.SetTicketNum(False)
                    Case "DeleteAll"
                        Me.CurrentPresenter.SetTicketNum(True)
                    Case "Reorder"
                        RaiseEvent GoToTree()
                        'Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.CategoryListTree)
                        Exit Sub
                End Select
                If Me.TicketNum > 0 Then
                    NextStep = TK.Domain.Enums.CategoryDeleteSteps.Step2_Ticket
                Else
                    NextStep = TK.Domain.Enums.CategoryDeleteSteps.Step4_Confirm
                End If

            Case TK.Domain.Enums.CategoryDeleteSteps.Step2_Ticket
                Select Case Me.CTRLrblTicketAss.SelectedValue
                    Case "ReassignALL"
                        NextStep = TK.Domain.Enums.CategoryDeleteSteps.Step3a_ReassignAll
                    Case "ReassignSingle"
                        NextStep = TK.Domain.Enums.CategoryDeleteSteps.Step3b_ReassignSingle
                End Select

            Case TK.Domain.Enums.CategoryDeleteSteps.Step3a_ReassignAll
                If CheckStepSingle() Then
                    NextStep = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryDeleteSteps.Step4_Confirm
                Else
                    Exit Sub
                End If


            Case TK.Domain.Enums.CategoryDeleteSteps.Step3b_ReassignSingle
                If CheckStepMulti() Then
                    NextStep = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryDeleteSteps.Step4_Confirm
                Else
                    Exit Sub
                End If


                'Case TK.Domain.Enums.CategoryDeleteSteps.Step4_Confirm
                '    NextStep = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryDeleteSteps.Step5_END
                'Case TK.Domain.Enums.CategoryDeleteSteps.Step5_END
                '    NextStep = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryDeleteSteps.Step5_END

        End Select

        'If Me.MLVwizDelete.ActiveViewIndex = 0 Then
        '    Me.MLVwizDelete.SetActiveView(Me.V_2subCatTkReassign)
        'Else
        '    Me.MLVwizDelete.SetActiveView(Me.V_1subCategory)
        'End If
        Me.ShowByStep(NextStep)
        Me.CurrentPresenter.BindStep(NextStep)

    End Sub

    Private Sub ShowByStep(ByVal NewStep As TK.Domain.Enums.CategoryDeleteSteps)
        Me.LNBback.Visible = False
        Me.LNBconfirm.Visible = False
        Me.LNBexit.Visible = False
        Me.LNBnext.Visible = False
        Me.LNBundo.Visible = False

        'LITconfirmTEST.Text = ""

        If (System.Convert.ToInt32(NewStep) < System.Convert.ToInt32(Me.StartStep)) Then
            NewStep = StartStep
        End If

        Me.MLVwizDelete.ActiveViewIndex = System.Convert.ToInt32(NewStep) - 1

        If Not Me.StartStep = NewStep Then
            Me.LNBback.Visible = True
        End If

        Select Case NewStep
            Case TK.Domain.Enums.CategoryDeleteSteps.Step1_Children
                Me.LNBnext.Visible = True
                Me.LNBundo.Visible = True

            Case TK.Domain.Enums.CategoryDeleteSteps.Step2_Ticket
                If (Me.TicketNum = 0) Then
                    Me.LNBback.Visible = False
                End If
                Me.LNBnext.Visible = True
                Me.LNBundo.Visible = True

            Case TK.Domain.Enums.CategoryDeleteSteps.Step3a_ReassignAll
                If Not Me.StartStep = NewStep AndAlso (Me.TicketNum > 0 OrElse Me.HasChildren) Then
                    Me.LNBback.Visible = True
                End If
                Me.LNBnext.Visible = True
                Me.LNBundo.Visible = True

            Case TK.Domain.Enums.CategoryDeleteSteps.Step3b_ReassignSingle
                If Not Me.StartStep = NewStep AndAlso (Me.TicketNum > 0 OrElse Me.HasChildren) Then
                    Me.LNBback.Visible = True
                End If
                Me.LNBnext.Visible = True
                Me.LNBundo.Visible = True

            Case TK.Domain.Enums.CategoryDeleteSteps.Step4_Confirm

                Me.LNBconfirm.Visible = True
                Me.LNBundo.Visible = True

                ''ONLY TEST
                'If Me.TicketNum > 0 AndAlso Me.RBLticketAss.SelectedValue = "ReassignSingle" Then
                '    'LITconfirmTEST
                '    Dim KVPl As IList(Of KeyValuePair(Of Int64, Int64)) = Me.GetReassignCategories()
                '    If Not IsNothing(KVPl) Then
                '        For Each kvp As KeyValuePair(Of Int64, Int64) In KVPl
                '            LITconfirmTEST.Text &= "Source: " & kvp.Key.ToString() & " - Dest: " & kvp.Value.ToString() & "<br/>"
                '        Next
                '    End If
                'Else
                '    LITconfirmTEST.Text = "no selection"
                'End If

            Case TK.Domain.Enums.CategoryDeleteSteps.Step5_END
                Me.LNBback.Visible = False
                Me.LNBexit.Visible = True

        End Select


    End Sub


    Private Sub LNBback_Click(sender As Object, e As System.EventArgs) Handles LNBback.Click

        If (CurrentStep = StartStep) Then
            Exit Sub
        End If

        Dim BackStep As TK.Domain.Enums.CategoryDeleteSteps = CurrentStep

        Select Case Me.CurrentStep
            Case TK.Domain.Enums.CategoryDeleteSteps.Step2_Ticket
                If (Me.TicketNum > 0) Then
                    BackStep = TK.Domain.Enums.CategoryDeleteSteps.Step1_Children
                End If
            Case TK.Domain.Enums.CategoryDeleteSteps.Step3a_ReassignAll
                If (Me.TicketNum > 0) Then
                    BackStep = TK.Domain.Enums.CategoryDeleteSteps.Step2_Ticket
                ElseIf (Me.HasChildren) Then
                    BackStep = TK.Domain.Enums.CategoryDeleteSteps.Step1_Children
                End If
            Case TK.Domain.Enums.CategoryDeleteSteps.Step3b_ReassignSingle
                If (Me.TicketNum > 0) Then
                    BackStep = TK.Domain.Enums.CategoryDeleteSteps.Step2_Ticket
                ElseIf (Me.HasChildren) Then
                    BackStep = TK.Domain.Enums.CategoryDeleteSteps.Step1_Children
                End If
            Case TK.Domain.Enums.CategoryDeleteSteps.Step4_Confirm
                If (Me.TicketNum > 0) Then
                    Select Case Me.CTRLrblTicketAss.SelectedValue
                        Case "ReassignALL"
                            BackStep = TK.Domain.Enums.CategoryDeleteSteps.Step3a_ReassignAll
                        Case "ReassignSingle"
                            BackStep = TK.Domain.Enums.CategoryDeleteSteps.Step3b_ReassignSingle
                    End Select
                ElseIf Me.HasChildren Then
                    BackStep = TK.Domain.Enums.CategoryDeleteSteps.Step1_Children
                End If
        End Select

        Me.ShowByStep(BackStep)
    End Sub

    Public ReadOnly Property CountChildren As Boolean Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewCategoryDelete.CountChildren
        Get

        End Get
    End Property

    Private Sub RPTreassignSingle_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTreassignSingle.ItemDataBound

        If e.Item.ItemType = ListItemType.Header Then
            Dim LITcateName_t As Literal = e.Item.FindControl("LITcateName_t")
            If Not IsNothing(LITcateName_t) Then
                Me.Resource.setLiteral(LITcateName_t)
            End If
            Dim LITnewCate_t As Literal = e.Item.FindControl("LITnewCate_t")
            If Not IsNothing(LITnewCate_t) Then
                Me.Resource.setLiteral(LITnewCate_t)
            End If
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim DTOCat As TK.Domain.DTO.DTO_CategoryList = e.Item.DataItem

            If Not IsNothing(DTOCat) Then
                Dim LITtr As Literal = e.Item.FindControl("LITtr")
                If Not IsNothing(LITtr) Then
                    LITtr.Text = LITtr.Text.Replace("{Id}", DTOCat.Id)

                    If DTOCat.FatherId <= 0 Then
                        LITtr.Text = LITtr.Text.Replace("{css}", "")
                    Else
                        LITtr.Text = LITtr.Text.Replace("{css}", "child-of-ctg-" & DTOCat.FatherId.ToString())
                    End If
                    'LITtr.Text.Replace("", DTOCat.Id)

                End If
                Dim HIDid As HiddenField = e.Item.FindControl("HIDid")
                If Not IsNothing(HIDid) Then
                    HIDid.Value = DTOCat.Id
                End If
                Dim LBLcateName As Label = e.Item.FindControl("LBLcateName")
                If Not IsNothing(LBLcateName) Then
                    LBLcateName.Text = DTOCat.Name
                    LBLcateName.ToolTip = DTOCat.Description
                End If
                Dim UCddlCate As Comunita_OnLine.UC_CategoryDDL = e.Item.FindControl("UCddlCate")
                If Not IsNothing(UCddlCate) AndAlso Not IsNothing(DDLCategoryItems) Then
                    UCddlCate.InitDDL(DDLCategoryItems, -1, Resource.getValue("DDLCategory.UnAssigned"))
                End If
            End If
        End If

    End Sub

    Private Function GetReassignCategories() As IDictionary(Of Int64, Int64)
        Dim DictIDs As New Dictionary(Of Int64, Int64)()

        If Not IsNothing(Me.RPTreassignSingle.Items) Then
            For Each Item As System.Web.UI.WebControls.RepeaterItem In Me.RPTreassignSingle.Items
                If Item.ItemType = ListItemType.Item OrElse Item.ItemType = ListItemType.AlternatingItem Then

                    Dim SourceId As Int64 = -1
                    Dim HIDid As HiddenField = Item.FindControl("HIDid")
                    If Not IsNothing(HIDid) Then
                        Try
                            SourceId = System.Convert.ToInt64(HIDid.Value)
                        Catch ex As Exception

                        End Try
                    End If

                    Dim DestId As Int64 = -1
                    Dim UCddlCate As Comunita_OnLine.UC_CategoryDDL = Item.FindControl("UCddlCate")
                    If Not IsNothing(UCddlCate) Then
                        DestId = UCddlCate.SelectedId
                    End If

                    If SourceId > 0 AndAlso DestId > 0 AndAlso Not DictIDs.ContainsKey(SourceId) Then
                        DictIDs.Add(SourceId, DestId)
                    End If
                End If
            Next
        End If
        Return DictIDs
    End Function

    Private Sub LNBundo_Click(sender As Object, e As System.EventArgs) Handles LNBundo.Click
        Me.PNLmain.Visible = False
    End Sub


    Public Property IsVisible As Boolean
        Get
            Return Me.PNLmain.Visible
        End Get
        Set(value As Boolean)
            Me.PNLmain.Visible = value
        End Set
    End Property

    Private Sub LNBconfirm_Click(sender As Object, e As System.EventArgs) Handles LNBconfirm.Click
        Dim deleted As Boolean = True
        'SALVA I DATI...
        Dim MoveUp As Boolean = False
        Dim ReassignSingle As Boolean = False

        If Me.CTRLrblDelCAtegory.SelectedValue = "MoveUp" Then
            MoveUp = True
        End If

        If Me.CTRLrblTicketAss.SelectedValue = "ReassignSingle" Then
            ReassignSingle = True
        End If

        Dim ReassignString As String = Resource.getValue("Reassign.message")

        If MoveUp OrElse Not ReassignSingle Then
            Dim NewCategoryId As Int64 = Me.CTRLddlCategory.SelectedId
            If (NewCategoryId <= 0) Then
                NewCategoryId = -1
            End If

            deleted = Me.CurrentPresenter.DeleteCategory(ReassignString, NewCategoryId, MoveUp)

        Else
            deleted = Me.CurrentPresenter.DeleteCategory(ReassignString, GetReassignCategories)
        End If

        If deleted Then
            Me.PNLmain.Visible = False
            RaiseEvent EndWizard()
            Exit Sub
        Else
            'Gestione errori...
        End If

    End Sub
    Private Function CheckStepSingle() As Boolean

        Dim CurrentId As Int64 = Me.CTRLddlCategory.SelectedId
        If (CurrentId <= 0) Then
            Me.LTerrorSingle.Visible = True
            Return False
        Else
            Me.LTerrorSingle.Visible = False
        End If

        Return True

    End Function

    Private Function CheckStepMulti() As Boolean

        Dim HasNoError As Boolean = True

        If Not IsNothing(Me.RPTreassignSingle.Items) Then
            For Each Item As System.Web.UI.WebControls.RepeaterItem In Me.RPTreassignSingle.Items
                Dim DestId As Int64 = -1
                Dim UCddlCate As Comunita_OnLine.UC_CategoryDDL = Item.FindControl("UCddlCate")
                If Not IsNothing(UCddlCate) Then
                    DestId = UCddlCate.SelectedId
                End If

                Dim LITtr As Literal = Item.FindControl("LITtr")
                Dim LITerrorCate As Literal = Item.FindControl("LITerrorCate")

                If (DestId <= 0) Then
                    If Not IsNothing(LITtr) Then
                        LITtr.Text = LITtr.Text.Replace("category", "category error")
                    End If

                    If Not IsNothing(LITerrorCate) Then
                        LITerrorCate.Visible = True
                    End If
                    HasNoError = False

                Else
                    If Not IsNothing(LITtr) Then
                        LITtr.Text = LITtr.Text.Replace("category error", "category")
                    End If

                    If Not IsNothing(LITerrorCate) Then
                        LITerrorCate.Visible = False
                    End If
                End If

            Next
        End If

        Me.LTerrorMulti.Visible = Not HasNoError
        Return HasNoError
    End Function

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

        CTRLrblDelCAtegory.ItemAttributesResetCssClass("inlinewrapper")
        CTRLrblTicketAss.ItemAttributesResetCssClass("inlinewrapper")

    End Sub

    Public Sub SendAction( _
                         Action As TK.ModuleTicket.ActionType, _
                         idCommunity As Integer, _
                         Type As TK.ModuleTicket.InteractionType, _
                        Optional Objects As System.Collections.Generic.IList(Of System.Collections.Generic.KeyValuePair(Of Integer, String)) = Nothing) _
                     Implements TK.Presentation.View.iViewCategoryDelete.SendAction
        
        Dim oList As List(Of WS_Actions.ObjectAction) = Nothing

        If Not IsNothing(Objects) Then
            oList = (From kvp As KeyValuePair(Of Integer, String) In Objects
                    Select Me.PageUtility.CreateObjectAction(kvp.Key, kvp.Value)).ToList()
        End If

        Me.PageUtility.AddActionToModule(idCommunity, Me.CurrentModuleID, Action, oList, Type)
    End Sub

    Private _CurrentModuleID As Integer = 0
    Private Property CurrentModuleID As Integer
        Get
            If _CurrentModuleID <= 0 Then
                Try
                    _CurrentModuleID = System.Convert.ToInt32(ViewState("CurrentModuleID"))
                Catch ex As Exception
                    _CurrentModuleID = 0
                End Try

            End If
            Return _CurrentModuleID
        End Get
        Set(value As Integer)
            _CurrentModuleID = value
            ViewState("CurrentModuleID") = value
        End Set
    End Property
End Class