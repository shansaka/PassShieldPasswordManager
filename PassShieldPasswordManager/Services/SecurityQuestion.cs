using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;
using Mapper = PassShieldPasswordManager.Utilities.Mapper;

namespace PassShieldPasswordManager.Services;

public class SecurityQuestion 
{
    public int SecurityQuestionId { get; set; }
    public string Question { get; set; }
    
    private readonly ISecurityQuestionRepo _securityQuestionRepo;
    private readonly Mapper _mapper = new Mapper();
    

    public SecurityQuestion(ISecurityQuestionRepo securityQuestionRepo)
    {
        _securityQuestionRepo = securityQuestionRepo;
    }


    public async Task<List<SecurityQuestion>> GetList()
    {
        try
        {
            var returnList = new List<SecurityQuestion>();
            var list = await _securityQuestionRepo.GetAll();
            foreach (var item in list)
            {
                returnList.Add(_mapper.MapToSecurityQuestion(item, new SecurityQuestion(_securityQuestionRepo)));
            }
            return returnList;
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
            return _mapper.MapToSecurityQuestion(await _securityQuestionRepo.GetById(id), new SecurityQuestion(_securityQuestionRepo));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;  
        }
    }
}