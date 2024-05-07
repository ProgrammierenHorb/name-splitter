using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Ioc;
using Prism.Regions;
using System.Net.Http;
using System.Windows;

namespace NameSplitter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindowView>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Container.Resolve<IRegionManager>().RegisterViewWithRegion("ContentRegion", typeof(SplitterView));
        }

        /// <summary>
        /// Used to register types with the container that will be used by this application.
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes( IContainerRegistry containerRegistry )
        {
            containerRegistry.RegisterForNavigation<MainWindowView>(nameof(MainWindowView));
            containerRegistry.RegisterForNavigation<SplitterView>(nameof(SplitterView));
            containerRegistry.RegisterSingleton<HttpClient>();
            containerRegistry.Register<IApiClient, ApiClient>();
        }
    }
}