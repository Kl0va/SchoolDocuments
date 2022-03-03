using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SchoolDocuments.Converters
{
    class RoleConverter : IValueConverter
    {
        const string admin = "Администратор";
        const string employee = "Сотрудник";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if((String)value == "Admin")
            {
                return admin;
            }
            else
            {
                return employee;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return "s";
        }
    }
}
