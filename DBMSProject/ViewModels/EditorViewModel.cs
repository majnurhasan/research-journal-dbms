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
using ServiceLayer.EditorService;
using ServiceLayer.IssueService;
using ServiceLayer.ManuscriptService;
using ServiceLayer.ReviewerService;
using ServiceLayer.ReviewService;

namespace DBMSProject.ViewModels
{
    public class EditorViewModel : ObservableObject
    {
        private readonly EfCoreContext _context;
        private EditorListDto _loggedEditor;
        private ListManuscriptService _manuscriptService;
        private ListReviewService _reviewService;
        private ListIssueService _issueService;
        private ListFrontReviewerService _listFrontReviewerService;
        private AddReviewService _addReviewService;
        private UpdateManuscriptService _updateManuscriptService;
        private string _editorFirstName;
        private InformationWindow _informationWindow;
        private InformationOnProfileWindow _informationOnProfileWindow;
        private ManuscriptListDto _selectedReceivedManuscript;
        private ManuscriptListDto _selectedUnderReviewManuscript;
        private ManuscriptListDto _selectedAcceptedManuscript;
        private ReviewListDto _selectedReview;
        private IssueListDto _selectedIssue;
        private ObservableCollection<ReviewerSelectionViewDto> _frontReviewerList;
        private string _inputOrderInIssue;
        private string _inputTotalPagesOccupied;
        private string _inputBeginningPageNumber;
        private ManuscriptListDto _selectedIssueManuscript;
        public ObservableCollection<ManuscriptListDto> ManuscriptList { get; set; }
        public ObservableCollection<ReviewListDto> ReviewList { get; set; }
        public ObservableCollection<IssueListDto> IssueList { get; set; }
        public ObservableCollection<ManuscriptListDto> ReceivedManuscriptList { get; set; } = new ObservableCollection<ManuscriptListDto>();
        public ObservableCollection<ManuscriptListDto> UnderReviewManuscriptList { get; set; } = new ObservableCollection<ManuscriptListDto>();
        public ObservableCollection<ManuscriptListDto> AcceptedManuscriptList { get; set; } = new ObservableCollection<ManuscriptListDto>();
        public ObservableCollection<ManuscriptListDto> SelectedIssueManuscriptList { get; set; } = new ObservableCollection<ManuscriptListDto>();
        public ObservableCollection<ReviewListDto> SelectedManuscriptReviewList { get; set; } = new ObservableCollection<ReviewListDto>();

        public ObservableCollection<ReviewerSelectionViewDto> FrontReviewerList
        {
            get { return _frontReviewerList; }
            set
            {
                _frontReviewerList = value;
                RaisePropertyChanged(nameof(FrontReviewerList));
            }
        }

        public string InputOrderInIssue
        {
            get { return _inputOrderInIssue; }
            set
            {
                _inputOrderInIssue = value;
                RaisePropertyChanged(nameof(InputOrderInIssue));
            }
        }

        public string InputTotalPagesOccupied
        {
            get { return _inputTotalPagesOccupied; }
            set
            {
                _inputTotalPagesOccupied = value;
                RaisePropertyChanged(nameof(InputTotalPagesOccupied));
            }
        }

