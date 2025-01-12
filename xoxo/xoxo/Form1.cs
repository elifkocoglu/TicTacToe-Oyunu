using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xoxo
{



    public partial class Form1 : Form
    {

        private int boardSize = 3;         // Varsayılan oyun tahtası boyutu (3x3)
        private Button[,] buttons;         // Oyun tahtasındaki butonlar
        private bool isXTurn = true;       // İlk hamlede X başlar
        private int movesCount = 0;        // Yapılan hamle sayısı

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            // Seçilen oyun boyutunu al
            string selectedSize = comboBoxSize.SelectedItem.ToString();
            boardSize = int.Parse(selectedSize[0].ToString());  // Örneğin "3x3" ise 3'ü al

            // Eski tahtayı temizle (varsa)
            if (buttons != null)
            {
                foreach (Button btn in buttons)
                    this.Controls.Remove(btn);  // Önceki butonları sil
            }

            // Hamle sayısını sıfırla ve X ile başlat
            movesCount = 0;
            isXTurn = true;

            // Yeni boyutta buton matrisi oluştur
            buttons = new Button[boardSize, boardSize];
            CreateBoard();  // Tahtayı oluştur
        }

        private void CreateBoard()
        {
            int buttonSize = 50;  // Her hücrenin boyutu
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Button button = new Button();
                    button.Size = new Size(buttonSize, buttonSize);
                    button.Location = new Point(75 + j * buttonSize, 10 + i * buttonSize); // Konumu ayarla
                    button.Click += Button_Click;  // Buton tıklama olayını ekle
                    button.Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);  // Yazı tipini ayarla
                    buttons[i, j] = button;  // Butonu matrise ekle
                    this.Controls.Add(button);  // Form üzerine butonu ekle
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            // Eğer buton boşsa X veya O yerleştir
            if (clickedButton.Text == "")
            {
                clickedButton.Text = isXTurn ? "X" : "O";  // Sırasına göre X veya O yerleştir
                isXTurn = !isXTurn;  // Sıra diğer oyuncuya geçsin
                movesCount++;  // Hamle sayısını arttır

                // Kazanan olup olmadığını kontrol et
                if (CheckWinner())
                {
                    DisplayWinner(clickedButton.Text);  // Kazanan oyuncuyu göster
                }
                // Eğer tahta dolarsa ve kazanan yoksa beraberlik mesajı göster
                else if (movesCount == boardSize * boardSize)
                {
                    MessageBox.Show("Berabere!", "Oyun Bitti");
                    PromptNewGame();  // Yeni oyun seçeneği sun
                }
            }
        }

        private bool CheckWinner()
        {
            // Satır ve sütunları kontrol et
            for (int i = 0; i < boardSize; i++)
            {
                if (IsWinningLine(i, 0, 0, 1) || IsWinningLine(0, i, 1, 0))
                {
                    return true;  // Kazanan var
                }
            }

            // Çaprazları kontrol et
            if (IsWinningLine(0, 0, 1, 1) || IsWinningLine(0, boardSize - 1, 1, -1))
            {
                return true;  // Kazanan var
            }

            return false;  // Kazanan yok
        }

        private bool IsWinningLine(int startX, int startY, int dx, int dy)
        {
            string first = buttons[startX, startY].Text;
            if (first == "") return false;  // İlk hücre boşsa kazanılmadı

            for (int i = 1; i < boardSize; i++)
            {
                if (buttons[startX + i * dx, startY + i * dy].Text != first)
                    return false;  // Farklı bir değer varsa kazanılmadı
            }
            return true;  // Aynı değer varsa kazandı
        }

        private void DisplayWinner(string winner)
        {
            MessageBox.Show($"{winner} kazandı!", "Oyun Bitti");
            PromptNewGame();  // Yeni oyun başlatma seçeneği sun
        }

        private void PromptNewGame()
        {
            var result = MessageBox.Show("Yeni bir oyun oynamak ister misiniz?", "Oyun Bitti", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                startButton_Click(null, null);  // Yeni oyun başlat
            }
            else
            {
                // Kullanıcı yeni oyun istemiyorsa tüm butonları devre dışı bırak
                foreach (Button btn in buttons)
                    btn.Enabled = false;
                                
            }
        }
    }
}
