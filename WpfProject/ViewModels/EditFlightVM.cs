using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebProject.WebModels;
using WpfProject.Tools;

namespace WpfProject.ViewModels
{
    public class EditFlightVM : BaseTools
    {
        public CommandVM SaveFlights { get; set; }

        public string Title { get; set; }
        public string Department { get; set; }
        public string Arrival { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        private Airplane airplanes;
        private FlightCompany flightCompanys;

        private List<Flight> flight;

        public List<Flight> Flight
        {
            get => flight;
            set
            {
                flight = value;

                Signal();
            }
        }

        private List<Airplane> airplane;

        public Airplane FlightAirplanes
        {
            get => airplanes;
            set => airplanes = value;
        }

        public FlightCompany FlightCompanys
        {
            get => flightCompanys;
            set => flightCompanys = value;
        }

        public List<Airplane> ListFlightAirplanes
        {
            get => airplane;
            set
            {
                airplane = value;

                Signal();
            }
        }

        private List<FlightCompany> flightCompany;

        public List<FlightCompany> ListFlightsCompanys
        {
            get => flightCompany;
            set
            {
                flightCompany = value;

                Signal();
            }
        }

        public EditFlightVM(Flight flight)
        {

            Task.Run(async () =>
            {
                var json = await HttpApi.Post("Flights", "ListFlights", null);
                Flight = HttpApi.Deserialize<List<Flight>>(json);

                var json2 = await HttpApi.Post("Airplanes", "ListAirplanes", null);
                ListFlightAirplanes = HttpApi.Deserialize<List<Airplane>>(json2);

                var json3 = await HttpApi.Post("FlightCompanies", "ListFlightCompanys", null);
                ListFlightsCompanys = HttpApi.Deserialize<List<FlightCompany>>(json3);

            });


            SaveFlights = new CommandVM(async () =>
            {
                try
                {
                    var json = await HttpApi.Post("Flights", "put", new Flight
                    {
                        FlightsId = flight.FlightsId,
                        AirplaneId = FlightAirplanes.AirplanesId,
                        FlightCompanyId = FlightCompanys.FlightCompanysId,
                        FlightDate = Date,
                        FlightTitle = Title,
                        FlightCityDeparture = Department,
                        FlightCityArrival = Arrival
                    });
                    if (FlightAirplanes != null || FlightCompanys != null || Date != null || Title != null || Department != null || Arrival != null)
                    {
                        MessageBox.Show("Сохранилось!");
                    }
                }
                catch
                {
                    MessageBox.Show("Не вышло");
                }

                Task.Run(async () =>
                {
                    var json = await HttpApi.Post("Flights", "ListFlights", null);
                    Flight = HttpApi.Deserialize<List<Flight>>(json);

                    var json2 = await HttpApi.Post("Airplanes", "ListAirplanes", null);
                    ListFlightAirplanes = HttpApi.Deserialize<List<Airplane>>(json2);

                    var json3 = await HttpApi.Post("FlightCompanies", "ListFlightCompanys", null);
                    ListFlightsCompanys = HttpApi.Deserialize<List<FlightCompany>>(json3);

                });


            });
        }
    }
}
