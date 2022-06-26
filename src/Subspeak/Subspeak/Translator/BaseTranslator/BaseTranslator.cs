using Dalamud.Game.Text;

namespace Subspeak
{
    /// <summary>
    /// Translator.
    /// </summary>
    public abstract class BaseTranslator
    {
        /// <summary>
        /// Subspeak plugin.
        /// </summary>
        protected readonly ISubspeakPlugin Plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTranslator"/> class.
        /// </summary>
        /// <param name="plugin">Subspeak plugin.</param>
        protected BaseTranslator(ISubspeakPlugin plugin)
        {
            this.Plugin = plugin;
        }
        
        /// <summary>
        /// Translate input string.
        /// </summary>
        /// <param name="input">text to translate.</param>
        /// <param name="chatType">Chat channel</param>
        /// <returns>translated text.</returns>
        public abstract string Translate(string input, XivChatType chatType);
    }
}
