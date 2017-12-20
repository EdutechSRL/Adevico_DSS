using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewGenericItemsSelector : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean isInitialized { get; set; }
        ExternalResource SelectedItems { get; }
        Boolean HasSelectedItems { get; }
        Int32 ItemsToSelect { get; }
        Int32 SelectedItemsCount { get; }
        List<Int32> SelectedRows { get; set; }
        List<ExternalColumn> Columns { get; set; }
        void InitializeControl(ExternalResource source, List<ExternalColumnComparer<String, Int32>> noDBfields, List<Int32> notEmptyColumns, List<Int32> notDuplicatedColumns);
        void InitializeControlAfterImport(ExternalResource source, List<ExternalColumnComparer<String, Int32>> noDBfields, List<Int32> notEmptyColumns, List<Int32> notDuplicatedColumns);


        void InitializeControl(ExternalResource source, List<ExternalColumnComparer<String, Int32>> noDBfields, List<ExternalColumnComparer<String, Int32>> notEmptyColumns, List<ExternalColumnComparer<String, Int32>> notDuplicatedColumns);
        void InitializeControlAfterImport(ExternalResource source, List<ExternalColumnComparer<String, Int32>> noDBfields, List<ExternalColumnComparer<String, Int32>> notEmptyColumns, List<ExternalColumnComparer<String, Int32>> notDuplicatedColumns);


        void LoadItems(List<ExternalResource> items, List<InvalidImport> invalidItems);
        void DisplaySessionTimeout();
        List<ExternalCell> ValidateDBItems(DestinationItem<Int32> item, List<ExternalCell> cells);
        List<ExternalCell> ValidateAlternateDBItems(List<DestinationItemCells<Int32>> item);
    }
}