using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DataLayer.EfCode;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ServiceLayer.AuthorService;
using ServiceLayer.ManuscriptService;
using ServiceLayer.NotificationService;
using ServiceLayer.ReviewerService;

namespace DBMSProject.ViewModels
{
    public class AuthorViewModel : ObservableObject
    {
        private readonly EfCoreContext _context;
        private AddMultipleManuscriptService _addMultipleManuscriptService;
        private ListNotificationService _notificationService;
        private ListManuscriptService _manuscriptService;
        private ListAuthorManuscriptService _authorManuscriptService;
        private ListFrontAuthorService _listFrontAuthorService;
        private AuthorListDto _loggedAuthor;
        private string _authorFirstName;
        private ManuscriptListDto _selectedManuscript;
        private string _inputManuscriptTitle;
        private ObservableCollection<AuthorManuscriptSelectionViewDto> _frontAuthorList;
        private InformationWindow _informationWindow;
        private InformationOnProfileWindow _informationOnProfileWindow;
        public AddMultipleManuscriptDto ManuscriptToAdd { get; private set; }
        public ObservableCollection<NotificationListDto> NotificationList { get; set; }
        public ObservableCollection<ManuscriptListDto> ManuscriptList { get; set; }
        public ObservableCollection<AuthorManuscriptListDto> AuthorManuscriptList { get; set; }
        public ObservableCollection<NotificationListDto> LoggedNotificationList { get; set;} = new ObservableCollection<NotificationListDto>();
        public ObservableCollection<ManuscriptListDto> LoggedManuscriptList { get; set; } = new ObservableCollection<ManuscriptListDto>();
        public AuthorListDto LoggedAuthor
        {
            get { return _loggedAuthor; }
            set
            {
                _loggedAuthor = value;
                RaisePropertyChanged(nameof(LoggedAuthor));
            }
        }
        public string AuthorFirstName
        {
            get { return _authorFirstName; }
            set
            {
                _authorFirstName = value;
                RaisePropertyChanged(nameof(AuthorFirstName));
            }
        }

        public ManuscriptListDto SelectedManuscript
        {
            get { return _selectedManuscript; }
            set
            {
                _selectedManuscript = value;
                RaisePropertyChanged(nameof(SelectedManuscript));
            }
        }

        public string InputManuscriptTitle
        {
            get { return _inputManuscriptTitle; }
            set
            {
                _inputManuscriptTitle = value;
                RaisePropertyChanged(nameof(InputManuscriptTitle));
            }
        }

        public ObservableCollection<AuthorManuscriptSelectionViewDto> FrontAuthorList
        {
            get { return _frontAuthorList; }
            set
            {
                _frontAuthorList = value;
                RaisePropertyChanged(nameof(FrontAuthorList));
            }
        }

        public AuthorViewModel(EfCoreContext context)
        {
            _context = context;
        }
        public AuthorViewModel(AuthorListDto loggedAuthor, AddMultipleManuscriptService addMultipleManuscriptService, ListFrontAuthorService listFrontAuthorService) : this(new EfCoreContext())
        {
            InputManuscriptTitle = "";
            LoggedAuthor = loggedAuthor;
            AuthorFirstName = loggedAuthor.Name.Split(' ').First() + "!";
            _addMultipleManuscriptService = addMultipleManuscriptService;
            _listFrontAuthorService = listFrontAuthorService;

            ManuscriptToAdd = new AddMultipleManuscriptDto
            {
                ManuscriptTitle = "",
            };

            _manuscriptService = new ListManuscriptService(_context);
            _notificationService = new ListNotificationService(_context);
            _authorManuscriptService = new ListAuthorManuscriptService(_context);

            var manuscripts = _manuscriptService.GetManuscriptList().ToList();
            var notifications = _notificationService.GetNotificationList().ToList();
            var authorManuscripts = _authorManuscriptService.GetAuthorManuscriptList().ToList();

            ManuscriptList = new ObservableCollection<ManuscriptListDto>(manuscripts);
            NotificationList = new ObservableCollection<NotificationListDto>(notifications);
            AuthorManuscriptList = new ObservableCollection<AuthorManuscriptListDto>(authorManuscripts);

            var notificationCounter = 1;
            foreach (var notification in NotificationList)
            {
                if (loggedAuthor.AuthorId == notification.AuthorId)
                {
                    notification.Message = notificationCounter + ". " + notification.Message;
                    notificationCounter++;
                    LoggedNotificationList.Add(notification);
                }
            }
            notificationCounter = 0;

            var manuscriptCounter = 1;
            var manuscriptIdBag = new List<int>();
            foreach (var authorManuscript in AuthorManuscriptList)
            {
                if (authorManuscript.AuthorId == loggedAuthor.AuthorId)
                {
                    manuscriptIdBag.Add(authorManuscript.ManuscriptId);
                }
            }

            foreach (var manuscript in manuscriptIdBag)
            {
                foreach (var parentManuscript in ManuscriptList)
                {
                    if (manuscript == parentManuscript.ManuscriptId)
                    {
                        parentManuscript.ManuscriptTitle = manuscriptCounter + ". " + parentManuscript.ManuscriptTitle;
                        manuscriptCounter++;
                        LoggedManuscriptList.Add(parentManuscript);
                    }
                }
            }
            manuscriptIdBag.Clear();
            manuscriptCounter = 0;

            FrontAuthorList = GetAuthorSelectionList();
        }

