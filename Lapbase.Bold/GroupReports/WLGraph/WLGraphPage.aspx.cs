using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Drawing2D;
using dotnetCHARTING;

public partial class Reports_WLGraph_WLGraphPage : System.Web.UI.Page
{
    #region Global Variables
    GlobalClass gClass = new GlobalClass();
    DataSet dsReport = new DataSet("dsSchema");
    string strMax = "", strMin = "";
    #endregion 

    //------------------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        try
        {
            gClass.OrganizationCode = Request.Cookies["OrganizationCode"].Value;
            txtHCulture.Value = Request.Cookies["CultureInfo"].Value;
            txtHApplicationURL.Value = Request.Url.Scheme + "://" + Request.Url.Host + "/";
            if (gClass.IsUserLogoned(Session.SessionID, Request.Cookies["UserPracticeCode"].Value, Request.Url.Host))
            {
                if (!IsPostBack)
                {
                    FetchWLData();
                    SetChartConfig();
                }
            }
            else
                gClass.ReturnToLoginPage(Request.Url.Host, Request.Cookies["LanguageCode"].Value, Response);
        }
        catch (Exception err)
        {
            string strLanguageCode;
            try
            {
                strLanguageCode = Request.Cookies["LanguageCode"].Value;
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "WL Graph", "Loading Files Data (Page_Load function)", err.ToString());
            }
            catch { strLanguageCode = "en-US"; }
            gClass.ReturnToLoginPage(Request.Url.Host, strLanguageCode, Response);
        }
        return;
    }

    //------------------------------------------------------------------------------------------------------------
    private void FetchWLData()
    {
        SqlCommand cmdSelect = new SqlCommand(); 

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Rep_EWL_WLGraphFullPage", true);
        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True");

        dsReport.Tables.Add(gClass.FetchData(cmdSelect, "tblReport").Tables[0].Copy());
        if (dsReport.Tables[0].Rows.Count > 0)  LoadWLData();
    }

    //------------------------------------------------------------------------------------------------------------
    private void LoadWLData()
    {
        DataView dvReport = dsReport.Tables[0].DefaultView;

        valueOfPatientName.Text = dvReport[0]["PatientName"].ToString();
        valueOfAGE.Text = dvReport[0]["AGE"].ToString();
        if (dvReport[0]["VisitWeeksFlag"].ToString() == "3")
        {
            lblSurgeryDate.Text = "Zero Date : ";
            valueOfLapBandDate.Text = dvReport[0]["strZeroDate"].ToString();
        }
        else
        {
            lblSurgeryDate.Text = "Surgery Date : ";
            valueOfLapBandDate.Text = dvReport[0]["strLapBandDate"].ToString();
        }
        valueOfCurrentWeight.Text = Math.Round (Convert.ToDecimal( dvReport[0]["CurrentWeight"].ToString())).ToString();
        valueOfTargetWeight.Text = Math.Round (Convert.ToDecimal( dvReport[0]["TargetWeight"].ToString())).ToString(); ;
        valueOfInitBMI.Text = dvReport[0]["InitBMI"].ToString();
        valueOfStartWeight.Text = Math.Round(Convert.ToDecimal(dvReport[0]["StartWeight"].ToString())).ToString(); ;
        valueOfStartWeight_Unit.Text = dsReport.Tables[0].Rows[0]["WeightMeasurment"].ToString();
        valueOfCurrentWeight_Unit.Text = dsReport.Tables[0].Rows[0]["WeightMeasurment"].ToString();
        valueOfTargetWeight_Unit.Text = dsReport.Tables[0].Rows[0]["WeightMeasurment"].ToString();
        if (dsReport.Tables.Count > 0)
            gClass.FindMinMax(dsReport.Tables[0].DefaultView, "Weight", ref strMax, ref strMin);
        return;
    }
    /** /
    #region protected void rptViewer_Init
    protected void rptViewer_Init(object sender, EventArgs e)
    {
        ((ReportViewer)sender).ProcessingMode = ProcessingMode.Local;
        ((ReportViewer)sender).LocalReport.EnableExternalImages = false;
        ((ReportViewer)sender).LocalReport.EnableHyperlinks = false;
        ((ReportViewer)sender).ShowBackButton = false;
        ((ReportViewer)sender).ShowDocumentMapButton = true;
        ((ReportViewer)sender).ShowExportControls = true;
        ((ReportViewer)sender).ShowFindControls = false;
        ((ReportViewer)sender).ShowParameterPrompts = false;
        ((ReportViewer)sender).ShowPageNavigationControls = false;
        ((ReportViewer)sender).ShowReportBody = true;
        ((ReportViewer)sender).SizeToReportContent = true;
        ((ReportViewer)sender).ShowToolBar = false;
        ((ReportViewer)sender).ShowZoomControl = false;
        ((ReportViewer)sender).DocumentMapCollapsed = true;
        ((ReportViewer)sender).SizeToReportContent = true;
        ((ReportViewer)sender).Visible = true;
    }
    #endregion 

    #region protected void rptViewer_DataBinding
    protected void rptViewer_DataBinding(object sender, EventArgs e)
    {
        try
        {
            GraphGenerator myGraph = new GraphGenerator();
            strReportPath = Server.MapPath("") + "\\"; //Server.MapPath("Default.aspx").Replace("Default.aspx", "");
            strReportName = Guid.NewGuid().ToString().Trim().Replace("-", "") + ".rdlc";
            string[,] XFields = { { "strDateSeen", "System.String" } };
            string[,] YFields = { { "WEIGHT", "System.Decimal" } };

            myGraph.Chart3D = false;
            myGraph.ChartType = GraphGenerator.ChartTypeList.Line;
            myGraph.ChartSubType = GraphGenerator.ChartSubTypeList.SmoothLine;
            myGraph.MarkerType = GraphGenerator.MarkerTypeList.Circle;
            myGraph.MarkerSize = 5;
            myGraph.Min = strMin;
            myGraph.Max = strMax;
            myGraph.GraphDescrition = "Weight Lost";
            myGraph.DatasetName = dsReport.DataSetName;
            myGraph.XAxisCaption = "Date Seen";
            myGraph.YAxisCaption = "WEIGHT";
            myGraph.XFields = XFields;
            myGraph.YFields = YFields;
            myGraph.SeriesCaption = "WEIGHT Series";
            myGraph.ReportWidth = 25;
            myGraph.ReportHeight = 11;
            myGraph.PaletteType = GraphGenerator.PaletteTypeList.EarthTones;

            myGraph.GenerateRdl(Response, "Weight Loss", strReportPath.Trim(), strReportName);

            ((ReportViewer)sender).LocalReport.DisplayName = "Weight Loss Report";
            ((ReportViewer)sender).LocalReport.ReportPath = strReportPath + strReportName;

            if ((dsReport.Tables.Count > 0) && (dsReport.Tables[0].Rows.Count > 0))
            {
                ((ReportViewer)sender).LocalReport.DataSources.Clear();
                ((ReportViewer)sender).LocalReport.DataSources.Add(new ReportDataSource("dsSchema", dsReport.Tables[0]));
            }
        }
        catch (Exception err)
        {
            Response.Write(err.ToString());
        }
    }
    #endregion 

    #region protected void rptViewer_Unload
    protected void rptViewer_Unload(object sender, EventArgs e)
    {
        ((ReportViewer)sender).Dispose();

        try
        {
            System.IO.FileInfo f = new System.IO.FileInfo(strReportPath + strReportName);
            f.Delete();
        }
        catch { }
    }
    #endregion 
    */

    //------------------------------------------------------------------------------------------------------------
    private void SetChartConfig()
    {
        //chartWL.Title = "Weight Loss";
        chartWL.TempDirectory = "../../temp";
        chartWL.Mentor = false;
        chartWL.Use3D = false;
        chartWL.Width = 800;
        chartWL.Height = 450;
        chartWL.Debug = true;
        chartWL.LegendBox.Visible = false;
        
        chartWL.DefaultSeries.Type = SeriesType.Line;
        chartWL.DefaultSeries.Line.Width = 2;
        chartWL.DefaultSeries.Line.DashStyle = DashStyle.Dash;
        chartWL.XAxis.Label.Text = "Date Seen";
        chartWL.YAxis.Label.Text = "WEIGHT";

        chartWL.XAxis.Scale = Scale.Time;
        chartWL.XAxis.TimeScaleLabels.RangeMode = TimeScaleLabelRangeMode.Dynamic;
        chartWL.XAxis.TimeScaleLabels.Mode = TimeScaleLabelMode.Dynamic;
        chartWL.XAxis.TimeScaleLabels.YearTick.SetAllColors(System.Drawing.Color.DarkRed);

        SeriesCollection mySC = getData();
        chartWL.SeriesCollection.Add(mySC);
        return;
    }

    //------------------------------------------------------------------------------------------------------------
    SeriesCollection getData()
    {
        SeriesCollection SC = new SeriesCollection();
        DateTime dtDateSeen = new DateTime();

        Series s = new Series();
        s.Name = "WL Series";
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Request.Cookies["CultureInfo"].Value, true);
        for (int b = 0; b < dsReport.Tables[0].Rows.Count; b++)
            try
            {
                Element e = new Element();
                dtDateSeen = Convert.ToDateTime(dsReport.Tables[0].Rows[b]["DateSeen"].ToString());
                e.XDateTime = dtDateSeen;
                e.Name = dsReport.Tables[0].Rows[b]["strDateSeen"].ToString();
                e.YValue = Convert.ToDouble(dsReport.Tables[0].Rows[b]["WEIGHT"].ToString());
                s.Elements.Add(e);
            }
            catch { }
        SC.Add(s);
        SC[0].PaletteName = Palette.Two;//.Color = Color.FromArgb(49,255,49);
        return SC;
    }
}
