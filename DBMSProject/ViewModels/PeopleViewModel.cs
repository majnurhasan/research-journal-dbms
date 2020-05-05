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
using DataLayer.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ServiceLayer.AreaOfInterestService;
using ServiceLayer.AuthorService;
using ServiceLayer.EditorService;
using ServiceLayer.ManuscriptService;
using ServiceLayer.ReviewerService;
using ServiceLayer.ReviewService;

namespace DBMSProject.ViewModels
{
    public class PeopleViewModel : ObservableObject
    {
        private readonly EfCoreContext _context;
        private ListAuthorService _authorService;
        private ListEditorService _editorService;
        private ListReviewerService _reviewerService;
        private string _inputUsername;
        private string _inputPassword;
        private EditorListDto _loggedEditor;
        private AuthorListDto _loggedAuthor;
        private ReviewerListDto _loggedReviewer;
        private AuthorViewWindow _authorViewWindow;
        private EditorViewWindow _editorViewWindow;
        private ReviewerViewWindow _reviewerViewWindow;
        private CreateAuthorWindow _createAuthorWindow;
        private CreateReviewerWindow _createReviewerWindow;

        public ObservableCollection<AuthorListDto> AuthorList { get; set; }
        public ObservableCollection<EditorListDto> EditorList { get; set; }
        public ObservableCollection<ReviewerListDto> ReviewerList { get; set; }

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

        public EditorListDto LoggedEditor
        {
            get { return _loggedEditor; }
            set
            {
                _loggedEditor = value;
                RaisePropertyChanged(nameof(LoggedEditor));
            }
        }

        public AuthorListDto LoggedAuthor
        {
            get { return _loggedAuthor; }
            set
            {
                _loggedAuthor = value;
                RaisePropertyChanged(nameof(LoggedAuthor));
            }
        }

        public ReviewerListDto LoggedReviewer
        {
            get { return _loggedReviewer; }
            set
            {
                _loggedReviewer = value;
                RaisePropertyChanged(nameof(LoggedReviewer));
            }
        }

        public PeopleViewModel(EfCoreContext context)
        {
            _context = context;
        }

        public PeopleViewModel() : this(new EfCoreContext())
        {
            _authorService = new ListAuthorService(_context);
            _editorService = new ListEditorService(_context);
            _reviewerService = new ListReviewerService(_context);

            var authors = _authorService.GetAuthorList().ToList();
            var editors = _editorService.GetEditorList().ToList();
            var reviewers = _reviewerService.GetReviewerList().ToList();

            AuthorList = new ObservableCollection<AuthorListDto>(authors);
            EditorList = new ObservableCollection<EditorListDto>(editors);
            ReviewerList = new ObservableCollection<ReviewerListDto>(reviewers);
        }

        //Button Commands
        public ICommand LoginCommand => new RelayCommand(execute: Login);
        private void Login()
        {
            bool accountFound = false;
            foreach (var editor in EditorList)
            {
                if (editor.Username == InputUsername && editor.Password == ComputeSha256Hash(InputPassword))
                {
                    accountFound = true;
                    LoggedEditor = editor;
                    break;
                }
            }
            foreach (var reviewer in ReviewerList)
            {
                if (reviewer.Username == InputUsername && reviewer.Password == ComputeSha256Hash(InputPassword))
                {
                    accountFound = true;
                    LoggedReviewer = reviewer;
                    break;
                }
            }
            foreach (var author in AuthorList)
            {
                if (author.Username == InputUsername && author.Password == ComputeSha256Hash(InputPassword))
                {
                    accountFound = true;
                    LoggedAuthor = author;
                    break;
                }
            }
            if (accountFound)
            {
                if (LoggedAuthor != null)
                {
                    InputPassword = "";
                    InputUsername = "";
                    _authorViewWindow = new AuthorViewWindow();
                    _authorViewWindow.Owner = Application.Current.MainWindow;
                    _authorViewWindow.DataContext = new AuthorViewModel(LoggedAuthor, new AddMultipleManuscriptService(_context), new ListFrontAuthorService(_context));
                    _authorViewWindow.ShowDialog();
                    LoggedAuthor = null;
                }
                else if (LoggedEditor != null)
                {
                    InputPassword = "";
                    InputUsername = "";
                    _editorViewWindow = new EditorViewWindow();
                    _editorViewWindow.Owner = Application.Current.MainWindow;
                    _editorViewWindow.DataContext = new EditorViewModel(LoggedEditor, new UpdateManuscriptService(_context), new AddReviewService(_context), new ListFrontReviewerService(_context));
                    _editorViewWindow.ShowDialog();
                    LoggedEditor = null;
                }
                else if (LoggedReviewer != null)
                {
                    InputPassword = "";
                    InputUsername = "";
                    _reviewerViewWindow = new ReviewerViewWindow();
                    _reviewerViewWindow.Owner = Application.Current.MainWindow;
                    _reviewerViewWindow.DataContext = new ReviewerViewModel(LoggedReviewer, new UpdateReviewService(_context));
                    _reviewerViewWindow.ShowDialog();
                    LoggedReviewer = null;
                }
            }
            else
            {
                MessageBox.Show($"Account does not exist or inputted password is wrong.",
                    "Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public ICommand GoToCreateAuthorWindowCommand => new RelayCommand(execute: GoToCreateAuthorWindow);

        private void GoToCreateAuthorWindow()
        {
            _createAuthorWindow = new CreateAuthorWindow();
            _createAuthorWindow.Owner = Application.Current.MainWindow;
            _createAuthorWindow.DataContext = new AddAuthorViewModel(new AddAuthorService(_context), new AddSingleManuscriptService(_context));
            _createAuthorWindow.ShowDialog();
            //Refresh DB
            _authorService = new ListAuthorService(_context);
            _editorService = new ListEditorService(_context);
            _reviewerService = new ListReviewerService(_context);

            var authors = _authorService.GetAuthorList().ToList();
            var editors = _editorService.GetEditorList().ToList();
            var reviewers = _reviewerService.GetReviewerList().ToList();

            AuthorList = new ObservableCollection<AuthorListDto>(authors);
            EditorList = new ObservableCollection<EditorListDto>(editors);
            ReviewerList = new ObservableCollection<ReviewerListDto>(reviewers);
        }
        public ICommand GoToCreateReviewerWindowCommand => new RelayCommand(execute: GoToCreateReviewerWindow);

        private void GoToCreateReviewerWindow()
        {
            _createReviewerWindow = new CreateReviewerWindow();
            _createReviewerWindow.Owner = Application.Current.MainWindow;
            _createReviewerWindow.DataContext = new AddReviewerViewModel(new AddReviewerService(_context), new ListAreaOfInterestService(_context));
            _createReviewerWindow.ShowDialog();
            //Refresh DB
            _authorService = new ListAuthorService(_context);
            _editorService = new ListEditorService(_context);
            _reviewerService = new ListReviewerService(_context);

            var authors = _authorService.GetAuthorList().ToList();
            var editors = _editorService.GetEditorList().ToList();
            var reviewers = _reviewerService.GetReviewerList().ToList();

            AuthorList = new ObservableCollection<AuthorListDto>(authors);
            EditorList = new ObservableCollection<EditorListDto>(editors);
            ReviewerList = new ObservableCollection<ReviewerListDto>(reviewers);
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
