﻿using SchoolDocuments.Moduls;
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

namespace SchoolDocuments.Users
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Task : Page
    {
        Frame rootFrame;
        List<Models.Task> tasks = new List<Models.Task>();
        List<Models.Task> tasksSearch = new List<Models.Task>();
        List<Models.Task> tasksChange = new List<Models.Task>();

        /// <summary>
        /// Инициализация
        /// </summary>
        public Task()
        {
            this.InitializeComponent();
            givenTasks.IsChecked = true;
        }
        /// <summary>
        /// Подгрузка данных
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootFrame = e.Parameter as Frame;
        }
        /// <summary>
        /// Переход на страницу добавления задания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_Click(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(CreateTask));
        }

        /// <summary>
        /// Выбор документа из списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void documentsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rootFrame.Navigate(typeof(TasksDetail), (Models.Task)documentsGrid.SelectedItem);
        }

        /// <summary>
        /// Переключение на выданные задания пользователем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void givenTasks_Checked(object sender, RoutedEventArgs e)
        {
            progress.Visibility = Visibility.Visible;
            tasks.Clear();
            Task<List<Models.Task>> getTasks = ApiWork.GetGivenTasks(UserInfo.Id);
            await getTasks.ContinueWith(t =>
            {
                tasks.Clear();
                if (getTasks.Result != null)
                {
                    foreach (Models.Task task in getTasks.Result)
                    {
                        tasks.Add(task);
                    }
                }
            });
            var orderedDocuments = from p in tasks orderby p.deadline select p;
            documentsGrid.ItemsSource = orderedDocuments;
            progress.Visibility = Visibility.Collapsed;
            var sort = tasks.All(x => x.performs.All(p => p.status == Models.PerformerStatus.Completed));
        }

        /// <summary>
        /// Переключение на выданные задания пользователю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void toDoTasks_Checked(object sender, RoutedEventArgs e)
        {
            progress.Visibility = Visibility.Visible;
            tasks.Clear();
            Task<List<Models.Task>> getTasks = ApiWork.GetToDoTasks(UserInfo.Id);
            await getTasks.ContinueWith(t =>
            {
                tasks.Clear();
                foreach (Models.Task task in getTasks.Result)
                {
                    tasks.Add(task);
                }
            });
            var orderedDocuments = from p in tasks orderby p.deadline select p;
            documentsGrid.ItemsSource = orderedDocuments;
            progress.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Переключение на выполненные задания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void completedTasks_Checked(object sender, RoutedEventArgs e)
        {
            progress.Visibility = Visibility.Visible;
            tasks.Clear();
            Task<List<Models.Task>> getTasks = ApiWork.GetGivenTasks(UserInfo.Id);
            await getTasks.ContinueWith(t =>
            {
                tasks.Clear();
                foreach (Models.Task task in getTasks.Result)
                {
                    tasks.Add(task);
                }
            });
            List<Models.Task> sort = new List<Models.Task>();
            sort = tasks.Where(x => x.performs.All(y => y.status == Models.PerformerStatus.Completed)).ToList();
            var orderedDocuments = from p in sort orderby p.deadline select p;
            documentsGrid.ItemsSource = orderedDocuments;
            progress.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Поиск заданий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            tasksSearch.Clear();
            foreach (Models.Task template in tasks)
            {
                if (template.title.ToLower().Contains(search.Text.ToLower()))
                {
                    tasksSearch.Add(template);
                }
            }
            documentsGrid.ItemsSource = null;
            documentsGrid.ItemsSource = tasksSearch;
        }
    }
}
