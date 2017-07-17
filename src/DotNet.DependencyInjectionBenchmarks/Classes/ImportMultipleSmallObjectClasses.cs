namespace DotNet.DependencyInjectionBenchmarks.Classes
{
	public interface IImportMultipleSmallObject
	{
		ISmallObjectService SmallObject1 { get; }

		ISmallObjectService SmallObject2 { get; }
	}

	public class ImportMultipleSmallObject : IImportMultipleSmallObject
	{
	    public ImportMultipleSmallObject(ISmallObjectService smallObject1, ISmallObjectService smallObject2)
	    {
	        SmallObject1 = smallObject1;
	        SmallObject2 = smallObject2;
	    }

	    public ISmallObjectService SmallObject1 { get; }

		public ISmallObjectService SmallObject2 { get; }
	}
}
