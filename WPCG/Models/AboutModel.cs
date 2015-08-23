using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Netcorex.Localizations;

namespace Netcorex.Models
{
	public class AboutModel : ModelBase
	{
		private string _text;


		public string Text
		{
			get { return _text; }
			set { SetProperty(ref _text, value); }
		}


		public override void Dispose()
		{
			try
			{
				_text = null;
			}
			finally
			{
				base.Dispose();
			}
		}


		protected override void Initialize()
		{
			try
			{
				Assembly assembly = Assembly.GetExecutingAssembly();

				AssemblyTitleAttribute titleAttribute = GetAssemblyAttribute<AssemblyTitleAttribute>(assembly);
				AssemblyDescriptionAttribute descriptionAttribute = GetAssemblyAttribute<AssemblyDescriptionAttribute>(assembly);
				AssemblyCompanyAttribute companyAttribute = GetAssemblyAttribute<AssemblyCompanyAttribute>(assembly);
				AssemblyProductAttribute productAttribute = GetAssemblyAttribute<AssemblyProductAttribute>(assembly);
				AssemblyCopyrightAttribute copyrightAttribute = GetAssemblyAttribute<AssemblyCopyrightAttribute>(assembly);

				string bigSeparator = new string('=', 30);
				string smallSeparator = new string('-', 30);

				StringBuilder builder = new StringBuilder();
				builder.AppendLine(bigSeparator);
				builder.AppendLine(string.Format("{0} ({1})", titleAttribute.Title, productAttribute.Product));
				builder.AppendLine(smallSeparator);
				builder.AppendLine(string.Format("{0}: v{1} aplha", Titles.Version, assembly.GetName().Version));
				builder.AppendLine(smallSeparator);
				builder.AppendLine(string.Format("{0}, {1}", descriptionAttribute.Description, companyAttribute.Company));
				builder.AppendLine(smallSeparator);
				builder.AppendLine(string.Format("{0}", copyrightAttribute.Copyright));
				builder.AppendLine(bigSeparator);
				builder.AppendLine(Titles.Icons);
				builder.AppendLine(bigSeparator);
				builder.AppendLine(string.Format("Fugue Icons"));
				builder.AppendLine(string.Format("https://github.com/yusukekamiyamane/fugue-icons"));
				builder.AppendLine(string.Format("Copyright © 2013 Yusuke Kamiyamane"));
				builder.AppendLine(bigSeparator);
				builder.AppendLine(Titles.ThirdParties);
				builder.AppendLine(bigSeparator);
				builder.AppendLine(string.Format("MySql.Data - ADO.Net driver for MySQL"));
				builder.AppendLine(string.Format("http://dev.mysql.com/downloads/"));
				builder.AppendLine(string.Format("Copyright © MySQL, Oracle Corporation"));
				builder.AppendLine(smallSeparator);
				builder.AppendLine(string.Format("DbExtensions - data-access API"));
				builder.AppendLine(string.Format("https://github.com/maxtoroq/DbExtensions"));
				builder.AppendLine(string.Format("Copyright © Max Toro Q."));
				builder.AppendLine(smallSeparator);
				builder.AppendLine(string.Format("Extended WPF Toolkit"));
				builder.AppendLine(string.Format("http://wpftoolkit.codeplex.com"));
				builder.AppendLine(string.Format("Copyright © Xceed"));
				builder.AppendLine(bigSeparator);
				_text = builder.ToString();
			}
			finally
			{
				base.Initialize();
			}
		}

		private T GetAssemblyAttribute<T>(Assembly assembly)
			where T : Attribute
		{
			return (T)assembly.GetCustomAttributes(typeof(T), true).SingleOrDefault();
		}
	}
}
