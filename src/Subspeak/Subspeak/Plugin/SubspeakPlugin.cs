using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using CheapLoc;
using Dalamud.Configuration;
using Dalamud.DrunkenToad;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.IoC;
using Dalamud.Plugin;
using XivCommon;
using XivCommon.Functions;

namespace Subspeak
{
    /// <summary>
    /// Subspeak Plugin.
    /// </summary>
    public class SubspeakPlugin : ISubspeakPlugin, IDalamudPlugin
    {
        private XivCommonBase xivCommon = null!;
        private Localization localization = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubspeakPlugin"/> class.
        /// </summary>
        public SubspeakPlugin()
        {
            Task.Run(() =>
            {
                // setup common libs
                this.localization = new Localization(PluginInterface, CommandManager);
                const Hooks hooks = Hooks.Talk | Hooks.BattleTalk | Hooks.ChatBubbles;
                this.xivCommon = new XivCommonBase(hooks);

                // load config
                try
                {
                    this.Configuration = PluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
                }
                catch (Exception ex)
                {
                    Logger.LogError("Failed to load config so creating new one.", ex);
                    this.Configuration = new PluginConfig();
                    this.SaveConfig();
                }
                
                if (File.Exists(Configuration.WhitelistLocation))
                {
                    var whitelist = File.ReadAllLines(Configuration.WhitelistLocation);
                    Whitelist.AddRange(whitelist);
                }
                else
                {
                    Logger.LogError("Unable to load whitelist.");
                }
                // setup translator
                this.TranslationService = new TranslationService(this);
                this.HistoryService = new HistoryService(this);
                Chat.ChatMessage += this.ChatMessage;

                // setup ui
                this.WindowManager = new WindowManager(this);

                // special handling for fresh install
                this.HandleFreshInstall();

                // toggle
                CommandManager.AddHandler("/subspeak", new CommandInfo(this.TogglePlugin)
                {
                    HelpMessage = Loc.Localize("ToggleCommandHelp", "Toggle Subspeak."),
                    ShowInHelp = true,
                });
            });
        }

        /// <summary>
        /// Gets pluginInterface.
        /// </summary>
        [PluginService]
        [RequiredVersion("1.0")]
        public static DalamudPluginInterface PluginInterface { get; private set; } = null!;

        /// <summary>
        /// Gets chat gui.
        /// </summary>
        [PluginService]
        [RequiredVersion("1.0")]
        public static ChatGui Chat { get; private set; } = null!;

        /// <summary>
        /// Gets command manager.
        /// </summary>
        [PluginService]
        [RequiredVersion("1.0")]
        public static CommandManager CommandManager { get; private set; } = null!;

        /// <inheritdoc />
        public string Name => "Subspeak";

        /// <summary>
        /// Gets translationService.
        /// </summary>
        public TranslationService TranslationService { get; private set; } = null!;

        /// <summary>
        /// Gets historyService.
        /// </summary>
        public HistoryService HistoryService { get; private set; } = null!;

        /// <summary>
        /// Gets or sets plugin name.
        /// </summary>
        public string PluginName { get; set; } = null!;

        /// <inheritdoc/>
        public SubspeakConfig Configuration { get; set; } = null!;

        public List<string> Whitelist { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets window manager.
        /// </summary>
        public WindowManager WindowManager { get; set; } = null!;

        /// <inheritdoc/>
        public void SaveConfig()
        {
            PluginInterface.SavePluginConfig((IPluginConfiguration)this.Configuration);
        }

        public void ReloadWhitelist()
        {
            if (File.Exists(Configuration.WhitelistLocation))
            {
                var whitelist = File.ReadAllLines(Configuration.WhitelistLocation);
                Whitelist.AddRange(whitelist);
            }
            else
            {
                Logger.LogError("Unable to load whitelist.");
            }
        }

        /// <summary>
        /// Dispose Subspeak plugin.
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.HistoryService.Dispose();
                this.WindowManager.Dispose();

                // plugin service
                CommandManager.RemoveHandler("/subspeak");
                Chat.ChatMessage -= this.ChatMessage;
                
                this.xivCommon.Dispose();

                // localization
                this.localization.Dispose();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to dispose plugin properly.");
            }
        }

        private void TogglePlugin(string command, string args)
        {
            this.Configuration.Enabled ^= true;
            this.SaveConfig();
        }

        private void ChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            if (!this.Configuration.Enabled)
            {
                return;
            }

            if (isHandled)
            {
                return;
            }

            var chatType = (uint)type & ~(~0 << 7);
            if (!Enum.IsDefined(typeof(SupportedChatType), chatType))
            {
                return;
            }

            this.Translate(message, type);
        }


        private void Translate(SeString message, XivChatType type)
        {
            try
            {
                foreach (var payload in message.Payloads)
                {
                    if (payload is TextPayload textPayload)
                    {
                        var input = textPayload.Text;
                        if (string.IsNullOrEmpty(input) || input.Contains('\uE0BB'))
                        {
                            continue;
                        }

                        var output = this.TranslationService.Translate(input, type);
                        if (!input.Equals(output))
                        {
                            textPayload.Text = output;
                            Logger.LogDebug($"{input}|{output}");
                            this.HistoryService.AddTranslation(new Translation(input, output));
                        }
                    }
                }
            }
            catch
            {
                Logger.LogDebug($"Failed to process message: {message}.");
            }
        }

        private void HandleFreshInstall()
        {
            if (!this.Configuration.FreshInstall)
            {
                return;
            }

            Chat.PluginPrintNotice(Loc.Localize("InstallThankYou", "Thank you for installing Subspeak!"));
            Thread.Sleep(500);
            Chat.PluginPrintNotice(
                Loc.Localize("Instructions", "You can use /subspeak to toggle the plugin, /subspeakconfig to view settings, and /subspeakhistory to see previous translations."));
            this.Configuration.FreshInstall = false;
            this.SaveConfig();
            this.WindowManager.ConfigWindow!.IsOpen = true;
        }
    }
}
