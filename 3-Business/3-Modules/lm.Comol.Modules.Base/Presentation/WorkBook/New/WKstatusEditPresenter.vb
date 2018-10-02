Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class WKstatusEditPresenter
        Inherits DomainPresenter

        Public Overloads Property CurrentManager() As ManagerWorkBook
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerWorkBook)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IWKstatusEdit
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IWKstatusEdit)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Dim oPermission As ModuleWorkBookManagement = Me.View.ModulePermission

                If Not oPermission.ListStatus Then
                    Me.View.NoPermissionToAccessPage()
                Else
                    Me.View.AllowSaveStatus = (Me.View.PreLoadedStatusID = 0 AndAlso oPermission.AddStatus) OrElse oPermission.Administration OrElse (Me.View.PreLoadedStatusID > 0 AndAlso oPermission.EditStatus)
                    Me.LoadStatus()

                End If
            Else
                Me.View.NoPermissionToAccessPage()
            End If
        End Sub

        Public Sub LoadStatus()
            Dim oStatus As WorkBookStatus = Me.CurrentManager.GetWorkBookStatus(Me.View.PreLoadedStatusID)
            If IsNothing(oStatus) And Me.View.PreLoadedStatusID > 0 Then
                Me.View.ShowNoStatusView()
            Else
                Me.View.ShowEditView()

                Dim oDto As New dtoWorkBookStatus
                If IsNothing(oStatus) Then
                    oDto.ID = 0
                    oDto.isDefault = False
                    oDto.ItemsCount = 0
                    oDto.WorkbookCount = 0
                    oDto.AvailableForPermission = EditingPermission.Responsible Or EditingPermission.Authors Or EditingPermission.ModuleManager Or EditingPermission.Owner
                Else
                    oDto.ID = oStatus.Id
                    oDto.isDefault = oStatus.IsDefault
                    oDto.AvailableForPermission = oStatus.AvailableFor
                    oDto.ItemsCount = Me.CurrentManager.GetItemCountByStatus(Me.View.PreLoadedStatusID)
                    oDto.WorkbookCount = Me.CurrentManager.GetWorkBookCountByStatus(Me.View.PreLoadedStatusID)
                End If
                Me.View.LoadStatus(oDto, Me.CurrentManager.LoadStatusTranslation(oStatus))
            End If
        End Sub

        Public Sub SaveStatus(ByVal oStatus As dtoWorkBookStatus, ByVal oList As List(Of dtoWorkBookStatusTranslation))
            If Not Me.CurrentManager.SaveStatus(Me.View.PreLoadedStatusID, oStatus, oList) Is Nothing Then
                Me.View.LoadStatusList()
            End If
        End Sub

    End Class
End Namespace