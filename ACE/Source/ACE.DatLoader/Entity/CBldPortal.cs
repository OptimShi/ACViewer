using System.Collections.Generic;
using System.IO;
using ACE.Entity.Enum;

namespace ACE.DatLoader.Entity
{
    public class CBldPortal : IUnpackable
    {
        public PortalFlags Flags { get; private set; }

        // Not sure what these do. They are both calculated from the flags.
        public bool ExactMatch => Flags.HasFlag(PortalFlags.ExactMatch);
        public bool PortalSide => Flags.HasFlag(PortalFlags.PortalSide);

        // Basically the cells that connect both sides of the portal
        public uint OtherCellId { get; private set; }
        public uint OtherPortalId { get; private set; }

        /// <summary>
        /// List of cells used in this structure. (Or possibly just those visible through it.)
        /// </summary>
        public List<uint> StabList { get; } = new List<uint>();

        public void Unpack(BinaryReader reader)
        {
            if (DatManager.DatVersion == DatVersionType.DM)
            {
                Flags = (PortalFlags)reader.ReadUInt32();
                OtherCellId = reader.ReadUInt32();
                OtherPortalId = reader.ReadUInt32();
                /* unknown = */ reader.ReadUInt32();
                uint num_stabs = reader.ReadUInt32();
                for (var i = 0; i < num_stabs; i++)
                    StabList.Add(reader.ReadUInt32());
            }
            else
            {
                Flags = (PortalFlags)reader.ReadUInt16();
                OtherCellId = reader.ReadUInt16();
                OtherPortalId = reader.ReadUInt16();
                ushort num_stabs = reader.ReadUInt16();
                for (var i = 0; i < num_stabs; i++)
                    StabList.Add(reader.ReadUInt16());
            }

            reader.AlignBoundary();
        }
    }
}
