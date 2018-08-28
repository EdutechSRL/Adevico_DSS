Imports Comol.Entity.Configuration.Components

Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class TagSettings

		Public Enum TagType
			Questionario = 1
			Wiki = 2
			Mail = 3
			MailingList = 4
			SMS = 5
			RSS = 6
			FileNotification = 7
			NewUserNotification = 8
			NewMemberNotification = 9
			SubscriptionNotification = 10
			ForumNotification = 11
		End Enum

		Private _HashTableTag As Hashtable

#Region "Public properties"
		Public ReadOnly Property Questionario() As List(Of TemplateTag)
			Get
				Questionario = DirectCast(_HashTableTag.Item(TagType.Questionario), List(Of TemplateTag))
			End Get
		End Property
#End Region

		Public Sub New()
			_HashTableTag = New Hashtable
		End Sub

		Public Sub AddTags(ByVal oTagList As List(Of TemplateTag), ByVal oTagType As TagType)
			_HashTableTag.Add(oTagType, oTagList)
		End Sub

	End Class
End Namespace