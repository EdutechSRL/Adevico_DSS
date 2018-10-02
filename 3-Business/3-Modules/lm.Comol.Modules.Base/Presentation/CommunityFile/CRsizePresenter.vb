Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class CRsizePresenter
        Inherits DomainPresenter

        Private _CommonManager As ManagerCommon
        Public Overloads Property CurrentManager() As ManagerCommunityFiles
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerCommunityFiles)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property

        Public Overloads ReadOnly Property View() As IviewCommunityRepositorySize
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewCommunityRepositorySize)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub


        Public Sub InitView()
            Dim FileSystemUsed As Long = Me.GetRepositorySize
            Dim AvailableMB As Long = Me.View.AvailableMB

            Dim MaxRepositorySize As Long
            Dim Rate As Double = 0

            If AvailableMB < 0 Then
                MaxRepositorySize = 10 * 1024 * 1024
            ElseIf AvailableMB = 1 Then
                MaxRepositorySize = AvailableMB * 1024
            Else
                MaxRepositorySize = AvailableMB * 1024 * 1024
            End If

            If MaxRepositorySize > 0 Then
                Rate = ((FileSystemUsed * 100) / MaxRepositorySize)
                If Rate < 1 Then
                    Rate = 1
                End If
            Else
                Rate = 20
            End If

            Me.View.TestoSezioneOver = ""
            Me.View.UsageRate25 = 0
            Me.View.UsageRate50 = 0
            Me.View.UsageRate75 = 0
            Me.View.UsageRate100 = 0
            Me.View.UsageRate150 = 0

            If Rate <= 25 Then
                Me.View.UsageRate25 = (Rate * Me.View.BaseUnit) / 25
            ElseIf Rate <= 50 Then
                Me.View.UsageRate25 = Me.View.BaseUnit
                Me.View.UsageRate50 = (Rate * Me.View.BaseUnit) / 50
            ElseIf Rate <= 75 Then
                Me.View.UsageRate25 = Me.View.BaseUnit
                Me.View.UsageRate50 = Me.View.BaseUnit
                Me.View.UsageRate75 = (Rate * Me.View.BaseUnit) / 75
            ElseIf Rate <= 100 Then
                Me.View.UsageRate25 = Me.View.BaseUnit
                Me.View.UsageRate50 = Me.View.BaseUnit
                Me.View.UsageRate75 = Me.View.BaseUnit
                Me.View.UsageRate100 = (Rate * Me.View.BaseUnit) / 100
            Else
                Me.View.UsageRate25 = Me.View.BaseUnit
                Me.View.UsageRate50 = Me.View.BaseUnit
                Me.View.UsageRate75 = Me.View.BaseUnit
                Me.View.UsageRate100 = Me.View.BaseUnit
                Me.View.UsageRate150 = Me.View.BaseUnit
                Me.View.TestoSezioneOver = String.Format("{0:F2}", Rate) & "%"
            End If
            If AvailableMB = 1 Then
                Me.View.SetDisplayInfo(FileSystemUsed / (1024 * 1024), MaxRepositorySize / 1024)
            Else
                Me.View.SetDisplayInfo(FileSystemUsed / (1024 * 1024), MaxRepositorySize / (1024 * 1024))
            End If
        End Sub

        Private Function GetRepositorySize() As Long
            Dim Size As Long = 0

            If Me.View.ChachedSize = 0 Then
                Size = Me.CurrentManager.GetRepositorySize(Me.View.RepositoryCommunityID, Me.View.isPersonalRepository, Me.View.EvaluateDeletedFiles)
                Me.View.ChachedSize = Size
            Else
                Size = Me.View.ChachedSize
            End If
            Return Size
        End Function

    End Class
End Namespace