using SKitLs.Utils.Localizations.Model;

namespace SKitLs.Data.InputForms.Notations
{
    /// <summary>
    /// Represents an attribute for a boolean input field.
    /// </summary>
    /// <remarks>
    /// This attribute is used to define a field that accepts a boolean input, typically represented as a flag (yes/no or true/false).
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class BoolDataAttribute : InputDataBaseAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoolDataAttribute"/> class.
        /// </summary>
        /// <param name="caption">The caption or label for the input field.</param>
        /// <param name="description">A brief description of the input field.</param>
        /// <param name="required">Indicates whether the input field is necessary. Defaults to true.</param>
        /// <param name="previewMethodName">Optional. The name of the method used to generate a preview of the input value.</param>
        public BoolDataAttribute(string caption, string description, bool required = true, string? previewMethodName = null) : base(caption, description, required, previewMethodName)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolDataAttribute"/> class with the specified properties.
        /// </summary>
        /// <param name="required">Indicates whether the input field is necessary. Defaults to true.</param>
        /// <param name="previewMethodName">Optional. The name of the method used to generate a preview of the input value. Can be null.</param>
        public BoolDataAttribute(bool required = true, string? previewMethodName = null) : base(required, previewMethodName) { }

        /// <inheritdoc/>
        public override LocalSet? DefaultPreview(object? input)
        {
            if (input is bool _)
                return null;
            else
                return Locals.ShouldBeTrueFalseErrorKey;
        }
    }
}