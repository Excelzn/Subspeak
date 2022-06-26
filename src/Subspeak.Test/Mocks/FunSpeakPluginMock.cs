using System.Collections.Generic;

namespace Subspeak.Test
{
    /// <inheritdoc />
    public class SubspeakPluginMock : ISubspeakPlugin
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        public SubspeakPluginMock()
        {
            PluginName = "Subspeak";
            Configuration = new SubspeakConfigMock();
            TranslationService = new TranslationService(this);
            HistoryService = new HistoryService(this);
        }

        /// <inheritdoc />
        public string PluginName { get; set; }

        /// <inheritdoc />
        public SubspeakConfig Configuration { get; }

        /// <inheritdoc />
        public TranslationService TranslationService { get; }

        /// <inheritdoc />
        public HistoryService HistoryService { get; }

        public List<string> Whitelist { get; set; }

        /// <inheritdoc />
        public void SaveConfig()
        {
            throw new System.NotImplementedException();
        }

        public void ReloadWhitelist()
        {
            throw new System.NotImplementedException();
        }
    }
}
