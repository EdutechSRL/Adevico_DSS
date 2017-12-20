using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CommunityFile : BaseCommunityFile
	{
		//Private _Community As Community
		//Private _Owner As Person
		//Private _Name As String
		//Private _Description As String
		//Private _CreatedOn As DateTime
		//Private _ContentType As String
		//Private _Size As Long

		//Private _isFile As Boolean
		//Private _isVirtual As Boolean
		//Private _isSCORM As Boolean
		//Private _isVideocast As Boolean
		//Private _isDeleted As Boolean
		//Private _isPersonal As Boolean
		//Private _isVisibile As Boolean
		//Private _UniqueID As System.Guid
		//Private _CloneID As Long
		//Private _FolderId As Long
		//Private _Downloads As Long
		//Private _FilePath As String
		//Private _UserSubscription As Subscription
		// Private _FileCategoryID As Integer
		// Private _ModifiedOn As DateTime?
		// Private _ModifiedBy As Person
		//     Private _IsDownloadable As Boolean
		//Private _Extension As String
		//Private _CloneUniqueID As System.Guid
		//Private _Assignments As IList(Of CommunityFileAssignment)
		//Public Overridable Person CommunityOwner() As Community
		//    Get
		//        Return _Community
		//    End Get
		//    Set(ByVal value As Community)
		//        _Community = value
		//    End Set
		//End Person
		//Public Overridable Person Owner() As Person
		//    Get
		//        Return _Owner
		//    End Get
		//    Set(ByVal value As Person)
		//        _Owner = value
		//    End Set
		//End Person
		//Public Overridable Person Name() As String
		//    Get
		//        Return _Name
		//    End Get
		//    Set(ByVal value As String)
		//        _Name = value
		//    End Set
		//End Person
		//Public Overridable Person Extension() As String
		//    Get
		//        Return _Extension
		//    End Get
		//    Set(ByVal value As String)
		//        _Extension = value
		//    End Set
		//End Person
		//Public Overridable Person Description() As String
		//    Get
		//        Return _Description
		//    End Get
		//    Set(ByVal value As String)
		//        _Description = value
		//    End Set
		//End Person
		//Public Overridable Person CreatedOn() As DateTime
		//    Get
		//        Return _CreatedOn
		//    End Get
		//    Set(ByVal value As DateTime)
		//        _CreatedOn = value
		//    End Set
		//End Person
		//Public Overridable Person Size() As Long
		//    Get
		//        Return _Size
		//    End Get
		//    Set(ByVal value As Long)
		//        _Size = value
		//    End Set
		//End Person
		//Public Overridable Person ContentType() As String
		//    Get
		//        Return _ContentType
		//    End Get
		//    Set(ByVal value As String)
		//        _ContentType = value
		//    End Set
		//End Person

		//Public Overridable Person isVirtual() As Boolean
		//    Get
		//        Return _isVirtual
		//    End Get
		//    Set(ByVal value As Boolean)
		//        _isVirtual = value
		//    End Set
		//End Person
		//Public Overridable Person isFile() As Boolean
		//    Get
		//        Return _isFile
		//    End Get
		//    Set(ByVal value As Boolean)
		//        _isFile = value
		//    End Set
		//End Person
		//Public Overridable Person isSCORM() As Boolean
		//    Get
		//        Return _isSCORM
		//    End Get
		//    Set(ByVal value As Boolean)
		//        _isSCORM = value
		//    End Set
		//End Person
		//Public Overridable Person isVideocast() As Boolean
		//    Get
		//        Return _isVideocast
		//    End Get
		//    Set(ByVal value As Boolean)
		//        _isVideocast = value
		//    End Set
		//End Person
		//Public Overridable Person isDeleted() As Boolean
		//    Get
		//        Return _isDeleted
		//    End Get
		//    Set(ByVal value As Boolean)
		//        _isDeleted = value
		//    End Set
		//End Person
		//Public Overridable Person isPersonal() As Boolean
		//    Get
		//        Return _isPersonal
		//    End Get
		//    Set(ByVal value As Boolean)
		//        _isPersonal = value
		//    End Set
		//End Person
		//Public Overridable Person isVisible() As Boolean
		//    Get
		//        Return _isVisibile
		//    End Get
		//    Set(ByVal value As Boolean)
		//        _isVisibile = value
		//    End Set
		//End Person
		public virtual int Level { get; set; }
		//Public Overridable Person CloneID() As Long
		//    Get
		//        Return _CloneID
		//    End Get
		//    Set(ByVal value As Long)
		//        _CloneID = value
		//    End Set
		//End Person
		//Public Overridable Person FolderId() As Long
		//    Get
		//        Return _FolderId
		//    End Get
		//    Set(ByVal value As Long)
		//        _FolderId = value
		//    End Set
		//End Person
		//Public Overridable Person Downloads() As Long
		//    Get
		//        Return _Downloads
		//    End Get
		//    Set(ByVal value As Long)
		//        _Downloads = value
		//    End Set
		//End Person

		//Public Overridable Person FilePath() As String
		//    Get
		//        Return _FilePath
		//    End Get
		//    Set(ByVal value As String)
		//        _FilePath = value
		//    End Set
		//End Person

		//Public Overridable Person ModifiedBy() As Person
		//    Get
		//        Return _ModifiedBy
		//    End Get
		//    Set(ByVal value As Person)
		//        _ModifiedBy = value
		//    End Set
		//End Person

		//Public Overridable Person ModifiedOn() As DateTime?
		//    Get
		//        Return _ModifiedOn
		//    End Get
		//    Set(ByVal value As DateTime?)
		//        _ModifiedOn = value
		//    End Set
		//End Person

		//Public Overridable Person FileCategoryID() As Integer
		//    Get
		//        Return _FileCategoryID
		//    End Get
		//    Set(ByVal value As Integer)
		//        _FileCategoryID = value
		//    End Set
		//End Person
		//'Public Overridable Person UserSubscription() As Subscription
		//'    Get
		//'        Return _UserSubscription
		//'    End Get
		//'    Set(ByVal value As Subscription)
		//'        _UserSubscription = value
		//'    End Set
		//'End Person

		//Public Overridable Person UniqueID() As System.Guid
		//    Get
		//        Return _UniqueID
		//    End Get
		//    Set(ByVal value As System.Guid)
		//        _UniqueID = value
		//    End Set
		//End Person

		//Public Overridable Person CloneUniqueID() As System.Guid
		//    Get
		//        Return _CloneUniqueID
		//    End Get
		//    Set(ByVal value As System.Guid)
		//        _CloneUniqueID = value
		//    End Set
		//End Person


		//Public Overridable Person IsDownloadable() As Boolean
		//    Get
		//        Return _IsDownloadable
		//    End Get
		//    Set(ByVal value As Boolean)
		//        _IsDownloadable = value
		//    End Set
		//End Person
		public virtual int DisplayOrder { get; set; }

		//Public Overridable Person DisplayName() As String
		//    Get
		//        Return _Name & _Extension
		//    End Get
		//    Set(ByVal value As String)

		//    End Set
		//End Person
		//'Public Overridable Person Assignments() As IList(Of CommunityFileAssignment)
		//'    Get
		//'        Return _Assignments
		//'    End Get
		//'    Set(ByVal value As IList(Of CommunityFileAssignment))
		//'        _Assignments = value
		//'    End Set
		//'End Person

		public CommunityFile() : base()
		{
			DisplayOrder = 1;
		}
	}
}