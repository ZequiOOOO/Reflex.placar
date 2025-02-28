using System.Diagnostics;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        List<Color> colorList = new List<Color>() { Color.Blue, Color.Red, Color.Green, Color.Yellow, Color.Gray };
        Random rand = new Random();

        //variáveis
        private Button btnIniciar;
        private Button btnAlvo;
        private Label label1;

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timerTrocaCor;

        //timer
        private Random random;

        //stopWatch
        private Stopwatch stopwatch;

        List<double> ultimasPontuacoes = new List<double>();

        // Variável para controlar o tamanho do botão
        private int tamanhoInicialBotao = 50;
        private int tamanhoMinimoBotao = 20;

        public Form1()
        {
            InitializeComponent();

            //determina o titulo da tela 
            this.Text = "Reflexo";
            this.Size = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            btnIniciar = new Button()
            {
                Text = "Iniciar",
                Size = new Size(100, 50)
            };
            btnIniciar.Click += IniciarJogo;
            this.Controls.Add(btnIniciar);

            //btnalvo
            btnAlvo = new Button()
            {
                Size = new Size(tamanhoInicialBotao, tamanhoInicialBotao),
                BackColor = colorList[rand.Next(0, colorList.Count - 1)],
                Visible = false,
            };
            btnAlvo.Click += btnAlvoClick;
            this.Controls.Add(btnAlvo);

            //label1
            label1 = new Label();
            label1.Location = new Point(50, 50);
            label1.Size = new Size(60, 120);
            label1.Text = "";
            label1.Visible = true;

            this.Controls.Add(label1);

            //timer
            timer = new System.Windows.Forms.Timer();
            timer.Tick += MostrarBotaoAlvo;

            timerTrocaCor = new System.Windows.Forms.Timer();
            timerTrocaCor.Interval = 5000;
            timerTrocaCor.Tick += MostrarBotaoAlvo;

            random = new Random();
            stopwatch = new Stopwatch();
        }

        //iniciar o jogo
        private void IniciarJogo(object sender, EventArgs e)
        {
            //desabilita o botão
            btnIniciar.Enabled = false;
            IniciarNovaRodada();
        }

        private void IniciarNovaRodada()
        {
            //determina um timer aleatorio entre 1 e 3 segundos
            timer.Interval = random.Next(1000, 3000);
            timerTrocaCor.Start();
            timer.Start();
            stopwatch.Restart();
        }

        private void MostrarBotaoAlvo(object sender, EventArgs e)
        {
            timer.Stop();
            int x = random.Next(50, this.ClientSize.Width - 70);
            int y = random.Next(50, this.ClientSize.Height - 70);
            btnAlvo.BackColor = colorList[rand.Next(0, colorList.Count - 1)];
            btnAlvo.Location = new Point(x, y);
            btnAlvo.Visible = true;
            stopwatch.Restart();
        }

        // Ao clicar no botão alvo
        private void btnAlvoClick(object sender, EventArgs e)
        {
            stopwatch.Stop();
            btnAlvo.Visible = false;
            timerTrocaCor.Stop();

            // Verifica se a cor do botão é azul
            if (btnAlvo.BackColor == Color.Blue)
            {
                // Adiciona o tempo de reação nas últimas pontuações
                ultimasPontuacoes.Add(stopwatch.ElapsedMilliseconds);

                // Se já tem mais de 5 pontuações armazenadas
                if (ultimasPontuacoes.Count() > 5)
                {
                    // Remove a pontuação mais antiga
                    ultimasPontuacoes.RemoveAt(0);
                }

                string pontuacoes = "";

                foreach (double pontuacao in ultimasPontuacoes)
                {
                    pontuacoes += $"{pontuacao} ms \n";
                }

                label1.Text = pontuacoes;

                MessageBox.Show($"Tempo de reação: {stopwatch.ElapsedMilliseconds}ms", "Você acertouuuu!");

                // Diminui o tamanho do botão após o acerto
                DiminuirTamanhoBotao();

                // Inicia uma nova rodada após um pequeno delay
                Task.Delay(500).ContinueWith(t => IniciarNovaRodada(), TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                MessageBox.Show("Botão cor errada, seu burro");

                // Inicia uma nova rodada após um pequeno delay
                Task.Delay(500).ContinueWith(t => IniciarNovaRodada(), TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void DiminuirTamanhoBotao()
        {
            // Verifica se o tamanho do botão não atingiu o limite mínimo
            if (btnAlvo.Width > tamanhoMinimoBotao && btnAlvo.Height > tamanhoMinimoBotao)
            {
                int novoTamanho = btnAlvo.Width - 5;  // Diminuir 5 unidades a cada acerto
                btnAlvo.Size = new Size(novoTamanho, novoTamanho);
            }
        }
    }
}