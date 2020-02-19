using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ZadaniaWPF.ViewModel;

namespace ZadaniaWPF.Converters
{
	public class TaskConverter : IMultiValueConverter
	{
		PriorityConverter priorityConverter = new PriorityConverter();

		public object Convert(object[] values, Type targetType, object parameter,
							  CultureInfo culture)
		{
			string opis = (string)values[0];
			DateTime dataUtworzenia = DateTime.Now;
			DateTime? planowanyTerminRealizacji = (DateTime?)values[1];
			Model.Priority priorytet = (Model.Priority)priorityConverter.ConvertBack(
				values[2], typeof(Model.Priority), null,
				CultureInfo.CurrentCulture);
			if (!string.IsNullOrWhiteSpace(opis) && planowanyTerminRealizacji.HasValue)
				return new TaskVM(1,opis, dataUtworzenia,
					planowanyTerminRealizacji.Value, priorytet, false);
			else return null;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
									CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
