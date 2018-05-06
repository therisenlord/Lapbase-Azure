using System;
using System.Collections.Generic;

namespace Lapbase.Business
{
    public class Patient : Lapbase.Data.Patient
    {
        #region methods
        public new Boolean GetPatientByPatientCustomID()
        {
            return base.GetPatientByPatientCustomID();
        }
        #endregion
    }
}
