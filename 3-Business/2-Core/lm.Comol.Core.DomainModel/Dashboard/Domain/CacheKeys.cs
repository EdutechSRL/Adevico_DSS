using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Dashboard.Domain
{
    public static class CacheKeys
    {
        public static String AllDashboard
        {
            get { return "Dashboard_"; }
        }
        public static String AllUserDashboard
        {
            get { return AllDashboard +"_UserDashboard_"; }
        }
        public static String AllDashboardTiles
        {
            get { return AllDashboard + "_DashboardTiles_"; }
        }
        public static String Dashboard(DashboardType type)
        {
            return AllDashboard + type.ToString() + "_";
        }
        public static String Dashboard(Int32 idCommunity)
        {
            return (idCommunity > 0) ? Dashboard(DashboardType.Community) + idCommunity.ToString(): ((idCommunity == 0) ? Dashboard(DashboardType.Portal) : Dashboard(DashboardType.AllCommunities));
        }
        public static String DashboardTiles(long idDashboard)
        {
            return AllDashboardTiles + idDashboard.ToString() + "_";
        }
        public static String DashboardUserTiles(Int32 idUser)
        {
            return AllUserDashboard+ idUser.ToString() + "_";
        }
        public static String DashboardUserTiles(long idDashboard, Int32 idUser)
        {
            return DashboardUserTiles(idUser) + idDashboard.ToString() + "_";
        }
        public static String DashboardUserTiles(long idDashboard, Int32 idUser, TileType type)
        {
            return DashboardUserTiles(idDashboard, idUser) + type.ToString() + "_";
        }
        public static String AllCurrentSettings
        {
            get { return "UserCurrentDashboardSettings_"; }
        }
        public static String CurrentDashboardSettings(Int32 idUser, Int32 idCommunity)
        {
            return AllCurrentSettings + idCommunity.ToString() + "_" + idUser.ToString();
        }
        public static String CurrentDashboardSettings(Int32 idCommunity)
        {
            return AllCurrentSettings + idCommunity.ToString() + "_" ;
        }
        public static String UserSettings(Int32 idUser) {
            return "UserDashboardSettings_" + idUser.ToString() + "_";
        }

        public static String UserSettings(Int32 idUser,Int32 idCommunity)
        {
            return UserSettings(idUser) + idCommunity.ToString();
        }
        //public static String RenderAllCommunity
        //{
        //     get { return "MenuCommunity"; }
        //}
        //public static String RenderCommunity(int idCommunity)
        //{
        //    return "MenuCommunity_" + idCommunity.ToString();
        //}
        //public static String RenderCommunity(int idCommunity, int idRole)
        //{
        //    return RenderCommunity(idCommunity) + "_" + idRole.ToString();
        //}
        //public static String RenderCommunity(int idCommunity, int idRole, int idLanguage)
        //{
        //    return RenderCommunity(idCommunity,idRole) + "_" + idLanguage.ToString();
        //}
        //public static String RenderAllPortal
        //{
        //     get { return "MenuPortal"; }
        //}
        //public static String RenderPortal(MenuBarType type)
        //{
        //    return "MenuPortal_" + type.ToString();
        //}
        //public static String RenderPortal(MenuBarType type, int IdProfileType)
        //{
        //    return RenderPortal(type) +"_" + IdProfileType.ToString();
        //}
        //public static String RenderPortal(MenuBarType type, int IdProfileType, int idLanguage)
        //{
        //    return RenderPortal(type,IdProfileType) +"_" + idLanguage.ToString();
        //}
        //public static String RenderMenu(MenuBarType type)
        //{
        //    return (type == MenuBarType.Portal || type == MenuBarType.PortalAdministration) ? RenderPortal(type) : RenderAllCommunity;
        //}
        //public static String RenderTopBar()
        //{
        //    return "MenuTopBar_";
        //}
        //public static String RenderTopBar(int idUser)
        //{
        //    return RenderTopBar() + idUser.ToString();
        //}
        //public static String RenderTopBar(int idUser, int idLanguage)
        //{
        //    return RenderTopBar(idUser) + "_" + idLanguage.ToString();
        //}
    }
}