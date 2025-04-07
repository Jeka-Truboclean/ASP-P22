using ASP_P22.Data;
using ASP_P22.Data.Entities;
using ASP_P22.Middleware.Auth;
using ASP_P22.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Claims;

namespace ASP_P22.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ApiCartController(DataAccessor dataAccessor) : ControllerBase
    {
        private readonly DataAccessor _dataAccessor = dataAccessor;

        RestResponseModel restResponseModel => new()
            {
                CacheLifetime = 0,
                Description = "User's cart API: Active cart",
                Manipulations = new()
                {
                    Read = "/api/cart",
                },
                Meta = new() {
                    { "locale", "uk" },
                    { "dataType", "object" }
                },
            };

        [HttpGet]
        public RestResponseModel DoGet(String? id)
        {
            var res = restResponseModel;

            String? userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            if(userId == null)
            {
                res.Status = new() { Code=401, IsSuccess=false, Phrase="Unauthorized" };
                res.Data = HttpContext.Items[nameof(AuthTokenMiddleware)];
                return res;
            }
            Cart? cart;
            try { 
                cart = _dataAccessor.GetCartInfo(userId, id); 
            }
            catch (AccessViolationException ex) {
                res.Status = new() { Code = 403, IsSuccess = false, Phrase = "Forbidden" };
                res.Data = ex.Message;
                return res;
            }
            catch (Exception ex)
            {
                res.Status = new() { Code = 400, IsSuccess = false, Phrase = "Bad Request" };
                res.Data = ex.Message;
                return res;
            }
            res.Data = cart;
            return res;
        }

        [HttpPost]
        public RestResponseModel DoPost([FromQuery]String productId)
        {
            var res = restResponseModel;
            String? userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            if (userId == null)
            {
                res.Status = new() { Code = 401, IsSuccess = false, Phrase = "Unauthorized" };
                res.Data = HttpContext.Items[nameof(AuthTokenMiddleware)];
                return res;
            }
            try
            {
                _dataAccessor.AddToCart(userId, productId);
                res.Data = "Created";
            }
            catch (Exception ex) 
            {
                res.Status = new() { Code = 400, IsSuccess = false, Phrase = "Bad request" };
                res.Data = ex.Message;
            }
            return res;
        }

        [HttpPatch]
        public RestResponseModel DoPatch([FromRoute] String id, [FromQuery] int delta)
        {
            var res = restResponseModel;
            String? userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            if (userId == null)
            {
                res.Status = new() { Code = 401, IsSuccess = false, Phrase = "Unauthorized" };
                res.Data = HttpContext.Items[nameof(AuthTokenMiddleware)];
                return res;
            }

            try
            {
                // Перевірка, що CartDetail належить авторизованому користувачу
                Guid cartDetailId = Guid.Parse(id);
                var cartDetail = _dataAccessor
                    .DataContext
                    .CartDetails
                    .Include(cd => cd.Cart)
                    .FirstOrDefault(cd => cd.Id == cartDetailId);

                if (cartDetail == null)
                {
                    throw new Win32Exception(404, "CartDetail not found");
                }

                if (cartDetail.Cart.UserId.ToString() != userId)
                {
                    throw new Win32Exception(403, "Access denied: CartDetail does not belong to current user");
                }

                // Зміна кількості товару в кошику
                _dataAccessor.ModifyCart(id, delta, userId);
                res.Data = _dataAccessor.GetCartInfo(userId, null);
            }
            catch (Win32Exception ex)
            {
                res.Status = new() { Code = ex.ErrorCode, IsSuccess = false, Phrase = ex.Message };
            }
            catch (Exception ex)
            {
                res.Status = new() { Code = 400, IsSuccess = false, Phrase = ex.Message };
            }
            return res;
        }

    }
}
