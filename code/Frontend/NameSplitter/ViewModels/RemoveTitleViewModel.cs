using NameSplitter.DTOs;
using NameSplitter.Events;
using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace NameSplitter.ViewModels
{
    /// <summary>
    /// This ViewModel handles all button interactions and is used to remove a title
    /// inside the SplitterView on the right hand side
    /// </summary>
    /// <seealso cref="Prism.Mvvm.BindableBase" />
    public class RemoveTitleViewModel: BindableBase
    {
        public DelegateCommand CancleButton { get; set; }
        public DelegateCommand RemoveButton { get; set; }

        /// <summary>
        /// Binding property for the displayed title
        /// </summary>
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

        #region private variables

        private IApiClient _apiClient;
        private IEventAggregator _eventAggregator;
        private RemoveTitleView _removeTitleView;
        private string _title;

        #endregion private variables

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveTitleViewModel"/> class. It also initializes
        /// the Cancle and the RemoveButton as well as the shown title inside the opened view.
        /// </summary>
        /// <param name="apiClient">The API client.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="removeTitleView">The remove title view.</param>
        /// <param name="title">The title.</param>
        public RemoveTitleViewModel( IApiClient apiClient, IEventAggregator eventAggregator, RemoveTitleView removeTitleView, Title title )
        {
            _apiClient = apiClient;
            _removeTitleView = removeTitleView;
            _eventAggregator = eventAggregator;

            Title = $"Sind Sie sicher, dass sie \"{title}\" löschen möchten?";

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