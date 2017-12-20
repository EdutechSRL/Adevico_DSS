Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.UCServices
Imports System.Linq

Partial Public Class UC_WorkBookData
    Inherits BaseControlSession

#Region "Property"
	Public Property ShowReadonly() As Boolean
		Get
			Return Me.TXBtitle.ReadOnly
		End Get
		Set(ByVal value As Boolean)
			Me.TXBtitle.ReadOnly = value
            Me.CTRLeditorNote.IsEnabled = value
            Me.CTRLeditorText.IsEnabled = value
		End Set
	End Property
	Private Property WorkBookID() As Guid
		Get
			If TypeOf Me.ViewState("WorkBookID") Is System.Guid Then
				Return Me.ViewState("WorkBookID")
			Else
				Me.ViewState("WorkBookID") = System.Guid.Empty
			End If
		End Get
		Set(ByVal value As Guid)
			Me.ViewState("WorkBookID") = value
		End Set
	End Property
	Public Property WorkBookCommunityID() As Integer
		Get
			Return Me.ViewState("WorkBookCommunityID")
		End Get
		Set(ByVal value As Integer)
			Me.ViewState("WorkBookCommunityID") = value
		End Set
	End Property
	Public Property Mode() As ModeView
		Get
			Return Me.ViewState("Mode")
		End Get
		Set(ByVal value As ModeView)
			Me.ViewState("Mode") = value
		End Set
	End Property
	Private Property isPersonal() As Boolean
		Get
			Return Me.ViewState("WKisPersonal")
		End Get
		Set(ByVal value As Boolean)
			Me.ViewState("WKisPersonal") = value
		End Set
	End Property
	Private Property Type() As WorkBookType
		Get
			Return Me.ViewState("WKtype")
		End Get
		Set(ByVal value As WorkBookType)
			Me.ViewState("WKtype") = value
		End Set
	End Property
