Imports COL_BusinessLogic_v2.UCServices

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class ModuleCommunityFaq

#Region "Private Property"
        Private _ViewFaqs As Boolean
        Private _CreateFaq As Boolean
        Private _ModifyFaq As Boolean
        Private _DeleteFaq As Boolean
        Private _CreateCategory As Boolean
        Private _ModifyCategory As Boolean
        Private _DeleteCategory As Boolean
#End Region

#Region "Public Property"
        Public Property ViewFaqs As Boolean
            Get
                ViewFaqs = _ViewFaqs
            End Get
            Set(ByVal Value As Boolean)
                _ViewFaqs = Value
            End Set
        End Property

        Public Property CreateFaq As Boolean
            Get
                CreateFaq = _CreateFaq
            End Get
            Set(ByVal Value As Boolean)
                _CreateFaq = Value
            End Set
        End Property
        Public Property ModifyFaq As Boolean
            Get
                ModifyFaq = _ModifyFaq
            End Get
            Set(ByVal Value As Boolean)
                _ModifyFaq = Value
            End Set
        End Property
        Public Property DeleteFaq As Boolean
            Get
                DeleteFaq = _DeleteFaq
            End Get
            Set(ByVal Value As Boolean)
                _DeleteFaq = Value
            End Set
        End Property
        Public Property CreateCategory As Boolean
            Get
                CreateCategory = _CreateCategory
            End Get
            Set(ByVal Value As Boolean)
                _CreateCategory = Value
            End Set
        End Property
        Public Property ModifyCategory As Boolean
            Get
                ModifyCategory = _ModifyCategory
            End Get
            Set(ByVal Value As Boolean)
                _ModifyCategory = Value
            End Set
        End Property
        Public Property DeleteCategory As Boolean
            Get
                DeleteCategory = _DeleteCategory
            End Get
            Set(ByVal Value As Boolean)
                _DeleteCategory = Value
            End Set
        End Property
#End Region

        Sub New()

        End Sub
        Sub New(ByVal oService As Services_Faq)
            With oService
                'None = -1               '<- NO PERMISSIONE
                'Read = 0 '1             '<- Visualizzazione FAQ
                Me.ViewFaqs = .Read OrElse .Admin
                'Write = 1 '2            '<- Scrive/Crea FAQ
                Me.CreateFaq = .Write OrElse .Admin
                'Change = 2 '4           '<- Modifica FAQ
                Me.ModifyFaq = .Change OrElse .Admin
                'Delete = 3 '8           '<- Cancella FAQ
                Me.DeleteFaq = .Delete OrElse .Admin
                'Moderate = 4 '16        '<- Manage delle CATEGORY
                Me.CreateCategory = .Moderate OrElse .Admin
                Me.DeleteCategory = .Moderate OrElse .Admin
                Me.ModifyCategory = .Moderate OrElse .Admin

                'Admin = 6 '64           '<- ALL

            End With
        End Sub
        'Shared Function CreatePortalmodule(ByVal TypeID As Integer) As ModuleCommunityDiary
        '    Dim oService As New ModuleCommunityDiary
        '    With oService
        '        .Administration = (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
        '        .DeleteItem = (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo)
        '        .Edit = (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo)
        '        .ManagementPermission = (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
        '        .PrintList = (TypeID <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)
        '        .UploadFile = (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin) OrElse (TypeID = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo)
        '        .ViewDiaryItems = (TypeID <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest)
        '    End With

        '    Return oService
        'End Function

    End Class
End Namespace