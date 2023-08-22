﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

public partial class DeleteLyricDialog : PopupDialog
{
    public DeleteLyricDialog(Action<bool> okAction)
    {
        Icon = FontAwesome.Solid.Globe;
        HeaderText = "Confirm deletion of";
        BodyText = "lyric";
        Buttons = new PopupDialogButton[]
        {
            new PopupDialogOkButton
            {
                Text = "Yes. Go for it.",
                Action = () => okAction(true),
            },
            new PopupDialogCancelButton
            {
                Text = "No! Abort mission!",
                Action = () => okAction(false),
            },
        };
    }
}
