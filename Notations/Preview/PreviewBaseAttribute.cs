using SKitLs.Utils.Localizations.Model;

namespace SKitLs.Data.InputForms.Notations.Preview
{
    internal abstract class PreviewBaseAttribute : Attribute
    {
        public abstract LocalSet? PreviewInput(object? input);
    }

    internal abstract class PreviewBaseAttribute<T> : PreviewBaseAttribute
    {
        public override LocalSet? PreviewInput(object? input)
        {
            if (input is T required)
            {
                return PreviewInput(required);
            }
            throw new NotSupportedException($"Preview of a type {typeof(T).Name} can't handle type of {input.GetType().Name}");
        }

        public abstract LocalSet? PreviewInput(T input);
    }
}