using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHSAPI.Common.CommonMethods
{
    public class CommonFunction
    {
        #region for generating otp
        public static string GenerateOTP(int length)
        {
            string characters = "1234567890";

            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp.ToString().PadLeft(6, '0');
        }
        #endregion

        #region CreatePassword
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion

        #region AgeCalculator
        public static int GetAgeFromDOB(string DOB)
        {
            if (!string.IsNullOrEmpty(DOB))
            {
                var dateTimeDOB = Convert.ToDateTime(DOB);
                var today = DateTime.Today;
                // Calculate the age.
                var age = today.Year - dateTimeDOB.Year;
                // Go back to the year the person was born in case of a leap year
                if (dateTimeDOB.Date > today.AddYears(-age)) age--;
                return age;
            }
            return 0;
        }
        #endregion

    public static int Get8DigitID()
    {
      Random rnd = new Random();
      return  rnd.Next(10000000, 99999999);
    }
    public static int Get4DigitID()
    {
      Random rnd = new Random();
      return rnd.Next(1000, 9999);
    }
    public static List<DateTime> getDates(DateTime startDate, DateTime endDate,int interval = 1)
    {
      var dates = new List<DateTime>();

      for (var dt = startDate; dt <= endDate; dt = dt.AddDays(interval))
      {
        
            dates.Add(dt);
      }
      return dates;
    }

    public static DateTime GetEndDate(DateTime startDate, int days)
    {

      return startDate.AddDays(days);
    }

  }
}
