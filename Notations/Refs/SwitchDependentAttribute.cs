using SKitLs.Data.InputForms.InputParts;

namespace SKitLs.Data.InputForms.Notations.Refs
{
    /// <summary>
    /// Represents an attribute that defines a dependent reference with a switch mechanism.
    /// This attribute is used to lock or unlock a dependent field based on the value of a master field.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SwitchDependentAttribute"/> class with the specified reference update method name,
    /// a flag indicating whether to ignore failed previews, and a list of dependent field names.
    /// </remarks>
    /// <param name="referenceUpdatedMethodName">The name of the method to be called when the reference is updated.</param>
    /// <param name="ignoreFailedPreview">Indicates whether to ignore failed previews when resolving dependencies. Default is true.</param>
    /// <param name="refs">The list of dependent field names.</param>
    [AttributeUsage(AttributeTargets.Property)]
    public class SwitchDependentAttribute(string referenceUpdatedMethodName, bool ignoreFailedPreview = true, params string[] refs) : DependentReferenceAttribute(referenceUpdatedMethodName, ignoreFailedPreview, refs)
    {
        /// <summary>
        /// Switches the state of the dependent field based on the value of the master field.
        /// </summary>
        /// <param name="slave">The dependent field.</param>
        /// <param name="master">The master field.</param>
        public virtual void SwitchDependent(InputPartBase master, InputPartBase slave)
        {
            if (master is BoolInputPart switcher)
                slave.Locked = (bool)switcher.GetInputValue()!;
        }

        // TODO
        /// <summary>
        /// Resolves the callback reference method for switching the dependent field.
        /// </summary>
        /// <returns>The method info for the <see cref="SwitchDependent"/> method.</returns>
        /// <exception cref="NullReferenceException">Thrown when the method info cannot be resolved.</exception>
        public override object ResolveCallbackReference() => GetType().GetMethod(nameof(SwitchDependent)) ?? throw new NullReferenceException();
    }
}