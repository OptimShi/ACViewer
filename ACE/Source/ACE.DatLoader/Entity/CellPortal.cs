using System.IO;
using ACE.Entity.Enum;

namespace ACE.DatLoader.Entity
{
    public class CellPortal : IUnpackable
    {
        public PortalFlags Flags { get; private set; }
        public ushort PolygonId { get; private set; }
        public uint OtherCellId { get; private set; }
        public uint OtherPortalId { get; private set; }

        public bool ExactMatch => (Flags & PortalFlags.ExactMatch) != 0;
        public bool PortalSide => (Flags & PortalFlags.PortalSide) == 0;

        public void Unpack(BinaryReader reader)
        {
            switch (DatManager.DatVersion)
            {
                case DatVersionType.DM:
                    PolygonId = reader.ReadUInt16();
                    Flags = (PortalFlags)reader.ReadUInt16();
                    OtherCellId = reader.ReadUInt32();
                    OtherPortalId = reader.ReadUInt32();
                    reader.ReadUInt32(); // Unknown
                    break;
                case DatVersionType.TOD:
                    Flags = (PortalFlags)reader.ReadUInt16();
                    PolygonId = reader.ReadUInt16();
                    OtherCellId = reader.ReadUInt16();
                    OtherPortalId = reader.ReadUInt16();
                    break;
            }
        }
    }
}
