using SKitLs.Data.InputForms.Notations;
using System.Reflection;

namespace SKitLs.Data.InputForms.InputParts
{
    /// <summary>
    /// Represents an input part for handling browse input values.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BrowseInputPart"/> class with the specified property information, metadata, and form.
    /// </remarks>
    /// <param name="info">The property information of the browse input field.</param>
    /// <param name="meta">The metadata attribute containing information about the browse input field.</param>
    public class BrowseInputPart(PropertyInfo info, BrowseDataAttribute meta) : InputPartBase(info, meta)
    {
        /// <summary>
        /// Gets the source attribute containing metadata for the browse input.
        /// </summary>
        public BrowseDataAttribute SourceAttribute { get; set; } = meta;

        /// <summary>
        /// Gets the title of the browser window.
        /// </summary>
        public string? BrowserTitle => SourceAttribute.BrowserTitle;

        /// <summary>
        /// Gets the file filter for the browser.
        /// </summary>
        public string? Filter => SourceAttribute.Filter;

        /// <summary>
        /// Gets the initial directory for the browser.
        /// </summary>
        public string? InitialDirectory => SourceAttribute.InitialDirectory;

        /// <summary>
        /// Builds a new instance of <see cref="BrowseInputPart"/> based on the provided property information and metadata attribute.
        /// </summary>
        /// <param name="info">The property information of the field to be built.</param>
        /// <param name="meta">The metadata attribute containing information about the input field.</param>
        /// <param name="form">The object form that contains the input field.</param>
        /// <returns>A new instance of <see cref="BrowseInputPart"/>.</returns>
        public static InputPartBase BuildSelf(PropertyInfo info, InputDataBaseAttribute meta, object form)
        {
            // TODO
            var browse = (meta as BrowseDataAttribute) ?? throw new NotImplementedException();
            return new BrowseInputPart(info, browse);
        }
    }
}