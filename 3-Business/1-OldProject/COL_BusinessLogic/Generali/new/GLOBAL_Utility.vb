Imports System.Text.RegularExpressions
Imports System.Web.UI


Public Class HTML_Utility

	'Elimina i tag html ed eventuali attribute ma preserva innerText
	Public Shared Function PlainTextFromHTML(ByVal strHTML As String) As String
		Dim objRegExp As New Regex("<(.|\n)+?>", RegexOptions.IgnoreCase)
		Dim strOutput As String

		strOutput = objRegExp.Replace(strHTML, "")

		Return strOutput
	End Function

	'Sostituisce < > con &lt; e &gt; per impedire il riconoscimento html
	Public Shared Function BlockHTML(ByVal strHTML As String) As String
		Return strHTML.Replace("<", "&lt;").Replace(">", "&gt;")
	End Function

	'Cancella il tag fornito, anche l'inner
	Public Shared Function EraseTag(ByVal strHTML As String, ByVal tag As String) As String
		Dim objRegExp As New Regex("<[ \t]*" + tag + "(.|\n)+?/[ \t]*" + tag + "[ \t]*>", RegexOptions.IgnoreCase) '<tag>...</tag>
		Dim strOutput As String

		strOutput = objRegExp.Replace(strHTML, "")

		Return strOutput
	End Function

	'Sostituisce < > del tag fornito con &lt; e &gt;
	Public Shared Function BlockTag(ByVal strHTMl As String, ByVal tag As String) As String
		Dim objRegExp As New Regex("<[ \t]*/[ \t]*" + tag + "[ \t]*>", RegexOptions.IgnoreCase)	'</tag>
		Dim strOutput As String

		strOutput = objRegExp.Replace(strHTMl, "&lt;/" + tag + "&gt;")

		objRegExp = New Regex("<[ \t]*" + tag + "[ \t]*>", RegexOptions.IgnoreCase)	'<tag>
		strOutput = objRegExp.Replace(strOutput, "&lt;" + tag + "&gt;")

		Return strOutput
	End Function

	'rende lowerCase tutti i gli elementi tag
	Public Shared Function LowerTag(ByVal strHTML As String, ByVal tag As String) As String
		Dim objRegExp As New Regex("<[ \t]*/[ \t]*" + tag + "[ \t]*>", RegexOptions.IgnoreCase)	'</tag>
		Dim strOutput As String

		strOutput = objRegExp.Replace(strHTML, "</" + tag.ToLower + ">")

		objRegExp = New Regex("<[ \t]*" + tag + "[ \t]*>", RegexOptions.IgnoreCase)	'<tag>
		strOutput = objRegExp.Replace(strOutput, "<" + tag.ToLower + ">")

		Return strOutput
	End Function

	'rende lowerCase tutti i tag indicati
	Public Shared Function LowerTags(ByVal strHTML As String, ByVal tags As String) As String
		Dim strOutput As String = strHTML

		Dim tagArr() As String = tags.Split(",;".ToCharArray)

		For Each tag As String In tagArr
			If tag <> "" Then
				strOutput = LowerTag(strOutput, tag)
			End If
		Next

		Return strOutput
	End Function

	'Come EraseTag ma su un elenco ( , o ; ) di tags
	Public Shared Function EraseTags(ByVal strHTML As String, ByVal tags As String) As String
		Dim strOutput As String = strHTML

		Dim tagArr() As String = tags.Split(",;".ToCharArray)

		For Each tag As String In tagArr
			If tag <> "" Then
				strOutput = EraseTag(strOutput, tag)
			End If
		Next

		Return strOutput
	End Function

	'Come BlockTag ma su un elenco ( , o ; ) di tags
	Public Shared Function BlockTags(ByVal strHTMl As String, ByVal tags As String) As String
		Dim strOutput As String = strHTMl

		Dim tagArr() As String = tags.Split(",;".ToCharArray)

		For Each tag As String In tagArr
			If tag <> "" Then
				strOutput = BlockTag(strOutput, tag)
			End If
		Next

		Return strOutput
	End Function

	'Come EraseTag ma sul tag script
	Public Shared Function EraseScript(ByVal strHTML As String) As String
		Return EraseTag(strHTML, "script")
	End Function

	'Come BlockTag ma sul tag script
	Public Shared Function BlockScript(ByVal strHTML As String) As String
		Return BlockTag(strHTML, "script")
	End Function

	'estrae un elenco degli header <H*></H*>
	Public Shared Function ExtractH(ByVal strHTML As String) As IList
		Dim matchs As System.Text.RegularExpressions.MatchCollection

		Dim outputlist As IList = New ArrayList

		outputlist.Clear()

		Dim objRegExp As New Regex("<[ \t]*h(1|2|3|4|5|6|)(.|\n)+?/[ \t]*h(1|2|3|4|5|6)[ \t]*>", RegexOptions.IgnoreCase) '<h*>...</h*>
		matchs = objRegExp.Matches(strHTML)

		For Each match As System.Text.RegularExpressions.Match In matchs
			outputlist.Add(LowerTags(match.Value, "h1;h2;h3;h4;h5;h6"))
		Next

		Return outputlist
	End Function

End Class


'Utilities per generare Link del tag <head> (Javascript, CSS etc)
Public Class DynamicLink_Utility
	Public Shared Function CSS(ByVal url As String) As HtmlControls.HtmlLink
		Dim link As HtmlControls.HtmlLink = New HtmlControls.HtmlLink()
		link.Attributes.Add("type", "text/css")
		link.Attributes.Add("rel", "stylesheet")
		link.Attributes.Add("href", url)
		Return link
	End Function

	Public Shared Function CSS(ByVal url As String, ByVal media As String) As HtmlControls.HtmlLink
		Dim link As HtmlControls.HtmlLink = New HtmlControls.HtmlLink()
		link.Attributes.Add("type", "text/css")
		link.Attributes.Add("rel", "stylesheet")
		link.Attributes.Add("media", media)
		link.Attributes.Add("href", url)

		Return link
	End Function

	Public Shared Function JS(ByVal url As String) As HtmlControls.HtmlGenericControl
		Dim link As HtmlControls.HtmlGenericControl = New HtmlControls.HtmlGenericControl("script")
		link.Attributes.Add("type", "text/javascript")
		link.Attributes.Add("language", "javascript")
		link.Attributes.Add("src", url)

		Return link
	End Function

End Class
