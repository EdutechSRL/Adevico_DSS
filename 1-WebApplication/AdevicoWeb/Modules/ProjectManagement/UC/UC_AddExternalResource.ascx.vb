Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Presentation
Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Public Class UC_AddExternalResource
    Inherits BaseControl
    Implements IViewAddExternalResources

#Region "Context"
    Private _Presenter As AddExternalResourcesPresenter
    Private ReadOnly Property CurrentPresenter() As AddExternalResourcesPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddExternalResourcesPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Settings"
    Public Property isInitialized As Boolean Implements IViewAddExternalResources.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property Rows As Integer Implements IViewAddExternalResources.Rows
        Get
            Return ViewStateOrDefault("Rows", 5)
        End Get
        Set(value As Integer)
            ViewState("Rows") = value
        End Set
    End Property
    Public Property AllowMail As Boolean Implements IViewAddExternalResources.AllowMail
        Get
            Return ViewStateOrDefault("AllowMail", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowMail") = value
            THmail.Visible = value
        End Set
    End Property
    Public Property DisplayDescription As Boolean Implements IViewAddExternalResources.DisplayDescription
        Get
            Return ViewStateOrDefault("DisplayDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayDescription") = value
            Me.DVdescription.Visible = value
        End Set
    End Property
    Public Property RaiseCommandEvents As Boolean Implements IViewAddExternalResources.RaiseCommandEvents
        Get
            Return ViewStateOrDefault("RaiseCommandEvents", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseCommandEvents") = value
        End Set
    End Property
#End Region

#End Region

#Region "Property"
    Public Property InModalWindow As Boolean
        Get
            Return ViewStateOrDefault("InModalWindow", True)
        End Get
        Set(value As Boolean)
            ViewState("InModalWindow") = value
            DVcommands.Visible = value
        End Set
    End Property
    Public ReadOnly Property OnLoadingTranslation As String
        Get
            Return Me.Resource.getValue("OnLoadingTranslation")
        End Get
    End Property

    Public Event CloseWindow()
    Public Event AddingResources(ByVal resources As List(Of dtoExternalResource))
    Public Event AddedResources(ByVal resources As List(Of ProjectResource))
    Private AllowEditing As Boolean = True
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
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(BTNcancelExternalResources, True)
            .setButton(BTNaddExternalResources, True)
            .setLiteral(LTlongName_t)
            .setLiteral(LTshortName_t)
            .setLiteral(LTmail_t)
            .setLabel(LBexternalResourcesListTitle)
                   
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(Optional description As String = "") Implements IViewAddExternalResources.InitializeControl
        Me.LBdescription.Text = description
        If DisplayDescription AndAlso String.IsNullOrEmpty(description) Then
            DisplayDescription = False
        End If
        CurrentPresenter.initView(Rows, AllowMail)
    End Sub
    Private Sub DisplayRows(rows As Integer, displayMail As Boolean, Optional editing As Boolean = True) Implements IViewAddExternalResources.DisplayRows
        AllowEditing = editing

        Me.RPTresources.DataSource = (From n As Integer In Enumerable.Range(1, rows) Select New dtoExternalResource())
        Me.RPTresources.DataBind()
    End Sub
    Public Function AddResources(idProject As Long, items As System.Collections.Generic.List(Of lm.Comol.Modules.Standard.ProjectManagement.Domain.dtoExternalResource), visibility As lm.Comol.Modules.Standard.ProjectManagement.Domain.ProjectVisibility, role As lm.Comol.Modules.Standard.ProjectManagement.Domain.ActivityRole) As System.Collections.Generic.List(Of lm.Comol.Modules.Standard.ProjectManagement.Domain.ProjectResource) Implements lm.Comol.Modules.Standard.ProjectManagement.Presentation.IViewAddExternalResources.AddResources

    End Function

    Private Function GetResources() As List(Of dtoExternalResource) Implements IViewAddExternalResources.GetResources
        Dim items As New List(Of dtoExternalResource)

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTresources.Items Select r).ToList()
            Dim dto As New dtoExternalResource
        
            With dto
                Dim oTextBox As TextBox = row.FindControl("TXBlongName")
                dto.LongName = oTextBox.Text
                oTextBox = row.FindControl("TXBshortName")
                dto.ShortName = oTextBox.Text
                oTextBox = row.FindControl("TXBmail")
                dto.Mail = oTextBox.Text
            End With
            If Not (String.IsNullOrEmpty(dto.ShortName) AndAlso String.IsNullOrEmpty(dto.LongName)) Then
                items.Add(dto)
            End If
        Next
        Return items
    End Function
#End Region

    Private Sub BTNcancel_Click(sender As Object, e As System.EventArgs) Handles BTNcancelExternalResources.Click
        If RaiseCommandEvents Then
            RaiseEvent CloseWindow()
        End If
    End Sub
    Private Sub BTNselect_Click(sender As Object, e As System.EventArgs) Handles BTNaddExternalResources.Click
        If RaiseCommandEvents Then
            RaiseEvent AddingResources(GetResources())
        End If
    End Sub

    Private Sub RPTresources_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTresources.ItemDataBound
        Dim oTextBox As TextBox = e.Item.FindControl("TXBlongName")
        oTextBox.ReadOnly = Not AllowEditing
        oTextBox = e.Item.FindControl("TXBshortName")
        oTextBox.ReadOnly = Not AllowEditing
        oTextBox = e.Item.FindControl("TXBmail")
        oTextBox.ReadOnly = Not AllowEditing
        Dim oCell As HtmlTableCell = e.Item.FindControl("TDmail")
        oCell.Visible = AllowMail
    End Sub
End Class