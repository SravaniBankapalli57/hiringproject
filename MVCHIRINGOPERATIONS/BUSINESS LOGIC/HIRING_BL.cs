using System.Data;
using MVCHIRINGOPERATIONS.Models;
using System.Data.SqlClient;
using System.Security.Policy;

namespace MVCHIRINGOPERATIONS.BUSINESS_LOGIC
{
    public class HIRING_BL
    {
        public static DataTable Login(Loginviewmodel OBJ)
        {
            var dbconfig = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                SqlCommand cmd = new SqlCommand("SP_TBL_ADMIN", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EMAILID", OBJ.Emailid);
                cmd.Parameters.AddWithValue("@PASSWORD", OBJ.Password);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public static bool InsertData(USERSMODEL OBJ)
        {
            bool res = false;
            var dbconfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_USERS", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FIRSTNAME", OBJ.FIRSTNAME);
                    cmd.Parameters.AddWithValue("@LASTNAME", OBJ.LASTNAME);
                    cmd.Parameters.AddWithValue("@EMAILID", OBJ.EMAILID);
                    cmd.Parameters.AddWithValue("@PASSWORD", OBJ.PASSWORD);
                    cmd.Parameters.AddWithValue("@GENDER", OBJ.GENDER);
                    cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(OBJ.DOB));
                    cmd.Parameters.AddWithValue("@ROLE", OBJ.ROLE);
                    cmd.Parameters.AddWithValue("@STATUS", OBJ.STATUS);
                    int x = cmd.ExecuteNonQuery();
                    if (x > 0)
                    {
                        return res = true;
                    }
                    else
                    {
                        return res = false;
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    con.Close();
                }
                return res = true;
            }
        }
        public static List<STUDENTMODEL> GetAllData()
        {
            List<STUDENTMODEL> obj = new List<STUDENTMODEL>();
            var dbconfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * from STUDENTDE", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    obj.Add(
                        new STUDENTMODEL
                        {
                            Hall_ticket_no = dr["Hall_ticket_no"].ToString(),
                            Name_of_the_student = dr["Name_of_the_student"].ToString(),
                            Emailid = dr["Emailid"].ToString(),
                            Engineering_College_Name = dr["Engineering_College_Name"].ToString(),
                            Btech_Year_of_Pass_out = Convert.ToInt32(dr["Btech_Year_of_Pass_out"].ToString()),
                            Total_backlogs = Convert.ToInt32(dr["Total_backlogs"].ToString()),
                            Status = dr["Status"].ToString()
                        }
                        );
                }
                return obj;
            }
        }

        //public static List<STUDENTMODEL> GetData()
        //{
        //    List<STUDENTMODEL> obj = new List<STUDENTMODEL>();
        //    var dbconfig = new ConfigurationBuilder()
        //                .SetBasePath(Directory.GetCurrentDirectory())
        //                .AddJsonFile("appsettings.json").Build();
        //    string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
        //    using (SqlConnection con = new SqlConnection(dbconnectionstr))
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter("Select * from STUDENTDE", con);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            obj.Add(
        //                new STUDENTMODEL
        //                {
        //                   Hall_ticket_no = dr["Hall_ticket_no"].ToString(),
        //                    Name_of_the_student = dr["Name_of_the_student"].ToString(),
        //                    Emailid = dr["Emailid"].ToString(),
        //                    PH_No = Convert.ToInt64(dr["PH_No"].ToString()),
        //                    Engineering_College_Name = dr["Engineering_College_Name"].ToString(),
        //                    Btech_Year_of_Pass_out = Convert.ToInt32(dr["Btech_Year_of_Pass_out"].ToString()),
        //                    Total_backlogs = Convert.ToInt32(dr["Total_backlogs"].ToString()),
        //                    Status = dr["Status"].ToString()

        //                 }
        //                );
        //        }
        //     return obj;
        //    }
        //}
        public static bool UpdateData(STUDENTMODEL obj)
        {
            bool res = false;
            var dbconfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_Update_StudentData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Hall_ticket_no", obj.Hall_ticket_no);
                    
                    int x = cmd.ExecuteNonQuery();
                    if (x > 0)
                    {
                        return res = true;
                    }
                    else
                    {
                        return res = false;
                    }



                }
                catch (Exception)
                {
                    return res = true;
                }



        }
        public static List<STUDENTMODEL> GetAllL2data()
        {
            List<STUDENTMODEL> obj = new List<STUDENTMODEL>();
            var dbconfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from dummyL1 where Status in ('Waiting For L2 Interview','L2 Rejected','Waiting For L3 Interview','Ready To Onboard')", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    obj.Add(
                        new STUDENTMODEL
                        {
                            Hall_ticket_no = dr["Hall_ticket_no"].ToString(),
                            Emailid = dr["Emailid"].ToString(),
                            PH_No = Convert.ToInt64(dr["PH_No"].ToString()),
                            Engineering_College_Name = dr["Engineering_College_Name"].ToString(),
                            Total_backlogs = Convert.ToInt32(dr["Total_backlogs"].ToString()),
                            Btech_Year_of_Pass_out = Convert.ToInt32(dr["Btech_Year_of_Pass_out"].ToString()),
                            Status = dr["Status"].ToString()
                        }
                        );
                }
                return obj;
            }
        }
        public static List<STUDENTMODEL> GetAllL3data()
        {
            List<STUDENTMODEL> obj = new List<STUDENTMODEL>();
            var dbconfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from dummyL2 where Status in ('Waiting For L3 Interview','L3 Rejected','Ready To Onboard')", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    obj.Add(
                        new STUDENTMODEL
                        {
                            Hall_ticket_no = dr["Hall_ticket_no"].ToString(),
                            Emailid = dr["Emailid"].ToString(),
                            PH_No = Convert.ToInt64(dr["PH_No"].ToString()),
                            Engineering_College_Name = dr["Engineering_College_Name"].ToString(),
                            Total_backlogs = Convert.ToInt32(dr["Total_backlogs"].ToString()),
                            Btech_Year_of_Pass_out = Convert.ToInt32(dr["Btech_Year_of_Pass_out"].ToString()),
                            Status = dr["Status"].ToString()
                        }
                        );
                }
                return obj;
            }
        }
        public static List<USERSMODEL> GetAllUsersData()
        {
            List<USERSMODEL> obj = new List<USERSMODEL>();
            var dbconfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from Users", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    obj.Add(new USERSMODEL
                    {

                        USERID = Convert.ToInt32(dr["USERID"].ToString()),
                        FIRSTNAME = dr["FIRSTNAME"].ToString(),
                        LASTNAME = dr["LASTNAME"].ToString(),
                        EMAILID = dr["EMAILID"].ToString(),
                        PASSWORD = dr["PASSWORD"].ToString(),
                        GENDER = dr["GENDER"].ToString(),
                        DOB = Convert.ToDateTime(dr["DOB"].ToString()),
                        ROLE = dr["ROLE"].ToString(),
                        STATUS = Convert.ToBoolean(dr["STATUS"].ToString())
                    }
                    );

                }
                return obj;
            }
        }
        public static USERSMODEL GetDataByID(int UserID)
        {

            USERSMODEL obj = null;
            var dbconfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllUserByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    obj = new USERSMODEL();
                    obj.USERID = Convert.ToInt32(ds.Tables[0].Rows[i]["USERID"].ToString());
                    obj.FIRSTNAME = ds.Tables[0].Rows[i]["FIRSTNAME"].ToString();
                    obj.LASTNAME = ds.Tables[0].Rows[i]["LASTNAME"].ToString();
                    obj.EMAILID = ds.Tables[0].Rows[i]["EMAILID"].ToString();
                    obj.PASSWORD = ds.Tables[0].Rows[i]["PASSWORD"].ToString();
                    obj.GENDER = ds.Tables[0].Rows[i]["GENDER"].ToString();
                    obj.DOB = Convert.ToDateTime(ds.Tables[0].Rows[i]["DOB"].ToString());
                    obj.ROLE = ds.Tables[0].Rows[i]["ROLE"].ToString();
                    obj.STATUS = Convert.ToBoolean(ds.Tables[0].Rows[i]["STATUS"].ToString());
                }
                return obj;
            }
        }
        public static bool UpdateUsers(USERSMODEL obj)
        {
            bool res = false;
            var dbconfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_Update_UserData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FIRSTNAME", obj.FIRSTNAME);
                    cmd.Parameters.AddWithValue("@LASTNAME", obj.LASTNAME);
                    cmd.Parameters.AddWithValue("@EMAILID", obj.EMAILID);
                    cmd.Parameters.AddWithValue("@PASSWORD", obj.PASSWORD);

                    cmd.Parameters.AddWithValue("@GENDER", obj.GENDER);
                    cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(obj.DOB));
                    cmd.Parameters.AddWithValue("@ROLE", obj.ROLE);

                    cmd.Parameters.AddWithValue("@STATUS", obj.STATUS);
                    cmd.Parameters.AddWithValue("@USERID", obj.USERID);
                    int x = cmd.ExecuteNonQuery();
                    if (x > 0)
                    {
                        return res = true;
                    }
                    else
                    {
                        return res = false;
                    }

                }
                catch (Exception)
                {
                    return res = true;
                }

        }
        public static bool DeleteUsers(int UserID)
        {
            bool res = false;
            var dbconfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
                try
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_Delete_Users", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    int x = cmd.ExecuteNonQuery();
                    if (x > 0)
                    {
                        return res = true;
                    }
                    else
                    {
                        return res = false;
                    }

                }
                catch (Exception)
                {
                    return res = true;
                }

        }
        public static DataTable ForgetPassword(Loginviewmodel obj)
        {
            var dbconfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))

            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_ForgotPswd", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EMAILID", obj.Emailid);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
        }

        public static bool ResetPassword(ResetPassModel obj)
        {

            var dbconfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("sp_ResetPswd", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMAILID", obj.EmailID);
                    cmd.Parameters.AddWithValue("@PASSWORD", obj.NewPassword);
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                }
                return true;
            }
        }

    }
}
