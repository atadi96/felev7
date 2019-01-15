using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Zh.WPF.Persistence;
using Zh.WPF.ViewModel;
using Zh.WPF.View;
using Zh.Persistence.DTO;

namespace Zh.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IPortalPersistence persistence;
        private LoginViewModel loginViewModel;
        private LoginWindow loginWindow;
        private MainViewModel mainViewModel;
        private MainWindow mainWindow;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
            Exit += new ExitEventHandler(App_Exit);
        }

        private async void App_Exit(object sender, ExitEventArgs e)
        {
            if (persistence.IsLoggedOn)
            {
                await persistence.LogoutAsync();
            }
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            persistence = new PortalServicePersistence("http://localhost:5000/");
            loginViewModel = new LoginViewModel(persistence);
            loginViewModel.ExitApplication += ExitApplication;
            loginViewModel.LoginFailed += LoginViewModel_LoginFailed;
            loginViewModel.MessageApplication += ViewModel_MessageApplication;
            loginViewModel.LoginSuccess += LoginSuccess;

            loginWindow = new LoginWindow();
            loginWindow.DataContext = loginViewModel;
            loginWindow.Show();
        }

        private void LoginViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Login failed!", "Publisher login");
        }

        private void LoginSuccess(object sender, PublisherDTO author)
        {
            mainViewModel = new MainViewModel(persistence, author);
            mainViewModel.CreateArticle += MainViewModel_CreateArticle;
            mainViewModel.EditArticle += MainViewModel_EditArticle;
            mainViewModel.ExitApplication += ExitApplication;
            mainViewModel.MessageApplication += ViewModel_MessageApplication;

            mainWindow = new MainWindow();
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();

            loginWindow.Close();
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Auction portal", MessageBoxButton.OK);
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void MainViewModel_EditArticle(object sender, int article)
        {
            ArticleEditViewModel vm = new ArticleEditViewModel(persistence, article);
            ArticleEditWindow editWindow = new ArticleEditWindow();
            vm.BackEvent += (object s, EventArgs e) =>
            {
                editWindow.Close();
            };
            vm.MessageApplication += ViewModel_MessageApplication;
            editWindow.DataContext = vm;
            editWindow.ShowDialog();
        }

        private void MainViewModel_CreateArticle(object sender, EventArgs e)
        {
            ArticleEditViewModel vm = new ArticleEditViewModel(persistence, null);
            ArticleEditWindow editWindow = new ArticleEditWindow();
            vm.BackEvent += (object o, EventArgs ea) =>
            {
                editWindow.Close();
            };
            vm.MessageApplication += ViewModel_MessageApplication;
            editWindow.DataContext = vm;
            editWindow.ShowDialog();
        }
    }
}
