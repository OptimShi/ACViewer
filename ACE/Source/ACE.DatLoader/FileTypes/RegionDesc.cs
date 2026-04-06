using ACE.DatLoader.Entity;
using log4net.Util;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// This is the client_portal.dat file starting with 0x13 -- There is only one of these, which is why REGION_ID is a constant.
    /// </summary>
    [DatFileType(DatFileType.Region)]
    public class RegionDesc : FileType
    {
        internal const uint FILE_ID = 0x13000000; // Post ToD, this is the only Region. Pre, it's got the PalShift render data
        internal const uint HW_FILE_ID = 0x130F0000; // Equivalent to the ToD Region

        public uint RegionNumber { get; private set; }
        public uint Version { get; private set; }
        public string RegionName { get; private set; }

        public LandDefs LandDefs { get; } = new LandDefs();
        public GameTime GameTime { get; } = new GameTime();

        public uint PartsMask { get; private set; }

        public SkyDesc SkyInfo { get; } = new SkyDesc();
        public SoundDesc SoundInfo { get; } = new SoundDesc();
        public SceneDesc SceneInfo { get; } = new SceneDesc();
        public TerrainDesc TerrainInfo { get; } = new TerrainDesc();
        public RegionMisc RegionMisc { get; } = new RegionMisc();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            RegionNumber    = reader.ReadUInt32();
            Version         = reader.ReadUInt32();
            RegionName      = reader.ReadPString(); // "Dereth", "Lands of Dereth" in ACDM
            reader.AlignBoundary();

            LandDefs.Unpack(reader);
            GameTime.Unpack(reader);

            PartsMask = reader.ReadUInt32();

            if ((PartsMask & 0x10) != 0)
                SkyInfo.Unpack(reader);

            if ((PartsMask & 0x01) != 0)
                SoundInfo.Unpack(reader);

            if ((PartsMask & 0x02) != 0)
                SceneInfo.Unpack(reader);

            TerrainInfo.Unpack(reader);

            if ((PartsMask & 0x0200) != 0)
                RegionMisc.Unpack(reader);
        }

        public static float GetLandHeight(int idx)
        {
            if(DatManager.PortalDat.RegionDesc.LandDefs == null)
                return (idx * 2); // No landheight table, so this is just a guess

            var lh = DatManager.PortalDat.RegionDesc.LandDefs.LandHeightTable;
            return lh[idx];
        }

        public List<uint> GetScenes(int terrainType, int sceneType)
        {
            //return new List<uint>();
            if (terrainType < 0 || sceneType < 0)
                return new List<uint>();

            if (terrainType < DatManager.PortalDat.RegionDesc.TerrainInfo.TerrainTypes.Count)
            {
                var terrain = DatManager.PortalDat.RegionDesc.TerrainInfo.TerrainTypes[terrainType];
                if (sceneType < terrain.SceneTypes.Count)
                {
                    var sceneInfo = (int)terrain.SceneTypes[sceneType];
                    if (sceneInfo < DatManager.PortalDat.RegionDesc.SceneInfo.SceneTypes.Count)
                    {
                        var scenes = DatManager.PortalDat.RegionDesc.SceneInfo.SceneTypes[sceneInfo].Scenes;
                        // Note that the number of Scenes can be 0, even if it exists. Some scenes are just set up to play ambient sounds.
                        return scenes;
                    }
                }
            }

            return new List<uint>();
        }
    }
}
