using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KawaiiBot2.Modules;
using KawaiiBot2.JSONClasses;

namespace KawaiiBot2
{
    public static class Persistance
    {
        private static string filename;
        private static uint saveIter;

        private static string FileNameFromConfName(string confName)
        {
            var name = Path.GetFileNameWithoutExtension(confName);
            string replaceName = "db-{0}";
            if (name.Contains("conf"))
            {
                replaceName = name.Replace("conf", "db") + "-{0}";
            }
            var dirName = Path.GetDirectoryName(confName);

            return $"{dirName}{(string.IsNullOrEmpty(dirName) ? "" : Path.DirectorySeparatorChar)}{replaceName}.json";
        }


        public static void LoadEverything(string confName)
        {
            filename = FileNameFromConfName(confName);
            var omoName = string.Format(filename, "omote");
            var uraName = string.Format(filename, "ura");
            PersistanceDBJson omoFile = null, uraFile = null;
            if (File.Exists(omoName))
            {
                omoFile = JsonConvert.DeserializeObject<PersistanceDBJson>(File.ReadAllText(omoName));
            }
            if (File.Exists(uraName))
            {
                uraFile = JsonConvert.DeserializeObject<PersistanceDBJson>(File.ReadAllText(uraName));
            }
            //compare and find the newer
            if (omoFile == null && uraFile == null)
            {
                return;
            }
            var notNull = omoFile ?? uraFile;
            var latestFile = notNull.SaveNumber > uraFile?.SaveNumber ? notNull : uraFile;
            saveIter = latestFile.SaveNumber;
            Slots.PerpetuatePersistance(latestFile.Slots);
            Informational.PerpetuatePopularityPersistance(latestFile.CommandCounter);
        }


        public static void SaveEverything()
        {
            var saveFilename = string.Format(filename, ++saveIter % 2 == 1 ? "omote" : "ura");

            var slots = Slots.GetSlotsSaveObject();
            var commandCounter = Informational.GetPopularitySave();
            //also for command usage or wherever that comes from
            var persistanceObject = new
            {
                saveNumber = saveIter,
                slots,
                commandCounter
            };
            File.WriteAllText(saveFilename, JsonConvert.SerializeObject(persistanceObject));
        }

        public static void StartSaveScheduler()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000/*ms/s*/ * 60/*s/m*/ * 60/*m/h*/* 1/*hours total*/);
                    SaveEverything();
                }
            });
        }

    }
}
