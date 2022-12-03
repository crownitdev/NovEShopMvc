using System.Collections.Generic;

namespace NovEShop.Handler.Users.Dtos
{
    public class RoleAssignRequestDto
    {
        public int Id { get; set; }
        public List<RoleSelectItemDto> Roles { get; set; } = new List<RoleSelectItemDto>();
    }
}
