namespace TesteRec.Layouts.Iniciais.RecuperacaoSenha;

public partial class InsiraEmail : ContentPage
{
	public InsiraEmail()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void Button_Email_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new AtualizaSenha());
    }
}