using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MTPLib.Structs;
using Reloaded.Messaging.Interfaces;
using Reloaded.Messaging.Interfaces.Message;
using Reloaded.Messaging.Serializer.SystemTextJson;

namespace MTPLib
{
    public class MotionPackageAnimationProperties : ISerializable
    {
        private static SystemTextJsonSerializer _serializer = new SystemTextJsonSerializer(new JsonSerializerOptions() { WriteIndented = true });

        /// <summary>
        /// Listing of all the files and their properties.
        /// </summary>
        public Dictionary<string, PropertyTuple[]> Files { get; set; } = new Dictionary<string, PropertyTuple[]>();

        /// <summary>
        /// Serialization use only.
        /// </summary>
        public MotionPackageAnimationProperties() { }

        /// <summary>
        /// Creates the properties structure from the MotionPackage.
        /// </summary>
        public MotionPackageAnimationProperties(MotionPackage package)
        {
            foreach (var entry in package.Entries)
            {
                if (entry.Tuples != null)
                    Files[entry.FileName] = entry.Tuples;
            }
        }

        public static MotionPackageAnimationProperties FromJson(string filePath) => (MotionPackageAnimationProperties) Serializable.Deserialize<MotionPackageAnimationProperties>(File.ReadAllBytes(filePath));
        public void ToJson(string filePath) => File.WriteAllBytes(filePath, Serializable.Serialize(this));
        public byte[] ToJson() => Serializable.Serialize(this);

        public ISerializer GetSerializer() => _serializer;
        public ICompressor GetCompressor() => null;
    }
}
