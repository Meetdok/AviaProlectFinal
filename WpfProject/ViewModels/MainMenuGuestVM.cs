using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WebProject.WebModels;
using WpfProject.Pages;
using WpfProject.Tools;
using WpfProject.WebModels;
using WpfProject.Windows;

namespace WpfProject.ViewModels
{
    public class MainMenuGuestVM : BaseTools
    {
        CurrentPageControl currentPageControl;

        public Page CurrentPage
        {
            get => currentPageControl.Page;
        }

        private void CurrentPageControl_PageChanged(object sender, EventArgs e)
        {
            Signal(nameof(CurrentPage));
        }

        public string Title { get; set; }
        public string Dep { get; set; }
        public string Arrive { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;        
        private FlightCompany flightCompanyes;
        public FlightCompany SelectedCompany
        {
            get => flightCompanyes;
            set
            {
                flightCompanyes = value;
                Signal();
            }
        }

        private Airplane airplanees;
        public Airplane SelectedAirplane
        {
            get => airplanees;
            set
            {
                airplanees = value;
                Signal();
            }
        }

        public Flight SelectedItem { get; set; }

        private List<Airplane> airplane;
        private List<Flight> flights;
        public Flight flight { get; set; }
        private List<FlightCompany> flightCompanie;

        public List<Flight> Flight
        {
            get => flights;
            set
            {
                flights = value;

                Signal();
            }
        }

        public List<FlightCompany> FlightCompanie
        {
            get => flightCompanie;
            set
            {
                flightCompanie = value;

                Signal();
            }
        }

        public List<Airplane> Airplane
        {
            get => airplane;
            set
            {
                airplane = value;

                Signal();
            }
        }
        public CommandVM EditFlight { get; set; }

        public CommandVM Save { get; set; }

        public CommandVM DeleteFlight { get; set; }

        public CommandVM nav_airplanes { get; set; }

        public CommandVM nav_companys { get; set; }  

        public CommandVM nav_service { get; set; }


        public CommandVM nav_tickets { get; set; }
       
        public CommandVM nav_companysEmploy { get; set; }

        public CommandVM nav_companysServices { get; set; }

        public CommandVM nav_airplanesEmploy { get; set; }

        public CommandVM nav_usersEmploy { get; set; }

        public CommandVM nav_ticketsUsers { get; set; }

        public MainMenuGuestVM()
        {

            Task.Run(async () =>
            {
                var json = await HttpApi.Post("Flights", "ListFlights", null);
                Flight = HttpApi.Deserialize<List<Flight>>(json);

                var json2 = await HttpApi.Post("Airplanes", "ListAirplanes", null);
                Airplane = HttpApi.Deserialize<List<Airplane>>(json2);

                var json3 = await HttpApi.Post("FlightCompanies", "ListFlightCompanys", null);
                FlightCompanie = HttpApi.Deserialize<List<FlightCompany>>(json3);

            });


            currentPageControl = new CurrentPageControl();
            currentPageControl.PageChanged += CurrentPageControl_PageChanged;



            EditFlight = new CommandVM(async () =>
            {
                if (SelectedItem == null)
                {
                    MessageBox.Show("Ошибка");
                }
                else
                {
                    flight = SelectedItem;
                    new FlightsOrderEdit(flight).Show();
                }
            });

            Save = new CommandVM(async () =>
            {
                try
                {
                    var json = await HttpApi.Post("Flights", "save", new Flight
                    {
                        FlightTitle = Title,
                        FlightCityDeparture = Dep,
                        FlightCityArrival = Arrive,
                        FlightDate = Date,
                        AirplaneId = SelectedAirplane.AirplanesId,
                        FlightCompanyId = SelectedCompany.FlightCompanysId
                    });
                    Flight result = HttpApi.Deserialize<Flight>(json);
                    if (Title != null || Dep != null || Arrive != null || Date != null || SelectedAirplane != null || SelectedCompany != null)
                    { MessageBox.Show("Сохранилось!"); }
                }
                catch
                {
                    MessageBox.Show("Ошибка");
                }
                
            });

            DeleteFlight = new CommandVM(async () =>
            {
                if (SelectedItem == null)
                {
                    MessageBox.Show("Ошибка");
                }
                else
                {
                    var json = await HttpApi.Post("Flights", "delete", SelectedItem.FlightsId);

                    Task.Run(async () =>
                    {
                        var json = await HttpApi.Post("Flights", "ListFlights", null);
                        Flight = HttpApi.Deserialize<List<Flight>>(json);

                        var json2 = await HttpApi.Post("Airplanes", "ListAirplanes", null);
                        Airplane = HttpApi.Deserialize<List<Airplane>>(json2);

                        var json3 = await HttpApi.Post("FlightCompanies", "ListFlightCompanys", null);
                        FlightCompanie = HttpApi.Deserialize<List<FlightCompany>>(json3);
                    });
                }
            });

            nav_airplanes = new CommandVM(() =>
            {
                currentPageControl.SetPage(new ListAirplanes());
            });

            nav_airplanesEmploy = new CommandVM(() =>
            {
                currentPageControl.SetPage(new ListAirplanesEmploy());
            });

            nav_service = new CommandVM(() =>
            {
                currentPageControl.SetPage(new ListService());
            });

            nav_companys = new CommandVM(() =>
            {
                currentPageControl.SetPage(new ListCompanys());
            });

            nav_companysServices = new CommandVM(() =>
            {
                currentPageControl.SetPage(new ListServices());
            });

            nav_companysEmploy = new CommandVM(() =>
            {
                currentPageControl.SetPage(new ListCompanysEmploy());
            });

            nav_tickets = new CommandVM(() =>
            {
                currentPageControl.SetPage(new ListTickets());
            });

            nav_ticketsUsers = new CommandVM(() =>
            {
                currentPageControl.SetPage(new ListTicketsUsers());
            });

            nav_usersEmploy = new CommandVM(() =>
            {
                currentPageControl.SetPage(new ListUsers ());
            });
        }

    }
}
