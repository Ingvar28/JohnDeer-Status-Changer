using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;

namespace Status_changer
{
    class DBContext
    {
        static public DataTable GetConsStatus()
        {
            DataTable result = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.BPA_RUConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        //cmd.CommandText = "Select ROW_NUMBER() OVER(ORDER BY data ASC) AS Row#, 'Nakladnaya TNT', Status, Data, 'Tekuschee mestopolozhenie' from JohnDeere WHERE Status IS NOT NULL;";
                        cmd.CommandText = "Select * FROM JohnDeere WHERE Status IS NOT NULL";


                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                result.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }

        static public void ChangeRecordStatus()
        {
            
            try
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.BPA_RUConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "Update JohnDeere Set Status = 'Груз выдан (внесено)' Where Status = 'Груз выдан'";                       
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {                
            }
            
        }
        //static public void ChangeRecordStatus(int id)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Properties.Settings.Default.conString))
        //        {
        //            con.Open();

        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                cmd.Connection = con;
        //                cmd.CommandText = "Update InvoicesStatuses Set InMainframe = 1 Where id = @id";

        //                cmd.Parameters.Clear();

        //                cmd.Parameters.Add("@id", SqlDbType.Int);
        //                cmd.Parameters["@id"].Value = id;

        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
    }

}