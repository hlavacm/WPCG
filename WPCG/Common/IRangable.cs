namespace Netcorex.Common
{
	public interface IRangable
	{
		long Value { get; set; }
		long Minimum { get; }
		long Maximum { get; }
		long SmallChange { get; }
		long LargeChange { get; }
	}
}
