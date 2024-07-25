using SKitLs.Data.InputForms.Notations;
using SKitLs.Utils.Localizations.Languages;
using SKitLs.Utils.Localizations.Localizators;
using SKitLs.Utils.Localizations.Model;
using System.Reflection;

namespace SKitLs.Data.InputForms.InputParts
{
    /// <summary>
    /// Represents the base class for all input parts in the form.
    /// </summary>
    public abstract class InputPartBase
    {
        /// <summary>
        /// Event that triggers when the value of the input part is updated.
        /// </summary>
        public event Action<object?>? OnValueUpdated;
        
        /// <summary>
        /// Event that triggers when the validation status of the input part is updated.
        /// </summary>
        public event Action<bool>? ValidationUpdated;

        /// <summary>
        /// Event that triggers when the input part successfully passes validation.
        /// </summary>
        public event Action<object?>? OnSuccessValidation;

        /// <summary>
        /// Gets or sets the input field parent form.
        /// </summary>
        public InputForm? Parent { get; set; }

        /// <summary>
        /// Gets the localizator used to resolve localized strings for this input part.
        /// </summary>
        private ILocalizator? Localizator => Parent?.Localizator;

        // TODO
        /// <summary>
        /// Gets the language code used to resolve localized strings for this input part.
        /// </summary>
        private LanguageCode Language => Parent?.Language ?? throw new NullReferenceException(nameof(Language));

        /// <summary>
        /// Gets or sets the <see cref="PropertyInfo"/> associated with the input part.
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the input part is locked and cannot be edited.
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets the caption of the input part.
        /// </summary>
        public string InputCaption { get; set; }

        /// <summary>
        /// Gets or sets the description of the input part.
        /// </summary>
        public string? InputDescription { get; set; }

        private object? _inputValue;

        /// <summary>
        /// Gets or sets the value of the input part.
        /// When the value changes, triggers <c><see cref="OnValueUpdated"/></c> and <c><see cref="OnSuccessValidation"/></c> if valid.
        /// </summary>
        public object? InputValue
        {
            get => _inputValue;
            set
            {
                if (_inputValue != value)
                {
                    _inputValue = value;
                    Validate(value);
                    if (IsValid)
                        OnSuccessValidation?.Invoke(InputValue);
                }
            }
        }

        /// <summary>
        /// Gets or sets the function used to preview input value: checks whether input is valid.
        /// </summary>
        public Func<object?, LocalSet?>? InputPreview { get; set; }

        /// <summary>
        /// Updates input field validation, based on <see cref="InputPreview"/>.
        /// </summary>
        /// <param name="input"></param>
        public virtual void Validate(object? input) => IsValid = Preview(input) is null;

        /// <summary>
        /// Generates an error of the input preview, which can be used for display purposes.
        /// </summary>
        /// <param name="input">The input to be previewed.</param>
        /// <returns>A <see cref="LocalSet"/> containing the preview error information, or <see langword="null"/> if the input is valid.</returns>
        public virtual LocalSet? Preview(object? input) => InputPreview?.Invoke(input);

        /// <summary>
        /// Generates a preview of the input in the specified language.
        /// </summary>
        /// <param name="input">The input to be previewed.</param>
        /// <param name="language">The language code for localization. The form's <see cref="InputForm{T}.Language"/> is used if not specified.</param>
        /// <returns>The localized error preview string, or <see langword="null"/> if the input is valid.</returns>
        /// <exception cref="NullReferenceException">Thrown when the <see cref="Localizator"/> is null.</exception>
        public virtual string? Preview(object? input, LanguageCode? language = default)
        {
            var error = Preview(input);
            language ??= Language;
            if (error.HasValue) // TODO
                return (Localizator ?? throw new NullReferenceException(nameof(Localizator))).ResolveString(language, error.Value, true) ?? throw new Exception($"Local key not found {error.Value}.");
            return null;
        }

        private bool _isValid;

        /// <summary>
        /// Gets or sets a value indicating whether the input part is valid.
        /// When the validation status changes, triggers <see cref="ValidationUpdated"/>.
        /// </summary>
        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    ValidationUpdated?.Invoke(IsValid);
                }
            }
        }

        /// <summary>
        /// Gets or sets the function used to build the input value.
        /// </summary>
        public Func<object?, object?>? InputValueBuilder { get; set; }

        /// <summary>
        /// Gets the current input value, using <see cref="InputValueBuilder"/> if provided.
        /// </summary>
        /// <returns>The current input value.</returns>
        public virtual object? GetInputValue()
        {
            if (InputValueBuilder is not null)
                return InputValueBuilder.Invoke(InputValue);
            return InputValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputPartBase"/> class with the specified property information.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> associated with this input part.</param>
        /// <param name="meta">The meta information provided for a property.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyInfo"/> or <paramref name="meta"/> is <see langword="null"/>.</exception>
        public InputPartBase(PropertyInfo propertyInfo, InputDataBaseAttribute meta)
        {
            ArgumentNullException.ThrowIfNull(meta);

            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
            InputCaption = meta.Caption;
            InputDescription = meta.Description;
        }

        // public abstract InputPartBase BuildSelf(PropertyInfo info, InputDataBaseAttribute meta, object form);

        // public abstract InputPartBase BuildSelf(PropertyInfo info, InputDataBaseAttribute meta, object form, params object[] args) => throw new NotImplementedException();

        /// <inheritdoc/>
        public override string ToString() => $"{InputCaption}: {InputValue} ({IsValid})";
    }
}