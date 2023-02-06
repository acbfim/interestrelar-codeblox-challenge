using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interestrelar.Domain;
using Interestrelar.Domain.Request;

namespace Interestrelar.Application.Contract
{
    public interface IGeral
    {
     Task<RetornoDto> GetAllCargoAsync(int skip, int take);
     Task<RetornoDto> GetAllExitsByYearAndMonthCargoAsync(int skip, int take, int year, int month);
     Task<RetornoDto> GetAllByYearAndMonthCargoAsync(int skip, int take, int year, int month);
     Task<RetornoDto> GetAllAvaliableYear();
     Task<RetornoDto> GetAllAvaliableMonthsByYear(int year);
    }
}