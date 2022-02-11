using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace SchoolDocuments.Admin
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CreateUserPage : Page
    {
        public CreateUserPage()
        {
            this.InitializeComponent();
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            if(Email.Text != "" && Email.Text.Length >=7 && Password.Password != "" && Password.Password.Length >= 3 && FirstName.Text != "" && SecondName.Text != "")
            {
                //Создание пользователя (API)
            }
        }
    }
}
