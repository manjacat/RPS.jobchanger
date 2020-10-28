using Oracle.ManagedDataAccess.Client;
using RPS.jobchanger.Constant;
using RPS.jobchanger.Utility;
using System;
using System.Data;

namespace RPS.jobchanger.Database
{
    public class OracleHelper
    {
        public string ConnectionString { get; set; }

        //public enum ServiceName
        //{
        //    BCRM_Update,
        //    Create_CPP,
        //    Create_SNC,
        //    Update_CPP,
        //    Update_SNC,
        //    ERMS_Update
        //}

        public DataTable QueryTable(string query, OracleParameter[] parameters = null)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (OracleConnection oraconn = new OracleConnection(ConnectionString))
            {
                try
                {
                    if (oraconn.State == ConnectionState.Open)
                        oraconn.Close();
                    oraconn.Open();
                    OracleCommand cmd = new OracleCommand(query, oraconn);
                    cmd.CommandType = CommandType.Text;
                    cmd.BindByName = true;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    PrintHelper.Error(string.Format(Messages.GetOracleQuery, query));
                    if (parameters != null)
                    {
                        PrintHelper.Error("Parameter List: ");
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            PrintHelper.Error
                                (string.Format("{0} = {1}",
                                parameters[i].ParameterName,
                                parameters[i].Value));
                        }
                    }
                    PrintHelper.Error(ex.ToString());
                    throw;
                }
                finally
                {
                    oraconn.Close();
                    oraconn.Dispose();
                }
            }
            //return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable(); 
            return dt;
        }

        public void ExecNonQuery(string query, OracleParameter[] parameters = null)
        {
            using (OracleConnection oraconn = new OracleConnection(ConnectionString))
            {
                try
                {
                    if (oraconn.State == ConnectionState.Open)
                        oraconn.Close();
                    oraconn.Open();
                    OracleCommand cmd = new OracleCommand(query, oraconn);
                    cmd.CommandType = CommandType.Text;
                    cmd.BindByName = true;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    int retVal = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    PrintHelper.Error(string.Format(Messages.GetOracleQuery, query));
                    if (parameters != null)
                    {
                        PrintHelper.Error("Parameter List below: ");
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            PrintHelper.Error
                                (string.Format("{0} = {1}",
                                parameters[i].ParameterName,
                                parameters[i].Value));
                        }
                    }
                    PrintHelper.Error(ex.ToString());
                    throw;
                }
                finally
                {
                    oraconn.Close();
                    oraconn.Dispose();
                }
            }
        }
    }
}
