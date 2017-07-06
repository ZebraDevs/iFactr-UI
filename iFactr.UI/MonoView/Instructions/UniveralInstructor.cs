namespace iFactr.UI.Instructions
{
    /// <summary>
    /// Represents an instructor that provides platform-agnostic instructions to various objects.
    /// </summary>
    public class UniversalInstructor : Instructor
    {
        /// <summary>
        /// Performs layout logic for the specified <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The <see cref="ILayoutInstruction"/> instance to lay out.</param>
        public sealed override void Layout(ILayoutInstruction element)
        {
            if (element != null)
            {
                OnLayout(element);
            }
        }

        /// <summary>
        /// Called when the specified <paramref name="element"/> needs to be laid out.
        /// </summary>
        /// <param name="element">The <see cref="ILayoutInstruction"/> instance to be laid out.</param>
        protected virtual void OnLayout(ILayoutInstruction element)
        {
            element.Layout();
        }
    }
}