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
            string TrimSlashes(string value) => value.TrimEnd(' ').TrimEnd('\\').TrimEnd('/');

            string? SetNullIfEmpty(string? value) => value switch
            {
                var val when string.IsNullOrWhiteSpace(val) => null,
                string val => TrimSlashes(val),
                _ => value,
            };

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            Config config = configuration.Get<Config>()!;

            config.WorkingDirectory = SetNullIfEmpty(config.WorkingDirectory);
            config.BranchName = SetNullIfEmpty(config.BranchName);
            config.TagPrefix = SetNullIfEmpty(config.TagPrefix);
            config.IgnoreBranchPrefix = SetNullIfEmpty(config.IgnoreBranchPrefix);
            config.Suffix = SetNullIfEmpty(config.Suffix);
            config.CurrentVersion = SetNullIfEmpty(config.CurrentVersion);
            config.ConfigPath = SetNullIfEmpty(config.ConfigPath);

            config = config switch
            {
                var cfg when string.IsNullOrWhiteSpace(cfg.NodeJsDirs) => throw new ArgumentNullException($"Missing '{nameof(Config.NodeJsDirs)}'!"),
                var cfg when string.IsNullOrWhiteSpace(cfg.NodeJsPath) => throw new ArgumentNullException($"Missing '{nameof(Config.NodeJsPath)}'!"),
                _ => config,
            };

            string[] dirs = config.NodeJsDirs.Split(';');

            foreach (string dir in dirs)
            {
                string path = Path.Combine(TrimSlashes(dir), config.NodeJsPath);

                if (File.Exists(path))
                {
                    StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ExecutablePath = path);

                    break;
                }
            }

            try
            {
                string? version = await StaticNodeJSService.InvokeFromFileAsync<string>("getVersion.js",
                                                                                        null,
                                                                                        [
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
                                                                                                config.NoIncrement,
                                                                                                config = config.ConfigPath,
                                                                                                noStdOut = true
                                                                                            }
                                                                                        ]);

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
