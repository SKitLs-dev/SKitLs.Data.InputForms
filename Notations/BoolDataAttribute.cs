using SKitLs.Utils.Localizations.Model;

namespace SKitLs.Data.InputForms.Notations
{
    /// <summary>
    /// Represents an attribute for a boolean input field.
    /// </summary>
    /// <remarks>
    /// This attribute is used to define a field that accepts a boolean input, typically represented as a flag (yes/no or true/false).
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BoolDataAttribute"/> class.
    /// </remarks>
    /// <param name="caption">The caption or label for the input field.</param>
    /// <param name="description">A brief description of the input field.</param>
    /// <param name="isNecessary">Indicates whether the input field is necessary. Defaults to true.</param>
    /// <param name="previewMethodName">Optional. The name of the method used to generate a preview of the input value.</param>
    [AttributeUsage(AttributeTargets.Property)]
    public class BoolDataAttribute(string caption, string description, bool isNecessary = true, string? previewMethodName = null) : InputDataBaseAttribute(caption, description, isNecessary, previewMethodName)
    {
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