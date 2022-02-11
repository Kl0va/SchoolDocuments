using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace SchoolDocuments.Users
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class ChangePasswordPage : Page
    {
        private static string email;
        public ChangePasswordPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                email = e.Parameter as string;
            }
        }

        private static int number;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Password.Password != "" && RePassword.Password != "" && Password.Password == RePassword.Password && Password.Password.Length >= 5 && RePassword.Password.Length >= 5)
            {
                Password.Visibility = Visibility.Collapsed;
                RePassword.Visibility = Visibility.Collapsed;
                Confirm.Visibility = Visibility.Collapsed;
                PasswordText.Visibility = Visibility.Collapsed;

                CodeText.Visibility = Visibility.Visible;
                Code.Visibility = Visibility.Visible;
                ConfirmCode.Visibility = Visibility.Visible;

                Random random = new Random();
                number = random.Next(1000,5000);

                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("SchoolDocuments@gmail.com", "Security");
                // кому отправляем
                MailAddress to = new MailAddress($"killer228322@gmail.com");
                // создаем объект сообщения
                MailMessage message = new MailMessage(from, to);
                // тема письма
                message.Subject = "Смена пароля";
                // текст письма
                message.Body = $"<h2>Введите следующий код в приложении: {number}</h2>";
                // письмо представляет код html
                message.IsBodyHtml = true;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                // логин и пароль
                smtp.Credentials = new NetworkCredential("isip_k.v.volk@mpt.ru", "V123789456k");
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
            else
            {
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Проверьте введенные данные",
                    PrimaryButtonText = "Ок"
                };
                ContentDialogResult result = await errorDialog.ShowAsync();
            }
        }

        private async void ConfirmCode_Click(object sender, RoutedEventArgs e)
        {
            if(int.Parse(Code.Text) == number)
            {
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = "Успешно",
                    Content = "Пароль успешно изменен!",
                    PrimaryButtonText = "Ок",
                    SecondaryButtonText = "Отмена"
                };
                ContentDialogResult result = await errorDialog.ShowAsync();
                if(result == ContentDialogResult.Primary)
                {
                    //Меняем пароль (API)
                }
                else if(result == ContentDialogResult.Secondary)
                {
                    PasswordText.Text = "Снова введите пароль";
                    Password.Password = "";
                    RePassword.Password = "";

                    Password.Visibility = Visibility.Visible;
                    RePassword.Visibility = Visibility.Visible;
                    Confirm.Visibility = Visibility.Visible;
                    PasswordText.Visibility = Visibility.Visible;

                    CodeText.Visibility = Visibility.Collapsed;
                    Code.Visibility = Visibility.Collapsed;
                    ConfirmCode.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Проверьте введенные данные",
                    PrimaryButtonText = "Ок"
                };
                ContentDialogResult result = await errorDialog.ShowAsync();
            }
        }
    }
}
