using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.DocTemplateVers.Business;
using lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport;
using lm.Comol.Core.DomainModel.Helpers;

using System.Xml;

using CpDomain = lm.Comol.Modules.CallForPapers.Domain;
using Adv = lm.Comol.Modules.CallForPapers.Advanced;
using EcoD = lm.Comol.Modules.CallForPapers.AdvEconomic.Domain;
using Eco = lm.Comol.Modules.CallForPapers.AdvEconomic;

namespace lm.Comol.Modules.CallForPapers.Business
{
    public partial class ServiceCallOfPapers
    {
        #region "Init Commission Evaluation"
        public bool EcoCreateEvaluation(long CommissionId)
        {
            if (!EcoEvalClear(CommissionId))
                return false;

            
            Adv.Domain.AdvCommission comm = Manager.Get<Adv.Domain.AdvCommission>(CommissionId);
           
            if (
                comm.Step == null || comm.Step.Type != Adv.StepType.economics
                || comm.Call == null)
                return false;

            var stepsQuery = (from Adv.Domain.AdvStep step in Manager.GetIQ<Adv.Domain.AdvStep>()
                              where step.Type != Adv.StepType.economics
                              && step.Call != null && step.Call.Id == comm.Call.Id
                              select step);

            Adv.Domain.AdvStep prevStep = stepsQuery.Where(s => s.Type == Adv.StepType.custom).OrderByDescending(s => s.Order).Skip(0).Take(1).FirstOrDefault();

            if(prevStep == null)
                prevStep = stepsQuery.Where(s => s.Type == Adv.StepType.validation).Skip(0).Take(1).FirstOrDefault();

            if (prevStep == null || prevStep.Status != Adv.StepStatus.Closed)
                return false;

            IList<Adv.Domain.AdvSubmissionToStep> subStep = Manager.GetAll<Adv.Domain.AdvSubmissionToStep>(
                                                               s => s.Step != null
                                                               && s.Step.Id == prevStep.Id
                                                               && s.Commission == null
                                                               && s.Admitted).OrderBy(s => s.Rank).ToList();

            IList<CpDomain.UserSubmission> AdmitSubmission = (from Adv.Domain.AdvSubmissionToStep sts in subStep
                                                              select sts.Submission).Distinct().ToList();

            IList<EcoD.EconomicEvaluation> ecEvaluations = Manager.GetAll<EcoD.EconomicEvaluation>(
                ecev => ecev.Call != null && ecev.Call.Id == comm.Call.Id
                && ecev.Step != null && ecev.Step.Id == comm.Step.Id
                && ecev.Commission != null && ecev.Commission.Id == comm.Id);

            if (ecEvaluations == null)
                ecEvaluations = new List<EcoD.EconomicEvaluation>();

            foreach (CpDomain.UserSubmission sub in AdmitSubmission)
            {
                EcoD.EconomicEvaluation ecEval = ecEvaluations.FirstOrDefault(ev => ev.Submission != null && ev.Submission.Id == sub.Id);
                bool isNew = false;
                if(ecEval == null)
                {
                    ecEval = new EcoD.EconomicEvaluation();
                    ecEval.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                    
                    ecEval.CurrentMember = null;
                    ecEval.Call = comm.Call;
                    ecEval.Commission = comm;
                    ecEval.Step = comm.Step;
                    ecEval.Submission = sub;
                    ecEval.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                    isNew = true;
                } else
                {
                    ecEval.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                }
                
                ecEval.Status = AdvEconomic.EvalStatus.draft;

               
                ecEval.Tables = EcoTableFromSubmission(sub, ref ecEval);

                try
                {
                    ecEval.Rank = subStep.FirstOrDefault(st => st.Submission != null && st.Submission.Id == sub.Id).Rank;
                    ecEval.AverageRating = subStep.FirstOrDefault(st => st.Submission != null && st.Submission.Id == sub.Id).AverageRating;
                    ecEval.SumRating = subStep.FirstOrDefault(st => st.Submission != null && st.Submission.Id == sub.Id).SumRating;

                } catch (Exception ex)
                {

                }
                

                if (isNew)
                    ecEvaluations.Add(ecEval);
            }

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();


            try
            {
                
                Manager.SaveOrUpdateList<EcoD.EconomicEvaluation>(ecEvaluations);
                Manager.Commit();
            } catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }


