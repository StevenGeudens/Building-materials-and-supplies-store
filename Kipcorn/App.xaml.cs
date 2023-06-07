using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using wpf.ViewModels;
using wpf.Views;

namespace Kipcorn
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(
                        CultureInfo.CurrentCulture.IetfLanguageTag)));

            // Zet dit in commentaar voor de end to end test "KlantToevoegenTest"
            var vm = new NavigationViewModel();
            var view = new NavigationView();
            view.DataContext = vm;
            view.Show();

            //// Haal dit uit commentaar voor de end to end test "KlantToevoegenTest"
            //var view = new KlantToevoegenView();
            //var vm = new KlantToevoegenViewModel(view);
            //view.DataContext = vm;
            //view.Show();
        }
    }
}
