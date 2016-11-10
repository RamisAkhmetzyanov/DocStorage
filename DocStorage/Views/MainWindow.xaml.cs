using DocStorage.Service;
using System.Windows;

namespace DocStorage.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SignInBtnOnClick(object sender, RoutedEventArgs e)
        {
            string login = LoginTxt.Text;
            string password = PasswordTxt.Password;

            if (login.Length < 4)
            {
                MessageBox.Show("Логин должен содержать 4 и более символа");
                return;
            }
            if (password.Length < 4)
            {
                MessageBox.Show("Пароль должно содержать 4 и более символа");
                return;
            }
            string responseMessage;
            bool authenticated = Authentication.Authenticate(login, password, out responseMessage);
            if (!authenticated)
                MessageBox.Show(responseMessage);
            else
            {
                Document DocWindow = new Document();
                this.Visibility = Visibility.Hidden;
                DocWindow.ShowDialog();
                this.Visibility = Visibility.Visible;
                Authentication.SignOut();
            }
        }

        private void SignUpBtnOnClick(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            this.Visibility = Visibility.Hidden;
            signUpWindow.ShowDialog();
            this.Visibility = Visibility.Visible;

        }
    }
}
