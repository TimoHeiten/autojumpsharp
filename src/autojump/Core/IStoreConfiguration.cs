namespace autojump.Core;

public interface IStoreConfiguration
{
    /// <summary>
    /// The path to the actual physical sqlite File
    /// </summary>
    /// <returns></returns>
    string ConnectionPath();

    /// <summary>
    /// The name of the database for the connection string
    /// </summary>
    /// <returns></returns>
    string DatabaseName();
}