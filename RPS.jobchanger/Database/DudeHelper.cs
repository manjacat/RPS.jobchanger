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
                    "ED_OH_PRIMARY_CONDUCTOR",
                    "ED_UG_PRIMARY_CONDUCTOR",
                    "ED_UG_PRIMARY_SPLICE",
                    "CGIS_MV_CABLE_TERMINAL",
                    "ED_UG_SECONDARY_CONDUCTOR",
                    "ED_OH_SECONDARY_CONDUCTOR",
                    "ED_UG_SECONDARY_SPLICE",
                    "ED_LIGHT",
                    "ED_RECLOSER",
                    "CGIS_DIST_SUBSTATION",
                    "SUB_SUBSTATION",
                    "ED_DEMAND_POINT",
                    "ED_CABINET",
                    "ED_SECONDARY_FUSE",
                    "ED_MANHOLE",
                    "ST_DUCT",
                    "ED_PIT",
                    "ED_POLE",
                    "ED_FUSE",
                    "ED_OH_PRIMARY_WIRE",
                    "ED_SECONDARY_SWITCH",
                    "ED_UG_PRIMARY_CABLE",
                    "CGIS_ED_RMU",
                    "ED_OH_SECONDARY_WIRE",
                    "ED_UG_SECONDARY_CABLE",
                    "ED_CIRCUIT",
                    "CGIS_LV_FEEDER_POINT",
                    "SUB_CIRCUIT_BREAKER",
                    "SUB_FUSE",
                    "SUB_GROUND_BANK",
                    "SUB_POTHEAD",
                    "SUB_SWITCH_BANK",
                    "ED_OH_TRANSFORMER",
                    "SUB_TRANSFORMER_BANK",
                    "SUB_BUS",
                    "EO_HYPERNODE"
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
            //list all table names here
            string[] excludedTable = new string[]
            {
                    "ED_OH_PRIMARY_WIRE",
                    "ED_UG_PRIMARY_CABLE",
                    "ED_OH_SECONDARY_WIRE",
                    "ED_UG_SECONDARY_CABLE",
                    "SUB_POTHEAD",
                    "EO_HYPERNODE"
            };

            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("JOBNAME_OLD", jobName_old),
                new OracleParameter("JOBNAME_NEW", jobName_new)
            };

            foreach (var item in dudeeo_table_list)
            {
                //DISABLE THE UPDATE TRIGGER
                string triggerEnable = string.Format("ALTER TRIGGER DUDEEO.U_{0} DISABLE", item);
                if (!excludedTable.Contains(item))
                    ExecNonQuery(triggerEnable, parameters);
                
                // dalam query jgn taruk semi colon nanti error
                string query =
                    string.Format("UPDATE DUDEEO.{0} SET JOB_NAME = :JOBNAME_NEW WHERE JOB_NAME = :JOBNAME_OLD ", item);
                //Console.WriteLine(query);
                ExecNonQuery(query, parameters);

                //ENABLE BACK THE UPDATE TRIGGER
                triggerEnable = string.Format("ALTER TRIGGER DUDEEO.U_{0} ENABLE", item);
                if (!excludedTable.Contains(item))
                    ExecNonQuery(triggerEnable, parameters);
            }

        }
    }
}
