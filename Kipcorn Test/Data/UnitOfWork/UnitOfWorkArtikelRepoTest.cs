using dal.Data.UnitOfWork;
using FakeItEasy;
using models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kipcorn_Test.Data.UnitOfWork
{
    [TestFixture]
    public class UnitOfWorkArtikelRepoTest
    {
        private IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();

        [Test]
        public void Ophalen_ReturnsObservableCollectionVanTypeArtikel()
        {
            // Arrange
            ObservableCollection<Artikel> Artikels;

            // Act
            Artikels = new ObservableCollection<Artikel>(unitOfWork.ArtikelRepo.Ophalen());

            // Assert
            Assert.NotNull( Artikels );
            Assert.IsInstanceOf<ObservableCollection<Artikel>>( Artikels );
        }
    }
}
