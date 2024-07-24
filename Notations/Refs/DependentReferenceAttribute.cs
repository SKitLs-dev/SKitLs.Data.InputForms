namespace SKitLs.Data.InputForms.Notations.Refs
{
    /// <summary>
    /// Represents an attribute that defines a dependent reference between fields.
    /// This attribute is used to specify which fields (dependents/slaves) are affected when a specified field (master) is updated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DependentReferenceAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether to ignore failed previews when master value is updated.
        /// </summary>
        public bool IgnoreFailedPreview { get; set; }

        /// <summary>
        /// Gets or sets the name of the method to be called when the master reference is updated.
        /// </summary>
        public string ReferenceUpdatedMethodName { get; set; }

        /// <summary>
        /// Gets the list of dependent properties names.
        /// </summary>
        public List<string> Dependents { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependentReferenceAttribute"/> class with the specified reference update method name,
        /// a flag indicating whether to ignore failed previews, and a list of dependent field names.
        /// </summary>
        /// <param name="referenceUpdatedMethodName">The name of the method to be called when the reference is updated.</param>
        /// <param name="ignoreFailedPreview">Indicates whether to ignore failed previews when resolving dependencies. Default is true.</param>
        /// <param name="refs">The list of dependent field names.</param>
        /// <exception cref="ArgumentException">Thrown when no dependent field names are provided.</exception>
        public DependentReferenceAttribute(string referenceUpdatedMethodName, bool ignoreFailedPreview = true, params string[] refs)
        {
            IgnoreFailedPreview = ignoreFailedPreview;
            ReferenceUpdatedMethodName = referenceUpdatedMethodName;

            if (refs.Length == 0)
                throw new ArgumentException("At least one dependent field must be specified.", nameof(refs));
            Dependents = new List<string>(refs);
        }

        /// <summary>
        /// Resolves the callback reference method or object.
        /// </summary>
        /// <returns>The name of the method to be called when the reference is updated.</returns>
        public virtual object ResolveCallbackReference() => ReferenceUpdatedMethodName;
    }
}