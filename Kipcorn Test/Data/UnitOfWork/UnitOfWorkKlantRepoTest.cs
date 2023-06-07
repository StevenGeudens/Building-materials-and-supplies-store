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
    public class UnitOfWorkKlantRepoTest
    {
        private IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();

        [Test]
        public void Ophalen_ReturnsObservableCollectionVanTypeKlant()
        {
            // Arrange
            ObservableCollection<Klant> Klanten;

            // Act
            Klanten = new ObservableCollection<Klant>(unitOfWork.KlantRepo.Ophalen());

            // Assert
            Assert.NotNull(Klanten);
            Assert.IsInstanceOf<ObservableCollection<Klant>>(Klanten);
        }
    }
}
