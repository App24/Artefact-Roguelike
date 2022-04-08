using Artefact.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Artefact.Audio.MusicSystem
{
    internal static class Music
    {
        static Thread thread;

        static bool playing;

        static List<Note> queueNotes;

        public static void StartThread()
        {
            if (thread != null)
                return;
            queueNotes = new List<Note>();
            thread = new Thread(new ThreadStart(Play));
            thread.Start();
            playing = true;
        }

        static void Play()
        {
            while (GlobalSettings.Running)
            {
                if (!playing || queueNotes.Count <= 0)
                {
                    Thread.Sleep(1);
                    continue;
                }

                Note note = queueNotes[0];

                if(note.NoteTone == Tone.REST)
                {
                    Thread.Sleep((int)note.NoteDuration);
                }
                else
                {
                    Console.Beep((int)note.NoteTone, (int)note.NoteDuration);
                }

                queueNotes.RemoveAt(0);
            }
        }

        public static void AddToQueue(params Note[] notes)
        {
            queueNotes.AddRange(notes);
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
