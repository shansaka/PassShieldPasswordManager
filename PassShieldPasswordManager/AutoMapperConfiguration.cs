using AutoMapper;
using Microsoft.Data.Sqlite;
using PassShieldPasswordManager.Models;

namespace PassShieldPasswordManager;

public class AutoMapperConfiguration
{
    private static AutoMapperConfiguration _instance;
    private IMapper _mapper;
    
    private AutoMapperConfiguration()
    {
        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<UserModel, Admin>();
            cfg.CreateMap<UserModel, User>();
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