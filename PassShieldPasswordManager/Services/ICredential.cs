namespace PassShieldPasswordManager.Services;

public interface ICredential
{
    public Task Add();
    public Task Edit();
}