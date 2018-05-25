using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using lm.Comol.Core.FileRepository.Domain;
using System.Linq.Expressions;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain.ScormSettings;

namespace lm.Comol.Core.FileRepository.Business
{
    public partial class ServiceFileRepository
    {
        public ScormPackageSettings ScormPackageSettingsClone(long idSettings, long idItem, Guid uniqueId, long idVersion, Guid uniqueIdVersion, RepositoryIdentifier repository )
        {
            ScormPackageSettings clone = null;
            try
            {
                litePerson person = GetValidPerson(UC.CurrentUserID);
                ScormPackageSettings settings = Manager.Get<ScormPackageSettings>(idSettings);
                if (person != null && settings!=null)
                {
                    Manager.BeginTransaction();
                    clone = settings.Copy(person.Id, UC.IpAddress, UC.ProxyIpAddress, idItem, uniqueId, idVersion, uniqueIdVersion, repository);
                    Manager.SaveOrUpdate(clone);
                    if (settings.Organizations.Any())
                    {
                        foreach(ScormOrganizationSettings o in settings.Organizations){
                            ScormOrganizationSettings organization = o.Copy(clone.Id);
                             Manager.SaveOrUpdate(organization);
                             foreach (ScormItemSettings source in o.Items.Where(i=>i.IdParentItem==0))
                             {
                                 ScormItemSettings nItem = source.Copy(clone.Id, organization.Id, 0);
                                 Manager.SaveOrUpdate(nItem);
                                 ItemSettingsClone(nItem,source.Id, o.Items.Where(i => i.IdParentItem > 0));
                             }
                             clone.Organizations.Add(organization);
                        }
                    }
                    if (settings.IdItem == idItem && settings.IdVersion == idVersion)
                    {
                        settings.IsCurrent = false;
                        settings.ValidUntil = DateTime.Now;
                        clone.IsCurrent = true;
                        Manager.SaveOrUpdate(clone);
                        Manager.SaveOrUpdate(settings);
                    }
                    Manager.Commit();
                }
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                clone = null;
            }
            return clone;
        }
        private void ItemSettingsClone(ScormItemSettings nFather,long idOldFather, IEnumerable<ScormItemSettings> sourceItems)
        {
            foreach (ScormItemSettings source in sourceItems.Where(i => i.IdParentItem == idOldFather))
            {
                ScormItemSettings nItem = source.Copy(nFather.IdScormPackageSettings, nFather.IdScormOrganizationSettings, nFather.Id);
                Manager.SaveOrUpdate(nItem);
                if (sourceItems.Any(i => i.IdParentItem == source.Id))
                    ItemSettingsClone(nItem, source.Id, sourceItems);
            }
        }

        public ScormPackageSettings ScormPackageSettingsSave(long idItem, long idVersion, long idSettings, EvaluationType evaluation, List<dtoScormItemEvaluationSettings> items)
        {
            ScormPackageSettings aSettings = null;
            try
            {
                litePerson person = GetValidPerson(UC.CurrentUserID);
                ScormPackageSettings settings = Manager.Get<ScormPackageSettings>(idSettings);
                if (person != null && settings != null)
                {
                    liteRepositoryItemVersion version = Manager.Get<liteRepositoryItemVersion>(idVersion);
                    if (version != null && version.IdItem == idItem)
                    {
                        Manager.BeginTransaction();

                        aSettings = settings.CreateForUpdateSettings(
                            person.Id, 
                            UC.IpAddress, 
                            UC.ProxyIpAddress, 
                            version, 
                            evaluation, 
                            items.Where(i => i.ForPackage).FirstOrDefault());



                        Manager.SaveOrUpdate(aSettings);
                        if (settings.Organizations.Any())
                        {
                            foreach (ScormOrganizationSettings o in settings.Organizations)
                            {
                                ScormOrganizationSettings organization = o.Copy(aSettings.Id);
                                Manager.SaveOrUpdate(organization);
                                foreach (ScormItemSettings source in o.Items.Where(i => i.IdParentItem == 0))
                                {
                                    ScormItemSettings nItem = source.CreateForUpdateSettings(aSettings.Id, organization.Id, 0,evaluation,items.Where(i=>i.Id==source.Id).FirstOrDefault());
                                    Manager.SaveOrUpdate(nItem);
                                    organization.Items.Add(nItem);
                                    ItemSettingsClone(organization,nItem, source.Id, o.Items.Where(i => i.IdParentItem > 0), evaluation, items);
                                }
                                aSettings.Organizations.Add(organization);
                            }
                        }
                        List<ScormPackageSettings> pSettings = (from s in Manager.GetIQ<ScormPackageSettings>() where s.IdVersion==idVersion && s.IdItem==idItem && s.IsCurrent && s.Id != aSettings.Id select s).ToList();
                        if (pSettings.Any()){
                            DateTime date = DateTime.Now;
                            foreach(ScormPackageSettings s in pSettings){
                                s.IsCurrent = false;
                                s.ValidUntil = DateTime.Now;
                                s.UpdateMetaInfo(person.Id, UC.IpAddress,UC.ProxyIpAddress,date );
                            }
                            Manager.SaveOrUpdateList(pSettings);
                        }
                    }
                    Manager.Commit();
                }
            }
            catch (Exception ex)
            {

            }
            return aSettings;
        }
        private void ItemSettingsClone(ScormOrganizationSettings orgFather, ScormItemSettings nFather, long idOldFather, IEnumerable<ScormItemSettings> sourceItems, EvaluationType evaluation, List<dtoScormItemEvaluationSettings> items)
        {
            foreach (ScormItemSettings source in sourceItems.Where(i => i.IdParentItem == idOldFather))
            {
                ScormItemSettings nItem = source.CreateForUpdateSettings(nFather.IdScormPackageSettings, nFather.IdScormOrganizationSettings, nFather.Id, evaluation, items.Where(i => i.Id == source.Id).FirstOrDefault());
                Manager.SaveOrUpdate(nItem);
                orgFather.Items.Add(nItem);
                if (sourceItems.Any(i => i.IdParentItem == source.Id))
                    ItemSettingsClone(orgFather,nItem, source.Id, sourceItems, evaluation, items);
            }
        }
        public dtoScormPackageSettings ScormPackageGetDtoCompletionSettings(long idItem, long idVersion, long idSettings=0){
            liteRepositoryItemVersion version = ItemGetVersion(idItem, idVersion);
            if (version==null)
                return null;
            else
                return ScormPackageGetDtoCompletionSettings(version, idSettings);
        }
        public dtoScormPackageSettings ScormPackageGetDtoCompletionSettings(liteRepositoryItemVersion version, long idSettings = 0)
        {
            dtoScormPackageSettings item = null;
            try
            {
                ScormPackageSettings settings = (from s in Manager.GetIQ<ScormPackageSettings>() 
                                                 where (idSettings==0 || idSettings==s.Id) && s.IdItem== version.IdItem && s.IdVersion== version.Id select s).OrderByDescending(s=> s.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                if (settings != null)
                {
                    item = dtoScormPackageSettings.CreateFrom(settings, version);
                }
            }
            catch (Exception ex) { 
            }
            return item;
        }
    }
}