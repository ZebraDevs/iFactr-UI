using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iFactr.UI.Instructions
{
    /// <summary>
    /// Defines an element that may have special layout requirements depending on the target platform.
    /// </summary>
    public interface ILayoutInstruction
    {
        /// <summary>
        /// Performs a platform-agnostic layout.
        /// This is used when the target platform does not have any special layout instructions for the element.
        /// </summary>
        void Layout();
    }
}
