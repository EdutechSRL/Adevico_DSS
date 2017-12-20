Imports lm.Comol.Core.ModuleLinks
Imports lm.Comol.Core.DomainModel

Public Class UC_ExternalModuleDisplayAction
    Inherits BaseControl
    Implements IExternalModuleDisplayAction


#Region "Implements"
    Public Property ContainerCSS As String Implements IExternalModuleDisplayAction.ContainerCSS
        Get
            Return ViewStateOrDefault("ContainerCSS", "")
        End Get
        Set(value As String)
            ViewState("ContainerCSS") = value
        End Set
    End Property
    Public Property Display As DisplayActionMode Implements IExternalModuleDisplayAction.Display
        Get
            Return ViewStateOrDefault("Display", DisplayActionMode.defaultAction)
        End Get
        Set(value As DisplayActionMode)
            ViewState("Display") = value
        End Set
    End Property
    Public Property IconSize As lm.Comol.Core.DomainModel.Helpers.IconSize Implements IExternalModuleDisplayAction.IconSize
        Get
            Return ViewStateOrDefault("IconSize", DisplayActionMode.defaultAction)
        End Get
        Set(value As lm.Comol.Core.DomainModel.Helpers.IconSize)
            ViewState("IconSize") = value
        End Set
    End Property
    Public Property EnableAnchor As Boolean Implements IExternalModuleDisplayAction.EnableAnchor
        Get
            Return ViewStateOrDefault("EnableAnchor", False)
        End Get
        Set(value As Boolean)
            ViewState("EnableAnchor") = value
        End Set
    End Property
    Public Property ShortDescription As Boolean Implements IExternalModuleDisplayAction.ShortDescription
        Get
            Return ViewStateOrDefault("ShortDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("ShortDescription") = value
        End Set
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

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Implements"
    Public Sub DisplayRemovedObject() Implements IExternalModuleDisplayAction.DisplayRemovedObject
        If Me.PLHdisplayAction.Controls.Count = 1 AndAlso TypeOf Me.PLHdisplayAction.Controls(0) Is IGenericModuleDisplayAction Then
            DirectCast(Me.PLHdisplayAction.Controls(0), IGenericModuleDisplayAction).DisplayRemovedObject()
        End If
    End Sub

    Public Sub InitializeControl(idUser As Integer, initializer As dtoExternalModuleInitializer) Implements IExternalModuleDisplayAction.InitializeControl
        Dim oControl As IGenericModuleDisplayAction = LoadActionControl(initializer.Link)
        If Not oControl Is Nothing Then
            oControl.EnableAnchor = EnableAnchor
            oControl.ShortDescription = ShortDescription
            oControl.InitializeControl(idUser, dtoModuleDisplayActionInitializer.Create(initializer, Display, ContainerCSS, IconSize))
            Me.PLHdisplayAction.Controls.Add(oControl)
        Else
            Me.PLHdisplayAction.Controls.Clear()
        End If
    End Sub
    Public Sub InitializeControl(idUser As Integer, initializer As dtoExternalModuleInitializer, actionType As StandardActionType) Implements IExternalModuleDisplayAction.InitializeControl
        Dim oControl As IGenericModuleDisplayAction = LoadActionControl(initializer.Link)
        If Not oControl Is Nothing Then
            oControl.EnableAnchor = EnableAnchor
            oControl.ShortDescription = ShortDescription
            oControl.InitializeControl(idUser, dtoModuleDisplayActionInitializer.Create(initializer, Display, ContainerCSS, IconSize), actionType)
            Me.PLHdisplayAction.Controls.Add(oControl)
        Else
            Me.PLHdisplayAction.Controls.Clear()
        End If
    End Sub
    Public Sub InitializeControl(initializer As dtoExternalModuleInitializer) Implements IExternalModuleDisplayAction.InitializeControl
        Dim oControl As IGenericModuleDisplayAction = LoadActionControl(initializer.Link)
        If Not oControl Is Nothing Then
            oControl.EnableAnchor = EnableAnchor
            oControl.ShortDescription = ShortDescription
            oControl.InitializeControl(dtoModuleDisplayActionInitializer.Create(initializer, Display, ContainerCSS, IconSize))
            Me.PLHdisplayAction.Controls.Add(oControl)
        Else
            Me.PLHdisplayAction.Controls.Clear()
        End If
    End Sub
    Public Sub InitializeControl(initializer As dtoExternalModuleInitializer, actionType As StandardActionType) Implements IExternalModuleDisplayAction.InitializeControl
        Dim oControl As IGenericModuleDisplayAction = LoadActionControl(initializer.Link)
        If Not oControl Is Nothing Then
            oControl.EnableAnchor = EnableAnchor
            oControl.ShortDescription = ShortDescription
            oControl.InitializeControl(dtoModuleDisplayActionInitializer.Create(initializer, Display, ContainerCSS, IconSize), actionType)
            Me.PLHdisplayAction.Controls.Add(oControl)
        Else
            Me.PLHdisplayAction.Controls.Clear()
        End If
    End Sub

    Public Function InitializeRemoteControl(idUser As Integer, initializer As dtoExternalModuleInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl) Implements IExternalModuleDisplayAction.InitializeRemoteControl
        Dim actions As New List(Of dtoModuleActionControl)
        Dim oControl As IGenericModuleDisplayAction = LoadActionControl(initializer.Link)
        If Not oControl Is Nothing Then
            oControl.EnableAnchor = EnableAnchor
            oControl.ShortDescription = ShortDescription
            actions = oControl.InitializeRemoteControl(idUser, dtoModuleDisplayActionInitializer.Create(initializer, Display, ContainerCSS, IconSize), actionsToDisplay)
            Me.PLHdisplayAction.Controls.Add(oControl)
        Else
            Me.PLHdisplayAction.Controls.Clear()
        End If
        Return actions
    End Function

    Public Function InitializeRemoteControl(initializer As dtoExternalModuleInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl) Implements IExternalModuleDisplayAction.InitializeRemoteControl
        Dim actions As New List(Of dtoModuleActionControl)
        Dim oControl As IGenericModuleDisplayAction = LoadActionControl(initializer.Link)
        If Not oControl Is Nothing Then
            oControl.EnableAnchor = EnableAnchor
            oControl.ShortDescription = ShortDescription
            actions = oControl.InitializeRemoteControl(dtoModuleDisplayActionInitializer.Create(initializer, Display, ContainerCSS, IconSize), actionsToDisplay)
            Me.PLHdisplayAction.Controls.Add(oControl)
        Else
            Me.PLHdisplayAction.Controls.Clear()
        End If
        Return actions
    End Function
    Private Function LoadActionControl(ByVal pLink As ModuleLink) As IGenericModuleDisplayAction
        Dim oControl As IGenericModuleDisplayAction = Nothing
        If Not IsNothing(pLink) Then
            Select Case pLink.DestinationItem.ServiceCode
                Case COL_BusinessLogic_v2.UCServices.Services_File.Codex
                    oControl = CType(LoadControl("~/" & lm.Comol.Core.BaseModules.Repository.Domain.RootObject.DisplayModuleActionControl), IGenericModuleDisplayAction)

                Case COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex
                    oControl = CType(LoadControl("~/" & COL_Questionario.RootObject.DisplayModuleActionControl), IGenericModuleDisplayAction)

                Case COL_BusinessLogic_v2.UCServices.Services_Forum.Codex

                Case COL_BusinessLogic_v2.UCServices.Services_DiarioLezioni.Codex

                Case COL_BusinessLogic_v2.UCServices.Services_Wiki.Codex

                Case COL_BusinessLogic_v2.UCServices.Services_WorkBook.Codex

                Case COL_BusinessLogic_v2.UCServices.Services_TaskList.Codex

                Case Else

            End Select
        End If
        Return oControl
    End Function
#End Region

    Public Function getDescriptionByLink(ByVal pLink As lm.Comol.Core.DomainModel.ModuleLink, isGeneric As Boolean) As String Implements IExternalModuleDisplayAction.getDescriptionByLink
        Dim oControl As IGenericModuleDisplayAction = LoadActionControl(pLink)

        If Not oControl Is Nothing Then
            Return oControl.getDescriptionByLink(pLink, isGeneric)
        Else
            Return "Module"
        End If
    End Function

    Public Function GetInLineDescriptionByLink(pLink As lm.Comol.Core.DomainModel.ModuleLink, isGeneric As Boolean) As String Implements IExternalModuleDisplayAction.GetInLineDescriptionByLink
        Dim oControl As IGenericModuleDisplayAction = LoadActionControl(pLink)

        If Not oControl Is Nothing Then
            Return oControl.getDescriptionByLink(pLink, isGeneric)
        Else
            Return "Module"
        End If
    End Function

   
End Class