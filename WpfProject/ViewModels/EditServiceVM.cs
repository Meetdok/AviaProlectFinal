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
    public class EditServiceVM : BaseTools
    {
        public CommandVM Save { get; set; }

        public string Name { get; set; }
        public int Cost { get; set; }

        public EditServiceVM(Service service)
        {
            try
            {



                Save = new CommandVM(async () =>
                {
                    var json = await HttpApi.Post("Services", "put", new Service
                    {
                        ServiceId = service.ServiceId,
                        ServiceType = Name,
                        ServiceCost = Cost
                    });
                    if (Name != null || Cost != null)

                        MessageBox.Show("Сохранилось!");

                });
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }

        }
    }
}
