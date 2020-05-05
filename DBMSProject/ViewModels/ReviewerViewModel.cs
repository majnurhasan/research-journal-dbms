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
using ServiceLayer.ManuscriptService;
using ServiceLayer.ReviewerService;
using ServiceLayer.ReviewService;

namespace DBMSProject.ViewModels
{
    public class ReviewerViewModel : ObservableObject
    {
        private readonly EfCoreContext _context;
        private ReviewerListDto _loggedReviewer;
        private string _reviewerFirstName;
        private UpdateReviewService _updateReviewService;
        private ListManuscriptService _manuscriptService;
        private ListReviewerService _reviewerService;
        private ListReviewService _reviewService;
        private InformationWindow _informationWindow;
        private InformationOnProfileWindow _informationOnProfileWindow;
        private ManuscriptListDto _selectedManuscript;
        private string _inputAppropriatenessScore;
        private string _inputClarityScore;
        private string _inputMethodologyScore;
        private string _inputContributionScore;
        private string _inputRecommendationStatus;
        public ObservableCollection<ReviewerListDto> ReviewerList { get; set; }
        public ObservableCollection<ManuscriptListDto> ManuscriptList { get; set; }
        public ObservableCollection<ReviewListDto> ReviewList { get; set; }
        public ObservableCollection<ManuscriptListDto> LoggedManuscriptList { get; set; } = new ObservableCollection<ManuscriptListDto>();

        public ReviewerListDto LoggedReviewer
        {
            get { return _loggedReviewer; }
            set
            {
                _loggedReviewer = value;
                RaisePropertyChanged(nameof(LoggedReviewer));
            }
        }

        public string ReviewerFirstName
        {
            get { return _reviewerFirstName; }
            set
            {
                _reviewerFirstName = value;
                RaisePropertyChanged(nameof(ReviewerFirstName));
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

        public string InputAppropriatenessScore
        {
            get { return _inputAppropriatenessScore; }
            set
            {
                _inputAppropriatenessScore = value;
                RaisePropertyChanged(nameof(InputAppropriatenessScore));
            }
        }
        public string InputClarityScore
        {
            get { return _inputClarityScore; }
            set
            {
                _inputClarityScore = value;
                RaisePropertyChanged(nameof(InputClarityScore));
            }
        }
        public string InputMethodologyScore
        {
            get { return _inputMethodologyScore; }
            set
            {
                _inputMethodologyScore = value;
                RaisePropertyChanged(nameof(InputMethodologyScore));
            }
        }
        public string InputContributionScore
        {
            get { return _inputContributionScore; }
            set
            {
                _inputContributionScore = value;
                RaisePropertyChanged(nameof(InputContributionScore));
            }
        }
        public string InputRecommendationStatus
        {
            get { return _inputRecommendationStatus; }
            set
            {
                _inputRecommendationStatus = value;
                RaisePropertyChanged(nameof(InputRecommendationStatus));
            }
        }
        public ReviewListDto ReviewToUpdate { get; private set; }
        public ReviewerViewModel(EfCoreContext context)
        {
            _context = context;
        }

        public ReviewerViewModel(ReviewerListDto loggedReviewer, UpdateReviewService updateReviewService) : this(new EfCoreContext())
        {
            LoggedReviewer = loggedReviewer;
            ReviewerFirstName = loggedReviewer.Name.Split(' ').First() + "!";

            InputAppropriatenessScore = "";
            InputClarityScore = "";
            InputMethodologyScore = "";
            InputContributionScore = "";
            InputRecommendationStatus = "";

            _updateReviewService = updateReviewService;

            ReviewToUpdate = new ReviewListDto
            {
                AppropriatenessScore = 0,
                ClarityScore = 0,
                MethodologyScore = 0,
                ContributionScore = 0,
                DateReviewed = DateTime.Now,
            };

            _manuscriptService = new ListManuscriptService(_context);
            _reviewerService = new ListReviewerService(_context);
            _reviewService = new ListReviewService(_context);

            var manuscripts = _manuscriptService.GetManuscriptList().ToList();
            var reviewers = _reviewerService.GetReviewerList().ToList();
            var reviews = _reviewService.GetReviewList().ToList();

            ManuscriptList = new ObservableCollection<ManuscriptListDto>(manuscripts);
            ReviewerList = new ObservableCollection<ReviewerListDto>(reviewers);
            ReviewList = new ObservableCollection<ReviewListDto>(reviews);

            var manuscriptIdBag = new List<int>();
            foreach (var review in ReviewList)
            {
                if (review.ReviewerId == loggedReviewer.ReviewerId && review.AppropriatenessScore == 0)
                {
                    manuscriptIdBag.Add(review.ManuscriptId);
                }
            }

            foreach (var manuscript in manuscriptIdBag)
            {
                foreach (var parentManuscript in ManuscriptList)
                {
                    if (manuscript == parentManuscript.ManuscriptId)
                    {
                        LoggedManuscriptList.Add(parentManuscript);
                    }
                }
            }
            manuscriptIdBag.Clear();
        }

        public ICommand UpdateReviewCommand => new RelayCommand(execute: UpdateReview);
        private void UpdateReview()
        {
            if (InputAppropriatenessScore == "" || InputClarityScore == "" || InputMethodologyScore == "" ||
                InputContributionScore == "" || InputRecommendationStatus == "")
            {
                MessageBox.Show($"Please fill in all ratings.",
                    "Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                ReviewToUpdate.AppropriatenessScore = Convert.ToInt32(InputAppropriatenessScore);
                ReviewToUpdate.ClarityScore = Convert.ToInt32(InputClarityScore);
                ReviewToUpdate.MethodologyScore = Convert.ToInt32(InputMethodologyScore);
                ReviewToUpdate.ContributionScore = Convert.ToInt32(InputContributionScore);
                ReviewToUpdate.RecommendationStatus = Convert.ToBoolean(Convert.ToInt32(InputRecommendationStatus));

                foreach (var manuscript in ManuscriptList)
                {
                    if (manuscript.ManuscriptId == SelectedManuscript.ManuscriptId)
                    {
                        foreach (var review in ReviewList)
                        {
                            if (review.ReviewerId == LoggedReviewer.ReviewerId && review.ManuscriptId == SelectedManuscript.ManuscriptId)
                            {
                                ReviewToUpdate.ReviewId = review.ReviewId;
                            }
                        }
                    }
                }
                _updateReviewService.UpdateReview(ReviewToUpdate);
                InputAppropriatenessScore = "";
                InputClarityScore = "";
                InputMethodologyScore = "";
                InputContributionScore = "";
                InputRecommendationStatus = "";
                MessageBox.Show($"Manuscript entitled as '{SelectedManuscript.ManuscriptTitle}' has been reviewed, thank you for your service!",
                    "Success!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                //Refresh DB
                _manuscriptService = new ListManuscriptService(_context);
                _reviewerService = new ListReviewerService(_context);
                _reviewService = new ListReviewService(_context);

                var manuscripts = _manuscriptService.GetManuscriptList().ToList();
                var reviewers = _reviewerService.GetReviewerList().ToList();
                var reviews = _reviewService.GetReviewList().ToList();

                ManuscriptList = new ObservableCollection<ManuscriptListDto>(manuscripts);
                ReviewerList = new ObservableCollection<ReviewerListDto>(reviewers);
                ReviewList = new ObservableCollection<ReviewListDto>(reviews);

                
                //Refresh ListBox
                LoggedManuscriptList.Remove(SelectedManuscript);
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
            _informationOnProfileWindow.DataContext = LoggedReviewer;
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
