using System.Collections.Generic;
using Netcorex.Common;

namespace Netcorex.Models
{
	public class GenerationModel : ModelBase, IRangable
	{
		public const long DefaultMinimum = 1;
		public const long DefaultMaximum = 1000;
		public const long DefaultSmallChange = 1;
		public const long DefaultLargeChange = 10;
		private long _value = 10;


		public long Value
		{
			get { return _value; }
			set { SetProperty(ref _value, value); }
		}
		public long Minimum
		{
			get { return DefaultMinimum; }
		}
		public long Maximum
		{
			get { return DefaultMaximum; }
		}
		public long SmallChange
		{
			get { return DefaultSmallChange; }
		}
		public long LargeChange
		{
			get { return DefaultLargeChange; }
		}


		public IList<long> GetGenerateItemsIds()
		{
			List<long> generateItemsIds = new List<long>();
			long currentValue = Minimum;
			long maximum = Value + 1;
      while (currentValue < maximum)
			{
				generateItemsIds.Add(currentValue++);
			}
			return generateItemsIds;
		}
	}
}
