Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.BaseModules.CommonControls

Public Class UC_AlphabetSelector
    Inherits BaseControl
    Implements IViewAlphabetSelector

#Region "Context"
    Private _Presenter As AlphabetSelectorPresenter
    Private ReadOnly Property CurrentPresenter() As AlphabetSelectorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AlphabetSelectorPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property DisplayMode As AlphabetDisplayMode Implements IViewAlphabetSelector.DisplayMode
        Get
            Return ViewStateOrDefault("DisplayMode", AlphabetDisplayMode.allCharsItem Or AlphabetDisplayMode.commonletters Or AlphabetDisplayMode.otherCharsItem Or AlphabetDisplayMode.addUnmatchLetters)
        End Get
        Set(value As AlphabetDisplayMode)
            ViewState("DisplayMode") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewAlphabetSelector.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property RaiseSelectionEvent As Boolean Implements IViewAlphabetSelector.RaiseSelectionEvent
        Get
            Return ViewStateOrDefault("RaiseSelectionEvent", True)
        End Get
        Set(value As Boolean)
            ViewState("RaiseSelectionEvent") = value
        End Set
    End Property
    Public Property AutoSelectLetter As Boolean Implements IViewAlphabetSelector.AutoSelectLetter
        Get
            Return ViewStateOrDefault("AutoSelectLetter", False)
        End Get
        Set(value As Boolean)
            ViewState("AutoSelectLetter") = value
        End Set
    End Property
    Public Property SelectedItem As String Implements IViewAlphabetSelector.SelectedItem
        Get
            Return ViewStateOrDefault("AlphabetItem", "")
        End Get
        Set(value As String)
            ViewState("AlphabetItem") = value
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

#Region "Property"
    Private ReadOnly Property RenderItemCss(ByVal item As AlphabetItem) As String
        Get
            Dim css As String = ""
            If item.DisplayAs <> lm.Comol.Core.DomainModel.Helpers.AlphabetItem.AlphabetItemDisplayAs.item Then
                css = item.DisplayAs.ToString
            End If
            If Not item.isEnabled Then
                css &= " disabled"
            End If
            If item.isSelected Then
                css &= " active"
            End If
            Return css
        End Get
    End Property
    Public Event SelectItem(ByVal letter As String)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfilesManagement", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource

        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(availableWords As List(Of String)) Implements IViewAlphabetSelector.InitializeControl
        CurrentPresenter.InitView(availableWords, "-1")
    End Sub
    Public Sub InitializeControl(availableWords As List(Of String), displayMode As AlphabetDisplayMode) Implements IViewAlphabetSelector.InitializeControl
        CurrentPresenter.InitView(availableWords, "-1", displayMode)
    End Sub
    Public Sub InitializeControl(availableWords As List(Of String), activeWord As String) Implements IViewAlphabetSelector.InitializeControl
        CurrentPresenter.InitView(availableWords, activeWord)
    End Sub
    Public Sub InitializeControl(availableWords As List(Of String), activeWord As String, displayMode As AlphabetDisplayMode) Implements IViewAlphabetSelector.InitializeControl
        CurrentPresenter.InitView(availableWords, activeWord, displayMode)
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewAlphabetSelector.DisplaySessionTimeout
        MLVselector.SetActiveView(VIWempty)
    End Sub
    Private Sub LoadItems(alphabet As List(Of AlphabetItem)) Implements IViewAlphabetSelector.LoadItems
        MLVselector.SetActiveView(VIWactive)
        Me.RPTletters.DataSource = alphabet
        Me.RPTletters.DataBind()
    End Sub
#End Region

    Private Sub RPTletters_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTletters.ItemDataBound
        Dim item As lm.Comol.Core.DomainModel.Helpers.AlphabetItem = DirectCast(e.Item.DataItem, lm.Comol.Core.DomainModel.Helpers.AlphabetItem)
        Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBletter")
        Select Case item.Type
            Case AlphabetItemType.all
                oLinkbutton.Text = Resource.getValue("AlphabetItem.All")
            Case AlphabetItemType.letter
                oLinkbutton.Text = item.DisplayName
            Case AlphabetItemType.otherChars
                oLinkbutton.Text = Resource.getValue("AlphabetItem.Other")
        End Select
        oLinkbutton.CommandArgument = item.Value
        oLinkbutton.CssClass &= " " & RenderItemCss(item)
        oLinkbutton.Enabled = item.isEnabled
    End Sub
    Private Sub RPTletters_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTletters.ItemCommand
        If AutoSelectLetter Then
            For Each item As RepeaterItem In RPTletters.Items
                Dim oLinkButton As LinkButton = item.FindControl("LNBletter")
                oLinkButton.CssClass = Replace(oLinkButton.CssClass, "active", "")
                If (e.CommandArgument = oLinkButton.CommandArgument) Then
                    oLinkButton.CssClass &= " active"
                End If

            Next
        End If
        If RaiseSelectionEvent Then
            RaiseEvent SelectItem(e.CommandArgument)
        End If
    End Sub

End Class