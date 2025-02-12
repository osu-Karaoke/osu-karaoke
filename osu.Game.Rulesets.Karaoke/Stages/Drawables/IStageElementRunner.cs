// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

public interface IStageElementRunner
{
    /// <summary>
    /// Apply <see cref="IStageElement"/> to the stage.
    /// </summary>
    /// <param name="container"></param>
    void UpdateStageElements(Container container);
}
