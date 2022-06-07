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
    public sealed partial class AgreeDocuments : Page
    {
        Frame rootFrame;
        private static readonly List<Models.Agreement> documents = new List<Models.Agreement>();
        private static readonly List<Models.Agreement> documentsSearch = new List<Models.Agreement>();

        /// <summary>
        /// Инициализация
        /// </summary>
        public AgreeDocuments()
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
            documents.Clear();
            rootFrame = e.Parameter as Frame;
            Task<List<Models.Agreement>> getDocuments = ApiWork.GetAllAgreements(UserInfo.Id);
            await getDocuments.ContinueWith(t =>
            {
                documents.Clear();
                foreach (Models.Agreement document in getDocuments.Result)
                {
                    documents.Add(document);
                }
            });
            var orderedDocuments = from p in documents orderby p.status select p;
            documentsGrid.ItemsSource = orderedDocuments;
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
                documentsGrid.ItemsSource = documents;
            }
            else
            {
                foreach (Models.Agreement document in documents)
                {
                    if (document.document.title.Contains(search.Text))
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
        private void documentsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rootFrame.Navigate(typeof(AgreedDocument), (Agreement)documentsGrid.SelectedItem);
        }
    }
}
