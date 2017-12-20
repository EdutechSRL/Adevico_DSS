Imports DTh = lm.Comol.Core.DomainModel.Helpers.Export

Public Class UC_EditPagePlacing
    Inherits BaseControl

#Region "Internal"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property
    Public Property Enable As Boolean
        Get
            Return Me.RblPageSelect.Enabled
        End Get
        Set(value As Boolean)
            Me.RblPageSelect.Enabled = value
            RblEnable(value AndAlso Me.RblPageSelect.SelectedValue = "-1")
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
            .setRegularExpressionValidator(Me.REVpageRange)

            .setRadioButtonList(RblPageSelect, "1")
            .setRadioButtonList(RblPageSelect, "0")
            .setRadioButtonList(RblPageSelect, "-1")

            .setCheckBox(CBXeven)
            .setCheckBox(CBXodd)
            .setCheckBox(CBXfirst)
            .setCheckBox(CBXlast)
            .setCheckBox(CBXspecific)
        End With
    End Sub
#End Region

#Region "Internal"

#Region "Init"
    Public Sub InitControl()
        InitControl(DTh.PageMaskingInclude.none, "")
    End Sub
    Public Sub InitControl(ByVal Mask As Integer, ByVal Range As String)
        If (Mask = DTh.PageMaskingInclude.none) Then
            Me.RblPageSelect.SelectedValue = "0"
            RblEnable(False)

        ElseIf (Mask And DTh.PageMaskingInclude.All) Then    'Tutte
            Me.RblPageSelect.SelectedValue = "1"
            RblEnable(False)

        Else
            Me.RblPageSelect.SelectedValue = "-1"
            RblEnable(True)

            Me.CBXeven.Checked = ((Mask And DTh.PageMaskingInclude.Even) > 0)
            Me.CBXfirst.Checked = ((Mask And DTh.PageMaskingInclude.First) > 0)
            Me.CBXlast.Checked = ((Mask And DTh.PageMaskingInclude.Last) > 0)
            Me.CBXodd.Checked = ((Mask And DTh.PageMaskingInclude.Odd) > 0)
            Me.CBXspecific.Checked = ((Mask And DTh.PageMaskingInclude.Range) > 0)

            Me.TXBpagesRange.Text = Range

        End If

        Me.SetInternazionalizzazione()

    End Sub
#End Region

    Public Function GetMask() As Integer

        If Me.RblPageSelect.SelectedValue = "0" Then
            Return 0
        ElseIf Me.RblPageSelect.SelectedValue = "1" Then    'Tutte
            Return 1
        Else
            Dim Mask As Integer = 0

            Mask += IIf(Me.CBXeven.Checked, DTh.PageMaskingInclude.Even, 0)

            Mask += IIf(Me.CBXfirst.Checked, DTh.PageMaskingInclude.First, 0)
            Mask += IIf(Me.CBXlast.Checked, DTh.PageMaskingInclude.Last, 0)
            Mask += IIf(Me.CBXodd.Checked, DTh.PageMaskingInclude.Odd, 0)
            Mask += IIf(Me.CBXspecific.Checked, DTh.PageMaskingInclude.Range, 0)

            Return Mask
        End If

    End Function
    Public Function GetRange() As String
        If Me.RblPageSelect.SelectedValue = "-1" AndAlso Me.CBXspecific.Checked Then
            Return Me.TXBpagesRange.Text
        Else
            Return ""
        End If
    End Function
    Private Sub RblEnable(ByVal Enable As Boolean)
        Me.CBXeven.Enabled = Enable
        Me.CBXfirst.Enabled = Enable
        Me.CBXlast.Enabled = Enable
        Me.CBXodd.Enabled = Enable
        Me.CBXspecific.Enabled = Enable

        Me.TXBpagesRange.Enabled = Enable
    End Sub

#End Region

#Region "Handler"
    Private Sub RblPageSelect_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RblPageSelect.SelectedIndexChanged
        If Me.RblPageSelect.SelectedValue = "0" OrElse Me.RblPageSelect.SelectedValue = "1" Then
            RblEnable(False)
        Else
            RblEnable(True)
        End If
    End Sub
#End Region

End Class