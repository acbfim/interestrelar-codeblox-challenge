using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interestrelar.Domain.Request;
using System.Net.Http;
using Newtonsoft.Json;
using RestSharp;
using Interestrelar.Domain;
using Microsoft.Extensions.Logging;

namespace Interestrelar.Repository
{
    public class DataContext
    {
        public static List<Cargo> _Cargo = new List<Cargo>();
        public static List<Freighter> _Freighters = new List<Freighter>();
        public static List<Mineral> _Minerals = new List<Mineral>();
        public static List<ExitRegister> _ExitRegisters = new List<ExitRegister>();
        public static List<FreightersAvailable> _FreightersAvaliable = new List<FreightersAvailable>();

        string _urlBase = "https://fuct-smk186-code-challenge.cblx.com.br";
        private readonly ILogger<DataContext> _logger;

        public DataContext(ILogger<DataContext> logger)
        {
            _logger = logger;

            var rand = new Random();
            int anoAleatorio = rand.Next(2000, 3000);

            this.AddInitialMinerals();
            this.AddInicialFreighter();
            this.AddInicialFreighterAvaliable();


            if (_Cargo.Count == 0)
                _Cargo = this.GetAllDataByYear(anoAleatorio).Result;

            if (_Cargo[0].Exits == null && _Cargo.Count > 0)
                this.AddInitialExitRegister();

        }

        private void AddInicialFreighter()
        {

            _Freighters.Add(new Freighter() { Id = 1, Class = "I", Capacity = 5, CompatibleMinerals = _Minerals.FindAll(x => x.Name == "D") });
            _Freighters.Add(new Freighter() { Id = 2, Class = "II", Capacity = 3, CompatibleMinerals = _Minerals.FindAll(x => x.Name == "A") });
            _Freighters.Add(new Freighter() { Id = 3, Class = "III", Capacity = 2, CompatibleMinerals = _Minerals.FindAll(x => x.Name == "C") });
            _Freighters.Add(new Freighter() { Id = 4, Class = "IV", Capacity = 0.5m, CompatibleMinerals = _Minerals.FindAll(x => x.Name.Equals("B") || x.Name.Equals("C")) });


        }

        private void AddInicialFreighterAvaliable()
        {
            _FreightersAvaliable.Add(new FreightersAvailable() { Id = 1, Amount = 15, Freighter = _Freighters.First(a => a.Class == "I") });
            _FreightersAvaliable.Add(new FreightersAvailable() { Id = 2, Amount = 10, Freighter = _Freighters.First(a => a.Class == "II") });
            _FreightersAvaliable.Add(new FreightersAvailable() { Id = 3, Amount = 3, Freighter = _Freighters.First(a => a.Class == "III") });
            _FreightersAvaliable.Add(new FreightersAvailable() { Id = 4, Amount = 2, Freighter = _Freighters.First(a => a.Class == "IV") });
        }

        private void AddInitialMinerals()
        {
            // Os valores foram convertidos para toneladas

            _Minerals.Add(new Mineral() { Id = 1, Name = "A", Feature = "Inflamável 🔥", Price = (5000.00m) });
            _Minerals.Add(new Mineral() { Id = 2, Name = "B", Feature = "Risco Biológico ☣", Price = 1000000000.00m });
            _Minerals.Add(new Mineral() { Id = 3, Name = "C", Feature = "Refrigerado 🧊", Price = 3000000.00m });
            _Minerals.Add(new Mineral() { Id = 4, Name = "D", Feature = "Sem características especiais", Price = 100000.00m });
        }

        private void AddInitialExitRegister()
        {
            var aux = _Cargo;

            try
            {
                for (int i = 0; i < aux.Count; i++)
                {

                    aux[i].Exits = new List<ExitRegister>();

                    var usedA = this.CalculateTotalFreighter(
                        _Minerals.First(x => x.Name == "A"),
                        _Freighters.First(n => n.CompatibleMinerals.Any(a => a.Name == "A"))
                        , (aux[i].A)
                        );

                    var usedB = this.CalculateTotalFreighter(
                        _Minerals.First(x => x.Name == "B"),
                        _Freighters.First(n => n.CompatibleMinerals.Any(a => a.Name == "B"))
                        , (aux[i].B)
                        );

                    var usedC = this.CalculateTotalFreighter(
                        _Minerals.First(x => x.Name == "C"),
                        _Freighters.First(n => n.CompatibleMinerals.Any(a => a.Name == "C"))
                        , (aux[i].C)
                        );

                    var usedD = this.CalculateTotalFreighter(
                        _Minerals.First(x => x.Name == "D"),
                        _Freighters.First(n => n.CompatibleMinerals.Any(a => a.Name == "D"))
                        , (aux[i].D)
                        );

                    for (int a = 0; a < usedA; a++)
                        if (aux[i].A > 0)
                            aux[i].Exits.Add(CreateRegister(aux[i], "A", (aux[i].A / usedA)));

                    for (int a = 0; a < usedB; a++)
                        if (aux[i].B > 0)
                            aux[i].Exits.Add(CreateRegister(aux[i], "B", (aux[i].B / usedB)));

                    for (int a = 0; a < usedC; a++)
                        if (aux[i].C > 0)
                            aux[i].Exits.Add(CreateRegister(aux[i], "C", (aux[i].C / usedC)));

                    for (int a = 0; a < usedD; a++)
                        if (aux[i].D > 0)
                            aux[i].Exits.Add(CreateRegister(aux[i], "D", (aux[i].D / usedD)));

                    aux[i].Dashboard = this.GenerateDashboad(aux[i]);

                    _Cargo[i] = aux[i];

                }
            }
            catch (Exception ex)
            {
                var error = $"Error AddInitialExitRegister: {ex.Message}";
                _logger.LogError(ex, error);
            }
        }

