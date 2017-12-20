using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using lm.Comol.Core.DomainModel.Helpers;
using Microsoft.VisualBasic.FileIO;

namespace lm.Comol.Core.File
{
    public class CsvFileReader {

        public CsvFile ReadFile(String fullname, dtoCsvSettings settings, int lines)
        {
            CsvFile result = null;
            TextFieldParser parser = null;
            try{
                result = new CsvFile();
                using (parser = new TextFieldParser(fullname,System.Text.Encoding.Default))
                {
                    parser.Delimiters = settings.GetColumnDelimiters;
                    parser.TrimWhiteSpace = true;
                    long rowIndex=1;
                    while ((lines<=0 && !parser.EndOfData) || (rowIndex<=lines)){
                        try
                        {
                            string[] parts = parser.ReadFields();
                            if (!(settings.RowsToSkip > 0 && rowIndex <= settings.RowsToSkip)) {
                                if (rowIndex == 1 && settings.FirstRowColumnNames)
                                {
                                    List<TextColumn> columns = new List<TextColumn>();
                                    columns.AddRange((from i in Enumerable.Range(1, parts.Count()).ToList() 
                                                      select new TextColumn() { Number=i, Value= parts[i-1]}).ToList());
                                    result.ColumHeader.AddRange(columns);
                                }
                                else
                                {
                                    List<string> cells = parts.ToList();
                                    if (cells.Where(c => !string.IsNullOrEmpty(c)).Any())
                                    {
                                        TextRow row = new TextRow();
                                        row.AddRange(cells);
                                        result.Rows.Add(row);
                                    }
                                    else
                                        rowIndex--;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        
                        }
                        rowIndex++;
                    }
                    if (result.Rows.Count > 0)
                    {
                        int colCount = result.ColumHeader.Count;
                        int colToAdd = (from r in result.Rows select r.Count).Max();
                        if (colToAdd > 0 && colCount < colToAdd)
                            result.ColumHeader.AddRange((from i in Enumerable.Range(colCount + 1, colToAdd - colCount).ToList() select new TextColumn() { Number = i }).ToList());
                        colCount = result.ColumHeader.Count;
                        foreach (TextRow tRow in result.Rows.Where(r => r.Count < colCount).ToList())
                        {
                            tRow.AddRange((from i in Enumerable.Range(1, colCount - tRow.Count).ToList() select "").ToList());
                        }
                    }
                }
                parser.Dispose();
                parser = null;
            }
            catch( Exception ex){
                if (parser!=null)
                    parser.Dispose();
                result = null;
            }
            return result;
        }
    }
}
    //public class CsvFileReader : StreamReader
    //{
    //    public CsvFileReader(Stream stream)
    //        : base(stream)
    //    {
    //    }
    //    public CsvFileReader(string filename)
    //        : base(filename)
    //    {
    //    }


    //    /// <summary>
    //    /// Reads a row of data from a CSV file
    //    /// </summary>
    //    /// <param name="row"></param>
    //    /// <returns></returns>
    //    public bool ReadRow(TextRow row, char separator)
    //    {
    //        row.LineText = ReadLine();
    //        if (String.IsNullOrEmpty(row.LineText))
    //            return false;

    //        int pos = 0;
    //        int rows = 0;

    //        while (pos < row.LineText.Length)
    //        {
    //            string value;

    //            // Special handling for quoted field
    //            if (row.LineText[pos] == '"')
    //            {
    //                // Skip initial quote
    //                pos++;

    //                // Parse quoted value
    //                int start = pos;
    //                while (pos < row.LineText.Length)
    //                {
    //                    // Test for quote character
    //                    if (row.LineText[pos] == '"')
    //                    {
    //                        // Found one
    //                        pos++;

    //                        // If two quotes together, keep one
    //                        // Otherwise, indicates end of value
    //                        if (pos >= row.LineText.Length || row.LineText[pos] != '"')
    //                        {
    //                            pos--;
    //                            break;
    //                        }
    //                    }
    //                    pos++;
    //                }
    //                value = row.LineText.Substring(start, pos - start);
    //                value = value.Replace("\"\"", "\"");
    //            }
    //            else
    //            {
    //                // Parse unquoted value
    //                int start = pos;
    //                while (pos < row.LineText.Length && row.LineText[pos] != separator)
    //                    pos++;
    //                value = row.LineText.Substring(start, pos - start);
    //            }

    //            // Add field to list
    //            if (rows < row.Count)
    //                row[rows] = value;
    //            else
    //                row.Add(value);
    //            rows++;

    //            // Eat up to and including next comma
    //            while (pos < row.LineText.Length && row.LineText[pos] != separator)
    //                pos++;
    //            if (pos < row.LineText.Length)
    //                pos++;
    //        }
    //        // Delete any unused items
    //        while (row.Count > rows)
    //            row.RemoveAt(rows);

    //        // Return true if any columns read
    //        return (row.Count > 0);
    //    }
    //}