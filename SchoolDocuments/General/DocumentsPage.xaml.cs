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

namespace SchoolDocuments.General
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class DocumentsPage : Page
    {
        private static readonly List<Models.Document> documents = new List<Models.Document>();
        private static readonly List<Models.Document> documentsSearch = new List<Models.Document>();
        Frame rootFrame;
        public DocumentsPage()
        {
            this.InitializeComponent();
        }

        private void addDocument_Click(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(CreateDocument));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            progress.Visibility = Visibility.Visible;
            documents.Clear();
            rootFrame = e.Parameter as Frame;
            Task<List<Models.Document>> getDocuments = ApiWork.GetAllDocuments();
            await getDocuments.ContinueWith(t =>
            {
                documents.Clear();
                foreach (Models.Document document in getDocuments.Result)
                {
                    documents.Add(document);
                }
            });

            documentsGrid.ItemsSource = documents;
            progress.Visibility = Visibility.Collapsed;
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            documentsSearch.Clear();
            foreach (Document document in documents)
            {
                if (document.title.Contains(search.Text))
                {
                    documentsSearch.Add(document);
                }
            }
            documentsGrid.ItemsSource = null;
            documentsGrid.ItemsSource = documentsSearch;
        }
    }
}
