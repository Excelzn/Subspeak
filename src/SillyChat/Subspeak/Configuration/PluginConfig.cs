using System;

using Dalamud.Configuration;

namespace Subspeak
{
    /// <summary>
    /// Plugin configuration class used for dalamud.
    /// </summary>
    [Serializable]
    public class PluginConfig : SubspeakConfig, IPluginConfiguration
    {
        /// <inheritdoc/>
        public int Version { get; set; } = 0;
    }
}
