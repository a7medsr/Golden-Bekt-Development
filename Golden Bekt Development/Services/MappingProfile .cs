using AutoMapper;
using AutoMapper;
using Golden_Bekt_Development;
using Golden_Bekt_Development.DTOs;
using Golden_Bekt_Development.Models;
using System.Xml;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<programs, ProgramCreateDto>();
        CreateMap<ProgramCreateDto, programs>();

        CreateMap<Annual_report, Annual_reportCreatDto>();
        CreateMap<Annual_reportCreatDto,Annual_report>();

        CreateMap<Association_Member, Association_MemberDto>();
        CreateMap<Association_MemberDto, Association_Member>();

        CreateMap<Association_Minute, Association_MinuteDto>();
        CreateMap<Association_MinuteDto, Association_Minute>();

        CreateMap<Board_of_Director, Board_of_DirectorDto>();
        CreateMap<Board_of_DirectorDto, Board_of_Director>();

        CreateMap<Branche, BrancheDto>();
        CreateMap<BrancheDto, Branche>();


        CreateMap<Commercial_register, Commercial_registerDto>();
        CreateMap<Commercial_registerDto, Commercial_register>();

        CreateMap<Committee, CommitteeDto>();
        CreateMap<CommitteeDto, Committee>();


        CreateMap<Disclosure, DisclosureDto>();
        CreateMap<DisclosureDto, Disclosure>();

        CreateMap<Financial_report, Financial_reportDto>();
        CreateMap<Financial_reportDto, Financial_report>();

        CreateMap<Investment, InvestmentDto>();
        CreateMap<InvestmentDto, Investment>();


        CreateMap<Library, LibraryDto>();
        CreateMap<LibraryDto, Library>();


        CreateMap<Membership, MembershipDto>();
        CreateMap<MembershipDto, Membership>();

        CreateMap<Our_news, Our_newsDto>();
        CreateMap<Our_newsDto, Our_news>();

        CreateMap<Partners_of_Success, Partners_of_SuccessDto>();
        CreateMap<Partners_of_SuccessDto, Partners_of_Success>();

        CreateMap<Policies_and_regulations, Policies_and_regulationsDto>();
        CreateMap<Policies_and_regulationsDto, Policies_and_regulations>();

        CreateMap<said_about_us, said_about_usDto>();
        CreateMap<said_about_usDto, said_about_us>();

        CreateMap<Satisfaction_measurement, Satisfaction_measurementDto>();
        CreateMap<Satisfaction_measurementDto, Satisfaction_measurement>();



        CreateMap<service, serviceDto>();
        CreateMap<serviceDto, service>();

        CreateMap<service, serviceDto>();
        CreateMap<serviceDto, service>();

        CreateMap<Statistics, StatisticsDto>();
        CreateMap<StatisticsDto, Statistics>();


        CreateMap<Strategic_and_operational_objectives, Strategic_and_operational_objectivesDto>();
        CreateMap<Strategic_and_operational_objectivesDto, Strategic_and_operational_objectives>();

        CreateMap<Vacancies, VacanciesDto>();
        CreateMap<VacanciesDto, Vacancies>();















    }
}
