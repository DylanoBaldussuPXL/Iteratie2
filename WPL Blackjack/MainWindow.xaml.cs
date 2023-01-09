using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPL_Blackjack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Random random = new Random();
        string nl = Environment.NewLine;
        public MainWindow()
        {
            InitializeComponent();
            btnHit.IsEnabled = false;
            btnStand.IsEnabled = false;
        }


        public string GeefKaart(Boolean isSpeler)
        {
            string kaart = "";
            int randomNummer = random.Next(1, 13);
            int randomNummer2 = random.Next(1, 4);

            if (randomNummer2 == 1)
            {
                kaart += "Harten ";
            }
            else if (randomNummer2 == 2)
            {
                kaart += "Ruiten ";
            }
            else if (randomNummer2 == 3)
            {
                kaart += "Klaveren ";
            }
            else if (randomNummer2 == 4)
            {
                kaart += "Schoppen ";
            }

            if (randomNummer == 1)
            {
                kaart += "Aas";
            }
            else if (randomNummer == 13)
            {
                kaart += "Koning";
            }
            else if (randomNummer == 12)
            {
                kaart += "Koningin";
            }
            else if (randomNummer == 11)
            {
                kaart += "Boer";
            }
            else if (randomNummer >= 2 && randomNummer <= 10)
            {
                kaart += Convert.ToString(randomNummer);
            }
            return kaart;
        }

        private int berekenKaartScore(string kaart)
        {
            string[] array = kaart.Split(' ');
            if (array[1] == "Koning" || array[1] == "Koningin" || array[1] == "Boer")
            {
                return 10;
            }
            else if (array[1] == "Aas")
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(array[1]);
            }
        }

        private void laatKaartZienSpeler(string kaart)
        {
            if(kaart == "Harten Aas")
            {
                Bitmap hartenAas = new Bitmap(@"C:\Users\Dylan\Desktop\Graduaat Programmeren\WPL 1\BlackJack - Iteratie 2\Assets\heart\cardHearts_A.png");
                imgSpeler
            }
        }
        private void btnDeel_Click(object sender, RoutedEventArgs e)
        {
            string spelerKaart1 = GeefKaart(true);
            string spelerKaart2 = GeefKaart(true);
            int scoreSpeler = 0;
            int scoreBank = 0;
            int inzet = Convert.ToInt32(txtInzet.Text);
            int kapitaal = Convert.ToInt32(txtKapitaal.Text);

            if (kapitaal > 0)
            {
                if (inzet <= kapitaal && inzet >= 0.1 * kapitaal)
                {
                    kapitaal -= inzet;
                    txtKapitaal.Text = Convert.ToString(kapitaal);

                    while (txtblockSpeler.Text.Contains(spelerKaart2) || txtblockBank.Text.Contains(spelerKaart2))
                    {
                        spelerKaart2 = GeefKaart(true);
                    }
                    txtblockSpeler.Text = spelerKaart1 + nl + spelerKaart2;

                    string bankKaart1 = GeefKaart(false);
                    while (txtblockSpeler.Text.Contains(bankKaart1) || txtblockBank.Text.Contains(bankKaart1))
                    {
                        bankKaart1 = GeefKaart(false);
                    }
                    txtblockBank.Text = bankKaart1;

                    scoreSpeler += berekenKaartScore(spelerKaart1);
                    scoreSpeler += berekenKaartScore(spelerKaart2);

                    scoreBank += berekenKaartScore(bankKaart1);

                    lblScoreSpeler.Content = scoreSpeler;
                    lblScoreBank.Content = scoreBank;

                    btnDeel.IsEnabled = false;
                    btnHit.IsEnabled = true;
                    btnStand.IsEnabled = true;
                    lblResultaat.Content = "";
                    txtInzet.IsReadOnly = true;
                }
                else
                {
                    MessageBox.Show("Inzet is niet juist. Kijk weker na of het tenminste 10% is van het kapitaal en niet hoger is dan het kapitaal zelf.");
                }
            }
            else
            {
                MessageBox.Show("Je hebt geen geld meer, start een nieuw spel.");
            }

        }

        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            string nl = Environment.NewLine;
            string extraSpelerKaart = GeefKaart(true);
            int scoreSpeler = Convert.ToInt32(lblScoreSpeler.Content);

            while (txtblockSpeler.Text.Contains(extraSpelerKaart) || txtblockBank.Text.Contains(extraSpelerKaart))
            {
                extraSpelerKaart = GeefKaart(true);
            }

            scoreSpeler += berekenKaartScore(extraSpelerKaart);

            txtblockSpeler.Text = txtblockSpeler.Text + nl + extraSpelerKaart;
            lblScoreSpeler.Content = scoreSpeler;

            if (Convert.ToInt32(lblScoreSpeler.Content) > 21)
            {
                btnHit.IsEnabled = false;
                btnStand.IsEnabled = false;
                lblResultaat.Content = "Bust";
                lblResultaat.Foreground = Brushes.Red;
                btnDeel.IsEnabled = true;
                txtInzet.IsReadOnly = false;
            }
        }

        private void btnStand_Click(object sender, RoutedEventArgs e)
        {
            btnHit.IsEnabled = false;
            btnStand.IsEnabled = false;
            txtInzet.IsReadOnly = false;
            int inzet = Convert.ToInt32(txtInzet.Text);
            int kapitaal = Convert.ToInt32(txtKapitaal.Text);
            int scoreBank = Convert.ToInt32(lblScoreBank.Content);
            string[] extraKaartenBankArray = { };

            while (scoreBank < 17)
            {
                string extraKaartBank = GeefKaart(false);
                scoreBank += berekenKaartScore(extraKaartBank);
                txtblockBank.Text = txtblockBank.Text + nl + extraKaartBank;
            }
            lblScoreBank.Content = scoreBank;
            if (scoreBank > 21 || scoreBank < Convert.ToInt32(lblScoreSpeler.Content))
            {
                lblResultaat.Content = "Gewonnen";
                lblResultaat.Foreground = Brushes.Green;
                btnDeel.IsEnabled = true;
                txtInzet.IsReadOnly = false;
                kapitaal += 2 * inzet;
            }
            else if(scoreBank < 21 && scoreBank > Convert.ToInt32(lblScoreSpeler.Content))
            {
                lblResultaat.Content = "Bust";
                lblResultaat.Foreground= Brushes.Red;
                btnDeel.IsEnabled= true;
                txtInzet.IsReadOnly = false;
            }
            else
            {
                lblResultaat.Content = "Push";
                lblResultaat.Foreground = Brushes.Black;
                btnDeel.IsEnabled=true;
                txtInzet.IsReadOnly = false;
                kapitaal += inzet;
            }
            txtKapitaal.Text = Convert.ToString(kapitaal);
        }

        private void btnNieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
