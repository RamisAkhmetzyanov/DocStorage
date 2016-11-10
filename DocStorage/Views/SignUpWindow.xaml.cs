using DocStorage.Extensions;
using System.Windows;

namespace DocStorage.Views
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void SignUpBtnOnClick(object sender, RoutedEventArgs e)
        {
            string login = LoginTxt.Text;
            string name = NameTxt.Text;
            string password = PasswordTxt.Password;

            if (login.Length < 4)
            {
                MessageBox.Show("Логин должен содержать 4 и более символа");
                return;
            }
            if (name.Length < 4)
            {
                MessageBox.Show("Имя должно содержать 4 и более символа");
                return;
            }
            if (password.Length < 4)
            {
                MessageBox.Show("Пароль должно содержать 4 и более символа");
                return;
            }
            string responseMessage;
            ExtensionFactory.UserContext.CreateUser(login, name, password, out responseMessage);
            MessageBox.Show(responseMessage);
        }
    }
}