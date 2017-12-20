using System;
using System.Linq;
using System.Collections.Generic;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.DomainModel.Helpers
{
     [Serializable]
    public class ExternalResource 
    {
         public virtual List<ExternalColumn> ColumHeader { get; set; }
         public virtual List<ExternalRow> Rows { get; set; }

         public List<ExternalColumn> VerifyDuplicationValues() {
             if (ColumHeader == null && ColumHeader.Count == 0)
                 return new List<ExternalColumn>();
             else
                 return ColumHeader.Where(c => c.AllowDuplicate == false).ToList();
         }

         public ExternalResource()
         {
             ColumHeader = new List<ExternalColumn>();
             Rows = new List<ExternalRow>();
         }
         public ExternalResource(List<ExternalColumnComparer<String, Int32>> columns, CsvFile csvFile) :this()
         {
            if (csvFile != null) {
                 List<int> colIndexes = columns.Where(ec => ec.isValid).Select(c => c.Number).ToList();

                 foreach (TextColumn column in csvFile.ColumHeader.Where(c => colIndexes.Contains(c.Number)))
                 {
                     ColumHeader.Add(new ExternalColumn( columns.Where(c => c.Number == column.Number).Select(c => c.InputType).FirstOrDefault()) { Number = column.Number, Name=  columns.Where(c => c.Number == column.Number).Select(c => c.DestinationColumn.Name).FirstOrDefault()});
                 }
                 int index = 0;
                 csvFile.Rows.ForEach(r => Rows.Add(CreateRow(r, ref index)));
                 ValidateSourceData();
             }
         }
         //public ProfileExternalResource(List<ProfileColumnComparer<String>> columns, List<List<String>> rowItems)
         //{
         //    ColumHeader = new List<ProfileAttributeColumn>();
         //    Rows = new List<ProfileAttributesRow>();
         //    if (rowItems != null)
         //    {
         //        foreach (ProfileColumnComparer<String> column in columns.Where(c => c.isValid).ToList())
         //        {
         //            ColumHeader.Add(new ProfileAttributeColumn() { Number = column.Number, Attribute = columns.Where(c => c.Number == column.Number).Select(c => c.DestinationColumn).FirstOrDefault() });
         //        }
         //        int index = 0;
         //        rowItems.ForEach(r => Rows.Add(CreateRow(r, ref index)));
         //        ValidateStartData();
         //    }
         //}
         public ExternalResource(List<ExternalColumn> columns, List<List<String>> rowItems)
         {
             ColumHeader = new List<ExternalColumn>();
             ColumHeader.AddRange(columns);
             Rows = new List<ExternalRow >();
             if (rowItems != null && columns.Count>0)
             {
                 int index = 0;
                 rowItems.ForEach(r => Rows.Add(CreateRow(r, ref index)));
             }
         }
         private ExternalRow CreateRow(TextRow tRow, ref int index)
         {
             ExternalRow row = new ExternalRow();
             row.Number = index++;

             ColumHeader.ForEach(c=> row.Cells.Add(new ExternalCell() { Column=c, Row=row, Value= tRow[c.Number-1]}));

             return row;
         }
         private ExternalRow CreateRow(List<String> tRow, ref int index)
         {
             ExternalRow row = new ExternalRow();
             row.Number = index++;
             if (tRow.Count== ColumHeader.Count)
                 ColumHeader.Select((c, colIndex) => new { Col = c, Index = colIndex }).ToList().ForEach(c => row.Cells.Add(new ExternalCell() { Column = c.Col, Row = row, Value = tRow[c.Index] }));
             else
                ColumHeader.ForEach(c => row.Cells.Add(new ExternalCell() { Column = c, Row = row, Value = tRow[c.Number - 1]}));

             return row;
         }

         public void ValidateSourceData(List<Int32> notEmptyColumns, List<Int32> notDuplicatedColumns)
         {
             ColumHeader.Where(c => notEmptyColumns.Contains(c.Number)).ToList().ForEach(c => c.AllowEmpty = false);
             ColumHeader.Where(c => notDuplicatedColumns.Contains(c.Number)).ToList().ForEach(c => c.AllowDuplicate = false);
             ValidateSourceData();
         }

         private void ValidateSourceData()
         {
             // Find all columns to validate at startup !
             List<ExternalColumn> cols = ColumHeader.Where(c => c.AllowDuplicate == false).ToList();
             foreach (ExternalColumn col in cols)
             {
                 List<ExternalCell> cells = GetDuplicatedCells(col.InputType, col.Number);
                 if (cells != null && cells.Count > 0)
                     cells.ForEach(c => c.SetDuplicatedRows(cells.Where(cc => cc != c).Select(cr => cr.Row.Number).ToList()));
             }
             foreach (ExternalRow row in Rows)
             {
                 row.AllowImport = row.isValid();
             }
         }

         public List<ExternalCell> GetColumnCells(InputType inputType, Int32 index)
         {
             List<ExternalCell> items = new List<ExternalCell>();
             if (ColumHeader.Where(c => c.InputType == inputType && c.Number==index).Any())
             {
                 ExternalColumn col = ColumHeader.Where(c => c.InputType == inputType && c.Number == index).FirstOrDefault();

                 items = (from r in Rows select r.GetCell(col.Number)).ToList();
                 // Rows.ForEach(r => items.AddRange(r.Select((value, i) => new { Value = value, Index = i }).Where(item => item.Index == index).Select(item => item.Value).ToList()));
             }
             return items;
         }
       
         //public Dictionary<int, String> GetColumnCellsValue(ProfileAttributeType attribute)
         //{
         //    return GetColumnCells(attribute).Select((cell, i) => new { Value = cell.Value, RowIndex = i }).ToDictionary(item => item.RowIndex,item=> item.Value);
         //}
         public List<ExternalCell> GetDuplicatedCells(InputType inputType, Int32 index)
         {
             List<ExternalCell> items = new List<ExternalCell>();
             List<ExternalCell> values = GetColumnCells(inputType, index);
             var query = values.Where(ri=> !String.IsNullOrEmpty(ri.Value)).GroupBy(ri => ri.Value, ri => ri).Where(ri => ri.Count() > 1);

             query.ToList().ForEach(r => items.AddRange(r));

             return items.Distinct().ToList();

         }
         //public Dictionary<int, String> GetDuplicatedCellsValue(ProfileAttributeType attribute)
         //{
         //    return GetDuplicatedCells(attribute).Select((cell, i) => new { Value = cell.Value, RowIndex = i }).ToDictionary(item => item.RowIndex, item => item.Value);
         //}
    }
}