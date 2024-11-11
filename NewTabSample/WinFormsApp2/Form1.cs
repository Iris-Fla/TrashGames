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
            // 基本サイズの設定
            this.Size = new Size(200, 500);

            // 最小/最大サイズの制限
            this.MinimumSize = new Size(400, 300);
            this.MaximumSize = new Size(1200, 900);

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(1500, 500);

            // タイトル設定
            this.Text = "いっぱい表示するお";

            // サイズ変更可能に設定
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
            // 安全なウェブサイトのリストを初期化
            websites = new List<string>
            {
                "https://www.irasutoya.com/2017/01/blog-post_275.html",
                "https://www.irasutoya.com/2017/01/blog-post_275.html",
                "https://www.irasutoya.com/2021/05/blog-post312.html",
                "https://www.irasutoya.com/2013/03/blog-post_2894.html",
                "https://www.irasutoya.com/2024/04/blog-post_16.html",
                // 必要に応じてURLを追加できます
            };

            random = new Random();
            countdown = 3;

            // タイマーの設定
            timer = new Timer
            {
                Interval = 1000 // 1秒間隔
            };
            timer.Tick += Timer_Tick;

            // スタートボタンの作成
            startButton = new Button
            {
                Location = new Point(12, 12),
                Size = new Size(75, 23),
                Text = "開始"
            };
            startButton.Click += StartButton_Click;

            // ストップボタンの作成
            stopButton = new Button
            {
                Location = new Point(93, 12),
                Size = new Size(75, 23),
                Text = "停止",
                Enabled = false
            };
            stopButton.Click += StopButton_Click;

            // カウントダウン表示用ラベル
            countdownLabel = new Label
            {
                Location = new Point(174, 16),
                Size = new Size(100, 23),
                Text = "待機中..."
            };

            // フォームにコントロールを追加
            Controls.AddRange(new Control[] { startButton, stopButton, countdownLabel });
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            countdown = 3;
            countdownLabel.Text = $"残り {countdown} 秒";
            startButton.Enabled = false;
            stopButton.Enabled = true;
            timer.Start();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            startButton.Enabled = true;
            stopButton.Enabled = false;
            countdownLabel.Text = "停止中...";
        }

        private void OpenUrlInNewTab(string url)
        {
            try
            {
                // OSによって処理を分ける
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Windowsの場合
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
                    // macOSの場合
                    Process.Start("open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // Linuxの場合
                    Process.Start("xdg-open", url);
                }
                else
                {
                    // その他のOSの場合、標準の方法で開く
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"URLを開く際にエラーが発生しました: {ex.Message}");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            countdown--;
            countdownLabel.Text = $"残り {countdown} 秒";

            if (countdown <= 0)
            {
                // ランダムなURLを選択
                string randomUrl = websites[random.Next(websites.Count)];

                try
                {
                    // 新しいタブでURLを開く
                    OpenUrlInNewTab(randomUrl);

                    // カウントダウンをリセット
                    countdown = 3;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"エラーが発生しました: {ex.Message}");
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