using System;
using System.IO;
using System.Threading.Tasks;
using Jering.Javascript.NodeJS;
using RessurectIT.Git.Version.Misc;
using Microsoft.Extensions.Configuration;

namespace RessurectIT.Git.Version
{
    /// <summary>
    /// Main program configuration
    /// </summary>
    public class Program
    {
        #region public methods - app entry

        /// <summary>
        /// Application entry method
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static async Task<int> Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            Config config = new Config();

            configuration.Bind(config);

            if (config.BuildNumber.HasValue && config.BuildNumber.Value == -1)
            {
                config.BuildNumber = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
            }

            if(string.IsNullOrWhiteSpace(config.WorkingDirectory))
            {
                config.WorkingDirectory = null;
            }

            if(config.WorkingDirectory != null)
            {
                config.WorkingDirectory = config.WorkingDirectory.TrimEnd(' ').TrimEnd('\\').TrimEnd('/');
            }

            const string nodeExe = "node.exe";
            string nodePath = nodeExe;

            if (!string.IsNullOrEmpty(config.NodeJsPath))
            {
                nodePath = config.NodeJsPath;
            }

            if(!string.IsNullOrEmpty(config.NodeJsDirs))
            {
                string[] dirs = config.NodeJsDirs.Split(';');

                foreach (string dir in dirs)
                {
                    string path = Path.Combine(dir, nodePath);

                    if (File.Exists(path))
                    {
                        StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ExecutablePath = path);

                        break;
                    }
                }
            }

            try
            {
                string version = await StaticNodeJSService.InvokeFromFileAsync<string>("getVersion.js",
                                                                                       null,
                                                                                       new object[]
                                                                                       {
                                                                                           new
                                                                                           {
                                                                                               config.BranchName,
                                                                                               config.BuildNumber,
                                                                                               config.CurrentVersion,
                                                                                               config.IgnoreBranchPrefix,
                                                                                               config.Pre,
                                                                                               config.Suffix,
                                                                                               config.TagPrefix,
                                                                                               config.WorkingDirectory,
                                                                                               noStdOut = true
                                                                                           }
                                                                                       });

                Console.WriteLine(version);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to generated version {e}");

                return -1;
            }

            return 0;
        }
        #endregion
    }
}
