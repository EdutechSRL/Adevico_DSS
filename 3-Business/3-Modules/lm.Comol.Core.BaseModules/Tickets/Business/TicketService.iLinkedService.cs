using System;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel;

using lm.Comol.Core.Business;

using NHibernate.Mapping;

using System.Linq;


using TK = lm.Comol.Core.BaseModules.Tickets.Domain;


//DEBUG
//using System.Diagnostics;

namespace lm.Comol.Core.BaseModules.Tickets
{
	public partial class TicketService : CoreServices, iLinkedService
	{

	#region iLinkedService Members
		#region Not Implemented - By PM
        public void SaveActionExecution(ModuleLink link, bool isStarted, bool isPassed, short Completion, bool isCompleted, short mark, int idUser, bool alreadyCompleted, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
			{
			}
			public void SaveActionsExecution(List<dtoItemEvaluation<ModuleLink>> evaluatedLinks, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
			{
			}
			public List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<ModuleLink> links, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
			{
				return new List<dtoItemEvaluation<long>>();
			}
			public dtoEvaluation EvaluateModuleLink(ModuleLink link, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
			{
				return null;
			}
			public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(long objectId, int objectTypeId, Dictionary<int, string> translations, int idCommunity, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
			{
				return null;
			}
		#endregion

		// VERIFICARE: QUALI SONO LE ACTION CHE SERVONO IN GENERALE E QUALI SONO QUELLE CHE SERVONO AI TICKET NELLO SPECIFICO?
		public List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, int idUser, int idRole, int idCommunity, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
		{
			TK.TicketUser usr;

			List<StandardActionType> actions = new List<StandardActionType>();

			//Verificare, eventualmente aggiungere controllo sul tipo.
			TK.Message msg = Manager.Get<TK.Message>(source.ObjectLongID);

			if (moduleUserLong != null && moduleUserLong.ContainsKey(ModuleTicket.UniqueCode))
			{
				usr = this.UserGet(moduleUserLong[ModuleTicket.UniqueCode]);
			}
			else
			{
				litePerson person = Manager.Get<litePerson>(idUser);
				usr = this.UserGetfromPerson(person.Id);
			}

			

			TK.TicketFile attach = (from TK.TicketFile a in msg.Attachments
                                    where a.Item != null && a.Item.Id == destination.ObjectLongID
									select a).Skip(0).Take(1).ToList().FirstOrDefault();
			
			//CONTROLLARE: in teoria SOLO x download...
			if (idCommunity <= 0)
				idCommunity = destination.CommunityID;


			//---------------         Begin DEBUG        ------------------
			//Debug.WriteLine("IdUser: " + idUser.ToString());
			//Debug.WriteLine("idRole: " + idRole.ToString());
			//Debug.WriteLine("idCommunity: " + idCommunity.ToString());
			//if(source != null)
			//{
			//    Debug.WriteLine("source - Com ID: " + source.CommunityID.ToString());
			//    Debug.WriteLine("source - Obj ID: " + source.ObjectLongID.ToString());
			//    Debug.WriteLine("source - Obj Type: " + source.ObjectTypeID.ToString());
			//}
			//else
			//{
			//    Debug.WriteLine("source: NULL!");
			//}
			//if (destination != null)
			//{
			//    Debug.WriteLine("destination - Com Id: " + destination.CommunityID.ToString());
			//    Debug.WriteLine("destination - Obj ID: " + destination.ObjectLongID.ToString());
			//    Debug.WriteLine("destination - Obj Type: " + destination.ObjectTypeID.ToString());
			//}
			//else
			//{
			//    Debug.WriteLine("destination: NULL!");
			//}
			//if(person == null)
			//    Debug.WriteLine("person: NULL");
			//if (usr == null)
			//    Debug.WriteLine("usr: NULL");
			//if(msg == null)
			//    Debug.WriteLine("msg: ");
			//else
			//{
			//    if(msg.Attachments == null)
			//        Debug.WriteLine("msg: Attachments = NULL!" );
			//    else
			//    {
			//        Debug.WriteLine("msg: Attachments = " + msg.Attachments.Count());
			//        foreach(TK.TicketFile fl in msg.Attachments)
			//        {
			//            Debug.WriteLine("msg - Attachments TkFile: " + fl.Id + " - " + fl.Name);
			//            if(fl.File != null)
			//            {
			//                Debug.WriteLine("msg - Attachments File: " + fl.File.Id + " - " + fl.File.Name);
			//            }
			//            Debug.WriteLine("");
			//        }
			//    }
			//}
			//if (attach == null)
			//    Debug.WriteLine("attach: NULL");


			//---------------         END DEBUG        ------------------


			//SE non esiste messaggio, utente o allegati: non ho permessi.
			if(msg == null || usr == null || attach == null)
				return actions;

			//Se sono PROPRIETARIO del messaggio: ho TUTTI i permessi!
			if(msg.Creator != null && msg.Creator.Id == usr.Id)
			{
				//SE il messaggio è in DRAFT, posso anche cancellare il file!
				if(msg.IsDraft)
				{
					actions.Add(StandardActionType.Delete);
				}

				//SONO creatore del messaggio in DRAFT, comunque ho TUTTI i permessi.
				actions.Add(StandardActionType.EditMetadata);
				actions.Add(StandardActionType.Play);
				actions.Add(StandardActionType.DownloadItem);
				actions.Add(StandardActionType.ViewPersonalStatistics);
				actions.Add(StandardActionType.ViewAdvancedStatistics);

				actions.Add(StandardActionType.ViewDescription);
				actions.Add(StandardActionType.ViewPreview);

				//return actions;
			}
			else if (!msg.IsDraft && usr.Person != null && msg.Ticket.CreatedBy.Id == usr.Person.Id)
				//SONO il CREATORE del Ticket
			{
				actions.Add(StandardActionType.EditMetadata);
				actions.Add(StandardActionType.Play);
				actions.Add(StandardActionType.DownloadItem);
				actions.Add(StandardActionType.ViewPersonalStatistics);
				actions.Add(StandardActionType.ViewAdvancedStatistics);

				actions.Add(StandardActionType.ViewDescription);
				actions.Add(StandardActionType.ViewPreview);
			}
		   else if (!msg.IsDraft && this.UserHasManResTicketPermission(msg.Ticket.Id, usr.Id))
				//SONO un manager/REsolver del Ticket
			{
				actions.Add(StandardActionType.EditMetadata);
				actions.Add(StandardActionType.Play);
				actions.Add(StandardActionType.DownloadItem);
				actions.Add(StandardActionType.ViewPersonalStatistics);
				actions.Add(StandardActionType.ViewAdvancedStatistics);

				actions.Add(StandardActionType.ViewDescription);
				actions.Add(StandardActionType.ViewPreview);
			}

			if (actions.Count() <= 0 && (msg.Ticket.Owner.Id == usr.Id && msg.Visibility && attach.Visibility == TK.Enums.FileVisibility.visible))
			{
				//Se sono MANAGER/RESOLVER ho tutti i permessi

				//SE sono associato al Ticket o ad una categoria associata al ticket (sono manager o resolver del ticket),
				//ho tutti i permessi sul file allegato al messaggio?
				//actions.Add(StandardActionType.EditMetadata);
				actions.Add(StandardActionType.Play);
				actions.Add(StandardActionType.DownloadItem);
				//actions.Add(StandardActionType.ViewPersonalStatistics);
				//actions.Add(StandardActionType.ViewAdvancedStatistics);

				actions.Add(StandardActionType.ViewDescription);
				actions.Add(StandardActionType.ViewPreview);
			}
			
			//else 
			//{
			//    //Non sono MANAGER/RESOLVER, nè proprietario del messaggio: non ho permessi!!!
			//    //if(msg.Visibility == true && attach.Visibility == TK.Enums.FileVisibility.visible)
			//    //{
			//    //    //Il messaggio ED il FILE sono VISIBILI!

			//    //    //actions.Add(StandardActionType.EditMetadata);
			//    //    //actions.Add(StandardActionType.Play);
			//    //    //actions.Add(StandardActionType.DownloadItem);
			//    //    //actions.Add(StandardActionType.ViewPersonalStatistics);
			//    //    //actions.Add(StandardActionType.ViewAdvancedStatistics);

			//    //    //actions.Add(StandardActionType.ViewDescription);
			//    //    //actions.Add(StandardActionType.ViewPreview);
			//    //}
			//}
			
			return actions;
		}

		// VERIFICARE... In particolare Id Community, che comunque non uso...
		public bool AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, int idUser, int idRole, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
		{
			List<StandardActionType> actions = GetAllowedStandardAction(source, destination, idUser, idRole, -1, moduleUserLong, moduleUserString);
			return actions.Contains(actionType);
		}

		//VERIFICARE...
		public bool AllowActionExecution(ModuleLink link, int idUser, int idCommunity, int idRole, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
		{
			//In pratica "se" ho permesso di "play"?
			//In tal caso dovrebbe essere ok.
			List<StandardActionType> actions = GetAllowedStandardAction(link.SourceItem, link.DestinationItem, idUser, idRole, -1, moduleUserLong, moduleUserString);
			return actions.Contains(StandardActionType.Play);
		}

	#region "IgnoreItems- Delete Community"
        public void PhisicalDeleteCommunity(Int32 idCommunity, Int32 idUser, String baseFilePath, String baseThumbnailPath)
		{
		}
		public void PhisicalDeleteRepositoryItem(long idFileItem, int idCommunity, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
		{
		}
	#endregion

		#endregion

		#region Upload Action
		/// <summary>
		/// Restituisce le ActionType
		/// </summary>
		/// <param name="UserType">Tipo utente. Se diverso da creatore, controllo permessi per file comunità</param>
		/// <param name="idCommunity">= 0 Se creatore o portale</param>
		/// <param name="idPerson">= 0 se creatore o portale</param>
		/// <param name="repositoryPermissions">Repository permission: if NULL = no community permission</param>
		/// <returns></returns>
		/// <remarks>
		/// Eventuamente modificare i riferimenti per avere Id utente e comnità reali.
		/// </remarks>
		public List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> UploadAvailableActionsGet(Domain.Enums.MessageUserType UserType, int idCommunity, int idPerson, lm.Comol.Core.FileRepository.Domain.ModuleRepository repositoryPermissions)
		{
			List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions = new List<DomainModel.Repository.RepositoryAttachmentUploadActions>();

			//UserType == TK.Enums.MessageUserType.Partecipant ||

			if(idCommunity <= 0 || idPerson <= 0)
                actions.Add(DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem);
            else
			{
                actions.Add(DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem);

                if (repositoryPermissions != null)
				{
                    if ( repositoryPermissions.Administration || repositoryPermissions.ManageItems || repositoryPermissions.ViewItemsList)
                        actions.Add(DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity);
                    if (repositoryPermissions.UploadFile || repositoryPermissions.Administration || repositoryPermissions.ManageItems)
                        actions.Add(DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity);
				}
			}
            return actions;
		}

		#endregion


	   
	}
}
