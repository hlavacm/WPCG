using System;

namespace Netcorex.Common
{
	public struct WpItemStruct : IEquatable<WpItemStruct>, IComparable<WpItemStruct>, IComparable, ICloneable
	{
		private readonly int _id;
		private readonly string _title;
		private readonly string _tag;


		public WpItemStruct(int id, string title, string tag = null)
		{
			_id = id;
			_title = title;
			_tag = tag;
		}


		public int Id
		{
			get { return _id; }
		}
		public string Title
		{
			get { return _title; }
		}
		public string Tag
		{
			get { return _tag; }
		}


		public bool Equals(WpItemStruct other)
		{
			return Equals(Id, other.Id) && Equals(Title, other.Title) && Equals(Tag, other.Tag);
		}

		public int CompareTo(WpItemStruct other)
		{
			int result = Id.CompareTo(other.Id);
			if (result == 0)
			{
				result = string.CompareOrdinal(Title, other.Title);
				if (result == 0)
				{
					result = string.CompareOrdinal(Tag, other.Tag);
				}
			}
			return result;
		}

		public int CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			return CompareTo((WpItemStruct)other);
		}

		public object Clone()
		{
			return new WpItemStruct(Id, Title, Tag);
		}
	}
}
