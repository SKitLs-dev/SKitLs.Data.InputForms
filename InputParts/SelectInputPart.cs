using SKitLs.Data.InputForms.Notations;
using SKitLs.Utils.Localizations.Model;
using System.Reflection;

namespace SKitLs.Data.InputForms.InputParts
{
    /// <summary>
    /// Represents an input part for handling select input values.
    /// </summary>
    public class SelectInputPart : InputPartBase
    {
        /// <summary>
        /// Occurs when the options for the select input part are updated.
        /// </summary>
        public event Action? OnOptionsUpdated;

        /// <summary>
        /// Gets the source attribute containing metadata for the select input.
        /// </summary>
        private SelectDataAttribute SourceAttribute { get; set; }

        /// <summary>
        /// Gets or sets the options available for selection.
        /// </summary>
        public List<SelectValue> Options { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectInputPart"/> class with the specified property information, metadata, and form data.
        /// </summary>
        /// <param name="propertyInfo">The property information of the select input field.</param>
        /// <param name="meta">The metadata attribute containing information about the select input field.</param>
        /// <param name="formData">The object form that contains the input field.</param>
        public SelectInputPart(PropertyInfo propertyInfo, SelectDataAttribute meta, object formData) : base(propertyInfo, meta)
        {
            SourceAttribute = meta;
            Options = null!;
            InputPreview = Preview;
            UpdateOptions(formData);
        }

        /// <summary>
        /// Updates the options available for selection based on the form data.
        /// </summary>
        /// <param name="formData">The object form that contains the input field.</param>
        public void UpdateOptions(object formData)
        {
            // TODO
            if (SourceAttribute is null)
                throw new NullReferenceException();

            var methodInfo = formData.GetType().GetMethod(SourceAttribute.GetOptionsMethodName) ?? throw new InvalidOperationException($"Method '{SourceAttribute.GetOptionsMethodName}' not found in type '{formData.GetType().FullName}'");
            var result = methodInfo.Invoke(formData, null);

            if (result is not List<SelectValue> resolvedOptions)
            {
                // TODO
                throw new InvalidOperationException($"The result of the method is not of type {typeof(List<string>)}");
            }

            if (!SourceAttribute.Required)
                resolvedOptions.Insert(0, new (Locals.DefaultSelectOption, null!));

            Options = resolvedOptions;
            OnOptionsUpdated?.Invoke();
        }

        /// <summary>
        /// Generates a preview of the select input value.
        /// </summary>
        /// <param name="input">The input value to be previewed.</param>
        /// <returns>A localized error message if preview failed; otherwise <see langword="null"/>.</returns>
        public override LocalSet? Preview(object? input)
        {
            if (input is string str)
            {
                if (SourceAttribute.Required && string.IsNullOrEmpty(str))
                    return Locals.FieldRequiredErrorKey;
                if (Options.Find(x => str.Equals(x.Label, StringComparison.CurrentCultureIgnoreCase)) is null)
                    return Locals.ShouldSelectOptionErrorKey;
                return null;
            }
            else return Locals.ShouldTypeTextErrorKey;
        }

        /// <inheritdoc/>
        public override object? GetInputValue() => Options.Find(x => x.Label == InputValue?.ToString())?.Value;

        /// <summary>
        /// Builds a new instance of <see cref="SelectInputPart"/> based on the provided property information and metadata attribute.
        /// </summary>
        /// <param name="info">The property information of the field to be built.</param>
        /// <param name="meta">The metadata attribute containing information about the input field.</param>
        /// <param name="form">The object form that contains the input field.</param>
        /// <returns>A new instance of <see cref="SelectInputPart"/>.</returns>
        public static InputPartBase BuildSelf(PropertyInfo info, InputDataBaseAttribute meta, object form)
        {
            // TODO
            var select = (meta as SelectDataAttribute) ?? throw new NotImplementedException();
            return new SelectInputPart(info, select, form);
        }
    }

    /// <summary>
    /// Represents a selectable value with a label and associated value.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SelectValue"/> class with the specified label and value.
    /// </remarks>
    /// <param name="label">The label of the selectable value.</param>
    /// <param name="value">The value associated with the selectable value.</param>
    public class SelectValue(string label, object value)
    {
        /// <summary>
        /// Gets or sets the label of the selectable value.
        /// </summary>
        public string Label { get; set; } = label;

        /// <summary>
        /// Gets or sets the value associated with the selectable value.
        /// </summary>
        public object Value { get; set; } = value;

        /// <inheritdoc/>
        public override string ToString() => $"'{Label}': {Value}";
    }
}