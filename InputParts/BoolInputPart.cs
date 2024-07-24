using SKitLs.Data.InputForms.Notations;
using System.Reflection;

namespace SKitLs.Data.InputForms.InputParts
{
    /// <summary>
    /// Represents a boolean input field.
    /// </summary>
    /// <remarks>
    /// This class is used to define a field that accepts a boolean input, typically represented as a flag (yes/no or true/false).
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BoolInputPart"/> class.
    /// </remarks>
    /// <param name="propertyInfo">The property information of the boolean input field.</param>
    /// <param name="meta">The metadata attribute containing information about the boolean input field.</param>
    public class BoolInputPart(PropertyInfo propertyInfo, InputDataBaseAttribute meta) : InputPartBase(propertyInfo, meta)
    {
        /// <summary>
        /// Builds a new instance of <see cref="BoolInputPart"/> based on the provided property information and metadata attribute.
        /// </summary>
        /// <param name="info">The property information of the field to be built.</param>
        /// <param name="meta">The metadata attribute containing information about the input field.</param>
        /// <param name="form">The object form that contains the input field.</param>
        /// <returns>A new instance of <see cref="BoolInputPart"/>.</returns>
        public static InputPartBase BuildSelf(PropertyInfo info, InputDataBaseAttribute meta, object form) => new BoolInputPart(info, meta);
    }
}