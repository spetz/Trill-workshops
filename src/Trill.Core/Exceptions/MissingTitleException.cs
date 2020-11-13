namespace Trill.Core.Exceptions
{
    public class MissingTitleException : DomainException
    {
        public override string Code { get; } = "missing_title";

        public MissingTitleException() : base("Missing title.")
        {
        }
    }
}