using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.WebPages;
using Bank.BusinessLogic;
using Bank.Commons.Concretes.Helpers;
using Bank.Models.Concretes;
using BankAppWebApi.Models;
using BankAppWebApi.Results;
using Microsoft.Ajax.Utilities;

namespace BankAppWebApi.Controllers
{
    public class CustomerController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            using (var customerBusiness = new CustomersBusiness())
            {
                // Get customers from business layer (Core App)
                List<Customers> customers = customerBusiness.SelectAllCustomers();

                // Prepare a content
                var content = new ResponseContent<Customers>(customers);

                // Return content as a json and proper http response
                return new StandartResult<Customers>(content, Request);
            }
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            ResponseContent<Customers> content;

            using (var customerBusiness = new CustomersBusiness())
            {
                // Get customer from business layer (Core App)
                List<Customers> customers = null;
                try
                {
                    var c = customerBusiness.SelectCustomerById(id);
                    if (c != null)
                    {
                        customers = new List<Customers>();
                        customers.Add(c);
                    }

                    // Prepare a content
                    content = new ResponseContent<Customers>(customers);

                    // Return content as a json and proper http response
                    return new XmlResult<Customers>(content, Request);
                }
                catch (Exception)
                {
                    // Prepare a content
                    content = new ResponseContent<Customers>(null);
                    return new XmlResult<Customers>(content, Request);
                }
            }
        }

        // POST api/<controller>
        public IHttpActionResult Post([FromBody]Customers customer)
        {
            var content = new ResponseContent<Customers>(null);
            if (customer != null)
            {
                using (var customerBusiness = new CustomersBusiness())
                {
                    content.Result = customerBusiness.InsertCustomer(customer) ? "1" : "0";

                    return new StandartResult<Customers>(content, Request);
                }
            }

            content.Result = "0";

            return new StandartResult<Customers>(content, Request);
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(int id, [FromBody]Customers customer)
        {
            var content = new ResponseContent<Customers>(null);

            if (customer != null)
            {
                using (var customerBusiness = new CustomersBusiness())
                {
                    content.Result = customerBusiness.UpdateCustomer(customer) ? "1" : "0";

                    return new StandartResult<Customers>(content, Request);
                }
            }

            content.Result = "0";

            return new StandartResult<Customers>(content, Request);
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            var content = new ResponseContent<Customers>(null);

            using (var customerBusiness = new CustomersBusiness())
            {
                content.Result = customerBusiness.DeleteCustomerById(id) ? "1" : "0";

                return new StandartResult<Customers>(content, Request);
            }
        }
    }
}