

namespace Products.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Products.Models;
    using System.Linq;
    using Xamarin.Forms;
    using System.IO;
    using Plugin.Media.Abstractions;

    public class ApiServiceWithoutConnection : IApiService
    {
        #region Properties
        static IList<Category> CategoriesList;
        static IList<Customer> customerList;
        #endregion

        #region Constructors
        public ApiServiceWithoutConnection()
        {
            if(CategoriesList == null)
            {
                CategoriesList = new List<Category>()
                    {
                        new Category()
                        {
                            CategoryId = 2,
                            Description = "Aseo Hogar",
                            Products = new List<Product>()
                            {
                                new Product()
                                {
                                    ProductId = 3,
                                    CategoryId = 2,
                                    Description = "Fabuloso",
                                    Price = 5200,
                                    Image = null,
                                    IsActive = true,
                                    Stock = 12,
                                    LastPurchase = new DateTime(2018,08,11),
                                    Remarks = null
                                },
                                new Product()
                                {
                                    ProductId = 5,
                                    CategoryId = 2,
                                    Description = "Tanga de tigre",
                                    Price = 10000,
                                    Image = "tanga_de_tigre.jpeg",
                                    IsActive = true,
                                    Stock = 24,
                                    LastPurchase = new DateTime(2018,08,12),
                                    Remarks = "Sexy"
                                }
                            }

                        },
                        new Category()
                        {
                            CategoryId = 3,
                            Description = "Carnes",


                        },
                        new Category()
                        {
                            CategoryId = 4,
                            Description = "Frutas y Verduras",
                            Products = new List<Product>()
                            {
                                new Product()
                                {
                                    ProductId = 1,
                                    CategoryId = 4,
                                    Description = "Manzana",
                                    Price = 1.34M,
                                    Image = null,
                                    IsActive = true,
                                    Stock = 54,
                                    LastPurchase = new DateTime(2018,09,1),
                                    Remarks = null
                                }

                            }

                        },
                        new Category()
                        {
                            CategoryId = 1,
                            Description = "Lacteos",
                            Products = new List<Product>()
                            {
                                new Product()
                                {
                                    ProductId = 4,
                                    CategoryId = 1,
                                    Description = "Mantequilla Rama",
                                    Price = 3200,
                                    Image = "matequilla_rama.jpeg",
                                    IsActive = true,
                                    Stock = 48,
                                    LastPurchase = new DateTime(2018,09,13),
                                    Remarks = null
                                }

                            }

                        },
                        new Category()
                        {
                            CategoryId = 6,
                            Description = "Lenceria",


                        },
                        new Category()
                        {
                            CategoryId = 5,
                            Description = "Licores",
                            Products = new List<Product>()
                            {
                                new Product()
                                {
                                    ProductId = 2,
                                    CategoryId = 5,
                                    Description = "Aguardiente Antioqueño X 700 ml",
                                    Price = 4000,
                                    Image = "/storage/emulated/0/Android/data/Products.Android/files/Pictures/Sample/527a1ec6-49ce-4c0a-b461-a18a671592e1.jpeg",
                                    IsActive = true,
                                    Stock = 34,
                                    LastPurchase = new DateTime(2018,09,11),
                                    Remarks = "Maluco"
                                }

                            }

                        },
                    };
                customerList = new List<Customer>()
                {
                    new Customer()
                    {
                        CustomerId = 1,
                        CustomerType = 1,
                        Email = "erislandy.cabrales@gmail.com",
                        Password ="123456",
                        Address = "La Habana",
                        FirstName = "Erislandy",
                        LastName = "Cabrales",
                        Phone = "52180586"
                    }
                };

            }
            
        }


        #endregion

        #region Methods
        public async Task<Response> PasswordRecovery(
            string urlBase,
            string servicePrefix,
            string controller,
            string email)
        {
            try
            {
               

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> ChangePassword(
       string urlBase,
       string servicePrefix,
       string controller,
       string tokenType,
       string accessToken,
       ChangePasswordRequest changePasswordRequest)
        {
            try
            {
                var customer = customerList.Where(c => c.Email.ToLower() == changePasswordRequest.Email.ToLower()).FirstOrDefault();
                customer.Password = changePasswordRequest.NewPassword;
               

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");

            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                };

            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok",
            };

        }



        public async Task<TokenResponse> GetToken(
            string urlBase,
            string username,
            string password)
        {
            try

            {
                TokenResponse result;
                var customer = customerList.Where(c => c.Email == username).FirstOrDefault();
                if(customer != null)
                {
                    if(customer.Password == password)
                    {
                        result = new TokenResponse()
                        {
                            AccessToken = "pnVlnGugBc9HLcZbfNXrw2ij6IpyCIAxC_Ohs_XNuZGi75Nj7yxOG40VuBRBhU_hRTCvHJAaetpDIemeqKbBJ1cVXcs1n394uFzVLadvDhit8gityMJt0t50FQpwgKN8EqmoZUhUemRy0OiPevIFnHAlpb-sSR2qJQbUvIBKdT4LxtO00gyNOZOLUyama1cnrkP5ZJ9l8_7NIPEAEf-jtNZXpc3zqhyJeFsfoH8nmQwGPpBsgACn_eRpfzvED4YIR53Dq0waO8auFKUYwvDzyNT4zY309cQRqI3bIe1ZoraViIEqTIQe7mMU4IMDLV4LtiCHuQEgxcHjYXz7zD5LVvdgS9napuEs110lrWlht3OgaoGZYp0HA1bafIXjG28HH-zn_sG2gZlWKr3ipeF8k0W_Alf0hWkb77DWzV7zUydRQVyTVkVIQSpVee2e36LfcjN2k-usplsBfHn6buMz2ZJ4fa2Whbqklaf3le4ubQ6nicLkBTUMocDo4KmAA2L9",
                            TokenType = "bearer",
                            ExpiresIn = "1209599",
                            UserName = "erislandy.cabrales@gmail.com",
                            Issued = new DateTime(2018, 9, 16, 0, 11, 26),
                            Expires = new DateTime(2018, 9, 30, 0, 11, 26)
                        };
                        return result;
                    }
                }
               
                return new TokenResponse()
                {
                    ErrorDescription = "The user name or password is incorrect."
                };
              
               
               
            }
            catch
            {
                return null;
            }
        }

        public async Task<Response> Get<T>(
            string urlBase, 
            string servicePrefix, 
            string controller,
            string tokenType, 
            string accessToken, int id)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, id);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = model,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }
        }

        public async Task<Response> GetCategory(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken, int id)
        {
            try
            {
                var model = CategoriesList.Where(c => c.CategoryId == id).FirstOrDefault();
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = model,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }
        }


        public async Task<Response> GetList<T>(
            string urlBase, 
            string servicePrefix, 
            string controller,
            string tokenType, 
            string accessToken)
        {
            try
            {
                 return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = CategoriesList,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }



        public async Task<Response> GetList<T>(

            string urlBase, string servicePrefix, string controller,

            string tokenType, string accessToken, int id)

        {

            try

            {

                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                client.BaseAddress = new Uri(urlBase);

                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, id);

                var response = await client.GetAsync(url);



                if (!response.IsSuccessStatusCode)

                {

                    return new Response

                    {

                        IsSuccess = false,

                        Message = response.StatusCode.ToString(),

                    };

                }



                var result = await response.Content.ReadAsStringAsync();

                var list = JsonConvert.DeserializeObject<List<T>>(result);

                return new Response

                {

                    IsSuccess = true,

                    Message = "Ok",

                    Result = list,

                };

            }

            catch (Exception ex)

            {

                return new Response

                {

                    IsSuccess = false,

                    Message = ex.Message,

                };

            }

        }



        public async Task<Response> PostCategory(
            string urlBase, 
            string servicePrefix, 
            string controller,
            string tokenType, 
            string accessToken,
            Category model)

        {

            try

            {
                var category =  model;

                if(CategoriesList.Where(c => c.Description == model.Description).FirstOrDefault() != null)
                {
                    return new Response

                    {

                        IsSuccess = false,

                        Message = string.Format("Category {0} is an exist category" ,model.Description),

                        Result = model,

                    };
                }
                category.CategoryId = CategoriesList.Count + 100; 
                CategoriesList.Add(category);

              
                return new Response

                {

                    IsSuccess = true,

                    Message = "Record added OK",

                    Result = model,

                };

            }

            catch (Exception ex)

            {

                return new Response

                {

                    IsSuccess = false,

                    Message = ex.Message,

                };

            }

        }

        public async Task<Response> PostProduct(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            Product model)

        {

            try

            {
                var product = model;
                var category = CategoriesList.Where(c => c.CategoryId == product.CategoryId).FirstOrDefault();
                
                if (category.Products.Where(p => p.Description == product.Description).FirstOrDefault() != null)
                {
                    return new Response

                    {

                        IsSuccess = false,

                        Message = string.Format("Product {0} is an exist product", product.Description),

                        Result = model,

                    };
                }
                product.ProductId = category.Products.Count + 100;

                category.Products.Add(product);


                return new Response

                {

                    IsSuccess = true,

                    Message = "Record added OK",

                    Result = product,

                };

            }

            catch (Exception ex)

            {

                return new Response

                {

                    IsSuccess = false,

                    Message = ex.Message,

                };

            }

        }

        public async Task<Response> PostCustomers(
           string urlBase,
           string servicePrefix,
           string controller,
           Customer model)

        {

            try

            {
               
                if (customerList.Where(c => c.Email == model.Email).FirstOrDefault() != null)
                {
                    return new Response

                    {

                        IsSuccess = false,

                        Message = string.Format("Customer {0} is an exist customer", model.Email),

                        Result = model,

                    };
                }
                model.CustomerId = customerList.Count + 100;

                customerList.Add(model);


                return new Response

                {

                    IsSuccess = true,

                    Message = "Record added OK",

                    Result = model,

                };

            }

            catch (Exception ex)

            {

                return new Response

                {

                    IsSuccess = false,

                    Message = ex.Message,

                };

            }

        }


        public async Task<Response> Post<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {

                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    tokenType,
                    accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
                };
            }

            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }



        public async Task<Response> Post<T>(

            string urlBase, string servicePrefix, string controller, T model)

        {

            try

            {

                var request = JsonConvert.SerializeObject(model);

                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var url = string.Format("{0}{1}", servicePrefix, controller);

                var response = await client.PostAsync(url, content);



                if (!response.IsSuccessStatusCode)

                {

                    return new Response

                    {

                        IsSuccess = false,

                        Message = response.StatusCode.ToString(),

                    };

                }



                var result = await response.Content.ReadAsStringAsync();

                var newRecord = JsonConvert.DeserializeObject<T>(result);



                return new Response

                {

                    IsSuccess = true,

                    Message = "Record added OK",

                    Result = newRecord,

                };

            }

            catch (Exception ex)

            {

                return new Response

                {

                    IsSuccess = false,

                    Message = ex.Message,

                };

            }

        }



        public async Task<Response> Put<T>(

            string urlBase, string servicePrefix, string controller,

            string tokenType, string accessToken, T model)

        {

            try

            {

                var request = JsonConvert.SerializeObject(model);

                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                client.BaseAddress = new Uri(urlBase);

                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, model.GetHashCode());

                var response = await client.PutAsync(url, content);



                if (!response.IsSuccessStatusCode)

                {

                    return new Response

                    {

                        IsSuccess = false,

                        Message = response.StatusCode.ToString(),

                    };

                }



                var result = await response.Content.ReadAsStringAsync();

                var newRecord = JsonConvert.DeserializeObject<T>(result);



                return new Response

                {

                    IsSuccess = true,

                    Message = "Record updated OK",

                    Result = newRecord,

                };

            }

            catch (Exception ex)

            {

                return new Response

                {

                    IsSuccess = false,

                    Message = ex.Message,

                };

            }

        }

        public async Task<Response> PutCategory(

           string urlBase, string servicePrefix, string controller,

           string tokenType, string accessToken, Category model)

        {

            try

            {

                var category = CategoriesList.Where(c => c.CategoryId == model.CategoryId).FirstOrDefault();
                category.Description = model.Description;
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record updated OK",
                    Result = model,
                };
            }

            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }

        }

        public async Task<Response> PutProduct(

          string urlBase, 
          string servicePrefix, 
          string controller,
          string tokenType, 
          string accessToken, 
          Product model)

        {

            try

            {

                var category = CategoriesList.Where(c => c.CategoryId == model.CategoryId).FirstOrDefault();
                var product = category.Products.Where(p => p.ProductId == model.ProductId).FirstOrDefault();
                product.Description = model.Description;
                product.Price = model.Price;
                product.IsActive = model.IsActive;
                product.LastPurchase = model.LastPurchase;
                product.Remarks = model.Remarks;
                product.Stock = model.Stock;
                return new Response
                {
                    IsSuccess = true,
                    Message = "Record updated OK",
                    Result = model,
                };
            }

            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }

        }


        public async Task<Response> Delete<T>(

            string urlBase, string servicePrefix, string controller,

            string tokenType, string accessToken, T model)

        {

            try

            {

                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, model.GetHashCode());

                var response = await client.DeleteAsync(url);



                if (!response.IsSuccessStatusCode)

                {

                    return new Response

                    {

                        IsSuccess = false,

                        Message = response.StatusCode.ToString(),

                    };

                }



                return new Response

                {

                    IsSuccess = true,

                    Message = "Record deleted OK",

                };

            }

            catch (Exception ex)

            {

                return new Response

                {

                    IsSuccess = false,

                    Message = ex.Message,

                };

            }

        }

        public async Task<Response> DeleteCategory(
            string urlBase, 
            string servicePrefix, 
            string controller,
            string tokenType, 
            string accessToken,
            Category model)
        {
            try
            {
                if(model.Products != null)
                {
                    if (model.Products.Count > 0)
                    {
                        return new Response()
                        {
                            IsSuccess = false,
                            Message = "You can't delete this recors, 'cause it has related records."

                        };
                    }
                    
                }
                CategoriesList.Remove(model);

                return new Response()
                {
                    IsSuccess = true
                };
            }

            catch (Exception ex)

            {

                return new Response

                {

                    IsSuccess = false,

                    Message = ex.Message,

                };

            }

        }

        public async Task<Response> DeleteProduct(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            Product model)
        {
            try
            {
                var category = CategoriesList.Where(c => c.CategoryId == model.CategoryId).FirstOrDefault();
                category.Products.Remove(model);
                
                return new Response()
                {
                    IsSuccess = true,
                    Result = category.Products
                };
            }

            catch (Exception ex)

            {

                return new Response

                {

                    IsSuccess = false,

                    Message = ex.Message,

                };

            }

        }

        #endregion

    }
}

