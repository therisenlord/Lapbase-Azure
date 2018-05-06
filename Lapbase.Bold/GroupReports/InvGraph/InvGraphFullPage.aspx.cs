using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using dotnetCHARTING;

public partial class Reports_InvGraph_InvGraphFullPage : System.Web.UI.Page
{
    DataSet dsReport = new DataSet("dsSchema");
    
    //------------------------------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if (!IsPostBack)
        {
            FetchEWLData();
            SetEWLChartConfig_Data();
            SetHBA1C_Insulin_GlucoseChartConfig_Data();
            SetLIPIDSChartConfig_Data();
            SetBloodPressureChartConfig_Data();
            SetHematologyChartConfig_Data();
            SetLiveFunctionTestsConfig_Data();
            SetFatMassConfig_Data();
            //chartHematology
        }
    }

    //------------------------------------------------------------------------------------------------------------
    private void FetchEWLData()
    {
        SqlCommand      cmdSelect = new SqlCommand(), cmdSysConfig = new SqlCommand(), cmdINVSelect = new SqlCommand();
        GlobalClass     gClass = new GlobalClass();

        gClass.MakeStoreProcedureName(ref cmdSelect, "sp_Rep_EWLGraphFullPage", true);
        gClass.MakeStoreProcedureName(ref cmdSysConfig, "sp_SystemConfig", true);
        gClass.MakeStoreProcedureName(ref cmdINVSelect, "sp_Rep_InvestigationGraph", true);

        cmdSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);
        cmdINVSelect.Parameters.Add("@PatientID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PID"]);

        dsReport.Tables.Add(gClass.FetchData(cmdSelect, "tblEWL").Tables[0].Copy());
        dsReport.Tables.Add(gClass.FetchData(cmdSysConfig, "tblSysConfig").Tables[0].Copy());
        dsReport.Tables.Add(gClass.FetchData(cmdINVSelect, "tblInvestigation").Tables[0].Copy());

        if (dsReport.Tables[0].Rows.Count > 0)
            LoadEWLData();
    }

    //------------------------------------------------------------------------------------------------------------
    private void LoadEWLData()
    {
        DataView dvReport = dsReport.Tables[0].DefaultView;

        valueOfPatientName.Text = dvReport[0]["PatientName"].ToString();
        valueOfAGE.Text = dvReport[0]["AGE"].ToString();
        valueOfLapBandDate.Text = dvReport[0]["strLapBandDate"].ToString();
        return;
    }

    //------------------------------------------------------------------------------------------------------------
    private void SetEWLChartConfig_Data()
    {
        SeriesCollection mySC = getEWLData();

        chartEWL.Title = "% Excess Weight Loss";
        chartEWL.TempDirectory = "../../temp";
        chartEWL.Mentor = false;
        chartEWL.Use3D = false;
        chartEWL.Width = 800;
        chartEWL.Height = 300;
        chartEWL.Debug = true;
        chartEWL.DefaultSeries.Type = SeriesType.Spline;
        chartEWL.XAxis.Label.Text = "Date Seen";
        chartEWL.YAxis.Label.Text = "% EWL";
        
        chartEWL.SeriesCollection.Add(mySC);
        return;
    }

    //------------------------------------------------------------------------------------------------------------
    private SeriesCollection getEWLData()
    {
        SeriesCollection SC = new SeriesCollection();
        Series s = new Series();

        s.Name = "Series EWL";
        for (int Xh = 0; Xh < dsReport.Tables[0].Rows.Count; Xh++)
        {
            Element e = new Element();
            //e.Name = "Element " + (b + 1).ToString();
            e.Name = dsReport.Tables[0].Rows[Xh]["strDateSeen"].ToString();
            //e.YValue = myR.Next(50);
            e.YValue = Convert.ToDouble(dsReport.Tables[0].Rows[Xh]["EWL"].ToString());
            s.Elements.Add(e);
        }
        SC.Add(s);
        SC[0].PaletteName = Palette.Two;
        return SC;
    }

    //------------------------------------------------------------------------------------------------------------
    private void SetHBA1C_Insulin_GlucoseChartConfig_Data()
    {
        chartHBA_Insulin_Glucose.Title="HBA1C, Serum Insulin and Blood Glucose";
        chartHBA_Insulin_Glucose.Depth = 15;
        chartHBA_Insulin_Glucose.Use3D = false;
        chartHBA_Insulin_Glucose.XAxis.ClusterColumns = false;
        chartHBA_Insulin_Glucose.DefaultSeries.DefaultElement.Transparency = 20;
        chartHBA_Insulin_Glucose.DefaultSeries.Type = SeriesType.Spline;
        chartHBA_Insulin_Glucose.YAxis.Scale = Scale.Normal;
        chartHBA_Insulin_Glucose.XAxis.Label.Text = "Date Seen";
        chartHBA_Insulin_Glucose.YAxis.Label.Text = "Amount";
        chartHBA_Insulin_Glucose.TempDirectory = "../../temp";
        chartHBA_Insulin_Glucose.Width = 600;
        chartHBA_Insulin_Glucose.Height = 350;
        chartHBA_Insulin_Glucose.SeriesCollection.Add(getHBA_Insulin_GlucoseData());
    }

    //------------------------------------------------------------------------------------------------------------
    private SeriesCollection getHBA_Insulin_GlucoseData()
    {
        SeriesCollection SC = new SeriesCollection();
        DataView MyDV = dsReport.Tables["tblInvestigation"].DefaultView;

        MyDV.RowFilter = "FBloodGlucose > 0";
        MyDV.RowStateFilter = DataViewRowState.OriginalRows;

        for (int Xh = 0; Xh < 3; Xh++)
        {
            Series s = new Series();

            for (int Idx = 0; Idx < MyDV.Count; Idx++)
            {
                Element e = new Element();
                e.Name = MyDV[Idx]["strLapBandDate"].ToString();
                switch (Xh)
                {
                    case 0 :
                        s.Name = "HBA1C";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["HBA1C"].ToString());
                        break;
                    case 1 :
                        s.Name = "Serum Insulin";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["FSerumInsulin"].ToString());
                        break;
                    case 2 :
                        s.Name = "Blood Glucose";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["FBloodGlucose"].ToString());
                        break;
                }
                s.Elements.Add(e);
            }
            SC.Add(s);
        }
        SC[0].DefaultElement.Color = Color.FromArgb(49, 255, 49);
        SC[1].DefaultElement.Color = Color.FromArgb(255, 255, 0);
        SC[2].DefaultElement.Color = Color.FromArgb(255, 99, 49);
        return SC;
    }


    //------------------------------------------------------------------------------------------------------------
    private void SetLIPIDSChartConfig_Data()
    {
        chartLIPIDS.Title = "LIPIDS";
        chartLIPIDS.Depth = 15;
        chartLIPIDS.Use3D = false;
        chartLIPIDS.XAxis.ClusterColumns = false;
        chartLIPIDS.DefaultSeries.DefaultElement.Transparency = 20;
        chartLIPIDS.DefaultSeries.Type = SeriesType.Spline;
        chartLIPIDS.YAxis.Scale = Scale.Normal;
        chartLIPIDS.XAxis.Label.Text = "Date Seen";
        chartLIPIDS.YAxis.Label.Text = "Amount";
        chartLIPIDS.TempDirectory = "../../temp";
        chartLIPIDS.Width = 600;
        chartLIPIDS.Height = 350;
        chartLIPIDS.SeriesCollection.Add(getLIPIDSData());
    }

    //------------------------------------------------------------------------------------------------------------
    private SeriesCollection getLIPIDSData()
    {
        SeriesCollection SC = new SeriesCollection();
        DataView MyDV = dsReport.Tables["tblInvestigation"].DefaultView;

        MyDV.RowFilter = "Triglycerides > 0";
        MyDV.RowStateFilter = DataViewRowState.OriginalRows;

        for (int Xh = 0; Xh < 3; Xh++)
        {
            Series s = new Series();

            for (int Idx = 0; Idx < MyDV.Count; Idx++)
            {
                Element e = new Element();
                e.Name = MyDV[Idx]["strLapBandDate"].ToString();
                switch (Xh)
                {
                    case 0:
                        s.Name = "Triglycerides";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["Triglycerides"].ToString());
                        break;
                    case 1:
                        s.Name = "Total Cholesterol";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["TotalCholesterol"].ToString());
                        break;
                    case 2:
                        s.Name = "HDL Cholesterol";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["HDLCholesterol"].ToString());
                        break;
                }
                s.Elements.Add(e);
            }
            SC.Add(s);
        }
        SC[0].DefaultElement.Color = Color.FromArgb(49, 255, 49);
        SC[1].DefaultElement.Color = Color.FromArgb(255, 255, 0);
        SC[2].DefaultElement.Color = Color.FromArgb(255, 99, 49);
        return SC;
    }

    //------------------------------------------------------------------------------------------------------------
    private void SetBloodPressureChartConfig_Data()
    {
        chartBloodPressure.Title = "Blood Pressure";
        chartBloodPressure.Depth = 15;
        chartBloodPressure.Use3D = false;
        chartBloodPressure.XAxis.ClusterColumns = false;
        chartBloodPressure.DefaultSeries.DefaultElement.Transparency = 20;
        chartBloodPressure.DefaultSeries.Type = SeriesType.Spline;
        chartBloodPressure.YAxis.Scale = Scale.Normal;
        chartBloodPressure.XAxis.Label.Text = "Date Seen";
        chartBloodPressure.YAxis.Label.Text = "Amount";
        chartBloodPressure.TempDirectory = "../../temp";
        chartBloodPressure.Width = 600;
        chartBloodPressure.Height = 350;
        chartBloodPressure.SeriesCollection.Add(getBloodPressureData());
    }

    //------------------------------------------------------------------------------------------------------------
    private SeriesCollection getBloodPressureData()
    {
        SeriesCollection SC = new SeriesCollection();
        DataView MyDV = dsReport.Tables["tblInvestigation"].DefaultView;

        MyDV.RowFilter = "SystolicBP > 0";
        MyDV.RowStateFilter = DataViewRowState.OriginalRows;

        for (int Xh = 0; Xh < 2; Xh++)
        {
            Series s = new Series();

            for (int Idx = 0; Idx < MyDV.Count; Idx++)
            {
                Element e = new Element();
                e.Name = MyDV[Idx]["strLapBandDate"].ToString();
                switch (Xh)
                {
                    case 0:
                        s.Name = "Systolic Blood Pressure";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["SystolicBP"].ToString());
                        break;
                    case 1:
                        s.Name = "Diastolic Blood Pressure";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["DiastolicBP"].ToString());
                        break;
                }
                s.Elements.Add(e);
            }
            SC.Add(s);
        }
        SC[0].DefaultElement.Color = Color.FromArgb(49, 255, 49);
        SC[1].DefaultElement.Color = Color.FromArgb(255, 255, 0);
        return SC;
    }


    //------------------------------------------------------------------------------------------------------------
    private void SetHematologyChartConfig_Data()
    {
        chartHematology.Title = "Hematology";
        chartHematology.Depth = 15;
        chartHematology.Use3D = false;
        chartHematology.XAxis.ClusterColumns = false;
        chartHematology.DefaultSeries.DefaultElement.Transparency = 20;
        chartHematology.DefaultSeries.Type = SeriesType.Spline;
        chartHematology.YAxis.Scale = Scale.Normal;
        chartHematology.XAxis.Label.Text = "Date Seen";
        chartHematology.YAxis.Label.Text = "Amount";
        chartHematology.TempDirectory = "../../temp";
        chartHematology.Width = 600;
        chartHematology.Height = 350;
        chartHematology.SeriesCollection.Add(getHematologyData());
    }

    //------------------------------------------------------------------------------------------------------------
    private SeriesCollection getHematologyData()
    {
        SeriesCollection SC = new SeriesCollection();
        DataView MyDV = dsReport.Tables["tblInvestigation"].DefaultView;

        MyDV.RowFilter = "Hemoglobin > 0";
        MyDV.RowStateFilter = DataViewRowState.OriginalRows;

        for (int Xh = 0; Xh < 5; Xh++)
        {
            Series s = new Series();

            for (int Idx = 0; Idx < MyDV.Count; Idx++)
            {
                Element e = new Element();
                e.Name = MyDV[Idx]["strLapBandDate"].ToString();
                switch (Xh)
                {
                    case 0:
                        s.Name = "Hemoglobin";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["Hemoglobin"].ToString());
                        break;
                    case 1:
                        s.Name = "Ferritin";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["Ferritin"].ToString());
                        break;
                    case 2:
                        s.Name = "Iron";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["Iron"].ToString());
                        break;
                    case 3:
                        s.Name = "Folate";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["Folate"].ToString());
                        break;
                    case 4:
                        s.Name = "B12";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["B12"].ToString());
                        break;
                }
                s.Elements.Add(e);
            }
            SC.Add(s);
        }
        SC[0].DefaultElement.Color = Color.FromArgb(49, 255, 49);
        SC[1].DefaultElement.Color = Color.FromArgb(255, 255, 0);
        SC[2].DefaultElement.Color = Color.FromArgb(255, 99, 49);
        SC[3].DefaultElement.Color = Color.FromArgb(0, 156, 255);
        SC[4].DefaultElement.Color = Color.FromArgb(99, 156, 255);
        return SC;
    }

    //------------------------------------------------------------------------------------------------------------
    private void SetLiveFunctionTestsConfig_Data()
    {
        chartLiveFunctionTests.Title = "Live Function Tests";
        chartLiveFunctionTests.Depth = 15;
        chartLiveFunctionTests.Use3D = false;
        chartLiveFunctionTests.XAxis.ClusterColumns = false;
        chartLiveFunctionTests.DefaultSeries.DefaultElement.Transparency = 20;
        chartLiveFunctionTests.DefaultSeries.Type = SeriesType.Spline;
        chartLiveFunctionTests.YAxis.Scale = Scale.Normal;
        chartLiveFunctionTests.XAxis.Label.Text = "Date Seen";
        chartLiveFunctionTests.YAxis.Label.Text = "Amount";
        chartLiveFunctionTests.TempDirectory = "../../temp";
        chartLiveFunctionTests.Width = 600;
        chartLiveFunctionTests.Height = 350;
        chartLiveFunctionTests.SeriesCollection.Add(getLiveFunctionTestsData());
    }

    //------------------------------------------------------------------------------------------------------------
    private SeriesCollection getLiveFunctionTestsData()
    {
        SeriesCollection SC = new SeriesCollection();
        DataView MyDV = dsReport.Tables["tblInvestigation"].DefaultView;

        MyDV.RowFilter = "AlkPhos > 0";
        MyDV.RowStateFilter = DataViewRowState.OriginalRows;

        for (int Xh = 0; Xh < 5; Xh++)
        {
            Series s = new Series();

            for (int Idx = 0; Idx < MyDV.Count; Idx++)
            {
                Element e = new Element();
                e.Name = MyDV[Idx]["strLapBandDate"].ToString();
                switch (Xh)
                {
                    case 0:
                        s.Name = "AlkPhos";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["AlkPhos"].ToString());
                        break;
                    case 1:
                        s.Name = "AST";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["AST"].ToString());
                        break;
                    case 2:
                        s.Name = "ALT";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["ALT"].ToString());
                        break;
                    case 3:
                        s.Name = "GGT";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["GGT"].ToString());
                        break;
                    case 4:
                        s.Name = "Albumin";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["Albumin"].ToString());
                        break;
                }
                s.Elements.Add(e);
            }
            SC.Add(s);
        }
        SC[0].DefaultElement.Color = Color.FromArgb(49, 255, 49);
        SC[1].DefaultElement.Color = Color.FromArgb(255, 255, 0);
        SC[2].DefaultElement.Color = Color.FromArgb(255, 99, 49);
        SC[3].DefaultElement.Color = Color.FromArgb(0, 156, 255);
        SC[4].DefaultElement.Color = Color.FromArgb(99, 156, 255);
        return SC;
    }

    //------------------------------------------------------------------------------------------------------------
    private void SetFatMassConfig_Data()
    {
        chartFatMass.Title = "Fat Mass";
        chartFatMass.Depth = 15;
        chartFatMass.Use3D = false;
        chartFatMass.XAxis.ClusterColumns = false;
        chartFatMass.DefaultSeries.DefaultElement.Transparency = 20;
        chartFatMass.DefaultSeries.Type = SeriesType.Spline;
        chartFatMass.YAxis.Scale = Scale.Normal;
        chartFatMass.XAxis.Label.Text = "Date Seen";
        chartFatMass.YAxis.Label.Text = "Amount";
        chartFatMass.TempDirectory = "../../temp";
        chartFatMass.Width = 600;
        chartFatMass.Height = 350;
        chartFatMass.SeriesCollection.Add(getFatMassData());
    }

    //------------------------------------------------------------------------------------------------------------
    private SeriesCollection getFatMassData()
    {
        SeriesCollection SC = new SeriesCollection();
        DataView MyDV = dsReport.Tables["tblInvestigation"].DefaultView;

        MyDV.RowFilter = "FatPerCent > 0";
        MyDV.RowStateFilter = DataViewRowState.OriginalRows;

        for (int Xh = 0; Xh < 4; Xh++)
        {
            Series s = new Series();

            for (int Idx = 0; Idx < MyDV.Count; Idx++)
            {
                Element e = new Element();
                e.Name = MyDV[Idx]["strLapBandDate"].ToString();
                switch (Xh)
                {
                    case 0:
                        s.Name = "Impedance";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["Impedance"].ToString());
                        break;
                    case 1:
                        s.Name = "Fat percent";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["FatPerCent"].ToString());
                        break;
                    case 2:
                        s.Name = "Free Fat Mass";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["FreeFatMass"].ToString());
                        break;
                    case 3:
                        s.Name = "Total Body Water";
                        e.YValue = Convert.ToDouble(MyDV[Idx]["TotalBodyWater"].ToString());
                        break;
                }
                s.Elements.Add(e);
            }
            SC.Add(s);
        }
        SC[0].DefaultElement.Color = Color.FromArgb(49, 255, 49);
        SC[1].DefaultElement.Color = Color.FromArgb(255, 255, 0);
        SC[2].DefaultElement.Color = Color.FromArgb(255, 99, 49);
        SC[3].DefaultElement.Color = Color.FromArgb(0, 156, 255);
        return SC;
    }
}
