namespace SC2BM.DomainModel.Routine
{
    public class NameValue<TName, TValue>
    {
        private readonly TName _name;
        private readonly TValue _value;

        public NameValue(TName name, TValue value)
        {
            _name = name;
            _value = value;
        }

        public TName Name
        {
            get { return _name; }
        }

        public TValue Value
        {
            get { return _value; }
        }
    }
}