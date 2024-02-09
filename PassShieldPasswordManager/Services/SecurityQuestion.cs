using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Repos;
using PassShieldPasswordManager.Repos.Interfaces;
using PassShieldPasswordManager.Utilities;
using Mapper = PassShieldPasswordManager.Utilities.Mapper;

namespace PassShieldPasswordManager.Services
{
    public class SecurityQuestion 
    {
        // Properties
        public int SecurityQuestionId { get; set; }
        public string Question { get; set; }
        
        // Dependency
        private readonly ISecurityQuestionRepo _securityQuestionRepo;
        private readonly Mapper _mapper = new ();
        
        // Constructor with repository injection
        public SecurityQuestion(ISecurityQuestionRepo securityQuestionRepo)
        {
            _securityQuestionRepo = securityQuestionRepo;
        }

        // Method to get a list of security questions
        public async Task<List<SecurityQuestion>> GetList()
        {
            try
            {
                var returnList = new List<SecurityQuestion>();
                var list = await _securityQuestionRepo.GetAll();
                foreach (var item in list)
                {
                    // Mapping each security question
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
        
        // Method to get a security question by its ID
        public async Task<SecurityQuestion> GetById(int id)
        {
            try
            {
                // Mapping the security question
                return _mapper.MapToSecurityQuestion(await _securityQuestionRepo.GetById(id), new SecurityQuestion(_securityQuestionRepo));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;  
            }
        }
    }
}
