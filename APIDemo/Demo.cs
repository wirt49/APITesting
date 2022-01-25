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

        public IRestResponse GetUsersResponse(string endPoint)
        {
            var user = new Helper<ListOfUsersDTO>();
            var url = user.SetUrl(endPoint);

            var request = user.CreateGetRequest();
            IRestResponse response = user.GetResponse(url, request);
            return response;
        }

        public CreateUserDTO CreateUser(string endPoint, dynamic requestBody)
        {
            var user = new Helper<CreateUserDTO>();
            var url = user.SetUrl(endPoint);

            var jsonRequest = user.Serialize(requestBody);
            var request = user.CreatePostRequest(jsonRequest);
            var response = user.GetResponse(url, request);
            CreateUserDTO content = user.GetContent<CreateUserDTO>(response);

            return content;
        }

        public UpdateUserDTO UpdateUser(string endPoint, dynamic requestBody)
        {
            var user = new Helper<UpdateUserDTO>();
            var url = user.SetUrl(endPoint);

            var request = user.CreatePutRequest(requestBody);
            var response = user.GetResponse(url, request);
            UpdateUserDTO content = user.GetContent<UpdateUserDTO>(response);

            return content;
        }

        public IRestResponse GetUpdateUserResponse(string endPoint, dynamic requestBody)
        {
            var user = new Helper<UpdateUserDTO>();
            var url = user.SetUrl(endPoint);

            var request = user.CreatePutRequest(requestBody);
            IRestResponse response = user.GetResponse(url, request);

            return response;
        }

        public IRestResponse DeleteUser(string endPoint)
        {
            var user = new Helper<ListOfUsersDTO>();
            var url = user.SetUrl(endPoint);

            var request = user.CreateDeleteRequest();
            var response = user.GetResponse(url, request);
            return response;
        }
    }
}