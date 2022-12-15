using System.ComponentModel.DataAnnotations;

namespace MVCHIRINGOPERATIONS.Models
{
    public class Loginviewmodel
    {
        [Required(ErrorMessage = "requires email")]

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Emailid { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }

    }
    public class USERSMODEL
    {
        public int USERID { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string EMAILID { get; set; }
        public string PASSWORD { get; set; }
        public string GENDER { get; set; }
        public DateTime DOB { get; set; }
        public string ROLE { get; set; }
        public bool STATUS { get; set; }

    }
    public class STUDENTMODEL
    {
        public int Sno { get; set; }
        public string Hall_ticket_no { get; set; }
        public string Name_of_the_student { get; set; }
        public string Emailid { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public long PH_No { get; set; }
        public long Aadhar_no { get; set; }
        public string School_Name { get; set; }
        public int ssc_Year_of_Pass_out { get; set; }
        public string Ssc_Aggregate { get; set; }
        public string Junior_College_Name { get; set; }
        public int inter_Year_of_Pass_out { get; set; }
        public string inter_Aggregate { get; set; }
        public string Engineering_College_Name { get; set; }
        public string Branch { get; set; }
        public int Btech_Year_of_Pass_out { get; set; }
        public int Total_backlogs { get; set; }
        public string Graduation_Aggregate { get; set; }
        public string Fathers_name { get; set; }
        public string Fathers_occupation { get; set; }
        public string Permanent_address { get; set; }
        public string Present_address { get; set; }
        public string Fathers_Mobile_No { get; set; }
        public string Mothers_Name { get; set; }
        public string Mothers_Occupation { get; set; }
        public string Resume_Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string Remarks { get; set; }
        public int Score { get; set; }
        public DateTime DateOfL1Exam { get; set; }
        public string Status { get; set; }
    }
    public class ResetPassModel
    {

        [Required(ErrorMessage = "Registered Email required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string EmailID { get; set; }
        [Required(ErrorMessage = "New password required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password does not match")]
        public string ConfirmPassword { get; set; }
    }

    public class OtpModel
    {
        public string Otp { get; set; }
    }

}
