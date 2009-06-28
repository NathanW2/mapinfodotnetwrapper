using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.Core.IoC
{
    internal static class Bootstrapper
    {
        public static void WireUp(IMapinfoWrapper mapinfoInstance)
        {
            DependencyResolver resolver = new DependencyResolver();
            IFactoryBuilder builder = new FactoryBuilder(mapinfoInstance);

            resolver.Register<IMapinfoWrapper>(mapinfoInstance);
            resolver.Register<IFactoryBuilder>(builder);
            ServiceLocator.Initialize(resolver);
        }
    }
}
