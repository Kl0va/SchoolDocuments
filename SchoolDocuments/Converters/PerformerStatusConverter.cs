using SchoolDocuments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SchoolDocuments.Converters
{
    class PerformerStatusConverter : IValueConverter
    {
        const string waiting = "В ожидании";
        const string inProgress = "В процессе";
        const string completed = "Завершено";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((PerformerStatus)value == PerformerStatus.Waiting)
            {
                return waiting;
            }
            else if ((PerformerStatus)value == PerformerStatus.InProgress)
            {
                return inProgress;
            }
            else if ((PerformerStatus)value == PerformerStatus.Completed) return completed;

            return "Ничего";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            switch (value.ToString())
            {
                case completed:
                    return true;
                case waiting:
                    return false;
                case inProgress:
                    return false;
            }
            return false;
        }
    }
}
