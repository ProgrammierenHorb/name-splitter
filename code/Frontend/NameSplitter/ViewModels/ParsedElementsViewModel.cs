using NameSplitter.DTOs;
using NameSplitter.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace NameSplitter.ViewModels
{
    internal class ParsedElementsViewModel: BindableBase
    {
        public DelegateCommand ButtonCancle { get; set; }
        public DelegateCommand ButtonSave { get; set; }

        public ParseResponse ParsedItems
        {
            get { return _parsedElement; }
            set
            {
                _parsedElement = value;
                RaisePropertyChanged(nameof(ParsedItems));
            }
        }

        private IEventAggregator _eventAggregator;
        private ParseResponse _parsedElement;
        private ParsedElements _parsedElementsView;

        public ParsedElementsViewModel( ParsedElements parsedElementsView, ParseResponse parsedElement, IEventAggregator eventAggregator )
        {
            ParsedItems = parsedElement;
            _parsedElementsView = parsedElementsView;
            _eventAggregator = eventAggregator;
            ButtonSave = new DelegateCommand(SaveParsedElementsButtonHandler);
            ButtonCancle = new DelegateCommand(CancleButtonHandler);
        }

        public void CancleButtonHandler() =>
            _parsedElementsView.Close();

        public void SaveParsedElementsButtonHandler()
        {
            //TODO: SplitterViewModel Properties updaten; Api Zugriff, zum Saven

            _parsedElementsView.Close();
        }
    }
}