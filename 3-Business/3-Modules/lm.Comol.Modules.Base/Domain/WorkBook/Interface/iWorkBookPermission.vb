Namespace lm.Comol.Modules.Base.DomainModel
    <CLSCompliant(True)> Public Interface iWorkBookPermission
        Property AddItems() As Boolean
		Property ReadWorkBook() As Boolean
		Property DeleteWorkBook() As Boolean
        Property ChangeApprovation() As Boolean
		Property CreateWorkBook() As Boolean
        Property EditWorkBook() As Boolean
		Property UndeleteWorkBook() As Boolean
		Property Admin() As Boolean
    End Interface
End Namespace