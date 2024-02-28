﻿using Marketplace.Enitities;
using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Base;

namespace Marketplace.Services.Interface
{
    public interface IRolePermissionService
    {
        public Task<List<RolePermission>> GetAll();
    }
}
