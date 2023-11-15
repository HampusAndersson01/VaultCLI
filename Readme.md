# VaultCLI - Password Manager

## Source Code

### Overview

VaultCLI is a simple password manager command-line application written in C#.

### Features

- Add passwords securely with encryption
- Display all passwords
- Search passwords by account name or alias
- Save and load passwords from a file

### Prerequisites

- .NET 7.0 SDK
- DotNetEnv library
- xUnit library

### Getting Started

1. Clone this repository.
2. Open the project in Visual Studio or your preferred C# IDE.
3. Configure your environment variables by creating a `.env` file. Set `ENCRYPTION_KEY` to a strong encryption key.
4. Build and run the project.

### Usage

1. Display Passwords

   - Show all passwords
   - Search passwords by account or alias

2. Add Password

   - Enter account name
   - Enter password (leave empty to generate a random password)
   - Enter an alias

3. Exit

### Security

- Passwords are encrypted using AES encryption.
- Ensure your `ENCRYPTION_KEY` is secure and kept secret.

## Tests

### Overview

VaultCLI.Tests contains unit tests for the VaultCLI project.

### Running Tests

1. Open the project in Visual Studio or your preferred C# IDE.
2. Build the solution.
3. Run the tests.

### Test Cases

1. **PasswordServiceTests**

   - Test password encryption.
   - Test adding passwords.
   - Test loading passwords from a file.
   - Test saving passwords to a file.

### Note

- Ensure your test environment is configured correctly.
- Review test cases and adapt as needed for your specific implementation.

### Author

Hampus Andersson

### License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
