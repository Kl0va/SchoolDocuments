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
    public sealed partial class AgreedDocument : Page
    {
        static Agreement familiarize1;
        /// <summary>
        /// Инициализация
        /// </summary>
        public AgreedDocument()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Подгрузка данных
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                Models.Agreement familiarize = e.Parameter as Models.Agreement;
                if (familiarize.status == AgreementStatus.Declined || familiarize.status == AgreementStatus.Agreed)
                {
                    save.IsEnabled = false;
                    dontSave.IsEnabled = false;
                    Comment.IsEnabled = false;
                }
                familiarize1 = e.Parameter as Agreement;
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
                TemplateText.Document.LoadFromStream(TextSetOptions.FormatRtf, randAccStream);
            }
        }

        /// <summary>
        /// Сохранение подписания/согласования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Click(object sender, RoutedEventArgs e)
        {
            familiarize1.status = AgreementStatus.Agreed;
            ApiWork.SaveAgreementWithout(familiarize1);
            Frame.Navigate(typeof(UsersPage));
        }
        /// <summary>
        /// Отклонение документа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Copy_Click(object sender, RoutedEventArgs e)
        {
            familiarize1.comment = Comment.Text;
            familiarize1.status = AgreementStatus.Declined;
            ApiWork.SaveAgreementWith(familiarize1);
            Frame.Navigate(typeof(UsersPage));
        }
    }
}
