using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iFactr.UI.Instructions
{
    /// <summary>
    /// Represents a tool for giving platform-specific instructions to cross-platform objects.
    /// This class is abstract.
    /// </summary>
    public abstract class Instructor
    {
        /// <summary>
        /// Performs layout logic for the specified <paramref name="element"/> that is determined by the target platform.
        /// </summary>
        /// <param name="element">The <see cref="ILayoutInstruction"/> instance to lay out.</param>
        public abstract void Layout(ILayoutInstruction element);
    }
}
