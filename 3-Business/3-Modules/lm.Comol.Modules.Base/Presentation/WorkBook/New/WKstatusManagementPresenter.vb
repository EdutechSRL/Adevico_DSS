Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class WKstatusManagementPresenter
        Inherits DomainPresenter

        Public Overloads Property CurrentManager() As ManagerWorkBook
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerWorkBook)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IWKstatusManagement
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IWKstatusManagement)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Dim oPermission As ModuleWorkBookManagement = Me.View.ModulePermission

                If Not oPermission.ListStatus Then
                    Me.View.NoPermissionToAccessPage()
                Else
                    Me.View.AllowCreateStatus = oPermission.AddStatus OrElse oPermission.Administration
                    Me.LoadStatus()

                End If
            Else
                Me.View.NoPermissionToAccessPage()
            End If
        End Sub
        Public Sub LoadStatus()
            Dim oPager As New lm.Comol.Core.DomainModel.PagerBase

            If oPager.PageSize <> Me.View.CurrentPageSize Then
                oPager.PageSize = Me.View.CurrentPageSize
            End If
            oPager.Count = Me.CurrentManager.GetManagementWorkBookStatusCount
            oPager.Count -= 1
            oPager.PageIndex = Me.View.PreLoadedPageIndex

            Me.View.CurrentPager = oPager

            Dim oList As List(Of dtoWorkBookStatus) = Me.CurrentManager.GetManagementWorkBookStatusList(Me.UserContext.Language, oPager)

            If oList.Count = 0 Then
                Me.View.ShowNoStatusView()
                Me.View.DefaultStatus = ""
            Else
                Me.View.ShowListView()
                Me.View.LoadStatus(oList.OrderBy(Function(c) c.Name).ToList)
                Me.View.NavigationUrl()
                Dim oStatus As WorkBookStatus = Me.CurrentManager.GetDefaultWorkBookStatus
                If Not IsNothing(oStatus) Then
                    Me.View.DefaultStatus = Me.CurrentManager.GetTranslationWorkBookStatus(Me.UserContext.Language, oStatus.Id)
                Else
                    Me.View.DefaultStatus = ""
                End If
            End If
        End Sub

        Public Sub DeleteStatus(ByVal StatusID As Integer)
            Me.CurrentManager.DeleteWorkbookStatus(StatusID)
            Me.LoadStatus()
        End Sub
    End Class
End Namespace