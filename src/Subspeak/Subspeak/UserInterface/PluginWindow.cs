using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace Subspeak
{
    /// <summary>
    /// Plugin window which extends window with Subspeak plugin.
    /// </summary>
    public abstract class PluginWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginWindow"/> class.
        /// </summary>
        /// <param name="plugin">Subspeak plugin.</param>
        /// <param name="windowName">Name of the window.</param>
        /// <param name="flags">ImGui flags.</param>
        protected PluginWindow(ISubspeakPlugin plugin, string windowName, ImGuiWindowFlags flags = ImGuiWindowFlags.None)
            : base(windowName, flags)
        {
            this.Plugin = plugin;
        }

        /// <summary>
        /// Gets Subspeak plugin for window.
        /// </summary>
        protected ISubspeakPlugin Plugin { get; }

        /// <inheritdoc/>
        public override void Draw()
        {
        }
    }
}
