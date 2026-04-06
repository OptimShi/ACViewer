using System;
using System.IO;

namespace ACE.DatLoader
{
    public class DatDirectoryHeader : IUnpackable
    {
        //internal const uint ObjectSize = ((sizeof(uint) * 0x3E) + sizeof(uint) + (DatFile.ObjectSize * 0x3D));

        public uint[] Branches { get; private set; }
        public uint EntryCount { get; private set; }
        public DatFile[] Entries { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            var branchSize = DatManager.GetBranchSize();

            Branches = new uint[branchSize];

            for (int i = 0; i < Branches.Length; i++)
                Branches[i] = reader.ReadUInt32();

            EntryCount = reader.ReadUInt32();

            Entries = new DatFile[EntryCount];

            for (int i = 0; i < EntryCount; i++)
            {
                Entries[i] = new DatFile();
                Entries[i].Unpack(reader);
            }
        }

        /// <summary>
        /// Different dat versions have different branch sizes, and this different Object Sizes
        /// </summary>
        /// <returns></returns>
        public uint GetObjectSize()
        {
            if(DatManager.DatVersion == DatVersionType.DM && DatManager.Iteration <= 8)
            {
                return 0x400;
            }
            var branchSize = (uint)DatManager.GetBranchSize();
            return ((sizeof(uint) * branchSize) + sizeof(uint) + (DatFile.ObjectSize * (branchSize - 1)));
        }
    }
}
