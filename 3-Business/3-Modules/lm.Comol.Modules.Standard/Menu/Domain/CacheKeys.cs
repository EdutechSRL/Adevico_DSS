using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Menu.Domain
{
    public static class CacheKeys
    {
        public static String AllMenuBar 
        {
            get { return "Menubar_"; }
        }
        public static String MenuBar(MenuBarType type)
        {
           return "Menubar_" + type.ToString();
        }
        public static String RenderAllCommunity
        {
             get { return "MenuCommunity"; }
        }
        public static String RenderCommunity(int idCommunity)
        {
            return "MenuCommunity_" + idCommunity.ToString();
        }
        public static String RenderCommunity(int idCommunity, int idRole)
        {
            return RenderCommunity(idCommunity) + "_" + idRole.ToString();
        }
        public static String RenderCommunity(int idCommunity, int idRole, int idLanguage)
        {
            return RenderCommunity(idCommunity,idRole) + "_" + idLanguage.ToString();
        }
        public static String RenderAllPortal
        {
             get { return "MenuPortal"; }
        }
        public static String RenderPortal(MenuBarType type)
        {
            return "MenuPortal_" + type.ToString();
        }
        public static String RenderPortal(MenuBarType type, int IdProfileType)
        {
            return RenderPortal(type) +"_" + IdProfileType.ToString();
        }
        public static String RenderPortal(MenuBarType type, int IdProfileType, int idLanguage)
        {
            return RenderPortal(type,IdProfileType) +"_" + idLanguage.ToString();
        }
        public static String RenderMenu(MenuBarType type)
        {
            return (type == MenuBarType.Portal || type == MenuBarType.PortalAdministration) ? RenderPortal(type) : RenderAllCommunity;
        }
        public static String RenderTopBar()
        {
            return "MenuTopBar_";
        }
        public static String RenderTopBar(int idUser)
        {
            return RenderTopBar() + idUser.ToString();
        }
        public static String RenderTopBar(int idUser, int idLanguage)
        {
            return RenderTopBar(idUser) + "_" + idLanguage.ToString();
        }
    }
}