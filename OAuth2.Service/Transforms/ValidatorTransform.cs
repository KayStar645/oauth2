namespace OAuth2.Infrastructure.Transforms
{
    public static class ValidatorTransform
    {
        public static string MaximumLength(string name, int max)
        {
            return $"Trường {name} phải nhỏ hơn hoặc bằng {max} kí tự!";
        }

        public static string MinimumLength(string name, int min)
        {
            return $"Trường {name} phải lớn hơn hoặc bằng {min} kí tự!";
        }

        public static string Length(string name, int number)
        {
            return $"Trường {name} phải đúng {number} kí tự!";
        }

        public static string Required(string name)
        {
            return $"Trường {name} là bắt buộc!";
        }

        public static string MustIn(string name)
        {
            return $"Trường {name} đã chọn không hợp lệ!";
        }

        public static string MustDate(string name, int year)
        {
            return $"Trường {name} tối thiểu phải đủ {year} tuổi!";
        }

        public static string MustUrl(string name)
        {
            return $"Trường {name} phải là một URL hợp lệ!";
        }

        public static string MustUrls(string name)
        {
            return $"Trường {name} phải là một mảng URL hợp lệ!";
        }

        public static string Must(string name, string[] options)
        {
            return $"Trường {name} phải thuộc {string.Join(", ", options)}!";
        }

        public static string MustWhen(string name, string[] options, string nameOther, string value)
        {
            return $"Trường {name} có thể thuộc {string.Join(", ", options)} nếu {nameOther} là {value} ; ngược lại trường {name} phải null!";
        }
        public static string MustFile(string name)
        {
            return $"Trường {name} phải là một file hợp lệ!";
        }
        public static string ValidValue(string name)
        {
            return $"Trường {name} không hợp lệ!";
        }

        public static string ValidValue(string name, string value)
        {
            return $"Trường {name} = {value} không hợp lệ!";
        }

        public static string Exists(string name)
        {
            return $"Giá trị của {name} đã tồn tại!";
        }

        public static string ExistsValue(string name, string value)
        {
            return $"{name} = {value} đã tồn tại!";
        }

        public static string ExistsIn(string name, string key)
        {
            return $"{name} đã tồn tại trong {key}!";
        }

        public static string NotExistsValue(string name, string value)
        {
            return $"{name} = {value} không tồn tại!";
        }

        public static string NotExistsValueInTable(string name, string table)
        {
            return $"Giá trị của {name} không tồn tại trong bảng {table}!";
        }

        public static string AnyIsLower(string name)
        {
            return $"Trường {name} phải chứa ít nhất 1 chữ thường!";
        }

        public static string AnyIsUpper(string name)
        {
            return $"Trường {name} phải chứa ít nhất 1 chữ hoa!";
        }
        public static string AnyIsDigit(string name)
        {
            return $"Trường {name} phải chứa ít nhất 1 số!";
        }
        public static string AnyIsLetterOrDigit(string name)
        {
            return $"Trường {name} phải chứa ít nhất 1 ký tự đặc biệt!";
        }

        public static string GreaterThanOrEqualTo(string name, int number)
        {
            return $"Trường {name} ít nhất lớn hơn hoặc bằng {number}!";
        }

        public static string LessThanOrEqualTo(string name, int number)
        {
            return $"Trường {name} phải nhỏ hơn hoặc bằng {number}!";
        }

        public static string GreaterThanToday(string name)
        {
            return $"Trường {name} ít nhất phải lớn hơn hôm nay!";
        }

        public static string GreaterThanDay(string name, DateTime day)
        {
            return $"Trường {name} ít nhất phải lớn hơn ngày {day}!";
        }

        public static string GreaterEqualOrThanDay(string name, DateTime day)
        {
            return $"Trường {name} ít nhất phải lớn hơn hoặc bằng ngày {day}!";
        }

        public static string LessThanToday(string name)
        {
            return $"Trường {name} phải nhỏ hơn hôm nay!";
        }

        public static string LessThanDay(string name, DateTime day)
        {
            return $"Trường {name} phải nhỏ hơn ngày {day}!";
        }

        public static string LessEqualOrThanDay(string name, DateTime day)
        {
            return $"Trường {name} phải nhỏ hơn hoặc bằng ngày {day}!";
        }

        public static string Equal(string name, string value)
        {
            return $"Trường {name} phải trùng khớp với trường {value}!";
        }
    }
}
