using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.ScormSettings
{
    [Serializable()]
    public class dtoScormActivitySettings : dtoScormSettingsEvaluationBase
    {
        public virtual String OrganizationName { get; set; }
        public virtual String ActivityId { get; set; }
        public dtoScormActivitySettings()
        {
            Type = ScormSettingsType.a;
        }

        public static dtoScormActivitySettings CreateFrom(ScormItemSettings source, dtoScormOrganizationSettings organization, dtoScormPackageSettings package)
        {
            dtoScormActivitySettings activity = new dtoScormActivitySettings();
            activity.Id = source.Id;
            activity.Name = source.Title;
            activity.ActivityId= source.ActivityId;
            activity.CheckScore = source.CheckScore;
            activity.CheckScormCompletion = source.CheckScormCompletion;
            activity.CheckTime = source.CheckTime;

            activity.MinScore = source.MinScore;
            activity.MinTime = source.MinTime;
            activity.UseScoreScaled = source.UseScoreScaled;
            activity.OrganizationName = organization.Name;
            package.Activities.Add(activity);
            return activity;
        }              
    }
}