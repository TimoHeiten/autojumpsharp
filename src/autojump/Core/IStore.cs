
namespace autojump.Core;

/// <summary>
/// Access to the underlying store. Inferface is just for IoC / testing in this case. leaks heavily the underlying sqlite store, which is fine for now
/// </summary>
public interface IStore
{
    /// <summary>
    /// create a new store. Idempotent
    /// </summary>
    void Touch();
    /// <summary>
    /// Upsert (insert / update) a path into the store
    /// </summary>
    /// <param name="path"></param>
    void Bump(string path);

    /// <summary>
    /// all values currently in the store, with last_accessed and count
    /// </summary>
    /// <returns></returns>
    string List();
    /// <summary>
    /// if part of the args is a match, return the path
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    string? Lookup(IEnumerable<string> args);
}