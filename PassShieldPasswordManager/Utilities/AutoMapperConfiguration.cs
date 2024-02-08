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
            cfg.CreateMap<UserModel, User>().ReverseMap();
            cfg.CreateMap<UserModel, Admin>().ReverseMap();
            cfg.CreateMap<SecurityQuestionModel, SecurityQuestion>().ReverseMap();
            cfg.CreateMap<CredentialModel, CredentialWebsite>().ReverseMap();
            cfg.CreateMap<CredentialModel, CredentialGame>().ReverseMap();
            cfg.CreateMap<CredentialModel, CredentialDesktopApp>().ReverseMap();
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