Namespace lm.Comol.Modules.Base.DomainModel
	<Serializable(), CLSCompliant(True)> Public Class WorkBookItemPermission

#Region "private Property"
        Private _Add As Boolean
        Private _Write As Boolean
        Private _DownladAllowed As Boolean
        Private _Read As Boolean
        Private _Delete As Boolean
        Private _ChangeApprovation As Boolean
        Private _UnDelete As Boolean
        Private _ViewPersonalNote As Boolean
        Private _ChangeEditing As Boolean
#End Region

#Region "Public Overridable Property"
        Public Overridable Property ChangeEditing() As Boolean
            Get
                ChangeEditing = _ChangeEditing
            End Get
            Set(ByVal Value As Boolean)
                _ChangeEditing = Value
            End Set
        End Property
        Public Overridable Property ChangeApprovation() As Boolean
            Get
                ChangeApprovation = _ChangeApprovation
            End Get
            Set(ByVal Value As Boolean)
                _ChangeApprovation = Value
            End Set
        End Property
        Public Overridable Property UnDelete() As Boolean
            Get
                Return _UnDelete
            End Get
            Set(ByVal value As Boolean)
                _UnDelete = value
            End Set
        End Property
        Public Overridable Property Delete() As Boolean
            Get
                Delete = _Delete
            End Get
            Set(ByVal Value As Boolean)
                _Delete = Value
            End Set
        End Property
        Public Overridable Property DownLoadFile() As Boolean
            Get
                DownLoadFile = _DownladAllowed
            End Get
            Set(ByVal Value As Boolean)
                _DownladAllowed = Value
            End Set
        End Property
        Public Overridable Property Read() As Boolean
            Get
                Read = _Read
            End Get
            Set(ByVal Value As Boolean)
                _Read = Value
            End Set
        End Property
        'Public Overridable Property Add() As Boolean Implements iWorkBookItemPermission.Add
        '	Get
        '		Add = _Add
        '	End Get
        '	Set(ByVal Value As Boolean)
        '		_Add = Value
        '	End Set
        'End Property
        Public Overridable Property Write() As Boolean
            Get
                Write = _Write
            End Get
            Set(ByVal Value As Boolean)
                _Write = Value
            End Set
        End Property
        Public Overridable Property ViewPersonalNote() As Boolean
            Get
                Return _ViewPersonalNote
            End Get
            Set(ByVal value As Boolean)
                _ViewPersonalNote = value
            End Set
        End Property
#End Region

       
    End Class
End Namespace