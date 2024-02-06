using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Utilities;

namespace PassShieldPasswordManager.Services;

public class SecurityQuestion : SecurityQuestions
{
    private readonly SecurityQuestionRepo _securityQuestionRepo;
    private readonly IMapper _mapper;

    public SecurityQuestion()
    {
        _mapper = AutoMapperConfiguration.Instance.Mapper;
        _securityQuestionRepo = new SecurityQuestionRepo();
    }


    public async Task<List<SecurityQuestion>> GetList()
    {
        try
        {
            return _mapper.Map<List<SecurityQuestion>>(await _securityQuestionRepo.GetAll());
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