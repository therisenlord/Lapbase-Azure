using System;
using System.Collections.Generic;
using System.Text;

namespace Lapbase.Business
{
    public class BoldData : Lapbase.Data.Baseline
    {
        #region variables
        Int32 mOrganizationCode;
        String mChartNumber = String.Empty;
        #endregion

        #region Properties
        public Int32 OrganizationCode
        {
            get { return mOrganizationCode; }
            set { mOrganizationCode = value; }
        }
        public String ChartNumber
        {
            get { return mChartNumber; }
            set { mChartNumber = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Saves BOLD data, Sending Request to SRC web site using SRC Web service.
        /// </summary>
        /// <returns></returns>
        public Boolean SaveSRCData()
        {
            Boolean saveFlag = true;

            return saveFlag;
        }

        private bool IsChartNumberUnique()
        {
            return false;
            //return base.IsChartNumberUnique(this.OrganizationCode, this.ChartNumber);
        }
        #endregion
    }
}
