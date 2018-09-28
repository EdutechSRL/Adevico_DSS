using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public enum CellType: int 
    {
        Auto = 0,
        Time = 1,
        Mark = 2,
        StartDate = 3,
        EndDate = 4,
        Deadline= 5,
        QuizCells = 6,
        QuizCompleted = 7,
        SemiCorrectAnswers = 8,
        UngradedAnswers = 9,
        QuestionsSkipped = 10,
        NoEvaluations =11,
        WithEvaluations = 12,
        QuizAttempts = 13,
        //Identifiers =14,
        AgencyStart = 15,
        AgencyEnd = 16,
        AgencyCurrent = 17,
        UserTaxCode = 18,
        QuestionsCount = 19,
        IdUser = 20,
        IdCommunity = 21,
        IdRole = 22,
        IdAgency = 23,
        IdPath = 24,
        IdUnit = 25,
        IdActivity = 26,
        IdSubActivity = 27,
        IdQuestionnaire = 28,
        CorrectAnswers = 29,
        WrongAnswers = 30,
        IdOrganization = 31,
        OrganizationName= 32,
        CommunityName = 33,
        ViewedOn = 34,
        RoleName = 35,
        PathName = 36,
        FirstActivityOn = 37,
        MinCompletion = 38,
        UserSurname = 39,
        UserName = 40,
        UserMail = 41,
        PathTime = 42,
        UserPathCompletion = 43,
    }
}