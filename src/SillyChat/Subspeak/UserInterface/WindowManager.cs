using System.Numerics;

using CheapLoc;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace Subspeak
{
    /// <summary>
    /// Window manager to hold plugin windows and window system.
    /// </summary>
    public class WindowManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowManager"/> class.
        /// </summary>
        /// <param name="plugin">Subspeak plugin.</param>
        public WindowManager(ISubspeakPlugin plugin)
        {
            this.Plugin = plugin;

            // create windows
            this.ConfigWindow = new ConfigWindow(this.Plugin) { Size = new Vector2(500, 130) };
            this.HistoryWindow = new HistoryWindow(this.Plugin)
            {
                Size = new Vector2(200, 200),
                SizeCondition = ImGuiCond.FirstUseEver,
            };

            // setup events
            SubspeakPlugin.PluginInterface.UiBuilder.Draw += this.OnBuildUi;
            SubspeakPlugin.PluginInterface.UiBuilder.OpenConfigUi += this.OnOpenConfigUi;

            // setup window system
            this.WindowSystem = new WindowSystem("SubspeakWindowSystem");
            this.WindowSystem.AddWindow(this.ConfigWindow);
            this.WindowSystem.AddWindow(this.HistoryWindow);

            // setup ui commands
            SubspeakPlugin.CommandManager.AddHandler("/subspeakconfig", new CommandInfo(this.ToggleConfig)
            {
                HelpMessage = Loc.Localize("ConfigCommandHelp", "Show Subspeak config window."),
                ShowInHelp = true,
            });
        }

        /// <summary>
        /// Gets config window.
        /// </summary>
        public HistoryWindow HistoryWindow { get; }

        /// <summary>
        /// Gets config window.
        /// </summary>
        public ConfigWindow? ConfigWindow { get; }

        private WindowSystem WindowSystem { get; }

        private ISubspeakPlugin Plugin { get; }

        /// <summary>
        /// Dispose plugin windows.
        /// </summary>
        public void Dispose()
        {
            SubspeakPlugin.PluginInterface.UiBuilder.Draw -= this.OnBuildUi;
            SubspeakPlugin.PluginInterface.UiBuilder.OpenConfigUi -= this.OnOpenConfigUi;
            SubspeakPlugin.CommandManager.RemoveHandler("/subspeakconfig");
        }

        private void ToggleHistory(string command, string args)
        {
            this.HistoryWindow!.IsOpen ^= true;
        }

        private void ToggleConfig(string command, string args)
        {
            this.ConfigWindow!.IsOpen ^= true;
        }

        private void OnOpenConfigUi()
        {
            this.ConfigWindow!.IsOpen ^= true;
        }

        private void OnBuildUi()
        {
            this.WindowSystem.Draw();
        }
    }
}
