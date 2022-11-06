﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.RubyRomaji
{
    public abstract class TextTagEditModeSection<TEditModeState, TEditMode> : EditModeSection<TEditModeState, TEditMode>
        where TEditModeState : IHasEditModeState<TEditMode>
        where TEditMode : Enum
    {
        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Pink;
    }
}
