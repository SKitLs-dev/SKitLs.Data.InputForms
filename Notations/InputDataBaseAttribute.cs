using SKitLs.Utils.Localizations.Languages;
using SKitLs.Utils.Localizations.Localizators;
using SKitLs.Utils.Localizations.Model;
using System;
using System.Reflection;

namespace SKitLs.Data.InputForms.Notations
{
    /// <summary>
    /// Represents the base attribute for input data, providing metadata for form fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class InputDataBaseAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether the input field is necessary.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the caption for the input field.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the description for the input field.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the method used to preview input value.
        /// </summary>
        public string? PreviewMethodName { get; set; }

        internal bool ShouldLocalize { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDataBaseAttribute"/> class with the default properties for localization.
        /// </summary>
        public InputDataBaseAttribute(bool required, string? previewMethodName)
        {
            Required = required;
            PreviewMethodName = previewMethodName;
            ShouldLocalize = true;
            Caption = "{0}.{1}.{2}.caption";
            Description = "{0}.{1}.{2}.description";
        }

        /// <summary>
        /// Initializes the attribute with the context of the specified property, setting the
        /// <see cref="Caption"/> and <see cref="Description"/> based on the class and property names if localization is required.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> of the property to which this attribute is applied.</param>
        /// <param name="parent">The paren <see cref="InputForm"/>.</param>
        /// <exception cref="Exception">
        /// Thrown when the declaring type or property name cannot be retrieved from the <paramref name="property"/>.
        /// </exception>
        public void Localize(PropertyInfo property, InputForm parent)
        {
            if (ShouldLocalize)
            {
                // TODO
                var className = property.DeclaringType?.Name ?? throw new Exception("Error initializing data attribute: unable to retrieve declaring type.");
                var propertyName = property.Name ?? throw new Exception("Error initializing data attribute: unable to retrieve property name.");

                Caption = string.Format(Caption, Locals.NamespacePrefix, className, propertyName);
                Description = string.Format(Description, Locals.NamespacePrefix, className, propertyName);

                Caption = parent.Localizator?.ResolveString(parent.Language, Caption, false) ?? throw new Exception($"Unable to resolve key '{Caption}'");
                Description = parent.Localizator?.ResolveString(parent.Language, Description, false) ?? throw new Exception($"Unable to resolve key '{Description}'");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDataBaseAttribute"/> class with the specified properties.
        /// </summary>
        /// <param name="caption">The caption for the input field.</param>
        /// <param name="description">The description for the input field.</param>
        /// <param name="required">A value indicating whether the input field is necessary.</param>
        /// <param name="previewMethodName">The name of the method used to generate a preview of the input value.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="caption"/> or <paramref name="description"/> is <see langword="null"/></exception>
        public InputDataBaseAttribute(string caption, string description, bool required, string? previewMethodName)
        {
            Caption = caption ?? throw new ArgumentNullException(nameof(caption));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Required = required;
            PreviewMethodName = previewMethodName;
        }

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