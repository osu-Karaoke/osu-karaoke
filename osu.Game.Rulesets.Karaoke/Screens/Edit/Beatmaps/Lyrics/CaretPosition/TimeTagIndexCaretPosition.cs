// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

public readonly struct TimeTagIndexCaretPosition : ICharIndexCaretPosition
{
    public TimeTagIndexCaretPosition(Lyric lyric, int charIndex)
    {
        Lyric = lyric;
        CharIndex = charIndex;
    }

    public Lyric Lyric { get; }

    public int CharIndex { get; }
}
