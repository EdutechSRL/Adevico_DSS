Imports COL_BusinessLogic_v2.UCServices

Public Class WebMeetingNotificationUtility
    Inherits BaseNotificationUtility

    Public ReadOnly Property BaseServiceUrl(ByVal RoomID As System.Guid) As String
        Get
            Return "WebMeeting/WMEnter.aspx?RoomId=" & RoomID.ToString
        End Get
    End Property
    Public ReadOnly Property BaseServiceUrlOM(ByVal RoomID As Integer) As String
        Get
            Return "WebMeeting/EnterRoom.aspx?RoomID=" & RoomID.ToString
        End Get
    End Property
    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub

    Public Sub NotifyAddMeeting(ByVal CommunityID As Integer, ByVal RoomID As System.Guid, ByVal RoomName As String, ByVal oStart As DateTime, ByVal oDurationHour As Integer, ByVal oDurationMinutes As Integer)
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrl(RoomID)))
        oValues.Add(RoomName)
        oValues.Add(oDurationHour)
        oValues.Add(oDurationMinutes)
        oValues.Add(FormatDateTime(oStart, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(oStart, DateFormat.ShortDate))

        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_DimDim.ActionType.AddTemporaryRoom, CommunityID, Services_DimDim.Codex, oValues)
    End Sub
    Public Sub NotifyAddOpenMeeting(ByVal CommunityID As Integer, ByVal RoomID As Integer, ByVal RoomName As String, ByVal oStart As DateTime, ByVal oDurationHour As Integer, ByVal oDurationMinutes As Integer)
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrlOM(RoomID)))
        oValues.Add(RoomName)
        oValues.Add(oDurationHour)
        oValues.Add(oDurationMinutes)
        oValues.Add(FormatDateTime(oStart, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(oStart, DateFormat.ShortDate))

        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_DimDim.ActionType.AddTemporaryRoom, CommunityID, Services_DimDim.Codex, oValues)
    End Sub
    Public Sub NotifyAddOpenMeeting(ByVal CommunityID As Integer, ByVal RoomID As Integer, ByVal RoomName As String)
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, BaseServiceUrlOM(RoomID)))
        oValues.Add(RoomName)

        _Utility.SendNotificationToPermission(Me.PermissionToSee, Services_DimDim.ActionType.AddPermanentRoom, CommunityID, Services_DimDim.Codex, oValues)
    End Sub

#Region "Object To Notify"
    'Private Function CreateQuestion(ByVal QuestionID As Integer) As dtoObjectToNotify
    '    Return CreateObjectToNotify(QuestionID, Services_Questionario.ObjectType.Domanda)
    'End Function
    'Private Function CreateQuestionario(ByVal QuestionarioID As Integer) As dtoObjectToNotify
    '    Return CreateObjectToNotify(QuestionarioID, Services_Questionario.ObjectType.Questionario)
    'End Function
    'Private Function CreateLibrary(ByVal LibraryID As Integer) As dtoObjectToNotify
    '    Return CreateObjectToNotify(LibraryID, Services_Questionario.ObjectType.Libreria)
    'End Function
    'Private Function CreateObjectToNotify(ByVal ObjectID As Integer, ByVal oType As Services_DimDim.ObjectType) As dtoObjectToNotify
    '    Dim obj As New dtoObjectToNotify
    '    obj.ObjectID = ObjectID.ToString
    '    obj.ObjectTypeID = oType
    '    Select Case oType
    '        Case Services_DimDim.ObjectType.Room
    '            obj.FullyQualiFiedNameField = GetType(COL_Questionario.Questionario).FullName
    '        Case Services_DimDim.ObjectType.Reservation
    '            obj.FullyQualiFiedNameField = GetType(DimDimSettings).FullName
    '        Case Services_DimDim.ObjectType.Person
    '            obj.FullyQualiFiedNameField = GetType(COL_Persona).FullName
    '    End Select
    '    Return obj
    'End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionToSee() As Integer
        Return Services_DimDim.Base2Permission.ViewRooms Or Services_DimDim.Base2Permission.AdminService
    End Function
    Public Function PermissionToAdmin() As Integer
        Return Services_DimDim.Base2Permission.AdminService
    End Function
#End Region
End Class