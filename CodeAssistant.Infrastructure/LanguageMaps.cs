using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeAssistant.Domain.Languages;
using AutoMapper;

namespace CodeAssistant.Infrastructure
{
    static class LanguageMaps
    {
        static LanguageMaps()
        {
            Mapper.CreateMap<Language, LanguageBase>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Extension, o => o.MapFrom(s => s.Extension))
                .ForMember(d => d.Arguments, o => o.MapFrom(s => s.Arguments))
                .ForMember(d => d.Syntax, o => o.MapFrom(s => s.Syntax))
                .ForMember(d => d.Template, o => o.MapFrom(s => s.Template))
                .AfterMap((s, d) => 
                {
                    foreach (var resource in s.Resources)
                    {
                        d.AddResource(resource.IsDefault, resource.Path);
                    }
                });
        }

        public static void MapLanguage(Language languauge, LanguageBase domainModel)
        {
            Mapper.Map(languauge, domainModel);
        }
    }
}
