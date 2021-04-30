// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Test.TestHelpers
{
    public class ModularMonolithRulesChecker
    {
        private const string AllProjectSelect = ".*";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="layersInOrderString">comma delimited list of layer names, with highest first</param>
        /// <param name="ignoreProjectsString">comma delimited list of project names to exclude: two formats for each name
        /// "Main.FrontEnd" => ${AppName}.Main.FrontEnd"
        /// "Main.*" => all projects starting with ${AppName}.Main"
        /// </param>
        public ModularMonolithRulesChecker(string appName, string layersInOrderString, string ignoreProjectsString = null)
        {
            AppName = appName;
            ProjectsToScan = GetProjectAssemblies(ignoreProjectsString).ToList();
            LayersNamesInOrder = layersInOrderString?.Split(',').Select(x => x.Trim()).ToArray()
                            ?? throw new ArgumentNullException(nameof(layersInOrderString),
                                "You must provide a comma delimited list of the layer names in order, with highest first");
        }

        public string AppName { get; }

        public List<ModMonProject> ProjectsToScan { get; }

        public string[] LayersNamesInOrder { get; }

        /// <summary>
        /// The OutputMessage will out output something if the messageLevel >= than the property
        /// </summary>
        public LogLevel ShowLevel { get; set; } = LogLevel.Information;

        public int NumErrorsInTest { get; private set; }

        public string AppNameAndLayer(string bContextName, string layerName)
        {
            return $"{AppName}.{bContextName}.{layerName}";
        }

        public Dictionary<string, List<string>> GetBoundedContextLayerWithTheirReferencedProjectNames()
        {
            return ProjectsToScan.GroupBy(x => x.BContextName)
                .ToDictionary(x => x.Key,
                    y => y.SelectMany(z =>
                        z.PropertyAssembly.GetReferencedAssemblies().Select(a => a.Name)
                            .Where(a => a.StartsWith(AppName))).ToList());
        }


        public IEnumerable<(string higherLayer, string lowerLayer)> CombinationOfLayerNames()
        {

            for (int i = 1; i < LayersNamesInOrder.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    yield return (LayersNamesInOrder[j], LayersNamesInOrder[i]);
                }
            }
        }

        public void OutputMessage(string message, ITestOutputHelper output, LogLevel messageLevel = LogLevel.Information)
        {
            if (messageLevel < ShowLevel)
                return;

            switch (messageLevel)
            {
                case LogLevel.Information:
                    output.WriteLine(message);
                    break;
                case LogLevel.Warning:
                    output.WriteLine($"Warning: {message}");
                    break;
                case LogLevel.Error:
                    output.WriteLine($"ERROR: {message}");
                    NumErrorsInTest++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageLevel), messageLevel, "Only some levels accepted");
            }
        }

        //---------------------------------------------------------------------------
        //private

        /// <summary>
        /// This finds all the projects (assemblies) that the unit test is is linked to and filters them
        /// to only look at the projects starting with the NameSpacePrefix constant and not in the ignore list
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ModMonProject> GetProjectAssemblies(string ignoreProjectsString)
        {

            //see https://stackoverflow.com/a/55672480/1434764
            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var assembleFilesStartingWithAppName = Directory.GetFiles(assemblyFolder, $"{AppName}*.dll");
            var ignoreProjectNames = FormIgnoreNamesFrom(ignoreProjectsString, assembleFilesStartingWithAppName);

            foreach (var path in Directory.GetFiles(assemblyFolder, $"{AppName}*.dll"))
            {
                if (ignoreProjectNames.Contains(Path.GetFileNameWithoutExtension(path)))
                    continue;

                yield return new ModMonProject(Assembly.LoadFrom(path));
            }
        }

        private List<string> FormIgnoreNamesFrom(string ignoreProjectsString, string[] allProjectFileNames)
        {
            var result = new List<string>();
            foreach (var ignoreName in ignoreProjectsString?.Split(',').Select(x => x.Trim())
                .Select(x => $"{AppName}.{x}") ?? new string[0])
            {
                if(ignoreName.EndsWith(AllProjectSelect))
                    result.AddRange(allProjectFileNames
                        .Select(x => Path.GetFileNameWithoutExtension(x))
                        .Where(x => x.StartsWith(ignoreName.Substring(0, ignoreName.Length - AllProjectSelect.Length))));
                else
                {
                    result.Add(ignoreName);
                }
            }

            return result;
        }
    }
}