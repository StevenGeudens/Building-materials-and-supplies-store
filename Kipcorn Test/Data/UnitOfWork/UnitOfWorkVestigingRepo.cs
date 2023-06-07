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
    public class UnitOfWorkVestigingRepoTest
    {
        private IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();

        [Test]
        public void Ophalen_ReturnsObservableCollectionVanTypeVestiging()
        {
            // Arrange
            ObservableCollection<Vestiging> Vestigingen;

            // Act
            Vestigingen = new ObservableCollection<Vestiging>(unitOfWork.VestigingRepo.Ophalen());

            // Assert
            Assert.NotNull(Vestigingen);
            Assert.IsInstanceOf<ObservableCollection<Vestiging>>(Vestigingen);
        }
    }
}
