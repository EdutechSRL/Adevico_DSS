using System;
using System.Linq;
using System.Collections.Generic;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.DomainModel.Helpers
{
     [Serializable]
    public class ProfileExternalResource 
    {
         public virtual List<ProfileAttributeColumn> ColumHeader { get; set; }
         public virtual List<ProfileAttributesRow> Rows { get; set; }

         public List<ProfileAttributeColumn> VerifyDuplicationValues() {
             if (ColumHeader == null && ColumHeader.Count == 0)
                 return new List<ProfileAttributeColumn>();
             else
                 return ColumHeader.Where(c => c.AllowDuplicate == false).ToList();
         }

         public ProfileExternalResource()
         {
             ColumHeader = new List<ProfileAttributeColumn>();
             Rows = new List<ProfileAttributesRow>();
         }
         public ProfileExternalResource(List<ProfileColumnComparer<String>> columns, CsvFile csvFile) 
         {
            ColumHeader = new List<ProfileAttributeColumn>();
            Rows = new List<ProfileAttributesRow>();
            if (csvFile != null) {
                 List<int> colIndexes = columns.Where(ec => ec.isValid).Select(c => c.Number).ToList();

                 foreach (TextColumn column in csvFile.ColumHeader.Where(c => colIndexes.Contains(c.Number)))
                 {
                     ColumHeader.Add(new ProfileAttributeColumn() { Number = column.Number, Attribute = columns.Where(c => c.Number == column.Number).Select(c => c.DestinationColumn).FirstOrDefault() });
                 }
                 int index = 0;
                 csvFile.Rows.ForEach(r => Rows.Add(CreateRow(r, ref index)));
                 ValidateStartData();
             }
         }
       

       
         public ProfileExternalResource(List<ProfileColumnComparer<String>> columns, List<List<String>> rowItems)
         {
             ColumHeader = new List<ProfileAttributeColumn>();
             Rows = new List<ProfileAttributesRow>();
             if (rowItems != null)
             {
                 foreach (ProfileColumnComparer<String> column in columns.Where(c => c.isValid).ToList())
                 {
                     ColumHeader.Add(new ProfileAttributeColumn() {   Number = column.Number, Attribute = columns.Where(c => c.Number == column.Number).Select(c => c.DestinationColumn).FirstOrDefault() });
                 }
                 int index = 0;
                 rowItems.ForEach(r => Rows.Add(CreateRow(r, ref index)));
                 ValidateStartData();
             }
         }
         public ProfileExternalResource(List<ProfileAttributeColumn> columns, List<List<String>> rowItems)
         {
             ColumHeader = new List<ProfileAttributeColumn>();
             ColumHeader.AddRange(columns);
             Rows = new List<ProfileAttributesRow>();
             if (rowItems != null)
             {
                 int index = 0;
                 rowItems.ForEach(r => Rows.Add(CreateRow(r, ref index)));
                 ValidateStartData();
             }
         }
         public ProfileExternalResource(List<ProfileAttributeColumn> columns, List<List<ProfileAttributeCell>> rowItems)
         {
             ColumHeader = new List<ProfileAttributeColumn>();
             ColumHeader.AddRange(columns);
             Rows = new List<ProfileAttributesRow>();
             if (rowItems != null)
             {
                 int index = 0;
                 rowItems.ForEach(r => Rows.Add(CreateRow(r, ref index)));
                 ValidateStartData();
             }
         }
         private ProfileAttributesRow CreateRow(TextRow tRow, ref int index)
         {
             ProfileAttributesRow row = new ProfileAttributesRow();
             row.Number = index++;

             ColumHeader.ForEach(c=> row.Cells.Add(new ProfileAttributeCell() { Column=c, Row=row, Value= tRow[c.Number-1]}));

             return row;
         }
         private ProfileAttributesRow CreateRow(List<String> tRow, ref int index)
         {
             ProfileAttributesRow row = new ProfileAttributesRow();
             row.Number = index++;
             if (tRow.Count== ColumHeader.Count)
                 ColumHeader.Select((c, colIndex) => new { Col = c, Index = colIndex }).ToList().ForEach(c => row.Cells.Add(new ProfileAttributeCell() { Column = c.Col, Row = row, Value = tRow[c.Index] }));
             else
                ColumHeader.ForEach(c => row.Cells.Add(new ProfileAttributeCell() { Column = c, Row = row, Value = tRow[c.Number - 1]}));

             return row;
         }
         private ProfileAttributesRow CreateRow(List<ProfileAttributeCell> tRow, ref int index)
         {
             ProfileAttributesRow row = new ProfileAttributesRow();
             row.Number = index++;
             if (tRow.Count == ColumHeader.Count)
                 ColumHeader.Select((c, colIndex) => new { Col = c, Index = colIndex }).ToList().ForEach(c => row.Cells.Add(ProfileAttributeCell.Update(tRow[c.Index],c.Col, row)));
             else
                 ColumHeader.ForEach(c => row.Cells.Add( ProfileAttributeCell.Update(tRow[c.Number - 1],c, row)));

             return row;
         }

         private void ValidateStartData() { 
            // Find all columns to validate at startup !
            List<ProfileAttributeColumn> cols = ColumHeader.Where(c=> c.AllowDuplicate==false).ToList();
            foreach (ProfileAttributeColumn col in cols) {
                List<ProfileAttributeCell> cells = GetDuplicatedCells(col.Attribute);
                if (cells !=null && cells.Count>0)
                    cells.ForEach(c => c.SetDuplicatedRows(cells.Where(cc => cc != c).Select(cr => cr.Row.Number).ToList()));
            }
            foreach (ProfileAttributesRow row in Rows)
            {
                row.AllowImport = row.isValid();
            }
        }

         //private ProfileExternalTextRow CreateExternalRow(TextRow row, List<int> colIndexes, ref int index)
         //{
         //    ProfileExternalTextRow result = new ProfileExternalTextRow();
         //    result.Number = index;
         //    result.AddRange(row.Select((value, i) => new { Value = value, Index = i }).Where(c => colIndexes.Contains(c.Index)).Select(c => c.Value).ToList());
         //    index += 1;
         //    return result;
         //}

         public List<ProfileAttributeCell> GetColumnCells(ProfileAttributeType attribute)
         {
             List<ProfileAttributeCell> items = new List<ProfileAttributeCell>();
             if (ColumHeader.Where(c => c.Attribute == attribute).Any())
             {
                 ProfileAttributeColumn col = ColumHeader.Where(c => c.Attribute == attribute).FirstOrDefault();

                 items = (from r in Rows select r.GetCell(col)).ToList();
                // Rows.ForEach(r => items.AddRange(r.Select((value, i) => new { Value = value, Index = i }).Where(item => item.Index == index).Select(item => item.Value).ToList()));
             }
             return items;
         }
         public List<ProfileAttributeCell> GetColumnCells(List<ProfileAttributeType> attributes)
         {
             List<ProfileAttributeCell> items = new List<ProfileAttributeCell>();
             if (ColumHeader.Where(c => attributes.Contains(c.Attribute)).Any())
             {
                 List<ProfileAttributeColumn> cols = ColumHeader.Where(c => attributes.Contains(c.Attribute)).ToList();
                 cols.ForEach(c=> items.AddRange( (from r in Rows select r.GetCell(c)).ToList()));
             }
             return items;
         }
         public Dictionary<int, String> GetColumnCellsValue(ProfileAttributeType attribute)
         {
             return GetColumnCells(attribute).Select((cell, i) => new { Value = cell.Value, RowIndex = i }).ToDictionary(item => item.RowIndex,item=> item.Value);
         }
         public List<ProfileAttributeCell> GetDuplicatedCells(ProfileAttributeType attribute)
         {
             List<ProfileAttributeCell> items = new List<ProfileAttributeCell>();
             List<ProfileAttributeCell> values = GetColumnCells(attribute);
             var query = values.Where(ri=> !String.IsNullOrEmpty(ri.Value)).GroupBy(ri => ri.Value, ri => ri).Where(ri =>ri.Count() > 1);

             query.ToList().ForEach(r => items.AddRange(r));

             return items.Distinct().ToList();

         }
         public Dictionary<int, String> GetDuplicatedCellsValue(ProfileAttributeType attribute)
         {
             return GetDuplicatedCells(attribute).Select((cell, i) => new { Value = cell.Value, RowIndex = i }).ToDictionary(item => item.RowIndex, item => item.Value);
         }
         //public List<String> GetColumnValues(ProfileAttributeType column){
         //    List<String> items = new List<String>();
         //    if ( ColumHeader.Where(c=> c.Attribute== column).Any()){
         //       int index = ColumHeader.Where(c=> c.Attribute== column).Select(c=>c.Number-1).FirstOrDefault();

         //       Rows.ForEach(r=>  items.AddRange(r.Select((value,i) => new {Value=value, Index=i}).Where(item=> item.Index== index).Select(item=>item.Value).ToList()));
         //    }
         //    return items;
         //}

         //public List<int> GetDuplicatedRow(List<ProfileAttributeType> attributes) { 
         //   List<int> items = new List<int>();
         //   attributes.ForEach( a=> items.AddRange(GetDuplicatedRow(a)));
         //   return items.Distinct().ToList();
         //}
         //public List<int> GetDuplicatedRow(ProfileAttributeType attribute)
         //{
         //    List<int> items = new List<int>();
         //    Dictionary<int, String> values = GetColumnCellValues(attribute);
         //    var query = values.GroupBy(ri => ri.Value, ri => ri).Where(ri => ri.Count() > 1);

         //    //.GroupBy(it=>new {Value= it.Value, Index=it.Index}).Select(lst => new { Item = lst.Key, Count = lst.Count() });
         //    query.ToList().ForEach(r => items.AddRange(r.Select(g => g.Key)));

         //    return items.Distinct().ToList();

         //}

         //public List<int> GetDuplicatedRow(ProfileAttributeType attribute)
         //{
         //    List<int> items = new List<int>();
         //    List<String> values = GetColumnValues(attribute);
         //    var query = values.Select((value,rowIndex) => new {Value=value, Index=rowIndex}).GroupBy(ri=>  ri.Value,ri=>ri).Where(ri=>ri.Count()>1);

         //    //.GroupBy(it=>new {Value= it.Value, Index=it.Index}).Select(lst => new { Item = lst.Key, Count = lst.Count() });
         //    query.ToList().ForEach(r => items.AddRange(r.Select(g => g.Index)));

         //    return items.Distinct().ToList();

         //}

         public List<ProfileAttributesRow> GetOrderedRows()
         {
             return (from ProfileAttributesRow r in Rows orderby !r.Selectable, r.DisplayName select r).ToList();
         }
    }
}