using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Timer = System.Windows.Forms.Timer;

namespace WarningApplication
{
    public partial class MainForm : Form
    {
        private Random random = new Random();
        private List<Form> activeWarnings = new List<Form>();
        private readonly Color[] warningColors =
[
            Color.LightBlue,
            Color.LightYellow,
            Color.LightGreen,
            Color.LightCoral
];

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool MessageBeep(uint uType);

        public MainForm()
        {
            InitializeComponent();
            zousyoku();
        }

        private void ShowWarningButton_Click(object? sender, EventArgs e)
        {
            ShowRandomPositionWarning(1);
        }

        private void ShowRandomPositionWarning(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateWarningWindow();
            }
        }

        private void CreateWarningWindow()
        {
            Rectangle workingArea = GetWorkingArea();

            // ランダムな位置を計算
            int newX = random.Next(workingArea.Left, workingArea.Right - 300);
            int newY = random.Next(workingArea.Top, workingArea.Bottom - 150);

            Color randomColor = warningColors[random.Next(warningColors.Length)];

            // カスタムメッセージボックスを作成
            Form customMsgBox = new Form
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(newX, newY),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "警告",
                Size = new Size(300, 150),
                ControlBox = false,
                TopMost = false, // 常に最前面に表示
                BackColor = randomColor // 背景色を設定

            };

            Label label = new Label
            {
                Text = "うひょ～",
                AutoSize = true,
                Location = new Point(20, 20)
            };
            customMsgBox.Controls.Add(label);

            Button okButton = new Button
            {
                Text = "OK",
                Location = new Point(110, 70)
            };
            okButton.Click += (s, e) =>
            {
                customMsgBox.Close();
                activeWarnings.Remove(customMsgBox);
                ShowRandomPositionWarning(1);
            };
            customMsgBox.Controls.Add(okButton);

            customMsgBox.FormClosed += (s, e) => activeWarnings.Remove(customMsgBox);

            // ウィンドウを動かすためのタイマーを設定
            Timer moveTimer = new()
            {
                Interval = 10 // 50ミリ秒ごとに移動 (適宜調整してください)
            };
            moveTimer.Tick += (s, e) => MoveWarningWindow(customMsgBox);
            moveTimer.Start();

            activeWarnings.Add(customMsgBox);
            customMsgBox.Show();

            MessageBeep(0x00000030); // 警告音を鳴らす
        }

        private void MoveWarningWindow(Form warningWindow)
        {
            Rectangle workingArea = GetWorkingArea();
            int speed = 5; // 移動速度 (適宜調整してください)

            // 新しい位置を計算
            int newX = warningWindow.Location.X + speed;
            int newY = warningWindow.Location.Y + speed;

            // 画面の端に到達したら反対側に移動
            if (newX < workingArea.Left) newX = workingArea.Right - warningWindow.Width;
            if (newX > workingArea.Right - warningWindow.Width) newX = workingArea.Left;
            if (newY < workingArea.Top) newY = workingArea.Bottom - warningWindow.Height;
            if (newY > workingArea.Bottom - warningWindow.Height) newY = workingArea.Top;

            warningWindow.Location = new Point(newX,newY);
        }

        private void InitializeComponent()
        {
            this.ShowWarningButton = new Button();
            this.SuspendLayout();
            // ShowWarningButton
            this.ShowWarningButton.Location = new System.Drawing.Point(50, 50);
            this.ShowWarningButton.Name = "ShowWarningButton";
            this.ShowWarningButton.Size = new System.Drawing.Size(200, 30);
            this.ShowWarningButton.Text = "警告を表示";
            this.ShowWarningButton.Click += new EventHandler(ShowWarningButton_Click);
            // MainForm
            this.ClientSize = new Size(300, 150);
            this.Controls.Add(this.ShowWarningButton);
            this.Name = "MainForm";
            this.Text = "警告アプリケーション";
            this.ResumeLayout(false);
        }

        private void zousyoku()
        {
            Timer zousyoku = new()
            {
                Interval = 500
            };
            zousyoku.Tick += (s, e) =>
            {
                ShowRandomPositionWarning(1);
            };
            zousyoku.Start();
        }

        private Rectangle GetWorkingArea()
        {
            if (Screen.PrimaryScreen == null)
            {
                return new Rectangle(0, 0, 1024, 768); // デフォルトの解像度
            }
            return Screen.PrimaryScreen.WorkingArea;
        }

        private Button ShowWarningButton = new Button();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}