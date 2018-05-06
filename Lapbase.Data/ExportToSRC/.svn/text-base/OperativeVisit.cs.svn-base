using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Lapbase.Data
{
    public class Comorbidity
    {
        private String mstrComorbidityCode
                        , mstrStratificationCode;

        public String ComorbidityCode
        {
            get { return mstrComorbidityCode; }
            set { mstrComorbidityCode = value; }
        }

        public String StratificationCode
        {
            get { return mstrStratificationCode; }
            set { mstrStratificationCode = value; }
        }

        /// <summary>
        /// Initializes a new Comorbidity object.
        /// </summary>
        /// <param name="strComorbidityCode">Comorbidity Code from Bold System Code.</param>
        /// <param name="strStratificationCode">SRC Comorbidity Category Code</param>
        public Comorbidity(String strComorbidityCode, String strStratificationCode)
        {
            this.ComorbidityCode = strComorbidityCode;
            this.StratificationCode = strStratificationCode;
        }
    }

    [Serializable]
    public class OperativeVisit : ObjectBase
    {
        #region Variables & Consts
        const String        M_STR_HYPERTENSION = "HYPERT"
                            , M_STR_CONGESTIVE = "CONGHF"
                            , M_STR_ISCHEMIC = "ISCHHD"
                            , M_STR_ANGINA = "ANGASM"
                            , M_STR_PERIPHERAL = "PEVASD"
                            , M_STR_LOWER = "LOEXED"
                            , M_STR_DVT = "DVTPE" 
                            , M_STR_GLUCOSE = "GLUMET" 
                            , M_STR_LIPIDS = "LIPDYH" 
                            , M_STR_GOUT = "GOUHYP"
                            , M_STR_OBSTRUCTIVE = "OBSSYN" 
                            , M_STR_OBESITY = "OBHSYN" 
                            , M_STR_PULMONARY = "PULHYP" 
                            , M_STR_ASHTMA = "ASTHMA" 
                            , M_STR_GERD = "GERD" 
                            , M_STR_CHOLELITHIASIS = "CHOLEL" 
                            , M_STR_LIVER = "LVRDIS" 
                            , M_STR_BACKPAIN = "BCKPAIN" 
                            , M_STR_MUSCULOSKELETAL = "MUSDIS" 
                            , M_STR_FIBROMYALGIA = "FBMGIA" 
                            , M_STR_POLYCYSTIC = "PLOVSYN" 
                            , M_STR_MENSTRUAL = "MENIRG" 
                            , M_STR_PSYCHOSOCIAL = "PSYIMP" 
                            , M_STR_DEPRESSION = "DEPRSN" 
                            , M_STR_CONFIRMED = "CONMEN" 
                            , M_STR_ALCOHOL = "ALCUSE" 
                            , M_STR_TOBACCO = "TOBUSE" 
                            ;


        private String      mstrSupportGroupFrequency, mstrBoldVisitID;
        private String[]    mstrVitamins
                            , mstrMedications;
        private Int32       mintPatientID
                            , mintOrganizationCode
                            , mintConsultID;
        private DateTime    mdtmVisitDate;
        private Decimal     mdecWeight
                            , mdecHeight;
        Comorbidity[]       comorbiditiesList;
        List<Comorbidity> objComorbidityList = new List<Comorbidity>();
        #endregion

        #region properties
        /// <summary>
        /// Gets ot sets the selected Patient's Id.
        /// </summary>
        public Int32 PatientID
        {
            set { mintPatientID = value; }
        }

        /// <summary>
        /// Gets ot sets the selected Organization's Code.
        /// </summary>
        public Int32 OrganizationCode
        {
            set { mintOrganizationCode = value; }
        }

        /// <summary>
        /// Gets ot sets the selected consult Id.
        /// </summary>
        public Int32 ConsultID
        {
            get { return mintConsultID; }
            set { mintConsultID = value; }
        }

        /// <summary>
        /// Gets or sets the Bold Visit ID
        /// </summary>
        public String VisitID
        {
            get { return mstrBoldVisitID; }
            set { mstrBoldVisitID = value; }
        }

        /// <summary>
        /// Gets the Pre-operative visit date.
        /// </summary>
        public DateTime VisitDate
        {
            get { return mdtmVisitDate; }
            set { mdtmVisitDate = value; }
        }

        /// <summary>
        /// Gets the Weight of pre-operative visit.
        /// </summary>
        public Decimal Weight
        {
            set { mdecWeight = value; }
            get { return mdecWeight; }
        }

        /// <summary>
        /// Gets the Height of pre-operative visit.
        /// </summary>
        public Decimal Height
        {
            set { mdecHeight = value; }
            get { return mdecHeight; }
        }

        /// <summary>
        /// Gets the Comorbidities list of pre-operative visit.
        /// </summary>
        public Comorbidity[] ComorbiditiesList
        {
            //get { return comorbiditiesList; }
            get { return objComorbidityList.ToArray(); }
        }

        /// <summary>
        /// Sets the Vitamins list of pre-operative visit.
        /// </summary>
        private String AllVitamins
        {
            set
            {
                StringBuilder sb = new StringBuilder();
                String[] strVitamins = new string[] { "" };

                base.SplitString(value, ref strVitamins, ";");
                for (Int16 Xh = 0; Xh < strVitamins.Length; Xh++)
                {
                    if (strVitamins[Xh].Equals("1"))
                        switch (Xh)
                        {
                            case 0:
                                sb.Append("VAA1058;");// "Multiple Vitamin;";
                                break;
                            case 1:
                                sb.Append("VAA1060;"); // "Calcium;";
                                break;
                            case 2:
                                sb.Append("VAA1066;"); // "Vitamin B-12;";
                                break;
                            case 3:
                                sb.Append("VAA1063;"); // "IRON;";
                                break;
                            case 4:
                                sb.Append("VAA1067;"); // "Vitamin D;";
                                break;
                            case 5:
                                sb.Append("VAA1068;"); // "Vitamin A, D, E Combo;";
                                break;
                            case 6:
                                sb.Append("VAA1069;"); // "Calcium with Vitamin D;";
                                break;
                        }
                }
                if (sb.Length > 0)
                    base.SplitString(sb.ToString(), ref mstrVitamins, ";");
            }
        }

        /// <summary>
        /// Gets the Vitamins list of pre-operative visit.
        /// </summary>
        public String[] VitmainCodes
        {
            get { return mstrVitamins; }
        }

        private String AllMedicationCodes
        {
            set { base.SplitString(value, ref mstrMedications, ";"); }
        }

        /// <summary>
        /// Gets the Medications.
        /// </summary>
        public String[] MedicationCodes
        {
            get { return mstrMedications; }
        }

        /// <summary>
        /// Gets or sets the Support Group Frequency for Post Operative Visit 
        /// </summary>
        public String SupportGroupFrequency
        {
            get { return mstrSupportGroupFrequency; }
            set { mstrSupportGroupFrequency = value; }
        }
        #endregion

        #region overrides
        /// <summary>
        /// Set the SRC object to an empty instance.
        /// </summary>
        /// <history>
        /// 	<change user="ALI-87AB9129BB0\Administrator" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
        /// </history>
        public override void Clear()
        {
            mdtmVisitDate = DateTime.Now;
            mstrBoldVisitID = "";
            mintPatientID = 0;
            mintOrganizationCode = 0;
            mintConsultID = 0;
            mdtmVisitDate = DateTime.Now;
            mdecWeight = 0m;
            mdecHeight = 0m;
            mstrSupportGroupFrequency = String.Empty;
            base.Clear();
        }

        /// <summary>
        /// Uses a given data set cell (from a dataset, datareader ...) to set one of the tblUserApplicationData object property.
        /// </summary>
        /// <param name="vName">Name of the column holding the data.</param>
        /// <param name="vValue">Value used to set one of the object properties.</param>
        /// <returns>Boolean. True if the vName parameter has been found and processed and is representative of the object/record.</returns>
        /// <history>
        /// 	<change user="AFarahani" date="24 Jun 2008">Initial version. (CS 2.1.0)</change>
        /// </history>
        protected internal override bool SetProperty(string vName, object vValue)
        {
            bool returnValue = false;

            switch (vName.ToLower())
            {
                case "consultid":
                    if (vValue != System.DBNull.Value)
                    {
                        Int32 intTemp = 0;

                        Int32.TryParse(Convert.ToString(vValue), out intTemp);
                        this.ConsultID = intTemp;
                        returnValue = true;
                    }
                    break;

                case "boldvisitid":
                    if (vValue != System.DBNull.Value)
                    {
                        this.VisitID = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "visitdate":
                    if (vValue != System.DBNull.Value)
                    {
                        DateTime dtmTemp;
                        if (DateTime.TryParse(vValue.ToString(), out dtmTemp))
                            this.VisitDate = dtmTemp;
                        else
                            this.VisitDate = DateTime.MinValue;

                        returnValue = true;
                    }
                    break;

                case "weight":
                    if (vValue != System.DBNull.Value)
                    {
                        Decimal decTemp = 0m;
                        if (Decimal.TryParse(vValue.ToString(), out decTemp))
                            this.Weight = decTemp;
                        else
                            this.Weight = 0;
                        returnValue = true;
                    }
                    break;

                case "height":
                    if (vValue != System.DBNull.Value)
                    {
                        Decimal decTemp = 0m;
                        if (Decimal.TryParse(vValue.ToString(), out decTemp))
                            this.Height = decTemp;
                        else
                            this.Height = 0;
                        returnValue = true;
                    }
                    break;

                case "cvs_hypertension":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue(M_STR_HYPERTENSION, vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "cvs_congestive":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Congestive", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "cvs_ischemic":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Ischemic", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "cvs_angina":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Angina", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "cvs_peripheral":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Peripheral", vValue.ToString());
                        returnValue = true;
                    }
                    break;
                    
                case "cvs_lower":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Lower", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "cvs_dvt":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("DVT", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "met_glucose":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Glucose", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "met_lipids":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Lipids", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "met_gout":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Gout", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "pul_obstructive":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Obstructive", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "pul_obesity":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Obesity", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "pul_pulhypertension":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Pulhypertension", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "pul_asthma":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Asthma", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "gas_gerd":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Gerd", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "gas_cholelithiasis":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Cholelithiasis", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "gas_liver":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Liver", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "mus_backpain":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("BackPain", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "mus_musculoskeletal":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Musculoskeletal", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "mus_fibromyalgia":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Fibromyalgia", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "reprd_polycystic":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("polycystic", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "reprd_menstrual":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("menstrual", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "psy_impairment":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Impairment", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "psy_depression":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Depression", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "psy_mentalhealth":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("MentalHealth", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "psy_alcohol":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Alcohol", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "psy_tobacoo":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Tobacoo", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "psy_abuse":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Abuse", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "gen_stress":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Stress", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "gen_cerebri":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Cerebri", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "gen_hernia":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Hernia", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "gen_functional":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Functional", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "gen_skin":
                    if (vValue != System.DBNull.Value)
                    {
                        SetComorbidityLevelValue("Skin", vValue.ToString());
                        returnValue = true;
                    }
                    break;

                case "vitaminlist":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllVitamins = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "medicationlist":
                    if (vValue != System.DBNull.Value)
                    {
                        this.AllMedicationCodes = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                case "supportgroupfrequency" :
                    if (vValue != System.DBNull.Value)
                    {
                        this.SupportGroupFrequency = vValue.ToString();
                        returnValue = true;
                    }
                    break;

                default:
                    returnValue = base.SetProperty(vName, vValue);
                    break;
            }
            base.NewRecord = false;
            return returnValue;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the pre-operative visit before bariatric surgery
        /// </summary>
        /// <returns></returns>
        public bool LoadPatientPreOperativeVisitData()
        {
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_SRCPreOperativeVisitDataGet", false));
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, this.mintOrganizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, this.mintPatientID);

            return this.GetProperties(command);
        }

        /// <summary>
        /// Returns the pre-operative visit before bariatric surgery
        /// </summary>
        /// <returns></returns>
        public bool LoadPatientPostOperativeVisitData()
        {
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_SRCPostOperativeVisitDataGet", false));
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, this.mintOrganizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, this.mintPatientID);

            return this.GetProperties(command);
        }

        /// <summary>
        /// Saves each comorbidity into Comorbidity list.
        /// </summary>
        /// <param name="strComorbidityName">Comorbidity group name.</param>
        /// <param name="strComorbidityValue">Comorbidity level value.</param>
        private void SetComorbidityLevelValue(String strComorbidityName, String strComorbidityLevelValue) 
        {
            this.SetComorbidityLevelValue(strComorbidityLevelValue);
        }

        private void SetComorbidityLevelValue(String strComorbidityValue)
        {
            if (strComorbidityValue != null && strComorbidityValue != String.Empty)
            {
                String[] strComorbidities = new String[] { "" };

                base.SplitString(strComorbidityValue, ref strComorbidities, ";");
                objComorbidityList.Add(new Comorbidity(strComorbidities[1], strComorbidities[0]));
            }
            return;
        }

        /// <summary>
        /// Flag Patient Visit as sent to BOLD
        /// </summary>
        /// <returns></returns>
        public void FlagPatientOperativeVisitData(Int32 organizationCode, Int32 patientID, Int32 consultID, Decimal height, Decimal weight, DateTime visitDate, String visitID, String supportGroupFrequency)
        {            
            Database objectDatabase = base.ObjectDatabase;

            DbCommand command = objectDatabase.GetStoredProcCommand(base.GetStoreProcedureName("sp_SRCFlagPatientConsult", false));
            base.ObjectDatabase.AddInParameter(command, "@vintOrganizationCode", DbType.Int32, organizationCode);
            base.ObjectDatabase.AddInParameter(command, "@vintPatientId", DbType.Int32, patientID);
            base.ObjectDatabase.AddInParameter(command, "@vintConsultID", DbType.Int32, consultID);
            base.ObjectDatabase.AddInParameter(command, "@vdecHeight", DbType.Decimal, height);
            base.ObjectDatabase.AddInParameter(command, "@vdecWeight", DbType.Decimal, weight);
            base.ObjectDatabase.AddInParameter(command, "@vdtVisitDate", DbType.DateTime, visitDate);
            base.ObjectDatabase.AddInParameter(command, "@vstrVisitID", DbType.String, visitID);
            base.ObjectDatabase.AddInParameter(command, "@vstrSupportGroup", DbType.String, supportGroupFrequency);

            this.Save(command);
        }
        #endregion 
    }
}
