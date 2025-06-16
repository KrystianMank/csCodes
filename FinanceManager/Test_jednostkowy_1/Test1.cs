using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinanceManager;
using FinanceManager.Models;
using FinanceManager.Services;
using FinanceManager.ViewModels;
using Moq;
using System.Collections.Generic;

namespace FinanceManager.Tests
{
    [TestClass]
    public class TransactionViewModelTests
    {
        private TransactionViewModel _viewModel;
        private Mock<TransactionRepository> _mockRepository;

        [TestInitialize]
        public void Setup()
        {
            // Utwórz mock dla repozytorium
            _mockRepository = new Mock<TransactionRepository>();
            _mockRepository.Setup(r => r.GetAll()).Returns(new List<Transaction>());

            // Podmień pole _repository w TransactionViewModel używając refleksji
            var viewModel = new TransactionViewModel();
            var repositoryField = typeof(TransactionViewModel).GetField("_repository",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            repositoryField?.SetValue(viewModel, _mockRepository.Object);

            _viewModel = viewModel;
        }

        [TestMethod]
        public void Title_WhenSet_ShouldUpdateAndNotifyPropertyChanged()
        {
            // Arrange
            var propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(TransactionViewModel.Title))
                    propertyChanged = true;
            };

            // Act
            _viewModel.Title = "Test Transaction";

            // Assert
            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("Test Transaction", _viewModel.Title);
        }

        [TestMethod]
        public void Amount_WhenSetToValidValue_ShouldUpdateAndNotifyPropertyChanged()
        {
            // Arrange
            var propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(TransactionViewModel.Amount))
                    propertyChanged = true;
            };

            // Act
            _viewModel.Amount = 100.50m;

            // Assert
            Assert.IsTrue(propertyChanged);
            Assert.AreEqual(100.50m, _viewModel.Amount);
        }

        [TestMethod]
        public void IsIncome_WhenSet_ShouldUpdateAndNotifyPropertyChanged()
        {
            // Arrange
            var propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(TransactionViewModel.IsIncome))
                    propertyChanged = true;
            };

            // Act
            _viewModel.IsIncome = true;

            // Assert
            Assert.IsTrue(propertyChanged);
            Assert.IsTrue(_viewModel.IsIncome);
        }

        [TestMethod]
        public void AddTransaction_WithValidData_ShouldAddToTransactionsCollection()
        {
            // Arrange
            _mockRepository.Setup(r => r.AddTransaction(It.IsAny<Transaction>())).Verifiable();

            _viewModel.Title = "Test Transaction";
            _viewModel.Amount = 100;
            _viewModel.Description = "Test Description";
            _viewModel.Source = "Test Source";
            _viewModel.IsIncome = true;

            // Act
            _viewModel.AddTransactionCommand.Execute(null);

            // Assert
            _mockRepository.Verify(r => r.AddTransaction(It.IsAny<Transaction>()), Times.Once);
            Assert.AreEqual(1, _viewModel.Transactions.Count);
            var addedTransaction = _viewModel.Transactions[0];
            Assert.AreEqual("Test Transaction", addedTransaction.Title);
            Assert.AreEqual(100, addedTransaction.Amount);
            Assert.AreEqual("Test Description", addedTransaction.Description);
            Assert.AreEqual("Test Source", addedTransaction.Source);
            Assert.IsTrue(addedTransaction.IsIncome);
        }

        [TestMethod]
        public void ResetForm_ShouldClearAllProperties()
        {
            // Arrange
            _viewModel.Title = "Test";
            _viewModel.Amount = 100;
            _viewModel.Description = "Test Description";
            _viewModel.Source = "Test Source";
            _viewModel.IsIncome = true;

            var propertiesChanged = new List<string>();
            _viewModel.PropertyChanged += (s, e) => {
                propertiesChanged.Add(e.PropertyName);
            };

            // Act
            _viewModel.ResetFormCommand.Execute(null);

            // Assert
            Assert.IsTrue(propertiesChanged.Contains(nameof(TransactionViewModel.Title)));
            Assert.IsTrue(propertiesChanged.Contains(nameof(TransactionViewModel.Amount)));
            Assert.IsTrue(propertiesChanged.Contains(nameof(TransactionViewModel.Description)));
            Assert.IsTrue(propertiesChanged.Contains(nameof(TransactionViewModel.Source)));
            Assert.IsTrue(propertiesChanged.Contains(nameof(TransactionViewModel.IsIncome)));
            
            Assert.IsNull(_viewModel.Title);
            Assert.AreEqual(0, _viewModel.Amount);
            Assert.IsNull(_viewModel.Description);
            Assert.IsNull(_viewModel.Source);
            Assert.IsFalse(_viewModel.IsIncome);
        }
    }
}