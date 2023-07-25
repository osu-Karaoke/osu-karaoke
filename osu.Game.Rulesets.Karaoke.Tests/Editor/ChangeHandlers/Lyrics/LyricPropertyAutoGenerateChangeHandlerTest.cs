// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Tests.Extensions;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

/// <summary>
/// This test is focus on make sure that:
/// If the <see cref="Lyric.ReferenceLyric"/> in the <see cref="Lyric"/> is not empty.
/// <see cref="ILyricPropertyAutoGenerateChangeHandler"/> should be able to change the property.
/// </summary>
/// <typeparam name="TChangeHandler"></typeparam>
[TestFixture(typeof(LyricReferenceChangeHandler))]
[TestFixture(typeof(LyricLanguageChangeHandler))]
[TestFixture(typeof(LyricRubyTagsChangeHandler))]
[TestFixture(typeof(LyricRomajiTagsChangeHandler))]
[TestFixture(typeof(LyricTimeTagsChangeHandler))]
[TestFixture(typeof(LyricNotesChangeHandler))]
public partial class LyricPropertyAutoGenerateChangeHandlerTest<TChangeHandler> : LyricPropertyChangeHandlerTest<TChangeHandler>
    where TChangeHandler : LyricPropertyChangeHandler, ILyricPropertyAutoGenerateChangeHandler, new()
{
    protected override bool IncludeAutoGenerator => true;

    [Test]
    [Description("Should be able to generate the property if the lyric is not reference to other lyric.")]
    public void ChangeWithNormalLyric()
    {
        // for detect reference lyric.
        if (isLyricReferenceChangeHandler())
        {
            PrepareHitObject(() => new Lyric
            {
                Text = "karaoke",
            }, false);
        }

        PrepareHitObject(() => new Lyric
        {
            Text = "karaoke",
            Language = new CultureInfo(17), // for auto-generate ruby and romaji.
            TimeTags = new[] // for auto-generate notes.
            {
                new TimeTag(new TextIndex(0), 0),
                new TimeTag(new TextIndex(1), 1000),
                new TimeTag(new TextIndex(2), 2000),
                new TimeTag(new TextIndex(3), 3000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
            },
        });

        TriggerHandlerChanged(c =>
        {
            Assert.IsTrue(c.CanGenerate());
        });

        TriggerHandlerChanged(c =>
        {
            Assert.IsEmpty(c.GetGeneratorNotSupportedLyrics());
        });

        TriggerHandlerChanged(c =>
        {
            Assert.DoesNotThrow(c.AutoGenerate);
        });
    }

    [Test]
    [Description("Should not be able to generate the property if the lyric is missing detectable property.")]
    public void ChangeWithMissingPropertyLyric()
    {
        PrepareHitObject(() => new Lyric());

        TriggerHandlerChanged(c =>
        {
            Assert.IsFalse(c.CanGenerate());
        });

        TriggerHandlerChanged(c =>
        {
            Assert.IsNotEmpty(c.GetGeneratorNotSupportedLyrics());
        });

        TriggerHandlerChanged(c =>
        {
            var exception = Assert.Catch(c.AutoGenerate);
            Assert.Contains(exception?.GetType(), new[] { typeof(GeneratorNotSupportedException), typeof(DetectorNotSupportedException) });
        });
    }

    [Test]
    [Description("Should not be able to generate the property if the lyric is reference to other lyric.")]
    public void CheckWithReferencedLyric()
    {
        if (isLyricReferenceChangeHandler())
            return;

        PrepareHitObject(() => new Lyric
        {
            Text = "karaoke",
            Language = new CultureInfo(17), // for auto-generate ruby and romaji.
            TimeTags = new[] // for auto-generate notes.
            {
                new TimeTag(new TextIndex(0), 0),
                new TimeTag(new TextIndex(1), 1000),
                new TimeTag(new TextIndex(2), 2000),
                new TimeTag(new TextIndex(3), 3000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
            },
            // has reference lyric.
            ReferenceLyricId = TestCaseElementIdHelper.CreateElementIdByNumber(1),
            ReferenceLyric = new Lyric().ChangeId(1),
            ReferenceLyricConfig = new SyncLyricConfig(),
        });

        TriggerHandlerChanged(c =>
        {
            Assert.IsFalse(c.CanGenerate());
        });

        TriggerHandlerChanged(c =>
        {
            Assert.IsNotEmpty(c.GetGeneratorNotSupportedLyrics());
        });

        TriggerHandlerChangedWithChangeForbiddenException(c => c.AutoGenerate());
    }

    private bool isLyricReferenceChangeHandler()
        => typeof(TChangeHandler) == typeof(LyricReferenceChangeHandler);
}
