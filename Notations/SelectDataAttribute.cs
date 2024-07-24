using SKitLs.Utils.Localizations.Model;

namespace SKitLs.Data.InputForms.Notations
{
    /// <summary>
    /// Represents an attribute for a "select one from many" input field.
    /// </summary>
    /// <remarks>
    /// This attribute is used to define a field that allows the user to select one option from a predefined set of options.
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SelectDataAttribute"/> class.
    /// </remarks>
    /// <param name="caption">The caption or label for the input field.</param>
    /// <param name="description">A brief description of the input field.</param>
    /// <param name="getOptionsMethodName">The name of the method that provides the options for the input field.</param>
    /// <param name="isNecessary">Indicates whether the input field is necessary. Defaults to true.</param>
    /// <param name="previewMethodName">Optional. The name of the method used to generate a preview of the input value.</param>
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectDataAttribute(string caption, string description, string getOptionsMethodName, bool isNecessary = true, string? previewMethodName = null) : InputDataBaseAttribute(caption, description, isNecessary, previewMethodName)
    {
        /// <summary>
        /// Gets or sets the name of the method that provides the options for selection.
        /// </summary>
        public string GetOptionsMethodName { get; set; } = getOptionsMethodName;

        /// <inheritdoc/>
        public override LocalSet? DefaultPreview(object? input)
        {
            if (input is string str)
            {
                if (IsNecessary && string.IsNullOrEmpty(str))
                {
                    return Locals.FieldIsNecessaryErrorKey;
                }
                return null;
            }
            else
                return Locals.ShouldSelectOptionErrorKey;
        }
    }
}