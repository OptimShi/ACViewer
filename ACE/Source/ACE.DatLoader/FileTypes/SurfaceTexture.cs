using ACE.Common;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.SurfaceTexture)]
    public class SurfaceTexture : FileType
    {
        // public int Id { get; private set; }
        public int Unknown { get; private set; }
        public byte UnknownByte { get; private set; }
        // public int TextureCount { get; private set; }
        public List<uint> Textures { get; private set; } = new List<uint>(); // These values correspond to a Surface (0x06) entry


        // Pre TOD Properties
        public int Width;
        public int Height;
        public int Length;
        public uint? DefaultPaletteId;
        public byte[] SourceData { get; set; }
        public SurfacePixelFormat Format; // Only INDEX8 => 1268 and ARGB4444 => 7 in Preview2

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            if (DatManager.DatVersion == DatVersionType.TOD)
            {
                Unknown = reader.ReadInt32();
                UnknownByte = reader.ReadByte();
                Textures.Unpack(reader);
            }
            else
            {
                Format = (SurfacePixelFormat)reader.ReadInt32();

                Width = reader.ReadInt32();
                Height = reader.ReadInt32();
                SourceData = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
                switch (Format)
                {
                    case SurfacePixelFormat.INDEX8:
                        reader.BaseStream.Position = reader.BaseStream.Length - 4; // move position back 4 bytes
                        DefaultPaletteId = reader.ReadUInt32();
                        break;
                }

            }
        }

        public Texture ConvertToTexture()
        {
            Texture tex = new Texture();
            tex.SetId(Id);
            tex.Width = Width;
            tex.Height = Height;
            tex.SourceData = SourceData;
            tex.Format = Format;
            switch (Format)
            {
                case SurfacePixelFormat.INDEX8:
                    tex.DefaultPaletteId = DefaultPaletteId;
                    tex.Length = Width * Height * 8 - 4;
                    break;
                case SurfacePixelFormat.COLOR_SEP:
                    tex.Length = Width * Height * 3;
                    break;
                case SurfacePixelFormat.ARGB4444:
                    tex.Length = Width * Height * 4;
                    break;
            }
            tex.ConvertTextureFormat();
            return tex;
        }

    }
}
