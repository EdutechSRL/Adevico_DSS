using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using lm.Comol.Core.FileRepository.Domain;
using System.Linq.Expressions;

namespace lm.Comol.Core.FileRepository.Business
{
    public partial class ServiceFileRepository 
    {
        #region "Player"

            public litePlayerSettings PlayerGet(long idPlayer)
            {
                return Manager.Get<litePlayerSettings>(idPlayer);
            }
            public List<litePlayerSettings> PlayerGetSettings()
            {
                List<litePlayerSettings> settings = PlayerGetSettings(s => s.Deleted == DomainModel.BaseStatusDeleted.None).ToList();
                if (settings != null && settings.Any())
                    Manager.DetachList(settings);
                else
                    settings = new List<litePlayerSettings>();
                return settings;
            }

        public litePlayerSettings PlayerScormGetSettings()
        {

            litePlayerSettings setting =
                Manager.GetAll<litePlayerSettings>(
                    s => s.Type == ItemType.ScormPackage
                    ).Skip(0).Take(1).FirstOrDefault();

            if (setting != null)
                Manager.Detach(setting);

            return setting;
            
        }
        public IQueryable<litePlayerSettings> PlayerGetSettings(Expression<Func<litePlayerSettings, bool>> filters)
            {
                return (from q in Manager.GetIQ<litePlayerSettings>() select q).Where(filters);
            }
        #endregion
    }
}