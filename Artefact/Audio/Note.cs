using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Audio
{
    internal struct Note
    {
        public Note(Tone frequency, Duration time)
        {
            NoteTone = frequency;
            NoteDuration = time;
        }

        public Tone NoteTone { get; }
        public Duration NoteDuration { get; }
    }
}
