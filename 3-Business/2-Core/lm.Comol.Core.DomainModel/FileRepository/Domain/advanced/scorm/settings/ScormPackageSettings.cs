using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.FileRepository.Domain.ScormSettings
{
    [Serializable()]
    public class ScormPackageSettings : DomainBaseObjectIdLiteMetaInfo<long>
    {
        #region "Item identifier"  
            public virtual long IdItem { get; set; }
            public virtual Guid UniqueIdItem { get; set; }
            public virtual long IdVersion { get; set; }
            public virtual Guid UniqueIdVersion { get; set; }
            public virtual RepositoryIdentifier Repository { get; set; }
        #endregion

        #region "SCORM details"
            /// <summary>
            /// Stringa con il GUID del pacchetto (per allineamento dati play)
            /// </summary>
            public virtual String PackageGuid { get; set; }
            public virtual String PackagePath { get; set; }
        #endregion

        public virtual IList<ScormOrganizationSettings> Organizations { get; set; }

        #region "Settings validity"
            public virtual DateTime? ValidUntil { get; set; }
            public virtual Boolean IsCurrent { get; set; }
        #endregion

        #region "Completion settings"
            public virtual long ActivityCount { get; set; } 
            public virtual Boolean IsValid { get; set; }
            public virtual EvaluationType EvaluationType { get; set; }
            public virtual Boolean CheckScormCompletion { get; set; }
            public virtual Boolean CheckTime { get; set; }
            public virtual long MinTime { get; set; }
            public virtual Boolean CheckScore { get; set;}
            public virtual Boolean UseScoreScaled { get; set; }
            public virtual Decimal MinScore { get; set; }
        #endregion
       
        public ScormPackageSettings()
        {
            Repository = new RepositoryIdentifier();
            Organizations = new List<ScormOrganizationSettings>();
            IsValid = true;
        }
        public ScormPackageSettings(Boolean isValid)
        {
            Repository = new RepositoryIdentifier();
            Organizations = new List<ScormOrganizationSettings>();
            IsValid = isValid;
        }
        public ScormPackageSettings(String packagePath,Boolean isValid=true)
        {
            Repository = new RepositoryIdentifier();
            PackagePath = packagePath;
            Organizations = new List<ScormOrganizationSettings>();
            IsValid = isValid;
        }

        public virtual ScormPackageSettings Copy(Int32 idPerson, String ipAddress, String proxyIpAddress,  long idItem,
            Guid uniqueId, long idVersion, Guid uniqueIdVersion, RepositoryIdentifier repository, DateTime? validUntil = null, String packagePath = "")
        {
            ScormPackageSettings item = new ScormPackageSettings() { IdItem = idItem, IdVersion = idVersion, UniqueIdItem = uniqueId, UniqueIdVersion = uniqueIdVersion, Repository = repository };
            item.CreateMetaInfo(idPerson, ipAddress, proxyIpAddress);
            item.ActivityCount = ActivityCount;
            item.CheckScore = CheckScore;
            item.CheckScormCompletion = CheckScormCompletion;
            item.CheckTime = CheckTime;
            item.EvaluationType = EvaluationType;
            item.IsCurrent = IsCurrent;
            item.IsValid = IsValid;
            item.MinScore = MinScore;
            item.MinTime = MinTime;
            item.UseScoreScaled = UseScoreScaled;

            item.PackageGuid = uniqueIdVersion.ToString();
            item.PackagePath = (String.IsNullOrWhiteSpace(packagePath) ? PackagePath.Replace(UniqueIdVersion.ToString() + "\\", uniqueIdVersion.ToString() + "\\") : packagePath);
            item.ValidUntil = validUntil;
            return item;
        }
        public virtual ScormPackageSettings CreateForUpdateSettings(Int32 idPerson, String ipAddress, String proxyIpAddress, liteRepositoryItemVersion version, EvaluationType evaluation, dtoScormItemEvaluationSettings dto)
        {
            ScormPackageSettings item = new ScormPackageSettings() { IdItem = version.IdItem, IdVersion = version.Id, UniqueIdItem = version.UniqueIdItem, UniqueIdVersion = version.UniqueIdVersion, Repository = version.Repository };
            item.CreateMetaInfo(idPerson, ipAddress, proxyIpAddress);
            item.ActivityCount = ActivityCount;
            item.EvaluationType = evaluation;
            item.CheckScore = false;
            item.CheckScormCompletion = false;
            item.CheckTime = false;
            item.MinScore = 0;
            item.MinTime = 0;
            item.UseScoreScaled = false;

            switch(evaluation){
                case ScormSettings.EvaluationType.CustomForPackage:
                    if (dto !=null){
                        item.CheckScore= dto.CheckScore;
                        item.CheckScormCompletion= dto.CheckScormCompletion;
                        item.CheckTime= dto.CheckTime;
                        if (dto.CheckTime)
                        {
                            item.MinTime = dto.MinTime;
                        }
                        if (dto.CheckScore)
                        {
                            item.MinScore = dto.MinScore;
                            item.UseScoreScaled = dto.UseScoreScaled;
                        }
                    }
                    break;
                case ScormSettings.EvaluationType.FromScormEvaluation :
                    item.CheckScormCompletion = true;
                    break;
            }
            item.IsValid = IsValid;
            item.PackageGuid = version.UniqueIdVersion.ToString();
            item.PackagePath = PackagePath;
            item.ValidUntil = null;
            item.IsCurrent = true;
            return item;
        }

        public virtual ScormPackageSettings Copy(Int32 idPerson, String ipAddress, String proxyIpAddress, BaseItemIdentifiers itemIdentifiers, RepositoryIdentifier repository, DateTime? validUntil = null, String packagePath = "")
        {
            return Copy(idPerson, ipAddress, proxyIpAddress, itemIdentifiers.IdItem, itemIdentifiers.UniqueIdItem, itemIdentifiers.IdVersion, itemIdentifiers.UniqueIdVersion, repository, validUntil, packagePath);
        }
    }
}