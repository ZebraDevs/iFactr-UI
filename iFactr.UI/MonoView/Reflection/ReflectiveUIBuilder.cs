using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using iFactr.Core;
using MonoCross.Utilities;
using iFactr.UI.Controls;

namespace iFactr.UI.Reflection
{
    internal static class ReflectiveUIBuilder
    {
        public static IView GenerateView(object model)
        {
            var viewAtt = Device.Reflector.GetCustomAttribute<ViewAttribute>(model.GetType(), true);
            IView view = null;
            switch (viewAtt.Type)
            {
                case ViewType.Browser:
                    view = GenerateBrowserView(model, viewAtt);
                    break;
                case ViewType.Canvas:
                    view = GenerateCanvasView(model, viewAtt);
                    break;
                case ViewType.List:
                    view = GenerateListView(model, viewAtt);
                    break;
                case ViewType.Tabs:
                    view = GenerateTabView(model, viewAtt);
                    break;
            }

            view.SetModel(model);
            view.SetBackground(GetColorFromString(ExtractParameters(viewAtt.BackgroundColor, model)));
            view.HeaderColor = GetColorFromString(ExtractParameters(viewAtt.HeaderColor, model));
            view.PreferredOrientations = viewAtt.PreferredOrientations;
            view.TitleColor = GetColorFromString(ExtractParameters(viewAtt.TitleColor, model));
            view.Title = ExtractParameters(viewAtt.Title, model) ?? FormatStringForDisplay(model.GetType().Name);

            var entry = view as IHistoryEntry;
            if (entry != null)
            {
                entry.BackLink = viewAtt.BackAddress == null ? null : new Link(viewAtt.BackAddress);
                entry.OutputPane = viewAtt.OutputPane;
            }

            return view;
        }

        private static IBrowserView GenerateBrowserView(object model, ViewAttribute viewAtt)
        {
            var browserView = new BrowserView<object>(model);
            browserView.EnableDefaultControls = viewAtt.EnableBrowserDefaultControls;

            browserView.Rendering += (sender, e) =>
            {
                var view = sender as IBrowserView;
                if (view != null)
                {
                    browserView.Load(ExtractParameters(viewAtt.BrowserUrl, browserView.GetModel()));
                }
            };

            return browserView;
        }

        private static ICanvasView GenerateCanvasView(object model, ViewAttribute viewAtt)
        {
            var canvasView = new CanvasView<object>(model);

            canvasView.StrokeColor = GetColorFromString(ExtractParameters(viewAtt.CanvasStrokeColor, model));
            canvasView.StrokeThickness = double.Parse(ExtractParameters(viewAtt.CanvasStrokeThickness, model));

            return canvasView;
        }

        private static IListView GenerateListView(object model, ViewAttribute viewAtt)
        {
            var listView = new ListView<object>(model, viewAtt.ListStyle);

            listView.SeparatorColor = GetColorFromString(ExtractParameters(viewAtt.ListSeparatorColor, model));

            listView.Rendering += (object sender, EventArgs e) =>
            {
                var view = sender as IListView;
                if (view != null)
                {
                    view.Sections.Clear();
                    model = view.GetModel();
                    if (model != null)
                    {
                        var parameters = new ReflectiveParameters();
                        ProcessObjectForList(model, parameters, null);

                        var collection = model as ICollection;
                        if (collection != null && collection.Count > 0)
                        {
                            ProcessObjectForList(collection.ElementAt(0), parameters, collection);
                        }

                        foreach (var section in parameters.Sections)
                        {
                            section.ItemCount = section.Items.Count;
                            view.Sections.Add(section);
                        }
                    }
                }
            };

            ((IListView)listView).CellRequested = (sectionIndex, cellIndex, recycledCell) =>
            {
                var section = (ReflectiveSection)listView.Sections[sectionIndex];
                var cellInfo = section.Items[cellIndex];

                return ProcessCell(cellInfo, recycledCell);
            };

            ((IListView)listView).ItemIdRequested = (sectionIndex, itemIndex) =>
            {
                var section = (ReflectiveSection)listView.Sections[sectionIndex];
                var item = section.Items[itemIndex];

                return item.CellType == typeof(ContentCell) ? 0 :
                    item.Infos.Length * (item.CellType == typeof(HeaderedControlCell) ? -1 : 1);
            };

            return listView;
        }

        private static ITabView GenerateTabView(object model, ViewAttribute viewAtt)
        {
            var tabView = new TabView<object>(model);
            tabView.SelectionColor = GetColorFromString(ExtractParameters(viewAtt.TabSelectionColor, model));

            tabView.Rendering += (sender, e) =>
            {
                var view = sender as ITabView;
                if (view != null)
                {
                    view.TabItems = null;
                    model = view.GetModel();
                    if (model != null)
                    {
                        var parameters = new ReflectiveParameters() { Sections = { new ReflectiveSection() } };
                        ProcessObjectForTabs(model, parameters, null);

                        var collection = model as ICollection;
                        if (collection != null && collection.Count > 0)
                        {
                            ProcessObjectForTabs(collection.ElementAt(0), parameters, collection);
                        }

                        var tabItems = new List<TabItem>();
                        foreach (var tabInfo in parameters.Sections[0].Items)
                        {
                            var info = tabInfo.Infos[0];
                            var attributes = info.GetCustomAttributes(false);

                            var tabItem = new TabItem();
                            tabItem.Title = FormatStringForDisplay(info.Name);

                            var propertyInfo = info as PropertyInfo;
                            if (propertyInfo != null)
                            {
                                if (propertyInfo.
#if NETCF
GetGetMethod()
#else
                                    GetMethod
#endif
 == null)
                                {
                                    throw new MissingMemberException(string.Format("Missing getter for property {0}.  A getter is required for UI reflection.", propertyInfo.Name));
                                }

                                var value = propertyInfo.GetValue(tabInfo.Object, null);
                                tabItem.NavigationLink = new Link(value == null ? null : value.ToString());
                            }

                            foreach (var attribute in attributes)
                            {
                                var labelAtt = attribute as LabelAttribute;
                                if (labelAtt != null)
                                {
                                    tabItem.Title = ExtractParameters(labelAtt.Text, tabInfo.Object);
                                    tabItem.TitleColor = GetColorFromString(ExtractParameters(labelAtt.ForegroundColor, tabInfo.Object));
                                    tabItem.TitleFont = new Font(labelAtt.FontName, double.Parse(ExtractParameters(labelAtt.FontSize, tabInfo.Object)), labelAtt.FontFormatting);
                                }

                                var selectableAtt = attribute as SelectableAttribute;
                                if (selectableAtt != null)
                                {
                                    tabItem.NavigationLink = new Link(ExtractParameters(selectableAtt.NavigationAddress, tabInfo.Object));
                                }

                                var imageAtt = attribute as ImageAttribute;
                                if (imageAtt != null)
                                {
                                    tabItem.ImagePath = ExtractParameters(imageAtt.FilePath, tabInfo.Object);
                                }
                            }

                            tabItems.Add(tabItem);
                        }
                        view.TabItems = tabItems.Cast<ITabItem>();
                    }
                }
            };

            return tabView;
        }

