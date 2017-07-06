using System;
using System.ComponentModel;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a field with a slider control.
    /// </summary>
    public class SliderField : Field, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the value of this instance.
        /// </summary>
        /// <value>The field value as a <see cref="float"/> value.</value>
        public float Value
        {
            get { return _value; }
            set
            {
                _value = Math.Min(Max, Math.Max(Min, (int)(value / StepSize) * StepSize));
                OnPropertyChanged("Value");
            }
        }
        private float _value;

        /// <summary>
        /// Maximum value of slider
        /// </summary>
        /// <value>The field value as a <see cref="float"/> value.</value>
        public float Max { get; set; }
        /// <summary>
        /// Minimum value of slider
        /// </summary>
        /// <value>The field value as a <see cref="float"/> value.</value>
        public float Min { get; set; }
        /// <summary>
        /// Sets increment size for slider
        /// </summary>
        /// <value>The field value as a <see cref="float"/> value.</value>
        public float StepSize { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderField"/> class with no submission ID.
        /// </summary>
        public SliderField() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderField"/> class using the ID provided.
        /// </summary>
        /// <param name="id">A <see cref="string"/> representing the ID.</param>
        public SliderField(string id)
        {
            ID = id;
            StepSize = 1;
            Max = 100;
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new SliderField Clone()
        {
            return (SliderField)CloneOverride();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null) propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}