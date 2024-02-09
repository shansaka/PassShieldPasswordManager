using PassShieldPasswordManager;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;

// Creating instances of repositories
IUserRepo userRepo = new UserRepo(); 
ISecurityQuestionRepo securityQuestionRepo = new SecurityQuestionRepo(); 
ICredentialRepo credentialRepo = new CredentialRepo(); 

// Creating an instance of the user interface
var ui = new UserInterface(userRepo, securityQuestionRepo, credentialRepo);

// Running the user interface
await ui.Run();