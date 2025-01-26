using SKitLs.Data.InputForms.InputParts;
using SKitLs.Data.InputForms.Notations;
using System.Reflection;

namespace SKitLs.Data.InputForms
{
    /// <summary>
    /// Represents a delegate for building input fields based on property information and metadata attributes.
    /// </summary>
    /// <param name="info">The property information of the field to be built.</param>
    /// <param name="meta">The metadata attribute containing information about the input field.</param>
    /// <param name="form">The object form that contains the input field.</param>
    /// <returns>An instance of <see cref="InputPartBase"/> representing the constructed input field.</returns>
    public delegate InputPartBase FieldBuilder(PropertyInfo info, InputDataBaseAttribute meta, object form);

    /// <summary>
    /// Manages the mapping of metadata attributes to their corresponding input field builders.
    /// </summary>
    public class FieldMapper
    {
        private static readonly FieldMapper _globalMapper = new();

        /// <summary>
        /// Gets the global instance of the <see cref="FieldMapper"/>.
        /// </summary>
        public static FieldMapper GlobalMapper
        {
            get => _globalMapper;
        }

        /// <summary>
        /// Gets or sets the dictionary that maps metadata attributes to field builders.
        /// </summary>
        public Dictionary<Type, FieldBuilder> MapData { get; set; }

        /// <summary>
        /// Gets or sets the default field builder.
        /// </summary>
        public FieldBuilder DefaultField { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldMapper"/> class.
        /// <para/>
        /// The constructor automatically adds default mapping data for several types of input fields.
        /// </summary>
        /// <param name="defaultFieldBuilder">Optional. The default field builder to use if no mapping is found. If not provided, <see cref="StringInputPart.BuildSelf"/> is used.</param>
        public FieldMapper(FieldBuilder? defaultFieldBuilder = null)
        {
            DefaultField = defaultFieldBuilder ?? StringInputPart.BuildSelf;

            MapData = new()
            {
                { typeof(BoolDataAttribute), BoolInputPart.BuildSelf },
                { typeof(TextInputAttribute), StringInputPart.BuildSelf },
                { typeof(IntInputAttribute), StringInputPart.BuildSelf },
                { typeof(T2D8InputAttribute), StringInputPart.BuildSelf },
                { typeof(SelectDataAttribute), SelectInputPart.BuildSelf },
                { typeof(BrowseDataAttribute), BrowseInputPart.BuildSelf },
            };
        }

        /// <summary>
        /// Declares a new input field builder for a specific metadata attribute type.
        /// </summary>
        /// <param name="metaAttribute">The type of the metadata attribute.</param>
        /// <param name="field">The field builder to associate with the metadata attribute.</param>
        /// <param name="override">Specifies whether to override the existing field builder if one already exists.</param>
        public void DeclareInputField(Type metaAttribute, FieldBuilder field, bool @override)
        {
            if (!MapData.TryAdd(metaAttribute, field))
            {
                if (@override)
                    MapData[metaAttribute] = field;
                else // TODO
                    throw new Exception("Field builder already exists and override is not allowed.");
            }
        }

        /// <summary>
        /// Resolves the input field builder for a given property and metadata attribute.
        /// </summary>
        /// <param name="info">The property information of the input field.</param>
        /// <param name="meta">The metadata attribute associated with the input field.</param>
        /// <param name="form">The form object that contains the input field.</param>
        /// <returns>The resolved input field builder.</returns>
        public InputPartBase ResolveInputField(PropertyInfo info, InputDataBaseAttribute meta, object form)
        {
            return MapData[meta.GetType()]?.Invoke(info, meta, form) ?? DefaultField.Invoke(info, meta, form);
        }
    }
}