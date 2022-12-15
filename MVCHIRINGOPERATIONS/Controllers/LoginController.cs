using Microsoft.AspNetCore.Mvc;
using MVCHIRINGOPERATIONS.BUSINESS_LOGIC;
using MVCHIRINGOPERATIONS.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace MVCHIRINGOPERATIONS.Controllers
{
   
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Loginviewmodel obj)
        {
            if (ModelState.IsValid)
            {
                DataTable dt = new DataTable();
                dt = HIRING_BL.Login(obj);
                if (dt.Rows.Count > 0)
                {
                    HttpContext.Session.SetString("LoginName", dt.Rows[0]["FIRSTNAME"].ToString());
                    HttpContext.Session.SetString("UserName", dt.Rows[0]["Emailid"].ToString());
                    HttpContext.Session.SetString("Time", System.DateTime.Now.ToShortTimeString());
                    return RedirectToAction("Adminhomepage", "Login");
                }
                else
                {
                    return View();
                }
            }
            return View(obj);
        }
        public IActionResult Adminhomepage()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Adminhome()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Adminhome(USERSMODEL OBJ)
        {
            if (ModelState.IsValid)
            {
                bool res = HIRING_BL.InsertData(OBJ);
                if (res == true)
                {
                    return View();
                }
                else
                {
                    return View();
                }

            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult Trainehome()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Trainehome(List<IFormFile> PostedFiles, STUDENTMODEL obj)
        {
            foreach (IFormFile PostedFile in PostedFiles)
            {
                string fileName = Path.GetFileName(PostedFile.FileName);
                string type = PostedFile.ContentType;
                byte[] bytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    PostedFile.CopyTo(ms);
                    bytes = ms.ToArray();
                }
                var dbconfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
                string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
                using (SqlConnection con = new SqlConnection(dbconnectionstr))
                {
                    SqlCommand cmd = new SqlCommand("SP_STUDENTDE", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Hall_ticket_no", obj.Hall_ticket_no);
                    cmd.Parameters.AddWithValue("@Name_of_the_student", obj.Name_of_the_student);
                    cmd.Parameters.AddWithValue("@Emailid", obj.Emailid);
                    cmd.Parameters.AddWithValue("@Dob", Convert.ToDateTime(obj.Dob));
                    cmd.Parameters.AddWithValue("@Gender", obj.Gender);
                    cmd.Parameters.AddWithValue("@PH_No", Convert.ToInt64(obj.PH_No));
                    cmd.Parameters.AddWithValue("@Aadhar_no", Convert.ToInt64(obj.Aadhar_no));
                    cmd.Parameters.AddWithValue("@School_Name", obj.School_Name);
                    cmd.Parameters.AddWithValue("@ssc_Year_of_Pass_out", Convert.ToInt32(obj.ssc_Year_of_Pass_out));
                    cmd.Parameters.AddWithValue("@Ssc_Aggregate", obj.Ssc_Aggregate);
                    cmd.Parameters.AddWithValue("@Junior_College_Name", obj.Junior_College_Name);
                    cmd.Parameters.AddWithValue("@inter_Year_of_Pass_out", Convert.ToInt32(obj.inter_Year_of_Pass_out));
                    cmd.Parameters.AddWithValue("@inter_Aggregate", obj.inter_Aggregate);
                    cmd.Parameters.AddWithValue("@Engineering_College_Name", obj.Engineering_College_Name);
                    cmd.Parameters.AddWithValue("@Branch", obj.Branch);
                    cmd.Parameters.AddWithValue("@Btech_Year_of_Pass_out", Convert.ToInt32(obj.Btech_Year_of_Pass_out));
                    cmd.Parameters.AddWithValue("@Total_backlogs", Convert.ToInt32(obj.Total_backlogs));
                    cmd.Parameters.AddWithValue("@Graduation_Aggregate", obj.Graduation_Aggregate);
                    cmd.Parameters.AddWithValue("@Fathers_name", obj.Fathers_name);
                    cmd.Parameters.AddWithValue("@Fathers_occupation", obj.Fathers_occupation);
                    cmd.Parameters.AddWithValue("@Permanent_address", obj.Permanent_address);
                    cmd.Parameters.AddWithValue("@Present_address", obj.Present_address);
                    cmd.Parameters.AddWithValue("@Fathers_Mobile_No", obj.Fathers_Mobile_No);
                    cmd.Parameters.AddWithValue("@Mothers_Name", obj.Mothers_Name);
                    cmd.Parameters.AddWithValue("@Mothers_Occupation", obj.Mothers_Occupation);
                    cmd.Parameters.AddWithValue("@Name", fileName);
                    cmd.Parameters.AddWithValue("@ContentType", type);
                    cmd.Parameters.AddWithValue("@Data", bytes);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return View();
        }
        public IActionResult L1module()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DisplayStd()
        {
            return View(HIRING_BL.GetAllData());
        }
        [HttpPost]
        public IActionResult DisplayStd(string Hall_ticket_no, string Remarks, int Score,string To)
        {
            string LoginName = HttpContext.Session.GetString("LoginName");
            string Status = "";
            string Result = "";
            string Result1 = "";
            if (Score >= 60)
            {
                Status = "Waiting For L2 Interview";
                Result = "Dear Student,Congratulations you are Selected In First Round";
                Result1 = "this student waiting for the L2 round";
                  
            }
            else
            {
                Result1 = "this student rejected in first round";
                Result = "Dear Student, you are Rejected In First Round";
                Status = "L1 Rejected";
            }

            var dbconfig = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))

            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_interview_records", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Hall_ticket_no", Hall_ticket_no);
                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                cmd.Parameters.AddWithValue("@Score", Score);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@LoginName", LoginName);
                MailMessage mail = new MailMessage();
                mail.To.Add(To);
                mail.From = new MailAddress("karthikkakarla99@gmail.com");
                mail.Subject = "Interview Result";
                string Body = Result;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("karthikkakarla99@gmail.com", "ztckgsckdekvsvwn"); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
                
                MailMessage mail1 = new MailMessage();
                mail1.To.Add("jayanthigollori16@gmail.com");
                mail1.From = new MailAddress("karthikkakarla99@gmail.com");
                mail1.Subject = "Interview Result";
                string Body1 = Result1;
                mail1.Body = Body1;
                mail1.IsBodyHtml = true;
                SmtpClient smtp1 = new SmtpClient();
                smtp1.Host = "smtp.gmail.com";
                smtp1.Port = 587;
                smtp1.UseDefaultCredentials = false;
                smtp1.Credentials = new System.Net.NetworkCredential("karthikkakarla99@gmail.com", "ztckgsckdekvsvwn"); // Enter seders User name and password       
                smtp1.EnableSsl = true;
                smtp1.Send(mail1);
                int x = cmd.ExecuteNonQuery();
                if (x > 0)
                {
                    return RedirectToAction("DisplayStd", "Login");
                }
                return View();
            }
           
        }
        [HttpGet]
        //public IActionResult Update()
        //{
        //    return View(HIRING_BL.GetAllData());
        //}
        [HttpGet]
        public IActionResult L2module()
        {
            return View((HIRING_BL.GetAllL2data()));
        }
        [HttpPost]
        public IActionResult L2module(string Hall_ticket_no, string Remarks, int Score, string To)
        {
            string LoginName = HttpContext.Session.GetString("LoginName");
            string Status = "";
            string Result = "";
            string Result1 = "";
            if (Score >= 60)
            {
                Status = "Waiting For L3 Interview";
                Result = "Dear Student,Congratulations you are Selected In Second Round";
                Result1 = "this student waiting for the L2 round";

            }
            else
            {
                Result1 = "this student rejected in first round";
                Result = "Dear Student, you are Rejected In Second Round";
                Status = "L2 Rejected";
            }

            var dbconfig = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))

            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_L2interview_records", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Hall_ticket_no", Hall_ticket_no);
                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                cmd.Parameters.AddWithValue("@Score", Score);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@LoginName", LoginName);
                MailMessage mail = new MailMessage();
                mail.To.Add(To);
                mail.From = new MailAddress("karthikkakarla99@gmail.com");
                mail.Subject = "Interview Result";
                string Body = Result;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("karthikkakarla99@gmail.com", "ztckgsckdekvsvwn"); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);

                MailMessage mail1 = new MailMessage();
                mail1.To.Add("jayanthigollori16@gmail.com");
                mail1.From = new MailAddress("karthikkakarla99@gmail.com");
                mail1.Subject = "Interview Result";
                string Body1 = Result1;
                mail1.Body = Body1;
                mail1.IsBodyHtml = true;
                SmtpClient smtp1 = new SmtpClient();
                smtp1.Host = "smtp.gmail.com";
                smtp1.Port = 587;
                smtp1.UseDefaultCredentials = false;
                smtp1.Credentials = new System.Net.NetworkCredential("karthikkakarla99@gmail.com", "ztckgsckdekvsvwn"); // Enter seders User name and password       
                smtp1.EnableSsl = true;
                smtp1.Send(mail1);
                int x = cmd.ExecuteNonQuery();
                if (x > 0)
                {
                    return RedirectToAction("L2module", "Login");
                }
                return View();
            }

        }
        [HttpGet]
        public IActionResult L3module()
        {
            return View(HIRING_BL.GetAllL3data());
        }
        [HttpPost]
        public IActionResult L3module(string Hall_ticket_no, string Remarks, int Score, string To)
        {
            string LoginName = HttpContext.Session.GetString("LoginName");
            string Status = "";
            string Result = "";
            string Result1 = "";
            if (Score >= 60)
            {
                Status = "Ready To Onboard";
                Result = "Dear Student,Congratulations you are Selected In Second Round";
                Result1 = "this student waiting for the L2 round";

            }
            else
            {
                Result1 = "this student rejected in first round";
                Result = "Dear Student, you are Rejected In Second Round";
                Status = "L3 Rejected";
            }

            var dbconfig = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))

            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_L2interview_records", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Hall_ticket_no", Hall_ticket_no);
                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                cmd.Parameters.AddWithValue("@Score", Score);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@LoginName", LoginName);
                MailMessage mail = new MailMessage();
                mail.To.Add(To);
                mail.From = new MailAddress("karthikkakarla99@gmail.com");
                mail.Subject = "Interview Result";
                string Body = Result;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("karthikkakarla99@gmail.com", "ztckgsckdekvsvwn"); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);

                MailMessage mail1 = new MailMessage();
                mail1.To.Add("jayanthigollori16@gmail.com");
                mail1.From = new MailAddress("karthikkakarla99@gmail.com");
                mail1.Subject = "Interview Result";
                string Body1 = Result1;
                mail1.Body = Body1;
                mail1.IsBodyHtml = true;
                SmtpClient smtp1 = new SmtpClient();
                smtp1.Host = "smtp.gmail.com";
                smtp1.Port = 587;
                smtp1.UseDefaultCredentials = false;
                smtp1.Credentials = new System.Net.NetworkCredential("karthikkakarla99@gmail.com", "ztckgsckdekvsvwn"); // Enter seders User name and password       
                smtp1.EnableSsl = true;
                smtp1.Send(mail1);
                int x = cmd.ExecuteNonQuery();
                if (x > 0)
                {
                    return RedirectToAction("L3module", "Login");
                }
                return View();
            }

        }
        public IActionResult DisplayUsers()
        {
            return View(HIRING_BL.GetAllUsersData());
        }

        [HttpGet]
        public IActionResult Edit(int? UserID)
        {
            return View(HIRING_BL.GetDataByID((int)UserID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(USERSMODEL obj)
        {
            if (ModelState.IsValid)
            {
                bool res = HIRING_BL.UpdateUsers(obj);
                if (res == true)
                {
                    return RedirectToAction("DisplayUsers");
                }
                else
                {
                    return View(obj);
                }
            }
            return View();
        }
        public IActionResult Delete(int? UserID)
        {
            bool res = HIRING_BL.DeleteUsers((int)UserID);
            if (res == true)
            {
                return View("DisplayUsers");
            }
            else
            {
                return View();
            }
            return View();
        }
        public IActionResult Logout()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgetPassword(Loginviewmodel obj)
        {
            DataTable dt = new DataTable();
            dt = HIRING_BL.ForgetPassword(obj);
            if (dt.Rows.Count > 0)
            {
                Random rand = new Random();
                HttpContext.Session.SetString("OTP", rand.Next(1111, 9999).ToString());
                bool result = SendEmail(obj.Emailid);
                if (result == true)
                {
                    return RedirectToAction("VerifyOtp", "Login");
                }
                return View("ResetPassword", "Login");
            }
            else
            {
                return View();
            }
            return View();
        }
       
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPassModel obj)
        {
            if (ModelState.IsValid)
            {
                bool res = HIRING_BL.ResetPassword(obj);
                if (res == true)
                {
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View(obj);
            }
        }
        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyOtp(OtpModel obj)
        {
            if (obj.Otp.Equals(HttpContext.Session.GetString("OTP")))
            {
                return RedirectToAction("ResetPassword", "Login");
            }
            else
            {
                return View("Login");
            }

        }
        public bool SendEmail(string receiver)
        {
            bool chk = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("karthikkakarla99@gmail.com");
                mail.To.Add(receiver);
                mail.IsBodyHtml = true;
                mail.Subject = "OTP";
                mail.Body = "Your OTP is :" + HttpContext.Session.GetString("OTP");
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new NetworkCredential("karthikkakarla99@gmail.com", "ztckgsckdekvsvwn");
                client.EnableSsl = true;
                client.Send(mail);
                chk = true;
            }
            catch (Exception)
            {
                throw;
            }
            return chk;
        }
    }
}
