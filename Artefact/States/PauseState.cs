using Artefact.Audio;
using Artefact.Audio.SoundEffectsSystem;
using Artefact.MenuSystem;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.States
{
    internal class PauseState : State
    {
        public override void Init()
        {
            Menu.SwitchMenu(new PauseMenu());
            SFXSystem.AddSoundEffect(new SoundEffect(SoundEffectType.Menu, new Note(Tone.GbelowC, Duration.EIGHTH)));
        }

        public override void Update()
        {
            Menu.Instance.NavigateOptions();

            if (InputSystem.IsKeyHeld(ConsoleKey.Escape))
            {
                StateMachine.RemoveState();
                InputSystem.SkipNextKey = true;
            }
        }

        public override void Pause()
        {
            Console.Clear();
        }

        public override void Remove()
        {
            Pause();
        }
    }
}
