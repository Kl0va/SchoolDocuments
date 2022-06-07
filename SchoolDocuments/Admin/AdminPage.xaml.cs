using SchoolDocuments.General;
using SchoolDocuments.Moduls;
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
    public sealed partial class AdminPage : Page
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        public AdminPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Проверка выбора пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (documents.IsSelected)
            {
                NavigateDocuments();
            }
            else if (templates.IsSelected)
            {
                NavigateTemplates();
            }
            else if (roles.IsSelected)
            {
                NavigateRoles();
            }
        }

        /// <summary>
        /// Переход на страницу документов
        /// </summary>
        private void NavigateDocuments()
        {
            myFrame.Navigate(typeof(DocumentsPage), Frame);
            pageHeader.Text = "Документы";
        }
        /// <summary>
        /// Переход на страницу шаблонов
        /// </summary>
        private void NavigateTemplates()
        {
            myFrame.Navigate(typeof(TemplatesPage), Frame);
            pageHeader.Text = "Шаблоны документов";
        }
        /// <summary>
        /// Переходл на страницу ролей
        /// </summary>
        private void NavigateRoles()
        {
            myFrame.Navigate(typeof(RolesPage), Frame);
            pageHeader.Text = "Роли сотрудников";
        }

        /// <summary>
        /// Открытие раскрывающегося окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        /// <summary>
        /// Загрузка страницы
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigateDocuments();
        }

        /// <summary>
        /// Выход
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainPage.exiting = true;
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            File.WriteAllText(storageFolder.Path + @"\auth.txt", "");
            ApiWork.Logout();
            Frame.Navigate(typeof(MainPage));
        }
    }
}
