using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.DependencyInjectionBenchmarks.Containers
{
    public enum RegistrationMode
    {
        Single,
        Multiple,
    }

    public enum RegistrationLifestyle
    {
        Transient,
        Singleton,
        SingletonPerScope,
        SingletonPerObjectGraph,
        SingletonPerNamedScope,
        SingletonPerAncestor
    }

    public class RegistrationDefinition
    {
        public Type ActivationType { get; set; }

        public Type ExportType { get; set; }

        public object ExportKey { get; set; }

        public RegistrationMode RegistrationMode { get; set; } = RegistrationMode.Single;

        public RegistrationLifestyle RegistrationLifestyle { get; set; } = RegistrationLifestyle.Transient;

        public object LifestyleInformation { get; set; }

        public Dictionary<object, object> Metadata { get; set; }
    }

    public interface IResolveScope : IDisposable
    {
        IResolveScope CreateScope(string scopeName = "");

        object Resolve(Type type);

        object Resolve(Type type, object data);

        bool TryResolve(Type type, object data, out object value);
    }

    public static class IResolveScopeExtensions
    {
        public static T Resolve<T>(this IResolveScope scope)
        {
            return (T)scope.Resolve(typeof(T));
        }
    }

    public interface IContainer : IResolveScope
    {
        void BuildContainer();

        void Registration(IEnumerable<RegistrationDefinition> definitions);

        void RegisterFactory<TResult>(Func<TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle);

        void RegisterFactory<T1, TResult>(Func<T1, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle);

        void RegisterFactory<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle);
    }

    public static class ContainerScopeExtensions
    {
        public static void Register<TInterface, TImplementation>(this IContainer scope,
            RegistrationLifestyle lifestyle = RegistrationLifestyle.Transient) where TImplementation : TInterface
        {
            scope.Registration(new[] { new RegistrationDefinition { ExportType = typeof(TInterface), ActivationType = typeof(TImplementation) } });
        }
    }
}
