using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SchoolDocuments.Converters
{
    class BoolConverter : IValueConverter
    {
        const string familiarized = "Ознакомлен";
        const string notFamiliarized = "Не ознакомлен";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
                return familiarized;
            else return notFamiliarized;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            switch (value.ToString())
            {
                case familiarized:
                    return true;
                case notFamiliarized:
                    return false;
            }
            return false;
        }
    }
}
