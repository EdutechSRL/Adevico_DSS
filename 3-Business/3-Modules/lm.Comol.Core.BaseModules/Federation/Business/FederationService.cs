using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Federation.Domain;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Federation.Business
{
    public class FederationService : CoreServices 
    {
        public FederationService(iDataContext oContext) 
                : base(oContext)    //, string xmlPath
        {
           
        }

        public FederationService(iApplicationContext oAppContext)
            : base(oAppContext)    //, string xmlPath
        {
            
        }

        public bool FederationSet(int communityId)
        {
            liteFederation federation =
                (from liteFederation f in Manager.GetIQ<liteFederation>()
                    where f.CommunityId == communityId
                    select f).FirstOrDefault();

            if (federation == null)
            {
                federation = new liteFederation();
                federation.CreateMetaInfo(Manager.GetLitePerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                federation.UpdateMetaInfo(Manager.GetLitePerson(UC.CurrentUserID), UC.IpAddress, UC.ProxyIpAddress);
            }

            federation.CommunityId = communityId;
            federation.Type = Enums.FederationType.TrentinoSviluppo;

            try
            {
                Manager.SaveOrUpdate<liteFederation>(federation);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public Enums.FederationType CommunityFederation(int communityId)
        {

            liteFederation federation = (from liteFederation f in Manager.GetIQ<liteFederation>()
                where f.CommunityId == communityId
                select f).FirstOrDefault();

            if (federation == null || federation.CommunityId <= 0)
                return Enums.FederationType.None;

            return federation.Type;
        }
        
        public litePerson UserGet(int userId)
        {
            return Manager.GetLitePerson(userId);
        }

        public liteCommunity CommunityGet(int comId)
        {
            return Manager.GetLiteCommunity(comId);
        }
    }
}
