﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class FontSelectionDialog : TitleFocusedOverlayContainer, IHasCurrentValue<FontUsage>
    {
        protected override string Title => "Select font";

        private readonly SpriteText previewText;

        private readonly BindableWithCurrent<FontUsage> current = new BindableWithCurrent<FontUsage>();

        public Bindable<FontUsage> Current
        {
            get => current.Current;
            set => current.Current = value;
        }

        public FontSelectionDialog()
        {
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.6f, 0.8f);

            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Relative, 0.4f),
                    new Dimension()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        previewText = new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = "カラオケ, karaoke"
                        }
                    },
                    new Drawable[]
                    {
                        new Container
                        {
                            Padding = new MarginPadding(10),
                            RelativeSizeAxes = Axes.Both,
                            Child = new GridContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                ColumnDimensions = new[]
                                {
                                    new Dimension(GridSizeMode.Relative, 0.5f),
                                    new Dimension(GridSizeMode.Relative, 0.3f),
                                    new Dimension(GridSizeMode.Relative, 0.2f),
                                },
                                Content = new []
                                {
                                    new Drawable[]
                                    {
                                        new TextPropertyList<int>
                                        {
                                            Name = "Font family selection area",
                                            RelativeSizeAxes = Axes.Both
                                        },
                                        new TextPropertyList<int>
                                        {
                                            Name = "Font widget selection area",
                                            RelativeSizeAxes = Axes.Both
                                        },
                                        new GridContainer
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            RowDimensions = new[]
                                            {
                                                new Dimension(GridSizeMode.Distributed),
                                                new Dimension(GridSizeMode.Absolute, 48),
                                                new Dimension(GridSizeMode.Absolute, 64),
                                            },
                                            Content = new []
                                            {
                                                new Drawable[]
                                                {
                                                    new TextPropertyList<int>
                                                    {
                                                        Name = "Font size selection area",
                                                        RelativeSizeAxes = Axes.Both
                                                    },
                                                },
                                                new Drawable[]
                                                {
                                                    new OsuCheckbox
                                                    {
                                                        Name = "Font fixed width selection area",
                                                        RelativeSizeAxes = Axes.X,
                                                        Padding = new MarginPadding(10),
                                                        LabelText = "FixedWidth",
                                                    },
                                                },
                                                new Drawable[]
                                                {
                                                    // OK Button.
                                                    new TriangleButton
                                                    {
                                                        Name = "OK Button",
                                                        RelativeSizeAxes = Axes.X,
                                                        Padding = new MarginPadding(10),
                                                        Text = "OK",
                                                        Height = 64,
                                                        Action = () => {
                                                            // set to current value and hide.
                                                            var font = generateFontUsage();
                                                            Current.Value = font;
                                                            Hide();
                                                        }
                                                    },
                                                }
                                            }
                                        }
                                    },
                                }
                            }
                        }
                    }
                }
            };
        }

        private void previewChange()
        {
            previewText.Font = generateFontUsage();
        }

        private FontUsage generateFontUsage()
        {
            // todo : implemeht
            return OsuFont.Default;
        }

        internal class TextPropertyList<T> : CompositeDrawable
        {
            private readonly CornerBackground background;
            private readonly TextPropertySearchTextBox filter;
            private readonly RearrangeableTextListContainer<T> propertyList;

            private readonly BindableWithCurrent<T> current = new BindableWithCurrent<T>();

            public Bindable<T> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            public BindableList<T> List => propertyList.Items;

            public TextPropertyList()
            {
                InternalChild = new Container
                {
                    Padding = new MarginPadding(10),
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                    background = new CornerBackground
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                    new GridContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        RowDimensions = new[]
                        {
                            new Dimension(GridSizeMode.Absolute, 40),
                            new Dimension()
                        },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                filter = new TextPropertySearchTextBox
                                {
                                    RelativeSizeAxes = Axes.X,
                                }
                            },
                            new Drawable[]
                            {
                                propertyList = new RearrangeableTextListContainer<T>
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    RequestSelection = item =>
                                    {
                                        Current.Value = item;
                                        Hide();
                                    },
                                }
                            }
                        }
                    }
                    }
                };

                filter.Current.BindValueChanged(e => propertyList.Filter(e.NewValue));
                Current.BindValueChanged(e => propertyList.SelectedSet.Value = e.NewValue);
            }


            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                background.Colour = colours.ContextMenuGray;
            }

            public class TextPropertySearchTextBox : SearchTextBox
            {
                protected override Color4 SelectionColour => Color4.Gray;

                public TextPropertySearchTextBox()
                {
                    PlaceholderText = @"Search...";
                }
            }
        }
    }
}
