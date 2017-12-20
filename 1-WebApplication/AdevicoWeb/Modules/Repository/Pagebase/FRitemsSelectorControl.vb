Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.FileRepository.Domain
Imports lm.Comol.Core.BaseModules.FileRepository.Presentation
Public MustInherit Class FRitemsSelectorControl
    Inherits FRbaseControl
    Implements IViewItemSelectorsBase

#Region "Implements"
    Public Property AllowSelectAll As Boolean Implements IViewItemSelectorsBase.AllowSelectAll
        Get
            Return ViewStateOrDefault("AllowSelectAll", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSelectAll") = value
        End Set
    End Property
    Protected Friend Property AvailableTypes As List(Of ItemType) Implements IViewItemSelectorsBase.AvailableTypes
        Get
            Return ViewStateOrDefault("AvailableTypes", New List(Of ItemType))
        End Get
        Set(value As List(Of ItemType))
            ViewState("AvailableTypes") = value
        End Set
    End Property
    Protected Friend Property CurrentAvailability As ItemAvailability Implements IViewItemSelectorsBase.CurrentAvailability
        Get
            Return ViewStateOrDefault("CurrentAvailability", ItemAvailability.available)
        End Get
        Set(value As ItemAvailability)
            ViewState("CurrentAvailability") = value
        End Set
    End Property
    Protected Friend Property AllSelected As Boolean Implements IViewItemSelectorsBase.AllSelected
        Get
            Return ViewStateOrDefault("AllSelected", False)
        End Get
        Set(value As Boolean)
            ViewState("AllSelected") = value
        End Set
    End Property

    Protected Friend Property IdRemovedItems As List(Of Long) Implements IViewItemSelectorsBase.IdRemovedItems
        Get
            Return ViewStateOrDefault("IdRemovedItems", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            ViewState("IdRemovedItems") = value
        End Set
    End Property
    Protected Friend Property IdSelectedItems As List(Of Long) Implements IViewItemSelectorsBase.IdSelectedItems
        Get
            Return ViewStateOrDefault("IdSelectedItems", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            ViewState("IdSelectedItems") = value
        End Set
    End Property


    Private Property IdUserLoader As Integer Implements IViewItemSelectorsBase.IdUserLoader
        Get
            Return ViewStateOrDefault("IdUserLoader", 0)
        End Get
        Set(value As Integer)
            ViewState("IdUserLoader") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewItemSelectorsBase.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Protected Friend Property LoadForModule As Boolean Implements IViewItemSelectorsBase.LoadForModule
        Get
            Return ViewStateOrDefault("LoadForModule", False)
        End Get
        Set(value As Boolean)
            ViewState("LoadForModule") = value
        End Set
    End Property
    Private Property ModuleCode As String Implements IViewItemSelectorsBase.ModuleCode
        Get
            Return ViewStateOrDefault("ModuleCode", "")
        End Get
        Set(value As String)
            ViewState("ModuleCode") = value
        End Set
    End Property
    Protected Friend Property RepositoryIdentifier As RepositoryIdentifier Implements IViewItemSelectorsBase.RepositoryIdentifier
        Get
            Return ViewStateOrDefault("RepositoryIdentifier", New RepositoryIdentifier())
        End Get
        Set(value As RepositoryIdentifier)
            ViewState("RepositoryIdentifier") = value
        End Set
    End Property
    Protected Friend Property DisplayStatistics As List(Of StatisticType) Implements IViewItemSelectorsBase.DisplayStatistics
        Get
            Return ViewStateOrDefault("DisplayStatistics", New List(Of StatisticType))
        End Get
        Set(value As List(Of StatisticType))
            ViewState("DisplayStatistics") = value
        End Set
    End Property
    Protected Friend Property CurrentAdminMode As Boolean Implements IViewItemSelectorsBase.CurrentAdminMode
        Get
            Return ViewStateOrDefault("CurrentAdminMode", False)
        End Get
        Set(value As Boolean)
            ViewState("CurrentAdminMode") = value
        End Set
    End Property
    Protected Friend Property CurrentDisableNotAvailableItems As Boolean Implements IViewItemSelectorsBase.CurrentDisableNotAvailableItems
        Get
            Return ViewStateOrDefault("CurrentDisableNotAvailableItems", True)
        End Get
        Set(value As Boolean)
            ViewState("CurrentDisableNotAvailableItems") = value
        End Set
    End Property
    Protected Friend Property CurrentShowHiddenItems As Boolean Implements IViewItemSelectorsBase.CurrentShowHiddenItems
        Get
            Return ViewStateOrDefault("CurrentShowHiddenItems", False)
        End Get
        Set(value As Boolean)
            ViewState("CurrentShowHiddenItems") = value
        End Set
    End Property
    Public ReadOnly Property HasAvailableItems As Boolean Implements IViewItemSelectorsBase.HasAvailableItems
        Get
            Return HasAvailableItemsToSelect
        End Get
    End Property

#End Region


#Region "Implements"
    Public Sub InitializeControl(idUser As Integer, identifier As RepositoryIdentifier, adminMode As Boolean, showHiddenItems As Boolean, disableNotAvailableItems As Boolean, typesToLoad As List(Of ItemType), availability As ItemAvailability, displayStatistics As List(Of StatisticType), idRemovedItems As List(Of Long), Optional idSelectedItems As List(Of Long) = Nothing, Optional orderBy As OrderBy = lm.Comol.Core.FileRepository.Domain.OrderBy.name, Optional ascending As Boolean = True) Implements IViewItemSelectorsBase.InitializeControl
        InternalInitialize(idUser, identifier, adminMode, showHiddenItems, disableNotAvailableItems, typesToLoad, availability, displayStatistics, idRemovedItems, idSelectedItems, orderBy, ascending)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewItemSelectorsBase.DisplaySessionTimeout
        SetSessionTimeout()
    End Sub
    Protected Friend Function GetCurrentSelection() As Dictionary(Of Boolean, List(Of Long)) Implements IViewItemSelectorsBase.GetCurrentSelection
        Return GetCurrentItemsSelection()
    End Function
    Public Function GetSelectedItems() As List(Of dtoRepositoryItemToSelect) Implements IViewItemSelectorsBase.GetSelectedItems
        Return GetInternalSelectedItems()
    End Function
#End Region

#Region "Internal"
    Protected Friend MustOverride Function GetCurrentItemsSelection() As Dictionary(Of Boolean, List(Of Long))
    Protected Friend MustOverride Function HasAvailableItemsToSelect() As Boolean
    Protected Friend MustOverride Sub SetSessionTimeout()
    Protected Friend MustOverride Sub InternalInitialize(idUser As Integer, identifier As RepositoryIdentifier, adminMode As Boolean, showHiddenItems As Boolean, disableNotAvailableItems As Boolean, typesToLoad As List(Of ItemType), availability As ItemAvailability, displayStatistics As List(Of StatisticType), idRemovedItems As List(Of Long), idSelectedItems As List(Of Long), orderBy As OrderBy, ascending As Boolean)
    Protected Friend MustOverride Function GetInternalSelectedItems() As List(Of dtoRepositoryItemToSelect)

#End Region

End Class