Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports NHibernate
Imports NHibernate.Linq

Namespace lm.Comol.Modules.Base.Presentation
    Public Class NoticeBoardEditPresenter
        Inherits DomainPresenter

#Region "Standard"
        Private _CommonManager As ManagerCommon
        Private _ModuleID As Integer
        Private ReadOnly Property ModuleID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.CommonManager.GetModuleID(COL_BusinessLogic_v2.UCServices.Services_Bacheca.Codex)
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
        Public Overloads Property CurrentManager() As ManagerNoticeBoard
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerNoticeBoard)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewNoticeBoardEdit
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerNoticeBoard(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewNoticeBoardEdit)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerNoticeBoard(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView(ByVal AdvancedEditor As Boolean)
            Dim CommunityID As Integer = Me.View.PreLoadedCommunityID
            If CommunityID = -1 Then
                CommunityID = Me.UserContext.CurrentCommunityID
            End If
            If Not Me.UserContext.isAnonymous Then
                Dim MessageID As Long = Me.View.PreLoadedMessageID
                Dim oPermission As ModuleNoticeBoard = New ModuleNoticeBoard
                Dim oNoticeboard As NoticeBoard = Me.CurrentManager.GetMessage(MessageID)
                Dim Name As String = ""

                If oNoticeboard Is Nothing AndAlso MessageID > 0 Then
                    MessageID = 0
                    Me.View.NoMessageWithThisID(CommunityID, ModuleID, MessageID)
                Else
                    If MessageID > 0 Then
                        If oNoticeboard.CommunityOwner Is Nothing AndAlso oNoticeboard.isForPortal Then
                            CommunityID = 0
                        ElseIf Not IsNothing(oNoticeboard.CommunityOwner) Then
                            CommunityID = oNoticeboard.CommunityOwner.Id
                        Else
                            CommunityID = -1
                        End If
                        If Not oNoticeboard.CreateByAdvancedEditor AndAlso Not AdvancedEditor Then
                            Me.View.MessageStyle = oNoticeboard.Style
                        End If
                    End If
                End If
                oPermission = Me.View.NoticeboardPermission(CommunityID)
                If oPermission.ServiceAdministration OrElse oPermission.EditMessage Then
                    If MessageID = 0 Then
                        Me.View.MessageText = ""
                    Else
                        Me.View.MessageText = oNoticeboard.Message
                    End If
                Else
                    Me.View.SendNoPermissionAction(CommunityID, ModuleID, MessageID)
                End If
                Dim oCommunity As Community = Me.CommonManager.GetCommunity(CommunityID)
                If CommunityID > 0 AndAlso Not oCommunity Is Nothing Then
                    Name = oCommunity.Name
                ElseIf CommunityID = 0 Then
                    Name = Me.View.PortalName
                End If
                Me.View.NoticeboardCommunityID = CommunityID
                Me.View.setHeaderTitle(Name, MessageID > 0, CommunityID = 0)
                Me.View.SetPreviousUrl(Me.View.PreLoadedContainer, Me.View.PreLoadedFromPage, Me.View.PreLoadedMessagesToShow, MessageID, CommunityID, Me.View.PreLoadedPage)
            Else
                Me.View.SendNoPermissionAction(CommunityID, ModuleID, 0)
            End If
        End Sub

        Public Sub Save(ByVal AdvancedEditor As Boolean)
            '    Dim oContext As NoticeBoardContext = Me.ContexFromView.Clone
            '    Dim RegisterAction As Boolean = True
            '    If oContext.PreviousView <> NoticeBoardContext.ViewModeType.None Then
            '        oContext.ViewMode = oContext.PreviousView
            '    Else
            '        oContext.ViewMode = NoticeBoardContext.ViewModeType.CurrentMessage
            '    End If
            '    If IsNothing(oNoticeboard) Then
            '        Me.View.CurrentMessageID = oContext.MessageID
            '        Me.LoadMessage(oContext, RegisterAction)
            '    Else
            '        Dim ToSave As NoticeBoard = Me.DataContext.GetById(Of NoticeBoard)(oNoticeboard.Id)
            '        Try
            '            DataContext.BeginTransaction()
            '            If Not IsNothing(ToSave) AndAlso ToSave.Message <> oNoticeboard.Message Then
            '                ToSave = New NoticeBoard
            '            End If
            '            If IsNothing(ToSave) OrElse ToSave.Id = 0 Then
            '                ToSave = New NoticeBoard
            '                Dim oMetadata As New MetaData
            '                oMetadata.canDelete = True
            '                oMetadata.canModify = True
            '                oMetadata.CreatedBy = Me.DataContext.GetById(Of Person)(Me.UserContext.CurrentUserID)
            '                oMetadata.CreatedOn = Now
            '                oMetadata.isDeleted = False
            '                oMetadata.ModifiedBy = oMetadata.CreatedBy
            '                oMetadata.ModifiedOn = oMetadata.CreatedOn
            '                oMetadata.DeletedOn = DateTime.MaxValue
            '                oMetadata.ApprovedOn = DateTime.MaxValue
            '                Me.DataContext.SaveOrUpdate(oMetadata)
            '                ToSave.Owner = oMetadata.CreatedBy
            '                ToSave.MetaInfo = oMetadata
            '            Else
            '                ToSave.MetaInfo.ModifiedBy = Me.DataContext.GetById(Of Person)(Me.UserContext.CurrentUserID)
            '                ToSave.MetaInfo.ModifiedOn = Now
            '                Me.DataContext.SaveOrUpdate(ToSave.MetaInfo)
            '            End If
            '            ToSave.CommunityOwner = Me.DataContext.GetById(Of Community)(Me.UserContext.CurrentCommunityID)
            '            ToSave.CreateByAdvancedEditor = oNoticeboard.CreateByAdvancedEditor
            '            If ToSave.Id = 0 Then
            '                ToSave.isForPortal = (Me.UserContext.CurrentCommunityID = 0)
            '                If Not IsNothing(oNoticeboard.Style) Then
            '                    Dim oStyle As New TextStyle
            '                    oStyle = oNoticeboard.Style
            '                    Me.DataContext.SaveOrUpdate(oStyle)
            '                    ToSave.Style = oStyle
            '                End If
            '            Else
            '                If ToSave.CreateByAdvancedEditor And Not IsNothing(ToSave.Style) Then
            '                    Me.DataContext.Delete(ToSave.Style)
            '                    ToSave.Style = Nothing
            '                ElseIf ToSave.CreateByAdvancedEditor = False Then
            '                    Dim oStyle As New TextStyle
            '                    If IsNothing(ToSave.Style) Then
            '                        oStyle = oNoticeboard.Style
            '                        Me.DataContext.SaveOrUpdate(oStyle)
            '                        ToSave.Style = oStyle
            '                    Else
            '                        ToSave.Style.Align = oNoticeboard.Style.Align
            '                        ToSave.Style.BackGround = oNoticeboard.Style.BackGround
            '                        ToSave.Style.Color = oNoticeboard.Style.Color
            '                        ToSave.Style.Face = oNoticeboard.Style.Face
            '                        ToSave.Style.Size = oNoticeboard.Style.Size
            '                        Me.DataContext.SaveOrUpdate(ToSave.Style)
            '                    End If
            '                End If
            '            End If
            '            ToSave.Message = oNoticeboard.Message
            '            If HasPermissionToEdit(ToSave.CommunityOwner) Then
            '                Me.DataContext.SaveOrUpdate(ToSave)
            '                DataContext.Commit()
            '                Me.View.CurrentMessageID = ToSave.Id
            '                oContext.MessageID = ToSave.Id

            '                RegisterAction = False
            '                Dim Values As List(Of String) = Me.GetNotificationValues(ToSave, NoticeBoardContext.ViewModeType.Message, NoticeBoardContext.SmallViewType.LastFourMessage)

            '                If oNoticeboard.Id = 0 Then
            '                    If ToSave.CommunityOwner Is Nothing Then
            '                        Me.View.AddCreateAction(ToSave.Id, 0, Values)
            '                    Else
            '                        Me.View.AddCreateAction(ToSave.Id, ToSave.CommunityOwner.Id, Values)
            '                    End If
            '                Else
            '                    If ToSave.CommunityOwner Is Nothing Then
            '                        Me.View.AddEditAction(ToSave.Id, 0, Values)
            '                    Else
            '                        Me.View.AddEditAction(ToSave.Id, ToSave.CommunityOwner.Id, Values)
            '                    End If
            '                End If

            '            Else
            '                DataContext.Rollback()
            '            End If
            '        Catch ex As Exception
            '            DataContext.Rollback()
            '        End Try
            '    End If
        End Sub
    End Class
End Namespace