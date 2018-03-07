namespace SC2BM.DataAccess.Core
{
    /// <summary>
    /// Fills fields of specified entity.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity
    /// </typeparam>
    public abstract class BaseMapper<T> : IMapper<T>
    {
        #region Methods

        /// <summary>
        /// Fills fields of specified entity.
        /// </summary>
        /// <param name="adapter">
        /// The source of date.
        /// </param>
        /// <param name="target">
        /// The target entity to fill.
        /// </param>
        public virtual void Fill(DataReaderAdapter adapter, T target)
        {
            Fill(adapter, ref target);
        }

        public virtual void Fill(DataReaderAdapter adapter, ref T target)
        {
        }

        #endregion

        #region Helper methods

        #endregion
    }
}