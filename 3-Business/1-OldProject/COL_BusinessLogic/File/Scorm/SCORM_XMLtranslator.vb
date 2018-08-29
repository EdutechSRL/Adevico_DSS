Imports System.Xml
Imports COL_BusinessLogic_v2.Comol.Materiale.Scorm.Metadata

Namespace Comol.Materiale.Scorm
	Public Class SCORM_XMLtranslator
		Inherits ObjectBase

		Sub New()

		End Sub

		Public Shared Function isScormPackage(ByVal PackagePath As String, Optional ByVal PackageManifest As String = "imsmanifest.xml") As Boolean
            Return lm.Comol.Core.File.Exists.File(PackagePath & PackageManifest)
		End Function

		Public Function LoadPackage(ByVal ScormID As Integer, ByVal PackagePath As String, ByVal PackageManifest As String) As SCORM_Package
			Dim oPackage As SCORM_Package
			Dim cacheKey As String = CachePolicy.ScormManifest(ScormID)

			If ObjectBase.Cache(cacheKey) Is Nothing Then
				oPackage = Me.LoadFromXML(PackagePath, PackageManifest)
				If oPackage.IsValid Then
					oPackage.Validate()
				End If
				ObjectBase.Cache.Insert(cacheKey, oPackage, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
			Else
				oPackage = CType(ObjectBase.Cache(cacheKey), SCORM_Package)
			End If
			Return oPackage
		End Function

		Private Function LoadFromXML(ByVal PackagePath As String, ByVal PackageManifest As String) As SCORM_Package
			Dim oScormManifest As New XmlDocument

			Try
				oScormManifest.Load(PackagePath & PackageManifest)
				If IsNothing(oScormManifest.GetElementsByTagName("manifest")(0)) Then
					Return New SCORM_Package(PackagePath, False)
				Else
					Return ParseXMLfile(oScormManifest, PackagePath)
				End If
			Catch ex As Exception
				Return New SCORM_Package(PackagePath, False)
			End Try
			Return Nothing
		End Function


		Private Function ParseXMLfile(ByVal oXmlDocument As XmlDocument, ByVal PackagePath As String) As SCORM_Package
			Dim oScormPackage As New SCORM_Package(PackagePath)
			Dim oXMLnodeList As XmlNodeList


			oXMLnodeList = oXmlDocument.GetElementsByTagName("manifest")
			If oXMLnodeList.Count > 0 Then
				Dim oBaseUrl As String = ""
				oBaseUrl = Me.GetAttribute(oXMLnodeList(0), "base")
				If oBaseUrl <> "" Then
					oBaseUrl = Me.GetAttribute(oXMLnodeList(0), "xml:base")
				End If
				oScormPackage.ChangeUrlBase(oBaseUrl)
			End If
			oXMLnodeList = oXmlDocument.GetElementsByTagName("metadata")
			If oXMLnodeList.Count > 0 Then
				oScormPackage.IsValid = (oXMLnodeList.Count = 1)
				oScormPackage.MetaData = Me.ParseMetaData(oXMLnodeList(0))
			End If

			oXMLnodeList = oXmlDocument.GetElementsByTagName("resources")
			If oXMLnodeList.Count > 0 Then
				If oScormPackage.IsValid Then
					oScormPackage.IsValid = (oXMLnodeList.Count = 1)
				End If
				For Each oNode As XmlNode In oXMLnodeList(0).ChildNodes
					Dim oResource As SCORM_Resource
					oResource = Me.ParseResourceItem(oNode)
					If Not IsNothing(oResource) Then
						oScormPackage.Resources.Add(oResource)
					End If
				Next
			End If
			If oScormPackage.Resources.Count > 1 Then
				oScormPackage.RebuildResourceDependency()
			End If
			oXMLnodeList = oXmlDocument.GetElementsByTagName("organizations")
			If oXMLnodeList.Count > 0 Then
				If oScormPackage.IsValid Then
					oScormPackage.IsValid = (oXMLnodeList.Count = 1)
				End If

				Dim DefaultOrganization As String = Me.GetAttribute(oXMLnodeList(0), "default")
				For Each oNode As XmlNode In oXMLnodeList(0).ChildNodes
					Dim oOrganization As New SCORM_Organization
					oOrganization = Me.ParseOrganizations(oNode, oScormPackage.Resources)
					If oOrganization.ID = DefaultOrganization Then
						oOrganization.isDefault = True
					End If
					oScormPackage.Organizations.Add(oOrganization)
				Next
			End If
			Return oScormPackage
		End Function

		Private Function ParseOrganizations(ByVal oXmlNode As XmlNode, ByVal oResources As List(Of SCORM_Resource)) As SCORM_Organization
			Dim oOrganization As New SCORM_Organization

			oOrganization.ID = Me.GetAttribute(oXmlNode, "identifier")
			oOrganization.Title = oXmlNode.FirstChild.InnerText
			oOrganization.StructureType = Me.GetAttribute(oXmlNode, "structure")

			For Each oNode As XmlNode In oXmlNode.ChildNodes
				Dim oScormItem As New SCORM_Item
				oScormItem = Me.ParseScormItem(oNode, oResources)
				If Not IsNothing(oScormItem) Then
					oOrganization.Items.Add(oScormItem)
				End If
			Next
			Return oOrganization
		End Function
		Private Function ParseScormItem(ByVal oXmlNode As XmlNode, ByVal oResources As List(Of SCORM_Resource)) As SCORM_Item
			Dim oScormItem As New SCORM_Item

			If oXmlNode.Name = "item" Then
				If oXmlNode.HasChildNodes Then
					With oScormItem
						.ID = Me.GetAttribute(oXmlNode, "identifier")
						.IDresource = Me.GetAttribute(oXmlNode, "identifierref")
						.isVisible = CBool(Me.GetAttribute(oXmlNode, "isvisible") <> "False")
						.IndexResourceURL = Me.GetAttribute(oXmlNode, "href")
						.Title = oXmlNode.FirstChild.InnerText
						.Resource = oResources.Find(New GenericPredicate(Of SCORM_Resource, String)(.IDresource, AddressOf SCORM_Resource.FindResourceDependency))
						For Each oNode As XmlNode In oXmlNode.ChildNodes
							Dim oItem As New SCORM_Item
							oItem = Me.ParseScormItem(oNode, oResources)
							If Not IsNothing(oItem) Then
								.Items.Add(oItem)
							End If
						Next
					End With
				Else
					oScormItem.IsValid = False
				End If
			Else
				Return Nothing
			End If
			Return oScormItem
		End Function
		Private Function ParseResourceItem(ByVal oXmlNode As XmlNode) As SCORM_Resource
			Dim oScormResource As New SCORM_Resource

			If oXmlNode.Name = "resource" Then
				If oXmlNode.HasChildNodes Then
					With oScormResource
						.ID = Me.GetAttribute(oXmlNode, "identifier")
						.Href = Me.GetAttribute(oXmlNode, "href")
						.ScormType = Me.GetAttribute(oXmlNode, "adlcp:scormType")
						.Type = Me.GetAttribute(oXmlNode, "type")

						For Each oNode As XmlNode In oXmlNode.ChildNodes
							If oNode.Name = "file" Then
								Try
									.ResourceFiles.Add(New SCORM_ResourceFile(Me.GetAttribute(oNode, "name"), Me.GetAttribute(oNode, "href")))
								Catch ex As Exception
									.IsValid = False
								End Try
							ElseIf oNode.Name = "dependency" Then
								.DependencyList.Add(Me.GetAttribute(oNode, "identifierref"))
							End If
						Next
					End With
				Else
					oScormResource.IsValid = False
				End If
			End If
			Return oScormResource
		End Function

#Region "Parsificatori MetaData"
		Private Function ParseMetaData(ByVal oXmlNode As XmlNode) As SCORM_MetaData
			Dim oMetadata As New SCORM_MetaData()

			For Each oNode As XmlNode In oXmlNode.ChildNodes
				If oNode.Name = "schema" Then
					oMetadata.Schema = oNode.InnerText
				ElseIf oNode.Name = "schemaversion" Then
					oMetadata.SchemaVersion = oNode.InnerText
				ElseIf oNode.Name = "lom" Then
					For Each oNodeLom As XmlNode In oNode.ChildNodes
						If oNodeLom.Name = "general" Then
							oMetadata.General = Me.ParseMetaData_General(oNodeLom)
						ElseIf oNodeLom.Name = "lifecycle" Then
							oMetadata.Lifecycle = Me.ParseMetaData_LifeCycle(oNodeLom)
						ElseIf oNodeLom.Name = "technical" Then

						ElseIf oNodeLom.Name = "classification" Then
							oMetadata.Classification = Me.ParseMetaData_Classification(oNodeLom)
						ElseIf oNodeLom.Name = "rights" Then
							oMetadata.Rights = Me.ParseMetaData_Rights(oNodeLom)
						ElseIf oNodeLom.Name = "classification" Then
						End If
					Next
				End If
			Next
			Return oMetadata
		End Function
		Private Function ParseMetaData_General(ByVal oXmlNode As XmlNode) As SCORM_MetaDataGeneral
			Dim oMetadata As New SCORM_MetaDataGeneral

			For Each oNode As XmlNode In oXmlNode.ChildNodes
				If oNode.Name = "title" Then
					oMetadata.Title = ParseMetaData_General_SubKey(oNode)
				ElseIf oNode.Name = "keyword" Then
					oMetadata.Keywords.Add(New SCORM_Keyword(Me.ParseMetaData_General_SubKey(oNode)))
				ElseIf oNode.Name = "language" Then
					oMetadata.Language.Add(oNode.InnerText)
				ElseIf oNode.Name = "description" Then
					oMetadata.Description = ParseMetaData_General_SubKey(oNode)
				ElseIf oNode.Name = "identifier" Or oNode.Name = "catalogentry" Then
					Dim Entry As New SCORM_LangString
					Dim Catalog As String = ""
					For Each subNode As XmlNode In oNode.ChildNodes
						If subNode.Name = "entry" Then
							Entry = Me.GetLangString(subNode)
						ElseIf subNode.Name = "catalog" Then
							Catalog = subNode.InnerText
						End If
					Next
					oMetadata.Identifier = New SCORM_Identifier(Catalog, Entry)
				End If
			Next
			Return oMetadata
		End Function
		Private Function ParseMetaData_Rights(ByVal oXmlNode As XmlNode) As SCORM_MetaDataRights
			Dim oMetaDataRights As New SCORM_MetaDataRights

			For Each oNode As XmlNode In oXmlNode.ChildNodes
				If oNode.Name = "cost" Then
					oMetaDataRights.Cost = ParseMetaData_General_SourceValue(oNode)
				ElseIf oNode.Name = "copyrightandotherrestrictions" Then
					oMetaDataRights.CopyrightAndOtherRestrictions = ParseMetaData_General_SourceValue(oNode)
				ElseIf oNode.Name = "description" Then
					oMetaDataRights.Description = ParseMetaData_General_SubKey(oNode)
				End If
			Next
			Return oMetaDataRights
		End Function
		Private Function ParseMetaData_Classification(ByVal oXmlNode As XmlNode) As SCORM_MetaDataClassification
			Dim oMetaDataClassification As New SCORM_MetaDataClassification

			For Each oNode As XmlNode In oXmlNode.ChildNodes
				If oNode.Name = "purpose" Then
					oMetaDataClassification.Purpose = ParseMetaData_General_SourceValue(oNode)
				ElseIf oNode.Name = "keyword" Then
					oMetaDataClassification.Keywords.Add(New SCORM_Keyword(ParseMetaData_General_SubKey(oNode)))
				ElseIf oNode.Name = "taxonPath" Then
					oMetaDataClassification.TaxonPath.Add(ParseMetaData_Classification_TaxonPath(oNode))
				ElseIf oNode.Name = "description" Then
					oMetaDataClassification.Description = ParseMetaData_General_SubKey(oNode)
				End If
			Next
			Return oMetaDataClassification
		End Function
		Private Function ParseMetaData_LifeCycle(ByVal oXmlNode As XmlNode) As SCORM_MetaDataLifeCycle
			Dim oLifeCycle As New SCORM_MetaDataLifeCycle

			For Each oNode As XmlNode In oXmlNode.ChildNodes
				If oNode.Name = "version" Then
					oLifeCycle.Version = ParseMetaData_General_SubKey(oNode)
				ElseIf oNode.Name = "status" Then
					oLifeCycle.Status = Me.ParseMetaData_General_SourceValue(oNode)
				ElseIf oNode.Name = "contribute" Then
					oLifeCycle.Contributes.Add(Me.ParseMetaData_General_Contribute(oNode))
				End If
			Next
			Return oLifeCycle
		End Function
		Private Function ParseMetaData_Classification_TaxonPath(ByVal oXmlNode As XmlNode) As SCORM_TaxonPath
			Dim oTaxonPath As New SCORM_TaxonPath

			For Each oNode As XmlNode In oXmlNode.ChildNodes
				If oNode.Name = "source" Then
					oTaxonPath.Source = New SCORM_LangString(oNode.InnerText, Me.GetAttribute(oNode, "language"))
				ElseIf oNode.Name = "taxon" Then
					Dim ID As String = ""
					Dim oListaEntry As New List(Of SCORM_LangString)
					For Each oNodeTaxon As XmlNode In oNode.ChildNodes
						If oNodeTaxon.Name = "id" Or oNodeTaxon.Name = "identifier" Then
							ID = oNodeTaxon.InnerText
						ElseIf oNodeTaxon.Name = "entry" Then
							oListaEntry = Me.ParseMetaData_General_SubKey(oNodeTaxon)
						End If
					Next
					oTaxonPath.Taxon.Add(New SCORM_Taxon(ID, oListaEntry))
				End If
			Next
			Return oTaxonPath
		End Function
#End Region

#Region "Parsificatori Comuni"
		Private Function GetAttribute(ByVal oXmlNode As XmlNode, ByVal attribute As String) As String
			Try
				Return CType(oXmlNode, XmlElement).GetAttribute(attribute)
			Catch ex As Exception
				Return ""
			End Try
			Return ""
		End Function
		Private Function GetLangString(ByVal oXmlNode As XmlNode) As SCORM_LangString
			Dim Language As String = Me.GetAttribute(oXmlNode, "language")
			If Language = "" Then
				Language = Me.GetAttribute(oXmlNode, "lang")
			End If
			Return New SCORM_LangString(oXmlNode.InnerText, Language)
		End Function

		Private Function ParseMetaData_General_SubKey(ByVal oXmlNode As XmlNode) As List(Of SCORM_LangString)
			Dim oLista As New List(Of SCORM_LangString)

			For Each oNode As XmlNode In oXmlNode.ChildNodes
				oLista.Add(Me.GetLangString(oNode))
			Next
			Return oLista
		End Function
		Private Function ParseMetaData_General_SourceValue(ByVal oXmlNode As XmlNode) As SCORM_SourceValue
            Dim oSourceValue As SCORM_SourceValue
			Dim oSource, oValue As SCORM_LangString

			For Each oNode As XmlNode In oXmlNode.ChildNodes
				If oNode.Name = "source" Then
					oSource = Me.GetLangString(oNode)
				ElseIf oNode.Name = "value" Then
					oValue = Me.GetLangString(oNode)
				End If
			Next
			oSourceValue = New SCORM_SourceValue(oSource, oValue)
			Return oSourceValue
		End Function
		Private Function ParseMetaData_General_Contribute(ByVal oXmlNode As XmlNode) As SCORM_Contribute
			Dim oContribute As SCORM_Contribute
			Dim oRole As SCORM_SourceValue
			Dim oDate As Date
			Dim iList As New List(Of String)

			For Each oNode As XmlNode In oXmlNode.ChildNodes
				If oNode.Name = "role" Then
					oRole = Me.ParseMetaData_General_SourceValue(oNode)
				ElseIf oNode.Name = "entity" Or oNode.Name = "centity" Then
					iList.Add(oNode.InnerText)
				ElseIf oNode.Name = "date" Then
					Try
						oDate = CDate(oNode.InnerText)
					Catch ex As Exception
						oDate = Nothing
					End Try
				End If
			Next
			oContribute = New SCORM_Contribute(oRole, iList, oDate)
			Return oContribute
		End Function
#End Region
	
	End Class
End Namespace