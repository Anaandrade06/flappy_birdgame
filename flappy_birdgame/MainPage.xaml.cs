

namespace flappy_birdgame;
public partial class MainPage : ContentPage
{
	int velocidade = 20;
	const int gravidade = 8;
	const int forcaPulo = 40;
	const int maxTempoPulando = 3;
	int score = 0;
	const int aberturaMin = 200;
	const int TimeToFrame = 80;
	double alturaJanela = 0;
	double larguraJanela = 0;
	bool estaMorto = true;
	bool estaPulando = false;
	int tempoPulando = 0;


	public MainPage()
	{
		InitializeComponent();
	}
	void Inicializar()
	{
		Canobaixo.TranslationX = -larguraJanela;
		Canocima.TranslationX = -larguraJanela;
		pardal.TranslationX = 0;
		pardal.TranslationX = 0;
		score = 0;

		GerenciaCanos();
	}
	async Task Desenhar()
	{
		while (!estaMorto)
		{
			if (estaPulando)
				AplicaPulo();
			else
				AplicaGravidade();

			GerenciaCanos();

			if (VerificaColisao())
			{
				estaMorto = true;
				labelGameOverScore.Text = "Você passou por \n" + score + "canos";
				FrameGameOver.IsVisible = true;
				break;
			}
			await Task.Delay(TimeToFrame);
		}
	}
	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);
		larguraJanela = width;
		alturaJanela = height;
		if (height > 0)
		{
			Canocima.HeightRequest = height - Chao.HeightRequest;
			Canobaixo.HeightRequest = height - Chao.HeightRequest;
		}

	}
	void GerenciaCanos()
	{
		Canocima.TranslationX -= velocidade;
		Canobaixo.TranslationX -= velocidade;
		if (Canobaixo.TranslationX <= -larguraJanela)
		{
			Canobaixo.TranslationX = 0;
			Canocima.TranslationX = 0;

			var alturaMax = -(Canobaixo.HeightRequest * 0.1);
			var alturaMin = -(Canobaixo.HeightRequest * 0.8);

			Canocima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			Canobaixo.TranslationY = Canocima.HeightRequest + Canocima.TranslationY + aberturaMin;

			score++;
			LabelScore.Text = "Score:" + score.ToString("D5");
			if (score % 4 == 0)
				velocidade++;
		}
	}
	void AplicaGravidade()
	{
		pardal.TranslationY += gravidade;

	}
	void AplicaPulo()
	{
		pardal.TranslationY -= forcaPulo;
		tempoPulando++;
		if (tempoPulando >= maxTempoPulando)
		{
			tempoPulando = 0;
			estaPulando = false;
		}
	}
	bool VerificaColisao()
	{
		{
			if (VerificaColisaoTeto() ||
			 VerificaColisaoChao() ||
			 VerificaColisaoCanoCima() ||
			 VerificaColisaoCanobaixo()) ;
			{
				return true;
			}
		}
		return false;
	}
	private void OnGameOverClicked(object sender, EventArgs a)
	{
		FrameGameOver.IsVisible = false;
		Inicializar();
		Desenhar();
	}

	bool VerificaColisaoTeto()
	{
		var minY = -alturaJanela / 2;
		if (pardal.TranslationY <= minY)
			return true;
		else
			return false;
	}

	
	bool VerificaColisaoChao()
	{
		var maxY = alturaJanela / 2;
		if (pardal.TranslationY >= maxY - Chao.HeightRequest)
			return true;
		else
			return false;
	}

	void OnGridClicked(object sender, EventArgs a)
	{
		estaPulando = true;
	}
	private bool VerificaColisaoCanoCima()
	{
		var posHpardal = (larguraJanela / 2) - (pardal.WidthRequest / 2);
		var posVpardal = (alturaJanela / 2) - (pardal.HeightRequest / 2) + pardal.TranslationY;
		if (posHpardal >= Math.Abs(Canocima.TranslationX) - Canocima.WidthRequest &&
		posHpardal <= Math.Abs(Canocima.TranslationX) + Canocima.WidthRequest &&
		posVpardal <= Canocima.HeightRequest + Canocima.TranslationY)
		{
			return true;
		}
		else
		{
			return false;
		}

	}
	bool VerificaColisaoCanobaixo()
	{
		var posHpardal = (larguraJanela / 2) - (pardal.WidthRequest / 2);
		var posVpardal = (alturaJanela / 2) + (pardal.HeightRequest / 2) + pardal.TranslationY;
		var yMaxCano = Canocima.HeightRequest + Canocima.TranslationY + aberturaMin;
		if (posHpardal >= Math.Abs(Canobaixo.TranslationX) - Canobaixo.WidthRequest &&
		posHpardal <= Math.Abs(Canobaixo.TranslationX) + Canobaixo.HeightRequest &&
		posVpardal >= yMaxCano)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}