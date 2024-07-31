using System.Text.RegularExpressions;

namespace OAuth2.Infrastructure.Custom
{
    public static class ValidatorCustom
    {
        public static bool IsAtLeastNYearsOld(DateTime? dateOfBirth, int year)
        {
            DateTime currentDate = DateTime.Now;
            DateTime minimumBirthDate = currentDate.AddYears(-year);
            return dateOfBirth <= minimumBirthDate;
        }

        public static bool BeValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }
        public static bool IsEqualOrAfterDay(DateTime? time, DateTime day)
        {
            return time >= day;
        }


        public static bool IsAfterDay(DateTime? time, DateTime day)
        {
            return time > day;
        }


        public static bool IsValidFile(string file)
        {
            string pattern = @"^[^.]+\.[a-zA-Z]+$";

            return Regex.IsMatch(file, pattern) && file.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }
    }
}