        public string InputBeginningPageNumber
        {
            get { return _inputBeginningPageNumber; }
            set
            {
                _inputBeginningPageNumber = value;
                RaisePropertyChanged(nameof(InputBeginningPageNumber));
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

        public string EditorFirstName
        {
            get { return _editorFirstName; }
            set
            {
                _editorFirstName = value;
                RaisePropertyChanged(nameof(EditorFirstName));
            }
        }

        public ManuscriptListDto SelectedReceivedManuscript
        {
            get { return _selectedReceivedManuscript; }
            set
            {
                _selectedReceivedManuscript = value;
                RaisePropertyChanged(nameof(SelectedReceivedManuscript));
            }
        }

        public ManuscriptListDto SelectedUnderReviewManuscript
        {
            get { return _selectedUnderReviewManuscript; }
            set
            {
                _selectedUnderReviewManuscript = value;
                RaisePropertyChanged(nameof(SelectedUnderReviewManuscript));
            }
        }

        public ManuscriptListDto SelectedAcceptedManuscript
        {
            get { return _selectedAcceptedManuscript; }
            set
            {
                _selectedAcceptedManuscript = value;
                RaisePropertyChanged(nameof(SelectedAcceptedManuscript));
            }
        }

        public ManuscriptListDto SelectedIssueManuscript
        {
            get { return _selectedIssueManuscript; }
            set
            {
                _selectedIssueManuscript = value;
                RaisePropertyChanged(nameof(SelectedIssueManuscript));
            }
        }

        public ReviewListDto SelectedReview
        {
            get { return _selectedReview; }
            set
            {
                _selectedReview = value;
                RaisePropertyChanged(nameof(SelectedReview));
            }
        }

        public IssueListDto SelectedIssue
        {
            get { return _selectedIssue; }
            set
            {
                _selectedIssue = value;
                RaisePropertyChanged(nameof(SelectedIssue));
            }
        }

        public ManuscriptListDto ManuscriptToUpdate { get; private set; }
        public AddReviewDto ReviewToAdd { get; private set; }
        public EditorViewModel(EfCoreContext context) 
        {
            _context = context;
        }

        public EditorViewModel(EditorListDto loggedEditor, UpdateManuscriptService updateManuscriptService, 
            AddReviewService addReviewService, ListFrontReviewerService listFrontReviewerService) : this(new EfCoreContext())
        {
            LoggedEditor = loggedEditor;
            EditorFirstName = loggedEditor.Name.Split(' ').First() + "!";

            InputOrderInIssue = "";
            InputTotalPagesOccupied = "";
            InputBeginningPageNumber = "";

            _updateManuscriptService = updateManuscriptService;
            _addReviewService = addReviewService;
            _listFrontReviewerService = listFrontReviewerService;

            FrontReviewerList = GetReviewerSelectionList();

            ManuscriptToUpdate = new ManuscriptListDto();
            ReviewToAdd = new AddReviewDto();

            _manuscriptService = new ListManuscriptService(_context);
            _issueService = new ListIssueService(_context);
            _reviewService = new ListReviewService(_context);

            var manuscripts = _manuscriptService.GetManuscriptList().ToList();
            var issues = _issueService.GetIssueList().ToList();
            var reviews = _reviewService.GetReviewList().ToList();

            ManuscriptList = new ObservableCollection<ManuscriptListDto>(manuscripts);
            IssueList = new ObservableCollection<IssueListDto>(issues);
            ReviewList = new ObservableCollection<ReviewListDto>(reviews);

            foreach (var manuscript in ManuscriptList)
            {
                if (manuscript.ManuscriptStatus == 1)
                {
                    ReceivedManuscriptList.Add(manuscript);
                }
                if (manuscript.ManuscriptStatus == 3)
                {
                    foreach (var review in ReviewList)
                    {
                        if (review.ManuscriptId == manuscript.ManuscriptId && review.AppropriatenessScore != 0 && UnderReviewManuscriptList.Contains(manuscript) != true)
                        {
                            UnderReviewManuscriptList.Add(manuscript);
                        }
                    }
                }
                if (manuscript.ManuscriptStatus == 4)
                {
                    AcceptedManuscriptList.Add(manuscript);
                }
            }
        }

        private ObservableCollection<ReviewerSelectionViewDto> GetReviewerSelectionList()
        {
            var list = new ObservableCollection<ReviewerSelectionViewDto>(
                _listFrontReviewerService.GetFrontReviewers().ToList().Select(c => new ReviewerSelectionViewDto
                {
                    ReviewerId = c.ReviewerId,
                    Name = c.Name,
                }));
            return list;
        }
        public ICommand RejectManuscriptCommand => new RelayCommand(execute: RejectManuscript);
        private void RejectManuscript()
        {
            ManuscriptToUpdate.ManuscriptId = SelectedReceivedManuscript.ManuscriptId;
            ManuscriptToUpdate.IssueId = 3;
            ManuscriptToUpdate.DateAccepted = new DateTime(2099, 1, 1);
            ManuscriptToUpdate.ManuscriptStatus = 2;
            ManuscriptToUpdate.NumberOfPagesOccupied = 0;
            ManuscriptToUpdate.OrderInIssue = 0;
            ManuscriptToUpdate.BeginningPageNumber = 0;
            _updateManuscriptService.UpdateManuscript(ManuscriptToUpdate);
            MessageBox.Show($"Manuscript entitled as '{SelectedReceivedManuscript.ManuscriptTitle}' has been rejected.",
                "Success!",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            //Refresh DB
            _manuscriptService = new ListManuscriptService(_context);
            _issueService = new ListIssueService(_context);
            _reviewService = new ListReviewService(_context);

            var manuscripts = _manuscriptService.GetManuscriptList().ToList();
            var issues = _issueService.GetIssueList().ToList();
            var reviews = _reviewService.GetReviewList().ToList();

            ManuscriptList = new ObservableCollection<ManuscriptListDto>(manuscripts);
            IssueList = new ObservableCollection<IssueListDto>(issues);
            ReviewList = new ObservableCollection<ReviewListDto>(reviews);

            //Refresh ListBox
            ReceivedManuscriptList.Remove(SelectedReceivedManuscript);
        }
        public ICommand AddReviewCommand => new RelayCommand(AddReview);
        private void AddReview()
        {
            var selectedReviewers = FrontReviewerList.Where(c => c.IsSelected);
            var reviewerIdBag = new List<int>();
            foreach (var dto in selectedReviewers)
            {
                reviewerIdBag.Add(dto.ReviewerId);
            }
            if (reviewerIdBag.Count >= 3)
            {
                //update manuscript
                ManuscriptToUpdate.ManuscriptId = SelectedReceivedManuscript.ManuscriptId;
                ManuscriptToUpdate.IssueId = 3;
                ManuscriptToUpdate.DateAccepted = new DateTime(2099, 1, 1);
                ManuscriptToUpdate.ManuscriptStatus = 3;
                ManuscriptToUpdate.NumberOfPagesOccupied = 0;
                ManuscriptToUpdate.OrderInIssue = 0;
                ManuscriptToUpdate.BeginningPageNumber = 0;
                _updateManuscriptService.UpdateManuscript(ManuscriptToUpdate);

                //add reviews
                ReviewToAdd.ManuscriptId = SelectedReceivedManuscript.ManuscriptId;
                foreach (var reviewerId in reviewerIdBag)
                {
                    ReviewToAdd.ReviewerId = reviewerId;
                    _addReviewService.AddReview(ReviewToAdd);
                }

                MessageBox.Show($"The manuscript entitled as '{SelectedReceivedManuscript.ManuscriptTitle}' has been sent for peer review.",
                    "Success!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                //Refresh DB
                _manuscriptService = new ListManuscriptService(_context);
                _issueService = new ListIssueService(_context);
                _reviewService = new ListReviewService(_context);

                var manuscripts = _manuscriptService.GetManuscriptList().ToList();
                var issues = _issueService.GetIssueList().ToList();
                var reviews = _reviewService.GetReviewList().ToList();

                ManuscriptList = new ObservableCollection<ManuscriptListDto>(manuscripts);
                IssueList = new ObservableCollection<IssueListDto>(issues);
                ReviewList = new ObservableCollection<ReviewListDto>(reviews);

                //Refresh ListBox
                ReceivedManuscriptList.Remove(SelectedReceivedManuscript);
            }
            else
            {
                MessageBox.Show($"Please select three or more reviewers to peer review.",
                    "Success!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
        public ICommand LoadSelectedManuscriptReviewListCommand => new RelayCommand(LoadSelectedManuscriptReviewList);
        private void LoadSelectedManuscriptReviewList()
        {
            if (SelectedManuscriptReviewList.Count != 0)
            {
                SelectedManuscriptReviewList.Clear();
                foreach (var review in SelectedManuscriptReviewList)
                {
                    SelectedManuscriptReviewList.Remove(review);
                }
            }
            foreach (var review in ReviewList)
            {
                if (SelectedUnderReviewManuscript.ManuscriptId == review.ManuscriptId)
                {
                    SelectedManuscriptReviewList.Add(review);
                }
            }
        }
        public ICommand LoadSelectedIssueManuscriptListCommand => new RelayCommand(LoadSelectedIssueManuscriptList);
        private void LoadSelectedIssueManuscriptList()
        {
            if (SelectedIssueManuscriptList.Count != 0)
            {
                SelectedIssueManuscriptList.Clear();
                foreach (var manuscript in SelectedIssueManuscriptList)
                {
                    SelectedIssueManuscriptList.Remove(manuscript);
                }
            }
            foreach (var manuscript in ManuscriptList)
            {
                if (SelectedIssue.IssueId == manuscript.IssueId)
                {
                    SelectedIssueManuscriptList.Add(manuscript);
                }
            }
        }
        public ICommand RejectManuscriptAgainCommand => new RelayCommand(execute: RejectManuscriptAgain);
        private void RejectManuscriptAgain()
        {
            ManuscriptToUpdate.ManuscriptId = SelectedUnderReviewManuscript.ManuscriptId;
            ManuscriptToUpdate.IssueId = 3;
            ManuscriptToUpdate.DateAccepted = new DateTime(2099, 1, 1);
            ManuscriptToUpdate.ManuscriptStatus = 2;
            ManuscriptToUpdate.NumberOfPagesOccupied = 0;
            ManuscriptToUpdate.OrderInIssue = 0;
            ManuscriptToUpdate.BeginningPageNumber = 0;
            _updateManuscriptService.UpdateManuscript(ManuscriptToUpdate);
            MessageBox.Show($"Manuscript entitled as '{SelectedUnderReviewManuscript.ManuscriptTitle}' has been rejected.",
                "Success!",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            //Refresh DB
            _manuscriptService = new ListManuscriptService(_context);
            _issueService = new ListIssueService(_context);
            _reviewService = new ListReviewService(_context);

            var manuscripts = _manuscriptService.GetManuscriptList().ToList();
            var issues = _issueService.GetIssueList().ToList();
            var reviews = _reviewService.GetReviewList().ToList();

            ManuscriptList = new ObservableCollection<ManuscriptListDto>(manuscripts);
            IssueList = new ObservableCollection<IssueListDto>(issues);
            ReviewList = new ObservableCollection<ReviewListDto>(reviews);

            //Refresh ListBox
            SelectedManuscriptReviewList.Clear();
            foreach (var review in SelectedManuscriptReviewList)
            {
                SelectedManuscriptReviewList.Remove(review);
            }
            UnderReviewManuscriptList.Remove(SelectedUnderReviewManuscript);
        }
        public ICommand AcceptManuscriptCommand => new RelayCommand(execute: AcceptManuscript);
        private void AcceptManuscript()
        {
            ManuscriptToUpdate.ManuscriptId = SelectedUnderReviewManuscript.ManuscriptId;
            ManuscriptToUpdate.IssueId = 3;
            ManuscriptToUpdate.DateAccepted = DateTime.Now;
            ManuscriptToUpdate.ManuscriptStatus = 4;
            ManuscriptToUpdate.NumberOfPagesOccupied = 0;
            ManuscriptToUpdate.OrderInIssue = 0;
            ManuscriptToUpdate.BeginningPageNumber = 0;
            _updateManuscriptService.UpdateManuscript(ManuscriptToUpdate);
            MessageBox.Show($"Manuscript entitled as '{SelectedUnderReviewManuscript.ManuscriptTitle}' has been accepted.",
                "Success!",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            //Refresh DB
            _manuscriptService = new ListManuscriptService(_context);
            _issueService = new ListIssueService(_context);
            _reviewService = new ListReviewService(_context);

            var manuscripts = _manuscriptService.GetManuscriptList().ToList();
            var issues = _issueService.GetIssueList().ToList();
            var reviews = _reviewService.GetReviewList().ToList();

            ManuscriptList = new ObservableCollection<ManuscriptListDto>(manuscripts);
            IssueList = new ObservableCollection<IssueListDto>(issues);
            ReviewList = new ObservableCollection<ReviewListDto>(reviews);

            //Refresh ListBox
            SelectedManuscriptReviewList.Clear();
            foreach (var review in SelectedManuscriptReviewList)
            {
                SelectedManuscriptReviewList.Remove(review);
            }
            AcceptedManuscriptList.Add(SelectedUnderReviewManuscript);
            UnderReviewManuscriptList.Remove(SelectedUnderReviewManuscript);
        }
        public ICommand ScheduleManuscriptCommand => new RelayCommand(execute: ScheduleManuscript);
        private void ScheduleManuscript()
        {
            ManuscriptToUpdate.ManuscriptId = SelectedAcceptedManuscript.ManuscriptId;
            ManuscriptToUpdate.IssueId = 1; // placeholder for now
            ManuscriptToUpdate.ManuscriptStatus = 5;
            ManuscriptToUpdate.NumberOfPagesOccupied = Convert.ToInt32(InputTotalPagesOccupied);
            ManuscriptToUpdate.OrderInIssue = Convert.ToInt32(InputOrderInIssue);
            ManuscriptToUpdate.BeginningPageNumber = Convert.ToInt32(InputBeginningPageNumber);
            _updateManuscriptService.UpdateManuscript(ManuscriptToUpdate);
            MessageBox.Show($"Manuscript entitled as '{SelectedAcceptedManuscript.ManuscriptTitle}' has been schduled.",
                "Success!",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            //Refresh DB
            _manuscriptService = new ListManuscriptService(_context);
            _issueService = new ListIssueService(_context);
            _reviewService = new ListReviewService(_context);

            var manuscripts = _manuscriptService.GetManuscriptList().ToList();
            var issues = _issueService.GetIssueList().ToList();
            var reviews = _reviewService.GetReviewList().ToList();

            ManuscriptList = new ObservableCollection<ManuscriptListDto>(manuscripts);
            IssueList = new ObservableCollection<IssueListDto>(issues);
            ReviewList = new ObservableCollection<ReviewListDto>(reviews);

            //Refresh ListBox
            AcceptedManuscriptList.Remove(SelectedAcceptedManuscript);
            InputOrderInIssue = "";
            InputTotalPagesOccupied = "";
            InputBeginningPageNumber = "";
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
            _informationOnProfileWindow.DataContext = LoggedEditor;
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
