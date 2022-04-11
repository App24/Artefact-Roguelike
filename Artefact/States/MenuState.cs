using Artefact.Audio;
using Artefact.Audio.MusicSystem;
using Artefact.MapSystem;
using Artefact.MenuSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.States
{
    internal class MenuState : State
    {
        public override void Init()
        {
            Map.Instance = null;
            Menu.SwitchMenu(null);
            Menu.SwitchMenu(new MainMenu());

            Music.AddToQueue(
                new Note(Tone.B, Duration.HALF),
                new Note(Tone.B, Duration.HALF),
                new Note(Tone.G, Duration.HALF),
                new Note(Tone.C, Duration.QUARTER),
                new Note(Tone.A, Duration.QUARTER)
                );
        }

        public override void Update()
        {
            Menu.Instance.NavigateOptions();
        }
    }
}
