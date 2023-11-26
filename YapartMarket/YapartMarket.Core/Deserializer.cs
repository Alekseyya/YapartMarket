using System;

namespace YapartMarket.Core
{
    public abstract class Deserializer<T>
    {
        protected Deserializer()
        {
        }

        public T Deserialize(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            return DeserializeCore(data);
        }
        protected abstract T DeserializeCore(string data);
    }
}
