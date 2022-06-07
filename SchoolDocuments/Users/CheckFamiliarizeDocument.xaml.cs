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
using Windows.Storage;
using Windows.UI.Text;
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
    public sealed partial class CheckFamiliarizeDocument : Page
    {
        static Familiarize familiarize1;
        /// <summary>
        /// Инициализация
        /// </summary>
        public CheckFamiliarizeDocument()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Ознакомление с документом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Click(object sender, RoutedEventArgs e)
        {
            familiarize1.familiarized = true;
            ApiWork.SaveFam(familiarize1);
            Frame.Navigate(typeof(UsersPage));
        }
        /// <summary>
        /// Подгрузка данных
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                Models.Familiarize familiarize = e.Parameter as Models.Familiarize;
                familiarize1 = e.Parameter as Familiarize;
                pageHeader.Text = familiarize.document.title;
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

                byte[] files = null;
                Task<Models.File> task = ApiWork.GetUserDocumentsFile(familiarize.document.id);
                await task.ContinueWith(task1 =>
                {
                    files = task1.Result.file;
                });
                System.IO.File.WriteAllBytes(storageFolder.Path + @"\save.mod.docx", files.ToArray());
                StorageFile file = await StorageFile.GetFileFromPathAsync(storageFolder.Path + @"\save.mod.docx");
                Windows.Storage.Streams.IRandomAccessStream randAccStream =
                await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                TemplateText.Document.LoadFromStream(TextSetOptions.FormatRtf,randAccStream);
            }
        }
    }
}
