using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Collections;

namespace lm.Comol.Core.DomainModel.Helpers.Tags
{
    [Serializable()]
    public class SmartTags
    {
        private Dictionary<String, SmartTag> tags = new Dictionary<String, SmartTag>();
        public SmartTags()
        {
        }

        public void Add(SmartTag item)
        {
            if (item!=null && !String.IsNullOrEmpty(item.Tag)){
                if (tags.ContainsKey(item.Tag))
                    tags[item.Tag] =item;
                else
                    tags.Add(item.Tag,item);
            }
        }
        public void Remove(SmartTag item)
        {
            if (item != null && !String.IsNullOrEmpty(item.Tag) && tags.ContainsKey(item.Tag))
                tags.Remove(item.Tag);
        }
        public void Remove(String tag)
        {
            if (!String.IsNullOrEmpty(tag) && tags.ContainsKey(tag))
                tags.Remove(tag);
        }
        public void Clear()
        {
            tags.Clear();
        }
        public Int32 Count()
        {
            return tags.Count;
        }

        //Public Function GetTag(ByVal code As String) As String
        //	Return _hash.Item(code)
        //End Function

        //Public Function TagAll(ByVal html As String) As String

        //	Dim Reg As Regex
        //	Dim Match As Match
        //	Dim Matches As MatchCollection

        //       Reg = New Regex("(?'start'\[[a-zA-Z0-9]*\])(?<code>(.*?))(?'end'\[/\k'start'\])")
        //       'Reg = New Regex("(?'start'\[[a-zA-Z0-9]*\])(?'code'(.*?))(?'end'\[/[a-zA-Z0-9]*\])")

        //	Matches = Reg.Matches(html)
        //	For Each Match In Matches
        //		Dim tmp As String
        //		tmp = Match.Value

        //           Dim a As String = Match.Groups("start").Value
        //		Dim b As String = Match.Groups("end").Value
        //		Dim c As String = Match.Groups("code").Value

        //		a = a.Remove(0, 1)
        //		a = a.Remove(a.LastIndexOf("]"), 1)
        //		b = b.Remove(0, 2)
        //		b = b.Remove(b.LastIndexOf("]"), 1)

        //		If a = b Then
        //			'If a = b Then
        //			Dim t As SmartTag
        //			t = _hash(a)
        //			If t IsNot Nothing Then
        //				Dim r As String = t.TagIt(c)
        //				html = html.Replace(tmp, r)
        //			End If
        //		End If
        //	Next

        //	Return html
        //End Function

        public String TagAll(String html)
        {
            if ((!string.IsNullOrEmpty(html)))
            {
                Regex Reg = null;
                Match Match = null;
                MatchCollection Matches = null;

                Reg = new Regex("\\[(?<start>.*)\\](?<code>(.*?))(\\[/\\k<start>\\])");
                //Reg = New Regex("(?'start'\[[a-zA-Z0-9]*\])(?'code'(.*?))(?'end'\[/[a-zA-Z0-9]*\])")

                Matches = Reg.Matches(html);
                foreach (Match Match_loopVariable in Matches)
                {
                    Match = Match_loopVariable;
                    String tmp = Match.Value;
                    String a = Match.Groups["start"].Value;
                    //Dim b As String = Match.Groups("end").Value
                    String c = Match.Groups["code"].Value;

                    //a = a.Remove(0, 1)
                    //a = a.Remove(a.LastIndexOf("]"), 1)
                    //b = b.Remove(0, 2)
                    //b = b.Remove(b.LastIndexOf("]"), 1)

                    //If a = b Then
                    //If a = b Then
                    SmartTag t = tags[a];
                    if (t != null)
                        html = html.Replace(tmp, t.TagIt(c));
                    //End If
                }
            }
            return html;
        }
        public String TagThis(String html, SmartTag tag)
        {
            if ((!string.IsNullOrEmpty(html) & (tag != null)))
            {
                Regex Reg = null;
                Match Match = null;
                MatchCollection Matches = null;

                Reg = new Regex(tag.RegularEx);

                Matches = Reg.Matches(html);
                foreach (Match Match_loopVariable in Matches)
                {
                    Match = Match_loopVariable;
                    string tmp = Match.Value;

                    string a = Match.Groups["start"].Value;
                    string b = Match.Groups["end"].Value;
                    string c = Match.Groups["code"].Value;

                    a = a.Remove(0, 1);
                    a = a.Remove(a.LastIndexOf("]"), 1);
                    b = b.Remove(0, 2);
                    b = b.Remove(b.LastIndexOf("]"), 1);

                    if (a == b)
                    {
                        //If a = b Then
                        SmartTag t = tags[a];
                        if (t != null)
                            html = html.Replace(tmp, t.TagIt(c));
                    }
                }
            }
            return html;
        }

        public IList<SmartTag> TagList()
        {
            if (tags == null || tags.Count == 0)
                return new List<SmartTag>();
            else
                return tags.Select(t => t.Value).ToList();
        }
    }
}