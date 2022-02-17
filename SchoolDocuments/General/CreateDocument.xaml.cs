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
        private void addsigner_Click(object sender, RoutedEventArgs e)
        {
            string text = "";
            DocumentText.Document.GetText(Windows.UI.Text.TextGetOptions.None, out text);
            if (first)
            {
                text += "\n\nС приказом ознакомлен(а):\n" + Signatory.SelectedValue.ToString();
                DocumentText.Document.SetText(TextSetOptions.FormatRtf, text);
                signerList.Add(Signatory.SelectedValue.ToString());
                first = false;
            }
            else
            {
                text += "\n" + Signatory.SelectedValue.ToString();
                DocumentText.Document.SetText(TextSetOptions.FormatRtf, text);
                signerList.Add(Signatory.SelectedValue.ToString());
            }
        }

        private static readonly List<Models.Template> templates1 = new List<Models.Template>();
        private async void save_Click(object sender, RoutedEventArgs e)
        {
            if(Template.SelectedValue.ToString() != "")
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
                foreach (User user1 in users) {
                    Familiarize familiarize = new Familiarize(user1.id, 0, false, DateTime.Now);
                    famList1.Add(familiarize);
                }
                foreach(User user2 in usersSig)
                {
                    Agreement agreement = new Agreement(user2.id,0,TimeOfAgreement.Date.DateTime,AgreementStatus.Sent.ToString(),"",DateTime.Now);
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
                Document document = new Document(Template.SelectedValue.ToString(),users[0],pageHeader.Text,saveText,"",famList1,signerList1);
                ApiWork.AddDocument(document);
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
                //DocumentText.Document.SetText(TextSetOptions.FormatRtf, template1.file);
            }
        }
    }
}
