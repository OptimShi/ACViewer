using log4net;
using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader
{
    public static class DatManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string datFile;

        private static int count;

        // End of retail Iteration versions.
        private static int ITERATION_CELL = 982;
        private static int ITERATION_PORTAL = 2072;
        private static int ITERATION_HIRES = 497;
        private static int ITERATION_LANGUAGE = 994;

        public static DatVersionType DatVersion;
        public static int Iteration;
        public static CellDatDatabase CellDat { get; private set; }

        public static PortalDatDatabase PortalDat { get; private set; }
        public static DatDatabase HighResDat { get; private set; }
        public static LanguageDatDatabase LanguageDat { get; private set; }

        // Cheater for some beta versions
        public static List<uint> ReadSectors = new List<uint>();


        public static void Initialize(string datFileDirectory, bool keepOpen = false, bool loadCell = true)
        {
            var datDir = Path.GetFullPath(Path.Combine(datFileDirectory));

            if (loadCell)
            {
                try
                {
                    datFile = Path.Combine(datDir, "client_cell_1.dat");
                    if(!File.Exists(datFile)) // Try the old version. The new CellDatDatabase will handle it this one ALSO does not exist
                        datFile = Path.Combine(datDir, "cell.dat");
                    CellDat = new CellDatDatabase(datFile, keepOpen);
                    ReadSectors.Clear();
                    count = CellDat.AllFiles.Count;
                    Console.WriteLine($"-- Loaded {datFile} with {count} files.");
                    log.Info($"Successfully opened {datFile} file, containing {count} records, iteration {CellDat.Iteration}");
                    if (CellDat.Iteration != ITERATION_CELL)
                        log.Warn($"{datFile} iteration does not match expected end-of-retail version of {ITERATION_CELL}.");
                }
                catch (FileNotFoundException ex)
                {
                    log.Error($"An exception occured while attempting to open {datFile} file!  This needs to be corrected in order for Landblocks to load!");
                    log.Error($"Exception: {ex.Message}");
                }
            }

            try
            {
                datFile = Path.Combine(datDir, "client_portal.dat");
                if (!File.Exists(datFile)) // Try the old version. The new PortalDatDatabase will handle it this one ALSO does not exist
                    datFile = Path.Combine(datDir, "portal.dat");

                PortalDat = new PortalDatDatabase(datFile, keepOpen);
                count = PortalDat.AllFiles.Count;
                Console.WriteLine($"-- Loaded {datFile} with {count} files.");
                PortalDat.ReadBaseFiles();
                ReadSectors.Clear();
//                PortalDat.SkillTable.AddRetiredSkills();
                log.Info($"Successfully opened {datFile} file, containing {count} records, iteration {PortalDat.Iteration}");
                if (PortalDat.Iteration != ITERATION_PORTAL)
                    log.Warn($"{datFile} iteration does not match expected end-of-retail version of {ITERATION_PORTAL}.");
            }
            catch (FileNotFoundException ex)
            {
                log.Error($"An exception occured while attempting to open {datFile} file!\n\n *** Please check your 'DatFilesDirectory' setting in the config.js file. ***\n *** ACE will not run properly without this properly configured! ***\n");
                log.Error($"Exception: {ex.Message}");
            }

            // Load the client_highres.dat file. This is not required for ACE operation, so no exception needs to be generated.
            datFile = Path.Combine(datDir, "client_highres.dat");
            if (File.Exists(datFile))
            {
                HighResDat = new DatDatabase(datFile, keepOpen);
                ReadSectors.Clear();
                count = HighResDat.AllFiles.Count;
                log.Info($"Successfully opened {datFile} file, containing {count} records, iteration {HighResDat.Iteration}");
                if (HighResDat.Iteration != ITERATION_HIRES)
                    log.Warn($"{datFile} iteration does not match expected end-of-retail version of {ITERATION_HIRES}.");
            }

            datFile = Path.Combine(datDir, "client_local_English.dat");
            if (File.Exists(datFile))
            {
                try
                {
                    datFile = Path.Combine(datDir, "client_local_English.dat");
                    LanguageDat = new LanguageDatDatabase(datFile, keepOpen);
                    ReadSectors.Clear();
                    count = LanguageDat.AllFiles.Count;
                    log.Info($"Successfully opened {datFile} file, containing {count} records, iteration {LanguageDat.Iteration}");
                    if (LanguageDat.Iteration != ITERATION_LANGUAGE)
                        log.Warn($"{datFile} iteration does not match expected end-of-retail version of {ITERATION_LANGUAGE}.");
                }
                catch (FileNotFoundException ex)
                {
                    log.Error($"An exception occured while attempting to open {datFile} file!\n\n *** Please check your 'DatFilesDirectory' setting in the config.json file. ***\n *** ACE will not run properly without this properly configured! ***\n");
                    log.Error($"Exception: {ex.Message}");
                }
            }
        }



        /// <summary>
        /// Some dat versions have a different branch size for the DatDirectoryHeader
        /// </summary>
        /// <returns></returns>
        public static int GetBranchSize()
        {
/*
Beta 0
Cell Iteration: 8
Portal Iteration: 8

Beta 2
Cell Iteration: 12
Portal Iteration: 12

Beta 3
Cell Iteration: 20
Portal Iteration: 20

LAUNCH
Cell 67
Portal 102
*/

            switch (DatVersion)
            {
                case DatVersionType.DM:
                    // Beta 0 Iteration
                    if (Iteration <= 8)
                        return 0x25;

                    // Beta 2+
                    return 0x3E;
                case DatVersionType.TOD:
                    return 0x3E;
            }

            return 0;
        }
    }
}
