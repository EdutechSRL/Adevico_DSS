using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath
{
        [Serializable()]
        public static class TemplateEduPathPlaceHolders
        {
            private static String tagString = "{0}ep.{1}{2}";
            public static String OpenTag { get { return "["; } }
            public static String CloseTag { get { return "]"; } }


            private static String tagQuizList = "<ul data-foreach='ep.QuizList'><li>{0} {1} {2}</li></ul>";
            private static String tagQuizTable = "<table data-foreach='ep.QuizList'><tr><td>{0}</td><td>{1}</td><td>{2}</td></tr></table>";


            private static Dictionary<EduPathPlaceHoldersType, String> _PlaceHolders = new Dictionary<EduPathPlaceHoldersType, String>();
            public static Dictionary<EduPathPlaceHoldersType, String> PlaceHolders()
            {
                if (_PlaceHolders.Count == 0)
                {
                    //_PlaceHolders = (from e in Enum.GetValues(typeof(EduPathPlaceHoldersType)).OfType<EduPathPlaceHoldersType>()
                    //                 where e != EduPathPlaceHoldersType.None && e != EduPathPlaceHoldersType.QuizList && e != EduPathPlaceHoldersType.QuizTable
                    //                 select e).ToDictionary(k=> k, v=> string.Format(tagString, OpenTag,v.ToString(), CloseTag));

                    _PlaceHolders = (from e in Enum.GetValues(typeof(EduPathPlaceHoldersType)).OfType<EduPathPlaceHoldersType>()
                                     where e != EduPathPlaceHoldersType.None 
                                     select e).ToDictionary(k => k, v => string.Format(tagString, OpenTag, v.ToString(), CloseTag));

                    

                    //_PlaceHolders.Add(CommonPlaceHoldersType.CommunityName, "[CallName]");
                    //_PlaceHolders.Add(CommonPlaceHoldersType.UserName, "[CallEdition]");
                    //_PlaceHolders.Add(CommonPlaceHoldersType.RoleName, "[SubmissionDay]");
                    //_PlaceHolders.Add(CommonPlaceHoldersType.ProfileType, "[SubmissionTime]");
                    //_PlaceHolders.Add(CommonPlaceHoldersType.SubscriptionOn, "[SubmitterName]");
                    //_PlaceHolders.Add(CommonPlaceHoldersType.CommunityType, "[SubmitterSurname]");
                    //_PlaceHolders.Add(CommonPlaceHoldersType.OrganizationName, "[SubmitterMail]");
                }
                return _PlaceHolders;
            }
            public static String GetPlaceHolder(EduPathPlaceHoldersType type)
            {
                return string.Format(tagString, OpenTag, type.ToString(), CloseTag);

                //String quizName = OpenTag + "ep.QuizList.Name" + CloseTag;
                //String quizDate = OpenTag + "ep.QuizList.Date" + CloseTag;
                //String quizMark = OpenTag + "ep.QuizList.Mark" + CloseTag;

                //switch (type)
                //{                    
                //    case EduPathPlaceHoldersType.QuizList:
                //        return string.Format(tagQuizList, quizName, quizDate, quizMark);                                            
                //    case EduPathPlaceHoldersType.QuizTable:
                //        return string.Format(tagQuizTable, quizName, quizDate, quizMark);                        
                //    default:
                //        return string.Format(tagString, OpenTag, type.ToString(), CloseTag);
                //}

                 
            }

            public static String Translate(string content){
                String translation = content;

                return translation;
            }
            public static String Translate(string content, lm.Comol.Core.DomainModel.iApplicationContext appContext, long idPath, Int32 idUser,SubActivity subActivity)
            {
                String translation = content;
                lm.Comol.Modules.EduPath.BusinessLogic.Service service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(appContext);
                Path path = service.GetPath(idPath);
                if (path != null)
                    translation = Translate(translation, service, path, idUser, subActivity);
                return translation;
            }
            public static String Translate(string content, lm.Comol.Modules.EduPath.BusinessLogic.Service service,Path path , Int32 idUser, SubActivity subActivity)
            {
                String translation = content;
                if (path != null)
                {
                    Int32 IdCRole = service.CommunityRoleId(path.Community.Id,idUser);
                    ILookup<StatusStatistic, ActivityStatistic> statdict = null;

                    DateTime? startEpDate = path.StartDate;
                    if (startEpDate.HasValue)
                        translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathStartedOn), ReplaceChars(startEpDate.Value.ToShortDateString()));
                    else
                        translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathStartedOn), "");

                    DateTime? endDate = service.GetEpEndDate(path, idUser);
                    if(endDate.HasValue)
                        translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathEndedOn), ReplaceChars(endDate.Value.ToShortDateString()));
                    else
                        translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathEndedOn), "");



                    //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.PathName)))
                    translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathName), ReplaceChars(path.Name));
                    if(!String.IsNullOrEmpty(path.PathCode))
                        translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathCode), ReplaceChars(path.PathCode));
                    else
                    {
                        translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathCode), "");
                    }
                    translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathId), ReplaceChars(path.Id.ToString()));


                    //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.UnitCount)))
                    //    translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.UnitCount), path.UnitList.Where(u=> u.Deleted== BaseStatusDeleted.None));
                    if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.PathMinCompletion)))
                        translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathMinCompletion), path.MinCompletion.ToString() + "%");
                    //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.PathMinMark)))
                    //    translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathMinMark), path.MinCompletion.ToString());
                    Boolean timeBased = service.CheckEpType(path.EPType, EPType.Time);

                    Int32 totalAct = -1;
                    Int32 notStarted = -1;
                    Int32 started = -1;
                    Int32 completed = -1;
                    Int32 completedAndPassed = -1;

                    // ANCHE SE è AUTO devo verificare ??
                    //Boolean timeBased = service.CheckEpType(path.EPType, EPType.Time) && service.CheckEpType(path.EPType, EPType.Auto);
                    if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.PathDuration)) && timeBased )
                        translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathDuration), (path.Weight > 0) ? service.ConvertTimeFromLong(path.Weight) : "//");

                    if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.UnitCount)))
                    {
                        short count = service.GetActiveUnitsCount(path.UnitList, idUser, IdCRole, true, false);
                        translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.UnitCount), count.ToString());
                    }
                    if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.ActivitiesCount)))
                    {
                        totalAct = service.GetActiveActivitiesCount(path.UnitList.SelectMany(x => x.ActivityList).ToList(), idUser, IdCRole, true, false);
                        //foreach (var item in path.UnitList)
                        //{
                        //    count += service.GetActiveActivitiesCount(item.ActivityList , idUser, IdCRole, true, false);
                        //}
                        //totalAct = count;
                        translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.ActivitiesCount), totalAct.ToString());
                    }
                    if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.StartedActivities)))
                    {
                        //if (statdict == null)
                        //    statdict = service.ServiceStat.GetStatCount_ByUser(path.Id, idUser, DateTime.Now);
                        

                        //started = statdict[StatusStatistic.BrowsedStarted].Count();

                        started = service.ServiceStat.GetStatCount_ByUserAndStatus(path.Id, idUser, DateTime.Now, StatusStatistic.Started);

                        translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.StartedActivities), started.ToString());
                    }
                    if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.CompletedActivities)))
                    {
                        //if (statdict == null)
                        //    statdict = service.ServiceStat.GetStatCount_ByUser(path.Id, idUser, DateTime.Now);
                        //completed = statdict[StatusStatistic.Started | StatusStatistic.Browsed | StatusStatistic.Completed ].Count();
                        //completed = statdict[StatusStatistic.Started | StatusStatistic.Browsed | StatusStatistic.CompletedPassed].Count();

                        completedAndPassed = service.ServiceStat.GetStatCount_ByUserAndStatus(path.Id, idUser, DateTime.Now, StatusStatistic.Completed);

                        translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.CompletedActivities), completedAndPassed.ToString());

                    }
                    //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.NotStartedActivities)))
                    //{
                    //    //if (statdict == null)
                    //    //    statdict = service.ServiceStat.GetStatCount_ByUser(path.Id, idUser, DateTime.Now);
                    //    //if (totalAct == -1)
                    //    //{
                    //    //    totalAct = service.GetActiveActivitiesCount(path.UnitList.SelectMany(x => x.ActivityList).ToList(), idUser, IdCRole, true, false);
                    //    //}
                    //    //Int32 x = statdict[StatusStatistic.].Count();

                    //    notStarted = totalAct - (started);//service.ServiceStat.GetStatCount_ByUserAndStatus(path.Id, idUser, DateTime.Now, StatusStatistic.None ,true );
                    //    notStarted = (notStarted < 0) ? 0 : notStarted;
                    //    translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.NotStartedActivities), notStarted.ToString());
                    //}

                    //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.CompletedOn)))
                    //{

                        //(!subActivity.UsePathEndDateStatistics || !endDate.HasValue) || (subActivity.UsePathEndDateStatistics && endDate.HasValue && a.CompletedOn <= endDate.Value));

                        PathStatistic pathStatistic = null;

                        if (!subActivity.UsePathEndDateStatistics || !endDate.HasValue)
                            pathStatistic = service.ServiceStat.GetPathStat(path.Id, idUser);
                        else if (subActivity.UsePathEndDateStatistics && endDate.HasValue)
                            pathStatistic = service.ServiceStat.GetPathStat(path.Id, idUser, endDate.Value);

                    if (pathStatistic != null)
                    {
                        //TODO:Check epAuto
                        if (service.ServiceStat.CheckStatusStatistic(pathStatistic.Status, StatusStatistic.Completed) ||
                            service.ServiceStat.CheckStatusStatistic(pathStatistic.Status,
                                StatusStatistic.CompletedPassed))
                        {
                            translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.CompletedOn),
                                pathStatistic.CreatedOn.Value.ToShortDateString());
                        }
                        else
                        {
                            translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.CompletedOn),"");
                        }

                        DateTime? compilationStartedOn = pathStatistic.StartDate;

                        if (compilationStartedOn.HasValue)
                            translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathStartCompilationOn),
                                compilationStartedOn.Value.ToShortDateString());
                        else
                            translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.PathStartCompilationOn), "");


                    }
                         
                        //else
                        //{
                        //    translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.CompletedOn), "###");
                        //}
                    //}  
                }

                return translation;
            }
            public static String Translate(string content,lm.Comol.Core.DomainModel.iApplicationContext appContext, long idPath, Int32 idUser, SubActivity subActivity ,List<dtoQuizInfo> items )
            {
                //return content;

                #region Setup

                lm.Comol.Modules.EduPath.BusinessLogic.Service service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(appContext);

                Path path = service.GetPath(idPath);
                String translation = Translate(content, service, path, idUser, subActivity);

                DateTime? endDate = service.GetEpEndDate(path, idUser);

                #endregion

                if (items.Count > 0 && translation.Contains(OpenTag + "ep.Quiz"))
                {
                    foreach (dtoQuizInfo quiz in items) { 
                        // DATE LE INFO RELATIVE AI QUIZ SISTEMARE l'OUTPUT INTANTO UN ESEMPIO PER VEDERE SE VA
                        var query = quiz.Attempts.Where(a => (!subActivity.UsePathEndDateStatistics || !endDate.HasValue) || (subActivity.UsePathEndDateStatistics && endDate.HasValue && a.CompletedOn <= endDate.Value));

                        //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.QuizName)))
                        //    translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.QuizName), ReplaceChars(quiz.Name));
                        //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.QuizMinMark)))
                        //    translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.QuizMinMark), quiz.MinScore.ToString());
                        //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.QuizMaxMark)))
                        //    translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.QuizMaxMark), quiz.EvaluationScale.ToString());
                        //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.QuizMark)))
                        //    translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.QuizMark), (query.Any()) ? ((int)query.OrderByDescending(a=> a.CompletedOn).FirstOrDefault().RelativeScore).ToString() : "");

                        //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.QuizAttempts)))
                        //    translation = translation.Replace(GetPlaceHolder(EduPathPlaceHoldersType.QuizAttempts), query.Count().ToString());
                       
                        //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.QuizQuestionsNumber)))
                        //{

                        //}
                        //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.QuizList)))
                        //{
                        //}
                        //if (content.Contains(GetPlaceHolder(EduPathPlaceHoldersType.QuizTable)))
                        //{
                        //}
                    }
                }
                return translation;
            }

            public static String ReplaceChars(String value)
            {
                string regex = string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape("<>"));
                System.Text.RegularExpressions.Regex removeInvalidChars = new System.Text.RegularExpressions.Regex(regex, System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant);

                return removeInvalidChars.Replace(value, "");
            }
        }

        [Serializable()]
        public enum EduPathPlaceHoldersType
        {
            None = 0,
            PathName = 1,
            UnitCount = 2,
            ActivitiesCount = 3,
            StartedActivities = 4,
            CompletedActivities = 5,
            //NotStartedActivities = 6,
            
            //QuizName = 7,
            //QuizMinMark = 8,
            //QuizMark = 9,
            //QuizMaxMark = 10,
            //QuizQuestionsNumber = 11,
            //QuizAttempts = 12,
            //QuizList=13,
            //QuizTable= 14,
            //ATTENZIONE: in molti casi i tag QuizList e QuizTable erano tolti dalgi elenchi dei tag...

            PathDuration = 15,
            CompletedOn = 16,
            PathMinCompletion = 17,
            //PathMinMark = 18,
            //PathWeight = 19,
            PathStartedOn = 20,
            PathEndedOn = 21,
            PathStartCompilationOn = 22,
            PathCode = 23,
            PathId = 24
        }
}
