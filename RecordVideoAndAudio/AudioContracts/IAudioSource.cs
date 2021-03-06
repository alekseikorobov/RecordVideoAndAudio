﻿using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;

namespace Captura.Audio
{
    public interface IAudioSource : IDisposable
    {
        string Name { get; }

        IEnumerable<IAudioItem> Microphones { get; }

        IAudioItem DefaultMicrophone { get; }

        IEnumerable<IAudioItem> Speakers { get; }

        IAudioItem DefaultSpeaker { get; }

        IAudioProvider GetAudioProvider(MMDevice Microphone, MMDevice Speaker);
        IAudioProvider GetAudioProviderDefault();

        event Action DevicesUpdated;
    }
}