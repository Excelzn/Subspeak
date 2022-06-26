using System.Linq;

using CheapLoc;
using Dalamud.Interface.Colors;
using ImGuiNET;

namespace Subspeak
{
    /// <summary>
    /// History window for the plugin.
    /// </summary>
    public class HistoryWindow : PluginWindow
    {
        private readonly ISubspeakPlugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryWindow"/> class.
        /// </summary>
        /// <param name="plugin">Subspeak plugin.</param>
        public HistoryWindow(ISubspeakPlugin plugin)
            : base(plugin, Loc.Localize("HistoryTitle", "Subspeak History"))
        {
            this.plugin = plugin;
        }

        /// <inheritdoc/>
        public override void Draw()
        {
            try
            {
                if (this.plugin.HistoryService.IsProcessing) return;
                var translations = this.plugin.HistoryService.Translations.ToList();
                if (translations.Count > 0)
                {
                    ImGui.Columns(2);
                    ImGui.TextColored(ImGuiColors.HealerGreen, Loc.Localize("SourceMessage", "Source"));
                    ImGui.NextColumn();
                    ImGui.TextColored(ImGuiColors.DPSRed, Loc.Localize("TranslationMessage", "Translation"));
                    ImGui.NextColumn();
                    ImGui.Separator();
                    foreach (var translation in translations)
                    {
                        ImGui.TextWrapped(translation.Input);
                        ImGui.NextColumn();
                        ImGui.TextWrapped(translation.Output);
                        ImGui.NextColumn();
                        ImGui.Separator();
                    }
                }
                else
                {
                    ImGui.Text(Loc.Localize("NoTranslations", "Nothing has been translated yet."));
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}
