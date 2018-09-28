using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoDateException
    {
        public virtual long Id { get; set; }
        public virtual long IdCalendar { get; set; }
        public virtual long IdProject { get; set; }
        public virtual String Name { get; set; }
        public virtual Int32 Day { get; set; }
        public virtual Int32 Month { get; set; }
        public virtual Int32 Year { get; set; }
        public virtual DateTime? Date { get; set; }
        public virtual Boolean EveryYear { get; set; }
        public virtual Boolean EveryMonth { get; set; }
        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
        public virtual DateType Type { get; set; }
        public virtual Boolean Include { get; set; }
        public dtoDateException()
        {

        }

        public dtoDateException(ProjectDateException exception)
        {
            Id = exception.Id;
            IdCalendar = (exception.Calendar != null) ? exception.Calendar.Id : 0;
            IdProject = (exception.Project != null) ? exception.Project.Id : 0;
            Name = exception.Name;
            Day = exception.Day;
            Month = exception.Month;
            Year = exception.Year;
            Date = exception.Date;
            EveryYear = exception.EveryYear;
            EveryMonth = exception.EveryMonth;
            FromDate = exception.FromDate;
            ToDate = exception.ToDate;
            Type = exception.Type;
            Include = exception.Include;

        }
    }
}