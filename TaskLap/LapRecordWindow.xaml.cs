using System.Diagnostics;
using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TaskLap
{
    public partial class LapRecordWindow : Window
    {
        private string filePath = "Log.csv"; // CSVファイルのパス

        public List<LapRecord> LapRecords { get; set; }

        public LapRecordWindow()
        {
            InitializeComponent();
            LapRecords = new List<LapRecord>();
            LapRecordList.ItemsSource = LapRecords;  // ListViewにデータをバインド
            LoadCsv();
        }

        // LapRecordデータモデル
        public class LapRecord
        {
            public string Timestamp { get; set; }
            public string Ticket { get; set; }
            public string StartTime { get; set; }
            public string ElapsedTime { get; set; }
            public string Work { get; set; }
            public string Comment { get; set; }
            
        }

        private void LoadCsv()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Debug.Print("CSVファイルが見つかりません。");
                    return;
                }

                using (var reader = new StreamReader(filePath))
                {
                    bool isFirst = true;

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (isFirst)
                        {
                            isFirst = false;
                            continue;
                        }
                        if (line != null)
                        {
                            // CSVの各行を分割
                            var values = line.Split(',');

                            if (values.Length >= 5)
                            {                              
                                // LapRecordオブジェクトを作成してリストに追加
                                LapRecords.Add(new LapRecord
                                {
                                    Timestamp = values[0],
                                    Ticket = values[1],
                                    StartTime = values[2],
                                    ElapsedTime = values[5],
                                    Work = values[3],
                                    Comment = values[4],                                    
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print("CSV読み込みエラー: " + ex.Message);
            }
        }

        // 選択された行のコメントを表示
        private void LapRecordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LapRecordList.SelectedItem is LapRecord selectedRecord)
            {
                DetailDialog dialog = new DetailDialog(selectedRecord.Timestamp, selectedRecord.Ticket, selectedRecord.StartTime, selectedRecord.ElapsedTime, selectedRecord.Work, selectedRecord.Comment);

                dialog.Show();
            }
        }
    }
}
