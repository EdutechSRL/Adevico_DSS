using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Helper = lm.Comol.Core.DomainModel.Helpers;


namespace lm.Comol.Core.BaseModules.CommunityExpiration.Business
{
    public static class CacheHelper
    {

        //#region Config Cache
        //public static string CacheKey_SysRoleDuration = "SystemRoleDuration_";

        //public static IDictionary<int, int> SystemRoleDurationGet(int CommunityTypeId)
        //{
        //    return Helper.CacheHelper.Find<IDictionary<int, int>>(CacheKey_SysRoleDuration + CommunityTypeId.ToString());
        //}

        //public static void SystemRoleDurationSet(int CommunityId, IDictionary<int, int> values)
        //{
        //    Helper.CacheHelper.AddToCache<IDictionary<int, int>>(CacheKey_SysRoleDuration + CommunityId.ToString(), values, new TimeSpan(6, 0, 0));
        //}

        //public static void SystemRoleReset()
        //{
        //    Helper.CacheHelper.PurgeCacheItems(CacheKey_SysRoleDuration);
        //}

        //public static void SystemRoleReset(int ComTypeId)
        //{
        //    Helper.CacheHelper.PurgeCacheItems(CacheKey_SysRoleDuration + ComTypeId.ToString());
        //}
        //#endregion


        #region Commnuity Cache

        public static string CacheKey_ComRoleDuration = "CommunityRoleDuration_";

        public static IDictionary<int, int> CommunityRoleDurationGet(int CommunityId)
        {
            return Helper.CacheHelper.Find<IDictionary<int, int>>(CacheKey_ComRoleDuration + CommunityId.ToString());
        }


        public static void CommunityRoleDurationSet(int CommunityId, IDictionary<int, int> values)
        {
            Helper.CacheHelper.AddToCache<IDictionary<int, int>>(CacheKey_ComRoleDuration + CommunityId.ToString(), values, new TimeSpan(6, 0, 0));
        }

        public static void CommunityRoleReset()
        {
            Helper.CacheHelper.PurgeCacheItems(CacheKey_ComRoleDuration);
        }

        public static void CommunityRoleReset(int comId)
        {
            Helper.CacheHelper.PurgeCacheItems(CacheKey_ComRoleDuration + comId.ToString());
        }

        #endregion

        #region User Cache

        public static string CacheKey_UserDuration = "UserSubscriptionDuration_";

        public static Dictionary<int, Domain.dtoUserExpiration> UserDurationGet(int UserId)
        {
            return Helper.CacheHelper.Find<Dictionary<int, Domain.dtoUserExpiration>>(CacheKey_UserDuration + UserId.ToString());
        }


        public static void UserDurationSet(int UserId, Dictionary<int, Domain.dtoUserExpiration> values)
        {
            Helper.CacheHelper.AddToCache<Dictionary<int, Domain.dtoUserExpiration>>(CacheKey_UserDuration + UserId.ToString(), values, new TimeSpan(6, 0, 0));
        }

        public static void UserReset()
        {
            Helper.CacheHelper.PurgeCacheItems(CacheKey_UserDuration);
        }

        public static void UserReset(int UserId)
        {
            Helper.CacheHelper.PurgeCacheItems(CacheKey_UserDuration + UserId.ToString());
        }

        #endregion

        public static void CacheReset()
        {
            //SystemRoleReset();
            CommunityRoleReset();
            UserReset();
        }

    }
}
