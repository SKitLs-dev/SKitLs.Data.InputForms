using SKitLs.Utils.Localizations.Model;

namespace SKitLs.Data.InputForms.Notations
{
    /// <summary>
    /// Represents an attribute for a "select one from many" input field.
    /// </summary>
    /// <remarks>
    /// This attribute is used to define a field that allows the user to select one option from a predefined set of options.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectDataAttribute : InputDataBaseAttribute
    {
        /// <summary>
        /// Gets or sets the name of the method that provides the options for selection.
        /// </summary>
        public string GetOptionsMethodName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectDataAttribute"/> class.
        /// </summary>
        /// <param name="caption">The caption or label for the input field.</param>
        /// <param name="description">A brief description of the input field.</param>
        /// <param name="getOptionsMethodName">The name of the method that provides the options for the input field.</param>
        /// <param name="required">Indicates whether the input field is necessary. Defaults to true.</param>
        /// <param name="previewMethodName">Optional. The name of the method used to generate a preview of the input value.</param>
        public SelectDataAttribute(string caption, string description, string getOptionsMethodName, bool required = true, string? previewMethodName = null) : base(caption, description, required, previewMethodName)
        {
            GetOptionsMethodName = getOptionsMethodName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectDataAttribute"/> class with the specified properties.
        /// </summary>
        /// <param name="getOptionsMethodName">The name of the method that provides the options for the input field.</param>
        /// <param name="required">Indicates whether the input field is necessary. Defaults to true.</param>
        /// <param name="previewMethodName">Optional. The name of the method used to generate a preview of the input value. Can be null.</param>
        public SelectDataAttribute(string getOptionsMethodName, bool required = true, string? previewMethodName = null) : base(required, previewMethodName)
        {
            GetOptionsMethodName = getOptionsMethodName;
        }

        /// <inheritdoc/>
        public override LocalSet? DefaultPreview(object? input)
        {
            if (input is string str)
            {
                if (Required && string.IsNullOrEmpty(str))
                {
                    return Locals.FieldRequiredErrorKey;
                }
                return null;
            }
            else
                return Locals.ShouldSelectOptionErrorKey;
        }
    }
}