#End Region

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
        End If
	End Sub

    Public Sub LoadWorkBook(ByVal oDTO As dtoWorkBook, ByVal oAvailableStatus As List(Of TranslatedItem(Of Integer)))
        Dim oWorkBook As WorkBook = oDTO.WorkBook
        Dim oPermission As iWorkBookPermission = oDTO.Permission

        Try
            Me.DDLstatus.Items.Clear()
            Me.DDLstatus.DataSource = oAvailableStatus
            Me.DDLstatus.DataTextField = "Translation"
            Me.DDLstatus.DataValueField = "Id"
            Me.DDLstatus.DataBind()
            Me.DDLstatus.SelectedValue = oWorkBook.Status.Id
        Catch ex As Exception

        End Try
        If Me.DDLstatus.SelectedIndex >= 0 Then
            Me.LBverificato.Text = Me.DDLstatus.SelectedItem.Text
        End If

        If oWorkBook.Title = "" Then
            Me.TXBtitle.Text = "TEST TEST TEST"
        Else
            Me.TXBtitle.Text = oWorkBook.Title
        End If
        Me.CTRLeditorNote.HTML = oWorkBook.Note
        Me.CTRLeditorText.HTML = oWorkBook.Text
        Me.WorkBookID = oWorkBook.Id

        If IsNothing(oWorkBook.CommunityOwner) Then
            Me.WorkBookCommunityID = 0
        Else
            Me.WorkBookCommunityID = oWorkBook.CommunityOwner.Id
        End If

        Me.isPersonal = oWorkBook.isPersonal
        Me.Type = oWorkBook.Type

        If oWorkBook.Id = System.Guid.Empty Then
            Me.LBowner.Text = String.Format(Me.Resource.getValue("Creatingowner"), oWorkBook.CreatedBy.SurnameAndName)
        Else
            Me.Resource.setLabel(Me.LBowner)
            Me.LBowner.Text = String.Format(Me.Resource.getValue("createdHeader"), oWorkBook.CreatedBy.SurnameAndName, oWorkBook.CreatedOn.Value.ToString("dd/MM/yy"), oWorkBook.CreatedOn.Value.ToString("HH:mm"))
            If Not oWorkBook.ModifiedOn.Equals(New Date) Then
                If oWorkBook.isDeleted Then
                    Me.LBowner.Text &= " " & String.Format(MyBase.Resource.getValue("deletedHeader"), oWorkBook.ModifiedOn.ToString("dd/MM/yy"), oWorkBook.ModifiedOn.ToString("HH:mm"), oWorkBook.ModifiedBy.SurnameAndName)
                ElseIf oWorkBook.ModifiedBy Is oWorkBook.CreatedBy Then
                    Me.LBowner.Text &= " " & String.Format(Me.Resource.getValue("selfchangedHeader"), oWorkBook.ModifiedOn.ToString("dd/MM/yy"), oWorkBook.ModifiedOn.ToString("HH:mm"), oWorkBook.ModifiedBy.SurnameAndName)
                Else
                    Me.LBowner.Text &= " " & String.Format(Me.Resource.getValue("changedHeader"), oWorkBook.ModifiedOn.ToString("dd/MM/yy"), oWorkBook.ModifiedOn.ToString("HH:mm"), oWorkBook.ModifiedBy.SurnameAndName)
                End If
            End If
        End If
        Me.CBXdraft.Checked = oWorkBook.isDraft
        Me.LTtype.Text = Me.Resource.getValue("WorkBookType." & oWorkBook.Type)
    End Sub
    Protected Friend Function GetWorkBook() As lm.Comol.Modules.Base.DomainModel.WorkBook
        Dim oWorkBook As New lm.Comol.Modules.Base.DomainModel.WorkBook
        If WorkBookID = System.Guid.Empty Then
            oWorkBook.Id = System.Guid.Empty
        Else
            oWorkBook.Id = Me.WorkBookID
        End If
        oWorkBook.Title = Me.TXBtitle.Text
        oWorkBook.Text = Me.CTRLeditorText.HTML
        oWorkBook.Note = Me.CTRLeditorNote.HTML
        oWorkBook.isDraft = Me.CBXdraft.Checked
        If Me.DDLstatus.SelectedIndex >= 0 Then
            oWorkBook.Status = New WorkBookStatus(Me.DDLstatus.SelectedValue)
        Else
            oWorkBook.Status = Nothing
        End If
        If Me.DDLediting.SelectedIndex >= 0 Then
            oWorkBook.Editing = Me.DDLediting.SelectedValue
        Else
            oWorkBook.Editing = EditingPermission.Authors
        End If
        Return oWorkBook
    End Function

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WorkBookEdit", "Generici")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(Me.LBtext)
            .setLabel(Me.LBtitle)
            .setLabel(Me.LBnote)
            .setLabel(Me.LBtype)

            .setLabel(Me.LBverificato_t)
            .setLabel(Me.LBowner_t)
            .setLabel(Me.LBdraft_t)
            .setCheckBox(CBXdraft)
            .setLabel(LBediting_t)
        End With
    End Sub

	Public Enum ModeView
		Editing
		Creating
    End Enum

    Public WriteOnly Property SetStatus() As lm.Comol.Core.DomainModel.MetaApprovationStatus
        Set(ByVal value As lm.Comol.Core.DomainModel.MetaApprovationStatus)
            Me.LBverificato_t.Text = MyBase.Resource.getValue("status." & value)
        End Set
    End Property

    Public Sub SetEditing(ByVal oAvailableEditing As List(Of TranslatedItem(Of Integer)), ByVal ItemEditing As EditingPermission)
        Me.DDLediting.Items.Clear()
        Me.DDLediting.DataSource = oAvailableEditing
        Me.DDLediting.DataValueField = "Id"
        Me.DDLediting.DataTextField = "Translation"
        Me.DDLediting.DataBind()
        Try
            Me.DDLediting.SelectedValue = ItemEditing
        Catch ex As Exception

        End Try
        If Me.DDLediting.SelectedIndex > -1 Then
            Me.LBediting.Text = Me.DDLediting.SelectedItem.Text
        End If
    End Sub

    Public ReadOnly Property GetEditingTranslation(ByVal Permissions As Integer) As String
        Get
            Return Me.Resource.getValue("EditingSettings." & Permissions.ToString)
        End Get
    End Property

    Public WriteOnly Property AllowEditingChanging() As Boolean
        Set(ByVal value As Boolean)
            Me.DDLediting.Visible = value
            Me.LBediting.Visible = Not value
        End Set
    End Property
    Public Property AllowStatusChange() As Boolean
        Get
            Return Me.DDLstatus.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.DDLstatus.Visible = value
            Me.LBverificato.Visible = Not value
        End Set
    End Property
End Class