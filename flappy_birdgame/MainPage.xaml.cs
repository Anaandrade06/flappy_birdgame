

namespace flappy_birdgame;
public partial class MainPage : ContentPage
{
	const int gravity = 10;

	const int TimeToFrame = 2000;

	bool estaMorto = true;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 20;


	public MainPage()
	{
		InitializeComponent();
	}

	async Task Drawn()
	{
		while (!estaMorto)


		{
			IntroGravity();
			await Task.Delay(TimeToFrame);
		}
	}
	async void IntroGravity()
	{
		pardal.TranslationY += gravity;

	}

	void OnGameOverClicked(object sender, EventArgs a)
	{
		FrameGameOver.IsVisible = false;
		Inicializar();
		Desenhar();
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);
		larguraJanela = width;
	}
	void GerenciadorCanos()
	{
		Canocima.TranslationX -= velocidade;
		Canobaixo.TranslationX -= velocidade;
		if (Canobaixo.TranslationX <= -larguraJanela)
		{
			Canobaixo.TranslationX = 0;
			Canocima.TranslationX = 0;
		}
		
	}

}