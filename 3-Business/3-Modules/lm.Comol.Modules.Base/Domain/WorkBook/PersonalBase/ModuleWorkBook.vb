Imports COL_BusinessLogic_v2.UCServices

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class ModuleWorkBook

#Region "Private Property"
        Private _CreateWorkBook As Boolean
        Private _CreateGroupWorkBook As Boolean
        Private _ManagementPermission As Boolean
        Private _Administration As Boolean
        Private _AddItemToOther As Boolean
        Private _DeleteItemsFromOther As Boolean
        Private _ChangeOtherWorkBook As Boolean
        Private _ChangeApprovationStatus As Boolean
        Private _ListOtherWorkBooks As Boolean
        Private _ReadOtherWorkBook As Boolean
        Private _DownladAllowed As Boolean
        Private _PrintOtherWorkBook As Boolean
#End Region

#Region "Public Property"
        Public Property CreateWorkBook() As Boolean
            Get
                CreateWorkBook = _CreateWorkBook
            End Get
            Set(ByVal Value As Boolean)
                _CreateWorkBook = Value
            End Set
        End Property
        Public Property CreateGroupWorkBook() As Boolean
            Get
                CreateGroupWorkBook = _CreateGroupWorkBook
            End Get
            Set(ByVal Value As Boolean)
                _CreateGroupWorkBook = Value
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
        Public Property Administration() As Boolean
            Get
                Administration = _Administration
            End Get
            Set(ByVal Value As Boolean)
                _Administration = Value
            End Set
        End Property
        Public Property AddItemsToOther() As Boolean
            Get
                AddItemsToOther = _AddItemToOther
            End Get
            Set(ByVal Value As Boolean)
                _AddItemToOther = Value
            End Set
        End Property
        Public Property PrintOtherWorkBook() As Boolean
            Get
                PrintOtherWorkBook = _PrintOtherWorkBook
            End Get
            Set(ByVal Value As Boolean)
                _PrintOtherWorkBook = Value
            End Set
        End Property
        Public Property DeleteItemsFromOther() As Boolean
            Get
                DeleteItemsFromOther = _DeleteItemsFromOther
            End Get
            Set(ByVal Value As Boolean)
                _DeleteItemsFromOther = Value
            End Set
        End Property
        Public Property ChangeOtherWorkBook() As Boolean
            Get
                ChangeOtherWorkBook = _ChangeOtherWorkBook
            End Get
            Set(ByVal Value As Boolean)
                _ChangeOtherWorkBook = Value
            End Set
        End Property
        Public Property ChangeApprovation() As Boolean
            Get
                ChangeApprovation = _ChangeApprovationStatus
            End Get
            Set(ByVal Value As Boolean)
                _ChangeApprovationStatus = Value
            End Set
        End Property
        Public Property ListOtherWorkBooks() As Boolean
            Get
                ListOtherWorkBooks = _ListOtherWorkBooks
            End Get
            Set(ByVal Value As Boolean)
                _ListOtherWorkBooks = Value
            End Set
        End Property
        Public Property ReadOtherWorkBook() As Boolean
            Get
                ReadOtherWorkBook = _ReadOtherWorkBook
            End Get
            Set(ByVal Value As Boolean)
                _ReadOtherWorkBook = Value
            End Set
        End Property
        Public Property DownLoadItemFiles() As Boolean
            Get
                DownLoadItemFiles = _DownladAllowed
            End Get
            Set(ByVal Value As Boolean)
                _DownladAllowed = Value
            End Set
        End Property
#End Region

        Sub New()

        End Sub
        Sub New(ByVal oService As Services_WorkBook)
            With oService
                Me.AddItemsToOther = .AddItemsToOther
                Me.Administration = .Admin
                Me.ChangeApprovation = .ChangeApprovationStatus
                Me.ChangeOtherWorkBook = .ChangeOtherWorkbook
                Me.CreateGroupWorkBook = .CreateGroupWorkbook
                Me.CreateWorkBook = .CreateWorkBook
                Me.DeleteItemsFromOther = .DeleteItemsFromOther
                Me.DownLoadItemFiles = .DownLoadItemFiles
                Me.ListOtherWorkBooks = .ListOtherWorkBook
                Me.ManagementPermission = .GrantPermission
                Me.PrintOtherWorkBook = .PrintOtherWorkBook
                Me.ReadOtherWorkBook = .ReadOtherWorkBook
            End With
        End Sub

        Public Shared Function CreatePortalmodule() As ModuleWorkBook
            Dim oModule As New ModuleWorkBook
            oModule.AddItemsToOther = False
            oModule.Administration = False
            oModule.ChangeApprovation = False
            oModule.ChangeOtherWorkBook = False
            oModule.CreateGroupWorkBook = True
            oModule.CreateWorkBook = True
            oModule.DeleteItemsFromOther = False
            oModule.DownLoadItemFiles = True
            oModule.ListOtherWorkBooks = False
            oModule.ManagementPermission = False
            oModule.PrintOtherWorkBook = False
            oModule.ReadOtherWorkBook = False
            Return oModule
        End Function

       
    End Class
End Namespace