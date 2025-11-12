namespace RessurectIT.Git.Version.Misc
{
    /// <summary>
    /// Class storing app configuration
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets or sets branch name which can be used to override current branch
        /// </summary>
        public string? BranchName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets build number which can be used to override generated build number
        /// </summary>
        public long? BuildNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets prefix used for locating git tags
        /// </summary>
        public string? TagPrefix
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets prefix which is ignored while evaluating branch name
        /// </summary>
        public string? IgnoreBranchPrefix
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets indication whether is pre release version requested
        /// </summary>
        public bool? Pre
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets suffix name for prerelease version
        /// </summary>
        public string? Suffix
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets current version which can be incremented if matches generated version
        /// </summary>
        public string? CurrentVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets working directory for git
        /// </summary>
        public string? WorkingDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets path to ngv config json file
        /// </summary>
        public string? ConfigPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets indication whether no new version should be incremented/calculated.
        /// </summary>
        public bool? NoIncrement
        {
            get;
            set;
        }

        /// <summary>
        /// Semicolon separated paths to folders where can be node executable located
        /// </summary>
        public string NodeJsDirs
        {
            get;
            set;
        } = string.Empty;

        /// <summary>
        /// Path inside of <see cref="NodeJsDirs"/> to node executable
        /// </summary>
        public string NodeJsPath
        {
            get;
            set;
        } = string.Empty;
    }
}