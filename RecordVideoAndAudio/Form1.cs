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


        }

        private void RecorderAudio_DevicesUpdated()
        {
            microphonesComboBox.Invoke((MethodInvoker)delegate
            {
                microphonesComboBox.Items.Clear();

                var list1 = recorderAudio.DevicesOut();
                list1.ForEach(c => microphonesComboBox.Items.Add(c));
            });

            speakerComboBox.Invoke((MethodInvoker)delegate
            {
                speakerComboBox.Items.Clear();
                var list2 = recorderAudio.DevicesIn();
                list2.ForEach(c => speakerComboBox.Items.Add(c));
            });
        }

        private void UpdateListener()
        {
            if (microphonesComboBox.SelectedItem != null
                && speakerComboBox.SelectedItem != null)
            {
                if (listener == null)
                    listener = new ListenerDivice(this);
                else
                    listener.StopListeningForPeakLevel();

                listener.StartListener();
            }
        }

        ListenerDivice listener;
        Config config;
        private void ReadConfig()
        {
            if (!File.Exists("config.json")) return;
            var json = File.ReadAllText("config.json");
            config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(json);
            if (config != null)
            {
                resultFolderTextBox.Text = config.ResultFolder;
                if (!string.IsNullOrEmpty(config.MicrophoneName))
                    microphonesComboBox.SelectedItem = config.MicrophoneName;
                if (!string.IsNullOrEmpty(config.SpeakerName))
                    speakerComboBox.SelectedItem = config.SpeakerName;
                if (config.IsCheckBoxMicrophone.HasValue)
                    checkBoxMicrophone.Checked = config.IsCheckBoxMicrophone.Value;
                if (config.IsCheckBoxSpeaker.HasValue)
                    checkBoxSpeaker.Checked = config.IsCheckBoxSpeaker.Value;
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
                files = files.Concat(dir.GetFiles(ext, SearchOption.AllDirectories));
            }
            return files;
        }
        private void UpdateStatistic()
        {
            var dir = new DirectoryInfo(resultFolderTextBox.Text);
            var files = GetFilesByExtensions(dir, "*");

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
        public RecorderAudio recorderAudio;
        private string fullName;
        private string fileName;
        private RecorderVideo recorderVideo;

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
                }

                if (microphonesComboBox.Visible && microphonesComboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("Select divace microphone");
                    return;
                }
                if (speakerComboBox.Visible && speakerComboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("Select divace speaker");
                    return;
                }
                if (microphonesComboBox.Visible && speakerComboBox.Visible)
                {
                    config.MicrophoneName = (string)microphonesComboBox.SelectedItem;
                    config.SpeakerName = (string)speakerComboBox.SelectedItem;
                    config.IsCheckBoxMicrophone = checkBoxMicrophone.Checked;
                    config.IsCheckBoxSpeaker = checkBoxSpeaker.Checked;
                    SaveConfig();
                }

                isStart = true;
                this.startRecordButton.Enabled = false;
                startRecordButton.BackColor = Color.Gray;
                this.stopRecordButton.Enabled = true;
                stopRecordButton.BackColor = Color.LightCoral;
                isOnlyAudioCheckBox.Enabled = false;
                checkBoxMicrophone.Enabled = false;
                checkBoxSpeaker.Enabled = false;


                //this.Icon = Properties.Resources.IconRecording;

                timeRecord = new TimeSpan();
                timer1.Start();
                this.textBoxFileName.Text = $"record_{DateTime.Now:yyyyMMdd_HHmmss}";
                fileName = this.textBoxFileName.Text;
                fullName = Path.Combine(config.ResultFolder, this.textBoxFileName.Text);
                try
                {
                    recorderAudio.StartRecording(fullName
                        , config.IsCheckBoxMicrophone.Value ? (string)microphonesComboBox.SelectedItem : null
                        , config.IsCheckBoxSpeaker.Value ? (string)speakerComboBox.SelectedItem : null
                        );

                    if (!isOnlyAudioCheckBox.Checked)
                    {
                        var encoder = KnownFourCCs.Codecs.MotionJpeg;
                        var encodingQuality = 70;
                        string aviPath = Path.ChangeExtension(fullName, ".video");
                        recorderVideo.StartRecord(aviPath, encoder, encodingQuality);
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

                //this.Icon = Properties.Resources.IconsPlay;

                recorderAudio.StopRecording();

                if (!isOnlyAudioCheckBox.Checked)
                {
                    recorderVideo.StopRecord();

                    MixingAudioAndVideo();
                }
                else
                {
                    MoveAudio();
                }

                resultLabel.Text = "recorded - ";// + recorderAudio.FileName;
                UpdateStatistic();
            }
        }
        private void MoveAudio()
        {
            GetName(".mp3", true);
        }
        private string GetName(string extention, bool isMove)
        {
            string output = Path.ChangeExtension(fullName, extention);

            if (isMove && !File.Exists(output))
                throw new Exception($"Not exists File {output}");

            if (this.textBoxFileName.Text != fileName)
            {
                string fullNameNew = Path.Combine(config.ResultFolder, this.textBoxFileName.Text);
                string outputNew = Path.ChangeExtension(fullNameNew, extention);
                if (isMove)
                    File.Move(output, outputNew);
                output = outputNew;
            }
            return output;
        }

        private void MixingAudioAndVideo()
        {
            string output = GetName(".mp4", false);
            string mp3Path = GetName(".mp3", true);
            string aviPath = GetName(".video", true);

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
            var mp3PathTemp = Path.Combine(tempFolder, mp3File);
            File.Move(mp3Path, mp3PathTemp);

            var aviFile = Path.GetFileName(aviPath);
            var aviPathTemp = Path.Combine(tempFolder, aviFile);
            File.Move(aviPath, aviPathTemp);
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
            config.IsCheckBoxMicrophone = checkBoxMicrophone.Checked;
            config.IsCheckBoxSpeaker = checkBoxSpeaker.Checked;
            config.MicrophoneName = (string)microphonesComboBox.SelectedItem;
            config.SpeakerName = (string)speakerComboBox.SelectedItem;

            SaveConfig();
            UpdateStatistic();
            saveButton.Enabled = false;
        }

        private void microphonesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListener();
        }

        private void speakerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListener();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetToolTip();

            recorderAudio = new RecorderAudio();
            recorderVideo = new RecorderVideo();

            recorderAudio.DevicesUpdated += RecorderAudio_DevicesUpdated;

            RecorderAudio_DevicesUpdated();

            //Assembly a = Assembly.GetExecutingAssembly();
            //Stream st = a.GetManifestResourceStream("RecordVideoAndAudio.Icon.IconsPlay.ico");
            //this.Icon = new Icon(st);



            ReadConfig();

            if (string.IsNullOrEmpty(resultFolderTextBox.Text))
            {
                resultFolderTextBox.Text = Path.Combine(Environment.CurrentDirectory, "Records");
                config = new Config
                {
                    MicrophoneName = (string)microphonesComboBox.SelectedItem,
                    SpeakerName = (string)speakerComboBox.SelectedItem,
                    ResultFolder = resultFolderTextBox.Text
                };
            }
            Directory.CreateDirectory(resultFolderTextBox.Text);

            UpdateStatistic();

            UpdateListener();
        }

        private void SetToolTip()
        {
            // Create the ToolTip and associate with the Form container.
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.checkBoxSpeaker, "Record Speaker");
            toolTip1.SetToolTip(this.checkBoxMicrophone, "Record Microphone");
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(resultFolderTextBox.Text))
                Process.Start(resultFolderTextBox.Text);
        }

        private void checkBoxMicrophone_CheckedChanged(object sender, EventArgs e)
        {
            if (config != null)
            {
                if (!config.IsCheckBoxMicrophone.HasValue)
                    saveButton.Enabled = true;
                else
                    saveButton.Enabled = config.IsCheckBoxMicrophone.Value != checkBoxMicrophone.Enabled;
            }
            //UpdateListener();
        }

        private void checkBoxSpeaker_CheckedChanged(object sender, EventArgs e)
        {
            if (config != null)
            {
                if (!config.IsCheckBoxSpeaker.HasValue)
                    saveButton.Enabled = true;
                else
                    saveButton.Enabled = config.IsCheckBoxSpeaker.Value != checkBoxSpeaker.Enabled;
            }
            //UpdateListener();
        }
    }
}
