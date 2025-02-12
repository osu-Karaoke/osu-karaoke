// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Types;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;

public class PreviewStageInfo : StageInfo, IHasCalculatedProperty
{
    #region Category

    /// <summary>
    /// The definition for the <see cref="Lyric"/>.
    /// Like how many lyrics can in the playfield at the same time.
    /// </summary>
    public PreviewStageDefinition StageDefinition { get; set; } = new();

    /// <summary>
    /// Category to save the <see cref="Lyric"/>'s and <see cref="Note"/>'s style.
    /// This property will not be saved because it's real-time calculated.
    /// </summary>
    [JsonIgnore]
    private PreviewStyleCategory styleCategory { get; set; } = new();

    /// <summary>
    /// Category to save the <see cref="Lyric"/>'s layout.
    /// This property will not be saved because it's real-time calculated.
    /// </summary>
    [JsonIgnore]
    private PreviewLyricLayoutCategory layoutCategory { get; set; } = new();

    #endregion

    #region Validation

    /// <summary>
    /// If the calculated property is not updated, then re-calculate the property inside the stage info in the <see cref="KaraokeBeatmapProcessor"/>
    /// </summary>
    /// <param name="beatmap"></param>
    public void ValidateCalculatedProperty(IBeatmap beatmap)
    {
        var calculator = new PreviewStageTimingCalculator(beatmap, StageDefinition);

        // also, clear all mapping in the layout and re-create one.
        layoutCategory.ClearElements();

        // Note: only deal with those lyrics has time.
        var matchedLyrics = beatmap.HitObjects.OfType<Lyric>().Where(x => x.TimeValid).OrderBy(x => x.StartTime).ToArray();

        foreach (var lyric in matchedLyrics)
        {
            var element = layoutCategory.AddElement(x =>
            {
                x.Name = $"Auto-generated layout with lyric {lyric.ID}";
                x.StartTime = calculator.CalculateStartTime(lyric);
                x.EndTime = calculator.CalculateEndTime(lyric);
                x.Timings = calculator.CalculateTimings(lyric);
            });
            layoutCategory.AddToMapping(element, lyric);
        }
    }

    #endregion

    #region Stage element

    protected override IEnumerable<StageElement> GetLyricStageElements(Lyric lyric)
    {
        yield return styleCategory.GetElementByItem(lyric);
        yield return layoutCategory.GetElementByItem(lyric);
    }

    protected override IEnumerable<StageElement> GetNoteStageElements(Note note)
    {
        // todo: should check the real-time mapping result.
        yield return styleCategory.GetElementByItem(note.ReferenceLyric!);
    }

    #endregion

    #region Provider

    public override IPlayfieldCommandProvider CreatePlayfieldCommandProvider(bool displayNotePlayfield)
        => new PreviewPlayfieldCommandProvider(this, displayNotePlayfield);

    public override IStageElementProvider? CreateStageElementProvider(bool displayNotePlayfield)
        => new PreviewElementProvider(this, displayNotePlayfield);

    public override IHitObjectCommandProvider? CreateHitObjectCommandProvider<TObject>() =>
        typeof(TObject) switch
        {
            Type type when type == typeof(Lyric) => new PreviewLyricCommandProvider(this),
            Type type when type == typeof(Note) => null,
            _ => null
        };

    #endregion
}
