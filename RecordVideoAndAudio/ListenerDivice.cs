using NAudio.CoreAudioApi;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
//using Reactive.Bindings.Extensions;

namespace RecordVideoAndAudio
{
    public class ListenerDivice
    {
        public ListenerDivice(Form1 form1)
        {
            Form1 = form1;
        }
        internal void StartListener()
        {
            var microphone = Form1.recorderAudio.GetDeviceInByName((string)Form1.microphonesComboBox.SelectedItem);
            if (microphone == null)
                throw new ArgumentNullException(nameof(microphone));
            var speaker = Form1.recorderAudio.GetDeviceOutByName((string)Form1.speakerComboBox.SelectedItem);
            if (speaker == null)
                throw new ArgumentNullException(nameof(speaker));

            StartListeningForPeakLevel(ref _audioClientMic,ref microphone);
            StartListeningForPeakLevel(ref _audioClientSpeak, ref speaker);

            Observable.Interval(TimeSpan.FromMilliseconds(5))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x =>
                    Form1.progressBarMicrophone.Value = (int)(microphone?.AudioMeterInformation?.MasterPeakValue*100 ?? 0)
                //.Where(M => ListeningPeakLevel)
                //.ObserveOnUIDispatcher()
                //.Select(M => microphone?.AudioMeterInformation?.MasterPeakValue ?? 0)
                );//.
                  //.ToReadOnlyReactivePropertySlim();


            Observable.Interval(TimeSpan.FromMilliseconds(5))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x =>
                    Form1.progressBarSpeaker.Value = (int)(speaker?.AudioMeterInformation?.MasterPeakValue * 100 ?? 0)
                );
        }

        AudioClient _audioClientMic;
        AudioClient _audioClientSpeak;

        public Form1 Form1 { get; }

        public void StartListeningForPeakLevel(ref AudioClient _audioClient,  ref MMDevice Device)
        {
            if (_audioClient != null)
                return;

            // Peak Level is available for recording devices only when they are active
            //if (IsLoopback)
            //    return;

            _audioClient = Device.AudioClient;
            _audioClient.Initialize(AudioClientShareMode.Shared,
                AudioClientStreamFlags.None,
                100,
                100,
                _audioClient.MixFormat,
                Guid.Empty);

            _audioClient.Start();
        }

        public void StopListeningForPeakLevel()
        {
            if (_audioClientMic == null)
                return;

            _audioClientMic.Stop();
            _audioClientMic.Dispose();
            _audioClientMic = null;

            if (_audioClientSpeak == null)
                return;

            _audioClientSpeak.Stop();
            _audioClientSpeak.Dispose();
            _audioClientSpeak = null;
        }
    }
}
