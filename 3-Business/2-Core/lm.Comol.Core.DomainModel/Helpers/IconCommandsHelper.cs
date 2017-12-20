using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using lm.Comol.Core.DomainModel.Repository;

namespace lm.Comol.Core.DomainModel.Helpers
{
    public static class IconCommandsHelper
    {

        #region "HtmlLink"
            public static string RenderActionlLink(String url, String title, CommandIcon icon)
            {
               return RenderActionlLink("",url,title, LinkTarget._self, icon, IconSize.Medium ,"");
            }
            public static string RenderActionlLink(String url, String title, CommandIcon icon, IconSize size)
            {
                return RenderActionlLink("",url,title, LinkTarget._self, icon, size ,"");
            }
            public static string RenderActionlLink(String url, String title, CommandIcon icon, IconSize size, String cssClass)
            {
                return RenderActionlLink("", url, title, LinkTarget._self, icon, size, cssClass);
            }
            public static string RenderActionlLink(String url, String title, LinkTarget target, CommandIcon icon, IconSize size)
            {
                return RenderActionlLink("",url,title, target, icon, size ,"");
            }
            public static string RenderActionlLink(String url, String title, LinkTarget target, CommandIcon icon, IconSize size, String cssClass)
            {
                return RenderActionlLink("",url,title, target, icon, size ,cssClass);
            }
            public static string RenderActionlLink(String text, String url, String title,LinkTarget target, CommandIcon icon,IconSize size,String cssClass)
            {
                String render = "<a href=\"{0}\" title=\"{1}\" class=\"{2} {3} {4}\" target=\"{5}\">{6}</a>";
                return string.Format(render, url, title, HTMLinputClass.Link.GetStringValue(), icon.GetStringValue() + size.GetStringValue(), cssClass, target.ToString(), text);
            }
        #endregion

        #region "SPAN"
        /// <summary>
        /// Medium SPAN icon
        /// </summary>
        /// <param name="title">Tool Tip</param>
        /// <param name="icon">Icon Type</param>
        /// <returns></returns>
        public static string RenderSpan(String title, CommandIcon icon)
        {
             return  RenderSpan(title,icon, IconSize.Medium,"");
        }

        /// <summary>
       /// 
       /// </summary>
       /// <param name="title">ToolTip</param>
       /// <param name="size">Icon size</param>
       /// <param name="icon">Icon type</param>
       /// <returns></returns>
        public static string RenderSpan(String title, RepositoryItemType type, IconSize size)
        {
            return RenderSpan(title, GetDefaultIcon(type), size, "");
        }
        public static string RenderSpan(String title, RepositoryItemType type, IconSize size,String cssClass)
        {
            return RenderSpan(title, GetDefaultIcon(type), size, cssClass);
        }

        public static string RenderSpan(String title, CommandIcon icon, IconSize size)
        {
            return RenderSpan(title, icon, size, "");
        }

        /// <summary>
        ///     Span with Icon with ToolTip, icon, icon size and other css Class
        /// </summary>
        /// <param name="title">ToolTip</param>
        /// <param name="icon">Icon type</param>
        /// <param name="size">Icon size</param>
        /// <param name="cssClass">other css classes</param>
        /// <returns></returns>
        public static string RenderSpan(String title, CommandIcon icon, IconSize size, String cssClass)
        {
            String render = "<span title=\"{0}\" class=\"img_span {1} {2}\"></span>";
            return string.Format(render, title, icon.GetStringValue() + size.GetStringValue(), cssClass);
        }
        #endregion



        public static CommandIcon GetDefaultIcon(RepositoryItemType type)
        {
            switch (type) { 
                case RepositoryItemType.VideoStreaming:
                case RepositoryItemType.Multimedia:
                    return CommandIcon.PlayMultimediaItem;
                case RepositoryItemType.ScormPackage:
                    return CommandIcon.PlayScormItem;
                case RepositoryItemType.FileStandard:
                    return CommandIcon.Document;
                default:
                    return CommandIcon.None;
            }
        }
        #region "Enum"
            public enum HTMLinputClass
            {
                [StringValue("img_link")]
                Link = 0,
                [StringValue("img_btn")]
                Button = 1
            }
            public enum LinkTarget
            {
                _self = 0,
                _blank = 1,
                _parent = 2,
                _top =3,
            }
            
