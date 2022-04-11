using Artefact.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Artefact.Audio.SoundEffectsSystem
{
    internal static class SFXSystem
    {
        private static Dictionary<SoundEffectType, List<SoundEffect>> soundEffects;
        private static List<Thread> threads;

        public static void StartThreads()
        {
            if (threads != null)
                return;
            soundEffects = new Dictionary<SoundEffectType, List<SoundEffect>>();
            threads = new List<Thread>();
            for (int i = 0; i < (int)SoundEffectType.Last; i++)
            {
                int index = i;
                SoundEffectType type = (SoundEffectType)index;
                soundEffects.Add(type, new List<SoundEffect>());
                Thread thread = new Thread(() => Play(type));
                thread.Start();
                threads.Add(thread);
            }
        }

        private static void Play(SoundEffectType scope)
        {
            while (GlobalSettings.Running)
            {
                List<SoundEffect> list;
                soundEffects.TryGetValue(scope, out list);

                if (list.Count <= 0)
                {
                    Thread.Sleep(1);
                    continue;
                }

                SoundEffect soundEffect = list[0];

                Note note = soundEffect.notes[0];

                if (note.NoteTone == Tone.REST)
                {
                    Thread.Sleep((int)note.NoteDuration);
                }
                else
                {
                    Console.Beep((int)note.NoteTone, (int)note.NoteDuration);
                }

                soundEffect.notes.RemoveAt(0);

                if (soundEffect.notes.Count <= 0)
                    list.RemoveAt(0);
            }
        }

        public static void AddSoundEffect(SoundEffect soundEffect)
        {
            soundEffects.TryGetValue(soundEffect.soundEffectType, out var list);
            if (soundEffect.soundEffectType == SoundEffectType.Menu)
            {
                list.Clear();
            }
            list.Add(soundEffect.Clone());
        }
    }

    internal enum SoundEffectType
    {
        Entity,
        Item,
        Tile,
        Menu,
        Last
    }
}
