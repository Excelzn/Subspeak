using System;
using System.Collections.Generic;
using System.Linq;

using Dalamud.DrunkenToad;
using Dalamud.Game.Text;

namespace Subspeak
{
    /// <summary>
    /// Orchestrate translation process.
    /// </summary>
    public class TranslationService
    {
        private BaseTranslator translator = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationService"/> class.
        /// </summary>
        /// <param name="plugin">Subspeak plugin.</param>
        public TranslationService(ISubspeakPlugin plugin)
        {
            this.translator = new SubspeakTranslator(plugin);
        }

        /// <summary>
        /// Translate.
        /// </summary>
        /// <param name="input">untranslated to text.</param>
        /// <param name="type">Chat type. Needed for custom emote support.</param>
        /// <returns>translated text.</returns>
        public string Translate(string input, XivChatType type)
        {
            return this.translator.Translate(input, type);
        }
    }
}