            public enum CommandIcon {
                [StringValue("")]
                None = -1,
                [StringValue("ico_info")]
                Info = 0,
                [StringValue("ico_add")]
                Add = 1,
                [StringValue("ico_edit")]
                Edit = 2,
                [StringValue("ico_delete")]
                Delete = 3,
                [StringValue("ico_is_new_item")]
                IsNewItem = 4,
                [StringValue("ico_copy")]
                Copy = 5,
                [StringValue("ico_playscorm")]
                PlayScormItem = 6,
                [StringValue("ico_playmmd")]
                PlayMultimediaItem = 7,
                [StringValue("ico_download")]
                DownloadItem = 8,
                [StringValue("ico_settings")]
                Settings = 9,
                [StringValue("ico_stat")]
                Statistics = 10,
                [StringValue("ico_export_pdf")]
                ExportPDF = 11,
                [StringValue("ico_export_rtf")]
                ExportRTF = 12,
                [StringValue("ico_export_xls")]
                ExportXLS = 13,
                [StringValue("ico_help")]
                Help = 14,
                [StringValue("ico_com_perm")]
                CommunityPermission=14,
                [StringValue("ico_usr_perm")]
                UserPermission = 15,
                [StringValue("ico_preview")]
                Preview = 16,
                [StringValue("ico_search")]
                Search = 17,
                [StringValue("ico_import")]
                Import = 18,
                [StringValue("ico_publish")]
                Publish = 19,
                [StringValue("ico_export")]
                Export = 20,
                [StringValue("ico_hasnews")]
                HasNews = 21,
                [StringValue("ico_document")]
                Document = 22,
                [StringValue("ico_move")]
                Move = 23,
                [StringValue("ico_moveup")]
                MoveUp = 24,
                [StringValue("ico_movedown")]
                MoveDown = 25,
                [StringValue("ico_moveleft")]
                MoveLeft = 26,
                [StringValue("ico_moveright")]
                MoveRight = 27,
                [StringValue("ico_movetop")]
                MoveTop = 28,
                [StringValue("ico_movebottom")]
                MoveBottom = 29,
                [StringValue("ico_folder")]
                Folder = 30,
                [StringValue("ico_trash")]
                VirtualDelete = 32,
                [StringValue("ico_recover")]
                VirtualUndelete = 33,
                [StringValue("ico_move_vertical")]
                MoveVertical = 34,
                [StringValue("ico_move_horizontal")]
                MoveHorizontal = 35,
                [StringValue("ico_export_xml")]
                ExportXML = 36,
                [StringValue("ico_export_csv")]
                ExportCSV = 37,
                [StringValue("ico_lk_forum")]
                LinkForum = 38,
                [StringValue("ico_lk_glossary")]
                LinkGlossary = 39,
                [StringValue("ico_lk_mail")]
                LinkMail = 40,
                [StringValue("ico_lk_quest")]
                LinkQuest = 41,
                [StringValue("ico_lk_workbook")]
                LinkworkBook = 42,
                [StringValue("ico_is_favorite")]
                IsFavorite = 31,
                [StringValue("ico_not_favorite")]
                NotFavorite = 43,
            }

        #endregion
    }
    public enum IconSize
    {
        [Description("13x13")]
        [StringValue("_xs")]
        Smaller = 4,
        [Description("16x16")]
        [StringValue("_s")]
        Small = 1,
        [Description("24x24")]
        [StringValue("_m")]
        Medium = 0,
        [Description("30x30")]
        [StringValue("_l")]
        Large = 2
    }
}