        private ExitRegister CreateRegister(Cargo cargo, string mineralName, decimal partialCargo)
        {
            var exit = new ExitRegister();

            try
            {
                var freighter = new Freighter();
                var returnRegister = new ReturnRegister();
                var returnCargo = new ReturnCargo();
                var mineral = _Minerals.FirstOrDefault(x => x.Name == mineralName);

                int Id = _ExitRegisters.Count == 0 ? 1 : _ExitRegisters.OrderByDescending(x => x.Id).First().Id++;
                int IdReturn = _ExitRegisters.Count == 0 ? 1 : _ExitRegisters.OrderByDescending(x => x.ReturnRegister.Id).First().Id++;
                int IdReturnCargo = _ExitRegisters.Count == 0 ? 1 : _ExitRegisters.OrderByDescending(x => x.ReturnRegister.ReturnCargo.Id).First().Id++;

                exit.Id = Id;
                exit.ExitDate = new DateTime(cargo.Year, cargo.Month, new Random().Next(01, 28));

                freighter = _Freighters.First(n => n.CompatibleMinerals.Any(a => a.Name == mineralName));

                exit.Freighter = freighter;
                exit.FreighterId = freighter.Id;
                exit.ReturnRegisterId = IdReturn;

                returnRegister.Id = IdReturn;
                returnRegister.ReturnCargoId = IdReturnCargo;
                returnRegister.ReturneDate = exit.ExitDate.AddDays(2).AddHours(new Random().Next(8, 22)).AddMinutes(new Random().Next(1, 59));

                returnCargo.Id = IdReturnCargo;
                returnCargo.Cargo = partialCargo;
                returnCargo.Amount = this.CalculateAmount(_Minerals.First(x => x.Name == mineralName), freighter, partialCargo);
 
                returnCargo.MineralId = mineral.Id;
                returnCargo.Mineral = mineral;

                returnRegister.ReturnCargo = returnCargo;

                exit.ReturnRegister = returnRegister;
            }
            catch (Exception ex)
            {
                var error = $"Error CreateRegister: {ex.Message}";
                _logger.LogError(ex, error);
            }


            _ExitRegisters.Add(exit);

            return exit;
        }

        private decimal CalculateAmount(Mineral mineral, Freighter freighter, decimal cargo)
        {
            return cargo * mineral.Price;
        }

        private int CalculateTotalFreighter(Mineral mineral, Freighter freighter, decimal cargo)
        {
            return (int)Math.Ceiling((cargo / freighter.Capacity));
        }

        private Dashboard GenerateDashboad(Cargo cargo)
        {
            var dashboard = new Dashboard();
            var byMinerals = new List<Dashboard.ByMineral>();
            var byFreighter = new List<Dashboard.ByFreighter>();

            cargo.Exits.ForEach(x =>
            {
                byMinerals.Add(new Dashboard.ByMineral() { Amount = x.ReturnRegister.ReturnCargo.Amount, Mineral = x.ReturnRegister.ReturnCargo.Mineral });
            });

            for (int i = 0; i < cargo.Exits.Count; i++)
            {
                var exit = cargo.Exits[i];
                var c = exit.ReturnRegister.ReturnCargo.Cargo;
                var totalUsed = this.CalculateTotalFreighter(exit.ReturnRegister.ReturnCargo.Mineral, exit.Freighter, c);
                var totalAvaliable = _FreightersAvaliable.First(f => f.Id == exit.Freighter.Id).Amount;
                byFreighter.Add(new Dashboard.ByFreighter()
                {
                    TotalUsed = totalUsed
                ,
                    TotalIdle = totalAvaliable - totalUsed
                    ,
                    Freighter = exit.Freighter
                }
                );
            }

            dashboard.TotalByFreighter = byFreighter;
            dashboard.TotalByMineral = byMinerals;


            return dashboard;
        }


        private async Task<List<Cargo>> GetAllDataByYear(int year)
        {
            var client = new RestClient($"{_urlBase}/minerais");

            for (int i = 1; i <= 12; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    var request = new RestRequest();
                    request.Method = Method.Get;

                    request.AddParameter("mes", i);
                    request.AddParameter("ano", year);
                    request.AddParameter("semana", j);

                    try
                    {
                        var response = await client.ExecuteAsync(request);

                        if (!response.IsSuccessful)
                            throw new Exception(response.ErrorMessage);

                        var resp = JsonConvert.DeserializeObject<Cargo>(response.Content);

                        resp.Year = year;
                        resp.Month = i;
                        resp.Week = j;

                        _Cargo.Add(resp);
                    }
                    catch (Exception ex)
                    {
                        var error = $"Error GetAllDataByYear: {ex.Message}";
                        _logger.LogError(ex, error);
                        return null;
                    }



                }
            }

            return _Cargo;
        }
    }
}