Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers

Public Class UC_EditHeaderFooter
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event DeleteOldElement(ByVal Id As Int64)
    Public Event DeleteOldElements(ByVal Ids As IList(Of Int64))

    Public Event RecoverLeft(ByVal Id As Int64)
    Public Event RecoverCenter(ByVal Id As Int64)
    Public Event RecoverRight(ByVal Id As Int64)

    Public Property LeftElement As TemplateVers.PageElement
        Get
            Return Me.UCleft.Item
        End Get
        Private Set(value As TemplateVers.PageElement)
            Me.UCleft.Item = value
        End Set
    End Property
    Public Property CenterElement As TemplateVers.PageElement
        Get
            Return Me.UCcenter.Item
        End Get
        Private Set(value As TemplateVers.PageElement)
            Me.UCcenter.Item = value
        End Set
    End Property
    Public Property RightElement As TemplateVers.PageElement
        Get
            Return Me.UCright.Item
        End Get
        Private Set(value As TemplateVers.PageElement)
            Me.UCright.Item = value
        End Set
    End Property
    Private _TemplateimgBasePath As String
    Public Property TemplateImageBasePath() As String
        Get
            If String.IsNullOrEmpty(_TemplateimgBasePath) Then
                _TemplateimgBasePath = Me.ViewState("LocalImgBasePath")
            End If
            Return _TemplateimgBasePath
        End Get
        Set(value As String)
            _TemplateimgBasePath = value
            Me.ViewState("LocalImgBasePath") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templateEdit", "Modules", "DocTemplates")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLiteral(LITlegendLeft)
            .setLiteral(LITlegendCenter)
            .setLiteral(LITlegendRight)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub SetElement( _
                      ByVal LeftElement As TemplateVers.PageElement, _
                      ByVal CenterElement As TemplateVers.PageElement, _
                      ByVal RightElement As TemplateVers.PageElement, _
                      ByVal TemplateId As Int64, _
                      ByVal VersionId As Int64)

        Me.UCleft.SetTemplteVersion(TemplateId, VersionId)
        Me.UCcenter.SetTemplteVersion(TemplateId, VersionId)
        Me.UCright.SetTemplteVersion(TemplateId, VersionId)

        Me.LeftElement = LeftElement
        Me.RightElement = RightElement
        Me.CenterElement = CenterElement

    End Sub
    Public Sub SetLeftElemt(ByVal Element As TemplateVers.PageElement, _
                             ByVal TemplateId As Int64, _
                          ByVal VersionId As Int64)
        Me.UCleft.SetTemplteVersion(TemplateId, VersionId)
        Me.LeftElement = Element
    End Sub
    Public Sub SetCenterElemt(ByVal Element As TemplateVers.PageElement, _
                             ByVal TemplateId As Int64, _
                          ByVal VersionId As Int64)
        Me.UCcenter.SetTemplteVersion(TemplateId, VersionId)
        Me.CenterElement = Element
    End Sub
    Public Sub SetRightElemt(ByVal Element As TemplateVers.PageElement, _
                             ByVal TemplateId As Int64, _
                          ByVal VersionId As Int64)
        Me.UCright.SetTemplteVersion(TemplateId, VersionId)
        Me.RightElement = Element
    End Sub

    Public Sub SetRevision(ByVal LeftItems As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditPreviousVersion), ByVal CenterItems As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditPreviousVersion), ByVal RightItems As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditPreviousVersion))
        Me.SetRevisionLeft(LeftItems)
        Me.SetRevisionCenter(CenterItems)
        Me.SetRevisionRight(RightItems)
    End Sub
    Public Sub SetRevisionLeft(ByVal Items As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditPreviousVersion))

        If Not IsNothing(Items) AndAlso Items.Count > 0 Then
            Me.UCprevVersionLeft.Visible = True
            Me.UCprevVersionLeft.BindList(Items)
        Else
            Me.UCprevVersionLeft.Visible = False
        End If
    End Sub
    Public Sub SetRevisionCenter(ByVal Items As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditPreviousVersion))

        If Not IsNothing(Items) AndAlso Items.Count > 0 Then
            Me.UCprevVersionCenter.Visible = True
            Me.UCprevVersionCenter.BindList(Items)
        Else
            Me.UCprevVersionCenter.Visible = False
        End If
    End Sub
    Public Sub SetRevisionRight(ByVal Items As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditPreviousVersion))

        If Not IsNothing(Items) AndAlso Items.Count > 0 Then
            Me.UCprevVersionRight.Visible = True
            Me.UCprevVersionRight.BindList(Items)
        Else
            Me.UCprevVersionRight.Visible = False
        End If
    End Sub
#End Region

#Region "Handler"
    Private Sub UCprevVersionLeft_DeleteItem(Id As Long) Handles UCprevVersionLeft.DeleteItem
        RaiseEvent DeleteOldElement(Id)
    End Sub
    Private Sub UCprevVersionCenter_DeleteItem(Id As Long) Handles UCprevVersionCenter.DeleteItem
        RaiseEvent DeleteOldElement(Id)
    End Sub
    Private Sub UCprevVersionRight_DeleteItem(Id As Long) Handles UCprevVersionRight.DeleteItem
        RaiseEvent DeleteOldElement(Id)
    End Sub
    Private Sub UCprevVersionLeft_DeleteItems(Ids As System.Collections.Generic.IList(Of Long)) Handles UCprevVersionLeft.DeleteItems
        RaiseEvent DeleteOldElements(Ids)
    End Sub
    Private Sub UCprevVersionCenter_DeleteItems(Ids As System.Collections.Generic.IList(Of Long)) Handles UCprevVersionCenter.DeleteItems
        RaiseEvent DeleteOldElements(Ids)
    End Sub
    Private Sub UCprevVersionRight_DeleteItems(Ids As System.Collections.Generic.IList(Of Long)) Handles UCprevVersionRight.DeleteItems
        RaiseEvent DeleteOldElements(Ids)
    End Sub
    Private Sub UCprevVersionLeft_RecoverItem(Id As Long) Handles UCprevVersionLeft.RecoverItem
        RaiseEvent RecoverLeft(Id)
    End Sub
    Private Sub UCprevVersionCenter_RecoverItem(Id As Long) Handles UCprevVersionCenter.RecoverItem
        RaiseEvent RecoverCenter(Id)
    End Sub
    Private Sub UCprevVersionRight_RecoverItem(Id As Long) Handles UCprevVersionRight.RecoverItem
        RaiseEvent RecoverLeft(Id)
    End Sub
#End Region

End Class