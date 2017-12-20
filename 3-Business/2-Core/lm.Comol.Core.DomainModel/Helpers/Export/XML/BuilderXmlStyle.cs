using System;
using System.Text;
using System.Collections.Generic;

namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    public static class BuilderXmlStyle
    {
        public static String StyleDefinition(String name, Alignment horizontal, Border border, Color font, Boolean bold)
        {
            String style = "<Style ss:ID=\"" + name + "\">" + "\r\n" +
                CellAlignment(horizontal) + border.GetStringValue()
                + FontCell(font, bold);

            style += "\r\n" + "</Style>";
            return style;
        }
        public static String StyleDefinition(String name, Alignment horizontal, Border border, Color font,Boolean bold, Color background)
        {
            String style = "<Style ss:ID=\"" + name + "\">" + "\r\n" +
                CellAlignment(horizontal) + border.GetStringValue()
                + FontCell(font, bold) + Background(background);

            style += "\r\n" + "</Style>";
            return style;
        }
        public static String StyleDefinition(String name, Alignment horizontal, Border border, Color font, Boolean bold, Color background, ItemFormat format)
        {
            String style = "<Style ss:ID=\"" + name + "\">" + "\r\n" +
                CellAlignment(horizontal) + border.GetStringValue()
                + FontCell(font, bold) + Background(background);
            switch (format) { 
                case ItemFormat.SmallDate:
                    style += "<NumberFormat ss:Format=\"d/m/yy\\ h\\.mm;@\"/>" + "\r\n";
                    break;
                case ItemFormat.ExtendedDate:
                    style += "<NumberFormat ss:Format=\"d/m/yy\\ h\\.mm;@\"/>" + "\r\n";
                    break;
                case ItemFormat.Time:
                    style += "<NumberFormat ss:Format=\"[$-F400]h:mm:ss\\ AM/PM\"/>" + "\r\n";
                    break;
            }
            style += "\r\n" + "</Style>";
            return style;
        }
        public static String StyleDefinition(String name, String content){
            return "<Style ss:ID=\"" + name + "\">" + "\r\n" + content + "\r\n" + "</Style>";
        }
        public static String Background(Color color) {
            return "\r\n" + "<Interior ss:Color=\"" + color.GetStringValue() + "\" ss:Pattern=\"Solid\"/>";
        }
        public static String FontCell(Color color, Boolean bold)
        {
            return "\r\n" + "<Font  ss:Color=\"" + color.GetStringValue() + "\" ss:Bold=\"" + (bold ? "1" : "0") + "\"/>";
        }
        public static String FontCell(Color color, Boolean bold , Boolean italic, Boolean underline) {
            return "\r\n" + "<Font  ss:Color=\"" + color.GetStringValue() + "\" ss:Bold=\"" + (bold ? "1" : "0") + "\" ss:Italic=\"" + (italic ? "1" : "0") + "\" ss:Underline=\"" + (underline ? "Single" : "") + "\"/>";
        }
        public static String CellAlignment(Alignment horizontal)
        {
            return CellAlignment(horizontal, Alignment.bottom);
        }
        public static String CellAlignment(Alignment horizontal, Alignment vertical)
        {
            return "\r\n" + "<Alignment ss:Horizontal=\"" + horizontal.GetStringValue() + "\" ss:Vertical=\"" + vertical.GetStringValue() + "\"/>";
        }


        public static List<String> GetDefaulSettings()
        {
            List<String> styles = new List<String>();
            styles.Add(StyleDefinition(DefaultXmlStyleElement.HeaderTable.ToString(), Alignment.center,
                            Border.continuous, Color.white, true, Color.darkgray));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.SubHeaderTable.ToString(), Alignment.center,
                          Border.continuous, Color.white, true, Color.darkgray));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.RowItem.ToString(), Alignment.left,
                            Border.continuous, Color.black, false, Color.white));

            styles.Add(BuilderXmlStyle.StyleDefinition(DefaultXmlStyleElement.RowAlternatingItem.ToString(), Alignment.left,
                            Border.continuous, Color.black, false, Color.lightgray));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemEmpty.ToString(), Alignment.center,
                           Border.continuous, BuilderXmlStyle.Color.black, false, Color.white));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemEmptyAlternate.ToString(), Alignment.center,
                          Border.continuous, BuilderXmlStyle.Color.black, false, Color.lightgray));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemYellow.ToString(), Alignment.center,
                         Border.continuous, BuilderXmlStyle.Color.black, false, Color.yellow));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemRed.ToString(), Alignment.center,
                         Border.continuous, BuilderXmlStyle.Color.black, false, Color.red));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemGreen.ToString(), Alignment.center,
                        Border.continuous, BuilderXmlStyle.Color.black, false, Color.green));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.Title.ToString(), Alignment.left,
                        Border.none, Color.black, true, Color.white));
            styles.Add(StyleDefinition(DefaultXmlStyleElement.TitleItem.ToString(), Alignment.left,
                       Border.none, Color.black, true));
            styles.Add(StyleDefinition(DefaultXmlStyleElement.PrintInfo.ToString(), Alignment.left,
                        Border.none, Color.black, false, Color.white));


            styles.Add(StyleDefinition(DefaultXmlStyleElement.HeaderTable.ToString()+"Date", Alignment.center,
                           Border.continuous, Color.white, true, Color.darkgray, ItemFormat.SmallDate ));
            styles.Add(StyleDefinition(DefaultXmlStyleElement.HeaderTable.ToString() + "Time", Alignment.center,
                         Border.continuous, Color.white, true, Color.darkgray, ItemFormat.Time));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.SubHeaderTable.ToString() + "Date", Alignment.center,
                          Border.continuous, Color.white, true, Color.darkgray, ItemFormat.SmallDate));
            styles.Add(StyleDefinition(DefaultXmlStyleElement.SubHeaderTable.ToString() + "Time", Alignment.center,
                         Border.continuous, Color.white, true, Color.darkgray, ItemFormat.Time));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.RowItem.ToString() + "Date", Alignment.left,
                            Border.continuous, Color.black, false, Color.white, ItemFormat.SmallDate));
            styles.Add(StyleDefinition(DefaultXmlStyleElement.RowItem.ToString() + "Time", Alignment.left,
                            Border.continuous, Color.black, false, Color.white, ItemFormat.Time));


            styles.Add(BuilderXmlStyle.StyleDefinition(DefaultXmlStyleElement.RowAlternatingItem.ToString() + "Date", Alignment.left,
                            Border.continuous, Color.black, false, Color.lightgray, ItemFormat.SmallDate));
            styles.Add(BuilderXmlStyle.StyleDefinition(DefaultXmlStyleElement.RowAlternatingItem.ToString() + "Time", Alignment.left,
                           Border.continuous, Color.black, false, Color.lightgray, ItemFormat.Time));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemEmpty.ToString() + "Date", Alignment.center,
                           Border.continuous, BuilderXmlStyle.Color.black, false, Color.white, ItemFormat.SmallDate));
            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemEmpty.ToString() + "Time", Alignment.center,
                          Border.continuous, BuilderXmlStyle.Color.black, false, Color.white, ItemFormat.Time));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemEmptyAlternate.ToString() + "Date", Alignment.center,
                          Border.continuous, BuilderXmlStyle.Color.black, false, Color.lightgray, ItemFormat.SmallDate));
            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemEmptyAlternate.ToString() + "Time", Alignment.center,
                         Border.continuous, BuilderXmlStyle.Color.black, false, Color.lightgray, ItemFormat.Time));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemYellow.ToString() + "Date", Alignment.center,
                         Border.continuous, BuilderXmlStyle.Color.black, false, Color.yellow, ItemFormat.SmallDate));
            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemYellow.ToString() + "Time", Alignment.center,
                        Border.continuous, BuilderXmlStyle.Color.black, false, Color.yellow, ItemFormat.Time));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemRed.ToString() + "Date", Alignment.center,
                         Border.continuous, BuilderXmlStyle.Color.black, false, Color.red, ItemFormat.SmallDate));
            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemRed.ToString() + "Time", Alignment.center,
                        Border.continuous, BuilderXmlStyle.Color.black, false, Color.red, ItemFormat.Time));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemGreen.ToString() + "Date", Alignment.center,
                        Border.continuous, BuilderXmlStyle.Color.black, false, Color.green, ItemFormat.SmallDate));
            styles.Add(StyleDefinition(DefaultXmlStyleElement.ItemGreen.ToString() + "Time", Alignment.center,
                        Border.continuous, BuilderXmlStyle.Color.black, false, Color.green, ItemFormat.Time));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.Title.ToString() + "Date", Alignment.left,
                        Border.none, Color.black, true, Color.white, ItemFormat.SmallDate));
            styles.Add(StyleDefinition(DefaultXmlStyleElement.Title.ToString() + "Time", Alignment.left,
                       Border.none, Color.black, true, Color.white, ItemFormat.Time));

            styles.Add(StyleDefinition(DefaultXmlStyleElement.PrintInfo.ToString() + "Date", Alignment.left,
                        Border.none, Color.black, false, Color.white, ItemFormat.SmallDate));
            styles.Add(StyleDefinition(DefaultXmlStyleElement.PrintInfo.ToString() + "Time", Alignment.left,
                      Border.none, Color.black, false, Color.white, ItemFormat.Time));
            return styles;
        }

    //    public static String FontCell(Color color, Boolean bold , Boolean italic, Boolean underline) { 
    //        return "\r\n" +
    //          <Font ss:FontName="Calibri" x:Family="Swiss" ss:Color="#F2F2F2" ss:Bold="1"
    //ss:Italic="1" ss:Underline="Single"/>
    //    }
        public enum Alignment {
            [StringValue("")]
            none = 0,
            [StringValue("Left")]
            left = 1,
            [StringValue("Center")]
            center = 2,
            [StringValue("Right")]
            right = 3,
            [StringValue("Justify")]
            justify = 4,
            [StringValue("Bottom")]
            bottom = 5,
            [StringValue("Top")]
            top = 6
        };

        public enum Border {
            [StringValue("")]
            none = 0,
            [StringValue("\r\n<Borders>" +
            "\r\n<Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>" +
            "\r\n<Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>" +
            "\r\n<Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>" +
            "\r\n<Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>"
            + "\r\n</Borders>")]
            continuous = 1
        
        };

        public enum Color {
            [StringValue("")]
            none = 0,
            [StringValue("#FFFFFF")]
            white = 1,
            [StringValue("#000000")]
            black = 2,
            [StringValue("#efefef")]
            lightgray = 3,
            [StringValue("#666666")]
            darkgray = 4,
            [StringValue("#FF9999")]
            red = 5,
            [StringValue("#FFFF99")]
            yellow = 6,
            [StringValue("#99FF99")]
            green = 7
        };
        
        public enum ItemFormat {
            [StringValue("")]
            none = 0,
            [StringValue(" <NumberFormat ss:Format=\"d/m/yy\\ h\\.mm;@\"/>" + "\r\n")]
            SmallDate = 1,
            [StringValue(" <NumberFormat ss:Format=\"[$-F400]h:mm:ss\\ AM/PM\"/>" + "\r\n")]
            Time = 2,
            [StringValue("<NumberFormat ss:Format=\"[$-F800]dddd\\,\\ mmmm\\ dd\\,\\ yyyy\"/>" + "\r\n")]
            ExtendedDate = 3
        };
    }

    [Serializable]
    public enum DefaultXmlStyleElement
    {
        HeaderTable = 1,
        RowItem = 2,
        RowAlternatingItem = 3,
        RowDeleted = 4,
        ItemEmpty = 5,
        ItemEmptyAlternate = 6,
        Title = 7,
        PrintInfo = 8,
        ItemYellow = 9,
        ItemRed = 10,
        ItemGreen = 11,
        SubHeaderTable = 12,
        TitleItem = 13
        //ItemYellow = 7,
        //ItemRed = 8,
        //Item
        
    }
}
