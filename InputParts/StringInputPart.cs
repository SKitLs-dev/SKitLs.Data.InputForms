using SKitLs.Data.InputForms.Notations;
using System.Reflection;

namespace SKitLs.Data.InputForms.InputParts
{
    /// <summary>
    /// Represents an input part for handling string input values.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="StringInputPart"/> class with the specified property information and metadata.
    /// </remarks>
    /// <param name="propertyInfo">The <see cref="PropertyInfo"/> associated with this input part.</param>
    /// <param name="meta">The <see cref="InputDataBaseAttribute"/> providing metadata for this input part.</param>
    public class StringInputPart(PropertyInfo propertyInfo, InputDataBaseAttribute meta) : InputPartBase(propertyInfo, meta)
    {
        /// <summary>
        /// Builds a new instance of <see cref="StringInputPart"/> based on the provided property information and metadata attribute.
        /// </summary>
        /// <param name="info">The property information of the field to be built.</param>
        /// <param name="meta">The metadata attribute containing information about the input field.</param>
        /// <param name="form">The object form that contains the input field.</param>
        /// <returns>A new instance of <see cref="StringInputPart"/>.</returns>
        public static InputPartBase BuildSelf(PropertyInfo info, InputDataBaseAttribute meta, object form) => new StringInputPart(info, meta);
    }
}