        private bool EcoEvalClear(long CommId)
        {

            if(!Manager.IsInTransaction())
                Manager.BeginTransaction();

            IList<EcoD.EconomicItem> items = Manager.GetAll<EcoD.EconomicItem>(i => i.Table != null && i.Table.EcoEvaluation != null && i.Table.EcoEvaluation.Commission != null && i.Table.EcoEvaluation.Commission.Id == CommId);

            try
            {
                Manager.DeletePhysicalList<EcoD.EconomicItem>(items);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            IList<EcoD.EconomicTable> tables = Manager.GetAll<EcoD.EconomicTable>(t => t.EcoEvaluation != null && t.EcoEvaluation.Commission != null && t.EcoEvaluation.Commission.Id == CommId);

            try
            {
                Manager.DeletePhysicalList<EcoD.EconomicTable>(tables);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            IList<EcoD.EconomicEvaluation> evals = Manager.GetAll<EcoD.EconomicEvaluation>(ev => 
                ev.Commission != null && ev.Commission.Id == CommId);

            try
            {
                Manager.DeletePhysicalList<EcoD.EconomicEvaluation>(evals);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }


        private IList<EcoD.EconomicTable> EcoTableFromSubmission(CpDomain.UserSubmission submission, ref EcoD.EconomicEvaluation eval)
        {
            List<EcoD.EconomicTable> tables = new List<EcoD.EconomicTable>();


            long idSubmitterType = (submission.Type != null) ? submission.Type.Id : 0;
            long revId = (submission.Revisions != null && submission.Revisions.Any()) ?  submission.Revisions.FirstOrDefault().Id : 0;
            
            List<CpDomain.dtoCallSection<CpDomain.dtoSubmissionValueField>> sections = GetSubmissionFields(submission.Call, idSubmitterType, submission.Id, revId);

            IList<CpDomain.FieldDefinition> fielddefinitions = Manager.GetAll<CpDomain.FieldDefinition>(fd =>
                       fd.Call != null && fd.Call.Id == submission.Call.Id
                       && fd.Type == CpDomain.FieldType.TableReport
                       && fd.Deleted == BaseStatusDeleted.None);

            //sf.Fields.FirstOrDefault().Value
            //sf.Fields.FirstOrDefault().Field.Type == CpDomain.FieldType.TableReport
            
            foreach(var sect in sections)
            {
                foreach(var field in sect.Fields)
                {
                    if(field.Field != null && field.Field.Type == CpDomain.FieldType.TableReport)
                    {
                        //bool isFirst = true;
                        //string Table = field.Value.Text;
                        
                        EcoD.EconomicTable table = new EcoD.EconomicTable();
                        table.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                        table.Items = new List<EcoD.EconomicItem>();
                        
                        CpDomain.FieldDefinition fDef = fielddefinitions.FirstOrDefault(f => f.Id == field.IdField);
                        //String[] Cold = fDef.TableCols.Split('|');
                        table.FieldDefinition = fDef;
                        
                        table.HeaderValue = fDef.TableCols;
                        table.AdmitMax = field.Field.TableFieldSetting.MaxTotal;

                        int AddcolNum = table.HeaderValues.Count();


                        string contentXml = String.Format("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>{0}{1}",
                            System.Environment.NewLine, field.Value.Text);

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(contentXml);

                        foreach (XmlNode trXml in doc.GetElementsByTagName("tr"))
                        {
                            EcoD.EconomicItem itm = new EcoD.EconomicItem();
                            itm.CreateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);


                            double unitycost = 0;
                            double quantity = 0;
                            double total = 0;

                            int curCol = 0;
                            foreach (XmlNode tdXml in trXml.ChildNodes)
                            {
                                curCol++;


                                //if (tdXml.Attributes != null && tdXml.Attributes.Count > 0)
                                //{
                                string cssclass = "";

                                try
                                {
                                    cssclass = tdXml.Attributes["class"].Value;
                                }
                                catch (Exception ex)
                                {

                                }
                                if (!String.IsNullOrEmpty(cssclass) && cssclass == "quantity")
                                {
                                    Double.TryParse(tdXml.InnerText.ToString(), out quantity);
                                }
                                else if (!String.IsNullOrEmpty(cssclass) && cssclass == "unitycost")
                                {
                                    Double.TryParse(tdXml.InnerText.ToString(), out unitycost);
                                }
                                else if (!String.IsNullOrEmpty(cssclass) && cssclass == "total")
                                {
                                    Double.TryParse(tdXml.InnerText.ToString(), out total);
                                }
                                else
                                {
                                    if (String.IsNullOrEmpty(itm.InfoValue))
                                    {
                                        itm.InfoValue = String.IsNullOrEmpty(tdXml.InnerText.ToString())? "&nbsp;" : tdXml.InnerText.ToString();
                                    }
                                    else
                                    {
                                        itm.InfoValue = string.Format("{0}|{1}",
                                            itm.InfoValue,
                                            tdXml.InnerText.ToString());
                                    }

                                }


                                //}
                            }
                            //itm.InfoValue = 
                            itm.IsAdmit = false;
                            itm.RequestQuantity = quantity;
                            itm.RequestUnitPrice = unitycost;
                            //itm.RequestTotal
                            itm.Table = table;

                            table.Items.Add(itm);

                            //table.RequestTotal
                        }
                        table.EcoEvaluation = eval;
                        
                        tables.Add(table);
                    }
                }
            }
            return tables;
        }

        #endregion

        #region GET
        public Eco.dto.dtoEcoSummaryContainer EcoSummaryGet(long commId)
        {
            Eco.dto.dtoEcoSummaryContainer container = new Eco.dto.dtoEcoSummaryContainer();

            Adv.Domain.AdvCommission commission = Manager.Get<Adv.Domain.AdvCommission>(commId);
            if (commission == null)
                return container;

            if(!((commission.President != null && commission.President.Id == UC.CurrentUserID) ||
                (commission.Members != null && commission.Members.Any(m => m.Member != null && m.Member.Id == UC.CurrentUserID)
                )))
            {
                return container;
            }

            container.hasPermission = true;

            

            container.StepId = (commission.Step != null) ? commission.Step.Id : 0;

            if(commission.Call != null)
            {
                container.CallId = commission.Call.Id;
            } else
            {
                container.CallId = 0;
            }

            

            container.Summaries = Manager.GetAll<EcoD.EconomicEvaluation>(ev =>
                ev.Commission != null && ev.Commission.Id == commId && ev.Deleted == BaseStatusDeleted.None
                )
                .OrderBy(ev => ev.Rank)
                .Select(ev => new Eco.dto.dtoEcoSummary(ev))
                .ToList();


            container.CanCloseCommission =
                commission.Status == Adv.CommissionStatus.Started &&
                (container.Summaries.All(s => s.status == Eco.EvalStatus.confirmed) &&
                (commission.President != null && commission.President.Id == UC.CurrentUserID));


            return container;

        }

        public Eco.dto.dtoEcoEvaluation EcoEvaluationGet(long commId, long evaluationId)
        {
            Adv.Domain.AdvCommission Comm = Manager.Get<Adv.Domain.AdvCommission>(commId);
            if (Comm == null)
                return new Eco.dto.dtoEcoEvaluation();

            Eco.dto.dtoEcoEvaluation eval = new Eco.dto.dtoEcoEvaluation(Manager.Get<EcoD.EconomicEvaluation>(evaluationId));

            //Eco.dto.dtoEcoEvaluation evals = Manager.GetAll<EcoD.EconomicEvaluation>(ev =>
            //    ev.Commission != null && ev.Commission.Id == commId && ev.Deleted == BaseStatusDeleted.None
            //    && ev.
            //    ).Select(ev => new Eco.dto.dtoEcoEvaluation(ev)).FirstOrDefault();
            
            if(Comm.Members.Any(m => m.Member.Id == UC.CurrentUserID))
            {
                eval.CanView = true;
                eval.CanModify = eval.status == Eco.EvalStatus.draft || eval.status == Eco.EvalStatus.take;
                eval.CanReopen = eval.status == Eco.EvalStatus.completed;
                eval.CanClose = false;
            }

            if (Comm.President.Id == UC.CurrentUserID || Comm.Members.Any(m => m.Member.Id == UC.CurrentUserID && m.IsPresident))
            {
                eval.CanView = true;
                eval.CanModify = eval.status == Eco.EvalStatus.draft || eval.status == Eco.EvalStatus.take;
                eval.CanReopen = eval.status == Eco.EvalStatus.completed;
                eval.CanClose = eval.status == Eco.EvalStatus.completed;
            }

            return eval;
        }
        #endregion

        #region other Crud
        public bool EcoEvalsUpdate(IList<Eco.dto.dtoEcoEvlItem> dtoItems)
        {
            IList<Int64> evIds = dtoItems.Select(i => i.itmId).ToList();

            IList<Eco.Domain.EconomicItem> Items = Manager.GetAll<Eco.Domain.EconomicItem>(i => evIds.Contains(i.Id));

            foreach(Eco.Domain.EconomicItem itm in Items)
            {
                Eco.dto.dtoEcoEvlItem dto = dtoItems.FirstOrDefault(i => i.itmId == itm.Id);

                if(dto != null)
                {
                    itm.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                    //itm.Table.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                    //itm.Table.EcoEvaluation.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
                    itm.IsAdmit = dto.isAdmit;
                    itm.AdmitQuantity = dto.AdmitQuantity;

                    if(itm.IsAdmit)
                        itm.AdmitTotal = dto.AdmitTotal;
                    else
                        itm.AdmitTotal = 0;

                    itm.Comment = dto.Comment;
                    
                }
            }

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdateList<Eco.Domain.EconomicItem>(Items);
                Manager.Commit();
            } catch(Exception ex)
            {
                Manager.RollBack();
                return false;
            }
            
            return true;
        }

        public bool EcoEvalsAssignCurrent(long evalId)
        {
            Eco.Domain.EconomicEvaluation eval = Manager.Get<Eco.Domain.EconomicEvaluation>(evalId);

            if (eval == null)
                return false;

            

            lm.Comol.Modules.CallForPapers.Advanced.Domain.AdvMember Member = null;
            try
            {
                Member = eval.Commission.Members.FirstOrDefault(mem => mem.Member.Id == UC.CurrentUserID);
            } catch
            {

            }

            if (Member == null)
                return false;

            eval.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
            eval.Status = Eco.EvalStatus.take;
            eval.CurrentMember = Member;

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Eco.Domain.EconomicEvaluation>(eval);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }

        public bool EcoEvalsRemoveAssign(long evalId)
        {
            Eco.Domain.EconomicEvaluation eval = Manager.Get<Eco.Domain.EconomicEvaluation>(evalId);

            if (eval == null)
                return false;
            
            eval.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
            eval.CurrentMember = null;

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Eco.Domain.EconomicEvaluation>(eval);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }

        public bool EcoEvalsChangeStatus(long evalId, Eco.EvalStatus status)
        {
            Eco.Domain.EconomicEvaluation eval = Manager.Get<Eco.Domain.EconomicEvaluation>(evalId);

            if (eval == null)
                return false;

            eval.UpdateMetaInfo(GetCurrentPerson(), UC.IpAddress, UC.ProxyIpAddress);
            eval.Status = status;

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            try
            {
                Manager.SaveOrUpdate<Eco.Domain.EconomicEvaluation>(eval);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }
        #endregion


    }
}
