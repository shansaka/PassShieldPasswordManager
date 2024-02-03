using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;

namespace PassShieldPasswordManager;

public class SecurityQuestion : SecurityQuestionModel
{
    private readonly SecurityQuestionRepo _securityQuestionRepo = new();
    private readonly IMapper _mapper = AutoMapperConfiguration.Instance.Mapper;

    
    public async Task<List<SecurityQuestion>> GetList()
    {
        try
        {
            return _mapper.Map<List<SecurityQuestion>>(await _securityQuestionRepo.GetList());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;  
        }
    }
}