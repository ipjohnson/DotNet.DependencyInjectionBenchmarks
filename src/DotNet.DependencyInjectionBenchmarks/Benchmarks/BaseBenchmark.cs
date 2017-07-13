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
        public static readonly int ExtraRegistrationsCount = 200;
        public static readonly int ExtraRegistrationsResolveCount = ExtraRegistrationsCount / 2;

        protected IContainer AutofacContainer;
        protected IContainer CastleWindsorContainer;
        protected IContainer DryIocContainer;
        protected IContainer GraceContainer;
        protected IContainer LightInjectContainer;
        protected IContainer MicrosoftDependencyInjectionContainer;
        protected IContainer NInjectContainer;
        protected IContainer SimpleInjectorContainer;
        protected IContainer StructureMapContainer;

        protected IContainer CreateAutofacContainer()
        {
            return AutofacContainer = new AutofacContainer();
        }

        protected IContainer CreateCastleWindsorContainer()
        {
            return CastleWindsorContainer = new CastleWindsorContainer();
        }

        protected IContainer CreateDryIocContainer()
        {
            return DryIocContainer = new DryIocContainer();
        }

        protected IContainer CreateGraceContainer()
        {
            return GraceContainer = new GraceContainer();
        }

        protected IContainer CreateLightInjectContainer()
        {
            return LightInjectContainer = new LightInjectContainer();
        }

        protected IContainer CreateMicrosoftDependencyInjectionContainer()
        {
            return MicrosoftDependencyInjectionContainer = new MicrosoftDependencyInjectionContainer();
        }

        protected IContainer CreateNInjectContainer()
        {
            return NInjectContainer = new NInjectContainer();
        }

        protected IContainer CreateSimpleInjectorContainer()
        {
            return SimpleInjectorContainer = new SimpleInjectorContainer();
        }

        protected IContainer CreateStructureMapContainer()
        {
            return StructureMapContainer = new StructureMapContainer();
        }

        /// <summary>
        /// Registers definitions and dummy classes for scope
        /// </summary>
        /// <param name="container"></param>
        /// <param name="definitions"></param>
        /// <param name="resolveStatements"></param>
        protected void SetupContainerForTest(IContainer container, IEnumerable<RegistrationDefinition> definitions, params Action<IResolveScope>[] resolveStatements)
        {
            var dummyTypes = DummyClasses.GetTypes(ExtraRegistrationsCount).ToArray();

            container.Registration(dummyTypes.Select(t => new RegistrationDefinition { ExportType = t, ActivationType = t }));

            var definitionArray = definitions.ToArray();

            container.Registration(definitionArray);

            container.BuildContainer();

            var resolveTypes = new List<Type>(dummyTypes.Take(ExtraRegistrationsResolveCount));

            if (resolveStatements != null && resolveStatements.Length > 0)
            {
                var gap = resolveTypes.Count / resolveStatements.Length;

                var index = gap / 2;

                for (var i = 0; i < resolveTypes.Count; i++)
                {
                    if (index < resolveStatements.Length && i == index * gap)
                    {
                        resolveStatements[index](container);
                        index++;
                    }

                    container.Resolve(resolveTypes[i]);
                }
            }
            else
            {
                var gap = resolveTypes.Count / definitionArray.Length;

                var index = gap / 2;

                foreach (var definition in definitionArray)
                {
                    resolveTypes.Insert(index, definition.ExportType);

                    index += gap + 1;
                }

                foreach (var resolveType in resolveTypes)
                {
                    container.Resolve(resolveType);
                }
            }
        }
    }
}
