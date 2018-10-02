Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2
Imports System.Linq
Imports lm.Comol.Core.DataLayer.LinqExtension

Namespace lm.Comol.Modules.Base.Presentation
    Public Class ScormPresenter
        Inherits DomainPresenter

        Private _CommonManager As ManagerCommon
        Private Overloads Property CurrentManager() As ManagerCommunityFiles
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerCommunityFiles)
                _CurrentManager = value
            End Set
        End Property
        Private Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property

        Private Overloads ReadOnly Property View() As IviewScormFileSelector
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewScormFileSelector)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView(ByVal CommunityID As Integer, ByVal UserID As Integer, ByVal PreSelectID As List(Of Long))
            Dim ModuleID As Integer = Me.CommonManager.GetModuleID(UCServices.Services_File.Codex)
            Me.InitViewForModule(CommunityID, UserID, PreSelectID, ModuleID, CommunityID, GetType(Community).FullName)
        End Sub
        Public Sub InitViewForModule(ByVal CommunityID As Integer, ByVal UserID As Integer, ByVal PreSelectID As List(Of Long), ByVal ModuleID As Integer, ByVal ItemID As String, ByVal ItemType As String)
            Dim ModuleCode As String = Me.CommonManager.GetModuleCode(ModuleID)
            If ModuleID = 0 Then
                ModuleCode = UCServices.Services_File.Codex
                ModuleID = Me.CommonManager.GetModuleID(ModuleCode)
            End If
            Me.View.SelectorModuleID = ModuleID
            Me.View.SelectorModuleCode = ModuleCode
            Me.View.SelectorCommunityID = CommunityID
            Me.View.SelectorItemID = ItemID
            Me.View.SelectorItemTypeID = ItemType
            Me.LoadFiles(0, PreSelectID)
        End Sub
        Public Sub LoadFiles(ByVal PageIndex As Integer, ByVal PreSelectID As List(Of Long))
            Select Case Me.View.SelectorModuleCode
                Case UCServices.Services_File.Codex
                    LoadRepositoryScormFiles(Me.View.SelectorCommunityID, Me.UserContext.CurrentUserID, PageIndex, PreSelectID)
            End Select
        End Sub


        Private Sub LoadRepositoryScormFiles(ByVal CommunityID As Integer, ByVal UserID As Integer, ByVal PageIndex As Integer, ByVal PreSelectID As List(Of Long))
            Dim oList As New List(Of dtoFileScorm)
            Dim oPermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(CommunityID)
            Dim PageSize As Integer = Me.View.PageSize

            oList = Me.CurrentManager.GetRepositoryDTOscormFiles(CommunityID, oPermission.Administration, oPermission.Administration, UserID)

            Dim FilesID As List(Of Long) = (From f In oList Select f.Id).ToList
            Me.View.TemporaryFilesID = PreSelectID ' (From FileId In PreSelectID Where FilesID.Contains(FileId)).ToList
            'oList.OrderByDescending(Function(f) f.FullName)
            'If PageSize > 0 Then
            '    Me.View.LoadFiles(oList.Skip(PageSize * PageIndex).Take(PageSize).ToList)
            'Else
            Me.View.LoadFiles(oList)
            'End If

        End Sub
    End Class
End Namespace