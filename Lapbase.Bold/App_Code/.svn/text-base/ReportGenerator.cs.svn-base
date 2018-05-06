#region using modules
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Xml;
#endregion 

public class ReportGenerator
{
    public ReportGenerator() { }


/** /
public class GraphGenerator
{
    #region Build the class constructor
    public GraphGenerator()
	{
    }
    #endregion Build the class constructor
    
    #region Generate Report Method
    public void GenerateRdl(HttpResponse MyResp, string reportTitle, string reportPath, string reportName)
    {
        FileStream stream = File.OpenWrite(reportPath + reportName);
        XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);

        writer.Formatting = Formatting.Indented;
        writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
        #region Report
        writer.WriteStartElement("Report");

        #region Report configuration
        writer.WriteAttributeString("xmlns", null, "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition");
        writer.WriteAttributeString("xmlns:rd", null, "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
        writer.WriteAttributeString("Description", null, strDecsription);
        writer.WriteAttributeString("Author", null, "Gate13 Company");
        //writer.WriteElementString("rd:ReportID", "342d3811-e7e9-4311-a5f9-dd3879ccc21a");
        writer.WriteElementString("Language", "en-US");
        writer.WriteElementString("Width", strReportWidth); // 28.5
        //writer.WriteElementString("InteractiveHeight", "19.7cm"); //29.7
        //writer.WriteElementString("InteractiveWidth", "21cm");//21
        writer.WriteElementString("TopMargin", "0.5cm"); // 2.5
        writer.WriteElementString("LeftMargin", "0.5cm"); //2.5
        writer.WriteElementString("RightMargin", "0.5cm");//2.5
        writer.WriteElementString("BottomMargin", "0.5cm");//2.5
        //writer.WriteElementString("PageWidth", "21cm");//21
        //writer.WriteElementString("PageHeight", "29.7cm");
        writer.WriteElementString("rd:DrawGrid", "true");
        writer.WriteElementString("rd:GridSpacing", "0.1cm");//0.25
        writer.WriteElementString("rd:SnapToGrid", "true");
        #endregion Report configuration

        #region DataSources element
        writer.WriteStartElement("DataSources");
        writer.WriteStartElement("DataSource");
        writer.WriteAttributeString("Name", null, strDatasetName);

        writer.WriteStartElement("ConnectionProperties");
        writer.WriteStartElement("ConnectString");
        writer.WriteEndElement();
        writer.WriteElementString("DataProvider", "SQL");
        writer.WriteEndElement();
        //writer.WriteElementString("rd:DataSourceID", "b98b3838-e20c-4371-8c28-20092373a2f0");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion DataSources element

        #region DataSets
        writer.WriteStartElement("DataSets");
        writer.WriteStartElement("DataSet");
        writer.WriteAttributeString("Name", null, strDatasetName);
        //writer.WriteStartElement("rd:DataSetInfo");
        //writer.WriteElementString("rd:DataSetName", strDatasetName);
        //writer.WriteElementString("rd:TableName", strTableName);
        //writer.WriteEndElement();

        writer.WriteStartElement("Query");
        writer.WriteElementString("rd:UseGenericDesigner", "true");

        writer.WriteStartElement("CommandText");
        writer.WriteEndElement();
        writer.WriteElementString("DataSourceName", strDatasetName);
        writer.WriteEndElement();

        // FIELDS XXXXXXXXXXXXXXXXXXXX
        writer.WriteStartElement("Fields");

        //writer.WriteStartElement("Field");
        //writer.WriteAttributeString("Name", null, "strDateSeen");
        //writer.WriteElementString("rd:TypeName", "System.String");
        //writer.WriteElementString("DataField", "strDateSeen");
        //writer.WriteEndElement();

        //writer.WriteStartElement("Field");
        //writer.WriteAttributeString("Name", null, "EWL");
        //writer.WriteElementString("rd:TypeName", "System.Double");
        //writer.WriteElementString("DataField", "EWL");
        //writer.WriteEndElement();

        for (int Xh = 0; Xh < strXFields.Length / 2; Xh++)
        {
            writer.WriteStartElement("Field");
            writer.WriteAttributeString("Name", null, strXFields[Xh, 0]);
            writer.WriteElementString("rd:TypeName", strXFields[Xh, 1]);
            writer.WriteElementString("DataField", strXFields[Xh, 0]);
            writer.WriteEndElement();
        }


        for (int Xh = 0; Xh < strYFields.Length / 2; Xh++)
        {
            writer.WriteStartElement("Field");
            writer.WriteAttributeString("Name", null, strYFields[Xh, 0]);
            writer.WriteElementString("rd:TypeName", strYFields[Xh, 1]);
            writer.WriteElementString("DataField", strYFields[Xh, 0]);
            writer.WriteEndElement();
        }

        writer.WriteEndElement(); // end of Fields
        writer.WriteEndElement(); // end of DataSet
        writer.WriteEndElement();
        #endregion DataSets

        #region Element Body
        writer.WriteStartElement("Body");
        writer.WriteElementString("ColumnSpacing", "1cm");
        writer.WriteStartElement("ReportItems");
        writer.WriteStartElement("Chart");
        writer.WriteAttributeString("Name", null, "Excess_Weight_Loss");
        writer.WriteElementString("PointWidth", "0");
        writer.WriteElementString("Palette",  strPaletteType[intPaletteType] /*"EarthTones"* /);

        #region ChartData
        writer.WriteStartElement("ChartData");
        writer.WriteStartElement("ChartSeries");
        writer.WriteStartElement("DataPoints");
        writer.WriteStartElement("DataPoint");

        writer.WriteStartElement("DataValues");
        //writer.WriteStartElement("DataValue");
        //writer.WriteElementString("Value", "=Fields!strDateSeen.Value");
        //writer.WriteEndElement();

        //writer.WriteStartElement("DataValue");
        //writer.WriteElementString("Value", "=Fields!EWL.Value");
        //writer.WriteEndElement();
        //writer.WriteEndElement(); // end of DataValues

        for (int Xh = 0; Xh < strYFields.Length / 2; Xh++)
        {
            writer.WriteStartElement("DataValue");
            writer.WriteElementString("Value", "=Fields!" + strYFields[Xh, 0] + ".Value");
            writer.WriteEndElement();
            writer.WriteEndElement(); // end of DataValues
        }

        writer.WriteStartElement("DataLabel");
        writer.WriteStartElement("Style");
        writer.WriteElementString("FontSize", "8pt");
        writer.WriteElementString("FontStyle", "Italic");
        writer.WriteEndElement();

        //writer.WriteElementString("Value", "=Fields!EWL.Value");
        //writer.WriteElementString("Visible", "true");
        //writer.WriteEndElement(); // end of DataLabel

        for (int Xh = 0; Xh < strYFields.Length / 2; Xh++)
        {
            writer.WriteElementString("Value", "=Fields!" + strYFields[Xh, 0] + ".Value");
            writer.WriteElementString("Visible", "true");
            writer.WriteEndElement(); // end of DataLabel
        }

        writer.WriteStartElement("Style");
        writer.WriteStartElement("BorderStyle");
        writer.WriteElementString("Default", "Solid");
        writer.WriteEndElement();
        writer.WriteStartElement("BorderWidth");
        writer.WriteElementString("Default", "1pt");
        writer.WriteEndElement();
        writer.WriteEndElement(); // end of Style

        writer.WriteStartElement("Marker");
        writer.WriteElementString("Type", strMarkerType[intMarkerType]);
        writer.WriteElementString("Size", intMarkerSize.ToString() + "pt");
        writer.WriteEndElement(); // end of Marker

        writer.WriteEndElement(); // end of DataPoint
        writer.WriteEndElement(); // end of DataPOints
        writer.WriteEndElement(); // end of ChartSeries
        writer.WriteEndElement();
        #endregion ChartData

        #region Legend
        writer.WriteStartElement("Legend");
        writer.WriteElementString("Visible", "true");
        writer.WriteStartElement("Style");
        writer.WriteStartElement("BorderStyle");
        writer.WriteElementString("Default", "Solid");
        writer.WriteEndElement();
        writer.WriteElementString("BackgroundColor", "#ffff80");
        writer.WriteEndElement();
        writer.WriteElementString("Position", "RightTop");
        writer.WriteElementString("InsidePlotArea", "false");
        writer.WriteEndElement();
        #endregion Legend
        writer.WriteElementString("DataSetName", strDatasetName);
        writer.WriteElementString("Type", strChartType[intChartType]);
        switch ((ChartTypeList)intChartType) 
        {
            case ChartTypeList.Column :
            case ChartTypeList.Bar :
            case ChartTypeList.Area :
                switch ((ChartSubTypeList)intChartSubType)
                {
                    case ChartSubTypeList.Plain:
                    case ChartSubTypeList.Stacked:
                    case ChartSubTypeList.PercentStacked:
                        writer.WriteElementString("Subtype", strChartSubType[intChartSubType]);
                        break;
                    default:
                        writer.WriteElementString("Subtype", strChartSubType[(int)ChartSubTypeList.Plain]);
                        break;
                }
                break;
            
            case ChartTypeList.Line :
                switch ((ChartSubTypeList)intChartSubType)
                {
                    case ChartSubTypeList.Plain:
                    case ChartSubTypeList.Exploded:
                        writer.WriteElementString("Subtype", strChartSubType[intChartSubType]);
                        break;
                    default:
                        writer.WriteElementString("Subtype", strChartSubType[(int)ChartSubTypeList.Plain]);
                        break;
                }
                break;

            case ChartTypeList.Pie :
                switch ((ChartSubTypeList)intChartSubType)
                {
                    case ChartSubTypeList.Plain:
                    case ChartSubTypeList.Smooth:
                        writer.WriteElementString("Subtype", strChartSubType[intChartSubType]);
                        break;
                    default:
                        writer.WriteElementString("Subtype", strChartSubType[(int)ChartSubTypeList.Plain]);
                        break;
                }
                break;

            case ChartTypeList.Scatter :
                switch ((ChartSubTypeList)intChartSubType)
                {
                    case ChartSubTypeList.Plain:
                    case ChartSubTypeList.Line:
                    case ChartSubTypeList.SmoothLine:
                        writer.WriteElementString("Subtype", strChartSubType[intChartSubType]);
                        break;
                    default:
                        writer.WriteElementString("Subtype", strChartSubType[(int)ChartSubTypeList.Plain]);
                        break;
                }
                break;

            case ChartTypeList.Bubble :
                writer.WriteElementString("Subtype", strChartSubType[(int)ChartSubTypeList.Plain]);
                break;

            case ChartTypeList.Doughnut :
                switch ((ChartSubTypeList)intChartSubType)
                {
                    case ChartSubTypeList.Plain:
                    case ChartSubTypeList.Exploded:
                        writer.WriteElementString("Subtype", strChartSubType[intChartSubType]);
                        break;
                    default:
                        writer.WriteElementString("Subtype", strChartSubType[(int)ChartSubTypeList.Plain]);
                        break;
                }
                break;
            case ChartTypeList.Stock :
                switch ((ChartSubTypeList)intChartSubType)
                {
                    case ChartSubTypeList.Plain:
                    case ChartSubTypeList.HighLowClose:
                    case ChartSubTypeList.OpenHighLowClode:
                    case ChartSubTypeList.Candlestick:
                        writer.WriteElementString("Subtype", strChartSubType[intChartSubType]);
                        break;
                    default:
                        writer.WriteElementString("Subtype", strChartSubType[(int)ChartSubTypeList.Plain]);
                        break;
                }
                break;
        }

        #region Title
        writer.WriteStartElement("Title");
        writer.WriteElementString("Caption", reportTitle);
        writer.WriteStartElement("Style");
        writer.WriteElementString("FontWeight", "700");
        writer.WriteElementString("FontStyle", "Italic");
        writer.WriteEndElement();
        writer.WriteEndElement();
        #endregion Title

        #region CategotyAxis
        writer.WriteStartElement("CategoryAxis");
        writer.WriteStartElement("Axis");
        //writer.WriteElementString("MajorInterval", "1");
        writer.WriteElementString("Margin", "true");
        writer.WriteStartElement("Title"); // title
        writer.WriteElementString("Caption", strXAxisCaption /*"Date Seen"* /);
        writer.WriteStartElement("Style"); // title style
        writer.WriteElementString("FontWeight", "700");
        writer.WriteElementString("Color", "CadetBlue");
        writer.WriteElementString("FontStyle", "Italic");
        writer.WriteEndElement(); // end title style
        writer.WriteEndElement(); // end title

        writer.WriteStartElement("Style"); // Axis Style
        writer.WriteElementString("FontSize", "8pt");
        writer.WriteElementString("FontStyle", "Italic");
        writer.WriteEndElement(); // end Axis Style

        writer.WriteStartElement("MajorGridLines"); 
        writer.WriteStartElement("Style");
        writer.WriteStartElement("BorderStyle");
        writer.WriteElementString("Default", "Solid");
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement(); // end of MajorGridLines

        writer.WriteStartElement("MinorGridLines");
        writer.WriteStartElement("Style");
        writer.WriteStartElement("BorderStyle");
        writer.WriteElementString("Default", "Solid");
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement(); // end of MinorGridLines

        writer.WriteElementString("MajorTickMarks", "Outside");
        writer.WriteElementString("Visible", "true");
        writer.WriteEndElement(); // end of Axis
        writer.WriteEndElement(); // end of CategoryAxis
        #endregion CategotyAxis

        writer.WriteStartElement("ThreeDProperties");
        writer.WriteElementString("Enabled", strChart3D);
        writer.WriteElementString("Rotation", "30");
        writer.WriteElementString("Inclination", "30");
        writer.WriteElementString("Shading", "Real");
        writer.WriteElementString("WallThickness", "50");
        writer.WriteEndElement();

        //<SeriesGroupings>
        //  <SeriesGrouping>
        //    <DynamicSeries>
        //      <Grouping Name="column_chart_SeriesGroup">
        //        <GroupExpressions>
        //          <GroupExpression>=Fields!Category.Value</GroupExpression>
        //        </GroupExpressions>
        //      </Grouping>
        //      <Label>=Fields!Category.Value</Label>
        //    </DynamicSeries>
        //  </SeriesGrouping>
        //</SeriesGroupings>

        writer.WriteStartElement("PlotArea");
        writer.WriteStartElement("Style");
        writer.WriteStartElement("BorderStyle");
        writer.WriteElementString("Default", "Solid");
        writer.WriteEndElement();
        writer.WriteElementString("BackgroundColor", "LightGrey");
        writer.WriteStartElement("BorderWidth");
        writer.WriteElementString("Default", "2pt");
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement(); // end of PlotArea

        #region ValueAxis
        writer.WriteStartElement("ValueAxis");
        writer.WriteStartElement("Axis");
        writer.WriteStartElement("Title");
        writer.WriteElementString("Caption", strYAxisCaption );
        writer.WriteStartElement("Style");
        writer.WriteElementString("FontWeight", "700");
        writer.WriteElementString("Color", "DarkCyan");
        writer.WriteElementString("FontStyle", "Italic");
        writer.WriteEndElement();
        writer.WriteEndElement(); // end of Axis-Title
        writer.WriteStartElement("Style");
        writer.WriteElementString("FontSize", "8pt");
        writer.WriteElementString("FontStyle", "Italic");
        writer.WriteEndElement();

        writer.WriteStartElement("MajorGridLines");
        writer.WriteElementString("ShowGridLines", "true");
        writer.WriteStartElement("Style");
        writer.WriteStartElement("BorderStyle");
        writer.WriteElementString("Default", "Solid");
        writer.WriteEndElement();
        writer.WriteStartElement("BorderColor");
        writer.WriteElementString("Default", "DarkGray");
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteStartElement("MinorGridLines");
        writer.WriteStartElement("Style");
        writer.WriteStartElement("BorderStyle");
        writer.WriteElementString("Default", "Solid");
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();

        writer.WriteElementString("MajorTickMarks", "Outside");
        writer.WriteElementString("Min", strMin);
        writer.WriteElementString("Max", strMax);
        writer.WriteElementString("Interlaced", "true");
        writer.WriteElementString("Margin", "true");
        writer.WriteElementString("Visible", "true");
        writer.WriteElementString("Scalar", "true");

        writer.WriteEndElement(); // end of Axis
        writer.WriteEndElement();
        #endregion ValueAxis

        #region CategoryGroupings
        writer.WriteStartElement("CategoryGroupings");
        writer.WriteStartElement("CategoryGrouping");
        writer.WriteStartElement("DynamicCategories");
        for (int Xh = 0; Xh < strXFields.Length / strXFields.Rank; Xh++)
        {
            writer.WriteStartElement("Grouping");
            writer.WriteAttributeString("Name", null, strXFields[Xh, 0]);
            writer.WriteStartElement("GroupExpressions");
            writer.WriteElementString("GroupExpression", "=Fields!" + strXFields[Xh, 0] + ".Value");
            writer.WriteEndElement();
            writer.WriteEndElement(); // end of Grouping 
            writer.WriteElementString("Label", "=Fields!" + strXFields[Xh, 0] + ".Value");
        }
        
        writer.WriteEndElement(); // end of DynamicCategories
        writer.WriteEndElement(); // end of CategoryGrouping
        writer.WriteEndElement();
        #endregion CategoryGroupings

        writer.WriteStartElement("SeriesGroupings");
        writer.WriteStartElement("SeriesGrouping");
        writer.WriteStartElement("StaticSeries");
        writer.WriteStartElement("StaticMember");
        writer.WriteElementString("Label", strSeriesCaption );
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement(); // end of SeriesGroupings

        writer.WriteStartElement("Style");
        writer.WriteElementString("BackgroundColor", "White");
        writer.WriteEndElement();
        writer.WriteEndElement(); // end of Element Chart

        writer.WriteEndElement(); // end of ReportItems
        writer.WriteElementString("Height", strReportHeight); //19 //XXXXX
        writer.WriteEndElement();
        #endregion  Element Body
        
        writer.WriteEndElement();
        #endregion Report
        writer.Flush();
        stream.Close();
    }
    #endregion Generate Report Method



    public enum ChartTypeList { Area = 1, Bar, Bubble, Column, Doughnut, Line, Pie, Scatter, Stock }
    public enum ChartSubTypeList { Plain = 0, Candlestick = 1, Exploded, HighLowClose, Line, OpenHighLowClode, PercentStacked, Smooth, SmoothLine, Stacked}
    public enum MarkerTypeList {None = 0, Auto = 1, Circle, Cross, Diamond, Square, Triangle}
    public enum PaletteTypeList { Default, EarthTones, Excel, GrayScale, Light, Pastel, SemiTransparent};
    string[]    strChartType = { "", "Area", "Bar", "Bubble", "Column", "Doughnut", "Line", "Pie", "Scatter", "Stock" };
    string[]    strChartSubType = { "Plain", "Candlestick", "Exploded", "HighLowClose", "Line", "OpenHighLowClode", "PercentStacked", "Smooth", "SmoothLine", "Stacked"};
    string[]    strMarkerType = { "None", "Auto", "Circle", "Cross", "Diamond", "Square", "Triangle" };
    string[]    strPaletteType = { "Default", "EarthTones", "Excel", "GrayScale", "Light", "Pastel", "SemiTransparent" };
    string[,]   strXFields, strYFields;
    int         intChartType = 0, intChartSubType = 0, intMarkerType, intMarkerSize = 3, intPaletteType = 1;
    string      strChart3D = "false", strMin, strMax, strDecsription, strDatasetName,
                strXAxisCaption, strYAxisCaption, strSeriesCaption, strReportWidth = "20cm",
                strReportHeight = "10cm";

    #region Properties
    public ChartTypeList ChartType
    {
        set { intChartType = (int)value; }
    }
    public ChartSubTypeList ChartSubType
    {
        set { intChartSubType = (int)value; }
    }
    public bool Chart3D{
        set { strChart3D = value ? "true" : "false";}
    }

    public MarkerTypeList MarkerType
    {
        set {intMarkerType = (int)value;}
    }
    public PaletteTypeList PaletteType
    {
        set { intPaletteType = (int)value; }
    }

    public int MarkerSize
    {
        set { intMarkerSize = value; }
    }

    public string Min
    {
        set { strMin = value; }
    }

    public string Max
    {
        set { strMax = value; }
    }

    public string GraphDescrition
    {
        set { strDecsription = value; }
    }

    public string DatasetName
    {
        set { strDatasetName = value; }
    }

    public string XAxisCaption
    {
        set { strXAxisCaption = value;}
    }

    public string YAxisCaption
    {
        set { strYAxisCaption = value; }
    }

    public string [,] XFields{
        set { strXFields = value; }
    }

    public string[,] YFields
    {
        set { strYFields = value; }
    }

    public string SeriesCaption
    {
        set {strSeriesCaption = value;}
    }

    public int ReportWidth
    {
        set { strReportWidth = value.ToString() + "cm"; }
    }

    public int ReportHeight
    {
        set { strReportHeight = value.ToString() + "cm"; }
    }

    #endregion Properties
 * /**/
}
/**/
