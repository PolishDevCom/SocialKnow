﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Application.Common.Mapping
{
    public interface IMapTo<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T));
    }
}
