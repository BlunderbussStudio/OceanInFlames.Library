using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Helpers.Extensions
{

    public static class DirectoryInfoExtensions
    {
        public static void MkDirP(this DirectoryInfo directory)
        {
            if (directory.Exists) return;
            directory.Parent.MkDirP();
            directory.Create();
        }

        public static void Archive(this DirectoryInfo directoryInfo, Stream stream)
        {
            var writer = new BinaryWriter(stream);
            Archive(directoryInfo, writer);
        }
        static void Archive(DirectoryInfo directoryInfo, BinaryWriter writer)
        {
            var directories = directoryInfo.GetDirectories().OrderBy(d => d.Name).ToList();
            writer.Write(directories.Count);
            foreach (var child in directories)
            {
                writer.Write(child.Name);
                Archive(child, writer);
            }
            var files = directoryInfo.GetFiles().OrderBy(f => f.Name).ToList();
            writer.Write(files.Count);
            foreach (var file in files)
            {
                writer.Write(file.Name);
                Archive(file, writer);
            }
        }
        static void Archive(FileInfo fileInfo, BinaryWriter writer)
        {
            var length = fileInfo.Length;
            writer.Write((ulong)length);

            using (var stream = fileInfo.OpenRead())
            {
                var reader = new BinaryReader(stream);
                var cursor = 0L;
                while (cursor < length)
                {
                    var writeLength = (int)Math.Min(1 << 16, length - cursor);
                    var bytes = reader.ReadBytes(writeLength);
                    writer.Write(bytes);
                    cursor += writeLength;
                }
            }
        }

        public static void RestoreArchive(this DirectoryInfo directoryInfo, Stream stream)
        {
            if (directoryInfo.Exists)
            {
                directoryInfo.Delete(true);
            }
            var reader = new BinaryReader(stream);
            RestoreArchive(directoryInfo, reader);
        }
        static void RestoreArchive(DirectoryInfo directoryInfo, BinaryReader reader)
        {
            if (directoryInfo.Exists) throw new IOException("Directory already exists: " + directoryInfo.FullName);
            directoryInfo.Create();
            var directoryCount = reader.ReadUInt32();
            for (var _ = 0UL; _ < directoryCount; _++)
            {
                var name = reader.ReadString();
                var childInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, name));
                RestoreArchive(childInfo, reader);
            }
            var fileCount = reader.ReadUInt32();
            for (var _ = 0UL; _ < fileCount; _++)
            {
                var name = reader.ReadString();
                var fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, name));
                RestoreArchive(fileInfo, reader);
            }
        }
        static void RestoreArchive(FileInfo fileInfo, BinaryReader reader)
        {
            var length = (long)reader.ReadUInt64();

            using (var fileStream = fileInfo.Create())
            {
                var cursor = 0L;
                while (cursor < length)
                {
                    var readLength = (int)Math.Min(1 << 16, length - cursor);
                    var bytes = reader.ReadBytes(readLength);
                    fileStream.Write(bytes);
                    cursor += readLength;
                }
            }
        }
    }
}
