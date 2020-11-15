using NAudio.CoreAudioApi;
using NAudio.Lame;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecordVideoAndAudio
{
    public class RecorderAudio
    {
        const string extention = ".mp3";
        WaveInEvent waveOut;
        WasapiLoopbackCapture waveIn;

        LameMP3FileWriter waveWriter_in;        
        LameMP3FileWriter waveWriter_out;
        public bool Startded { get; private set; } = false;
        public string FileName { get; private set; }

        WaveFormat wf;
        public RecorderAudio()
        {
            wf = WaveFormat.CreateCustomFormat(tag: WaveFormatEncoding.IeeeFloat, sampleRate: 48000, channels: 2, averageBytesPerSecond: 384000, blockAlign: 8, bitsPerSample: 32);
        }

        public void StartRecording(String fileName, int inputDeviceIndex, int outputDeviceIndex)
        {
            if (!Startded)
            {
                this.FileName = fileName;
                this.RecordIn(outputDeviceIndex);
                this.RecordOut(inputDeviceIndex);
                Startded = true;
            }
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


        object o1 = new object();
        object o2 = new object();

        /// <summary>
        /// переменная шума для динамиков, когда из динамиков не выходит звук
        /// </summary>
        WaveOut waveOneOut;
        //запись входящих звуков из динамиков
        private void RecordIn(int inputDeviceIndex)
        {
            lock (o1)
            {
                var device = _devices.ElementAt(inputDeviceIndex);                
                waveIn = new WasapiLoopbackCapture()
                {                
                };
                
                waveIn.DataAvailable += WaveIn_DataAvailable;
                waveIn.RecordingStopped += this.WaveIn_RecordingStopped;

                waveWriter_in = new LameMP3FileWriter(FileName + "_in" + extention, wf, 32);

                waveOneOut = new WaveOut();
                waveOneOut.Init(new SilentWaveProvider());
                waveOneOut.Play();

                waveIn.StartRecording();
            }
        }
        /// <summary>
        /// запись исходящих звуков из микрофонов
        /// </summary>
        /// <param name="inputDeviceIndex"></param>
        private void RecordOut(int inputDeviceIndex)
        {
            lock (o2)
            {
                waveOut = new WaveInEvent
                {
                    DeviceNumber = inputDeviceIndex,
                    WaveFormat = wf                    
                };                

                waveOut.DataAvailable += this.WaveOut_DataAvailable;
                waveOut.RecordingStopped += this.WaveOut_RecordingStopped;

                waveWriter_out = new LameMP3FileWriter(FileName + "_out" + extention, wf, 32);
                waveOut.StartRecording();
            }
        }
        private void WaveOut_RecordingStopped(object sender, StoppedEventArgs e)
        {
        }
        private void WaveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {

        }
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveWriter_in == null) return;
            waveWriter_in.Write(e.Buffer, 0, e.BytesRecorded);
            //waveWriter_in.Flush();
        }
        private void WaveOut_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveWriter_out == null) return;
            waveWriter_out.Write(e.Buffer, 0, e.BytesRecorded);
            //waveWriter_out.Flush();
        }
        public void StopRecording()
        {
            if (waveOut != null)
            {
                waveOut.StopRecording();
                waveOut.Dispose();
                waveOut = null;
            }
            if (waveIn != null)
            {
                waveOneOut.Stop();
                waveIn.StopRecording();
                waveIn.Dispose();
            }
            if (this.waveWriter_in != null)
            {
                this.waveWriter_in.Flush();
                this.waveWriter_in.Dispose();
                this.waveWriter_in = null;
            }
            if (this.waveWriter_out != null)
            {
                this.waveWriter_out.Flush();
                this.waveWriter_out.Dispose();
                this.waveWriter_out = null;
            }
            MixFile();
            Startded = false;
        }
        private void MixFile()
        {

            try
            {
                if (!(File.Exists(FileName + "_in" + extention)
                && File.Exists(FileName + "_out" + extention)))
                {
                    return;
                }
                using (var reader1 = new AudioFileReader(FileName + "_in" + extention))
                using (var reader2 = new AudioFileReader(FileName + "_out" + extention))
                {
                    var mixer = new MixingSampleProvider(new[] { reader1, reader2 });
                    WaveFileWriter.CreateWaveFile16(FileName + extention, mixer);
                }
                File.Delete(FileName + "_in" + extention);
                File.Delete(FileName + "_out" + extention);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
