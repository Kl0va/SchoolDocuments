using SautinSoft.Document;
using SchoolDocuments.Models;
using SchoolDocuments.Moduls;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
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
    public sealed partial class TasksDetail : Page
    {
        public TasksDetail()
        {
            this.InitializeComponent();
        }

        private static Models.Task task;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            task = e.Parameter as Models.Task;
            if (!(task.author.email == UserInfo.Email))
            {
                Agreement.Visibility = Visibility.Collapsed;
                save.Visibility = Visibility.Collapsed;
                perfComment.Visibility = Visibility.Collapsed;

                if (task.performs.Where(x => x.user.email == UserInfo.Email).All(y => y.status == Models.PerformerStatus.InProgress))
                {
                    InProgress.Visibility = Visibility.Collapsed;
                    Attach.Visibility = Visibility.Visible;
                }
                else if (task.performs.Where(x => x.user.email == UserInfo.Email).All(y => y.status == Models.PerformerStatus.Completed))
                {
                    InProgress.Visibility = Visibility.Collapsed;
                    Comment.IsReadOnly = true;
                    foreach (Performer perf in task.performs)
                    {
                        if (UserInfo.user.id.ToString() == perf.user.id.ToString())
                        {
                            if (perf.comment != null && perf.comment != "")
                            {
                                Comment.Text = perf.comment;
                            }
                            else
                            {
                                Comment.Text = "Комментария не было оставлено";
                            }
                        }
                    }
                    Finished.Visibility = Visibility.Collapsed;
                }
                else if (task.performs.Where(x => x.user.email == UserInfo.Email).All(y => y.status == Models.PerformerStatus.Waiting))
                {
                    Comment.Visibility = Visibility.Collapsed;
                    Finished.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                InProgress.Visibility = Visibility.Collapsed;
                Finished.Visibility = Visibility.Collapsed;
                Comment.Visibility = Visibility.Collapsed;
            }
            pageHeader.Text = task.title;
            Description.Text = task.desc;

            foreach (Models.Performer performer in task.performs)
            {
                Agreement.Items.Add(performer.user.firstName + " " + performer.user.secondName + " " + performer.user.middleName);
            }
        }
        private static bool change = false;
        private void Agreement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Models.Performer performer in task.performs)
            {
                if (Agreement.SelectedItem.ToString() == performer.user.firstName + " " + performer.user.secondName + " " + performer.user.middleName)
                {
                    if (performer.documents.Count > 0)
                    {
                        Attach.Visibility = Visibility.Visible;
                        Attach.Content = "Открыть файл";
                        change = true;
                        AttachStatus.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        AttachStatus.Visibility = Visibility.Visible;
                        AttachStatus.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                        AttachStatus.Text = "Документ не был прикреплен";
                    }
                    if (performer.status == Models.PerformerStatus.InProgress)
                    {
                        perfStatus.Text = "В процессе";
                    }
                    else if (performer.status == Models.PerformerStatus.Completed)
                    {
                        perfStatus.Text = "Выполнено";
                    }
                    else if (performer.status == Models.PerformerStatus.Waiting)
                    {
                        perfStatus.Text = "В ожидании";
                    }
                    if (performer.comment == "" || performer.comment == null)
                    {
                        perfComment.Text = "Комментария не было оставлено";
                    }
                    else
                    {
                        perfComment.Text = performer.comment;
                    }
                }
            }
        }

        private async void save_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog errorDialog = new ContentDialog()
            {
                Title = "Удаление",
                Content = "Вы точно хотите удалить задание ?",
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Отмена"
            };

            ContentDialogResult result = await errorDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ApiWork.DeleteTask(task);
                Frame.GoBack();
            }
        }

        private void InProgress_Click(object sender, RoutedEventArgs e)
        {
            Models.Performer idPerf = task.performs.Where(x => x.user.email == UserInfo.Email).Single();
            ApiWork.PutTaskStatus(idPerf.id, Models.PerformerStatus.InProgress);
            Frame.Navigate(typeof(UsersPage));
        }

        private void Finished_Click(object sender, RoutedEventArgs e)
        {
            Models.Performer idPerf = task.performs.Where(x => x.user.email == UserInfo.Email).Single();
            ApiWork.PutTaskStatus(idPerf.id, Models.PerformerStatus.Completed);
            if (Comment.Text != null && Comment.Text != "")
            {
                ApiWork.PutTaskComment(idPerf.id, Comment.Text);
            }
            if (document != null)
            {
                performer.id = idPerf.id;
                ApiWork.AddDocumentToPerformer(document[0], performer);
            }
            Frame.Navigate(typeof(UsersPage));
        }
        private static List<Document> document = new List<Document>();
        private static Performer performer;

        public static WordDocument document1 = new WordDocument();
        public static WSection section = document1.AddSection() as WSection;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!change)
            {
                Windows.Storage.Pickers.FileOpenPicker open = new Windows.Storage.Pickers.FileOpenPicker();
                open.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                open.FileTypeFilter.Add(".rtf");
                Windows.Storage.StorageFile file = await open.PickSingleFileAsync();
                if (file != null)
                {
                    Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                    StorageFile file1 = await StorageFile.GetFileFromPathAsync(storageFolder.Path + @"\save.mod.docx");

                    Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                    Inp.Document.LoadFromStream(TextSetOptions.FormatRtf, randAccStream);

                    Windows.Storage.Streams.IRandomAccessStream randAccStream1 = await file1.OpenAsync(FileAccessMode.ReadWrite);
                    Inp.Document.SaveToStream(TextGetOptions.FormatRtf, randAccStream1);
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file1);

                    randAccStream.Dispose();
                    randAccStream1.Dispose();

                    List<Familiarize> familiarizes = new List<Familiarize>();
                    List<Agreement> agreements = new List<Agreement>();

                    Document doc = new Document(null, UserInfo.user, "Prik", File.ReadAllBytes(file1.Path), "", familiarizes, agreements);
                    document.Add(doc);
                    performer = new Performer(UserInfo.user, document);
                    AttachStatus.Visibility = Visibility.Visible;
                }
            }
            else
            {
                foreach (Models.Performer performer in task.performs)
                {
                    if (Agreement.SelectedItem.ToString() == performer.user.firstName + " " + performer.user.secondName + " " + performer.user.middleName)
                    {

                        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                        File.WriteAllBytes(storageFolder.Path + @"\save.mod.docx", performer.documents[0].file.ToArray());
                        StorageFile file = await StorageFile.GetFileFromPathAsync(storageFolder.Path + @"\save.mod.docx");
                        Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                        Inp.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randAccStream);
                        randAccStream.Dispose();


                        Windows.Storage.Streams.IRandomAccessStream randAccStream1 = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                        Inp.Document.SaveToStream(TextGetOptions.FormatRtf, randAccStream1);
                        await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

                        document1.Open(randAccStream1.AsStream(), FormatType.Rtf);

                        WParagraphStyle style = document1.AddParagraphStyle("Normal") as WParagraphStyle;
                        style.CharacterFormat.FontName = "Times New Roman";
                        style.CharacterFormat.FontSize = 12f;
                        style.ParagraphFormat.BeforeSpacing = 0;
                        style.ParagraphFormat.LineSpacing = 13.8f;

                        style.CharacterFormat.TextColor = Syncfusion.DocIO.DLS.Color.Black;
                        style.ParagraphFormat.BeforeSpacing = 12;
                        style.ParagraphFormat.AfterSpacing = 0;
                        style.ParagraphFormat.Keep = true;
                        style.ParagraphFormat.KeepFollow = true;
                        style.ParagraphFormat.OutlineLevel = Syncfusion.DocIO.OutlineLevel.Level1;

                        IWParagraph paragraph = section.HeadersFooters.Header.AddParagraph();
                        paragraph.ApplyStyle("Normal");
                        paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
                        paragraph = section.AddParagraph();

                        MemoryStream memoryStream = new MemoryStream();
                        await document1.SaveAsync(memoryStream, FormatType.Rtf);
                        Save(memoryStream, "Document.rtf");
                        randAccStream1.Dispose();
                    }
                }
            }
        }
        async void Save(MemoryStream streams, string filename)
        {
            streams.Position = 0;
            StorageFile stFile;
            if (!(Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")))
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.DefaultFileExtension = ".rtf";
                savePicker.SuggestedFileName = filename;
                savePicker.FileTypeChoices.Add("Word Documents", new List<string>() { ".rtf" });
                stFile = await savePicker.PickSaveFileAsync();
            }
            else
            {
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                stFile = await local.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            }
            if (stFile != null)
            {
                using (IRandomAccessStream zipStream = await stFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (Stream outstream = zipStream.AsStreamForWrite())
                    {
                        byte[] buffer = streams.ToArray();
                        outstream.Write(buffer, 0, buffer.Length);
                        outstream.Flush();
                    }
                }
                await Windows.System.Launcher.LaunchFileAsync(stFile);
            }
        }
    }
}
