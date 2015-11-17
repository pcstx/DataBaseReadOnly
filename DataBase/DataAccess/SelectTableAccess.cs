using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.Text;

namespace DataBase.DataAccess
{
    public class SelectTableAccess
    { 
        DataSet ds = new DataSet();

        public DataSet Select(string sql, string dbName, string connName)
        {
             try
             {
                StringBuilder sqlSB = new StringBuilder();
                sqlSB.AppendFormat(" use [{0}]; set rowcount 100; ", dbName);
                sqlSB.Append(@" set statistics io on;
                                     set statistics time on;");                
                sqlSB.Append(sql);
                sqlSB.Append(@" ;set statistics time off; 
                                    set statistics io off; ");
               // sql = " use [" + dbName + "]; set rowcount 100; " + sql;
                 using (SqlConnection conn = ConnectionString.GetConnection(connName,2) as SqlConnection)
                 {
                     try
                     {
                        conn.InfoMessage += Conn_InfoMessage; 
                        DataColumn dc = new DataColumn("消息");
                        messageDT.Columns.Add(dc);

                        using (SqlDataAdapter dapter = new SqlDataAdapter(sqlSB.ToString(), conn))
                         {
                             dapter.Fill(ds);
                         }
                     }
                     catch (Exception ex)
                     {
                         DataTable dt = new DataTable("结果");                         
                         DataColumn dc = new DataColumn("错误");
                         dt.Columns.Add(dc);
                         DataRow dr = dt.NewRow();
                         dr[0] = ex.Message;
                         dt.Rows.Add(dr);
                         ds.Tables.Add(dt);
                     }
                     conn.Close();
                 }
             }
             catch (Exception ex)
             {
                 DataTable dt = new DataTable("结果");
                 DataColumn dc = new DataColumn("错误");
                 dt.Columns.Add(dc);
                 DataRow dr = dt.NewRow();
                 dr[0] = ex.Message;
                 dt.Rows.Add(dr);
                 ds.Tables.Add(dt);
             }

            if (messageDT.Rows.Count > 0)
            { 
                ds.Tables.Add(messageDT);
            }
            return ds;
        }

        DataTable messageDT = new DataTable("消息");

        private void Conn_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        { 
            DataRow dr = messageDT.NewRow();
            dr[0] = e.Message;
            messageDT.Rows.Add(dr);
        }
    }
}