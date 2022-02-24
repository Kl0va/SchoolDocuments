using SchoolDocuments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SchoolDocuments.Converters
{
    class StatusConverter : IValueConverter
    {
        const string familiarized = "Не согласовано";
        const string notFamiliarized = "Согласовано";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((AgreementStatus)value == AgreementStatus.Sent || (AgreementStatus)value == AgreementStatus.Declined)
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
