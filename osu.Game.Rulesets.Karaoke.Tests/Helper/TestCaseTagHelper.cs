﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Helper
{
    public static class TestCaseTagHelper
    {
        /// <summary>
        /// Process test case ruby string format into <see cref="RubyTag"/>
        /// </summary>
        /// <example>
        /// [0,3]:ruby
        /// </example>
        /// <param name="str"></param>
        /// <returns><see cref="RubyTag"/></returns>
        public static RubyTag ParseRubyTag(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new RubyTag();

            var regex = new Regex("(?<start>[-0-9]+),(?<end>[-0-9]+)]:(?<ruby>.*$)");
            var result = regex.Match(str);
            if (!result.Success)
                throw new ArgumentException(nameof(str));

            var startIndex = int.Parse(result.Groups["start"]?.Value);
            var endIndex = int.Parse(result.Groups["end"]?.Value);
            var text = result.Groups["ruby"]?.Value;

            return new RubyTag
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                Text = text
            };
        }

        /// <summary>
        /// Process test case romaji string format into <see cref="RomajiTag"/>
        /// </summary>
        /// <example>
        /// [0,3]:romaji
        /// </example>
        /// <param name="str"></param>
        /// <returns><see cref="RomajiTag"/></returns>
        public static RomajiTag ParseRomajiTag(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new RomajiTag();

            var regex = new Regex("(?<start>[-0-9]+),(?<end>[-0-9]+)]:(?<romaji>.*$)");
            var result = regex.Match(str);
            if (!result.Success)
                throw new ArgumentException(nameof(str));

            var startIndex = int.Parse(result.Groups["start"]?.Value);
            var endIndex = int.Parse(result.Groups["end"]?.Value);
            var text = result.Groups["romaji"]?.Value;

            return new RomajiTag
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                Text = text
            };
        }

        /// <summary>
        /// Process test case time tag string format into <see cref="TimeTag"/>
        /// </summary>
        /// <example>
        /// [0,start]:1000
        /// </example>
        /// <param name="str"></param>
        /// <returns><see cref="TimeTag"/></returns>
        public static TimeTag ParseTimeTag(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new TimeTag(new TextIndex());

            var regex = new Regex("(?<index>[-0-9]+),(?<state>start|end)]:(?<time>[-0-9]+|s*|)");
            var result = regex.Match(str);
            if (!result.Success)
                throw new ArgumentException(nameof(str));

            var index = int.Parse(result.Groups["index"]?.Value);
            var state = result.Groups["state"]?.Value == "start" ? TextIndex.IndexState.Start : TextIndex.IndexState.End;
            var timeStr = result.Groups["time"]?.Value;
            var time = timeStr == "" ? default(int?) : int.Parse(timeStr);

            return new TimeTag(new TextIndex(index, state), time);
        }

        /// <summary>
        /// Process test case lyric string format into <see cref="Lyric"/>
        /// </summary>
        /// <example>
        /// [1000,3000]:karaoke
        /// </example>
        /// <param name="str"></param>
        /// <returns><see cref="TimeTag"/></returns>
        public static Lyric ParseLyric(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new Lyric();

            var regex = new Regex("(?<startTime>[-0-9]+),(?<endTime>[-0-9]+)]:(?<lyric>.*$)");
            var result = regex.Match(str);
            if (!result.Success)
                throw new ArgumentException(nameof(str));

            var startTime = double.Parse(result.Groups["startTime"]?.Value);
            var endTime = double.Parse(result.Groups["endTime"]?.Value);
            var text = result.Groups["lyric"]?.Value;

            return new Lyric
            {
                StartTime = startTime,
                Duration = endTime - startTime,
                Text = text,
                TimeTags = new[]
                {
                    new TimeTag(new TextIndex(0), startTime),
                    new TimeTag(new TextIndex((text?.Length ?? 0) - 1, TextIndex.IndexState.End), endTime)
                }
            };
        }

        public static RubyTag[] ParseRubyTags(string[] strings)
            => strings?.Select(ParseRubyTag).ToArray();

        public static RomajiTag[] ParseRomajiTags(string[] strings)
            => strings?.Select(ParseRomajiTag).ToArray();

        public static TimeTag[] ParseTimeTags(string[] strings)
            => strings?.Select(ParseTimeTag).ToArray();

        public static Lyric[] ParseLyrics(string[] strings)
            => strings?.Select(ParseLyric).ToArray();
    }
}
