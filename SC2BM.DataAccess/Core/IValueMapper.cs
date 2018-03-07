namespace SC2BM.DataAccess.Core
{
    /// <summary>
    /// Interface for mappers.
    /// </summary>
    /// <typeparam name="T">
    /// Type for filling from data reader.
    /// </typeparam>
    public interface IValueMapper<T>
    {
        T GetValue(DataReaderAdapter adapter);
    }
}