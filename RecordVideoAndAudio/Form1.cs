using SharpAvi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RecordVideoAndAudio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ///
            /// https://github.com/BtbN/FFmpeg-Builds/releases
            ///install ffmpeg-N-99919-g5bb313e723-win64-gpl-shared
            ///include to PATH

            recorderAudio = new RecorderAudio();
            recorderVideo = new RecorderVideo(recorderAudio);

            
            Assembly a = Assembly.GetExecutingAssembly();
            Stream st = a.GetManifestResourceStream("RecordVideoAndAudio.Icon.IconsPlay.ico");
            this.Icon = new Icon(st);

            //var list1 = recorderAudio.DevicesOut();
            //list1.ForEach(c => microphonesComboBox.Items.Add(c));

            //var list2 = recorderAudio.DevicesIn();
            //list2.ForEach(c => speakerComboBox.Items.Add(c));

            ReadConfig();

            if (string.IsNullOrEmpty(resultFolderTextBox.Text))
            {
                resultFolderTextBox.Text = Path.Combine(Environment.CurrentDirectory, "Records");
                config = new Config
                {
                    ResultFolder = resultFolderTextBox.Text
                };
            }
            Directory.CreateDirectory(resultFolderTextBox.Text);

            UpdateStatistic();
        }
        Config config;
        private void ReadConfig()
        {
            if (!File.Exists("config.json")) return;
            var json = File.ReadAllText("config.json");
            config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(json);
            if (config != null)
            {
                resultFolderTextBox.Text = config.ResultFolder;
                //if (config.MicrophoneIndex.HasValue)
                //    microphonesComboBox.SelectedIndex = config.MicrophoneIndex.Value;
                //if (config.SpeakerIndex.HasValue)
                //    speakerComboBox.SelectedIndex = config.SpeakerIndex.Value;
            }
        }
        private void SaveConfig()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(config);
            File.WriteAllText("config.json", json);
        }
        public IEnumerable<FileInfo> GetFilesByExtensions(DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = Enumerable.Empty<FileInfo>();
            foreach (string ext in extensions)
            {
                files = files.Concat(dir.GetFiles(ext));
            }
            return files;
        }
        private void UpdateStatistic()
        {
            var dir = new DirectoryInfo(resultFolderTextBox.Text);
            var files = GetFilesByExtensions(dir,"*.mp4","*.mp3");

            var bytes = files.Sum(c => c.Length);

            string size = FrendlySizy(bytes);

            statisticValueLabel.Text = $"Count: {files.Count()}, Size: {size}";
        }

        private string FrendlySizy(long bytes)
        {
            string[] m = new string[] { "byte", "Кб", "Мб", "Гб", "Тб" };
            decimal sizeValue = bytes;
            int index = 0;
            while (sizeValue > 1023)
            {
                index++;
                sizeValue = Math.Round(sizeValue / 1024, 2);
            }
            return $"{sizeValue} {m[index]}";
        }

        bool isStart = false;
        TimeSpan timeRecord = new TimeSpan();
        private readonly RecorderAudio recorderAudio;
        private string fileName;
        private readonly RecorderVideo recorderVideo;

        private void startRecordButton_Click(object sender, EventArgs e)
        {
            if (!isStart)
            {
                if (microphonesComboBox.Visible && speakerComboBox.Visible)
                {
                    if (microphonesComboBox.SelectedIndex < 0 && speakerComboBox.SelectedIndex < 0)
                    {
                        MessageBox.Show("Select divace for record");
                        return;
                    }


                    //if (microphonesComboBox.SelectedIndex < 0)
                    //{
                    //    MessageBox.Show("Select divace microphone");
                    //    return;
                    //}
                    //if (speakerComboBox.SelectedIndex < 0)
                    //{
                    //    MessageBox.Show("Select divace speaker");
                    //    return;
                    //}
                    config.MicrophoneIndex = microphonesComboBox.SelectedIndex;
                    config.SpeakerIndex = speakerComboBox.SelectedIndex;
                    SaveConfig();
                }

                isStart = true;
                this.startRecordButton.Enabled = false;
                startRecordButton.BackColor = Color.Gray;
                this.stopRecordButton.Enabled = true;
                stopRecordButton.BackColor = Color.LightCoral;
                isOnlyAudioCheckBox.Enabled = false;

                Assembly a = Assembly.GetExecutingAssembly();
                Stream st = a.GetManifestResourceStream("RecordVideoAndAudio.Icon.IconRecording.ico");
                this.Icon = new Icon(st);

                timeRecord = new TimeSpan();
                timer1.Start();                

                fileName = Path.Combine(config.ResultFolder, $"record_{DateTime.Now:yyyyMMdd_HHmmss}");
                try
                {
                    

                    if (!isOnlyAudioCheckBox.Checked)
                    {
                        var encoder = KnownFourCCs.Codecs.MotionJpeg;
                        var encodingQuality = 70;
                        recorderVideo.StartRecord(fileName + "_video.mp4", encoder, encodingQuality);
                    }
                    else
                    {
                        recorderAudio.StartRecording(fileName, microphonesComboBox.SelectedIndex,
                        speakerComboBox.SelectedIndex);
                    }

                    resultLabel.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void stopRecordButton_Click(object sender, EventArgs e)
        {
            if (isStart)
            {
                isStart = false;
                this.startRecordButton.Enabled = true;
                this.stopRecordButton.Enabled = false;
                startRecordButton.BackColor = Color.PaleGreen;
                stopRecordButton.BackColor = Color.Gray;
                isOnlyAudioCheckBox.Enabled = true;
                timer1.Stop();

                Assembly a = Assembly.GetExecutingAssembly();
                Stream st = a.GetManifestResourceStream("RecordVideoAndAudio.Icon.IconsPlay.ico");
                this.Icon = new Icon(st);

                try
                {
                    recorderAudio.StopRecording();

                    if (!isOnlyAudioCheckBox.Checked)
                    {
                        recorderVideo.StopRecord();

                        //MixingAudioAndVideo(); 
                    }

                    resultLabel.Text = "recorded - " + recorderAudio.FileName;
                    UpdateStatistic();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void MixingAudioAndVideo()
        {
            string output = Path.ChangeExtension(fileName, ".mp4");
            string mp3Path = Path.ChangeExtension(fileName, ".mp3");
            string aviPath = fileName + "_video";

            if (!File.Exists(mp3Path))
                throw new Exception($"Not exists File {mp3Path}");
            if (!File.Exists(aviPath))
                throw new Exception($"Not exists File {aviPath}");

            string args = $"-i \"{aviPath}\" -i \"{mp3Path}\" -shortest {output}";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.FileName = Path.Combine(Environment.CurrentDirectory, @"lib\ffmpeg\ffmpeg.exe");

            startInfo.Arguments = args;
            using (Process exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }

            if (!File.Exists(output))
                throw new Exception($"Not exists File {output}");

            if (File.ReadAllBytes(output).Length == 0)
                throw new Exception($"File exists but is empty {output}");

            var tempFolder = Path.Combine(config.ResultFolder, "Temp");
            Directory.CreateDirectory(tempFolder);

            var mp3File = Path.GetFileName(mp3Path);
            var mp3PathNew = Path.Combine(tempFolder, mp3File);
            File.Move(mp3Path, mp3PathNew);

            var aviFile = Path.GetFileName(aviPath);
            var aviPathNew = Path.Combine(tempFolder, aviFile);
            File.Move(aviPath, aviPathNew);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeRecord = timeRecord.Add(TimeSpan.FromSeconds(1));
            timeValueLabel.Text = timeRecord.ToString("hh\\:mm\\:ss");
        }

        private void resultFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            if (config != null)
                saveButton.Enabled = config.ResultFolder != resultFolderTextBox.Text;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            config.ResultFolder = resultFolderTextBox.Text;
            SaveConfig();
            UpdateStatistic();
            saveButton.Enabled = false;
        }
    }
}
