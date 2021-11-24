﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace Assets.Code.Data
{
    //if sony can do it so can I
    public class SqlHelper
    {
        private static object lockThis = new object();


        private static SQLiteConnection con = null;

        private static SQLiteDataAdapter DA;
        private static DataSet DS;
        private static DataTable DT;


        private static void SetupConnection(string constr)
        {
            try
            {
                lock (lockThis)
                {
                    //if (con == null)
                    {

                        con = new SQLiteConnection(constr);

                    }

                    try { con.Open(); }
                    catch (InvalidOperationException ex) // The connection was not closed.
                    { }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    System.Threading.Thread.Sleep(50); //Wait for status to change to Connecting
                    while (con.State == ConnectionState.Connecting)
                    {
                        //Wait for Connection to Connect:
                    }
                }
            }
            catch
            {

            }

        }

        public static DataTable GetDataTable(string sqlqry, string constr, DataTable DT = null)
        {
            try
            {
                SetupConnection(constr);

                DA = new SQLiteDataAdapter(sqlqry, con);
                DA.ReturnProviderSpecificTypes = true;
                DA.SelectCommand.CommandTimeout = 800;//800;
                DA.SelectCommand.Parameters.Clear();
                if (DT == null)
                {
                    //DA.SelectCommand.Parameters.AddRange(_param);
                    DT = new DataTable();
                }


                try
                {
                    DA.Fill(DT);
                }
                catch (Exception ex)
                {
                    string what = ex.Message;
                    // we need another selution to the error handeling
                    //ErrorLoging.WriteErrorLog(0, ex.Message, ex.StackTrace);
                }
                con.Close();
                return DT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Execute a query returning true if successfull, rollback transaction if not
        /// </summary>
        /// <param name="Connstring">Connection string</param>
        /// <param name="Qry">Query to execute</param>
        public static bool ExecuteNonQueryBL1(string Qry, string constr)
        {
            bool success = false;
            try
            {
                SetupConnection(constr);
                var cmd = con.CreateCommand();
                cmd.CommandText = Qry;
                cmd.Parameters.Clear();
                //cmd.Parameters.AddRange(parms);
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                // ErrorLoging.WriteErrorLog(0, ex.Message, ex.StackTrace);
                // throw ex;
            }
            finally
            {
                con.Close();
            }

            return success;
        }

        /// <summary>
        /// Get a single value returned as an object
        /// </summary>
        /// <param name="ConnectionString">Connection string</param>
        /// <param name="Qry">Query to execute</param>
        /// <returns></returns>
        public static object GetSingleValue(string Qry, string constr)
        {

            object objValue = null;

            SetupConnection(constr);

            IDbCommand cmd = con.CreateCommand();

            cmd.CommandText = Qry;
            cmd.Parameters.Clear();
            //cmd.Parameters.AddRange(parms);

            try
            {

                objValue = (object)cmd.ExecuteScalar();

                string isnull = Convert.ToString(objValue).Trim();

                //if (isnull == "")
                //{
                //    objValue = null;
                //}
            }
            catch (Exception ex)
            {

            }
            finally
            {

                if (cmd != null) cmd.Dispose();
                cmd = null;
                con.Close();
            }

            return objValue;
        }
    }
}
