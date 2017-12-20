Imports lm.Comol.Core.DomainModel.Languages
Imports lm.Comol.Core.BaseModules.CommonControls.Presentation

Public Class UC_ContentTranslationSelector
    Inherits BaseControl
    Implements IViewContentTranslatorSelector


#Region "Context"
    Private _Presenter As ContentTranslatorSelectorPresenter
    Private ReadOnly Property CurrentPresenter() As ContentTranslatorSelectorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ContentTranslatorSelectorPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IsInitialized As Boolean Implements IViewContentTranslatorSelector.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public Property RaiseSelectionEvent As Boolean Implements IViewContentTranslatorSelector.RaiseSelectionEvent
        Get
            Return ViewStateOrDefault("RaiseSelectionEvent", True)
        End Get
        Set(value As Boolean)
            ViewState("RaiseSelectionEvent") = value
        End Set
    End Property
    Private Property AvailableLanguages As List(Of BaseLanguageItem) Implements IViewContentTranslatorSelector.AvailableLanguages
        Get
            Return ViewStateOrDefault("AvailableLanguages", New List(Of BaseLanguageItem))
        End Get
        Set(value As List(Of BaseLanguageItem))
            ViewState("AvailableLanguages") = value
        End Set
    End Property
    Private Property InUseLanguages As List(Of LanguageItem) Implements IViewContentTranslatorSelector.InUseLanguages
        Get
            Return ViewStateOrDefault("InUseLanguages", New List(Of LanguageItem))
        End Get
        Set(value As List(Of LanguageItem))
            ViewState("InUseLanguages") = value
        End Set
    End Property
    Public Property SelectedItem As LanguageItem Implements IViewContentTranslatorSelector.SelectedItem
        Get
            Return ViewStateOrDefault("SelectedItem", New LanguageItem())
        End Get
        Set(value As LanguageItem)
            ViewState("SelectedItem") = value
        End Set
    End Property
    Private ReadOnly Property MultiName As String Implements IViewContentTranslatorSelector.MultiName
        Get
            Return Resource.getValue("MultiLanguage.Name")
        End Get
    End Property
    Private ReadOnly Property MultiToolTip As String Implements IViewContentTranslatorSelector.MultiToolTip
        Get
            Return Resource.getValue("MultiLanguage.ToolTip")
        End Get
    End Property
    Private Property FirstItemToLoad As BaseLanguageItem Implements IViewContentTranslatorSelector.FirstItemToLoad
        Get
            If IsNothing(ViewState("FirstItemToLoad")) Then
                Return Nothing
            Else
                Return DirectCast(ViewState("FirstItemToLoad"), BaseLanguageItem)
            End If
        End Get
        Set(value As BaseLanguageItem)
            ViewState("FirstItemToLoad") = value
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
    Public Property InEditing As Boolean
        Get
            Return ViewStateOrDefault("InEditing", True)
        End Get
        Set(value As Boolean)
            ViewState("InEditing") = value
        End Set
    End Property
    Private ReadOnly Property RenderItemCss(ByVal item As LanguageItem) As String
        Get
            Dim css As String = item.DisplayAs.ToString
            css = css.Replace(",", " ")
            If Not item.IsEnabled Then
                css &= " disabled"
            End If
            If item.IsSelected Then
                css &= " active"
            End If
            Return css
        End Get
    End Property
    Public Event LanguageAdded(ByVal l As BaseLanguageItem)
    Public Event SelectedLanguage(ByVal l As LanguageItem)
    Public Property LanguageTitleCssClass() As String
        Get
            Return LBusedLanguage_t.CssClass
        End Get
        Set(value As String)
            LBusedLanguage_t.CssClass = value
        End Set
    End Property


