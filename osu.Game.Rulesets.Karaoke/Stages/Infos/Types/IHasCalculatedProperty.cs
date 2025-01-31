// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Types;

public interface IHasCalculatedProperty
{
    /// <summary>
    /// If the calculated property is not updated, then re-calculate the property inside the stage info in the <see cref="KaraokeBeatmapProcessor"/>
    /// </summary>
    /// <param name="beatmap"></param>
    void ValidateCalculatedProperty(IBeatmap beatmap);
}
