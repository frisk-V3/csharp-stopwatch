using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

// 1. 名前空間（Namespace）で囲むことで、トップレベルのエラーを防ぐよ
namespace StopwatchApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StopwatchForm());
        }
    }

    public class StopwatchForm : Form
    {
        private Label lblTime;
        private Button btnStartStop;
        private Button btnReset;
        private System.Windows.Forms.Timer mainTimer;
        private Stopwatch sw = new Stopwatch();

        public StopwatchForm()
        {
            this.Text = "爆速ストップウォッチ";
            this.Size = new Size(300, 220);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTime = new Label()
            {
                Text = "00:00:00.0",
                Font = new Font("Consolas", 24, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 80,
                ForeColor = Color.DarkSlateBlue
            };

            btnStartStop = new Button()
            {
                Text = "START",
                Size = new Size(100, 45),
                Location = new Point(40, 100),
                BackColor = Color.LightGreen
            };
            // 2. イベント登録。BtnStartStop_Clickの中身を修正したからエラー消えるよ
            btnStartStop.Click += BtnStartStop_Click;

            btnReset = new Button()
            {
                Text = "RESET",
                Size = new Size(100, 45),
                Location = new Point(150, 100)
            };
            // 3. ラムダ式の (s, e) も、型を明示するか「?」を意識すると通るよ
            btnReset.Click += (object? sender, EventArgs e) => {
                sw.Reset();
                UpdateLabel();
                btnStartStop.Text = "START";
                btnStartStop.BackColor = Color.LightGreen;
            };

            mainTimer = new System.Windows.Forms.Timer() { Interval = 100 };
            mainTimer.Tick += (object? sender, EventArgs e) => UpdateLabel();

            this.Controls.Add(lblTime);
            this.Controls.Add(btnStartStop);
            this.Controls.Add(btnReset);
        }

        // 4. 「object?」にしてNull許容の警告を回避！
        private void BtnStartStop_Click(object? sender, EventArgs e)
        {
            if (sw.IsRunning)
            {
                sw.Stop();
                mainTimer.Stop();
                btnStartStop.Text = "START";
                btnStartStop.BackColor = Color.LightGreen;
            }
            else
            {
                sw.Start();
                mainTimer.Start();
                btnStartStop.Text = "STOP";
                btnStartStop.BackColor = Color.Tomato;
            }
        }

        private void UpdateLabel()
        {
            TimeSpan ts = sw.Elapsed;
            lblTime.Text = string.Format("{0:00}:{1:00}:{2:00}.{3:0}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 100);
        }
    }
}