#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_ContentTranslation", "Modules", "Common")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBusedLanguage_t)
            .setLabel(LBlanguageAdd_t)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(availableLanguages As List(Of BaseLanguageItem)) Implements IViewContentTranslatorSelector.InitializeControl
        Me.CurrentPresenter.InitView(availableLanguages)
    End Sub
    Public Sub InitializeControl(availableLanguages As List(Of BaseLanguageItem), inUseItems As List(Of LanguageItem), current As LanguageItem) Implements IViewContentTranslatorSelector.InitializeControl
        Me.CurrentPresenter.InitView(availableLanguages, inUseItems, current)
    End Sub
    Public Sub InitializeControl(availableLanguages As List(Of lm.Comol.Core.DomainModel.Language), alsoMultilanguage As Boolean) Implements IViewContentTranslatorSelector.InitializeControl
        Me.CurrentPresenter.InitView(availableLanguages, alsoMultilanguage)
    End Sub
    Public Sub InitializeControl(availableLanguages As List(Of lm.Comol.Core.DomainModel.Language), inUseItems As List(Of Integer), Optional alsoMultilanguage As Boolean = False, Optional selectMultilanguage As Boolean = False, Optional idSelected As Integer = -1) Implements IViewContentTranslatorSelector.InitializeControl
        Me.CurrentPresenter.InitView(availableLanguages, inUseItems, alsoMultilanguage, selectMultilanguage, idSelected)
    End Sub
    Public Sub InitializeControl(availableLanguages As List(Of lm.Comol.Core.DomainModel.Language), inUseItems As Generic.List(Of String), Optional alsoMultilanguage As Boolean = False, Optional selectMultilanguage As Boolean = False, Optional selectedCode As String = "") Implements IViewContentTranslatorSelector.InitializeControl
        Me.CurrentPresenter.InitView(availableLanguages, inUseItems, alsoMultilanguage, selectMultilanguage, selectedCode)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewContentTranslatorSelector.DisplaySessionTimeout
        MLVselector.SetActiveView(VIWempty)
    End Sub

    Private Sub LoadItems(itemsToAdd As List(Of BaseLanguageItem), Optional inUseItems As List(Of LanguageItem) = Nothing, Optional selected As LanguageItem = Nothing) Implements IViewContentTranslatorSelector.LoadItems
        MLVselector.SetActiveView(VIWactive)

        SPNlanguageAdd.Visible = (itemsToAdd.Any()) AndAlso InEditing
        If (itemsToAdd.Any()) Then
            If itemsToAdd.Count > 1 Then
                If Not (DVaddLanguage.Attributes("class").Contains("enabled")) Then
                    DVaddLanguage.Attributes("class") &= " enabled"
                End If
            Else
                DVaddLanguage.Attributes("class") = Replace(DVaddLanguage.Attributes("class"), "enabled", "")
            End If
            Me.RPTlanguages.DataSource = itemsToAdd
            Me.RPTlanguages.DataBind()
        End If
        If Not IsNothing(inUseItems) AndAlso inUseItems.Any() Then
            RPTtranslations.Visible = True
            Me.RPTtranslations.DataSource = inUseItems
            Me.RPTtranslations.DataBind()

            If inUseItems.Count = 1 AndAlso RPTtranslations.Items.Count > 0 Then
                Dim oLinkbutton As LinkButton = RPTtranslations.Items(0).FindControl("LNBtranslation")
                oLinkbutton.Enabled = False
            End If
        Else
            RPTtranslations.Visible = False
        End If
    End Sub
    Private Sub LoadItems(inUseItems As List(Of LanguageItem)) Implements IViewContentTranslatorSelector.LoadItems
        If Not IsNothing(inUseItems) AndAlso inUseItems.Any() Then
            RPTtranslations.Visible = True
            Me.RPTtranslations.DataSource = inUseItems
            Me.RPTtranslations.DataBind()
        Else
            RPTtranslations.Visible = False
        End If
    End Sub
    Private Function GetMultiLanguageItem() As BaseLanguageItem Implements IViewContentTranslatorSelector.GetMultiLanguageItem
        Dim item As New BaseLanguageItem
        item.Id = 0
        item.Name = Resource.getValue("MultiLanguage.Name")
        item.Code = "multi"
        item.IsDefault = False
        item.IsMultiLanguage = True
        item.ToolTip = Resource.getValue("MultiLanguage.ToolTip")
        Return item
    End Function
    Public Function RemoveCurrent() As LanguageItem Implements IViewContentTranslatorSelector.RemoveCurrent
        Return Me.CurrentPresenter.RemoveCurrent(SelectedItem())
    End Function
#End Region

    Private Sub RPTlanguages_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTlanguages.ItemDataBound
        Dim item As BaseLanguageItem = DirectCast(e.Item.DataItem, BaseLanguageItem)
        Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBlanguage")

        oLinkbutton.CommandName = item.Id
        oLinkbutton.CommandArgument = item.Code
        oLinkbutton.Text = String.Format(LTaddLanguage.Text, item.ShortCode, item.Name)
        oLinkbutton.ToolTip = item.ToolTip

        If item.IsSelected Then
            oLinkbutton.CssClass &= " active"
        End If
    End Sub
    Private Sub RPTlanguages_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTlanguages.ItemCommand
        Dim item As BaseLanguageItem = CurrentPresenter.AddLanguage(CInt(e.CommandName), e.CommandArgument)

        If Not IsNothing(item) AndAlso RaiseSelectionEvent Then
            RaiseEvent LanguageAdded(item)
        End If
    End Sub

    Private Sub RPTtranslations_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTtranslations.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As LanguageItem = DirectCast(e.Item.DataItem, LanguageItem)
            Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBtranslation")

            oLinkbutton.CommandName = item.Id
            oLinkbutton.CommandArgument = item.Code
            Dim css As String = ""
            Select Case item.Status
                Case ItemStatus.valid
                    css = "green"
                Case ItemStatus.warning
                    css = "yellow"
                Case ItemStatus.wrong
                    css = "red"
            End Select
            oLinkbutton.CommandName = item.Id
            oLinkbutton.CommandArgument = item.Code
            oLinkbutton.Text = String.Format(LTtranslation.Text, css, item.ShortCode)
            oLinkbutton.ToolTip = item.ToolTip
            oLinkbutton.CssClass &= " " & RenderItemCss(item)
        End If

    End Sub
    Private Sub RPTtranslations_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTtranslations.ItemCommand
        Dim item As LanguageItem = CurrentPresenter.SelectLanguage(CInt(e.CommandName), e.CommandArgument)

        If Not IsNothing(item) AndAlso RaiseSelectionEvent Then
            RaiseEvent SelectedLanguage(item)
        End If
    End Sub
End Class