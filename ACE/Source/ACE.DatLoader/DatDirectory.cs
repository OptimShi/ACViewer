using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader
{
    public class DatDirectory
    {
        private uint rootSectorOffset { get; }

        private uint blockSize { get; }


        public DatDirectoryHeader DatDirectoryHeader { get; } = new DatDirectoryHeader();

        public List<DatDirectory> Directories { get; } = new List<DatDirectory>();


        public DatDirectory(uint rootSectorOffset, uint blockSize)
        {
            this.rootSectorOffset = rootSectorOffset;
            this.blockSize = blockSize;
        }

        public void Read(FileStream stream)
        {
            var headerReader = new DatReader(stream, rootSectorOffset, DatDirectoryHeader.GetObjectSize(), blockSize);

            using (var memoryStream = new MemoryStream(headerReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                DatDirectoryHeader.Unpack(reader);


            // Beta 0
            if(DatManager.DatVersion == DatVersionType.DM && DatManager.Iteration <= 8)
            {
                for (int i = 0; i < DatDirectoryHeader.Branches.Length; i++)
                {
                    if (DatDirectoryHeader.Branches[i] != 0
                       && DatDirectoryHeader.Branches[i] != 0xcdcdcdcd
                       && DatDirectoryHeader.Branches[i] != rootSectorOffset // Preview 2 thing
                       && !DatManager.ReadSectors.Contains(DatDirectoryHeader.Branches[i])
                    )
                    {
                        var directory = new DatDirectory(DatDirectoryHeader.Branches[i], blockSize);
                        directory.Read(stream);
                        Directories.Add(directory);
                        DatManager.ReadSectors.Add(DatDirectoryHeader.Branches[i]);
                    }
                }

            }
            else
            // directory is allowed to have files + 1 subdirectories
            //if (DatDirectoryHeader.Branches[0] != 0)
            {
                if (DatDirectoryHeader.Branches[0] != 0)
                {
                    for (int i = 0; i < DatDirectoryHeader.EntryCount + 1; i++)
                    {
                        var directory = new DatDirectory(DatDirectoryHeader.Branches[i], blockSize);
                        directory.Read(stream);
                        Directories.Add(directory);
                    }
                }
            }
        }

        public void AddFilesToList(Dictionary<uint, DatFile> dicFiles)
        {
            Directories.ForEach(d => d.AddFilesToList(dicFiles));

            for (int i = 0; i < DatDirectoryHeader.EntryCount; i++)
                dicFiles[DatDirectoryHeader.Entries[i].ObjectId] = DatDirectoryHeader.Entries[i];
        }
    }
}
