using SchoolDocuments.Models;
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
        Frame rootFrame;
        public TemplatesPage()
        {
            this.InitializeComponent();
        }

        private void createTemplateBtn_Click(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(CreateTemplatePage));
        }

        private void TemplatesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rootFrame.Navigate(typeof(CreateTemplatePage), (Template)TemplatesGrid.SelectedItem);
        }

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
    }
}
