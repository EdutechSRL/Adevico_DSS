Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers
Imports DTHelpers = lm.Comol.Core.BaseModules.DocTemplate.Helpers

Public Class UC_EditSignatures
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event DeletePrevSignature(ByVal Id As Int64)
    Public Event DeletePrevSignatures(ByVal Ids As IList(Of Int64))
    Public Event RecoverPrevSignature(ByVal Id As Int64)

    Private Ordering As IDictionary(Of Int64, Integer)
    Private _CurPos As Integer = 0

    Public Property Signatures As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature))
        Get
            Dim CurSignatures As New List(Of TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature))

            'ricreo il dictionari con le posizioni degli elementi...
            ResetPos()
            'RPTsignatures.Visible AndAlso
            If (Not IsNothing(RPTsignatures.Items) AndAlso RPTsignatures.Items.Count() > 0) Then

                For Each Ritem As RepeaterItem In Me.RPTsignatures.Items
                    If Ritem.ItemType = ListItemType.Item OrElse Ritem.ItemType = ListItemType.AlternatingItem Then
                        Dim UCsignature As UC_EditSignature = Ritem.FindControl("UCsignature")


                        If Not IsNothing(UCsignature) Then

                            Dim dtoSignatures As New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature)()

                            dtoSignatures.Data = UCsignature.signature
                            dtoSignatures.Data.Position = Me.GetPos(dtoSignatures.Data.Id)

                            CurSignatures.Add(dtoSignatures)

                        End If
                    End If
                Next

            End If

            Return CurSignatures
        End Get
        Set(value As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature)))
            If Not IsNothing(value) AndAlso value.Count() > 0 Then 'AndAlso value.Count() > 0 Then
                LBLnosign.Visible = False

                RPTsignatures.Visible = True
                Me.RPTsignatures.DataSource = value
                Me.RPTsignatures.DataBind()

            Else
                LBLnosign.Visible = True

                Me.RPTsignatures.DataSource = New List(Of TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature))()
                Me.RPTsignatures.DataBind()

                RPTsignatures.Visible = False
            End If

        End Set
    End Property
    Private ReadOnly Property NewSignId
        Get
            Dim NewId As Int64 = -1
            If (Not IsNothing(Me.ViewState("NewSgnId"))) Then
                Try
                    NewId = Me.ViewState("NewSgnId")
                    NewId -= 1
                Catch ex As Exception

                End Try
            End If

            Me.ViewState("NewSgnId") = NewId
            Return NewId
        End Get
    End Property
    Public Property TemplateId As Integer
        Get
            Dim TmplId As Int64 = 0
            Try
                TmplId = Me.ViewState("CurTmplId")
            Catch ex As Exception
            End Try
            Return TmplId

        End Get
        Set(value As Integer)
            Me.ViewState("CurTmplId") = value
        End Set
    End Property
    Public Property VersionId As Integer
        Get
            Dim VrsId As Int64 = 0
            Try
                VrsId = Me.ViewState("CurVrsId")
            Catch ex As Exception
            End Try
            Return VrsId

        End Get
        Set(value As Integer)
            Me.ViewState("CurVrsId") = value
        End Set
    End Property
    Private ReadOnly Property TemplateTmpFolder As String
        Get
            Return (MyBase.SystemSettings.DocTemplateSettings.BasePath & "\" & MyBase.SystemSettings.DocTemplateSettings.BaseTemporaryFolder & "\").Replace("/", "\").Replace("\\", "\")
        End Get
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
            '.setCheckBox(CBXuseSignatures)
            .setLabel(LBLnosign)
            .setLiteral(LITrevision_t)
            .setLinkButton(LKBaddSignature, True, True, False, False)
        End With
    End Sub
#End Region

#Region "Internal"
    Private Sub OnDeleteSignature(ByVal SignatureKey As String)
        Dim CurSignatures As New List(Of TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature))

        Dim Pos As Short = 0

        For Each Ritem As RepeaterItem In Me.RPTsignatures.Items
            If Ritem.ItemType = ListItemType.Item OrElse Ritem.ItemType = ListItemType.AlternatingItem Then
                Dim UCsignature As UC_EditSignature = Ritem.FindControl("UCsignature")

                If Not IsNothing(UCsignature) Then
                    If Not (UCsignature.CurrentKey = SignatureKey) Then
                        Dim dtoSignatures As New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature)()
                        dtoSignatures.Data = UCsignature.signature
                        dtoSignatures.Data.Placing = Pos
                        Pos += 1
                        CurSignatures.Add(dtoSignatures)
                    Else
                        Dim sgn As TemplateVers.Signature = UCsignature.signature
                        If (sgn.HasImage AndAlso Not String.IsNullOrEmpty(sgn.Path) AndAlso sgn.Path.StartsWith("#")) Then
                            RemoveOldFile(sgn.Path.Remove(0, 1))
                        End If
                    End If
                End If
            End If
        Next

        Me.Signatures = CurSignatures

    End Sub
    Public Sub SetTemplteVersion(ByVal TemplateId As Int64, ByVal VersionId As Int64)
        Me.TemplateId = TemplateId
        Me.VersionId = VersionId
    End Sub
    Private Sub RemoveOldFile(ByVal fileName As String)
        DTHelpers.DocTempalteFileHelper.DeleteFile(TemplateVers.Business.ImageHelper.GetImagePath(fileName.Replace("#", ""), Me.TemplateTmpFolder, Me.TemplateId, Me.VersionId))
    End Sub
    Public Sub BindPrevVersion(ByVal PrevVersions As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditPreviousVersion))

        If Not IsNothing(PrevVersions) AndAlso PrevVersions.Count > 0 Then
            Me.UCprevVersion.BindList(PrevVersions)
            Me.PNLsubVersion.Visible = True
        Else
            Me.PNLsubVersion.Visible = False
        End If

    End Sub
    Private Sub UCprevVersion_DeleteItem(Id As Long) Handles UCprevVersion.DeleteItem
        RaiseEvent DeletePrevSignature(Id)
    End Sub
    Private Sub UCprevVersion_DeleteItems(Ids As System.Collections.Generic.IList(Of Long)) Handles UCprevVersion.DeleteItems
        RaiseEvent DeletePrevSignatures(Ids)
    End Sub
    Private Sub UCprevVersion_RecoverItem(Id As Long) Handles UCprevVersion.RecoverItem
        RaiseEvent RecoverPrevSignature(Id)
    End Sub
    Public Sub AddRecSignature(ByVal NewSign As TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature))

        If Not IsNothing(NewSign) AndAlso Not IsNothing(NewSign.Data) Then


            Dim CurrentSignatures As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature)) = Me.Signatures

            If IsNothing(CurrentSignatures) Then
                CurrentSignatures = New List(Of TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature))
            End If

            NewSign.Data.Placing = Me.RPTsignatures.Items.Count

            CurrentSignatures.Add(NewSign)

            Me.Signatures = CurrentSignatures
        End If

    End Sub
    Private Sub ResetPos()
        _CurPos = 0
        If Not String.IsNullOrEmpty(Me.HF_Order.Value) Then
            Dim Orders() As String = Me.HF_Order.Value.Split(",")

            Dim i As Integer = 1


            Ordering = New Dictionary(Of Int64, Integer)

            For Each Order As String In Orders
                Dim ID As Int64

                If Not String.IsNullOrEmpty(Order) Then
                    Try
                        ID = System.Convert.ToInt64(Order.Replace("sgn_", ""))
                    Catch ex As Exception
                        ID = -50
                    End Try

                    If (ID > 0) Then
                        Ordering.Add(ID, i)
                        i += 1
                    End If

                End If

            Next
        End If
    End Sub
    Private Function GetPos(Id As Int64) As Integer
        If IsNothing(Ordering) Then
            ResetPos()
        End If

        If IsNothing(Ordering) OrElse Not Ordering.ContainsKey(Id) Then
            _CurPos += 1
            Return _CurPos
        Else
            Return Ordering(Id)
        End If
    End Function
