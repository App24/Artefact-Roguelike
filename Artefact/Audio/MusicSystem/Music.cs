using Artefact.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Artefact.Audio.MusicSystem
{
    internal static class Music
    {
        private static Thread thread;
        private static bool playing;
        private static List<Note> queueNotes;
        private static int index;

        public static void StartThread()
        {
            if (thread != null)
                return;
            queueNotes = new List<Note>();
#if !NO_MUSIC
            thread = new Thread(new ThreadStart(Play));
            thread.Start();
#endif
            playing = true;
        }

        private static void Play()
        {
            while (GlobalSettings.Running)
            {
                if (!playing || queueNotes.Count <= 0)
                {
                    Thread.Sleep(1);
                    continue;
                }

                Note note = queueNotes[index];

                if (note.NoteTone == Tone.REST)
                {
                    Thread.Sleep((int)note.NoteDuration);
                }
                else
                {
                    Console.Beep((int)note.NoteTone, (int)note.NoteDuration);
                }

                index++;

                if (index >= queueNotes.Count)
                    index = 0;
            }
        }

        public static void AddToQueue(params Note[] notes)
        {
            queueNotes.AddRange(notes);
        }

        public static void ClearQueue()
        {
            queueNotes.Clear();
        }

        public static void Toggle()
        {
            playing = !playing;
        }

        public static void Pause()
        {
            playing = false;
        }

        public static void Resume()
        {
            playing = true;
        }
    }
}
