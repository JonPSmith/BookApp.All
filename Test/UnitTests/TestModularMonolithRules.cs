// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Test.UnitTests
{
    public class TestModularMonolithRules
    {

        private readonly ITestOutputHelper _output;

        public TestModularMonolithRules(ITestOutputHelper output)
        {
            _output = output;
        }


        /// <summary>
        /// This should set this constant to the prefix for the project names
        /// </summary>
        private const string ProjectPrefix = "BookApp.";

        /// <summary>
        /// This should define the prefix of projects in each layer.
        /// Note that they should be in order, with the higher levels coming first 
        /// </summary>
        private readonly string[] _layersPrefixInOrder = new[]
        {
            $"{ProjectPrefix}ServiceLayer",
            $"{ProjectPrefix}Infrastructure",
            $"{ProjectPrefix}BizLogic",
            $"{ProjectPrefix}BizDbAccess",
            $"{ProjectPrefix}Persistence",
            $"{ProjectPrefix}Domain",
        };

        /// <summary>
        /// This should contain the project names to ignore
        /// </summary>
        private static readonly string[] _assembliesToIgnore = new[]
        {
            $"{ProjectPrefix}AppSetup",
            $"{ProjectPrefix}Test",
        };

        //Holds all the the assemblies starting with the prefix
        private static readonly List<Assembly> AllAppAssemblies = GetAppAssemblies().ToList();

        /// <summary>
        /// This finds all the projects (assemblies) that the unit test is is linked to and filters them
        /// to only look at the projects starting with the NameSpacePrefix constant and not in the ignore list
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Assembly> GetAppAssemblies()
        {
            //see https://stackoverflow.com/a/55672480/1434764
            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (var path in Directory.GetFiles(assemblyFolder, $"{ProjectPrefix}*.dll"))
            {
                if (_assembliesToIgnore.Contains( Path.GetFileNameWithoutExtension(path)))
                    continue;

                yield return Assembly.LoadFrom(path);
            }
        }

        /// <summary>
        /// This returns the a list of two layer names: a higher level and a lower level
        /// This list provides every combination of higher and lower levels so that the code
        /// can check that lower levels don't reference a higher level
        /// </summary>
        /// <returns>
        /// higherLayer = a higher layer name
        /// lowerLayer = a lower layer that should not refer to the higher layer
        /// </returns>
        private IEnumerable<(string higherLayer, string lowerLayer)> CheckNotAccessingOuterLayers()
        {
            for (int i = 1; i < _layersPrefixInOrder.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    yield return (_layersPrefixInOrder[i], _layersPrefixInOrder[j]);
                }
            }
        }

        [Fact]
        public void CheckNoUnknownProjects()
        {
            //SETUP
            var hasErrors = false;
            var assemblies = AllAppAssemblies.ToList();

            //ATTEMPT
            foreach (var prefix in _layersPrefixInOrder)
            {
                var assembliesInLayer = assemblies.Where(x => x.GetName().Name.StartsWith(prefix)).ToList();
                assembliesInLayer.ForEach(x => assemblies.Remove(x));
            }

            //VERIFY
            if (assemblies.Any())
            {
                _output.WriteLine(
                    "The following projects aren't in the valid layer names. Please add to ignore list if they are OK");
                _output.WriteLine(string.Join(", ", assemblies.Select(x => x.GetName().Name)));
                Assert.False(hasErrors);
            }
        }

        [Fact]
        public void TestLowerLayersDoNotDependOnHigherLayers()
        {
            //SETUP
            var hasErrors = false;

            //ATTEMPT
            foreach (var namespacesPrefix in CheckNotAccessingOuterLayers())
            {
                var assembliesToCheck = AllAppAssemblies
                    .Where(x => x.GetName().Name.StartsWith(namespacesPrefix.higherLayer)).ToArray();
                _output.WriteLine(assembliesToCheck.Any()
                    ? $"Checking {namespacesPrefix.higherLayer}.. does not rely on a {namespacesPrefix.lowerLayer}"
                    : $"No projects found in {namespacesPrefix.higherLayer}.. namespace");

                foreach (var assemblyToCheck in assembliesToCheck)
                {
                    var badLinks = assemblyToCheck.GetReferencedAssemblies()
                        .Where(x => x.Name.StartsWith(namespacesPrefix.lowerLayer)).ToList();
                    if (badLinks.Any())
                    {
                        hasErrors = true;
                        foreach (var assemblyName in badLinks)
                        {
                            _output.WriteLine($"Assembly {assemblyToCheck.GetName().Name} should not link to project {assemblyName.Name}");
                        }
                    }
                }

                //VERIFY
                Assert.False(hasErrors);
            }
        }

        [Fact]
        public void TestOnlyAccessesProjectsInSameNameSpaceOtherThanCommon()
        {
            var hasErrors = false;
            foreach (var namespacePrefix in _layersPrefixInOrder)
            {
                var assembliesToCheck = AllAppAssemblies.Where(x => x.GetName().Name.StartsWith(namespacePrefix)).ToArray();
                _output.WriteLine(assembliesToCheck.Any()
                    ? $"Check {namespacePrefix}.. for linking to project in same layer that hasn't got \"Common\" in its name"
                    : $"No projects found in {namespacePrefix}.. namespace");

                _output.WriteLine($"Check {namespacePrefix}.. for linking to project in same layer that hasn't got \"Common\" in its name");
                foreach (var assemblyToCheck in AllAppAssemblies.Where(x => x.GetName().Name.StartsWith(namespacePrefix)))
                {
                    var badLinks = assemblyToCheck.GetReferencedAssemblies()
                        .Where(x => x.Name.StartsWith(namespacePrefix) && !x.Name.Contains("Common")).ToList();
                    if (badLinks.Any())
                    {
                        hasErrors = true;
                        foreach (var assemblyName in badLinks)
                        {
                            _output.WriteLine($"Assembly {assemblyToCheck.GetName().Name} should not link to project {assemblyName.Name}");
                        }
                    }
                }

                //VERIFY
                Assert.False(hasErrors);
            }
        }


    }
}
