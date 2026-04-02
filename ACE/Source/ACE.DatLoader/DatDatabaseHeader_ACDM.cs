using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ACE.DatLoader
{
    /// <summary>
    /// DiskFileInfo_t in the client
    /// </summary>
    public class DatDatabaseHeader_ACDM : IUnpackable
    {
        public uint FileType { get; private set; }
        public uint BlockSize { get; private set; }
        public uint FileSize { get; private set; }
        public int Iteration { get; private set; }

        public uint FreeHead { get; private set; }
        public uint FreeTail { get; private set; }
        public uint FreeCount { get; private set; }
        public uint BTree { get; private set; }

        // These are probably not the correct variable names...
        public uint NewLRU { get; private set; }
        public uint OldLRU { get; private set; }
        public bool UseLRU { get; private set; }

        public bool Success = false;


        public void Unpack(BinaryReader reader)
        {
            FileType = reader.ReadUInt32();
            if (FileType != 0x5442) // 'TB';
                return;

            BlockSize = reader.ReadUInt32();
            FileSize = reader.ReadUInt32();
            Iteration = reader.ReadInt32();

            FreeHead = reader.ReadUInt32();
            FreeTail = reader.ReadUInt32();
            FreeCount = reader.ReadUInt32();
            BTree = reader.ReadUInt32();

            NewLRU = reader.ReadUInt32();
            OldLRU = reader.ReadUInt32();
            UseLRU = (reader.ReadUInt32() == 1);

            Success = true;
        }

    }
}
