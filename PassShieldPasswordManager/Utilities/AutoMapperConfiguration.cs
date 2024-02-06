using AutoMapper;
using PassShieldPasswordManager.Models;
using PassShieldPasswordManager.Services;

namespace PassShieldPasswordManager.Utilities;

public class AutoMapperConfiguration
{
    private static AutoMapperConfiguration _instance;
    private readonly IMapper _mapper;
    
    private AutoMapperConfiguration()
    {
        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<Users, User>().ReverseMap();
            cfg.CreateMap<Users, Admin>().ReverseMap();
            cfg.CreateMap<SecurityQuestions, SecurityQuestion>().ReverseMap();
            cfg.CreateMap<Credentials, CredentialWebsite>().ReverseMap();
            cfg.CreateMap<Credentials, CredentialGame>().ReverseMap();
            cfg.CreateMap<Credentials, CredentialDesktopApp>().ReverseMap();
        });

        _mapper = config.CreateMapper();
    }

    public static AutoMapperConfiguration Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AutoMapperConfiguration();
            }
            return _instance;
        }
    }

    public IMapper Mapper
    {
        get { return _mapper; }
    }
}