using System.Linq;
using System.Reflection;
using iFactr.Core.Targets;
using MonoCross.Utilities;
using iFactr.UI;

namespace System
{
    /// <summary>
    /// Provides methods for executing an <see cref="EventHandler"/> on <see cref="IPairable"/> objects.
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="handler">The delegate to invoke.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void Raise(this Delegate handler, object sender, EventArgs eventArgs)
        {
            if (handler == null) return;
            try
            {
                handler.GetMethodInfo()
                    .Invoke(handler.Target, new[] { TargetFactory.GetPair(sender), eventArgs });
            }
            catch (TargetInvocationException e)
            {
                throw new TargetInvocationException("Please check the inner exception for error details.", e.InnerException);
            }
        }

        /// <summary>
        /// Attempts to raise the event with the specified name and <see cref="EventArgs"/> on the current instance.
        /// </summary>
        /// <param name="obj">The current object.</param>
        /// <param name="eventName">The name of the event to be raised.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the event is successfully raised, otherwise <c>false</c>.</returns>
        public static bool RaiseEvent(this IPairable obj, string eventName, EventArgs args)
        {
            if (obj == null) return false;
            var type = obj.GetType();
            var evt = Device.Reflector.GetEvent(type, eventName);
            if (evt == null) return false;
            var attribute = evt.GetCustomAttributes(typeof(EventDelegateAttribute), true).FirstOrDefault() as EventDelegateAttribute;
            if (attribute != null)
            {
                eventName = attribute.DelegateName;
            }

            FieldInfo info;
            do
            {
                info = Device.Reflector.GetField(type, eventName);
                type = Device.Reflector.GetBaseType(type);
            }
            while (info == null && type != null);

            if (info == null && obj.Pair != null)
            {
                obj = obj.Pair;
                type = obj.GetType();
                evt = Device.Reflector.GetEvent(type, eventName);
                if (evt == null) return false;
                attribute = evt.GetCustomAttributes(typeof(EventDelegateAttribute), true).FirstOrDefault() as EventDelegateAttribute;
                if (attribute != null)
                {
                    eventName = attribute.DelegateName;
                }

                do
                {
                    info = Device.Reflector.GetField(type, eventName);
                    type = Device.Reflector.GetBaseType(type);
                }
                while (info == null && type != null);
            }

            if (info == null) return false;
            var del = info.GetValue(obj) as MulticastDelegate;
            if (del == null) return false;
            var invocationList = del.GetInvocationList();
            foreach (var method in invocationList)
            {
                method.Raise(obj,args);
            }
            return true;
        }
    }
}