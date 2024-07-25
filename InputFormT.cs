using SKitLs.Data.InputForms.InputParts;
using SKitLs.Data.InputForms.Notations;
using SKitLs.Data.InputForms.Notations.Refs;
using SKitLs.Utils.Extensions.Lists;
using SKitLs.Utils.Localizations.Languages;
using SKitLs.Utils.Localizations.Localizators;
using SKitLs.Utils.Localizations.Model;
using System.Reflection;

namespace SKitLs.Data.InputForms
{
    /// <summary>
    /// The base <see cref="InputForm{T}"/> abstract class containing static members and common properties.
    /// </summary>
    public abstract class InputForm
    {
        /// <summary>
        /// Gets or sets the default localizator for resolving localized strings.
        /// </summary>
        public static ILocalizator? DefaultLocalizator { get; set; }

        /// <summary>
        /// Gets or sets the language code used to resolve localized strings for this form.
        /// </summary>
        public static LanguageCode DefaultLanguage { get; set; } = LanguageCode.EN;

        /// <summary>
        /// Gets or sets the localizator used to resolve localized strings for this form.
        /// </summary>
        public ILocalizator? Localizator { get; set; }

        /// <summary>
        /// Gets or sets the language code used to resolve localized strings for this form.
        /// </summary>
        public LanguageCode Language { get; set; }
    }

    /// <summary>
    /// Represents a form for user input, containing the data and the input parts.
    /// </summary>
    /// <typeparam name="T">The type of the data that this form handles.</typeparam>
    public class InputForm<T> : InputForm where T : notnull
    {
        /// <summary>
        /// Event that triggers when the validation status of the form is updated.
        /// </summary>
        public event Action<bool>? ValidationUpdated;

        /// <summary>
        /// Triggers the <see cref="ValidationUpdated"/> event with the specified validation status.
        /// </summary>
        /// <param name="validation">The validation status to be triggered.</param>
        protected void OnValidationUpdated(bool validation) => ValidationUpdated?.Invoke(validation);

        /// <summary>
        /// Gets or sets the <see cref="FieldMapper"/> used to map metadata attributes to input field builders.
        /// </summary>
        public FieldMapper Mapper { get; set; }

        /// <summary>
        /// Gets the list of input parts that make up the form.
        /// </summary>
        protected List<InputPartBase> InputParts { get; set; }

        /// <summary>
        /// Gets the list of input parts that make up the form.
        /// </summary>
        public IReadOnlyList<InputPartBase> GetDefinedParts() => InputParts;

        /// <summary>
        /// Gets a value indicating whether the form is valid based on the validation status of all input parts.
        /// </summary>
        public bool IsValid => InputParts.Select(x => x.IsValid).AllTrue();

