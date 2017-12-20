Imports lm.Comol.Modules.Standard.Glossary.Domain.Dto


Public Class ImportGlossary
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            InitWizard()
        End If
    End Sub

#Region "Navigation"

    Private Sub InitWizard()
        Me.CurrentStep = MyStep.SelectCommunity

        Me.UC_Community.SelectionMode = ListSelectionMode.Single

        Me.UC_Community.isModalitaAmministrazione = True
        Me.UC_Community.AllowMultipleOrganizationSelection = False
        Me.UC_Community.InitializeControl(- 1)
        Me.UC_Community.SelectedCommunitiesID = New List(Of Integer)

        Me.Resource.setLabel_To_Value(LBstepTitle, "Step1.Title")
        Me.Resource.setLabel_To_Value(LBstepDescription, "Step1.Description")
    End Sub

    Private Sub BindStep(ByVal BindGlossary As Boolean)

        Me.BTNcompleteBottom.Enabled = False
        Me.BTNcompleteBottom.Visible = False
        Me.BTNcompleteTop.Enabled = False
        Me.BTNcompleteTop.Visible = False

        Me.BTNbackBottom.Enabled = True
        Me.BTNbackBottom.Visible = True
        Me.BTNbackTop.Enabled = True
        Me.BTNbackTop.Visible = True

        Me.BTNnextBottom.Enabled = True
        Me.BTNnextBottom.Visible = True
        Me.BTNnextTop.Enabled = True
        Me.BTNnextTop.Visible = True

        Select Case CurrentStep

            Case MyStep.SelectCommunity
                Me.BTNbackBottom.Enabled = False
                Me.BTNbackTop.Enabled = False

                Me.Resource.setLabel_To_Value(LBstepTitle, "Step1.Title")
                Me.Resource.setLabel_To_Value(LBstepDescription, "Step1.Description")

            Case MyStep.SelectGlossary

                Dim CommunityId As Integer = Me.UC_Community.SelectedCommunitiesID(0)

                If BindGlossary Then
                    Me.UC_Glossary.InitControl(CommunityId)
                End If


                Me.Resource.setLabel_To_Value(LBstepTitle, "Step2.Title")
                Me.Resource.setLabel_To_Value(LBstepDescription, "Step2.Description")

            Case MyStep.Summary
                Me.BTNnextBottom.Enabled = False
                Me.BTNnextBottom.Visible = False
                Me.BTNnextTop.Enabled = False
                Me.BTNnextTop.Visible = False

                Me.BTNcompleteBottom.Enabled = True
                Me.BTNcompleteBottom.Visible = True
                Me.BTNcompleteTop.Enabled = True
                Me.BTNcompleteTop.Visible = True

                Dim Summaries As IList(Of GlossaryGroupSummaryDTO) = Me.UC_Glossary.GetSelectedGlossarySummary

                If Summaries.Count > 0 Then
                    Me.Rpt_Summary.Visible = True
                    Me.Rpt_Summary.DataSource = Summaries
                    Me.Rpt_Summary.DataBind()
                    Lbl_NoItemSelected.Visible = False
                Else
                    Me.Rpt_Summary.Visible = False
                    Lbl_NoItemSelected.Visible = True
                End If

                Me.Resource.setLabel_To_Value(LBstepTitle, "Step3.Title")
                Me.Resource.setLabel_To_Value(LBstepDescription, "Step3.Description")

            Case MyStep.FinalStep
                Me.BTNbackBottom.Enabled = False
                Me.BTNbackBottom.Visible = False
                Me.BTNbackTop.Enabled = False
                Me.BTNbackTop.Visible = False

                Me.BTNnextBottom.Enabled = False
                Me.BTNnextBottom.Visible = False
                Me.BTNnextTop.Enabled = False
                Me.BTNnextTop.Visible = False

                Me.Resource.setLabel_To_Value(LBstepTitle, "Step4.Title")
                Me.Resource.setLabel_To_Value(LBstepDescription, "Step4.Description")

        End Select
    End Sub

    Private Sub NextStep()

        Me.Lbl_Error.Visible = False

        Select Case Me.CurrentStep
            Case MyStep.SelectCommunity

                Dim CommunityId As Integer = - 1
                Try
                    CommunityId = Me.UC_Community.SelectedCommunitiesID(0)
                Catch ex As Exception

                End Try

                If CommunityId > 0 Then
                    Me.CurrentStep = MyStep.SelectGlossary
                Else
                    Me.Resource.setLabel_To_Value(Me.Lbl_Error, "Lbl_Error.NoCommunity")
                    Me.Lbl_Error.Visible = True
                End If

            Case MyStep.SelectGlossary
                Me.CurrentStep = MyStep.Summary
            Case MyStep.Summary
                Me.CurrentStep = MyStep.FinalStep
            Case MyStep.FinalStep
                Exit Sub
        End Select

        Me.BindStep(True)
    End Sub

    Private Sub BackStep()
        Select Case Me.CurrentStep
            Case MyStep.SelectCommunity
                'Me.CurrentStep = MyStep.SelectCommunity
            Case MyStep.SelectGlossary
                Me.CurrentStep = MyStep.SelectCommunity
            Case MyStep.Summary
                Me.CurrentStep = MyStep.SelectGlossary
            Case MyStep.FinalStep
                Exit Sub
        End Select

        Me.BindStep(False)
    End Sub

    Private Sub Confirm()
        Dim copiedElement As Integer = Me.UC_Glossary.MakeCopy(Me.ComunitaCorrenteID, False)
        Me.Lbl_Complete_pre.Text = Me.Resource.getValue("Complete.Pre")
        Me.Lbl_Complete_post.Text = copiedElement.ToString() & Me.Resource.getValue("Complete.Post")
        Me.NextStep()
    End Sub

