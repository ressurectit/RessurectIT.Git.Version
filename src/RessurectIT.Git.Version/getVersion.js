import {VersionsExtractor, processEnvCfg, processCfgFile, updateBuildNumber} from 'npm-git-version';

export default async options =>
{
    const envConfig = processEnvCfg();
    const fileConfig = processCfgFile({config: options.config ?? envConfig.config, noStdOut: options.noStdOut, workingDirectory: options.workingDirectory ?? envConfig.workingDirectory});

    const args = 
    {
        ...envConfig,
        ...fileConfig,
    };

    if(options.branchName !== undefined && options.branchName !== null)
    {
        args.branchName = options.branchName;
    }

    if(options.buildNumber !== undefined && options.BuildNumber !== null)
    {
        args.buildNumber = options.buildNumber;
    }

    if(options.tagPrefix !== undefined && options.tagPrefix !== null)
    {
        args.tagPrefix = options.tagPrefix;
    }

    if(options.ignoreBranchPrefix !== undefined && options.ignoreBranchPrefix !== null)
    {
        args.ignoreBranchPrefix = options.ignoreBranchPrefix;
    }

    if(options.pre !== undefined && options.pre !== null)
    {
        args.pre = options.pre;
    }

    if(options.suffix !== undefined && options.suffix !== null)
    {
        args.suffix = options.suffix;
    }

    if(options.currentVersion !== undefined && options.currentVersion !== null)
    {
        args.currentVersion = options.currentVersion;
    }
    
    if(options.workingDirectory !== undefined && options.workingDirectory !== null)
    {
        args.workingDirectory = options.workingDirectory;
    }

    if(options.noIncrement !== undefined && options.noIncrement !== null)
    {
        args.noIncrement = options.noIncrement;
    }

    if(options.noStdOut !== undefined && options.noStdOut !== null)
    {
        args.noStdOut = options.noStdOut;
    }

    updateBuildNumber(args);

    const extractor = new VersionsExtractor(args);
    const result = await extractor.process();

    return result.version;
};
