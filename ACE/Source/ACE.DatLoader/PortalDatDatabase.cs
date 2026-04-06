
using ACE.DatLoader.FileTypes;

namespace ACE.DatLoader
{
    public class PortalDatDatabase : DatDatabase
    {
        public PortalDatDatabase(string filename, bool keepOpen = false) : base(filename, keepOpen)
        {
        }

        public void ReadBaseFiles()
        {
            if (DatManager.DatVersion == DatVersionType.TOD)
            {
                BadData = ReadFromDat<BadData>(BadData.FILE_ID);
                MasterProperty = ReadFromDat<MasterProperty>(MasterProperty.FILE_ID);
                ContractTable = ReadFromDat<ContractTable>(ContractTable.FILE_ID);
                RegionDesc = ReadFromDat<RegionDesc>(RegionDesc.FILE_ID);
            }
            else
            {
                RegionDesc = ReadFromDat<RegionDesc>(RegionDesc.HW_FILE_ID);
            }

            //            ChatPoseTable = ReadFromDat<ChatPoseTable>(ChatPoseTable.FILE_ID);
            //            CharGen = ReadFromDat<CharGen>(CharGen.FILE_ID);
            //            GeneratorTable = ReadFromDat<GeneratorTable>(GeneratorTable.FILE_ID);
            //            NameFilterTable = ReadFromDat<NameFilterTable>(NameFilterTable.FILE_ID);

            SecondaryAttributeTable = ReadFromDat<SecondaryAttributeTable>(SecondaryAttributeTable.FILE_ID);
            SkillTable = ReadFromDat<SkillTable>(SkillTable.FILE_ID);
            //          SpellComponentsTable = ReadFromDat<SpellComponentsTable>(SpellComponentsTable.FILE_ID);
            //          SpellTable = ReadFromDat<SpellTable>(SpellTable.FILE_ID);
            //          TabooTable = ReadFromDat<TabooTable>(TabooTable.FILE_ID);
            //          XpTable = ReadFromDat<XpTable>(XpTable.FILE_ID);
        }

        public BadData BadData { get; private set; }
        public ChatPoseTable ChatPoseTable { get; private set; }
        public CharGen CharGen { get; private set; }
        public ContractTable ContractTable { get; private set; }
        public GeneratorTable GeneratorTable { get; private set; }
        public MasterProperty MasterProperty { get; private set; }
        public NameFilterTable NameFilterTable { get; private set; }
        public RegionDesc RegionDesc { get; private set; }
        public SecondaryAttributeTable SecondaryAttributeTable { get; private set; }
        public SkillTable SkillTable { get; private set; }
        public SpellComponentsTable SpellComponentsTable { get; private set; }
        public SpellTable SpellTable { get; private set; }
        public TabooTable TabooTable { get; private set; }
        public XpTable XpTable { get; private set; }
    }
}
