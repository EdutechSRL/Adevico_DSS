using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.FileRepository.Domain.ScormSettings
{

    [Serializable()]
    public class dtoScormPackageSettings : dtoScormSettingsEvaluationBase
    {
        public virtual EvaluationType EvaluationType { get; set; }
        public virtual List<dtoScormOrganizationSettings> Children { get; set; }
        public virtual List<dtoScormActivitySettings> Activities { get; set; }

        #region "Settings validity"
            public virtual DateTime? ValidUntil { get; set; }
            public virtual Boolean IsCurrent { get; set; }
        #endregion
       
        public dtoScormPackageSettings()
        {
            Activities = new List<dtoScormActivitySettings>();
            Children = new List<dtoScormOrganizationSettings>();
            Type = ScormSettingsType.p;
        }


        public static dtoScormPackageSettings CreateFrom(ScormPackageSettings settings, liteRepositoryItemVersion version )
        {
            dtoScormPackageSettings item = new dtoScormPackageSettings();
            item.Id = settings.Id;
            item.CheckScore = settings.CheckScore;
            item.CheckScormCompletion = settings.CheckScormCompletion;
            item.CheckTime = settings.CheckTime;
            item.EvaluationType = settings.EvaluationType;
            item.IsCurrent = settings.IsCurrent;
            item.MinScore = settings.MinScore;
            item.MinTime = settings.MinTime;
            item.Name = version.DisplayName;
            item.UseScoreScaled = settings.UseScoreScaled;
            item.ValidUntil = settings.ValidUntil;

            item.Children.AddRange(settings.Organizations.Select(o => dtoScormOrganizationSettings.CreateFrom(o, item)).ToList());
            if (item.Children.Any())
                item.DataChildren = String.Join(",", item.Children.Select(i=> i.DataId).ToList().Union(item.Children.Select(i => i.DataChildren).ToList()));
            return item;
        }                
    }
}