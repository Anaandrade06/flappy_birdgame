﻿

namespace flappy_birdgame;
public partial class MainPage : ContentPage
{
	const int gravidade = 8;

	const int TimeToFrame = 80;

	bool estaMorto = true;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 20;
	const int maxTempoPulando = 3;
	int tempoPulando = 0;
	bool estaPulando = false;
	const int forcaPulo = 40;
	const int aberturaMin = 50;
	int score = 0;


	public MainPage()
	{
		InitializeComponent();
	}
	async void AplicaGravidade()
	{
		pardal.TranslationY += gravidade;

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

	}
	void GerenciaCanos()
	{
		Canocima.TranslationX -= velocidade;
		Canobaixo.TranslationX -= velocidade;
		if (Canobaixo.TranslationX <= -larguraJanela)
		{
			Canobaixo.TranslationX = 0;
			Canocima.TranslationX = 0;
			var alturaMax = -100;
			var alturaMin = -Canobaixo.HeightRequest;
			Canocima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			Canobaixo.TranslationY = Canocima.TranslationY + alturaMin + aberturaMin + Canobaixo.HeightRequest;
			score++;
			LabelScore.Text = "Canos:" + score.ToString("D3");
			if (score %2==0)
			velocidade ++;
		}


	}
	private void OnGameOverClicked(object sender, EventArgs a)
	{
		FrameGameOver.IsVisible = false;
		Inicializar();
		Desenhar();
	}

	void Inicializar()
	{
		estaMorto = false;
		pardal.TranslationY = 0;
	}

	bool VerificaColisao()
	{
		if (!estaMorto)
		{
			if (VerificaColisaoTeto() ||
			 VerificaColisaoChao() ||
			 VerificaColisaoCanoCima()||
			 VerificaColisaoCanobaixo());
			{
				return true;
			}
		}
		return false;
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
		var maxY = alturaJanela / 2 - Chao.HeightRequest;
		if (pardal.TranslationY >= maxY)
			return true;
		else
			return false;
	}
	void AplicaPulo()
	{
		pardal.TranslationY -= forcaPulo;
		tempoPulando++;
		if (tempoPulando >= maxTempoPulando)
		{
			estaPulando = false;
			tempoPulando = 0;
		}
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
	var posHpardal = (larguraJanela /2) - (pardal.WidthRequest/2);
    var posVpardal = (alturaJanela /2) + (pardal.HeightRequest/2)+ pardal.TranslationY;
	var yMaxCano = Canocima.HeightRequest + Canocima.TranslationY + aberturaMin;
	if (posHpardal >= Math.Abs(Canobaixo.TranslationX) - Canobaixo.WidthRequest &&
	posHpardal <= Math.Abs (Canobaixo.TranslationX) + Canobaixo.HeightRequest &&
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