using SKitLs.Utils.Localizations.Model;

namespace SKitLs.Data.InputForms.Notations
{
    /// <summary>
    /// Represents an attribute for an integer input field.
    /// </summary>
    /// <remarks>
    /// This attribute is used to define a field that accepts integer input, along with optional constraints for the minimum and maximum values.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class IntInputAttribute : TextInputAttribute
    {
        /// <summary>
        /// Gets or sets the minimum allowable value for the integer input.
        /// </summary>
        public int MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowable value for the integer input.
        /// </summary>
        public int MaxValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntInputAttribute"/> class.
        /// </summary>
        /// <param name="caption">The caption or label for the input field.</param>
        /// <param name="description">A brief description of the input field.</param>
        /// <param name="min">The minimum allowable value for the input field. Defaults to <see cref="int.MinValue"/>.</param>
        /// <param name="max">The maximum allowable value for the input field. Defaults to <see cref="int.MaxValue"/>.</param>
        /// <param name="required">Indicates whether the input field is necessary. Defaults to true.</param>
        /// <param name="previewMethodName">Optional. The name of the method used to generate a preview of the input value.</param>
        public IntInputAttribute(string caption, string description, int min = int.MinValue, int max = int.MaxValue, bool required = true, string? previewMethodName = null) : base(caption, description, required, previewMethodName)
        {
            MinValue = min;
            MaxValue = max;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntInputAttribute"/> class with the specified properties.
        /// </summary>
        /// <param name="min">The minimum allowable value for the input field. Defaults to <see cref="int.MinValue"/>.</param>
        /// <param name="max">The maximum allowable value for the input field. Defaults to <see cref="int.MaxValue"/>.</param>
        /// <param name="required">Indicates whether the input field is necessary. Defaults to true.</param>
        /// <param name="previewMethodName">Optional. The name of the method used to generate a preview of the input value. Can be null.</param>
        public IntInputAttribute(int min = int.MinValue, int max = int.MaxValue, bool required = true, string? previewMethodName = null) : base(required, previewMethodName)
        {
            MinValue = min;
            MaxValue = max;
        }

        /// <inheritdoc/>
        public override LocalSet? DefaultPreview(object? input)
        {
            var error = base.DefaultPreview(input);
            if (error is null)
            {
                var str = (string)input!;
                if (string.IsNullOrEmpty(str) && !Required)
                    return null;
                
                if (!int.TryParse(str, out int value))
                    return Locals.ShouldTypeNumberErrorKey;
                else if (value < MinValue || value > MaxValue)
                    return new(Locals.NumberBetweenErrorKey, [ MinValue, MaxValue ]);
                else if (value < MinValue)
                    return new(Locals.NumberGreaterThanErrorKey, [ MinValue ]);
                else if (value > MaxValue)
                    return new(Locals.NumberLessThanErrorKey, [ MaxValue ]);
                return null;
            }
            else return error;
        }

        /// <inheritdoc/>
        public override object? ValueBuilder(object? input)
        {
            if (input is string str)
            {
                if (int.TryParse(str, out int value))
                    return value;
            }
            return base.ValueBuilder(input);
        }
    }
}