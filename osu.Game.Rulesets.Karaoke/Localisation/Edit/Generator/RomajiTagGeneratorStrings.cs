﻿using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Localisation.Edit.Generator
{
    public static class RomajiTagGeneratorStrings
    {
        private const string prefix = @"osu.Game.Rulesets.Karaoke.Localisation.RomajiTagGenerator";

        /// <summary>
        /// "Lyric should not be empty."
        /// </summary>
        public static LocalisableString LyricShouldNotBeEmpty => new TranslatableString(getKey(@"lyric_should_not_be_empty"), @"Lyric should not be empty.");

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}
