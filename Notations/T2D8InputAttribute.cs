using SKitLs.Utils.Localizations.Model;
using System.Globalization;

namespace SKitLs.Data.InputForms.Notations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class T2D8InputAttribute(string formatTemplate = "dd.MM.yyyy") : TextInputAttribute()
    {
        /// <summary>
        /// <see href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings">Microsoft Documentation</see>
        /// </summary>
        public string FormatTemplate { get; set; } = formatTemplate;

        /// <inheritdoc/>
        public override LocalSet? DefaultPreview(object? input)
        {
            var error = base.DefaultPreview(input);
            if (error is null)
            {
                var str = (string)input!;
                if (string.IsNullOrEmpty(str) && !Required)
                    return null;

                // TODO
                if (!DateTime.TryParseExact(str, FormatTemplate, CultureInfo.InvariantCulture, DateTimeStyles.None, out var _))
                    return $"Should Enter Date in format '{FormatTemplate}'";
                return null;
            }
            else return error;
        }

        /// <inheritdoc/>
        public override object? ValueBuilder(object? input)
        {
            if (input is string str)
            {
                if (DateTime.TryParseExact(str, FormatTemplate, CultureInfo.InvariantCulture, DateTimeStyles.None, out var value))
                    return value;
            }
            return base.ValueBuilder(input);
        }
    }
}