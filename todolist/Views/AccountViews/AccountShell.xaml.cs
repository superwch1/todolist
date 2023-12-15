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
		Routing.RegisterRoute("policy", typeof(PolicyView));

		InitializeComponent();

		login.Content = new LoginView();
    }
}

