Imports Comol.Entity.Configuration.Components

Namespace Configuration
    <Serializable(), CLSCompliant(True)> Public Class NotificationErrorSettings

#Region "Private properties"
        Private _Enabled As Boolean
        Private _ErrorsType As List(Of dtoErrorType)
        Private _ComolUniqueID As String
#End Region

#Region "Public properties"
        Public Property Enabled() As Boolean
            Get
                Enabled = _Enabled
            End Get
            Set(ByVal value As Boolean)
                _Enabled = value
            End Set
        End Property
        Public Property ComolUniqueID() As String
            Get
                ComolUniqueID = _ComolUniqueID
            End Get
            Set(ByVal value As String)
                _ComolUniqueID = value
            End Set
        End Property
        Public Property ErrorsType() As List(Of dtoErrorType)
            Get
                ErrorsType = _ErrorsType
            End Get
            Set(ByVal value As List(Of dtoErrorType))
                _ErrorsType = value
            End Set
        End Property
#End Region

        Public Sub New()
            _Enabled = False
            _ErrorsType = New List(Of dtoErrorType)
        End Sub

        Public Function isSendingEnabled(ByVal pType As ErrorsNotificationService.ErrorType) As Boolean
            Return (From oType In _ErrorsType Where oType.Type = pType AndAlso oType.Enabled).Any
        End Function

        Public Function FindPersistTo(ByVal pType As ErrorsNotificationService.ErrorType) As ErrorsNotificationService.PersistTo
            Return (From oType In _ErrorsType Where oType.Type = pType AndAlso oType.Enabled Select oType.SendTo).FirstOrDefault
        End Function

    End Class
End Namespace