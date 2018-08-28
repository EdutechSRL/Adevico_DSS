Imports System.Text.RegularExpressions

<Serializable(), CLSCompliant(True)> Public Class SmartTag
	Private _NeedEncoding As Boolean
	Private _Image As String
	Private _DisplayWidth As String
	Private _DisplayHeight As String

	Public Property DisplayWidth() As String
		Get
			Return _DisplayWidth
		End Get
		Set(ByVal value As String)
			_DisplayWidth = value
		End Set
	End Property
	Public Property DisplayHeight() As String
		Get
			Return _DisplayHeight
		End Get
		Set(ByVal value As String)
			_DisplayHeight = value
		End Set
	End Property
	Private _name As String
	Public Property Name() As String
		Get
			Return _name
		End Get
		Set(ByVal value As String)
			_name = value
		End Set
	End Property

	Private _tag As String
	Public Property Tag() As String
		Get
			Return _tag
		End Get
		Set(ByVal value As String)
			_tag = value
		End Set
	End Property

	Private _html As String
	Public Property Html() As String
		Get
			Return _html
		End Get
		Set(ByVal value As String)
			_html = value
		End Set
	End Property

	Private _replacelist As IList
	Public Property ReplaceList() As IList
		Get
			Return _replacelist
		End Get
		Set(ByVal value As IList)
			_replacelist = value
		End Set
	End Property

	Private _regex As String
	Public Property RegularEx() As String
		Get
			Return _regex
		End Get
		Set(ByVal value As String)
			_regex = value
		End Set
	End Property

	Public Property NeedEncoding() As Boolean
		Get
			Return _NeedEncoding
		End Get
		Set(ByVal value As Boolean)
			_NeedEncoding = value
		End Set
	End Property

	Public Shared ReadOnly Property OpenTag() As String
		Get
			Return "[{0}]"
		End Get
	End Property
	Public Shared ReadOnly Property CloseTag() As String
		Get
			Return "[/{0}]"
		End Get
	End Property
	Public Property Image() As String
		Get
			Return _Image
		End Get
		Set(ByVal value As String)
			_Image = value
		End Set
	End Property
	Public Sub New()

	End Sub

	Public Sub New(ByVal name As String, ByVal tag As String, ByVal html As String, Optional ByVal replist As IList = Nothing, Optional ByVal regex As String = "", Optional ByVal Encoding As Boolean = False, Optional ByVal iImage As String = "")
		Me.Name = name
		Me.Tag = tag
		Me.Html = html
		Me._NeedEncoding = Encoding
		If replist IsNot Nothing Then
			ReplaceList = replist
		End If

		Me.RegularEx = regex
		Me._Image = iImage
	End Sub

    Public Overridable Function TagIt(ByVal code As String) As String
        code = code.Replace("</span>", "")
        code = code.Replace("<span contenteditable=""true"" class=""editable"">", "")
        If Me._NeedEncoding Then
            code = System.Web.HttpUtility.UrlEncode(code)
        End If
        If ReplaceList IsNot Nothing Then
            For Each replacer As ReplaceIt In ReplaceList
                If replacer.Regex Then
                    code = Regex.Replace(code, replacer.Original, replacer.Replaced)
                Else
                    code = code.Replace(replacer.Original, replacer.Replaced)
                End If
            Next
        End If
        If Me._NeedEncoding Then
            Try
                Return String.Format(Html, code, System.Web.HttpUtility.UrlDecode(code))
            Catch ex As Exception
                Return Html.Replace("{0}", code).Replace("{1}", System.Web.HttpUtility.UrlDecode(code))
            End Try
        Else
            Try
                Return String.Format(Html, code)
            Catch ex As Exception
                Return Html.Replace("{0}", code)
            End Try
        End If
    End Function

	'Public Function DeTagIt(ByVal code As String) As String
	'    Return code
	'End Function

	'Public Function Comment() As String
	'    Return ""
	'End Function
	Public Overrides Function ToString() As String
		Return "[" & Me._tag & "]"
	End Function
End Class

<Serializable(), CLSCompliant(True)> Public Class ReplaceIt
	Private _original As String
	Public Property Original() As String
		Get
			Return _original
		End Get
		Set(ByVal value As String)
			_original = value
		End Set
	End Property

	Private _replaced As String
	Public Property Replaced() As String
		Get
			Return _replaced
		End Get
		Set(ByVal value As String)
			_replaced = value
		End Set
	End Property

	Private _regex As Boolean
	Public Property Regex() As Boolean
		Get
			Return _regex
		End Get
		Set(ByVal value As Boolean)
			_regex = value
		End Set
	End Property


	Public Sub New(ByVal original As String, ByVal replaced As String, Optional ByVal regex As Boolean = False)
		Me.Original = original
		Me.Replaced = replaced
		Me.Regex = regex
	End Sub

End Class

