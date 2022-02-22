// This code is under Copyright (C) 2021 of Cegid SAS all right reserved

using AutoMapper;
using Business.Models;


namespace Business.Mappings.BasicWL
{
    internal class PayloadProfile : Profile
    {
        public PayloadProfile()
        {
            CreateMap<PayloadDataType1, PayloadData>()
               .ForRecordMember(trg => trg.Number, src => src.SomeNumber)
               .ForRecordMember(trg => trg.Format, src => "SomeFormat")
               .ForRecordMember(trg => trg.IssuedAt, src => DateTime.Now)
               .ForRecordMember(trg => trg.NestedObject, src => src.GetNestedObject());

        }
    }
}