        private static void ProcessObjectForList(object obj, ReflectiveParameters parameters, ICollection collection)
        {
            var objType = obj.GetType();

            var attributes = Device.Reflector.GetCustomAttributes(objType, false);
            if (attributes.Any(a => a is SkipAttribute))
            {
                return;
            }

            parameters.Participation = Participation.OptOut;

            var typeSection = parameters.Sections.LastOrDefault();
            Group objectGroup = null;
            var groups = new List<Group>();
            foreach (var attribute in attributes)
            {
                var memparAtt = attribute as MemberParticipationAttribute;
                if (memparAtt != null)
                {
                    parameters.Participation = memparAtt.Participation;
                }

                var groupAtt = attribute as GroupAttribute;
                if (groupAtt != null)
                {
                    objectGroup = new Group() { Id = groupAtt.Id, Infos = new List<Order>(), Order = 0 };
                    groups.Add(objectGroup);
                }

                var sectionAtt = attribute as SectionAttribute;
                if (sectionAtt != null)
                {
                    typeSection = new ReflectiveSection();
                    if (sectionAtt.HeaderText == null)
                    {
                        typeSection.Header = null;
                    }
                    else
                    {
                        if (typeSection.Header == null)
                        {
                            typeSection.Header = new SectionHeader();
                        }
                        typeSection.Header.Text = ExtractParameters(sectionAtt.HeaderText, obj);
                        typeSection.Header.ForegroundColor = GetColorFromString(ExtractParameters(sectionAtt.HeaderTextColor, obj));
                        typeSection.Header.BackgroundColor = GetColorFromString(ExtractParameters(sectionAtt.HeaderColor, obj));
                    }

                    if (sectionAtt.FooterText == null)
                    {
                        typeSection.Footer = null;
                    }
                    else
                    {
                        if (typeSection.Footer == null)
                        {
                            typeSection.Footer = new SectionFooter();
                        }
                        typeSection.Footer.Text = ExtractParameters(sectionAtt.FooterText, obj);
                        typeSection.Footer.ForegroundColor = GetColorFromString(ExtractParameters(sectionAtt.FooterTextColor, obj));
                        typeSection.Footer.BackgroundColor = GetColorFromString(ExtractParameters(sectionAtt.FooterColor, obj));
                    }
                }
            }

            typeSection = typeSection ?? new ReflectiveSection() { Header = null, Footer = null };
            if (objectGroup != null)
            {
                objectGroup.Section = typeSection;
            }

            var propertyInfos = Device.Reflector.GetProperties(objType).Where(p => p.DeclaringType == obj.GetType() && p.Name != "Item");
            foreach (var info in propertyInfos)
            {
                attributes = info.GetCustomAttributes(false).Cast<Attribute>();
                bool optedOut = attributes.Any(a => a is SkipAttribute);
                bool optedIn = attributes.Any(a => (a is OptInAttribute));
                if (!optedOut && !optedIn)
                {
                    var col = info.GetValue(obj, null) as ICollection;
                    if (col != null)
                    {
                        var order = (OrderAttribute)attributes.FirstOrDefault(a => a is OrderAttribute);
                        var group = new Group() { Collection = new List<ICollection>() { col } };

                        if (order != null)
                        {
                            group.Order = Math.Min(group.Order, order.Index);
                        }

                        groups.Add(group);
                        continue;
                    }
                }

                if ((parameters.Participation == Participation.OptOut && optedOut)
                    || (parameters.Participation == Participation.OptIn && !optedIn))
                {
                    continue;
                }

                if (objectGroup != null)
                {
                    var orderAtt = (OrderAttribute)attributes.FirstOrDefault(a => a is OrderAttribute);
                    objectGroup.Infos.Add(new Order()
                    {
                        Index = orderAtt == null ? null : (int?)orderAtt.Index,
                        Info = info,
                        Attributes = attributes.Where(a =>
                            a is ControlAttribute || a is LabelAttribute || a is ImageAttribute || a is SelectableAttribute
                        ).ToArray()
                    });
                }
                else
                {
                    ReflectiveSection section = null;
                    Group group = null;
                    Order order = new Order()
                    {
                        Info = info,
                        Attributes = attributes.Where(a =>
                            a is ControlAttribute || a is LabelAttribute || a is ImageAttribute || a is SelectableAttribute
                        ).ToArray()
                    };

                    foreach (var attribute in attributes)
                    {
                        var groupAtt = attribute as GroupAttribute;
                        if (groupAtt != null)
                        {
                            group = groups.FirstOrDefault(g => g.Id == groupAtt.Id) ?? new Group() { Infos = new List<Order>() };
                            group.Id = groupAtt.Id;
                        }

                        var orderAtt = attribute as OrderAttribute;
                        if (orderAtt != null)
                        {
                            order.Index = orderAtt.Index;
                        }

                        var sectionAtt = attribute as SectionAttribute;
                        if (sectionAtt != null)
                        {
                            section = new ReflectiveSection();
                            if (sectionAtt.HeaderText == null)
                            {
                                section.Header = null;
                            }
                            else
                            {
                                if (section.Header == null)
                                {
                                    section.Header = new SectionHeader();
                                }
                                section.Header.Text = ExtractParameters(sectionAtt.HeaderText, obj); ;
                                section.Header.ForegroundColor = GetColorFromString(ExtractParameters(sectionAtt.HeaderTextColor, obj));
                                section.Header.BackgroundColor = GetColorFromString(ExtractParameters(sectionAtt.HeaderColor, obj));
                            }

                            if (sectionAtt.FooterText == null)
                            {
                                section.Footer = null;
                            }
                            else
                            {
                                if (section.Footer == null)
                                {
                                    section.Footer = new SectionFooter();
                                }
                                section.Footer.Text = ExtractParameters(sectionAtt.FooterText, obj);
                                section.Footer.ForegroundColor = GetColorFromString(ExtractParameters(sectionAtt.FooterTextColor, obj));
                                section.Footer.BackgroundColor = GetColorFromString(ExtractParameters(sectionAtt.FooterColor, obj));
                            }
                        }
                    }

                    group = group ?? new Group() { Infos = new List<Order>() };

                    if (section != null)
                    {
                        group.Section = section;
                        section = null;
                    }

                    if (order.Index.HasValue)
                    {
                        group.Order = Math.Min(group.Order, order.Index.Value);
                    }

                    group.Infos.Add(order);
                    if (group.Id == null || !groups.Contains(group))
                    {
                        groups.Add(group);
                    }
                }
            }

            if (typeSection != null && !parameters.Sections.Contains(typeSection))
            {
                parameters.Sections.Add(typeSection);
            }

            var ordered = groups.OrderBy(g => g.Order);
            ReflectiveSection currentSection = null;

            for (int i = 0; i < (collection == null ? 1 : collection.Count); i++)
            {
                foreach (var group in ordered)
                {
                    if (group.Collection != null)
                    {
                        foreach (var col in group.Collection.Where(c => c.Count > 0))
                        {
                            parameters.Participation = Participation.OptOut;
                            ProcessObjectForList(col.ElementAt(0), parameters, col);
                        }
                        currentSection = parameters.Sections.LastOrDefault();
                        continue;
                    }

                    var infos = group.Infos.OrderBy(o => o.Index.HasValue ? o.Index.Value : long.MaxValue);
                    if (group.Section == null && currentSection == null)
                    {
                        currentSection = typeSection;
                    }
                    else if (group.Section != null)
                    {
                        var section = group.Section.Clone();
                        parameters.Sections.Add(section);
                        currentSection = section;
                    }

                    var itemInfos = infos.Select(o => o.Info).ToArray();
                    var itemAtts = infos.Select(o => o.Attributes).ToArray();
                    if (group.CellType == null)
                    {
                        group.CellType = GetCellType(itemInfos, itemAtts);
                    }

                    currentSection.Items.Add(new Item()
                    {
                        Object = collection == null ? obj : collection.ElementAt(i),
                        Infos = itemInfos,
                        Attributes = itemAtts,
                        CellType = group.CellType
                    });
                }
            }
        }

