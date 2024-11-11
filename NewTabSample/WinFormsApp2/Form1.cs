using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Timer = System.Windows.Forms.Timer;
using System.Drawing;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {

        private Timer timer;
        private Button startButton;
        private Button stopButton;
        private Label countdownLabel;
        private Random random;
        private List<string> websites;
        private int countdown;
        private PictureBox pictureBox;

        public Form1()
        {
            InitializeComponent();
            InitializeFormSettings();
            InitializeRandomJumper();
            
        }

        private void InitializeFormSettings()
        {
            // ��{�T�C�Y�̐ݒ�
            this.Size = new Size(200, 500);

            // �ŏ�/�ő�T�C�Y�̐���
            this.MinimumSize = new Size(400, 300);
            this.MaximumSize = new Size(1200, 900);

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(1500, 500);

            // �^�C�g���ݒ�
            this.Text = "�����ς��\�����邨";

            // �T�C�Y�ύX�\�ɐݒ�
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.ControlBox = false;
            pictureBox = new PictureBox
            {
                Size = new Size(600, 400),
                Location = new Point(-100, 50),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            using (var client = new System.Net.WebClient())
            {
                byte[] data = client.DownloadData("https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEh7-UGizrD-yvLdfmLH2_UtkASeHB7486S9rUKUy2cuPaSMufCvYb5R7JKeA2c-wzcXPnHIH1Ly_9-la89QDdpYlE2BHJMksH3yVdfHTsliasDw3pIm1IJ9H1uD1Qxs0A9Rsb_mxE7edx0c/s450/party_bunny_girl.png");
                using (var ms = new System.IO.MemoryStream(data))
                {
                    pictureBox.Image = Image.FromStream(ms);
                }
            }
            Controls.Add(pictureBox);
        }

        private void InitializeRandomJumper()
        {
            // ���S�ȃE�F�u�T�C�g�̃��X�g��������
            websites = new List<string>
            {
                "https://www.irasutoya.com/2017/01/blog-post_275.html",
                "https://www.irasutoya.com/2017/01/blog-post_275.html",
                "https://www.irasutoya.com/2021/05/blog-post312.html",
                "https://www.irasutoya.com/2013/03/blog-post_2894.html",
                "https://www.irasutoya.com/2024/04/blog-post_16.html",
                // �K�v�ɉ�����URL��ǉ��ł��܂�
            };

            random = new Random();
            countdown = 3;

            // �^�C�}�[�̐ݒ�
            timer = new Timer
            {
                Interval = 1000 // 1�b�Ԋu
            };
            timer.Tick += Timer_Tick;

            // �X�^�[�g�{�^���̍쐬
            startButton = new Button
            {
                Location = new Point(12, 12),
                Size = new Size(75, 23),
                Text = "�J�n"
            };
            startButton.Click += StartButton_Click;

            // �X�g�b�v�{�^���̍쐬
            stopButton = new Button
            {
                Location = new Point(93, 12),
                Size = new Size(75, 23),
                Text = "��~",
                Enabled = false
            };
            stopButton.Click += StopButton_Click;

            // �J�E���g�_�E���\���p���x��
            countdownLabel = new Label
            {
                Location = new Point(174, 16),
                Size = new Size(100, 23),
                Text = "�ҋ@��..."
            };

            // �t�H�[���ɃR���g���[����ǉ�
            Controls.AddRange(new Control[] { startButton, stopButton, countdownLabel });
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            countdown = 3;
            countdownLabel.Text = $"�c�� {countdown} �b";
            startButton.Enabled = false;
            stopButton.Enabled = true;
            timer.Start();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            startButton.Enabled = true;
            stopButton.Enabled = false;
            countdownLabel.Text = "��~��...";
        }

        private void OpenUrlInNewTab(string url)
        {
            try
            {
                // OS�ɂ���ď����𕪂���
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Windows�̏ꍇ
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c start {url}",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    // macOS�̏ꍇ
                    Process.Start("open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // Linux�̏ꍇ
                    Process.Start("xdg-open", url);
                }
                else
                {
                    // ���̑���OS�̏ꍇ�A�W���̕��@�ŊJ��
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"URL���J���ۂɃG���[���������܂���: {ex.Message}");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            countdown--;
            countdownLabel.Text = $"�c�� {countdown} �b";

            if (countdown <= 0)
            {
                // �����_����URL��I��
                string randomUrl = websites[random.Next(websites.Count)];

                try
                {
                    // �V�����^�u��URL���J��
                    OpenUrlInNewTab(randomUrl);

                    // �J�E���g�_�E�������Z�b�g
                    countdown = 3;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"�G���[���������܂���: {ex.Message}");
                    StopButton_Click(sender, e);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timer.Stop();
            base.OnFormClosing(e);
        }
    }
}