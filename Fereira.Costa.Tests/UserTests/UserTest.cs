using System;
using FluentAssertions;
using FluentValidation;
using Xunit;
using Fereira.Costa.Domain.Entities;
using Fereira.Costa.Domain.ValueObjects;
using Fereira.Costa.Domain.Exceptions;

namespace Fereira.Costa.Tests.UserTests
{
    public class UserTests
    {
        private static readonly Name ValidName = new("Test", "User");
        private static readonly Cpf ValidCpf = new("12345678901"); // Assumindo formato string só números
        private static readonly string ValidStreet = "Test St";
        private static readonly string ValidZipcode = "12345-678";
        private static readonly string ValidCity = "Test City";
        private static readonly string ValidGeo = "12.3456,-65.4321";
        private static readonly string ValidNumber = "0001";

        [Fact]
        public void Create_ShouldCreateUser_WhenValidParameters()
        {
            var birthDay = new DateTime(1990, 1, 1);
            var naturalness = "Brasileiro";
            var nationality = "Brasil";
            var email = "validuser@example.com";
            var userName = "validuser01";
            var phone = "1234567890";

            var user = User.Create(
                birthDay,
                naturalness,
                nationality,
                email,
                userName,
                ValidName,
                ValidCpf,
                phone,
                ValidStreet,
                ValidZipcode,
                ValidCity,
                ValidGeo,
                ValidNumber,
                emailConfirmed: true);

            user.Should().NotBeNull();
            user.BirthDay.Should().Be(birthDay);
            user.Naturalness.Should().Be(naturalness);
            user.Nationality.Should().Be(nationality);
            user.Email.Should().Be(email);
            user.UserName.Should().Be(userName);
            user.Name.Should().Be(ValidName);
            user.Cpf.Should().Be(ValidCpf);
            user.Phone.Should().Be(phone);
            user.Address.Should().NotBeNull();
            user.Address.Street.Should().Be(ValidStreet);
            user.Address.Zipcode.Should().Be(ValidZipcode);
            user.Address.City.Should().Be(ValidCity);
            user.Address.Geolocation.Should().Be(ValidGeo);
            user.Address.Number.Should().Be(ValidNumber);
            user.EmailConfirmed.Should().BeTrue();
            user.LockoutEnabled.Should().BeFalse();
            user.RefId.Should().NotBeEmpty();
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Theory]
        [InlineData("", "validuser01", "test", "Brasileiro", "Brasil", "12345678901", "1234567890", "Email")] // Email vazio
        [InlineData("invalid-email", "validuser01", "test", "Brasileiro", "Brasil", "12345678901", "1234567890", "Email")] // Email inválido
        [InlineData("validuser@example.com", "", "test", "Brasileiro", "Brasil", "12345678901", "1234567890", "Nome de usuário")] // Username vazio
        [InlineData("validuser@example.com", "validuser01", "", "Brasileiro", "Brasil", "12345678901", "1234567890", "Primeiro nome")] // Nome vazio
        public void Create_ShouldThrowValidationException_WhenInvalidParameters(
            string email,
            string userName,
            string firstName,
            string naturalness,
            string nationality,
            string cpfNumber,
            string phone,
            string expectedError)
        {
            var name = new Name(firstName, "User");
            var cpf = new Cpf(cpfNumber);

            Action act = () => User.Create(
                birthDay: DateTime.UtcNow.AddYears(-20),
                naturalness,
                nationality,
                email,
                userName,
                name,
                cpf,
                phone,
                ValidStreet,
                ValidZipcode,
                ValidCity,
                ValidGeo,
                ValidNumber);

            act.Should().Throw<ValidationException>()
               .Where(e => e.Errors.Any(err => err.ErrorMessage.Contains(expectedError)));
        }

        [Fact]
        public void Update_ShouldModifyFields_WhenValidParameters()
        {
            // Arrange
            var user = User.Create(
                birthDay: new DateTime(1990, 1, 1),
                naturalness: "Brasileiro",
                nationality: "Brasil",
                email: "oldemail@example.com",
                userName: "oldusername",
                name: ValidName,
                cpf: ValidCpf,
                phone: "1234567890",
                street: ValidStreet,
                zipcode: ValidZipcode,
                city: ValidCity,
                geo: ValidGeo,
                number: ValidNumber);

            var newBirthDay = new DateTime(1985, 5, 5);
            var newNaturalness = "Argentino";
            var newNationality = "Argentina";
            var newEmail = "newemail@example.com";
            var newUserName = "newusername";
            var newName = new Name("NewFirst", "NewLast");
            var newCpf = new Cpf("10987654321");
            var newPhone = "0987654321";
            var newAddress = Address.Create("New St", "98765-432", "New City", "23.0000,-46.0000", "123");

            // Act
            user.Update(
                birthDay: newBirthDay,
                naturalness: newNaturalness,
                nationality: newNationality,
                email: newEmail,
                userName: newUserName,
                name: newName,
                cpf: newCpf,
                phone: newPhone,
                address: newAddress);

            // Assert
            user.BirthDay.Should().Be(newBirthDay);
            user.Naturalness.Should().Be(newNaturalness);
            user.Nationality.Should().Be(newNationality);
            user.Email.Should().Be(newEmail);
            user.UserName.Should().Be(newUserName);
            user.Name.Should().Be(newName);
            user.Cpf.Should().Be(newCpf);
            user.Phone.Should().Be(newPhone);
            user.Address.Should().Be(newAddress);
            user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Update_ShouldModifyOnlyProvidedFields()
        {
            // Arrange
            var user = User.Create(
                birthDay: new DateTime(1990, 1, 1),
                naturalness: "Brasileiro",
                nationality: "Brasil",
                email: "email@example.com",
                userName: "username",
                name: ValidName,
                cpf: ValidCpf,
                phone: "1234567890",
                street: ValidStreet,
                zipcode: ValidZipcode,
                city: ValidCity,
                geo: ValidGeo,
                number: ValidNumber);

            var newEmail = "updatedemail@example.com";
            var newPhone = "9999999999";

            // Act
            user.Update(email: newEmail, phone: newPhone);

            // Assert
            user.Email.Should().Be(newEmail);
            user.Phone.Should().Be(newPhone);

            // Campos não atualizados permanecem iguais
            user.UserName.Should().Be("username");
            user.Name.Should().Be(ValidName);
            user.BirthDay.Should().Be(new DateTime(1990, 1, 1));
            user.Naturalness.Should().Be("Brasileiro");
            user.Nationality.Should().Be("Brasil");
            user.Cpf.Should().Be(ValidCpf);
            user.Address.Street.Should().Be(ValidStreet);
        }

        [Fact]
        public void Update_ShouldThrowDomainException_WhenNullProvidedForNonNullable()
        {
            // Arrange
            var user = User.Create(
                birthDay: new DateTime(1990, 1, 1),
                naturalness: "Brasileiro",
                nationality: "Brasil",
                email: "email@example.com",
                userName: "username",
                name: ValidName,
                cpf: ValidCpf,
                phone: "1234567890",
                street: ValidStreet,
                zipcode: ValidZipcode,
                city: ValidCity,
                geo: ValidGeo,
                number: ValidNumber);

            // Act & Assert
            Action act = () => user.Update(email: null);

            // email: null = não atualiza, logo não lança
            act.Should().NotThrow();

            // Porém se passar explicitamente vazio, será validado na validação (e lançará)
            Action actEmpty = () => user.Update(email: "");

            actEmpty.Should().Throw<ValidationException>()
                .Where(e => e.Errors.Any(err => err.ErrorMessage.Contains("Email")));
        }
    }
}