        private static ICell ProcessCell(Item cellInfo, ICell recycledCell)
        {
            #region ContentCell
            if (cellInfo.CellType == typeof(ContentCell))
            {
                var cell = recycledCell as ContentCell ?? new ContentCell();
                cell.NullifyEvents();
                cell.NavigationLink = null;
                cell.AccessoryLink = null;
                cell.SelectionStyle = SelectionStyle.Default;
                cell.SelectionColor = iApp.Instance.Style.SelectionColor;
                cell.TextLabel.Text = null;
                cell.SubtextLabel.Text = null;
                cell.ValueLabel.Text = null;

                string imagePath = null;
                if (cellInfo.Infos.Length > 0)
                {
                    ControlAttribute controlAttribute = null;
                    LabelAttribute labelAttribute = null;
                    foreach (var attribute in cellInfo.Attributes[0])
                    {
                        var controlAtt = attribute as ControlAttribute;
                        if (controlAtt != null)
                        {
                            controlAttribute = controlAtt;
                        }

                        var labelAtt = attribute as LabelAttribute;
                        if (labelAtt != null)
                        {
                            labelAttribute = labelAtt;
                        }

                        var selectableAtt = attribute as SelectableAttribute;
                        if (selectableAtt != null)
                        {
                            cell.SelectionColor = GetColorFromString(ExtractParameters(selectableAtt.SelectionColor, cellInfo.Object));
                            cell.SelectionStyle = selectableAtt.SelectionStyle;

                            if (selectableAtt.NavigationAddress != null)
                            {
                                cell.NavigationLink = new Link(ExtractParameters(selectableAtt.NavigationAddress, cellInfo.Object))
                                {
                                    Action = ActionType.Submit
                                };
                            }

                            if (selectableAtt.AccessoryAddress != null)
                            {
                                cell.AccessoryLink = new Link(ExtractParameters(selectableAtt.AccessoryAddress, cellInfo.Object))
                                {
                                    Action = ActionType.Submit
                                };
                            }
                        }

                        var imageAtt = attribute as ImageAttribute;
                        if (imageAtt != null)
                        {
                            imagePath = imageAtt.FilePath;
                        }
                    }

                    var info = cellInfo.Infos[0];
                    if (info.
#if NETCF
GetGetMethod()
#else
                        GetMethod
#endif
 == null)
                    {
                        throw new MissingMemberException(string.Format("Missing getter for property {0}.  A getter is required for UI reflection.", info.Name));
                    }

                    if (labelAttribute == null)
                    {
                        if (controlAttribute == null)
                        {
                            cell.TextLabel.Lines = 1;
                            cell.TextLabel.ForegroundColor = iApp.Instance.Style.TextColor;
                            cell.TextLabel.Font = Font.PreferredLabelFont;
                        }
                        else
                        {
                            cell.TextLabel.Lines = int.Parse(ExtractParameters(controlAttribute.LabelLines, cellInfo.Object)); ;
                            cell.TextLabel.ForegroundColor = GetColorFromString(ExtractParameters(controlAttribute.ForegroundColor, cellInfo.Object));
                            cell.TextLabel.Font = new Font(ExtractParameters(controlAttribute.FontName, cellInfo.Object),
                                double.Parse(ExtractParameters(controlAttribute.FontSize, cellInfo.Object)), controlAttribute.FontFormatting);
                        }

                        cell.TextLabel.Text = info.GetValue(cellInfo.Object, null) as string;
                    }
                    else
                    {
                        cell.TextLabel.Lines = int.Parse(ExtractParameters(labelAttribute.Lines, cellInfo.Object));
                        cell.TextLabel.ForegroundColor = GetColorFromString(ExtractParameters(labelAttribute.ForegroundColor, cellInfo.Object));
                        cell.TextLabel.Font = new Font(ExtractParameters(labelAttribute.FontName, cellInfo.Object),
                            double.Parse(ExtractParameters(labelAttribute.FontSize, cellInfo.Object)), labelAttribute.FontFormatting);
                        cell.TextLabel.Text = ExtractParameters(labelAttribute.Text, cellInfo.Object);

                        if (controlAttribute == null)
                        {
                            cell.ValueLabel.Lines = 1;
                            cell.ValueLabel.ForegroundColor = iApp.Instance.Style.SecondarySubTextColor;
                            cell.ValueLabel.Font = Font.PreferredValueFont;
                        }
                        else
                        {
                            cell.ValueLabel.Lines = int.Parse(ExtractParameters(controlAttribute.LabelLines, cellInfo.Object));
                            cell.ValueLabel.ForegroundColor = GetColorFromString(ExtractParameters(controlAttribute.ForegroundColor, cellInfo.Object));
                            cell.ValueLabel.Font = new Font(ExtractParameters(controlAttribute.FontName, cellInfo.Object),
                                double.Parse(ExtractParameters(controlAttribute.FontSize, cellInfo.Object)), controlAttribute.FontFormatting);
                        }

                        cell.ValueLabel.Text = info.GetValue(cellInfo.Object, null) as string;
                    }
                }

                if (cellInfo.Infos.Length > 1)
                {
                    ControlAttribute controlAttribute = null;
                    foreach (var attribute in cellInfo.Attributes[1])
                    {
                        var controlAtt = attribute as ControlAttribute;
                        if (controlAtt != null)
                        {
                            controlAttribute = controlAtt;
                        }

                        var selectableAtt = attribute as SelectableAttribute;
                        if (selectableAtt != null)
                        {
                            cell.SelectionColor = GetColorFromString(ExtractParameters(selectableAtt.SelectionColor, cellInfo.Object));
                            cell.SelectionStyle = selectableAtt.SelectionStyle;

                            if (selectableAtt.NavigationAddress != null)
                            {
                                cell.NavigationLink = new Link(selectableAtt.NavigationAddress) { Action = ActionType.Submit };
                            }

                            if (selectableAtt.AccessoryAddress != null)
                            {
                                cell.AccessoryLink = new Link(selectableAtt.AccessoryAddress) { Action = ActionType.Submit };
                            }
                        }

                        var imageAtt = attribute as ImageAttribute;
                        if (imageAtt != null)
                        {
                            imagePath = imageAtt.FilePath;
                        }
                    }

                    var info = cellInfo.Infos[1];
                    if (info.
#if NETCF
GetGetMethod()
#else
                        GetMethod
#endif
 == null)
                    {
                        throw new MissingMemberException(string.Format("Missing getter for property {0}.  A getter is required for UI reflection.", info.Name));
                    }

                    if (controlAttribute == null)
                    {
                        cell.SubtextLabel.Lines = 1;
                        cell.SubtextLabel.ForegroundColor = iApp.Instance.Style.SubTextColor;
                        cell.SubtextLabel.Font = Font.PreferredSmallFont;
                    }
                    else
                    {
                        cell.SubtextLabel.Lines = int.Parse(ExtractParameters(controlAttribute.LabelLines, cellInfo.Object));
                        cell.SubtextLabel.ForegroundColor = GetColorFromString(ExtractParameters(controlAttribute.ForegroundColor, cellInfo.Object));
                        cell.SubtextLabel.Font = new Font(ExtractParameters(controlAttribute.FontName, cellInfo.Object),
                            double.Parse(ExtractParameters(controlAttribute.FontSize, cellInfo.Object)), controlAttribute.FontFormatting);
                    }

                    cell.SubtextLabel.Text = info.GetValue(cellInfo.Object, null) as string;
                }

                cell.Image.FilePath = ExtractParameters(imagePath, cellInfo.Object);
                return cell;
            }
            #endregion

            bool hasLink = cellInfo.Attributes.Any(a => a.OfType<SelectableAttribute>().Any(s => s.NavigationAddress != null));
            EventHandler selectHandler = null;

            #region HeaderedControlCell
            if (cellInfo.CellType == typeof(HeaderedControlCell))
            {
                var cell = recycledCell as HeaderedControlCell ?? new HeaderedControlCell(null);
                cell.NullifyEvents();
                cell.NavigationLink = null;
                cell.AccessoryLink = null;
                cell.SelectionStyle = SelectionStyle.Default;
                cell.SelectionColor = iApp.Instance.Style.SelectionColor;

                LabelAttribute labelAttribute = null;
                for (int i = 0; i < cellInfo.Infos.Length; i++)
                {
                    var attributes = cellInfo.Attributes[i];
                    ControlAttribute controlAttribute = null;
                    foreach (var attribute in attributes)
                    {
                        var controlAtt = attribute as ControlAttribute;
                        if (controlAtt != null)
                        {
                            controlAttribute = controlAtt;
                        }

                        var labelAtt = attribute as LabelAttribute;
                        if (labelAtt != null)
                        {
                            labelAttribute = labelAtt;
                        }

                        var selectableAtt = attribute as SelectableAttribute;
                        if (selectableAtt != null)
                        {
                            cell.SelectionColor = GetColorFromString(ExtractParameters(selectableAtt.SelectionColor, cellInfo.Object));
                            cell.SelectionStyle = selectableAtt.SelectionStyle;

                            if (selectableAtt.NavigationAddress != null)
                            {
                                cell.NavigationLink = new Link(ExtractParameters(selectableAtt.NavigationAddress, cellInfo.Object))
                                {
                                    Action = ActionType.Submit
                                };
                            }

                            if (selectableAtt.AccessoryAddress != null)
                            {
                                cell.AccessoryLink = new Link(ExtractParameters(selectableAtt.AccessoryAddress, cellInfo.Object))
                                {
                                    Action = ActionType.Submit
                                };
                            }
                        }
                    }

                    var info = cellInfo.Infos[i];
                    if (info.
#if NETCF
GetGetMethod()
#else
                        GetMethod
#endif
 == null)
                    {
                        throw new MissingMemberException(string.Format("Missing getter for property {0}.  A getter is required for UI reflection.", info.Name));
                    }

                    var value = info.GetValue(cellInfo.Object, null);

                    var controlType = GetControlType(info, controlAttribute);
                    var control = cell.GetChild<IControl>("control" + i);
                    if (control != null && control.GetType() != controlType)
                    {
                        cell.RemoveControl(control);
                        control = null;
                    }

                    if (control == null)
                    {
                        control = (IControl)Activator.CreateInstance(controlType);
                        control.ID = "control" + i;
                        cell.AddControl(control);
                    }

                    string submitKey = controlAttribute == null || !controlAttribute.IsSubmitKeySet ?
                    info.DeclaringType.Name + "." + info.Name : ExtractParameters(controlAttribute.SubmitKey, cellInfo.Object);

                    var background = new Color();
                    var foreground = new Color();
                    var font = Font.PreferredLabelFont;

                    if (controlAttribute != null)
                    {
                        background = GetColorFromString(ExtractParameters(controlAttribute.BackgroundColor, cellInfo.Object));
                        foreground = GetColorFromString(ExtractParameters(controlAttribute.ForegroundColor, cellInfo.Object));
                        font = new Font(ExtractParameters(controlAttribute.FontName, cellInfo.Object),
                            double.Parse(ExtractParameters(controlAttribute.FontSize, cellInfo.Object)), controlAttribute.FontFormatting);
                    }

                    control.SubmitKey = submitKey;

                    var button = control as IButton;
                    if (button != null)
                    {
                        button.BackgroundColor = background;
                        button.ForegroundColor = foreground;
                        button.Font = font;
                        button.HorizontalAlignment = HorizontalAlignment.Stretch;
                        button.ColumnIndex = 1;
                        button.ColumnSpan = 2;
                        button.Title = labelAttribute == null ? FormatStringForDisplay(info.Name) : ExtractParameters(labelAttribute.Text, cellInfo.Object);
                        button.NavigationLink = new Link(controlAttribute == null ? (value == null ? null : value.ToString()) :
                            ExtractParameters(controlAttribute.ButtonAddress, cellInfo.Object)) { Action = ActionType.Submit };

                        continue;
                    }

                    IValueConverter converter = null;
                    if (controlAttribute != null && controlAttribute.ValueConverterType != null)
                    {
                        converter = Activator.CreateInstance(controlAttribute.ValueConverterType) as IValueConverter;
                    }

                    var label = control as ILabel;
                    if (label != null)
                    {
                        label.ForegroundColor = foreground;
                        label.Font = font;
                        label.HorizontalAlignment = HorizontalAlignment.Right;
                        label.Lines = controlAttribute == null ? 1 : int.Parse(ExtractParameters(controlAttribute.LabelLines, cellInfo.Object));

                        control.SetBinding(new Binding("Text", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                        continue;
                    }

                    var selectList = control as ISelectList;
                    if (selectList != null)
                    {
                        selectList.BackgroundColor = background;
                        selectList.ForegroundColor = foreground;
                        selectList.Font = font;
                        selectList.HorizontalAlignment = HorizontalAlignment.Right;

                        var enumInfo = Device.Reflector.GetFields(info.PropertyType);
                        selectList.Items = enumInfo.Where(f => f != null && !f.IsSpecialName).Select(f => f.GetValue(cellInfo.Object));

                        if (!hasLink && selectHandler == null)
                        {
                            cell.SelectionStyle = SelectionStyle.None;
                            cell.SetValue("SelectionStyle", SelectionStyle.IndicatorOnly, MobileTarget.Touch);
                            selectHandler = (sender, e) => { selectList.ShowList(); };
                        }

                        control.SetBinding(new Binding("SelectedItem", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                        continue;
                    }

                    var boolSwitch = control as ISwitch;
                    if (boolSwitch != null)
                    {
                        boolSwitch.ForegroundColor = foreground;
                        boolSwitch.HorizontalAlignment = HorizontalAlignment.Right;

                        if (!hasLink && selectHandler == null)
                        {
                            selectHandler = (sender, e) => { boolSwitch.Value = !boolSwitch.Value; };
                        }

                        control.SetBinding(new Binding("Value", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                        continue;
                    }

                    var datePicker = control as IDatePicker;
                    if (datePicker != null)
                    {
                        datePicker.BackgroundColor = background;
                        datePicker.ForegroundColor = foreground;
                        datePicker.Font = font;
                        datePicker.HorizontalAlignment = HorizontalAlignment.Right;
                        datePicker.DateFormat = controlAttribute == null ? null : ExtractParameters(controlAttribute.PickerValueFormat, cellInfo.Object);

                        if (!hasLink && selectHandler == null)
                        {
                            selectHandler = (sender, e) => { datePicker.ShowPicker(); };
                        }

                        control.SetBinding(new Binding("Date", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                        continue;
                    }

                    var timePicker = control as ITimePicker;
                    if (timePicker != null)
                    {
                        timePicker.BackgroundColor = background;
                        timePicker.ForegroundColor = foreground;
                        timePicker.Font = font;
                        timePicker.HorizontalAlignment = HorizontalAlignment.Right;
                        timePicker.TimeFormat = controlAttribute == null ? null : ExtractParameters(controlAttribute.PickerValueFormat, cellInfo.Object);

                        if (!hasLink && selectHandler == null)
                        {
                            selectHandler = (sender, e) => { timePicker.ShowPicker(); };
                        }

                        control.SetBinding(new Binding("Time", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                        continue;
                    }

                    var slider = control as ISlider;
                    if (slider != null)
                    {
                        slider.HorizontalAlignment = HorizontalAlignment.Stretch;

                        if (controlAttribute == null)
                        {
                            slider.MaxValue = 100;
                            slider.MinValue = 0;
                        }
                        else
                        {
                            slider.MaxValue = double.Parse(ExtractParameters(controlAttribute.SliderMaxValue, cellInfo.Object));
                            slider.MinValue = double.Parse(ExtractParameters(controlAttribute.SliderMinValue, cellInfo.Object));
                        }

                        control.SetBinding(new Binding("Value", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                        continue;
                    }

                    var textBox = control as ITextBox;
                    if (textBox != null)
                    {
                        textBox.BackgroundColor = background;
                        textBox.ForegroundColor = foreground;
                        textBox.Font = font;
                        textBox.HorizontalAlignment = HorizontalAlignment.Stretch;

                        if (controlAttribute == null)
                        {
                            textBox.Expression = null;
                            textBox.Placeholder = null;
                            textBox.KeyboardType = KeyboardType.AlphaNumeric;
                            textBox.TextAlignment = TextAlignment.Left;
                            textBox.TextCompletion = TextCompletion.AutoCapitalize | TextCompletion.OfferSuggestions;
                        }
                        else
                        {
                            textBox.Expression = ExtractParameters(controlAttribute.TextEntryExpression, cellInfo.Object);
                            textBox.Placeholder = ExtractParameters(controlAttribute.TextEntryPlaceholder, cellInfo.Object);
                            textBox.KeyboardType = controlAttribute.TextEntryKeyboardType;
                            textBox.TextAlignment = controlAttribute.TextEntryAlignment;
                            textBox.TextCompletion = controlAttribute.TextEntryCompletion;
                        }

                        if (!hasLink && selectHandler == null)
                        {
                            selectHandler = (sender, e) => { textBox.Focus(); };
                        }

                        control.SetBinding(new Binding("Text", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                        continue;
                    }

                    var passwordBox = control as IPasswordBox;
                    if (passwordBox != null)
                    {
                        passwordBox.BackgroundColor = background;
                        passwordBox.ForegroundColor = foreground;
                        passwordBox.Font = font;
                        passwordBox.HorizontalAlignment = HorizontalAlignment.Stretch;

                        if (controlAttribute == null)
                        {
                            passwordBox.Expression = null;
                            passwordBox.KeyboardType = KeyboardType.AlphaNumeric;
                        }
                        else
                        {
                            passwordBox.Expression = ExtractParameters(controlAttribute.TextEntryExpression, cellInfo.Object);
                            passwordBox.KeyboardType = controlAttribute.TextEntryKeyboardType;
                        }

                        if (!hasLink && selectHandler == null)
                        {
                            selectHandler = (sender, e) => { passwordBox.Focus(); };
                        }

                        control.SetBinding(new Binding("Password", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                        continue;
                    }

                    var textArea = control as ITextArea;
                    if (textArea != null)
                    {
                        textArea.BackgroundColor = background;
                        textArea.ForegroundColor = foreground;
                        textArea.Font = font;
                        textArea.HorizontalAlignment = HorizontalAlignment.Stretch;

                        if (controlAttribute == null)
                        {
                            textArea.Expression = null;
                            textArea.KeyboardType = KeyboardType.AlphaNumeric;
                            textArea.TextAlignment = TextAlignment.Left;
                            textArea.TextCompletion = TextCompletion.AutoCapitalize | TextCompletion.OfferSuggestions;
                        }
                        else
                        {
                            textArea.Expression = ExtractParameters(controlAttribute.TextEntryExpression, cellInfo.Object);
                            textArea.KeyboardType = controlAttribute.TextEntryKeyboardType;
                            textArea.TextAlignment = controlAttribute.TextEntryAlignment;
                            textArea.TextCompletion = controlAttribute.TextEntryCompletion;
                        }

                        if (!hasLink && selectHandler == null)
                        {
                            selectHandler = (sender, e) => { textArea.Focus(); };
                        }

                        control.SetBinding(new Binding("Text", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                        continue;
                    }
                }

                if (labelAttribute == null)
                {
                    cell.Header.Text = cellInfo.Infos.Length > 0 ? FormatStringForDisplay(cellInfo.Infos[0].Name) : null;
                    cell.Header.Lines = 1;
                    cell.Header.ForegroundColor = iApp.Instance.Style.TextColor;
                    cell.Header.Font = Font.PreferredHeaderFont;
                }
                else
                {
                    cell.Header.Text = ExtractParameters(labelAttribute.Text, cellInfo.Object);
                    cell.Header.Lines = int.Parse(ExtractParameters(labelAttribute.Lines, cellInfo.Object));
                    cell.Header.ForegroundColor = GetColorFromString(ExtractParameters(labelAttribute.ForegroundColor, cellInfo.Object));
                    cell.Header.Font = new Font(ExtractParameters(labelAttribute.FontName, cellInfo.Object),
                            double.Parse(ExtractParameters(labelAttribute.FontSize, cellInfo.Object)), labelAttribute.FontFormatting);
                }

                return cell;
            }
            #endregion

            var grid = recycledCell as IGridCell;
            if (grid == null)
            {
                grid = new GridCell();
                grid.Columns.Add(Column.AutoSized);
                grid.Columns.Add(Column.OneStar);
                grid.Columns.Add(Column.AutoSized);
                grid.SetRows(cellInfo.Infos.Length);

                grid.MinHeight = Cell.StandardCellHeight;
            }

            grid.NullifyEvents();
            grid.NavigationLink = null;
            grid.AccessoryLink = null;
            grid.SelectionColor = new Color();
            grid.SelectionStyle = SelectionStyle.None;

            grid.Columns[1] = Column.OneStar;
            grid.Columns[2] = Column.AutoSized;

            for (int i = 0; i < cellInfo.Infos.Length; i++)
            {
                var info = cellInfo.Infos[i];
                ControlAttribute controlAttribute = null;
                LabelAttribute labelAttribute = null;
                SelectableAttribute selectAttribute = null;
                ImageAttribute imageAttribute = null;
                object value = null;

                foreach (var attribute in cellInfo.Attributes[i])
                {
                    var controlAtt = attribute as ControlAttribute;
                    if (controlAtt != null)
                    {
                        controlAttribute = controlAtt;
                    }

                    var labelAtt = attribute as LabelAttribute;
                    if (labelAtt != null)
                    {
                        labelAttribute = labelAtt;
                    }

                    var selectableAtt = attribute as SelectableAttribute;
                    if (selectableAtt != null)
                    {
                        selectAttribute = selectableAtt;
                    }

                    var imageAtt = attribute as ImageAttribute;
                    if (imageAtt != null)
                    {
                        imageAttribute = imageAtt;
                    }
                }

                var image = grid.GetChild<Image>("icon" + i);
                if (imageAttribute == null)
                {
                    if (image != null)
                    {
                        grid.RemoveChild(image);
                    }
                }
                else
                {
                    if (image == null)
                    {
                        image = new Image();
                        image.ID = "icon" + i;
                        grid.AddChild(image);
                    }

                    image.ColumnIndex = 0;
                    image.Margin = new Thickness(0, 0, Thickness.LargeHorizontalSpacing, 0);
                    image.RowIndex = i;
                    image.FilePath = ExtractParameters(imageAttribute.FilePath, cellInfo.Object);
                }

                if (info.
#if NETCF
GetGetMethod()
#else
                    GetMethod
#endif
 == null)
                {
                    throw new MissingMemberException(string.Format("Missing getter for property {0}.  A getter is required for UI reflection.", info.Name));
                }

                value = info.GetValue(cellInfo.Object, null);

                var controlType = GetControlType(info, controlAttribute);
                var infoType = info.PropertyType;

                if (selectAttribute != null)
                {
                    if (selectAttribute.NavigationAddress != null)
                    {
                        grid.NavigationLink = new Link(ExtractParameters(selectAttribute.NavigationAddress, cellInfo.Object)) { Action = ActionType.Submit };
                    }

                    if (selectAttribute.AccessoryAddress != null)
                    {
                        grid.AccessoryLink = new Link(ExtractParameters(selectAttribute.AccessoryAddress, cellInfo.Object)) { Action = ActionType.Submit };
                    }

                    grid.SelectionColor = GetColorFromString(ExtractParameters(selectAttribute.SelectionColor, cellInfo.Object));
                    grid.SelectionStyle = selectAttribute.SelectionStyle;
                }

                bool soloLabel = (controlType == typeof(Label) && (labelAttribute != null && labelAttribute.Text == null));

                string submitKey = controlAttribute == null || !controlAttribute.IsSubmitKeySet ?
                    info.DeclaringType.Name + "." + info.Name : ExtractParameters(controlAttribute.SubmitKey, cellInfo.Object);

                var background = new Color();
                var foreground = new Color();
                var font = Font.PreferredLabelFont;

                if (controlAttribute != null)
                {
                    background = GetColorFromString(ExtractParameters(controlAttribute.BackgroundColor, cellInfo.Object));
                    foreground = GetColorFromString(ExtractParameters(controlAttribute.ForegroundColor, cellInfo.Object));
                    font = new Font(ExtractParameters(controlAttribute.FontName, cellInfo.Object),
                        double.Parse(ExtractParameters(controlAttribute.FontSize, cellInfo.Object)), controlAttribute.FontFormatting);
                }

                var header = grid.GetChild<Label>("header" + i);
                if (controlType == typeof(Button))
                {
                    if (header != null)
                    {
                        grid.RemoveChild(header);
                    }
                }
                else
                {
                    if (header == null)
                    {
                        header = new Label();
                        header.ID = "header" + i;
                        grid.AddChild(header);
                    }

                    header.RowIndex = i;
                    header.ColumnIndex = 1;
                    header.ColumnSpan = soloLabel ? 2 : 1;
                    header.VerticalAlignment = VerticalAlignment.Center;
                    header.Margin = new Thickness(0, 0, soloLabel ? 0 : Thickness.LargeHorizontalSpacing, 0);
                    header.SubmitKey = soloLabel ? submitKey : null;

                    if (soloLabel || labelAttribute == null)
                    {
                        header.Text = soloLabel ? (value == null ? null : value.ToString()) : FormatStringForDisplay(info.Name);
                        header.Font = font;
                        header.ForegroundColor = soloLabel ? foreground : iApp.Instance.Style.TextColor;
                        header.Lines = soloLabel ? 0 : 1;
                    }
                    else
                    {
                        header.Text = ExtractParameters(labelAttribute.Text, cellInfo.Object);
                        header.Font = new Font(labelAttribute.FontName, double.Parse(ExtractParameters(labelAttribute.FontSize, cellInfo.Object)), labelAttribute.FontFormatting);
                        header.ForegroundColor = GetColorFromString(ExtractParameters(labelAttribute.ForegroundColor, cellInfo.Object));
                        header.Lines = int.Parse(ExtractParameters(labelAttribute.Lines, cellInfo.Object));
                    }
                }

                var control = grid.GetChild<IControl>("control" + i);
                if (control != null && control.GetType() != controlType)
                {
                    grid.RemoveChild(control);
                    control = null;
                }

                if (soloLabel)
                {
                    if (control != null)
                    {
                        grid.RemoveChild(control);
                    }
                    continue;
                }

                if (control == null)
                {
                    control = (IControl)Activator.CreateInstance(controlType);
                    control.ID = "control" + i;
                    grid.AddChild(control);
                }

                control.RowIndex = i;
                control.ColumnIndex = soloLabel ? 1 : 2;
                control.ColumnSpan = soloLabel ? 2 : 1;
                control.VerticalAlignment = VerticalAlignment.Center;
                control.SubmitKey = submitKey;

                var button = control as IButton;
                if (button != null)
                {
                    button.BackgroundColor = background;
                    button.ForegroundColor = foreground;
                    button.Font = font;
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.ColumnIndex = 1;
                    button.ColumnSpan = 2;
                    button.Title = labelAttribute == null ? FormatStringForDisplay(info.Name) : ExtractParameters(labelAttribute.Text, cellInfo.Object);
                    button.NavigationLink = new Link(controlAttribute == null ? (value == null ? null : value.ToString()) :
                        ExtractParameters(controlAttribute.ButtonAddress, cellInfo.Object)) { Action = ActionType.Submit };

                    continue;
                }

                IValueConverter converter = null;
                if (controlAttribute != null && controlAttribute.ValueConverterType != null)
                {
                    converter = Activator.CreateInstance(controlAttribute.ValueConverterType) as IValueConverter;
                }

                var label = control as ILabel;
                if (label != null)
                {
                    label.ForegroundColor = foreground;
                    label.Font = font;
                    label.HorizontalAlignment = HorizontalAlignment.Right;
                    label.Lines = controlAttribute == null ? 1 : int.Parse(ExtractParameters(controlAttribute.LabelLines, cellInfo.Object));

                    control.SetBinding(new Binding("Text", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                    continue;
                }

                var selectList = control as ISelectList;
                if (selectList != null)
                {
                    selectList.BackgroundColor = background;
                    selectList.ForegroundColor = foreground;
                    selectList.Font = font;
                    selectList.HorizontalAlignment = HorizontalAlignment.Right;

                    var enumInfo = Device.Reflector.GetFields(infoType);
                    selectList.Items = enumInfo.Where(f => f != null && !f.IsSpecialName).Select(f => f.GetValue(cellInfo.Object));

                    if (!hasLink && selectHandler == null)
                    {
                        grid.SelectionStyle = SelectionStyle.None;
                        grid.SetValue("SelectionStyle", SelectionStyle.IndicatorOnly, MobileTarget.Touch);
                        selectHandler = (sender, e) => { selectList.ShowList(); };
                    }

                    control.SetBinding(new Binding("SelectedItem", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                    continue;
                }

                var boolSwitch = control as ISwitch;
                if (boolSwitch != null)
                {
                    boolSwitch.ForegroundColor = foreground;
                    boolSwitch.HorizontalAlignment = HorizontalAlignment.Right;

                    if (!hasLink && selectHandler == null)
                    {
                        selectHandler = (sender, e) => { boolSwitch.Value = !boolSwitch.Value; };
                    }

                    control.SetBinding(new Binding("Value", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                    continue;
                }

                var datePicker = control as IDatePicker;
                if (datePicker != null)
                {
                    datePicker.BackgroundColor = background;
                    datePicker.ForegroundColor = foreground;
                    datePicker.Font = font;
                    datePicker.HorizontalAlignment = HorizontalAlignment.Right;
                    datePicker.DateFormat = controlAttribute == null ? null : ExtractParameters(controlAttribute.PickerValueFormat, cellInfo.Object);

                    if (!hasLink && selectHandler == null)
                    {
                        selectHandler = (sender, e) => { datePicker.ShowPicker(); };
                    }

                    control.SetBinding(new Binding("Date", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                    continue;
                }

                var timePicker = control as ITimePicker;
                if (timePicker != null)
                {
                    timePicker.BackgroundColor = background;
                    timePicker.ForegroundColor = foreground;
                    timePicker.Font = font;
                    timePicker.HorizontalAlignment = HorizontalAlignment.Right;
                    timePicker.TimeFormat = controlAttribute == null ? null : ExtractParameters(controlAttribute.PickerValueFormat, cellInfo.Object);

                    if (!hasLink && selectHandler == null)
                    {
                        selectHandler = (sender, e) => { timePicker.ShowPicker(); };
                    }

                    control.SetBinding(new Binding("Time", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                    continue;
                }

                grid.Columns[1] = Column.AutoSized;
                grid.Columns[2] = Column.OneStar;

                var slider = control as ISlider;
                if (slider != null)
                {
                    slider.HorizontalAlignment = HorizontalAlignment.Stretch;

                    if (controlAttribute == null)
                    {
                        slider.MaxValue = 100;
                        slider.MinValue = 0;
                    }
                    else
                    {
                        slider.MaxValue = double.Parse(ExtractParameters(controlAttribute.SliderMaxValue, cellInfo.Object));
                        slider.MinValue = double.Parse(ExtractParameters(controlAttribute.SliderMinValue, cellInfo.Object));
                    }

                    control.SetBinding(new Binding("Value", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                    continue;
                }

                var textBox = control as ITextBox;
                if (textBox != null)
                {
                    textBox.BackgroundColor = background;
                    textBox.ForegroundColor = foreground;
                    textBox.Font = font;
                    textBox.HorizontalAlignment = HorizontalAlignment.Stretch;

                    if (controlAttribute == null)
                    {
                        textBox.Expression = null;
                        textBox.Placeholder = null;
                        textBox.KeyboardType = KeyboardType.AlphaNumeric;
                        textBox.TextAlignment = TextAlignment.Left;
                        textBox.TextCompletion = TextCompletion.AutoCapitalize | TextCompletion.OfferSuggestions;
                    }
                    else
                    {
                        textBox.Expression = ExtractParameters(controlAttribute.TextEntryExpression, cellInfo.Object);
                        textBox.Placeholder = ExtractParameters(controlAttribute.TextEntryPlaceholder, cellInfo.Object);
                        textBox.KeyboardType = controlAttribute.TextEntryKeyboardType;
                        textBox.TextAlignment = controlAttribute.TextEntryAlignment;
                        textBox.TextCompletion = controlAttribute.TextEntryCompletion;
                    }

                    if (!hasLink && selectHandler == null)
                    {
                        selectHandler = (sender, e) => { textBox.Focus(); };
                    }

                    control.SetBinding(new Binding("Text", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                    continue;
                }

                var passwordBox = control as IPasswordBox;
                if (passwordBox != null)
                {
                    passwordBox.BackgroundColor = background;
                    passwordBox.ForegroundColor = foreground;
                    passwordBox.Font = font;
                    passwordBox.HorizontalAlignment = HorizontalAlignment.Stretch;

                    if (controlAttribute == null)
                    {
                        passwordBox.Expression = null;
                        passwordBox.KeyboardType = KeyboardType.AlphaNumeric;
                    }
                    else
                    {
                        passwordBox.Expression = ExtractParameters(controlAttribute.TextEntryExpression, cellInfo.Object);
                        passwordBox.KeyboardType = controlAttribute.TextEntryKeyboardType;
                    }

                    if (!hasLink && selectHandler == null)
                    {
                        selectHandler = (sender, e) => { passwordBox.Focus(); };
                    }

                    control.SetBinding(new Binding("Password", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                    continue;
                }

                var textArea = control as ITextArea;
                if (textArea != null)
                {
                    textArea.BackgroundColor = background;
                    textArea.ForegroundColor = foreground;
                    textArea.Font = font;
                    textArea.HorizontalAlignment = HorizontalAlignment.Stretch;

                    if (controlAttribute == null)
                    {
                        textArea.Expression = null;
                        textArea.KeyboardType = KeyboardType.AlphaNumeric;
                        textArea.TextAlignment = TextAlignment.Left;
                        textArea.TextCompletion = TextCompletion.AutoCapitalize | TextCompletion.OfferSuggestions;
                    }
                    else
                    {
                        textArea.Expression = ExtractParameters(controlAttribute.TextEntryExpression, cellInfo.Object);
                        textArea.KeyboardType = controlAttribute.TextEntryKeyboardType;
                        textArea.TextAlignment = controlAttribute.TextEntryAlignment;
                        textArea.TextCompletion = controlAttribute.TextEntryCompletion;
                    }

                    if (!hasLink && selectHandler == null)
                    {
                        selectHandler = (sender, e) => { textArea.Focus(); };
                    }

                    control.SetBinding(new Binding("Text", info.Name) { Source = cellInfo.Object, Mode = BindingMode.TwoWay, ValueConverter = converter });
                    continue;
                }
            }

            if (selectHandler != null)
            {
                grid.Selected += selectHandler;
            }

            return grid;
        }

        private static void ProcessObjectForTabs(object obj, ReflectiveParameters parameters, ICollection collection)
        {
            var objType = obj.GetType();

            var attributes = Device.Reflector.GetCustomAttributes(objType, false);
            if (attributes.Any(a => a is SkipAttribute))
            {
                return;
            }

            var memparAtt = (MemberParticipationAttribute)attributes.FirstOrDefault(a => a is MemberParticipationAttribute);
            parameters.Participation = memparAtt == null ? Participation.OptOut : memparAtt.Participation;

            var orders = new List<Order>();
            var propertyInfos = Device.Reflector.GetProperties(objType).Where(p => p.DeclaringType == obj.GetType() && p.Name != "Item");
            foreach (var info in propertyInfos)
            {
                attributes = info.GetCustomAttributes(false).Cast<Attribute>();
                bool optedOut = attributes.Any(a => a is SkipAttribute);
                bool optedIn = attributes.Any(a => (a is OptInAttribute));
                if (!optedOut && !optedIn)
                {
                    var col = info.GetValue(obj, null) as ICollection;
                    if (col != null)
                    {
                        var orderAtt = (OrderAttribute)attributes.FirstOrDefault(a => a is OrderAttribute);
                        orders.Add(new Order() { Collection = col, Index = orderAtt == null ? (int?)null : orderAtt.Index });
                        continue;
                    }
                }

                if ((parameters.Participation == Participation.OptOut && optedOut)
                    || (parameters.Participation == Participation.OptIn && !optedIn))
                {
                    continue;
                }

                {
                    var orderAtt = (OrderAttribute)attributes.FirstOrDefault(a => a is OrderAttribute);
                    orders.Add(new Order()
                    {
                        Info = info,
                        Index = orderAtt == null ? (int?)null : orderAtt.Index,
                        Attributes = attributes.Where(a =>
                            a is ControlAttribute || a is LabelAttribute || a is ImageAttribute || a is SelectableAttribute
                        ).ToArray()
                    });
                }
            }

            var ordered = orders.OrderBy(o => (o.Index == null ? long.MaxValue : o.Index.Value));
            for (int i = 0; i < (collection == null ? 1 : collection.Count); i++)
            {
                foreach (var order in ordered)
                {
                    if (order.Collection != null)
                    {
                        if (order.Collection.Count > 0)
                        {
                            parameters.Participation = Participation.OptOut;
                            ProcessObjectForTabs(order.Collection.ElementAt(0), parameters, order.Collection);
                        }
                        continue;
                    }

                    parameters.Sections[0].Items.Add(new Item()
                    {
                        Object = collection == null ? obj : collection.ElementAt(i),
                        Infos = new[] { order.Info },
                        Attributes = new[] { attributes.ToArray() }
                    });
                }
            }
        }

        private static Type GetCellType(PropertyInfo[] infos, Attribute[][] attributes)
        {
            if (infos.Length >= 3)
            {
                bool hasLabel = false;
                for (int i = 0; i < infos.Length; i++)
                {
                    if (attributes[i].Any(a => a is ImageAttribute))
                    {
                        return typeof(IGridCell);
                    }

                    if (HasLabel(attributes[i]))
                    {
                        if (hasLabel)
                        {
                            return typeof(IGridCell);
                        }

                        hasLabel = true;
                    }
                }

                return typeof(HeaderedControlCell);
            }

            bool hasImage = false;
            if (infos.Length > 1)
            {
                ControlAttribute controlAtt = null;
                var atts = attributes[1];
                for (int i = 0; i < atts.Length; i++)
                {
                    var att = atts[i];

                    var control = att as ControlAttribute;
                    if (control != null)
                    {
                        controlAtt = control;
                        continue;
                    }

                    var image = att as ImageAttribute;
                    if (image != null && !string.IsNullOrEmpty(image.FilePath))
                    {
                        hasImage = true;
                    }
                }

                if (GetControlType(infos[1], controlAtt) == typeof(ILabel))
                {
                    if (HasLabel(attributes[1]))
                    {
                        if (attributes.Any(a => a.Any(att => att is ImageAttribute)))
                        {
                            return typeof(IGridCell);
                        }

                        return HasLabel(attributes[0]) ? typeof(IGridCell) : typeof(HeaderedControlCell);
                    }
                }
                else
                {
                    if (attributes.Any(a => a.Any(att => att is ImageAttribute)))
                    {
                        return typeof(IGridCell);
                    }

                    int headerCount = (HasLabel(attributes[1]) ? 1 : 0) + (HasLabel(attributes[0]) ? 1 : 0);
                    return headerCount > 1 ? typeof(IGridCell) : typeof(HeaderedControlCell);
                }
            }

            if (infos.Length > 0)
            {
                ControlAttribute controlAtt = null;
                var atts = attributes[0];
                for (int i = 0; i < atts.Length; i++)
                {
                    var att = atts[i];

                    var control = att as ControlAttribute;
                    if (control != null)
                    {
                        controlAtt = control;
                        continue;
                    }

                    var image = att as ImageAttribute;
                    if (image != null && !string.IsNullOrEmpty(image.FilePath) && hasImage)
                    {
                        return typeof(IGridCell);
                    }
                }

                if (GetControlType(infos[0], controlAtt) != typeof(ILabel))
                {
                    if (attributes.Any(a => a.Any(att => att is ImageAttribute)))
                    {
                        return typeof(IGridCell);
                    }

                    int headerCount = (attributes.Length > 1 ? HasLabel(attributes[1]) ? 1 : 0 : 0) + (HasLabel(attributes[0]) ? 1 : 0);
                    return headerCount > 1 ? typeof(IGridCell) : typeof(HeaderedControlCell);
                }
            }

            return typeof(ContentCell);
        }

        private static Type GetControlType(PropertyInfo info, ControlAttribute controlAttribute)
        {
            if (controlAttribute == null)
            {
                var infoType = Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType;

                if (info.
#if NETCF
GetSetMethod()
#else
                    SetMethod
#endif
 == null)
                {
                    return typeof(Label);
                }
                else if (Device.Reflector.IsEnum(infoType))
                {
                    return typeof(SelectList);
                }
                else if (infoType == typeof(bool))
                {
                    return typeof(Switch);
                }
                else if (infoType == typeof(DateTime))
                {
                    return typeof(DatePicker);
                }
                else if (infoType == typeof(TimeSpan))
                {
                    return typeof(TimePicker);
                }
                else if (Device.Reflector.IsPrimitive(infoType) && infoType != typeof(char))
                {
                    return typeof(Slider);
                }
                else
                {
                    return typeof(Label);
                }
            }

            return Type.GetType("iFactr.UI.Controls." + controlAttribute.Type.ToString());
        }

        private static bool HasLabel(Attribute[] attributes)
        {
            var label = attributes.OfType<LabelAttribute>().FirstOrDefault();
            return label != null && !string.IsNullOrEmpty(label.Text);
        }

        private static string ExtractParameters(string value, object obj)
        {
            if (value == null)
            {
                return null;
            }

            string originalValue = value;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                if (value[i] == '{')
                {
                    if (i > 0 && value[i - 1] == '{')
                    {
                        value = value.Remove(i--, 1);
                        continue;
                    }
                    else
                    {
                        throw new FormatException(string.Format("Invalid parameter declaration in string '{0}'.", originalValue));
                    }
                }

                if (value[i] == '}')
                {
                    if (i > 0 && value[i - 1] == '}')
                    {
                        value = value.Remove(i--, 1);
                        continue;
                    }

                    int opener = value.LastIndexOf('{', i - 1);
                    if (opener < 0)
                    {
                        throw new FormatException(string.Format("Invalid parameter declaration in string '{0}'.", originalValue));
                    }

                    var tokens = value.Substring(opener + 1, (i - opener) - 1).Split('.');
                    for (int j = 0; j < tokens.Length; j++)
                    {
                        var objType = obj.GetType();
                        string token = tokens[j];
                        var propertyInfo = Device.Reflector.GetProperty(objType, token);
                        if (propertyInfo == null)
                        {
                            throw new ArgumentException(string.Format("No property with the name {0} was found for the type {1}.", token, objType.FullName));
                        }
                        else
                        {
                            obj = propertyInfo.GetValue(obj, null);
                        }
                    }

                    value = value.Remove(opener, (i - opener) + 1);
                    if (obj != null)
                    {
                        value = value.Insert(opener, obj.ToString());
                    }

                    i = opener;
                }
            }

            return value;
        }

        private static string FormatStringForDisplay(string value)
        {
            if (value == null)
            {
                return null;
            }

            for (int i = value.Length - 1; i > 0 /*skip zero*/; i--)
            {
                char c = value[i];
                if (c == '_')
                {
                    value = value.Remove(i, 1);
                }
                else if (char.IsUpper(c))
                {
                    value = value.Insert(i, " ");
                }
                else if (char.IsNumber(c))
                {
                    for (i -= 1; i >= 0; i--)
                    {
                        if (char.IsLetter(value[i]))
                        {
                            value = value.Insert(++i, " ");
                            break;
                        }
                    }
                }
            }

            return value;
        }

        private static Color GetColorFromString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new Color();
            }

            if (value[0] == '#')
            {
                return new Color(value);
            }

            var propertyInfo = Device.Reflector.GetProperty(typeof(Color), value);
            if (propertyInfo != null)
            {
                return (Color)propertyInfo.GetValue(null, null);
            }

            return new Color(value);
        }

        private class ReflectiveSection : Section
        {
            public List<Item> Items { get; private set; }

            internal ReflectiveSection()
            {
                Items = new List<Item>();
            }

            internal ReflectiveSection Clone()
            {
                var section = (ReflectiveSection)base.MemberwiseClone();
                section.Items = new List<Item>(); // don't copy list

                if (Header != null)
                {
                    section.Header = new SectionHeader();
                    section.Header.Text = Header.Text;
                    section.Header.Font = Header.Font;
                    section.Header.BackgroundColor = Header.BackgroundColor;
                    section.Header.ForegroundColor = Header.ForegroundColor;
                }

                if (Footer != null)
                {
                    section.Footer = new SectionFooter();
                    section.Footer.Text = Footer.Text;
                    section.Footer.Font = Footer.Font;
                    section.Footer.BackgroundColor = Footer.BackgroundColor;
                    section.Footer.ForegroundColor = Footer.ForegroundColor;
                }

                return section;
            }
        }

        private class ReflectiveParameters
        {
            public Participation Participation;
            public List<ReflectiveSection> Sections;

            internal ReflectiveParameters()
            {
                Participation = Participation.OptOut;
                Sections = new List<ReflectiveSection>();
            }

            private class GroupComparer : IEqualityComparer<Group>
            {
                public bool Equals(Group x, Group y)
                {
                    return x.Id != null && x.Id == y.Id;
                }

                public int GetHashCode(Group obj)
                {
                    return obj.Id == null ? -1 : obj.Id.GetHashCode();
                }
            }
        }

        private class Group
        {
            public string Id;
            public long Order = long.MaxValue;
            public ReflectiveSection Section;
            public List<Order> Infos;
            public List<ICollection> Collection;
            public Type CellType;

            internal Group()
            {
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is Group)
                {
                    var group = (Group)obj;
                    return group.Id != null && group.Id == Id;
                }
                return false;
            }
        }

        private struct Order
        {
            public int? Index;
            public PropertyInfo Info;
            public Attribute[] Attributes;
            public ICollection Collection;
        }

        private class Item
        {
            public object Object { get; internal set; }
            public PropertyInfo[] Infos { get; internal set; }
            public Attribute[][] Attributes { get; internal set; }
            public Type CellType { get; internal set; }
        }
    }
}