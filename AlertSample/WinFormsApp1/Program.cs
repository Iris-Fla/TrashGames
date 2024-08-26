using System;
using System.Windows.Forms;

namespace WarningApplication
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ShowWarningButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("����͌x�����b�Z�[�W�ł�!", "�x��", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            this.ClientSize = new System.Drawing.Size(300, 150);
            this.Controls.Add(this.ShowWarningButton);
            this.Name = "MainForm";
            this.Text = "�x���A�v���P�[�V����";
            this.ResumeLayout(false);
        }

        private Button ShowWarningButton;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}