<Serializable(), CLSCompliant(True)> Public Class YoutubeSmartTag
	Inherits SmartTag

	'Public Sub New(Optional ByVal name As String = "YouTube", Optional ByVal tag As String = "youtube", Optional ByVal width As Integer = 425, Optional ByVal height As Integer = 355)
	'	MyBase.New(name, tag, "<object width=" + CStr(width) + " height=" + CStr(height) + "><param name=""movie"" value=""{0}&hl=en""></param><param name=""wmode"" value=""transparent""></param><embed src=""{0}&hl=en"" type=""application/x-shockwave-flash"" wmode=""transparent"" width=" + CStr(width) + " height=" + CStr(height) + "></embed></object>")
	'	Me.ReplaceList = New ArrayList
	'	Me.ReplaceList.Add(New ReplaceIt("watch?v=", "v/"))
	'End Sub

    Public Sub New(Optional ByVal name As String = "YouTube", Optional ByVal tag As String = "youtube", Optional ByVal width As Integer = 425, Optional ByVal height As Integer = 355, Optional ByVal oArrayList As ArrayList = Nothing, Optional ByVal iImage As String = "")
        MyBase.New(name, tag, "<iframe width=" + CStr(width) + " height=" + CStr(height) + " frameborder=""0""allowfullscreen src=""{0}""></iframe>", , , , iImage)
        Me.ReplaceList = oArrayList
    End Sub
	Public Overrides Function TagIt(ByVal code As String) As String
		Return MyBase.TagIt(code)
	End Function
End Class

<Serializable(), CLSCompliant(True)> Public Class LatexSmartTag
	Inherits SmartTag

	Public Sub New(Optional ByVal name As String = "Latex", Optional ByVal tag As String = "latex", Optional ByVal address As String = "http://localhost/Comunita_online/SmartTagsRender/latexImage.aspx", Optional ByVal dpi As Integer = 160, Optional ByVal addressPopUp As String = "http://localhost/Comunita_online/SmartTagsRender/LatexPopUp.aspx", Optional ByVal iImage As String = "")
		MyBase.New(name, tag, "<a href="""" onclick=""javascript:return LatexPopup(this,'" & addressPopUp & "');"" class='latexhref' title='{0}'><img class='lateximg' src='" + address + "?{0}\dpi{" + CStr(dpi) + "}' alt='{1}' title='{1}'/></a>", , , True, iImage)
	End Sub
End Class

<Serializable(), CLSCompliant(True)> Public Class SlideShareSmartTag
	Inherits SmartTag

	Public Sub New(Optional ByVal name As String = "SlideShare", Optional ByVal tag As String = "slideshare", Optional ByVal width As Integer = 425, Optional ByVal height As Integer = 355, Optional ByVal oArrayList As ArrayList = Nothing, Optional ByVal iImage As String = "")
		MyBase.New(name, tag, "<object style=""margin:0px"" width=" + CStr(width) + " height=" + CStr(height) + "><param name=""movie"" value=""http://static.slideshare.net/swf/ssplayer2.swf?doc={0}""/><param name=""allowFullScreen"" value=""true""/><param name=""allowScriptAccess"" value=""always""/><embed src=""http://static.slideshare.net/swf/ssplayer2.swf?doc={0}"" type=""application/x-shockwave-flash"" allowscriptaccess=""always"" allowfullscreen=""true"" width=" + CStr(width) + " height=" + CStr(height) + "></embed></object>", , , , iImage)
		'Me.ReplaceList = New ArrayList
		'Me.ReplaceList.Add(New ReplaceIt("\[slideshare id=.*&doc=", "", True))
		'Me.ReplaceList.Add(New ReplaceIt("&w=.*\]", "", True))
		Me.ReplaceList = oArrayList
	End Sub
End Class

<Serializable(), CLSCompliant(True)> Public Class DocStocSmartTag
	Inherits SmartTag
	Public Sub New(Optional ByVal name As String = "DocStoc", Optional ByVal tag As String = "docstoc", Optional ByVal width As Integer = 425, Optional ByVal height As Integer = 355, Optional ByVal iImage As String = "")
		MyBase.New(name, tag, "<iframe width=" + CStr(width) + " height=" + CStr(height) + " src=""http://www.docstoc.com/docs/document-preview.aspx?doc_id={0}""/>", , , , iImage)
	End Sub
End Class

<Serializable(), CLSCompliant(True)> Public Class WellKnownSmartTag
	Private _SmartTags As New SmartTags

	Public Sub New()
		'Dim youtube As New YoutubeSmartTag()
		Dim latex As New LatexSmartTag()

		'Dim dmlist As IList = New ArrayList
		'dmlist.Add(New ReplaceIt("/video", "/swf"))
		'dmlist.Add(New ReplaceIt("/it/featured", ""))
		'dmlist.Add(New ReplaceIt("_.*", "", True))
		'Dim dailymotion As New SmartTag("DailyMotion", "dailymotion", "<object width=""420"" height=""339""><param name=""movie"" value=""{0}"" /><param name=""allowFullScreen"" value=""true"" /><param name=""allowScriptAccess"" value=""always"" /><embed src=""{0}"" type=""application/x-shockwave-flash"" width=""420"" height=""339"" allowFullScreen=""true"" allowScriptAccess=""always""></embed></object>", dmlist)

		'Dim bold As New SmartTag("bold", "b", "<b>{0}</b>")

		'Dim slideshare As New SlideShareSmartTag()

		'Dim docstoc As New DocStocSmartTag()

		'   smts.add(youtube)
		_SmartTags.add(latex)
		'  smts.add(dailymotion)
		'  smts.add(slideshare)
		'  smts.add(docstoc)
		'  smts.add(bold)
	End Sub

	Public Function TagAll(ByVal html As String) As String
		Return _SmartTags.TagAll(html)
	End Function
End Class