#End Region

#Region "Handler"
    Private Sub RPTsignatures_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTsignatures.ItemCommand
        If e.CommandName = "DeleteSignature" Then
            Me.OnDeleteSignature(e.CommandArgument)
        End If
    End Sub
    Private Sub RPTsignatures_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsignatures.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim DTO_Sign As TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature) = e.Item.DataItem

            If Not IsNothing(DTO_Sign) AndAlso Not IsNothing(DTO_Sign.Data) Then
                Dim UCsignature As UC_EditSignature = e.Item.FindControl("UCsignature")

                If Not IsNothing(UCsignature) Then
                    If (Me.TemplateId <= 0 OrElse Me.VersionId <= 0) Then
                        Throw New NullReferenceException("Id Template or Id Version cannot be null or <= 0 in UC_EditSignatures-ascx.vb : 69")
                    End If
                    UCsignature.SetSignature(DTO_Sign.Data, Me.TemplateId, Me.VersionId)
                    UCsignature.CurrentKey = e.Item.ItemIndex.ToString()
                End If
            End If
        End If
    End Sub
    Private Sub LKBaddSignature_Click(sender As Object, e As System.EventArgs) Handles LKBaddSignature.Click
        Dim sign As New TemplateVers.Signature()
        sign.HasImage = False
        sign.HasPDFPositioning = False
        sign.Height = 0
        sign.IsActive = True
        sign.IsHTML = True
        sign.Path = ""
        sign.Placing = Me.RPTsignatures.Items.Count
        sign.PosBottom = 0
        sign.Position = lm.Comol.Core.DomainModel.DocTemplateVers.SignaturePosition.center
        sign.PosLeft = 0
        sign.Text = ""
        sign.Width = 0

        sign.Id = Me.NewSignId

        Dim CurrentSignatures As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature)) = Me.Signatures

        If IsNothing(CurrentSignatures) Then
            CurrentSignatures = New List(Of TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature))
        End If

        Dim Dtosign As New TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.Signature)
        Dtosign.Data = sign

        CurrentSignatures.Add(Dtosign)

        Me.Signatures = CurrentSignatures

    End Sub
#End Region

End Class

