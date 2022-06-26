using System.Collections.Generic;

namespace Subspeak
{
    /// <summary>
    /// Subspeak plugin interface.
    /// </summary>
    public interface ISubspeakPlugin
    {
        /// <summary>
        /// Gets plugin configuration.
        /// </summary>
        SubspeakConfig Configuration { get; }

        /// <summary>
        /// Gets translation service.
        /// </summary>
        TranslationService TranslationService { get; }

        /// <summary>
        /// Gets history service.
        /// </summary>
        HistoryService HistoryService { get; }


        public List<string> Whitelist { get; set; }

        /// <summary>
        /// Save plugin configuration.
        /// </summary>
        void SaveConfig();

        void ReloadWhitelist();
    }
}