<Serializable(), CLSCompliant(True)> Public Class SmartTags
	Private _hash As New Hashtable

	Public Sub New()

	End Sub

	Public Sub Add(ByVal stag As SmartTag)
		_hash.Add(stag.Tag, stag)
	End Sub

	Public Sub Remove(ByVal stag As SmartTag)
		_hash.Remove(stag.Tag)
	End Sub

	Public Sub Remove(ByVal tag As String)
		_hash.Remove(tag)
	End Sub

	Public Sub Clear()
		_hash.Clear()
	End Sub
	Public Function Count()
		Return _hash.Count
	End Function

	'Public Function GetTag(ByVal code As String) As String
	'	Return _hash.Item(code)
	'End Function

    'Public Function TagAll(ByVal html As String) As String

    '	Dim Reg As Regex
    '	Dim Match As Match
    '	Dim Matches As MatchCollection

    '       Reg = New Regex("(?'start'\[[a-zA-Z0-9]*\])(?<code>(.*?))(?'end'\[/\k'start'\])")
    '       'Reg = New Regex("(?'start'\[[a-zA-Z0-9]*\])(?'code'(.*?))(?'end'\[/[a-zA-Z0-9]*\])")

    '	Matches = Reg.Matches(html)
    '	For Each Match In Matches
    '		Dim tmp As String
    '		tmp = Match.Value

    '           Dim a As String = Match.Groups("start").Value
    '		Dim b As String = Match.Groups("end").Value
    '		Dim c As String = Match.Groups("code").Value

    '		a = a.Remove(0, 1)
    '		a = a.Remove(a.LastIndexOf("]"), 1)
    '		b = b.Remove(0, 2)
    '		b = b.Remove(b.LastIndexOf("]"), 1)

    '		If a = b Then
    '			'If a = b Then
    '			Dim t As SmartTag
    '			t = _hash(a)
    '			If t IsNot Nothing Then
    '				Dim r As String = t.TagIt(c)
    '				html = html.Replace(tmp, r)
    '			End If
    '		End If
    '	Next

    '	Return html
    'End Function

    Public Function TagAll(ByVal html As String) As String
        If (Not String.IsNullOrEmpty(html)) Then


            Dim Reg As Regex
            Dim Match As Match
            Dim Matches As MatchCollection

            Reg = New Regex("\[(?<start>.*)\](?<code>(.*?))(\[/\k<start>\])")
            'Reg = New Regex("(?'start'\[[a-zA-Z0-9]*\])(?'code'(.*?))(?'end'\[/[a-zA-Z0-9]*\])")

            Matches = Reg.Matches(html)
            For Each Match In Matches
                Dim tmp As String
                tmp = Match.Value

                Dim a As String = Match.Groups("start").Value
                'Dim b As String = Match.Groups("end").Value
                Dim c As String = Match.Groups("code").Value

                'a = a.Remove(0, 1)
                'a = a.Remove(a.LastIndexOf("]"), 1)
                'b = b.Remove(0, 2)
                'b = b.Remove(b.LastIndexOf("]"), 1)

                'If a = b Then
                'If a = b Then
                Dim t As SmartTag
                t = _hash(a)
                If t IsNot Nothing Then
                    Dim r As String = t.TagIt(c)
                    html = html.Replace(tmp, r)
                End If
                'End If
            Next
        End If
        Return html


    End Function

	Public Function TagThis(ByVal html As String, ByVal tag As SmartTag) As String
        If (Not String.IsNullOrEmpty(html) And Not tag Is Nothing) Then
            Dim Reg As Regex
            Dim Match As Match
            Dim Matches As MatchCollection

            If Not String.IsNullOrEmpty(tag.RegularEx) Then
                Reg = New Regex(tag.RegularEx)
            Else
                Reg = New Regex("\[(?<start>.*)\](?<code>(.*?))(\[/\k<start>\])")
            End If



            Matches = Reg.Matches(html)
            For Each Match In Matches
                Dim tmp As String
                tmp = Match.Value
             
                Dim a As String = Match.Groups("start").Value
                Dim c As String = Match.Groups("code").Value
                If tag IsNot Nothing Then
                    Dim r As String = tag.TagIt(c)
                    html = html.Replace(tmp, r)
                End If
                'Dim a As String = Match.Groups("start").Value
                'Dim b As String = Match.Groups("end").Value
                'Dim c As String = Match.Groups("code").Value

                'a = a.Remove(0, 1)
                'a = a.Remove(a.LastIndexOf("]"), 1)
                'b = b.Remove(0, 2)
                'b = b.Remove(b.LastIndexOf("]"), 1)

                'If a = b Then
                '    'If a = b Then
                '    Dim t As SmartTag
                '    t = _hash(a)
                '    If t IsNot Nothing Then
                '        Dim r As String = t.TagIt(c)
                '        html = html.Replace(tmp, r)
                '    End If
                'End If
            Next
        End If
        Return html
    End Function

	Public Function TagList() As IList
		Dim oList As New List(Of SmartTag)
		For Each oSmartTag As Object In Me._hash.Values
			oList.Add(DirectCast(oSmartTag, SmartTag))
		Next
		Return oList
	End Function
End Class