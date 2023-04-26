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
    public class EditCompanyVM : BaseTools
    {
        public CommandVM Save { get; set; }

        private List<Service> services;
        public List<Service> Service
        {
            get => services;
            set
            {
                services = value;

                Signal();
            }
        }
        private Service service;
        public Service SelectedService
        {
            get => service;
            set => service = value;
        }
        public string Title { get; set; }
        public int Cost { get; set; }
        public EditCompanyVM(FlightCompany flightCompany)
        {
            Task.Run(async () =>
            {
                var json = await HttpApi.Post("Services", "ListServices", null);
                Service = HttpApi.Deserialize<List<Service>>(json);
            });


            Save = new CommandVM(async () =>
            {
                try
                {
                    var json2 = await HttpApi.Post("FlightCompanies", "put", new FlightCompany
                    {
                        FlightCompanysId = flightCompany.FlightCompanysId,
                        FlightCompanyName = Title,
                        ServiceId = SelectedService.ServiceId
                    });

                    if (Title != null || SelectedService != null)
                    {
                        { MessageBox.Show("Сохранилось!"); }
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка");
                }
            });
        }
    }
}
