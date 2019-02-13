using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace SSISLibrary
{
    public class CommonFunctionLibrary
    {
        private SSISDbModelContext ctx = new SSISDbModelContext();

        public int GetDeptID(string userEmail)
        {
            var dpt = (from p in ctx.SSISUser where p.Email == userEmail select p.DepartmentID).Single();
            int deptID =0;
            deptID = Convert.ToInt32(dpt);
            return deptID;
        }

        public string GetDeptRepEmail(int deptID)
        {
            string deptRepEmail = ctx.Department.Where(p => p.DepartmentID == deptID).FirstOrDefault().RepEmail;
            return deptRepEmail;
        }

        public string GetDepartmentName(int deptId)
        {
            var dName = (from p in ctx.Department where p.DepartmentID == deptId select p.DeptName).Single();
            string deptName = string.Empty;
            deptName = Convert.ToString(dName);
            return deptName;
        }
        public void SendEmail(string fromEmail, string toEmail, string password, string mailSubject, string mailBody)
        {
            if (fromEmail != null && toEmail != null)
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new System.Net.NetworkCredential("ronaldlimsk@gmail.com", "HelloTeam11");
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mm = new MailMessage("ronaldlimsk@gmail.com", "ronaldlimsk@gmail.com");
                mm.Subject = mailSubject;
                mm.Body = mailBody;
                client.Send(mm);
            }

        }

    }
}
