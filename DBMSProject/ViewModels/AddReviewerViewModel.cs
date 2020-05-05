using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ServiceLayer.AreaOfInterestService;
using ServiceLayer.ReviewerService;

namespace DBMSProject.ViewModels
{
    public class AddReviewerViewModel : ObservableObject
    {
        private AddReviewerService _addReviewerService;
        private ListAreaOfInterestService _listAreaOfInterestService;
        private ObservableCollection<AreaOfInterestReviewerSelectionViewDto> _frontAreaOfInterestList;
        private string _inputName;
        private string _inputEmailAddress;
        private string _inputAffiliation;
        private string _inputUsername;
        private string _inputPassword;
        public AddReviewerDto ReviewerToAdd { get; private set; }

        public ObservableCollection<AreaOfInterestReviewerSelectionViewDto> FrontAreaOfInterestList
        {
            get { return _frontAreaOfInterestList; }
            set
            {
                _frontAreaOfInterestList = value;
                RaisePropertyChanged(nameof(FrontAreaOfInterestList));
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


        public AddReviewerViewModel(AddReviewerService addReviewerService, ListAreaOfInterestService listAreaOfInterestService)
        {
            InputName = "";
            InputEmailAddress = "";
            InputAffiliation = "";
            InputUsername = "";
            InputPassword = "";

            _addReviewerService = addReviewerService;
            _listAreaOfInterestService = listAreaOfInterestService;

            ReviewerToAdd = new AddReviewerDto
            {
                Name = "",
                EmailAddress = "",
                Affiliation = "",
                Username = "",
                Password = "",
            };
            FrontAreaOfInterestList = GetAreaOfInterestSelectionList();
        }
        private ObservableCollection<AreaOfInterestReviewerSelectionViewDto> GetAreaOfInterestSelectionList()
        {
            var list = new ObservableCollection<AreaOfInterestReviewerSelectionViewDto>(
                _listAreaOfInterestService.GetAreaOfInterests().ToList().Select(c => new AreaOfInterestReviewerSelectionViewDto
                {
                    AreaOfInterestId = c.AreaOfInterestId,
                    ISCode = c.ISCode,
                    Description = c.Description
                }));
            return list;
        }
        public ICommand SaveReviewerCommand => new RelayCommand(SaveReviewer);
        private void SaveReviewer()
        {
            if (InputName == "" || InputAffiliation == "" || InputEmailAddress == "" || InputPassword == "" ||
                InputPassword == "")
            {
                MessageBox.Show($"Please fill in all fields.",
                    "Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                var reviewerFirstName = InputName.Split(' ').First();
                var selectedAreaOfInterests = FrontAreaOfInterestList.Where(c => c.IsSelected);
                foreach (var dto in selectedAreaOfInterests)
                {
                    ReviewerToAdd.AreaOfInterestsId.Add(dto.AreaOfInterestId);
                }
                ReviewerToAdd.Name = InputName;
                ReviewerToAdd.EmailAddress = InputEmailAddress;
                ReviewerToAdd.Affiliation = InputAffiliation;
                ReviewerToAdd.Username = InputUsername;
                ReviewerToAdd.Password = ComputeSha256Hash(InputPassword);
                _addReviewerService.AddReviewer(ReviewerToAdd);

                InputName = "";
                InputEmailAddress = "";
                InputAffiliation = "";
                InputUsername = "";
                InputPassword = "";
                foreach (var areaOfInterest in FrontAreaOfInterestList)
                {
                    areaOfInterest.IsSelected = false;
                }
                MessageBox.Show($"Thank you for registering, {reviewerFirstName}! May more amazing manuscripts be published with your skills!",
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
