using SKitLs.Utils.Localizations.Model;

namespace SKitLs.Data.InputForms.Notations
{
    /// <summary>
    /// Represents an attribute for text input fields, providing metadata and validation for string inputs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TextInputAttribute : InputDataBaseAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputAttribute"/> class with the specified properties.
        /// </summary>
        /// <param name="caption">The caption for the input field.</param>
        /// <param name="description">The description for the input field.</param>
        /// <param name="isNecessary">A value indicating whether the input field is necessary. Defaults to true.</param>
        /// <param name="previewMethodName">The name of the method used to generate a preview of the input value. Defaults to null.</param>
        public TextInputAttribute(string caption, string description, bool isNecessary = true, string? previewMethodName = null) : base(caption, description, isNecessary, previewMethodName)
        { }

        /// <inheritdoc/>
        /// <remarks>
        /// Validates <paramref name="input"/> is a <see cref="string"/> and checks if the field is necessary.
        /// </remarks>
        public override LocalSet? DefaultPreview(object? input)
        {
            if (input is string str)
            {
                if (IsNecessary && string.IsNullOrEmpty(str))
                    return Locals.FieldIsNecessaryErrorKey;
                return null;
            }
            else return Locals.ShouldTypeTextErrorKey;
        }
    }
}