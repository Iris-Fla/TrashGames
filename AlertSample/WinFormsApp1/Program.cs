using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Timer = System.Windows.Forms.Timer;

namespace WarningApplication
{
    public partial class  : Form
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

            // �����_���Ȉʒu���v�Z
            int newX = random.Next(workingArea.Left, workingArea.Right - 300);
            int newY = random.Next(workingArea.Top, workingArea.Bottom - 150);

            Color randomColor = warningColors[random.Next(warningColors.Length)];

            // �J�X�^�����b�Z�[�W�{�b�N�X���쐬
            Form customMsgBox = new Form
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(newX, newY),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "�x��",
                Size = new Size(300, 150),
                ControlBox = false,
                TopMost = false, // ��ɍőO�ʂɕ\��
                BackColor = randomColor // �w�i�F��ݒ�

            };

            Label label = new Label
            {
                Text = "���Ђ�`",
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

            // �E�B���h�E�𓮂������߂̃^�C�}�[��ݒ�
            Timer moveTimer = new()
            {
                Interval = 10 // 50�~���b���ƂɈړ� (�K�X�������Ă�������)
            };
            moveTimer.Tick += (s, e) => MoveWarningWindow(customMsgBox);
            moveTimer.Start();

            activeWarnings.Add(customMsgBox);
            customMsgBox.Show();

            MessageBeep(0x00000030); // �x������炷
        }

        private void MoveWarningWindow(Form warningWindow)
        {
            Rectangle workingArea = GetWorkingArea();
            int speed = 5; // �ړ����x (�K�X�������Ă�������)

            // �V�����ʒu���v�Z
            int newX = warningWindow.Location.X + speed;
            int newY = warningWindow.Location.Y + speed;

            // ��ʂ̒[�ɓ��B�����甽�Α��Ɉړ�
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
            this.ShowWarningButton.Text = "�x����\��";
            this.ShowWarningButton.Click += new EventHandler(ShowWarningButton_Click);
            // MainForm
            this.ClientSize = new Size(300, 150);
            this.Controls.Add(this.ShowWarningButton);
            this.Name = "MainForm";
            this.Text = "�x���A�v���P�[�V����";
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
                return new Rectangle(0, 0, 1024, 768); // �f�t�H���g�̉𑜓x
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