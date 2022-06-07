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
        private static readonly List<Models.Document> documentsforAdd = new List<Models.Document>();
        private static readonly List<Models.Document> documentsSearch = new List<Models.Document>();
        Frame rootFrame;
        /// <summary>
        /// Инициализация
        /// </summary>
        public DocumentsPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Переход на страницу добавления документа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addDocument_Click(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(CreateDocument));
        }
        /// <summary>
        /// Подгрузка данных
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            progress.Visibility = Visibility.Visible;
            documents.Clear();
            documentsforAdd.Clear();
            rootFrame = e.Parameter as Frame;
            Task<List<Models.Document>> getDocuments = ApiWork.GetUserDocuments(UserInfo.Id);
            await getDocuments.ContinueWith(t =>
            {
                documents.Clear();
                foreach (Models.Document document in getDocuments.Result)
                {
                    documents.Add(document);
                }
            });
            foreach(Document document1 in documents)
            {
                if(document1.author.id == UserInfo.Id)
                {
                    documentsforAdd.Add(document1);
                }
            }
            documentsGrid.ItemsSource = documentsforAdd;
            progress.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Поиск документов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            documentsSearch.Clear();           
            if (search.Text == "")
            {
                documentsGrid.ItemsSource = documentsforAdd;
            }
            else
            {
                foreach (Document document in documentsforAdd)
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
        /// <summary>
        /// Выбор документа из списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void documentsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ext = "";
            Document document1 = (Document)documentsGrid.SelectedItem;
            if (document1 != null)
            {
                Task<Models.File> file = ApiWork.GetUserDocumentsFile(document1.id);
                await file.ContinueWith(file1 =>
                {
                    ext = file1.Result.extension;
                });
                if (ext != "rtf")
                {
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = "Ошибка",
                        Content = "Данный файл не поддерживается",
                        PrimaryButtonText = "Ок"
                    };
                    ContentDialogResult result = await errorDialog.ShowAsync();
                }
                else
                {
                    rootFrame.Navigate(typeof(CreateDocument), (Document)documentsGrid.SelectedItem);
                }
            }
        }
    }
}
