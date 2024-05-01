using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Ioc;
using Prism.Regions;
using System.Diagnostics;
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
            System.Diagnostics.Process clientProcess = new Process();
            string jarPath = "";
            string argumentsFortheJarFile = "";
            clientProcess.StartInfo.FileName = "java";
            clientProcess.StartInfo.Arguments = @"-jar " + jarPath + " " + argumentsFortheJarFile;
            clientProcess.Start();
        }

        protected override void RegisterTypes( IContainerRegistry containerRegistry )
        {
            containerRegistry.RegisterForNavigation<MainWindowView>(nameof(MainWindowView));
            containerRegistry.RegisterForNavigation<SplitterView>(nameof(SplitterView));
            containerRegistry.Register<IApiClient, ApiClient>();
        }
    }
}