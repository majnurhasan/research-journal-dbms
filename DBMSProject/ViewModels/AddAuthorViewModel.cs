using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DataLayer.EfCode;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ServiceLayer.AuthorService;
using ServiceLayer.EditorService;
using ServiceLayer.ManuscriptService;
using ServiceLayer.ReviewerService;

namespace DBMSProject.ViewModels
{
    public class AddAuthorViewModel : ObservableObject
    {
        private readonly EfCoreContext _context;
        private AddAuthorService _addAuthorService;
        private ListManuscriptService _listManuscriptService;
        private AddSingleManuscriptService _addSingleManuscriptService;
        private string _inputName;
        private string _inputEmailAddress;
        private string _inputAffiliation;
        private string _inputUsername;
        private string _inputPassword;
        private string _inputMailingAddress;
        private string _inputManuscriptTitle;
        public AddAuthorDto AuthorToAdd { get; private set; }
        public AddSingleManuscriptDto ManuscriptToAdd { get; private set; }
        public string InputManuscriptTitle
        {
            get { return _inputManuscriptTitle; }
            set
            {
                _inputManuscriptTitle = value;
                RaisePropertyChanged(nameof(InputManuscriptTitle));
            }
        }
        public string InputName
        {
            get { return _inputName; }
            set
            {
                _inputName = value;
                RaisePropertyChanged(nameof(InputName));
            }
        }
        public string InputMailingAddress
        {
            get { return _inputMailingAddress; }
            set
            {
                _inputMailingAddress = value;
                RaisePropertyChanged(nameof(InputMailingAddress));
            }
        }
        public string InputEmailAddress
        {
            get { return _inputEmailAddress; }
            set
            {
                _inputEmailAddress = value;
                RaisePropertyChanged(nameof(InputEmailAddress));
            }
        }
        public string InputAffiliation
        {
            get { return _inputAffiliation; }
            set
            {
                _inputAffiliation = value;
                RaisePropertyChanged(nameof(InputAffiliation));
            }
        }
        public string InputUsername
        {
            get { return _inputUsername; }
            set
            {
                _inputUsername = value;
                RaisePropertyChanged(nameof(InputUsername));
            }
        }
        public string InputPassword
        {
            get { return _inputPassword; }
            set
            {
                _inputPassword = value;
                RaisePropertyChanged(nameof(InputPassword));
            }
        }
        public ObservableCollection<ManuscriptListDto> ManuscriptList { get; set; }
        public AddAuthorViewModel(EfCoreContext context)
        {
            _context = context;
        }
        public AddAuthorViewModel(AddAuthorService addAuthorService, AddSingleManuscriptService addSingleManuscriptService) : this(new EfCoreContext())
        {
            InputName = "";
            InputMailingAddress = "";
            InputEmailAddress = "";
            InputAffiliation = "";
            InputUsername = "";
            InputPassword = "";
            InputManuscriptTitle = "";

            _addAuthorService = addAuthorService;
            _addSingleManuscriptService = addSingleManuscriptService;

            AuthorToAdd = new AddAuthorDto
            {
                Name = "",
                MailingAddress = "",
                EmailAddress = "",
                Affiliation = "",
                Username = "",
                Password = "",
            };
            ManuscriptToAdd = new AddSingleManuscriptDto
            {
                EditorId = 1,
                ManuscriptTitle = "",
                DateReceived = DateTime.Now,
                ManuscriptStatus = 1,
            };

            _listManuscriptService = new ListManuscriptService(_context);

            var manuscripts = _listManuscriptService.GetManuscriptList().ToList();

            ManuscriptList = new ObservableCollection<ManuscriptListDto>(manuscripts);
        }

        public ICommand SaveAuthorCommand => new RelayCommand(SaveAuthor);
        private void SaveAuthor()
        {
            if (InputName == "" || InputAffiliation == "" || InputManuscriptTitle == "" ||
                InputEmailAddress == "" || InputMailingAddress == "" || InputPassword == "" ||
                InputPassword == "")
            {
                MessageBox.Show($"Please fill in all fields.",
                    "Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                //Manucript
                Random rnd = new Random();
                ManuscriptToAdd.EditorId = rnd.Next(1, 6);
                ManuscriptToAdd.ManuscriptTitle = InputManuscriptTitle;
                ManuscriptToAdd.DateReceived = DateTime.Now;
                ManuscriptToAdd.ManuscriptStatus = 1;
                _addSingleManuscriptService.AddSingleManuscript(ManuscriptToAdd);

                //Author
                var authorFirstName = InputName.Split(' ').First();
                var singleManuscript = ManuscriptList.Last();
                AuthorToAdd.ManuscriptsId.Add(singleManuscript.ManuscriptId + 1);
                AuthorToAdd.Name = InputName;
                AuthorToAdd.MailingAddress = InputMailingAddress;
                AuthorToAdd.EmailAddress = InputEmailAddress;
                AuthorToAdd.Affiliation = InputAffiliation;
                AuthorToAdd.Username = InputUsername;
                AuthorToAdd.Password = ComputeSha256Hash(InputPassword);
                _addAuthorService.AddAuthor(AuthorToAdd);

                InputName = "";
                InputMailingAddress = "";
                InputEmailAddress = "";
                InputAffiliation = "";
                InputUsername = "";
                InputPassword = "";
                InputManuscriptTitle = "";

                MessageBox.Show($"Thank you for registering, {authorFirstName}! May more amazing manuscripts be published with your skills!",
                    "Success!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        //Hash Method
        static string ComputeSha256Hash(string rawData)
        {

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
