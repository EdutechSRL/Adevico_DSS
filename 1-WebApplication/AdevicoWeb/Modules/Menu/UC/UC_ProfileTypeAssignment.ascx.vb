Imports lm.Comol.Modules.Standard.Menu.Domain
Imports lm.Comol.Modules.Standard.Menu.Presentation
Public Class UC_ItemProfileTypeAssignment
    Inherits BaseControl
    Implements IViewProfileTypeAssignments


#Region "Implements"
    Public ReadOnly Property GetSelectedTypes As List(Of Integer) Implements IViewProfileTypeAssignments.GetSelectedTypes
        Get
            If Me.RPTprofileTypes.Items.Count = 0 Then
                Return New List(Of Integer)
            Else
                Return (From r As RepeaterItem In RPTprofileTypes.Items _
                  Where ((r.ItemType = ListItemType.AlternatingItem) OrElse (r.ItemType = ListItemType.Item)) AndAlso (DirectCast(r.FindControl("HTCprofileType"), HtmlInputCheckBox).Checked) Select GetProfileId(r)).ToList
            End If
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_MenubarEdit", "Modules", "Menu")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBprofileTypes_t)
        End With
    End Sub
#End Region

    Public Sub InitalizeControl(selectedTypes As List(Of Integer), availableTypes As List(Of Integer), allowEdit As Boolean) Implements IViewProfileTypeAssignments.InitalizeControl
        LoadProfileTypes(selectedTypes, availableTypes, allowEdit)
    End Sub

    Private Sub LoadProfileTypes(selectedTypes As List(Of Integer), availableTypes As List(Of Integer), allowEdit As Boolean)
        Try
            Dim oUserTypes As List(Of COL_TipoPersona)
            If availableTypes Is Nothing Then
                oUserTypes = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Order By o.Descrizione Select o).ToList
            Else
                oUserTypes = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where availableTypes.Contains(o.ID) Order By o.Descrizione Select o).ToList
            End If

            Me.RPTprofileTypes.DataSource = oUserTypes
            Me.RPTprofileTypes.DataBind()
        Catch ex As Exception

        End Try
        Dim list As List(Of HtmlInputCheckBox) = (From r As RepeaterItem In RPTprofileTypes.Items _
                 Where (r.ItemType = ListItemType.AlternatingItem) OrElse (r.ItemType = ListItemType.Item) Select DirectCast(r.FindControl("HTCprofileType"), HtmlInputCheckBox)).ToList

        'Dim test As List(Of HtmlInputCheckBox) = list.Where(Function(c) selectedTypes.Contains(CInt(c.Value))).ToList()
        'test.ForEach(Function(c) c.Checked = True)

        For Each item As HtmlInputCheckBox In list
            item.Checked = selectedTypes.Contains(CInt(item.Value))
            item.Disabled = Not allowEdit
        Next

        'list.ForEach(Function(c) c.Disabled = Not allowEdit)
    End Sub

    Private Function GetProfileId(row As RepeaterItem) As Integer
        Return CInt(DirectCast(row.FindControl("HTCprofileType"), HtmlInputCheckBox).Value)
    End Function

End Class