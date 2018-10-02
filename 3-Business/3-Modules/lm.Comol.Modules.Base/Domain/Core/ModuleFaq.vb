Imports COL_BusinessLogic_v2.UCServices

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class ModuleFaq

#Region "Private Property"
        Private _ViewFaq As Boolean
        Private _CreateFaq As Boolean
        Private _ModifyFaq As Boolean
        Private _DeleteFaq As Boolean
        Private _ManageCategory As Boolean
        Private _Admin As Boolean
#End Region


#Region "Public Property"
        Public Property ViewFaq() As Boolean
            Get
                Return _ViewFaq
            End Get
            Set(ByVal Value As Boolean)
                _ViewFaq = Value
            End Set
        End Property

        Public Property CreateFaq() As Boolean
            Get
                Return _CreateFaq
            End Get
            Set(ByVal Value As Boolean)
                _CreateFaq = Value
            End Set
        End Property

        Public Property ModifyFaq() As Boolean
            Get
                Return _ModifyFaq
            End Get
            Set(ByVal Value As Boolean)
                _ModifyFaq = Value
            End Set
        End Property

        Public Property DeleteFaq() As Boolean
            Get
                Return _DeleteFaq
            End Get
            Set(ByVal Value As Boolean)
                _DeleteFaq = Value
            End Set
        End Property

        Public Property ManageCategory() As Boolean
            Get
                Return _ManageCategory
            End Get
            Set(ByVal Value As Boolean)
                _ManageCategory = Value
            End Set
        End Property

        Public Property Admin() As Boolean
            Get
                Return _Admin
            End Get
            Set(ByVal Value As Boolean)
                _Admin = Value
            End Set
        End Property

#End Region

        Sub New()

        End Sub
        Sub New(ByVal oService As Services_Faq)
            With oService
                Me.ViewFaq = .Read OrElse .Write OrElse .Moderate OrElse .Delete OrElse .Change OrElse .Admin
                Me.CreateFaq = .Write OrElse .Moderate OrElse .Delete OrElse .Change OrElse .Admin
                Me.ModifyFaq = .Moderate OrElse .Delete OrElse .Change OrElse .Admin
                Me.DeleteFaq = .Delete OrElse .Change OrElse .Admin
                Me.ManageCategory = .Change OrElse .Admin
                Me.Admin = .Admin
            End With
        End Sub

        Shared Function CreatePortalmodule(ByVal UserTypeID As Integer) As ModuleFaq
            Dim oService As New ModuleFaq
            With oService
                .ViewFaq = (UserTypeID <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)

                .CreateFaq = (UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
                .ModifyFaq = (UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
                .DeleteFaq = (UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
                .ManageCategory = (UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
                .Admin = (UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
            End With
            Return oService
        End Function


    End Class
End Namespace