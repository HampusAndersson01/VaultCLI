using System;
using System.IO;
using Xunit;

namespace VaultCLITests
{
    public class PasswordServiceTests
    {
        private const string GlobalEncryptionKey = "umvIYIjveplpc6bAj1umvOHkKZCCPFiv";

        [Fact]
        public void AddPassword_ShouldEncryptPassword()
        {
            // Arrange
            var passwordService = new PasswordService(GlobalEncryptionKey);
            var passwordModel = new PasswordModel
            {
                AccountName = "TestAccount",
                Password = "TestPassword",
                Alias = "TestAlias"
            };

            // Act
            passwordService.AddPassword(passwordModel);

            // Assert
            Assert.NotEqual("TestPassword", passwordModel.Password);
        }

        [Fact]
        public void Encrypt_ShouldEncryptText()
        {
            // Arrange
            var passwordService = new PasswordService(GlobalEncryptionKey);
            var textToEncrypt = "TestText";

            // Act
            var encryptedText = passwordService.Encrypt(textToEncrypt);

            // Assert
            Assert.NotEqual(textToEncrypt, encryptedText);
        }

        [Fact]
        public void LoadPasswordsFromFile_ShouldLoadPasswords()
        {
            // Arrange
            var passwordService = new PasswordService(GlobalEncryptionKey);
            var filePath = "testPasswords.txt";

            // Create a test file with passwords
            File.WriteAllLines(filePath, new[]
            {
                "TestAccount1,EncryptedPassword1,TestAlias1",
                "TestAccount2,EncryptedPassword2,TestAlias2",
            });

            // Act
            passwordService.LoadPasswordsFromFile(filePath);

            // Assert
            Assert.Equal(2, passwordService.Count()); // Adjust the expected count based on the number of test passwords
        }

        [Fact]
        public void SavePasswordsToFile_ShouldSavePasswords()
        {
            // Arrange
            var passwordService = new PasswordService(GlobalEncryptionKey);
            var filePath = "testPasswords.txt";

            // Add test passwords
            passwordService.AddPassword(new PasswordModel { AccountName = "TestAccount1", Password = "TestPassword1", Alias = "TestAlias1" });
            passwordService.AddPassword(new PasswordModel { AccountName = "TestAccount2", Password = "TestPassword2", Alias = "TestAlias2" });
            // Add more test passwords as needed

            // Act
            passwordService.SavePasswordsToFile(filePath);

            // Assert
            Assert.True(File.Exists(filePath));

            // Clean up: Delete the test file
            File.Delete(filePath);
        }
    }
}
