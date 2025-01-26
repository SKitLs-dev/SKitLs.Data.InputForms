using SKitLs.Utils.Localizations.Model;

namespace SKitLs.Data.InputForms.Notations
{
    /// <summary>
    /// Represents metadata for a browse input field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class BrowseDataAttribute : TextInputAttribute
    {
        /// <summary>
        /// Gets or sets the title of the browser window.
        /// </summary>
        public string? BrowserTitle { get; set; }

        /// <summary>
        /// Gets or sets the file filter for the browser.
        /// </summary>
        public string? Filter { get; set; }

        /// <summary>
        /// Gets or sets the initial directory for the browser.
        /// </summary>
        public string? InitialDirectory { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseDataAttribute"/> class with the specified caption, description, and optional parameters.
        /// </summary>
        /// <param name="caption">The caption of the browse input field.</param>
        /// <param name="description">The description of the browse input field.</param>
        /// <param name="filter">The file filter for the browser.</param>
        /// <param name="required">Indicates whether the field is necessary.</param>
        /// <param name="initialDirectory">The initial directory for the browser.</param>
        /// <param name="browserTitle">The title of the browser window.</param>
        /// <param name="previewMethodName">The name of the method to use for previewing input.</param>
        public BrowseDataAttribute(string caption, string description, string? filter = "All files (*.*)|*.*", bool required = true, string? initialDirectory = null, string? browserTitle = null, string? previewMethodName = null) : base(caption, description, required, previewMethodName)
        {
            Filter = filter;
            InitialDirectory = initialDirectory;
            BrowserTitle = browserTitle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseDataAttribute"/> class with the specified properties.
        /// </summary>
        /// <param name="filter">The file filter for the browser. Defaults to "All files (*.*)|*.*".</param>
        /// <param name="required">Indicates whether the field is necessary. Defaults to true.</param>
        /// <param name="initialDirectory">The initial directory for the browser. Can be null.</param>
        /// <param name="browserTitle">The title of the browser window. Can be null.</param>
        /// <param name="previewMethodName">The name of the method to use for previewing input. Can be null.</param>
        public BrowseDataAttribute(string? filter = "All files (*.*)|*.*", bool required = true, string? initialDirectory = null, string? browserTitle = null, string? previewMethodName = null) : base(required, previewMethodName)
        {
            Filter = filter;
            InitialDirectory = initialDirectory;
            BrowserTitle = browserTitle;
        }

        /// <inheritdoc/>
        public override LocalSet? DefaultPreview(object? input)
        {
            var error = base.DefaultPreview(input);
            if (error is null)
            {
                var str = (string)input!;
                if (!Path.Exists(str))
                {
                    return Locals.PathNotFoundErrorKey;
                }
                return null;
            }
            else return error;
        }
    }
}