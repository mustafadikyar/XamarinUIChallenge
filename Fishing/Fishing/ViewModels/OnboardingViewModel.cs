using Fishing.Models;
using MvvmHelpers;

namespace Fishing.ViewModels
{
    public class OnboardingViewModel : BaseViewModel
    {
        private ObservableRangeCollection<OnboardingModel> items;

        public ObservableRangeCollection<OnboardingModel> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public OnboardingViewModel()
        {
            // create our dummy onboarding items
            Items = new ObservableRangeCollection<OnboardingModel>
            {
                new OnboardingModel
                {
                    Title = "What is Lorem Ipsum?",
                    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                    Image  = "OnboardImage1"
                },
                new OnboardingModel
                {
                    Title = "Why do we use it?",
                    Content = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English.",
                    Image  = "OnboardImage2"
                },
                new OnboardingModel
                {
                    Title = "Where can I get some?",
                    Content = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable.",
                    Image  = "OnboardImage3"
                },
            };
        }
    }
}
