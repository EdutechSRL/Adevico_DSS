Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class WorkBookCommunityFile
        Inherits WorkBookFile

#Region "Private Property"
        Private _FileCommunity As lm.Comol.Core.DomainModel.CommunityFile
#End Region

#Region "Public Overridable Property"
        Public Overridable Property FileCommunity() As lm.Comol.Core.DomainModel.CommunityFile
            Get
                Return Me._FileCommunity
            End Get
            Set(ByVal value As lm.Comol.Core.DomainModel.CommunityFile)
                Me._FileCommunity = value
            End Set
        End Property
#End Region

    End Class
End Namespace