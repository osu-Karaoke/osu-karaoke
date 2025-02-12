// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods;

public partial class TestSceneKaraokeModLyricConfiguration : KaraokeModTestScene
{
    [Test]
    public void TestAllPanelExist() => CreateModTest(new ModTestData
    {
        Mod = new KaraokeModLyricConfiguration(),
        Autoplay = false,
        CreateBeatmap = () => new TestKaraokeBeatmap(new RulesetInfo()),
        PassCondition = () => true,
    });
}
