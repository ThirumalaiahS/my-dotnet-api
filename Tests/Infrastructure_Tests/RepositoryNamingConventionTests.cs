using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Infrastructure_Tests
{
    public class RepositoryNamingConventionTests
    {
        private readonly Assembly _repositoryInterfacesAssembly;
        private readonly Assembly _repositoryClassesAssembly;
        public RepositoryNamingConventionTests()
        {
            _repositoryClassesAssembly = typeof(Infrastructure.AssemblyReference).Assembly;
            _repositoryInterfacesAssembly = typeof(Application.AssemblyReference).Assembly;
        }

        [Fact]
        public void AllRepositoryInterfaces_ShouldStartWithI_AndEndWithRepository()
        {
            var invalidRepoInterfaces = _repositoryInterfacesAssembly.GetTypes()
                .Where(t => t.IsInterface)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Application.Interfaces"))
                .Where(t => !t.Name.StartsWith("I") || !t.Name.EndsWith("Repository"))
                .ToList();

            Assert.True(invalidRepoInterfaces.Count == 0,
                $"The following interfaces do not follow the naming convention (should start with 'I' and end with 'Repository'):\n{string.Join("\n", invalidRepoInterfaces.Select(i => i.FullName))}");
        }

        [Fact]
        public void AllRepositoryClasses_ShouldEndWithRepository()
        {
            var inavalidRepoClasses = _repositoryClassesAssembly.GetTypes()
                .Where(t => t.IsClass)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Infrastructure.Repositories"))
                .Where(t => !t.Name.EndsWith("Repository"))
                .ToList();

            Assert.True(inavalidRepoClasses.Count == 0,
                $"The following classes do not follow the naming convention (should end with 'Repository'):\n{string.Join("\n", inavalidRepoClasses.Select(c => c.FullName))}");
        }

        [Fact]
        public void AllRepositoryClasses_ShouldImplementCorrespondingInterfaces()
        {
            var repoClasses = _repositoryClassesAssembly.GetTypes()
                .Where(x => x.IsClass && x.Namespace != null && x.Namespace.Contains("Infrastructure.Repositories"))
                .ToList();

            var repoInterfaces = _repositoryInterfacesAssembly.GetTypes()
                .Where(x => x.IsInterface && x.Namespace != null && x.Namespace.Contains("Application.Interfaces"))
                .ToList();

            foreach (var repoClass in repoClasses)
            {
                var interfaceName = "I" + repoClass.Name;
                var correspondingInterface = repoInterfaces.FirstOrDefault(x => x.Name == interfaceName);
                Assert.NotNull(correspondingInterface);
                Assert.True(correspondingInterface.IsAssignableFrom(repoClass),
                    $"{repoClass.FullName} does not implement {correspondingInterface.FullName}");
            }
        }
    }
}
