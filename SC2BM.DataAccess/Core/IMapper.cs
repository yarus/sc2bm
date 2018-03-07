namespace SC2BM.DataAccess.Core
{
    /// <summary>
    /// Interface for mappers.
    /// </summary>
    /// <typeparam name="T">
    /// Type for filling from data reader.
    /// </typeparam>
    public interface IMapper<in T>
    {
        void Fill(DataReaderAdapter adapter, T target);
    }
}