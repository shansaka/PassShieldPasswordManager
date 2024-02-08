using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Services;

public class User 
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int SecurityQuestionId { get; set; }
    public string SecurityAnswer { get; set; }
    public DateTime DateCreated { get; set; }
    
    private readonly Credential _credential;

    public User()
    {
        _credential = new Credential();
    }

    public async Task EditCredential(Credential credential)
    {
        try
        {
            switch (credential)
            {
                case CredentialGame game:
                    await game.Edit();
                    break;
                case CredentialWebsite website:
                    await website.Edit();
                    break;
                case CredentialDesktopApp desktopApp:
                    await desktopApp.Edit();
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task AddCredential(Credential credential)
    {
        try
        {
            credential.User = this;
            switch (credential)
            {
                case CredentialGame game:
                    await game.Add();
                    break;
                case CredentialWebsite website:
                    await website.Add();
                    break;
                case CredentialDesktopApp desktopApp:
                    await desktopApp.Add();
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<List<Credential>> ViewCredentials(SortBy sortBy = SortBy.None, SortOrder sortOrder = SortOrder.None, string search = null)
    {
        var credentialsList = await _credential.GetList(this, search);
        switch (sortBy)
        {
            case SortBy.Username:
                credentialsList = sortOrder == SortOrder.Ascending
                    ? credentialsList.OrderBy(x => x.Username).ToList()
                    : credentialsList.OrderByDescending(x => x.Username).ToList();
                break;
            case SortBy.CreatedDate:
                credentialsList = sortOrder == SortOrder.Ascending
                    ? credentialsList.OrderBy(x => x.CreatedDate).ToList()
                    : credentialsList.OrderByDescending(x => x.CreatedDate).ToList();
                break;
            case SortBy.UpdatedDate:
                credentialsList = sortOrder == SortOrder.Ascending
                    ? credentialsList.OrderBy(x => x.UpdatedDate).ToList()
                    : credentialsList.OrderByDescending(x => x.UpdatedDate).ToList();
                break;
        }

        return credentialsList;
    }

    public async Task DeleteCredential(int credentialId)
    {
        await _credential.Delete(credentialId);
    }
}