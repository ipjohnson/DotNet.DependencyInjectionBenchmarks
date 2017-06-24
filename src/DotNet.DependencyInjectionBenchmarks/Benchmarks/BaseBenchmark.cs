using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks
{
    public abstract class BaseBenchmark
    {
        protected IContainerScope AutofacScope;
        protected IContainerScope DryIocScope;
        protected IContainerScope GraceScope;
        protected IContainerScope LightInjectScope;
        protected IContainerScope SimpleInjectorScope;

        protected IContainerScope CreateAutofacScope()
        {
            return AutofacScope = new AutofacContainer();
        }
        
        protected IContainerScope CreateDryIocScope()
        {
            return DryIocScope = new DryIocContainer();
        }

        protected IContainerScope CreateGraceScope()
        {
            return GraceScope = new GraceContainer();
        }
        
        protected IContainerScope CreateLightInjectScope()
        {
            return LightInjectScope = new LightInjectContainer();
        }

        protected IContainerScope CreateSimpleInjectorContainerScope()
        {
            return SimpleInjectorScope = new SimpleInjectorContainerScope();
        }

        /// <summary>
        /// Registers definitions and dummy classes for scope
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="definitions"></param>
        /// <param name="resolveStatements"></param>
        protected void SetupScopeForTest(IContainerScope scope, IEnumerable<RegistrationDefinition> definitions, params Action<IResolveScope>[] resolveStatements)
        {
            var dummyTypes = DummyClasses.GetTypes(200).ToArray();
            
            scope.Registration(dummyTypes.Select(t => new RegistrationDefinition{ ExportType = t, ActivationType = t}));

            var definitionArray = definitions.ToArray();

            scope.Registration(definitionArray);

            scope.BuildContainer();

            if (resolveStatements != null && resolveStatements.Length > 0)
            {
                var resolveTypes = new List<Type>(dummyTypes.Take(50));

                var gap = resolveTypes.Count / resolveStatements.Length;

                var index = 0;

                for (var i = 0; i < resolveTypes.Count; i++)
                {
                    if (index < resolveStatements.Length && i == index * gap)
                    {
                        resolveStatements[index](scope);
                        index++;
                    }
                    
                    scope.Resolve(resolveTypes[i]);
                }
            }
            else
            {
                var resolveTypes = new List<Type>(dummyTypes.Take(50));

                var gap = resolveTypes.Count / definitionArray.Length;

                var index = 0;

                foreach (var definition in definitionArray)
                {
                    resolveTypes.Insert(index, definition.ExportType);

                    index += gap + 1;
                }

                foreach (var resolveType in resolveTypes)
                {
                    scope.Resolve(resolveType);
                }
            }
        }
    }
}
