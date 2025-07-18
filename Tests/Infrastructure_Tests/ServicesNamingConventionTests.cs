using System.Reflection;

namespace Tests.Infrastructure_Tests.Tests
{
    public class ServicesNamingConventionTests
    {
        private readonly Assembly _serviceInterfacesAssembly;
        private readonly Assembly _serviceClassesAssemblyAssembly;
        public ServicesNamingConventionTests()
        {
            _serviceInterfacesAssembly = typeof(Application.AssemblyReference).Assembly;
            _serviceClassesAssemblyAssembly = typeof(Application.AssemblyReference).Assembly;
        }

        [Fact]
        public void AllServiceInterfaces_ShouldStartWithI_AndEndWithService()
        {
            var invalidInterfaces = _serviceInterfacesAssembly.GetTypes()
                .Where(t => t.IsInterface)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Application.Services"))
                .Where(t => !t.Name.StartsWith("I") || !t.Name.EndsWith("Service"))
                .ToList();

            Assert.True(invalidInterfaces.Count == 0,
                $"The following interfaces do not follow the naming convention (should start with 'I' and end with 'Service'):\n{string.Join("\n", invalidInterfaces.Select(i => i.FullName))}");
        }

        [Fact]
        public void AllServiceClasses_ShouldEndWithService()
        {
            var invalidClasses = _serviceClassesAssemblyAssembly.GetTypes()
                .Where(t => t.IsClass)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Infrastructure.Services"))
                .Where(t => !t.Name.EndsWith("Service"))
                .ToList();

            Assert.True(invalidClasses.Count == 0,
                $"The following classes do not follow the naming convention (should end with 'Service'):\n{string.Join("\n", invalidClasses.Select(c => c.FullName))}");
        }

        [Fact]
        public void AllServiceClasses_ShouldImplementCorrespondingInterfaces()
        {
            var serviceClasses = _serviceClassesAssemblyAssembly.GetTypes()
                .Where(t => t.IsClass && t.Namespace != null && t.Namespace.Contains("Infrastructure.Services"))
                .ToList();
            var serviceInterfaces = _serviceInterfacesAssembly.GetTypes()
                .Where(t => t.IsInterface && t.Namespace != null && t.Namespace.Contains("Application.Services"))
                .ToList();
            foreach (var serviceClass in serviceClasses)
            {
                var interfaceName = "I" + serviceClass.Name;
                var correspondingInterface = serviceInterfaces.FirstOrDefault(i => i.Name == interfaceName);
                Assert.NotNull(correspondingInterface);
                Assert.True(correspondingInterface.IsAssignableFrom(serviceClass),
                    $"{serviceClass.FullName} does not implement {correspondingInterface.FullName}");
            }
        }
    }
}
