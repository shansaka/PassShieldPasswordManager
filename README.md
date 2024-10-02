# Pass Shield - Secure Password Manager

**Pass Shield** is a robust password management solution designed to securely store, manage, and generate credentials for a wide variety of platforms, including websites, desktop applications, and games. The application focuses on simplicity, user security, and flexibility, making it ideal for individual users, families, or businesses that need to manage multiple accounts securely.

<img width="540" alt="image" src="https://github.com/user-attachments/assets/0420f558-23ce-4da9-8291-31b33b601cc0">
<img width="540" alt="image" src="https://github.com/user-attachments/assets/00d532cf-cb3f-49c7-9dc3-aad8861245a7">
<img width="540" alt="image" src="https://github.com/user-attachments/assets/92ca3e47-7e45-49bb-82ea-2c9dd04885f3">

## Key Features:
- **User Registration & Login**: Allows multiple users to securely register and log in, with personal credentials stored separately for each user. Includes a password reset functionality via a security question.
- **Manage Credentials**: Users can add, edit, delete, and view their credentials, which are securely stored with encryption. Users can also search and sort credentials by date, username, or application.
- **Generate Random Passwords**: Pass Shield can generate secure, random passwords with customizable options, including the length and inclusion of special characters, numbers, and uppercase letters.
- **Login Sessions**: Users can stay logged in securely, with a login session that remembers their username, requiring only a password for re-entry.
- **Admin Features**: Admin users have additional control over managing other users and their credentials, making it a useful tool for families or businesses.
- **Encryption**: All passwords are stored securely using SHA-512 encryption for login and a custom encryption algorithm for stored credentials, ensuring maximum security.

## Technology:
- Built with Object-Oriented Programming principles, utilizing core OOP concepts such as inheritance, encapsulation, and polymorphism.
- Follows SOLID design principles to ensure maintainability, scalability, and clean code architecture.
- Implements **Singleton** and **Repository** design patterns for efficient database management and data handling.
- Programming Language: .NET, C#
- Database: SQLite

## Future Improvements:
- Integration of cloud-based storage for syncing passwords across multiple devices.
- Enhanced encryption using advanced algorithms such as AES for stronger security.
- Support for two-factor authentication and email verification for added layers of security.
- Logging and auditing features to track access and changes to user credentials.




## Installation
Please install .NET 6 or higher. 
Go to following URL: https://dotnet.microsoft.com/en-us/download/dotnet

## IDE
This project has been created using Jetbrains Rider IDE. 
Please use Rider or Visual Studio 2022. 

## Default Data
Database has default admin login since you can't register as an admin. But system can make normal users admins. In order to do this system should have atleast one admin,
Default Admin: 
* Username: admin
* Password: admin

