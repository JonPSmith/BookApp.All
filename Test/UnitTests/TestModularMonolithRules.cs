// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.Extensions.Logging;
using Test.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace Test.UnitTests
{
    public class TestModularMonolithRules
    {
        private const string NameToAllowMultipleAccessed = "Common";

        private readonly ITestOutputHelper _output;
        private readonly ModularMonolithRulesChecker _rulesChecker;

        public TestModularMonolithRules(ITestOutputHelper output)
        {
            _output = output;
            _rulesChecker = new ModularMonolithRulesChecker("BookApp",
                "ServiceLayer,Infrastructure,BizLogic,BizDbAccess,Persistence,Domain",
                "Main.*");
        }

        private LogLevel ShowLevel = LogLevel.Information;

        [Fact]
        public void CheckNoUnknownProjects()
        {
            //SETUP
            _rulesChecker.OutputMessage($"Checking no projects are left out of the tests", _output);

            //ATTEMPT
            var localProjectsList = _rulesChecker.ProjectsToScan.ToList();
            foreach (var layerName in _rulesChecker.LayersNamesInOrder)
            {
                var assembliesInLayer = localProjectsList.Where(x => x.LayerName == layerName).ToList();
                assembliesInLayer.ForEach(x => localProjectsList.Remove(x));
            }

            //VERIFY
            if (localProjectsList.Any())
            {
                var message =
                    "The following projects aren't in the valid layer names. Please add to ignore list if they are OK\n" +
                    string.Join("\n", localProjectsList.Select(x => x.ToString()));
                _rulesChecker.OutputMessage(message, _output , LogLevel.Error);
            }
            Assert.Equal(0, _rulesChecker.NumErrorsInTest);
        }

        [Fact]
        public void TestDifferentBoundedContextsDoNotMix()
        {
            //SETUP
            _rulesChecker.OutputMessage($"Checking that different bounded contexts only share the {NameToAllowMultipleAccessed} layer.", _output);

            //ATTEMPT
            var bContextDir = _rulesChecker.GetBoundedContextLayerWithTheirReferencedProjectNames();
            foreach (var firstBContextNames in bContextDir.Keys.Where(x => !x.Contains( NameToAllowMultipleAccessed)))
            {
                foreach (var secondBContextNames in bContextDir.Keys
                    .Where(x => x != firstBContextNames && !x.Contains(NameToAllowMultipleAccessed)))
                {
                    var badOverlaps = bContextDir[firstBContextNames]
                        .Where(x => bContextDir[secondBContextNames].Contains(x)
                              && !x.StartsWith($"{_rulesChecker.AppName}.{NameToAllowMultipleAccessed}"))
                        .ToList();
                    badOverlaps.ForEach(x => _rulesChecker.OutputMessage(
                            $"The {firstBContextNames} bounded context shares {x} with bounded context {secondBContextNames}",
                            _output, LogLevel.Error));
                }
            }

            //VERIFY
            Assert.Equal(0, _rulesChecker.NumErrorsInTest);
        }

        //--------------------------------------------------------------------------------

        [Fact]
        public void TestThatEachBoundedContextsObeysTheOneProjectPerLayer()
        {
            //SETUP

            //ATTEMPT
            var bContextGroup = _rulesChecker.ProjectsToScan.GroupBy(x => x.BContextName);
            foreach (var groupToCheck in bContextGroup)
            {
                var projectsInBContext = groupToCheck.ToList();

                foreach (var layerNames in _rulesChecker.CombinationOfLayerNames())
                {
                    var projectsInTopLayer = projectsInBContext
                        .Where(x => x.LayerName == layerNames.higherLayer).ToList();
                    foreach (var topLayerProject in projectsInTopLayer)
                    {
                        var projectsReferredToInLowerLayer = topLayerProject.PropertyAssembly.GetReferencedAssemblies()
                            .Where(x => x.Name.StartsWith(_rulesChecker.AppNameAndLayer(groupToCheck.Key, layerNames.lowerLayer)))
                            .ToList();

                        if (projectsReferredToInLowerLayer.Count > 1)
                        {
                                var message =
                                    $"Project {topLayerProject} links to multiple projects in layer {layerNames.lowerLayer}\n    " +
                                    string.Join(" and ", projectsReferredToInLowerLayer.Select(x => x.Name));
                            _rulesChecker.OutputMessage(message, _output, LogLevel.Error);
                        }
                    }
                }
            }


            //VERIFY
            Assert.Equal(0, _rulesChecker.NumErrorsInTest);
        }


        [Fact]
        public void TestLowerLayersDoNotDependOnHigherLayers()
        {
            //SETUP

            //ATTEMPT
            var bContextGroup = _rulesChecker.ProjectsToScan.GroupBy(x => x.BContextName);
            foreach (var groupToCheck in bContextGroup)
            {
                var projectsInBContext = groupToCheck.ToList();

                foreach (var layerNames in _rulesChecker.CombinationOfLayerNames())
                {
                    var projectsInLowerLayer = projectsInBContext
                        .Where(x => x.LayerName == layerNames.lowerLayer)
                        .ToList();

                    foreach (var projectInLowerLayer in projectsInLowerLayer)
                    {
                        var badReferencedProjects = projectInLowerLayer.PropertyAssembly.GetReferencedAssemblies()
                            .Where(x => x.Name.StartsWith(_rulesChecker.AppNameAndLayer(groupToCheck.Key, layerNames.higherLayer)))
                            .ToList();
                        if (badReferencedProjects.Any())
                        {
                            var message =
                                $"Project {projectInLowerLayer} links to projects in the higher layer {layerNames.higherLayer}\n    " +
                                string.Join(" and ", badReferencedProjects.Select(x => x.Name));
                            _rulesChecker.OutputMessage(message, _output, LogLevel.Error);
                        }
                    }
                }

                //VERIFY
                Assert.Equal(0, _rulesChecker.NumErrorsInTest);
            }
        }


        [Fact]
        public void TestOnlyAccessesProjectsInSameNameSpaceOtherThanCommon()
        {
            var bContextGroup = _rulesChecker.ProjectsToScan.GroupBy(x => x.BContextName);
            foreach (var groupToCheck in bContextGroup)
            {
                var projectsInBContext = groupToCheck.ToList();

                foreach (var layerName in _rulesChecker.LayersNamesInOrder)
                {
                    foreach (var assemblyToCheck in projectsInBContext.Where(x => x.LayerName == layerName))
                    {
                        var badLinks = assemblyToCheck.PropertyAssembly.GetReferencedAssemblies()
                            .Where(x => x.Name.StartsWith(_rulesChecker.AppNameAndLayer(groupToCheck.Key, layerName)) 
                                        && !x.Name.Contains("Common")).ToList();
                        if (badLinks.Any())
                        {
                            foreach (var assemblyName in badLinks)
                            {
                                _rulesChecker.OutputMessage(
                                    $"Assembly {assemblyToCheck} should not link to project {assemblyName.Name}",
                                    _output, LogLevel.Error);
                            }
                        }
                    }
                }

                //VERIFY
                Assert.Equal(0, _rulesChecker.NumErrorsInTest);
            }
        }


    }
}
