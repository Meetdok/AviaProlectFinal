using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebProject.WebModels;
using WpfProject.Tools;
using WpfProject.WebModels;
using WpfProject.Windows;

namespace WpfProject.ViewModels
{
    public class ListCompanysVM : BaseTools
    {
        public FlightCompany SelectedItem { get; set; }
        public string Title { get; set; }

        private Service service;
        public FlightCompany flightCompanyes { get; set; }




        private List<FlightCompany> flightCompany;
        private List<Service> services;
        

        public List<FlightCompany> FlightCompany
        {
            get => flightCompany;
            set
            {
                flightCompany = value;

                Signal();
            }
        }

        public Service SelectedService
        {
            get => service;
            set
            {
                service = value;
                Signal();
            }
        }

        public List<Service> Service
        {
            get => services;
            set
            {
                services = value;

                Signal();
            }
        }

        public CommandVM Edit { get; set; }
        public CommandVM DeleteCompany { get; set; }
        public CommandVM DeleteService { get; set; }
        public CommandVM Save { get; set; }

        public ListCompanysVM()
        {
            Task.Run(async () =>
            {
                var json = await HttpApi.Post("FlightCompanies", "ListFlightCompanys", null);
                FlightCompany = HttpApi.Deserialize<List<FlightCompany>>(json);

                var json2 = await HttpApi.Post("Services", "ListServices", null);
                Service = HttpApi.Deserialize<List<Service>>(json2);
            });

            Edit = new CommandVM(async () =>
            {
                if (SelectedItem == null)
                {
                    MessageBox.Show("Ошибка");
                }
                else
                {
                    flightCompanyes = SelectedItem;
                    new CompanyEdit(flightCompanyes).Show();
                }
            });


            Save = new CommandVM(async () =>
            {
                try
                {
                    var json = await HttpApi.Post("FlightCompanies", "save", new FlightCompany
                    {
                        FlightCompanyName = Title,
                        ServiceId = SelectedService.ServiceId
                    });
                    Airplane result = HttpApi.Deserialize<Airplane>(json);
                    if (Title != null || SelectedService != null)
                    {
                        MessageBox.Show("Сохранилось!");
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка");
                }

                
            });

            DeleteCompany = new CommandVM(async () =>
            {
                var json = await HttpApi.Post("FlightCompanies", "delete", SelectedItem.FlightCompanysId);

                Task.Run(async () =>
                {
                    var json = await HttpApi.Post("FlightCompanies", "ListFlightCompanys", null);
                    FlightCompany = HttpApi.Deserialize<List<FlightCompany>>(json);

                    var json2 = await HttpApi.Post("Services", "ListServices", null);
                    Service = HttpApi.Deserialize<List<Service>>(json2);
                });
            });

            DeleteService = new CommandVM(async () =>
            {
                var json = await HttpApi.Post("FlightCompanies", "delete", SelectedItem.FlightCompanysId);

                Task.Run(async () =>
                {                   
                    var json2 = await HttpApi.Post("Services", "ListServices", null);
                    Service = HttpApi.Deserialize<List<Service>>(json2);
                });
            });

        }
    }
}
