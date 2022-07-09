using System;
using CheapLoc;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using ImGuiNET;

namespace Subspeak
{
    /// <summary>
    /// Config window for the plugin.
    /// </summary>
    public class ConfigWindow : PluginWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigWindow"/> class.
        /// </summary>
        /// <param name="plugin">Subspeak plugin.</param>
        public ConfigWindow(ISubspeakPlugin plugin)
            : base(plugin, Loc.Localize("SettingsTitle", "Subspeak Settings"))
        {
        }

        /// <inheritdoc/>
        public override void Draw()
        {
            // plugin enabled
            var enabled = this.Plugin.Configuration.Enabled;
            if (ImGui.Checkbox(
                Loc.Localize("PluginEnabled", "Enabled") + "###Subspeak_PluginEnabled_Checkbox",
                ref enabled))
            {
                this.Plugin.Configuration.Enabled = enabled;
                this.Plugin.SaveConfig();
            }

            ImGuiComponents.HelpMarker(Loc.Localize("PluginEnabled_HelpMarker", "enable or disable the plugin"));
            ImGui.Spacing();
            // plugin enabled
            var blacklistMode = this.Plugin.Configuration.BlacklistMode;
            if (ImGui.Checkbox(
                    "Blacklist Mode###Subspeak_Blacklist_Checkbox",
                    ref blacklistMode))
            {
                this.Plugin.Configuration.BlacklistMode = blacklistMode;
                this.Plugin.SaveConfig();
            }

            ImGuiComponents.HelpMarker(Loc.Localize("PluginEnabled_HelpMarker", "enable or disable the plugin"));
            ImGui.Spacing();

            var whitelistLocation = Plugin.Configuration.WhitelistLocation;
            if (
                ImGui.InputText("Whitelist Location###Subspeak_Whitelist_Location", ref whitelistLocation, 32))
            {
                Plugin.Configuration.WhitelistLocation = whitelistLocation;
                Plugin.SaveConfig();
                Plugin.ReloadWhitelist();
            }

            ImGuiComponents.HelpMarker("Sets the location of the whitelist on your PC. Whitelist must be a txt file with one word per line.");
            ImGui.Spacing();

            var replacementCharacter = Plugin.Configuration.ReplacementCharacter;
            if (
                ImGui.InputText("Replacement Character###Subspeak_Replacement_Character", ref replacementCharacter, 5))
            {
                Plugin.Configuration.ReplacementCharacter = replacementCharacter;
                Plugin.SaveConfig();
            }
            ImGuiComponents.HelpMarker("Letters in words that aren't in the whitelist will be replaced with this character.");
            ImGui.Spacing();
            if (ImGui.Button("Reload Whitelist", ImGuiHelpers.ScaledVector2(-1, 25)))
                Plugin.ReloadWhitelist();
        }
    }
}
