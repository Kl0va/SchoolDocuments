using SchoolDocuments.Models;
using SchoolDocuments.Moduls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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
    public sealed partial class RolesPage : Page
    {
        Frame rootFrame;
        private static List<User> users = new List<User>();
        private static List<User> usersSearch = new List<User>();
        /// <summary>
        /// Инициализация
        /// </summary>
        public RolesPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Подгрузка данных
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            progress.Visibility = Visibility.Visible;
            users.Clear();
            rootFrame = e.Parameter as Frame;
            Task<List<User>> getTemplates = ApiWork.GetAllUsers();
            await getTemplates.ContinueWith(t =>
            {
                users.Clear();
                foreach (User template in getTemplates.Result)
                {
                    users.Add(template);
                }
            });
            TemplatesGrid.ItemsSource = users;
            progress.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Поиск по пользователям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            usersSearch.Clear();
            foreach (User template in users)
            {
                if (template.email.ToLower().Contains(search.Text.ToLower()))
                {
                    usersSearch.Add(template);
                }
            }
            TemplatesGrid.ItemsSource = null;
            TemplatesGrid.ItemsSource = usersSearch;
        }
        /// <summary>
        /// Выбор пользователя из списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TemplatesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = (User)TemplatesGrid.SelectedItem;
            Role role = new Role("");
            if (user.email != "pr.pr.gr-1@ok654.ru")
            {
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = "Выбор",
                    Content = "Выберите роль для пользователя",
                    PrimaryButtonText = "Администратор",
                    SecondaryButtonText = "Сотрудник",
                    CloseButtonText = "Отмена"
                };
                ContentDialogResult result = await errorDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    user.role = "Admin";
                    role.name = "Admin";
                    ApiWork.SaveRole(user,role);
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    user.role = "Employee";
                    role.name = "Employee";
                    ApiWork.SaveRole(user,role);
                }
                Thread.Sleep(500);
                Frame.Navigate(this.GetType());
            }
            else
            {
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Роль этого сотрудника не может быть изменена",
                    PrimaryButtonText = "Ok"
                };
                ContentDialogResult result = await errorDialog.ShowAsync();
            }
        }
    }
}
