using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Presentation;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    public class RootObject
    {

        #region "NEW"

            #region "List"
                
                public static string ViewCalls(CallForPaperType type, CallStandardAction action, int idCommunity, CallStatusForSubmitters view)
                {
                    String url = (type == CallForPaperType.CallForBids) ? "Modules/CallForPapers/Calls.aspx?" : "Modules/CallForPapers/Requests.aspx?";
                    return url + "action=" + action.ToString() + "&idCommunity=" + idCommunity.ToString() + "&View=" + view.ToString();
                }
                public static string ViewCalls(long idCall, CallForPaperType type, CallStandardAction action, int idCommunity, CallStatusForSubmitters view)
                {
                    return ViewCalls(type, action, idCommunity, view) + "#" + idCall.ToString();
                }
                public static string PublicCollectorCalls(CallForPaperType type, long idCall, Int32 idCommunity)
                {
                    String baseurl = "Modules/CallForPapers/Public{0}sCollector.aspx?";
                    switch (type)
                    {
                        case CallForPaperType.CallForBids:
                            baseurl = String.Format(baseurl, "Call");
                            break;
                        case CallForPaperType.RequestForMembership:
                            baseurl = String.Format(baseurl, "Request");
                            break;
                    }
                    return baseurl + "type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "#call_" + idCall.ToString();
                }
            #endregion

            #region "Submit"
                private static string BaseSubmitToCall(CallForPaperType type, long idCall, Boolean isPublic, Boolean fromPublicList, CallStatusForSubmitters view, Int32 containerIdCommunity)
                {
                    String baseurl = "Modules/CallForPapers/" + ((isPublic) ? "Public" : "");
                    switch (type)
                    {
                        case CallForPaperType.CallForBids:
                            baseurl += "SubmitCall.aspx?";
                            break;
                        case CallForPaperType.RequestForMembership:
                            baseurl += "SubmitRequest.aspx?";
                            break;
                    }
                    return baseurl + "type=" + type.ToString() + "&View=" + view.ToString() + "&idCall=" + idCall.ToString() + ((fromPublicList == true) ? "&FromPublicList=true" : "") + ((containerIdCommunity>-1) ? "&idOtherCommunity=" + containerIdCommunity.ToString() :"");
                }
                public static string StartNewSubmission(CallForPaperType type, long idCall, Boolean isPublic, Boolean fromPublicList, CallStatusForSubmitters view, Int32 containerIdCommunity)
                {
                    return BaseSubmitToCall(type, idCall, isPublic, fromPublicList, view, containerIdCommunity);
                }
                public static string ContinueSubmission(CallForPaperType type, long idCall, Boolean isPublic, long idSubmission, Boolean fromPublicList, CallStatusForSubmitters view, Int32 containerIdCommunity)
                {
                    return BaseSubmitToCall(type, idCall, isPublic, fromPublicList, view, containerIdCommunity) + "&idSubmission=" + idSubmission.ToString();
                }
                public static string SubmitToCallBySubmitterType(CallForPaperType type, long idCall, Boolean isPublic, long idSubmission, long idSubmitterType, Boolean fromPublicList, CallStatusForSubmitters view, Int32 containerIdCommunity)
                {
                    return ContinueSubmission(type, idCall, isPublic, idSubmission, fromPublicList, view, containerIdCommunity) + "&idType=" + idSubmitterType.ToString();
                }
                public static string FinalMessage(CallForPaperType type, long idCall, long idSubmission, long idRevision, Boolean fromPublicList, CallStatusForSubmitters view, Int32 containerIdCommunity)
                {
                    return FinalMessage(type, idCall, idSubmission, idRevision, Guid.Empty, false, fromPublicList, view, containerIdCommunity);
                }
                public static string FinalMessage(CallForPaperType type, long idCall, long idSubmission, long idRevision, System.Guid uniqueId, Boolean isPublic, Boolean fromPublicList, CallStatusForSubmitters view, Int32 containerIdCommunity)
                {
                    String baseurl = "Modules/CallForPapers/" + ((isPublic) ? "Public" : "Internal") + "FinalMessage.aspx?";

                    return baseurl + "type=" + type.ToString() + "&View=" + view.ToString() + "&idCall=" + idCall.ToString() + "&idSubmission=" + idSubmission.ToString() + "&idRevision=" + idRevision.ToString() + ((uniqueId == System.Guid.Empty) ? "" : "&uniqueId=" + uniqueId.ToString()) + ((fromPublicList == true) ? "&FromPublicList=true" : "") + ((containerIdCommunity > -1) ? "&idOtherCommunity=" + containerIdCommunity.ToString() : "");
                }
            #endregion
                
            #region "Revisions"
                public static string ViewRevisions(CallForPaperType type, CallStandardAction action, int idCommunity, CallStatusForSubmitters view)
                {
                    String url = "Modules/CallForPapers/Revisions.aspx?action={0}&idCommunity={1}&View={2}&Type={3}";
                    return String.Format(url, action.ToString(), idCommunity.ToString(), view.ToString(), type.ToString());
                }
                public static string ViewRevisions(long idRevision, CallForPaperType type, CallStandardAction action, int idCommunity, CallStatusForSubmitters view)
                {
                    return ViewRevisions(type, action, idCommunity, view) + "#" + idRevision.ToString();
                }
                public static string UserReviewCall(CallForPaperType type, long idCall, long idSubmission, long idRevision, CallStatusForSubmitters view, Int32 containerIdCommunity)
                {
                    String baseurl = "Modules/CallForPapers/ReviewSubmission.aspx?type={0}&View={1}&idCall={2}&idSubmission={3}&idRevision={4}";

                    return String.Format(baseurl, type.ToString(), view.ToString(), idCall, idSubmission, idRevision) + ((containerIdCommunity > -1) ? "&idOtherCommunity=" + containerIdCommunity.ToString() : "");
                }
                public static string ManageReviewSubmission(CallForPaperType type, long idCall, long idSubmission, long idRevision, CallStandardAction action, CallStatusForSubmitters view)
                {
                    String baseurl = "Modules/CallForPapers/ManageReview.aspx?type={0}&View={1}&idCall={2}&idSubmission={3}&idRevision={4}&Action={5}";

                    return String.Format(baseurl, type.ToString(), view.ToString(), idCall.ToString(), idSubmission.ToString(), idRevision.ToString(), action.ToString());
                }
                public static string ManageReviewSubmission(CallForPaperType type, long idCall, long idSubmission, long idRevision, CallStandardAction action, CallStatusForSubmitters view, SubmissionFilterStatus filter, SubmissionsOrder order, Boolean ascending, int pageIndex, int pageSize)
                {
                    return ManageReviewSubmission(type, idCall, idSubmission, idRevision, action, view) + "&filter=" + filter.ToString() + "&order=" + order.ToString() + "&pageIndex=" + pageIndex.ToString() + "&manage=True"; // + "#subrev" + idRevision.ToString();
                }
            #endregion

            #region "View Submission"
                public static string ViewSubmissions(CallForPaperType type, long idCall, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/SubmissionsList.aspx?type=" + type.ToString() + "&View=" + view.ToString() + "&idCall=" + idCall.ToString();
                }
                public static string ViewSubmissions(CallForPaperType type, long idCall, long idSubmission, long idRevision, CallStatusForSubmitters view, SubmissionFilterStatus filter, SubmissionsOrder order, Boolean ascending, int pageIndex, int pageSize)
                {
                    return ViewSubmissions(type, idCall, view) + "&idSubmission=" + idSubmission.ToString() + "&filter=" + filter.ToString() + "&order=" + order.ToString() + "&ascending=" + ascending.ToString() + "&pageIndex=" + pageIndex.ToString() + "&pageSize=" + pageSize.ToString() + "#sub" + idSubmission.ToString() + "rev" + idRevision.ToString();
                }
                public static string ViewSubmission(CallForPaperType type, long idCall, long idSubmission, Boolean fromPublicList, CallStatusForSubmitters view, Int32 containerIdCommunity, long CommissionId)
                {
                    return ViewSubmission(type, idCall, idSubmission, 0, Guid.Empty, false, fromPublicList, view, containerIdCommunity, CommissionId);
                }
                public static string ViewSubmission(CallForPaperType type, long idCall, long idSubmission, System.Guid uniqueId, Boolean isPublic, Boolean fromPublicList, CallStatusForSubmitters view, Int32 containerIdCommunity, long CommissionId)
                {
                    return ViewSubmission(type, idCall, idSubmission, 0, uniqueId, isPublic, fromPublicList, view, containerIdCommunity, CommissionId);
                }
                public static string ViewSubmission(
                    CallForPaperType type, 
                    long idCall, 
                    long idSubmission, 
                    long idRevision, 
                    System.Guid uniqueId, 
                    Boolean isPublic, 
                    Boolean fromPublicList, 
                    CallStatusForSubmitters view, 
                    Int32 containerIdCommunity,
                    long CommissionId)
                {
                    String baseurl = "Modules/CallForPapers/" + ((isPublic) ? "Public" : "") + "Submission.aspx?";
                    baseurl += "type=" + type.ToString() + "&idCall=" + idCall.ToString() + "&idSubmission=" 
                        + idSubmission.ToString() + ((idRevision == 0) ? "" : "&idRevision=" + idRevision.ToString()) 
                        + ((uniqueId == System.Guid.Empty) ? "" : "&uniqueId=" + uniqueId.ToString()) + "&View=" + view.ToString() 
                        + ((fromPublicList == true) ? "&FromPublicList=true" : "") + ((containerIdCommunity > -1) ? "&idOtherCommunity=" 
                        + containerIdCommunity.ToString() : "");
                    baseurl += "&cmmId=" + CommissionId;
            
                    return baseurl;
                    
                }
                public static string ViewSubmissionAsManager(
                    CallForPaperType type, 
                    long idCall, 
                    long idSubmission, 
                    long idRevision, 
                    System.Guid uniqueId, 
                    Boolean isPublic, 
                    CallStatusForSubmitters view, 
                    SubmissionFilterStatus filter, 
                    SubmissionsOrder order, 
                    Boolean ascending, 
                    int pageIndex, 
                    int pageSize,
                     long CommissionId)
                {
                    return ViewSubmission(type, idCall, idSubmission, idRevision, uniqueId, isPublic, false, view, -1, CommissionId) 
                        + "&Manage=true&filter=" + filter.ToString() + "&order=" + order.ToString() 
                        + "&ascending=" + ascending.ToString() + "&pageIndex=" + pageIndex.ToString() 
                        + "&pageSize=" + pageSize.ToString();
                }
            #endregion
                #region "Editing"
                public static string AddCall(CallForPaperType type, int idCommunity, CallStatusForSubmitters view)
                {
                    return EditCallSettings(type, (long)0, idCommunity, CallStandardAction.Add, view);
                }
                //public static string EditCall(CallForPaperType type, int idCommunity)
                //{
                //    return EditCallSettings(type,0 idCommunity, CallStandardAction.Edit);
                //}
                public static string EditCallSettings(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return EditCallSettings(type, idCall, idCommunity, CallStandardAction.Edit, view);
                }
                public static string EditCallSettings(CallForPaperType type, long idCall, int idCommunity, CallStandardAction action, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditSettings.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&action=" + action.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                }
                public static string EditCallAttachments(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditAttachments.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&action=" + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                }
                public static string EditCallAvailability(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditCallAvailability.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                }
                public static string EditCallFileToSubmit(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditFileToSubmit.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                }
                public static string EditCallRequestMessages(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditRequestMessages.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                }
                public static string EditCallSubmissionEditor(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditCall.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                }
                public static string CallSubmissionEditorSectionAdded(long idSection, CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditCall.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString() + "#section_" + idSection.ToString();
                }
                public static string CallSubmissionEditorSectionRemoved(long idNewSection, CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditCall.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString() + "#section_" + idNewSection.ToString();
                }
                public static string CallSubmissionEditorFieldAdded(long idField, CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditCall.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString() + "#field_" + idField.ToString();
                }
                public static string CallSubmissionEditorFieldRemoved(long idNewField, CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditCall.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString() + "#field_" + idNewField.ToString();
                }
                public static string CallSubmissionEditorOptionAdded(long idOption, CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditCall.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString() + "#option_" + idOption.ToString();
                }
                public static string CallSubmissionEditorOptionRemoved(long idNewOption, CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditCall.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString() + "#option_" + idNewOption.ToString();
                }
                public static string EditCallSubmittersType(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditSubmittersType.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                }
                public static string EditCallFieldProfileAssociation(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditFieldAssociation.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                }
                public static string EditCallTemplateMail(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditTemplateMail.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                }
                public static string EditNotificationTemplateMail(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/EditNotificationTemplate.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                }
                public static string EditByStep(CallForPaperType type, long idCall, int idCommunity, WizardCallStep step, CallStatusForSubmitters view)
                {
                    switch (step)
                    {
                        case WizardCallStep.GeneralSettings:
                            return EditCallSettings(type, idCall, idCommunity, view);
                        case WizardCallStep.Attachments:
                            return EditCallAttachments(type, idCall, idCommunity, view);
                        case WizardCallStep.CallAvailability:
                            return EditCallAvailability(type, idCall, idCommunity, view);
                        case WizardCallStep.FileToSubmit:
                            return EditCallFileToSubmit(type, idCall, idCommunity, view);
                        case WizardCallStep.RequestMessages:
                            return EditCallRequestMessages(type, idCall, idCommunity, view);
                        case WizardCallStep.SubmissionEditor:
                            return EditCallSubmissionEditor(type, idCall, idCommunity, view);
                        case WizardCallStep.SubmittersType:
                            return EditCallSubmittersType(type, idCall, idCommunity, view);
                        case WizardCallStep.FieldsAssociation:
                            return EditCallFieldProfileAssociation(type, idCall, idCommunity, view);
                        case WizardCallStep.SubmitterTemplateMail:
                            return EditCallTemplateMail(type, idCall, idCommunity, view);
                        case WizardCallStep.NotificationTemplateMail:
                            return EditNotificationTemplateMail(type, idCall, idCommunity, view);
                        default:
                            return "";
                    }
                }
                public static string AddFromCall(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Edit/AddFromCall.aspx?type=" + type.ToString() + "&idCommunity=" + idCommunity.ToString() + "&FromCall=" + idCall.ToString() + "&View=" + view.ToString();
                }
                public static string PreviewCall(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    String url = "Modules/CallForPapers/";
                    switch (type)
                    {
                        case CallForPaperType.CallForBids:
                            url += "PreviewCall.aspx?";
                            break;
                        case CallForPaperType.RequestForMembership:
                            url += "PreviewRequest.aspx?";
                            break;

                    }
                    url += "idCall=" + idCall.ToString() + "&idCommunity=" + idCommunity.ToString() + "&View=" + view.ToString();
                    return url;
                }
            #endregion

            #region "Commission"
                public static string ManageEvaluation(CallForPaperType type, long idCall, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Evaluate/Evaluations.aspx?type=" + type.ToString() + "&View=" + view.ToString() + "&idCall=" + idCall.ToString();
                }
              
            #endregion
        #endregion

        #region "Evaluations"
            #region "Editing"
                public static string EditCommiteeByStep(long idCall, int idCommunity, Evaluation.WizardEvaluationStep step, CallStatusForSubmitters view)
                {
                    switch (step)
                    {
                        case Evaluation.WizardEvaluationStep.GeneralSettings:
                            return EditCommitteeSettings(idCall, idCommunity, view);
                        case Evaluation.WizardEvaluationStep.FullManageEvaluators:
                            return EditFullManageEvaluators(idCall, idCommunity, view);
                        case Evaluation.WizardEvaluationStep.ManageEvaluators:
                            return ViewEvaluators(idCall, idCommunity, view);
                        case Evaluation.WizardEvaluationStep.AssignSubmission:
                            return EditSingleCommitteeAssignments(idCall, idCommunity, view);
                        case Evaluation.WizardEvaluationStep.MultipleAssignSubmission:
                            return EditMultipleCommitteeAssignments(idCall, idCommunity, view);
                        case Evaluation.WizardEvaluationStep.AssignSubmissionWithNoEvaluation:
                            return EditAssignSubmissionWithNoEvaluation(idCall, idCommunity, view);
                        case Evaluation.WizardEvaluationStep.ManageEvaluations:
                            return EditManageEvaluations(idCall, idCommunity, view);
                        default:
                            return "";
                    }
                }
                #region "1 - Settings"
                    public static string EditCommitteeSettings(long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return EditCommitteeSettings(idCall, idCommunity, CallStandardAction.Edit, view);
                    }
                    public static string EditCommitteeSettings(long idCall, int idCommunity, CallStandardAction action, CallStatusForSubmitters view)
                    {
                        return "Modules/CallForPapers/Evaluate/EditEvaluationCommittees.aspx?idCommunity=" + idCommunity.ToString() + "&action=" + action.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                    }
                    public static string CommitteeAddedToCall(long idCommittee, long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return EditCommitteeSettings(idCall,idCommunity,view) + "#committee_" + idCommittee.ToString();
                    }
                    public static string CommitteeRemovedFromCall(long idNewCommittee, long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return EditCommitteeSettings(idCall,idCommunity,view) + "#committee_" + idNewCommittee.ToString();
                    }
                    public static string CriterionAddedToCommittee(long idCriterion, long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return EditCommitteeSettings(idCall,idCommunity,view) + "#criterion_" + idCriterion.ToString();
                    }
                    public static string CriterionRemovedFromCommittee(long idNewCriterion, long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return EditCommitteeSettings(idCall,idCommunity,view) + "#criterion_" + idNewCriterion.ToString();
                    }
                    public static string OptionAddedToCriterion(long idOption, long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return EditCommitteeSettings(idCall,idCommunity,view) + "#option_" + idOption.ToString();
                    }
                    public static string OptionRemovedFromCriterion(long idNewOption, long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return EditCommitteeSettings(idCall,idCommunity,view) + "#option_" + idNewOption.ToString();
                    }
                #endregion
                #region "2 - Evaluators"
                    public static string EditFullManageEvaluators(long idCall, int idCommunity,CallStatusForSubmitters view)
                    {
                        return "Modules/CallForPapers/Evaluate/ManageEvaluators.aspx?idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                    }
                    public static string EvaluatorAddedToCall(long idEvaluator, long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return EditFullManageEvaluators(idCall, idCommunity, view) + "#evaluator_" + idEvaluator.ToString();
                    }
                    public static string EvaluatorRemovedFromCall(long idEvaluator, long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return EditFullManageEvaluators(idCall, idCommunity, view) + "#evaluator_" + idEvaluator.ToString();
                    }
                    public static string ViewEvaluators(long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return "Modules/CallForPapers/Evaluate/ViewEvaluators.aspx?idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                    }
                    public static string ViewEvaluators(long idMembership,long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return ViewEvaluators(idCall, idCommunity, view) + "#membership_" + idMembership.ToString();
                    }
                    public static string DeleteInEvaluationMembership(long idMembership, long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return "Modules/CallForPapers/Evaluate/DeleteInEvaluationMembership.aspx?idMembership=" + idMembership.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                    }
                    public static string ReplaceInEvaluationMembership(long idMembership, long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return "Modules/CallForPapers/Evaluate/ReplaceInEvaluationMembership.aspx?idMembership=" + idMembership.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                    }
                #endregion
                #region "3 - Assing UserSubmissions"
                    public static string EditCommitteeAssignments(Boolean multiple) {
                        return (multiple) ? "Modules/CallForPapers/Evaluate/MultipleAssignEvaluators.aspx?" : "Modules/CallForPapers/Evaluate/SingleAssignEvaluators.aspx?";
                    }
                    public static string EditSingleCommitteeAssignments(long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return EditCommitteeAssignments(false) + "idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                    }
                    public static string EditMultipleCommitteeAssignments(long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return EditCommitteeAssignments(true) + "idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                    }
                    public static string EditAssignSubmissionWithNoEvaluation(long idCall, int idCommunity, CallStatusForSubmitters view)
                    {
                        return "Modules/CallForPapers/Evaluate/AssignSubmissionWithNoEvaluation.aspx?" + "idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                    }
                #endregion
                public static string EditManageEvaluations(long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Evaluate/ManageEvaluations.aspx?idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&View=" + view.ToString();
                }
            #endregion

            #region "Evaluating"
                public static string ViewSubmissionsToEvaluate(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view, SubmissionsOrder order, Boolean ascending)
                {
                    return ViewSubmissionsToEvaluate(type, idCall, idCommunity, view, order, ascending, -1, Evaluation.EvaluationFilterStatus.All, "");
                }
        
        public static string ViewSubmissionsToEvaluateAdv(
            CallForPaperType type,
            long idCall,
            int idCommunity,
            long idCommission, 
            CallStatusForSubmitters view, 
            SubmissionsOrder order, 
            Boolean ascending, 
            long idType, 
            lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationFilterStatus status, 
            String searchBy)
        {

            return string.Format("Modules/CallForPapers/Evaluate/SubmissionsToEvaluate.aspx?type={0}&View={1}&idCall={2}&AdCId={3}&idCommunity={4}&OrderBy={5}&Ascending={6}{7}&Filter={8}{9}",
                type.ToString(),
                view,
                idCall,
                idCommission,
                idCommunity,
                order,
                ascending,
                ((idType > 0) ? "&idType=" + idType.ToString() : ""),
                status,
                ((String.IsNullOrEmpty(searchBy)) ? "" : "&searchBy=" + searchBy));

            //return "Modules/CallForPapers/Evaluate/SubmissionsToEvaluate.aspx?type=" 
            //    + type.ToString() 
            //    + "&View=" + view.ToString() 
            //    + "&idCall=" + idCall.ToString() 
            //    + "&AdCId=" + idCommission.ToString()
            //    + "&idCommunity=" + idCommunity.ToString() 
            //    + "&OrderBy=" + order.ToString() 
            //    + "&Ascending=" + ascending.ToString()
            //    + ((idType > 0) ? "&idType=" + idType.ToString() : "") 
            //    + "&Filter=" + status.ToString() 
            //    + ((String.IsNullOrEmpty(searchBy)) ? "" : "&searchBy=" + searchBy);

        }

        public static string ViewSubmissionsToEvaluate(CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view, SubmissionsOrder order, Boolean ascending, long idType, lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationFilterStatus status, String searchBy)
        {
            return "Modules/CallForPapers/Evaluate/SubmissionsToEvaluate.aspx?type=" + type.ToString() + "&View=" + view.ToString() + "&idCall=" + idCall.ToString() + "&idCommunity=" + idCommunity.ToString() + "&OrderBy=" + order.ToString() + "&Ascending=" + ascending.ToString()
                + ((idType > 0) ? "&idType=" + idType.ToString() : "") + "&Filter=" + status.ToString() + ((String.IsNullOrEmpty(searchBy)) ? "" : "&searchBy=" + searchBy);
        }
        public static string ViewSubmissionsToEvaluate(long idCommittee, CallForPaperType type, long idCall, int idCommunity, CallStatusForSubmitters view, SubmissionsOrder order, Boolean ascending, long idType, lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationFilterStatus status, String searchBy)
                {
                    return ViewSubmissionsToEvaluate(type, idCall, idCommunity, view, order, ascending, idType,status,searchBy) + "&idCommittee=" + idCommittee.ToString() + "&AdCId=" + idCommittee.ToString();
                }
                public static string EvaluateSubmission(long idCall, int idCommunity, long idEvaluation)
                {
                    return "Modules/CallForPapers/Evaluate/Evaluate.aspx?idCall=" + idCall.ToString() + "&idCommunity=" + idCommunity.ToString() + "&idEvaluation=" + idEvaluation.ToString();
                }

                public static string EvaluationsSummary(long idCall,int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Statistics/EvaluationsSummary.aspx?idCommunity=" + idCommunity.ToString() +"&View=" + view.ToString() + "&idCall=" + idCall.ToString();
                }
                public static string EvaluationsSummary(long idCall,int idCommunity,  CallStatusForSubmitters view, long idSubmission, long idSubmitterType, Evaluation.EvaluationFilterStatus filter, SubmissionsOrder order, Boolean ascending, int pageIndex, int pageSize)
                {
                    return EvaluationsSummary(idCall, idCommunity, view, idSubmission, idSubmitterType,filter, order, ascending, pageIndex, pageSize, "");
                }
                public static string EvaluationsSummary(long idCall, int idCommunity, CallStatusForSubmitters view, long idSubmission, long idSubmitterType, Evaluation.EvaluationFilterStatus filter, SubmissionsOrder order, Boolean ascending, int pageIndex, int pageSize, String searchBy)
                {
                    return EvaluationsSummary(idCall, idCommunity, view) + (String.IsNullOrEmpty(searchBy) ? "" : "&SearchForName=" + searchBy) + "&filter=" + filter.ToString() + "&idType=" + idSubmitterType.ToString() + "&OrderBy=" + order.ToString() + "&ascending=" + ascending.ToString() + "&pageIndex=" + pageIndex.ToString() + "&pageSize=" + pageSize.ToString() + "#sub" + idSubmission.ToString();
                }

                public static string CommitteesSummary(long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Statistics/CommitteesSummary.aspx?idCommunity=" + idCommunity.ToString() + "&View=" + view.ToString() + "&idCall=" + idCall.ToString();
                }
                public static string CommitteesSummary(long idCall, int idCommunity, CallStatusForSubmitters view, long idSubmitterType, Evaluation.EvaluationFilterStatus filter, SubmissionsOrder order, Boolean ascending, int pageIndex, int pageSize)
                {
                    return CommitteesSummary(idCall, idCommunity, view, idSubmitterType, filter, order, ascending, pageIndex, pageSize, "");
                }
                public static string CommitteesSummary(long idCall, int idCommunity, CallStatusForSubmitters view, long idSubmitterType, Evaluation.EvaluationFilterStatus filter, SubmissionsOrder order, Boolean ascending, int pageIndex, int pageSize, String searchBy)
                {
                    return CommitteesSummary(idCall, idCommunity, view) + (String.IsNullOrEmpty(searchBy) ? "" : "&SearchForName=" + searchBy) + "&idType=" + idSubmitterType.ToString() + "&filter=" + filter.ToString() + "&OrderBy=" + order.ToString() + "&ascending=" + ascending.ToString() + "&pageIndex=" + pageIndex.ToString() + "&pageSize=" + pageSize.ToString();
                }
                public static string CommitteeSummary(long idCommittee, long idCall, int idCommunity, CallStatusForSubmitters view)
                {
                    return "Modules/CallForPapers/Statistics/CommitteeSummary.aspx?idCommunity=" + idCommunity.ToString() + "&View=" + view.ToString() + "&idCall=" + idCall.ToString() + "&idCommittee=" + idCommittee.ToString(); 
                }
                public static string CommitteeSummary(long idCommittee, long idCall, int idCommunity, CallStatusForSubmitters view,long idSubmitterType, Evaluation.EvaluationFilterStatus filter, SubmissionsOrder order, Boolean ascending, int pageIndex, int pageSize, String searchBy)
                {
                    return CommitteeSummary(idCommittee, idCall, idCommunity, view) + (String.IsNullOrEmpty(searchBy) ? "" : "&SearchForName=" + searchBy) + "&idType=" + idSubmitterType.ToString() + "&filter=" + filter.ToString() + "&OrderBy=" + order.ToString() + "&ascending=" + ascending.ToString() + "&pageIndex=" + pageIndex.ToString() + "&pageSize=" + pageSize.ToString();
                }

                private static string ViewEvaluation(long idCall, int idCommunity, long advCommId)
                {
                    return "Modules/CallForPapers/Statistics/ViewEvaluation.aspx?idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&acId=" + advCommId.ToString();
                }
                public static string ViewSubmissionEvaluations(long idSubmission, long idCall, int idCommunity, long advCommId)
                {
                    return ViewEvaluation(idCall,idCommunity, advCommId) + "&idSubmission=" + idSubmission.ToString();
                }
                public static string ViewUserEvaluations(long idEvaluator, long idSubmission, long idCall, int idCommunity, long advCommId)
                {
                    return ViewSubmissionEvaluations(idSubmission, idCall,idCommunity, advCommId) + "&idEvaluator=" + idEvaluator.ToString();
                }
                public static string ViewSingleEvaluation(long idEvaluation, long idSubmission, long idCall, int idCommunity, long advCommId)
                {
                    return ViewSubmissionEvaluations(idSubmission, idCall, idCommunity, advCommId) + ((idEvaluation <= 0) ? "" : "&idEvaluation=" + idEvaluation.ToString());
                }
                public static string ViewSubmissionTableEvaluations(long idSubmission, long idCall, int idCommunity)
                {
                    return  "Modules/CallForPapers/Statistics/EvaluationSummary.aspx?idCommunity=" + idCommunity.ToString() + "&idCall=" + idCall.ToString() + "&idSubmission=" + idSubmission.ToString();
                }
        #endregion
        #endregion

        #region "Advanced"
        public static string AdvCommissionEdit(long idCall, long idCommission, Advanced.CommiteeEditPage Page)
        {
            String BaseUrl = "Modules/CallForPapersAdv/cpAdvCommissionEdit.aspx?cId={0}&cnId={1}&pg={2}";
            return String.Format(BaseUrl, idCall, idCommission, Page);
        }

        //public static string AdvCommissionEditCriterion(long idCall, long idCommission, Advanced.CommiteeEditPage Page)
        //{
        //    String BaseUrl = "Modules/CallForPapersAdv/cpAdvCommissionEdit.aspx?cId={0}&cnId={1}&pg={2}";
        //    return String.Format(BaseUrl, idCall, idCommission, lm.Comol.Modules.CallForPapers.Advanced.CommiteeEditPage.Criterion);
        //}

        public static string AdvStepsEdit(long idCall)
        {
            string BaseUrl = "Modules/CallForPapersAdv/cpAdvEvaluationSteps.aspx?cId={0}";
            return string.Format(BaseUrl, idCall);
        }

        //public static string AdvStepsSummary(long CallId, long StepId)
        //{
        //    string BaseUrl = "Modules/CallForPapersAdv/cpAdvStepSummary.aspx?cmId={0}&stId={1}";
        //    return string.Format(BaseUrl, CallId, StepId);
        //}


        public static string AdvEvalSummary(int idCommunity, long idCall,  long idCommission, CallStatusForSubmitters view, long idSubmission, long idSubmitterType, Evaluation.EvaluationFilterStatus filter, SubmissionsOrder order, Boolean ascending, int pageIndex, int pageSize, String searchBy)
        {
            return AdvEvalSummary(idCall, idCommunity, idCommission, view) + (String.IsNullOrEmpty(searchBy) ? "" : "&SearchForName=" + searchBy) + "&filter=" + filter.ToString() + "&idType=" + idSubmitterType.ToString() + "&OrderBy=" + order.ToString() + "&ascending=" + ascending.ToString() + "&pageIndex=" + pageIndex.ToString() + "&pageSize=" + pageSize.ToString() + "#sub" + idSubmission.ToString();
        }       

        public static string AdvEvalSummary(long idCommunity, long idCall, long idCommission, CallStatusForSubmitters view)
        {
            return string.Format("Modules/CallForPapersAdv/cpAdvEvalSummary.aspx?idCommunity={0}&idCall={1}&acId={2}&View={3}",
                idCommunity,
                idCall, 
                idCommission,
                view.ToString());
            
        }



        public static string AdvViewSubmissionTableEvaluations(long idSubmission, long idCall, int idCommunity, long idCommission)
        {
            return string.Format("Modules/CallForPapersAdv/cpAdvEvalDetail.aspx?idCommunity={0}&idCall={1}&idSubmission={2}&acId={3}",
                idCommunity,
                idCall,
                idSubmission,
                idCommission);
        }

        public static string AdvStepSummary(long StepId, long CommissionId)
        {
            return string.Format("Modules/CallForPapersAdv/cpAdvStepSummary.aspx?stId={0}&cmId={1}", StepId, CommissionId);
        }

        #endregion
        #region "Economic"

        public static string EcoSummaries(long CommissionId)
        {
            return string.Format("Modules/CallForPapersAdv/cpAdvEconomicSubmissions.aspx?cmId={0}", CommissionId);
        }

        public static string EcoEvaluation(long CommissionId, long EvaluationId)
        {
            return string.Format("Modules/CallForPapersAdv/cpAdvEconomicEvaluation.aspx?cmId={0}&evId={1}", CommissionId, EvaluationId);
        }

        #endregion
    }
}