using System;
using System.Windows.Controls;
using Zh.WPF.Persistence;
using Zh.Persistence.DTO;

namespace Zh.WPF.ViewModel
{

    public class LoginViewModel : ViewModelBase
    {
        private IPortalPersistence _model;

        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand LoginCommand { get; private set; }

        public String UserName { get; set; }

        public event EventHandler ExitApplication;

        public event EventHandler<PublisherDTO> LoginSuccess;

        public event EventHandler LoginFailed;

        public LoginViewModel(IPortalPersistence model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            UserName = String.Empty;

            ExitCommand = new DelegateCommand(param => OnExitApplication());

            LoginCommand = new DelegateCommand(param => LoginAsync(param as PasswordBox));
        }

        private async void LoginAsync(PasswordBox passwordBox)
        {
            if (passwordBox == null) return;

            try
            {
                var result = await _model.LoginAsync(UserName, passwordBox.Password);

                if (result != null)
                    OnLoginSuccess(result);
                else
                    OnLoginFailed();
            }
            catch (PersistenceUnavailableException ex)
            {
                OnMessageApplication($"No connection to the service. Reason: {ex.Message}");
            }
        }

        private void OnLoginSuccess(PublisherDTO author)
        {
            LoginSuccess?.Invoke(this, author);
        }

        private void OnExitApplication()
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoginFailed()
        {
            LoginFailed?.Invoke(this, EventArgs.Empty);
        }

    }
}
