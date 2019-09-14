using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataSync
{
    public class OracleHelper
    {
        public void InsertOracle(DataTable dt, string sqlConn, string tableName)
        {
            //using (OracleConnection conn = new OracleConnection(sqlConn))
            //{
            //    if (conn.State == ConnectionState.Closed)
            //    {
            //        conn.Open();
            //    }
            //    OracleBulkCopy oracleCopy = new OracleBulkCopy(conn);
            //    oracleCopy.BatchSize = dt.Rows.Count;
            //    oracleCopy.DestinationTableName = tableName;
            //    oracleCopy.WriteToServer(dt);
            //    oracleCopy.Close();
            //}
        }
    }
}
