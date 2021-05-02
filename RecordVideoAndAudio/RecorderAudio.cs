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
        public IAudioProvider AudioProvider { get; private set; }

        public RecorderAudio()
        {
        }
        IAudioSource _audioSource;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="inputDeviceIndex">microphone</param>
        /// <param name="outputDeviceIndex">speaker</param>
        public void StartRecording(String fileName, int inputDeviceIndex, int outputDeviceIndex)
        {
            if (!Startded)
            {
                

                if (!SetupAudioProvider(fileName))
                    return;

                if (!InitAudioRecorder())
                {
                    AudioProvider?.Dispose();

                    return;
                }

                _recorder.Start();

                Startded = true;
            }
        }

        public bool SetupAudioProvider(string fileName)
        {
            FileName = fileName;
            _audioSource = new NAudioSource();

            AudioProvider = null;

            try
            {
                    AudioProvider = _audioSource.GetAudioProviderDefault();
                
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);

                return false;
            }

            return true;
        }

        bool InitAudioRecorder()
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
            for (int waveInDevice = 0; waveInDevice < WaveIn.DeviceCount; waveInDevice++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                list.Add($"{waveInDevice} - {deviceInfo.ProductName}, {deviceInfo.Channels}");
            }
            return list;
        }
        MMDeviceCollection _devices;
        private IRecorder _recorder;

        public List<string> DevicesIn()
        {
            List<string> list = new List<string>();

            _devices = new MMDeviceEnumerator()
                .EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            foreach (var item in _devices)
            {
                list.Add($"{item.DeviceFriendlyName} - {item.FriendlyName}");
            }
            return list;
        }
        
        public void StopRecording()
        {
            _recorder?.Dispose();

            Startded = false;
        }        
    }
}
