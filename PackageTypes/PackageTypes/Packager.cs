using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace PackageTypes
{
    public static class Packager
    {
        /// Convert an object to a byte array
        public static byte[] SerializeToBytes(this object source)
        {
            try
            {
                var formatter = new BinaryFormatter();
                var stream = new MemoryStream();
                formatter.Serialize(stream, source);

                return stream.ToArray();
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
        }

        // Convert a byte array to an Object
        public static T DeserializeFromBytes<T>(this byte[] source)
        {
            try
            {
                MemoryStream stream = new MemoryStream(source);
                BinaryFormatter formatter = new BinaryFormatter();
                object obj = formatter.Deserialize(stream);
                stream.Flush();
                stream.Close();
                stream.Dispose();
                return (T) obj;
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
        }
    }
}