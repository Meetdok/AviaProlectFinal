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
    public class EditUserVM : BaseTools
    {
        public CommandVM Save { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Patron { get; set; }
        public int Phone { get; set; }
        public string Mail { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }

        private Post postes;
        private Post posts;
        private List<Post> post;
        public List<Post> Post
        {
            get => post;
            set
            {
                post = value;

                Signal();
            }
        }
        public Post SelectedPost
        {
            get => posts;
            set
            {
                posts = value;
                Signal();
            }
        }

        public Post Posts
        {
            get => postes;
            set => postes = value;
        }
        public EditUserVM(User user)
        {

            Task.Run(async () =>
            {
                var json = await HttpApi.Post("Posts", "list", null);
                Post = HttpApi.Deserialize<List<Post>>(json);               
            });

            Save = new CommandVM(async () =>
            {
                try
                {
                    var json3 = await HttpApi.Post("Users", "put", new User
                    {
                        UserId = user.UserId,
                        PostId = SelectedPost.PostId,
                        FirstName = Name,
                        LastName = LastName,
                        PatronomycName = Patron,
                        PhoneNumber = Phone,
                        Mail = Mail,
                        Login = Login,
                        Password = Pass
                    });
                    if(SelectedPost != null || Name != null || LastName != null || Patron != null || Phone != null || Mail != null || Login != null || Pass != null)
                    {
                        MessageBox.Show("Сохранилось!");
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
