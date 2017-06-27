using iFactr.Core;
using iFactr.Core.Forms;
using iFactr.Core.Layers;
using iFactr.UI.Controls;
using MonoCross;
using MonoCross.Navigation;
using MonoCross.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace iFactr.UI
{
    /// <summary>
    /// Provides additional methods for <see cref="iLayer"/> objects.
    /// </summary>
    public static class iLayerExtensions
    {
        /// <summary>
        /// Generates an <see cref="IMXView"/> instance that visually and functionally replicates the layer.
        /// </summary>
        /// <param name="layer">The layer from which the view will be generated.</param>
        /// <returns>The view that was generated.</returns>
        public static IMXView GetView(this iLayer layer)
        {
            if (layer is NavigationTabs)
            {
                return GetTabView((NavigationTabs)layer);
            }

            if (layer is Browser)
            {
                return GetBrowserView((Browser)layer);
            }

            if (layer is LoginLayer)
            {
                return GetLoginView((LoginLayer)layer);
            }

            return GetListView(layer);
        }

        private static IListView GetListView(iLayer layer)
        {
            var view = new ListView<iLayer>(layer, layer.Layout == LayerLayout.Rounded ? ListViewStyle.Grouped : ListViewStyle.Default);

            var popover = layer as IPopoverLayer;
            view.PopoverPresentationStyle = popover != null && popover.IsFullscreen ||
                layer.PopoverPresentationStyle == PopoverPresentationStyle.FullScreen ?
                PopoverPresentationStyle.FullScreen : PopoverPresentationStyle.Normal;

            view.Activated += ViewActivated;
            view.Deactivated += ViewDeactivated;

            view.Rendering += (o, e) =>
            {
                layer = view.GetModel() as iLayer;
                if (layer == null) return;
                layer.FieldValuesRequested -= GetFieldValues;
                layer.FieldValuesRequested += GetFieldValues;

                view.StackID = layer.Name ?? layer.GetType().FullName;
                view.Title = layer.Title;
                view.TitleColor = layer.LayerStyle.HeaderTextColor;
                view.HeaderColor = layer.LayerStyle.HeaderColor;
                view.SeparatorColor = layer.LayerStyle.SeparatorColor;
                view.BackLink = layer.BackButton;
                view.OutputPane = layer.NavContext.OutputOnPane;
                ((IHistoryEntry)view).ShouldNavigate = layer.ShouldNavigateFrom;
                view.ColumnMode = (ColumnMode)layer.CompositeLayerLayout;

                if (layer.LayerStyle.LayerBackgroundImage != null)
                {
                    view.SetBackground(layer.LayerStyle.LayerBackgroundImage, ContentStretch.Fill);
                }
                else
                {
                    view.SetBackground(layer.LayerStyle.LayerBackgroundColor);
                }

                var searchList = layer.Items.OfType<SearchList>().FirstOrDefault();
                if (searchList != null)
                {
                    var searchBox = new SearchBox();
                    searchBox.SearchPerformed += (sender, args) =>
                    {
                        if (layer == null) return;
                        for (int i = 0; i < layer.Items.Count; i++)
                        {
                            var list = layer.Items[i] as SearchList;
                            if (list == null) continue;
                            list.PerformSearch(layer.NavContext.NavigatedUrl, args.SearchText);
                            view.Sections[i].ItemCount = list.Count;
                        }
                        view.ReloadSections();
                    };

                    searchBox.BorderColor = layer.LayerStyle.SectionHeaderColor;
                    searchBox.Placeholder = iApp.Factory.GetResourceString("SearchHint");
                    searchBox.Text = searchList.SearchText;

                    view.SearchBox = searchBox;
                    if (searchList.AutoFocus)
                    {
                        searchBox.Focus();
                    }
                }

                if (layer.ActionButtons != null && layer.ActionButtons.Count > 0)
                {
                    var menu = new Menu();
                    if (menu != null)
                    {
                        menu.BackgroundColor = layer.LayerStyle.HeaderColor;
                        menu.ForegroundColor = layer.LayerStyle.HeaderTextColor;

                        foreach (var button in layer.ActionButtons)
                        {
                            var item = new MenuButton(button.Text);
                            if (item == null) continue;
                            item.NavigationLink = button;
                            item.ImagePath = button.Image == null ? null : button.Image.Location;

                            menu.Add(item);
                        }

                        view.Menu = menu;
                    }
                }
                else
                {
                    view.Menu = null;
                }

                int sectionIndex = 0;
                view.Sections.Clear();
                for (int i = 0; i < layer.Items.Count; i++)
                {
                    var item = layer.Items[i];
                    var aggregate = iApp.Factory.Target == MobileTarget.Windows ? item as AggregateFieldset : null;
                    if (aggregate != null && aggregate.AggregateHeader == null && i > 0 && layer.Items[i - 1] is AggregateFieldset)
                    {
                        aggregate.SectionIndex = -1;
                        continue;
                    }

                    item.SectionIndex = sectionIndex++;
                    var section = new Section { ItemCount = (item is IList && aggregate == null) ? ((IList)item).Count : 1, };

                    if (aggregate != null && aggregate.AggregateHeader == null)
                    {
                        section.Header = null;
                    }
                    else if (item.Header == null)
                    {
                        section.Header = null;
                    }
                    else
                    {
                        if (section.Header == null)
                        {
                            section.Header = new SectionHeader();
                        }
                        section.Header.BackgroundColor = layer.LayerStyle.SectionHeaderColor;
                        section.Header.ForegroundColor = layer.LayerStyle.SectionHeaderTextColor;
                        section.Header.Text = aggregate == null ? item.Header : aggregate.AggregateHeader;
                    }

                    if (item.Footer == null)
                    {
                        section.Footer = null;
                    }
                    else
                    {
                        if (section.Footer == null)
                        {
                            section.Footer = new SectionFooter();
                        }
                        section.Footer.BackgroundColor = layer.LayerStyle.SectionHeaderColor;
                        section.Footer.ForegroundColor = layer.LayerStyle.SectionHeaderTextColor;
                        section.Footer.Text = item.Footer;
                    }

                    view.Sections.Add(section);
                }

                if (layer.FocusedItem != null)
                {
                    for (int i = 0; i < layer.Items.Count; i++)
                    {
                        var item = layer.Items[i];
                        if (item == layer.FocusedItem)
                        {
                            view.ScrollToCell(i, 0, false);
                            break;
                        }

                        var list = item as IList;
                        if (list != null)
                        {
                            int index = list.IndexOf(layer.FocusedItem);
                            if (index >= 0)
                            {
                                view.Metadata["NextField"] = list[index];
                                view.ScrollToCell(i, index, false);
                            }
                        }
                    }
                }
            };

            view.Submitting += (o, e) =>
            {
                e.Cancel = true;

                layer = view.GetModel() as iLayer;
                if (layer == null) return;
                var values = layer.GetFieldValues();
                layer.Validate(values);

                if (layer.IsValid)
                {
                    foreach (var key in values.Keys)
                    {
                        e.DestinationLink.Parameters[key] = values[key];
                    }
                    iApp.Navigate(e.DestinationLink, view);
                }
                else
                {
                    view.Render();
                    new Alert(iApp.Factory.GetResourceString("ValidationFailure"), string.Empty, AlertButtons.OK).Show();
                }
            };

            ((IListView)view).ItemIdRequested = (section, index) =>
            {
                layer = view.Model;
                if (layer == null)
                {
                    return 0;
                }

                var item = iApp.Factory.Target == MobileTarget.Windows ?
                    layer.Items.SingleOrDefault(i => i.SectionIndex == section) : layer.Items.ElementAtOrDefault(section);

                if (item == null)
                {
                    return 0;
                }

                if (iApp.Factory.Target == MobileTarget.Windows && item is AggregateFieldset)
                {
                    return item.SectionIndex;
                }

                var list = item as IList;
                if (list == null)
                {
                    return item.GetType().Name.GetHashCode();
                }

                var obj = list[index];
                var textField = obj as TextField;
                var hashCode = (textField != null && textField.IsPassword) ? "PasswordField".GetHashCode() : obj.GetType().Name.GetHashCode();
                if (Device.Platform == MobilePlatform.Android && (obj is TextField || obj is SelectListField))
                {
                    hashCode = (hashCode + 1) * 31 + section.GetHashCode();
                    hashCode = hashCode * 31 + index.GetHashCode();
                }
                return hashCode;
            };

            ((IListView)view).CellRequested = (section, index, recycledCell) =>
            {
                layer = view.Model;
                if (layer != null)
                {
                    var item = layer.Items.ElementAtOrDefault(section);

                    #region Aggregate Fieldsets
                    if (iApp.Factory.Target == MobileTarget.Windows)
                    {
                        AggregateFieldset aggregate = null;

                        try
                        {
                            item = layer.Items.SingleOrDefault(i => i.SectionIndex == section);
                        }
                        catch
                        {
                            int sectionIndex = 0;
                            for (int i = 0; i < layer.Items.Count; i++)
                            {
                                var li = layer.Items[i];
                                aggregate = li as AggregateFieldset;
                                if (aggregate != null && aggregate.AggregateHeader == null && i > 0 && layer.Items[i - 1] is AggregateFieldset)
                                {
                                    aggregate.SectionIndex = -1;
                                    continue;
                                }

                                li.SectionIndex = sectionIndex++;
                                if (li.SectionIndex == section)
                                {
                                    item = li;
                                }
                            }
                        }

                        aggregate = item as AggregateFieldset;
                        if (aggregate != null)
                        {
                            index = layer.Items.IndexOf(aggregate);
                            int count = 1;
                            for (int i = index + 1; i < layer.Items.Count; i++)
                            {
                                var fieldset = layer.Items[i] as AggregateFieldset;
                                if (fieldset == null || fieldset.AggregateHeader != null)
                                {
                                    break;
                                }
                                count++;
                            }

                            return iApp.Factory.Converter.ConvertToCell(layer.Items.GetRange(index, count).Cast<AggregateFieldset>(), layer.LayerStyle, view, recycledCell);
                        }
                    }
                    #endregion

                    var list = item as IList;

                    object cell = null;
                    if (list == null)
                    {
                        cell = item;
                    }
                    else if (index >= 0 && index < list.Count)
                    {
                        cell = list[index];
                    }

                    if (cell is ICustomItem)
                    {
                        return new CustomItemContainer(iApp.Factory.OnGetCustomItem((ICustomItem)cell, layer, view, recycledCell));
                    }

                    return cell == null ? null : iApp.Factory.Converter.ConvertToCell(cell, layer.LayerStyle, view, recycledCell);
                }

                return null;
            };

            return view;
        }

        private static IMXView GetLoginView(LoginLayer layer)
        {
            var view = new GridView<LoginLayer>();

            var popover = layer as IPopoverLayer;
            view.PopoverPresentationStyle = popover != null && popover.IsFullscreen ||
                layer.PopoverPresentationStyle == PopoverPresentationStyle.FullScreen ?
                PopoverPresentationStyle.FullScreen : PopoverPresentationStyle.Normal;

            view.Activated += ViewActivated;
            view.Deactivated += ViewDeactivated;

            view.Rendering += (o, e) =>
            {
                layer = view.GetModel() as LoginLayer;
                if (layer == null) return;
                layer.FieldValuesRequested -= GetFieldValues;
                layer.FieldValuesRequested += GetFieldValues;

                view.StackID = layer.Name ?? layer.GetType().FullName;
                view.Title = layer.Title;
                view.TitleColor = layer.LayerStyle.HeaderTextColor;
                view.HeaderColor = layer.LayerStyle.HeaderColor;
                view.BackLink = layer.BackButton;
                view.OutputPane = layer.NavContext.OutputOnPane;
                ((IHistoryEntry)view).ShouldNavigate = layer.ShouldNavigateFrom;
                if (layer.LayerStyle.LayerBackgroundImage != null)
                {
                    view.SetBackground(layer.LayerStyle.LayerBackgroundImage, ContentStretch.Fill);
                }
                else
                {
                    view.SetBackground(layer.LayerStyle.LayerBackgroundColor);
                }

                if (layer.ActionButtons != null && layer.ActionButtons.Count > 0)
                {
                    var menu = new Menu();
                    if (menu != null)
                    {
                        menu.BackgroundColor = layer.LayerStyle.HeaderColor;
                        menu.ForegroundColor = layer.LayerStyle.HeaderTextColor;

                        foreach (var button in layer.ActionButtons)
                        {
                            var item = new MenuButton(button.Text);
                            if (item == null) continue;
                            item.NavigationLink = button;
                            item.ImagePath = button.Image == null ? null : button.Image.Location;

                            menu.Add(item);
                        }

                        view.Menu = menu;
                    }
                }
                else
                {
                    view.Menu = null;
                }

                view.VerticalScrollingEnabled = true;
                foreach (var element in view.Children.ToList())
                {
                    view.RemoveChild(element);
                }
                view.Columns.Clear();
                view.Rows.Clear();
                view.Columns.AddRange(new[] { Column.AutoSized, Column.OneStar });
                view.Padding = new Thickness(Thickness.LeftMargin, Thickness.TopMargin, Thickness.RightMargin, Thickness.BottomMargin);

                if (layer.BrandImage != null)
                {
                    var brandImage = new Image(layer.BrandImage.Location)
                    {
                        RowIndex = view.Rows.Count,
                        RowSpan = 1,
                        ColumnIndex = 0,
                        ColumnSpan = 2,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    view.Rows.Add(Row.AutoSized);
                    view.AddChild(brandImage);
                }

                var typeSwitch = new TypeSwitch();
                foreach (var field in layer.Items.OfType<Fieldset>().SelectMany(f => f))
                {
                    var control = iApp.Factory.Converter.ConvertToControl(field, view, typeSwitch, null);
                    control.RowIndex = view.Rows.Count;
                    control.RowSpan = 1;
                    control.VerticalAlignment = VerticalAlignment.Center;
                    view.Rows.Add(new Row(Cell.StandardCellHeight, LayoutUnitType.Absolute));
                    var label = control as Label;
                    if (label != null)
                    {
                        if (string.IsNullOrEmpty(label.Text))
                            label.Text = field.Label;
                        if (label.Text == layer.ErrorText)
                        {
                            var color = layer.LayerStyle.ErrorTextColor;
                            label.ForegroundColor = color.IsDefaultColor ? Color.Red : color;
                        }
                    }
                    if (label != null && (string.IsNullOrEmpty(field.Label) || string.IsNullOrEmpty(field.Text)) || control is Image || control is Button)
                    {
                        control.ColumnIndex = 0;
                        control.ColumnSpan = 2;
                    }
                    else
                    {
                        view.AddChild(new Label
                        {
                            Text = field.Label,
                            ColumnIndex = 0,
                            ColumnSpan = 1,
                            RowIndex = control.RowIndex,
                            RowSpan = 1,
                            Margin = new Thickness(0, 0, Thickness.SmallHorizontalSpacing, 0),
                            VerticalAlignment = VerticalAlignment.Center
                        });
                        control.ColumnIndex = 1;
                        control.ColumnSpan = 1;
                    }
                    view.AddChild(control);
                }

                view.AddChild(new Label
                {
                    Text = " ",
                    ColumnIndex = 0,
                    ColumnSpan = 1,
                    RowIndex = view.Rows.Count,
                    RowSpan = 1,
                    Margin = new Thickness(0, 0, Thickness.SmallHorizontalSpacing, 0),
                    VerticalAlignment = VerticalAlignment.Center
                });
                view.Rows.Add(Row.OneStar);
            };

            view.Submitting += (o, e) =>
            {
                e.Cancel = true;

                layer = view.GetModel() as LoginLayer;
                if (layer == null) return;
                var values = layer.GetFieldValues();
                layer.Validate(values);

                if (layer.IsValid)
                {
                    foreach (var key in values.Keys)
                    {
                        e.DestinationLink.Parameters[key] = values[key];
                    }
                    iApp.Navigate(e.DestinationLink, view);
                }
                else
                {
                    view.Render();
                    new Alert(iApp.Factory.GetResourceString("ValidationFailure"), string.Empty, AlertButtons.OK).Show();
                }
            };

            return view;
        }

        private static IBrowserView GetBrowserView(Browser browserLayer)
        {
            var view = new BrowserView<Browser>(browserLayer);

            var popover = browserLayer as IPopoverLayer;
            view.PopoverPresentationStyle = popover != null && popover.IsFullscreen ||
                browserLayer.PopoverPresentationStyle == PopoverPresentationStyle.FullScreen ?
                PopoverPresentationStyle.FullScreen : PopoverPresentationStyle.Normal;

            view.Activated += ViewActivated;
            view.Deactivated += ViewDeactivated;

            view.Rendering += (o, e) =>
            {
                browserLayer = view.Model;
                if (browserLayer != null)
                {
                    view.StackID = browserLayer.Name ?? browserLayer.GetType().FullName;
                    view.Title = browserLayer.Title;
                    view.TitleColor = browserLayer.LayerStyle.HeaderTextColor;
                    view.HeaderColor = browserLayer.LayerStyle.HeaderColor;
                    view.BackLink = browserLayer.BackButton;
                    view.OutputPane = browserLayer.NavContext.OutputOnPane;
                    ((IHistoryEntry)view).ShouldNavigate = browserLayer.ShouldNavigateFrom;
                    view.SetBackground(browserLayer.LayerStyle.LayerBackgroundColor);

                    bool localSource = iApp.File.Exists(browserLayer.Url);
                    view.EnableDefaultControls = !localSource;

                    if (!localSource || (browserLayer.ActionButtons != null && browserLayer.ActionButtons.Count > 0))
                    {
                        var menu = new Menu();
                        menu.BackgroundColor = browserLayer.LayerStyle.HeaderColor;
                        menu.ForegroundColor = browserLayer.LayerStyle.HeaderTextColor;

                        foreach (var button in browserLayer.ActionButtons)
                        {
                            var item = new MenuButton(button.Text);
                            item.NavigationLink = button;
                            item.ImagePath = button.Image == null ? null : button.Image.Location;

                            menu.Add(item);
                        }

                        if (!localSource)
                        {
                            var refresh = new MenuButton(iApp.Factory.GetResourceString("Refresh"));
                            refresh.SetValue("ImagePath", "/Images/AppBar/appbar.refresh.rest.png", MobileTarget.WinPhone);
                            refresh.Clicked += (sender, args) => view.Refresh();
                            menu.Add(refresh);

                            var launch = new MenuButton(iApp.Factory.GetResourceString("LaunchExternal"));
                            launch.SetValue("ImagePath", "/Images/AppBar/appbar.launch.rest.png", MobileTarget.WinPhone); ;
                            launch.Clicked += (sender, args) =>
                            {
                                var alert = new Alert(iApp.Factory.GetResourceString("LaunchExternalConfirm"), string.Empty, AlertButtons.OKCancel);
                                alert.Dismissed += (obj, result) =>
                                {
                                    if (result.Result == AlertResult.OK)
                                    {
                                        view.LaunchExternal(browserLayer.Url);
                                    }
                                };
                                alert.Show();
                            };
                            menu.Add(launch);
                        }

                        view.Menu = menu;
                    }
                    else
                    {
                        view.Menu = null;
                    }

                    view.Load(browserLayer.Url);
                }
            };

            return view;
        }

        private static ITabView GetTabView(NavigationTabs tabLayer)
        {
            var view = new TabView<NavigationTabs>(tabLayer);

            view.Rendering += (o, e) =>
            {
                tabLayer = view.Model;
                if (tabLayer == null) return;
                view.HeaderColor = tabLayer.LayerStyle.HeaderColor;
                view.Title = tabLayer.Title;
                view.TitleColor = tabLayer.LayerStyle.HeaderTextColor;

                if (tabLayer.LayerStyle.LayerBackgroundImage != null)
                {
                    view.SetBackground(tabLayer.LayerStyle.LayerBackgroundImage, ContentStretch.Fill);
                }
                else
                {
                    view.SetBackground(tabLayer.LayerStyle.LayerBackgroundColor);
                }

                var tabs = new TabItem[tabLayer.TabItems.Count];
                for (int i = 0; i < tabLayer.TabItems.Count; i++)
                {
                    var item = tabLayer.TabItems[i];

                    var tab = new TabItem();
                    tab.Title = item.Text;
                    tab.BadgeValue = item.Badge;
                    tab.NavigationLink = item.Link;

                    if (item.RefreshOnFocus)
                    {
                        tab.Selected += (sender, args) => iApp.Navigate(tab.NavigationLink, view);
                    }

                    if (item.Icon != null)
                    {
                        // metro has some funny logic to account for
                        if (iApp.Factory.Target == MobileTarget.Metro && item.Icon.Location != null)
                        {
                            string navIcon = item.Icon.Location.Insert(item.Icon.Location.LastIndexOf('/') + 1, "Nav-");
                            tab.ImagePath = navIcon;
                        }
                        else
                        {
                            tab.ImagePath = item.Icon.Location;
                        }
                    }

                    tabs[i] = tab;
                }

                view.TabItems = tabs;
            };

            return view;
        }

        private static void ViewActivated(object sender, EventArgs e)
        {
            var view = sender as IMXView;
            var entry = sender as IHistoryEntry;
            var layer = view == null ? null : view.GetModel() as iLayer;
            if (layer == null || entry == null) return;

            layer.NavContext.NavigatedActiveTab = iApp.CurrentNavContext.ActiveTab;
            layer.NavContext.OutputOnPane = entry.Stack == null ? iApp.CurrentNavContext.ActivePane : entry.Stack.FindPane();

            iApp.CurrentNavContext.ActiveLayer = layer;
            iApp.SetNavigationContext(layer.NavContext);
            if (layer.DetailLink != null && PaneManager.IsSplitView && entry.OutputPane == Pane.Master)
            {
                iApp.Navigate(layer.DetailLink, view);
            }
        }

        private static void ViewDeactivated(object sender, EventArgs e)
        {
            if (!PaneManager.IsSplitView)
            {
                return;
            }

            var entry = sender as IHistoryEntry;
            var stack = PaneManager.Instance.FromNavContext(Pane.Master, PaneManager.Instance.CurrentTab);
            if (entry != null && entry.OutputPane == Pane.Master && stack != null && stack.CurrentView != entry)
            {
                var detail = PaneManager.Instance.FromNavContext(Pane.Detail, 0);
                if (detail != null && detail.FindPane() == Pane.Detail)
                {
                    detail.PopToRoot();
                }
            }
        }

        private static Dictionary<string, string> GetFieldValues(object sender)
        {
            var layer = sender as iLayer;
            if (layer == null) return null;
            var parameters = new Dictionary<string, string>();
            var typeSwitch = new TypeSwitch();
            foreach (var field in layer.Items.OfType<Fieldset>().SelectMany(f => f).Where(f => f.ID != null))
            {
                typeSwitch.Object = field;
                typeSwitch
                    .Case<SelectListField>(c =>
                    {
                        parameters[c.ID] = c.SelectedValue;
                        parameters[c.ID + ".Key"] = c.SelectedKey;
                    })
                    .Case<BoolField>(c => parameters[c.ID] = c.Value ? "on" : "off")
                    .Case<DateField>(f =>
                    {
                        if (f.Value == null)
                        {
                            parameters[f.ID] = string.Empty;
                        }
                        else switch (f.Type)
                            {
                                case DateField.DateType.Date:
                                    parameters[f.ID] = f.Value.Value.ToString("d");
                                    break;
                                case DateField.DateType.Time:
                                    parameters[f.ID] = f.Value.Value.ToString("t");
                                    break;
                                default:
                                    parameters[f.ID] = f.Value.Value.ToString("g");
                                    break;
                            }
                    })
                    .Case<DrawingField>(c => parameters[c.ID] = c.DrawnImageId ?? string.Empty)
                    .Case<SliderField>(c => parameters[c.ID] = c.Value.ToString())
                    .Case<LabelField>(f => f.Text.IsNullOrEmptyOrWhiteSpace(), c => parameters[c.ID] = c.Label ?? string.Empty)
                    .Case<Field>(f => parameters[f.ID] = f.Text ?? string.Empty);
            }

            return parameters;
        }
    }
}