        /// <summary>
        /// Gets the form data.
        /// </summary>
        public T FormData { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputForm{T}"/> class.
        /// </summary>
        /// <param name="formData">The form data to initialize the form with.</param>
        /// <param name="language">The language used to localize form's fields.</param>
        /// <param name="mapper">The <see cref="FieldMapper"/> class to manage the mapping of metadata attributes to their corresponding input field builders.</param>
        /// <param name="localizator">The <see cref="ILocalizator"/> class to resolve localized strings.</param>
        /// <exception cref="ArgumentNullException">Thrown when the form data is null.</exception>
        public InputForm(T formData, FieldMapper? mapper = null, ILocalizator? localizator = null, LanguageCode? language = null)
        {
            Language = language ?? DefaultLanguage;
            Mapper = mapper ?? FieldMapper.GlobalMapper ?? throw new ArgumentNullException(nameof(mapper));
            Localizator = localizator ?? DefaultLocalizator;

            InputParts = [];
            FormData = formData ?? throw new ArgumentNullException(nameof(formData));

            Initialize(Mapper);
        }

        /// <summary>
        /// Initializes the input form by setting up the input parts based on the form data properties and their attributes.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when a self-reference is found in dependent references.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a required method for reference updates is not found.</exception>
        private void Initialize(FieldMapper mapper)
        {
            var fields = FormData.GetType().GetProperties().Where(p => p.GetCustomAttribute<InputDataBaseAttribute>(true) is not null);
            
            // Setup form fields
            foreach (var property in fields)
            {
                var meta = property.GetCustomAttribute<InputDataBaseAttribute>(true)!;
                meta.Localize(property, this);
                var value = property.GetValue(FormData);

                var inputFieldData = mapper.ResolveInputField(property, meta, FormData);

                inputFieldData.InputValue = value is ICloneable clone ? clone.Clone() : value;
                inputFieldData.InputPreview = ResolveInputPreview(meta);
                inputFieldData.ValidationUpdated += OnValidationUpdated;
                inputFieldData.InputValueBuilder = meta.ValueBuilder;
                inputFieldData.Parent = this;
                InputParts.Add(inputFieldData);
            }

            // Setup references
            foreach (var property in fields)
            {
                var masterReferenceAttribute = property.GetCustomAttribute<DependentReferenceAttribute>(true);
                if (masterReferenceAttribute is null)
                    continue;

                // TODO
                if (masterReferenceAttribute.Dependents.Contains(property.Name))
                    throw new ArgumentException("No Self dependence is allowed.");

                // Find master field / ведущее поле
                var masterField = InputParts.Find(x => x.PropertyInfo.Name == property.Name) ?? throw new ArgumentException("Ref not found");
                foreach (var slaveFieldName in masterReferenceAttribute.Dependents)
                {
                    // Find slave field / зависящее поле
                    var slaveField = InputParts.Find(x => x.PropertyInfo.Name == slaveFieldName) ?? throw new ArgumentException("Ref not found");

                    var callbackObject = masterReferenceAttribute.ResolveCallbackReference();
                    MethodInfo callbackMethod;
                    object callbackReceiver;

                    // Если ссылка на метод формы / If linked to a form
                    if (callbackObject is string formMethodName)
                    {
                        callbackMethod = FormData.GetType().GetMethod(formMethodName) ?? throw new InvalidOperationException($"Form callback method '{masterReferenceAttribute.ReferenceUpdatedMethodName}' not found in type '{FormData.GetType().FullName}'");
                        callbackReceiver = FormData;
                    }
                    // Если собственный метод / If attribute's method
                    else if (callbackObject is MethodInfo method)
                    {
                        callbackMethod = method ?? throw new InvalidOperationException($"Requested attribute form callback method not found in '{masterReferenceAttribute.GetType().FullName}'");
                        callbackReceiver = masterReferenceAttribute;
                    }
                    else // TODO
                        throw new Exception("Unresolved type of the callback.");

                    if (masterReferenceAttribute.IgnoreFailedPreview)
                        masterField.OnSuccessValidation += CallbackWrapper;
                    else
                        masterField.OnValueUpdated += CallbackWrapper;

                    void CallbackWrapper(object? value)
                    {
                        callbackMethod.Invoke(callbackReceiver, [masterField, slaveField]);
                    }
                }
            }
        }

        /// <summary>
        /// Applies changes from the input parts to the form data and returns the updated form data.
        /// </summary>
        /// <returns>The updated form data.</returns>
        /// <exception cref="Exception">Thrown when the input data is not valid.</exception>
        public T ApplyChanges()
        {
            if (IsValid)
            {
                foreach (var part in InputParts)
                {
                    part.PropertyInfo.SetValue(FormData, part.GetInputValue());
                }
                return FormData;
            }
            else // TODO
                throw new Exception("Invalid Input.");
        }

        /// <summary>
        /// Resolves the input preview function for a given metadata attribute.
        /// </summary>
        /// <param name="inputInfo">The metadata attribute associated with the input field.</param>
        /// <returns>The resolved input preview function.</returns>
        internal static Func<object?, LocalSet?>? ResolveInputPreview(InputDataBaseAttribute inputInfo)
        {
            if (inputInfo.PreviewMethodName is not null)
            {
                var method = typeof(T).GetMethod(inputInfo.PreviewMethodName);
                if (method is not null)
                {
                    var func = (Func<object?, LocalSet?>?)Delegate.CreateDelegate(typeof(Func<object, LocalSet?>), null, method);
                    return func;
                }
            }
            return inputInfo.DefaultPreview;
        }
    }
}