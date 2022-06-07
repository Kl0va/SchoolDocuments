﻿using SchoolDocuments.Models;
using SchoolDocuments.Moduls;
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

namespace SchoolDocuments.Admin
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TemplatesPage : Page
    {
        private static readonly List<Template> templates = new List<Template>();
        private static readonly List<Template> templatesSearch = new List<Template>();
        Frame rootFrame;
        /// <summary>
        /// Инициализация
        /// </summary>
        public TemplatesPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Переход на страницу создания шаблона
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createTemplateBtn_Click(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(CreateTemplatePage));
        }
        /// <summary>
        /// Выбор шаблона из списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemplatesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rootFrame.Navigate(typeof(CreateTemplatePage), (Template)TemplatesGrid.SelectedItem);
        }
        /// <summary>
        /// Подгрузка данных
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            progress.Visibility = Visibility.Visible;
            templates.Clear();
            rootFrame = e.Parameter as Frame;
            Task<List<Template>> getTemplates = ApiWork.GetAllTemplates();
            await getTemplates.ContinueWith(t =>
            {
                templates.Clear();
                foreach (Template template in getTemplates.Result)
                {
                    templates.Add(template);
                }
            });
            TemplatesGrid.ItemsSource = templates;
            progress.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Поиск шаблонов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            templatesSearch.Clear();
            foreach(Template template in templates)
            {
                if (template.name.Contains(search.Text))
                {
                    templatesSearch.Add(template);
                }
            }
            TemplatesGrid.ItemsSource = null;
            TemplatesGrid.ItemsSource = templatesSearch;
        }
    }
}
