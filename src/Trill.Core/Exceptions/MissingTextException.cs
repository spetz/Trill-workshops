namespace Trill.Core.Exceptions
{
    public class MissingTextException : DomainException
    {
        public override string Code { get; } = "missing_text";

        public MissingTextException() : base("Missing text.")
        {
        }
    }
}