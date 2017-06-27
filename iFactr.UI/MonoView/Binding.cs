using MonoCross.Utilities;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using MonoCross.Utilities.Threading;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a connection between two properties.  When the value of one
    /// property changes, the value of the other is automatically updated.
    /// </summary>
    public class Binding : IDisposable
    {
        /// <summary>
        /// Gets or sets the object that contains the bound property specified by the <see cref="P:SourcePath"/>.
        /// If set to <c>null</c>, the target of the binding will act as the source.
        /// </summary>
        public object Source
        {
            get { return source; }
            set
            {
                if (value != source)
                {
                    source = value;
                    if (isActive)
                    {
                        // reactivate
                        Deactivate();
                        Activate();
                    }
                }
            }
        }
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private object source;

        /// <summary>
        /// Gets the property path for the source object.
        /// </summary>
        public string SourcePath { get; private set; }

        /// <summary>
        /// Gets the property path for the target object.
        /// </summary>
        public string TargetPath { get; private set; }

        /// <summary>
        /// Gets or sets the converter to use when passing values between bound objects.
        /// </summary>
        public IValueConverter ValueConverter { get; set; }

        /// <summary>
        /// Gets or sets an optional parameter to pass to the value converter.
        /// </summary>
        public object ValueConverterParameter { get; set; }

        /// <summary>
        /// Gets or sets the direction in which values are passed.
        /// </summary>
        public BindingMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the target object for the binding.
        /// </summary>
        internal object Target
        {
            get { return target == null ? null : target.Target; }
            set { target = (value == null ? null : new WeakReference(value)); }
        }
        private WeakReference target;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private PropertyInfo[] sourceInfos, targetInfos;
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private object[] sourceObjects;
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private WeakReference[] targetObjects;
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private object[][] sourceIndices, targetIndices;
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private bool isActive;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly SynchronizationContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Binding"/> class.
        /// </summary>
        /// <param name="targetPath">A tokenized path that points to the target property to be bound.</param>
        /// <param name="sourcePath">A tokenized path that points to the source property to be bound.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="targetPath"/> is <c>null</c> -or- when the <paramref name="sourcePath"/> is <c>null</c>.</exception>
        public Binding(string targetPath, string sourcePath)
        {
            if (targetPath == null)
            {
                throw new ArgumentNullException("targetPath");
            }

            if (sourcePath == null)
            {
                throw new ArgumentNullException("sourcePath");
            }

            TargetPath = targetPath;
            SourcePath = sourcePath;
            Mode = BindingMode.OneWayToTarget;

            context = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(context);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Deactivate();
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Binding"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            int targetHash = Target == null ? 0 : Target.GetHashCode();
            int sourceHash = Source == null ? 0 : Source.GetHashCode();
            return targetHash ^ TargetPath.GetHashCode() ^ sourceHash ^ SourcePath.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Binding"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Binding"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Binding"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var binding = obj as Binding;
            if (obj == null)
            {
                return base.Equals(obj);
            }

            return binding.Target == Target && binding.TargetPath == TargetPath && binding.SourcePath == SourcePath &&
                (binding.Source == Source || (binding.Source == null && binding.Target == Source) || (Source == null && binding.Source == Target));
        }

        internal void Activate()
        {
            var sourceObject = GetNotifier(Source ?? Target);
            var type = sourceObject.GetType();
            var sourceTokens = SourcePath.Split('.');
            sourceInfos = new PropertyInfo[sourceTokens.Length];
            sourceObjects = new object[sourceTokens.Length];
            sourceIndices = new object[sourceTokens.Length][];
            for (int i = 0; i < sourceTokens.Length; i++)
            {
                string token = sourceTokens[i];

                if (token[token.Length - 1] == ']')
                {
                    int start = token.IndexOf('[') + 1;
                    sourceIndices[i] = ParseIndex(token.Substring(start, token.Length - start - 1));
                    token = token.Substring(0, start - 1);
                }

                var props = Device.Reflector.GetProperties(type).Where(p => p.Name == token);
                var sourceInfo = props.FirstOrDefault(p => p.DeclaringType == type) ?? props.FirstOrDefault();
                if (sourceInfo != null && sourceIndices[i] != null)
                {
                    sourceObject = sourceInfo.GetValue(sourceObject, null);
                    type = sourceObject.GetType();
                    if (!type.IsArray)
                    {
                        sourceInfo = Device.Reflector.GetProperty(type, "Item");
                    }
                }

                if (sourceInfo == null)
                {
                    var pairable = sourceObject as IPairable;
                    if (pairable != null && pairable.Pair != null)
                    {
                        sourceObject = pairable.Pair;
                        type = sourceObject.GetType();
                        props = Device.Reflector.GetProperties(type).Where(p => p.Name == token);
                        sourceInfo = props.FirstOrDefault(p => p.DeclaringType == type) ?? props.FirstOrDefault();
                        if (sourceInfo != null && sourceIndices[i] != null)
                        {
                            sourceObject = sourceInfo.GetValue(sourceObject, null);
                            type = sourceObject.GetType();
                            if (!type.IsArray)
                            {
                                sourceInfo = Device.Reflector.GetProperty(type, "Item");
                            }
                        }
                    }
                }

                if (sourceInfo == null)
                {
                    throw new ArgumentException(string.Format("Could not find a property with the name '{0}'.", token));
                }

                if (i < sourceTokens.Length - 1)
                {
                    var notify = sourceObject as INotifyPropertyChanged;
                    if (notify != null)
                    {
                        RemoveSourceHandlers(notify);
                        notify.PropertyChanged += OnSourceMidPropertyChanged;
                    }
                }

                sourceInfos[i] = sourceInfo;
                sourceObjects[i] = sourceObject;
                if (type.IsArray)
                {
                    type = sourceObject.GetType().GetElementType();
                    sourceObject = ((Array)sourceObject).GetValue(sourceIndices[i].Cast<int>().ToArray());
                }
                else
                {
                    type = sourceInfo.PropertyType;
                    sourceObject = sourceInfo.GetValue(sourceObject, sourceIndices[i]);
                }

                sourceObject = GetNotifier(sourceObject);
                if (sourceObject != null)
                {
                    type = sourceObject.GetType();
                }
            }

            var notifier = sourceObjects.Last() as INotifyPropertyChanged;
            if (notifier != null)
            {
                RemoveSourceHandlers(notifier);
                notifier.PropertyChanged += OnSourceEndPropertyChanged;
            }

            var targetObject = GetNotifier(Target);
            type = targetObject.GetType();
            var targetTokens = TargetPath.Split('.');
            targetInfos = new PropertyInfo[targetTokens.Length];
            targetObjects = new WeakReference[targetTokens.Length];
            targetIndices = new object[targetTokens.Length][];
            for (int i = 0; i < targetTokens.Length; i++)
            {
                string token = targetTokens[i];

                if (token[token.Length - 1] == ']')
                {
                    int start = token.IndexOf('[') + 1;
                    targetIndices[i] = ParseIndex(token.Substring(start, token.Length - start - 1));
                    token = token.Substring(0, start - 1);
                }

                var props = Device.Reflector.GetProperties(type).Where(p => p.Name == token);
                var targetInfo = props.FirstOrDefault(p => p.DeclaringType == type) ?? props.FirstOrDefault();
                if (targetInfo != null && targetIndices[i] != null)
                {
                    targetObject = targetInfo.GetValue(targetObject, null);
                    type = targetObject.GetType();
                    if (!type.IsArray)
                    {
                        targetInfo = Device.Reflector.GetProperty(type, "Item");
                    }
                }

                if (targetInfo == null)
                {
                    var pairable = targetObject as IPairable;
                    if (pairable != null && pairable.Pair != null)
                    {
                        targetObject = pairable.Pair;
                        type = targetObject.GetType();
                        props = Device.Reflector.GetProperties(type).Where(p => p.Name == token);
                        targetInfo = props.FirstOrDefault(p => p.DeclaringType == type) ?? props.FirstOrDefault();
                        if (targetInfo != null && targetIndices[i] != null)
                        {
                            targetObject = targetInfo.GetValue(targetObject, null);
                            type = targetObject.GetType();
                            if (!type.IsArray)
                            {
                                targetInfo = Device.Reflector.GetProperty(type, "Item");
                            }
                        }
                    }
                }

                if (targetInfo == null)
                {
                    throw new ArgumentException(string.Format("Could not find a property with the name '{0}'.", token));
                }

                if (i < targetTokens.Length - 1)
                {
                    var notify = targetObject as INotifyPropertyChanged;
                    if (notify != null)
                    {
                        RemoveTargetHandlers(notify);
                        notify.PropertyChanged += OnTargetMidPropertyChanged;
                    }
                }

                targetInfos[i] = targetInfo;
                targetObjects[i] = new WeakReference(targetObject);
                if (type.IsArray)
                {
                    type = targetObject.GetType().GetElementType();
                    targetObject = ((Array)targetObject).GetValue(targetIndices[i].Cast<int>().ToArray());
                }
                else
                {
                    type = targetInfo.PropertyType;
                    targetObject = targetInfo.GetValue(targetObject, targetIndices[i]);
                }

                targetObject = GetNotifier(targetObject);
                if (targetObject != null)
                {
                    type = targetObject.GetType();
                }
            }

            notifier = targetObjects.Last().Target as INotifyPropertyChanged;
            if (notifier != null)
            {
                RemoveTargetHandlers(notifier);
                notifier.PropertyChanged += OnTargetEndPropertyChanged;
            }

            if (Mode == BindingMode.OneWayToSource)
            {
                OnTargetEndPropertyChanged(this, new PropertyChangedEventArgs(targetInfos.Last().Name));
            }
            else
            {
                OnSourceEndPropertyChanged(this, new PropertyChangedEventArgs(sourceInfos.Last().Name));
            }

            isActive = true;
        }

        internal void Deactivate()
        {
            isActive = false;

            if (targetObjects != null)
            {
                foreach (var obj in targetObjects)
                {
                    var notifier = obj.Target as INotifyPropertyChanged;
                    if (notifier != null)
                    {
                        RemoveSourceHandlers(notifier);
                        RemoveTargetHandlers(notifier);
                    }
                }
            }

            if (sourceObjects != null)
            {
                foreach (var obj in sourceObjects)
                {
                    var notifier = obj as INotifyPropertyChanged;
                    if (notifier != null)
                    {
                        RemoveSourceHandlers(notifier);
                        RemoveTargetHandlers(notifier);
                    }
                }
            }
        }

        internal static object GetConvertedValue(object value, Type type)
        {
            if (value == null)
            {
                return Device.Reflector.IsValueType(type) ? Activator.CreateInstance(type) : null;
            }

            var valueType = value.GetType();
            if (Device.Reflector.IsAssignableFrom(type, valueType))
            {
                return value;
            }

            var targetType = Nullable.GetUnderlyingType(type) ?? type;
            valueType = Nullable.GetUnderlyingType(valueType) ?? valueType;
            if (valueType == typeof(TimeSpan) && targetType == typeof(DateTime))
            {
                return DateTime.Today + (TimeSpan)value;
            }

            if (valueType == typeof(DateTime) && targetType == typeof(TimeSpan))
            {
                return ((DateTime)value).TimeOfDay;
            }

            try
            {
                if (Device.Reflector.IsEnum(targetType))
                {
                    return Enum.Parse(targetType, value.ToString(), false);
                }
                return Convert.ChangeType(value, targetType, CultureInfo.CurrentUICulture);
            }
            catch
            {
                return Device.Reflector.IsValueType(type) ? Activator.CreateInstance(type) : null;
            }
        }

        private void OnSourceEndPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (context != null && SynchronizationContext.Current != context)
            {
                context.Post((o) =>
                {
                    SynchronizationContext.SetSynchronizationContext(context);
                    OnSourceEndPropertyChanged(sender, e);
                }, this);
                return;
            }

            if (Mode == BindingMode.OneWayToSource)
            {
                return;
            }

            var sourceToken = SourcePath.Substring(SourcePath.LastIndexOf('.') + 1);
            if (e.PropertyName != sourceToken)
            {
                return;
            }

            var targetObject = targetObjects.Last().Target;
            if (targetObject == null)
            {
                Deactivate();
                return;
            }

            var targetType = targetObject.GetType();
            var targetInfo = targetInfos.Last();
            var sourceObject = sourceObjects.Last();

            object value = null;
            if (sourceObject.GetType().IsArray)
            {
                value = ((Array)sourceObject).GetValue(sourceIndices.Last().Cast<int>().ToArray());
            }
            else
            {
                value = sourceInfos.Last().GetValue(sourceObject, sourceIndices.Last());
            }

            if (ValueConverter != null)
            {
                value = ValueConverter.Convert(value, targetInfo.PropertyType, ValueConverterParameter);
            }
            else
            {
                value = GetConvertedValue(value, targetType.IsArray ? targetType.GetElementType() : targetInfo.PropertyType);
            }

            if (targetType.IsArray)
            {
                ((Array)targetObject).SetValue(value, targetIndices.Last().Cast<int>().ToArray());
            }
            else
            {
                targetInfo.SetValue(targetObject, value, targetIndices.Last());
            }
        }

        private void OnSourceMidPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (context != null && SynchronizationContext.Current != context)
            {
                context.Post((o) =>
                {
                    SynchronizationContext.SetSynchronizationContext(context);
                    OnSourceMidPropertyChanged(sender, e);
                }, this);
                return;
            }

            for (int i = sourceInfos.Length - 1; i >= 0; i--)
            {
                var info = sourceInfos[i];
                if (info.Name == e.PropertyName)
                {
                    break;
                }

                var notify = sourceObjects[i] as INotifyPropertyChanged;
                if (notify != null)
                {
                    RemoveSourceHandlers(notify);
                }
                sourceObjects[i] = null;
            }

            bool above = true;
            var tokens = SourcePath.Split('.');
            for (int i = 0; i < sourceInfos.Length; i++)
            {
                var info = sourceInfos[i];
                if (info.Name != e.PropertyName && above)
                {
                    continue;
                }

                above = false;
                object sourceObj = sourceObjects[i];
                if (sourceObj.GetType().IsArray)
                {
                    sourceObj = ((Array)sourceObj).GetValue(sourceIndices[i].Cast<int>().ToArray());
                }
                else
                {
                    sourceObj = info.GetValue(sourceObjects[i], sourceIndices[i]);
                }

                sourceObj = GetNotifier(sourceObj);
                if (Device.Reflector.GetProperty(sourceObj.GetType(), info.Name) == null)
                {
                    var pairable = sourceObj as IPairable;
                    if (pairable != null)
                    {
                        sourceObj = pairable.Pair;
                    }
                }

                sourceObjects[i + 1] = sourceObj;

                if (i >= sourceInfos.Length - 2)
                {
                    var notify = sourceObj as INotifyPropertyChanged;
                    if (notify != null)
                    {
                        RemoveSourceHandlers(notify);
                        notify.PropertyChanged += OnSourceEndPropertyChanged;
                    }

                    OnSourceEndPropertyChanged(this, new PropertyChangedEventArgs(sourceInfos.Last().Name));
                    break;
                }
                else
                {
                    var notify = sourceObj as INotifyPropertyChanged;
                    if (notify != null)
                    {
                        RemoveSourceHandlers(notify);
                        notify.PropertyChanged += OnSourceMidPropertyChanged;
                    }
                }
            }
        }

        private void OnTargetEndPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (context != null && SynchronizationContext.Current != context)
            {
                context.Post((o) =>
                {
                    SynchronizationContext.SetSynchronizationContext(context);
                    OnTargetEndPropertyChanged(sender, e);
                }, this);
                return;
            }

            if (Mode == BindingMode.OneWayToTarget)
            {
                return;
            }

            var targetToken = TargetPath.Substring(TargetPath.LastIndexOf('.') + 1);
            if (e.PropertyName != targetToken)
            {
                return;
            }

            var sourceObject = sourceObjects.Last();
            var sourceType = sourceObject.GetType();
            var sourceInfo = sourceInfos.Last();
            var targetObject = targetObjects.Last().Target;
            if (targetObject == null)
            {
                Deactivate();
                return;
            }

            object value = null;
            if (targetObject.GetType().IsArray)
            {
                value = ((Array)targetObject).GetValue(targetIndices.Last().Cast<int>().ToArray());
            }
            else
            {
                value = targetInfos.Last().GetValue(targetObject, targetIndices.Last());
            }

            if (ValueConverter != null)
            {
                value = ValueConverter.ConvertBack(value, sourceInfo.PropertyType, ValueConverterParameter);
            }
            else
            {
                value = GetConvertedValue(value, sourceType.IsArray ? sourceType.GetElementType() : sourceInfo.PropertyType);
            }


            if (sourceType.IsArray)
            {
                ((Array)sourceObject).SetValue(value, sourceIndices.Last().Cast<int>().ToArray());
            }
            else
            {
                sourceInfo.SetValue(sourceObject, value, sourceIndices.Last());
            }
        }

        private void OnTargetMidPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (context != null && SynchronizationContext.Current != context)
            {
                context.Post((o) =>
                {
                    SynchronizationContext.SetSynchronizationContext(context);
                    OnTargetMidPropertyChanged(sender, e);
                }, this);
                return;
            }

            for (int i = targetInfos.Length - 1; i >= 0; i--)
            {
                var info = targetInfos[i];
                if (info.Name == e.PropertyName)
                {
                    break;
                }

                var notify = targetObjects[i].Target as INotifyPropertyChanged;
                if (notify != null)
                {
                    RemoveTargetHandlers(notify);
                }
                targetObjects[i] = null;
            }

            bool above = true;
            var tokens = TargetPath.Split('.');
            for (int i = 0; i < targetInfos.Length; i++)
            {
                var info = targetInfos[i];
                if (info.Name != e.PropertyName && above)
                {
                    continue;
                }

                above = false;
                object targetObj = targetObjects[i].Target;
                if (targetObj == null)
                {
                    Deactivate();
                    return;
                }

                if (targetObj.GetType().IsArray)
                {
                    targetObj = ((Array)targetObj).GetValue(targetIndices[i].Cast<int>().ToArray());
                }
                else
                {
                    targetObj = info.GetValue(targetObj, targetIndices[i]);
                }

                targetObj = GetNotifier(targetObj);
                if (Device.Reflector.GetProperty(targetObj.GetType(), info.Name) == null)
                {
                    var pairable = targetObj as IPairable;
                    if (pairable != null)
                    {
                        targetObj = pairable.Pair;
                    }
                }

                targetObjects[i + 1] = new WeakReference(targetObj);

                if (i >= targetInfos.Length - 2)
                {
                    var notify = targetObj as INotifyPropertyChanged;
                    if (notify != null)
                    {
                        RemoveTargetHandlers(notify);
                        notify.PropertyChanged += OnTargetEndPropertyChanged;
                    }

                    OnTargetEndPropertyChanged(this, new PropertyChangedEventArgs(targetInfos.Last().Name));
                    break;
                }
                else
                {
                    var notify = targetObj as INotifyPropertyChanged;
                    if (notify != null)
                    {
                        RemoveTargetHandlers(notify);
                        notify.PropertyChanged += OnTargetMidPropertyChanged;
                    }
                }
            }
        }

        private object GetNotifier(object obj)
        {
            if (obj is INotifyPropertyChanged)
            {
                return obj;
            }

            var pairable = obj as IPairable;
            if (pairable == null)
            {
                return obj;
            }

            if (pairable.Pair is INotifyPropertyChanged)
            {
                return pairable.Pair;
            }

            return obj;
        }

        private void RemoveSourceHandlers(INotifyPropertyChanged notify)
        {
            notify.PropertyChanged -= OnSourceEndPropertyChanged;
            notify.PropertyChanged -= OnSourceMidPropertyChanged;
        }
        
        private void RemoveTargetHandlers(INotifyPropertyChanged notify)
        {
            notify.PropertyChanged -= OnTargetEndPropertyChanged;
            notify.PropertyChanged -= OnTargetMidPropertyChanged;
        }

        private object[] ParseIndex(string index)
        {
            string[] strings = index.Split(',');
            object[] indices = new object[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                string value = strings[i].Trim();
                int integer = value.TryParseInt32(-2);
                if (integer > -2)
                {
                    indices[i] = integer;
                }
                else
                {
                    indices[i] = value;
                }
            }

            return indices;
        }
    }
}
