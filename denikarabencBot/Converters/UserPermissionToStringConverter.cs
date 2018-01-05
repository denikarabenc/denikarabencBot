using Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace denikarabencBot.Converters
{
    public class UserPermissionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enum.GetName(value.GetType(), value);
            return UserPermissionStringMapping(Enum.GetName(value.GetType(), value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string UserPermissionStringMapping(string userType)
        {
            switch (userType)
            {
                case "Editor":
                    return "Editor";                   
                case "Follower":
                    return "Follower";     
                case "King":
                    return "Broadcaster";
                case "Mod":
                    return "Moderator";
                case "Regular":
                    return "Any user";
                case "Sub":
                    return "Subscriber";
                default:
                    BotLogger.Logger.Log(LoggingType.Warning, "[UserPermissionToStringConverter] - > User is not known type");
                    return "Error getting user";
            }
        }
    }
}
