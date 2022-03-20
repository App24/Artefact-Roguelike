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
        private const string SAVE_FOLDER = "saves";
        private const int SAVE_SLOTS = 1;

        public static bool HasAnySaveGames => Directory.Exists(SAVE_FOLDER);

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
                //Utils.WriteColor($"[{ColorConstants.ERROR_COLOR}]There was an error whilst saving:\n{e.Message}");
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
            long currentTime = DateTime.UtcNow.Ticks;
            //GameSettings.GameTime += currentTime - GameSettings.SessionStartDate;
            //GameSettings.SessionStartDate = currentTime;
            SaveClass(Path.Combine(SAVE_FOLDER, "0", fileName), new Save());
        }

        public static LoadResult LoadGame(string fileName = SAVE_FILE, int slot = 0)
        {
            /*if (slot <= 0)
                slot = GameSettings.SaveSlot;*/
            LoadDetails<Save> loadDetails = LoadClass<Save>(Path.Combine(SAVE_FOLDER, slot.ToString(), fileName));
            if (loadDetails.LoadResult == LoadResult.Success)
            {
                //Utils.WriteColor($"[{ColorConstants.GOOD_COLOR}]Save loaded successfully!");
                Console.WriteLine("Save loaded successfully!");
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
                //GameSettings.Instance = save.GameSettings;
                //Story.Step = save.StoryStep;
                //Map.Instance = save.Map;
                //GlobalSettings.JustLoaded = true;
                //GameSettings.SessionStartDate = DateTime.UtcNow.Ticks;
            }
            else
            {
                //Utils.WriteColor($"[{ColorConstants.ERROR_COLOR}]There was a problem loading the save game!");
                Console.WriteLine("There was a problem loading the save game!");
                switch (loadDetails.LoadResult)
                {
                    case LoadResult.InvalidFile:
                        {
                            //Utils.WriteColor($"[{ColorConstants.ERROR_COLOR}]Save file is an invalid file!");
                            Console.WriteLine("Save file is an invalid file!");
                        }
                        break;
                    case LoadResult.NoFile:
                        {
                            //Utils.WriteColor($"[{ColorConstants.ERROR_COLOR}]There is no save file!");
                            Console.WriteLine("There is no save file!");
                        }
                        break;
                }
            }
            return loadDetails.LoadResult;
        }

        public static void NewGame()
        {
            //new GameSettings();
            //GameSettings.GameStartDate = DateTime.UtcNow.Ticks;
            //GameSettings.SessionStartDate = DateTime.UtcNow.Ticks;
            //Story.Step = 0;
            //new Map();
        }

        public static List<string> GetSaveGameNames()
        {
            List<string> saves = new List<string>();
            for (int i = 1; i < SAVE_SLOTS + 1; i++)
            {
                string saveName = "";
                if (!Directory.Exists(Path.Combine(SAVE_FOLDER, saveName)))
                {
                    saveName = $"Empty";
                }
                else
                {
                    LoadDetails<Save> loadDetails = LoadClass<Save>(Path.Combine(SAVE_FOLDER, i.ToString(), SAVE_FILE));
                    if (loadDetails.LoadResult != LoadResult.Success)
                        saveName = $"Empty";
                    else
                    {
                        saveName = i.ToString();
                        /*saveName = $"[{ColorConstants.CHARACTER_COLOR}]{loadDetails.Saveable.GameSettings.playerName}[/]";
                        DateTime gameTime = new DateTime(loadDetails.Saveable.GameSettings.gameTime);
                        saveName += $" - [{ColorConstants.GOOD_COLOR}]{gameTime.ToString("H:mm:ss")}[/]";*/
                    }
                }
                saves.Add(saveName);
            }

            return saves;
        }

        public static bool HasSavegame(int slot)
        {
            return LoadClass<Save>(Path.Combine(SAVE_FOLDER, slot.ToString(), SAVE_FILE)).LoadResult == LoadResult.Success;
        }
    }

    internal class LoadDetails<T>
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
