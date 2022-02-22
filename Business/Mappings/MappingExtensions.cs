// This code is under Copyright (C) 2021 of Cegid SAS all right reserved

using AutoMapper;
using Business.Models;
using System.Linq.Expressions;

namespace Business.Mappings
{
    
    internal static class MappingExtensions
    {
        public static IMappingExpression<TSource, TDestination> ForRecordMember<TSource, TDestination, TMember>(
            this IMappingExpression<TSource, TDestination> mapping,
            Expression<Func<TDestination, TMember>> destinationMember,
            Expression<Func<TSource, object?>> sourceMember)
        {
            var memberName = (destinationMember?.Body as MemberExpression)?.Member.Name ?? throw new InvalidOperationException("The expression does not represent the access to a type member");

            return mapping.ForCtorParam(memberName, o => o.MapFrom(sourceMember));
        }
    }
}
