using dal.Data.UnitOfWork;
using dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using wpf.Views;
using models;

namespace wpf.ViewModels
{
    public class NavigationViewModel : BasisViewModel
    {
        private IUnitOfWork _unitOfWork = new UnitOfWork(new KipcornDbContext());

        private ContentControl _windowContent;
        private int? _AantalArtikelsInWinkelwagen;

        public ContentControl WindowContent
        {
            get { return _windowContent; }
            set { _windowContent = value; NotifyPropertyChanged(); }
        }
        public int? AantalArtikelsInWinkelwagen 
        {
            get 
            {
                if (_AantalArtikelsInWinkelwagen == 0) // null == geen badge zichtbaar
                {
                    return null;
                }
                return _AantalArtikelsInWinkelwagen; 
            }
            set { _AantalArtikelsInWinkelwagen = value; }
        }

        public NavigationViewModel()
        {
            UpdateWinkelmand();
            WindowContent = LoadHomeView(); // Laad bij opstart HomeView
        }

        public override string this[string columnName]
        {
            get { return string.Empty; }
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Winkelwagen":
                    WindowContent = LoadWinkelwagenView();
                    break;
                case "Klanten":
                    WindowContent = LoadKlantenView();
                    break;
                case "Stock":
                    WindowContent = LoadStockView();
                    break;
                default:
                    WindowContent = LoadHomeView();
                    break;
            }
        }

        private HomeView LoadHomeView()
        {
            var vm = new HomeViewModel();
            vm.WinkelmandUpdateEvent += UpdateWinkelmand;
            var view = new HomeView();
            view.DataContext = vm;
            return view;
        }
        private WinkelwagenView LoadWinkelwagenView()
        {
            var vm = new WinkelwagenViewModel();
            vm.WinkelmandUpdateEvent += UpdateWinkelmand;
            var view = new WinkelwagenView();
            view.DataContext = vm;
            return view;
        }
        private KlantenView LoadKlantenView()
        {
            var vm = new KlantenViewModel();
            var view = new KlantenView();
            view.DataContext = vm;
            return view;
        }
        private StockView LoadStockView()
        {
            var vm = new StockViewModel();
            var view = new StockView();
            view.DataContext = vm;
            return view;
        }

        private void UpdateWinkelmand()
        {
            int totaalAantalArtikels = 0;
            IEnumerable<WinkelmandItem> winkelmandItems = _unitOfWork.WinkelmandItemRepo.Ophalen();
            if (winkelmandItems.Any())
            {
                foreach (WinkelmandItem winkelmandItem in winkelmandItems)
                {
                    _unitOfWork.WinkelmandItemRepo.Reload(winkelmandItem); // Update het record (wanneer het aantal aangepast wordt (meerdere keren hetzelfde artikel))
                    totaalAantalArtikels += winkelmandItem.Aantal;
                }
            }
            AantalArtikelsInWinkelwagen = totaalAantalArtikels;
        }
    }
}
