using Artefact.Entities;
using Artefact.Settings;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Artefact.Saving
{
    internal static class SaveSystem
    {
        private const string SETTINGS_FILE = "settings.dat";
        private const string SAVE_FILE = "save.dat";

        public static bool HasSaveGame => File.Exists(SAVE_FILE);

        /// <summary>
        /// Save an object to a file by converting it into binary
        /// </summary>
        /// <param name="fileName">File path for the file</param>
        /// <param name="value">Object to be turned into a file</param>
        private static void SaveClass(string fileName, object value)
        {
            string directory = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);
            FileStream stream = File.Create(fileName);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(stream, value);
                stream.Close();
            }
            catch (Exception e)
            {
                stream.Close();
                File.Delete(fileName);
                throw e;
            }
        }

        /// <summary>
        /// Load an object into a class
        /// </summary>
        /// <typeparam name="T">Type of object to be loaded</typeparam>
        /// <param name="fileName">File path of the file</param>
        /// <returns><see cref="LoadDetails{T}"/> showing whether the loading failed or succeeded and the loaded object</returns>
        private static LoadDetails<T> LoadClass<T>(string fileName) where T : class
        {
            if (!File.Exists(fileName))
            {
                return new LoadDetails<T>(LoadResult.NoFile, null);
            }

            FileStream stream = File.OpenRead(fileName);

            if (stream.Length <= 0)
            {
                stream.Close();
                return new LoadDetails<T>(LoadResult.InvalidFile, null);
            }

            T value;

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                value = (T)formatter.Deserialize(stream);
                stream.Close();
            }
            catch
            {
                stream.Close();
                return new LoadDetails<T>(LoadResult.InvalidFile, null);
            }

            return new LoadDetails<T>(LoadResult.Success, value);
        }

        public static void SaveSettings()
        {
            SaveClass(SETTINGS_FILE, GlobalSettings.Instance);
        }

        public static void LoadSettings()
        {
            LoadDetails<GlobalSettings> loadDetails = LoadClass<GlobalSettings>(SETTINGS_FILE);
            if (loadDetails.LoadResult == LoadResult.Success)
                GlobalSettings.Instance = loadDetails.Saveable;
            else
                new GlobalSettings();
        }

        public static void SaveGame(string fileName = SAVE_FILE)
        {
            SaveClass(fileName, new Save());
        }

        public static LoadResult LoadGame(string fileName = SAVE_FILE)
        {
            LoadDetails<Save> loadDetails = LoadClass<Save>(fileName);
            if (loadDetails.LoadResult == LoadResult.Success)
            {
                Save save = loadDetails.Saveable;
                World.Instance = save.CurrentWorld;
                World.Instance.RegenerateRandom();
                if (World.Instance is CaveWorld caveWorld)
                {
                    World upperWorld = caveWorld;
                    do
                    {
                        if(upperWorld is CaveWorld c && c.ExitLadders.Count > 0)
                        {
                            upperWorld = c.ExitLadders[0].WorldToGo;
                        }

                        if (upperWorld != null && upperWorld!=caveWorld)
                        {
                            upperWorld.RegenerateRandom();
                        }
                    } while (upperWorld is CaveWorld);

                    World lowerWorld = caveWorld;
                    do
                    {
                        if (lowerWorld is CaveWorld c && c.ExitLadders.Count > 0)
                        {
                            lowerWorld = c.ExitLadders[0].WorldToGo;
                        }

                        if (lowerWorld != null && lowerWorld!=caveWorld)
                        {
                            lowerWorld.RegenerateRandom();
                        }
                    } while (lowerWorld is CaveWorld);
                }
                PlayerEntity.Instance = save.Player;
            }
            return loadDetails.LoadResult;
        }

        public static void NewGame()
        {
            World.GenerateRandomSeed();
            World.Instance = new OverworldWorld(60, 60);
        }
    }

    internal struct LoadDetails<T>
    {
        public LoadResult LoadResult { get; }
        public T Saveable { get; }

        public LoadDetails(LoadResult loadResult, T saveable)
        {
            LoadResult = loadResult;
            Saveable = saveable;
        }
    }

    internal enum LoadResult
    {
        Success,
        NoFile,
        InvalidFile
    }
}
