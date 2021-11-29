namespace RecordVideoAndAudio
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.startRecordButton = new System.Windows.Forms.Button();
            this.stopRecordButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timeValueLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.microphonesComboBox = new System.Windows.Forms.ComboBox();
            this.microphoneLabel = new System.Windows.Forms.Label();
            this.resultFolderTextBox = new System.Windows.Forms.TextBox();
            this.resultFolderLabel = new System.Windows.Forms.Label();
            this.StatisticLabel = new System.Windows.Forms.Label();
            this.statisticValueLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.speakerLabel = new System.Windows.Forms.Label();
            this.speakerComboBox = new System.Windows.Forms.ComboBox();
            this.isOnlyAudioCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBarMicrophone = new System.Windows.Forms.ProgressBar();
            this.progressBarSpeaker = new System.Windows.Forms.ProgressBar();
            this.buttonOpenFolder = new System.Windows.Forms.Button();
            this.checkBoxMicrophone = new System.Windows.Forms.CheckBox();
            this.checkBoxSpeaker = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.checkBoxNotListener = new System.Windows.Forms.CheckBox();
            this.linkLabelError = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // startRecordButton
            // 
            this.startRecordButton.BackColor = System.Drawing.Color.PaleGreen;
            this.startRecordButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startRecordButton.Location = new System.Drawing.Point(12, 142);
            this.startRecordButton.Name = "startRecordButton";
            this.startRecordButton.Size = new System.Drawing.Size(123, 43);
            this.startRecordButton.TabIndex = 0;
            this.startRecordButton.Text = "Start Record";
            this.startRecordButton.UseVisualStyleBackColor = false;
            this.startRecordButton.Click += new System.EventHandler(this.startRecordButton_Click);
            // 
            // stopRecordButton
            // 
            this.stopRecordButton.BackColor = System.Drawing.Color.Gray;
            this.stopRecordButton.Enabled = false;
            this.stopRecordButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopRecordButton.Location = new System.Drawing.Point(141, 142);
            this.stopRecordButton.Margin = new System.Windows.Forms.Padding(0);
            this.stopRecordButton.Name = "stopRecordButton";
            this.stopRecordButton.Size = new System.Drawing.Size(123, 43);
            this.stopRecordButton.TabIndex = 1;
            this.stopRecordButton.Text = "Stop Record";
            this.stopRecordButton.UseVisualStyleBackColor = false;
            this.stopRecordButton.Click += new System.EventHandler(this.stopRecordButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Time:";
            // 
            // timeValueLabel
            // 
            this.timeValueLabel.AutoSize = true;
            this.timeValueLabel.Location = new System.Drawing.Point(51, 192);
            this.timeValueLabel.Name = "timeValueLabel";
            this.timeValueLabel.Size = new System.Drawing.Size(49, 13);
            this.timeValueLabel.TabIndex = 3;
            this.timeValueLabel.Text = "00:00:00";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Result";
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Location = new System.Drawing.Point(54, 209);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(0, 13);
            this.resultLabel.TabIndex = 5;
            // 
            // microphonesComboBox
            // 
            this.microphonesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.microphonesComboBox.FormattingEnabled = true;
            this.microphonesComboBox.Location = new System.Drawing.Point(87, 12);
            this.microphonesComboBox.Name = "microphonesComboBox";
            this.microphonesComboBox.Size = new System.Drawing.Size(292, 21);
            this.microphonesComboBox.TabIndex = 6;
            this.microphonesComboBox.SelectedIndexChanged += new System.EventHandler(this.microphonesComboBox_SelectedIndexChanged);
            // 
            // microphoneLabel
            // 
            this.microphoneLabel.AutoSize = true;
            this.microphoneLabel.Location = new System.Drawing.Point(12, 15);
            this.microphoneLabel.Name = "microphoneLabel";
            this.microphoneLabel.Size = new System.Drawing.Size(63, 13);
            this.microphoneLabel.TabIndex = 7;
            this.microphoneLabel.Text = "Microphone";
            // 
            // resultFolderTextBox
            // 
            this.resultFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultFolderTextBox.Location = new System.Drawing.Point(12, 307);
            this.resultFolderTextBox.Name = "resultFolderTextBox";
            this.resultFolderTextBox.Size = new System.Drawing.Size(308, 20);
            this.resultFolderTextBox.TabIndex = 8;
            this.resultFolderTextBox.TextChanged += new System.EventHandler(this.resultFolderTextBox_TextChanged);
            // 
            // resultFolderLabel
            // 
            this.resultFolderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resultFolderLabel.AutoSize = true;
            this.resultFolderLabel.Location = new System.Drawing.Point(13, 288);
            this.resultFolderLabel.Name = "resultFolderLabel";
            this.resultFolderLabel.Size = new System.Drawing.Size(66, 13);
            this.resultFolderLabel.TabIndex = 9;
            this.resultFolderLabel.Text = "Result folder";
            // 
            // StatisticLabel
            // 
            this.StatisticLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StatisticLabel.AutoSize = true;
            this.StatisticLabel.Location = new System.Drawing.Point(12, 334);
            this.StatisticLabel.Name = "StatisticLabel";
            this.StatisticLabel.Size = new System.Drawing.Size(47, 13);
            this.StatisticLabel.TabIndex = 10;
            this.StatisticLabel.Text = "Statistic:";
            // 
            // statisticValueLabel
            // 
            this.statisticValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statisticValueLabel.AutoSize = true;
            this.statisticValueLabel.Location = new System.Drawing.Point(66, 334);
            this.statisticValueLabel.Name = "statisticValueLabel";
            this.statisticValueLabel.Size = new System.Drawing.Size(0, 13);
            this.statisticValueLabel.TabIndex = 11;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Enabled = false;
            this.saveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(200)));
            this.saveButton.Location = new System.Drawing.Point(323, 279);
            this.saveButton.Margin = new System.Windows.Forms.Padding(0);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(69, 21);
            this.saveButton.TabIndex = 12;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // speakerLabel
            // 
            this.speakerLabel.AutoSize = true;
            this.speakerLabel.Location = new System.Drawing.Point(12, 42);
            this.speakerLabel.Name = "speakerLabel";
            this.speakerLabel.Size = new System.Drawing.Size(47, 13);
            this.speakerLabel.TabIndex = 14;
            this.speakerLabel.Text = "Speaker";
            // 
            // speakerComboBox
            // 
            this.speakerComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.speakerComboBox.FormattingEnabled = true;
            this.speakerComboBox.Location = new System.Drawing.Point(87, 39);
            this.speakerComboBox.Name = "speakerComboBox";
            this.speakerComboBox.Size = new System.Drawing.Size(292, 21);
            this.speakerComboBox.TabIndex = 13;
            this.speakerComboBox.SelectedIndexChanged += new System.EventHandler(this.speakerComboBox_SelectedIndexChanged);
            // 
            // isOnlyAudioCheckBox
            // 
            this.isOnlyAudioCheckBox.AutoSize = true;
            this.isOnlyAudioCheckBox.Location = new System.Drawing.Point(12, 120);
            this.isOnlyAudioCheckBox.Name = "isOnlyAudioCheckBox";
            this.isOnlyAudioCheckBox.Size = new System.Drawing.Size(77, 17);
            this.isOnlyAudioCheckBox.TabIndex = 15;
            this.isOnlyAudioCheckBox.Text = "Only Audio";
            this.isOnlyAudioCheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Speaker";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Microphone";
            // 
            // progressBarMicrophone
            // 
            this.progressBarMicrophone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarMicrophone.Location = new System.Drawing.Point(87, 67);
            this.progressBarMicrophone.Name = "progressBarMicrophone";
            this.progressBarMicrophone.Size = new System.Drawing.Size(313, 11);
            this.progressBarMicrophone.TabIndex = 18;
            // 
            // progressBarSpeaker
            // 
            this.progressBarSpeaker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarSpeaker.Location = new System.Drawing.Point(87, 86);
            this.progressBarSpeaker.Name = "progressBarSpeaker";
            this.progressBarSpeaker.Size = new System.Drawing.Size(313, 11);
            this.progressBarSpeaker.TabIndex = 19;
            // 
            // buttonOpenFolder
            // 
            this.buttonOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(200)));
            this.buttonOpenFolder.Location = new System.Drawing.Point(323, 307);
            this.buttonOpenFolder.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOpenFolder.Name = "buttonOpenFolder";
            this.buttonOpenFolder.Size = new System.Drawing.Size(69, 21);
            this.buttonOpenFolder.TabIndex = 20;
            this.buttonOpenFolder.Text = "Open";
            this.buttonOpenFolder.UseVisualStyleBackColor = true;
            this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
            // 
            // checkBoxMicrophone
            // 
            this.checkBoxMicrophone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxMicrophone.AutoSize = true;
            this.checkBoxMicrophone.Checked = true;
            this.checkBoxMicrophone.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMicrophone.Location = new System.Drawing.Point(385, 15);
            this.checkBoxMicrophone.Name = "checkBoxMicrophone";
            this.checkBoxMicrophone.Size = new System.Drawing.Size(15, 14);
            this.checkBoxMicrophone.TabIndex = 21;
            this.checkBoxMicrophone.UseVisualStyleBackColor = true;
            this.checkBoxMicrophone.CheckedChanged += new System.EventHandler(this.checkBoxMicrophone_CheckedChanged);
            // 
            // checkBoxSpeaker
            // 
            this.checkBoxSpeaker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxSpeaker.AutoSize = true;
            this.checkBoxSpeaker.Checked = true;
            this.checkBoxSpeaker.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSpeaker.Location = new System.Drawing.Point(385, 42);
            this.checkBoxSpeaker.Name = "checkBoxSpeaker";
            this.checkBoxSpeaker.Size = new System.Drawing.Size(15, 14);
            this.checkBoxSpeaker.TabIndex = 22;
            this.checkBoxSpeaker.UseVisualStyleBackColor = true;
            this.checkBoxSpeaker.CheckedChanged += new System.EventHandler(this.checkBoxSpeaker_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 230);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "File Name";
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFileName.Location = new System.Drawing.Point(13, 249);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(308, 20);
            this.textBoxFileName.TabIndex = 23;
            // 
            // checkBoxNotListener
            // 
            this.checkBoxNotListener.AutoSize = true;
            this.checkBoxNotListener.Location = new System.Drawing.Point(95, 119);
            this.checkBoxNotListener.Name = "checkBoxNotListener";
            this.checkBoxNotListener.Size = new System.Drawing.Size(74, 17);
            this.checkBoxNotListener.TabIndex = 25;
            this.checkBoxNotListener.Text = "Not Listen";
            this.checkBoxNotListener.UseVisualStyleBackColor = true;
            this.checkBoxNotListener.Visible = false;
            // 
            // linkLabelError
            // 
            this.linkLabelError.AutoSize = true;
            this.linkLabelError.Location = new System.Drawing.Point(268, 156);
            this.linkLabelError.Name = "linkLabelError";
            this.linkLabelError.Size = new System.Drawing.Size(34, 13);
            this.linkLabelError.TabIndex = 26;
            this.linkLabelError.TabStop = true;
            this.linkLabelError.Text = "Errors";
            this.linkLabelError.Visible = false;
            this.linkLabelError.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelError_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 354);
            this.Controls.Add(this.linkLabelError);
            this.Controls.Add(this.checkBoxNotListener);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.checkBoxSpeaker);
            this.Controls.Add(this.checkBoxMicrophone);
            this.Controls.Add(this.buttonOpenFolder);
            this.Controls.Add(this.progressBarSpeaker);
            this.Controls.Add(this.progressBarMicrophone);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.isOnlyAudioCheckBox);
            this.Controls.Add(this.speakerLabel);
            this.Controls.Add(this.speakerComboBox);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.statisticValueLabel);
            this.Controls.Add(this.StatisticLabel);
            this.Controls.Add(this.resultFolderLabel);
            this.Controls.Add(this.resultFolderTextBox);
            this.Controls.Add(this.microphoneLabel);
            this.Controls.Add(this.microphonesComboBox);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timeValueLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stopRecordButton);
            this.Controls.Add(this.startRecordButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Record Video And Audio";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startRecordButton;
        private System.Windows.Forms.Button stopRecordButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label timeValueLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.Label microphoneLabel;
        private System.Windows.Forms.TextBox resultFolderTextBox;
        private System.Windows.Forms.Label resultFolderLabel;
        private System.Windows.Forms.Label StatisticLabel;
        private System.Windows.Forms.Label statisticValueLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label speakerLabel;
        private System.Windows.Forms.CheckBox isOnlyAudioCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox microphonesComboBox;
        public System.Windows.Forms.ComboBox speakerComboBox;
        public System.Windows.Forms.ProgressBar progressBarMicrophone;
        public System.Windows.Forms.ProgressBar progressBarSpeaker;
        private System.Windows.Forms.Button buttonOpenFolder;
        private System.Windows.Forms.CheckBox checkBoxMicrophone;
        private System.Windows.Forms.CheckBox checkBoxSpeaker;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.CheckBox checkBoxNotListener;
        private System.Windows.Forms.LinkLabel linkLabelError;
    }
}

