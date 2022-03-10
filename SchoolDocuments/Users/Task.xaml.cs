﻿using SchoolDocuments.Moduls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace SchoolDocuments.Users
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Task : Page
    {
        Frame rootFrame;
        List<Models.Task> tasks = new List<Models.Task>();
        public Task()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            progress.Visibility = Visibility.Visible;
            tasks.Clear();
            rootFrame = e.Parameter as Frame;
            Task<List<Models.Task>> getTasks = ApiWork.GetGivenTasks(UserInfo.Id);
            await getTasks.ContinueWith(t =>
            {
                tasks.Clear();
                foreach (Models.Task task in getTasks.Result)
                {
                    tasks.Add(task);
                }
            });
            var orderedDocuments = from p in tasks orderby p.created select p;
            documentsGrid.ItemsSource = orderedDocuments;
            progress.Visibility = Visibility.Collapsed;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(CreateTask));
        }
    }
}