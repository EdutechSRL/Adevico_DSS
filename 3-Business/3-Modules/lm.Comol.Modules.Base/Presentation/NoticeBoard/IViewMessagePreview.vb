Imports lm.Comol.Modules.Base.DomainModel

Public Interface IViewMessagePreview
    Property ContenitoreWidth() As System.Web.UI.WebControls.Unit
    Property ContenitoreBachecaWidth() As System.Web.UI.WebControls.Unit
    Property ContenitoreBachecaHeight() As System.Web.UI.WebControls.Unit
    Property isPreview() As Boolean
    ReadOnly Property iBackgroundTemplate() As String
    ReadOnly Property BachecaWidth() As String
    ReadOnly Property BachecaHeight() As String
    Sub InitController(ByVal oNoticeBoard As NoticeBoard, ByVal oPermission As ModuleNoticeBoard, ByVal isCurrent As Boolean, ByVal Container As NoticeBoardContext.ViewModeType, ByVal CommunityID As Integer, ByVal UserID As Integer)
End Interface