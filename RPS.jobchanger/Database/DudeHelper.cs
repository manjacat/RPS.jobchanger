using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPS.jobchanger.Database
{
    public class DudeHelper : OracleHelper
    {
        #region properties

        private string[] dudeeo_table_list
        {
            get
            {
                //list all table names here
                string[] arr = new string[]
                {
                    "ED_DEMAND_POINT"
                };
                return arr;
            }
        }

        #endregion

        // constructor
        public DudeHelper()
        {
            //Connection to DUDE
            ConnectionString = ConfigurationManager.AppSettings["dudeconn"].ToString();
        }

        //for testing
        public DataTable GetSample()
        {
            string query = "SELECT SW_ID, STATUS, JOB_NAME, CGIS_LABEL FROM DUDEEO.ED_DEMAND_POINT WHERE ROWNUM < 10 AND JOB_NAME IS NOT NULL";
            DataTable dt = QueryTable(query);
            return dt;
        }

        public void UpdateJobName(string jobName_old, string jobName_new)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("JOBNAME_OLD", jobName_old),
                new OracleParameter("JOBNAME_NEW", jobName_new)
            };

            foreach (var item in dudeeo_table_list)
            {
                // dalam query jgn taruk semi colon nanti error
                string query =
                    string.Format("UPDATE DUDEEO.{0} SET JOB_NAME = :JOBNAME_NEW WHERE JOB_NAME = :JOBNAME_OLD ", item);
                //Console.WriteLine(query);
                ExecNonQuery(query, parameters);
            }

        }
    }
}
