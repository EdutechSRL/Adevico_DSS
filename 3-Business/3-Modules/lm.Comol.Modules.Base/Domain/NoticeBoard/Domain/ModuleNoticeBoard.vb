Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2
Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class ModuleNoticeBoard

        Private _Read As Boolean
        Private _Write As Boolean
        Private _GrantPermission As Boolean
        Private _Admin As Boolean

#Region "Private Property"
        Private _ViewCurrentMessage As Boolean
        Private _EditMessage As Boolean
        Private _DeleteMessage As Boolean
        Private _ManagementPermission As Boolean
        Private _ServiceAdministration As Boolean
        Private _ViewOldMessage As Boolean
        Private _RetrieveOldMessage As Boolean
        Private _PrintMessage As Boolean
#End Region

#Region "Public Property"
        Public Property ViewCurrentMessage() As Boolean
            Get
                ViewCurrentMessage = _ViewCurrentMessage
            End Get
            Set(ByVal Value As Boolean)
                _ViewCurrentMessage = Value
            End Set
        End Property
        Public Property EditMessage() As Boolean
            Get
                EditMessage = _EditMessage
            End Get
            Set(ByVal Value As Boolean)
                _EditMessage = Value
            End Set
        End Property
        Public Property DeleteMessage() As Boolean
            Get
                DeleteMessage = _DeleteMessage
            End Get
            Set(ByVal Value As Boolean)
                _DeleteMessage = Value
            End Set
        End Property
        Public Property ManagementPermission() As Boolean
            Get
                ManagementPermission = _ManagementPermission
            End Get
            Set(ByVal Value As Boolean)
                _ManagementPermission = Value
            End Set
        End Property
        Public Property ServiceAdministration() As Boolean
            Get
                ServiceAdministration = _ServiceAdministration
            End Get
            Set(ByVal Value As Boolean)
                _ServiceAdministration = Value
            End Set
        End Property
        Public Property ViewOldMessage() As Boolean
            Get
                ViewOldMessage = _ViewOldMessage
            End Get
            Set(ByVal Value As Boolean)
                _ViewOldMessage = Value
            End Set
        End Property
        Public Property RetrieveOldMessage() As Boolean
            Get
                RetrieveOldMessage = _RetrieveOldMessage
            End Get
            Set(ByVal Value As Boolean)
                _RetrieveOldMessage = Value
            End Set
        End Property
        Public Property PrintMessage() As Boolean
            Get
                PrintMessage = _PrintMessage
            End Get
            Set(ByVal Value As Boolean)
                _PrintMessage = Value
            End Set
        End Property
#End Region

        Sub New()

        End Sub

        Sub New(ByVal oService As Services_Bacheca)
            With oService
                Me.DeleteMessage = .Admin OrElse .Write
                Me.EditMessage = .Admin OrElse .Write
                Me.ManagementPermission = .GrantPermission
                Me.PrintMessage = .Read OrElse .Write OrElse .Admin
                Me.RetrieveOldMessage = .Write OrElse .Admin
                Me.ServiceAdministration = .Admin OrElse .Write
                Me.ViewCurrentMessage = .Read OrElse .Write OrElse .Admin
                Me.ViewOldMessage = .Read OrElse .Write OrElse .Admin
            End With
        End Sub

        Shared Function CreatePortalmodule(ByVal UserTypeID As Integer) As ModuleNoticeBoard
            Dim oService As Services_Bacheca = Services_Bacheca.Create
            oService.Admin = (UserTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = Main.TipoPersonaStandard.SysAdmin OrElse UserTypeID = Main.TipoPersonaStandard.Amministrativo)
            oService.Read = (UserTypeID <> Main.TipoPersonaStandard.Guest)
            oService.GrantPermission = (UserTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = Main.TipoPersonaStandard.SysAdmin)
            oService.Write = (UserTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = Main.TipoPersonaStandard.SysAdmin OrElse UserTypeID = Main.TipoPersonaStandard.Amministrativo)

            Return New ModuleNoticeBoard(oService)
        End Function


                    

    End Class
End Namespace