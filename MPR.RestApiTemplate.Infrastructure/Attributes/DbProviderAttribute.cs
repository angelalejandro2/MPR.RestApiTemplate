namespace MPR.RestApiTemplate.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class DbProviderAttribute(DbProvider provider) : Attribute
    {
        public DbProvider Provider { get; } = provider;
    }

    public enum DbProvider
    {
        SqlServer,
        Oracle
    }
}