        private ObservableCollection<AuthorManuscriptSelectionViewDto> GetAuthorSelectionList()
        {
            var list = new ObservableCollection<AuthorManuscriptSelectionViewDto>(
                _listFrontAuthorService.GetFrontAuthors().ToList().Where(c => c.AuthorId != LoggedAuthor.AuthorId).Select(c => new AuthorManuscriptSelectionViewDto
                {
                    AuthorId = c.AuthorId,
                    Name = c.Name,
                }));
            return list;
        }

        public ICommand SendManuscriptCommand => new RelayCommand(execute: SendManuscript);
        private void SendManuscript()
        {
            if (InputManuscriptTitle == "")
            {
                MessageBox.Show($"Please enter a title for the manuscript.",
                    "Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                var messageManuscriptTitle = InputManuscriptTitle;
                var la = new AuthorManuscriptSelectionViewDto()
                {
                    AuthorId = LoggedAuthor.AuthorId,
                    IsSelected = true,
                };
                FrontAuthorList.Add(la);
                var selectedAuthors = FrontAuthorList.Where(c => c.IsSelected);
                foreach (var dto in selectedAuthors)
                {
                    ManuscriptToAdd.AuthorsId.Add(dto.AuthorId);
                }
                FrontAuthorList.RemoveAt(FrontAuthorList.Count - 1);
                ManuscriptToAdd.ManuscriptTitle = InputManuscriptTitle;
                _addMultipleManuscriptService.AddMultipleManuscript(ManuscriptToAdd);
                InputManuscriptTitle = "";

                MessageBox.Show($"Manuscript entitled as '{messageManuscriptTitle}' has been sent and will be reviewed by the editors and peer reviewers as soon as possible, stay tuned!",
                    "Success!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Refresh DB
                _manuscriptService = new ListManuscriptService(_context);
                _notificationService = new ListNotificationService(_context);
                _authorManuscriptService = new ListAuthorManuscriptService(_context);

                var manuscripts = _manuscriptService.GetManuscriptList().ToList();
                var notifications = _notificationService.GetNotificationList().ToList();
                var authorManuscripts = _authorManuscriptService.GetAuthorManuscriptList().ToList();

                ManuscriptList = new ObservableCollection<ManuscriptListDto>(manuscripts);
                NotificationList = new ObservableCollection<NotificationListDto>(notifications);
                AuthorManuscriptList = new ObservableCollection<AuthorManuscriptListDto>(authorManuscripts);

                //Refresh ListBox
                var latestManuscript = ManuscriptList.Last();
                latestManuscript.ManuscriptTitle =
                    LoggedManuscriptList.Count + 1 + ". " + latestManuscript.ManuscriptTitle;
                LoggedManuscriptList.Add(latestManuscript);
            }
        }
        public ICommand LogoutCommand => new RelayCommand(execute: Logout);
        private void Logout()
        {
            MessageBox.Show($"This button is a work in progress, please close the window manually instead.",
                "Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        public ICommand RefreshCommand => new RelayCommand(execute: Refresh);
        private void Refresh()
        {
            MessageBox.Show($"All records are updated!",
                "Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        public ICommand ProfileCommand => new RelayCommand(execute: Profile);
        private void Profile()
        {

            _informationOnProfileWindow = new InformationOnProfileWindow();
            _informationOnProfileWindow.Owner = Application.Current.MainWindow;
            _informationOnProfileWindow.DataContext = LoggedAuthor;
            _informationOnProfileWindow.ShowDialog();

        }
        public ICommand InformationCommand => new RelayCommand(execute: Information);
        private void Information()
        {
            _informationWindow = new InformationWindow();
            _informationWindow.Owner = Application.Current.MainWindow;
            _informationWindow.ShowDialog();
        }
    }
}
