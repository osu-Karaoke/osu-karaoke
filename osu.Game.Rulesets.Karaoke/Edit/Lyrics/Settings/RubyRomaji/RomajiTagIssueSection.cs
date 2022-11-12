﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.RubyRomaji
{
    public class RomajiTagIssueSection : TextTagIssueSection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditRomaji;

        protected override IssueTable CreateIssueTable() => new RomajiTagIssueTable();

        private class RomajiTagIssueTable : TextTagIssueTable<RomajiTag>
        {
            protected override Tuple<Lyric, RomajiTag> GetInvalidByIssue(Issue issue)
            {
                if (issue is not RomajiTagIssue romajiTagIssue)
                    throw new InvalidCastException();

                var lyric = issue.HitObjects.OfType<Lyric>().Single();
                var textTag = romajiTagIssue.RomajiTag;

                return new Tuple<Lyric, RomajiTag>(lyric, textTag);
            }
        }
    }
}
