#region Using Modules
using System;
using System.Data;
//using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using dotnetCHARTING;
#endregion Using Modules

public partial class Reports_IEWLGraph_IEWLGraphPage : System.Web.UI.Page
{
    #region Global Variables
    GlobalClass gClass = new GlobalClass();
    DataSet dsReport = new DataSet("dsSchema");
    DateTime firstVisitDate;
    DateTime lastVisitDate;
    DateTime lastVisitDatePrev;
    Boolean nextVisitExist = false;
    int monthDiff;
    bool dataExist = true;

    int[] month = new int[] { 0, 3, 6, 9, 12, 18, 24 };
    Double[] idealWeightLoss = new Double[] { 0, 30, 38, 45, 51, 60, 66 };
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
                    FetchIEWLData();
                    if (dataExist == true)
                    {
                        SetChartConfig();

                        if (nextVisitExist == true)
                            SetChartConfigNext();
                        else
                        {
                            lblNextVisit.Style["display"] = "none";
                            chartIEWLNext.Style["display"] = "none";
                        }
                    }
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
                gClass.AddErrorLogData(Request.Cookies["UserPracticeCode"].Value, Request.Url.Host, Request.Cookies["Logon_UserName"].Value, "IEWL Graph", "Loading Files Data (Page_Load function)", err.ToString());
            }
            catch { strLanguageCode = "en-US"; }
            gClass.ReturnToLoginPage(Request.Url.Host, strLanguageCode, Response);
        }
    }

    //------------------------------------------------------------------------------------------------------------
    #region private void FetchIEWLData
    private void FetchIEWLData()
    {
        SqlCommand cmdSelect = new SqlCommand();
        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Rep_EWL_WLGraphFullPage", true);

        cmdSelect.Parameters.Add("@OrganizationCode", SqlDbType.Int).Value = Convert.ToInt32(gClass.OrganizationCode);
        cmdSelect.Parameters.Add("@UserPracticeCode", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserPracticeCode"].Value);
        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);
        cmdSelect.Parameters.Add("@ImperialFlag", SqlDbType.Bit).Value = Request.Cookies["Imperial"].Value.Equals("True"); 
        
        dsReport.Tables.Add(gClass.FetchData(cmdSelect, "tblReport").Tables[0].Copy());
        if (dsReport.Tables[0].Rows.Count > 0)
        {
            //AddDateSeenDate();
            LoadIEWLData();
        }
    }
    #endregion

    #region private void AddDateSeenDate
    private void AddDateSeenDate()
    {
        DataColumn tempDC = new DataColumn();

        tempDC.DataType = Type.GetType("System.DateTime");
        tempDC.Caption = "VisitDate";
        tempDC.ColumnName = "VisitDate";
        dsReport.Tables[0].Columns.Add(tempDC);

        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Context.Request.Cookies["CultureInfo"].Value, true);
        //System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(GlobalClass.CultureInfo, true);
        for (int Xh = 0; Xh < dsReport.Tables[0].Rows.Count; Xh++)
        {
            dsReport.Tables[0].Rows[Xh]["VisitDate"] = Convert.ToDateTime(dsReport.Tables[0].Rows[Xh]["DateSeen"].ToString());
        }
        dsReport.AcceptChanges();
    }
    #endregion

    #region private void LoadIEWLData
    private void LoadIEWLData()
    {
        DataView    dvReport = dsReport.Tables[0].DefaultView;

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

        valueOfCurrentWeight.Text = Math.Round(Convert.ToDecimal(dvReport[0]["CurrentWeight"].ToString())).ToString();
        valueOfTargetWeight.Text = Math.Round(Convert.ToDecimal(dvReport[0]["TargetWeight"].ToString())).ToString();
        valueOfInitBMI.Text = dvReport[0]["InitBMI"].ToString();
        valueOfStartWeight.Text = Math.Round(Convert.ToDecimal(dvReport[0]["StartWeight"].ToString())).ToString();
        valueOfStartWeight_Unit.Text = dsReport.Tables[0].Rows[0]["WeightMeasurment"].ToString();
        valueOfCurrentWeight_Unit.Text = dsReport.Tables[0].Rows[0]["WeightMeasurment"].ToString();
        valueOfTargetWeight_Unit.Text = dsReport.Tables[0].Rows[0]["WeightMeasurment"].ToString();

        try
        {
            //do this check if there are no visit data
            if (dvReport[0]["VisitWeeksFlag"].ToString() == "3")
            {
                firstVisitDate = Convert.ToDateTime(dvReport[0]["strZeroDate"].ToString());
            }
            else
            {
                firstVisitDate = Convert.ToDateTime(dvReport[0]["strLapBandDate"].ToString());
            }
        }
        catch (Exception err) 
        {
            dataExist = false;
        }
        //if (dsReport.Tables.Count > 0)
        //    gClass.FindMinMax(dsReport.Tables[0].DefaultView, "EWL", ref strMax, ref strMin);
        return;
    }
    #endregion

    private void SetChartConfig()
    {
        //chartEWL.Title = "% Excess Weight Loss";
        chartIEWL.TempDirectory = "../../temp";
        chartIEWL.Mentor = false;
        chartIEWL.Use3D = false;
        chartIEWL.Width = 800;
        chartIEWL.Height = 450;
        chartIEWL.Debug = true;
        chartIEWL.LegendBox.Visible = true;
        chartIEWL.LegendBox.Orientation = dotnetCHARTING.Orientation.Bottom;

        chartIEWL.DefaultSeries.Type = SeriesType.Line;
        chartIEWL.DefaultElement.Marker.Size = 5;
        chartIEWL.DefaultSeries.Line.Width = 2;
        chartIEWL.DefaultSeries.Line.DashStyle = DashStyle.Solid;

        chartIEWL.XAxis.Label.Text = "Date Seen";
        chartIEWL.YAxis.Label.Text = "% EWL";

        chartIEWL.XAxis.Scale = Scale.Time;
        chartIEWL.XAxis.TimeScaleLabels.RangeMode = TimeScaleLabelRangeMode.Default;
        chartIEWL.XAxis.TimeScaleLabels.Mode = TimeScaleLabelMode.Default;
        chartIEWL.XAxis.TimeScaleLabels.MonthFormatString = "MMM";
        chartIEWL.XAxis.TimeScaleLabels.YearTick.SetAllColors(Color.DarkRed);
        
        SeriesCollection actualEWL = getEWLData();
        SeriesCollection idealEWL = getIdealWeightLossData();
        chartIEWL.SeriesCollection.Add(actualEWL);
        chartIEWL.SeriesCollection.Add(idealEWL);

        /*
        //DEMO SETTING
        string url = Request.Url.Host;
        if (url.ToUpper().IndexOf("DEMO") >= 0 || url.ToUpper().IndexOf("FERRY") >= 0 || url.ToUpper().IndexOf(".0.105") >= 0)
        {
            if (Request.QueryString["Param"] == "1")
            {
                SeriesCollection idealActualEWL = getIdealActualWeightLossData();
                chartIEWL.SeriesCollection.Add(idealActualEWL);
            }
        }
        */

        return;
    }

    private void SetChartConfigNext()
    {
        //chartEWL.Title = "% Excess Weight Loss";
        chartIEWLNext.TempDirectory = "../../temp";
        chartIEWLNext.Mentor = false;
        chartIEWLNext.Use3D = false;
        chartIEWLNext.Width = 800;
        chartIEWLNext.Height = 450;
        chartIEWLNext.Debug = true;
        chartIEWLNext.LegendBox.Visible = true;
        chartIEWLNext.LegendBox.Orientation = dotnetCHARTING.Orientation.Bottom;

        chartIEWLNext.DefaultSeries.Type = SeriesType.Line;
        chartIEWLNext.DefaultElement.Marker.Size = 5;
        chartIEWLNext.DefaultSeries.Line.Width = 2;
        chartIEWLNext.DefaultSeries.Line.DashStyle = DashStyle.Solid;

        chartIEWLNext.XAxis.Label.Text = "Date Seen";
        chartIEWLNext.YAxis.Label.Text = "% EWL";

        chartIEWLNext.XAxis.Scale = Scale.Time;
        chartIEWLNext.XAxis.TimeScaleLabels.RangeMode = TimeScaleLabelRangeMode.Default;
        chartIEWLNext.XAxis.TimeScaleLabels.Mode = TimeScaleLabelMode.Default;
        chartIEWLNext.XAxis.TimeScaleLabels.MonthFormatString = "MMM";
        chartIEWLNext.XAxis.TimeScaleLabels.YearTick.SetAllColors(Color.DarkRed);

        SeriesCollection actualEWL = getEWLDataNext();
        SeriesCollection idealEWL = getIdealWeightLossDataNext();
        chartIEWLNext.SeriesCollection.Add(actualEWL);
        chartIEWLNext.SeriesCollection.Add(idealEWL);
        /*
        //DEMO SETTING
        string url = Request.Url.Host;
        if (url.ToUpper().IndexOf("DEMO") >= 0 || url.ToUpper().IndexOf("FERRY") >= 0 || url.ToUpper().IndexOf(".0.105") >= 0)
        {
            if (Request.QueryString["Param"] == "1")
            {
                SeriesCollection idealActualEWL = getIdealActualWeightLossDataNext();
                chartIEWLNext.SeriesCollection.Add(idealActualEWL);
            }
        }
        */
        return;
    }

    //------------------------------------------------------------------------------------------------------------
    SeriesCollection getEWLData()
    {
        SeriesCollection SC = new SeriesCollection();
        DateTime dtDateSeen = new DateTime();
        DateTime maxDateSeen = new DateTime();
        Boolean firstVisit = true;
        Series s = new Series();

        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CultureInfo"].Value, true);
        for (int b = 0; b < dsReport.Tables[0].Rows.Count; b++)
        {
            try
            {
                dtDateSeen = Convert.ToDateTime(dsReport.Tables[0].Rows[b]["DateSeen"].ToString());
                if (firstVisit == false)
                {
                    if (dtDateSeen > maxDateSeen)
                    {
                        nextVisitExist = true;
                        break;
                    }
                }
                Element e = new Element();
                e.Name = dsReport.Tables[0].Rows[b]["DateSeen"].ToString(); //VisitDate
                e.XDateTime = dtDateSeen;
                e.YValue = Convert.ToDouble(dsReport.Tables[0].Rows[b]["EWL"].ToString());

                //e.ShowValue = true;
                s.Elements.Add(e);
                if (firstVisit == true)
                {
                    firstVisitDate = dtDateSeen;
                    maxDateSeen = dtDateSeen.AddMonths(24);
                    firstVisit = false;
                }
                lastVisitDatePrev = dtDateSeen;
            }
            catch { }
        }

        monthDiff = 24 - lastVisitDatePrev.Month;

        s.LegendEntry.Marker.Size = 5;
        s.LegendEntry.Value = "";
        s.LegendEntry.Name = "% Actual EWL";
        SC.Add(s);

        SC[0].DefaultElement.Color = Color.Blue;
        return SC;
    }
    
    SeriesCollection getEWLDataNext()
    {
        SeriesCollection SC = new SeriesCollection();
        DateTime dtDateSeen = new DateTime();
        DateTime minDateSeen = new DateTime();
        Series s = new Series();
        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CultureInfo"].Value, true);
        for (int b = 0; b < dsReport.Tables[0].Rows.Count; b++)
        {
            try
            {
                dtDateSeen = Convert.ToDateTime(dsReport.Tables[0].Rows[b]["DateSeen"].ToString());
                if (dtDateSeen >= lastVisitDatePrev)
                {
                    Element e = new Element();
                    e.Name = dsReport.Tables[0].Rows[b]["DateSeen"].ToString(); //VisitDate
                    e.XDateTime = dtDateSeen;
                    e.YValue = Convert.ToDouble(dsReport.Tables[0].Rows[b]["EWL"].ToString());

                    s.Elements.Add(e);
                    lastVisitDate = dtDateSeen;
                }
            }
            catch { }
        }
        s.LegendEntry.Marker.Size = 5;
        s.LegendEntry.Value = "";
        s.LegendEntry.Name = "% Actual EWL";
        SC.Add(s);

        SC[0].DefaultElement.Color = Color.Blue;
        return SC;
    }
    //------------------------------------------------------------------------------------------------------------
    SeriesCollection getIdealWeightLossData()
    {
        SeriesCollection SC = new SeriesCollection();
        DateTime dtDateSeen = new DateTime();

        Series s = new Series();
        double value = 0;
        string url = Request.Url.Host;

        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CultureInfo"].Value, true);
        for (int b = 0; b < month.Length ; b++)
        {
            try
            {
                Element e = new Element();
                dtDateSeen = firstVisitDate.AddMonths(month[b]);
                e.XDateTime = dtDateSeen;
                e.YValue = Convert.ToDouble(idealWeightLoss[b]);

                //plot textbox to show the actual weight loss target
                if (Request.QueryString["Param"] == "1")
                {
                    e.ShowValue = true;
                    value = Math.Round(Convert.ToDouble(dsReport.Tables[0].Rows[0]["StartWeight"].ToString()) - ((Convert.ToDouble(dsReport.Tables[0].Rows[0]["StartWeight"].ToString()) - Convert.ToDouble(dsReport.Tables[0].Rows[0]["IdealWeight"].ToString())) * Convert.ToDouble(idealWeightLoss[b]) / 100));
                    e.SmartLabel.Text = value.ToString("#") + " " + dsReport.Tables[0].Rows[0]["WeightMeasurment"].ToString();
                    e.SmartLabel.Color = Color.DarkGreen;
                }
                s.Elements.Add(e);
            }
            catch { }
        }
        s.LegendEntry.Marker.Size = 5;
        s.LegendEntry.Value = "";
        s.LegendEntry.Name = "% Target EWL";
        SC.Add(s);
        SC[0].DefaultElement.Color = Color.Green;
        return SC;
    }
    //------------------------------------------------------------------------------------------------------------
    SeriesCollection getIdealActualWeightLossData()
    {
        //not used
        SeriesCollection SC = new SeriesCollection();
        DateTime dtDateSeen = new DateTime();

        Series s = new Series();
        Double value = 0;

        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CultureInfo"].Value, true);
        for (int b = 0; b < month.Length; b++)
        {
            try
            {
                Element e = new Element();
                dtDateSeen = firstVisitDate.AddMonths(month[b]);
                e.XDateTime = dtDateSeen;
                value = ((Convert.ToDouble(dsReport.Tables[0].Rows[0]["StartWeight"].ToString()) - Convert.ToDouble(dsReport.Tables[0].Rows[0]["IdealWeight"].ToString())) * Convert.ToDouble(idealWeightLoss[b]) / 100);
                e.YValue = value;

                if (b > 0)
                {
                    e.ShowValue = true;
                    e.SmartLabel.Text = value.ToString("#.##") + " " + dsReport.Tables[0].Rows[0]["WeightMeasurment"].ToString();
                }
                s.Elements.Add(e);
            }
            catch { }
        }
        s.LegendEntry.Marker.Size = 5;
        s.LegendEntry.Value = "";
        s.LegendEntry.Name = "Target Weight Loss";
        SC.Add(s);
        SC[0].DefaultElement.Color = Color.Turquoise;
        return SC;
    }

    SeriesCollection getIdealWeightLossDataNext()
    {
        SeriesCollection SC = new SeriesCollection();
        DateTime dtDateSeen = new DateTime();
        int count = 0;
        double value = 0;

        string monthNextStr = "";
        string idealWeightLossNextStr = "";

        string[] monthNextArr;
        string[] idealWeightLossNextArr;

        Series s = new Series();
        string url = Request.Url.Host;

        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CultureInfo"].Value, true);
        for (int b = (month.Length -1); b >= 0; b--)
        {
            if (month[b] >= monthDiff)
            {
                try
                {
                    monthNextStr = month[b] + "*" + monthNextStr;
                    idealWeightLossNextStr = idealWeightLoss[b] + "*" + idealWeightLossNextStr;
                }
                catch { }
            }
        }
        monthNextArr = monthNextStr.Split('*');
        idealWeightLossNextArr = idealWeightLossNextStr.Split('*');

        for (int b = 0; b < monthNextArr.Length; b++)
        {
            try
            {
                Element e = new Element();
                dtDateSeen = firstVisitDate.AddMonths(Convert.ToInt32(monthNextArr[b]));
                e.XDateTime = dtDateSeen;
                e.YValue = Convert.ToDouble(Convert.ToDouble(idealWeightLossNextArr[b]));

                //plot textbox to show the actual weight loss target
                if (Request.QueryString["Param"] == "1")
                {
                    e.ShowValue = true;
                    value = Math.Round(Convert.ToDouble(dsReport.Tables[0].Rows[b]["StartWeight"].ToString()) - ((Convert.ToDouble(dsReport.Tables[0].Rows[b]["StartWeight"].ToString()) - Convert.ToDouble(dsReport.Tables[0].Rows[b]["IdealWeight"].ToString())) * Convert.ToDouble(Convert.ToDouble(idealWeightLossNextArr[b])) / 100));
                    e.SmartLabel.Text = value.ToString("#") + " " + dsReport.Tables[0].Rows[b]["WeightMeasurment"].ToString();
                    e.SmartLabel.Color = Color.DarkGreen;
                }
                s.Elements.Add(e);
            }
            catch { }
        }
        try
        {
            //add last point
            Element e = new Element();
            e.XDateTime = lastVisitDate;
            e.YValue = Convert.ToDouble(66);
            s.Elements.Add(e);
        }
        catch { }

        s.LegendEntry.Marker.Size = 5;
        s.LegendEntry.Value = "";
        s.LegendEntry.Name = "% Target EWL";
        SC.Add(s);
        SC[0].DefaultElement.Color = Color.Green;
        return SC;
    }

    SeriesCollection getIdealActualWeightLossDataNext()
    {
        //not used
        SeriesCollection SC = new SeriesCollection();
        DateTime dtDateSeen = new DateTime();
        int count = 0;
        Double value = 0;

        string monthNextStr = "";
        string idealWeightLossNextStr = "";

        string[] monthNextArr;
        string[] idealWeightLossNextArr;

        Series s = new Series();

        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(Request.Cookies["CultureInfo"].Value, true);
        for (int b = (month.Length - 1); b >= 0; b--)
        {
            if (month[b] >= monthDiff)
            {
                try
                {
                    monthNextStr = month[b] + "*" + monthNextStr;
                    idealWeightLossNextStr = idealWeightLoss[b] + "*" + idealWeightLossNextStr;
                }
                catch { }
            }
        }
        monthNextArr = monthNextStr.Split('*');
        idealWeightLossNextArr = idealWeightLossNextStr.Split('*');

        for (int b = 0; b < monthNextArr.Length; b++)
        {
            try
            {
                Element e = new Element();
                dtDateSeen = firstVisitDate.AddMonths(Convert.ToInt32(monthNextArr[b]));
                e.XDateTime = dtDateSeen;
                value = ((Convert.ToDouble(dsReport.Tables[0].Rows[b]["StartWeight"].ToString()) - Convert.ToDouble(dsReport.Tables[0].Rows[b]["IdealWeight"].ToString())) * Convert.ToDouble(Convert.ToDouble(idealWeightLossNextArr[b])) / 100);
                e.YValue = value;
                e.ShowValue = true;
                e.SmartLabel.Text = value.ToString("#.##") + " " + dsReport.Tables[0].Rows[b]["WeightMeasurment"].ToString();

                s.Elements.Add(e);
            }
            catch { }
        }
        try
        {
            //add last point
            //Element e = new Element();
            //e.XDateTime = lastVisitDate;
            //e.YValue = value;
            //s.Elements.Add(e);
        }
        catch { }

        s.LegendEntry.Marker.Size = 5;
        s.LegendEntry.Value = "";
        s.LegendEntry.Name = "Target Weight Loss";
        SC.Add(s);
        SC[0].DefaultElement.Color = Color.Turquoise;
        return SC;
    }
}
