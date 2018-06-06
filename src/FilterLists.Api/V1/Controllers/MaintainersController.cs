﻿using System.Threading.Tasks;
using FilterLists.Data.Entities;
using FilterLists.Services.Seed;
using FilterLists.Services.Seed.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilterLists.Api.V1.Controllers
{
    public class MaintainersController : BaseController
    {
        public MaintainersController(SeedService seedService) : base(seedService)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Seed() => Json(await SeedService.GetAllAsync<Maintainer, MaintainerSeedDto>());
    }
}