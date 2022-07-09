namespace Subspeak
{
    /// <summary>
    /// Subspeak plugin configuration.
    /// </summary>
    public abstract class SubspeakConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether fresh install.
        /// </summary>
        public bool FreshInstall { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether plugin is enabled.
        /// </summary>
        public bool Enabled { get; set; } = true;

        public string WhitelistLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets timer to process intervals for history view.
        /// </summary>
        public int ProcessTranslationInterval { get; set; } = 300000;

        /// <summary>
        /// Gets or sets max number of translations to keep in history.
        /// </summary>
        public int TranslationHistoryMax { get; set; } = 30;

        public string ReplacementCharacter { get; set; } = "*";
        public bool BlacklistMode { get; set; } = false;
    }
}
