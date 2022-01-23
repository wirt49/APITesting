using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using APIDemo.DTO;

namespace APIDemo
{
    public class Demo<T>
    {
        public ListOfUsersDTO GetUsers(string endPoint)
        {
            var user = new Helper<ListOfUsersDTO>();
            var url = user.SetUrl(endPoint);

            var request = user.CreateGetRequest();
            var response = user.GetResponse(url, request);
            ListOfUsersDTO content = user.GetContent<ListOfUsersDTO>(response);
            return content;
        }

        public CreateUserDTO CreateUser(string endPoint, dynamic requestBody)
        {
            var user = new Helper<CreateUserDTO>();
            var url = user.SetUrl(endPoint);

            var request = user.CreatePostRequest(requestBody);
            var response = user.GetResponse(url, request);
            CreateUserDTO content = user.GetContent<CreateUserDTO>(response);

            return content;
        }
    }
}