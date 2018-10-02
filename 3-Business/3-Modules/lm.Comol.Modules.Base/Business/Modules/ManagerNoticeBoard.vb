Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.DomainModel

Imports NHibernate
Imports NHibernate.Linq

Namespace lm.Comol.Modules.Base.BusinessLogic
    Public Class ManagerNoticeBoard
        Implements lm.Comol.Core.DomainModel.Common.iDomainManager

#Region "Private property"
        Private _UserContext As iUserContext
        Private _Datacontext As iDataContext
#End Region

#Region "Public property"
        Private ReadOnly Property DC() As iDataContext
            Get
                Return _Datacontext
            End Get
        End Property
        Private ReadOnly Property CurrentUserContext() As iUserContext
            Get
                Return _UserContext
            End Get
        End Property
#End Region

        Public Sub New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            Me._UserContext = oContext.UserContext
            Me._Datacontext = oContext.DataContext
        End Sub
        Public Sub New(ByVal oUserContext As iUserContext, ByVal oDatacontext As iDataContext)
            Me._UserContext = oUserContext
            Me._Datacontext = oDatacontext
        End Sub

        Public Function GetMessage(ByVal MessageID As Long) As NoticeBoard
            Dim oNoticeBoard As NoticeBoard = Nothing
            Try
                DC.BeginTransaction()
                oNoticeBoard = _Datacontext.GetById(Of NoticeBoard)(MessageID)
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try
            Return oNoticeBoard
        End Function

        Public Function SetActive(ByVal MessageID As Long, ByVal UserId As Integer) As NoticeBoard
            Dim oNoticeBoard As NoticeBoard = Nothing

            Try
                Dim oUser As Person = Me.DC.GetById(Of Person)(UserId)
                Dim ToSetActive As NoticeBoard = Me.GetMessage(MessageID)
                If Not IsNothing(ToSetActive) AndAlso Not IsNothing(oUser) Then
                    DC.BeginTransaction()
                    oNoticeBoard = New NoticeBoard
                    oNoticeBoard.CreatedOn = Now
                    oNoticeBoard.CreatedBy = oUser
                    oNoticeBoard.isDeleted = False
                    oNoticeBoard.ModifiedOn = oNoticeBoard.CreatedOn
                    oNoticeBoard.ModifiedBy = oUser
                    oNoticeBoard.CommunityOwner = ToSetActive.CommunityOwner
                    oNoticeBoard.CreateByAdvancedEditor = ToSetActive.CreateByAdvancedEditor
                    oNoticeBoard.isForPortal = ToSetActive.isForPortal
                    oNoticeBoard.Message = ToSetActive.Message
                    oNoticeBoard.Owner = oUser
                    If ToSetActive.CreateByAdvancedEditor = False Then
                        Dim oStyle As New TextStyle

                        oStyle.Align = ToSetActive.Style.Align
                        oStyle.BackGround = ToSetActive.Style.BackGround
                        oStyle.Color = ToSetActive.Style.Color
                        oStyle.Face = ToSetActive.Style.Face
                        oStyle.Size = ToSetActive.Style.Size
                        Me.DC.SaveOrUpdate(oStyle)
                        oNoticeBoard.Style = oStyle
                    End If
                    DC.SaveOrUpdate(oNoticeBoard)
                    DC.Commit()
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oNoticeBoard = Nothing
            End Try
            Return oNoticeBoard
        End Function

        Public Function ClearNoticeBoard(ByVal CommunityID As Integer, ByVal UserID As Integer, ByVal isForPortal As Boolean) As NoticeBoard
            Dim oNoticeBoard As NoticeBoard = Nothing

            Try
                Dim oUser As Person = Me.DC.GetById(Of Person)(UserID)
                Dim oCommunity As Community = Me.DC.GetById(Of Community)(CommunityID)
                If Not IsNothing(oUser) AndAlso Not (IsNothing(oCommunity) AndAlso CommunityID > 0) Then
                    DC.BeginTransaction()
                    oNoticeBoard = New NoticeBoard
                    oNoticeBoard.CreatedOn = Now
                    oNoticeBoard.CreatedBy = oUser
                    oNoticeBoard.isDeleted = False
                    oNoticeBoard.ModifiedOn = oNoticeBoard.CreatedOn
                    oNoticeBoard.ModifiedBy = oUser
                    oNoticeBoard.CommunityOwner = oCommunity
                    oNoticeBoard.CreateByAdvancedEditor = False
                    oNoticeBoard.isForPortal = isForPortal
                    oNoticeBoard.Message = ""
                    oNoticeBoard.Owner = oUser
                    DC.SaveOrUpdate(oNoticeBoard)
                    DC.Commit()
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oNoticeBoard = Nothing
            End Try
            Return oNoticeBoard
        End Function


        Public Function DeleteMessage(ByVal MessageID As Long) As Boolean
            Dim oNoticeBoard As NoticeBoard = Nothing
            Dim iResponse As Boolean = False
            Try
                oNoticeBoard = Me.GetMessage(MessageID)
                If Not IsNothing(oNoticeBoard) Then
                    DC.BeginTransaction()
                    DC.Delete(oNoticeBoard)
                    DC.Commit()
                    iResponse = True
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function UnDeleteMessage(ByVal MessageID As Long, ByVal UserID As Integer, ByVal SetActive As Boolean) As NoticeBoard
            Dim oNoticeBoard As NoticeBoard = Nothing
            Try
                oNoticeBoard = Me.GetMessage(MessageID)
                Dim oUser As Person = Me.DC.GetById(Of Person)(UserID)
                If Not IsNothing(oNoticeBoard) AndAlso Not IsNothing(oUser) Then
                    DC.BeginTransaction()
                    oNoticeBoard.isDeleted = True
                    If SetActive Then
                        oNoticeBoard.ModifiedOn = Now
                        oNoticeBoard.ModifiedBy = oUser
                    End If
                    DC.SaveOrUpdate(oNoticeBoard)
                    DC.Commit()
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oNoticeBoard = Nothing
            End Try
            Return oNoticeBoard
        End Function
        Public Function VirtualDeleteMessage(ByVal MessageID As Long, ByVal UserID As Integer) As NoticeBoard
            Dim oNoticeBoard As NoticeBoard = Nothing
            Try
                oNoticeBoard = Me.GetMessage(MessageID)
                Dim oUser As Person = Me.DC.GetById(Of Person)(UserID)
                If Not IsNothing(oNoticeBoard) AndAlso Not IsNothing(oUser) Then
                    DC.BeginTransaction()
                    oNoticeBoard.isDeleted = True
                    oNoticeBoard.ModifiedOn = Now
                    oNoticeBoard.ModifiedBy = oUser
                    DC.SaveOrUpdate(oNoticeBoard)
                    DC.Commit()
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                oNoticeBoard = Nothing
            End Try
            Return oNoticeBoard
        End Function


        Public Function GetLastMessage(ByVal CommunityID As Integer) As NoticeBoard
            Dim oNoticeBoard As NoticeBoard = Nothing
            Try
                Dim oCommunity As Community = DC.GetById(Of Community)(CommunityID)
                If CommunityID = 0 Then
                    oNoticeBoard = (From n In DC.GetCurrentSession.Linq(Of NoticeBoard)() Where n.isForPortal AndAlso n.isDeleted = False AndAlso n.CommunityOwner Is oCommunity Order By n.ModifiedOn Descending Select n).FirstOrDefault
                Else
                    oNoticeBoard = (From n In DC.GetCurrentSession.Linq(Of NoticeBoard)() Where n.isDeleted = False AndAlso n.CommunityOwner Is oCommunity Order By n.ModifiedOn Descending Select n).FirstOrDefault
                End If
            Catch ex As Exception
                oNoticeBoard = Nothing
            End Try
            Return oNoticeBoard
        End Function

        Public Function DeleteCommunityNoticeboard(ByVal CommunityID As Integer) As Boolean
            Dim iResponse As Boolean = False

            Try
                Dim oCommunity As Community = Me.DC.GetById(Of Community)(CommunityID)
                If CommunityID > 0 AndAlso oCommunity Is Nothing Then
                    iResponse = True
                Else
                    DC.BeginTransaction()
                    Dim oList As List(Of NoticeBoard) = (From n In DC.GetCurrentSession.Linq(Of NoticeBoard)() Where n.CommunityOwner Is oCommunity Select n).ToList
                    For Each oNoticeboard In oList
                        DC.Delete(oNoticeboard)
                    Next
                    DC.Commit()
                End If
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
            End Try

            Return iResponse
        End Function
    End Class
End Namespace