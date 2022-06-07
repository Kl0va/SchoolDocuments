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

namespace SchoolDocuments.Users
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class UsersPage : Page
    {
        private static string email;
        /// <summary>
        /// Инициализация
        /// </summary>
        public UsersPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Подгрузка данных
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                email = e.Parameter as string;
            }
            NavigateDocuments();
        }

        /// <summary>
        /// Открытие всплывающего окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        /// <summary>
        /// Проверка выбора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (documents.IsSelected)
            {
                NavigateDocuments();
            }
            else if (Agreed.IsSelected)
            {
                NavigateAgreement();
            }
            else if (document.IsSelected)
            {
                NavigateDoc();
            }
            else if (task.IsSelected)
            {
                NavigateTask();
            }
        }
        /// <summary>
        /// Переход на страницу ознакомления
        /// </summary>
        public void NavigateDocuments()
        {
            myFrame.Navigate(typeof(FamiliarizeDocuments), Frame);
            pageHeader.Text = "На ознакомление";
        }
        /// <summary>
        /// Переход на страницу согласований/подписаний документов
        /// </summary>
        public void NavigateAgreement()
        {
            myFrame.Navigate(typeof(AgreeDocuments), Frame);
            pageHeader.Text = "Согласование/подписание";
        }
        /// <summary>
        /// Переход на страницу документов
        /// </summary>
        public void NavigateDoc()
        {
            myFrame.Navigate(typeof(DocumentsPage), Frame);
            pageHeader.Text = "Документы";
        }
        /// <summary>
        /// Переход на страницу заданий
        /// </summary>
        public void NavigateTask()
        {
            myFrame.Navigate(typeof(Task), Frame);
            pageHeader.Text = "Задания";
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
            File.WriteAllText(storageFolder.Path + @"\auth.txt","");
            ApiWork.Logout();
            Frame.Navigate(typeof(MainPage));
        }
    }
}