#End Region

#Region "Navigation property"

    Private Property CurrentStep As MyStep
        Get
            Select Case Me.MLVwizard.GetActiveView.ID
                Case "VIWsourceCom"
                    Return MyStep.SelectCommunity
                Case "VIWsourceGlossary"
                    Return MyStep.SelectGlossary
                Case "VIWconfirm"
                    Return MyStep.Summary
                Case "VIWcomplete"
                    Return MyStep.FinalStep
            End Select
        End Get
        Set(value As MyStep)
            Select Case value
                Case MyStep.SelectCommunity
                    Me.MLVwizard.SetActiveView(Me.VIWsourceCom)
                Case MyStep.SelectGlossary
                    Me.MLVwizard.SetActiveView(Me.VIWsourceGlossary)
                Case MyStep.Summary
                    Me.MLVwizard.SetActiveView(Me.VIWconfirm)
                Case MyStep.FinalStep
                    Me.MLVwizard.SetActiveView(Me.VIWcomplete)
            End Select
        End Set
    End Property

    Private Enum MyStep As Integer
        SelectCommunity = 1
        SelectGlossary = 2
        Summary = 10
        FinalStep = 11
    End Enum

#End Region

#Region "Buttons events"

    Private Sub BTNnextBottom_Click(sender As Object, e As EventArgs) Handles BTNnextBottom.Click
        Me.NextStep()
    End Sub

    Private Sub BTNnextTop_Click(sender As Object, e As EventArgs) Handles BTNnextTop.Click
        Me.NextStep()
    End Sub


    Private Sub Rpt_Summary_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) _
        Handles Rpt_Summary.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim Summary As GlossaryGroupSummaryDTO = e.Item.DataItem

            Dim Lbl_selGlo As Label = e.Item.FindControl("Lbl_selGlo")
            If Not IsNothing(Lbl_selGlo) Then
                Lbl_selGlo.Text = Summary.Name
            End If

            Dim lit_NumElement As Literal = e.Item.FindControl("lit_NumElement")
            If Not IsNothing(lit_NumElement) Then
                lit_NumElement.Text = Summary.NumItem
            End If

            Dim lit_NumElement_t As Literal = e.Item.FindControl("lit_NumElement_t")
            If Not IsNothing(lit_NumElement) Then
                Me.Resource.setLiteral(lit_NumElement_t)
            End If

        End If
    End Sub

    Private Sub BTNbackBottom_Click(sender As Object, e As EventArgs) Handles BTNbackBottom.Click
        Me.BackStep()
    End Sub

    Private Sub BTNbackTop_Click(sender As Object, e As EventArgs) Handles BTNbackTop.Click
        Me.BackStep()
    End Sub

    Private Sub BTNcompleteTop_Click(sender As Object, e As EventArgs) Handles BTNcompleteTop.Click
        Me.Confirm()
    End Sub

    Private Sub BTNcompleteBottom_Click(sender As Object, e As EventArgs) Handles BTNcompleteBottom.Click
        Me.Confirm()
    End Sub

#End Region

#Region "PageBase"

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
        End Get
    End Property

    Public Overrides Sub BindDati()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Resource.setLabel(LBLNoPermission)
        PNLnoPermision.Visible = True
    End Sub


    Public Overrides Function HasPermessi() As Boolean
        Return Me.UC_Glossary.HasComPermission(MyBase.PageUtility.CurrentContext.UserContext.CurrentCommunityID)
    End Function

    Public Overrides Sub RegistraAccessoPagina()
    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("GlossaryImport", "Glossary")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

        Me.Resource.setButton(BTNbackTop, True, False, False, True)
        Me.Resource.setButton(BTNnextTop, True, False, False, True)
        Me.Resource.setButton(BTNcompleteTop, True, False, False, True)

        Me.Resource.setButton(BTNbackBottom, True, False, False, True)
        Me.Resource.setButton(BTNnextBottom, True, False, False, True)
        Me.Resource.setButton(BTNcompleteBottom, True, False, False, True)

        Me.Resource.setLabel(Lbl_NoItemSelected)

        Me.Resource.setLabel(Lbl_SelectedGlossary_t)

        Me.Resource.setLiteral(Lit_PageTitle)
        Me.Resource.setLiteral(Lit_PageTitle_top)
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return True
        End Get
    End Property

#End Region

    Protected Function GetBaseUrl() As String
        Return String.Empty
    End Function
End Class