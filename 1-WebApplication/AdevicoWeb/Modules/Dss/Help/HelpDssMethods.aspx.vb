Public Class HelpDssMethods
    Inherits PageBase

#Region "inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "inherits"
    Public Overrides Sub BindDati()
        If Not Page.IsPostBack Then
            Dim dto As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext

            Dim serviceSkin As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin = New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(PageUtility.CurrentContext)
            dto = serviceSkin.GetItemSkinSettings(PageUtility.CurrentContext.UserContext.CurrentCommunityID, PageUtility.CurrentContext.UserContext.CurrentCommunityID, lm.Comol.Modules.Standard.Skin.Domain.SkinDisplayType.CurrentCommunity)
            dto.ShowDocType = True
            Me.Master.InitializeMaster(dto)
        End If
      
    End Sub
    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AggregationSelector", "Modules", "Dss")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            Master.ServiceTitle = "Informazioni generali DSS"
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

   
    Private Sub HelpDssMethods_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub
End Class