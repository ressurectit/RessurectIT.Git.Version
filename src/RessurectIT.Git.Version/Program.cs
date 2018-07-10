using System;
using System.Threading.Tasks;
using RessurectIT.Git.Version.Misc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .AddEnvironmentVariables("AGV_")
                .Build();

            Config config = new Config();

            configuration.Bind(config);

            ServiceCollection services = new ServiceCollection();
            services.AddNodeServices();

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            INodeServices nodeServices = serviceProvider.GetRequiredService<INodeServices>();

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
                config.WorkingDirectory = config.WorkingDirectory.TrimEnd('\\');
            }

            try
            {
                string version = await nodeServices.InvokeAsync<string>("./getVersion",
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
