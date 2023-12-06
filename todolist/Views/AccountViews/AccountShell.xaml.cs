namespace todolist.Views.AccountViews;

public partial class AccountShell : Shell
{
	public AccountShell()
	{
		Routing.RegisterRoute("forgetpassword", typeof(ForgetPasswordView));
		Routing.RegisterRoute("login", typeof(LoginView));
		Routing.RegisterRoute("registeraccount", typeof(RegisterAccountView));
		Routing.RegisterRoute("resetpassword", typeof(ResetPasswordView));
		Routing.RegisterRoute("verifypasscode", typeof(VerifyPasscodeView));

		InitializeComponent();

		login.Content = new LoginView();
    }
}

