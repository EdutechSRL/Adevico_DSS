
using System;
using System.Collections.Generic;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
	public class ItemPermission
	{
        public virtual Boolean UnDelete { get; set; }
        public virtual Boolean Delete { get; set; }
		public virtual Boolean Download { get; set; }
		public virtual Boolean VirtualDelete { get; set; }
		public virtual Boolean Link  { get; set; }
		public virtual Boolean Play  { get; set; }
        public virtual Boolean ViewMyStatistics { get; set; }
        public virtual Boolean ViewOtherStatistics { get; set; }
        public virtual Boolean AllowSelection  { get; set; }
		public virtual Boolean ViewPermission { get; set; }
		public virtual Boolean Edit  { get; set; }
        public virtual Boolean EditPermission { get; set; }
        public virtual Boolean EditSettings { get; set; }
        public virtual Boolean CanEditSettings { get; set; }

        public virtual Boolean Zip { get; set; }
        public virtual Boolean Unzip { get; set; }
        public virtual Boolean VersioningAvailable { get; set; }
        public virtual Boolean Preview { get; set; }
        public virtual Boolean Move { get; set; }
        public virtual Boolean AddVersion { get; set; }
        public virtual Boolean AddVersionFromModule { get; set; }
        public virtual Boolean RemoveVersion { get; set; }
        public virtual Boolean SetVersion { get; set; }
        public virtual Boolean Hide { get; set; }
        public virtual Boolean Show { get; set; }
        public virtual Boolean AllowUpload { get; set; }
		public ItemPermission()
		{
		}

        public List<ItemAction> GetActions()
        {
            List<ItemAction> actions = new List<ItemAction>();
            if (UnDelete)
                actions.Add(ItemAction.undelete);
            if (Delete)
                actions.Add(ItemAction.delete);
            if (Download)
                actions.Add(ItemAction.download);
            if (VirtualDelete)
                actions.Add(ItemAction.virtualdelete);
            if (Link)
                actions.Add(ItemAction.link);
            if (Play)
                actions.Add(ItemAction.play);
            if (Play || Download || Edit )
                actions.Add(ItemAction.details);
            if (ViewMyStatistics)
                actions.Add(ItemAction.viewMyStatistics);
            if (ViewOtherStatistics)
                actions.Add(ItemAction.viewOtherStatistics);
            if (Edit)
                actions.Add(ItemAction.edit);
            if (EditPermission)
                actions.Add(ItemAction.editPermission);
            if (EditSettings)
                actions.Add(ItemAction.editSettings);
            if (Preview)
                actions.Add(ItemAction.preview);
            if (Move)
                actions.Add(ItemAction.move);
            if (AddVersion)
                actions.Add(ItemAction.addVersion);
            if (RemoveVersion)
                actions.Add(ItemAction.removeVersion);
            if (SetVersion)
                actions.Add(ItemAction.setCurrentVersion);
            if (Hide)
                actions.Add(ItemAction.hide);
            if (Show)
                actions.Add(ItemAction.show);
            if (AllowUpload)
            {
                actions.Add(ItemAction.addfolder);
                actions.Add(ItemAction.addlink);
                actions.Add(ItemAction.upload);
            }
            return actions;
        }
        public List<ItemAction> GetMultipleActions()
        {
            List<ItemAction> actions = new List<ItemAction>();
            if (UnDelete)
                actions.Add(ItemAction.undelete);
            if (Delete)
                actions.Add(ItemAction.delete);
            if (Download)
                actions.Add(ItemAction.download);
            if (VirtualDelete)
                actions.Add(ItemAction.virtualdelete);
            if (Move)
                actions.Add(ItemAction.move);
            if (Hide)
                actions.Add(ItemAction.hide);
            if (Show)
                actions.Add(ItemAction.show);
            return actions;
        }
	}
}