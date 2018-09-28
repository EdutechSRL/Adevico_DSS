using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entity = Comol.Entity;

namespace lm.Comol.Modules.Standard.Skin.Business
{
    public class SkinHtmlHelper
    {
        /// <summary>
        /// Data una Skin, restituisce l'HTML relativo ad un logo
        /// </summary>
        /// <param name="BaseVirtualPath">Il percorso web base dell'applicazione</param>
        /// <param name="SkinID">L'ID della skin</param>
        /// <param name="Logo">Il nome dell'immagine</param>
        /// <returns>Una stringa con il codice HTML del logo.</returns>
        public static string RenderLogo(string BaseVirtualPath, Int64 SkinID, Domain.Logo Logo)
        {
            String Html = "";
            Html += "<img src=\"" + SkinFileManagement.GetLogoVirtualFullPath(BaseVirtualPath, SkinID, Logo.Id, Logo.ImageUrl);
            Html += "\" alt=\"" + Logo.Alt;
            Html += "\" title=\"" + Logo.Alt;
            Html += "\" class=\"logo\"/>";


            if (Logo.Link != "")
            {
                Html = "<a href=\"" + Logo.Link + "\" target=\"_blank\" >" + Html + "</a>";
            }

            return Html;
        }

        public static string RenderVoidLogo()
        {
            return "&nbsp;";

            //String Html = "";
            //Html += "<img src=\"" + SkinFileManagement.GetLogoVirtualFullPath(BaseVirtualPath, SkinID, Logo.Id, Logo.ImageUrl);
            //Html += "\" alt=\"" + Logo.Alt;
            //Html += "\" title=\"" + Logo.Alt;
            //Html += "\" class=\"logo\"/>";


            //if (Logo.Link != "")
            //{
            //    Html = "<a href=\"" + Logo.Link + "\" target=\"_blank\" >" + Html + "</a>";
            //}

            //return Html;
        }
        public static Domain.HTML.HTMLSkin RenderSkin(string BaseVirtualPath, Domain.Skin Skin, string LangCode, string DefLangCode)
        {
            Domain.HTML.HTMLSkin HtmlSk = new Domain.HTML.HTMLSkin();

            //Loghi Header
            if (Skin.HeaderLogos != null)
            {
                Domain.Logo IntLogo = (from Domain.HeaderLogo HL in Skin.HeaderLogos where HL.LangCode == LangCode select HL).FirstOrDefault();
                if(IntLogo == null)
                {
                    IntLogo = (from Domain.HeaderLogo HL in Skin.HeaderLogos where HL.LangCode == DefLangCode select HL).FirstOrDefault();
                }

                if (IntLogo != null)
                {
                    HtmlSk.HtmlHeadLogo = RenderLogo(BaseVirtualPath, Skin.Id, IntLogo);
                }
            }
            

            // Loghi Footer
            if (Skin.FooterLogos != null)
            {
                foreach (Domain.FooterLogo HL in Skin.FooterLogos)
                {
                    if (HL.Languages != null && HL.Languages.Count > 0)
                    {
                        Boolean found = false;
                        foreach (Domain.LogoToLang Ltl in HL.Languages)
                        {
                            if (Ltl.LangCode == LangCode)
                            { found = true; break; }
                        }

                        if (found)
                        {
                            HtmlSk.HtmlFooterLogos.Add(RenderLogo(BaseVirtualPath, Skin.Id, HL));
                        }
                    }
                }
            }

            if (!HtmlSk.HtmlFooterLogos.Any() && Skin.OverrideVoidFooterLogos)
            {
                HtmlSk.HtmlFooterLogos.Add(RenderVoidLogo());
            }

            // Testo Footer
            string LangTxt = "";
            string DefTxt = "";
            foreach (Domain.FooterText Txt in Skin.FooterText)
            {
                if (Txt.LangCode == LangCode)
                {
                    LangTxt = Txt.Value;
                    break;
                }
                else if (Txt.LangCode == DefLangCode)
                {
                    DefTxt = Txt.Value;
                }
            }

            if (LangTxt != "")
            { HtmlSk.FooterText = LangTxt; }
            else
            { HtmlSk.FooterText = DefTxt; }
            
            //template
            if (Skin.HeaderTemplate != null) { HtmlSk.HeaderTemplate = Skin.HeaderTemplate.Css; }
            if (Skin.FooterTemplate != null) { HtmlSk.FooterTemplate = Skin.FooterTemplate.Css; }

            return HtmlSk;
        }

        public static string RenderDefLogo(Entity.Configuration.SkinSettings.Logo Logo, String BaseUrl)
        {
            String Html = "";

            Html += "<img src=\"" + BaseUrl + Logo.Url;
            Html += "\" alt=\"" + Logo.Alt;
            Html += "\" title=\"" + Logo.Alt;
            Html += "\" class=\"logo\"/>";

            if (Logo.Link != "")
            {
                Html = "<a href=\"" + Logo.Link + "\" target=\"_blank\" >" + Html + "</a>";
            }

            return Html;
        }
    }
}
