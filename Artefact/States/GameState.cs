using Artefact.Audio;
using Artefact.Audio.MusicSystem;
using Artefact.Entities;
using Artefact.MapSystem;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.States
{
    internal class GameState : State
    {
        public static bool SkipNextDraw { get; set; }

        public override void Init()
        {
            Console.Clear();
            Map.Instance.PrintMap();
            Music.AddToQueue(
                new Note(Tone.A, Duration.EIGHTH),
                new Note(Tone.A, Duration.EIGHTH),
                new Note(Tone.D, Duration.QUARTER),
                new Note(Tone.C, Duration.EIGHTH),
                new Note(Tone.C, Duration.EIGHTH),
                new Note(Tone.A, Duration.QUARTER),
                new Note(Tone.A, Duration.EIGHTH),
                new Note(Tone.A, Duration.EIGHTH),
                new Note(Tone.D, Duration.QUARTER),
                new Note(Tone.C, Duration.EIGHTH),
                new Note(Tone.C, Duration.EIGHTH),
                new Note(Tone.A, Duration.QUARTER),
                new Note(Tone.A, Duration.EIGHTH),
                new Note(Tone.A, Duration.EIGHTH),
                new Note(Tone.D, Duration.QUARTER),
                new Note(Tone.C, Duration.EIGHTH),
                new Note(Tone.C, Duration.EIGHTH),
                new Note(Tone.A, Duration.QUARTER),
                new Note(Tone.G, Duration.SIXTEENTH),
                new Note(Tone.Fsharp, Duration.SIXTEENTH),
                new Note(Tone.F, Duration.SIXTEENTH),
                new Note(Tone.Dsharp, Duration.SIXTEENTH),
                new Note(Tone.Dsharp, Duration.EIGHTH),
                new Note(Tone.G, Duration.EIGHTH),
                new Note(Tone.Fsharp, Duration.EIGHTH)
                );
        }

        public override void Update()
        {
            Map.Instance.Update();
            if (InputSystem.IsKeyHeld(ConsoleKey.Escape))
            {
                StateMachine.AddState(new PauseState(), false);
                InputSystem.SkipNextKey = true;
            }

            if (Map.Instance.entities.Count <= 1)
            {
                new Map(50, (int)(Console.WindowWidth * 0.35f), (int)(Console.WindowHeight * 0.8f));
                StateMachine.AddState(new GameState());
                InputSystem.SkipNextKey = true;
            }
        }

        public override void Resume()
        {
            if (!SkipNextDraw)
            {
                Map.Instance.PrintMap();
            }
            SkipNextDraw = false;
            Music.Resume();
        }

        public override void Remove()
        {
            Music.ClearQueue();
        }

        public override void Pause()
        {
            Music.Pause();
        }
    }
}
