using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.ScormSettings
{
    [Serializable()]
    public class ScormItemSettings
    {
        public virtual long Id { get; set; }
        /// <summary>
        /// id univoco del pacchetto di appartenenza !
        /// </summary>
        public virtual long IdScormPackageSettings { get; set; }
        /// <summary>
        /// Id univoco dell'organizzazione di appartenenza
        /// </summary>
        public virtual long IdScormOrganizationSettings { get; set; }
        /// <summary>
        /// Id univoco del padre
        /// </summary>
        public virtual long IdParentItem { get; set; }
     
        #region "SCORM details"
            //Dati pacchetto
            public virtual String Title { get; set; }
            public virtual String ScormType { get; set; }
            public virtual String ActivityId { get; set; }
            public virtual Boolean IsLeaf { get; set; }
            public virtual Boolean IsVisible { get; set; }
        #endregion

        #region "Completion settings"
            public virtual Boolean CheckScormCompletion { get; set; } 
            public virtual Boolean CheckTime { get; set; }
            public virtual long MinTime { get; set; }
            public virtual Boolean CheckScore { get; set; }
            public virtual Decimal MinScore { get; set; }
            public virtual Boolean UseScoreScaled { get; set; }
        #endregion

        public ScormItemSettings()
        {
            IsVisible = true;
        }

        public ScormItemSettings(String title,String idActivity,Boolean isVisible)
        {
            ActivityId = idActivity;
            Title = title;
            IsVisible = isVisible;
        }

        public virtual ScormItemSettings Copy(long idPackage,long idOrganization, long idParentItem)
        {
            ScormItemSettings item = new ScormItemSettings() { IdScormPackageSettings = idPackage, IdScormOrganizationSettings = idOrganization, IdParentItem = idParentItem };
            item.IsVisible = IsVisible;
            item.CheckScormCompletion = CheckScormCompletion;
            item.CheckTime = CheckTime;
            item.MinTime = MinTime;
            item.CheckScore = CheckScore;
            item.MinScore = MinScore;
            item.UseScoreScaled = UseScoreScaled;
            item.Title = Title;
            item.ScormType = ScormType;
            item.ActivityId = ActivityId;
            item.IsLeaf = IsLeaf;
            return item;
        }
        public virtual ScormItemSettings CreateForUpdateSettings(long idPackage, long idOrganization, long idParentItem, EvaluationType evaluation, dtoScormItemEvaluationSettings dto)
        {
            ScormItemSettings item = new ScormItemSettings() { IdScormPackageSettings = idPackage, IdScormOrganizationSettings = idOrganization, IdParentItem = idParentItem };
            item.IsVisible = IsVisible;
            item.Title = Title;
            item.ScormType = ScormType;
            item.CheckScore = false;
            item.CheckScormCompletion = false;
            item.CheckTime = false;
            item.MinScore = 0;
            item.MinTime = 0;
            item.UseScoreScaled = false;
            item.ActivityId = ActivityId;
            item.IsLeaf = IsLeaf;

            if (evaluation== EvaluationType.CustomForActivities && dto !=null)
            {
                item.CheckScore = dto.CheckScore;
                item.CheckScormCompletion = dto.CheckScormCompletion;
                item.CheckTime = dto.CheckTime;
                if (dto.CheckTime)
                    item.MinTime = dto.MinTime;
                if (dto.CheckScore)
                {
                    item.MinScore = dto.MinScore;
                    item.UseScoreScaled = dto.UseScoreScaled;
                }
            }

            return item;
        }
    }
}