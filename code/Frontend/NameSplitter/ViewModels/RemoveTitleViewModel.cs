using NameSplitter.Events;
using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace NameSplitter.ViewModels
{
    public class RemoveTitleViewModel: BindableBase
    {
        public DelegateCommand CancleButton { get; set; }
        public DelegateCommand RemoveButton { get; set; }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        private IApiClient _apiClient;
        private IEventAggregator _eventAggregator;
        private RemoveTitleView _removeTitleView;
        private string _title;

        public RemoveTitleViewModel( IApiClient apiClient, IEventAggregator eventAggregator, RemoveTitleView removeTitleView, string title )
        {
            _apiClient = apiClient;
            _removeTitleView = removeTitleView;
            _eventAggregator = eventAggregator;

            Title = $"Sind Sie sicher, dass sie {title} löschen möchten?";

            RemoveButton = new DelegateCommand(async () =>
            {
                var response = await _apiClient.RemoveTitle(title);
                if( response )
                {
                    _eventAggregator.GetEvent<UpdateAvailableTitleList>().Publish(title);
                    _removeTitleView.Close();
                }
            });
            CancleButton = new DelegateCommand(() => _removeTitleView.Close());
        }
    }
}