namespace SKitLs.Data.InputForms
{
    /// <summary>
    /// Provides static properties for localization keys, allowing for configuration and namespace prefixing.
    /// </summary>
    public static class Locals
    {
        /// <summary>
        /// Gets or sets the namespace prefix for all localization keys.
        /// </summary>
        public static string NamespacePrefix { get; set; } = "iforms";

        private static string _fieldRequiredErrorKey = "FieldRequired";
        /// <summary>
        /// Gets or sets the localization key for the "field is necessary" error message.
        /// </summary>
        public static string FieldRequiredErrorKey
        {
            get => NamespacePrefix + "." + _fieldRequiredErrorKey;
            set => _fieldRequiredErrorKey = value;
        }

        private static string _shouldTypeTextErrorKey = "ShouldTypeText";
        /// <summary>
        /// Gets or sets the localization key for the "should type text" error message.
        /// </summary>
        public static string ShouldTypeTextErrorKey
        {
            get => NamespacePrefix + "." + _shouldTypeTextErrorKey;
            set => _shouldTypeTextErrorKey = value;
        }

        private static string _shouldTypeNumberErrorKey = "ShouldTypeNumber";
        /// <summary>
        /// Gets or sets the localization key for the "should type number" error message.
        /// </summary>
        public static string ShouldTypeNumberErrorKey
        {
            get => NamespacePrefix + "." + _shouldTypeNumberErrorKey;
            set => _shouldTypeNumberErrorKey = value;
        }

        private static string _numberBetweenErrorKey = "NumberBetweenErrorKey";
        /// <summary>
        /// Gets or sets the localization key for the "number between" error message.
        /// </summary>
        public static string NumberBetweenErrorKey
        {
            get => NamespacePrefix + "." + _numberBetweenErrorKey;
            set => _numberBetweenErrorKey = value;
        }

        private static string _numberGreaterThanErrorKey = "NumberGreaterThanErrorKey";
        /// <summary>
        /// Gets or sets the localization key for the "number greater than" error message.
        /// </summary>
        public static string NumberGreaterThanErrorKey
        {
            get => NamespacePrefix + "." + _numberGreaterThanErrorKey;
            set => _numberGreaterThanErrorKey = value;
        }

        private static string _numberLowerThanErrorKey = "NumberLowerThanErrorKey";
        /// <summary>
        /// Gets or sets the localization key for the "number lower than" error message.
        /// </summary>
        public static string NumberLessThanErrorKey
        {
            get => NamespacePrefix + "." + _numberLowerThanErrorKey;
            set => _numberLowerThanErrorKey = value;
        }

        private static string _shouldSelectOptionErrorKey = "ShouldSelectOptionErrorKey";
        /// <summary>
        /// Gets or sets the localization key for the "should select option" error message.
        /// </summary>
        public static string ShouldSelectOptionErrorKey
        {
            get => NamespacePrefix + "." + _shouldSelectOptionErrorKey;
            set => _shouldSelectOptionErrorKey = value;
        }

        private static string _shouldBeTrueFalseErrorKey = "ShouldBeTrueFalseErrorKey";
        /// <summary>
        /// Gets or sets the localization key for the "should be True or False" error message.
        /// </summary>
        public static string ShouldBeTrueFalseErrorKey
        {
            get => NamespacePrefix + "." + _shouldBeTrueFalseErrorKey;
            set => _shouldBeTrueFalseErrorKey = value;
        }

        private static string _pathNotFoundErrorKey = "PathNotFoundErrorKey";
        /// <summary>
        /// Gets or sets the localization key for the "path not found" error message.
        /// </summary>
        public static string PathNotFoundErrorKey
        {
            get => NamespacePrefix + "." + _pathNotFoundErrorKey;
            set => _pathNotFoundErrorKey = value;
        }


        private static string _defaultSelectOption = "DefaultSelectOption";
        /// <summary>
        /// Gets or sets the localization key for the "path not found" error message.
        /// </summary>
        public static string DefaultSelectOption
        {
            get => NamespacePrefix + "." + _defaultSelectOption;
            set => _defaultSelectOption = value;
        }
    }
}