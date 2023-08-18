// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.EditLyric;

public partial class EditLyricNavigation : TopNavigation<EditLyricStepScreen>
{
    private const string typing_mode = "TYPING_MODE";
    private const string split_mode = "SPLIT_MODE";

    private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

    public EditLyricNavigation(EditLyricStepScreen screen)
        : base(screen)
    {
        bindableMode.BindValueChanged(_ =>
        {
            // should update the display text in navigation bar if mode change.
            TriggerStateChange();
        });
    }

    [BackgroundDependencyLoader]
    private void load(ILyricEditorState state)
    {
        bindableMode.BindTo(state.BindableMode);
    }

    protected override NavigationTextContainer CreateTextContainer()
        => new EditLyricTextFlowContainer(Screen);

    protected override NavigationState GetState(Lyric[] lyrics)
        => NavigationState.Working;

    protected override LocalisableString GetNavigationText(NavigationState value)
    {
        switch (value)
        {
            case NavigationState.Initial:
                return $"Does something looks weird? Try switching [{typing_mode}] or [{split_mode}] to edit your lyric.";

            case NavigationState.Working:
            case NavigationState.Done:
                var step = Screen.GetLyricEditorModeState<TextingEditStep>();

                return step switch
                {
                    TextingEditStep.Typing => $"Cool! Try switching to [{split_mode}] if you want to cut or combine lyric.",
                    TextingEditStep.Split => $"Cool! Try switching to [{typing_mode}] if you want to edit lyric.",
                    TextingEditStep.Verify => $"Cool! Try switching to [{split_mode}] or [{typing_mode}] if you want to fix the issue.",
                    _ => throw new InvalidEnumArgumentException(nameof(step)),
                };

            case NavigationState.Error:
                return "Oops, seems cause some error in here.";

            default:
                throw new InvalidEnumArgumentException(nameof(value));
        }
    }

    protected override bool AbleToNextStep(NavigationState value)
        => true;

    private partial class EditLyricTextFlowContainer : NavigationTextContainer
    {
        public EditLyricTextFlowContainer(EditLyricStepScreen screen)
        {
            AddLinkFactory(typing_mode, "typing mode", () => screen.SwitchToEditModeState(TextingEditStep.Typing));
            AddLinkFactory(split_mode, "split mode", () => screen.SwitchToEditModeState(TextingEditStep.Split));
        }
    }
}
