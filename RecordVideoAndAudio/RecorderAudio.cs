using Captura.Audio;
using Captura.FFmpeg;
using NAudio.CoreAudioApi;
using NAudio.Lame;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RecordVideoAndAudio
{
    public class RecorderAudio
    {
        const string extention = ".mp3";
        public bool Startded { get; private set; } = false;
        public string FileName { get; private set; }


        MMDeviceEnumerator _deviceEnumerator = new MMDeviceEnumerator();
        NAudioNotificationClient _notificationClient = new NAudioNotificationClient();
        public event Action DevicesUpdated;

        public RecorderAudio()
        {
            _notificationClient.DevicesUpdated += () => DevicesUpdated?.Invoke();

            _deviceEnumerator.RegisterEndpointNotificationCallback(_notificationClient);

            

            //foreach (var device in devices)
            //{
            //    new NAudioItem(device, false);
            //}


        }
        IAudioSource _audioSource;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="inputDeviceIndex">microphone</param>
        /// <param name="outputDeviceIndex">speaker</param>
        public void StartRecording(String fileName, string inputDeviceName, string outputDeviceName)
        {
            if (!Startded)
            {
                FileName = fileName;
                _audioSource = new NAudioSource();

                if (!SetupAudioProvider(inputDeviceName, outputDeviceName, out var audioProvider))
                    return;

                if (!InitAudioRecorder(audioProvider))
                {
                    audioProvider?.Dispose();

                    return;
                }

                _recorder.Start();

                Startded = true;
            }
        }

        bool SetupAudioProvider(string inputDeviceName, string outputDeviceName, out IAudioProvider AudioProvider)
        {
            AudioProvider = null;

            try
            {
                var microphone = GetDeviceInByName(inputDeviceName);
                if (microphone == null)
                    throw new ArgumentNullException(nameof(microphone));
                var speaker = GetDeviceOutByName(outputDeviceName);
                if (speaker == null)
                    throw new ArgumentNullException(nameof(speaker));

                AudioProvider = _audioSource.GetAudioProvider(microphone, speaker);
                //AudioProvider = _audioSource.GetAudioProviderDefault();

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);

                return false;
            }

            return true;
        }

        bool InitAudioRecorder(IAudioProvider AudioProvider)
        {
            try
            {
                _recorder = GetAudioRecorder(AudioProvider);
            }
            catch (Exception ex)
            {
                //_ffmpegViewsProvider.ShowUnavailable();
                System.Windows.Forms.MessageBox.Show(ex.Message);

                return false;
            }

            return true;
        }
        IRecorder GetAudioRecorder(IAudioProvider AudioProvider)
        {
            var audioFileWriter = GetAudioFileWriter(
                FileName + extention,
                AudioProvider?.WaveFormat,
                80);

            return new AudioRecorder(audioFileWriter, AudioProvider);
        }
        public IAudioFileWriter GetAudioFileWriter(string FileName, Captura.Audio.WaveFormat Wf, int AudioQuality)
        {
            var AudioArgsProvider = FFmpegAudioItem.Mp3;
            return new FFmpegAudioWriter(FileName, AudioQuality, AudioArgsProvider, Wf.SampleRate, Wf.Channels);
        }



        public List<string> DevicesOut()
        {
            List<string> list = new List<string>();
            //for (int waveInDevice = 0; waveInDevice < WaveIn.DeviceCount; waveInDevice++)
            //{
            //    WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
            //    list.Add($"{waveInDevice} - {deviceInfo.ProductName}, {deviceInfo.Channels}");
            //}
            _devicesIn = _deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            
            foreach (var item in _devicesIn)
            {
                list.Add($"{item.FriendlyName}");
            }
            return list;
        }
        private MMDeviceCollection _devicesIn;
        private MMDeviceCollection _devicesOut;
        private IRecorder _recorder;

        public List<string> DevicesIn()
        {
            List<string> list = new List<string>();

            _devicesOut = _deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            foreach (var item in _devicesOut)
            {
                list.Add($"{item.FriendlyName}");
            }
            return list;
        }
        public MMDevice GetDeviceOutByName(string name)
        {
            foreach (var item in _devicesOut)
            {
                if (item.FriendlyName == name)
                    return item;
            }
            return null;
        }
        public MMDevice GetDeviceInByName(string name)
        {
            foreach (var item in _devicesIn)
            {
                if (item.FriendlyName == name)
                    return item;
            }
            return null;
        }

        public void StopRecording()
        {
            _recorder.Dispose();

            Startded = false;
        }        
    }
}
