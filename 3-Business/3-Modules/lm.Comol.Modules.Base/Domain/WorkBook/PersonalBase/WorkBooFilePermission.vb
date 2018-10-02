Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class WorkBooFilePermission

#Region "private Property"
        Private _ChangeApprovation As Boolean
        Private _Undelete As Boolean
        Private _Delete As Boolean
        Private _Download As Boolean
        Private _VirtualDelete As Boolean
        Private _Unlink As Boolean
        Private _Play As Boolean
#End Region

#Region "Public Overridable Property"
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
                Return _Undelete
            End Get
            Set(ByVal value As Boolean)
                _Undelete = value
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
        Public Overridable Property Download() As Boolean
            Get
                Download = _Download
            End Get
            Set(ByVal Value As Boolean)
                _Download = Value
            End Set
        End Property
        Public Overridable Property VirtualDelete() As Boolean
            Get
                VirtualDelete = _VirtualDelete
            End Get
            Set(ByVal Value As Boolean)
                _VirtualDelete = Value
            End Set
        End Property
        Public Overridable Property Unlink() As Boolean
            Get
                Unlink = _Unlink
            End Get
            Set(ByVal Value As Boolean)
                _Unlink = Value
            End Set
        End Property
        Public Overridable Property Play() As Boolean
            Get
                Return _Play
            End Get
            Set(ByVal value As Boolean)
                _Play = value
            End Set
        End Property
#End Region

        Sub New()

        End Sub

    End Class
End Namespace