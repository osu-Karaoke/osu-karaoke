// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Infos;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public abstract class CheckStageInfo<TStageInfo> : ICheck
    where TStageInfo : StageInfo
{
    public CheckMetadata Metadata => new(CheckCategory.Files, Description);

    protected abstract string Description { get; }

    public IEnumerable<IssueTemplate> PossibleTemplates => baseTemplates.Concat(CustomTemplates);

    private IEnumerable<IssueTemplate> baseTemplates => new IssueTemplate[]
    {
        new IssueTemplateNoElement(this),
        new IssueTemplateMappingHitObjectNotExist(this),
        new IssueTemplateMappingItemNotExist(this),
    };

    public abstract IEnumerable<IssueTemplate> CustomTemplates { get; }

    private readonly IList<Func<TStageInfo, IReadOnlyList<KaraokeHitObject>, IEnumerable<Issue>>> stageInfoCategoryActions
        = new List<Func<TStageInfo, IReadOnlyList<KaraokeHitObject>, IEnumerable<Issue>>>();

    public void RegisterCategory<TStageElement, THitObject>(Func<TStageInfo, StageElementCategory<TStageElement, THitObject>> categoryAction, int minimumRequiredElements)
        where TStageElement : StageElement, new()
        where THitObject : KaraokeHitObject, IHasPrimaryKey
    {
        stageInfoCategoryActions.Add((info, hitObjects) =>
        {
            var category = categoryAction(info);
            return checkElementCategory(category, hitObjects.OfType<THitObject>().ToList(), minimumRequiredElements);
        });
    }

    private IEnumerable<Issue> checkElementCategory<TStageElement, THitObject>(StageElementCategory<TStageElement, THitObject> category, IReadOnlyList<THitObject> hitObjects,
                                                                               int minimumRequiredElements)
        where TStageElement : StageElement, new()
        where THitObject : KaraokeHitObject, IHasPrimaryKey
    {
        // check mapping.
        var issues = checkMappings(category, hitObjects).ToList();

        // check element amount.
        if (category.AvailableElements.Count < minimumRequiredElements)
            issues.Add(new IssueTemplateNoElement(this).Create(minimumRequiredElements));

        // check elements.
        issues.AddRange(CheckElement(category.DefaultElement));

        foreach (var element in category.AvailableElements)
        {
            issues.AddRange(CheckElement(element));
        }

        return issues;
    }

    private IEnumerable<Issue> checkMappings<TStageElement, THitObject>(StageElementCategory<TStageElement, THitObject> category, IReadOnlyList<THitObject> hitObjects)
        where TStageElement : StageElement, new()
        where THitObject : KaraokeHitObject, IHasPrimaryKey
    {
        var elements = category.AvailableElements;
        var mappings = category.Mappings;

        foreach (var mapping in mappings)
        {
            if (hitObjects.All(x => x.ID != mapping.Key))
                yield return new IssueTemplateMappingHitObjectNotExist(this).Create();

            if (elements.All(x => x.ID != mapping.Value))
                yield return new IssueTemplateMappingItemNotExist(this).Create();
        }
    }

    public IEnumerable<Issue> Run(BeatmapVerifierContext context)
    {
        var property = getStageInfo(context);
        if (property == null)
            return Array.Empty<Issue>();

        var hitObjects = context.Beatmap.HitObjects.OfType<KaraokeHitObject>().ToList();
        var issues = CheckStageInfoWithHitObjects(property, hitObjects).ToList();

        foreach (var stageInfoCategoryAction in stageInfoCategoryActions)
        {
            issues.AddRange(stageInfoCategoryAction(property, hitObjects));
        }

        return issues;

        // todo: get stage info from context.
        static TStageInfo? getStageInfo(BeatmapVerifierContext context)
            => throw new NotImplementedException();
    }

    public abstract IEnumerable<Issue> CheckStageInfoWithHitObjects(TStageInfo stageInfo, IReadOnlyList<KaraokeHitObject> hitObjects);

    protected abstract IEnumerable<Issue> CheckElement<TStageElement>(TStageElement element) where TStageElement : StageElement;

    public class IssueTemplateNoElement : IssueTemplate
    {
        public IssueTemplateNoElement(ICheck check)
            : base(check, IssueType.Warning, "Should have at least {0} elements in the stage.")
        {
        }

        public Issue Create(int minimumRequiredElements) => new(this, minimumRequiredElements);
    }

    public class IssueTemplateMappingHitObjectNotExist : IssueTemplate
    {
        public IssueTemplateMappingHitObjectNotExist(ICheck check)
            : base(check, IssueType.Warning, "Maybe caused by hit-object has been deleted. Don't worry, go to the stage editor and will be easy to fix them.")
        {
        }

        public Issue Create() => new(this);
    }

    public class IssueTemplateMappingItemNotExist : IssueTemplate
    {
        public IssueTemplateMappingItemNotExist(ICheck check)
            : base(check, IssueType.Error, "It's caused by stage element has been deleted, but still remain the mapping data.")
        {
        }

        public Issue Create() => new(this);
    }
}
