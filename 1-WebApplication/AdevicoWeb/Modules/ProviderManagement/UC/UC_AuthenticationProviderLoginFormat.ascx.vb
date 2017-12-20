Public Class UC_AuthenticationProviderLoginFormat
    Inherits BaseControl

#Region "Property"
    Public Property IdLoginFormat As Long
        Get
            Return Me.ViewStateOrDefault("IdLoginFormat", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("IdLoginFormat") = value
        End Set
    End Property
    Public Property CurrentLoginFormat As lm.Comol.Core.BaseModules.ProviderManagement.dtoLoginFormat
        Get
            Dim loginFormat As New lm.Comol.Core.BaseModules.ProviderManagement.dtoLoginFormat

            loginFormat.Id = IdLoginFormat
            loginFormat.After = Me.TXBtextAfter.Text
            loginFormat.Before = Me.TXBtextBefore.Text
            loginFormat.isDefault = Me.CBXloginFormatDefault.Checked
            loginFormat.Name = Me.TXBname.Text
           
            Return loginFormat
        End Get
        Set(value As lm.Comol.Core.BaseModules.ProviderManagement.dtoLoginFormat)
            IdLoginFormat = value.Id
            Me.CBXloginFormatDefault.Checked = value.isDefault
            Me.TXBtextAfter.Text = value.After
            Me.TXBtextBefore.Text = value.Before
            Me.TXBname.Text = value.Name
        End Set
    End Property
    Public Property isInitialized As Boolean
        Get
            Return Me.ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("isInitialized") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Property UpdateContainer As Boolean
        Get
            Return ViewStateOrDefault("UpdateContainer", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("UpdateContainer") = value
        End Set
    End Property
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
        MyBase.SetCulture("pg_AuthenticationProvider", "Modules", "ProviderManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBloginFormatName_t)
            .setLabel(LBloginFormatDefault_t)
            .setLabel(LBloginFormatTextBefore_t)
            .setLabel(LBloginFormatTextAfter_t)
            .setCheckBox(CBXloginFormatDefault)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(loginFormat As lm.Comol.Core.BaseModules.ProviderManagement.dtoLoginFormat)
        Me.isInitialized = True
        Me.CurrentLoginFormat = loginFormat
    End Sub
End Class