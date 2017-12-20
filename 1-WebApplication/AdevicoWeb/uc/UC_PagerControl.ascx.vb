Imports lm.Comol.Core.DomainModel

Partial Public Class UC_PagerControl
	Inherits BaseControlWithLoad
	Dim _pager As PagerBase

	Public Event OnPageSelected()
	Public Enum Command
		FirstPage = -4
		PrevPage = -3
		NextPage = -2
		LastPage = -1
    End Enum
    Public Property EnableQueryString() As Boolean
        Get
			Try
				Return Me.ViewState("EnableQueryString")
			Catch ex As Exception
				Me.ViewState("EnableQueryString") = False
				Return False
			End Try
        End Get
        Set(ByVal value As Boolean)
            Me.spanbutton.Visible = Not value
			Me.spannavigate.Visible = value
			Me.ViewState("EnableQueryString") = value
        End Set
    End Property
    Public Property Pager() As PagerBase
        Get
            Return _pager
        End Get
        Set(ByVal value As PagerBase)
            _pager = value
            Try
                SetPagerLabels()
            Catch ex As Exception
            End Try
        End Set
    End Property
    Public Property ShowNavigationButton() As Boolean
        Get
            Return Me.ViewState("ShowNavigationButton")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ShowNavigationButton") = value
        End Set
    End Property
    Public Property BaseNavigateUrl() As String
        Get
            Return Me.ViewState("BaseNavigateUrl")
        End Get
        Set(ByVal value As String)
            Me.ViewState("BaseNavigateUrl") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub


	Private Sub SetButtonNavigation()
		Me.LTseparator_First.Visible = False
		Me.LTseparator_Last.Visible = False
		Me.LTseparator_Last.Visible = False
		Me.LTseparator_Next.Visible = False
		If ShowNavigationButton Then
			IMBfirst.Visible = Not (Pager.isFirstPage Or Pager.isSecondPage)
			IMBlast.Visible = Not (Pager.isLastPage Or Pager.isPenultimatePage)
			IMBprev.Visible = Not (Pager.isFirstPage)
			IMBnext.Visible = Not (Pager.isLastPage)
			If IMBfirst.Visible AndAlso Me.IMBprev.Visible Then
				Me.LTseparator_First.Visible = True
				Me.LTseparator_Previous.Visible = True
			ElseIf IMBfirst.Visible Then
				Me.LTseparator_Previous.Visible = True
			End If
			If IMBlast.Visible AndAlso Me.IMBnext.Visible Then
				Me.LTseparator_Last.Visible = True
				Me.LTseparator_Next.Visible = True
			ElseIf IMBlast.Visible Then
				Me.LTseparator_Next.Visible = True
			End If
		Else
			IMBfirst.Visible = False
			IMBlast.Visible = False
			IMBprev.Visible = False
			IMBnext.Visible = False
		End If
	End Sub

	Private Sub SetPagerLabels()
        If Me.EnableQueryString Then : SetHyperLink()
        Else : SetLinkButton()
        End If
	End Sub


    Private Sub SetLinkButton()
        For Each ctr As UI.Control In spanbutton.Controls
            Dim oLinkPage As UI.WebControls.LinkButton
            oLinkPage = TryCast(ctr, UI.WebControls.LinkButton)

            If oLinkPage IsNot Nothing Then
                Dim val As Integer = CInt(oLinkPage.ID.Replace("LNB", ""))
                oLinkPage.Text = Pager.DeltaFirst + val
                oLinkPage.CommandArgument = Pager.DeltaFirst + val - 1
                If Pager.DeltaFirst + val - 1 <= Pager.LastPage Then
                    oLinkPage.Visible = True
                Else
                    oLinkPage.Visible = False
                End If
                If Pager.DeltaFirst + val <= 0 Then
                    oLinkPage.Visible = False
                End If

                Dim val1 As Integer = CInt(oLinkPage.CommandArgument)
                If val1 = Pager.PageIndex Then
                    '	oLinkPage.Text = "[" + oLinkPage.Text + "]"
                    oLinkPage.CssClass = "PagerSpan"
                    oLinkPage.ToolTip = String.Format(Me.Resource.getValue("ActualPage"), oLinkPage.Text)
                Else
                    oLinkPage.CssClass = "PagerLink"
                    oLinkPage.ToolTip = String.Format(Me.Resource.getValue("GoToPage"), oLinkPage.Text)
                End If
            End If
        Next
    End Sub
    Private Sub SetHyperLink()
        For Each ctr As UI.Control In spannavigate.Controls
            Dim oHyperLink As UI.WebControls.HyperLink
            oHyperLink = TryCast(ctr, UI.WebControls.HyperLink)

            If oHyperLink IsNot Nothing Then
                Dim val As Integer = CInt(oHyperLink.ID.Replace("HYP", ""))
                Dim GoToPage As Integer = Pager.DeltaFirst + val - 1
                oHyperLink.Text = Pager.DeltaFirst + val
                Try
                    oHyperLink.NavigateUrl = String.Format(Me.BaseNavigateUrl, GoToPage)
                Catch ex As Exception
                    oHyperLink.NavigateUrl = "#"
                End Try

                If Pager.DeltaFirst + val - 1 <= Pager.LastPage Then
                    oHyperLink.Visible = True
                Else
                    oHyperLink.Visible = False
                End If
                If Pager.DeltaFirst + val <= 0 Then
                    oHyperLink.Visible = False
                End If

                If GoToPage = Pager.PageIndex Then
                    '	oLinkPage.Text = "[" + oLinkPage.Text + "]"
                    oHyperLink.CssClass = "PagerSpan"
                    oHyperLink.ToolTip = String.Format(Me.Resource.getValue("ActualPage"), oHyperLink.Text)
                Else
                    oHyperLink.CssClass = "PagerLink"
                    oHyperLink.ToolTip = String.Format(Me.Resource.getValue("GoToPage"), oHyperLink.Text)
                End If
            End If
        Next
    End Sub


	Private Sub Selected()
		For Each ctr As UI.Control In spanbutton.Controls
			Dim lnb As UI.WebControls.LinkButton
			lnb = DirectCast(ctr, UI.WebControls.LinkButton)
			If lnb IsNot Nothing Then
				Dim val As Integer = CInt(lnb.CommandArgument)
				If val = Pager.PageIndex Then
					lnb.Text = "[" + lnb.Text + "]"
					lnb.CssClass = "PagerSpan"
				Else
					lnb.CssClass = "PagerLink"
				End If
			End If
		Next
	End Sub

	Private Sub LNB1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNB1.Click, LNB2.Click, LNB3.Click, LNB4.Click, LNB5.Click, LNB6.Click, LNB7.Click, LNB8.Click, LNB9.Click
		Dim lnb As UI.WebControls.LinkButton = CType(sender, UI.WebControls.LinkButton)
		'Dim val As Integer = CInt(lnb.Text.Replace("[", "").Replace("]", ""))
		Dim val As Integer = CInt(lnb.CommandArgument)

		Pager.PageIndex = val
		RaiseEvent OnPageSelected()
	End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Try
            SetButtonNavigation()
            SetPagerLabels()
        Catch ex As Exception
        End Try
    End Sub

	Private Sub IMBfirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBfirst.Click
		Pager.GoFirst()
		RaiseEvent OnPageSelected()
	End Sub

	Private Sub IMBnext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBnext.Click
		Pager.GoNext()
		RaiseEvent OnPageSelected()
	End Sub

	Private Sub IMBlast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBlast.Click
		Pager.GoLast()
		RaiseEvent OnPageSelected()
	End Sub

	Private Sub IMBprev_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBprev.Click
		Pager.GoPrev()
		RaiseEvent OnPageSelected()
	End Sub

	Public Overrides ReadOnly Property AlwaysBind() As Boolean
		Get
			Return False
		End Get
	End Property
	Public Overrides Sub BindDati()

	End Sub
	Public Overrides Sub SetCultureSettings()
		MyBase.SetCulture("UC_PagerControl", "UC")
	End Sub

	Public Overrides Sub SetInternazionalizzazione()
		With Me.Resource
			.setImageButton(Me.IMBfirst, False, True, True)
			.setImageButton(Me.IMBlast, False, True, True)
			.setImageButton(Me.IMBnext, False, True, True)
			.setImageButton(Me.IMBprev, False, True, True)
		End With
	End Sub
End Class