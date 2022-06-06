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

namespace SchoolDocuments.Users
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class FamiliarizeDocuments : Page
    {
        private static readonly List<Models.Familiarize> documents = new List<Models.Familiarize>();
        private static readonly List<Models.Familiarize> documentsSearch = new List<Models.Familiarize>();
        Frame rootFrame;
        public FamiliarizeDocuments()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            progress.Visibility = Visibility.Visible;
            documents.Clear();
            rootFrame = e.Parameter as Frame;
            Task<List<Models.Familiarize>> getDocuments = ApiWork.GetAllFamiliarize(UserInfo.Id);
            await getDocuments.ContinueWith(t =>
            {
                documents.Clear();
                if (getDocuments.Result == null)
                {
                    
                }
                else
                {
                    foreach (Models.Familiarize document in getDocuments.Result)
                    {
                        documents.Add(document);
                    }
                }
            });
            var orderedDocuments = from p in documents orderby p.familiarized select p;
            documentsGrid.ItemsSource = orderedDocuments;
            progress.Visibility = Visibility.Collapsed;
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            documentsSearch.Clear();
            foreach (Familiarize template in documents)
            {
                if (template.document.title.Contains(search.Text))
                {
                    documentsSearch.Add(template);
                }
            }
            documentsGrid.ItemsSource = null;
            documentsGrid.ItemsSource = documentsSearch;
        }

        private void documentsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rootFrame.Navigate(typeof(CheckFamiliarizeDocument), (Familiarize)documentsGrid.SelectedItem);
        }
    }
}
