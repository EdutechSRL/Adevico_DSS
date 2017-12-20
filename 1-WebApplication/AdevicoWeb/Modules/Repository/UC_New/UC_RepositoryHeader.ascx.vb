Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_RepositoryHeader
    Inherits FRbaseControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Internal"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
    Public Sub InitializeHeader(isGeneric As Boolean, currentSet As PresetType, availableOptions As Dictionary(Of PresetType, List(Of ViewOption)), activeOptions As Dictionary(Of PresetType, List(Of ViewOption)), Optional idCommunity As Integer = -1, Optional idPerson As Integer = -1, Optional alternate As String = "", Optional tags As List(Of String) = Nothing)
        LTscriptRender.Visible = True
        LTscriptRender.Text = LTscript.Text
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#idUser#", IIf(isGeneric, "", idPerson))
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#idCommunity#", IIf(isGeneric, "", idCommunity))


        LTscriptRender.Text = Replace(LTscriptRender.Text, "#alternate#", alternate)

        'selectedOption? 
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#default#", currentSet.ToString)
        For Each pSet As PresetType In (From s In [Enum].GetValues(GetType(PresetType)) Where s <> PresetType.None Select s)
            If availableOptions.ContainsKey(pSet) Then
                LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Tree.ToString & "#", IIf(activeOptions(pSet).Contains(ViewOption.Tree), 0, 1))
                LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Statistics.ToString & "#", IIf(activeOptions(pSet).Contains(ViewOption.Statistics), 0, 1))
                LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Extrainfo.ToString & "#", IIf(activeOptions(pSet).Contains(ViewOption.Extrainfo), 0, 1))
                LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Date.ToString & "#", IIf(activeOptions(pSet).Contains(ViewOption.Date), 0, 1))
                LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.NarrowWideView.ToString & "#", IIf(activeOptions(pSet).Contains(ViewOption.NarrowWideView), 0, 1))
            Else
                LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Tree.ToString & "#", 0)
                LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Statistics.ToString & "#", 0)
                LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Extrainfo.ToString & "#", 0)
                LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Date.ToString & "#", 0)
                LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.NarrowWideView.ToString & "#", 0)
            End If
        Next
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#itemError_Extension#", Resource.getValue("itemError_Extension"))
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#itemError_Size#", Resource.getValue("itemError_Size"))
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#itemError_NotSupported#", Resource.getValue("itemError_NotSupported"))
        If (Not IsNothing(tags) AndAlso tags.Any) Then
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#tags#", """" & Join(tags.ToArray(), """,""") & """")
        Else
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#tags#", "")
        End If
        '//1 = hidden, 0 = visible
        '// Tree , Statistics , Extrainfo , Date , NarrowWideView 
        'var filerepository_simple = "#Tree#,#Statistics#,#Extrainfo#,#Date#,#NarrowWideView#";
        'var filerepository_standard = "#Tree#,#Statistics#,#Extrainfo#,#Date#,#NarrowWideView#";
        'var filerepository_advanced = "#Tree#,#Statistics#,#Extrainfo#,#Date#,#NarrowWideView#";
    End Sub



    Public Sub InitializeGenericHeader(tags As List(Of String))
        LTscriptRender.Visible = True
        LTscriptRender.Text = LTscript.Text
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#idUser#", "")
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#idCommunity#", "")
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#alternate#", "generic")
        'selectedOption? 
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#default#", PresetType.Simple.ToString)
        For Each pSet As PresetType In (From s In [Enum].GetValues(GetType(PresetType)) Where s <> PresetType.None Select s)
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Tree.ToString & "#", 0)
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Statistics.ToString & "#", 0)
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Extrainfo.ToString & "#", 0)
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Date.ToString & "#", 0)
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.NarrowWideView.ToString & "#", 0)
        Next
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#itemError_Extension#", Resource.getValue("itemError_Extension"))
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#itemError_Size#", Resource.getValue("itemError_Size"))
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#itemError_NotSupported#", Resource.getValue("itemError_NotSupported"))
        If (Not IsNothing(tags) AndAlso tags.Any) Then
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#tags#", """" & Join(tags.ToArray(), """,""") & """")
        Else
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#tags#", "")
        End If


        '//1 = hidden, 0 = visible
        '// Tree , Statistics , Extrainfo , Date , NarrowWideView 
        'var filerepository_simple = "#Tree#,#Statistics#,#Extrainfo#,#Date#,#NarrowWideView#";
        'var filerepository_standard = "#Tree#,#Statistics#,#Extrainfo#,#Date#,#NarrowWideView#";
        'var filerepository_advanced = "#Tree#,#Statistics#,#Extrainfo#,#Date#,#NarrowWideView#";
    End Sub

    Public Sub InitializeGenericHeader(prefix As String, identifier As RepositoryIdentifier)
        LTscriptRender.Visible = True
        LTscriptRender.Text = LTscript.Text
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#idUser#", identifier.IdPerson)
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#idCommunity#", identifier.IdCommunity)
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#alternate#", prefix)
        'selectedOption? 
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#default#", PresetType.Simple.ToString)
        For Each pSet As PresetType In (From s In [Enum].GetValues(GetType(PresetType)) Where s <> PresetType.None Select s)
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Tree.ToString & "#", 0)
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Statistics.ToString & "#", 0)
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Extrainfo.ToString & "#", 0)
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.Date.ToString & "#", 0)
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#PresetType." & pSet.ToString & "." & ViewOption.NarrowWideView.ToString & "#", 0)
        Next
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#itemError_Extension#", Resource.getValue("itemError_Extension"))
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#itemError_Size#", Resource.getValue("itemError_Size"))
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#itemError_NotSupported#", Resource.getValue("itemError_NotSupported"))

        LTscriptRender.Text = Replace(LTscriptRender.Text, "#tags#", "")
    End Sub
#End Region

End Class