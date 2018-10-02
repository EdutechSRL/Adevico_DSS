Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic

Namespace lm.Comol.Modules.Base.Presentation
    Public Class ManagementCommunityPresenter
        Inherits DomainPresenter



        Private _ModuleID As Integer
        Private _CommonManager As ManagerCommon
        Private _ManagerCommunityFiles As ManagerCommunityFiles
        Private _ManagerWorkBook As ManagerWorkBook
        Private _ManagerNoticeBoard As ManagerNoticeBoard

        Private Property ManagerFiles() As ManagerCommunityFiles
            Get
                Return _ManagerCommunityFiles
            End Get
            Set(ByVal value As ManagerCommunityFiles)
                _ManagerCommunityFiles = value
            End Set
        End Property
        Private Property ManagerCommunityWorkBook() As ManagerWorkBook
            Get
                Return _ManagerWorkBook
            End Get
            Set(ByVal value As ManagerWorkBook)
                _ManagerWorkBook = value
            End Set
        End Property
        Private Property ManagerCommunityNoticeBoard() As ManagerNoticeBoard
            Get
                Return _ManagerNoticeBoard
            End Get
            Set(ByVal value As ManagerNoticeBoard)
                _ManagerNoticeBoard = value
            End Set
        End Property

        Private ReadOnly Property ModuleID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.CommonManager.GetModuleID(COL_BusinessLogic_v2.UCServices.Services_DiarioLezioni.Codex)
                End If
                Return _ModuleID
            End Get
        End Property
        Private Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property
        Public Overloads Property CurrentManager() As ManagerCommunityFiles
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerCommunityFiles)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewManagementCommunity
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            ' MyBase.CurrentManager = New ManagerCommunityDiary(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
            Me.ManagerFiles = New ManagerCommunityFiles(MyBase.AppContext)
            Me.ManagerCommunityWorkBook = New ManagerWorkBook(MyBase.AppContext)
            Me.ManagerCommunityNoticeBoard = New ManagerNoticeBoard(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewManagementCommunity)
            MyBase.New(oContext, view)
            '   MyBase.CurrentManager = New ManagerCommunityDiary(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
            Me.ManagerFiles = New ManagerCommunityFiles(MyBase.AppContext)
            Me.ManagerCommunityWorkBook = New ManagerWorkBook(MyBase.AppContext)
            Me.ManagerCommunityNoticeBoard = New ManagerNoticeBoard(MyBase.AppContext)
        End Sub

        Public Function RemoveCommunity(ByVal CommunityID As Integer, ByVal CommunityPath As String, ByVal BaseUserRepositoryPath As String) As Boolean
            Dim oCommunity As Community = Me.CommonManager.GetCommunity(CommunityID)

            Me.ManagerCommunityNoticeBoard.DeleteCommunityNoticeboard(CommunityID)
            Me.ManagerCommunityWorkBook.DeleteCommunityWorkBook(CommunityID, BaseUserRepositoryPath)
            Me.ManagerFiles.DeleteRepository(False, oCommunity, CommunityPath)
            Return True
        End Function
    End Class

End Namespace