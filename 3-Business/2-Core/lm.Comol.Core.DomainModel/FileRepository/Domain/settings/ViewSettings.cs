using lm.Comol.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class ViewSettings : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual RepositoryContextSettings Settings { get; set; }
        public virtual Boolean Enabled { get; set; }
        public virtual TreeViewOption Tree { get; set; }
        public virtual ViewOption AvailableOptions { get; set; }
        public virtual ViewOption ActiveOptions { get; set; }
        public virtual PresetType Type { get; set; }
        public virtual Boolean FullWidth { get; set; }


        public ViewSettings()
        {
            Enabled = true;
            Type = PresetType.Standard;
            Tree = GetDefaultTree(Type);
            AvailableOptions = DefaultAvaliableOptions(Type);
            ActiveOptions = DefaultActiveOptions(Type);
        }

        public static List<ViewOption> GetListOfDefaultAvaliableOptions(PresetType type)
        {
            return FromFlagToList(DefaultAvaliableOptions(type));
        }
        public static List<ViewOption> GetListOfDefaultActiveOptions(PresetType type)
        {
            return FromFlagToList(DefaultActiveOptions(type));
        }
        public static ViewOption DefaultAvaliableOptions(PresetType type)
        {
            ViewOption result = ViewOption.FolderPath | ViewOption.NarrowWideView | ViewOption.AvailableSpace;

            switch (type)
            {
                case PresetType.Simple:
                    result |= ViewOption.Date | ViewOption.Extrainfo;
                    break;
                case PresetType.Standard:
                    result |= ViewOption.Tree | ViewOption.Date  | ViewOption.Extrainfo;
                    break;
                case PresetType.Advanced:
                    result |= ViewOption.Tree | ViewOption.Date | ViewOption.Statistics | ViewOption.Extrainfo;
                    break;
            }

            return result;
        }

        public static ViewOption DefaultActiveOptions(PresetType type)
        {
            ViewOption result = ViewOption.FolderPath | ViewOption.NarrowWideView | ViewOption.AvailableSpace;

            switch (type)
            {
                case PresetType.Simple:
                    result |= ViewOption.Extrainfo;
                    break;
                case PresetType.Standard:
                    result |= ViewOption.Tree | ViewOption.Date  | ViewOption.Extrainfo;
                    break;
                case PresetType.Advanced:
                    result |= ViewOption.Tree | ViewOption.Date | ViewOption.Statistics | ViewOption.Extrainfo;
                    break;
            }

            return result;
        }
        public static TreeViewOption GetDefaultTree(PresetType type)
        {
            switch (type)
            {
                case PresetType.Simple:
                    return TreeViewOption.None;
                case PresetType.Standard:
                    return TreeViewOption.OnlyWithFolders;
                case PresetType.Advanced:
                    return TreeViewOption.OnlyWithFolders;
                default:
                    return TreeViewOption.None;
            }
        }
        public static ViewOption FromListToFlag(List<ViewOption> options)
        {
            ViewOption result = ViewOption.None;
            if (options != null && options.Any())
                options.ForEach(o => result = result | o);
            return result;
        }
        public static List<ViewOption> FromFlagToList(ViewOption options)
        {
            List<ViewOption> result = new List<ViewOption>();
            if (options != ViewOption.None)
                result = (from o in Enum.GetValues(typeof(ViewOption)).Cast<ViewOption>() where o!= ViewOption.None && (o & options)== o select o).ToList();
            return result;
        }
    }
}