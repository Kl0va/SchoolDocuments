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
    public sealed partial class CreateDocument : Page
    {
        public CreateDocument()
        {
            this.InitializeComponent();
            Load();
        }


        private static readonly List<User> users = new List<User>();
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

        private void addsigner_Click(object sender, RoutedEventArgs e)
        {

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_fam_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Template_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
