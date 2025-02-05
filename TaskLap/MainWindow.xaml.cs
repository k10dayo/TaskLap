using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace TaskLap
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        //[Required]
        //[RegularExpression(@"^[0-9]*$", ErrorMessage = "0～9 の数字のみ入力できます")]
        //public BindableReactiveProperty<int?> TicketNumber { get; } = new BindableReactiveProperty<int?>().EnableValidation<MainWindow>();
        //Xaml側↓
        //<TextBox x:Name="InputTicket" Text="{Binding TicketNumber.Value, UpdateSourceTrigger=PropertyChanged}" TextAlignment="Center" VerticalContentAlignment="Center"/>

        private int previousMinute;

        public MainWindow()
        {
            InitializeComponent();

            //DataContext = this;
            // DispatcherTimer を設定して1秒ごとに時間をチェック
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void LapRecordButtonClick(object sender, RoutedEventArgs e)
        {
            LapRecordWindow recordWindow = new LapRecordWindow();
            recordWindow.Show();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // これでEnterの既定動作（改行など）を防ぐ

                if(InputTicket.Text != string.Empty)
                {
                    CurrentTicket.Text = "A231-" + InputTicket.Text;
                }

                CurrentWork.Text = InputWork.Text;
                InputWork.Text = string.Empty;

                //現在時刻入れる
                LapStartTime.Text = DateTime.Now.ToString("HH:mm");

                //経過時間をリセット
                ElapsedTime.Text = (TimeSpan.Zero).ToString(@"hh\:mm");
            }
        }

        private void InputTicket_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 0-9 以外の入力を拒否
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]$");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int currentMinute = DateTime.Now.Minute;

            // 分が変わった場合
            if (currentMinute != previousMinute)
            {
                // 前回の分を更新
                previousMinute = currentMinute;

                TimeSpan currentTime;
                if (TimeSpan.TryParseExact(ElapsedTime.Text, @"hh\:mm", null, out currentTime))
                {
                    // 1分追加
                    currentTime = currentTime.Add(TimeSpan.FromMinutes(1));

                    // 新しい時刻を表示 (hh:mm 形式)
                    ElapsedTime.Text = currentTime.ToString(@"hh\:mm");
                }
            }
        }
    }
}
