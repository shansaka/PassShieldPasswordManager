using AutoMapper;
using PassShieldPasswordManager;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;

// Dependency injection 
IUserRepo userRepo = new UserRepo();
ISecurityQuestionRepo securityQuestionRepo = new SecurityQuestionRepo();
ICredentialRepo credentialRepo = new CredentialRepo();

// Running the User Interface
var ui = new UserInterface(userRepo, securityQuestionRepo, credentialRepo);
await ui.Run();
