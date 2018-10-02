Imports lm.Comol.Core.DomainModel
Imports NHibernate

Namespace lm.Comol.Modules.Base.BusinessLogic
	Public Class ManagerFiles
		Implements lm.Comol.Core.DomainModel.Common.iDomainManager

#Region "Private property"
		Private _UserContext As iUserContext
		Private _Datacontext As iDataContext
#End Region

#Region "Public property"
		Private ReadOnly Property DC() As iDataContext
			Get
				Return _Datacontext
			End Get
		End Property
		Private ReadOnly Property CurrentUserContext() As iUserContext
			Get
				Return _UserContext
			End Get
		End Property
#End Region

		Public Sub New()
		End Sub
		Public Sub New(ByVal oContext As iApplicationContext)
			Me._UserContext = oContext.UserContext
			Me._Datacontext = oContext.DataContext
		End Sub
		Public Sub New(ByVal oUserContext As iUserContext, ByVal oDatacontext As iDataContext)
			Me._UserContext = oUserContext
			Me._Datacontext = oDatacontext
		End Sub

		Public Function GetBaseFile(ByVal FileID As System.Guid) As BaseFile
			Dim oFile As BaseFile = Nothing
			Try
				oFile = DC.GetById(Of BaseFile)(FileID)
			Catch ex As Exception
				Debug.Write(ex.ToString)

				Return Nothing
			End Try
			Return oFile
		End Function
		Public Function DeleteVirtual(ByVal FileID As System.Guid) As BaseFile
			Dim oFile As BaseFile = Me.GetBaseFile(FileID)
			Dim oMeta As New MetaData

			Try
				DC.BeginTransaction()
				Dim oPerson As Person = DC.GetById(Of Person)(Me.CurrentUserContext.CurrentUser.Id)
				If Not IsNothing(oFile) AndAlso IsNothing(oPerson) = False Then
					oFile.MetaInfo.DeletedOn = Now
					oFile.MetaInfo.DeletedBy = oPerson
					oFile.MetaInfo.isDeleted = True
					DC.SaveOrUpdate(oFile)

				End If
				DC.Commit()
				Return oFile
			Catch ex As Exception
				Debug.Write(ex.ToString)
				DC.Rollback()
				Return oFile
			End Try
			Return oFile
		End Function
		Public Function UnDeleteVirtual(ByVal FileID As System.Guid) As BaseFile
			Dim oFile As BaseFile = Me.GetBaseFile(FileID)
			Dim oMeta As New MetaData

			Try
				DC.BeginTransaction()
				If Not IsNothing(oFile) Then
					oFile.MetaInfo.DeletedOn = Nothing
					oFile.MetaInfo.DeletedBy = Nothing
					oFile.MetaInfo.isDeleted = False
					DC.SaveOrUpdate(oFile)
				End If
				DC.Commit()
				Return oFile
			Catch ex As Exception
				Debug.Write(ex.ToString)
				DC.Rollback()
				Return oFile
			End Try
			Return oFile
		End Function
		Public Function Save(ByVal CreatedByID As Integer, ByVal OwnerID As Integer, ByVal oUnsavedItem As BaseFile) As BaseFile
			Dim oBaseFile As New BaseFile
			Dim oMeta As New MetaData

			Try
				DC.BeginTransaction()
				Dim oCreatedBy As Person = DC.GetById(Of Person)(CreatedByID)
				Dim oOwner As Person = DC.GetById(Of Person)(OwnerID)

				If oUnsavedItem.Id = System.Guid.Empty Then
					oBaseFile.MetaInfo.CreatedBy = oCreatedBy
					oBaseFile.MetaInfo.CreatedOn = Now
					oBaseFile.HardLink = 1
				Else
					oBaseFile = DC.GetById(Of BaseFile)(oUnsavedItem.Id)
					oBaseFile.MetaInfo.ModifiedBy = oCreatedBy
					oBaseFile.MetaInfo.ModifiedOn = Now
				End If

				With oBaseFile
					.Owner = oOwner
					.ContentType = oUnsavedItem.ContentType
					.Description = oUnsavedItem.Description
					.Name = oUnsavedItem.Name
					.MetaInfo.Approvation = oUnsavedItem.MetaInfo.Approvation
					.MetaInfo.canDelete = oUnsavedItem.MetaInfo.canDelete
					.MetaInfo.canModify = oUnsavedItem.MetaInfo.canModify
					.Extension = oUnsavedItem.Extension
					.Size = oUnsavedItem.Size

				End With
				DC.SaveOrUpdate(oBaseFile)
				DC.Commit()
			Catch ex As Exception
				Debug.Write(ex.ToString)
				DC.Rollback()
				Return Nothing
			End Try
			Return oBaseFile
		End Function

	End Class
End Namespace