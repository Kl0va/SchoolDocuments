using SchoolDocuments.Models;
using SchoolDocuments.Moduls;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;
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
using Windows.UI.Xaml.Printing;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace SchoolDocuments.General
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CreateDocument : Page
    {
        private static bool saving = false;

        private static List<string> famList = new List<string>();
        private static List<Familiarize> famList1 = new List<Familiarize>();
        private static List<string> signerList = new List<string>();
        private static List<Agreement> signerList1 = new List<Agreement>();

        public CreateDocument()
        {
            this.InitializeComponent();
            Load();
        }


        private static readonly List<User> users = new List<User>();
        private static readonly List<User> usersSig = new List<User>();
        private static readonly List<Models.Template> templates = new List<Models.Template>();
        public async void Load()
        {
            Agreement.Items.Clear();
            Task<List<User>> userTask = ApiWork.GetAllUsers();
            await userTask.ContinueWith(task =>
            { 
                users.Clear();
                foreach (User user in userTask.Result)
                {
                    users.Add(user);
                }
            });
            Task<List<Models.Template>> getTemplates = ApiWork.GetAllTemplates();
            await getTemplates.ContinueWith(t =>
            {
                templates.Clear();
                foreach (Models.Template template in getTemplates.Result)
                {
                    templates.Add(template);
                }
            });
            foreach (Models.Template template1 in templates)
            {
                Template.Items.Add(template1.name);
            }
            List<string> FIO = new List<string>();
            foreach (User user1 in users)
            {
                FIO.Add(user1.firstName + " " + user1.secondName + " " + user1.middleName);
            }
            if (!saving)
            {
                foreach (string fio in FIO)
                {
                    Signatory.Items.Add(fio);
                    familiarize.Items.Add(fio);
                    Agreement.Items.Add(fio);
                }
            }
        }
        private static bool first = true;
        private async void addsigner_Click(object sender, RoutedEventArgs e)
        {
            if (!signerList.Contains(Signatory.SelectedValue.ToString()))
            {
                if (first)
                {
                    string text = "";
                    DocumentText.Document.GetText(TextGetOptions.FormatRtf, out text);
                    string textToAdd = Utils.toRTF("С приказом ознакомлен(а):" + Signatory.SelectedValue.ToString());
                    DocumentText.Document.SetText(TextSetOptions.FormatRtf, text.Insert(text.LastIndexOf('}') - 1, "\n" + textToAdd));
                    signerList.Add(Signatory.SelectedValue.ToString());
                    first = false;
                }
                else if (!first)
                {
                    string text = "";
                    DocumentText.Document.GetText(TextGetOptions.FormatRtf, out text);
                    string textToAdd = Utils.toRTF(Signatory.SelectedValue.ToString());
                    DocumentText.Document.SetText(TextSetOptions.FormatRtf, text.Insert(text.LastIndexOf('}') - 1, textToAdd));
                    signerList.Add(Signatory.SelectedValue.ToString());
                }
            }
            else
            {
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Человек уже был добавлен",
                    PrimaryButtonText = "Ок"
                };
                ContentDialogResult result = await errorDialog.ShowAsync();
            }
        }

        private static readonly List<Models.Template> templates1 = new List<Models.Template>();
        private async void save_Click(object sender, RoutedEventArgs e)
        {
            if (saving)
            {
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                //File.WriteAllBytes(storageFolder.Path + @"\save.mod.docx", );
                File.Create(storageFolder.Path + @"\save.mod.docx").Close();

                StorageFile file = await StorageFile.GetFileFromPathAsync(storageFolder.Path + @"\save.mod.docx");

                Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                DocumentText.Document.SaveToStream(TextGetOptions.FormatRtf, randAccStream);
                await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                randAccStream.Dispose();
                documentForSave.file = File.ReadAllBytes(file.Path);
                ApiWork.SaveDocument(documentForSave);
                Frame.GoBack();
            }
            else if (Template.SelectedValue.ToString() != "")
            {
                Task<List<User>> userTask = ApiWork.GetAllUsers();
                await userTask.ContinueWith(task =>
                {
                    users.Clear();
                    foreach (User user in userTask.Result)
                    {
                        if (famList.Contains(user.firstName + " " + user.secondName + " " + user.middleName))
                        {
                            users.Add(user);
                        }
                        if (signerList.Contains(user.firstName + " " + user.secondName + " " + user.middleName))
                        {
                            usersSig.Add(user);
                        }
                    }
                });
                string saveText = "";
                //Добавление текста
                DocumentText.Document.GetText(Windows.UI.Text.TextGetOptions.None, out saveText);
                Document document1 = null;
                foreach (User user1 in users)
                {
                    Familiarize familiarize = new Familiarize(user1, document1, false, DateTime.Now);
                    famList1.Add(familiarize);
                }
                foreach (User user2 in usersSig)
                {
                    Agreement agreement = new Agreement(user2, document1, TimeOfAgreement.Date.DateTime, AgreementStatus.Sent, "", DateTime.Now);
                    signerList1.Add(agreement);
                }
                await userTask.ContinueWith(task =>
                {
                    users.Clear();
                    foreach (User user in userTask.Result)
                    {
                        if (user.id == UserInfo.Id)
                        {
                            users.Add(user);
                        }
                    }
                });

                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                //File.WriteAllBytes(storageFolder.Path + @"\save.mod.docx", );
                File.Create(storageFolder.Path + @"\save.mod.docx").Close();

                StorageFile file = await StorageFile.GetFileFromPathAsync(storageFolder.Path + @"\save.mod.docx");

                Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                DocumentText.Document.SaveToStream(TextGetOptions.FormatRtf, randAccStream);
                await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                randAccStream.Dispose();

                Document document = new Document(Template.SelectedValue.ToString(), users[0], pageHeader.Text.Trim(), File.ReadAllBytes(file.Path), null, famList1, signerList1);
                ApiWork.AddDocument(document);
                Frame.GoBack();
            }
            else
            {
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Выберите шаблон",
                    PrimaryButtonText = "Ок"
                };
                ContentDialogResult result = await errorDialog.ShowAsync();
            }
        }
        private async void add_fam_Click(object sender, RoutedEventArgs e)
        {
            if (famList.Contains(familiarize.SelectedValue.ToString()))
            {
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Человек уже был добавлен",
                    PrimaryButtonText = "Ок"
                };
                ContentDialogResult result = await errorDialog.ShowAsync();
            }
            else
            {
                famList.Add(familiarize.SelectedValue.ToString());
            }
        }
        private static List<Models.Template> searchTemplate = new List<Models.Template>();
        private static string header;

        private async void Template_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            header = Template.SelectedValue.ToString();
            Task<List<Models.Template>> getTemplates = ApiWork.GetAllTemplates();
            await getTemplates.ContinueWith(t =>
            {
                searchTemplate.Clear();
                foreach (Models.Template template in getTemplates.Result)
                {
                    if (template.name == header)
                    {
                        searchTemplate.Add(template);
                    }
                }
            });
            foreach (Models.Template template1 in searchTemplate)
            {
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                File.WriteAllBytes(storageFolder.Path + @"\save.mod.docx", template1.file.ToArray());
                StorageFile file = await StorageFile.GetFileFromPathAsync(storageFolder.Path + @"\save.mod.docx");
                Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                DocumentText.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randAccStream);
            }
        }

        private async void add_agreed_Click(object sender, RoutedEventArgs e)
        {
            if (!signerList.Contains(Agreement.SelectedValue.ToString()))
            {
                signerList.Add(Agreement.SelectedValue.ToString());
            }
            else
            {
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Человек уже был добавлен",
                    PrimaryButtonText = "Ок"
                };
                ContentDialogResult result = await errorDialog.ShowAsync();
            }
        }
        private static Document document2;
        private static List<string> fios = new List<string>();
        private static List<Agreement> agreements = new List<Agreement>();
        private static List<Familiarize> familiarizes = new List<Familiarize>();
        private static Document documentForSave;

        public static WordDocument document = new WordDocument();
        public static WSection section = document.AddSection() as WSection;

        

        private static RichEditBox richEditBox = new RichEditBox();

        private static byte[] testByte;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (e.Parameter != null)
            {
                Models.Document template = e.Parameter as Models.Document;
                documentForSave = e.Parameter as Models.Document;
                agreements = template.agreement;
                testByte = template.file;
                familiarizes = template.familiarize;
                document2 = e.Parameter as Document;
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

                File.WriteAllBytes(storageFolder.Path + @"\save.mod.docx", template.file.ToArray());
                StorageFile file = await StorageFile.GetFileFromPathAsync(storageFolder.Path + @"\save.mod.docx");
                Windows.Storage.Streams.IRandomAccessStream randAccStream =
                await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                DocumentText.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randAccStream);
                richEditBox.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randAccStream);
                pageHeader.Text = template.title;
                saving = true;

                Template.Visibility = Visibility.Collapsed;
                TimeOfAgreement.Visibility = Visibility.Collapsed;
                Signatory.Visibility = Visibility.Collapsed;
                familiarize.Visibility = Visibility.Collapsed;
                add_fam.Visibility = Visibility.Collapsed;
                addsigner.Visibility = Visibility.Collapsed;
                add_agreed.Visibility = Visibility.Collapsed;
                time.Visibility = Visibility.Collapsed;
                Comment.Visibility = Visibility.Visible;

                Agreement.Items.Clear();
                fios.Clear();
                int count = 0;
                foreach (Agreement agreement in agreements)
                {
                    Task<User> userTask = ApiWork.GetUserInfo(agreement.user.id);
                    await userTask.ContinueWith(task =>
                    {
                        fios.Add(userTask.Result.firstName + " " + userTask.Result.secondName + " " + userTask.Result.middleName);
                    });
                    if (agreement.status == AgreementStatus.Agreed)
                    {
                        count += 1;
                    }
                }
                if (count == agreements.Count)
                {
                    Console.WriteLine(count);
                }
                foreach (string fio in fios)
                {
                    Agreement.Items.Add(fio);
                }
                List<Agreement> check = agreements.FindAll(p => p.status == AgreementStatus.Agreed);
                if (check.Count == agreements.Count)
                {
                    SaveDoc.Visibility = Visibility.Visible;
                }
            }
        }

        private void Signatory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void Agreement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (saving)
            {
                agreedStatus.Visibility = Visibility.Visible;
                Comment.Visibility = Visibility.Visible;
                string sign = Agreement.SelectedValue.ToString();
                usersSig.Clear();
                foreach (Agreement agreement in agreements)
                {
                    Task<User> userTask = ApiWork.GetUserInfo(agreement.user.id);
                    await userTask.ContinueWith(task =>
                    {
                        if (sign.Contains(userTask.Result.firstName + " " + userTask.Result.secondName + " " + userTask.Result.middleName))
                        {
                            usersSig.Add(userTask.Result);
                        }
                    });
                    if (usersSig.Count > 0)
                    {
                        foreach (User user1 in usersSig)
                        {
                            if (sign == user1.firstName + " " + user1.secondName + " " + user1.middleName)
                            {
                                if (agreement.status.ToString() == "Sent")
                                {
                                    agreedStatus.Text = "Отправлено";
                                }
                                else if (agreement.status.ToString() == "Declined")
                                {
                                    agreedStatus.Text = "Отклонено";
                                }
                                else if (agreement.status.ToString() == "Agreed")
                                {
                                    agreedStatus.Text = "Одобрено";
                                }
                            }
                        }
                    }
                    else
                    {
                        agreedStatus.Text = "Не отправлено";
                    }
                    if (agreement.comment != "" && agreement.comment != null)
                    {
                        Comment.Text = agreement.comment;
                    }
                    else
                    {
                        Comment.Text = "Комментария не было оставлено";
                    }
                }
            }
        }

        /// <summary>
        /// Сохранение файла на комп
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveDoc_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            File.Create(storageFolder.Path + @"\save.mod.docx").Close();

            StorageFile file = await StorageFile.GetFileFromPathAsync(storageFolder.Path + @"\save.mod.docx");

            Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.ReadWrite);
            DocumentText.Document.SaveToStream(TextGetOptions.FormatRtf, randAccStream);
            await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

            document.Open(randAccStream.AsStream(),FormatType.Rtf);

            WParagraphStyle style = document.AddParagraphStyle("Normal") as WParagraphStyle;
            style.CharacterFormat.FontName = "Times New Roman";
            style.CharacterFormat.FontSize = 12f;
            style.ParagraphFormat.BeforeSpacing = 0;
            style.ParagraphFormat.LineSpacing = 13.8f;

            style.CharacterFormat.TextColor = Syncfusion.DocIO.DLS.Color.Black;
            style.ParagraphFormat.BeforeSpacing = 12;
            style.ParagraphFormat.AfterSpacing = 0;
            style.ParagraphFormat.Keep = true;
            style.ParagraphFormat.KeepFollow = true;
            style.ParagraphFormat.OutlineLevel = OutlineLevel.Level1;

            IWParagraph paragraph = section.HeadersFooters.Header.AddParagraph();
            paragraph.ApplyStyle("Normal");
            paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
            paragraph = section.AddParagraph();

            MemoryStream memoryStream = new MemoryStream();
            await document.SaveAsync(memoryStream, FormatType.Rtf);
            Save(memoryStream,"Document.rtf");
            randAccStream.Dispose();
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
                //Frame.Navigate(typeof(AdminPage));
            }
        }
    }
}
