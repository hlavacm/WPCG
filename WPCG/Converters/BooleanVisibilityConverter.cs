using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Netcorex.Converters
{
	public class BooleanVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool? current = value as bool?;
			switch (current)
			{
				case true:
					return Visibility.Visible;
				case false:
					return Visibility.Collapsed;
				case null:
					return Visibility.Hidden;
				default:
					throw new NotImplementedException(string.Format("Nullable Boolean: {0}", current));
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			Visibility current;
			if (Enum.TryParse(value.ToString(), out current))
			{
				switch (current)
				{
					case Visibility.Visible:
						return true;
					case Visibility.Collapsed:
						return false;
					case Visibility.Hidden:
						return null;
					default:
						throw new NotImplementedException(string.Format("Visibility: {0}", current));
				}
			}
			throw new InvalidCastException(string.Format("{0} to Visibility", value));
		}
	}
}
