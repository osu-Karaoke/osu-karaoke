// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Objects.Stages;

public abstract class StageEffectApplier<TStageDefinition, TDrawableHitObject> : IStageEffectApplier
    where TStageDefinition : StageDefinition
    where TDrawableHitObject : DrawableHitObject
{
    private readonly IEnumerable<StageElement> elements;

    public readonly TStageDefinition Definition;

    protected StageEffectApplier(IEnumerable<StageElement> elements, TStageDefinition definition)
    {
        this.elements = elements;
        Definition = definition;
    }

    /// <summary>
    /// Apply (generally fade-in) transforms leading into the <see cref="KaraokeHitObject"/> start time.
    /// </summary>
    /// <param name="drawableHitObject"></param>
    public void UpdateInitialTransforms(DrawableHitObject drawableHitObject)
    {
        if (drawableHitObject is not TDrawableHitObject drawable)
            throw new InvalidCastException();

        drawable.ClearTransforms();

        var transform = drawable.FadeOut();

        foreach (var element in elements)
        {
            // todo: not very sure how to combine the transform.
            UpdateInitialTransforms(transform, element);
        }
    }

    /// <summary>
    /// Apply passive transforms at the <see cref="KaraokeHitObject"/>'s StartTime.
    /// </summary>
    /// <param name="drawableHitObject"></param>
    public void UpdateStartTimeStateTransforms(DrawableHitObject drawableHitObject)
    {
        if (drawableHitObject is not TDrawableHitObject drawable)
            throw new InvalidCastException();

        drawable.ClearTransforms();

        var transform = drawable.Delay(0);

        foreach (var element in elements)
        {
            // todo: not very sure how to combine the transform.
            UpdateStartTimeStateTransforms(transform, element);
        }

        drawable.FadeIn();
    }

    /// <summary>
    /// Apply transforms based on the current <see cref="ArmedState"/>.
    /// This call is offset by (HitObject.EndTime + Result.Offset), equivalent to when the user hit the object.
    /// </summary>
    /// <param name="drawableHitObject"></param>
    /// <param name="state"></param>
    public void UpdateHitStateTransforms(DrawableHitObject drawableHitObject, ArmedState state)
    {
        if (drawableHitObject is not TDrawableHitObject drawable)
            throw new InvalidCastException();

        drawable.ClearTransforms();

        var transform = drawable.Delay(0);

        foreach (var element in elements)
        {
            // todo: not very sure how to combine the transform.
            UpdateHitStateTransforms(transform, state, element);
        }

        drawable.FadeOut();
    }

    protected abstract void UpdateInitialTransforms(TransformSequence<TDrawableHitObject> transformSequence, StageElement element);

    protected abstract void UpdateStartTimeStateTransforms(TransformSequence<TDrawableHitObject> transformSequence, StageElement element);

    protected abstract void UpdateHitStateTransforms(TransformSequence<TDrawableHitObject> transformSequence, ArmedState state, StageElement element);
}
