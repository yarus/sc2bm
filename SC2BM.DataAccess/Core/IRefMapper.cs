namespace SC2BM.DataAccess.Core
{
    /// <summary>
    /// Interface for mappers by ref.
    /// </summary>
    /// <typeparam name="T">
    /// Type for filling from data reader.
    /// </typeparam>
    public interface IRefMapper<T>
    {
        void Fill(DataReaderAdapter adapter, ref T target);
    }
}