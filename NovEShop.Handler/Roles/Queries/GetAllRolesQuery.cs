using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NovEShop.Data.Models.Commons;
using NovEShop.Handler.Commons;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Roles.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NovEShop.Handler.Roles.Queries
{
    public class GetAllRolesQuery : IQuery<GetAllRolesQueryResponse>
    {
        public string TokenAuth { get; set; }
    }

    public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, GetAllRolesQueryResponse>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public GetAllRolesQueryHandler(
            RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<GetAllRolesQueryResponse> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAllRolesQueryResponse();

            var roles = await _roleManager.Roles.Select(x => new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name
            })
                .ToListAsync();

            response.Data = roles;
            response.Message = "Lấy dữ liệu Roles thành công";
            response.IsSucceed = true;

            return response;
        }
    }

    public class GetAllRolesQueryResponse : Response<List<RoleViewModel>>
    {

    }
}
