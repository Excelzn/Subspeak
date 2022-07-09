using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Text;

namespace Subspeak
{
    public class SubspeakTranslator: BaseTranslator
    {
        public SubspeakTranslator(ISubspeakPlugin plugin) : base(plugin) { }

        public override string Translate(string input, XivChatType type)
        {
            var words = input.Split(' ');
            var output = new List<string>();
            var isInQuotes = false;
            foreach (var word in words)
            {
                if (!isInQuotes)
                {
                    isInQuotes = word.StartsWith("\"");
                }

                if (!isInQuotes)
                {
                    output.Add(word);
                }

                if (isInQuotes)
                {
                    if (Plugin.Configuration.BlacklistMode)
                    {
                        if (!Plugin.Whitelist.Contains(word, new StringComparer()))
                        {
                            output.Add(word);
                        }
                        else
                        {
                            output.Add(new(word.Select(x =>
                            {
                                if (char.IsSymbol(x) || char.IsPunctuation(x))
                                    return x;
                                var replacement = Plugin.Configuration.ReplacementCharacter.FirstOrDefault();
                                if (replacement == default)
                                    return '*';
                                return replacement;
                            }).ToArray()));
                        }

                        isInQuotes = !word.EndsWith("\"");
                    }
                    else
                    {
                        if (Plugin.Whitelist.Contains(word, new StringComparer()))
                        {
                            output.Add(word);
                        }
                        else
                        {
                            output.Add(new(word.Select(x =>
                            {
                                if (char.IsSymbol(x) || char.IsPunctuation(x))
                                    return x;
                                var replacement = Plugin.Configuration.ReplacementCharacter.FirstOrDefault();
                                if (replacement == default)
                                    return '*';
                                return replacement;
                            }).ToArray()));
                        }

                        isInQuotes = !word.EndsWith("\"");
                    }
                }
            }

            var final = string.Join(' ', output);
            if (type == XivChatType.CustomEmote)
            {
                final = $" {final}";
            }
            return final;
        }
    }

    public class StringComparer: IEqualityComparer<string>
    {
        public bool Equals(string? x, string? y)
        {
            if (string.IsNullOrWhiteSpace(x) || string.IsNullOrWhiteSpace(y))
            {
                return false;
            }
            
            var trimmedX = new string(x.Where(c => !char.IsPunctuation(c) && !char.IsSymbol(c)).ToArray());
            var trimmedY = new string(y.Where(c => !char.IsPunctuation(c) && !char.IsSymbol(c)).ToArray());

            return trimmedX.Equals(trimmedY, StringComparison.OrdinalIgnoreCase);
        }
        

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
