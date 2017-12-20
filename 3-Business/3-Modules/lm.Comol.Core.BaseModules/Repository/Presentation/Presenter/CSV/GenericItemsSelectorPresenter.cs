using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class GenericItemsSelectorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private int _ModuleID;
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewGenericItemsSelector View
        {
            get { return (IViewGenericItemsSelector)base.View; }
        }
       
        public GenericItemsSelectorPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public GenericItemsSelectorPresenter(iApplicationContext oContext, IViewGenericItemsSelector view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(ExternalResource source, List<ExternalColumnComparer<String, Int32>> noDBfields, List<Int32> notEmptyColumns, List<Int32> notDuplicatedColumns)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.Columns = source.ColumHeader;
                AnalyzeItems(source, noDBfields, notEmptyColumns, notDuplicatedColumns);
                List<ExternalResource> items = new List<ExternalResource>();
                items.Add(source);

                View.LoadItems(items, GetInvalidItems(source));
            }
        }
        public void AnalyzeItems(ExternalResource source, List<ExternalColumnComparer<String, Int32>> noDBfields, List<Int32> notEmptyColumns, List<Int32> notDuplicatedColumns)
        {
            if (noDBfields.Count > 0)
            {
                foreach (ExternalColumnComparer<String, Int32> field in noDBfields)
                {
                    List<ExternalCell> invalidItems = View.ValidateDBItems(field.DestinationColumn, source.GetColumnCells(field.DestinationColumn.InputType, field.Number));
                    invalidItems.ForEach(i => i.isDBduplicate = true);
                }
            }
            source.ValidateSourceData(notEmptyColumns, notDuplicatedColumns);
        }
        //public void AnalyzeItems(ExternalResource source, List<ExternalColumnComparer<String, Int32>> dbValidateFields, List<Int32> notEmptyColumns, List<Int32> notDuplicatedColumns)
        //{
        //    if (dbValidateFields.Count > 0)
        //    {
        //        List<Int32> validatedItems = new List<Int32>();
        //        foreach (ExternalColumnComparer<String, Int32> dbValidate in dbValidateFields)
        //        {
        //            List<ExternalCell> invalidItems = null;
        //            //if (fields.Where(f=> dbValidate.DestinationColumn.Id == f.Id && f.HasAlternative))
        //            if (dbValidate.DestinationColumn.HasAlternative && !validatedItems.Contains(dbValidate.DestinationColumn.Id))
        //            {
        //                List<DestinationItemCells<Int32>> items = new List<DestinationItemCells<Int32>>();
        //                items.Add(new DestinationItemCells<Int32>(dbValidate.DestinationColumn) { Cells = source.GetColumnCells(dbValidate.DestinationColumn.InputType, dbValidate.Number) });
        //                foreach (Int32 index in dbValidate.DestinationColumn.AlternativeAttributes)
        //                {
        //                    if (dbValidateFields.Where(f => f.DestinationColumn.Id == index).Any())
        //                        items.Add(new DestinationItemCells<Int32>(dbValidateFields.Where(f => f.DestinationColumn.Id == index).Select(f => f.DestinationColumn).FirstOrDefault()) { Cells = source.GetColumnCells(dbValidateFields.Where(f => f.DestinationColumn.Id == index).Select(f => f.DestinationColumn).FirstOrDefault().InputType, dbValidateFields.Where(f => f.DestinationColumn.Id == index).Select(f => f.Number).FirstOrDefault()) });
        //                }
        //                validatedItems.Add(dbValidate.DestinationColumn.Id);
        //                validatedItems.AddRange(dbValidate.DestinationColumn.AlternativeAttributes);
        //                invalidItems = View.ValidateAlternateDBItems(items);
        //            }
        //            else if (!validatedItems.Contains(dbValidate.DestinationColumn.Id))
        //            {
        //                invalidItems = View.ValidateDBItems(dbValidate.DestinationColumn, source.GetColumnCells(dbValidate.DestinationColumn.InputType, dbValidate.Number));
        //                validatedItems.Add(dbValidate.DestinationColumn.Id);
        //            }
        //            invalidItems.ForEach(i => i.isDBduplicate = true);
        //        }
        //        //foreach (ExternalColumnComparer<String, Int32> field in noDBfields)
        //        //{
        //        //    List<ExternalCell> invalidItems = View.ValidateDBItems(field.DestinationColumn, source.GetColumnCells(field.DestinationColumn.InputType, field.Number));
        //        //    invalidItems.ForEach(i => i.isDBduplicate = true);
        //        //}
        //    }
        //    source.ValidateSourceData(notEmptyColumns, notDuplicatedColumns);
        //}


        public void InitView(ExternalResource source, List<ExternalColumnComparer<String, Int32>> noDBfields,List<ExternalColumnComparer<String, Int32>> notEmptyColumns, List<ExternalColumnComparer<String, Int32>> notDuplicatedColumns)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.Columns = source.ColumHeader;
                AnalyzeItems(source, noDBfields, notEmptyColumns, notDuplicatedColumns);
                List<ExternalResource> items = new List<ExternalResource>();
                items.Add(source);

                View.LoadItems(items, GetInvalidItems(source));
            }
        }
        public void AnalyzeItems(ExternalResource source, List<ExternalColumnComparer<String, Int32>> DBvalidateFields,List<ExternalColumnComparer<String, Int32>> notEmptyColumns, List<ExternalColumnComparer<String, Int32>> notDuplicatedColumns)
        {
            if (DBvalidateFields.Count > 0)
            {
                List<Int32> validatedItems = new List<Int32>();
                foreach (ExternalColumnComparer<String, Int32> dbValidate in DBvalidateFields)
                {
                    List<ExternalCell> invalidItems = null;
                    //if (fields.Where(f=> dbValidate.DestinationColumn.Id == f.Id && f.HasAlternative))
                    if (dbValidate.DestinationColumn.HasAlternative && !validatedItems.Contains(dbValidate.DestinationColumn.Id))
                    {
                        List<DestinationItemCells<Int32>> items = new List<DestinationItemCells<Int32>>();
                        items.Add(new DestinationItemCells<Int32>(dbValidate.DestinationColumn) {  Cells= source.GetColumnCells(dbValidate.DestinationColumn.InputType, dbValidate.Number)});
                        foreach (Int32 index in dbValidate.DestinationColumn.AlternativeAttributes) {
                            if (DBvalidateFields.Where(f => f.DestinationColumn.Id == index).Any())
                                items.Add(new DestinationItemCells<Int32>(DBvalidateFields.Where(f => f.DestinationColumn.Id == index).Select(f => f.DestinationColumn).FirstOrDefault()) { Cells = source.GetColumnCells(DBvalidateFields.Where(f => f.DestinationColumn.Id == index).Select(f => f.DestinationColumn).FirstOrDefault().InputType, DBvalidateFields.Where(f => f.DestinationColumn.Id == index).Select(f => f.Number).FirstOrDefault()) });
                        }
                        validatedItems.Add(dbValidate.DestinationColumn.Id);
                        validatedItems.AddRange(dbValidate.DestinationColumn.AlternativeAttributes);
                        invalidItems = View.ValidateAlternateDBItems(items);
                        invalidItems.ForEach(i => i.isDBduplicate = true);
                    }
                    else if (!validatedItems.Contains(dbValidate.DestinationColumn.Id))
                    {
                        invalidItems = View.ValidateDBItems(dbValidate.DestinationColumn, source.GetColumnCells(dbValidate.DestinationColumn.InputType, dbValidate.Number));
                        validatedItems.Add(dbValidate.DestinationColumn.Id);
                        invalidItems.ForEach(i => i.isDBduplicate = true);
                    }
                }
            }
            ValidateSourceData(source, notEmptyColumns, notDuplicatedColumns);
        }
        private List<InvalidImport> GetInvalidItems(ExternalResource source)
        {
            List<InvalidImport> invalidItems = new List<InvalidImport>();
            if (source.Rows.Where(r => r.isValid() == false).Any())
                invalidItems.Add(InvalidImport.InvalidData);
            if (source.Rows.Where(r => r.HasDuplicatedValues).Any())
                invalidItems.Add(InvalidImport.SourceDuplicatedData);
            if (source.Rows.Where(r => r.HasDBDuplicatedValues).Any())
                invalidItems.Add(InvalidImport.AlreadyExist);
            return invalidItems;
        }

        public void ValidateSourceData(ExternalResource source, List<ExternalColumnComparer<String, Int32>> notEmptyColumns, List<ExternalColumnComparer<String, Int32>> notDuplicatedColumns)
        {
            List<Int32> notEmptyCol = notEmptyColumns.Where(f => !f.DestinationColumn.HasAlternative).Select(f => f.Number).ToList();
            source.ColumHeader.Where(c => notEmptyCol.Contains(c.Number)).ToList().ForEach(c => c.AllowEmpty = false);


            //source.ColumHeader.Where(c => notDuplicatedColumns.Where(f => f.DestinationColumn.Mandatory && !f.DestinationColumn.HasAlternative).Select(f => f.Number).ToList().Contains(c.Number)).ToList().ForEach(c => c.AllowEmpty = false);
            List<Int32> notDupCol = notDuplicatedColumns.Where(f => f.DestinationColumn.Mandatory && !f.DestinationColumn.HasAlternative).Select(f => f.Number).ToList();

            source.ColumHeader.Where(c => notDupCol.Contains(c.Number)).ToList().ForEach(c => c.AllowDuplicate = false);
            foreach (ExternalColumn col in source.ColumHeader.Where(c=> notEmptyCol.Contains(c.Number) || notDupCol.Contains(c.Number)).ToList())
            {
                List<ExternalCell> cells = source.GetDuplicatedCells(col.InputType, col.Number);
                if (cells != null && cells.Count > 0)
                    cells.ForEach(c => c.SetDuplicatedRows(cells.Where(cc => cc != c).Select(cr => cr.Row.Number).ToList()));
            }
            source.Rows.ForEach(r=>r.AllowImport= r.isValid());
         
            // verifica alternative !

            List<ExternalColumn> colEmpty = source.ColumHeader.Where(c => notEmptyColumns.Where(f => f.DestinationColumn.Mandatory && f.DestinationColumn.HasAlternative).Select(f => f.Number).ToList().Contains(c.Number)).ToList();
          //  List<ExternalColumn> colDuplicate = source.ColumHeader.Where(c => notDuplicatedColumns.Where(f => f.DestinationColumn.Mandatory && f.DestinationColumn.HasAlternative).Select(f => f.Number).ToList().Contains(c.Number)).ToList();


            foreach (ExternalColumn col in source.ColumHeader.Where(c => notEmptyColumns.Where(f => f.DestinationColumn.Mandatory && f.DestinationColumn.HasAlternative).Select(f => f.Number).ToList().Contains(c.Number)).ToList())
            {
                List<ExternalCell> cells = source.GetDuplicatedCells(col.InputType, col.Number);
                if (cells != null && cells.Count > 0)
                    cells.ForEach(c => c.SetDuplicatedRows(cells.Where(cc => cc != c).Select(cr => cr.Row.Number).ToList()));
            }
            source.Rows.ForEach(r => r.AllowImport = r.isValid());
            if (colEmpty.Any()) {
                foreach (ExternalRow row in source.Rows)
                {
                    List<ExternalCell> cells = row.Cells.Where(c => colEmpty.Where(ce => ce.Number == c.Column.Number).Any()).ToList();
                    row.AllowImport = row.AllowImport && cells.Where(c => !c.isEmpty).Any();
                }
            }

            
            //foreach (ExternalRow row in Rows)
            //{
            //    row.AllowImport = row.isValid();
            //}
        }

        //private void ValidateSourceData()
        //{
        //    // Find all columns to validate at startup !
        //    List<ExternalColumn> cols = ColumHeader.Where(c => c.AllowDuplicate == false).ToList();
        //    foreach (ExternalColumn col in cols)
        //    {
        //        List<ExternalCell> cells = GetDuplicatedCells(col.InputType, col.Number);
        //        if (cells != null && cells.Count > 0)
        //            cells.ForEach(c => c.SetDuplicatedRows(cells.Where(cc => cc != c).Select(cr => cr.Row.Number).ToList()));
        //    }
        //    foreach (ExternalRow row in Rows)
        //    {
        //        row.AllowImport = row.isValid();
        //    }
        //}
    }
}