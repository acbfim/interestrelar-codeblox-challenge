using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interestrelar.Application.Contract;
using Interestrelar.Domain;
using Interestrelar.Domain.Request;
using Interestrelar.Repository;
using Microsoft.Extensions.Logging;

namespace Interestrelar.Application
{
    public class Geral : IGeral
    {

        private readonly ILogger<Geral> _logger;

        public Geral(ILogger<Geral> logger)
        {
            _logger = logger;
        }
        public async Task<RetornoDto> GetAllCargoAsync(int skip, int take)
        {

            var response = DataContext._Cargo;

            var newSkip = skip == 0 ? 0 : (skip * take);

            var responseFiltered = response.Skip(newSkip).Take(take).ToList();

            return RetornoDto.objectsFoundSuccess(responseFiltered, skip, take, response.Count);
        }

        public async Task<RetornoDto> GetAllExitsByYearAndMonthCargoAsync(int skip, int take, int year, int month)
        {

            var response = DataContext._Cargo;

            var total = response.Where(x => x.Exits.Any(a => a.ExitDate.Month == month && a.ExitDate.Year == year)).SelectMany(x => x.Exits).ToList().Count;

            var newSkip = skip == 0 ? 0 : (skip * take);

            var res = response.Where(x => x.Exits.Any(a => a.ExitDate.Month == month && a.ExitDate.Year == year))
                .SelectMany(x => x.Exits)
                .Skip(newSkip)
                .Take(take)
                .ToList();

            return RetornoDto.objectsFoundSuccess(res, skip, take, total);
        }

        public async Task<RetornoDto> GetAllByYearAndMonthCargoAsync(int skip, int take, int year, int month)
        {

            var response = DataContext._Cargo;

            var total = response.Where(x => x.Exits.Any(a => a.ExitDate.Month == month && a.ExitDate.Year == year)).SelectMany(x => x.Exits).ToList().Count;

            var newSkip = skip == 0 ? 0 : (skip * take);

            var res = response.Where(x => x.Exits.Any(a => a.ExitDate.Month == month && a.ExitDate.Year == year))
                .Select(x => x.Dashboard)
                .Skip(newSkip)
                .Take(take)
                .ToList();

            return RetornoDto.objectsFoundSuccess(res, skip, take, total);
        }

        public async Task<RetornoDto> GetAllAvaliableYear()
        {

            var response = DataContext._Cargo;

            HashSet<long> years = new HashSet<long>();


            try
            {
                for (int i = 0; i < response.Count; i++)
                {
                    for (int j = 0; j < response[i].Exits.Count; j++)
                    {

                        if (response[i].Exits[j].ExitDate.Year != 1 && response[i].Exits[j].ExitDate != null)
                            years.Add(response[i].Exits[j].ExitDate.Year);
                    }
                }
            }
            catch (Exception ex)
            {
                var error = $"Error GetAllAvaliableYear: {ex.Message}";
                _logger.LogError(ex, error);
                return null;
            }

            return RetornoDto.objectFoundSuccess(years);
        }

        public async Task<RetornoDto> GetAllAvaliableMonthsByYear(int year)
        {

            var response = DataContext._Cargo;

            HashSet<int> months = new HashSet<int>();


            for (int i = 0; i < response.Count; i++)
            {
                for (int j = 0; j < response[i].Exits.Count; j++)
                {
                    var y = response[i].Exits[j].ExitDate;

                    if (y.Year == year)
                        months.Add(y.Month);
                }
            }

            return RetornoDto.objectFoundSuccess(months);
        }
    }
}