Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.NoticeBoard.Presentation
Imports lm.Comol.Core.BaseModules.NoticeBoard.Domain
Public MustInherit Class NBnoticeboardPage
    Inherits NoticeboardPageBase
    Implements IViewNoticeboardPage


#Region "Implements"

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewNoticeboardPage.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleNoticeboard.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
#End Region
    Protected Friend MustOverride Sub DisplaySessionTimeout() Implements IViewNoticeboardPage.DisplaySessionTimeout
  
#End Region

End Class