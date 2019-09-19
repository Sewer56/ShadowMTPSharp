using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using MTPLib.Structs;
using Reloaded.Memory;
using Reloaded.Memory.Streams;
using String = MTPLib.Util.String;

namespace MTPLib
{
    public unsafe class MotionPackage
    {
        private const string PropertiesFileName = "Properties.json";
        private const string MotionExtension = "MTN";

        /// <summary>
        /// Retrieves a native header for this package.
        /// </summary>
        public MotionPackageHeader Header => MotionPackageHeader.FromPackage(this);

        /// <summary>
        /// Contains all of the file entries for this archive.
        /// </summary>
        public ManagedAnimationEntry[] Entries { get; set; }

        public MotionPackage() { }

        /// <summary>
        /// Creates a <see cref="MotionPackage"/> given a list of file entries.
        /// </summary>
        public MotionPackage(ManagedAnimationEntry[] entries)
        {
            Entries = entries;
        }

        /// <summary>
        /// Extracts the current <see cref="MotionPackage"/> and all associated properties to a directory.
        /// </summary>
        /// <param name="directoryPath">The directory to extract to.</param>
        public void ToDirectory(string directoryPath)
        {
            var animationProperties = new MotionPackageAnimationProperties(this);
            var fullDirectoryPath   = Path.GetFullPath(directoryPath);
            var propertiesPath      = Path.Combine(fullDirectoryPath, PropertiesFileName);

            File.WriteAllBytes(propertiesPath, animationProperties.ToJson());
            foreach (var entry in Entries)
            {
                var filePath = Path.Combine(fullDirectoryPath, $"{entry.FileName}.{MotionExtension}");
                File.WriteAllBytes(filePath, entry.FileData);
            }
        }

        /// <summary>
        /// Imports a <see cref="MotionPackage"/> from a directory.
        /// </summary>
        /// <param name="directoryPath">The directory to import from.</param>
        public static MotionPackage FromDirectory(string directoryPath)
        {
            var fullDirectoryPath   = Path.GetFullPath(directoryPath);
            var propertiesPath      = Path.Combine(fullDirectoryPath, PropertiesFileName);

            if (!File.Exists(propertiesPath))
                throw new FileNotFoundException($"{propertiesPath} does not exist.");

            var properties = MotionPackageAnimationProperties.FromJson(propertiesPath);
            var files      = Directory.GetFiles(fullDirectoryPath, $"*.{MotionExtension}");

            var animations = files.Select(x =>
            {
                var bytes = File.ReadAllBytes(x);
                var fileName = Path.GetFileNameWithoutExtension(x);
                
                return properties.Files.ContainsKey(fileName) ? new ManagedAnimationEntry(fileName, bytes, properties.Files[fileName]) 
                                                              : new ManagedAnimationEntry(fileName, bytes, null);
            });

            return new MotionPackage(animations.ToArray());
        }

        /// <summary>
        /// Converts a managed array into a <see cref="MotionPackage"/>
        /// </summary>
        /// <param name="data">An array containing the entire MTP file.</param>
        public static MotionPackage FromMtp(byte[] data)
        {
            fixed (byte* dataPtr = data)
            {
                return FromMtp(dataPtr, data.Length);
            }
        }

        /// <summary>
        /// Converts a native array into a <see cref="MotionPackage"/>
        /// </summary>
        /// <param name="data">An array containing the entire MTP file.</param>
        /// <param name="sizeOfData">Size of the provided array.</param>
        public static MotionPackage FromMtp(byte* data, int sizeOfData)
        {
            var entries       = new List<ManagedAnimationEntry>();

            using (var stream = new UnmanagedMemoryStream(data, sizeOfData))
            using (var streamReader = new BufferedStreamReader(stream, 8192))
            {
                streamReader.ReadBigEndianStruct(out MotionPackageHeader header);
                streamReader.Seek(header.EntryOffset, SeekOrigin.Begin);

                for (int x = 0; x < header.NumberOfFiles; x++)
                {
                    streamReader.ReadBigEndianStruct(out AnimationEntry animationEntry);
                    entries.Add(ManagedAnimationEntry.FromAnimationEntry(data, animationEntry));
                }

                return new MotionPackage(entries.ToArray());
            }
        }

        /// <summary>
        /// Converts a <see cref="MotionPackage"/> into a native .MTP file.
        /// </summary>
        public byte[] ToMtp()
        {
            var bytes   = new List<byte>(1000000);

            var header  = Header;
            header.SwapEndian();
            bytes.AddRange(Struct.GetBytes(ref header));

            // Write entries
            var dummyAnimationEntry = new AnimationEntry();
            var dummyAnimationEntryBytes = Struct.GetBytes(ref dummyAnimationEntry);
            int[] entryOffsets = Entries.Select(x => AddRange(bytes, dummyAnimationEntryBytes)).ToArray();

            // Write file names.
            int[] fileNameOffsets = Entries.Select(x => AddRange(bytes, String.GetNullTerminatedBytes(String.Win1252Encoder, x.FileName))).ToArray();

            // Write file data.
            int[] fileDataOffsets = Entries.Select(x => AddRange(bytes, x.FileData)).ToArray();

            // Write extra properties.
            int[] filePropertyOffsets = Entries.Select(x =>
            {
                if (x.Tuples != null)
                {
                    // Temporarily swap out the endian of all tuples before writing to array, then swap back.
                    for (int i = 0; i < x.Tuples.Length; i++)
                        x.Tuples[i].SwapEndian();

                    var result = AddRange(bytes, StructArray.GetBytes(x.Tuples));

                    for (int i = 0; i < x.Tuples.Length; i++)
                        x.Tuples[i].SwapEndian();

                    return result;
                }

                return 0;
            }).ToArray();

            // Fix Offsets
            var byteArray = bytes.ToArray();
            fixed (byte* byteArrayPtr = byteArray)
            {
                for (int x = 0; x < Entries.Length; x++)
                {
                    ref var entry = ref Unsafe.AsRef<AnimationEntry>(byteArrayPtr + entryOffsets[x]);
                    Endian.Reverse(ref fileNameOffsets[x]);
                    Endian.Reverse(ref fileDataOffsets[x]);
                    Endian.Reverse(ref filePropertyOffsets[x]);

                    entry.FileNamePtr = fileNameOffsets[x];
                    entry.FileDataPtr = fileDataOffsets[x];
                    entry.PropertyTuplePtr = filePropertyOffsets[x];
                }
            }

            return byteArray;
        }

        /// <summary>
        /// Adds a set of items onto the list and returns the offset of the first added item.
        /// </summary>
        private int AddRange<TItem>(List<TItem> list, IEnumerable<TItem> item)
        {
            var position = list.Count;
            list.AddRange(item);
            return position;
        }

        /* Implemented by R# */
        protected bool Equals(MotionPackage other) => Entries.SequenceEqual(other.Entries);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MotionPackage)obj);
        }

        public override int GetHashCode()
        {
            return (Entries != null ? Entries.GetHashCode() : 0);
        }
    }
}
