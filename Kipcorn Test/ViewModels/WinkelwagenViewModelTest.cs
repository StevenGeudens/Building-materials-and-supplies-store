using models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using wpf.ViewModels;

namespace Kipcorn_Test.ViewModels
{
    [TestFixture]
    public class WinkelwagenViewModelTest
    {
        WinkelwagenViewModel winkelwagenViewModel = new WinkelwagenViewModel();

        [Test]
        public void TotaalPrijsZonderBtwBerekenen_2Artikels_ZetTotaalPrijsZonderBtwNaarResultaat()
        {
            // Arrange
            winkelwagenViewModel.WinkelmandItems = new ObservableCollection<WinkelmandItem>() { 
                new WinkelmandItem() {
                    Aantal = 2,
                    Artikel = new Artikel() { Prijs = 5 }
                },
                new WinkelmandItem() {
                    Aantal = 1,
                    Artikel = new Artikel() { Prijs = 5 }
                }};

            // Act
            winkelwagenViewModel.TotaalPrijsZonderBtwBerekenen();

            // Assert
            Assert.AreEqual(15, winkelwagenViewModel.TotaalPrijsZonderBtw);
        }

        [Test]
        public void TotaalPrijsMetBtwBerekenen_2Artikels_ZetTotaalPrijsZonderBtwNaarResultaat()
        {
            // Arrange
            winkelwagenViewModel.GeselecteerdBtwPercentage = 10;
            winkelwagenViewModel.TotaalPrijsZonderBtw = 100;

            // Act
            winkelwagenViewModel.TotaalPrijsMetBtwBerekenen();

            // Assert
            Assert.AreEqual(110, winkelwagenViewModel.TotaalPrijsMetBtw);
        }
    }
}
