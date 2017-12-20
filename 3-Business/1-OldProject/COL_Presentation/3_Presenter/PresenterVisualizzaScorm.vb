Imports COL_BusinessLogic_v2.Comol.Materiale.Scorm

Public Class PresenterVisualizzaScorm
	Inherits GenericPresenter

	Public Sub New(ByVal view As IviewVisualizzaScorm)
		MyBase.view = view
	End Sub
	Private Shadows ReadOnly Property View() As IviewVisualizzaScorm
		Get
			View = MyBase.view
		End Get
	End Property

	Public Sub Init()
		If Me.View.ScormUniqueID > 0 Then
			Me.View.PackageDownLoadUrl = "FileID=" & Me.View.ScormUniqueID
			Me.CreateTreeView()
		Else
			Me.View.ShowErrorMessager(Me.View.Resource.getValue("FileNotFound"))
		End If
	End Sub

	Private Sub CreateTreeView()
		Dim oPackage As SCORM_Package
		Dim oScormTranslator As New SCORM_XMLtranslator
		Dim Percorso As String = ""

		oPackage = oScormTranslator.LoadPackage(Me.View.ScormUniqueID, Me.View.DrivePath, "imsmanifest.xml")

		Percorso = Me.View.ComunitaLavoroID & "/" & Me.View.ScormUniqueID & "/" & oPackage.UrlBase
		Percorso = Replace(Percorso, "//", "/")
		Percorso = Replace(Percorso, "\/", "/")
		Percorso = Replace(Percorso, "\", "/")

		'Dim oMail As New COL_E_Mail(Me.View.LocalizedMail)

		'oMail.IndirizziTO.Add(Me.View.LocalizedMail.SendErrorTo)
		'oMail.Mittente = Me.View.LocalizedMail.ErrorSender
		'oMail.Oggetto = "scorm"
		'oMail.Body = "Me.View.DrivePath=" & Me.View.DrivePath & vbCrLf & "ScormPackageBasePath=" & Percorso

		''oMail.InviaMailHTML()

		Me.View.ScormPackageBasePath = Percorso
		Me.View.ScormPackage = Me.CreateWebTreeView(oPackage.GetWebTreeNode(Me.View.LinguaCode))
	End Sub

	Public Sub ShowResources(ByVal ResourceID As String)
		Dim oPackage As SCORM_Package
		Dim oScormTranslator As New SCORM_XMLtranslator
		Dim oResource As SCORM_Resource

		oPackage = oScormTranslator.LoadPackage(Me.View.ScormUniqueID, Me.View.DrivePath, "imsmanifest.xml")
		oResource = oPackage.FindResource(ResourceID)

		If IsNothing(oResource) Then
			Me.View.ShowResourceList(Nothing)
		Else
			Me.View.ShowResourceList(oResource.GetAllFiles)
		End If
	End Sub

	Private Function CreateWebTreeView(ByVal oScormNode As SCORM_TreeNode) As System.Web.UI.WebControls.TreeNode
		Dim oTreeNode As New TreeNode

		oTreeNode.Expanded = oScormNode.Expanded
		If oScormNode.isSystemNode Then
			oTreeNode.Value = oScormNode.Value
			Select Case oScormNode.NodeType
				Case SCORM_TreeNodeType.Organization
					oTreeNode.ImageUrl = Me.View.OrganizationsImage
					'oTreeNode.Text = "Organizations"
					oTreeNode.Text = Me.View.Resource.getValue("TipoNodo." & CType(oScormNode.NodeType, SCORM_TreeNodeType))
				Case SCORM_TreeNodeType.Package
					oTreeNode.Text = oScormNode.Text
					If oTreeNode.Text <> "" Then
						oTreeNode.Text = Me.View.Resource.getValue("TipoNodo." & CType(oScormNode.NodeType, SCORM_TreeNodeType))
					End If
					oTreeNode.ImageUrl = Me.View.PackageImage
				Case SCORM_TreeNodeType.InfoManifest
					oTreeNode.Text = Me.View.Resource.getValue("TipoNodo." & CType(oScormNode.NodeType, SCORM_TreeNodeType)) '  "Informazioni"
				Case SCORM_TreeNodeType.InvalidPackage
					'oTreeNode.Text = "Invalid scorm package"
					oTreeNode.Text = Me.View.Resource.getValue("TipoNodo." & CType(oScormNode.NodeType, SCORM_TreeNodeType))
			End Select
			'oTreeNode.Text = oScormNode.Value
		Else
			oTreeNode.Text = oScormNode.Text
			oTreeNode.Value = oScormNode.Value
			If oScormNode.NodeType = SCORM_TreeNodeType.Item Then
				If oScormNode.ChildNodes.Count = 0 Then
					oTreeNode.ImageUrl = Me.View.ItemImage
				Else
					oTreeNode.ImageUrl = Me.View.ItemsImage
				End If
			ElseIf oScormNode.NodeType = SCORM_TreeNodeType.Organization Then
				oTreeNode.ImageUrl = Me.View.OrganizationsImage
			End If
		End If
		For Each oChild As SCORM_TreeNode In oScormNode.ChildNodes
			Try
				Dim oCreato As TreeNode = CreateWebTreeView(oChild)
				oTreeNode.ChildNodes.Add(oCreato)
			Catch ex As Exception

			End Try
		Next
		Return oTreeNode
	End Function
End Class