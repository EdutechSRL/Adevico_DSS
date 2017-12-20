using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    public class DTO_Assignment
    {
        public String CurrentUserName { get; set; }
        public String CurrentCategoryName { get; set; }
        public Boolean IsManager { get; set; }
        public DateTime CreatedOn { get; set; }


        public DTO_Assignment()
        {
            CurrentUserName = "";
            CurrentCategoryName = "";
            IsManager = false;
        }

        private void Create(Domain.Assignment Assignment, String LangCode)
        {
            if (String.IsNullOrEmpty(LangCode))
            {
                LangCode = TicketService.LangMultiCODE;
            }

            if (Assignment.AssignedCategory != null)
            {
                if (Assignment.AssignedCategory.Translations != null)
                {
                    CategoryTranslation trans = Assignment.AssignedCategory.Translations.Where(t => t.LanguageCode == LangCode).FirstOrDefault();
                    if (trans == null)
                    {
                        trans = Assignment.AssignedCategory.Translations.Where(t => t.LanguageCode == TicketService.LangMultiCODE).FirstOrDefault();
                    }

                    if (trans == null)
                        CurrentCategoryName = Assignment.AssignedCategory.Name;
                    else
                        CurrentCategoryName = trans.Name;
                }
            }
            else
            {
                CurrentCategoryName = "";
            }


            if (Assignment.Type == Enums.AssignmentType.Category)
            {
                CurrentUserName = "";
                IsManager = false;
            }
            else
            {
                IsManager = (Assignment.Type == Enums.AssignmentType.Manager);

                if (Assignment.AssignedTo != null)
                {
                    if (Assignment.AssignedTo.Person != null)
                    {
                        CurrentUserName = Assignment.AssignedTo.Person.SurnameAndName;
                    }
                    else
                    {
                        CurrentUserName = Assignment.AssignedTo.Sname + " " + Assignment.AssignedTo.Name;
                    }
                }
            }

            if (Assignment.CreatedOn != null)
                CreatedOn = (DateTime)Assignment.CreatedOn;
        }

        public DTO_Assignment(Domain.Assignment Assignment, String LangCode)
        {
            Create(Assignment, LangCode);
        }


        //public DTO_Assignment(IList<Domain.Assignment> Assignments, String LangCode)
        //{
        //    Create(
        //        Assignments.FirstOrDefault(),
        //        LangCode
        //        );
        //}
    }

    
}
