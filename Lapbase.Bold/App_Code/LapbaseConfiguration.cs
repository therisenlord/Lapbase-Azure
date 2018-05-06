using System;
using System.Data;
using System.Configuration;
using System.Configuration.Assemblies;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for LapbaseConfiguration
/// </summary>
/// 
namespace Lapbase.Configuration.ConfigurationApplication
{
    public class LapbaseConfiguration : ConfigurationSection
    {
        public LapbaseConfiguration()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Properties
        /// <summary>
        /// Gets the SRC Vendor Code.
        /// </summary>
        public static String SRCVendorCode
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["SRCVendorCode"].ToString(); }
        }

        /// <summary>
        /// Gets the Practice CEO Code.
        /// </summary>
        public static String PracticeCEOCode
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["PracticeCEOCode"].ToString(); }
        }

        /// <summary>
        /// Gets the Surgeon CEO Code.
        /// </summary>
        public static String SurgeonCEOCode
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["SurgeonCEOCode"].ToString(); }
        }

        /// <summary>
        /// Gets the Facility CEO Code.
        /// </summary>
        public static String FacilityCEOCode
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["FacilityCEOCode"].ToString(); }
        }

        /// <summary>
        /// Gets the SRC User's Name.
        /// </summary>
        public static String SRCUserName
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["SRCUserName"].ToString(); }
        }

        /// <summary>
        /// Gets the SRC User's Password.
        /// </summary>
        public static String SRCPassword
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["SRCPassword"].ToString(); }
        }

        /// <summary>
        /// Gets the SRC visibility
        /// </summary>
        public static Boolean SRCVisibility
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["SRCVisibility"].ToString().Equals(Boolean.TrueString); }
        }
        #endregion
    }
}