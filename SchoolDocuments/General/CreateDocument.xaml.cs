﻿using SchoolDocuments.Models;
using SchoolDocuments.Moduls;
using SchoolDocuments.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
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

namespace SchoolDocuments.General
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CreateDocument : Page
    {
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
        private static readonly List<User> usersFam = new List<User>();
        private static readonly List<Models.Template> templates = new List<Models.Template>();
        private static readonly List<Models.Document> documents1 = new List<Models.Document>();
        public async void Load()
        {
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
            foreach (string fio in FIO)
            {
                Signatory.Items.Add(fio);
                familiarize.Items.Add(fio);
            }
        }
        private static bool first = true;
        private async void addsigner_Click(object sender, RoutedEventArgs e)
        {
            string text = "";
            DocumentText.Document.GetText(TextGetOptions.AdjustCrlf, out text);
            if (first)
            {
                //string signText = "\n\nС приказом ознакомлен(а):\n" + Signatory.SelectedValue.ToString();
                //DocumentText.Document.SetText(TextSetOptions.UnicodeBidi, signText);
                //DocumentText.Document.GetText(TextGetOptions.AdjustCrlf, out signText);
                ////signText = "\n\nС приказом ознакомлен(а):\n" + Signatory.SelectedValue.ToString();
                //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                //File.Create(storageFolder.Path + @"\save.mod.rtf").Close();
                //StorageFile file = await StorageFile.GetFileFromPathAsync(storageFolder.Path + @"\save.mod.rtf");
                //string sumText = text + signText;
                //File.WriteAllText(file.Path, sumText);
                

                //var randAccStream = await file.OpenAsync(FileAccessMode.Read);
                //DocumentText.Document.LoadFromStream(TextSetOptions.CheckTextLimit, randAccStream);
                //randAccStream.Dispose();
                signerList.Add(Signatory.SelectedValue.ToString());
                first = false;
            }
            else
            {
                //text += "\n" + Signatory.SelectedValue.ToString();
                //DocumentText.Document.SetText(TextSetOptions.FormatRtf, text);
                signerList.Add(Signatory.SelectedValue.ToString());
            }
        }

        private static readonly List<Models.Template> templates1 = new List<Models.Template>();
        private async void save_Click(object sender, RoutedEventArgs e)
        {
            if (Template.SelectedValue.ToString() != "")
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
                foreach (User user1 in users)
                {
                    Familiarize familiarize = new Familiarize(user1.id, 0, false, DateTime.Now);
                    famList1.Add(familiarize);
                }
                foreach (User user2 in usersSig)
                {
                    Agreement agreement = new Agreement(user2.id, 0, TimeOfAgreement.Date.DateTime, AgreementStatus.Sent, "", DateTime.Now);
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


                MemoryStream stream = new MemoryStream();

                Document document = new Document(Template.SelectedValue.ToString(), users[0], pageHeader.Text, File.ReadAllBytes(file.Path), "", famList1, signerList1);
                ApiWork.AddDocument(document);
                File.WriteAllBytes(storageFolder.Path,File.ReadAllBytes(file.Path));
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

        async void Save(MemoryStream streams, string filename)
        {
            streams.Position = 0;
            StorageFile stFile;
            if (!(Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")))
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.DefaultFileExtension = ".docx";
                savePicker.SuggestedFileName = filename;
                savePicker.FileTypeChoices.Add("Word Documents", new List<string>() { ".docx" });
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
                Frame.Navigate(typeof(UsersPage));
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
    }
}
