Imports lm.Comol.Modules.Base.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common


Public Class UC_ModuleToCallForPapersAction
    Inherits BaseControl
    Implements iModuleActionView



#Region "Inherited"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherited"
    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

    Public Sub DisplayRemovedObject() Implements lm.Comol.Core.DomainModel.Common.iModuleActionView.DisplayRemovedObject

    End Sub

    Public Sub InitializeControlByLink(ByVal pLink As lm.Comol.Core.DomainModel.ModuleLink) Implements lm.Comol.Core.DomainModel.Common.iModuleActionView.InitializeControlByLink

    End Sub

    Public Function InitializeRemoteControlByLink(ByVal DescriptionOnly As Boolean, ByVal pLink As lm.Comol.Core.DomainModel.ModuleLink) As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoModuleActionControl) Implements lm.Comol.Core.DomainModel.Common.iModuleActionView.InitializeRemoteControlByLink
        Return Nothing
    End Function


    Public Sub InitializeControlByActionType(ByVal pLink As lm.Comol.Core.DomainModel.ModuleLink, ByVal actionType As lm.Comol.Core.DomainModel.StandardActionType) Implements lm.Comol.Core.DomainModel.Common.iModuleActionView.InitializeControlByActionType

    End Sub

    Public Function getDescriptionByLink(ByVal pLink As lm.Comol.Core.DomainModel.ModuleLink) As String Implements lm.Comol.Core.DomainModel.Common.iModuleActionView.getDescriptionByLink
        Return "CallForPaper"

    End Function

    Public Function GetInLineDescriptionByLink(link As lm.Comol.Core.DomainModel.ModuleLink) As String Implements lm.Comol.Core.DomainModel.Common.iModuleActionView.GetInLineDescriptionByLink
        Return "CallForPaper"
    End Function

    Public Sub InitializeControlInlineByLink(pLink As lm.Comol.Core.DomainModel.ModuleLink) Implements lm.Comol.Core.DomainModel.Common.iModuleActionView.InitializeControlInlineByLink

    End Sub

    Public Function InitializeRemoteControlInlineByLink(DescriptionOnly As Boolean, pLink As lm.Comol.Core.DomainModel.ModuleLink) As List(Of dtoModuleActionControl) Implements lm.Comol.Core.DomainModel.Common.iModuleActionView.InitializeRemoteControlInlineByLink
        Return New List(Of dtoModuleActionControl)
    End Function

    Public Sub InitializeControlForUserByActionType(pLink As lm.Comol.Core.DomainModel.ModuleLink, idUser As Integer, actionType As lm.Comol.Core.DomainModel.StandardActionType) Implements lm.Comol.Core.DomainModel.Common.iModuleActionView.InitializeControlForUserByActionType

    End Sub

    Public Sub InitializeControlInlineForUserByLink(pLink As lm.Comol.Core.DomainModel.ModuleLink, idUser As Integer) Implements lm.Comol.Core.DomainModel.Common.iModuleActionView.InitializeControlInlineForUserByLink

    End Sub

    Public Function InitializeRemoteControlInlineForUserByLink(DescriptionOnly As Boolean, pLink As lm.Comol.Core.DomainModel.ModuleLink, idUser As Integer) As List(Of dtoModuleActionControl) Implements iModuleActionView.InitializeRemoteControlInlineForUserByLink
        Return New List(Of dtoModuleActionControl)
    End Function
End Class