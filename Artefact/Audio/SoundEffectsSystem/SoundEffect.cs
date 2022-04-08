using Artefact.Audio.MusicSystem;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Audio.SoundEffectsSystem
{
    internal class SoundEffect : IPrototype<SoundEffect>
    {
        public List<Note> notes;

        public SoundEffectType soundEffectType;

        public SoundEffect(SoundEffectType soundEffectType, params Note[] notes)
        {
            this.notes=new List<Note>(notes);
            this.soundEffectType = soundEffectType;
        }

        public SoundEffect Clone()
        {
            return (SoundEffect)MemberwiseClone();
        }
    }
}
