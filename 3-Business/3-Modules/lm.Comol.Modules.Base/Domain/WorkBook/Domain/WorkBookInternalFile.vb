Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class WorkBookInternalFile
        Inherits WorkBookFile

#Region "Private Property"
        Private _File As lm.Comol.Core.DomainModel.BaseFile
#End Region

#Region "Public Overridable Property"
        Public Overridable Property File() As lm.Comol.Core.DomainModel.BaseFile
            Get
                Return Me._File
            End Get
            Set(ByVal value As lm.Comol.Core.DomainModel.BaseFile)
                Me._File = value
            End Set
        End Property
#End Region

    End Class
End Namespace