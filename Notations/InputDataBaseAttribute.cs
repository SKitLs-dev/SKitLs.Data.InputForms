using SKitLs.Utils.Localizations.Languages;
using SKitLs.Utils.Localizations.Localizators;
using SKitLs.Utils.Localizations.Model;

namespace SKitLs.Data.InputForms.Notations
{
    /// <summary>
    /// Represents the base attribute for input data, providing metadata for form fields.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="InputDataBaseAttribute"/> class with the specified properties.
    /// </remarks>
    /// <param name="caption">The caption for the input field.</param>
    /// <param name="description">The description for the input field.</param>
    /// <param name="isNecessary">A value indicating whether the input field is necessary.</param>
    /// <param name="previewMethodName">The name of the method used to generate a preview of the input value.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="caption"/> or <paramref name="description"/> is <see langword="null"/></exception>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class InputDataBaseAttribute(string caption, string description, bool isNecessary, string? previewMethodName) : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether the input field is necessary.
        /// </summary>
        public bool IsNecessary { get; set; } = isNecessary;

        /// <summary>
        /// Gets or sets the caption for the input field.
        /// </summary>
        public string Caption { get; set; } = caption ?? throw new ArgumentNullException(nameof(caption));

        /// <summary>
        /// Gets or sets the description for the input field.
        /// </summary>
        public string Description { get; set; } = description ?? throw new ArgumentNullException(nameof(description));

        /// <summary>
        /// Gets or sets the name of the method used to preview input value.
        /// </summary>
        public string? PreviewMethodName { get; set; } = previewMethodName;

        /// <summary>
        /// Realizes default validation mechanism for provided input.
        /// </summary>
        /// <param name="input">The input value to preview.</param>
        /// <returns>An error message if input is invalid.</returns>
        public abstract LocalSet? DefaultPreview(object? input);

        /// <summary>
        /// Builds the input value. This method can be overridden to provide custom value building logic.
        /// </summary>
        /// <param name="input">The input value to build.</param>
        /// <returns>The built input value.</returns>
        public virtual object? ValueBuilder(object? input) => input;
    }
}