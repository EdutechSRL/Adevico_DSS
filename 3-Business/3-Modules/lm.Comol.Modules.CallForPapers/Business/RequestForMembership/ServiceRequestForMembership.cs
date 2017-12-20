using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Modules.CallForPapers.Domain;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using iTextSharp.text;
using lm.Comol.Core.File;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Modules.CallForPapers.Business 
{
    [Serializable()]
    public class ServiceRequestForMembership : BaseService
    {

        #region initClass
            public ServiceRequestForMembership() { }
            public ServiceRequestForMembership(iApplicationContext appContext)
                : base(appContext)
            {
            }
            public ServiceRequestForMembership(iDataContext dataContext)
                : base(dataContext)
            {
             }

        #endregion

        public int ServiceModuleID()
        {
            return this.Manager.GetModuleID(ModuleRequestForMembership.UniqueCode);
        }

        public RequestForMembership GetRequestForMembership(long idCall)
        {
            return Manager.Get<RequestForMembership>(idCall);
        }

        #region "Manage Submission"
        //    #region "View Submission"
        //        public Boolean isUserSubmission(long submissionID, int PersonId)
        //        {
        //            return Manager.GetAll<RfM_UserSubmission>(s => s.Id == submissionID && s.Owner!= null  && s.Owner.Id == PersonId).Any();
        //        }
        //        public RfM_UserSubmission GetUserSubmission(long submissionID)
        //        {
        //            return Manager.Get<RfM_UserSubmission>(submissionID);
        //        }
        //        public String GetSubmissionOwnerDisplayName(long submissionID)
        //        {
        //            try
        //            {
        //                RfM_LazyUserSubmission sub = Manager.Get<RfM_LazyUserSubmission>(submissionID);
        //                if (sub != null)
        //                    return sub.Owner.SurnameAndName;
        //                else
        //                    return "";
        //            }
        //            catch (Exception ex)
        //            {
        //                return "";
        //            }
        //        }

        //        public RfM_UserSubmission GetActiveSubmission(long callForPaperID, int PersonId)
        //        {
        //            RfM_UserSubmission submission = (from s in Manager.GetAll<RfM_UserSubmission>(s => s.BaseForPaper.Id == callForPaperID && s.Owner.Id == PersonId && s.Deleted == BaseStatusDeleted.None)
        //                                         select s).Skip(0).Take(1).ToList().FirstOrDefault();
        //            return submission;
        //        }
        //        public dtoSubmission GetActiveUserSubmission(long callForPaperID, int PersonId)
        //        {
        //            dtoSubmission dto = (from s in Manager.GetAll<RfM_UserSubmission>(s => s.BaseForPaper.Id == callForPaperID && s.Owner.Id == PersonId && s.Deleted == BaseStatusDeleted.None)
        //                                     select new dtoSubmission()
        //                                 {
        //                                     CreatedOn = s.CreatedOn,
        //                                     Deleted = s.Deleted,
        //                                     Id = s.Id,
        //                                     ModifiedOn = s.ModifiedOn,
        //                                     ModifiedBy = s.ModifiedBy,
        //                                     Status = s.Status,
        //                                     SubmittedOn = s.SubmittedOn,
        //                                     PersonId = s.Owner.Id,
        //                                     CallForPaperId = callForPaperID,
        //                                     ExtensionDate = s.ExtensionDate,
        //                                     SubmittedBy = s.SubmittedBy,
        //                                     Type = new dtoSubmitterType() { Deleted = s.Type.Deleted, Description = s.Type.Description, Name = s.Type.Name, Id = s.Type.Id }
        //                                 }).Skip(0).Take(1).ToList().FirstOrDefault();
        //            if (dto != null)
        //                dto.Owner = Manager.Get<Person>(dto.PersonId);
        //            return dto;
        //        }
        //       //
        //public dtoSubmission GetActiveUserSubmission(long callForPaperID, Guid PersonGuid)
        //        {
        //            dtoSubmission dto = (from s in Manager.GetAll<RfM_UserSubmission>(s => s.BaseForPaper.Id == callForPaperID && s.UserCode == PersonGuid && s.Deleted == BaseStatusDeleted.None)
        //                                 select new dtoSubmission()
        //                                 {
        //                                     CreatedOn = s.CreatedOn,
        //                                     Deleted = s.Deleted,
        //                                     Id = s.Id,
        //                                     ModifiedOn = s.ModifiedOn,
        //                                     ModifiedBy = s.ModifiedBy,
        //                                     Status = s.Status,
        //                                     SubmittedOn = s.SubmittedOn,
        //                                     PersonId = s.Owner.Id,
        //                                     CallForPaperId = callForPaperID,
        //                                     ExtensionDate = s.ExtensionDate,
        //                                     SubmittedBy = s.SubmittedBy,
        //                                     Type = new dtoSubmitterType() { Deleted = s.Type.Deleted, Description = s.Type.Description, Name = s.Type.Name, Id = s.Type.Id }
        //                                 }).Skip(0).Take(1).ToList().FirstOrDefault();
        //            if (dto != null)
        //                dto.Owner = Manager.Get<Person>(dto.PersonId);
        //            return dto;
        //        }
        ////
        //        public dtoSubmission GetDtoSubmission(long submissionID)
        //        {
        //            dtoSubmission dto = (from s in Manager.GetAll<RfM_UserSubmission>(s => s.Id == submissionID)
        //                                     select new dtoSubmission()
        //                                 {
        //                                     CreatedOn = s.CreatedOn,
        //                                     Deleted = s.Deleted,
        //                                     Id = s.Id,
        //                                     ModifiedOn = s.ModifiedOn,
        //                                     ModifiedBy = s.ModifiedBy,
        //                                     Status = s.Status,
        //                                     SubmittedOn = s.SubmittedOn,
        //                                     SubmittedBy = s.SubmittedBy,
        //                                     ExtensionDate = s.ExtensionDate,
        //                                     PersonId = s.Owner.Id,
        //                                     CallForPaperId = s.BaseForPaper.Id,
        //                                     Type = new dtoSubmitterType() { Deleted = s.Type.Deleted, Description = s.Type.Description, Name = s.Type.Name, Id = s.Type.Id },
        //                                     LinkZip = s.LinkZip
        //                                 }).Skip(0).Take(1).ToList().FirstOrDefault();
        //            if (dto != null)
        //                dto.Owner = Manager.Get<Person>(dto.PersonId);
        //            return dto;
        //        }
        //        private IList<RfM_AttachmentFile> GetAttachmentFiles(long callForPaperId, Boolean getAll)
        //        {
        //            RequestForMembership call = Manager.Get<RequestForMembership>(callForPaperId);
        //            IList<RfM_AttachmentFile> attachments = (from a in Manager.GetAll<RfM_AttachmentFile>(a => a.BaseForPaper == call && (getAll || a.Deleted == BaseStatusDeleted.None)) select a).ToList();
        //            if ((from a in attachments where a.File == null || a.Link == null select a).Count() > 0 && getAll == false)
        //            {
        //                Boolean OpenTransaction = !Manager.IsInTransaction();
        //                try
        //                {
        //                    if (OpenTransaction)
        //                        Manager.BeginTransaction();
        //                    foreach (RfM_AttachmentFile attachment in attachments.Where(a => a.File == null || a.Link == null).ToList())
        //                    {
        //                        if (attachment.Deleted != BaseStatusDeleted.None)
        //                            attachment.Deleted = attachment.Deleted | BaseStatusDeleted.Automatic;
        //                        else
        //                            attachment.Deleted = BaseStatusDeleted.Automatic;
        //                        Manager.SaveOrUpdate(attachment);
        //                    }
        //                    Manager.Commit();
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (OpenTransaction)
        //                        Manager.RollBack();
        //                }

        //            }
        //            return (from a in attachments where getAll || (a.File != null || a.Link != null) select a).ToList();
        //        }
        //        public IList<dtoAttachmentFile> GetDtoAttachmentFiles(long callForPaperID)
        //        {
        //            return (from a in GetAttachmentFiles(callForPaperID, false)
        //                    orderby a.File.DisplayName
        //                    select new dtoAttachmentFile()
        //                    {
        //                        File = a.File,
        //                        ModuleLinkId = a.Link.Id,
        //                        Id = a.Id
        //                    }).ToList();
        //        }
        //        public IList<dtoSubmitterType> GetDtoSubmittersType(long callForPaperID, Boolean getAll)
        //        {
        //            return (from s in Manager.GetAll<RfM_SubmitterType>(s => s.BaseForPaper.Id == callForPaperID && (getAll || s.Deleted == BaseStatusDeleted.None))
        //                    orderby s.Name
        //                    select new dtoSubmitterType()
        //                    {
        //                        Name = s.Name,
        //                        Description = s.Description,
        //                        Deleted = s.Deleted,
        //                        Id = s.Id
        //                    }).ToList();
        //        }
        //        public IList<dtoSubmitterType> GetDtoSubmittersType(RequestForMembership call, Boolean getAll)
        //        {
        //            return (from s in Manager.GetAll<RfM_SubmitterType>(s => s.BaseForPaper == call && (getAll || s.Deleted == BaseStatusDeleted.None))
        //                    orderby s.Name
        //                    select new dtoSubmitterType()
        //                    {
        //                        Name = s.Name,
        //                        Description = s.Description,
        //                        Deleted = s.Deleted,
        //                        Id = s.Id
        //                    }).ToList();
        //        }
        //        public RfM_dtoRequestedFile GetDtoRequestedFile(long fileId)
        //        {

        //            return (from rf in Manager.GetAll<RfM_RequestedFile>(rf => rf.Id == fileId)
        //                    select new RfM_dtoRequestedFile()
        //                    {
        //                        Name = rf.Name,
        //                        Id = rf.Id,
        //                        Deleted = rf.Deleted,
        //                        Mandatory = rf.Mandatory,
        //                        SubmittersTypeID = (from a in Manager.GetIQ<RfM_RequestedFileAssignment>() where a.Deleted == BaseStatusDeleted.None && a.RequestedFile == rf select a.SubmitterType.Id).ToList()
        //                    }).ToList().FirstOrDefault();
        //        }

        //        public IList<RfM_dtoSubmissionFile> GetSubmissionFiles(long submissionID)
        //        {
        //            RfM_UserSubmission submission = Manager.Get<RfM_UserSubmission>(submissionID);
        //            return GetSubmissionFiles(submission);
        //        }
        //        public IList<RfM_dtoSubmissionFile> GetSubmissionFiles(RfM_UserSubmission submission)
        //        {
        //            if (submission != null)
        //            {
        //                Boolean allowUpload = (submission.Status == SubmissionStatus.draft || submission.Status == SubmissionStatus.none);
        //                return (from s in Manager.GetAll<RfM_RequestedFileAssignment>(s =>
        //                            s.RequestedFile.BaseForPaper == submission.BaseForPaper && s.Deleted == BaseStatusDeleted.None && s.SubmitterType == submission.Type
        //                        && s.RequestedFile.Deleted == BaseStatusDeleted.None)
        //                        orderby s.RequestedFile.Name
        //                        select new RfM_dtoSubmissionFile()
        //                        {
        //                            FileToSubmit = new RfM_dtoRequestedFile(s.RequestedFile),
        //                            FileSubmitted = (from sf in Manager.GetAll<RfM_SubmittedFile>(sf => sf.Deleted == BaseStatusDeleted.None && sf.SubmittedAs == s.RequestedFile && sf.Submission == submission) select new RfM_dtoSubmittedFile(sf)).Skip(0).Take(1).ToList().FirstOrDefault(),
        //                            //FileSubmitted = (from sf in Manager.GetIQ<SubmittedFile>() where sf.Deleted == BaseStatusDeleted.None && sf.SubmittedAs == s.RequestedFile && sf.Submission == submission select new dtoSubmittedFile(sf)).Skip(0).Take(1).ToList().FirstOrDefault(),
        //                            Deleted = s.Deleted,
        //                            AllowUpload = allowUpload,
        //                            AllowRemove = allowUpload,
        //                            Id = s.Id
        //                        }).ToList();
        //            }
        //            else
        //                return new List<RfM_dtoSubmissionFile>();

        //        }
        //        public IList<RfM_dtoSubmissionFile> GetSubmissionFiles(long callForPaperID, long submitterTypeID)
        //        {
        //            RfM_SubmitterType submitter = Manager.Get<RfM_SubmitterType>(submitterTypeID);
        //            RequestForMembership baseForPaper = Manager.Get<RequestForMembership >(callForPaperID);
        //            if (baseForPaper != null && submitter != null)
        //                return (from s in Manager.GetAll<RfM_RequestedFileAssignment>(s => s.RequestedFile.BaseForPaper == baseForPaper && s.Deleted == BaseStatusDeleted.None && s.SubmitterType == submitter
        //                        && s.RequestedFile.Deleted == BaseStatusDeleted.None)

        //                        orderby s.RequestedFile.Name
        //                        select new RfM_dtoSubmissionFile()
        //                        {
        //                            FileToSubmit = new RfM_dtoRequestedFile(s.RequestedFile),
        //                            FileSubmitted = null,
        //                            Deleted = s.Deleted,
        //                            AllowUpload = true,
        //                            Id = s.Id
        //                        }).ToList();
        //            else
        //                return new List<RfM_dtoSubmissionFile>();
        //        }

        //        public IList<dtoFieldsSection< dtoSubmissionField>> GetSubmissionSections(long callForPaperID, long submitterTypeID)
        //        {
        //            RfM_SubmitterType submitter = Manager.Get<RfM_SubmitterType>(submitterTypeID);
        //            RequestForMembership baseForPaper = Manager.Get<RequestForMembership>(callForPaperID);
        //            if (baseForPaper != null && submitter != null)
        //            {
        //                return (from f in Manager.GetAll<RfM_FieldAssignment>(f => f.Deleted == BaseStatusDeleted.None && f.SubmitterType == submitter && f.Field.BaseForPaper == baseForPaper)
        //                        group f by f.Field.Section into sections
        //                        orderby sections.Key.DisplayOrder
        //                        select new dtoFieldsSection< dtoSubmissionField>()
        //                        {
        //                            CallForPaperId = callForPaperID,
        //                            Deleted = sections.Key.Deleted,
        //                            DisplayOrder = sections.Key.DisplayOrder,
        //                            Id = sections.Key.Id,
        //                            Name = sections.Key.Name,
        //                            Fields = (from sf in sections where sf.Deleted == BaseStatusDeleted.None && sf.Field != null && sf.Field.Deleted == BaseStatusDeleted.None && sf.SubmitterType == submitter select new dtoSubmissionField(sf.Field)).OrderBy(f => f.DisplayOrder).ToList()
        //                        }
        //                       ).ToList();
        //            }
        //            else
        //                return new List<dtoFieldsSection< dtoSubmissionField>>();
        //        }
        ///// <summary>
        ///// ToDo NH3
        ///// </summary>
        ///// <param name="submission"></param>
        ///// <returns></returns>
        
        //public IList<dtoFieldsSection< dtoSubmissionField>> GetSubmissionSections(RfM_UserSubmission submission)
        //        {
        //            IList<dtoFieldsSection<dtoSubmissionField>> results = null;

        //            if (submission != null)
        //            {
        //                RequestForMembership baseForPaper = Manager.Get<RequestForMembership>(submission.BaseForPaper.Id);
        //                String test = baseForPaper.Name;
        //                results = (from f in Manager.GetAll<RfM_FieldAssignment>(f => f.Deleted == BaseStatusDeleted.None && f.SubmitterType == submission.Type && f.Field.BaseForPaper == baseForPaper)
        //                           group f by f.Field.Section into sections
        //                           orderby sections.Key.DisplayOrder
        //                           select new dtoFieldsSection<dtoSubmissionField>()
        //                           {
        //                               CallForPaperId = submission.BaseForPaper.Id,
        //                               Deleted = sections.Key.Deleted,
        //                               DisplayOrder = sections.Key.DisplayOrder,
        //                               Id = sections.Key.Id,
        //                               Name = sections.Key.Name,
        //                               Fields = (from sf in sections
        //                                         where sf.Deleted == BaseStatusDeleted.None && sf.SubmitterType == submission.Type && sf.Field.Deleted == BaseStatusDeleted.None
        //                                         select new dtoSubmissionField(sf.Field, (from vf in Manager.GetAll<RfM_FieldValue>(vf => vf.Field == sf.Field && vf.Submission == submission && vf.Deleted == BaseStatusDeleted.None) select vf).FirstOrDefault())).OrderBy(f => f.DisplayOrder).ToList()
        //                           }
        //                       ).ToList();

        //                //// CHIEDERE A ROBERTO
        //                //results = GetSubmissionSections(submission.BaseForPaper.Id, submission.Type.Id);

        //                //List<RfM_FieldValue> values = (from vf in Manager.GetAll<RfM_FieldValue>(vf => vf.Submission == submission && vf.Deleted == BaseStatusDeleted.None) select vf).ToList();
        //                //foreach (dtoFieldsSection<dtoSubmissionField> section in results)
        //                //{
        //                //    foreach (dtoSubmissionField field in section.Fields)
        //                //    {
        //                //        RfM_FieldValue value = (from v in values where v.Field.Id == field.FieldDefinitionId select v).FirstOrDefault();
        //                //        if (value != null)
        //                //        {
        //                //            field.Value = value.Value;
        //                //            field.FieldValueId = value.Id;
        //                //        }
        //                //    }
        //                //}

        //                //List<dtoSubmissionField> fields = new List<dtoSubmissionField>();
        //                //fields.AddRange(results.ToList().ForEach(r=> r.Fields)
        //                //RequestForMembership baseForPaper = Manager.Get<RequestForMembership>(submission.BaseForPaper.Id);
                      
        //                //results = (from f in Manager.GetAll<RfM_FieldAssignment>(f => f.Deleted == BaseStatusDeleted.None && f.SubmitterType == submission.Type && f.Field.BaseForPaper == submission.BaseForPaper)
        //                //        group f by f.Field.Section into sections
        //                //        orderby sections.Key.DisplayOrder
        //                //        select new dtoFieldsSection< dtoSubmissionField>()
        //                //        {
        //                //            CallForPaperId = submission.BaseForPaper.Id,
        //                //            Deleted = sections.Key.Deleted,
        //                //            DisplayOrder = sections.Key.DisplayOrder,
        //                //            Id = sections.Key.Id,
        //                //            Name = sections.Key.Name,
        //                //            Fields = (from sf in sections
        //                //                      where sf.Deleted == BaseStatusDeleted.None && sf.SubmitterType == submission.Type && sf.Field.Deleted == BaseStatusDeleted.None
        //                //                      select new  dtoSubmissionField(sf.Field)).OrderBy(f=>f.DisplayOrder).ToList()
        //                //        }
        //                //       ).ToList();

        //                //
                       
        //            }
        //            else
        //                results = new List<dtoFieldsSection< dtoSubmissionField>>();
        //            return results;
        //        }
        //    #endregion

        //    #region "Save Submission"
        //        public Boolean AllowCompleteSubmission(RfM_UserSubmission submission)
        //        {
        //            Boolean allow = false;
        //            if (submission != null)
        //            {
        //                List<long> valuesID = (from fv in submission.FieldsValues where fv.Field.Mandatory && fv.Deleted == BaseStatusDeleted.None && !string.IsNullOrEmpty(fv.Value) select fv.Field.Id).ToList();
        //                var FilesID = (from f in Manager.GetIQ<RfM_SubmittedFile>()
        //                               where f.Submission == submission && f.Deleted == BaseStatusDeleted.None && f.SubmittedAs.Deleted == BaseStatusDeleted.None
        //                               select f.SubmittedAs.Id);
        //                allow = !(from f in Manager.GetIQ<RfM_FieldAssignment>()
        //                          where f.SubmitterType == submission.Type && f.Deleted == BaseStatusDeleted.None && f.Field.Deleted== BaseStatusDeleted.None && f.Field.Mandatory && !valuesID.Contains(f.Field.Id)
        //                          select f.Id).Any();
        //                allow &= !(from rf in Manager.GetIQ<RfM_RequestedFileAssignment>()
        //                           where rf.SubmitterType == submission.Type && rf.Deleted == BaseStatusDeleted.None && rf.RequestedFile.Deleted == BaseStatusDeleted.None
        //                           && rf.RequestedFile.Mandatory && !FilesID.ToList().Contains(rf.RequestedFile.Id)
        //                           select rf.Id).Any();
        //            }
        //            return allow;
        //        }
        //        public void SaveSubmissionFiles(RfM_UserSubmission submission, int personId, String IpAddress, String ProxyIpAddress, IList<dtoSubmissionUploadedFile> files)
        //        {
        //            try
        //            {
        //                litePerson person = Manager.GetLitePerson(personId);
        //                if (files.Count > 0)
        //                {
        //                    int moduleID = Manager.GetModuleID(ModuleRequestForMembership.UniqueCode);
        //                    Manager.BeginTransaction();
        //                    foreach (dtoSubmissionUploadedFile file in files)
        //                    {
        //                        RfM_SubmittedFile ToSubmit = new RfM_SubmittedFile();
        //                        ToSubmit.CreateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                        ToSubmit.Submission = submission;
        //                        ToSubmit.SubmittedAs = Manager.Get<RfM_RequestedFile>(file.FileToSubmitID);
        //                        ToSubmit.File = (BaseCommunityFile)file.actionLink.ModuleObject.ObjectOwner;
        //                        RfM_SubmittedFile submitted = Manager.GetAll<RfM_SubmittedFile>(s => s.Submission == submission && s.SubmittedAs.Id == file.FileToSubmitID && s.Deleted == BaseStatusDeleted.None).FirstOrDefault();
        //                        if (submitted != null)
        //                        {
        //                            submitted.UpdateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                            submitted.File.isDeleted = true;
        //                            Manager.SaveOrUpdate(submitted.File);
        //                            submitted.Link.Deleted = BaseStatusDeleted.Automatic;
        //                            submitted.Deleted = BaseStatusDeleted.Manual;
        //                            Manager.SaveOrUpdate(submitted);
        //                        }
        //                        Manager.SaveOrUpdate(ToSubmit);
        //                        ModuleLink link = new ModuleLink(file.actionLink.Description, file.actionLink.Permission, file.actionLink.Action);
        //                        link.CreateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                        link.DestinationItem = (ModuleObject)file.actionLink.ModuleObject;
        //                        link.SourceItem = ModuleObject.CreateLongObject(submission.Id, submission, (int)ModuleRequestForMembership.ObjectType.UserSubmission, submission.BaseForPaper.Community.Id, ModuleRequestForMembership.UniqueCode, moduleID);
        //                        Manager.SaveOrUpdate(link);
        //                        ToSubmit.Link = link;
        //                        Manager.SaveOrUpdate(ToSubmit);
        //                    }
        //                    Manager.Commit();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Manager.RollBack();
        //                throw new SubmissionInternalFileNotLinked(ex.Message, ex);
        //            }
        //        }
        //        public RfM_UserSubmission SaveSubmission(long submissionId, long callForPaperID, long selectedSubmitterTypeID, int ownerId, string IpAddress, string ProxyIpAddress, IList<dtoSubmissionField> fields)
        //        {
        //            RfM_UserSubmission submission = Manager.Get<RfM_UserSubmission>(submissionId);
        //            litePerson person = Manager.GetLitePerson(ownerId);
        //            if (submission == null && submissionId > 0)
        //                throw new SubmissionNotFound(submissionId.ToString());
        //            else
        //            {
        //                try
        //                {
        //                    Manager.BeginTransaction();
        //                    Boolean forCreate = false;
        //                    if (submission == null)
        //                    {
        //                        submission = new RfM_UserSubmission();
        //                        submission.CreateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                        submission.BaseForPaper = Manager.Get<RequestForMembership>(callForPaperID);
        //                        submission.Community = submission.BaseForPaper.Community;
        //                        submission.Type = Manager.Get<RfM_SubmitterType>(selectedSubmitterTypeID);
        //                        submission.Owner = person;
        //                        submission.UserCode = Guid.NewGuid();
        //                        submission.isAnonymous = false;
        //                        forCreate = true;
        //                    }
        //                    else
        //                    {
        //                        submission.UpdateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                    }
        //                    Manager.SaveOrUpdate(submission);
        //                    IList<RfM_FieldValue> values = new List<RfM_FieldValue>();
        //                    if (fields.Count > 0)
        //                    {
        //                        foreach (dtoSubmissionField field in fields)
        //                        {
        //                            RfM_FieldValue fieldValue = Manager.Get<RfM_FieldValue>(field.FieldValueId);
        //                            if (fieldValue == null)
        //                            {
        //                                RfM_FieldDefinition definition = Manager.Get<RfM_FieldDefinition>(field.FieldDefinitionId);
        //                                if (definition.BaseForPaper.Id == submission.BaseForPaper.Id)
        //                                {
        //                                    fieldValue = new RfM_FieldValue();
        //                                    fieldValue.CreateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                                    fieldValue.Submission = submission;
        //                                    fieldValue.Field = definition;
        //                                    fieldValue.Value = field.Value;
        //                                    values.Add(fieldValue);
        //                                }
        //                            }
        //                            else if (fieldValue.Field.BaseForPaper.Id == submission.BaseForPaper.Id)
        //                            {
        //                                fieldValue.UpdateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                                fieldValue.Value = field.Value;
        //                                values.Add(fieldValue);
        //                            }
        //                        }
        //                        if (values.Count > 0)
        //                        {
        //                            Manager.SaveOrUpdateList(values);
        //                            //   submission.FieldsValues = values;
        //                            //   Manager.SaveOrUpdate(submission);
        //                        }
        //                    }
        //                    Manager.Commit();
        //                    var queryUpdate = (from fv in values where !(from RfM_FieldValue av in submission.FieldsValues select av.Id).ToList().Contains(fv.Id) select fv);
        //                    foreach (RfM_FieldValue fv in queryUpdate.ToList())
        //                    {
        //                        submission.FieldsValues.Add(fv);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Manager.RollBack();
        //                    if (submissionId > 0)
        //                        throw new SubmissionNotSaved(ex.Message, ex);
        //                    else
        //                        throw new SubmissionNotCreated(ex.Message, ex);
        //                }

        //            }
        //            return submission;
        //        }
        ////------------------------------------------------------------------------------------------------------------------------------------
        //        public RfM_UserSubmission SaveAnonymousSubmission(long submissionId, long callForPaperID, long selectedSubmitterTypeID, string IpAddress, string ProxyIpAddress, IList<dtoSubmissionField> fields)
        //        {
        //            RfM_UserSubmission submission = Manager.Get<RfM_UserSubmission>(submissionId);
        //            litePerson person = (from p in Manager.GetIQ<Person>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
        //            if (submission == null && submissionId > 0)
        //                throw new SubmissionNotFound(submissionId.ToString());
        //            else
        //            {
        //                try
        //                {
        //                    Manager.BeginTransaction();
        //                    Boolean forCreate = false;
        //                    if (submission == null)
        //                    {
        //                        submission = new RfM_UserSubmission();
        //                        submission.CreateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                        submission.BaseForPaper = Manager.Get<RequestForMembership>(callForPaperID);
        //                        submission.Community = submission.BaseForPaper.Community;
        //                        submission.Type = Manager.Get<RfM_SubmitterType>(selectedSubmitterTypeID);
        //                        submission.Owner = person;
        //                        submission.UserCode = Guid.NewGuid();
        //                        submission.isAnonymous = true;
        //                        submission.isComplete = false;
        //                        forCreate = true;
        //                    }
        //                    else
        //                    {
        //                        submission.UpdateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                    }
        //                    Manager.SaveOrUpdate(submission);
        //                    IList<RfM_FieldValue> values = new List<RfM_FieldValue>();
        //                    if (fields.Count > 0)
        //                    {
        //                        foreach (dtoSubmissionField field in fields)
        //                        {
        //                            RfM_FieldValue fieldValue = Manager.Get<RfM_FieldValue>(field.FieldValueId);
        //                            if (fieldValue == null)
        //                            {
        //                                RfM_FieldDefinition definition = Manager.Get<RfM_FieldDefinition>(field.FieldDefinitionId);
        //                                if (definition.BaseForPaper.Id == submission.BaseForPaper.Id)
        //                                {
        //                                    fieldValue = new RfM_FieldValue();
        //                                    fieldValue.CreateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                                    fieldValue.Submission = submission;
        //                                    fieldValue.Field = definition;
        //                                    fieldValue.Value = field.Value;
        //                                    values.Add(fieldValue);
        //                                }
        //                            }
        //                            else if (fieldValue.Field.BaseForPaper.Id == submission.BaseForPaper.Id)
        //                            {
        //                                fieldValue.UpdateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                                fieldValue.Value = field.Value;
        //                                values.Add(fieldValue);
        //                            }
        //                        }
        //                        if (values.Count > 0)
        //                        {
        //                            Manager.SaveOrUpdateList(values);
        //                            //   submission.FieldsValues = values;
        //                            //   Manager.SaveOrUpdate(submission);
        //                        }
        //                    }
        //                    Manager.Commit();
        //                    var queryUpdate = (from fv in values where !(from RfM_FieldValue av in submission.FieldsValues select av.Id).ToList().Contains(fv.Id) select fv);
        //                    foreach (RfM_FieldValue fv in queryUpdate.ToList())
        //                    {
        //                        submission.FieldsValues.Add(fv);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Manager.RollBack();
        //                    if (submissionId > 0)
        //                        throw new SubmissionNotSaved(ex.Message, ex);
        //                    else
        //                        throw new SubmissionNotCreated(ex.Message, ex);
        //                }

        //            }
        //            return submission;
        //        }
        ////---------------------------------------------------------------------------------------------------------------------
        //        public void CompleteSubmission(RfM_UserSubmission submission, String BaseFilePath)
        //        {
        //            if (AllowCompleteSubmission(submission))
        //            {
        //                try
        //                {
        //                    IList<string> filesToRemove = new List<string>();

        //                    Manager.BeginTransaction();
        //                    submission.Status = SubmissionStatus.submitted;
        //                    submission.SubmittedOn = DateTime.Now;
        //                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);

        //                    string fileName = BaseFilePath + "\\{0}\\{1}.stored";

        //                    if (submission.LinkZip != null)
        //                        Manager.DeletePhysical(submission.LinkZip);
        //                    if (submission.FileZip != null)
        //                    {
        //                        filesToRemove.Add(String.Format(fileName, submission.FileZip.CommunityOwner == null ? "0" : submission.FileZip.CommunityOwner.Id.ToString(), submission.FileZip.UniqueID.ToString()));
        //                        Manager.DeletePhysical(submission.FileZip);
        //                    }

        //                    ModuleLongInternalFile internalFile = CreateInternalFile(person, submission);
        //                    int index = 1;
        //                    int count = (from f in submission.Files where f.Deleted == BaseStatusDeleted.None select f).Count();
        //                    List<dtoFileToZip> filesToZip = (from f in submission.Files
        //                                                     where f.Deleted == BaseStatusDeleted.None
        //                                                     orderby f.SubmittedAs.Name
        //                                                     select new dtoFileToZip(String.Format(fileName, f.File.CommunityOwner == null ? "0" : f.File.CommunityOwner.Id.ToString(), f.File.UniqueID.ToString()),
        //                                                         f.File.DisplayName, ref index, count)).ToList();

        //                    String fileZip = String.Format(fileName, submission.Community == null ? "0" : submission.Community.Id.ToString(), internalFile.UniqueID.ToString());
        //                    ZipFilesRename(filesToZip, fileZip);
        //                    System.IO.FileInfo info = new System.IO.FileInfo(fileZip);
        //                    if (info != null)
        //                    {
        //                        internalFile.Size = info.Length;
        //                        Manager.SaveOrUpdate(internalFile);

        //                        ModuleLink link = new ModuleLink("", (int)CoreModuleRepository.Base2Permission.DownloadFile, (int)CoreModuleRepository.ActionType.DownloadFile);
        //                        link.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        //                        link.DestinationItem = ModuleObject.CreateLongObject(internalFile.Id, internalFile, (int)CoreModuleRepository.ObjectType.File, submission.BaseForPaper.Community.Id, CoreModuleRepository.UniqueID, Manager.GetModuleID(CoreModuleRepository.UniqueID));
        //                        link.SourceItem = ModuleObject.CreateLongObject(submission.Id, submission, (int)ModuleRequestForMembership.ObjectType.UserSubmission, submission.BaseForPaper.Community.Id, ModuleRequestForMembership.UniqueCode, ServiceModuleID());
        //                        Manager.SaveOrUpdate(link);
        //                        submission.LinkZip = link;
        //                        submission.FileZip = internalFile;
        //                        Manager.SaveOrUpdate(submission);
        //                    }

        //                    Manager.SaveOrUpdate(submission);
        //                    Manager.Commit();
        //                    if (!string.IsNullOrEmpty(submission.BaseForPaper.NotificationEmail)) //TODO: verificare IF vuoto
        //                    {

        //                    }
        //                    Delete.Files(filesToRemove);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Manager.RollBack();
        //                    throw new SubmissionStatusChange(ex.Message, ex);
        //                }
        //            }
        //            else
        //                throw new SubmissionItemsEmpty();
        //        }
        //        public void UserCompleteSubmission(RfM_UserSubmission submission, DateTime InitTime, int userId, String IpAddress, String ProxyIpAddress, String BaseFilePath)
        //        {
        //            if (!submission.BaseForPaper.AllowLateSubmission(InitTime, submission))
        //            {
        //                throw new SubmissionTimeExpired();
        //            }
        //            else
        //            {
        //                if (AllowCompleteSubmission(submission))
        //                {
        //                    IList<string> filesToRemove = new List<string>();
        //                    try
        //                    {
        //                        Manager.BeginTransaction();
        //                        litePerson person = Manager.GetLitePerson(userId);
        //                        Manager.Refresh(submission);
        //                        submission.Status = SubmissionStatus.submitted;
        //                        submission.SubmittedOn = DateTime.Now;
        //                        submission.SubmittedBy = person;
        //                        submission.UpdateMetaInfo(person, IpAddress, ProxyIpAddress);


        //                        // DA VERIFICARE DOPO CON ALESSIO


        //                        string fileName = BaseFilePath + "\\{0}\\{1}.stored";

        //                        if (submission.LinkZip != null)
        //                            Manager.DeletePhysical(submission.LinkZip);
        //                        if (submission.FileZip != null)
        //                        {
        //                            filesToRemove.Add(String.Format(fileName, submission.FileZip.CommunityOwner == null ? "0" : submission.FileZip.CommunityOwner.Id.ToString(), submission.FileZip.UniqueID.ToString()));
        //                            Manager.DeletePhysical(submission.FileZip);
        //                        }

        //                        int count = (from f in submission.Files where f.Deleted == BaseStatusDeleted.None select f).Count();
        //                        if (count > 0)
        //                        {
        //                            ModuleLongInternalFile internalFile = CreateInternalFile(person, submission);
        //                            int index = 1;
                                    
        //                            List<dtoFileToZip> filesToZip = (from f in submission.Files
        //                                                             where f.Deleted == BaseStatusDeleted.None
        //                                                             orderby f.SubmittedAs.Name
        //                                                             select new dtoFileToZip(String.Format(fileName, f.File.CommunityOwner == null ? "0" : f.File.CommunityOwner.Id.ToString(), f.File.UniqueID.ToString()),
        //                                                                 f.File.DisplayName, ref index, count)).ToList();

        //                            String fileZip = String.Format(fileName, submission.Community == null ? "0" : submission.Community.Id.ToString(), internalFile.UniqueID.ToString());
        //                            ZipFilesRename(filesToZip, fileZip);
        //                            System.IO.FileInfo info = new System.IO.FileInfo(fileZip);
        //                            if (info != null)
        //                            {
        //                                internalFile.Size = info.Length;
        //                                Manager.SaveOrUpdate(internalFile);

        //                                ModuleLink link = new ModuleLink("", (int)CoreModuleRepository.Base2Permission.DownloadFile, (int)CoreModuleRepository.ActionType.DownloadFile);
        //                                link.CreateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                                link.DestinationItem = ModuleObject.CreateLongObject(internalFile.Id, internalFile, (int)CoreModuleRepository.ObjectType.File, submission.BaseForPaper.Community.Id, CoreModuleRepository.UniqueID, Manager.GetModuleID(CoreModuleRepository.UniqueID));
        //                                link.SourceItem = ModuleObject.CreateLongObject(submission.Id, submission, (int)ModuleRequestForMembership.ObjectType.UserSubmission, submission.BaseForPaper.Community.Id, ModuleRequestForMembership.UniqueCode, ServiceModuleID());
        //                                Manager.SaveOrUpdate(link);
        //                                submission.LinkZip = link;
        //                                submission.FileZip = internalFile;
        //                                Manager.SaveOrUpdate(submission);
        //                            }

        //                        }
                              

                               
        //                        Manager.SaveOrUpdate(submission);
        //                        Manager.Commit();

        //                        Delete.Files(filesToRemove);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Manager.RollBack();
        //                        throw new SubmissionStatusChange(ex.Message, ex);
        //                    }
        //                }
        //                else
        //                    throw new SubmissionItemsEmpty();
        //            }
        //        }

        //        public void UserCompleteAnonymousSubmission(RfM_UserSubmission submission, DateTime InitTime, Guid usercode, String IpAddress, String ProxyIpAddress, String BaseFilePath)
        //        {
        //            if (!submission.BaseForPaper.AllowLateSubmission(InitTime, submission))
        //            {
        //                throw new SubmissionTimeExpired();
        //            }
        //            else
        //            {
        //                if (AllowCompleteSubmission(submission))
        //                {
        //                    IList<string> filesToRemove = new List<string>();
        //                    try
        //                    {
        //                        Manager.BeginTransaction();
        //                        litePerson person = submission.Owner; // Manager.GetLitePerson(userId);
        //                        Manager.Refresh(submission);
        //                        submission.Status = SubmissionStatus.submitted;
        //                        submission.SubmittedOn = DateTime.Now;
        //                        submission.SubmittedBy = person;
        //                        submission.UpdateMetaInfo(person, IpAddress, ProxyIpAddress);


        //                        string fileName = BaseFilePath + "\\{0}\\{1}.stored";

        //                        if (submission.LinkZip != null)
        //                            Manager.DeletePhysical(submission.LinkZip);
        //                        if (submission.FileZip != null)
        //                        {
        //                            filesToRemove.Add(String.Format(fileName, submission.FileZip.CommunityOwner == null ? "0" : submission.FileZip.CommunityOwner.Id.ToString(), submission.FileZip.UniqueID.ToString()));
        //                            Manager.DeletePhysical(submission.FileZip);
        //                        }

        //                        ModuleLongInternalFile internalFile = CreateInternalFile(person, submission);
        //                        int index = 1;
        //                        int count = (from f in submission.Files where f.Deleted == BaseStatusDeleted.None select f).Count();
        //                        List<dtoFileToZip> filesToZip = (from f in submission.Files
        //                                                         where f.Deleted == BaseStatusDeleted.None
        //                                                         orderby f.SubmittedAs.Name
        //                                                         select new dtoFileToZip(String.Format(fileName, f.File.CommunityOwner == null ? "0" : f.File.CommunityOwner.Id.ToString(), f.File.UniqueID.ToString()),
        //                                                             f.File.DisplayName, ref index, count)).ToList();

        //                        String fileZip = String.Format(fileName, submission.Community == null ? "0" : submission.Community.Id.ToString(), internalFile.UniqueID.ToString());
        //                        ZipFilesRename(filesToZip, fileZip);
        //                        System.IO.FileInfo info = new System.IO.FileInfo(fileZip);
        //                        if (info != null)
        //                        {
        //                            internalFile.Size = info.Length;
        //                            Manager.SaveOrUpdate(internalFile);

        //                            ModuleLink link = new ModuleLink("", (int)CoreModuleRepository.Base2Permission.DownloadFile, (int)CoreModuleRepository.ActionType.DownloadFile);
        //                            link.CreateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                            link.DestinationItem = ModuleObject.CreateLongObject(internalFile.Id, internalFile, (int)CoreModuleRepository.ObjectType.File, submission.BaseForPaper.Community.Id, CoreModuleRepository.UniqueID, Manager.GetModuleID(CoreModuleRepository.UniqueID));
        //                            link.SourceItem = ModuleObject.CreateLongObject(submission.Id, submission, (int)ModuleRequestForMembership.ObjectType.UserSubmission, submission.BaseForPaper.Community.Id, ModuleRequestForMembership.UniqueCode, ServiceModuleID());
        //                            Manager.SaveOrUpdate(link);
        //                            submission.LinkZip = link;
        //                            submission.FileZip = internalFile;
        //                            Manager.SaveOrUpdate(submission);
        //                        }

        //                        Manager.SaveOrUpdate(submission);
        //                        Manager.Commit();

        //                        Delete.Files(filesToRemove);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Manager.RollBack();
        //                        throw new SubmissionStatusChange(ex.Message, ex);
        //                    }
        //                }
        //                else
        //                    throw new SubmissionItemsEmpty();
        //            }
        //        }
        //        public void UpdateSubmissionZipFile(RfM_UserSubmission submission, String IpAddress, String ProxyIpAddress, String BaseFilePath)
        //        {
        //            IList<string> filesToRemove = new List<string>();
        //            try
        //            {
        //                Manager.BeginTransaction();
        //                litePerson person = submission.SubmittedBy;
        //                string fileName = BaseFilePath + "\\{0}\\{1}.stored";

        //                if (submission.LinkZip != null)
        //                    Manager.DeletePhysical(submission.LinkZip);
        //                if (submission.FileZip != null)
        //                {
        //                    filesToRemove.Add(String.Format(fileName, submission.FileZip.CommunityOwner == null ? "0" : submission.FileZip.CommunityOwner.Id.ToString(), submission.FileZip.UniqueID.ToString()));
        //                    Manager.DeletePhysical(submission.FileZip);
        //                }

        //                ModuleLongInternalFile internalFile = CreateInternalFile(person, submission);
        //                int index = 1;
        //                int count = (from f in submission.Files where f.Deleted == BaseStatusDeleted.None select f).Count();
        //                List<dtoFileToZip> filesToZip = (from f in submission.Files
        //                                                    where f.Deleted == BaseStatusDeleted.None
        //                                                    orderby f.SubmittedAs.Name
        //                                                    select new dtoFileToZip(String.Format(fileName, f.File.CommunityOwner == null ? "0" : f.File.CommunityOwner.Id.ToString(), f.File.UniqueID.ToString()),
        //                                                        f.File.DisplayName, ref index, count)).ToList();

        //                String fileZip = String.Format(fileName, submission.Community == null ? "0" : submission.Community.Id.ToString(), internalFile.UniqueID.ToString());
        //                ZipFilesRename(filesToZip, fileZip);
        //                System.IO.FileInfo info = new System.IO.FileInfo(fileZip);
        //                if (info != null)
        //                {
        //                    internalFile.Size = info.Length;
        //                    Manager.SaveOrUpdate(internalFile);

        //                    ModuleLink link = new ModuleLink("", (int)CoreModuleRepository.Base2Permission.DownloadFile, (int)CoreModuleRepository.ActionType.DownloadFile);
        //                    link.CreateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                    link.DestinationItem = ModuleObject.CreateLongObject(internalFile.Id, internalFile, (int)CoreModuleRepository.ObjectType.File, submission.BaseForPaper.Community.Id, CoreModuleRepository.UniqueID, Manager.GetModuleID(CoreModuleRepository.UniqueID));
        //                    link.SourceItem = ModuleObject.CreateLongObject(submission.Id, submission, (int)ModuleCallForPaper.ObjectType.UserSubmission, submission.BaseForPaper.Community.Id, ModuleRequestForMembership.UniqueCode, ServiceModuleID());
        //                    Manager.SaveOrUpdate(link);
        //                    submission.LinkZip = link;
        //                    submission.FileZip = internalFile;
        //                    Manager.SaveOrUpdate(submission);
        //                }

        //                Manager.SaveOrUpdate(submission);
        //                Manager.Commit();

        //                Delete.Files(filesToRemove);
        //            }
        //            catch (Exception ex)
        //            {
        //                Manager.RollBack();
        //                throw new SubmissionStatusChange(ex.Message, ex);
        //            }
        //        }

        //        public IList<dtoFieldsSection<dtoFieldDefinition>> SubmissionEmptyMandatoryFields(RfM_UserSubmission submission)
        //        {
        //            IList<dtoFieldsSection<dtoFieldDefinition>> items = new List<dtoFieldsSection<dtoFieldDefinition>>();
        //            if (submission != null)
        //            {
        //                var ValuesID = (from fv in submission.FieldsValues where fv.Field.Mandatory && fv.Deleted == BaseStatusDeleted.None && !string.IsNullOrEmpty(fv.Value) select fv.Field.Id);
        //                List<long> test = ValuesID.ToList();
        //                items = (from f in Manager.GetAll<RfM_FieldAssignment>(f => f.Deleted == BaseStatusDeleted.None && f.Field.Deleted== BaseStatusDeleted.None && f.SubmitterType == submission.Type && f.Field.BaseForPaper == submission.BaseForPaper)
        //                         group f by f.Field.Section into sections
        //                         orderby sections.Key.DisplayOrder
        //                         select new dtoFieldsSection<dtoFieldDefinition>()
        //                         {
        //                             DisplayOrder = sections.Key.DisplayOrder,
        //                             Id = sections.Key.Id,
        //                             Name = sections.Key.Name,
        //                             Fields = (from sf in sections
        //                                       where sf.Deleted == BaseStatusDeleted.None && sf.SubmitterType == submission.Type && sf.Field.Mandatory && !ValuesID.ToList().Contains(sf.Field.Id)
        //                                       select new dtoFieldDefinition(sf.Field.Id, sf.Field.Name, sf.Field.DisplayOrder)).ToList()
        //                         }
        //                       ).ToList();
        //                items = (from i in items where i.Fields.Count > 0 select i).ToList();
        //            }
        //            return items;
        //        }
        //        public IList<RfM_dtoRequestedFile> SubmissionEmptyMandatoryFiles(RfM_UserSubmission submission)
        //        {
        //            IList<RfM_dtoRequestedFile> files = new List<RfM_dtoRequestedFile>();
        //            if (submission != null)
        //            {
        //                var FilesID = (from f in Manager.GetIQ<RfM_SubmittedFile>()
        //                               where f.Submission == submission && f.Deleted == BaseStatusDeleted.None && f.SubmittedAs.Deleted == BaseStatusDeleted.None
        //                               select f.SubmittedAs.Id);

        //                files = (from rf in Manager.GetAll<RfM_RequestedFileAssignment>(rf => rf.SubmitterType == submission.Type && rf.Deleted == BaseStatusDeleted.None && rf.RequestedFile.Deleted == BaseStatusDeleted.None
        //                         && rf.RequestedFile.Mandatory && !FilesID.ToList().Contains(rf.RequestedFile.Id))
        //                         select new RfM_dtoRequestedFile(rf.RequestedFile)).ToList();
        //            }
        //            return files;
        //        }

        //        public void RemoveSubmittedFile(long SubmittedId, int personId, string IpAddress, string ProxyIpAddress)
        //        {
        //            try
        //            {
        //                litePerson person = Manager.GetLitePerson(personId);
        //                RfM_SubmittedFile submittedFile = Manager.Get<RfM_SubmittedFile>(SubmittedId);
        //                if (submittedFile != null)
        //                {
        //                    Manager.BeginTransaction();
        //                    submittedFile.UpdateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                    submittedFile.Deleted |= BaseStatusDeleted.Manual;
        //                    Manager.SaveOrUpdate(submittedFile);
        //                    Manager.Commit();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Manager.RollBack();
        //            }
        //        }
        //        public void VirtualDeleteSubmission(long itemId, int personId, string IpAddress, string ProxyIpAddress, Boolean delete)
        //        {
        //            try
        //            {
        //                Manager.BeginTransaction();
        //                RfM_UserSubmission sub = Manager.Get<RfM_UserSubmission>(itemId);
        //                litePerson person = Manager.GetLitePerson(personId);
        //                if (sub != null && person != null)
        //                {
        //                    sub.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
        //                    sub.UpdateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                    Manager.SaveOrUpdate(sub);
        //                    Manager.Commit();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Manager.RollBack();
        //            }

        //        }

        //        public void SaveSubmissionStatus(long submissionId, int personId, SubmissionStatus status, string IpAddress, string ProxyIpAddress)
        //        {
        //            RfM_UserSubmission submission = Manager.Get<RfM_UserSubmission>(submissionId);
        //            litePerson person = Manager.GetLitePerson(personId);
        //            if (submission == null && submissionId > 0)
        //                throw new SubmissionNotFound(submissionId.ToString());
        //            else
        //            {
        //                try
        //                {
        //                    Manager.BeginTransaction();
        //                    if (submission != null)
        //                    {
        //                        submission.UpdateMetaInfo(person, IpAddress, ProxyIpAddress);
        //                        submission.Status = status;
        //                        Manager.SaveOrUpdate(submission);
        //                    }
        //                    Manager.Commit();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Manager.RollBack();
        //                    throw new SubmissionStatusChange(ex.Message, ex);
        //                }
        //            }
        //        }
        //    #endregion

        //    #region "Zip Files"
        //        public void ZipFilesRename(List<dtoFileToZip> zipFiles, String OutputFile)
        //        {

        //            using (ZipOutputStream s = new ZipOutputStream(File.Create(OutputFile)))
        //            {
        //                s.SetLevel(9); // 0-9, 9 being the highest compression

        //                byte[] buffer = new byte[4096];

        //                foreach (dtoFileToZip file in zipFiles)
        //                {

        //                    //String newFilename = Path.GetFileName(file.Replace(".dll", "._dll"));

        //                    String newFilename = file.FileName;
        //                    ZipEntry entry = new ZipEntry(newFilename);

        //                    entry.DateTime = DateTime.Now;

        //                    s.PutNextEntry(entry);

        //                    using (FileStream fs = File.OpenRead(file.StoredName))
        //                    {

        //                        int sourceBytes;

        //                        do
        //                        {

        //                            sourceBytes = fs.Read(buffer, 0, buffer.Length);

        //                            s.Write(buffer, 0, sourceBytes);

        //                        }

        //                        while (sourceBytes > 0);

        //                    }

        //                }

        //                s.Finish();

        //                s.Close();

        //                s.Dispose();
        //            }
        //        }

                
        //        private ModuleLongInternalFile CreateInternalFile(litePerson person, RfM_UserSubmission submission)
        //        {
        //            ModuleLongInternalFile zipFile = new ModuleLongInternalFile();
        //            zipFile.CloneID = 0;
        //            zipFile.ContentType = "application/x-zip-compressed";
        //            zipFile.Description = "";
        //            zipFile.Downloads = 0;
        //            zipFile.FileCategoryID = 0;
        //            zipFile.FolderId = 0;
        //            zipFile.isDeleted = false;
        //            zipFile.isPersonal = false;
        //            zipFile.isSCORM = false;
        //            zipFile.isVirtual = false;
        //            zipFile.isVideocast = false;
        //            zipFile.IsDownloadable = true;
        //            zipFile.isVisible = true;
        //            zipFile.isFile = true;
        //            zipFile.UniqueID = System.Guid.NewGuid();
        //            zipFile.IsInternal = true;
        //            zipFile.Name = "SubmittedFiles";
        //            zipFile.Extension = ".zip";
        //            zipFile.ObjectOwner = submission;
        //            zipFile.ObjectTypeID = (int)ModuleRequestForMembership.ObjectType.UserSubmission;
        //            zipFile.Owner = person;
        //            zipFile.CreatedOn = DateTime.Now;
        //            zipFile.ModifiedOn = zipFile.CreatedOn;
        //            zipFile.ModifiedBy = person;
        //            zipFile.CommunityOwner = submission.Community;
        //            zipFile.ServiceActionAjax = (int)ModuleRequestForMembership.ActionType.DownloadSubmittedFile;
        //            zipFile.ServiceOwner = ModuleRequestForMembership.UniqueCode;
        //            return zipFile;
        //        }
        //    #endregion

        //    #region "Print Submission"
        //        public Document PrintSubmission(long IdSubmission, Dictionary<SubmissionTranslations, string> translations,System.IO.Stream stream, Boolean isPdf) {
        //            try
        //            {
        //                RfM_UserSubmission submission = Manager.Get<RfM_UserSubmission>(IdSubmission);
        //                litePerson person = Manager.Get<Person>(UC.CurrentUserID);
        //                if (submission == null || person == null)
        //                    return null;
        //                else
        //                    return RfM_ExportHelpers.SubmissionCompiled(submission, GetSubmissionFiles(submission), GetSubmissionSections(submission), person, translations, stream, isPdf);
        //            }
        //            catch (Exception ex) {
        //                throw new ExportError("", ex) { ErrorDocument = RfM_ExportHelpers.CreateErrorDocument(translations, stream, isPdf) };
        //            }                
        //        }            
        //    #endregion
        #endregion

        #region iLinkedService
            protected override Boolean AllowCallAction(StandardActionType actionType, long idCall, litePerson person, int idCommunity, int idRole, ModuleObject destination)
            {
                bool hasPortalPermission = false;
                if (idCommunity == 0)
                {
                    ModuleRequestForMembership m = ModuleRequestForMembership.CreatePortalmodule(person.TypeID);
                    hasPortalPermission = m.Administration || m.ManageBaseForPapers || m.EditBaseForPaper;
                }

                Boolean iResponse = false;
                iResponse = (from s in Manager.GetIQ<RequestForMembership>() where s.Id == idCall select s.Id).Any();
                if (iResponse)
                {
                    switch (actionType)
                    {
                        case StandardActionType.Edit:
                            ModuleRequestForMembership m = ModuleRequestForMembership.CreatePortalmodule(person.TypeID);
                            Boolean isOwner = (from s in Manager.GetIQ<RequestForMembership>() where s.Id == idCall && s.CreatedBy == person select s.Id).Any();
                            long permission = (long)ModuleRequestForMembership.Base2Permission.Admin | (long)ModuleRequestForMembership.Base2Permission.ManageRequests | (long)ModuleRequestForMembership.Base2Permission.EditRequest;
                            iResponse = isOwner || (idCommunity==0 && (m.Administration || m.ManageBaseForPapers || m.EditBaseForPaper)) || Manager.HasModulePermission(person.Id, idRole, idCommunity, this.ServiceModuleID(), permission);
                            break;
                    }
                }
                return iResponse || hasPortalPermission;
            }
            //Inutile, solo per compatibilità
            protected override bool AllowDownloadSignFile(long idRevision, int idUser, litePerson person, int idCommunity, int idRole)
            {

                Boolean iResponse = false;
                Revision revision = Manager.Get<Revision>(idRevision);
                if (revision != null && revision.Submission != null && revision.Submission.Call != null)
                {
                    //Sottomittore
                    iResponse = revision.CreatedBy.Id == idUser
                        //Creatore
                                || revision.Submission.Call.CreatedBy.Id == idUser;
                }
                return iResponse;
            }

            protected override List<StandardActionType> GetStandardActionForFileOfCall(long idCall, long idItem, int idUser, litePerson person, int idCommunity, int idRole)
            {
                bool hasPortalPermission = false;
                if (idCommunity == 0)
                {
                    ModuleRequestForMembership m = ModuleRequestForMembership.CreatePortalmodule(person.TypeID);
                    hasPortalPermission = m.Administration || m.ManageBaseForPapers || m.EditBaseForPaper;
                }
                

                List<StandardActionType> actions = new List<StandardActionType>();

                lm.Comol.Core.FileRepository.Domain.ItemType type = (
                    from f in Manager.GetIQ<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>() 
                    where f.Id == idItem 
                    select f.Type
                    ).Skip(0).Take(1).ToList().FirstOrDefault();

                long permission = (long)ModuleRequestForMembership.Base2Permission.Admin | (long)ModuleRequestForMembership.Base2Permission.ManageRequests | (long)ModuleRequestForMembership.Base2Permission.ListRequests;

                Boolean isOwner = (from s in Manager.GetIQ<RequestForMembership>() where s.Id == idCall && (s.CreatedBy == person || s.IsPublic) select s.Id).Any();

                Boolean hasPermission = (isOwner) ? isOwner : Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);

                Boolean hasEditPermission = (isOwner) ? isOwner : Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), (long)ModuleRequestForMembership.Base2Permission.Admin | (long)ModuleRequestForMembership.Base2Permission.ManageRequests);


                if (isOwner || hasPermission || hasPortalPermission)
                {
                    actions.Add(StandardActionType.Play);
                    switch (type)
                    {
                        case Core.FileRepository.Domain.ItemType.Multimedia:
                        case Core.FileRepository.Domain.ItemType.ScormPackage:
                            actions.Add(StandardActionType.ViewPersonalStatistics);
                            break;
                    }
                }
                if (isOwner || hasEditPermission || hasPortalPermission)
                {
                    switch (type)
                    {
                        case Core.FileRepository.Domain.ItemType.Multimedia:
                        case Core.FileRepository.Domain.ItemType.ScormPackage:
                            actions.Add(StandardActionType.ViewAdvancedStatistics);
                            actions.Add(StandardActionType.EditMetadata);
                            break;
                    }
                }
                return actions;
            }
            protected override List<StandardActionType> GetStandardActionForAttachment(long idAttachment, long idItem, int idUser, litePerson person, int idCommunity, int idRole)
            {
                List<StandardActionType> actions = new List<StandardActionType>();

                lm.Comol.Core.FileRepository.Domain.ItemType type = (from f in Manager.GetIQ<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>() where f.Id == idItem select f.Type).Skip(0).Take(1).ToList().FirstOrDefault();

                long permission = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls | (long)ModuleCallForPaper.Base2Permission.ListCalls;

                AttachmentFile attachment = Manager.Get<AttachmentFile>(idAttachment);

                if (attachment != null && attachment.Call != null)
                {
                    Boolean isOwner = (from s in Manager.GetIQ<BaseForPaper>() where s.Id == attachment.Call.Id && (s.CreatedBy == person || s.IsPublic) select s.Id).Any();

                    Boolean hasPermission = (isOwner) ? isOwner 
                        : Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);

                    Boolean hasEditPermission = (isOwner) ? isOwner : Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls);


                    bool hasPortalPermission = false;
                    if (idCommunity == 0)
                    {
                        ModuleRequestForMembership m = ModuleRequestForMembership.CreatePortalmodule(person.TypeID);
                        hasPortalPermission = m.Administration || m.ManageBaseForPapers || m.EditBaseForPaper;
                    }

                    if (isOwner || hasPortalPermission || hasEditPermission || (hasPermission && (attachment.ForAll || AllowSeeAttachment(attachment, person))))
                    {
                        actions.Add(StandardActionType.Play);
                        switch (type)
                        {
                            case Core.FileRepository.Domain.ItemType.Multimedia:
                            case Core.FileRepository.Domain.ItemType.ScormPackage:
                                actions.Add(StandardActionType.ViewPersonalStatistics);
                                break;
                        }
                    }
                    if (isOwner || hasEditPermission || hasPortalPermission)
                    {
                        switch (type)
                        {
                            case Core.FileRepository.Domain.ItemType.Multimedia:
                            case Core.FileRepository.Domain.ItemType.ScormPackage:
                                actions.Add(StandardActionType.ViewAdvancedStatistics);
                                actions.Add(StandardActionType.EditMetadata);
                                break;
                        }
                    }
                }
                return actions;
            }
            protected override List<StandardActionType> GetStandardActionForFileOfSubmission(long idSubmission, long idItem, int idUser, litePerson person, int idCommunity, int idRole)
            {
                bool hasPortalPermission = false;
                if (idCommunity == 0)
                {
                    ModuleRequestForMembership m = ModuleRequestForMembership.CreatePortalmodule(person.TypeID);
                    hasPortalPermission = m.Administration || m.ManageBaseForPapers || m.EditBaseForPaper;
                }

                List<StandardActionType> actions = new List<StandardActionType>();

                lm.Comol.Core.FileRepository.Domain.ItemType type = (
                    from f in Manager.GetIQ<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>() 
                    where f.Id == idItem select f.Type
                    ).Skip(0).Take(1).ToList().FirstOrDefault();

                long permission = (long)ModuleRequestForMembership.Base2Permission.Admin | (long)ModuleRequestForMembership.Base2Permission.ManageRequests;

                Boolean isOwner = (from s in Manager.GetIQ<UserSubmission>() where s.Id == idSubmission && (s.CreatedBy == person || s.Owner == person) select s.Id).Any();

                Boolean hasPermission = (isOwner) ? isOwner : Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);

                if (isOwner || hasPermission || hasPortalPermission)
                {
                    actions.Add(StandardActionType.Play);
                    switch (type)
                    {
                        case Core.FileRepository.Domain.ItemType.Multimedia:
                        case Core.FileRepository.Domain.ItemType.ScormPackage:
                            actions.Add(StandardActionType.ViewPersonalStatistics);
                            break;
                    }
                }
                switch (type)
                {
                    case Core.FileRepository.Domain.ItemType.Multimedia:
                    case Core.FileRepository.Domain.ItemType.ScormPackage:
                        if (isOwner)
                            actions.Add(StandardActionType.EditMetadata);
                        if (isOwner || hasPermission || hasPortalPermission)
                            actions.Add(StandardActionType.ViewAdvancedStatistics);
                        break;
                }
                return actions;
            }
            protected override List<StandardActionType> GetStandardActionForSubmittedFile(long idSubmittedFile, long idItem, int idUser, litePerson person, int idCommunity, int idRole)
            {
                bool hasPortalPermission = false;
                if (idCommunity == 0)
                {
                    ModuleRequestForMembership m = ModuleRequestForMembership.CreatePortalmodule(person.TypeID);
                    hasPortalPermission = m.Administration || m.ManageBaseForPapers || m.EditBaseForPaper;
                }

                List<StandardActionType> actions = new List<StandardActionType>();
                lm.Comol.Core.FileRepository.Domain.ItemType type = (from f in Manager.GetIQ<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>() where f.Id == idItem select f.Type).Skip(0).Take(1).ToList().FirstOrDefault();
                long permission = (long)ModuleRequestForMembership.Base2Permission.Admin | (long)ModuleRequestForMembership.Base2Permission.ManageRequests;

                Boolean isOwner = (from sf in Manager.GetAll<SubmittedFile>(sf => sf.Id == idSubmittedFile &&
                             (sf.CreatedBy == person || sf.Submission.CreatedBy == person || sf.Submission.Owner == person))
                                   select sf.Id).Any();

                Boolean hasPermission = (isOwner) ? isOwner : Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);

                if (isOwner || hasPermission || hasPortalPermission)
                {
                    actions.Add(StandardActionType.Play);
                    switch (type)
                    {
                        case Core.FileRepository.Domain.ItemType.Multimedia:
                        case Core.FileRepository.Domain.ItemType.ScormPackage:
                            actions.Add(StandardActionType.ViewPersonalStatistics);
                            break;
                    }
                }
                switch (type)
                {
                    case Core.FileRepository.Domain.ItemType.Multimedia:
                    case Core.FileRepository.Domain.ItemType.ScormPackage:
                        if (isOwner)
                            actions.Add(StandardActionType.EditMetadata);
                        if (isOwner || hasPermission)
                            actions.Add(StandardActionType.ViewAdvancedStatistics);
                        break;
                }
                return actions;
            }

            protected override Boolean AllowDownloadFileOfCall(long idCall, int idUser, litePerson person, int idCommunity, int idRole)
            {
                bool hasPortalPermission = false;
                if (idCommunity == 0)
                {
                    ModuleRequestForMembership m = ModuleRequestForMembership.CreatePortalmodule(person.TypeID);
                    hasPortalPermission = m.Administration || m.ManageBaseForPapers || m.EditBaseForPaper;
                }

                Boolean iResponse = false;

                iResponse = (from s in Manager.GetIQ<BaseForPaper>() where s.Id == idCall && (s.CreatedBy == person || s.IsPublic) select s.Id).Any();
                if (!iResponse)
                {
                    long permission = (long)ModuleRequestForMembership.Base2Permission.Admin | (long)ModuleRequestForMembership.Base2Permission.ManageRequests | (long)ModuleRequestForMembership.Base2Permission.ListRequests;

                    iResponse = Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
                    if (!iResponse)
                        iResponse = IsCallAvailableByUser(idCall, person);
                }
                return iResponse || hasPortalPermission;
            }
            protected override Boolean AllowAttachmentDownload(long idAttachment, int idUser, litePerson person, int idCommunity, int idRole)
            {
                bool hasPortalPermission = false;
                if (idCommunity == 0)
                {
                    ModuleRequestForMembership m = ModuleRequestForMembership.CreatePortalmodule(person.TypeID);
                    hasPortalPermission = m.Administration || m.ManageBaseForPapers || m.EditBaseForPaper;
                }

                Boolean iResponse = false;

                AttachmentFile attachment = Manager.Get<AttachmentFile>(idAttachment);
                if (attachment != null && attachment.Call != null)
                {
                    iResponse = (from s in Manager.GetIQ<BaseForPaper>() where s.Id == attachment.Call.Id && (s.CreatedBy == person || s.IsPublic) select s.Id).Any();
                    if (!iResponse)
                    {
                        iResponse = IsCallAvailableByUser(attachment.Call.Id, person);
                        //long permission = (long)ModuleCallForPaper.Base2Permission.Admin | (long)ModuleCallForPaper.Base2Permission.ManageCalls | (long)ModuleCallForPaper.Base2Permission.ListCalls;

                        //iResponse = Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
                        if (iResponse)
                            iResponse = (attachment.ForAll || AllowSeeAttachment(attachment, person));
                    }
                }
                return iResponse || hasPortalPermission;
            }
            protected override Boolean AllowDownloadFileOfSubmission(long idSubmission, int idUser, litePerson person, int idCommunity, int idRole)
            {
                bool hasPortalPermission = false;
                if (idCommunity == 0)
                {
                    ModuleRequestForMembership m = ModuleRequestForMembership.CreatePortalmodule(person.TypeID);
                    hasPortalPermission = m.Administration || m.ManageBaseForPapers || m.EditBaseForPaper;
                }

                Boolean iResponse = false;

                iResponse = (from s in Manager.GetIQ<UserSubmission>() where s.Id == idSubmission && (s.CreatedBy == person || s.Owner == person) select s.Id).Any();
                if (!iResponse)
                {
                    long permission = (long)ModuleRequestForMembership.Base2Permission.Admin | (long)ModuleRequestForMembership.Base2Permission.ManageRequests;

                    iResponse = Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
                }
                return iResponse || hasPortalPermission;
            }
            protected override Boolean AllowDownloadSubmittedFile(long idSubmittedFile, int idUser, litePerson person, int idCommunity, int idRole)
            {
                bool hasPortalPermission = false;
                if (idCommunity == 0)
                {
                    ModuleRequestForMembership m = ModuleRequestForMembership.CreatePortalmodule(person.TypeID);
                    hasPortalPermission = m.Administration || m.ManageBaseForPapers || m.EditBaseForPaper;
                }

                Boolean iResponse = false;
                iResponse = (from sf in Manager.GetAll<SubmittedFile>(sf => sf.Id == idSubmittedFile &&
                             (sf.CreatedBy == person || sf.Submission.CreatedBy == person || sf.Submission.Owner == person))
                             select sf.Id).Any();
                if (!iResponse)
                {
                    long permission = (long)ModuleRequestForMembership.Base2Permission.Admin | (long)ModuleRequestForMembership.Base2Permission.ManageRequests;

                    iResponse = Manager.HasModulePermission(idUser, idRole, idCommunity, this.ServiceModuleID(), permission);
                }
                return iResponse || hasPortalPermission;
            }
        #endregion

        #region "Request Presentation Methods"
            public List<dtoRequestItemPermission> GetRequests(ModuleRequestForMembership module, CallStandardAction action, Boolean fromAllcommunities, Boolean forPortal, int idCommunity, int idPerson, CallStatusForSubmitters status, FilterCallVisibility filter, int pageIndex, int pageSize)
            {
                liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                litePerson person = Manager.GetLitePerson(idPerson);
                return GetRequests(module, action, fromAllcommunities, forPortal, community, person, status, filter, pageIndex, pageSize);
            }
            public List<dtoRequestItemPermission> GetRequests(ModuleRequestForMembership module, CallStandardAction action, Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, litePerson person, CallStatusForSubmitters status, FilterCallVisibility filter, int pageIndex, int pageSize)
            {
                List<dtoRequestItemPermission> items = null;
                if (action == CallStandardAction.Manage)
                    items = GetCalls(module, forPortal, community, person, status, filter, pageIndex, pageSize);
                else
                    items = GetCalls(fromAllcommunities, forPortal, community, person, status, filter, pageIndex, pageSize);
                return items;
            }

            protected List<dtoRequestItemPermission> GetCalls(Boolean fromAllcommunities, Boolean forPortal, liteCommunity community, litePerson person, CallStatusForSubmitters status, FilterCallVisibility filter, int pageIndex, int pageSize)
            {
                List<dtoRequestItemPermission> items = null;
                try
                {
                    DateTime currentTime = DateTime.Now;
                    Boolean isAnonymous = (person.TypeID == (int)UserTypeStandard.Guest || person.TypeID == (int)UserTypeStandard.PublicUser);

                    var query = GetBaseUserSubmissionQuery(fromAllcommunities, forPortal, community, person, CallForPaperType.RequestForMembership, filter);
                    switch (status)
                    {
                        case CallStatusForSubmitters.Submitted:
                            var callSubmitted = query.Where(s => s.Status >= SubmissionStatus.submitted).Select(s => new { id = s.Call.Id, Name = s.Call.Name, EndDate = s.Call.EndDate }).ToList().Distinct().ToList();
                            List<long> idSubCalls = callSubmitted.OrderByDescending(c => c.EndDate).ThenBy(c => c.Name).Skip(pageIndex * pageSize).Take(pageSize).Select(c => c.id).ToList();

                            items = (from c in Manager.GetIQ<BaseForPaper>() where idSubCalls.Contains(c.Id) select c).OrderByDescending(c => c.EndDate).ThenBy(c => c.Name).ToList().Select(s =>
                                    new dtoRequestItemPermission(s.Id, s.Community, status,
                                    new dtoRequest()
                                    {
                                        Id = s.Id,
                                        Name = s.Name,
                                        Edition = s.Edition,
                                        Summary = s.Summary,
                                        Description = s.Description,
                                        StartDate = s.StartDate,
                                        EndDate = s.EndDate,
                                        SubmissionClosed = s.SubmissionClosed,
                                        Type = s.Type,
                                        Community = s.Community,
                                        IsPublic = s.IsPublic,
                                        IsPortal = s.IsPortal,
                                        Status = s.Status,
                                        AllowSubmissionExtension = s.UseStartCompilationDate
                                    }
                                    )).ToList();
                            break;
                        case CallStatusForSubmitters.SubmissionsLimitReached:
                        case CallStatusForSubmitters.SubmissionClosed:
                        case CallStatusForSubmitters.SubmissionOpened:
                            var queryCalls = GetBaseForPaperQuery(false,fromAllcommunities, forPortal, community, person, CallForPaperType.RequestForMembership, status, FilterCallVisibility.OnlyVisible);

                            switch (status)
                            {
                                case CallStatusForSubmitters.SubmissionsLimitReached:
                                case CallStatusForSubmitters.SubmissionClosed:
                                    queryCalls = queryCalls.OrderByDescending(c => c.EndDate).ThenBy(c => c.Name);
                                    break;
                                case CallStatusForSubmitters.SubmissionOpened:
                                    queryCalls = queryCalls.OrderByDescending(c => c.EndDate).ThenBy(c => c.Name);
                                    break;
                                default:
                                    queryCalls = queryCalls.OrderByDescending(c => c.CreatedOn).ThenBy(c => c.Name);
                                    break;
                            }
                            List<long> idSubmittedCalls = query.Where(s => s.Status >= SubmissionStatus.submitted).Select(s => s.Call.Id).Distinct().ToList();

                            if (idSubmittedCalls.Count > 0)
                                idSubmittedCalls = query.Where(s => s.Status < SubmissionStatus.submitted && !idSubmittedCalls.Contains(s.Call.Id)).Select(s => s.Call.Id).ToList();

                            var calls = queryCalls.Select(c => new { Id = c.Id, IsPublic = c.IsPublic, ForSubscribedUsers = c.ForSubscribedUsers }).ToList();

                            List<long> idCalls = GetIdCallsBySubmissionQuery(calls.Where(c => !c.IsPublic && !c.ForSubscribedUsers && !idSubmittedCalls.Contains(c.Id)).Select(c => c.Id).ToList(),
                                fromAllcommunities, forPortal, community, person, CallForPaperType.CallForBids, status, filter);

                            items = queryCalls.Where(c => (c.IsPublic || (c.ForSubscribedUsers && person != null && person.TypeID != (Int32)UserTypeStandard.Guest && person.TypeID != (Int32)UserTypeStandard.PublicUser)) || idCalls.Contains(c.Id)).Skip(pageIndex * pageSize).Take(pageSize).ToList().ToProxySafeList().Select(c =>
                                 new dtoRequestItemPermission(c.Id, c.Community, status,
                                 new dtoRequest()
                                 {
                                     Id = c.Id,
                                     Name = c.Name,
                                     Edition = c.Edition,
                                     Summary = c.Summary,
                                     Description = c.Description,
                                     StartDate = c.StartDate,
                                     EndDate = c.EndDate,
                                     SubmissionClosed = c.SubmissionClosed,
                                     Type = c.Type,
                                     Community = c.Community,
                                     IsPublic = c.IsPublic,
                                     IsPortal = c.IsPortal,
                                     ForSubscribedUsers = c.ForSubscribedUsers,
                                     Status = c.Status,
                                     AllowSubmissionExtension = c.UseStartCompilationDate,
                                     Deleted = c.Deleted
                                 }
                                 )).ToList();

                            break;
                    }
                    if (items.Count > 0)
                        items.ForEach(c => c.SubmissionsInfo = (from s in Manager.GetIQ<UserSubmission>()
                                                                where s.Type != null && s.Call != null && s.Call.Id == c.Id && s.Owner == person && s.Deleted == BaseStatusDeleted.None
                                                                select s).ToList().Select(
                                                            s => new dtoSubmissionDisplayInfo(s.Id, s.Revisions.Where(r=>r.Deleted== BaseStatusDeleted.None).ToList(), false) { IdSubmitterType = s.Type.Id, Status = s.Status, Owner = person, ModifiedOn = s.ModifiedOn, SubmittedBy = s.SubmittedBy, SubmittedOn = s.SubmittedOn, ExtensionDate = s.ExtensionDate, UniqueId = s.UserCode }).ToList());

                    
                    if (fromAllcommunities && items.Count > 0)
                    {
                        List<int> idCommunities = items.Where(c=>c.Community != null).Select(c => c.Community.Id).Distinct().ToList();
                        if (items.Where(c=> c.Call.IsPortal).Any()){
                            idCommunities.Insert(0,0);
                        }
                        Dictionary<int, ModuleRequestForMembership> permissions = GetCallCommunitiesPermission(idCommunities, person);
                        items.ForEach(i => i.RefreshUserPermission(permissions, person));
                    }
                    else if (items.Count > 0)
                        items.ForEach(i => i.RefreshUserPermission(RequestForMembershipServicePermission(person, (community == null) ? 0 : community.Id), person));
                    
                    items.Where(i => i.SubmissionsInfo != null & i.SubmissionsInfo.Count > 0).ToList().ForEach(i => i.AllowSubmissionAs = GetSubmissionAs(i.Id, i.SubmissionsInfo.Select(si => si.IdSubmitterType).Distinct().ToList(), person));
                }
                catch (Exception ex)
                {
                    items = new List<dtoRequestItemPermission>();
                }
                return items;
            }
            protected List<dtoRequestItemPermission> GetCalls(ModuleRequestForMembership module, Boolean forPortal, liteCommunity community, litePerson person, CallStatusForSubmitters status, FilterCallVisibility filter, int pageIndex, int pageSize)
            {
                List<dtoRequestItemPermission> items = null;
                try
                {
                    Boolean isAnonymous = (person.TypeID == (int)UserTypeStandard.Guest || person.TypeID == (int)UserTypeStandard.PublicUser);

                    switch (status)
                    {
                        case CallStatusForSubmitters.Draft:
                        case CallStatusForSubmitters.SubmissionClosed:
                        case CallStatusForSubmitters.SubmissionOpened:
                            var queryCall = GetBaseForPaperQuery(true,false, forPortal, community, person, CallForPaperType.RequestForMembership, status, filter);
                            switch (status)
                            {
                                case CallStatusForSubmitters.Draft:
                                    queryCall = queryCall.OrderByDescending(c => c.CreatedOn).ThenBy(c => c.Name);
                                    break;
                                case CallStatusForSubmitters.SubmissionClosed:
                                    queryCall = queryCall.OrderByDescending(c => c.EndDate).ThenBy(c => c.Name);
                                    break;
                                case CallStatusForSubmitters.SubmissionOpened:
                                    queryCall = queryCall.OrderByDescending(c => c.EndDate).ThenBy(c => c.Name);
                                    break;
                                default:
                                    queryCall = queryCall.OrderByDescending(c => c.CreatedOn).ThenBy(c => c.Name);
                                    break;
                            }
                            items = queryCall.Select(c =>
                                     new dtoRequestItemPermission(c.Id, c.Community, status,
                                     new dtoRequest()
                                     {
                                         Id = c.Id,
                                         Name = c.Name,
                                         Edition = c.Edition,
                                         Summary = c.Summary,
                                         Description = c.Description,
                                         StartDate = c.StartDate,
                                         EndDate = c.EndDate,
                                         SubmissionClosed = c.SubmissionClosed,
                                         Type = c.Type,
                                         Community = c.Community,
                                         IsPublic = c.IsPublic,
                                         Status = c.Status,
                                         AllowSubmissionExtension = c.UseStartCompilationDate,
                                         Owner = c.CreatedBy,
                                         Deleted = c.Deleted
                                     }
                                     )).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                            break;
                    }
                    items.ForEach(i => i.RefreshPermission(module, person, (from s in Manager.GetIQ<UserSubmission>()
                                                                            where s.Deleted == BaseStatusDeleted.None && s.Call.Id == i.Id
                                                                            select s.Id).Count(), (from s in Manager.GetIQ<UserSubmission>()
                                                                                                   where s.Deleted == BaseStatusDeleted.None && s.Status<= SubmissionStatus.submitted && s.Call.Id == i.Id
                                                                                                   select s.Id).Count()));
                }
                catch (Exception ex)
                {
                    items = new List<dtoRequestItemPermission>();
                }
                return items;
            }

            private Dictionary<int, ModuleRequestForMembership> GetCallCommunitiesPermission(List<int> idCommunities, litePerson person)
            {
                Dictionary<int, ModuleRequestForMembership> permissions = new Dictionary<int, ModuleRequestForMembership>();
                if (idCommunities.Contains(0))
                    permissions[0] = RequestForMembershipServicePermission(person, 0);

                idCommunities.Where(id => id > 0).Distinct().ToList().ForEach(i => permissions.Add(i, RequestForMembershipServicePermission(person, i)));

                return permissions;
            }
            public long CallCountBySubmission(Boolean forAdministrationMode, Boolean fromAllcommunities, Boolean forPortal, int idCommunity, int idUser, CallStatusForSubmitters status)
            {
                try
                {
                    liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                    litePerson person = Manager.GetLitePerson(idUser);
                    return CallCountBySubmission(forAdministrationMode,fromAllcommunities, forPortal, community, person, CallForPaperType.RequestForMembership, status);
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

            public long PublicCallCount(Boolean forAdministrationMode, Boolean fromAllcommunities, Boolean forPortal, int idCommunity, CallStatusForSubmitters status)
            {
                try
                {
                    liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                    litePerson person = (from p in Manager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
                    return CallCountBySubmission(forAdministrationMode,fromAllcommunities, forPortal, community, person, CallForPaperType.RequestForMembership, status);
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        #endregion
        #region "Call"
            public dtoRequest GetDtoRequest(long idCall)
            {
                dtoRequest call = null;
                try
                {
                    call = (from c in Manager.GetAll<RequestForMembership>(c=>c.Id == idCall)
                            select new dtoRequest()
                            {
                                AllowSubmissionExtension = c.UseStartCompilationDate,
                                Deleted = c.Deleted,
                                Description = c.Description,
                                Edition = c.Edition,
                                EndDate = c.EndDate,
                                Id = c.Id,
                                SubmissionClosed = c.SubmissionClosed,
                                IsPortal = c.IsPortal,
                                IsPublic = c.IsPublic,
                                ForSubscribedUsers = c.ForSubscribedUsers,
                                Name = c.Name,
                                RevisionSettings = c.RevisionSettings,
                                AcceptRefusePolicy = c.AcceptRefusePolicy,
                                OverrideHours = c.OverrideHours,
                                OverrideMinutes = c.OverrideMinutes,
                                StartDate = c.StartDate,
                                Status = c.Status,
                                Summary = c.Summary,
                                StartMessage= c.StartMessage,
                                EndMessage = c.EndMessage,
                                Type = c.Type,
                                Community = c.Community,
                                Owner = c.CreatedBy
                            }).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch (Exception ex)
                {

                }
                return call;
            }
            public RequestForMembership SaveCallSettings(dtoRequest dto, int idCommunity, Boolean validate, String submitterName)
            {
                RequestForMembership call = null;
                try
                {
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (dto != null)
                    {
                        Manager.BeginTransaction();
                        call = Manager.Get<RequestForMembership>(dto.Id);
                        if (call == null)
                        {
                            call = new RequestForMembership();
                            call.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            if (idCommunity == 0)
                            {
                                call.IsPortal = true;
                                call.IsPublic = true;
                            }
                            else
                                call.Community = Manager.GetLiteCommunity(idCommunity);
                            call.Type = CallForPaperType.RequestForMembership;
                            call.Status = CallForPaperStatus.Draft;
                            call.StartMessage = "--";
                            call.EndMessage = "--";
                        }
                        else
                            call.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        call.UseStartCompilationDate = dto.AllowSubmissionExtension;
                        call.Description = dto.Description;
                        call.Summary = dto.Summary;
                        call.Edition = dto.Edition;
                        call.Name = dto.Name;
                        call.RevisionSettings = dto.RevisionSettings;
                        call.AcceptRefusePolicy = dto.AcceptRefusePolicy;
                        call.OverrideHours = dto.OverrideHours;
                        call.OverrideMinutes = dto.OverrideMinutes;
                        call.StartDate = dto.StartDate;

                        call.AllowPrintDraft = dto.AllowDraft;
                        call.AttachSign = dto.AttachSign;

                        if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
                        {
                            call.StartDate = dto.EndDate.Value;
                            call.EndDate = dto.StartDate;
                        }
                        else
                            call.EndDate = dto.EndDate;
                        call.SubmissionClosed = dto.SubmissionClosed;
                        if (!validate)
                            call.Status = dto.Status;
                        Manager.SaveOrUpdate(call);

                        if (dto.Id==0 && !String.IsNullOrEmpty(submitterName))
                        {
                            SubmitterType submitter = new SubmitterType();
                            submitter.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                            submitter.Name = submitterName;
                            submitter.AllowMultipleSubmissions = false;
                            submitter.Call = call;
                            submitter.Description = "";
                            submitter.DisplayOrder = 1;
                            submitter.MaxMultipleSubmissions = 1;
                            Manager.SaveOrUpdate(submitter);
                        }
                        

                        Manager.Commit();
                        if (validate && ValidateStatus(call, dto.Status))
                        {
                            Manager.BeginTransaction();
                            call.Status = dto.Status;
                            Manager.SaveOrUpdate(call);
                            if (dto.Id == 0 && call.Community != null)
                            {
                                BaseForPaperCommunityAssignment assignment = new BaseForPaperCommunityAssignment();
                                assignment.Deleted = BaseStatusDeleted.None;
                                assignment.Deny = false;
                                assignment.AssignedTo = call.Community;
                                assignment.BaseForPaper = call;
                                assignment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                                Manager.SaveOrUpdate(assignment);
                            }
                            Manager.Commit();
                        }
                        else if (validate)
                            throw new CallForPaperInvalidStatus();
                    }
                }
                catch (CallForPaperInvalidStatus ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return call;
            }
            public dtoRequest GetRequestMessages(long idCall)
            {
                dtoRequest call = null;
                try
                {
                    call = (from c in Manager.GetAll<RequestForMembership>(c => c.Id == idCall)
                            select new dtoRequest()
                            {
                                Id = c.Id,
                                IsPortal = c.IsPortal,
                                IsPublic = c.IsPublic,
                                ForSubscribedUsers = c.ForSubscribedUsers,
                                Name = c.Name,
                                RevisionSettings = c.RevisionSettings,
                                AcceptRefusePolicy = c.AcceptRefusePolicy,
                                StartMessage = c.StartMessage,
                                EndMessage = c.EndMessage,
                                Type = c.Type,
                                Community = c.Community,
                                Owner = c.CreatedBy
                            }).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch (Exception ex)
                {

                }
                return call;
            }
            public void GetRequestMessages(long idCall, ref String startMessage, ref String endMessage)
            {
                try
                {
                    var query = (from r in Manager.GetIQ<RequestForMembership>()
                                 where r.Id == idCall
                                 select new { StartMessage = r.StartMessage, EndMessage = r.EndMessage }).Skip(0).Take(1).ToList();
                    if (query.Count == 1)
                    {
                        startMessage = query.Select(r => r.StartMessage).FirstOrDefault();
                        endMessage = query.Select(r => r.EndMessage).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    startMessage = "--";
                    endMessage = "--";
                }
            }
            public Boolean SaveRequestMessages(long idCall, String startMessage, String endMessage)
            {
                Boolean result = false;
                try
                {
                    Manager.BeginTransaction();
                    RequestForMembership call = Manager.Get<RequestForMembership>(idCall);
                    litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                    if (call != null && person != null)
                    {
                        call.StartMessage = startMessage;
                        call.EndMessage = endMessage;
                        call.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate(call);
                        result = true;
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    result = false;
                }
                return result;
            }
        #endregion

        protected override bool AllowDownloadVerbali(long idCommission, int idUser, lm.Comol.Core.DomainModel.litePerson person, int idCommunity, int idRole)
        {
            return false;
        }

        protected override bool AllowDownloadIntegrazioni(long idIntegration, int idUser, litePerson person, int idCommunity, int idRole)
        {
            return false;
        }
    }
}
