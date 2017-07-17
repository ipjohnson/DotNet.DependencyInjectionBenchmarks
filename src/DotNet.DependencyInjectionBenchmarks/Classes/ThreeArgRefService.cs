namespace DotNet.DependencyInjectionBenchmarks.Classes
{
	#region Interfaces
	public interface IThreeArgTransient1
	{
		
	}

	public interface IThreeArgTransient2
	{

	}

	public interface IThreeArgTransient3
	{

	}

	public interface IThreeArgRefService
	{
		IThreeArgTransient1 Transient1 { get; }

		IThreeArgTransient2 Transient2 { get; }

		IThreeArgTransient3 Transient3 { get; }
	}
	#endregion

	#region Impl

	public class ThreeArgTransient1 : IThreeArgTransient1
	{
		
	}

	public class ThreeArgTransient2 : IThreeArgTransient2
	{

	}

	public class ThreeArgTransient3 : IThreeArgTransient3
	{

	}

	public class ThreeArgRefService : IThreeArgRefService
	{
		public ThreeArgRefService(IThreeArgTransient1 transient1, IThreeArgTransient2 transient2, IThreeArgTransient3 transient3)
		{
			Transient1 = transient1;
			Transient2 = transient2;
			Transient3 = transient3;
		}

		public IThreeArgTransient1 Transient1 { get; }

		public IThreeArgTransient2 Transient2 { get; }

		public IThreeArgTransient3 Transient3 { get; }
	}

	#endregion
}
