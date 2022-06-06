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
    public sealed partial class CreateTask : Page
    {
        private static List<User> users = new List<User>();
        public CreateTask()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
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
            var itemToDelete = users.Where(x => x.email == UserInfo.Email);
            users.Remove(itemToDelete.First());
            List<string> FIO = new List<string>();
            foreach (User user1 in users)
            {
                FIO.Add(user1.firstName + " " + user1.secondName + " " + user1.middleName);
            }
            foreach (string fio in FIO)
            {
                Agreement.Items.Add(fio);
            }
        }

        private static List<Performer> performers = new List<Performer>();
        private async void add_agreed_Click(object sender, RoutedEventArgs e)
        {
            User user1 = null;
            foreach (User user in users)
            {
                if (user.firstName + " " + user.secondName + " " + user.middleName == Agreement.SelectedItem.ToString())
                {
                    if (performers.Where(x => x.user.fullName.Contains(Agreement.SelectedItem.ToString())).Any())
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
                        user1 = user;
                    }
                }
            }
            if (user1 != null)
            {
                addedPerf.Text += $"\n {user1.firstName + " " + user1.secondName}";
                List<Document> documents = new List<Document>();
                Performer performer = new Performer(user1, documents);
                performers.Add(performer);
            }
        }

        private async void save_Click(object sender, RoutedEventArgs e)
        {
            if (performers.Count < 1)
            {
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Не было добавлено исполнителей",
                    PrimaryButtonText = "Ок"
                };
                ContentDialogResult result = await errorDialog.ShowAsync();
            }
            else
            {
                if (pageHeader.Text == "" || pageHeader.Text == null || Description.Text == "" || Description.Text == null || TimeOfAgreement.Date.DateTime.Year < 2022)
                {
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = "Ошибка",
                        Content = "Заполните все поля",
                        PrimaryButtonText = "Ок"
                    };
                    ContentDialogResult result = await errorDialog.ShowAsync();
                }
                else
                {
                    List<Models.Document> docs = new List<Document>();
                    Models.Task task = new Models.Task(pageHeader.Text.Trim(), Description.Text, DateTime.Now, TimeOfAgreement.Date.DateTime, UserInfo.user, performers,docs);
                    List<Models.File> file = new List<Models.File>();
                    Models.TaskWithFiles taskWithFile = new TaskWithFiles(task,file);
                    ApiWork.AddTask(taskWithFile);
                    Frame.Navigate(typeof(UsersPage));
                }
            }
        }

        private async void TimeOfAgreement_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            if(DateTime.Now > TimeOfAgreement.Date)
            {
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = "Ошибка",
                    Content = "Некорректная дата",
                    PrimaryButtonText = "Ок"
                };
                ContentDialogResult result = await errorDialog.ShowAsync();
                TimeOfAgreement.Date = DateTime.Now.AddDays(1);
            }
        }
    }
}
