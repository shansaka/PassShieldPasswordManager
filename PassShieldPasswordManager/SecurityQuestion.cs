using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;

namespace PassShieldPasswordManager;

public class SecurityQuestion : SecurityQuestions
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
    
    public async Task<SecurityQuestion> GetById(int id)
    {
        try
        {
            return _mapper.Map<SecurityQuestion>(await _securityQuestionRepo.GetById(id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;  
        }
    }
}